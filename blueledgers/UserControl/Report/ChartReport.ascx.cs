using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BlueLedger;

using BlueLedger.PL.BaseClass;
using DevExpress.Data;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;

namespace BlueLedger.PL.UserControls.Report
{   
    public partial class ChartReport : BaseUserControl
    {
        #region "Attributies"

        private int _reportID;
        private int _dataSourceID;
        private Blue.BL.GnxLib.ReportCategory _category;

        /// <summary>
        /// Get or Set ReportID of ChartReport UserControl.
        /// </summary>
        public int ReportID
        {
            get { return this._reportID; }
            set { this._reportID = value; }
        }

        /// <summary>
        /// Get or Set DataSourceID of ChartReport UserControl.
        /// </summary>
        public int DataSourceID
        {
            get { return this._dataSourceID; }
            set { this._dataSourceID = value; }
        }

        /// <summary>
        /// Get or Set ReportCategory of ChartReport UserControl.
        /// </summary>
        public Blue.BL.GnxLib.ReportCategory Category
        {
            get { return this._category; }
            set { this._category = value; }
        }

        #endregion
        
        #region "Operations" 
       
        public DataSet dsReport                         = new DataSet();
        Blue.BL.Report.ChartType chartType                   = new Blue.BL.Report.ChartType();
        Blue.BL.Report.Palette palette                       = new Blue.BL.Report.Palette();
        Blue.BL.Report.Report report                         = new Blue.BL.Report.Report();
        Blue.BL.Report.ReportChart reportChart               = new Blue.BL.Report.ReportChart();
        Blue.BL.Report.ReportChartSeries reportChartSeries   = new Blue.BL.Report.ReportChartSeries();
        Blue.BL.Report.DataSourceColumn dataSourceColumn     = new Blue.BL.Report.DataSourceColumn();
        Blue.BL.APP.Field field                              = new Blue.BL.APP.Field();
        Blue.BL.Report.DataSource dataSource = new Blue.BL.Report.DataSource();
        /// <summary>
        /// Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   
            if (!IsPostBack)
            {
                dsReport = (DataSet)Session["dsReport"];

                if (dsReport == null)
                {
                    return;
                }

                if (dsReport.Tables[report.TableName] == null)
                {
                    return;
                }

                if (dsReport.Tables[report.TableName].Rows.Count > 0)
                {
                    // Report TypeID = 1 is type of Chart Report
                    if ((int)dsReport.Tables[report.TableName].Rows[0]["Type"] == 1)
                    {
                        this.Page_Retrieve();
                        this.Page_Setting();
                    }                   
                }
            }
            else
            {
                dsReport = (DataSet)Session["dsReport"];
            }
        }

        /// <summary>
        /// Page retrieve 
        /// </summary>
        private void Page_Retrieve()
        {
            Blue.BL.GnxLib.ReportCategory category = (Request.Params["category"] == null ? Blue.BL.GnxLib.ReportCategory.Consolidate : (Blue.BL.GnxLib.ReportCategory)Enum.Parse(typeof(Blue.BL.GnxLib.ReportCategory), Request.Params["category"].ToString()));

            if (dsReport.Tables[report.TableName].Rows.Count > 0)
            {
                int ReportID = (int)dsReport.Tables[report.TableName].Rows[0]["ReportID"];

                if (Request.Params["action"].ToUpper() == "NEW")
                {
                    // Get ReportChart Schema.
                    if (category == Blue.BL.GnxLib.ReportCategory.Consolidate)
                    {
                        reportChart.GetReportChartSchema(dsReport, string.Empty);
                    }
                    else
                    {
                        reportChart.GetReportChartSchema(dsReport, LoginInfo.ConnStr);
                    }                   

                    // Assign default value
                    DataRow drReportChart           = dsReport.Tables[reportChart.TableName].NewRow();
                    drReportChart["ReportChartID"]  = (category == Blue.BL.GnxLib.ReportCategory.Consolidate ?
                                                       reportChart.GetNewReportChartID(string.Empty) :
                                                       reportChart.GetNewReportChartID(LoginInfo.ConnStr)) + 1;
                    drReportChart["ReportID"]       = ReportID;
                    dsReport.Tables[reportChart.TableName].Rows.Add(drReportChart);

                    // Get report chart series structure.
                    if (category == Blue.BL.GnxLib.ReportCategory.Consolidate)
                    {
                        reportChartSeries.GetReportChartSeriesSchema(dsReport, string.Empty);
                    }
                    else 
                    {
                        reportChartSeries.GetReportChartSeriesSchema(dsReport, LoginInfo.ConnStr);
                    }                   
                }
                else
                {
                    // Get report chart data.
                    if (category == Blue.BL.GnxLib.ReportCategory.Consolidate)
                    {
                        reportChart.GetReportChart(dsReport, ReportID, string.Empty);
                    }
                    else
                    {
                        reportChart.GetReportChart(dsReport, ReportID, LoginInfo.ConnStr);
                    }                   

                    if (dsReport.Tables[reportChart.TableName].Rows.Count > 0)
                    {
                        // Get report chart series data.
                        string reportChartID = dsReport.Tables[reportChart.TableName].Rows[0]["ReportChartID"].ToString();

                        if (category == Blue.BL.GnxLib.ReportCategory.Consolidate)
                        {
                            reportChartSeries.GetReportChartSeries(dsReport, reportChartID.ToString(), string.Empty);
                        }
                        else
                        {
                            reportChartSeries.GetReportChartSeries(dsReport, reportChartID.ToString(), LoginInfo.ConnStr);
                        }
                    }
                }
            }

            // Save to session
            Session["dsReport"] = dsReport;
        }

        /// <summary>
        /// Page setting
        /// </summary>
        private void Page_Setting()
        {
            Blue.BL.GnxLib.ReportCategory category = (Request.Params["category"] == null ? Blue.BL.GnxLib.ReportCategory.Consolidate : (Blue.BL.GnxLib.ReportCategory)Enum.Parse(typeof(Blue.BL.GnxLib.ReportCategory), Request.Params["category"].ToString()));

            // Assing client side script for new Series
            btn_New.OnClientClick = "return btn_New_Click('" + Parent.FindControl("ddl_DataSource").ClientID +
                                    "','" + ddl_ChartType.ClientID + "')";

            // Binding report chart
            if (dsReport.Tables[reportChart.TableName].Rows.Count > 0)
            {
                // Binding ChartType combobox
                ddl_ChartType.DataSource                = (category == Blue.BL.GnxLib.ReportCategory.Consolidate ?
                                                           chartType.GetActiveList(string.Empty) : chartType.GetActiveList(LoginInfo.ConnStr));                                                          
                ddl_ChartType.TextField                 = "Name";
                ddl_ChartType.ValueField                = "ChartTypeCode";
                ddl_ChartType.ImageUrlField             = "ImageURL";
                ddl_ChartType.DataBind();

                // Binding PaletteName combobox
                ddl_PaletteName.DataSource              = (category == Blue.BL.GnxLib.ReportCategory.Consolidate ?
                                                           palette.GetActiveList(string.Empty) : palette.GetActiveList(LoginInfo.ConnStr));
                ddl_PaletteName.TextField               = "Name";
                ddl_PaletteName.ValueField              = "PaletteCode";
                ddl_PaletteName.ImageUrlField           = "ImageURL";
                ddl_PaletteName.DataBind();

                DataRow drReportChart                   = dsReport.Tables[reportChart.TableName].Rows[0];

                ddl_ChartType.Value                     = (drReportChart["ChartTypeCode"]   == DBNull.Value ? string.Empty : drReportChart["ChartTypeCode"].ToString());
                ddl_PaletteName.Value                   = (drReportChart["PaletteCode"]     == DBNull.Value ? string.Empty : drReportChart["PaletteCode"].ToString());
                txt_TitleChartReport.Text               = (drReportChart["Title"]           == DBNull.Value ? string.Empty : drReportChart["Title"].ToString());
                rdo_TitleAlignment.SelectedValue        = drReportChart["TitleAlignment"].ToString();
                txt_AxisXTitle.Text                     = (drReportChart["AxisXTitle"]      == DBNull.Value ? string.Empty : drReportChart["AxisXTitle"].ToString());
                rdo_AxisXTitleAlignment.SelectedValue   = drReportChart["AxisXTitleAlignment"].ToString();
                txt_AxisYTitle.Text                     = (drReportChart["AxisYTitle"]      == DBNull.Value ? string.Empty : drReportChart["AxisYTitle"].ToString());
                rdo_AxisYTitleAlignment.SelectedValue   = drReportChart["AxisYTitleAlignment"].ToString();
                rdo_SeriesSorting.SelectedValue         = drReportChart["SeriesSorting"].ToString();
                rdo_ChartAlignment.SelectedValue        = drReportChart["IsRotate"].ToString();
                chk_ShowLegend.Checked                  = (drReportChart["IsShowLegend"]    == DBNull.Value ? false : (bool)drReportChart["IsShowLegend"]);
                ddl_HorizentalAlignment.SelectedValue   = drReportChart["LegendAlignmentHr"].ToString();
                ddl_VerticalAlignment.SelectedValue     = drReportChart["LegendAlignmentVr"].ToString();
                ddl_Direction.SelectedValue             = drReportChart["LegendDirection"].ToString();
            }

            // Set coltrols visibiliy depend on chart type
            if (dsReport.Tables[reportChart.TableName].Rows[0]["ChartTypeCode"] != DBNull.Value)
            {
                switch (int.Parse(dsReport.Tables[reportChart.TableName].Rows[0]["ChartTypeCode"].ToString()))
                {
                    case 15: // Pie
                    case 16: // Pie 3D                        
                        tr_Chart1.Visible = false;
                        tr_Chart2.Visible = false;
                        tr_Chart3.Visible = false;

                        break;

                    default:
                        tr_Chart1.Visible = true;
                        tr_Chart2.Visible = true;
                        tr_Chart3.Visible = true;

                        break;
                }
            }

            // Binding report chart series
            grd_ReportChartSerie.DataSource = dsReport.Tables[reportChartSeries.TableName];
            grd_ReportChartSerie.DataBind();
        }

        /// <summary>
        /// Add new report chartseries.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_New_Click(object sender, EventArgs e)
        {
            // Create new row
            DataRow drNew = dsReport.Tables[reportChartSeries.TableName].NewRow();

            // Assign default value
            drNew["ReportChartID"]  = dsReport.Tables[reportChart.TableName].Rows[0]["ReportChartID"];

            if (dsReport.Tables[reportChartSeries.TableName].Rows.Count > 0)
            {
                drNew["SeriesNo"] = (int)dsReport.Tables[reportChartSeries.TableName].Rows[dsReport.Tables[reportChartSeries.TableName].Rows.Count - 1]["SeriesNo"] + 1;
            }
            else
            {
                drNew["SeriesNo"] = 1;
            }

            // Charttypecode assign to press new process.
            drNew["ChartTypeCode"]  = ddl_ChartType.SelectedItem.Value;

            // Add new row
            dsReport.Tables[reportChartSeries.TableName].Rows.Add(drNew);

            // Refresh grid data
            grd_ReportChartSerie.DataSource = dsReport.Tables[reportChartSeries.TableName];
            grd_ReportChartSerie.EditIndex  = dsReport.Tables[reportChartSeries.TableName].Rows.Count - 1;
            grd_ReportChartSerie.DataBind();

            // Save to session
            Session["dsReport"] = dsReport;
        }

        /// <summary>
        /// Set column visibility depend on Chart Type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ReportChartSerie_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header ||
                e.Row.RowType == DataControlRowType.DataRow ||
                e.Row.RowType == DataControlRowType.Footer)
            {
                if (ddl_ChartType.SelectedItem.Value != null)
                {
                    switch (int.Parse(ddl_ChartType.SelectedItem.Value.ToString()))
                    {
                        case 15: // Pie
                        case 16: // Pie 3D                            
                            e.Row.Cells[1].Visible = false;
                            e.Row.Cells[2].Visible = false;
                            e.Row.Cells[3].Visible = true;
                            e.Row.Cells[4].Visible = true;

                            break;

                        default:
                            e.Row.Cells[1].Visible = true;
                            e.Row.Cells[2].Visible = true;
                            e.Row.Cells[3].Visible = false;
                            e.Row.Cells[4].Visible = false;

                            break;
                    }
                }
                else
                {
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                }
            }

            if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                TableCell td_Col1 = (TableCell)e.Row.FindControl("Col1");
                TableCell td_Col2 = (TableCell)e.Row.FindControl("Col2");
                TableCell td_Col3 = (TableCell)e.Row.FindControl("Col3");
                TableCell td_Col4 = (TableCell)e.Row.FindControl("Col4");

                if (dsReport.Tables[reportChart.TableName].Rows[0]["ChartTypeCode"] != DBNull.Value)
                {
                    switch (int.Parse(dsReport.Tables[reportChart.TableName].Rows[0]["ChartTypeCode"].ToString()))
                    {
                        case 15: // Pie
                        case 16: // Pie 3D                        
                            td_Col1.Visible = false;
                            td_Col2.Visible = false;
                            td_Col3.Visible = true;
                            td_Col4.Visible = true;                            

                            break;

                        default:
                            td_Col1.Visible = true;
                            td_Col2.Visible = true;
                            td_Col3.Visible = false;
                            td_Col4.Visible = false;                            

                            break;
                    }
                }                       
            }
        }

        /// <summary>
        /// Display report chart series data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ReportChartSerie_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Blue.BL.GnxLib.ReportCategory category  = (Request.Params["category"] == null ? Blue.BL.GnxLib.ReportCategory.Consolidate : (Blue.BL.GnxLib.ReportCategory)Enum.Parse(typeof(Blue.BL.GnxLib.ReportCategory), Request.Params["category"].ToString()));
            Blue.BL.Report.DataSource dataSource = new Blue.BL.Report.DataSource();
            
            // Get dropdownlist datasource            
           // int dataSourceID = int.Parse(((DropDownList)Parent.FindControl("ddl_DataSource")).SelectedItem.Value.ToString() == string.Empty ? ("0").ToString() : ((DropDownList)Parent.FindControl("ddl_DataSource")).SelectedItem.Value.ToString());
            
           
            DataTable dtDataSourceColumn = (category == Blue.BL.GnxLib.ReportCategory.Consolidate ?
                                            field.GetList(dataSource.GetTableSchema(this._dataSourceID, string.Empty), dataSource.GetTableName(this._dataSourceID, string.Empty), string.Empty) :
                                            field.GetList(dataSource.GetTableSchema(this._dataSourceID, LoginInfo.ConnStr), dataSource.GetTableName(this._dataSourceID, LoginInfo.ConnStr), LoginInfo.ConnStr));
            
            if (dtDataSourceColumn != null)
            {
                // Insert blank row
                DataRow drBlank = dtDataSourceColumn.NewRow();
                dtDataSourceColumn.Rows.InsertAt(drBlank, 0);
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            { 
                // AxisXColumn
                if (e.Row.FindControl("lbl_AxisXColumn") != null)
                {
                    Label lbl_AxisXColumn   = (Label)e.Row.FindControl("lbl_AxisXColumn");
                    lbl_AxisXColumn.Text    = (DataBinder.Eval(e.Row.DataItem, "AxisXColumn") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "AxisXColumn").ToString());                                            
                                              
                }
                if (e.Row.FindControl("ddl_AxisXColumn") != null)
                {
                    DropDownList ddl_AxisXColumn = (DropDownList)e.Row.FindControl("ddl_AxisXColumn");                   

                    if (dtDataSourceColumn != null)
                    { 
                        ddl_AxisXColumn.DataSource      = dtDataSourceColumn;
                        ddl_AxisXColumn.DataTextField   = (LoginInfo.BuFmtInfo.LangCode.ToUpper() == "EN-US" ? "Desc" : "OthDesc");
                        ddl_AxisXColumn.DataValueField  = "Desc";
                        ddl_AxisXColumn.DataBind();
                        ddl_AxisXColumn.SelectedValue   = (DataBinder.Eval(e.Row.DataItem, "AxisXColumn") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "AxisXColumn").ToString());
                    }
                }

                // AxisYColumn 
                if (e.Row.FindControl("lbl_AxisYColumn") != null)
                {
                    Label lbl_AxisYColumn   = (Label)e.Row.FindControl("lbl_AxisYColumn");
                    lbl_AxisYColumn.Text    = (DataBinder.Eval(e.Row.DataItem, "AxisYColumn") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "AxisYColumn").ToString());
                                               
                }
                if (e.Row.FindControl("ddl_AxisYColumn") != null)
                {
                    DropDownList ddl_AxisYColumn = (DropDownList)e.Row.FindControl("ddl_AxisYColumn");                    

                    if (dtDataSourceColumn != null)
                    {
                        ddl_AxisYColumn.DataSource      = dtDataSourceColumn;
                        ddl_AxisYColumn.DataTextField   = (LoginInfo.BuFmtInfo.LangCode.ToUpper() == "EN-US" ? "Desc" : "OthDesc");
                        ddl_AxisYColumn.DataValueField  = "Desc";
                        ddl_AxisYColumn.DataBind();
                        ddl_AxisYColumn.SelectedValue   = (DataBinder.Eval(e.Row.DataItem, "AxisYColumn") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "AxisYColumn").ToString());
                    }
                }

                // ArgumentColumn 
                if (e.Row.FindControl("lbl_ArgumentColumn") != null)
                {
                    Label lbl_ArgumentColumn = (Label)e.Row.FindControl("lbl_ArgumentColumn");
                    lbl_ArgumentColumn.Text  = (DataBinder.Eval(e.Row.DataItem, "ArgumentColumn") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "ArgumentColumn").ToString());
                                                
                }
                if (e.Row.FindControl("ddl_ArgumentColumn") != null)
                {
                    DropDownList ddl_ArgumentColumn = (DropDownList)e.Row.FindControl("ddl_ArgumentColumn");

                    if (dtDataSourceColumn != null)
                    {
                        ddl_ArgumentColumn.DataSource       = dtDataSourceColumn;
                        ddl_ArgumentColumn.DataTextField    = (LoginInfo.BuFmtInfo.LangCode.ToUpper() == "EN-US" ? "Desc" : "OthDesc");
                        ddl_ArgumentColumn.DataValueField   = "Desc";
                        ddl_ArgumentColumn.DataBind();
                        ddl_ArgumentColumn.SelectedValue    = (DataBinder.Eval(e.Row.DataItem, "ArgumentColumn") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "ArgumentColumn").ToString());
                    }
                }

                // ValueColumn
                if (e.Row.FindControl("lbl_ValueColumn") != null)
                {
                    Label lbl_ValueColumn   = (Label)e.Row.FindControl("lbl_ValueColumn");
                    lbl_ValueColumn.Text    = (DataBinder.Eval(e.Row.DataItem, "ValueColumn") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "ValueColumn").ToString());
                                             
                }
                if (e.Row.FindControl("ddl_ValueColumn") != null)
                {
                    DropDownList ddl_ValueColumn = (DropDownList)e.Row.FindControl("ddl_ValueColumn");

                    if (dtDataSourceColumn != null)
                    {
                        ddl_ValueColumn.DataSource      = dtDataSourceColumn;
                        ddl_ValueColumn.DataTextField   = (LoginInfo.BuFmtInfo.LangCode.ToUpper() == "EN-US" ? "Desc" : "OthDesc");
                        ddl_ValueColumn.DataValueField  = "Desc";
                        ddl_ValueColumn.DataBind();
                        ddl_ValueColumn.SelectedValue   = (DataBinder.Eval(e.Row.DataItem, "ValueColumn") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "ValueColumn").ToString());
                    }
                }

                // LegendText 
                if (e.Row.FindControl("lbl_LegendText") != null)
                {
                    Label lbl_LegendText     = (Label)e.Row.FindControl("lbl_LegendText");
                    lbl_LegendText.Text      = (DataBinder.Eval(e.Row.DataItem, "LegendText") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "LegendText").ToString());
                }
                if (e.Row.FindControl("txt_LegendText") != null)
                {
                    TextBox txt_LegendText   = (TextBox)e.Row.FindControl("txt_LegendText");
                    txt_LegendText.Text      = (DataBinder.Eval(e.Row.DataItem, "LegendText") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "LegendText").ToString());
                }

                // IsShowLabel 
                if (e.Row.FindControl("lbl_IsShowLabel") != null)
                {
                    Label lbl_IsShowLabel    = (Label)e.Row.FindControl("lbl_IsShowLabel");
                    lbl_IsShowLabel.Text     = (DataBinder.Eval(e.Row.DataItem, "IsShowLabel") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "IsShowLabel").ToString());
                }
                if (e.Row.FindControl("chk_IsShowLabel") != null)
                {
                    CheckBox chk_IsShowLabel = (CheckBox)e.Row.FindControl("chk_IsShowLabel");
                    chk_IsShowLabel.Checked  = (DataBinder.Eval(e.Row.DataItem, "IsShowLabel") == DBNull.Value ? false : bool.Parse(DataBinder.Eval(e.Row.DataItem, "IsShowLabel").ToString()));
                }

                // LabelView
                if (e.Row.FindControl("lbl_LabelView") != null)
                {
                    Label lbl_LabelView     = (Label)e.Row.FindControl("lbl_LabelView");
                    lbl_LabelView.Text      = (DataBinder.Eval(e.Row.DataItem, "LabelView") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "LabelView").ToString());
                }
                if (e.Row.FindControl("txt_LabelView") != null)
                {
                    TextBox txt_LabelView   = (TextBox)e.Row.FindControl("txt_LabelView");
                    txt_LabelView.Text      = (DataBinder.Eval(e.Row.DataItem, "LabelView") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "LabelView").ToString());
                }
            }
        }

        /// <summary>
        /// Edit report chart series data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ReportChartSerie_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grd_ReportChartSerie.DataSource = dsReport.Tables[reportChartSeries.TableName];
            grd_ReportChartSerie.EditIndex  = e.NewEditIndex;
            grd_ReportChartSerie.DataBind();
        }

        /// <summary>
        /// Delete report chart series data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ReportChartSerie_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Deleted report chart series.
            for (int i = dsReport.Tables[reportChartSeries.TableName].Rows.Count - 1; i >= 0; i--)
            {
                DataRow drDelete = dsReport.Tables[reportChartSeries.TableName].Rows[i];

                if (drDelete.RowState != DataRowState.Deleted)
                {
                    // Equal condition for delete.
                    if (drDelete["ReportChartID"].ToString() == grd_ReportChartSerie.DataKeys[e.RowIndex].Values[0].ToString() &&
                        drDelete["SeriesNo"].ToString() == grd_ReportChartSerie.DataKeys[e.RowIndex].Values[1].ToString())
                    {
                        drDelete.Delete();
                    }
                }
            }

            // Refresh gird data.
            grd_ReportChartSerie.DataSource = dsReport.Tables[reportChartSeries.TableName];
            grd_ReportChartSerie.EditIndex  = -1;
            grd_ReportChartSerie.DataBind();

            // Save to session
            Session["dsReport"] = dsReport;
        }

        /// <summary>
        /// Update changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ReportChartSerie_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {            
            DropDownList ddl_AxisXColumn    = (DropDownList)grd_ReportChartSerie.Rows[e.RowIndex].FindControl("ddl_AxisXColumn");            
            DropDownList ddl_AxisYColumn    = (DropDownList)grd_ReportChartSerie.Rows[e.RowIndex].FindControl("ddl_AxisYColumn");            
            DropDownList ddl_ArgumentColumn = (DropDownList)grd_ReportChartSerie.Rows[e.RowIndex].FindControl("ddl_ArgumentColumn");            
            DropDownList ddl_ValueColumn    = (DropDownList)grd_ReportChartSerie.Rows[e.RowIndex].FindControl("ddl_ValueColumn");            
            TextBox txt_LegendText          = (TextBox)grd_ReportChartSerie.Rows[e.RowIndex].FindControl("txt_LegendText");           
            CheckBox chk_IsShowLabel        = (CheckBox)grd_ReportChartSerie.Rows[e.RowIndex].FindControl("chk_IsShowLabel");
            TextBox txt_LabelView           = (TextBox)grd_ReportChartSerie.Rows[e.RowIndex].FindControl("txt_LabelView");
                
            // Updating to datarow
            DataRow drUpdate = dsReport.Tables[reportChartSeries.TableName].Rows[e.RowIndex];

            if (grd_ReportChartSerie.Columns[1].Visible)
            {
                if (ddl_AxisYColumn.SelectedItem.Value != string.Empty)
                {
                    drUpdate["AxisXColumn"] = ddl_AxisXColumn.SelectedItem.Value;
                }
                else
                {
                    drUpdate["AxisXColumn"] = DBNull.Value;
                }                 
            }
            else
            {
                drUpdate["AxisXColumn"] = DBNull.Value;
            }

            if (grd_ReportChartSerie.Columns[2].Visible)
            {
                if(ddl_AxisYColumn.SelectedItem.Value != string.Empty)
                {
                    drUpdate["AxisYColumn"] = ddl_AxisYColumn.SelectedItem.Value;
                }
                else
                {
                    drUpdate["AxisYColumn"] = DBNull.Value;
                }                
            }
            else
            {
                drUpdate["AxisYColumn"] = DBNull.Value;
            }

            if (grd_ReportChartSerie.Columns[3].Visible)
            {
                if (ddl_ArgumentColumn.SelectedItem.Value != string.Empty)
                {
                    drUpdate["ArgumentColumn"] = ddl_ArgumentColumn.SelectedItem.Value;
                }
                else
                {
                    drUpdate["ArgumentColumn"] = DBNull.Value;
                }
                
            }
            else
            {
                drUpdate["ArgumentColumn"] = DBNull.Value;
            }

            if (grd_ReportChartSerie.Columns[4].Visible)
            {
                if (ddl_ValueColumn.SelectedItem.Value != string.Empty)
                {
                    drUpdate["ValueColumn"] = ddl_ValueColumn.SelectedItem.Value;
                }
                else
                {
                    drUpdate["ValueColumn"] = DBNull.Value;
                }                
            }
            else
            {
                drUpdate["ValueColumn"] = DBNull.Value;
            }

            drUpdate["LegendText"]  = txt_LegendText.Text.Trim();
            drUpdate["IsShowLabel"] = chk_IsShowLabel.Checked;
            drUpdate["LabelView"]   = txt_LabelView.Text.Trim();

            // Refresh gird data.
            grd_ReportChartSerie.DataSource = dsReport.Tables[reportChartSeries.TableName];
            grd_ReportChartSerie.EditIndex  = -1;
            grd_ReportChartSerie.DataBind();

            // Save to session
            Session["dsReport"] = dsReport;
        }

        /// <summary>
        /// Canceling add new or edit report chart series.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ReportChartSerie_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Deleted row information minus from dataset.
            if (dsReport.Tables[reportChartSeries.TableName].Rows[e.RowIndex].RowState == DataRowState.Added)
            {
                dsReport.Tables[reportChartSeries.TableName].Rows[e.RowIndex].Delete();
            }

            // Refresh grid data.
            grd_ReportChartSerie.DataSource = dsReport.Tables[reportChartSeries.TableName];
            grd_ReportChartSerie.EditIndex  = -1;
            grd_ReportChartSerie.DataBind();

            // Save to session
            Session["dsReport"] = dsReport;
        }

        /// <summary>
        /// Select index change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_ChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dsReport.Tables[reportChart.TableName].Rows[0]["ChartTypeCode"] = ddl_ChartType.SelectedItem.Value;

            switch (int.Parse(ddl_ChartType.SelectedItem.Value.ToString()))
            {
                case 15: // Pie
                case 16: // Pie 3D
                    tr_Chart1.Visible = false;
                    tr_Chart2.Visible = false;
                    tr_Chart3.Visible = false;                   

                    break;

                default:
                    tr_Chart1.Visible = true;
                    tr_Chart2.Visible = true;
                    tr_Chart3.Visible = true;                    

                    break;
            }

            // Binding report chart series
            grd_ReportChartSerie.DataSource = dsReport.Tables[reportChartSeries.TableName];
            grd_ReportChartSerie.DataBind();
        }      

        #endregion            
    }
}
