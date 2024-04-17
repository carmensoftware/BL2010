using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.UserControls
{
    public partial class AttachmentForPrint : BaseUserControl
    {
        #region "Attributies"

        private readonly Blue.BL.Reference.AccountAttachment accountAttachment =
            new Blue.BL.Reference.AccountAttachment();

        private readonly Blue.BL.Consolidation.Setup.Application.AccountAttachment accountCon =
            new Blue.BL.Consolidation.Setup.Application.AccountAttachment();

        private readonly Blue.BL.GL.BudgetAttachment budgetAttachment = new Blue.BL.GL.BudgetAttachment();

        private readonly Blue.BL.AP.InvoiceAttachment invoiceAttachment = new Blue.BL.AP.InvoiceAttachment();

        private readonly Blue.BL.GL.JournalVoucherAttachment journalVoucherAttachment =
            new Blue.BL.GL.JournalVoucherAttachment();

        private readonly Blue.BL.AP.PaymentAttachment paymentAttachment = new Blue.BL.AP.PaymentAttachment();

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
                    var lnk_FileName = (Label) e.Row.FindControl("lnk_FileName");
                    lnk_FileName.Text = DataBinder.Eval(e.Row.DataItem, "FileName").ToString();
                }

                // Descriotion
                if (e.Row.FindControl("lbl_Description") != null)
                {
                    var lbl_Description = (Label) e.Row.FindControl("lbl_Description");
                    lbl_Description.Text = DataBinder.Eval(e.Row.DataItem, "Description").ToString();
                }

                // Public
                if (e.Row.FindControl("lbl_IsPublic") != null)
                {
                    var lbl_IsPublic = (Label) e.Row.FindControl("lbl_IsPublic");
                    lbl_IsPublic.Text = (DataBinder.Eval(e.Row.DataItem, "IsPublic").ToString() == string.Empty
                        ? "Private"
                        : (bool) DataBinder.Eval(e.Row.DataItem, "IsPublic") ? "Public" : "Private");
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

        #endregion
    }
}