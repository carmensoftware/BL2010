using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

// ReSharper disable once CheckNamespace
namespace BlueLedger.PL.UserControls.AP
{
    public partial class Vendor : BaseUserControl
    {
        #region "Attributies"

        private readonly Blue.BL.AP.Vendor _vendor = new Blue.BL.AP.Vendor();
        private DataSet _dsVendor = new DataSet();

        #endregion

        #region "Operations"

        /// <summary>
        ///     retrieve for all data.
        /// </summary>
        private void Page_Retrieve()
        {
            // For vendor detail aspxgrid.
            _vendor.GetVendorList(_dsVendor, LoginInfo.ConnStr);
        }

        /// <summary>
        ///     setting for all of controls.
        /// </summary>
        private void Page_Setting()
        {
            // Binding vendor detail gridview.
            grd_Result.DataSource = _dsVendor.Tables[_vendor.TableName];
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
                _dsVendor = (DataSet) Session["dsVendor"];
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
            var vendorPopup = (AjaxControlToolkit.ModalPopupExtender) Parent.FindControl("VendorPopup");
            vendorPopup.Show();

            // request search content.
            var searchParam = txt_Search.Text.Trim();

            // query for match value.
            var dtSearch = _vendor.GetSearchVendorList(searchParam, LoginInfo.ConnStr);

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
                e.Row.Attributes.Add("onDblClick", string.Format("VendorCode_Click('{0}','{1}')", DataBinder.Eval(e.Row.DataItem, "VendorCode"), _vendor.GetVendorName(DataBinder.Eval(e.Row.DataItem, "ProfileCode").ToString(), LoginInfo.ConnStr)));
                e.Row.Style.Add("cursor", "hand");

                // Vendor Code
                if (e.Row.FindControl("lbl_VendorCode") != null)
                {
                    var lblVendorCode = (Label) e.Row.FindControl("lbl_VendorCode");
                    lblVendorCode.Text = DataBinder.Eval(e.Row.DataItem, "VendorCode").ToString();
                }

                // Vendor Name
                if (e.Row.FindControl("lbl_VendorName") != null)
                {
                    var lblVendorName = (Label) e.Row.FindControl("lbl_VendorName");
                    lblVendorName.Text = _vendor.GetVendorName(DataBinder.Eval(e.Row.DataItem, "ProfileCode").ToString(), LoginInfo.ConnStr);
                }


                // Description
                if (e.Row.FindControl("lbl_Description") != null)
                {
                    var lblDescription = (Label) e.Row.FindControl("lbl_Description");
                    lblDescription.Text = DataBinder.Eval(e.Row.DataItem, "Description").ToString();
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