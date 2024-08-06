using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Admin.Security.Role
{
    public partial class RoleList : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Admin.Security.Role role = new Blue.BL.Option.Admin.Security.Role();
        private readonly Blue.BL.ADMIN.RolePermission rolePermission = new Blue.BL.ADMIN.RolePermission();
        private readonly Blue.BL.Option.Admin.Security.UserRole userRole = new Blue.BL.Option.Admin.Security.UserRole();

        private DataSet dsRole = new DataSet();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Page load event. (Gets/Display Role Data)
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
        ///     Gets Role Data.
        /// </summary>
        private void Page_Retrieve()
        {
            dsRole.Clear();

            var result = role.GetList(dsRole, LoginInfo.ConnStr);

            // Display role data.
            if (result)
            {
                Session["dsRole"] = dsRole;
                Page_Setting();
            }
        }

        /// <summary>
        ///     Displays Role Data.
        /// </summary>
        private void Page_Setting()
        {
            gvRoleList.DataSource = dsRole.Tables[role.TableName];
            gvRoleList.DataBind();
        }

        protected void gvRoleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect("Role.aspx?MODE=EDIT");
        }

        protected void gvRoleList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblStatus = (Label)(e.Row.FindControl("lblStatus"));
            int activestatus = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "IsActive"));
            if (lblStatus != null)
            {
                lblStatus.Text = activestatus.ToString();
            }

            Image imgCheck = (Image)(e.Row.FindControl("imgCheck"));
            if (imgCheck != null)
            {
                if (activestatus == 1)
                {
                    imgCheck.ImageUrl = "~/App_Themes/Default/Images/master/icon/check-icon.png";
                }
                else if (activestatus == 0)
                {
                    //imgCheck.ImageUrl = "~/App_Themes/Default/Images/master/icon/inactive-icon.png";
                    imgCheck.ImageUrl = "~/App_Themes/Default/Images/master/icon/greyCheck.png";
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='#4D4D4D'; this.style.color='white';";
                e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white'; this.style.color='black';";
                //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvRoleList, "Select$" + e.Row.RowIndex);
                e.Row.Attributes.Add("OnClick",
                    "document.location.href='role.aspx?MODE=Edit&ID=" + DataBinder.Eval(e.Row.DataItem, "RoleName") +
                    "'");
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }

        //protected void btnAddUser_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("Role.aspx?MODE=NEW");
        //}

        protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        {
            //List<object> columnValues = grd_Role.GetSelectedFieldValues("RoleName");
            var columnValues = new List<object>();

            foreach (GridViewRow grd_Row in gvRoleList.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;
                if (chk_Item.Checked)
                {
                    columnValues.Add(gvRoleList.DataKeys[grd_Row.RowIndex].Value);
                }
            }

            foreach (string delRoleName in columnValues)
            {
                // Get UserRole data and also delete            
                var getUserRole = userRole.GetListByRoleName(dsRole, delRoleName, LoginInfo.ConnStr);

                if (getUserRole)
                {
                    foreach (DataRow drUserRole in dsRole.Tables[userRole.TableName].Rows)
                    {
                        if (drUserRole.RowState != DataRowState.Deleted)
                        {
                            drUserRole.Delete();
                        }
                    }
                }

                // Get RolePermission data and also delete
                var getRolePermission = rolePermission.GetList(dsRole, delRoleName, LoginInfo.ConnStr);

                if (getRolePermission)
                {
                    foreach (DataRow drRolePermission in dsRole.Tables[rolePermission.TableName].Rows)
                    {
                        if (drRolePermission.RowState != DataRowState.Deleted)
                        {
                            drRolePermission.Delete();
                        }
                    }
                }

                foreach (DataRow drDeleting in dsRole.Tables[role.TableName].Rows)
                {
                    if (drDeleting.RowState != DataRowState.Deleted)
                    {
                        if (drDeleting["RoleName"].ToString().ToUpper() == delRoleName.ToUpper())
                        {
                            drDeleting.Delete();
                        }
                    }
                }
            }

            // Save to database
            var deleteRole = role.Delete(dsRole, LoginInfo.ConnStr);

            if (deleteRole)
            {
                pop_ConfrimDelete.ShowOnPageLoad = false;

                Page_Retrieve();
            }
        }

        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            //grd_Role.Selection.UnselectAll();
            for (var i = 0; i < gvRoleList.Rows.Count; i++)
            {
                var chk_Item = gvRoleList.Rows[i].FindControl("chk_Item") as CheckBox;
                if (chk_Item.Checked)
                {
                    chk_Item.Checked = false;
                }
            }

            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Response.Redirect("Role.aspx?MODE=NEW");
                    break;
            }
        }
        #endregion

    }
}