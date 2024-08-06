using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN
{
    public partial class DefaultPayment : BaseUserControl
    {
        #region "Attributies"

        private readonly Blue.BL.Reference.Account account = new Blue.BL.Reference.Account();
        private readonly Blue.BL.Bank.Bank bank = new Blue.BL.Bank.Bank();
        private readonly Blue.BL.Bank.BankAccountType bankAccountType = new Blue.BL.Bank.BankAccountType();
        private readonly Blue.BL.AP.PaymentDefault paymentDefault = new Blue.BL.AP.PaymentDefault();
        private readonly Blue.BL.AP.PaymentDefaultAuto paymentDefaultAuto = new Blue.BL.AP.PaymentDefaultAuto();
        private readonly Blue.BL.AP.PaymentDefaultCash paymentDefaultCash = new Blue.BL.AP.PaymentDefaultCash();
        private readonly Blue.BL.AP.PaymentDefaultCheq paymentDefaultCheq = new Blue.BL.AP.PaymentDefaultCheq();
        private readonly Blue.BL.AP.PaymentDefaultCredit paymentDefaultCredit = new Blue.BL.AP.PaymentDefaultCredit();
        private readonly Blue.BL.AP.PaymentDefaultTrans paymentDefaultTrans = new Blue.BL.AP.PaymentDefaultTrans();
        private readonly Blue.BL.Reference.PaymentMethod paymentMethod = new Blue.BL.Reference.PaymentMethod();
        private readonly Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();
        private readonly Blue.BL.AP.VendorDefaultWHT vendorDefaultWHT = new Blue.BL.AP.VendorDefaultWHT();
        private readonly Blue.BL.Reference.WhtType whtType = new Blue.BL.Reference.WhtType();


        private DataSet _dataSource = new DataSet();
        private Guid _profileCode;

        private bool _readOnly;
        private bool _readonly = false;

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
        ///     Payment rowdatabound event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Payment_RowDataBound(object sender, GridViewRowEventArgs e)
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
                if (e.Row.FindControl("lbl_SeqPayment") != null)
                {
                    var lbl_SeqPayment = (Label) e.Row.FindControl("lbl_SeqPayment");
                    lbl_SeqPayment.Text = DataBinder.Eval(e.Row.DataItem, "SeqNo").ToString();
                }

                // PaymentMethod
                if (e.Row.FindControl("lbl_PaymentMethod") != null)
                {
                    var lbl_PaymentMethod = (Label) e.Row.FindControl("lbl_PaymentMethod");
                    lbl_PaymentMethod.Text =
                        paymentMethod.GetPaymentMethodName(
                            DataBinder.Eval(e.Row.DataItem, "PaymentMethodCode").ToString(), LoginInfo.ConnStr);
                }

                // PaymentMethod
                if (e.Row.FindControl("hid_PaymentMethod") != null)
                {
                    var hid_PaymentMethod = (HiddenField) e.Row.FindControl("hid_PaymentMethod");
                    hid_PaymentMethod.Value = DataBinder.Eval(e.Row.DataItem, "PaymentMethodCode").ToString();
                }


                // Transactiontype
                if (e.Row.FindControl("ddl_PaymentMethod") != null)
                {
                    var ddl_PaymentMethod = (DropDownList) e.Row.FindControl("ddl_PaymentMethod");
                    ddl_PaymentMethod.DataSource = paymentMethod.GetPaymentMethodListForLookUp(LoginInfo.ConnStr);
                    ddl_PaymentMethod.DataTextField = "Name";
                    ddl_PaymentMethod.DataValueField = "PaymentMethodCode";
                    ddl_PaymentMethod.DataBind();
                    ddl_PaymentMethod.SelectedValue = DataBinder.Eval(e.Row.DataItem, "PaymentMethodCode").ToString();
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
        ///     Payment rowcanceling event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Payment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Setup primary key(s)
            var vendorCode = grd_Payment.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_Payment.DataKeys[e.RowIndex].Values[1]);

            // Delete new row
            for (var i = DataSource.Tables[paymentDefault.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = DataSource.Tables[paymentDefault.TableName].Rows[i];

                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["VendorCode"].ToString().ToUpper() == vendorCode.ToUpper() &&
                        Convert.ToInt32(dr["SeqNo"]) == seqNo &&
                        dr.RowState == DataRowState.Added)
                    {
                        dr.Delete();
                        break;
                    }
                }
            }

            // Refresh data in gridview
            grd_Payment.DataSource = DataSource.Tables[paymentDefault.TableName];
            grd_Payment.EditIndex = -1;
            grd_Payment.DataBind();
        }

        /// <summary>
        ///     Payment rowupdating event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Payment_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Setup primary key(s)
            var vendorCode = grd_Payment.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_Payment.DataKeys[e.RowIndex].Values[1]);


            foreach (DataRow drUpdating in DataSource.Tables[paymentDefault.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["VendorCode"].ToString().ToUpper() == vendorCode.ToUpper() &&
                        Convert.ToInt32(drUpdating["SeqNo"]) == seqNo)
                    {
                        // Find controls inside of gridview.
                        var lbl_SeqPayment = (Label) grd_Payment.Rows[e.RowIndex].FindControl("lbl_SeqPayment");
                        var ddl_PaymentMethod =
                            (DropDownList) grd_Payment.Rows[e.RowIndex].FindControl("ddl_PaymentMethod");
                        var txt_Description = (TextBox) grd_Payment.Rows[e.RowIndex].FindControl("txt_Description");


                        // Updating for assign data.
                        // SeqNo
                        drUpdating["SeqNo"] = Convert.ToInt32(lbl_SeqPayment.Text);

                        // TransTypeCode
                        drUpdating["PaymentMethodCode"] = ddl_PaymentMethod.SelectedItem.Value;

                        // Description
                        drUpdating["Description"] = txt_Description.Text.Trim();


                        // Refresh data in GridView
                        grd_Payment.EditIndex = -1;
                        grd_Payment.DataSource = DataSource.Tables[paymentDefault.TableName];
                        grd_Payment.DataBind();
                    }
                }
            }
        }


        /// <summary>
        ///     Payment row editing event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Payment_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grd_Payment.EditIndex = e.NewEditIndex;
            grd_Payment.DataSource = DataSource.Tables[paymentDefault.TableName];
            grd_Payment.DataBind();
        }

        /// <summary>
        ///     Payment row deleting event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Payment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Setup primary key(s)
            var vendorCode = grd_Payment.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_Payment.DataKeys[e.RowIndex].Values[1]);

            // Delete the record 
            for (var i = DataSource.Tables[paymentDefault.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drDelete = DataSource.Tables[paymentDefault.TableName].Rows[i];

                if (drDelete.RowState != DataRowState.Deleted)
                {
                    if (drDelete["VendorCode"].ToString().ToUpper() == vendorCode.ToUpper() &&
                        Convert.ToInt32(drDelete["SeqNo"]) == seqNo)
                    {
                        drDelete.Delete();
                    }
                }
            }

            // Binding grid
            grd_Payment.DataSource = DataSource.Tables[paymentDefault.TableName];
            grd_Payment.DataBind();
        }


        /// <summary>
        ///     Button payment click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Payment_Click(object sender, EventArgs e)
        {
            var drNew = DataSource.Tables[paymentDefault.TableName].NewRow();

            drNew["ProfileCode"] = ProfileCode.ToString();
            drNew["VendorCode"] = DataSource.Tables[vendor.TableName].Rows[0]["VendorCode"].ToString();
            drNew["SeqNO"] = paymentDefault.GetPaymentDefaultMaxSeq(LoginInfo.ConnStr);
            drNew["PaymentMethodCode"] = string.Empty;
            drNew["Description"] = string.Empty;


            // Add new row
            DataSource.Tables[paymentDefault.TableName].Rows.Add(drNew);

            // Editing on new row
            grd_Payment.DataSource = DataSource.Tables[paymentDefault.TableName];
            grd_Payment.EditIndex = grd_Payment.Rows.Count;
            grd_Payment.DataBind();

            // Get gridview editindex.
            var index = grd_Payment.EditIndex;

            // Enable assign true for paymentmethod dropdownlist.
            if (grd_Payment.Rows[index].FindControl("ddl_PaymentMethod") != null)
            {
                var ddl_PaymentMethod = ((DropDownList) grd_Payment.Rows[index].FindControl("ddl_PaymentMethod"));
                ddl_PaymentMethod.Enabled = true;
            }
        }

        /// <summary>
        ///     Payment Cash rowdatabound event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentCash_RowDataBound(object sender, GridViewRowEventArgs e)
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


                // Paymentaccount
                if (e.Row.FindControl("lbl_PaymentAccount") != null)
                {
                    var lbl_PaymentAccount = (Label) e.Row.FindControl("lbl_PaymentAccount");
                    lbl_PaymentAccount.Text = DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString();
                }

                // Paymentaccountname
                if (e.Row.FindControl("lbl_PaymentAccount_Name") != null)
                {
                    var lbl_PaymentAccount_Name = (Label) e.Row.FindControl("lbl_PaymentAccount_Name");
                    lbl_PaymentAccount_Name.Text =
                        account.GetName(DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString(), LoginInfo.ConnStr);
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
            }
        }


        /// <summary>
        ///     paymentDefaultCash row canceling event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentCash_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Find grdivew 
            var grd_PaymentCash = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentCash");

            // Setup primary key(s)
            var profileCode = grd_PaymentCash.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_PaymentCash.DataKeys[e.RowIndex].Values[1]);

            // Delete new row
            for (var i = DataSource.Tables[paymentDefaultCash.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = DataSource.Tables[paymentDefaultCash.TableName].Rows[i];

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
            grd_PaymentCash.DataSource = DataSource.Tables[paymentDefaultCash.TableName];
            grd_PaymentCash.EditIndex = -1;
            grd_PaymentCash.DataBind();
        }

        /// <summary>
        ///     paymentDefaultCash rowupdating event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentCash_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Find grdivew 
            var grd_PaymentCash = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentCash");


            // Setup primary key(s)
            var profileCode = grd_PaymentCash.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_PaymentCash.DataKeys[e.RowIndex].Values[1]);


            foreach (DataRow drUpdating in DataSource.Tables[paymentDefaultCash.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["ProfileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                        Convert.ToInt32(drUpdating["SeqNo"]) == seqNo)
                    {
                        // Find controls inside of gridview.
                        var ddl_AccountList =
                            (DropDownList) grd_PaymentCash.Rows[e.RowIndex].FindControl("ddl_AccountList");


                        // Updating for assign data.
                        // AccountCode
                        drUpdating["AccountCode"] = ddl_AccountList.SelectedItem.Value;


                        // Refresh data in GridView
                        grd_PaymentCash.EditIndex = -1;
                        grd_PaymentCash.DataSource = DataSource.Tables[paymentDefaultCash.TableName];
                        grd_PaymentCash.DataBind();
                    }
                }
            }
        }


        /// <summary>
        ///     paymentDefaultCash row editing event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentCash_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Find grdivew 
            var grd_PaymentCash = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentCash");

            // Determine GridRow
            var grdGridRow = (GridViewRow) grd_PaymentCash.NamingContainer;

            // Binding gridview by depend on each of the selected row.
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCash")).EditIndex = e.NewEditIndex;
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCash")).DataSource =
                DataSource.Tables[paymentDefaultCash.TableName];
            grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCash").DataBind();
        }

        /// <summary>
        ///     paymentDefaultCash row deleting event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentCash_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Find grdivew 
            var grd_PaymentCash = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentCash");


            // Setup primary key(s)
            var profileCode = grd_PaymentCash.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_PaymentCash.DataKeys[e.RowIndex].Values[1]);

            // Delete the record 
            for (var i = DataSource.Tables[paymentDefaultCash.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drDelete = DataSource.Tables[paymentDefaultCash.TableName].Rows[i];

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
            grd_PaymentCash.DataSource = DataSource.Tables[paymentDefaultCash.TableName];
            grd_PaymentCash.DataBind();
        }


        /// <summary>
        ///     PaymentDefaultCash new event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_PaymentCash_Click(object sender, EventArgs e)
        {
            // Find button control.
            var btn_PaymentCash = (Button) ((Button) sender).Parent.Parent.FindControl("btn_PaymentCash");

            // Determine GridRow
            var grdGridRow = (GridViewRow) btn_PaymentCash.NamingContainer;

            var drNew = DataSource.Tables[paymentDefaultCash.TableName].NewRow();

            drNew["ProfileCode"] = ProfileCode;
            drNew["VendorCode"] = DataSource.Tables[vendor.TableName].Rows[0]["VendorCode"].ToString();
            drNew["SeqNo"] = paymentDefaultCash.GetPaymentDefaultCashMaxSeqNo(LoginInfo.ConnStr);
            drNew["AccountCode"] = "AccountCode";


            // Add new row
            DataSource.Tables[paymentDefaultCash.TableName].Rows.Add(drNew);


            // Editing process after created the new event.
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCash")).DataSource =
                DataSource.Tables[paymentDefaultCash.TableName];
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCash")).EditIndex =
                ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCash")).Rows.Count;
            grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCash").DataBind();
        }


        /// <summary>
        ///     PaymentCheque new event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_PaymentCheque_Click(object sender, EventArgs e)
        {
            // Find button control.
            var btn_PaymentCheque = (Button) ((Button) sender).Parent.Parent.FindControl("btn_PaymentCheque");

            // Determine GridRow
            var grdGridRow = (GridViewRow) btn_PaymentCheque.NamingContainer;

            var drNew = DataSource.Tables[paymentDefaultCheq.TableName].NewRow();

            drNew["ProfileCode"] = ProfileCode;
            drNew["VendorCode"] = DataSource.Tables[vendor.TableName].Rows[0]["VendorCode"].ToString();
            drNew["SeqNo"] = paymentDefaultCheq.GetPaymentDefaultCheqMaxSeqNo(LoginInfo.ConnStr);
            drNew["AccountCode"] = string.Empty;
            drNew["BankCode"] = string.Empty;


            // Add new row
            DataSource.Tables[paymentDefaultCheq.TableName].Rows.Add(drNew);


            // Editing process after created the new event.
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCheque")).DataSource =
                DataSource.Tables[paymentDefaultCheq.TableName];
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCheque")).EditIndex =
                ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCheque")).Rows.Count;
            grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCheque").DataBind();
        }


        /// <summary>
        ///     PaymentCredit new event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_PaymentCredit_Click(object sender, EventArgs e)
        {
            // Find button control.
            var btn_PaymentCredit = (Button) ((Button) sender).Parent.Parent.FindControl("btn_PaymentCredit");

            // Determine GridRow
            var grdGridRow = (GridViewRow) btn_PaymentCredit.NamingContainer;

            var drNew = DataSource.Tables[paymentDefaultCredit.TableName].NewRow();

            drNew["ProfileCode"] = ProfileCode;
            drNew["VendorCode"] = DataSource.Tables[vendor.TableName].Rows[0]["VendorCode"].ToString();
            drNew["SeqNo"] = paymentDefaultCredit.GetPaymentDefaultCreditMaxSeqNo(LoginInfo.ConnStr);
            drNew["AccountCode"] = "AccountCode";
            drNew["CardNo"] = "CardNo";
            drNew["CardHolder"] = "CardHolder";
            drNew["Type"] = "Type";
            drNew["BankCode"] = "BankCode";
            drNew["Valid"] = "Vali";
            drNew["Expire"] = "Expi";


            // Add new row
            DataSource.Tables[paymentDefaultCredit.TableName].Rows.Add(drNew);

            // Editing process after created the new event.
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCredit")).DataSource =
                DataSource.Tables[paymentDefaultCredit.TableName];
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCredit")).EditIndex =
                ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCredit")).Rows.Count;
            grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCredit").DataBind();
        }


        /// <summary>
        ///     AutoPayment new event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_AutoPayment_Click(object sender, EventArgs e)
        {
            // Find button control.
            var btn_AutoPayment = (Button) ((Button) sender).Parent.Parent.FindControl("btn_AutoPayment");

            // Determine GridRow
            var grdGridRow = (GridViewRow) btn_AutoPayment.NamingContainer;

            var drNew = DataSource.Tables[paymentDefaultAuto.TableName].NewRow();

            drNew["ProfileCode"] = ProfileCode;
            drNew["VendorCode"] = DataSource.Tables[vendor.TableName].Rows[0]["VendorCode"].ToString();
            drNew["SeqNo"] = paymentDefaultAuto.GetPaymentDefaultAutoMaxSeqNo(LoginInfo.ConnStr);
            drNew["AccountCode"] = string.Empty;
            drNew["BankCode"] = string.Empty;
            drNew["BranchCode"] = string.Empty;
            drNew["AccountNo"] = string.Empty;
            drNew["AccountName"] = string.Empty;
            drNew["AccountTypeCode"] = string.Empty;
            drNew["RenewalDate"] = DateTime.MinValue;
            drNew["Limit"] = System.DBNull.Value;
            drNew["DebtorRef"] = string.Empty;


            // Add new row
            DataSource.Tables[paymentDefaultAuto.TableName].Rows.Add(drNew);

            // Editing process after created the new event.
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_AutoPayment")).DataSource =
                DataSource.Tables[paymentDefaultAuto.TableName];
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_AutoPayment")).EditIndex =
                ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_AutoPayment")).Rows.Count;
            grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_AutoPayment").DataBind();
        }


        /// <summary>
        ///     PaymentTransfer click new event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Transfer_Click(object sender, EventArgs e)
        {
            // Find button control.
            var btn_Transfer = (Button) ((Button) sender).Parent.Parent.FindControl("btn_Transfer");

            // Determine GridRow
            var grdGridRow = (GridViewRow) btn_Transfer.NamingContainer;

            var drNew = DataSource.Tables[paymentDefaultTrans.TableName].NewRow();

            drNew["ProfileCode"] = ProfileCode;
            drNew["VendorCode"] = DataSource.Tables[vendor.TableName].Rows[0]["VendorCode"].ToString();
            drNew["SeqNo"] = paymentDefaultTrans.GetPaymentDefaultTransMaxSeqNo(LoginInfo.ConnStr);
            drNew["AccountCode"] = "AccountCode";
            drNew["BankCode"] = "BankCode";
            drNew["BranchCode"] = string.Empty;
            drNew["AccountNo"] = "AccountNo";
            drNew["AccountName"] = "AccountName";
            drNew["AccountTypeCode"] = "TypeCode";
            drNew["VBankCode"] = string.Empty;
            drNew["VBranchCode"] = string.Empty;
            drNew["VAccountNo"] = "VAccountNo";
            drNew["VAccountName"] = string.Empty;
            drNew["VAccountTypeCode"] = string.Empty;

            // Add new row
            DataSource.Tables[paymentDefaultTrans.TableName].Rows.Add(drNew);

            // Editing process after created the new event.
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentTransfer")).DataSource =
                DataSource.Tables[paymentDefaultTrans.TableName];
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentTransfer")).EditIndex =
                ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentTransfer")).Rows.Count;
            grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentTransfer").DataBind();


            // Editing process after created the new event.
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentTransferVendor")).DataSource =
                DataSource.Tables[paymentDefaultTrans.TableName];
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentTransferVendor")).EditIndex =
                ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentTransferVendor")).Rows.Count;
            grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentTransferVendor").DataBind();
        }


        /// <summary>
        ///     Withholding new event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Withholding_Click(object sender, EventArgs e)
        {
            // Find button control.
            var btn_Withholding = (Button) ((Button) sender).Parent.Parent.FindControl("btn_Withholding");

            // Determine GridRow
            var grdGridRow = (GridViewRow) btn_Withholding.NamingContainer;

            var drNew = DataSource.Tables[vendorDefaultWHT.TableName].NewRow();

            drNew["ProfileCode"] = ProfileCode;
            drNew["VendorCode"] = DataSource.Tables[vendor.TableName].Rows[0]["VendorCode"].ToString();
            drNew["SeqNo"] = vendorDefaultWHT.GetVendorDefaultWHTMaxSeqNo(LoginInfo.ConnStr);
            drNew["WHTTaxTypeCode"] = "WHTTax";
            drNew["Rate"] = Convert.ToDecimal("0.00");
            drNew["AccountCode"] = "AccountCode";


            // Add new row
            DataSource.Tables[vendorDefaultWHT.TableName].Rows.Add(drNew);

            // Editing process after created the new event.
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentWithholding")).DataSource =
                DataSource.Tables[vendorDefaultWHT.TableName];
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentWithholding")).EditIndex =
                ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentWithholding")).Rows.Count;
            grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentWithholding").DataBind();
        }


        /// <summary>
        ///     Autopayment rowdatabound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_AutoPayment_RowDataBound(object sender, GridViewRowEventArgs e)
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

                // BankCode
                if (e.Row.FindControl("txt_AutoBankCode") != null)
                {
                    var txt_AutoBankCode = (TextBox) e.Row.FindControl("txt_AutoBankCode");
                    txt_AutoBankCode.Text = DataBinder.Eval(e.Row.DataItem, "BankCode").ToString();
                }

                // AutoBankCode
                if (e.Row.FindControl("ddl_AutoBankCode") != null)
                {
                    var ddl_AutoBankCode = (DropDownList) e.Row.FindControl("ddl_AutoBankCode");

                    ddl_AutoBankCode.DataSource = bank.GetBankList(LoginInfo.ConnStr);
                    ddl_AutoBankCode.DataTextField = "Name";
                    ddl_AutoBankCode.DataValueField = "BankCode";
                    ddl_AutoBankCode.DataBind();
                    ddl_AutoBankCode.SelectedValue = DataBinder.Eval(e.Row.DataItem, "BankCode").ToString();
                }


                // BankCode
                if (e.Row.FindControl("lbl_AutoBankCode") != null)
                {
                    var lbl_AutoBankCode = (Label) e.Row.FindControl("lbl_AutoBankCode");
                    lbl_AutoBankCode.Text = DataBinder.Eval(e.Row.DataItem, "BankCode").ToString();
                }

                // BankName
                if (e.Row.FindControl("txt_AutoBankName") != null)
                {
                    var txt_AutoBankName = (TextBox) e.Row.FindControl("txt_AutoBankName");
                    txt_AutoBankName.Text = bank.GetBankName(DataBinder.Eval(e.Row.DataItem, "BankCode").ToString(),
                        LoginInfo.ConnStr);
                }


                // BankName
                if (e.Row.FindControl("lbl_AutoBankName") != null)
                {
                    var lbl_AutoBankName = (Label) e.Row.FindControl("lbl_AutoBankName");
                    lbl_AutoBankName.Text = bank.GetBankName(DataBinder.Eval(e.Row.DataItem, "BankCode").ToString(),
                        LoginInfo.ConnStr);
                }


                // AutoBranchCode
                if (e.Row.FindControl("ddl_AutoBranchName") != null)
                {
                    var ddl_AutoBranchName = (DropDownList) e.Row.FindControl("ddl_AutoBranchName");

                    ddl_AutoBranchName.DataSource = bank.GetBankList(LoginInfo.ConnStr);
                    ddl_AutoBranchName.DataTextField = "BranchName";
                    ddl_AutoBranchName.DataValueField = "BranchCode";
                    ddl_AutoBranchName.DataBind();
                    ddl_AutoBranchName.SelectedValue = DataBinder.Eval(e.Row.DataItem, "BranchCode").ToString();
                }

                // BranchCode
                if (e.Row.FindControl("lbl_AutoBranchCode") != null)
                {
                    var lbl_AutoBranchCode = (Label) e.Row.FindControl("lbl_AutoBranchCode");
                    lbl_AutoBranchCode.Text = DataBinder.Eval(e.Row.DataItem, "BranchCode").ToString();
                }


                // BranchName
                if (e.Row.FindControl("lbl_AutoBranchName") != null)
                {
                    var lbl_AutoBranchName = (Label) e.Row.FindControl("lbl_AutoBranchName");
                    lbl_AutoBranchName.Text =
                        bank.GetBranchName(DataBinder.Eval(e.Row.DataItem, "BranchCode").ToString(), LoginInfo.ConnStr);
                }

                // AccountNo
                if (e.Row.FindControl("txt_Account") != null)
                {
                    var txt_Account = (TextBox) e.Row.FindControl("txt_Account");
                    txt_Account.Text = DataBinder.Eval(e.Row.DataItem, "AccountNo").ToString();
                }

                // AccountNo
                if (e.Row.FindControl("lbl_Account") != null)
                {
                    var lbl_Account = (Label) e.Row.FindControl("lbl_Account");
                    lbl_Account.Text = DataBinder.Eval(e.Row.DataItem, "AccountNo").ToString();
                }


                // AccountNo
                if (e.Row.FindControl("lbl_AccountType") != null)
                {
                    var lbl_AccountType = (Label) e.Row.FindControl("lbl_AccountType");
                    lbl_AccountType.Text =
                        bankAccountType.GetBankAccountTypeName(
                            DataBinder.Eval(e.Row.DataItem, "AccountTypeCode").ToString(), LoginInfo.ConnStr);
                }


                // AccountTypeCode
                if (e.Row.FindControl("ddl_AccountType") != null)
                {
                    var ddl_AccountType = (DropDownList) e.Row.FindControl("ddl_AccountType");

                    ddl_AccountType.DataSource = bankAccountType.GetBankAccountTypeList(LoginInfo.ConnStr);
                    ddl_AccountType.DataTextField = "Name";
                    ddl_AccountType.DataValueField = "BankAccountTypeCode";
                    ddl_AccountType.DataBind();
                    ddl_AccountType.SelectedValue = DataBinder.Eval(e.Row.DataItem, "AccountTypeCode").ToString();
                }


                // AccountName
                if (e.Row.FindControl("txt_AccountName") != null)
                {
                    var txt_AccountName = (TextBox) e.Row.FindControl("txt_AccountName");
                    txt_AccountName.Text = DataBinder.Eval(e.Row.DataItem, "AccountName").ToString();
                }


                // AccountName
                if (e.Row.FindControl("lbl_AccountName") != null)
                {
                    var lbl_AccountName = (Label) e.Row.FindControl("lbl_AccountName");
                    lbl_AccountName.Text = DataBinder.Eval(e.Row.DataItem, "AccountName").ToString();
                }

                // RenewalDate
                if (e.Row.FindControl("txt_RenewalDate") != null)
                {
                    var txt_RenewalDate = (TextBox) e.Row.FindControl("txt_RenewalDate");
                    txt_RenewalDate.Text =
                        ((DateTime) DataBinder.Eval(e.Row.DataItem, "RenewalDate")).ToString("dd/MM/yyyy");
                        //Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "RenewalDate"));
                }


                // RenewalDate
                if (e.Row.FindControl("lbl_RenewalDate") != null)
                {
                    var lbl_RenewalDate = (Label) e.Row.FindControl("lbl_RenewalDate");
                    lbl_RenewalDate.Text =
                        ((DateTime) DataBinder.Eval(e.Row.DataItem, "RenewalDate")).ToString("dd/MM/yyyy");
                        //Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "RenewalDate"));
                }


                // limit
                if (e.Row.FindControl("txt_Limit") != null)
                {
                    var txt_Limit = (TextBox) e.Row.FindControl("txt_Limit");
                    txt_Limit.Text = DataBinder.Eval(e.Row.DataItem, "Limit").ToString();
                }

                // limit
                if (e.Row.FindControl("lbl_Limit") != null)
                {
                    var lbl_Limit = (Label) e.Row.FindControl("lbl_Limit");
                    lbl_Limit.Text = DataBinder.Eval(e.Row.DataItem, "Limit").ToString();
                }


                // AccountTypeCode
                if (e.Row.FindControl("ddl_AutoAccount") != null)
                {
                    var ddl_AutoAccount = (DropDownList) e.Row.FindControl("ddl_AutoAccount");

                    ddl_AutoAccount.DataSource = account.GetAccountListLookup(LoginInfo.ConnStr);
                    ddl_AutoAccount.DataTextField = "NameEng";
                    ddl_AutoAccount.DataValueField = "AccountCode";
                    ddl_AutoAccount.DataBind();
                    ddl_AutoAccount.SelectedValue = DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString();
                }


                // Paymentaccount
                if (e.Row.FindControl("lbl_AutoAccount") != null)
                {
                    var lbl_AutoAccount = (Label) e.Row.FindControl("lbl_AutoAccount");
                    lbl_AutoAccount.Text = DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString();
                }


                // Paymentaccountname
                if (e.Row.FindControl("lbl_AutoAccount_Name") != null)
                {
                    var lbl_AutoAccount_Name = (Label) e.Row.FindControl("lbl_AutoAccount_Name");
                    lbl_AutoAccount_Name.Text =
                        account.GetName(DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString(), LoginInfo.ConnStr);
                }


                // DebtorReference
                if (e.Row.FindControl("txt_DebtorReference") != null)
                {
                    var txt_DebtorReference = (TextBox) e.Row.FindControl("txt_DebtorReference");
                    txt_DebtorReference.Text = DataBinder.Eval(e.Row.DataItem, "DebtorRef").ToString();
                }

                // DebtorReference
                if (e.Row.FindControl("lbl_DebtorReference") != null)
                {
                    var lbl_DebtorReference = (Label) e.Row.FindControl("lbl_DebtorReference");
                    lbl_DebtorReference.Text = DataBinder.Eval(e.Row.DataItem, "DebtorRef").ToString();
                }
            }
        }

        /// <summary>
        ///     paymentDefaultAuto row canceling event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_AutoPayment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Find grdivew 
            var grd_AutoPayment = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_AutoPayment");

            // Setup primary key(s)
            var profileCode = grd_AutoPayment.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_AutoPayment.DataKeys[e.RowIndex].Values[1]);

            // Delete new row
            for (var i = DataSource.Tables[paymentDefaultAuto.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = DataSource.Tables[paymentDefaultAuto.TableName].Rows[i];

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
            grd_AutoPayment.DataSource = DataSource.Tables[paymentDefaultAuto.TableName];
            grd_AutoPayment.EditIndex = -1;
            grd_AutoPayment.DataBind();
        }

        /// <summary>
        ///     paymentDefaultAuto rowupdating event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_AutoPayment_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Find grdivew 
            var grd_AutoPayment = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_AutoPayment");

            // Setup primary key(s)
            var profileCode = grd_AutoPayment.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_AutoPayment.DataKeys[e.RowIndex].Values[1]);


            foreach (DataRow drUpdating in DataSource.Tables[paymentDefaultAuto.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["ProfileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                        Convert.ToInt32(drUpdating["SeqNo"]) == seqNo)
                    {
                        // Find controls inside of gridview.
                        var ddl_AutoBankCode =
                            (DropDownList) grd_AutoPayment.Rows[e.RowIndex].FindControl("ddl_AutoBankCode");
                        var ddl_AutoBranchName =
                            (DropDownList) grd_AutoPayment.Rows[e.RowIndex].FindControl("ddl_AutoBranchName");
                        var txt_Account = (TextBox) grd_AutoPayment.Rows[e.RowIndex].FindControl("txt_Account");
                        var ddl_AccountType =
                            (DropDownList) grd_AutoPayment.Rows[e.RowIndex].FindControl("ddl_AccountType");
                        var txt_AccountName = (TextBox) grd_AutoPayment.Rows[e.RowIndex].FindControl("txt_AccountName");
                        var txt_RenewalDate = (TextBox) grd_AutoPayment.Rows[e.RowIndex].FindControl("txt_RenewalDate");
                        var txt_Limit = (TextBox) grd_AutoPayment.Rows[e.RowIndex].FindControl("txt_Limit");
                        var ddl_AutoAccount =
                            (DropDownList) grd_AutoPayment.Rows[e.RowIndex].FindControl("ddl_AutoAccount");
                        var txt_DebtorReference =
                            (TextBox) grd_AutoPayment.Rows[e.RowIndex].FindControl("txt_DebtorReference");


                        // Updating for assign data.
                        // BankCode
                        drUpdating["BankCode"] = ddl_AutoBankCode.SelectedItem.Value;

                        // BranchCode
                        drUpdating["BranchCode"] = ddl_AutoBranchName.SelectedItem.Value;

                        // AccountNo
                        drUpdating["AccountNo"] = txt_Account.Text.Trim();


                        // AccountTypeCode
                        drUpdating["AccountTypeCode"] = ddl_AccountType.SelectedItem.Value;

                        // AccountName
                        drUpdating["AccountName"] = txt_AccountName.Text.Trim();

                        // RenewalDate
                        drUpdating["RenewalDate"] = DateTime.Parse(txt_RenewalDate.Text,
                            System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);

                        // Limit
                        drUpdating["Limit"] = txt_Limit.Text.Trim();

                        // AccountCode
                        drUpdating["AccountCode"] = ddl_AutoAccount.SelectedItem.Value;

                        // DebtorRef
                        drUpdating["DebtorRef"] = txt_DebtorReference.Text.Trim();

                        // Refresh data in GridView
                        grd_AutoPayment.EditIndex = -1;
                        grd_AutoPayment.DataSource = DataSource.Tables[paymentDefaultAuto.TableName];
                        grd_AutoPayment.DataBind();
                    }
                }
            }
        }


        /// <summary>
        ///     paymentDefaultAuto row editing event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_AutoPayment_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Find grdivew 
            var grd_AutoPayment = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_AutoPayment");

            // Determine GridRow
            var grdGridRow = (GridViewRow) grd_AutoPayment.NamingContainer;

            // Binding gridview by depend on each of the selected row.
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_AutoPayment")).EditIndex = e.NewEditIndex;
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_AutoPayment")).DataSource =
                DataSource.Tables[paymentDefaultAuto.TableName];
            grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_AutoPayment").DataBind();
        }

        /// <summary>
        ///     paymentDefaultAuto row deleting event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_AutoPayment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Nested gridview need to find grdivew again.
            var grd_AutoPayment = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_AutoPayment");

            // Setup primary key(s)
            var profileCode = grd_AutoPayment.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_AutoPayment.DataKeys[e.RowIndex].Values[1]);

            // Delete the record 
            for (var i = DataSource.Tables[paymentDefaultAuto.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drDelete = DataSource.Tables[paymentDefaultAuto.TableName].Rows[i];

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
            grd_AutoPayment.DataSource = DataSource.Tables[paymentDefaultAuto.TableName];
            grd_AutoPayment.DataBind();
        }


        /// <summary>
        ///     Paymentcredit rowdatabound event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentCredit_RowDataBound(object sender, GridViewRowEventArgs e)
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


                // Card
                if (e.Row.FindControl("txt_Card") != null)
                {
                    var txt_Card = (TextBox) e.Row.FindControl("txt_Card");
                    txt_Card.Text = DataBinder.Eval(e.Row.DataItem, "CardNo").ToString();
                }

                // Card
                if (e.Row.FindControl("lbl_Card") != null)
                {
                    var lbl_Card = (Label) e.Row.FindControl("lbl_Card");
                    lbl_Card.Text = DataBinder.Eval(e.Row.DataItem, "CardNo").ToString();
                }

                // Type
                if (e.Row.FindControl("lbl_Type") != null)
                {
                    var lbl_Type = (Label) e.Row.FindControl("lbl_Type");
                    lbl_Type.Text = DataBinder.Eval(e.Row.DataItem, "Type").ToString();
                }

                // Type
                if (e.Row.FindControl("ddl_Type") != null)
                {
                    var ddl_Type = (DropDownList) e.Row.FindControl("ddl_Type");
                    ddl_Type.Items.Add(new ListItem("VISA", "VISA"));
                    ddl_Type.Items.Add(new ListItem("MASTER", "MASTER"));
                    ddl_Type.DataBind();
                    ddl_Type.SelectedValue = DataBinder.Eval(e.Row.DataItem, "Type").ToString();
                }


                // CardHolder
                if (e.Row.FindControl("txt_CardHolder") != null)
                {
                    var txt_CardHolder = (TextBox) e.Row.FindControl("txt_CardHolder");
                    txt_CardHolder.Text = DataBinder.Eval(e.Row.DataItem, "CardHolder").ToString();
                }

                // CardHolder
                if (e.Row.FindControl("lbl_CardHolder") != null)
                {
                    var lbl_CardHolder = (Label) e.Row.FindControl("lbl_CardHolder");
                    lbl_CardHolder.Text = DataBinder.Eval(e.Row.DataItem, "CardHolder").ToString();
                }


                // BankName
                if (e.Row.FindControl("ddl_CreditBankName") != null)
                {
                    var ddl_CreditBankName = (DropDownList) e.Row.FindControl("ddl_CreditBankName");
                    ddl_CreditBankName.DataSource = bank.GetBankList(LoginInfo.ConnStr);
                    ddl_CreditBankName.DataTextField = "Name";
                    ddl_CreditBankName.DataValueField = "BankCode";
                    ddl_CreditBankName.DataBind();
                    ddl_CreditBankName.SelectedValue = DataBinder.Eval(e.Row.DataItem, "BankCode").ToString();
                }

                // BankName
                if (e.Row.FindControl("lbl_CreditBankName") != null)
                {
                    var lbl_CreditBankName = (Label) e.Row.FindControl("lbl_CreditBankName");
                    lbl_CreditBankName.Text = bank.GetBankName(DataBinder.Eval(e.Row.DataItem, "BankCode").ToString(),
                        LoginInfo.ConnStr);
                }

                // ValidFormMonth
                if (e.Row.FindControl("lbl_ValidFormMonth") != null)
                {
                    var lbl_ValidFormMonth = (Label) e.Row.FindControl("lbl_ValidFormMonth");
                    lbl_ValidFormMonth.Text = DataBinder.Eval(e.Row.DataItem, "Valid").ToString().Substring(0, 2);
                }

                // ValidFormMonth
                if (e.Row.FindControl("ddl_ValidFormMonth") != null)
                {
                    var ddl_ValidFormMonth = (DropDownList) e.Row.FindControl("ddl_ValidFormMonth");

                    for (var i = 1; i <= 12; i++)
                    {
                        if (i < 10)
                        {
                            var j = "0" + i;
                            ddl_ValidFormMonth.Items.Add(new ListItem(j, j));
                        }
                        else
                        {
                            ddl_ValidFormMonth.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        }
                    }

                    ddl_ValidFormMonth.DataBind();
                    ddl_ValidFormMonth.SelectedValue = DataBinder.Eval(e.Row.DataItem, "Valid")
                        .ToString()
                        .Substring(0, 2);
                }

                // ValidFormYear
                if (e.Row.FindControl("lbl_ValidFormYear") != null)
                {
                    var lbl_ValidFormYear = (Label) e.Row.FindControl("lbl_ValidFormYear");
                    lbl_ValidFormYear.Text = ServerDateTime.Year.ToString().Substring(0, 2) +
                                             DataBinder.Eval(e.Row.DataItem, "Valid").ToString().Substring(2, 2);
                }

                // ValidFormYear
                if (e.Row.FindControl("ddl_ValidFormYear") != null)
                {
                    var ddl_ValidFormYear = (DropDownList) e.Row.FindControl("ddl_ValidFormYear");

                    for (var i = ServerDateTime.Year - 7; i <= ServerDateTime.Year + 4; i++)
                    {
                        ddl_ValidFormYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }

                    ddl_ValidFormYear.DataBind();
                    ddl_ValidFormYear.SelectedValue = ServerDateTime.Year.ToString().Substring(0, 2) +
                                                      DataBinder.Eval(e.Row.DataItem, "Valid")
                                                          .ToString()
                                                          .Substring(2, 2);
                }

                // ExpiredMonth
                if (e.Row.FindControl("lbl_ExpiredMonth") != null)
                {
                    var lbl_ExpiredMonth = (Label) e.Row.FindControl("lbl_ExpiredMonth");
                    lbl_ExpiredMonth.Text = DataBinder.Eval(e.Row.DataItem, "Expire").ToString().Substring(0, 2);
                }

                // Expire
                if (e.Row.FindControl("ddl_ExpiredMonth") != null)
                {
                    var ddl_ExpiredMonth = (DropDownList) e.Row.FindControl("ddl_ExpiredMonth");

                    for (var i = 1; i <= 12; i++)
                    {
                        if (i < 10)
                        {
                            var j = "0" + i;
                            ddl_ExpiredMonth.Items.Add(new ListItem(j, j));
                        }
                        else
                        {
                            ddl_ExpiredMonth.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        }
                    }

                    ddl_ExpiredMonth.DataBind();
                    ddl_ExpiredMonth.SelectedValue = DataBinder.Eval(e.Row.DataItem, "Expire")
                        .ToString()
                        .Substring(0, 2);
                }

                // ExpiredYear
                if (e.Row.FindControl("lbl_ExpiredYear") != null)
                {
                    var lbl_ExpiredYear = (Label) e.Row.FindControl("lbl_ExpiredYear");
                    lbl_ExpiredYear.Text = ServerDateTime.Year.ToString().Substring(0, 2) +
                                           DataBinder.Eval(e.Row.DataItem, "Expire").ToString().Substring(2, 2);
                }

                // ExpiredYear
                if (e.Row.FindControl("ddl_ExpiredYear") != null)
                {
                    var ddl_ExpiredYear = (DropDownList) e.Row.FindControl("ddl_ExpiredYear");

                    for (var i = ServerDateTime.Year - 7; i <= ServerDateTime.Year + 4; i++)
                    {
                        ddl_ExpiredYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }

                    ddl_ExpiredYear.DataBind();
                    ddl_ExpiredYear.SelectedValue = ServerDateTime.Year.ToString().Substring(0, 2) +
                                                    DataBinder.Eval(e.Row.DataItem, "Expire").ToString().Substring(2, 2);
                }

                // Paymentaccount
                if (e.Row.FindControl("lbl_PaymentCreditAccount") != null)
                {
                    var lbl_PaymentCreditAccount = (Label) e.Row.FindControl("lbl_PaymentCreditAccount");
                    lbl_PaymentCreditAccount.Text = DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString();
                }

                // PaymentaccountName
                if (e.Row.FindControl("lbl_PaymentCreditAccount_Name") != null)
                {
                    var lbl_PaymentCreditAccount_Name = (Label) e.Row.FindControl("lbl_PaymentCreditAccount_Name");
                    lbl_PaymentCreditAccount_Name.Text =
                        account.GetName(DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString(), LoginInfo.ConnStr);
                }

                // AccountCode
                if (e.Row.FindControl("ddl_PaymentCreditAccount_Name") != null)
                {
                    var ddl_PaymentCreditAccount_Name =
                        (DropDownList) e.Row.FindControl("ddl_PaymentCreditAccount_Name");
                    ddl_PaymentCreditAccount_Name.DataSource = account.GetAccountListLookup(LoginInfo.ConnStr);
                    ddl_PaymentCreditAccount_Name.DataTextField = "NameEng";
                    ddl_PaymentCreditAccount_Name.DataValueField = "AccountCode";
                    ddl_PaymentCreditAccount_Name.DataBind();
                    ddl_PaymentCreditAccount_Name.SelectedValue =
                        DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString();
                }
            }
        }

        /// <summary>
        ///     paymentDefaultCredit row canceling event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentCredit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Find grdivew 
            var grd_PaymentCredit = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentCredit");

            // Setup primary key(s)
            var profileCode = grd_PaymentCredit.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_PaymentCredit.DataKeys[e.RowIndex].Values[1]);

            // Delete new row
            for (var i = DataSource.Tables[paymentDefaultCredit.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = DataSource.Tables[paymentDefaultCredit.TableName].Rows[i];

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
            grd_PaymentCredit.DataSource = DataSource.Tables[paymentDefaultCredit.TableName];
            grd_PaymentCredit.EditIndex = -1;
            grd_PaymentCredit.DataBind();
        }

        /// <summary>
        ///     paymentDefaultCredit rowupdating event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentCredit_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Find grdivew 
            var grd_PaymentCredit = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentCredit");


            // Setup primary key(s)
            var profileCode = grd_PaymentCredit.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_PaymentCredit.DataKeys[e.RowIndex].Values[1]);


            foreach (DataRow drUpdating in DataSource.Tables[paymentDefaultCredit.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["ProfileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                        Convert.ToInt32(drUpdating["SeqNo"]) == seqNo)
                    {
                        // Find controls inside of gridview.
                        var txt_Card = (TextBox) grd_PaymentCredit.Rows[e.RowIndex].FindControl("txt_Card");
                        var ddl_Type = (DropDownList) grd_PaymentCredit.Rows[e.RowIndex].FindControl("ddl_Type");
                        var txt_CardHolder = (TextBox) grd_PaymentCredit.Rows[e.RowIndex].FindControl("txt_CardHolder");
                        var ddl_CreditBankName =
                            (DropDownList) grd_PaymentCredit.Rows[e.RowIndex].FindControl("ddl_CreditBankName");
                        var ddl_ValidFormMonth =
                            (DropDownList) grd_PaymentCredit.Rows[e.RowIndex].FindControl("ddl_ValidFormMonth");
                        var ddl_ValidFormYear =
                            (DropDownList) grd_PaymentCredit.Rows[e.RowIndex].FindControl("ddl_ValidFormYear");
                        var ddl_ExpiredMonth =
                            (DropDownList) grd_PaymentCredit.Rows[e.RowIndex].FindControl("ddl_ExpiredMonth");
                        var ddl_ExpiredYear =
                            (DropDownList) grd_PaymentCredit.Rows[e.RowIndex].FindControl("ddl_ExpiredYear");
                        var ddl_PaymentCreditAccount_Name =
                            (DropDownList)
                                grd_PaymentCredit.Rows[e.RowIndex].FindControl("ddl_PaymentCreditAccount_Name");


                        // Updating for assign data.
                        // CardNo
                        drUpdating["CardNo"] = txt_Card.Text;

                        // Type
                        drUpdating["Type"] = ddl_Type.SelectedItem.Value;

                        // CardHolder
                        drUpdating["CardHolder"] = txt_CardHolder.Text.Trim();

                        // BankCode
                        drUpdating["BankCode"] = ddl_CreditBankName.SelectedItem.Value;


                        // Valid
                        drUpdating["Valid"] = (ddl_ValidFormMonth.SelectedItem.Value +
                                               ddl_ValidFormYear.SelectedItem.Value.Substring(2, 2));

                        // Expire
                        drUpdating["Expire"] = (ddl_ExpiredMonth.SelectedItem.Value +
                                                ddl_ValidFormYear.SelectedItem.Value.Substring(2, 2));

                        // Expire
                        drUpdating["AccountCode"] = (ddl_PaymentCreditAccount_Name.SelectedItem.Value);


                        // Refresh data in GridView
                        grd_PaymentCredit.EditIndex = -1;
                        grd_PaymentCredit.DataSource = DataSource.Tables[paymentDefaultCredit.TableName];
                        grd_PaymentCredit.DataBind();
                    }
                }
            }
        }


        /// <summary>
        ///     paymentDefaultCredit row editing event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentCredit_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Find grdivew 
            var grd_PaymentCredit = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentCredit");

            // Determine GridRow
            var grdGridRow = (GridViewRow) grd_PaymentCredit.NamingContainer;

            // Binding gridview by depend on each of the selected row.
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCredit")).EditIndex =
                e.NewEditIndex;
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCredit")).DataSource =
                DataSource.Tables[paymentDefaultCredit.TableName];
            grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCredit").DataBind();
        }

        /// <summary>
        ///     paymentDefaultCredit row deleting event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentCredit_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Find grdivew 
            var grd_PaymentCredit = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentCredit");


            // Setup primary key(s)
            var profileCode = grd_PaymentCredit.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_PaymentCredit.DataKeys[e.RowIndex].Values[1]);

            // Delete the record 
            for (var i = DataSource.Tables[paymentDefaultCredit.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drDelete = DataSource.Tables[paymentDefaultCredit.TableName].Rows[i];

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
            grd_PaymentCredit.DataSource = DataSource.Tables[paymentDefaultCredit.TableName];
            grd_PaymentCredit.DataBind();
        }


        /// <summary>
        ///     PaymentCheque row databound event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentCheque_RowDataBound(object sender, GridViewRowEventArgs e)
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

                // BankCode
                if (e.Row.FindControl("ddl_BankNameCheque") != null)
                {
                    var ddl_BankNameCheque = (DropDownList) e.Row.FindControl("ddl_BankNameCheque");
                    ddl_BankNameCheque.DataSource = bank.GetBankList(LoginInfo.ConnStr);
                    ddl_BankNameCheque.DataTextField = "Name";
                    ddl_BankNameCheque.DataValueField = "BankCode";
                    ddl_BankNameCheque.DataBind();
                    ddl_BankNameCheque.SelectedValue = DataBinder.Eval(e.Row.DataItem, "BankCode").ToString();
                }

                // BankCode
                if (e.Row.FindControl("lbl_BankCodeCheque") != null)
                {
                    var lbl_BankCodeCheque = (Label) e.Row.FindControl("lbl_BankCodeCheque");
                    lbl_BankCodeCheque.Text = DataBinder.Eval(e.Row.DataItem, "BankCode").ToString();
                }

                // BankName
                if (e.Row.FindControl("lbl_BankNameCheque") != null)
                {
                    var lbl_BankNameCheque = (Label) e.Row.FindControl("lbl_BankNameCheque");
                    lbl_BankNameCheque.Text = bank.GetBankName(DataBinder.Eval(e.Row.DataItem, "BankCode").ToString(),
                        LoginInfo.ConnStr);
                }


                // Paymentaccount
                if (e.Row.FindControl("lbl_PaymentAccountCheque") != null)
                {
                    var lbl_PaymentAccountCheque = (Label) e.Row.FindControl("lbl_PaymentAccountCheque");
                    lbl_PaymentAccountCheque.Text = DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString();
                }


                // Paymentaccountname
                if (e.Row.FindControl("lbl_PaymentAccount_NameCheque") != null)
                {
                    var lbl_PaymentAccount_NameCheque = (Label) e.Row.FindControl("lbl_PaymentAccount_NameCheque");
                    lbl_PaymentAccount_NameCheque.Text =
                        account.GetName(DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString(), LoginInfo.ConnStr);
                }

                // Paymentaccountname
                if (e.Row.FindControl("ddl_PaymentAccount_NameCheque") != null)
                {
                    var ddl_PaymentAccount_NameCheque =
                        (DropDownList) e.Row.FindControl("ddl_PaymentAccount_NameCheque");
                    ddl_PaymentAccount_NameCheque.DataSource = account.GetAccountListLookup(LoginInfo.ConnStr);
                    ddl_PaymentAccount_NameCheque.DataTextField = "NameEng";
                    ddl_PaymentAccount_NameCheque.DataValueField = "AccountCode";
                    ddl_PaymentAccount_NameCheque.DataBind();
                    ddl_PaymentAccount_NameCheque.SelectedValue =
                        DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString();
                }
            }
        }


        /// <summary>
        ///     paymentDefaultCheq row canceling event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentCheque_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            var grd_PaymentCheque = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentCheque");


            // Setup primary key(s)
            var profileCode = grd_PaymentCheque.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_PaymentCheque.DataKeys[e.RowIndex].Values[1]);

            // Delete new row
            for (var i = DataSource.Tables[paymentDefaultCheq.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = DataSource.Tables[paymentDefaultCheq.TableName].Rows[i];

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
            grd_PaymentCheque.DataSource = DataSource.Tables[paymentDefaultCheq.TableName];
            grd_PaymentCheque.EditIndex = -1;
            grd_PaymentCheque.DataBind();
        }

        /// <summary>
        ///     paymentDefaultCheq rowupdating event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentCheque_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var grd_PaymentCheque = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentCheque");


            // Setup primary key(s)
            var profileCode = grd_PaymentCheque.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_PaymentCheque.DataKeys[e.RowIndex].Values[1]);


            foreach (DataRow drUpdating in DataSource.Tables[paymentDefaultCheq.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["ProfileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                        Convert.ToInt32(drUpdating["SeqNo"]) == seqNo)
                    {
                        // Find controls inside of gridview.
                        var ddl_PaymentAccount_NameCheque =
                            (DropDownList)
                                grd_PaymentCheque.Rows[e.RowIndex].FindControl("ddl_PaymentAccount_NameCheque");
                        var ddl_BankNameCheque =
                            (DropDownList) grd_PaymentCheque.Rows[e.RowIndex].FindControl("ddl_BankNameCheque");


                        // Updating for assign data.
                        // SeqNo
                        drUpdating["BankCode"] = ddl_BankNameCheque.SelectedItem.Value;

                        // TransTypeCode
                        drUpdating["AccountCode"] = ddl_PaymentAccount_NameCheque.SelectedItem.Value;


                        // Refresh data in GridView
                        grd_PaymentCheque.EditIndex = -1;
                        grd_PaymentCheque.DataSource = DataSource.Tables[paymentDefaultCheq.TableName];
                        grd_PaymentCheque.DataBind();
                    }
                }
            }
        }


        /// <summary>
        ///     paymentDefaultCheq row editing event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentCheque_RowEditing(object sender, GridViewEditEventArgs e)
        {
            var grd_PaymentCheque = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentCheque");

            // Determine GridRow
            var grdGridRow = (GridViewRow) grd_PaymentCheque.NamingContainer;

            // Binding gridview by depend on each of the selected row.
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCheque")).EditIndex =
                e.NewEditIndex;
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCheque")).DataSource =
                DataSource.Tables[paymentDefaultCheq.TableName];
            grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentCheque").DataBind();
        }

        /// <summary>
        ///     paymentDefaultCheq row deleting event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentCheque_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var grd_PaymentCheque = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentCheque");

            // Setup primary key(s)
            var profileCode = grd_PaymentCheque.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_PaymentCheque.DataKeys[e.RowIndex].Values[1]);

            // Delete the record 
            for (var i = DataSource.Tables[paymentDefaultCheq.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drDelete = DataSource.Tables[paymentDefaultCheq.TableName].Rows[i];

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
            grd_PaymentCheque.DataSource = DataSource.Tables[paymentDefaultCheq.TableName];
            grd_PaymentCheque.DataBind();
        }


        /// <summary>
        ///     Paymenttransfer rowdatabound event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentTransfer_RowDataBound(object sender, GridViewRowEventArgs e)
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

                // Bank
                if (e.Row.FindControl("lbl_TransBank") != null)
                {
                    var lbl_TransBank = (Label) e.Row.FindControl("lbl_TransBank");
                    lbl_TransBank.Text = DataBinder.Eval(e.Row.DataItem, "BankCode").ToString();
                }


                // BankName
                if (e.Row.FindControl("ddl_TransBankName") != null)
                {
                    var ddl_TransBankName = (DropDownList) e.Row.FindControl("ddl_TransBankName");
                    ddl_TransBankName.DataSource = bank.GetBankList(LoginInfo.ConnStr);
                    ddl_TransBankName.DataTextField = "Name";
                    ddl_TransBankName.DataValueField = "BankCode";
                    ddl_TransBankName.DataBind();
                    ddl_TransBankName.SelectedValue = DataBinder.Eval(e.Row.DataItem, "BankCode").ToString();
                }

                // BankName
                if (e.Row.FindControl("lbl_TransBankName") != null)
                {
                    var lbl_TransBankName = (Label) e.Row.FindControl("lbl_TransBankName");
                    lbl_TransBankName.Text = bank.GetBankName(DataBinder.Eval(e.Row.DataItem, "BankCode").ToString(),
                        LoginInfo.ConnStr);
                }


                // Branch
                if (e.Row.FindControl("ddl_TransBranchName") != null)
                {
                    var ddl_TransBranchName = (DropDownList) e.Row.FindControl("ddl_TransBranchName");
                    ddl_TransBranchName.DataSource = bank.GetBankList(LoginInfo.ConnStr);
                    ddl_TransBranchName.DataTextField = "BranchName";
                    ddl_TransBranchName.DataValueField = "BranchCode";
                    ddl_TransBranchName.DataBind();
                    ddl_TransBranchName.SelectedValue = DataBinder.Eval(e.Row.DataItem, "BranchCode").ToString();
                }


                // Branch
                if (e.Row.FindControl("lbl_TransBranch") != null)
                {
                    var lbl_TransBranch = (Label) e.Row.FindControl("lbl_TransBranch");
                    lbl_TransBranch.Text = DataBinder.Eval(e.Row.DataItem, "BranchCode").ToString();
                }


                // BranchName
                if (e.Row.FindControl("lbl_TransBranchName") != null)
                {
                    var lbl_TransBranchName = (Label) e.Row.FindControl("lbl_TransBranchName");
                    lbl_TransBranchName.Text =
                        bank.GetBranchName(DataBinder.Eval(e.Row.DataItem, "BranchCode").ToString(), LoginInfo.ConnStr);
                }


                // AccountNo
                if (e.Row.FindControl("txt_AccountNo") != null)
                {
                    var txt_AccountNo = (TextBox) e.Row.FindControl("txt_AccountNo");
                    txt_AccountNo.Text = DataBinder.Eval(e.Row.DataItem, "AccountNo").ToString();
                }

                // AccountNo
                if (e.Row.FindControl("lbl_AccountNo") != null)
                {
                    var lbl_AccountNo = (Label) e.Row.FindControl("lbl_AccountNo");
                    lbl_AccountNo.Text = DataBinder.Eval(e.Row.DataItem, "AccountNo").ToString();
                }

                // Account Name
                if (e.Row.FindControl("txt_AccountName") != null)
                {
                    var txt_AccountName = (TextBox) e.Row.FindControl("txt_AccountName");
                    txt_AccountName.Text = DataBinder.Eval(e.Row.DataItem, "AccountName").ToString();
                }

                // Account Name
                if (e.Row.FindControl("lbl_AccountName") != null)
                {
                    var lbl_AccountName = (Label) e.Row.FindControl("lbl_AccountName");
                    lbl_AccountName.Text = DataBinder.Eval(e.Row.DataItem, "AccountName").ToString();
                }


                // Account Type
                if (e.Row.FindControl("ddl_AccountType") != null)
                {
                    var ddl_AccountType = (DropDownList) e.Row.FindControl("ddl_AccountType");

                    ddl_AccountType.DataSource = bankAccountType.GetBankAccountTypeList(LoginInfo.ConnStr);
                    ddl_AccountType.DataTextField = "Name";
                    ddl_AccountType.DataValueField = "BankAccountTypeCode";
                    ddl_AccountType.DataBind();
                    ddl_AccountType.SelectedValue = DataBinder.Eval(e.Row.DataItem, "AccountTypeCode").ToString();
                }

                // Account Type
                if (e.Row.FindControl("lbl_AccountType") != null)
                {
                    var lbl_AccountType = (Label) e.Row.FindControl("lbl_AccountType");
                    lbl_AccountType.Text =
                        bankAccountType.GetBankAccountTypeName(
                            DataBinder.Eval(e.Row.DataItem, "AccountTypeCode").ToString(), LoginInfo.ConnStr);
                }


                // Paymentaccount
                if (e.Row.FindControl("lbl_TransPaymentAccount") != null)
                {
                    var lbl_TransPaymentAccount = (Label) e.Row.FindControl("lbl_TransPaymentAccount");
                    lbl_TransPaymentAccount.Text = DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString();
                }

                // PaymentaccountName
                if (e.Row.FindControl("ddl_TransPaymentAccount_Name") != null)
                {
                    var ddl_TransPaymentAccount_Name = (DropDownList) e.Row.FindControl("ddl_TransPaymentAccount_Name");
                    ddl_TransPaymentAccount_Name.DataSource = account.GetAccountListLookup(LoginInfo.ConnStr);
                    ddl_TransPaymentAccount_Name.DataTextField = "NameEng";
                    ddl_TransPaymentAccount_Name.DataValueField = "AccountCode";
                    ddl_TransPaymentAccount_Name.DataBind();
                    ddl_TransPaymentAccount_Name.SelectedValue =
                        DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString();
                }

                // PaymentaccountName
                if (e.Row.FindControl("lbl_TransPaymentAccount_Name") != null)
                {
                    var lbl_TransPaymentAccount_Name = (Label) e.Row.FindControl("lbl_TransPaymentAccount_Name");
                    lbl_TransPaymentAccount_Name.Text =
                        account.GetName(DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString(), LoginInfo.ConnStr);
                }
            }
        }


        /// <summary>
        ///     paymentDefaultTrans row canceling event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentTransfer_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Find grdivew 
            var grd_PaymentTransfer = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentTransfer");


            // Setup primary key(s)
            var profileCode = grd_PaymentTransfer.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_PaymentTransfer.DataKeys[e.RowIndex].Values[1]);

            // Delete new row
            for (var i = DataSource.Tables[paymentDefaultTrans.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = DataSource.Tables[paymentDefaultTrans.TableName].Rows[i];

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
            grd_PaymentTransfer.DataSource = DataSource.Tables[paymentDefaultTrans.TableName];
            grd_PaymentTransfer.EditIndex = -1;
            grd_PaymentTransfer.DataBind();
        }

        /// <summary>
        ///     paymentDefaultTrans rowupdating event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentTransfer_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Find grdivew 
            var grd_PaymentTransfer = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentTransfer");


            // Setup primary key(s)
            var profileCode = grd_PaymentTransfer.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_PaymentTransfer.DataKeys[e.RowIndex].Values[1]);


            foreach (DataRow drUpdating in DataSource.Tables[paymentDefaultTrans.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["ProfileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                        Convert.ToInt32(drUpdating["SeqNo"]) == seqNo)
                    {
                        // Find controls inside of gridview.
                        var ddl_TransBankName =
                            (DropDownList) grd_PaymentTransfer.Rows[e.RowIndex].FindControl("ddl_TransBankName");
                        var ddl_TransBranchName =
                            (DropDownList) grd_PaymentTransfer.Rows[e.RowIndex].FindControl("ddl_TransBranchName");

                        var txt_AccountNo = (TextBox) grd_PaymentTransfer.Rows[e.RowIndex].FindControl("txt_AccountNo");
                        var txt_AccountName =
                            (TextBox) grd_PaymentTransfer.Rows[e.RowIndex].FindControl("txt_AccountName");
                        var ddl_AccountType =
                            (DropDownList) grd_PaymentTransfer.Rows[e.RowIndex].FindControl("ddl_AccountType");
                        var ddl_TransPaymentAccount_Name =
                            (DropDownList)
                                grd_PaymentTransfer.Rows[e.RowIndex].FindControl("ddl_TransPaymentAccount_Name");


                        // Updating for assign data.
                        // BankCode
                        drUpdating["BankCode"] = ddl_TransBankName.SelectedItem.Value;

                        // BranchCode
                        drUpdating["BranchCode"] = ddl_TransBranchName.SelectedItem.Value;

                        // AccountNo
                        drUpdating["AccountNo"] = txt_AccountNo.Text.Trim();

                        // AccountName
                        drUpdating["AccountName"] = txt_AccountName.Text.Trim();


                        // AccountTypeCode
                        drUpdating["AccountTypeCode"] = ddl_AccountType.SelectedItem.Value;

                        // AccountCode
                        drUpdating["AccountCode"] = ddl_TransPaymentAccount_Name.SelectedItem.Value;


                        // Refresh data in GridView
                        grd_PaymentTransfer.EditIndex = -1;
                        grd_PaymentTransfer.DataSource = DataSource.Tables[paymentDefaultTrans.TableName];
                        grd_PaymentTransfer.DataBind();
                    }
                }
            }
        }


        /// <summary>
        ///     paymentDefaultTrans row editing event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentTransfer_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Find grdivew 
            var grd_PaymentTransfer = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentTransfer");


            // Determine GridRow
            var grdGridRow = (GridViewRow) grd_PaymentTransfer.NamingContainer;

            // Binding gridview by depend on each of the selected row.
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentTransfer")).EditIndex =
                e.NewEditIndex;
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentTransfer")).DataSource =
                DataSource.Tables[paymentDefaultTrans.TableName];
            grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentTransfer").DataBind();
        }

        /// <summary>
        ///     paymentDefaultTrans row deleting event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentTransfer_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Find grdivew 
            var grd_PaymentTransfer = (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentTransfer");


            // Setup primary key(s)
            var profileCode = grd_PaymentTransfer.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_PaymentTransfer.DataKeys[e.RowIndex].Values[1]);

            // Delete the record 
            for (var i = DataSource.Tables[paymentDefaultTrans.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drDelete = DataSource.Tables[paymentDefaultTrans.TableName].Rows[i];

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
            grd_PaymentTransfer.DataSource = DataSource.Tables[paymentDefaultTrans.TableName];
            grd_PaymentTransfer.DataBind();
        }


        /// <summary>
        ///     PaymentTransfervendor rowdatabound event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentTransferVendor_RowDataBound(object sender, GridViewRowEventArgs e)
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

                // BankName
                if (e.Row.FindControl("ddl_TransVendorBankName") != null)
                {
                    var ddl_TransVendorBankName = (DropDownList) e.Row.FindControl("ddl_TransVendorBankName");
                    ddl_TransVendorBankName.DataSource = bank.GetBankList(LoginInfo.ConnStr);
                    ddl_TransVendorBankName.DataTextField = "Name";
                    ddl_TransVendorBankName.DataTextField = "BankCode";
                    ddl_TransVendorBankName.DataBind();
                    ddl_TransVendorBankName.SelectedValue = DataBinder.Eval(e.Row.DataItem, "VBankCode").ToString();
                }

                // Bank
                if (e.Row.FindControl("lbl_TransVendorBank") != null)
                {
                    var lbl_TransVendorBank = (Label) e.Row.FindControl("lbl_TransVendorBank");
                    lbl_TransVendorBank.Text = DataBinder.Eval(e.Row.DataItem, "VBankCode").ToString();
                }


                // BankName
                if (e.Row.FindControl("lbl_TransVendorBankName") != null)
                {
                    var lbl_TransVendorBankName = (Label) e.Row.FindControl("lbl_TransVendorBankName");
                    lbl_TransVendorBankName.Text =
                        bank.GetBankName(DataBinder.Eval(e.Row.DataItem, "VBankCode").ToString(), LoginInfo.ConnStr);
                }


                // BankName
                if (e.Row.FindControl("ddl_TransVendorBranchName") != null)
                {
                    var ddl_TransVendorBranchName = (DropDownList) e.Row.FindControl("ddl_TransVendorBranchName");
                    ddl_TransVendorBranchName.DataSource = bank.GetBankList(LoginInfo.ConnStr);
                    ddl_TransVendorBranchName.DataTextField = "BranchName";
                    ddl_TransVendorBranchName.DataTextField = "BranchCode";
                    ddl_TransVendorBranchName.DataBind();
                    ddl_TransVendorBranchName.SelectedValue = DataBinder.Eval(e.Row.DataItem, "VBranchCode").ToString();
                }


                // Branch
                if (e.Row.FindControl("lbl_TransVendorBranch") != null)
                {
                    var lbl_TransVendorBranch = (Label) e.Row.FindControl("lbl_TransVendorBranch");
                    lbl_TransVendorBranch.Text = DataBinder.Eval(e.Row.DataItem, "VBranchCode").ToString();
                }


                // BranchName
                if (e.Row.FindControl("lbl_TransVendorBranchName") != null)
                {
                    var lbl_TransVendorBranchName = (Label) e.Row.FindControl("lbl_TransVendorBranchName");
                    lbl_TransVendorBranchName.Text =
                        bank.GetBranchName(DataBinder.Eval(e.Row.DataItem, "VBranchCode").ToString(), LoginInfo.ConnStr);
                }


                // VAccountNo
                if (e.Row.FindControl("txt_AccountNo") != null)
                {
                    var txt_AccountNo = (TextBox) e.Row.FindControl("txt_AccountNo");
                    txt_AccountNo.Text = DataBinder.Eval(e.Row.DataItem, "VAccountNo").ToString();
                }

                // VAccountNo
                if (e.Row.FindControl("lbl_AccountNo") != null)
                {
                    var lbl_AccountNo = (Label) e.Row.FindControl("lbl_AccountNo");
                    lbl_AccountNo.Text = DataBinder.Eval(e.Row.DataItem, "VAccountNo").ToString();
                }


                // VAccount Name
                if (e.Row.FindControl("txt_AccountName") != null)
                {
                    var txt_AccountName = (TextBox) e.Row.FindControl("txt_AccountName");
                    txt_AccountName.Text = DataBinder.Eval(e.Row.DataItem, "VAccountName").ToString();
                }


                // VAccount Name
                if (e.Row.FindControl("lbl_AccountName") != null)
                {
                    var lbl_AccountName = (Label) e.Row.FindControl("lbl_AccountName");
                    lbl_AccountName.Text = DataBinder.Eval(e.Row.DataItem, "VAccountName").ToString();
                }


                // VAccount Type
                if (e.Row.FindControl("ddl_AccountType") != null)
                {
                    var ddl_AccountType = (DropDownList) e.Row.FindControl("ddl_AccountType");

                    ddl_AccountType.DataSource = bankAccountType.GetBankAccountTypeList(LoginInfo.ConnStr);
                    ddl_AccountType.DataTextField = "Name";
                    ddl_AccountType.DataValueField = "BankAccountTypeCode";
                    ddl_AccountType.DataBind();
                    ddl_AccountType.SelectedValue = DataBinder.Eval(e.Row.DataItem, "VAccountTypeCode").ToString();
                }

                // VAccount Name
                if (e.Row.FindControl("lbl_AccountType") != null)
                {
                    var lbl_AccountType = (Label) e.Row.FindControl("lbl_AccountType");
                    lbl_AccountType.Text =
                        bankAccountType.GetBankAccountTypeName(
                            DataBinder.Eval(e.Row.DataItem, "VAccountTypeCode").ToString(), LoginInfo.ConnStr);
                }
            }
        }


        /// <summary>
        ///     paymentDefaultTransVendor row canceling event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentTransferVendor_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            var grd_PaymentTransferVendor =
                (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentTransferVendor");


            // Setup primary key(s)
            var profileCode = grd_PaymentTransferVendor.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_PaymentTransferVendor.DataKeys[e.RowIndex].Values[1]);

            // Delete new row
            for (var i = DataSource.Tables[paymentDefaultTrans.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = DataSource.Tables[paymentDefaultTrans.TableName].Rows[i];

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
            grd_PaymentTransferVendor.DataSource = DataSource.Tables[paymentDefaultTrans.TableName];
            grd_PaymentTransferVendor.EditIndex = -1;
            grd_PaymentTransferVendor.DataBind();
        }

        /// <summary>
        ///     paymentDefaultTransVendor rowupdating event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentTransferVendor_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var grd_PaymentTransferVendor =
                (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentTransferVendor");


            // Setup primary key(s)
            var profileCode = grd_PaymentTransferVendor.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_PaymentTransferVendor.DataKeys[e.RowIndex].Values[1]);


            foreach (DataRow drUpdating in DataSource.Tables[paymentDefaultTrans.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["ProfileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                        Convert.ToInt32(drUpdating["SeqNo"]) == seqNo)
                    {
                        // Find controls inside of gridview.
                        var ddl_TransVendorBankName =
                            (DropDownList)
                                grd_PaymentTransferVendor.Rows[e.RowIndex].FindControl("ddl_TransVendorBankName");
                        var ddl_TransVendorBranchName =
                            (DropDownList)
                                grd_PaymentTransferVendor.Rows[e.RowIndex].FindControl("ddl_TransVendorBranchName");
                        var txt_AccountNo =
                            (TextBox) grd_PaymentTransferVendor.Rows[e.RowIndex].FindControl("txt_AccountNo");
                        var txt_AccountName =
                            (TextBox) grd_PaymentTransferVendor.Rows[e.RowIndex].FindControl("txt_AccountName");
                        var ddl_AccountType =
                            (DropDownList) grd_PaymentTransferVendor.Rows[e.RowIndex].FindControl("ddl_AccountType");


                        // Updating for assign data.
                        // VBankCode
                        drUpdating["VBankCode"] = ddl_TransVendorBankName.SelectedItem.Value;

                        // VBranchCode
                        drUpdating["VBranchCode"] = ddl_TransVendorBranchName.SelectedItem.Value;

                        // VAccountNo
                        drUpdating["VAccountNo"] = txt_AccountNo.Text.Trim();

                        // VAccountName
                        drUpdating["VAccountName"] = txt_AccountName.Text.Trim();

                        // VAccountTypeCode
                        drUpdating["VAccountTypeCode"] = ddl_AccountType.SelectedItem.Value;


                        // Refresh data in GridView
                        grd_PaymentTransferVendor.EditIndex = -1;
                        grd_PaymentTransferVendor.DataSource = DataSource.Tables[paymentDefaultTrans.TableName];
                        grd_PaymentTransferVendor.DataBind();
                    }
                }
            }
        }


        /// <summary>
        ///     paymentDefaultTransVendor row editing event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentTransferVendor_RowEditing(object sender, GridViewEditEventArgs e)
        {
            var grd_PaymentTransferVendor =
                (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentTransferVendor");

            // Determine GridRow
            var grdGridRow = (GridViewRow) grd_PaymentTransferVendor.NamingContainer;

            // Binding gridview by depend on each of the selected row.
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentTransferVendor")).EditIndex =
                e.NewEditIndex;
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentTransferVendor")).DataSource =
                DataSource.Tables[paymentDefaultTrans.TableName];
            grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentTransferVendor").DataBind();
        }

        /// <summary>
        ///     paymentDefaultTransVendor row deleting event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentTransferVendor_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var grd_PaymentTransferVendor =
                (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentTransferVendor");


            // Setup primary key(s)
            var profileCode = grd_PaymentTransferVendor.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_PaymentTransferVendor.DataKeys[e.RowIndex].Values[1]);

            // Delete the record 
            for (var i = DataSource.Tables[paymentDefaultTrans.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drDelete = DataSource.Tables[paymentDefaultTrans.TableName].Rows[i];

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
            grd_PaymentTransferVendor.DataSource = DataSource.Tables[paymentDefaultTrans.TableName];
            grd_PaymentTransferVendor.DataBind();
        }


        /// <summary>
        ///     PaymentWithholding rowdatabound event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentWithholding_RowDataBound(object sender, GridViewRowEventArgs e)
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

                // Item
                if (e.Row.FindControl("lbl_Item") != null)
                {
                    var lbl_Item = (Label) e.Row.FindControl("lbl_Item");
                    lbl_Item.Text = DataBinder.Eval(e.Row.DataItem, "SeqNo").ToString();
                }

                // Item
                if (e.Row.FindControl("txt_Item") != null)
                {
                    var txt_Item = (TextBox) e.Row.FindControl("txt_Item");
                    txt_Item.Text = DataBinder.Eval(e.Row.DataItem, "SeqNo").ToString();
                }

                // Type
                if (e.Row.FindControl("lbl_Type") != null)
                {
                    var lbl_Type = (Label) e.Row.FindControl("lbl_Type");
                    lbl_Type.Text = whtType.GetWhtTypeName(
                        DataBinder.Eval(e.Row.DataItem, "WHTTaxTypeCode").ToString(), LoginInfo.ConnStr);
                }

                // Type
                if (e.Row.FindControl("ddl_Type") != null)
                {
                    var ddl_Type = (DropDownList) e.Row.FindControl("ddl_Type");

                    ddl_Type.DataSource = whtType.GetWhtTypeList(LoginInfo.ConnStr);
                    ddl_Type.DataTextField = "Name";
                    ddl_Type.DataValueField = "WHTTypeCode";
                    ddl_Type.DataBind();
                    ddl_Type.SelectedValue = DataBinder.Eval(e.Row.DataItem, "WHTTaxTypeCode").ToString();
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


                // Account
                if (e.Row.FindControl("lbl_Account") != null)
                {
                    var lbl_Account = (Label) e.Row.FindControl("lbl_Account");
                    lbl_Account.Text = DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString();
                }


                // WithAccount
                if (e.Row.FindControl("ddl_WithAccount") != null)
                {
                    var ddl_WithAccount = (DropDownList) e.Row.FindControl("ddl_WithAccount");

                    ddl_WithAccount.DataSource = account.GetAccountListLookup(LoginInfo.ConnStr);
                    ddl_WithAccount.DataTextField = "NameEng";
                    ddl_WithAccount.DataValueField = "AccountCode";
                    ddl_WithAccount.DataBind();
                    ddl_WithAccount.SelectedValue = DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString();
                }

                // AccountName
                if (e.Row.FindControl("lbl_AccountName") != null)
                {
                    var lbl_AccountName = (Label) e.Row.FindControl("lbl_AccountName");
                    lbl_AccountName.Text = account.GetName(DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString(),
                        LoginInfo.ConnStr);
                }
            }
        }

        /// <summary>
        ///     PaymentWithholding rowdeleting event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentWithholding_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var grd_PaymentWithholding =
                (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentWithholding");

            // Setup primary key(s)
            var profileCode = grd_PaymentWithholding.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_PaymentWithholding.DataKeys[e.RowIndex].Values[1]);


            // Delete the record 
            for (var i = DataSource.Tables[vendorDefaultWHT.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drDelete = DataSource.Tables[vendorDefaultWHT.TableName].Rows[i];

                if (drDelete.RowState != DataRowState.Deleted)
                {
                    if (drDelete["profileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                        Convert.ToInt32(drDelete["seqNo"]) == seqNo)
                    {
                        drDelete.Delete();
                    }
                }
            }

            // Binding grid
            grd_PaymentWithholding.DataSource = DataSource.Tables[vendorDefaultWHT.TableName];
            grd_PaymentWithholding.DataBind();
        }


        /// <summary>
        ///     PaymentWithholding rowcancelingedit event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentWithholding_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            var grd_PaymentWithholding =
                (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentWithholding");

            // Setup primary key(s)
            var profileCode = grd_PaymentWithholding.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_PaymentWithholding.DataKeys[e.RowIndex].Values[1]);

            // Delete new row
            for (var i = DataSource.Tables[vendorDefaultWHT.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var dr = DataSource.Tables[vendorDefaultWHT.TableName].Rows[i];

                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["profileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                        Convert.ToInt32(dr["seqNo"]) == seqNo &&
                        dr.RowState == DataRowState.Added)
                    {
                        dr.Delete();
                        break;
                    }
                }
            }

            // Refresh data in gridview
            grd_PaymentWithholding.DataSource = DataSource.Tables[vendorDefaultWHT.TableName];
            grd_PaymentWithholding.EditIndex = -1;
            grd_PaymentWithholding.DataBind();
        }

        /// <summary>
        ///     PaymentWithholding rowupdating event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentWithholding_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var grd_PaymentWithholding =
                (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentWithholding");


            // Setup primary key(s)
            var profileCode = grd_PaymentWithholding.DataKeys[e.RowIndex].Values[0].ToString();
            var seqNo = Convert.ToInt32(grd_PaymentWithholding.DataKeys[e.RowIndex].Values[1]);

            foreach (DataRow drUpdating in DataSource.Tables[vendorDefaultWHT.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["profileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                        Convert.ToInt32(drUpdating["seqNo"]) == seqNo)
                    {
                        // Find controls inside of gridview.

                        var ddl_WithAccount =
                            (DropDownList) grd_PaymentWithholding.Rows[e.RowIndex].FindControl("ddl_WithAccount");
                        var txt_Rate = (TextBox) grd_PaymentWithholding.Rows[e.RowIndex].FindControl("txt_Rate");
                        var ddl_Type = (DropDownList) grd_PaymentWithholding.Rows[e.RowIndex].FindControl("ddl_Type");


                        // Updating for assign data.
                        // WHTTaxTypeCode
                        drUpdating["WHTTaxTypeCode"] = ddl_Type.SelectedItem.Value;

                        // Rate
                        drUpdating["Rate"] = txt_Rate.Text.Trim();

                        // BankAccountName
                        drUpdating["AccountCode"] = ddl_WithAccount.SelectedItem.Value;


                        // Refresh data in GridView
                        grd_PaymentWithholding.EditIndex = -1;
                        grd_PaymentWithholding.DataSource = DataSource.Tables[vendorDefaultWHT.TableName];
                        grd_PaymentWithholding.DataBind();
                    }
                }
            }
        }

        /// <summary>
        ///     PaymentWithholding rowediting event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PaymentWithholding_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Find grdivew 
            var grd_PaymentWithholding =
                (GridView) ((GridView) sender).Parent.Parent.FindControl("grd_PaymentWithholding");

            // Determine GridRow
            var grdGridRow = (GridViewRow) grd_PaymentWithholding.NamingContainer;

            // Binding gridview by depend on each of the selected row.
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentWithholding")).EditIndex =
                e.NewEditIndex;
            ((GridView) grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentWithholding")).DataSource =
                DataSource.Tables[vendorDefaultWHT.TableName];
            grd_Payment.Rows[grdGridRow.RowIndex].FindControl("grd_PaymentWithholding").DataBind();
        }


        /// <summary>
        ///     Imagebutton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void img_PaymentDetail_Click(object sender, ImageClickEventArgs e)
        {
            var img_PaymentDetail = (ImageButton) ((ImageButton) sender).Parent.Parent.FindControl("img_PaymentDetail");
            var pnl_PaymentDetail = (Panel) ((ImageButton) sender).Parent.Parent.FindControl("pnl_PaymentDetail");
            var grd_PaymentCash = (GridView) ((ImageButton) sender).Parent.Parent.FindControl("grd_PaymentCash");
            var grd_AutoPayment = (GridView) ((ImageButton) sender).Parent.Parent.FindControl("grd_AutoPayment");
            var grd_PaymentCheque = (GridView) ((ImageButton) sender).Parent.Parent.FindControl("grd_PaymentCheque");
            var grd_PaymentCredit = (GridView) ((ImageButton) sender).Parent.Parent.FindControl("grd_PaymentCredit");
            var grd_PaymentTransfer = (GridView) ((ImageButton) sender).Parent.Parent.FindControl("grd_PaymentTransfer");
            var grd_PaymentTransferVendor =
                (GridView) ((ImageButton) sender).Parent.Parent.FindControl("grd_PaymentTransferVendor");
            var grd_PaymentWithholding =
                (GridView) ((ImageButton) sender).Parent.Parent.FindControl("grd_PaymentWithholding");
            var pnl_PaymentCash = (Panel) ((ImageButton) sender).Parent.Parent.FindControl("pnl_PaymentCash");
            var pnl_PaymentCheque = (Panel) ((ImageButton) sender).Parent.Parent.FindControl("pnl_PaymentCheque");
            var pnl_PaymentCredit = (Panel) ((ImageButton) sender).Parent.Parent.FindControl("pnl_PaymentCredit");
            var pnl_AutoPayment = (Panel) ((ImageButton) sender).Parent.Parent.FindControl("pnl_AutoPayment");
            var pnl_Transfer = (Panel) ((ImageButton) sender).Parent.Parent.FindControl("pnl_Transfer");
            var pnl_Withholding = (Panel) ((ImageButton) sender).Parent.Parent.FindControl("pnl_Withholding");
            var lbl_Seq = (Label) ((ImageButton) sender).Parent.Parent.FindControl("lbl_Seq");
            var hid_PaymentMethod = (HiddenField) ((ImageButton) sender).Parent.Parent.FindControl("hid_PaymentMethod");
            var btn_AutoPayment = (Button) ((ImageButton) sender).Parent.Parent.FindControl("btn_AutoPayment");
            var btn_PaymentCash = (Button) ((ImageButton) sender).Parent.Parent.FindControl("btn_PaymentCash");
            var btn_PaymentCheque = (Button) ((ImageButton) sender).Parent.Parent.FindControl("btn_PaymentCheque");
            var btn_PaymentCredit = (Button) ((ImageButton) sender).Parent.Parent.FindControl("btn_PaymentCredit");
            var btn_Transfer = (Button) ((ImageButton) sender).Parent.Parent.FindControl("btn_Transfer");
            var btn_Withholding = (Button) ((ImageButton) sender).Parent.Parent.FindControl("btn_Withholding");


            GridViewRow grdGridRow;


            // Determine GridRow
            grdGridRow = (GridViewRow) img_PaymentDetail.NamingContainer;


            switch (hid_PaymentMethod.Value.Trim())
            {
                case "CASH":
                    pnl_PaymentCash.Visible = true;
                    pnl_PaymentCheque.Visible = false;
                    pnl_PaymentCredit.Visible = false;
                    pnl_AutoPayment.Visible = false;
                    pnl_Transfer.Visible = false;
                    pnl_Transfer.Visible = false;
                    pnl_Withholding.Visible = false;

                    img_PaymentDetail.ImageUrl = string.Empty;


                    if (pnl_PaymentDetail.Visible)
                    {
                        pnl_PaymentDetail.Visible = false;
                        img_PaymentDetail.ImageUrl = "~/App_Themes/default/pics/show_detail_icon.png";


                        grd_PaymentCash.DataSource = null;
                        grd_PaymentCash.DataBind();
                    }
                    else
                    {
                        pnl_PaymentDetail.Visible = true;
                        img_PaymentDetail.ImageUrl = "~/App_Themes/default/pics/hide_detail_icon.png";

                        // if the readonly is true.not allow to create new process.
                        if (ReadOnly)
                        {
                            // visible for new button.
                            btn_PaymentCash.Visible = false;
                        }

                        if (lbl_Seq.Text != string.Empty)
                        {
                            // Clear table value before binding.
                            DataSource.Tables[paymentDefaultCash.TableName].Clear();


                            paymentDefaultCash.GetPaymentDefaultCashList(ProfileCode.ToString(), DataSource,
                                LoginInfo.ConnStr);
                        }

                        grd_PaymentCash.DataSource = DataSource.Tables[paymentDefaultCash.TableName];
                        grd_PaymentCash.DataBind();
                    }
                    break;

                case "CHEQ":

                    pnl_PaymentCash.Visible = false;
                    pnl_PaymentCheque.Visible = true;
                    pnl_PaymentCredit.Visible = false;
                    pnl_AutoPayment.Visible = false;
                    pnl_Transfer.Visible = false;
                    pnl_Transfer.Visible = false;
                    pnl_Withholding.Visible = false;

                    img_PaymentDetail.ImageUrl = string.Empty;


                    if (pnl_PaymentDetail.Visible)
                    {
                        pnl_PaymentDetail.Visible = false;
                        img_PaymentDetail.ImageUrl = "~/App_Themes/default/pics/show_detail_icon.png";


                        grd_PaymentCheque.DataSource = null;
                        grd_PaymentCheque.DataBind();
                    }
                    else
                    {
                        pnl_PaymentDetail.Visible = true;
                        img_PaymentDetail.ImageUrl = "~/App_Themes/default/pics/hide_detail_icon.png";


                        // if the readonly is true.not allow to create new process.
                        if (ReadOnly)
                        {
                            // visible for new button.
                            btn_PaymentCheque.Visible = false;
                        }

                        if (lbl_Seq.Text != string.Empty)
                        {
                            paymentDefaultCheq.GetPaymentDefaultCheqList(ProfileCode.ToString(), DataSource,
                                LoginInfo.ConnStr);
                        }

                        grd_PaymentCheque.DataSource = DataSource.Tables[paymentDefaultCheq.TableName];
                        grd_PaymentCheque.DataBind();
                    }
                    break;

                case "CARD":

                    pnl_PaymentCash.Visible = false;
                    pnl_PaymentCheque.Visible = false;
                    pnl_PaymentCredit.Visible = true;
                    pnl_AutoPayment.Visible = false;
                    pnl_Transfer.Visible = false;
                    pnl_Transfer.Visible = false;
                    pnl_Withholding.Visible = false;

                    img_PaymentDetail.ImageUrl = string.Empty;


                    if (pnl_PaymentDetail.Visible)
                    {
                        pnl_PaymentDetail.Visible = false;
                        img_PaymentDetail.ImageUrl = "~/App_Themes/default/pics/show_detail_icon.png";

                        grd_PaymentCredit.DataSource = null;
                        grd_PaymentCredit.DataBind();
                    }
                    else
                    {
                        pnl_PaymentDetail.Visible = true;
                        img_PaymentDetail.ImageUrl = "~/App_Themes/default/pics/hide_detail_icon.png";

                        // if the readonly is true.not allow to create new process.
                        if (ReadOnly)
                        {
                            // visible for new button.
                            btn_PaymentCredit.Visible = false;
                        }

                        if (lbl_Seq.Text != string.Empty)
                        {
                            paymentDefaultCredit.GetPaymentDefaultCreditList(ProfileCode.ToString(), DataSource,
                                LoginInfo.ConnStr);
                        }

                        grd_PaymentCredit.DataSource = DataSource.Tables[paymentDefaultCredit.TableName];
                        grd_PaymentCredit.DataBind();
                    }

                    break;

                case "AUTO":

                    pnl_PaymentCash.Visible = false;
                    pnl_PaymentCheque.Visible = false;
                    pnl_PaymentCredit.Visible = false;
                    pnl_AutoPayment.Visible = true;
                    pnl_Transfer.Visible = false;
                    pnl_Transfer.Visible = false;
                    pnl_Withholding.Visible = false;

                    img_PaymentDetail.ImageUrl = string.Empty;


                    if (pnl_PaymentDetail.Visible)
                    {
                        pnl_PaymentDetail.Visible = false;
                        img_PaymentDetail.ImageUrl = "~/App_Themes/default/pics/show_detail_icon.png";


                        grd_AutoPayment.DataSource = null;
                        grd_AutoPayment.DataBind();
                    }
                    else
                    {
                        pnl_PaymentDetail.Visible = true;
                        img_PaymentDetail.ImageUrl = "~/App_Themes/default/pics/hide_detail_icon.png";

                        // if the readonly is true.not allow to create new process.
                        if (ReadOnly)
                        {
                            // visible for new button.
                            btn_AutoPayment.Visible = false;
                        }

                        if (lbl_Seq.Text != string.Empty)
                        {
                            //Clear table value before binding.
                            DataSource.Tables[paymentDefaultAuto.TableName].Clear();

                            paymentDefaultAuto.GetPaymentDefaultAutoList(ProfileCode.ToString(), DataSource,
                                LoginInfo.ConnStr);
                        }

                        grd_AutoPayment.DataSource = DataSource.Tables[paymentDefaultAuto.TableName];
                        grd_AutoPayment.DataBind();
                    }

                    break;

                case "TRAN":

                    pnl_PaymentCash.Visible = false;
                    pnl_PaymentCheque.Visible = false;
                    pnl_PaymentCredit.Visible = false;
                    pnl_AutoPayment.Visible = false;
                    pnl_Transfer.Visible = false;
                    pnl_Transfer.Visible = true;
                    pnl_Withholding.Visible = false;

                    img_PaymentDetail.ImageUrl = string.Empty;


                    if (pnl_PaymentDetail.Visible)
                    {
                        pnl_PaymentDetail.Visible = false;
                        img_PaymentDetail.ImageUrl = "~/App_Themes/default/pics/show_detail_icon.png";


                        grd_PaymentTransfer.DataSource = null;
                        grd_PaymentTransfer.DataBind();

                        grd_PaymentTransferVendor.DataSource = null;
                        grd_PaymentTransferVendor.DataBind();
                    }
                    else
                    {
                        pnl_PaymentDetail.Visible = true;
                        img_PaymentDetail.ImageUrl = "~/App_Themes/default/pics/hide_detail_icon.png";

                        // if the readonly is true.not allow to create new process.
                        if (ReadOnly)
                        {
                            // visible for new button.
                            btn_Transfer.Visible = false;
                        }

                        if (lbl_Seq.Text != string.Empty)
                        {
                            paymentDefaultTrans.GetPaymentDefaultTransList(ProfileCode.ToString(), DataSource,
                                LoginInfo.ConnStr);
                        }

                        // For payment.
                        grd_PaymentTransfer.DataSource = DataSource.Tables[paymentDefaultTrans.TableName];
                        grd_PaymentTransfer.DataBind();

                        // For vendor.
                        grd_PaymentTransferVendor.DataSource = DataSource.Tables[paymentDefaultTrans.TableName];
                        grd_PaymentTransferVendor.DataBind();
                    }

                    break;

                case "WHT":

                    pnl_PaymentCash.Visible = false;
                    pnl_PaymentCheque.Visible = false;
                    pnl_PaymentCredit.Visible = false;
                    pnl_AutoPayment.Visible = false;
                    pnl_Transfer.Visible = false;
                    pnl_Transfer.Visible = false;
                    pnl_Withholding.Visible = true;

                    img_PaymentDetail.ImageUrl = string.Empty;


                    if (pnl_PaymentDetail.Visible)
                    {
                        pnl_PaymentDetail.Visible = false;
                        img_PaymentDetail.ImageUrl = "~/App_Themes/default/pics/show_detail_icon.png";

                        grd_PaymentWithholding.DataSource = null;
                        grd_PaymentWithholding.DataBind();
                    }
                    else
                    {
                        pnl_PaymentDetail.Visible = true;
                        img_PaymentDetail.ImageUrl = "~/App_Themes/default/pics/hide_detail_icon.png";

                        // if the readonly is true.not allow to create new process.
                        if (ReadOnly)
                        {
                            // visible for new button.
                            btn_Withholding.Visible = false;
                        }

                        if (lbl_Seq.Text != string.Empty)
                        {
                            vendorDefaultWHT.GetVendorDefaultWHTList(ProfileCode.ToString(), DataSource,
                                LoginInfo.ConnStr);
                        }

                        // For payment.
                        grd_PaymentWithholding.DataSource = DataSource.Tables[vendorDefaultWHT.TableName];
                        grd_PaymentWithholding.DataBind();
                    }
                    break;
            }
        }


        /// <summary>
        ///     Get data.
        /// </summary>
        private void Page_Retrieve()
        {
        }

        /// <summary>
        ///     Display Paymentdefault data.
        /// </summary>
        private void Page_Setting()
        {
            // if the readonly is true.edit/delte/update/new process would not be allow.
            if (ReadOnly)
            {
                btn_Payment.Visible = false;
            }

            grd_Payment.DataSource = DataSource.Tables[paymentDefault.TableName];
            grd_Payment.DataBind();
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