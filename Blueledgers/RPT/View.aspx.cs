using System;
using BlueLedger.PL.BaseClass;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace BlueLedger.PL.Report
{
    public partial class View : BasePage
    {
        const string rptPath = @"~\App_Files\Reports\";

        // Properties
        private string _ID
        {
            get { return Request.QueryString["id"].ToString(); }
        }


        private Dictionary<string, string> _PARAM
        {
            get
            {
                var id = Request.QueryString["session"].ToString();
                return Session[id] == null ? new Dictionary<string, string>() : Session[id] as Dictionary<string, string>;
            }
        }


        // Event(s)
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Setting();
            }

        }

        protected void WebReport1_StartReport(object sender, EventArgs e)
        {
            for (var i = 0; i < WebReport1.Report.Dictionary.Connections.Count; i++)
                WebReport1.Report.Dictionary.Connections[0].ConnectionString = LoginInfo.ConnStr;

        }


        // Private method(s)
        private void Page_Setting()
        {
            var sql = "SELECT * FROM RPT.Report WHERE Id=" + _ID;

            var dt = ExecuteQuery(sql, null, LoginInfo.ConnStr);

            if (dt != null && dt.Rows.Count > 0)
            {
                var name = dt.Rows[0]["Name"].ToString();
                var filename = dt.Rows[0]["FileName"].ToString();

                this.Title = name;
                lbl_Title.Text = string.Format("{0} @{1}", name, LoginInfo.BuInfo.BuName);

                var rptFilename = rptPath + filename;

                
                
                
                WebReport1.ReportFile = rptFilename;
                var p = _PARAM;

                if (p != null)
                {
                    foreach (var item in p)
                    {
                        WebReport1.Report.SetParameterValue(item.Key, item.Value);
                    }
                }


            }
        }

        private DataTable ExecuteQuery(string sql, SqlParameter[] parameters, string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var da = new SqlDataAdapter(sql, conn);
                if (parameters != null)
                {
                    foreach (var p in parameters)
                    {
                        da.SelectCommand.Parameters.Add(p);
                    }
                }

                var dt = new DataTable();

                da.Fill(dt);

                return dt;
            }
        }

    }
}