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
        private void Page_Retrieve()
        {
            dsUser.Clear();

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

            Session["dsUser"] = dsUser;

            // Display User data.
            Page_Setting();
        }

        /// <summary>
        ///     Display user data.
        /// </summary>
        private void Page_Setting()
        {
            if (dsUser.Tables[user.TableName].Rows.Count > 0)
            {
                var drUser = dsUser.Tables[user.TableName].Rows[0];

                lbl_LoginName.Text = drUser["LoginName"].ToString();
                lbl_IsActive.Text = (bool.Parse(drUser["IsActived"].ToString()) ? "Active" : "Inactive");

                if (drUser["LastLogin"] != DBNull.Value)
                {
                    lbl_LastLogin.Text = DateTime.Parse(drUser["LastLogin"].ToString()).ToString(
                        LoginInfo.BuFmtInfo.FmtSDate + " " + LoginInfo.BuFmtInfo.FmtSTime);
                }

                lbl_FName.Text = drUser["FName"].ToString();
                lbl_MName.Text = drUser["MName"].ToString();
                lbl_LName.Text = drUser["LName"].ToString();
                lnk_Email.Text = drUser["Email"].ToString();
                lnk_Email.NavigateUrl += lnk_Email.Text;
                lbl_DepartmentCode.Text = department.GetName(drUser["DepartmentCode"].ToString(), LoginInfo.ConnStr);
                lbl_DivisionCode.Text = drUser["DivisionCode"].ToString();
                lbl_SectionCode.Text = drUser["SectionCode"].ToString();
                lbl_JobTitle.Text = drUser["JobTitle"].ToString();

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

                        var chk_Item = grd_UserRole1.Rows[i].FindControl("chk_Item") as CheckBox;
                        chk_Item.Checked = true;
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

                            var chk_Item = grd_UserStore1.Rows[i].FindControl("chk_Item") as CheckBox;
                            chk_Item.Checked = true;
                        }
                    }
                }
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
                case "CREATE":
                    Create();
                    break;

                case "EDIT":
                    Edit();
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
                        var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;
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
                        Page_Retrieve();

                        // Display Successfully Message
                        pop_SavedUserRole.ShowOnPageLoad = true;
                    }

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

                    // Delete all UserStore
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

                    var columnValues1 = new List<object>();

                    foreach (GridViewRow grd_Row in grd_UserStore1.Rows)
                    {
                        var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;
                        if (chk_Item.Checked)
                        {
                            columnValues1.Add(grd_UserStore1.DataKeys[grd_Row.RowIndex].Value);
                        }
                    }

                    foreach (string selStore in columnValues1)
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
    }
}