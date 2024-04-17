using System;
using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentMisc : DbHandler
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
        public PaymentMisc()
        {
            SelectCommand = "SELECT * FROM AP.PaymentMisc";
            TableName = "PaymentMisc";
        }

        /// <summary>
        ///     Get all of payment Misc.
        /// </summary>
        /// <param name="dsPaymentMisc"></param>
        /// <param name="voucherNo"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsPaymentMisc, string voucherNo, string connectionString)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@VoucherNo", voucherNo);

            return DbRetrieve("AP.GetPaymentMiscListByVoucherNo", dsPaymentMisc, dbParam, TableName, connectionString);
        }

        /// <summary>
        ///     Get schema for payment misc.
        /// </summary>
        /// <param name="dsPaymentMisc"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsPaymentMisc, string connectionString)
        {
            return DbRetrieveSchema("AP.GetPaymentMiscList", dsPaymentMisc, null, TableName, connectionString);
        }

        #endregion
    }
}