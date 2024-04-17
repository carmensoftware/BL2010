using System;

namespace ErrorPages
{
    public partial class ErrorPagesSessionTimeOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AppendHeader("Refresh", "13;url=../login.aspx");
        }
    }
}