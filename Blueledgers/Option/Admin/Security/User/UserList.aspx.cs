using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Data.SqlClient;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Web.ASPxEditors;
using System.Text;
using System.Text.RegularExpressions;


namespace BlueLedger.PL.Option.Admin.Security.User
{
    public partial class UserList : BasePage
    {
        private readonly string moduleID = "99.98.1.2";
        
        private readonly Blue.BL.dbo.Bu _bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.dbo.User _user = new Blue.BL.dbo.User();

        private readonly Blue.BL.Option.Admin.Security.UserRole userRole = new Blue.BL.Option.Admin.Security.UserRole();

        private readonly Blue.BL.ADMIN.RolePermission _rolePermiss = new Blue.BL.ADMIN.RolePermission();

        #region "Declaration"


        private string _status { get { return Request.QueryString["status"] == null ? "" : Request.QueryString["status"].ToString(); } }

        private string _buCode { get { return Request.QueryString["bu"] == null ? "" : Request.QueryString["bu"].ToString(); } }

        private string _connectionString { get { return System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString(); } }

        protected DataTable _dtBu
        {
            get { return ViewState["_dtBu"] as DataTable; }
            set { ViewState["_dtBu"] = value; }
        }

        protected DataTable _dtBusinessUnit
        {
            get { return ViewState["_dtBusinessUnit"] as DataTable; }
            set { ViewState["_dtBusinessUnit"] = value; }
        }

        [Serializable]
        protected class PasswordPolicy
        {
            public int Length { get; set; }
            public bool Complexity { get; set; }
        }

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                _dtBusinessUnit = GetBusinessUnit();
                LoadUserList(_status, _buCode);
            }

            Control_HeaderMenuBar();
        }

        protected void Control_HeaderMenuBar()
        {
            int pagePermission = _rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);

            if (LoginInfo.LoginName == "support@carmen")
            {
                pagePermission = 7;
            }


            if (pagePermission < 3)
            {
                Response.Redirect("~/Option/User/Default.aspx");
            }
            else
            {
                btn_Create.Visible = (pagePermission >= 3) ? btn_Create.Visible : false;
            }



        }

        // Event(s)

        protected void btn_Create_Click(object sender, EventArgs e)
        {
            txt_NewUsername.Text = "";
            txt_NewPassword.Text = "";

            pop_NewUser.ShowOnPageLoad = true;
        }

        protected void btn_NewUserCreate_Click(object sender, EventArgs e)
        {
            var loginName = txt_NewUsername.Text.Trim();
            var pwd = txt_NewPassword.Text.Trim();
            //var email = txt_NewEmail.Text.Trim();
            var email = "";

            if (string.IsNullOrEmpty(loginName))
            {
                ShowAlert("Username is required.");
                return;
            }

            var error = CheckPassword(pwd, pwd);

            if (error != "")
            {
                ShowAlert(error);

                return;
            }


            var sql = new Helpers.SQL(_connectionString);

            var dtFound = sql.ExecuteQuery("SELECT TOP(1) LoginName FROM [dbo].[User] WHERE LoginName=@LoginName", new SqlParameter[] { new SqlParameter("@LoginName", loginName) });

            if (dtFound != null && dtFound.Rows.Count > 0)
            {
                ShowAlert(string.Format("This username '{0}' is already used.", loginName));

                return;
            }

            var password = Blue.BL.GnxLib.EnDecryptString(pwd, Blue.BL.GnxLib.EnDeCryptor.EnCrypt, Blue.BL.GnxLib.KEY_LOGIN_PASSWORD);
            var query = "INSERT INTO [dbo].[User]([LoginName], [Password], [FName], [Email], [IsActived]) VALUES (@LoginName, @Password, @FName, @Email, 0)";

            sql.ExecuteQuery(query, new SqlParameter[]
            {
                new SqlParameter("@LoginName",loginName),
                new SqlParameter("@Password", password),
                new SqlParameter("@FName", loginName),
                new SqlParameter("@Email",email),
            });

            pop_NewUser.ShowOnPageLoad = false;
            ShowUserProfile(loginName, true);
        }

        protected void btn_Print_Click(object sender, EventArgs e)
        {
        }

        protected void gv_User_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("img_Status") != null)
                {
                    var item = e.Row.FindControl("img_Status") as Image;
                    var active = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAACXBIWXMAABZ/AAAWfwGkE7q/AAAAGXRFWHRTb2Z0d2FyZQB3d3cuaW5rc2NhcGUub3Jnm+48GgAAAnNJREFUWIXt101oVFcYh/HfmRSF0l2pEHXRbqrtqip+JKJFkkniQhQJARGRLty7NwuRbN1m5UbFdiOBrNRkUGtKlYIWV35AaRd+gKJWXAh+3OPizjj3JpnJnZnMSv/Le95znufeuXPOe/nUE1qqnrVBYq9gB3qxpjryEI9Fc0qmlf29fAJRULFfNIH1Bde9Kxg3aEoQ2xe45DucE/QVBOcTXcdBw/5tXWDGdkxhVVvwep6Jxgy7XFxgxs+YwYoO4bW8wZAhvy8tcMG3evyFb5YJXsszPbYa8E/2YilXEgU9fusCHL72zlkxf9N5gVmj2NYFeJqgT8X+xQVSsxNdg9c5E9mnUBeo+Enx/3mjvBDs8pUvRb82qFlfZc0TSOztGJ4YVHZVv9eC0w0rM6y6QMnOjuEjboHjSjjSsDrDyr6Eq5sAXovO4HYheL9TGG24Wvx4huRewt4mE44adtj/toimC8B/aXIzZG621KzqY4JXYMwbL41VJdqF51IXCB43qTup4vucxHt9HcAfLRTIXFwkvRJXchK73WsTTvBwoUA0t8S01RLXVPyYmRP0m2wJDolrdZdaLtqo5GaB6Y8k9ljpvrcmcagleCqwqfbz1QXSzueOaF3BZaJWW7qUeM+gH2qdUvYljBLjLS3VXo5l27T8IlEw60/dOhGj64Zszwrk94Egeu8AnnYB/9wXDs1vUhduRLv9JxiTtlHLlbcSo/O7ocUFoOyqYABPlgH+XDRixJXFBhtvxWV/KOnDjQ7gN5RsbtQRU/TDJG3VJqjuhEvnPsaVne/sw2R+0s1qH3ZIT7S11ZEH0q18TjRtyK2lwJ9Tywfmj7RnYfikdAAAAABJRU5ErkJggg==";
                    var inactive = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAAsQAAALEBxi1JjQAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAO4SURBVFiFvZdLTFxlFMd/594LOMwAjS/AtEVNN5poiQYM6WYEFcpjIYsubOLKaNImfSVSNbEaF6alESxJXdi13ZjYBcXSSUA0gaYPom2qG1EibVJpFVs7U2Dm3u+4GB4zMJ17Qeh/de/Muef3/17n+z4hoG5t21YSKkq1qlAvsBXlSYQNACi3ERlX5TLC4MyM0/fY8PDdIHnFLyDR+GKl8exDorwJFAf0e0/hpPHsI2VD58ZWZUDr6kLxYvO+oAeAcEDwUqUQusIFUx/JmbHZwAbuROu22Lb7DchzqwRnSWFEkPbIwPlJXwPxV2uqMXIWeHwt4Bm6prZpLoldunpfA+mWe8PrAJ/XBEhtZk9Y8w8ajT5k297X6wgH2KzoKd2+pWiZgbg9/QFQvY5wAATqErOPfJzxnl5q6tq/svrZvlIlXfWe3TA4+psFYDz7kC+84glkY5VvZtlUBeWVfmGFNs5BAPl7+0ulRUm9Qb4iU15J4YmvwHFwj3Vizp7OGWY1tuLs7YCUS/LtnTB5I5+JxD2vuMIqmtWWvHBACgrBcUAsnL0dWE1ty+FNbWm4WOA4SEFBvpQA4ZCTaLZUqPeL1Ot/4H5+BNSkTezpwG5tX4S/1oKz5900XBXvi270+oRfWlAaHIGt/pFgYn24gLPvIIiFvWt/OkdyduE3VPGOd+H1nQqSEpDnJd5Qewt4NOAX6a5eaK2Zy5N+dnuOYvp7g6YCuGkBpSv5wvT3Zg3HYsu7VwoHKLP8YwJKfHf2nLKAf1f0QVNbxpibuZ4Q7F37cq4OH92xgPHA8KWz/Xj38tXR8npgusLvjqr8JKI1geD3me1Zq2P3AYCgK+GyhTDoFyWbqrK63e3pzAKYWB9uz9HF4di9P1DZRhmwpr3QaYF43rhkElIuGJMuxTlmu+nvxT3WmTaRctFUzhNYphLTpviMANxtqD0h8Fbe8PIKpKDQt8LJxs1oKgmTf+aNU/TLkoGL7wgsnIR+AXwL+Bpp1rZ5JhS7MG4BlA2dG0PoekBwFD4LxS6MQ8aJKPyP9yHwwwOAj0QKpz6Zf18wIKOjKZAdwLV15E8I0p55R8gqxZGB85Nqm+Z1MjGhRpuX3g2W7QUlsUtXjee9gPL9WpEVRkBqS767+PPS/3JuRqVDo3+Fi6YaUTkMJP8HO6nwaeThxMu5bkUQ4HI680rN0yljvSeibxDw1CwQN+hJx5bD87M9T2ww3YxGIyEn0YxKvQjVKE/B3PUcbiOMK/yIYTDizHwrsSuJIHn/A/NYeWJNjN7eAAAAAElFTkSuQmCC";
                    var icon = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsActived")) ? active : inactive;


                    item.ImageUrl = icon;
                }
            }
        }

        protected void btn_View_Click(object sender, EventArgs e)
        {
            var btn = sender as ImageButton;
            var loginName = btn.CommandArgument.ToString();

            ShowUserProfile(loginName);
        }

        #region -- User--

        protected void btn_EditUser_Click(object sender, EventArgs e)
        {
            SetUserEditMode(true);
        }

        protected void btn_DelUser_Click(object sender, EventArgs e)
        {
            var loginName = lbl_LoginName.Text;
            var name = txt_FirstName.Text + " " + txt_MidName.Text.Trim() + " " + txt_LastName.Text.Trim();

            lbl_UserConfirmDelete.Text = string.Format("Do you want to delete this user <b>'{0}'</b>?<br/><br/><center><b>{1}</b></center>", loginName, name);

            pop_UserConfirmDelete.ShowOnPageLoad = true;
        }

        protected void btn_UserConfirm_Yes_Click(object sender, EventArgs e)
        {
            var loginName = lbl_LoginName.Text;

            DeleteUser(loginName);
            pop_UserConfirmDelete.ShowOnPageLoad = false;
            pop_User.ShowOnPageLoad = false;

            Response.Redirect("UserList.aspx");
        }

        protected void btn_SaveUser_Click(object sender, EventArgs e)
        {
            var loginName = lbl_LoginName.Text.Trim();

            SaveUser(loginName);
            GetUserProfile(loginName);
            SetUserEditMode(false);
        }

        protected void btn_CancelUser_Click(object sender, EventArgs e)
        {
            var loginName = lbl_LoginName.Text.Trim();

            GetUserProfile(loginName);

            SetUserEditMode(false);
        }

        protected void btn_ChangePassword_Click(object sender, EventArgs e)
        {
            var passwordPolicy = GetPasswordPolicy();

            var length = passwordPolicy.Length;
            var isComplex = passwordPolicy.Complexity;

            lbl_PwdLength.Text = string.Format("*The password must contain at least {0} character(s).", length);
            if (isComplex)
                lbl_PwdComplexity.Text = string.Format("*The password must be a stronger password with a mix of letters, numbers, and symbols.", length);
            else
                lbl_PwdComplexity.Text = "";



            pop_ChangePassword.ShowOnPageLoad = true;
        }

        protected void btn_ChangePassword_Yes_Click(object sender, EventArgs e)
        {
            var pwd = txt_ChangePassword.Text.Trim();
            var pwd2 = txt_ChangePasswordConfirm.Text.Trim();

            //if (string.IsNullOrEmpty(pwd))
            //{
            //    ShowAlert("Password is required.");

            //    return;
            //}

            //if (pwd != pwd2)
            //{
            //    ShowAlert("Password does not match.");

            //    return;
            //}

            //var length = Convert.ToInt32(hf_PwdLength.Value);
            //var isComplex = Convert.ToBoolean(hf_PwdComplexity.Value);

            //// length
            //if (pwd.Length < length)
            //{
            //    ShowAlert(string.Format("The password you entered does not meet the required length.<br/><small>*Minimum {0} characters.</small>", length));

            //    return;
            //}

            //// Complexity
            //if (isComplex)
            //{
            //    var isStrong = IsStrongPassword(pwd, length);

            //    if (!isStrong)
            //    {
            //        ShowAlert("Please choose a stronger password. Try a mix of letters, numbers, and symbols.");

            //        return;
            //    }
            //}

            var error = CheckPassword(pwd, pwd2);

            if (error != "")
            {
                ShowAlert(error);

                return;
            }

            var password = Blue.BL.GnxLib.EnDecryptString(pwd, Blue.BL.GnxLib.EnDeCryptor.EnCrypt, Blue.BL.GnxLib.KEY_LOGIN_PASSWORD);

            var loginName = lbl_LoginName.Text;

            var dt = new Helpers.SQL(_connectionString).ExecuteQuery("UPDATE [dbo].[User] SET [Password]=@Password WHERE LoginName=@LoginName",
                new SqlParameter[]
                {
                    new SqlParameter("@Password", password),
                    new SqlParameter("@LoginName", loginName)
                });


            pop_ChangePassword.ShowOnPageLoad = false;
        }

        protected void btn_AddBu_Click(object sender, EventArgs e)
        {
            var selectedBu = new List<string>();

            foreach (ListEditItem item in list_Bu.Items)
            {
                selectedBu.Add(item.Value.ToString());
            }

            var items = new List<ListEditItem>();
            if (selectedBu.Count > 0)
            {
                items = _dtBusinessUnit.AsEnumerable()
                    .Where(x => !selectedBu.Contains(x.Field<string>("BuCode")))
                    .Select(x => new ListEditItem
                    {
                        Value = x.Field<string>("BuCode"),
                        Text = string.Format("{0} ({1})", x.Field<string>("BuName"), x.Field<string>("BuCode")),
                    })
                    .ToList();

            }
            else
            {
                items = _dtBusinessUnit.AsEnumerable()
                    .Select(x => new ListEditItem
                    {
                        Value = x.Field<string>("BuCode"),
                        Text = string.Format("{0} ({1})", x.Field<string>("BuName"), x.Field<string>("BuCode")),
                    })
                    .ToList();
            }

            list_AddBu.Items.Clear();
            list_AddBu.Items.AddRange(items);


            pop_AddBu.ShowOnPageLoad = true;
        }

        protected void btn_AddBuSeleted_Click(object sender, EventArgs e)
        {
            var loginName = lbl_LoginName.Text;
            var selectedBu = new List<string>();

            foreach (ListEditItem item in list_AddBu.Items)
            {
                if (item.Selected)
                {
                    selectedBu.Add(item.Value.ToString());
                }
            }

            if (selectedBu.Count > 0)
            {
                foreach (var buCode in selectedBu)
                {
                    new Helpers.SQL(_connectionString).ExecuteQuery("INSERT INTO [dbo].[BuUser] (BuCode, LoginName, Theme, DispLang) VALUES (@BuCode, @LoginName, 'Default', 'en-US')",
                        new SqlParameter[]
                    {
                        new SqlParameter("@BuCode", buCode),
                        new SqlParameter("@LoginName", loginName)
                    });
                }
                ShowUserProfile(loginName);
            }

            pop_AddBu.ShowOnPageLoad = false;

        }

        protected void btn_DelBu_Click(object sender, EventArgs e)
        {
            var buName = list_Bu.SelectedItem.Text;
            var loginName = lbl_LoginName.Text;

            lbl_BuConfirmDelete.Text = string.Format("Do you want to delete this user '{0}' from '{1}'?", loginName, buName);
            pop_BuConfirmDelete.ShowOnPageLoad = true;
        }

        protected void btn_BuConfirm_Yes_Click(object sender, EventArgs e)
        {
            var buCode = list_Bu.SelectedItem.Value.ToString();
            var loginName = lbl_LoginName.Text;

            DeleteBuUser(buCode, loginName);

            ShowUserProfile(loginName);

        }

        protected void list_Bu_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = sender as ASPxListBox;

            if (item.SelectedItem == null)
                return;

            panel_BuUser.Visible = false;
            btn_DelBu.Enabled = false;

            if (item.SelectedItem.Value != null)
            {
                var buCode = item.SelectedItem.Value.ToString();
                var loginName = lbl_LoginName.Text;

                panel_BuUser.Visible = true;
                btn_DelBu.Enabled = true;

                lbl_SelectedBu.Text = item.SelectedItem.Text;

                GetBuUser(buCode, loginName);

                SetBuEditMode(false);
            }
        }

        protected void btn_BuEdit_Click(object sender, EventArgs e)
        {
            SetBuEditMode(true);
        }

        protected void btn_BuSave_Click(object sender, EventArgs e)
        {
            var loginName = lbl_LoginName.Text;
            var buCode = list_Bu.SelectedItem.Value.ToString();

            SaveBuUser(buCode, loginName);
            SetBuEditMode(false);
        }

        protected void btn_BuCancel_Click(object sender, EventArgs e)
        {
            var loginName = lbl_LoginName.Text;
            var buCode = list_Bu.SelectedItem.Value.ToString();

            GetBuUser(buCode, loginName);
            SetBuEditMode(false);
        }

        protected void btn_RoleSelAll_Click(object sender, EventArgs e)
        {
            foreach (ListEditItem item in list_Role.Items)
            {
                item.Selected = true;
            }
        }

        protected void btn_RoleSelNone_Click(object sender, EventArgs e)
        {
            foreach (ListEditItem item in list_Role.Items)
            {
                item.Selected = false;
            }

        }

        protected void btn_LocationSelAll_Click(object sender, EventArgs e)
        {
            foreach (ListEditItem item in list_Location.Items)
            {
                item.Selected = true;
            }

        }

        protected void btn_LocationSelNone_Click(object sender, EventArgs e)
        {
            foreach (ListEditItem item in list_Location.Items)
            {
                item.Selected = false;
            }
        }


        #endregion


        // Method(s)
        private void ShowAlert(string text)
        {
            lbl_Alert.Text = text;
            pop_Alert.ShowOnPageLoad = true;
        }


        private bool IsValidUsername(string username)
        {
            // Example pattern:
            // - Starts with a letter (uppercase or lowercase)
            // - Contains letters, numbers, or underscores
            // - Has a length between 6 and 12 characters
            string pattern = @"^[a-zA-Z][a-zA-Z0-9_]{5,11}$";

            Regex regex = new Regex(pattern);

            return regex.IsMatch(username);
        }

        private bool IsStrongPassword(string password, int length)
        {
            // This regex requires:
            // - At least 8 characters long (.{8,})
            // - At least one uppercase letter (?=.*[A-Z])
            // - At least one lowercase letter (?=.*[a-z])
            // - At least one digit (?=.*\d)
            // - At least one special character from the set @$!%*?& (?=.*[@$!%*?&])
            //string passwordRegex = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$";
            string passwordRegex = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&]).{" + length.ToString() + @",}$";

            return Regex.IsMatch(password, passwordRegex);
        }

        private string CheckPassword(string password, string passwordConfirm)
        {
            var pwd = password;
            var pwd2 = passwordConfirm;

            if (string.IsNullOrEmpty(pwd))
            {
                return "Password is required.";
            }

            if (pwd != pwd2)
            {
                return "Password does not match.";
            }

            var passwordPolicy = GetPasswordPolicy();

            var length = passwordPolicy.Length;
            var isComplex = passwordPolicy.Complexity;

            // length
            if (pwd.Length < length)
            {
                return string.Format("The password you entered does not meet the required length.<br/><small>*Minimum {0} characters.</small>", length);
            }

            // Complexity
            if (isComplex && !IsStrongPassword(pwd, length))
            {
                return "Please choose a stronger password. Try a mix of letters, numbers, and symbols.";
            }

            return "";
        }

        private PasswordPolicy GetPasswordPolicy()
        {
            var dt = new Helpers.SQL(_connectionString).ExecuteQuery("SELECT LOWER([Key]) as [Key], LTRIM(RTRIM([Value])) as [Value] FROM [dbo].[Config]");
            var configItems = dt.AsEnumerable().Select(x => new ListEditItem { Text = x.Field<string>("Key"), Value = x.Field<string>("Value") }).ToArray();

            var pwdLength = configItems.Where(x => x.Text == "password.length").FirstOrDefault();
            var pwdComplex = configItems.Where(x => x.Text == "password.complexity").FirstOrDefault();
            //var pwdSymbol = configItems.Where(x => x.Text == "password.symbols").FirstOrDefault();
            //var pwdNumber = configItems.Where(x => x.Text == "password.numbers").FirstOrDefault();
            //var pwdLowerCase = configItems.Where(x => x.Text == "password.lowercase").FirstOrDefault();
            //var pwdUpperCase = configItems.Where(x => x.Text == "password.uppercase").FirstOrDefault();

            var length = pwdLength != null && !string.IsNullOrEmpty(pwdLength.Value.ToString()) ? Convert.ToInt32(pwdLength.Value) : 0;
            var isComplex = pwdComplex.Value.ToString() == "1";

            var policy = new PasswordPolicy { Length = length, Complexity = isComplex };

            return policy;
        }

        private DataTable GetBusinessUnit()
        {
            var sql = new Helpers.SQL(_connectionString);
            var query = @"SELECT BuCode, BuName, 1 as Selected FROM dbo.Bu WHERE IsActived=1 ORDER BY BuName";

            return sql.ExecuteQuery(query);
        }

        private DataRow GetUserInfo(string loginName)
        {
            var sql = new Helpers.SQL(_connectionString);
            var query = @"
SELECT 
    *
FROM 
	[dbo].[User]
WHERE
    LoginName=@LoginName
ORDER BY
    IsActived DESC,
    LoginName";
            var dt = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@LoginName", loginName) });

            _dtBu = new DataTable();

            #region --Query: BusinessUnit--
            query = @"
;WITH
bu AS(
	SELECT  
		b.BuCode,
		b.BuName,
		CASE WHEN LoginName IS NULL THEN 0 ELSE 1 END as Selected
	
	FROM 
		[dbo].Bu b
		LEFT JOIN [dbo].BuUser bu
			ON b.BuCode=bu.BuCode AND bu.LoginName = @loginName 
	WHERE 
		b.IsActived=1
)
SELECT
	*
FROM
	bu
ORDER BY
	Selected DESC,
	BuName";
            #endregion

            _dtBu = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@LoginName", loginName) });


            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        private void LoadUserList(string status = "", string buCode = "")
        {
            var sql = new Helpers.SQL(_connectionString);
            var dt = new DataTable();

            if (string.IsNullOrEmpty(buCode))
            {
                var query = @"
SELECT 
    Row_Number() OVER(ORDER BY LoginName) as RowId,
	LoginName, 
	CONCAT(ISNULL(FName,''),' ',ISNULL(MName,''),ISNULL(LName,'')) as FullName,
	Email,
	JobTitle,
	IsActived,
	LastLogin
FROM 
	[dbo].[User]
WHERE
    LoginName <> 'support@carmen'
    AND IsActived = CASE @status WHEN 'active' THEN 1 WHEN 'inactive' THEN 0 ELSE IsActived END
ORDER BY
    LoginName";

                dt = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@status", status) });

            }
            else
            {
                var query = @"
SELECT 
    Row_Number() OVER(ORDER BY IsActived DESC, u.LoginName) as RowId,
	u.LoginName, 
	CONCAT(ISNULL(FName,''),' ',ISNULL(MName,''),ISNULL(LName,'')) as FullName,
	Email,
	JobTitle,
	IsActived,
	LastLogin
FROM 
	[dbo].[User] u
    JOIN [dbo].BuUser bu ON bu.LoginName=u.LoginName
WHERE
    u.LoginName <> 'support@carmen'
    AND IsActived = CASE @status WHEN 'active' THEN 1 WHEN 'inactive' THEN 0 ELSE IsActived END
    AND bu.BuCode = CASE WHEN ISNULL(@buCode,'') = '' THEN bu.BuCode ELSE @buCode END
ORDER BY
    IsActived DESC,
    u.LoginName";

                dt = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@status", status), new SqlParameter("@buCode", buCode) });

            }

            gv_User.DataSource = dt;
            gv_User.DataBind();

            var dtUser = sql.ExecuteQuery("SELECT COUNT(LoginName) FROM [dbo].[User] WHERE IsActived=1 AND LoginName <> 'support@carmen'");

            var users = Convert.ToInt32(dtUser.Rows[0][0]);
            var license = _user.GetActiveUserLicense();

            //var all = dt.AsEnumerable()
            //    //.Where(x => x.Field<string>("LoginName") != username)
            //    .Count();
            //var active = dt.AsEnumerable()
            //    //.Where(x => x.Field<string>("LoginName") != username)
            //    .Where(x => x.Field<bool>("IsActived") == true)
            //    .Count();
            //var inactive = dt.AsEnumerable()
            //    //.Where(x => x.Field<string>("LoginName") != username)
            //    .Where(x => x.Field<bool>("IsActived") == false)
            //    .Count();


            //lbl_UserCount.Text = string.Format("<b>License</b>: {0}/{1} | <b>Active</b>: {2} | <b>Inactive</b>: {3}", users, license, active, inactive);
            lbl_UserCount.Text = string.Format("<b>License</b>: {0}/{1}", users, license);

        }

        #region --BU User--

        private void DeleteUser(string loginName)
        {
            var dtBuUser = new Helpers.SQL(_connectionString).ExecuteQuery("SELECT BuCode FROM [dbo].[BuUser] WHERE LoginName=@LoginName", new SqlParameter[] { new SqlParameter("@LoginName", loginName) });
            var buUsers = dtBuUser.AsEnumerable().Select(x => x.Field<string>("BuCode").Trim()).ToArray();

            foreach (var buCode in buUsers)
            {
                DeleteBuUser(buCode, loginName);
            }

            // Delete from AdminDB
            new Helpers.SQL(_connectionString).ExecuteQuery("DELETE FROM [dbo].[User] WHERE LoginName=@LoginName", new SqlParameter[] { new SqlParameter("@LoginName", loginName) });
        }

        private void ShowUserProfile(string loginName, bool isEdit = false)
        {
            GetUserProfile(loginName);
            SetUserEditMode(isEdit);
            pop_User.ShowOnPageLoad = true;
        }

        private void SetUserEditMode(bool isEdit)
        {
            //txt_LoginName.ReadOnly = !isEdit;
            chk_IsActive.Enabled = isEdit;

            txt_FirstName.ReadOnly = !isEdit;
            txt_MidName.ReadOnly = !isEdit;
            txt_LastName.ReadOnly = !isEdit;

            txt_Email.ReadOnly = !isEdit;
            txt_JobTitle.ReadOnly = !isEdit;

            btn_EditUser.Visible = !isEdit;
            btn_SaveUser.Visible = isEdit;
            btn_CacelUser.Visible = isEdit;

            btn_ChangePassword.Visible = !isEdit;
            btn_DelUser.Visible = !isEdit;


            btn_AddBu.Enabled = !isEdit;
            list_Bu.Enabled = !isEdit;
            panel_BuUser.Visible = !isEdit && list_Bu.SelectedItem != null;

        }

        private void SetBuEditMode(bool isEdit)
        {
            btn_DelBu.Visible = !isEdit;
            btn_BuEdit.Visible = !isEdit;
            btn_BuSave.Visible = isEdit;
            btn_BuCancel.Visible = isEdit;

            list_Bu.Enabled = !isEdit;

            ddl_Department.Enabled = isEdit;

            btn_RoleSelAll.Visible = isEdit;
            btn_RoleSelNone.Visible = isEdit;
            list_Role.Enabled = isEdit;

            btn_LocationSelAll.Visible = isEdit;
            btn_LocationSelNone.Visible = isEdit;
            list_Location.Enabled = isEdit;



            btn_AddBu.Enabled = !isEdit;

            btn_EditUser.Visible = !isEdit;
            btn_ChangePassword.Visible = !isEdit;
            btn_DelUser.Visible = !isEdit;
        }

        private void GetUserProfile(string loginName)
        {

            lbl_LoginName.Text = loginName;

            txt_FirstName.Text = "";
            txt_MidName.Text = "";
            txt_LastName.Text = "";

            txt_Email.Text = "";
            txt_JobTitle.Text = "";

            chk_IsActive.Checked = false;

            #region --Profile--
            var sql = new Helpers.SQL(_connectionString);
            var query = @"SELECT * FROM [dbo].[User] WHERE LoginName=@LoginName";
            var dtUser = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@LoginName", loginName) });

            if (dtUser != null && dtUser.Rows.Count > 0)
            {
                var dr = dtUser.Rows[0];

                txt_FirstName.Text = dr["FName"].ToString();
                txt_MidName.Text = dr["MName"].ToString();
                txt_LastName.Text = dr["LName"].ToString();

                txt_Email.Text = dr["Email"].ToString();
                txt_JobTitle.Text = dr["JobTitle"].ToString();

                chk_IsActive.Checked = Convert.ToBoolean(dr["IsActived"]);

            }




            #endregion


            #region --Business Units--
            query = @"
SELECT
	bu.BuCode,
	b.BuName
FROM
	[dbo].BuUser bu
	LEFT JOIN [dbo].[Bu] b ON b.BuCode=bu.BuCode
WHERE
	LoginName=@LoginName
	AND b.IsActived=1
ORDER BY
    b.BuName
";
            var dtBu = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@LoginName", loginName) });
            var buItems = dtBu.AsEnumerable()
                .Select(x => new ListEditItem
                {
                    Value = x.Field<string>("BuCode"),
                    Text = string.Format("{0} ({1})", x.Field<string>("BuName"), x.Field<string>("BuCode"))
                })
                .ToArray();

            list_Bu.Items.Clear();
            list_Bu.Items.AddRange(buItems);
            #endregion

        }

        private void GetBuUser(string buCode, string loginName)
        {
            var buConnStr = _bu.GetConnectionString(buCode);
            var sql = new Helpers.SQL(buConnStr);

            // Departmetn
            var userDep = sql.ExecuteQuery("SELECT TOP(1) DepCode FROM [ADMIN].UserDepartment WHERE LoginName=@LoginName", new SqlParameter[] { new SqlParameter("@LoginName", loginName) });
            var depCode = userDep != null && userDep.Rows.Count > 0 ? userDep.Rows[0][0].ToString() : "";

            var dtDepartment = sql.ExecuteQuery("SELECT DepCode as [Value], DepName as [Text] FROM [ADMIN].Department WHERE IsActive=1 ORDER BY DepName");
            var depItems = dtDepartment.AsEnumerable()
                .Select(x => new ListItem
                {
                    Value = x.Field<string>("Value"),
                    Text = x.Field<string>("Text"),
                    Selected = x.Field<string>("Value") == depCode
                }).ToArray();

            ddl_Department.Items.Clear();
            ddl_Department.Items.AddRange(depItems);

            // Roles
            var userRole = sql.ExecuteQuery("SELECT RoleName FROM [ADMIN].UserRole WHERE LoginName=@LoginName", new SqlParameter[] { new SqlParameter("@LoginName", loginName) });
            var roles = userRole != null && userRole.Rows.Count > 0 ? userRole.AsEnumerable().Select(x => x.Field<string>("RoleName").Trim()).ToArray() : new string[0];

            var dtRole = sql.ExecuteQuery("SELECT RoleName as [Value], RoleDesc as [Text] FROM [ADMIN].Role WHERE IsActive=1 ORDER BY RoleDesc");
            var roleItems = dtRole.AsEnumerable()
                .Select(x => new ListEditItem
                {
                    Value = x.Field<string>("Value"),
                    Text = x.Field<string>("Text"),
                    Selected = roles.Contains(x.Field<string>("Value").Trim())
                }).ToArray();

            list_Role.Items.Clear();
            list_Role.Items.AddRange(roleItems);


            // Location
            var userLoc = sql.ExecuteQuery("SELECT LocationCode FROM [ADMIN].UserStore WHERE LoginName=@LoginName", new SqlParameter[] { new SqlParameter("@LoginName", loginName) });
            var locations = userLoc != null && userLoc.Rows.Count > 0 ? userLoc.AsEnumerable().Select(x => x.Field<string>("LocationCode").Trim()).ToArray() : new string[0];

            var dtLocaton = sql.ExecuteQuery("SELECT LocationCode as [Value], CONCAT(LocationCode, ' : ', LocationName) as [Text] FROM [IN].StoreLocation WHERE IsActive=1 ORDER BY LocationCode");
            var locItems = dtLocaton.AsEnumerable()
                .Select(x => new ListEditItem
                {
                    Value = x.Field<string>("Value"),
                    Text = x.Field<string>("Text"),
                    Selected = locations.Contains(x.Field<string>("Value").Trim())
                }).ToArray();

            list_Location.Items.Clear();
            list_Location.Items.AddRange(locItems);



            //var query = @"SELECT * FROM [dbo].[User] WHERE LoginName=@LoginName";
            //var dtUser = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@LoginName", loginName) });

        }

        private void SaveUser(string loginName)
        {
            var fname = txt_FirstName.Text.Trim();
            var mname = txt_MidName.Text.Trim();
            var lname = txt_LastName.Text.Trim();

            var email = txt_Email.Text.Trim();
            var jobTitle = txt_JobTitle.Text.Trim();
            var isActive = chk_IsActive.Checked;

            var sql = new Helpers.SQL(_connectionString);

            var query = @"
UPDATE 
    dbo.[User] 
SET 
    FName=@FName,
    MName=@MName,
    LName=@LName,
    Email=@Email,
    JobTitle=@JobTitle,
    IsActived=@IsActived
 WHERE
    LoginName=@LoginName
";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@LoginName", loginName),
                new SqlParameter("@FName",fname),
                new SqlParameter("@MName",mname),
                new SqlParameter("@LName",lname),
                new SqlParameter("@Email",email),
                new SqlParameter("@JobTitle", jobTitle),
                new SqlParameter("@IsActived", isActive)
            };

            sql.ExecuteQuery(query, parameters);

        }

        private void SaveBuUser(string buCode, string loginName)
        {
            var buConnStr = _bu.GetConnectionString(buCode);
            var sql = new Helpers.SQL(buConnStr);

            // Department
            var depCode = ddl_Department.SelectedItem.Value.ToString();
            var query = @"
DELETE FROM [ADMIN].UserDepartment WHERE LoginName=@LoginName
INSERT INTO [ADMIN].UserDepartment (LoginName, DepCode) VALUES (@LoginName, @DepCode)";
            sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@LoginName", loginName), new SqlParameter("@DepCode", depCode) });

            // Roles
            sql.ExecuteQuery("DELETE FROM [ADMIN].UserRole WHERE LoginName=@LoginName", new SqlParameter[] { new SqlParameter("@LoginName", loginName) });

            query = "INSERT INTO [ADMIN].UserRole (LoginName, RoleName, IsActive) VALUES ";

            var values = new StringBuilder();

            foreach (ListEditItem item in list_Role.Items)
            {
                var roleName = item.Value;
                var isActive = item.Selected ? 1 : 0;
                values.AppendFormat("(@LoginName, '{0}', {1}),", roleName, isActive);
            }

            if (values.ToString().Length > 0)
            {
                query += values.ToString().Trim().TrimEnd(',');

                sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@LoginName", loginName) });
            }

            // Locations
            sql.ExecuteQuery("DELETE FROM [ADMIN].UserStore WHERE LoginName=@LoginName", new SqlParameter[] { new SqlParameter("@LoginName", loginName) });

            query = "INSERT INTO [ADMIN].UserStore (LoginName, LocationCode) VALUES ";

            values.Clear();

            foreach (ListEditItem item in list_Location.Items)
            {
                var code = item.Value;

                if (item.Selected)
                {
                    values.AppendFormat("(@LoginName, '{0}'),", code);
                }
            }

            if (values.ToString().Length > 0)
            {
                query += values.ToString().Trim().TrimEnd(',');

                sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@LoginName", loginName) });
            }
        }

        private void DeleteBuUser(string buCode, string loginName)
        {
            try
            {
                var buConnStr = _bu.GetConnectionString(buCode);
                var sql = new Helpers.SQL(buConnStr);

                // Department
                sql.ExecuteQuery("DELETE FROM [ADMIN].UserDepartment WHERE LoginName=@LoginName", new SqlParameter[] { new SqlParameter("@LoginName", loginName) });

                // Role
                sql.ExecuteQuery("DELETE FROM [ADMIN].UserRole WHERE LoginName=@LoginName", new SqlParameter[] { new SqlParameter("@LoginName", loginName) });

                // Location
                sql.ExecuteQuery("DELETE FROM [ADMIN].UserStore WHERE LoginName=@LoginName", new SqlParameter[] { new SqlParameter("@LoginName", loginName) });


                // Remove Head of department
                var dtDep = sql.ExecuteQuery("SELECT DepCode, HeadOfDep FROM [ADMIN].Department WHERE DepCode IN (SELECT DISTINCT DepCode FROM [admin].vHeadOfDepartment WHERE LoginName=@LoginName)", new SqlParameter[] { new SqlParameter("@LoginName", loginName) });

                if (dtDep != null && dtDep.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDep.Rows)
                    {
                        var depCode = dr["DepCode"].ToString().ToUpper();
                        var hod = dr["HeadOfDep"].ToString().Split(',').Select(x => x.TrimEnd()).ToList();
                        var headOfDep = hod.Where(x => x.ToUpper() != depCode).Select(x => x.Trim()).Distinct().ToArray();

                        sql.ExecuteQuery(
                            "UPDATE [ADMIN].Department SET HeadOfDep=@HeadOfDep WHERE DepCode=@DepCode",
                            new SqlParameter[] 
                        { 
                            new SqlParameter("@HeadOfDep", string.Join(",", headOfDep)), 
                            new SqlParameter("@DepCode", depCode) 
                        });
                    }
                }

                // Remove from Workflow
                var dtWf = sql.ExecuteQuery("SELECT WfId, Step, Approvals FROM APP.WFDt");

                if (dtWf != null && dtWf.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtWf.Rows)
                    {
                        var wfId = dr["WfId"].ToString();
                        var step = dr["Step"].ToString();
                        var items = dr["Approvals"].ToString().Split(',').Select(x => x.TrimEnd()).ToList();


                        var username = string.Format("#{0}", loginName);
                        var item = items.Where(x => x.ToUpper() == username.ToUpper()).FirstOrDefault();

                        if (item != null)
                        {
                            var approvals = items.Where(x => x.ToUpper() != username.ToUpper()).Select(x => x.Trim()).Distinct().ToArray();

                            sql.ExecuteQuery(
                                "UPDATE [APP].WfDt SET Approvals=@Approvals WHERE WfId=@WfId AND Step=@Step",
                                new SqlParameter[] 
                        { 
                            new SqlParameter("@Approvals", string.Join(",", approvals)), 
                            new SqlParameter("@WfId", wfId), 
                            new SqlParameter("@Step", step) 
                        });
                        }
                    }
                }

                // Remove in [dbo].[BuUser]
                new Helpers.SQL(_connectionString).ExecuteQuery("DELETE FROM [dbo].BuUser WHERE LoginName=@LoginName AND BuCode=@BuCode", new SqlParameter[] 
                { 
                    new SqlParameter("@LoginName", loginName),
                    new SqlParameter("@BuCode", buCode) 
                });
            }
            catch
            {
            }



            pop_BuConfirmDelete.ShowOnPageLoad = false;
        }


        #endregion



        public int license { get; set; }
    }

}