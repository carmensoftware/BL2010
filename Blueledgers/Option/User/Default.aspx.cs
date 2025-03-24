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

            if (Request.QueryString["debug"] != null)
            {

                var items = new System.Collections.Generic.List<string>();

                foreach (var i in Session.Contents)
                {
                    items.Add(i.ToString());
                }

                //foreach (var item in items)
                //{
                //    if (item != "LoginInfo" && item != "Err" && item != "PreviousPage" && item != "dsUser")
                //    {
                //        Session.Remove(item);
                //    }
                //}

                var text = string.Join(", ", items);

                lbl_Session.Text = text;
            }

        }

        protected void btn_ChangePassword_Click(object sender, EventArgs e)
        {
            dlgPassword.ChangePassword(LoginInfo.LoginName, "");
        }
    }
}