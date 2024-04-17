using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BlueLedger.PL.BaseClass;
using Newtonsoft.Json.Linq;
using System.Text;
using DevExpress.Web.ASPxEditors;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.UI.HtmlControls;
using System.Globalization;

namespace BlueLedger.PL.Report
{
    public partial class Reports : BasePage
    {
        private string _id
        {
            get { return Request.QueryString["id"] == null ? string.Empty : Request.QueryString["id"].ToString(); }
        }

        protected IEnumerable<ReportItem> _reports
        {
            get
            {
                return ViewState["ReportItems"] as IEnumerable<ReportItem>;
            }
            set
            {
                //if (ViewState["ReportItems"] == null)
                //    ViewState["ReportItems"] = new List<ReportItem>();

                ViewState["ReportItems"] = value;
            }
        }

        // Event(s)

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _reports = new List<ReportItem>();

                GetReportList();

                if (!string.IsNullOrEmpty(_id))
                {
                    CreateDialog(_id);

                }
            }
        }


        // Private method(s)
        private string FormatDate(DateTime date)
        {
            return date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        private DataTable ExecuteQuery(string connectionString, string query, IEnumerable<SqlParameter> parameters = null)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    using (var da = new SqlDataAdapter(query, conn))
                    {
                        if (parameters != null && parameters.Count() > 0)
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

        private void GetReportList()
        {
            var connStr = LoginInfo.ConnStr;
            var dt = ExecuteQuery(connStr, "SELECT Id, [Name], [Description], IsStandard FROM RPT.Report WHERE IsActive=1");

            _reports = dt.AsEnumerable()
                .Select(x => new ReportItem
                {
                    Id = x.Field<string>("Id"),
                    Name = x.Field<string>("Name"),
                    Description = x.Field<string>("Description"),
                    IsStandard = x.Field<bool>("IsStandard")
                })
                .OrderBy(x => x.Name)
                .ToArray();


        }

        private void CreateDialog(string id)
        {
            var connStr = LoginInfo.ConnStr;
            var dtReport = ExecuteQuery(connStr, "SELECT * FROM RPT.Report WHERE Id=@id", new List<SqlParameter> { new SqlParameter("@id", id) });

            if (dtReport != null && dtReport.Rows.Count > 0)
            {
                DataRow drReport = dtReport.Rows[0];

                var reportName = drReport["Name"].ToString();
                var jsonDialog = drReport["Dialog"].ToString();

                lbl_ReportName.Text = reportName;

                panel_Dialog.Controls.Clear();


                if (!string.IsNullOrEmpty(jsonDialog))
                {


                    var dataSet = GetDialogData(jsonDialog, connStr);
                    var items = GetDialogItem(jsonDialog, connStr);


                    foreach (var item in items)
                    {
                        var label = new Label();

                        label.Text = item.Title;
                        label.Font.Bold = true;
                        var panel = new Panel();

                        var itemValue = item.Value.ToLower();

                        switch (item.Type.ToLower())
                        {
                            case "select":
                                #region -- Select --
                                var select = new HtmlGenericControl("select");

                                select.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                                select.ID = item.Name;
                                select.Attributes["class"] = "parameter select2";
                                select.Style.Add("width", "320px");

                                var optionItems = new StringBuilder();

                                if (string.IsNullOrEmpty(item.Data))
                                {
                                    if (item.Options != null)
                                    {
                                        foreach (var option in item.Options)
                                        {
                                            var selected = option.Selected ? "selected" : "";
                                            optionItems.AppendLine(string.Format("<option value='{0}' {2}>{1}</option>", option.Value, option.Text, selected));
                                        }

                                        select.InnerHtml = optionItems.ToString();
                                    }
                                }
                                else if (dataSet.Tables[item.Data] != null)
                                {
                                    var dt = dataSet.Tables[item.Data];
                                    var index = -1;

                                    switch (itemValue)
                                    {
                                        case "last":
                                            index = dt.Rows.Count > 0 ? dt.Rows.Count - 1 : -1;
                                            break;
                                        default:
                                            index = dt.Rows.Count > 0 ? 0 : -1;
                                            break;
                                    }


                                    //foreach (var option in dt.AsEnumerable())
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        var dr = dt.Rows[i];

                                        var value = dr["Value"].ToString();
                                        var text = dr["Text"].ToString();
                                        var selected = i == index ? "selected" : "";

                                        optionItems.AppendLine(string.Format("<option value='{0}' {2}>{1}</option>", value, text, selected));

                                    }
                                    select.InnerHtml = optionItems.ToString();


                                }
                                
                                panel.Controls.Add(select);
                                panel_Dialog.Controls.Add(label);
                                panel_Dialog.Controls.Add(panel);

                                break;
                                #endregion
                            case "date":
                                #region --DatePicker--
                                var datepicker = new HtmlGenericControl("input");

                                datepicker.ID = "fromDate";
                                datepicker.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                                datepicker.Attributes["class"] = "parameter datepicker";

                                var today = DateTime.Today;

                                var dateValue = today;

                                switch (itemValue)
                                {
                                    case "today":
                                        break;
                                    case "firstmonth":
                                        dateValue = new DateTime(today.Year, today.Month, 1);
                                        break;
                                    case "endmonth":
                                        var firstMonth = new DateTime(today.Year, today.Month, 1);
                                        dateValue = firstMonth.AddMonths(1).AddDays(-1);
                                        break;
                                    case "firstYear":
                                        dateValue = new DateTime(today.Year, 1, 1);
                                        break;
                                    case "endYear":
                                        dateValue = new DateTime(today.Year, 12, 31);
                                        break;
                                    default:
                                        break;
                                }

                                datepicker.Attributes["Value"] = FormatDate( dateValue);


                                panel.Controls.Add(datepicker);
                                panel_Dialog.Controls.Add(label);
                                panel_Dialog.Controls.Add(panel);
                                #endregion
                                break;

                        }

                        panel_Dialog.Controls.Add(new Literal { Text = "<br />" });
                    }
                }
            }
        }


        private DataSet GetDialogData(string jsonDialog, string connectionString)
        {
            var ds = new DataSet();

            var dialog = JObject.Parse(jsonDialog);
            var data = (JArray)dialog["Data"];

            if (data != null)
            {
                foreach (JObject item in data)
                {
                    var name = item["Name"].ToString();
                    var query = item["Query"].ToString();

                    var dt = ExecuteQuery(connectionString, "EXEC " + query);

                    dt.TableName = name;

                    ds.Tables.Add(dt);

                }

            }

            return ds;
        }

        private IEnumerable<ReportDialogItem> GetDialogItem(string jsonDialog, string connectionString)
        {
            var items = new List<ReportDialogItem>();
            var dialogs = (JArray)JObject.Parse(jsonDialog)["Dialog"];

            if (dialogs != null)
            {
                foreach (JObject dialog in dialogs)
                {
                    var name = dialog["Name"].ToString();
                    var title = dialog["Title"].ToString();
                    var type = dialog["Type"].ToString();
                    var data = dialog["Data"] == null ? string.Empty : dialog["Data"].ToString();
                    var value = dialog["Value"] == null ? string.Empty : dialog["Value"].ToString();
                    var options = dialog["Options"] == null ? null : JsonConvert.DeserializeObject<IEnumerable<SelectOptionItem>>(dialog["Options"].ToString());

                    items.Add(new ReportDialogItem
                    {
                        Name = name,
                        Title = title,
                        Type = type,
                        Value = value,
                        Data = data,
                        Options = options
                    });

                }

            }

            return items;
        }


    }


    [Serializable]
    public class ReportItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsStandard { get; set; }
        public string Dialog { get; set; }
        public string FileName { get; set; }
    }

    public class ReportDialogData
    {
        public string Name { get; set; }
        public string Query { get; set; }
    }

    public class ReportDialogItem
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        public string Value { get; set; }
        public IEnumerable<SelectOptionItem> Options { get; set; }
    }

    public class SelectOptionItem
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; }
    }

}

