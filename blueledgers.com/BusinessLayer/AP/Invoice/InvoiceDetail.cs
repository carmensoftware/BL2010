using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class InvoiceDetail : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public InvoiceDetail()
        {
            SelectCommand = "SELECT * FROM AP.InvoiceDetail";
            TableName = "InvoiceDetail";
        }

        /// <summary>
        ///     Get invoice detail list using invoice id
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="dsInvoice"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceDetailList(string voucherNo, DataSet dsInvoice, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var result = DbRetrieve("AP.GetInvoiceDetailListByVoucherNo", dsInvoice, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get invoice detail using invoice id
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetInvoiceDetailListByVoucherNo(string voucherNo, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var dtInvoiceDetail = DbRead("AP.GetInvoiceDetailListByVoucherNo", dbParams, connStr);

            // Return result
            return dtInvoiceDetail;
        }

        /// <summary>
        ///     Get InvoiceDetail
        /// </summary>
        /// <param name="dsInvoiceDetail"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceDetailStructure(DataSet dsInvoiceDetail, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetInvoiceDetailList", dsInvoiceDetail, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}