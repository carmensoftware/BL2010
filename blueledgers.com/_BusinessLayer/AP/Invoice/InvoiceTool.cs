using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class InvoiceTool : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public InvoiceTool()
        {
            SelectCommand = "SELECT * FROM AP.InvoiceTool";
            TableName = "InvoiceTool";
        }

        /// <summary>
        ///     Get standard voucher tool list
        /// </summary>
        /// <param name="dsInvoiceReport"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceToolList(DataSet dsInvoiceReport, string connStr)
        {
            // Get data
            var result = DbRetrieve("AP.GetInvoiceToolList", dsInvoiceReport, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}