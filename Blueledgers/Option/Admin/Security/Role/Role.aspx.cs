using System;
using System.Data;
using BlueLedger.PL.BaseClass;
using System.Web.UI;

using System.Data.SqlClient;
using System.Web.UI.WebControls;


namespace BlueLedger.PL.Option.Admin.Security.Role
{
    public partial class Role : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.APP.Module module = new Blue.BL.APP.Module();
        private readonly Blue.BL.Option.Admin.Security.Role role = new Blue.BL.Option.Admin.Security.Role();
        private readonly Blue.BL.ADMIN.RolePermission rolePermission = new Blue.BL.ADMIN.RolePermission();
        private readonly Blue.BL.dbo.User user = new Blue.BL.dbo.User();
        private readonly Blue.BL.Option.Admin.Security.UserRole userRole = new Blue.BL.Option.Admin.Security.UserRole();

        private DataSet dsRole = new DataSet();
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.Option.Inventory.ProdCateType ProdCateType =
            new Blue.BL.Option.Inventory.ProdCateType();
        #endregion

        #region "Operations"

        #region "Page Load"

        /// <summary>
        ///     Gets role data and display.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsRole = (DataSet)Session["dsRole"];
            }
        }

        /// <summary>
        ///     Gets role data.
        /// </summary>
        private void Page_Retrieve()
        {
            dsRole.Clear();

            var roleName = string.Empty;
            if (Request.Params["MODE"] != null)
            {
                #region  Mode: New
                if (Request.Params["MODE"].ToUpper() == "NEW")
                {
                    // New Role : Get Role Schema.
                    var getRoleSchema = role.GetSchema(dsRole, LoginInfo.ConnStr);

                    if (getRoleSchema)
                    {
                        menu_CmdBar0.Items.FindByName("Create").Visible = false;
                        //dsRole.Tables["UserProdCateTypeByLoginName"].Clear();

                        // Insert new row to prepare user key-in
                        var drRole = dsRole.Tables[role.TableName].NewRow();
                        drRole["IsActive"] = true;
                        drRole["CreatedDate"] = ServerDateTime;
                        drRole["CreatedBy"] = LoginInfo.LoginName;
                        drRole["UpdatedDate"] = ServerDateTime;
                        drRole["UpdatedBy"] = LoginInfo.LoginName;
                        dsRole.Tables[role.TableName].Rows.Add(drRole);

                        Session["dsRole"] = dsRole;

                        // Prepare UserRole data for new role
                        // Get UserRole Schema
                        var getUserRoleSchema = userRole.GetSchema(dsRole, LoginInfo.ConnStr);

                        if (!getUserRoleSchema)
                        {
                            // Display Error Message
                            return;
                        }

                        // Get User data
                        var getUser = user.GetList(dsRole, LoginInfo.BuInfo.BuCode);

                        if (!getUser)
                        {
                            // Display Error Message
                            return;
                        }

                        // Insert UserRole data
                        foreach (DataRow drUser in dsRole.Tables[user.TableName].Rows)
                        {
                            var drUserRole = dsRole.Tables[userRole.TableName].NewRow();
                            drUserRole["LoginName"] = drUser["LoginName"].ToString();
                            drUserRole["RoleName"] = string.Empty;
                            drUserRole["IsActive"] = false;
                            dsRole.Tables[userRole.TableName].Rows.Add(drUserRole);
                        }
                    }
                    else
                    {
                        // Display Error Message
                        return;
                    }
                }
                #endregion
                #region Mode: Edit
                else
                {
                    if (Request.Params["ID"] != null)
                    {
                        roleName = Request.Params["ID"];
                    }

                    var getRole = role.Get(dsRole, roleName, LoginInfo.ConnStr);

                    if (!getRole)
                    {
                        // Display Error Message
                        return;
                    }

                    var getUserRole = userRole.GetListByRoleName(dsRole, roleName, LoginInfo.ConnStr);

                    if (!getUserRole)
                    {
                        // Display Error Message
                        return;
                    }

                    // Get RolePermission Data
                    var getRolePermission = rolePermission.GetList(dsRole, roleName, LoginInfo.ConnStr);

                    if (!getRolePermission)
                    {
                        // Display Error Message
                        lbl_Test.Text = "GetRolePerMission: " + getRolePermission;
                        return;
                    }

                }
                #endregion
            }
            GetUserProdCateType();
            GetUserProdCateTypeByRole();
            Session["dsRole"] = dsRole;

            // Display Role and Role Permission Data.
            Page_Setting();
        }

        /// <summary>
        ///     Display role data.
        /// </summary>
        private void Page_Setting()
        {
            if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                if (dsRole.Tables[role.TableName].Rows.Count > 0)
                {
                    var drRole = dsRole.Tables[role.TableName].Rows[0];
                    //txt_RoleName.Text = drRole["RoleName"].ToString();
                    txt_RoleName.Text = drRole["RoleDesc"].ToString();      //Modified on: 2017/05/12, By: Fon

                    //chk_IsActive.Text           = (bool.Parse(drRole["IsActive"].ToString()) ? "Active" : "Inactive");
                    chk_IsActive.Checked = bool.Parse(drRole["IsActive"].ToString());
                    //txt_CreatedDate.Text =
                    //    DateTime.Parse(drRole["CreatedDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate
                    //                                                              + " " + LoginInfo.BuFmtInfo.FmtSTime);
                    lnk_CreatedBy.Text = drRole["CreatedBy"].ToString();
                    lnk_CreatedBy.NavigateUrl = "mailto:" + drRole["CreatedBy"];
                    //txt_UpdatedDate.Text =
                    //    DateTime.Parse(drRole["UpdatedDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate
                    //                                                              + " " + LoginInfo.BuFmtInfo.FmtSTime);
                    lnk_UpdatedBy.Text = drRole["UpdatedBy"].ToString();
                    lnk_UpdatedBy.NavigateUrl = "mailto:" + drRole["UpdatedBy"];

                    // Display Module List
                    tl_Module.DataSource = module.GetActiveList2(LoginInfo.ConnStr);
                    tl_Module.DataBind();

                    // Assign Module Selected
                    foreach (DataRow drRolePermission in dsRole.Tables[rolePermission.TableName].Rows)
                    {
                        if (tl_Module.FindNodeByKeyValue(drRolePermission["ModuleID"].ToString()) != null)
                        {
                            tl_Module.FindNodeByKeyValue(drRolePermission["ModuleID"].ToString()).Selected = true;
                        }
                    }
                }
            }
            else
            {
                lnk_CreatedBy.Text = LoginInfo.LoginName;
                lnk_UpdatedBy.Text = LoginInfo.LoginName;
                //txt_CreatedDate.Text = ServerDateTime.ToString();
                //txt_UpdatedDate.Text = ServerDateTime.ToString();
            }

            tl_Module.SettingsSelection.Recursive = true;
        }

        #endregion

        #region "Commandbar"

        /// <summary>
        ///     Commandbar Click.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Create();
                    break;

                case "SAVE":
                    Save();
                    break;

                case "EDIT":
                    Edit();
                    break;

                case "DELETE":
                    Delete();
                    break;

                case "PRINT":
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    break;

                case "BACK":
                    Back();
                    break;
            }
        }

        /// <summary>
        ///     Redirect to RoleEdit.aspx page and send "MODE=NEW" as parameter.
        /// </summary>
        protected void Save()
        {
            var saved = false;

            // Commit changed
            // Role
            var drRole = dsRole.Tables[role.TableName].Rows[0];

            //Modified on: 2017/05/12, By: Fon, About: RoleName & RoleDesc
            if (drRole["RoleName"].ToString() == string.Empty)
            {
                // Create new id
                string newID = GenerateNewID();
                drRole["RoleName"] = newID;
            }
            drRole["RoleDesc"] = txt_RoleName.Text.Trim();

            drRole["IsActive"] = chk_IsActive.Checked;
            drRole["UpdatedDate"] = ServerDateTime;
            drRole["UpdatedBy"] = LoginInfo.LoginName;

            //if (Request.Params["MODE"].ToString().ToUpper() == "NEW")
            //{
            //    // Update UserRole data with new RoleName
            foreach (DataRow drUserRole in dsRole.Tables[userRole.TableName].Rows)
            {
                drUserRole["RoleName"] = drRole["RoleName"].ToString();
            }

            saved = role.Save(dsRole, LoginInfo.ConnStr);
            //}
            //else
            //{
            //    saved = role.Save(dsRole, LoginInfo.ConnStr);
            //}

            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                //// Delete all RolePermission Data.
                //foreach (DataRow drRolePermission in dsRole.Tables[rolePermission.TableName].Rows)
                //{
                //    drRolePermission.Delete();
                //}

                // Get RolePermission Data
                var getRolePermission = rolePermission.GetList(dsRole, string.Empty, LoginInfo.ConnStr);

                if (!getRolePermission)
                {
                    // Display Error Message
                    return;
                }

                // Insert new RolePermission Data.
                var selectedNodes = tl_Module.GetSelectedNodes();

                for (var i = 0; i < selectedNodes.Count; i++)
                {
                    var drNewRolePermission = dsRole.Tables[rolePermission.TableName].NewRow();
                    drNewRolePermission["RoleName"] = dsRole.Tables[role.TableName].Rows[0]["RoleName"].ToString();
                    drNewRolePermission["ModuleID"] = selectedNodes[i].GetValue("ID");
                    dsRole.Tables[rolePermission.TableName].Rows.Add(drNewRolePermission);
                }

                // Save Change to RolePermission Table.
                var saveRolePermission = rolePermission.Save(dsRole, LoginInfo.ConnStr);

                if (saveRolePermission)
                {
                    // Refresh all Role Data.
                    Page_Retrieve();

                    // Display Successfully Message
                    pop_RolePermissionSaveSuccess.ShowOnPageLoad = true;
                }


                //  Response.Redirect("Role.aspx?MODE=EDIT&ID=" + drRole["RoleName"].ToString());
            }
            else
            {
                // Delete all RolePermission Data.
                foreach (DataRow drRolePermission in dsRole.Tables[rolePermission.TableName].Rows)
                {
                    drRolePermission.Delete();
                }

                // Insert new RolePermission Data.
                var selectedNodes = tl_Module.GetSelectedNodes();

                for (var i = 0; i < selectedNodes.Count; i++)
                {
                    var drNewRolePermission = dsRole.Tables[rolePermission.TableName].NewRow();
                    drNewRolePermission["RoleName"] = dsRole.Tables[role.TableName].Rows[0]["RoleName"].ToString();
                    drNewRolePermission["ModuleID"] = selectedNodes[i].GetValue("ID");
                    dsRole.Tables[rolePermission.TableName].Rows.Add(drNewRolePermission);
                }

                // Save Change to RolePermission Table.
                var saveRolePermission = rolePermission.Save(dsRole, LoginInfo.ConnStr);
                SaveProdCateTypeToDataBase();

                if (saveRolePermission)
                {
                    // Refresh all Role Data.
                    Page_Retrieve();

                    // Display Successfully Message
                    pop_RolePermissionSaveSuccess.ShowOnPageLoad = true;
                }
            }
        }

        protected void Create()
        {
            Response.Redirect("Role.aspx?MODE=NEW");
        }

        /// <summary>
        ///     Redirect to RoleEdit.aspx page.
        /// </summary>
        protected void Edit()
        {
            Response.Redirect("RoleEdit.aspx?MODE=EDIT&ID=" + Request.Params["ID"]);
        }

        /// <summary>
        ///     Display Confrim Deleting Role Data.
        /// </summary>
        protected void Delete()
        {
            pop_ConfrimDelete.ShowOnPageLoad = true;
        }

        /// <summary>
        ///     Redirect to RoleList.aspx page.
        /// </summary>
        protected void Back()
        {
            Response.Redirect("RoleList.aspx");
        }

        #endregion

        #region "Confirm Delete Popup"

        /// <summary>
        ///     Delete selected Role (also UserRole).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        {
            // Delete UserRole
            foreach (DataRow drUserRole in dsRole.Tables[userRole.TableName].Rows)
            {
                drUserRole.Delete();
            }

            // Delete RolePermission
            foreach (DataRow drRolePermission in dsRole.Tables[rolePermission.TableName].Rows)
            {
                drRolePermission.Delete();
            }

            // Delete Role
            dsRole.Tables[role.TableName].Rows[0].Delete();

            // Commit changed to Database.
            var deleted = role.Delete(dsRole, LoginInfo.ConnStr);

            if (deleted)
            {
                Response.Redirect("RoleList.aspx");
            }
        }

        /// <summary>
        ///     Cancel Deleting Role Data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        #endregion

        #region "TreeView Module"

        /// <summary>
        ///     Rebinding Module List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tl_Module_Load(object sender, EventArgs e)
        {
            tl_Module.DataSource = module.GetActiveList2(LoginInfo.ConnStr);
            tl_Module.DataBind();
        }

        /// <summary>
        ///     Save Changed RolePermission.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_Module_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":

                    // Delete all RolePermission Data.
                    foreach (DataRow drRolePermission in dsRole.Tables[rolePermission.TableName].Rows)
                    {
                        drRolePermission.Delete();
                    }

                    // Insert new RolePermission Data.
                    var selectedNodes = tl_Module.GetSelectedNodes();

                    for (var i = 0; i < selectedNodes.Count; i++)
                    {
                        var drNewRolePermission = dsRole.Tables[rolePermission.TableName].NewRow();
                        drNewRolePermission["RoleName"] = dsRole.Tables[role.TableName].Rows[0]["RoleName"].ToString();
                        drNewRolePermission["ModuleID"] = selectedNodes[i].GetValue("ID");
                        dsRole.Tables[rolePermission.TableName].Rows.Add(drNewRolePermission);
                    }

                    // Save Change to RolePermission Table.
                    var saveRolePermission = rolePermission.Save(dsRole, LoginInfo.ConnStr);
                    // Save Change to [IN].RoleType



                    if (saveRolePermission)
                    {
                        // Refresh all Role Data.
                        Page_Retrieve();

                        // Display Successfully Message
                        pop_RolePermissionSaveSuccess.ShowOnPageLoad = true;
                    }

                    break;
            }
        }

        #endregion

        #region Product Category Type

        protected void GetUserProdCateTypeByRole()
        {
            if (dsRole.Tables["GetUserProdCateTypeByRole"] != null)
                dsRole.Tables.Remove(dsRole.Tables["GetUserProdCateTypeByRole"]);

            DataTable dt = new DataTable("GetUserProdCateTypeByRole");
            SqlConnection con = new SqlConnection(LoginInfo.ConnStr);
            con.Open();
            string sqlStr = "select rt.*, pct.[Description] ";
            sqlStr += "from [IN].[RoleType] rt ";
            sqlStr += "Inner Join [IN].[ProdCateType] pct on pct.[Code] = rt.ProdCateTypeCode ";
            sqlStr += "Where [RoleName] = '" + txt_RoleName.Text + "' ";
            SqlCommand cmd = new SqlCommand(sqlStr, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            dsRole.Tables.Add(dt);
        }

        protected void GetUserProdCateType()
        {
            if (dsRole.Tables["UserProdCateType"] != null)
                dsRole.Tables.Remove(dsRole.Tables["UserProdCateType"]);

            DataTable dt = new DataTable("UserProdCateType");
            SqlConnection con = new SqlConnection(LoginInfo.ConnStr);
            con.Open();
            string sqlStr = "EXEC [IN].[ProdCateType_GetActivedList] ";
            SqlCommand cmd = new SqlCommand(sqlStr, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            dsRole.Tables.Add(dt);
        }

        protected bool ParentNodeChecked()
        {
            bool parentChecked = false;
            var selectedNodes = tl_Module.GetSelectedNodes();
            for (int i = 0; i < selectedNodes.Count; i++)
            {
                string moduleId = selectedNodes[i].GetValue("ID").ToString();
                if (moduleId.Split('.').Length == 1)
                {
                    parentChecked = true;
                    break;
                }
            }

            return parentChecked;
        }

        protected void btn_RoleType_Click(object sender, EventArgs e)
        {

            if (ParentNodeChecked())
            {
                if (dsRole.Tables["GetUserProdCateTypeByRole"] != null)
                {
                    //dsRole.Tables["GetUserProdCateTypeByRole"].Clear();
                    dsRole.Tables.Remove(dsRole.Tables["GetUserProdCateTypeByRole"]);
                }

                if (dsRole.Tables["GetUserProdCateTypeByRole"] == null)
                {
                    GetUserProdCateTypeByRole();
                    gv_ProdType.DataSource = dsRole.Tables["UserProdCateType"];
                    gv_ProdType.DataBind();

                    foreach (GridViewRow gvr in gv_ProdType.Rows)
                    {
                        CheckBox cb_ProdType = new CheckBox();
                        Label lbl_TypeCode = new Label();
                        string prodCode = string.Empty;

                        if (gvr.FindControl("cb_ProdType") != null)
                        {
                            cb_ProdType = (CheckBox)gvr.FindControl("cb_ProdType");
                            lbl_TypeCode = (Label)gvr.FindControl("lbl_TypeCode");
                            prodCode = lbl_TypeCode.Text;

                            if (dsRole.Tables["UpdatedProdCatedType"] == null)
                            {
                                if (dsRole.Tables["GetUserProdCateTypeByRole"].Select("[ProdCateTypeCode] = '" + prodCode + "'").Length != 0)
                                    cb_ProdType.Checked = true;
                                //lbl_Test.Text = "UpdatedProdCatedType == null, ";
                                //lbl_Test.Text += dsRole.Tables["GetUserProdCateTypeByRole"].Rows.Count;
                            }
                            else
                            {
                                if (dsRole.Tables["UpdatedProdCatedType"].Select("[ProdCateTypeCode] = '" + prodCode + "'").Length != 0)
                                    cb_ProdType.Checked = true;
                                //lbl_Test.Text = "UpdatedProdCatedType != null, ";
                                //lbl_Test.Text += dsRole.Tables["UpdatedProdCatedType"].Rows.Count;
                            }

                        }
                    }
                }

                Session["dsRole"] = dsRole;
                pop_SetProdType.ShowOnPageLoad = true;
            }
            else
            {
                lbl_ProdTypeMsg.Text = "Please select at least 1 module.";
                pop_Msg.ShowOnPageLoad = true;
            }
        }

        protected void btn_OKSetProdType_Click(object sender, EventArgs e)
        {
            //Response.Redirect(Request.Url.ToString());

            SaveProdCateTypeToDataTable();
            pop_SetProdType.ShowOnPageLoad = false;
        }

        protected void btn_CancelProdType_Click(object sender, EventArgs e)
        {
            pop_SetProdType.ShowOnPageLoad = false;
        }

        protected void SaveProdCateTypeToDataTable()
        {
            DataTable dt = new DataTable();
            dt = dsRole.Tables["GetUserProdCateTypeByRole"].Copy();
            dt.TableName = "UpdatedProdCatedType";
            dt.Clear();

            DataRow dr;
            foreach (GridViewRow gvr in gv_ProdType.Rows)
            {
                CheckBox cb_ProdType = new CheckBox();
                Label lbl_TypeCode = new Label();
                if (gvr.FindControl("cb_ProdType") != null &&
                    gvr.FindControl("lbl_TypeCode") != null)
                {
                    cb_ProdType = (CheckBox)gvr.FindControl("cb_ProdType");
                    lbl_TypeCode = (Label)gvr.FindControl("lbl_TypeCode");

                    if (cb_ProdType.Checked)
                    {
                        dr = dt.NewRow();
                        dr["RoleName"] = txt_RoleName.Text;
                        dr["ProdCateTypeCode"] = Convert.ToInt32(lbl_TypeCode.Text);
                        dr["Description"] = gvr.Cells[1].Text;

                        dt.Rows.Add(dr);
                    }
                }
            }

            if (dsRole.Tables["UpdatedProdCatedType"] != null)
                dsRole.Tables.Remove(dsRole.Tables["UpdatedProdCatedType"]);

            dsRole.Tables.Add(dt);
            Session["dsRole"] = dsRole;

        }

        protected void SaveProdCateTypeToDataBase()
        {
            SqlConnection con = new SqlConnection(LoginInfo.ConnStr);
            con.Open();

            try
            {
                string sqlStrDelete = "Delete From [IN].[RoleType] ";
                sqlStrDelete += "Where [RoleName] = '" + txt_RoleName.Text + "' ";
                SqlCommand cmd = new SqlCommand(sqlStrDelete, con);
                cmd.ExecuteNonQuery();
                con.Close();

                foreach (DataRow dr in dsRole.Tables["UpdatedProdCatedType"].Rows)
                {
                    con.Open();
                    string sqlInsert = "Insert Into [IN].[RoleType] ";
                    sqlInsert += "Values (@RoleName, @TypeCode) ";

                    cmd = new SqlCommand(sqlInsert, con);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@RoleName", dr["RoleName"]);
                    cmd.Parameters.AddWithValue("@TypeCode", dr["ProdCateTypeCode"]);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);

            }
        }

        protected void gv_ProdType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_TypeCode") != null)
                {
                    Label lbl_TypeCode = (Label)e.Row.FindControl("lbl_TypeCode");
                    lbl_TypeCode.Text = DataBinder.Eval(e.Row.DataItem, "Code").ToString();
                }
            }
        }

        protected void btn_OK_popMsg_Click(object sender, EventArgs e)
        {
            pop_Msg.ShowOnPageLoad = false;
        }

        protected void btn_OK_SaveSuccess_Click(object sender, EventArgs e)
        {
            //pop_RolePermissionSaveSuccess.ShowOnPageLoad = false;
            Response.Redirect("RoleList.aspx");
        }
        #endregion

        protected string GenerateNewID()
        {
            // Modified on: 2017-07-06, By: Fon
            // Sql start = count letter that U want, not index.
            string newID = string.Empty;
            string sqlCmd = "SELECT 'RoleID_' + CONVERT( nvarchar(10),";
            sqlCmd += " CAST( SUBSTRING( ISNULL(MAX([RoleName]), '0'), 8, 8)AS int) + 1 )";
            sqlCmd += " AS [NewID] FROM [ADMIN].[Role]";
            sqlCmd += " WHERE [RoleName] like 'RoleID_%'";
            SqlConnection conn = new SqlConnection(LoginInfo.ConnStr);
            conn.Open();

            SqlCommand cmd = new SqlCommand(sqlCmd, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();

            return newID = dt.Rows[0]["NewID"].ToString();
        }

        #endregion



    }
}