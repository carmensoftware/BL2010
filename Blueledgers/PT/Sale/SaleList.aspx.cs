using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlueLedger.PL.PT.Sale
{
    public partial class SaleList : BasePage
    {
        private readonly string moduleID = "4.3";
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();

        private readonly string[] ImportFields = { "SaleDate", "OutletCode", "DepartmentCode", "ItemCode", "Qty", "Price", "Total" };
        public string iconEdit = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAAcQAAAHEBHD+AdwAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAEESURBVDiNldO/LuxRFMXxz5KJRqFSKCU6IUIp0XoGkXgBDUFHoldQ6XQ6iUqlcm8iOh5AoUGp9C83dysQMn4zmVnJKU72/q61T7KPqtLPwRL+4BAjqSpJJjHrWw9VdaZNSVaxginMYznYwRguf/TeVdVpAzyHexTWcQFXPYy9imO0Pu97uMbmAAbaR+2QvFhV/36Uzqtqt284yR5U1Zpu6b3AHQ2SrPQCNxokCTYwhFY32FdDm2bw18eynCS5wf8muNMTFvCECUzjuQlOspVktGmCJ7zgDFtV9dyUjHEM/zKoqv0OQKO67kGvBo9JRvqBkgz6+D+3LWzjKMlwHx5vOKiq13cd46KPLEvGfQAAAABJRU5ErkJggg==";


        private DataTable dtSale
        {
            get
            {
                if (Session["dtSale"] == null)
                    Session["dtSale"] = new DataTable();
                return (DataTable)Session["dtSale"];
            }

            set
            {
                Session["dtSale"] = value;
            }
        }

        private DataTable dtImport
        {
            get
            {
                if (Session["dtImport"] == null)
                    Session["dtImport"] = new DataTable();
                return (DataTable)Session["dtImport"];
            }

            set
            {
                Session["dtImport"] = value;
            }
        }

        private DataTable dtOutlet
        {
            get
            {
                if (Session["dtOutlet"] == null)
                    Session["dtOutlet"] = new DataTable();
                return (DataTable)Session["dtOutlet"];
            }

            set
            {
                Session["dtOutlet"] = value;
            }
        }

        private DataTable dtDepartment
        {
            get
            {
                if (Session["dtDepartment"] == null)
                    Session["dtDepartment"] = new DataTable();
                return (DataTable)Session["dtDepartment"];
            }

            set
            {
                Session["dtDepartment"] = value;
            }
        }

        private DataTable dtItem
        {
            get
            {
                if (Session["dtItem"] == null)
                    Session["dtItem"] = new DataTable();
                return (DataTable)Session["dtItem"];
            }

            set
            {
                Session["dtItem"] = value;
            }
        }


        private PosData _PosData
        {
            get { return (PosData)Session["PosData"]; }
            set { Session["PosData"] = value; }
        }

        private string _Date
        {
            get
            {
                var date = Request.QueryString["date"];

                return string.IsNullOrEmpty(date) ? DateTime.Today.ToString("yyyy-MM-dd") : date.ToString();
            }
        }

        // Event(s)

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Setting();

            }
            else
            {

            }
        }

        private void Page_Setting()
        {
            var date = Convert.ToDateTime(_Date);

            cal_Sale.SelectedDate = date;
            BindSaleData(date);
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "OUTLET":
                    ShowOutlet();
                    break;
                case "DEPARTMENT":
                    ShowDepartment();
                    break;
                case "ITEM":
                    ShowItem();
                    break;
                case "IMPORT":
                    dtImport = null;
                    BindImportData();
                    pop_Import.ShowOnPageLoad = true;
                    break;
                //case "STOCKOUT":
                //    pop_StockOut.ShowOnPageLoad = true;
                //    break;
                //case "INTERFACE":
                //    pop_Interface.ShowOnPageLoad = true;
                //    break;
                //case "PRINT":
                //    ShowAlert("Test");
                //    break;

            }
        }

        #region --Outlet--

        protected void btn_OutletAdd_Click(object sender, EventArgs e)
        {
            // Check existing code

            string outletCode = txt_NewOutletCode.Text;

            string sql = string.Format("SELECT COUNT(OutletCode) as RecordCount FROM PT.Outlet WHERE OutletCode='{0}'", outletCode);
            DataTable dt = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                ShowAlert(string.Format("'{0}' is duplicated.", outletCode));
            }
            else
            {
                string outletName = txt_NewOutletName.Text;
                string locationCode = ddl_NewOutletLocation.SelectedValue.ToString();
                sql = string.Format("INSERT INTO PT.Outlet (OutletCode, OutletName, LocationCode) VALUES(N'{0}', N'{1}', N'{2}') ", outletCode.ToUpper(), outletName, locationCode);
                bu.DbParseQuery(@sql, null, LoginInfo.ConnStr);
                BindOutletData();

                txt_NewOutletName.Text = string.Empty;
                txt_NewOutletName.Text = string.Empty;
            }

        }

        // Grid OutletItem

        protected void grd_Outlet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var gv = sender as GridView;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("ddl_LocationCode") != null)
                {
                    var ddl_LocationCode = e.Row.FindControl("ddl_LocationCode") as ASPxComboBox;

                    ddl_LocationCode.Value = DataBinder.Eval(e.Row.DataItem, "LocationCode");
                }
            }

        }

        protected void grd_Outlet_RowEditing(object sender, GridViewEditEventArgs e)
        {
            (sender as GridView).EditIndex = e.NewEditIndex;
            BindOutletData();

        }

        protected void grd_Outlet_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            (sender as GridView).EditIndex = -1;
            BindOutletData();
        }

        protected void grd_Outlet_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var txt_OutletName = grd_Outlet.Rows[e.RowIndex].FindControl("txt_OutletName") as TextBox;
            var ddl_LocationCode = grd_Outlet.Rows[e.RowIndex].FindControl("ddl_LocationCode") as ASPxComboBox;

            var code = grd_Outlet.DataKeys[e.RowIndex].Value.ToString();
            var name = txt_OutletName.Text.Trim();
            var locationCode = ddl_LocationCode.Value == null ? "" : ddl_LocationCode.SelectedItem.Value.ToString();

            var sql = string.Format("UPDATE PT.Outlet SET OutletName=@Name, LocationCode = N'{0}'WHERE OutletCode = N'{1}'", locationCode, code);
            var p = new List<Blue.DAL.DbParameter>();

            p.Add(new Blue.DAL.DbParameter("Name", name));
            bu.DbExecuteQuery(@sql, p.ToArray(), LoginInfo.ConnStr);

            (sender as GridView).EditIndex = -1;
            BindOutletData();
        }

        protected void grd_Outlet_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var code = grd_Outlet.DataKeys[e.RowIndex].Value.ToString();

            var sql = string.Format("SELECT TOP(1) OutletCode FROM PT.Sale WHERE OutletCode='{0}'", code);
            var dt = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            if (dt != null && dt.Rows.Count > 0)
            {
                ShowAlert(string.Format("No deleted. The '{0}' is exist in sale.", code), "Warning");

                return;
            }

            ShowConfirmDelete("Outlet", code);
        }

        // Controls
        protected void ddl_LocationCode_Load(object sender, EventArgs e)
        {
            var gv = sender as ASPxComboBox;

            gv.DataSource = GetLocation();
            gv.ValueField = "LocationCode";
            gv.DataBind();
        }

        // Private method(s)

        private void ShowOutlet()
        {
            BindOutletData();

            // Bind location
            ddl_NewOutletLocation.DataSource = GetLocation();
            ddl_NewOutletLocation.DataValueField = "LocationCode";
            ddl_NewOutletLocation.DataTextField = "Location";
            ddl_NewOutletLocation.DataBind();

            pop_Outlet.ShowOnPageLoad = true;

        }

        private void BindOutletData()
        {
            string sql = string.Empty;

            sql = "SELECT o.OutletCode, o.OutletName, OutletCode + ' : ' + ISNULL(OutletName,'') as Outlet, o.LocationCode, l.LocationCode + ' : ' + l.LocationName as Location";
            sql += " FROM PT.Outlet o";
            sql += " LEFT JOIN [IN].StoreLocation l ON o.LocationCode = l.LocationCode";
            sql += " ORDER BY o.OutletCode";

            //DataSet ds = new DataSet();

            //dtOutlet = null;
            //bu.DbExecuteQuery(sql, ds, null, "Table1", LoginInfo.ConnStr);
            //dtOutlet = ds.Tables["Table1"];

            dtOutlet = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            grd_Outlet.DataSource = dtOutlet;
            grd_Outlet.DataBind();
        }

        #endregion

        #region --Department--

        protected void btn_DepartmentAdd_Click(object sender, EventArgs e)
        {
            // Check existing code

            string depCode = txt_NewDepartmentCode.Text;

            string sql = string.Format("SELECT COUNT(DepartmentCode) as RecordCount FROM PT.Department WHERE DepartmentCode='{0}'", depCode);
            DataTable dt = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                ShowAlert(string.Format("'{0}' is duplicated.", depCode));
            }
            else
            {
                string depName = txt_NewDepartmentName.Text;
                string locationCode = ddl_NewOutletLocation.SelectedValue.ToString();
                sql = string.Format("INSERT INTO PT.Department (DepartmentCode, DepartmentName) VALUES(N'{0}', N'{1}') ", depCode.ToUpper(), depName);
                bu.DbParseQuery(@sql, null, LoginInfo.ConnStr);
                BindDepartmentData();

                txt_NewDepartmentCode.Text = string.Empty;
                txt_NewDepartmentName.Text = string.Empty;
            }

        }

        //Grid Department
        protected void grd_Department_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            (sender as GridView).PageIndex = e.NewPageIndex;
            (sender as GridView).DataSource = dtDepartment;
            (sender as GridView).DataBind();

        }

        protected void grd_Department_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                string departmentCode = ((sender as GridView).FooterRow.FindControl("txt_DepartmentCode_New") as TextBox).Text;
                string departmentName = ((sender as GridView).FooterRow.FindControl("txt_DepartmentName_New") as TextBox).Text;

                DataTable dt = bu.DbExecuteQuery(string.Format("SELECT TOP(1) * FROM PT.Department WHERE DepartmentCode = '{0}'", departmentCode), null, LoginInfo.ConnStr);
                if (dt.Rows.Count > 0)
                {
                    ShowAlert("Duplicate Department Code!");
                }
                else
                {
                    string sql = string.Format("INSERT INTO PT.Department (DepartmentCode, DepartmentName) VALUES('{0}','{1}')", departmentCode, departmentName);
                    bu.DbExecuteQuery(@sql, null, LoginInfo.ConnStr);

                    (sender as GridView).EditIndex = -1;
                    BindDepartmentData();
                }
            }
        }

        protected void grd_Department_RowEditing(object sender, GridViewEditEventArgs e)
        {
            (sender as GridView).EditIndex = e.NewEditIndex;
            BindDepartmentData();

        }

        protected void grd_Department_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            (sender as GridView).EditIndex = -1;
            BindDepartmentData();
        }

        protected void grd_Department_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string departmentName = ((sender as GridView).Rows[e.RowIndex].FindControl("txt_DepartmentName") as TextBox).Text;
            string departmentCode = (sender as GridView).DataKeys[e.RowIndex].Value.ToString();

            string sql = string.Format("UPDATE PT.Department SET DepartmentName = '{0}'", departmentName);
            sql += string.Format(" WHERE DepartmentCode = '{0}'", departmentCode);
            bu.DbExecuteQuery(@sql, null, LoginInfo.ConnStr);

            (sender as GridView).EditIndex = -1;
            BindDepartmentData();
        }

        protected void grd_Department_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string departmentCode = (sender as GridView).DataKeys[e.RowIndex].Value.ToString();

            string sql = string.Format("DELETE FROM PT.Department WHERE DepartmentCode = '{0}'", departmentCode);
            bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
            BindDepartmentData();
        }

        // Control(s)


        // Private method(s)

        private void ShowDepartment()
        {
            GetNewDepartment();

            BindDepartmentData();
            pop_Department.ShowOnPageLoad = true;

        }

        private void BindDepartmentData()
        {
            string sql = string.Empty;

            sql = "SELECT DepartmentCode, DepartmentName";
            sql += " FROM PT.Department";
            sql += " ORDER BY DepartmentCode";

            //DataSet ds = new DataSet();

            //dtDepartment = null;
            //bu.DbExecuteQuery(sql, ds, null, "Table1", LoginInfo.ConnStr);
            //dtDepartment = ds.Tables["Table1"];
            dtDepartment = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            grd_Department.DataSource = dtDepartment;
            grd_Department.DataBind();
        }


        private void GetNewDepartment()
        {
            string sql = "INSERT INTO PT.Department (DepartmentCode)";
            sql += " SELECT DISTINCT s.DepartmentCode";
            sql += " FROM PT.Sale s";
            sql += " LEFT JOIN PT.Department d ON d.DepartmentCode = s.DepartmentCode";
            sql += " WHERE d.DepartmentCode IS NULL";

            bu.DbExecuteQuery(@sql, null, LoginInfo.ConnStr);
        }


        #endregion

        #region --Item--

        protected void btn_ReloadItem_Click(object sender, EventArgs e)
        {
            txt_SearchItem.Text = "";
            BindItemData();
        }

        protected void btn_SearchItem_Click(object sender, EventArgs e)
        {
            BindItemData(txt_SearchItem.Text.Trim());
        }

        protected void btn_ItemAdd_Click(object sender, EventArgs e)
        {
            // Check existing code

            var code = txt_NewItemCode.Text;
            var name = txt_NewItemName.Text;
            var rcpCode = ddl_NewItemRecipe.SelectedItem == null ? "" : ddl_NewItemRecipe.SelectedItem.Value.ToString();

            string sql = string.Format("SELECT COUNT(ItemCode) as RecordCount FROM PT.Item WHERE ItemCode='{0}'", code);
            DataTable dt = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                ShowAlert(string.Format("'{0}' is duplicated.", code));
            }
            else
            {
                sql = "INSERT INTO PT.Item (ItemCode, ItemName, RcpCode) VALUES (@Code, @Name, @RcpCode)";
                var p = new List<Blue.DAL.DbParameter>();

                p.Add(new Blue.DAL.DbParameter("Code", code));
                p.Add(new Blue.DAL.DbParameter("Name", name));
                p.Add(new Blue.DAL.DbParameter("RcpCode", rcpCode));

                bu.DbParseQuery(@sql, p.ToArray(), LoginInfo.ConnStr);

                BindItemData();

                ShowAlert(string.Format("{0} is added.", code));

                txt_NewItemCode.Text = string.Empty;
                txt_NewItemName.Text = string.Empty;

            }

        }

        protected void ddl_ItemRecipe_Load(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;

            var sql = "SELECT RcpCode, RcpDesc1, RcpDesc2 FROM PT.Rcp WHERE IsActived=1 ORDER BY RcpCode";

            var dt = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
            ddl.DataSource = dt;
            ddl.DataBind();
        }

        //Grid Item

        protected void grd_Item_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var gv = sender as GridView;

            gv.PageIndex = e.NewPageIndex;

            BindItemData(txt_SearchItem.Text);
        }

        protected void grd_Item_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("ddl_ItemRecipe") != null)
                {
                    var ddl = e.Row.FindControl("ddl_ItemRecipe") as ASPxComboBox;

                    ddl.Value = DataBinder.Eval(e.Row.DataItem, "RcpCode");
                }
            }
        }

        protected void grd_Item_RowEditing(object sender, GridViewEditEventArgs e)
        {

            (sender as GridView).EditIndex = e.NewEditIndex;
            BindItemData(txt_SearchItem.Text);
        }

        protected void grd_Item_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var gv = sender as GridView;
            var ddl_ItemRecipe = gv.Rows[e.RowIndex].FindControl("ddl_ItemRecipe") as ASPxComboBox;

            var code = gv.DataKeys[e.RowIndex].Value.ToString();
            var name = (gv.Rows[e.RowIndex].FindControl("txt_ItemName") as TextBox).Text;
            var rcpCode = ddl_ItemRecipe.Value != null ? ddl_ItemRecipe.SelectedItem.Value.ToString() : "";


            string sql = string.Format("UPDATE PT.Item SET ItemName=@Name, RcpCode=@RcpCode WHERE ItemCode = '{0}'", code);
            var p = new List<Blue.DAL.DbParameter>();

            p.Add(new Blue.DAL.DbParameter("Name", name));
            p.Add(new Blue.DAL.DbParameter("RcpCode", rcpCode));

            bu.DbExecuteQuery(@sql, p.ToArray(), LoginInfo.ConnStr);

            (sender as GridView).EditIndex = -1;
            BindItemData(txt_SearchItem.Text);
        }

        protected void grd_Item_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            (sender as GridView).EditIndex = -1;
            BindItemData(txt_SearchItem.Text);
        }

        protected void grd_Item_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string code = (sender as GridView).DataKeys[e.RowIndex].Value.ToString();

            var sql = string.Format("SELECT TOP(1) * FROM PT.Sale WHERE ItemCode = '{0}'", code);

            var dtExist = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            if (dtExist != null && dtExist.Rows.Count > 0)
            {
                ShowAlert(string.Format("No deleted. This code '{0}' was exist in sale.", code));

                return;
            }

            ShowConfirmDelete("Item", code);
        }

        // Private method(s)

        private void ShowItem()
        {
            //GetNewItem();
            BindItemData();


            pop_Item.ShowOnPageLoad = true;

        }

        private void BindItemData(string textSearch = null)
        {
            string sql = string.Empty;

            sql = "SELECT i.ItemCode, i.ItemName, i.RcpCode, r.RcpDesc1, r.RcpDesc2";
            sql += " FROM PT.Item i";
            sql += " LEFT JOIN PT.Rcp r ON r.RcpCode = i.RcpCode";
            if (!string.IsNullOrEmpty(textSearch))
            {
                sql += " WHERE i.ItemCode LIKE @text OR i.ItemName LIKE @text OR i.RcpCode LIKE @text OR r.RcpDesc1 LIKE @text OR r.RcpDesc2 LIKE @text";
            }
            sql += " ORDER BY i.ItemCode";

            if (string.IsNullOrEmpty(textSearch))
                dtItem = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
            else
            {
                var p = new List<Blue.DAL.DbParameter>();
                p.Add(new Blue.DAL.DbParameter("text", "%" + textSearch + "%"));

                dtItem = bu.DbExecuteQuery(sql, p.ToArray(), LoginInfo.ConnStr);
            }


            grd_Item.DataSource = dtItem;
            grd_Item.DataBind();
        }

        #endregion

        #region --Confirm Delete Outlet, Department, Item --

        protected void btn_ConfirmDelete_Yes_Click(object sender, EventArgs e)
        {
            var mode = hf_DeleteMode.Value;
            var code = hf_DeleteCode.Value;

            switch (mode.ToLower())
            {
                case "outlet":
                    DeleteOutlet(code);
                    pop_ConfirmDelete.ShowOnPageLoad = false;
                    break;
                case "department":
                    DeleteDepartment(code);
                    pop_ConfirmDelete.ShowOnPageLoad = false;
                    break;
                case "item":
                    DeleteItem(code);
                    pop_ConfirmDelete.ShowOnPageLoad = false;
                    break;
            }

        }

        private void ShowConfirmDelete(string mode, string code)
        {
            hf_DeleteMode.Value = mode.ToLower();
            hf_DeleteCode.Value = code;
            lbl_ConfirmDelete.Text = string.Format("Do you want to delete {0} '{1}'?", mode, code);
            pop_ConfirmDelete.ShowOnPageLoad = true;

        }

        private void DeleteOutlet(string code)
        {
            string sql = string.Format("DELETE FROM PT.Outlet WHERE OutletCode = N'{0}'", code);

            bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
            BindOutletData();
        }

        private void DeleteDepartment(string code)
        {
            string sql = string.Format("DELETE FROM PT.Department WHERE DepartmentCode = N'{0}'", code);

            bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
            BindDepartmentData();
        }

        private void DeleteItem(string code)
        {
            var sql = string.Format("DELETE FROM PT.Item WHERE ItemCode = '{0}'", code);

            bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
            BindItemData(txt_SearchItem.Text);
        }

        #endregion

        #region --Sale--
        // Sale

        protected void cal_Sale_SelectionChanged(object sender, EventArgs e)
        {
            if (cal_Sale.Value != null)
            {
                Response.Redirect("SaleList.aspx?date=" + cal_Sale.SelectedDate.ToString("yyyy-MM-dd"));
            }

        }

        protected void gv_Sale_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var gv = sender as GridView;

            gv.PageIndex = e.NewPageIndex;

            BindSaleData(cal_Sale.SelectedDate);

        }

        // Private method(s)

        private string GetJsonData(DateTime date)
        {
            var sql = string.Format("SELECT * FROM [INTF].[Data] WHERE Provider='POS' AND [Type]='Sale' AND DocDate = '{0}'", date.ToString("yyyy-MM-dd"));
            var dt = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            var json = dt != null && dt.Rows.Count > 0 ? dt.Rows[0]["Data"].ToString() : null;

            return json;
        }

        private PosData ConvertToPosData(string json, string docDate)
        {
            JObject data = JObject.Parse(@json);


            var outlets = new List<OutletItem>();

            if (data["Outlet"] != null)
            {
                outlets = JsonConvert.DeserializeObject<List<OutletItem>>(data["Outlet"].ToString());
            }

            var items = new List<ItemItem>();

            if (data["PLU"] != null) // Old version
            {
                items = JsonConvert.DeserializeObject<List<ItemItem>>(data["PLU"].ToString());
            }
            else if (data["Item"] != null) // new version
            {
                items = JsonConvert.DeserializeObject<List<ItemItem>>(data["Item"].ToString());
            }


            // Sale
            var sales = new List<SaleItem>();
            var date = docDate;

            if (data["Sale"] != null) // new version
            {
                var jsonSale = data["Sale"];

                if (jsonSale != null)
                {
                    sales = JsonConvert.DeserializeObject<List<SaleItem>>(jsonSale.ToString());
                    sales.ForEach(x => x.PLU = x.ItemCode);
                }
            }
            else
            {
                var jsonSale = data["POS"]["Sales"];


                if (jsonSale != null)
                {
                    sales = JsonConvert.DeserializeObject<List<SaleItem>>(jsonSale.ToString());
                    sales.ForEach(x => x.ItemCode = x.PLU);
                }



                date = data["POS"]["Date"] == null ? "" : data["POS"]["Date"].ToString();
            }



            var result = new PosData();

            result.Date = date;
            result.Outlets = outlets;
            result.Items = items;
            result.Sales = sales;

            return result;
        }

        private void SetSaleData(PosData data)
        {
            lbl_Intf_Date.Text = Convert.ToDateTime(data.Date).ToString("dd/MM/yyyy");

            //var sales = data.Outlets
            //    .Join(data.Sales,
            //    o => o.Code,
            //    s => s.Outlet,
            //    (o, s) => new
            //    {
            //        OutletCode = s.Outlet,
            //        OutletName = o.Desc,
            //        ItemCode = string.IsNullOrEmpty(s.ItemCode)? s.PLU : s.ItemCode,
            //        Qty = s.Qty,
            //        Price = s.UnitPrice,
            //        Total = s.Total
            //    })
            //    .Join(data.Items,
            //    s => s.ItemCode,
            //    i => i.Code,
            //    (s, i) => new
            //    {
            //        OutletCode = s.OutletCode,
            //        OutletName = s.OutletName,
            //        ItemCode = s.ItemCode,
            //        ItemName1 = i.Desc1,
            //        ItemName2 = i.Desc2,
            //        Qty = s.Qty,
            //        Price = s.Price,
            //        Total = s.Total
            //    })
            //    .ToList();
            //var so = from s in data.Sales
            //         join o in data.Outlets on s.Outlet equals o.Code into gj
            //         from g in gj.DefaultIfEmpty()
            //         select new
            //         {
            //             OutletCode = s.Outlet,
            //             OutletName = g.Desc ?? "",
            //             ItemCode = s.ItemCode,
            //             Qty = s.Qty,
            //             Price = s.UnitPrice,
            //             Total = s.Total
            //         };

            //var sales = from s in so
            //            join i in data.Items on s.ItemCode equals i.Code into gj
            //            from g in gj.DefaultIfEmpty()
            //            select new
            //            {
            //                OutletCode = s.OutletCode,
            //                OutletName = s.OutletName,
            //                ItemCode = s.ItemCode,
            //                ItemName1 = g.Desc1 ?? "",
            //                ItemName2 = g.Desc2 ?? "",
            //                Qty = s.Qty,
            //                Price = s.Price,
            //                Total = s.Total
            //            };

            var sales = data.Sales
                .Select(x => new
                {
                    OutletCode = x.Outlet,
                    OutletName = data.Outlets.FirstOrDefault(o => o.Code == x.Outlet).Desc,
                    ItemCode = x.ItemCode,
                    ItemName1 = data.Items,
                    ItemName2 = "",
                    Qty = x.Qty,
                    Price = x.UnitPrice,
                    Total = x.Total
                });

            gv_Intf_Sale.DataSource = sales.ToList();
            gv_Intf_Sale.DataBind();

            var total = sales.Select(x => x.Total).Sum();
            lbl_Intf_GrandTotal.Text = string.Format("{0:N2}", total);
        }

        private void BindSaleData(DateTime date)
        {
            var dt = GetSaleData(date);

            if (dt != null)
            {

                gv_Sale.DataSource = dt;
                gv_Sale.DataBind();


                var total = 0m;

                if (dt.Rows.Count > 0)
                {
                    total = dt.AsEnumerable()
                        .Select(x => x.Field<decimal>("Total"))
                        .Sum();
                }

                lbl_SaleTotal.Text = string.Format("{0:N2}", total);
                lbl_SaleItems.Text = string.Format("{0:N0}", dt.Rows.Count);

                btn_Consumption.Visible = gv_Sale.Rows.Count > 0;
            }
        }

        private DataTable GetSaleData(DateTime date)
        {
            var sql =string.Format(@"DECLARE @SaleDate DATE = '{0}'
 
SELECT 
	CONCAT(s.OutletCode,' : ', o.OutletName) as Outlet, 
	CASE 
		WHEN ISNULL(s.LocationCode,'')='' THEN CONCAT(o.LocationCode, CASE WHEN ISNULL(o.LocationCode,'')='' THEN '' ELSE ' : ' END,l.LocationName) 
		ELSE s.LocationCode
	END as Location,
	CONCAT(s.ItemCode,' : ', i.ItemName) as Item, 
	CASE 
		WHEN ISNULL(s.RcpCode,'')='' THEN CONCAT(i.RcpCode,CASE WHEN ISNULL(i.RcpCode,'')='' THEN '' ELSE ' : ' END, r.RcpDesc1)
		ELSE  s.RcpCode
	END as Recipe, 
	s.Qty, 
	s.UnitPrice, 
	s.Total
 FROM 
	PT.Sale s
	LEFT JOIN PT.Outlet o 
		ON o.OutletCode=s.OutletCode
	LEFT JOIN [IN].StoreLocation l
		ON l.LocationCode=o.LocationCode
	LEFT JOIN PT.Item i 
		ON i.ItemCode=s.ItemCode
	LEFT JOIN PT.Rcp r
		ON r.RcpCode=i.RcpCode
 WHERE 
	SaleDate=@SaleDate
 ORDER BY 
	s.ID", date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

            return bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
        }


        #endregion

        #region -- Post to stock issue
        protected void btn_Consumption_Click(object sender, EventArgs e)
        {
            var date = cal_Sale.SelectedDate;

            var outlets = GetOutletNoMap(date);

            if (outlets.Count() > 0)
            {
                var list = outlets
                    .Select(x => x.Key + " : " + x.Value)
                    .ToArray();

                lbl_Alert.Text = string.Format("Found outlets are not mapped to locations.<br /> {0}", string.Join("<br/>", list));
                //lbl_Alert.Text = string.Format("Found outlets are not mapped to locations.<br /> {0}", string.Join(",", outlets.Select(x => x.Key)));
                pop_Alert.ShowOnPageLoad = true;

                return;
            }

            var items = GetItemNoMap(date);

            if (items.Count() > 0)
            {
                lbl_Alert.Text = string.Format("Found items are not mapped to locations.<br /> {0}", string.Join(",", items.Select(x => x.Key)));
                pop_Alert.ShowOnPageLoad = true;

                return;
            }




            var dt = GetProductItems(date);

            var locations = dt.AsEnumerable()
                .Select(x => new
                {
                    Code = x.Field<string>("LocationCode"),
                    Name = x.Field<string>("LocationCode") + " : " + x.Field<string>("LocationName"),
                })
                .Distinct()
                .ToList();
            if (locations != null)
            {
                listbox_Location.DataTextField = "Name";
                listbox_Location.DataValueField = "Code";
                listbox_Location.DataSource = locations;
                listbox_Location.DataBind();
            }


            pop_Consumptioon.ShowOnPageLoad = true;

            //var date = cal_Sale.SelectedDate;
            //var url = string.Format("Consumption.aspx?date={0}", date.ToString("yyyy-MM-dd"));

            //Response.Redirect(url);
        }

        protected void listbox_Location_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listbox = sender as ListBox;
            var locationCode = listbox.SelectedValue.ToString();


            var dt = GetProductItems(cal_Sale.SelectedDate).AsEnumerable()
                .Where(x => x.Field<string>("LocationCode") == locationCode)
                .Select(x => new
                {
                    ProductCode = x.Field<string>("ProductCode"),
                    ProductDesc1 = x.Field<string>("ProductDesc1"),
                    ProductDesc2 = x.Field<string>("ProductDesc2"),
                    Unit = x.Field<string>("Unit"),
                    Qty = x.Field<decimal>("Qty")
                })
                .ToList();

            gv_Items.DataSource = dt;
            gv_Items.DataBind();


        }
        protected void btn_PostToIssue_Click(object sender, EventArgs e)
        {
        }


        // Private
        //private List<RecipeItem> GetProductItems(IEnumerable<SaleItem> saleItems)

        #endregion

        #region --Interface/Import--

        // Post from POS
        protected void btn_PostFromPOS_Click(object sender, EventArgs e)
        {
            var date = cal_Sale.Value == null ? DateTime.Today : cal_Sale.SelectedDate;

            var data = PostInterfaceData(date);

            if (data.Count() > 0)
            {
                var dt = GetSaleData(date);

                if (dt != null)
                {
                    gv_Sale.DataSource = dt;
                    gv_Sale.DataBind();


                    var total = 0m;

                    if (dt.Rows.Count > 0)
                    {
                        total = dt.AsEnumerable()
                            .Select(x => x.Field<decimal>("Total"))
                            .Sum();
                    }

                    lbl_SaleTotal.Text = string.Format("{0:N2}", total);
                    lbl_SaleItems.Text = string.Format("{0:N0}", dt.Rows.Count);

                    btn_Consumption.Visible = gv_Sale.Rows.Count > 0;
                }
            }
            else
            {
                lbl_Alert.Text = "No data";
                pop_Alert.ShowOnPageLoad = true;
            }
        }

        protected void btn_Interface_Post_Click(object sender, EventArgs e)
        {
            lbl_Intf_Confirm_PostDate.Text = string.Format("Confirm to post data on {0}?", Convert.ToDateTime(_PosData.Date).ToString("dd/MM/yyyy"));
            pop_ConfirmPostInterface.ShowOnPageLoad = true;

        }

        protected void btn_Interface_ConfirmPost_Click(object sender, EventArgs e)
        {
            var data = _PosData;
            var tbName = '#' + Guid.NewGuid().ToString("N");
            var sql = new StringBuilder();


            // Insert outlet
            if (data.Outlets.Count > 0)
            {
                sql.AppendLine(string.Format("CREATE TABLE {0} ( [Code] nvarchar(20) NOT NULL PRIMARY KEY, [Name] nvarchar(255) )", tbName));
                sql.AppendLine(string.Format("INSERT INTO {0} ([Code],[Name]) VALUES", tbName));

                var text = "";

                foreach (var item in data.Outlets)
                {
                    var code = item.Code;
                    var name = item.Desc.Trim().Replace("'", "''");

                    text += string.Format("('{0}','{1}'),", code, name);
                }

                sql.AppendLine(text.TrimEnd(','));

                sql.AppendLine("INSERT INTO PT.Outlet(OutletCode, OutletName)");
                sql.AppendLine("SELECT t.Code, t.Name");
                sql.AppendLine(string.Format("FROM {0} t", tbName));
                sql.AppendLine("LEFT JOIN  PT.Outlet o ON t.Code COLLATE DATABASE_DEFAULT =o.OutletCode COLLATE DATABASE_DEFAULT");
                sql.AppendLine("WHERE o.OutletCode IS NULL");

                bu.DbExecuteQuery(sql.ToString(), null, LoginInfo.ConnStr);
            }

            sql.Clear();

            // Insert PLU (Item)
            if (data.Items.Count > 0)
            {
                sql.AppendLine(string.Format("CREATE TABLE {0} ( [Code] nvarchar(20) NOT NULL PRIMARY KEY, [Name] nvarchar(255) )", tbName));

                foreach (var item in data.Items)
                {
                    var code = item.Code;
                    var name = item.Desc1.Trim().Replace("'", "''");

                    sql.AppendLine(string.Format("INSERT INTO {0} ([Code],[Name]) VALUES ('{1}','{2}')", tbName, code, name));
                }

                sql.AppendLine("INSERT INTO PT.Item (ItemCode, ItemName)");
                sql.AppendLine("SELECT t.Code, t.Name");
                sql.AppendLine(string.Format("FROM {0} t", tbName));
                sql.AppendLine("LEFT JOIN  PT.Item o ON t.Code COLLATE DATABASE_DEFAULT =o.ItemCode COLLATE DATABASE_DEFAULT");
                sql.AppendLine("WHERE o.ItemCode IS NULL");

                bu.DbExecuteQuery(sql.ToString(), null, LoginInfo.ConnStr);

            }

            // Sale saleItems
            sql.Clear();
            var date = data.Date;

            sql.AppendLine(string.Format("DELETE FROM PT.Sale WHERE SaleDate = '{0}'", date));
            foreach (var item in data.Sales)
            {
                var outlet = item.Outlet;
                var itemCode = item.PLU;
                var qty = item.Qty;
                var price = item.UnitPrice;
                var total = item.Total;

                sql.AppendLine(string.Format("INSERT INTO PT.Sale (SaleDate, OutletCode, ItemCode, Qty, UnitPrice, Total) VALUES ('{0}','{1}','{2}',{3},{4},{5})",
                    date,
                    outlet,
                    itemCode,
                    qty,
                    price,
                    total
                    ));
            }

            bu.DbExecuteQuery(sql.ToString(), null, LoginInfo.ConnStr);


            BindSaleData(Convert.ToDateTime(date));


            pop_Interface.ShowOnPageLoad = false;
            pop_ConfirmPostInterface.ShowOnPageLoad = false;


            lbl_Alert.Text = "Done";
            pop_Alert.ShowOnPageLoad = true;

        }

        // Import

        protected void btn_Import_Click(object sender, EventArgs e)
        {
            lblErrorMessage.InnerText = string.Empty;

            string result = ImportData();

            if (result == string.Empty)
            {
                dtImport = null;
                BindImportData();
                lbl_FileName.InnerText = "Import done";
            }
            else
                lblErrorMessage.InnerText = result;


        }

        protected void grd_Import_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd_Import.PageIndex = e.NewPageIndex;

            grd_Import.DataSource = dtImport;
            grd_Import.DataBind();

        }

        protected void grd_Import_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //e.Row.Cells[0].ForeColor = Color.Blue;

                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (i < ImportFields.Length)
                        e.Row.Cells[i].ForeColor = Color.Blue;
                    else
                        e.Row.Cells[i].ForeColor = Color.Silver;
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
            }
        }

        protected void btn_Upload_Click(object sender, EventArgs e)
        {
            dtImport = null;
            BindImportData();

            if (FileUploadControl.HasFile)
            {
                string fileName = Path.GetFileName(FileUploadControl.PostedFile.FileName);
                string fileExt = Path.GetExtension(FileUploadControl.PostedFile.FileName);

                if (fileExt == ".xls" || fileExt == ".xlsx" || fileExt == ".csv")
                {
                    string folderPath = "~/Tmp/";
                    string newName = Guid.NewGuid().ToString();
                    string filePath = Server.MapPath(folderPath + newName + fileExt);

                    FileUploadControl.SaveAs(filePath);
                    lbl_FileName.InnerText = fileName;

                    FillGridFromFile(filePath, fileExt, "NO");

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                }
                else
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.InnerText = "Please upload valid file.";
                }

            }
            else
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.InnerText = "No file found.";
            }
        }

        private DataTable CreateImportDataTable()
        {
            var dt = new DataTable();

            dt.Columns.Add("SaleDate", typeof(DateTime));
            dt.Columns.Add("OutletCode", typeof(string));
            dt.Columns.Add("DepartmentCode", typeof(string));
            dt.Columns.Add("ItemCode", typeof(string));
            dt.Columns.Add("Qty", typeof(decimal));
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("Total", typeof(decimal));

            return dt;
        }

        private DataTable CsvToDatatable(string filename)
        {
            var dt = CreateImportDataTable();

            string[] lines = File.ReadAllLines(filename);


            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Split(',');
                DataRow dr = dt.NewRow();

                for (int column = 0; column < dt.Columns.Count; column++)
                {

                    dr[column] = line[column];

                }
                dt.Rows.Add(dr);
            }

            return dt;
        }


        protected void btn_PreviewFile_Click(object sender, EventArgs e)
        {
            if (FileUpload.HasFile)
            {
                var uploadFile = FileUpload.PostedFile.FileName;
                var fileName = Path.GetFileName(uploadFile);
                var fileExt = Path.GetExtension(uploadFile).ToLower();


                switch (fileExt)
                {
                    case ".xls":
                    case ".xlsx":
                    case ".csv":

                        break;
                    default:
                        ShowAlert(string.Format("{0} is not supported.", uploadFile));
                        return;
                }

                var tempPath = Path.GetTempPath();
                var tempFilename = string.Format("{0}{1}{2}", tempPath, Guid.NewGuid(), fileExt);

                FileUpload.SaveAs(tempFilename);

                var dt = new DataTable();

                if (fileExt == ".csv")
                {
                    dt = CsvToDatatable(tempFilename);
                }

                gv_PreviewFile.DataSource = dt;
                gv_PreviewFile.DataBind();



                //if (fileExt == ".xls" || fileExt == ".xlsx" || fileExt == ".csv")
                //{

                //string folderPath = "~/Tmp/";
                //string newName = Guid.NewGuid().ToString();
                //string filePath = Server.MapPath(folderPath + newName + fileExt);


                //FileUploadControl.SaveAs(filePath);
                //lbl_FileName.InnerText = fileName;


                //FillGridFromFile(filePath, fileExt, "NO");

                //if (File.Exists(filePath))
                //{
                //    File.Delete(filePath);
                //}

                //}
                //else
                //{
                //    lblErrorMessage.Visible = true;
                //    lblErrorMessage.InnerText = "Please upload valid file.";
                //}

            }
            else
            {
                ShowAlert("Please select a file.");
            }
        }

        // Import from API
        protected void ddl_Import_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as DropDownList;

            panel_File.Visible = ddl.SelectedIndex == 0;
            panel_API.Visible = ddl.SelectedIndex == 1;

        }

        // Private method(s)
        private void BindImportData()
        {
            ///Bind Sheet Data to GridView
            grd_Import.DataSource = dtImport;
            grd_Import.DataBind();

            bool haveData = dtImport.Rows.Count > 0;
            btn_Import.Visible = haveData;

            lblErrorMessage.InnerText = string.Empty;
            lbl_FileName.InnerText = string.Empty;



        }

        public DataTable ConvertCSVToDataTable(string filePath)
        {
            char delimiter = ',';
            DataTable tbl = new DataTable();
            int numberOfColumns;
            string[] lines = System.IO.File.ReadAllLines(filePath);

            if (lines.ToString().Trim() != string.Empty)
            {
                numberOfColumns = lines[0].Split(delimiter).Length;


                for (int col = 0; col < numberOfColumns; col++)
                    tbl.Columns.Add(new DataColumn("F" + (col + 1).ToString()));



                foreach (string line in lines)
                {
                    var cols = line.Split(delimiter);

                    DataRow dr = tbl.NewRow();
                    for (int cIndex = 0; cIndex < numberOfColumns; cIndex++)
                    {
                        dr[cIndex] = cols[cIndex];
                    }

                    tbl.Rows.Add(dr);
                }
            }

            return tbl;
        }


        private void FillGridFromFile(string filePath, string ext, string isHader)
        {
            if (ext == ".xls" || ext == ".xlsx")
            {
                string providerConfig = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
                string connectionString = String.Format(providerConfig, filePath, isHader);
                OleDbConnection conn = new OleDbConnection(connectionString);
                OleDbCommand cmd = new OleDbCommand();
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
                cmd.Connection = conn;
                //Fetch 1st Sheet Name
                conn.Open();
                DataTable dtSchema;
                dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string ExcelSheetName = dtSchema.Rows[0]["TABLE_NAME"].ToString();
                conn.Close();

                //Read all data of fetched Sheet to a Data Table
                conn.Open();
                cmd.CommandText = "SELECT * From [" + ExcelSheetName + "]";
                dataAdapter.SelectCommand = cmd;
                dataAdapter.Fill(dtImport);
                conn.Close();
            }
            else  // ext==".csv"
            {
                dtImport = ConvertCSVToDataTable(filePath);
            }


            for (int i = 0; i < dtImport.Columns.Count; i++)
            {
                if (i < ImportFields.Length)
                    dtImport.Columns[i].ColumnName = ImportFields[i].ToString();
            }


            BindImportData();

        }

        private string ImportData()
        {
            string result = string.Empty;

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlTransaction trans;

            conn.ConnectionString = LoginInfo.ConnStr;
            conn.Open();

            trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);  //*** Start Transaction ***//

            cmd.Connection = conn;
            cmd.Transaction = trans;  //*** Command & Transaction ***//

            try
            {
                string sql;
                string saleDate, outlet, departmentCode, itemCode; //, itemName;
                decimal qty, price, total;
                string value = string.Empty;
                foreach (DataRow dr in dtImport.Rows)
                {
                    saleDate = Convert.ToDateTime(dr["SaleDate"]).ToString("yyyy-MM-dd");
                    outlet = dr["Outlet"].ToString();
                    departmentCode = dr["DepartmentCode"].ToString();
                    itemCode = dr["ItemCode"].ToString();

                    value = dr["Qty"].ToString();
                    if (value == "NULL" || value == string.Empty)
                        qty = 0m;
                    else
                        qty = Convert.ToDecimal(dr["Qty"]);

                    value = dr["Price"].ToString();
                    if (value == "NULL" || value == string.Empty)
                        price = 0m;
                    else
                        price = Convert.ToDecimal(dr["Price"]);

                    value = dr["Total"].ToString();
                    if (value == "NULL" || value == string.Empty)
                        total = 0m;
                    else
                        total = Convert.ToDecimal(dr["Total"]);

                    sql = "INSERT INTO PT.Sale (SaleDate, OutletCode, DepartmentCode, ItemCode,  Qty, Price, Total, IsVoid)";
                    sql += " VALUES (@SaleDate, @Outlet, @DepartmentCode, @ItemCode, @Qty, @Price, @Total, 0)";

                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@SaleDate", saleDate);
                    cmd.Parameters.AddWithValue("@Outlet", outlet);
                    cmd.Parameters.AddWithValue("@DepartmentCode", departmentCode);
                    cmd.Parameters.AddWithValue("@ItemCode", itemCode);
                    cmd.Parameters.AddWithValue("@Qty", qty);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Total", total);

                    cmd.ExecuteNonQuery();
                }

                trans.Commit();  //*** Commit Transaction ***//


            }
            catch (Exception ex)
            {
                trans.Rollback(); //*** RollBack Transaction ***//
                result = ex.ToString();
            }


            conn.Close();
            conn = null;

            return result;

        }


        #endregion

        #region --Stock Out--

        protected void btn_SoDate_Click(object sender, EventArgs e)
        {
        }

        protected void grd_StockOut_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            (sender as GridView).PageIndex = e.NewPageIndex;
        }

        protected void grd_StockOut_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ASPxComboBox ddl_Outlet = e.Row.FindControl("ddl_Outlet") as ASPxComboBox;
                if (ddl_Outlet != null)
                {
                    ddl_Outlet.DataSource = GetOutletList();
                    ddl_Outlet.DataBind();
                    ddl_Outlet.Value = DataBinder.Eval(e.Row.DataItem, "OutletCode");
                }

                ASPxComboBox ddl_Department = e.Row.FindControl("ddl_Department") as ASPxComboBox;
                if (ddl_Department != null)
                {
                    ddl_Department.DataSource = GetDepartmentList();
                    ddl_Department.DataBind();
                    ddl_Department.Value = DataBinder.Eval(e.Row.DataItem, "DepartmentCode");
                }

                ASPxComboBox ddl_Item = e.Row.FindControl("ddl_Item") as ASPxComboBox;
                if (ddl_Item != null)
                {
                    ddl_Item.DataSource = GetItemList();
                    ddl_Item.DataBind();
                    ddl_Item.Value = DataBinder.Eval(e.Row.DataItem, "ItemCode");
                }

                CheckBox chk_IsPosted = e.Row.FindControl("chk_IsPosted") as CheckBox;
                if (chk_IsPosted != null)
                {
                    var value = DataBinder.Eval(e.Row.DataItem, "IsPosted");
                    if (value == DBNull.Value)
                        value = false;

                    chk_IsPosted.Checked = Convert.ToBoolean(value);
                    chk_IsPosted.ToolTip = DataBinder.Eval(e.Row.DataItem, "RefNo").ToString();
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                object sumTotal;
                sumTotal = dtSale.Compute("SUM(Total)", string.Empty);
                var lbl_SumTotal = e.Row.FindControl("lbl_SumTotal") as Label;
                lbl_SumTotal.Text = string.Format(DefaultAmtFmt, Convert.ToDecimal(sumTotal.ToString()));

            }
        }

        // Private method(s)

        protected DataTable GetOutletList()
        {
            string sql = "SELECT OutletCode, OutletCode + ' : ' + ISNULL(OutletName,'') as Outlet FROM [PT].Outlet ORDER BY OutletCode";
            return bu.DbExecuteQuery(@sql, null, LoginInfo.ConnStr);
        }

        protected DataTable GetDepartmentList()
        {
            string sql = "SELECT DepartmentCode, DepartmentCode + ' : ' + ISNULL(DepartmentName,'') as Department FROM [PT].Department ORDER BY DepartmentCode";
            return bu.DbExecuteQuery(@sql, null, LoginInfo.ConnStr);
        }

        protected DataTable GetItemList()
        {
            string sql = "SELECT ItemCode, ItemCode + ' : ' + ISNULL(ItemName,'') as Item FROM [PT].Item ORDER BY ItemCode";
            return bu.DbExecuteQuery(@sql, null, LoginInfo.ConnStr);
        }


        #endregion


        // Private Method(s)

        private void ShowAlert(string text, string headerText = null)
        {

            lbl_Alert.Text = text;
            pop_Alert.HeaderText = string.IsNullOrEmpty(headerText) ? "Alert" : headerText;
            pop_Alert.ShowOnPageLoad = true;
        }

        protected DataTable GetLocation()
        {
            string sql = "SELECT LocationCode, LocationName, LocationCode + ' : ' + LocationName as Location FROM [IN].StoreLocation WHERE EOP=1 ORDER BY LocationCode";
            return bu.DbExecuteQuery(@sql, null, LoginInfo.ConnStr);
        }


        private IEnumerable<PT_Sale> PostInterfaceData(DateTime date)
        {
            var result = new List<PT_Sale>();

            var sql = string.Format("SELECT * FROM [INTF].[Data] WHERE Provider='POS' AND [Type]='Sale' AND DocDate = '{0}'", date.ToString("yyyy-MM-dd"));
            var dt = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            var json = dt != null && dt.Rows.Count > 0 ? dt.Rows[0]["Data"].ToString() : null;

            if (!string.IsNullOrEmpty(json))
            {
                var data = JObject.Parse(json);

                // Get outlet
                if (data["Outlet"] != null)
                {
                    var outlets = JsonConvert.DeserializeObject<List<OutletItem>>(data["Outlet"].ToString());

                    UpdateOutlet(outlets);
                }


                if (data["PLU"] != null) // Old version
                {
                    var items = JsonConvert.DeserializeObject<List<ItemItem>>(data["PLU"].ToString());

                    UpdateItem(items);

                }
                else if (data["Item"] != null) // new version
                {
                    var items = JsonConvert.DeserializeObject<List<ItemItem>>(data["Item"].ToString());

                    UpdateItem(items);
                }



                // Sale
                var sales = new List<SaleItem>();

                if (data["Sale"] != null) // new version
                {
                    var jsonSale = data["Sale"];

                    if (jsonSale != null)
                    {
                        sales = JsonConvert.DeserializeObject<List<SaleItem>>(jsonSale.ToString());
                        sales.ForEach(x => x.PLU = x.ItemCode);
                    }
                }
                else if (data["POS"]["Sales"] != null)
                {
                    var jsonSale = data["POS"]["Sales"];

                    if (jsonSale != null)
                    {
                        sales = JsonConvert.DeserializeObject<List<SaleItem>>(jsonSale.ToString());
                        sales.ForEach(x => x.ItemCode = x.PLU);
                    }

                    var docDate = data["POS"]["Date"] == null ? "" : data["POS"]["Date"].ToString();

                    date = Convert.ToDateTime(docDate);
                }

                UpdateSale(date, sales);

                sql =
                    string.Format(@"SELECT
	                    s.*,
	                    o.OutletName,
	                    i.ItemName
                    FROM
	                    PT.Sale s
	                    LEFT JOIN PT.Outlet o
		                    ON o.OutletCode=s.OutletCode
	                    LEFT JOIN PT.Item i
		                    ON i.ItemCode = s.ItemCode
                    WHERE
	                    s.SaleDate = '{0}'", date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

                var dtSale = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

                result = dtSale.AsEnumerable()
                    .Select(x => new PT_Sale
                    {
                        Id = x.Field<int>("ID"),
                        SaleDate = date,
                        OutletCode = x.Field<string>("OutletCode"),
                        OutletName = x.Field<string>("OutletName"),
                        ItemCode = x.Field<string>("ItemCode"),
                        ItemName = x.Field<string>("Itemname"),
                        Qty = x.Field<decimal>("Qty"),
                        UnitPrice = x.Field<decimal>("UnitPrice"),
                        Total = x.Field<decimal>("Total")
                    }).ToList();
            }

            return result;
        }

        private void UpdateOutlet(IEnumerable<OutletItem> outlets)
        {
            if (outlets.Count() > 0)
            {

                var dt = bu.DbExecuteQuery("SELECT OutletCode as Code FROM PT.Outlet", null, LoginInfo.ConnStr);
                var codes = dt != null && dt.Rows.Count > 0 ? dt.AsEnumerable().Select(x => x.Field<string>("Code")).ToList() : new List<string>();
                var new_items = outlets.Select(x => x.Code).Except(codes);

                var sql = new StringBuilder();

                sql.AppendLine("INSERT INTO PT.Outlet (OutletCode, OutletName)");
                sql.AppendLine("VALUES");

                foreach (var item in outlets.Where(x => new_items.Contains(x.Code)))
                {
                    sql.Append(string.Format("('{0}','{1}'),", item.Code, item.Desc.Replace("'", "''")));
                }

                bu.DbExecuteQuery(sql.ToString().Trim().TrimEnd(','), null, LoginInfo.ConnStr);
            }
        }

        private void UpdateItem(IEnumerable<ItemItem> items)
        {
            if (items.Count() > 0)
            {
                var dt = bu.DbExecuteQuery("SELECT ItemCode as Code FROM PT.item", null, LoginInfo.ConnStr);
                var codes = dt != null && dt.Rows.Count > 0 ? dt.AsEnumerable().Select(x => x.Field<string>("Code")).ToList() : new List<string>();
                var new_items = items.Select(x => x.Code).Except(codes);

                var sql = new StringBuilder();

                sql.AppendLine("INSERT INTO PT.Item (ItemCode, ItemName)");
                sql.AppendLine("VALUES");
                foreach (var item in items.Where(x => new_items.Contains(x.Code)))
                {
                    sql.Append(string.Format("('{0}','{1}'),", item.Code, item.Desc1.Replace("'", "''")));
                }

                bu.DbExecuteQuery(sql.ToString().Trim().TrimEnd(','), null, LoginInfo.ConnStr);

            }
        }

        private void UpdateSale(DateTime date, IEnumerable<SaleItem> sales)
        {
            var connStr = LoginInfo.ConnStr;

            var saleDate = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            // Remove old items
            bu.DbExecuteQuery(string.Format("DELETE FROM PT.Sale WHERE SaleDate='{0}'", saleDate), null, connStr);


            var sql = new StringBuilder();

            sql.AppendLine("INSERT INTO PT.Sale (SaleDate, OutletCode, itemCode, Qty, UnitPrice, Total, Void)");
            sql.AppendLine("VALUES");
            foreach (var item in sales)
            {
                sql.AppendFormat("('{0}','{1}','{2}',{3},{4},{5},0),",
                    saleDate,
                    item.Outlet,
                    item.ItemCode,
                    item.Qty,
                    item.UnitPrice,
                    item.Total);
            }
            var query = sql.ToString().TrimEnd(',');

            bu.DbExecuteQuery(query, null, LoginInfo.ConnStr);

            // update outlet , itemcode


            var outlets = sales
                .Select(x => x.Outlet)
                .Distinct()
                .Select(x => new OutletItem
                {
                    Code = x,
                    Desc = x
                });

            UpdateOutlet(outlets);

            var items = sales
                .Select(x => x.ItemCode)
                .Distinct()
                .Select(x => new ItemItem
                {
                    Code = x,
                    Desc1 = x
                });

            UpdateItem(items);
        }

        protected string FormatQty(object sender)
        {
            return string.Format("{0:N" + DefaultAmtDigit.ToString() + "}", Convert.ToDecimal(sender));
        }

        // Consumption

        private DataTable GetProductItems(DateTime date)
        {
            var sql = string.Format("EXEC PT.GetConsumptionOfSale '{0}'", date.ToString("yyyy-MM-dd"));

            return bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
        }

        private IEnumerable<KeyValue> GetOutletNoMap(DateTime date)
        {
            var saleDate = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var sql = string.Format(@"
SELECT 
    DISTINCT s.OutletCode, 
    o.OutletName 
FROM 
    PT.Sale s 
    LEFT JOIN PT.Outlet o 
        ON o.OutletCode=s.OutletCode 
WHERE 
    SaleDate='{0}' 
    AND ISNULL(o.LocationCode,'')=''", saleDate);

            var dt = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            return dt.AsEnumerable()
                .Select(x => new KeyValue
                {
                    Key = x.Field<string>("OutletCode"),
                    Value = x.Field<string>("OutletName")
                });
        }

        private IEnumerable<KeyValue> GetItemNoMap(DateTime date)
        {
            var saleDate = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var sql = string.Format("SELECT s.ItemCode, i.ItemName FROM PT.Sale s LEFT JOIN PT.Item i ON i.ItemCode=s.ItemCode WHERE SaleDate='{0}' AND ISNULL(i.RcpCode,'')='' ", saleDate);

            var dt = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            return dt.AsEnumerable()
                .Select(x => new KeyValue
                {
                    Key = x.Field<string>("ItemCode"),
                    Value = x.Field<string>("ItemName")
                });
        }


        // Internal Classes
        public class PT_Sale
        {
            public int Id { get; set; }
            public DateTime SaleDate { get; set; }
            public string RevenueCode { get; set; }
            public string RevenueName { get; set; }
            public string DepartmentCode { get; set; }
            public string DepartmetnName { get; set; }
            public string OutletCode { get; set; }
            public string OutletName { get; set; }
            public string ItemCode { get; set; }
            public string ItemName { get; set; }
            public decimal Qty { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal Total { get; set; }
            public bool isvoid { get; set; }
        }


        internal class PosData
        {
            public PosData()
            {
                Outlets = new List<OutletItem>();
                Items = new List<ItemItem>();
                Sales = new List<SaleItem>();
            }

            public string Date { get; set; }
            public List<OutletItem> Outlets { get; set; }
            public List<ItemItem> Items { get; set; }
            public List<SaleItem> Sales { get; set; }
        }

        [Serializable]
        internal class OutletItem
        {
            public string Code { get; set; }
            public string Desc { get; set; }
        }

        [Serializable]
        internal class ItemItem
        {
            public string Code { get; set; }
            public string Desc1 { get; set; }
            public string Desc2 { get; set; }
        }

        [Serializable]
        internal class SaleItem
        {
            public string Outlet { get; set; }
            public string PLU { get; set; }
            public string ItemCode { get; set; }
            public decimal Qty { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal Total { get; set; }
        }

        [Serializable]
        internal class KeyValue
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        public class RowImport
        {
            public DateTime SaleDate { get; set; }
            public string OutletCode { get; set; }
            public string DepartmentCode { get; set; }
            public string ItemCode { get; set; }
            public decimal Qty { get; set; }
            public decimal Price { get; set; }
            public decimal Total { get; set; }
        }
    }

}
