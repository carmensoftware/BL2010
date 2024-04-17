using System;
using BlueLedger.PL.BaseClass;



namespace BlueLedger.PL.PC.CN
{
    public partial class CnList : BasePage
    {
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly string moduleID = "2.4";

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListPage2.PrintItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Summary", "CNSM"));
                ListPage2.DataBind();
            }

            ListPage2.CreateItems.Menu.ItemClick += menu_ItemClick;
            Control_HeaderMenuBar();
            base.Page_Load(sender, e);
        }

        // Added on: 09/10/2017, By: Fon
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
                case "CNSM":

                    Session["dtBuKeys"] = ListPage2.dtBuKeys;

                    var reportLink3 = "../../RPT/ReportCriteria.aspx?category=012&reportid=390";
                    ClientScript.RegisterStartupScript(GetType(), "newWindow",
                        "<script>window.open('" + reportLink3 + "','_blank')</script>");
                    break;

                default:
                    Response.Redirect("~/PC/CN/CnEdit.aspx?BuCode=" + LoginInfo.BuFmtInfo.BuCode + "&MODE=NEW");
                    break;
            }
        }
    }
}