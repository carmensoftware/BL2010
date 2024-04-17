using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress;
using DevExpress.Web;
using DevExpress.Web.ASPxEditors;
using BlueLedger;

using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.UserControls.AP
{
    public partial class PaymentDefaultTransBank : BaseUserControl
    {
        #region "Attributies"

        private Blue.BL.Bank.Bank bank = new Blue.BL.Bank.Bank();
        private DataSet dsBank = new DataSet();


        #endregion

        #region "Operations"

        /// <summary>
        /// retrieve for all data.
        /// </summary>
        private void Page_Retrieve()
        {
            // For bank detail gridview.
            bank.GetBankList(dsBank, LoginInfo.ConnStr);
        }

        /// <summary>
        /// setting for all of controls.
        /// </summary>
        private void Page_Setting()
        {

            // Binding bank detail gridview.
            grd_Result.DataSource = dsBank.Tables[bank.TableName];
            grd_Result.DataBind();
        }

        /// <summary>
        /// Page load event.
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

                dsBank = (DataSet)Session["dsBank"];

            }
        }

        /// <summary>
        /// search for all of column value with match and after binding aspxgrid control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Go_Click(object sender, EventArgs e)
        {

            // Show for ModalPoupExtender control.
            AjaxControlToolkit.ModalPopupExtender MolPaymentTransBankPoup = (AjaxControlToolkit.ModalPopupExtender)Parent.FindControl("MolPaymentTransBankPoup");
            MolPaymentTransBankPoup.Show();

            // request search content.
            string Search_Param = this.txt_Search.Text.Trim();

            // query for match value.
            DataTable dtSearch = bank.GetSearchBankList(Search_Param, LoginInfo.ConnStr);

            grd_Result.DataSource = dtSearch;
            grd_Result.DataBind();

        }

        /// <summary>
        /// grid inside controls data binding.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Result_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onMouseOver", "GridView_Row_MouseOver(this);");
                e.Row.Attributes.Add("onMouseOut", "GridView_Row_MouseOut(this);");
                e.Row.Attributes.Add("onDblClick", "TransBankCode_Click('" + DataBinder.Eval(e.Row.DataItem, "BankCode").ToString() +
                    "','" + DataBinder.Eval(e.Row.DataItem, "Name").ToString() +
                    "')");
                e.Row.Style.Add("cursor", "hand");

                // Vendor Code
                if (e.Row.FindControl("lbl_BankCode") != null)
                {
                    Label lbl_BankCode = (Label)e.Row.FindControl("lbl_BankCode");
                    lbl_BankCode.Text = DataBinder.Eval(e.Row.DataItem, "BankCode").ToString();
                }


                // Name
                if (e.Row.FindControl("lbl_Name") != null)
                {
                    Label lbl_Name = (Label)e.Row.FindControl("lbl_Name");
                    lbl_Name.Text = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                }

                // Status
                if (e.Row.FindControl("img_Status") != null)
                {
                    ImageButton img_Status = (ImageButton)e.Row.FindControl("img_Status");
                    img_Status.ImageUrl = ((bool)DataBinder.Eval(e.Row.DataItem, "IsActive") == false ? "/" + AppName + "/App_Themes/default/pics/red_light_icon.png" : "/" + AppName + "/App_Themes/default/pics/green_light_icon.png");
                }



            }
        }
        #endregion
    }
}