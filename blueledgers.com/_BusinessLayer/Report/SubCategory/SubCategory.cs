using System.Data;
using Blue.DAL;

namespace Blue.BL.Report
{
    public class SubCategory : DbHandler
    {
        #region "Operations"

        public SubCategory()
        {
            SelectCommand = "SELECT * FROM Report.SubCategory";
            TableName = "SubCategory";
        }

        /// <summary>
        ///     Get chart type data by active.
        /// </summary>
        /// <returns></returns>
        public bool GetSubCategoryList(DataSet dsSubCategory, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Report.GetSubCategoryActive", dsSubCategory, null, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        ///     Get category data for DropDownList
        /// </summary>
        /// <returns></returns>
        public DataTable GetSubCategoryForDropDownList(string connStr)
        {
            var dsSubCategory = new DataSet();

            // Get data
            GetSubCategoryList(dsSubCategory, connStr);

            // Return result
            if (dsSubCategory.Tables[TableName] != null)
            {
                var drBlank = dsSubCategory.Tables[TableName].NewRow();
                dsSubCategory.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsSubCategory.Tables[TableName];
        }

        /// <summary>
        /// </summary>
        /// <param name="DivisionCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetSubCategoryLookupByCategoryID(string CategoryID, string connStr)
        {
            var dsSubCategory = new DataSet();

            // Get data
            var subCategory = new SubCategory();

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CategoryID", CategoryID);


            subCategory.DbRetrieve("Report.GetSubCategoryByCategoryID", dsSubCategory, dbParams, TableName, connStr);

            // Return result
            if (dsSubCategory.Tables[TableName] != null)
            {
                var drBlank = dsSubCategory.Tables[TableName].NewRow();
                dsSubCategory.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsSubCategory.Tables[TableName];
        }

        #endregion
    }
}