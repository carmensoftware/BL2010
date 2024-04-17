using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;

namespace BlueLedger.PL.IN
{
    public partial class AdjTypeLst : BasePage
    {
        private readonly Blue.BL.IN.AdjType adjType = new Blue.BL.IN.AdjType();
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly string moduleID = "3.10.5";

        private DataSet dsAdj = new DataSet();
        private DataSet dsSearchAdj = new DataSet();

        private string AdjMode
        {
            get { return ViewState["AdjMode"].ToString(); }
            set { ViewState["AdjMode"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsAdj = (DataSet)Session["dsAdj"];
            }
        }

        private void Page_Retrieve()
        {
            dsAdj.Clear();

            var getAdjType = adjType.GetSchema(dsAdj, LoginInfo.ConnStr);

            if (!getAdjType)
            {
                return;
            }

            Session["dsAdj"] = dsAdj;

            Page_Setting();
        }

        private void Page_Setting()
        {
            // ASP GridView.
            grd_AdjType1.DataSource = dsAdj.Tables[adjType.TableName];
            grd_AdjType1.EditIndex = -1;
            grd_AdjType1.DataBind();

            Control_HeaderMenuBar();
        }

        // Add on: 06/10/2017, By: Fon
        protected void Control_HeaderMenuBar()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            menu_CmdBar.Items.FindByName("Create").Visible = (pagePermiss >= 3)
                ? menu_CmdBar.Items.FindByName("Create").Visible : false;
            menu_CmdBar.Items.FindByName("Delete").Visible = (pagePermiss >= 7)
                ? menu_CmdBar.Items.FindByName("Delete").Visible : false;
        }
        // End Added.

        protected void Create()
        {
            // Disable Create & Delete
            menu_CmdBar.Enabled = false;

            // Add new stkInDt row
            var drNew = dsAdj.Tables[adjType.TableName].NewRow();

            dsAdj.Tables[adjType.TableName].Rows.Add(drNew);

            grd_AdjType1.DataSource = dsAdj.Tables[adjType.TableName];
            grd_AdjType1.EditIndex = dsAdj.Tables[adjType.TableName].Rows.Count - 1;
            grd_AdjType1.DataBind();

            AdjMode = "NEW";
        }

        protected void btn_Print_Click(object sender, EventArgs e)
        {
        }

        protected void Delete()
        {
            pop_ConfrimDelete.ShowOnPageLoad = true;
        }

        protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        {
            for (var i = grd_AdjType1.Rows.Count - 1; i >= 0; i--)
            {
                var Chk_Item = grd_AdjType1.Rows[i].Cells[0].FindControl("Chk_Item") as CheckBox;
                if (Chk_Item.Checked)
                {
                    dsAdj = (DataSet)Session["dsAdj"];

                    dsAdj.Tables[adjType.TableName].Rows[i].Delete();
                }
            }

            // Save to database
            var saveAdjType = adjType.Save(dsAdj, LoginInfo.ConnStr);

            if (saveAdjType)
            {
                grd_AdjType1.DataSource = dsAdj.Tables[adjType.TableName];
                grd_AdjType1.EditIndex = -1;
                grd_AdjType1.DataBind();

                pop_ConfrimDelete.ShowOnPageLoad = false;

                Page_Retrieve();
            }
        }

        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
        }

        // ASP GridView.
        protected void grd_AdjType1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Aded on: 06/10/2017, By: Fon
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (pagePermiss < 3)
                    e.Row.Cells[1].Attributes.Add("style", "display: none;");
            }
            // End Added.

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Added on: 06/10/2017, By: Fon
                if (pagePermiss < 3)
                    e.Row.Cells[1].Attributes.Add("style", "display: none;");
                // End Added.

                // Code.
                if (e.Row.FindControl("lbl_Code") != null)
                {
                    var lbl_Code = e.Row.FindControl("lbl_Code") as Label;
                    lbl_Code.Text = DataBinder.Eval(e.Row.DataItem, "AdjId").ToString();
                }
                if (e.Row.FindControl("txt_Code") != null)
                {
                    var txt_Code = e.Row.FindControl("txt_Code") as TextBox;
                    txt_Code.Text = DataBinder.Eval(e.Row.DataItem, "AdjId").ToString();
                }

                // Adjust Type.
                if (e.Row.FindControl("lbl_AdjType") != null)
                {
                    var lbl_AdjType = e.Row.FindControl("lbl_AdjType") as Label;
                    lbl_AdjType.Text = DataBinder.Eval(e.Row.DataItem, "AdjType").ToString();
                }
                if (e.Row.FindControl("ddl_AdjType") != null)
                {
                    var ddl_AdjType = e.Row.FindControl("ddl_AdjType") as ASPxComboBox;
                    ddl_AdjType.Value = DataBinder.Eval(e.Row.DataItem, "AdjType").ToString();
                }

                // Adjust Code.
                if (e.Row.FindControl("lbl_AdjCode") != null)
                {
                    var lbl_AdjCode = e.Row.FindControl("lbl_AdjCode") as Label;
                    lbl_AdjCode.Text = DataBinder.Eval(e.Row.DataItem, "AdjCode").ToString();
                }
                if (e.Row.FindControl("txt_AdjCode") != null)
                {
                    var txt_AdjCode = e.Row.FindControl("txt_AdjCode") as TextBox;
                    txt_AdjCode.Text = DataBinder.Eval(e.Row.DataItem, "AdjCode").ToString();
                }

                // Adjust Name.
                if (e.Row.FindControl("lbl_AdjName") != null)
                {
                    var lbl_AdjName = e.Row.FindControl("lbl_AdjNAme") as Label;
                    lbl_AdjName.Text = DataBinder.Eval(e.Row.DataItem, "AdjName").ToString();
                }
                if (e.Row.FindControl("txt_AdjName") != null)
                {
                    var txt_AdjName = e.Row.FindControl("txt_AdjName") as TextBox;
                    txt_AdjName.Text = DataBinder.Eval(e.Row.DataItem, "AdjName").ToString();
                }

                // Description.
                if (e.Row.FindControl("lbl_Desc") != null)
                {
                    var lbl_Desc = e.Row.FindControl("lbl_Desc") as Label;
                    lbl_Desc.Text = DataBinder.Eval(e.Row.DataItem, "Description").ToString();
                }
                if (e.Row.FindControl("txt_Desc") != null)
                {
                    var txt_Desc = e.Row.FindControl("txt_Desc") as TextBox;
                    txt_Desc.Text = DataBinder.Eval(e.Row.DataItem, "Description").ToString();
                }

                // Actived.
                if (e.Row.FindControl("Img_Btn_ChkBox") != null)
                {
                    var Img_Btn_ChkBox = e.Row.FindControl("Img_Btn_ChkBox") as ImageButton;
                    Img_Btn_ChkBox.Enabled = false;

                    if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsActived")))
                    {
                        Img_Btn_ChkBox.ImageUrl = "~/App_Themes/Default/Images/IN/STDREQ/chk_True.png";
                    }
                    else
                    {
                        Img_Btn_ChkBox.ImageUrl = "~/App_Themes/Default/Images/IN/STDREQ/chk_False.png";
                    }
                }
                if (e.Row.FindControl("chk_Actived") != null)
                {
                    var chk_Actived = e.Row.FindControl("chk_Actived") as CheckBox;
                    if (DataBinder.Eval(e.Row.DataItem, "IsActived") != DBNull.Value)
                    {
                        chk_Actived.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsActived"));
                    }
                    else
                    {
                        chk_Actived.Checked = true;
                    }
                }
            }
        }

        protected void grd_AdjType1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dsAdj = (DataSet)Session["dsAdj"];

            // Deisable Create & Delete
            menu_CmdBar.Enabled = false;

            grd_AdjType1.DataSource = dsAdj.Tables[adjType.TableName];
            grd_AdjType1.EditIndex = e.NewEditIndex;
            grd_AdjType1.DataBind();

            AdjMode = "EDIT";
        }

        protected void grd_AdjType1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dsAdj = (DataSet)Session["dsAdj"];

            if (AdjMode == "NEW")
            {
                dsAdj.Tables[adjType.TableName].Rows[dsAdj.Tables[adjType.TableName].Rows.Count - 1].Delete();
            }

            if (AdjMode == "EDIT")
            {
                dsAdj.Tables[adjType.TableName].Rows[dsAdj.Tables[adjType.TableName].Rows.Count - 1].CancelEdit();
            }

            // Enable Create & Delete
            menu_CmdBar.Enabled = true;

            grd_AdjType1.DataSource = dsAdj.Tables[adjType.TableName];
            grd_AdjType1.EditIndex = -1;
            grd_AdjType1.DataBind();

            AdjMode = string.Empty;
        }

        protected void grd_AdjType1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            dsAdj = (DataSet)Session["dsAdj"];

            var ddl_AdjType = grd_AdjType1.Rows[e.RowIndex].FindControl("ddl_AdjType") as ASPxComboBox;
            var txt_AdjCode = grd_AdjType1.Rows[e.RowIndex].FindControl("txt_AdjCode") as TextBox;
            var txt_AdjName = grd_AdjType1.Rows[e.RowIndex].FindControl("txt_AdjName") as TextBox;
            var txt_Desc = grd_AdjType1.Rows[e.RowIndex].FindControl("txt_Desc") as TextBox;
            var chk_Actived = grd_AdjType1.Rows[e.RowIndex].FindControl("chk_Actived") as CheckBox;

            if (txt_AdjCode.Text == string.Empty)
            {
                pop_Warning.ShowOnPageLoad = true;
                lbl_Warning.Text = "Adjust Code can not be empty.";
                return;
            }
            if (txt_AdjName.Text == string.Empty)
            {
                pop_Warning.ShowOnPageLoad = true;
                lbl_Warning.Text = "Adjust Name can not be empty.";
                return;
            }

            if (AdjMode == "NEW")
            {
                var drNew = dsAdj.Tables[adjType.TableName].Rows[grd_AdjType1.EditIndex];

                drNew["AdjType"] = ddl_AdjType.Value;
                drNew["AdjCode"] = txt_AdjCode.Text;
                drNew["AdjName"] = txt_AdjName.Text;
                drNew["Description"] = txt_Desc.Text;
                drNew["IsActived"] = chk_Actived.Checked;
                drNew["CreatedBy"] = LoginInfo.LoginName;
                drNew["CreatedDate"] = ServerDateTime;
                drNew["UpdatedBy"] = LoginInfo.LoginName;
                drNew["UpdatedDate"] = ServerDateTime;

                var countTypeCode = adjType.Get_CountByCodeType(drNew["AdjType"].ToString(), drNew["AdjCode"].ToString(),
                    LoginInfo.ConnStr);

                if (countTypeCode > 0)
                {
                    lbl_Warning.Text = "Can not to Insert data.";
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }
                var save = adjType.Save(dsAdj, LoginInfo.ConnStr);

                if (save)
                {
                    grd_AdjType1.DataSource = dsAdj.Tables[adjType.TableName];
                    grd_AdjType1.EditIndex = -1;
                    grd_AdjType1.DataBind();
                }

                Page_Retrieve();
            }
            else
            {
                var drUpdating = dsAdj.Tables[adjType.TableName].Rows[e.RowIndex];

                drUpdating["AdjType"] = ddl_AdjType.Value;
                drUpdating["AdjCode"] = txt_AdjCode.Text;
                drUpdating["AdjName"] = txt_AdjName.Text;
                drUpdating["Description"] = txt_Desc.Text;
                drUpdating["IsActived"] = chk_Actived.Checked;
                drUpdating["UpdatedBy"] = LoginInfo.LoginName;
                drUpdating["UpdatedDate"] = ServerDateTime;

                var save = adjType.Save(dsAdj, LoginInfo.ConnStr);

                if (save)
                {
                    grd_AdjType1.DataSource = dsAdj.Tables[adjType.TableName];
                    grd_AdjType1.EditIndex = -1;
                    grd_AdjType1.DataBind();
                }
            }

            // Enable Create & Delete
            menu_CmdBar.Enabled = true;

            grd_AdjType1.DataSource = dsAdj.Tables[adjType.TableName];
            grd_AdjType1.EditIndex = -1;
            grd_AdjType1.DataBind();

            AdjMode = string.Empty;
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
            }
        }
    }
}