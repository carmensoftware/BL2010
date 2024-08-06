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

using DevExpress;
using DevExpress.Web;
//using DevExpress.Web.ASPxPivotGrid;
//using DevExpress.XtraPivotGrid;

namespace BlueLedger.PL.UserControls.Report
{
    public partial class Pivot : BaseUserControl
    {
        #region "Attributies"

        private DataSet dsPivotGridReport                   = new DataSet();
        private DataTable dtReport                          = new DataTable();
        private int ReportID;

        private Blue.BL.Report.ReportPivot reportPivot           = new Blue.BL.Report.ReportPivot();
        private Blue.BL.Report.ReportPivotField reportPivotField = new Blue.BL.Report.ReportPivotField();
        Blue.BL.Report.DataSource dataSource = new Blue.BL.Report.DataSource();
        //private BL.Application.Field field                  = new BlueLedger.BL.Application.Field();
        private Blue.BL.APP.Field field = new Blue.BL.APP.Field();

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

        #region "Operations"

        /// <summary>
        /// Retrieve needed data from database.
        /// </summary>
        private void Page_Retrieve()
        {
            Blue.BL.GnxLib.ReportCategory category  = (Request.Params["category"] == null ? Blue.BL.GnxLib.ReportCategory.Consolidate : (Blue.BL.GnxLib.ReportCategory)Enum.Parse(typeof(Blue.BL.GnxLib.ReportCategory), Request.Params["category"].ToString()));
            ReportID                        = int.Parse(Request.Params["reportid"].ToString());            
             
            // ReportPivot
            if (category == Blue.BL.GnxLib.ReportCategory.Consolidate)
            {
                reportPivot.GetListByReportID(dsPivotGridReport, ReportID, string.Empty);
            }
            else
            {
                reportPivot.GetListByReportID(dsPivotGridReport, ReportID, LoginInfo.ConnStr);
            }            

            // ReportPivotField
            if (dsPivotGridReport.Tables[reportPivot.TableName] != null)
            {
                if (dsPivotGridReport.Tables[reportPivot.TableName].Rows.Count > 0)
                {
                    int reportPivotID = int.Parse(dsPivotGridReport.Tables[reportPivot.TableName].Rows[0]["ReportPivotID"].ToString());

                    if (category == Blue.BL.GnxLib.ReportCategory.Consolidate)
                    {
                        reportPivotField.GetListByReportPivotID(dsPivotGridReport, reportPivotID, string.Empty);
                    }
                    else
                    {
                        reportPivotField.GetListByReportPivotID(dsPivotGridReport, reportPivotID, LoginInfo.ConnStr);
                    }
                }
            }            

            // Get report data.
            dtReport = (DataTable)Session["dtReport"];

            if (dtReport != null)
            {
                dtReport.TableName = "Report";
                dsPivotGridReport.Tables.Add(dtReport);
            }

            Session["dsPivotGridReport"] = dsPivotGridReport;
        }

        /// <summary>
        /// Binding controls.
        /// </summary>
        private void Page_Setting()
        {
            Blue.BL.GnxLib.ReportCategory category = (Request.Params["category"] == null ? Blue.BL.GnxLib.ReportCategory.Consolidate : (Blue.BL.GnxLib.ReportCategory)Enum.Parse(typeof(Blue.BL.GnxLib.ReportCategory), Request.Params["category"].ToString()));

            if (dsPivotGridReport != null)
            {
                // Binding PivotGrid properties.
                if (dsPivotGridReport.Tables[reportPivot.TableName] != null)
                {
                    if (dsPivotGridReport.Tables[reportPivot.TableName].Rows.Count > 0)
                    {
                        DataRow drPivot = dsPivotGridReport.Tables[reportPivot.TableName].Rows[0];

                        //// Pager Visibility
                        //pg_PivotReport.OptionsPager.Visible     = (drPivot["IsShowPager"] == DBNull.Value ? false : (bool)drPivot["IsShowPager"]);

                        //// Page Size
                        //pg_PivotReport.OptionsPager.RowsPerPage = (drPivot["PageSize"]    == DBNull.Value ? 10 : (int)drPivot["PageSize"]);
                    }
                }                

                // Binding PivotGrid field.
                if (dsPivotGridReport.Tables[reportPivotField.TableName] != null)
                {                    
                    foreach (DataRow drPivotField in dsPivotGridReport.Tables[reportPivotField.TableName].Rows)
                    {
                        //DevExpress.Web.ASPxPivotGrid.PivotGridField pivotGridField = new DevExpress.Web.ASPxPivotGrid.PivotGridField();

                        if (drPivotField["FieldName"] != DBNull.Value)
                        {
                            //pivotGridField.FieldName = (category == Blue.BL.GnxLib.ReportCategory.Consolidate ?                                                       
                            //                            field.GetFieldName(drPivotField["FieldID"].ToString(), string.Empty) :
                            //                            field.GetFieldName(drPivotField["FieldID"].ToString(), LoginInfo.ConnStr));
                            //pivotGridField.FieldName = drPivotField["FieldName"].ToString();
                       }                        

                        if (drPivotField["Caption"] != DBNull.Value)
                        {
                            //pivotGridField.Caption = drPivotField["Caption"].ToString();
                        }                        

                        if (drPivotField["FieldArea"] != DBNull.Value)
                        {                            
                            //pivotGridField.Area = (PivotArea)Enum.Parse(typeof(PivotArea), drPivotField["FieldArea"].ToString());
                        }   
                        
                        //pg_PivotReport.Fields.Add(pivotGridField);
                    }
                }

                // Binding DataSource
                if (dsPivotGridReport.Tables["Report"] != null)
                {
                    //pg_PivotReport.DataSource               = dsPivotGridReport.Tables["Report"];
                }                

                //pg_PivotReport.DataBind();
            }
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
                    this.Page_Retrieve();
                    this.Page_Setting();
                }
                else
                {
                    // Restore dataset from session
                    dsPivotGridReport           = (DataSet)Session["dsPivotGridReport"];

                    // Re-binding PivotGrid
                    //pg_PivotReport.DataSource   = dsPivotGridReport.Tables["Report"];
                    //pg_PivotReport.DataBind();
                }
            }
        }

        #endregion
    }
}