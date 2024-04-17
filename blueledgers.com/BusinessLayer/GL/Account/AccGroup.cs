using System.Data;
using Blue.DAL;

namespace Blue.BL.GL.Account
{
    public class AccGroup : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public AccGroup()
        {
            SelectCommand = "SELECT * FROM GL.AccGroup";
            TableName = "AccGroup";
        }

        /// <summary>
        ///     Get description account group
        /// </summary>
        /// <param name="SectionCode"></param>
        /// <returns></returns>
        public string GetName(string GroupCode, string connStr)
        {
            string GroupName;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@GroupCode", GroupCode);

            DbRetrieve("GL.GetAccGroup_GroupCode", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                GroupName = dsTmp.Tables[TableName].Rows[0]["Desc"].ToString();
            }
            else
            {
                GroupName = null;
            }
            return GroupName;
        }

        /// <summary>
        ///     return dataset and get account list
        /// </summary>
        /// <param name="dsAccount"></param>
        public void GetList(DataSet dsAccGroup, string connStr)
        {
            var account = new Account();
            account.DbRetrieve("GL.GetAccGroupList", dsAccGroup, null, TableName, connStr);
        }

        /// <summary>
        ///     Get account group code data for DropDownList
        /// </summary>
        /// <returns></returns>
        public DataTable GetDropDownList(string connStr)
        {
            var dsAccGroup = new DataSet();

            // Get data
            GetList(dsAccGroup, connStr);

            // Return result
            if (dsAccGroup.Tables[TableName] != null)
            {
                var drBlank = dsAccGroup.Tables[TableName].NewRow();
                dsAccGroup.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsAccGroup.Tables[TableName];
        }

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
        /// <summary>
        ///     Get name of group code by account code.
        /// </summary>
        /// <param name="accountCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        /// <summary>
        ///     Commit changed to database.
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);

            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}