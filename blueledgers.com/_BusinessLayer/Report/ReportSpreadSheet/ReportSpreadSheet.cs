using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Report
{
    public class ReportSpreadSheet : DbHandler
    {
        #region " Attributies "

        #endregion

        #region " Operations "

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public ReportSpreadSheet()
        {
            SelectCommand = "SELECT * FROM Report.ReportSpreadSheet";
            TableName = "ReportSpreadSheet";
        }

        /// <summary>
        ///     Get all of ReportSpreadSheet list
        /// </summary>
        /// <param name="dsDataSource"></param>
        /// <param name="connStr"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsReportSpreadSheet, string connStr)
        {
            var result = false;
            result = DbRetrieve("Report.GetReportSpreadSheetList", dsReportSpreadSheet, null, TableName, connStr);
            return result;
        }

        /// <summary>
        ///     Get all of ReportSpreadSheet list by reportID.
        /// </summary>
        /// <param name="dataSourceID"></param>
        /// <param name="connStr"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetListByReportID(DataSet dsReportSpreadSheet, int reportID, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@reportID", Convert.ToString(reportID));

            result = DbRetrieve("Report.GetReportSpreadSheetListByReportID", dsReportSpreadSheet, dbParams, TableName,
                connStr);
            return result;
        }

        /// <summary>
        ///     Get schema table reportspreadsheet.
        /// </summary>
        /// <param name="dsReport"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        public void GetReportSpreadSheetSchema(DataSet dsReportSpreadSheet, string connStr)
        {
            DbRetrieveSchema("Report.GetReportSpreadSheetList", dsReportSpreadSheet, null, TableName, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewReportSpreadSheetID(string connStr)
        {
            var dsTmp = new DataSet();
            DbRetrieve("Report.GetReportSpreadSheetIDMax", dsTmp, null, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows[0]["ReportSpreadSheetID"].ToString() != string.Empty)
            {
                return (int) dsTmp.Tables[TableName].Rows[0]["ReportSpreadSheetID"];
            }
            return 1;
        }

        /// <summary>
        ///     Save changes to database.
        /// </summary>
        /// <param name="dsDataSource"></param>
        /// <param name="connStr"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool Save(DataSet dsReportSpreadSheet, string connStr)
        {
            var result = false;

            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsReportSpreadSheet, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}