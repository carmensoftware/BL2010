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
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Drawing;

namespace BlueLedger.PL.Option.Admin.Interface.AccountMap
{
    public partial class AccountMapp : BasePage
    {
        #region "Attributes"
        const string moduleID = "2.9.6";

        private readonly Blue.BL.APP.Config _config = new Blue.BL.APP.Config();
        private readonly Blue.BL.ADMIN.RolePermission permission = new Blue.BL.ADMIN.RolePermission();

        protected string _postType
        {
            get
            {
                var id = ddl_View.SelectedValue.ToString();
                var postType = _dtDataView == null
                    ? "AP"
                    : _dtDataView.AsEnumerable().FirstOrDefault(x => x.Field<string>("Id") == id).Field<string>("PostType").ToUpper();
                return postType;
            }
        }

        protected string _textSearch { get { return txt_Search.Text.Trim(); } }

        protected string _sortColumn
        {
            get { return ViewState["_sortColumn"] == null ? "LocationCode" : ViewState["_sortColumn"].ToString().Trim(); }
            set { ViewState["_sortColumn"] = value; }
        }

        protected string _sortDirection
        {
            get { return ViewState["_sortDirection"] == null ? "ASC" : ViewState["_sortDirection"].ToString().ToUpper(); }
            set { ViewState["_sortDirection"] = value; }
        }

        // Data

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

        protected DataTable _dtDataPrint
        {
            get
            {
                return ViewState["_dtDataPrint"] == null ? new DataTable() : ViewState["_dtDataPrint"] as DataTable;
            }
            set
            {
                ViewState["_dtDataPrint"] = value;
            }
        }


        // Pagination
        protected int _rows { get { return Convert.ToInt32(ddl_Rows.SelectedValue); } }

        protected int _page
        {
            get { return ViewState["_page"] == null ? 1 : (int)ViewState["_page"]; }
            set { ViewState["_page"] = value; }
        }

        protected int _lastPage
        {
            get { return ViewState["_lastPage"] == null ? 1 : (int)ViewState["_lastPage"]; }
            set { ViewState["_lastPage"] = value; }
        }


        protected bool _hasPermissionEdit
        {
            get
            {
               var pagePermiss = permission.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);

               return pagePermiss >= 3;
            }
        }
        
        #endregion




        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }


        }

        private void Page_Retrieve()
        {
            var sql = new Helpers.SQL(LoginInfo.ConnStr);

            _dtDataView = sql.ExecuteQuery("SELECT CAST(ID AS VARCHAR) Id, ViewName, PostType FROM [ADMIN].[AccountMappView] ORDER BY Id");

            ddl_View.DataSource = _dtDataView;
            ddl_View.DataTextField = "ViewName";
            ddl_View.DataValueField = "Id";
            ddl_View.DataBind();


            _dtDataPrint = new DataTable();

            Page_Setting();
        }

        private void Page_Setting()
        {
            //BindData();
            var page = 1;
            var rows = _rows;
            GetData(rows, page);

            SetEditMode(false);
        }

        // Event(s)
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                switch (e.Item.Name.ToUpper())
                {
                    case "GETNEW":
                        GetNewCode();
                        break;

                    case "IMPORT":
                        pop_ImportExport.ShowOnPageLoad = true;
                        break;

                    case "PRINT":
                        Session["AccountMappPrint"] = GetData1(true);
                        //ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                        //ScriptManager.RegisterStartupScript(Page, GetType(), "test", "<script>test1()</script>", false);
                        ScriptManager.RegisterStartupScript(Page, GetType(), "print", string.Format("<script>window.open('AccountMappPrint.aspx?type={0}', 'Print');</script>", _postType), false);
                        break;
                }
            }
        }

        protected void ddl_View_SelectedIndexChanged(object sender, EventArgs e)
        {
            _sortColumn = "LocationCode";
            _sortDirection = "ASC";

            var items = gv_Data.Columns.Cast<DataControlField>().ToArray();

            foreach (var item in items)
            {
                item.Visible = true;
            }

            var hideType = _postType == "AP" ? "GL" : "AP";

            var listHide = gv_Data.Columns.Cast<DataControlField>().Where(x => x.ControlStyle.CssClass.Contains(hideType)).ToArray();

            foreach (var item in listHide)
            {
                item.Visible = false;
            }

            txt_Search.Text = "";

            GetData(_rows, 1);
        }

        protected void ddl_Rows_SelectedIndexChanged(object sender, EventArgs e)
        {
            var page = 1;
            var rows = Convert.ToInt32((sender as DropDownList).SelectedValue);

            GetData(rows, page);
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            GetData(_rows, 1);
        }

        protected void txt_Search_TextChanged(object sender, EventArgs e)
        {
            GetData(_rows, 1);
        }
        // Pagination

        protected void btn_Page_Click(object sender, EventArgs e)
        {
            var page = Convert.ToInt32((sender as Button).Text);

            GetData(_rows, page);
        }

        protected void btn_Page_Fist_Click(object sender, EventArgs e)
        {
            GetData(_rows, 1);
        }

        protected void btn_Page_Previous_Click(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(btn_Page1.Text);

            if (id < _lastPage && id > 1)
            {
                GetData(_rows, id - 2);
            }
        }

        protected void btn_Page_Next_Click(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(btn_Page10.Text);

            if (id < _lastPage)
            {
                GetData(_rows, id + 1);
            }
        }

        protected void btn_Page_Last_Click(object sender, EventArgs e)
        {
            GetData(_rows, _lastPage);
        }

        // GridView
        protected void gv_Data_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            (sender as GridView).PageIndex = e.NewPageIndex;
            BindData();
        }

        protected void gv_Data_Sorting(object sender, GridViewSortEventArgs e)
        {
            _sortColumn = e.SortExpression;
            _sortDirection = _sortDirection == "ASC" ? "DESC" : "ASC";

            var page = _page;
            var rows = _rows;

            GetData(rows, page);
        }

        protected void gv_Data_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var isEdit = _hasPermissionEdit;

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




                // AP 
                if (_postType == "AP")
                {

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

                    if (e.Row.FindControl("ddl_DepCode") != null)
                    {
                        var ddl = e.Row.FindControl("ddl_DepCode") as ASPxComboBox;
                        var value = DataBinder.Eval(dataItem, "DepCode").ToString();

                        ddl.Items.Clear();
                        ddl.Items.AddRange(GetDepartments(value).ToArray());
                        ddl.ToolTip = ddl.Text;


                    }

                    if (e.Row.FindControl("ddl_AccCode") != null)
                    {
                        var ddl = e.Row.FindControl("ddl_AccCode") as ASPxComboBox;
                        var value = DataBinder.Eval(dataItem, "AccCode").ToString();

                        ddl.Items.Clear();
                        ddl.Items.AddRange(GetAccounts(value).ToArray());
                        ddl.ToolTip = ddl.Text;
                    }

                }
                // GL
                else
                {
                    if (e.Row.FindControl("AdjCode") != null)
                    {
                        (e.Row.FindControl("AdjCode") as Label).Text = DataBinder.Eval(dataItem, "AdjCode").ToString();
                        (e.Row.FindControl("AdjName") as Label).Text = DataBinder.Eval(dataItem, "AdjName").ToString();
                    }

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



                    if (e.Row.FindControl("ddl_DrDepCode") != null)
                    {
                        var ddl = e.Row.FindControl("ddl_DrDepCode") as ASPxComboBox;
                        var value = DataBinder.Eval(dataItem, "DrDepCode").ToString();

                        ddl.Items.Clear();
                        ddl.Items.AddRange(GetDepartments(value).ToArray());
                        ddl.ToolTip = ddl.Text;


                    }

                    if (e.Row.FindControl("ddl_DrAccCode") != null)
                    {
                        var ddl = e.Row.FindControl("ddl_DrAccCode") as ASPxComboBox;
                        var value = DataBinder.Eval(dataItem, "DrAccCode").ToString();

                        ddl.Items.Clear();
                        ddl.Items.AddRange(GetAccounts(value).ToArray());
                        ddl.ToolTip = ddl.Text;
                    }

                    if (e.Row.FindControl("ddl_CrDepCode") != null)
                    {
                        var ddl = e.Row.FindControl("ddl_CrDepCode") as ASPxComboBox;
                        var value = DataBinder.Eval(dataItem, "CrDepCode").ToString();

                        ddl.Items.Clear();
                        ddl.Items.AddRange(GetDepartments(value).ToArray());
                        ddl.ToolTip = ddl.Text;


                    }

                    if (e.Row.FindControl("ddl_CrAccCode") != null)
                    {
                        var ddl = e.Row.FindControl("ddl_CrAccCode") as ASPxComboBox;
                        var value = DataBinder.Eval(dataItem, "CrAccCode").ToString();

                        ddl.Items.Clear();
                        ddl.Items.AddRange(GetAccounts(value).ToArray());
                        ddl.ToolTip = ddl.Text;
                    }

                }

                if (e.Row.FindControl("btn_Edit") != null)
                {
                    var btn = e.Row.FindControl("btn_Edit") as ImageButton;

                    btn.Visible = isEdit;
                }

            }
        }

        protected void gv_Data_RowEditing(object sender, GridViewEditEventArgs e)
        {
            var gv = sender as GridView;

            gv.EditIndex = e.NewEditIndex;
            BindData();

            SetEditMode(true);
        }

        protected void gv_Data_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //Reset the edit index.
            (sender as GridView).EditIndex = -1;
            //Bind data to the GridView control.
            BindData();

            SetEditMode(false);
        }

        protected void gv_Data_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var row = (sender as GridView).Rows[e.RowIndex] as GridViewRow;
            var hf_ID = row.FindControl("hf_ID") as HiddenField;
            var id = hf_ID.Value;

            if (_postType == "AP")
            {

                var ddl_DepCode = row.FindControl("ddl_DepCode") as ASPxComboBox;
                var ddl_AccCode = row.FindControl("ddl_AccCode") as ASPxComboBox;

                var sql = new Helpers.SQL(LoginInfo.ConnStr);
                var query = @"UPDATE [ADMIN].AccountMapp SET A1=@DepCode, A2=@AccCode WHERE ID=@Id";

                var department = ddl_DepCode.Text.Split(':');
                var account = ddl_AccCode.Text.Split(':');

                var depCode = department[0].Trim();
                var depName = department.Length > 1 ? department[1].Trim() : "";
                var accCode = account[0].Trim();
                var accName = accCode.Length > 1 ? account[1].Trim() : "";


                sql.ExecuteQuery(query, new SqlParameter[]
                {
                    new SqlParameter("ID", id),
                    new SqlParameter("DepCode", depCode),
                    new SqlParameter("AccCode", accCode)
                });

                var dr = _dtData.AsEnumerable().FirstOrDefault(x => x.Field<string>("ID") == id);

                if (dr != null)
                {
                    dr["DepCode"] = depCode;
                    dr["DepName"] = depName;
                    dr["AccCode"] = accCode;
                    dr["AccName"] = accName;
                    dr.AcceptChanges();
                }

            }
            else
            {
                var ddl_DrDepCode = row.FindControl("ddl_DrDepCode") as ASPxComboBox;
                var ddl_DrAccCode = row.FindControl("ddl_DrAccCode") as ASPxComboBox;
                var ddl_CrDepCode = row.FindControl("ddl_CrDepCode") as ASPxComboBox;
                var ddl_CrAccCode = row.FindControl("ddl_CrAccCode") as ASPxComboBox;

                var sql = new Helpers.SQL(LoginInfo.ConnStr);
                var query = @"UPDATE [ADMIN].AccountMapp SET A2=@DrDepCode, A3=@DrAccCode, A4=@CrDepCode, A5=@CrAccCode WHERE ID=@Id";

                var drDep = ddl_DrDepCode.Text.Split(':');
                var drAcc = ddl_DrAccCode.Text.Split(':');
                var crDep = ddl_CrDepCode.Text.Split(':');
                var crAcc = ddl_CrAccCode.Text.Split(':');

                var drDepCode = drDep[0].Trim();
                var drDepName = drDep.Length > 1 ? drDep[1].Trim() : "";
                var drAccCode = drAcc[0].Trim();
                var drAccName = drAcc.Length > 1 ? drAcc[1].Trim() : "";

                var crDepCode = crDep[0].Trim();
                var crDepName = crDep.Length > 1 ? crDep[1].Trim() : "";
                var crAccCode = crAcc[0].Trim();
                var crAccName = crAcc.Length > 1 ? crAcc[1].Trim() : "";

                sql.ExecuteQuery(query, new SqlParameter[]
                {
                    new SqlParameter("ID", id),
                    new SqlParameter("DrDepCode", drDepCode),
                    new SqlParameter("DrAccCode", drAccCode),
                    new SqlParameter("CrDepCode", crDepCode),
                    new SqlParameter("CrAccCode", crAccCode)
                });

                var dr = _dtData.AsEnumerable().FirstOrDefault(x => x.Field<string>("ID") == id);

                if (dr != null)
                {
                    dr["DrDepCode"] = drDepCode;
                    dr["DrDepName"] = drDepName;
                    dr["DrAccCode"] = drAccCode;
                    dr["DrAccName"] = drAccName;
                    dr["CrDepCode"] = crDepCode;
                    dr["CrDepName"] = crDepName;
                    dr["CrAccCode"] = crAccCode;
                    dr["CrAccName"] = crAccName;
                    dr.AcceptChanges();
                }
            }

            SetEditMode(false);
            //Reset the edit index.
            (sender as GridView).EditIndex = -1;

            //Bind data to the GridView control.
            BindData();
        }

        // Action(s)
        private void GetNewCode()
        {
            new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery("EXEC [ADMIN].[GetNewAccountMapp]");

            GetAPI_AccDep();
        }

        protected void btn_Import_Click(object sender, EventArgs e)
        {
            lbl_Import.Text = "";

            if (FileUploadControl.HasFile)
            {
                var filename = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

                FileUploadControl.SaveAs(filename);

                var error = ImportFile(filename);

                if (string.IsNullOrEmpty(error))
                {
                    lbl_Import.Text = "Done";
                    lbl_Import.ForeColor = Color.Black;
                }
                else
                {
                    lbl_Import.Text = error;
                    lbl_Import.ForeColor = Color.Red;
                }

            }
            else
            {
                lbl_Import.Text = "Please choose a file.";
                lbl_Import.ForeColor = Color.Red;
            }
        }

        protected void btn_Export_Click(object sender, EventArgs e)
        {
            Export();
        }


        #region -- Method(s)--

        private void SaveAsCsv(string text, string filename)
        {
            Response.Clear();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + filename);
            Response.ContentType = "text/csv";
            Response.Write(text);
            Response.Flush();
            Response.End();
        }

        private void SetEditMode(bool isEdit)
        {
            ddl_View.Enabled = !isEdit;
            txt_Search.Enabled = !isEdit;
            btn_Search.Enabled = !isEdit;
        }

        private void BindData()
        {
            gv_Data.DataSource = _dtData;
            gv_Data.DataBind();
        }

        private void Search(string text)
        {

        }

        private DataTable GetData1(bool isPrint = false)
        {

            var query_data = "";
            var query_select = "";
            var query_where = "";
            var query_order = "";

            var queries = new StringBuilder();
            var parameters = new List<SqlParameter>();


            var textSearch = string.Format("%{0}%", _textSearch);
            var postType = _postType;

            if (postType == "AP")
            {
                #region -- AP --
                query_data = @";WITH
map AS(
	SELECT
		CAST(ID as VARCHAR(100)) as ID,
		StoreCode as LocationCode,
		ItemGroupCode,
		A1 as DepCode,
		A2 as AccCode
	FROM
		[ADMIN].AccountMapp
	WHERE
		PostType='AP'
),
data AS(
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
)";
                query_select = " SELECT  * FROM data";
                query_where = " WHERE 1=1 ";

                if (!string.IsNullOrEmpty(textSearch))
                {
                    query_where += @"    
    AND (CategoryCode LIKE @search
    OR CategoryName LIKE @search
    OR SubCategoryCode LIKE @search
    OR SubCategoryName LIKE @search
    OR ItemGroupCode LIKE @search
    OR ItemGroupName LIKE @search

    OR DepCode LIKE @search
    OR DepName LIKE @search
    OR AccCode LIKE @search
    OR AccName LIKE @search)";
                    parameters.Add(new SqlParameter("search", textSearch));
                };


                query_order = string.Format(" ORDER BY {0} {1}, LocationCode, CategoryCode, SubCategoryCode, ItemGroupCode,".Replace(" " + _sortColumn + ",", " "), _sortColumn, _sortDirection).Trim().TrimEnd(',');

                #endregion
            }
            else // GL
            {
                #region -- GL --
                query_data = @"
;WITH
map AS(
	SELECT
		CAST(ID as VARCHAR(100)) as ID,
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
		PostType='GL'
),
data AS(
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
)";
                query_select = " SELECT * FROM data ";
                query_where = " WHERE 1=1 ";

                if (!string.IsNullOrEmpty(textSearch))
                {
                    query_where += @"   
    AND(CategoryCode LIKE @search
    OR CategoryName LIKE @search
    OR SubCategoryCode LIKE @search
    OR SubCategoryName LIKE @search
    OR ItemGroupCode LIKE @search
    OR ItemGroupName LIKE @search
    OR AdjCode LIKE @search
    OR AdjName LIKE @search

    OR DrDepCode LIKE @search
    OR DrDepName LIKE @search
    OR DrAccCode LIKE @search
    OR DrAccName LIKE @search
    OR CrDepCode LIKE @search
    OR CrDepName LIKE @search
    OR CrAccCode LIKE @search
    OR CrAccName LIKE @search)";
                    parameters.Add(new SqlParameter("Search", textSearch));
                }
                query_order = string.Format(" ORDER BY {0} {1}, LocationCode, CategoryCode, SubCategoryCode, ItemGroupCode, AdjCode,".Replace(" " + _sortColumn + ",", " "), _sortColumn, _sortDirection).Trim().TrimEnd(',');
                #endregion
            }


            // Data
            queries.AppendLine(query_data);
            queries.AppendLine(query_select);
            queries.AppendLine(query_where);
            queries.AppendLine(query_order);

            if (!isPrint)
            {
                queries.AppendFormat(" OFFSET {0} * ({1}-1) ROWS FETCH NEXT {0} ROWS ONLY", _rows, _page);
            }

            var sql = new Helpers.SQL(LoginInfo.ConnStr);

            Helpers.WriteLog.Error(queries.ToString());

            var dt = sql.ExecuteQuery(queries.ToString(), parameters.ToArray());


            if (!isPrint)
            {
                // Get last page
                queries.Clear();
                queries.AppendLine(query_data);
                queries.AppendLine("SELECT COUNT(ID) FROM data");
                queries.AppendLine(query_where);

                var dtCount = sql.ExecuteQuery(queries.ToString(), parameters.ToArray());
                var totalRows = (int)dtCount.Rows[0][0];

                _lastPage = totalRows / _rows + 1;
            }

            return dt;
        }

        private void GetData(int rows, int page)
        {
            _dtData = GetData1();
            BindData();
            SetPageButton(page);

            //            var query_data = "";
            //            var query_select = "";
            //            var query_where = "";
            //            var query_order = "";

            //            var queries = new StringBuilder();
            //            var parameters = new List<SqlParameter>();


            //            var textSearch = string.Format("%{0}%", _textSearch);
            //            var postType = _postType;

            //            if (postType == "AP")
            //            {
            //                #region -- AP --
            //                query_data = @";WITH
            //map AS(
            //	SELECT
            //		CAST(ID as VARCHAR(100)) as ID,
            //		StoreCode as LocationCode,
            //		ItemGroupCode,
            //		A1 as DepCode,
            //		A2 as AccCode
            //	FROM
            //		[ADMIN].AccountMapp
            //	WHERE
            //		PostType=@PostType
            //),
            //data AS(
            //    SELECT
            //	    m.*,
            //
            //	    l.LocationName,
            //	    c.CategoryCode,
            //	    c.CategoryName,
            //	    c.SubCategoryCode,
            //	    c.SubCategoryName,
            //	    c.ItemGroupName,
            //
            //	    d.DeptDesc as DepName,
            //	    a.AccDesc1 as AccName
            //    FROM
            //	    map m
            //	    JOIN [IN].StoreLocation l ON l.LocationCode=m.LocationCode
            //	    JOIN [IN].vProdCatList c ON c.ItemGroupCode=m.ItemGroupCode
            //	    LEFT JOIN [ADMIN].AccountDepartment d ON d.DeptCode=m.DepCode
            //	    LEFT JOIN [ADMIN].AccountCode a ON a.AccCode=m.AccCode
            //)";
            //                query_select = " SELECT  * FROM data";
            //                query_where = " WHERE 1=1 ";

            //                if (!string.IsNullOrEmpty(textSearch))
            //                {
            //                    query_where += @"    
            //    AND (CategoryCode LIKE @search
            //    OR CategoryName LIKE @search
            //    OR SubCategoryCode LIKE @search
            //    OR SubCategoryName LIKE @search
            //    OR ItemGroupCode LIKE @search
            //    OR ItemGroupName LIKE @search
            //
            //    OR DepCode LIKE @search
            //    OR DepName LIKE @search
            //    OR AccCode LIKE @search
            //    OR AccName LIKE @search)";
            //                    parameters.Add(new SqlParameter("search", textSearch));
            //                };


            //                query_order = string.Format(" ORDER BY {0} {1}, LocationCode, CategoryCode, SubCategoryCode, ItemGroupCode,".Replace(" " + _sortColumn + ",", " "), _sortColumn, _sortDirection).Trim().TrimEnd(',');

            //                #endregion
            //            }
            //            else // GL
            //            {
            //                #region -- GL --
            //                query_data = @"
            //;WITH
            //map AS(
            //	SELECT
            //		CAST(ID as VARCHAR(100)) as ID,
            //		StoreCode as LocationCode,
            //		ItemGroupCode,
            //		A1 as AdjCode,
            //		
            //		A2 as DrDepCode,
            //		A3 as DrAccCode,
            //		A4 as CrDepCode,
            //		A5 as CrAccCode
            //	FROM
            //		[ADMIN].AccountMapp
            //	WHERE
            //		PostType=@PostType
            //),
            //data AS(
            //    SELECT
            //	    m.*,
            //
            //	    l.LocationName,
            //	    c.CategoryCode,
            //	    c.CategoryName,
            //	    c.SubCategoryCode,
            //	    c.SubCategoryName,
            //	    c.ItemGroupName,
            //    
            //	    a.AdjName,
            //	    dd.DeptDesc as DrDepName,
            //	    da.AccDesc1 as DrAccName,
            //	    cd.DeptDesc as CrDepName,
            //	    ca.AccDesc1 as CrAccName
            //
            //    FROM
            //	    map m
            //	    JOIN [IN].StoreLocation l ON l.LocationCode=m.LocationCode
            //	    JOIN [IN].vProdCatList c ON c.ItemGroupCode=m.ItemGroupCode
            //	    JOIN [IN].AdjType a ON a.AdjCode=m.AdjCode
            //	    LEFT JOIN [ADMIN].AccountDepartment dd ON dd.DeptCode=m.DrDepCode
            //	    LEFT JOIN [ADMIN].AccountCode da ON da.AccCode=m.DrAccCode
            //	    LEFT JOIN [ADMIN].AccountDepartment cd ON cd.DeptCode=m.CrDepCode
            //	    LEFT JOIN [ADMIN].AccountCode ca ON ca.AccCode=m.CrAccCode
            //)";
            //                query_select = " SELECT * FROM data ";
            //                query_where = " WHERE 1=1 ";

            //                if (!string.IsNullOrEmpty(textSearch))
            //                {
            //                    query_where += @"   
            //    AND(CategoryCode LIKE @search
            //    OR CategoryName LIKE @search
            //    OR SubCategoryCode LIKE @search
            //    OR SubCategoryName LIKE @search
            //    OR ItemGroupCode LIKE @search
            //    OR ItemGroupName LIKE @search
            //    OR AdjCode LIKE @search
            //    OR AdjName LIKE @search
            //
            //    OR DrDepCode LIKE @search
            //    OR DrDepName LIKE @search
            //    OR DrAccCode LIKE @search
            //    OR DrAccName LIKE @search
            //    OR CrDepCode LIKE @search
            //    OR CrDepName LIKE @search
            //    OR CrAccCode LIKE @search
            //    OR CrAccName LIKE @search)";
            //                    parameters.Add(new SqlParameter("Search", textSearch));
            //                }
            //                query_order = string.Format(" ORDER BY {0} {1}, LocationCode, CategoryCode, SubCategoryCode, ItemGroupCode, AdjCode,".Replace(" " + _sortColumn + ",", " "), _sortColumn, _sortDirection).Trim().TrimEnd(',');
            //                #endregion
            //            }

            //            var query_offset = " OFFSET @rows * (@page-1) ROWS FETCH NEXT @rows ROWS ONLY";

            //            // Data
            //            queries.AppendLine(query_data);
            //            queries.AppendLine(query_select);
            //            queries.AppendLine(query_where);
            //            queries.AppendLine(query_order);
            //            queries.AppendLine(query_offset);

            //            var sql = new Helpers.SQL(LoginInfo.ConnStr);

            //            parameters.Add(new SqlParameter("PostType", postType));
            //            parameters.Add(new SqlParameter("@rows", rows));
            //            parameters.Add(new SqlParameter("@page", page));

            //            var p_offset = new List<SqlParameter>();
            //            foreach (var p in parameters)
            //            {
            //                p_offset.Add(p);
            //            }

            //            _dtData = sql.ExecuteQuery(queries.ToString(), parameters.ToArray());

            //            _dtData.PrimaryKey = new DataColumn[] { _dtData.Columns["ID"] };

            //            gv_Data.DataSource = _dtData;
            //            gv_Data.DataBind();

            //            // Get last page
            //            queries.Clear();
            //            queries.AppendLine(query_data);
            //            queries.AppendLine("SELECT COUNT(ID) FROM data");
            //            queries.AppendLine(query_where);

            //            var dt = sql.ExecuteQuery(queries.ToString(), p_offset.ToArray());
            //            var totalRows = (int)dt.Rows[0][0];

            //            _lastPage = totalRows / rows + 1;

            //SetPageButton(page);
        }

        private IEnumerable<ListEditItem> GetDepartments(string value = "")
        {
            var dt = new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery("SELECT DeptCode, DeptDesc FROM [ADMIN].AccountDepartment ORDER BY DeptCode");

            return dt.AsEnumerable()
                .Select(x => new ListEditItem
                {
                    Value = x.Field<string>("DeptCode"),
                    Text = x.Field<string>("DeptCode") + " : " + x.Field<string>("DeptDesc"),
                    Selected = x.Field<string>("DeptCode").ToUpper() == value.ToUpper()
                }).ToArray();
        }

        private IEnumerable<ListEditItem> GetAccounts(string value = "")
        {
            var dt = new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery("SELECT AccCode, AccDesc1 FROM [ADMIN].AccountCode WHERE AccType <> 'Header' ORDER BY AccCode");

            return dt.AsEnumerable()
                .Select(x => new ListEditItem
                {
                    Value = x.Field<string>("AccCode"),
                    Text = x.Field<string>("AccCode") + " : " + x.Field<string>("AccDesc1"),
                    Selected = x.Field<string>("AccCode").ToUpper() == value.ToUpper()
                }).ToArray();
        }

        private string ImportFile(string filename)
        {
            var error = "";
            var queries = new StringBuilder();

            using (var csv = File.OpenText(filename))
            {
                var line = "";
                var limit = 0;

                // Skip the first line
                csv.ReadLine();
                while ((line = csv.ReadLine()) != null)
                {
                    #region -- each line --

                    var columns = line.Split(',').Select(x => x.Trim()).ToArray();
                    var lastIndex = columns.Length - 1;

                    var type = columns[0];

                    if (type.Length > 2)
                    {
                        return "Invalid account mapping file.";
                    }

                    var id = columns.Length > 1 ? columns[1] : "";

                    if (type.ToUpper() == "AP")
                    {
                        var depCode = columns[lastIndex - 1];
                        var accCode = columns[lastIndex];


                        queries.AppendLine(string.Format("UPDATE [ADMIN].AccountMapp SET A1=N'{1}', A2=N'{2}'  WHERE ID=N'{0}'", id, depCode, accCode));
                    }
                    else if (type.ToUpper() == "GL")
                    {
                        var drDepCode = columns[lastIndex - 3];
                        var drAccCode = columns[lastIndex - 2];
                        var crDepCode = columns[lastIndex - 1];
                        var crAccCode = columns[lastIndex];

                        queries.AppendLine(string.Format("UPDATE [ADMIN].AccountMapp SET A2=N'{1}', A3=N'{2}', A4=N'{3}', A5=N'{4}' WHERE ID=N'{0}'", id, drDepCode, drAccCode, crDepCode, crAccCode));
                    }

                    #endregion

                    if (limit % 200 == 0)
                    {
                        new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery(queries.ToString());
                        queries.Clear();
                    }

                    limit++;
                }

                //Helpers.WriteLog.Error(queries.ToString());


                if (queries.ToString().Trim() != "")
                {
                    new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery(queries.ToString());
                }
                else
                    error = "No data";

                //SaveAsCsv(queries.ToString(), "test.txt");

                csv.Close();
                File.Delete(filename);
                pop_ImportExport.ShowOnPageLoad = false;

                Response.Redirect("AccountMapp.aspx");
            }



            return error;
        }


        private string ImportFile1(string filename)
        {
            var error = "";
            var queries = new StringBuilder();

            queries.AppendLine("DECLARE @acc TABLE ( ID nvarchar(50) NOT NULL, A1 nvarchar(20), A2 nvarchar(20), A3 nvarchar(20), A4 nvarchar(20), A5 nvarchar(20), PRIMARY KEY (ID))");
            //queries.AppendLine("INSERT INTO @acc (ID, A1, A2, A3, A4, A5) VALUES");


            using (var csv = File.OpenText(filename))
            {
                var line = "";
                var insert = new StringBuilder();
                // Skip the first line
                csv.ReadLine();
                var limit = 0;

                while ((line = csv.ReadLine()) != null)
                {
                    if (limit % 1000 == 0)
                    {
                        queries.Append(insert.ToString().Trim().TrimEnd(','));

                        insert.Clear();

                        insert.AppendLine("INSERT INTO @acc (ID, A1, A2, A3, A4, A5) VALUES");


                    }

                    limit++;
                    var columns = line.Split(',').Select(x => x.Trim()).ToArray();
                    var lastIndex = columns.Length - 1;
                    // get the first (Type) and second (Id)
                    var type = columns[0];
                    var id = columns.Length > 1 ? columns[1] : "";

                    if (type.Length > 2)
                        return "Invalid account mapping file.";

                    if (!string.IsNullOrEmpty(id))
                    {
                        if (_postType == "AP")
                        {
                            var depCode = columns[lastIndex - 1];
                            var accCode = columns[lastIndex];

                            //insert.AppendLine(string.Format("INSERT INTO @acc (ID, A1, A2, A3, A4, A5) VALUES('{0}','{1}','{2}','','','');", id, depCode, accCode));
                            insert.AppendLine(string.Format("(N'{0}',N'{1}',N'{2}','','',''),", id, depCode, accCode));

                        }
                        else
                        {
                            var drDepCode = columns[lastIndex - 3];
                            var drAccCode = columns[lastIndex - 2];
                            var crDepCode = columns[lastIndex - 1];
                            var crAccCode = columns[lastIndex];

                            insert.AppendLine(string.Format("INSERT INTO @acc (ID, A1, A2, A3, A4, A5) VALUES(N'{0}',N'{1}',N'{2}',N'{3}',N'{4}');", id, drDepCode, drAccCode, crDepCode, crAccCode));
                        }

                    }


                }


                queries.Append(insert.ToString().Trim().TrimEnd(','));

                if (_postType == "AP")
                {
                    queries.AppendLine("UPDATE [ADMIN].AccountMapp SET	A1=acc.A1, A2=acc.A2 FROM [ADMIN].AccountMapp map JOIN @acc acc ON acc.ID=map.ID ");
                }
                else
                {
                    queries.AppendLine("UPDATE [ADMIN].AccountMapp SET	A2=acc.A2, A3=acc.A3, A4=acc.A4, A5=acc.A5 FROM [ADMIN].AccountMapp map JOIN @acc acc ON acc.ID=map.ID ");
                }


                if (insert.Length > 0)
                {
                    new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery(queries.ToString());
                }
                else
                    error = "No data";

                //SaveAsCsv(queries.ToString(), "test.txt");

                csv.Close();
                File.Delete(filename);
                pop_ImportExport.ShowOnPageLoad = false;

                Response.Redirect("AccountMapp.aspx");
            }



            return error;
        }

        private void SetPageButton(int page)
        {
            _page = page;
            // Reset Color
            btn_Page1.BackColor = default(Color);
            btn_Page1.ForeColor = default(Color);
            btn_Page2.BackColor = default(Color);
            btn_Page2.ForeColor = default(Color);
            btn_Page3.BackColor = default(Color);
            btn_Page3.ForeColor = default(Color);
            btn_Page4.BackColor = default(Color);
            btn_Page4.ForeColor = default(Color);
            btn_Page5.BackColor = default(Color);
            btn_Page5.ForeColor = default(Color);
            btn_Page6.BackColor = default(Color);
            btn_Page6.ForeColor = default(Color);
            btn_Page7.BackColor = default(Color);
            btn_Page7.ForeColor = default(Color);
            btn_Page8.BackColor = default(Color);
            btn_Page8.ForeColor = default(Color);
            btn_Page9.BackColor = default(Color);
            btn_Page9.ForeColor = default(Color);
            btn_Page10.BackColor = default(Color);
            btn_Page10.ForeColor = default(Color);

            var lastPage = _lastPage;

            var id = page % 10;
            var x = id == 0 ? page - 10 : (page / 10) * 10;

            btn_Page1.Text = (x + 1).ToString();

            btn_Page2.Text = (x + 2).ToString();
            btn_Page2.Enabled = x + 2 < lastPage;

            btn_Page3.Text = (x + 3).ToString();
            btn_Page3.Enabled = x + 3 < lastPage;

            btn_Page4.Text = (x + 4).ToString();
            btn_Page4.Enabled = x + 4 < lastPage;

            btn_Page5.Text = (x + 5).ToString();
            btn_Page5.Enabled = x + 5 < lastPage;

            btn_Page6.Text = (x + 6).ToString();
            btn_Page6.Enabled = x + 6 < lastPage;

            btn_Page7.Text = (x + 7).ToString();
            btn_Page7.Enabled = x + 7 < lastPage;

            btn_Page8.Text = (x + 8).ToString();
            btn_Page8.Enabled = x + 8 < lastPage;

            btn_Page9.Text = (x + 9).ToString();
            btn_Page9.Enabled = x + 9 < lastPage;

            btn_Page10.Text = id == 0 ? page.ToString() : (x + 10).ToString();
            btn_Page10.Enabled = x + 10 < lastPage;


            var bgColor = Color.DarkBlue;
            var color = Color.White;

            switch (id)
            {
                case 1:
                    btn_Page1.BackColor = bgColor;
                    btn_Page1.ForeColor = color;
                    break;
                case 2:
                    btn_Page2.BackColor = bgColor;
                    btn_Page2.ForeColor = color;
                    break;
                case 3:
                    btn_Page3.BackColor = bgColor;
                    btn_Page3.ForeColor = color;
                    break;
                case 4:
                    btn_Page4.BackColor = bgColor;
                    btn_Page4.ForeColor = color;

                    break;
                case 5:
                    btn_Page5.BackColor = bgColor;
                    btn_Page5.ForeColor = color;
                    break;
                case 6:
                    btn_Page6.BackColor = bgColor;
                    btn_Page6.ForeColor = color;
                    break;
                case 7:
                    btn_Page7.BackColor = bgColor;
                    btn_Page7.ForeColor = color;
                    break;
                case 8:
                    btn_Page8.BackColor = bgColor;
                    btn_Page8.ForeColor = color;
                    break;
                case 9:
                    btn_Page9.BackColor = bgColor;
                    btn_Page9.ForeColor = color;
                    break;
                case 0:
                    btn_Page10.BackColor = bgColor;
                    btn_Page10.ForeColor = color;
                    break;
            }



        }

        private void Export()
        {
            var query = @"
;WITH
acc AS(
	SELECT
		PostType as [Type],
		ac.ID,
		CONCAT(ac.StoreCode, ' : ', l.LocationName )as Location,
		CONCAT(c.CategoryCode, ' : ', c.CategoryName) as Category,
		CONCAT(c.SubCategoryCode, ' : ', c.SubCategoryName) as SubCategory,
		CONCAT(c.ItemGroupCode, ' : ', c.ItemGroupName) as ItemGroup,
		A1 as Department,
		A2 as Account

	FROM
		[ADMIN].AccountMapp ac
		JOIN [IN].StoreLocation l ON l.LocationCode=ac.StoreCode
		JOIN [IN].vProdCatList c ON c.ItemGroupCode=ac.ItemGroupCode
	WHERE
		BusinessUnitCode=(SELECT TOP(1) BuCode FROM [ADMIN].Bu)
		AND PostType='AP'
)
SELECT
	*
FROM
	acc
ORDER BY
	[Location],
	[Category],
	[SubCategory],
	[ItemGroup]";

            if (_postType == "GL")
            {
                query = @"
;WITH
acc AS(
	SELECT
		PostType as [Type],
		ac.ID,
		CONCAT(ac.StoreCode, ' : ', l.LocationName )as Location,
		CONCAT(c.CategoryCode, ' : ', c.CategoryName) as Category,
		CONCAT(c.SubCategoryCode, ' : ', c.SubCategoryName) as SubCategory,
		CONCAT(c.ItemGroupCode, ' : ', c.ItemGroupName) as ItemGroup,
		A1 as MovementType,
		A2 as [Dr. Department],
		A3 as [Dr. Account],
		A4 as [Cr. Department],
		A5 as [Cr. Account]

	FROM
		[ADMIN].AccountMapp ac
		JOIN [IN].StoreLocation l ON l.LocationCode=ac.StoreCode
		JOIN [IN].vProdCatList c ON c.ItemGroupCode=ac.ItemGroupCode
	WHERE
		BusinessUnitCode=(SELECT TOP(1) BuCode FROM [ADMIN].Bu)
		AND PostType='GL'
)
SELECT
	*
FROM
	acc
ORDER BY
	[Location],
	[Category],
	[SubCategory],
	[ItemGroup]";
            }

            var dt = new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery(query);

            if (dt != null & dt.Rows.Count > 0)
            {
                var csv = new StringBuilder();

                var columns = "";
                foreach (DataColumn col in dt.Columns)
                {
                    columns += col.ColumnName + ",";
                }

                csv.AppendLine(columns.TrimEnd(','));

                foreach (DataRow dr in dt.Rows)
                {
                    var values = "";
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        values += dr[i].ToString() + ",";
                    }
                    csv.AppendLine(values.TrimEnd(','));
                }


                // Save to a file
                var buCode = LoginInfo.BuInfo.BuCode;
                var filename = string.Format("{0}-Account-Mapping-{1}-{2}.csv", buCode, _postType, DateTime.Today.ToString("yyyyMMdd"));

                SaveAsCsv(csv.ToString(), filename);
            }
        }

        // API
        private void GetAPI_AccDep()
        {
            var configString = _config.GetValue("APP", "INTF", "ACCOUNT", LoginInfo.ConnStr);

            if (string.IsNullOrEmpty(configString))
                return;


            var config = new KeyValues();

            // set config key/value
            config.Text = configString;

            var type = config.Value("type").ToUpper();

            if (type == "API")
            {
                try
                {
                    var host = config.Value("host").Trim().TrimEnd('/');
                    var authorization = config.Value("auth");

                    var endpoint_account = config.Value("accountcode").Trim().TrimStart('/').TrimEnd('/');
                    var endpoint_department = config.Value("department").Trim().TrimStart('/').TrimEnd('/');

                    using (var client = new WebClient())
                    {
                        client.BaseAddress = host;
                        client.Headers.Add("Authorization", authorization);

                        var sql = new Helpers.SQL(LoginInfo.ConnStr);
                        var queries = new StringBuilder();
                        var values = new StringBuilder();

                        // Department
                        var jsonDep = Encoding.UTF8.GetString(client.DownloadData(new Uri(string.Format("{0}/{1}", host, endpoint_department))));

                        var dataDep = JObject.Parse(jsonDep);
                        var listDep = JsonConvert.DeserializeObject<IEnumerable<Department>>(dataDep["Data"].ToString()).ToArray();
                        var param_Dep = new List<SqlParameter>();

                        queries.AppendLine("DELETE FROM [ADMIN].AccountDepartment;");
                        queries.AppendLine("INSERT INTO [ADMIN].AccountDepartment (Id, DeptCode, DeptDesc, UpdateDate, UpdateBy) VALUES");

                        values.Clear();
                        for (int i = 0; i < listDep.Length; i++)
                        {
                            values.AppendFormat("(@id{0}, @code{0}, @desc{0}, @date{0}, @by{0}),", i);

                            param_Dep.Add(new SqlParameter(string.Format("id{0}", i), listDep[i].Id));
                            param_Dep.Add(new SqlParameter(string.Format("code{0}", i), listDep[i].DeptCode));
                            param_Dep.Add(new SqlParameter(string.Format("desc{0}", i), listDep[i].Description ?? ""));
                            param_Dep.Add(new SqlParameter(string.Format("date{0}", i), listDep[i].LastModified));
                            param_Dep.Add(new SqlParameter(string.Format("by{0}", i), listDep[i].UserModified ?? ""));
                        }

                        queries.AppendLine(values.ToString().TrimEnd(','));
                        sql.ExecuteQuery(queries.ToString(), param_Dep.ToArray());


                        // Account
                        var jsonAcc = Encoding.UTF8.GetString(client.DownloadData(new Uri(string.Format("{0}/{1}", host, endpoint_account))));

                        var dataAcc = JObject.Parse(jsonAcc);
                        var listAcc = JsonConvert.DeserializeObject<IEnumerable<Account>>(dataAcc["Data"].ToString()).ToArray();


                        sql.ExecuteQuery("DELETE FROM [ADMIN].AccountCode");

                        queries.Clear();
                        values.Clear();

                        for (int i = 0; i < listAcc.Length; i++)
                        {
                            var param_Acc = new List<SqlParameter>();
                            queries.Clear();

                            queries.AppendFormat("INSERT INTO [ADMIN].AccountCode (Id, AccCode, AccDesc1, AccDesc2, AccNature, AccType, IsActive, UpdateDate, UpdateBy) VALUES(@id{0}, @code{0}, @desc1{0}, @desc2{0}, @nature{0}, @type{0}, @active{0}, @date{0}, @by{0}) ", i);

                            param_Acc.Add(new SqlParameter(string.Format("id{0}", i), listAcc[i].Id));
                            param_Acc.Add(new SqlParameter(string.Format("code{0}", i), listAcc[i].AccCode));
                            param_Acc.Add(new SqlParameter(string.Format("desc1{0}", i), listAcc[i].Description ?? ""));
                            param_Acc.Add(new SqlParameter(string.Format("desc2{0}", i), listAcc[i].Description2 ?? ""));

                            param_Acc.Add(new SqlParameter(string.Format("nature{0}", i), listAcc[i].Nature ?? ""));
                            param_Acc.Add(new SqlParameter(string.Format("type{0}", i), listAcc[i].Type ?? ""));
                            param_Acc.Add(new SqlParameter(string.Format("active{0}", i), listAcc[i].Active));

                            param_Acc.Add(new SqlParameter(string.Format("date{0}", i), listAcc[i].LastModified));
                            param_Acc.Add(new SqlParameter(string.Format("by{0}", i), listAcc[i].UserModified ?? ""));

                            sql.ExecuteQuery(queries.ToString(), param_Acc.ToArray());
                        }

                        //queries.AppendLine(values.ToString().Trim().TrimEnd(','));
                        //sql.ExecuteQuery(queries.ToString(), param_Acc.ToArray());


                        lbl_Pop_Alert.Text = "Done";
                        pop_Alert.ShowOnPageLoad = true;

                    }
                }
                catch (WebException ex)
                {
                    lbl_Pop_Alert.Text = ex.InnerException.Message;
                    pop_Alert.ShowOnPageLoad = true;
                }
            }
        }

        #endregion

        #region --Classes--

        [Serializable]
        public class Department
        {
            public int Id { get; set; }
            public string DeptCode { get; set; }
            public string Description { get; set; }
            public string UserModified { get; set; }
            public DateTime LastModified { get; set; }
        }

        [Serializable]
        public class Account
        {
            public int Id { get; set; }
            public string AccCode { get; set; }
            public string Description { get; set; }
            public string Description2 { get; set; }
            public string Nature { get; set; }
            public string Type { get; set; }
            public bool Active { get; set; }
            public string UserModified { get; set; }
            public DateTime LastModified { get; set; }
        }


        #endregion

    }
}