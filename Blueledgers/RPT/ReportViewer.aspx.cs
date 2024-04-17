using System;
using System.Data.SqlClient;
using BlueLedger.PL.BaseClass;
using System.Xml;

namespace BlueLedger.PL.Report
{
    public partial class ReportViewer : BasePage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            var rptID = Request.QueryString["id"];
            string rptName, rptCategory, rptFileName, rptPath;

            var conn = new SqlConnection(LoginInfo.ConnStr);

            string sql;
            sql = "SELECT rpt.Rptname, rpt.RptDesc, rpt.RptFileName, cat.Name as CatName";
            sql = sql + " FROM RPT.Report2 rpt";
            sql = sql + " LEFT JOIN RPT.Category cat ON cat.CategoryCode = rpt.CatCode";
            sql = sql + " WHERE rpt.RptID = " + rptID;


            var cmd = new SqlCommand(sql, conn);
            conn.Open();

            var reader = cmd.ExecuteReader();
            reader.Read();

            rptName = reader["RptName"].ToString();
            rptCategory = reader["CatName"].ToString();
            rptFileName = reader["RptFileName"].ToString();
            rptPath = @"~\App_Files\Reports\";

            reader.Close();
            conn.Close();

            Title = rptName + " <@" + conn.Database + ">";
            WebReport1.ReportFile = rptPath + rptFileName;

            // Set Page size dynamic with Report Paper Size
            XmlDocument xDoc = new XmlDocument();
            XmlNodeList report;
            xDoc.Load(Server.MapPath(rptPath + rptFileName));  //Load frx File
            report = xDoc.GetElementsByTagName("ReportPage"); //Get Tagname (ReportPage)

            double paperWidth = 0;
            double paperHeigth = 0;
            try
            {
                double convRate = 3.77;    // coefficient mm*3.77 =Pixel

                for (int i = 0; i < report.Count; i++)
                {
                    paperWidth = Convert.ToDouble(report[i].Attributes["PaperWidth"].Value) * convRate;  //PaperWidth
                    paperHeigth = Convert.ToDouble(report[i].Attributes["PaperHeight"].Value) * convRate; //PaperHeight

                }
            }
            catch     //If Not Find
            {
                paperWidth = 980;  //PaperWidth
                paperHeigth = 620;  //PaperHeight
            }

            WebReport1.Width = Convert.ToInt16(paperWidth);
            WebReport1.Height =  Convert.ToInt16(paperHeigth);

            //WebReport1.AutoHeight = true;
            WebReport1.AutoWidth = true;
        }

        protected void WebReport1_StartReport(object sender, EventArgs e)
        {
            for (var i = 0; i < WebReport1.Report.Dictionary.Connections.Count; i++)
                WebReport1.Report.Dictionary.Connections[0].ConnectionString = LoginInfo.ConnStr;
        }
    }
}