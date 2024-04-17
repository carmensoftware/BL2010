using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Data;
using DevExpress.Web.ASPxEditors;
using System.Web.UI;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Dialog
/// </summary>
public class ReportDialog
{
    private readonly Panel _panel;
    private readonly string _connectionString;

    public ReportDialog(Panel panel, string connectionString)
    {
        _panel = panel;
        _connectionString = connectionString;
    }


    public void CreateDialog(string xml)
    {
        _panel.Visible = true;

        CreateControls(_panel, xml);
    }

    private void CreateControls(Panel panel, string xml)
    {
        if (string.IsNullOrEmpty(xml))
            return;

        XElement doc = XElement.Parse(xml);

        var ds = new DataSet();
        var dataSource = doc.DescendantsAndSelf("DataSource").FirstOrDefault();

        if (dataSource != null)
        {
            foreach (var item in dataSource.Descendants())
            {
                var tagName = item.Name.LocalName.ToLower();
                var tableName = GetAttributeValue(item, "Name");

                if (tagName == "data")
                {
                    var dt = new DataTable();

                    var sql = GetAttributeValue(item, "Query");

                    if (!string.IsNullOrEmpty(sql))
                    {
                        dt = ExecuteQuery(sql, null, _connectionString);
                        dt.TableName = tableName;

                        ds.Tables.Add(dt);
                    }
                }
            }
        }


        var dialog = doc.DescendantsAndSelf("Dialog").FirstOrDefault();

        if (dialog != null)
        {

            foreach (var item in dialog.Descendants())
            {
                const int FONT_SIZE = 10;
                var tagName = item.Name.LocalName.ToLower();
                var p = new Panel();
                p.CssClass = "dialog-control";

                var controlId = GetAttributeValue(item, "Name");
                var div = new Panel();

                div.Style.Add("padding-top", "5px");

                switch (tagName)
                {
                    #region --Label--
                    case "label":
                        var label = new Label();

                        label.Font.Size = FONT_SIZE;
                        label.Font.Bold = true;
                        label.Text = GetAttributeValue(item, "Text");

                        div.Style.Add("padding-top", "10px");
                        div.Controls.Add(label);
                        p.Controls.Add(div);

                        break;
                    #endregion
                    #region --Date--
                    case "date":
                        var date = new ASPxDateEdit()
                        {
                            ID = controlId,
                            AutoPostBack = true
                        };

                        date.Font.Size = FONT_SIZE;
                        date.Date = DateTime.Today;
                        //p.Controls.Add(date);
                        div.Controls.Add(date);
                        p.Controls.Add(div);

                        break;
                    #endregion
                    #region --Text--
                    case "text":
                        var text = new TextBox()
                        {
                            ID = controlId,
                            AutoPostBack = true
                        };

                        text.Font.Size = FONT_SIZE;
                        text.Text = GetAttributeValue(item, "Text");
                        div.Controls.Add(text);
                        p.Controls.Add(div);

                        break;
                    #endregion
                    #region --Checkbox--
                    case "checkbox":
                        var checkbox = new CheckBox()
                        {
                            ID = controlId,
                            AutoPostBack = true
                        };

                        checkbox.Font.Size = FONT_SIZE;
                        checkbox.Text = GetAttributeValue(item, "Text");
                        checkbox.Checked = GetAttributeValue(item, "Checked").ToLower() == "true";
                        div.Controls.Add(checkbox);
                        p.Controls.Add(div);

                        break;
                    #endregion
                    #region --Text--
                    case "select":
                        var select = new ASPxComboBox()
                        {
                            ID = controlId,
                            EnableCallbackMode = true,
                            CallbackPageSize = 50,
                            IncrementalFilteringMode = IncrementalFilteringMode.Contains,
                            AutoPostBack = true,
                            DropDownStyle = DropDownStyle.DropDownList
                        };

                        select.Font.Size = FONT_SIZE;
                        var optWidth = GetAttributeValue(item, "Width");
                        select.Width = string.IsNullOrEmpty(optWidth) ? 400 : Convert.ToInt32(optWidth);
                        select.Items.Clear();


                        foreach (var option in item.Descendants("Option"))
                        {
                            select.Items.Add(new ListEditItem
                            {
                                Text = GetAttributeValue(option, "Text"),
                                Value = GetAttributeValue(option, "Value"),
                                Selected = GetAttributeValue(option, "Selected").ToLower() == "true"
                            });
                        }

                        div.Controls.Add(select);
                        p.Controls.Add(div);

                        break;
                    #endregion
                    #region --Lookup--
                    case "lookup":
                        var lookup = new ASPxComboBox()
                        {
                            ID = controlId,
                            EnableCallbackMode = true,
                            CallbackPageSize = 50,
                            IncrementalFilteringMode = IncrementalFilteringMode.Contains,
                            AutoPostBack = true,
                            DropDownStyle = DropDownStyle.DropDownList
                        };

                        lookup.Font.Size = FONT_SIZE;
                        lookup.Width = 400;
                        lookup.Items.Clear();

                        var dsName = GetAttributeValue(item, "DataSource");

                        if (!string.IsNullOrEmpty(dsName) && ds.Tables[dsName] != null)
                        {
                            foreach (DataRow dr in ds.Tables[dsName].Rows)
                            {
                                var dataValue = GetAttributeValue(item, "DataValue");
                                var dataText = GetAttributeValue(item, "DataText");

                                lookup.Items.Add(new ListEditItem
                                {
                                    Value = dr[dataValue].ToString(),
                                    Text = dr[dataText].ToString()
                                });

                            }

                        }

                        // Set value
                        var value = GetAttributeValue(item, "Value").ToLower();

                        switch (value)
                        {
                            case "first":
                                lookup.SelectedIndex = lookup.Items.Count > 0 ? 0 : -1;
                                break;

                            case "end":
                            case "last":
                                lookup.SelectedIndex = lookup.Items.Count > 0 ? lookup.Items.Count - 1 : -1;
                                break;
                        }


                        lookup.AutoPostBack = true;

                        div.Controls.Add(lookup);
                        p.Controls.Add(div);


                        break;
                    #endregion
                }

                panel.Controls.Add(p);
            }


        }
    }

    private void SetLookupItems(ASPxComboBox lookup, string items, string values)
    {
        if (items != "")
        {
            var itemArray = items.Split('~');
            var valueArray = values.Split('~');

            lookup.Items.Clear();
            for (int i = 0; i < itemArray.Length; i++)
            {
                lookup.Items.Add(new ListEditItem
                {
                    Text = itemArray[i],
                    Value = valueArray[i]
                });
            }
        }
    }

    private string GetAttributeValue(XElement item, string attributeName)
    {
        return item.Attribute(attributeName) != null ? item.Attribute(attributeName).Value : string.Empty;
    }

    private string GetControlValue(Control control)
    {
        var value = "";
        switch (control.GetType().ToString())
        {
            case "DevExpress.Web.ASPxEditors.ASPxDateEdit":
                value = (control as ASPxDateEdit).Date.ToString("yyyy-MM-dd");

                break;
            case "DevExpress.Web.ASPxEditors.ASPxComboBox":
                value = (control as ASPxComboBox).Value.ToString();
                break;
        }



        return value;

    }

    // DB
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
