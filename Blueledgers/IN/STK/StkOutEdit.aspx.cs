using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxTabControl;
using Blue.DAL;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BlueLedger.PL.IN.STK
{
    public partial class StkOutEdit : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();
        private readonly Blue.BL.Option.Admin.Interface.AccountMapp accMapp = new Blue.BL.Option.Admin.Interface.AccountMapp();

        private readonly Blue.BL.IN.AdjType adjType = new Blue.BL.IN.AdjType();
        private readonly Blue.BL.IN.Inventory inv = new Blue.BL.IN.Inventory();
        private readonly Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.Option.Inventory.Product prod = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private readonly Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.IN.StockOut stockOut = new Blue.BL.IN.StockOut();
        private readonly Blue.BL.IN.StockOutDt stockOutDt = new Blue.BL.IN.StockOutDt();
        private readonly Blue.BL.Option.Inventory.StoreLct strLct = new Blue.BL.Option.Inventory.StoreLct();

        private string StkId = string.Empty;
        private decimal SumQtyForChkOnhand;
        private decimal Total = 0;
        private DataSet dsStockOut = new DataSet();
        private Blue.BL.GnxLib gnx = new Blue.BL.GnxLib();
        private Blue.BL.Option.Inventory.Unit unit = new Blue.BL.Option.Inventory.Unit();

        private string MODE
        {
            get { return Request.QueryString["MODE"]; }
        }

        private string StockOutEditMode
        {
            get { return ViewState["StockOutEditMode"].ToString(); }
            set { ViewState["StockOutEditMode"] = value; }

        }

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                //dsStockOut = (DataSet)Session["dsStockOut"];
                dsStockOut = (DataSet)ViewState["dsStockOut"];
            }
        }

        private void Page_Retrieve()
        {
            if (MODE.ToUpper() == "NEW" || MODE.ToUpper() == "SDR")
            {
                var result = stockOut.GetSchema(dsStockOut, LoginInfo.ConnStr);

                if (!result)
                {
                    return;
                }

                var resultDt = stockOutDt.GetSchema(dsStockOut, LoginInfo.ConnStr);

                if (!resultDt)
                {
                    return;
                }
            }
            else
            {
                StkId = Request.Params["ID"];

                var get = stockOut.Get(dsStockOut, StkId, LoginInfo.ConnStr);

                if (!get)
                {
                    return;
                }

                var getDt = stockOutDt.Get(dsStockOut, StkId, LoginInfo.ConnStr);

                if (!getDt)
                {
                    return;
                }
            }

            //Session["dsStockOut"] = dsStockOut;
            ViewState["dsStockOut"] = dsStockOut;

            Page_Setting();
        }

        private void Page_Setting()
        {
            if (MODE.ToUpper() == "NEW")
            {
                dsStockOut.Clear();

                // Visible Commit Menu.
                menu_CmdBar.Items[1].Visible = false;

                txt_Date.Text = ServerDateTime.ToShortDateString();

            }
            else if (MODE.ToUpper() == "SDR")
            {
                dsStockOut.Clear();
                // Visible Commit Menu.
                menu_CmdBar.Items[1].Visible = false;

                txt_Date.Text = ServerDateTime.ToShortDateString();

                var refId = Request.Params["refid"];

                if (string.IsNullOrEmpty(refId))
                    ShowStandardRequisition();
                else
                {
                    string sql = string.Format(@"SELECT h.LocationCode, d.ProductCode, p.InventoryUnit
                                    FROM [IN].StandardRequisition h 
                                    JOIN [IN].StandardRequisitionDetail d ON d.DocumentId = h.RefId
                                    JOIN [IN].Product p ON p.ProductCode = d.ProductCode
                                    WHERE h.RefId = {0}", refId.ToString());
                    DataTable dt = inv.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

                    int i = 1;
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataRow drNew = dsStockOut.Tables[stockOutDt.TableName].NewRow();
                        drNew["Id"] = i++;
                        drNew["StoreId"] = dr["LocationCode"];
                        drNew["SKU"] = dr["ProductCode"];
                        drNew["Unit"] = dr["InventoryUnit"];
                        drNew["Qty"] = 0;

                        dsStockOut.Tables[stockOutDt.TableName].Rows.Add(drNew);
                    }
                }
            }
            else
            {
                var drStkOut = dsStockOut.Tables[stockOut.TableName].Rows[0];

                //Get AdjCode+Name
                var adjText = string.Empty;
                var dsAdj = new DataSet();
                var b = adjType.GetList(dsAdj, "STOCK OUT", LoginInfo.ConnStr);
                if (dsAdj.Tables[adjType.TableName].Rows.Count > 0)
                {
                    var dtAdj = dsAdj.Tables[adjType.TableName].Select("AdjId = " + drStkOut["Type"]);
                }

                ddl_Type.Value = Convert.ToInt32(drStkOut["Type"]);

                txt_Date.Text = DateTime.Parse(drStkOut["CreateDate"].ToString()).ToString("dd/MM/yyyy");
                txt_Date.Enabled = false;
                lbl_Ref.Text = drStkOut["RefId"].ToString();
                lbl_Ref.ToolTip = lbl_Ref.Text;
                lbl_Status.Text = drStkOut["Status"].ToString();
                lbl_Status.ToolTip = lbl_Status.Text;
                //ddl_Type.Value      = adjText;
                txt_Desc.Text = drStkOut["Description"].ToString();
            }

            grd_StkOutEdit1.DataSource = dsStockOut.Tables[stockOutDt.TableName];
            grd_StkOutEdit1.DataBind();
        }

        protected void btn_Save_Success_Click(object sender, EventArgs e)
        {
            pop_Save.ShowOnPageLoad = false;
        }

        protected void ddl_Unit_Load(object sender, EventArgs e)
        {
            var ddl_Unit = sender as ASPxComboBox;
            var ddl_Product = sender as ASPxComboBox;

            if (ddl_Product != null && ddl_Unit.Value != null)
            {
                ddl_Unit.DataSource = prodUnit.GetLookUp_InvenUnit(ddl_Product.Value.ToString(), LoginInfo.ConnStr);
                ddl_Unit.DataBind();
            }
        }

        protected void Img_Create_Click(object sender, ImageClickEventArgs e)
        {
            Create();
        }

        private decimal GetOnHand(string productCode, string locationCode, DateTime date)
        {
            if (dsStockOut.Tables[prDt.TableName] != null)
            {
                dsStockOut.Tables[prDt.TableName].Clear();
            }

            var get = prDt.GetStockSummary(dsStockOut, productCode, locationCode, date.ToString("yyyy-MM-dd"), LoginInfo.ConnStr);

            if (get)
            {
                if (dsStockOut.Tables[prDt.TableName].Rows[0]["OnHand"].ToString() != string.Empty &&
                    dsStockOut.Tables[prDt.TableName].Rows[0]["OnHand"] != null)
                {
                    return decimal.Parse(dsStockOut.Tables[prDt.TableName].Rows[0]["OnHand"].ToString());
                }
            }

            return 0;
        }

        protected void txt_Qty_TextChanged(object sender, EventArgs e)
        {
            var txt_Qty = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("txt_Qty") as ASPxSpinEdit;
            var hf_ProductCode = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("hf_ProductCode") as HiddenField;
            var ddl_Store = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("ddl_Store") as ASPxComboBox;
            var ddl_Product = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("ddl_Product") as ASPxComboBox;

            var onHand = GetOnHand(hf_ProductCode.Value, ddl_Store.Value.ToString(), Convert.ToDateTime(txt_Date.Text));

            foreach (GridViewRow grv_Row in grd_StkOutEdit1.Rows)
            {
                var lbl_Qty = grv_Row.FindControl("lbl_Qty") as Label;

                if (dsStockOut.Tables[stockOutDt.TableName].Rows[grv_Row.RowIndex]["SKU"].ToString() != null &&
                    lbl_Qty != null
                    && dsStockOut.Tables[stockOutDt.TableName].Rows[grv_Row.RowIndex]["StoreId"].ToString() != null)
                {
                    if (dsStockOut.Tables[stockOutDt.TableName].Rows[grv_Row.RowIndex]["SKU"].ToString() == ddl_Product.Value.ToString()
                        &&
                        dsStockOut.Tables[stockOutDt.TableName].Rows[grv_Row.RowIndex]["StoreId"].ToString() == ddl_Store.Value.ToString())
                    {
                        SumQtyForChkOnhand += Convert.ToDecimal(lbl_Qty.Text);
                    }
                }

                if (dsStockOut.Tables[stockOutDt.TableName].Rows[grv_Row.RowIndex]["SKU"].ToString() != null &&
                    txt_Qty != null
                    && dsStockOut.Tables[stockOutDt.TableName].Rows[grv_Row.RowIndex]["StoreId"].ToString() != null)
                {
                    if (dsStockOut.Tables[stockOutDt.TableName].Rows[grv_Row.RowIndex]["SKU"].ToString() == ddl_Product.Value.ToString()
                        &&
                        dsStockOut.Tables[stockOutDt.TableName].Rows[grv_Row.RowIndex]["StoreId"].ToString() == ddl_Store.Value.ToString())
                    {
                        SumQtyForChkOnhand += Convert.ToDecimal(txt_Qty.Text);
                    }
                }
            }

            if (decimal.Parse(txt_Qty.Text) > onHand)
            {
                lbl_Warning.Text = "Cannot approve quantity more than on hand";
                pop_Warning.ShowOnPageLoad = true;

                txt_Qty.Text = String.Format(DefaultQtyFmt, onHand);
            }

            if (SumQtyForChkOnhand > onHand)
            {
                lbl_Warning.Text = "Cannot approve quantity more than on hand";
                pop_Warning.ShowOnPageLoad = true;

                txt_Qty.Text = "0.00";
            }
        }

        // Check Period
        protected void txt_Date_TextChanged(object sender, EventArgs e)
        {
            // Period
            //for (var i = 0; i < grd_StkOutEdit1.Rows.Count; i++)
            //{
            //    var lbl_Store = grd_StkOutEdit1.Rows[i].FindControl("lbl_StoreName") as Label;
            //    var lbl_Product = grd_StkOutEdit1.Rows[i].FindControl("lbl_Item_Desc") as Label;
            //    var lbl_Qty = grd_StkOutEdit1.Rows[i].FindControl("lbl_Qty") as Label;

            //    //if (!period.GetIsValidDate(DateTime.Parse(txt_Date.Text), lbl_Store.Text, LoginInfo.ConnStr))
            //    //{
            //    //    lbl_WarningPeriod.Text = "Store " + lbl_Store.Text.Split(':')[1].Trim() + " is not period.";
            //    //    pop_WarningPeriod.ShowOnPageLoad = true;
            //    //    return;
            //    //}

            //    var onHand = GetOnHand(lbl_Product.Text.Split(':')[0].Trim(), lbl_Store.Text.Split(':')[0].Trim(), txt_Date.Text);
            //    var qty = decimal.Parse(lbl_Qty.Text);

            //    if (onHand - qty < 0)
            //    {
            //        lbl_Warning.Text = "Commit quantity of product " +
            //                           lbl_Product.Text.Split(':')[0].Trim() + " : " +
            //                           prod.GetName(lbl_Product.Text.Split(':')[0].Trim(), LoginInfo.ConnStr) + ", " +
            //                           prod.GetName2(lbl_Product.Text.Split(':')[0].Trim(), LoginInfo.ConnStr) +
            //                           " must be less than on hand.";
            //        pop_Warning.ShowOnPageLoad = true;
            //        return;
            //    }
            //}
        }

        protected void btn_WarningPeriod_Click(object sender, EventArgs e)
        {
            pop_WarningPeriod.ShowOnPageLoad = false;
        }

        protected void ddl_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void ddl_Type_Load(object sender, EventArgs e)
        {
            var comboBox = (ASPxComboBox)sender;
            comboBox.DataSource = adjType.GetList(lbl_StockOut.Text, LoginInfo.ConnStr);
            comboBox.ValueField = "AdjID";
            comboBox.DataBind();
            if (comboBox.Value != null)
            {
                // comboBox.Text = adjType.GetName(comboBox.Value.ToString(), LoginInfo.ConnStr);
            }
        }

        #region "Editors"

        protected void btn_ConfrimSave_Click(object sender, EventArgs e)
        {
            //Save();
            var value = hf_IsCommit.Value.ToString();
            var isCommit = value == "1";

            SaveAndCommit(isCommit);
        }

        protected void btn_ComfirmDelete_Click(object sender, EventArgs e)
        {
            for (var i = grd_StkOutEdit1.Rows.Count - 1; i >= 0; i--)
            {
                var chk_Item = grd_StkOutEdit1.Rows[i].Cells[0].FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    var drStkOutDt = dsStockOut.Tables[stockOutDt.TableName].Rows[i];

                    if (drStkOutDt.RowState != DataRowState.Deleted)
                    {
                        drStkOutDt.Delete();
                    }
                }
            }

            grd_StkOutEdit1.DataSource = dsStockOut.Tables[stockOutDt.TableName];
            grd_StkOutEdit1.EditIndex = -1;
            grd_StkOutEdit1.DataBind();

            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        protected void ddl_Store_Load(object sender, EventArgs e)
        {
            var ddl_StoreCode = sender as ASPxComboBox;
            //var ds = strLct.GetList(LoginInfo.LoginName, LoginInfo.ConnStr);

            //// Modified on: 12/02/2018, By: Fon, For: P' Oat request.
            ////ds.DefaultView.RowFilter = "EOP = 1";
            //ds.DefaultView.RowFilter = "EOP <> 2";
            //// End Modified.
            //var dt = ds.DefaultView.ToTable();
            //ddl_StoreCode.DataSource = dt;
            //ddl_StoreCode.DataBind();
            string sql = string.Format(@"SELECT l.LocationCode, l.LocationName 
                                        FROM [IN].StoreLocation l 
                                        JOIN [ADMIN].UserStore us ON us.LocationCode = l.LocationCode 
                                        WHERE EOP <> 2
                                        AND us.LoginName = '{0}'
                                        ORDER BY LocationCode", LoginInfo.LoginName);


            var dt = inv.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
            ddl_StoreCode.DataSource = dt;
            ddl_StoreCode.DataBind();

        }

        //protected void ddl_Store_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ASPxComboBox ddl_Product    = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
        //    ASPxComboBox ddl_Store      = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("ddl_Store") as ASPxComboBox;

        //    string StoreName = strLct.GetName(ddl_Store.Value.ToString(), LoginInfo.ConnStr);

        //    if (ddl_Product != null)
        //    {
        //        //Check Period
        //        if (period.GetIsValidDate(DateTime.Parse(txt_Date.Text), ddl_Store.Value.ToString(), LoginInfo.ConnStr))
        //        {
        //            ddl_Product.DataSource = product.GetLookUp_LocationCode(ddl_Store.Value.ToString(), LoginInfo.ConnStr);
        //            ddl_Product.DataBind();
        //        }
        //        else
        //        {
        //            ddl_Store.Value = string.Empty;
        //            lbl_WarningPeriod.Text = "Store " + StoreName + " is not period.";
        //            pop_WarningPeriod.ShowOnPageLoad = true;
        //        }
        //    }
        //}

        protected void ddl_Product_ItemRequestedByValue(object source, ListEditItemRequestedByValueEventArgs e)
        {
            var comboBox = source as ASPxComboBox;
            var ddlStore = comboBox.Parent.FindControl("ddl_Store") as ASPxComboBox;

            if (e.Value == null || comboBox == null || ddlStore.Value == null)
                return;

            var sqlDataSource1 = new SqlDataSource();

            new ProductLookup().ItemRequestedByValue(ref comboBox, sqlDataSource1, LoginInfo.ConnStr, e, ddlStore.Value.ToString());
        }

        protected void ddl_Product_ItemsRequestedByFilterCondition(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            var ddlProduct = source as ASPxComboBox;
            var ddlStore = ddlProduct.Parent.FindControl("ddl_Store") as ASPxComboBox;

            if (ddlStore.Value == null)
                return;
            // if (ddlStore.Value == null || ddlProduct == null || e.Filter == null)
            // return;

            var sqlDataSource1 = new SqlDataSource();
            new ProductLookup().ItemsRequestedByFilterCondition(ref ddlProduct, sqlDataSource1, LoginInfo.ConnStr, e, ddlStore.Value.ToString());
        }

        protected void ddl_Product_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl_Product = sender as ASPxComboBox;

            if (ddl_Product.SelectedItem == null)
                return;


            var tp_Information = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("tp_Information") as ASPxPageControl;
            //var ddl_Product = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
            var lbl_Unit = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("lbl_Unit") as Label;
            var hf_ProductCode = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("hf_ProductCode") as HiddenField;
            var hf_Cost = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("hf_Cost") as HiddenField;
            //var txt_UnitCost = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("txt_UnitCost") as ASPxSpinEdit;
            var lbl_UnitCost = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("lbl_UnitCost") as Label;
            var ddl_Store = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("ddl_Store") as ASPxComboBox;
            var ddl_Unit = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("ddl_Unit") as ASPxComboBox;


            string productCode = ddl_Product.ClientValue;

            if (ddl_Unit != null)
            {
                ddl_Unit.DataSource = prodUnit.GetLookUp_InvenUnit(ddl_Product.ClientValue, LoginInfo.ConnStr);
                ddl_Unit.DataBind();
                ddl_Unit.Value = prodUnit.GetInvenUnitType(ddl_Product.ClientValue, LoginInfo.ConnStr);
            }

            if (lbl_Unit != null)
            {
                //lbl_Unit.Text = prodUnit.GetInvenUnitType(productCode, LoginInfo.ConnStr); ;
                var dtUnit = prodUnit.DbExecuteQuery(string.Format("SELECT InventoryUnit FROM [IN].Product WHERE ProductCode = '{0}'", productCode), null, LoginInfo.ConnStr);
                if (dtUnit != null && dtUnit.Rows.Count > 0)
                {
                    lbl_Unit.Text = dtUnit.Rows[0][0].ToString();
                }
            }



            lbl_UnitCost.Text = string.Format(DefaultAmtFmt, GetLastCost(productCode, DateTime.Today, LoginInfo.ConnStr));



            //Get StartDate and EndDate for update Avg in inventory
            //var startDate = period.GetStartDate(Convert.ToDateTime(txt_Date.Text).Date, LoginInfo.ConnStr);
            //var endDate = period.GetEndDate(Convert.ToDateTime(txt_Date.Text).Date, LoginInfo.ConnStr);


            //if (ddl_Product != null)
            //{
            //    if (ddl_Product.ClientValue == hf_ProductCode.Value)
            //    {
            //        var OldUnitCost = String.Format("{0:N}", GetAvgCost(ddl_Product.ClientValue, endDate, LoginInfo.ConnStr));

            //        if (txt_UnitCost.Text != OldUnitCost)
            //        {
            //            txt_UnitCost.Text = txt_UnitCost.Text;

            //        }
            //    }
            //    else
            //    {
            //        //txt_UnitCost.Text = String.Format("{0:N}", GetAvgCost(ddl_Product.ClientValue, endDate, LoginInfo.ConnStr));
            //    }
            //}

            //var dsPrDtStockSum = new DataSet();

            //var getPrDtStockSum = prDt.GetStockSummary(dsPrDtStockSum, ddl_Product.ClientValue,
            //    ddl_Store.Value.ToString(), txt_Date.Text, LoginInfo.ConnStr);

            //if (getPrDtStockSum)
            //{
            //    if (dsPrDtStockSum.Tables[prDt.TableName].Rows.Count > 0)
            //    {
            //        var drStockSummary = dsPrDtStockSum.Tables[prDt.TableName].Rows[0];

            //        var lbl_OnHand = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("lbl_OnHand") as Label;
            //        lbl_OnHand.Text = drStockSummary["OnHand"].ToString();
            //        //lbl_OnHand.Text = this.GetOnHand(ddl_Product.Value.ToString(), ddl_Store.Value.ToString()
            //        //                        , DateTime.Parse(txt_Date.Text).ToShortDateString(), LoginInfo.ConnStr).ToString();
            //        lbl_OnHand.ToolTip = lbl_OnHand.Text;

            //        var lbl_OnOrder =
            //            grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("lbl_OnOrder") as Label;
            //        lbl_OnOrder.Text = drStockSummary["OnOrder"].ToString();
            //        lbl_OnOrder.ToolTip = lbl_OnOrder.Text;

            //        var lbl_ReOrder =
            //            grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("lbl_ReOrder") as Label;
            //        lbl_ReOrder.Text = drStockSummary["Reorder"].ToString();
            //        lbl_ReOrder.ToolTip = lbl_ReOrder.Text;

            //        var lbl_Restock =
            //            grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("lbl_Restock") as Label;
            //        lbl_Restock.Text = drStockSummary["Restock"].ToString();
            //        lbl_Restock.ToolTip = lbl_Restock.Text;

            //        var lbl_LastPrice =
            //            grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("lbl_LastPrice") as Label;
            //        lbl_LastPrice.Text = drStockSummary["LastPrice"].ToString();
            //        lbl_LastPrice.ToolTip = lbl_LastPrice.Text;

            //        var lbl_LastVendor =
            //            grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("lbl_LastVendor") as Label;
            //        lbl_LastVendor.Text = drStockSummary["LastVendor"].ToString();
            //        lbl_LastVendor.ToolTip = lbl_LastVendor.Text;
            //    }
            //}

            hf_ProductCode.Value = ddl_Product.ClientValue;
            //hf_Cost.Value = txt_UnitCost.Text;
        }

        public decimal GetAvgCost(string ProductCode, string CommittedDate, string ConnStr)
        {
            var dbParams = new Blue.DAL.DbParameter[2];
            dbParams[0] = new Blue.DAL.DbParameter("@ProductCode", ProductCode);
            dbParams[1] = new Blue.DAL.DbParameter("@CommittedDate", CommittedDate);

            var dtGet = new DataTable();
            dtGet = new Blue.DAL.DbHandler().DbRead("[IN].[GetLastAvg_ProductCode_CommittedDate]", dbParams, ConnStr);

            if (dtGet.Rows.Count > 0 && dtGet.Rows[0][0] != DBNull.Value)
            {
                return decimal.Parse(dtGet.Rows[0][0].ToString());
            }

            return 0;
        }

        //public decimal GetOnHand(string ProductCode, string LocationCode, string Date, string ConnStr)
        //{
        //    DAL.DbParameter[] dbParams = new DAL.DbParameter[3];
        //    dbParams[0] = new DAL.DbParameter("@ProductCode", ProductCode);
        //    dbParams[1] = new DAL.DbParameter("@LocationCode", LocationCode);
        //    dbParams[2] = new DAL.DbParameter("@Date", DateTime.Parse(Date).ToString("yyyy-MM-dd"));

        //    DataTable dtGet = new DataTable();
        //    dtGet = new DAL.DbHandler().DbRead("[IN].[GetOnHand_ProductCode_LocationCode_Date]", dbParams, ConnStr);

        //    if (dtGet.Rows.Count > 0 && dtGet.Rows[0][0] != DBNull.Value)
        //    {
        //        return decimal.Parse(dtGet.Rows[0][0].ToString());
        //    }

        //    return 0;
        //}

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":

                    if (ddl_Type.Value == null)
                    {
                        lbl_Warning.Text = "Please select <b>Type</b> for create item.";
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }

                    if (grd_StkOutEdit1.Rows.Count == 0)
                    {
                        lbl_Warning.Text = "Please clicks <b>Create</b> button to add item.";
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }

                    hf_IsCommit.Value = "0";
                    lbl_SureSave_Nm.Text = "Do you want to save?";
                    pop_ConfrimSave.ShowOnPageLoad = true;

                    break;

                case "COMMIT":

                    if (ddl_Type.Value == null)
                    {
                        lbl_Warning.Text = "Please select <b>Type</b> for create item.";
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }
                    if (grd_StkOutEdit1.Rows.Count == 0)
                    {
                        lbl_Warning.Text = "Please clicks <b>Create</b> button to add item.";
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }

                    //Commit();

                    hf_IsCommit.Value = "1";
                    lbl_SureSave_Nm.Text = "Do you want to save and commit?";
                    pop_ConfrimSave.ShowOnPageLoad = true;


                    break;

                case "BACK":
                    Back();
                    break;
            }
        }

        protected void menu_CmdGrd_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Create();
                    break;

                case "DELETE":
                    Delete();
                    break;

                case "EDITQTY":
                    EditQty();
                    break;
            }
        }


        private void Create()
        {
            //// Disable Save & Commit Button On Header.
            //menu_CmdBar.Items[0].Enabled = false; //Save.
            //menu_CmdBar.Items[1].Enabled = false; //Commit.

            //// Disable Create & Delete when click create button on detail grid.
            //menu_CmdGrd.Enabled = false;

            txt_Date.Enabled = false;

            // Add new stkOutDt row
            var drNew = dsStockOut.Tables[stockOutDt.TableName].NewRow();

            drNew["RefId"] = string.Empty;
            drNew["Id"] = (dsStockOut.Tables[stockOutDt.TableName].Rows.Count == 0
                ? 1
                : int.Parse(
                    dsStockOut.Tables[stockOutDt.TableName].Rows[dsStockOut.Tables[stockOutDt.TableName].Rows.Count - 1]
                        ["Id"].ToString()) + 1);

            if (dsStockOut.Tables[stockOutDt.TableName].Rows.Count > 0)
            {
                drNew["StoreId"] = dsStockOut.Tables[stockOutDt.TableName].Rows[dsStockOut.Tables[stockOutDt.TableName].Rows.Count - 1]["StoreId"].ToString();
            }

            dsStockOut.Tables[stockOutDt.TableName].Rows.Add(drNew);

            grd_StkOutEdit1.DataSource = dsStockOut.Tables[stockOutDt.TableName];
            grd_StkOutEdit1.EditIndex = dsStockOut.Tables[stockOutDt.TableName].Rows.Count - 1;
            grd_StkOutEdit1.DataBind();

            StockOutEditMode = "NEW";
        }

        private void Delete()
        {
            pop_ConfrimDelete.ShowOnPageLoad = true;
        }

        private void EditQty()
        {
            throw new NotImplementedException();
        }
        /*
        private void Save()
        {
            // Added on: 22/09/2017, By: Fon, About: hdrNo
            string hdrNo = string.Empty;
            string _action = string.Empty;

            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                var drStkOut = dsStockOut.Tables[stockOut.TableName].NewRow();
                if (MODE.ToUpper() == "NEW" || MODE.ToUpper() == "SDR") // ?
                {
                    _action = "CREATE";
                    drStkOut["RefId"] = stockOut.GetNewID(DateTime.Parse(txt_Date.Text), LoginInfo.ConnStr);
                    hdrNo = drStkOut["RefId"].ToString();

                    foreach (DataRow drStkOutDt in dsStockOut.Tables[stockOutDt.TableName].Rows)
                    {
                        if (drStkOutDt.RowState == DataRowState.Deleted)
                            continue;

                        drStkOutDt["RefId"] = drStkOut["RefId"].ToString();
                    }
                }

                drStkOut["Type"] = ddl_Type.Value.ToString();
                drStkOut["Status"] = "Saved";
                drStkOut["Description"] = txt_Desc.Text;
                drStkOut["CreateDate"] =
                    DateTime.Parse(txt_Date.Text)
                        .AddHours(ServerDateTime.Hour)
                        .AddMinutes(ServerDateTime.Minute)
                        .AddSeconds(ServerDateTime.Second);
                drStkOut["CreateBy"] = LoginInfo.LoginName;
                drStkOut["UpdateDate"] = ServerDateTime;
                drStkOut["UpdateBy"] = LoginInfo.LoginName;

                dsStockOut.Tables[stockOut.TableName].Rows.Add(drStkOut);
            }
            else
            {
                var drStkOut = dsStockOut.Tables[stockOut.TableName].Rows[0];

                if (MODE.ToUpper() == "EDIT")
                {
                    _action = "MODIFY";
                    foreach (DataRow drStkOutDt in dsStockOut.Tables[stockOutDt.TableName].Rows)
                    {
                        if (drStkOutDt.RowState == DataRowState.Deleted)
                        {
                            continue;
                        }

                        drStkOutDt["RefId"] = drStkOut["RefId"].ToString();
                    }
                }

                drStkOut["Type"] = ddl_Type.Value.ToString();
                drStkOut["Description"] = txt_Desc.Text;
                drStkOut["Status"] = "Saved";
                drStkOut["UpdateDate"] = ServerDateTime;
                drStkOut["UpdateBy"] = LoginInfo.LoginName;

                hdrNo = drStkOut["RefId"].ToString();
            }

            var save = stockOut.Save(dsStockOut, LoginInfo.ConnStr);

            if (save)
            {
                pop_ConfrimSave.ShowOnPageLoad = false;
                pop_Save.ShowOnPageLoad = true;

                //// Added on: 21/09/2017, By: Fon
                //ClassLogTool pctool = new ClassLogTool();
                //pctool.SaveActionLog("SO", hdrNo, "Save");
                //// End Added.
                _transLog.Save("IN", "STKOUT", hdrNo, _action, string.Empty, LoginInfo.LoginName, LoginInfo.ConnStr);

                // Save Success then Redirect to Dt page
                Response.Redirect("StkOutDt.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + dsStockOut.Tables[stockOut.TableName].Rows[0]["RefId"] + "&VID=" + Request.Params["VID"]);
            }

            pop_ConfrimSave.ShowOnPageLoad = false;
            //pop_ConfrimSave.ShowOnPageLoad = true;
        }

        private void Commit()
        {
            dsStockOut = (DataSet)ViewState["dsStockOut"];

            var drStkOut = dsStockOut.Tables[stockOut.TableName].Rows[0];
            drStkOut["Status"] = "Committed";
            drStkOut["CommitDate"] = ServerDateTime;
            drStkOut["UpdateDate"] = ServerDateTime;
            drStkOut["UpdateBy"] = LoginInfo.LoginName;

            foreach (DataRow dr in dsStockOut.Tables[stockOutDt.TableName].Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {

                    DataTable dt = dsStockOut.Tables[stockOutDt.TableName];
                    string sku = dr["SKU"].ToString();
                    string loc = dr["StoreId"].ToString();

                    var onHand = GetOnHand(sku, loc, txt_Date.Text);
                    var qty = decimal.Parse(dr["Qty"].ToString());

                    qty = decimal.Parse(dt.Compute("SUM(Qty)", string.Format("SKU='{0}' AND StoreId='{1}'", sku, loc)).ToString());

                    if (onHand - qty < 0)
                    {
                        lbl_Warning.Text = "Commit quantity of product " + dr["SKU"] + " : " + prod.GetName(dr["SKU"].ToString(), LoginInfo.ConnStr) + ", " + prod.GetName2(dr["SKU"].ToString(), LoginInfo.ConnStr) + " must be less than on hand.";
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }
                }
            }

            var saveStatus = stockOut.Save(dsStockOut, LoginInfo.ConnStr);

            if (saveStatus)
            {
                //Update Inventory
                inv.GetStructure(dsStockOut, LoginInfo.ConnStr);

                //// FIFO 
                var Fifo = new Blue.BL.IN.Inventory.Fifo();

                // initial value
                var HdrNo = string.Empty;
                var Location = string.Empty;
                var ProductCode = string.Empty;
                decimal qtyRequest = 0;
                //decimal amt = 0;

                foreach (DataRow drStkOutDt in dsStockOut.Tables[stockOutDt.TableName].Rows)
                {
                    HdrNo = drStkOutDt["RefId"].ToString();
                    Location = drStkOutDt["StoreId"].ToString();
                    ProductCode = drStkOutDt["SKU"].ToString();

                    qtyRequest = DecimalOrZero(drStkOutDt["Qty"]);

                    Fifo.SaveSO(HdrNo, drStkOutDt["Id"].ToString(), Location, ProductCode, qtyRequest, LoginInfo.ConnStr);
                }

                var result = inv.Save(dsStockOut, LoginInfo.ConnStr);

                if (result)
                {
                    //// Added on: 21/09/2017, By: Fon
                    //ClassLogTool pctool = new ClassLogTool();
                    //pctool.SaveActionLog("SO", lbl_Ref.Text, "Commit");
                    //// End Added.
                    _transLog.Save("IN", "STKOUT", HdrNo, "COMMIT", string.Empty, LoginInfo.LoginName, LoginInfo.ConnStr);

                    Response.Redirect("StkOutDt.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + dsStockOut.Tables[stockOut.TableName].Rows[0]["RefId"] + "&VID=" + Request.Params["VID"]);
                }
            }
        }
        */
        private void SaveAndCommit(bool isCommit)
        {
            pop_ConfrimSave.ShowOnPageLoad = false;

            var startPeriodDate = period.GetLatestOpenStartDate(LoginInfo.ConnStr);
            var endPeriodDate = period.GetLatestOpenEndDate(LoginInfo.ConnStr);
            var docDate = Convert.ToDateTime(txt_Date.Text);

            // Validate required 
            if (docDate < startPeriodDate)
            {
                lbl_Warning.Text = "The date should be during the open period.";
                pop_Warning.ShowOnPageLoad = true;

                return;
            }

            if (isCommit && docDate > endPeriodDate)
            {
                lbl_Warning.Text = string.Format("Stock out is able to commit during the opening period ({0} - {1}).", startPeriodDate.ToString("dd/MM/yyyy"), endPeriodDate.ToString("dd/MM/yyyy"));
                pop_Warning.ShowOnPageLoad = true;

                return;
            }
            




            string docNo = string.Empty;
            string _action = string.Empty;

            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                #region --new--

                var drStkOut = dsStockOut.Tables[stockOut.TableName].NewRow();

                if (MODE.ToUpper() == "NEW" || MODE.ToUpper() == "SDR")
                {
                    _action = "CREATE";

                    docNo = stockOut.GetNewID(DateTime.Parse(txt_Date.Text), LoginInfo.ConnStr);
                    drStkOut["RefId"] = docNo;

                    foreach (DataRow drStkOutDt in dsStockOut.Tables[stockOutDt.TableName].Rows)
                    {
                        if (drStkOutDt.RowState == DataRowState.Deleted)
                            continue;

                        drStkOutDt["RefId"] = docNo;
                    }
                }

                drStkOut["Type"] = ddl_Type.Value.ToString();
                drStkOut["Status"] = "Saved";
                drStkOut["Description"] = txt_Desc.Text.Trim();
                drStkOut["CreateDate"] = docDate;
                drStkOut["CreateBy"] = LoginInfo.LoginName;
                drStkOut["UpdateDate"] = ServerDateTime;
                drStkOut["UpdateBy"] = LoginInfo.LoginName;

                dsStockOut.Tables[stockOut.TableName].Rows.Add(drStkOut);
                #endregion
            }
            else
            {
                #region --edit--
                var drStkOut = dsStockOut.Tables[stockOut.TableName].Rows[0];

                if (MODE.ToUpper() == "EDIT")
                {
                    _action = "MODIFY";
                    foreach (DataRow drStkOutDt in dsStockOut.Tables[stockOutDt.TableName].Rows)
                    {
                        if (drStkOutDt.RowState == DataRowState.Deleted)
                        {
                            continue;
                        }

                        drStkOutDt["RefId"] = drStkOut["RefId"].ToString();
                    }
                }

                drStkOut["Type"] = ddl_Type.Value.ToString();
                drStkOut["Description"] = txt_Desc.Text.Trim();
                drStkOut["Status"] = "Saved";
                drStkOut["UpdateDate"] = ServerDateTime;
                drStkOut["UpdateBy"] = LoginInfo.LoginName;

                docNo = drStkOut["RefId"].ToString();
                #endregion
            }

            var save = stockOut.Save(dsStockOut, LoginInfo.ConnStr);

            if (save)
            {
                _transLog.Save("IN", "STKOUT", docNo, _action, string.Empty, LoginInfo.LoginName, LoginInfo.ConnStr);

                if (isCommit)
                {
                    var query = @"
DECLARE @DocNo nvarchar(20)='{0}'
DECLARE @DocDate DATE = (SELECT CreateDate FROM [IN].StockOut WHERE RefId=@DocNo)
DECLARE @CommittedDate DATETIME = [IN].GetCommittedDate( @DocDate, GETDATE())

;WITH
so AS(
	SELECT
		StoreId,
		SKU,
		SUM(Qty) as Qty
	FROM
		[IN].StockOutDt
	WHERE
		RefId=@DocNo
	GROUP BY
		StoreId,
		SKU
),
onhand AS(
	SELECT
		so.StoreId as [Location],
		so.SKU as ProductCode,
		so.Qty,
		ISNULL(SUM([IN]-[OUT]), 0) as Onhand
	FROM
		so
		LEFT JOIN [IN].Inventory i ON i.Location=so.StoreId AND i.ProductCode=so.SKU AND HdrNo <> @DocNo
	GROUP BY
		so.StoreId,
		so.SKU,
		so.Qty
)
SELECT
	*,
	@CommittedDate as CommittedDate
FROM
	onhand
WHERE
	Qty > Onhand
";
                    var dtOnhand = stockOut.DbExecuteQuery(string.Format(query, docNo), null, LoginInfo.ConnStr);

                    if (dtOnhand != null && dtOnhand.Rows.Count > 0)
                    {
                        var products = dtOnhand.AsEnumerable()
                            .Select(x => string.Format("- {0} : {1}, remain = {2}", x.Field<string>("Location"), x.Field<string>("ProductCode"),x.Field<decimal>("Onhand")))
                            .ToArray();

                        var committedDate = Convert.ToDateTime(dtOnhand.Rows[0]["CommittedDate"]);


                        lbl_Warning.Text = string.Format("These products are not enough onhand on {0}<br/> {1}", committedDate.ToString("dd/MM/yyyy") , string.Join("<br />", products));
                        pop_Warning.ShowOnPageLoad = true;

                        return;
                    }

                    // Commit SO ([IN].In
                    var sql = new Helpers.SQL(LoginInfo.ConnStr);
                    try
                    {
                        sql.ExecuteQuery("EXEC [IN].[SoCommit] @DocNo", new SqlParameter[] { new SqlParameter("@DocNo", docNo) });
                        _transLog.Save("IN", "STKOUT", docNo, "COMMIT", string.Empty, LoginInfo.LoginName, LoginInfo.ConnStr);

                    }
                    catch (Exception ex)
                    {
                        lbl_Warning.Text = ex.Message;
                        pop_Warning.ShowOnPageLoad = true;

                        return;
                    }

                    //stockOut.DbExecuteQuery("EXEC [IN].[SoCommit] @DocNo, NULL", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@DocNo", docNo) }, LoginInfo.ConnStr);
                }

                Response.Redirect("StkOutDt.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + docNo + "&VID=" + Request.Params["VID"]);
            }

            //pop_Save.ShowOnPageLoad = true;
        }




        private void Back()
        {
            if (MODE.ToUpper() == "EDIT")
            {
                Response.Redirect("StkOutDt.aspx?BuCode=" + Request.Params["BuCode"] +
                                  "&ID=" + Request.Params["ID"] +
                                  "&VID=" + Request.Params["VID"]);
            }
            else
            {
                Response.Redirect("StkOutLst.aspx");
            }
        }

        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
        }

        protected void btn_CancelSave_Click(object sender, EventArgs e)
        {
            pop_ConfrimSave.ShowOnPageLoad = false;
        }

        private void ShowStandardRequisition()
        {
            string sql = string.Format(@"SELECT sdr.RefId, sdr.[Description], sdr.LocationCode, l.LocationName
                            FROM [IN].StandardRequisition sdr
                            LEFT JOIN [IN].StoreLocation l ON l.LocationCode = sdr.LocationCode
                            JOIN [ADMIN].UserStore us ON us.LocationCode = sdr.LocationCode
                            WHERE sdr.[Status] = 1
                            AND us.LoginName = '{0}'
                            ORDER BY sdr.LocationCode", LoginInfo.LoginName);

            var dt = inv.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
            gvSDR.DataSource = dt;
            gvSDR.DataBind();
            pop_SDR.ShowOnPageLoad = true;

        }

        protected void gvSDR_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            var refId = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "Select")
            {
                pop_SDR.ShowOnPageLoad = false;
                string url = "~/IN/STK/StkOutEdit.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&MODE=sdr&VID=" + Request.Cookies["[IN].[vStockOut]"].Value;
                Response.Redirect(url + "&refid=" + refId);
            }
        }


        #endregion

        private decimal GetLastCost(string productCode, DateTime toDate, string connStr)
        {
            var sql = @"    SELECT 
		                        TOP(1)	
		                        ISNULL(Amount, 0) as Value
	                        FROM 
		                        [IN].Inventory i
		                        JOIN [IN].StoreLocation l
			                        ON l.LocationCode=i.[Location] AND l.EOP <> 2
	                        WHERE 
		                        ISNULL(Amount, 0) <> 0
		                        AND [Type] IN ('SI','RC')
		                        AND ProductCode = @productCode
		                        AND CAST(CommittedDate as DATE) <= @toDate
	                        ORDER BY 
		                        CommittedDate DESC";

            var dbParams = new DbParameter[]
            {
                new DbParameter("@productCode", productCode),
                new DbParameter("@toDate", toDate.ToString("yyyy-MM-dd"))            
            };

            var dt = stockOut.DbExecuteQuery(sql, dbParams, connStr);

            if (dt.Rows.Count > 0)
            {
                return Convert.ToDecimal(dt.Rows[0][0]);
            }
            else
                return 0;
        }

        #region "GridView"

        protected void grd_StkOutEdit1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            menu_CmdBar.Visible = grd_StkOutEdit1.EditIndex == -1; //Save, Commit, Back
            menu_CmdGrd.Visible = grd_StkOutEdit1.EditIndex == -1; //Create, Delete

            //if (grd_StkOutEdit1.EditIndex != -1)
            //{
            //    menu_CmdBar.Visible = false; //Save, Commit, Back
            //    menu_CmdGrd.Visible = false; //Create, Delete
            //}
            //else
            //{
            //    menu_CmdBar.Visible = true; //Save, Commit, Back
            //    menu_CmdGrd.Visible = true; //Create, Delete
            //}

            // Data
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string productCode = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();

                // View Mode
                if (e.Row.FindControl("lbl_StoreName") != null)
                {
                    var lbl_StoreName = e.Row.FindControl("lbl_StoreName") as Label;
                    lbl_StoreName.Text = DataBinder.Eval(e.Row.DataItem, "StoreId") + " : " + strLct.GetName(DataBinder.Eval(e.Row.DataItem, "StoreId").ToString(), LoginInfo.ConnStr);
                    lbl_StoreName.ToolTip = lbl_StoreName.Text;
                }

                if (e.Row.FindControl("lbl_Item_Desc") != null)
                {
                    var lbl_Item_Desc = e.Row.FindControl("lbl_Item_Desc") as Label;
                    lbl_Item_Desc.Text = DataBinder.Eval(e.Row.DataItem, "SKU") + " : " + product.GetName(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(), LoginInfo.ConnStr) + " : " + product.GetName2(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(), LoginInfo.ConnStr);
                    lbl_Item_Desc.ToolTip = lbl_Item_Desc.Text;
                }

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                    lbl_Unit.ToolTip = lbl_Unit.Text;
                }

                if (e.Row.FindControl("lbl_UnitCost") != null)
                {
                    var lbl_UnitCost = e.Row.FindControl("lbl_UnitCost") as Label;
                    //lbl_UnitCost.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "UnitCost"));

                    lbl_UnitCost.Text = string.Format(DefaultAmtFmt, GetLastCost(productCode, DateTime.Today, LoginInfo.ConnStr));

                    lbl_UnitCost.ToolTip = lbl_UnitCost.Text;
                }

                if (e.Row.FindControl("lbl_Qty") != null)
                {
                    var lbl_Qty = e.Row.FindControl("lbl_Qty") as Label;
                    lbl_Qty.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "Qty"));
                    //Total += decimal.Parse(lbl_Qty.Text);
                    lbl_Qty.ToolTip = Total.ToString();
                }

                if (e.Row.FindControl("hf_Id") != null)
                {
                    var hf = e.Row.FindControl("hf_Id") as HiddenField;
                    hf.Value = DataBinder.Eval(e.Row.DataItem, "Id").ToString();
                }



                // Edit Mode
                if (e.Row.FindControl("ddl_Store") != null)
                {
                    var ddl_Store = e.Row.FindControl("ddl_Store") as ASPxComboBox;
                    ddl_Store.Value = DataBinder.Eval(e.Row.DataItem, "StoreId");
                }

                if (e.Row.FindControl("ddl_Product") != null)
                {
                    var ddl_Product = e.Row.FindControl("ddl_Product") as ASPxComboBox;
                    var ddl_Store = e.Row.FindControl("ddl_Store") as ASPxComboBox;

                    if (ddl_Store != null && ddl_Store.Value != null)
                    {
                        ddl_Product.DataSource = product.GetLookUp_LocationCode(ddl_Store.Value.ToString(), LoginInfo.ConnStr);
                        ddl_Product.ValueField = "ProductCode";
                        ddl_Product.Value = DataBinder.Eval(e.Row.DataItem, "SKU");
                        ddl_Product.DataBind();
                    }
                }

                if (e.Row.FindControl("txt_Qty") != null)
                {
                    var txt_Qty = e.Row.FindControl("txt_Qty") as ASPxSpinEdit;
                    txt_Qty.Text = DataBinder.Eval(e.Row.DataItem, "Qty").ToString();
                }

                //if (e.Row.FindControl("txt_Comment") != null)
                //{
                //    var txt_Comment = e.Row.FindControl("txt_Comment") as TextBox;
                //    txt_Comment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                //}


                //if (e.Row.FindControl("lbl_AvgCost") != null)
                //{
                //    var lbl_AvgCost = e.Row.FindControl("lbl_AvgCost") as Label;
                //    lbl_AvgCost.Text = String.Format(DefaultAmtFmt, inv.GetMAvgAudit(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(), LoginInfo.ConnStr));
                //    lbl_AvgCost.ToolTip = lbl_AvgCost.Text;
                //}


                //if (e.Row.FindControl("txt_UnitCost") != null)
                //{
                //    var txt_UnitCost = e.Row.FindControl("txt_UnitCost") as ASPxSpinEdit;
                //    txt_UnitCost.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "UnitCost"));
                //}

                //if (e.Row.FindControl("txt_Comment") != null)
                //{
                //    var txt_Comment = e.Row.FindControl("txt_Comment") as TextBox;
                //    txt_Comment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                //}
                //if (e.Row.FindControl("lbl_Comment") != null)
                //{
                //    var lbl_Comment = e.Row.FindControl("lbl_Comment") as Label;
                //    lbl_Comment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                //}


                //****************** Display Stock Summary **************
                var dsPrDtStockSum = new DataSet();

                var getPrDtStockSum = prDt.GetStockSummary(dsPrDtStockSum,
                    DataBinder.Eval(e.Row.DataItem, "SKU").ToString(),
                    DataBinder.Eval(e.Row.DataItem, "StoreId").ToString()
                    , DateTime.Today.ToString("yyyy-MM-dd"), LoginInfo.ConnStr);

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
                        lbl_OnOrder.ToolTip = lbl_OnOrder.Text;

                        var lbl_ReOrder = e.Row.FindControl("lbl_ReOrder") as Label;
                        lbl_ReOrder.Text = string.Format(DefaultQtyFmt, drStockSummary["Reorder"].ToString());
                        lbl_ReOrder.ToolTip = lbl_ReOrder.Text;

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



                //if (e.Row.FindControl("lbl_SKU_Ex") != null)
                //{
                //    var lbl_SKU_Ex = e.Row.FindControl("lbl_SKU_Ex") as Label;
                //    lbl_SKU_Ex.Text = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();
                //    lbl_SKU_Ex.ToolTip = lbl_SKU_Ex.Text;
                //}

                //if (e.Row.FindControl("lbl_ItemGroup_Ex") != null)
                //{
                //    var lbl_ItemGroup_Ex = e.Row.FindControl("lbl_ItemGroup_Ex") as Label;
                //    lbl_ItemGroup_Ex.Text =
                //        prodCat.GetName(
                //            prod.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(), LoginInfo.ConnStr),
                //            LoginInfo.ConnStr);
                //    lbl_ItemGroup_Ex.ToolTip = lbl_ItemGroup_Ex.Text;
                //}

                //if (e.Row.FindControl("lbl_SubCategory_Ex") != null)
                //{
                //    var lbl_SubCategory_Ex = e.Row.FindControl("lbl_SubCategory_Ex") as Label;
                //    lbl_SubCategory_Ex.Text =
                //        prodCat.GetName(
                //            prod.GetParentNoByCategoryCode(
                //                prod.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(),
                //                    LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr);
                //    lbl_SubCategory_Ex.ToolTip = lbl_SubCategory_Ex.Text;
                //}

                //if (e.Row.FindControl("lbl_Category_Ex") != null)
                //{
                //    var lbl_Category_Ex = e.Row.FindControl("lbl_Category_Ex") as Label;
                //    lbl_Category_Ex.Text =
                //        prodCat.GetName(
                //            prod.GetParentNoByCategoryCode(
                //                prod.GetParentNoByCategoryCode(
                //                    prod.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(),
                //                        LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr);
                //    lbl_Category_Ex.ToolTip = lbl_Category_Ex.Text;
                //}

                ////****************** Display Stock Movement *****************
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
            }
        }

        protected void grd_StkOutEdit1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            dsStockOut.Tables[stockOutDt.TableName].Rows[e.RowIndex].Delete();

            grd_StkOutEdit1.DataSource = dsStockOut.Tables[stockOutDt.TableName];
            grd_StkOutEdit1.EditIndex = -1;
            grd_StkOutEdit1.DataBind();

            StockOutEditMode = string.Empty;
        }

        protected void grd_StkOutEdit1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grd_StkOutEdit1.DataSource = dsStockOut.Tables[stockOutDt.TableName];
            grd_StkOutEdit1.EditIndex = e.NewEditIndex;
            grd_StkOutEdit1.DataBind();

            StockOutEditMode = "EDIT";
        }

        protected void grd_StkOutEdit1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (MODE.ToUpper() == "NEW" || MODE.ToUpper() == "SDR")
            {
                if (StockOutEditMode.ToUpper() == "NEW")
                {
                    dsStockOut.Tables[stockOutDt.TableName].Rows[dsStockOut.Tables[stockOutDt.TableName].Rows.Count - 1].Delete();
                }

                if (StockOutEditMode.ToUpper() == "EDIT")
                {
                    dsStockOut.Tables[stockOutDt.TableName].Rows[dsStockOut.Tables[stockOutDt.TableName].Rows.Count - 1].CancelEdit();
                }
            }
            else if (MODE.ToUpper() == "EDIT")
            {
                if (StockOutEditMode.ToUpper() == "NEW")
                {
                    dsStockOut.Tables[stockOutDt.TableName].Rows[dsStockOut.Tables[stockOutDt.TableName].Rows.Count - 1].Delete();
                }

                if (StockOutEditMode.ToUpper() == "EDIT")
                {
                    dsStockOut.Tables[stockOutDt.TableName].Rows[dsStockOut.Tables[stockOutDt.TableName].Rows.Count - 1].CancelEdit();
                }
            }

            //// Enable Create & Delete when click create button on detail grid.
            menu_CmdGrd.Enabled = true;

            txt_Date.Enabled = true;

            grd_StkOutEdit1.DataSource = dsStockOut.Tables[stockOutDt.TableName];
            grd_StkOutEdit1.EditIndex = -1;
            grd_StkOutEdit1.DataBind();

            StockOutEditMode = string.Empty;
        }

        protected void grd_StkOutEdit1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var ddl_Store = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("ddl_Store") as ASPxComboBox;
            var ddl_Product = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
            var ddl_Debit = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("ddl_Debit") as ASPxComboBox;
            var ddl_Credit = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("ddl_Credit") as ASPxComboBox;
            var txt_Qty = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("txt_Qty") as ASPxSpinEdit;
            var txt_Comment = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("txt_Comment") as TextBox;
            var txt_UnitCost =
                grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("txt_UnitCost") as ASPxSpinEdit;
            var lbl_Unit = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("lbl_Unit") as Label;
            var ddl_Unit = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("ddl_Unit") as ASPxComboBox;

            //Check field cannot empty
            if (ddl_Store != null)
            {
                if (ddl_Store.Value == null)
                {
                    lbl_Warning.Text = "'Store' cannot be empty";
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }
            }

            if (ddl_Product != null)
            {
                if (ddl_Product.Value == null)
                {
                    lbl_Warning.Text = "'SKU #' cannot be empty";
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }
            }

            if (txt_Qty != null)
            {
                if (txt_Qty.Text == string.Empty)
                {
                    lbl_Warning.Text = "'Qty' cannot be empty";
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }

                if (decimal.Parse(txt_Qty.Text) == 0)
                {
                    lbl_Warning.Text = "'Qty' must have value";
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }
            }

            var drUpdating = dsStockOut.Tables[stockOutDt.TableName].Rows[grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].DataItemIndex];
            drUpdating["StoreId"] = ddl_Store.Value.ToString().Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            drUpdating["SKU"] = ddl_Product.Value.ToString().Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            drUpdating["Qty"] = decimal.Parse(txt_Qty.Text);
            drUpdating["Unit"] = lbl_Unit.Text;
            //drUpdating["Unit"] = ddl_Unit.Value;
            //drUpdating["UnitCost"] = decimal.Parse(txt_UnitCost.Text);
            drUpdating["Comment"] = txt_Comment.Text == string.Empty ? null : txt_Comment.Text;

            grd_StkOutEdit1.DataSource = dsStockOut.Tables[stockOutDt.TableName];
            grd_StkOutEdit1.EditIndex = -1;
            grd_StkOutEdit1.DataBind();

            //// Enable Save & Commit Button On Header.
            //menu_CmdBar.Items[0].Enabled = true; //Save.
            //menu_CmdBar.Items[1].Enabled = true; //Commit.

            //// Enable Create & Delete when click create button on detail grid.
            //menu_CmdGrd.Enabled = true;

            txt_Date.Enabled = true;

            StockOutEditMode = string.Empty;
        }

        protected void grd_StkOutEdit1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SaveNew")
            {
                var lnkb_SaveNew = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("lnkb_SaveNew") as LinkButton;

                var ddl_Store = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("ddl_Store") as ASPxComboBox;
                var ddl_Product = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
                var ddl_Debit = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("ddl_Debit") as ASPxComboBox;
                var ddl_Credit = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("ddl_Credit") as ASPxComboBox;
                var txt_Qty = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("txt_Qty") as ASPxSpinEdit;
                var txt_Comment = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("txt_Comment") as TextBox;
                var txt_UnitCost = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("txt_UnitCost") as ASPxSpinEdit;
                var ddl_Unit = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("ddl_Unit") as ASPxComboBox;

                var lbl_UnitCost = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("lbl_UnitCost") as Label;
                var lbl_Unit = grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].FindControl("lbl_Unit") as Label;

                //Check field cannot empty
                if (ddl_Store != null)
                {
                    if (ddl_Store.Value == null)
                    {
                        lbl_Warning.Text = "'Store' cannot be empty";
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }
                }

                if (ddl_Product != null)
                {
                    if (ddl_Product.Value == null)
                    {
                        lbl_Warning.Text = "'SKU #' cannot be empty";
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }
                }

                if (txt_Qty != null)
                {
                    if (txt_Qty.Text == string.Empty)
                    {
                        lbl_Warning.Text = "'Qty' cannot empty";
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }

                    if (decimal.Parse(txt_Qty.Text) == 0)
                    {
                        lbl_Warning.Text = "'Qty' cannot equal zero";
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }
                }

                var drUpdating = dsStockOut.Tables[stockOutDt.TableName].Rows[grd_StkOutEdit1.Rows[grd_StkOutEdit1.EditIndex].DataItemIndex];
                drUpdating["StoreId"] = ddl_Store.Value.ToString().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                drUpdating["SKU"] = ddl_Product.Value.ToString().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                drUpdating["Qty"] = decimal.Parse(txt_Qty.Text);
                drUpdating["Unit"] = lbl_Unit.Text.Split(' ')[0].Trim();
                drUpdating["UnitCost"] = decimal.Parse(lbl_UnitCost.Text);
                drUpdating["Comment"] = txt_Comment.Text == string.Empty ? null : txt_Comment.Text;

                grd_StkOutEdit1.DataSource = dsStockOut.Tables[stockOutDt.TableName];
                grd_StkOutEdit1.EditIndex = -1;
                grd_StkOutEdit1.DataBind();

                StockOutEditMode = string.Empty;

                Create();
            }
            else if (e.CommandName == "Create")
            {
                Create();
            }
            else if (e.CommandName == "EditQty")
            {
                var id = Convert.ToInt32(e.CommandArgument.ToString());

                var dr = dsStockOut.Tables[stockOutDt.TableName].Select(string.Format("Id={0}", id.ToString())).FirstOrDefault();

                string qty = dr["Qty"].ToString();
                txt_EditQty.Text = qty;
                pop_EditQty.ShowOnPageLoad = true;
            }

        }

        protected void btn_SaveEditQty_Click(object sender, EventArgs e)
        {
            pop_EditQty.ShowOnPageLoad = false;
        }
        #endregion


        public class Product_Location_Onhand
        {
            public string LocationCode { get; set; }
            public string ProudctCode { get; set; }
            public decimal Qty { get; set; }
            public decimal Onhand { get; set; }
        }
    }
}