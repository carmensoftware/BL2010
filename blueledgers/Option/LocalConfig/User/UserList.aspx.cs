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
        private DataSet dsUser = new DataSet();

        private DataTable dtUser = new DataTable();
        private static string loginNameText = "";

        #endregion


        /// <summary>
        ///     Display all User data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            //Response.Write("TEST_PageLoad");

            base.Page_Load(sender, e);
            if (!IsPostBack)
            {
                Page_Retrieve();
                txtSearch.Attributes.Add("onKeyPress", "doClick('" + btnS.ClientID + "',event)");
            }
            else
            {
                dsUser = (DataSet)Session["dsUser"];
                dtUser = (DataTable)Session["dtUser"];
                countRow(dtUser);
            }

        }

        /// <summary>
        ///     Get user data.
        /// </summary>
        private void Page_Retrieve()
        {
            dsUser.Clear();
            var result = user.GetList(dsUser, LoginInfo.BuInfo.BuCode);

            if (result)
            {
                Session["dsUser"] = dsUser;

                // Display User data.
                Page_Setting();
            }
        }

        /// <summary>
        ///     Display user data.
        /// </summary>
        private void Page_Setting()
        {
            string connetionString = System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString();
            SqlConnection cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();

            }
            catch (Exception ex)
            {
                cnn.Close();
                string ErrorMess = ex.ToString();
            }
            string strsql = "select FName+' '+MName+' '+LName AS FullName,";
            strsql += " Email, LoginName, JobTitle, IsActived, LastLogin,";
            strsql += " Case IsActived When '1' Then 'Active' When '0' Then 'Inactive' Else 'Null' End AS IsActived2 ";
            strsql += " from dbo.[User]";
            SqlCommand myCommand = new SqlCommand(strsql, cnn);
            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            DataSet dUser = new DataSet();
            dtUser = dsUser.Tables[0];
            dtUser.Clear();
            da.Fill(dtUser);

            gvUserList.DataSource = dtUser;
            gvUserList.DataBind();
            countRow(dtUser);

            Session["dsUser"] = (DataSet)dsUser;
            Session["dtUser"] = (DataTable)dtUser;

        }
        protected void gvUserList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblFullName = (Label)(e.Row.FindControl("lblFull"));
            Label lblEmail = (Label)(e.Row.FindControl("lblEmail"));
            if (lblFullName != null)
            {
                lblFullName.Text = (string)DataBinder.Eval(e.Row.DataItem, "FullName");
            }
            if (lblEmail != null)
            {
                lblEmail.Text = (string)DataBinder.Eval(e.Row.DataItem, "Email");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='#4D4D4D'; this.style.color='white';";
                e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white'; this.style.color='black';";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvUserList, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }

        protected void countRow(DataTable dt)
        {
            DataRow[] rowA = dt.Select("IsActived = 1");
            int countT = dt.Rows.Count;
            int countA = rowA.Length;

            lblcountA.Text = String.Format("{0:,0}", countA) + " active(s)/ " + String.Format("{0:,0}", countT) + " user(s)";
        }
        protected void btnHome_Click(object sender, EventArgs e)
        {
            dtUser.DefaultView.RowFilter = string.Empty;
            Page_Setting();
        }

        protected void ddlActive_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string filterText = string.Format("IsActived2 = '{0}'", ddlActive.SelectedItem.Text);
            if (ddlActive.SelectedIndex == 1)
            {
                filterText = "IsActived = 1";
            }
            else if (ddlActive.SelectedIndex == 2)
            {
                filterText = "IsActived = 0";
            }
            filterStatus(dtUser, filterText);

        }

        private void filterStatus(DataTable dt_, string filter)
        {
            string filterAll = string.Format("IsActived2 = '{0}'", "All");
            if (filter == filterAll)
            {
                dt_.DefaultView.RowFilter = string.Empty;
                gvUserList.DataSource = dt_;
                gvUserList.DataBind();
            }
            else
            {
                dt_.DefaultView.RowFilter = filter;
                gvUserList.DataSource = dt_;
                gvUserList.DataBind();
            }

        }
        protected void btnS_Click(object sender, EventArgs e)
        {
            GettxtSearch(dtUser, txtSearch.Text);
        }
        private void GettxtSearch(DataTable dt_, string txt)
        {
            string filtertxt;
            string searchFilter = "(FullName like '%{0}%' OR Email like '%{0}%' OR LoginName like '%{0}%' OR  JobTitle like '%{0}%')";
            dt_.DefaultView.RowFilter = string.Empty;
            if (ddlActive.SelectedItem.Text == "Active")
            {
                filtertxt = string.Format(searchFilter + " AND (IsActived2  = '{1}')", txt, "Active");
            }
            else if (ddlActive.SelectedItem.Text == "Inactive")
            {
                filtertxt = string.Format(searchFilter + " AND (IsActived2  = '{1}')", txt, "Inactive");
            }
            else
            {
                filtertxt = string.Format(searchFilter, txt);
            }

            dt_.DefaultView.RowFilter = filtertxt;
            gvUserList.DataSource = dt_;
            gvUserList.DataBind();
        }
        protected void gvUserList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string loginName = gvUserList.SelectedRow.Cells[1].Text;
            iFrame_UserInfo.Attributes["src"] = "UserProfile.aspx?mode=VIEW&user=" + loginName + "";
            Label_UserInfo.Text = loginName;
            Session["saveStatus"] = "0";
            pop_UserInfo.Show();
        }

        protected void Create(object sender, EventArgs e)
        {
            // Check Maximum User Number.
            // Not allow to create user if the number of user in the business unit is equal to maximum.
            //var buLicense = new Blue.BL.dbo.BuLicense();
            //var buUser = new Blue.BL.dbo.BUUser();

            //if (buUser.GetUserNo(LoginInfo.BuInfo.BuCode) > buLicense.GetMaxUser(LoginInfo.BuInfo.BuCode))
            //{
            //    // Display Error Message
            //    pop_ReachMaxUserNo.ShowOnPageLoad = true;

            //}
            //else
            {
                //pop_UserInfo.BehaviorID = "btnCreate";
                //iFrame_UserInfo.Attributes["src"] = "UserManage.aspx?mode=CREATE";
                iFrame_UserInfo.Attributes["src"] = "UserProfile.aspx?mode=CREATE";
                Label_UserInfo.Text = "New";
                pop_UserInfo.Show();
            }
        }

        protected void ButtonClose_Click(object sender, EventArgs e)
        {
            //Response.Redirect(Request.RawUrl);
            Response.Write("TEST");

            //string status = Session["saveStatus"].ToString();
            //if (status == "1")
            //{
            //    //Response.Redirect(Request.RawUrl);
            //    Page.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            //}
        }
    }
}