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
    public partial class PivotGridReport : BaseUserControl
    {
        #region "Attributes"
        
        private DataSet dsReport                            = new DataSet();
        private Blue.BL.Report.ReportPivot reportPivot           = new Blue.BL.Report.ReportPivot();
        private Blue.BL.Report.ReportPivotField reportPivotField = new Blue.BL.Report.ReportPivotField();
        
        //private BL.Application.Field field                  = new BlueLedger.BL.Application.Field();
        private Blue.BL.APP.Field field                          = new Blue.BL.APP.Field();
        
        private Blue.BL.Report.Report report                     = new Blue.BL.Report.Report();
        private Blue.BL.Report.DataSource dataSource             = new Blue.BL.Report.DataSource();         
        private int _reportID;
        private Blue.BL.GnxLib.ReportCategory _category;

        /// <summary>
        /// Get report id from setup page.
        /// </summary>
        public int ReportID
        {
            get { return this._reportID; }
            set { this._reportID = value; }
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
                    // Report TypeID = 3 is type of Privot Report
                    if ((int)dsReport.Tables[report.TableName].Rows[0]["Type"] == 3)
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
            
                //if (!IsPostBack)
                //{
                //    Page_Retrieve();
                //    Page_Setting();
                //}
                //else
                //{
                //    // Assign Session value to dataset.
                //    dsReport = (DataSet)Session["dsReport"];
                //}
            
        }
        
        /// <summary>
        /// Retrieve data.
        /// </summary>
        private void Page_Retrieve()
        {
            Blue.BL.GnxLib.ReportCategory category = (Request.Params["category"] == null ? Blue.BL.GnxLib.ReportCategory.Consolidate : (Blue.BL.GnxLib.ReportCategory)Enum.Parse(typeof(Blue.BL.GnxLib.ReportCategory), Request.Params["category"].ToString()));

            // Instant report id
            dsReport = (DataSet)Session["dsReport"];

            if (dsReport.Tables[report.TableName].Rows.Count > 0)
            {
                int Report_ID = (int)dsReport.Tables[report.TableName].Rows[0]["ReportID"];

                if (Request.Params["action"].ToUpper() == "NEW")
                {
                    // Get report chart structure.
                    if (category == Blue.BL.GnxLib.ReportCategory.Consolidate)
                    {
                        reportPivot.GetReportPivotSchema(dsReport, string.Empty);
                    }
                    else
                    {
                        reportPivot.GetReportPivotSchema(dsReport, LoginInfo.ConnStr);
                    }

                    // Add new row.
                    DataRow drReportPivot = dsReport.Tables[reportPivot.TableName].NewRow();
                    drReportPivot["ReportPivotID"] = (category == Blue.BL.GnxLib.ReportCategory.Consolidate ?
                                                       reportPivot.GetNewReportPivotID(string.Empty) :
                                                       reportPivot.GetNewReportPivotID(LoginInfo.ConnStr)) + 1;
                    drReportPivot["ReportID"]      = Report_ID;

                    dsReport.Tables[reportPivot.TableName].Rows.Add(drReportPivot);

                    // Get schema table report pivot field.
                    if (category == Blue.BL.GnxLib.ReportCategory.Consolidate)
                    {
                        reportPivotField.GetReportPivotFieldSchema(dsReport, string.Empty);
                    }
                    else
                    {
                        reportPivotField.GetReportPivotFieldSchema(dsReport, LoginInfo.ConnStr);
                    }
                }
                else
                //if(Request.Params["action"].ToUpper() == "EDIT")
                {
                    // Get report pivot data by reportid.
                    if (category == Blue.BL.GnxLib.ReportCategory.Consolidate)
                    {
                        reportPivot.GetListByReportID(dsReport, Report_ID, string.Empty);
                    }
                    else
                    {
                        reportPivot.GetListByReportID(dsReport, Report_ID, LoginInfo.ConnStr);
                    }

                    if (dsReport.Tables[reportPivot.TableName].Rows.Count > 0)
                    {
                        // Get report pivot field.
                        int reportPivotID = (int)dsReport.Tables[reportPivot.TableName].Rows[0]["ReportPivotID"];

                        if (category == Blue.BL.GnxLib.ReportCategory.Consolidate)
                        {
                            reportPivotField.GetListByReportPivotID(dsReport, Convert.ToInt32(reportPivotID), string.Empty);
                        }
                        else
                        {
                            reportPivotField.GetListByReportPivotID(dsReport, Convert.ToInt32(reportPivotID), LoginInfo.ConnStr);
                        }                        
                    }                    
                }
            }

            // Save to session
            Session["dsReport"] = dsReport;
        }

        /// <summary>
        /// Setting data to controls.
        /// </summary>
        private void Page_Setting()
        {
            if (dsReport.Tables[reportPivot.TableName].Rows.Count > 0)
            {
                // Binding all of reportpivot label control.
                this.txt_PageSize.Text = (dsReport.Tables[reportPivot.TableName].Rows[0]["PageSize"].ToString());
                this.txt_Title.Text    = (dsReport.Tables[reportPivot.TableName].Rows[0]["Title"].ToString());

                if (dsReport.Tables[reportPivot.TableName].Rows[0]["IsShowPager"] != DBNull.Value)
                {
                    bool isShowPager   = Convert.ToBoolean(dsReport.Tables[reportPivot.TableName].Rows[0]["IsShowPager"]);

                    // For show pager.
                    if (isShowPager)
                    {
                        chk_ShowPager.Checked = true;
                    }
                    else
                    {
                        chk_ShowPager.Checked = false;
                    }
                }               

                // Assign to session for changes effect.
                Session["dsReportPivot"] = dsReport;

                // Binding reportpivotfield column grid.
                this.ReportPivotFieldDataBinder();
            }            
        }

        /// <summary>
        /// Aspxgridview databinding for reportpivot.
        /// </summary>
        private void ReportPivotFieldDataBinder()
        {
            grd_ReportPivot.DataSource = dsReport.Tables[reportPivotField.TableName];
            grd_ReportPivot.DataBind();
        }
        
        /// <summary>
        /// Row databound event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ReportPivot_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Blue.BL.GnxLib.ReportCategory category = (Request.Params["category"] == null ? Blue.BL.GnxLib.ReportCategory.Consolidate : (Blue.BL.GnxLib.ReportCategory)Enum.Parse(typeof(Blue.BL.GnxLib.ReportCategory), Request.Params["category"].ToString()));

            // Get dropdownlist datasource            
            int dataSourceID = int.Parse(((DropDownList)Parent.FindControl("ddl_DataSource")).SelectedItem.Value.ToString() == string.Empty ? ("0").ToString() : ((DropDownList)Parent.FindControl("ddl_DataSource")).SelectedItem.Value.ToString());
            
            DataTable dtDataSourceColumn = (category == Blue.BL.GnxLib.ReportCategory.Consolidate ?
                        field.GetList(dataSource.GetTableSchema(dataSourceID, string.Empty), dataSource.GetTableName(dataSourceID, string.Empty), string.Empty) :
                        field.GetList(dataSource.GetTableSchema(dataSourceID, LoginInfo.ConnStr), dataSource.GetTableName(dataSourceID, LoginInfo.ConnStr), LoginInfo.ConnStr));
            
            if (dtDataSourceColumn != null)
            {
                // Insert blank row
                DataRow drBlank = dtDataSourceColumn.NewRow();
                dtDataSourceColumn.Rows.InsertAt(drBlank, 0);
            }
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow drPivotField = dsReport.Tables[reportPivotField.TableName].Rows[e.Row.DataItemIndex];

                // Field ID.
                if (e.Row.FindControl("lbl_FieldID") != null)
                {
                    Label lbl_FieldID  = (Label)e.Row.FindControl("lbl_FieldID");

                    lbl_FieldID.Text   = drPivotField["FieldName"].ToString();
                }
                
                if (e.Row.FindControl("ddl_FieldID") != null)
                {
                    if (dtDataSourceColumn != null)
                    {
                        DropDownList ddl_FieldID   = (DropDownList)e.Row.FindControl("ddl_FieldID");
                        ddl_FieldID.DataSource     = dtDataSourceColumn;
                        ddl_FieldID.DataTextField  = (LoginInfo.BuFmtInfo.LangCode.ToUpper() == "EN-US" ? "Desc" : "OthDesc");
                        ddl_FieldID.DataValueField = "Desc";
                        ddl_FieldID.DataBind();
                        ddl_FieldID.SelectedValue  = drPivotField["FieldName"].ToString();
                    }
                }
                
                // Caption
                if (e.Row.FindControl("lbl_Caption") != null)
                {
                    Label lbl_Caption               = (Label)e.Row.FindControl("lbl_Caption");
                    lbl_Caption.Text                = DataBinder.Eval(e.Row.DataItem, "Caption").ToString();
                }
                
                if (e.Row.FindControl("txt_Caption") != null)
                {
                    TextBox txt_Caption             = (TextBox)e.Row.FindControl("txt_Caption");
                    txt_Caption.Text                = DataBinder.Eval(e.Row.DataItem, "Caption").ToString();
                }

                // FieldArea
                if (e.Row.FindControl("lbl_FieldArea") != null)
                {
                    Label lbl_FieldArea             = (Label)e.Row.FindControl("lbl_FieldArea");
                    lbl_FieldArea.Text              = DataBinder.Eval(e.Row.DataItem, "FieldArea").ToString();
                }

                if (e.Row.FindControl("ddl_FieldArea") != null)
                {
                    DropDownList ddl_FieldArea      = (DropDownList)e.Row.FindControl("ddl_FieldArea");
                    ddl_FieldArea.SelectedValue     = (DataBinder.Eval(e.Row.DataItem, "FieldArea") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "FieldArea").ToString());
                }
                

                // FieldAreaIndex
                if (e.Row.FindControl("lbl_FieldAreaIndex") != null)
                {
                    Label lbl_FieldAreaIndex        = (Label)e.Row.FindControl("lbl_FieldAreaIndex");
                    lbl_FieldAreaIndex.Text         = DataBinder.Eval(e.Row.DataItem, "FieldAreaIndex").ToString();
                }

                if (e.Row.FindControl("txt_FieldAreaIndex") != null)
                {
                    TextBox txt_FieldAreaIndex      = (TextBox)e.Row.FindControl("txt_FieldAreaIndex");
                    txt_FieldAreaIndex.Text         = DataBinder.Eval(e.Row.DataItem, "FieldAreaIndex").ToString();
                }
                
            }
        }
        
        /// <summary>
        /// Row updating event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ReportPivot_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {            
            DataRow drUpdating          = dsReport.Tables[reportPivotField.TableName].Rows[e.RowIndex];

            // Find controls on edited row
            TextBox txt_Caption         = (TextBox)grd_ReportPivot.Rows[e.RowIndex].FindControl("txt_Caption");
            TextBox txt_FieldAreaIndex  = (TextBox)grd_ReportPivot.Rows[e.RowIndex].FindControl("txt_FieldAreaIndex");
            DropDownList ddl_FieldID    = (DropDownList)grd_ReportPivot.Rows[e.RowIndex].FindControl("ddl_FieldID");
            DropDownList ddl_FieldArea  = (DropDownList)grd_ReportPivot.Rows[e.RowIndex].FindControl("ddl_FieldArea");

            // Updating for assign data.
            
            // FieldName
            if (ddl_FieldID.SelectedItem.Value == string.Empty)
            {
                drUpdating["FieldName"] = DBNull.Value;
            }
            else
            {
                drUpdating["FieldName"] = ddl_FieldID.SelectedItem.Value;
            }
            
            
            // FieldArea
            //drUpdating["FieldArea"]     = ddl_FieldArea.SelectedItem.Value.ToString();
            if (ddl_FieldArea.SelectedItem.Value.ToString() == string.Empty)
            {
                drUpdating["FieldArea"] = DBNull.Value;
            }
            else
            {
                drUpdating["FieldArea"] = ddl_FieldArea.SelectedItem.Value.ToString();
            }            
            
            // Caption
            drUpdating["Caption"]       = txt_Caption.Text.Trim();

            // FieldAreaIndex
            drUpdating["FieldAreaIndex"]= (txt_FieldAreaIndex.Text.Trim() == string.Empty ? 0 : Convert.ToInt32(txt_FieldAreaIndex.Text.Trim())); 
            
            // Release editing row
            grd_ReportPivot.EditIndex   = -1;
            this.ReportPivotFieldDataBinder();

            // Save fresh data to session
            Session["dsReport"] = dsReport;            
        }
        
        /// <summary>
        /// Row editing event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ReportPivot_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grd_ReportPivot.EditIndex = e.NewEditIndex;
            this.ReportPivotFieldDataBinder();

        }
        
        /// <summary>
        /// Row canceling event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ReportPivot_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // If canceled row is new row, delete the new row.
            if (dsReport.Tables[reportPivotField.TableName].Rows[e.RowIndex].RowState == DataRowState.Added)
            {
                dsReport.Tables[reportPivotField.TableName].Rows[e.RowIndex].Delete();
            }
            
            grd_ReportPivot.EditIndex = -1;
            this.ReportPivotFieldDataBinder();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ReportPivot_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Get fieldNo from deleted row
            int fieldNo = (int)grd_ReportPivot.DataKeys[e.RowIndex].Value;

            // Delete the record 
            for (int i = dsReport.Tables[reportPivotField.TableName].Rows.Count - 1; i >= 0; i--)
            {
                DataRow drReportPivotGrid       = dsReport.Tables[reportPivotField.TableName].Rows[i];

                if (drReportPivotGrid.RowState != DataRowState.Deleted)
                {
                    if ((int)drReportPivotGrid["FieldNo"] == fieldNo)
                    {
                        drReportPivotGrid.Delete();
                    }
                }
            }

            // Binding grid
            this.ReportPivotFieldDataBinder();
        }

        /// <summary>
        /// Create new row.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_New_Click(object sender, EventArgs e)
        {
            // Create new row.
            DataRow drReportPivotField = dsReport.Tables[reportPivotField.TableName].NewRow();

            // Assign default value for new process.
            drReportPivotField["ReportPivotID"] = dsReport.Tables[reportPivot.TableName].Rows[0]["ReportPivotID"];

            // For FieldNo new process.
            if (dsReport.Tables[reportPivotField.TableName].Rows.Count > 0)
            {
                drReportPivotField["FieldNo"] = (int)dsReport.Tables[reportPivotField.TableName].Rows[dsReport.Tables[reportPivotField.TableName].Rows.Count - 1]["FieldNo"] + 1;
            }
            else
            {
                drReportPivotField["FieldNo"] = 1;
            }
            
            // Add new row
            dsReport.Tables[reportPivotField.TableName].Rows.Add(drReportPivotField);
            
            // Refresh data in GridView
            grd_ReportPivot.EditIndex = dsReport.Tables[reportPivotField.TableName].Rows.Count - 1;

            // Set editing row
            this.ReportPivotFieldDataBinder();

            // Store new data to session
            Session["dsReport"] = dsReport;
        }

        #endregion        
    }
}
