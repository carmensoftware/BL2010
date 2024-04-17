using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Inventory
{
    public partial class ProdBu : BaseUserControl
    {
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.Option.Inventory.Product prod = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private readonly Blue.BL.Option.Inventory.ProdLoc prodLoc = new Blue.BL.Option.Inventory.ProdLoc();
        private readonly Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private readonly Blue.BL.Option.Inventory.Unit unit = new Blue.BL.Option.Inventory.Unit();

        private string _connStr;
        private DataSet _dataSource = new DataSet();
        private string _productCode;

        /// <summary>
        ///     Gets or Set product code.
        /// </summary>
        public string ProductCode
        {
            get
            {
                if (ViewState["ProductCode"] != null)
                {
                    _productCode = ViewState["ProductCode"].ToString();
                }

                return _productCode;
            }
            set
            {
                _productCode = value;
                ViewState["ProductCode"] = _productCode;
            }
        }

        public string ConnStr
        {
            get
            {
                if (ViewState["ConnStr"] != null)
                {
                    _connStr = ViewState["ConnStr"].ToString();
                }

                return _connStr;
            }
            set
            {
                _connStr = value;
                ViewState["ConnStr"] = _connStr;
            }
        }

        /// <summary>
        ///     Keep which BU and Store has this product
        /// </summary>
        private DataSet DataSource
        {
            get
            {
                if (Session["DataSourceProdBU"] != null)
                {
                    _dataSource = (DataSet) Session["DataSourceProdBU"];
                }

                return _dataSource;
            }
            set
            {
                _dataSource = value;
                Session["DataSourceProdBU"] = _dataSource;
            }
        }

        /// <summary>
        ///     Display All Product's Location Data in All Properties
        /// </summary>
        public override void DataBind()
        {
            base.DataBind();

            // Display BU List            
            var dsBu = new DataSet();

            var getBu = bu.GetListNonHQ(dsBu, LoginInfo.BuInfo.BuGrpCode);

            if (getBu)
            {
                rpt_Bu.DataSource = dsBu;
                rpt_Bu.DataBind();
                DataSource = dsBu;
            }

            // Check select all for each BU
            foreach (RepeaterItem riBu in rpt_Bu.Items)
            {
                var count = 0;

                // Find GridView
                var grd_ProdLoc = riBu.FindControl("grd_ProdLoc") as GridView;

                // Start Count
                foreach (GridViewRow gvr_ProdLoc in grd_ProdLoc.Rows)
                {
                    var chk_Sel = gvr_ProdLoc.FindControl("chk_Sel") as CheckBox;

                    if (chk_Sel.Checked)
                    {
                        count++;
                    }
                }

                var chk_SelAll = grd_ProdLoc.HeaderRow.FindControl("chk_SelAll") as CheckBox;
                chk_SelAll.Checked = (grd_ProdLoc.Rows.Count == count ? true : false);
            }
        }

        /// <summary>
        ///     Display Business Unit List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rpt_Bu_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Display product's location data
                var dsProdLoc = new DataSet();

                var result = prodLoc.GetList_ProductCode(dsProdLoc, ProductCode,
                    bu.GetConnectionString(DataBinder.Eval(e.Item.DataItem, "BuCode").ToString()));

                if (result)
                {
                    var grd_ProdLoc = e.Item.FindControl("grd_ProdLoc") as GridView;
                    grd_ProdLoc.DataSource = dsProdLoc;
                    grd_ProdLoc.DataBind();
                }
            }
        }

        /// <summary>
        ///     Assign on client click event function to checkbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ProdLoc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Select all row
                var grd_ProdLoc = e.Row.Parent.Parent as GridView;
                var chk_SelAll = e.Row.FindControl("chk_SelAll") as CheckBox;
                chk_SelAll.Attributes.Add("onclick",
                    "javascript:SelectAllLocation(this, '" + grd_ProdLoc.ClientID + "');");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Select each row
                var grd_ProdLoc = e.Row.Parent.Parent as GridView;
                var chk_Sel = e.Row.FindControl("chk_Sel") as CheckBox;
                chk_Sel.Attributes.Add("onclick", "javascript:SelectLocation('" + grd_ProdLoc.ClientID + "');");
            }
        }

        /// <summary>
        ///     Save all product data from HQ to Properties.
        ///     Product, Product Category, Unit and Product's Location (Min and Max also).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgb_Save_Click(object sender, ImageClickEventArgs e)
        {
            // Get HQ product data ------------------------------------------------
            var dsHQProduct = new DataSet();
            var getHQProduct = prod.Get(dsHQProduct, ProductCode, ConnStr);
            DataRow drHQProduct;

            if (getHQProduct)
            {
                drHQProduct = dsHQProduct.Tables[prod.TableName].Rows[0];
            }
            else
            {
                return;
            }
            // --------------------------------------------------------------------

            // Save change to each database
            foreach (RepeaterItem riBu in rpt_Bu.Items)
            {
                // Find bu code from each item to generate connection string.
                var hf_BuCode = riBu.FindControl("hf_BuCode") as HiddenField;
                var lbl_ErrorMessage = riBu.FindControl("lbl_ErrorMessage") as Label;
                var connStr = bu.GetConnectionString(hf_BuCode.Value);

                // Start Validation ---------------------------------------------------
                // Check Product Code Exist, Not allow to assign
                var dsPropertiesProduct = new DataSet();
                var getProd = prod.Get(dsPropertiesProduct, drHQProduct["ProductCode"].ToString(), connStr);

                if (getProd)
                {
                    // Exist product in the properties
                    if (dsPropertiesProduct.Tables[prod.TableName].Rows.Count > 0)
                    {
                        // Same code and same description then update only prodloc also min max.
                        // Same code but difference description
                        var drPropertiesProduct = dsPropertiesProduct.Tables[prod.TableName].Rows[0];

                        if (drHQProduct["ProductDesc1"].ToString().ToUpper() !=
                            drPropertiesProduct["ProductDesc1"].ToString().ToUpper())
                        {
                            // Display error duplicate
                            lbl_ErrorMessage.Text = "The product code is already exist in this business unit";
                            return;
                        }
                    }
                        // Not exist product in properties
                    else
                    {
                        var dsPropertiesProdCat = new DataSet();
                        prodCat.GetStructure(dsPropertiesProdCat, connStr);

                        // Check Product Category Exist in Properties, If not then insert                                    
                        CopyCategory(dsPropertiesProdCat,
                            prodCat.GetCategoryCode(drHQProduct["ProductCate"].ToString(), ConnStr), connStr);

                        // Check Product Sub Category Exist. If not then insert  
                        CopyCategory(dsPropertiesProdCat,
                            prodCat.GetSubCategoryCode(drHQProduct["ProductCate"].ToString(), ConnStr), connStr);

                        // Check Product Item Group Exist. If not the insert
                        CopyCategory(dsPropertiesProdCat, drHQProduct["ProductCate"].ToString(), connStr);

                        // Save Category Data to Database
                        prodCat.Save(dsPropertiesProdCat, connStr);

                        // Check Inventory Unit, Order Unit and Recipe Unit Exist. If not then insert
                        var dsPropertiesUnit = new DataSet();
                        unit.GetStructure(dsPropertiesUnit, connStr);

                        // If not exist unit for product's inventory unit at properties, then insert.
                        if (drHQProduct["InventoryUnit"] != DBNull.Value)
                        {
                            CopyUnit(dsPropertiesUnit, drHQProduct["InventoryUnit"].ToString(), connStr);
                        }

                        // If not exist unit for product's order unit at properties, then insert.
                        if (drHQProduct["OrderUnit"] != DBNull.Value)
                        {
                            CopyUnit(dsPropertiesUnit, drHQProduct["OrderUnit"].ToString(), connStr);
                        }

                        // If not exist unit for product's recipe unit at properties, then insert.
                        if (drHQProduct["RecipeUnit"] != DBNull.Value)
                        {
                            CopyUnit(dsPropertiesUnit, drHQProduct["RecipeUnit"].ToString(), connStr);
                        }

                        // Save Unit Data to Properties Database
                        unit.Save(dsPropertiesUnit, connStr);

                        // Copy Product to Properties                        
                        CopyProduct(dsPropertiesProduct);

                        // Save Product Data to Properties Database
                        prod.Save(dsPropertiesProduct, connStr);

                        // Copy ProdUnit                        
                        var dsPropertiesProdUnit = new DataSet();
                        CopyProdUnit(dsPropertiesProdUnit, connStr);

                        // Save ProdUnit Data to Properties Database
                        prodUnit.Save(dsPropertiesProdUnit, connStr);
                    }
                }
                else
                {
                    // Display error message
                    lbl_ErrorMessage.Text = "Unknow Error, Please contact admin";
                    return;
                }
                // End Validation -----------------------------------------------------

                // Start Update ProdLoc
                // Get exist ProdLoc data.
                var dsProdLoc = new DataSet();
                prodLoc.GetList(dsProdLoc, drHQProduct["ProductCode"].ToString(), connStr);

                // Delete exist ProdLoc data.
                for (var i = dsProdLoc.Tables[prodLoc.TableName].Rows.Count - 1; i >= 0; i--)
                {
                    dsProdLoc.Tables[prodLoc.TableName].Rows[i].Delete();
                }

                // Insert new prodloc data.
                // Find GridView
                var grd_ProdLoc = riBu.FindControl("grd_ProdLoc") as GridView;

                foreach (GridViewRow gvr_ProdLoc in grd_ProdLoc.Rows)
                {
                    if (gvr_ProdLoc.RowType == DataControlRowType.DataRow)
                    {
                        // Find checkbox
                        var chk_Sel = gvr_ProdLoc.FindControl("chk_Sel") as CheckBox;
                        var hf_LocationCode = gvr_ProdLoc.FindControl("hf_LocationCode") as HiddenField;
                        var txt_Min = gvr_ProdLoc.FindControl("txt_Min") as TextBox;
                        var txt_Max = gvr_ProdLoc.FindControl("txt_Max") as TextBox;

                        // Save only checked store
                        if (chk_Sel.Checked)
                        {
                            var drNewProdLoc = dsProdLoc.Tables[prodLoc.TableName].NewRow();
                            drNewProdLoc["LocationCode"] = hf_LocationCode.Value;
                            drNewProdLoc["ProductCode"] = ProductCode;

                            if (txt_Min.Text.Trim() != string.Empty)
                            {
                                drNewProdLoc["MinQty"] = txt_Min.Text;
                            }

                            if (txt_Max.Text.Trim() != string.Empty)
                            {
                                drNewProdLoc["MaxQty"] = txt_Max.Text;
                            }

                            dsProdLoc.Tables[prodLoc.TableName].Rows.Add(drNewProdLoc);
                        }
                    }
                }

                // Save to database
                var saveProdLoc = prodLoc.Save(dsProdLoc, connStr);
            }

            // Refresh control
            DataBind();
        }

        /// <summary>
        ///     Copy category data from HQ to Properties
        /// </summary>
        /// <param name="dsPropertiesProdCat"></param>
        /// <param name="CategoryCode"></param>
        /// <param name="ConnStr"></param>
        private void CopyCategory(DataSet dsPropertiesProdCat, string CategoryCode, string ConnStr)
        {
            if (!prodCat.IsExist(CategoryCode, ConnStr))
            {
                //DataSet dsHQProdCat = new DataSet();                
                var result = prodCat.GetListByCategoryCode(dsPropertiesProdCat, CategoryCode, this.ConnStr);

                if (result)
                {
                    // Start Copy Category Data to Properties
                    var drPropertiesProdCat =
                        dsPropertiesProdCat.Tables[prodCat.TableName].Rows[
                            dsPropertiesProdCat.Tables[prodCat.TableName].Rows.Count - 1];
                    drPropertiesProdCat.SetAdded();

                    drPropertiesProdCat["CreatedDate"] = ServerDateTime;
                    drPropertiesProdCat["CreatedBy"] = LoginInfo.LoginName;
                    drPropertiesProdCat["UpdatedDate"] = ServerDateTime;
                    drPropertiesProdCat["UpdatedBy"] = LoginInfo.LoginName;
                }
            }
        }

        /// <summary>
        ///     Copy unit data from HQ to Properties
        /// </summary>
        /// <param name="dsPropertiesUnit"></param>
        /// <param name="UnitCode"></param>
        /// <param name="ConnStr"></param>
        private void CopyUnit(DataSet dsPropertiesUnit, string UnitCode, string ConnStr)
        {
            // Find Inventory Unit in Table Unit in Properties
            if (!unit.IsExist(UnitCode, ConnStr))
            {
                var result = unit.Get(dsPropertiesUnit, UnitCode, this.ConnStr);

                if (result)
                {
                    // Start Copy Category Data to Properties                   
                    var drPropertiesUnit =
                        dsPropertiesUnit.Tables[unit.TableName].Rows[
                            dsPropertiesUnit.Tables[unit.TableName].Rows.Count - 1];
                    drPropertiesUnit.SetAdded();

                    drPropertiesUnit["CreatedDate"] = ServerDateTime;
                    drPropertiesUnit["CreatedBy"] = LoginInfo.LoginName;
                    drPropertiesUnit["UpdatedDate"] = ServerDateTime;
                    drPropertiesUnit["UpdatedBy"] = LoginInfo.LoginName;
                }
            }
        }

        /// <summary>
        ///     Copy Product Data from HQ to Properties
        /// </summary>
        /// <param name="dsPropertiesProduct"></param>
        private void CopyProduct(DataSet dsPropertiesProduct)
        {
            var result = prod.Get(dsPropertiesProduct, ProductCode, ConnStr);

            if (result)
            {
                var drPropertiesProduct = dsPropertiesProduct.Tables[prod.TableName].Rows[0];
                drPropertiesProduct.SetAdded();

                drPropertiesProduct["CreatedDate"] = ServerDateTime;
                drPropertiesProduct["CreatedBy"] = LoginInfo.LoginName;
                drPropertiesProduct["UpdatedDate"] = ServerDateTime;
                drPropertiesProduct["UpdatedBy"] = LoginInfo.LoginName;
            }
        }

        /// <summary>
        ///     Copy ProdUnit
        /// </summary>
        /// <param name="dsPropertiesProdUnit"></param>
        /// <param name="ConnStr"></param>
        private void CopyProdUnit(DataSet dsPropertiesProdUnit, string ConnStr)
        {
            var getProdUnit = prodUnit.GetList(dsPropertiesProdUnit, ProductCode, this.ConnStr);

            if (getProdUnit)
            {
                foreach (DataRow drProdUnit in dsPropertiesProdUnit.Tables[prodUnit.TableName].Rows)
                {
                    // Check Exist Unit in Table [IN].Unit in Properties Database.
                    // If not Exist, the insert
                    if (!unit.IsExist(drProdUnit["OrderUnit"].ToString(), ConnStr))
                    {
                        var dsUnit = new DataSet();
                        CopyUnit(dsUnit, drProdUnit["OrderUnit"].ToString(), ConnStr);

                        // Save to Properties Database.
                        unit.Save(dsUnit, ConnStr);
                    }

                    drProdUnit.SetAdded();
                }
            }
        }
    }
}