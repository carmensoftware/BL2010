using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

// ReSharper disable once CheckNamespace
namespace BlueLedger.PL.UserControls.AP
{
    public partial class InvoiceItemsAccount : BaseUserControl
    {
        #region "Attributies"

        private readonly Blue.BL.Reference.Account _account = new Blue.BL.Reference.Account();
        private DataSet _dsAccount = new DataSet();

        #endregion

        #region "Operations"

        /// <summary>
        ///     retrieve for all data.
        /// </summary>
        private void Page_Retrieve()
        {
            // For account detail gridview.
            _account.GetAccountList(_dsAccount, LoginInfo.ConnStr);
        }

        /// <summary>
        ///     setting for all of controls.
        /// </summary>
        private void Page_Setting()
        {
            // Binding account detail gridview.
            grd_Result.DataSource = _dsAccount.Tables[_account.TableName];
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

                _dsAccount = (DataSet) Session["dsAccount"];
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
            var molInvoiceItemsPopup = (AjaxControlToolkit.ModalPopupExtender) Parent.FindControl("MolInvoiceItemsPopup");
            molInvoiceItemsPopup.Show();

            // request search content.
            var searchParam = txt_Search.Text.Trim();

            // query for match value.
            var dtSearch = _account.GetSearchAccountList(searchParam, LoginInfo.ConnStr);

            grd_Result.DataSource = dtSearch;
            grd_Result.DataBind();
        }

        /// <summary>
        ///     Button close event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkb_Close_Click(object sender, EventArgs e)
        {
            var molInvoiceItemsPopup =
                (AjaxControlToolkit.ModalPopupExtender) Parent.FindControl("MolInvoiceItemsPopup");

            lnkb_Close.Attributes.Add("OnClick", "ModalPoupExtenderHide('" + molInvoiceItemsPopup.ClientID + "');");
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
                var txtItemsAccount = (TextBox) Parent.FindControl("txt_ItemsAccount");
                var txtItemsAccountName = (TextBox) Parent.FindControl("txt_ItemsAccountName");
                var molInvoiceItemsPopup = (AjaxControlToolkit.ModalPopupExtender) Parent.FindControl("MolInvoiceItemsPopup");

                e.Row.Attributes.Add("onMouseOver", "GridView_Row_MouseOver(this);");
                e.Row.Attributes.Add("onMouseOut", "GridView_Row_MouseOut(this);");
                e.Row.Attributes.Add("onDblClick", string.Format("InvoiceItemsAccount_Click('{0}','{1}','{2}','{3}','{4}')", DataBinder.Eval(e.Row.DataItem, "AccountCode"), DataBinder.Eval(e.Row.DataItem, "NameEng"), txtItemsAccount.ClientID, txtItemsAccountName.ClientID, molInvoiceItemsPopup.ClientID));
                e.Row.Style.Add("cursor", "hand");


                // Vendor Code
                if (e.Row.FindControl("lbl_AccountCode") != null)
                {
                    var lblAccountCode = (Label) e.Row.FindControl("lbl_AccountCode");
                    lblAccountCode.Text = DataBinder.Eval(e.Row.DataItem, "AccountCode").ToString();
                }


                // Name
                if (e.Row.FindControl("lbl_Name") != null)
                {
                    var lblName = (Label) e.Row.FindControl("lbl_Name");
                    lblName.Text = DataBinder.Eval(e.Row.DataItem, "NameEng").ToString();
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