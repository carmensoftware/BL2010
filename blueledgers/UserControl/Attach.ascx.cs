using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.UserControls
{
    public partial class Attach : BaseUserControl
    {
        #region "Attributies"

        private readonly Blue.BL.Application.Attachment attachment = new Blue.BL.Application.Attachment();
        private readonly Blue.BL.ProjectAdmin.SysParameter sysParam = new Blue.BL.ProjectAdmin.SysParameter();
        private readonly Blue.BL.Security.User user = new Blue.BL.Security.User();
        private DataSet _dataSource = new DataSet();
        private string _moduleName = string.Empty;
        private bool _readOnly;
        private string _refNo = string.Empty;
        private Blue.DAL.DbHandler dbHandler = new Blue.DAL.DbHandler();

        /// <summary>
        ///     Gets or sets data source of attachment data.
        /// </summary>
        private DataSet DataSource
        {
            get
            {
                if (ViewState["DataSource"] == null)
                {
                    return _dataSource;
                }

                return (DataSet) ViewState["DataSource"];
            }
            set
            {
                _dataSource = value;
                ViewState["DataSource"] = _dataSource;
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

            // DataSource Contruction
            DataSource.DataSetName = "Attachment";

            var dtAttach = new DataTable();
            dtAttach.TableName = "Attach";

            dtAttach.Columns.Add(new DataColumn("SeqNo", Type.GetType("System.String")));
            dtAttach.Columns.Add(new DataColumn("FileName", Type.GetType("System.String")));
            dtAttach.Columns.Add(new DataColumn("FileContent", Type.GetType("System.Object")));
            dtAttach.Columns.Add(new DataColumn("Description", Type.GetType("System.String")));
            dtAttach.Columns.Add(new DataColumn("IsPublic", Type.GetType("System.Boolean")));
            dtAttach.Columns.Add(new DataColumn("CreatedDate", Type.GetType("System.DateTime")));
            dtAttach.Columns.Add(new DataColumn("CreatedBy", Type.GetType("System.Int32")));
            DataSource.Tables.Add(dtAttach);

            // Get attachment data
            var result = Attachment_Retrieve();

            if (result)
            {
                // Display attachment file list
                grd_Attach.DataSource = DataSource.Tables["Attach"];
                grd_Attach.DataBind();

                // Not allow user upload any file to not commited transaction.
                btn_New.Enabled = !ReadOnly;
            }
        }

        /// <summary>
        ///     Gets attachment data related to module and reference number.
        /// </summary>
        /// <returns></returns>
        private bool Attachment_Retrieve()
        {
            var dsAttachment = new DataSet();
            dsAttachment = DataSource;

            if (ModuleName != string.Empty && RefNo != string.Empty)
            {
                // Get attachment data
                attachment.Get(dsAttachment, ModuleName, RefNo, LoginInfo.ConnStr);

                // Read xml file to get attach file data.
                if (dsAttachment.Tables[attachment.TableName] != null)
                {
                    if (dsAttachment.Tables[attachment.TableName].Rows.Count > 0)
                    {
                        var attchPath = Server.MapPath(sysParam.GetValue("System", "AttachPath", LoginInfo.ConnStr));
                        var fileName = dsAttachment.Tables[attachment.TableName].Rows[0]["AttachID"] + ".xml";

                        var fi = new FileInfo(attchPath + "\\" + fileName);

                        if (fi.Exists)
                        {
                            dsAttachment.ReadXml(fi.FullName);
                        }
                    }
                    else
                    {
                        // Add new row for attachment data
                        var drNew = dsAttachment.Tables[attachment.TableName].NewRow();
                        drNew["Module"] = ModuleName;
                        drNew["RefNo"] = RefNo;
                        drNew["AttachID"] = Guid.NewGuid().ToString();
                        drNew["CreatedDate"] = ServerDateTime;
                        drNew["CreatedBy"] = 3;
                        drNew["UpdatedDate"] = ServerDateTime;
                        drNew["UpdatedBy"] = 3;
                        dsAttachment.Tables[attachment.TableName].Rows.Add(drNew);
                    }
                }
                else
                {
                    // Error when try to get attachment data
                    return false;
                }
            }
            else
            {
                // Error when module name or reference number was not set.
                return false;
            }

            DataSource = dsAttachment;
            return true;
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
                // Seq No.
                if (e.Row.FindControl("lbl_SeqNo") != null)
                {
                    var lbl_SeqNo = (Label) e.Row.FindControl("lbl_SeqNo");
                    lbl_SeqNo.Text = DataBinder.Eval(e.Row.DataItem, "SeqNo").ToString();
                }

                // File Name
                if (e.Row.FindControl("lnkb_FileName") != null)
                {
                    var lnkb_FileName = (LinkButton) e.Row.FindControl("lnkb_FileName");
                    lnkb_FileName.Text = DataBinder.Eval(e.Row.DataItem, "FileName").ToString();

                    // If IsPublic is false, not allow another user to access this file except the owner.
                    if (!bool.Parse(DataBinder.Eval(e.Row.DataItem, "IsPublic").ToString()))
                    {
                        //lnkb_FileName.Enabled = (int.Parse(DataBinder.Eval(e.Row.DataItem, "CreatedBy").ToString()) == LoginInfo.LoginName ? true : false);   
                    }
                }
                if (e.Row.FindControl("fu_FileName") != null)
                {
                    var fu_FileName = (FileUpload) e.Row.FindControl("fu_FileName");
                    fu_FileName.Enabled = (DataSource.Tables["Attach"].Rows[e.Row.DataItemIndex].RowState ==
                                           DataRowState.Added
                        ? true
                        : false);
                }

                // File Description
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
                    chk_IsPublic.Checked = bool.Parse(DataBinder.Eval(e.Row.DataItem, "IsPublic").ToString());

                    // If IsPublic is false, not allow another user to access this file except the owner.
                    if (!bool.Parse(DataBinder.Eval(e.Row.DataItem, "IsPublic").ToString()))
                    {
                        //chk_IsPublic.Enabled = (int.Parse(DataBinder.Eval(e.Row.DataItem, "CreatedBy").ToString()) == LoginInfo.LoginName ? true : false);
                    }
                }

                // Date
                if (e.Row.FindControl("lbl_Date") != null)
                {
                    var lbl_Date = (Label) e.Row.FindControl("lbl_Date");
                    lbl_Date.Text =
                        DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "CreatedDate").ToString())
                            .ToString(DateTimeFormat);
                }

                // By
                if (e.Row.FindControl("lbl_By") != null)
                {
                    var lbl_By = (Label) e.Row.FindControl("lbl_By");
                    lbl_By.Text =
                        user.GetUserDisplayName(int.Parse(DataBinder.Eval(e.Row.DataItem, "CreatedBy").ToString()),
                            LoginInfo.ConnStr);
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
            grd_Attach.DataSource = DataSource.Tables["Attach"];
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
            var dsDataSource = new DataSet();
            dsDataSource = DataSource;

            // Delete attachment new attach file.
            if (dsDataSource.Tables["Attach"].Rows[e.RowIndex].RowState == DataRowState.Added)
            {
                dsDataSource.Tables["Attach"].Rows[e.RowIndex].Delete();
            }

            // Update datasource
            DataSource = dsDataSource;
            DataSource.AcceptChanges();

            // Refresh attachment data.
            grd_Attach.DataSource = DataSource.Tables["Attach"];
            grd_Attach.EditIndex = -1;
            grd_Attach.DataBind();
        }

        /// <summary>
        ///     Update attachment to database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Attach_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var dsDataSource = new DataSet();
            dsDataSource = DataSource;

            var fu_FileName = (FileUpload) grd_Attach.Rows[e.RowIndex].FindControl("fu_FileName");
            var txt_Description = (TextBox) grd_Attach.Rows[e.RowIndex].FindControl("txt_Description");
            var chk_IsPublic = (CheckBox) grd_Attach.Rows[e.RowIndex].FindControl("chk_IsPublic");

            // Update attachment data
            var drAttach = dsDataSource.Tables[attachment.TableName].Rows[0];
            drAttach["Module"] = ModuleName;
            drAttach["RefNo"] = RefNo;
            drAttach["AttachID"] = (drAttach.RowState == DataRowState.Added
                ? Guid.NewGuid().ToString()
                : drAttach["AttachID"]);
            drAttach["CreatedDate"] = (drAttach.RowState == DataRowState.Added
                ? ServerDateTime
                : drAttach["CreatedDate"]);
            drAttach["CreatedBy"] = (drAttach.RowState == DataRowState.Added
                ? LoginInfo.LoginName
                : drAttach["CreatedBy"]);
            drAttach["UpdatedDate"] = ServerDateTime;
            drAttach["UpdatedBy"] = LoginInfo.LoginName;

            // Update attachment file data
            var drAttachFile = dsDataSource.Tables["Attach"].Rows[e.RowIndex];

            if (fu_FileName.Enabled)
            {
                if (fu_FileName.HasFile)
                {
                    drAttachFile["FileName"] = fu_FileName.FileName;
                    drAttachFile["FileContent"] = fu_FileName.FileBytes;
                }
                else
                {
                    //MessageBox("Missing file name or file name dose not exist");

                    // Exit updatint process
                    return;
                }
            }

            drAttachFile["Description"] = txt_Description.Text.Trim();
            drAttachFile["IsPublic"] = chk_IsPublic.Checked;
            drAttachFile["CreatedDate"] = (drAttachFile.RowState == DataRowState.Added
                ? ServerDateTime
                : drAttachFile["CreatedDate"]);
            drAttachFile["CreatedBy"] = (drAttachFile.RowState == DataRowState.Added
                ? LoginInfo.LoginName
                : drAttachFile["CreatedBy"]);

            // Save attachment data to database
            var result = attachment.Save(dsDataSource, LoginInfo.ConnStr);

            // Write attachment file data to xml file
            if (result)
            {
                var fileName = Server.MapPath(sysParam.GetValue("System", "AttachPath", LoginInfo.ConnStr)) +
                               "\\" + drAttach["AttachID"] + ".xml";
                dsDataSource.Tables["Attach"].WriteXml(fileName);

                DataSource = dsDataSource;
                DataSource.AcceptChanges();

                grd_Attach.DataSource = DataSource.Tables["Attach"];
                grd_Attach.EditIndex = -1;
                grd_Attach.DataBind();

                //MessageBox("Update complete");
            }
        }

        /// <summary>
        ///     Remove uploaded file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Attach_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var dsDataSource = new DataSet();
            dsDataSource = DataSource;

            // Remove uploaded file data.
            dsDataSource.Tables["Attach"].Rows[e.RowIndex].Delete();

            // Reorder the seq no.
            var i = 0;
            foreach (DataRow drAttactFile in dsDataSource.Tables["Attach"].Rows)
            {
                if (drAttactFile.RowState != DataRowState.Deleted)
                {
                    drAttactFile["SeqNo"] = i + 1;
                    i++;
                }
            }

            // Save attachment data to database
            dsDataSource.Tables[attachment.TableName].Rows[0]["UpdatedDate"] = ServerDateTime;
            dsDataSource.Tables[attachment.TableName].Rows[0]["UpdatedBy"] = LoginInfo.LoginName;

            var result = attachment.Save(dsDataSource, LoginInfo.ConnStr);

            // Write attachment file data to xml file
            if (result)
            {
                var fileName = Server.MapPath(sysParam.GetValue("System", "AttachPath", LoginInfo.ConnStr)) +
                               "\\" + DataSource.Tables[attachment.TableName].Rows[0]["AttachID"] + ".xml";
                dsDataSource.Tables["Attach"].WriteXml(fileName);

                DataSource = dsDataSource;
                DataSource.AcceptChanges();

                grd_Attach.DataSource = DataSource.Tables["Attach"];
                grd_Attach.EditIndex = -1;
                grd_Attach.DataBind();

                //MessageBox("Delete complete");
            }
        }

        // Add new attach file.
        protected void btn_New_Click(object sender, EventArgs e)
        {
            var dsAttachment = new DataSet();
            dsAttachment = DataSource;

            // Add new row
            var drNew = dsAttachment.Tables["Attach"].NewRow();
            drNew["SeqNo"] = dsAttachment.Tables["Attach"].Rows.Count + 1;
            drNew["IsPublic"] = true;
            drNew["CreatedDate"] = ServerDateTime;
            drNew["CreatedBy"] = LoginInfo.LoginName;
            dsAttachment.Tables["Attach"].Rows.Add(drNew);

            // Setup attachment GridView edit row index.
            DataSource = dsAttachment;

            grd_Attach.DataSource = DataSource.Tables["Attach"];
            grd_Attach.EditIndex = DataSource.Tables["Attach"].Rows.Count - 1;
            grd_Attach.DataBind();
        }

        /// <summary>
        ///     Display download dialogbox to allow user download selected file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkb_FileName_Click(object sender, EventArgs e)
        {
            // Find row where contain the file download link.
            var lnkb_FileName = (LinkButton) sender;
            var gvr_Attach = (GridViewRow) lnkb_FileName.Parent.Parent;

            // Get row data source.
            var drAttach = DataSource.Tables["Attach"].Rows[gvr_Attach.DataItemIndex];

            // Get bytes stream of downloading file.
            var fileName = drAttach["FileName"].ToString();
            var fileContent = new Byte[((Byte[]) drAttach["FileContent"]).Length];
            fileContent = (Byte[]) drAttach["FileContent"];

            // Display download dialogbox.
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.AddHeader("Content-Length", fileContent.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(fileContent);
        }

        #endregion
    }
}