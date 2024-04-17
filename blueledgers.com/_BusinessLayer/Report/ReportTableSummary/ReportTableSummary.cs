using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Report
{
    public class ReportTableSummary : DbHandler
    {
        #region " Attributies "

        #endregion

        #region " Operations "

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public ReportTableSummary()
        {
            SelectCommand = "SELECT * FROM Report.ReportTableSummary";
            TableName = "ReportTableSummary";
        }

        /// <summary>
        ///     Get all of reporttablesummary list
        /// </summary>
        /// <param name="dsDataSource"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsReportTableSummary, string connStr)
        {
            var result = false;
            result = DbRetrieve("Report.GetReportTableSummaryList", dsReportTableSummary, null, TableName, connStr);
            return result;
        }

        /// <summary>
        ///     Get all of reportablesummary list by reportID.
        /// </summary>
        /// <param name="dataSourceID"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetListByReportTableID(DataSet dsReportTableSummary, int reportTableID, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@reportTableID", Convert.ToString(reportTableID));

            result = DbRetrieve("Report.GetReportTableSummaryListByReportTableID", dsReportTableSummary, dbParams,
                TableName, connStr);
            return result;
        }

        /// <summary>
        ///     Get schema table report table summary.
        /// </summary>
        /// <param name="dsReportTableColumn"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        public void GetReportTableSummarySchema(DataSet dsReportTableSummary, string connStr)
        {
            DbRetrieveSchema("Report.GetReportTableSummaryList", dsReportTableSummary, null, TableName, connStr);
        }

        /// <summary>
        ///     Get max field summaryno.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public int GetNewSummaryNo(string connStr)
        {
            var dsTmp = new DataSet();
            DbRetrieve("Report.GetSummaryNoMax", dsTmp, null, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows[0]["SummaryNo"].ToString() != string.Empty)
            {
                return (int) dsTmp.Tables[TableName].Rows[0]["SummaryNo"];
            }
            return 1;
        }

        #endregion
    }
}