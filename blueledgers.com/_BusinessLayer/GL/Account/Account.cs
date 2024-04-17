using System.Data;
using Blue.DAL;

namespace Blue.BL.GL.Account
{
    public class Account : DbHandler
    {
        #region "Attributies"

        private readonly AccountView accView = new AccountView();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public Account()
        {
            SelectCommand = "SELECT * FROM GL.Account";
            TableName = "Account";
        }


        public string GetName(string strAccCode, string connStr)
        {
            // Create parameters
            var strName = string.Empty;
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AccCode", strAccCode);

            var drUnit = DbRead("dbo.Acc_GetName", dbParams, connStr);

            if (drUnit.Rows.Count > 0)
            {
                strName = drUnit.Rows[0]["Desc"].ToString();
            }

            return strName;
        }

        /// <summary>
        ///     Get account data related to specified account code
        /// </summary>
        /// <param name="dsAcc"></param>
        /// <param name="strAccCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Get(DataSet dsAcc, string strAccCode, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AccCode", strAccCode);

            return DbRetrieve("GL.GetAccount_AccCode", dsAcc, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get account's description related to specified account code.
        /// </summary>
        /// <param name="strAccCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetDesc(string strAccCode, string strConn)
        {
            var dsAccount = new DataSet();

            var result = Get(dsAccount, strAccCode, strConn);

            if (result)
            {
                if (dsAccount.Tables[TableName].Rows.Count > 0)
                {
                    return dsAccount.Tables[TableName].Rows[0]["Desc"].ToString();
                }
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get account's other description related to specified account code.
        /// </summary>
        /// <param name="strAccCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetOthDesc(string strAccCode, string strConn)
        {
            var dsAccount = new DataSet();

            var result = Get(dsAccount, strAccCode, strConn);

            if (result)
            {
                if (dsAccount.Tables[TableName].Rows.Count > 0)
                {
                    return dsAccount.Tables[TableName].Rows[0]["OthDesc"].ToString();
                }
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get table schema of account.
        /// </summary>
        /// <param name="dsAcc"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsAcc, string strConn)
        {
            return DbRetrieveSchema("GL.GetAccountList", dsAcc, null, TableName, strConn);
        }

        /// <summary>
        ///     Get account list related to specified view id.
        /// </summary>
        /// <param name="dsAcc"></param>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAcc, int intViewID, string strUserName, string strConn)
        {
            // Get query string and parameter
            var dbParams = new DbParameter[0];
            var strQuery = accView.GetQuery(intViewID, ref dbParams, strUserName, strConn);

            // Get account list.
            return DbExecuteQuery(strQuery, dsAcc, (dbParams.Length > 0 ? dbParams : null), TableName, strConn);
        }

        /// <summary>
        ///     Get account list related to isactive is true.
        /// </summary>
        /// <param name="dsAcc"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAcc, string strConn)
        {
            return DbRetrieve("GL.GetAccount_IsActived", dsAcc, null, TableName, strConn);
        }

        public DataTable GetList(string connStr)
        {
            // Create parameters
            return DbRead("dbo.Acc_GetActiveList", null, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetListByRowFilter(string filter, int startIndex, int endIndex, string connStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@filter", filter);
            dbParams[1] = new DbParameter("@startIndex", startIndex.ToString());
            dbParams[2] = new DbParameter("@endIndex", endIndex.ToString());

            // Create parameters
            return DbRead("dbo.Acc_GetActiveListByRowFilter", dbParams, connStr);
        }

        /// <summary>
        ///     return datatable and get searching match data.
        /// </summary>
        /// <param name="Search_Param"></param>
        /// <returns></returns>
        public DataTable GetSearch(string Search_Param, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Search_Param", Search_Param);

            return DbRead("GL.GetSearchAccountList", dbParams, connStr);
        }

        /// <summary>
        ///     Save account data to database.
        /// </summary>
        /// <param name="dsSaving"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Save(DataSet dsSaving, string strConn)
        {
            // Build savesource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSaving, SelectCommand, TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, strConn);
        }

        /// <summary>
        ///     Delete account data from database.
        /// </summary>
        /// <param name="dsDeleting"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsDeleting, string strConn)
        {
            // Build savesource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsDeleting, SelectCommand, TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, strConn);
        }

        /// <summary>
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
        /// </summary>
        /// <param name="strAccCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetRecType(string strAccCode, string strConn)
        {
            var dsAccount = new DataSet();

            var result = Get(dsAccount, strAccCode, strConn);

            if (result)
            {
                if (dsAccount.Tables[TableName].Rows.Count > 0)
                {
                    return dsAccount.Tables[TableName].Rows[0]["RecType"].ToString();
                }
                return string.Empty;
            }
            return string.Empty;
        }

        public int GetAccountByDepDivSec(string strDep, string strDiv, string strSec, string connStr)
        {
            int newAccCode;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@Depcode", strDep);
            dbParams[1] = new DbParameter("@DivCode", strDiv);
            dbParams[2] = new DbParameter("@SecCode", strSec);

            DbRetrieve("GL.GetAccountByDepDivSec", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                newAccCode = int.Parse(dsTmp.Tables[TableName].Rows[0]["NewAccCode"].ToString());
            }
            else
            {
                newAccCode = 1;
            }

            return newAccCode;
        }

        /// <summary>
        /// Get account data using account code
        /// </summary>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        //public bool GetAccount(DataSet dsAccount, string accountCode, string connStr)
        //{
        //    bool result = false;

        //    // Created parameter
        //    DbParameter[] dbParams  = new DbParameter[1];
        //    dbParams[0]             = new DbParameter("@AccountCode", accountCode);

        //    // Get data
        //    result = DbRetrieve("GL.GetAccount", dsAccount, dbParams, this.TableName, connStr);

        //    // Return result
        //    return result;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsAccount"></param>
        /// <param name="accountViewID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        //public void GetAccountList(DataSet dsAccount, int accountViewID, string connStr)
        //{
        //    //bool result = false;

        //    //// Paramter value assign to dbparameter array.
        //    //DbParameter[] dbParams = new DbParameter[1];
        //    //dbParams[0] = new DbParameter("@Accountviewid", Convert.ToString(accountViewID));

        //    //// Get data
        //    //result = DbRetrieve("Reference.GetAccountGenerateColumnList", dsAccount, dbParams, this.TableName, connStr);

        //    //// Return result
        //    //return result;

        //    DataTable dtAccount = new DataTable();
        //    Account account     = new Account();

        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0]            = new DbParameter("@Accountviewid", Convert.ToString(accountViewID));


        //    // Get data                
        //    dtAccount              = account.DbRead("Reference.GetAccountGenerateColumnList", dbParams, connStr);

        //    // Return result
        //    if (dsAccount.Tables[this.TableName] != null)
        //    {
        //        dsAccount.Tables.Remove(this.TableName);
        //    }

        //    dtAccount.TableName = this.TableName;
        //    dsAccount.Tables.Add(dtAccount);

        //}

        /// <summary>
        /// Get account for preview
        /// </summary>
        /// <param name="dsAccount"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        //public void GetAccountPreveiw(DataSet dsAccount, int userID, string connStr)
        //{
        //    DataTable dtAccount             = new DataTable();
        //    DataTable dtAccountViewCriteria = new DataTable();
        //    Account account                 = new Account();
        //    string accountViewQuery         = string.Empty;
        //    BL.Application.Field field      = new BL.Application.Field();

        //    // Generate  query
        //    accountViewQuery = new AccountView().GetAccountViewQueryPreview(dsAccount, userID, connStr);

        //    // Generate  parameter
        //    dtAccountViewCriteria = dsAccount.Tables["AccountViewCriteria"];

        //    if (dtAccountViewCriteria.Rows.Count > 0)
        //    {
        //        DbParameter[] dbParams = new DbParameter[dtAccountViewCriteria.Rows.Count];

        //        for (int i = 0; i < dtAccountViewCriteria.Rows.Count; i++)
        //        {
        //            DataRow dr = dtAccountViewCriteria.Rows[i];
        //            dbParams[i] = new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr), dr["Value"].ToString());
        //        }

        //        // Get data                
        //        dtAccount = account.DbExecuteQuery(accountViewQuery, dbParams, connStr);
        //    }
        //    else
        //    {
        //        // Get data                
        //        dtAccount = account.DbExecuteQuery(accountViewQuery, null, connStr);
        //    }

        //    // Return result
        //    if (dsAccount.Tables[this.TableName] != null)
        //    {
        //        dsAccount.Tables.Remove(this.TableName);
        //    }

        //    dtAccount.TableName = this.TableName;
        //    dsAccount.Tables.Add(dtAccount);
        //}

        /// <summary>
        /// Get account database structure
        /// </summary>
        /// <param name="dsAccount"></param>
        /// <returns></returns>
        //public bool GetAccountStructure(DataSet dsAccount, string connStr)
        //{
        //    bool result = false;

        //    // Get structure
        //    result = DbRetrieveSchema("Reference.GetAccountList", dsAccount, null, this.TableName, connStr);

        //    // Return result
        //    return result;
        //}

        /// <summary>
        /// Get description account
        /// </summary>
        /// <param name="SectionCode"></param>
        /// <returns></returns>
        //public string GetName(string AccountCode, string connStr)
        //{
        //    string accName;
        //    DataSet dsTmp          = new DataSet();
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0]            = new DbParameter("@AccountCode", AccountCode);

        //    DbRetrieve("GL.GetAccount", dsTmp, dbParams, this.TableName, connStr);

        //    if (dsTmp.Tables[this.TableName].Rows.Count > 0)
        //    {
        //        accName = dsTmp.Tables[this.TableName].Rows[0]["NameEng"].ToString();
        //    }
        //    else
        //    {
        //        accName = null;
        //    }

        //    return accName;
        //}

        /// <summary>
        /// return dataset and get account list
        /// </summary>
        /// <param name="dsAccount"></param>
        //public void GetAccountList(DataSet dsAccount, string connStr)
        //{
        //    Account account = new Account();
        //    account.DbRetrieve("Reference.GetAccountList", dsAccount, null, this.TableName, connStr);
        //}

        /// <summary>
        /// Get account using account view id 
        /// </summary>
        /// <param name="dsAccount"></param>
        //public void GetAccountList(DataSet dsAccount, int accountViewID, int userID, string connStr)
        //{
        //    DataTable dtAccount                   = new DataTable();
        //    DataTable dtAccountViewCriteria       = new DataTable();
        //    Account account                       = new Account();
        //    string accountViewQuery               = string.Empty;
        //    BlueLedger.BL.Application.Field field = new BlueLedger.BL.Application.Field();

        //    // Generate query
        //    accountViewQuery = new AccountView().GetAccountViewQuery(accountViewID, userID, connStr);

        //    // Generate parameter
        //    dtAccountViewCriteria = new AccountViewCriteria().GetAccountViewCriteriaList(accountViewID, connStr);

        //    if (dtAccountViewCriteria.Rows.Count > 0)
        //    {
        //        DbParameter[] dbParams = new DbParameter[dtAccountViewCriteria.Rows.Count];

        //        for(int i=0; i<dtAccountViewCriteria.Rows.Count; i++)
        //        {
        //            DataRow dr  = dtAccountViewCriteria.Rows[i];
        //            dbParams[i] = new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr) , dr["Value"].ToString());
        //        }

        //        // Get data                
        //        dtAccount = account.DbExecuteQuery(accountViewQuery, dbParams, connStr);
        //    }
        //    else
        //    {
        //        // Get data                
        //        dtAccount = account.DbExecuteQuery(accountViewQuery, null, connStr);
        //    }

        //    // Return resutl
        //    if (dsAccount.Tables[this.TableName] != null)
        //    {
        //        dsAccount.Tables.Remove(this.TableName);                
        //    }

        //    dtAccount.TableName = this.TableName;
        //    dsAccount.Tables.Add(dtAccount);            
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        //public DataTable GetAccountListLookup(string connStr)
        //{
        //    DataSet dsAccount = new DataSet();

        //    // Get Data
        //    DbRetrieve("Reference.GetAccountListLookup", dsAccount, null, this.TableName, connStr);


        //    // Return result
        //    if (dsAccount.Tables[this.TableName] != null)
        //    {
        //        DataRow drBlank = dsAccount.Tables[this.TableName].NewRow();
        //        dsAccount.Tables[this.TableName].Rows.InsertAt(drBlank, 0);
        //    }

        //    // Return result
        //    return dsAccount.Tables[this.TableName];
        //}

        /// <summary>
        /// Save to database.
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        //public bool Save(DataSet savedData, string connStr)
        //{
        //    BL.Reference.AccountMisc       accountMisc            = new BL.Reference.AccountMisc();
        //    //BL.Reference.AccountAttachment accountAttachment      = new BL.Reference.AccountAttachment();
        //    //BL.Reference.AccountComment    accountComment         = new AccountComment();
        //    //BL.Reference.AccountActiveLog  accountActiveLog       = new AccountActiveLog();

        //    // Build savesource object
        //    DbSaveSource[] dbSaveSource = new DbSaveSource[2];
        //    //dbSaveSource[0]             = new DbSaveSource(savedData, accountAttachment.SelectCommand, accountAttachment.TableName);
        //    dbSaveSource[0]             = new DbSaveSource(savedData, accountMisc.SelectCommand, accountMisc.TableName);
        //    dbSaveSource[1]             = new DbSaveSource(savedData, this.SelectCommand, this.TableName);
        //    //dbSaveSource[3]             = new DbSaveSource(savedData, accountComment.SelectCommand, accountComment.TableName);
        //    //dbSaveSource[4]             = new DbSaveSource(savedData, accountActiveLog.SelectCommand, accountActiveLog.TableName);


        //    // Call function dbCommit for commit data to database
        //    return DbCommit(dbSaveSource,connStr);
        //}

        /// <summary>
        /// Save only table reference.account
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        //public bool SaveAcc(DataSet savedData, string connStr)
        //{
        //    // Build savesource object
        //    DbSaveSource[] dbSaveSource = new DbSaveSource[1];
        //    dbSaveSource[0]             = new DbSaveSource(savedData, this.SelectCommand, this.TableName);

        //    // Call function dbCommit for commit data to database
        //    return DbCommit(dbSaveSource, connStr);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        //public bool Delete(DataSet savedData, string connStr)
        //{

        //    bool result = false;

        //    BL.Reference.AccountMisc        accountMisc          = new BL.Reference.AccountMisc();
        //    //BL.Reference.AccountAttachment  accountAttachment    = new BL.Reference.AccountAttachment();
        //    //BL.Reference.AccountComment     accountComment       = new AccountComment();
        //    //BL.Reference.AccountActiveLog   accountActiveLog     = new AccountActiveLog();


        //    // Build savesource object
        //    DbSaveSource[] dbSaveSource = new DbSaveSource[2];
        //    //dbSaveSource[0] = new DbSaveSource(savedData, accountAttachment.SelectCommand, accountAttachment.TableName);
        //    dbSaveSource[0] = new DbSaveSource(savedData, accountMisc.SelectCommand, accountMisc.TableName);
        //    //dbSaveSource[2] = new DbSaveSource(savedData, accountComment.SelectCommand,accountComment.TableName);
        //    //dbSaveSource[3] = new DbSaveSource(savedData, accountActiveLog.SelectCommand,accountActiveLog.TableName);
        //    dbSaveSource[1] = new DbSaveSource(savedData, this.SelectCommand, this.TableName);


        //    // Call dbCommit and send savesource object is parameter
        //    result = DbCommit(dbSaveSource, connStr);

        //    return result;
        //}

        #endregion
    }
}