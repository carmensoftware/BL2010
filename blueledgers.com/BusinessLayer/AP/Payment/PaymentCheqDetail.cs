using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentCheqDetail : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentCheqDetail()
        {
            SelectCommand = "SELECT * FROM AP.PaymentCheqDetail";
            TableName = "PaymentCheqDetail";
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="dsPaymentCheqDetail"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentCheqDetailList(string voucherNo, DataSet dsPaymentCheqDetail, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var result = DbRetrieve("AP.GetPaymentCheqDetailListByVoucherNo", dsPaymentCheqDetail, dbParams, TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetPaymentCheqDetailList(string voucherNo, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);


            DbRetrieve("AP.GetPaymentCheqDetailListByVoucherNo", dsTmp, dbParams, TableName, connStr);

            return dsTmp;
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetPaymentCheqDetailNoMax(string voucherNo, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var cheqDetailNoMax = DbReadScalar("AP.GetPaymentCheqDetailNoMax", dbParams, connStr);

            // Return result
            return cheqDetailNoMax;
        }


        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPaymentCheqDetailListByVoucherNo(string voucherNo, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var dtPaymentCheqDetail = DbRead("AP.GetPaymentCheqDetailListByVoucherNo", dbParams, connStr);

            // Return result
            return dtPaymentCheqDetail;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPaymentCheqDetail"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentCheqDetailStructure(DataSet dsPaymentCheqDetail, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetPaymentCheqDetailList", dsPaymentCheqDetail, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}