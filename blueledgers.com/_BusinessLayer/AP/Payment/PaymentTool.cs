using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentTool : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentTool()
        {
            SelectCommand = "SELECT * FROM AP.PaymentTool";
            TableName = "PaymentTool";
        }

        /// <summary>
        ///     Get tool list
        /// </summary>
        /// <param name="dsPaymentReport"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentToolList(DataSet dsPaymentReport, string connStr)
        {
            // Get data
            var result = DbRetrieve("AP.GetPaymentToolList", dsPaymentReport, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}