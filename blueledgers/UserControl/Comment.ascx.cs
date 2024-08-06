using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;

// ReSharper disable once CheckNamespace
namespace BlueLedger.PL.UserControls
{
    public partial class Comment : BaseUserControl
    {
        #region "Attributes"

        private readonly Blue.BL.ADMIN.TransComment _transComment = new Blue.BL.ADMIN.TransComment();
        private string _module = string.Empty;
        private string _refNo = string.Empty;
        private string _submodule = string.Empty;
        private string _tableName = string.Empty;

        private DataSet _dsComment = new DataSet();

        private string CommentMode
        {
            get { return ViewState["CommentMode"].ToString(); }
            set { ViewState["CommentMode"] = value; }
        }

        /// <summary>
        ///     Gets or sets table name which contain comment data.
        /// </summary>
        public string TableName
        {
            get
            {
                if (ViewState["TableName"] == null)
                {
                    return _tableName;
                }

                return ViewState["TableName"].ToString();
            }
            set
            {
                _tableName = value;
                ViewState["TableName"] = _tableName;
            }
        }

        /// <summary>
        ///     Gets or sets reference number related to comment data.
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

        public string Module
        {
            get
            {
                if (ViewState["Module"] == null)
                {
                    return _module;
                }

                return ViewState["Module"].ToString();
            }
            set
            {
                _module = value;
                ViewState["Module"] = _module;
            }
        }

        public string SubModule
        {
            get
            {
                if (ViewState["Submodule"] == null)
                {
                    return _submodule;
                }

                return ViewState["Submodule"].ToString();
            }
            set
            {
                _submodule = value;
                ViewState["Submodule"] = _submodule;
            }
        }

        /// <summary>
        ///     Whether row in the usercontrol can be add or not.
        /// </summary>
        //public bool ReadOnly
        //{
        //    get
        //    {
        //        if (ViewState["ReadOnly"] == null)
        //        {
        //            return this._readOnly;
        //        }
        //        return bool.Parse(ViewState["ReadOnly"].ToString());
        //    }
        //    set
        //    {
        //        this._readOnly = value;
        //        ViewState["ReadOnly"] = this._readOnly;
        //    }
        //}

        #endregion

        #region "Operations"
        public override void DataBind()
        {
            base.DataBind();

            Page_Retrieve();
        }

        private void Page_Retrieve()
        {
            _dsComment.Clear();

            var result = _transComment.GetTransCommentByModule_SubModule_RefNo(_dsComment,
                Module, SubModule, RefNo, LoginInfo.ConnStr);

            if (!result)
            {
                return;
            }

            Session["dsComment"] = _dsComment;

            Page_Setting();
        }

        private void Page_Setting()
        {
            _dsComment = (DataSet) Session["dsComment"];

            grd_Comment.DataSource = _dsComment.Tables[_transComment.TableName];
            grd_Comment.EditIndex = -1;
            grd_Comment.DataBind();
        }

        /// <summary>
        ///     Display comment data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Comment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Edit
                if (e.Row.FindControl("lnkb_Edit") != null)
                {
                    var lnkbEdit = (LinkButton) e.Row.FindControl("lnkb_Edit");
                    var commentOwner = (string) DataBinder.Eval(e.Row.DataItem, "CreatedBy");
                    lnkbEdit.Enabled = commentOwner == LoginInfo.LoginName;
                }

                // Delete
                if (e.Row.FindControl("lnkb_Del") != null)
                {
                    var lnkbDel = (LinkButton) e.Row.FindControl("lnkb_Del");

                    var commentOwner = (string) DataBinder.Eval(e.Row.DataItem, "CreatedBy");

                    lnkbDel.Enabled = commentOwner == LoginInfo.LoginName;
                }

                // Comment
                if (e.Row.FindControl("lbl_Comment") != null)
                {
                    var lblComment = (Label) e.Row.FindControl("lbl_Comment");
                    lblComment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                }

                if (e.Row.FindControl("txt_Comment") != null)
                {
                    var txtComment = (ASPxTextBox) e.Row.FindControl("txt_Comment");
                    txtComment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                }

                // Date
                if (e.Row.FindControl("lbl_Date") != null)
                {
                    var lblDate = (Label) e.Row.FindControl("lbl_Date");
                    lblDate.Text =
                        DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "CreatedDate").ToString())
                            .ToString(DateTimeFormat);
                }

                // By
                if (e.Row.FindControl("lbl_By") != null)
                {
                    var lblBy = (Label) e.Row.FindControl("lbl_By");
                    lblBy.Text = DataBinder.Eval(e.Row.DataItem, "CreatedBy").ToString();
                }
            }
        }

        /// <summary>
        ///     Edit comment.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Comment_RowEditing(object sender, GridViewEditEventArgs e)
        {
            _dsComment = (DataSet) Session["dsComment"];

            grd_Comment.DataSource = _dsComment.Tables[_transComment.TableName];
            grd_Comment.EditIndex = e.NewEditIndex;
            grd_Comment.DataBind();

            CommentMode = "EDIT";
        }

        /// <summary>
        ///     Update comment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Comment_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            _dsComment = (DataSet) Session["dsComment"];

            if (CommentMode == "NEW")
            {
                var txtComment = (ASPxTextBox) grd_Comment.Rows[e.RowIndex].FindControl("txt_Comment");

                var drNew = _dsComment.Tables[_transComment.TableName].Rows[grd_Comment.EditIndex];

                drNew["ID"] = _transComment.GetNewID(LoginInfo.ConnStr);
                drNew["Module"] = Module;
                drNew["SubModule"] = SubModule;
                drNew["RefNo"] = RefNo;
                drNew["Comment"] = txtComment.Text;
                drNew["CreatedBy"] = LoginInfo.LoginName;
                drNew["CreatedDate"] = ServerDateTime;

                var save = _transComment.Save(_dsComment, LoginInfo.ConnStr);

                if (save)
                {
                    grd_Comment.DataSource = _dsComment.Tables[_transComment.TableName];
                    grd_Comment.EditIndex = -1;
                    grd_Comment.DataBind();
                }

                Page_Retrieve();
            }
            else
            {
                if (grd_Comment.Rows[e.RowIndex].FindControl("txt_Comment") != null)
                {
                    var txtComment = (ASPxTextBox) grd_Comment.Rows[e.RowIndex].FindControl("txt_Comment");

                    if (txtComment.Text != string.Empty)
                    {
                        var drUpdating = _dsComment.Tables[_transComment.TableName].Rows[e.RowIndex];
                        drUpdating["Comment"] = txtComment.Text;

                        var save = _transComment.Save(_dsComment, LoginInfo.ConnStr);

                        if (save)
                        {
                            grd_Comment.DataSource = _dsComment.Tables[_transComment.TableName];
                            grd_Comment.EditIndex = -1;
                            grd_Comment.DataBind();
                        }
                    }
                }
            }

            grd_Comment.DataSource = _dsComment.Tables[_transComment.TableName];
            grd_Comment.EditIndex = -1;
            grd_Comment.DataBind();

            CommentMode = string.Empty;
        }

        /// <summary>
        ///     Cancel new or edit comment.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Comment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            _dsComment = (DataSet) Session["dsComment"];

            if (CommentMode == "NEW")
            {
                _dsComment.Tables[_transComment.TableName].Rows[_dsComment.Tables[_transComment.TableName].Rows.Count - 1]
                    .Delete();
            }

            if (CommentMode == "EDIT")
            {
                _dsComment.Tables[_transComment.TableName].Rows[_dsComment.Tables[_transComment.TableName].Rows.Count - 1]
                    .CancelEdit();
            }

            grd_Comment.DataSource = _dsComment.Tables[_transComment.TableName];
            grd_Comment.EditIndex = -1;
            grd_Comment.DataBind();
        }

        /// <summary>
        ///     Delete comment.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Comment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            _dsComment = (DataSet) Session["dsComment"];

            _dsComment.Tables[_transComment.TableName].Rows[e.RowIndex].Delete();

            var save = _transComment.Save(_dsComment, LoginInfo.ConnStr);

            if (save)
            {
                grd_Comment.DataSource = _dsComment.Tables[_transComment.TableName];
                grd_Comment.EditIndex = -1;
                grd_Comment.DataBind();
            }
        }

        /// <summary>
        ///     Add new comment.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_New_Click(object sender, EventArgs e)
        {
            _dsComment = (DataSet) Session["dsComment"];

            var drNew = _dsComment.Tables[_transComment.TableName].NewRow();

            drNew["ID"] = _transComment.GetNewID(LoginInfo.ConnStr);
            drNew["Module"] = Module;
            drNew["SubModule"] = SubModule;
            drNew["RefNo"] = RefNo;
            drNew["CreatedBy"] = LoginInfo.LoginName;
            drNew["CreatedDate"] = ServerDateTime;

            _dsComment.Tables[_transComment.TableName].Rows.Add(drNew);

            Session["dsComment"] = _dsComment;

            grd_Comment.DataSource = _dsComment.Tables[_transComment.TableName];
            grd_Comment.EditIndex = _dsComment.Tables[_transComment.TableName].Rows.Count - 1;
            grd_Comment.DataBind();

            CommentMode = "NEW";
        }

        #endregion
    }
}