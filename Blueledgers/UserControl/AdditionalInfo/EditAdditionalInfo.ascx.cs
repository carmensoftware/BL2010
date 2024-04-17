using System;
using System.Data;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;

// ReSharper disable once CheckNamespace
namespace BlueLedger.UserControls.AdditionalInfo
{
    public partial class EditAdditionalInfo : BaseUserControl
    {
        #region "Attributies"

        private readonly DataSet _dsField = new DataSet();
        private readonly Blue.BL.Application.Field _field = new Blue.BL.Application.Field();
        //private Blue.BL.Security.RolePermission _rolePermission = new Blue.BL.Security.RolePermission();

        /// <summary>
        ///     Schema of primary table.
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        ///     Name of main table which related to additional field table.
        /// </summary>
        public string PrimaryTable { get; set; }

        /// <summary>
        ///     Data source of this displayed data.
        /// </summary>
        public DataTable DataSource { get; set; }

        /// <summary>
        ///     Set or Get style of additional information field.
        /// </summary>
        public string CssClass { get; set; }

        #endregion

        #region "Operations"

        /// <summary>
        ///     Retrieve needed data.
        /// </summary>
        private void Page_Retrieve()
        {
            // Field
            _field.GetListIsAdditional(_dsField, SchemaName, PrimaryTable, LoginInfo.ConnStr);
        }

        /// <summary>
        ///     Binding Control
        /// </summary>
        private void Page_Setting()
        {
            if (_dsField.Tables[_field.TableName] != null)
            {
                for (var i = 0; i < _dsField.Tables[_field.TableName].Rows.Count; i++)
                {
                    var tr = new TableRow {Height = Unit.Pixel(20)};
                    var td1 = new TableCell();
                    var tdSpace1 = new TableCell();
                    var td2 = new TableCell();
                    var tdSpace2 = new TableCell();
                    var td3 = new TableCell();
                    var tdSpace3 = new TableCell();
                    var td4 = new TableCell();
                    var lblLabel1 = new Label();
                    var lblLabel2 = new Label();

                    // First Column
                    // Table cells
                    td1.HorizontalAlign = HorizontalAlign.Right;
                    td1.Width = Unit.Percentage(18);
                    //td1.CssClass        = CssClass;

                    tdSpace1.Width = Unit.Percentage(2);
                    //tdSpace1.CssClass   = CssClass;
                    tdSpace1.Text = "";

                    td2.HorizontalAlign = HorizontalAlign.Left;
                    td2.Width = Unit.Percentage(20);
                    //td2.CssClass        = CssClass;

                    // Controls    
                    // Title
                    lblLabel1.ID = "lbl_" + _dsField.Tables[_field.TableName].Rows[i]["FieldName"];
                    lblLabel1.SkinID = "LBL_BOLD";
                    lblLabel1.Text = (LoginInfo.BuFmtInfo.LangCode.ToUpper() == "EN-US"
                        ? (_dsField.Tables[_field.TableName].Rows[i]["DisplayText1"] == DBNull.Value
                            ? string.Empty
                            : _dsField.Tables[_field.TableName].Rows[i]["DisplayText1"].ToString())
                        : (_dsField.Tables[_field.TableName].Rows[i]["DisplayText2"] == DBNull.Value
                            ? string.Empty
                            : _dsField.Tables[_field.TableName].Rows[i]["DisplayText2"].ToString()));

                    // Add controls to table cell
                    td1.Controls.Add(lblLabel1);

                    // Input control
                    // Identify field type and create WebControl which control type related to field type
                    switch (int.Parse(_dsField.Tables[_field.TableName].Rows[i]["FieldType"].ToString()))
                    {
                        case 1: // String
                            var txtTextBox1 = new TextBox
                            {
                                ID = "txt_" + _dsField.Tables[_field.TableName].Rows[i]["FieldName"],
                                SkinID = "TXT_NORMAL",
                                Width = Unit.Pixel(150),
                                Text = GetMiscValue(_dsField.Tables[_field.TableName].Rows[i]["FieldID"].ToString())
                            };

                            //if (rolePermission.GetIsVisible(LoginInfo.LoginName, SchemaName, PrimaryTable,
                            //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr))
                            //{
                            //}

                            //txt_TextBox1.Enabled = rolePermission.GetIsModify(LoginInfo.LoginName, SchemaName, PrimaryTable,
                            //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr);

                            // Add controls to table cell                
                            td2.Controls.Add(txtTextBox1);
                            break;

                        case 2: // DateTime                            
                            var deCalendar = new ASPxDateEdit
                            {
                                ID = "de_" + _dsField.Tables[_field.TableName].Rows[i]["FieldName"],
                                Width = Unit.Pixel(150)
                            };

                            //if (rolePermission.GetIsVisible(LoginInfo.LoginName, SchemaName, PrimaryTable,
                            //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr))
                            //{
                            var miscValue = GetMiscValue(_dsField.Tables[_field.TableName].Rows[i]["FieldID"].ToString());

                            //    if (miscValue != string.Empty)
                            //    {
                            deCalendar.Value = DateTime.Parse(miscValue);
                            //    }
                            //}

                            //de_Calendar.Enabled = rolePermission.GetIsModify(LoginInfo.LoginName, SchemaName, PrimaryTable,
                            //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr);

                            // Add controls to table cell                
                            td2.Controls.Add(deCalendar);
                            break;

                        case 3: // Numeric
                            var txtTextBox2 = new TextBox
                            {
                                ID = "txt_" + _dsField.Tables[_field.TableName].Rows[i]["FieldName"],
                                SkinID = "TXT_NORMAL_NUM",
                                Width = Unit.Pixel(150),
                                Text = GetMiscValue(_dsField.Tables[_field.TableName].Rows[i]["FieldID"].ToString())
                            };

                            //if (rolePermission.GetIsVisible(LoginInfo.LoginName, SchemaName, PrimaryTable,
                            //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr))
                            //{
                            //}

                            //txt_TextBox2.Enabled = rolePermission.GetIsModify(LoginInfo.LoginName, SchemaName, PrimaryTable,
                            //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr);

                            // Add controls to table cell                
                            td2.Controls.Add(txtTextBox2);
                            break;

                        case 4: // Boolean
                            var chkCheckBox = new CheckBox
                            {
                                ID = "chk_" + _dsField.Tables[_field.TableName].Rows[i]["FieldName"],
                                Text = string.Empty,
                                SkinID = "CHK_NORMAL"
                            };

                            //if (rolePermission.GetIsVisible(LoginInfo.LoginName, SchemaName, PrimaryTable,
                            //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr))
                            //{
                            var isChecked = GetMiscValue(_dsField.Tables[_field.TableName].Rows[i]["FieldID"].ToString());
                            chkCheckBox.Checked = (isChecked != string.Empty && bool.Parse(isChecked));
                            //}

                            //chk_CheckBox.Enabled = rolePermission.GetIsModify(LoginInfo.LoginName, SchemaName, PrimaryTable,
                            //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr);

                            // Add controls to table cell                
                            td2.Controls.Add(chkCheckBox);
                            break;

                        case 5: // Lookup
                            // If TextFieldID and ValueFieldID is null, Item of this lookup is in LookupItemTable
                            // Otherwise in a dimention table which TableID is equal to LookupID
                            var ddlDropDownList = new DropDownList
                            {
                                ID = "ddl_" + _dsField.Tables[_field.TableName].Rows[i]["FieldName"],
                                SkinID = "DDL_NORMAL",
                                Width = Unit.Pixel(150)
                            };

                            if (_dsField.Tables[_field.TableName].Rows[i]["TextFieldID"] == DBNull.Value &&
                                _dsField.Tables[_field.TableName].Rows[i]["ValueFieldID"] == DBNull.Value)
                            {
                                // ItemList is in LookupItem
                                var lookupItem = new Blue.BL.Application.LookupItem();

                                var dtLookupItem =
                                    lookupItem.GetList(_dsField.Tables[_field.TableName].Rows[i]["LookupID"].ToString(),
                                        LoginInfo.ConnStr);

                                // Add blank row
                                var drBlank = dtLookupItem.NewRow();
                                dtLookupItem.Rows.InsertAt(drBlank, 0);

                                ddlDropDownList.DataSource = dtLookupItem;
                                ddlDropDownList.DataTextField = "Text";
                                ddlDropDownList.DataValueField = "Value";
                            }
                            else
                            {
                                // ItemList is in dimention table
                                // Find Schema Name
                                var schemaName = Blue.BL.GnxLib.GetFieldValue("Application.[Table]", "Schema", "TableID",
                                    _dsField.Tables[_field.TableName].Rows[i]["LookupID"].ToString(), LoginInfo.ConnStr);

                                // Find Table Name                                                
                                var tableName = Blue.BL.GnxLib.GetFieldValue("Application.[Table]", "TableName",
                                    "TableID", _dsField.Tables[_field.TableName].Rows[i]["LookupID"].ToString(),
                                    LoginInfo.ConnStr);

                                // Find Return Value Field and Parameter Field 
                                var textField =
                                    _field.GetFieldName(
                                        _dsField.Tables[_field.TableName].Rows[i]["TextFieldID"].ToString(),
                                        LoginInfo.ConnStr);
                                var valueField =
                                    _field.GetFieldName(
                                        _dsField.Tables[_field.TableName].Rows[i]["ValueFieldID"].ToString(),
                                        LoginInfo.ConnStr);

                                var dtDataList = Blue.BL.GnxLib.GetDataList("[" + schemaName + "].[" + tableName + "]",
                                    LoginInfo.ConnStr);

                                // Add blank row
                                var drBlank = dtDataList.NewRow();
                                dtDataList.Rows.InsertAt(drBlank, 0);

                                ddlDropDownList.DataSource = dtDataList;
                                ddlDropDownList.DataTextField = textField;
                                ddlDropDownList.DataValueField = valueField;
                            }

                            //ddl_DropDownList.Enabled = rolePermission.GetIsModify(LoginInfo.LoginName, SchemaName, PrimaryTable,
                            //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr);

                            ddlDropDownList.DataBind();

                            //if (rolePermission.GetIsVisible(LoginInfo.LoginName, SchemaName, PrimaryTable,
                            //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr))
                            //{
                            ddlDropDownList.SelectedValue =
                                GetMiscValue(_dsField.Tables[_field.TableName].Rows[i]["FieldID"].ToString());
                            //}                            

                            // Add controls to table cell
                            td2.Controls.Add(ddlDropDownList);
                            break;
                    }

                    // Add table cells to table row
                    tr.Cells.Add(td1);
                    tr.Cells.Add(tdSpace1);
                    tr.Cells.Add(td2);

                    tdSpace2.Width = Unit.Percentage(2);

                    tr.Cells.Add(tdSpace2);

                    i++;

                    // Table cells
                    td3.HorizontalAlign = HorizontalAlign.Right;
                    td3.Width = Unit.Percentage(18);

                    tdSpace3.Width = Unit.Percentage(2);
                    tdSpace3.Text = "";

                    td4.HorizontalAlign = HorizontalAlign.Left;
                    td4.Width = Unit.Percentage(38);

                    // Second Column
                    if (i + 1 <= _dsField.Tables[_field.TableName].Rows.Count)
                    {
                        // Controls
                        // Title
                        lblLabel2.ID = "lbl_" + _dsField.Tables[_field.TableName].Rows[i]["FieldName"];
                        lblLabel2.SkinID = "LBL_BOLD";
                        lblLabel2.Text = (LoginInfo.BuFmtInfo.LangCode.ToUpper() == "EN-US"
                            ? (_dsField.Tables[_field.TableName].Rows[i]["DisplayText1"] == DBNull.Value
                                ? string.Empty
                                : _dsField.Tables[_field.TableName].Rows[i]["DisplayText1"].ToString())
                            : (_dsField.Tables[_field.TableName].Rows[i]["DisplayText2"] == DBNull.Value
                                ? string.Empty
                                : _dsField.Tables[_field.TableName].Rows[i]["DisplayText2"].ToString()));

                        // Add controls to table cell
                        //td3.CssClass = CssClass;
                        td3.Controls.Add(lblLabel2);

                        // Set CssClass of space cell.
                        //tdSpace3.CssClass   = CssClass;

                        // Input control
                        // Identify field type and create WebControl which control type related to field type
                        switch (int.Parse(_dsField.Tables[_field.TableName].Rows[i]["FieldType"].ToString()))
                        {
                            case 1: // String
                                var txtTextBox1 = new TextBox
                                {
                                    ID = "txt_" + _dsField.Tables[_field.TableName].Rows[i]["FieldName"],
                                    SkinID = "TXT_NORMAL",
                                    Width = Unit.Pixel(150),
                                    Text = GetMiscValue(_dsField.Tables[_field.TableName].Rows[i]["FieldID"].ToString())
                                };

                                //if (rolePermission.GetIsVisible(LoginInfo.LoginName, SchemaName, PrimaryTable,
                                //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr))
                                //{
                                //}

                                //txt_TextBox1.Enabled = rolePermission.GetIsModify(LoginInfo.LoginName, SchemaName, PrimaryTable,
                                //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr);

                                // Add controls to table cell                
                                td4.Controls.Add(txtTextBox1);
                                break;

                            case 2: // DateTime
                                var deCalendar = new ASPxDateEdit
                                {
                                    ID = "de_" + _dsField.Tables[_field.TableName].Rows[i]["FieldName"],
                                    Width = Unit.Pixel(150)
                                };

                                //if (rolePermission.GetIsVisible(LoginInfo.LoginName, SchemaName, PrimaryTable,
                                //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr))
                                //{
                                var miscValue =
                                    GetMiscValue(_dsField.Tables[_field.TableName].Rows[i]["FieldID"].ToString());

                                //if (miscValue != string.Empty)
                                //{
                                deCalendar.Value = DateTime.Parse(miscValue);
                                //    }
                                //}

                                //de_Calendar.Enabled = rolePermission.GetIsModify(LoginInfo.LoginName, SchemaName, PrimaryTable,
                                //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr);

                                // Add controls to table cell                
                                td4.Controls.Add(deCalendar);
                                break;

                            case 3: // Numeric
                                var txtTextBox2 = new TextBox
                                {
                                    ID = "txt_" + _dsField.Tables[_field.TableName].Rows[i]["FieldName"],
                                    SkinID = "TXT_NORMAL_NUM",
                                    Width = Unit.Pixel(150),
                                    Text = GetMiscValue(_dsField.Tables[_field.TableName].Rows[i]["FieldID"].ToString())
                                };

                                //if (rolePermission.GetIsVisible(LoginInfo.LoginName, SchemaName, PrimaryTable,
                                //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr))
                                //{
                                //}

                                //txt_TextBox2.Enabled = rolePermission.GetIsModify(LoginInfo.LoginName, SchemaName, PrimaryTable,
                                //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr);

                                // Add controls to table cell                
                                td4.Controls.Add(txtTextBox2);
                                break;

                            case 4: // Boolean
                                var chkCheckBox = new CheckBox
                                {
                                    ID = "chk_" + _dsField.Tables[_field.TableName].Rows[i]["FieldName"],
                                    Text = string.Empty,
                                    SkinID = "CHK_NORMAL"
                                };

                                //if (rolePermission.GetIsVisible(LoginInfo.LoginName, SchemaName, PrimaryTable,
                                //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr))
                                //{
                                var isChecked =
                                    GetMiscValue(_dsField.Tables[_field.TableName].Rows[i]["FieldID"].ToString());
                                chkCheckBox.Checked = (isChecked != string.Empty && bool.Parse(isChecked));
                                //}

                                //chk_CheckBox.Enabled = rolePermission.GetIsModify(LoginInfo.LoginName, SchemaName, PrimaryTable,
                                //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr);

                                // Add controls to table cell                
                                td4.Controls.Add(chkCheckBox);
                                break;

                            case 5: // Lookup
                                // If TextFieldID and ValueFieldID is null, Item of this lookup is in LookupItemTable
                                // Otherwise in a dimention table which TableID is equal to LookupID
                                var ddlDropDownList = new DropDownList
                                {
                                    ID = "ddl_" + _dsField.Tables[_field.TableName].Rows[i]["FieldName"],
                                    SkinID = "DDL_NORMAL",
                                    Width = Unit.Pixel(150)
                                };

                                if (_dsField.Tables[_field.TableName].Rows[i]["TextFieldID"] == DBNull.Value &&
                                    _dsField.Tables[_field.TableName].Rows[i]["ValueFieldID"] == DBNull.Value)
                                {
                                    var lookupItem = new Blue.BL.Application.LookupItem();

                                    var dtLookupItem =
                                        lookupItem.GetList(
                                            _dsField.Tables[_field.TableName].Rows[i]["LookupID"].ToString(),
                                            LoginInfo.ConnStr);

                                    // Add blank row
                                    var drBlank = dtLookupItem.NewRow();
                                    dtLookupItem.Rows.InsertAt(drBlank, 0);

                                    ddlDropDownList.DataSource = dtLookupItem;
                                    ddlDropDownList.DataTextField = "Text";
                                    ddlDropDownList.DataValueField = "Value";
                                }
                                else
                                {
                                    // ItemList is in dimention table
                                    // Find Schema Name
                                    var schemaName = Blue.BL.GnxLib.GetFieldValue("Application.[Table]", "Schema",
                                        "TableID", _dsField.Tables[_field.TableName].Rows[i]["LookupID"].ToString(),
                                        LoginInfo.ConnStr);

                                    // Find Table Name                                                
                                    var tableName = Blue.BL.GnxLib.GetFieldValue("Application.[Table]", "TableName",
                                        "TableID", _dsField.Tables[_field.TableName].Rows[i]["LookupID"].ToString(),
                                        LoginInfo.ConnStr);

                                    // Find Return Value Field and Parameter Field 
                                    var textField =
                                        _field.GetFieldName(
                                            _dsField.Tables[_field.TableName].Rows[i]["TextFieldID"].ToString(),
                                            LoginInfo.ConnStr);
                                    var valueField =
                                        _field.GetFieldName(
                                            _dsField.Tables[_field.TableName].Rows[i]["ValueFieldID"].ToString(),
                                            LoginInfo.ConnStr);

                                    var dtDataList =
                                        Blue.BL.GnxLib.GetDataList("[" + schemaName + "].[" + tableName + "]",
                                            LoginInfo.ConnStr);

                                    // Add blank row
                                    var drBlank = dtDataList.NewRow();
                                    dtDataList.Rows.InsertAt(drBlank, 0);

                                    ddlDropDownList.DataSource = dtDataList;
                                    ddlDropDownList.DataTextField = textField;
                                    ddlDropDownList.DataValueField = valueField;
                                }

                                //ddl_DropDownList.Enabled = rolePermission.GetIsModify(LoginInfo.LoginName, SchemaName, PrimaryTable,
                                //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr);

                                ddlDropDownList.DataBind();

                                //if (rolePermission.GetIsVisible(LoginInfo.LoginName, SchemaName, PrimaryTable,
                                //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr))
                                //{
                                ddlDropDownList.SelectedValue =
                                    GetMiscValue(_dsField.Tables[_field.TableName].Rows[i]["FieldID"].ToString());
                                //}

                                // Add controls to table cell                                
                                td4.Controls.Add(ddlDropDownList);
                                break;
                        }

                        //td4.CssClass = CssClass;
                    }

                    // Add table cells to table row
                    tr.Cells.Add(td3);
                    tr.Cells.Add(tdSpace3);
                    tr.Cells.Add(td4);

                    // Add table row to table
                    tbl_AdditionalInfo.Rows.Add(tr);
                }
            }
        }

        /// <summary>
        ///     Get value of specified FieldID in DataSource
        /// </summary>
        /// <param name="fieldID"></param>
        /// <returns></returns>
        private string GetMiscValue(string fieldID)
        {
            if (DataSource.Rows != null)
            {
                foreach (DataRow drMisc in DataSource.Rows)
                {
                    if (drMisc["FieldID"].ToString() == fieldID)
                    {
                        return (drMisc["FieldID"] == DBNull.Value ? string.Empty : drMisc["Value"].ToString());
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Page load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
                Page_Setting();
            }
        }

        #endregion
    }
}