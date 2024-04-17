using System;
using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentDetailAmt : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentDetailAmt()
        {
            SelectCommand = "SELECT * FROM AP.PaymentDetailAmt";
            TableName = "PaymentDetailAmt";
        }

        /// <summary>
        /// </summary>
        /// <param name="paymentDetailNo"></param>
        /// <param name="dsPayment"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentDetailAmtList(int paymentDetailNo, DataSet dsPayment, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@PaymentDetailNo", Convert.ToString(paymentDetailNo));

            // Get data
            var result = DbRetrieve("AP.GetPaymentDetailAmtListByPaymentDetailNo", dsPayment, dbParams, TableName,
                connStr);

            // Return result
            return result;
        }


        /// <summary>
        /// </summary>
        /// <param name="paymentDetailNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetPaymentDetailAmtList(int paymentDetailNo, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@PaymentDetailNo", Convert.ToString(paymentDetailNo));


            DbRetrieve("AP.GetPaymentDetailAmtListByPaymentDetailNo", dsTmp, dbParams, TableName, connStr);

            return dsTmp;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPaymentDetailAmt"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentDetailAmtStructure(DataSet dsPaymentDetailAmt, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetPaymentDetailAmtList", dsPaymentDetailAmt, null, TableName, connStr);

            // Return result
            return result;
        }


        /// <returns></returns>
        public int GetPaymentDetailAmtMax(string voucherNo, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var paymentDetailAmtMax = DbReadScalar("AP.GetPaymentDetailAmtMax", dbParams, connStr);

            // Return result
            return paymentDetailAmtMax;
        }

        #endregion
    }
}