using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.AP
{
    public class InvoiceReport : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public InvoiceReport()
        {
            SelectCommand = "SELECT * FROM AP.InvoiceReport";
            TableName = "InvoiceReport";
        }

        /// <summary>
        ///     Get standard voucher report list
        /// </summary>
        /// <param name="dsInvoiceReport"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceReportList(DataSet dsInvoiceReport, string connStr)
        {
            // Get data
            var result = DbRetrieve("AP.GetInvoiceReportList", dsInvoiceReport, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}