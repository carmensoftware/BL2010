using System;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.STK
{
    public partial class StkInLst : BasePage
    {
        #region "Attributes"

        //BL.IN.StandardRequistion stdReq             = new BL.IN.StandardRequistion();
        //BL.IN.StandardRequisitionDetail stdReqDt    = new BL.IN.StandardRequisitionDetail();
        //BL.IN.storeRequisition storeReq = new BL.IN.storeRequisition();
        //BL.IN.StoreRequisitionDetail storeReqDt = new BL.IN.StoreRequisitionDetail();
        //DataSet dsStoreReq                          = new DataSet();

        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly string moduleID = "3.2";
        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            ListPage2.setViewMultipleBU(false);
            if (!Page.IsPostBack)
            {
                ListPage2.PrintItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Stock In List", "SL"));
                ListPage2.DataBind();

                ListPage2.CreateItems.NavigateUrl = "~/IN/STK/StkInEdit.aspx?MODE=new&BuCode=" + LoginInfo.BuInfo.BuCode +
                                                    "&VID=" + Request.Cookies["[IN].[vStockIn]"].Value;
            }
            //if (!Page.IsPostBack)
            //{
            //    ListPage.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Create", "CNew"));
            //    ListPage.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Create from Standard Requisition", "SR"));


            //}

            ListPage2.CreateItems.Menu.ItemClick += menu_ItemClick;
            Control_HeaderMenuBar();
            base.Page_Load(sender, e);
        }

        // Added on: 06/10/2017, By: Fon
        protected void Control_HeaderMenuBar()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            ListPage2.CreateItems.Visible = (pagePermiss >= 3) ? ListPage2.CreateItems.Visible : false;
        }
        // End Added.

        private void menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SL":

                    Session["dtBuKeys"] = ListPage2.dtBuKeys;
                    var reportLink = "../../RPT/ReportCriteria.aspx?category=012&reportid=322";
                    ClientScript.RegisterStartupScript(GetType(), "newWindow",
                        "<script>window.open('" + reportLink + "','_blank')</script>");
                    break;
            }
        }
    }
}