using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Report
{
    public class Report : DbHandler
    {
        #region "Operations"

        public Report()
        {
            SelectCommand = "SELECT * FROM Report.Report";
            TableName = "Report";
        }

        /// <summary>
        ///     This function is used for get all Report.Report data that related to specified CategoryID by
        ///     using stored procedure "Report.GetReportList_CategoryID".
        /// </summary>
        /// <param name="dsTrialBalLst"></param>
        /// <param name="catCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsTrialBalLst, string catCode, string connStr)
        {
            var result = false;

            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CategoryID", catCode);

            // Get data
            result = DbRetrieve("Report.GetReportList_CategoryID", dsTrialBalLst, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsTrialBalLst"></param>
        /// <param name="catCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListForDashBoard(DataSet dsReport, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Report.GetReportListByReportTypeID", dsReport, null, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        ///     Get schema from table report.
        /// </summary>
        /// <param name="dsReport"></param>
        public void GetReportSchema(DataSet dsReport, string connStr)
        {
            DbRetrieveSchema("Report.GetReportList", dsReport, null, TableName, connStr);
        }

        /// <summary>
        ///     Get max value at field reportid
        /// </summary>
        /// <returns></returns>
        public int GetNewReportID(string connStr)
        {
            var dsTmp = new DataSet();
            DbRetrieve("Report.GetReportIDMax", dsTmp, null, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows[0]["ReportID"].ToString() != string.Empty)
            {
                return (int) dsTmp.Tables[TableName].Rows[0]["ReportID"];
            }
            return 1;
        }

        /// <summary>
        ///     Get data from store procedure reportgetliststandard
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <param name="dsReport"></param>
        /// <returns></returns>
        public bool GetListStandard(DataSet dsReport, string categoryID, string connStr)
        {
            var result = false;

            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CategoryID", categoryID);

            // Get data
            result = DbRetrieve("Report.GetReportListStandard", dsReport, dbParams, "GLRptStd", connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get data from store procedure reportgetlistcustomize
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <param name="dsReport"></param>
        /// <returns></returns>
        public bool GetListCustomize(DataSet dsReport, string categoryID, string connStr)
        {
            var result = false;

            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CategoryID", categoryID);

            // Get data
            result = DbRetrieve("Report.GetReportListCustomize", dsReport, dbParams, "GLRptCus", connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get report data using ReportID
        /// </summary>
        /// <param name="dsReport"></param>
        /// <param name="reportID"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetReport(DataSet dsReport, int reportID, string connStr)
        {
            try
            {
                // Paramter value assign to dbparameter array.
                var dbParams = new DbParameter[1];
                dbParams[0] = new DbParameter("@ReportID", reportID.ToString());

                return DbRetrieve("Report.GetReportByReportID", dsReport, dbParams, "Report", connStr);
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                return false;
            }
        }

        /// <summary>
        ///     Get report data.
        /// </summary>
        /// <param name="dsReport"></param>
        /// <param name="parameters"></param>
        /// <param name="conStr"></param>
        public DataTable GetReportData(DataSet dsReport, string[,] param, string conStr)
        {
            var dataSource = new DataSource();
            var reportCriteria = new ReportCriteria();

            var parameterList = string.Empty;

            // Select command
            var cmdSelect = dsReport.Tables[dataSource.TableName].Rows[0]["SelectCommand"].ToString();

            // Select where clause
            var cmdWhere = string.Empty;
            var dbParam = new DbParameter[dsReport.Tables[reportCriteria.TableName].Rows.Count];

            for (var i = 0; i < dsReport.Tables[reportCriteria.TableName].Rows.Count; i++)
            {
                parameterList += ", " + param[i, 2] + " AS " + param[i, 0] + i;
                cmdWhere += param[i, 0] + " " + param[i, 1] + " " + param[i, 2] + " " + param[i, 4] + " ";

                // Add command parameters
                dbParam[i] = new DbParameter(param[i, 2], param[i, 3]);
            }

            if (parameterList != string.Empty)
            {
                cmdSelect = cmdSelect.Insert(cmdSelect.IndexOf("FROM") - 1, parameterList);
            }

            if (cmdWhere != string.Empty)
            {
                // Command included where clause.
                if (cmdSelect.ToUpper().IndexOf("WHERE") > 0)
                {
                    // Command included order clause.
                    if (cmdSelect.ToUpper().IndexOf("ORDER BY") > 0)
                    {
                        cmdSelect = cmdSelect.Insert(cmdSelect.IndexOf("ORDER BY") - 1, " AND " + cmdWhere);
                    }
                        // Command not included order clause.
                    else
                    {
                        cmdSelect += " AND " + cmdWhere;
                    }
                }
                    // Command not included where clause.   
                else
                {
                    // Command included order clause.
                    if (cmdSelect.ToUpper().IndexOf("ORDER BY") > 0)
                    {
                        cmdSelect = cmdSelect.Insert(cmdSelect.IndexOf("ORDER BY") - 1, " WHERE " + cmdWhere);
                    }
                        // Command not included order clause.
                    else
                    {
                        cmdSelect += " WHERE " + cmdWhere;
                    }
                }
            }

            // Return result
            return DbExecuteQuery(cmdSelect, dbParam, conStr);
        }

        /// <summary>
        ///     Save data
        /// </summary>
        /// <param name="dsReport"></param>
        /// <returns></returns>
        public bool Save(DataSet dsReport, string connStr)
        {
            var reportCriteria = new BL.Report.ReportCriteria();
            var result = false;
            var reportTypeID = (int) dsReport.Tables["Report"].Rows[0]["ReportTypeID"];

            switch (reportTypeID)
            {
                case 1:
                    var reportChart = new BL.Report.ReportChart();
                    var reportChartSeries = new BL.Report.ReportChartSeries();

                    // Create DbSaveSource
                    var dbSaveSource = new DbSaveSource[4];
                    dbSaveSource[0] = new DbSaveSource(dsReport, SelectCommand, TableName);
                    dbSaveSource[1] = new DbSaveSource(dsReport, reportCriteria.SelectCommand, reportCriteria.TableName);
                    dbSaveSource[2] = new DbSaveSource(dsReport, reportChart.SelectCommand, reportChart.TableName);
                    dbSaveSource[3] = new DbSaveSource(dsReport, reportChartSeries.SelectCommand,
                        reportChartSeries.TableName);

                    // Commit to database
                    result = DbCommit(dbSaveSource, connStr);
                    break;

                case 2:
                    var reportTable = new BL.Report.ReportTable();
                    var reportTableColumn = new ReportTableColumn();
                    var reportTableSummary = new ReportTableSummary();

                    // Create DbSaveSource
                    var dbSaveSourceTable = new DbSaveSource[5];
                    dbSaveSourceTable[0] = new DbSaveSource(dsReport, SelectCommand, TableName);
                    dbSaveSourceTable[1] = new DbSaveSource(dsReport, reportCriteria.SelectCommand,
                        reportCriteria.TableName);
                    dbSaveSourceTable[2] = new DbSaveSource(dsReport, reportTable.SelectCommand, reportTable.TableName);
                    dbSaveSourceTable[3] = new DbSaveSource(dsReport, reportTableColumn.SelectCommand,
                        reportTableColumn.TableName);
                    dbSaveSourceTable[4] = new DbSaveSource(dsReport, reportTableSummary.SelectCommand,
                        reportTableSummary.TableName);

                    // Commit to database
                    result = DbCommit(dbSaveSourceTable, connStr);
                    break;

                case 3:
                    var reportPivot = new ReportPivot();
                    var reportPivotField = new ReportPivotField();

                    // Create DbSaveSource
                    var dbSaveSourcePivot = new DbSaveSource[4];
                    dbSaveSourcePivot[0] = new DbSaveSource(dsReport, SelectCommand, TableName);
                    dbSaveSourcePivot[1] = new DbSaveSource(dsReport, reportCriteria.SelectCommand,
                        reportCriteria.TableName);
                    dbSaveSourcePivot[2] = new DbSaveSource(dsReport, reportPivot.SelectCommand, reportPivot.TableName);
                    dbSaveSourcePivot[3] = new DbSaveSource(dsReport, reportPivotField.SelectCommand,
                        reportPivotField.TableName);

                    // Commit to database
                    result = DbCommit(dbSaveSourcePivot, connStr);
                    break;

                case 4:
                    var reportSpreadSheet = new ReportSpreadSheet();
                    var reportSpreadSheetField = new ReportSpreadSheetField();

                    // Create DbSaveSource.
                    var dbSaveSourceSpreadSheet = new DbSaveSource[4];
                    dbSaveSourceSpreadSheet[0] = new DbSaveSource(dsReport, SelectCommand, TableName);
                    dbSaveSourceSpreadSheet[1] = new DbSaveSource(dsReport, reportCriteria.SelectCommand,
                        reportCriteria.TableName);
                    dbSaveSourceSpreadSheet[2] = new DbSaveSource(dsReport, reportSpreadSheet.SelectCommand,
                        reportSpreadSheet.TableName);
                    dbSaveSourceSpreadSheet[3] = new DbSaveSource(dsReport, reportSpreadSheetField.SelectCommand,
                        reportSpreadSheetField.TableName);

                    // Commit to database
                    result = DbCommit(dbSaveSourceSpreadSheet, connStr);

                    break;

                case 5:
                    // Save data to table report data.
                    var reportData = new ReportData();

                    // Create DbSaveSource.
                    //DbSaveSource[] dbSaveData   = new DbSaveSource[dsReport.Tables.Count];
                    var dbSaveData = new DbSaveSource[2];
                    dbSaveData[0] = new DbSaveSource(dsReport, SelectCommand, TableName);
                    dbSaveData[1] = new DbSaveSource(dsReport, reportCriteria.SelectCommand, reportCriteria.TableName);
                    //dbSaveData[2]               = new DbSaveSource(dsReport, reportData.SelectCommand, reportData.TableName);
                    //if (dsReport.Tables[reportCriteria.TableName] != null)
                    //{
                    //    dbSaveData[1] = new DbSaveSource(dsReport, reportCriteria.SelectCommand, reportCriteria.TableName);
                    //}

                    //if (dsReport.Tables[reportData.TableName] != null)
                    //{
                    //    dbSaveData[2] = new DbSaveSource(dsReport, reportData.SelectCommand, reportData.TableName);
                    //}

                    // Commit to database
                    result = DbCommit(dbSaveData, connStr);

                    break;
            }

            // Return result
            return result;
        }

        /// <summary>
        ///     Save row in database
        /// </summary>
        /// <param name="dsReport"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsReport, int reportTypeID, string connStr)
        {
            var reportCriteria = new BL.Report.ReportCriteria();
            var result = false;

            switch (reportTypeID)
            {
                case 1:
                    // Save data to table report chart and report chart series.
                    var reportChart = new BL.Report.ReportChart();
                    var reportChartSeries = new BL.Report.ReportChartSeries();

                    // Create DbSaveSource
                    var dbSaveSource = new DbSaveSource[4];
                    dbSaveSource[0] = new DbSaveSource(dsReport, reportChartSeries.SelectCommand,
                        reportChartSeries.TableName);
                    dbSaveSource[1] = new DbSaveSource(dsReport, reportChart.SelectCommand, reportChart.TableName);
                    dbSaveSource[2] = new DbSaveSource(dsReport, reportCriteria.SelectCommand, reportCriteria.TableName);
                    dbSaveSource[3] = new DbSaveSource(dsReport, SelectCommand, TableName);

                    // Commit to database
                    result = DbCommit(dbSaveSource, connStr);
                    break;

                case 2:
                    // Save data to table report table, report table column and report table summary.
                    var reportTable = new BL.Report.ReportTable();
                    var reportTableColumn = new ReportTableColumn();
                    var reportTableSummary = new ReportTableSummary();

                    // Create DbSaveSource
                    var dbSaveSourceTable = new DbSaveSource[5];
                    dbSaveSourceTable[0] = new DbSaveSource(dsReport, reportTableSummary.SelectCommand,
                        reportTableSummary.TableName);
                    dbSaveSourceTable[1] = new DbSaveSource(dsReport, reportTableColumn.SelectCommand,
                        reportTableColumn.TableName);
                    dbSaveSourceTable[2] = new DbSaveSource(dsReport, reportTable.SelectCommand, reportTable.TableName);
                    dbSaveSourceTable[3] = new DbSaveSource(dsReport, reportCriteria.SelectCommand,
                        reportCriteria.TableName);
                    dbSaveSourceTable[4] = new DbSaveSource(dsReport, SelectCommand, TableName);

                    // Commit to database
                    result = DbCommit(dbSaveSourceTable, connStr);
                    break;

                case 3:
                    // Save data to table report pivot and report pivot field.
                    var reportPivot = new ReportPivot();
                    var reportPivotField = new ReportPivotField();

                    // Create DbSaveSource
                    var dbSaveSourcePivot = new DbSaveSource[4];
                    dbSaveSourcePivot[0] = new DbSaveSource(dsReport, reportPivotField.SelectCommand,
                        reportPivotField.TableName);
                    dbSaveSourcePivot[1] = new DbSaveSource(dsReport, reportPivot.SelectCommand, reportPivot.TableName);
                    dbSaveSourcePivot[2] = new DbSaveSource(dsReport, reportCriteria.SelectCommand,
                        reportCriteria.TableName);
                    dbSaveSourcePivot[3] = new DbSaveSource(dsReport, SelectCommand, TableName);

                    // Commit to database
                    result = DbCommit(dbSaveSourcePivot, connStr);
                    break;

                case 4:
                    // Save data to table report spread sheet and report spread sheet field.
                    var reportSpreadSheet = new ReportSpreadSheet();
                    var reportSpreadSheetField = new ReportSpreadSheetField();

                    // Create DbSaveSource.
                    var dbSaveSourceSpreadSheet = new DbSaveSource[4];
                    dbSaveSourceSpreadSheet[0] = new DbSaveSource(dsReport, reportSpreadSheetField.SelectCommand,
                        reportSpreadSheetField.TableName);
                    dbSaveSourceSpreadSheet[1] = new DbSaveSource(dsReport, reportSpreadSheet.SelectCommand,
                        reportSpreadSheet.TableName);
                    dbSaveSourceSpreadSheet[2] = new DbSaveSource(dsReport, reportCriteria.SelectCommand,
                        reportCriteria.TableName);
                    dbSaveSourceSpreadSheet[3] = new DbSaveSource(dsReport, SelectCommand, TableName);

                    // Commit to database
                    result = DbCommit(dbSaveSourceSpreadSheet, connStr);

                    break;

                case 5:
                    // Save data to table report data.
                    var reportData = new ReportData();

                    // Create DbSaveSource.
                    var dbSaveSourceData = new DbSaveSource[3];
                    dbSaveSourceData[0] = new DbSaveSource(dsReport, reportData.SelectCommand, reportData.TableName);
                    dbSaveSourceData[1] = new DbSaveSource(dsReport, reportCriteria.SelectCommand,
                        reportCriteria.TableName);
                    dbSaveSourceData[2] = new DbSaveSource(dsReport, SelectCommand, TableName);

                    // Commit to database
                    result = DbCommit(dbSaveSourceData, connStr);

                    break;
            }

            // Return result
            return result;
        }

        /// <summary>
        ///     Get data from query in datasource table related to specified report id.
        /// </summary>
        /// <param name="dsReport"></param>
        /// <param name="ReportID"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSourceData(DataSet dsReport, int ReportID, string ConnectionString)
        {
            var cmdSelect = string.Empty;
            var dbParams = new DbParameter[1];

            // Get query string
            // Create parameters
            dbParams[0] = new DbParameter("@ReportID", ReportID.ToString());

            var dtDataSource = DbRead("Report.GetSourceData", dbParams, ConnectionString);

            if (dtDataSource != null)
            {
                if (dtDataSource.Rows.Count > 0)
                {
                    cmdSelect = dtDataSource.Rows[0]["SelectCommand"].ToString();

                    // Get data
                    var dtSourceData = new DataTable();
                    dtDataSource = DbExecuteQuery(cmdSelect, null, ConnectionString);
                    dtDataSource.TableName = TableName;

                    if (dtDataSource != null)
                    {
                        dsReport.Tables.Add(dtDataSource);
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        public string GetNewReportID(DataSet dsDataReport, int ReportID, string p)
        {
            throw new NotImplementedException();
        }
    }
}