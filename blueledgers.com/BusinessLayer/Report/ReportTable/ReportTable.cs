using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Report
{
    public class ReportTable : DbHandler
    {
        #region " Attributies "

        #endregion

        #region " Operations "

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public ReportTable()
        {
            SelectCommand = "SELECT * FROM Report.ReportTable";
            TableName = "ReportTable";
        }

        /// <summary>
        ///     Get all of reporttable list
        /// </summary>
        /// <param name="dsDataSource"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsReportTable, string connStr)
        {
            var result = false;
            result = DbRetrieve("Report.GetReportTableList", dsReportTable, null, TableName, connStr);
            return result;
        }

        /// <summary>
        ///     Get all of reportable list by reportID.
        /// </summary>
        /// <param name="dataSourceID"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetListByReportID(DataSet dsReportTable, int reportID, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@reportID", Convert.ToString(reportID));

            result = DbRetrieve("Report.GetReportTableListByReportID", dsReportTable, dbParams, TableName, connStr);
            return result;
        }

        /// <summary>
        ///     Get schema table reporttable
        /// </summary>
        /// <param name="dsReport"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        public void GetReportTableSchema(DataSet dsReportTable, string connStr)
        {
            DbRetrieveSchema("Report.GetReportTableList", dsReportTable, null, TableName, connStr);
        }

        /// <summary>
        ///     Get max field reporttableid
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public int GetNewReportTableID(string connStr)
        {
            var dsTmp = new DataSet();
            DbRetrieve("Report.GetReportTableIDMax", dsTmp, null, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows[0]["ReportTableID"].ToString() != string.Empty)
            {
                return (int) dsTmp.Tables[TableName].Rows[0]["ReportTableID"];
            }
            return 1;
        }

        #endregion
    }
}