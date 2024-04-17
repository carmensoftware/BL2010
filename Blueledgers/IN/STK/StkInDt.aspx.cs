using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.STK
{
    public partial class StkInDt : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();
        private readonly Blue.BL.Option.Inventory.StoreLct StrLct = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.IN.AdjType adjType = new Blue.BL.IN.AdjType();
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.Option.Inventory.Product prod = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private readonly Blue.BL.IN.StockIn stockIn = new Blue.BL.IN.StockIn();
        private readonly Blue.BL.IN.StockInDt stockInDt = new Blue.BL.IN.StockInDt();

        private string StkId = string.Empty;
        private decimal Total;
        private DataSet dsStockIn = new DataSet();
        private Blue.BL.IN.Inventory inv = new Blue.BL.IN.Inventory();
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly string moduleID = "3.2";
        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_ConnStr.Value = bu.GetConnectionString(Request.Params["BuCode"]);
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsStockIn = (DataSet)Session["dsStockIn"];
            }
        }

        private void Page_Retrieve()
        {
            dsStockIn.Clear();

            if (!string.IsNullOrEmpty(Request.Params["ID"]))
            {
                StkId = Request.Params["ID"];

                var get = stockIn.Get(dsStockIn, StkId, hf_ConnStr.Value);

                if (!get)
                {
                    return;
                }

                var getDt = stockInDt.Get(dsStockIn, StkId, hf_ConnStr.Value);

                if (!getDt)
                {
                    return;
                }
            }

            Session["dsStockIn"] = dsStockIn;
            Page_Setting();
        }

        private void Page_Setting()
        {
            var drStkIn = dsStockIn.Tables[stockIn.TableName].Rows[0];

            lbl_Date.Text = DateTime.Parse(drStkIn["CreateDate"].ToString()).ToString("dd/MM/yyyy");
            lbl_Date.ToolTip = lbl_Date.Text;
            lbl_Ref.Text = drStkIn["RefId"].ToString();
            lbl_Ref.ToolTip = lbl_Ref.Text;
            lbl_Status.Text = drStkIn["Status"].ToString();
            lbl_Status.ToolTip = lbl_Status.Text;

            if (drStkIn["CommitDate"].ToString() != string.Empty)
            {
                lbl_CommitDate.Text = String.Format("{0:dd/MM/yyyy}", drStkIn["CommitDate"]);
                lbl_CommitDate.ToolTip = lbl_CommitDate.Text;
            }

            //Get AdjCode+Name
            var adjText = string.Empty;
            var dsAdj = new DataSet();
            var b = adjType.GetList(dsAdj, "STOCK IN", hf_ConnStr.Value);
            if (dsAdj.Tables[adjType.TableName].Rows.Count > 0)
            {
                var dtAdj = dsAdj.Tables[adjType.TableName].Select("AdjId = " + drStkIn["Type"]);
                if (dtAdj.Count() > 0)
                {
                    adjText = dtAdj[0]["AdjCode"] + " : " + dtAdj[0]["AdjName"];
                }
            }

            lbl_Type.Text = adjText;
            lbl_Type.ToolTip = lbl_Type.Text;
            lbl_Desc.Text = drStkIn["Description"].ToString();
            lbl_Desc.ToolTip = lbl_Desc.Text;

            // Set Enable and Disable of Button Edit and void.
            if (drStkIn["Status"].ToString() != string.Empty)
            {
                if (drStkIn["Status"].ToString() == "Voided" || drStkIn["Status"].ToString() == "Committed")
                {
                    menu_CmdBar.Items[1].Visible = false; // Edit
                    menu_CmdBar.Items[2].Visible = false; // Void
                }
                else
                {
                    menu_CmdBar.Items[1].Visible = true; // Edit
                    menu_CmdBar.Items[2].Visible = true; // Void
                }
            }

            // Display Comment
            var comment = (UserControls.Comment2)Master.FindControl("Comment");
            comment.Module = "IN";
            comment.SubModule = "STKIN";
            comment.RefNo = dsStockIn.Tables[stockIn.TableName].Rows[0]["RefId"].ToString();
            comment.BuCode = Request.Params["BuCode"];
            comment.Visible = true;
            comment.DataBind();

            // Display Attach
            var attach = (PL.UserControls.Attach2)Master.FindControl("Attach");
            attach.BuCode = Request.Params["BuCode"];
            attach.ModuleName = "STKIN";
            attach.RefNo = dsStockIn.Tables[stockIn.TableName].Rows[0]["RefId"].ToString();
            ;
            attach.Visible = true;
            attach.DataBind();

            // Display Log
            var log = (PL.UserControls.Log2)Master.FindControl("Log");
            log.Module = "IN";
            log.SubModule = "STKIN";
            log.RefNo = dsStockIn.Tables[stockIn.TableName].Rows[0]["RefId"].ToString();
            ;
            log.Visible = true;
            log.DataBind();

            Control_HeaderMenuBar();
        }

        private void Control_HeaderMenuBar()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            menu_CmdBar.Items.FindByName("Create").Visible = (pagePermiss >= 3) ? menu_CmdBar.Items.FindByName("Create").Visible : false;
            menu_CmdBar.Items.FindByName("Edit").Visible = (pagePermiss >= 3) ? menu_CmdBar.Items.FindByName("Edit").Visible : false;
            menu_CmdBar.Items.FindByName("Void").Visible = (pagePermiss >= 7) ? menu_CmdBar.Items.FindByName("Void").Visible : false;
        }

        protected void grd_StkIn_Dt_Load(object sender, EventArgs e)
        {
            var dsStock = new DataSet();

            var getStockList = stockInDt.Get(dsStock, Request.Params["ID"], hf_ConnStr.Value);

            grd_StkIn_Dt.DataSource = dsStock.Tables[stockInDt.TableName];
            grd_StkIn_Dt.DataBind();
        }

        protected void grd_StkIn_Dt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //if (e.Row.FindControl("lbl_StoreID") != null)
                //{
                //    Label lbl_StoreID = e.Row.FindControl("lbl_StoreID") as Label;
                //    lbl_StoreID.Text = DataBinder.Eval(e.Row.DataItem, "StoreId").ToString();
                //}

                if (e.Row.FindControl("lbl_StoreName") != null)
                {
                    var lbl_StoreName = e.Row.FindControl("lbl_StoreName") as Label;
                    lbl_StoreName.Text = DataBinder.Eval(e.Row.DataItem, "StoreId") + " : " +
                                         StrLct.GetName(DataBinder.Eval(e.Row.DataItem, "StoreId").ToString(),
                                             hf_ConnStr.Value);
                    //--2012-03-02--> StrLct.GetName2(DataBinder.Eval(e.Row.DataItem,"StoreId").ToString(),LoginInfo.ConnStr);
                    lbl_StoreName.ToolTip = lbl_StoreName.Text;
                }

                //if (e.Row.FindControl("lbl_ProductCode") != null)
                //{
                //    Label lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                //    lbl_ProductCode.Text = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();
                //}

                //if (e.Row.FindControl("lbl_EngishName") != null)
                //{
                //    Label lbl_EnglishName = e.Row.FindControl("lbl_EnglishName") as Label;
                //    lbl_EnglishName.Text = DataBinder.Eval(e.Row.DataItem, "Descen").ToString();
                //}

                if (e.Row.FindControl("lbl_Item_Desc") != null)
                {
                    var lbl_Item_Desc = e.Row.FindControl("lbl_Item_Desc") as Label;
                    lbl_Item_Desc.Text = DataBinder.Eval(e.Row.DataItem, "SKU") + " : " +
                                         prod.GetName(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(),
                                             hf_ConnStr.Value) + " : " +
                                         prod.GetName2(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(),
                                             hf_ConnStr.Value);
                    lbl_Item_Desc.ToolTip = lbl_Item_Desc.Text;
                }

                //if (e.Row.FindControl("lbl_LocalName") != null)
                //{
                //    Label lbl_LocalName = e.Row.FindControl("lbl_LocalName") as Label;
                //    lbl_LocalName.Text = prod.GetName2(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(), LoginInfo.ConnStr);
                //}

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                    lbl_Unit.ToolTip = lbl_Unit.Text;
                }

                if (e.Row.FindControl("lbl_Qty") != null)
                {
                    var lbl_Qty = e.Row.FindControl("lbl_Qty") as Label;
                    lbl_Qty.Text = string.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "Qty"));
                    Total += decimal.Parse((lbl_Qty.Text == string.Empty ? "0" : lbl_Qty.Text));
                    lbl_Qty.ToolTip = Total.ToString();
                }

                if (e.Row.FindControl("lbl_UnitCost") != null)
                {
                    var lbl_UnitCost = e.Row.FindControl("lbl_UnitCost") as Label;
                    lbl_UnitCost.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "UnitCost"));
                    lbl_UnitCost.ToolTip = lbl_UnitCost.Text;
                }

                // ****************** Display Transaction Detail ******************
                if (e.Row.FindControl("lbl_SKU_Ex") != null)
                {
                    var lbl_SKU_Ex = e.Row.FindControl("lbl_SKU_Ex") as Label;
                    lbl_SKU_Ex.Text = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();
                }

                if (e.Row.FindControl("lbl_ItemGroup_Ex") != null)
                {
                    var lbl_ItemGroup_Ex = e.Row.FindControl("lbl_ItemGroup_Ex") as Label;
                    lbl_ItemGroup_Ex.Text =
                        prodCat.GetName(
                            prod.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(), hf_ConnStr.Value),
                            hf_ConnStr.Value);
                    lbl_ItemGroup_Ex.ToolTip = lbl_ItemGroup_Ex.Text;
                }

                if (e.Row.FindControl("lbl_SubCategory_Ex") != null)
                {
                    var lbl_SubCategory_Ex = e.Row.FindControl("lbl_SubCategory_Ex") as Label;
                    lbl_SubCategory_Ex.Text =
                        prodCat.GetName(
                            prod.GetParentNoByCategoryCode(
                                prod.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(),
                                    hf_ConnStr.Value), hf_ConnStr.Value), hf_ConnStr.Value);
                    lbl_SubCategory_Ex.ToolTip = lbl_SubCategory_Ex.Text;
                }

                if (e.Row.FindControl("lbl_Category_Ex") != null)
                {
                    var lbl_Category_Ex = e.Row.FindControl("lbl_Category_Ex") as Label;
                    lbl_Category_Ex.Text =
                        prodCat.GetName(
                            prod.GetParentNoByCategoryCode(
                                prod.GetParentNoByCategoryCode(
                                    prod.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(),
                                        hf_ConnStr.Value), hf_ConnStr.Value), hf_ConnStr.Value), hf_ConnStr.Value);
                    lbl_Category_Ex.ToolTip = lbl_Category_Ex.Text;
                }

                //if (e.Row.FindControl("lbl_BaseUnit_Ex") != null)
                //{
                //    Label lbl_BaseUnit_Ex = e.Row.FindControl("lbl_BaseUnit_Ex") as Label;
                //    lbl_BaseUnit_Ex.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                //    lbl_BaseUnit_Ex.ToolTip = lbl_BaseUnit_Ex.Text;
                //}

                if (e.Row.FindControl("lbl_EnglishName_Ex") != null)
                {
                    var lbl_EnglishName_Ex = e.Row.FindControl("lbl_EnglishName_Ex") as Label;
                    lbl_EnglishName_Ex.Text = prod.GetName(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(),
                        hf_ConnStr.Value);
                    lbl_EnglishName_Ex.ToolTip = lbl_EnglishName_Ex.Text;
                }

                if (e.Row.FindControl("lbl_LocalName_Ex") != null)
                {
                    var lbl_LocalName_Ex = e.Row.FindControl("lbl_LocalName_Ex") as Label;
                    lbl_LocalName_Ex.Text = prod.GetName2(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(),
                        hf_ConnStr.Value);
                    lbl_LocalName_Ex.ToolTip = lbl_LocalName_Ex.Text;
                }

                //if (e.Row.FindControl("lbl_BarCode_Ex") != null)
                //{
                //    Label lbl_BarCode_Ex = e.Row.FindControl("lbl_BarCode_Ex") as Label;
                //    lbl_BarCode_Ex.Text = prod.GetName2(DataBinder.Eval(e.Row.DataItem, "BarCode").ToString(), LoginInfo.ConnStr).ToString();
                //    lbl_BarCode_Ex.ToolTip = lbl_BarCode_Ex.Text;
                //}

                //if (e.Row.FindControl("lbl_DebitAc") != null)
                //{
                //    Label lbl_DebitAc = e.Row.FindControl("lbl_DebitAc") as Label;
                //    lbl_DebitAc.Text = DataBinder.Eval(e.Row.DataItem, "DebitAcc").ToString();
                //}

                //if (e.Row.FindControl("lbl_CreditAc") != null)
                //{
                //    Label lbl_CreditAc = e.Row.FindControl("lbl_CreditAc") as Label;
                //    lbl_CreditAc.Text = DataBinder.Eval(e.Row.DataItem, "CreditAcc").ToString();
                //}

                // ****************** Display Comment ******************
                if (e.Row.FindControl("lbl_Comment") != null)
                {
                    var lbl_Comment = e.Row.FindControl("lbl_Comment") as Label;
                    lbl_Comment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                }

                //****************** Display Stock Summary **************
                var dsPrDtStockSum = new DataSet();

                var getPrDtStockSum = prDt.GetStockSummary(dsPrDtStockSum,
                    DataBinder.Eval(e.Row.DataItem, "SKU").ToString()
                    , DataBinder.Eval(e.Row.DataItem, "StoreId").ToString(), lbl_Date.Text, hf_ConnStr.Value);

                if (getPrDtStockSum)
                {
                    if (dsPrDtStockSum.Tables[prDt.TableName].Rows.Count > 0)
                    {
                        var drStockSummary = dsPrDtStockSum.Tables[prDt.TableName].Rows[0];

                        var lbl_OnHand = e.Row.FindControl("lbl_OnHand") as Label;
                        lbl_OnHand.Text = string.Format(DefaultQtyFmt, drStockSummary["OnHand"].ToString());
                        lbl_OnHand.ToolTip = lbl_OnHand.Text;

                        var lbl_OnOrder = e.Row.FindControl("lbl_OnOrder") as Label;
                        lbl_OnOrder.Text = string.Format(DefaultQtyFmt, drStockSummary["OnOrder"].ToString());
                        lbl_OnOrder.ToolTip = lbl_OnHand.Text;

                        var lbl_ReOrder = e.Row.FindControl("lbl_ReOrder") as Label;
                        lbl_ReOrder.Text = string.Format(DefaultQtyFmt, drStockSummary["Reorder"].ToString());
                        lbl_ReOrder.ToolTip = lbl_OnHand.Text;

                        var lbl_Restock = e.Row.FindControl("lbl_Restock") as Label;
                        lbl_Restock.Text = string.Format(DefaultQtyFmt, drStockSummary["Restock"].ToString());
                        lbl_Restock.ToolTip = lbl_Restock.Text;

                        var lbl_LastPrice = e.Row.FindControl("lbl_LastPrice") as Label;
                        lbl_LastPrice.Text = string.Format(DefaultAmtFmt, drStockSummary["LastPrice"].ToString());
                        lbl_LastPrice.ToolTip = lbl_LastPrice.Text;

                        var lbl_LastVendor = e.Row.FindControl("lbl_LastVendor") as Label;
                        lbl_LastVendor.Text = drStockSummary["LastVendor"].ToString();
                        lbl_LastVendor.ToolTip = lbl_LastVendor.Text;
                    }
                }

                //****************** Display Stock Movement *****************
                if (e.Row.FindControl("StockMovement") != null)
                {
                    var uc_StockMovement = e.Row.FindControl("StockMovement") as PL.PC.StockMovement;
                    uc_StockMovement.HdrNo = DataBinder.Eval(e.Row.DataItem, "Id").ToString();
                    uc_StockMovement.DtNo = DataBinder.Eval(e.Row.DataItem, "RefId").ToString();
                    uc_StockMovement.ConnStr = hf_ConnStr.Value;
                    uc_StockMovement.DataBind();
                }

                //if (e.Row.FindControl("uc_StockSummary") != null)
                //{
                //    StockSummary uc_StockSummary = e.Row.FindControl("uc_StockSummary") as StockSummary;

                //    uc_StockSummary.ProductCode = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();
                //    uc_StockSummary.LocationCode = DataBinder.Eval(e.Row.DataItem, "StoreId").ToString();
                //    uc_StockSummary.ConnStr = LoginInfo.ConnStr;
                //    uc_StockSummary.DataBind();
                //}

                //****************** Display Account Detail **************
                if (e.Row.FindControl("lbl_NetAC_Ex") != null)
                {
                }

                if (e.Row.FindControl("lbl_TaxAC_Ex") != null)
                {
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (e.Row.FindControl("lbl_TotalQty") != null)
                {
                    var lbl_TotalQty = e.Row.FindControl("lbl_TotalQty") as Label;
                    lbl_TotalQty.Text = String.Format(DefaultAmtFmt, Total);
                }
            }
        }

        #region "Editor"

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Response.Redirect("StkInEdit.aspx?MODE=New&BuCode=" +
                                      LoginInfo.BuInfo.BuCode + "&VID=" +
                                      Request.Cookies["[IN].[vStockIn]"].Value);
                    break;

                case "EDIT":
                    Response.Redirect("StkInEdit.aspx?BuCode=" + Request.Params["BuCode"] +
                                      "&MODE=EDIT&ID=" + Request.Params["ID"] +
                                      "&VID=" + Request.Params["VID"]);
                    break;

                case "VOID":
                    pop_ConfirmVoid.ShowOnPageLoad = true;
                    break;

                case "PRINT":
                    //var objArrList = new ArrayList();
                    //objArrList.Add("'" + dsStockIn.Tables[stockIn.TableName].Rows[0]["RefID"] + "'");
                    //Session["s_arrNo"] = objArrList;
                    //var reportLink1 = "../../RPT/ReportCriteria.aspx?category=012&reportid=327" + "&BuCode=" +
                    //                  Request.Params["BuCode"];
                    //ClientScript.RegisterStartupScript(GetType(), "newWindow",
                    //    "<script>window.open('" + reportLink1 + "','_blank')</script>");
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    break;

                case "BACK":
                    Response.Redirect("StkInLst.aspx");
                    break;
            }
        }

        protected void btn_ConfirmVoid_Click(object sender, EventArgs e)
        {
            // Modified on: 21/09/2017, By: Fon
            dsStockIn = (DataSet)Session["dsStockIn"];
            var drStkIn = dsStockIn.Tables[stockIn.TableName].Rows[0];
            drStkIn["Status"] = "Voided";

            var saveStatus = stockIn.Save(dsStockIn, hf_ConnStr.Value);
            if (saveStatus)
            {
                _transLog.Save("IN", "STKIN", lbl_Ref.Text, "VOID", string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);

                dsStockIn.Clear();
                pop_Void.ShowOnPageLoad = true;
            }

            pop_ConfirmVoid.ShowOnPageLoad = false;
        }

        protected void btn_Void_Success_Click(object sender, EventArgs e)
        {
            pop_Void.ShowOnPageLoad = false;
            Page_Retrieve();
        }

        protected void btn_CancelVoid_Click(object sender, EventArgs e)
        {
            pop_ConfirmVoid.ShowOnPageLoad = false;
        }

        #endregion

    }
}