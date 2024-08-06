using System;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.SPO
{
    public partial class SPO : BasePage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Text.ToUpper())
            {
                case "EDIT":
                    Response.Redirect("SPOEdit.aspx");
                    break;
                case "DELETE":
                    Response.Redirect("SPOLst.aspx");
                    break;
                case "PRINT":
                    break;
                case "BACK":
                    Response.Redirect("SPOLst.aspx");
                    break;
            }
        }
    }
}