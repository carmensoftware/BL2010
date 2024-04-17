using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.RPT
{
    public class Report : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty contructor.
        /// </summary>
        public Report()
        {
            SelectCommand = "SELECT * FROM RPT.[Report]";
            TableName = "Report";
        }

        /// <summary>
        ///     Get report list by specified Module.
        /// </summary>
        /// <param name="dsReport"></param>
        /// <param name="Module"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsReport, string Module, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Module", Module);

            return DbRetrieve("RPT.GetReportListByModule", dsReport, dbParams, TableName, ConnStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsReport"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsReport, string ConnStr)
        {
            return DbRetrieve("RPT.GetReportList", dsReport, null, TableName, ConnStr);
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
            result = DbRetrieve("RPT.GetReportListStandard", dsReport, dbParams, "GLRptStd", connStr);

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
            result = DbRetrieve("RPT.GetReportListCustomize", dsReport, dbParams, "GLRptCus", connStr);

            // Return result
            return result;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsReport"></param>
        /// <param name="reportID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetReport(DataSet dsReport, int reportID, string connStr)
        {
            try
            {
                // Paramter value assign to dbparameter array.
                var dbParams = new DbParameter[1];
                dbParams[0] = new DbParameter("@ReportID", reportID.ToString());

                return DbRetrieve("RPT.GetReportByReportID", dsReport, dbParams, "Report", connStr);
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                return false;
            }
        }

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
            var chkLast = dsReport.Tables[reportCriteria.TableName].Rows.Count - 1;

            for (var i = 0; i < dsReport.Tables[reportCriteria.TableName].Rows.Count; i++)
            {
                if (param[i, 5] == "True")
                {
                    //string[] chk = param[i, 0].ToString().Split('.');

                    if (param[i, 1] == "in")
                    {
                        //cmdWhere += param[i, 0] + " " + param[i, 1] + " (" + param[i, 2] + ") " + param[i, 4] + " ";
                        if (param[i, 3] != "''")
                        {
                            cmdWhere += param[i, 0] + " " + param[i, 1] + " (" + param[i, 3] + ") " + param[i, 4] + " ";
                        }
                    }
                    else
                    {
                        //cmdWhere += param[i, 0] + " " + param[i, 1] + " " + param[i, 2] + " " + param[i, 4] + " ";

                        if (chkLast - 1 == i)
                        {
                            if (param[chkLast, 3] == "''")
                            {
                                cmdWhere += param[i, 0] + " " + param[i, 1] + " " + param[i, 2] + " ";
                            }
                            else
                            {
                                cmdWhere += param[i, 0] + " " + param[i, 1] + " " + param[i, 2] + " " + param[i, 4] +
                                            " ";
                            }
                        }
                        else
                        {
                            cmdWhere += param[i, 0] + " " + param[i, 1] + " " + param[i, 2] + " " + param[i, 4] + " ";
                        }
                    }

                    // Add command parameters
                    dbParam[i] = new DbParameter(param[i, 2], param[i, 3]);
                }
                else //Criteria use False    " where $i=$i "
                {
                    cmdWhere += i + " = " + i + param[i, 4] + " ";
                    dbParam[i] = new DbParameter(i.ToString(), i.ToString());
                }
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
            var reportCriteria = new BL.RPT.ReportCriteria();
            var result = false;
            var reportTypeID = (int) dsReport.Tables["Report"].Rows[0]["Type"];

            switch (reportTypeID)
            {
                    //case 1:
                    //    BL.RPT.ReportChart reportChart = new BL.RPT.ReportChart();
                    //    BL.Report.ReportChartSeries reportChartSeries = new BL.Report.ReportChartSeries();

                    //    // Create DbSaveSource
                    //    DbSaveSource[] dbSaveSource = new DbSaveSource[4];
                    //    dbSaveSource[0] = new DbSaveSource(dsReport, this.SelectCommand, this.TableName);
                    //    dbSaveSource[1] = new DbSaveSource(dsReport, reportCriteria.SelectCommand, reportCriteria.TableName);
                    //    dbSaveSource[2] = new DbSaveSource(dsReport, reportChart.SelectCommand, reportChart.TableName);
                    //    dbSaveSource[3] = new DbSaveSource(dsReport, reportChartSeries.SelectCommand, reportChartSeries.TableName);

                    //    // Commit to database
                    //    result = DbCommit(dbSaveSource, connStr);
                    //    break;

                    //case 2:
                    //    BlueLedger.BL.Report.ReportTable reportTable = new BL.Report.ReportTable();
                    //    BlueLedger.BL.Report.ReportTableColumn reportTableColumn = new BlueLedger.BL.Report.ReportTableColumn();
                    //    BlueLedger.BL.Report.ReportTableSummary reportTableSummary = new BlueLedger.BL.Report.ReportTableSummary();

                    //    // Create DbSaveSource
                    //    DbSaveSource[] dbSaveSourceTable = new DbSaveSource[5];
                    //    dbSaveSourceTable[0] = new DbSaveSource(dsReport, this.SelectCommand, this.TableName);
                    //    dbSaveSourceTable[1] = new DbSaveSource(dsReport, reportCriteria.SelectCommand, reportCriteria.TableName);
                    //    dbSaveSourceTable[2] = new DbSaveSource(dsReport, reportTable.SelectCommand, reportTable.TableName);
                    //    dbSaveSourceTable[3] = new DbSaveSource(dsReport, reportTableColumn.SelectCommand, reportTableColumn.TableName);
                    //    dbSaveSourceTable[4] = new DbSaveSource(dsReport, reportTableSummary.SelectCommand, reportTableSummary.TableName);

                    //    // Commit to database
                    //    result = DbCommit(dbSaveSourceTable, connStr);
                    //    break;

                    //case 3:
                    //    BlueLedger.BL.Report.ReportPivot reportPivot = new BlueLedger.BL.Report.ReportPivot();
                    //    BlueLedger.BL.Report.ReportPivotField reportPivotField = new BlueLedger.BL.Report.ReportPivotField();

                    //    // Create DbSaveSource
                    //    DbSaveSource[] dbSaveSourcePivot = new DbSaveSource[4];
                    //    dbSaveSourcePivot[0] = new DbSaveSource(dsReport, this.SelectCommand, this.TableName);
                    //    dbSaveSourcePivot[1] = new DbSaveSource(dsReport, reportCriteria.SelectCommand, reportCriteria.TableName);
                    //    dbSaveSourcePivot[2] = new DbSaveSource(dsReport, reportPivot.SelectCommand, reportPivot.TableName);
                    //    dbSaveSourcePivot[3] = new DbSaveSource(dsReport, reportPivotField.SelectCommand, reportPivotField.TableName);

                    //    // Commit to database
                    //    result = DbCommit(dbSaveSourcePivot, connStr);
                    //    break;

                    //case 4:
                    //    BlueLedger.BL.Report.ReportSpreadSheet reportSpreadSheet = new BlueLedger.BL.Report.ReportSpreadSheet();
                    //    BlueLedger.BL.Report.ReportSpreadSheetField reportSpreadSheetField = new BlueLedger.BL.Report.ReportSpreadSheetField();

                    //    // Create DbSaveSource.
                    //    DbSaveSource[] dbSaveSourceSpreadSheet = new DbSaveSource[4];
                    //    dbSaveSourceSpreadSheet[0] = new DbSaveSource(dsReport, this.SelectCommand, this.TableName);
                    //    dbSaveSourceSpreadSheet[1] = new DbSaveSource(dsReport, reportCriteria.SelectCommand, reportCriteria.TableName);
                    //    dbSaveSourceSpreadSheet[2] = new DbSaveSource(dsReport, reportSpreadSheet.SelectCommand, reportSpreadSheet.TableName);
                    //    dbSaveSourceSpreadSheet[3] = new DbSaveSource(dsReport, reportSpreadSheetField.SelectCommand, reportSpreadSheetField.TableName);

                    //    // Commit to database
                    //    result = DbCommit(dbSaveSourceSpreadSheet, connStr);

                    //    break;

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


        public bool Delete(DataSet dsReport, int reportTypeID, string connStr)
        {
            var reportCriteria = new BL.RPT.ReportCriteria();
            var result = false;

            switch (reportTypeID)
            {
                    //case 1:
                    //    // Save data to table report chart and report chart series.
                    //    BL.Report.ReportChart       reportChart         = new BL.Report.ReportChart();
                    //    BL.Report.ReportChartSeries reportChartSeries   = new BL.Report.ReportChartSeries();

                    //    // Create DbSaveSource
                    //    DbSaveSource[] dbSaveSource = new DbSaveSource[4];
                    //    dbSaveSource[0]             = new DbSaveSource(dsReport, reportChartSeries.SelectCommand, reportChartSeries.TableName);
                    //    dbSaveSource[1]             = new DbSaveSource(dsReport, reportChart.SelectCommand, reportChart.TableName);
                    //    dbSaveSource[2]             = new DbSaveSource(dsReport, reportCriteria.SelectCommand, reportCriteria.TableName);
                    //    dbSaveSource[3]             = new DbSaveSource(dsReport, this.SelectCommand, this.TableName);

                    //    // Commit to database
                    //    result = DbCommit(dbSaveSource, connStr);
                    //    break;

                    //case 2:
                    //    // Save data to table report table, report table column and report table summary.
                    //    BlueLedger.BL.Report.ReportTable        reportTable         = new BL.Report.ReportTable();
                    //    BlueLedger.BL.Report.ReportTableColumn  reportTableColumn   = new BlueLedger.BL.Report.ReportTableColumn();
                    //    BlueLedger.BL.Report.ReportTableSummary reportTableSummary  = new BlueLedger.BL.Report.ReportTableSummary();

                    //    // Create DbSaveSource
                    //    DbSaveSource[] dbSaveSourceTable    = new DbSaveSource[5];
                    //    dbSaveSourceTable[0]                = new DbSaveSource(dsReport, reportTableSummary.SelectCommand, reportTableSummary.TableName);
                    //    dbSaveSourceTable[1]                = new DbSaveSource(dsReport, reportTableColumn.SelectCommand, reportTableColumn.TableName);
                    //    dbSaveSourceTable[2]                = new DbSaveSource(dsReport, reportTable.SelectCommand, reportTable.TableName);
                    //    dbSaveSourceTable[3]                = new DbSaveSource(dsReport, reportCriteria.SelectCommand, reportCriteria.TableName);
                    //    dbSaveSourceTable[4]                = new DbSaveSource(dsReport, this.SelectCommand, this.TableName);

                    //    // Commit to database
                    //    result = DbCommit(dbSaveSourceTable, connStr);
                    //    break;

                    //case 3:
                    //    // Save data to table report pivot and report pivot field.
                    //    BlueLedger.BL.Report.ReportPivot      reportPivot = new BlueLedger.BL.Report.ReportPivot();
                    //    BlueLedger.BL.Report.ReportPivotField reportPivotField = new BlueLedger.BL.Report.ReportPivotField();

                    //    // Create DbSaveSource
                    //    DbSaveSource[] dbSaveSourcePivot    = new DbSaveSource[4];
                    //    dbSaveSourcePivot[0]                = new DbSaveSource(dsReport, reportPivotField.SelectCommand, reportPivotField.TableName);
                    //    dbSaveSourcePivot[1]                = new DbSaveSource(dsReport, reportPivot.SelectCommand, reportPivot.TableName);
                    //    dbSaveSourcePivot[2]                = new DbSaveSource(dsReport, reportCriteria.SelectCommand, reportCriteria.TableName);
                    //    dbSaveSourcePivot[3]                = new DbSaveSource(dsReport, this.SelectCommand, this.TableName);

                    //    // Commit to database
                    //    result = DbCommit(dbSaveSourcePivot, connStr);
                    //    break;

                    //case 4:
                    //    // Save data to table report spread sheet and report spread sheet field.
                    //    BlueLedger.BL.Report.ReportSpreadSheet      reportSpreadSheet      = new BlueLedger.BL.Report.ReportSpreadSheet();
                    //    BlueLedger.BL.Report.ReportSpreadSheetField reportSpreadSheetField = new BlueLedger.BL.Report.ReportSpreadSheetField();

                    //    // Create DbSaveSource.
                    //    DbSaveSource[] dbSaveSourceSpreadSheet  = new DbSaveSource[4];
                    //    dbSaveSourceSpreadSheet[0]              = new DbSaveSource(dsReport, reportSpreadSheetField.SelectCommand, reportSpreadSheetField.TableName);
                    //    dbSaveSourceSpreadSheet[1]              = new DbSaveSource(dsReport, reportSpreadSheet.SelectCommand, reportSpreadSheet.TableName);
                    //    dbSaveSourceSpreadSheet[2]              = new DbSaveSource(dsReport, reportCriteria.SelectCommand, reportCriteria.TableName);
                    //    dbSaveSourceSpreadSheet[3]              = new DbSaveSource(dsReport, this.SelectCommand, this.TableName);

                    //    // Commit to database
                    //    result = DbCommit(dbSaveSourceSpreadSheet, connStr);

                    //    break;

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


        public bool GetSourceData(DataSet dsReport, int ReportID, string ConnectionString)
        {
            var cmdSelect = string.Empty;
            var dbParams = new DbParameter[1];

            // Get query string
            // Create parameters
            dbParams[0] = new DbParameter("@ReportID", ReportID.ToString());

            var dtDataSource = DbRead("RPT.GetSourceData", dbParams, ConnectionString);

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


        /// <summary>
        ///     Get schema from table report.
        /// </summary>
        /// <param name="dsReport"></param>
        public void GetReportSchema(DataSet dsReport, string connStr)
        {
            DbRetrieveSchema("RPT.GetReportList", dsReport, null, TableName, connStr);
        }


        /// <summary>
        ///     Get max value at field reportid
        /// </summary>
        /// <returns></returns>
        public int GetNewReportID(string connStr)
        {
            var dsTmp = new DataSet();
            DbRetrieve("RPT.GetReportIDMax", dsTmp, null, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows[0]["ReportID"].ToString() != string.Empty)
            {
                return (int) dsTmp.Tables[TableName].Rows[0]["ReportID"];
            }
            return 1;
        }

        #endregion
    }
}