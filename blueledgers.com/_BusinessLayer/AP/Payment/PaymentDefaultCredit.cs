using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentDefaultCredit : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentDefaultCredit()
        {
            SelectCommand = "SELECT * FROM AP.PaymentDefaultCredit";
            TableName = "PaymentDefaultCredit";
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="dsPaymentDefaultCredit"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentDefaultCreditList(string profileCode, DataSet dsPaymentDefaultCredit, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            var result = DbRetrieve("AP.GetPaymentDefaultCreditListByProfileCode", dsPaymentDefaultCredit, dbParams,
                TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPaymentDefaultCreditListByProfileCode(string profileCode, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            var dtAP = DbRead("AP.GetPaymentDefaultCreditListByProfileCode", dbParams, connStr);

            // Return result
            return dtAP;
        }


        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetPaymentDefaultCreditList(string profileCode, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);


            DbRetrieve("AP.GetPaymentDefaultCreditListByProfileCode", dsTmp, dbParams, TableName, connStr);

            return dsTmp;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPaymentDefaultCredit"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentDefaultCreditStructure(DataSet dsPaymentDefaultCredit, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetPaymentDefaultCreditList", dsPaymentDefaultCredit, null, TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     GetPaymentDefaultCreditMaxSeqNo
        /// </summary>
        /// <returns></returns>
        public int GetPaymentDefaultCreditMaxSeqNo(string connStr)
        {
            // Get data
            var result = DbReadScalar("AP.GetPaymentDefaultCreditMaxSeqNo", null, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}