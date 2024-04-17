using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentReport : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentReport()
        {
            SelectCommand = "SELECT * FROM AP.PaymentReport";
            TableName = "PaymentReport";
        }

        /// <summary>
        ///     Get  report list
        /// </summary>
        /// <param name="dsPaymentReport"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentReportList(DataSet dsPaymentReport, string connStr)
        {
            // Get data
            var result = DbRetrieve("AP.GetPaymentReportList", dsPaymentReport, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}