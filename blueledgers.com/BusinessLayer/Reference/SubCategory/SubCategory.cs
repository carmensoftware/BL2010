using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class SubCategory : DbHandler
    {
        /// <summary>
        ///     Empty constructure
        /// </summary>
        public SubCategory()
        {
            SelectCommand = "SELECT * FROM Reference.SubCategory";
            TableName = "SubCategory";
        }

        /// <summary>
        ///     Get sub category name using sub category code
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <returns></returns>
        public string GetName(string subCategoryCode, string connStr)
        {
            var result = string.Empty;
            var dsSubCategory = new DataSet();

            // Get data
            GetBySubCategoryCode(dsSubCategory, subCategoryCode, connStr);

            // Return result
            if (dsSubCategory.Tables[TableName] != null)
            {
                if (dsSubCategory.Tables[TableName].Rows.Count > 0)
                {
                    result = dsSubCategory.Tables[TableName].Rows[0]["Name"].ToString();
                }
            }

            return result;
        }

        /// <summary>
        ///     Get all sub category
        /// </summary>
        /// <returns></returns>
        public bool GetList(DataSet dsSubCategory, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Reference.GetSubCategoryList", dsSubCategory, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get sub category code using sub category code
        /// </summary>
        /// <param name="dsSubCategory"></param>
        /// <param name="subCategoryCode"></param>
        /// <returns></returns>
        public bool GetBySubCategoryCode(DataSet dsSubCategory, string subCategoryCode, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@SubCategoryCode", subCategoryCode);

            // Get data
            result = DbRetrieve("Reference.GetSubCategory", dsSubCategory, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all sub-category by CategoryCode.
        /// </summary>
        /// <param name="dsSubcategory"></param>
        /// <param name="categoryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetByCategoryCode(DataSet dsSubcategory, string categoryCode, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CategoryCode", categoryCode);

            return DbRetrieve("Reference.GetSubCategoryByCategoryCode", dsSubcategory, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get all sub category which active
        /// </summary>
        /// <returns></returns>
        public bool GetByActive(DataSet dsSubCategory, bool isActive, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@IsActive", isActive.ToString());

            // Get data
            result = DbRetrieve("Reference.GetSubCategoryActive", dsSubCategory, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get sub category by isactive and category code.
        /// </summary>
        /// <param name="divisionCode"></param>
        /// <param name="dsDepartment"></param>
        /// <returns></returns>
        public DataSet GetByCateActive(string categoryCode, bool isActive, DataSet dsSubCategory, string connStr)
        {
            var dbParams = new DbParameter[2];

            dbParams[0] = new DbParameter("@CategoryCode", categoryCode);
            dbParams[1] = new DbParameter("@IsActive", isActive.ToString());

            DbRetrieve("Reference.GetSubCategoryByCateActive", dsSubCategory, dbParams, TableName, connStr);

            return dsSubCategory;
        }

        /// <summary>
        ///     Get sub category data by active.
        /// </summary>
        /// <returns></returns>
        public DataTable GetForDropDownList(string connStr)
        {
            var dsSubCategory = new DataSet();

            // Get data
            GetByActive(dsSubCategory, true, connStr);

            // Return result
            if (dsSubCategory.Tables[TableName] != null)
            {
                var drBlank = dsSubCategory.Tables[TableName].NewRow();
                dsSubCategory.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsSubCategory.Tables[TableName];
        }

        /// <summary>
        ///     Get sub category data by categoryCode and active.
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetForDropDownList(string categoryCode, string connStr)
        {
            var dsSubCategory = new DataSet();

            // Get data
            GetByCateActive(categoryCode, true, dsSubCategory, connStr);

            // Return result
            if (dsSubCategory.Tables[TableName] != null)
            {
                var drBlank = dsSubCategory.Tables[TableName].NewRow();
                dsSubCategory.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsSubCategory.Tables[TableName];
        }

        /// <summary>
        ///     Save data
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <returns></returns>
        public bool Save(DataSet dsSubCategory, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsSubCategory, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }
    }
}