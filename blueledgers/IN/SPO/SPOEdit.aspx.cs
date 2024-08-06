using System;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.SPO
{
    public partial class SPOEdit : BasePage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Text.ToUpper())
            {
                case "SAVE":
                    Response.Redirect("SPO.aspx");
                    break;
                case "BACK":
                    Response.Redirect("SPO.aspx");
                    break;
            }
        }
    }
}