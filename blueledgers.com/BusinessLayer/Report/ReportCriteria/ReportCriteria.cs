using System.Data;
using Blue.DAL;

namespace Blue.BL.Report
{
    public class ReportCriteria : DbHandler
    {
        #region "Operations"

        public ReportCriteria()
        {
            SelectCommand = "SELECT * FROM Report.ReportCriteria";
            TableName = "ReportCriteria";
        }

        /// <summary>
        ///     Get Database Structure of ReportCriteria table.
        /// </summary>
        /// <param name="dsReportCriteria"></param>
        /// <param name="connStr"></param>
        public void GetStructure(DataSet dsReportCriteria, string connStr)
        {
            DbRetrieveSchema("Report.GetReportCriteriaSchema", dsReportCriteria, null, TableName, connStr);
        }

        /// <summary>
        ///     Get all report criteria data
        /// </summary>
        public bool GetList8()
        {
            // Implement code later

            return true;
        }

        /// <summary>
        ///     Get report criteria data by using Report ID.
        /// </summary>
        /// <param name="dsReportCriteria"></param>
        /// <param name="reportID"></param>
        /// <param name="connStr"></param>
        public bool GetList(DataSet dsReportCriteria, int reportID, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ReportID", reportID.ToString());

            return DbRetrieve("Report.GetReportCriteria", dsReportCriteria, dbParams, TableName, connStr);
        }

        #endregion
    }
}