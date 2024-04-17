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
    public partial class Vendor : BaseUserControl
    {
        #region "Attributies"

        private Blue.BL.AP.Vendor  vendor                = new Blue.BL.AP.Vendor();
        private DataSet      dsVendor               = new DataSet();
       

        #endregion

        #region "Operations"

        /// <summary>
        /// retrieve for all data.
        /// </summary>
        private void Page_Retrieve()
        {
            // For vendor detail aspxgrid.
            vendor.GetVendorList(dsVendor,LoginInfo.ConnStr);
        }

        /// <summary>
        /// setting for all of controls.
        /// </summary>
        private void Page_Setting()
        {
            //// Set current pointer on menu and shortcut
            //Master.MenuID = 7; // JournalVoucher
            //Master.MenuEnable = false;
            //Master.ShortcutID = 7; // JournalVoucher           
            //Master.ShowShortcut = false;
            //Master.AllowHelp = false;
            //Master.AllowSetup = false;
            //Master.AllowLogout = false;

            // Binding vendor detail gridview.
            grd_Result.DataSource = dsVendor.Tables[vendor.TableName];
            grd_Result.DataBind();
        }

        /// <summary>
        /// Page load event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected  void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
                Page_Setting();
            }
            else
            {
                dsVendor = (DataSet)Session["dsVendor"];

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
            AjaxControlToolkit.ModalPopupExtender VendorPopup = (AjaxControlToolkit.ModalPopupExtender)Parent.FindControl("VendorPopup");
            VendorPopup.Show();
            
            // request search content.
            string Search_Param = this.txt_Search.Text.Trim();

            // query for match value.
            DataTable dtSearch = vendor.GetSearchVendorList(Search_Param, LoginInfo.ConnStr);

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
                e.Row.Attributes.Add("onDblClick", "VendorCode_Click('" + DataBinder.Eval(e.Row.DataItem, "VendorCode").ToString() +
                    "','" + vendor.GetVendorName(DataBinder.Eval(e.Row.DataItem, "ProfileCode").ToString(),LoginInfo.ConnStr) +
                    "')");
                e.Row.Style.Add("cursor", "hand");

                // Vendor Code
                if (e.Row.FindControl("lbl_VendorCode") != null)
                {
                    Label lbl_VendorCode = (Label)e.Row.FindControl("lbl_VendorCode");
                    lbl_VendorCode.Text  = DataBinder.Eval(e.Row.DataItem, "VendorCode").ToString();
                }

                // Vendor Name
                if (e.Row.FindControl("lbl_VendorName") != null)
                {
                    Label lbl_VendorName = (Label)e.Row.FindControl("lbl_VendorName");
                    lbl_VendorName.Text = vendor.GetVendorName(DataBinder.Eval(e.Row.DataItem, "ProfileCode").ToString(),LoginInfo.ConnStr);
                }


                // Description
                if (e.Row.FindControl("lbl_Description") != null)
                {
                    Label lbl_Description = (Label)e.Row.FindControl("lbl_Description");
                    lbl_Description.Text = DataBinder.Eval(e.Row.DataItem, "Description").ToString();
                }

                // Status
                if (e.Row.FindControl("img_Status") != null)
                {
                    ImageButton img_Status = (ImageButton)e.Row.FindControl("img_Status");
                    img_Status.ImageUrl    = ((bool)DataBinder.Eval(e.Row.DataItem, "IsActive") == false ? "/" + AppName + "/App_Themes/default/pics/red_light_icon.png" : "/" + AppName + "/App_Themes/default/pics/green_light_icon.png"); 
                }

                
                
            }
        }
        #endregion
    }
}