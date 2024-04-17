using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class TaxInvoice : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public TaxInvoice()
        {
            SelectCommand = "SELECT * FROM AP.TaxInvoice";
            TableName = "TaxInvoice";
        }


        public bool GetTaxInvoiceList(string voucherNo, DataSet dsInvoice, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var result = DbRetrieve("AP.GetTaxInvoiceListByVoucherNo", dsInvoice, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get invoice detail using invoice id
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetTaxInvoiceListByVoucherNo(string voucherNo, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var dtTaxInvoice = DbRead("AP.GetTaxInvoiceListByVoucherNo", dbParams, connStr);

            // Return result
            return dtTaxInvoice;
        }

        /// <summary>
        ///     Get TaxInvoice
        /// </summary>
        /// <param name="dsTaxInvoice"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetTaxInvoiceStructure(DataSet dsTaxInvoice, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetTaxInvoiceList", dsTaxInvoice, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}