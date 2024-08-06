using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace BlueLedger.PL.Option.Inventory
{
    public partial class JobCode : BasePage
    {
        private readonly Blue.BL.Import.JobCode _jobcode = new Blue.BL.Import.JobCode();

        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly string moduleID = "2.9.14";


        private DataTable dtJobCode
        {
            get { return ViewState["dtJobCode"] as DataTable; }
            set { ViewState["dtJobCode"] = value; }
        }


        // Initial
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
            }

            SetPermission();
        }

        private void Page_Retrieve()
        {
            BindData();

        }

        private void Page_Setting()
        {
        }


        // Event(s)
        protected void menu_ItemClick_Click(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Create();
                    break;


                case "PRINT":
                    //Print();
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    break;
            }
        }


        // gvJobcode
        protected void gvJobcode_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvJobcode.PageIndex = e.NewPageIndex;
            BindData();
        }

        protected void gvJobcode_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var gv = sender as GridView;
            var rowIndex = Convert.ToInt32(e.CommandArgument);
            var row = gv.Rows[rowIndex];

            switch (e.CommandName)
            {
                case "DeleteItem":
                    var code = row.Cells[1].Text;
                    var desc = row.Cells[2].Text;


                    hf_DeleteCode.Value = code;
                    hf_DeleteDesc.Value = desc;
                    lbl_ConfirmDelete.Text = string.Format("Do you want to delete code <b>{0}</b> : <b>{1}</b>?", code, desc);
                    pop_ConfirmDelete.ShowOnPageLoad = true;
                    break;
            }
        }

        protected void gvJobcode_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvJobcode.EditIndex = e.NewEditIndex;
            BindData();
        }

        protected void gvJobcode_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var row = gvJobcode.Rows[e.RowIndex] as GridViewRow;

            var code = row.Cells[1].Text.Trim();
            var desc = (row.Cells[2].Controls[0] as TextBox).Text.Trim();
            var isActive = (row.FindControl("chk_IsActive") as CheckBox).Checked;

            var sql = "UPDATE [IMPORT].[JobCode] SET [Description]=@Desc, IsActive=@isActive WHERE [Code]=@Code";
            var p = new SqlParameter[3];
            p[0] = new SqlParameter { ParameterName = "@Code", Value = code };
            p[1] = new SqlParameter { ParameterName = "@Desc", Value = desc };
            p[2] = new SqlParameter { ParameterName = "@isActive", Value = isActive };

            var error = ExecNonQuery(sql, p, LoginInfo.ConnStr);

            if (!string.IsNullOrEmpty(error))
            {
                ShowAlert(error);

            }
            else
            {
                gvJobcode.EditIndex = -1;
                BindData();
            }

        }

        protected void gvJobcode_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvJobcode.EditIndex = -1;
            BindData();
        }

        // Create
        protected void btn_SaveNew_Click(object sender, EventArgs e)
        {
            var code = txt_Code.Text.Trim().ToUpper();
            var desc = txt_Desc.Text.Trim();

            desc = string.IsNullOrEmpty(desc) ? code : desc;

            if (!Regex.IsMatch(code, "^[a-zA-Z0-9]*$"))
            {
                lbl_Alert.Text = "Invalid code, code accepts only A-Z and 0-9 and no space";
                pop_Alert.ShowOnPageLoad = true;
            }
            else
            {
                var sql = "INSERT INTO [IMPORT].[JobCode] ([Code],[Description],[IsActive]) VALUES (@Code, @Desc, 1)";

                var p = new SqlParameter[2];
                p[0] = new SqlParameter { ParameterName = "@Code", Value = code };
                p[1] = new SqlParameter { ParameterName = "@Desc", Value = desc };

                var result = ExecNonQuery(sql, p, LoginInfo.ConnStr);

                if (result != "")
                {
                    var error = result;

                    if (result.Contains("PRIMARY KEY"))
                        error = "This code already exists."; //string.Format("<b>{0}</b>, this code already exists.", code);

                    ShowAlert(error);
                }
                else

                    pop_Create.ShowOnPageLoad = false;
                //Page.Response.Redirect(Page.Request.Url.ToString(), true);
                BindData();
            }
        }


        // Delete
        protected void btn_ConfirmDelete_Click(object sender, EventArgs e)
        {
            var code = hf_DeleteCode.Value.ToString();
            var desc = hf_DeleteDesc.Value.ToString();

            var sql = string.Format("SELECT COUNT(PrNo) FROM PC.PR WHERE [AddField1]='{0}'", code);
            var dt = _jobcode.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            var count = (int)dt.Rows[0][0];

            if (count > 0)
            {
                ShowAlert(string.Format("Not allow to delete, <b>{0}</b>, this code has already used in Puruchase Request.", code));
                pop_ConfirmDelete.ShowOnPageLoad = false;
            }
            else
            {


                sql = "DELETE FROM [IMPORT].[JobCode] WHERE [Code]=@Code";
                var p = new SqlParameter[1];
                p[0] = new SqlParameter { ParameterName = "@Code", Value = code };

                var error = ExecNonQuery(sql, p, LoginInfo.ConnStr);

                if (!string.IsNullOrEmpty(error))
                {
                    ShowAlert(error);
                }
                else
                {
                    pop_ConfirmDelete.ShowOnPageLoad = false;
                    BindData();
                }

            }

        }

        // Method(s)
        private void SetPermission()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            menu_ItemClick.Items.FindByName("Create").Visible = (pagePermiss >= 3);

            foreach (GridViewRow row in gvJobcode.Rows)
            {
				if (row.FindControl("btn_Edit")!=null)
				{
					row.FindControl("btn_Edit").Visible = pagePermiss >= 3;
					row.FindControl("btn_Del").Visible = pagePermiss >= 7;
				}
            }

        }

        private string ExecNonQuery(string queryString, SqlParameter[] parameters, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(
               connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                cmd.Connection.Open();
                try
                {
                    if (parameters != null)
                    {
                        foreach (var item in parameters)
                            cmd.Parameters.Add(item);
                    }

                    cmd.ExecuteNonQuery();
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        private void BindData()
        {
            var dt = _jobcode.DbExecuteQuery("SELECT [Code], [Description], IsActive FROM [IMPORT].[JobCode]", null, LoginInfo.ConnStr);

            gvJobcode.DataSource = dt;
            gvJobcode.DataBind();
        }

        private void Create()
        {
            txt_Code.Text = "";
            txt_Desc.Text = "";
            pop_Create.ShowOnPageLoad = true;
        }

        private void ShowAlert(string text)
        {
            lbl_Alert.Text = text;
            pop_Alert.ShowOnPageLoad = true;
        }

    }
}
