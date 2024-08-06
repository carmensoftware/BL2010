using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

// ReSharper disable once CheckNamespace
namespace BlueLedger.PL.UserControls.AP
{
    public partial class PaymentTransVendorBank : BaseUserControl
    {
        #region "Attributies"

        private readonly Blue.BL.Bank.Bank _bank = new Blue.BL.Bank.Bank();
        private DataSet _dsBank = new DataSet();

        #endregion

        #region "Operations"

        /// <summary>
        ///     retrieve for all data.
        /// </summary>
        private void Page_Retrieve()
        {
            // For bank detail gridview.
            _bank.GetBankList(_dsBank, LoginInfo.ConnStr);
        }

        /// <summary>
        ///     setting for all of controls.
        /// </summary>
        private void Page_Setting()
        {
            // Binding bank detail gridview.
            grd_Result.DataSource = _dsBank.Tables[_bank.TableName];
            grd_Result.DataBind();
        }

        /// <summary>
        ///     Page load event.
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
                // We need to do after PostBack have to read data and bind again.
                Page_Retrieve();
                Page_Setting();

                _dsBank = (DataSet) Session["dsBank"];
            }
        }

        /// <summary>
        ///     search for all of column value with match and after binding aspxgrid control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Go_Click(object sender, EventArgs e)
        {
            // Show for ModalPoupExtender control.
            var molPaymentTransVBankPopup =
                (AjaxControlToolkit.ModalPopupExtender) Parent.FindControl("MolPaymentTransVBankPopup");
            molPaymentTransVBankPopup.Show();

            // request search content.
            var searchParam = txt_Search.Text.Trim();

            // query for match value.
            var dtSearch = _bank.GetSearchBankList(searchParam, LoginInfo.ConnStr);

            grd_Result.DataSource = dtSearch;
            grd_Result.DataBind();
        }

        /// <summary>
        ///     grid inside controls data binding.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Result_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onMouseOver", "GridView_Row_MouseOver(this);");
                e.Row.Attributes.Add("onMouseOut", "GridView_Row_MouseOut(this);");
                e.Row.Attributes.Add("onDblClick",
                    "TransVendorBankCode_Click('" + DataBinder.Eval(e.Row.DataItem, "BankCode") +
                    "','" + DataBinder.Eval(e.Row.DataItem, "Name") +
                    "')");
                e.Row.Style.Add("cursor", "hand");

                // Vendor Code
                if (e.Row.FindControl("lbl_BankCode") != null)
                {
                    var lblBankCode = (Label) e.Row.FindControl("lbl_BankCode");
                    lblBankCode.Text = DataBinder.Eval(e.Row.DataItem, "BankCode").ToString();
                }


                // Name
                if (e.Row.FindControl("lbl_Name") != null)
                {
                    var lblName = (Label) e.Row.FindControl("lbl_Name");
                    lblName.Text = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                }

                // Status
                if (e.Row.FindControl("img_Status") != null)
                {
                    var imgStatus = (ImageButton) e.Row.FindControl("img_Status");
                    imgStatus.ImageUrl = ((bool) DataBinder.Eval(e.Row.DataItem, "IsActive") == false
                        ? string.Format("/{0}/App_Themes/default/pics/red_light_icon.png", AppName)
                        : string.Format("/{0}/App_Themes/default/pics/green_light_icon.png", AppName));
                }
            }
        }

        #endregion
    }
}