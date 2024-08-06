using System;
using System.Data;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Inventory
{
    public partial class ProdEdit3 : BasePage
    {
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();

        private readonly Blue.BL.Option.Inventory.ProdCateType prodCateType =
            new Blue.BL.Option.Inventory.ProdCateType();

        private readonly Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.APP.TAX tax = new Blue.BL.APP.TAX();
        private readonly Blue.BL.Option.Inventory.Unit unit = new Blue.BL.Option.Inventory.Unit();

        private DataSet dsProductEdit = new DataSet();

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_ConnStr.Value = bu.GetConnectionString(Request.Params["BuCode"]);
        }

        /// <summary>
        ///     Gets and display season data when page load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
                Page_Setting();
            }
            else
            {
                dsProductEdit = (DataSet) Session["dsProductEdit"];
                //dsUnit        = (DataSet)Session["dsUnit"];
            }

            //ASPxWebControl.RegisterBaseScript(Page);
        }

        /// <summary>
        ///     Gets season data.
        /// </summary>
        private void Page_Retrieve()
        {
            dsProductEdit.Clear();

            if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                // Get exist product
                product.GetList(dsProductEdit, Request.Params["ID"], hf_ConnStr.Value);

                // Get exist product unit
                prodUnit.GetList(dsProductEdit, Request.Params["ID"], hf_ConnStr.Value);
            }
            else
            {
                // Get product structure
                product.GetStructure(dsProductEdit, hf_ConnStr.Value);

                // Add new blank data to product
                var drProductNew = dsProductEdit.Tables[product.TableName].NewRow();
                drProductNew["ProductCode"] = string.Empty;
                drProductNew["ProductDesc1"] = string.Empty;
                drProductNew["QuantityDeviation"] = decimal.Zero;
                drProductNew["PriceDeviation"] = decimal.Zero;
                drProductNew["IsActive"] = true;
                drProductNew["StandardCost"] = decimal.Zero;
                drProductNew["CreatedDate"] = ServerDateTime;
                drProductNew["CreatedBy"] = LoginInfo.LoginName;
                drProductNew["UpdatedDate"] = ServerDateTime;
                drProductNew["UpdatedBy"] = LoginInfo.LoginName;

                dsProductEdit.Tables[product.TableName].Rows.Add(drProductNew);

                // Get product unit structure
                prodUnit.GetSchema(dsProductEdit, hf_ConnStr.Value);
            }

            Session["dsProductEdit"] = dsProductEdit;
        }

        /// <summary>
        ///     Display season data.
        /// </summary>
        private void Page_Setting()
        {
            var drProduct = dsProductEdit.Tables[product.TableName].Rows[0];

            var categoryCode = prodCat.GetCategoryCode(drProduct["ProductCate"].ToString(), hf_ConnStr.Value);
                // Get Catgory Code from ItemGroup Code
            var subCategoryCode = prodCat.GetSubCategoryCode(drProduct["ProductCate"].ToString(), hf_ConnStr.Value);
                // Get SubCategory Code from ItemGroup Code

            // Binding Category DropDownList
            BindingCategory(categoryCode);

            // Binding Sub Category DropDownList
            BindingSubCategory(categoryCode, subCategoryCode);

            // Binding Item Group DropDownList
            BindingItemGroup(subCategoryCode, drProduct["ProductCate"].ToString());

            txt_Code.Text = drProduct["ProductCode"].ToString();
            txt_Description1.Text = drProduct["ProductDesc1"].ToString();
            txt_Descritpion2.Text = drProduct["ProductDesc2"].ToString();
            chk_IsRecipe.Checked = (drProduct["IsRecipe"].ToString() != string.Empty
                ? bool.Parse(drProduct["IsRecipe"].ToString())
                : false);

            // Binding TAX
            BindingTAX(drProduct["TaxType"].ToString());

            if (drProduct["TaxRate"] != DBNull.Value)
            {
                txt_TaxRate.Text = decimal.Parse(drProduct["TaxRate"].ToString()).ToString("0.00");
                    // TODO: Number format should take from setting
            }
            else
            {
                txt_TaxRate.Text = decimal.Zero.ToString("0.00"); // TODO: Number format should take from setting
            }

            txt_TaxAccCode.Text = drProduct["TaxAccCode"].ToString();

            // Binding Product Type
            BindingProductType(drProduct["ApprovalLevel"].ToString());

            txt_QuantityDeviation.Text = decimal.Parse(drProduct["QuantityDeviation"].ToString()).ToString("0.00");
                // TODO: Number format should take from setting
            txt_PriceDeviation.Text = decimal.Parse(drProduct["PriceDeviation"].ToString()).ToString("0.00");
                // TODO: Number format should take from setting

            if (drProduct["IsActive"] != DBNull.Value)
            {
                chk_Status.Checked = bool.Parse(drProduct["IsActive"].ToString());
            }

            if (drProduct["SaleItem"] != DBNull.Value)
            {
                chk_SaleItem.Checked = bool.Parse(drProduct["SaleItem"].ToString());
            }

            if (drProduct["ReqHQAppr"] != DBNull.Value)
            {
                chk_ReqHQAppr.Checked = bool.Parse(drProduct["ReqHQAppr"].ToString());
            }

            txt_BarCode.Text = drProduct["BarCode"].ToString();
            txt_StandardCost.Text = decimal.Parse(drProduct["StandardCost"].ToString()).ToString("0.00");

            // Take from last receiving not include VAT            
            txt_LastCost.Text =
                product.GetLastPrice(drProduct["ProductCode"].ToString(), hf_ConnStr.Value).ToString("0.00");
                // TODO: Number format should take from setting

            // Binding Inventory Unit
            BindingInventoryUnit(drProduct["InventoryUnit"].ToString());

            txt_OrderUnit.Text = drProduct["OrderUnit"].ToString();
            txt_InvenConvOrder.Text = drProduct["InventoryConvOrder"].ToString();
            txt_RecipeUnit.Text = drProduct["RecipeUnit"].ToString();
            txt_RecipeConverseRate.Text = drProduct["RecipeConvInvent"].ToString();
        }

        private void BindingCategory(string categryCode)
        {
            ddl_Category.DataSource = prodCat.GetList(0, hf_ConnStr.Value);
            ddl_Category.DataTextField = "CategoryName";
            ddl_Category.DataValueField = "CategoryCode";
            ddl_Category.Enabled = (Request.Params["MODE"].ToUpper() == "EDIT" ? false : true);
            ddl_Category.DataBind();
            ddl_Category.Items.Insert(0, new ListItem("", ""));
            ddl_Category.SelectedValue = categryCode;
        }

        protected void ddl_Category_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            // Rebinding SubCategory and ItemGroup DropDownList
            BindingSubCategory(ddl_Category.SelectedItem.Value, string.Empty);
            BindingItemGroup(ddl_SubCategory.SelectedItem.Value, string.Empty);

            // Default Category Type of Product
        }

        private void BindingSubCategory(string categoryCode, string subCategoryCode)
        {
            ddl_SubCategory.Items.Clear();

            if (categoryCode != string.Empty)
            {
                ddl_SubCategory.DataSource = prodCat.GetList(int.Parse(categoryCode), hf_ConnStr.Value);
                ddl_SubCategory.DataTextField = "CategoryName";
                ddl_SubCategory.DataValueField = "CategoryCode";
                ddl_SubCategory.Enabled = (Request.Params["MODE"].ToUpper() == "EDIT" ? false : true);
                ddl_SubCategory.DataBind();
            }

            ddl_SubCategory.Items.Insert(0, new ListItem("", ""));
            ddl_SubCategory.SelectedValue = subCategoryCode;
        }

        protected void ddl_SubCategory_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            // Rebinding ItemGroup DropDownList
            BindingItemGroup(ddl_SubCategory.SelectedItem.Value, string.Empty);

            // Default Category Type of Product
        }

        private void BindingItemGroup(string subCategoryCode, string itemGroupCode)
        {
            ddl_ItemGroup.Items.Clear();

            if (subCategoryCode != string.Empty)
            {
                ddl_ItemGroup.DataSource = prodCat.GetList(int.Parse(subCategoryCode), hf_ConnStr.Value);
                ddl_ItemGroup.DataTextField = "CategoryName";
                ddl_ItemGroup.DataValueField = "CategoryCode";
                ddl_ItemGroup.Enabled = (Request.Params["MODE"].ToUpper() == "EDIT" ? false : true);
                ddl_ItemGroup.DataBind();
            }

            ddl_ItemGroup.Items.Insert(0, new ListItem("", ""));
            ddl_ItemGroup.SelectedValue = itemGroupCode;
        }

        protected void ddl_ItemGroup_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            // Rebinding Product Code when Autogenerate
            txt_Code.Text = string.Empty;

            // Assign TaxAccount Code from Selected ItemGroup
            txt_TaxAccCode.Text =
                prodCat.ProdCat_GetTaxAccCodeByCategoryCode(Convert.ToInt32(ddl_ItemGroup.SelectedItem.Value),
                    hf_ConnStr.Value);

            // Default Product Type from Selected ItemGroup
            ddl_ProdType.SelectedValue =
                prodCat.GetCategoryType(ddl_ItemGroup.SelectedItem.Value, hf_ConnStr.Value).ToString();
        }

        private void BindingTAX(string taxCode)
        {
            ddl_TAXType.DataSource = tax.GetActiveList(hf_ConnStr.Value);
            ddl_TAXType.DataTextField = "Name";
            ddl_TAXType.DataValueField = "Code";
            ddl_TAXType.DataBind();
            ddl_TAXType.Items.Insert(0, new ListItem("", ""));
            ddl_TAXType.SelectedValue = taxCode;
        }

        protected void ddl_TAXType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Default Tax Rate from selected TaxType
            txt_TaxRate.Text = tax.GetRate(ddl_TAXType.SelectedItem.Value, hf_ConnStr.Value).ToString("0.00");
        }

        private void BindingProductType(string productType)
        {
            ddl_ProdType.DataSource = prodCateType.GetList(hf_ConnStr.Value);
            ddl_ProdType.DataTextField = "Description";
            ddl_ProdType.DataValueField = "Code";
            ddl_ProdType.DataBind();
            ddl_ProdType.Items.Insert(0, new ListItem("", ""));
            ddl_ProdType.SelectedValue = productType;
        }

        private void BindingInventoryUnit(string inventoryUnitCode)
        {
            ddl_InventoryUnit.DataSource = unit.GetUnitLookup(hf_ConnStr.Value);
            ddl_InventoryUnit.DataTextField = "UnitCode";
            ddl_InventoryUnit.DataValueField = "UnitCode";
            ddl_InventoryUnit.Enabled = (Request.Params["MODE"].ToUpper() == "NEW" ? true : false);
            ddl_InventoryUnit.DataBind();
            ddl_InventoryUnit.Items.Insert(0, new ListItem("", ""));
            ddl_InventoryUnit.SelectedValue = inventoryUnitCode;
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":

                    Page.Validate();

                    if (Page.IsValid)
                    {
                        var result = Save();

                        if (result)
                        {
                            Response.Redirect("Prod3.aspx?BuCode=" + Request.Params["BuCode"] +
                                              "&ID=" + dsProductEdit.Tables[product.TableName].Rows[0]["ProductCode"]);
                        }
                    }

                    break;

                case "BACK":

                    if (Request.Params["MODE"].ToUpper() == "EDIT")
                    {
                        Response.Redirect("Prod3.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" +
                                          Request.Params["ID"]);
                    }
                    else if (Request.Params["MODE"].ToUpper() == "NEW")
                    {
                        Response.Redirect("ProdList.aspx");
                    }

                    break;
            }
        }

        private bool Save()
        {
            var drProduct = dsProductEdit.Tables[product.TableName].Rows[0];

            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                // TODO: How generate product code if not use ItemGroup. How to manual assign product code.
                drProduct["ProductCode"] = product.GetGenerateProductCode(ddl_ItemGroup.SelectedItem.Value,
                    hf_ConnStr.Value);
                drProduct["InventoryUnit"] = ddl_InventoryUnit.SelectedItem.Value;
                drProduct["OrderUnit"] = ddl_InventoryUnit.SelectedItem.Value;
                drProduct["InventoryConvOrder"] = decimal.One;
                drProduct["RecipeUnit"] = ddl_InventoryUnit.SelectedItem.Value;
                drProduct["RecipeConvInvent"] = decimal.One;
                drProduct["CreatedDate"] = ServerDateTime;
                drProduct["CreatedBy"] = LoginInfo.LoginName;

                // Default Inventory Unit and Order Unit in [IN].ProdUnit
                // Inventory Unit
                var drProdUnitInventory = dsProductEdit.Tables[prodUnit.TableName].NewRow();
                drProdUnitInventory["ProductCode"] = drProduct["ProductCode"].ToString();
                drProdUnitInventory["OrderUnit"] = drProduct["InventoryUnit"].ToString();
                drProdUnitInventory["Rate"] = decimal.One;
                drProdUnitInventory["IsDefault"] = true;
                drProdUnitInventory["UnitType"] = "I";
                dsProductEdit.Tables[prodUnit.TableName].Rows.Add(drProdUnitInventory);

                // Order Unit 
                var drProdUnitOrder = dsProductEdit.Tables[prodUnit.TableName].NewRow();
                drProdUnitOrder["ProductCode"] = drProduct["ProductCode"].ToString();
                drProdUnitOrder["OrderUnit"] = drProduct["OrderUnit"].ToString();
                drProdUnitOrder["Rate"] = decimal.Parse(drProduct["InventoryConvOrder"].ToString());
                drProdUnitOrder["IsDefault"] = true;
                drProdUnitOrder["UnitType"] = "O";
                dsProductEdit.Tables[prodUnit.TableName].Rows.Add(drProdUnitOrder);

                // Recipe Unit 
                var drProdUnitRecipe = dsProductEdit.Tables[prodUnit.TableName].NewRow();
                drProdUnitRecipe["ProductCode"] = drProduct["ProductCode"].ToString();
                drProdUnitRecipe["OrderUnit"] = drProduct["RecipeUnit"].ToString();
                drProdUnitRecipe["Rate"] = decimal.Parse(drProduct["RecipeConvInvent"].ToString());
                drProdUnitRecipe["IsDefault"] = true;
                drProdUnitRecipe["UnitType"] = "R";
                dsProductEdit.Tables[prodUnit.TableName].Rows.Add(drProdUnitRecipe);
            }

            drProduct["ProductCate"] = ddl_ItemGroup.SelectedItem.Value;
            drProduct["ProductDesc1"] = txt_Description1.Text.Trim();
            drProduct["ProductDesc2"] = txt_Descritpion2.Text.Trim();
            drProduct["IsRecipe"] = chk_IsRecipe.Checked;
            drProduct["TaxType"] = ddl_TAXType.SelectedItem.Value;
            drProduct["TaxRate"] = decimal.Parse(txt_TaxRate.Text);
            drProduct["TaxAccCode"] = txt_TaxAccCode.Text.Trim();
            drProduct["ApprovalLevel"] = ddl_ProdType.SelectedItem.Value;
            drProduct["QuantityDeviation"] = decimal.Parse(txt_QuantityDeviation.Text.Trim());
            drProduct["PriceDeviation"] = decimal.Parse(txt_PriceDeviation.Text.Trim());
            drProduct["IsActive"] = chk_Status.Checked;
            drProduct["SaleItem"] = chk_SaleItem.Checked;
            drProduct["ReqHQAppr"] = chk_ReqHQAppr.Checked;
            drProduct["BarCode"] = txt_BarCode.Text.Trim();
            drProduct["StandardCost"] = decimal.Parse(txt_StandardCost.Text.Trim());
            drProduct["LastCost"] = decimal.Parse(txt_LastCost.Text.Trim());
            drProduct["UpdatedDate"] = ServerDateTime;
            drProduct["UpdatedBy"] = LoginInfo.LoginName;

            return product.SaveProductAndProdUnit(dsProductEdit, hf_ConnStr.Value);
        }
    }
}