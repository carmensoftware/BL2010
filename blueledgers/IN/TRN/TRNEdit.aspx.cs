using System;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.TRN
{
    public partial class TRN : BasePage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Text.ToUpper())
            {
                case "SAVE":
                    Response.Redirect("TRN.aspx");
                    break;
                case "BACK":
                    Response.Redirect("TRN.aspx");
                    break;
            }
        }
    }
}