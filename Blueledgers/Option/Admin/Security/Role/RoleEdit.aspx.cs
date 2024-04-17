using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Admin.Security.Role
{
    public partial class RoleEdit : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Admin.Security.Role role = new Blue.BL.Option.Admin.Security.Role();
        private readonly Blue.BL.dbo.User user = new Blue.BL.dbo.User();
        private readonly Blue.BL.Option.Admin.Security.UserRole userRole = new Blue.BL.Option.Admin.Security.UserRole();

        private DataSet dsRole = new DataSet();

        #endregion

        #region "Operations"

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
                dsRole = (DataSet) Session["dsRole"];
            }
        }

        /// <summary>
        ///     Gets role data.
        /// </summary>
        private void Page_Retrieve()
        {
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
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
            }
            else
            {
                // Edit Role : Get Role Data.
                var result = role.Get(dsRole, Request.Params["ID"], LoginInfo.ConnStr);

                if (result)
                {
                    Session["dsRole"] = dsRole;
                }
                else
                {
                    // Display Error Message
                    return;
                }
            }

            // Display Role and Role Permission Data.
            Page_Setting();
        }

        /// <summary>
        ///     Display role data.
        /// </summary>
        private void Page_Setting()
        {
            if (dsRole.Tables[role.TableName].Rows.Count > 0)
            {
                var drRole = dsRole.Tables[role.TableName].Rows[0];
                txt_RoleName.Text = drRole["RoleName"].ToString();
                txt_RoleName.ReadOnly = (Request.Params["MODE"].ToUpper() == "NEW" ? false : true);
                chk_IsActive.Checked = bool.Parse(drRole["IsActive"].ToString());
                txt_CreatedDate.Text =
                    DateTime.Parse(drRole["CreatedDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate
                                                                              + " " + LoginInfo.BuFmtInfo.FmtSTime);
                txt_CreatedBy.Text = drRole["CreatedBy"].ToString();
                txt_UpdatedDate.Text =
                    DateTime.Parse(drRole["UpdatedDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate
                                                                              + " " + LoginInfo.BuFmtInfo.FmtSTime);
                txt_UpdatedBy.Text = drRole["UpdatedBy"].ToString();
            }
        }

        /// <summary>
        ///     Commandbar Click.
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
        ///     Commit changed to DataBase.
        ///     Redirect to Role.aspx page and send "ID=[RoleName]" as parameter.
        /// </summary>
        protected void Save()
        {
            var saved = false;

            // Commit changed
            // Role
            var drRole = dsRole.Tables[role.TableName].Rows[0];
            drRole["RoleName"] = txt_RoleName.Text.Trim();
            drRole["IsActive"] = chk_IsActive.Checked;
            drRole["UpdatedDate"] = ServerDateTime;
            drRole["UpdatedBy"] = LoginInfo.LoginName;

            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                // Update UserRole data with new RoleName
                foreach (DataRow drUserRole in dsRole.Tables[userRole.TableName].Rows)
                {
                    drUserRole["RoleName"] = drRole["RoleName"].ToString();
                }

                saved = role.Save(dsRole, LoginInfo.ConnStr);
            }
            else
            {
                saved = role.Save(dsRole, LoginInfo.ConnStr);
            }

            if (saved)
            {
                Response.Redirect("Role.aspx?ID=" + drRole["RoleName"]);
            }
        }

        /// <summary>
        ///     Redirect to RoleList.aspx page.
        /// </summary>
        protected void Back()
        {
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                Response.Redirect("RoleList.aspx");
            }
            else
            {
                Response.Redirect("Role.aspx?ID=" + Request.Params["ID"]);
            }
        }

        #endregion
    }
}