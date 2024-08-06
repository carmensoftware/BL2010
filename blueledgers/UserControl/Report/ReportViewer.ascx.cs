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

//using DevExpress.XtraReports.UI;

namespace BlueLedger.PL.UserControls.Report
{
    public partial class ReportViewer : BaseUserControl
    {
        #region "Attributies"

        private DataSet dsDataReport = new DataSet();
        private DataTable dtReport = new DataTable();
        private Blue.BL.Report.ReportData reportData = new Blue.BL.Report.ReportData();

        private int ReportID;
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
        /// Retrieve needed data.
        /// </summary>
        private void Page_Retrieve()
        {
            Blue.BL.GnxLib.ReportCategory category = (Request.Params["category"] == null ? Blue.BL.GnxLib.ReportCategory.Consolidate : (Blue.BL.GnxLib.ReportCategory)Enum.Parse(typeof(Blue.BL.GnxLib.ReportCategory), Request.Params["category"].ToString()));
            ReportID = int.Parse(Request.Params["reportid"].ToString());

            // ReportData
            if (category == Blue.BL.GnxLib.ReportCategory.Consolidate)
            {
                reportData.Get(dsDataReport, ReportID, string.Empty);
            }
            else
            {
                reportData.Get(dsDataReport, ReportID, LoginInfo.ConnStr);
            }

            Session["dsDataReport"] = dsDataReport;
        }

        /// <summary>
        /// Binding controls.
        /// </summary>
        private void Page_Setting()
        {
            /* XtraReport report = new XtraReport();

            // Binding report
            if (dsDataReport.Tables[reportData.TableName] != null)
            {
                if (dsDataReport.Tables[reportData.TableName].Rows.Count > 0)
                {
                    if (dsDataReport.Tables[reportData.TableName].Rows[0]["ReportDir"] != DBNull.Value)
                    {
                        String ReportDir = dsDataReport.Tables[reportData.TableName].Rows[0]["ReportDir"].ToString();
                        report.LoadLayout(ReportDir);
                        // report.ShowPreview();
                    }


                }
            }

            // Bindging DataSource
            dtReport = (DataTable)Session["dtReport"];



            if (dtReport != null)
            {
                report.DataSource = dtReport;
            }

            // Bingding ReportViewer
            rv_DataReport.Report = report;
            rv_DataReport.DataBind();
             */

        }

        /// <summary>
        /// Page load event
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
                    dsDataReport = (DataSet)Session["dsDataReport"];
                }
            }
        }

        #endregion
    }
}