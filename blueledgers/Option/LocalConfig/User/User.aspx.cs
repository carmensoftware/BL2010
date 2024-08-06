using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Admin.Security.User
{
    public partial class User : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.dbo.BUUser buUser = new Blue.BL.dbo.BUUser();
        private readonly Blue.BL.ADMIN.Department department = new Blue.BL.ADMIN.Department();
        private readonly Blue.BL.Option.Admin.Security.Role role = new Blue.BL.Option.Admin.Security.Role();
        private readonly Blue.BL.Option.Inventory.StoreLct store = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.dbo.User user = new Blue.BL.dbo.User();
        private readonly Blue.BL.Option.Admin.Security.UserRole userRole = new Blue.BL.Option.Admin.Security.UserRole();
        private readonly Blue.BL.ADMIN.UserStore userStore = new Blue.BL.ADMIN.UserStore();
        private DataSet dsUser = new DataSet();
        private string getBuUserError = string.Empty;

        #endregion

        #region "Operations"

        #region "Page Load"

        /// <summary>
        ///     Display all User data.
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
                dsUser = (DataSet) Session["dsUser"];
            }
        }

        /// <summary>
        ///     Get user data.
        /// </summary>
        private void Save()
        {
            // Update User Data.
            if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                var drUser = dsUser.Tables[user.TableName].Rows[0];
                drUser["LoginName"] = txt_LoginName.Text.Trim().ToString();//
                drUser["FName"] = txt_FName.Text.Trim().ToString();//
                drUser["MName"] = txt_MName.Text.Trim().ToString();//
                drUser["LName"] = txt_LName.Text.Trim().ToString();//
                drUser["Email"] = txt_Email.Text.Trim().ToString();//
                drUser["DepartmentCode"] = cmb_Department.Value.ToString();
                var value_dep = cmb_Department.Value.ToString();
                var value = value_dep.Split(':');//

                drUser["DepartmentCode"] = value[0].ToString().Trim();//
                drUser["JobTitle"] = txt_JobTitle.Text.Trim().ToString();//
                drUser["IsActived"] = chk_IsActive.Checked;//

                // Prepare BuUser data before save
                dsUser.Tables[buUser.TableName].Rows[0]["LoginName"] = txt_LoginName.Text.Trim().ToString();//
                dsUser.Tables[buUser.TableName].Rows[0]["Theme"] = "Default";

                // Prepare UserRole data before save
                foreach (DataRow drUserRole in dsUser.Tables[userRole.TableName].Rows)
                {
                    drUserRole["LoginName"] = txt_LoginName.Text.Trim();
                }

                // Commit Changed User Data
                var saveUser = user.Save(dsUser, LoginInfo.ConnStr);

                if (saveUser)
                {
                    //Response.Redirect("User.aspx?MODE=EDIT&ID=" + drUser["LoginName"].ToString());
                }
            }
            else
            {
                var drUser = dsUser.Tables[user.TableName].Rows[0];
                drUser["LoginName"] = txt_LoginName.Text.Trim();
                drUser["FName"] = txt_FName.Text.Trim();
                drUser["MName"] = txt_MName.Text.Trim();
                drUser["LName"] = txt_LName.Text.Trim();
                drUser["Email"] = txt_Email.Text.Trim();
                drUser["Password"] = Blue.BL.GnxLib.EnDecryptString(txt_Pwd0.Text.Trim(),
                    Blue.BL.GnxLib.EnDeCryptor.EnCrypt, Blue.BL.GnxLib.KEY_LOGIN_PASSWORD);

                drUser["DepartmentCode"] = cmb_Department.Value.ToString().Trim();
                var value_dep = cmb_Department.Value.ToString();
                var value = value_dep.Split(' '); // split by SPACE

                drUser["DepartmentCode"] = value[0];
                drUser["JobTitle"] = txt_JobTitle.Text.Trim();
                drUser["IsActived"] = chk_IsActive.Checked;

                // Prepare BuUser data before save
                dsUser.Tables[buUser.TableName].Rows[0]["LoginName"] = txt_LoginName.Text.Trim();
                dsUser.Tables[buUser.TableName].Rows[0]["Theme"] = "Default";

                // Prepare UserRole data before save
                foreach (DataRow drUserRole in dsUser.Tables[userRole.TableName].Rows)
                {
                    drUserRole["LoginName"] = txt_LoginName.Text.Trim();
                }

                // Commit Changed User Data
                var saveUser = user.Save(dsUser, LoginInfo.ConnStr);


                if (saveUser)
                {
                    //Response.Redirect("User.aspx?MODE=EDIT&ID=" + drUser["LoginName"].ToString());
                }
            }
            // Clear all UserRoleData


            foreach (DataRow drUserRole in dsUser.Tables[userRole.TableName].Rows)
            {
                drUserRole["IsActive"] = false;
            }

            //List<object> columnValues = grd_UserRole.GetSelectedFieldValues("RoleName");

            //foreach (string selRole in columnValues)
            //{
            //    foreach (DataRow drUserRole in dsUser.Tables[userRole.TableName].Rows)
            //    {
            //        if (drUserRole["RoleName"].ToString().ToUpper() == selRole.ToUpper())
            //        {
            //            drUserRole["IsActive"] = true;
            //        }
            //    }
            //}

            var columnValues1 = new List<object>();

            foreach (GridViewRow grd_Row in grd_UserRole1.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_ItemR") as CheckBox;
                if (chk_Item.Checked)
                {
                    columnValues1.Add(grd_UserRole1.DataKeys[grd_Row.RowIndex].Value);
                }
            }

            foreach (string selRole in columnValues1)
            {
                foreach (DataRow drUserRole in dsUser.Tables[userRole.TableName].Rows)
                {
                    if (drUserRole["RoleName"].ToString().ToUpper() == selRole.ToUpper())
                    {
                        drUserRole["IsActive"] = true;
                    }
                }
            }

            // Save change to database
            var saveUserRole = userRole.Save(dsUser, LoginInfo.ConnStr);

            if (saveUserRole)
            {
                //this.Page_Retrieve();

                //// Display Successfully Message
                pop_SavedUserRole.ShowOnPageLoad = true;
            }
            // Get UserStore data.
            var getUserStore = userStore.GetList(dsUser, string.Empty, LoginInfo.ConnStr);

            if (!getUserStore)
            {
                // Display Error Message
                return;
            }
            //// Delete all UserStore
            foreach (DataRow drUserStore in dsUser.Tables[userStore.TableName].Rows)
            {
                drUserStore.Delete();
            }


            //List<object> columnValues = grd_UserStore.GetSelectedFieldValues("LocationCode");

            //foreach (string selStore in columnValues)
            //{
            //    DataRow drNew = dsUser.Tables[userStore.TableName].NewRow();
            //    drNew["LoginName"]      = dsUser.Tables[user.TableName].Rows[0]["LoginName"].ToString();
            //    drNew["LocationCode"]   = selStore;
            //    dsUser.Tables[userStore.TableName].Rows.Add(drNew);
            //}

            var columnValues2 = new List<object>();

            foreach (GridViewRow grd_Row in grd_UserStore1.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_ItemS") as CheckBox;
                if (chk_Item.Checked)
                {
                    columnValues2.Add(grd_UserStore1.DataKeys[grd_Row.RowIndex].Value);
                }
            }

            foreach (string selStore in columnValues2)
            {
                var drNew = dsUser.Tables[userStore.TableName].NewRow();
                drNew["LoginName"] = dsUser.Tables[user.TableName].Rows[0]["LoginName"].ToString();
                drNew["LocationCode"] = selStore;
                dsUser.Tables[userStore.TableName].Rows.Add(drNew);
            }

            // Save Changed
            var saveUserStore = userStore.Save(dsUser, LoginInfo.ConnStr);

            if (saveUserStore)
            {
                Page_Retrieve();
                pop_SavedUserStore.ShowOnPageLoad = true;
            }

            Response.Redirect("UserList.aspx?MODE=EDIT&ID=");
        }

        private void If(bool p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Redirec to UserList.aspx or User.aspx page.
        /// </summary>
        private void Page_Retrieve()
        {
            dsUser.Clear();

            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                // Get User Structure
                var getUserSchema = user.GetSchema(dsUser);

                if (getUserSchema)
                {
                    // Insert new row
                    var drUser = dsUser.Tables[user.TableName].NewRow();
                    drUser["Password"] = Blue.BL.GnxLib.EnDecryptString(Blue.BL.GnxLib.GenPassword(),
                        Blue.BL.GnxLib.EnDeCryptor.EnCrypt, Blue.BL.GnxLib.KEY_LOGIN_PASSWORD);
                    drUser["IsActived"] = true;
                    dsUser.Tables[user.TableName].Rows.Add(drUser);
                }
                else
                {
                    // Display Error Message
                    return;
                }

                // Get BuUser Structure
                var getBuUserSchema = buUser.GetSchema(dsUser);

                if (getBuUserSchema)
                {
                    // Prepare BuUser data for new user
                    var drBuUser = dsUser.Tables[buUser.TableName].NewRow();
                    drBuUser["BuCode"] = LoginInfo.BuInfo.BuCode;
                    drBuUser["LoginName"] = string.Empty;
                    dsUser.Tables[buUser.TableName].Rows.Add(drBuUser);
                }
                else
                {
                    // Display Error Message
                    return;
                }

                // Get UserRole Structure with UserRole data for new user
                var getUerRoleSchema = userRole.GetSchema(dsUser, LoginInfo.ConnStr);

                if (getUerRoleSchema)
                {
                    // Prepare UserRole data for new user
                    var getRole = role.GetList(dsUser, LoginInfo.ConnStr);

                    if (getRole)
                    {
                        foreach (DataRow drRole in dsUser.Tables[role.TableName].Rows)
                        {
                            var drUserRole = dsUser.Tables[userRole.TableName].NewRow();
                            drUserRole["LoginName"] = string.Empty;
                            drUserRole["RoleName"] = drRole["RoleName"].ToString();
                            drUserRole["IsActive"] = false;
                            dsUser.Tables[userRole.TableName].Rows.Add(drUserRole);
                        }
                    }
                    else
                    {
                        // Display Error Message
                        return;
                    }
                }
                else
                {
                    // Display Error Message
                    return;
                }

                // Get Store data.
                var getStore = store.GetList(dsUser, LoginInfo.ConnStr);

                if (!getStore)
                {
                    // Display Error Message
                    return;
                }
            }
            else
            {
                // Get User data.
                var getUser = user.Get(dsUser, Request.Params["ID"]);


                if (!getUser)
                {
                    // Display Error Message
                    return;
                }

                // Get BuUser data            
                var getBuUser = buUser.Get(dsUser, LoginInfo.BuInfo.BuCode, Request.Params["ID"], ref getBuUserError);

                if (!getBuUser)
                {
                    // Display Error Message
                    return;
                }

                // Get UserRole data.
                var getUserRole = userRole.GetList(dsUser, Request.Params["ID"], LoginInfo.ConnStr);

                if (!getUserRole)
                {
                    // Display Error Message
                    return;
                }

                // Get Store data.
                var getStore = store.GetList(dsUser, LoginInfo.ConnStr);

                if (!getStore)
                {
                    // Display Error Message
                    return;
                }

                // Get UserStore data.
                var getUserStore = userStore.GetList(dsUser, Request.Params["ID"], LoginInfo.ConnStr);

                if (!getUserStore)
                {
                    // Display Error Message
                    return;
                }
            }


            Session["dsUser"] = dsUser;

            // Display User data.
            Page_Setting();
        }

        /// <summary>
        ///     Display user data.
        /// </summary>
        private void Page_Setting()
        {
            hf_ConnStr.Value = LoginInfo.ConnStr;

            if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                if (dsUser.Tables[user.TableName].Rows.Count > 0)
                {
                    var drUser = dsUser.Tables[user.TableName].Rows[0];

                    txt_LoginName.Text = drUser["LoginName"].ToString();
                    txt_LoginName.Enabled = false;
                    chk_IsActive.Checked = (bool.Parse(drUser["IsActived"].ToString()));
                    //(bool.Parse(drUser["IsActived"].ToString()) ? "Active" : "Inactive");
                    if (drUser["LastLogin"] != DBNull.Value)
                    {
                        txt_LastLogin.Text = DateTime.Parse(drUser["LastLogin"].ToString()).ToString(
                            LoginInfo.BuFmtInfo.FmtSDate + " " + LoginInfo.BuFmtInfo.FmtSTime);
                    }

                    txt_FName.Text = drUser["FName"].ToString();
                    txt_MName.Text = drUser["MName"].ToString();
                    txt_LName.Text = drUser["LName"].ToString();
                    //lnk_Email.Text = drUser["Email"].ToString();
                    //lnk_Email.NavigateUrl += lnk_Email.Text;
                    txt_Email.Text = drUser["Email"].ToString();
                    cmb_Department.Text = drUser["DepartmentCode"].ToString()+" : "+department.GetName(drUser["DepartmentCode"].ToString(), LoginInfo.ConnStr);
                    txt_DivisionCode.Text = drUser["DivisionCode"].ToString();
                    txt_SectionCode.Text = drUser["SectionCode"].ToString();
                    txt_JobTitle.Text = drUser["JobTitle"].ToString();

                    // Display UserRole
                    //grd_UserRole.DataSource = dsUser.Tables[userRole.TableName];
                    //grd_UserRole.DataBind();

                    grd_UserRole1.DataSource = dsUser.Tables[userRole.TableName];
                    grd_UserRole1.DataBind();

                    for (var i = 0; i < dsUser.Tables[userRole.TableName].Rows.Count; i++)
                    {
                        if (bool.Parse(dsUser.Tables[userRole.TableName].Rows[i]["IsActive"].ToString()))
                        {
                            //grd_UserRole.Selection.SetSelection(i, true);

                            var chk_Item = grd_UserRole1.Rows[i].FindControl("chk_ItemR") as CheckBox;
                            if (chk_Item != null)
                            {
                                chk_Item.Checked = true;
                            }
                        }
                    }

                    // Display UserStore
                    //grd_UserStore.DataSource = dsUser.Tables[store.TableName];
                    //grd_UserStore.DataBind();

                    grd_UserStore1.DataSource = dsUser.Tables[store.TableName];
                    grd_UserStore1.DataBind();

                    for (var i = 0; i < dsUser.Tables[store.TableName].Rows.Count; i++)
                    {
                        for (var j = 0; j < dsUser.Tables[userStore.TableName].Rows.Count; j++)
                        {
                            if (dsUser.Tables[store.TableName].Rows[i]["LocationCode"].ToString().ToUpper() ==
                                dsUser.Tables[userStore.TableName].Rows[j]["LocationCode"].ToString().ToUpper())
                            {
                                //grd_UserStore.Selection.SetSelection(i, true);

                                var chk_Item = grd_UserStore1.Rows[i].FindControl("chk_ItemS") as CheckBox;
                                if (chk_Item != null)
                                {
                                    chk_Item.Checked = true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //lnk_Email.Visible = false;
                txt_Email.Visible = true;
                txt_LoginName.Enabled = true;
                lbl_Password_Nm.Visible = true;
                txt_Pwd0.Visible = true;
                lbl_ConfirmPass.Visible = true;
                txt_PwdConfirm0.Visible = true;
                grd_UserRole1.DataSource = dsUser.Tables[userRole.TableName];
                grd_UserRole1.DataBind();
                grd_UserStore1.DataSource = dsUser.Tables[store.TableName];
                grd_UserStore1.DataBind();
            }
        }

        #endregion

        #region "Commandbar"

        /// <summary>
        ///     Commandbar Click
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                    //case "CREATE" :
                    //    this.Create();
                    //    break;

                case "SAVE":
                    Save();
                    break;

                case "RESET":
                    ResetPwd();
                    break;

                case "DELETE":
                    Delete();
                    break;

                case "PRINT":
                    break;

                case "BACK":
                    Back();
                    break;
            }
        }

        /// <summary>
        ///     Redirect to UserEdit.aspx page.
        /// </summary>
        private void Create()
        {
            // Check Maximum User Number.
            // Not allow to create user if the number of user in the business unit is equal to maximum.
            var buLicense = new Blue.BL.dbo.BuLicense();
            var buUser = new Blue.BL.dbo.BUUser();

            if (buUser.GetUserNo(LoginInfo.BuInfo.BuCode) == buLicense.GetMaxUser(LoginInfo.BuInfo.BuCode))
            {
                // Display Error Message
                pop_ReachMaxUserNo.ShowOnPageLoad = true;
            }
            else
            {
                Response.Redirect("UserEdit.aspx?MODE=NEW");
            }
        }

        /// <summary>
        ///     Redirect to UserEdit.aspx page and send "MODE=EDIT&ID=[LoginName]" as parameter
        /// </summary>
        private void Edit()
        {
            Response.Redirect("UserEdit.aspx?MODE=EDIT&ID=" + Request.Params["ID"]);
        }

        /// <summary>
        ///     Reset password
        /// </summary>
        private void ResetPwd()
        {
            pop_ResetPwd.ShowOnPageLoad = true;
        }

        /// <summary>
        ///     Reset password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ResetPwd_Yes_Click(object sender, EventArgs e)
        {
            // Gen new password
            //string newPassword = Blue.BL.GnxLib.GenPassword();

            if (txt_Pwd.Text.Trim().ToUpper() == txt_PwdConfirm.Text.Trim().ToUpper())
            {
                // Update new password
                dsUser.Tables[user.TableName].Rows[0]["Password"] = Blue.BL.GnxLib.EnDecryptString(txt_Pwd.Text.Trim(),
                    Blue.BL.GnxLib.EnDeCryptor.EnCrypt, Blue.BL.GnxLib.KEY_LOGIN_PASSWORD);

                // Commit changed to database
                var saveUser = user.Save(dsUser);

                if (saveUser)
                {
                    pop_ResetPwd.ShowOnPageLoad = false;
                    pop_UpdatedNewPwd.ShowOnPageLoad = true;

                    //Send mail to confirm new password
                    //string body         = " LoginName : " + lbl_LoginName.Text + "\n Password : " + lbl_NewPassword.Text + "\n LoginPage: http://localhost/blueledgers/Login.aspx";
                    //string receivers    = lnk_Email.Text;
                    //string subjects     = "Confirm New Password";
                    //GnxLib.SendMail(receivers, subjects, body);
                }
            }
            else
            {
                pop_PwdConfirm.ShowOnPageLoad = true;
            }
        }

        protected void btn_ResetPwd_Close_Click(object sender, EventArgs e)
        {
            pop_ResetPwd.ShowOnPageLoad = false;
        }

        /// <summary>
        ///     Display Confrim Delete
        /// </summary>
        private void Delete()
        {
            pop_ConfrimDelete.ShowOnPageLoad = true;
        }

        /// <summary>
        ///     Delete User
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        {
            // Delete UserRole data.
            foreach (DataRow drUserRole in dsUser.Tables[userRole.TableName].Rows)
            {
                drUserRole.Delete();
            }

            // Delete UserStore
            foreach (DataRow drUserStore in dsUser.Tables[userStore.TableName].Rows)
            {
                drUserStore.Delete();
            }

            // Delete BuUser data.
            dsUser.Tables[buUser.TableName].Rows[0].Delete();

            // Delete User data.            
            dsUser.Tables[user.TableName].Rows[0].Delete();

            var delUser = user.Delete(dsUser, LoginInfo.ConnStr);

            if (delUser)
            {
                // Delete Successfully, redirect to UserList.aspx page.
                Response.Redirect("UserList.aspx");
            }
        }

        /// <summary>
        ///     Cancel Delete user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        /// <summary>
        ///     Redirec to UserList.aspx page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back()
        {
            Response.Redirect("UserList.aspx");
        }

        #endregion

        #region "Role"

        /// <summary>
        ///     Rebinding UserRole
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_UserRole_Load(object sender, EventArgs e)
        {
            //grd_UserRole.DataSource = dsUser.Tables[userRole.TableName];
            //grd_UserRole.DataBind();
        }

        /// <summary>
        ///     Save UserRole
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_Role_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":

                    //// Clear all UserRoleData
                    //foreach (DataRow drUserRole in dsUser.Tables[userRole.TableName].Rows)
                    //{
                    //    drUserRole["IsActive"] = false;
                    //}

                    ////List<object> columnValues = grd_UserRole.GetSelectedFieldValues("RoleName");

                    ////foreach (string selRole in columnValues)
                    ////{
                    ////    foreach (DataRow drUserRole in dsUser.Tables[userRole.TableName].Rows)
                    ////    {
                    ////        if (drUserRole["RoleName"].ToString().ToUpper() == selRole.ToUpper())
                    ////        {
                    ////            drUserRole["IsActive"] = true;
                    ////        }
                    ////    }
                    ////}

                    //List<object> columnValues1 = new List<object>();

                    //foreach (GridViewRow grd_Row in grd_UserRole1.Rows)
                    //{
                    //    CheckBox chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;
                    //    if (chk_Item.Checked)
                    //    {
                    //        columnValues1.Add(grd_UserRole1.DataKeys[grd_Row.RowIndex].Value);
                    //    }
                    //}

                    //foreach (string selRole in columnValues1)
                    //{
                    //    foreach (DataRow drUserRole in dsUser.Tables[userRole.TableName].Rows)
                    //    {
                    //        if (drUserRole["RoleName"].ToString().ToUpper() == selRole.ToUpper())
                    //        {
                    //            drUserRole["IsActive"] = true;
                    //        }
                    //    }
                    //}

                    //// Save change to database
                    //bool saveUserRole = userRole.Save(dsUser, LoginInfo.ConnStr);

                    //if (saveUserRole)
                    //{
                    //    this.Page_Retrieve();

                    //    // Display Successfully Message
                    //    pop_SavedUserRole.ShowOnPageLoad = true;
                    //}

                    break;
            }
        }

        #endregion

        #region "Store"

        /// <summary>
        ///     Rebinding UserStore List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_UserStore_Load(object sender, EventArgs e)
        {
            //grd_UserStore.DataSource = dsUser.Tables[store.TableName];
            //grd_UserStore.DataBind();
        }

        /// <summary>
        ///     Save User's Store Changed.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_Store_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":

                    //// Delete all UserStore
                    //    foreach (DataRow drUserStore in dsUser.Tables[userStore.TableName].Rows)
                    //    {
                    //        drUserStore.Delete();
                    //    }

                    //    //List<object> columnValues = grd_UserStore.GetSelectedFieldValues("LocationCode");

                    //    //foreach (string selStore in columnValues)
                    //    //{
                    //    //    DataRow drNew = dsUser.Tables[userStore.TableName].NewRow();
                    //    //    drNew["LoginName"]      = dsUser.Tables[user.TableName].Rows[0]["LoginName"].ToString();
                    //    //    drNew["LocationCode"]   = selStore;
                    //    //    dsUser.Tables[userStore.TableName].Rows.Add(drNew);
                    //    //}

                    //    List<object> columnValues1 = new List<object>();

                    //    foreach (GridViewRow grd_Row in grd_UserStore1.Rows)
                    //    {
                    //        CheckBox chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;
                    //        if (chk_Item.Checked)
                    //        {
                    //            columnValues1.Add(grd_UserStore1.DataKeys[grd_Row.RowIndex].Value);
                    //        }
                    //    }

                    //    foreach (string selStore in columnValues1)
                    //    {
                    //        DataRow drNew = dsUser.Tables[userStore.TableName].NewRow();
                    //        drNew["LoginName"] = dsUser.Tables[user.TableName].Rows[0]["LoginName"].ToString();
                    //        drNew["LocationCode"] = selStore;
                    //        dsUser.Tables[userStore.TableName].Rows.Add(drNew);
                    //    }

                    //    // Save Changed
                    //    bool saveUserStore = userStore.Save(dsUser, LoginInfo.ConnStr);

                    //    if (saveUserStore)
                    //    {
                    //        this.Page_Retrieve();
                    //        pop_SavedUserStore.ShowOnPageLoad = true;
                    //    }

                    break;
            }
        }

        #endregion

        #region "pop_PwdConfirm"

        /// <summary>
        ///     Hide popup confirmed error.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_PwdConfirm_OK_Click(object sender, EventArgs e)
        {
            pop_PwdConfirm.ShowOnPageLoad = false;
        }

        #endregion

        #endregion

        protected void grd_UserRole1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void grd_UserStore1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void txt_LoginName_TextChanged(object sender, EventArgs e)
        {
        }

        protected void grd_UserRole1_RowCreated(object sender, GridViewRowEventArgs e)
        {
        }
    }
}