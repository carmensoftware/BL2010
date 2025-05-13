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



namespace BlueLedger.PL.Option.Admin.Security.User
{
    public partial class UserList : BasePage
    {
        private readonly string moduleID = "99.98.1.2";

        #region "Declaration"
        private readonly Blue.BL.Option.Admin.Security.UserRole userRole = new Blue.BL.Option.Admin.Security.UserRole();

        private readonly Blue.BL.dbo.User _user = new Blue.BL.dbo.User();
        private readonly Blue.BL.dbo.BUUser _buUser = new Blue.BL.dbo.BUUser();
        private readonly Blue.BL.ADMIN.UserStore _userStore = new Blue.BL.ADMIN.UserStore();
        private readonly Blue.BL.ADMIN.RolePermission _rolePermiss = new Blue.BL.ADMIN.RolePermission();

        #endregion

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

        private string _status { get { return Request.QueryString["status"] == null ? "" : Request.QueryString["status"].ToString(); } }

        private string _bu { get { return Request.QueryString["bu"] == null ? "" : Request.QueryString["bu"].ToString(); } }


        // Event(s)

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                _dtBusinessUnit = GetBusinessUnit();
                LoadUserList(_status, _bu);

            }



            Control_HeaderMenuBar();
        }

        protected void Control_HeaderMenuBar()
        {
            int pagePermission = _rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);

            if (pagePermission < 3)
            {
                Response.Redirect("~/Option/User/Default.aspx");
            }
            else
            {
                btn_Create.Visible = (pagePermission >= 3) ? btn_Create.Visible : false;
            }



        }

        protected void btn_Create_Click(object sender, EventArgs e)
        {
            iFrame_UserInfo.Attributes["src"] = "UserProfile.aspx?mode=CREATE";
            pop_UserInfo.HeaderText = "New";
            pop_UserInfo.ShowOnPageLoad = true;
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

            iFrame_UserInfo.Attributes["src"] = "UserProfile.aspx?mode=VIEW&user=" + HttpUtility.HtmlEncode(loginName) + "";
            pop_UserInfo.HeaderText = loginName;
            pop_UserInfo.ShowOnPageLoad = true;
            


            //Response.Redirect(string.Format("UserProfile.aspx?id={0}", HttpUtility.HtmlEncode(loginName)));

            //var dr = GetUserInfo(loginName);

            //if (dr != null)
            //{
            //    var fullName = dr["FName"].ToString()
            //        + " "
            //        + (string.IsNullOrEmpty(dr["MName"].ToString()) ? "" : dr["MName"].ToString())
            //        + " "
            //        + dr["LName"].ToString();

            //    lbl_UserFullName.Text = fullName.Trim();

            //    txt_LoginName.Text = dr["LoginName"].ToString();
            //    chk_IsActived.Checked = Convert.ToBoolean(dr["IsActived"]);
            //    txt_FName.Text = dr["FName"].ToString();
            //    txt_MName.Text = dr["MName"].ToString();
            //    txt_LName.Text = dr["LName"].ToString();
            //    txt_Email.Text = dr["Email"].ToString();
            //    txt_JobTitle.Text = dr["JobTitle"].ToString();


            //    hf_LoginName.Value = loginName;


            //    list_Bu.Items.Clear();

            //    //list_Bu.Items.Add(new ListEditItem("", ""));
            //    //ListEditItem 
            //    var items = _dtBu.AsEnumerable()
            //        .Where(x => x.Field<int>("Selected") == 1)
            //        .Select(x => new ListEditItem(x.Field<string>("BuName"), x.Field<string>("BuCode")))
            //        .ToArray();
            //    list_Bu.Items.AddRange(items);

            //    pop_User.HeaderText = loginName;
            //    pop_User.ShowOnPageLoad = true;
                
            //}

        }

        // Method(s)
        #region --Method(s)--

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
    Row_Number() OVER(ORDER BY IsActived DESC, LoginName) as RowId,
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
    IsActived DESC,
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


        #endregion



        public int license { get; set; }
    }

}