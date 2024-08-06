using System;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.ISS
{
    public partial class ISS : BasePage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Text.ToUpper())
            {
                case "EDIT":
                    Response.Redirect("ISSEdit.aspx");
                    break;
                case "DELETE":
                    Response.Redirect("ISSLst.aspx");
                    break;
                case "PRINT":
                    break;
                case "BACK":
                    Response.Redirect("ISSLst.aspx");
                    break;
            }
        }
    }
}