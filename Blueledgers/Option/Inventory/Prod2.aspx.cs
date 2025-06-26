using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Blue.DAL.SQL;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;


namespace BlueLedger.PL.Option.Inventory
{
    public partial class Prod2 : BasePage
    {
        const string moduleID = "2.6";
        const string STATUS_ACTIVE = "Active";
        const string STATUS_INACTIVE = "Inactive";

        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.ADMIN.RolePermission permission = new Blue.BL.ADMIN.RolePermission();


        // URL Parameters
        private string _BuCode { get { return Request.Params["BuCode"].ToString(); } }

        private string _ID { get { return Request.Params["ID"].ToString(); } }

        private string _connStr { get { return LoginInfo.ConnStr; } }



        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            //Page_Retrieve();

        }

        private void Page_Retrieve()
        {
            var productCode = _ID;

            var sql = string.Format("SELECT * FROM [IN].Product WHERE ProductCode='{0}'", productCode);
            var dtProduct = bu.DbExecuteQuery(sql, null, _connStr);

            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                var drProduct = dtProduct.Rows[0];

                var itemGroupCode = drProduct["ProductCate"].ToString();
                var isActive = Convert.ToBoolean(drProduct["IsActive"]);

                //var receive = GetLastReceive(productCode, _connStr);
                //var lastCost = receive != null ? receive.Cost : 0m;
                var lastCost = GetLastCost(productCode);

                hf_ProductCode.Value = productCode;
                lbl_ProductCode.Text = productCode;
                lbl_Status.Text = isActive ? STATUS_ACTIVE : STATUS_INACTIVE;
                lbl_Status.CssClass = isActive ? "badge" : "badge-inactive";
                lbl_LastCost.Text = FormatAmt(lastCost, DefaultAmtDigit);

                var prodcat = GetProductCategory(itemGroupCode, _connStr);

                lbl_Category.Text = prodcat.CategoryName;
                lbl_SubCategory.Text = prodcat.SubCategoryName;
                lbl_ItemGroup.Text = prodcat.ItemGroupName;

                lbl_ProductDesc1.Text = drProduct["ProductDesc1"].ToString();
                lbl_ProductDesc2.Text = drProduct["ProductDesc2"].ToString();
                lbl_BarCode.Text = drProduct["Barcode"].ToString();

                var taxType = drProduct["TaxType"].ToString();
                var taxRate = Convert.ToDecimal(drProduct["TaxRate"]);

                lbl_TaxType.Text = taxType == "A" ? "Added" : taxType == "I" ? "Included" : "None";
                lbl_TaxRate.Text = FormatAmt(taxRate, DefaultAmtDigit);
                lbl_TaxAccCode.Text = drProduct["TaxAccCode"].ToString();


                var stdCost = string.IsNullOrEmpty(drProduct["StandardCost"].ToString()) ? 0m : Convert.ToDecimal(drProduct["StandardCost"]);
                var qtyDiv = string.IsNullOrEmpty(drProduct["QuantityDeviation"].ToString()) ? 0m : Convert.ToDecimal(drProduct["QuantityDeviation"]);
                var priceDiv = string.IsNullOrEmpty(drProduct["PriceDeviation"].ToString()) ? 0m : Convert.ToDecimal(drProduct["PriceDeviation"]);

                lbl_StandardCost.Text = FormatAmt(stdCost, DefaultAmtDigit);
                lbl_QuantityDeviation.Text = FormatAmt(qtyDiv, DefaultAmtDigit);
                lbl_PriceDeviation.Text = FormatAmt(priceDiv, DefaultAmtDigit);

                var createdDate = Convert.ToDateTime(drProduct["CreatedDate"]).ToString("dd/MM/yyyy HH:mm:ss");
                var createdBy = drProduct["CreatedBy"].ToString();

                lbl_Created.Text = string.Format("created on {0} by {1}", createdDate, createdBy);


                var isRecipe = Convert.ToBoolean(drProduct["IsRecipe"]);
                var saleItem = Convert.ToBoolean(drProduct["SaleItem"]);

                chk_IsRecipe.Checked = isRecipe;
                chk_SaleItem.Checked = saleItem;

                lbl_InventoryUnit.Text = drProduct["InventoryUnit"].ToString();
                lbl_OrderUnit.Text = drProduct["OrderUnit"].ToString();
                lbl_RecipeUnit.Text = drProduct["RecipeUnit"].ToString();


                // Order Unit
                sql = string.Format("SELECT OrderUnit as [Code], Rate FROM [IN].ProdUnit WHERE UnitType='O' AND ProductCode='{0}' ORDER BY OrderUnit", productCode);
                gv_OrderUnit.DataSource = bu.DbExecuteQuery(sql, null, _connStr);
                gv_OrderUnit.DataBind();
                // Recipe Unit
                sql = string.Format("SELECT OrderUnit as [Code], Rate FROM [IN].ProdUnit WHERE UnitType='R' AND ProductCode='{0}' ORDER BY OrderUnit", productCode);
                gv_RecipeUnit.DataSource = bu.DbExecuteQuery(sql, null, _connStr);
                gv_RecipeUnit.DataBind();


                // Location
                gv_Location.DataSource = GetProductLocation(productCode); //bu.DbExecuteQuery(sql, null, _connStr);
                gv_Location.DataBind();

                // Vendor
                //gv_Vendor.DataSource = GetProductVendor(productCode); // bu.DbExecuteQuery(sql, null, _connStr);
                //gv_Vendor.DataBind();



            }


            Control_HeaderMenuBar();

            // Display Activity Log
            var log = (PL.UserControls.Log2)Master.FindControl("Log");
            log.Module = "IN";
            log.SubModule = "Product";
            log.RefNo = hf_ProductCode.Value.ToString();
            log.Visible = true;
            log.DataBind();
        }

        private void Control_HeaderMenuBar()
        {
            int pagePermiss = permission.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            menu_CmdBar.Items.FindByName("Create").Visible = (pagePermiss >= 3) ? menu_CmdBar.Items.FindByName("Create").Visible : false;
            menu_CmdBar.Items.FindByName("Edit").Visible = (pagePermiss >= 3) ? menu_CmdBar.Items.FindByName("Edit").Visible : false;
            menu_CmdBar.Items.FindByName("Delete").Visible = (pagePermiss >= 7) ? menu_CmdBar.Items.FindByName("Delete").Visible : false;

            // Assign Store
            btn_AssignLocation.Enabled = (pagePermiss >= 3) ? true : false;
            //pop_AssignStore_btn_Save.Visible = (pagePermiss >= 3) ? true : false;
            //pop_AssignStore_btn_Cancel.Visible = (pagePermiss >= 3) ? true : false;

            // Assing Vendor
            //btn_Pop_AssignVendor_Save.Visible = (pagePermiss >= 3) ? true : false;
            //btn_Pop_AssignVendor_Cancel.Visible = (pagePermiss >= 3) ? true : false;

        }

        //  Event(s)
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Response.Redirect("ProdEdit2.aspx?MODE=new&BuCode=" + Request.Params["BuCode"]);

                    break;
                case "EDIT":
                    Response.Redirect("ProdEdit2.aspx?MODE=Edit&BuCode=" + Request.Params["BuCode"] + "&ID=" + Request.Params["ID"]);

                    break;
                case "DELETE":

                    var productCode = _ID;
                    var modules = GetProductAvailableOnModule(productCode);

                    if (modules != null) // avaialbel in some modules
                    {
                        var message = string.Format("Found this product is available in <b>{0}</b>", string.Join(", ", modules));

                        ShowAlert(message);
                    }
                    else
                    {


                        pop_ConfirmDelete.ShowOnPageLoad = true;
                    }

                    break;
                case "PRINT":
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    break;
                case "BACK":
                    Back();
                    break;
            }
        }


        protected void btn_Pop_ConfirmDelete_Yes_Click(object sender, EventArgs e)
        {
            Delete(_ID);
        }

        protected void btn_AssignLocation_Click(object sender, EventArgs e)
        {
            var productCode = hf_ProductCode.Value.ToString();

            var sql = @"SELECT 
	                        l.LocationCode, 
	                        l.LocationName,
	                        ISNULL(pl.MinQty, 0) as MinQty,
	                        ISNULL(pl.MaxQty, 0) as MaxQty,
	                        CAST(CASE WHEN l.LocationCode=pl.LocationCode THEN 1 ELSE 0 END AS tinyint) as IsChecked
                        FROM 
	                        [IN].StoreLocation l
	                        LEFT JOIN [IN].ProdLoc pl
		                        ON pl.LocationCode=l.LocationCode AND ProductCode=@ProductCode
                        WHERE
	                        l.IsActive = 1";
            var p = new SqlParameter[1]
            {
                new SqlParameter("@ProductCode",productCode)
            };

            gv_AssignLocation.DataSource = ExecuteQuery(sql, p, _connStr);
            gv_AssignLocation.DataBind();


            pop_AssignLocation.ShowOnPageLoad = true;
        }

        protected void btn_AssignLocation_Save_Click(object sender, EventArgs e)
        {
            var gv = gv_AssignLocation;

            var productCode = hf_ProductCode.Value;
            var sql = string.Format("DELETE FROM [IN].ProdLoc WHERE ProductCode='{0}'", productCode);

            ExecuteQuery(sql, null, _connStr);

            for (int i = 0; i <= gv.Rows.Count - 1; i++)
            {
                GridViewRow row = gv.Rows[i];

                var chk_Select = (CheckBox)row.FindControl("chk_Select");

                if (chk_Select.Checked)
                {
                    var lbl_LocationCode = row.FindControl("lbl_LocationCode") as Label;
                    var se_MinQty = row.FindControl("se_MinQty") as ASPxSpinEdit;
                    var se_MaxQty = row.FindControl("se_MaxQty") as ASPxSpinEdit;

                    var locationCode = lbl_LocationCode.Text;
                    var minQty = Convert.ToDecimal(se_MinQty.Value);
                    var maxQty = Convert.ToDecimal(se_MaxQty.Value);



                    sql = "INSERT INTO [IN].ProdLoc (LocationCode, ProductCode, Onhand, MinQty, MaxQty) VALUES (@LocationCode, @ProductCode, 0, @MinQty, @MaxQty)";
                    var p = new List<SqlParameter>();
                    p.Add(new SqlParameter("@LocationCode", locationCode));
                    p.Add(new SqlParameter("@ProductCode", productCode));
                    p.Add(new SqlParameter("@MinQty", minQty));
                    p.Add(new SqlParameter("@MaxQty", maxQty));

                    ExecuteQuery(sql, p.ToArray(), _connStr);
                }

                Page_Retrieve();
            }


            pop_AssignLocation.ShowOnPageLoad = false;
        }

        protected void btn_History_Click(object sender, EventArgs e)
        {
            var productCode = hf_ProductCode.Value;
            var sql = "";
            sql = @"SELECT 
                        TOP(10) 
	                    ROW_NUMBER() OVER(ORDER BY h.DeliDate DESC, h.PoNo DESC) as RowId,  
	                    CAST(h.DeliDate as DATE) as DocDate, 
	                    h.Vendor + ' : ' + v.[Name] as Vendor, 
	                    d.Location + ' : ' + l.LocationName as Location, 
	                    h.[Description], 
	                    h.DocStatus, 
	                    h.PoNo as DocNo, 
	                    d.OrdQty as Qty, 
	                    d.Unit, 
	                    CAST(d.Price as decimal(18,2)) as Price, 
	                    h.CurrencyCode
                    FROM 
	                    PC.PO h
	                    JOIN PC.PoDt d 
		                    ON d.PoNo = h.PoNo
	                    LEFT JOIN [AP].Vendor v 
		                    ON v.VendorCode = h.Vendor
	                    LEFT JOIN [IN].StoreLocation l 
		                    ON l.LocationCode = d.Location
                    WHERE 
	                    h.DocStatus <> 'Voided'
	                    AND d.Product = @ProductCode
                    ORDER BY 
	                    h.DeliDate Desc, 
	                    h.PoNo Desc";
            var p = new SqlParameter[1] { new SqlParameter("ProductCode", productCode) };

            gv_History_PO.DataSource = ExecuteQuery(sql, p, _connStr);
            gv_History_PO.DataBind();


            sql = @"SELECT TOP(10) 
	                    ROW_NUMBER() OVER(ORDER BY h.RecDate DESC, h.RecNo DESC) as RowId,  
	                    CAST(h.RecDate as DATE) as DocDate, 
	                    h.VendorCode + ' : ' + v.[Name] as Vendor, 
	                    d.LocationCode + ' : ' + l.LocationName as Location, 
	                    h.[Description], 
	                    h.DocStatus, 
	                    h.RecNo as DocNo, 
	                    d.RecQty as Qty, 
	                    d.RcvUnit as Unit, 
	                    d.Price,  
	                    h.CurrencyCode
                    FROM 
	                    PC.REC h
	                    JOIN PC.RecDt d 
		                    ON d.RecNo = h.RecNo
	                    LEFT JOIN [AP].Vendor v 
		                    ON v.VendorCode = h.VendorCode
	                    LEFT JOIN [IN].StoreLocation l 
		                    ON l.LocationCode = d.LocationCode
                    WHERE 
	                    h.DocStatus <> 'Voided'
	                    AND d.ProductCode = @ProductCode
                    ORDER BY 
	                    h.RecDate Desc, 
	                    h.RecNo Desc";

            var p1 = new SqlParameter[1] { new SqlParameter("ProductCode", productCode) };

            gv_History_RC.DataSource = ExecuteQuery(sql, p1, _connStr);
            gv_History_RC.DataBind();


            pop_History.ShowOnPageLoad = true;
        }


        /*protected void btn_AssignVendor_Click(object sender, EventArgs e)
        {
            var productCode = hf_ProductCode.Value.ToString();

            var sql = @"SELECT 
	                        v.VendorCode, 
	                        v.[Name] as VendorName,
	                        CAST(CASE WHEN v.VendorCode=vp.VendorCode THEN 1 ELSE 0 END AS tinyint) as IsChecked
                        FROM 
	                        [AP].Vendor v
	                        LEFT JOIN [IN].VendorProd vp
		                        ON v.VendorCode=vp.VendorCode AND ProductCode=@ProductCode
                        WHERE
	                        v.IsActive = 1";
            var p = new SqlParameter[1]
            {
                new SqlParameter("@ProductCode",productCode)
            };

            gv_AssignVendor.DataSource = ExecuteQuery(sql, p, _connStr);
            gv_AssignVendor.DataBind();

            pop_AssignVendor.ShowOnPageLoad = true;
        }

        protected void btn_AssignVendor_Save_Click(object sender, EventArgs e)
        {
            var gv = gv_AssignVendor;

            var productCode = hf_ProductCode.Value;
            var sql = string.Format("DELETE FROM [IN].VendorProd WHERE ProductCode='{0}'", productCode);

            ExecuteQuery(sql, null, _connStr);

            for (int i = 0; i <= gv.Rows.Count - 1; i++)
            {
                GridViewRow row = gv.Rows[i];

                var chk_Select = (CheckBox)row.FindControl("chk_Select");

                if (chk_Select.Checked)
                {
                    var lbl_VendorCode = row.FindControl("lbl_VendorCode") as Label;

                    var vendorCode = lbl_VendorCode.Text;

                    sql = "INSERT INTO [IN].VendorProd (VendorCode, ProductCode) VALUES (@VendorCode, @ProductCode)";

                    var p = new List<SqlParameter>();

                    p.Add(new SqlParameter("@VendorCode", vendorCode));
                    p.Add(new SqlParameter("@ProductCode", productCode));

                    ExecuteQuery(sql, p.ToArray(), _connStr);
                }

                Page_Retrieve();
            }
            pop_AssignVendor.ShowOnPageLoad = false;
        }*/

        // Method(s)

        private void Back()
        {
            Response.Redirect("ProdList.aspx");
        }

        private void Delete(string productCode)
        {
            var sql = string.Format("DELETE FROM [IN].ProdLoc WHERE ProductCode=@ProductCode; DELETE FROM [IN].ProdUnit WHERE ProductCode=@ProductCode; DELETE FROM [IN].Product WHERE ProductCode=@ProductCode;");
            var p = new SqlParameter[] { new SqlParameter("@ProductCode", productCode) };

            ExecuteQuery(sql, p, _connStr);
            Back();
        }

        private DataTable GetProductLocation(string productCode)
        {
            var sql = string.Format("SELECT pl.LocationCode, l.LocationName, ISNULL(MinQty,0) as MinQty, ISNULL(MaxQty,0) as MaxQty FROM [IN].ProdLoc pl JOIN [IN].StoreLocation l ON l.LocationCode=pl.LocationCode WHERE ProductCode = '{0}' ORDER BY pl.LocationCode", productCode);

            return bu.DbExecuteQuery(sql, null, _connStr);
        }

        private DataTable GetProductVendor(string productCode)
        {
            var sql = string.Format("SELECT vp.VendorCode, v.[Name] as VendorName FROM [IN].VendorProd vp JOIN AP.Vendor v ON v.VendorCode=vp.VendorCode WHERE ProductCode = '{0}' ORDER BY vp.VendorCode", productCode);

            return bu.DbExecuteQuery(sql, null, _connStr);
        }

        private void ShowAlert(string text, string headerText = null)
        {

            lbl_Alert.Text = text;
            pop_Alert.HeaderText = string.IsNullOrEmpty(headerText) ? "Alert" : headerText;
            pop_Alert.ShowOnPageLoad = true;
        }


        // -----

        private string[] GetProductAvailableOnModule(string productCode)
        {
            var sql =
@"SELECT TOP(1) 'Purchase Request' as [Module] FROM PC.Pr JOIN PC.PrDt ON pr.PRNo=prdt.PrNo WHERE pr.DocStatus NOT IN ('Voided') AND ProductCode = @ProductCode	
UNION ALL
SELECT TOP(1) 'Purchase Order' as [Module] FROM PC.Po JOIN PC.PoDt ON po.PoNo=podt.PoNo WHERE po.DocStatus NOT IN ('Voided') AND Product = @ProductCode
UNION ALL
SELECT TOP(1) 'Receiving' FROM PC.REC JOIN PC.RECDt ON rec.RecNo=recdt.RecNo WHERE rec.DocStatus NOT IN ('Voided') AND ProductCode = @ProductCode	
UNION ALL
SELECT TOP(1) 'Credit Note' FROM PC.Cn JOIN PC.CnDt ON cn.CnNo=cndt.CnNo WHERE cn.DocStatus NOT IN ('Voided') AND ProductCode = @ProductCode	
UNION ALL
SELECT TOP(1) 'Stock In' FROM [IN].StockIn si JOIN [IN].StockInDt sidt ON si.RefId=sidt.Id WHERE si.[Status] NOT IN ('Voided') AND SKU = @ProductCode
UNION ALL
SELECT TOP(1) 'Stock Out' FROM [IN].StockOut so JOIN [IN].StockOutDt sodt ON so.RefId=sodt.RefId WHERE so.[Status] NOT IN ('Voided') AND SKU = @ProductCode
UNION ALL
SELECT TOP(1) 'Store Requisition' FROM [IN].StoreRequisition sr JOIN [IN].StoreRequisitionDetail srdt ON sr.RefId=srdt.DocumentId  WHERE sr.DocStatus NOT IN ('Voided') AND ProductCode = @ProductCode
UNION ALL
SELECT TOP(1) 'Closing Balance' FROM [IN].Eop JOIN [IN].EopDt ON eop.EopId=eopdt.EopId WHERE eop.[Status] NOT IN ('Voided') AND ProductCode = @ProductCode
";

            var p = new Blue.DAL.DbParameter[1]
            {
                new Blue.DAL.DbParameter("ProductCode", productCode)
            };

            var dt = bu.DbExecuteQuery(sql, p, _connStr);

            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.AsEnumerable().Select(x => x.Field<string>("Module")).ToArray();
            }
            else
                return null;
        }


        // Static Method(s)
        #region -- Static Method(s) --


        public static string FormatAmt(decimal value, int digit = 2)
        {
            return string.Format("{0:N" + digit.ToString() + "}", value);
        }

        public static string FormatQty(decimal value, int digit = 3)
        {
            return string.Format("{0:N" + digit.ToString() + "}", value);
        }

        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters, string connectionString)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    using (var da = new SqlDataAdapter(query, conn))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            foreach (var p in parameters)
                            {
                                da.SelectCommand.Parameters.AddWithValue(p.ParameterName, p.Value);
                            }
                        }

                        var dt = new DataTable();
                        da.Fill(dt);

                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private decimal GetLastCost(string productCode)
        {

            var sql = string.Format(@"SELECT [IN].GetLastCost('{0}', NULL)", productCode);
            var dt = bu.DbExecuteQuery(sql, null, _connStr);

            return dt == null || dt.Rows.Count == 0 ? 0m : Convert.ToDecimal(dt.Rows[0][0]);
        }

//        public static LastCost GetLastReceive(string productCode, string _connStr)
//        {

//            var sql = string.Format(@"SELECT 
//	                                    TOP(1) 
//	                                    i.HdrNo as DocNo, 
//	                                    i.[Type] as DocType , 
//	                                    i.Location as LocationCode, 
//	                                    l.LocationName,
//	                                    i.Amount as Cost, 
//	                                    i.CommittedDate  
//                                    FROM 
//	                                    [IN].Inventory i
//	                                    LEFT JOIN [IN].StoreLocation l
//		                                    ON l.LocationCode=i.Location
//                                    WHERE 
//	                                    [Type] IN ('RC') 
//	                                    AND ProductCode='{0}' 
//                                    ORDER BY CommittedDate DESC", productCode);
//            var dt = ExecuteQuery(sql, null, _connStr);

//            if (dt != null && dt.Rows.Count > 0)
//            {
//                var dr = dt.Rows[0];

//                var docType = dr["DocType"].ToString();

//                return new LastCost
//                {
//                    DocNo = dr["DocNo"].ToString(),
//                    DocType = docType == "RC" ? "Receiving" : "Stock In",
//                    LocationCode = dr["LocationCode"].ToString(),
//                    LocationName = dr["LocationName"].ToString(),
//                    Cost = Convert.ToDecimal(dr["Cost"]),
//                    CommittedDate = Convert.ToDateTime(dr["CommittedDate"])
//                };
//            }
//            else
//                return null;
//        }

        public static Product_Category GetProductCategory(string itemgroupCode, string _connStr)
        {
            var sql = string.Format("SELECT TOP(1) * FROM [IN].vProdCatList WHERE ItemGroupCode='{0}'", itemgroupCode);
            var dt = ExecuteQuery(sql, null, _connStr);

            if (dt != null && dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];

                return new Product_Category
                {
                    CategoryCode = dr["CategoryCode"].ToString(),
                    CategoryName = dr["CategoryName"].ToString(),
                    SubCategoryCode = dr["SubCategoryCode"].ToString(),
                    SubCategoryName = dr["SubCategoryName"].ToString(),
                    ItemGroupCode = dr["ItemGroupCode"].ToString(),
                    ItemGroupName = dr["ItemGroupName"].ToString(),
                };
            }
            else
                return null;
        }



        #endregion
    }

    public class LastCost
    {
        public string DocNo { get; set; }
        public string DocType { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public decimal Cost { get; set; }
        public DateTime CommittedDate { get; set; }
    }

    public class Product_Category
    {
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryCode { get; set; }
        public string SubCategoryName { get; set; }
        public string ItemGroupCode { get; set; }
        public string ItemGroupName { get; set; }
    }
}