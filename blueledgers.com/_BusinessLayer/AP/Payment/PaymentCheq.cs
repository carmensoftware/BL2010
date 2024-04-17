using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentCheq : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentCheq()
        {
            SelectCommand = "SELECT * FROM AP.PaymentCheq";
            TableName = "PaymentCheq";
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="dsPaymentCheq"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentCheqList(string voucherNo, DataSet dsPaymentCheq, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var result = DbRetrieve("AP.GetPaymentCheqListByVoucherNo", dsPaymentCheq, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetPaymentCheqList(string voucherNo, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);


            DbRetrieve("AP.GetPaymentCheqListByVoucherNo", dsTmp, dbParams, TableName, connStr);

            return dsTmp;
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPaymentCheqListByVoucherNo(string voucherNo, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var dtPaymentCheq = DbRead("AP.GetPaymentCheqListByVoucherNo", dbParams, connStr);

            // Return result
            return dtPaymentCheq;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPaymentCheq"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentCheqStructure(DataSet dsPaymentCheq, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetPaymentCheqList", dsPaymentCheq, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}