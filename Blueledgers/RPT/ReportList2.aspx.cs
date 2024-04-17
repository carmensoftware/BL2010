using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Web;

namespace BlueLedger.PL.Report
{
    public partial class ReportList2 : BasePage // System.Web.UI.Page
    {
        public static string connstring = "";
        public DataSet data;
        public SqlConnection conn;
        public SqlDataReader dr;
        public SqlDataAdapter da;
        public int max;    //Max Order report  Show all
        public int min;    //Min Order report  Show all
        public int maxcat; //Max Order report  Show By Cat
        public int mincat;  //Min Order report  Show By Cat
        public SqlCommand cmd;
        public static string cmbcat;
        public static Boolean showall; // S
        public static string searchs;

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                showcat();
                //string a = Guid.NewGuid().ToString();
                ASPxButton1.Enabled = true;
                ASPxCheckBox1.Checked = true;
                showall = true;
                ASPxComboBox1.Enabled = false;
                LoadReport("", "", showall);
                ASPxPanel1.Visible = false;

            }
            else
            {

                searchs = ASPxTextBox1.Text.ToString();
                cmbcat = ASPxComboBox1.Text.ToString();
                ASPxComboBox1.Enabled = !ASPxCheckBox1.Checked;
                showall = ASPxCheckBox1.Checked;
                LoadReport(cmbcat, searchs, showall);
                if (ASPxGridView1.VisibleRowCount <= 0)
                {
                    ASPxButton1.Enabled = false;
                }
                else
                {
                    ASPxButton1.Enabled = true;
                }

            }

        }

        private void connects()
        {
            try
            {
                conn = new SqlConnection(connstring);
                conn.Open();
            }
            catch (Exception)
            {
                conn.Close();
            }
        }

        public void closeconnect()
        {
            conn.Close();
        }

        public void ShowReport(string filtername, string search, bool showall)   // Load Data To Dataset
        {
            connects();
            string sql;
            if (showall == true)
            {
                filtername = "";
                sql = "SELECT rpt.RptID,rpt.RptName, rpt.RptDesc,rpt.RptOrder ";
                sql = sql + ", CASE  rpt.IsStandard  WHEN 0  THEN 'Custommize'  WHEN 1 THEN 'Standard' END AS [Report Type]  ";
                sql = sql + " FROM RPT.Report2 rpt";
                sql = sql + " LEFT JOIN RPT.Category cat ON cat.CategoryCode=rpt.CatCode";
                sql = sql + " WHERE (rpt.IsActive = 1 ) ";
                sql = sql + " AND ('" + search + "'='' OR  rpt.RptDesc Like '%'+'" + search + "'+'%'  or rpt.RptName like '%'+'" + search + "'+'%') ";
                sql = sql + " ORDER BY rpt.RptOrder";
            }
            else
            {
                sql = " SELECT rpt.RptID, rpt.RptName, rpt.RptDesc,rc.Catorder As RptOrder,rc.ID ";
                sql = sql + ",  CASE  rpt.IsStandard  WHEN 0  THEN 'Custommize'  WHEN 1 THEN 'Standard' END AS [Report Type] ";
                sql = sql + " FROM RPT.Report2 rpt";
                sql = sql + " LEFT JOIN RPT.reportCat rc ON rc.ReportID=rpt.RptID ";
                sql = sql + " LEFT JOIN RPT.Category cat ON cat.CategoryCode=rc.catid ";
                sql = sql + " WHERE (rpt.IsActive = 1 AND (cat.Name='" + filtername + "')) ";
                sql = sql + " AND ('" + search + "'='' OR  rpt.RptDesc Like '%'+'" + search + "'+'%' or rpt.RptName like '%'+'" + search + "'+'%') ";
                sql = sql + " ORDER BY rc.catorder";
            }
            var cmd = new SqlCommand(sql, conn);
            var adapter = new SqlDataAdapter(cmd);
            data = new DataSet();
            adapter.Fill(data, "Report2");
            closeconnect();

        }

        public void showcat() // Show Category On Dropdown List
        {
            DataSet ds;   //Combobox Cat type
            String sql = " SELECT Name FROM RPT.Category";
            connstring = LoginInfo.ConnStr;
            connects();
            cmd = new SqlCommand(sql, conn);
            da = new SqlDataAdapter(cmd);
            ds = new DataSet();
            da.Fill(ds);
            closeconnect();
            ASPxComboBox1.DataSource = ds;
            ASPxComboBox1.TextField = "Name";
            ASPxComboBox1.ValueField = "Name";
            ASPxComboBox1.DataBind();
        }





        private void LoadReport(string filtername, string search, bool showall)  //Config Aspxgridview
        {

            ShowReport(filtername, search, showall);
            // Set row select property to grid
            ASPxGridView1.KeyFieldName = "RptID";
            ASPxGridView1.Settings.ShowGroupPanel = true;
            ASPxGridView1.Settings.GridLines = GridLines.Horizontal;
            ASPxGridView1.Settings.ShowGroupPanel = false;
            ASPxGridView1.Settings.ShowGroupButtons = false;
            ASPxGridView1.SettingsBehavior.AllowFocusedRow = true;
            ASPxGridView1.SettingsBehavior.AllowGroup = false;
            ASPxGridView1.SettingsBehavior.AllowDragDrop = false;
            ASPxGridView1.SettingsBehavior.ColumnResizeMode = DevExpress.Web.ASPxGridView.ColumnResizeMode.NextColumn;
            // ASPxGridView1.SettingsPager.Mode = DevExpress.Web.ASPxGridView.GridViewPagerMode.ShowAllRecords;
            ASPxGridView1.SettingsPager.PageSize = 15;
            ASPxGridView1.SettingsPager.RenderMode = DevExpress.Web.ASPxClasses.ControlRenderMode.Lightweight;
            // Set data
            ASPxGridView1.DataSource = data;
            ASPxGridView1.DataBind();
            // Set properties to each column
            // <RptID>
            ASPxGridView1.Columns[0].Visible = false;
            if (!ASPxCheckBox1.Checked && filtername != "")
            {
                try
                {
                    //   ASPxGridView1.Columns[4].Visible = false; 
                }
                catch { }
            }

            if (ASPxCheckBox1.Checked == false && ASPxGridView1.VisibleRowCount > 1)
            {
                string category = ASPxComboBox1.Text.ToString();
                mindatacat(category);
                maxdatacat(category);
            }
            else
            {
                maxdata();
                mindata();
            }
            // <rpt.Name>
            ASPxGridView1.Columns[3].Visible = false;
            ASPxGridView1.Columns[1].Caption = "Name";
            // <rpt.Remark>
            ASPxGridView1.Columns[2].Caption = "Description";
            ASPxGridView1.SettingsBehavior.AllowSort = false;

        }

        protected void ASPxButton1_Click(object sender, EventArgs e) // preview Report
        {
            try
            {
                var RptID = ASPxGridView1.GetRowValues(ASPxGridView1.FocusedRowIndex, "RptID").ToString();
                //string script = "window.open('ReportViewer.aspx?id=" + RptID + "',";
                ////script = script + "'_blank','menubar=no,resizable=yes,status=no,titlebar=no,top=0px,left=50px,width=1024px,height=660px'); </script>";
                //script += "'_newtab','resizable,width=1024px,height=660px');";
                ////AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(this, this.GetType(), "", script, true);




                string url = "ReportViewer.aspx?id=" + RptID;
                string script = string.Format("window.open('{0}');", url);
                AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(this, this.GetType(), "newPage" + UniqueID, script, true);

  //              string url = HttpContext.Current.Server.MapPath("~") + @"\App_Files\Reports\";
  //              url += @"ProductList.aspx";

  //              url = HttpContext.Current.Request.Url.Scheme + "://"
  //+ HttpContext.Current.Request.Url.Authority
  //+ HttpContext.Current.Request.ApplicationPath
  //+ @"/App_Files/Reports/ProductList.aspx";

  //              string script = string.Format("window.open('{0}');", url);
  //              AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(this, this.GetType(), "newPage" + UniqueID, script, true);


            }
            catch
            {
                alertshow("No report selected.");
            }
        }


        protected void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {

        }

        protected void ASPxButton2_Click(object sender, EventArgs e)  // Move Up Button
        {
            int Rptorder = Convert.ToInt32(ASPxGridView1.GetRowValues(ASPxGridView1.FocusedRowIndex, "RptOrder"));

            if (min == Rptorder || mincat == Rptorder)
            {
                alertshow("Report At the top");
            }
            else
            {
                int up = 0;
                int down = -1;
                movereport(up, down);
            }
            ASPxGridView1.FocusedRowIndex = ASPxGridView1.FocusedRowIndex - 1;
        }


        protected void ASPxButton3_Click(object sender, EventArgs e) // Move Down Button
        {
            int Rptorder = Convert.ToInt32(ASPxGridView1.GetRowValues(ASPxGridView1.FocusedRowIndex, "RptOrder"));

            if (max == Rptorder || maxcat == Rptorder)
            {
                alertshow("Report At the Bottom");
            }
            else
            {
                int up = 1;
                int down = 0;
                movereport(up, down);
            }
            ASPxGridView1.FocusedRowIndex = ASPxGridView1.FocusedRowIndex + 1;
        }

        public void sortup(int trptid) //Sort Up Report
        {
            connects();
            string sql;
            sql = "Update rpt.report2";
            sql = sql + " Set rptorder=rptorder-1";
            sql = sql + " Where rptid='" + trptid + "'";
            cmd = new SqlCommand(sql, conn);
            cmd.CommandText = sql;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
        }

        public void sortupCat(int trptid, int catid) //Sort Up Report By Cat
        {
            connects();
            string sql;
            sql = "Update RPT.ReportCat";
            sql = sql + " Set Catorder=Catorder-1";
            sql = sql + " Where ReportID='" + trptid + "' And ID='" + catid + "' ";
            cmd = new SqlCommand(sql, conn);
            cmd.CommandText = sql;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
        }

        public void sortdown(int trptid)  //Sort Down
        {
            connects();
            string sql;
            sql = "Update rpt.report2";
            sql = sql + " Set rptorder=rptorder+1";
            sql = sql + " Where rptid='" + trptid + "'";
            cmd = new SqlCommand(sql, conn);
            cmd.CommandText = sql;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
        }

        public void sortDownCat(int trptid, int catid) //Sort Down Report By Cat
        {
            connects();
            string sql;
            sql = "Update RPT.ReportCat";
            sql = sql + " Set Catorder=Catorder+1";
            sql = sql + " Where ReportID='" + trptid + "' And ID='" + catid + "' ";
            cmd = new SqlCommand(sql, conn);
            cmd.CommandText = sql;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
        }


        public void maxdata()   // Find Max Order Report
        {
            connects();
            string sql;
            sql = "SELECT MAX(rpt.rptorder)";
            sql = sql + " FROM RPT.Report2 rpt";
            cmd = new SqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            dr.Read();
            max = Convert.ToInt32(dr.GetInt32(0));
        }

        public void maxdatacat(string catname) // Find Max Order Report By Cat
        {
            connects();
            string sql;
            sql = "  SELECT MAX(catorder)";
            sql = sql + "  FROM [RPT].ReportCat rc";
            sql = sql + "  LEFT JOIN RPT.Category cat ON cat.CategoryCode=rc.CatID ";
            sql = sql + "  WHERE  cat.Name='" + catname + "'";

            cmd = new SqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            dr.Read();
            try
            {
                maxcat = Convert.ToInt32(dr.GetInt32(0));
            }
            catch
            {
                maxcat = 1;
            }
        }

        public void mindata() // Find Min Order
        {
            connects();
            string sql;
            sql = "SELECT MIN(rpt.rptorder)";
            sql = sql + " FROM RPT.Report2 rpt";
            cmd = new SqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            dr.Read();
            min = Convert.ToInt32(dr.GetInt32(0));
        }

        public void mindatacat(string catname) // Find Min Order Report By Cat
        {
            connects();
            string sql;
            sql = "  SELECT MIN(catorder)";
            sql = sql + "  FROM [RPT].ReportCat rc";
            sql = sql + "  LEFT JOIN RPT.Category cat ON cat.CategoryCode=rc.CatID ";
            sql = sql + "  WHERE  cat.Name='" + catname + "'";
            cmd = new SqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            dr.Read();
            try
            {
                mincat = Convert.ToInt32(dr.GetInt32(0));
            }
            catch
            {
                mincat = 1;
            }
        }

        private void alertshow(string alerttext)
        {
            string scriptText = @"alert('" + alerttext + "');";
            AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(this, this.GetType(), "", scriptText, true);
            if (alerttext == "Report At the top") ASPxGridView1.FocusedRowIndex = 1;
        }

        private void movereport(int UP, int DOWN)
        {
            int RptIDUP = Convert.ToInt32(ASPxGridView1.GetRowValues(ASPxGridView1.FocusedRowIndex + UP, "RptID"));
            int RptIDDOWN = Convert.ToInt32(ASPxGridView1.GetRowValues(ASPxGridView1.FocusedRowIndex + DOWN, "RptID"));
            int Rptorder = Convert.ToInt32(ASPxGridView1.GetRowValues(ASPxGridView1.FocusedRowIndex, "RptOrder"));
            connects();
            if (ASPxCheckBox1.Checked)   // check show all false
            {
                sortup(RptIDUP);
                sortdown(RptIDDOWN);
                LoadReport("", "", showall);
            }
            else
            {
                int IDUP = Convert.ToInt32(ASPxGridView1.GetRowValues(ASPxGridView1.FocusedRowIndex + UP, "ID"));
                int IDDOWN = Convert.ToInt32(ASPxGridView1.GetRowValues(ASPxGridView1.FocusedRowIndex + DOWN, "ID"));
                sortDownCat(RptIDDOWN, IDDOWN);
                sortupCat(RptIDUP, IDUP);
                LoadReport(ASPxComboBox1.Text.ToString(), "", showall);
            }
            closeconnect();
        }

        #region "Footer Bar"

        protected void btnAll_Click(object sender, EventArgs e)
        {
            btnAllClick();
        }

        protected void btnAllClick()
        {
            FilterReportType("All");
            btnStandard.Attributes["Style"] = StyleViewBorder(0);
            btnCustomize.Attributes["Style"] = StyleViewBorder(0);
            btnAll.Attributes.Add("Style", StyleViewBorder(1));
        }

        protected void btnStandard_Click(object sender, EventArgs e)
        {
            FilterReportType("Standard");
            btnAll.Attributes["Style"] = StyleViewBorder(0);
            btnCustomize.Attributes["Style"] = StyleViewBorder(0);
            btnStandard.Attributes.Add("Style", StyleViewBorder(1));
        }

        protected void btnCustomize_Click(object sender, EventArgs e)
        {
            FilterReportType("Customize");
            btnAll.Attributes["Style"] = StyleViewBorder(0);
            btnStandard.Attributes["Style"] = StyleViewBorder(0);
            btnCustomize.Attributes.Add("Style", StyleViewBorder(1));
        }

        protected string StyleViewBorder(int status)
        {
            string css = string.Empty;
            if (status == 1)
                css = "border: 1.5px solid #20B9EB; padding-left: 10px; padding-right: 10px;";
            return css;
        }

        protected void FilterReportType(string type)
        {
            string filter = string.Empty;
            switch (type.ToUpper())
            {
                case "STANDARD":
                    filter = string.Format("[Report Type] = '{0}'", "Standard");
                    break;

                case "CUSTOMIZE":
                    filter = string.Format("[Report Type] = '{0}'", "Custommize");
                    break;
            }

            data.Tables["Report2"].DefaultView.RowFilter = filter;

            //On above that set PageSize = 15. So, never mind about ddlRowPerPage.Items[0].
            ASPxGridView1.SettingsPager.PageSize = Convert.ToInt32(ddlRowPerPage.SelectedValue);
            ASPxGridView1.DataSource = data.Tables["Report2"];
            ASPxGridView1.DataBind();
        }

        protected void ddlRowPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAllClick();
        }

        #endregion

    }
}