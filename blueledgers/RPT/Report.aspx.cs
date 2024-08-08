using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Globalization;

public partial class Report : System.Web.UI.Page
{
    protected LoginInformation LoginInfo;

    protected override void InitializeCulture()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("th") { DateTimeFormat = { Calendar = new GregorianCalendar() } };

        //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("th-TH");

        base.InitializeCulture();
    }

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

    #region -- Dialog

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

        foreach (var dialog in dialogList)
        {
            var type = dialog["type"].Value<string>();

            switch (type.ToLower())
            {
                case "date":
                    var dateEdit = new DialogItem()
                    {
                        Name = GetValue(dialog, "name"),
                        Text = GetValue(dialog, "text"),
                        Value = GetValue(dialog, "value")
                    };

                    panel.Controls.Add(Create_DateEdit(dateEdit));

                    break;
                case "select":
                    var select = new Dialog_Select()
                    {
                        Name = GetValue(dialog, "name"),
                        Text = GetValue(dialog, "text"),
                        Value = GetValue(dialog, "value")
                    };

                    select.Data = GetValue(dialog, "data");
                    select.FieldText = GetValue(dialog, "fieldText");
                    select.FieldValue = GetValue(dialog, "fieldValue");


                   
                    if (dialog["options"] != null)
                    {
                        var options = new List<Dialog_Select_Option>();

                        var items = dialog["options"];

                        foreach (var item in items)
                        {
                            options.Add(new Dialog_Select_Option
                            {
                                Value = GetValue(item, "value"),
                                Text = GetValue(item, "text")
                            });
                        }

                        select.Options = options;
                    }
                    else
                        select.Options = null;

                    panel.Controls.Add(Create_Select(select, ds));
                    break;
                case "checkbox":
                    var checkbox = new DialogItem()
                    {
                        Name = GetValue(dialog, "name"),
                        Text = GetValue(dialog, "text"),
                        Value = GetValue(dialog, "value")
                    };

                    panel.Controls.Add(Create_CheckBox(checkbox));

                    break;
            }
        }




    }

    private string GetValue(JToken token, string propertyName)
    {
        JToken item;

        if (((JObject)token).TryGetValue(propertyName, out item))
        {
            return item.Value<string>();
        }
        else
            return null;
    }


    private HtmlGenericControl Create_Div_Label(string text)
    {
        var div = new HtmlGenericControl();

        div.TagName = "div";
        div.Attributes["class"] = "dialog-div";

        var label = new HtmlGenericControl();

        label.TagName = "label";
        label.InnerText = text;
        label.Attributes.Add("class", "dialog-label");

        div.Controls.Add(label);

        return div;
    }

    private HtmlGenericControl Create_DateEdit(DialogItem item)
    {
        var div = Create_Div_Label(item.Text);

        var dateEdit = new ASPxDateEdit();

        //dateEdit.ClientIDMode = System.Web.UI.ClientIDMode.Static;
        dateEdit.ID = item.Name;
        dateEdit.CssClass = "parameter";

        var date = DateTime.Today;

        var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddSeconds(-1);
        switch (item.Value.ToLower())
        {
            case "startmonth":
                date = firstDayOfMonth;
                break;
            case "endmonth":
                date = lastDayOfMonth;
                break;
            case "startyear":
                date = new DateTime(date.Year, 1, 1);
                break;
            case "endyear":
                date = new DateTime(date.Year, 12, 31);
                break;
            default:
                break;
        }

        dateEdit.Date = date;

        div.Controls.Add(dateEdit);

        return div;
    }

    private HtmlGenericControl Create_Select(Dialog_Select item, DataSet ds = null)
    {
        var div = Create_Div_Label(item.Text);

        var select = new HtmlGenericControl();


        select.TagName = "select";
        select.Attributes["id"] = item.Name;
        select.Attributes["name"] = item.Name;
        select.Attributes["class"] = "dialog-select parameter";


        var options = new List<Dialog_Select_Option>();

        if (string.IsNullOrEmpty(item.Data))
            options.AddRange(item.Options);
        else
        {
            var tableName = item.Data.ToLower();
            var dt = ds.Tables[tableName];

            options.AddRange(dt.AsEnumerable()
                .Select(x => new Dialog_Select_Option
                {
                    Value = x.Field<string>(item.FieldValue),
                    Text = x.Field<string>(item.FieldText),
                }));
        }

        var index = options.Count == 0 ? -1 : 0;
        switch (item.Value.ToLower())
        {
            case "last":
                index = options.Count - 1;
                break;
            case "first":
                index = 0;
                break;
            default:
                if(Int32.TryParse(item.Value, out index)){
                    index = index >= options.Count ? options.Count -1 : index;
                }
                break;
        }




        for(int i =0; i < options.Count ; i ++)
        {
            var option = options[i];
            var op = new HtmlGenericControl();
            op.TagName = "option";
            op.Attributes["value"] = option.Value;
            op.InnerText = option.Text;

            if (index == i)
            {
                op.Attributes["selected"] = "true";
            }

            select.Controls.Add(op);
        }




        div.Controls.Add(select);


        return div;
    }

    private HtmlGenericControl Create_CheckBox(DialogItem item)
    {
        var div = new HtmlGenericControl();

        div.TagName = "div";
        div.Attributes["style"] = "padding-top:5px; padding-bottom:5px; display:flex; align-items:center;";

        var check = new CheckBox();

        check.ClientIDMode = System.Web.UI.ClientIDMode.Static;
        check.ID = item.Name;
        check.Text = item.Text;
        check.CssClass = "parameter";

        switch (item.Value.ToLower())
        {
            case "check":
            case "checked":
            case "true":
                check.Checked = true;
                break;
            default:
                check.Checked = false;
                break;
        }
        div.Controls.Add(check);

        return div;
    }


    #endregion


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

    #region --Class--

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

    public class DialogItem
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public DialogType Type { get; set; }
        public string Value { get; set; }
    }

    public class Dialog_Select : DialogItem
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
    #endregion

}
