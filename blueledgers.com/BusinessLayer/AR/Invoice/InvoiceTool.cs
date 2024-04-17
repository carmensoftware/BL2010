using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.AR
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
            SelectCommand = "SELECT * FROM AR.InvoiceTool";
            TableName = "InvoiceTool";
        }

        /// <summary>
        ///     Get invoice tool list
        /// </summary>
        /// <param name="dsInvoiceReport"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceToolList(DataSet dsInvoiceReport, string connStr)
        {
            // Get data
            bool result = DbRetrieve("AR.GetInvoiceToolList", dsInvoiceReport, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}