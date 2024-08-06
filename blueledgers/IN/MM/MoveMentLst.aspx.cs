using System;
using System.Collections;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.MM
{
    public partial class MovementLst : BasePage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                ListPage2.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Create Stock In", "SI"));
                ListPage2.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Create Stock Out", "SO"));
                //ListPage2.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Create Transfer In", "TI"));
                ListPage2.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Create Transfer Out", "TO"));

                ListPage2.DataBind();
            }

            ListPage2.CreateItems.Menu.ItemClick += menu_ItemClick;
        }

        private void menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            ArrayList objArrList;
            objArrList = new ArrayList();

            switch (e.Item.Name.ToUpper())
            {
                case "SI":
                    Response.Redirect("~/IN/MM/MoveMentEdit.aspx?MODE=New&BuCode=" +
                                      LoginInfo.BuInfo.BuCode + "&Prefix=SI&VID=" +
                                      Request.Cookies["[IN].[vMoveMent]"].Value);
                    break;

                case "SO":
                    Response.Redirect("~/IN/MM/MoveMentEdit.aspx?MODE=New&BuCode=" +
                                      LoginInfo.BuInfo.BuCode + "&Prefix=SO&VID=" +
                                      Request.Cookies["[IN].[vMoveMent]"].Value);
                    break;

                    //case "TI":
                    //    Response.Redirect("~/IN/MM/MoveMentEdit.aspx?MODE=New&BuCode=" +
                    //    LoginInfo.BuInfo.BuCode + "&Prefix=TI&VID=" + Request.Cookies["[IN].[vMoveMent]"].Value.ToString());
                    //    break;

                case "TO":
                    //Response.Redirect("~/IN/MM/MoveMentEdit.aspx?MODE=New&BuCode=" +
                    //LoginInfo.BuInfo.BuCode + "&Prefix=TI&VID=" + Request.Cookies["[IN].[vMoveMent]"].Value.ToString());
                    Response.Redirect("~/IN/MM/SRtoMM.aspx?Prefix=TO&VID=" + Request.Cookies["[IN].[vMoveMent]"].Value);
                    break;
            }
        }
    }
}