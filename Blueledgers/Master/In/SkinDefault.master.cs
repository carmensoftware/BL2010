using System;
using System.Data;
//using System.Data.SqlClient;
using System.Web.UI;
using System.Web;

namespace BlueLedger.PL.Master.In
{
    public partial class MasterDefault : System.Web.UI.MasterPage
    {
        /// <summary>
        ///     Gets current login user information.
        /// </summary>
        protected BaseClass.LoginInformation LoginInfo
        {
            get { return (BaseClass.LoginInformation)Session["LoginInfo"]; }
        }

        public string ParentID { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                // Display Login Information
                //lnk_UserName.Text = LoginInfo.LoginName;

                //Display Menu
                Menu.DataBind();

                if (Session["License"] != null)
                {
                    //Response.Write("<script>alert('" + Session["License"].ToString() + "');</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "alert('" + Session["License"].ToString() + "');", true);
                    Session["License"] = null;
                }
            }

            SetHeaderScripts();
        }


        private void SetHeaderScripts()
        {
            string rootPath = ResolveUrl("~");

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(string.Format(" <link rel='stylesheet' type='text/css' href='{0}App_Themes/Default/theme.css' />", rootPath));
            sb.Append(string.Format(" <link rel='stylesheet' type='text/css' href='{0}App_Themes/Default/layout.css' />", rootPath));
            //sb.Append(string.Format(" <link rel='stylesheet' type='text/css' href='{0}App_Themes/Default/theme.css' />", rootPath));
            sb.Append(string.Format(" <link rel='stylesheet' type='text/css' href='{0}Scripts/jquery-ui-1.8.21.custom.css' />", rootPath));
            sb.Append(string.Format(" <script type='text/javascript' src='{0}Scripts/jquery-1.9.1.js'></script>", rootPath));
            sb.Append(string.Format(" <script type='text/javascript' src='{0}Scripts/jquery-ui-1.8.21.custom.min.js'></script>", rootPath));
            sb.Append(string.Format(" <script type='text/javascript' src='{0}Scripts/GnxLib.js'></script>", rootPath));


            if (!Page.ClientScript.IsClientScriptBlockRegistered("StartupScript"))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "StartupScript", sb.ToString());
            }

            // ------------------------------------------

            System.Text.StringBuilder jsb = new System.Text.StringBuilder();
            jsb.Append("<script type='text/javascript'>");
            jsb.Append("  function Fix_ASPxPopupControl_For_Firefox() {");
            jsb.Append("    var elems = document.getElementsByClassName('dxpcModalBackground');");
            jsb.Append("    if (elems != null && elems.length > 0) {");
            jsb.Append("      for (var i = 0; i < elems.length - 1; i++) {");
            jsb.Append("        var id = elems[i].id.replace('_DXPWMB', '_TCFix');");
            jsb.Append("        var e = document.getElementById(id);");
            jsb.Append("        if (e != null)");
            jsb.Append("          e.style.position = 'absolute';");
            jsb.Append("      }");
            jsb.Append("    }");
            jsb.Append("  } </script>");

            //Render the function definition. 
            if (!Page.ClientScript.IsClientScriptBlockRegistered("JSScriptBlock"))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "JSScriptBlock", jsb.ToString());
            }

            //Render the function invocation. 
            string funcCall = "<script language='javascript'>Fix_ASPxPopupControl_For_Firefox();</script>";

            if (!Page.ClientScript.IsStartupScriptRegistered("JSScript"))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "JSScript", funcCall);
            } 
        }

        protected void Home_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("~/Option/User/Default.aspx");
        }


        protected void ChangeBu_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Session["BackUrl"] = HttpContext.Current.Request.Url.AbsoluteUri;
            Response.Redirect("~/BuList.aspx");
        }

        protected void btn_ChangeBu_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/BuList.aspx");
        }

        protected void btn_Logout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }


        protected void LogOut_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            //Session.RemoveAll();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }

    }
}