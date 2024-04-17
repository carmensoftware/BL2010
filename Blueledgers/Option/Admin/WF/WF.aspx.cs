using BlueLedger.PL.BaseClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Text.RegularExpressions;

namespace BlueLedger.PL.Option.Admin.WF
{
    public partial class WF : BasePage
    {
        #region "Attribute"
        private readonly string moduleID = "99.98.3";

        private readonly Blue.BL.dbo.User user = new Blue.BL.dbo.User();
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private readonly Blue.BL.APP.WF wf = new Blue.BL.APP.WF();
        private readonly Blue.BL.APP.WFDt wfDt = new Blue.BL.APP.WFDt();
        private readonly Blue.BL.APP.Field field = new Blue.BL.APP.Field();

        private readonly Blue.BL.Option.Admin.Security.Role role = new Blue.BL.Option.Admin.Security.Role();
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();


        private int _pagePermission = 0;


        #endregion

        #region -- Page_Load --

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
            }
        }

        private void Page_Retrieve()
        {
            // Set module(s)
            var dtModule = config.DbExecuteQuery("SELECT WfId, CONCAT([Desc],' (',StepNo,' steps)') as [Desc] FROM [APP].Wf", null, LoginInfo.ConnStr);

            ddl_Module.DataSource = dtModule;
            ddl_Module.DataBind();
            ddl_Module.SelectedIndex = ddl_Module.Items.Count > 0 ? 0 : -1;


            Page_Setting();
        }

        private void Page_Setting()
        {
            // Get Permission
            _pagePermission = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);

            // Set Step Detail
            var dtWfDt = config.DbExecuteQuery(string.Format("SELECT * FROM [APP].WfDt WHERE WfId = {0}", ddl_Module.SelectedItem.Value.ToString()), null, LoginInfo.ConnStr);

            gv_WfDt.DataSource = dtWfDt;
            gv_WfDt.DataBind();

        }


        #endregion

        #region -- Event(s) --

        protected void ddl_Module_SelectedIndexChanged(object sender, EventArgs e)
        {
            Page_Setting();
        }

        // Rename
        protected void btn_RenStepDesc_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var wfId = ddl_Module.SelectedItem.Value.ToString();
            var wfStep = btn.Attributes["WfStep"].ToString();
            var dtWfDt = config.DbExecuteQuery(string.Format("SELECT StepDesc, EnableField, HideField FROM [APP].[WfDt] WHERE WfId={0} AND Step={1}", wfId, wfStep), null, LoginInfo.ConnStr);
            var stepDesc = dtWfDt.Rows[0]["StepDesc"].ToString();

            pop_Rename.HeaderText = string.Format("({0}) {1}",wfId, stepDesc);
            txt_RenameStepDesc.Text = stepDesc;
            //txt_RenameStepDesc.Attributes.Add("Placeholder", stepDesc);

            pop_Rename.ShowOnPageLoad = true;
            btn_RenameStepDesc_Save.Attributes.Add("WfStep", wfStep);
        }
        protected void btn_RenameStepDesc_Save_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var wfId = ddl_Module.SelectedItem.Value.ToString();
            var wfStep = btn.Attributes["WfStep"].ToString();
            var dtWfDt = config.DbExecuteQuery(string.Format("SELECT StepDesc FROM [APP].[WfDt] WHERE WfId={0} AND Step={1}", wfId, wfStep), null, LoginInfo.ConnStr);
            var stepDesc = dtWfDt.Rows[0]["StepDesc"].ToString();
            var newName = txt_RenameStepDesc.Text;

            if (!string.IsNullOrEmpty(newName) && newName != stepDesc)
            {
                config.DbParseQuery(string.Format("UPDATE [APP].[WfDt] SET StepDesc='{2}' WHERE WfId={0} AND Step={1}",
                wfId,
                wfStep,
                newName), null, LoginInfo.ConnStr);

                config.DbParseQuery(string.Format("UPDATE [APP].ViewHandler SET [Desc]='{2}' WHERE WfId={0} AND WfStep={1}",
                wfId,
                wfStep,
                newName), null, LoginInfo.ConnStr);

            }

            pop_Rename.ShowOnPageLoad = false;
            Page_Setting();
        }

        // Approval

        protected void btn_Edit_Approval_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var wfId = ddl_Module.SelectedItem.Value.ToString();
            var wfStep = btn.Attributes["WfStep"].ToString();
            var dtWfDt = config.DbExecuteQuery(string.Format("SELECT Approvals, StepDesc FROM [APP].[WfDt] WHERE WfId={0} AND Step={1}", wfId, wfStep), null, LoginInfo.ConnStr);

            var selectRoles = GetRoles(dtWfDt.Rows[0][0].ToString());
            var selectUsers = GetUsers(dtWfDt.Rows[0][0].ToString());

            lbl_Pop_Approval_Title.Text = string.Format("<b>({0})</b> {1}", wfStep, dtWfDt.Rows[0]["StepDesc"]);

            // Set Roles
            var dtRole = config.DbExecuteQuery("SELECT RoleName, RoleDesc FROM [ADMIN].[Role] WHERE IsActive = 1 ORDER BY RoleDesc", null, LoginInfo.ConnStr);
            var role = dtRole.AsEnumerable()
                .Select(x => new ListItem
                {
                    Text = x.Field<string>("RoleDesc"),
                    Value = x.Field<string>("RoleName"),
                    Selected = selectRoles.Contains(x.Field<string>("RoleName"))
                }).ToArray();

            cbl_Role.Items.Clear();
            cbl_Role.Items.AddRange(role);

            // Set Users
            var dtUser = config.DbExecuteQuery("SELECT LoginName, CONCAT(ISNULL(FName,''),' ',ISNULL(MName,''),' ',ISNULL(LName,''),' ') as FullName FROM [ADMIN].[vUser] WHERE IsActived = 1 ORDER BY LoginName", null, LoginInfo.ConnStr);
            var users = dtUser.AsEnumerable()
                .Select(x => new ListItem
                {
                    Text = string.Format("{0} : {1}", x.Field<string>("LoginName"), x.Field<string>("FullName")),
                    Value = x.Field<string>("LoginName"),
                    Selected = selectUsers.Contains(x.Field<string>("LoginName"))
                }).ToArray();

            cbl_User.Items.Clear();
            cbl_User.Items.AddRange(users);

            pop_Approval.ShowOnPageLoad = true;
            btn_Pop_Approval_Save.Attributes.Add("WfStep", wfStep);
        }

        protected void btn_Pop_Approval_Save_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var wfId = ddl_Module.SelectedItem.Value.ToString();
            var wfStep = btn.Attributes["WfStep"].ToString();

            var listRole = new List<string>();
            var listUser = new List<string>();


            // Get Roles
            foreach (ListItem item in cbl_Role.Items)
            {
                if (item.Selected)
                    listRole.Add(item.Value);
            }
            // Get Users
            foreach (ListItem item in cbl_User.Items)
            {
                if (item.Selected)
                    listUser.Add(string.Format("#{0}", item.Value));
            }

            var approvals = string.Join(",", listRole) + "," + string.Join(",", listUser);
            config.DbParseQuery(string.Format("UPDATE [APP].[WfDt] SET Approvals = '{0}' WHERE WfId={1} AND Step={2}", approvals, wfId, wfStep), null, LoginInfo.ConnStr);

            pop_Approval.ShowOnPageLoad = false;
            Page_Setting();

        }

        // Condition
        protected void btn_Edit_Condition_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var wfId = ddl_Module.SelectedItem.Value.ToString();
            var wfStep = btn.Attributes["WfStep"].ToString();
            var dtWfDt = config.DbExecuteQuery(string.Format("SELECT IsHOD, IsAllocateVendor, SentEmail, StepDesc FROM [APP].[WfDt] WHERE WfId={0} AND Step={1}", wfId, wfStep), null, LoginInfo.ConnStr);

            var dr = dtWfDt.Rows[0];

            lbl_Pop_Condition_Title.Text = string.Format("<b>({0})</b> {1}", wfStep, dtWfDt.Rows[0]["StepDesc"]);

            chk_IsHOD.Checked = Convert.ToBoolean(dr["IsHOD"]);
            chk_IsAllocateVendor.Checked = Convert.ToBoolean(dr["IsAllocateVendor"]);
            chk_IsEmail.Checked = Convert.ToBoolean(dr["SentEmail"]);
            chk_IsEmailReject.Checked = GetRejectToRequesterValue();

            pop_Condition.ShowOnPageLoad = true;
            btn_Pop_Condition_Save.Attributes.Add("WfStep", wfStep);
        }

        protected void btn_Pop_Condition_Save_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;

            var wfId = ddl_Module.SelectedItem.Value.ToString();
            var wfStep = btn.Attributes["WfStep"].ToString();

            var isHOD = chk_IsHOD.Checked ? 1 : 0;
            var isAV = chk_IsAllocateVendor.Checked ? 1 : 0;
            var isEmail = chk_IsEmail.Checked ? 1 : 0;
            var isReject = chk_IsEmailReject.Checked ? 1 : 0;

            config.DbParseQuery(string.Format("UPDATE [APP].[WfDt] SET IsHOD={2}, IsAllocateVendor={3}, SentEmail={4} WHERE WfId={0} AND Step={1}",
                wfId,
                wfStep,
                isHOD,
                isAV,
                isEmail), null, LoginInfo.ConnStr);

            config.SetConfigValue("WF", "SR", "EnableRejectMail", isReject.ToString(), LoginInfo.ConnStr);


            pop_Condition.ShowOnPageLoad = false;
            Page_Setting();

        }

        // Column
        protected void btn_Edit_Column_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var wfId = ddl_Module.SelectedItem.Value.ToString();
            var wfStep = btn.Attributes["WfStep"].ToString();
            var dtWfDt = config.DbExecuteQuery(string.Format("SELECT StepDesc, EnableField, HideField FROM [APP].[WfDt] WHERE WfId={0} AND Step={1}", wfId, wfStep), null, LoginInfo.ConnStr);

            var drWfDt = dtWfDt.Rows[0];

            lbl_Pop_Column_Title.Text = string.Format("<b>({0})</b> {1}", wfStep, drWfDt["StepDesc"]);

            var enableFields = drWfDt["EnableField"].ToString().Split(',').Select(x => x.Trim()).ToArray();
            var hideFields = drWfDt["HideField"].ToString().Split(',').Select(x => x.Trim()).ToArray();

            var dtField = config.DbExecuteQuery("SELECT FieldName, [Desc] as FieldDesc, 0 as IsEnabled, 0 as IsHidden FROM [APP].Field WHERE SchemaName='PC' AND TableName IN ('PC.PrDt') ORDER BY FieldName", null, LoginInfo.ConnStr);


            foreach (DataRow drField in dtField.Rows)
            {

                drField["IsEnabled"] = enableFields.Contains(drField["FieldName"].ToString().Trim());
                drField["IsHidden"] = hideFields.Contains(drField["FieldName"].ToString().Trim());
            }

            gv_Column.DataSource = dtField;
            gv_Column.DataBind();


            pop_Column.ShowOnPageLoad = true;
            btn_Pop_Column_Save.Attributes.Add("WfStep", wfStep);
        }

        protected void btn_Pop_Column_Save_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;

            var wfId = ddl_Module.SelectedItem.Value.ToString();
            var wfStep = btn.Attributes["WfStep"].ToString();

            var enabledFields = new List<string>();
            var hiddenFields = new List<string>();

            for (int i = 0; i < gv_Column.Rows.Count; i++)
            {
                var chk_Enabled = gv_Column.Rows[i].FindControl("chk_Enabled") as CheckBox;
                var chk_Hidden = gv_Column.Rows[i].FindControl("chk_Hidden") as CheckBox;

                if (chk_Enabled.Checked)
                {
                    var fieldName = chk_Enabled.Attributes["data-fieldname"].ToString();
                    enabledFields.Add(fieldName);
                }

                if (chk_Hidden.Checked)
                {
                    var fieldName = chk_Enabled.Attributes["data-fieldname"].ToString();
                    hiddenFields.Add(fieldName);
                }
            }


            config.DbParseQuery(string.Format("UPDATE [APP].[WfDt] SET EnableField='{2}', HideField='{3}' WHERE WfId={0} AND Step={1}",
                wfId,
                wfStep,
                string.Join(",", enabledFields),
                string.Join(",", hiddenFields)), null, LoginInfo.ConnStr);



            pop_Column.ShowOnPageLoad = false;
            Page_Setting();

        }

        protected void chk_Enabled_HD_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gv_Column.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    var chk = row.FindControl("chk_Enabled") as CheckBox;
                    chk.Checked = (sender as CheckBox).Checked;

                }
            }
        }

        protected void chk_Hidden_HD_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gv_Column.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    var chk = row.FindControl("chk_Hidden") as CheckBox;
                    chk.Checked = (sender as CheckBox).Checked;

                }
            }
        }

        // Workflow Detail

        protected void gv_WfDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                // Set Permission

                var btn_RenStepDesc = (Button)e.Row.FindControl("btn_RenStepDesc");
                if (btn_RenStepDesc != null)
                {
                    btn_RenStepDesc.Visible = _pagePermission >= 3;
                    btn_RenStepDesc.Attributes.Add("WfStep", DataBinder.Eval(e.Row.DataItem, "Step").ToString());
                }

                var btn_Edit_Approval = (Button)e.Row.FindControl("btn_Edit_Approval");
                if (btn_Edit_Approval != null)
                {
                    btn_Edit_Approval.Visible = _pagePermission >= 3;
                    btn_Edit_Approval.Attributes.Add("WfStep", DataBinder.Eval(e.Row.DataItem, "Step").ToString());
                }

                var btn_Edit_Condition = (Button)e.Row.FindControl("btn_Edit_Condition");
                if (btn_Edit_Condition != null)
                {
                    btn_Edit_Condition.Visible = _pagePermission >= 3;
                    btn_Edit_Condition.Attributes.Add("WfStep", DataBinder.Eval(e.Row.DataItem, "Step").ToString());
                }

                var btn_Edit_Column = (Button)e.Row.FindControl("btn_Edit_Column");
                if (btn_Edit_Column != null)
                {
                    btn_Edit_Column.Visible = _pagePermission >= 3 && Convert.ToInt32(ddl_Module.SelectedItem.Value) == 1;
                    btn_Edit_Column.Attributes.Add("WfStep", DataBinder.Eval(e.Row.DataItem, "Step").ToString());
                }

                // Display Column

                var panel_Id = (Panel)e.Row.FindControl("panel_Id");
                var btn = new LinkButton
                {
                    ID = DataBinder.Eval(e.Row.DataItem, "Step").ToString(),
                    ClientIDMode = System.Web.UI.ClientIDMode.Static
                };
                panel_Id.Controls.Clear();
                panel_Id.Controls.Add(btn);

                var lbl_Roles = (Label)e.Row.FindControl("lbl_Roles");
                if (lbl_Roles != null)
                {
                    var value = GetRoleDesc(DataBinder.Eval(e.Row.DataItem, "Approvals").ToString());
                    var roles = string.Join(",  ", value.Distinct().OrderBy(o => o));
                    lbl_Roles.Text = roles;
                }

                var lbl_Users = (Label)e.Row.FindControl("lbl_Users");
                if (lbl_Users != null)
                {
                    var value = GetUsers(DataBinder.Eval(e.Row.DataItem, "Approvals").ToString());
                    var users = string.Join(",  ", value.Distinct().OrderBy(o => o));
                    lbl_Users.Text = users;
                }

                if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsHOD")))
                {
                    var img = (Image)e.Row.FindControl("img_HOD");
                    img.Visible = true;

                    var lbl_HOD = (Label)e.Row.FindControl("lbl_HOD");
                    lbl_HOD.Visible = true;
                }


                if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsAllocateVendor")))
                {
                    var img = (Image)e.Row.FindControl("img_IsAllocateVendor");
                    img.Visible = true;

                    var lbl_IsAllocateVendor = (Label)e.Row.FindControl("lbl_IsAllocateVendor");
                    lbl_IsAllocateVendor.Visible = true;
                }



                if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "SentEMail")))
                {
                    var img = (Image)e.Row.FindControl("img_Email");
                    img.Visible = true;

                    var lbl_Email = (Label)e.Row.FindControl("lbl_Email");
                    lbl_Email.Visible = true;

                }

                
                if (GetRejectToRequesterValue())
                {
                    var img = (Image)e.Row.FindControl("img_Reject");
                    img.Visible = true;

                    var lbl_Reject = (Label)e.Row.FindControl("lbl_Reject");
                    lbl_Reject.Visible = true;

                }

                if (Convert.ToInt32(ddl_Module.SelectedItem.Value) == 1)
                {
                    var lbl_HideColumn_Title = (Label)e.Row.FindControl("lbl_HideColumn_Title");
                    lbl_HideColumn_Title.Visible = true;

                    var lbl_HideColumn = (Label)e.Row.FindControl("lbl_HideColumn");
                    if (lbl_HideColumn != null)
                    {
                        var value = GetFieldNames(DataBinder.Eval(e.Row.DataItem, "HideField").ToString());
                        lbl_HideColumn.Text = string.Join(", ", value.Distinct().OrderBy(o => o));
                    }

                    var lbl_EditColumn_Title = (Label)e.Row.FindControl("lbl_EditColumn_Title");
                    lbl_EditColumn_Title.Visible = true;

                    var lbl_EditColumn = (Label)e.Row.FindControl("lbl_EditColumn");
                    if (lbl_EditColumn != null)
                    {
                        var value = GetFieldNames(DataBinder.Eval(e.Row.DataItem, "EnableField").ToString());
                        lbl_EditColumn.Text = string.Join(", ", value.Distinct().OrderBy(o => o));
                    }

                }


            }
        }

        protected void gv_Column_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                var chk_Enabled = (CheckBox)e.Row.FindControl("chk_Enabled");
                chk_Enabled.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsEnabled"));
                chk_Enabled.Attributes.Add("data-fieldname", DataBinder.Eval(e.Row.DataItem, "FieldName").ToString());


                var chk_Hidden = (CheckBox)e.Row.FindControl("chk_Hidden");
                chk_Hidden.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsHidden"));
                chk_Hidden.Attributes.Add("data-fieldname", DataBinder.Eval(e.Row.DataItem, "FieldName").ToString());
            }
        }

        #endregion

        #region -- Private Method(s) --

        private void ShowAlert(string text)
        {
            lbl_Alert.Text = text;
            pop_Alert.ShowOnPageLoad = true;
        }

        private IEnumerable<string> GetRoles(string approvals)
        {
            var result = new List<string>();
            var roles = approvals.Split(',');

            foreach (var item in roles)
            {
                var role = item.Trim();
                if (!string.IsNullOrEmpty(role) && !role.StartsWith("#"))
                {
                    result.Add(role);
                }
            }

            return result;
        }

        private IEnumerable<string> GetRoleDesc(string approvals)
        {
            var result = new List<string>();
            var dtRole = config.DbExecuteQuery("SELECT RoleName, RoleDesc FROM [ADMIN].[Role] WHERE IsActive = 1 ORDER BY RoleDesc", null, LoginInfo.ConnStr);

            var roles = approvals.Split(',');

            foreach (var item in roles)
            {
                var role = item.Trim();
                if (!string.IsNullOrEmpty(role) && !role.StartsWith("#"))
                {
                    var value = dtRole.AsEnumerable().FirstOrDefault(x => x.Field<string>("RoleName") == role);
                    if (value != null)
                        result.Add(value.Field<string>("RoleDesc"));
                }
            }

            return result;
        }

        private IEnumerable<string> GetUsers(string approvals)
        {
            var result = new List<string>();

            var users = approvals.Split(',');

            foreach (var item in users)
            {
                var user = item.Trim();
                if (!string.IsNullOrEmpty(user) && user.StartsWith("#"))
                    result.Add(user.Remove(0, 1));
            }

            return result;
        }

        private IEnumerable<string> GetFieldNames(string text)
        {
            var result = new List<string>();
            var dtField = config.DbExecuteQuery("SELECT FieldName, [Desc] FROM [APP].Field WHERE SchemaName='PC' AND TableName IN ('PC.Pr','PC.PrDt') ORDER BY FieldName", null, LoginInfo.ConnStr);

            var fields = text.Split(',');

            foreach (var field in fields)
            {
                var item = dtField.AsEnumerable().FirstOrDefault(x => x.Field<string>("FieldName") == field.Trim());

                if (item != null)
                    result.Add(string.Format("[{0}]", item.Field<string>("Desc")));
            }

            return result;
        }

        private bool GetRejectToRequesterValue()
        {
            var isReject = config.GetConfigValue("WF", "SR", "EnableRejectMail", LoginInfo.ConnStr);
            return isReject == "1" || isReject.ToLower() == "true";
        }

        #endregion

    }
}