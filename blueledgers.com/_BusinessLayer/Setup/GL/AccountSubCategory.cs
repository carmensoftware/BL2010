using System.Data;
using Blue.DAL;

namespace Blue.BL.Setup.GL
{
    public class AccountSubCategory : DbHandler
    {
        /// <summary>
        ///     Empty constructure
        /// </summary>
        public AccountSubCategory()
        {
            SelectCommand = "SELECT * FROM GL.AccSubCat";
            TableName = "AccSubCat";
        }

        /// <summary>
        ///     Get account subcategory data related to specified group code and category code.
        /// </summary>
        /// <param name="dsSubCategory"></param>
        /// <param name="strGroupCode"></param>
        /// <param name="strCategoryCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsSubCategory, string strGroupCode, string strCategoryCode, string strConn)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@GroupCode", strGroupCode);
            dbParams[1] = new DbParameter("@CategoryCode", strCategoryCode);
            return DbRetrieve("GL.GetAccSubCatList_GroupCode_CategoryCode", dsSubCategory, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get account subcategory data related to specified group code, category code and sub category code.
        /// </summary>
        /// <param name="dsSubCategory"></param>
        /// <param name="strGroupCode"></param>
        /// <param name="strCategoryCode"></param>
        /// <param name="strSubCategoryCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Get(DataSet dsSubCategory, string strGroupCode, string strCategoryCode, string strSubCategoryCode,
            string strConn)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@GroupCode", strGroupCode);
            dbParams[1] = new DbParameter("@CategoryCode", strCategoryCode);
            dbParams[2] = new DbParameter("@SubCategoryCode", strSubCategoryCode);

            return DbRetrieve("GL.GetSubCategory_GroupCode_CategoryCode_SubcategoryCode", dsSubCategory, dbParams,
                TableName, strConn);
        }

        /// <summary>
        ///     Get account sub category's description related to specified group code, category code and sub category code.
        /// </summary>
        /// <param name="strGroupCode"></param>
        /// <param name="strCategoryCode"></param>
        /// <param name="strSubCategoryCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetDesc(string strGroupCode, string strCategoryCode, string strSubCategoryCode, string strConn)
        {
            var dsSubCat = new DataSet();

            var result = Get(dsSubCat, strGroupCode, strCategoryCode, strSubCategoryCode, strConn);

            if (result && dsSubCat.Tables[TableName].Rows.Count > 0)
            {
                return dsSubCat.Tables[TableName].Rows[0]["Desc"].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get account sub category's other description related to specified group code, category code and sub category code.
        /// </summary>
        /// <param name="strGroupCode"></param>
        /// <param name="strCategoryCode"></param>
        /// <param name="strSubCategoryCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetOthDesc(string strGroupCode, string strCategoryCode, string strSubCategoryCode, string strConn)
        {
            var dsSubCat = new DataSet();

            var result = Get(dsSubCat, strGroupCode, strCategoryCode, strSubCategoryCode, strConn);

            if (result && dsSubCat.Tables[TableName].Rows.Count > 0)
            {
                return dsSubCat.Tables[TableName].Rows[0]["OthDesc"].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get only active account subcategory related to sepecified group code and category code.
        /// </summary>
        /// <param name="strGroupCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public DataTable GetActiveList(string strGroupCode, string strCategoryCode, string strConn)
        {
            var dsAccSubCat = new DataSet();
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@GroupCode", strGroupCode);
            dbParams[1] = new DbParameter("@CategoryCode", strCategoryCode);
            var result = DbRetrieve("GL.GetAccSubCatActiveList_GroupCode_CategoryCode", dsAccSubCat, dbParams, TableName,
                strConn);

            if (result)
            {
                return dsAccSubCat.Tables[TableName];
            }
            return null;
        }

        /// <summary>
        ///     Save changed account sub category to database.
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <returns></returns>
        public bool Save(DataSet dsSubCategory, string strConn)
        {
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsSubCategory, SelectCommand, TableName);

            // Commit to database
            return DbCommit(dbSaveSource, strConn);
        }

        ///// <summary>
        ///// Get sub category name using sub category code
        ///// </summary>
        ///// <param name="categoryCode"></param>
        ///// <returns></returns>
        //public string GetName(string subCategoryCode, string connStr)
        //{
        //    string result         = string.Empty;
        //    DataSet dsSubCategory = new DataSet();

        //    // Get data
        //    this.GetBySubCategoryCode(dsSubCategory, subCategoryCode, connStr);

        //    // Return result
        //    if (dsSubCategory.Tables[this.TableName] != null)
        //    {
        //        if (dsSubCategory.Tables[this.TableName].Rows.Count > 0)
        //        {
        //            result = dsSubCategory.Tables[this.TableName].Rows[0]["Name"].ToString();
        //        }
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// Get all sub category
        ///// </summary>
        ///// <returns></returns>
        //public bool GetList(DataSet dsSubCategory, string connStr)
        //{
        //    bool result = false;

        //    // Get data
        //    result = DbRetrieve("Reference.GetSubCategoryList", dsSubCategory, null, this.TableName, connStr);

        //    // Return result
        //    return result;
        //}

        ///// <summary>
        ///// Get sub category code using sub category code
        ///// </summary>
        ///// <param name="dsSubCategory"></param>
        ///// <param name="subCategoryCode"></param>
        ///// <returns></returns>
        //public bool GetBySubCategoryCode(DataSet dsSubCategory, string subCategoryCode, string connStr)
        //{
        //    bool result = false;
        //    DbParameter[] dbParams = new DbParameter[1];

        //    // Create parameter
        //    dbParams[0] = new DbParameter("@SubCategoryCode", subCategoryCode);

        //    // Get data
        //    result = DbRetrieve("Reference.GetSubCategory", dsSubCategory, dbParams, this.TableName, connStr);

        //    // Return result
        //    return result;
        //}

        ///// <summary>
        ///// Get all sub-category by CategoryCode.
        ///// </summary>
        ///// <param name="dsSubcategory"></param>
        ///// <param name="categoryCode"></param>
        ///// <param name="connStr"></param>
        ///// <returns></returns>
        //public bool GetByCategoryCode(DataSet dsSubcategory, string categoryCode, string connStr)
        //{
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0]            = new DbParameter("@CategoryCode", categoryCode);

        //    return DbRetrieve("Reference.GetSubCategoryByCategoryCode", dsSubcategory, dbParams, this.TableName, connStr);
        //}

        ///// <summary>
        ///// Get all sub category which active
        ///// </summary>
        ///// <returns></returns>
        //public bool GetByActive(DataSet dsSubCategory, bool isActive, string connStr)
        //{
        //    bool result            = false;
        //    DbParameter[] dbParams = new DbParameter[1];

        //    // Create parameter
        //    dbParams[0] = new DbParameter("@IsActive", isActive.ToString());

        //    // Get data
        //    result = DbRetrieve("Reference.GetSubCategoryActive", dsSubCategory,dbParams,this.TableName,connStr);

        //    // Return result
        //    return result;
        //}

        ///// <summary>
        ///// Get sub category by isactive and category code.
        ///// </summary>
        ///// <param name="divisionCode"></param>
        ///// <param name="dsDepartment"></param>
        ///// <returns></returns>
        //public DataSet GetByCateActive(string categoryCode, bool isActive, DataSet dsSubCategory, string connStr)
        //{
        //    DbParameter[] dbParams = new DbParameter[2];

        //    dbParams[0] = new DbParameter("@CategoryCode", categoryCode.ToString());
        //    dbParams[1] = new DbParameter("@IsActive", isActive.ToString());

        //    DbRetrieve("Reference.GetSubCategoryByCateActive", dsSubCategory, dbParams, this.TableName, connStr);

        //    return dsSubCategory;
        //}

        ///// <summary>
        ///// Get sub category data by active.
        ///// </summary>
        ///// <returns></returns>
        //public DataTable GetForDropDownList(string connStr)
        //{
        //    DataSet dsSubCategory = new DataSet();

        //    // Get data
        //    this.GetByActive(dsSubCategory, true, connStr);

        //    // Return result
        //    if (dsSubCategory.Tables[this.TableName] != null)
        //    {
        //        DataRow drBlank = dsSubCategory.Tables[this.TableName].NewRow();
        //        dsSubCategory.Tables[this.TableName].Rows.InsertAt(drBlank,0);
        //    }

        //    return dsSubCategory.Tables[this.TableName];
        //}

        ///// <summary>
        ///// Get sub category data by categoryCode and active.
        ///// </summary>
        ///// <param name="categoryCode"></param>
        ///// <param name="connStr"></param>
        ///// <returns></returns>
        //public DataTable GetForDropDownList(string categoryCode, string connStr)
        //{
        //    DataSet dsSubCategory = new DataSet();

        //    // Get data
        //    this.GetByCateActive(categoryCode, true, dsSubCategory, connStr);

        //    // Return result
        //    if (dsSubCategory.Tables[this.TableName] != null)
        //    {
        //        DataRow drBlank = dsSubCategory.Tables[this.TableName].NewRow();
        //        dsSubCategory.Tables[this.TableName].Rows.InsertAt(drBlank, 0);
        //    }

        //    return dsSubCategory.Tables[this.TableName];
        //} 
    }
}