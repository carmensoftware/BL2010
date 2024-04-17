using System;

namespace BlueLedger.PL.Master.In
{
    public partial class MasterPM : System.Web.UI.MasterPage
    {
        /// <summary>
        ///     Gets current login user information.
        /// </summary>
        protected BaseClass.LoginInformation LoginInfo
        {
            get { return (BaseClass.LoginInformation)Session["LoginInfo"]; }
        }

        public string ParentID { get; set; }

        #region "Attributes"

/*
        private Blue.BL.APP.Module module = new Blue.BL.APP.Module();
        private Blue.BL.APP.TransType transType = new Blue.BL.APP.TransType();
*/

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Display Login Information
                lnk_UserName.Text = LoginInfo.LoginName;
                lnk_BuName.Text = LoginInfo.BuInfo.BuName;
            }
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

        protected void imb_Home_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("~/Option/User/Default.aspx");
        }
    }
}