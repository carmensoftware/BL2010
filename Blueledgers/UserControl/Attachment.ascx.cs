using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.UserControls
{
    public partial class Attachment : BaseUserControl
    {
        #region "Attributies"

        private readonly Blue.BL.GL.AccountReconcileAttachment accRecAttach =
            new Blue.BL.GL.AccountReconcileAttachment();

        private readonly Blue.BL.Reference.AccountAttachment accountAttachment =
            new Blue.BL.Reference.AccountAttachment();

        private readonly Blue.BL.Consolidation.Setup.Application.AccountAttachment accountCon =
            new Blue.BL.Consolidation.Setup.Application.AccountAttachment();

        private readonly Blue.BL.GL.BudgetAttachment budgetAttachment = new Blue.BL.GL.BudgetAttachment();
        private readonly Blue.BL.AR.ProfileAttachment debtorAttach = new Blue.BL.AR.ProfileAttachment();

        private readonly Blue.BL.AP.InvoiceAttachment invoiceAttachment = new Blue.BL.AP.InvoiceAttachment();

        private readonly Blue.BL.GL.JournalVoucherAttachment journalVoucherAttachment =
            new Blue.BL.GL.JournalVoucherAttachment();

        private readonly Blue.BL.AP.PaymentAttachment paymentAttachment = new Blue.BL.AP.PaymentAttachment();
        private readonly Blue.BL.AR.ReceiptAttachment receiptAttach = new Blue.BL.AR.ReceiptAttachment();

        private readonly Blue.BL.GL.StandardVoucherAttachment standardVoucherAttachment =
            new Blue.BL.GL.StandardVoucherAttachment();

        private readonly Blue.BL.ProjectAdmin.SysParameter sysParameter = new Blue.BL.ProjectAdmin.SysParameter();
        private readonly Blue.BL.Security.User user = new Blue.BL.Security.User();
        private readonly Blue.BL.AP.VendorAttachment vendorAttachment = new Blue.BL.AP.VendorAttachment();
        private AttachFromFile _file;
        private AttachMode _mode;
        private DataSet dsAttachment = new DataSet();
        private DataSet dsSave = new DataSet();

        /// <summary>
        ///     Get mode.
        /// </summary>
        public AttachMode Mode
        {
            get
            {
                _mode = (AttachMode) ViewState["Mode"];
                return _mode;
            }
            set
            {
                _mode = value;
                ViewState["Mode"] = _mode;
            }
        }

        /// <summary>
        ///     Get file.
        /// </summary>
        public AttachFromFile FromFile
        {
            get
            {
                _file = (AttachFromFile) ViewState["File"];
                return _file;
            }
            set
            {
                _file = value;
                ViewState["File"] = _file;
            }
        }

        public string GridHeaderCssClass { get; set; }

        public string GridRowCssClass { get; set; }

        #endregion

        #region "Operations"

        /// <summary>
        ///     Add new journal voucher attachment.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_New_Click(object sender, EventArgs e)
        {
            switch (FromFile)
            {
                case AttachFromFile.JournalVourcher: //enum AttachFromFile = 1
                    // Create new row
                    var drJVAttachment = dsAttachment.Tables[journalVoucherAttachment.TableName].NewRow();

                    // Assign default value
                    drJVAttachment["ID"] = Guid.NewGuid();
                    drJVAttachment["RefNo"] = Request.Params["journalvoucherno"];
                    drJVAttachment["UploadedDate"] = ServerDateTime;
                    drJVAttachment["UploadedBy"] = LoginInfo.LoginName;

                    // Add new row
                    dsAttachment.Tables[journalVoucherAttachment.TableName].Rows.Add(drJVAttachment);

                    // Prompt to edit at new row.
                    grd_Attachment.DataSource = dsAttachment.Tables[journalVoucherAttachment.TableName];

                    break;

                case AttachFromFile.Budget: //enum AttachFromFile = 2
                    // Create new row
                    var drBGAttachment = dsAttachment.Tables[budgetAttachment.TableName].NewRow();

                    // Assign default value
                    drBGAttachment["ID"] = Guid.NewGuid();
                    drBGAttachment["RefNo"] = Request.Params["budgetid"];
                    drBGAttachment["UploadedDate"] = ServerDateTime;
                    drBGAttachment["UploadedBy"] = LoginInfo.LoginName;

                    // Add new row
                    dsAttachment.Tables[budgetAttachment.TableName].Rows.Add(drBGAttachment);

                    // Prompt to edit at new row.
                    grd_Attachment.DataSource = dsAttachment.Tables[budgetAttachment.TableName];
                    break;

                case AttachFromFile.Account: //enum AttachFromFile = 3
                    // Create new row
                    var drAccAttachment = dsAttachment.Tables[accountAttachment.TableName].NewRow();

                    // Assign default value
                    drAccAttachment["ID"] = Guid.NewGuid();
                    drAccAttachment["RefNo"] = Request.Params["accountcode"];
                    drAccAttachment["UploadedDate"] = ServerDateTime;
                    drAccAttachment["UploadedBy"] = LoginInfo.LoginName;

                    // Add new row
                    dsAttachment.Tables[accountAttachment.TableName].Rows.Add(drAccAttachment);

                    // Prompt to edit at new row.
                    grd_Attachment.DataSource = dsAttachment.Tables[accountAttachment.TableName];

                    break;

                case AttachFromFile.StandartVourcher: //enum AttachFromFile = 4
                    // Create new row
                    var drSVAttachment = dsAttachment.Tables[standardVoucherAttachment.TableName].NewRow();

                    // Assign default value
                    drSVAttachment["ID"] = Guid.NewGuid();
                    drSVAttachment["RefNo"] = Request.Params["standardvoucherid"];
                    drSVAttachment["UploadedDate"] = ServerDateTime;
                    drSVAttachment["UploadedBy"] = LoginInfo.LoginName;

                    // Add new row
                    dsAttachment.Tables[standardVoucherAttachment.TableName].Rows.Add(drSVAttachment);

                    // Prompt to edit at new row.
                    grd_Attachment.DataSource = dsAttachment.Tables[standardVoucherAttachment.TableName];

                    break;

                case AttachFromFile.AccountConsolidate: //enum AttachFromFile = 5
                    // Create new row
                    var drACAttachment = dsAttachment.Tables[accountCon.TableName].NewRow();

                    // Assign default value
                    drACAttachment["ID"] = Guid.NewGuid();
                    drACAttachment["RefNo"] = Request.Params["accountcode"];
                    drACAttachment["UploadedDate"] = ServerDateTime;
                    drACAttachment["UploadedBy"] = LoginInfo.LoginName;

                    // Add new row
                    dsAttachment.Tables[accountCon.TableName].Rows.Add(drACAttachment);

                    // Prompt to edit at new row.
                    grd_Attachment.DataSource = dsAttachment.Tables[accountCon.TableName];

                    break;

                case AttachFromFile.Vendor: //enum AttachFromFile = 6
                    // Create new row
                    var drVendorAttachment = dsAttachment.Tables[vendorAttachment.TableName].NewRow();

                    // Assign default value
                    drVendorAttachment["ID"] = Guid.NewGuid();
                    drVendorAttachment["RefNo"] = Request.Params["vendorCode"];
                    drVendorAttachment["UploadedDate"] = ServerDateTime;
                    drVendorAttachment["UploadedBy"] = LoginInfo.LoginName;

                    // Add new row
                    dsAttachment.Tables[vendorAttachment.TableName].Rows.Add(drVendorAttachment);

                    // Prompt to edit at new row.
                    grd_Attachment.DataSource = dsAttachment.Tables[vendorAttachment.TableName];

                    break;
                case AttachFromFile.Debtor: //enum AttachFromFile = 7
                    // Create new row
                    var drDebtorAttachment = dsAttachment.Tables[debtorAttach.TableName].NewRow();

                    // Assign default value
                    drDebtorAttachment["ID"] = Guid.NewGuid();
                    drDebtorAttachment["RefNo"] = Request.Params["CustomerCode"];
                    drDebtorAttachment["UploadedDate"] = ServerDateTime;
                    drDebtorAttachment["UploadedBy"] = LoginInfo.LoginName;

                    // Add new row
                    dsAttachment.Tables[debtorAttach.TableName].Rows.Add(drDebtorAttachment);

                    // Prompt to edit at new row.
                    grd_Attachment.DataSource = dsAttachment.Tables[debtorAttach.TableName];

                    break;
                case AttachFromFile.Invoice: //enum AttachFromFile = 8                   
                    // Create new row
                    var drinvoiceAttachment = dsAttachment.Tables[invoiceAttachment.TableName].NewRow();

                    // Assign default value
                    drinvoiceAttachment["ID"] = Guid.NewGuid();
                    drinvoiceAttachment["RefNo"] = Request.Params["VoucherNo"];
                    drinvoiceAttachment["UploadedDate"] = ServerDateTime;
                    drinvoiceAttachment["UploadedBy"] = LoginInfo.LoginName;

                    // Add new row
                    dsAttachment.Tables[invoiceAttachment.TableName].Rows.Add(drinvoiceAttachment);

                    // Prompt to edit at new row.
                    grd_Attachment.DataSource = dsAttachment.Tables[invoiceAttachment.TableName];

                    break;

                case AttachFromFile.Payment: //enum AttachFromFile = 9
                    // Create new row
                    var drPaymentAttachment = dsAttachment.Tables[paymentAttachment.TableName].NewRow();

                    // Assign default value
                    drPaymentAttachment["ID"] = Guid.NewGuid();
                    drPaymentAttachment["RefNo"] = Request.Params["VoucherNo"];
                    drPaymentAttachment["UploadedDate"] = ServerDateTime;
                    drPaymentAttachment["UploadedBy"] = LoginInfo.LoginName;

                    // Add new row
                    dsAttachment.Tables[paymentAttachment.TableName].Rows.Add(drPaymentAttachment);

                    // Prompt to edit at new row.
                    grd_Attachment.DataSource = dsAttachment.Tables[paymentAttachment.TableName];

                    break;

                case AttachFromFile.Receipt: //enum AttachFromFile = 10
                    // Create new row
                    var drAttachment = dsAttachment.Tables[receiptAttach.TableName].NewRow();

                    // Assign default value
                    drAttachment["ID"] = Guid.NewGuid();
                    drAttachment["RefNo"] = Request.Params["ReceiptNo"];
                    drAttachment["UploadedDate"] = ServerDateTime;
                    drAttachment["UploadedBy"] = LoginInfo.LoginName;

                    // Add new row
                    dsAttachment.Tables[receiptAttach.TableName].Rows.Add(drAttachment);

                    // Prompt to edit at new row.
                    grd_Attachment.DataSource = dsAttachment.Tables[receiptAttach.TableName];
                    break;

                case AttachFromFile.AccountReconcile: //enum AttachFromFile = 11
                    // Create new row
                    var drAccRecAttachment = dsAttachment.Tables[accRecAttach.TableName].NewRow();

                    // Assign default value
                    drAccRecAttachment["ID"] = Guid.NewGuid();
                    drAccRecAttachment["RefNo"] = Request.Params["AccRecNo"];
                    drAccRecAttachment["UploadedDate"] = ServerDateTime;
                    drAccRecAttachment["UploadedBy"] = LoginInfo.LoginName;

                    // Add new row
                    dsAttachment.Tables[accRecAttach.TableName].Rows.Add(drAccRecAttachment);

                    // Prompt to edit at new row.
                    grd_Attachment.DataSource = dsAttachment.Tables[accRecAttach.TableName];
                    break;
            }

            grd_Attachment.EditIndex = grd_Attachment.Rows.Count;
            grd_Attachment.DataBind();

            // Save to session
            Session["dsAttachment"] = dsAttachment;
        }

        /// <summary>
        ///     Display journal voucher attachment data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Attachment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Apply style
                foreach (TableCell tdAttachment in e.Row.Cells)
                {
                    tdAttachment.CssClass = GridHeaderCssClass;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Apply style
                foreach (TableCell tdAttachment in e.Row.Cells)
                {
                    tdAttachment.CssClass = GridRowCssClass;
                }

                // File Name
                if (e.Row.FindControl("lnk_FileName") != null)
                {
                    var lnk_FileName = (HyperLink) e.Row.FindControl("lnk_FileName");

                    lnk_FileName.Text = DataBinder.Eval(e.Row.DataItem, "FileName").ToString();
                    lnk_FileName.NavigateUrl = "/" + AppName + DataBinder.Eval(e.Row.DataItem, "FilePath");
                    lnk_FileName.Target = "_blank";
                }
                if (e.Row.FindControl("fu_FileName") != null)
                {
                    var fu_FileName = (FileUpload) e.Row.FindControl("fu_FileName");
                }

                // Descriotion
                if (e.Row.FindControl("lbl_Description") != null)
                {
                    var lbl_Description = (Label) e.Row.FindControl("lbl_Description");
                    lbl_Description.Text = DataBinder.Eval(e.Row.DataItem, "Description").ToString();
                }
                if (e.Row.FindControl("txt_Description") != null)
                {
                    var txt_Description = (TextBox) e.Row.FindControl("txt_Description");
                    txt_Description.Text = DataBinder.Eval(e.Row.DataItem, "Description").ToString();
                }

                // Public
                if (e.Row.FindControl("chk_IsPublic") != null)
                {
                    var chk_IsPublic = (CheckBox) e.Row.FindControl("chk_IsPublic");
                    if (DataBinder.Eval(e.Row.DataItem, "IsPublic").ToString() == string.Empty)
                    {
                        chk_IsPublic.Checked = false;
                    }
                    else
                    {
                        chk_IsPublic.Checked = true;
                    }

                    chk_IsPublic.Enabled = (grd_Attachment.EditIndex == e.Row.RowIndex ? true : false);
                }

                // Uploaded Date
                if (e.Row.FindControl("lbl_UploadedDate") != null)
                {
                    var lbl_UploadedDate = (Label) e.Row.FindControl("lbl_UploadedDate");
                    lbl_UploadedDate.Text =
                        ((DateTime) DataBinder.Eval(e.Row.DataItem, "UploadedDate")).ToString(DateTimeFormat);
                }

                // Uploaded By
                if (e.Row.FindControl("lbl_UploadedBy") != null)
                {
                    var lbl_UploadedBy = (Label) e.Row.FindControl("lbl_UploadedBy");
                    lbl_UploadedBy.Text = user.GetUserName((int) DataBinder.Eval(e.Row.DataItem, "UploadedBy"),
                        LoginInfo.ConnStr);
                }
            }
        }

        /// <summary>
        ///     Remove journal voucher attachment data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Attachment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            switch (FromFile)
            {
                case AttachFromFile.JournalVourcher:
                    DeleteJV(e.RowIndex);
                    break;

                case AttachFromFile.Budget:
                    DeleteBG(e.RowIndex);
                    break;

                case AttachFromFile.Account:
                    DeleteAcc(e.RowIndex);
                    break;

                case AttachFromFile.StandartVourcher:
                    DeleteSV(e.RowIndex);
                    break;

                case AttachFromFile.AccountConsolidate:
                    DeleteAC(e.RowIndex);
                    break;

                case AttachFromFile.Vendor:
                    DeleteVendor(e.RowIndex);
                    break;

                case AttachFromFile.Debtor:
                    DeleteDebtor(e.RowIndex);
                    break;

                case AttachFromFile.Invoice:
                    DeleteInvoice(e.RowIndex);
                    break;

                case AttachFromFile.Payment:
                    DeletePayment(e.RowIndex);
                    break;

                case AttachFromFile.Receipt:
                    DeleteReceipt(e.RowIndex);
                    break;

                case AttachFromFile.AccountReconcile:
                    DeleteAccountReconcile(e.RowIndex);
                    break;
            }
        }

        /// <summary>
        ///     Cancel adding journal voucher attachment data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Attachment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            switch (FromFile)
            {
                case AttachFromFile.JournalVourcher:
                    CanclingEditJV(e.RowIndex);
                    break;

                case AttachFromFile.Budget:
                    CanclingEditBG(e.RowIndex);
                    break;

                case AttachFromFile.Account:
                    CanclingEditAcc(e.RowIndex);
                    break;

                case AttachFromFile.StandartVourcher:
                    CanclingEditSV(e.RowIndex);
                    break;

                case AttachFromFile.AccountConsolidate:
                    CanclingEditAC(e.RowIndex);
                    break;

                case AttachFromFile.Vendor:
                    CanclingEditVendor(e.RowIndex);
                    break;

                case AttachFromFile.Debtor:
                    CanclingEditDebtor(e.RowIndex);
                    break;

                case AttachFromFile.Invoice:
                    CanclingEditInvoice(e.RowIndex);
                    break;

                case AttachFromFile.Payment:
                    CanclingEditPayment(e.RowIndex);
                    break;

                case AttachFromFile.Receipt:
                    CanclingEditReceipt(e.RowIndex);
                    break;

                case AttachFromFile.AccountReconcile:
                    CanclingEditAccRec(e.RowIndex);
                    break;
            }

            grd_Attachment.EditIndex = -1;
            grd_Attachment.DataBind();

            // Save to session
            Session["dsAttachment"] = dsAttachment;
        }

        /// <summary>
        ///     Add journal voucher attachment data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Attachment_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            switch (FromFile)
            {
                case AttachFromFile.JournalVourcher:
                    UpdateJV(e.RowIndex);
                    break;

                case AttachFromFile.Budget:
                    UpdateBG(e.RowIndex);
                    break;

                case AttachFromFile.Account:
                    UpdateAcc(e.RowIndex);
                    break;

                case AttachFromFile.StandartVourcher:
                    UpdateSV(e.RowIndex);
                    break;

                case AttachFromFile.AccountConsolidate:
                    UpdateAC(e.RowIndex);
                    break;

                case AttachFromFile.Vendor:
                    UpdateVendor(e.RowIndex);
                    break;

                case AttachFromFile.Debtor:
                    UpdateDebtor(e.RowIndex);
                    break;

                case AttachFromFile.Invoice:
                    UpdateInvoice(e.RowIndex);
                    break;

                case AttachFromFile.Payment:
                    UpdatePayment(e.RowIndex);
                    break;

                case AttachFromFile.Receipt:
                    UpdateReceipt(e.RowIndex);
                    break;

                case AttachFromFile.AccountReconcile:
                    UpdateAccountReconcile(e.RowIndex);
                    break;
            }
        }

        /// <summary>
        ///     Get journal voucher attachment data.
        /// </summary>
        private void Page_Retrieve()
        {
            dsAttachment = (DataSet) Session["dsAttachment"];
        }

        /// <summary>
        ///     Display journal voucher attachment data.
        /// </summary>
        private void Page_Setting()
        {
            //grd_Attachment.HeaderStyle.CssClass         = GridHeaderCssClass;
            //grd_Attachment.RowStyle.CssClass            = GridRowCssClass;
            //grd_Attachment.AlternatingRowStyle.CssClass = GridRowCssClass;

            //grd_Attachment.HeaderRow.CssClass = GridHeaderCssClass;

            switch (FromFile)
            {
                    //JournalVourcher
                case AttachFromFile.JournalVourcher:

                    grd_Attachment.DataSource = dsAttachment.Tables[journalVoucherAttachment.TableName];
                    grd_Attachment.DataBind();

                    break;

                    //Budget
                case AttachFromFile.Budget:

                    grd_Attachment.DataSource = dsAttachment.Tables[budgetAttachment.TableName];
                    grd_Attachment.DataBind();

                    break;

                    //Account
                case AttachFromFile.Account:

                    grd_Attachment.DataSource = dsAttachment.Tables[accountAttachment.TableName];
                    grd_Attachment.DataBind();

                    break;

                    //StandartVourcher
                case AttachFromFile.StandartVourcher:

                    grd_Attachment.DataSource = dsAttachment.Tables[standardVoucherAttachment.TableName];
                    grd_Attachment.DataBind();

                    break;

                    //Account Consolidate
                case AttachFromFile.AccountConsolidate:

                    grd_Attachment.DataSource = dsAttachment.Tables[accountCon.TableName];
                    grd_Attachment.DataBind();

                    break;

                    // Vendor 
                case AttachFromFile.Vendor:
                    grd_Attachment.DataSource = dsAttachment.Tables[vendorAttachment.TableName];
                    grd_Attachment.DataBind();
                    break;

                    // Invoice 
                case AttachFromFile.Invoice:
                    grd_Attachment.DataSource = dsAttachment.Tables[invoiceAttachment.TableName];
                    grd_Attachment.DataBind();
                    break;

                    // Payment
                case AttachFromFile.Payment:
                    grd_Attachment.DataSource = dsAttachment.Tables[paymentAttachment.TableName];
                    grd_Attachment.DataBind();
                    break;
            }
        }

        /// <summary>
        ///     Main process.
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
            else
            {
                dsAttachment = (DataSet) Session["dsAttachment"];
            }
        }

        //------------ UPDATE ---------------------------
        /// <summary>
        ///     Update journal voucher table
        /// </summary>
        private void UpdateJV(int RowIndex)
        {
            // Updating
            // Get primary key of updating row
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            foreach (DataRow drUpdating in dsAttachment.Tables[journalVoucherAttachment.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        var fu_FileName = (FileUpload) grd_Attachment.Rows[RowIndex].FindControl("fu_FileName");
                        var txt_Description = (TextBox) grd_Attachment.Rows[RowIndex].FindControl("txt_Description");
                        var chk_IsPublic = (CheckBox) grd_Attachment.Rows[RowIndex].FindControl("chk_IsPublic");

                        // Uploading file
                        if (fu_FileName.HasFile)
                        {
                            // Check Upload directory **************************************************************************
                            // Get desination directory
                            var desFilePath = string.Empty;

                            if (sysParameter.GetValue("JV", "UploadPath", LoginInfo.ConnStr) != string.Empty)
                            {
                                desFilePath = "/" + AppName +
                                              sysParameter.GetValue("JV", "UploadPath", LoginInfo.ConnStr) +
                                              drUpdating["RefNo"] + "/";

                                // Create destination directory if not found.
                                if (!Directory.Exists(Server.MapPath(desFilePath)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(desFilePath));
                                }
                            }

                            // Check File Size (Bytes) *************************************************************************
                            // Default max upload file size is 4MB, if want to upload the larger one, need to change at
                            // "maxRequestLength" property in machine.config.comments from 4096 to what ever you want.
                            var maxFileSize =
                                int.Parse(sysParameter.GetValue("JV", "UploadSizeMax", LoginInfo.ConnStr))*1024*1024;
                            if (fu_FileName.FileBytes.Length > maxFileSize)
                            {
                                // Display Error
                                return;
                            }

                            // Check File's Extension. *************************************************************************            
                            // File extension control flag description
                            // 0 : Allow all files upload.
                            // 1 : Only file's extension in list can upload.
                            // 2 : Only file's extension in list can not upload.                            
                            var fileUpload = new FileInfo(fu_FileName.PostedFile.FileName);
                            var fileExtentionControl =
                                int.Parse(sysParameter.GetValue("JV", "UploadFileExtentionCotrol", LoginInfo.ConnStr));
                            var fileExtensionList = sysParameter.GetValue("JV", "UploadFileExtensionList",
                                LoginInfo.ConnStr);

                            switch (fileExtentionControl)
                            {
                                case 1: // File's extension in list allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) < 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;

                                case 2: // File's extension in list not allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) >= 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;
                            }

                            // Pending to new version
                            // Check File Exist. Replace, Rename or Cancel. ****************************************************
                            //FileInfo fileExist = new FileInfo(Server.MapPath(desFilePath) + fu_FileName.FileName);
                            //if (fileExist.Exists)
                            //{
                            //    // Display error
                            //    return;
                            //}

                            // Uploading File **********************************************************************************
                            var count = RowIndex + 1;
                            var GenFileName = drUpdating["RefNo"] + "_" + count + fileUpload.Extension;

                            fu_FileName.SaveAs(Server.MapPath(desFilePath) + GenFileName);

                            // Pending to new version
                            //// Auto Scan Virus *********************************************************************************                             
                            //string antiVirusEnginePath = "c:\\program files\\eset\\nod32.exe";
                            //string antiVirusParameter = "/clean /delete";
                            //ProcessStartInfo procStartInfo = new ProcessStartInfo();
                            //procStartInfo.FileName = antiVirusEnginePath;
                            //procStartInfo.Arguments = desFilePath + FileUpload1.FileName + " " + antiVirusParameter;
                            //procStartInfo.UseShellExecute = false;
                            //procStartInfo.RedirectStandardOutput = true;
                            //procStartInfo.RedirectStandardInput = true;
                            //procStartInfo.RedirectStandardError = true;
                            //Process proc = Process.Start(procStartInfo);
                            //proc.Close();
                            //// If uploaded file was deleted, It's a virus file.
                            //FileInfo fileUploaded = new FileInfo(desFilePath + FileUpload1.FileName);
                            //if (!fileUploaded.Exists)
                            //{
                            //    Label1.Text = FileUpload1.FileName + " may has some infect, please clening it before next upload.";
                            //    return;
                            //}                           

                            // Updating journal voucher attachment data
                            drUpdating["RefNo"] =
                                dsAttachment.Tables["JournalVoucher"].Rows[0]["JournalVoucherNo"].ToString();
                            drUpdating["FileName"] = (fu_FileName.HasFile ? fu_FileName.FileName : string.Empty);
                            drUpdating["Description"] = txt_Description.Text.Trim();
                            drUpdating["IsPublic"] = chk_IsPublic.Checked;
                            drUpdating["FilePath"] = sysParameter.GetValue("JV", "UploadPath", LoginInfo.ConnStr) +
                                                     drUpdating["RefNo"] + "/" + GenFileName;

                            // Refresh data in GridView
                            grd_Attachment.DataSource = dsAttachment.Tables[journalVoucherAttachment.TableName];
                            grd_Attachment.EditIndex = -1;
                            grd_Attachment.DataBind();

                            // Check for submit data.
                            // Edit data from journalvourcheredit.
                            if (Mode == AttachMode.Edit)
                            {
                                // Set session for journalvourcheredit page.
                                Session["dsJournalVourcherEdit"] = dsAttachment;
                            }
                                // Edit data from journalvourcher.
                            else if (Mode == AttachMode.Display)
                            {
                                journalVoucherAttachment.Save(dsAttachment, LoginInfo.ConnStr);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Update account attachment table
        /// </summary>
        private void UpdateAcc(int RowIndex)
        {
            // Updating
            // Get primary key of updating row
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            foreach (DataRow drUpdating in dsAttachment.Tables[accountAttachment.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        var fu_FileName = (FileUpload) grd_Attachment.Rows[RowIndex].FindControl("fu_FileName");
                        var txt_Description = (TextBox) grd_Attachment.Rows[RowIndex].FindControl("txt_Description");
                        var chk_IsPublic = (CheckBox) grd_Attachment.Rows[RowIndex].FindControl("chk_IsPublic");

                        // Uploading file
                        if (fu_FileName.HasFile)
                        {
                            // Check Upload directory **************************************************************************
                            // Get desination directory
                            var desFilePath = string.Empty;

                            if (sysParameter.GetValue("AC", "UploadPath", LoginInfo.ConnStr) != string.Empty)
                            {
                                desFilePath = "/" + AppName +
                                              sysParameter.GetValue("AC", "UploadPath", LoginInfo.ConnStr) +
                                              drUpdating["RefNo"] + "/";

                                // Create destination directory if not found.
                                if (!Directory.Exists(Server.MapPath(desFilePath)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(desFilePath));
                                }
                            }

                            // Check File Size (Bytes) *************************************************************************
                            // Default max upload file size is 4MB, if want to upload the larger one, need to change at
                            // "maxRequestLength" property in machine.config.comments from 4096 to what ever you want.
                            var maxFileSize =
                                int.Parse(sysParameter.GetValue("AC", "UploadSizeMax", LoginInfo.ConnStr))*1024*1024;
                            if (fu_FileName.FileBytes.Length > maxFileSize)
                            {
                                // Display Error
                                return;
                            }

                            // Check File's Extension. *************************************************************************            
                            // File extension control flag description
                            // 0 : Allow all files upload.
                            // 1 : Only file's extension in list can upload.
                            // 2 : Only file's extension in list can not upload.                            
                            var fileUpload = new FileInfo(fu_FileName.PostedFile.FileName);
                            var fileExtentionControl =
                                int.Parse(sysParameter.GetValue("AC", "UploadFileExtentionCotrol", LoginInfo.ConnStr));
                            var fileExtensionList = sysParameter.GetValue("AC", "UploadFileExtensionList",
                                LoginInfo.ConnStr);

                            switch (fileExtentionControl)
                            {
                                case 1: // File's extension in list allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) < 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;

                                case 2: // File's extension in list not allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) >= 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;
                            }

                            // Pending to new version
                            // Check File Exist. Replace, Rename or Cancel. ****************************************************
                            //FileInfo fileExist = new FileInfo(Server.MapPath(desFilePath) + fu_FileName.FileName);
                            //if (fileExist.Exists)
                            //{
                            //    // Display error
                            //    return;
                            //}

                            // Uploading File **********************************************************************************
                            var count = RowIndex + 1;
                            var GenFileName = drUpdating["RefNo"] + "_" + count + fileUpload.Extension;

                            fu_FileName.SaveAs(Server.MapPath(desFilePath) + GenFileName);

                            // Pending to new version
                            //// Auto Scan Virus *********************************************************************************                             
                            //string antiVirusEnginePath = "c:\\program files\\eset\\nod32.exe";
                            //string antiVirusParameter = "/clean /delete";
                            //ProcessStartInfo procStartInfo = new ProcessStartInfo();
                            //procStartInfo.FileName = antiVirusEnginePath;
                            //procStartInfo.Arguments = desFilePath + FileUpload1.FileName + " " + antiVirusParameter;
                            //procStartInfo.UseShellExecute = false;
                            //procStartInfo.RedirectStandardOutput = true;
                            //procStartInfo.RedirectStandardInput = true;
                            //procStartInfo.RedirectStandardError = true;
                            //Process proc = Process.Start(procStartInfo);
                            //proc.Close();
                            //// If uploaded file was deleted, It's a virus file.
                            //FileInfo fileUploaded = new FileInfo(desFilePath + FileUpload1.FileName);
                            //if (!fileUploaded.Exists)
                            //{
                            //    Label1.Text = FileUpload1.FileName + " may has some infect, please clening it before next upload.";
                            //    return;
                            //}                           

                            // Updating journal voucher attachment data
                            drUpdating["RefNo"] = dsAttachment.Tables["Account"].Rows[0]["AccountCode"].ToString();
                            drUpdating["FileName"] = (fu_FileName.HasFile ? fu_FileName.FileName : string.Empty);
                            drUpdating["Description"] = txt_Description.Text.Trim();
                            drUpdating["IsPublic"] = chk_IsPublic.Checked;
                            drUpdating["FilePath"] = sysParameter.GetValue("AC", "UploadPath", LoginInfo.ConnStr) +
                                                     drUpdating["RefNo"] + "/" + GenFileName;

                            // Refresh data in GridView
                            grd_Attachment.DataSource = dsAttachment.Tables[accountAttachment.TableName];
                            grd_Attachment.EditIndex = -1;
                            grd_Attachment.DataBind();

                            // Check for submit data.
                            // Edit data from accountedit.
                            if (Mode == AttachMode.Edit)
                            {
                                // Set session for accountedit page.
                                Session["dsAccountEdit"] = dsAttachment;
                            }
                                // Edit data from journalvourcher.
                            else if (Mode == AttachMode.Display)
                            {
                                accountAttachment.Save(dsAttachment, LoginInfo.ConnStr);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Update budget attachment table
        /// </summary>
        /// <param name="RowIndex"></param>
        private void UpdateBG(int RowIndex)
        {
            // Updating
            // Get primary key of updating row
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            foreach (DataRow drUpdating in dsAttachment.Tables[budgetAttachment.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        var fu_FileName = (FileUpload) grd_Attachment.Rows[RowIndex].FindControl("fu_FileName");
                        var txt_Description = (TextBox) grd_Attachment.Rows[RowIndex].FindControl("txt_Description");
                        var chk_IsPublic = (CheckBox) grd_Attachment.Rows[RowIndex].FindControl("chk_IsPublic");

                        // Uploading file
                        if (fu_FileName.HasFile)
                        {
                            // Check Upload directory **************************************************************************
                            // Get desination directory
                            var desFilePath = string.Empty;

                            if (sysParameter.GetValue("BG", "UploadPath", LoginInfo.ConnStr) != string.Empty)
                            {
                                desFilePath = "/" + AppName +
                                              sysParameter.GetValue("BG", "UploadPath", LoginInfo.ConnStr) +
                                              drUpdating["RefNo"] + "/";

                                // Create destination directory if not found.
                                if (!Directory.Exists(Server.MapPath(desFilePath)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(desFilePath));
                                }
                            }

                            // Check File Size (Bytes) *************************************************************************
                            // Default max upload file size is 4MB, if want to upload the larger one, need to change at
                            // "maxRequestLength" property in machine.config.comments from 4096 to what ever you want.
                            var maxFileSize =
                                int.Parse(sysParameter.GetValue("BG", "UploadSizeMax", LoginInfo.ConnStr))*1024*1024;
                            if (fu_FileName.FileBytes.Length > maxFileSize)
                            {
                                // Display Error
                                return;
                            }

                            // Check File's Extension. *************************************************************************            
                            // File extension control flag description
                            // 0 : Allow all files upload.
                            // 1 : Only file's extension in list can upload.
                            // 2 : Only file's extension in list can not upload.                            
                            var fileUpload = new FileInfo(fu_FileName.PostedFile.FileName);
                            var fileExtentionControl =
                                int.Parse(sysParameter.GetValue("BG", "UploadFileExtentionCotrol", LoginInfo.ConnStr));
                            var fileExtensionList = sysParameter.GetValue("BG", "UploadFileExtensionList",
                                LoginInfo.ConnStr);

                            switch (fileExtentionControl)
                            {
                                case 1: // File's extension in list allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) < 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;

                                case 2: // File's extension in list not allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) >= 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;
                            }

                            // Pending to new version
                            // Check File Exist. Replace, Rename or Cancel. ****************************************************
                            //FileInfo fileExist = new FileInfo(Server.MapPath(desFilePath) + fu_FileName.FileName);
                            //if (fileExist.Exists)
                            //{
                            //    // Display error
                            //    return;
                            //}

                            // Uploading File **********************************************************************************
                            var count = RowIndex + 1;
                            var GenFileName = drUpdating["RefNo"] + "_" + count + fileUpload.Extension;

                            fu_FileName.SaveAs(Server.MapPath(desFilePath) + GenFileName);

                            // Pending to new version
                            //// Auto Scan Virus *********************************************************************************                             
                            //string antiVirusEnginePath = "c:\\program files\\eset\\nod32.exe";
                            //string antiVirusParameter = "/clean /delete";
                            //ProcessStartInfo procStartInfo = new ProcessStartInfo();
                            //procStartInfo.FileName = antiVirusEnginePath;
                            //procStartInfo.Arguments = desFilePath + FileUpload1.FileName + " " + antiVirusParameter;
                            //procStartInfo.UseShellExecute = false;
                            //procStartInfo.RedirectStandardOutput = true;
                            //procStartInfo.RedirectStandardInput = true;
                            //procStartInfo.RedirectStandardError = true;
                            //Process proc = Process.Start(procStartInfo);
                            //proc.Close();
                            //// If uploaded file was deleted, It's a virus file.
                            //FileInfo fileUploaded = new FileInfo(desFilePath + FileUpload1.FileName);
                            //if (!fileUploaded.Exists)
                            //{
                            //    Label1.Text = FileUpload1.FileName + " may has some infect, please clening it before next upload.";
                            //    return;
                            //}                           

                            // Updating budget attachment data
                            drUpdating["RefNo"] = dsAttachment.Tables["Budget"].Rows[0]["BudgetID"].ToString();
                            drUpdating["FileName"] = (fu_FileName.HasFile ? fu_FileName.FileName : string.Empty);
                            drUpdating["Description"] = txt_Description.Text.Trim();
                            drUpdating["IsPublic"] = chk_IsPublic.Checked;
                            drUpdating["FilePath"] = sysParameter.GetValue("BG", "UploadPath", LoginInfo.ConnStr) +
                                                     drUpdating["RefNo"] + "/" + GenFileName;

                            // Refresh data in GridView
                            grd_Attachment.DataSource = dsAttachment.Tables[budgetAttachment.TableName];
                            grd_Attachment.EditIndex = -1;
                            grd_Attachment.DataBind();

                            // Check for submit data.
                            // Edit data from accountedit.
                            if (Mode == AttachMode.Edit)
                            {
                                // Set session for budgetedit page.
                                Session["dsBudgetEdit"] = dsAttachment;
                            }
                                // Edit data from budget.
                            else if (Mode == AttachMode.Display)
                            {
                                budgetAttachment.Save(dsAttachment, LoginInfo.ConnStr);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Update standart voucher table
        /// </summary>
        /// <param name="RowIndex"></param>
        private void UpdateSV(int RowIndex)
        {
            // Updating
            // Get primary key of updating row
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            foreach (DataRow drUpdating in dsAttachment.Tables[standardVoucherAttachment.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        var fu_FileName = (FileUpload) grd_Attachment.Rows[RowIndex].FindControl("fu_FileName");
                        var txt_Description = (TextBox) grd_Attachment.Rows[RowIndex].FindControl("txt_Description");
                        var chk_IsPublic = (CheckBox) grd_Attachment.Rows[RowIndex].FindControl("chk_IsPublic");

                        // Uploading file
                        if (fu_FileName.HasFile)
                        {
                            // Check Upload directory **************************************************************************
                            // Get desination directory
                            var desFilePath = string.Empty;

                            if (sysParameter.GetValue("SV", "UploadPath", LoginInfo.ConnStr) != string.Empty)
                            {
                                desFilePath = "/" + AppName +
                                              sysParameter.GetValue("SV", "UploadPath", LoginInfo.ConnStr) +
                                              drUpdating["RefNo"] + "/";

                                // Create destination directory if not found.
                                if (!Directory.Exists(Server.MapPath(desFilePath)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(desFilePath));
                                }
                            }

                            // Check File Size (Bytes) *************************************************************************
                            // Default max upload file size is 4MB, if want to upload the larger one, need to change at
                            // "maxRequestLength" property in machine.config.comments from 4096 to what ever you want.
                            var maxFileSize =
                                int.Parse(sysParameter.GetValue("SV", "UploadSizeMax", LoginInfo.ConnStr))*1024*1024;
                            if (fu_FileName.FileBytes.Length > maxFileSize)
                            {
                                // Display Error
                                return;
                            }

                            // Check File's Extension. *************************************************************************            
                            // File extension control flag description
                            // 0 : Allow all files upload.
                            // 1 : Only file's extension in list can upload.
                            // 2 : Only file's extension in list can not upload.                            
                            var fileUpload = new FileInfo(fu_FileName.PostedFile.FileName);
                            var fileExtentionControl =
                                int.Parse(sysParameter.GetValue("SV", "UploadFileExtentionCotrol", LoginInfo.ConnStr));
                            var fileExtensionList = sysParameter.GetValue("SV", "UploadFileExtensionList",
                                LoginInfo.ConnStr);

                            switch (fileExtentionControl)
                            {
                                case 1: // File's extension in list allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) < 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;

                                case 2: // File's extension in list not allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) >= 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;
                            }

                            // Pending to new version
                            // Check File Exist. Replace, Rename or Cancel. ****************************************************
                            //FileInfo fileExist = new FileInfo(Server.MapPath(desFilePath) + fu_FileName.FileName);
                            //if (fileExist.Exists)
                            //{
                            //    // Display error
                            //    return;
                            //}

                            // Uploading File **********************************************************************************
                            var count = RowIndex + 1;
                            var GenFileName = drUpdating["RefNo"] + "_" + count + fileUpload.Extension;

                            fu_FileName.SaveAs(Server.MapPath(desFilePath) + GenFileName);

                            // Pending to new version
                            //// Auto Scan Virus *********************************************************************************                             
                            //string antiVirusEnginePath = "c:\\program files\\eset\\nod32.exe";
                            //string antiVirusParameter = "/clean /delete";
                            //ProcessStartInfo procStartInfo = new ProcessStartInfo();
                            //procStartInfo.FileName = antiVirusEnginePath;
                            //procStartInfo.Arguments = desFilePath + FileUpload1.FileName + " " + antiVirusParameter;
                            //procStartInfo.UseShellExecute = false;
                            //procStartInfo.RedirectStandardOutput = true;
                            //procStartInfo.RedirectStandardInput = true;
                            //procStartInfo.RedirectStandardError = true;
                            //Process proc = Process.Start(procStartInfo);
                            //proc.Close();
                            //// If uploaded file was deleted, It's a virus file.
                            //FileInfo fileUploaded = new FileInfo(desFilePath + FileUpload1.FileName);
                            //if (!fileUploaded.Exists)
                            //{
                            //    Label1.Text = FileUpload1.FileName + " may has some infect, please clening it before next upload.";
                            //    return;
                            //}                           

                            // Updating standard voucher attachment data
                            drUpdating["RefNo"] =
                                dsAttachment.Tables["StandardVoucher"].Rows[0]["StandardVoucherID"].ToString();
                            drUpdating["FileName"] = (fu_FileName.HasFile ? fu_FileName.FileName : string.Empty);
                            drUpdating["Description"] = txt_Description.Text.Trim();
                            drUpdating["IsPublic"] = chk_IsPublic.Checked;
                            drUpdating["FilePath"] = sysParameter.GetValue("SV", "UploadPath", LoginInfo.ConnStr) +
                                                     drUpdating["RefNo"] + "/" + GenFileName;

                            // Refresh data in GridView
                            grd_Attachment.DataSource = dsAttachment.Tables[standardVoucherAttachment.TableName];
                            grd_Attachment.EditIndex = -1;
                            grd_Attachment.DataBind();

                            // Check for submit data.
                            // Edit data from accountedit.
                            if (Mode == AttachMode.Edit)
                            {
                                // Set session for standard voucher edit page.
                                Session["dsStandardVoucherEdit"] = dsAttachment;
                            }
                                // Edit data from budget.
                            else if (Mode == AttachMode.Display)
                            {
                                standardVoucherAttachment.Save(dsAttachment, LoginInfo.ConnStr);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Update account attachment table at consolidate.
        /// </summary>
        private void UpdateAC(int RowIndex)
        {
            // Updating
            // Get primary key of updating row
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            foreach (DataRow drUpdating in dsAttachment.Tables[vendorAttachment.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        var fu_FileName = (FileUpload) grd_Attachment.Rows[RowIndex].FindControl("fu_FileName");
                        var txt_Description = (TextBox) grd_Attachment.Rows[RowIndex].FindControl("txt_Description");
                        var chk_IsPublic = (CheckBox) grd_Attachment.Rows[RowIndex].FindControl("chk_IsPublic");

                        // Uploading file
                        if (fu_FileName.HasFile)
                        {
                            // Check Upload directory **************************************************************************
                            // Get desination directory
                            var desFilePath = string.Empty;

                            if (sysParameter.GetValue("ACCon", "UploadPath", string.Empty) != string.Empty)
                            {
                                desFilePath = "/" + AppName + sysParameter.GetValue("ACCon", "UploadPath", string.Empty) +
                                              drUpdating["RefNo"] + "/";

                                // Create destination directory if not found.
                                if (!Directory.Exists(Server.MapPath(desFilePath)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(desFilePath));
                                }
                            }

                            // Check File Size (Bytes) *************************************************************************
                            // Default max upload file size is 4MB, if want to upload the larger one, need to change at
                            // "maxRequestLength" property in machine.config.comments from 4096 to what ever you want.
                            var maxFileSize = int.Parse(sysParameter.GetValue("ACCon", "UploadSizeMax", string.Empty))*
                                              1024*1024;
                            if (fu_FileName.FileBytes.Length > maxFileSize)
                            {
                                // Display Error
                                return;
                            }

                            // Check File's Extension. *************************************************************************            
                            // File extension control flag description
                            // 0 : Allow all files upload.
                            // 1 : Only file's extension in list can upload.
                            // 2 : Only file's extension in list can not upload.                            
                            var fileUpload = new FileInfo(fu_FileName.PostedFile.FileName);
                            var fileExtentionControl =
                                int.Parse(sysParameter.GetValue("ACCon", "UploadFileExtentionCotrol", string.Empty));
                            var fileExtensionList = sysParameter.GetValue("ACCon", "UploadFileExtensionList",
                                string.Empty);

                            switch (fileExtentionControl)
                            {
                                case 1: // File's extension in list allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) < 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;

                                case 2: // File's extension in list not allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) >= 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;
                            }

                            // Pending to new version
                            // Check File Exist. Replace, Rename or Cancel. ****************************************************
                            //FileInfo fileExist = new FileInfo(Server.MapPath(desFilePath) + fu_FileName.FileName);
                            //if (fileExist.Exists)
                            //{
                            //    // Display error
                            //    return;
                            //}

                            // Uploading File **********************************************************************************
                            var count = RowIndex + 1;
                            var GenFileName = drUpdating["RefNo"] + "_" + count + fileUpload.Extension;

                            fu_FileName.SaveAs(Server.MapPath(desFilePath) + GenFileName);

                            // Pending to new version
                            //// Auto Scan Virus *********************************************************************************                             
                            //string antiVirusEnginePath = "c:\\program files\\eset\\nod32.exe";
                            //string antiVirusParameter = "/clean /delete";
                            //ProcessStartInfo procStartInfo = new ProcessStartInfo();
                            //procStartInfo.FileName = antiVirusEnginePath;
                            //procStartInfo.Arguments = desFilePath + FileUpload1.FileName + " " + antiVirusParameter;
                            //procStartInfo.UseShellExecute = false;
                            //procStartInfo.RedirectStandardOutput = true;
                            //procStartInfo.RedirectStandardInput = true;
                            //procStartInfo.RedirectStandardError = true;
                            //Process proc = Process.Start(procStartInfo);
                            //proc.Close();
                            //// If uploaded file was deleted, It's a virus file.
                            //FileInfo fileUploaded = new FileInfo(desFilePath + FileUpload1.FileName);
                            //if (!fileUploaded.Exists)
                            //{
                            //    Label1.Text = FileUpload1.FileName + " may has some infect, please clening it before next upload.";
                            //    return;
                            //}                           

                            // Updating journal voucher attachment data
                            drUpdating["RefNo"] = dsAttachment.Tables["Account"].Rows[0]["AccountCode"].ToString();
                            drUpdating["FileName"] = (fu_FileName.HasFile ? fu_FileName.FileName : string.Empty);
                            drUpdating["Description"] = txt_Description.Text.Trim();
                            drUpdating["IsPublic"] = chk_IsPublic.Checked;
                            drUpdating["FilePath"] = sysParameter.GetValue("ACCon", "UploadPath", string.Empty) +
                                                     drUpdating["RefNo"] + "/" + GenFileName;

                            // Refresh data in GridView
                            grd_Attachment.DataSource = dsAttachment.Tables[accountCon.TableName];
                            grd_Attachment.EditIndex = -1;
                            grd_Attachment.DataBind();

                            // Check for submit data.
                            // Edit data from accountedit.
                            if (Mode == AttachMode.Edit)
                            {
                                // Set session for accountedit page.
                                Session["dsAccountEdit"] = dsAttachment;
                            }
                                // Edit data from account in consolidate.
                            else if (Mode == AttachMode.Display)
                            {
                                accountCon.Save(dsAttachment, string.Empty);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Update Vendor
        /// </summary>
        /// <param name="RowIndex"></param>
        private void UpdateVendor(int RowIndex)
        {
            // Updating
            // Get primary key of updating row
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            foreach (DataRow drUpdating in dsAttachment.Tables[vendorAttachment.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        var fu_FileName = (FileUpload) grd_Attachment.Rows[RowIndex].FindControl("fu_FileName");
                        var txt_Description = (TextBox) grd_Attachment.Rows[RowIndex].FindControl("txt_Description");
                        var chk_IsPublic = (CheckBox) grd_Attachment.Rows[RowIndex].FindControl("chk_IsPublic");

                        // Uploading file
                        if (fu_FileName.HasFile)
                        {
                            // Check Upload directory **************************************************************************
                            // Get desination directory
                            var desFilePath = string.Empty;

                            if (sysParameter.GetValue("Vendor", "UploadPath", LoginInfo.ConnStr) != string.Empty)
                            {
                                desFilePath = "/" + AppName +
                                              sysParameter.GetValue("Vendor", "UploadPath", LoginInfo.ConnStr) +
                                              drUpdating["RefNo"] + "/";

                                // Create destination directory if not found.
                                if (!Directory.Exists(Server.MapPath(desFilePath)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(desFilePath));
                                }
                            }

                            // Check File Size (Bytes) *************************************************************************
                            // Default max upload file size is 4MB, if want to upload the larger one, need to change at
                            // "maxRequestLength" property in machine.config.comments from 4096 to what ever you want.
                            var maxFileSize =
                                int.Parse(sysParameter.GetValue("Vendor", "UploadSizeMax", LoginInfo.ConnStr))*1024*1024;
                            if (fu_FileName.FileBytes.Length > maxFileSize)
                            {
                                // Display Error
                                return;
                            }

                            // Check File's Extension. *************************************************************************            
                            // File extension control flag description
                            // 0 : Allow all files upload.
                            // 1 : Only file's extension in list can upload.
                            // 2 : Only file's extension in list can not upload.                            
                            var fileUpload = new FileInfo(fu_FileName.PostedFile.FileName);
                            var fileExtentionControl =
                                int.Parse(sysParameter.GetValue("Vendor", "UploadFileExtentionCotrol", LoginInfo.ConnStr));
                            var fileExtensionList = sysParameter.GetValue("Vendor", "UploadFileExtensionList",
                                LoginInfo.ConnStr);

                            switch (fileExtentionControl)
                            {
                                case 1: // File's extension in list allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) < 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;

                                case 2: // File's extension in list not allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) >= 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;
                            }

                            // Pending to new version
                            // Check File Exist. Replace, Rename or Cancel. ****************************************************
                            //FileInfo fileExist = new FileInfo(Server.MapPath(desFilePath) + fu_FileName.FileName);
                            //if (fileExist.Exists)
                            //{
                            //    // Display error
                            //    return;
                            //}

                            // Uploading File **********************************************************************************
                            var count = RowIndex + 1;
                            var GenFileName = drUpdating["RefNo"] + "_" + count + fileUpload.Extension;

                            fu_FileName.SaveAs(Server.MapPath(desFilePath) + GenFileName);

                            // Pending to new version
                            //// Auto Scan Virus *********************************************************************************                             
                            //string antiVirusEnginePath = "c:\\program files\\eset\\nod32.exe";
                            //string antiVirusParameter = "/clean /delete";
                            //ProcessStartInfo procStartInfo = new ProcessStartInfo();
                            //procStartInfo.FileName = antiVirusEnginePath;
                            //procStartInfo.Arguments = desFilePath + FileUpload1.FileName + " " + antiVirusParameter;
                            //procStartInfo.UseShellExecute = false;
                            //procStartInfo.RedirectStandardOutput = true;
                            //procStartInfo.RedirectStandardInput = true;
                            //procStartInfo.RedirectStandardError = true;
                            //Process proc = Process.Start(procStartInfo);
                            //proc.Close();
                            //// If uploaded file was deleted, It's a virus file.
                            //FileInfo fileUploaded = new FileInfo(desFilePath + FileUpload1.FileName);
                            //if (!fileUploaded.Exists)
                            //{
                            //    Label1.Text = FileUpload1.FileName + " may has some infect, please clening it before next upload.";
                            //    return;
                            //}                           

                            // Updating standard voucher attachment data
                            drUpdating["RefNo"] = dsAttachment.Tables["Vendor"].Rows[0]["VendorCode"].ToString();
                            drUpdating["FileName"] = (fu_FileName.HasFile ? fu_FileName.FileName : string.Empty);
                            drUpdating["Description"] = txt_Description.Text.Trim();
                            drUpdating["IsPublic"] = chk_IsPublic.Checked;
                            drUpdating["FilePath"] = sysParameter.GetValue("Vendor", "UploadPath", LoginInfo.ConnStr) +
                                                     drUpdating["RefNo"] + "/" + GenFileName;

                            // Refresh data in GridView
                            grd_Attachment.DataSource = dsAttachment.Tables[vendorAttachment.TableName];
                            grd_Attachment.EditIndex = -1;
                            grd_Attachment.DataBind();

                            // Check for submit data.
                            // Edit data from accountedit.
                            if (Mode == AttachMode.Edit)
                            {
                                // Set session for standard voucher edit page.
                                Session["dsVendorEdit"] = dsAttachment;
                            }
                                // Edit data from budget.
                            else if (Mode == AttachMode.Display)
                            {
                                vendorAttachment.Save(dsAttachment, LoginInfo.ConnStr);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Update Payment
        /// </summary>
        /// <param name="RowIndex"></param>
        private void UpdateDebtor(int RowIndex)
        {
            // Updating
            // Get primary key of updating row
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            foreach (DataRow drUpdating in dsAttachment.Tables[debtorAttach.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        var fu_FileName = (FileUpload) grd_Attachment.Rows[RowIndex].FindControl("fu_FileName");
                        var txt_Description = (TextBox) grd_Attachment.Rows[RowIndex].FindControl("txt_Description");
                        var chk_IsPublic = (CheckBox) grd_Attachment.Rows[RowIndex].FindControl("chk_IsPublic");

                        // Uploading file
                        if (fu_FileName.HasFile)
                        {
                            // Check Upload directory **************************************************************************
                            // Get desination directory
                            var desFilePath = string.Empty;

                            if (sysParameter.GetValue("Debtor", "UploadPath", LoginInfo.ConnStr) != string.Empty)
                            {
                                desFilePath = "/" + AppName +
                                              sysParameter.GetValue("Debtor", "UploadPath", LoginInfo.ConnStr) +
                                              drUpdating["RefNo"] + "/";

                                // Create destination directory if not found.
                                if (!Directory.Exists(Server.MapPath(desFilePath)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(desFilePath));
                                }
                            }

                            // Check File Size (Bytes) *************************************************************************
                            // Default max upload file size is 4MB, if want to upload the larger one, need to change at
                            // "maxRequestLength" property in machine.config.comments from 4096 to what ever you want.
                            var maxFileSize =
                                int.Parse(sysParameter.GetValue("Debtor", "UploadSizeMax", LoginInfo.ConnStr))*1024*1024;
                            if (fu_FileName.FileBytes.Length > maxFileSize)
                            {
                                // Display Error
                                return;
                            }

                            // Check File's Extension. *************************************************************************            
                            // File extension control flag description
                            // 0 : Allow all files upload.
                            // 1 : Only file's extension in list can upload.
                            // 2 : Only file's extension in list can not upload.                            
                            var fileUpload = new FileInfo(fu_FileName.PostedFile.FileName);
                            var fileExtentionControl =
                                int.Parse(sysParameter.GetValue("Debtor", "UploadFileExtentionCotrol", LoginInfo.ConnStr));
                            var fileExtensionList = sysParameter.GetValue("Debtor", "UploadFileExtensionList",
                                LoginInfo.ConnStr);

                            switch (fileExtentionControl)
                            {
                                case 1: // File's extension in list allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) < 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;

                                case 2: // File's extension in list not allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) >= 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;
                            }

                            // Pending to new version
                            // Check File Exist. Replace, Rename or Cancel. ****************************************************
                            //FileInfo fileExist = new FileInfo(Server.MapPath(desFilePath) + fu_FileName.FileName);
                            //if (fileExist.Exists)
                            //{
                            //    // Display error
                            //    return;
                            //}

                            // Uploading File **********************************************************************************
                            var count = RowIndex + 1;
                            var GenFileName = drUpdating["RefNo"] + "_" + count + fileUpload.Extension;

                            fu_FileName.SaveAs(Server.MapPath(desFilePath) + GenFileName);

                            // Pending to new version
                            //// Auto Scan Virus *********************************************************************************                             
                            //string antiVirusEnginePath = "c:\\program files\\eset\\nod32.exe";
                            //string antiVirusParameter = "/clean /delete";
                            //ProcessStartInfo procStartInfo = new ProcessStartInfo();
                            //procStartInfo.FileName = antiVirusEnginePath;
                            //procStartInfo.Arguments = desFilePath + FileUpload1.FileName + " " + antiVirusParameter;
                            //procStartInfo.UseShellExecute = false;
                            //procStartInfo.RedirectStandardOutput = true;
                            //procStartInfo.RedirectStandardInput = true;
                            //procStartInfo.RedirectStandardError = true;
                            //Process proc = Process.Start(procStartInfo);
                            //proc.Close();
                            //// If uploaded file was deleted, It's a virus file.
                            //FileInfo fileUploaded = new FileInfo(desFilePath + FileUpload1.FileName);
                            //if (!fileUploaded.Exists)
                            //{
                            //    Label1.Text = FileUpload1.FileName + " may has some infect, please clening it before next upload.";
                            //    return;
                            //}                           

                            // Updating standard voucher attachment data
                            drUpdating["RefNo"] = dsAttachment.Tables["Debtor"].Rows[0]["DebtorCode"].ToString();
                            drUpdating["FileName"] = (fu_FileName.HasFile ? fu_FileName.FileName : string.Empty);
                            drUpdating["Description"] = txt_Description.Text.Trim();
                            drUpdating["IsPublic"] = chk_IsPublic.Checked;
                            drUpdating["FilePath"] = sysParameter.GetValue("Debtor", "UploadPath", LoginInfo.ConnStr) +
                                                     drUpdating["RefNo"] + "/" + GenFileName;

                            // Refresh data in GridView
                            grd_Attachment.DataSource = dsAttachment.Tables[debtorAttach.TableName];
                            grd_Attachment.EditIndex = -1;
                            grd_Attachment.DataBind();

                            // Check for submit data.
                            // Edit data from accountedit.
                            if (Mode == AttachMode.Edit)
                            {
                                // Set session for standard voucher edit page.
                                Session["dsDebtor"] = dsAttachment;
                            }
                                // Edit data from budget.
                            else if (Mode == AttachMode.Display)
                            {
                                debtorAttach.Save(dsAttachment, LoginInfo.ConnStr);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Update Invoice
        /// </summary>
        /// <param name="RowIndex"></param>
        private void UpdateInvoice(int RowIndex)
        {
            // Updating
            // Get primary key of updating row
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            foreach (DataRow drUpdating in dsAttachment.Tables[invoiceAttachment.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        var fu_FileName = (FileUpload) grd_Attachment.Rows[RowIndex].FindControl("fu_FileName");
                        var txt_Description = (TextBox) grd_Attachment.Rows[RowIndex].FindControl("txt_Description");
                        var chk_IsPublic = (CheckBox) grd_Attachment.Rows[RowIndex].FindControl("chk_IsPublic");

                        // Uploading file
                        if (fu_FileName.HasFile)
                        {
                            // Check Upload directory **************************************************************************
                            // Get desination directory
                            var desFilePath = string.Empty;

                            if (sysParameter.GetValue("Invoice", "UploadPath", LoginInfo.ConnStr) != string.Empty)
                            {
                                desFilePath = "/" + AppName +
                                              sysParameter.GetValue("Invoice", "UploadPath", LoginInfo.ConnStr) +
                                              drUpdating["RefNo"] + "/";

                                // Create destination directory if not found.
                                if (!Directory.Exists(Server.MapPath(desFilePath)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(desFilePath));
                                }
                            }

                            // Check File Size (Bytes) *************************************************************************
                            // Default max upload file size is 4MB, if want to upload the larger one, need to change at
                            // "maxRequestLength" property in machine.config.comments from 4096 to what ever you want.
                            var maxFileSize =
                                int.Parse(sysParameter.GetValue("Invoice", "UploadSizeMax", LoginInfo.ConnStr))*1024*
                                1024;
                            if (fu_FileName.FileBytes.Length > maxFileSize)
                            {
                                // Display Error
                                return;
                            }

                            // Check File's Extension. *************************************************************************            
                            // File extension control flag description
                            // 0 : Allow all files upload.
                            // 1 : Only file's extension in list can upload.
                            // 2 : Only file's extension in list can not upload.                            
                            var fileUpload = new FileInfo(fu_FileName.PostedFile.FileName);
                            var fileExtentionControl =
                                int.Parse(sysParameter.GetValue("Invoice", "UploadFileExtentionCotrol",
                                    LoginInfo.ConnStr));
                            var fileExtensionList = sysParameter.GetValue("Invoice", "UploadFileExtensionList",
                                LoginInfo.ConnStr);

                            switch (fileExtentionControl)
                            {
                                case 1: // File's extension in list allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) < 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;

                                case 2: // File's extension in list not allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) >= 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;
                            }

                            // Pending to new version
                            // Check File Exist. Replace, Rename or Cancel. ****************************************************
                            //FileInfo fileExist = new FileInfo(Server.MapPath(desFilePath) + fu_FileName.FileName);
                            //if (fileExist.Exists)
                            //{
                            //    // Display error
                            //    return;
                            //}

                            // Uploading File **********************************************************************************
                            var count = RowIndex + 1;
                            var GenFileName = drUpdating["RefNo"] + "_" + count + fileUpload.Extension;

                            fu_FileName.SaveAs(Server.MapPath(desFilePath) + GenFileName);

                            // Pending to new version
                            //// Auto Scan Virus *********************************************************************************                             
                            //string antiVirusEnginePath = "c:\\program files\\eset\\nod32.exe";
                            //string antiVirusParameter = "/clean /delete";
                            //ProcessStartInfo procStartInfo = new ProcessStartInfo();
                            //procStartInfo.FileName = antiVirusEnginePath;
                            //procStartInfo.Arguments = desFilePath + FileUpload1.FileName + " " + antiVirusParameter;
                            //procStartInfo.UseShellExecute = false;
                            //procStartInfo.RedirectStandardOutput = true;
                            //procStartInfo.RedirectStandardInput = true;
                            //procStartInfo.RedirectStandardError = true;
                            //Process proc = Process.Start(procStartInfo);
                            //proc.Close();
                            //// If uploaded file was deleted, It's a virus file.
                            //FileInfo fileUploaded = new FileInfo(desFilePath + FileUpload1.FileName);
                            //if (!fileUploaded.Exists)
                            //{
                            //    Label1.Text = FileUpload1.FileName + " may has some infect, please clening it before next upload.";
                            //    return;
                            //}                           

                            // Updating standard voucher attachment data
                            drUpdating["RefNo"] = dsAttachment.Tables["Invoice"].Rows[0]["VoucherNo"].ToString();
                            drUpdating["FileName"] = (fu_FileName.HasFile ? fu_FileName.FileName : string.Empty);
                            drUpdating["Description"] = txt_Description.Text.Trim();
                            drUpdating["IsPublic"] = chk_IsPublic.Checked;
                            drUpdating["FilePath"] = sysParameter.GetValue("Invoice", "UploadPath", LoginInfo.ConnStr) +
                                                     drUpdating["RefNo"] + "/" + GenFileName;

                            // Refresh data in GridView
                            grd_Attachment.DataSource = dsAttachment.Tables[invoiceAttachment.TableName];
                            grd_Attachment.EditIndex = -1;
                            grd_Attachment.DataBind();

                            // Check for submit data.
                            // Edit data from accountedit.
                            if (Mode == AttachMode.Edit)
                            {
                                // Set session for standard voucher edit page.
                                Session["dsInvoiceEdit"] = dsAttachment;
                            }
                                // Edit data from budget.
                            else if (Mode == AttachMode.Display)
                            {
                                invoiceAttachment.Save(dsAttachment, LoginInfo.ConnStr);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Update Payment
        /// </summary>
        /// <param name="RowIndex"></param>
        private void UpdatePayment(int RowIndex)
        {
            // Updating
            // Get primary key of updating row
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            foreach (DataRow drUpdating in dsAttachment.Tables[paymentAttachment.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        var fu_FileName = (FileUpload) grd_Attachment.Rows[RowIndex].FindControl("fu_FileName");
                        var txt_Description = (TextBox) grd_Attachment.Rows[RowIndex].FindControl("txt_Description");
                        var chk_IsPublic = (CheckBox) grd_Attachment.Rows[RowIndex].FindControl("chk_IsPublic");

                        // Uploading file
                        if (fu_FileName.HasFile)
                        {
                            // Check Upload directory **************************************************************************
                            // Get desination directory
                            var desFilePath = string.Empty;

                            if (sysParameter.GetValue("Payment", "UploadPath", LoginInfo.ConnStr) != string.Empty)
                            {
                                desFilePath = "/" + AppName +
                                              sysParameter.GetValue("Payment", "UploadPath", LoginInfo.ConnStr) +
                                              drUpdating["RefNo"] + "/";

                                // Create destination directory if not found.
                                if (!Directory.Exists(Server.MapPath(desFilePath)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(desFilePath));
                                }
                            }

                            // Check File Size (Bytes) *************************************************************************
                            // Default max upload file size is 4MB, if want to upload the larger one, need to change at
                            // "maxRequestLength" property in machine.config.comments from 4096 to what ever you want.
                            var maxFileSize =
                                int.Parse(sysParameter.GetValue("Payment", "UploadSizeMax", LoginInfo.ConnStr))*1024*
                                1024;
                            if (fu_FileName.FileBytes.Length > maxFileSize)
                            {
                                // Display Error
                                return;
                            }

                            // Check File's Extension. *************************************************************************            
                            // File extension control flag description
                            // 0 : Allow all files upload.
                            // 1 : Only file's extension in list can upload.
                            // 2 : Only file's extension in list can not upload.                            
                            var fileUpload = new FileInfo(fu_FileName.PostedFile.FileName);
                            var fileExtentionControl =
                                int.Parse(sysParameter.GetValue("Payment", "UploadFileExtentionCotrol",
                                    LoginInfo.ConnStr));
                            var fileExtensionList = sysParameter.GetValue("Payment", "UploadFileExtensionList",
                                LoginInfo.ConnStr);

                            switch (fileExtentionControl)
                            {
                                case 1: // File's extension in list allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) < 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;

                                case 2: // File's extension in list not allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) >= 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;
                            }

                            // Pending to new version
                            // Check File Exist. Replace, Rename or Cancel. ****************************************************
                            //FileInfo fileExist = new FileInfo(Server.MapPath(desFilePath) + fu_FileName.FileName);
                            //if (fileExist.Exists)
                            //{
                            //    // Display error
                            //    return;
                            //}

                            // Uploading File **********************************************************************************
                            var count = RowIndex + 1;
                            var GenFileName = drUpdating["RefNo"] + "_" + count + fileUpload.Extension;

                            fu_FileName.SaveAs(Server.MapPath(desFilePath) + GenFileName);

                            // Pending to new version
                            //// Auto Scan Virus *********************************************************************************                             
                            //string antiVirusEnginePath = "c:\\program files\\eset\\nod32.exe";
                            //string antiVirusParameter = "/clean /delete";
                            //ProcessStartInfo procStartInfo = new ProcessStartInfo();
                            //procStartInfo.FileName = antiVirusEnginePath;
                            //procStartInfo.Arguments = desFilePath + FileUpload1.FileName + " " + antiVirusParameter;
                            //procStartInfo.UseShellExecute = false;
                            //procStartInfo.RedirectStandardOutput = true;
                            //procStartInfo.RedirectStandardInput = true;
                            //procStartInfo.RedirectStandardError = true;
                            //Process proc = Process.Start(procStartInfo);
                            //proc.Close();
                            //// If uploaded file was deleted, It's a virus file.
                            //FileInfo fileUploaded = new FileInfo(desFilePath + FileUpload1.FileName);
                            //if (!fileUploaded.Exists)
                            //{
                            //    Label1.Text = FileUpload1.FileName + " may has some infect, please clening it before next upload.";
                            //    return;
                            //}                           

                            // Updating standard voucher attachment data
                            drUpdating["RefNo"] = dsAttachment.Tables["Payment"].Rows[0]["VoucherNo"].ToString();
                            drUpdating["FileName"] = (fu_FileName.HasFile ? fu_FileName.FileName : string.Empty);
                            drUpdating["Description"] = txt_Description.Text.Trim();
                            drUpdating["IsPublic"] = chk_IsPublic.Checked;
                            drUpdating["FilePath"] = sysParameter.GetValue("Payment", "UploadPath", LoginInfo.ConnStr) +
                                                     drUpdating["RefNo"] + "/" + GenFileName;

                            // Refresh data in GridView
                            grd_Attachment.DataSource = dsAttachment.Tables[paymentAttachment.TableName];
                            grd_Attachment.EditIndex = -1;
                            grd_Attachment.DataBind();

                            // Check for submit data.
                            // Edit data from accountedit.
                            if (Mode == AttachMode.Edit)
                            {
                                // Set session for standard voucher edit page.
                                Session["dsPaymentEdit"] = dsAttachment;
                            }
                                // Edit data from budget.
                            else if (Mode == AttachMode.Display)
                            {
                                paymentAttachment.Save(dsAttachment, LoginInfo.ConnStr);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Update Receipt
        /// </summary>
        /// <param name="RowIndex"></param>
        private void UpdateReceipt(int RowIndex)
        {
            // Updating
            // Get primary key of updating row
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            foreach (DataRow drUpdating in dsAttachment.Tables[receiptAttach.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        var fu_FileName = (FileUpload) grd_Attachment.Rows[RowIndex].FindControl("fu_FileName");
                        var txt_Description = (TextBox) grd_Attachment.Rows[RowIndex].FindControl("txt_Description");
                        var chk_IsPublic = (CheckBox) grd_Attachment.Rows[RowIndex].FindControl("chk_IsPublic");

                        // Uploading file
                        if (fu_FileName.HasFile)
                        {
                            // Check Upload directory **************************************************************************
                            // Get desination directory
                            var desFilePath = string.Empty;

                            if (sysParameter.GetValue("Receipt", "UploadPath", LoginInfo.ConnStr) != string.Empty)
                            {
                                desFilePath = "/" + AppName +
                                              sysParameter.GetValue("Receipt", "UploadPath", LoginInfo.ConnStr) +
                                              drUpdating["RefNo"] + "/";

                                // Create destination directory if not found.
                                if (!Directory.Exists(Server.MapPath(desFilePath)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(desFilePath));
                                }
                            }

                            // Check File Size (Bytes) *************************************************************************
                            // Default max upload file size is 4MB, if want to upload the larger one, need to change at
                            // "maxRequestLength" property in machine.config.comments from 4096 to what ever you want.
                            var maxFileSize =
                                int.Parse(sysParameter.GetValue("Receipt", "UploadSizeMax", LoginInfo.ConnStr))*1024*
                                1024;
                            if (fu_FileName.FileBytes.Length > maxFileSize)
                            {
                                // Display Error
                                return;
                            }

                            // Check File's Extension. *************************************************************************            
                            // File extension control flag description
                            // 0 : Allow all files upload.
                            // 1 : Only file's extension in list can upload.
                            // 2 : Only file's extension in list can not upload.                            
                            var fileUpload = new FileInfo(fu_FileName.PostedFile.FileName);
                            var fileExtentionControl =
                                int.Parse(sysParameter.GetValue("Receipt", "UploadFileExtentionCotrol",
                                    LoginInfo.ConnStr));
                            var fileExtensionList = sysParameter.GetValue("Receipt", "UploadFileExtensionList",
                                LoginInfo.ConnStr);

                            switch (fileExtentionControl)
                            {
                                case 1: // File's extension in list allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) < 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;

                                case 2: // File's extension in list not allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) >= 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;
                            }

                            // Pending to new version
                            // Check File Exist. Replace, Rename or Cancel. ****************************************************
                            //FileInfo fileExist = new FileInfo(Server.MapPath(desFilePath) + fu_FileName.FileName);
                            //if (fileExist.Exists)
                            //{
                            //    // Display error
                            //    return;
                            //}

                            // Uploading File **********************************************************************************
                            var count = RowIndex + 1;
                            var GenFileName = drUpdating["RefNo"] + "_" + count + fileUpload.Extension;

                            fu_FileName.SaveAs(Server.MapPath(desFilePath) + GenFileName);

                            // Pending to new version
                            //// Auto Scan Virus *********************************************************************************                             
                            //string antiVirusEnginePath = "c:\\program files\\eset\\nod32.exe";
                            //string antiVirusParameter = "/clean /delete";
                            //ProcessStartInfo procStartInfo = new ProcessStartInfo();
                            //procStartInfo.FileName = antiVirusEnginePath;
                            //procStartInfo.Arguments = desFilePath + FileUpload1.FileName + " " + antiVirusParameter;
                            //procStartInfo.UseShellExecute = false;
                            //procStartInfo.RedirectStandardOutput = true;
                            //procStartInfo.RedirectStandardInput = true;
                            //procStartInfo.RedirectStandardError = true;
                            //Process proc = Process.Start(procStartInfo);
                            //proc.Close();
                            //// If uploaded file was deleted, It's a virus file.
                            //FileInfo fileUploaded = new FileInfo(desFilePath + FileUpload1.FileName);
                            //if (!fileUploaded.Exists)
                            //{
                            //    Label1.Text = FileUpload1.FileName + " may has some infect, please clening it before next upload.";
                            //    return;
                            //}                           

                            // Updating standard voucher attachment data
                            drUpdating["RefNo"] = dsAttachment.Tables["Receipt"].Rows[0]["ReceiptNo"].ToString();
                            drUpdating["FileName"] = (fu_FileName.HasFile ? fu_FileName.FileName : string.Empty);
                            drUpdating["Description"] = txt_Description.Text.Trim();
                            drUpdating["IsPublic"] = chk_IsPublic.Checked;
                            drUpdating["FilePath"] = sysParameter.GetValue("Receipt", "UploadPath", LoginInfo.ConnStr) +
                                                     drUpdating["RefNo"] + "/" + GenFileName;

                            // Refresh data in GridView
                            grd_Attachment.DataSource = dsAttachment.Tables[receiptAttach.TableName];
                            grd_Attachment.EditIndex = -1;
                            grd_Attachment.DataBind();

                            // Check for submit data.
                            // Edit data from accountedit.
                            if (Mode == AttachMode.Edit)
                            {
                                // Set session for standard voucher edit page.
                                Session["dsReceiptEdit"] = dsAttachment;
                            }
                                // Edit data from budget.
                            else if (Mode == AttachMode.Display)
                            {
                                receiptAttach.Save(dsAttachment, LoginInfo.ConnStr);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Update Account Reconcile
        /// </summary>
        /// <param name="RowIndex"></param>
        private void UpdateAccountReconcile(int RowIndex)
        {
            // Updating
            // Get primary key of updating row
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            foreach (DataRow drUpdating in dsAttachment.Tables[accRecAttach.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        var fu_FileName = (FileUpload) grd_Attachment.Rows[RowIndex].FindControl("fu_FileName");
                        var txt_Description = (TextBox) grd_Attachment.Rows[RowIndex].FindControl("txt_Description");
                        var chk_IsPublic = (CheckBox) grd_Attachment.Rows[RowIndex].FindControl("chk_IsPublic");

                        // Uploading file
                        if (fu_FileName.HasFile)
                        {
                            // Check Upload directory **************************************************************************
                            // Get desination directory
                            var desFilePath = string.Empty;

                            if (sysParameter.GetValue("AccountReconcile", "UploadPath", LoginInfo.ConnStr) !=
                                string.Empty)
                            {
                                desFilePath = "/" + AppName +
                                              sysParameter.GetValue("AccountReconcile", "UploadPath", LoginInfo.ConnStr) +
                                              drUpdating["RefNo"] + "/";

                                // Create destination directory if not found.
                                if (!Directory.Exists(Server.MapPath(desFilePath)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(desFilePath));
                                }
                            }

                            // Check File Size (Bytes) *************************************************************************
                            // Default max upload file size is 4MB, if want to upload the larger one, need to change at
                            // "maxRequestLength" property in machine.config.comments from 4096 to what ever you want.
                            var maxFileSize =
                                int.Parse(sysParameter.GetValue("AccountReconcile", "UploadSizeMax", LoginInfo.ConnStr))*
                                1024*1024;
                            if (fu_FileName.FileBytes.Length > maxFileSize)
                            {
                                // Display Error
                                return;
                            }

                            // Check File's Extension. *************************************************************************            
                            // File extension control flag description
                            // 0 : Allow all files upload.
                            // 1 : Only file's extension in list can upload.
                            // 2 : Only file's extension in list can not upload.                            
                            var fileUpload = new FileInfo(fu_FileName.PostedFile.FileName);
                            var fileExtentionControl =
                                int.Parse(sysParameter.GetValue("AccountReconcile", "UploadFileExtentionCotrol",
                                    LoginInfo.ConnStr));
                            var fileExtensionList = sysParameter.GetValue("AccountReconcile", "UploadFileExtensionList",
                                LoginInfo.ConnStr);

                            switch (fileExtentionControl)
                            {
                                case 1: // File's extension in list allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) < 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;

                                case 2: // File's extension in list not allow to upload.
                                    if (fileExtensionList.ToLower().IndexOf(fileUpload.Extension.ToLower()) >= 0)
                                    {
                                        // Display Error
                                        return;
                                    }
                                    break;
                            }

                            // Pending to new version
                            // Check File Exist. Replace, Rename or Cancel. ****************************************************
                            //FileInfo fileExist = new FileInfo(Server.MapPath(desFilePath) + fu_FileName.FileName);
                            //if (fileExist.Exists)
                            //{
                            //    // Display error
                            //    return;
                            //}

                            // Uploading File **********************************************************************************
                            var count = RowIndex + 1;
                            var GenFileName = drUpdating["RefNo"] + "_" + count + fileUpload.Extension;

                            fu_FileName.SaveAs(Server.MapPath(desFilePath) + GenFileName);

                            // Pending to new version
                            //// Auto Scan Virus *********************************************************************************                             
                            //string antiVirusEnginePath = "c:\\program files\\eset\\nod32.exe";
                            //string antiVirusParameter = "/clean /delete";
                            //ProcessStartInfo procStartInfo = new ProcessStartInfo();
                            //procStartInfo.FileName = antiVirusEnginePath;
                            //procStartInfo.Arguments = desFilePath + FileUpload1.FileName + " " + antiVirusParameter;
                            //procStartInfo.UseShellExecute = false;
                            //procStartInfo.RedirectStandardOutput = true;
                            //procStartInfo.RedirectStandardInput = true;
                            //procStartInfo.RedirectStandardError = true;
                            //Process proc = Process.Start(procStartInfo);
                            //proc.Close();
                            //// If uploaded file was deleted, It's a virus file.
                            //FileInfo fileUploaded = new FileInfo(desFilePath + FileUpload1.FileName);
                            //if (!fileUploaded.Exists)
                            //{
                            //    Label1.Text = FileUpload1.FileName + " may has some infect, please clening it before next upload.";
                            //    return;
                            //}                           

                            // Updating standard voucher attachment data
                            drUpdating["RefNo"] = dsAttachment.Tables["AccountReconcile"].Rows[0]["AccRecNo"].ToString();
                            drUpdating["FileName"] = (fu_FileName.HasFile ? fu_FileName.FileName : string.Empty);
                            drUpdating["Description"] = txt_Description.Text.Trim();
                            drUpdating["IsPublic"] = chk_IsPublic.Checked;
                            drUpdating["FilePath"] =
                                sysParameter.GetValue("AccountReconcile", "UploadPath", LoginInfo.ConnStr) +
                                drUpdating["RefNo"] + "/" + GenFileName;

                            // Refresh data in GridView
                            grd_Attachment.DataSource = dsAttachment.Tables[accRecAttach.TableName];
                            grd_Attachment.EditIndex = -1;
                            grd_Attachment.DataBind();

                            // Check for submit data.
                            // Edit data from accountedit.
                            if (Mode == AttachMode.Edit)
                            {
                                // Set session for standard voucher edit page.
                                Session["dsAccRecEdit"] = dsAttachment;
                            }
                                // Edit data from budget.
                            else if (Mode == AttachMode.Display)
                            {
                                accRecAttach.Save(dsAttachment, LoginInfo.ConnStr);
                            }
                        }
                    }
                }
            }
        }

        //------------ DELETE ---------------------------
        /// <summary>
        ///     Delete journal vourcher table
        /// </summary>
        private void DeleteJV(int RowIndex)
        {
            // Get journalVoucherDetailID from deleted row            
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete the record 
            for (var i = dsAttachment.Tables[journalVoucherAttachment.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drJournalVoucherAttachment = dsAttachment.Tables[journalVoucherAttachment.TableName].Rows[i];

                if (drJournalVoucherAttachment.RowState != DataRowState.Deleted)
                {
                    if (drJournalVoucherAttachment["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        // Delete attachment file if RowState is added.
                        if (drJournalVoucherAttachment.RowState == DataRowState.Added)
                        {
                            var fileInfo =
                                new FileInfo(Server.MapPath("/" + AppName + drJournalVoucherAttachment["FilePath"]));

                            if (fileInfo.Exists)
                            {
                                fileInfo.Delete();
                            }
                        }

                        drJournalVoucherAttachment.Delete();
                    }
                }
            }

            // Binding grid
            grd_Attachment.DataSource = dsAttachment.Tables[journalVoucherAttachment.TableName];
            grd_Attachment.DataBind();

            // Edit data from journalvourcheredit page.
            if (Mode == AttachMode.Edit)
            {
                // Set session for journalvourcheredit page.
                Session["dsJournalVourcherEdit"] = dsAttachment;
            }
                // Edit data from journalvourcher page.
            else if (Mode == AttachMode.Display)
            {
                journalVoucherAttachment.Save(dsAttachment, LoginInfo.ConnStr);
            }
        }

        /// <summary>
        ///     Delete account attachment table
        /// </summary>
        /// <param name="RowIndex"></param>
        private void DeleteAcc(int RowIndex)
        {
            // Get account from deleted row            
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete the record 
            for (var i = dsAttachment.Tables[accountAttachment.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drAccountAttachment = dsAttachment.Tables[accountAttachment.TableName].Rows[i];

                if (drAccountAttachment.RowState != DataRowState.Deleted)
                {
                    if (drAccountAttachment["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        // Delete attachment file if RowState is added.
                        if (drAccountAttachment.RowState == DataRowState.Added)
                        {
                            var fileInfo = new FileInfo(Server.MapPath("/" + AppName + drAccountAttachment["FilePath"]));

                            if (fileInfo.Exists)
                            {
                                fileInfo.Delete();
                            }
                        }

                        drAccountAttachment.Delete();
                    }
                }
            }

            // Binding grid
            grd_Attachment.DataSource = dsAttachment.Tables[accountAttachment.TableName];
            grd_Attachment.DataBind();

            // Edit data from account edit page.
            if (Mode == AttachMode.Edit)
            {
                // Set session for account edit page.
                Session["dsAccountEdit"] = dsAttachment;
            }
                // Edit data from account page.
            else if (Mode == AttachMode.Display)
            {
                accountAttachment.Save(dsAttachment, LoginInfo.ConnStr);
            }
        }

        /// <summary>
        ///     Delete budget table
        /// </summary>
        /// <param name="RowIndex"></param>
        private void DeleteBG(int RowIndex)
        {
            // Get BudgetDetailID from deleted row            
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete the record 
            for (var i = dsAttachment.Tables[budgetAttachment.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drBudgetAttachment = dsAttachment.Tables[budgetAttachment.TableName].Rows[i];

                if (drBudgetAttachment.RowState != DataRowState.Deleted)
                {
                    if (drBudgetAttachment["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        // Delete attachment file if RowState is added.
                        if (drBudgetAttachment.RowState == DataRowState.Added)
                        {
                            var fileInfo = new FileInfo(Server.MapPath("/" + AppName + drBudgetAttachment["FilePath"]));

                            if (fileInfo.Exists)
                            {
                                fileInfo.Delete();
                            }
                        }

                        drBudgetAttachment.Delete();
                    }
                }
            }

            // Binding grid
            grd_Attachment.DataSource = dsAttachment.Tables[budgetAttachment.TableName];
            grd_Attachment.DataBind();

            // Edit data from budget edit page.
            if (Mode == AttachMode.Edit)
            {
                // Set session for budget edit page.
                Session["dsBudgetEdit"] = dsAttachment;
            }
                // Edit data from budget page.
            else if (Mode == AttachMode.Display)
            {
                budgetAttachment.Save(dsAttachment, LoginInfo.ConnStr);
            }
        }

        /// <summary>
        ///     Delete standart voucher table
        /// </summary>
        /// <param name="RowIndex"></param>
        private void DeleteSV(int RowIndex)
        {
            // Get StandardVoucherID from deleted row            
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete the record 
            for (var i = dsAttachment.Tables[standardVoucherAttachment.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drStandardVoucherAttachment = dsAttachment.Tables[standardVoucherAttachment.TableName].Rows[i];

                if (drStandardVoucherAttachment.RowState != DataRowState.Deleted)
                {
                    if (drStandardVoucherAttachment["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        // Delete attachment file if RowState is added.
                        if (drStandardVoucherAttachment.RowState == DataRowState.Added)
                        {
                            var fileInfo =
                                new FileInfo(Server.MapPath("/" + AppName + drStandardVoucherAttachment["FilePath"]));

                            if (fileInfo.Exists)
                            {
                                fileInfo.Delete();
                            }
                        }

                        drStandardVoucherAttachment.Delete();
                    }
                }
            }

            // Binding grid
            grd_Attachment.DataSource = dsAttachment.Tables[standardVoucherAttachment.TableName];
            grd_Attachment.DataBind();

            // Edit data from standardVoucher edit page.
            if (Mode == AttachMode.Edit)
            {
                // Set session for budget edit page.
                Session["dsStandardVoucherEdit"] = dsAttachment;
            }
                // Edit data from budget page.
            else if (Mode == AttachMode.Display)
            {
                standardVoucherAttachment.Save(dsAttachment, LoginInfo.ConnStr);
            }
        }

        /// <summary>
        ///     Delete account attachment table at consolidate.
        /// </summary>
        /// <param name="RowIndex"></param>
        private void DeleteAC(int RowIndex)
        {
            // Get account from deleted row            
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete the record 
            for (var i = dsAttachment.Tables[accountCon.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drAccountCon = dsAttachment.Tables[accountCon.TableName].Rows[i];

                if (drAccountCon.RowState != DataRowState.Deleted)
                {
                    if (drAccountCon["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        // Delete attachment file if RowState is added.
                        if (drAccountCon.RowState == DataRowState.Added)
                        {
                            var fileInfo = new FileInfo(Server.MapPath("/" + AppName + drAccountCon["FilePath"]));

                            if (fileInfo.Exists)
                            {
                                fileInfo.Delete();
                            }
                        }

                        drAccountCon.Delete();
                    }
                }
            }

            // Binding grid
            grd_Attachment.DataSource = dsAttachment.Tables[accountCon.TableName];
            grd_Attachment.DataBind();

            // Edit data from account edit page.
            if (Mode == AttachMode.Edit)
            {
                // Set session for account edit page.
                Session["dsAccountEdit"] = dsAttachment;
            }
                // Edit data from account page.
            else if (Mode == AttachMode.Display)
            {
                accountCon.Save(dsAttachment, string.Empty);
            }
        }

        /// <summary>
        ///     Delete Vendor
        /// </summary>
        /// <param name="RowIndex"></param>
        private void DeleteVendor(int RowIndex)
        {
            // Get StandardVoucherID from deleted row            
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete the record 
            for (var i = dsAttachment.Tables[vendorAttachment.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drVendorAttachment = dsAttachment.Tables[vendorAttachment.TableName].Rows[i];

                if (drVendorAttachment.RowState != DataRowState.Deleted)
                {
                    if (drVendorAttachment["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        // Delete attachment file if RowState is added.
                        if (drVendorAttachment.RowState == DataRowState.Added)
                        {
                            var fileInfo = new FileInfo(Server.MapPath("/" + AppName + drVendorAttachment["FilePath"]));

                            if (fileInfo.Exists)
                            {
                                fileInfo.Delete();
                            }
                        }

                        drVendorAttachment.Delete();
                    }
                }
            }

            // Binding grid
            grd_Attachment.DataSource = dsAttachment.Tables[vendorAttachment.TableName];
            grd_Attachment.DataBind();

            // Edit data from standardVoucher edit page.
            if (Mode == AttachMode.Edit)
            {
                // Set session for budget edit page.
                Session["dsVendorEdit"] = dsAttachment;
            }
                // Edit data from budget page.
            else if (Mode == AttachMode.Display)
            {
                vendorAttachment.Save(dsAttachment, LoginInfo.ConnStr);
            }
        }

        /// <summary>
        ///     Delete Debtor
        /// </summary>
        /// <param name="RowIndex"></param>
        private void DeleteDebtor(int RowIndex)
        {
            // Get StandardVoucherID from deleted row            
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete the record             
            for (var i = dsAttachment.Tables[debtorAttach.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drDebtorAttachment = dsAttachment.Tables[debtorAttach.TableName].Rows[i];

                if (drDebtorAttachment.RowState != DataRowState.Deleted)
                {
                    if (drDebtorAttachment["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        // Delete attachment file if RowState is added.
                        if (drDebtorAttachment.RowState == DataRowState.Added)
                        {
                            var fileInfo = new FileInfo(Server.MapPath("/" + AppName + drDebtorAttachment["FilePath"]));

                            if (fileInfo.Exists)
                            {
                                fileInfo.Delete();
                            }
                        }

                        drDebtorAttachment.Delete();
                    }
                }
            }

            // Binding grid
            grd_Attachment.DataSource = dsAttachment.Tables[debtorAttach.TableName];
            grd_Attachment.DataBind();

            // Edit data from standardVoucher edit page.
            if (Mode == AttachMode.Edit)
            {
                // Set session for budget edit page.
                Session["dsDebtor"] = dsAttachment;
            }
                // Edit data from budget page.
            else if (Mode == AttachMode.Display)
            {
                debtorAttach.Save(dsAttachment, LoginInfo.ConnStr);
            }
        }

        /// <summary>
        ///     Delete Invoice
        /// </summary>
        /// <param name="RowIndex"></param>
        private void DeleteInvoice(int RowIndex)
        {
            // Get StandardVoucherID from deleted row            
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete the record 
            for (var i = dsAttachment.Tables[invoiceAttachment.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drinvoiceAttachment = dsAttachment.Tables[invoiceAttachment.TableName].Rows[i];

                if (drinvoiceAttachment.RowState != DataRowState.Deleted)
                {
                    if (drinvoiceAttachment["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        // Delete attachment file if RowState is added.
                        if (drinvoiceAttachment.RowState == DataRowState.Added)
                        {
                            var fileInfo = new FileInfo(Server.MapPath("/" + AppName + drinvoiceAttachment["FilePath"]));

                            if (fileInfo.Exists)
                            {
                                fileInfo.Delete();
                            }
                        }

                        drinvoiceAttachment.Delete();
                    }
                }
            }

            // Binding grid
            grd_Attachment.DataSource = dsAttachment.Tables[invoiceAttachment.TableName];
            grd_Attachment.DataBind();

            // Edit data from standardVoucher edit page.
            if (Mode == AttachMode.Edit)
            {
                // Set session for budget edit page.
                Session["dsInvoiceEdit"] = dsAttachment;
            }
                // Edit data from budget page.
            else if (Mode == AttachMode.Display)
            {
                invoiceAttachment.Save(dsAttachment, LoginInfo.ConnStr);
            }
        }

        /// <summary>
        ///     Delete Payment
        /// </summary>
        /// <param name="RowIndex"></param>
        private void DeletePayment(int RowIndex)
        {
            // Get StandardVoucherID from deleted row            
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete the record 
            for (var i = dsAttachment.Tables[paymentAttachment.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drPaymentAttachment = dsAttachment.Tables[paymentAttachment.TableName].Rows[i];

                if (drPaymentAttachment.RowState != DataRowState.Deleted)
                {
                    if (drPaymentAttachment["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        // Delete attachment file if RowState is added.
                        if (drPaymentAttachment.RowState == DataRowState.Added)
                        {
                            var fileInfo = new FileInfo(Server.MapPath("/" + AppName + drPaymentAttachment["FilePath"]));

                            if (fileInfo.Exists)
                            {
                                fileInfo.Delete();
                            }
                        }

                        drPaymentAttachment.Delete();
                    }
                }
            }

            // Binding grid
            grd_Attachment.DataSource = dsAttachment.Tables[paymentAttachment.TableName];
            grd_Attachment.DataBind();

            // Edit data from standardVoucher edit page.
            if (Mode == AttachMode.Edit)
            {
                // Set session for budget edit page.
                Session["dsPaymentEdit"] = dsAttachment;
            }
                // Edit data from budget page.
            else if (Mode == AttachMode.Display)
            {
                paymentAttachment.Save(dsAttachment, LoginInfo.ConnStr);
            }
        }

        /// <summary>
        ///     Delete Payment
        /// </summary>
        /// <param name="RowIndex"></param>
        private void DeleteReceipt(int RowIndex)
        {
            // Get StandardVoucherID from deleted row            
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete the record             
            for (var i = dsAttachment.Tables[receiptAttach.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drReceiptAttachment = dsAttachment.Tables[receiptAttach.TableName].Rows[i];

                if (drReceiptAttachment.RowState != DataRowState.Deleted)
                {
                    if (drReceiptAttachment["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        // Delete attachment file if RowState is added.
                        if (drReceiptAttachment.RowState == DataRowState.Added)
                        {
                            var fileInfo = new FileInfo(Server.MapPath("/" + AppName + drReceiptAttachment["FilePath"]));

                            if (fileInfo.Exists)
                            {
                                fileInfo.Delete();
                            }
                        }

                        drReceiptAttachment.Delete();
                    }
                }
            }

            // Binding grid
            grd_Attachment.DataSource = dsAttachment.Tables[receiptAttach.TableName];
            grd_Attachment.DataBind();

            // Edit data from standardVoucher edit page.
            if (Mode == AttachMode.Edit)
            {
                // Set session for budget edit page.
                Session["dsReceiptEdit"] = dsAttachment;
            }
                // Edit data from budget page.
            else if (Mode == AttachMode.Display)
            {
                receiptAttach.Save(dsAttachment, LoginInfo.ConnStr);
            }
        }

        /// <summary>
        ///     Delete Account Reconcile
        /// </summary>
        /// <param name="RowIndex"></param>
        private void DeleteAccountReconcile(int RowIndex)
        {
            // Get StandardVoucherID from deleted row            
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete the record             
            for (var i = dsAttachment.Tables[accRecAttach.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drAccRecAttachment = dsAttachment.Tables[accRecAttach.TableName].Rows[i];

                if (drAccRecAttachment.RowState != DataRowState.Deleted)
                {
                    if (drAccRecAttachment["ID"].ToString().ToUpper() == id.ToUpper())
                    {
                        // Delete attachment file if RowState is added.
                        if (drAccRecAttachment.RowState == DataRowState.Added)
                        {
                            var fileInfo = new FileInfo(Server.MapPath("/" + AppName + drAccRecAttachment["FilePath"]));

                            if (fileInfo.Exists)
                            {
                                fileInfo.Delete();
                            }
                        }

                        drAccRecAttachment.Delete();
                    }
                }
            }

            // Binding grid
            grd_Attachment.DataSource = dsAttachment.Tables[accRecAttach.TableName];
            grd_Attachment.DataBind();

            // Edit data from accRecAttach edit page.
            if (Mode == AttachMode.Edit)
            {
                // Set session for budget edit page.
                Session["dsAccRecEdit"] = dsAttachment;
            }
                // Edit data from budget page.
            else if (Mode == AttachMode.Display)
            {
                accRecAttach.Save(dsAttachment, LoginInfo.ConnStr);
            }
        }

        //------------ CANCLE ---------------------------
        /// <summary>
        ///     Cancel adding journal voucher attachment data.
        /// </summary>
        private void CanclingEditJV(int RowIndex)
        {
            // Setup primary key(s)
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete new row
            for (var i = dsAttachment.Tables[journalVoucherAttachment.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = dsAttachment.Tables[journalVoucherAttachment.TableName].Rows[i];

                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["ID"].ToString().ToUpper() == id.ToUpper() && dr.RowState == DataRowState.Added)
                    {
                        dr.Delete();
                    }
                }
            }

            // Refresh data in gridview
            grd_Attachment.DataSource = dsAttachment.Tables[journalVoucherAttachment.TableName];
            //grd_Attachment.EditIndex = -1;
            //grd_Attachment.DataBind();

            //// Save to session
            //Session["dsAttachment"] = dsAttachment;
        }

        /// <summary>
        ///     Cancel adding account attachment data.
        /// </summary>
        private void CanclingEditAcc(int RowIndex)
        {
            // Setup primary key(s)
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete new row
            for (var i = dsAttachment.Tables[accountAttachment.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = dsAttachment.Tables[accountAttachment.TableName].Rows[i];

                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["ID"].ToString().ToUpper() == id.ToUpper() && dr.RowState == DataRowState.Added)
                    {
                        dr.Delete();
                    }
                }
            }

            // Refresh data in gridview
            grd_Attachment.DataSource = dsAttachment.Tables[accountAttachment.TableName];
        }

        /// <summary>
        ///     Cancel adding budget attachment data.
        /// </summary>
        private void CanclingEditBG(int RowIndex)
        {
            // Setup primary key(s)
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete new row
            for (var i = dsAttachment.Tables[budgetAttachment.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = dsAttachment.Tables[budgetAttachment.TableName].Rows[i];

                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["ID"].ToString().ToUpper() == id.ToUpper() && dr.RowState == DataRowState.Added)
                    {
                        dr.Delete();
                    }
                }
            }

            // Refresh data in gridview
            grd_Attachment.DataSource = dsAttachment.Tables[budgetAttachment.TableName];
        }

        /// <summary>
        ///     Cancel adding standart voucher attachment data.
        /// </summary>
        /// <param name="RowIndex"></param>
        private void CanclingEditSV(int RowIndex)
        {
            // Setup primary key(s)
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete new row
            for (var i = dsAttachment.Tables[standardVoucherAttachment.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = dsAttachment.Tables[standardVoucherAttachment.TableName].Rows[i];

                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["ID"].ToString().ToUpper() == id.ToUpper() && dr.RowState == DataRowState.Added)
                    {
                        dr.Delete();
                    }
                }
            }

            // Refresh data in gridview
            grd_Attachment.DataSource = dsAttachment.Tables[standardVoucherAttachment.TableName];
        }

        /// <summary>
        ///     Cancel adding account attachment data at consolidate.
        /// </summary>
        private void CanclingEditAC(int RowIndex)
        {
            // Setup primary key(s)
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete new row
            for (var i = dsAttachment.Tables[accountCon.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = dsAttachment.Tables[accountCon.TableName].Rows[i];

                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["ID"].ToString().ToUpper() == id.ToUpper() && dr.RowState == DataRowState.Added)
                    {
                        dr.Delete();
                    }
                }
            }

            // Refresh data in gridview
            grd_Attachment.DataSource = dsAttachment.Tables[accountCon.TableName];
        }

        /// <summary>
        ///     Canceling Vendor
        /// </summary>
        /// <param name="RowIndex"></param>
        private void CanclingEditVendor(int RowIndex)
        {
            // Setup primary key(s)
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete new row
            for (var i = dsAttachment.Tables[vendorAttachment.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = dsAttachment.Tables[vendorAttachment.TableName].Rows[i];

                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["ID"].ToString().ToUpper() == id.ToUpper() && dr.RowState == DataRowState.Added)
                    {
                        dr.Delete();
                    }
                }
            }

            // Refresh data in gridview
            grd_Attachment.DataSource = dsAttachment.Tables[vendorAttachment.TableName];
        }

        /// <summary>
        ///     Canceling Debtor
        /// </summary>
        /// <param name="RowIndex"></param>
        private void CanclingEditDebtor(int RowIndex)
        {
            // Setup primary key(s)
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete new row
            for (var i = dsAttachment.Tables[debtorAttach.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = dsAttachment.Tables[debtorAttach.TableName].Rows[i];

                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["ID"].ToString().ToUpper() == id.ToUpper() && dr.RowState == DataRowState.Added)
                    {
                        dr.Delete();
                    }
                }
            }

            // Refresh data in gridview
            grd_Attachment.DataSource = dsAttachment.Tables[debtorAttach.TableName];
        }

        /// <summary>
        ///     Canceling Invoice
        /// </summary>
        /// <param name="RowIndex"></param>
        private void CanclingEditInvoice(int RowIndex)
        {
            // Setup primary key(s)
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete new row
            for (var i = dsAttachment.Tables[invoiceAttachment.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = dsAttachment.Tables[invoiceAttachment.TableName].Rows[i];

                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["ID"].ToString().ToUpper() == id.ToUpper() && dr.RowState == DataRowState.Added)
                    {
                        dr.Delete();
                    }
                }
            }

            // Refresh data in gridview
            grd_Attachment.DataSource = dsAttachment.Tables[invoiceAttachment.TableName];
        }

        /// <summary>
        ///     Canceling Payment
        /// </summary>
        /// <param name="RowIndex"></param>
        private void CanclingEditPayment(int RowIndex)
        {
            // Setup primary key(s)
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete new row
            for (var i = dsAttachment.Tables[paymentAttachment.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = dsAttachment.Tables[paymentAttachment.TableName].Rows[i];

                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["ID"].ToString().ToUpper() == id.ToUpper() && dr.RowState == DataRowState.Added)
                    {
                        dr.Delete();
                    }
                }
            }

            // Refresh data in gridview
            grd_Attachment.DataSource = dsAttachment.Tables[paymentAttachment.TableName];
        }

        /// <summary>
        ///     Canceling Receipt
        /// </summary>
        /// <param name="RowIndex"></param>
        private void CanclingEditReceipt(int RowIndex)
        {
            // Setup primary key(s)
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete new row
            for (var i = dsAttachment.Tables[receiptAttach.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = dsAttachment.Tables[receiptAttach.TableName].Rows[i];

                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["ID"].ToString().ToUpper() == id.ToUpper() && dr.RowState == DataRowState.Added)
                    {
                        dr.Delete();
                    }
                }
            }

            // Refresh data in gridview
            grd_Attachment.DataSource = dsAttachment.Tables[receiptAttach.TableName];
        }

        /// <summary>
        ///     Canceling Account Reconcile
        /// </summary>
        /// <param name="RowIndex"></param>
        private void CanclingEditAccRec(int RowIndex)
        {
            // Setup primary key(s)
            var id = grd_Attachment.DataKeys[RowIndex].Value.ToString();

            // Delete new row
            for (var i = dsAttachment.Tables[accRecAttach.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = dsAttachment.Tables[accRecAttach.TableName].Rows[i];

                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["ID"].ToString().ToUpper() == id.ToUpper() && dr.RowState == DataRowState.Added)
                    {
                        dr.Delete();
                    }
                }
            }

            // Refresh data in gridview
            grd_Attachment.DataSource = dsAttachment.Tables[accRecAttach.TableName];
        }

        #endregion
    }
}