using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class AccountView : DbHandler
    {
        #region "Attributies"

        //private string accountCode;
        //private string nameEng;
        //private string nameLocal;
        //private string nature;
        //private int type;
        //private string categoryCode;
        //private string subCategoryCode;
        //private bool isActive;
        //private DateTime createdDate;
        //private int createdBy;
        //private DateTime updatedDate;
        //private int updatedBy;

        //public string AccountCode
        //{
        //    get
        //    {
        //        return accountCode;
        //    }
        //    set
        //    {
        //        accountCode = value;
        //    }
        //}

        //public string NameEng
        //{
        //    get
        //    {
        //        return nameEng;
        //    }
        //    set
        //    {
        //        nameEng = value;
        //    }
        //}

        //public string NameLocal
        //{
        //    get
        //    {
        //        return nameLocal;
        //    }
        //    set
        //    {
        //        nameLocal = value;
        //    }
        //}

        //public string Nature
        //{
        //    get
        //    {
        //        return nature;
        //    }
        //    set
        //    {
        //        nature = value;
        //    }
        //}

        //public string Type
        //{
        //    get
        //    {
        //        return type;
        //    }
        //    set
        //    {
        //        type = value;
        //    }
        //}

        //public string AccountCode
        //{
        //    get
        //    {
        //        return accountCode;
        //    }
        //    set
        //    {
        //        accountCode = value;
        //    }
        //}

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public AccountView()
        {
            this.SelectCommand = "SELECT * FROM Reference.AccountView";
            this.TableName = "AccountView";
        }

        /// <summary>
        ///     Get account view schema
        /// </summary>
        /// <param name="accountViewID"></param>
        /// <returns></returns>
        public bool GetAccountViewSchema(DataSet accountView, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("Reference.GetAccountViewList", accountView, null, this.TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get account view using account view id
        /// </summary>
        /// <param name="accountViewID"></param>
        /// <returns></returns>
        public bool GetAccountView(int accountViewID, DataSet accountView, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@AccountViewID", accountViewID.ToString());

            // Get data
            result = DbRetrieve("Reference.GetAccountView", accountView, dbParams, this.TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get account view using account view id
        /// </summary>
        /// <param name="accountViewID"></param>
        /// <returns></returns>
        public DataTable GetAccountView(int accountViewID, string connStr)
        {
            var accountView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@AccountViewID", accountViewID.ToString());

            // Get data
            accountView = DbRead("Reference.GetAccountView", dbParams, connStr);

            // Return result
            return accountView;
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
            dtAccountView = DbRead("Reference.GetAccountViewListByCreatedBy", dbParams, connStr);

            // Return result
            return dtAccountView;
        }

        /// <summary>
        ///     Generate query of assigned AccountViewID
        /// </summary>
        /// <returns></returns>
        public string GetAccountViewQuery(int accountViewID, int userID, string connStr)
        {
            var accountViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            columnList = AccountViewColumn.GetAccountViewColumn(accountViewID, connStr);

            // Generate where clause
            whereClause = AccountViewCriteria.GetAccountViewCriteria(accountViewID, userID, connStr);

            // Generate query
            accountViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                               " AccountCode FROM Reference.vAccount " +
                               (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return accountViewQuery;
        }

        /// <summary>
        ///     Get lastest account view id
        /// </summary>
        /// <returns></returns>
        public int GetAccountViewMaxID(string connStr)
        {
            var maxAccountViewID = 0;

            // Get data
            maxAccountViewID = DbReadScalar("Reference.GetAccountViewMaxID", null, connStr);

            // Return result
            return maxAccountViewID;
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
            //accountViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
            //                   " AccountCode FROM Reference.vAccount " +
            //                   (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);
            accountViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                               " AccountCode FROM Reference.vAccountList " +
                               (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return accountViewQuery;
        }

        /// <summary>
        ///     Validate query before save
        /// </summary>
        /// <returns></returns>
        public bool GetAccountViewIsValidQuery(DataTable accountView, DataTable accountViewCriteria,
            DataTable accountViewColumn, string connStr)
        {
            var accountViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;
            var result = new DataTable();
            var dbParams = new DbParameter[accountViewCriteria.DefaultView.Count];
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();


            // Generate Column
            if (accountViewColumn != null)
            {
                foreach (DataRow dr in accountViewColumn.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        columnList += (columnList != string.Empty ? "," : string.Empty) +
                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);
                    }
                }
            }

            // Generate Criteria
            if (accountViewCriteria != null)
            {
                // Non-Advance option
                if (accountView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in accountViewCriteria.Rows)
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
                    whereClause = accountView.Rows[0]["AdvanceOption"].ToString();

                    foreach (DataRow dr in accountViewCriteria.Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                                  dr["Operator"] + " " + "@" + dr["SeqNo"] +
                                                  field.GetFieldName(dr["FieldID"].ToString(), connStr);
                            whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                        }
                    }
                }

                // Generate parameter
                if (accountViewCriteria.Rows.Count > 0)
                {
                    foreach (DataRow dr in accountViewCriteria.Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            dbParams[(int) dr["SeqNo"] - 1] =
                                new DbParameter(
                                    "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                                    dr["Value"].ToString());
                        }
                    }
                }
                else
                {
                    dbParams = null;
                }
            }

            // Generate query
            accountViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                               " AccountCode FROM Reference.vAccountList " +
                               (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            // Vlidate query
            result = DbExecuteQuery(accountViewQuery, dbParams, connStr);

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
            var accountViewCriteria = new AccountViewCriteria();
            var accountViewColumn = new AccountViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(savedData, this.SelectCommand, this.TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, accountViewCriteria.SelectCommand,
                accountViewCriteria.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, accountViewColumn.SelectCommand, accountViewColumn.TableName);

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
            var accountViewCriteria = new AccountViewCriteria();
            var accountViewColumn = new AccountViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(deletedData, accountViewCriteria.SelectCommand,
                accountViewCriteria.TableName);
            dbSaveSource[1] = new DbSaveSource(deletedData, accountViewColumn.SelectCommand, accountViewColumn.TableName);
            dbSaveSource[2] = new DbSaveSource(deletedData, this.SelectCommand, this.TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}