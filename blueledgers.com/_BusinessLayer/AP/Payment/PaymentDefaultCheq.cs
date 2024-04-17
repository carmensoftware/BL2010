using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentDefaultCheq : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentDefaultCheq()
        {
            SelectCommand = "SELECT * FROM AP.PaymentDefaultCheq";
            TableName = "PaymentDefaultCheq";
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="dsPaymentDefaultCheq"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentDefaultCheqList(string profileCode, DataSet dsPaymentDefaultCheq, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            var result = DbRetrieve("AP.GetPaymentDefaultCheqListByProfileCode", dsPaymentDefaultCheq, dbParams,
                TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetPaymentDefaultCheqList(string profileCode, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);


            DbRetrieve("AP.GetPaymentDefaultCheqListByProfileCode", dsTmp, dbParams, TableName, connStr);

            return dsTmp;
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPaymentDefaultCheqListByProfileCode(string profileCode, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            var dtPaymentDefaultCheq = DbRead("AP.GetPaymentDefaultCheqListByProfileCode", dbParams, connStr);

            // Return result
            return dtPaymentDefaultCheq;
        }


        /// <summary>
        ///     Get structure.
        /// </summary>
        /// <param name="dsPaymentDefaultCheq"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentDefaultCheqStructure(DataSet dsPaymentDefaultCheq, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetPaymentDefaultCheqList", dsPaymentDefaultCheq, null, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        ///     GetPaymentDefaultCheqMaxSeqNo
        /// </summary>
        /// <returns></returns>
        public int GetPaymentDefaultCheqMaxSeqNo(string connStr)
        {
            // Get data
            var result = DbReadScalar("AP.GetPaymentDefaultCheqMaxSeqNo", null, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}