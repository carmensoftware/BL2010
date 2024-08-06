using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.UserControls
{
    public partial class ARRptProc : BaseUserControl
    {
        #region "Attributies"

        private readonly Blue.BL.AR.DebtorReport debtorReport = new Blue.BL.AR.DebtorReport();
        private readonly Blue.BL.AR.DebtorTool debtorTool = new Blue.BL.AR.DebtorTool();
        private readonly DataSet dsAR = new DataSet();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Page load
        /// </summary>
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
            // Get vendor report link
            debtorReport.GetDebtorReportList(dsAR, LoginInfo.ConnStr);
            debtorTool.GetDebtorToolList(dsAR, LoginInfo.ConnStr);

            Session["dsAR"] = dsAR;

            //DataTable dtTmp2 = new DataTable();
            //for (int i = 0; i < 10; i++)
            //{
            //    dtTmp2.Rows.Add();
            //}

            //grd_ARReport.DataSource = dtTmp2;
            //grd_ARReport.DataBind();
        }

        /// <summary>
        ///     Set data to grid, lable
        /// </summary>
        private void Page_Setting()
        {
            grd_ARReport.DataSource = dsAR.Tables[debtorReport.TableName];
            grd_ARReport.DataBind();

            grd_ARProc.DataSource = dsAR.Tables[debtorTool.TableName];
            grd_ARProc.DataBind();
        }

        /// <summary>
        ///     Set HyperLink
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ARReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // DataRow
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lnk_Report") != null)
                {
                    var lnk_Report = (HyperLink) e.Row.FindControl("lnk_Report");
                    lnk_Report.Text = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                    lnk_Report.NavigateUrl = "~/Report/ReportCriteria.aspx?reportid=" +
                                             DataBinder.Eval(e.Row.DataItem, "ReportID") +
                                             "&category=GeneralLedger";
                    lnk_Report.Target = "_new";
                }
                //if (e.Row.FindControl("lnk_Report") != null && e.Row.RowIndex == 0)
                //{
                //    HyperLink lnk_Report    = (HyperLink)e.Row.FindControl("lnk_Report");
                //    lnk_Report.Text         = "Ageing";
                //    lnk_Report.NavigateUrl  = "";//DataBinder.Eval(e.Row.DataItem, "URL").ToString();
                //    lnk_Report.Target       = "_new";
                //}
                //if (e.Row.FindControl("lnk_Report") != null && e.Row.RowIndex == 1)
                //{
                //    HyperLink lnk_Report = (HyperLink)e.Row.FindControl("lnk_Report");
                //    lnk_Report.Text      = "AgingNew";
                //}
                //if (e.Row.FindControl("lnk_Report") != null && e.Row.RowIndex == 2)
                //{
                //    HyperLink lnk_Report = (HyperLink)e.Row.FindControl("lnk_Report");
                //    lnk_Report.Text      = "AR Withholding Tax Report";
                //}
                //if (e.Row.FindControl("lnk_Report") != null && e.Row.RowIndex == 3)
                //{
                //    HyperLink lnk_Report = (HyperLink)e.Row.FindControl("lnk_Report");
                //    lnk_Report.Text      = "Contract Listing";
                //}
                //if (e.Row.FindControl("lnk_Report") != null && e.Row.RowIndex == 4)
                //{
                //    HyperLink lnk_Report = (HyperLink)e.Row.FindControl("lnk_Report");
                //    lnk_Report.Text      = "Contract with AR Profile (Contract Detail)";
                //}
                //if (e.Row.FindControl("lnk_Report") != null && e.Row.RowIndex == 5)
                //{
                //    HyperLink lnk_Report = (HyperLink)e.Row.FindControl("lnk_Report");
                //    lnk_Report.Text      = "Empth Contract Profile";
                //}
                //if (e.Row.FindControl("lnk_Report") != null && e.Row.RowIndex == 6)
                //{
                //    HyperLink lnk_Report = (HyperLink)e.Row.FindControl("lnk_Report");
                //    lnk_Report.Text      = "Unpaid Invoice With Workflow";
                //}
                //if (e.Row.FindControl("lnk_Report") != null && e.Row.RowIndex == 7)
                //{
                //    HyperLink lnk_Report = (HyperLink)e.Row.FindControl("lnk_Report");
                //    lnk_Report.Text      = "A/R Profile Contract Detail";
                //}
                //if (e.Row.FindControl("lnk_Report") != null && e.Row.RowIndex == 8)
                //{
                //    HyperLink lnk_Report = (HyperLink)e.Row.FindControl("lnk_Report");
                //    lnk_Report.Text      = "A/R Profile Envelope";
                //}
                //if (e.Row.FindControl("lnk_Report") != null && e.Row.RowIndex == 9)
                //{
                //    HyperLink lnk_Report = (HyperLink)e.Row.FindControl("lnk_Report");
                //    lnk_Report.Text      = "A/R Profile Transaction";
                //}
            }
        }

        /// <summary>
        ///     Set HyperLink
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ARProc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lnk_Proc") != null)
                {
                    var lnk_Proc = (HyperLink) e.Row.FindControl("lnk_Proc");
                    lnk_Proc.Text = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                    lnk_Proc.NavigateUrl = DataBinder.Eval(e.Row.DataItem, "URL").ToString();
                    lnk_Proc.Target = "_new";
                }
            }
        }

        #endregion
    }
}