using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
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
            this.SelectCommand = "SELECT * FROM Reference.AccountGroup";
            this.TableName = "AccountGroup";
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

            DbRetrieve("Reference.GetAccountGroup", dsTmp, dbParams, this.TableName, connStr);

            if (dsTmp.Tables[this.TableName].Rows.Count > 0)
            {
                GroupName = dsTmp.Tables[this.TableName].Rows[0]["Name"].ToString();
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
        public void GetAccountGroupList(DataSet dsAccountGroup, string connStr)
        {
            var account = new Account();
            account.DbRetrieve("Reference.GetAccountGroupList", dsAccountGroup, null, this.TableName, connStr);
        }

        /// <summary>
        ///     Get account group code data for DropDownList
        /// </summary>
        /// <returns></returns>
        public DataTable GetAccountGroupForDropDownList(string connStr)
        {
            var dsAccountGroup = new DataSet();

            // Get data
            this.GetAccountGroupList(dsAccountGroup, connStr);

            // Return result
            if (dsAccountGroup.Tables[this.TableName] != null)
            {
                var drBlank = dsAccountGroup.Tables[this.TableName].NewRow();
                dsAccountGroup.Tables[this.TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsAccountGroup.Tables[this.TableName];
        }

        /// <summary>
        ///     Get name of group code by account code.
        /// </summary>
        /// <param name="accountCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetNameByAccountCode(string accountCode, string connStr)
        {
            var GroupName = string.Empty;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AccountCode", accountCode);

            DbRetrieve("Reference.GetAccountGroupbyAccountCode", dsTmp, dbParams, this.TableName, connStr);

            if (dsTmp.Tables[this.TableName].Rows.Count > 0)
            {
                GroupName = dsTmp.Tables[this.TableName].Rows[0]["Name"].ToString();
            }

            return GroupName;
        }

        /// <summary>
        ///     Commit changed to database.
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(savedData, this.SelectCommand, this.TableName);

            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}