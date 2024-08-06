using System;

namespace BlueLedger.PL.Master.Opt
{
    public partial class MasterDefault : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void imgb_Home_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("~/Option/User/Default.aspx");
        }

        protected void imgb_ChangeBu_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("~/BuList.aspx");
        }

        protected void imgb_Option_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("~/Option/Default.aspx");
        }

        protected void imgb_Help_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("~/Help/Default.aspx");
        }

        protected void imgb_LogOut_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }
    }
}