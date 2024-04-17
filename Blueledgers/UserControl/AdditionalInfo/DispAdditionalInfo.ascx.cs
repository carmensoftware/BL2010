using System;
using System.Data;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

// ReSharper disable once CheckNamespace
namespace BlueLedger.UserControls.AdditionalInfo
{
    public partial class DispAdditionalInfo : BaseUserControl
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
        ///     Get needed data.
        /// </summary>
        private void Page_Retrieve()
        {
            // Field
            _field.GetListIsAdditional(_dsField, SchemaName, PrimaryTable, LoginInfo.ConnStr);
        }

        /// <summary>
        ///     Display additional data
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
                    var lblLabel3 = new Label();
                    var lblLabel4 = new Label();

                    tr.Height = Unit.Pixel(15);

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
                    lblLabel1.ID = "lbl_" + _dsField.Tables[_field.TableName].Rows[i]["FieldName"] + "_Nm";
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

                    // Value
                    lblLabel2.ID = "lbl_" + _dsField.Tables[_field.TableName].Rows[i]["FieldName"];
                    lblLabel2.SkinID = "LBL_NORMAL";
                    //lbl_Label2.Visible  = rolePermission.GetIsVisible(LoginInfo.LoginName, SchemaName, PrimaryTable,
                    //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr);

                    // Identify field type and create WebControl which control type related to field type
                    switch (int.Parse(_dsField.Tables[_field.TableName].Rows[i]["FieldType"].ToString()))
                    {
                        case 1: // String
                        case 3: // Numeric
                            lblLabel2.Text = GetMiscValue(_dsField.Tables[_field.TableName].Rows[i]["FieldID"].ToString());

                            // Add controls to table cell
                            td2.Controls.Add(lblLabel2);
                            break;

                        case 2: // DateTime
                            var miscValue = GetMiscValue(_dsField.Tables[_field.TableName].Rows[i]["FieldID"].ToString());
                            lblLabel2.Text = (miscValue == string.Empty
                                ? string.Empty
                                : DateTime.Parse(miscValue).ToString("dd/MM/yyyy"));

                            // Add controls to table cell
                            td2.Controls.Add(lblLabel2);
                            break;

                        case 4: // Boolean
                            var chkCheckBox = new CheckBox
                            {
                                ID = "chk_" + _dsField.Tables[_field.TableName].Rows[i]["FieldName"],
                                Text = string.Empty,
                                SkinID = "CHK_NORMAL",
                                Enabled = false
                            };

                            var isChecked = GetMiscValue(_dsField.Tables[_field.TableName].Rows[i]["FieldID"].ToString());
                            chkCheckBox.Checked = (isChecked != string.Empty && bool.Parse(isChecked));
                            //chk_CheckBox.Visible    = rolePermission.GetIsVisible(LoginInfo.LoginName, SchemaName, PrimaryTable,
                            //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr);

                            // Add controls to table cell                
                            td2.Controls.Add(chkCheckBox);
                            break;

                        case 5: // Lookup
                            // If TextFieldID and ValueFieldID is null, Item of this lookup is in LookupItemTable
                            // Otherwise in a dimention table which TableID is equal to LookupID
                            if (_dsField.Tables[_field.TableName].Rows[i]["TextFieldID"] == DBNull.Value &&
                                _dsField.Tables[_field.TableName].Rows[i]["ValueFieldID"] == DBNull.Value)
                            {
                                // ItemList is in LookupItem
                                var lookupItem = new Blue.BL.Application.LookupItem();
                                lblLabel2.Text =
                                    lookupItem.GetDisplayText(
                                        _dsField.Tables[_field.TableName].Rows[i]["LookupID"].ToString(),
                                        GetMiscValue(_dsField.Tables[_field.TableName].Rows[i]["FieldID"].ToString()),
                                        LoginInfo.ConnStr);
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
                                var returnField =
                                    _field.GetFieldName(
                                        _dsField.Tables[_field.TableName].Rows[i]["TextFieldID"].ToString(),
                                        LoginInfo.ConnStr);
                                var parameterField =
                                    _field.GetFieldName(
                                        _dsField.Tables[_field.TableName].Rows[i]["ValueFieldID"].ToString(),
                                        LoginInfo.ConnStr);

                                lblLabel2.Text =
                                    Blue.BL.GnxLib.GetFieldValue("[" + schemaName + "].[" + tableName + "]", returnField,
                                        parameterField,
                                        GetMiscValue(_dsField.Tables[_field.TableName].Rows[i]["FieldID"].ToString()),
                                        LoginInfo.ConnStr);
                            }

                            // Add controls to table cell
                            td2.Controls.Add(lblLabel2);
                            break;
                    }

                    // Add table cells to table row
                    tr.Cells.Add(td1);
                    tr.Cells.Add(tdSpace1);
                    tr.Cells.Add(td2);

                    tdSpace2.Width = Unit.Percentage(2);

                    tr.Cells.Add(tdSpace2);

                    i++;

                    // Second Column
                    td3.HorizontalAlign = HorizontalAlign.Right;
                    td3.Width = Unit.Percentage(18);

                    tdSpace3.Width = Unit.Percentage(2);
                    tdSpace3.Text = "";

                    td4.HorizontalAlign = HorizontalAlign.Left;
                    td4.Width = Unit.Percentage(38);

                    if (i + 1 <= _dsField.Tables[_field.TableName].Rows.Count)
                    {
                        // Controls
                        // Title
                        lblLabel3.ID = "lbl_" + _dsField.Tables[_field.TableName].Rows[i]["FieldName"];
                        lblLabel3.SkinID = "LBL_BOLD";
                        lblLabel3.Text = (LoginInfo.BuFmtInfo.LangCode.ToUpper() == "EN-US"
                            ? (_dsField.Tables[_field.TableName].Rows[i]["DisplayText1"] == DBNull.Value
                                ? string.Empty
                                : _dsField.Tables[_field.TableName].Rows[i]["DisplayText1"].ToString())
                            : (_dsField.Tables[_field.TableName].Rows[i]["DisplayText2"] == DBNull.Value
                                ? string.Empty
                                : _dsField.Tables[_field.TableName].Rows[i]["DisplayText2"].ToString()));

                        // Add controls to table cell
                        //td3.CssClass = (lbl_Label3.Text != string.Empty ? CssClass : string.Empty);
                        td3.Controls.Add(lblLabel3);

                        // Set CssClass of space cell.
                        //tdSpace3.CssClass = (lbl_Label3.Text != string.Empty ? CssClass : string.Empty);

                        // Value
                        lblLabel4.ID = "lbl_" + _dsField.Tables[_field.TableName].Rows[i]["FieldName"];
                        lblLabel4.SkinID = "LBL_NORMAL";
                        //lbl_Label4.Visible  = rolePermission.GetIsVisible(LoginInfo.LoginName, SchemaName, PrimaryTable,
                        //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr);

                        // Identify field type and create WebControl which control type related to field type
                        switch (int.Parse(_dsField.Tables[_field.TableName].Rows[i]["FieldType"].ToString()))
                        {
                            case 1: // String
                            case 3: // Numeric
                                lblLabel4.Text =
                                    GetMiscValue(_dsField.Tables[_field.TableName].Rows[i]["FieldID"].ToString());

                                // Add controls to table cell
                                td4.Controls.Add(lblLabel4);
                                break;

                            case 2: // DateTime
                                var miscValue =
                                    GetMiscValue(_dsField.Tables[_field.TableName].Rows[i]["FieldID"].ToString());
                                lblLabel4.Text = (miscValue == string.Empty
                                    ? string.Empty
                                    : DateTime.Parse(miscValue).ToString("dd/MM/yyyy"));

                                // Add controls to table cell
                                td2.Controls.Add(lblLabel4);
                                break;

                            case 4: // Boolean
                                var chkCheckBox = new CheckBox
                                {
                                    ID = "chk_" + _dsField.Tables[_field.TableName].Rows[i]["FieldName"],
                                    Text = string.Empty,
                                    SkinID = "CHK_NORMAL",
                                    Enabled = false
                                };

                                var isChecked =
                                    GetMiscValue(_dsField.Tables[_field.TableName].Rows[i]["FieldID"].ToString());
                                chkCheckBox.Checked = (isChecked != string.Empty && bool.Parse(isChecked));
                                //chk_CheckBox.Visible    = rolePermission.GetIsVisible(LoginInfo.LoginName, SchemaName, PrimaryTable,
                                //    dsField.Tables[field.TableName].Rows[i]["FieldName"].ToString(), LoginInfo.ConnStr);

                                // Add controls to table cell
                                td4.Controls.Add(chkCheckBox);
                                break;

                            case 5: // Lookup
                                // If TextFieldID and ValueFieldID is null, Item of this lookup is in LookupItemTable
                                // Otherwise in a dimention table which TableID is equal to LookupID
                                if (_dsField.Tables[_field.TableName].Rows[i]["TextFieldID"] == DBNull.Value &&
                                    _dsField.Tables[_field.TableName].Rows[i]["ValueFieldID"] == DBNull.Value)
                                {
                                    // ItemList is in LookupItem
                                    var lookupItem = new Blue.BL.Application.LookupItem();
                                    lblLabel4.Text =
                                        lookupItem.GetDisplayText(
                                            _dsField.Tables[_field.TableName].Rows[i]["LookupID"].ToString(),
                                            GetMiscValue(_dsField.Tables[_field.TableName].Rows[i]["FieldID"].ToString()),
                                            LoginInfo.ConnStr);
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
                                    var returnField =
                                        _field.GetFieldName(
                                            _dsField.Tables[_field.TableName].Rows[i]["TextFieldID"].ToString(),
                                            LoginInfo.ConnStr);
                                    var parameterField =
                                        _field.GetFieldName(
                                            _dsField.Tables[_field.TableName].Rows[i]["ValueFieldID"].ToString(),
                                            LoginInfo.ConnStr);

                                    lblLabel4.Text =
                                        Blue.BL.GnxLib.GetFieldValue("[" + schemaName + "].[" + tableName + "]",
                                            returnField,
                                            parameterField,
                                            GetMiscValue(_dsField.Tables[_field.TableName].Rows[i]["FieldID"].ToString()),
                                            LoginInfo.ConnStr);
                                }

                                // Add controls to table cell                                
                                td4.Controls.Add(lblLabel4);
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
            foreach (DataRow drMisc in DataSource.Rows)
            {
                if (drMisc["FieldID"].ToString() == fieldID)
                {
                    return (drMisc["FieldID"] == DBNull.Value ? string.Empty : drMisc["Value"].ToString());
                }
            }

            return string.Empty;
        }

/*
        /// <summary>
        ///     Get field display text by FieldID.
        /// </summary>
        /// <param name="fieldID"></param>
        /// <returns></returns>
        private string GetFieldDisplayText(string fieldID)
        {
            var field = new Blue.BL.Application.Field();

            return field.GetDisplayText(fieldID, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);
        }
*/

/*
        /// <summary>
        ///     Get value of MISC field.
        /// </summary>
        /// <param name="fieldID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetFieldValueText(string fieldID, string value)
        {
            var result = string.Empty;
            var dsField = new DataSet();
            var field = new Blue.BL.Application.Field();

            // Get field specification data.
            var retrieved = field.Get(dsField, fieldID, LoginInfo.ConnStr);

            if (retrieved)
            {
                if (dsField.Tables[field.TableName].Rows.Count > 0)
                {
                    var drField = dsField.Tables[field.TableName].Rows[0];

                    if (drField["LookupID"] != DBNull.Value)
                    {
                        // Stored value isn't a display value. 
                        // Have to find display from lookup table or dimention table.
                        if (drField["TextFieldID"] != DBNull.Value && drField["ValueFieldID"] != DBNull.Value)
                        {
                            // Find display text from dimention table. So LookupID is a TableID.
                            // Find Schema Name
                            var schemaName = Blue.BL.GnxLib.GetFieldValue("Application.[Table]", "Schema", "TableID",
                                drField["LookupID"].ToString(), LoginInfo.ConnStr);

                            // Find Table Name                                                
                            var tableName = Blue.BL.GnxLib.GetFieldValue("Application.[Table]", "TableName", "TableID",
                                drField["LookupID"].ToString(), LoginInfo.ConnStr);

                            // Find Return Value Field and Parameter Field 
                            var returnValueField = field.GetFieldName(drField["TextFieldID"].ToString(),
                                LoginInfo.ConnStr);
                            var parameterField = field.GetFieldName(drField["ValueFieldID"].ToString(),
                                LoginInfo.ConnStr);

                            // Get Value
                            result = Blue.BL.GnxLib.GetFieldValue("[" + schemaName + "].[" + tableName + "]",
                                returnValueField, parameterField,
                                value, LoginInfo.ConnStr);
                        }
                        else
                        {
                            // Find display text from lookup table
                            var lookupItem = new Blue.BL.Application.LookupItem();
                            result = lookupItem.GetDisplayText(drField["LookupID"].ToString(), value, LoginInfo.ConnStr);
                        }
                    }
                    else
                    {
                        // Stored value is a display value.
                        result = value;
                    }
                }
            }

            return result;
        }
*/

        /// <summary>
        ///     Page load event.
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