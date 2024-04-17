using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.AP
{
    public class VendorTool : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public VendorTool()
        {
            SelectCommand = "SELECT * FROM AP.VendorTool";
            TableName = "VendorTool";
        }

        /// <summary>
        ///     Get standard voucher tool list
        /// </summary>
        /// <param name="dsVendorReport"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetVendorToolList(DataSet dsVendorReport, string connStr)
        {
            // Get data
            var result = DbRetrieve("AP.GetVendorToolList", dsVendorReport, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}