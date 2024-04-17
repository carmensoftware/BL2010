using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentCash : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentCash()
        {
            SelectCommand = "SELECT * FROM AP.PaymentCash";
            TableName = "PaymentCash";
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="dsPaymentCash"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentCashList(string voucherNo, DataSet dsPaymentCash, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var result = DbRetrieve("AP.GetPaymentCashListByVoucherNo", dsPaymentCash, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetPaymentCashList(string voucherNo, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);


            DbRetrieve("AP.GetPaymentCashListByVoucherNo", dsTmp, dbParams, TableName, connStr);

            return dsTmp;
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPaymentCashListByVoucherNo(string voucherNo, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var dtAP = DbRead("AP.GetPaymentCashListByVoucherNo", dbParams, connStr);

            // Return result
            return dtAP;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPaymentCash"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentCashStructure(DataSet dsPaymentCash, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetPaymentCashList", dsPaymentCash, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}