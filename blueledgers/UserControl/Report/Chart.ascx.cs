using System;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BlueLedger.PL.BaseClass;

//using DevExpress.XtraCharts;

namespace BlueLedger.PL.UserControls.Report
{
    public partial class Chart : BaseUserControl
    {
        #region "Attributies"
        
        private int ReportID;
        private DataTable dtReport = new DataTable();
        private Blue.BL.GnxLib.ReportCategory _category;
        int chkCase;

        DataSet dsReport                                = new DataSet();
        Blue.BL.Report.Report report                         = new Blue.BL.Report.Report();
        Blue.BL.Report.ReportType reportType                 = new Blue.BL.Report.ReportType();
        Blue.BL.Report.ReportChart reportChart               = new Blue.BL.Report.ReportChart();
        Blue.BL.Report.ReportChartSeries reportChartSeries   = new Blue.BL.Report.ReportChartSeries();
        Blue.BL.Report.Palette palette                       = new Blue.BL.Report.Palette();
       // BL.Report.DataSource dataSource = new BL.Report.DataSource();
        private Blue.BL.APP.Field field = new Blue.BL.APP.Field();
        private Blue.BL.Report.DataSource dataSource = new Blue.BL.Report.DataSource();


        /// <summary>
        /// Get or Set ReportCategory of ChartReport UserControl.
        /// </summary>
        public Blue.BL.GnxLib.ReportCategory Category
        {
            get { return this._category; }
            set { this._category = value;}
        }  

        #endregion

        #region "Operations"
        
        /// <summary>
        /// Retrieve Data
        /// </summary>
        private void Page_Retrieve()
        {
            Blue.BL.GnxLib.ReportCategory category  = (Request.Params["category"] == null ? Blue.BL.GnxLib.ReportCategory.Consolidate : (Blue.BL.GnxLib.ReportCategory)Enum.Parse(typeof(Blue.BL.GnxLib.ReportCategory), Request.Params["category"].ToString()));
            ReportID                        = int.Parse(Request.Params["reportid"].ToString());

            //Retrieve table reportchart and table reportchartseries.
            if (category == Blue.BL.GnxLib.ReportCategory.Consolidate)
            {
                reportChart.GetReportChart(dsReport, ReportID, string.Empty);
                reportChartSeries.GetReportChartSeries(dsReport, dsReport.Tables[reportChart.TableName].Rows[0]["ReportChartID"].ToString(), string.Empty);
            }
            else
            {
                reportChart.GetReportChart(dsReport, ReportID, LoginInfo.ConnStr);
                reportChartSeries.GetReportChartSeries(dsReport, dsReport.Tables[reportChart.TableName].Rows[0]["ReportChartID"].ToString(), LoginInfo.ConnStr);
            }           
           
            // Get report data.
            dtReport = (DataTable)Session["dtReport"];

            if (dtReport != null)
            {
                dtReport.TableName = "Report";
                dsReport.Tables.Add(dtReport);
            }
            else 
            {
                if (category == Blue.BL.GnxLib.ReportCategory.Consolidate)
                {
                    report.GetReportSchema(dsReport, string.Empty);

                    // Get data source data.
                    dataSource.GetByReportID(dsReport, ReportID, string.Empty);
                }
                else
                {
                    report.GetReportSchema(dsReport, LoginInfo.ConnStr);

                    // Get data source data.
                    dataSource.GetByReportID(dsReport, ReportID, LoginInfo.ConnStr);
                }               
            }

            Session["dsReport"] = dsReport;
        }

        /// <summary>
        /// Binding 
        /// </summary>
        private void Page_Setting()
        {
            Blue.BL.GnxLib.ReportCategory category = (Request.Params["category"] == null ? Blue.BL.GnxLib.ReportCategory.Consolidate : (Blue.BL.GnxLib.ReportCategory)Enum.Parse(typeof(Blue.BL.GnxLib.ReportCategory), Request.Params["category"].ToString()));

            /* if (dsReport.Tables[reportChart.TableName].Rows.Count > 0)
            {
                DataRow drReportChart = dsReport.Tables[reportChart.TableName].Rows[0];
               
                // Setup chart properties
                // Title
                ChartTitle chartTitle   = new ChartTitle();
                chartTitle.Text         = drReportChart["Title"].ToString();

                if (drReportChart["TitleAlignment"].ToString() != DBNull.Value.ToString())
                {
                    chartTitle.Alignment = (StringAlignment)Enum.Parse(typeof(StringAlignment), drReportChart["TitleAlignment"].ToString());
                }       
         
                chartReport.Titles.Add(chartTitle); 

                // PalleteCode
                chartReport.PaletteName = (category == Blue.BL.GnxLib.ReportCategory.Consolidate ?
                                           palette.GetName(drReportChart["PaletteCode"].ToString(), string.Empty) :
                                           palette.GetName(drReportChart["PaletteCode"].ToString(), LoginInfo.ConnStr));
                
                // Setup chart series properties
                foreach (DataRow drReportChartSeries in dsReport.Tables[reportChartSeries.TableName].Rows)
                {
                    ViewType viewType = new ViewType();
                    
                    // Create chart series ********************************
                    chkCase             = int.Parse(dsReport.Tables[reportChartSeries.TableName].Rows[0]["ChartTypeCode"].ToString());
                    chartReport.Height  = 500;
                    chartReport.Width   = 850;

                    switch (chkCase)
                    {
                        case 1:
                            viewType = ViewType.Bar;                            

                            break;
                        case 2:
                            viewType = ViewType.StackedBar;

                            break;
                        case 3:
                            viewType = ViewType.FullStackedBar;
                            
                            break;
                        case 4:
                            viewType            = ViewType.ManhattanBar;
                            //chartReport.Height  = 500;
                            //chartReport.Width   = 850;
                            
                            break;
                        case 5:
                            viewType = ViewType.Point;

                            break;
                        case 6:
                            viewType = ViewType.Line;

                            break;
                        case 7:
                            viewType = ViewType.StepLine;

                            break;
                        case 8:
                            viewType            = ViewType.Line3D;
                            //chartReport.Height  = 500;
                            //chartReport.Width   = 850;

                            break;
                        case 9:
                            viewType = ViewType.Area;
                            
                            break;
                        case 10:
                            viewType = ViewType.StackedArea;

                            break;
                        case 11:
                            viewType = ViewType.FullStackedArea;
                            
                            break;
                        case 12:
                            viewType = ViewType.Area3D;

                            break;
                        case 13:
                            viewType = ViewType.StackedArea3D;
                            
                            break;
                        case 14:
                            viewType           = ViewType.FullStackedArea3D;
                            //chartReport.Height = 500;
                            //chartReport.Width  = 850;

                            break;
                        case 15:
                            viewType            = ViewType.Pie;
                            //chartReport.Height  = 500;
                            //chartReport.Width   = 850;

                            break;
                        case 16:
                            viewType            = ViewType.Pie3D;
                            //chartReport.Height  = 500;
                            //chartReport.Width   = 850;

                            break;
                        case 17:
                            viewType = ViewType.StepLine3D;

                            break;
                    }
                    
                    Series series = new Series("Report", viewType);

                    int dataSourceID = int.Parse(dsReport.Tables[reportChart.TableName].Rows[0]["ReportChartID"].ToString() == string.Empty ? ("0").ToString() : dsReport.Tables[reportChart.TableName].Rows[0]["ReportChartID"].ToString());

                    // Setting chart series properties ********************
                    if (int.Parse(dsReport.Tables[reportChartSeries.TableName].Rows[0]["ChartTypeCode"].ToString()) == 15 ||
                        int.Parse(dsReport.Tables[reportChartSeries.TableName].Rows[0]["ChartTypeCode"].ToString()) == 16)
                    {
                        foreach (DataRow drReport in dsReport.Tables["Report"].Rows)
                        {
                            SeriesPoint seriesPoint = new SeriesPoint(drReport[(category == Blue.BL.GnxLib.ReportCategory.Consolidate ?
                                                                        field.GetFieldType(dataSource.GetTableSchema(dataSourceID, string.Empty), dataSource.GetTableName(dataSourceID, string.Empty), drReportChartSeries["ArgumentColumn"].ToString(), string.Empty) :
                                                                        field.GetFieldType(dataSource.GetTableSchema(dataSourceID, LoginInfo.ConnStr), dataSource.GetTableName(dataSourceID, LoginInfo.ConnStr), drReportChartSeries["ArgumentColumn"].ToString(), LoginInfo.ConnStr))],

                                                                        new object[] { drReport[(category == Blue.BL.GnxLib.ReportCategory.Consolidate ?   //ValueColumn
                                                                        field.GetFieldType(dataSource.GetTableSchema(dataSourceID, string.Empty), dataSource.GetTableName(dataSourceID, string.Empty), drReportChartSeries["ValueColumn"].ToString(), string.Empty) :
                                                                        field.GetFieldType(dataSource.GetTableSchema(dataSourceID, LoginInfo.ConnStr), dataSource.GetTableName(dataSourceID, LoginInfo.ConnStr), drReportChartSeries["ValueColumn"].ToString(), LoginInfo.ConnStr))] });
                            series.Points.Add(seriesPoint);                   
                        }
                        series.PointOptions.PointView = PointView.ArgumentAndValues;
                    }
                    else
                    {
                        foreach (DataRow drReport in dsReport.Tables["Report"].Rows)
                        {
                            //SeriesPoint seriesPoint = new SeriesPoint(drReport[field.GetFieldName(drReportChartSeries["AxisXColumn"].ToString(), LoginInfo.ConnStr)], new object[] { drReport[field.GetFieldName(drReportChartSeries["AxisYColumn"].ToString(), LoginInfo.ConnStr)] });
                            //series.Points.Add(seriesPoint);

                            SeriesPoint seriesPoint = new SeriesPoint(drReport[(category == Blue.BL.GnxLib.ReportCategory.Consolidate ?
                                                                                                field.GetFieldType(dataSource.GetTableSchema(dataSourceID, string.Empty), dataSource.GetTableName(dataSourceID, string.Empty), drReportChartSeries["AxisYColumn"].ToString(), string.Empty) :
                                                                                                field.GetFieldType(dataSource.GetTableSchema(dataSourceID, LoginInfo.ConnStr), dataSource.GetTableName(dataSourceID, string.Empty), drReportChartSeries["AxisYColumn"].ToString(), LoginInfo.ConnStr))],

                                                                      new object[] { drReport[(category == Blue.BL.GnxLib.ReportCategory.Consolidate ?
                                                                                                field.GetFieldType(dataSource.GetTableSchema(dataSourceID, string.Empty), dataSource.GetTableName(dataSourceID, string.Empty), drReportChartSeries["AxisYColumn"].ToString(), string.Empty) :
                                                                                                field.GetFieldType(dataSource.GetTableSchema(dataSourceID, LoginInfo.ConnStr), dataSource.GetTableName(dataSourceID, string.Empty), drReportChartSeries["AxisYColumn"].ToString(), LoginInfo.ConnStr))] });
                        }                       
                    }
                    // LegendText
                    series.LegendText = (drReportChartSeries["LegendText"] == DBNull.Value ? string.Empty : drReportChartSeries["LegendText"].ToString());

                    // Show Label
                    series.Label.Visible    = (drReportChartSeries["IsShowLabel"] == DBNull.Value ? false : (bool)drReportChartSeries["IsShowLabel"]);
                   
                    // Adding chart series ********************************
                    chartReport.Series.Add(series);
                }
                
                switch (chkCase)
                {
                    case 1: //Bar
                    case 2: //Bar stacked
                    case 3: //Bar stacked 100%
                    case 5: //Point
                    case 6: //Line
                    case 7: //Step line
                    case 9: //Area
                    case 10: //Area stacked
                    case 11: //Area stacked 100%

                        // Diagram properties 
                        XYDiagram xyDiagram             = (XYDiagram)chartReport.Diagram;
                        xyDiagram.AxisX.Title.Text      = drReportChart["AxisXTitle"].ToString();
                        xyDiagram.AxisX.Title.Alignment = (StringAlignment)Enum.Parse(typeof(StringAlignment),drReportChart["AxisXTitleAlignment"].ToString());
                        xyDiagram.AxisX.Title.Visible   = true;
                        xyDiagram.AxisY.Title.Text      = drReportChart["AxisYTitle"].ToString();
                        xyDiagram.AxisY.Title.Alignment = (StringAlignment)Enum.Parse(typeof(StringAlignment), drReportChart["AxisYTitleAlignment"].ToString());
                        xyDiagram.AxisY.Title.Visible   = true;
                        xyDiagram.Rotated               = (bool)drReportChart["IsRotate"];
                    break;

                    case 4: //Manhattan bar
                    case 8: //Line 3D
                    case 12: //Area 3D
                    case 13: //Area 3D Stacked
                    case 14: //Area 3D Stacked 100%
                    case 17: //Step Line 3D

                        XYDiagram3D xyDiagram3D = (XYDiagram3D)chartReport.Diagram;
                        xyDiagram3D.AxisX.Name = "ssssss";
                        
                        
                        //xyDiagram3D.RotationAngleX = 100;
                        //xyDiagram3D.RotationType = RotationType.UseAngles;
                        break;
                }

                //Pie 3D
                //SimpleDiagram3D simple = (SimpleDiagram3D)chartReport.Diagram;
                //simple.RuntimeZooming = false;
                //simple.RotationOrder = RotationOrder.XYZ;
                //simple.LayoutDirection = LayoutDirection.Horizontal;
                //simple.HorizontalScrollPercent = 100; //-100 to 100
                //simple.PerspectiveEnabled = true;
                //simple.PerspectiveAngle = 150; // able set 0 to 179 but set PerspectiveEnabled = true only
                

                //Area 3D
                //XYDiagram3D xyDiagram3D = (XYDiagram3D)chartReport.Diagram;
                //xyDiagram3D.RotationAngleX = 100;
                //xyDiagram3D.RotationType = RotationType.UseAngles;
                                
                // Chart properties
                chartReport.Legend.Visible              = (bool)drReportChart["IsShowLegend"];
                chartReport.Legend.AlignmentHorizontal  = (LegendAlignmentHorizontal)Enum.Parse(typeof(LegendAlignmentHorizontal), drReportChart["LegendAlignmentHr"].ToString());
                chartReport.Legend.AlignmentVertical    = (LegendAlignmentVertical)Enum.Parse(typeof(LegendAlignmentVertical), drReportChart["LegendAlignmentVr"].ToString());
                chartReport.Legend.Direction            = (LegendDirection)Enum.Parse(typeof(LegendDirection), drReportChart["LegendDirection"].ToString());

                if (drReportChart["SeriesSorting"].ToString() != DBNull.Value.ToString())
                {
                    chartReport.SeriesSorting = (SortingMode)Enum.Parse(typeof(SortingMode), drReportChart["SeriesSorting"].ToString());
                }                
            } */
        }        

        /// <summary>
        /// Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                if (!IsPostBack)
                {
                    Page_Retrieve();
                    Page_Setting();
                }
                else
                {
                    dsReport = (DataSet)Session["dsReport"];

                    //chartReport.DataSource = dsReport.Tables[report.TableName];
                    //chartReport.DataBind();
                }
            }
        }

        #endregion
    }
}