using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Inventory
{
    public partial class Prod : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Inventory.ApprLv apprLv = new Blue.BL.Option.Inventory.ApprLv();
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.IN.ProdBu prodBu = new Blue.BL.IN.ProdBu();
        private readonly Blue.BL.Option.Inventory.ProdLoc prodLoc = new Blue.BL.Option.Inventory.ProdLoc();
        private readonly Blue.BL.Option.Inventory.ProdCat prodcat = new Blue.BL.Option.Inventory.ProdCat();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.Option.Inventory.StoreLct store = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.Option.Inventory.Unit unit = new Blue.BL.Option.Inventory.Unit();
        private readonly Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();
        private readonly Blue.BL.IN.VendorProd vendorProd = new Blue.BL.IN.VendorProd();
        private string MsgError = string.Empty;
        private Blue.BL.GL.Account.Account account = new Blue.BL.GL.Account.Account();
        private DataSet dsProdLoc = new DataSet();
        private DataSet dsProduct = new DataSet();
        private DataSet dsStore = new DataSet();
        private DataSet dsStrMinMax = new DataSet();
        private List<GetLocationCodeKey> list = new List<GetLocationCodeKey>();
        private List<GetVendorCodeKey> listVendor = new List<GetVendorCodeKey>();
        private bool pageInit = true;


        private List<object> BuList
        {
            get { return (List<object>) ViewState["selBuList"]; }
            set { ViewState["selBuList"] = value; }
        }

        #endregion

        #region "Operations"

        /// <summary>
        ///     Gets and display season data when page load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            //if (!IsPostBack)
            //{
            //    //Page_Retrieve();
            //    //Page_Setting();
            //}
            //else
            //{
            //    //dsProduct = (DataSet)Session["dsProduct"];
            //    //dsStore   = (DataSet)Session["dsStore"];
            //    //dsProdLoc = (DataSet)Session["dsProdLoc"];
            //}
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            Page_Retrieve();
            Page_Setting();

            if (Page.IsPostBack)
            {
                pageInit = false;
            }
            else
            {
                pageInit = true;
            }

            //25/04/2012 edit by keng.
            if (dsStrMinMax.Tables[store.TableName].Rows.Count > 0)
            {
                var selectedvalues = new List<object>();

                for (var i = 0; i < grd_Store.Rows.Count; i++)
                {
                    var chk_Item = grd_Store.Rows[i].FindControl("chk_Item") as CheckBox;
                    if (chk_Item.Checked)
                    {
                        var lbl_LocationCode = grd_Store.Rows[i].FindControl("lbl_LocationCode") as Label;
                        selectedvalues.Add(lbl_LocationCode.Text);
                    }
                }

                if (dsProdLoc.Tables[prodLoc.TableName].Rows.Count > 0)
                {
                    if (dsProdLoc.Tables[prodLoc.TableName].Rows.Count == selectedvalues.Count)
                    {
                        foreach (DataRow drStore in dsStrMinMax.Tables[store.TableName].Rows)
                        {
                            var i = 0;
                            foreach (DataRow drAssigned in dsProdLoc.Tables[prodLoc.TableName].Rows)
                            {
                                if (drAssigned.RowState != DataRowState.Deleted)
                                {
                                    if (drStore["LocationCode"].ToString().ToUpper() ==
                                        drAssigned["LocationCode"].ToString().ToUpper())
                                    {
                                        var chk_Item = grd_Store.Rows[i].FindControl("chk_Item") as CheckBox;
                                        var txt_MinQty = grd_Store.Rows[i].FindControl("txt_MinQty") as TextBox;
                                        var txt_MaxQty = grd_Store.Rows[i].FindControl("txt_MaxQty") as TextBox;

                                        chk_Item.Checked = true;
                                        txt_MinQty.Enabled = true;
                                        txt_MaxQty.Enabled = true;
                                    }
                                }

                                i++;
                            }
                        }
                    }

                    if ((dsProdLoc.Tables[prodLoc.TableName].Rows.Count != selectedvalues.Count) && pageInit)
                    {
                        var j = 0;

                        foreach (DataRow drStoreAll in dsStrMinMax.Tables[store.TableName].Rows)
                        {
                            foreach (DataRow drAssigned in dsProdLoc.Tables[prodLoc.TableName].Rows)
                            {
                                if (drAssigned.RowState != DataRowState.Deleted)
                                {
                                    if (drStoreAll["LocationCode"].ToString().ToUpper() ==
                                        drAssigned["LocationCode"].ToString().ToUpper())
                                    {
                                        var chk_Item = grd_Store.Rows[j].FindControl("chk_Item") as CheckBox;
                                        var txt_MinQty = grd_Store.Rows[j].FindControl("txt_MinQty") as TextBox;
                                        var txt_MaxQty = grd_Store.Rows[j].FindControl("txt_MaxQty") as TextBox;

                                        chk_Item.Checked = true;
                                        txt_MinQty.Enabled = true;
                                        txt_MaxQty.Enabled = true;
                                    }
                                }
                            }

                            j++;
                        }

                        pageInit = false;
                    }
                    else if ((dsProdLoc.Tables[prodLoc.TableName].Rows.Count != selectedvalues.Count))
                    {
                        foreach (DataRow drStoreAll in dsStrMinMax.Tables[store.TableName].Rows)
                        {
                            var i = 0;
                            for (var k = 0; k < selectedvalues.Count; k++)
                            {
                                if (selectedvalues[k].ToString().ToUpper() ==
                                    drStoreAll["LocationCode"].ToString().ToUpper())
                                {
                                    var chk_Item = grd_Store.Rows[i].FindControl("chk_Item") as CheckBox;
                                    var txt_MinQty = grd_Store.Rows[i].FindControl("txt_MinQty") as TextBox;
                                    var txt_MaxQty = grd_Store.Rows[i].FindControl("txt_MaxQty") as TextBox;

                                    chk_Item.Checked = true;
                                    txt_MinQty.Enabled = true;
                                    txt_MaxQty.Enabled = true;
                                }

                                i++;
                            }
                        }
                    }
                }
                else
                {
                    foreach (DataRow drStoreAll in dsStrMinMax.Tables[store.TableName].Rows)
                    {
                        var i = 0;
                        for (var j = 0; j < selectedvalues.Count; j++)
                        {
                            if (selectedvalues[j].ToString().ToUpper() ==
                                drStoreAll["LocationCode"].ToString().ToUpper())
                            {
                                var chk_Item = grd_Store.Rows[i].FindControl("chk_Item") as CheckBox;
                                var txt_MinQty = grd_Store.Rows[j].FindControl("txt_MinQty") as TextBox;
                                var txt_MaxQty = grd_Store.Rows[j].FindControl("txt_MaxQty") as TextBox;

                                chk_Item.Checked = true;
                                txt_MinQty.Enabled = true;
                                txt_MaxQty.Enabled = true;
                            }

                            i++;
                        }
                    }
                }
            }

            dsProduct = (DataSet) Session["dsProduct"];
            dsStore = (DataSet) Session["dsStore"];
            dsProdLoc = (DataSet) Session["dsProdLoc"];
            dsStrMinMax = (DataSet) Session["dsStrMinMax"];
        }

        /// <summary>
        ///     Gets season data.
        /// </summary>
        private void Page_Retrieve()
        {
            // Get list by productcode.
            var getProduct = product.GetList(dsProduct, Request.Params["ID"],
                bu.GetConnectionString(Request.Params["BuCode"]));
            if (getProduct)
            {
                Session["dsProduct"] = dsProduct;
            }

            dsStore.Clear();
            dsStrMinMax.Clear();

            var getUnit = store.GetList(dsStore, bu.GetConnectionString(Request.Params["BuCode"]));

            // 2012-03-02 Version ที่มีการ Where isActive = true ของ Location.
            //bool getUnit = store.GetList2(dsStore, bu.GetConnectionString(Request.Params["BuCode"].ToString()));

            if (getUnit)
            {
                Session["dsStore"] = dsStore;
            }
            //else
            //{
            //    // Display Error Message
            //    return;
            //}

            var getProdLoc = prodLoc.GetList_ProductCode(dsProdLoc, Request.Params["ID"],
                bu.GetConnectionString(Request.Params["BuCode"]));
            if (getProdLoc)
            {
                Session["dsProdLoc"] = dsProdLoc;
            }

            var GetMinMax = store.GetListMinMax(dsStrMinMax, Request.Params["ID"],
                bu.GetConnectionString(Request.Params["BuCode"]));

            if (GetMinMax)
            {
                Session["dsStrMinMax"] = dsStrMinMax;
            }
            //else
            //{
            //    // Display Error Message
            //    return;
            //}

            Session["dsProdLoc"] = dsProdLoc;
            Session["dsProduct"] = dsProduct;
        }

        /// <summary>
        ///     Display season data.
        /// </summary>
        private void Page_Setting()
        {
            var drProduct = dsProduct.Tables[product.TableName].Rows[0];

            lbl_ItemGroup.Text = prodcat.GetName(drProduct["ProductCate"].ToString(),
                bu.GetConnectionString(Request.Params["BuCode"]));
            lbl_SubCategory.Text =
                prodcat.GetName(
                    product.GetParentNoByCategoryCode(drProduct["ProductCate"].ToString(),
                        bu.GetConnectionString(Request.Params["BuCode"])),
                    bu.GetConnectionString(Request.Params["BuCode"]));

            var dsView = new DataSet();
            prodcat.GetListByCategoryCode(dsView, drProduct["ProductCate"].ToString(),
                bu.GetConnectionString(Request.Params["BuCode"]));

            lbl_Cateogry.Text =
                prodcat.GetName(
                    product.GetParentNoByCategoryCode(dsView.Tables[prodcat.TableName].Rows[0]["ParentNo"].ToString(),
                        bu.GetConnectionString(Request.Params["BuCode"])),
                    bu.GetConnectionString(Request.Params["BuCode"]));

            lbl_Code.Text = drProduct["ProductCode"].ToString();

            if (drProduct["IsActive"].ToString() != string.Empty)
            {
                lbl_Status.Text = (Convert.ToBoolean(drProduct["IsActive"]) ? "Active" : "InActive");
            }
            else
            {
                lbl_Status.Text = "InActive";
            }

            lbl_Description1.Text = drProduct["ProductDesc1"].ToString();
            lbl_Description2.Text = drProduct["ProductDesc2"].ToString();
            lbl_BarCode.Text = drProduct["BarCode"].ToString();
            lbl_InventoryUnit.Text = unit.GetName(drProduct["InventoryUnit"].ToString(),
                bu.GetConnectionString(Request.Params["BuCode"]));
            lbl_OrderUnit.Text = unit.GetName(drProduct["OrderUnit"].ToString(),
                bu.GetConnectionString(Request.Params["BuCode"]));
            //lbl_RecipeUnit.Text         = unit.GetName(drProduct["RecipeUnit"].ToString(), bu.GetConnectionString(Request.Params["BuCode"].ToString()));
            lbl_OrderConverseRate.Text = String.Format("{0:n}", drProduct["InventoryConvOrder"]);
            //lbl_RecipeConverseRate.Text = String.Format("{0:n}", drProduct["RecipeConvInvent"]);

            if (drProduct["TAXType"].ToString() == "N")
            {
                lbl_TAXType.Text = "None";
            }
            else if (drProduct["TAXType"].ToString() == "I")
            {
                lbl_TAXType.Text = "Included";
            }
            else if (drProduct["TAXType"].ToString() == "A")
            {
                lbl_TAXType.Text = "Add";
            }

            lbl_TaxRate.Text = drProduct["TaxRate"].ToString();
            lbl_StandardCost.Text = String.Format("{0:n}", drProduct["StandardCost"]);
            //lbl_LastCost.Text    = String.Format("{0:n}", drProduct["LastCost"]);  

            // Get Last Price.
            var dsLastPrice = new DataSet();
            var getLastPrice = product.GetLastPrice(dsLastPrice, drProduct["ProductCode"].ToString(),
                bu.GetConnectionString(Request.Params["BuCode"]));

            if (getLastPrice)
            {
                if (dsLastPrice.Tables[product.TableName].Rows.Count > 0)
                {
                    var drLastPrice = dsLastPrice.Tables[product.TableName].Rows[0];

                    lbl_LastCost.Text = String.Format("{0:n}", drLastPrice["LastPrice"]);
                }
            }

            lbl_QuantityDeviation.Text = drProduct["QuantityDeviation"].ToString();
            lbl_PriceDeviation.Text = String.Format("{0:n}", drProduct["PriceDeviation"]);
            //lbl_AccountCode.Text              = drProduct["AccountCode"].ToString() + " - " + account.GetName(drProduct["AccountCode"].ToString(), bu.GetConnectionString(Request.Params["BuCode"].ToString()));
            lbl_AccountCode.Text = drProduct["TaxAccCode"].ToString();

            lbl_HeaderPriceDeviationStatus.Text = (Convert.ToBoolean(drProduct["ReqHQAppr"]) ? "Yes" : "No");

            //if (drProduct["IsRecipe"])
            //lbl_MemberInRecipe.Text             = (Convert.ToBoolean(drProduct["IsRecipe"]) == true ? "Yes" : "No");

            if (drProduct["SaleItem"] != DBNull.Value)
            {
                lbl_SaleItem.Text = (Convert.ToBoolean(drProduct["SaleItem"]) ? "Yes" : "No");
            }
            else
            {
                lbl_SaleItem.Text = "No";
            }

            lbl_ApprovalLevel.Text = drProduct["ApprovalLevel"] + " : " +
                                     apprLv.GetName(drProduct["ApprovalLevel"].ToString(),
                                         bu.GetConnectionString(Request.Params["BuCode"]));

            grd_Store.DataSource = dsStrMinMax.Tables[store.TableName];
            grd_Store.DataBind();

            // If user login at HQ visible to assign product to properties
            btn_AssignBu.Visible = LoginInfo.BuInfo.IsHQ;

            // Show message if no assign store.
            if (prodLoc.CountByProductCode(lbl_Code.Text, bu.GetConnectionString(Request.Params["BuCode"])) == 0)
            {
                lbl_MsgNoAssign.Text = "Product is not assigned to store.";
            }
            else
            {
                lbl_MsgNoAssign.Text = "";
            }

            // Display unit conversion rate.
            ProdUnit.ProductCode = drProduct["ProductCode"].ToString();
            ProdUnit.DataBind();
        }

        /// <summary>
        /// </summary>
        public class GetLocationCodeKey
        {
            public GetLocationCodeKey(string locationcode)
            {
                LocationCode = locationcode;
            }

            public string LocationCode { get; set; }
        }

        /// <summary>
        /// </summary>
        public class GetVendorCodeKey
        {
            public GetVendorCodeKey(string vendorcode)
            {
                VendorCode = vendorcode;
            }

            public string VendorCode { get; set; }
        }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Response.Redirect("ProdEdit.aspx?MODE=new&BuCode=" + Request.Params["BuCode"]);
                    //Response.Redirect("ProdEdit2.aspx?MODE=new&BuCode=" + Request.Params["BuCode"].ToString());

                    break;
                case "EDIT":
                    Response.Redirect("ProdEdit.aspx?MODE=Edit&BuCode=" + Request.Params["BuCode"] + "&ID=" +
                                      Request.Params["ID"]);
                    //Response.Redirect("ProdEdit2.aspx?MODE=Edit&BuCode=" + Request.Params["BuCode"].ToString() + "&ID=" + Request.Params["ID"].ToString());

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
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Yes_Click(object sender, EventArgs e)
        {
            dsProduct.Tables[product.TableName].Rows[0].Delete();

            var result = product.Save(dsProduct, bu.GetConnectionString(Request.Params["BuCode"]));

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
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_No_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = false;
            Response.Redirect("ProdList.aspx");
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_AssignStore_Click(object sender, EventArgs e)
        {
            if (dsStrMinMax != null)
            {
                dsStrMinMax.Clear();
            }

            pop_AssignStore.ShowOnPageLoad = true;
        }

        /// <summary>
        ///     Open popup bu list for assign product to properties.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_AssignBu_Click(object sender, EventArgs e)
        {
            var productCode = dsProduct.Tables[product.TableName].Rows[0]["ProductCode"].ToString();
            var dtBuList = bu.GetListNonHQ(LoginInfo.BuInfo.BuGrpCode);
            var dtProdBu = prodBu.GetList(productCode, bu.GetConnectionString(Request.Params["BuCode"]));

            grd_BuList.DataSource = dtBuList;
            grd_BuList.DataBind();

            grd_BUandLocate.DataSource = dtBuList;
            grd_BUandLocate.DataBind();

            pop_BuList.ShowOnPageLoad = true;
        }

        /// <summary>
        ///     Assign Product to Vendor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_AssignVendor_Click(object sender, EventArgs e)
        {
            var productCode = dsProduct.Tables[product.TableName].Rows[0]["ProductCode"].ToString();
            var dtVendorList = vendor.GetList(bu.GetConnectionString(Request.Params["BuCode"]));
            var dtVendorProd = vendorProd.GetList(productCode, bu.GetConnectionString(Request.Params["BuCode"]));

            grd_AssignProduct.DataSource = dtVendorList;
            grd_AssignProduct.DataBind();

            pop_AssignProduct.ShowOnPageLoad = true;
        }

        protected void btn_Expand_Click(object sender, ImageClickEventArgs e)
        {
            var dsStrLct = new DataSet();

            var btn_Expand = sender as ImageButton;
            var selectedRow = btn_Expand.Parent.Parent as GridViewRow;
            var p_StoreLocation = selectedRow.FindControl("p_StoreLocation") as Panel;
            var grd_StoreLct = selectedRow.FindControl("grd_StoreLocation") as GridView;
            var lbl_BuCode = selectedRow.FindControl("lbl_BuCode") as Label;

            if (p_StoreLocation.Visible)
            {
                p_StoreLocation.Visible = false;
                btn_Expand.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Plus.jpg";
            }
            else
            {
                p_StoreLocation.Visible = true;
                btn_Expand.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Minus.jpg";
            }


            var connStr = bu.GetConnectionString(lbl_BuCode.Text);


            var getStore = storeLct.GetList(dsStrLct, connStr);

            // 2012-03-02 : Change CLR Procedure to Store Procedure.
            //bool getStore = storeLct.GetList2(dsStrLct, connStr);

            if (getStore)
            {
                grd_StoreLct.DataSource = dsStrLct.Tables[storeLct.TableName];
                grd_StoreLct.DataBind();
            }
        }

        protected void grd_StoreLocation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //if (e.Row.FindControl("chk_Item") != null)
                //{
                //    string productCode = dsProduct.Tables[product.TableName].Rows[0]["ProductCode"].ToString();
                //    DataTable dtProdBu = prodBu.GetList(productCode, bu.GetConnectionString(Request.Params["BuCode"].ToString()));

                //    if (dtProdBu.Rows.Count > 0)
                //    {
                //        for (int j = 0; j < dtProdBu.Rows.Count; j++)
                //        {
                //            if (DataBinder.Eval(e.Row.DataItem, "BuCode").ToString().ToUpper() == dtProdBu.Rows[j]["BuCode"].ToString().ToUpper())
                //            {
                //                //grd_BuList.Selection.SetSelection(i, true);
                //                CheckBox chk_Item = e.Row.FindControl("chk_Item") as CheckBox;
                //                chk_Item.Checked = true;

                //                break;
                //            }
                //        }
                //    }
                //}

                if (e.Row.FindControl("lbl_LocationCode") != null)
                {
                    var lbl_LocationCode = e.Row.FindControl("lbl_LocationCode") as Label;
                    lbl_LocationCode.Text = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString();
                }

                if (e.Row.FindControl("lbl_LocationName") != null)
                {
                    var lbl_LocationName = e.Row.FindControl("lbl_LocationName") as Label;
                    lbl_LocationName.Text = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString();
                }
            }
        }

        #region "Assign TO Store"

        private void AssingUnSelection()
        {
            if (dsProdLoc.Tables[prodLoc.TableName].Rows.Count > 0)
            {
                //grd_Store.Selection.UnselectAll();
                for (var i = 0; i < grd_Store.Rows.Count; i++)
                {
                    var chk_Item = grd_Store.Rows[i].FindControl("chk_Item") as CheckBox;
                    var txt_MinQty = grd_Store.Rows[i].FindControl("txt_MinQty") as TextBox;
                    var txt_MaxQty = grd_Store.Rows[i].FindControl("txt_MaxQty") as TextBox;

                    chk_Item.Checked = false;
                    txt_MinQty.Text = string.Empty;
                    txt_MaxQty.Text = string.Empty;
                }

                var j = 0;
                //foreach (DataRow drStore in dsStore.Tables[store.TableName].Rows)
                foreach (DataRow drStore in dsStrMinMax.Tables[store.TableName].Rows)
                {
                    foreach (DataRow drAssigned in dsProdLoc.Tables[prodLoc.TableName].Rows)
                    {
                        //List<object> selectedvalues = grd_Store.GetSelectedFieldValues(new string[] { grd_Store.KeyFieldName });

                        if (drStore["LocationCode"].ToString().ToUpper() ==
                            drAssigned["LocationCode"].ToString().ToUpper())
                        {
                            var chk_Item = grd_Store.Rows[j].FindControl("chk_Item") as CheckBox;
                            var txt_MinQty = grd_Store.Rows[j].FindControl("txt_MinQty") as TextBox;
                            var txt_MaxQty = grd_Store.Rows[j].FindControl("txt_MaxQty") as TextBox;

                            chk_Item.Checked = false;
                            txt_MinQty.Text = string.Empty;
                            txt_MaxQty.Text = string.Empty;
                            txt_MinQty.Enabled = false;
                            txt_MaxQty.Enabled = false;
                        }
                    }

                    j++;
                }
            }
            else
            {
                for (var i = 0; i < grd_Store.Rows.Count; i++)
                {
                    var chk_Item = grd_Store.Rows[i].FindControl("chk_Item") as CheckBox;
                    var txt_MinQty = grd_Store.Rows[i].FindControl("txt_MinQty") as TextBox;
                    var txt_MaxQty = grd_Store.Rows[i].FindControl("txt_MaxQty") as TextBox;

                    chk_Item.Checked = false;
                    txt_MinQty.Text = string.Empty;
                    txt_MaxQty.Text = string.Empty;
                    txt_MinQty.Enabled = false;
                    txt_MaxQty.Enabled = false;
                }
            }
        }

        /// <summary>
        ///     Create New Unit
        /// </summary>
        private void CreateStore()
        {
            foreach (DataRow drDeleting in dsProdLoc.Tables[prodLoc.TableName].Rows)
            {
                if (drDeleting.RowState != DataRowState.Deleted)
                {
                    if (drDeleting["ProductCode"].ToString().ToUpper() == lbl_Code.Text.ToUpper())
                    {
                        drDeleting.Delete();
                    }
                }
            }

            if (grd_Store.Rows.Count > 0)
            {
                for (var j = 0; j <= grd_Store.Rows.Count - 1; j++)
                {
                    var chk_Item = grd_Store.Rows[j].FindControl("chk_Item") as CheckBox;
                    var lbl_LocationCode = grd_Store.Rows[j].FindControl("lbl_LocationCode") as Label;
                    var txt_MinQty = grd_Store.Rows[j].FindControl("txt_MinQty") as TextBox;
                    var txt_MaxQty = grd_Store.Rows[j].FindControl("txt_MaxQty") as TextBox;

                    if (chk_Item.Checked)
                    {
                        var drProdLoc = dsProdLoc.Tables[prodLoc.TableName].NewRow();

                        txt_MaxQty.Enabled = true;
                        txt_MinQty.Enabled = true;

                        drProdLoc["LocationCode"] = lbl_LocationCode.Text;
                        drProdLoc["ProductCode"] = lbl_Code.Text;
                        drProdLoc["OnHand"] = 0;

                        if (txt_MinQty.Text != string.Empty)
                        {
                            drProdLoc["MinQty"] = String.Format("{0:N}", txt_MinQty.Text);
                        }
                        else
                        {
                            drProdLoc["MinQty"] = DBNull.Value;
                        }

                        if (txt_MinQty.Text != string.Empty)
                        {
                            drProdLoc["MaxQty"] = String.Format("{0:N}", txt_MinQty.Text);
                        }
                        else
                        {
                            drProdLoc["MaxQty"] = DBNull.Value;
                        }

                        //if (txt_MinQty.Text != string.Empty)
                        //{
                        //    drProdLoc["MinQty"] = String.Format("{0:N}", txt_MinQty.Text);
                        //}
                        //else
                        //{
                        //    pop_AlertMinMaxEmpty.ShowOnPageLoad = true;
                        //    lbl_AlertMinMaxEmpty.Text = "Min. Qty can not be empty.";
                        //    return;
                        //}

                        //if (txt_MaxQty.Text != string.Empty)
                        //{
                        //    drProdLoc["MaxQty"] = String.Format("{0:N}", txt_MaxQty.Text);
                        //}
                        //else
                        //{
                        //    pop_AlertMinMaxEmpty.ShowOnPageLoad = true;
                        //    lbl_AlertMinMaxEmpty.Text = "Max. Qty can not be empty.";
                        //    return;
                        //}

                        //if (decimal.Parse(txt_MinQty.Text) > decimal.Parse(txt_MaxQty.Text))
                        //{
                        //    pop_AlertMinMax.ShowOnPageLoad = true;
                        //    return;
                        //}

                        // Add new record
                        dsProdLoc.Tables[prodLoc.TableName].Rows.Add(drProdLoc);
                    }
                    txt_MinQty.Text = string.Empty;
                    txt_MaxQty.Text = string.Empty;
                }
            }

            // Save to database
            var saveProdLoc = prodLoc.Save(dsProdLoc, bu.GetConnectionString(Request.Params["BuCode"]));

            if (saveProdLoc)
            {
                pop_AssignStore.ShowOnPageLoad = false;
                pop_AlertMinMaxSave.ShowOnPageLoad = true;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menuStore_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":
                    CreateStore();
                    break;

                case "CLOSE":
                    //Mdlp_StorePopup.Hide();
                    AssingUnSelection();
                    break;
            }
        }

        /// <summary>
        ///     Re-binding Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void grd_Store_OnLoad(object sender, EventArgs e)
        //{
        //    //int aacount = Convert.ToInt32(grd_Store.Selection.Count.ToString());
        //       //List<object> values = grd_Store.GetSelectedFieldValues(new string[] { grd_Store.KeyFieldName });
        //    //25/04/2012
        //    if (dsStore != null)
        //    {
        //        //check selected rows
        //        List<object> values = new List<object>();
        //        for (int i = 0; i < grd_Store.Rows.Count; i++)
        //        {
        //            CheckBox chk_Item = grd_Store.Rows[i].FindControl("chk_Item") as CheckBox;
        //            if (chk_Item.Checked)
        //            {
        //                Label lbl_LocationCode = grd_Store.Rows[i].FindControl("lbl_LocationCode") as Label;
        //                values.Add(lbl_LocationCode.Text);
        //            }
        //        }
        //        for (int i = 0; i < values.Count; i++)
        //        {
        //            int j = 0;
        //            foreach (DataRow drAssign in dsStore.Tables[store.TableName].Rows)
        //            {
        //                if (drAssign.RowState != DataRowState.Deleted)
        //                {
        //                    if (drAssign["LocationCode"].ToString().ToUpper() == values[i].ToString().ToUpper())
        //                    {
        //                        //grd_Store.Selection.SelectRowByKey(values[i].ToString());
        //                        //string LocationCode = values[i].ToString(); //grd_Store.GetRowValues(i, "LocationCode").ToString();
        //                        //list.Add(new GetLocationCodeKey(LocationCode));
        //                        //set select row
        //                        CheckBox chk_Item = grd_Store.Rows[j].FindControl("chk_Item") as CheckBox;
        //                        TextBox txt_MinQty = grd_Store.Rows[j].FindControl("txt_MinQty") as TextBox;
        //                        TextBox txt_MaxQty = grd_Store.Rows[j].FindControl("txt_MaxQty") as TextBox;
        //                        chk_Item.Checked = true;
        //                        txt_MinQty.Enabled = true;
        //                        txt_MaxQty.Enabled = true;
        //                        string LocationCode = values[i].ToString();
        //                        list.Add(new GetLocationCodeKey(LocationCode));
        //                    }
        //                }
        //                j++;
        //            }
        //        }
        //    }
        //}
        protected void grd_Store_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_LocationCode") != null)
                {
                    var lbl_LocationCode = e.Row.FindControl("lbl_LocationCode") as Label;
                    lbl_LocationCode.Text = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString();
                }

                if (e.Row.FindControl("lbl_LocationName") != null)
                {
                    var lbl_LocationName = e.Row.FindControl("lbl_LocationName") as Label;
                    lbl_LocationName.Text = store.GetName(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(),
                        bu.GetConnectionString(Request.Params["BuCode"]));
                    //2012-03-02: Change CLR to Store Location 
                    //store.GetName2(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(), bu.GetConnectionString(Request.Params["BuCode"].ToString()));
                }

                if (e.Row.FindControl("txt_MinQty") != null)
                {
                    var txt_MinQty = e.Row.FindControl("txt_MinQty") as TextBox;
                    txt_MinQty.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "MinQty"));
                }

                if (e.Row.FindControl("txt_MaxQty") != null)
                {
                    var txt_MaxQty = e.Row.FindControl("txt_MaxQty") as TextBox;
                    txt_MaxQty.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "MaxQty"));
                }
            }
        }

        /// <summary>
        ///     Save Assign Product To Store
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Save_Click(object sender, EventArgs e)
        {
            CreateStore();
        }

        protected void btn_Clear_Click(object sender, EventArgs e)
        {
            AssingUnSelection();
        }

        protected void chk_Item_CheckedChanged(object sender, EventArgs e)
        {
            for (var j = 0; j <= grd_Store.Rows.Count - 1; j++)
            {
                var chk_Item = grd_Store.Rows[j].FindControl("chk_Item") as CheckBox;
                var txt_MinQty = grd_Store.Rows[j].FindControl("txt_MinQty") as TextBox;
                var txt_MaxQty = grd_Store.Rows[j].FindControl("txt_MaxQty") as TextBox;

                if (chk_Item.Checked)
                {
                    txt_MaxQty.Enabled = true;
                    txt_MinQty.Enabled = true;
                }
                else
                {
                    txt_MaxQty.Enabled = false;
                    txt_MinQty.Enabled = false;
                }
            }
        }

        protected void btn_AlertMinMaxSave_Click(object sender, EventArgs e)
        {
            // Show message if no assign store.
            if (prodLoc.CountByProductCode(lbl_Code.Text, bu.GetConnectionString(Request.Params["BuCode"])) == 0)
            {
                lbl_MsgNoAssign.Text = "Product is not assigned to store.";
            }
            else
            {
                lbl_MsgNoAssign.Text = "";
            }

            pop_AlertMinMaxSave.ShowOnPageLoad = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ConfrimDeleteStore_Click(object sender, EventArgs e)
        {
            //List<object> columnValues = grd_Store.GetSelectedFieldValues("UnitCode");
            var columnValues = new List<object>();

            for (var i = 0; i < grd_Store.Rows.Count; i++)
            {
                var chk_Item = grd_Store.Rows[i].FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    columnValues.Add(dsStore.Tables[store.TableName].Rows[i]["UnitCode"]);
                }
            }

            foreach (DataRow drDeleting in dsStore.Tables[store.TableName].Rows)
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
            var saveUnit = unit.Save(dsStore, bu.GetConnectionString(Request.Params["BuCode"]));

            if (saveUnit)
            {
                Page_Retrieve();
            }
        }

        protected void btn_AlertOK_Click(object sender, EventArgs e)
        {
            pop_AlertMinMax.ShowOnPageLoad = false;
        }

        protected void btn_AlertMinMaxEmpty_Click(object sender, EventArgs e)
        {
            pop_AlertMinMaxEmpty.ShowOnPageLoad = false;
        }

        #endregion

        #region "Assign TO Property"

        /// <summary>
        ///     Insert/Update the product to selected BU
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Pop_BuList_OK_Click(object sender, EventArgs e)
        {
            // Get current product code.
            var productCode = dsProduct.Tables[product.TableName].Rows[0]["ProductCode"].ToString();

            var selBuList = new List<object>();
            var delBuList = new List<object>();

            var dsProdBu = new DataSet();
            var connStr = string.Empty;

            for (var i = 0; i < grd_BuList.Rows.Count; i++)
            {
                var chk_Item = grd_BuList.Rows[i].FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    var lbl_BuCode = grd_BuList.Rows[i].FindControl("lbl_BuCode") as Label;
                    selBuList.Add(lbl_BuCode.Text);
                }
                else
                {
                    var lbl_BuCode = grd_BuList.Rows[i].FindControl("lbl_BuCode") as Label;
                    delBuList.Add(lbl_BuCode.Text);
                }
            }

            //// Check for delete data when uncheck bu code. กรณีที่มี code ใน Local อยู่แล้ว ต้อง update ข้อมูลให้ตรงกับ HQ ด้วยหรือเปล่า??            
            //foreach (string delBu in delBuList)
            //{
            //    DataSet dsDel = new DataSet();

            //    // Get connection string of selected BU
            //    string connStr = bu.GetConnectionString(delBu);

            //    // Get Product data
            //    bool getDel = product.Get(dsDel, productCode, connStr);

            //    if (getDel)
            //    {
            //        foreach (DataRow drProd in dsDel.Tables[product.TableName].Rows)
            //        {
            //            drProd.Delete();
            //        }

            //        product.Save(dsDel, connStr);
            //    }
            //}

            ViewState["selBuList"] = selBuList;


            var strBU = string.Empty;

            foreach (string selBu in selBuList)
            {
                var dsProd = new DataSet();

                // Get connection string of selected BU
                connStr = bu.GetConnectionString(selBu);

                // Get Product data
                var getProduct = product.Get(dsProd, productCode, connStr);

                if (getProduct)
                {
                    // Get current product data.
                    var drProduct = dsProduct.Tables[product.TableName].Rows[0];

                    if (dsProd.Tables[product.TableName].Rows.Count > 0)
                    {
                        //pop_WarningReplace.ShowOnPageLoad = true;

                        if (strBU == string.Empty)
                        {
                            strBU = selBu;
                        }
                        else
                        {
                            strBU += ", " + selBu;
                        }
                    }
                }
            }

            if (strBU != string.Empty)
            {
                pop_WarningReplace.ShowOnPageLoad = true;
                lbl_Warning.Text = "There is already in <br />" + strBU + "<br /> Do you want to replace data?";
            }
            else
            {
                saveAssignToBu();
            }
        }

        /// <summary>
        ///     Cancel assign product to properties
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Pop_BuList_Cancel_Click(object sender, EventArgs e)
        {
            pop_BuList.ShowOnPageLoad = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_BuList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("chk_Item") != null)
                {
                    var productCode = dsProduct.Tables[product.TableName].Rows[0]["ProductCode"].ToString();
                    var dtProdBu = prodBu.GetList(productCode, bu.GetConnectionString(Request.Params["BuCode"]));

                    if (dtProdBu.Rows.Count > 0)
                    {
                        for (var j = 0; j < dtProdBu.Rows.Count; j++)
                        {
                            if (DataBinder.Eval(e.Row.DataItem, "BuCode").ToString().ToUpper() ==
                                dtProdBu.Rows[j]["BuCode"].ToString().ToUpper())
                            {
                                //grd_BuList.Selection.SetSelection(i, true);
                                var chk_Item = e.Row.FindControl("chk_Item") as CheckBox;
                                chk_Item.Checked = true;

                                break;
                            }
                        }
                    }
                }

                if (e.Row.FindControl("lbl_BuCode") != null)
                {
                    var lbl_BuCode = e.Row.FindControl("lbl_BuCode") as Label;
                    lbl_BuCode.Text = DataBinder.Eval(e.Row.DataItem, "BuCode").ToString();
                }

                if (e.Row.FindControl("lbl_BuName") != null)
                {
                    var lbl_BuName = e.Row.FindControl("lbl_BuName") as Label;
                    lbl_BuName.Text = DataBinder.Eval(e.Row.DataItem, "BuName").ToString();
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_WarningYes_Click(object sender, EventArgs e)
        {
            saveAssignToBu();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_WarningNo_Click(object sender, EventArgs e)
        {
            pop_WarningReplace.ShowOnPageLoad = false;
        }

        private void saveAssignToBu()
        {
            var BUList = (List<object>) ViewState["selBuList"];
            var strBU = string.Empty;
            var connStr = string.Empty;
            var productCode = dsProduct.Tables[product.TableName].Rows[0]["ProductCode"].ToString();
            var dsProdBu = new DataSet();

            foreach (string selBu in BUList)
            {
                var dsProd = new DataSet();

                // Get connection string of selected BU
                connStr = bu.GetConnectionString(selBu);

                // Get Product data
                var getProduct = product.Get(dsProd, productCode, connStr);

                if (getProduct)
                {
                    var drProduct = dsProduct.Tables[product.TableName].Rows[0];

                    //2012-03-06 : Check table Unit for add unit in other property.
                    //int intUnit = CheckUnit(drProduct, connStr);

                    // Get current product data.
                    if (dsProd.Tables[product.TableName].Rows.Count > 0)
                    {
                        // Update exist product data.                        
                        var drUpdating = dsProd.Tables[product.TableName].Rows[0];

                        drUpdating["ProductDesc1"] = drProduct["ProductDesc1"];
                        drUpdating["ProductDesc2"] = drProduct["ProductDesc2"];
                        drUpdating["ProductCate"] = drProduct["ProductCate"];
                        drUpdating["ProductSubCate"] = drProduct["ProductSubCate"];
                        drUpdating["BarCode"] = drProduct["BarCode"];
                        drUpdating["RecipeUnit"] = drProduct["RecipeUnit"];
                        drUpdating["RecipeConvInvent"] = drProduct["RecipeConvInvent"];
                        drUpdating["InventoryUnit"] = drProduct["InventoryUnit"];
                        drUpdating["InventoryConvOrder"] = drProduct["InventoryConvOrder"];
                        drUpdating["OrderUnit"] = drProduct["OrderUnit"];
                        drUpdating["TaxType"] = drProduct["TaxType"];
                        drUpdating["TaxRate"] = drProduct["TaxRate"];
                        drUpdating["StandardCost"] = drProduct["StandardCost"];
                        drUpdating["LastCost"] = drProduct["LastCost"];
                        drUpdating["QuantityDeviation"] = drProduct["QuantityDeviation"];
                        drUpdating["PriceDeviation"] = drProduct["PriceDeviation"];
                        drUpdating["ReqHQAppr"] = drProduct["ReqHQAppr"];
                        drUpdating["IsActive"] = drProduct["IsActive"];
                        drUpdating["AccountCode"] = drProduct["AccountCode"];
                        drUpdating["IsRecipe"] = drProduct["IsRecipe"];
                        drUpdating["SaleItem"] = drProduct["SaleItem"];
                        drUpdating["ApprovalLevel"] = drProduct["ApprovalLevel"];
                        drUpdating["UpdatedDate"] = ServerDateTime;
                        drUpdating["UpdatedBy"] = LoginInfo.LoginName;
                        drUpdating["TaxAccCode"] = drProduct["TaxAccCode"];
                    }
                    else
                    {
                        // Insert product data.
                        var drInserting = dsProd.Tables[product.TableName].NewRow();

                        drInserting["ProductCode"] = drProduct["ProductCode"];
                        drInserting["ProductDesc1"] = drProduct["ProductDesc1"];
                        drInserting["ProductDesc2"] = drProduct["ProductDesc2"];
                        drInserting["ProductCate"] = drProduct["ProductCate"];
                        drInserting["ProductSubCate"] = drProduct["ProductSubCate"];
                        drInserting["BarCode"] = drProduct["BarCode"];
                        drInserting["RecipeUnit"] = drProduct["RecipeUnit"];
                        drInserting["RecipeConvInvent"] = drProduct["RecipeConvInvent"];
                        drInserting["InventoryUnit"] = drProduct["InventoryUnit"];
                        drInserting["InventoryConvOrder"] = drProduct["InventoryConvOrder"];
                        drInserting["OrderUnit"] = drProduct["OrderUnit"];
                        drInserting["TaxType"] = drProduct["TaxType"];
                        drInserting["TaxRate"] = drProduct["TaxRate"];
                        drInserting["StandardCost"] = drProduct["StandardCost"];
                        drInserting["LastCost"] = drProduct["LastCost"];
                        drInserting["QuantityDeviation"] = drProduct["QuantityDeviation"];
                        drInserting["PriceDeviation"] = drProduct["PriceDeviation"];
                        drInserting["ReqHQAppr"] = drProduct["ReqHQAppr"];
                        drInserting["IsActive"] = drProduct["IsActive"];
                        drInserting["AccountCode"] = drProduct["AccountCode"];
                        drInserting["IsRecipe"] = drProduct["IsRecipe"];
                        drInserting["SaleItem"] = drProduct["SaleItem"];
                        drInserting["ApprovalLevel"] = drProduct["ApprovalLevel"];
                        drInserting["CreatedDate"] = ServerDateTime;
                        drInserting["CreatedBy"] = LoginInfo.LoginName;
                        drInserting["UpdatedDate"] = ServerDateTime;
                        drInserting["UpdatedBy"] = LoginInfo.LoginName;
                        drInserting["TaxAccCode"] = drProduct["TaxAccCode"];

                        dsProd.Tables[product.TableName].Rows.Add(drInserting);

                        //// Save product data.
                        //bool result = product.Save(dsProd, connStr);

                        //if (result)
                        //{
                        //    //มีการบันทึก Product ลงแต่ละ Local ที่ได้ Assign ไว้
                        //}
                        //else
                        //{
                        //    // Display Error
                        //}
                    }

                    // Save product data.
                    var result = product.Save(dsProd, connStr);

                    if (result)
                    {
                        //มีการบันทึก Product ลงแต่ละ Local ที่ได้ Assign ไว้
                    }
                }
            }

            // Get current assigned data to prodBu.
            var chkProdBU = prodBu.GetList(dsProdBu, productCode, bu.GetConnectionString(Request.Params["BuCode"]));
            if (!chkProdBU)
            {
                return;
            }

            // Delete all assigned
            foreach (DataRow drProdBu in dsProdBu.Tables[prodBu.TableName].Rows)
            {
                drProdBu.Delete();
            }

            // Insert new assingn
            foreach (string selProdBu in BUList)
            {
                var drNew = dsProdBu.Tables[prodBu.TableName].NewRow();
                drNew["BuCode"] = selProdBu;
                drNew["ProductCode"] = productCode;
                dsProdBu.Tables[prodBu.TableName].Rows.Add(drNew);
            }

            // Save change to database
            var saved = prodBu.Save(dsProdBu, bu.GetConnectionString(Request.Params["BuCode"]));

            if (saved)
            {
                pop_BuList.ShowOnPageLoad = false;
                pop_Message.ShowOnPageLoad = true;
                lbl_Message.Text = "This product assign to selected business unit successfull";
                pop_WarningReplace.ShowOnPageLoad = false;
            }
        }

        private int CheckUnit(DataRow drProduct, string ConnStr)
        {
            var strFindUnit = string.Empty;

            // Check Unit in other Unit.
            if (drProduct["RecipeUnit"] != null)
            {
                strFindUnit = "'" + drProduct["RecipeUnit"] + "'";
            }

            if (drProduct["InventoryUnit"] != null)
            {
                if (strFindUnit != string.Empty)
                {
                    strFindUnit = ", '" + drProduct["InventoryUnit"] + "'";
                }
                else
                {
                    strFindUnit = "'" + drProduct["InventoryUnit"] + "'";
                }
            }

            if (drProduct["OrderUnit"] != null)
            {
                if (strFindUnit != string.Empty)
                {
                    strFindUnit = ", '" + drProduct["OrderUnit"] + "'";
                }
                else
                {
                    strFindUnit = "'" + drProduct["OrderUnit"] + "'";
                }
            }

            return 0;
        }

        #endregion

        #region "Assign TO Vendor"

        protected void grd_AssignProduct_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("chk_Item") != null)
                {
                    var productCode = dsProduct.Tables[product.TableName].Rows[0]["ProductCode"].ToString();
                    var dtVendorProd = vendorProd.GetList(productCode, bu.GetConnectionString(Request.Params["BuCode"]));

                    if (dtVendorProd.Rows.Count > 0)
                    {
                        for (var j = 0; j < dtVendorProd.Rows.Count; j++)
                        {
                            if (DataBinder.Eval(e.Row.DataItem, "VendorCode").ToString().ToUpper() ==
                                dtVendorProd.Rows[j]["VendorCode"].ToString().ToUpper())
                            {
                                //grd_BuList.Selection.SetSelection(i, true);
                                var chk_Item = e.Row.FindControl("chk_Item") as CheckBox;
                                chk_Item.Checked = true;

                                break;
                            }
                        }
                    }
                }

                if (e.Row.FindControl("lbl_VendorCode") != null)
                {
                    var lbl_VendorCode = e.Row.FindControl("lbl_VendorCode") as Label;
                    lbl_VendorCode.Text = DataBinder.Eval(e.Row.DataItem, "VendorCode").ToString();
                }

                if (e.Row.FindControl("lbl_VendorName") != null)
                {
                    var lbl_VendorName = e.Row.FindControl("lbl_VendorName") as Label;
                    lbl_VendorName.Text = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Pop_Vendor_OK_Click(object sender, EventArgs e)
        {
            var dsVendorProd = new DataSet();

            // Get current product code.
            var productCode = dsProduct.Tables[product.TableName].Rows[0]["ProductCode"].ToString();

            var result = vendorProd.GetList(dsVendorProd, productCode, bu.GetConnectionString(Request.Params["BuCode"]));

            if (!result)
            {
                return;
            }

            //List<object> selVendorLst = grd_AssignProduct.GetSelectedFieldValues("VendorCode");
            var selVendorLst = new List<object>();

            for (var i = 0; i < grd_AssignProduct.Rows.Count; i++)
            {
                var chk_Item = grd_AssignProduct.Rows[i].FindControl("chk_Item") as CheckBox;
                if (chk_Item.Checked)
                {
                    var lbl_VendorCode = grd_AssignProduct.Rows[i].FindControl("lbl_VendorCode") as Label;
                    selVendorLst.Add(lbl_VendorCode.Text);
                }
            }

            foreach (string selVendor in selVendorLst)
            {
                var drNew = dsVendorProd.Tables[vendorProd.TableName].NewRow();

                drNew["ProductCode"] = productCode;
                drNew["VendorCode"] = selVendor;

                dsVendorProd.Tables[vendorProd.TableName].Rows.Add(drNew);
            }

            //save change to database.
            var save = vendorProd.Save(dsVendorProd, bu.GetConnectionString(Request.Params["BuCode"]));
            if (save)
            {
                pop_AssignProduct.ShowOnPageLoad = false;
                pop_Message.ShowOnPageLoad = true;
                lbl_Message.Text = "This product assign to selected vendor successfull";
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Pop_Vendor_Cancel_Click(object sender, EventArgs e)
        {
            pop_AssignProduct.ShowOnPageLoad = false;
        }

        #endregion
    }
}