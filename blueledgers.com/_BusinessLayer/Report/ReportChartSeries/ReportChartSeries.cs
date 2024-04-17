using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Report
{
    public class ReportChartSeries : DbHandler
    {
        #region "Operations"

        public ReportChartSeries()
        {
            SelectCommand = "SELECT * FROM Report.ReportChartSeries";
            TableName = "ReportChartSeries";
        }

        /// <summary>
        ///     Get all data
        /// </summary>
        /// <param name="dsReport"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetReportChartSeriesList(DataSet dsReportChartSeries, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Report.GetReportChartSeriesList", dsReportChartSeries, null, TableName, connStr);

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
        public bool GetReportChartSeries(DataSet dsReportChart, string reportChartID, string connStr)
        {
            try
            {
                // Paramter value assign to dbparameter array.
                var dbParams = new DbParameter[1];
                dbParams[0] = new DbParameter("@ReportChartID", reportChartID);

                return DbRetrieve("Report.GetReportChartSeries", dsReportChart, dbParams, TableName, connStr);
            }
            catch (Exception ex)
            {
               LogManager.Error(ex);
                return false;
            }
        }

        /// <summary>
        ///     Get schema at table reportchartseries
        /// </summary>
        /// <param name="dsReportChart"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        public void GetReportChartSeriesSchema(DataSet dsReportChartSeries, string connStr)
        {
            DbRetrieveSchema("Report.GetReportChartSeriesSchema", dsReportChartSeries, null, TableName, connStr);
        }

        /// <summary>
        ///     Get new seriesno
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        public int GetNewSeriesNo44(string connStr)
        {
            var dsTmp = new DataSet();
            DbRetrieve("Report.GetSeriesNoMax", dsTmp, null, TableName, connStr);

            return (int) dsTmp.Tables[TableName].Rows[0]["SeriesNo"];
        }

        /// <summary>
        ///     Save data
        /// </summary>
        /// <param name="dsReportTableColumn"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool Save(DataSet dsReportChart, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsReportChart, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}