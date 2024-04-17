using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.AR
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
            SelectCommand = "SELECT * FROM AR.InvoiceReport";
            TableName = "InvoiceReport";
        }

        /// <summary>
        ///     Get invoice report list
        /// </summary>
        /// <param name="dsInvoiceReport"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceReportList(DataSet dsInvoiceReport, string connStr)
        {
            // Get data
            bool result = DbRetrieve("AR.GetInvoiceReportList", dsInvoiceReport, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}