using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.UserControls
{
    public partial class APReport : BaseUserControl
    {
        #region "Attributies"

        private readonly DataSet dsReport = new DataSet();
        private readonly Blue.BL.AP.VendorReport report = new Blue.BL.AP.VendorReport();

        #endregion

        #region "Operations"

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Page_Retrieve();
                Page_Setting();
            }
        }

        /// <summary>
        ///     Retrieve Data
        /// </summary>
        private void Page_Retrieve()
        {
            var category = Blue.BL.GnxLib.ReportCategory.AccountPayable;
            report.GetVendorReportList(dsReport, LoginInfo.ConnStr);
            Session["dsReport"] = dsReport;
        }

        /// <summary>
        ///     Set data to grid, lable
        /// </summary>
        private void Page_Setting()
        {
            grd_APReport.DataSource = dsReport.Tables[report.TableName];
            grd_APReport.DataBind();
        }

        /// <summary>
        ///     Set HyperLink
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_APReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lnk_Report") != null)
                {
                    var lnk_Report = (HyperLink) e.Row.FindControl("lnk_Report");
                    lnk_Report.Text = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                    lnk_Report.NavigateUrl = "~/Report/ReportCriteria.aspx?reportid=" +
                                             DataBinder.Eval(e.Row.DataItem, "ReportID") +
                                             "&category=AccountPayable";
                    lnk_Report.Target = "_new";
                }
            }
        }

        #endregion
    }
}