using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Data.SqlClient;

namespace BlueLedger.PL.Option.User
{
    public partial class ChangePwd : BaseClass.BaseUserControl
    {
        #region Attributes

        private readonly Blue.BL.dbo.User user = new Blue.BL.dbo.User();
        private DataSet dsUser = new DataSet();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsUser = (DataSet)Session["dsUser"];
            }
        }

        private void Page_Retrieve()
        {
            dsUser.Clear();

            // Get User data.
            var getUser = user.Get(dsUser, LoginInfo.LoginName);

            if (!getUser)
            {
                // Display Error Message
            }
            Session["dsUser"] = dsUser;
        }

        protected void bnt_Ok_Click(object sender, EventArgs e)
        {
            var drChkPwd = dsUser.Tables[user.TableName].Rows[0];
            // Check Old Password.
            if (drChkPwd["Password"].ToString() != Blue.BL.GnxLib.EnDecryptString(txt_OldPwd.Text.Trim(), Blue.BL.GnxLib.EnDeCryptor.EnCrypt, Blue.BL.GnxLib.KEY_LOGIN_PASSWORD))
            {
                pop_AlertPwd.ShowOnPageLoad = true;
                txt_Pwd.Text = string.Empty;
                txt_PwdConfirm.Text = string.Empty;
            }
            else if (txt_Pwd.Text == txt_PwdConfirm.Text)
            {
                string messageError = CheckPasswordCondition(txt_Pwd.Text);

                if (messageError == string.Empty)
                {

                    // Update new password
                    dsUser.Tables[user.TableName].Rows[0]["Password"] = Blue.BL.GnxLib.EnDecryptString(txt_Pwd.Text.Trim(),
                        Blue.BL.GnxLib.EnDeCryptor.EnCrypt, Blue.BL.GnxLib.KEY_LOGIN_PASSWORD);

                    // Commit changed to database
                    var saveUser = user.Save(dsUser);

                    if (saveUser)
                    {
                        pop_UpdatedNewPwd.ShowOnPageLoad = true;
                        txt_Pwd.Text = string.Empty;
                        txt_PwdConfirm.Text = string.Empty;
                    }
                }
                else
                {
                    lbl_Alert.Text = messageError;
                    pop_Alert.ShowOnPageLoad = true;

                }
            }
            else
            {
                pop_PwdConfirm.ShowOnPageLoad = true;
            }
        }

        protected void btn_PwdConfirm_OK_Click(object sender, EventArgs e)
        {
            pop_PwdConfirm.ShowOnPageLoad = false;
        }

        protected void bnt_Cancel_Click(object sender, EventArgs e)
        {
            txt_Pwd.Text = string.Empty;
            txt_PwdConfirm.Text = string.Empty;
        }

        protected void btn_Ok_PwdIncorrect_Click(object sender, EventArgs e)
        {
            pop_AlertPwd.ShowOnPageLoad = false;
        }

        protected void btn_OK_NPass_Click(object sender, EventArgs e)
        {
            pop_UpdatedNewPwd.ShowOnPageLoad = false;
        }

        protected void btn_pop_Alert_OK_Click(object sender, EventArgs e)
        {
            pop_Alert.ShowOnPageLoad = false;
        }



        private string CheckPasswordCondition(string password)
        {
            string messageError = string.Empty;

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnStr"].ToString());
            con.Open();
            string sql = "SELECT * FROM dbo.[Config]";
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();


            int passwordLength = 0;
            bool isComplexity = false;

            if (reader.HasRows)
                while (reader.Read())
                {
                    string key = reader["Key"].ToString().ToLower();
                    string val = reader["Value"].ToString();

                    if (key == "password.length")
                        passwordLength = Convert.ToInt32(val);
                    else if (key == "password.complexity")
                        isComplexity = val.ToLower() == "true" || val == "1";
                }

            reader.Close();
            //con.Close();


            if (password.Length < passwordLength)
                messageError = string.Format("Password must be of minimum {0} characters length", passwordLength.ToString());

            if (isComplexity)
            {
                if (password.IndexOfAny("!@#$%^&*?_~-£().,".ToCharArray()) == -1)
                    messageError = "Password must contain at least one symbol";

                if (!password.Any(c => char.IsDigit(c)))
                    messageError = "Password must contain at least one number";

                if (!(password.Any(c => char.IsUpper(c)) && password.Any(c => char.IsLower(c))))
                    messageError = "Password must contain lowercase and uppercase";

            }


            return messageError;
        }

    }
}