using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.Setup.Application
{
    public class AccountView : DbHandler
    {
        #region "Attributies"

        public int AccountViewID{get;set;}

        public string Description { get; set; }

        public bool IsPublic { get; set; }

        public bool IsAll { get; set; }

        public string IsAdvance { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        #endregion

        #region "Operations"

        public AccountView()
        {
            SelectCommand = "SELECT * FROM [Application].AccountView";
            TableName = "AccountView";
        }

        /// <summary>
        ///     Get all account view depend on user id.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAccountViewList(int userID, string connStr)
        {
            var dtAccountView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@CreatedBy", userID.ToString());

            // Get data
            dtAccountView = DbRead("[Application].GetAccountViewListByCreatedBy", dbParams, connStr);

            // Return result
            return dtAccountView;
        }

        /// <summary>
        ///     Get Account view using Account view id
        /// </summary>
        /// <param name="AccountViewID"></param>
        /// <returns></returns>
        public DataTable GetAccountView(int AccountViewID, string connStr)
        {
            var AccountView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@AccountViewID", AccountViewID.ToString());

            // Get data
            AccountView = DbRead("[Application].GetAccountView", dbParams, connStr);

            // Return result
            return AccountView;
        }

        /// <summary>
        ///     Get Account view using Account view id
        /// </summary>
        /// <param name="AccountViewID"></param>
        /// <returns></returns>
        public bool GetAccountView(int AccountViewID, DataSet dsAccountView, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@AccountViewID", AccountViewID.ToString());

            // Get data
            result = DbRetrieve("[Application].GetAccountView", dsAccountView, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Generate query of assigned dataset
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetAccountViewQueryPreview(DataSet dsPreview, int userID, string connStr)
        {
            var accountViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            if (dsPreview.Tables["AccountViewColumn"] != null)
            {
                columnList = AccountViewColumn.GetAccountViewColumnPreview(dsPreview.Tables["AccountViewColumn"],
                    connStr);
            }

            // Generate where clause
            if (dsPreview.Tables["AccountViewCriteria"] != null)
            {
                whereClause = AccountViewCriteria.GetAccountViewCriteriaPreview(dsPreview, userID, connStr);
            }

            // Generate query
            accountViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                               " AccountCode FROM Application.vAccount " +
                               (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return accountViewQuery;
        }


        /// <summary>
        ///     Generate query of assigned AccountViewID
        /// </summary>
        /// <returns></returns>
        public string GetAccountViewQuery(int AccountViewID, int userID, string connStr)
        {
            var AccountViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            columnList = AccountViewColumn.GetAccountViewColumn(AccountViewID, connStr);

            // Generate where clause
            whereClause = AccountViewCriteria.GetAccountViewCriteria(AccountViewID, userID, connStr);

            // Generate query
            AccountViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                               " AccountCode FROM [Application].vAccount " +
                               (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return AccountViewQuery;
        }

        /// <summary>
        ///     Get Account view schema
        /// </summary>
        /// <param name="AccountView"></param>
        /// <returns></returns>
        public bool GetAccountViewSchema(DataSet AccountView, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("[Application].GetAccountViewList", AccountView, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get lastest Account view id
        /// </summary>
        /// <returns></returns>
        public int GetMaxAccountViewID(string connStr)
        {
            var maxAccountViewID = 0;

            // Get data
            maxAccountViewID = DbReadScalar("[Application].GetAccountViewMaxID", null, connStr);

            // Return result
            return maxAccountViewID;
        }

        /// <summary>
        ///     Validate query before save
        /// </summary>
        /// <returns></returns>
        public bool GetAccountViewIsValidQuery(DataTable AccountView, DataTable AccountViewCriteria,
            DataTable AccountViewColumn, string connStr)
        {
            var AccountViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;
            var result = new DataTable();
            var dbParams = new DbParameter[AccountViewCriteria.DefaultView.Count];

            var field = new BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate Column
            if (AccountViewColumn != null)
            {
                foreach (DataRow dr in AccountViewColumn.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        columnList += (columnList != string.Empty ? "," : string.Empty) +
                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);
                    }
                }
            }

            // Generate Criteria
            if (AccountViewCriteria != null)
            {
                // Non-Advance option
                if (AccountView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in AccountViewCriteria.Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            whereClause += (whereClause != string.Empty ? " " : string.Empty) +
                                           field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " + dr["Operator"] +
                                           " " +
                                           "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr) +
                                           " " + dr["LogicalOp"];
                        }
                    }
                }
                    // Advance option
                else
                {
                    whereClause = AccountView.Rows[0]["AdvanceOption"].ToString();

                    foreach (DataRow dr in AccountViewCriteria.Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                                  dr["Operator"] + " " +
                                                  "@" + dr["SeqNo"] +
                                                  field.GetFieldName(dr["FieldID"].ToString(), connStr);
                            whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                        }
                    }
                }

                // Generate parameter
                if (AccountViewCriteria.Rows.Count > 0)
                {
                    foreach (DataRow dr in AccountViewCriteria.Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            var paramName = "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr);
                            var paramValue = dr["Value"].ToString();

                            dbParams[(int) dr["SeqNo"] - 1] = new DbParameter(paramName, paramValue);
                        }
                    }
                }
                else
                {
                    dbParams = null;
                }
            }

            // Generate query
            AccountViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                               " AccountCode FROM [Application].vAccount " +
                               (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            // Validate query
            result = DbExecuteQuery(AccountViewQuery, dbParams, connStr);

            // Return result
            if (result != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Commit to data base for save
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var AccountViewCriteria = new AccountViewCriteria();
            var AccountViewColumn = new AccountViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, AccountViewCriteria.SelectCommand,
                AccountViewCriteria.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, AccountViewColumn.SelectCommand, AccountViewColumn.TableName);

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
            var AccountViewCriteria = new AccountViewCriteria();
            var AccountViewColumn = new AccountViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(deletedData, AccountViewCriteria.SelectCommand,
                AccountViewCriteria.TableName);
            dbSaveSource[1] = new DbSaveSource(deletedData, AccountViewColumn.SelectCommand, AccountViewColumn.TableName);
            dbSaveSource[2] = new DbSaveSource(deletedData, SelectCommand, TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}