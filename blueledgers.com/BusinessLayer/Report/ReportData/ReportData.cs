using System.Data;
using Blue.DAL;

namespace Blue.BL.Report
{
    public class ReportData : DbHandler
    {
        #region "Operations"

        public ReportData()
        {
            SelectCommand = "SELECT * FROM Report.ReportData";
            TableName = "ReportData";
        }

        /// <summary>
        ///     Get all report data record.
        /// </summary>
        /// <param name="dsReportData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsReportData, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Report.GetReportDataList", dsReportData, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get report data by using report id
        /// </summary>
        /// <param name="dsReportData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsReportData, int reportID, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ReportID", reportID.ToString());

            // Get data
            result = DbRetrieve("Report.GetReportData", dsReportData, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get report data.
        /// </summary>
        /// <returns></returns>
        public byte[] GetReportData(int reportID, string connStr)
        {
            var dsReportData = new DataSet();

            var result = Get(dsReportData, reportID, connStr);

            if (result)
            {
                if (dsReportData.Tables[TableName].Rows.Count > 0)
                {
                    if (dsReportData.Tables[TableName].Rows[0]["ReportData"] != System.DBNull.Value)
                    {
                        return (byte[]) dsReportData.Tables[TableName].Rows[0]["ReportData"];
                    }
                }
            }

            return null;
        }

        public bool Save(DataSet savedData, string connStr)
        {
            // สร้าง SaveSource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);

            // เรียก dbCommit โดยส่ง SaveSource object เป็น parameter
            DbCommit(dbSaveSource, connStr);

            return true;
        }

        #endregion
    }
}