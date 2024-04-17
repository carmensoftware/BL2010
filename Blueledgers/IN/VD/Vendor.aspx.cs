using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN
{
    public partial class Vendor : BasePage
    {
        #region "Attributies"

        private readonly Blue.BL.Profile.Address address = new Blue.BL.Profile.Address();
        private readonly Blue.BL.Profile.BankAccount bankAccount = new Blue.BL.Profile.BankAccount();
        private readonly Blue.BL.Profile.Contact contact = new Blue.BL.Profile.Contact();
        private readonly Blue.BL.Profile.ContactDetail contactDetail = new Blue.BL.Profile.ContactDetail();
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
        private Blue.BL.Profile.AddressPart addressPart = new Blue.BL.Profile.AddressPart();
        private Blue.BL.Profile.AddressType addressType = new Blue.BL.Profile.AddressType();
        private Blue.BL.Bank.Bank bank = new Blue.BL.Bank.Bank();
        private Blue.BL.Bank.BankAccountType bankAccountType = new Blue.BL.Bank.BankAccountType();
        private Blue.BL.Profile.ContactCategory contactCategory = new Blue.BL.Profile.ContactCategory();
        private Blue.BL.Profile.ContactType contactType = new Blue.BL.Profile.ContactType();
        private Blue.BL.Reference.Country country = new Blue.BL.Reference.Country();
        private DataSet dsContactDetail = new DataSet();
        private DataSet dsInvoiceDetail = new DataSet();
        private DataSet dsPaymentDetail = new DataSet();
        private DataSet dsVendor = new DataSet();
        private DataTable dtVendor = new DataTable();
        private Blue.BL.Reference.PaymentMethod paymentMethod = new Blue.BL.Reference.PaymentMethod();
        private Blue.BL.Security.RolePermission rolePermission = new Blue.BL.Security.RolePermission();

        private string schema = "AP";
        private string tableBankAccount = "BankAccount";
        private string tableVendor = "Vendor";
        private Blue.BL.AP.TransactionType transactionType = new Blue.BL.AP.TransactionType();
        private string vendorName;
        private Blue.BL.Reference.WhtType whtType = new Blue.BL.Reference.WhtType();

        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly string moduleID = "2.5";


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
                dsVendor = (DataSet)Session["dsVendor"];
                dsContactDetail = (DataSet)Session["dsContactDetail"];
                dsInvoiceDetail = (DataSet)Session["dsInvoiceDetail"];
                dsPaymentDetail = (DataSet)Session["dsPaymentDetail"];
            }

            ValidatePermission();
        }

        /// <summary>
        ///     Get data
        /// </summary>
        private void Page_Retrieve()
        {
            if (Request.QueryString["ID"] != null)
            {
                // Get vendor id from HTTP query string
                var VendorCode = (Request.QueryString["ID"]);


                // Vendor  
                vendor.GetVendor(VendorCode, dsVendor, LoginInfo.ConnStr);


                // Get vendorMisc.
                vendorMisc.GetList(dsVendor, VendorCode, LoginInfo.ConnStr);


                // Get bankaccount information
                bankAccount.GetBankAccountList(dsVendor.Tables[vendor.TableName].Rows[0]["ProfileCode"].ToString(),
                    dsVendor, LoginInfo.ConnStr);

                // Get contact information.
                contact.GetContactList(dsVendor.Tables[vendor.TableName].Rows[0]["ProfileCode"].ToString(), dsVendor,
                    LoginInfo.ConnStr);

                // Get contact detail structure for before click image button.
                //contactDetail.GetContactDetailStructure(dsVendor, LoginInfo.ConnStr);
                contactDetail.GetContactDetailList(dsVendor, LoginInfo.ConnStr);

                // Get addresslist.
                address.GetAddressList(dsVendor.Tables[vendor.TableName].Rows[0]["ProfileCode"].ToString(), dsVendor,
                    LoginInfo.ConnStr);

                // Get invoicelist.
                invoiceDefault.GetInvoiceDefaultList(
                    dsVendor.Tables[vendor.TableName].Rows[0]["ProfileCode"].ToString(), dsVendor, LoginInfo.ConnStr);

                // Get invoicedefault detail structure for before click image button.
                invoiceDefaultDetail.GetInvoiceDefaultDetailStructure(dsVendor, LoginInfo.ConnStr);

                // Get paymentlist.
                paymentDefault.GetPaymentDefaultList(
                    dsVendor.Tables[vendor.TableName].Rows[0]["ProfileCode"].ToString(), dsVendor, LoginInfo.ConnStr);

                // Get paymentdefaultcash structure.
                paymentDefaultCash.GetPaymentDefaultCashStructure(dsVendor, LoginInfo.ConnStr);

                // Get paymentdefaultcheq structure.
                paymentDefaultCheq.GetPaymentDefaultCheqStructure(dsVendor, LoginInfo.ConnStr);

                // Get paymentdefaultcredit structure.
                paymentDefaultCredit.GetPaymentDefaultCreditStructure(dsVendor, LoginInfo.ConnStr);

                // Get paymentdefaultauto structure.
                paymentDefaultAuto.GetPaymentDefaultAutoStructure(dsVendor, LoginInfo.ConnStr);

                // Get paymentdefaulttrans structure.
                paymentDefaultTrans.GetPaymentDefaultTransStructure(dsVendor, LoginInfo.ConnStr);

                // Get vendordefaultwht structure.
                vendorDefaultWHT.GetVendorDefaultWHTStructure(dsVendor, LoginInfo.ConnStr);


                // Get vendorComment.
                vendorComment.GetVendorCommentListByVendorCode(dsVendor, (Request.QueryString["ID"]), LoginInfo.ConnStr);

                // Get vendorActiveLog.
                vendorActiveLog.GetVendorActLogListByVendorCode(dsVendor, (Request.QueryString["ID"]), LoginInfo.ConnStr);

                // Get vendor attachment.
                vendorAttachment.GetList(dsVendor, (Request.QueryString["ID"]), LoginInfo.ConnStr);
            }
            else
            {
                dsVendor = (DataSet)Session["dsVendor"];
            }

            // Store in session
            Session["dsVendor"] = dsVendor;

            //Set to user control attachment.
            Session["dsAttachment"] = dsVendor;
        }

        /// <summary>
        ///     Binding controls
        /// </summary>
        private void Page_Setting()
        {
            // Send data to user control attachment.
            //Attachment.FromFile = BaseUserControl.AttachFromFile.Vendor;
            //Attachment.Mode = BaseUserControl.AttachMode.Display;

            // Send data to user control address information.
            AddressInformation.ReadOnly = true;
            AddressInformation.ProfileCode = (Guid)dsVendor.Tables[vendor.TableName].Rows[0]["ProfileCode"];

            // Bank account information datasource.
            //BankAccount.DataSource = dsVendor;
            //BankAccount.DataBind();
            //BankAccount.ReadOnly = true;

            // Contact person information datasource.
            //ContactPerson.DataSource = dsVendor;
            //ContactPerson.DataBind();
            ContactPerson.ProfileCode = (Guid)dsVendor.Tables[vendor.TableName].Rows[0]["ProfileCode"];
            ContactPerson.ReadOnly = true;

            // VendorDefaultInvoice information datasource.
            //VendorDefaultInvoice.DataSource = dsVendor;
            //VendorDefaultInvoice.DataBind();
            //VendorDefaultInvoice.ReadOnly = true;

            // Contact person information datasource.
            //VendorDefaultPayment.DataSource  = dsVendor;
            //VendorDefaultPayment.DataBind();
            //VendorDefaultPayment.ReadOnly    = true;
            //VendorDefaultPayment.ProfileCode = (Guid)dsVendor.Tables[vendor.TableName].Rows[0]["ProfileCode"];

            // VendorComment
            //VendorComment.TableName = "AP.VendorComment";
            //VendorComment.RefNo     = dsVendor.Tables[vendor.TableName].Rows[0]["vendorcode"].ToString();
            //VendorComment.DataBind();

            // Get Vendor
            // Show header information.
            var drVendor = dsVendor.Tables[vendor.TableName].Rows[0];

            lbl_Vendor.Text = drVendor["VendorCode"].ToString();
            //lbl_Vendor.Visible = rolePermission.GetIsVisible(LoginInfo.LoginName, schema, tableVendor, "VendorCode", LoginInfo.ConnStr);

            lbl_SunVendorCode.Text = drVendor["SunVendorCode"].ToString();

            lbl_VendorName.Text = drVendor["Name"].ToString();
            //lbl_VendorName.Visible = rolePermission.GetIsVisible(LoginInfo.LoginName, schema, tableVendor, "Name", LoginInfo.ConnStr);


            lbl_Category.Text = drVendor["VendorCategoryCode"].ToString();
            //lbl_Category.Visible = rolePermission.GetIsVisible(LoginInfo.LoginName, schema, tableVendor, "VendorCategoryCode", LoginInfo.ConnStr);

            lbl_CategoryName.Text = vendorCategory.GetVendorCategoryName(drVendor["VendorCategoryCode"].ToString(),
                LoginInfo.ConnStr);
            //lbl_CategoryName.Visible = rolePermission.GetIsVisible(LoginInfo.LoginName, schema, tableVendor, "VendorCategoryCode", LoginInfo.ConnStr);

            lbl_CreditTerm.Text = drVendor["CreditTerm"].ToString();
            //lbl_CreditTerm.Visible = rolePermission.GetIsVisible(LoginInfo.LoginName, schema, tableVendor, "CreditTerm", LoginInfo.ConnStr);

            lbl_TaxID.Text = drVendor["TaxID"].ToString();
            //lbl_TaxID.Visible = rolePermission.GetIsVisible(LoginInfo.LoginName, schema, tableVendor, "TaxID", LoginInfo.ConnStr);

            lbl_TaxBranchID.Text = drVendor["TaxBranchID"].ToString();
            //lbl_TaxBranchID.Visible = rolePermission.GetIsVisible(LoginInfo.LoginName, schema, tableVendor, "TaxBranchID", LoginInfo.ConnStr);

            lbl_DiscountTerm.Text = drVendor["DiscountTerm"].ToString();
            //lbl_DiscountTerm.Visible = rolePermission.GetIsVisible(LoginInfo.LoginName, schema, tableVendor, "DiscountTerm", LoginInfo.ConnStr);


            lbl_Reg.Text = drVendor["RegisterNo"].ToString();
            //lbl_Reg.Visible = rolePermission.GetIsVisible(LoginInfo.LoginName, schema, tableVendor, "RegisterNo", LoginInfo.ConnStr);

            lbl_DiscountRate.Text = drVendor["DiscountRate"].ToString();
            //lbl_DiscountRate.Visible = rolePermission.GetIsVisible(LoginInfo.LoginName, schema, tableVendor, "DiscountRate", LoginInfo.ConnStr);


            //Rat_Rating.CurrentRating = Convert.ToInt32(drVendor["Rating"]);
            if (drVendor["Rating"] != DBNull.Value)
                rc_Rating.Value = Convert.ToDecimal(drVendor["Rating"]);
            //Rat_Rating.Visible = rolePermission.GetIsVisible(LoginInfo.LoginName, schema, tableVendor, "Rating", LoginInfo.ConnStr);


            lbl_Description.Text = drVendor["Description"].ToString();
            //lbl_Description.Visible = rolePermission.GetIsVisible(LoginInfo.LoginName, schema, tableVendor, "Description", LoginInfo.ConnStr);


            // Status.
            //img_Status.ImageUrl = ((bool)drVendor["IsActive"] == false ? "/" + appName + "/App_Themes/default/pics/red_light_icon.png" : "/" + appName + "/App_Themes/default/pics/green_light_icon.png");
            if (drVendor["IsActive"].ToString() == string.Empty)
            {
                drVendor["IsActive"] = false;
            }

            img_Status.ImageUrl = ((bool)drVendor["IsActive"] == false
                ? "~/App_Themes/default/pics/red_light_icon.png"
                : "~/App_Themes/default/pics/green_light_icon.png");

            if (drVendor["TAXType"].ToString() == "N")
            {
                lbl_TaxType.Text = "None";
            }
            else if (drVendor["TAXType"].ToString() == "I")
            {
                lbl_TaxType.Text = "Included";
            }
            else if (drVendor["TAXType"].ToString() == "A")
            {
                lbl_TaxType.Text = "Add";
            }

            lbl_TaxRate.Text = drVendor["TaxRate"].ToString();

            // Setting Vendor Misc
            //DispAdditionalInfo.DataSource = dsVendor.Tables[vendorMisc.TableName];
            //DispAdditionalInfo.SchemaName = "AP";
            //DispAdditionalInfo.PrimaryTable = "Vendor";
            //DispAdditionalInfo.DataBind();


            // aging information.
            //grd_Aging.DataSource = dsVendor.Tables[contact.TableName];
            //grd_Aging.DataBind();

            // related invoice.
            //grd_RelatedInvoice.DataSource = null;
            //grd_RelatedInvoice.DataBind();

            // related payment.
            //grd_RelatedPayment.DataSource = null;
            //grd_RelatedPayment.DataBind();


            // Vendor ActiveLog.
            //grd_ActiveLog.DataSource = dsVendor.Tables[vendorActiveLog.TableName];
            //grd_ActiveLog.DataBind();

            // Display Comment.
            //PL.UserControls.Comment comment = (PL.UserControls.Comment)((BlueLedger.PL.Master.Pc.Blue)this.Master).FindControl("Comment");
            //comment.Module = "IN";
            //comment.SubModule = "VD";
            //comment.RefNo = dsVendor.Tables[vendor.TableName].Rows[0]["VendorCode"].ToString();
            //comment.Visible = true;
            //comment.DataBind();            

            // Display Comment.           
            var comment = (PL.UserControls.Comment2)Master.FindControl("Comment");
            comment.Module = "IN";
            comment.SubModule = "VD";
            comment.RefNo = dsVendor.Tables[vendor.TableName].Rows[0]["VendorCode"].ToString();
            comment.Visible = true;
            comment.DataBind();

            // Display Attach
            var attach = (PL.UserControls.Attach2)Master.FindControl("Attach");
            attach.ModuleName = "IN";
            attach.RefNo = dsVendor.Tables[vendor.TableName].Rows[0]["VendorCode"].ToString();
            attach.BuCode = Request.Params["BuCode"];
            attach.Visible = true;
            attach.DataBind();

            // Display Log
            var log = (PL.UserControls.Log2)Master.FindControl("Log");
            log.Module = "IN";
            log.SubModule = "VD";
            log.RefNo = dsVendor.Tables[vendor.TableName].Rows[0]["VendorCode"].ToString();
            log.Visible = true;
            log.DataBind();
        }


        protected void ValidatePermission()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);

            if (pagePermiss == 0)
                Response.Redirect("../../Option/User/Default.aspx");

            menu_CmdBar.Items.FindByName("Create").Visible = (pagePermiss >= 3);
            menu_CmdBar.Items.FindByName("Edit").Visible = (pagePermiss >= 3);
            menu_CmdBar.Items.FindByName("Delete").Visible = (pagePermiss >= 7);


        }

        /// <summary>
        ///     Menu Click Event
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Create();
                    break;

                case "EDIT":
                    Edit();
                    break;

                case "DELETE":

                    //menu_CmdBar.Attributes.Add("OnClick", "javascript:return Confirm();");
                    //e.Item.Menu.Attributes.Add("OnClick", "javascript:return Confirm();");
                    //bool deleted = this.Delete();

                    //if (deleted)
                    //{
                    //    Response.Redirect("VendorList.aspx");
                    //}
                    //break;

                    Delete();
                    break;

                case "PRINT":
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    //// Send Vendor  to Session            
                    //var objArrList = new ArrayList();
                    //objArrList.Add("'" + Request.Params["ID"] + "'");
                    //Session["s_arrNo"] = objArrList;

                    //var reportLink = "../../RPT/ReportCriteria.aspx?category=001&reportid=131" + "&BuCode=" +
                    //                 Request.Params["BuCode"];
                    //ClientScript.RegisterStartupScript(GetType(), "newWindow",
                    //    "<script>window.open('" + reportLink + "','_blank')</script>");
                    break;

                case "BACK":
                    Back();
                    break;
            }
        }

        /// <summary>
        ///     Create New Vendor
        /// </summary>
        private void Create()
        {
            Response.Redirect("VendorEdit.aspx?MODE=new");
        }

        /// <summary>
        ///     Edit Vendor
        /// </summary>
        private void Edit()
        {
            Response.Redirect("VendorEdit.aspx?BuCode=" + Request.Params["BuCode"] + "&MODE=edit&ID=" +
                              Request.Params["ID"]);
        }

        /// <summary>
        ///     Delete Vendor
        /// </summary>
        /// <returns></returns>
        private void Delete()
        {
            pop_ConfirmDelete.ShowOnPageLoad = true;
        }

        /// <summary>
        ///     Back to Vendor List.
        /// </summary>
        private void Back()
        {
            // Clear session
            Session.Remove("dsVendor");

            // Redirect to list page.
            Response.Redirect("VendorList.aspx");
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

            for (var i = 0; i < dsVendor.Tables[bankAccount.TableName].Rows.Count; i++)
            {
                var drBankAccount = dsVendor.Tables[bankAccount.TableName].Rows[i];

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

            for (var i = 0; i < dsVendor.Tables[bankAccount.TableName].Rows.Count; i++)
            {
                var drBankAccount = dsVendor.Tables[bankAccount.TableName].Rows[i];

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

            for (var i = 0; i < dsVendor.Tables[bankAccount.TableName].Rows.Count; i++)
            {
                var drBankAccount = dsVendor.Tables[bankAccount.TableName].Rows[i];

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

            for (var i = 0; i < dsVendor.Tables[bankAccount.TableName].Rows.Count; i++)
            {
                var drBankAccount = dsVendor.Tables[bankAccount.TableName].Rows[i];

                if (drBankAccount.RowState != DataRowState.Deleted)
                {
                    crPercentage += (drBankAccount["CrPercentage"] == DBNull.Value
                        ? 0
                        : (decimal)drBankAccount["CrPercentage"]);
                }
            }

            return crPercentage;
        }

        //================================================================================
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

        /// ==================================end=========================================

        #endregion

        private string CheckVendorExists(string vendorCode)
        {
            string message = string.Empty;
            int count = 0;

            // Price List
            count = Convert.ToInt32(vendor.DbExecuteQuery(string.Format("SELECT COUNT(*) as RecordCount FROM [IN].PL WHERE VendorCode = '{0}'", vendorCode), null, LoginInfo.ConnStr).Rows[0][0]);
            if (count > 0)
                message += "Price List, ";

            // PR
            count = Convert.ToInt32(vendor.DbExecuteQuery(string.Format("SELECT COUNT(*) as RecordCount FROM [PC].PrDt WHERE VendorCode = '{0}'", vendorCode), null, LoginInfo.ConnStr).Rows[0][0]);
            if (count > 0)
                message += "Purchase Request, ";

            // PO
            count = Convert.ToInt32(vendor.DbExecuteQuery(string.Format("SELECT COUNT(*) as RecordCount FROM [PC].PO WHERE Vendor = '{0}'", vendorCode), null, LoginInfo.ConnStr).Rows[0][0]);
            if (count > 0)
                message += "Purchase Order, ";

            // Receiving
            count = Convert.ToInt32(vendor.DbExecuteQuery(string.Format("SELECT COUNT(*) as RecordCount FROM [PC].REC WHERE VendorCode = '{0}'", vendorCode), null, LoginInfo.ConnStr).Rows[0][0]);
            if (count > 0)
                message += "Receiving,";


            if (message != string.Empty)
                message = message.Remove(message.Length - 1);

            return message;
        }



        protected void btn_Yes_Click(object sender, EventArgs e)
        {
            pop_ConfirmDelete.ShowOnPageLoad = false;

            string message = CheckVendorExists(lbl_Vendor.Text);

            if (message != string.Empty)
            {

                lbl_WarningDelete.Text = string.Format("This vendor code '{0}' already exists in {1} ", lbl_Vendor.Text, message);
                pop_WarningDelete.ShowOnPageLoad = true;

                return;
            }

            var profileCode = dsVendor.Tables[vendor.TableName].Rows[0]["ProfileCode"].ToString();

            // Get Profile
            profile.GetProfileListByProfileCode(profileCode, dsVendor, LoginInfo.ConnStr);


            // Delete for profile
            for (var i = dsVendor.Tables[profile.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drprofile = dsVendor.Tables[profile.TableName].Rows[i];

                if (drprofile.RowState != DataRowState.Deleted)
                {
                    drprofile.Delete();
                }
            }

            // Prepare delete for contactdetail.
            for (var i = dsVendor.Tables[contactDetail.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drContactDetail = dsVendor.Tables[contactDetail.TableName].Rows[i];
                if (dsVendor.Tables[contact.TableName].Rows.Count > 0)
                {
                    if (Convert.ToInt32(drContactDetail["ContactID"]) ==
                        Convert.ToInt32(dsVendor.Tables[contact.TableName].Rows[0]["ContactID"]))
                    {
                        if (drContactDetail.RowState != DataRowState.Deleted)
                        {
                            drContactDetail.Delete();
                        }
                    }
                }
            }

            // Prepare delete for contact.
            for (var i = dsVendor.Tables[contact.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drContact = dsVendor.Tables[contact.TableName].Rows[i];

                if (drContact.RowState != DataRowState.Deleted)
                {
                    drContact.Delete();
                }
            }

            // Prepare delete for address.
            for (var i = dsVendor.Tables[address.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drAddress = dsVendor.Tables[address.TableName].Rows[i];

                if (drAddress.RowState != DataRowState.Deleted)
                {
                    drAddress.Delete();
                }
            }

            // Prepare delete for vendor.
            dsVendor.Tables[vendor.TableName].Rows[0].Delete();


            // Delete data from database
            vendor.Delete(dsVendor, LoginInfo.ConnStr);

            // Remove session
            Session.Remove("dsVendor");
            Response.Redirect("VendorList.aspx");
        }

        protected void btn_No_Click(object sender, EventArgs e)
        {
            pop_ConfirmDelete.ShowOnPageLoad = false;
        }

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            pop_WarningDelete.ShowOnPageLoad = false;
            pop_ConfirmDelete.ShowOnPageLoad = false;
        }
    }
}