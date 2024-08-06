using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Store
{
    public partial class StoreLst2 : BasePage
    {
        #region "Attributes"

        private Blue.BL.GL.Account.Account accCode = new Blue.BL.GL.Account.Account();
        private Blue.BL.Option.Inventory.DeliveryPoint delPoint = new Blue.BL.Option.Inventory.DeliveryPoint();


        private DataSet dsStoreLct = new DataSet();
        private DataTable dtAccCode = new DataTable();
        private DataTable dtDeliPoint = new DataTable();
        private Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private Blue.BL.Option.Inventory.ProdLoc prodLoc = new Blue.BL.Option.Inventory.ProdLoc();
        private Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
        private Blue.BL.Option.Inventory.StoreLct strLct = new Blue.BL.Option.Inventory.StoreLct();

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ListPage1.PrintItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Store Location", "SLoc"));
                ListPage1.DataBind();
            }

            ListPage1.CreateItems.Menu.ItemClick += menu_ItemClick;
        }

        private void menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Response.Redirect("StoreEdit2.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&MODE=NEW&VID=" +
                                      Request.Cookies["[IN].[vStoreLocation]"].Value);
                    break;
                case "SLOC":

                    Session["dtBuKeys"] = ListPage1.dtBuKeys;
                    var reportLink = "../../RPT/ReportCriteria.aspx?category=001&reportid=200";
                    ClientScript.RegisterStartupScript(GetType(), "newWindow",
                        "<script>window.open('" + reportLink + "','_blank')</script>");
                    break;
            }
        }
    }
}