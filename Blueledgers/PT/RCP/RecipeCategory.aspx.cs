using System;
using BlueLedger.PL.BaseClass;
using System.Data;

namespace BlueLedger.PL.PT.RCP
{
    public partial class RecipeCategory : BasePage
    {
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.PT.RCP.RcpCategory rcpCat = new Blue.BL.PT.RCP.RcpCategory();

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ListPageLookup.PrintItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Recipe Category List", "SL"));
                ListPageLookup.DataBind();

                //ListPageLookup.CreateItems.NavigateUrl = "~/PT/RCP/RecipeCategoryEdit.aspx?MODE=new&BuCode=" + LoginInfo.BuInfo.BuCode + "&VID=" + Request.Cookies["[PT].[vRcpCategoryList]"].Value;
                ListPageLookup.CreateItems.NavigateUrl = "~/PT/RCP/RecipeCategory.aspx?MODE=new&BuCode=" + LoginInfo.BuInfo.BuCode + "&VID=" + Request.Cookies["[PT].[vRcpCategoryList]"].Value;

                var id = Request.Params["ID"];
                if (id != null)
                {
                    IsEditMode(false);

                    string sql = string.Format("SELECT * FROM PT.RcpCategory WHERE RcpCateCode = '{0}'", id.ToString());
                    DataTable dt = rcpCat.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        txt_CateCode.Text = dt.Rows[0]["RcpCateCode"].ToString();
                        txt_CateDesc.Text = dt.Rows[0]["RcpCateDesc"].ToString();

                        txt_CateCode.Enabled = false;

                        IsEditMode(false);
                        pop_EditCatgory.ShowOnPageLoad = true;
                    }


                }
                else
                {
                    var mode = Request.Params["MODE"];
                    if (mode != null && mode.ToUpper() == "NEW")
                    {
                        txt_CateCode.Text = string.Empty;
                        txt_CateDesc.Text = string.Empty;

                        txt_CateCode.Enabled = true;
                        IsEditMode(true);

                        pop_EditCatgory.ShowOnPageLoad = true;
                    }
                }
            }
            ListPageLookup.CreateItems.Menu.ItemClick += menu_ItemClick;

            base.Page_Load(sender, e);
        }

        private void menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SL":

                    Session["dtBuKeys"] = ListPageLookup.dtBuKeys;
                    var reportLink = "../../RPT/ReportCriteria.aspx?category=012&reportid=322";
                    ClientScript.RegisterStartupScript(GetType(), "newWindow",
                        "<script>window.open('" + reportLink + "','_blank')</script>");
                    break;
            }
        }


        private void IsEditMode(bool isEdited)
        {
            menu_CmdBar.Items.FindByName("Edit").Visible = !isEdited;
            menu_CmdBar.Items.FindByName("Delete").Visible = !isEdited;
            menu_CmdBar.Items.FindByName("Back").Visible = !isEdited;

            btn_Save.Visible = isEdited;
            btn_Cancel.Visible = isEdited;

            txt_CateCode.ReadOnly = !isEdited;
            txt_CateDesc.ReadOnly = !isEdited;
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "EDIT":
                    IsEditMode(true);
                    break;

                case "DELETE":
                    lbl_Confirm.Text = string.Format("Do you want to delete '{0} : {1}'?", txt_CateCode.Text, txt_CateDesc.Text);
                    pop_ConfirmDelete.ShowOnPageLoad = true;
                    break;

                case "BACK":
                    Back();
                    break;
            }

        }

        private void Back()
        {
            Response.Redirect("RecipeCategory.aspx");
        }


        protected void btn_Save_Click(object sender, EventArgs e)
        {
            string errorMessage = SaveRcpCate();
            if (errorMessage == string.Empty)
                Back();
            else
            {
                lbl_Alert.Text = errorMessage;
                pop_Alert.ShowOnPageLoad = true;
            }

        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            var mode = Request.Params["MODE"];

            if (mode != null && mode.ToUpper() == "NEW")
                Back();
            else
                IsEditMode(false);
        }

        protected void btn_ConfirmYes_Click(object sender, EventArgs e)
        {
            string errorMessage = DeleteRcpCate();
            if (errorMessage == string.Empty)
                Back();
            else
            {
                lbl_Alert.Text = errorMessage;
                pop_Alert.ShowOnPageLoad = true;
            }
        }

        private string SaveRcpCate()
        {
            string errorMessage = string.Empty;

            var id = Request.Params["ID"];
            if (id != null)  //Edit
            {
                string sql = string.Format("UPDATE PT.RcpCategory SET RcpCateDesc = N'{1}' WHERE RcpCateCode = '{0}'", id.ToString(), txt_CateDesc.Text);
                rcpCat.DbParseQuery(@sql, null, LoginInfo.ConnStr);
            }
            else // New
            {
                if (string.IsNullOrWhiteSpace(txt_CateCode.Text))
                {
                    return "Category code is required.";
                }

                // Check existing code
                string sql = string.Format("SELECT COUNT(RcpCateCode) as RecordCount FROM PT.RcpCategory WHERE RcpCateCode = '{0}'", txt_CateCode.Text);
                DataTable dt = rcpCat.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
                int recordCount = Convert.ToInt32(dt.Rows[0][0]);

                if (recordCount > 0) //Existing
                    errorMessage = "Duplicate code!";
                else
                {
                    sql = string.Format("INSERT INTO PT.RcpCategory(RcpCateCode, RcpCateDesc) VALUES(N'{0}', N'{1}')", txt_CateCode.Text, txt_CateDesc.Text);
                    rcpCat.DbParseQuery(@sql, null, LoginInfo.ConnStr);

                }
            }
            return errorMessage;
        }

        private string DeleteRcpCate()
        {
            string errorMessage = string.Empty;
            // Check code is used.


            string sql = string.Format("DELETE FROM PT.RcpCategory WHERE RcpCateCode = '{0}'", txt_CateCode.Text);
            rcpCat.DbParseQuery(@sql, null, LoginInfo.ConnStr);

            return errorMessage;
        }



    }
}