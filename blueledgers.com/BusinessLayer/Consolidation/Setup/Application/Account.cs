using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.Setup.Application
{
    public class Account : DbHandler
    {
        /// <summary>
        ///     Empty constructor
        /// </summary>
        public Account()
        {
            SelectCommand = "SELECT * FROM [Application].Account";
            TableName = "Account";
        }

        /// <summary>
        ///     Get Account data by using AccountCode.
        /// </summary>
        /// <param name="dsAccount"></param>
        /// <param name="accountCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(ref DataSet dsAccount, string accountCode, string connStr)
        {
            var result = false;

            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@accountCode", accountCode);

            // Get data
            result = DbRetrieve("[Application].GetAccountByAccountCode", dsAccount, dbParams, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get all account data using account view id
        /// </summary>
        /// <param name="dsAccount"></param>
        /// <param name="accountViewID"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetList(DataSet dsAccount, int accountViewID, int userID, string connStr)
        {
            var dtAccount = new DataTable();
            var dtAccountViewCriteria = new DataTable();
            var account = new Account();
            var accountViewQuery = string.Empty;
            var field = new BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate  query
            accountViewQuery = new AccountView().GetAccountViewQuery(accountViewID, userID, connStr);

            // Generate  parameter
            dtAccountViewCriteria = new AccountViewCriteria().GetAccountViewCriteriaList(accountViewID, connStr);

            if (dtAccountViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtAccountViewCriteria.Rows.Count];

                for (var i = 0; i < dtAccountViewCriteria.Rows.Count; i++)
                {
                    var dr = dtAccountViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtAccount = account.DbExecuteQuery(accountViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtAccount = account.DbExecuteQuery(accountViewQuery, null, connStr);
            }

            // Return result
            if (dsAccount.Tables[TableName] != null)
            {
                dsAccount.Tables.Remove(TableName);
            }

            dtAccount.TableName = TableName;
            dsAccount.Tables.Add(dtAccount);
        }

        /// <summary>
        ///     Get all account data.
        /// </summary>
        /// <param name="dsAccount"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(ref DataSet dsAccount, string connStr)
        {
            return DbRetrieve("[Application].GetAccountList", dsAccount, null, TableName, connStr);
        }

        /// <summary>
        ///     Get all active account data.
        /// </summary>
        /// <param name="dsAccount"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(ref DataSet dsAccount, string chkActive, string connStr)
        {
            var result = false;

            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@IsActive", chkActive.ToLower());

            // Get data
            result = DbRetrieve("[Application].GetAccountListIsActive", dsAccount, dbParams, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get Account for preview.
        /// </summary>
        /// <param name="dsAccount"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetAccountPreveiw(DataSet dsAccount, int userID, string connStr)
        {
            var dtAccount = new DataTable();
            var dtAccountViewCriteria = new DataTable();
            var account = new Account();
            var accountViewQuery = string.Empty;
            var field = new BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate  query
            accountViewQuery = new AccountView().GetAccountViewQueryPreview(dsAccount, userID, connStr);

            // Generate  parameter
            dtAccountViewCriteria = dsAccount.Tables["AccountViewCriteria"];

            if (dtAccountViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtAccountViewCriteria.Rows.Count];

                for (var i = 0; i < dtAccountViewCriteria.Rows.Count; i++)
                {
                    var dr = dtAccountViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtAccount = account.DbExecuteQuery(accountViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtAccount = account.DbExecuteQuery(accountViewQuery, null, connStr);
            }

            // Return result
            if (dsAccount.Tables[TableName] != null)
            {
                dsAccount.Tables.Remove(TableName);
            }

            dtAccount.TableName = TableName;
            dsAccount.Tables.Add(dtAccount);
        }

        /// <summary>
        ///     Get account table schema.
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="conStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsAccount, string conStr)
        {
            return DbRetrieveSchema("[Application].GetAccountList", dsAccount, null, TableName, conStr);
        }

        /// <summary>
        ///     Get DisplayText1 of specified AccountCode
        /// </summary>
        /// <param name="accountCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetDisplayText1(string accountCode, string connStr)
        {
            var result = string.Empty;
            var dtTmp = new DataTable();

            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@accountCode", accountCode);

            // Get data
            dtTmp = DbRead("[Application].GetAccountName", dbParams, connStr);

            // return result
            if (dtTmp != null)
            {
                if (dtTmp.Rows.Count > 0)
                {
                    result = dtTmp.Rows[0]["DisplayText1"].ToString();
                }
                else
                {
                    result = string.Empty;
                }
            }

            return result;
        }

        /// <summary>
        ///     Get DisplayText2 of specified AccountCode
        /// </summary>
        /// <param name="accountCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetDisplayText2(string accountCode, string connStr)
        {
            var result = string.Empty;
            var dtTmp = new DataTable();

            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@accountCode", accountCode);

            // Get data
            dtTmp = DbRead("[Application].GetAccountName", dbParams, connStr);

            // return result
            if (dtTmp != null)
            {
                if (dtTmp.Rows.Count > 0)
                {
                    result = dtTmp.Rows[0]["DisplayText2"].ToString();
                }
                else
                {
                    result = string.Empty;
                }
            }

            return result;
        }

        /// <summary>
        ///     Commit changed Account data to DataBase
        /// </summary>
        /// <param name="dsAccount"></param>
        /// <param name="dbSaveSource"></param>
        /// <returns></returns>
        public bool Save(DataSet dsAccount, string connStr)
        {
            var accountAttachment = new AccountAttachment();

            var result = false;
            var dbSaveSource = new DbSaveSource[2];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsAccount, accountAttachment.SelectCommand, accountAttachment.TableName);
            dbSaveSource[1] = new DbSaveSource(dsAccount, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }
    }
}