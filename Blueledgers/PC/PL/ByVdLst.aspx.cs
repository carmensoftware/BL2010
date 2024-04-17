using System;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.PC.PL
{
    public partial class ByVdLst : BasePage
    {
        #region "Attributes"

        //DataSet dsVdLst = new DataSet();
        //BlueLedger.BL.IN.PriceList prictList = new BlueLedger.BL.IN.PriceList();

        #endregion

        #region "Operations"

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                ListPage.PrintItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Price List By Vendor", "", "",
                    "~/RPT/ReportCrit.aspx?rn=Plistv", "_blank"));
                ListPage.DataBind();
            }
        }

        #endregion

        //private void Page_Retrieve()
        //{

        //    prictList.GetGroupVendor(dsVdLst, LoginInfo.ConnStr);
        //    //prictList.GetGroupProduct(dsVdLst, LoginInfo.ConnStr);
        //    prictList.GetList(dsVdLst, LoginInfo.ConnStr);

        //    Session["dsVdLst"] = dsVdLst;

        //    this.Page_Setting();

        //}

        //private void Page_Setting()
        //{
        //    grd_VendorList.DataSource = dsVdLst.Tables[prictList.TableName];
        //    grd_VendorList.DataBind();

        //    grd_VendorList.Visible = true;
        //    grd_PriceList.Visible = false;

        //}

        //protected void lnkb_New_Click(object sender, EventArgs e)
        //{

        //}

        //protected void ddl_Type_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    dsVdLst.Clear();

        //    if (ddl_Type.SelectedValue.ToUpper() == "V")
        //    {
        //        prictList.GetGroupVendor(dsVdLst, LoginInfo.ConnStr);
        //        grd_VendorList.DataSource = dsVdLst.Tables[prictList.TableName];
        //        grd_VendorList.DataBind();

        //        grd_VendorList.Visible = true;
        //        grd_PriceList.Visible = false;
        //    }
        //    else
        //    {
        //        prictList.GetGroupProduct(dsVdLst, LoginInfo.ConnStr);
        //        grd_PriceList.DataSource = dsVdLst.Tables[prictList.TableName];
        //        grd_PriceList.DataBind();

        //        grd_VendorList.Visible = false;
        //        grd_PriceList.Visible = true;
        //    }


        //}

        //protected void btn_OK_Click(object sender, EventArgs e)
        //{

        //}
        //protected void grd_VendorList_RowEditing(object sender, GridViewEditEventArgs e)
        //{
        //    Response.Redirect("ByVdEdit.aspx?prlno=" + dsVdLst.Tables[prictList.TableName].Rows[e.NewEditIndex]["PrlNo"]);

        //}
        //protected void grd_PriceList_RowEditing(object sender, GridViewEditEventArgs e)
        //{
        //    Response.Redirect("ByVdEdit.aspx?prlno=" + dsVdLst.Tables[prictList.TableName].Rows[e.NewEditIndex]["PrlNo"]);

        //    //DataRow dr = dsVdLst.Tables[prictList.TableName].NewRow();
        //    //dr["PrlNo"] = dsVdLst.Tables[prictList.TableName].Rows[0]["PrlNo"];
        //}
    }
}