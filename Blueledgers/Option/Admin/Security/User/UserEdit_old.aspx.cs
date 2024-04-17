using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Admin.Security.User
{
    public partial class UserEdit : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.dbo.BUUser buUser = new Blue.BL.dbo.BUUser();
        private readonly Blue.BL.Option.Admin.Security.Role role = new Blue.BL.Option.Admin.Security.Role();
        private readonly Blue.BL.dbo.User user = new Blue.BL.dbo.User();
        private readonly Blue.BL.Option.Admin.Security.UserRole userRole = new Blue.BL.Option.Admin.Security.UserRole();
        private Blue.BL.ADMIN.Department Department = new Blue.BL.ADMIN.Department();
        private DataSet dsUser = new DataSet();

        #endregion

        #region "Operations"

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
            hf_ConnStr.Value = LoginInfo.ConnStr;

            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                // Get User Structure
                var getUserSchema = user.GetSchema(dsUser);

                if (getUserSchema)
                {
                    // Insert new row
                    var drUser = dsUser.Tables[user.TableName].NewRow();
                    drUser["Password"] = Blue.BL.GnxLib.EnDecryptString(Blue.BL.GnxLib.GenPassword(),
                        Blue.BL.GnxLib.EnDeCryptor.EnCrypt);
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

                // Get BuUser
                var getBuUser = buUser.GetList(dsUser, Request.Params["ID"]);

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
            }

            // Display User data.
            Session["dsUser"] = dsUser;

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

                txt_LoginName.Text = drUser["LoginName"].ToString();
                txt_LoginName.ReadOnly = (Request.Params["MODE"].ToUpper() == "NEW" ? false : true);
                chk_IsActive.Checked = bool.Parse(drUser["IsActived"].ToString());

                if (drUser["LastLogin"] != DBNull.Value)
                {
                    txt_LastLogin.Text =
                        DateTime.Parse(drUser["LastLogin"].ToString())
                            .ToString(LoginInfo.BuFmtInfo.FmtSDate + " " + LoginInfo.BuFmtInfo.FmtSTime);
                }

                txt_FName.Text = drUser["FName"].ToString();
                txt_MName.Text = drUser["MName"].ToString();
                txt_LName.Text = drUser["LName"].ToString();
                txt_Email.Text = drUser["Email"].ToString();
                cmb_Department.Value = drUser["DepartmentCode"];
                txt_JobTitle.Text = drUser["JobTiTle"].ToString();

                if (Request.Params["MODE"].ToUpper() == "EDIT")
                {
                    lbl_Password_Nm.Visible = false;
                    lbl_ConfirmPass.Visible = false;
                    txt_Pwd.Visible = false;
                    txt_PwdConfirm.Visible = false;

                    //txt_Pwd.Enabled         = false;
                    //txt_PwdConfirm.Enabled  = false;
                }

                //ddl_DepartmentCode.Value    = drUser["DepartmentCode"].ToString();
                //ddl_DivisionCode.Value      = drUser["DivisionCode"].ToString();
                //ddl_SectionCode.Value       = drUser["SectionCode"].ToString();                
            }
        }

        /// <summary>
        ///     Commandbar Click
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
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
            }
        }

        /// <summary>
        ///     Commit changed to Database.
        /// </summary>
        private void Save()
        {
            // Update User Data.
            var drUser = dsUser.Tables[user.TableName].Rows[0];
            drUser["LoginName"] = txt_LoginName.Text.Trim();
            drUser["FName"] = txt_FName.Text.Trim();
            drUser["MName"] = txt_MName.Text.Trim();
            drUser["LName"] = txt_LName.Text.Trim();
            drUser["Email"] = txt_Email.Text.Trim();
            //drUser["DepartmentCode"] = cmb_Department.Value.ToString();
            var value_dep = cmb_Department.Value.ToString();
            var value = value_dep.Split('-');

            drUser["DepartmentCode"] = value[0];
            drUser["JobTitle"] = txt_JobTitle.Text.Trim();

            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                if (txt_Pwd.Text.Trim().ToUpper() == txt_PwdConfirm.Text.Trim().ToUpper())
                {
                    drUser["Password"] = Blue.BL.GnxLib.EnDecryptString(txt_Pwd.Text.Trim(),
                        Blue.BL.GnxLib.EnDeCryptor.EnCrypt);
                }
                else
                {
                    pop_PwdConfirm.ShowOnPageLoad = true;
                    return;
                }
            }

            //if (ddl_DepartmentCode.SelectedItem != null)
            //{
            //    drUser["DepartmentCode"] = ddl_DepartmentCode.SelectedItem.Value.ToString();
            //}
            //else
            //{
            //    drUser["DepartmentCode"] = DBNull.Value;
            //}

            //if (ddl_DivisionCode.SelectedItem != null)
            //{
            //    drUser["DivisionCode"] = ddl_DivisionCode.SelectedItem.Value;
            //}
            //else
            //{
            //    drUser["DivisionCode"] = DBNull.Value;
            //}

            //if (ddl_SectionCode.SelectedItem != null)
            //{
            //    drUser["SectionCode"] = ddl_SectionCode.SelectedItem.Value;
            //}
            //else
            //{
            //    drUser["SectionCode"] = DBNull.Value;
            //}

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
                Response.Redirect("User.aspx?ID=" + drUser["LoginName"]);
            }
        }

        /// <summary>
        ///     Redirec to UserList.aspx or User.aspx page.
        /// </summary>
        private void Back()
        {
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                Response.Redirect("UserList.aspx");
            }
            else
            {
                Response.Redirect("User.aspx?ID=" + Request.Params["ID"]);
            }
        }

        #endregion

        #region "Popup Show Generated Password for New User"

        /// <summary>
        ///     Redirect to User.aspx page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ShowPassword_OK_Click(object sender, EventArgs e)
        {
            // Send email to created user to confirm user name and password
            var body = " LoginName : " + txt_LoginName.Text + "\n FirstName : " + txt_FName.Text + "\n Password : " +
                       lbl_NewPassword.Text + "\n LoginPage: http://localhost/blueledgers/Login.aspx";
            var receivers = txt_Email.Text;
            var subjects = "Confirm UserName and Password";
            Blue.BL.GnxLib.SendMail(receivers, subjects, body);

            // Redirect to user detail page.
            Response.Redirect("User.aspx?ID=" + dsUser.Tables[user.TableName].Rows[0]["LoginName"]);
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
    }
}