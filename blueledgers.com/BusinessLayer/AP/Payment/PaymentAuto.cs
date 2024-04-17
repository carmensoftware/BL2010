using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentAuto : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentAuto()
        {
            SelectCommand = "SELECT * FROM AP.PaymentAuto";
            TableName = "PaymentAuto";
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="dsPaymentAuto"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentAutoList(string voucherNo, DataSet dsPaymentAuto, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var result = DbRetrieve("AP.GetPaymentAutoListByVoucherNo", dsPaymentAuto, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetPaymentAutoList(string voucherNo, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);


            DbRetrieve("AP.GetPaymentAutoListByVoucherNo", dsTmp, dbParams, TableName, connStr);

            return dsTmp;
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPaymentAutoListByVoucherNo(string voucherNo, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var dtAP = DbRead("AP.GetPaymentAutoListByVoucherNo", dbParams, connStr);

            // Return result
            return dtAP;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPaymentAuto"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentAutoStructure(DataSet dsPaymentAuto, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetPaymentAutoList", dsPaymentAuto, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}