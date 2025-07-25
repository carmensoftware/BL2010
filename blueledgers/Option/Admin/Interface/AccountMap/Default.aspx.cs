﻿using System;
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
    public partial class Default : BasePage
    {
        const string moduleID = "2.9.6";

        private readonly Blue.BL.APP.Config _config = new Blue.BL.APP.Config();
        private readonly Blue.BL.ADMIN.RolePermission permission = new Blue.BL.ADMIN.RolePermission();

        protected bool _hasPermissionEdit
        {
            get
            {
                var pagePermiss = permission.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);

                return pagePermiss >= 3;
            }
        }

        protected DataTable _dtData
        {
            get { return ViewState["_dtAP"] as DataTable; }
            set { ViewState["_dtAP"] = value; }
        }

        protected AccountMappView _accMapView
        {
            get { return ViewState["_accMapView"] as AccountMappView; }
            set { ViewState["_accMapView"] = value; }
        }

        protected IEnumerable<ListItem> _accountCodes
        {
            get { return ViewState["_accountCodes"] as IEnumerable<ListItem>; }
            set { ViewState["_accountCodes"] = value; }
        }

        protected IEnumerable<ListItem> _departmentCodes
        {
            get { return ViewState["_departmentCodes"] as IEnumerable<ListItem>; }
            set { ViewState["_departmentCodes"] = value; }
        }

        #region --Event(s)--

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }


        }

        private void Page_Retrieve()
        {
            _departmentCodes = GetDepartments();
            _accountCodes = GetAccounts();


            Page_Setting();
        }

        private void Page_Setting()
        {
            // Set AP
            var postType = ddl_View.SelectedValue.ToString();
            SetData(postType);
            BindGridView();

            SetEditMode(false);
        }


        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                switch (e.Item.Name.ToUpper())
                {
                    case "GETNEW":
                        //GetNewCode();
                        break;

                    case "IMPORT":
                        //pop_ImportExport.ShowOnPageLoad = true;
                        break;

                    case "PRINT":
                        //Session["AccountMappPrint"] = GetData1(true);
                        //ScriptManager.RegisterStartupScript(Page, GetType(), "print", string.Format("<script>window.open('AccountMappPrint.aspx?type={0}', 'Print');</script>", _postType), false);
                        break;
                }
            }
        }

        protected void ddl_View_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = (sender as ASPxComboBox).SelectedItem.Value.ToString();

            SetData(value);
            BindGridView();

            //_sortColumn = "LocationCode";
            //_sortDirection = "ASC";

            //var items = gv_Data.Columns.Cast<DataControlField>().ToArray();

            //foreach (var item in items)
            //{
            //    item.Visible = true;
            //}

            //var hideType = _postType == "AP" ? "GL" : "AP";

            //var listHide = gv_Data.Columns.Cast<DataControlField>().Where(x => x.ControlStyle.CssClass.Contains(hideType)).ToArray();

            //foreach (var item in listHide)
            //{
            //    item.Visible = false;
            //}

            //txt_Search.Text = "";

            //GetData(_rows, 1);
        }

        protected void txt_Search_TextChanged(object sender, EventArgs e)
        {
            //GetData(_rows, 1);
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            //GetData(_rows, 1);
        }


        // gv_AP


        protected void gv_AP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var gv = sender as GridView;
            var isEdit = _hasPermissionEdit;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem;

                var a1 = DataBinder.Eval(dataItem, "A1").ToString();
                var a2 = DataBinder.Eval(dataItem, "A2").ToString();
                var a3 = DataBinder.Eval(dataItem, "A3").ToString();
                var a4 = DataBinder.Eval(dataItem, "A4").ToString();
                var a5 = DataBinder.Eval(dataItem, "A5").ToString();
                var a6 = DataBinder.Eval(dataItem, "A6").ToString();
                var a7 = DataBinder.Eval(dataItem, "A7").ToString();
                var a8 = DataBinder.Eval(dataItem, "A8").ToString();
                var a9 = DataBinder.Eval(dataItem, "A9").ToString();

                // A1
                if (e.Row.FindControl("lbl_A1_Desc") != null)
                {
                    var lbl = e.Row.FindControl("lbl_A1_Desc") as Label;

                    AssignName(lbl, _accMapView.TypeA1, a1);
                }


                if (e.Row.FindControl("txt_A1") != null)
                {
                    var txt = e.Row.FindControl("txt_A1") as TextBox;

                    txt.Text = a1;
                    txt.Visible = _accMapView.A1 && string.IsNullOrEmpty(_accMapView.TypeA1);

                }

                if (e.Row.FindControl("ddl_A1") != null)
                {
                    var ddl = e.Row.FindControl("ddl_A1") as ASPxComboBox;

                    ddl.Value = a1;
                    ddl.Visible = _accMapView.A1 && !string.IsNullOrEmpty(_accMapView.TypeA1);
                    AssignDropdownList(ddl, _accMapView.TypeA1, a1);
                }

                // A2
                if (e.Row.FindControl("lbl_A2_Desc") != null)
                {
                    var lbl = e.Row.FindControl("lbl_A2_Desc") as Label;

                    AssignName(lbl, _accMapView.TypeA2, a2);
                }

                if (e.Row.FindControl("txt_A2") != null)
                {
                    var txt = e.Row.FindControl("txt_A2") as TextBox;

                    txt.Text = a2;
                    txt.Visible = _accMapView.A2 && string.IsNullOrEmpty(_accMapView.TypeA2);
                }

                if (e.Row.FindControl("ddl_A2") != null)
                {
                    var ddl = e.Row.FindControl("ddl_A2") as ASPxComboBox;

                    ddl.Value = a2;
                    ddl.Visible = _accMapView.A2 && !string.IsNullOrEmpty(_accMapView.TypeA2);
                    AssignDropdownList(ddl, _accMapView.TypeA2, a2);
                }

                // A3
                if (e.Row.FindControl("lbl_A3_Desc") != null)
                {
                    var lbl = e.Row.FindControl("lbl_A3_Desc") as Label;

                    AssignName(lbl, _accMapView.TypeA3, a3);
                }

                if (e.Row.FindControl("txt_A3") != null)
                {
                    var txt = e.Row.FindControl("txt_A3") as TextBox;

                    txt.Text = a3;
                    txt.Visible = _accMapView.A3 && string.IsNullOrEmpty(_accMapView.TypeA3);
                }

                if (e.Row.FindControl("ddl_A3") != null)
                {
                    var ddl = e.Row.FindControl("ddl_A3") as ASPxComboBox;

                    ddl.Value = a3;
                    ddl.Visible = _accMapView.A3 && !string.IsNullOrEmpty(_accMapView.TypeA3);
                    AssignDropdownList(ddl, _accMapView.TypeA3, a3);
                }

                // A4
                if (e.Row.FindControl("lbl_A4_Desc") != null)
                {
                    var lbl = e.Row.FindControl("lbl_A4_Desc") as Label;

                    AssignName(lbl, _accMapView.TypeA4, a4);
                }

                if (e.Row.FindControl("txt_A4") != null)
                {
                    var txt = e.Row.FindControl("txt_A4") as TextBox;

                    txt.Text = a4;
                    txt.Visible = _accMapView.A4 && string.IsNullOrEmpty(_accMapView.TypeA4);

                }


                if (e.Row.FindControl("ddl_A4") != null)
                {
                    var ddl = e.Row.FindControl("ddl_A4") as ASPxComboBox;

                    ddl.Value = a4;
                    ddl.Visible = _accMapView.A4 && !string.IsNullOrEmpty(_accMapView.TypeA4);
                    AssignDropdownList(ddl, _accMapView.TypeA4, a4);
                }

                // A5
                if (e.Row.FindControl("lbl_A5_Desc") != null)
                {
                    var lbl = e.Row.FindControl("lbl_A5_Desc") as Label;

                    AssignName(lbl, _accMapView.TypeA5, a5);
                }

                if (e.Row.FindControl("txt_A5") != null)
                {
                    var txt = e.Row.FindControl("txt_A5") as TextBox;

                    txt.Text = a5;
                    txt.Visible = _accMapView.A5 && string.IsNullOrEmpty(_accMapView.TypeA5);
                }

                if (e.Row.FindControl("ddl_A5") != null)
                {
                    var ddl = e.Row.FindControl("ddl_A5") as ASPxComboBox;

                    ddl.Value = a5;
                    ddl.Visible = _accMapView.A5 && !string.IsNullOrEmpty(_accMapView.TypeA5);
                    AssignDropdownList(ddl, _accMapView.TypeA5, a5);
                }

                // A6
                if (e.Row.FindControl("lbl_A6_Desc") != null)
                {
                    var lbl = e.Row.FindControl("lbl_A6_Desc") as Label;

                    AssignName(lbl, _accMapView.TypeA6, a6);
                }

                if (e.Row.FindControl("txt_A6") != null)
                {
                    var txt = e.Row.FindControl("txt_A6") as TextBox;

                    txt.Text = a6;
                    txt.Visible = _accMapView.A6 && string.IsNullOrEmpty(_accMapView.TypeA6);
                }

                if (e.Row.FindControl("ddl_A6") != null)
                {
                    var ddl = e.Row.FindControl("ddl_A6") as ASPxComboBox;

                    ddl.Value = a6;
                    ddl.Visible = _accMapView.A6 && !string.IsNullOrEmpty(_accMapView.TypeA6);
                    AssignDropdownList(ddl, _accMapView.TypeA6, a6);
                }

                // A7
                if (e.Row.FindControl("lbl_A7_Desc") != null)
                {
                    var lbl = e.Row.FindControl("lbl_A7_Desc") as Label;

                    AssignName(lbl, _accMapView.TypeA7, a7);
                }

                if (e.Row.FindControl("txt_A7") != null)
                {
                    var txt = e.Row.FindControl("txt_A7") as TextBox;

                    txt.Text = a7;
                    txt.Visible = _accMapView.A7 && string.IsNullOrEmpty(_accMapView.TypeA7);
                }

                if (e.Row.FindControl("ddl_A7") != null)
                {
                    var ddl = e.Row.FindControl("ddl_A7") as ASPxComboBox;

                    ddl.Value = a7;
                    ddl.Visible = _accMapView.A7 && !string.IsNullOrEmpty(_accMapView.TypeA7);
                    AssignDropdownList(ddl, _accMapView.TypeA7, a7);
                }

                // A8
                if (e.Row.FindControl("lbl_A8_Desc") != null)
                {
                    var lbl = e.Row.FindControl("lbl_A8_Desc") as Label;

                    AssignName(lbl, _accMapView.TypeA8, a8);
                }

                if (e.Row.FindControl("txt_A8") != null)
                {
                    var txt = e.Row.FindControl("txt_A8") as TextBox;

                    txt.Text = a8;
                    txt.Visible = _accMapView.A8 && string.IsNullOrEmpty(_accMapView.TypeA8);
                }

                if (e.Row.FindControl("ddl_A8") != null)
                {
                    var ddl = e.Row.FindControl("ddl_A8") as ASPxComboBox;

                    ddl.Value = a8;
                    ddl.Visible = _accMapView.A8 && !string.IsNullOrEmpty(_accMapView.TypeA8);
                    AssignDropdownList(ddl, _accMapView.TypeA8, a8);
                }

                // A9
                if (e.Row.FindControl("lbl_A9_Desc") != null)
                {
                    var lbl = e.Row.FindControl("lbl_A9_Desc") as Label;

                    AssignName(lbl, _accMapView.TypeA9, a9);
                }

                if (e.Row.FindControl("txt_A9") != null)
                {
                    var txt = e.Row.FindControl("txt_A9") as TextBox;

                    txt.Text = a9;
                    txt.Visible = _accMapView.A9 && string.IsNullOrEmpty(_accMapView.TypeA9);
                }

                if (e.Row.FindControl("ddl_A9") != null)
                {
                    var ddl = e.Row.FindControl("ddl_A9") as ASPxComboBox;

                    ddl.Value = a9;
                    ddl.Visible = _accMapView.A9 && !string.IsNullOrEmpty(_accMapView.TypeA9);
                    AssignDropdownList(ddl, _accMapView.TypeA9, a9);
                }

            }
        }

        protected void gv_AP_RowEditing(object sender, GridViewEditEventArgs e)
        {
            var gv = sender as GridView;

            gv.EditIndex = e.NewEditIndex;
            BindGridView();

            SetEditMode(true);
        }

        protected void gv_AP_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //Reset the edit index.
            (sender as GridView).EditIndex = -1;
            //Bind data to the GridView control.
            BindGridView();

            SetEditMode(false);
        }

        protected void gv_AP_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //var row = (sender as GridView).Rows[e.RowIndex] as GridViewRow;
            //var hf_ID = row.FindControl("hf_ID") as HiddenField;
            //var id = hf_ID.Value;


            //var ddl_DepCode = row.FindControl("ddl_DepCode") as ASPxComboBox;
            //var ddl_AccCode = row.FindControl("ddl_AccCode") as ASPxComboBox;

            //var sql = new Helpers.SQL(LoginInfo.ConnStr);
            //var query = @"UPDATE [ADMIN].AccountMapp SET A1=@DepCode, A2=@AccCode WHERE ID=@Id";

            //var department = ddl_DepCode.Text.Split(':');
            //var account = ddl_AccCode.Text.Split(':');

            //var depCode = department[0].Trim();
            //var depName = department.Length > 1 ? department[1].Trim() : "";
            //var accCode = account[0].Trim();
            //var accName = accCode.Length > 1 ? account[1].Trim() : "";


            //sql.ExecuteQuery(query, new SqlParameter[]
            //    {
            //        new SqlParameter("ID", id),
            //        new SqlParameter("DepCode", depCode),
            //        new SqlParameter("AccCode", accCode)
            //    });

            //var dr = _dtAP.AsEnumerable().FirstOrDefault(x => x.Field<string>("ID") == id);

            //if (dr != null)
            //{
            //    dr["DepCode"] = depCode;
            //    dr["DepName"] = depName;
            //    dr["AccCode"] = accCode;
            //    dr["AccName"] = accName;
            //    dr.AcceptChanges();
            //}



            //SetEditMode(false);
            ////Reset the edit index.
            //(sender as GridView).EditIndex = -1;

            ////Bind data to the GridView control.
            //BindAP();
        }

        protected void gv_AP_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            (sender as GridView).PageIndex = e.NewPageIndex;
            BindGridView();
        }

        protected void gv_AP_Sorting(object sender, GridViewSortEventArgs e)
        {
            //_sortColumn = e.SortExpression;
            //_sortDirection = _sortDirection == "ASC" ? "DESC" : "ASC";

            //var page = _page;
            //var rows = _rows;

            //GetData(rows, page);
        }

        #endregion


        #region --Method(s)--

        private void SetEditMode(bool isEdit)
        {
            ddl_View.Enabled = !isEdit;
            txt_Search.Enabled = !isEdit;
            btn_Search.Enabled = !isEdit;
        }

        private void SetAccMapView()
        {
            var sql = new Helpers.SQL(LoginInfo.ConnStr);

            var postType = ddl_View.SelectedValue.ToString();
            var dtAccMapView = sql.ExecuteQuery("SELECT * FROM [ADMIN].AccountMappView WHERE PostType=@PostType", new SqlParameter[] { new SqlParameter("@PostType", postType) });

            if (dtAccMapView != null && dtAccMapView.Rows.Count > 0)
            {

                // Set AccountMappView
                _accMapView = dtAccMapView.AsEnumerable()
                    .Select(x => new AccountMappView
                    {
                        ID = x.Field<int>("ID"),
                        StoreCode = x.Field<bool>("StoreCode"),
                        CategoryCode = x.Field<bool>("CategoryCode"),
                        SubCategoryCode = x.Field<bool>("SubCategoryCode"),
                        ItemGroupCode = x.Field<bool>("ItemGroupCode"),

                        KeyA1 = x.Field<bool>("KeyA1"),
                        KeyA2 = x.Field<bool>("KeyA2"),
                        KeyA3 = x.Field<bool>("KeyA3"),
                        KeyA4 = x.Field<bool>("KeyA4"),
                        KeyA5 = x.Field<bool>("KeyA5"),
                        KeyA6 = x.Field<bool>("KeyA6"),
                        KeyA7 = x.Field<bool>("KeyA7"),
                        KeyA8 = x.Field<bool>("KeyA8"),
                        KeyA9 = x.Field<bool>("KeyA9"),

                        DescKeyA1 = x.Field<string>("DescKeyA1"),
                        DescKeyA2 = x.Field<string>("DescKeyA2"),
                        DescKeyA3 = x.Field<string>("DescKeyA3"),
                        DescKeyA4 = x.Field<string>("DescKeyA4"),
                        DescKeyA5 = x.Field<string>("DescKeyA5"),
                        DescKeyA6 = x.Field<string>("DescKeyA6"),
                        DescKeyA7 = x.Field<string>("DescKeyA7"),
                        DescKeyA8 = x.Field<string>("DescKeyA8"),
                        DescKeyA9 = x.Field<string>("DescKeyA9"),

                        A1 = x.Field<bool>("A1"),
                        A2 = x.Field<bool>("A2"),
                        A3 = x.Field<bool>("A3"),
                        A4 = x.Field<bool>("A4"),
                        A5 = x.Field<bool>("A5"),
                        A6 = x.Field<bool>("A6"),
                        A7 = x.Field<bool>("A7"),
                        A8 = x.Field<bool>("A8"),
                        A9 = x.Field<bool>("A9"),

                        DescA1 = x.Field<string>("DescA1"),
                        DescA2 = x.Field<string>("DescA2"),
                        DescA3 = x.Field<string>("DescA3"),
                        DescA4 = x.Field<string>("DescA4"),
                        DescA5 = x.Field<string>("DescA5"),
                        DescA6 = x.Field<string>("DescA6"),
                        DescA7 = x.Field<string>("DescA7"),
                        DescA8 = x.Field<string>("DescA8"),
                        DescA9 = x.Field<string>("DescA9"),

                        TypeA1 = x.Field<string>("TypeA1"),
                        TypeA2 = x.Field<string>("TypeA2"),
                        TypeA3 = x.Field<string>("TypeA3"),
                        TypeA4 = x.Field<string>("TypeA4"),
                        TypeA5 = x.Field<string>("TypeA5"),
                        TypeA6 = x.Field<string>("TypeA6"),
                        TypeA7 = x.Field<string>("TypeA7"),
                        TypeA8 = x.Field<string>("TypeA8"),
                        TypeA9 = x.Field<string>("TypeA9"),

                    })
                    .FirstOrDefault();
            }
        }

        private void SetData(string postType)
        {
            var query = "";

            if (postType.ToUpper() == "AP")
            {
                #region --AP--
                query = @"
SELECT
	ac.ID,
	ac.StoreCode as LocationCode,
	ac.CategoryCode,
	ac.SubCategoryCode,
	ac.ItemGroupCode,
	A1,
	A2,
	A3,
	A4,
	A5,
	A6,
	A7,
	A8,
	A9,

	l.LocationName,
	c.CategoryName,
	c.SubCategoryName,
	c.ItemGroupName
FROM
	[ADMIN].AccountMapp ac
	LEFT JOIN [IN].StoreLocation l ON l.LocationCode=ac.StoreCode
	LEFT JOIN [IN].vProdCatList c ON c.ItemGroupCode=ac.ItemGroupCode
WHERE
	PostType=@PostType
ORDER BY
	ac.StoreCode,
	ac.CategoryCode,
	ac.ItemGroupCode";
            #endregion
            }
            // GL
            else
            {
                #region --GL--
                query = @"
SELECT
	ac.ID,
	ac.LocationCode,
	ac.CategoryCode,
	ac.SubCategoryCode,
	ac.ItemGroupCode,
    ac.AdjCode,
	A1,
	A2,
	A3,
	A4,
	A5,
	A6,
	A7,
	A8,
	A9,

	l.LocationName,
	c.CategoryName,
	c.SubCategoryName,
	c.ItemGroupName,
	CASE ac.AdjCode
		WHEN 'EOPI' THEN 'EOP-IN'
		WHEN 'EOPO' THEN 'EOP-OUT'
		ELSE t.AdjName
	END AdjName
FROM
	[ADMIN].AccountMapp ac
	LEFT JOIN [IN].StoreLocation l ON l.LocationCode=ac.LocationCode
	LEFT JOIN [IN].vProdCatList c ON c.ItemGroupCode=ac.ItemGroupCode
	LEFT JOIN [IN].AdjType t ON t.AdjCode=ac.AdjCode
WHERE
	PostType='GL'
ORDER BY
	ac.LocationCode,
	ac.CategoryCode,
	ac.ItemGroupCode,
	ac.AdjCode";
                #endregion
            }

            var sql = new Helpers.SQL(LoginInfo.ConnStr);

            _dtData = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@PostType", postType) });
        }

        private void BindGridView()
        {
            SetAccMapView();

            //Location
            gv_AP.Columns[0].Visible = _accMapView.StoreCode;
            // Category
            gv_AP.Columns[1].Visible = _accMapView.CategoryCode;
            // Sub-Category
            gv_AP.Columns[2].Visible = _accMapView.SubCategoryCode;
            // ItemGroup
            gv_AP.Columns[3].Visible = _accMapView.ItemGroupCode;

            // A1
            gv_AP.Columns[4].Visible = _accMapView.A1;
            gv_AP.Columns[4].HeaderText = _accMapView.DescA1;
            // A2
            gv_AP.Columns[5].Visible = _accMapView.A2;
            gv_AP.Columns[5].HeaderText = _accMapView.DescA2;
            // A3
            gv_AP.Columns[6].Visible = _accMapView.A3;
            gv_AP.Columns[6].HeaderText = _accMapView.DescA3;
            // A4
            gv_AP.Columns[7].Visible = _accMapView.A4;
            gv_AP.Columns[7].HeaderText = _accMapView.DescA4;
            // A5
            gv_AP.Columns[8].Visible = _accMapView.A5;
            gv_AP.Columns[8].HeaderText = _accMapView.DescA5;
            // A6
            gv_AP.Columns[9].Visible = _accMapView.A6;
            gv_AP.Columns[9].HeaderText = _accMapView.DescA6;
            // A7
            gv_AP.Columns[10].Visible = _accMapView.A7;
            gv_AP.Columns[10].HeaderText = _accMapView.DescA7;
            // A8
            gv_AP.Columns[11].Visible = _accMapView.A8;
            gv_AP.Columns[11].HeaderText = _accMapView.DescA8;
            // A9
            gv_AP.Columns[12].Visible = _accMapView.A9;
            gv_AP.Columns[12].HeaderText = _accMapView.DescA9;

            gv_AP.DataSource = _dtData;
            gv_AP.DataBind();
        }

        private IEnumerable<ListItem> GetAccounts()
        {
            var dt = new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery("SELECT AccCode as [Code], AccDesc1 as [Name1], AccDesc2 as [Name2] FROM [ADMIN].AccountCode ORDER BY AccCode");

            var items = dt.AsEnumerable()
                .Select(x => new ListItem
                {
                    Code = x.Field<string>("Code"),
                    Name1 = x.Field<string>("Name1"),
                    Name2 = x.Field<string>("Name2")
                })
                .ToArray();
            return items;
        }

        private IEnumerable<ListItem> GetDepartments()
        {
            var dt = new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery("SELECT DeptCode as [Code], DeptDesc as [Name1] FROM [ADMIN].AccountDepartment ORDER BY DeptCode");

            var items = dt.AsEnumerable()
                .Select(x => new ListItem
                {
                    Code = x.Field<string>("Code"),
                    Name1 = x.Field<string>("Name1"),
                    Name2 = x.Field<string>("Name1")
                })
                .ToArray();
            return items;
        }

        private ListEditItemCollection GetListEdit_Account(string selectedValue)
        {
            var items = new ListEditItemCollection();

            foreach (ListItem item in _accountCodes)
            {
                items.Add(new ListEditItem
                {
                    Value = item.Code,
                    Text = string.Format("{0} : {1} | {2}", item.Code, item.Name1, item.Name2),
                    Selected = item.Code == selectedValue
                });

            }

            return items;
        }

        private ListEditItemCollection GetListEdit_Department(string selectedValue)
        {
            var items = new ListEditItemCollection();

            foreach (ListItem item in _departmentCodes)
            {
                items.Add(new ListEditItem
                {
                    Value = item.Code,
                    Text = string.Format("{0} : {1}", item.Code, item.Name1),
                    Selected = item.Code == selectedValue
                });

            }

            return items;
        }

        private void AssignDropdownList(ASPxComboBox ddl, string type, string value = "")
        {
            var items = new ListEditItemCollection();

            switch (type.ToLower())
            {
                case "acccode":
                    items = GetListEdit_Account(value);
                    break;
                case "accdept":
                    items = GetListEdit_Department(value);
                    break;
            }

            ddl.Items.Clear();
            ddl.Items.AddRange(items);
            ddl.Width = new Unit("90%");
        }

        private void AssignName(Label label, string type, string code)
        {
            var name = "";


            switch (type.ToLower())
            {
                case "acccode":
                    var acc = _accountCodes.FirstOrDefault(x => x.Code.ToLower() == code.ToLower());

                    name = acc == null ? "" : string.Format("{0} | {1}", acc.Name1, acc.Name2);
                    break;
                case "accdept":
                    var dep = _departmentCodes.FirstOrDefault(x => x.Code.ToLower() == code.ToLower());

                    name = dep == null ? "" : dep.Name1;
                    break;
            }

            label.Text = name;
        }


        #endregion


        [Serializable]
        public class AccountMappView
        {
            public int ID { get; set; }
            public string ViewName { get; set; }
            public bool BusinessUnitCode { get; set; }
            public bool StoreCode { get; set; }
            public bool CategoryCode { get; set; }
            public bool SubCategoryCode { get; set; }
            public bool ItemGroupCode { get; set; }
            public bool KeyA1 { get; set; }
            public bool KeyA2 { get; set; }
            public bool KeyA3 { get; set; }
            public bool KeyA4 { get; set; }
            public bool KeyA5 { get; set; }
            public bool KeyA6 { get; set; }
            public bool KeyA7 { get; set; }
            public bool KeyA8 { get; set; }
            public bool KeyA9 { get; set; }
            public string DescKeyA1 { get; set; }
            public string DescKeyA2 { get; set; }
            public string DescKeyA3 { get; set; }
            public string DescKeyA4 { get; set; }
            public string DescKeyA5 { get; set; }
            public string DescKeyA6 { get; set; }
            public string DescKeyA7 { get; set; }
            public string DescKeyA8 { get; set; }
            public string DescKeyA9 { get; set; }
            public bool A1 { get; set; }
            public bool A2 { get; set; }
            public bool A3 { get; set; }
            public bool A4 { get; set; }
            public bool A5 { get; set; }
            public bool A6 { get; set; }
            public bool A7 { get; set; }
            public bool A8 { get; set; }
            public bool A9 { get; set; }
            public string DescA1 { get; set; }
            public string DescA2 { get; set; }
            public string DescA3 { get; set; }
            public string DescA4 { get; set; }
            public string DescA5 { get; set; }
            public string DescA6 { get; set; }
            public string DescA7 { get; set; }
            public string DescA8 { get; set; }
            public string DescA9 { get; set; }
            public string PostType { get; set; }
            public string TypeA1 { get; set; }
            public string TypeA2 { get; set; }
            public string TypeA3 { get; set; }
            public string TypeA4 { get; set; }
            public string TypeA5 { get; set; }
            public string TypeA6 { get; set; }
            public string TypeA7 { get; set; }
            public string TypeA8 { get; set; }
            public string TypeA9 { get; set; }
        }


        [Serializable]
        public class ListItem
        {
            public string Code { get; set; }
            public string Name1 { get; set; }
            public string Name2 { get; set; }
        }
    }
}