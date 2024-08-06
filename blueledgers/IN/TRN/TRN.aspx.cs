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
                case "EDIT":
                    Response.Redirect("TRNEdit.aspx");
                    break;
                case "DELETE":
                    Response.Redirect("TRNLst.aspx");
                    break;
                case "PRINT":
                    break;
                case "BACK":
                    Response.Redirect("TRNLst.aspx");
                    break;
            }
        }
    }
}