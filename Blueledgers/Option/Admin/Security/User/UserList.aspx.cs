using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Data.SqlClient;
using System.Web;

namespace BlueLedger.PL.Option.Admin.Security.User
{
    public partial class UserList : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.dbo.BUUser buUser = new Blue.BL.dbo.BUUser();
        private readonly Blue.BL.dbo.User user = new Blue.BL.dbo.User();
        private readonly Blue.BL.Option.Admin.Security.UserRole userRole = new Blue.BL.Option.Admin.Security.UserRole();
        private readonly Blue.BL.ADMIN.UserStore userStore = new Blue.BL.ADMIN.UserStore();
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly string moduleID = "99.98.1.2";

        private readonly string connetionString = System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString();

        //private DataSet dsUser = new DataSet();
        //private DataTable dtUser = new DataTable();
        private DataTable _dtUser
        {
            get { return ViewState["dtUser"] as DataTable; }
            set { ViewState["dtUser"] = value; }
        }

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
                txtSearch.Attributes.Add("onKeyPress", "doClick('" + btnS.ClientID + "',event)");
            }
            else
            {
                //dsUser = (DataSet)Session["dsUser"];
                //dtUser = (DataTable)Session["dtUser"];
                //CountRows(dtUser);
            }

        }

        private void Page_Retrieve()
        {
            //_dtUser = GetListUsers();

            Page_Setting();
            //dsUser.Clear();
            //var result = user.GetList(dsUser, LoginInfo.BuInfo.BuCode);

            //if (result)
            //{
            //    Session["dsUser"] = dsUser;

            //    // Display User data.
            //    Page_Setting();
            //}
        }

        private void Page_Setting()
        {
            Control_HeaderMenuBar();

            BindUserList();

            //            SqlConnection cnn = new SqlConnection(connetionString);
            //            try
            //            {
            //                cnn.Open();

            //            }
            //            catch (Exception ex)
            //            {
            //                cnn.Close();
            //                string ErrorMess = ex.ToString();
            //            }
            //            string sql = @"
            //SELECT 
            //    ROW_NUMBER() OVER(ORDER BY LoginName) as RowId,
            //	LoginName, 
            //	CONCAT(ISNULL(FName,''), ' ', ISNULL(MName,''), ' ', ISNULL(LName,'')) AS FullName,
            //	Email, 
            //	JobTitle, 
            //	IsActived, 
            //	LastLogin,
            //	CASE IsActived 
            //		WHEN '1' THEN 'Active' 
            //		ELSE 'Inactive' 
            //	END IsActived2
            //FROM 
            //	dbo.[User]
            //WHERE
            //	LoginName NOT IN (SELECT LoginName FROM [dbo].[User] WHERE LoginName IN ('support@carmen','support@genex') OR [SectionCode] = 'SUPPORT')
            //ORDER BY
            //	LoginName";


            //            SqlCommand myCommand = new SqlCommand(sql, cnn);
            //            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            //            DataSet dUser = new DataSet();
            //            dtUser = dsUser.Tables[0];
            //            dtUser.Clear();
            //            da.Fill(dtUser);

            //            gvUserList.DataSource = dtUser;
            //            gvUserList.DataBind();
            //            CountRows(dtUser);

            //            Control_HeaderMenuBar();
            //            Session["dsUser"] = (DataSet)dsUser;
            //            Session["dtUser"] = (DataTable)dtUser;

        }

        protected void Control_HeaderMenuBar()
        {
            int pagePermission = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            btnAddUser.Visible = (pagePermission >= 3) ? btnAddUser.Visible : false;
        }

        #region -- Event(s) --

        protected void Create(object sender, EventArgs e)
        {
            iFrame_UserInfo.Attributes["src"] = "UserProfile.aspx?mode=CREATE";
            Label_UserInfo.Text = "New";
            pop_UserInfo.Show();
        }

        protected void Print(object sender, EventArgs e)
        {
            Report rpt = new Report();
            rpt.PrintForm(this, "../../../../RPT/PrintForm.aspx", "", "UserList");
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            BindUserList();
            //_dtUser.DefaultView.RowFilter = string.Empty;
            //Page_Setting();
        }

        //protected void chkViewAllBu_CheckedChanged(object sender, EventArgs e)
        //{
        //    Page_Load(sender, e);
        //}

        protected void btnS_Click(object sender, EventArgs e)
        {
            BindUserList();
            //GettxtSearch(dtUser, txtSearch.Text);
        }

        protected void ddl_Status_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindUserList();
            //var ddl = (sender as DropDownList);
            //string filterText = string.Format("IsActived2 = '{0}'", ddl.SelectedItem.Text);
            //if (ddl.SelectedIndex == 1)
            //{
            //    filterText = "IsActived = 1";
            //}
            //else if (ddl.SelectedIndex == 2)
            //{
            //    filterText = "IsActived = 0";
            //}
            //filterStatus(dtUser, filterText);

        }

        protected void gvUserList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblFullName = (Label)(e.Row.FindControl("lblFull"));
            Label lblEmail = (Label)(e.Row.FindControl("lblEmail"));
            if (lblFullName != null)
            {
                //lblFullName.Text = (string)DataBinder.Eval(e.Row.DataItem, "FullName");
                lblFullName.Text = string.Format("{0}", DataBinder.Eval(e.Row.DataItem, "FullName"));
            }
            if (lblEmail != null)
            {
                //lblEmail.Text = (string)DataBinder.Eval(e.Row.DataItem, "Email");
                lblEmail.Text = string.Format("{0}", DataBinder.Eval(e.Row.DataItem, "Email"));
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='#4D4D4D'; this.style.color='white';";
                e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white'; this.style.color='black';";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvUserList, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }

        protected void gvUserList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string loginName = gvUserList.SelectedRow.Cells[2].Text;
            iFrame_UserInfo.Attributes["src"] = "UserProfile.aspx?mode=VIEW&user=" + loginName + "";
            Label_UserInfo.Text = loginName;
            Session["saveStatus"] = "0";
            pop_UserInfo.Show();
        }

        #endregion

        private void BindUserList()
        {
            _dtUser = GetListUsers();

            gvUserList.DataSource = _dtUser;
            gvUserList.DataBind();

            ShowLicense();
        }


        private DataTable GetListUsers()
        {
            var buCode = "";
            var status = ddl_Status.SelectedValue.ToString();
            var search = "%" + txtSearch.Text.Trim() + "%";
            var query = @"
;WITH
exclude AS(
	SELECT 
		LoginName 
	FROM 
		[dbo].[User] 
	WHERE 
		LoginName IN ('support@carmen','support@genex') 
		OR [SectionCode] = 'SUPPORT'		
),
u AS(
	SELECT
		DISTINCT u.LoginName
	FROM
		dbo.[User] u
		JOIN [dbo].BuUser bu ON bu.LoginName=u.LoginName
	WHERE
		u.LoginName NOT IN (SELECT LoginName FROM exclude)
		AND bu.BuCode=CASE WHEN ISNULL(@BuCode,'')='' THEN bu.BuCode ELSE @BuCode END
),
usr AS(
	SELECT 
		LoginName, 
		CONCAT(ISNULL(FName,''), ' ', ISNULL(MName,''), ' ', ISNULL(LName,'')) AS FullName,
		Email, 
		JobTitle, 
		IsActived, 
		CASE IsActived 
			WHEN '1' THEN 'Active' 
			ELSE 'Inactive' 
		END [Status],
		LastLogin
	FROM 
		dbo.[User]
	WHERE
		IsActived= CASE @Status 
			WHEN '0' THEN 0
			WHEN '1' THEN 1
			ELSE IsActived
		END
		AND LoginName IN (SELECT LoginName FROM u)
)
SELECT
	ROW_NUMBER() OVER(ORDER BY LoginName) as RowId,
	*
FROM
	usr
WHERE
	LoginName LIKE @Search
	OR FullName LIKE @Search
	OR Email LIKE @Search
	OR JobTitle LIKE @Search
ORDER BY
	LoginName";
            var parameters = new Blue.DAL.DbParameter[]
            {
                new Blue.DAL.DbParameter("@BuCode", buCode),
                new Blue.DAL.DbParameter("@Search", search),
                new Blue.DAL.DbParameter("@Status", status),
            };

            var dt = user.DbExecuteQuery(query, parameters, connetionString);

            return dt;
        }




        protected void ShowLicense()
        {
            var license = user.GetActiveUserLicense();
            var active = user.GetActiveUser();
            var available = license - active;

            var text = string.Format("License: {0} purchased | {1} available, {2} assigned", license, available, active);

            lblcountA.Text = text;
        }

        //private void filterStatus(DataTable dt_, string filter)
        //{
        //    string filterAll = string.Format("IsActived2 = '{0}'", "All");
        //    if (filter == filterAll)
        //    {
        //        dt_.DefaultView.RowFilter = string.Empty;
        //        gvUserList.DataSource = dt_;
        //        gvUserList.DataBind();
        //    }
        //    else
        //    {
        //        dt_.DefaultView.RowFilter = filter;
        //        gvUserList.DataSource = dt_;
        //        gvUserList.DataBind();
        //    }

        //}

        //private void GettxtSearch(DataTable dt_, string txt)
        //{
        //    string filtertxt;
        //    string searchFilter = "(FullName like '%{0}%' OR Email like '%{0}%' OR LoginName like '%{0}%' OR  JobTitle like '%{0}%')";
        //    dt_.DefaultView.RowFilter = string.Empty;
        //    if (ddlActive.SelectedItem.Text == "Active")
        //    {
        //        filtertxt = string.Format(searchFilter + " AND (IsActived2  = '{1}')", txt, "Active");
        //    }
        //    else if (ddlActive.SelectedItem.Text == "Inactive")
        //    {
        //        filtertxt = string.Format(searchFilter + " AND (IsActived2  = '{1}')", txt, "Inactive");
        //    }
        //    else
        //    {
        //        filtertxt = string.Format(searchFilter, txt);
        //    }

        //    dt_.DefaultView.RowFilter = filtertxt;
        //    gvUserList.DataSource = dt_;
        //    gvUserList.DataBind();
        //}


    }
}