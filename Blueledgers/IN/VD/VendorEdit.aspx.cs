using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;

namespace BlueLedger.PL.IN
{
    public partial class VendorEdit : BasePage
    {
        #region "Attributies"

        private readonly Blue.BL.Profile.Address address = new Blue.BL.Profile.Address();
        private readonly Blue.BL.Profile.AddressPart addressPart = new Blue.BL.Profile.AddressPart();
        private readonly Blue.BL.Profile.AddressType addressType = new Blue.BL.Profile.AddressType();
        private readonly Blue.BL.Profile.BankAccount bankAccount = new Blue.BL.Profile.BankAccount();
        private readonly Blue.BL.Profile.Contact contact = new Blue.BL.Profile.Contact();
        private readonly Blue.BL.Profile.ContactDetail contactDetail = new Blue.BL.Profile.ContactDetail();
        private readonly Blue.BL.Reference.Country country = new Blue.BL.Reference.Country();
        private readonly Blue.BL.Application.Field field = new Blue.BL.Application.Field();
        private readonly Blue.BL.AP.InvoiceDefault invoiceDefault = new Blue.BL.AP.InvoiceDefault();
        private readonly Blue.BL.AP.InvoiceDefaultDetail invoiceDefaultDetail = new Blue.BL.AP.InvoiceDefaultDetail();
        private readonly Blue.BL.AP.PaymentDefault paymentDefault = new Blue.BL.AP.PaymentDefault();
        private readonly Blue.BL.AP.PaymentDefaultAuto paymentDefaultAuto = new Blue.BL.AP.PaymentDefaultAuto();
        private readonly Blue.BL.AP.PaymentDefaultCash paymentDefaultCash = new Blue.BL.AP.PaymentDefaultCash();
        private readonly Blue.BL.AP.PaymentDefaultCheq paymentDefaultCheq = new Blue.BL.AP.PaymentDefaultCheq();
        private readonly Blue.BL.AP.PaymentDefaultCredit paymentDefaultCredit = new Blue.BL.AP.PaymentDefaultCredit();
        private readonly Blue.BL.AP.PaymentDefaultTrans paymentDefaultTrans = new Blue.BL.AP.PaymentDefaultTrans();
        private readonly Blue.BL.Profile.Profile profile = new Blue.BL.Profile.Profile();
        private readonly Blue.BL.Security.User user = new Blue.BL.Security.User();
        private readonly Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();
        private readonly Blue.BL.AP.VendorActiveLog vendorActiveLog = new Blue.BL.AP.VendorActiveLog();
        private readonly Blue.BL.AP.VendorAttachment vendorAttachment = new Blue.BL.AP.VendorAttachment();
        private readonly Blue.BL.AP.VendorCategory vendorCategory = new Blue.BL.AP.VendorCategory();
        private readonly Blue.BL.AP.VendorComment vendorComment = new Blue.BL.AP.VendorComment();
        private readonly Blue.BL.AP.VendorDefaultWHT vendorDefaultWHT = new Blue.BL.AP.VendorDefaultWHT();
        private readonly Blue.BL.AP.VendorMisc vendorMisc = new Blue.BL.AP.VendorMisc();
        private Blue.BL.Reference.Account account = new Blue.BL.Reference.Account();
        private Blue.BL.Bank.Bank bank = new Blue.BL.Bank.Bank();
        private Blue.BL.Bank.BankAccountType bankAccountType = new Blue.BL.Bank.BankAccountType();
        private Blue.BL.Profile.ContactCategory contactCategory = new Blue.BL.Profile.ContactCategory();
        private Blue.BL.Profile.ContactType contactType = new Blue.BL.Profile.ContactType();
        private DataSet dsBankAccount = new DataSet();
        private DataSet dsContactPerson = new DataSet();
        private DataSet dsVendorEdit = new DataSet();
        private DataTable dtVendor = new DataTable();
        private Blue.BL.Reference.PaymentMethod paymentMethod = new Blue.BL.Reference.PaymentMethod();
        private Blue.BL.Security.RolePermission rolePermission = new Blue.BL.Security.RolePermission();

        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();


        private string schema = "AP";
        private string schemaprofile = "Profile";
        private string tableBankAccount = "BankAccount";
        private string tableContact = "Contact";
        private string tableContactDetail = "ContactDetail";
        private string tableVendor = "Vendor";
        private Blue.BL.AP.TransactionType transactionType = new Blue.BL.AP.TransactionType();
        private string vendorName;
        private Blue.BL.Reference.WhtType whtType = new Blue.BL.Reference.WhtType();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            // Check login
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
                Page_Setting();
            }
            else
            {
                dsVendorEdit = (DataSet)Session["dsVendorEdit"];
            }
        }

        /// <summary>
        ///     Get data
        /// </summary>
        private void Page_Retrieve()
        {
            if (Request.QueryString["MODE"].ToUpper() == "EDIT")
            {
                // Get vendor id from HTTP query string
                var VendorCode = (Request.QueryString["ID"]);

                // Vendor  
                vendor.GetVendor(VendorCode, dsVendorEdit, LoginInfo.ConnStr);

                // Profile
                profile.GetProfileStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get vendorMisc.
                //vendorMisc.GetList(dsVendorEdit, VendorCode, LoginInfo.ConnStr);

                // Get bankaccount information
                //bankAccount.GetBankAccountList(dsVendorEdit.Tables[vendor.TableName].Rows[0]["ProfileCode"].ToString(),
                //    dsVendorEdit, LoginInfo.ConnStr);

                // Get contact information.
                contact.GetContactList(dsVendorEdit.Tables[vendor.TableName].Rows[0]["ProfileCode"].ToString(), dsVendorEdit, LoginInfo.ConnStr);

                // Get contact detail structure for before click image button.
                contactDetail.GetContactDetailStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get addresslist.
                address.GetAddressList(dsVendorEdit.Tables[vendor.TableName].Rows[0]["ProfileCode"].ToString(), dsVendorEdit, LoginInfo.ConnStr);

                // Get invoicelist.
                //invoiceDefault.GetInvoiceDefaultList(
                //    dsVendorEdit.Tables[vendor.TableName].Rows[0]["ProfileCode"].ToString(), dsVendorEdit,
                //    LoginInfo.ConnStr);

                // Get invoicedetail structure.
                //invoiceDefaultDetail.GetInvoiceDefaultDetailStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get paymentlist.
                //paymentDefault.GetPaymentDefaultList(
                //    dsVendorEdit.Tables[vendor.TableName].Rows[0]["ProfileCode"].ToString(), dsVendorEdit,
                //    LoginInfo.ConnStr);

                // Get paymentdefaultcash structure.
                //paymentDefaultCash.GetPaymentDefaultCashStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get paymentdefaultcheq structure.
                //paymentDefaultCheq.GetPaymentDefaultCheqStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get paymentdefaultcredit structure.
                //paymentDefaultCredit.GetPaymentDefaultCreditStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get paymentdefaultauto structure.
                //paymentDefaultAuto.GetPaymentDefaultAutoStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get paymentdefaulttrans structure.
                //paymentDefaultTrans.GetPaymentDefaultTransStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get vendordefaultwht structure.
                //vendorDefaultWHT.GetVendorDefaultWHTStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get vendorComment.
                vendorComment.GetVendorCommentListByVendorCode(dsVendorEdit, (Request.QueryString["ID"]),
                    LoginInfo.ConnStr);

                // Get vendorActiveLog.
                vendorActiveLog.GetVendorActLogListByVendorCode(dsVendorEdit, (Request.QueryString["ID"]),
                    LoginInfo.ConnStr);

                // Get vendor attachment.
                vendorAttachment.GetList(dsVendorEdit, (Request.QueryString["ID"]), LoginInfo.ConnStr);
            }
            else
            {
                // Vendor
                vendor.GetVendorStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Profile
                profile.GetProfileStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get vendorMisc.
                //vendorMisc.GetSchema(dsVendorEdit, LoginInfo.ConnStr);

                // Get bankaccount information
                //bankAccount.GetBankAccountStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get contact information.
                contact.GetContactStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get contact detail structure for before click image button.
                contactDetail.GetContactDetailStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get addresslist.
                address.GetAddressStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get invoicelist.
                //invoiceDefault.GetInvoiceDefaultStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get invoicedetail structure.
                //invoiceDefaultDetail.GetInvoiceDefaultDetailStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get paymentlist.
                //paymentDefault.GetPaymentDefaultStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get paymentdefaultcash structure.
                //paymentDefaultCash.GetPaymentDefaultCashStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get paymentdefaultcheq structure.
                //paymentDefaultCheq.GetPaymentDefaultCheqStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get paymentdefaultcredit structure.
                //paymentDefaultCredit.GetPaymentDefaultCreditStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get paymentdefaultauto structure.
                //paymentDefaultAuto.GetPaymentDefaultAutoStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get paymentdefaulttrans structure.
                //paymentDefaultTrans.GetPaymentDefaultTransStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get vendordefaultwht structure.
                //vendorDefaultWHT.GetVendorDefaultWHTStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Get vendorComment.
                vendorComment.GetSchema(dsVendorEdit, LoginInfo.ConnStr);

                // Get vendorActiveLog.
                vendorActiveLog.GetSchema(dsVendorEdit, LoginInfo.ConnStr);

                // Get vendor attachment.
                vendorAttachment.GetStructure(dsVendorEdit, LoginInfo.ConnStr);

                // Add new row in vendor
                var drNew = dsVendorEdit.Tables[vendor.TableName].NewRow();

                drNew["ProfileCode"] = Guid.NewGuid();
                drNew["VendorCode"] = vendor.GetNewVendorCode(LoginInfo.ConnStr);
                drNew["SunVendorCode"] = string.Empty;
                drNew["Name"] = string.Empty;
                drNew["VendorCategoryCode"] = string.Empty;
                drNew["TaxID"] = string.Empty;
                drNew["RegisterNo"] = string.Empty;
                drNew["CreditTerm"] = System.DBNull.Value;
                drNew["DiscountTerm"] = System.DBNull.Value;
                drNew["DiscountRate"] = Convert.ToDecimal("0.00");
                drNew["Description"] = string.Empty;
                drNew["Rating"] = Convert.ToInt16("0");
                drNew["TaxType"] = string.Empty;
                drNew["TaxRate"] = Convert.ToDecimal("0.00");

                drNew["IsActive"] = true;
                drNew["CreatedBy"] = LoginInfo.LoginName;
                drNew["CreatedDate"] = ServerDateTime;
                drNew["UpdatedBy"] = LoginInfo.LoginName;
                drNew["UpdatedDate"] = ServerDateTime;

                // Add new row
                dsVendorEdit.Tables[vendor.TableName].Rows.Add(drNew);

                // Add new profile.
                var drProflileNew = dsVendorEdit.Tables[profile.TableName].NewRow();

                drProflileNew["ProfileCode"] = dsVendorEdit.Tables[vendor.TableName].Rows[0]["ProfileCode"];
                //Guid.NewGuid();
                drProflileNew["IsActive"] = true;
                drProflileNew["CreatedBy"] = LoginInfo.LoginName;
                drProflileNew["CreatedDate"] = ServerDateTime;
                drProflileNew["UpdatedBy"] = LoginInfo.LoginName;
                drProflileNew["UpdatedDate"] = ServerDateTime;

                // Add new row
                dsVendorEdit.Tables[profile.TableName].Rows.Add(drProflileNew);
            }

            // Get addresspart.
            addressPart.GetAddressPartList(dsVendorEdit, LoginInfo.ConnStr);

            // Get country.
            country.GetCountryList(dsVendorEdit, LoginInfo.ConnStr);

            // Get addresstry.
            addressType.GetAddressTypeList(dsVendorEdit, LoginInfo.ConnStr);

            // Store in session
            Session["dsVendorEdit"] = dsVendorEdit;

            // Set to user control address.
            Session["dsAddress"] = dsVendorEdit;

            Session["dsContactList"] = dsVendorEdit;


            //Set to user control attachment.
            Session["dsAttachment"] = dsVendorEdit;
        }

        /// <summary>
        ///     Binding controls
        /// </summary>
        private void Page_Setting()
        {
            // Send data to user control attachment.
            //Attachment.FromFile      = BaseUserControl.AttachFromFile.Vendor;
            //Attachment.Mode          = BaseUserControl.AttachMode.Display;

            // Send data to user control address information.
            AddressInformation.ReadOnly = false;
            AddressInformation.DataSource = dsVendorEdit;
            AddressInformation.DataBind();
            AddressInformation.ProfileCode = (Guid)dsVendorEdit.Tables[vendor.TableName].Rows[0]["ProfileCode"];

            // Bank account information datasource.
            //BankAccount.ReadOnly      = false;
            //BankAccount.DataSource    = dsVendorEdit;
            //BankAccount.DataBind();
            //BankAccount.ProfileCode   = (Guid)dsVendorEdit.Tables[vendor.TableName].Rows[0]["ProfileCode"];

            // Contact person information datasource.
            ContactPerson.ReadOnly = false;
            ContactPerson.DataSource = dsVendorEdit;
            ContactPerson.DataBind();
            ContactPerson.ProfileCode = (Guid)dsVendorEdit.Tables[vendor.TableName].Rows[0]["ProfileCode"];

            // Vendor default invoice information datasource.
            //VendorDefaultInvoice.ReadOnly    = false;
            //VendorDefaultInvoice.DataSource  = dsVendorEdit;
            //VendorDefaultInvoice.DataBind();
            //VendorDefaultInvoice.ProfileCode = (Guid)dsVendorEdit.Tables[vendor.TableName].Rows[0]["ProfileCode"];

            // Vendor default payment information datasource.
            //VendorDefaultPayment.ReadOnly     = false;
            //VendorDefaultPayment.DataSource   = dsVendorEdit;
            //VendorDefaultPayment.DataBind();
            //VendorDefaultPayment.ProfileCode  = (Guid)dsVendorEdit.Tables[vendor.TableName].Rows[0]["ProfileCode"];

            // VendorComment
            //VendorComment.TableName = "AP.VendorComment";
            //VendorComment.RefNo     = dsVendorEdit.Tables[vendor.TableName].Rows[0]["vendorcode"].ToString();
            //VendorComment.DataBind();

            // Get Vendor
            // Show header information.
            var drVendor = dsVendorEdit.Tables[vendor.TableName].Rows[0];

            if (Request.QueryString["MODE"].ToUpper() == "EDIT")
            {
                txt_Vendor.Text = drVendor["VendorCode"].ToString();
            }
            else
            {
                txt_Vendor.Text = string.Empty;
                txt_Vendor.Enabled = false;
            }

            txt_SunVendorCode.Text = drVendor["SunVendorCode"].ToString();

            //txt_Vendor.Visible       = rolePermission.GetIsVisible(LoginInfo.LoginName, schema, tableVendor,"VendorCode",LoginInfo.ConnStr);

            txt_VendorName.Text = drVendor["Name"].ToString();
            //txt_VendorName.Visible   = rolePermission.GetIsVisible(LoginInfo.LoginName,schema,tableVendor,"VendorCode",LoginInfo.ConnStr);

            cmb_VendorCategory.Value = drVendor["VendorCategoryCode"].ToString();

            txt_CreditTerm.Text = drVendor["CreditTerm"].ToString();
            //txt_CreditTerm.Visible   = rolePermission.GetIsVisible(LoginInfo.LoginName,schema,tableVendor,"CreditTerm",LoginInfo.ConnStr);

            txt_TaxID.Text = drVendor["TaxID"].ToString();
            //txt_TaxID.Visible        = rolePermission.GetIsVisible(LoginInfo.LoginName,schema,tableVendor,"TaxID",LoginInfo.ConnStr);

            txt_TaxBranchID.Text = drVendor["TaxBranchID"].ToString();
            //txt_TaxBranchID.Visible        = rolePermission.GetIsVisible(LoginInfo.LoginName,schema,tableVendor,"TaxID",LoginInfo.ConnStr);

            txt_DiscountTerm.Text = drVendor["DiscountTerm"].ToString();
            //txt_DiscountTerm.Visible = rolePermission.GetIsVisible(LoginInfo.LoginName,schema,tableVendor,"DiscountTerm",LoginInfo.ConnStr);


            txt_Reg.Text = drVendor["RegisterNo"].ToString();
            //txt_Reg.Visible          = rolePermission.GetIsVisible(LoginInfo.LoginName,schema,tableVendor,"RegisterNo",LoginInfo.ConnStr);

            txt_DiscountRate.Text = drVendor["DiscountRate"].ToString();
            //txt_DiscountRate.Visible = rolePermission.GetIsVisible(LoginInfo.LoginName,schema,tableVendor,"DiscountRate",LoginInfo.ConnStr);

            if (drVendor["Rating"] != DBNull.Value)
                rc_Rating.Value = Convert.ToDecimal(drVendor["Rating"]);
            //Rat_Rating.Visible       = rolePermission.GetIsVisible(LoginInfo.LoginName,schema,tableVendor,"Rating",LoginInfo.ConnStr);

            txt_Description.Text = drVendor["Description"].ToString();
            //txt_Description.Visible  = rolePermission.GetIsVisible(LoginInfo.LoginName,schema,tableVendor,"Description",LoginInfo.ConnStr);

            ddl_TaxType.SelectedValue = drVendor["TaxType"].ToString();

            txt_TaxRate.Text = (drVendor["TaxRate"].ToString());

            // Status.
            if (drVendor["IsActive"].ToString() != string.Empty)
            {
                if (((bool)drVendor["IsActive"] == false ? false : true))
                {
                    chk_Status.Checked = true;
                }
                else
                {
                    chk_Status.Checked = false;
                }
            }
            else
            {
                chk_Status.Checked = false;
            }

            //ddl_VendorCategory.DataSource     = vendorCategory.GetVendorCategoryLookup(LoginInfo.ConnStr);
            //ddl_VendorCategory.DataValueField = "VendorCategoryCode";
            //ddl_VendorCategory.DataTextField  = "Name";
            //ddl_VendorCategory.SelectedValue  = drVendor["VendorCategoryCode"].ToString();
            //ddl_VendorCategory.DataBind();

            //ddl_VendorCategory1.DataSource    = vendorCategory.GetVendorCategoryLookup(LoginInfo.ConnStr);
            //ddl_VendorCategory1.TextField     = "Name";
            //ddl_VendorCategory1.ValueField    = "VendorCategoryCode";
            //ddl_VendorCategory1.DataBind();


            // Setting Vendor Misc
            //EditAdditionalInfo.DataSource   = dsVendorEdit.Tables[vendorMisc.TableName];
            //EditAdditionalInfo.SchemaName   = "AP";
            //EditAdditionalInfo.PrimaryTable = "vVendor";
            //EditAdditionalInfo.DataBind();

            // Vendor ActiveLog.
            //grd_ActiveLog.DataSource = dsVendorEdit.Tables[vendorActiveLog.TableName];
            //grd_ActiveLog.DataBind();
        }

        /// <summary>
        ///     Menu Click Event.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":
                    Page.Validate();
                    if (Page.IsValid)
                    {
                        var saved = Save();

                        if (saved)
                        {
                            Response.Redirect("Vendor.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&ID=" +
                                              dsVendorEdit.Tables[vendor.TableName].Rows[0]["VendorCode"]);
                        }
                    }

                    break;

                case "BACK":
                    Back();
                    break;
            }
        }

        /// <summary>
        ///     Save Vendor
        /// </summary>
        /// <returns></returns>
        private bool Save()
        {
            string id = string.Empty;
            string _action = string.Empty;

            // Set default zero value

            if (string.IsNullOrEmpty(txt_TaxRate.Text))
            {
                txt_TaxRate.Text = "0.00";
            }

            if (string.IsNullOrEmpty(txt_DiscountTerm.Text))
            {
                txt_DiscountTerm.Text = "0";
            }

            if (string.IsNullOrEmpty(txt_DiscountRate.Text))
            {
                txt_DiscountRate.Text = "0.00";
            }

            if (string.IsNullOrEmpty(txt_CreditTerm.Text))
            {
                txt_CreditTerm.Text = "0";
            }

            //Check address information before save. Require at least one.
            if (AddressInformation.DataSource.Tables[address.TableName].Rows.Count <= 0)
            {
                lbl_Warning.Text = "Address information is required.";
                pop_Warning.ShowOnPageLoad = true;
                return false;
            }

            // Prepare data for save
            // Vendors' header information.
            var drVendor = dsVendorEdit.Tables[vendor.TableName].Rows[0];

            if (Request.QueryString["MODE"].ToUpper() == "EDIT")
            {
                _action = "MODIFY";

                if (GetIsVisible(tableVendor, "VendorCode"))
                {
                    drVendor["VendorCode"] = txt_Vendor.Text.Trim();
                }
            }
            else
            {
                _action = "CREATE";

                if (GetIsVisible(tableVendor, "VendorCode"))
                {
                    string vendorName = txt_VendorName.Text.Trim();
                    string vendorCode = vendor.GetNewVendorCodeByName(vendorName, LoginInfo.ConnStr);
                    if (vendorCode == string.Empty)
                    {
                        lbl_Warning.Text = "Cannot create vendor code";
                        pop_Warning.ShowOnPageLoad = true;
                        return false;
                    }
                    else
                        //drVendor["VendorCode"] = vendor.GetNewVendorCode(LoginInfo.ConnStr);
                        drVendor["VendorCode"] = vendorCode;
                }
            }

            id = drVendor["VendorCode"].ToString();

            drVendor["SunVendorCode"] = txt_SunVendorCode.Text.Trim();

            if (GetIsVisible(tableVendor, "Name"))
            {
                drVendor["Name"] = txt_VendorName.Text.Trim();
            }

            if (GetIsVisible(tableVendor, "VendorCategoryCode"))
            {
                drVendor["VendorCategoryCode"] = cmb_VendorCategory.SelectedItem.Value.ToString();
            }

            if (GetIsVisible(tableVendor, "TaxID"))
            {
                // Date time format.
                drVendor["TaxID"] = (txt_TaxID.Text.Trim());
            }

            if (GetIsVisible(tableVendor, "TaxBranchID"))
            {
                // Date time format.
                drVendor["TaxBranchID"] = (txt_TaxBranchID.Text.Trim());
            }

            if (GetIsVisible(tableVendor, "RegisterNo"))
            {
                drVendor["RegisterNo"] = txt_Reg.Text.Trim();
            }

            if (GetIsVisible(tableVendor, "CreditTerm"))
            {
                drVendor["CreditTerm"] = (txt_CreditTerm.Text == string.Empty
                    ? 0
                    : Convert.ToInt32(txt_CreditTerm.Text));
            }

            if (GetIsVisible(tableVendor, "DiscountTerm"))
            {
                drVendor["DiscountTerm"] = (txt_DiscountTerm.Text == string.Empty
                    ? 0
                    : Convert.ToInt32(txt_DiscountTerm.Text));
            }

            if (GetIsVisible(tableVendor, "DiscountRate"))
            {
                drVendor["DiscountRate"] = (txt_DiscountRate.Text == string.Empty
                    ? 0
                    : Convert.ToDecimal(txt_DiscountRate.Text));
            }

            if (GetIsVisible(tableVendor, "Description"))
            {
                drVendor["Description"] = txt_Description.Text;

            }

            if (GetIsVisible(tableVendor, "Rating"))
            {
                drVendor["Rating"] = rc_Rating.Value;
            }

            if (GetIsVisible(tableVendor, "IsActive"))
            {
                drVendor["IsActive"] = chk_Status.Checked.ToString();
            }

            drVendor["TaxType"] = ddl_TaxType.SelectedItem.Value;


            if (!string.IsNullOrEmpty(txt_TaxRate.Text) && ddl_TaxType.SelectedItem.Value == "N")
            {
                drVendor["TaxRate"] = 0;
            }
            else if (txt_TaxRate.Text != "0.00" && ddl_TaxType.SelectedItem.Value != "N")
            {
                drVendor["TaxRate"] = Convert.ToDecimal(txt_TaxRate.Text);
            }
            else
            {
                pop_AlertTaxRate.ShowOnPageLoad = true;
                return false;
            }

            drVendor["CreatedDate"] = ServerDateTime;
            drVendor["CreatedBy"] = LoginInfo.LoginName;
            drVendor["UpdatedDate"] = ServerDateTime;
            drVendor["UpdatedBy"] = LoginInfo.LoginName;



            // Address.
            dsVendorEdit.Tables[address.TableName].Merge(AddressInformation.DataSource.Tables[address.TableName]);

            // BankAccount.
            //dsVendorEdit.Tables[bankAccount.TableName].Merge(BankAccount.DataSource.Tables[bankAccount.TableName]);

            // Contact.
            dsVendorEdit.Tables[contact.TableName].Merge(ContactPerson.DataSource.Tables[contact.TableName]);

            // ContactDetail.
            dsVendorEdit.Tables[contactDetail.TableName].Merge(
                ContactPerson.DataSource.Tables[contactDetail.TableName]);



            // Save data
            var result = vendor.Save(dsVendorEdit, LoginInfo.ConnStr);

            if (result)
            {
                _transLog.Save("IN", "VD", id, _action, string.Empty, LoginInfo.LoginName, LoginInfo.ConnStr);


                // New row effect data keep by session.
                Session["dsVendor"] = dsVendorEdit;
                return true;
            }
            else
            {
                // Display error
                MessageBox("Error on saving!");
                return false;
            }
        }

        /// <summary>
        ///     Validation for discount rate
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void ValidateDiscountRate(object source, ServerValidateEventArgs args)
        {
            if (txt_DiscountTerm.Text != string.Empty)
            {
                var discountTerm = Convert.ToInt32(txt_DiscountTerm.Text);
                if (discountTerm > 0)
                {
                    try
                    {
                        if (args.Value.Contains("."))
                        {
                            var num = Convert.ToInt32(args.Value.Substring(0, args.Value.IndexOf('.', 0)));
                            args.IsValid = (num > 0);
                        }
                        else
                        {
                            var num = Convert.ToInt32(args.Value);
                            args.IsValid = (num > 0);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.Error(ex);
                        args.IsValid = false;
                    }
                }
            }
        }

        /// <summary>
        ///     Back to Vedor or Vendor List
        /// </summary>
        private void Back()
        {
            // Clear session
            Session.Remove("dsVendorEdit");

            if (Request.QueryString["MODE"].ToUpper() == "EDIT")
            {
                Response.Redirect("Vendor.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + Request.QueryString["ID"]);
            }
            else
            {
                Response.Redirect("VendorList.aspx");
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            cmb_VendorCategory.ValueField = "VendorCategoryCode";
            cmb_VendorCategory.TextField = "Name";
            cmb_VendorCategory.TextFormatString = "{0}";
            cmb_VendorCategory.DataSource = vendorCategory.GetVendorCategoryLookup(LoginInfo.ConnStr);
            cmb_VendorCategory.DataBind();
        }

        /// <summary>
        ///     Get visibility permission for specified field from RolePermission table.
        /// </summary>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        private bool GetIsVisible(string TableName, string FieldName)
        {
            return true;
            //rolePermission.GetIsVisible(LoginInfo.LoginName, schema, TableName, FieldName, LoginInfo.ConnStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="FieldID"></param>
        /// <param name="Value"></param>
        private void SetVendorMiscValue(string FieldID, string Value)
        {
            var rowExist = false;

            foreach (DataRow drVendorMisc in dsVendorEdit.Tables[vendorMisc.TableName].Rows)
            {
                if (drVendorMisc["FieldID"].ToString().ToUpper() == FieldID.ToUpper())
                {
                    if (GetIsVisible(tableVendor, field.GetFieldName(FieldID, LoginInfo.ConnStr)))
                    {
                        drVendorMisc["Value"] = Value;
                    }

                    rowExist = true;
                    break;
                }
            }

            // If have no row related with specified FieldID. Add new one.
            if (!rowExist)
            {
                var drVendorMisc = dsVendorEdit.Tables[vendorMisc.TableName].NewRow();
                drVendorMisc["ProfileCode"] = dsVendorEdit.Tables[vendor.TableName].Rows[0]["ProfileCode"];
                drVendorMisc["VendorCode"] = dsVendorEdit.Tables[vendor.TableName].Rows[0]["VendorCode"];
                drVendorMisc["FieldID"] = FieldID;
                drVendorMisc["Value"] = Value;

                dsVendorEdit.Tables[vendorMisc.TableName].Rows.Add(drVendorMisc);
            }
        }

        /// <summary>
        ///     Vendor delte process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            var result = false;

            // Prepare delete for vendor detail.
            dsVendorEdit.Tables[vendor.TableName].Rows[0].Delete();

            for (var i = dsVendorEdit.Tables[bankAccount.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drBankAccount = dsVendorEdit.Tables[bankAccount.TableName].Rows[i];

                if (drBankAccount.RowState != DataRowState.Deleted)
                {
                    drBankAccount.Delete();
                }
            }

            // Prepare delete for vendorMisc/additional process.
            for (var i = dsVendorEdit.Tables[vendorMisc.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drvendorMisc = dsVendorEdit.Tables[vendorMisc.TableName].Rows[i];

                if (drvendorMisc.RowState != DataRowState.Deleted)
                {
                    drvendorMisc.Delete();
                }
            }

            // Delete data from database
            result = vendor.Delete(dsVendorEdit, LoginInfo.ConnStr);

            if (result)
            {
                // Remove session
                Session.Remove("dsVendorEdit");

                Response.Redirect("VendorList.aspx");
            }
            else
            {
                // Display error message
                MessageBox("You can not delete this row because reference from another process !");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Country_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Find gridview row.
            var gr = (GridViewRow)((DropDownList)sender).Parent.Parent;

            // Find the control in gridView.
            var countryCode = string.Empty;
            var txt_Country = (TextBox)gr.FindControl("txt_Country");
            var ddl_Country = (DropDownList)gr.FindControl("ddl_Country");
            countryCode = (ddl_Country.SelectedItem == null ? string.Empty : ddl_Country.SelectedItem.Value);

            // Get country name after dropdown change.
            txt_Country.Text = country.GetCountryName(countryCode, LoginInfo.ConnStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_AddressType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var gr = (GridViewRow)((DropDownList)sender).Parent.Parent;

            // Find the control in gridView.
            var addressTypeCode = string.Empty;
            var txt_AddressType = (TextBox)gr.FindControl("txt_AddressType");
            var ddl_AddressType = (DropDownList)gr.FindControl("ddl_AddressType");
            addressTypeCode = (ddl_AddressType.SelectedItem == null ? string.Empty : ddl_AddressType.SelectedItem.Value);

            // Get country name after dropdown change.
            txt_AddressType.Text = addressType.GetAddressTypeName(addressTypeCode, LoginInfo.ConnStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Aging_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_RelatedInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_RelatedPayment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        /// <summary>
        ///     Get total DrAmt
        /// </summary>
        /// <returns></returns>
        private decimal GetTotalDrAmt()
        {
            decimal drAmt = 0;

            for (var i = 0; i < dsVendorEdit.Tables[bankAccount.TableName].Rows.Count; i++)
            {
                var drBankAccount = dsVendorEdit.Tables[bankAccount.TableName].Rows[i];

                if (drBankAccount.RowState != DataRowState.Deleted)
                {
                    drAmt += (drBankAccount["DrAmt"] == DBNull.Value ? 0 : (decimal)drBankAccount["DrAmt"]);
                }
            }

            return drAmt;
        }

        /// <summary>
        ///     Get total CrAmt
        /// </summary>
        /// <returns></returns>
        private decimal GetTotalCrAmt()
        {
            decimal crAmt = 0;

            for (var i = 0; i < dsVendorEdit.Tables[bankAccount.TableName].Rows.Count; i++)
            {
                var drBankAccount = dsVendorEdit.Tables[bankAccount.TableName].Rows[i];

                if (drBankAccount.RowState != DataRowState.Deleted)
                {
                    crAmt += (drBankAccount["CrAmt"] == DBNull.Value ? 0 : (decimal)drBankAccount["CrAmt"]);
                }
            }

            return crAmt;
        }

        /// <summary>
        ///     Get total DrPercentage
        /// </summary>
        /// <returns></returns>
        private decimal GetTotalDrPercentage()
        {
            decimal drPercentage = 0;

            for (var i = 0; i < dsVendorEdit.Tables[bankAccount.TableName].Rows.Count; i++)
            {
                var drBankAccount = dsVendorEdit.Tables[bankAccount.TableName].Rows[i];

                if (drBankAccount.RowState != DataRowState.Deleted)
                {
                    drPercentage += (drBankAccount["DrPercentage"] == DBNull.Value
                        ? 0
                        : (decimal)drBankAccount["DrPercentage"]);
                }
            }

            return drPercentage;
        }

        /// <summary>
        ///     Get total CrPercentage
        /// </summary>
        /// <returns></returns>
        private decimal GetTotalCrPercentage()
        {
            decimal crPercentage = 0;

            for (var i = 0; i < dsVendorEdit.Tables[bankAccount.TableName].Rows.Count; i++)
            {
                var drBankAccount = dsVendorEdit.Tables[bankAccount.TableName].Rows[i];

                if (drBankAccount.RowState != DataRowState.Deleted)
                {
                    crPercentage += (drBankAccount["CrPercentage"] == DBNull.Value
                        ? 0
                        : (decimal)drBankAccount["CrPercentage"]);
                }
            }

            return crPercentage;
        }

        /// <summary>
        ///     Error message
        /// </summary>
        /// <param name="msg"></param>
        private void MessageBox(string msg)
        {
            var lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Edit_Click(object sender, EventArgs e)
        {
            // Get vendor id from HTTP query string
            var VendorCode = int.Parse(Request.QueryString["VendorCode"]);

            Response.Redirect("VendorEdit.aspx?MODE=edit&VendorCode=" + VendorCode);
        }

        /// <summary>
        ///     RowDataBound Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ActiveLog_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // DataRow
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onMouseOver", "GridView_Row_MouseOver(this);");
                e.Row.Attributes.Add("onMouseOut", "GridView_Row_MouseOut(this);");
                e.Row.Style.Add("cursor", "hand");


                // Activity
                if (e.Row.FindControl("lbl_Activity") != null)
                {
                    var lbl_Activity = (Label)e.Row.FindControl("lbl_Activity");
                    lbl_Activity.Text = (DataBinder.Eval(e.Row.DataItem, "Activity").ToString() == string.Empty
                        ? string.Empty
                        : DataBinder.Eval(e.Row.DataItem, "Activity").ToString());
                }

                // Date
                if (e.Row.FindControl("lbl_Date") != null)
                {
                    var lbl_Date = (Label)e.Row.FindControl("lbl_Date");
                    lbl_Date.Text = ((DateTime)DataBinder.Eval(e.Row.DataItem, "CreatedDate")).ToString("dd/MM/yyyy");
                }

                // Activator
                if (e.Row.FindControl("lbl_Activator") != null)
                {
                    var lbl_Activator = (Label)e.Row.FindControl("lbl_Activator");
                    lbl_Activator.Text = user.GetUserName((int)DataBinder.Eval(e.Row.DataItem, "CreatedBy"),
                        LoginInfo.ConnStr);
                }
            }
        }

        protected void cmb_VendorCategory_OnItemsRequestedByFilterCondition(object source,
            ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            //ASPxComboBox comboBox = (ASPxComboBox)source;

            //string filter = string.Format("%{0}%", e.Filter);
            //int startIndex = int.Parse((e.BeginIndex + 1).ToString());
            //int endIndex = int.Parse((e.EndIndex + 1).ToString());

            //DataTable dtVendorCategory = vendorCategory.GetListByRowFilter(filter, startIndex, endIndex, LoginInfo.ConnStr);


            //comboBox.DataSource = dtVendorCategory;
            //comboBox.DataBind();
        }

        protected void cmb_VendorCategory_OnItemRequestedByValue(object source, ListEditItemRequestedByValueEventArgs e)
        {
            //ASPxComboBox comboBox = (ASPxComboBox)source;

            //comboBox.DataSource = vendorCategory.GetVendorCategoryLookup(LoginInfo.ConnStr);
            //comboBox.DataBind();
        }

        #endregion

        protected void btn_OK_Click(object sender, EventArgs e)
        {
            pop_AlertTaxRate.ShowOnPageLoad = false;
        }

        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
        }
    }
}