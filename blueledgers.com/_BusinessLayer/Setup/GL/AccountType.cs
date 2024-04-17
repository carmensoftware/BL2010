using System.Data;
using Blue.DAL;

namespace Blue.BL.Setup.GL
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
            SelectCommand = "SELECT * FROM GL.AccType";
            TableName = "AccType";
        }

        /// <summary>
        ///     Get account type data related to specified type code.
        /// </summary>
        /// <param name="dsAccType"></param>
        /// <param name="strAccType"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Get(DataSet dsAccType, string strTypeCode, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@TypeCode", strTypeCode);

            return DbRetrieve("GL.GetAccType_TypeCode", dsAccType, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get account type's desc related to specified type code.
        /// </summary>
        /// <param name="strTypeCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetDesc(string strTypeCode, string strConn)
        {
            var dsAccType = new DataSet();

            var result = Get(dsAccType, strTypeCode, strConn);

            if (result && dsAccType.Tables[TableName].Rows.Count > 0)
            {
                return dsAccType.Tables[TableName].Rows[0]["Desc"].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get account type's desc related to specified type code.
        /// </summary>
        /// <param name="strTypeCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetOthDesc(string strTypeCode, string strConn)
        {
            var dsAccType = new DataSet();

            var result = Get(dsAccType, strTypeCode, strConn);

            if (result && dsAccType.Tables[TableName].Rows.Count > 0)
            {
                return dsAccType.Tables[TableName].Rows[0]["OthDesc"].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get all Account Type data.
        /// </summary>
        /// <returns></returns>
        public bool GetList(DataSet dsAccType, string strConn)
        {
            // Get data
            var result = DbRetrieve("GL.GetAccTypeList", dsAccType, null, TableName, strConn);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all active account type.
        /// </summary>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public DataTable GetActiveList(string strConn)
        {
            var dsAccType = new DataSet();

            var result = DbRetrieve("GL.GetAccTypeActiveList", dsAccType, null, TableName, strConn);

            if (result)
            {
                return dsAccType.Tables[TableName];
            }
            return null;
        }

        /// <summary>
        ///     Save process.
        /// </summary>
        /// <param name="dsUOM"></param>
        /// <returns></returns>
        public bool Save(DataSet dsAccountType, string strConn)
        {
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsAccountType, SelectCommand, TableName);

            // Commit to database
            var result = DbCommit(dbSaveSource, strConn);

            // Return result
            return result;
        }

        /// <summary>
        /// Get account type data using account code
        /// </summary>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        //public bool GetAccountType(DataSet dsAccountType, string strTypeCode, string strConn)
        //{           
        //    DbParameter[] dbParams = new DbParameter[1];

        //    //// Created parameter
        //    dbParams[0] = new DbParameter("@TypeID", typeID.ToString());

        //    // Get data
        //    bool result = DbRetrieve("Reference.GetAccountType", dsAccountType, dbParams, this.TableName, connStr);

        //    // Return result
        //    return result;
        //}

        /// <summary>
        /// Get account type name
        /// </summary>
        /// <param name="SectionCode"></param>
        /// <returns></returns>
        //public string GetAccountTypeName(int typeID, string connStr)
        //{
        //    string result         = string.Empty;
        //    DataSet dsAccountType = new DataSet();

        //    // Get data
        //    this.GetAccountType(dsAccountType, typeID, connStr);

        //    // Return result
        //    if (dsAccountType.Tables[this.TableName] != null)
        //    {
        //        if (dsAccountType.Tables[this.TableName].Rows.Count > 0)
        //        {
        //            result = dsAccountType.Tables[this.TableName].Rows[0]["Name"].ToString();
        //        }
        //    }

        //    return result;
        //}

        /// <summary>
        /// Get all account type
        /// </summary>
        /// <returns></returns>
        //public bool GetAccountTypeList(DataSet dsAccountType, string connStr)
        //{
        //    bool result = false;

        //    // Get data
        //    result = DbRetrieve("Reference.GetAccountTypeList", dsAccountType, null, this.TableName, connStr);

        //    // Return result
        //    return result;
        //}


        /// <summary>
        /// Get all account type which active
        /// </summary>
        /// <returns></returns>
        //public bool GetAccountTypeList(DataSet dsAccountType, bool isActive, string connStr)
        //{
        //    bool result            = false;
        //    DbParameter[] dbParams = new DbParameter[1];

        //    // Create parameter
        //    dbParams[0]            = new DbParameter("@IsActive",isActive.ToString());

        //    // Get data
        //    result                 = DbRetrieve("Reference.GetAccountTypeActive",dsAccountType,dbParams,this.TableName,connStr);

        //    // Return result
        //    return result;
        //}

        /// <summary>
        /// Get account type data for DropDownList
        /// </summary>
        /// <returns></returns>
        //public DataTable GetAccountTypeForDropDownList(string connStr)
        //{
        //    DataSet dsAccountType = new DataSet();

        //    // Get data
        //    this.GetAccountTypeList(dsAccountType, true, connStr);

        //    // Return result
        //    if (dsAccountType.Tables[this.TableName] != null)
        //    {
        //        DataRow drBlank = dsAccountType.Tables[this.TableName].NewRow();
        //        dsAccountType.Tables[this.TableName].Rows.InsertAt(drBlank,0);
        //    }

        //    return dsAccountType.Tables[this.TableName];
        //}

        /// <summary>
        /// Get max account typeID.
        /// </summary>
        /// <returns></returns>
        //public int GetTypeIDMax(string connStr)
        //{
        //    int result = 0;

        //    // Get data
        //    result = DbReadScalar("Reference.GetTypeIDMax",null, connStr);

        //    if (result == 0)
        //    {
        //        result = 1;

        //    }
        //    // Return result
        //    return result;
        //}        

        #endregion
    }
}