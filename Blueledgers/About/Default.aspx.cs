using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;

using DevExpress.Web.ASPxGridView;
using BlueLedger.PL.BaseClass;
using Blue.BL;
using Blue.DAL;
using DevExpress.Web.ASPxEditors;


public partial class License : System.Web.UI.Page
{
    private readonly Blue.BL.dbo.Bu _bu = new Blue.BL.dbo.Bu();
    private readonly Blue.BL.dbo.User _user = new Blue.BL.dbo.User();
    private DataSet dsAboutUser = new DataSet();
    private string dtSaveUser = "SaveUser";


    /* BIG NOTE:
     * BasePage = cannot get expiredDate, ActiveUserLicense, ActiveUserCurrent.
     */
    protected void Page_Load(object sender, EventArgs e)
    {
        var userLicense = _user.GetActiveUserLicense();
        var userActive = _user.GetActiveUser();
        var expiredDate = _user.GetLicenseExpiredDate();

        hf_UserActive.Value = userActive.ToString();
        hf_UserLicense.Value = userLicense.ToString();
        hf_ExpiredDate.Value = expiredDate.ToString("yyyy-MM-dd");

        lbl_ExpiredDate.Text = expiredDate.ToShortDateString();
        lbl_ActiveUserLicense.Text = userLicense.ToString();
        lbl_ActiveUserCurrent.Text = userActive.ToString();

        if (!IsPostBack)
        {
            Session["dsAboutUser"] = dsAboutUser;
        }
        else
        {
            lbl_Test.Text = string.Empty;
            dsAboutUser = (DataSet)Session["dsAboutUser"];
        }
    }

    // Modified on: 21/11/2017, By: Fon
    protected void btn_LogIn_Click(object sender, EventArgs e)
    {
        string usr = txt_login.Text;
        string pwd = txt_password.Text;
        dsAboutUser.Clear();
        Session["dsAboutUser"] = dsAboutUser;

        if (_user.GetUserListByLoginName(dsAboutUser, usr, pwd))
        {
            BindUsers();
            pop_UserList.ShowOnPageLoad = true;            
        }

        pop_Login.ShowOnPageLoad = false;
    }


    protected void gv_Users_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        var loginName = e.CommandArgument.ToString();
        var isActive = e.CommandName == "active" ? 1 : 0;

        var query = "UPDATE [dbo].[User] SET IsActived=@IsActived WHERE LoginName=@LoginName";


        var connStr = System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString();
        var conn = new SqlConnection(connStr);

        try
        {
            conn.Open();
            var cmd = new SqlCommand(query, conn);

            cmd.Parameters.Add(new SqlParameter("@IsActived", isActive));
            cmd.Parameters.Add(new SqlParameter("@LoginName", loginName));
            cmd.ExecuteNonQuery();

            conn.Close();

            BindUsers();

        }
        catch (Exception ex)
        {
            conn.Close();
            throw new Exception(ex.Message);
        }


        // You can add more else if blocks for other command names
        // Get the row index from the CommandArgument

        // Access the GridViewRow and data (e.g., using DataKeys)

        // Implement your logic based on the command and data
        //System.Diagnostics.Debug.WriteLine($"Adding customer {customerId} to cart.");
    }


    private void BindUsers()
    {
        gv_Users.DataSource = GetUsers();
        gv_Users.DataBind();
    }


    private DataTable GetUsers()
    {
        var connStr = System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString();
        var query = @"
SELECT 
    ROW_NUMBER() OVER(ORDER BY IsActived DESC, LoginName) as RowId,
    LoginName, 
    FName, 
    MName, 
    LName, 
    IsActived, 
    CASE WHEN IsActived=1 THEN 'Active' ELSE '' END [Status] 
FROM 
    [dbo].[User] 
WHERE
    LoginName <> 'support@carmen'
ORDER BY 
    IsActived DESC,
    LoginName";

        var dtUser = new DataTable();
        var conn = new SqlConnection(connStr);

        try
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtUser);


            query = "SELECT COUNT(LoginName) FROM [dbo].[User] WHERE IsActived=1 AND LoginName <> 'support@carmen'";

            var cmd1 = new SqlCommand(query, conn);


            var count = (int)cmd1.ExecuteScalar();

            lbl_ActiveUserCount.Text = string.Format("Actived user(s) = {0}", count);
            
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