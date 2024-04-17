using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Data.SqlClient;
using DevExpress.Web.ASPxEditors;

namespace BlueLedger.PL.Option.Admin.Security.Department
{
    public partial class DepartmentList : BasePage
    {
        #region Attribute

        private readonly string moduleId = "99.98.1.3";
        private readonly Blue.BL.ADMIN.Department dep = new Blue.BL.ADMIN.Department();
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();

        //private static List<string> listHOD = new List<string>();

        private DataTable dtDepartment
        {
            set { Session["dtDepartment"] = value; }
            get { return (DataTable)Session["dtDepartment"]; }
        }

        private DataTable dtUserList
        {
            set { Session["dsUserList"] = value; }
            get { return (DataTable)Session["dsUserList"]; }
        }

        private DataTable dtHOD
        {
            set { Session["dtHOD"] = value; }
            get { return (DataTable)Session["dtHOD"]; }
        }


        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                dtDepartment = new DataTable();
                dtUserList = new DataTable();
                dtHOD = new DataTable();
                dtHOD.Columns.Add(new DataColumn("LoginName", System.Type.GetType("System.String")));


                Page_Retrieve();
            }
            else
            {
                list_HOD.DataSource = dtHOD;
                list_HOD.DataBind();
            }
            hf_ConnStr.Value = LoginInfo.ConnStr;
        }

        private void Page_Retrieve()
        {
            //var getDep = dep.GetList(dtDepartment, LoginInfo.ConnStr);
            dtDepartment = dep.DbExecuteQuery("SELECT * FROM [ADMIN].Department ORDER BY DepCode", null, LoginInfo.ConnStr);

            Page_Setting();
        }

        private void Page_Setting()
        {
            gvDepartment.DataSource = dtDepartment;
            gvDepartment.DataBind();

            Control_HeaderMenuBar();
        }

        private void Control_HeaderMenuBar()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleId, LoginInfo.LoginName, LoginInfo.ConnStr);
            menu_CmdBar.Items.FindByName("Create").Visible = (pagePermiss >= 3) ? menu_CmdBar.Items.FindByName("Create").Visible : false;
            menu_CmdBar.Items.FindByName("Delete").Visible = (pagePermiss >= 3) ? menu_CmdBar.Items.FindByName("Delete").Visible : false;
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Create();
                    break;

                case "DELETE":
                    Delete();
                    break;

                //case "PRINT":
                //    break;
            }
        }

        private void Create()
        {
            pop_Department.HeaderText = "New";

            txt_DepCode.Enabled = true;
            txt_DepCode.Text = string.Empty;
            txt_DepName.Text = string.Empty;
            check_Active.Checked = true;
            list_HOD.Items.Clear();

            // Asign head of deartment to list (list_HOD)
            dtHOD = dep.DbExecuteQuery(string.Format("SELECT LoginName FROM [ADMIN].vHeadOfDepartment WHERE ISNULL(LoginName,'') <> '' AND DepCode = '{0}'", string.Empty), null, LoginInfo.ConnStr);
            BindListHOD(dtHOD);

            pop_Department.ShowOnPageLoad = true;

            //var drNew = dtDepartment.NewRow();

            //drNew["DepCode"] = string.Empty;
            //drNew["DepName"] = string.Empty;
            //drNew["HeadOfDep"] = string.Empty;
            //drNew["AccDepCode"] = string.Empty;

            //drNew["IsActive"] = true;
            //drNew["CreatedDate"] = ServerDateTime.Date;
            //drNew["CreatedBy"] = LoginInfo.LoginName;
            //drNew["UpdatedDate"] = ServerDateTime.Date;
            //drNew["UpdatedBy"] = LoginInfo.LoginName;



            //var drNew = dsDep.Tables[dep.TableName].NewRow();

            //// Modified on: 26/02/2018, By: Fon
            //drNew["DepCode"] = string.Empty;
            ////drNew["DepCode"] = GenDepCode();
            //// End Modifed.

            //drNew["DepName"] = string.Empty;
            //drNew["HeadOfDep"] = string.Empty;
            //drNew["AccDepCode"] = string.Empty;

            //drNew["IsActive"] = true;
            //drNew["CreatedDate"] = ServerDateTime.Date;
            //drNew["CreatedBy"] = LoginInfo.LoginName;
            //drNew["UpdatedDate"] = ServerDateTime.Date;
            //drNew["UpdatedBy"] = LoginInfo.LoginName;

            //dsDep.Tables[dep.TableName].Rows.Add(drNew);

            //grd_Department1.DataSource = dsDep.Tables[dep.TableName];
            //grd_Department1.EditIndex = grd_Department1.Rows.Count;
            //grd_Department1.DataBind();

            //DepEditMode = "NEW";

            //// Added on: 28/03/2018, By: Fon
            //menu_CmdBar.Items.FindByName("Create").Visible = false;
            ////this.ClientScript.RegisterStartupScript(this.GetType(), "ScrollScript", "window.scrollTo(0, document.body.scrollHeight);", true);
            ////ClientScript.RegisterStartupScript(typeof(Page), "Scroll", "setTimeOut(scrollToDiv, 1);", true);
            //// End Added.
        }

        private void Delete()
        {
            List<string> list = new List<string>();

            foreach (GridViewRow row in gvDepartment.Rows)
            {
                var lbl_DepCode = row.FindControl("lbl_DepCode") as Label;
                var chk_Item = row.FindControl("chk_Item") as CheckBox;
                if (chk_Item.Checked)
                    list.Add("'" + lbl_DepCode.Text + "'");
            }

            string message = string.Empty;

            if (list.Count > 0)
            {
                string sql;
                DataTable dt = new DataTable();
                // Check PR
                sql = string.Format(@"SELECT COUNT(*) as RecordCount
                                FROM PC.PR
                                WHERE DocStatus <> 'Rejected'
                                AND HOD IN ({0})", string.Join(",", list));
                dt = dep.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    message = "Do not deleted, some department code(s) have already used in Purchase Request.";

                // Check SR
                //                if (message == string.Empty)
                //                {
                //                    sql = string.Format(@"SELECT COUNT(*) as RecordCount
                //                                                    FROM [IN].StoreRequisition
                //                                                    WHERE DocStatus <> 'Rejected')");
                //                    dt = dep.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
                //                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                //                        message = "Do not deleted, some department code(s) have already used in Store Requisition.";
                //                }

                // Check user department
                if (message == string.Empty)
                {
                    sql = string.Format(@"SELECT COUNT(*) as RecordCount
                                FROM [ADMIN].UserDepartment
                                WHERE DepCode IN ({0})", string.Join(",", list));
                    dt = dep.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                        message = "Do not deleted, some department code(s) have already been assigned to users.";
                }
            }


            if (message != string.Empty)
            {
                lbl_Alert.Text = message;
                pop_Alert.ShowOnPageLoad = true;
            }
            else
            {
                lbl_Confirm.Text = "Do you want to delete selected department code(s)?";
                pop_Confirm.ShowOnPageLoad = true;
            }
        }

        protected void btn_ConfirmYes_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();

            foreach (GridViewRow row in gvDepartment.Rows)
            {
                var lbl_DepCode = row.FindControl("lbl_DepCode") as Label;
                var chk_Item = row.FindControl("chk_Item") as CheckBox;
                if (chk_Item.Checked)
                    list.Add("'" + lbl_DepCode.Text + "'");
            }

            string sql = string.Format(@"DELETE FROM [ADMIN].Department WHERE DepCode IN ({0})", string.Join(",", list));
            DataTable dt = dep.DbExecuteQuery(sql, null, LoginInfo.ConnStr);


            pop_Confirm.ShowOnPageLoad = false;
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void gvDepartment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["style"] = "cursor:pointer";

                if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
                    e.Row.Attributes["onclick"] = null;
                else
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvDepartment, "Select$" + e.Row.RowIndex);

                // ---------------------------------------------------------

                if (e.Row.FindControl("lbl_DepCode") != null)
                {
                    var label = e.Row.FindControl("lbl_DepCode") as Label;
                    label.Text = DataBinder.Eval(e.Row.DataItem, "DepCode").ToString();
                }

                if (e.Row.FindControl("lbl_DepName") != null)
                {
                    var label = e.Row.FindControl("lbl_DepName") as Label;
                    label.Text = DataBinder.Eval(e.Row.DataItem, "DepName").ToString();
                }

                if (e.Row.FindControl("lbl_HOD") != null)
                {
                    var label = e.Row.FindControl("lbl_HOD") as Label;
                    label.Text = DataBinder.Eval(e.Row.DataItem, "HeadOfDep").ToString();
                }

                if (e.Row.FindControl("lbl_AccCode") != null)
                {
                    var label = e.Row.FindControl("lbl_AccCode") as Label;
                    label.Text = DataBinder.Eval(e.Row.DataItem, "AccDepCode").ToString();
                }

                bool isActive = DataBinder.Eval(e.Row.DataItem, "IsActive").ToString() == "True";


                if (e.Row.FindControl("hf_IsActive") != null)
                {
                    var hf = e.Row.FindControl("hf_IsActive") as HiddenField;
                    hf.Value = isActive.ToString();
                }

                Image imgCheck = (Image)(e.Row.FindControl("imgActive"));
                if (imgCheck != null)
                {
                    if (isActive)
                    {
                        imgCheck.ImageUrl = "~/App_Themes/Default/Images/master/icon/check-icon.png";
                    }
                    else
                    {
                        imgCheck.ImageUrl = "~/App_Themes/Default/Images/master/icon/inactive-icon.png";
                    }
                }
            }
        }

        protected void btn_Edit_Click(object sender, EventArgs e)
        {
            EditDepartment(sender);
        }

        private void EditDepartment(object sender)
        {
            var row = (GridViewRow)(sender as LinkButton).NamingContainer; ;
            var depCode = (row.FindControl("lbl_DepCode") as Label).Text;
            var depName = (row.FindControl("lbl_DepName") as Label).Text;
            var accCode = (row.FindControl("lbl_AccCode") as Label).Text;
            bool isActive = Boolean.Parse((row.FindControl("hf_IsActive") as HiddenField).Value);

            txt_DepCode.Enabled = false;
            txt_DepCode.Text = depCode;
            txt_DepName.Text = depName;
            txt_AccCode.Text = accCode;
            check_Active.Checked = isActive;

            // Asign head of deartment to list (list_HOD)
            dtHOD = dep.DbExecuteQuery(string.Format("SELECT LoginName FROM [ADMIN].vHeadOfDepartment WHERE ISNULL(LoginName,'') <> '' AND DepCode = '{0}'", depCode), null, LoginInfo.ConnStr);

            list_HOD.Items.Clear();
            BindListHOD(dtHOD);


            // Popup - Department Detail
            pop_Department.HeaderText = "Edit";
            pop_Department.ShowOnPageLoad = true;

        }

        protected void btn_AddUser_Click(Object sender, EventArgs e)
        {
            string sql = "SELECT DISTINCT LoginName FROM [ADMIN].UserRole";

            dtUserList = dep.DbExecuteQuery(sql, null, hf_ConnStr.Value);

            // Remove LoginName that is exist in this department (HOD)
            if (dtHOD != null)
            {
                foreach (DataRow dr in dtHOD.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        var rows = dtUserList.Select(string.Format("LoginName = '{0}'", dr["LoginName"].ToString()));
                        foreach (var row in rows)
                            row.Delete();
                    }
                }
            }

            list_SelectUser.Items.Clear();
            list_SelectUser.DataSource = dtUserList;
            list_SelectUser.DataBind();

            pop_UserList.ShowOnPageLoad = true;
        }

        protected void btn_DelUser_Click(Object sender, EventArgs e)
        {
            foreach (ListEditItem item in list_HOD.Items)
            {
                if (item.Selected)
                {
                    var rows = dtHOD.Select(string.Format("LoginName='{0}'", item.Text));
                    foreach (var row in rows)
                        row.Delete();
                }
            }

            BindListHOD(dtHOD);
        }

        protected void btn_SaveDep_Click(Object sender, EventArgs e)
        {
            string depCode = txt_DepCode.Text.ToUpper().Trim();
            string depName = txt_DepName.Text.Trim();
            string accCode = txt_AccCode.Text;
            string isActive = check_Active.Checked == true ? "1" : "0";

            string message = CheckDepCode(depCode);
            if (message != string.Empty)
            {
                lbl_Alert.Text = message;
                pop_Alert.ShowOnPageLoad = true;
            }
            else
            {
                // HOD
                List<string> list = new List<string>();
                foreach (ListEditItem item in list_HOD.Items)
                {
                    if (item.Text.Trim() != string.Empty)
                        list.Add(item.Text.Trim());
                }
                string hod = String.Join(",", list);

                string sql = string.Format(@"UPDATE [ADMIN].Department 
                                        SET 
                                            DepCode = '{0}',
                                            DepName = '{1}',
                                            IsActive = {2},
                                            HeadOfDep = '{3}',
                                            AccDepCode = '{4}'
                                        WHERE DepCode = '{0}'", depCode, depName, isActive, hod, accCode);

                if (txt_DepCode.Enabled) // Insert
                {
                    if (depName == string.Empty)
                        depName = depCode;

                    sql = string.Format(@"INSERT INTO [ADMIN].Department (DepCode, DepName, IsActive, HeadOfDep, AccDepCode, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)
                                        VALUES('{0}','{1}',{2},'{3}','{4}','{5}','{6}','{7}','{8}')",
                                            depCode,
                                            depName,
                                            isActive,
                                            hod,
                                            accCode,
                                            ServerDateTime.ToString("yyyy-MM-dd hh:mm:ss"),
                                            LoginInfo.LoginName,
                                            ServerDateTime.ToString("yyyy-MM-dd hh:mm:ss"),
                                            LoginInfo.LoginName);
                }


                dep.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

                pop_Department.ShowOnPageLoad = false;
                Page.Response.Redirect(Page.Request.Url.ToString(), true);
            }
        }

        protected void btn_CancelDep_Click(Object sender, EventArgs e)
        {
            pop_Department.ShowOnPageLoad = false;
        }

        protected void btn_Select_UserList_Click(Object sender, EventArgs e)
        {
            foreach (ListEditItem item in list_SelectUser.Items)
            {
                if (item.Selected)
                {
                    dtHOD.Rows.Add(item.Value);
                }
            }

            BindListHOD(dtHOD);
            pop_UserList.ShowOnPageLoad = false;
        }

        protected void btn_Cancel_UserList_Click(Object sender, EventArgs e)
        {
            BindListHOD(dtHOD);
            pop_UserList.ShowOnPageLoad = false;
        }

        private void BindListHOD(DataTable dt)
        {
            list_HOD.Items.Clear();
            list_HOD.DataSource = dt;
            list_HOD.DataBind();
        }

        private string CheckDepCode(string depCode)
        {
            string message = string.Empty;

            if (txt_DepCode.Enabled)
            {
                // DepCode is empty.
                if (string.IsNullOrEmpty(depCode))
                    message = "Department Code is required.";
                else if (string.IsNullOrWhiteSpace(depCode))
                    message = "Department Code is invalid format, space between code is not allowed.";
                else
                {
                    // Existing DepCode
                    string sql = string.Format("SELECT COUNT(*) as RecordCount FROM [ADMIN].Department WHERE DepCode = '{0}'", depCode);
                    DataTable dt = dep.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
                    int recordCount = Convert.ToInt32(dt.Rows[0][0]);

                    if (recordCount > 0)
                        message = "Duplicate department code!";
                }

            }

            return message;
        }
    }
}