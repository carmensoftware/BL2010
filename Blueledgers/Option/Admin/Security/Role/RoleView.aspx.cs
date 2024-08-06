using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using BlueLedger.PL.BaseClass;
using Blue.DAL;
using System.Data.SqlClient;

namespace BlueLedger.PL.Option.Admin.Security.Role
{
    public partial class RoleView : BasePage
    {
        private readonly Blue.BL.APP.Module module = new Blue.BL.APP.Module();
        private readonly Blue.BL.Option.Admin.Security.Role role = new Blue.BL.Option.Admin.Security.Role();
        private readonly Blue.BL.ADMIN.RolePermission rolePermission = new Blue.BL.ADMIN.RolePermission();
        private readonly Blue.BL.dbo.User user = new Blue.BL.dbo.User();
        private readonly Blue.BL.Option.Admin.Security.UserRole userRole = new Blue.BL.Option.Admin.Security.UserRole();

        private DataSet dsRole = new DataSet();
        private DataSet dsModule = new DataSet();

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Session["dsRole"] = null;
                Session["dsModule"] = null;
                Page_Retrieve();
            }
            else
            {
                dsRole = (DataSet)Session["dsRole"];
                dsModule = (DataSet)Session["dsModule"];
            }
        }

        /// <summary>
        ///     Gets role data.
        /// </summary>
        private void Page_Retrieve()
        {
            dsModule.Clear();
            dsRole.Clear();

            gv_App.DataSource = module.GetListModule(LoginInfo.ConnStr);
            gv_App.DataBind();


            var id = Request.Params["ID"];
            if (id == null)
            {
                #region Create/New
                // New Role : Get Role Schema.
                var getRoleSchema = role.GetSchema(dsRole, LoginInfo.ConnStr);
                if (getRoleSchema)
                {
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
                #endregion
            }
            else // Edit
            {
                var roleName = id;
                roleName = Request.Params["ID"];

                if (!role.Get(dsRole, roleName, LoginInfo.ConnStr))
                {
                    // Display Error Message
                    ShowMessage("Warning", "Invalid role.");
                    return;
                }

                if (!userRole.GetListByRoleName(dsRole, roleName, LoginInfo.ConnStr))
                {
                    // Display Error Message
                    ShowMessage("Warning", "Invalid user and role.");
                    return;
                }

                // Get RolePermission Data
                if (!rolePermission.GetList(dsRole, roleName, LoginInfo.ConnStr))
                {
                    // Display Error Message
                    ShowMessage("Warning", "Invalid role permission.");
                    return;
                }

            }

            #region Old ver.
            //if (Request.Params["MODE"] != null)
            //{
            //    if (Request.Params["MODE"].ToUpper() == "NEW")
            //    {
            //        // New Role : Get Role Schema.
            //        var getRoleSchema = role.GetSchema(dsRole, LoginInfo.ConnStr);

            //        if (getRoleSchema)
            //        {
            //            menu_CmdBar0.Items.FindByName("Create").Visible = false;

            //            // Insert new row to prepare user key-in
            //            var drRole = dsRole.Tables[role.TableName].NewRow();
            //            drRole["IsActive"] = true;
            //            drRole["CreatedDate"] = ServerDateTime;
            //            drRole["CreatedBy"] = LoginInfo.LoginName;
            //            drRole["UpdatedDate"] = ServerDateTime;
            //            drRole["UpdatedBy"] = LoginInfo.LoginName;
            //            dsRole.Tables[role.TableName].Rows.Add(drRole);

            //            Session["dsRole"] = dsRole;

            //            // Prepare UserRole data for new role
            //            // Get UserRole Schema
            //            var getUserRoleSchema = userRole.GetSchema(dsRole, LoginInfo.ConnStr);

            //            if (!getUserRoleSchema)
            //            {
            //                // Display Error Message
            //                ShowMessage("Warning", "Problem found with ADMIN.UserRole");
            //                return;
            //            }

            //            // Get User data
            //            var getUser = user.GetList(dsRole, LoginInfo.BuInfo.BuCode);

            //            if (!getUser)
            //            {
            //                // Display Error Message
            //                ShowMessage("Warning", "Problem found with user list");
            //                return;
            //            }

            //            // Insert UserRole data
            //            foreach (DataRow drUser in dsRole.Tables[user.TableName].Rows)
            //            {
            //                var drUserRole = dsRole.Tables[userRole.TableName].NewRow();
            //                drUserRole["LoginName"] = drUser["LoginName"].ToString();
            //                drUserRole["RoleName"] = string.Empty;
            //                drUserRole["IsActive"] = false;
            //                dsRole.Tables[userRole.TableName].Rows.Add(drUserRole);
            //            }
            //        }
            //        else
            //        {
            //            // Display Error Message
            //            ShowMessage("Warning", "Problem found with ADMIN.Role");
            //            return;
            //        }

            //    }
            //    else
            //    {
            //        if (Request.Params["ID"] != null)
            //        {

            //        }
            //        else
            //        {
            //            // Display Error Message
            //            ShowMessage("Warning", "Invalid ID.");
            //            return;
            //        }

            //    }

            //}
            //else // Create
            //{
            //}
            #endregion

            Get_ProdCateTypeByRole();
            Session["dsRole"] = dsRole;
            Page_Setting();
        }

        private void Page_Setting()
        {
            btn_RoleType.Visible = false;

            gv_ProdType.DataSource = Get_ProdCateType();
            gv_ProdType.DataBind();


            // Note: This part is never been 0.
            // Modified on: 02/10/2017, By: Fon
            if (dsRole.Tables[role.TableName].Rows.Count > 0)
            {
                var drRole = dsRole.Tables[role.TableName].Rows[0];
                txt_RoleDesc.Text = drRole["RoleDesc"].ToString();
                chk_IsActive.Checked = bool.Parse(drRole["IsActive"].ToString());
            }
        }

        protected void btn_RoleType_Click(object sender, EventArgs e)
        {
            if (IsSelectedModule())
            {
                gv_ProdType.DataSource = Get_ProdCateType();
                gv_ProdType.DataBind();

                pop_SetProdType.ShowOnPageLoad = true;
            }
            else
                ShowMessage("Avaliable", "Please select .");
        }

        #region menu_Item
        protected void menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":
                    Save();
                    break;

                case "BACK":
                    Back();
                    break;

                case "DELETE":
                    pop_ConfrimDelete.ShowOnPageLoad = true;
                    break;

                case "PRINT":
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    break;

            }
        }

        protected void Save()
        {
            SaveProdCateTypeToDataTable();

            // Added on: 27/09/2017, By: Fon
            // Note: Follow from role.aspx

            #region Header
            var drRole = dsRole.Tables[role.TableName].Rows[0];

            //Modified on: 2017/05/12, By: Fon, About: RoleName & RoleDesc
            if (drRole["RoleName"].ToString() == string.Empty)
            {
                // Create new id
                drRole["RoleName"] = GenerateNewID();
            }
            drRole["RoleDesc"] = txt_RoleDesc.Text.Trim();
            drRole["IsActive"] = chk_IsActive.Checked;
            drRole["UpdatedDate"] = ServerDateTime;
            drRole["UpdatedBy"] = LoginInfo.LoginName;

            // Update UserRole data with new RoleName
            foreach (DataRow drUserRole in dsRole.Tables[userRole.TableName].Rows)
            {
                drUserRole["RoleName"] = drRole["RoleName"].ToString();
            }
            role.Save(dsRole, LoginInfo.ConnStr);
            #endregion

            #region Detail
            #region MODE: NEW
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                var getRolePermission = rolePermission.GetList(dsRole, string.Empty, LoginInfo.ConnStr);
                if (!getRolePermission)
                { return; }
            }
            #endregion
            #region MODE: Edit
            else if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                //dsRole.Tables[rolePermission.TableName].Rows.Clear();
                //dsRole.Tables[rolePermission.TableName].AcceptChanges();

                // Note: Rows.Clear(), cannot delete for DataAccessLayer
                foreach (DataRow drRolePermiss in dsRole.Tables[rolePermission.TableName].Rows)
                { drRolePermiss.Delete(); }
            }
            #endregion

            // Insert new RolePermission Data.
            DataTable dt = Get_SelectedMenu();
            foreach (DataRow dr in dt.Rows)
            {
                var drNewRolePermission = dsRole.Tables[rolePermission.TableName].NewRow();
                drNewRolePermission["RoleName"] = dsRole.Tables[role.TableName].Rows[0]["RoleName"].ToString();
                drNewRolePermission["ModuleID"] = dr["ModuleID"];
                drNewRolePermission["Permission"] = dr["Permission"];
                dsRole.Tables[rolePermission.TableName].Rows.Add(drNewRolePermission);
            }
            #endregion

            // Save Change to RolePermission Table.
            //var deleteRolePermission = rolePermission.Delete(dsRole, LoginInfo.ConnStr);

            // Note: Now, we have delete [rolePermission] function but. Save function is one stop service. 
            var saveRolePermission = rolePermission.Save(dsRole, LoginInfo.ConnStr);
            string saveRoleType = string.Empty;
            if (dsRole.Tables["UpdatedProdCatedType"] != null) saveRoleType = SaveProdCateTypeToDataBase();
            if (saveRolePermission && (saveRoleType == string.Empty))
            {
                // Note: If U don't have parent, U will not see even 1 menu.
                Save_ParentByLoginName(LoginInfo.LoginName);

                // Display Successfully Message
                pop_RolePermissionSaveSuccess.ShowOnPageLoad = true;
            }



            // End Added.
        }

        protected void Back()
        {
            Response.Redirect("RoleList.aspx");
        }

        protected void Delete()
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
        #endregion

        #region About GridView
        protected void gv_App_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Set Module(s)
                if (e.Row.FindControl("lbl_Module") != null)
                {
                    var label = (Label)e.Row.FindControl("lbl_Module");
                    label.Text = DataBinder.Eval(e.Row.DataItem, "Desc").ToString();
                }



                var gv = (GridView)e.Row.FindControl("gv_Module");
                if (gv != null)
                {
                    string id = DataBinder.Eval(e.Row.DataItem, "ID").ToString();
                    gv.DataSource = module.GetList2(id, LoginInfo.ConnStr);
                    gv.DataBind();
                }
            }
        }

        protected void gv_Module_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Label lbl_Module = (Label)e.Row.FindControl("lbl_Module");
                //Label lbl_SubModule = (Label)e.Row.FindControl("lbl_SubModule");
                CheckBox chkView = (CheckBox)e.Row.FindControl("chkView");
                CheckBox chkEdit = (CheckBox)e.Row.FindControl("chkEdit");
                CheckBox chkDelete = (CheckBox)e.Row.FindControl("chkDelete");

                // Added on: 27/09/2017, By: Fon
                string moduleID = string.Empty;
                int permissValue = 0;
                if (e.Row.FindControl("lbl_ID") != null)
                {
                    Label lbl_ID = (Label)e.Row.FindControl("lbl_ID");
                    moduleID = string.Format("{0}", DataBinder.Eval(e.Row.DataItem, "ID"));
                    permissValue = rolePermission.GetPermission(Request.Params["ID"], moduleID, LoginInfo.ConnStr);
                    lbl_ID.Text = moduleID;
                }

                if (e.Row.FindControl("lbl_Desc") != null)
                {
                    Label lbl_Desc = (Label)e.Row.FindControl("lbl_Desc");
                    lbl_Desc.Text = string.Format("{0}", DataBinder.Eval(e.Row.DataItem, "Desc"));
                }

                if (e.Row.FindControl("lbl_Parent") != null)
                {
                    Label lbl_Parent = (Label)e.Row.FindControl("lbl_Parent");
                    lbl_Parent.Text = string.Format("{0}", DataBinder.Eval(e.Row.DataItem, "PArent"));
                }

                if (e.Row.FindControl("chkView") != null
                    && e.Row.FindControl("chkEdit") != null
                    && e.Row.FindControl("chkDelete") != null)
                {
                    Control_CheckBoxPermission(permissValue, e);
                }
                // End Added.


                string id = DataBinder.Eval(e.Row.DataItem, "ID").ToString();
                GridView gv = e.Row.FindControl("gv_SubModule") as GridView;
                var dt = module.GetList2(id, LoginInfo.ConnStr);

                Panel panel = (Panel)e.Row.FindControl("pn_SubModule");
                panel.Visible = dt.Rows.Count > 0;

                gv.DataSource = dt;
                gv.DataBind();
            }

        }

        protected void gv_SubModule_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Added on: 27/09/2017, By: Fonstring moduleID = string.Empty;
                string moduleID = string.Empty;
                int permissValue = 0;
                if (e.Row.FindControl("lbl_ID") != null)
                {
                    Label lbl_ID = (Label)e.Row.FindControl("lbl_ID");
                    moduleID = string.Format("{0}", DataBinder.Eval(e.Row.DataItem, "ID"));
                    permissValue = rolePermission.GetPermission(Request.Params["ID"], moduleID, LoginInfo.ConnStr);
                    lbl_ID.Text = moduleID;
                }

                if (e.Row.FindControl("lbl_Desc") != null)
                {
                    Label lbl_Desc = (Label)e.Row.FindControl("lbl_Desc");
                    lbl_Desc.Text = string.Format("{0}", DataBinder.Eval(e.Row.DataItem, "Desc"));
                }

                if (e.Row.FindControl("lbl_Parent") != null)
                {
                    Label lbl_Parent = (Label)e.Row.FindControl("lbl_Parent");
                    lbl_Parent.Text = string.Format("{0}", DataBinder.Eval(e.Row.DataItem, "PArent"));
                }

                if (e.Row.FindControl("chkView") != null
                    && e.Row.FindControl("chkEdit") != null
                    && e.Row.FindControl("chkDelete") != null)
                {
                    Control_CheckBoxPermission(permissValue, e);
                }
                // End Added.
            }
        }

        protected void Control_CheckBoxPermission(int permission, GridViewRowEventArgs e)
        {
            // Note: Delete | Create | View
            // Ex. 8 = 111 = true | true | true
            int noOfChk = 3;
            string binPermiss = Convert.ToString(permission, 2);
            string completeBin = string.Empty;
            for (int i = (noOfChk - 1) - binPermiss.Length; i >= 0; i--) completeBin += "0";
            completeBin = completeBin + binPermiss;

            //lbl_test.Text = string.Format("Ori Input: {0}, After Convert: {1}", permission, completeBin);
            //lbl_test.Text += string.Format("<br/> Binary Len: {0}", completeBin.Length);

            for (int i = completeBin.Length - 1; i >= 0; i--)
            {
                bool boolean = (completeBin[i].ToString() == "1") ? true : false;
                if ((i == (completeBin.Length - 1)) && (e.Row.FindControl("chkView") != null))
                {
                    CheckBox chkView = (CheckBox)e.Row.FindControl("chkView");
                    chkView.Checked = boolean;
                    //chkView.Checked = true;
                }

                if ((i == (completeBin.Length - 2)) && (e.Row.FindControl("chkEdit") != null))
                {
                    CheckBox chkEdit = (CheckBox)e.Row.FindControl("chkEdit");
                    chkEdit.Checked = boolean;
                    //chkEdit.Checked = true;
                }

                if ((i == (completeBin.Length - 3)) && (e.Row.FindControl("chkDelete") != null))
                {
                    CheckBox chkDelete = (CheckBox)e.Row.FindControl("chkDelete");
                    chkDelete.Checked = boolean;
                    //chkDelete.Checked = true;
                }
            }
        }

        protected void chkView_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chk.NamingContainer;
            GridView gv = (GridView)row.Parent.NamingContainer;
            int rowIndex = row.RowIndex;

            if (!chk.Checked)
            {
                CheckBox chkEdit = (CheckBox)gv.Rows[rowIndex].FindControl("chkEdit");
                CheckBox chkDelete = (CheckBox)gv.Rows[rowIndex].FindControl("chkDelete");

                chkEdit.Checked = false;
                chkDelete.Checked = false;
            }

            // Added on: 27/09/2017, By: Fon
            // Note: BeCareFul, both gv_Module & gv_SubModule are use this method;
            #region gv_SubModule == null
            if (row.FindControl("gv_SubModule") == null)
            {
                Control ctr = gv.NamingContainer;
                if (ctr is GridViewRow)
                {
                    CheckBox chkViewP = (CheckBox)ctr.FindControl("chkView");
                    CheckBox chkEditP = (CheckBox)ctr.FindControl("chkEdit");
                    CheckBox chkDeleteP = (CheckBox)ctr.FindControl("chkDelete");
                    chkViewP.Checked = true;

                    bool statusViewP = false;
                    bool statusEditP = false;
                    bool statusDelP = false;
                    if (ctr.FindControl("gv_SubModule") != null)
                    {
                        GridView gv_SubModule = (GridView)ctr.FindControl("gv_SubModule");
                        foreach (GridViewRow gvr in gv_SubModule.Rows)
                        {
                            CheckBox chk_SubView = (CheckBox)gvr.FindControl("chkView");
                            if (chk_SubView.Checked) statusViewP = true;

                            CheckBox chk_SubEdit = (CheckBox)gvr.FindControl("chkEdit");
                            if (chk_SubEdit.Checked) statusEditP = true;

                            CheckBox chk_SubDel = (CheckBox)gvr.FindControl("chkDelete");
                            if (chk_SubDel.Checked) statusDelP = true;

                            if (statusViewP && statusEditP && statusDelP) break;

                        }
                    }
                    chkViewP.Checked = statusViewP;
                    chkEditP.Checked = statusEditP;
                    chkDeleteP.Checked = statusDelP;
                }

            }
            #endregion

            if (row.FindControl("gv_SubModule") != null)
            {
                GridView gv_SubModule = (GridView)row.FindControl("gv_SubModule");
                if (gv_SubModule.Rows.Count > 0)
                {
                    foreach (GridViewRow gvr in gv_SubModule.Rows)
                    {
                        CheckBox chk_SubView = (CheckBox)gvr.FindControl("chkView");
                        chk_SubView.Checked = chk.Checked;

                        if (!chk.Checked)
                        {
                            CheckBox chkEdit = (CheckBox)gvr.FindControl("chkEdit");
                            CheckBox chkDelete = (CheckBox)gvr.FindControl("chkDelete");

                            chkEdit.Checked = false;
                            chkDelete.Checked = false;
                        }
                    }
                }

            }
            // End Added.
        }

        protected void chkEdit_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chk.NamingContainer;
            GridView gv = (GridView)row.Parent.NamingContainer;
            int rowIndex = row.RowIndex;

            CheckBox chkView = (CheckBox)gv.Rows[rowIndex].FindControl("chkView");
            CheckBox chkDelete = (CheckBox)gv.Rows[rowIndex].FindControl("chkDelete");
            if (chk.Checked) chkView.Checked = true;
            else chkDelete.Checked = false;

            // Added on: 27/09/2017, By: Fon
            // Note: BeCareFul, both gv_Module & gv_SubModule are use this method;
            #region gv_SubModule == null
            if (row.FindControl("gv_SubModule") == null)
            {
                Control ctr = gv.NamingContainer;
                if (ctr is GridViewRow)
                {
                    CheckBox chkViewP = (CheckBox)ctr.FindControl("chkView");
                    CheckBox chkEditP = (CheckBox)ctr.FindControl("chkEdit");
                    CheckBox chkDeleteP = (CheckBox)ctr.FindControl("chkDelete");
                    if (chk.Checked)
                    {
                        chkViewP.Checked = true;
                        chkEditP.Checked = true;
                    }

                    bool statusEditP = false;
                    bool statusDelP = false;
                    if (ctr.FindControl("gv_SubModule") != null)
                    {
                        GridView gv_SubModule = (GridView)ctr.FindControl("gv_SubModule");
                        foreach (GridViewRow gvr in gv_SubModule.Rows)
                        {
                            CheckBox chk_SubEdit = (CheckBox)gvr.FindControl("chkEdit");
                            if (chk_SubEdit.Checked) statusEditP = true;

                            CheckBox chk_SubDel = (CheckBox)gvr.FindControl("chkDelete");
                            if (chk_SubDel.Checked) statusDelP = true;

                            if (statusEditP && statusDelP) break;
                        }
                    }
                    chkEditP.Checked = statusEditP;
                    chkDeleteP.Checked = statusDelP;
                }
            }
            #endregion

            if (row.FindControl("gv_SubModule") != null)
            {
                GridView gv_SubModule = (GridView)row.FindControl("gv_SubModule");
                if (gv_SubModule.Rows.Count > 0)
                {
                    foreach (GridViewRow gvr in gv_SubModule.Rows)
                    {
                        CheckBox chk_SubView = (CheckBox)gvr.FindControl("chkView");
                        CheckBox chk_SubEdit = (CheckBox)gvr.FindControl("chkEdit");
                        CheckBox chk_SubDel = (CheckBox)gvr.FindControl("chkDelete");

                        if (!chk.Checked) chk_SubDel.Checked = false;
                        if (chk.Checked) chk_SubView.Checked = true;
                        chk_SubEdit.Checked = chk.Checked;
                    }
                }
            }
            // End Added.
        }

        protected void chkDelete_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chk.NamingContainer;
            GridView gv = (GridView)row.Parent.NamingContainer;
            int rowIndex = row.RowIndex;

            if (chk.Checked)
            {
                CheckBox chkView = (CheckBox)gv.Rows[rowIndex].FindControl("chkView");
                CheckBox chkEdit = (CheckBox)gv.Rows[rowIndex].FindControl("chkEdit");
                chkView.Checked = true;
                chkEdit.Checked = true;
            }

            // Added on: 27/09/2017, By: Fon
            // Note: BeCareFul, both gv_Module & gv_SubModule are use this method;
            #region gv_SubModule == null
            if (row.FindControl("gv_SubModule") == null)
            {
                // This mean they have parent.
                Control ctr = gv.NamingContainer;
                if (ctr is GridViewRow)
                {
                    CheckBox chkViewP = (CheckBox)ctr.FindControl("chkView");
                    CheckBox chkEditP = (CheckBox)ctr.FindControl("chkEdit");
                    CheckBox chkDeleteP = (CheckBox)ctr.FindControl("chkDelete");
                    if (chk.Checked)
                    {
                        chkDeleteP.Checked = chk.Checked;
                        chkViewP.Checked = true;
                        chkEditP.Checked = true;
                    }

                    bool statusDelP = false;
                    if (ctr.FindControl("gv_SubModule") != null)
                    {
                        GridView gv_SubModule = (GridView)ctr.FindControl("gv_SubModule");
                        foreach (GridViewRow gvr in gv_SubModule.Rows)
                        {
                            CheckBox chk_SubDel = (CheckBox)gvr.FindControl("chkDelete");
                            if (chk_SubDel.Checked)
                            {
                                statusDelP = true;
                                break;
                            }
                        }
                    }
                    chkDeleteP.Checked = statusDelP;
                }
            }
            #endregion

            if (row.FindControl("gv_SubModule") != null)
            {
                GridView gv_SubModule = (GridView)row.FindControl("gv_SubModule");
                if (gv_SubModule.Rows.Count > 0)
                {
                    foreach (GridViewRow gvr in gv_SubModule.Rows)
                    {
                        CheckBox chk_SubDel = (CheckBox)gvr.FindControl("chkDelete");
                        CheckBox chk_SubView = (CheckBox)gvr.FindControl("chkView");
                        CheckBox chk_SubEdit = (CheckBox)gvr.FindControl("chkEdit");
                        chk_SubDel.Checked = chk.Checked;
                        if (chk_SubDel.Checked)
                        {
                            chk_SubView.Checked = true;
                            chk_SubEdit.Checked = true;
                        }
                    }
                } // End If gv_SubModule.Row > 0
            }
        }
        #endregion

        #region Function
        protected string GenerateNewID()
        {
            // Modified on: 2017-07-06, By: Fon
            // Sql start = count letter that U want, not index.
            string newID = string.Empty;
            //string sqlCmd = "SELECT 'RoleID_' + CONVERT( nvarchar(10),";
            //sqlCmd += " CAST( SUBSTRING( ISNULL(MAX([RoleName]), '0'), 8, 8)AS int) + 1 )";
            //sqlCmd += " AS [NewID] FROM [ADMIN].[Role]";
            //sqlCmd += " WHERE [RoleName] like 'RoleID_%'";
            string sqlCmd = ";WITH r AS(SELECT CAST(SUBSTRING(RoleName,8,10) AS INT) as id FROM [ADMIN].[Role] WHERE [RoleName] like 'RoleID_%') SELECT 'RoleID_' + CAST(ISNULL(MAX(id),0) + 1 AS VARCHAR) as [NewID] FROM r";
            SqlConnection conn = new SqlConnection(LoginInfo.ConnStr);
            conn.Open();

            SqlCommand cmd = new SqlCommand(sqlCmd, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();

            return newID = dt.Rows[0]["NewID"].ToString();
        }

        protected int Calculate_PermissionFromChk(GridViewRow gvr)
        {
            int permiss = 0;
            // Note:  Delete = 2^2 | Create/Edit = 2^1 | View = 2^0

            if (gvr.FindControl("chkView") != null)
            {
                CheckBox chkView = (CheckBox)gvr.FindControl("chkView");
                if (chkView.Checked) permiss += Convert.ToInt32(Math.Round(Math.Pow(2, 0), 0));
            }

            if (gvr.FindControl("chkEdit") != null)
            {
                CheckBox chkEdit = (CheckBox)gvr.FindControl("chkEdit");
                if (chkEdit.Checked) permiss += Convert.ToInt32(Math.Round(Math.Pow(2, 1), 0));
            }

            if (gvr.FindControl("chkDelete") != null)
            {
                CheckBox chkDelete = (CheckBox)gvr.FindControl("chkDelete");
                if (chkDelete.Checked) permiss += Convert.ToInt32(Math.Round(Math.Pow(2, 2), 0));
            }

            return permiss;
        }

        protected bool IsSelectedModule()
        {
            foreach (GridViewRow gvr in gv_App.Rows)
            {
                if (gvr.FindControl("gv_Module") != null)
                {
                    GridView gv_Module = (GridView)gvr.FindControl("gv_Module");
                    foreach (GridViewRow gvr_M in gv_Module.Rows)
                    {
                        if (gvr_M.FindControl("chkView") != null)
                        {
                            CheckBox chkView = (CheckBox)gvr_M.FindControl("chkView");
                            if (chkView.Checked) return true;
                        }

                        if (gvr_M.FindControl("gv_SubModule") != null)
                        {
                            GridView gv_SubModule = (GridView)gvr_M.FindControl("gv_SubModule");
                            foreach (GridViewRow gvr_SM in gv_SubModule.Rows)
                            {
                                if (gvr_SM.FindControl("chkView") != null)
                                {
                                    CheckBox chkView = (CheckBox)gvr_SM.FindControl("chkView");
                                    if (chkView.Checked) return true;
                                }
                            } // End foreach gv_SubModule
                        }
                    } // End foreach gv_Module
                }
            }

            return false;
        }

        // Added on: 05/01/2018, By: Fon
        protected bool Save_ParentByLoginName(string loginName)
        {
            // Note: module.GetRoot() = [dbo].[APP_Module_GetRoot_LoginName] is not exist.
            DataTable dtP = module.GetRoot2(loginName, LoginInfo.ConnStr);
            foreach (DataRow dr in dtP.Rows)
            {
                var drP = dsRole.Tables[rolePermission.TableName].NewRow();
                drP["RoleName"] = dsRole.Tables[role.TableName].Rows[0]["RoleName"].ToString();
                drP["ModuleID"] = dr["ID"];
                drP["Permission"] = 1; // 1: View
                dsRole.Tables[rolePermission.TableName].Rows.Add(drP);
            }
            return rolePermission.Save(dsRole, LoginInfo.ConnStr);
        }
        // End Added.

        protected DataTable Get_ProdCateType()
        {
            string cmdStr = string.Format(@"EXEC [IN].[ProdCateType_GetActivedList]");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(LoginInfo.ConnStr);

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(cmdStr, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            { return null; }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        protected DataTable Get_ProdCateTypeByRole()
        {
            string cmdStr = string.Format(@"SELECT rt.*, pct.[Description] 
                    FROM [IN].[RoleType] rt
                    INNER JOIN [IN].[ProdCateType] pct ON pct.[Code] = rt.ProdCateTypeCode
                    WHERE rt.RoleName = '{0}' ", dsRole.Tables[role.TableName].Rows[0]["RoleName"]);
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(LoginInfo.ConnStr);

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(cmdStr, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        protected DataTable Get_SelectedMenu()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ModuleID", typeof(string));
            dt.Columns.Add("Permission", typeof(int));

            foreach (GridViewRow gvr in gv_App.Rows)
            {
                if (gvr.FindControl("gv_Module") != null)
                {
                    GridView gv_Module = (GridView)gvr.FindControl("gv_Module");
                    foreach (GridViewRow gvr_Module in gv_Module.Rows)
                    {
                        if (gvr_Module.FindControl("chkView") != null)
                        {
                            CheckBox chkView = (CheckBox)gvr_Module.FindControl("chkView");
                            if (chkView.Checked)
                                dt = PerpareDataForSelectedRow(dt, gvr_Module);
                        }

                        if (gvr_Module.FindControl("gv_SubModule") != null)
                        {
                            GridView gv_SubModule = (GridView)gvr_Module.FindControl("gv_SubModule");
                            foreach (GridViewRow gvr_SubModle in gv_SubModule.Rows)
                            {
                                if (gvr_SubModle.FindControl("chkView") != null)
                                {
                                    CheckBox chkView = (CheckBox)gvr_SubModle.FindControl("chkView");
                                    if (chkView.Checked)
                                        dt = PerpareDataForSelectedRow(dt, gvr_SubModle);
                                }
                            }
                        }

                    }
                    // End Foreach
                }
            }
            return dt;
        }

        protected DataTable PerpareDataForSelectedRow(DataTable dt, GridViewRow gvr)
        {
            Label lbl_ID = (Label)gvr.FindControl("lbl_ID");
            int permiss = Calculate_PermissionFromChk(gvr);
            DataRow dr = dt.NewRow();
            dr["ModuleID"] = lbl_ID.Text;
            dr["Permission"] = permiss;
            dt.Rows.Add(dr);
            return dt;
        }

        protected void SaveProdCateTypeToDataTable()
        {
            DataTable dt = new DataTable();
            dt = Get_ProdCateTypeByRole().Copy();
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
                        dr["RoleName"] = dsRole.Tables[role.TableName].Rows[0]["RoleName"];
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

        protected string SaveProdCateTypeToDataBase()
        {
            SqlConnection con = new SqlConnection(LoginInfo.ConnStr);
            con.Open();

            try
            {
                string sqlStrDelete = string.Format(@" DELETE FROM [IN].[RoleType]
                    WHERE [RoleName] = '{0}'", dsRole.Tables[role.TableName].Rows[0]["RoleName"]);

                SqlCommand cmd = new SqlCommand(sqlStrDelete, con);
                cmd.ExecuteNonQuery();
                con.Close();

                foreach (DataRow dr in dsRole.Tables["UpdatedProdCatedType"].Rows)
                {
                    con.Open();
                    string sqlInsert = string.Format(@" INSERT INTO [IN].[RoleType] 
                        VALUES( @RoleName, @TypeCode)");

                    cmd = new SqlCommand(sqlInsert, con);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@RoleName", dr["RoleName"]);
                    cmd.Parameters.AddWithValue("@TypeCode", dr["ProdCateTypeCode"]);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Pop-up
        private void ShowMessage(string headerText, string message)
        {
            pop_Message.HeaderText = headerText;
            pop_lbl_message.Text = message;
            pop_Message.ShowOnPageLoad = true;
        }

        protected void pop_btn_OK_ToCloseClick(object sender, EventArgs e)
        {
            pop_Message.ShowOnPageLoad = false;
        }

        protected void btn_OK_SaveSuccess_Click(object sender, EventArgs e)
        {
            Response.Redirect("RoleList.aspx");
        }

        protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        {
            Delete();
        }

        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        protected void gv_ProdType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int prodCateType = 0;
                if (e.Row.FindControl("lbl_TypeCode") != null)
                {
                    Label lbl_TypeCode = (Label)e.Row.FindControl("lbl_TypeCode");
                    lbl_TypeCode.Text = DataBinder.Eval(e.Row.DataItem, "Code").ToString();
                    prodCateType = int.Parse(lbl_TypeCode.Text);
                }

                if (e.Row.FindControl("cb_ProdType") != null)
                {
                    CheckBox cb_ProdType = (CheckBox)e.Row.FindControl("cb_ProdType");
                    DataTable dtSelect = new DataTable();
                    if (dsRole.Tables["UpdatedProdCatedType"] == null) dtSelect = Get_ProdCateTypeByRole();
                    else dtSelect = dsRole.Tables["UpdatedProdCatedType"];

                    cb_ProdType.Checked
                            = (dtSelect.Select("[ProdCateTypeCode] = '" + prodCateType + "'").Length > 0)
                            ? true : false;
                }
            }
        }

        protected void btn_OKSetProdType_Click(object sender, EventArgs e)
        {
            SaveProdCateTypeToDataTable();
            pop_SetProdType.ShowOnPageLoad = false;
        }
        protected void btn_CancelProdType_Click(object sender, EventArgs e)
        {
            pop_SetProdType.ShowOnPageLoad = false;
        }
        #endregion
    }
}

