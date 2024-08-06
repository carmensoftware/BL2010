using System;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.ISS
{
    public partial class ISSLst : BasePage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Text.ToUpper())
            {
                case "CREATE":
                    Response.Redirect("ISSEdit.aspx");
                    break;
                case "PRINT":
                    break;
            }
        }
    }
}