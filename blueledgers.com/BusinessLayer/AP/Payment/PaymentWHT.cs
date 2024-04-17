using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentWHT : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentWHT()
        {
            SelectCommand = "SELECT * FROM AP.PaymentWHT";
            TableName = "PaymentWHT";
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="dsPayment"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentWHTList(string voucherNo, DataSet dsPayment, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var result = DbRetrieve("AP.GetPaymentWHTListByVoucherNo", dsPayment, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPaymentWHTListByVoucherNo(string voucherNo, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var dtPaymentWHT = DbRead("AP.GetPaymentWHTListByVoucherNo", dbParams, connStr);

            // Return result
            return dtPaymentWHT;
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetPaymentWHTMax(string voucherNo, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var paymentWHTMax = DbReadScalar("AP.GetPaymentWHTMax", dbParams, connStr);

            // Return result
            return paymentWHTMax;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPaymentWHT"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentWHTStructure(DataSet dsPaymentWHT, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetPaymentWHTList", dsPaymentWHT, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}