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

    private string _connStr { get { return System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString(); } }



    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_ExpiredDate.Text = _user.GetLicenseExpiredDate().ToShortDateString();
        lbl_ActiveUserLicense.Text = _user.GetActiveUserLicense().ToString();
        lbl_ActiveUserCurrent.Text = _user.GetActiveUser().ToString();

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


            pop_Users.ShowOnPageLoad = true;

            //DataTable dt = Get_Ori_dboUser();
            //dsAboutUser.Tables.Add(dt);

            //if (dsAboutUser != null)
            //{
            //    grid_UserList.KeyFieldName = "LoginName";
            //    grid_UserList.DataSource = dsAboutUser.Tables[_user.TableName];
            //    grid_UserList.DataBind();

            //    Session["dsAboutUser"] = dsAboutUser;
            //    pop_Manage.ShowOnPageLoad = true;
            //}
        }

        pop_Login.ShowOnPageLoad = false;
    }

    protected void btn_Inactive_Click(object sender, EventArgs e)
    {
        var gvr = (sender as Button).NamingContainer;
        var hf_LoginName = gvr.FindControl("hf_LoginName") as HiddenField;
        var loginName = hf_LoginName.Value;

        var p = new Blue.DAL.DbParameter[1];

        p[0] = new DbParameter("@LoginName", loginName);

        _bu.DbExecuteQuery("UPDATE [dbo].[User] SET IsActived=0 WHERE LoginName=@LoginName", p, _connStr);

        BindUsers();
        pop_Users.ShowOnPageLoad = true;


    }

    private void BindUsers()
    {
        var dt = _bu.DbExecuteQuery("SELECT LoginName, CONCAT(ISNULL(FName,''),' ',ISNULL(MName,''), ' ',ISNULL(LName,'')) as FullName, IsActived FROM [dbo].[User] ORDER BY LoginName", null, _connStr);
        gv_Users.DataSource = dt;
        gv_Users.DataBind();

    }
}