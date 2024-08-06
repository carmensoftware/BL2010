using System;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.PC.CN
{
    public partial class CnView : BasePage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Text.ToUpper())
            {
                case "EDIT":
                    Response.Redirect("CnEdit.aspx");
                    break;
                case "VOID":
                    break;
                case "PRINT":
                    // Do nothings
                    break;
                case "BACK":
                    Response.Redirect("CnList.aspx");
                    break;
            }
        }
    }
}