using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Report
{
    public class ReportTableColumn : DbHandler
    {
        #region " Attributies "

        #endregion

        #region " Operations "

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public ReportTableColumn()
        {
            SelectCommand = "SELECT * FROM Report.ReportTableColumn";
            TableName = "ReportTableColumn";
        }

        /// <summary>
        ///     Get all of reporttablecolumn list
        /// </summary>
        /// <param name="dsDataSource"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsReportTableColumn, string connStr)
        {
            var result = false;
            result = DbRetrieve("Report.GetReportTableColumnList", dsReportTableColumn, null, TableName, connStr);
            return result;
        }

        /// <summary>
        ///     Get all of reporttablecolumn list by reportTableID.
        /// </summary>
        /// <param name="dataSourceID"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetListByReportTableID(DataSet dsReportTableColumn, int reportTableID, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@reportTableID", Convert.ToString(reportTableID));

            result = DbRetrieve("Report.GetReportTableColumnListByReportTableID", dsReportTableColumn, dbParams,
                TableName, connStr);
            return result;
        }

        /// <summary>
        ///     Get schema table report table column
        /// </summary>
        /// <param name="dsReport"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        public void GetReportTableColumnSchema(DataSet dsReportTableColumn, string connStr)
        {
            DbRetrieveSchema("Report.GetReportTableColumnList", dsReportTableColumn, null, TableName, connStr);
        }

        /// <summary>
        ///     Get max field columnno.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public int GetNewColumnNo(string connStr)
        {
            var dsTmp = new DataSet();
            DbRetrieve("Report.GetColumnNoMax", dsTmp, null, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows[0]["ColumnNo"].ToString() != string.Empty)
            {
                return (int) dsTmp.Tables[TableName].Rows[0]["ColumnNo"];
            }
            return 1;
        }

        #endregion
    }
}