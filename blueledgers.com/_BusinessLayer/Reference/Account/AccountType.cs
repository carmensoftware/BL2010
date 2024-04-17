using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class AccountType : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public AccountType()
        {
            this.SelectCommand = "SELECT * FROM Reference.AccountType";
            this.TableName = "AccountType";
        }

        /// <summary>
        ///     Get account type data using account code
        /// </summary>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        public bool GetAccountType(DataSet dsAccountType, int typeID, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            //// Created parameter
            dbParams[0] = new DbParameter("@TypeID", typeID.ToString());

            // Get data
            result = DbRetrieve("Reference.GetAccountType", dsAccountType, dbParams, this.TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get account type name
        /// </summary>
        /// <param name="SectionCode"></param>
        /// <returns></returns>
        public string GetAccountTypeName(int typeID, string connStr)
        {
            var result = string.Empty;
            var dsAccountType = new DataSet();

            // Get data
            this.GetAccountType(dsAccountType, typeID, connStr);

            // Return result
            if (dsAccountType.Tables[this.TableName] != null)
            {
                if (dsAccountType.Tables[this.TableName].Rows.Count > 0)
                {
                    result = dsAccountType.Tables[this.TableName].Rows[0]["Name"].ToString();
                }
            }

            return result;
        }

        //public bool GetAccountTypeList(DataSet dsAccountType, string connStr)
        //{
        //    bool result = false;
        //    // Get data
        //    result = DbRetrieve("Reference.GetAccountTypeList", dsAccountType, null, this.TableName, connStr);
        //    // Return result
        //    return result;
        //}
        /// <summary>
        ///     Get all account type
        /// </summary>
        /// <returns></returns>
        /// <summary>
        ///     Get all account type from accountType table.
        /// </summary>
        /// <returns></returns>
        public bool GetAccountTypeListAll(DataSet dsAccountType, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Reference.GetAccountTypeList", dsAccountType, null, this.TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all account type which active
        /// </summary>
        /// <returns></returns>
        public bool GetAccountTypeList(DataSet dsAccountType, bool isActive, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@IsActive", isActive.ToString());

            // Get data
            result = DbRetrieve("Reference.GetAccountTypeActive", dsAccountType, dbParams, this.TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get account type data for DropDownList
        /// </summary>
        /// <returns></returns>
        public DataTable GetAccountTypeForDropDownList(string connStr)
        {
            var dsAccountType = new DataSet();

            // Get data
            this.GetAccountTypeList(dsAccountType, true, connStr);

            // Return result
            if (dsAccountType.Tables[this.TableName] != null)
            {
                DataRow drBlank = dsAccountType.Tables[this.TableName].NewRow();
                dsAccountType.Tables[this.TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsAccountType.Tables[this.TableName];
        }

        /// <summary>
        ///     Get max account typeID.
        /// </summary>
        /// <returns></returns>
        public int GetTypeIDMax(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("Reference.GetTypeIDMax", null, connStr);

            if (result == 0)
            {
                result = 1;
            }
            // Return result
            return result;
        }

        /// <summary>
        ///     Save process.
        /// </summary>
        /// <param name="dsUOM"></param>
        /// <returns></returns>
        public bool Save(DataSet dsAccountType, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsAccountType, this.SelectCommand, this.TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}