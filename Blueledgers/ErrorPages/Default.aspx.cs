using System;
using System.Web.UI.WebControls;

namespace ErrorPages
{
    public partial class ErrorPagesDefault : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Session["Err"] != null)
            {
                var objErr = (Exception) Session["Err"];
                var lblMessage = (Label) ASPxNavBar.Groups[0].FindControl("lbl_Message");
                var lblStackTrace = (Label) ASPxNavBar.Groups[0].FindControl("lbl_StackTrace");
                var lblTarget = (Label) ASPxNavBar.Groups[0].FindControl("lbl_Target");

                //string[] StackTrace = Regex.Split(objErr.StackTrace, "at");
                //var StackText = string.Empty;
                lblMessage.Text = string.Format("<br />{0}<br />", objErr.Message);
                //foreach (var outputStack in StackTrace)
                //{
                //   StackText += outputStack + "<br />";
                //}
                lblStackTrace.Text = objErr.StackTrace.Replace("at", "<br/>");
                lblTarget.Text = string.Format("<br />{0}", objErr.TargetSite);
            }
        }
    }
}