using System.Data;
using Blue.DAL;

namespace Blue.BL.Setup.GL
{
    public class AccountCategory : DbHandler
    {
        /// <summary>
        ///     Empty constructure
        /// </summary>
        public AccountCategory()
        {
            SelectCommand = "SELECT * FROM GL.AccCat";
            TableName = "AccCat";
        }

        /// <summary>
        ///     Get account category related to specified group code and category code.
        /// </summary>
        /// <param name="dsAccCat"></param>
        /// <param name="strAccGroupCode"></param>
        /// <param name="strAccCateCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Get(DataSet dsAccCat, string strAccGroupCode, string strAccCatCode, string strConn)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@GroupCode", strAccGroupCode);
            dbParams[1] = new DbParameter("@CategoryCode", strAccCatCode);

            return DbRetrieve("GL.GetAccCat_GroupCode_CategoryCode", dsAccCat, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get account category's description related to specified group code and category code.
        /// </summary>
        /// <param name="strGrpCode"></param>
        /// <param name="strCatCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetDesc(string strGroupCode, string strCategoryCode, string strConn)
        {
            var dsCat = new DataSet();

            var result = Get(dsCat, strGroupCode, strCategoryCode, strConn);

            if (result && dsCat.Tables[TableName].Rows.Count > 0)
            {
                return dsCat.Tables[TableName].Rows[0]["Desc"].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get account category's other description related to specified group code and category code.
        /// </summary>
        /// <param name="strGrpCode"></param>
        /// <param name="strCatCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetOthDesc(string strGroupCode, string strCategoryCode, string strConn)
        {
            var dsCat = new DataSet();

            var result = Get(dsCat, strGroupCode, strCategoryCode, strConn);

            if (result && dsCat.Tables[TableName].Rows.Count > 0)
            {
                return dsCat.Tables[TableName].Rows[0]["OthDesc"].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get nature of specified account category.
        /// </summary>
        /// <param name="strAccGroupCode"></param>
        /// <param name="strAccCatCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetNature(string strAccGroupCode, string strAccCatCode, string strConn)
        {
            var dsAccCat = new DataSet();

            var result = Get(dsAccCat, strAccGroupCode, strAccCatCode, strConn);

            if (result)
            {
                return dsAccCat.Tables[TableName].Rows[0]["Nature"].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get account category data related to specified account group code.
        /// </summary>
        /// <param name="dsAccCat"></param>
        /// <param name="strAccGroupCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAccCat, string strAccGroupCode, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@GroupCode", strAccGroupCode);
            return DbRetrieve("GL.GetAccCatList_GroupCode", dsAccCat, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get only active account category related to sepecified group code.
        /// </summary>
        /// <param name="strGroupCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public DataTable GetActiveList(string strGroupCode, string strConn)
        {
            var dsAccCat = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@GroupCode", strGroupCode);
            var result = DbRetrieve("GL.GetAccCatActiveList_GroupCode", dsAccCat, dbParams, TableName, strConn);

            if (result)
            {
                return dsAccCat.Tables[TableName];
            }
            return null;
        }

        /// <summary>
        ///     Save data
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <param name="Connstr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsAccCat, string strConn)
        {
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsAccCat, SelectCommand, TableName);

            // Commit to database
            var result = DbCommit(dbSaveSource, strConn);

            // Return result
            return result;
        }

        /// <summary>
        /// Get category using category code
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <param name="categoryCode"></param>
        /// <returns></returns>
        //public bool GetCategory(DataSet dsCategory, string categoryCode,string connStr)
        //{
        //    bool result            = false;
        //    DbParameter[] dbParams = new DbParameter[1];

        //    // Create parameter
        //    dbParams[0] = new DbParameter("@CategoryCode", categoryCode);

        //    // Get data
        //    result = DbRetrieve("Reference.GetCategory", dsCategory, dbParams, this.TableName,connStr);

        //    // Return result
        //    return result;
        //}

        /// <summary>
        /// Get category name using category code
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <returns></returns>
        //public string GetCategoryName(string categoryCode,string connStr)
        //{
        //    string result      = string.Empty;
        //    DataSet dsCategory = new DataSet();

        //    // Get data
        //    this.GetCategory(dsCategory,categoryCode,connStr);

        //    // Return result
        //    if (dsCategory.Tables[this.TableName] != null)
        //    {
        //        if (dsCategory.Tables[this.TableName].Rows.Count > 0)
        //        {
        //            result = dsCategory.Tables[this.TableName].Rows[0]["Name"].ToString();
        //        }
        //    }

        //    return result;
        //}


        //public string GetReconcileType(string categoryCode, string connStr)
        //{
        //    string result       = string.Empty;
        //    DataSet dsCategory  = new DataSet();

        //    // Get data
        //    this.GetCategory(dsCategory, categoryCode, connStr);

        //    // Return result
        //    if (dsCategory.Tables[this.TableName] != null)
        //    {
        //        if (dsCategory.Tables[this.TableName].Rows.Count > 0)
        //        {
        //            result = dsCategory.Tables[this.TableName].Rows[0]["ReconcileType"].ToString();
        //        }
        //    }

        //    return result;
        //}

        /// <summary>
        /// Get all category
        /// </summary>
        /// <returns></returns>
        //public bool GetCategoryList(DataSet dsCategory, string connStr)
        //{
        //    bool result = false;

        //    // Get data
        //    result = DbRetrieve("Reference.GetCategorylist", dsCategory, null, this.TableName,connStr);

        //    // Return result
        //    return result;
        //}

        /// <summary>
        /// Get all category which active
        /// </summary>
        /// <returns></returns>
        //public bool GetCategoryList(DataSet dsCategory, bool isActive, string connStr)
        //{
        //    bool result = false;
        //    DbParameter[] dbParams = new DbParameter[1];

        //    // Create parameter
        //    dbParams[0] = new DbParameter("@IsActive", isActive.ToString());

        //    // Get data
        //    result = DbRetrieve("Reference.GetCategoryActive", dsCategory, dbParams, this.TableName, connStr);

        //    // Return result
        //    return result;
        //}

        /// <summary>
        /// Get category by accgroup and active
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <param name="accGroup"></param>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        //public bool GetCategoryList(DataSet dsCategory, string accGroup, bool isActive, string connStr)
        //{
        //    bool result = false;
        //    DbParameter[] dbParams = new DbParameter[2];

        //    // Create parameter
        //    dbParams[0] = new DbParameter("@AccountGroup", accGroup.ToString());
        //    dbParams[1] = new DbParameter("@IsActive", isActive.ToString());

        //    // Get data
        //    result = DbRetrieve("Reference.GetCategoryByGroupActive", dsCategory, dbParams, this.TableName, connStr);

        //    // Return result
        //    return result;
        //}       

        /// <summary>
        /// Get category data by account group and active
        /// </summary>
        /// <returns></returns>
        //public DataTable GetCategoryForDropDownList(string accGroup, string connStr)
        //{
        //    DataSet dsCategory = new DataSet();

        //    // Get data
        //    this.GetCategoryList(dsCategory, accGroup.ToString(), true, connStr);

        //    // Return result
        //    if (dsCategory.Tables[this.TableName] != null)
        //    {
        //        DataRow drBlank = dsCategory.Tables[this.TableName].NewRow();
        //        dsCategory.Tables[this.TableName].Rows.InsertAt(drBlank, 0);
        //    }

        //    return dsCategory.Tables[this.TableName];
        //}

        /// <summary>
        /// Get category data by active.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        //public DataTable GetCategoryForDropDownList(string connStr)
        //{
        //    DataSet dsCategory = new DataSet();

        //    // Get data
        //    this.GetCategoryList(dsCategory, true, connStr);

        //    // Return result
        //    if (dsCategory.Tables[this.TableName] != null)
        //    {
        //        DataRow drBlank = dsCategory.Tables[this.TableName].NewRow();
        //        dsCategory.Tables[this.TableName].Rows.InsertAt(drBlank, 0);
        //    }

        //    return dsCategory.Tables[this.TableName];
        //}        
    }
}