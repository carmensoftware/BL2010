using System.Data;
using System.Globalization;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class VendorDetail : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public VendorDetail()
        {
            SelectCommand = "SELECT * FROM AP.VendorDetail";
            TableName = "VendorDetail";
        }

        /// <summary>
        ///     Get vendor detail list using vendor id
        /// </summary>
        /// <param name="vendorID"></param>
        /// <param name="dsVendor"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetVendorDetailList(int vendorID, DataSet dsVendor, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VendorCode", vendorID.ToString(CultureInfo.InvariantCulture));

            // Get data
            var result = DbRetrieve("AP.GetVendorDetailListByVendorCode", dsVendor, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get vendor detail using vendor id
        /// </summary>
        /// <param name="vendorID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetVendorDetailListByVendorCode(int vendorID, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VendorCode", vendorID.ToString(CultureInfo.InvariantCulture));

            // Get data
            var dtVendorDetail = DbRead("AP.GetVendorDetailListByVendorCode", dbParams, connStr);

            // Return result
            return dtVendorDetail;
        }

        /// <summary>
        ///     Get VendorDetail
        /// </summary>
        /// <param name="dsVendorDetail"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetVendorDetailStructure(DataSet dsVendorDetail, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetVendorDetailList", dsVendorDetail, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}