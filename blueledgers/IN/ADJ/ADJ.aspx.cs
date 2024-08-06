using System;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.ADJ
{
    public partial class ADJ : BasePage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Text.ToUpper())
            {
                case "EDIT":
                    Response.Redirect("ADJEdit.aspx");
                    break;
                case "DELETE":
                    Response.Redirect("ADJLst.aspx");
                    break;
                case "PRINT":
                    break;
                case "BACK":
                    Response.Redirect("ADJLst.aspx");
                    break;
            }
        }
    }
}