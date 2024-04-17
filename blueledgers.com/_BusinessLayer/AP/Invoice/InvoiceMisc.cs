using System;
using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
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
            SelectCommand = "SELECT * FROM AP.InvoiceMisc";
            TableName = "InvoiceMisc";
        }

        /// <summary>
        ///     Get all of invoice Misc.
        /// </summary>
        /// <param name="dsInvoiceMisc"></param>
        /// <param name="voucherNo"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsInvoiceMisc, string voucherNo, string connectionString)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@VoucherNo", voucherNo);

            return DbRetrieve("AP.GetInvoiceMiscListByVoucherNo", dsInvoiceMisc, dbParam, TableName, connectionString);
        }

        /// <summary>
        ///     Get schema for invoice misc.
        /// </summary>
        /// <param name="dsInvoiceMisc"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsInvoiceMisc, string connectionString)
        {
            return DbRetrieveSchema("AP.GetInvoiceMiscList", dsInvoiceMisc, null, TableName, connectionString);
        }

        #endregion
    }
}