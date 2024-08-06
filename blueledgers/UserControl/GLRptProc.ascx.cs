using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.UserControls
{
    public partial class GLRptProc : BaseUserControl
    {
        #region "Attributies"

        private readonly DataSet dsProc = new DataSet();
        private readonly DataSet dsReport = new DataSet();

        private readonly Blue.BL.GL.GLProc proc = new Blue.BL.GL.GLProc();
        private readonly Blue.BL.Report.Report report = new Blue.BL.Report.Report();

        #endregion

        /// <summary>
        ///     Page load
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            Page_Retrieve();
            Page_Setting();
        }

        /// <summary>
        ///     Retrieve Data
        /// </summary>
        private void Page_Retrieve()
        {
            var category = Blue.BL.GnxLib.ReportCategory.GeneralLedger;
            report.GetList(dsReport, ((int) category).ToString(), LoginInfo.ConnStr);
            Session["dsReport"] = dsReport;

            proc.GetList(dsProc, LoginInfo.ConnStr);
            Session["dsProc"] = dsProc;
        }

        /// <summary>
        ///     Set data to grid, lable
        /// </summary>
        private void Page_Setting()
        {
            //grd_Proc.DataSource = dsProc.Tables[proc.TableName];
            //grd_Proc.DataBind();

            grd_Report.DataSource = dsReport.Tables[report.TableName];
            grd_Report.DataBind();
        }

        protected void grd_Proc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lnk_Proc") != null)
                {
                    var lnk_Proc = (HyperLink) e.Row.FindControl("lnk_Proc");
                    lnk_Proc.Text = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                    lnk_Proc.NavigateUrl = DataBinder.Eval(e.Row.DataItem, "URL").ToString();
                    //lnk_Proc.Target = "_new";
                }
            }
        }

        protected void grd_Report_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lnk_Report") != null)
                {
                    var lnk_Report = (HyperLink) e.Row.FindControl("lnk_Report");
                    lnk_Report.Text = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                    lnk_Report.NavigateUrl = "~/RPT/ReportCriteria.aspx?reportid=" +
                                             DataBinder.Eval(e.Row.DataItem, "ReportID") +
                                             "&category=GeneralLedger";
                    //lnk_Report.Target = "_new";
                }
            }
        }
    }
}