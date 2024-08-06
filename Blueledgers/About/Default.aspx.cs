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
            DataTable dt = Get_Ori_dboUser();
            dsAboutUser.Tables.Add(dt);

            if (dsAboutUser != null)
            {
                grid_UserList.KeyFieldName = "LoginName";
                grid_UserList.DataSource = dsAboutUser.Tables[_user.TableName];
                grid_UserList.DataBind();

                Session["dsAboutUser"] = dsAboutUser;
                pop_Manage.ShowOnPageLoad = true;
            }
        }

        pop_Login.ShowOnPageLoad = false;
    }

    protected virtual DataTable Get_Ori_dboUser()
    {
        string conStr = System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString();
        string sqlStr = "SELECT * FROM [dbo].[User]";
        DataTable dtUser = new DataTable();
        SqlConnection con = new SqlConnection(conStr);

        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(sqlStr, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtUser);
            dtUser.TableName = dtSaveUser;
            con.Close();
            return dtUser;
        }
        catch (Exception ex)
        {
            string mess = ex.Message;
            con.Close();
            return null;
        }

    }

    protected void grid_UserList_Init(object sender, EventArgs e)
    {
        if (dsAboutUser.Tables[_user.TableName] != null)
        {
            DataTable dt = dsAboutUser.Tables[_user.TableName];
            grid_UserList.KeyFieldName = "LoginName";
            grid_UserList.DataSource = dt;
            grid_UserList.DataBind();
        }
    }

    protected void grid_UserList_Load(object sender, EventArgs e)
    {
        if (dsAboutUser.Tables[_user.TableName] != null)
        {
            DataTable dt = dsAboutUser.Tables[_user.TableName];
            grid_UserList.KeyFieldName = "LoginName";
            grid_UserList.DataSource = dt;
            grid_UserList.DataBind();
        }
    }

    protected void grid_UserList_PageIndexChanged(object sender, EventArgs e)
    {
        int pageIndex = (sender as ASPxGridView).PageIndex;
        string filter = string.Empty;
        DataTable dt = dsAboutUser.Tables[_user.TableName];

        grid_UserList.PageIndex = pageIndex;
        grid_UserList.KeyFieldName = "LoginName";
        grid_UserList.DataSource = dt;
        grid_UserList.DataBind();
    }

    protected void cb_IsActive_CheckedChanged(object sender, EventArgs e)
    {
        // Note: ASPxGridView cann't use DataBound like ASP.net(normal). So, I use Value='<% #Bind("...")%>'
        ASPxCheckBox cb_IsActive = (ASPxCheckBox)sender;
        GridViewDataItemTemplateContainer cb_con = (GridViewDataItemTemplateContainer)cb_IsActive.NamingContainer;
        string rowValue = Convert.ToString(cb_con.KeyValue); // KeyValueField = "LoginName"
        bool cbBool = Convert.ToBoolean(cb_IsActive.Value);

        string filter = string.Format("LoginName = '{0}'", rowValue);
        DataTable dt = dsAboutUser.Tables[_user.TableName];
        DataRow[] drrow = dt.Select(filter);
        if (drrow.Length > 0 && drrow.Length <= 1)
        {
            drrow[0]["IsActived"] = cbBool;

            DataRow[] drUserRow = dsAboutUser.Tables[dtSaveUser].Select(filter);
            drUserRow[0]["IsActived"] = cbBool;
            Session["dsAboutUser"] = dsAboutUser;
        }
    }

    protected void btn_Save_Click(object sender, EventArgs e)
    {
        dsAboutUser.Tables.Remove(dsAboutUser.Tables[_user.TableName]);
        dsAboutUser.Tables[dtSaveUser].TableName = _user.TableName;

        bool returnSave = _user.Save(dsAboutUser);
        pop_Manage.ShowOnPageLoad = (returnSave) ? false : true;
        Page.Response.Redirect(Page.Request.Url.ToString(), true);
    }

    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        pop_Manage.ShowOnPageLoad = false;
    }
}