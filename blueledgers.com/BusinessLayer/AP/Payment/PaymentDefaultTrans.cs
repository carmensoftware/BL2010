using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentDefaultTrans : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentDefaultTrans()
        {
            SelectCommand = "SELECT * FROM AP.PaymentDefaultTrans";
            TableName = "PaymentDefaultTrans";
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="dsPaymentDefaultTrans"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentDefaultTransList(string profileCode, DataSet dsPaymentDefaultTrans, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            var result = DbRetrieve("AP.GetPaymentDefaultTransListByProfileCode", dsPaymentDefaultTrans, dbParams,
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
        public DataTable GetPaymentDefaultTransListByProfileCode(string profileCode, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            var dtAP = DbRead("AP.GetPaymentDefaultTransListByProfileCode", dbParams, connStr);

            // Return result
            return dtAP;
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetPaymentDefaultTransList(string profileCode, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);


            DbRetrieve("AP.GetPaymentDefaultTransListByProfileCode", dsTmp, dbParams, TableName, connStr);

            return dsTmp;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPaymentDefaultTrans"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentDefaultTransStructure(DataSet dsPaymentDefaultTrans, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetPaymentDefaultTransList", dsPaymentDefaultTrans, null, TableName,
                connStr);

            // Return result
            return result;
        }


        /// <summary>
        ///     GetPaymentDefaultTransMaxSeqNo
        /// </summary>
        /// <returns></returns>
        public int GetPaymentDefaultTransMaxSeqNo(string connStr)
        {
            // Get data
            var result = DbReadScalar("AP.GetPaymentDefaultTransMaxSeqNo", null, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}