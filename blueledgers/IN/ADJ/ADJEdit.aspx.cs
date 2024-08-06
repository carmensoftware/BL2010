using System;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.ADJ
{
    public partial class ADJEdit : BasePage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Text.ToUpper())
            {
                case "SAVE":
                    Response.Redirect("ADJ.aspx");
                    break;
                case "BACK":
                    Response.Redirect("ADJ.aspx");
                    break;
            }
        }
    }
}