using System;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Inventory
{
    public partial class ProdList : BasePage
    {
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly string moduleID = "2.6";


        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Add create menu options                
                ListPage.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Create Manually", "MC"));
                ListPage.PrintItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Product List", "PL"));

                if (!LoginInfo.BuInfo.IsHQ)
                {
                    //ListPage.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("From HQ", "HQ"));
                }

                ListPage.DataBind();

            }

            ListPage.CreateItems.Menu.ItemClick += menu_ItemClick;
            base.Page_Load(sender, e);

            Control_HeaderMenuBar();
        }

        private void menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "MC":
                    //Response.Redirect("ProdEdit.aspx?MODE=new&BuCode=" + LoginInfo.BuFmtInfo.BuCode );
                    Response.Redirect("ProdEdit2.aspx?MODE=new&BuCode=" + LoginInfo.BuFmtInfo.BuCode);
                    break;

                case "HQ":
                    Response.Redirect("ProdSync.aspx");
                    break;

                case "PL":
                    Session["dtBuKeys"] = ListPage.dtBuKeys;

                    var reportLink = "../../RPT/ReportCriteria.aspx?category=001&reportid=140";
                    ClientScript.RegisterStartupScript(GetType(), "newWindow",
                        "<script>window.open('" + reportLink + "','_blank')</script>");

                    break;
            }
        }

        protected void Control_HeaderMenuBar()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            //ListPage.CreateItems.Visible = (pagePermiss >= 3) ? ListPage.CreateItems.Visible : false;
            ListPage.AllowCreate = (pagePermiss >= 3) ? true : false;

        }


     
    }
}