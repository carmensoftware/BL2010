using System;
using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class InvoiceDetailAmt : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public InvoiceDetailAmt()
        {
            SelectCommand = "SELECT * FROM AP.InvoiceDetailAmt";
            TableName = "InvoiceDetailAmt";
        }

        /// <summary>
        ///     Get invoice detail list using invoice id
        /// </summary>
        /// <param name="invoiceDetailNo"></param>
        /// <param name="dsInvoice"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceDetailAmtList(int invoiceDetailNo, DataSet dsInvoice, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@InvoiceDetailNo", Convert.ToString(invoiceDetailNo));

            // Get data
            var result = DbRetrieve("AP.GetInvoiceDetailAmtListByInvoiceDetailNo", dsInvoice, dbParams, TableName,
                connStr);

            // Return result
            return result;
        }


        /// <summary>
        /// </summary>
        /// <param name="invoiceDetailNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetInvoiceDetailAmtList(int invoiceDetailNo, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@InvoiceDetailNo", Convert.ToString(invoiceDetailNo));


            DbRetrieve("AP.GetInvoiceDetailAmtListByInvoiceDetailNo", dsTmp, dbParams, TableName, connStr);

            return dsTmp;
        }

        /// <summary>
        ///     Get invoice detail using invoice id
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetInvoiceDetailAmtListByVoucherNo(string voucherNo, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var dtInvoiceDetailAmt = DbRead("AP.GetInvoiceDetailAmtListByVoucherNo", dbParams, connStr);

            // Return result
            return dtInvoiceDetailAmt;
        }

        /// <summary>
        ///     Get InvoiceDetailAmt
        /// </summary>
        /// <param name="dsInvoiceDetailAmt"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceDetailAmtStructure(DataSet dsInvoiceDetailAmt, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetInvoiceDetailAmtList", dsInvoiceDetailAmt, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}