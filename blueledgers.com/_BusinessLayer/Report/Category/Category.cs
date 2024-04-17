using System.Data;
using Blue.DAL;

namespace Blue.BL.Report
{
    public class Category : DbHandler
    {
        #region "Operations"

        public Category()
        {
            SelectCommand = "SELECT * FROM Report.Category";
            TableName = "Category";
        }

        /// <summary>
        ///     Get chart type data by active.
        /// </summary>
        /// <returns></returns>
        public bool GetCategoryList(DataSet dsCategory, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Report.GetCategoryActive", dsCategory, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get category data for DropDownList
        /// </summary>
        /// <returns></returns>
        public DataTable GetCategoryForDropDownList(string connStr)
        {
            var dsCategory = new DataSet();

            // Get data
            GetCategoryList(dsCategory, connStr);

            // Return result
            if (dsCategory.Tables[TableName] != null)
            {
                var drBlank = dsCategory.Tables[TableName].NewRow();
                dsCategory.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsCategory.Tables[TableName];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        //public int GetCategoryID(string categoryName, string connStr)
        //{
        //    int result;
        //    DataSet dsCategory = new DataSet();

        //    // Create parameter
        //    DbParameter[] dbParams  = new DbParameter[1];
        //    dbParams[0]             = new DbParameter("@CategoryName", categoryName.ToString());

        //    // Get data
        //    result = DbRetrieve("Report.GetReportCategoryID", dsCategory, dbParams, this.TableName, connStr);

        //    return result;
        //}

        #endregion
    }
}