using System.Data;
using Blue.DAL;

namespace Blue.BL.Report
{
    public class ChartType : DbHandler
    {
        #region "Operations"

        public ChartType()
        {
            SelectCommand = "SELECT * FROM Report.ChartType";
            TableName = "ChartType";
        }

        /// <summary>
        ///     Get chart type data by active.
        /// </summary>
        /// <returns></returns>
        public DataTable GetActiveList(string connStr)
        {
            return DbRead("Report.GetChartTypeActiveList", null, connStr);
        }

        #endregion
    }
}