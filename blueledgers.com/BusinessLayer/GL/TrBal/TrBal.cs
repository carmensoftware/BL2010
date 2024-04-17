using System.Data;
using Blue.DAL;

namespace Blue.BL.GL.TrBal
{
    public class TrBal : DbHandler
    {
        #region "Attributies"

        private readonly TrBalView _trBalView = new TrBalView();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public TrBal()
        {
            SelectCommand = "SELECT * FROM GL.TrBal";
            TableName = "TrBal";
        }

        //public bool Get(DataSet dsAcc, string strAccCode, string strConn)
        //{
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0] = new DbParameter("@AccCode", strAccCode);
        //    return DbRetrieve("GL.GetAccount_AccCode", dsAcc, dbParams, this.TableName, strConn);
        //}
        //public string GetDesc(string strAccCode, string strConn)
        //{
        //    DataSet dsAccount = new DataSet();
        //    bool result = this.Get(dsAccount, strAccCode, strConn);
        //    if (result)
        //    {
        //        if (dsAccount.Tables[this.TableName].Rows.Count > 0)
        //        {
        //            return dsAccount.Tables[this.TableName].Rows[0]["Desc"].ToString();
        //        }
        //        else
        //        {
        //            return string.Empty;
        //        }
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}
        //public string GetOthDesc(string strAccCode, string strConn)
        //{
        //    DataSet dsAccount = new DataSet();
        //    bool result = this.Get(dsAccount, strAccCode, strConn);
        //    if (result)
        //    {
        //        if (dsAccount.Tables[this.TableName].Rows.Count > 0)
        //        {
        //            return dsAccount.Tables[this.TableName].Rows[0]["OthDesc"].ToString();
        //        }
        //        else
        //        {
        //            return string.Empty;
        //        }
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}
        //public bool GetStructure(DataSet dsAcc, string strConn)
        //{
        //    return DbRetrieveSchema("GL.GetAccountList", dsAcc, null, this.TableName, strConn);
        //}
        /// <summary>
        ///     Get account data related to specified account code
        /// </summary>
        /// <param name="dsAcc"></param>
        /// <param name="strAccCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        /// <summary>
        ///     Get account's description related to specified account code.
        /// </summary>
        /// <param name="strAccCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        /// <summary>
        ///     Get account's other description related to specified account code.
        /// </summary>
        /// <param name="strAccCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        /// <summary>
        ///     Get table schema of account.
        /// </summary>
        /// <param name="dsAcc"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        /// <summary>
        ///     Get account list related to specified view id.
        /// </summary>
        /// <param name="dsAcc"></param>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsTrBal, int intViewID, string strUserName, string strConn)
        {
            // Get query string and parameter
            var dbParams = new DbParameter[0];
            var strQuery = _trBalView.GetQuery(intViewID, ref dbParams, strUserName, strConn);

            // Get account list.
            return DbExecuteQuery(strQuery, dsTrBal, (dbParams.Length > 0 ? dbParams : null), TableName, strConn);
        }

        /// <summary>
        /// Get account list related to isactive is true.
        /// </summary>
        /// <param name="dsAcc"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        //public bool GetList(DataSet dsAcc, string strConn)
        //{
        //    return DbRetrieve("GL.GetAccount_IsActived", dsAcc, null, this.TableName, strConn);
        //}

        /// <summary>
        /// return datatable and get searching match data.
        /// </summary>
        /// <param name="Search_Param"></param>
        /// <returns></returns>
        //public DataTable GetSearch(string Search_Param, string connStr)
        //{
        //    // parameter value assing to param array.
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0]            = new DbParameter("@Search_Param", Search_Param);

        //    return this.DbRead("GL.GetSearchAccountList", dbParams, connStr);
        //}

        /// <summary>
        /// Save account data to database.
        /// </summary>
        /// <param name="dsSaving"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        //public bool Save(DataSet dsSaving, string strConn)
        //{
        //    // Build savesource object
        //    DbSaveSource[] dbSaveSource = new DbSaveSource[1];
        //    dbSaveSource[0] = new DbSaveSource(dsSaving, this.SelectCommand, this.TableName);

        //    // Call function dbCommit for commit data to database
        //    return DbCommit(dbSaveSource, strConn);
        //}

        /// <summary>
        /// Delete account data from database.
        /// </summary>
        /// <param name="dsDeleting"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        //public bool Delete(DataSet dsDeleting, string strConn)
        //{
        //    // Build savesource object
        //    DbSaveSource[] dbSaveSource = new DbSaveSource[1];
        //    dbSaveSource[0] = new DbSaveSource(dsDeleting, this.SelectCommand, this.TableName);

        //    // Call function dbCommit for commit data to database
        //    return DbCommit(dbSaveSource, strConn);
        //}        

        #endregion
    }
}