using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class DebtorReport : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public DebtorReport()
        {
            SelectCommand = "SELECT * FROM AR.DebtorReport";
            TableName = "DebtorReport";
        }

        /// <summary>
        ///     Get debtor report list
        /// </summary>
        /// <param name="dsVendorReport"></param>
        /// <returns></returns>
        public bool GetDebtorReportList(DataSet dsDebtorReport, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("AR.GetDebtorReportList", dsDebtorReport, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}