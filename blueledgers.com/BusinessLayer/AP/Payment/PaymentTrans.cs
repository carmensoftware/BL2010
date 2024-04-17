using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentTrans : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentTrans()
        {
            SelectCommand = "SELECT * FROM AP.PaymentTrans";
            TableName = "PaymentTrans";
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="dsPaymentTrans"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentTransList(string voucherNo, DataSet dsPaymentTrans, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var result = DbRetrieve("AP.GetPaymentTransListByVoucherNo", dsPaymentTrans, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPaymentTransListByVoucherNo(string voucherNo, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var dtAP = DbRead("AP.GetPaymentTransListByVoucherNo", dbParams, connStr);

            // Return result
            return dtAP;
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetPaymentTransList(string voucherNo, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);


            DbRetrieve("AP.GetPaymentTransListByVoucherNo", dsTmp, dbParams, TableName, connStr);

            return dsTmp;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPaymentTrans"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentTransStructure(DataSet dsPaymentTrans, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetPaymentTransList", dsPaymentTrans, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}