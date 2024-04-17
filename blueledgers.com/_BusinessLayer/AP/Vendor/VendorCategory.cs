using System.Data;
using System.Globalization;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class VendorCategory : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public VendorCategory()
        {
            SelectCommand = "SELECT * FROM AP.VendorCategory";
            TableName = "VendorCategory";
        }


        /// <summary>
        /// </summary>
        /// <param name="dsVendorCategory"></param>
        /// <param name="vendorCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetVendorCategoryListByVendorCategoryCode(DataSet dsVendorCategory, string vendorCode,
            string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@VendorCategoryCode", vendorCode);

            // Get data
            var result = DbRetrieve("AP.GetVendorCategoryListByVendorCategoryCode", dsVendorCategory, dbParams,
                TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsVendorCategory"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetVendorCategoryList(DataSet dsVendorCategory, string connStr)
        {
            var result = DbRetrieve("AP.GetVendorCategoryList", dsVendorCategory, null, TableName, connStr);

            return result;
        }


        /// <summary>
        ///     Get schema for VendorCategory.
        /// </summary>
        /// <param name="dsVendorCategory"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsVendorCategory, string connectionString)
        {
            return DbRetrieveSchema("AP.GetVendorCategoryList", dsVendorCategory, null, TableName, connectionString);
        }


        /// <summary>
        ///     Get VendorCategoryName by VendorCategoryCode
        /// </summary>
        /// <param name="vendorCategoryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetVendorCategoryName(string vendorCategoryCode, string connStr)
        {
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@VendorCategoryCode", vendorCategoryCode);

            DbRetrieve("AP.GetVendorCategoryListByVendorCategoryCode", dsTmp, dbParams, TableName, connStr);

            var vendorCategoryName = dsTmp.Tables[TableName].Rows.Count > 0
                ? dsTmp.Tables[TableName].Rows[0]["Name"].ToString()
                : null;
            return vendorCategoryName;
        }


        /// <summary>
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetSearchVendorCategoryList(string searchParam, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Search_Param", searchParam);

            return DbRead("AP.GetSearchVendorCategoryList", dbParams, connStr);
        }

        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsVendorCategory"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsVendorCategory, string connStr)
        {
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsVendorCategory, SelectCommand, TableName);

            // Save to database
            var result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }


        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetVendorCategoryLookup(string connStr)
        {
            var dsVendorCategory = new DataSet();

            // Get Data
            DbRetrieve("AP.GetVendorCategoryLookup", dsVendorCategory, null, TableName, connStr);

            // Return result
            return dsVendorCategory.Tables[TableName];
        }


        /// <summary>
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetListByRowFilter(string filter, int startIndex, int endIndex, string connStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@filter", filter);
            dbParams[1] = new DbParameter("@startIndex", startIndex.ToString(CultureInfo.InvariantCulture));
            dbParams[2] = new DbParameter("@endIndex", endIndex.ToString(CultureInfo.InvariantCulture));

            // Create parameters
            return DbRead("dbo.VendorCategory_GetActiveListByRowFilter", dbParams, connStr);
        }

        #endregion
    }
}