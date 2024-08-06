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
using BlueLedger.PL.BaseClass;

using BlueLedger;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxGridView.Export;

namespace BlueLedger.PL.UserControls.Report
{    
    public partial class Table : BaseUserControl
    {
        #region "Attributies"
        
        int reportTableID;
        private int ReportID;
        private Blue.BL.GnxLib.ReportCategory _category;
        private DataTable dtReport                  = new DataTable();
        DataSet dsReportTable                       = new DataSet();
        DataTable dtTest                            = new DataTable();
        
        Blue.BL.Application.Field field                  = new Blue.BL.Application.Field();
        //BL.APP.Field field = new BL.APP.Field();

        Blue.BL.Report.ReportTable        reportTable    = new Blue.BL.Report.ReportTable();
        Blue.BL.Report.ReportTableColumn  reportColumn   = new Blue.BL.Report.ReportTableColumn();
        Blue.BL.Report.ReportTableSummary reportSummary  = new Blue.BL.Report.ReportTableSummary();
        Blue.BL.Report.Report report                     = new Blue.BL.Report.Report();
        
        /// <summary>
        /// Get or Set ReportCategory of ChartReport UserControl.
        /// </summary>
        public Blue.BL.GnxLib.ReportCategory Category
        {
            get { return this._category; }
            set { this._category = value; }
        
            //get
            //{
            //    _category = (Blue.BL.GnxLib.ReportCategory)ViewState["Category"];
            //    return _category;
            //}
            //set
            //{
            //    _category = value;
            //    ViewState["Category"] = _category;
            //}
        
        }

        #endregion

        #region "Operations"

        /// <summary>
        /// Retrieve data
        /// </summary>
        private void Page_Retrieve()
        {
            Blue.BL.GnxLib.ReportCategory category  = (Request.Params["category"] == null ? Blue.BL.GnxLib.ReportCategory.Consolidate : (Blue.BL.GnxLib.ReportCategory)Enum.Parse(typeof(Blue.BL.GnxLib.ReportCategory), Request.Params["category"].ToString()));
            ReportID                        = int.Parse(Request.Params["reportid"].ToString());
            
            if (category == Blue.BL.GnxLib.ReportCategory.Consolidate)
            {
                reportTable.GetListByReportID(dsReportTable, ReportID, string.Empty);
            }
            else
            {
                reportTable.GetListByReportID(dsReportTable, ReportID, LoginInfo.ConnStr);
            }           

            reportTableID = (int)dsReportTable.Tables[reportTable.TableName].Rows[0]["ReportTableID"];

            if (category == Blue.BL.GnxLib.ReportCategory.Consolidate)
            {
                reportColumn.GetListByReportTableID(dsReportTable, reportTableID, string.Empty);
                reportSummary.GetListByReportTableID(dsReportTable, reportTableID, string.Empty);
            }
            else
            {
                reportColumn.GetListByReportTableID(dsReportTable, reportTableID, LoginInfo.ConnStr);
                reportSummary.GetListByReportTableID(dsReportTable, reportTableID, LoginInfo.ConnStr);
            }

            // Get report data.
            dtReport = (DataTable)Session["dtReport"];

            if (dtReport != null)
            {
                dtReport.TableName = "Report";
                dsReportTable.Tables.Add(dtReport);
            }
           
            Session["dsReportTable"] = dsReportTable;
        }

        /// <summary>
        /// Setting data
        /// </summary>
        private void Page_Setting()
        {
            Blue.BL.GnxLib.ReportCategory category = (Request.Params["category"] == null ? Blue.BL.GnxLib.ReportCategory.Consolidate : (Blue.BL.GnxLib.ReportCategory)Enum.Parse(typeof(Blue.BL.GnxLib.ReportCategory), Request.Params["category"].ToString()));

            grd_Table.SettingsBehavior.AllowGroup = (bool)dsReportTable.Tables[reportTable.TableName].Rows[0]["AllowGroup"];
            grd_Table.SettingsBehavior.AllowSort  = (bool)dsReportTable.Tables[reportTable.TableName].Rows[0]["AllowSort"];
                        
            grd_Table.SettingsPager.PageSize      = (dsReportTable.Tables[reportTable.TableName].Rows[0]["PageSize"].ToString() == string.Empty ?
                                                     0 : (int)dsReportTable.Tables[reportTable.TableName].Rows[0]["PageSize"]);
            grd_Table.SettingsPager.Visible       = (bool)dsReportTable.Tables[reportTable.TableName].Rows[0]["IsShowPager"];

            grd_Table.SettingsText.Title          = dsReportTable.Tables[reportTable.TableName].Rows[0]["Title"].ToString();

            grd_Table.Settings.ShowTitlePanel     = (bool)dsReportTable.Tables[reportTable.TableName].Rows[0]["IsShowTitlePanel"];
            grd_Table.Settings.ShowGroupPanel     = (bool)dsReportTable.Tables[reportTable.TableName].Rows[0]["IsShowGroupPanel"];
            grd_Table.Settings.ShowFooter         = (bool)dsReportTable.Tables[reportTable.TableName].Rows[0]["IsShowFooter"];

            grd_Table.Settings.ShowGroupedColumns = false;
            //if 'grd_Table.Settings.ShowGroupedColumns   = true;' program will not set group.
            //grd_Table.Settings.ShowGroupedColumns    = (bool)dsReportTable.Tables[reportTable.TableName].Rows[0]["IsShowGroupedColumns"];

            if (dsReportTable.Tables[reportSummary.TableName].Rows.Count > 0)
            {
                for (int iii = 0; iii < dsReportTable.Tables[reportSummary.TableName].Rows.Count; iii++)
                {
                    //Set colume in grid
                    GridViewDataColumn aa   = new GridViewDataColumn();
                    aa.FieldName = (category == Blue.BL.GnxLib.ReportCategory.Consolidate ?
                                    field.GetFieldName(dsReportTable.Tables[reportSummary.TableName].Rows[iii]["FieldID"].ToString(), string.Empty) :
                                    field.GetFieldName(dsReportTable.Tables[reportSummary.TableName].Rows[iii]["FieldID"].ToString(), LoginInfo.ConnStr));
                        
                    grd_Table.Columns.Add(aa);

                    //Set GroupSummary and TotalSummary
                    string mode             = dsReportTable.Tables[reportSummary.TableName].Rows[iii]["SummaryMode"].ToString();

                    ASPxSummaryItem sumItem = new ASPxSummaryItem();
                    sumItem.FieldName       = (category == Blue.BL.GnxLib.ReportCategory.Consolidate ?
                                               field.GetFieldName(dsReportTable.Tables[reportSummary.TableName].Rows[iii]["FieldID"].ToString(), string.Empty) :
                                               field.GetFieldName(dsReportTable.Tables[reportSummary.TableName].Rows[iii]["FieldID"].ToString(), LoginInfo.ConnStr));

                    sumItem.ShowInColumn    = (category == Blue.BL.GnxLib.ReportCategory.Consolidate ?
                                               field.GetFieldName(dsReportTable.Tables[reportSummary.TableName].Rows[iii]["ShowInColumn"].ToString(), string.Empty) :
                                               field.GetFieldName(dsReportTable.Tables[reportSummary.TableName].Rows[iii]["ShowInColumn"].ToString(), LoginInfo.ConnStr));
                    
                    sumItem.DisplayFormat   = dsReportTable.Tables[reportSummary.TableName].Rows[iii]["DisplayFormat"].ToString();
                    sumItem.Tag             = dsReportTable.Tables[reportSummary.TableName].Rows[iii]["Tag"].ToString();
                                        
                    if (dsReportTable.Tables[reportSummary.TableName].Rows[0]["SummaryType"] != DBNull.Value)
                    {
                        sumItem.SummaryType = (DevExpress.Data.SummaryItemType)Enum.Parse(typeof(DevExpress.Data.SummaryItemType), dsReportTable.Tables[reportSummary.TableName].Rows[iii]["SummaryType"].ToString());
                    }

                    if (mode.ToUpper() == "T")
                    {
                        grd_Table.TotalSummary.Add(sumItem);
                    }

                    if (mode.ToUpper() == "G")
                    {
                        grd_Table.GroupSummary.Add(sumItem);
                    }
                }
            }            
            
            grd_Table.DataSource = dsReportTable.Tables[report.TableName];
            grd_Table.DataBind();
        }
    
        /// <summary>
        /// Page load event.
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
                    dsReportTable           = (DataSet)Session["dsReportTable"];
                    grd_Table.DataSource    = dsReportTable.Tables["Report"];
                    grd_Table.DataBind();
                }
            }
        }

        protected void btn_Ok_Click(object sender, EventArgs e)
        {   
            string value = ddl_SelectTypeReport.SelectedItem.Value.ToString();

            switch (value.ToUpper())
            {
                case "PDF":
                    this.grdexp_Table.WritePdfToResponse();

                    break;

                case "RTF":
                    this.grdexp_Table.WriteRtfToResponse();

                    break;

                case "XLS":
                    this.grdexp_Table.WriteXlsToResponse();

                    break;
            }            
        }

        #endregion                        
    }
}