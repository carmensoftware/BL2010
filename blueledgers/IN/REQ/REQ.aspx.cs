using System;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.REQ
{
    public partial class REQ : BasePage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Text.ToUpper())
            {
                case "EDIT":
                    Response.Redirect("REQEdit.aspx");
                    break;
                case "DELETE":
                    Response.Redirect("REQLst.aspx");
                    break;
                case "PRINT":
                    break;
                case "BACK":
                    Response.Redirect("REQLst.aspx");
                    break;
            }
        }
    }
}