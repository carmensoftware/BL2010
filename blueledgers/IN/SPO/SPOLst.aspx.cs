using System;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.SPO
{
    public partial class SPOLst : BasePage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Text.ToUpper())
            {
                case "CREATE":
                    Response.Redirect("SPOEdit.aspx");
                    break;
                case "PRINT":
                    break;
            }
        }
    }
}