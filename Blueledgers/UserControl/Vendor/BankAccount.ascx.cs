using System;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BlueLedger.PL.BaseClass;
//using DevExpress.XtraBars;
using System.IO;
using BlueLedger;


namespace BlueLedger.PL.UserControls.AP
{
    public partial class BankAccount : BaseUserControl
    {
        #region "Attributies"

        private Blue.BL.Bank.Bank bank = new Blue.BL.Bank.Bank();
        private Blue.BL.Profile.BankAccount bankAccount = new Blue.BL.Profile.BankAccount();
        private Blue.BL.Bank.BankAccountType bankAccountType = new Blue.BL.Bank.BankAccountType();
        private Blue.BL.Reference.Account account = new Blue.BL.Reference.Account();
        private Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();
      
        private DataSet _dataSource = new DataSet();
        /// <summary>
        /// Get or Set Control DataSource.
        /// </summary>
        public DataSet DataSource
        {
            get
            {
                this._dataSource = (ViewState["DataSource"] == null ? new DataSet() : (DataSet)ViewState["DataSource"]);
                return this._dataSource;
            }
            set
            {
                this._dataSource = value;
                ViewState.Add("DataSource", this._dataSource);
            }
        }

        private bool _readOnly;
        /// <summary>
        /// Get or Set Control DataSource.
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                this._readOnly = (ViewState["ReadOnly"] == null ? false : (bool)ViewState["ReadOnly"]);
                return this._readOnly;
            }
            set
            {
                this._readOnly = value;
                ViewState.Add("ReadOnly", this._readOnly);
            }
        }

        private Guid _profileCode;
        /// <summary>
        /// Get or Set readonly property.
        /// </summary>
        public Guid ProfileCode
        {
            get
            {
                this._profileCode = (ViewState["ProfileCode"] == null ? new Guid() : (Guid)ViewState["ProfileCode"]);
                return this._profileCode;
            }
            set
            {
                this._profileCode = value;
                ViewState.Add("ProfileCode", this._profileCode);
            }
        }



        #endregion

        #region "Operations"

        /// <summary>
        /// Add new bankaccout information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        protected void btn_New_Click(object sender, EventArgs e)
        {

            DataRow drNew                = this.DataSource.Tables[bankAccount.TableName].NewRow();

            drNew["ProfileCode"]         = this.ProfileCode;
            drNew["SeqNo"]               = bankAccount.GetBankAccountMaxSeqNo(LoginInfo.ConnStr);
            drNew["BankCode"]            = string.Empty;
            drNew["BankAccountCode"]     = string.Empty;
            drNew["BankAccountName"]     = string.Empty;
            drNew["BankAccountTypeCode"] = string.Empty;
            drNew["AccountCode"]         = string.Empty;
            drNew["Branch"]              = string.Empty;

            // Add new row
            this.DataSource.Tables[bankAccount.TableName].Rows.Add(drNew);

            // Editing on new row
            grd_BankAccount.DataSource = this.DataSource.Tables[bankAccount.TableName];
            grd_BankAccount.EditIndex = grd_BankAccount.Rows.Count;
            grd_BankAccount.DataBind();

           
        }


        /// <summary>
        /// Binding bankaccount.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_BankAccount_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            // DataRow
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                // Bank
                if (e.Row.FindControl("lbl_Bank") != null)
                {
                    Label lbl_Bank = (Label)e.Row.FindControl("lbl_Bank");
                    lbl_Bank.Text = bank.GetBankName(DataBinder.Eval(e.Row.DataItem, "BankCode").ToString(), LoginInfo.ConnStr);

                }

                // Bank
                if (e.Row.FindControl("ddl_Bank") != null)
                {
                    DropDownList ddl_Bank = (DropDownList)e.Row.FindControl("ddl_Bank");
                    ddl_Bank.DataSource = bank.GetBankList(LoginInfo.ConnStr);
                    ddl_Bank.DataTextField = "Name";
                    ddl_Bank.DataValueField = "BankCode";
                    ddl_Bank.DataBind();
                    ddl_Bank.SelectedValue = DataBinder.Eval(e.Row.DataItem, "BankCode").ToString();

                }

                // Branch
                if (e.Row.FindControl("lbl_Branch") != null)
                {
                    Label lbl_Branch = (Label)e.Row.FindControl("lbl_Branch");
                    lbl_Branch.Text = (DataBinder.Eval(e.Row.DataItem, "Branch").ToString());

                }


                // BranchName
                if (e.Row.FindControl("txt_Branch") != null)
                {
                    TextBox txt_Branch = (TextBox)e.Row.FindControl("txt_Branch");
                    txt_Branch.Text = (DataBinder.Eval(e.Row.DataItem, "Branch").ToString());

                }

                // BankAccountCode
                if (e.Row.FindControl("lbl_Bank_Acc") != null)
                {
                    Label lbl_Bank_Acc = (Label)e.Row.FindControl("lbl_Bank_Acc");
                    lbl_Bank_Acc.Text = DataBinder.Eval(e.Row.DataItem, "BankAccountCode").ToString();

                }

                // BankAccountCode
                if (e.Row.FindControl("txt_Bank_Acc") != null)
                {
                    TextBox txt_Bank_Acc = (TextBox)e.Row.FindControl("txt_Bank_Acc");
                    txt_Bank_Acc.Text = DataBinder.Eval(e.Row.DataItem, "BankAccountCode").ToString();

                }

                // BankAccountName
                if (e.Row.FindControl("lbl_Bank_AccName") != null)
                {
                    Label lbl_Bank_AccName = (Label)e.Row.FindControl("lbl_Bank_AccName");
                    lbl_Bank_AccName.Text = DataBinder.Eval(e.Row.DataItem, "BankAccountName").ToString();

                }


                // BankAccountName
                if (e.Row.FindControl("txt_Bank_AccName") != null)
                {
                    TextBox txt_Bank_AccName = (TextBox)e.Row.FindControl("txt_Bank_AccName");
                    txt_Bank_AccName.Text = DataBinder.Eval(e.Row.DataItem, "BankAccountName").ToString();

                }

                // BankAccountTypeCode
                if (e.Row.FindControl("lbl_Bank_AccType") != null)
                {
                    Label lbl_Bank_AccType = (Label)e.Row.FindControl("lbl_Bank_AccType");
                    lbl_Bank_AccType.Text = bankAccountType.GetBankAccountTypeName(DataBinder.Eval(e.Row.DataItem, "BankAccountTypeCode").ToString(), LoginInfo.ConnStr);

                }

                // BankAccountTypeCode
                if (e.Row.FindControl("ddl_Bank_AccType") != null)
                {
                    DropDownList ddl_Bank_AccType = (DropDownList)e.Row.FindControl("ddl_Bank_AccType");
                    ddl_Bank_AccType.DataSource = bankAccountType.GetBankAccountTypeList(LoginInfo.ConnStr);
                    ddl_Bank_AccType.DataTextField = "Name";
                    ddl_Bank_AccType.DataValueField = "BankAccountTypeCode";
                    ddl_Bank_AccType.DataBind();
                    ddl_Bank_AccType.SelectedValue = DataBinder.Eval(e.Row.DataItem, "BankAccountTypeCode").ToString();

                }

                // AccountCode
                if (e.Row.FindControl("lbl_AccCode") != null)
                {
                    Label lbl_AccCode = (Label)e.Row.FindControl("lbl_AccCode");
                    lbl_AccCode.Text = DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString();

                }

                // AccountCode
                if (e.Row.FindControl("txt_AccCode") != null)
                {
                    TextBox txt_AccCode = (TextBox)e.Row.FindControl("txt_AccCode");
                    txt_AccCode.Text = DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString();

                }


                // Accountlist
                if (e.Row.FindControl("ddl_AccountList") != null)
                {
                    DropDownList ddl_AccountList = (DropDownList)e.Row.FindControl("ddl_AccountList");
                    ddl_AccountList.DataSource = account.GetAccountListLookup(LoginInfo.ConnStr);
                    ddl_AccountList.DataTextField = "NameEng";
                    ddl_AccountList.DataValueField = "AccountCode";
                    ddl_AccountList.DataBind();
                    ddl_AccountList.SelectedValue = DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString();

                }


                // AccountCodeName
                if (e.Row.FindControl("lbl_AccCode_Name") != null)
                {
                    Label lbl_AccCode_Name = (Label)e.Row.FindControl("lbl_AccCode_Name");
                    lbl_AccCode_Name.Text = account.GetName(DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString(), LoginInfo.ConnStr);

                }

                // AccountCodeName
                if (e.Row.FindControl("txt_AccCode_Name") != null)
                {
                    TextBox txt_AccCode_Name = (TextBox)e.Row.FindControl("txt_AccCode_Name");
                    txt_AccCode_Name.Text = account.GetName(DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString(), LoginInfo.ConnStr);

                }

            }

        }

        /// <summary>
        /// Bankaccount row deleting event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_BankAccount_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Setup primary key(s)
            string profileCode = grd_BankAccount.DataKeys[e.RowIndex].Values[0].ToString();
            System.Int16 seqNo = Convert.ToInt16(grd_BankAccount.DataKeys[e.RowIndex].Values[1]);


            // Delete the record 
            for (int i = this.DataSource.Tables[bankAccount.TableName].Rows.Count - 1; i >= 0; i--)
            {
                DataRow drDelete = this.DataSource.Tables[bankAccount.TableName].Rows[i];

                if (drDelete.RowState != DataRowState.Deleted)
                {
                    if (drDelete["profileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                       Convert.ToInt16(drDelete["seqNo"]) == seqNo)
                    {
                        drDelete.Delete();
                        continue;
                    }
                }
            }

            // Binding grid
            grd_BankAccount.DataSource = this.DataSource.Tables[bankAccount.TableName];
            grd_BankAccount.DataBind();

            
        }

        /// <summary>
        /// Bankaccount row cancelingedit event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_BankAccount_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            // Setup primary key(s)
            string profileCode = grd_BankAccount.DataKeys[e.RowIndex].Values[0].ToString();
            System.Int16 seqNo = Convert.ToInt16(grd_BankAccount.DataKeys[e.RowIndex].Values[1]);

            // Delete new row
            for (int i = this.DataSource.Tables[bankAccount.TableName].Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = this.DataSource.Tables[bankAccount.TableName].Rows[i];

                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["profileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                       Convert.ToInt16(dr["seqNo"]) == seqNo &&
                        dr.RowState == DataRowState.Added)
                    {
                        dr.Delete();
                        break;
                    }
                }
            }

            // Refresh data in gridview
            grd_BankAccount.DataSource = this.DataSource.Tables[bankAccount.TableName];
            grd_BankAccount.EditIndex = -1;
            grd_BankAccount.DataBind();

          
        }

        /// <summary>
        /// bankaccout row updating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_BankAccount_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Setup primary key(s)
            // Setup primary key(s)
            string profileCode = grd_BankAccount.DataKeys[e.RowIndex].Values[0].ToString();
            System.Int16 seqNo = Convert.ToInt16(grd_BankAccount.DataKeys[e.RowIndex].Values[1]);

            foreach (DataRow drUpdating in this.DataSource.Tables[bankAccount.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {

                    if (drUpdating["profileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                       Convert.ToInt16(drUpdating["seqNo"]) == seqNo)
                    {
                        // Find controls inside of gridview.
                        DropDownList ddl_Bank          = (DropDownList)grd_BankAccount.Rows[e.RowIndex].FindControl("ddl_Bank");
                        TextBox txt_BranchCode         = (TextBox)grd_BankAccount.Rows[e.RowIndex].FindControl("txt_BranchCode");
                        TextBox txt_Branch             = (TextBox)grd_BankAccount.Rows[e.RowIndex].FindControl("txt_Branch");
                        TextBox txt_Bank_Acc           = (TextBox)grd_BankAccount.Rows[e.RowIndex].FindControl("txt_Bank_Acc");
                        TextBox txt_Bank_AccName       = (TextBox)grd_BankAccount.Rows[e.RowIndex].FindControl("txt_Bank_AccName");
                        DropDownList ddl_Bank_AccType  = (DropDownList)grd_BankAccount.Rows[e.RowIndex].FindControl("ddl_Bank_AccType");
                        TextBox txt_AccCode            = (TextBox)grd_BankAccount.Rows[e.RowIndex].FindControl("txt_AccCode");
                        TextBox txt_AccCode_Name       = (TextBox)grd_BankAccount.Rows[e.RowIndex].FindControl("txt_AccCode_Name");
                        DropDownList ddl_AccountList   = (DropDownList)grd_BankAccount.Rows[e.RowIndex].FindControl("ddl_AccountList");


                        // Updating for assign data.
                        // BankCode
                        drUpdating["BankCode"] = ddl_Bank.SelectedItem.Value.ToString();

                        // BankAccountCode
                        drUpdating["BankAccountCode"] = txt_Bank_Acc.Text.Trim();

                        // BankAccountName
                        drUpdating["BankAccountName"] = txt_Bank_AccName.Text.Trim();

                        // BankAccountTypeCode
                        drUpdating["BankAccountTypeCode"] = ddl_Bank_AccType.SelectedValue.ToString();

                        // AccountCode
                        drUpdating["AccountCode"] = ddl_AccountList.SelectedItem.Value.ToString();

                        // Branch
                        drUpdating["Branch"] = txt_Branch.Text.Trim();

                        // Refresh data in GridView
                        grd_BankAccount.EditIndex = -1;
                        grd_BankAccount.DataSource = this.DataSource.Tables[bankAccount.TableName];
                        grd_BankAccount.DataBind();

                      

                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Bankaccount row editing event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_BankAccount_RowEditing(object sender, GridViewEditEventArgs e)
        {

            grd_BankAccount.EditIndex = e.NewEditIndex;
            grd_BankAccount.DataSource = this.DataSource.Tables[bankAccount.TableName];
            grd_BankAccount.DataBind();


        }



        /// <summary>
        /// Get bankaccount data.
        /// </summary>
        private void Page_Retrieve()
        {

        }

        /// <summary>
        /// Display bankaccount data.
        /// </summary>
        private void Page_Setting()
        {
            if (ReadOnly)
            {
                grd_BankAccount.Columns[0].Visible = false;
                btn_New.Visible = false;
            }

            grd_BankAccount.DataSource = this.DataSource.Tables[bankAccount.TableName];
            grd_BankAccount.DataBind();

        }

        /// <summary>
        /// Main process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Page_Retrieve();
                this.Page_Setting();

            }
          
        }


        #endregion
    }
}