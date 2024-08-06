using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN
{
    public partial class DefaultInvoice : BaseUserControl
    {
        #region "Attributies"

        private readonly Blue.BL.Reference.Account account = new Blue.BL.Reference.Account();
        private readonly Blue.BL.AP.InvoiceDefault invoiceDefault = new Blue.BL.AP.InvoiceDefault();
        private readonly Blue.BL.AP.InvoiceDefaultDetail invoiceDefaultDetail = new Blue.BL.AP.InvoiceDefaultDetail();
        private readonly Blue.BL.AP.TransactionType transactionType = new Blue.BL.AP.TransactionType();
        private readonly Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();
        private DataSet _dataSource = new DataSet();
        private Guid _profileCode;
        private bool _readOnly;


        private bool _readonly = false;
        private int _seqNo;

        /// <summary>
        ///     Gets or Sets current row index of invoiceDefaultID for show detail gridview.
        /// </summary>
        private int SeqNo
        {
            get
            {
                _seqNo = (ViewState["SeqNo"] == null ? 0 : (int) ViewState["SeqNo"]);
                return _seqNo;
            }
            set
            {
                _seqNo = value;
                ViewState.Add("SeqNo", _seqNo);
            }
        }


        /// <summary>
        ///     Get or Set Control DataSource.
        /// </summary>
        public DataSet DataSource
        {
            get
            {
                _dataSource = (ViewState["DataSource"] == null ? new DataSet() : (DataSet) ViewState["DataSource"]);
                return _dataSource;
            }
            set
            {
                _dataSource = value;
                ViewState.Add("DataSource", _dataSource);
            }
        }

        /// <summary>
        ///     Get or Set readonly property.
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                _readOnly = (ViewState["ReadOnly"] == null ? false : (bool) ViewState["ReadOnly"]);
                return _readOnly;
            }
            set
            {
                _readOnly = value;
                ViewState.Add("ReadOnly", _readOnly);
            }
        }


        /// <summary>
        ///     Get or Set readonly property.
        /// </summary>
        public Guid ProfileCode
        {
            get
            {
                _profileCode = (ViewState["ProfileCode"] == null ? new Guid() : (Guid) ViewState["ProfileCode"]);
                return _profileCode;
            }
            set
            {
                _profileCode = value;
                ViewState.Add("ProfileCode", _profileCode);
            }
        }

        #endregion

        #region "Operations"

        /// <summary>
        ///     Invoicedefault row databound event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Invoice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // DataRow
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lnkb_Edit") != null)
                {
                    var lnkb_Edit = (LinkButton) e.Row.FindControl("lnkb_Edit");

                    // Check edit button for read only and assign visible false.
                    if (ReadOnly)
                    {
                        lnkb_Edit.Visible = false;
                    }
                }

                // Separator
                if (e.Row.FindControl("lbl_Separator") != null)
                {
                    var lbl_Separator = (Label) e.Row.FindControl("lbl_Separator");

                    // Check Separator for read only and assign visible false.
                    if (ReadOnly)
                    {
                        lbl_Separator.Visible = false;
                    }
                }

                if (e.Row.FindControl("lnkb_Update") != null)
                {
                    var lnkb_Update = (LinkButton) e.Row.FindControl("lnkb_Update");

                    // Check update button for read only and assign visible false.
                    if (ReadOnly)
                    {
                        lnkb_Update.Visible = false;
                    }
                }

                if (e.Row.FindControl("lnkb_Cancel") != null)
                {
                    var lnkb_Cancel = (LinkButton) e.Row.FindControl("lnkb_Cancel");

                    // Check cancel button for read only and assign visible false.
                    if (ReadOnly)
                    {
                        lnkb_Cancel.Visible = false;
                    }
                }

                if (e.Row.FindControl("lnkb_Delete") != null)
                {
                    var lnkb_Delete = (LinkButton) e.Row.FindControl("lnkb_Delete");

                    // Check  delete button for read only and assign visible false.
                    if (ReadOnly)
                    {
                        lnkb_Delete.Visible = false;
                    }
                }


                // SeqNo
                if (e.Row.FindControl("lbl_Seq") != null)
                {
                    var lbl_Seq = (Label) e.Row.FindControl("lbl_Seq");
                    lbl_Seq.Text = DataBinder.Eval(e.Row.DataItem, "SeqNo").ToString();
                }

                // SeqNo
                if (e.Row.FindControl("lbl_SeqInvoice") != null)
                {
                    var lbl_SeqInvoice = (Label) e.Row.FindControl("lbl_SeqInvoice");
                    lbl_SeqInvoice.Text = DataBinder.Eval(e.Row.DataItem, "SeqNo").ToString();
                }


                // TransType
                if (e.Row.FindControl("lbl_TransactionType") != null)
                {
                    var lbl_TransactionType = (Label) e.Row.FindControl("lbl_TransactionType");
                    lbl_TransactionType.Text =
                        transactionType.GetTransactionTypeName(
                            DataBinder.Eval(e.Row.DataItem, "TransTypeCode").ToString(), LoginInfo.ConnStr);
                }

                // Transactiontype
                if (e.Row.FindControl("ddl_TransactionType") != null)
                {
                    var ddl_TransactionType = (DropDownList) e.Row.FindControl("ddl_TransactionType");
                    ddl_TransactionType.DataSource = transactionType.GetTransactionTypeListLookup(LoginInfo.ConnStr);
                    ddl_TransactionType.DataTextField = "Name";
                    ddl_TransactionType.DataValueField = "TransactionTypeCode";
                    ddl_TransactionType.DataBind();
                    ddl_TransactionType.SelectedValue = DataBinder.Eval(e.Row.DataItem, "TransTypeCode").ToString();
                }

                // Description
                if (e.Row.FindControl("lbl_Description") != null)
                {
                    var lbl_Description = (Label) e.Row.FindControl("lbl_Description");
                    lbl_Description.Text = DataBinder.Eval(e.Row.DataItem, "Description").ToString();
                }

                // Description
                if (e.Row.FindControl("txt_Description") != null)
                {
                    var txt_Description = (TextBox) e.Row.FindControl("txt_Description");
                    txt_Description.Text = DataBinder.Eval(e.Row.DataItem, "Description").ToString();
                }
            }
        }


        /// <summary>
        ///     Invoicedefault row canceling event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Invoice_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Setup primary key(s)
            var profileCode = grd_Invoice.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_Invoice.DataKeys[e.RowIndex].Values[1]);

            // Delete new row
            for (var i = DataSource.Tables[invoiceDefault.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = DataSource.Tables[invoiceDefault.TableName].Rows[i];

                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["ProfileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                        Convert.ToInt32(dr["SeqNo"]) == seqNo &&
                        dr.RowState == DataRowState.Added)
                    {
                        dr.Delete();
                        break;
                    }
                }
            }

            // Refresh data in gridview
            grd_Invoice.DataSource = DataSource.Tables[invoiceDefault.TableName];
            grd_Invoice.EditIndex = -1;
            grd_Invoice.DataBind();
        }

        /// <summary>
        ///     Invoicedefault rowupdating event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Invoice_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Setup primary key(s)
            var profileCode = grd_Invoice.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_Invoice.DataKeys[e.RowIndex].Values[1]);


            foreach (DataRow drUpdating in DataSource.Tables[invoiceDefault.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["ProfileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                        Convert.ToInt32(drUpdating["SeqNo"]) == seqNo)
                    {
                        // Find controls inside of gridview.
                        var lbl_SeqInvoice = (Label) grd_Invoice.Rows[e.RowIndex].FindControl("lbl_SeqInvoice");
                        var ddl_TransactionType =
                            (DropDownList) grd_Invoice.Rows[e.RowIndex].FindControl("ddl_TransactionType");
                        var txt_Description = (TextBox) grd_Invoice.Rows[e.RowIndex].FindControl("txt_Description");


                        // Updating for assign data.
                        // SeqNo
                        drUpdating["SeqNo"] = Convert.ToInt32(lbl_SeqInvoice.Text);

                        // TransTypeCode
                        drUpdating["TransTypeCode"] = ddl_TransactionType.SelectedItem.Value;

                        // Description
                        drUpdating["Description"] = txt_Description.Text.Trim();


                        // Refresh data in GridView
                        grd_Invoice.EditIndex = -1;
                        grd_Invoice.DataSource = DataSource.Tables[invoiceDefault.TableName];
                        grd_Invoice.DataBind();
                    }
                }
            }
        }


        /// <summary>
        ///     Invoicedefault row editing event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Invoice_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grd_Invoice.EditIndex = e.NewEditIndex;
            grd_Invoice.DataSource = DataSource.Tables[invoiceDefault.TableName];
            grd_Invoice.DataBind();
        }

        /// <summary>
        ///     InvoiceDefault row deleting event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Invoice_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Setup primary key(s)
            var profileCode = grd_Invoice.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_Invoice.DataKeys[e.RowIndex].Values[1]);

            // Delete the record 
            for (var i = DataSource.Tables[invoiceDefault.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drDelete = DataSource.Tables[invoiceDefault.TableName].Rows[i];

                if (drDelete.RowState != DataRowState.Deleted)
                {
                    if (drDelete["ProfileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                        Convert.ToInt32(drDelete["SeqNo"]) == seqNo)
                    {
                        drDelete.Delete();
                    }
                }
            }

            // Binding grid
            grd_Invoice.DataSource = DataSource.Tables[invoiceDefault.TableName];
            grd_Invoice.DataBind();
        }


        /// <summary>
        ///     Invoicedefault detail row data bound event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_InvoiceDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // DataRow
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lnkb_Edit") != null)
                {
                    var lnkb_Edit = (LinkButton) e.Row.FindControl("lnkb_Edit");

                    // Check edit button for read only and assign visible false.
                    if (ReadOnly)
                    {
                        lnkb_Edit.Visible = false;
                    }
                }

                // Separator
                if (e.Row.FindControl("lbl_Separator") != null)
                {
                    var lbl_Separator = (Label) e.Row.FindControl("lbl_Separator");

                    // Check Separator for read only and assign visible false.
                    if (ReadOnly)
                    {
                        lbl_Separator.Visible = false;
                    }
                }

                if (e.Row.FindControl("lnkb_Update") != null)
                {
                    var lnkb_Update = (LinkButton) e.Row.FindControl("lnkb_Update");

                    // Check update button for read only and assign visible false.
                    if (ReadOnly)
                    {
                        lnkb_Update.Visible = false;
                    }
                }

                if (e.Row.FindControl("lnkb_Cancel") != null)
                {
                    var lnkb_Cancel = (LinkButton) e.Row.FindControl("lnkb_Cancel");

                    // Check cancel button for read only and assign visible false.
                    if (ReadOnly)
                    {
                        lnkb_Cancel.Visible = false;
                    }
                }

                if (e.Row.FindControl("lnkb_Delete") != null)
                {
                    var lnkb_Delete = (LinkButton) e.Row.FindControl("lnkb_Delete");

                    // Check  delete button for read only and assign visible false.
                    if (ReadOnly)
                    {
                        lnkb_Delete.Visible = false;
                    }
                }

                // DetailSeqNo
                if (e.Row.FindControl("lbl_Item") != null)
                {
                    var lbl_Item = (Label) e.Row.FindControl("lbl_Item");
                    lbl_Item.Text = DataBinder.Eval(e.Row.DataItem, "DetailSeqNo").ToString();
                }

                // ItemName
                if (e.Row.FindControl("lbl_ItemName") != null)
                {
                    var lbl_ItemName = (Label) e.Row.FindControl("lbl_ItemName");
                    lbl_ItemName.Text = DataBinder.Eval(e.Row.DataItem, "ItemName").ToString();
                }

                // ItemName
                if (e.Row.FindControl("txt_ItemName") != null)
                {
                    var txt_ItemName = (TextBox) e.Row.FindControl("txt_ItemName");
                    txt_ItemName.Text = DataBinder.Eval(e.Row.DataItem, "ItemName").ToString();
                }

                // AccountCode
                if (e.Row.FindControl("lbl_AccCode") != null)
                {
                    var lbl_AccCode = (Label) e.Row.FindControl("lbl_AccCode");
                    lbl_AccCode.Text = DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString();
                }

                // AccountCodeName
                if (e.Row.FindControl("lbl_AccCode_Name") != null)
                {
                    var lbl_AccCode_Name = (Label) e.Row.FindControl("lbl_AccCode_Name");
                    lbl_AccCode_Name.Text = account.GetName(DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString(),
                        LoginInfo.ConnStr);
                }


                // Accountlist
                if (e.Row.FindControl("ddl_AccountList") != null)
                {
                    var ddl_AccountList = (DropDownList) e.Row.FindControl("ddl_AccountList");
                    ddl_AccountList.DataSource = account.GetAccountListLookup(LoginInfo.ConnStr);
                    ddl_AccountList.DataTextField = "NameEng";
                    ddl_AccountList.DataValueField = "AccountCode";
                    ddl_AccountList.DataBind();
                    ddl_AccountList.SelectedValue = DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString();
                }

                // Rate
                if (e.Row.FindControl("lbl_Rate") != null)
                {
                    var lbl_Rate = (Label) e.Row.FindControl("lbl_Rate");
                    lbl_Rate.Text = DataBinder.Eval(e.Row.DataItem, "Rate").ToString();
                }


                // Rate
                if (e.Row.FindControl("txt_Rate") != null)
                {
                    var txt_Rate = (TextBox) e.Row.FindControl("txt_Rate");
                    txt_Rate.Text = DataBinder.Eval(e.Row.DataItem, "Rate").ToString();
                }


                // fx
                if (e.Row.FindControl("lbl_fx") != null)
                {
                    var lbl_fx = (Label) e.Row.FindControl("lbl_fx");
                    lbl_fx.Text = DataBinder.Eval(e.Row.DataItem, "fx").ToString();
                }

                // fx
                if (e.Row.FindControl("txt_fx") != null)
                {
                    var txt_fx = (TextBox) e.Row.FindControl("txt_fx");
                    txt_fx.Text = DataBinder.Eval(e.Row.DataItem, "fx").ToString();
                }
            }
        }

        /// <summary>
        ///     Invoicedefault Detail row cancelingedit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_InvoiceDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Find grdivew 
            var grd_InvoiceDetail = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_InvoiceDetail");

            // Determine GridRow
            var grdGridRow = (GridViewRow) grd_InvoiceDetail.NamingContainer;

            // Setup primary key(s)
            var profileCode = grd_InvoiceDetail.DataKeys[grdGridRow.RowIndex].Values[0].ToString();
            var detailSeqNo = Convert.ToInt32(grd_InvoiceDetail.DataKeys[e.RowIndex].Values[1]);

            // Delete new row
            for (var i = DataSource.Tables[invoiceDefaultDetail.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = DataSource.Tables[invoiceDefaultDetail.TableName].Rows[i];

                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["ProfileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                        Convert.ToInt32(dr["DetailSeqNo"]) == detailSeqNo &&
                        dr.RowState == DataRowState.Added)
                    {
                        dr.Delete();
                        break;
                    }
                }
            }


            // Binding gridview by depend on each of the selected row.
            ((GridView) grd_Invoice.Rows[grdGridRow.RowIndex].FindControl("grd_InvoiceDetail")).DataSource =
                DataSource.Tables[invoiceDefaultDetail.TableName];
            ((GridView) grd_Invoice.Rows[grdGridRow.RowIndex].FindControl("grd_InvoiceDetail")).EditIndex = -1;
            grd_Invoice.Rows[grdGridRow.RowIndex].FindControl("grd_InvoiceDetail").DataBind();
        }

        /// <summary>
        ///     Invoicedefault detail row updating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_InvoiceDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Find grdivew 
            var grd_InvoiceDetail = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_InvoiceDetail");

            // Determine GridRow
            var grdGridRow = (GridViewRow) grd_InvoiceDetail.NamingContainer;

            var profileCode = grd_InvoiceDetail.DataKeys[e.RowIndex].Values[0].ToString();
            var detailSeqNo = Convert.ToInt32(grd_InvoiceDetail.DataKeys[e.RowIndex].Values[1]);

            foreach (DataRow drUpdating in DataSource.Tables[invoiceDefaultDetail.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["ProfileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                        Convert.ToInt32(drUpdating["DetailSeqNo"]) == detailSeqNo)
                    {
                        // Find controls inside of gridview.
                        var txt_ItemName = (TextBox) grd_InvoiceDetail.Rows[e.RowIndex].FindControl("txt_ItemName");
                        var ddl_AccountList =
                            (DropDownList) grd_InvoiceDetail.Rows[e.RowIndex].FindControl("ddl_AccountList");
                        var txt_Rate = (TextBox) grd_InvoiceDetail.Rows[e.RowIndex].FindControl("txt_Rate");
                        var txt_fx = (TextBox) grd_InvoiceDetail.Rows[e.RowIndex].FindControl("txt_fx");


                        // Updating for assign data.
                        // ContactPerson
                        drUpdating["ItemName"] = txt_ItemName.Text;

                        // Contact
                        drUpdating["AccountCode"] = ddl_AccountList.SelectedItem.Value;

                        // Remark
                        drUpdating["Rate"] = txt_Rate.Text.Trim();

                        // Remark
                        drUpdating["fx"] = txt_fx.Text.Trim();


                        // Refresh data in GridView
                        grd_InvoiceDetail.EditIndex = -1;
                        grd_InvoiceDetail.DataSource = DataSource.Tables[invoiceDefaultDetail.TableName];
                        grd_InvoiceDetail.DataBind();
                    }
                }
            }
        }

        /// <summary>
        ///     InvoiceDefaul detail row editing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_InvoiceDetail_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Find grdivew 
            var grd_InvoiceDetail = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_InvoiceDetail");

            // Determine GridRow
            var grdGridRow = (GridViewRow) grd_InvoiceDetail.NamingContainer;

            // Binding gridview by depend on each of the selected row.
            ((GridView) grd_Invoice.Rows[grdGridRow.RowIndex].FindControl("grd_InvoiceDetail")).EditIndex =
                e.NewEditIndex;
            ((GridView) grd_Invoice.Rows[grdGridRow.RowIndex].FindControl("grd_InvoiceDetail")).DataSource =
                DataSource.Tables[invoiceDefaultDetail.TableName];
            grd_Invoice.Rows[grdGridRow.RowIndex].FindControl("grd_InvoiceDetail").DataBind();
        }

        /// <summary>
        ///     Invoicedefaul detail row deleting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_InvoiceDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Find grdivew 
            var grd_InvoiceDetail = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_InvoiceDetail");

            // Determine GridRow
            var grdGridRow = (GridViewRow) grd_InvoiceDetail.NamingContainer;

            // Setup primary key(s)
            var profileCode = grd_InvoiceDetail.DataKeys[grdGridRow.RowIndex].Values[0].ToString();
            var detailSeqNo = Convert.ToInt32(grd_InvoiceDetail.DataKeys[e.RowIndex].Values[1]);

            // Delete the record 
            for (var i = DataSource.Tables[invoiceDefaultDetail.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drDelete = DataSource.Tables[invoiceDefaultDetail.TableName].Rows[i];

                if (drDelete.RowState != DataRowState.Deleted)
                {
                    if (drDelete["ProfileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                        Convert.ToInt32(drDelete["DetailSeqNo"]) == detailSeqNo)
                    {
                        drDelete.Delete();
                    }
                }
            }

            // Binding grid
            // Binding gridview by depend on each of the selected row.
            ((GridView) grd_Invoice.Rows[grdGridRow.RowIndex].FindControl("grd_InvoiceDetail")).DataSource =
                DataSource.Tables[invoiceDefaultDetail.TableName];
            grd_Invoice.Rows[grdGridRow.RowIndex].FindControl("grd_InvoiceDetail").DataBind();
        }


        /// <summary>
        ///     Image button click event for show invoicedefaul detail.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void img_InvoiceDetail_Click(object sender, ImageClickEventArgs e)
        {
            var img_InvoiceDetail = (ImageButton) ((ImageButton) sender).Parent.Parent.FindControl("img_InvoiceDetail");
            var pnl_InvoiceDetail = (Panel) ((ImageButton) sender).Parent.Parent.FindControl("pnl_InvoiceDetail");
            var btn_InvoiceDetail = (Button) ((ImageButton) sender).Parent.Parent.FindControl("btn_InvoiceDetail");
            var grd_InvoiceDetail = (GridView) ((ImageButton) sender).Parent.Parent.FindControl("grd_InvoiceDetail");
            var lbl_Seq = (Label) ((ImageButton) sender).Parent.Parent.FindControl("lbl_Seq");


            img_InvoiceDetail.ImageUrl = string.Empty;

            if (pnl_InvoiceDetail != null)
            {
                if (pnl_InvoiceDetail.Visible)
                {
                    pnl_InvoiceDetail.Visible = false;
                    img_InvoiceDetail.ImageUrl = "~/App_Themes/default/pics/show_detail_icon.png";


                    grd_InvoiceDetail.DataSource = null;
                    grd_InvoiceDetail.DataBind();
                }
                else
                {
                    pnl_InvoiceDetail.Visible = true;
                    img_InvoiceDetail.ImageUrl = "~/App_Themes/default/pics/hide_detail_icon.png";

                    // if the readonly is true.not allow to create new process.
                    if (ReadOnly)
                    {
                        // visible for new button.
                        btn_InvoiceDetail.Visible = false;
                    }

                    if (lbl_Seq.Text != string.Empty)
                    {
                        GridViewRow grdGridRow;

                        int seqNo;

                        // Determine GridRow
                        grdGridRow = (GridViewRow) img_InvoiceDetail.NamingContainer;


                        // Derive DataKey from GridRow
                        seqNo = Convert.ToInt32(grd_Invoice.DataKeys[grdGridRow.RowIndex].Values[1]);

                        // assign seqno for public use.
                        SeqNo = seqNo;

                        // Clear table value before binding.
                        DataSource.Tables[invoiceDefaultDetail.TableName].Clear();

                        // Get invoicedefault detail by seqNo.
                        invoiceDefaultDetail.GetInvoiceDetailListBySeqNo(Convert.ToInt32(seqNo), DataSource,
                            LoginInfo.ConnStr);
                    }

                    grd_InvoiceDetail.DataSource = DataSource.Tables[invoiceDefaultDetail.TableName];
                    grd_InvoiceDetail.DataBind();
                }
            }
            else
            {
                img_InvoiceDetail.ImageUrl = "~/App_Themes/default/pics/show_detail_icon.png";
                MessageBox("Please try for update process complete !");
            }
        }


        /// <summary>
        ///     Invoice new click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Invoice_Click(object sender, EventArgs e)
        {
            var drNew = DataSource.Tables[invoiceDefault.TableName].NewRow();

            drNew["ProfileCode"] = ProfileCode.ToString();
            drNew["VendorCode"] = DataSource.Tables[vendor.TableName].Rows[0]["VendorCode"].ToString();
            drNew["SeqNO"] = invoiceDefault.GetInvoiceDefaultMaxSeq(LoginInfo.ConnStr);
            drNew["TransTypeCode"] = string.Empty;
            drNew["Description"] = string.Empty;


            // Add new row
            DataSource.Tables[invoiceDefault.TableName].Rows.Add(drNew);

            // Editing on new row
            grd_Invoice.DataSource = DataSource.Tables[invoiceDefault.TableName];
            grd_Invoice.EditIndex = grd_Invoice.Rows.Count;
            grd_Invoice.DataBind();
        }


        /// <summary>
        ///     Invoicedefault detail new click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_InvoiceDetail_Click(object sender, EventArgs e)
        {
            // Find button control.
            var btn_InvoiceDetail = (Button) ((Button) sender).Parent.Parent.FindControl("btn_InvoiceDetail");

            // Determine GridRow
            var grdGridRow = (GridViewRow) btn_InvoiceDetail.NamingContainer;

            var drNew = DataSource.Tables[invoiceDefaultDetail.TableName].NewRow();

            drNew["ProfileCode"] = ProfileCode.ToString();
            drNew["VendorCode"] = DataSource.Tables[vendor.TableName].Rows[0]["VendorCode"].ToString();
            drNew["SeqNO"] = SeqNo;
            drNew["DetailSeqNo"] = invoiceDefaultDetail.GetInvoiceDefaultDetailMaxSeq(LoginInfo.ConnStr);
            drNew["ItemName"] = string.Empty;
            drNew["AccountCode"] = string.Empty;
            drNew["Rate"] = Convert.ToDecimal("0.00");
            drNew["fx"] = string.Empty;


            // Add new row
            DataSource.Tables[invoiceDefaultDetail.TableName].Rows.Add(drNew);

            // Using dataview for make sorting.
            var dv = new DataView(DataSource.Tables[invoiceDefaultDetail.TableName]);
            dv.Sort = "SeqNo ASC";


            // Editing process after created the new event.
            ((GridView) grd_Invoice.Rows[grdGridRow.RowIndex].FindControl("grd_InvoiceDetail")).DataSource = dv;
            ((GridView) grd_Invoice.Rows[grdGridRow.RowIndex].FindControl("grd_InvoiceDetail")).EditIndex =
                ((GridView) grd_Invoice.Rows[grdGridRow.RowIndex].FindControl("grd_InvoiceDetail")).Rows.Count;
            grd_Invoice.Rows[grdGridRow.RowIndex].FindControl("grd_InvoiceDetail").DataBind();
        }


        /// <summary>
        ///     Get data.
        /// </summary>
        private void Page_Retrieve()
        {
        }

        /// <summary>
        ///     Display Invoicedefault data.
        /// </summary>
        private void Page_Setting()
        {
            // if the readonly is true.edit/delte/update/new process would not be allow.
            if (ReadOnly)
            {
                btn_Invoice.Visible = false;
            }

            grd_Invoice.DataSource = DataSource.Tables[invoiceDefault.TableName];
            grd_Invoice.DataBind();
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

        #endregion
    }
}