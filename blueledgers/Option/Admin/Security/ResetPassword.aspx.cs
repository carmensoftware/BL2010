using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Text;


public partial class Option_Admin_Security_ResetPassword : System.Web.UI.Page
{
    private readonly Blue.BL.dbo.Bu _bu = new Blue.BL.dbo.Bu();
    private readonly string connString = ConfigurationManager.AppSettings["ConnStr"];

    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void btnReset_Click(object sender, EventArgs e)
    {
        var username = txtUsername.Text.Trim();
        var oldPassword = txtOldPassword.Text.Trim();


        var query = "SELECT [PasswordKey] FROM [dbo].[User] WHERE LoginName=@LoginName";


        //string email = txtEmail.Text.Trim();
        //string newPassword = txtNewPassword.Text;
        //string confirmPassword = txtConfirmPassword.Text;

        //// ตรวจสอบความถูกต้องเบื้องต้น
        //if (newPassword != confirmPassword)
        //{
        //    lblMessage.Text = "รหัสผ่านและยืนยันรหัสผ่านไม่ตรงกัน";
        //    return;
        //}

        //// เข้ารหัสผ่านก่อนบันทึกลงฐานข้อมูล (แนะนำ)
        //string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword, "SHA1");

        //// บันทึกลงฐานข้อมูล
        //using (SqlConnection conn = new SqlConnection(connString))
        //{
        //    // ตัวอย่าง Query: UPDATE Users SET Password = @Password WHERE Email = @Email
        //    string query = "UPDATE Users SET Password = @Password WHERE Email = @Email";
        //    using (SqlCommand cmd = new SqlCommand(query, conn))
        //    {
        //        cmd.Parameters.AddWithValue("@Password", hashedPassword);
        //        cmd.Parameters.AddWithValue("@Email", email);

        //        try
        //        {
        //            conn.Open();
        //            int rowsAffected = cmd.ExecuteNonQuery();

        //            if (rowsAffected > 0)
        //            {
        //                lblMessage.ForeColor = System.Drawing.Color.Green;
        //                lblMessage.Text = "รีเซ็ตรหัสผ่านสำเร็จ!";
        //            }
        //            else
        //            {
        //                lblMessage.Text = "ไม่พบอีเมลนี้ในระบบ";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            lblMessage.Text = "เกิดข้อผิดพลาด: " + ex.Message;
        //        }
        //    }
        //}
    }

    #region -- function --

    public DateTime? GetLastUpdatedPassword(string loginName)
    {
        Nullable<DateTime> updatedDate = null;

        var dt = _bu.DbExecuteQuery("SELECT LastUpdatedPassword FROM dbo.[User] WHERE LoginName=@LoginName", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@LoginName", loginName) }, connString);

        if (dt != null && dt.Rows.Count > 0)
        {
            var value = dt.Rows[0][0].ToString();

            if (!string.IsNullOrEmpty(value))
                updatedDate = Convert.ToDateTime(value);
        }


        return updatedDate;
    }

    public bool IsPasswordExpired(string loginName)
    {
        bool isExpired = false;

        var query = @"
DECLARE @Value nvarchar(100) = (SELECT Value FROM dbo.[Config] WHERE [Key] = 'Password.ExpireDays')
DECLARE @ExpDays INT = CASE WHEN ISNULL(@value,'')='' THEN 0 ELSE CAST(@Value AS INT) END
DECLARE @Overdue INT =0
DECLARE @LastUpdatedPassword DATE = (SELECT LastUpdatedPassword FROM [dbo].[User] WHERE LoginName=@LoginName)

IF @ExpDays > 0
BEGIN
	IF @LastUpdatedPassword IS NULL
	BEGIN
		UPDATE [dbo].[User] SET LastUpdatedPassword=GETDATE() WHERE LoginName=@LoginName
		SET @LastUpdatedPassword=GETDATE()
	END

	SET @Overdue = DATEDIFF(Day, @LastUpdatedPassword, GETDATE())
END

SELECT ISNULL(@Overdue,0) as Overdue";
        var dt = _bu.DbExecuteQuery(query, new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@LoginName", loginName) }, connString);

        if (dt != null && dt.Rows.Count > 0)
        {
            var overdue = Convert.ToInt32(dt.Rows[0][0]);

            isExpired = overdue > 0;
        }

        return isExpired;
    }

    public string CheckPasswordPolicy(string loginName, string password, string password2)
    {
        if (password != password2)
        {
            return "Password and confirm password does not match.";
        }

        // No same as old password
        var dtSamePwd = _bu.DbExecuteQuery(
            "SELECT CASE WHEN [Password]=@Password THEN 1 ELSE 0 END FROM dbo.[User] WHERE LoginName=@LoginName",
            new Blue.DAL.DbParameter[] 
            { 
                new Blue.DAL.DbParameter("@Password", GetEncryptPassword(password)), 
                new Blue.DAL.DbParameter("@LoginName", loginName) 
            },
            connString);

        if (dtSamePwd != null && dtSamePwd.Rows.Count > 0)
        {
            var value = dtSamePwd.Rows[0][0].ToString();

            if (value == "1")
            {
                return "New password cannot match previous passwords.";
            }
        }


        //var dt = _bu.DbExecuteQuery("SELECT [Key], [Value] FROM dbo.[Config]", null);
        //var policies = dt.AsEnumerable()
        //    .Select(x => new KeyValue
        //    {
        //        Value = x.Field<string>("Value"),
        //        Key = x.Field<string>("Key"),
        //    })
        //    .ToArray();

        //var policy_Length = policies.FirstOrDefault(x => x.Key.ToLower() == "password.length").Value;
        //var policy_Complexity = policies.FirstOrDefault(x => x.Key.ToLower() == "password.complexity").Value;
        ////var policy_ExpDays = policies.FirstOrDefault(x => x.Key.ToLower() == "password.expiredays").Value;


        //var pwdLength = string.IsNullOrEmpty(policy_Length) ? 6 : Convert.ToInt32(policy_Length);
        //var pwdComplex = string.IsNullOrEmpty(policy_Complexity) ? false : policy_Complexity == "1";
        ////var pwdExpDays = string.IsNullOrEmpty(policy_ExpDays) ? 0 : Convert.ToInt32(policy_ExpDays);
        var policy = GetPasswordPolicy();


        if (password.Length < policy.Length)
        {
            return string.Format("Minimum {0} characters required.", policy.Length);
        }

        if (policy.IsComplexity)
        {
            return ValidateComplexityPassword(password);
        }



        return "";
    }

    #endregion


    // Method(s)
    private PasswordPolicy GetPasswordPolicy()
    {
        var dt = _bu.DbExecuteQuery("SELECT [Key], [Value] FROM dbo.[Config]", null);
        var policies = dt.AsEnumerable()
            .Select(x => new KeyValue
            {
                Value = x.Field<string>("Value"),
                Key = x.Field<string>("Key"),
            })
            .ToArray();

        var policy_Length = policies.FirstOrDefault(x => x.Key.ToLower() == "password.length").Value;
        var policy_Complexity = policies.FirstOrDefault(x => x.Key.ToLower() == "password.complexity").Value;
        var policy_ExpDays = policies.FirstOrDefault(x => x.Key.ToLower() == "password.expiredays").Value;


        var pwdLength = string.IsNullOrEmpty(policy_Length) ? 6 : Convert.ToInt32(policy_Length);
        var pwdComplex = string.IsNullOrEmpty(policy_Complexity) ? false : policy_Complexity == "1";
        var pwdExpDays = string.IsNullOrEmpty(policy_ExpDays) ? 0 : Convert.ToInt32(policy_ExpDays);


        return new PasswordPolicy
        {
            Length = pwdLength,
            IsComplexity = pwdComplex,
            ExpiredDays = pwdExpDays
        };
    }

    private string GetEncryptPassword(string painText)
    {
        return Blue.BL.GnxLib.EnDecryptString(painText, Blue.BL.GnxLib.EnDeCryptor.EnCrypt, Blue.BL.GnxLib.KEY_LOGIN_PASSWORD);
    }

    private bool IsStrongPassword(string password)
    {
        // เงื่อนไข: ยาว 8+, มีพิมพ์ใหญ่, พิมพ์เล็ก, ตัวเลข, และอักขระพิเศษอย่างน้อย 1 ตัว
        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
        return System.Text.RegularExpressions.Regex.IsMatch(password, pattern);
    }

    private string ValidateComplexityPassword(string password)
    {
        var errors = new StringBuilder();

        if (!password.Any(char.IsUpper))
            errors.Append("Missing uppercase letter.<br/>");

        if (!password.Any(char.IsLower))
            errors.AppendLine("Missing lowercase letter.<br/>");

        if (!password.Any(char.IsDigit))
            errors.AppendLine("Missing number.<br/>");

        if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            errors.AppendLine("Missing special character such as @ # $ % ^ & * - .<br/>");

        return errors.ToString().Trim(); ;
    }

    private void UpdatePassword(string loginName, string password)
    {
        var passwordEncrypt = GetEncryptPassword(password);


        var sql = "UPDATE [dbo].[User] SET [Password]=@Password, LastUpdatedPassword=GETDATE() WHERE LoginName=@LoginName";

        _bu.DbExecuteQuery(
            sql,
            new Blue.DAL.DbParameter[]
            {
                new Blue.DAL.DbParameter("@Password", passwordEncrypt),
                new Blue.DAL.DbParameter("@LoginName", loginName)
            },
            connString);
    }

    [Serializable]
    internal class PasswordPolicy
    {
        public int Length { get; set; }
        public bool IsComplexity { get; set; }
        public int ExpiredDays { get; set; }
    }

    [Serializable]
    protected class KeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}