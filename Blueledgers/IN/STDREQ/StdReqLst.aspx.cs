using System;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.STDREQ
{
    public partial class StdReqLst : BasePage
    {
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private string moduleID = "3.10.11";

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ListPage2.PrintItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Standard Requisition", "SL"));
                ListPage2.DataBind();

                ListPage2.CreateItems.NavigateUrl = "~/IN/STDREQ/StdReqEdit.aspx?BuCode=" +
                                                    LoginInfo.BuInfo.BuCode + "&MODE=NEW&VID=" +
                                                    Request.Cookies["[IN].[vStandardRequisition]"].Value;
            }
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
                    var reportLink = "../../RPT/ReportCriteria.aspx?category=012&reportid=331";
                    ClientScript.RegisterStartupScript(GetType(), "newWindow",
                        "<script>window.open('" + reportLink + "','_blank')</script>");
                    break;
            }
        }
    }
}