using System;
using System.Data.SqlClient;
using FastReport;
using BlueLedger.PL.BaseClass;

public partial class RPT_PrintForm : BasePage
{
    FastReport.Report report = new FastReport.Report();
    string id = string.Empty;


    protected override void Page_Load(object sender, EventArgs e)
    {
        id = Request.QueryString["ID"] == null ? "" : Request.QueryString["ID"].ToString();

        string reportName = Request.QueryString["Report"];
        string rptID = string.Empty;

        switch (reportName)
        {
            case "PurchaseRequestForm":
                rptID = "9001";
                break;
            case "PurchaseOrderForm":
                rptID = "9002";
                break;
            case "StoreRequisitionForm":
                rptID = "9011";
                break;
            default:
                reportName = reportName + ".frx";
                break;
        }

        if (rptID != string.Empty)
        {
            string sql = string.Format("SELECT TOP(1) RptFileName FROM RPT.Report2 WHERE RptID = '{0}'", rptID);

            SqlConnection conn = new SqlConnection(LoginInfo.ConnStr);
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Connection.Open();
            object rptFileName = cmd.ExecuteScalar();
            conn.Close();

            if (rptFileName == null)
                reportName = reportName + ".frx";
            else
                reportName = rptFileName.ToString();
        }


        report.StartReport += new EventHandler(report_StartReport);
        
        //var items = id.Split(',');

        //foreach (var item in items)
        //{
        //    report.Load(Server.MapPath("~/App_Files/Reports").TrimEnd('/') + "/" + reportName);
        //    report.SetParameterValue("ID", item);
        //    report.Prepare(true);
        //}

        if (id.Length > 30)
        {
            var list = Session[id].ToString();
            var items = list.Split(',');

            foreach (var item in items)
            {
                report.Load(Server.MapPath("~/App_Files/Reports").TrimEnd('/') + "/" + reportName);
                report.SetParameterValue("ID", item);
                report.Prepare(true);
            }

            //Session.Remove(id);

        }
        else
        {
            var items = id.Split(',');

            foreach (var item in items)
            {
                report.Load(Server.MapPath("~/App_Files/Reports").TrimEnd('/') + "/" + reportName);
                report.SetParameterValue("ID", item);
                report.Prepare(true);
            }
        }


        WebReport1.Width = 1200;
        WebReport1.Height = 740;
    }

    protected void report_StartReport(object sender, EventArgs e)
    {
        var rpt = sender as FastReport.Report;

        for (var i = 0; i < rpt.Dictionary.Connections.Count; i++)
            rpt.Dictionary.Connections[0].ConnectionString = LoginInfo.ConnStr;

    }

    protected void WebReport1_Load(object sender, EventArgs e)
    {
        WebReport1.Report = report;
        WebReport1.ReportDone = true;

        WebReport1.Width = 1140;
        WebReport1.Height = new System.Web.UI.WebControls.Unit(WebReport1.Report.Pages[0].Height);
    }
}