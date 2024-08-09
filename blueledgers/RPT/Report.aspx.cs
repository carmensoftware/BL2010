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
    private readonly string pathReport = @"~\App_Files\Reports\";

    protected LoginInformation LoginInfo;

    protected IEnumerable<DialogParameter> _Parameters
    {
        get
        {
            var urlParam = Request.QueryString["parameters"] == null ? "" : Request.QueryString["parameters"].ToString();
            var json = DecodeBase64(urlParam);
            return JsonConvert.DeserializeObject<IEnumerable<DialogParameter>>(json);

            //return values == null ? new DialogParameter[] { } : values.ToArray();
        }
    }


    protected override void InitializeCulture()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("th") { DateTimeFormat = { Calendar = new GregorianCalendar() } };
        base.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["LoginInfo"] == null)
            {
                Session["PreviousPage"] = HttpContext.Current.Request.Url.AbsoluteUri;
                Response.Redirect("~/ErrorPages/SessionTimeOut.aspx");
            }

            LoginInfo = (LoginInformation)Session["LoginInfo"];

            Page_Setup();
        }
        catch
        {
            Response.Redirect("~/Option/User/Default.aspx");
        }
    }

    private void Page_Setup()
    {
        this.Title = string.Format("Reports - {0}", LoginInfo.BuInfo.BuName);
        var id = Request.QueryString["id"];

        if (id != null)
        {
            var report = GetReport(LoginInfo.ConnStr, id.ToString());

            lbl_Title.Text = report.Name;
            lbl_Bu.Text = "at " + LoginInfo.BuInfo.BuName;

            if (string.IsNullOrEmpty(report.Dialog))
            {
                panel_Dialog.Visible = false;
                LoadReport_Old(report);
            }
            else
            {
                panel_Dialog.Visible = true;
                var parameters = _Parameters;

                SetDialog(panel_Parameters, report.Dialog, parameters);
                LoadReport(report.Report, parameters);
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
    private string EncodeBase64(string value)
    {
        var valueBytes = Encoding.UTF8.GetBytes(value);
        return Convert.ToBase64String(valueBytes);
    }

    private string DecodeBase64(string value)
    {
        var valueBytes = System.Convert.FromBase64String(value);
        return Encoding.UTF8.GetString(valueBytes);
    }

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

    #region -- Dialog --

    // Set dialog
    private void SetDialog(Panel panel, string json, IEnumerable<DialogParameter> parameters = null)
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
            var name = GetValue(dialog, "name");
            var text = GetValue(dialog, "text");
            var value = GetValue(dialog, "value");
            var p_value = "";

            if (parameters != null)
            {
                var item = parameters.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

                p_value = item != null ? item.Value : "";
            }


            switch (type.ToLower())
            {
                case "date":
                    var dateEdit = new DialogItem()
                    {
                        Name = name,
                        Text = text,
                        Value = value
                    };

                    panel.Controls.Add(Create_DateEdit(dateEdit, p_value));

                    break;

                case "select":
                    var select = new Dialog_Select()
                    {
                        Name = name,
                        Text = text,
                        Value = value
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

                    panel.Controls.Add(Create_Select(select, ds, p_value));
                    break;

                case "checkbox":
                    var checkbox = new DialogItem()
                    {
                        Name = name,
                        Text = text,
                        Value = value
                    };

                    panel.Controls.Add(Create_CheckBox(checkbox, p_value));

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

    private HtmlGenericControl Create_DateEdit(DialogItem item, string parameter_value)
    {
        var div = Create_Div_Label(item.Text);

        var dateEdit = new ASPxDateEdit();

        //dateEdit.ClientIDMode = System.Web.UI.ClientIDMode.Static;
        dateEdit.ID = item.Name;
        dateEdit.CssClass = "parameter";

        var date = DateTime.Today;

        var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddSeconds(-1);

        if (string.IsNullOrEmpty(parameter_value))
        {
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
        }
        else
        {
            var testDate = DateTime.Today;

            if (DateTime.TryParse(parameter_value, out testDate))
            {
                date = testDate;
            }
        }

        dateEdit.Date = date;

        div.Controls.Add(dateEdit);

        return div;
    }

    private HtmlGenericControl Create_Select(Dialog_Select item, DataSet ds, string parameter_value)
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

        // Set the value


        var index = options.Count == 0 ? -1 : 0;

        if (string.IsNullOrEmpty(parameter_value))
        {
            switch (item.Value.ToLower())
            {
                case "last":
                    index = options.Count - 1;
                    break;
                case "first":
                    index = 0;
                    break;
                default:
                    if (Int32.TryParse(item.Value, out index))
                    {
                        index = index >= options.Count ? options.Count - 1 : index;
                    }
                    break;
            }

        }
        else
        {
            for (int i = 0; i < options.Count; i++)
            {
                if (options[i].Value.ToLower() == parameter_value.ToLower())
                {
                    index = i;
                    break;
                }
            }
        }

        for (int i = 0; i < options.Count; i++)
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

    private HtmlGenericControl Create_CheckBox(DialogItem item, string parameter_value)
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

        if (!string.IsNullOrEmpty(parameter_value))
        {
            var value = false;
            if (bool.TryParse(parameter_value, out value))
            {
                check.Checked = value;
            }
        }

        div.Controls.Add(check);

        return div;
    }

    #endregion

    #region --Fast Report--
    private void LoadReport(string report, IEnumerable<DialogParameter> parameters = null)
    {
        if (parameters == null)
        {
            WebReport1.Visible = false;

            return;
        }

        var filename = pathReport + report;

        WebReport1.Visible = true;
        WebReport1.ReportFile = filename;
        SetReportPage(filename);

        WebReport1.Dialogs = false;

        // Set parameters to report
        foreach (var item in parameters)
        {
            WebReport1.Report.SetParameterValue(item.Name, item.Value);
        }


    }

    private void LoadReport_Old(ReportItem item)
    {
        var filename = pathReport + item.Report;
        WebReport1.Visible = true;
        WebReport1.ReportFile = filename;

        SetReportPage(filename);
    }

    private void SetReportPage(string filename)
    {
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
    #endregion

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

    public class DialogParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    #endregion

}
