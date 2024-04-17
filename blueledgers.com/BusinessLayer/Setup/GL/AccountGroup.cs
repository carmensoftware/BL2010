using System.Data;
using Blue.DAL;

namespace Blue.BL.Setup.GL
{
    public class AccountGroup : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public AccountGroup()
        {
            SelectCommand = "SELECT * FROM GL.AccGroup";
            TableName = "AccGroup";
        }

        /// <summary>
        ///     Get account group data related to specified group code.
        /// </summary>
        /// <param name="dsAccGrp"></param>
        /// <param name="strGrpCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Get(DataSet dsAccGrp, string strGrpCode, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@GroupCode", strGrpCode);

            return DbRetrieve("GL.GetAccGroup_GroupCode", dsAccGrp, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get account group's desc related to specified group code.
        /// </summary>
        /// <param name="strGroupCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetDesc(string strGroupCode, string strConn)
        {
            var dsAccGrp = new DataSet();

            var result = Get(dsAccGrp, strGroupCode, strConn);

            if (result && dsAccGrp.Tables[TableName].Rows.Count > 0)
            {
                return dsAccGrp.Tables[TableName].Rows[0]["Desc"].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get account group's desc related to specified group code.
        /// </summary>
        /// <param name="strGroupCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetOthDesc(string strGroupCode, string strConn)
        {
            var dsAccGrp = new DataSet();

            var result = Get(dsAccGrp, strGroupCode, strConn);

            if (result && dsAccGrp.Tables[TableName].Rows.Count > 0)
            {
                return dsAccGrp.Tables[TableName].Rows[0]["OthDesc"].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get all account group data.
        /// </summary>
        /// <param name="dsAccountGroup"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAccountGroup, string strConn)
        {
            return DbRetrieve("GL.GetAccGroupList", dsAccountGroup, null, TableName, strConn);
        }

        /// <summary>
        ///     Get only actived account group data.
        /// </summary>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public DataTable GetActiveList(string strConn)
        {
            var dsAccGroup = new DataSet();

            var result = DbRetrieve("GL.GetAccGroupActiveList", dsAccGroup, null, TableName, strConn);

            if (result)
            {
                return dsAccGroup.Tables[TableName];
            }
            return null;
        }

        /// <summary>
        ///     Commit changed to database.
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsAccountGroup, string strConn)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsAccountGroup, SelectCommand, TableName);

            return DbCommit(dbSaveSource, strConn);
        }

        /// <summary>
        /// Get description account group
        /// </summary>
        /// <param name="SectionCode"></param>
        /// <returns></returns>
        //public string GetName(string GroupCode, string connStr)
        //{
        //    string GroupName;
        //    DataSet dsTmp          = new DataSet();
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0]            = new DbParameter("@GroupCode", GroupCode);

        //    DbRetrieve("Reference.GetAccountGroup", dsTmp, dbParams, this.TableName, connStr);

        //    if (dsTmp.Tables[this.TableName].Rows.Count > 0)
        //    {
        //        GroupName = dsTmp.Tables[this.TableName].Rows[0]["Name"].ToString();
        //    }
        //    else
        //    {
        //        GroupName = null;
        //    }
        //    return GroupName;
        //}

        /// <summary>
        /// return dataset and get account list
        /// </summary>
        /// <param name="dsAccount"></param>
        //public void GetAccountGroupList(DataSet dsAccountGroup, string connStr)
        //{
        //    Account account = new Account();
        //    account.DbRetrieve("Reference.GetAccountGroupList", dsAccountGroup, null, this.TableName, connStr);
        //}

        /// <summary>
        /// Get account group code data for DropDownList
        /// </summary>
        /// <returns></returns>
        //public DataTable GetAccountGroupForDropDownList(string connStr)
        //{
        //    DataSet dsAccountGroup = new DataSet();

        //    // Get data
        //    this.GetAccountGroupList(dsAccountGroup, connStr);

        //    // Return result
        //    if (dsAccountGroup.Tables[this.TableName] != null)
        //    {
        //        DataRow drBlank = dsAccountGroup.Tables[this.TableName].NewRow();
        //        dsAccountGroup.Tables[this.TableName].Rows.InsertAt(drBlank, 0);
        //    }

        //    return dsAccountGroup.Tables[this.TableName];
        //}

        /// <summary>
        /// Get name of group code by account code.
        /// </summary>
        /// <param name="accountCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        //public string GetNameByAccountCode(string accountCode, string connStr)
        //{
        //    string GroupName        = string.Empty;
        //    DataSet dsTmp           = new DataSet();
        //    DbParameter[] dbParams  = new DbParameter[1];
        //    dbParams[0]             = new DbParameter("@AccountCode", accountCode.ToString());

        //    DbRetrieve("Reference.GetAccountGroupbyAccountCode", dsTmp, dbParams, this.TableName, connStr);

        //    if (dsTmp.Tables[this.TableName].Rows.Count > 0)
        //    {
        //        GroupName = dsTmp.Tables[this.TableName].Rows[0]["Name"].ToString();
        //    }

        //    return GroupName;
        //}        

        #endregion
    }
}