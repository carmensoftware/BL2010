using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;



public partial class License : System.Web.UI.Page
{
    private readonly string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString();
    private readonly Blue.BL.dbo.User _user = new Blue.BL.dbo.User();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }

        ShowLicense();
    }

    protected void btn_Login_Click(object sender, EventArgs e)
    {
        var username = txt_Username.Text.Trim();
        var password = txt_Password.Text.Trim();

        var error = Login(username, password);

        if (string.IsNullOrEmpty(error))
        {
            ShowUsers();


            pop_Login.ShowOnPageLoad = false;
            pop_Setting.ShowOnPageLoad = true;
        }
        else
        {
            ShowAlert(error);
        }


    }

    protected void btn_Setting_Click(object sender, EventArgs e)
    {
        pop_Login.ShowOnPageLoad = true;
    }

    protected void gv_Users_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        var loginName = e.CommandArgument.ToString();
        var isActive = e.CommandName == "active" ? 1 : 0;

        var query = "UPDATE [dbo].[User] SET IsActived=@IsActived WHERE LoginName=@LoginName";


        var conn = new SqlConnection(connStr);

        try
        {
            conn.Open();
            var cmd = new SqlCommand(query, conn);

            cmd.Parameters.Add(new SqlParameter("@IsActived", isActive));
            cmd.Parameters.Add(new SqlParameter("@LoginName", loginName));
            cmd.ExecuteNonQuery();

            conn.Close();

            ShowLicense();
            ShowUsers();

        }
        catch (Exception ex)
        {
            conn.Close();
            throw new Exception(ex.Message);
        }
    }


    // Method(s)
    private void ShowAlert(string text)
    {
        lbl_Alert.Text = text;
        pop_Alert.ShowOnPageLoad = true;
    }

    private void ShowLicense()
    {
        var userLicense = _user.GetActiveUserLicense();
        var userActive = _user.GetActiveUser();
        var expiredDate = _user.GetLicenseExpiredDate();

        lbl_ExpiryDate.Text = expiredDate.ToString("dd/MM/yyyy");
        lbl_UserLicense.Text = userLicense.ToString();
        lbl_UserActive.Text = userActive.ToString();

    }

    private string Login(string username, string password)
    {
        var error = "";

        if (username.ToLower() != "support@carmen")
        {

            var sql = new Helpers.SQL(connStr);
            var query = "SELECT LoginName FROM [dbo].[User] WHERE SectionCode='SUPPORT' AND LoginName=@LoginName";
            var dt = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@LoginName", username) });

            if (dt != null && dt.Rows.Count == 0)
            {
                error = "Username is not support.";
            }
        }

        var ds = new DataSet();

        if (_user.GetUserListByLoginName(ds, username, password))
        {

        }
        else
        {
            error = "Invalid username/password.";
        }

        return error;

    }

    private void ShowUsers()
    {
        gv_Users.DataSource = GetUsers();
        gv_Users.DataBind();
    }

    private DataTable GetUsers()
    {
        var query = @"
SELECT 
    ROW_NUMBER() OVER(ORDER BY LoginName) as RowId,
    LoginName, 
    FName, 
    MName, 
    LName, 
    IsActived, 
	SectionCode,
    CASE WHEN IsActived=1 THEN 'Active' ELSE '' END [Status]
FROM 
    [dbo].[User] 
-- WHERE LoginName NOT IN (SELECT LoginName FROM dbo.[User] WHERE LoginName = 'support@carmen' OR SectionCode = 'SUPPORT')
ORDER BY 
    LoginName";

        var dtUser = new DataTable();
        var conn = new SqlConnection(connStr);

        try
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtUser);



            var license = lbl_UserLicense.Text;
            var assigned = lbl_UserActive.Text;

            lbl_Setting_License.Text = string.Format("License: {0} purchased | {1} assigned", license, assigned);

            conn.Close();

            return dtUser;
        }
        catch (Exception ex)
        {
            conn.Close();
            throw new Exception(ex.Message);
        }
    }

}