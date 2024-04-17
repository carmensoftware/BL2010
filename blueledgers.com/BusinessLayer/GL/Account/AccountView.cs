using System;
using System.Data;
using System.Text;
using Blue.DAL;

namespace Blue.BL.GL.Account
{
    public class AccountView : DbHandler
    {
        #region "Attributies"

        private readonly AccountViewColumn _accViewCols = new AccountViewColumn();
        private readonly AccountViewCriteria _accViewCrtr = new AccountViewCriteria();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public AccountView()
        {
            SelectCommand = "SELECT * FROM GL.AccountView";
            TableName = "AccountView";
        }

        /// <summary>
        ///     Get account view data related to specified view id.
        /// </summary>
        /// <param name="dsAccView"></param>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Get(DataSet dsAccView, int intViewID, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewID", intViewID.ToString());

            return DbRetrieve("GL.GetAccView_ViewID", dsAccView, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get advance criteria option.
        /// </summary>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetIsAdvance(int intViewID, string strConn)
        {
            var dsAccView = new DataSet();

            var result = Get(dsAccView, intViewID, strConn);

            if (result)
            {
                if (dsAccView.Tables[TableName].Rows.Count > 0)
                {
                    return bool.Parse(dsAccView.Tables[TableName].Rows[0]["IsAdvance"].ToString());
                }
                return false;
            }
            return false;
        }

        /// <summary>
        ///     Get creteria in advance option.
        /// </summary>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetAdvOpt(int intViewID, string strConn)
        {
            var dsAccView = new DataSet();

            var result = Get(dsAccView, intViewID, strConn);

            if (result)
            {
                return dsAccView.Tables[TableName].Rows[0]["AdvOpt"].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get criteria search scope which is all account or only the account was created by the user ralated to specified
        ///     view id.
        /// </summary>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetSearchIn(int intViewID, string strConn)
        {
            var dsAccView = new DataSet();

            var result = Get(dsAccView, intViewID, strConn);

            if (result)
            {
                if (dsAccView.Tables[TableName].Rows.Count > 0)
                {
                    return dsAccView.Tables[TableName].Rows[0]["SearchIn"].ToString();
                }
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get the view modification permission.
        /// </summary>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetIsStandard(int intViewID, string strConn)
        {
            var dsAccView = new DataSet();

            var result = Get(dsAccView, intViewID, strConn);

            if (result)
            {
                return bool.Parse(dsAccView.Tables[TableName].Rows[0]["IsStandard"].ToString());
            }
            return false;
        }

        /// <summary>
        ///     Get account view table structure.
        /// </summary>
        /// <param name="dsAccView"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsAccView, string strConn)
        {
            return DbRetrieveSchema("GL.GetAccViewList", dsAccView, null, TableName, strConn);
        }

        /// <summary>
        ///     Get account view list ralated to logged in user name.
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public DataTable GetList(string strUserName, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CreatedBy", strUserName);
            return DbRead("GL.GetAccViewList_IsPublic_CreatedBy", dbParams, strConn);
        }

        /// <summary>
        ///     Get SQL query string related sepcified view id.
        ///     The query was generated from view column and view criteria data.
        /// </summary>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetQuery(int intViewID, ref DbParameter[] dbParams, string strUserName, string strConn)
        {
            var sbQuery = new StringBuilder();

            // Initial query string.
            sbQuery.Append("SELECT ");

            // Add column list to query.
            sbQuery.Append(_accViewCols.GetColumn(intViewID, strConn));

            // Add table name to query
            sbQuery.Append(" FROM GL.vAccountList ");

            // Add where clause to query
            if (GetIsAdvance(intViewID, strConn))
            {
                sbQuery.Append(_accViewCrtr.GetAdvanceCriteria(intViewID, ref dbParams, GetAdvOpt(intViewID, strConn),
                    strConn));
            }
            else
            {
                sbQuery.Append(_accViewCrtr.GetCriteria(intViewID, ref dbParams, strConn));
            }

            // Add search scope criteria.
            // Search in all account or only account was created by the user.
            if (GetSearchIn(intViewID, strConn).ToUpper() != "A")
            {
                // If this query already has no where clause.
                // Add "where" before add the search scope criteria.
                if (sbQuery.ToString().IndexOf("WHERE") < 0)
                {
                    sbQuery.Append(" WHERE CreatedBy=@CreatedBy");
                }
                else
                {
                    sbQuery.Append(" AND CreatedBy=@CreatedBy");
                }

                // Add parameter.
                Array.Resize(ref dbParams, dbParams.Length + 1);
                dbParams[dbParams.Length - 1] = new DbParameter("@CreatedBy", strUserName);
            }

            return sbQuery.ToString();
        }

        /// <summary>
        ///     Get new id.
        /// </summary>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public int GetNewID(string strConn)
        {
            return DbReadScalar("GL.GetAccView_NewID", null, strConn);
        }

        /// <summary>
        ///     Save account view data changed to database.
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var accViewCrtr = new AccountViewCriteria();
            var accViewCol = new AccountViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, accViewCrtr.SelectCommand, accViewCrtr.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, accViewCol.SelectCommand, accViewCol.TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        ///     Commit to data base for delete
        /// </summary>
        /// <param name="deletedData"></param>
        /// <returns></returns>
        public bool Delete(DataSet deletedData, string connStr)
        {
            var accViewCrtr = new AccountViewCriteria();
            var accViewCol = new AccountViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(deletedData, accViewCrtr.SelectCommand, accViewCrtr.TableName);
            dbSaveSource[1] = new DbSaveSource(deletedData, accViewCol.SelectCommand, accViewCol.TableName);
            dbSaveSource[2] = new DbSaveSource(deletedData, SelectCommand, TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        /// Get account view schema
        /// </summary>
        /// <param name="accountViewID"></param>
        /// <returns></returns>
        //public bool GetAccountViewSchema(DataSet accountView, string connStr)
        //{
        //    bool result = false;

        //    // Get data
        //    result = DbRetrieveSchema("Reference.GetAccountViewList", accountView, null, this.TableName, connStr);

        //    // Return result
        //    return result;
        //}

        /// <summary>
        /// Get account view using account view id
        /// </summary>
        /// <param name="accountViewID"></param>
        /// <returns></returns>
        //public bool GetAccountView(int accountViewID, DataSet accountView, string connStr)
        //{
        //    bool result            = false;
        //    DbParameter[] dbParams = new DbParameter[1];

        //    // Create Parameter
        //    dbParams[0] = new DbParameter("@AccountViewID", accountViewID.ToString());

        //    // Get data
        //    result = DbRetrieve("Reference.GetAccountView", accountView, dbParams, this.TableName, connStr);

        //    // Return result
        //    return result;
        //}

        /// <summary>
        /// Get account view using account view id
        /// </summary>
        /// <param name="accountViewID"></param>
        /// <returns></returns>
        //public DataTable GetAccountView(int accountViewID, string connStr)
        //{
        //    DataTable accountView = new DataTable();
        //    DbParameter[] dbParams = new DbParameter[1];

        //    // Create Parameter
        //    dbParams[0] = new DbParameter("@AccountViewID", accountViewID.ToString());

        //    // Get data
        //    accountView = DbRead("Reference.GetAccountView", dbParams, connStr);

        //    // Return result
        //    return accountView;
        //}

        /// <summary>
        /// Get all account view depend on user id.
        /// </summary>
        /// <returns></returns>
        //public DataTable GetAccountViewList(int userID, string connStr)
        //{
        //    DataTable dtAccountView = new DataTable();
        //    DbParameter[] dbParams = new DbParameter[1];

        //    // Create Parameter
        //    dbParams[0] = new DbParameter("@CreatedBy",userID.ToString());

        //    // Get data
        //    dtAccountView = DbRead("Reference.GetAccountViewListByCreatedBy",dbParams, connStr);

        //    // Return result
        //    return dtAccountView;        
        //}

        /// <summary>
        /// Generate query of assigned AccountViewID
        /// </summary>
        /// <returns></returns>
        //public string GetAccountViewQuery(int accountViewID, int userID, string connStr)
        //{
        //    string accountViewQuery = string.Empty;
        //    string columnList = string.Empty;
        //    string whereClause = string.Empty;

        //    // Generate columns
        //    columnList = AccountViewColumn.GetAccountViewColumn(accountViewID, connStr);

        //    // Generate where clause
        //    whereClause = AccountViewCriteria.GetAccountViewCriteria(accountViewID, userID, connStr);

        //    // Generate query
        //    accountViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) + 
        //        " AccountCode FROM Reference.vAccount " +
        //        (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

        //    return accountViewQuery;
        //}

        /// <summary>
        /// Get lastest account view id
        /// </summary>
        /// <returns></returns>
        //public int GetAccountViewMaxID(string connStr)
        //{
        //    int maxAccountViewID = 0;

        //    // Get data
        //    maxAccountViewID = DbReadScalar("Reference.GetAccountViewMaxID", null, connStr);

        //    // Return result
        //    return maxAccountViewID;
        //}

        /// <summary>
        /// Generate query of assigned dataset
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        //public string GetAccountViewQueryPreview(DataSet dsPreview, int userID, string connStr)
        //{
        //    string accountViewQuery = string.Empty;
        //    string columnList       = string.Empty;
        //    string whereClause      = string.Empty;

        //    // Generate columns
        //    if (dsPreview.Tables["AccountViewColumn"] != null)
        //    {
        //        columnList = AccountViewColumn.GetAccountViewColumnPreview(dsPreview.Tables["AccountViewColumn"], connStr);
        //    }

        //    // Generate where clause
        //    if (dsPreview.Tables["AccountViewCriteria"] != null)
        //    {
        //        whereClause = AccountViewCriteria.GetAccountViewCriteriaPreview(dsPreview, userID, connStr);
        //    }

        //    // Generate query
        //    //accountViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
        //    //                   " AccountCode FROM Reference.vAccount " +
        //    //                   (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);
        //    accountViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
        //                       " AccountCode FROM Reference.vAccountList " +
        //                       (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

        //    return accountViewQuery;
        //}

        /// <summary>
        /// Validate query before save
        /// </summary>
        /// <returns></returns>
        //public bool GetAccountViewIsValidQuery(DataTable accountView, DataTable accountViewCriteria, DataTable accountViewColumn, string connStr)
        //{
        //    string accountViewQuery               = string.Empty;
        //    string columnList                     = string.Empty;
        //    string whereClause                    = string.Empty;
        //    DataTable result                      = new DataTable();
        //    DbParameter[] dbParams                = new DbParameter[accountViewCriteria.DefaultView.Count];
        //    BlueLedger.BL.Application.Field field = new BlueLedger.BL.Application.Field();
        //    // Generate Column
        //    if (accountViewColumn != null)
        //    {
        //        foreach (DataRow dr in accountViewColumn.Rows)
        //        {
        //            if (dr.RowState != DataRowState.Deleted)
        //            {
        //                columnList += (columnList != string.Empty ? "," : string.Empty) + field.GetFieldName(dr["FieldID"].ToString(), connStr);
        //            }
        //        }
        //    }

        //    // Generate Criteria
        //    if (accountViewCriteria != null)
        //    {   
        //        // Non-Advance option
        //        if (accountView.Rows[0]["IsAdvance"].ToString() != "True")
        //        {
        //            foreach (DataRow dr in accountViewCriteria.Rows)
        //            {
        //                if (dr.RowState != DataRowState.Deleted)
        //                {
        //                    whereClause += (whereClause != string.Empty ? " " : string.Empty) +
        //                       field.GetFieldName(dr["FieldID"].ToString(), connStr) +" " + dr["Operator"] + " " +
        //                       "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr)+ " " + dr["LogicalOp"];
        //                }
        //            }
        //        }
        //        // Advance option
        //        else
        //        {
        //            whereClause = accountView.Rows[0]["AdvanceOption"].ToString();

        //            foreach (DataRow dr in accountViewCriteria.Rows)
        //            {
        //                if (dr.RowState != DataRowState.Deleted)
        //                {
        //                    string eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " + dr["Operator"] + " " + "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(),connStr);
        //                    whereClause            = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
        //                }
        //            }
        //        }

        //        // Generate parameter
        //        if (accountViewCriteria.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in accountViewCriteria.Rows)
        //            {
        //                if (dr.RowState != DataRowState.Deleted)
        //                {
        //                    dbParams[(int)dr["SeqNo"] - 1] = new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr), dr["Value"].ToString());
        //                }
        //            }
        //        }
        //        else
        //        {
        //            dbParams = null;                   
        //        }
        //    }

        //    // Generate query
        //    accountViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) + 
        //        " AccountCode FROM Reference.vAccountList " +
        //        (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

        //    // Vlidate query
        //    result = DbExecuteQuery(accountViewQuery, dbParams, connStr);

        //    // Return result
        //    if (result != null)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }            
        //}       

        #endregion
    }
}