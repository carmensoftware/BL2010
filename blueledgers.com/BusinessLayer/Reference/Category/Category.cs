using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class Category : DbHandler
    {
        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Category()
        {
            SelectCommand = "SELECT * FROM Reference.Category";
            TableName = "Category";
        }

        /// <summary>
        ///     Get category using category code
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <param name="categoryCode"></param>
        /// <returns></returns>
        public bool GetCategory(DataSet dsCategory, string categoryCode, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@CategoryCode", categoryCode);

            // Get data
            result = DbRetrieve("Reference.GetCategory", dsCategory, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get category name using category code
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <returns></returns>
        public string GetCategoryName(string categoryCode, string connStr)
        {
            var result = string.Empty;
            var dsCategory = new DataSet();

            // Get data
            GetCategory(dsCategory, categoryCode, connStr);

            // Return result
            if (dsCategory.Tables[TableName] != null)
            {
                if (dsCategory.Tables[TableName].Rows.Count > 0)
                {
                    result = dsCategory.Tables[TableName].Rows[0]["Name"].ToString();
                }
            }

            return result;
        }


        public string GetReconcileType(string categoryCode, string connStr)
        {
            var result = string.Empty;
            var dsCategory = new DataSet();

            // Get data
            GetCategory(dsCategory, categoryCode, connStr);

            // Return result
            if (dsCategory.Tables[TableName] != null)
            {
                if (dsCategory.Tables[TableName].Rows.Count > 0)
                {
                    result = dsCategory.Tables[TableName].Rows[0]["ReconcileType"].ToString();
                }
            }

            return result;
        }

        /// <summary>
        ///     Get all category
        /// </summary>
        /// <returns></returns>
        public bool GetCategoryList(DataSet dsCategory, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Reference.GetCategorylist", dsCategory, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all category which active
        /// </summary>
        /// <returns></returns>
        public bool GetCategoryList(DataSet dsCategory, bool isActive, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@IsActive", isActive.ToString());

            // Get data
            result = DbRetrieve("Reference.GetCategoryActive", dsCategory, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get category by accgroup and active
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <param name="accGroup"></param>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetCategoryList(DataSet dsCategory, string accGroup, bool isActive, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[2];

            // Create parameter
            dbParams[0] = new DbParameter("@AccountGroup", accGroup);
            dbParams[1] = new DbParameter("@IsActive", isActive.ToString());

            // Get data
            result = DbRetrieve("Reference.GetCategoryByGroupActive", dsCategory, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get category data by account group and active
        /// </summary>
        /// <returns></returns>
        public DataTable GetCategoryForDropDownList(string accGroup, string connStr)
        {
            var dsCategory = new DataSet();

            // Get data
            GetCategoryList(dsCategory, accGroup, true, connStr);

            // Return result
            if (dsCategory.Tables[TableName] != null)
            {
                var drBlank = dsCategory.Tables[TableName].NewRow();
                dsCategory.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsCategory.Tables[TableName];
        }

        /// <summary>
        ///     Get category data by active.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetCategoryForDropDownList(string connStr)
        {
            var dsCategory = new DataSet();

            // Get data
            GetCategoryList(dsCategory, true, connStr);

            // Return result
            if (dsCategory.Tables[TableName] != null)
            {
                var drBlank = dsCategory.Tables[TableName].NewRow();
                dsCategory.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsCategory.Tables[TableName];
        }

        /// <summary>
        ///     Save data
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <param name="Connstr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsCategory, string Connstr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsCategory, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, Connstr);

            // Return result
            return result;
        }
    }
}