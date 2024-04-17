using System.Data;
using System.Globalization;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentDefault : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentDefault()
        {
            SelectCommand = "SELECT * FROM AP.PaymentDefault";
            TableName = "PaymentDefault";
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="dsPaymentDefault"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentDefaultList(string profileCode, DataSet dsPaymentDefault, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            var result = DbRetrieve("AP.GetPaymentDefaultListByProfileCode", dsPaymentDefault, dbParams, TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPaymentDefaultListByProfileCode(string profileCode, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            var dtAP = DbRead("AP.GetPaymentDefaultListByProfileCode", dbParams, connStr);

            // Return result
            return dtAP;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPaymentDefault"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentDefaultStructure(DataSet dsPaymentDefault, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetPaymentDefaultList", dsPaymentDefault, null, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        ///     Get payment default  max Id
        /// </summary>
        /// <returns></returns>
        public int GetPaymentDefaultMaxSeq(string connStr)
        {
            // Get data
            var result = DbReadScalar("AP.GetPaymentDefaultMaxSeq", null, connStr);

            // Return result
            return result;
        }


        /// <summary>
        ///     GetPaymentMethodCode seqNo.
        /// </summary>
        /// <param name="seqNo"></param>
        /// <param name="conStr"></param>
        /// <returns></returns>
        public string GetPaymentMethodCode(int seqNo, string conStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@SeqNo", seqNo.ToString(CultureInfo.InvariantCulture));

            // Get data
            var dtPaymentDefault = DbRead("AP.GetPaymentMethodCodeBySeqNO", dbParams, conStr);

            // Return result            
            return dtPaymentDefault.Rows[0]["PaymentMethodCode"].ToString();
        }

        #endregion
    }
}