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
    public partial class ContactDetail : BaseUserControl
    {
        #region "Attributies"

        private Blue.BL.Profile.ContactDetail contactDetail = new Blue.BL.Profile.ContactDetail();
        private DataSet                dsContactDetail   = new DataSet();


        #endregion

        #region "Operations"

        /// <summary>
        /// retrieve for all data.
        /// </summary>
        private void Page_Retrieve()
        {
            // For branch detail gridview.
            contactDetail.GetContactDetailList(dsContactDetail,LoginInfo.ConnStr);
        }

        /// <summary>
        /// setting for all of controls.
        /// </summary>
        private void Page_Setting()
        {

            // Binding branch detail gridview.
            grd_Result.DataSource = dsContactDetail.Tables[contactDetail.TableName];
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

                dsContactDetail = (DataSet)Session["dsContactDetail"];
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
            AjaxControlToolkit.ModalPopupExtender MolContactDetailPopup = (AjaxControlToolkit.ModalPopupExtender)Parent.FindControl("MolContactDetailPopup");
            MolContactDetailPopup.Show();

            // request search content.
            string Search_Param = this.txt_Search.Text.Trim();

            // query for match value.
            DataTable dtSearch = contactDetail.GetSearchContactDetailList(Search_Param, LoginInfo.ConnStr);

            // search result show and bind.
            grd_Result.DataSource = dtSearch;
            grd_Result.DataBind();

        }

        /// <summary>
        /// Close button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkb_Close_Click(object sender, EventArgs e)
        {
            AjaxControlToolkit.ModalPopupExtender MolContactDetailPopup = (AjaxControlToolkit.ModalPopupExtender)Parent.FindControl("MolContactDetailPopup");

            lnkb_Close.Attributes.Add("onClick", "ModalPoupExtenderHide('"+ MolContactDetailPopup.ClientID +"');");
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

                TextBox txt_Contact   = (TextBox)Parent.FindControl("txt_Contact");
                AjaxControlToolkit.ModalPopupExtender MolContactDetailPopup = (AjaxControlToolkit.ModalPopupExtender)Parent.FindControl("MolContactDetailPopup");

                e.Row.Attributes.Add("onMouseOver", "GridView_Row_MouseOver(this);");
                e.Row.Attributes.Add("onMouseOut", "GridView_Row_MouseOut(this);");
                e.Row.Attributes.Add("onDblClick", "Contact_Click('" + DataBinder.Eval(e.Row.DataItem, "Contact").ToString() +
                  "','" + txt_Contact.ClientID +
                  "','" + MolContactDetailPopup.ClientID +
                   "')");
                e.Row.Style.Add("cursor","hand");

                // ContactDetail Code
                if (e.Row.FindControl("lbl_ContactDetailID") != null)
                {
                    Label lbl_ContactDetailID = (Label)e.Row.FindControl("lbl_ContactDetailID");
                    lbl_ContactDetailID.Text  = DataBinder.Eval(e.Row.DataItem, "ContactDetailID").ToString();
                }

                // ContactDetail Name
                if (e.Row.FindControl("lbl_Contact") != null)
                {
                    Label lbl_Contact = (Label)e.Row.FindControl("lbl_Contact");
                    lbl_Contact.Text = DataBinder.Eval(e.Row.DataItem, "Contact").ToString();
                }

                // Remark 
                if (e.Row.FindControl("lbl_Remark") != null)
                {
                    Label lbl_Remark = (Label)e.Row.FindControl("lbl_Remark");
                    lbl_Remark.Text = DataBinder.Eval(e.Row.DataItem, "Remark").ToString();
                }
            }
        }
        #endregion
    }
}