using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentDetail : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentDetail()
        {
            SelectCommand = "SELECT * FROM AP.PaymentDetail";
            TableName = "PaymentDetail";
        }

        /// <summary>
        ///     Get invoice detail list using invoice id
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="dsPayment"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentDetailList(string voucherNo, DataSet dsPayment, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var result = DbRetrieve("AP.GetPaymentDetailListByVoucherNo", dsPayment, dbParams, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetPaymentDetailMaxID(string voucherNo, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);
            // Get data
            var result = DbReadScalar("AP.GetPaymentDetailMaxID", dbParams, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="dsPayment"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentDetailListSelectedByVoucherNo(string voucherNo, DataSet dsPayment, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var result = DbRetrieve("AP.GetPaymentDetailListSelectedByVoucherNo", dsPayment, dbParams, TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPayment"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceDetailListSelected(DataSet dsPayment, string connStr)
        {
            // Get data
            var result = DbRetrieve("AP.GetInvoiceDetailListSelected", dsPayment, null, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        ///     Get invoice detail using invoice id
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPaymentDetailListByVoucherNo(string voucherNo, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var dtPaymentDetail = DbRead("AP.GetPaymentDetailListByVoucherNo", dbParams, connStr);

            // Return result
            return dtPaymentDetail;
        }

        /// <summary>
        ///     Get PaymentDetail
        /// </summary>
        /// <param name="dsPaymentDetail"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentDetailStructure(DataSet dsPaymentDetail, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetPaymentDetailList", dsPaymentDetail, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}