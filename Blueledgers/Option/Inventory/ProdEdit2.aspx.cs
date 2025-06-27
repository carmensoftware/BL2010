using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;

namespace BlueLedger.PL.Option.Inventory
{
    public partial class ProdEdit2 : BasePage
    {
        const string STATUS_ACTIVE = "Active";
        const string STATUS_INACTIVE = "Inactive";
        const string TABLE_ORDER_UNIT = "OrderUnit";
        const string TABLE_RECIPE_UNIT = "RecipeUnit";

        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();

        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();

        private string _BuCode { get { return Request.Params["BuCode"].ToString(); } }

        private string _ID { get { return Request.Params["ID"].ToString(); } }

        protected bool _IsEditMode { get { return Request.Params["MODE"].ToString().ToUpper() == "EDIT"; } }

        private string _connStr { get { return LoginInfo.ConnStr; } }


        private DataSet dsProduct
        {
            get
            {
                return (DataSet)Session["dsProduct"];
            }
            set
            {
                Session["dsProduct"] = value;
            }
        }

        private DataTable dtCategory
        {
            get
            {
                return (DataTable)Session["dtCategory"];
            }
            set
            {
                Session["dtCategory"] = value;
            }
        }

        private DataTable dtSubCategory
        {
            get
            {
                return (DataTable)Session["dtSubCategory"];
            }
            set
            {
                Session["dtSubCategory"] = value;
            }
        }

        private DataTable dtItemGroup
        {
            get
            {
                return (DataTable)Session["dtItemGroup"];
            }
            set
            {
                Session["dtItemGroup"] = value;
            }
        }

        private DataTable dtOrderUnit
        {
            get
            {
                return (DataTable)Session["dtOrderUnit"];
            }
            set
            {
                Session["dtOrderUnit"] = value;
            }
        }

        private DataTable dtRecipeUnit
        {
            get
            {
                return (DataTable)Session["dtRecipeUnit"];
            }
            set
            {
                Session["dtRecipeUnit"] = value;
            }
        }

        // Page Load

        protected void Page_Init(object sender, EventArgs e)
        {
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
            }

        }

        private void Page_Retrieve()
        {
            dsProduct = new DataSet();

            if (_IsEditMode)
            {
                product.GetList(dsProduct, _ID, _connStr);

                var dtProduct = dsProduct.Tables[product.TableName];
                var productCode = dtProduct != null && dtProduct.Rows.Count > 0 ? dtProduct.Rows[0]["ProductCode"].ToString() : "";

                dtOrderUnit = GetProductUnit("O", productCode);
                dtRecipeUnit = GetProductUnit("R", productCode);
            }
            else
            {

                dtCategory = GetCategories();

                var categoryCode = dtCategory != null & dtCategory.Rows.Count > 0 ? dtCategory.Rows[0]["Code"].ToString() : "";
                dtSubCategory = GetSubCategories(categoryCode);

                var subCategoryCode = dtSubCategory != null & dtSubCategory.Rows.Count > 0 ? dtSubCategory.Rows[0]["Code"].ToString() : "";
                dtItemGroup = GetItemGroups(subCategoryCode);

                dtOrderUnit = GetProductUnit();
                dtRecipeUnit = GetProductUnit();
            }

            Page_Setting();
        }

        private void Page_Setting()
        {
            var isEditMode = _IsEditMode;

            if (isEditMode)
            {
                var dtProduct = dsProduct.Tables[product.TableName];
                var drProduct = dtProduct.Rows[0];

                var productCode = drProduct["ProductCode"].ToString();
                var isActive = Convert.ToBoolean(drProduct["IsActive"]);
                var taxType = drProduct["TaxType"].ToString();
                var qtyDiv = string.IsNullOrEmpty(drProduct["QuantityDeviation"].ToString()) ? 0m : Convert.ToDecimal(drProduct["QuantityDeviation"]);
                var priceDiv = string.IsNullOrEmpty(drProduct["PriceDeviation"].ToString()) ? 0m : Convert.ToDecimal(drProduct["PriceDeviation"]);
                var stdCost = string.IsNullOrEmpty(drProduct["StandardCost"].ToString()) ? 0m : Convert.ToDecimal(drProduct["StandardCost"]);
                var isRecipe = Convert.ToBoolean(drProduct["IsRecipe"]);
                var saleItem = Convert.ToBoolean(drProduct["SaleItem"]);
                var lastCost = string.IsNullOrEmpty(drProduct["LastCost"].ToString()) ? 0m : Convert.ToDecimal(drProduct["LastCost"]);


                //var receive = GetLastReceive(productCode);
                var prodcat = GetProductCategory(drProduct["ProductCate"].ToString());

                menu_CmdBar.Items.FindByName("SetActive").Text = string.Format("Set to {0}", isActive ? STATUS_INACTIVE : STATUS_ACTIVE);


                hf_ProductCode.Value = productCode;
                hf_IsActive.Value = isActive.ToString();
                lbl_ProductCode.Text = productCode;
                //lbl_Status.Text = isActive ? STATUS_ACTIVE : STATUS_INACTIVE;
                //lbl_Status.CssClass = isActive ? "badge" : "badge-inactive";
                SetActiveStatus(isActive);

                var recNo = drProduct["AddField1"].ToString();
                var recDate = drProduct["AddField2"].ToString();

                lbl_LastCost.Text = FormatAmt(lastCost);
                lbl_LastCost.ToolTip = string.IsNullOrEmpty(recDate) ? "" : string.Format("{0} @{1}", recNo, Convert.ToDateTime(recDate).ToString("dd/MM/yyyy"));

                if (prodcat != null)
                {

                    lbl_Category.Text = prodcat.CategoryName;
                    lbl_SubCategory.Text = prodcat.SubCategoryName;
                    lbl_ItemGroup.Text = prodcat.ItemGroupName;
                }

                txt_ProductDesc1.Text = drProduct["ProductDesc1"].ToString();
                txt_ProductDesc2.Text = drProduct["ProductDesc2"].ToString();

                txt_BarCode.Text = drProduct["BarCode"].ToString();
                ddl_TaxType.SelectedValue = string.IsNullOrEmpty(taxType) ? "N" : taxType;
                se_TaxRate.Value = ddl_TaxType.SelectedValue == "N" ? "0" : drProduct["TaxRate"].ToString();
                se_TaxRate.Enabled = taxType != "N";
                txt_TaxAccCode.Text = drProduct["TaxAccCode"].ToString();


                se_QtyDev.Number = qtyDiv;
                se_PriceDev.Number = priceDiv;
                se_StandardCost.Number = stdCost;

                chk_IsRecipe.Checked = isRecipe;
                chk_SaleItem.Checked = saleItem;

                // Default Units
                var iUnit = drProduct["InventoryUnit"].ToString();
                var oUnit = drProduct["OrderUnit"].ToString();
                var oRate = Convert.ToDecimal(drProduct["InventoryConvOrder"]);
                var rUnit = drProduct["RecipeUnit"].ToString();
                var rRate = Convert.ToDecimal(drProduct["RecipeConvInvent"]);

                txt_InventoryUnit.Text = iUnit;

                SetDefault_OrderUnit(oUnit, oRate);
                SetDefault_RecipeUnit(rUnit, rRate);
            }
            else
            {
                ddl_Category.DataSource = dtCategory;
                ddl_Category.DataBind();

                ddl_SubCategory.DataSource = dtSubCategory;
                ddl_SubCategory.DataBind();

                ddl_ItemGroup.DataSource = dtItemGroup;
                ddl_ItemGroup.DataBind();

            }

            Bind_OrderUnit();
            Bind_RecipeUnit();




            // Product Category
            lbl_Category.Visible = isEditMode;
            lbl_SubCategory.Visible = isEditMode;
            lbl_ItemGroup.Visible = isEditMode;

            ddl_Category.Visible = !isEditMode;
            ddl_SubCategory.Visible = !isEditMode;
            ddl_ItemGroup.Visible = !isEditMode;


            // Inventory Unit
            txt_InventoryUnit.Visible = isEditMode;
            ddl_InventoryUnit.Visible = !isEditMode;

            if (ddl_ItemGroup.SelectedItem != null)
                SetDeviation(ddl_ItemGroup.SelectedItem.Value.ToString(), se_PriceDev, se_QtyDev);


        }



        // Event(s)
        #region --Event(s)--

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SETACTIVE":
                    var isActive = string.IsNullOrEmpty(hf_IsActive.Value) ? true : Convert.ToBoolean(hf_IsActive.Value);

                    SetProductActive(!isActive);
                    break;
                case "SAVE":
                    // Check required fields
                    var error = CheckRequiredFields();

                    if (error == "")
                    {
                        Save();
                    }
                    else
                        ShowAlert(error);
                    break;

                case "BACK":
                    if (_IsEditMode)
                        Response.Redirect(string.Format("Prod2.aspx?BuCode={0}&ID={1}", _BuCode, _ID));
                    else
                        Response.Redirect("ProdList.aspx");
                    break;
            }
        }

        protected void ddl_Category_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var categoryCode = (sender as DropDownList).SelectedValue.ToString();

            dtSubCategory = GetSubCategories(categoryCode);
            ddl_SubCategory.DataSource = dtSubCategory;
            ddl_SubCategory.DataBind();

            var subCategoryCode = dtSubCategory != null && dtSubCategory.Rows.Count > 0 ? dtSubCategory.Rows[0]["Code"].ToString() : "";

            dtItemGroup = GetItemGroups(subCategoryCode);
            ddl_ItemGroup.DataSource = dtItemGroup;
            ddl_ItemGroup.DataBind();

            if (ddl_ItemGroup.SelectedItem != null)
                SetDeviation(ddl_ItemGroup.SelectedItem.Value.ToString(), se_PriceDev, se_QtyDev);

        }

        protected void ddl_SubCategory_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var subCategoryCode = (sender as DropDownList).SelectedValue.ToString();

            dtItemGroup = GetItemGroups(subCategoryCode);
            ddl_ItemGroup.DataSource = dtItemGroup;
            ddl_ItemGroup.DataBind();

            if (ddl_ItemGroup.SelectedItem != null)
                SetDeviation(ddl_ItemGroup.SelectedValue.ToString(), se_PriceDev, se_QtyDev);

        }

        protected void ddl_ItemGroup_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            // Get Price/Qty Deviation from ItemGroup -> Sub-Category -> Category
            var itemGroupCode = (sender as DropDownList).SelectedValue.ToString();
            SetDeviation(itemGroupCode, se_PriceDev, se_QtyDev);
        }

        protected void ddl_TaxType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            se_TaxRate.Enabled = (sender as DropDownList).SelectedValue.ToString() != "N";

        }

        protected void ddl_InventoryUnit_Load(object sender, EventArgs e)
        {
            ddl_InventoryUnit.DataSource = GetUnits();
            ddl_InventoryUnit.DataBind();
        }


        // Order Unit

        protected void gv_OrderUnit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var invUnit = _IsEditMode ? txt_InventoryUnit.Text : ddl_InventoryUnit.Value.ToString();
                var unitCode = DataBinder.Eval(e.Row.DataItem, "Code").ToString();
                var lbl_UnitRate = e.Row.FindControl("lbl_UnitRate") as Label;

                if (lbl_UnitRate != null)
                {
                    var rate = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Rate"));

                    lbl_UnitRate.Text = FormatQty(rate);
                    lbl_UnitRate.ToolTip = string.Format("1 {0} = {1} {2}(s)", unitCode, FormatQty(rate), invUnit);
                }
            }
        }

        protected void btn_NewOrderUnit_Click(object sender, EventArgs e)
        {
            if (!_IsEditMode && string.IsNullOrEmpty(ddl_InventoryUnit.Text))
            {
                ShowAlert("Inventory unit is required.");

                return;
            }

            txt_OrderUnitCode.Visible = false;
            ddl_OrderUnitCode.Visible = true;

            var excludedCodes = GetCurrentCodes(dtOrderUnit, "Code");

            ddl_OrderUnitCode.DataSource = GetUnits(excludedCodes);
            ddl_OrderUnitCode.DataTextField = "CodeName";
            ddl_OrderUnitCode.DataTextField = "Code";
            ddl_OrderUnitCode.DataBind();
            se_OrderUnitRate.Number = 1;
            chk_OrderUnitIsDefault.Checked = false;

            pop_OrderUnit.HeaderText = "New Order Unit";
            pop_OrderUnit.ShowOnPageLoad = true;
        }

        protected void btn_EditOrderUnit_Click(object sender, EventArgs e)
        {
            var row = (GridViewRow)(sender as Button).NamingContainer;
            var lbl_UnitCode = row.FindControl("lbl_UnitCode") as Label;
            var lbl_UnitRate = row.FindControl("lbl_UnitRate") as Label;
            var invUnitCode = _IsEditMode ? txt_InventoryUnit.Text : ddl_InventoryUnit.Value.ToString();

            txt_OrderUnitCode.Visible = true;
            ddl_OrderUnitCode.Visible = false;

            var unitCode = lbl_UnitCode.Text;
            txt_OrderUnitCode.Text = unitCode;
            se_OrderUnitRate.Text = lbl_UnitRate.Text;
            se_OrderUnitRate.Enabled = unitCode != invUnitCode;
            chk_OrderUnitIsDefault.Checked = unitCode == txt_OrderUnit.Text;
            chk_OrderUnitIsDefault.Enabled = !chk_OrderUnitIsDefault.Checked;

            pop_OrderUnit.HeaderText = "Edit Order Unit";
            pop_OrderUnit.ShowOnPageLoad = true;
        }

        protected void btn_DeleteOrderUnit_Click(object sender, EventArgs e)
        {
            var row = (GridViewRow)(sender as Button).NamingContainer;
            var lbl_UnitCode = row.FindControl("lbl_UnitCode") as Label;

            var productCode = hf_ProductCode.Value;
            var unitCode = lbl_UnitCode.Text;


            if (unitCode == txt_OrderUnit.Text)
            {
                ShowAlert("Unable to delete the default order unit. Please changing the default unit code before deleting.");

                return;
            }

            var module = IsAbleToDeleteOrderUnit(productCode, unitCode);

            if (!string.IsNullOrEmpty(module))
            {
                ShowAlert(string.Format("This code '{0}' is using on {1}.", unitCode, module));

                return;
            }

            DeleteRow(dtOrderUnit, "Code", unitCode);
            Bind_OrderUnit();


        }

        protected void btn_SaveOrderUnit_Click(object sender, EventArgs e)
        {
            var isCreateMode = ddl_OrderUnitCode.Visible;
            var unitRate = se_OrderUnitRate.Number;
            var unitCode = isCreateMode ? ddl_OrderUnitCode.SelectedValue.ToString() : txt_OrderUnitCode.Text;
            var isDefault = chk_OrderUnitIsDefault.Checked;

            if (unitRate <= 0)
            {
                ShowAlert("Unit rate does not allow to be zero or negative value.");

                return;
            }

            if (isCreateMode)
            {
                var dr = dtOrderUnit.NewRow();

                dr["Code"] = unitCode;
                dr["Rate"] = unitRate;
                dr["IsDefault"] = isDefault;

                dtOrderUnit.Rows.Add(dr);

            }
            else
            {
                foreach (DataRow dr in dtOrderUnit.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        if (dr["Code"].ToString() == unitCode)
                        {
                            if (Convert.ToDecimal(dr["Rate"]) != unitRate)
                                dr["Rate"] = unitRate;

                            if (Convert.ToBoolean(dr["IsDefault"]) != isDefault)
                                dr["IsDefault"] = isDefault;

                            break;
                        }
                    }
                }
            }

            if (isDefault && txt_OrderUnit.Text != unitCode) // set new default code
            {
                SetDefault_OrderUnit(unitCode, unitRate);
            }

            Bind_OrderUnit();
            pop_OrderUnit.ShowOnPageLoad = false;
        }

        // Recipe Unit

        protected void gv_RecipeUnit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var invUnit = _IsEditMode ? txt_InventoryUnit.Text : ddl_InventoryUnit.Value.ToString();
                var unitCode = DataBinder.Eval(e.Row.DataItem, "Code").ToString();
                var lbl_UnitRate = e.Row.FindControl("lbl_UnitRate") as Label;

                if (lbl_UnitRate != null)
                {
                    var rate = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Rate"));

                    lbl_UnitRate.Text = FormatQty(rate);
                    lbl_UnitRate.ToolTip = string.Format("1 {0} = {1} {2}(s)", invUnit, FormatQty(rate), unitCode);
                }
            }
        }

        protected void btn_NewRecipeUnit_Click(object sender, EventArgs e)
        {
            if (!_IsEditMode && string.IsNullOrEmpty(ddl_InventoryUnit.Text))
            {
                ShowAlert("Inventory unit is required.");

                return;
            }


            txt_RecipeUnitCode.Visible = false;
            ddl_RecipeUnitCode.Visible = true;

            var excludedCodes = GetCurrentCodes(dtRecipeUnit, "Code");

            ddl_RecipeUnitCode.DataSource = GetUnits(excludedCodes);
            ddl_RecipeUnitCode.DataTextField = "CodeName";
            ddl_RecipeUnitCode.DataTextField = "Code";
            ddl_RecipeUnitCode.DataBind();
            se_RecipeUnitRate.Number = 1;
            chk_RecipeUnitIsDefault.Checked = false;

            pop_RecipeUnit.HeaderText = "New Recipe Unit";
            pop_RecipeUnit.ShowOnPageLoad = true;
        }

        protected void btn_EditRecipeUnit_Click(object sender, EventArgs e)
        {
            var row = (GridViewRow)(sender as Button).NamingContainer;
            var lbl_UnitCode = row.FindControl("lbl_UnitCode") as Label;
            var lbl_UnitRate = row.FindControl("lbl_UnitRate") as Label;
            var invUnitCode = _IsEditMode ? txt_InventoryUnit.Text : ddl_InventoryUnit.Value.ToString();

            txt_RecipeUnitCode.Visible = true;
            ddl_RecipeUnitCode.Visible = false;

            var unitCode = lbl_UnitCode.Text;
            txt_RecipeUnitCode.Text = unitCode;
            se_RecipeUnitRate.Text = lbl_UnitRate.Text;
            se_RecipeUnitRate.Enabled = unitCode != invUnitCode;

            chk_RecipeUnitIsDefault.Checked = unitCode == txt_RecipeUnit.Text;
            chk_RecipeUnitIsDefault.Enabled = !chk_RecipeUnitIsDefault.Checked;

            pop_RecipeUnit.HeaderText = "Edit Recipe Unit";
            pop_RecipeUnit.ShowOnPageLoad = true;
        }

        protected void btn_DeleteRecipeUnit_Click(object sender, EventArgs e)
        {
            var row = (GridViewRow)(sender as Button).NamingContainer;
            var lbl_UnitCode = row.FindControl("lbl_UnitCode") as Label;

            var productCode = hf_ProductCode.Value;
            var unitCode = lbl_UnitCode.Text;


            if (unitCode == txt_RecipeUnit.Text)
            {
                ShowAlert("Unable to delete the default Recipe unit. Please changing the default unit code before deleting.");

                return;
            }

            var module = IsAbleToDeleteRecipeUnit(productCode, unitCode);

            if (!string.IsNullOrEmpty(module))
            {
                ShowAlert(string.Format("This code '{0}' is using on {1}.", unitCode, module));

                return;
            }

            DeleteRow(dtRecipeUnit, "Code", unitCode);
            Bind_RecipeUnit();
        }

        protected void btn_SaveRecipeUnit_Click(object sender, EventArgs e)
        {
            var isCreateMode = ddl_RecipeUnitCode.Visible;
            var unitRate = se_RecipeUnitRate.Number;
            var unitCode = isCreateMode ? ddl_RecipeUnitCode.SelectedValue.ToString() : txt_RecipeUnitCode.Text;
            var isDefault = chk_RecipeUnitIsDefault.Checked;

            if (unitRate <= 0)
            {
                ShowAlert("Unit rate does not allow to be zero or negative value.");

                return;
            }

            if (isCreateMode)
            {
                var dr = dtRecipeUnit.NewRow();

                dr["Code"] = unitCode;
                dr["Rate"] = unitRate;
                dr["IsDefault"] = isDefault;

                dtRecipeUnit.Rows.Add(dr);

            }
            else
            {
                foreach (DataRow dr in dtRecipeUnit.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        if (dr["Code"].ToString() == unitCode)
                        {
                            if (Convert.ToDecimal(dr["Rate"]) != unitRate)
                                dr["Rate"] = unitRate;

                            if (Convert.ToBoolean(dr["IsDefault"]) != isDefault)
                                dr["IsDefault"] = isDefault;

                            break;
                        }
                    }
                }
            }

            if (isDefault && txt_RecipeUnit.Text != unitCode) // set new default code
            {
                SetDefault_RecipeUnit(unitCode, unitRate);
            }

            Bind_RecipeUnit();
            pop_RecipeUnit.ShowOnPageLoad = false;
        }

        #endregion

        // Method(s)

        private void ShowAlert(string text, string caption = null)
        {
            lbl_Alert.Text = text;
            pop_Alert.HeaderText = string.IsNullOrEmpty(caption) ? "Warning" : caption;
            pop_Alert.ShowOnPageLoad = true;
        }

        private bool IsValidDb(DataTable dt)
        {
            return dt != null && dt.Rows.Count > 0;
        }

        // -------------------
        private void Bind_OrderUnit()
        {
            // Oder Unit
            gv_OrderUnit.DataSource = dtOrderUnit;
            gv_OrderUnit.DataBind();

        }

        private void Bind_RecipeUnit()
        {
            // RecipeUnit
            gv_RecipeUnit.DataSource = dtRecipeUnit;
            gv_RecipeUnit.DataBind();
        }

        // -------------------

        private void SetProductActive(bool value)
        {
            var dt = dsProduct.Tables[product.TableName];

            if (value == false) // set to inactive
            {
                var productCode = hf_ProductCode.Value.ToString();
                var documents = GetActiveDocuments(productCode);
                var locqty = GetOnhandByLocation(productCode);

                if (documents != null || locqty != null)
                {
                    pop_Document.HeaderText = "Product is not able to inactive";
                    pop_Document.HeaderStyle.BackColor = Color.Red;
                    pop_Document.HeaderStyle.ForeColor = Color.White;

                    lbl_Document.Text = "The product is active on these documents";

                    gv_Document.DataSource = documents;
                    gv_Document.DataBind();

                    gv_LocatonOnhand.DataSource = locqty;
                    gv_LocatonOnhand.DataBind();

                    pop_Document.ShowOnPageLoad = true;

                    return;
                }


                dt.Rows[0]["IsActive"] = 0;

            }

            dt.Rows[0]["IsActive"] = 1;

            SetActiveStatus(value);


        }

        private void SetActiveStatus(bool isActive)
        {
            hf_IsActive.Value = isActive.ToString();

            lbl_Status.Text = isActive ? STATUS_ACTIVE : STATUS_INACTIVE;
            lbl_Status.CssClass = isActive ? "badge" : "badge-inactive";
            menu_CmdBar.Items.FindByName("SetActive").Text = isActive ? "Set to inactive" : "Set to active";
        }

        private string CheckRequiredFields()
        {

            // Description
            if (string.IsNullOrEmpty(txt_ProductDesc1.Text.Trim()))
            {

                return "Product description is required.";
            };

            // Inventory Unit
            var invUnit = _IsEditMode ? txt_InventoryUnit.Text : ddl_InventoryUnit.Text;
            if (string.IsNullOrEmpty(invUnit))
            {

                return "Inventory unit is required.";
            }

            if (dtOrderUnit.Rows.Count == 0)
            {
                return "Order unit is required.";
            }


            return string.Empty;
        }

        private void Save()
        {
            var query = new Helpers.SQL(_connStr);

            #region -- Set variables--

            var productCate = lbl_ItemGroup.Visible ? lbl_ItemGroup.Text : ddl_ItemGroup.SelectedValue.ToString();

            var productCode = _IsEditMode ? hf_ProductCode.Value : product.GetGenerateProductCode(productCate, _connStr);
            var productDesc1 = txt_ProductDesc1.Text;
            var productDesc2 = txt_ProductDesc2.Text;
            var barcode = txt_BarCode.Text;
            var inventoryUnit = _IsEditMode ? txt_InventoryUnit.Text : ddl_InventoryUnit.Value.ToString();
            var orderUnit = txt_OrderUnit.Text;
            var inventoryConvOrder = string.IsNullOrEmpty(hf_InventoryConvOrder.Value) ? 1 : Convert.ToDecimal(hf_InventoryConvOrder.Value);
            var recipeUnit = txt_RecipeUnit.Text;
            var recipeConvInvent = string.IsNullOrEmpty(hf_RecipeConvInvent.Value) ? 1 : Convert.ToDecimal(hf_RecipeConvInvent.Value);
            var taxType = ddl_TaxType.SelectedValue.ToString();
            var taxRate = taxType == "N" ? 0 : Convert.ToDecimal(se_TaxRate.Value);
            var standardCost = Convert.ToDecimal(se_StandardCost.Value);
            var priceDeviation = Convert.ToDecimal(se_PriceDev.Value);
            var quantityDeviation = Convert.ToDecimal(se_QtyDev.Value);

            var isActive = string.IsNullOrEmpty(hf_IsActive.Value) ? 1 : Convert.ToBoolean(hf_IsActive.Value) ? 1 : 0;
            var isRecipe = chk_IsRecipe.Checked ? 1 : 0;
            var saleItem = chk_SaleItem.Checked ? 1 : 0;

            var reqHQAppr = 0;
            var accountCode = "";
            var taxAccCode = txt_TaxAccCode.Text;
            var lastCost = 0m;
            var min = 0;
            var max = 0;

            #endregion

            var sql = new StringBuilder();

            if (_IsEditMode)
            {
                #region --Save Edit--
                sql.Clear();
                sql.Append("UPDATE [IN].Product SET ");

                sql.Append(" ProductDesc1=@ProductDesc1,");
                sql.Append(" ProductDesc2=@ProductDesc2,");
                //sql.Append(" ProductCate=@ProductCate,");
                sql.Append(" BarCode=@BarCode,");

                sql.Append(" OrderUnit=@OrderUnit, ");
                sql.Append(" InventoryConvOrder=@InventoryConvOrder,");
                sql.Append(" RecipeUnit=@RecipeUnit,");
                sql.Append(" RecipeConvInvent=@RecipeConvInvent,");
                sql.Append(" IsRecipe=@IsRecipe,");
                sql.Append(" SaleItem=@SaleItem,");

                sql.Append(" TaxType=@TaxType,");
                sql.Append(" TaxRate=@TaxRate,");
                sql.Append(" StandardCost=@StandardCost,");
                sql.Append(" PriceDeviation=@PriceDeviation,");
                sql.Append(" QuantityDeviation=@QuantityDeviation,");
                sql.Append(" IsActive=@IsActive,");

                sql.Append(" AccountCode=@AccountCode,");
                sql.Append(" TaxAccCode=@TaxAccCode,");
                sql.Append(" ReqHQAppr=@ReqHQAppr,");

                sql.Append(" UpdatedDate=@UpdatedDate, ");
                sql.Append(" UpdatedBy=@UpdatedBy");

                sql.Append(" WHERE ProductCode=@ProductCode");

                var p = new List<SqlParameter>();

                p.Add(new SqlParameter("ProductDesc1", productDesc1));
                p.Add(new SqlParameter("ProductDesc2", productDesc2));
                //p.Add(new SqlParameter("ProductCate", productCate));
                p.Add(new SqlParameter("BarCode", barcode));

                p.Add(new SqlParameter("OrderUnit", orderUnit));
                p.Add(new SqlParameter("InventoryConvOrder", inventoryConvOrder));
                p.Add(new SqlParameter("RecipeUnit", recipeUnit));
                p.Add(new SqlParameter("RecipeConvInvent", recipeConvInvent));
                p.Add(new SqlParameter("IsRecipe", isRecipe));
                p.Add(new SqlParameter("SaleItem", saleItem));

                p.Add(new SqlParameter("TaxType", taxType));
                p.Add(new SqlParameter("TaxRate", taxRate));
                p.Add(new SqlParameter("StandardCost", standardCost));
                p.Add(new SqlParameter("PriceDeviation", priceDeviation));
                p.Add(new SqlParameter("QuantityDeviation", quantityDeviation));
                p.Add(new SqlParameter("IsActive", isActive));

                p.Add(new SqlParameter("AccountCode", accountCode));
                p.Add(new SqlParameter("TaxAccCode", taxAccCode));
                p.Add(new SqlParameter("ReqHQAppr", reqHQAppr));

                p.Add(new SqlParameter("UpdatedDate", DateTime.Now));
                p.Add(new SqlParameter("UpdatedBy", LoginInfo.LoginName));

                p.Add(new SqlParameter("ProductCode", productCode));

                query.ExecuteQuery(sql.ToString(), p.ToArray());

                #endregion
            }
            else
            {
                #region --Save New--
                sql.Clear();
                sql.Append("INSERT INTO [IN].Product (");
                sql.Append("ProductCode, ProductDesc1, ProductDesc2, ProductCate, ProductSubCate, BarCode,");
                sql.Append("InventoryUnit, OrderUnit, InventoryConvOrder, RecipeUnit, RecipeConvInvent, IsRecipe, SaleItem,");
                sql.Append("TaxType, TaxRate, StandardCost, PriceDeviation, QuantityDeviation, IsActive,");
                sql.Append("AccountCode, TaxAccCode, ReqHQAppr, ApprovalLevel,");
                sql.Append("AddField1, AddField2, AddField3, AddField4, AddField5, AddField6, AddField7, AddField8, AddField9, AddField10,");
                sql.Append("CreatedDate, CreatedBy, UpdatedDate, UpdatedBy");
                sql.Append(") VALUES(");

                sql.Append("@ProductCode, @ProductDesc1, @ProductDesc2, @ProductCate, null, @BarCode,");
                sql.Append("@InventoryUnit, @OrderUnit, @InventoryConvOrder, @RecipeUnit, @RecipeConvInvent, @IsRecipe, @SaleItem,");
                sql.Append("@TaxType, @TaxRate, @StandardCost, @PriceDeviation, @QuantityDeviation, @IsActive,");
                sql.Append("@AccountCode, @TaxAccCode, @ReqHQAppr, 0,");
                sql.Append("null, null, null, null, null, null, null, null, null, null,");
                sql.Append("@CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy");
                sql.Append(")");

                var p = new List<SqlParameter>();

                // If not available then get from ItemGroup->Sub-Category->Category

                p.Add(new SqlParameter("ProductCode", productCode));
                p.Add(new SqlParameter("ProductDesc1", productDesc1));
                p.Add(new SqlParameter("ProductDesc2", productDesc2));
                p.Add(new SqlParameter("ProductCate", productCate));
                p.Add(new SqlParameter("BarCode", barcode));

                p.Add(new SqlParameter("InventoryUnit", inventoryUnit));
                p.Add(new SqlParameter("OrderUnit", orderUnit));
                p.Add(new SqlParameter("InventoryConvOrder", inventoryConvOrder));
                p.Add(new SqlParameter("RecipeUnit", recipeUnit));
                p.Add(new SqlParameter("RecipeConvInvent", recipeConvInvent));
                p.Add(new SqlParameter("IsRecipe", isRecipe));
                p.Add(new SqlParameter("SaleItem", saleItem));

                p.Add(new SqlParameter("AccountCode", accountCode));
                p.Add(new SqlParameter("TaxAccCode", taxAccCode));
                p.Add(new SqlParameter("ReqHQAppr", reqHQAppr));

                p.Add(new SqlParameter("TaxType", taxType));
                p.Add(new SqlParameter("TaxRate", taxRate));
                p.Add(new SqlParameter("StandardCost", standardCost));
                p.Add(new SqlParameter("PriceDeviation", priceDeviation));
                p.Add(new SqlParameter("QuantityDeviation", quantityDeviation));
                p.Add(new SqlParameter("IsActive", isActive));


                p.Add(new SqlParameter("CreatedDate", DateTime.Now));
                p.Add(new SqlParameter("CreatedBy", LoginInfo.LoginName));
                p.Add(new SqlParameter("UpdatedDate", DateTime.Now));
                p.Add(new SqlParameter("UpdatedBy", LoginInfo.LoginName));



                query.ExecuteQuery(sql.ToString(), p.ToArray());

                sql.Clear();
                sql.AppendFormat("DELETE FROM [IN].ProdUnit WHERE ProductCode='{0}'" + Environment.NewLine, productCode);
                sql.AppendFormat("INSERT INTO [IN].ProdUnit (ProductCode, OrderUnit, Rate, IsDefault, UnitType) VALUES ('{0}', '{1}', 1, 1, 'I')", productCode, inventoryUnit);
                query.ExecuteQuery(sql.ToString());

                #endregion
            }

            // Order Units
            SaveOrderUnit(productCode);
            SaveRecipeUnit(productCode);

            Response.Redirect(string.Format("Prod2.aspx?BuCode={0}&ID={1}", _BuCode, productCode));
        }

        private void SaveOrderUnit(string productCode)
        {
            var query = new Helpers.SQL(_connStr);
            var sql = "";

            // Delete items
            var delCodes = GetDeletedCodes(dtOrderUnit, "Code");

            if (delCodes.Length > 0)
            {
                var codes = "'" + string.Join("','", delCodes) + "'";

                sql = string.Format("DELETE FROM [IN].ProdUnit WHERE UnitType='O' AND ProductCode='{0}' AND OrderUnit IN ({1}) ", productCode, codes);
                product.DbExecuteQuery(sql, null, _connStr);
            }

            // Added and edited items
            var sbAdd = new StringBuilder();
            var default_UnitCode = txt_OrderUnit.Text;
            var invUnit = _IsEditMode ? txt_InventoryUnit.Text : ddl_InventoryUnit.Value.ToString();

            foreach (DataRow dr in dtOrderUnit.Rows)
            {
                if (dr.RowState == DataRowState.Added)
                {
                    var unitCode = dr["Code"].ToString();
                    var rate = Convert.ToDecimal(dr["Rate"]);
                    var isDefault = Convert.ToBoolean(dr["IsDefault"]) ? 1 : 0;

                    rate = invUnit == unitCode ? 1 : rate;

                    sbAdd.AppendFormat("(N'{0}',N'{1}',{2},{3},'O'),", productCode, unitCode, rate, isDefault);
                }

                if (dr.RowState == DataRowState.Modified)
                {
                    var unitCode = dr["Code"].ToString();
                    var rate = Convert.ToDecimal(dr["Rate"]);
                    var isDefault = Convert.ToBoolean(dr["IsDefault"]) ? 1 : 0;

                    rate = invUnit == unitCode ? 1 : rate;

                    sql = "UPDATE [IN].ProdUnit SET Rate=@Rate, IsDefault=@IsDefault WHERE UnitType='O' AND ProductCode=@ProductCode AND OrderUnit=@OrderUnit";

                    var p = new List<SqlParameter>();

                    p.Add(new SqlParameter("Rate", rate));
                    p.Add(new SqlParameter("isDefault", isDefault));
                    p.Add(new SqlParameter("ProductCode", productCode));
                    p.Add(new SqlParameter("OrderUnit", unitCode));

                    query.ExecuteQuery(sql, p.ToArray());
                }
            }

            if (sbAdd.Length > 0)
            {
                sql = "INSERT INTO [IN].ProdUnit (ProductCode, OrderUnit, Rate, IsDefault, UnitType) VALUES" + sbAdd.ToString().TrimEnd(',');
                query.ExecuteQuery(sql);
            }

            query.ExecuteQuery(string.Format("UPDATE [IN].ProdUnit SET IsDefault=0 WHERE UnitType='O' AND ProductCode='{0}'", productCode));
            query.ExecuteQuery(string.Format("UPDATE [IN].ProdUnit SET IsDefault=1 WHERE UnitType='O' AND ProductCode='{0}' AND OrderUnit='{1}'", productCode, default_UnitCode));
        }

        private void SaveRecipeUnit(string productCode)
        {
            var query = new Helpers.SQL(_connStr);
            var sql = "";

            // Delete items
            var delCodes = GetDeletedCodes(dtRecipeUnit, "Code");

            if (delCodes.Length > 0)
            {
                var codes = "'" + string.Join("','", delCodes) + "'";

                sql = string.Format("DELETE FROM [IN].ProdUnit WHERE UnitType='R' AND ProductCode='{0}' AND OrderUnit IN ({1}) ", productCode, codes);
                product.DbExecuteQuery(sql, null, _connStr);
            }

            // Added and edited items
            var sbAdd = new StringBuilder();
            var default_UnitCode = txt_RecipeUnit.Text;
            var invUnit = _IsEditMode ? txt_InventoryUnit.Text : ddl_InventoryUnit.Value.ToString();

            foreach (DataRow dr in dtRecipeUnit.Rows)
            {
                if (dr.RowState == DataRowState.Added)
                {
                    var unitCode = dr["Code"].ToString();
                    var rate = Convert.ToDecimal(dr["Rate"]);
                    var isDefault = Convert.ToBoolean(dr["IsDefault"]) ? 1 : 0;

                    rate = invUnit == unitCode ? 1 : rate;

                    sbAdd.AppendFormat("(N'{0}',N'{1}',{2},{3},'R'),", productCode, unitCode, rate, isDefault);
                }

                if (dr.RowState == DataRowState.Modified)
                {
                    var unitCode = dr["Code"].ToString();
                    var rate = Convert.ToDecimal(dr["Rate"]);
                    var isDefault = Convert.ToBoolean(dr["IsDefault"]) ? 1 : 0;

                    rate = invUnit == unitCode ? 1 : rate;

                    sql = "UPDATE [IN].ProdUnit SET Rate=@Rate, IsDefault=@IsDefault WHERE UnitType='R' AND ProductCode=@ProductCode AND OrderUnit=@OrderUnit";

                    var p = new List<SqlParameter>();

                    p.Add(new SqlParameter("Rate", rate));
                    p.Add(new SqlParameter("isDefault", isDefault));
                    p.Add(new SqlParameter("ProductCode", productCode));
                    p.Add(new SqlParameter("OrderUnit", unitCode));

                    query.ExecuteQuery(sql, p.ToArray());
                }
            }

            if (sbAdd.Length > 0)
            {
                sql = "INSERT INTO [IN].ProdUnit (ProductCode, OrderUnit, Rate, IsDefault, UnitType) VALUES" + sbAdd.ToString().TrimEnd(',');
                query.ExecuteQuery(sql);
            }

            query.ExecuteQuery(string.Format("UPDATE [IN].ProdUnit SET IsDefault=0 WHERE UnitType='R' AND ProductCode='{0}'", productCode));
            query.ExecuteQuery(string.Format("UPDATE [IN].ProdUnit SET IsDefault=1 WHERE UnitType='R' AND ProductCode='{0}' AND OrderUnit='{1}'", productCode, default_UnitCode));
        }

        private void SetDeviation(string itemGroupCode, ASPxSpinEdit priceEdit, ASPxSpinEdit qtyEdit)
        {
            var sql = @"DECLARE @ItemGroupCode nvarchar(20) ='{0}'

                    DECLARE @ParentCode nvarchar(20), @price decimal, @qty decimal

                    -- Item Group
                    SELECT @ParentCode=ParentNo, @Price=PriceDeviation, @Qty=QuantityDeviation FROM [IN].ProductCategory WHERE CategoryCode=@ItemGroupCode

                    -- Sub-Category
                    IF @price IS NULL OR @qty IS NULL
                    BEGIN
	                    DECLARE @price1 decimal, @qty1 decimal
	                    SELECT @ParentCode=ParentNo, @Price1=PriceDeviation, @Qty1=QuantityDeviation FROM [IN].ProductCategory WHERE CategoryCode=@ParentCode

	                    SET @price = CASE WHEN @price IS NULL THEN @price1 ELSE @price END
	                    SET @qty = CASE WHEN @qty IS NULL THEN @qty1 ELSE @qty END

	                    -- Category
	                    IF @price IS NULL OR @qty IS NULL
	                    BEGIN
		                    SELECT @ParentCode=ParentNo, @Price1=PriceDeviation, @Qty1=QuantityDeviation FROM [IN].ProductCategory WHERE CategoryCode=@ParentCode
	
		                    SET @price = CASE WHEN @price IS NULL THEN @price1 ELSE @price END
		                    SET @qty = CASE WHEN @qty IS NULL THEN @qty1 ELSE @qty END
	                    END
                    END

                    SELECT ISNULL(@price, 0) AS Price, ISNULL(@qty, 0) as Qty";


            if (!string.IsNullOrEmpty(itemGroupCode))
            {
                sql = string.Format(sql, itemGroupCode);

                var dt = product.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

                var price = 0m;
                var qty = 0m;

                if (IsValidDb(dt))
                {
                    price = Convert.ToDecimal(dt.Rows[0]["Price"]);
                    qty = Convert.ToDecimal(dt.Rows[0]["Qty"]);
                }

                priceEdit.Value = price;
                qtyEdit.Value = qty;
            }

        }

        private void SetDefault_OrderUnit(string code, decimal rate)
        {
            var iUnit = txt_InventoryUnit.Text;

            txt_OrderUnit.Text = code;
            hf_InventoryConvOrder.Value = rate.ToString();

            lbl_OrderUnitDesc.Text = string.Format("1 {0} = {1} {2}", code, FormatQty(rate), iUnit);

        }

        private void SetDefault_RecipeUnit(string code, decimal rate)
        {
            var iUnit = txt_InventoryUnit.Text;

            txt_RecipeUnit.Text = code;
            lbl_RecipeUnitDesc.Text = string.IsNullOrEmpty(code) ? "" : string.Format("1 {0} = {1} {2}", iUnit, FormatQty(rate), code);
            hf_RecipeConvInvent.Value = rate.ToString();
        }

        private DataTable GetProductUnit(string unitType = null, string productCode = null)
        {
            var sql =
                    @"SELECT 
	                    pu.OrderUnit as [Code], 
	                    u.[Name],
	                    pu.Rate, 
	                    pu.IsDefault, 
	                    CAST(0 AS BIT) IsDeleted, 
	                    CAST(0 AS BIT) as IsChanged 
                    FROM 
	                    [IN].ProdUnit pu
	                    LEFT JOIN [IN].Unit u
		                    ON u.UnitCode = pu.OrderUnit
                    WHERE 
	                    pu.UnitType=@UnitType 
	                    AND pu.ProductCode=@ProductCode
                    ORDER BY 
	                    pu.OrderUnit";

            var p = new List<Blue.DAL.DbParameter>();

            unitType = string.IsNullOrEmpty(unitType) ? "" : unitType;
            productCode = string.IsNullOrEmpty(productCode) ? "" : productCode;

            p.Add(new Blue.DAL.DbParameter("@UnitType", unitType));
            p.Add(new Blue.DAL.DbParameter("@ProductCode", productCode));

            return product.DbExecuteQuery(sql, p.ToArray(), _connStr);

        }

        private string[] GetCurrentCodes(DataTable dataTable, string codeFieldName)
        {
            var result = new List<string>();

            foreach (DataRow dr in dataTable.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                    result.Add(dr[codeFieldName].ToString());
            }

            return result.ToArray();
        }

        private string[] GetDeletedCodes(DataTable dataTable, string codeFiledName)
        {
            DataRow[] dataRows = dataTable.Select(null, null, DataViewRowState.Deleted);

            var result = new List<string>();

            foreach (DataRow row in dataRows)
            {
                result.Add(row[codeFiledName, DataRowVersion.Original].ToString());
            }

            return result.ToArray();
        }


        // function(s)
        #region --Function(s)--
        private DataTable GetCategories()
        {
            var sql = "SELECT CategoryCode as [Code], CategoryName as [Name] FROM [IN].ProductCategory WHERE LevelNo=1 ORDER BY CategoryCode";

            return product.DbExecuteQuery(sql, null, _connStr);
        }

        private DataTable GetSubCategories(string categoryCode)
        {
            var sql = string.Format("SELECT CategoryCode as [Code], CategoryName as [Name] FROM [IN].ProductCategory WHERE LevelNo=2 AND ParentNo='{0}' ORDER BY CategoryCode", categoryCode);

            return product.DbExecuteQuery(sql, null, _connStr);

        }

        private DataTable GetItemGroups(string subCategoryCode)
        {
            var sql = string.Format("SELECT CategoryCode as [Code], CategoryName as [Name] FROM [IN].ProductCategory WHERE LevelNo=3 AND ParentNo='{0}' ORDER BY CategoryCode", subCategoryCode);

            return product.DbExecuteQuery(sql, null, _connStr);
        }

        //private decimal GetLastCost(string productCode)
        //{

        //    var sql = string.Format(@"SELECT [IN].GetLastCost('{0}', NULL)", productCode);
        //    var dt = product.DbExecuteQuery(sql, null, _connStr);

        //    return dt == null || dt.Rows.Count == 0 ? 0m : Convert.ToDecimal(dt.Rows[0][0]);
        //}

//        private LastReceive GetLastReceive(string productCode)
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
//	                                    [Type] IN ('RC','SI') 
//	                                    AND ProductCode='{0}' 
//                                    ORDER BY CommittedDate DESC", productCode);
//            var dt = product.DbExecuteQuery(sql, null, _connStr);

//            if (dt != null && dt.Rows.Count > 0)
//            {
//                var dr = dt.Rows[0];

//                var docType = dr["DocType"].ToString();

//                return new LastReceive
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

        private ProductCategory GetProductCategory(string itemgroupCode)
        {
            var sql = string.Format("SELECT TOP(1) * FROM [IN].vProdCatList WHERE ItemGroupCode='{0}'", itemgroupCode);
            var dt = product.DbExecuteQuery(sql, null, _connStr);

            if (dt != null && dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];

                return new ProductCategory
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

        private DataTable GetUnits(string[] excludeCodes = null)
        {
            var sql = "";

            if (excludeCodes == null || excludeCodes.Length == 0)
                sql = "SELECT UnitCode as [Code], [Name], CONCAT(UnitCode,' : ', [Name]) as CodeName FROM [IN].Unit WHERE IsActived=1 ORDER BY UnitCode";
            else
            {
                var codes = "'" + string.Join("','", excludeCodes) + "'"; ;
                sql = string.Format("SELECT UnitCode as [Code], [Name], CONCAT(UnitCode,' : ', [Name]) as CodeName FROM [IN].Unit WHERE IsActived=1 AND UnitCode NOT IN ({0}) ORDER BY UnitCode", codes);
            }

            return product.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
        }

        private string IsAbleToDeleteOrderUnit(string productCode, string unitCode)
        {
            var dt = new DataTable();

            var sql = string.Format("SELECT Top(1) PrDtNo FROM PC.Pr JOIN PC.Prdt ON pr.PrNo=prdt.PrNo WHERE DocStatus='In Process' AND ProductCode='{0}' AND OrderUnit='{1}'", productCode, unitCode);
            dt = product.DbExecuteQuery(sql, null, _connStr);

            if (dt != null && dt.Rows.Count > 0)
                return "Purchase Request";

            sql = string.Format("SELECT TOP(1) PoDt FROM PC.Po JOIN PC.PoDt ON po.PoNo=podt.PoNo WHERE po.DocStatus NOT IN ('Closed','Completed')  AND Product='{0}'  AND podt.Unit='{1}'", productCode, unitCode);
            dt = product.DbExecuteQuery(sql, null, _connStr);

            if (dt != null && dt.Rows.Count > 0)
                return "Purchase Order";

            sql = string.Format("SELECT TOP(1) RecDtNo FROM PC.REC JOIN PC.RECDt ON rec.RecNo=recdt.RecNo WHERE DocStatus NOT IN ('Committed','Voided') AND ProductCode='{0}' AND recdt.UnitCode='{1}'", productCode, unitCode);
            dt = product.DbExecuteQuery(sql, null, _connStr);
            if (dt != null && dt.Rows.Count > 0)
                return "Receiving";
            else
                return string.Empty;


        }
        private string IsAbleToDeleteRecipeUnit(string productCode, string unitCode)
        {
            var dt = new DataTable();

            var sql = string.Format("SELECT TOP (1) RcpDtID FROM PT.RcpDt WHERE IngredientType='P' AND IngredientCode='{0}' AND Unit='{1}'", productCode, unitCode);
            dt = product.DbExecuteQuery(sql, null, _connStr);

            if (dt != null && dt.Rows.Count > 0)
                return "Recipe";
            else
                return string.Empty;

        }

        private IEnumerable<Document> GetActiveDocuments(string productCode)
        {
            var dt = new DataTable();
            var sql =
@"
-- PR
SELECT
	'Purchase Request' as DocType,
	h.PrNo as DocNo,
	h.PRDate as DocDate,
	h.[Description],
	h.DocStatus
FROM
	PC.Pr h
	JOIN PC.PrDt d
		ON h.PrNo=d.PrNo
WHERE
	h.DocStatus NOT IN ('Complete', 'Completed','Voided')
	AND d.ProductCode = @ProductCode

UNION ALL
SELECT
	'Purchase Order' as DocType,
	h.PoNo as DocNo,
	h.PoDate as DocDate,
	h.[Description],
	h.DocStatus
FROM
	PC.Po h
	JOIN PC.PoDt d
		ON h.PoNo=d.PoNo
WHERE
	h.DocStatus NOT IN ('Complete','Completed','Voided','Closed')
	AND d.Product = @ProductCode

UNION ALL
SELECT
	'Receiving' as DocType,
	h.RecNo as DocNo,
	h.RecDate as DocDate,
	h.[Description],
	h.DocStatus
FROM
	PC.REC h
	JOIN PC.RECDt d
		ON h.RecNo=d.RecNo
WHERE
	h.DocStatus NOT IN ('Committed', 'Voided')
	AND d.ProductCode = @ProductCode

UNION ALL
SELECT
	'Credit Note' as DocType,
	h.CnNo as DocNo,
	h.CnDate as DocDate,
	h.[Description],
	h.DocStatus
FROM
	PC.Cn h
	JOIN PC.CnDt d
		ON h.CnNo=d.CnNo
WHERE
	h.DocStatus NOT IN ('Committed', 'Voided')
	AND d.ProductCode = @ProductCode

UNION ALL
SELECT
	'Stock In' as DocType,
	h.RefId as DocNo,
	CAST(h.CreateDate as DATE) as DocDate,
	h.[Description],
	h.[Status]
FROM
	[IN].StockIn h
	JOIN [In].StockInDt d
		ON h.RefId = d.Id
WHERE
	h.[Status] NOT IN ('Committed', 'Voided')
	AND d.SKU = @ProductCode

UNION ALL
SELECT
	'Stock Out' as DocType,
	h.RefId as DocNo,
	CAST(h.CreateDate as DATE) as DocDate,
	h.[Description],
	h.[Status]
FROM
	[IN].StockOut h
	JOIN [In].StockOutDt d
		ON h.RefId = d.Id
WHERE
	h.[Status] NOT IN ('Committed', 'Voided')
	AND d.SKU = @ProductCode

UNION ALL
SELECT
	'Store Requisition' as DocType,
	h.RequestCode as DocNo,
	CAST(h.DeliveryDate as DATE) as DocDate,
	h.[Description],
	h.[DocStatus]
FROM
	[IN].StoreRequisition h
	JOIN [In].StoreRequisitionDetail d
		ON h.RefId = d.DocumentId
WHERE
	h.[DocStatus] NOT IN ('Committed','Complete', 'Completed', 'Voided')
	AND d.ProductCode = @ProductCode


UNION ALL
SELECT
	'Closing Balance' as DocType,
	'Store :' + h.StoreId as DocNo,
	CAST(h.[Date] as DATE) as DocDate,
	h.[Description],
	h.[Status]
FROM
	[IN].Eop h
	JOIN [In].EopDt d
		ON h.EopId=d.EopId
WHERE
	h.[Status] NOT IN ('Committed', 'Voided')
	AND d.ProductCode = @ProductCode
";
            var p = new Blue.DAL.DbParameter[1]
            {
                new Blue.DAL.DbParameter("ProductCode", productCode)
            };

            dt = product.DbExecuteQuery(sql, p, _connStr);

            if (dt != null && dt.Rows.Count > 0)
            {
                var result = dt.AsEnumerable()
                    .Select(x => new Document
                    {
                        DocType = x.Field<string>("DocType"),
                        DocNo = x.Field<string>("DocNo"),
                        DocDate = x.Field<DateTime>("DocDate"),
                        DocStatus = x.Field<string>("DocStatus"),
                        Description = x.Field<string>("Description")
                    })
                    .ToList();
                return result;
            }
            else
                return null;
        }

        private IEnumerable<LocationOnhand> GetOnhandByLocation(string productCode)
        {
            var dt = new DataTable();
            var sql =
@";WITH
p AS(
	SELECT
		i.[Location] as LocationCode,
		l.LocationName,
		SUM([IN]-[OUT]) as Qty
	FROM
		[IN].Inventory i
		JOIN [IN].StoreLocation l
			ON i.Location=l.LocationCode AND l.EOP <> 2
	WHERE
		i.ProductCode=@ProductCode
	GROUP BY
		i.[Location],
		l.LocationName
)
SELECT
	*
FROM
	p
WHERE
	Qty > 0";

            var p = new Blue.DAL.DbParameter[1]
            {
                new Blue.DAL.DbParameter("ProductCode", productCode)
            };

            dt = product.DbExecuteQuery(sql, p, _connStr);

            if (dt != null && dt.Rows.Count > 0)
            {
                var result = dt.AsEnumerable()
                    .Select(x => new LocationOnhand
                    {
                        LocationCode = x.Field<string>("LocationCode"),
                        LocationName = x.Field<string>("LocationName"),
                        Qty = x.Field<decimal>("Qty")
                    })
                    .ToList();
                return result;
            }
            else
                return null;
        }



        private void DeleteRow(DataTable dataTable, string keyField, string value)
        {
            foreach (DataRow dr in dataTable.Rows)
            {
                if (dr.RowState != DataRowState.Deleted && dr[keyField].ToString() == value)
                {
                    dr.Delete();

                    break;
                }
            }

        }
        // -------------------------------



        public string FormatAmt(decimal value)
        {
            return string.Format("{0:N" + DefaultAmtDigit.ToString() + "}", value);
        }

        public string FormatQty(decimal value)
        {
            return string.Format("{0:N" + DefaultQtyDigit.ToString() + "}", value);
        }

        #endregion
    }

    internal class Deviation
    {
        public decimal Price { get; set; }
        public decimal Qty { get; set; }
    }

    public class LastReceive
    {
        public string DocNo { get; set; }
        public string DocType { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public decimal Cost { get; set; }
        public DateTime CommittedDate { get; set; }
    }

    public class ProductCategory
    {
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryCode { get; set; }
        public string SubCategoryName { get; set; }
        public string ItemGroupCode { get; set; }
        public string ItemGroupName { get; set; }
    }

    public class Document
    {
        public string DocType { get; set; }
        public string DocNo { get; set; }
        public DateTime DocDate { get; set; }
        public string Description { get; set; }
        public string DocStatus { get; set; }
    }

    public class LocationOnhand
    {
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public decimal Qty { get; set; }
    }

}