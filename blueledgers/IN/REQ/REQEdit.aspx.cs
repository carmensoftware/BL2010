using System;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.REQ
{
    public partial class REQEdt : BasePage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Text.ToUpper())
            {
                case "SAVE":
                    Response.Redirect("REQ.aspx");
                    break;
                case "BACK":
                    Response.Redirect("REQ.aspx");
                    break;
            }
        }
    }
}