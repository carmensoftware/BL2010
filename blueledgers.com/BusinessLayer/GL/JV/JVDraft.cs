using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL.JV
{
    public class JVDraft : DbHandler
    {
        #region "Attributies"

        private readonly JVView _jvView = new JVView();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public JVDraft()
        {
            SelectCommand = "SELECT * FROM GL.JVDraft";
            TableName = "JVDraft";
        }

        /// <summary>
        ///     Get journal voucher data structure.
        /// </summary>
        /// <param name="dsJV"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsJV, string strConn)
        {
            return DbRetrieveSchema("GL.GetJVDraftList", dsJV, null, TableName, strConn);
        }

        /// <summary>
        /// </summary>
        /// <param name="strPrefixCode"></param>
        /// <param name="date"></param>
        /// <param name="conStr"></param>
        /// <returns></returns>
        public string GetNewJVNo(string strPrefixCode, DateTime date, string conStr)
        {
            var dtJV = new DataTable();
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@PrefixCode", strPrefixCode);
            dbParams[1] = new DbParameter("@JVDate", date.ToString());

            // Get data
            dtJV = DbRead("GL.GetJournalVoucherNo", dbParams, conStr);

            // Return result            
            return dtJV.Rows[0]["NewJournalVoucher"].ToString();
        }

        /// <summary>
        ///     Return dataset from journalvoucher.
        /// </summary>
        /// <param name="dsJournalVoucher"></param>
        public void GetByJVNo(DataSet dsJournalVoucher, string jvNo, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@JVNo", jvNo);

            var jv = new JV();
            jv.DbRetrieve("GL.GetJV_JVNo", dsJournalVoucher, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get account list related to specified view id.
        /// </summary>
        /// <param name="dsAcc"></param>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsJV, int intViewID, string strUserName, string strConn)
        {
            // Get query string and parameter
            var dbParams = new DbParameter[0];
            var strQuery = _jvView.GetQuery(intViewID, ref dbParams, strUserName, strConn);

            // Get account list.
            return DbExecuteQuery(strQuery, dsJV, (dbParams.Length > 0 ? dbParams : null), TableName, strConn);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsJV"></param>
        /// <param name="AccCode"></param>
        /// <param name="connStr"></param>
        public void GetByAccCode(DataSet dsJV, string AccCode, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AccCode", AccCode);

            var jv = new JV();
            jv.DbRetrieve("GL.GetJV_AccCode", dsJV, dbParams, TableName, connStr);
        }

        public string GetCurrencyCode(string JvNo, string conStr)
        {
            var dsTmp = new DataSet();

            GetByJVNo(dsTmp, JvNo, conStr);

            return dsTmp.Tables["JV"].Rows[0]["CurrencyCode"].ToString();
        }

        /// <summary>
        ///     Save voided jv to database
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Void(DataSet savedData, string connStr)
        {
            // Build savesource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        ///     Save to database.
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var jvDtDraft = new JVDtDraft();
            var ledgersDraft = new LedgersDraft();

            // Build savesource object
            var dbSaveSource = new DbSaveSource[3];
            //dbSaveSource[0] = new DbSaveSource(savedData, ledgers.SelectCommand, ledgers.TableName);
            //dbSaveSource[1] = new DbSaveSource(savedData, jvDt.SelectCommand, jvDt.TableName);            
            //dbSaveSource[2] = new DbSaveSource(savedData, this.SelectCommand, this.TableName);

            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, jvDtDraft.SelectCommand, jvDtDraft.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, ledgersDraft.SelectCommand, ledgersDraft.TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, connStr);
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
        /// Get account for check category.
        /// </summary>
        /// <param name="dsAccount"></param>
        /// <param name="accountCode"></param>
        /// <param name="connStr"></param>
        //public void GetByChkCategory(DataSet dsAccount, string accountCode, string connStr)
        //{
        //    DbParameter[] dbParams  = new DbParameter[1];
        //    dbParams[0]             = new DbParameter("@AccountCode", accountCode);

        //    DbRetrieve("GL.GetAccountByChkCategory", dsAccount, dbParams, this.TableName, connStr);
        //}

        /// <summary>
        /// return datatable and get searching match data.
        /// </summary>
        /// <param name="Search_Param"></param>
        /// <returns></returns>
        //public DataTable GetSearchAccountList(string Search_Param, string connStr)
        //{   
        //    // parameter value assing to param array.
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0] = new DbParameter("@Search_Param", Search_Param);

        //    return this.DbRead("Reference.GetSearchAccountList", dbParams, connStr);
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