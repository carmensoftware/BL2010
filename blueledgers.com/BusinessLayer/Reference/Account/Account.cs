using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class Account : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public Account()
        {
            SelectCommand = "SELECT * FROM Reference.Account";
            TableName = "Account";
        }

        /// <summary>
        ///     Get account data using account code
        /// </summary>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        public bool GetAccount(DataSet dsAccount, string accountCode, string connStr)
        {
            var result = false;

            // Created parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AccountCode", accountCode);

            // Get data
            result = DbRetrieve("GL.GetAccount", dsAccount, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsAccount"></param>
        /// <param name="accountViewID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public void GetAccountList(DataSet dsAccount, int accountViewID, string connStr)
        {
            //bool result = false;

            //// Paramter value assign to dbparameter array.
            //DbParameter[] dbParams = new DbParameter[1];
            //dbParams[0] = new DbParameter("@Accountviewid", Convert.ToString(accountViewID));

            //// Get data
            //result = DbRetrieve("Reference.GetAccountGenerateColumnList", dsAccount, dbParams, this.TableName, connStr);

            //// Return result
            //return result;

            var dtAccount = new DataTable();
            var account = new Account();

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Accountviewid", Convert.ToString(accountViewID));


            // Get data                
            dtAccount = account.DbRead("Reference.GetAccountGenerateColumnList", dbParams, connStr);

            // Return result
            if (dsAccount.Tables[TableName] != null)
            {
                dsAccount.Tables.Remove(TableName);
            }

            dtAccount.TableName = TableName;
            dsAccount.Tables.Add(dtAccount);
        }

        /// <summary>
        ///     Get account for preview
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
            var field = new Blue.BL.Application.Field();
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
        ///     Get account database structure
        /// </summary>
        /// <param name="dsAccount"></param>
        /// <returns></returns>
        public bool GetAccountStructure(DataSet dsAccount, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("Reference.GetAccountList", dsAccount, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get description account
        /// </summary>
        /// <param name="SectionCode"></param>
        /// <returns></returns>
        public string GetName(string AccountCode, string connStr)
        {
            string accName;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AccountCode", AccountCode);

            DbRetrieve("GL.GetAccount", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                accName = dsTmp.Tables[TableName].Rows[0]["NameEng"].ToString();
            }
            else
            {
                accName = null;
            }

            return accName;
        }

        /// <summary>
        ///     return dataset and get account list
        /// </summary>
        /// <param name="dsAccount"></param>
        public void GetAccountList(DataSet dsAccount, string connStr)
        {
            var account = new Account();
            account.DbRetrieve("Reference.GetAccountList", dsAccount, null, TableName, connStr);
        }

        /// <summary>
        ///     Get account using account view id
        /// </summary>
        /// <param name="dsAccount"></param>
        public void GetAccountList(DataSet dsAccount, int accountViewID, int userID, string connStr)
        {
            var dtAccount = new DataTable();
            var dtAccountViewCriteria = new DataTable();
            var account = new Account();
            var accountViewQuery = string.Empty;

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate query
            accountViewQuery = new AccountView().GetAccountViewQuery(accountViewID, userID, connStr);

            // Generate parameter
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

            // Return resutl
            if (dsAccount.Tables[TableName] != null)
            {
                dsAccount.Tables.Remove(TableName);
            }

            dtAccount.TableName = TableName;
            dsAccount.Tables.Add(dtAccount);
        }

        /// <summary>
        ///     Get account for check category.
        /// </summary>
        /// <param name="dsAccount"></param>
        /// <param name="accountCode"></param>
        /// <param name="connStr"></param>
        public void GetByChkCategory(DataSet dsAccount, string accountCode, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AccountCode", accountCode);

            DbRetrieve("GL.GetAccountByChkCategory", dsAccount, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     return datatable and get searching match data.
        /// </summary>
        /// <param name="Search_Param"></param>
        /// <returns></returns>
        public DataTable GetSearchAccountList(string Search_Param, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Search_Param", Search_Param);

            return DbRead("Reference.GetSearchAccountList", dbParams, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetAccountListLookup(string connStr)
        {
            var dsAccount = new DataSet();

            // Get Data
            DbRetrieve("Reference.GetAccountListLookup", dsAccount, null, TableName, connStr);


            // Return result
            if (dsAccount.Tables[TableName] != null)
            {
                var drBlank = dsAccount.Tables[TableName].NewRow();
                dsAccount.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            // Return result
            return dsAccount.Tables[TableName];
        }

        /// <summary>
        ///     Save to database.
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var accountMisc = new BL.Reference.AccountMisc();
            //BL.Reference.AccountAttachment accountAttachment      = new BL.Reference.AccountAttachment();
            //BL.Reference.AccountComment    accountComment         = new AccountComment();
            //BL.Reference.AccountActiveLog  accountActiveLog       = new AccountActiveLog();

            // Build savesource object
            var dbSaveSource = new DbSaveSource[2];
            //dbSaveSource[0]             = new DbSaveSource(savedData, accountAttachment.SelectCommand, accountAttachment.TableName);
            dbSaveSource[0] = new DbSaveSource(savedData, accountMisc.SelectCommand, accountMisc.TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, SelectCommand, TableName);
            //dbSaveSource[3]             = new DbSaveSource(savedData, accountComment.SelectCommand, accountComment.TableName);
            //dbSaveSource[4]             = new DbSaveSource(savedData, accountActiveLog.SelectCommand, accountActiveLog.TableName);


            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        ///     Save only table reference.account
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool SaveAcc(DataSet savedData, string connStr)
        {
            // Build savesource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet savedData, string connStr)
        {
            var result = false;

            var accountMisc = new BL.Reference.AccountMisc();
            //BL.Reference.AccountAttachment  accountAttachment    = new BL.Reference.AccountAttachment();
            //BL.Reference.AccountComment     accountComment       = new AccountComment();
            //BL.Reference.AccountActiveLog   accountActiveLog     = new AccountActiveLog();


            // Build savesource object
            var dbSaveSource = new DbSaveSource[2];
            //dbSaveSource[0] = new DbSaveSource(savedData, accountAttachment.SelectCommand, accountAttachment.TableName);
            dbSaveSource[0] = new DbSaveSource(savedData, accountMisc.SelectCommand, accountMisc.TableName);
            //dbSaveSource[2] = new DbSaveSource(savedData, accountComment.SelectCommand,accountComment.TableName);
            //dbSaveSource[3] = new DbSaveSource(savedData, accountActiveLog.SelectCommand,accountActiveLog.TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, SelectCommand, TableName);


            // Call dbCommit and send savesource object is parameter
            result = DbCommit(dbSaveSource, connStr);

            return result;
        }

        #endregion
    }
}