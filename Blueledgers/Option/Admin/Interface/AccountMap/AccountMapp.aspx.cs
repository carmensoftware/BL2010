using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

namespace BlueLedger.PL.Option.Admin.Interface.AccountMap
{
    public partial class AccountMapp : BasePage
    {
        #region "Attributes"
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();

        #endregion

        protected string _Type
        {
            get { return Request.QueryString["type"] == null ? "1" : Request.QueryString["type"].ToString(); }
        }

        protected DataTable _dtDataView
        {
            get
            {
                return ViewState["_dtDataView"] as DataTable;
            }
            set
            {
                ViewState["_dtDataView"] = value;
            }
        }

        protected DataTable _dtData
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

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
         

        }

        private void Page_Retrieve()
        {
            _dtDataView = new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery("SELECT ID, ViewName FROM [ADMIN].[AccountMappView] ORDER BY Id");
            GetData();

            Page_Setting();
        }

        private void Page_Setting()
        {
            BindData();
        }

        // Event(s)
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                switch (e.Item.Name.ToUpper())
                {
                    case "EDIT": //2016-10-05 Add case Edit on Menu_CmdBar
                        break;

                    case "SAVE":
                        break;

                    case "PRINT":
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                        break;

                    case "BACK":
                        break;

                    case "IMPORT":
                        break;

                    // Added on: 23/11/2017, For: New feature(Get new account-Mapping)
                    case "GETNEW":
                        break;
                    // End Added.
                }
            }
        }

        // GridView

        protected void gv_AP_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            (sender as GridView).PageIndex = e.NewPageIndex;
            BindData();
        }

        protected void gv_AP_Sorting(object sender, GridViewSortEventArgs e)
        {
        }

        protected void gv_AP_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem;

                // Main Keys

                if (e.Row.FindControl("LocationCode") != null)
                {
                    (e.Row.FindControl("LocationCode") as Label).Text = DataBinder.Eval(dataItem, "LocationCode").ToString();
                    (e.Row.FindControl("LocationName") as Label).Text = DataBinder.Eval(dataItem, "LocationName").ToString();
                }

                if (e.Row.FindControl("CategoryCode") != null)
                {
                    (e.Row.FindControl("CategoryCode") as Label).Text = DataBinder.Eval(dataItem, "CategoryCode").ToString();
                    (e.Row.FindControl("CategoryName") as Label).Text = DataBinder.Eval(dataItem, "CategoryName").ToString();
                }

                if (e.Row.FindControl("SubCategoryCode") != null)
                {
                    (e.Row.FindControl("SubCategoryCode") as Label).Text = DataBinder.Eval(dataItem, "SubCategoryCode").ToString();
                    (e.Row.FindControl("SubCategoryName") as Label).Text = DataBinder.Eval(dataItem, "SubCategoryName").ToString();
                }

                if (e.Row.FindControl("ItemGroupCode") != null)
                {
                    (e.Row.FindControl("ItemGroupCode") as Label).Text = DataBinder.Eval(dataItem, "ItemGroupCode").ToString();
                    (e.Row.FindControl("ItemGroupName") as Label).Text = DataBinder.Eval(dataItem, "ItemGroupName").ToString();
                }

                // Alternate key
                if (e.Row.FindControl("AdjCode") != null)
                {
                    (e.Row.FindControl("AdjCode") as Label).Text = DataBinder.Eval(dataItem, "AdjCode").ToString();
                    (e.Row.FindControl("AdjName") as Label).Text = DataBinder.Eval(dataItem, "AdjName").ToString();
                }



                // Value(s)
                // AP
                if (e.Row.FindControl("DepCode") != null)
                {
                    (e.Row.FindControl("DepCode") as Label).Text = DataBinder.Eval(dataItem, "DepCode").ToString();
                    (e.Row.FindControl("DepName") as Label).Text = DataBinder.Eval(dataItem, "DepName").ToString();
                }

                if (e.Row.FindControl("AccCode") != null)
                {
                    (e.Row.FindControl("AccCode") as Label).Text = DataBinder.Eval(dataItem, "AccCode").ToString();
                    (e.Row.FindControl("AccName") as Label).Text = DataBinder.Eval(dataItem, "AccName").ToString();
                }

                // GL
                if (e.Row.FindControl("DrDepCode") != null)
                {
                    (e.Row.FindControl("DrDepCode") as Label).Text = DataBinder.Eval(dataItem, "DrDepCode").ToString();
                    (e.Row.FindControl("DrDepName") as Label).Text = DataBinder.Eval(dataItem, "DrDepName").ToString();
                }

                if (e.Row.FindControl("DrAccCode") != null)
                {
                    (e.Row.FindControl("DrAccCode") as Label).Text = DataBinder.Eval(dataItem, "DrAccCode").ToString();
                    (e.Row.FindControl("DrAccName") as Label).Text = DataBinder.Eval(dataItem, "DrAccName").ToString();
                }

                if (e.Row.FindControl("CrDepCode") != null)
                {
                    (e.Row.FindControl("CrDepCode") as Label).Text = DataBinder.Eval(dataItem, "CrDepCode").ToString();
                    (e.Row.FindControl("CrDepName") as Label).Text = DataBinder.Eval(dataItem, "CrDepName").ToString();
                }

                if (e.Row.FindControl("CrAccCode") != null)
                {
                    (e.Row.FindControl("CrAccCode") as Label).Text = DataBinder.Eval(dataItem, "CrAccCode").ToString();
                    (e.Row.FindControl("CrAccName") as Label).Text = DataBinder.Eval(dataItem, "CrAccName").ToString();
                }


            }
        }

        protected void gv_AP_RowEditing(object sender, GridViewEditEventArgs e)
        {
            (sender as GridView).EditIndex = e.NewEditIndex;
            BindData();
        }

        protected void gv_AP_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //Reset the edit index.
            (sender as GridView).EditIndex = -1;
            //Bind data to the GridView control.
            BindData();
        }
        protected void gv_AP_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var row = (sender as GridView).Rows[e.RowIndex] as GridViewRow;
            var hf_ID = row.FindControl("hf_ID") as HiddenField;
            var ddl_DepCode = row.FindControl("ddl_DepCode") as DropDownList;
            var txt_DepCode = row.FindControl("txt_DepCode") as TextBox;
            var ddl_AccCode = row.FindControl("ddl_AccCode") as DropDownList;
            var txt_AccCode = row.FindControl("txt_AccCode") as TextBox;

            var sql = new Helpers.SQL(LoginInfo.ConnStr);
            var query = @"UPDATE [ADMIN].AccountMapp SET A1=@DepCode, A2=@AccCode WHERE ID=@Id";

            var id = hf_ID.Value;
            var depCode = ddl_DepCode == null ? txt_DepCode.Text.Trim() : ddl_DepCode.SelectedValue.ToString();
            var accCode = ddl_AccCode == null ? txt_AccCode.Text.Trim() : ddl_AccCode.SelectedValue.ToString(); ;

            depCode = "GEN";
            accCode = "617-0003";

            sql.ExecuteQuery(query, new SqlParameter[]
            {
                new SqlParameter("ID", id),
                new SqlParameter("DepCode", depCode),
                new SqlParameter("AccCode", accCode)
            });

            var dt = _dtData;

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ID"] == id)
                {
                    dr["DepCode"] = depCode;
                    dr["AccCode"] = accCode;
                    
                    break;
                }
            }
            dt.AcceptChanges();
            _dtData = dt;

            //var dr = _dtData.Rows.Find(id);
            //var dr = _dtData.Select(string.Format("ID='{0}'", id)).FirstOrDefault();

            //dr["DepCode"] = depCode;
            //dr["AccCode"] = accCode;

            //_dtData.AcceptChanges();

            //Reset the edit index.
            (sender as GridView).EditIndex = -1;

            //Bind data to the GridView control.
            BindData();
        }

        #region -- Method(s)--

        private void GetData()
        {
            var sql = new Helpers.SQL(LoginInfo.ConnStr);

            var dtView = sql.ExecuteQuery(string.Format("SELECT * FROM [ADMIN].[AccountMappView] WHERE ID = '{0}'", _Type));

            if (dtView.Rows.Count == 0)
                return;

            var dr = dtView.Rows[0];
            var postType = dr["PostType"].ToString().ToUpper();
            var query = "";

            if (postType == "AP")
            {
                #region -- AP --
                query = @";WITH
map AS(
	SELECT
		ID,
		StoreCode as LocationCode,
		ItemGroupCode,
		A1 as DepCode,
		A2 as AccCode
	FROM
		[ADMIN].AccountMapp
	WHERE
		PostType=@PostType
)
SELECT
	m.*,

	l.LocationName,
	c.CategoryCode,
	c.CategoryName,
	c.SubCategoryCode,
	c.SubCategoryName,
	c.ItemGroupName,

	d.DeptDesc as DepName,
	a.AccDesc1 as AccName
FROM
	map m
	JOIN [IN].StoreLocation l ON l.LocationCode=m.LocationCode
	JOIN [IN].vProdCatList c ON c.ItemGroupCode=m.ItemGroupCode
	LEFT JOIN [ADMIN].AccountDepartment d ON d.DeptCode=m.DepCode
	LEFT JOIN [ADMIN].AccountCode a ON a.AccCode=m.AccCode
ORDER BY
	LocationCode,
	CategoryCode,
	SubCategoryCode,
	ItemGroupCode";
                #endregion
            }
            else // GL
            {
                #region -- GL --
                query = @"
;WITH
map AS(
	SELECT
		ID,
		StoreCode as LocationCode,
		ItemGroupCode,
		A1 as AdjCode,
		
		A2 as DrDepCode,
		A3 as DrAccCode,
		A4 as CrDepCode,
		A5 as CrAccCode
	FROM
		[ADMIN].AccountMapp
	WHERE
		PostType=@PostType
)
SELECT
	m.*,

	l.LocationName,
	c.CategoryCode,
	c.CategoryName,
	c.SubCategoryCode,
	c.SubCategoryName,
	c.ItemGroupName,

	a.AdjName,
	dd.DeptDesc as DrDepName,
	da.AccDesc1 as DrAccName,
	cd.DeptDesc as CrDepName,
	ca.AccDesc1 as CrAccName

FROM
	map m
	JOIN [IN].StoreLocation l ON l.LocationCode=m.LocationCode
	JOIN [IN].vProdCatList c ON c.ItemGroupCode=m.ItemGroupCode
	JOIN [IN].AdjType a ON a.AdjCode=m.AdjCode
	LEFT JOIN [ADMIN].AccountDepartment dd ON dd.DeptCode=m.DrDepCode
	LEFT JOIN [ADMIN].AccountCode da ON da.AccCode=m.DrAccCode
	LEFT JOIN [ADMIN].AccountDepartment cd ON cd.DeptCode=m.CrDepCode
	LEFT JOIN [ADMIN].AccountCode ca ON ca.AccCode=m.CrAccCode
ORDER BY
	LocationCode,
	CategoryCode,
	SubCategoryCode,
	ItemGroupCode";
                #endregion
            }


            _dtData = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("PostType", postType) });

            _dtData.PrimaryKey = new DataColumn[] { _dtData.Columns["ID"] };

        }

        private void BindData()
        {
            gv_AP.DataSource = _dtData;
            gv_AP.DataBind();
        }

        #endregion

    }



}