using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

// ReSharper disable once CheckNamespace
namespace BlueLedger.PL.UserControls
{
    public partial class Comment2 : BaseClass.BaseUserControl
    {
        #region "Attributes"

        private readonly Blue.BL.ADMIN.TransComment _transComment = new Blue.BL.ADMIN.TransComment();
        private readonly Blue.BL.dbo.Bu _bu = new Blue.BL.dbo.Bu();
        private string _bucode = string.Empty;
        /*
                private DataSet _dataSource = new DataSet();
        */
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

        /*
                /// <summary>
                ///     Gets or sets data source of comment data.
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
        */

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

        public string BuCode
        {
            get
            {
                if (ViewState["BuCode"] == null)
                {
                    return _bucode;
                }

                return ViewState["BuCode"].ToString();
            }
            set
            {
                _bucode = value;
                ViewState["BuCode"] = _bucode;
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

            //bool result = TransComment.GetTransCommentByModule_SubModule_RefNo(dsComment,
            //   this.Module, this.SubModule, this.RefNo, LoginInfo.ConnStr);.

            var result = _transComment.GetTransCommentByModule_SubModule_RefNo(_dsComment,
                Module, SubModule, RefNo, _bu.GetConnectionString(Request.Params["BuCode"]));

            if (!result)
            {
                return;
            }

            Session["dsComment"] = _dsComment;

            Page_Setting();
        }

        private void Page_Setting()
        {
            _dsComment = (DataSet)Session["dsComment"];

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
                // Find Control Edit and Delete in GridView.
                var lnkLabel1 = (LinkButton)e.Row.Cells[0].Controls[0];
                var lnkLabel2 = (LinkButton)e.Row.Cells[0].Controls[2];

                // Edit.
                if (lnkLabel1.Text == @"Edit")
                {
                    var commentOwner = (string)DataBinder.Eval(e.Row.DataItem, "CreatedBy");
                    lnkLabel1.Visible = commentOwner == LoginInfo.LoginName;
                }

                // Update.
                if (lnkLabel1.Text == @"Update")
                {
                    var commentOwner = (string)DataBinder.Eval(e.Row.DataItem, "CreatedBy");
                    lnkLabel1.Enabled = commentOwner == LoginInfo.LoginName;
                }

                // Cancel.
                if (lnkLabel2.Text == @"Cancel")
                {
                    var commentOwner = (string)DataBinder.Eval(e.Row.DataItem, "CreatedBy");
                    lnkLabel2.Enabled = commentOwner == LoginInfo.LoginName;
                }

                // Delete.
                if (lnkLabel2.Text == @"Del")
                {
                    var commentOwner = (string)DataBinder.Eval(e.Row.DataItem, "CreatedBy");
                    lnkLabel2.Visible = commentOwner == LoginInfo.LoginName;

                    lnkLabel2.OnClientClick = "return confirm('Confirm Delete?')";
                }

                // Comment
                if (e.Row.FindControl("lbl_Comment") != null)
                {
                    var lblComment = (Label)e.Row.FindControl("lbl_Comment");
                    lblComment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                }

                if (e.Row.FindControl("txt_Comment") != null)
                {
                    var txtComment = (TextBox)e.Row.FindControl("txt_Comment");
                    txtComment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                }

                // Date
                if (e.Row.FindControl("lbl_Date") != null)
                {
                    var lblDate = (Label)e.Row.FindControl("lbl_Date");

                    // Modified on: 29/12/2017, By: Fon
                    // lblDate.Text =
                    //    DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "CreatedDate").ToString())
                    //        .ToString(DateTimeFormat);

                    // I don't knoe why  ".ToString(DateTimeFormat)" didn't work on server
                    lblDate.Text = string.Format("{0}", ((DateTime)DataBinder.Eval(e.Row.DataItem, "CreatedDate")).ToString("dd/MM/yyyy"));
                    // End
                }

                // By
                if (e.Row.FindControl("lbl_By") != null)
                {
                    var lblBy = (Label)e.Row.FindControl("lbl_By");
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
            _dsComment = (DataSet)Session["dsComment"];

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
            _dsComment = (DataSet)Session["dsComment"];

            if (CommentMode == "NEW")
            {
                var txtComment = (TextBox)grd_Comment.Rows[e.RowIndex].FindControl("txt_Comment");

                var drNew = _dsComment.Tables[_transComment.TableName].Rows[grd_Comment.EditIndex];

                drNew["ID"] = _transComment.GetNewID(_bu.GetConnectionString(Request.Params["BuCode"]));
                drNew["Module"] = Module;
                drNew["SubModule"] = SubModule;
                drNew["RefNo"] = RefNo;
                drNew["Comment"] = txtComment.Text;
                drNew["CreatedBy"] = LoginInfo.LoginName;
                drNew["CreatedDate"] = ServerDateTime;

                var save = _transComment.Save(_dsComment, _bu.GetConnectionString(Request.Params["BuCode"]));

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
                    var txtComment = (TextBox)grd_Comment.Rows[e.RowIndex].FindControl("txt_Comment");

                    if (txtComment.Text != string.Empty)
                    {
                        var drUpdating = _dsComment.Tables[_transComment.TableName].Rows[e.RowIndex];
                        drUpdating["Comment"] = txtComment.Text;

                        var save = _transComment.Save(_dsComment, _bu.GetConnectionString(Request.Params["BuCode"]));

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
            _dsComment = (DataSet)Session["dsComment"];

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
            _dsComment = (DataSet)Session["dsComment"];

            _dsComment.Tables[_transComment.TableName].Rows[e.RowIndex].Delete();

            var save = _transComment.Save(_dsComment, _bu.GetConnectionString(Request.Params["BuCode"]));

            if (save)
            {
                grd_Comment.DataSource = _dsComment.Tables[_transComment.TableName];
                grd_Comment.EditIndex = -1;
                grd_Comment.DataBind();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_New_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Add new comment.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            _dsComment = (DataSet)Session["dsComment"];

            var drNew = _dsComment.Tables[_transComment.TableName].NewRow();

            drNew["ID"] = _transComment.GetNewID(_bu.GetConnectionString(Request.Params["BuCode"]));
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