using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Inventory
{
    public partial class Prod3 : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();

        private readonly Blue.BL.Option.Inventory.ProdCateType prodCateType =
            new Blue.BL.Option.Inventory.ProdCateType();

        private readonly Blue.BL.Option.Inventory.ProdCat prodcat = new Blue.BL.Option.Inventory.ProdCat();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.APP.TAX tax = new Blue.BL.APP.TAX();
        private readonly Blue.BL.Option.Inventory.Unit unit = new Blue.BL.Option.Inventory.Unit();
        private DataSet dsProdLoc = new DataSet();
        private DataSet dsProduct = new DataSet();
        private DataSet dsStore = new DataSet();
        private DataSet dsStrMinMax = new DataSet();
        private Blue.BL.IN.ProdBu prodBu = new Blue.BL.IN.ProdBu();
        private Blue.BL.Option.Inventory.ProdLoc prodLoc = new Blue.BL.Option.Inventory.ProdLoc();
        private Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private Blue.BL.Option.Inventory.StoreLct store = new Blue.BL.Option.Inventory.StoreLct();
        private Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
        private Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();
        private Blue.BL.IN.VendorProd vendorProd = new Blue.BL.IN.VendorProd();

        #endregion

        #region "Operations"

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_ConnStr.Value = bu.GetConnectionString(Request.Params["BuCode"]);

            ////25/04/2012 edit by keng.
            //if (dsStrMinMax.Tables[store.TableName].Rows.Count > 0)
            //{
            //    List<object> selectedvalues = new List<object>();

            //    for (int i = 0; i < grd_Store.Rows.Count; i++)
            //    {
            //        CheckBox chk_Item = grd_Store.Rows[i].FindControl("chk_Item") as CheckBox;
            //        if (chk_Item.Checked)
            //        {
            //            Label lbl_LocationCode = grd_Store.Rows[i].FindControl("lbl_LocationCode") as Label;
            //            selectedvalues.Add(lbl_LocationCode.Text);
            //        }
            //    }

            //    if (dsProdLoc.Tables[prodLoc.TableName].Rows.Count > 0)
            //    {                    
            //        if (dsProdLoc.Tables[prodLoc.TableName].Rows.Count == selectedvalues.Count)
            //        {
            //            foreach (DataRow drStore in dsStrMinMax.Tables[store.TableName].Rows)
            //            {
            //                int i = 0;
            //                foreach (DataRow drAssigned in dsProdLoc.Tables[prodLoc.TableName].Rows)
            //                {
            //                    if (drAssigned.RowState != DataRowState.Deleted)
            //                    {
            //                        if (drStore["LocationCode"].ToString().ToUpper() == drAssigned["LocationCode"].ToString().ToUpper())
            //                        {                                        
            //                            CheckBox chk_Item  = grd_Store.Rows[i].FindControl("chk_Item") as CheckBox;                                        
            //                            TextBox txt_MinQty = grd_Store.Rows[i].FindControl("txt_MinQty") as TextBox;
            //                            TextBox txt_MaxQty = grd_Store.Rows[i].FindControl("txt_MaxQty") as TextBox;

            //                            chk_Item.Checked   = true;
            //                            txt_MinQty.Enabled = true;
            //                            txt_MaxQty.Enabled = true;                                        
            //                        }
            //                    }

            //                    i++;
            //                }
            //            }
            //        }

            //        if ((dsProdLoc.Tables[prodLoc.TableName].Rows.Count != selectedvalues.Count) && this.pageInit == true)
            //        {
            //            int j = 0;

            //            foreach (DataRow drStoreAll in dsStrMinMax.Tables[store.TableName].Rows)
            //            {
            //                foreach (DataRow drAssigned in dsProdLoc.Tables[prodLoc.TableName].Rows)
            //                {
            //                    if (drAssigned.RowState != DataRowState.Deleted)
            //                    {
            //                        if (drStoreAll["LocationCode"].ToString().ToUpper() == drAssigned["LocationCode"].ToString().ToUpper())
            //                        {
            //                            CheckBox chk_Item  = grd_Store.Rows[j].FindControl("chk_Item") as CheckBox;
            //                            TextBox txt_MinQty = grd_Store.Rows[j].FindControl("txt_MinQty") as TextBox;
            //                            TextBox txt_MaxQty = grd_Store.Rows[j].FindControl("txt_MaxQty") as TextBox;

            //                            chk_Item.Checked   = true;
            //                            txt_MinQty.Enabled = true;
            //                            txt_MaxQty.Enabled = true;
            //                        }
            //                    }
            //                }

            //                j++;
            //            }

            //            pageInit = false;
            //        }                   
            //        else if ((dsProdLoc.Tables[prodLoc.TableName].Rows.Count != selectedvalues.Count))
            //        {                        
            //            foreach (DataRow drStoreAll in dsStrMinMax.Tables[store.TableName].Rows)
            //            {
            //                int i = 0;
            //                for (int k = 0; k < selectedvalues.Count; k++)
            //                {                                
            //                    if (selectedvalues[k].ToString().ToUpper() == drStoreAll["LocationCode"].ToString().ToUpper())
            //                    {
            //                        CheckBox chk_Item  = grd_Store.Rows[i].FindControl("chk_Item") as CheckBox;                                    
            //                        TextBox txt_MinQty = grd_Store.Rows[i].FindControl("txt_MinQty") as TextBox;
            //                        TextBox txt_MaxQty = grd_Store.Rows[i].FindControl("txt_MaxQty") as TextBox;

            //                        chk_Item.Checked   = true;
            //                        txt_MinQty.Enabled = true;
            //                        txt_MaxQty.Enabled = true;                                                                     
            //                    }

            //                    i++;
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {                    
            //        foreach (DataRow drStoreAll in dsStrMinMax.Tables[store.TableName].Rows)
            //        {
            //            int i = 0;
            //            for (int j = 0; j < selectedvalues.Count; j++)
            //            {                            
            //                if (selectedvalues[j].ToString().ToUpper() == drStoreAll["LocationCode"].ToString().ToUpper())
            //                {
            //                    CheckBox chk_Item  = grd_Store.Rows[i].FindControl("chk_Item") as CheckBox;
            //                    TextBox txt_MinQty = grd_Store.Rows[j].FindControl("txt_MinQty") as TextBox;
            //                    TextBox txt_MaxQty = grd_Store.Rows[j].FindControl("txt_MaxQty") as TextBox;

            //                    chk_Item.Checked   = true;
            //                    txt_MinQty.Enabled = true;
            //                    txt_MaxQty.Enabled = true;                         
            //                }

            //                i++;
            //            }
            //        }
            //    }
            //}

            //dsProduct   = (DataSet)Session["dsProduct"];
            //dsStore     = (DataSet)Session["dsStore"];
            //dsProdLoc   = (DataSet)Session["dsProdLoc"];
            //dsStrMinMax = (DataSet)Session["dsStrMinMax"];
        }

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
                dsProduct = (DataSet) Session["dsProduct"];
                //dsStore   = (DataSet)Session["dsStore"];
                //dsProdLoc = (DataSet)Session["dsProdLoc"];
            }
        }

        private void Page_Retrieve()
        {
            // Get product data
            var getProduct = product.GetList(dsProduct, Request.Params["ID"], hf_ConnStr.Value);

            if (getProduct)
            {
                Session["dsProduct"] = dsProduct;
            }

            //bool getProdUnit = prodUnit.GetListAll(dsProduct, Request.Params["ID"].ToString(), hf_ConnStr.Value);

            //bool getProdUnitO = prodUnit.GetListByUnitType(dsProduct, Request.Params["ID"].ToString(), "OrderUnit", "O", hf_ConnStr.Value);
            //bool getProdUnitR = prodUnit.GetListByUnitType(dsProduct, Request.Params["ID"].ToString(), "RecipeUnit", "R", hf_ConnStr.Value);

            //if (getProdUnit)
            //{
            //    grd_OrderUnit1.DataSource = dsProduct.Tables["OrderUnit"]; //dvTable;
            //    grd_OrderUnit1.DataBind();

            //    grd_RecipeUnit.DataSource = dsProduct.Tables["RecipeUnit"]; //dvTable;
            //    grd_RecipeUnit.DataBind();
            //}

            //dsStore.Clear();
            //dsStrMinMax.Clear();

            //bool getUnit = store.GetList(dsStore, hf_ConnStr.Value);

            //// 2012-03-02 Version ที่มีการ Where isActive = true ของ Location.
            ////bool getUnit = store.GetList2(dsStore, hf_ConnStr.Value);

            //if (getUnit)
            //{
            //    Session["dsStore"] = dsStore;
            //}
            ////else
            ////{
            ////    // Display Error Message
            ////    return;
            ////}

            //bool getProdLoc = prodLoc.GetList_ProductCode(dsProdLoc, Request.Params["ID"].ToString(), hf_ConnStr.Value);
            //if (getProdLoc)
            //{
            //    Session["dsProdLoc"] = dsProdLoc;
            //}

            //bool GetMinMax = store.GetListMinMax(dsStrMinMax, Request.Params["ID"].ToString(), hf_ConnStr.Value);

            //if (GetMinMax)
            //{
            //    Session["dsStrMinMax"] = dsStrMinMax;
            //}
            ////else
            ////{
            ////    // Display Error Message
            ////    return;
            ////}

            //Session["dsProdLoc"] = dsProdLoc;
            //Session["dsProduct"] = dsProduct;
        }

        private void Page_Setting()
        {
            // Product Information ----------------------------------------------------------------
            var drProduct = dsProduct.Tables[product.TableName].Rows[0];

            lbl_Cateogry.Text =
                prodcat.GetName(prodcat.GetCategoryCode(drProduct["ProductCate"].ToString(), hf_ConnStr.Value),
                    hf_ConnStr.Value);
            lbl_SubCategory.Text =
                prodcat.GetName(prodcat.GetSubCategoryCode(drProduct["ProductCate"].ToString(), hf_ConnStr.Value),
                    hf_ConnStr.Value);
            lbl_ItemGroup.Text = prodcat.GetName(drProduct["ProductCate"].ToString(), hf_ConnStr.Value);
            lbl_Code.Text = drProduct["ProductCode"].ToString();
            lbl_Description1.Text = drProduct["ProductDesc1"].ToString();
            lbl_Description2.Text = drProduct["ProductDesc2"].ToString();
            lbl_MemberInRecipe.Text = (drProduct["IsRecipe"].ToString() != string.Empty
                ? Convert.ToBoolean(drProduct["IsRecipe"]) ? "Yes" : "No"
                : "No");
            lbl_TAXType.Text = tax.GetName(drProduct["TAXType"].ToString(), hf_ConnStr.Value);
            lbl_TaxRate.Text = Convert.ToDecimal(drProduct["TaxRate"]).ToString("0.00");
                // TODO: Number format should take from setting
            lbl_AccountCode.Text = drProduct["TaxAccCode"].ToString();
            lbl_ProdType.Text = prodCateType.GetName(drProduct["ApprovalLevel"].ToString(), hf_ConnStr.Value);
            lbl_QuantityDeviation.Text = Convert.ToDecimal(drProduct["QuantityDeviation"]).ToString("0.00");
                // TODO: Number format should take from setting
            lbl_PriceDeviation.Text = Convert.ToDecimal(drProduct["PriceDeviation"]).ToString("0.00");
                // TODO: Number format should take from setting
            lbl_Status.Text = (Convert.ToBoolean(drProduct["IsActive"]) ? "Active" : "InActive");
                // TODO: Wording "Active" or "InActive" should take from setting
            lbl_SaleItem.Text = (Convert.ToBoolean(drProduct["SaleItem"]) ? "Yes" : "No");
                // TODO: Wording "Yes" or "No" should take from setting
            lbl_ReqHQAppr.Text = (Convert.ToBoolean(drProduct["ReqHQAppr"]) ? "Yes" : "No");
                // TODO: Wording "Yes" or "No" should take from setting
            lbl_BarCode.Text = drProduct["BarCode"].ToString();
            lbl_StandardCost.Text = Convert.ToDecimal(drProduct["StandardCost"]).ToString("0.00");
                // TODO: Number format should take from setting
            lbl_LastCost.Text =
                product.GetLastPrice(drProduct["ProductCode"].ToString(), hf_ConnStr.Value).ToString("0.00");
                // TODO: Number format should take from setting
            lbl_InventoryUnit.Text = unit.GetName(drProduct["InventoryUnit"].ToString(), hf_ConnStr.Value);
            lbl_OrderUnit.Text = unit.GetName(drProduct["OrderUnit"].ToString(), hf_ConnStr.Value);
            lbl_OrderConverseRate.Text = Convert.ToDecimal(drProduct["InventoryConvOrder"]).ToString("0.000");
                // TODO: Number format should take from setting
            lbl_RecipeUnit.Text = unit.GetName(drProduct["RecipeUnit"].ToString(), hf_ConnStr.Value);
            lbl_RecipeConverseRate.Text =
                Convert.ToDecimal(StringOrZero(drProduct["RecipeConvInvent"])).ToString("0.000");
                // TODO: Number format should take from setting            

            // Unit Information -------------------------------------------------------------------
            // Binding Order Unit
            ProdUnitOrder.ProductCode = Request.Params["ID"];
            ProdUnitOrder.UnitType = PL.Option.Inventory.ProdUnit.ProdUnitType.Order;
            ProdUnitOrder.ConnStr = hf_ConnStr.Value;
            ProdUnitOrder.DataBind();

            // Binding Reccipe Unit
            ProdUnitRecipe.ProductCode = Request.Params["ID"];
            ProdUnitRecipe.UnitType = PL.Option.Inventory.ProdUnit.ProdUnitType.Recipe;
            ProdUnitRecipe.ConnStr = hf_ConnStr.Value;
            ProdUnitRecipe.DataBind();

            // Binding Product Location
            ProdLoc.ProductCode = Request.Params["ID"];
            ProdLoc.ConnStr = hf_ConnStr.Value;
            ProdLoc.DataBind();

            // Binding Vendor's Product
            ProdVendor.ProductCode = Request.Params["ID"];
            ProdVendor.ConnStr = hf_ConnStr.Value;
            ProdVendor.DataBind();

            // Binding Bu's Product
            if (bu.IsHQ(Request.Params["BuCode"]))
            {
                ProdBu.ProductCode = Request.Params["ID"];
                ProdBu.ConnStr = hf_ConnStr.Value;
                ProdBu.Visible = true;
                ProdBu.DataBind();
            }
            else
            {
                ProdBu.Visible = false;
            }
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Response.Redirect("ProdEdit3.aspx?MODE=new&BuCode=" + Request.Params["BuCode"]);
                    break;

                case "EDIT":
                    Response.Redirect("ProdEdit3.aspx?MODE=Edit&BuCode=" + Request.Params["BuCode"] + "&ID=" +
                                      Request.Params["ID"]);
                    break;

                case "DELETE":
                    pop_ConfrimDelete.ShowOnPageLoad = true;
                    break;

                case "BACK":
                    Response.Redirect("ProdList.aspx");
                    break;
            }
        }

        /// <summary>
        ///     Confirm Delete Product
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Yes_Click(object sender, EventArgs e)
        {
            dsProduct.Tables[product.TableName].Rows[0].Delete();

            var result = product.Save(dsProduct, hf_ConnStr.Value);

            if (result)
            {
                Response.Redirect("ProdList.aspx");
            }
            else
            {
                // show error message
                pop_WarningDelete.ShowOnPageLoad = true;
            }
        }

        /// <summary>
        ///     Cancel Delete Product
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_No_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        /// <summary>
        ///     OK button press on delete warning message, redirect to list page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = false;
            Response.Redirect("ProdList.aspx");
        }

        #endregion
    }
}