using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Report
{
    public class ReportPivot : DbHandler
    {
        #region " Attributies "

        #endregion

        #region " Operations "

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public ReportPivot()
        {
            SelectCommand = "SELECT * FROM Report.ReportPivot";
            TableName = "ReportPivot";
        }

        /// <summary>
        ///     Get all of reportpivot list
        /// </summary>
        /// <param name="dsDataSource"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsReportPivot, string connStr)
        {
            var result = false;
            result = DbRetrieve("Report.GetReportPivotList", dsReportPivot, null, TableName, connStr);
            return result;
        }

        /// <summary>
        ///     Get all of reportpivot list by reportID.
        /// </summary>
        /// <param name="dataSourceID"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetListByReportID(DataSet dsReportPivot, int reportID, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@reportID", Convert.ToString(reportID));

            result = DbRetrieve("Report.GetReportPivotListByReportID", dsReportPivot, dbParams, TableName, connStr);
            return result;
        }

        /// <summary>
        ///     Get max field fieldno.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public int GetNewReportPivotID(string connStr)
        {
            var dsTmp = new DataSet();
            DbRetrieve("Report.GetReportPivotIDMax", dsTmp, null, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows[0]["ReportPivotID"].ToString() != string.Empty)
            {
                return (int) dsTmp.Tables[TableName].Rows[0]["ReportPivotID"];
            }
            return 1;
        }

        /// <summary>
        ///     Get schema table reportpivot
        /// </summary>
        /// <param name="dsReport"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        public void GetReportPivotSchema(DataSet dsReportPivot, string connStr)
        {
            DbRetrieveSchema("Report.GetReportPivotList", dsReportPivot, null, TableName, connStr);
        }

        #endregion
    }
}