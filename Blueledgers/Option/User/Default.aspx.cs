using System;
namespace BlueLedger.PL.Option.User
{
    public partial class Default : BaseClass.BasePage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void btn_ChangePassword_Click(object sender, EventArgs e)
        {
            dlgPassword.ChangePassword(LoginInfo.LoginName, "");
        }
    }
}