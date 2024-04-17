using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentCredit : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentCredit()
        {
            SelectCommand = "SELECT * FROM AP.PaymentCredit";
            TableName = "PaymentCredit";
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="dsPaymentCredit"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentCreditList(string voucherNo, DataSet dsPaymentCredit, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var result = DbRetrieve("AP.GetPaymentCreditListByVoucherNo", dsPaymentCredit, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPaymentCreditListByVoucherNo(string voucherNo, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var dtAP = DbRead("AP.GetPaymentCreditListByVoucherNo", dbParams, connStr);

            // Return result
            return dtAP;
        }


        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetPaymentCreditList(string voucherNo, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);


            DbRetrieve("AP.GetPaymentCreditListByVoucherNo", dsTmp, dbParams, TableName, connStr);

            return dsTmp;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPaymentCredit"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentCreditStructure(DataSet dsPaymentCredit, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetPaymentCreditList", dsPaymentCredit, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}