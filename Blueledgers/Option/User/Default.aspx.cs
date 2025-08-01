using System;
using System.Data;
using System.Linq;

namespace BlueLedger.PL.Option.User
{
    public partial class Default : BaseClass.BasePage
    {
        private static string sysConnStr = System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString();

        private readonly Blue.BL.dbo.Bu _bu = new Blue.BL.dbo.Bu();


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


                var text = string.Join(", ", items);

                lbl_Session.Text = text;
            }

            btn_ChangePassword.Visible = LoginInfo.LoginName.ToLower() != "support@carmen";

        }

        // Password

        protected void btn_ChangePassword_Click(object sender, EventArgs e)
        {
            //dlgPassword.ChangePassword(LoginInfo.LoginName, "");
            lbl_ErrorMessage.Text = "";
            pop_Password.ShowOnPageLoad = true;
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            lbl_ErrorMessage.Text = "";

            var pwd1 = txt_Password.Text;
            var pwd2 = txt_PasswordConfirm.Text;

            var error = CheckPasswordCondition(pwd1, pwd2);

            if (!string.IsNullOrEmpty(error))
            {
                lbl_ErrorMessage.Text = error;

                return;
            }


            var pwd = Blue.BL.GnxLib.EnDecryptString(pwd1, Blue.BL.GnxLib.EnDeCryptor.EnCrypt, Blue.BL.GnxLib.KEY_LOGIN_PASSWORD);

            var query = "UPDATE [dbo].[USER] SET [Password]=@Password, LastUpdatedPassword=@Update WHERE LoginName=@LoginName";
            var parameters = new Blue.DAL.DbParameter[]
            {
                new Blue.DAL.DbParameter("@Password", pwd),
                new Blue.DAL.DbParameter("@LoginName", LoginInfo.LoginName),
                new Blue.DAL.DbParameter("@Update", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:fff"))
            };

            _bu.DbExecuteQuery(query, parameters, sysConnStr);

            pop_Password.ShowOnPageLoad = false;

        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            pop_Password.ShowOnPageLoad = false;
        }

        private string CheckPasswordCondition(string pwd1, string pwd2)
        {
            string messageError = string.Empty;

            if (pwd1 != pwd2)
            {
                return "Password and confirm password does not match.";
            }

            DataTable dt = _bu.DbExecuteQuery("SELECT * FROM dbo.[Config]", null);

            if (dt != null)
            {
                int passwordLength = 0;
                bool isComplexity = false;


                foreach (DataRow dr in dt.Rows)
                {
                    string key = dr["Key"].ToString().ToLower();
                    string val = dr["Value"].ToString();

                    if (key == "password.length")
                        passwordLength = Convert.ToInt32(val);
                    else if (key == "password.complexity")
                        isComplexity = val.ToLower() == "true" || val == "1";
                }

                if (pwd1.Length < passwordLength)
                    messageError = string.Format("Password must be of minimum {0} characters length", passwordLength.ToString());

                if (isComplexity)
                {
                    if (pwd1.IndexOfAny("!@#$%^&*?_~-£().,".ToCharArray()) == -1)
                        messageError = "Password must contain at least one symbol";

                    if (!pwd1.Any(c => char.IsDigit(c)))
                        messageError = "Password must contain at least one number";

                    if (!(pwd1.Any(c => char.IsUpper(c)) && pwd1.Any(c => char.IsLower(c))))
                        messageError = "Password must contain lowercase and uppercase";

                }

            }
            else
                messageError = string.Empty;

            return messageError;
        }
    }
}