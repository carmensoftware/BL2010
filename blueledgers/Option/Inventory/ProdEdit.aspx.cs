using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxClasses;

namespace BlueLedger.PL.Option.Inventory
{
    public partial class ProdEdit : BasePage
    {
        private readonly Blue.BL.Option.Inventory.ApprLv apprLv = new Blue.BL.Option.Inventory.ApprLv();
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private readonly Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.Option.Inventory.Unit unit = new Blue.BL.Option.Inventory.Unit();
        private Blue.BL.GL.Account.Account account = new Blue.BL.GL.Account.Account();

        private DataSet dsProductEdit = new DataSet();
        private DataSet dsUnit = new DataSet();
        private DataTable dtItemGroup = new DataTable();
        //private DataSet     dsProdUnit      = new DataSet();
        private string msgError = string.Empty;


        /// <summary>
        ///     Gets and display season data when page load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            hf_ConnStr.Value = bu.GetConnectionString(LoginInfo.BuInfo.BuCode);

            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
                Page_Setting();
            }
            else
            {
                dsProductEdit = (DataSet)Session["dsProductEdit"];
                dsUnit = (DataSet)Session["dsUnit"];
                //dsProdUnit = (DataSet)Session["dsProdUnit"];
            }

            ASPxWebControl.RegisterBaseScript(Page);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            cmb_OrderUnit.ValueField = "UnitCode";
            cmb_OrderUnit.TextField = "UnitCode";
            cmb_OrderUnit.TextFormatString = "{0}";
            cmb_OrderUnit.DataSource = unit.GetUnitLookup(bu.GetConnectionString(Request.Params["BuCode"]));
            cmb_OrderUnit.DataBind();


            cmb_InventoryUnit.ValueField = "UnitCode";
            cmb_InventoryUnit.TextField = "UnitCode";
            cmb_InventoryUnit.TextFormatString = "{0}";
            cmb_InventoryUnit.DataSource = unit.GetUnitLookup(bu.GetConnectionString(Request.Params["BuCode"]));
            cmb_InventoryUnit.DataBind();

            // Display Product list
            ddl_Product.DataSource = product.GetList(bu.GetConnectionString(Request.Params["BuCode"]));
            ddl_Product.DataTextField = "ProductDesc1";
            ddl_Product.DataValueField = "ProductCode";
            ddl_Product.DataBind();
            //ddl_Product.SelectedValue = this.ProductCode; 

            //cmb_RecipeUnit.ValueField          = "UnitCode";
            //cmb_RecipeUnit.TextField           = "UnitCode";
            //cmb_RecipeUnit.TextFormatString    = "{0}";
            //cmb_RecipeUnit.DataSource          = unit.GetUnitLookup(LoginInfo.ConnStr);
            //cmb_RecipeUnit.DataBind();
        }

        /// <summary>
        ///     Gets season data.
        /// </summary>
        private void Page_Retrieve()
        {
            if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                if (dsProductEdit != null)
                {
                    dsProductEdit.Clear();
                }

                // Get list by productcode.
                product.GetList(dsProductEdit, Request.Params["ID"], bu.GetConnectionString(Request.Params["BuCode"]));

                var getProdUnit = prodUnit.GetList(dsProductEdit, Request.Params["ID"],
                    bu.GetConnectionString(Request.Params["BuCode"]));

                if (getProdUnit)
                {
                    ddl_Product.SelectedValue = Request.Params["ID"];

                    // Show Display ProdUnit
                    grd_ProdUnit.DataSource = dsProductEdit.Tables[prodUnit.TableName];
                    grd_ProdUnit.DataBind();
                }

                // Display all unit
                if (dsProductEdit.Tables["UnitList"] != null)
                {
                    dsProductEdit.Tables["UnitList"].Clear();
                }

                var getUnitList = prodUnit.GetUnitList(dsProductEdit, Request.Params["ID"],
                    bu.GetConnectionString(Request.Params["BuCode"]));

                if (getUnitList)
                {
                    grd_UnitList.DataSource = dsProductEdit.Tables["UnitList"];
                    grd_UnitList.DataBind();
                }

                //Session["dsProdUnit"] = dsProdUnit;
                Session["dsProductEdit"] = dsProductEdit;
            }
            else
            {
                var getProduct = product.GetStructure(dsProductEdit, bu.GetConnectionString(Request.Params["BuCode"]));

                if (!getProduct)
                {
                    return;
                }

                //Get Schema 
                prodUnit.GetSchema(dsProductEdit, bu.GetConnectionString(Request.Params["BuCode"]));

                //Get Product 
                var dtTmp = product.GetList(bu.GetConnectionString(Request.Params["BuCode"]));

                var get = prodUnit.GetUnitList(dsProductEdit, dtTmp.Rows[0]["ProductCode"].ToString(),
                    bu.GetConnectionString(Request.Params["BuCode"]));

                if (get)
                {
                    grd_UnitList.DataSource = dsProductEdit.Tables["UnitList"];
                    grd_UnitList.DataBind();
                }

                //Session["dsProdUnit"] = dsProdUnit;    

                Session["dsProductEdit"] = dsProductEdit;
            }

            dsUnit.Clear();

            var getUnit = unit.GetList(dsUnit, bu.GetConnectionString(Request.Params["BuCode"]));

            if (getUnit)
            {
                // Assign Primarykey                
                dsUnit.Tables[unit.TableName].PrimaryKey = GetPK();

                Session["dsUnit"] = dsUnit;
            }
            else
            {
                // Display Error Message
                return;
            }

            Session["dsProductEdit"] = dsProductEdit;
        }

        /// <summary>
        ///     Display season data.
        /// </summary>
        private void Page_Setting()
        {
            if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                txt_Code.Text = dsProductEdit.Tables[product.TableName].Rows[0]["ProductCode"].ToString();
                chk_Status.Checked = Convert.ToBoolean(dsProductEdit.Tables[product.TableName].Rows[0]["IsActive"]);

                txt_Description1.Text = dsProductEdit.Tables[product.TableName].Rows[0]["ProductDesc1"].ToString();
                txt_Descritpion2.Text = dsProductEdit.Tables[product.TableName].Rows[0]["ProductDesc2"].ToString();

                // Product category binding for level 3.
                var dtItemGroup = product.GetItemGroupListLookup(bu.GetConnectionString(Request.Params["BuCode"]));
                ddl_ItemGroup.DataSource = dtItemGroup;
                ddl_ItemGroup.DataTextField = "CategoryName";
                ddl_ItemGroup.DataValueField = "CategoryCode";
                ddl_ItemGroup.DataBind();
                ddl_ItemGroup.SelectedValue = dsProductEdit.Tables[product.TableName].Rows[0]["ProductCate"].ToString();

                // Product category binding for level 2.
                var dtProdSubCat = product.GetSubCategoryListLookup(bu.GetConnectionString(Request.Params["BuCode"]));
                ddl_SubCategory.DataSource = dtProdSubCat;
                ddl_SubCategory.DataTextField = "CategoryName";
                ddl_SubCategory.DataValueField = "CategoryCode";
                ddl_SubCategory.DataBind();
                ddl_SubCategory.SelectedValue =
                    product.GetParentNoByCategoryCode(
                        dsProductEdit.Tables[product.TableName].Rows[0]["ProductCate"].ToString(),
                        bu.GetConnectionString(Request.Params["BuCode"]));

                // Product category binding for level 1.
                var dtProdCat = product.GetCategoryListLookup(bu.GetConnectionString(Request.Params["BuCode"]));
                ddl_Category.DataSource = dtProdCat;
                ddl_Category.DataTextField = "CategoryName";
                ddl_Category.DataValueField = "CategoryCode";
                ddl_Category.DataBind();
                ddl_Category.SelectedValue = product.GetParentNoByCategoryCode(ddl_SubCategory.SelectedItem.Value
                    , bu.GetConnectionString(Request.Params["BuCode"]));

                // BarCOde
                txt_BarCode.Text = dsProductEdit.Tables[product.TableName].Rows[0]["BarCode"].ToString();
                cmb_OrderUnit.Value = dsProductEdit.Tables[product.TableName].Rows[0]["OrderUnit"].ToString();

                if (dsProductEdit.Tables[product.TableName].Rows[0]["OrderUnit"].ToString().Equals(""))
                {
                    cmb_OrderUnit.Value = System.DBNull.Value;
                }
                else
                {
                    cmb_OrderUnit.Value = (dsProductEdit.Tables[product.TableName].Rows[0]["OrderUnit"].ToString());
                }

                // OrderConverseRate
                txt_InvenConvOrder.Text =
                    dsProductEdit.Tables[product.TableName].Rows[0]["InventoryConvOrder"].ToString();

                cmb_InventoryUnit.Value = dsProductEdit.Tables[product.TableName].Rows[0]["InventoryUnit"].ToString();

                if (dsProductEdit.Tables[product.TableName].Rows[0]["InventoryUnit"].ToString().Equals(""))
                {
                    cmb_InventoryUnit.Value = System.DBNull.Value;
                }
                else
                {
                    cmb_InventoryUnit.Value =
                        (dsProductEdit.Tables[product.TableName].Rows[0]["InventoryUnit"].ToString());
                }

                //cmb_RecipeUnit.Value  = dsProductEdit.Tables[product.TableName].Rows[0]["RecipeUnit"].ToString();                

                //if (dsProductEdit.Tables[product.TableName].Rows[0]["RecipeUnit"].ToString().Equals(""))
                //{
                //    cmb_RecipeUnit.Value = System.DBNull.Value;
                //}
                //else 
                //{
                //    cmb_RecipeUnit.Value  = (dsProductEdit.Tables[product.TableName].Rows[0]["RecipeUnit"].ToString());
                //}

                ddl_TAXType.SelectedValue = dsProductEdit.Tables[product.TableName].Rows[0]["TAXType"].ToString();

                txt_RecipeConverseRate.Text =
                    dsProductEdit.Tables[product.TableName].Rows[0]["RecipeConvInvent"].ToString();
                txt_TaxRate.Text = dsProductEdit.Tables[product.TableName].Rows[0]["TaxRate"].ToString();

                txt_StandardCost.Text = dsProductEdit.Tables[product.TableName].Rows[0]["StandardCost"].ToString();
                //txt_LastCost.Text             = dsProductEdit.Tables[product.TableName].Rows[0]["LastCost"].ToString();

                // Get Last Price.
                var dsLastPrice = new DataSet();
                var getLastPrice = product.GetLastPrice(dsLastPrice,
                    dsProductEdit.Tables[product.TableName].Rows[0]["ProductCode"].ToString()
                    , bu.GetConnectionString(Request.Params["BuCode"]));

                if (getLastPrice)
                {
                    if (dsLastPrice.Tables[product.TableName].Rows.Count > 0)
                    {
                        var drLastPrice = dsLastPrice.Tables[product.TableName].Rows[0];

                        txt_LastCost.Text = String.Format("{0:n}", drLastPrice["LastPrice"]);
                    }
                }

                txt_QuantityDeviation.Text =
                    dsProductEdit.Tables[product.TableName].Rows[0]["QuantityDeviation"].ToString();
                txt_PriceDeviation.Text = dsProductEdit.Tables[product.TableName].Rows[0]["PriceDeviation"].ToString();

                chk_ReqHQAppr.Checked =
                    (Convert.ToBoolean(dsProductEdit.Tables[product.TableName].Rows[0]["ReqHQAppr"]));

                txt_TaxAccCode.Text = dsProductEdit.Tables[product.TableName].Rows[0]["TaxAccCode"].ToString();

                chk_IsRecipe.Checked = Convert.ToBoolean(dsProductEdit.Tables[product.TableName].Rows[0]["IsRecipe"]);
                chk_SaleItem.Checked = Convert.ToBoolean(dsProductEdit.Tables[product.TableName].Rows[0]["SaleItem"]);

                //txt_ApprovalLevel.Text      = dsProductEdit.Tables[product.TableName].Rows[0]["ApprovalLevel"].ToString();
                ddl_ApprovalLevel.Value = dsProductEdit.Tables[product.TableName].Rows[0]["ApprovalLevel"] + " : "
                                          +
                                          apprLv.GetName(
                                              dsProductEdit.Tables[product.TableName].Rows[0]["ApprovalLevel"].ToString(),
                                              bu.GetConnectionString(Request.Params["BuCode"]));
            }
            else
            {
                // Enable dropdown controls
                EnableControls();

                // Default
                chk_Status.Checked = true;

                // Product category binding for level 1.
                var dtProdCat = product.GetCategoryListLookup(bu.GetConnectionString(Request.Params["BuCode"]));

                ddl_Category.DataSource = dtProdCat;
                ddl_Category.DataTextField = "CategoryName";
                ddl_Category.DataValueField = "CategoryCode";
                ddl_Category.DataBind();

                // Product category binding for level 2.
                var dtProdSubCat = product.GetSubCategoryListLookup(bu.GetConnectionString(Request.Params["BuCode"]));
                ddl_SubCategory.DataSource = dtProdSubCat;
                ddl_SubCategory.DataTextField = "CategoryName";
                ddl_SubCategory.DataValueField = "CategoryCode";
                ddl_SubCategory.DataBind();

                // Product category binding for level 3.
                var dtItemGroup = product.GetItemGroupListLookup(bu.GetConnectionString(Request.Params["BuCode"]));
                ddl_ItemGroup.DataSource = dtItemGroup;
                ddl_ItemGroup.DataTextField = "CategoryName";
                ddl_ItemGroup.DataValueField = "CategoryCode";
                ddl_ItemGroup.DataBind();

                // Show Prod Unit
                grd_ProdUnit.DataBind();
            }

            grd_Unit.DataSource = dsUnit.Tables[unit.TableName];
            grd_Unit.DataBind();

            EnableRecipeControls();
        }

        /// <summary>
        /// </summary>
        private void EnableControls()
        {
            ddl_Category.Enabled = true;
            ddl_SubCategory.Enabled = true;
            ddl_ItemGroup.Enabled = true;
        }

        /// <summary>
        ///     Recipe Controls enable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnableRecipeControls()
        {
            if (chk_IsRecipe.Checked)
            {
                //cmb_RecipeUnit.Enabled        = true;
                txt_RecipeConverseRate.Enabled = true;
                //req_RecipeUnit.Enabled        = true;
                req_RecipeConverseRate.Enabled = true;
                reg_RecipeConverseRate.Enabled = true;
            }
            else
            {
                //cmb_RecipeUnit.Enabled         = false;
                txt_RecipeConverseRate.Enabled = false;
                //cmb_RecipeUnit.Value           = string.Empty;
                txt_RecipeConverseRate.Text = string.Empty;
                //req_RecipeUnit.Enabled         = false;
                req_RecipeConverseRate.Enabled = false;
                reg_RecipeConverseRate.Enabled = false;
            }

            if (ddl_TAXType.SelectedItem.Value.ToUpper() == "N")
            {
                txt_TaxRate.Text = Convert.ToInt32("0").ToString();
                reg_TaxRate.Enabled = false;
            }
            else if (ddl_TAXType.SelectedItem.Value.ToUpper() == "I")
            {
                //txt_TaxRate.Text = string.Empty;
                reg_TaxRate.Enabled = true;
            }
            else if (ddl_TAXType.SelectedItem.Value.ToUpper() == "A")
            {
                //txt_TaxRate.Text = string.Empty;
                reg_TaxRate.Enabled = true;
            }
        }


        protected void ddl_TaxAccCode_SelectedIndexChanged(object sender, EventArgs e)
        {
        }


        protected void ddl_Category_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            // Product category binding for level 2.
            var dtProdSubCat = product.GetSubCategoryListLookup(bu.GetConnectionString(Request.Params["BuCode"]));
            dtProdSubCat.DefaultView.RowFilter = "ParentNo = '" + ddl_Category.SelectedItem.Value + "' ";
            var dvProdSubCat = dtProdSubCat.DefaultView;
            ddl_SubCategory.DataSource = dvProdSubCat;
            ddl_SubCategory.DataTextField = "CategoryName";
            ddl_SubCategory.DataValueField = "CategoryCode";
            ddl_SubCategory.DataBind();

            // Product category binding for level 3.
            dtItemGroup = product.GetItemGroupListLookupByParentNo(ddl_SubCategory.SelectedItem.Value,
                bu.GetConnectionString(Request.Params["BuCode"]));

            ddl_ItemGroup.DataSource = dtItemGroup;
            ddl_ItemGroup.DataTextField = "CategoryName";
            ddl_ItemGroup.DataValueField = "CategoryCode";
            ddl_ItemGroup.DataBind();

            txt_Code.Text = string.Empty;
            ddl_ItemGroup.SelectedValue = string.Empty;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_SubCategory_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            // Product category binding for level 3.
            dtItemGroup = product.GetItemGroupListLookupByParentNo(ddl_SubCategory.SelectedItem.Value,
                bu.GetConnectionString(Request.Params["BuCode"]));

            ddl_ItemGroup.DataSource = dtItemGroup;
            ddl_ItemGroup.DataTextField = "CategoryName";
            ddl_ItemGroup.DataValueField = "CategoryCode";
            ddl_ItemGroup.DataBind();

            txt_Code.Text = string.Empty;
        }

        /// <summary>
        ///     Itemgroup selected change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_ItemGroup_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            txt_Code.Text = string.Empty;
            //txt_ApprovalLevel.Text = (prodCat.ProdCat_GeApprovalLevelByCategoryCode(Convert.ToInt32(ddl_ItemGroup.SelectedItem.Value.ToString()), LoginInfo.ConnStr).ToString());
            ddl_ApprovalLevel.Value = (prodCat.ProdCat_GeApprovalLevelByCategoryCode(
                (ddl_ItemGroup.SelectedItem.Value), bu.GetConnectionString(Request.Params["BuCode"]))
                                       + " : " +
                                       (apprLv.GetName(
                                           prodCat.ProdCat_GeApprovalLevelByCategoryCode(
                                               (ddl_ItemGroup.SelectedItem.Value),
                                               bu.GetConnectionString(Request.Params["BuCode"])).ToString(),
                                           bu.GetConnectionString(Request.Params["BuCode"]))));
            txt_TaxAccCode.Text =
                (prodCat.ProdCat_GetTaxAccCodeByCategoryCode(Convert.ToInt32(ddl_ItemGroup.SelectedItem.Value),
                    bu.GetConnectionString(Request.Params["BuCode"])));
        }

        /// <summary>
        ///     Check Recipe item or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chk_IsRecipe_OnCheckedChanged(object sender, EventArgs e)
        {
            if (chk_IsRecipe.Checked)
            {
                //cmb_RecipeUnit.Enabled         = true;
                txt_RecipeConverseRate.Enabled = true;
                //req_RecipeUnit.Enabled         = true;
                req_RecipeConverseRate.Enabled = true;
                reg_RecipeConverseRate.Enabled = true;
            }
            else
            {
                //cmb_RecipeUnit.Enabled         = false;
                txt_RecipeConverseRate.Enabled = false;
                //cmb_RecipeUnit.Value           = string.Empty;
                txt_RecipeConverseRate.Text = string.Empty;
                //req_RecipeUnit.Enabled         = false;
                req_RecipeConverseRate.Enabled = false;
                reg_RecipeConverseRate.Enabled = false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_TAXType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_TAXType.SelectedItem.Value.ToUpper() == "N")
            {
                txt_TaxRate.Text = Convert.ToInt32("0").ToString();
                reg_TaxRate.Enabled = false;
            }
            else if (ddl_TAXType.SelectedItem.Value.ToUpper() == "I")
            {
                txt_TaxRate.Text = string.Empty;
                reg_TaxRate.Enabled = true;
            }
            else if (ddl_TAXType.SelectedItem.Value.ToUpper() == "A")
            {
                txt_TaxRate.Text = string.Empty;
                reg_TaxRate.Enabled = true;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":
                    Page.Validate();

                    dsProductEdit = (DataSet)Session["dsProductEdit"];

                    if (Page.IsValid)
                    {
                        if (Request.Params["MODE"].ToUpper() == "EDIT")
                        {
                            var drProductEdit = dsProductEdit.Tables[product.TableName].Rows[0];

                            drProductEdit["ProductCode"] = txt_Code.Text.Trim();
                            drProductEdit["ProductDesc1"] = txt_Description1.Text;
                            drProductEdit["ProductDesc2"] = txt_Descritpion2.Text;
                            drProductEdit["ProductCate"] = ddl_ItemGroup.SelectedItem.Value;
                            drProductEdit["BarCode"] = txt_BarCode.Text;

                            if (cmb_InventoryUnit.SelectedIndex >= 0)
                            {
                                drProductEdit["InventoryUnit"] = (cmb_InventoryUnit.SelectedItem.Value.ToString());
                            }
                            else
                            {
                                drProductEdit["InventoryUnit"] = System.DBNull.Value;
                            }

                            if (cmb_OrderUnit.SelectedIndex >= 0)
                            {
                                drProductEdit["OrderUnit"] = (cmb_OrderUnit.SelectedItem.Value.ToString());
                            }
                            else
                            {
                                drProductEdit["OrderUnit"] = System.DBNull.Value;
                            }


                            //if (cmb_RecipeUnit.SelectedIndex >=0)
                            //{

                            //    drProductEdit["RecipeUnit"] = cmb_RecipeUnit.SelectedItem.Value.ToString();
                            //}
                            //else
                            //{
                            //    drProductEdit["RecipeUnit"] = System.DBNull.Value;
                            //}


                            if (txt_InvenConvOrder.Text != string.Empty)
                            {
                                drProductEdit["InventoryConvOrder"] = Convert.ToDecimal(txt_InvenConvOrder.Text);
                            }
                            else
                            {
                                drProductEdit["InventoryConvOrder"] = 0;
                            }

                            if (txt_RecipeConverseRate.Text != string.Empty)
                            {
                                drProductEdit["RecipeConvInvent"] = Convert.ToDecimal(txt_RecipeConverseRate.Text);
                            }
                            else
                            {
                                drProductEdit["RecipeConvInvent"] = 0;
                            }

                            if (txt_InvenConvOrder.Text != string.Empty)
                            {
                                drProductEdit["InventoryConvOrder"] = Convert.ToDecimal(txt_InvenConvOrder.Text);
                            }
                            else
                            {
                                drProductEdit["InventoryConvOrder"] = 0;
                            }

                            if (ddl_TAXType.SelectedItem.Value != string.Empty)
                            {
                                drProductEdit["TaxType"] = ddl_TAXType.SelectedItem.Value;
                            }

                            //if (txt_TaxRate.Text != string.Empty)
                            //{
                            //    drProductEdit["TaxRate"] = Convert.ToDecimal(txt_TaxRate.Text.ToString());
                            //}
                            //else
                            //{
                            //    drProductEdit["TaxRate"] = 0;
                            //}

                            if (!string.IsNullOrEmpty(txt_TaxRate.Text) && ddl_TAXType.SelectedItem.Value == "N")
                            {
                                drProductEdit["TaxRate"] = 0;
                            }
                            else if (!string.IsNullOrEmpty(txt_TaxRate.Text) && ddl_TAXType.SelectedItem.Value != "N")
                            {
                                drProductEdit["TaxRate"] = Convert.ToDecimal(txt_TaxRate.Text);
                            }
                            else
                            {
                                pop_AlertTaxRate.ShowOnPageLoad = true;
                                return;
                            }

                            if (txt_StandardCost.Text != string.Empty)
                            {
                                drProductEdit["StandardCost"] = Convert.ToDecimal(txt_StandardCost.Text);
                            }
                            else
                            {
                                drProductEdit["StandardCost"] = 0;
                            }

                            if (txt_LastCost.Text != string.Empty)
                            {
                                drProductEdit["LastCost"] = Convert.ToDecimal(txt_LastCost.Text);
                            }
                            else
                            {
                                drProductEdit["LastCost"] = 0;
                            }

                            if (txt_QuantityDeviation.Text != string.Empty)
                            {
                                drProductEdit["QuantityDeviation"] = Convert.ToDecimal(txt_QuantityDeviation.Text);
                            }
                            else
                            {
                                drProductEdit["QuantityDeviation"] = 0;
                            }

                            if (txt_PriceDeviation.Text != string.Empty)
                            {
                                drProductEdit["PriceDeviation"] = Convert.ToDecimal(txt_PriceDeviation.Text);
                            }
                            else
                            {
                                drProductEdit["PriceDeviation"] = 0;
                            }

                            if (chk_ReqHQAppr.Checked)
                            {
                                drProductEdit["ReqHQAppr"] = true;
                            }
                            else
                            {
                                drProductEdit["ReqHQAppr"] = false;
                            }
                            drProductEdit["IsActive"] = Convert.ToBoolean(chk_Status.Checked);
                            drProductEdit["TaxAccCode"] = txt_TaxAccCode.Text.Trim();
                            drProductEdit["IsRecipe"] = chk_IsRecipe.Checked;
                            drProductEdit["SaleItem"] = chk_SaleItem.Checked;
                            //drProductEdit["ApprovalLevel"] = (txt_ApprovalLevel.Text.Trim() == string.Empty ? 0 : int.Parse(txt_ApprovalLevel.Text.Trim()));
                            var ApprLvValue = ddl_ApprovalLevel.Value.ToString();
                            var Value = ApprLvValue.Split(':');
                            drProductEdit["ApprovalLevel"] = (Value[0] == string.Empty ? 0 : int.Parse(Value[0]));
                            drProductEdit["UpdatedDate"] = ServerDateTime;
                            drProductEdit["UpdatedBy"] = LoginInfo.LoginName;
                        }
                        else // NEW
                        {
                            //product.GetStructure(dsProductEdit, bu.GetConnectionString(Request.Params["BuCode"].ToString()));

                            // New Process
                            var drProductNew = dsProductEdit.Tables["Product"].NewRow();

                            drProductNew["ProductCode"] = product.GetGenerateProductCode(ddl_ItemGroup.SelectedItem.Value
                                    , bu.GetConnectionString(Request.Params["BuCode"]));
                            drProductNew["ProductDesc1"] = txt_Description1.Text;
                            drProductNew["ProductDesc2"] = txt_Descritpion2.Text;
                            drProductNew["ProductCate"] = ddl_ItemGroup.SelectedItem.Value;
                            drProductNew["BarCode"] = txt_BarCode.Text;

                            //if (cmb_RecipeUnit.SelectedIndex >= 0)
                            //{

                            //    drProductNew["RecipeUnit"] = (cmb_RecipeUnit.SelectedItem.Value.ToString());
                            //}                            
                            //else
                            //{
                            //    drProductNew["RecipeUnit"] = System.DBNull.Value;
                            //}


                            if (cmb_InventoryUnit.SelectedIndex >= 0)
                            {
                                drProductNew["InventoryUnit"] = (cmb_InventoryUnit.SelectedItem.Value.ToString());
                            }
                            else
                            {
                                drProductNew["InventoryUnit"] = System.DBNull.Value;
                            }

                            if (cmb_InventoryUnit.SelectedIndex >= 0)
                            {
                                drProductNew["OrderUnit"] = (cmb_OrderUnit.SelectedItem.Value.ToString());
                            }
                            else
                            {
                                drProductNew["OrderUnit"] = System.DBNull.Value;
                            }

                            drProductNew["TaxType"] = ddl_TAXType.SelectedItem.Value;

                            if (txt_RecipeConverseRate.Text != string.Empty)
                            {
                                drProductNew["RecipeConvInvent"] = Convert.ToDecimal(txt_RecipeConverseRate.Text);
                            }
                            else
                            {
                                drProductNew["RecipeConvInvent"] = 0;
                            }

                            if (txt_InvenConvOrder.Text != string.Empty)
                            {
                                drProductNew["InventoryConvOrder"] = Convert.ToDecimal(txt_InvenConvOrder.Text);
                            }
                            else
                            {
                                drProductNew["InventoryConvOrder"] = 0;
                            }

                            if (!string.IsNullOrEmpty(txt_TaxRate.Text) && ddl_TAXType.SelectedItem.Value == "N")
                            {
                                drProductNew["TaxRate"] = 0;
                            }
                            else if (!string.IsNullOrEmpty(txt_TaxRate.Text) && ddl_TAXType.SelectedItem.Value != "N")
                            {
                                drProductNew["TaxRate"] = Convert.ToDecimal(txt_TaxRate.Text);
                            }
                            else
                            {
                                pop_AlertTaxRate.ShowOnPageLoad = true;
                                return;
                            }

                            if (txt_StandardCost.Text != string.Empty)
                            {
                                drProductNew["StandardCost"] = Convert.ToDecimal(txt_StandardCost.Text);
                            }
                            else
                            {
                                drProductNew["StandardCost"] = 0;
                            }

                            if (txt_LastCost.Text != string.Empty)
                            {
                                drProductNew["LastCost"] = Convert.ToDecimal(txt_LastCost.Text);
                            }
                            else
                            {
                                drProductNew["LastCost"] = 0;
                            }

                            if (txt_QuantityDeviation.Text != string.Empty)
                            {
                                drProductNew["QuantityDeviation"] = Convert.ToDecimal(txt_QuantityDeviation.Text);
                            }
                            else
                            {
                                drProductNew["QuantityDeviation"] = 0;
                            }

                            if (txt_PriceDeviation.Text != string.Empty)
                            {
                                drProductNew["PriceDeviation"] = Convert.ToDecimal(txt_PriceDeviation.Text);
                            }
                            else
                            {
                                drProductNew["PriceDeviation"] = 0;
                            }

                            if (chk_ReqHQAppr.Checked)
                            {
                                drProductNew["ReqHQAppr"] = true;
                            }
                            else
                            {
                                drProductNew["ReqHQAppr"] = false;
                            }

                            drProductNew["IsActive"] = Convert.ToBoolean(chk_Status.Checked);
                            drProductNew["TaxAccCode"] = txt_TaxAccCode.Text.Trim();
                            drProductNew["IsRecipe"] = chk_IsRecipe.Checked;
                            drProductNew["SaleItem"] = chk_SaleItem.Checked;
                            //drProductNew["ApprovalLevel"] = (txt_ApprovalLevel.Text.Trim() == string.Empty ? 0 : int.Parse(txt_ApprovalLevel.Text.Trim()));
                            var ApprLvValue = ddl_ApprovalLevel.Value.ToString();
                            var Value = ApprLvValue.Split(':');
                            drProductNew["ApprovalLevel"] = (Value[0] == string.Empty ? 0 : int.Parse(Value[0]));
                            drProductNew["CreatedDate"] = ServerDateTime;
                            drProductNew["CreatedBy"] = LoginInfo.LoginName;
                            drProductNew["UpdatedDate"] = ServerDateTime;
                            drProductNew["UpdatedBy"] = LoginInfo.LoginName;

                            // Add new row
                            dsProductEdit.Tables[product.TableName].Rows.Add(drProductNew);

                            //if (dsProductEdit.Tables[prodUnit.TableName] != null)
                            //{
                            //    if (dsProductEdit.Tables[prodUnit.TableName].Rows.Count > 0)
                            //    {
                            //        for (int i = 0; i < dsProductEdit.Tables[prodUnit.TableName].Rows.Count; i++)
                            //        {
                            //            DataRow drProdUnit = dsProductEdit.Tables[prodUnit.TableName].Rows[i];

                            //            if (dsProductEdit.Tables[prodUnit.TableName].Rows[i].RowState != DataRowState.Deleted)
                            //            {
                            //                drProdUnit["ProductCode"] = dsProductEdit.Tables[product.TableName].Rows[0]["ProductCode"].ToString();
                            //            }                                        
                            //        }                                
                            //    }
                            //}

                            //Insert ProdUnit
                            // Default Inventory Unit to Database
                            var drProdUnitInv = dsProductEdit.Tables[prodUnit.TableName].NewRow();
                            drProdUnitInv["ProductCode"] = drProductNew["ProductCode"].ToString();
                            drProdUnitInv["OrderUnit"] = cmb_InventoryUnit.Value;
                            drProdUnitInv["Rate"] = 1;
                            drProdUnitInv["IsDefault"] = true;
                            drProdUnitInv["UnitType"] = 'I';
                            dsProductEdit.Tables[prodUnit.TableName].Rows.Add(drProdUnitInv);

                            // Default Order Unit to Database
                            var drProdUnitOdr = dsProductEdit.Tables[prodUnit.TableName].NewRow();
                            drProdUnitOdr["ProductCode"] = drProductNew["ProductCode"].ToString();
                            drProdUnitOdr["OrderUnit"] = cmb_OrderUnit.Value;
                            drProdUnitOdr["Rate"] = txt_InvenConvOrder.Text;
                            drProdUnitOdr["IsDefault"] = true;
                            drProdUnitOdr["UnitType"] = 'O';
                            dsProductEdit.Tables[prodUnit.TableName].Rows.Add(drProdUnitOdr);
                        }

                        var result = product.SaveProductAndProdUnit(dsProductEdit,
                            bu.GetConnectionString(Request.Params["BuCode"]));

                        if (result)
                        {
                            if (Request.Params["MODE"] == "EDIT")
                            {
                                Response.Redirect("Prod.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" +
                                                  Request.Params["ID"]);
                            }
                            else
                            {
                                Response.Redirect("Prod.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" +
                                                  dsProductEdit.Tables[product.TableName].Rows[0]["ProductCode"]);
                            }
                        }
                    }
                    break;


                case "BACK":
                    if (Request.Params["MODE"] == "Edit")
                    {
                        Response.Redirect("Prod.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + Request.Params["ID"]);
                    }
                    else if (Request.Params["MODE"] == "new")
                    {
                        Response.Redirect("ProdList.aspx");
                    }

                    break;
            }
        }

        /******** Start Unit Databinding for poup***************************/

        /// <summary>
        ///     Define statement for create/delete/print
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Text.ToUpper())
            {
                case "CREATE":
                    Create();
                    break;

                case "DELETE":
                    Delete();
                    break;

                case "CLOSE":
                    //Mdlp_RecipeUnitPopup.Hide();
                    break;
            }
        }

        /// <summary>
        ///     Create New Unit
        /// </summary>
        private void Create()
        {
            grd_Unit.AddNewRow();
            //Mdlp_RecipeUnitPopup.Show();
        }

        /// <summary>
        ///     Display confrim delete Unit
        /// </summary>
        private void Delete()
        {
            if (grd_Unit.Selection.Count > 0)
            {
                //Mdlp_RecipeUnitPopup.Show();
                pop_ConfrimDelete.ShowOnPageLoad = true;
            }
        }

        /// <summary>
        ///     Delete selected Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        {
            var columnValues = grd_Unit.GetSelectedFieldValues("UnitCode");

            foreach (DataRow drDeleting in dsUnit.Tables[unit.TableName].Rows)
            {
                if (drDeleting.RowState != DataRowState.Deleted)
                {
                    if (drDeleting["UnitCode"].ToString().ToUpper() == columnValues[0].ToString().ToUpper())
                    {
                        drDeleting.Delete();
                    }
                }
            }

            // Save to database
            var saveUnit = unit.Save(dsUnit, bu.GetConnectionString(Request.Params["BuCode"]));

            if (saveUnit)
            {
                pop_ConfrimDelete.ShowOnPageLoad = false;

                CheckDeletedRow(columnValues[0].ToString());

                RefreshUnitDataBinding();

                Page_Retrieve();
            }
        }

        /// <summary>
        ///     Check deleted row is selected in combo box.
        /// </summary>
        private void CheckDeletedRow(string UnitCode)
        {
            // Checking poup deleted row is selected in combo box or not.
            // If selected, clear selected value in combo box.
            //if (cmb_RecipeUnit.SelectedIndex >= 0)
            //{
            //    if (cmb_RecipeUnit.SelectedItem.Value.ToString().ToUpper() == UnitCode.ToString().ToUpper())
            //    {
            //        cmb_RecipeUnit.Value = string.Empty;
            //    }
            //}

            if (cmb_OrderUnit.SelectedIndex >= 0)
            {
                if (cmb_OrderUnit.SelectedItem.Value.ToString().ToUpper() == UnitCode.ToUpper())
                {
                    cmb_OrderUnit.Value = string.Empty;
                }
            }

            if (cmb_InventoryUnit.SelectedIndex >= 0)
            {
                if (cmb_InventoryUnit.SelectedItem.Value.ToString().ToUpper() == UnitCode.ToUpper())
                {
                    cmb_InventoryUnit.Value = string.Empty;
                }
            }
        }

        /// <summary>
        ///     No click process, closed poup dialogue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ConfrimDeleteNo_Click(object sender, EventArgs e)
        {
            grd_Unit.Selection.UnselectAll();
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        /* End Unit Databinding for poup*/

        protected void btn_OK_Click(object sender, EventArgs e)
        {
            pop_AlertTaxRate.ShowOnPageLoad = false;
        }

        protected void lnkb_New_Click(object sender, EventArgs e)
        {
            // Show popup
            pop_ProdUnit.ShowOnPageLoad = true;
        }

        protected void ddl_Product_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dsProductEdit.Tables["UnitList"] != null)
            {
                dsProductEdit.Tables["UnitList"].Clear();
            }

            var getUnit = prodUnit.GetUnitList(dsProductEdit, ddl_Product.SelectedItem.Value,
                bu.GetConnectionString(Request.Params["BuCode"]));

            if (getUnit)
            {
                grd_UnitList.DataSource = dsProductEdit.Tables["UnitList"];
                grd_UnitList.DataBind();
            }

            Session["dsProductEdit"] = dsProductEdit;
        }

        protected void grd_UnitList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("chk_Selected") != null)
                {
                    var chk_Selected = e.Row.FindControl("chk_Selected") as CheckBox;
                    chk_Selected.Checked = bool.Parse(DataBinder.Eval(e.Row.DataItem, "Selected").ToString());
                }
            }
        }

        protected void btn_OK_ProdUnit_Click(object sender, EventArgs e)
        {
            dsProductEdit = (DataSet)Session["dsProductEdit"];

            if (dsProductEdit != null)
            {
                for (var i = dsProductEdit.Tables[prodUnit.TableName].Rows.Count - 1; i >= 0; i--)
                {
                    var drProdUnit = dsProductEdit.Tables[prodUnit.TableName].Rows[i];
                    drProdUnit.Delete();
                }

                // Check base unit required by check only one rate = 1.00
                var baseUnitCount = 0;

                foreach (GridViewRow gvr_UnitList in grd_UnitList.Rows)
                {
                    // Insert new conversion rate (only selected unit which rate not equal to 0)  
                    var chk_Selected = gvr_UnitList.FindControl("chk_Selected") as CheckBox;
                    var lbl_Unit = gvr_UnitList.FindControl("lbl_Unit") as Label;
                    var txt_Rate = gvr_UnitList.FindControl("txt_Rate") as TextBox;

                    if (chk_Selected.Checked)
                    {
                        if (decimal.Parse(txt_Rate.Text.Trim()) != 0)
                        {
                            var drNewProdUnit = dsProductEdit.Tables[prodUnit.TableName].NewRow();
                            drNewProdUnit["ProductCode"] = txt_Code.Text;
                            drNewProdUnit["OrderUnit"] = lbl_Unit.Text;
                            drNewProdUnit["Rate"] = decimal.Parse(txt_Rate.Text.Trim());
                            drNewProdUnit["UnitType"] = "O";

                            dsProductEdit.Tables[prodUnit.TableName].Rows.Add(drNewProdUnit);

                            if (decimal.Parse(txt_Rate.Text.Trim()) == 1)
                            {
                                baseUnitCount++;
                            }
                        }
                    }
                }

                if (baseUnitCount == 0)
                {
                    // Display error message : There is no base unit assign
                    lbl_Error.Text = "There is no base unit assign (Rate=1)";
                    pop_Error.ShowOnPageLoad = true;
                    return;
                }

                pop_ProdUnit.ShowOnPageLoad = false;

                grd_ProdUnit.DataSource = dsProductEdit.Tables[prodUnit.TableName];
                grd_ProdUnit.DataBind();
            }
        }

        protected void btn_Cancel_ProdUnit_Click(object sender, EventArgs e)
        {
            pop_ProdUnit.ShowOnPageLoad = false;
        }

        #region "View"

        protected void btn_ViewGo_Click(object sender, EventArgs e)
        {
            Page_Retrieve();
        }

        #endregion

        #region "grd_Unit"

        /// <summary>
        ///     Re-binding Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Unit_OnLoad(object sender, EventArgs e)
        {
            grd_Unit.DataSource = dsUnit.Tables[unit.TableName];
            grd_Unit.DataBind();
        }

        /// <summary>
        ///     Assign Default Value for New Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Unit_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["UnitCode"] = string.Empty;
            e.NewValues["Name"] = string.Empty;
            e.NewValues["IsActived"] = true;
            e.NewValues["CreatedBy"] = LoginInfo.LoginName;
            e.NewValues["CreatedDate"] = ServerDateTime;
            e.NewValues["UpdatedBy"] = LoginInfo.LoginName;
            e.NewValues["UpdatedDate"] = ServerDateTime;
        }

        /// <summary>
        ///     Create New Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Unit_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            var drInserting = dsUnit.Tables[unit.TableName].NewRow();

            drInserting["UnitCode"] = e.NewValues["UnitCode"].ToString();
            drInserting["Name"] = e.NewValues["Name"].ToString();
            drInserting["IsActived"] = e.NewValues["IsActived"].ToString();
            drInserting["CreatedBy"] = LoginInfo.LoginName;
            drInserting["CreatedDate"] = ServerDateTime;
            drInserting["UpdatedBy"] = LoginInfo.LoginName;
            drInserting["UpdatedDate"] = ServerDateTime;

            dsUnit.Tables[unit.TableName].Rows.Add(drInserting);

            // Save to database
            var saveUnit = unit.Save(dsUnit, bu.GetConnectionString(Request.Params["BuCode"]));

            if (saveUnit)
            {
                grd_Unit.DataSource = dsUnit.Tables[unit.TableName];
                grd_Unit.CancelEdit();
                grd_Unit.DataBind();

                e.Cancel = true;

                Session["dsUnit"] = dsUnit;
            }
            else
            {
                // Display Error Message    
                dsUnit.Tables[unit.TableName].RejectChanges();
                grd_Unit.CancelEdit();

                e.Cancel = true;
            }
        }

        /// <summary>
        ///     Save Existing Unit Changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Unit_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            var drUpdating = dsUnit.Tables[unit.TableName].Rows[grd_Unit.EditingRowVisibleIndex];

            drUpdating["Name"] = e.NewValues["Name"].ToString();
            drUpdating["IsActived"] = e.NewValues["IsActived"].ToString();
            drUpdating["UpdatedBy"] = LoginInfo.LoginName;
            drUpdating["UpdatedDate"] = ServerDateTime;

            // Save to database
            var saveUnit = unit.Save(dsUnit, bu.GetConnectionString(Request.Params["BuCode"]));

            if (saveUnit)
            {
                grd_Unit.DataSource = dsUnit.Tables[unit.TableName];
                grd_Unit.CancelEdit();
                grd_Unit.DataBind();

                e.Cancel = true;

                Session["dsUnit"] = dsUnit;
            }
            else
            {
                // Display Error Message    
                dsUnit.Tables[unit.TableName].RejectChanges();
                grd_Unit.CancelEdit();

                e.Cancel = true;
            }
        }

        #endregion

        #region "grd_InventoryUnit"

        /// <summary>
        ///     Create New Unit
        /// </summary>
        private void CreateInventoryUnit()
        {
            grd_InventoryUnit.AddNewRow();
            //Mdlp_InventoryUnitPopup.Show();
        }

        /// <summary>
        ///     Display confrim delete Unit
        /// </summary>
        private void DeleteInventoryUnit()
        {
            if (grd_InventoryUnit.Selection.Count > 0)
            {
                //Mdlp_InventoryUnitPopup.Show();
                pop_ConfrimDeleteInventoryUnit.ShowOnPageLoad = true;
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menuInventoryUnit_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Text.ToUpper())
            {
                case "CREATE":
                    CreateInventoryUnit();
                    break;

                case "DELETE":
                    DeleteInventoryUnit();
                    break;

                case "CLOSE":
                    //Mdlp_RecipeUnitPopup.Hide();
                    break;
            }
        }

        /// <summary>
        ///     Re-binding Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_InventoryUnit_OnLoad(object sender, EventArgs e)
        {
            grd_InventoryUnit.DataSource = dsUnit.Tables[unit.TableName];
            grd_InventoryUnit.DataBind();
        }

        /// <summary>
        ///     Assign Default Value for New Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_InventoryUnit_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["UnitCode"] = string.Empty;
            e.NewValues["Name"] = string.Empty;
            e.NewValues["IsActived"] = true;
            e.NewValues["CreatedBy"] = LoginInfo.LoginName;
            e.NewValues["CreatedDate"] = ServerDateTime;
            e.NewValues["UpdatedBy"] = LoginInfo.LoginName;
            e.NewValues["UpdatedDate"] = ServerDateTime;
        }

        /// <summary>
        ///     Create New Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_InventoryUnit_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            var drInserting = dsUnit.Tables[unit.TableName].NewRow();

            drInserting["UnitCode"] = e.NewValues["UnitCode"].ToString();
            drInserting["Name"] = e.NewValues["Name"].ToString();
            drInserting["IsActived"] = e.NewValues["IsActived"].ToString();
            drInserting["CreatedBy"] = LoginInfo.LoginName;
            drInserting["CreatedDate"] = ServerDateTime;
            drInserting["UpdatedBy"] = LoginInfo.LoginName;
            drInserting["UpdatedDate"] = ServerDateTime;

            dsUnit.Tables[unit.TableName].Rows.Add(drInserting);

            // Save to database
            var saveUnit = unit.Save(dsUnit, bu.GetConnectionString(Request.Params["BuCode"]));

            if (saveUnit)
            {
                grd_InventoryUnit.DataSource = dsUnit.Tables[unit.TableName];
                grd_InventoryUnit.CancelEdit();
                grd_InventoryUnit.DataBind();

                e.Cancel = true;

                Session["dsUnit"] = dsUnit;
            }
            else
            {
                // Display Error Message    
                dsUnit.Tables[unit.TableName].RejectChanges();
                grd_InventoryUnit.CancelEdit();

                e.Cancel = true;
            }
        }

        /// <summary>
        ///     Save Existing Unit Changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_InventoryUnit_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            var drUpdating = dsUnit.Tables[unit.TableName].Rows[grd_InventoryUnit.EditingRowVisibleIndex];

            drUpdating["Name"] = e.NewValues["Name"].ToString();
            drUpdating["IsActived"] = e.NewValues["IsActived"].ToString();
            drUpdating["UpdatedBy"] = LoginInfo.LoginName;
            drUpdating["UpdatedDate"] = ServerDateTime;

            // Save to database
            var saveUnit = unit.Save(dsUnit, bu.GetConnectionString(Request.Params["BuCode"]));

            if (saveUnit)
            {
                grd_InventoryUnit.DataSource = dsUnit.Tables[unit.TableName];
                grd_InventoryUnit.CancelEdit();
                grd_InventoryUnit.DataBind();

                e.Cancel = true;

                Session["dsUnit"] = dsUnit;
            }
            else
            {
                // Display Error Message    
                dsUnit.Tables[unit.TableName].RejectChanges();
                grd_InventoryUnit.CancelEdit();

                e.Cancel = true;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ConfrimDeleteInventoryUnit_Click(object sender, EventArgs e)
        {
            var columnValues = grd_InventoryUnit.GetSelectedFieldValues("UnitCode");

            foreach (DataRow drDeleting in dsUnit.Tables[unit.TableName].Rows)
            {
                if (drDeleting.RowState != DataRowState.Deleted)
                {
                    if (drDeleting["UnitCode"].ToString().ToUpper() == columnValues[0].ToString().ToUpper())
                    {
                        drDeleting.Delete();
                    }
                }
            }

            // Save to database
            var saveUnit = unit.Save(dsUnit, bu.GetConnectionString(Request.Params["BuCode"]));

            if (saveUnit)
            {
                pop_ConfrimDeleteInventoryUnit.ShowOnPageLoad = false;

                CheckDeletedRow(columnValues[0].ToString());

                RefreshUnitDataBinding();

                Page_Retrieve();
            }
        }

        /// <summary>
        ///     RefreshUnitDataBinding
        /// </summary>
        protected void RefreshUnitDataBinding()
        {
            cmb_OrderUnit.ValueField = "UnitCode";
            cmb_OrderUnit.TextField = "UnitCode";
            cmb_OrderUnit.TextFormatString = "{0}";
            cmb_OrderUnit.DataSource = unit.GetUnitLookup(bu.GetConnectionString(Request.Params["BuCode"]));
            cmb_OrderUnit.DataBind();


            cmb_InventoryUnit.ValueField = "UnitCode";
            cmb_InventoryUnit.TextField = "UnitCode";
            cmb_InventoryUnit.TextFormatString = "{0}";
            cmb_InventoryUnit.DataSource = unit.GetUnitLookup(bu.GetConnectionString(Request.Params["BuCode"]));
            cmb_InventoryUnit.DataBind();


            //cmb_RecipeUnit.ValueField = "UnitCode";
            //cmb_RecipeUnit.TextField = "UnitCode";
            //cmb_RecipeUnit.TextFormatString = "{0}";
            //cmb_RecipeUnit.DataSource = unit.GetUnitLookup(LoginInfo.ConnStr);
            //cmb_RecipeUnit.DataBind();
        }

        /// <summary>
        ///     Poup confirm dialogue closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ConfrimDeleteInventoryUnitNo_Click(object sender, EventArgs e)
        {
            grd_InventoryUnit.Selection.UnselectAll();
            pop_ConfrimDeleteInventoryUnit.ShowOnPageLoad = false;
        }

        #endregion

        #region "grd_Orderunit"

        /// <summary>
        ///     Create New Unit
        /// </summary>
        private void CreateOrderunit()
        {
            grd_Orderunit.AddNewRow();
            //Mdlp_OrderunitPopup.Show();
        }

        /// <summary>
        ///     Display confrim delete Unit
        /// </summary>
        private void DeleteOrderunit()
        {
            if (grd_Orderunit.Selection.Count > 0)
            {
                //Mdlp_OrderunitPopup.Show();
                pop_ConfrimDeleteOrderunit.ShowOnPageLoad = true;
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menuOrderunit_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Text.ToUpper())
            {
                case "CREATE":
                    CreateOrderunit();
                    break;

                case "DELETE":
                    DeleteOrderunit();
                    break;

                case "CLOSE":
                    //Mdlp_RecipeUnitPopup.Hide();
                    break;
            }
        }


        /// <summary>
        ///     Re-binding Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Orderunit_OnLoad(object sender, EventArgs e)
        {
            grd_Orderunit.DataSource = dsUnit.Tables[unit.TableName];
            grd_Orderunit.DataBind();
        }

        /// <summary>
        ///     Assign Default Value for New Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Orderunit_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["UnitCode"] = string.Empty;
            e.NewValues["Name"] = string.Empty;
            e.NewValues["IsActived"] = true;
            e.NewValues["CreatedBy"] = LoginInfo.LoginName;
            e.NewValues["CreatedDate"] = ServerDateTime;
            e.NewValues["UpdatedBy"] = LoginInfo.LoginName;
            e.NewValues["UpdatedDate"] = ServerDateTime;
        }

        /// <summary>
        ///     Create New Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Orderunit_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            var drInserting = dsUnit.Tables[unit.TableName].NewRow();

            drInserting["UnitCode"] = e.NewValues["UnitCode"].ToString();
            drInserting["Name"] = e.NewValues["Name"].ToString();
            drInserting["IsActived"] = e.NewValues["IsActived"].ToString();
            drInserting["CreatedBy"] = LoginInfo.LoginName;
            drInserting["CreatedDate"] = ServerDateTime;
            drInserting["UpdatedBy"] = LoginInfo.LoginName;
            drInserting["UpdatedDate"] = ServerDateTime;

            dsUnit.Tables[unit.TableName].Rows.Add(drInserting);

            // Save to database
            var saveUnit = unit.Save(dsUnit, bu.GetConnectionString(Request.Params["BuCode"]));

            if (saveUnit)
            {
                grd_Orderunit.DataSource = dsUnit.Tables[unit.TableName];
                grd_Orderunit.CancelEdit();
                grd_Orderunit.DataBind();

                e.Cancel = true;

                Session["dsUnit"] = dsUnit;
            }
            else
            {
                // Display Error Message    
                dsUnit.Tables[unit.TableName].RejectChanges();
                grd_Orderunit.CancelEdit();

                e.Cancel = true;
            }
        }

        /// <summary>
        ///     Save Existing Unit Changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Orderunit_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            var drUpdating = dsUnit.Tables[unit.TableName].Rows[grd_Orderunit.EditingRowVisibleIndex];

            drUpdating["Name"] = e.NewValues["Name"].ToString();
            drUpdating["IsActived"] = e.NewValues["IsActived"].ToString();
            drUpdating["UpdatedBy"] = LoginInfo.LoginName;
            drUpdating["UpdatedDate"] = ServerDateTime;

            // Save to database
            var saveUnit = unit.Save(dsUnit, bu.GetConnectionString(Request.Params["BuCode"]));

            if (saveUnit)
            {
                grd_Orderunit.DataSource = dsUnit.Tables[unit.TableName];
                grd_Orderunit.CancelEdit();
                grd_Orderunit.DataBind();

                e.Cancel = true;

                Session["dsUnit"] = dsUnit;
            }
            else
            {
                // Display Error Message    
                dsUnit.Tables[unit.TableName].RejectChanges();
                grd_Orderunit.CancelEdit();

                e.Cancel = true;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ConfrimDeleteOrderunit_Click(object sender, EventArgs e)
        {
            var columnValues = grd_Orderunit.GetSelectedFieldValues("UnitCode");

            foreach (DataRow drDeleting in dsUnit.Tables[unit.TableName].Rows)
            {
                if (drDeleting.RowState != DataRowState.Deleted)
                {
                    if (drDeleting["UnitCode"].ToString().ToUpper() == columnValues[0].ToString().ToUpper())
                    {
                        drDeleting.Delete();
                    }
                }
            }

            // Save to database
            var saveUnit = unit.Save(dsUnit, bu.GetConnectionString(Request.Params["BuCode"]));

            if (saveUnit)
            {
                pop_ConfrimDeleteOrderunit.ShowOnPageLoad = false;

                CheckDeletedRow(columnValues[0].ToString());

                RefreshUnitDataBinding();

                Page_Retrieve();
            }
        }

        /// <summary>
        ///     No click process, closed poup dialogue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_OrderunitNo_Click(object sender, EventArgs e)
        {
            grd_Orderunit.Selection.UnselectAll();
            pop_ConfrimDeleteOrderunit.ShowOnPageLoad = false;
        }

        #endregion

        #region "Misc"

        private DataColumn[] GetPK()
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsUnit.Tables[unit.TableName].Columns["UnitCode"];

            return primaryKeys;
        }

        #endregion
    }
}