using BlueLedger.PL.BaseClass;
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using DevExpress.Web.ASPxEditors;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;

namespace BlueLedger.PL.PT.Sale
{
    public partial class SaleList : BasePage
    {
        #region Properties
        private readonly string moduleID = "4.3";
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();

        //private DataSet dsSale
        //{
        //    get
        //    {
        //        if (Session["dsSale"] == null)
        //            Session["dsSale"] = new DataSet();

        //        return (DataSet)Session["dsSale"];
        //    }
        //    set
        //    {
        //        Session["dsSale"] = value;
        //    }
        //}

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

        string[] ImportFields = { "SaleDate", "Outlet", "DepartmentCode", "ItemCode", "Qty", "Price", "Total" };

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
        }

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
            txt_Search.Attributes.Add("placeholder", "Search");


            txt_PageSize.Text = grd_Sale.PageSize.ToString();
            //chk_FilterByDate.Checked = true;
            de_DateFrom.Date = DateTime.Today;
            de_DateTo.Date = DateTime.Today;

            ddl_Outlet_New.DataBind();
            ddl_DepartmentCode_New.DataBind();
            ddl_ItemCode_New.DataBind();

            BindSaleData(grd_Sale, de_DateFrom.Date, de_DateTo.Date, txt_Search.Text);

        }


        // ----------------------------------------------------------------------------------------------------

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "STOCKOUT":
                    pop_StockOut.ShowOnPageLoad = true;
                    break;
                case "IMPORT":
                    dtImport = null;
                    BindImportData();

                    pop_Import.ShowOnPageLoad = true;
                    break;
                //case "OUTLET":
                //    GetNewOutlet();
                //    BindOutletData();
                //    pop_Outlet.ShowOnPageLoad = true;
                //    break;
                //case "DEPARTMENT":
                //    GetNewDepartment();
                //    BindDepartmentData();
                //    pop_Department.ShowOnPageLoad = true;
                //    break;
                //case "ITEM":
                //    GetNewItem();
                //    BindItemData();
                //    pop_Item.ShowOnPageLoad = true;
                //    break;

            }
        }

        protected void chk_FilterByDate_ChckedChanged(object sender, EventArgs e)
        {
            //div_Filter.Visible = chk_FilterByDate.Checked;
            BindSaleData(grd_Sale, de_DateFrom.Date, de_DateTo.Date, txt_Search.Text); 
        }

        protected void btn_FilterByDate_Click(object sender, EventArgs e)
        {
            BindSaleData(grd_Sale, de_DateFrom.Date, de_DateTo.Date, txt_Search.Text); 
        }

        protected void txt_Search_TextChanged(object sender, EventArgs e)
        {
            BindSaleData(grd_Sale, de_DateFrom.Date, de_DateTo.Date, txt_Search.Text); 
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            BindSaleData(grd_Sale, de_DateFrom.Date, de_DateTo.Date, txt_Search.Text); 

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

        protected void btn_Alert_Ok_Click(object sender, EventArgs e)
        {
            pop_Alert.ShowOnPageLoad = false;
        }

        protected void txt_PageSize_TextChanged(object sender, EventArgs e)
        {
            int pageSize = 0;
            if (Int32.TryParse(txt_PageSize.Text, out pageSize))
                BindSaleData(grd_Sale, de_DateFrom.Date, de_DateTo.Date, txt_Search.Text);
            else
                txt_PageSize.Text = grd_Sale.PageSize.ToString();
        }

        protected void pop_Outlet_WindowCallback(object source, DevExpress.Web.ASPxPopupControl.PopupWindowEventArgs e)
        {
            ddl_Outlet_New.DataBind();

        }

        #region Sale

        protected void menu_Sale_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "BACK":
                    pop_StockOut.ShowOnPageLoad = false;
                    break;

            }
        }

        protected void btn_SaleAdd_Click(object sender, EventArgs e)
        {
            // Validate data
            string saleDate = string.Empty;

            if (de_SaleDate_New.Text != string.Empty)
                saleDate = de_SaleDate_New.Date.ToString("yyyy-MM-dd");
            else
            {
                AlertBox("Invalid Sale Date.");
                return;
            }

            string outlet = string.Empty;
            if (ddl_Outlet_New.SelectedValue != null)
                outlet = ddl_Outlet_New.SelectedValue.ToString();
            else
            {
                AlertMessageBox("Invalid Outlet");
                return;
            }

            string departmentCode = string.Empty;
            if (ddl_DepartmentCode_New.SelectedValue != null)
                departmentCode = ddl_DepartmentCode_New.SelectedValue.ToString();
            else
            {
                AlertMessageBox("Invalid Department");
                return;
            }

            string item = string.Empty;
            if (ddl_ItemCode_New.SelectedValue != null)
                item = ddl_ItemCode_New.SelectedValue.ToString();
            else
            {
                AlertMessageBox("Invalid Item");
                return;
            }

            decimal qty = string.IsNullOrEmpty(txt_NewSaleQty.Text) ? 0 : Convert.ToDecimal(txt_NewSaleQty.Text);
            decimal price = string.IsNullOrEmpty(txt_NewSalePrice.Text) ? 0 : Convert.ToDecimal(txt_NewSalePrice.Text);
            decimal total = string.IsNullOrEmpty(txt_NewSaleTotal.Text) ? 0 : Convert.ToDecimal(txt_NewSaleTotal.Text);

            string sql = "INSERT INTO PT.Sale (SaleDate, Outlet, DepartmentCode, ItemCode, Qty, Price, Total, Void) ";
            sql += string.Format("VALUES('{0}', '{1}', '{2}', '{3}', {4}, {5}, {6}, 0)",
                saleDate,
                outlet,
                departmentCode,
                item,
                qty,
                price,
                total);

            bu.DbExecuteQuery(@sql, null, LoginInfo.ConnStr);


            BindSaleData(grd_Sale, de_DateFrom.Date, de_DateTo.Date, txt_Search.Text); 
        }

        //Grid Sale
        protected void grd_Sale_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            (sender as GridView).PageIndex = e.NewPageIndex;
            BindSaleData(grd_Sale, de_DateFrom.Date, de_DateTo.Date, txt_Search.Text);
        }

        protected void grd_Sale_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void grd_Sale_RowEditing(object sender, GridViewEditEventArgs e)
        {
            (sender as GridView).EditIndex = e.NewEditIndex;
            BindSaleData(grd_Sale, de_DateFrom.Date, de_DateTo.Date, txt_Search.Text);

        }

        protected void grd_Sale_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            (sender as GridView).EditIndex = -1;
            BindSaleData(grd_Sale, de_DateFrom.Date, de_DateTo.Date, txt_Search.Text);
        }

        protected void grd_Sale_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //getting key value, row id
            int id = Convert.ToInt32(grd_Sale.DataKeys[e.RowIndex].Value.ToString());
            string sql = "UPDATE PT.Sale SET";
            sql += string.Format(" SaleDate='{0}',", (grd_Sale.Rows[e.RowIndex].FindControl("de_SaleDate") as ASPxDateEdit).Date.ToString("yyyy-MM-dd"));
            sql += string.Format(" Outlet='{0}',", (grd_Sale.Rows[e.RowIndex].FindControl("ddl_Outlet") as ASPxComboBox).Value);
            sql += string.Format(" DepartmentCode='{0}',", (grd_Sale.Rows[e.RowIndex].FindControl("ddl_Department") as ASPxComboBox).Value);
            sql += string.Format(" ItemCode='{0}',", (grd_Sale.Rows[e.RowIndex].FindControl("ddl_Item") as ASPxComboBox).Value);
            //sql += string.Format(" ItemName='{0}',", (grd_Sale.Rows[e.RowIndex].FindControl("txt_ItemName") as TextBox).Text);
            sql += string.Format(" Qty={0},", Convert.ToDecimal((grd_Sale.Rows[e.RowIndex].FindControl("txt_Qty") as ASPxSpinEdit).Text));
            sql += string.Format(" Price={0},", Convert.ToDecimal((grd_Sale.Rows[e.RowIndex].FindControl("txt_UnitPrice") as ASPxSpinEdit).Text));
            sql += string.Format(" Total={0}", Convert.ToDecimal((grd_Sale.Rows[e.RowIndex].FindControl("txt_Total") as ASPxSpinEdit).Text));
            sql += string.Format(" WHERE ID = {0}", id.ToString());
            bu.DbExecuteQuery(@sql, null, LoginInfo.ConnStr);

            (sender as GridView).EditIndex = -1;
            BindSaleData(grd_Sale, de_DateFrom.Date, de_DateTo.Date, txt_Search.Text);
        }

        protected void grd_Sale_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //getting key value, row id
            int id = Convert.ToInt32((sender as GridView).DataKeys[e.RowIndex].Value.ToString());

            string sql = string.Format("DELETE FROM PT.Sale WHERE ID = {0}", id.ToString());
            bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
            BindSaleData(grd_Sale, de_DateFrom.Date, de_DateTo.Date, txt_Search.Text);
        }

        //Grid Import

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

        #endregion

        #region Outlet
        protected void btn_Outlet_Click(object sender, EventArgs e)
        {
            //GetNewOutlet();
            BindOutletData();

            // Bind location
            ddl_NewOutletLocation.DataSource = GetLocation();
            ddl_NewOutletLocation.DataValueField = "LocationCode";
            ddl_NewOutletLocation.DataTextField = "Location";
            ddl_NewOutletLocation.DataBind();

            pop_Outlet.ShowOnPageLoad = true;

        }

        protected void menu_Outlet_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "PRINT":
                    break;
                case "BACK":
                    pop_Outlet.ShowOnPageLoad = false;
                    break;

            }
        }

        protected void btn_OutletAdd_Click(object sender, EventArgs e)
        {
            // Check existing code

            string outletCode = txt_NewOutletCode.Text;

            string sql = string.Format("SELECT COUNT(OutletCode) as RecordCount FROM PT.Outlet WHERE OutletCode='{0}'", outletCode);
            DataTable dt = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                AlertBox(string.Format("'{0}' is duplicated.", outletCode));
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

        // Grid Outlet
        protected void grd_Outlet_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            (sender as GridView).PageIndex = e.NewPageIndex;

            (sender as GridView).DataSource = dtOutlet;
            (sender as GridView).DataBind();

        }

        protected void grd_Outlet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                string outletCode = ((sender as GridView).FooterRow.FindControl("txt_OutletCode_New") as TextBox).Text;
                string locationCode = ((sender as GridView).FooterRow.FindControl("ddl_LocationCode_New") as DropDownList).SelectedValue.ToString();

                DataTable dt = bu.DbExecuteQuery(string.Format("SELECT TOP(1) * FROM PT.Outlet WHERE OutletCode = '{0}'", outletCode), null, LoginInfo.ConnStr);
                if (dt.Rows.Count > 0)
                {
                    AlertBox("Duplicate Outlet Code!");
                }
                else
                {
                    string sql = string.Format("INSERT INTO PT.Outlet (OutletCode, LocationCode) VALUES('{0}','{1}')", outletCode, locationCode);
                    bu.DbExecuteQuery(@sql, null, LoginInfo.ConnStr);

                    (sender as GridView).EditIndex = -1;
                    BindOutletData();
                }
            }
        }

        protected void grd_Outlet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Location
                var obj = e.Row.FindControl("lbl_LocationCode");
                if (obj != null)
                {
                    Label label = obj as Label;

                    if (label.Text == string.Empty)
                    {
                        label.Text = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString();
                        label.ToolTip = label.Text + " : This location is not found!";
                    }
                    else
                        label.ToolTip = label.Text;
                }




                if ((sender as GridView).EditIndex == e.Row.RowIndex)
                {
                    DropDownList ddl_LocationCode = (DropDownList)e.Row.FindControl("ddl_LocationCode");
                    ddl_LocationCode.DataSource = GetLocation();
                    ddl_LocationCode.DataBind();

                    string locationCode = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString();

                    if (ddl_LocationCode.Items.FindByValue(locationCode) == null)
                    {
                        ddl_LocationCode.Items.Insert(0, new ListItem(locationCode, ""));
                        ddl_LocationCode.SelectedIndex = 0;
                    }
                    else
                        ddl_LocationCode.Items.FindByValue(locationCode).Selected = true;
                }


            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
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
            DropDownList ddl_LocationCode = grd_Outlet.Rows[e.RowIndex].FindControl("ddl_LocationCode") as DropDownList;
            TextBox txt_OutletName = grd_Outlet.Rows[e.RowIndex].FindControl("txt_OutletName") as TextBox;

            //if (ddl_LocationCode.Text != string.Empty)
            //{
            string outletCode = grd_Outlet.DataKeys[e.RowIndex].Value.ToString();

            string sql = string.Format("UPDATE PT.Outlet SET LocationCode = N'{0}', OutletName = N'{1}'", ddl_LocationCode.SelectedValue, txt_OutletName.Text);
            sql += string.Format(" WHERE OutletCode = N'{0}'", outletCode);
            bu.DbExecuteQuery(@sql, null, LoginInfo.ConnStr);
            //}

            (sender as GridView).EditIndex = -1;
            BindOutletData();
        }

        protected void grd_Outlet_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string outletCode = grd_Outlet.DataKeys[e.RowIndex].Value.ToString();

            string sql = string.Format("DELETE FROM PT.Outlet WHERE OutletCode = N'{0}'", outletCode);
            bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
            BindOutletData();
        }

        #endregion

        #region Department
        protected void btn_Department_Click(object sender, EventArgs e)
        {
            GetNewDepartment();

            BindDepartmentData();
            pop_Department.ShowOnPageLoad = true;

        }

        protected void menu_Department_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "PRINT":
                    break;
                case "BACK":
                    pop_Department.ShowOnPageLoad = false;
                    break;

            }
        }

        protected void btn_DepartmentAdd_Click(object sender, EventArgs e)
        {
            // Check existing code

            string depCode = txt_NewDepartmentCode.Text;

            string sql = string.Format("SELECT COUNT(DepartmentCode) as RecordCount FROM PT.Department WHERE DepartmentCode='{0}'", depCode);
            DataTable dt = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                AlertBox(string.Format("'{0}' is duplicated.", depCode));
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
                    AlertBox("Duplicate Department Code!");
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

        #endregion

        #region Item

        protected void btn_Item_Click(object sender, EventArgs e)
        {
            //GetNewItem();
            BindItemData();

            // Bind location
            string type = ddl_NewItemType.SelectedIndex == 0 ? "R" : "P";

            ddl_NewItemRecipe.DataSource = GetProductRecipe(type);
            //ddl_NewItemRecipe.DataValueField = "ProductCode";
            //ddl_NewItemRecipe.DataTextField = "ProductName";
            ddl_NewItemRecipe.ValueField = "ProductCode";
            ddl_NewItemRecipe.TextField = "ProductName";
            ddl_NewItemRecipe.DataBind();

            pop_Item.ShowOnPageLoad = true;

        }

        protected void ddl_NewItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = ddl_NewItemType.SelectedIndex == 0 ? "R" : "P";

            ddl_NewItemRecipe.DataSource = GetProductRecipe(type);
            //ddl_NewItemRecipe.DataValueField = "ProductCode";
            //ddl_NewItemRecipe.DataTextField = "ProductName";
            ddl_NewItemRecipe.ValueField = "ProductCode";
            ddl_NewItemRecipe.TextField = "ProductName";
            ddl_NewItemRecipe.DataBind();

        }

        protected void menu_Item_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "PRINT":
                    break;
                case "BACK":
                    pop_Item.ShowOnPageLoad = false;
                    break;

            }
        }

        protected void btn_ItemAdd_Click(object sender, EventArgs e)
        {
            // Check existing code

            string itemCode = txt_NewItemCode.Text;

            string sql = string.Format("SELECT COUNT(ItemCode) as RecordCount FROM PT.Item WHERE ItemCode='{0}'", itemCode);
            DataTable dt = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                AlertBox(string.Format("'{0}' is duplicated.", itemCode));
            }
            else
            {
                string itemName = txt_NewItemName.Text;
                //string rcpCode = ddl_NewItemRecipe.SelectedValue.ToString();
                string rcpType = ddl_NewItemType.SelectedIndex == 0 ? "R" : "P";
                string rcpCode = ddl_NewItemRecipe.SelectedItem.Value.ToString();
                string locationCode = ddl_NewOutletLocation.SelectedValue.ToString();
                sql = string.Format("INSERT INTO PT.Item (ItemCode, ItemName, ProductCode, ProductType) VALUES(N'{0}', N'{1}', N'{2}', N'{3}') ", itemCode.ToUpper(), itemName, rcpCode, rcpType);
                bu.DbParseQuery(@sql, null, LoginInfo.ConnStr);
                BindItemData();

                txt_NewDepartmentCode.Text = string.Empty;
                txt_NewDepartmentName.Text = string.Empty;
            }

        }

        //Grid Item
        protected void grd_Item_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            (sender as GridView).PageIndex = e.NewPageIndex;
            (sender as GridView).DataSource = dtDepartment;
            (sender as GridView).DataBind();

        }

        protected void grd_Item_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Product Code
                var lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                if (lbl_ProductCode != null)
                {
                    lbl_ProductCode.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                    lbl_ProductCode.ToolTip = lbl_ProductCode.Text;
                }

                // Product Type
                var lbl_ProductType = e.Row.FindControl("lbl_ProductType") as Label;
                if (lbl_ProductType != null)
                {
                    string productType = DataBinder.Eval(e.Row.DataItem, "ProductType").ToString();
                    lbl_ProductType.Text = productType == "R" ? "Recipe" : "Product";
                    lbl_ProductType.ToolTip = lbl_ProductType.Text;
                }


                // Product Type
                var ddl_ProductType = e.Row.FindControl("ddl_ProductType") as DropDownList;
                if (ddl_ProductType != null)
                {
                    string productType = DataBinder.Eval(e.Row.DataItem, "ProductType").ToString();
                    ddl_ProductType.SelectedIndex = productType == "R" ? 0 : 1;
                    ddl_ProductType.ToolTip = ddl_ProductType.Text;
                }


                if ((sender as GridView).EditIndex == e.Row.RowIndex)
                {
                    DropDownList ddl_ProductCode = (DropDownList)e.Row.FindControl("ddl_ProductCode");
                    //string rcpType = ddl_NewItemType.SelectedIndex == 0 ? "R" : "P";
                    ddl_ProductCode.DataSource = GetProductRecipe(DataBinder.Eval(e.Row.DataItem, "ProductType").ToString());
                    ddl_ProductCode.DataBind();

                    string productCode = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();

                    if (ddl_ProductCode.Items.FindByValue(productCode) == null)
                    {
                        ddl_ProductCode.Items.Insert(0, new ListItem(productCode, ""));
                        ddl_ProductCode.SelectedIndex = 0;
                    }
                    else
                        ddl_ProductCode.Items.FindByValue(productCode).Selected = true;
                }


            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
            }
        }

        protected void grd_Item_RowEditing(object sender, GridViewEditEventArgs e)
        {


            (sender as GridView).EditIndex = e.NewEditIndex;
            BindItemData();

        }

        protected void grd_Item_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string dataKey = (sender as GridView).DataKeys[e.RowIndex].Value.ToString();

            string sql = string.Format("DELETE FROM PT.Item WHERE ItemCode = '{0}'", dataKey);
            bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
            BindItemData();
        }

        protected void grd_Item_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string itemName = ((sender as GridView).Rows[e.RowIndex].FindControl("txt_ItemName") as TextBox).Text;
            string rcpCode = ((sender as GridView).Rows[e.RowIndex].FindControl("ddl_ProductCode") as DropDownList).SelectedValue.ToString();
            string rcpType = ((sender as GridView).Rows[e.RowIndex].FindControl("ddl_ProductType") as DropDownList).SelectedIndex == 0 ? "R" : "P";
            string dataKey = (sender as GridView).DataKeys[e.RowIndex].Value.ToString();

            string sql = string.Format("UPDATE PT.Item SET ItemName = '{0}', ProductCode = '{1}', ProductType = '{2}'", itemName, rcpCode, rcpType);
            sql += string.Format(" WHERE ItemCode = '{0}'", dataKey);

            bu.DbExecuteQuery(@sql, null, LoginInfo.ConnStr);

            (sender as GridView).EditIndex = -1;
            BindItemData();
        }

        protected void grd_Item_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            (sender as GridView).EditIndex = -1;
            BindItemData();
        }

        protected void ddl_ProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl_ProductType = grd_Item.Rows[grd_Item.EditIndex].FindControl("ddl_ProductType") as DropDownList;
            var ddl_ProductCode = grd_Item.Rows[grd_Item.EditIndex].FindControl("ddl_ProductCode") as DropDownList;

            string type = ddl_ProductType.SelectedIndex == 0 ? "R" : "P";

            ddl_ProductCode.DataSource = GetProductRecipe(type);
            ddl_ProductCode.DataValueField = "ProductCode";
            ddl_ProductCode.DataTextField = "ProductName";

            //ddl.ValueField = "ProductCode";
            //ddl.TextField = "ProductName";
            ddl_ProductCode.DataBind();

        }

        #endregion

        #region Stock Out
        
        protected void btn_SoDate_Click(object sender, EventArgs e)
        {
            BindSaleData(grd_StockOut, de_SoDate.Date, de_SoDate.Date); 
        }

        protected void grd_StockOut_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            (sender as GridView).PageIndex = e.NewPageIndex;
            BindSaleData(grd_StockOut, de_SoDate.Date, de_SoDate.Date); 
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


        #endregion

        #region Function(s)

        private void AlertBox(string text)
        {
            lbl_Alert.Text = text;
            pop_Alert.ShowOnPageLoad = true;
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

        private void BindSaleData(GridView gv, DateTime dateFrom, DateTime dateTo, string filterSearch = "")
        {

            //bool isFilterByDate = chk_FilterByDate.Checked;
            //string filterSearch = txt_Search.Text;

            string sql = ";WITH sale AS(";
            sql += "SELECT";
            sql += " ID,";
            sql += " SaleDate,";
            sql += " RevenueCode,";
            sql += " s.Outlet as OutletCode,";
            sql += " s.Outlet + ' : ' + ISNULL(o.OutletName,'') as Outlet,";
            sql += " s.DepartmentCode,";
            sql += " s.DepartmentCode + ' : ' + ISNULL(d.DepartmentName,'') as Department,";
            sql += " s.ItemCode,";
            sql += " s.ItemCode + ' : ' + ISNULL(i.ItemName,'') as Item,";
            sql += " Qty,";
            sql += " Price,";
            sql += " Total,";
            sql += " IsPosted,";
            sql += " RefNo,";
            sql += " Void";
            sql += " FROM PT.Sale s";
            sql += " LEFT JOIN PT.Outlet o ON o.OutletCode = s.Outlet";
            sql += " LEFT JOIN PT.Department d ON d.DepartmentCode = s.DepartmentCode";
            sql += " LEFT JOIN PT.Item i ON i.ItemCode = s.ItemCode";
            sql += string.Format(" WHERE CAST(s.SaleDate AS DATE) BETWEEN '{0}' AND '{1}'", dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"));
            //if (isFilterByDate)
            //{
            //    string dateFrom = de_DateFrom.Date.ToString("yyyy-MM-dd");
            //    string dateTo = de_DateTo.Date.ToString("yyyy-MM-dd");

            //    sql += string.Format(" WHERE CAST(s.SaleDate AS DATE) BETWEEN '{0}' AND '{1}'", dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"));
            //}
            sql += ") ";

            sql += " SELECT * FROM sale";
            if (filterSearch != string.Empty)
                sql += string.Format(" WHERE Outlet LIKE N'%{0}%' OR DepartmentCode LIKE N'%{0}%' OR ItemCode LIKE N'%{0}%'", filterSearch);
            //sql += " ORDER BY SaleDate, RevenueCode, Outlet, DepartmentCode, ItemCode";
            sql += " ORDER BY ID";

            //dsSale = null;
            //bu.DbExecuteQuery(sql, dsSale, null, "Sale", LoginInfo.ConnStr);
            //DataTable dt = dsSale.Tables["Sale"];
            dtSale = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            //grd_Sale.PageSize = Convert.ToInt32(txt_PageSize.Text);
            //grd_Sale.DataSource = dtSale;
            //grd_Sale.DataBind();
            gv.PageSize = Convert.ToInt32(txt_PageSize.Text);
            gv.DataSource = dtSale;
            gv.DataBind();

            if (dtSale.Rows.Count > 0)
            {
                // Calculate SUM of Total
                object sumTotal;
                sumTotal = dtSale.Compute("SUM(Total)", string.Empty);
                //lbl_SumTotal.Text = string.Format(DefaultAmtFmt, Convert.ToDecimal(sumTotal.ToString()));
                //lbl_SumTotal_Nm.Visible = true;
                //lbl_SumTotal.Visible = true;
            }
            else
            {
                //lbl_SumTotal_Nm.Visible = false;
                //lbl_SumTotal.Visible = false;
                //lbl_SumTotal.Text = string.Empty;
            }
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

        private void BindItemData()
        {
            string sql = string.Empty;

            sql = "SELECT ItemCode, ItemName, ProductCode, ProductType";
            sql += " FROM PT.Item";
            sql += " ORDER BY ItemCode";

            dtItem = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            grd_Item.DataSource = dtItem;
            grd_Item.DataBind();
        }

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
                string saleDate, outlet, departmentCode, itemCode, itemName;
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

                    sql = "INSERT INTO PT.Sale (SaleDate, Outlet, DepartmentCode, ItemCode,  Qty, Price, Total, Void)";
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

        protected DataTable GetLocation()
        {
            string sql = "SELECT LocationCode, LocationCode + ' : ' + LocationName as Location FROM [IN].StoreLocation ORDER BY LocationCode";
            return bu.DbExecuteQuery(@sql, null, LoginInfo.ConnStr);
        }

        protected DataTable GetProductRecipe(string type)
        {
            //string sql = "SELECT RcpCode,  RcpCode + ' : ' + ISNULL(RcpDesc1,'') + ' | ' + ISNULL(RcpDesc2,'') as RcpName";
            //sql += " FROM [PT].Rcp";
            //sql += " WHERE IsActived = 1";
            //sql += "ORDER BY RcpCode";
            string sql = string.Empty;
            if (type.ToUpper() == "R") // Recipe
            {
                sql = @"SELECT
	                        RcpCode as ProductCode,  
	                        RcpCode + ' : ' + ISNULL(RcpDesc1,'') + ' | ' + ISNULL(RcpDesc2,'') as ProductName
                        FROM 
                            [PT].Rcp
                        WHERE 
                            IsActived = 1
                        ORDER BY 
                            RcpCode";
            }
            else
            {
                sql = @"SELECT
	                        ProductCode as ProductCode,
	                        ProductCode + ' : ' + ISNULL(ProductDesc1,'') + ' | ' + ISNULL(ProductDesc2,'') as ProductName
                        FROM
	                        [IN].Product
                        WHERE
	                        IsActive = 1
	                        AND IsRecipe = 1
                        ORDER BY 
                            ProductCode";

            }
            return bu.DbExecuteQuery(@sql, null, LoginInfo.ConnStr);

        }

        private void GetNewOutlet()
        {
            string sql = "INSERT INTO PT.Outlet (OutletCode)";
            sql += " SELECT DISTINCT s.Outlet";
            sql += " FROM PT.Sale s";
            sql += " LEFT JOIN PT.Outlet o ON o.OutletCode = s.Outlet";
            sql += " WHERE o.OutletCode IS NULL";

            bu.DbExecuteQuery(@sql, null, LoginInfo.ConnStr);
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

        private void GetNewItem()
        {
            string sql = "INSERT INTO PT.Item (ItemCode)";
            sql += " SELECT DISTINCT s.ItemCode";
            sql += " FROM PT.Sale s";
            sql += " LEFT JOIN PT.Item i ON i.ItemCode = s.ItemCode";
            sql += " WHERE i.ItemCode IS NULL";

            bu.DbExecuteQuery(@sql, null, LoginInfo.ConnStr);
        }

        #endregion
    }
}
