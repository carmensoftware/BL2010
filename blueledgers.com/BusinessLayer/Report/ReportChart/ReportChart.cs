using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Report
{
    public class ReportChart : DbHandler
    {
        #region "Operations"

        public ReportChart()
        {
            SelectCommand = "SELECT * FROM Report.ReportChart";
            TableName = "ReportChart";
        }

        /// <summary>
        ///     Get all data
        /// </summary>
        /// <param name="dsReportChart"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetReportChartList(DataSet dsReportChart, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Report.GetReportChartList", dsReportChart, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get Data
        /// </summary>
        /// <param name="dsReport"></param>
        /// <param name="reportID"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetReportChart(DataSet dsReportChart, int reportID, string connStr)
        {
            try
            {
                // Paramter value assign to dbparameter array.
                var dbParams = new DbParameter[1];
                dbParams[0] = new DbParameter("@ReportID", Convert.ToString(reportID));

                return DbRetrieve("Report.GetReportChart", dsReportChart, dbParams, TableName, connStr);
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                return false;
            }
        }

        /// <summary>
        ///     Get schema at table reportchart
        /// </summary>
        /// <param name="dsReportChart"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        public void GetReportChartSchema(DataSet dsReportChart, string connStr)
        {
            DbRetrieveSchema("Report.GetReportChartSchema", dsReportChart, null, TableName, connStr);
        }

        /// <summary>
        ///     Get max field reportchartid
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public int GetNewReportChartID(string connStr)
        {
            var dsTmp = new DataSet();
            DbRetrieve("Report.GetReportChartIDMax", dsTmp, null, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows[0]["ReportChartID"].ToString() != string.Empty)
            {
                return (int) dsTmp.Tables[TableName].Rows[0]["ReportChartID"];
            }
            return 1;
        }

        #endregion
    }
}