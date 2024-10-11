using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Inventory
{
    public partial class Unit : BasePage
    {

        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();


        private DataTable _dtData
        {
            get
            {
                return ViewState["_dtData"] as DataTable;
            }
            set
            {
                ViewState["_dtData"] = value;
            }
        }



        protected void Page_Init(object sender, EventArgs e)
        {
            ddl_View.SelectedIndex = 1;
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
        }

        private void Page_Retrieve()
        {
            _dtData = new DataTable();

            Page_Setting();
        }

        private void Page_Setting()
        {
            BindData();
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    pop_New.ShowOnPageLoad = true;
                    break;


                case "PRINT":
                    //Print();
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    break;
            }
        }

        protected void ddl_View_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void gv_Data_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem;

                var chk = e.Row.FindControl("chk_IsActived") as CheckBox;

                if (chk != null)
                {
                    chk.Checked = Convert.ToBoolean(DataBinder.Eval(dataItem, "IsActived"));
                }
            }

        }

        protected void gv_Data_RowEditing(object sender, GridViewEditEventArgs e)
        {
            (sender as GridView).EditIndex = e.NewEditIndex;
            BindData();
        }

        protected void gv_Data_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //Reset the edit index.
            (sender as GridView).EditIndex = -1;
            //Bind data to the GridView control.
            BindData();
        }
        
        protected void gv_Data_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var row = (sender as GridView).Rows[e.RowIndex] as GridViewRow;

            var chk = row.FindControl("chk_IsActived") as CheckBox;
            var hf_UnitCode = row.FindControl("hf_UnitCode") as HiddenField;


            var code = hf_UnitCode.Value; //(row.Cells[0].Controls[0] as Label).Text;
            var name = (row.Cells[1].Controls[0] as TextBox).Text.Trim();
            var isActived = chk.Checked;

            

            var query = @"UPDATE [IN].Unit SET [Name]=@Name, IsActived=@IsActived, UpdatedDate=GETDATE(), UpdatedBy=@LoginName WHERE UnitCode=@Code";
            var p = new Blue.DAL.DbParameter[4];
            
            p[0] = new Blue.DAL.DbParameter("@Code", code);
            p[1] = new Blue.DAL.DbParameter("@Name", name);
            p[2] = new Blue.DAL.DbParameter("@IsActived", isActived ? "1" : "0");
            p[3] = new Blue.DAL.DbParameter("@LoginName", LoginInfo.LoginName);


            config.DbExecuteQuery(query, p, LoginInfo.ConnStr);

            //Reset the edit index.
            (sender as GridView).EditIndex = -1;

            //Bind data to the GridView control.
            BindData();
        }

        protected void btn_Save_New_Click(object sender, EventArgs e)
        {
            var code = txt_UnitCode_New.Text.Trim();
            var name = txt_UnitName_New.Text.Trim();
            var isActived = chk_IsActived_New.Checked;

            var query = @"SELECT TOP(1) * FROM [IN].Unit WHERE UnitCode=@Code";
            var p1= new Blue.DAL.DbParameter[1];

            p1[0] = new Blue.DAL.DbParameter("@Code", code);

            var dt = config.DbExecuteQuery(query, p1, LoginInfo.ConnStr);

            if (dt.Rows.Count > 0)
            {
                lbl_Pop_Alert.Text = string.Format("This code '{0}' is already exists.", code);
                pop_Alert.ShowOnPageLoad = true;

                return;
            }

            query = "INSERT INTO [IN].Unit(UnitCode,[Name],IsActived, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)";
            query += " VALUES(@Code, @Name, @IsActived, GETDATE(), @LoginName, GETDATE(), @LoginName)";

            var p = new Blue.DAL.DbParameter[4];

            p[0] = new Blue.DAL.DbParameter("@Code", code);
            p[1] = new Blue.DAL.DbParameter("@Name", name);
            p[2] = new Blue.DAL.DbParameter("@IsActived", isActived ? "1" : "0");
            p[3] = new Blue.DAL.DbParameter("@LoginName", LoginInfo.LoginName);

            config.DbExecuteQuery(query, p, LoginInfo.ConnStr);

            BindData();
            pop_New.ShowOnPageLoad = false;
        }

        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            var row = (sender as Button).NamingContainer;

            var hf_UnitCode = row.FindControl("hf_UnitCode") as HiddenField;

            hf_UnitCode_Delete.Value = hf_UnitCode.Value;

            lbl_Confirm_Delete.Text = string.Format("Do you want to delete '{0}'?", hf_UnitCode.Value);

            pop_Confirm_Delete.ShowOnPageLoad = true;
        }

        protected void btn_Confirm_Delete_Yes_Click(object sender, EventArgs e)
        {
            var code = hf_UnitCode_Delete.Value;

            var p = new Blue.DAL.DbParameter[1];
            
            p[0] = new Blue.DAL.DbParameter("@Code", code);
            
            var dt = config.DbExecuteQuery("SELECT DISTINCT ProductCode FROM [IN].ProdUnit WHERE OrderUnit=@Code", p, LoginInfo.ConnStr);

            if (dt.Rows.Count > 0)
            {
                var items = dt.AsEnumerable().Select(x => x.Field<string>("ProductCode"));


                lbl_Pop_Alert.Text = string.Format("'{0}' is using on proudct '{1}'", code, string.Join(",", items));
                pop_Alert.ShowOnPageLoad = true;
            }
            else
            {
                var p1 = new Blue.DAL.DbParameter[1];

                p1[0] = new Blue.DAL.DbParameter("@Code", code);
                config.DbExecuteQuery("DELETE FROM [IN].Unit WHERE UnitCode=@Code", p1, LoginInfo.ConnStr);
            }

            pop_Confirm_Delete.ShowOnPageLoad = false;

            BindData();
        }

        private void BindData()
        {
            var view = ddl_View.SelectedValue.ToString().ToUpper();
            var search = txt_Search.Text.Trim();

            var query = "SELECT * FROM [IN].Unit WHERE UnitCode LIKE @Search OR [Name] LIKE @Search ORDER BY UnitCode";

            if (view== "ACTIVE")
            {
                query = "SELECT * FROM [IN].Unit WHERE IsActived=1 AND (UnitCode LIKE @Search OR [Name] LIKE @Search) ORDER BY UnitCode";
            }
            else if (view == "INACTIVE")
            {
                query = "SELECT * FROM [IN].Unit WHERE IsActived=0 AND (UnitCode LIKE @Search OR [Name] LIKE @Search) ORDER BY UnitCode";
            }

            var p = new Blue.DAL.DbParameter[1];

            p[0] = new Blue.DAL.DbParameter("@Search", string.Format("%{0}%", search));

            _dtData = config.DbExecuteQuery(query, p, LoginInfo.ConnStr);

            gv_Data.DataSource = _dtData;
            gv_Data.DataBind();
        }

    }

}