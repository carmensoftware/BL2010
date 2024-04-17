using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BlueLedger.PL.BaseClass;
using System.IO;
using BlueLedger;


namespace BlueLedger.PL.UserControls
{
    public partial class AddressInformation : BaseUserControl
    {
        #region "Attributies"

        Blue.BL.Profile.Address address = new Blue.BL.Profile.Address();
        Blue.BL.Profile.AddressPart addPart = new Blue.BL.Profile.AddressPart();
        Blue.BL.Profile.AddressType addType = new Blue.BL.Profile.AddressType();
        Blue.BL.Reference.Country country = new Blue.BL.Reference.Country();
        Blue.BL.AR.ARProfile arProfile = new Blue.BL.AR.ARProfile();
        DataSet dsAddress = new DataSet();
        DataSet dsAddressCount = new DataSet();
        DataTable dtLookupItem = new DataTable();
        //string profileCode;

        private Boolean _readOnly;
        /// <summary>
        /// Get or Set enable object.
        /// </summary>
        public Boolean ReadOnly
        {
            get
            {
                this._readOnly = (ViewState["ReadOnly"] == null ? false : (bool)ViewState["ReadOnly"]);
                return this._readOnly;
            }
            set
            {
                this._readOnly = value;
                ViewState.Add("ReadOnly", this._readOnly);
            }
        }

        private DataSet _dataSource = new DataSet();
        /// <summary>
        /// Get or Set Control DataSource.
        /// </summary>
        public DataSet DataSource
        {
            get
            {
                this._dataSource = (ViewState["DataSource"] == null ? new DataSet() : (DataSet)ViewState["DataSource"]);
                return this._dataSource;
            }
            set
            {
                this._dataSource = value;
                ViewState.Add("DataSource", this._dataSource);
            }
        }

        private Guid _profileCode;
        private int _addressID;
        public Guid ProfileCode
        {
            get
            {
                this._profileCode = (ViewState["ProfileCode"] == null ? new Guid() : (Guid)ViewState["ProfileCode"]);
                return this._profileCode;
            }
            set
            {
                this._profileCode = value;
                ViewState.Add("ProfileCode", this._profileCode);
            }
        }

        public int AddressID
        {
            get
            {
                this._addressID = (ViewState["AddressID"] == null ? 0 : (int)ViewState["AddressID"]);
                return this._addressID;
            }
            set
            {
                this._addressID = value;
                ViewState.Add("AddressID", this._addressID);
            }
        }

        #endregion

        #region "Operations"

        /// <summary>
        /// Get address data.
        /// </summary>
        private void Page_Retrieve()
        {
            address.GetAddressListByProfileCode(ProfileCode.ToString(), dsAddress, LoginInfo.ConnStr);
            address.GetAddressListDescByProfileCode(ProfileCode.ToString(), dsAddressCount, LoginInfo.ConnStr);


            if (Path.GetFileName(Request.Path).ToString() == "VendorEdit.aspx")
            {
                if (Request.QueryString["MODE"].ToString().ToUpper() == "NEW")
                {
                    this.AddressID = 0;
                }
                else
                {
                    if (dsAddress.Tables[address.TableName].Rows.Count > 0)
                    {
                        this.AddressID = Convert.ToInt32(dsAddressCount.Tables[address.TableName].Rows[0]["AddressID"].ToString());
                    }

                }
            }



            if (!ReadOnly)
            {
                // Set Lookup
                dtLookupItem = addPart.GetAddressPartList(LoginInfo.ConnStr);

                // Get country.
                country.GetCountryList(dsAddress, LoginInfo.ConnStr);

                // Get addresstry.
                addType.GetAddressTypeList(dsAddress, LoginInfo.ConnStr);
            }

            // Set session
            Session["dtLookup"] = dtLookupItem;
            Session["dsAddress"] = dsAddress;
            Session["dsAddressCount"] = dsAddressCount;

            this.DataSource = dsAddress;
        }

        /// <summary>
        /// Display address data.
        /// </summary>
        private void Page_Setting()
        {

            grd_Address.DataSource = dsAddress.Tables[address.TableName];
            grd_Address.Columns[0].Visible = !ReadOnly;
            grd_Address.DataBind();


            // Hide new button when control is readonly.
            btn_New.Visible = !ReadOnly;

        }

        /// <summary>
        /// Load page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Page_Retrieve();
                this.Page_Setting();
            }
            else
            {
                dsAddress = (DataSet)Session["dsAddress"];
                dsAddressCount = (DataSet)Session["dsAddressCount"];
                dtLookupItem = (DataTable)Session["dtLookup"];
            }
        }

        /// <summary>
        /// Binding data to grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Address_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // AddressID
                if (e.Row.FindControl("lbl_AddressID") != null)
                {
                    Label lbl_AddressID = (Label)e.Row.FindControl("lbl_AddressID");
                    lbl_AddressID.Text = DataBinder.Eval(e.Row.DataItem, "AddressID").ToString();
                }

                // Address
                if (e.Row.FindControl("lbl_Address_Nm") != null)
                {
                    Label lbl_Address = (Label)e.Row.FindControl("lbl_Address_Nm");
                    int row = e.Row.DataItemIndex + 1;
                    lbl_Address.Text = "Address#" + row.ToString();
                }

                // Default
                if (e.Row.FindControl("chk_Default") != null)
                {
                    CheckBox chk_Default = (CheckBox)e.Row.FindControl("chk_Default");
                    chk_Default.Checked = (DataBinder.Eval(e.Row.DataItem, "IsDefault") == DBNull.Value ? false : bool.Parse(DataBinder.Eval(e.Row.DataItem, "IsDefault").ToString()));
                }

                // Active
                if (e.Row.FindControl("chk_Active") != null)
                {
                    CheckBox chk_Active = (CheckBox)e.Row.FindControl("chk_Active");
                    chk_Active.Checked = (DataBinder.Eval(e.Row.DataItem, "IsActive") == DBNull.Value ? false : bool.Parse(DataBinder.Eval(e.Row.DataItem, "IsActive").ToString()));
                }

                // Street
                if (e.Row.FindControl("lbl_Street") != null)
                {
                    Label lbl_Street = (Label)e.Row.FindControl("lbl_Street");
                    lbl_Street.Text = DataBinder.Eval(e.Row.DataItem, "Street").ToString();
                }

                // Address1Part
                if (e.Row.FindControl("lbl_Address1Part") != null)
                {
                    Label lbl_Address1Part = (Label)e.Row.FindControl("lbl_Address1Part");
                    lbl_Address1Part.Text = addPart.GetAddressPartName(DataBinder.Eval(e.Row.DataItem, "Address1Part").ToString(), LoginInfo.ConnStr) == string.Empty ?
                        string.Empty : addPart.GetAddressPartName(DataBinder.Eval(e.Row.DataItem, "Address1Part").ToString(), LoginInfo.ConnStr) + ":";
                }

                // Address1
                if (e.Row.FindControl("lbl_Address1") != null)
                {
                    Label lbl_Address1 = (Label)e.Row.FindControl("lbl_Address1");
                    lbl_Address1.Text = DataBinder.Eval(e.Row.DataItem, "Address1").ToString();
                }

                // Address2Part
                if (e.Row.FindControl("lbl_Address2Part") != null)
                {
                    Label lbl_Address2Part = (Label)e.Row.FindControl("lbl_Address2Part");
                    lbl_Address2Part.Text = addPart.GetAddressPartName(DataBinder.Eval(e.Row.DataItem, "Address2Part").ToString(), LoginInfo.ConnStr) == string.Empty ?
                        string.Empty : addPart.GetAddressPartName(DataBinder.Eval(e.Row.DataItem, "Address2Part").ToString(), LoginInfo.ConnStr) + ":";
                }

                // Address2
                if (e.Row.FindControl("lbl_Address2") != null)
                {
                    Label lbl_Address2 = (Label)e.Row.FindControl("lbl_Address2");
                    lbl_Address2.Text = DataBinder.Eval(e.Row.DataItem, "Address2").ToString();
                }

                // Address3Part
                if (e.Row.FindControl("lbl_Address3Part") != null)
                {
                    Label lbl_Address3Part = (Label)e.Row.FindControl("lbl_Address3Part");
                    lbl_Address3Part.Text = addPart.GetAddressPartName(DataBinder.Eval(e.Row.DataItem, "Address3Part").ToString(), LoginInfo.ConnStr) == string.Empty ?
                        string.Empty : addPart.GetAddressPartName(DataBinder.Eval(e.Row.DataItem, "Address3Part").ToString(), LoginInfo.ConnStr) + ":";
                }

                // Address3
                if (e.Row.FindControl("lbl_Address3") != null)
                {
                    Label lbl_Address3 = (Label)e.Row.FindControl("lbl_Address3");
                    lbl_Address3.Text = DataBinder.Eval(e.Row.DataItem, "Address3").ToString();
                }

                // Address4Part
                if (e.Row.FindControl("lbl_Address4Part") != null)
                {
                    Label lbl_Address4Part = (Label)e.Row.FindControl("lbl_Address4Part");
                    lbl_Address4Part.Text = addPart.GetAddressPartName(DataBinder.Eval(e.Row.DataItem, "Address4Part").ToString(), LoginInfo.ConnStr) == string.Empty ?
                        string.Empty : addPart.GetAddressPartName(DataBinder.Eval(e.Row.DataItem, "Address4Part").ToString(), LoginInfo.ConnStr) + ":";
                }

                // Address4
                if (e.Row.FindControl("lbl_Address4") != null)
                {
                    Label lbl_Address4 = (Label)e.Row.FindControl("lbl_Address4");
                    lbl_Address4.Text = DataBinder.Eval(e.Row.DataItem, "Address4").ToString();
                }

                // Address5Part
                if (e.Row.FindControl("lbl_Address5Part") != null)
                {
                    Label lbl_Address5Part = (Label)e.Row.FindControl("lbl_Address5Part");
                    lbl_Address5Part.Text = addPart.GetAddressPartName(DataBinder.Eval(e.Row.DataItem, "Address5Part").ToString(), LoginInfo.ConnStr) == string.Empty ?
                        string.Empty : addPart.GetAddressPartName(DataBinder.Eval(e.Row.DataItem, "Address5Part").ToString(), LoginInfo.ConnStr) + ":";
                }

                // Address5
                if (e.Row.FindControl("lbl_Address5") != null)
                {
                    Label lbl_Address5 = (Label)e.Row.FindControl("lbl_Address5");
                    lbl_Address5.Text = DataBinder.Eval(e.Row.DataItem, "Address5").ToString();
                }

                // Address6Part
                if (e.Row.FindControl("lbl_Address6Part") != null)
                {
                    Label lbl_Address6Part = (Label)e.Row.FindControl("lbl_Address6Part");
                    lbl_Address6Part.Text = addPart.GetAddressPartName(DataBinder.Eval(e.Row.DataItem, "Address6Part").ToString(), LoginInfo.ConnStr) == string.Empty ?
                        string.Empty : addPart.GetAddressPartName(DataBinder.Eval(e.Row.DataItem, "Address6Part").ToString(), LoginInfo.ConnStr) + ":";
                }

                // Address6
                if (e.Row.FindControl("lbl_Address6") != null)
                {
                    Label lbl_Address6 = (Label)e.Row.FindControl("lbl_Address6");
                    lbl_Address6.Text = DataBinder.Eval(e.Row.DataItem, "Address6").ToString();
                }

                // Address7Part
                if (e.Row.FindControl("lbl_Address7Part") != null)
                {
                    Label lbl_Address7Part = (Label)e.Row.FindControl("lbl_Address7Part");
                    lbl_Address7Part.Text = addPart.GetAddressPartName(DataBinder.Eval(e.Row.DataItem, "Address7Part").ToString(), LoginInfo.ConnStr) == string.Empty ?
                        string.Empty : addPart.GetAddressPartName(DataBinder.Eval(e.Row.DataItem, "Address7Part").ToString(), LoginInfo.ConnStr) + ":";
                }

                // Address7
                if (e.Row.FindControl("lbl_Address7") != null)
                {
                    Label lbl_Address7 = (Label)e.Row.FindControl("lbl_Address7");
                    lbl_Address7.Text = DataBinder.Eval(e.Row.DataItem, "Address7").ToString();
                }

                // Address8Part
                if (e.Row.FindControl("lbl_Address8Part") != null)
                {
                    Label lbl_Address8Part = (Label)e.Row.FindControl("lbl_Address8Part");
                    lbl_Address8Part.Text = addPart.GetAddressPartName(DataBinder.Eval(e.Row.DataItem, "Address8Part").ToString(), LoginInfo.ConnStr) == string.Empty ?
                        string.Empty : addPart.GetAddressPartName(DataBinder.Eval(e.Row.DataItem, "Address8Part").ToString(), LoginInfo.ConnStr) + ":";
                }

                // Address8
                if (e.Row.FindControl("lbl_Address8") != null)
                {
                    Label lbl_Address8 = (Label)e.Row.FindControl("lbl_Address8");
                    lbl_Address8.Text = DataBinder.Eval(e.Row.DataItem, "Address8").ToString();
                }

                // Address9Part
                if (e.Row.FindControl("lbl_Address9Part") != null)
                {
                    Label lbl_Address9Part = (Label)e.Row.FindControl("lbl_Address9Part");
                    lbl_Address9Part.Text = addPart.GetAddressPartName(DataBinder.Eval(e.Row.DataItem, "Address9Part").ToString(), LoginInfo.ConnStr) == string.Empty ?
                        string.Empty : addPart.GetAddressPartName(DataBinder.Eval(e.Row.DataItem, "Address9Part").ToString(), LoginInfo.ConnStr) + ":";
                }

                // Address9
                if (e.Row.FindControl("lbl_Address9") != null)
                {
                    Label lbl_Address9 = (Label)e.Row.FindControl("lbl_Address9");
                    lbl_Address9.Text = DataBinder.Eval(e.Row.DataItem, "Address9").ToString();
                }

                // Address10Part
                if (e.Row.FindControl("lbl_Address10Part") != null)
                {
                    Label lbl_Address10Part = (Label)e.Row.FindControl("lbl_Address10Part");
                    lbl_Address10Part.Text = addPart.GetAddressPartName(DataBinder.Eval(e.Row.DataItem, "Address10Part").ToString(), LoginInfo.ConnStr) == string.Empty ?
                        string.Empty : addPart.GetAddressPartName(DataBinder.Eval(e.Row.DataItem, "Address10Part").ToString(), LoginInfo.ConnStr) + ":";
                }

                // Address10
                if (e.Row.FindControl("lbl_Address10") != null)
                {
                    Label lbl_Address10 = (Label)e.Row.FindControl("lbl_Address10");
                    lbl_Address10.Text = DataBinder.Eval(e.Row.DataItem, "Address10").ToString();
                }

                // Country
                if (e.Row.FindControl("lbl_Country") != null)
                {
                    Label lbl_Country = (Label)e.Row.FindControl("lbl_Country");
                    lbl_Country.Text = country.GetCountryName(DataBinder.Eval(e.Row.DataItem, "CountryCode") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "CountryCode").ToString(), LoginInfo.ConnStr);
                }

                // Address type 
                if (e.Row.FindControl("lbl_AddressType") != null)
                {
                    Label lbl_AddressType = (Label)e.Row.FindControl("lbl_AddressType");
                    lbl_AddressType.Text = addType.GetAddressTypeName(DataBinder.Eval(e.Row.DataItem, "AddressTypeCode").ToString(), LoginInfo.ConnStr);
                }

                // AddressID
                if (e.Row.FindControl("lbl_AddressID") != null)
                {
                    Label lbl_AddressID = (Label)e.Row.FindControl("lbl_AddressID");
                    lbl_AddressID.Text = DataBinder.Eval(e.Row.DataItem, "AddressID").ToString();
                }

                // Address
                if (e.Row.FindControl("lbl_Address_Nm") != null)
                {
                    Label lbl_Address = (Label)e.Row.FindControl("lbl_Address_Nm");
                    int row = e.Row.DataItemIndex + 1;
                    lbl_Address.Text = "Address#" + row.ToString();
                }

                // Street
                if (e.Row.FindControl("txt_Street") != null)
                {
                    TextBox txt_Street = (TextBox)e.Row.FindControl("txt_Street");
                    txt_Street.Text = DataBinder.Eval(e.Row.DataItem, "Street").ToString();
                }

                // Address1Part
                if (e.Row.FindControl("ddl_Address1Part") != null)
                {
                    DropDownList ddl_Address1Part = (DropDownList)e.Row.FindControl("ddl_Address1Part");
                    this.BindingLookup(ddl_Address1Part);
                    ddl_Address1Part.SelectedValue = DataBinder.Eval(e.Row.DataItem, "Address1Part").ToString(); ;
                }

                // Address1
                if (e.Row.FindControl("txt_Address1") != null)
                {
                    TextBox txt_Address1 = (TextBox)e.Row.FindControl("txt_Address1");
                    txt_Address1.Text = DataBinder.Eval(e.Row.DataItem, "Address1").ToString();
                }

                // Address2Part
                if (e.Row.FindControl("ddl_Address2Part") != null)
                {
                    DropDownList ddl_Address2Part = (DropDownList)e.Row.FindControl("ddl_Address2Part");
                    this.BindingLookup(ddl_Address2Part);
                    ddl_Address2Part.SelectedValue = DataBinder.Eval(e.Row.DataItem, "Address2Part").ToString();
                }

                // Address2
                if (e.Row.FindControl("txt_Address2") != null)
                {
                    TextBox txt_Address2 = (TextBox)e.Row.FindControl("txt_Address2");
                    txt_Address2.Text = DataBinder.Eval(e.Row.DataItem, "Address2").ToString();
                }

                // Address3Part
                if (e.Row.FindControl("ddl_Address3Part") != null)
                {
                    DropDownList ddl_Address3Part = (DropDownList)e.Row.FindControl("ddl_Address3Part");
                    this.BindingLookup(ddl_Address3Part);
                    ddl_Address3Part.SelectedValue = DataBinder.Eval(e.Row.DataItem, "Address3Part").ToString();
                }

                // Address3
                if (e.Row.FindControl("txt_Address3") != null)
                {
                    TextBox txt_Address3 = (TextBox)e.Row.FindControl("txt_Address3");
                    txt_Address3.Text = DataBinder.Eval(e.Row.DataItem, "Address3").ToString();
                }

                // Address4Part
                if (e.Row.FindControl("ddl_Address4Part") != null)
                {
                    DropDownList ddl_Address4Part = (DropDownList)e.Row.FindControl("ddl_Address4Part");
                    this.BindingLookup(ddl_Address4Part);
                    ddl_Address4Part.SelectedValue = DataBinder.Eval(e.Row.DataItem, "Address4Part").ToString();
                }

                // Address4
                if (e.Row.FindControl("txt_Address4") != null)
                {
                    TextBox txt_Address4 = (TextBox)e.Row.FindControl("txt_Address4");
                    txt_Address4.Text = DataBinder.Eval(e.Row.DataItem, "Address4").ToString();
                }

                // Address5Part
                if (e.Row.FindControl("ddl_Address5Part") != null)
                {
                    DropDownList ddl_Address5Part = (DropDownList)e.Row.FindControl("ddl_Address5Part");
                    this.BindingLookup(ddl_Address5Part);
                    ddl_Address5Part.SelectedValue = DataBinder.Eval(e.Row.DataItem, "Address5Part").ToString();
                }

                // Address5
                if (e.Row.FindControl("txt_Address5") != null)
                {
                    TextBox txt_Address5 = (TextBox)e.Row.FindControl("txt_Address5");
                    txt_Address5.Text = DataBinder.Eval(e.Row.DataItem, "Address5").ToString();
                }

                // Address6Part
                if (e.Row.FindControl("ddl_Address6Part") != null)
                {
                    DropDownList ddl_Address6Part = (DropDownList)e.Row.FindControl("ddl_Address6Part");
                    this.BindingLookup(ddl_Address6Part);
                    ddl_Address6Part.SelectedValue = DataBinder.Eval(e.Row.DataItem, "Address6Part").ToString();
                }

                // Address6
                if (e.Row.FindControl("txt_Address6") != null)
                {
                    TextBox txt_Address6 = (TextBox)e.Row.FindControl("txt_Address6");
                    txt_Address6.Text = DataBinder.Eval(e.Row.DataItem, "Address6").ToString();
                }

                // Address7Part
                if (e.Row.FindControl("ddl_Address7Part") != null)
                {
                    DropDownList ddl_Address7Part = (DropDownList)e.Row.FindControl("ddl_Address7Part");
                    this.BindingLookup(ddl_Address7Part);
                    ddl_Address7Part.SelectedValue = DataBinder.Eval(e.Row.DataItem, "Address7Part").ToString();
                }

                // Address7
                if (e.Row.FindControl("txt_Address7") != null)
                {
                    TextBox txt_Address7 = (TextBox)e.Row.FindControl("txt_Address7");
                    txt_Address7.Text = DataBinder.Eval(e.Row.DataItem, "Address7").ToString();
                }

                // Address8Part
                if (e.Row.FindControl("ddl_Address8Part") != null)
                {
                    DropDownList ddl_Address8Part = (DropDownList)e.Row.FindControl("ddl_Address8Part");

                    this.BindingLookup(ddl_Address8Part);

                    ddl_Address8Part.SelectedValue = DataBinder.Eval(e.Row.DataItem, "Address8Part").ToString();
                }

                // Address8 
                if (e.Row.FindControl("txt_Address8") != null)
                {
                    TextBox txt_Address8 = (TextBox)e.Row.FindControl("txt_Address8");
                    txt_Address8.Text = DataBinder.Eval(e.Row.DataItem, "Address8").ToString();
                }

                // Address9Part
                if (e.Row.FindControl("ddl_Address9Part") != null)
                {
                    DropDownList ddl_Address9Part = (DropDownList)e.Row.FindControl("ddl_Address9Part");
                    this.BindingLookup(ddl_Address9Part);
                    ddl_Address9Part.SelectedValue = DataBinder.Eval(e.Row.DataItem, "Address9Part").ToString();
                }

                // Address9
                if (e.Row.FindControl("txt_Address9") != null)
                {
                    TextBox txt_Address9 = (TextBox)e.Row.FindControl("txt_Address9");
                    txt_Address9.Text = DataBinder.Eval(e.Row.DataItem, "Address9").ToString();
                }

                // Address10Part
                if (e.Row.FindControl("ddl_Address10Part") != null)
                {
                    DropDownList ddl_Address10Part = (DropDownList)e.Row.FindControl("ddl_Address10Part");
                    this.BindingLookup(ddl_Address10Part);
                    ddl_Address10Part.SelectedValue = DataBinder.Eval(e.Row.DataItem, "Address10Part").ToString();
                }

                // Address10
                if (e.Row.FindControl("txt_Address10") != null)
                {
                    TextBox txt_Address10 = (TextBox)e.Row.FindControl("txt_Address10");
                    txt_Address10.Text = DataBinder.Eval(e.Row.DataItem, "Address10").ToString();
                }

                // Count Code
                if (e.Row.FindControl("ddl_Country") != null)
                {
                    DropDownList ddl_Country = (DropDownList)e.Row.FindControl("ddl_Country");

                    ddl_Country.DataSource = dsAddress.Tables[country.TableName];
                    ddl_Country.DataTextField = "Name";
                    ddl_Country.DataValueField = "CountryCode";
                    ddl_Country.DataBind();

                    if (DataBinder.Eval(e.Row.DataItem, "CountryCode").ToString() != string.Empty)
                    {
                        ddl_Country.SelectedValue = DataBinder.Eval(e.Row.DataItem, "CountryCode").ToString();
                    }
                    else
                    {

                    }
                    ddl_Country.SelectedValue = "TH";
                }

                // Country
                if (e.Row.FindControl("txt_Country") != null)
                {
                    TextBox txt_Country = (TextBox)e.Row.FindControl("txt_Country");
                    txt_Country.Text = country.GetCountryName(DataBinder.Eval(e.Row.DataItem, "CountryCode").ToString(), LoginInfo.ConnStr);
                }

                //Address type code
                if (e.Row.FindControl("ddl_AddressType") != null)
                {
                    DropDownList ddl_AddressType = (DropDownList)e.Row.FindControl("ddl_AddressType");

                    ddl_AddressType.DataSource = dsAddress.Tables[addType.TableName];
                    ddl_AddressType.DataTextField = "Name";
                    ddl_AddressType.DataValueField = "AddressTypeCode";
                    ddl_AddressType.DataBind();

                    ddl_AddressType.SelectedValue = DataBinder.Eval(e.Row.DataItem, "AddressTypeCode").ToString();
                }

                //Address type code
                if (e.Row.FindControl("txt_AddressType") != null)
                {
                    TextBox txt_AddressType = (TextBox)e.Row.FindControl("txt_AddressType");
                    txt_AddressType.Text = addType.GetAddressTypeName(DataBinder.Eval(e.Row.DataItem, "AddressTypeCode").ToString(), LoginInfo.ConnStr);
                }
            }
        }

        /// <summary>
        /// Edit select row.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Address_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grd_Address.EditIndex = e.NewEditIndex;
            grd_Address.DataSource = dsAddress.Tables[address.TableName];
            grd_Address.DataBind();
        }

        /// <summary>
        /// Delete select row.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Address_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string ChkAddressID = grd_Address.DataKeys[e.RowIndex].Values[1].ToString();

            // Setup primary key(s)
            string profileCode = grd_Address.DataKeys[e.RowIndex].Values[0].ToString();
            int addressID = Convert.ToInt32(grd_Address.DataKeys[e.RowIndex].Values[1]);

            // Delete row in grid display
            for (int i = dsAddress.Tables[address.TableName].Rows.Count - 1; i >= 0; i--)
            {
                DataRow drAddress = dsAddress.Tables[address.TableName].Rows[i];

                if (drAddress.RowState != DataRowState.Deleted)
                {
                    if (drAddress["profileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                       Convert.ToInt32(drAddress["addressID"]) == addressID)
                    {
                        drAddress.Delete();
                        continue;
                    }
                }
            }


            // Binding grid
            grd_Address.DataSource = dsAddress.Tables[address.TableName];
            grd_Address.DataBind();

            // Save changed to session
            Session["dsAddress"] = dsAddress;

            this.DataSource = dsAddress;

        }

        /// <summary>
        /// Cancel edit row.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Address_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Cancel row edit
            string AddressID = grd_Address.DataKeys[e.RowIndex].Values[0].ToString();

            if (dsAddress.Tables[address.TableName].Rows.Count > 0)
            {
                for (int i = dsAddress.Tables[address.TableName].Rows.Count - 1; i >= 0; i--)
                {
                    DataRow drCanceling = dsAddress.Tables[address.TableName].Rows[i];

                    if (drCanceling.RowState != DataRowState.Deleted)
                    {
                        if (drCanceling["AddressID"].ToString() == AddressID && drCanceling.RowState == DataRowState.Added)
                        {
                            drCanceling.Delete();
                            continue;
                        }
                    }
                }
            }

            // Refresh data in GridView
            grd_Address.DataSource = dsAddress.Tables[address.TableName];
            grd_Address.EditIndex = -1;
            grd_Address.DataBind();

            // Save changed to session
            Session["dsAddress"] = dsAddress;
            this.DataSource = dsAddress;
        }

        /// <summary>
        /// Update data to dataset.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Address_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            CheckBox chk_Default = (CheckBox)grd_Address.Rows[e.RowIndex].FindControl("chk_Default");
            CheckBox chk_Active = (CheckBox)grd_Address.Rows[e.RowIndex].FindControl("chk_Active");
            TextBox txt_Street = (TextBox)grd_Address.Rows[e.RowIndex].FindControl("txt_Street");
            TextBox txt_Address1 = (TextBox)grd_Address.Rows[e.RowIndex].FindControl("txt_Address1");
            TextBox txt_Address2 = (TextBox)grd_Address.Rows[e.RowIndex].FindControl("txt_Address2");
            TextBox txt_Address3 = (TextBox)grd_Address.Rows[e.RowIndex].FindControl("txt_Address3");
            TextBox txt_Address4 = (TextBox)grd_Address.Rows[e.RowIndex].FindControl("txt_Address4");
            TextBox txt_Address5 = (TextBox)grd_Address.Rows[e.RowIndex].FindControl("txt_Address5");
            TextBox txt_Address6 = (TextBox)grd_Address.Rows[e.RowIndex].FindControl("txt_Address6");
            TextBox txt_Address7 = (TextBox)grd_Address.Rows[e.RowIndex].FindControl("txt_Address7");
            TextBox txt_Address8 = (TextBox)grd_Address.Rows[e.RowIndex].FindControl("txt_Address8");
            TextBox txt_Address9 = (TextBox)grd_Address.Rows[e.RowIndex].FindControl("txt_Address9");
            TextBox txt_Address10 = (TextBox)grd_Address.Rows[e.RowIndex].FindControl("txt_Address10");
            DropDownList ddl_Address1Part = (DropDownList)grd_Address.Rows[e.RowIndex].FindControl("ddl_Address1Part");
            DropDownList ddl_Address2Part = (DropDownList)grd_Address.Rows[e.RowIndex].FindControl("ddl_Address2Part");
            DropDownList ddl_Address3Part = (DropDownList)grd_Address.Rows[e.RowIndex].FindControl("ddl_Address3Part");
            DropDownList ddl_Address4Part = (DropDownList)grd_Address.Rows[e.RowIndex].FindControl("ddl_Address4Part");
            DropDownList ddl_Address5Part = (DropDownList)grd_Address.Rows[e.RowIndex].FindControl("ddl_Address5Part");
            DropDownList ddl_Address6Part = (DropDownList)grd_Address.Rows[e.RowIndex].FindControl("ddl_Address6Part");
            DropDownList ddl_Address7Part = (DropDownList)grd_Address.Rows[e.RowIndex].FindControl("ddl_Address7Part");
            DropDownList ddl_Address8Part = (DropDownList)grd_Address.Rows[e.RowIndex].FindControl("ddl_Address8Part");
            DropDownList ddl_Address9Part = (DropDownList)grd_Address.Rows[e.RowIndex].FindControl("ddl_Address9Part");
            DropDownList ddl_Address10Part = (DropDownList)grd_Address.Rows[e.RowIndex].FindControl("ddl_Address10Part");
            DropDownList ddl_Country = (DropDownList)grd_Address.Rows[e.RowIndex].FindControl("ddl_Country");
            DropDownList ddl_AddressType = (DropDownList)grd_Address.Rows[e.RowIndex].FindControl("ddl_AddressType");


            // Setup primary key(s)
            string profileCode = grd_Address.DataKeys[e.RowIndex].Values[0].ToString();
            int addressID = Convert.ToInt32(grd_Address.DataKeys[e.RowIndex].Values[1]);

            //Update data to dsAddress
            foreach (DataRow drUpdating in dsAddress.Tables[address.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["profileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                       Convert.ToInt32(drUpdating["addressID"]) == addressID)
                    {


                        drUpdating["IsDefault"] = chk_Default.Checked;
                        drUpdating["IsActive"] = chk_Active.Checked;

                        if (ddl_Address1Part.SelectedItem.Value.ToString() == string.Empty)
                        {
                            drUpdating["Address1Part"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Address1Part"] = ddl_Address1Part.SelectedItem.Value.ToString();
                        }

                        if (ddl_Address2Part.SelectedItem.Value.ToString() == string.Empty)
                        {
                            drUpdating["Address2Part"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Address2Part"] = ddl_Address2Part.SelectedItem.Value.ToString();
                        }

                        if (ddl_Address3Part.SelectedItem.Value.ToString() == string.Empty)
                        {
                            drUpdating["Address3Part"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Address3Part"] = ddl_Address3Part.SelectedItem.Value.ToString();
                        }

                        if (ddl_Address4Part.SelectedItem.Value.ToString() == string.Empty)
                        {
                            drUpdating["Address4Part"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Address4Part"] = ddl_Address4Part.SelectedItem.Value.ToString();
                        }

                        if (ddl_Address5Part.SelectedItem.Value.ToString() == string.Empty)
                        {
                            drUpdating["Address5Part"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Address5Part"] = ddl_Address5Part.SelectedItem.Value.ToString();
                        }

                        if (ddl_Address6Part.SelectedItem.Value.ToString() == string.Empty)
                        {
                            drUpdating["Address6Part"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Address6Part"] = ddl_Address6Part.SelectedItem.Value.ToString();
                        }

                        if (ddl_Address7Part.SelectedItem.Value.ToString() == string.Empty)
                        {
                            drUpdating["Address7Part"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Address7Part"] = ddl_Address7Part.SelectedItem.Value.ToString();
                        }

                        if (ddl_Address8Part.SelectedItem.Value.ToString() == string.Empty)
                        {
                            drUpdating["Address8Part"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Address8Part"] = ddl_Address8Part.SelectedItem.Value.ToString();
                        }

                        if (ddl_Address9Part.SelectedItem.Value.ToString() == string.Empty)
                        {
                            drUpdating["Address9Part"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Address9Part"] = ddl_Address9Part.SelectedItem.Value.ToString();
                        }

                        if (ddl_Address10Part.SelectedItem.Value.ToString() == string.Empty)
                        {
                            drUpdating["Address10Part"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Address10Part"] = ddl_Address10Part.SelectedItem.Value.ToString();
                        }

                        if (txt_Street.Text == string.Empty)
                        {
                            drUpdating["Street"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Street"] = txt_Street.Text.Trim();
                        }

                        //if (ddl_Country.SelectedItem.Value.ToString() == string.Empty)
                        if (ddl_Country.SelectedItem == null)
                        {
                            drUpdating["CountryCode"] = DBNull.Value;
                            drUpdating["RegionCode"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["CountryCode"] = ddl_Country.SelectedItem.Value.ToString();
                            drUpdating["RegionCode"] = country.GetRegionCodeByCountryCode(ddl_Country.SelectedItem.Value.ToString(), LoginInfo.ConnStr);
                        }


                        if (ddl_AddressType.SelectedItem.Value.ToString() == string.Empty)
                        {
                            drUpdating["AddressTypeCode"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["AddressTypeCode"] = ddl_AddressType.SelectedItem.Value.ToString();
                        }

                        if (txt_Address1.Text == string.Empty)
                        {
                            drUpdating["Address1"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Address1"] = txt_Address1.Text.Trim();
                        }

                        if (txt_Address2.Text == string.Empty)
                        {
                            drUpdating["Address2"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Address2"] = txt_Address2.Text.Trim();
                        }

                        if (txt_Address3.Text == string.Empty)
                        {
                            drUpdating["Address3"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Address3"] = txt_Address3.Text.Trim();
                        }

                        if (txt_Address4.Text == string.Empty)
                        {
                            drUpdating["Address4"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Address4"] = txt_Address4.Text.Trim();
                        }

                        if (txt_Address5.Text == string.Empty)
                        {
                            drUpdating["Address5"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Address5"] = txt_Address5.Text.Trim();
                        }

                        if (txt_Address6.Text == string.Empty)
                        {
                            drUpdating["Address6"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Address6"] = txt_Address6.Text.Trim();
                        }

                        if (txt_Address7.Text == string.Empty)
                        {
                            drUpdating["Address7"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Address7"] = txt_Address7.Text.Trim();
                        }

                        if (txt_Address8.Text == string.Empty)
                        {
                            drUpdating["Address8"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Address8"] = txt_Address8.Text.Trim();
                        }

                        if (txt_Address9.Text == string.Empty)
                        {
                            drUpdating["Address9"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Address9"] = txt_Address9.Text.Trim();
                        }

                        if (txt_Address10.Text == string.Empty)
                        {
                            drUpdating["Address10"] = DBNull.Value;
                        }
                        else
                        {
                            drUpdating["Address10"] = txt_Address10.Text.Trim();
                        }
                    }
                }
            }


            // Refresh data in GridView
            grd_Address.DataSource = dsAddress.Tables[address.TableName];


            // Refresh data in GridView
            grd_Address.EditIndex = -1;
            grd_Address.DataBind();

            // Save changed to session
            Session["dsAddress"] = dsAddress;
            this.DataSource = dsAddress;

            //}            
        }

        /// <summary>
        /// Click button new
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_New_Click(object sender, EventArgs e)
        {
            // Get 
            DataSet dsNewRow = new DataSet();

            // New row for dataset
            DataRow drNew = dsAddress.Tables[address.TableName].NewRow();

            drNew["ProfileCode"] = ProfileCode.ToString();
            drNew["AddressID"] = this.AddressID + 1;
            drNew["IsActive"] = true;

            drNew["Address1Part"] = 1;
            drNew["Address2Part"] = 2;
            drNew["Address3Part"] = 3;
            drNew["Address4Part"] = 4;
            drNew["Address5Part"] = 5;

            // Add new row
            dsAddress.Tables[address.TableName].Rows.Add(drNew);


            DataRow drAddressCount = dsAddressCount.Tables[address.TableName].NewRow();
            drAddressCount["ProfileCode"] = this.ProfileCode;
            drAddressCount["AddressID"] = Convert.ToInt32(dsAddressCount.Tables[address.TableName].Rows.Count) + 1; //Convert.ToInt32(dsAddressCount.Tables[address.TableName].Rows[0]["AddressID"]) + 1;


            dsAddressCount.Tables[address.TableName].Rows.Add(drAddressCount);

            // Editing on new row
            grd_Address.EditIndex = grd_Address.Rows.Count;
            grd_Address.DataSource = dsAddress.Tables[address.TableName];
            grd_Address.DataBind();

            // Save changed to session
            Session["dsAddress"] = dsAddress;
            Session["dsAddressCount"] = dsAddressCount;
            this.DataSource = dsAddress;
            this.AddressID = Convert.ToInt32(drNew["AddressID"]);
        }

        /// <summary>
        /// Select index change of drop drow list country.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Country_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl_Country = (DropDownList)sender;
            TextBox txt_Country = (TextBox)ddl_Country.Parent.Parent.FindControl("txt_Country");
            txt_Country.Text = country.GetCountryName(ddl_Country.SelectedValue.ToString(), LoginInfo.ConnStr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ddl_AddPart"></param>
        protected void BindingLookup(DropDownList ddl_AddPart)
        {
            ddl_AddPart.DataSource = dtLookupItem;
            ddl_AddPart.DataTextField = "Name";
            ddl_AddPart.DataValueField = "AddressPartCode";
            ddl_AddPart.DataBind();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chk_Default_OnCheckedChanged(object sender, EventArgs e)
        {
            //foreach (GridViewRow drAddress in grd_Address.Rows)
            //{
            //    // ItemName
            //    if (drAddress.FindControl("chk_default") != null)
            //    {
            //       CheckBox chk_default = (CheckBox)drAddress.FindControl("chk_default");

            //       // Determine for gridview selecte row.
            //       GridViewRow grdGridRow = (GridViewRow)chk_default.NamingContainer;

            //        if (grd_Address.Rows[grdGridRow.RowIndex].FindControl("chk_default") !=null)
            //        {
            //            CheckBox chk_default1 = (CheckBox)grd_Address.Rows[grdGridRow.RowIndex].FindControl("chk_default");
            //            if (chk_default1.Checked)
            //            { 

            //            }

            //        }
            //    }
            //}
        }

        #endregion
    }
}