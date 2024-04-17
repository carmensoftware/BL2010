using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class UserControl_Password : System.Web.UI.UserControl
{
    private readonly Blue.BL.dbo.Bu _bu = new Blue.BL.dbo.Bu();

    private static string sysConnStr = System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString();
    private static bool isResetPassword = false;

    public DateTime GetLastUpdatedPassword(string loginName)
    {
        DataTable dt = _bu.DbExecuteQuery(string.Format(@"SELECT LastUpdatedPassword FROM dbo.[User] WHERE LoginName = '{0}'", loginName), null, sysConnStr);
        if (dt.Rows.Count == 0)
            return DateTime.Now;
        else
            return Convert.ToDateTime(dt.Rows[0][0]);
    }

    public bool IsPasswordExpired(string loginName)
    {
        bool isExpired = false;

        DataTable dt = _bu.DbExecuteQuery(@"SELECT Value FROM dbo.[Config] WHERE [Key] = 'Password.ExpireDays'", null, sysConnStr);
        if (dt != null && dt.Rows.Count > 0)
        {
            int expireDays = 0;

            if (dt.Rows[0][0].ToString() != string.Empty)
                Convert.ToInt32(dt.Rows[0][0]);

            if (expireDays > 0)
            {
                dt = _bu.DbExecuteQuery(string.Format(@"SELECT LastUpdatedPassword FROM dbo.[User] WHERE LoginName = '{0}'", loginName), null, sysConnStr);

                if (dt.Rows.Count > 0)
                {
                    DateTime lastUpdatedPassword;
                    if (dt.Rows[0][0] == DBNull.Value)
                    {
                        _bu.DbExecuteQuery(string.Format(@"UPDATE dbo.[User] SET LastUpdatedPassword = GETDATE() WHERE LoginName = '{0}'", loginName), null, sysConnStr);
                        lastUpdatedPassword = DateTime.Now;
                    }
                    else
                        lastUpdatedPassword = Convert.ToDateTime(dt.Rows[0][0]);

                    int days = Convert.ToInt32((DateTime.Today.Date - lastUpdatedPassword.Date).TotalDays);

                    if (days > expireDays)
                        isExpired = true;
                }
            }
        }


        return isExpired;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_PasswordKey.Visible = isResetPassword;
        txt_PasswordKey.Visible = isResetPassword;
    }

    protected void btn_Save_Click(object sender, EventArgs e)
    {
        string message = string.Empty;
        // Check Password Condition(s)
        message = CheckPasswordCondition(txt_Password.Text);

        if (message == string.Empty)
        {
            UpdatePassword(hf_LoginName.Value, txt_Password.Text);
            pop_Password.ShowOnPageLoad = false;

        }
        else
        {
            lbl_Alert.Text = message;
            pop_Alert.ShowOnPageLoad = true;
            pop_Password.ShowOnPageLoad = true;
        }


    }

    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        pop_Password.ShowOnPageLoad = false;
    }

    protected void btn_Ok_Alert_Click(object sender, EventArgs e)
    {
        pop_Alert.ShowOnPageLoad = false;
    }



    public void ChangePassword(string loginName, string message)
    {
        hf_LoginName.Value = loginName;

        lbl_Message.Text = message;
        isResetPassword = false;

        pop_Password.HeaderText = "Change password for " + loginName;
        pop_Password.ShowOnPageLoad = true;
    }

    public void ResetPassword(string loginName, string message)
    {
        hf_LoginName.Value = loginName;

        lbl_Message.Text = message;
        isResetPassword = true;

        pop_Password.HeaderText = "Reset password for " + loginName;
        pop_Password.ShowOnPageLoad = true;
    }


    private string GetEncryptPassword(string painText)
    {
        return Blue.BL.GnxLib.EnDecryptString(painText, Blue.BL.GnxLib.EnDeCryptor.EnCrypt, Blue.BL.GnxLib.KEY_LOGIN_PASSWORD);
    }


    private string CheckPasswordCondition(string password)
    {
        string messageError = string.Empty;

        if (txt_Password.Text != txt_PasswordConfirm.Text)
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

        }
        else
            messageError = string.Empty;

        return messageError;
    }


    private void UpdatePassword(string loginName, string password)
    {

        string passwordEncrypt = GetEncryptPassword(password);

        string sql = string.Empty;
        sql = "UPDATE dbo.[User] ";
        sql += " SET Password='{0}',";
        sql += "    LastUpdatedPassword='{1}'";
        sql += " WHERE LoginName='{2}'";

        sql = string.Format(sql, @passwordEncrypt, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), @loginName);

        _bu.DbExecuteQuery(@sql, null, sysConnStr);
    }



}