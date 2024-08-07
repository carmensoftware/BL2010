using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Web.UI.HtmlControls;
using Newtonsoft.Json.Linq;
using System.Text;

public partial class Report : System.Web.UI.Page
{
    protected LoginInformation LoginInfo;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoginInfo"] == null)
        {
            Session["PreviousPage"] = HttpContext.Current.Request.Url.AbsoluteUri;
            Response.Redirect("~/ErrorPages/SessionTimeOut.aspx");
        }

        LoginInfo = (LoginInformation)Session["LoginInfo"];

        if (!IsPostBack)
        {
            Page_Setup();
        }
    }

    private void Page_Setup()
    {
        this.Title = string.Format("Report-{0}", LoginInfo.BuInfo.BuName);
        var id = Request.QueryString["id"];

        if (id != null)
        {
            var report = GetReport(LoginInfo.ConnStr, id.ToString());

            lbl_Title.Text = report.Name;
            lbl_Bu.Text = "at " + LoginInfo.BuInfo.BuName;

            if (string.IsNullOrEmpty(report.Dialog))
            {
                LoadReport_Old(report);
            }
            else
            {
                WebReport1.Visible = false;
                SetDialog(panel_Dialog, report.Dialog);
            }

        }

    }

    // Event(s)
    protected void WebReport1_StartReport(object sender, EventArgs e)
    {
        for (var i = 0; i < WebReport1.Report.Dictionary.Connections.Count; i++)
            WebReport1.Report.Dictionary.Connections[0].ConnectionString = LoginInfo.ConnStr;
    }


    // Method(s)
    private DataTable ExecuteQuery(string connectionString, string query, IEnumerable<SqlParameter> parameters = null)
    {
        try
        {

            using (var conn = new SqlConnection(connectionString))
            {
                using (var da = new SqlDataAdapter(query, conn))
                {
                    if (parameters != null)
                    {
                        foreach (var p in parameters)
                        {
                            da.SelectCommand.Parameters.AddWithValue(p.ParameterName, p.Value);
                        }
                    }

                    var dt = new DataTable();

                    da.Fill(dt);

                    return dt;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private ReportItem GetReport(string connectionString, string id)
    {

        var query = "SELECT * FROM RPT.Report WHERE Id=@Id";
        var dt = ExecuteQuery(connectionString, query, new SqlParameter[] { new SqlParameter("Id", id) });

        if (dt != null & dt.Rows.Count > 0)
        {
            var item = dt.AsEnumerable()
                .Select(x => new ReportItem
                {
                    Id = x.Field<string>("Id"),
                    Name = x.Field<string>("Name"),
                    Description = x.Field<string>("Description"),
                    Dialog = x.Field<string>("Dialog"),
                    Report = x.Field<string>("Report"),

                })
                .FirstOrDefault();

            return item;
        }
        else
            return null;
    }


    // Set dialog
    private void SetDialog(Panel panel, string json)
    {
        var connStr = LoginInfo.ConnStr;
        var test = new StringBuilder();
        var report = JObject.Parse(json);

        // Load data tables
        var dataList = JArray.Parse(report["data"].ToString());
        var ds = new DataSet();

        foreach (var data in dataList)
        {
            var dt = new DataTable();
            var tableName = data["name"].Value<string>();
            var query = string.Format("EXEC {0}", data["exec"]);

            dt = ExecuteQuery(connStr, query);
            dt.TableName = tableName.ToLower();

            ds.Tables.Add(dt);

        }

        // Create Controls
        var dialogList = JArray.Parse(report["dialog"].ToString());

        foreach (var item in dialogList)
        {
            var type = item["type"].Value<string>();

            switch (type.ToLower())
            {
                case "date":
                    break;
                case "select":
                    var select = new Dialog_Select();

                    select.Name = GetValue(item, "name");
                    select.Text = GetValue(item, "text");
                    select.Data = GetValue(item, "data");
                    select.FieldText = GetValue(item, "fieldText");
                    select.FieldValue = GetValue(item, "fieldValue");
                    select.Value = GetValue(item, "value");



                    panel.Controls.Add(Create_Select(select, ds));

                    break;
                case "checkbox":
                    break;
            }
        }



        //foreach (DataTable dt in ds.Tables)
        //{
        //    test.AppendLine(dt.TableName);
        //}

        //lbl_test.Text = test.ToString();


        //var item1 = new Dialog_Select
        //{
        //    Name = "test",
        //    Text = "Product",
        //    Options = new Dialog_Select_Option[]
        //    {
        //        new Dialog_Select_Option{ Text="Procurement", Value="PC" },
        //        new Dialog_Select_Option{ Text="Inventory", Value="IN" },
        //        new Dialog_Select_Option{ Text="Portion", Value="PT" },
        //    }

        //};


        //var select1 = Create_Select(item1, ds);

        //var item2 = new Dialog_Select
        //{
        //    Name = "test2",
        //    Text = "To",
        //    Data="Product",
        //    FieldText = "Text",
        //    FieldValue = "Value",
        //};


        //var select2 = Create_Select(item2, ds);

        //panel.Controls.Add(select1);
        //panel.Controls.Add(select2);
    }

    private string GetValue(JToken token, string propertyName)
    {
        JToken item;

        if (((JObject) token).TryGetValue(propertyName, out item))
        {
            return item.Value<string>();
        }
        else
            return null;






    }

    private HtmlGenericControl Create_Select(Dialog_Select item, DataSet ds = null)
    {
        var div = new HtmlGenericControl();
        div.TagName = "div";
        div.Attributes["class"] = "dialog-div";

        var label = new HtmlGenericControl();
        label.TagName = "label";
        label.InnerText = item.Text;
        label.Attributes.Add("class", "dialog-label");

        var select = new HtmlGenericControl();
        select.TagName = "select";
        select.Attributes["name"] = item.Name;
        select.Attributes["class"] = "dialog-select";

        if (string.IsNullOrEmpty(item.Data))
        {
            foreach (var option in item.Options)
            {
                var o = new HtmlGenericControl();
                o.TagName = "option";
                o.Attributes["value"] = option.Value;
                o.InnerText = option.Text;

                select.Controls.Add(o);
            }
        }
        else
        {
            var tableName = item.Data.ToLower();
            var dt = ds.Tables[tableName];

            for (int i = 0; i < dt.Rows.Count; i++ )
            {
                DataRow dr = dt.Rows[i];
                var value = dr[item.FieldValue].ToString();
                var text = dr[item.FieldText].ToString();

                var o = new HtmlGenericControl();
                o.TagName = "option";
                o.Attributes["value"] = value;
                o.InnerText = text;

                if (value == "1FB03")
                {
                    o.Attributes["selected"] = "true";
                }

                select.Controls.Add(o);
            }

        }


        div.Controls.Add(label);
        div.Controls.Add(select);


        return div;
    }




    private void LoadReport_Old(ReportItem item)
    {
        var path = @"~\App_Files\Reports\";
        var filename = path + item.Report;

        WebReport1.ReportFile = filename;

        // Set Page size dynamic with Report Paper Size
        XmlDocument xDoc = new XmlDocument();
        XmlNodeList report;
        xDoc.Load(Server.MapPath(filename));  //Load frx File
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
        WebReport1.Height = Convert.ToInt16(paperHeigth);

        WebReport1.AutoHeight = true;
        WebReport1.AutoWidth = true;


    }

    internal class ReportItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Dialog { get; set; }
        public string Report { get; set; }
        //public bool IsStandard { get; set; }
        //public bool IsActive { get; set; }

    }


    public enum DialogType
    {
        date,
        select,
        checkbox
    }

    public class DialogBase
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public DialogType Type { get; set; }
        public string Value { get; set; }
    }




    public class Dialog_Select : DialogBase
    {
        public string Data { get; set; }
        public string FieldValue { get; set; }
        public string FieldText { get; set; }
        public IEnumerable<Dialog_Select_Option> Options { get; set; }
    }

    public class Dialog_Select_Option
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}
