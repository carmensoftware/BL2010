using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Report
{
    public class ReportPivotField : DbHandler
    {
        #region " Attributies "

        #endregion

        #region " Operations "

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public ReportPivotField()
        {
            SelectCommand = "SELECT * FROM Report.ReportPivotField";
            TableName = "ReportPivotField";
        }

        /// <summary>
        ///     Get all of reportpivotfield list
        /// </summary>
        /// <param name="dsDataSource"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsReportPivotField, string connStr)
        {
            var result = false;
            result = DbRetrieve("Report.GetReportPivotFieldList", dsReportPivotField, null, TableName, connStr);
            return result;
        }

        /// <summary>
        ///     Get all of reportpivotfield list by reportpivotID.
        /// </summary>
        /// <param name="dataSourceID"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetListByReportPivotID(DataSet dsReportPivotField, int reportpivotID, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@reportpivotID", Convert.ToString(reportpivotID));

            result = DbRetrieve("Report.GetReportPivotFieldListByReportPivotID", dsReportPivotField, dbParams, TableName,
                connStr);
            return result;
        }

        /// <summary>
        ///     Get max fieldno
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public int GetNewFieldNo(string reportPivotID, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@reportpivotID", reportPivotID);

            DbRetrieve("Report.GetFieldNoMax", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows[0]["FieldNo"] != DBNull.Value)
            {
                return (int) dsTmp.Tables[TableName].Rows[0]["FieldNo"];
            }
            return 0;
        }

        /// <summary>
        ///     Get schema table report pivot field
        /// </summary>
        /// <param name="dsReport"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        public void GetReportPivotFieldSchema(DataSet dsReportPivotField, string connStr)
        {
            DbRetrieveSchema("Report.GetReportPivotFieldList", dsReportPivotField, null, TableName, connStr);
        }

        #endregion
    }
}