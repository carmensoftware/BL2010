using System.Data;
using Blue.DAL;

namespace Blue.BL.GL.Account
{
    public class AccType : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public AccType()
        {
            SelectCommand = "SELECT * FROM GL.AccType";
            TableName = "AccType";
        }

        /// <summary>
        ///     Get account type data using account code
        /// </summary>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        public bool Get(DataSet dsAccountType, string typeCode, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Created parameter
            dbParams[0] = new DbParameter("@TypeCode", typeCode);

            // Get data
            result = DbRetrieve("GL.GetAccountType_TypeCode", dsAccountType, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get account type name
        /// </summary>
        /// <param name="SectionCode"></param>
        /// <returns></returns>
        public string GetName(string typeCode, string connStr)
        {
            var result = string.Empty;
            var dsAccountType = new DataSet();

            // Get data
            Get(dsAccountType, typeCode, connStr);

            // Return result
            if (dsAccountType.Tables[TableName] != null)
            {
                if (dsAccountType.Tables[TableName].Rows.Count > 0)
                {
                    result = dsAccountType.Tables[TableName].Rows[0]["Desc"].ToString();
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
        public bool GetList(DataSet dsAccountType, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Reference.GetAccountTypeList", dsAccountType, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all account type which active
        /// </summary>
        /// <returns></returns>
        public bool GetList(DataSet dsAccountType, bool isActive, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@IsActive", isActive.ToString());

            // Get data
            result = DbRetrieve("Reference.GetAccountTypeActive", dsAccountType, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get account type data for DropDownList
        /// </summary>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            var dsAccountType = new DataSet();

            // Get data
            GetList(dsAccountType, true, connStr);

            // Return result
            if (dsAccountType.Tables[TableName] != null)
            {
                var drBlank = dsAccountType.Tables[TableName].NewRow();
                dsAccountType.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsAccountType.Tables[TableName];
        }

        /// <summary>
        ///     Get max account typeID.
        /// </summary>
        /// <returns></returns>
        public int GetMax(string connStr)
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
            dbSaveSource[0] = new DbSaveSource(dsAccountType, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}