using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Data;
using DevExpress.Web.ASPxEditors;


namespace BlueLedger.PL.IN.REC
{
    public partial class ExtraCost : BasePage
    {
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();

        private readonly string moduleID = "2.9.1.5";
        private int userPermission = 0;

        private DataTable dtExtCostType
        {
            get { return (DataTable)Session["dtExtCostType"]; }
            set { Session["dtExtCostType"] = value; }
        }


        // ---------------------------------------------------


        protected override void Page_Load(object sender, EventArgs e)
        {
            // Set Permission Page
            userPermission = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);

            menu_Item.Items.FindByName("Create").Visible = (userPermission >= 3) ? true : false;
            menu_Item.Items.FindByName("Delete").Visible = (userPermission >= 7) ? true : false;

            if (!IsPostBack)
            {
                Grd_ExtCostType_BindData();
            }


        }

        private void Grd_ExtCostType_BindData()
        {
            string sql = "SELECT * FROM PC.ExtCostType";
            dtExtCostType = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            grd_ExtCostType.DataSource = dtExtCostType;
            grd_ExtCostType.DataBind();
        }

        protected void menu_Item_Click(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Create();
                    break;

                case "DELETE":
                    Delete();
                    break;

                case "PRINT":
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    break;

            }
        }

        private void Create()
        {
            txt_TypeName.Text = string.Empty;
            lbl_TypeName_Error.Text = string.Empty;

            pop_Create.ShowOnPageLoad = true;
        }

        private void Delete()
        {
            int itemCount = 0;

            foreach (GridViewRow gvr in grd_ExtCostType.Rows)
            {
                var chk_Item = gvr.FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    itemCount++;
                }
            }

            if (itemCount > 0)
            {

                lbl_Confirm.Text = string.Format("Do you want to delete {0} item(s)", itemCount.ToString());
                pop_Confirmation.ShowOnPageLoad = true;
            }
            else
            {
                lbl_Alert.Text = "No item is selected to delete";
                pop_Alert.ShowOnPageLoad = true;
            }
        }


        protected void btn_Save_Click(object sender, EventArgs e)
        {
            string newTypeName = txt_TypeName.Text;


            // Check existing name
            DataRow dr = CheckDuplicateName(newTypeName);
            if (dr != null)
            {
                lbl_TypeName_Error.Text = string.Format("Duplicate name '{0}' was created by {1} at {2}", newTypeName, dr["CreatedBy"].ToString(), dr["CreatedDate"].ToString());
                Grd_ExtCostType_BindData();

            }
            else
            {
                string sql = "SELECT ISNULL(MAX(TypeId), 0) FROM PC.ExtCostType";
                DataTable dt = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
                int newId = Convert.ToInt32(dt.Rows[0][0]) + 1;

                sql = string.Format("INSERT INTO PC.ExtCostType(TypeId, TypeName, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy) VALUES({0}, '{1}', '{2}', '{3}', '{4}', '{5}')",
                    newId.ToString(),
                    newTypeName,
                    DateTime.Now.ToString("yyyy-MM-dd hh:mm:dd:ss"),
                    LoginInfo.LoginName,
                    DateTime.Now.ToString("yyyy-MM-dd hh:mm:dd:ss"),
                    LoginInfo.LoginName);
                bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

                Grd_ExtCostType_BindData();
                txt_TypeName.Text = string.Empty;

                pop_Create.ShowOnPageLoad = false;
            }

        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            Grd_ExtCostType_BindData();
            txt_TypeName.Text = string.Empty;

            pop_Create.ShowOnPageLoad = false;
        }


        protected void grd_ExtCostType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var hf_TypeId = (HiddenField)(e.Row.FindControl("hf_TypeId"));
            if (hf_TypeId != null)
            {
                hf_TypeId.Value = DataBinder.Eval(e.Row.DataItem, "TypeId").ToString();
            }

            var lnkb_Edit = (LinkButton)(e.Row.FindControl("lnkb_Edit"));
            if (lnkb_Edit != null)
            {
                lnkb_Edit.Visible = userPermission >= 3 ? true : false;
            }
        }

        protected void grd_ExtCostType_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grd_ExtCostType.DataSource = dtExtCostType;
            grd_ExtCostType.EditIndex = e.NewEditIndex;
            grd_ExtCostType.DataBind();
        }


        protected void grd_ExtCostType_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var txt_TypeName = grd_ExtCostType.Rows[e.RowIndex].FindControl("txt_TypeName") as TextBox;
            var hf_TypeId = grd_ExtCostType.Rows[e.RowIndex].FindControl("hf_TypeId") as HiddenField;

            string newName = txt_TypeName.Text;

            DataRow dr = CheckDuplicateName(newName, Convert.ToInt32(hf_TypeId.Value));
            if (dr != null)
            {
                lbl_Alert.Text = string.Format("Duplicate name '{0}' was created by {1} at {2}", newName, dr["CreatedBy"].ToString(), dr["CreatedDate"].ToString());
                pop_Alert.ShowOnPageLoad = true;
            }
            else
            {

                string sql = "UPDATE PC.ExtCostType SET TypeName=@TypeName WHERE TypeId = @TypeId";
                var param = new Blue.DAL.DbParameter[2];
                param[0] = new Blue.DAL.DbParameter("@TypeName", newName);
                param[1] = new Blue.DAL.DbParameter("@TypeId", hf_TypeId.Value.ToString());

                bu.DbExecuteQuery(sql, param, LoginInfo.ConnStr);

                grd_ExtCostType.EditIndex = -1;
                Grd_ExtCostType_BindData();
            }

        }

        protected void grd_ExtCostType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grd_ExtCostType.EditIndex = -1;
            Grd_ExtCostType_BindData();
        }

        protected void btn_AlertOK_Click(object sender, EventArgs e)
        {
            pop_Alert.ShowOnPageLoad = false;
        }

        protected void btn_ConfirmYes_Click(object sender, EventArgs e)
        {
            string listTypeId = string.Empty;

            foreach (GridViewRow gvr in grd_ExtCostType.Rows)
            {

                var chk_Item = gvr.FindControl("chk_Item") as CheckBox;
                var hf_TypeId = gvr.FindControl("hf_TypeId") as HiddenField;

                if (chk_Item.Checked)
                {
                    listTypeId += hf_TypeId.Value.ToString() + ",";
                }
            }

            DataTable dt = new DataTable();

            // Remove last comma if there is value.
            if (listTypeId != string.Empty)
            {
                listTypeId = listTypeId.Remove(listTypeId.Length - 1);

                string sql = string.Format("SELECT * FROM PC.ExtCostType WHERE TypeId in ({0}) AND TypeId IN (SELECT DISTINCT TypeId FROM PC.RecExtCost)", listTypeId);
                dt = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

                sql = string.Format("DELETE FROM PC.ExtCostType WHERE TypeId in ({0}) AND TypeId NOT IN (SELECT DISTINCT TypeId FROM PC.RecExtCost)", listTypeId);
                bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

                Grd_ExtCostType_BindData();
            }

            pop_Confirmation.ShowOnPageLoad = false;

            if (dt != null && dt.Rows.Count > 0)
            {
                string list = "Some type(s) are not deleted becuase have exist in Receiving. ";
                foreach (DataRow dr in dt.Rows)
                {
                    list += string.Format("{0}, ", dr["TypeName"].ToString()) ;
                }

                lbl_Alert.Text = @list;
                pop_Alert.ShowOnPageLoad = true;
            }
        }

        protected void btn_ConfirmNo_Click(object sender, EventArgs e)
        {

            pop_Confirmation.ShowOnPageLoad = false;
        }


        private DataRow CheckDuplicateName(string newName, int typeId = -1)
        {

            string sql = string.Format("SELECT * FROM PC.ExtCostType WHERE TypeName = '{0}' AND TypeId <> {1}", newName, typeId.ToString());
            DataTable dt = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            if (dt.Rows.Count > 0)
                return dt.Rows[0];
            else
                return null;
        }

    }
}