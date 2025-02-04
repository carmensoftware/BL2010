using System;
using BlueLedger.PL.BaseClass;
using System.Web.UI.WebControls;

namespace BlueLedger.PL.PT.RCP
{
    public partial class RecipeList : BasePage
    {
        #region Variable
        private readonly Blue.BL.PT.RCP.RcpDt rcpDt = new Blue.BL.PT.RCP.RcpDt();
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();

        private readonly string moduleID = "4.1";
        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            var vid = Request.Cookies["[PT].[vRcpList]"] == null ? "0" : Request.Cookies["[PT].[vRcpList]"].Value;

            if (!Page.IsPostBack)
            {
                ListPage2.PrintItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Recipe List", "SL"));

                ListPage2.CreateItems.NavigateUrl = "~/PT/RCP/RecipeEdit.aspx?MODE=new&BuCode=" + LoginInfo.BuInfo.BuCode + "&VID=" + vid;

                // Added on: 15/09/2017, By:Fon, For: Create update cost Option.
                ListPage2.menuItems.Add("Update Cost", "UC");
                ListPage2.menuItems.FindByName("UC").ItemStyle.ForeColor = System.Drawing.Color.White;
                ListPage2.menuItems.FindByName("UC").ItemStyle.HoverStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D8D8D8");
                ListPage2.menuItems.FindByName("UC").ItemStyle.Font.Size = 7;
                ListPage2.menuItems.FindByName("UC").ItemStyle.VerticalAlign = VerticalAlign.Bottom;
                ListPage2.menuItems.FindByName("UC").VisibleIndex = 0;
                // End Added.
                ListPage2.DataBind();
            }

            ListPage2.CreateItems.Menu.ItemClick += menu_ItemClick;
            Control_HeaderMenuBar();
            base.Page_Load(sender, e);
        }

        // Added on: 06/10/2017, By: Fon
        protected void Control_HeaderMenuBar()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            ListPage2.menuItems.FindByName("UC").Visible = (pagePermiss >= 3)
                ? ListPage2.menuItems.FindByName("UC").Visible : false;
            ListPage2.CreateItems.Visible = (pagePermiss >= 3)
                ? ListPage2.CreateItems.Visible : false;
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

                // Added on: 15/09/2017, By: Fon
                // Note: Added in Business Layer
                case "UC":


                    rcpDt.DbExecuteQuery("EXEC [PT].[UpdateRecipeCost]", null, LoginInfo.ConnStr);

                    //var parameters = new Blue.DAL.DbParameter[1];
                    //parameters[0] = new Blue.DAL.DbParameter("@RcpCode", "");

                    //bool result = rcpDt.UpdateCostOfRecipe(p, LoginInfo.ConnStr);
                    //bool result = rcpDt.UpdateCostOfRecipe(parameters, LoginInfo.ConnStr);

                    //if (result)
                        Response.Redirect("RecipeList.aspx");

                    break;
                // End Added.
            }
        }



    }
  
}