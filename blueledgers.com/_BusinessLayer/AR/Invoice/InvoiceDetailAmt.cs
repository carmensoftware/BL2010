using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
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
            SelectCommand = "SELECT * FROM AR.InvoiceDetailAmt";
            TableName = "InvoiceDetailAmt";
        }

        /// <summary>
        ///     Get invoice detail list using invoice id
        /// </summary>
        /// <param name="standardVoucherID"></param>
        /// <param name="dsInvoice"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsInvoice, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("AP.GetInvoiceDetailAmtList", dsInvoice, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetByInvoiceDetailNo(string invoiceNo, int invoiceDetailNo, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@InvoiceNo", invoiceNo);
            dbParams[1] = new DbParameter("@InvoiceDetailNo", Convert.ToString(invoiceDetailNo));

            DbRetrieve("AR.GetInvoiceDetailAmtByInvoiceDetailNo", dsTmp, dbParams, TableName, connStr);

            return dsTmp;
        }

        /// <summary>
        ///     Get invoice detail amt using invoice no.
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetByInvoiceNo(string invoiceNo, string connStr)
        {
            var dtInvoiceDetailAmt = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@InvoiceNo", invoiceNo);

            // Get data
            dtInvoiceDetailAmt = DbRead("AP.GetInvoiceDetailAmtByInvoiceNo", dbParams, connStr);

            // Return result
            return dtInvoiceDetailAmt;
        }

        /// <summary>
        ///     Get InvoiceDetailAmt
        /// </summary>
        /// <param name="dsInvoiceDetailAmt"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsInvoiceDetailAmt, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("AP.GetInvoiceDetailAmtList", dsInvoiceDetailAmt, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get max no.
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <param name="invoiceDetailNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetMaxNo(string invoiceNo, int invoiceDetailNo, string connStr)
        {
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@InvoiceNo", invoiceNo);
            dbParams[1] = new DbParameter("@InvoiceDetailNo", Convert.ToString(invoiceDetailNo));

            // Retrieve data.
            DbRetrieve("AR.GetInvoiceDetailAmtMaxID", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows[0]["MaxInvoiceDetailAmtNo"].ToString() != string.Empty)
            {
                return (int) dsTmp.Tables[TableName].Rows[0]["MaxInvoiceDetailAmtNo"];
            }
            return 1;
        }

        #endregion
    }
}