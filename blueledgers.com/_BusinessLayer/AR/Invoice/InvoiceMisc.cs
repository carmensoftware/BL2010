using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class InvoiceMisc : DbHandler
    {
        #region "Attributies"

        public int VoucherNo { get; set; }
        public Guid FieldID { get; set; }
        public string Value { get; set; }

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public InvoiceMisc()
        {
            SelectCommand = "SELECT * FROM AR.InvoiceMisc";
            TableName = "InvoiceMisc";
        }

        /// <summary>
        ///     Get all of invoice Misc.
        /// </summary>
        /// <param name="dsInvoiceMisc"></param>
        /// <param name="InvoiceNo"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsInvoiceMisc, string InvoiceNo, string connStr)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@InvoiceNo", InvoiceNo);

            return DbRetrieve("AR.GetInvoiceMiscListByInvoiceNo", dsInvoiceMisc, dbParam, TableName, connStr);
        }

        /// <summary>
        ///     Get schema for invoice misc.
        /// </summary>
        /// <param name="dsInvoiceMisc"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsInvoiceMisc, string ConnectionString)
        {
            return DbRetrieveSchema("AR.GetInvoiceMiscList", dsInvoiceMisc, null, TableName, ConnectionString);
        }

        #endregion
    }
}