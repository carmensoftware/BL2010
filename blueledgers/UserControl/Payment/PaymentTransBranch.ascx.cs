using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

// ReSharper disable once CheckNamespace
namespace BlueLedger.PL.UserControls.AP
{
    public partial class PaymentTransBranch : BaseUserControl
    {
        #region "Attributies"

        private readonly Blue.BL.Bank.Bank _bank = new Blue.BL.Bank.Bank();
        private DataSet _dsBranch = new DataSet();

        #endregion

        #region "Operations"

        /// <summary>
        ///     retrieve for all data.
        /// </summary>
        private void Page_Retrieve()
        {
            // For branch detail aspxgrid.
            _bank.GetBankList(_dsBranch, LoginInfo.ConnStr);
        }

        /// <summary>
        ///     setting for all of controls.
        /// </summary>
        private void Page_Setting()
        {
            // Binding branch detail gridview.
            grd_Result.DataSource = _dsBranch.Tables[_bank.TableName];
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

                _dsBranch = (DataSet) Session["dsBranch"];
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
            var molPaymentTransBranchPopup =
                (AjaxControlToolkit.ModalPopupExtender) Parent.FindControl("MolPaymentTransBranchPopup");
            molPaymentTransBranchPopup.Show();

            // request search content.
            var searchParam = txt_Search.Text.Trim();

            // query for match value.
            var dtSearch = _bank.GetSearchBranchList(searchParam, LoginInfo.ConnStr);

            // search result show and bind.
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
                e.Row.Attributes.Add("onDblClick", string.Format("BranchCode_Click('{0}','{1}')", DataBinder.Eval(e.Row.DataItem, "BranchCode"), DataBinder.Eval(e.Row.DataItem, "BranchName")));
                e.Row.Style.Add("cursor", "hand");

                // Branch Code
                if (e.Row.FindControl("lbl_BranchCode") != null)
                {
                    var lblBranchCode = (Label) e.Row.FindControl("lbl_BranchCode");
                    lblBranchCode.Text = DataBinder.Eval(e.Row.DataItem, "BranchCode").ToString();
                }

                // Branch Name
                if (e.Row.FindControl("lbl_BranchName") != null)
                {
                    var lblBranchName = (Label) e.Row.FindControl("lbl_BranchName");
                    lblBranchName.Text = DataBinder.Eval(e.Row.DataItem, "BranchName").ToString();
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