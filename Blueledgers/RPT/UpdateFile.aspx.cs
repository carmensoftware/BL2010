using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace BlueLedger.PL.Report
{
    public partial class UpdateFile : BasePage
    {
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_UpdateFile_Click(object sender, EventArgs e)
        {
            // DROP Existing table
            bu.DbExecuteQuery("DROP TABLE RPT.ReportFile", null, LoginInfo.ConnStr);

            // CREATE table
            bu.DbExecuteQuery("CREATE TABLE RPT.ReportFile ( RptFileName nvarchar(200) NOT NULL, Data nvarchar(max) NULL, PRIMARY KEY (RptFileName))", null, LoginInfo.ConnStr);

            // Insert Report File Name
            string sql = @"INSERT INTO RPT.ReportFile(RptFileName)
                    SELECT DISTINCT RptFileName
                    FROM RPT.Report2
                    ORDER BY RptFileName";
            bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            // Update Report file data

            DataTable dt = bu.DbExecuteQuery("SELECT RptFileName FROM RPT.ReportFile", null, LoginInfo.ConnStr);

            int i = 1;
            lbl_Message.Text = string.Format("&ensp; {0} <br />", DateTime.Now.ToString("u"));

            foreach (DataRow dr in dt.Rows)
            {
                string name = dr["RptFileName"].ToString();

                string rptFile = Server.MapPath(string.Format(@"~\App_Files\Reports\{0}", name));

                if (File.Exists(rptFile))
                {
                    StreamReader sr = File.OpenText(rptFile);
                    string text = sr.ReadToEnd();
                    sr.Close();

                    using (SqlConnection conn = new SqlConnection(LoginInfo.ConnStr))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("UPDATE RPT.ReportFile SET Data = @data WHERE RptFileName = @rptfilename", conn))
                        {
                            cmd.Parameters.AddWithValue("@data", text);
                            cmd.Parameters.AddWithValue("@rptfilename", name);
                            
                            cmd.ExecuteNonQuery();
                        }
                    }

                    lbl_Message.Text += string.Format(" <br/> {0:D}. {1}", i++, name);

                }
            }

            lbl_Message.Text += " <br/> Done." ;





        }
    }
}