using System.Data;
using System.Globalization;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentDefaultCash : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentDefaultCash()
        {
            SelectCommand = "SELECT * FROM AP.PaymentDefaultCash";
            TableName = "PaymentDefaultCash";
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="dsPaymentDefaultCash"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentDefaultCashList(string profileCode, DataSet dsPaymentDefaultCash, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            var result = DbRetrieve("AP.GetPaymentDefaultCashListByProfileCode", dsPaymentDefaultCash, dbParams,
                TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get paymentdefaultcash by seqNo
        /// </summary>
        /// <param name="seqNo"></param>
        /// <param name="dsPaymentDefaultCash"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentDefaultCashListBySeqNo(int seqNo, DataSet dsPaymentDefaultCash, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@SeqNo", seqNo.ToString(CultureInfo.InvariantCulture));

            // Get data
            var result = DbRetrieve("AP.GetPaymentDefaultCashListBySeqNo", dsPaymentDefaultCash, dbParams, TableName,
                connStr);

            // Return result
            return result;
        }


        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetPaymentDefaultCashList(string profileCode, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);


            DbRetrieve("AP.GetPaymentDefaultCashListByProfileCode", dsTmp, dbParams, TableName, connStr);

            return dsTmp;
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPaymentDefaultCashListByProfileCode(string profileCode, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            var dtAP = DbRead("AP.GetPaymentDefaultCashListByProfileCode", dbParams, connStr);

            // Return result
            return dtAP;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPaymentDefaultCash"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentDefaultCashStructure(DataSet dsPaymentDefaultCash, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetPaymentDefaultCashList", dsPaymentDefaultCash, null, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        ///     GetPaymentDefaultCashMaxSeqNo
        /// </summary>
        /// <returns></returns>
        public int GetPaymentDefaultCashMaxSeqNo(string connStr)
        {
            // Get data
            var result = DbReadScalar("AP.GetPaymentDefaultCashMaxSeqNo", null, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}