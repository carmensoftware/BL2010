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
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxTabControl;
using DevExpress.Web.ASPxGridView;
using System.Collections.Generic;
using DevExpress.Data;

namespace BlueLedger.PL.UserControls.Report
{
    public partial class TableReport : BaseUserControl
    {
        #region     " Attributes "
        
        private DataSet dsReport                                 = new DataSet();
        private Blue.BL.Report.Report report                          = new Blue.BL.Report.Report();
        private Blue.BL.Report.ReportTable reportTable                = new Blue.BL.Report.ReportTable();
        private Blue.BL.Report.ReportTableColumn reportTableColumn    = new Blue.BL.Report.ReportTableColumn();
        private Blue.BL.Report.ReportTableSummary reportTableSummary  = new Blue.BL.Report.ReportTableSummary();
        Blue.BL.Report.DataSourceColumn dataSourceColumn              = new Blue.BL.Report.DataSourceColumn();
        Blue.BL.APP.Field field                                       = new Blue.BL.APP.Field();
        Blue.BL.Application.LookupItem lookupItem                     = new Blue.BL.Application.LookupItem();
        Blue.BL.Report.DataSource dataSource                          = new Blue.BL.Report.DataSource();
        private Blue.BL.GnxLib.ReportCategory _category;

        /// <summary>
        /// Get or Set ReportCategory of ChartReport UserControl.
        /// </summary>
        public Blue.BL.GnxLib.ReportCategory Category
        {
            get { return this._category; }
            set { this._category = value; }
        }

        #endregion

        #region     " Operations "

        /// <summary>
        /// Call function for page postback and after post back
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected  void Page_Load(object sender, EventArgs e)
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
                    // For edit process
                    // Report TypeID = 2 is type of Tabular Report
                    if ((int)dsReport.Tables[report.TableName].Rows[0]["Type"] == 2)
                    {
                        this.Page_Retrieve();
                        this.Page_Setting();
                    }
                    else if (Request.Params["action"].ToUpper() == "NEW")
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
        /// Retrieve data.
        /// </summary>
        private void Page_Retrieve()
        {
            Blue.BL.GnxLib.ReportCategory category = (Request.Params["category"] == null ? Blue.BL.GnxLib.ReportCategory.Consolidate : (Blue.BL.GnxLib.ReportCategory)Enum.Parse(typeof(Blue.BL.GnxLib.ReportCategory), Request.Params["category"].ToString()));
            
            // For edit condition
            if (Request.Params["action"].ToUpper() == "EDIT")
            {
                // Get report properties.
                if (category == Blue.BL.GnxLib.ReportCategory.Consolidate)
                {
                    reportTable.GetListByReportID(dsReport,Convert.ToInt32(Request.Params["reportid"].ToString()),string.Empty);
                }
                else
                {
                    reportTable.GetListByReportID(dsReport,Convert.ToInt32(Request.Params["reportid"].ToString()),LoginInfo.ConnStr);
                }

                if (dsReport.Tables[reportTable.TableName].Rows.Count > 0)
                {
                    if (category == Blue.BL.GnxLib.ReportCategory.Consolidate)
                    {
                        // Get report table column.
                        reportTableColumn.GetListByReportTableID(dsReport,Convert.ToInt32(dsReport.Tables[reportTable.TableName].Rows[0]["ReportTableID"]), string.Empty);

                        // Get report table summary.
                        reportTableSummary.GetListByReportTableID(dsReport,Convert.ToInt32(dsReport.Tables[reportTable.TableName].Rows[0]["ReportTableID"]), string.Empty);
                    }
                    else
                    {
                        // Get report table column.
                        reportTableColumn.GetListByReportTableID(dsReport,Convert.ToInt32(dsReport.Tables[reportTable.TableName].Rows[0]["ReportTableID"]), LoginInfo.ConnStr);

                        // Get report table summary.
                        reportTableSummary.GetListByReportTableID(dsReport,Convert.ToInt32(dsReport.Tables[reportTable.TableName].Rows[0]["ReportTableID"]), LoginInfo.ConnStr);
                    }
                }
            }
            // For new condition 
            else
            {

                dsReport = (DataSet)Session["dsReport"];

                // Get schema table reporttable.
                if (category == Blue.BL.GnxLib.ReportCategory.Consolidate)
                {
                    reportTable.GetReportTableSchema(dsReport, string.Empty);
                }
                else
                {
                    reportTable.GetReportTableSchema(dsReport, LoginInfo.ConnStr);
                }

                // Add new row.
                DataRow drReportTable = dsReport.Tables[reportTable.TableName].NewRow();
                drReportTable["ReportTableID"] = (category == Blue.BL.GnxLib.ReportCategory.Consolidate ?
                                                  reportTable.GetNewReportTableID(string.Empty) :
                                                  reportTable.GetNewReportTableID(LoginInfo.ConnStr)) + 1;
                drReportTable["ReportID"]      = dsReport.Tables["Report"].Rows[0]["ReportID"];

                dsReport.Tables[reportTable.TableName].Rows.Add(drReportTable);

                // Get schema table report table column.
                if (category == Blue.BL.GnxLib.ReportCategory.Consolidate)
                {
                    reportTableColumn.GetReportTableColumnSchema(dsReport, string.Empty);
                }
                else
                {
                    reportTableColumn.GetReportTableColumnSchema(dsReport, LoginInfo.ConnStr);
                }

                // Add new row.
                DataRow drReportTableColumn          = dsReport.Tables[reportTableColumn.TableName].NewRow();
                drReportTableColumn["ReportTableID"] = dsReport.Tables[reportTable.TableName].Rows[0]["ReportTableID"];
                drReportTableColumn["ColumnNo"]      = (category == Blue.BL.GnxLib.ReportCategory.Consolidate ?
                                                       reportTableColumn.GetNewColumnNo(string.Empty) :
                                                       reportTableColumn.GetNewColumnNo(LoginInfo.ConnStr)) + 1;

                // Get schema table report table summary.
                if (category == Blue.BL.GnxLib.ReportCategory.Consolidate)
                {
                    reportTableSummary.GetReportTableSummarySchema(dsReport, string.Empty);
                }
                else
                {
                    reportTableSummary.GetReportTableSummarySchema(dsReport, LoginInfo.ConnStr);
                }

                // Add new row.
                DataRow drReportTableSummary            = dsReport.Tables[reportTableSummary.TableName].NewRow();
                drReportTableSummary["ReportTableID"]   = dsReport.Tables[reportTable.TableName].Rows[0]["ReportTableID"];
                drReportTableSummary["SummaryNo"]       = (category == Blue.BL.GnxLib.ReportCategory.Consolidate ?
                                                           reportTableSummary.GetNewSummaryNo(string.Empty) :
                                                           reportTableSummary.GetNewSummaryNo(LoginInfo.ConnStr)) + 1;

                //dsReport.Tables[reportTableSummary.TableName].Rows.Add(drReportTableSummary);
               
            }
            // Assign to session object.
            Session["dsReport"] = dsReport;
        }

        /// <summary>
        /// Setting data to controls.
        /// </summary>
        private void Page_Setting()
        {
            if (dsReport.Tables[reportTable.TableName].Rows.Count > 0)
            {                 
                // Binding all of reporttable label control.
                this.txt_Title.Text      = dsReport.Tables[reportTable.TableName].Rows[0]["Title"].ToString();
                this.txt_PageSize.Text   = dsReport.Tables[reportTable.TableName].Rows[0]["PageSize"].ToString();

                if (dsReport.Tables[reportTable.TableName].Rows[0]["IsShowTitlePanel"] != DBNull.Value)
                {
                    bool isShowTitlePanel = Convert.ToBoolean(dsReport.Tables[reportTable.TableName].Rows[0]["IsShowTitlePanel"]);

                    // For show title.
                    if (isShowTitlePanel)
                    {
                        chk_ShowTitle.Checked = true;
                    }
                    else
                    {
                        chk_ShowTitle.Checked = false;
                    }
                }

                if (dsReport.Tables[reportTable.TableName].Rows[0]["AllowGroup"] != DBNull.Value)
                {
                    bool allowGroup = Convert.ToBoolean(dsReport.Tables[reportTable.TableName].Rows[0]["AllowGroup"]);
                    
                    // For allowgroup
                    if (allowGroup)
                    {
                        chk_AllowGrouping.Checked = true;
                    }
                    else
                    {
                        chk_AllowGrouping.Checked = false;
                    }
                }

                if (dsReport.Tables[reportTable.TableName].Rows[0]["AllowSort"] != DBNull.Value)
                {
                    bool allowSort = Convert.ToBoolean(dsReport.Tables[reportTable.TableName].Rows[0]["AllowSort"]);
                    
                    // For allowsort
                    if (allowSort)
                    {
                        chk_AllowSorting.Checked = true;
                    }
                    else
                    {
                        chk_AllowSorting.Checked = false;
                    }
                }

                if (dsReport.Tables[reportTable.TableName].Rows[0]["IsShowPager"] != DBNull.Value)
                {
                    bool isShowPager = Convert.ToBoolean(dsReport.Tables[reportTable.TableName].Rows[0]["IsShowPager"]);
                    // For showpager
                    if (isShowPager)
                    {
                        chk_ShowPager.Checked = true;
                    }
                    else
                    {
                        chk_ShowPager.Checked = false;
                    }
                }

                if (dsReport.Tables[reportTable.TableName].Rows[0]["IsShowHeader"] != DBNull.Value)
                {
                    bool isShowHeader = Convert.ToBoolean(dsReport.Tables[reportTable.TableName].Rows[0]["IsShowHeader"]);
                    // For showheader
                    if (isShowHeader)
                    {
                        chk_ShowHeader.Checked = true;
                    }
                    else
                    {
                        chk_ShowHeader.Checked = false;
                    }
                }

                if (dsReport.Tables[reportTable.TableName].Rows[0]["IsShowFooter"] != DBNull.Value)
                {
                    bool isShowFooter = Convert.ToBoolean(dsReport.Tables[reportTable.TableName].Rows[0]["IsShowFooter"]);
                    // For showFooter
                    if (isShowFooter)
                    {
                        chk_ShowFooter.Checked = true;
                    }
                    else
                    {
                        chk_ShowFooter.Checked = false;
                    }
                }

                if (dsReport.Tables[reportTable.TableName].Rows[0]["IsShowFilterRow"] != DBNull.Value)
                {
                    bool isShowFilterRow = Convert.ToBoolean(dsReport.Tables[reportTable.TableName].Rows[0]["IsShowFilterRow"]);
                    // For showfilterrow
                    if (isShowFilterRow)
                    {
                        chk_ShowFilterRow.Checked = true;
                    }
                    else
                    {
                        chk_ShowFilterRow.Checked = false;
                    }
                }

                if (dsReport.Tables[reportTable.TableName].Rows[0]["IsShowGroupedColumns"] != DBNull.Value)
                {
                    bool isShowGroupedColumn = Convert.ToBoolean(dsReport.Tables[reportTable.TableName].Rows[0]["IsShowGroupedColumns"]);
                    // For showgroupedcolumn
                    if (isShowGroupedColumn)
                    {
                        chk_ShowGroupedColumn.Checked = true;
                    }
                    else
                    {
                        chk_ShowGroupedColumn.Checked = false;
                    }
                }

                if (dsReport.Tables[reportTable.TableName].Rows[0]["IsShowGroupPanel"] != DBNull.Value)
                {
                    bool isShowGroupPanel    = Convert.ToBoolean(dsReport.Tables[reportTable.TableName].Rows[0]["IsShowGroupPanel"]);
                
                    // For showgrouppanel
                
                    if (isShowGroupPanel)
                    {
                        chk_ShowGroupPanel.Checked = true;
                    }
                    else
                    {
                        chk_ShowGroupPanel.Checked = false;
                    }
                }                 
            
                // Assign to session object.
                Session["dsTableReport"] = dsReport;
            
                // Binding column grid.
                this.ReportTableColumnDataBinder();

                // Binding summary grid.
                this.ReportTableSummaryDataBinder();
            }
        }

        /// <summary>
        /// Aspxgridview databinding for report table column.
        /// </summary>
        private void ReportTableColumnDataBinder()
        {
            grd_TableColumn.DataSource = dsReport.Tables[reportTableColumn.TableName];
            grd_TableColumn.DataBind();
        }

        /// <summary>
        /// Aspxgridview databinding for report table summary.
        /// </summary>
        private void ReportTableSummaryDataBinder()
        {
            grd_TableSummary.DataSource = dsReport.Tables[reportTableSummary.TableName];
            grd_TableSummary.DataBind();
        }
       
        /// <summary>
        /// Add new row for summary grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_SummaryNew_Click(object sender, EventArgs e)
        {
            // Create new row
            DataRow drReportSummaryColumn          = dsReport.Tables[reportTableSummary.TableName].NewRow();
            
            // Assign default value
            drReportSummaryColumn["ReportTableID"] = dsReport.Tables[reportTable.TableName].Rows[0]["ReportTableID"];

            if (dsReport.Tables[reportTableSummary.TableName].Rows.Count > 0)
            {
                drReportSummaryColumn["SummaryNo"] = (int)dsReport.Tables[reportTableSummary.TableName].Rows[dsReport.Tables[reportTableSummary.TableName].Rows.Count - 1]["SummaryNo"] + 1;
            }
            else
            {
                drReportSummaryColumn["SummaryNo"] = 1;
            } 

            // Add new row
            dsReport.Tables[reportTableSummary.TableName].Rows.Add(drReportSummaryColumn);

            // Refresh grid data
            grd_TableSummary.DataSource = dsReport.Tables[reportTableSummary.TableName];
            grd_TableSummary.EditIndex  = dsReport.Tables[reportTableSummary.TableName].Rows.Count - 1;
            grd_TableSummary.DataBind();

            // Save to session6
            Session["dsReport"]         = dsReport;

        }

        /// <summary>
        /// Data bound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_TableSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Blue.BL.GnxLib.ReportCategory category  = (Request.Params["category"] == null ? Blue.BL.GnxLib.ReportCategory.Consolidate : (Blue.BL.GnxLib.ReportCategory)Enum.Parse(typeof(Blue.BL.GnxLib.ReportCategory), Request.Params["category"].ToString()));
            
            
            // Get dropdownlist datasource
            //int dataSourceID                = int.Parse(((DropDownList)Parent.FindControl("ddl_DataSource")).SelectedItem.Value.ToString());
            int dataSourceID                  = int.Parse(((DropDownList)Parent.FindControl("ddl_DataSource")).SelectedItem.Value.ToString() == string.Empty ? ("0").ToString() : ((DropDownList)Parent.FindControl("ddl_DataSource")).SelectedItem.Value.ToString());
            

            DataTable dtDataSourceColumn = (category == Blue.BL.GnxLib.ReportCategory.Consolidate ?
                        field.GetList(dataSource.GetTableSchema(dataSourceID, string.Empty), dataSource.GetTableName(dataSourceID, string.Empty), string.Empty) :
                        field.GetList(dataSource.GetTableSchema(dataSourceID, LoginInfo.ConnStr), dataSource.GetTableName(dataSourceID, LoginInfo.ConnStr), LoginInfo.ConnStr));

            // Binding item
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //FieldName
                if (e.Row.FindControl("lbl_FieldName") != null)
                {
                    Label lbl_FieldName = (Label)e.Row.FindControl("lbl_FieldName");
                    lbl_FieldName.Text = (DataBinder.Eval(e.Row.DataItem, "FieldID") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "FieldID").ToString());
                                          
                }
                if (e.Row.FindControl("ddl_FieldName") != null)
                {
                    DropDownList ddl_FieldName = (DropDownList)e.Row.FindControl("ddl_FieldName");

                    if (dtDataSourceColumn != null)
                    {
                        ddl_FieldName.DataSource     = dtDataSourceColumn;
                        ddl_FieldName.DataTextField  = (LoginInfo.BuFmtInfo.LangCode.ToUpper() == "EN-US" ? "Desc" : "OthDesc");
                        ddl_FieldName.DataValueField = "Desc";
                        ddl_FieldName.DataBind();
                        ddl_FieldName.SelectedValue  = (DataBinder.Eval(e.Row.DataItem, "FieldID") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "FieldID").ToString());
                    }
                }

                //ShowInColumn
                if (e.Row.FindControl("lbl_ShowInColumn") != null)
                {
                    Label lbl_ShowInColumn  = (Label)e.Row.FindControl("lbl_ShowInColumn");
                    lbl_ShowInColumn.Text   = (DataBinder.Eval(e.Row.DataItem, "ShowInColumn") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "ShowInColumn").ToString());
                                              
                        
                }
                if (e.Row.FindControl("ddl_ShowInColumn") != null)
                {
                    DropDownList ddl_ShowInColumn = (DropDownList)e.Row.FindControl("ddl_ShowInColumn");

                    if (dtDataSourceColumn != null)
                    {
                        ddl_ShowInColumn.DataSource     = dtDataSourceColumn;
                        ddl_ShowInColumn.DataTextField  = (LoginInfo.BuFmtInfo.LangCode.ToUpper() == "EN-US" ? "Desc" : "OthDesc");
                        ddl_ShowInColumn.DataValueField = "Desc";
                        ddl_ShowInColumn.DataBind();
                        ddl_ShowInColumn.SelectedValue  = (DataBinder.Eval(e.Row.DataItem, "FieldID") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "FieldID").ToString());
                    }
                }

                //Summary Mode
                if (e.Row.FindControl("lbl_SummaryMode") != null)
                {
                    Label lbl_SummaryMode = (Label)e.Row.FindControl("lbl_SummaryMode");
                    lbl_SummaryMode.Text  = (DataBinder.Eval(e.Row.DataItem, "SummaryMode") == DBNull.Value ? string.Empty : (DataBinder.Eval(e.Row.DataItem, "SummaryMode").ToString()));
                }

                if (e.Row.FindControl("ddl_SummaryMode") != null)
                {
                    DataTable dtLookUpSummaryMode   = (category == Blue.BL.GnxLib.ReportCategory.Consolidate ?
                                                       lookupItem.GetList("24867516-89c0-4a82-ad48-a9df46376641", string.Empty) :
                                                       lookupItem.GetList("24867516-89c0-4a82-ad48-a9df46376641", LoginInfo.ConnStr));

                    DropDownList ddl_SummaryMode    = (DropDownList)e.Row.FindControl("ddl_SummaryMode");

                    if (dtDataSourceColumn != null)
                    {
                        ddl_SummaryMode.DataSource      = dtLookUpSummaryMode;
                        ddl_SummaryMode.DataTextField   = "Text";
                        ddl_SummaryMode.DataValueField  = "Value";
                        ddl_SummaryMode.DataBind();
                        ddl_SummaryMode.SelectedValue   = (DataBinder.Eval(e.Row.DataItem, "SummaryMode") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "SummaryMode").ToString());
                    }
                }

                //Summary Type
                if (e.Row.FindControl("lbl_SummaryType") != null)
                {
                    Label lbl_SummaryType = (Label)e.Row.FindControl("lbl_SummaryType");
                    lbl_SummaryType.Text  = (DataBinder.Eval(e.Row.DataItem, "SummaryType") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "SummaryType").ToString());
                }
                if (e.Row.FindControl("ddl_SummaryType") != null)
                {
                    DataTable dtLookUpSummaryType   = (category == Blue.BL.GnxLib.ReportCategory.Consolidate ?
                                                       lookupItem.GetList("199cdb75-0e17-42a4-9d28-0a53eb96660d", string.Empty) :
                                                       lookupItem.GetList("199cdb75-0e17-42a4-9d28-0a53eb96660d", LoginInfo.ConnStr));

                    DropDownList ddl_SummaryType    = (DropDownList)e.Row.FindControl("ddl_SummaryType");

                    if (dtDataSourceColumn != null)
                    {
                        ddl_SummaryType.DataSource      = dtLookUpSummaryType;
                        ddl_SummaryType.DataTextField   = "Text";
                        ddl_SummaryType.DataValueField  = "Value";
                        ddl_SummaryType.DataBind();
                        ddl_SummaryType.SelectedValue   = (DataBinder.Eval(e.Row.DataItem, "SummaryType") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "SummaryType").ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Delete report table summary data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_TableSummary_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Deleted report chart series.
            for (int i = dsReport.Tables[reportTableSummary.TableName].Rows.Count - 1; i >= 0; i--)
            {
                DataRow drDelete = dsReport.Tables[reportTableSummary.TableName].Rows[i];

                if (drDelete.RowState != DataRowState.Deleted)
                {
                    // Equal condition for delete.
                    if (drDelete["ReportTableID"].ToString() == grd_TableSummary.DataKeys[e.RowIndex].Values[0].ToString() &&
                        drDelete["SummaryNo"].ToString() == grd_TableSummary.DataKeys[e.RowIndex].Values[1].ToString())
                    {
                        drDelete.Delete();
                    }
                }
            }

            // Refresh gird data.
            grd_TableSummary.DataSource = dsReport.Tables[reportTableSummary.TableName];
            grd_TableSummary.EditIndex  = -1;
            grd_TableSummary.DataBind();

            // Save to session
            Session["dsReport"] = dsReport;
        }

        /// <summary>
        /// Canceling add new or edit report table summary.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_TableSummary_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Check row for cancle
            if (dsReport.Tables[reportTableSummary.TableName].Rows[e.RowIndex].RowState == DataRowState.Added)
            {
                dsReport.Tables[reportTableSummary.TableName].Rows[e.RowIndex].Delete();
            }

            // Refresh grid data.
            grd_TableSummary.DataSource = dsReport.Tables[reportTableSummary.TableName];
            grd_TableSummary.EditIndex  = -1;
            grd_TableSummary.DataBind();

            // Save to session
            Session["dsReport"] = dsReport;
        }

        /// <summary>
        /// Edit report table summary data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_TableSummary_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grd_TableSummary.DataSource = dsReport.Tables[reportTableSummary.TableName];
            grd_TableSummary.EditIndex  = e.NewEditIndex;
            grd_TableSummary.DataBind();
        }

        /// <summary>
        /// Update report table summary data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_TableSummary_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DropDownList ddl_FieldName    = (DropDownList)grd_TableSummary.Rows[e.RowIndex].FindControl("ddl_FieldName");
            DropDownList ddl_ShowInColumn = (DropDownList)grd_TableSummary.Rows[e.RowIndex].FindControl("ddl_ShowInColumn");
            DropDownList ddl_SummaryMode  = (DropDownList)grd_TableSummary.Rows[e.RowIndex].FindControl("ddl_SummaryMode");
            DropDownList ddl_SummaryType  = (DropDownList)grd_TableSummary.Rows[e.RowIndex].FindControl("ddl_SummaryType");
                        
            // Updating to datarow
            DataRow drUpdate = dsReport.Tables[reportTableSummary.TableName].Rows[e.RowIndex];

            if (grd_TableSummary.Columns[1].Visible)
            {
                drUpdate["FieldID"] = ddl_FieldName.SelectedItem.Value;
            }
            else
            {
                drUpdate["FieldID"] = DBNull.Value;
            }
            
            if (grd_TableSummary.Columns[2].Visible)
            {
                drUpdate["ShowInColumn"] = ddl_ShowInColumn.SelectedItem.Value;
            }
            else
            {
                drUpdate["ShowInColumn"] = DBNull.Value;
            }

            if (grd_TableSummary.Columns[3].Visible)
            {
                drUpdate["SummaryMode"] = ddl_SummaryMode.SelectedItem.Value;
            }
            else
            {
                drUpdate["SummaryMode"] = DBNull.Value;
            }

            if (grd_TableSummary.Columns[4].Visible)
            {
                drUpdate["SummaryType"] = ddl_SummaryType.SelectedItem.Value;
            }
            else
            {
                drUpdate["SummaryType"] = DBNull.Value;
            }
            
            // Refresh gird data.
            grd_TableSummary.DataSource = dsReport.Tables[reportTableSummary.TableName];
            grd_TableSummary.EditIndex  = -1;
            grd_TableSummary.DataBind();

            // Save to session
            Session["dsReport"] = dsReport;
        }

        /// <summary>
        /// Add new report table column.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_New_Click(object sender, EventArgs e)
        {
            // Create new row
            DataRow drReportTableColumn          = dsReport.Tables[reportTableColumn.TableName].NewRow();

            // Assign default value
            drReportTableColumn["ReportTableID"] = dsReport.Tables[reportTable.TableName].Rows[0]["ReportTableID"];

            if (dsReport.Tables[reportTableColumn.TableName].Rows.Count > 0)
            {
                drReportTableColumn["ColumnNo"]  = (int)dsReport.Tables[reportTableColumn.TableName].Rows[dsReport.Tables[reportTableColumn.TableName].Rows.Count - 1]["ColumnNo"] + 1;
            }
            else
            {
                drReportTableColumn["ColumnNo"]  = 1;
            }

            // Add new row
            dsReport.Tables[reportTableColumn.TableName].Rows.Add(drReportTableColumn);

            // Refresh grid data
            grd_TableColumn.DataSource          = dsReport.Tables[reportTableColumn.TableName];
            grd_TableColumn.EditIndex           = dsReport.Tables[reportTableColumn.TableName].Rows.Count - 1;
            grd_TableColumn.DataBind();

            // Save to session
            Session["dsReport"]                 = dsReport;
        }

        /// <summary>
        /// Data bound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_TableColumn_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Blue.BL.GnxLib.ReportCategory category  = (Request.Params["category"] == null ? Blue.BL.GnxLib.ReportCategory.Consolidate : (Blue.BL.GnxLib.ReportCategory)Enum.Parse(typeof(Blue.BL.GnxLib.ReportCategory), Request.Params["category"].ToString()));
           

            // Get dropdownlist datasource
            //int dataSourceID               = int.Parse(((DropDownList)Parent.FindControl("ddl_DataSource")).SelectedItem.Value.ToString());
            int dataSourceID                 = int.Parse(((DropDownList)Parent.FindControl("ddl_DataSource")).SelectedItem.Value.ToString() == string.Empty ? ("0").ToString() : ((DropDownList)Parent.FindControl("ddl_DataSource")).SelectedItem.Value.ToString());
           

            DataTable dtDataSourceColumn = (category == Blue.BL.GnxLib.ReportCategory.Consolidate ?
                        field.GetList(dataSource.GetTableSchema(dataSourceID, string.Empty), dataSource.GetTableName(dataSourceID, string.Empty), string.Empty) :
                        field.GetList(dataSource.GetTableSchema(dataSourceID, LoginInfo.ConnStr), dataSource.GetTableName(dataSourceID, LoginInfo.ConnStr), LoginInfo.ConnStr));

            // Binding item
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //FieldName
                if (e.Row.FindControl("lbl_FieldName") != null)
                {
                    Label lbl_FieldName = (Label)e.Row.FindControl("lbl_FieldName");
                    lbl_FieldName.Text  = (DataBinder.Eval(e.Row.DataItem, "FieldID") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "FieldID").ToString());
                    
                }

                if (e.Row.FindControl("ddl_FieldName") != null)
                {
                    DropDownList ddl_FieldName = (DropDownList)e.Row.FindControl("ddl_FieldName");
                                    
                    if (dtDataSourceColumn != null)
                    {
                        ddl_FieldName.DataSource     = dtDataSourceColumn;
                        ddl_FieldName.DataTextField  = (LoginInfo.BuFmtInfo.LangCode.ToUpper() == "EN-US" ? "Desc" : "OthDesc");
                        ddl_FieldName.DataValueField = "Desc";
                        ddl_FieldName.DataBind();
                        ddl_FieldName.SelectedValue  = (DataBinder.Eval(e.Row.DataItem, "FieldID") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "FieldID").ToString());
                                                                               
                    }
                }

                //Caption
                if (e.Row.FindControl("lbl_Caption") != null)
                {
                    Label lbl_Caption    = (Label)e.Row.FindControl("lbl_Caption");
                    lbl_Caption.Text     = (DataBinder.Eval(e.Row.DataItem, "Caption") == DBNull.Value ? string.Empty : (DataBinder.Eval(e.Row.DataItem, "Caption").ToString()));                                         
                    
                }
                if (e.Row.FindControl("txt_Caption") != null)
                {
                    TextBox txt_Caption = (TextBox)e.Row.FindControl("txt_Caption");
                    txt_Caption.Text    = (DataBinder.Eval(e.Row.DataItem, "Caption") == DBNull.Value ? string.Empty : (DataBinder.Eval(e.Row.DataItem, "Caption").ToString()));
                }

                //GroupIndex
                if (e.Row.FindControl("lbl_GroupIndex") != null)
                {
                    Label lbl_GroupIndex = (Label)e.Row.FindControl("lbl_GroupIndex");
                    lbl_GroupIndex.Text  = (DataBinder.Eval(e.Row.DataItem, "GroupIndex") == DBNull.Value ? string.Empty : (DataBinder.Eval(e.Row.DataItem, "GroupIndex").ToString()));
                }
                if (e.Row.FindControl("txt_GroupIndex") != null)
                {
                    TextBox txt_GroupIndex = (TextBox)e.Row.FindControl("txt_GroupIndex");
                    txt_GroupIndex.Text    = (DataBinder.Eval(e.Row.DataItem, "GroupIndex") == DBNull.Value ? string.Empty : (DataBinder.Eval(e.Row.DataItem, "GroupIndex").ToString()));
                }

                //Column Width
                if (e.Row.FindControl("lbl_ColumnWidth") != null)
                {
                    Label lbl_ColumnWidth = (Label)e.Row.FindControl("lbl_ColumnWidth");
                    lbl_ColumnWidth.Text  = (DataBinder.Eval(e.Row.DataItem, "Width") == DBNull.Value ? string.Empty : (DataBinder.Eval(e.Row.DataItem, "Width").ToString()));
                } 
                if (e.Row.FindControl("txt_ColumnWidth") != null)
                {
                    TextBox txt_ColumnWidth = (TextBox)e.Row.FindControl("txt_ColumnWidth");
                    txt_ColumnWidth.Text    = (DataBinder.Eval(e.Row.DataItem, "Width") == DBNull.Value ? string.Empty : (DataBinder.Eval(e.Row.DataItem, "Width").ToString()));
                }
            }
        }

        /// <summary>
        /// Delete report table column data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_TableColumn_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Deleted report table column
            for (int i = dsReport.Tables[reportTableColumn.TableName].Rows.Count - 1; i >= 0; i--)
            {
                DataRow drDelete = dsReport.Tables[reportTableColumn.TableName].Rows[i];

                if (drDelete.RowState != DataRowState.Deleted)
                {
                    // Equal condition for delete.
                    if (drDelete["ReportTableID"].ToString() == grd_TableColumn.DataKeys[e.RowIndex].Values[0].ToString() &&
                        drDelete["ColumnNo"].ToString()      == grd_TableColumn.DataKeys[e.RowIndex].Values[1].ToString())
                    {
                        drDelete.Delete();
                    }
                }
            }

            // Refresh gird data.
            grd_TableColumn.DataSource = dsReport.Tables[reportTableColumn.TableName];
            grd_TableColumn.EditIndex  = -1;
            grd_TableColumn.DataBind();

            // Save to session
            Session["dsReport"]        = dsReport;
        }

        /// <summary>
        /// Canceling add new or edit report table column.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_TableColumn_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Check row for cancle
            if (dsReport.Tables[reportTableColumn.TableName].Rows[e.RowIndex].RowState == DataRowState.Added)
            {
                dsReport.Tables[reportTableColumn.TableName].Rows[e.RowIndex].Delete();
            }

            // Refresh grid data.
            grd_TableColumn.DataSource = dsReport.Tables[reportTableColumn.TableName];
            grd_TableColumn.EditIndex  = -1;
            grd_TableColumn.DataBind();

            // Enable add button
            btn_New.Enabled = true;

            // Save to session
            Session["dsReport"] = dsReport;
        }
        
        /// <summary>
        /// Edit report table column data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_TableColumn_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grd_TableColumn.DataSource = dsReport.Tables[reportTableColumn.TableName];
            grd_TableColumn.EditIndex  = e.NewEditIndex;
            grd_TableColumn.DataBind();
        }

        /// <summary>
        /// Update report table column data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_TableColumn_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {   
            DropDownList ddl_FieldName  = (DropDownList)grd_TableColumn.Rows[e.RowIndex].FindControl("ddl_FieldName");
            //DropDownList ddl_Caption  = (DropDownList)grd_TableColumn.Rows[e.RowIndex].FindControl("ddl_FieldName");
            TextBox txt_Caption         = (TextBox)grd_TableColumn.Rows[e.RowIndex].FindControl("txt_Caption");
            TextBox txt_GroupIndex      = (TextBox)grd_TableColumn.Rows[e.RowIndex].FindControl("txt_GroupIndex");
            TextBox txt_ColumnWidth     = (TextBox)grd_TableColumn.Rows[e.RowIndex].FindControl("txt_ColumnWidth");

            // Updating to datarow

            DataRow drUpdate = dsReport.Tables[reportTableColumn.TableName].Rows[e.RowIndex];

            if (grd_TableColumn.Columns[1].Visible)
            {
                drUpdate["FieldID"] = ddl_FieldName.SelectedItem.Value;
            }
            else
            {
                drUpdate["FieldID"] = DBNull.Value;
            }

            drUpdate["Caption"]     = (txt_Caption.Text == string.Empty ? string.Empty : txt_Caption.Text);
            drUpdate["GroupIndex"]  = (txt_GroupIndex.Text == string.Empty ? 0 : int.Parse(txt_GroupIndex.Text.ToString()));
            drUpdate["Width"]       = (txt_ColumnWidth.Text == string.Empty ? 0 : int.Parse(txt_ColumnWidth.Text.ToString()));
            
            // Refresh gird data.
            grd_TableColumn.DataSource   = dsReport.Tables[reportTableColumn.TableName];
            grd_TableColumn.EditIndex    = -1;
            grd_TableColumn.DataBind();

            // Save to session
            Session["dsReport"] = dsReport;
        }
        
        #endregion
    }
}
