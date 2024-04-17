using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StandardVoucherReport : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public StandardVoucherReport()
        {
            SelectCommand = "SELECT * FROM GL.StandardVoucherReport";
            TableName = "StandardVoucherReport";
        }

        /// <summary>
        ///     Get standard voucher report list
        /// </summary>
        /// <param name="dsStandardVoucherReport"></param>
        /// <returns></returns>
        public bool GetStandardVoucherReportList(DataSet dsStandardVoucherReport, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetStandardVoucherReportList", dsStandardVoucherReport, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}