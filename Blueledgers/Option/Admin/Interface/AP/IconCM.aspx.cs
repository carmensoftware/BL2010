using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Linq;

namespace BlueLedger.PL.Option.Admin.Interface.AP
{
    public partial class IconCM : BlueLedger.PL.BaseClass.BasePage
    {
        private DataSet ds
        {
            get
            {
                return ViewState["ds"] == null ? new DataSet() : ViewState["ds"] as DataSet;
            }
            set
            {
                ViewState["ds"] = value;
            }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
            }

        }

        // Events
        protected void btn_Preview_Click(object sender, EventArgs e)
        {
            ds = QueryData();

            btn_ExportHeader.Visible = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;
            btn_ExportDetail.Visible = ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0;

            BindDataGrid(ds);

        }

        protected void btn_ExportHeader_Click(object sender, EventArgs e)
        {
            // Header - HDYYYYMMDD   [HD20200510.CSV]
            var fileName = string.Format("HD{0}.csv", date_DateFrom.Date.ToString("yyyyMMdd"));

            Export(ds.Tables[0], fileName);
            BindDataGrid(ds);
        }

        protected void btn_ExportDetail_Click(object sender, EventArgs e)
        {
            // Detail - DTYYYYMMDD   [DT20200510.CSV]
            var fileName = string.Format("DT{0}.csv", date_DateFrom.Date.ToString("yyyyMMdd"));

            Export(ds.Tables[1], fileName);
            BindDataGrid(ds);
        }

        // Private method(s)

        private void BindDataGrid(DataSet ds)
        {
            if (ds.Tables.Count > 0)
            {
                gvHeader.DataSource = ds.Tables[0];
                gvHeader.DataBind();
            }

            if (ds.Tables.Count > 1)
            {
                gvDetail.DataSource = ds.Tables[1];
                gvDetail.DataBind();
            }
        }


        private DataSet QueryData()
        {
            var dateFrom = date_DateFrom.Date;


            var sql = "EXEC Tool.ExportToAP_IconCM @FrDate";
            var parameters = new SqlParameter[1];

            parameters[0] = new SqlParameter("@FrDate", dateFrom.ToString("yyyy-MM-dd"));

            return ExectueQuery(sql, parameters);
        }


        private void Export(DataTable dt, string fileName, string delimiter = ";")
        {
            var sb = new StringBuilder();

            sb.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                string[] values = dt.Columns.Cast<DataColumn>()
                             .Select(x => dr[x.ColumnName].ToString())
                             .ToArray();

                sb.AppendLine(string.Join(delimiter, values));
            }

            FileDownload(fileName, sb.ToString());
        }

        private void FileDownload(string fileName, string text)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = "";
            Response.ContentType = "application/text";
            Response.Output.Write(text);
            Response.Flush();
            Response.End();
        }

        private DataSet ExectueQuery(string sql, SqlParameter[] parameters = null, string connStr = null)
        {
            try
            {
                using (var conn = new SqlConnection(LoginInfo.ConnStr))
                {
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        if (parameters != null)
                        {
                            foreach (var p in parameters)
                                cmd.Parameters.Add(p);
                        }


                        var adapter = new SqlDataAdapter();
                        adapter.SelectCommand = cmd;
                        conn.Open();
                        var ds = new DataSet();
                        adapter.Fill(ds);
                        conn.Close();
                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}

