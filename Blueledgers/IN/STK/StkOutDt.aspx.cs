﻿using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using Blue.DAL;

namespace BlueLedger.PL.IN.STK
{
    public partial class StkOutDt : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();
        private readonly Blue.BL.Option.Inventory.StoreLct StrLct = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.IN.AdjType adjType = new Blue.BL.IN.AdjType();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.Option.Inventory.Product prod = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private readonly Blue.BL.IN.StockOut stkOut = new Blue.BL.IN.StockOut();
        private readonly Blue.BL.IN.StockOutDt stkOutDt = new Blue.BL.IN.StockOutDt();
        private readonly Blue.BL.IN.Inventory inventory = new Blue.BL.IN.Inventory();

        private string StkId = string.Empty;

        private decimal totalQty = 0;
        private decimal totalAmt = 0;
        private DataSet dsStockOut = new DataSet();
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly string moduleID = "3.3";

        private DataTable dtCostById
        {
            get
            {
                if (ViewState["dtCost"] == null)
                    return new DataTable();
                else
                    return ViewState["dtCost"] as DataTable;
            }

            set
            {
                ViewState["dtCost"] = value;
            }
        }
        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
        }

        private void Page_Retrieve()
        {
            dsStockOut.Clear();

            if (!string.IsNullOrEmpty(Request.Params["ID"]))
            {
                StkId = Request.Params["ID"];

                var get = stkOut.Get(dsStockOut, StkId, LoginInfo.ConnStr);

                if (!get)
                {
                    return;
                }

                //var getDt = stkOutDt.Get(dsStockOut, StkId, LoginInfo.ConnStr);
                var getDt = GetStockOutDt(dsStockOut, StkId, 1, LoginInfo.ConnStr);

                if (!getDt)
                {
                    return;
                }
            }

            Session["dsStockOut"] = dsStockOut;

            Page_Setting();
        }

        private void Page_Setting()
        {
            var drStkOut = dsStockOut.Tables[stkOut.TableName].Rows[0];

            lbl_Date.Text = DateTime.Parse(drStkOut["CreateDate"].ToString()).ToString("dd/MM/yyyy");
            lbl_Date.ToolTip = lbl_Date.Text;
            lbl_Ref.Text = drStkOut["RefId"].ToString();
            lbl_Ref.ToolTip = lbl_Ref.Text;
            lbl_Status.Text = drStkOut["Status"].ToString();
            lbl_Status.ToolTip = lbl_Status.Text;

            var query = @"SELECT TOP(1) CAST(CommittedDate as DATE) as CommittedDate FROM [IN].Inventory WHERE HdrNo=@DocNo";
            var dtCommit = stkOut.DbExecuteQuery(query,
                new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@DocNo", lbl_Ref.Text) },
                LoginInfo.ConnStr);

            //if (drStkOut["CommitDate"].ToString() != string.Empty)
            if (dtCommit != null & dtCommit.Rows.Count > 0)
            {
                //lbl_CommitDate.Text = String.Format("{0:dd/MM/yyyy}", drStkOut["CommitDate"]);

                lbl_CommitDate.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dtCommit.Rows[0][0]));
                lbl_CommitDate.ToolTip = lbl_CommitDate.Text;
            }

            //Get AdjCode+Name
            var adjText = string.Empty;
            var dsAdj = new DataSet();
            var b = adjType.GetList(dsAdj, "STOCK OUT", LoginInfo.ConnStr);
            if (dsAdj.Tables[adjType.TableName].Rows.Count > 0)
            {
                var dtAdj = dsAdj.Tables[adjType.TableName].Select("AdjId = " + drStkOut["Type"]);
                if (dtAdj.Count() > 0)
                {
                    adjText = dtAdj[0]["AdjCode"] + " : " + dtAdj[0]["AdjName"];
                }
            }

            lbl_Type.Text = adjText;
            lbl_Type.ToolTip = lbl_Type.Text;
            lbl_Desc.Text = drStkOut["Description"].ToString();
            lbl_Desc.ToolTip = lbl_Desc.Text;

            // Set Enable and Disable of Button Edit and void.
            if (drStkOut["Status"].ToString() != string.Empty)
            {
                if (drStkOut["Status"].ToString() == "Voided" || drStkOut["Status"].ToString() == "Committed")
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
            var comment = (PL.UserControls.Comment2)Master.FindControl("Comment");
            comment.Module = "IN";
            comment.SubModule = "STKOUT";
            comment.RefNo = dsStockOut.Tables[stkOut.TableName].Rows[0]["RefId"].ToString();
            comment.BuCode = Request.Params["BuCode"];
            comment.Visible = true;
            comment.DataBind();

            //// Display Attach
            var attach = (PL.UserControls.Attach2)Master.FindControl("Attach");
            attach.BuCode = Request.Params["BuCode"];
            attach.ModuleName = "STKOUT";
            attach.RefNo = dsStockOut.Tables[stkOut.TableName].Rows[0]["RefId"].ToString();
            attach.Visible = true;
            attach.DataBind();

            // Display Log
            var log = (PL.UserControls.Log2)Master.FindControl("Log");
            log.Module = "IN";
            log.SubModule = "STKOUT";
            log.RefNo = dsStockOut.Tables[stkOut.TableName].Rows[0]["RefId"].ToString();
            log.Visible = true;
            log.DataBind();

            Control_HeaderMenuBar();

            //
            string sql = @"
    ;WITH 
    pl AS(
	    SELECT
		    Id,
		    SKU,
		    StoreId
	    FROM
		    [IN].StockOutDt
	    WHERE
		    RefId = @Id
    ),
    group_pl AS(
	    SELECT
		    ROW_NUMBER() OVER(PARTITION BY ProductCode, Location ORDER BY CommittedDate DESC) as RowId,
		    ProductCode,
		    Location,
		    Amount
	    FROM
		    [IN].Inventory i
		    JOIN pl ON pl.SKU = i.ProductCode AND pl.StoreId = i.Location
	    WHERE
		    [IN] > 0
    ),
    cost AS(
    SELECT
	    ProductCode,
	    Location,
	    Amount
    FROM
	    group_pl
    WHERE
	    RowId = 1
    )
    SELECT
	    Id,
	    cost.Amount
    FROM
	    pl
	    LEFT JOIN cost
		    ON cost.ProductCode = pl.SKU AND cost.Location = pl.StoreId
";

            if (lbl_Status.Text != "Committed")
            {
                var dbParams = new DbParameter[1];
                dbParams[0] = new DbParameter("@Id", lbl_Ref.Text);

                dtCostById = prod.DbExecuteQuery(sql, dbParams, LoginInfo.ConnStr);
            }


        }

        private void Control_HeaderMenuBar()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            menu_CmdBar.Items.FindByName("Create").Visible = (pagePermiss >= 3) ? menu_CmdBar.Items.FindByName("Create").Visible : false;
            menu_CmdBar.Items.FindByName("Edit").Visible = (pagePermiss >= 3) ? menu_CmdBar.Items.FindByName("Edit").Visible : false;
            menu_CmdBar.Items.FindByName("Void").Visible = (pagePermiss >= 7) ? menu_CmdBar.Items.FindByName("Void").Visible : false;
        }

        public int GetPageNo()
        {
            var pageNo = Request.Params["Page"];
            if (pageNo == null || pageNo.ToString() == "0")
                pageNo = "1";

            return int.Parse(pageNo);
            //return Session["PageNo"] == null ? 1 : (int)Session["PageNo"];
        }

        public bool GetStockOutDt(DataSet ds, string refId, int pageNo, string connectionString)
        {
            int rows = 10;
            int offset = (pageNo - 1) * rows;
            string sql = "SELECT * FROM [IN].StockOutDt";
            sql += string.Format(" WHERE RefId = '{0}' ", refId);
            sql += " ORDER BY Id";
            sql += string.Format(" OFFSET {0} ROWS", offset);
            sql += string.Format(" FETCH NEXT {0} ROWS ONLY", rows);



            DataTable dt = stkOutDt.DbExecuteQuery(sql, null, connectionString);
            dt.TableName = "[IN].[StockOutDt]";
            ds.Tables.Add(dt);
            //ds.Tables[0].TableName = "[IN].[StockOutDt]";

            return dt.Rows.Count > 0;
        }


        #region "Editor"

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Response.Redirect("StkOutEdit.aspx?BuCode=" +
                                      LoginInfo.BuInfo.BuCode + "&MODE=New&VID=" +
                                      Request.Cookies["[IN].[vStockOut]"].Value);
                    break;

                case "EDIT":
                    Response.Redirect("StkOutEdit.aspx?BuCode=" + Request.Params["BuCode"] +
                                      "&MODE=EDIT&ID=" + Request.Params["ID"] +
                                      "&VID=" + Request.Params["VID"]);
                    break;

                case "VOID":
                    pop_ConfirmVoid.ShowOnPageLoad = true;
                    break;

                case "PRINT":
                    //var objArrList = new ArrayList();
                    //objArrList.Add("'" + Request.Params["ID"] + "'");
                    //Session["s_arrNo"] = objArrList;
                    //var reportLink1 = "../../RPT/ReportCriteria.aspx?category=012&reportid=328" + "&BuCode=" +
                    //                  Request.Params["BuCode"];
                    //ClientScript.RegisterStartupScript(GetType(), "newWindow",
                    //    "<script>window.open('" + reportLink1 + "','_blank')</script>");
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);

                    break;

                case "BACK":
                    Response.Redirect("StkOutLst.aspx");
                    break;
            }
        }

        protected void btn_ConfirmVoid_Click(object sender, EventArgs e)
        {
            // Modified on: 21/09/2017, By: Fon
            // pop_ConfirmVoid.ShowOnPageLoad = false;
            // pop_Void.ShowOnPageLoad = true;

            dsStockOut = (DataSet)Session["dsStockOut"];
            var drStkOut = dsStockOut.Tables[stkOut.TableName].Rows[0];
            drStkOut["Status"] = "Voided";
            var saveStatus = stkOut.Save(dsStockOut, LoginInfo.ConnStr);

            if (saveStatus)
            {
                //ClassLogTool pctool = new ClassLogTool();
                //pctool.SaveActionLog("SO", lbl_Ref.Text, "Void");
                _transLog.Save("IN", "STKOUT", lbl_Ref.Text, "VOID", string.Empty, LoginInfo.LoginName, LoginInfo.ConnStr);

                dsStockOut.Clear();
                pop_Void.ShowOnPageLoad = true;
            }

            pop_ConfirmVoid.ShowOnPageLoad = false;
            // End Modified.
        }

        protected void btn_Void_Success_Click(object sender, EventArgs e)
        {
            pop_Void.ShowOnPageLoad = false;
            Page_Retrieve();
            // End Modifed.
        }

        protected void btn_CancelVoid_Click(object sender, EventArgs e)
        {
            pop_ConfirmVoid.ShowOnPageLoad = false;
        }

        #endregion

        #region "Grid View"
        private void BindData()
        {
            var dsStock = new DataSet();

            var result = stkOutDt.GetDetailById(dsStock, Request.Params["ID"], LoginInfo.ConnStr);


            if (result)
            {
                grd_StkOut_Dt.DataSource = dsStock.Tables[stkOutDt.TableName];
                grd_StkOut_Dt.DataBind();
            }
        }

        protected void grd_StkOut_Dt_Load(object sender, EventArgs e)
        {
            BindData();
        }

        protected void grd_StkOut_Dt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int id = int.Parse(DataBinder.Eval(e.Row.DataItem, "ID").ToString());
                string productCode = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();

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
                                             LoginInfo.ConnStr);
                    //StrLct.GetName2(DataBinder.Eval(e.Row.DataItem, "StoreId").ToString(), LoginInfo.ConnStr);
                    lbl_StoreName.ToolTip = lbl_StoreName.Text;
                }

                if (e.Row.FindControl("lbl_Item_Desc") != null)
                {
                    var lbl_Item_Desc = e.Row.FindControl("lbl_Item_Desc") as Label;
                    lbl_Item_Desc.Text = DataBinder.Eval(e.Row.DataItem, "SKU") + " : " +
                                         prod.GetName(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(),
                                             LoginInfo.ConnStr) + " : " +
                                         prod.GetName2(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(),
                                             LoginInfo.ConnStr);
                    lbl_Item_Desc.ToolTip = lbl_Item_Desc.Text;
                }

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                    lbl_Unit.ToolTip = lbl_Unit.Text;
                }


                // ---------------------------------------------------------------------
                decimal qty = 0;

                if (e.Row.FindControl("lbl_Qty") != null)
                {
                    var lbl_Qty = e.Row.FindControl("lbl_Qty") as Label;
                    qty = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Qty"));
                    lbl_Qty.Text = string.Format(DefaultQtyFmt, qty);
                    lbl_Qty.ToolTip = lbl_Qty.Text;
                    totalQty += qty;
                }

                // ---------------------------------------------------------------------

                decimal unitCost = 0;

                if (e.Row.FindControl("lbl_UnitCost") != null)
                {
                    string docStatus = lbl_Status.Text;

                    if (docStatus == "Committed")
                    {
                        string hdrNo = DataBinder.Eval(e.Row.DataItem, "RefId").ToString();
                        string dtNo = DataBinder.Eval(e.Row.DataItem, "Id").ToString();

                        DataTable dt = inventory.GetListByHdrNo_DtNo(hdrNo, dtNo, LoginInfo.ConnStr);

                        if (dt.Rows.Count > 0)
                        {
                            unitCost = Convert.ToDecimal(dt.Rows[0]["Amount"]);
                        }
                    }
                    else
                    {

                        DataRow[] drCost = dtCostById.Select("Id = " + id.ToString());
                        if (drCost.Length > 0)
                            unitCost = string.IsNullOrEmpty(drCost[0]["Amount"].ToString()) ? 0 : decimal.Parse(drCost[0]["Amount"].ToString());

                        //unitCost = prod.GetLastCost(productCode, DateTime.Today, LoginInfo.ConnStr);
                    }


                    var lbl = e.Row.FindControl("lbl_UnitCost") as Label;
                    lbl.Text = string.Format(DefaultAmtFmt, unitCost);
                    lbl.ToolTip = lbl.ToString();
                }




                if (e.Row.FindControl("lbl_Amt") != null)
                {
                    decimal amt = RoundAmt(qty * unitCost);


                    var lbl = e.Row.FindControl("lbl_Amt") as Label;
                    lbl.Text = string.Format(DefaultAmtFmt, amt);
                    lbl.ToolTip = lbl.ToString();

                    totalAmt += amt;
                }





                // ****************** Display Transaction Detail ******************
                if (e.Row.FindControl("lbl_SKU_Ex") != null)
                {
                    var lbl_SKU_Ex = e.Row.FindControl("lbl_SKU_Ex") as Label;
                    lbl_SKU_Ex.Text = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();
                    lbl_SKU_Ex.ToolTip = lbl_SKU_Ex.Text;
                }

                if (e.Row.FindControl("lbl_ItemGroup_Ex") != null)
                {
                    var lbl_ItemGroup_Ex = e.Row.FindControl("lbl_ItemGroup_Ex") as Label;
                    lbl_ItemGroup_Ex.Text =
                        prodCat.GetName(
                            prod.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(), LoginInfo.ConnStr),
                            LoginInfo.ConnStr);
                    lbl_ItemGroup_Ex.ToolTip = lbl_ItemGroup_Ex.Text;
                }

                if (e.Row.FindControl("lbl_SubCategory_Ex") != null)
                {
                    var lbl_SubCategory_Ex = e.Row.FindControl("lbl_SubCategory_Ex") as Label;
                    lbl_SubCategory_Ex.Text =
                        prodCat.GetName(
                            prod.GetParentNoByCategoryCode(
                                prod.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(),
                                    LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr);
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
                                        LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr);
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
                        LoginInfo.ConnStr);
                    lbl_EnglishName_Ex.ToolTip = lbl_EnglishName_Ex.Text;
                }

                if (e.Row.FindControl("lbl_LocalName_Ex") != null)
                {
                    var lbl_LocalName_Ex = e.Row.FindControl("lbl_LocalName_Ex") as Label;
                    lbl_LocalName_Ex.Text = prod.GetName2(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(),
                        LoginInfo.ConnStr);
                    lbl_LocalName_Ex.ToolTip = lbl_LocalName_Ex.Text;
                }

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
                    lbl_Comment.ToolTip = lbl_Comment.Text;
                }

                //****************** Display Stock Summary ******************
                var dsPrDtStockSum = new DataSet();

                var getPrDtStockSum = prDt.GetStockSummary(dsPrDtStockSum,
                    DataBinder.Eval(e.Row.DataItem, "SKU").ToString()
                    , DataBinder.Eval(e.Row.DataItem, "StoreId").ToString(), lbl_Date.Text, LoginInfo.ConnStr);

                if (getPrDtStockSum)
                {
                    if (dsPrDtStockSum.Tables[prDt.TableName].Rows.Count > 0)
                    {
                        var drStockSummary = dsPrDtStockSum.Tables[prDt.TableName].Rows[0];

                        var lbl_OnHand = e.Row.FindControl("lbl_OnHand") as Label;
                        lbl_OnHand.Text = string.Format(DefaultQtyFmt,
                            GetOnHand(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(),
                                DataBinder.Eval(e.Row.DataItem, "StoreId").ToString()
                                , Convert.ToDateTime(lbl_Date.Text).Date.ToString(), LoginInfo.ConnStr).ToString());
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
                //if (e.Row.FindControl("uc_StockSummary") != null)
                //{
                //    StockSummary uc_StockSummary = e.Row.FindControl("uc_StockSummary") as StockSummary;

                //    uc_StockSummary.ProductCode = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();
                //    uc_StockSummary.LocationCode = DataBinder.Eval(e.Row.DataItem, "StoreId").ToString();
                //    uc_StockSummary.ConnStr = LoginInfo.ConnStr;
                //    uc_StockSummary.DataBind();
                //}
                //****************** Display Stock Movement *****************
                if (e.Row.FindControl("StockMovement") != null)
                {
                    var uc_StockMovement = e.Row.FindControl("StockMovement") as PL.PC.StockMovement;
                    uc_StockMovement.HdrNo = DataBinder.Eval(e.Row.DataItem, "RefId").ToString();
                    uc_StockMovement.DtNo = DataBinder.Eval(e.Row.DataItem, "Id").ToString();
                    uc_StockMovement.ConnStr = LoginInfo.ConnStr;
                    uc_StockMovement.DataBind();
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (e.Row.FindControl("lbl_TotalQty") != null)
                {
                    var lbl_TotalQty = e.Row.FindControl("lbl_TotalQty") as Label;
                    lbl_TotalQty.Text = String.Format(DefaultQtyFmt, totalQty);
                }

                if (e.Row.FindControl("lbl_TotalAmt") != null)
                {
                    var lbl = e.Row.FindControl("lbl_TotalAmt") as Label;
                    lbl.Text = String.Format(DefaultAmtFmt, totalAmt);
                }

            }
        }

        protected void grd_StkOut_Dt_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd_StkOut_Dt.PageIndex = e.NewPageIndex;
            BindData();
        }

        public decimal GetOnHand(string ProductCode, string LocationCode, string Date, string ConnStr)
        {
            var dbParams = new Blue.DAL.DbParameter[3];
            dbParams[0] = new Blue.DAL.DbParameter("@ProductCode", ProductCode);
            dbParams[1] = new Blue.DAL.DbParameter("@LocationCode", LocationCode);
            dbParams[2] = new Blue.DAL.DbParameter("@Date", DateTime.Parse(Date).ToString("yyyy-MM-dd"));

            var dtGet = new DataTable();
            dtGet = new Blue.DAL.DbHandler().DbRead("[IN].[GetOnHand_ProductCode_LocationCode_Date]", dbParams, ConnStr);

            if (dtGet.Rows.Count > 0 && dtGet.Rows[0][0] != DBNull.Value)
            {
                return decimal.Parse(dtGet.Rows[0][0].ToString());
            }

            return 0;
        }

        #endregion
    }
}