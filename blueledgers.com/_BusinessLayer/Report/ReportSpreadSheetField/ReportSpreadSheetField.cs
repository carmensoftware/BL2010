using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Report
{
    public class ReportSpreadSheetField : DbHandler
    {
        #region " Attributies "

        #endregion

        #region " Operations "

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public ReportSpreadSheetField()
        {
            SelectCommand = "SELECT * FROM Report.ReportSpreadSheetField";
            TableName = "ReportSpreadSheetField";
        }

        /// <summary>
        ///     Get all of ReportSpreadSheet list
        /// </summary>
        /// <param name="dsDataSource"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsReportSpreadSheetField, string connStr)
        {
            var result = false;
            result = DbRetrieve("Report.GetReportSpreadSheetFieldList", dsReportSpreadSheetField, null, TableName,
                connStr);
            return result;
        }

        /// <summary>
        ///     Get all of ReportSpreadSheet list by reportID.
        /// </summary>
        /// <param name="dataSourceID"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetListByReportSpreadSheetID(DataSet dsReportSpreadSheetField, int reportSpreadSheetID,
            string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@reportSpreadSheetID", Convert.ToString(reportSpreadSheetID));

            result = DbRetrieve("Report.GetReportSpreadSheetFieldListByReportSpreadSheetID", dsReportSpreadSheetField,
                dbParams, TableName, connStr);
            return result;
        }


        /// <summary>
        ///     Get schema table reportspreadsheetfield.
        /// </summary>
        /// <param name="dsReport"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        public void GetReportSpreadSheetFieldSchema(DataSet dsReportSpreadSheetField, string connStr)
        {
            DbRetrieveSchema("Report.GetReportSpreadSheetFieldList", dsReportSpreadSheetField, null, TableName, connStr);
        }

        /// <summary>
        ///     Save changes to database.
        /// </summary>
        /// <param name="dsDataSource"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool Save(DataSet dsReportSpreadSheetField, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];


            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsReportSpreadSheetField, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}