using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Admin.Security.User
{
    public partial class User : BasePage
    {
        private readonly string moduleID = "99.98.1.2";
        private readonly Blue.BL.ADMIN.RolePermission _rolePermiss = new Blue.BL.ADMIN.RolePermission();

        private string _connectionString { get { return System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString(); } }

        private string _mode { get { return Request.QueryString["mode"] == null ? "" : Request.QueryString["mode"].ToString(); } }


        // Event(s)
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {

            }



            Control_HeaderMenuBar();
        }

        protected void Control_HeaderMenuBar()
        {
            int pagePermission = _rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);

            if (pagePermission < 5)
            {
                Response.Redirect("~/Option/User/Default.aspx");
            }
        }
    }
}