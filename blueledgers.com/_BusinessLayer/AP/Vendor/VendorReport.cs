using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class VendorReport : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public VendorReport()
        {
            SelectCommand = "SELECT * FROM AP.VendorReport";
            TableName = "VendorReport";
        }

        /// <summary>
        ///     Get standard voucher report list
        /// </summary>
        /// <param name="dsVendorReport"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetVendorReportList(DataSet dsVendorReport, string connStr)
        {
            // Get data
            var result = DbRetrieve("AP.GetVendorReportList", dsVendorReport, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}