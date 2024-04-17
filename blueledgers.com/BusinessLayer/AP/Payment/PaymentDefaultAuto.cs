using System.Data;
using System.Globalization;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentDefaultAuto : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentDefaultAuto()
        {
            SelectCommand = "SELECT * FROM AP.PaymentDefaultAuto";
            TableName = "PaymentDefaultAuto";
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="dsPaymentDefaultAuto"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentDefaultAutoList(string profileCode, DataSet dsPaymentDefaultAuto, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            var result = DbRetrieve("AP.GetPaymentDefaultAutoListByProfileCode", dsPaymentDefaultAuto, dbParams,
                TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get paymentdefaultauto by seqNo
        /// </summary>
        /// <param name="dsPaymentDefaultAuto"></param>
        /// <param name="connStr"></param>
        /// <param name="seqNo"></param>
        /// <returns></returns>
        public bool GetPaymentDefaultAutoListBySeqNo(int seqNo, DataSet dsPaymentDefaultAuto, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@SeqNo", seqNo.ToString(CultureInfo.InvariantCulture));

            // Get data
            var result = DbRetrieve("[AP].GetPaymentDefaultAutoListBySeqNo", dsPaymentDefaultAuto, dbParams, TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetPaymentDefaultAutoList(string profileCode, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);


            DbRetrieve("AP.GetPaymentDefaultAutoListByProfileCode", dsTmp, dbParams, TableName, connStr);

            return dsTmp;
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPaymentDefaultAutoListByProfileCode(string profileCode, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            var dtAP = DbRead("AP.GetPaymentDefaultAutoListByProfileCode", dbParams, connStr);

            // Return result
            return dtAP;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPaymentDefaultAuto"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentDefaultAutoStructure(DataSet dsPaymentDefaultAuto, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetPaymentDefaultAutoList", dsPaymentDefaultAuto, null, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        ///     GetPaymentDefaultAutoMaxSeqNo
        /// </summary>
        /// <returns></returns>
        public int GetPaymentDefaultAutoMaxSeqNo(string connStr)
        {
            // Get data
            var result = DbReadScalar("AP.GetPaymentDefaultAutoMaxSeqNo", null, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}