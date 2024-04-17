using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Text.RegularExpressions;

namespace BlueLedger.PL.UserControls
{
    public partial class Attach2 : BaseUserControl
    {
        #region "Attributies"

        private readonly Blue.BL.APP.Attachment attach = new Blue.BL.APP.Attachment();
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();

        private string _buCode = string.Empty;

        private string _moduleName = string.Empty;
        private bool _readOnly;

        private string _refNo = string.Empty;
        private DataSet dsAttachment = new DataSet();

        /// <summary>
        ///     Get or Set business unit code related to attachment data.
        /// </summary>
        public string BuCode
        {
            get
            {
                if (ViewState["BuCode"] == null)
                {
                    return _buCode;
                }

                return ViewState["BuCode"].ToString();
            }
            set
            {
                _buCode = value;
                ViewState["BuCode"] = _buCode;
            }
        }

        /// <summary>
        ///     Gets or sets module name related to attachment data.
        /// </summary>
        public string ModuleName
        {
            get
            {
                if (ViewState["ModuleName"] == null)
                {
                    return _moduleName;
                }

                return ViewState["ModuleName"].ToString();
            }
            set
            {
                _moduleName = value;
                ViewState["ModuleName"] = _moduleName;
            }
        }

        /// <summary>
        ///     Gets or sets reference number related to attachment data.
        /// </summary>
        public string RefNo
        {
            get
            {
                if (ViewState["RefNo"] == null)
                {
                    return _refNo;
                }

                return ViewState["RefNo"].ToString();
            }
            set
            {
                _refNo = value;
                ViewState["RefNo"] = _refNo;
            }
        }

        /// <summary>
        ///     Whether row in the usercontrol can be add or not.
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                if (ViewState["ReadOnly"] == null)
                {
                    return _readOnly;
                }

                return bool.Parse(ViewState["ReadOnly"].ToString());
            }
            set
            {
                _readOnly = value;
                ViewState["ReadOnly"] = _readOnly;
            }
        }

        #endregion

        #region "Operations"

        public override void DataBind()
        {
            base.DataBind();

            Control_Setting();
        }

        private void Control_Setting()
        {
            var getData = Control_Retrieve();

            if (getData)
            {
                grd_Attach.DataSource = dsAttachment;
                grd_Attach.EditIndex = -1;
                grd_Attach.DataBind();
            }
        }

        private bool Control_Retrieve()
        {
            if (dsAttachment != null)
            {
                dsAttachment.Clear();
            }

            if (ModuleName != string.Empty && RefNo != string.Empty)
            {
                var getAttachment = attach.GetList(dsAttachment, ModuleName, RefNo, bu.GetConnectionString(BuCode));

                if (getAttachment)
                {
                    Session["dsAttachment"] = dsAttachment;
                    return true;
                }
                // Display Error Message
                return false;
            }
            return false;
        }


        private string getAttachFileName(string buCode, string refNo, string fileName)
        {
            string attachmentPath = config.GetValue("SYS", "ATTACH", "Path", bu.GetConnectionString(buCode));
            return @Server.MapPath(attachmentPath + "\\" + buCode + "\\" + refNo + "\\" + fileName);
        }

        /// <summary>
        ///     Display attachment file list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Attach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // File Name
                if (e.Row.FindControl("lnkb_FileName") != null)
                {
                    var lnkb_FileName = (LinkButton)e.Row.FindControl("lnkb_FileName");
                    lnkb_FileName.Text = DataBinder.Eval(e.Row.DataItem, "FileName").ToString();

                    // If IsPublic is false, not allow another user to access this file except the owner.
                    if (!bool.Parse(DataBinder.Eval(e.Row.DataItem, "IsPublic").ToString()))
                    {
                        lnkb_FileName.Enabled = (DataBinder.Eval(e.Row.DataItem, "CreatedBy").ToString().ToUpper() ==
                                                 LoginInfo.LoginName.ToUpper()
                            ? true
                            : false);
                    }
                }
                if (e.Row.FindControl("fu_FileName") != null)
                {
                    var fu_FileName = (FileUpload)e.Row.FindControl("fu_FileName");
                    fu_FileName.Enabled = (dsAttachment.Tables[attach.TableName].Rows[e.Row.DataItemIndex].RowState ==
                                           DataRowState.Added
                        ? true
                        : false);
                }

                // File Description
                if (e.Row.FindControl("lbl_Description") != null)
                {
                    var lbl_Description = (Label)e.Row.FindControl("lbl_Description");
                    lbl_Description.Text = DataBinder.Eval(e.Row.DataItem, "Description").ToString();
                }

                if (e.Row.FindControl("txt_Description") != null)
                {
                    var txt_Description = (TextBox)e.Row.FindControl("txt_Description");
                    txt_Description.Text = DataBinder.Eval(e.Row.DataItem, "Description").ToString();
                }

                // Public
                if (e.Row.FindControl("chk_IsPublic") != null)
                {
                    var chk_IsPublic = (CheckBox)e.Row.FindControl("chk_IsPublic");
                    chk_IsPublic.Checked = bool.Parse(DataBinder.Eval(e.Row.DataItem, "IsPublic").ToString());

                    // If IsPublic is false, not allow another user to access this file except the owner.
                    if (!bool.Parse(DataBinder.Eval(e.Row.DataItem, "IsPublic").ToString()))
                    {
                        chk_IsPublic.Enabled = (DataBinder.Eval(e.Row.DataItem, "CreatedBy").ToString().ToUpper() ==
                                                LoginInfo.LoginName.ToUpper()
                            ? true
                            : false);
                    }
                }

                // Date
                if (e.Row.FindControl("lbl_Date") != null)
                {
                    var lbl_Date = (Label)e.Row.FindControl("lbl_Date");
                    
                    // Modified on: 28/12/2017, By:Fon
                    //lbl_Date.Text =
                    //    DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "CreatedDate").ToString())
                    //        .ToString(DateTimeFormat);

                    lbl_Date.Text = string.Format("{0}", ((DateTime)DataBinder.Eval(e.Row.DataItem, "CreatedDate")).ToString("dd/MM/yyyy"));
                    // End Modified.
                }

                // By
                if (e.Row.FindControl("lbl_By") != null)
                {
                    var lbl_By = (Label)e.Row.FindControl("lbl_By");
                    lbl_By.Text = DataBinder.Eval(e.Row.DataItem, "CreatedBy").ToString();
                }

                if (LoginInfo.LoginName != DataBinder.Eval(e.Row.DataItem, "CreatedBy").ToString())
                {
                    var lnkb_Edit = (LinkButton)e.Row.FindControl("lnkb_Edit");
                    var lnkb_Del = (LinkButton)e.Row.FindControl("lnkb_Del");
                    var lbl_Separator = (Label)e.Row.FindControl("lbl_Separator");
                    lnkb_Edit.Visible = false;
                    lnkb_Del.Visible = false;
                    lbl_Separator.Visible = false;
                }
            }
        }

        /// <summary>
        ///     Edit upload file description.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Attach_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dsAttachment = Session["dsAttachment"] as DataSet;

            grd_Attach.DataSource = dsAttachment.Tables[attach.TableName];
            grd_Attach.EditIndex = e.NewEditIndex;
            grd_Attach.DataBind();
        }

        /// <summary>
        ///     Cancel add/edit upload file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Attach_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dsAttachment = Session["dsAttachment"] as DataSet;

            // Delete attachment new attach file.
            if (dsAttachment.Tables[attach.TableName].Rows[e.RowIndex].RowState == DataRowState.Added)
            {
                dsAttachment.Tables[attach.TableName].Rows[e.RowIndex].Delete();
            }

            // Refresh attachment data.
            grd_Attach.DataSource = dsAttachment.Tables[attach.TableName];
            grd_Attach.EditIndex = -1;
            grd_Attach.DataBind();

            Session["dsAttachment"] = dsAttachment;
        }



        /// <summary>
        ///     Update attachment to database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Attach_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            dsAttachment = Session["dsAttachment"] as DataSet;

            // Find all control
            var fu_FileName = (FileUpload)grd_Attach.Rows[e.RowIndex].FindControl("fu_FileName");
            var txt_Description = (TextBox)grd_Attach.Rows[e.RowIndex].FindControl("txt_Description");
            var chk_IsPublic = (CheckBox)grd_Attach.Rows[e.RowIndex].FindControl("chk_IsPublic");

            if (!fu_FileName.HasFile)
            {
                return;
            }

            var drUpdating = dsAttachment.Tables[attach.TableName].Rows[grd_Attach.Rows[e.RowIndex].DataItemIndex];

            // Check file exist
            //var fileName = Server.MapPath(config.GetValue("SYS", "ATTACH", "Path", bu.GetConnectionString(BuCode))) +
            //               "\\" + drUpdating["RefNo"].ToString() + "\\" + fu_FileName.FileName;
            string fileName = getAttachFileName(BuCode, RefNo, fu_FileName.FileName);

            string subPath = getAttachFileName(BuCode, RefNo, ""); // your code goes here

            bool exists = System.IO.Directory.Exists(subPath);

            if (!exists)
                System.IO.Directory.CreateDirectory(subPath);

            var uploadigFile = new FileInfo(fileName);

            if (uploadigFile.Exists)
            {
                return;
            }

            // Update row

            if (fu_FileName.Enabled)
            {
                drUpdating["FileName"] = fu_FileName.FileName;
            }

            drUpdating["Description"] = txt_Description.Text.Trim();
            drUpdating["IsPublic"] = chk_IsPublic.Checked;
            drUpdating["UpdatedDate"] = ServerDateTime;
            drUpdating["UpdatedBy"] = LoginInfo.LoginName;

            var updated = attach.Save(dsAttachment, bu.GetConnectionString(BuCode));

            // Upload file
            if (updated)
            {
                fu_FileName.SaveAs(fileName);
            }


            txt_Description.Text = drUpdating["RefNo"].ToString();
            Session["dsAttachment"] = dsAttachment;
            Control_Setting();
        }

        /// <summary>
        ///     Remove uploaded file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Attach_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            dsAttachment = Session["dsAttachment"] as DataSet;

            string attachFileName = dsAttachment.Tables[attach.TableName].Rows[grd_Attach.Rows[e.RowIndex].DataItemIndex]["FileName"].ToString();

            // Get file name before remove record.
            //var fileName = Server.MapPath(config.GetValue("SYS", "ATTACH", "Path", bu.GetConnectionString(BuCode))) +
            //               "\\" + refNo + "\\" + attachFileName;

            string fileName = getAttachFileName(BuCode, RefNo, attachFileName);

            // Remove uploaded file data.
            dsAttachment.Tables[attach.TableName].Rows[grd_Attach.Rows[e.RowIndex].DataItemIndex].Delete();

            // Save attachment data to database
            var saved = attach.Save(dsAttachment, bu.GetConnectionString(BuCode));

            if (saved)
            {
                // Remove file
                var deleteFile = new FileInfo(fileName);

                if (deleteFile.Exists)
                {
                    deleteFile.Delete();
                }
            }

            Session["dsAttachment"] = dsAttachment;
            Control_Setting();
        }

        public static string MakeValidFileName(string name)
        {
            string invalidChars = Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidReStr = string.Format(@"[{0}]+", invalidChars);
            string replace = Regex.Replace(name, invalidReStr, "_").Replace(";", "").Replace(",", "");
            return replace;
        }

        /// <summary>
        ///     Display download dialogbox to allow user download selected file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkb_FileName_Click(object sender, EventArgs e)
        {
            dsAttachment = Session["dsAttachment"] as DataSet;

            // Find row where contain the file download link.
            var lnkb_FileName = (LinkButton)sender;
            var gvr_Attach = (GridViewRow)lnkb_FileName.Parent.Parent;

            // Get row data source.
            var drAttach = dsAttachment.Tables[attach.TableName].Rows[gvr_Attach.DataItemIndex];

            // Get bytes stream of downloading file.
            //var fileName = Server.MapPath(config.GetValue("SYS", "ATTACH", "Path", bu.GetConnectionString(BuCode))) +
            //               "\\" + drAttach["RefNo"] + "\\" + drAttach["FileName"];
            string fileName = getAttachFileName(BuCode, RefNo, drAttach["FileName"].ToString());

            // Check File Exist
            var fileInfo = new FileInfo(fileName);

            if (fileInfo.Exists)
            {
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + MakeValidFileName(drAttach["FileName"].ToString()));
                Response.TransmitFile(fileName);
                Response.End();
            }
        }

        /// <summary>
        ///     Create new attachment
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    dsAttachment = Session["dsAttachment"] as DataSet;

                    // Add new row
                    var drNew = dsAttachment.Tables[attach.TableName].NewRow();
                    drNew["Module"] = ModuleName;
                    drNew["RefNo"] = RefNo;
                    drNew["FileName"] = string.Empty;
                    drNew["Description"] = string.Empty;
                    drNew["IsPublic"] = true;
                    drNew["CreatedDate"] = ServerDateTime;
                    drNew["CreatedBy"] = LoginInfo.LoginName;
                    drNew["UpdatedDate"] = ServerDateTime;
                    drNew["UpdatedBy"] = LoginInfo.LoginName;
                    dsAttachment.Tables[attach.TableName].Rows.Add(drNew);

                    grd_Attach.DataSource = dsAttachment;
                    grd_Attach.EditIndex = dsAttachment.Tables[attach.TableName].Rows.Count - 1;
                    grd_Attach.DataBind();

                    Session["dsAttachment"] = dsAttachment;

                    break;
            }
        }

        #endregion

        protected void fu_FileName_DataBinding(object sender, EventArgs e)
        {
        }
    }
}