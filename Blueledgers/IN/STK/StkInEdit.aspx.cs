using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxTabControl;
using System.Data.SqlClient;

namespace BlueLedger.PL.IN.STK
{
    public partial class StkInEdit : BasePage
    {
        #region "Attributes"



        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();
        private readonly Blue.BL.IN.AdjType adjType = new Blue.BL.IN.AdjType();
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.IN.Inventory inv = new Blue.BL.IN.Inventory();
        private readonly Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.Option.Inventory.Product prod = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private readonly Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.IN.StockIn stkIn = new Blue.BL.IN.StockIn();
        private readonly Blue.BL.IN.StockInDt stkInDt = new Blue.BL.IN.StockInDt();
        private readonly Blue.BL.Option.Inventory.StoreLct strLct = new Blue.BL.Option.Inventory.StoreLct();
        private string StkId = string.Empty;

        private decimal Total;

        private DataSet dsStkInEdit = new DataSet();
        private Blue.BL.GnxLib gnx = new Blue.BL.GnxLib();
        private Blue.BL.Option.Inventory.Unit unit = new Blue.BL.Option.Inventory.Unit();

        private string StkInEditMode
        {
            get { return Session["StkInEditMode"].ToString(); }
            set { Session["StkInEditMode"] = value; }
        }

        private string MODE
        {
            get { return Request.QueryString["MODE"]; }
        }

        private readonly Blue.BL.APP.Module module = new Blue.BL.APP.Module();
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
                dsStkInEdit = (DataSet)Session["dsStkInEdit"];
            }
        }

        private void Page_Retrieve()
        {
            dsStkInEdit.Clear();

            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                var get = stkIn.GetSchema(dsStkInEdit, hf_ConnStr.Value);

                if (!get)
                {
                    return;
                }

                var getDt = stkInDt.GetSchema(dsStkInEdit, hf_ConnStr.Value);

                if (!getDt)
                {
                    return;
                }
            }
            else
            {
                StkId = Request.Params["ID"];

                var get = stkIn.Get(dsStkInEdit, StkId, hf_ConnStr.Value);

                if (!get)
                {
                    return;
                }

                var getDt = stkInDt.Get(dsStkInEdit, StkId, hf_ConnStr.Value);

                if (!getDt)
                {
                    return;
                }
            }

            Session["dsStkInEdit"] = dsStkInEdit;

            Page_Setting();
        }

        private void Page_Setting()
        {
            if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                var drStkIn = dsStkInEdit.Tables[stkIn.TableName].Rows[0];

                txt_Date.Text = DateTime.Parse(drStkIn["CreateDate"].ToString()).ToString("dd/MM/yyyy");
                txt_Date.Enabled = false;
                lbl_Ref.Text = drStkIn["RefId"].ToString();
                lbl_Ref.ToolTip = lbl_Ref.Text;
                lbl_Status.Text = drStkIn["Status"].ToString();
                lbl_Status.ToolTip = lbl_Status.Text;
                ddl_Type.Value = Convert.ToInt32(drStkIn["Type"]);
                txt_Desc.Text = drStkIn["Description"].ToString();

                menu_CmdBar.Items[1].Visible = true;
            }
            else
            {
                dsStkInEdit.Clear();

                //menu_CmdBar.Items[1].Visible = false;
                menu_CmdBar.Items[1].Visible = true;


                txt_Date.Text = ServerDateTime.ToShortDateString();
                // Assign Stock In Type automatically if only one.
                ddl_Type.SelectedIndex = ddl_Type.Items.Count > 1 ? -1 : 0;
            }


            grd_StkInEdit.DataSource = dsStkInEdit.Tables[stkInDt.TableName];
            grd_StkInEdit.DataBind();
        }



        protected void btn_Save_Success_Click(object sender, EventArgs e)
        {
            //Save();
            pop_ConfirmSave.ShowOnPageLoad = false;
            pop_Save.ShowOnPageLoad = false;
            Response.Redirect("StkInDt.aspx?BuCode=" + Request.Params["BuCode"] +
                  "&ID=" + dsStkInEdit.Tables[stkIn.TableName].Rows[0]["RefId"]
                  + "&VID=" + Request.Params["VID"]);

        }

        protected void ddl_Unit_Load(object sender, EventArgs e)
        {
            var ddl_Unit = sender as ASPxComboBox;
            var ddl_Product = sender as ASPxComboBox;

            if (ddl_Product != null && ddl_Unit.Value != null)
            {
                var s = ddl_Product.Value.ToString().Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                ddl_Unit.DataSource = prodUnit.GetLookUp_InvenUnit(s, hf_ConnStr.Value);
                ddl_Unit.DataBind();
            }
        }

        protected void grd_StkInEdit_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper() == "SAVENEW")
            {
                var ddl_Store = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("ddl_Store") as ASPxComboBox;
                var ddl_Product = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
                var ddl_Debit = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("ddl_Debit") as ASPxComboBox;
                var ddl_Credit = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("ddl_Credit") as ASPxComboBox;
                var txt_Qty = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("txt_Qty") as ASPxSpinEdit;
                var txt_UnitCost =
                    grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("txt_UnitCost") as ASPxSpinEdit;
                var txt_Comment = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("txt_Comment") as TextBox;
                var ddl_Unit = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("ddl_Unit") as ASPxComboBox;
                var hf_Cost = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("hf_Cost") as HiddenField;

                //Check field cannot empty
                if (ddl_Store != null)
                {
                    if (ddl_Store.Value == null)
                    {
                        lbl_Warning.Text = "'Store' is required.";
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }
                }

                if (ddl_Product != null)
                {
                    if (ddl_Product.Value == null)
                    {
                        lbl_Warning.Text = "'SKU #' is required.";
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }
                }

                if (txt_Qty != null)
                {
                    if (txt_Qty.Text == string.Empty)
                    {
                        lbl_Warning.Text = "'Qty' is requried.";
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }
                }

                if (txt_UnitCost != null)
                {
                    if (txt_UnitCost.Text == string.Empty)
                    {
                        lbl_Warning.Text = "'Unit Cost' is required.";
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }
                }

                var drUpdating =
                    dsStkInEdit.Tables[stkInDt.TableName].Rows[grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].DataItemIndex
                        ];
                drUpdating["StoreId"] =
                    ddl_Store.Value.ToString().Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                drUpdating["SKU"] =
                    ddl_Product.Value.ToString().Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                drUpdating["Qty"] = decimal.Parse(txt_Qty.Text);
                drUpdating["Unit"] = ddl_Unit.Value;
                drUpdating["UnitCost"] = decimal.Parse(txt_UnitCost.Text);
                drUpdating["Comment"] = txt_Comment.Text == string.Empty ? null : txt_Comment.Text;

                //// Enable Save & Commit Button On Header.
                //menu_CmdBar.Items[0].Enabled = true; //Save.
                //menu_CmdBar.Items[1].Enabled = true; //Commit.

                //// Enable Create & Delete when click create button on detail grid.
                //menu_CmdGrd.Enabled = true;

                grd_StkInEdit.DataSource = dsStkInEdit.Tables[stkInDt.TableName];
                grd_StkInEdit.EditIndex = -1;
                grd_StkInEdit.DataBind();

                StkInEditMode = string.Empty;

                Create();
            }

            if (e.CommandName == "Create")
            {
                Create();
            }
        }

        protected void grd_StkInEdit_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            dsStkInEdit.Tables[stkInDt.TableName].Rows[e.RowIndex].Delete();

            grd_StkInEdit.DataSource = dsStkInEdit.Tables[stkInDt.TableName];
            grd_StkInEdit.EditIndex = -1;
            grd_StkInEdit.DataBind();

            StkInEditMode = string.Empty;
        }

        protected void Img_Create_Click(object sender, ImageClickEventArgs e)
        {
            Create();
        }

        protected void btn_SavedClose_Click(object sender, ImageClickEventArgs e)
        {
            Save();
            pop_Save.ShowOnPageLoad = false;
        }

        //protected void btn_Save_Success_Click1(object sender, EventArgs e)
        //{
        //    Save();
        //    pop_Save.ShowOnPageLoad = false;
        //}

        protected void txt_Date_TextChanged(object sender, EventArgs e)
        {
            for (var i = 0; i < grd_StkInEdit.Rows.Count; i++)
            {
                var lbl_Store = grd_StkInEdit.Rows[i].FindControl("lbl_StoreName") as Label;

                if (!period.GetIsValidDate(DateTime.Parse(txt_Date.Text), lbl_Store.Text, LoginInfo.ConnStr))
                {
                    lbl_WarningPeriod.Text = "Store " + lbl_Store.Text.Split(':')[1].Trim() + " is not period.";
                    pop_WarningPeriod.ShowOnPageLoad = true;
                    return;
                }
            }
        }

        protected void btn_WarningPeriod_Click(object sender, EventArgs e)
        {
            pop_WarningPeriod.ShowOnPageLoad = false;
        }

        protected void ddl_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        #region "Editors"

        protected void btn_ConfirmSave_Click(object sender, EventArgs e)
        {
            pop_ConfirmSave.ShowOnPageLoad = false;
            if (Save())
            {
                Response.Redirect("StkInDt.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" +
                                  dsStkInEdit.Tables[stkIn.TableName].Rows[0]["RefId"] +
                                  "&VID=" + Request.Params["VID"]);
            }

            //pop_Save.ShowOnPageLoad = true;
        }

        protected void btn_ConfirmCommit_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                pop_ConfirmCommit.ShowOnPageLoad = false;
                if (Commit())
                {
                    // Added on: 21/09/2017, By: Fon
                    ClassLogTool pctool = new ClassLogTool();
                    pctool.SaveActionLog("SI", lbl_Ref.Text, "Commit");
                    // End Added.

                    pop_Save.ShowOnPageLoad = true;
                }
                else
                {
                    lbl_Warning.Text = "Commit fail!";
                    pop_Warning.ShowOnPageLoad = true;
                }
            }
        }

        protected void btn_ComfirmDelete_Click(object sender, EventArgs e)
        {
            for (var i = grd_StkInEdit.Rows.Count - 1; i >= 0; i--)
            {
                var chk_Item = grd_StkInEdit.Rows[i].Cells[0].FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    var drStkInDt = dsStkInEdit.Tables[stkInDt.TableName].Rows[i];

                    if (drStkInDt.RowState != DataRowState.Deleted)
                    {
                        drStkInDt.Delete();
                    }
                }
            }

            grd_StkInEdit.DataSource = dsStkInEdit.Tables[stkInDt.TableName];
            grd_StkInEdit.EditIndex = -1;
            grd_StkInEdit.DataBind();

            pop_ConfirmDelete.ShowOnPageLoad = false;
        }

        protected void ddl_Type_Load(object sender, EventArgs e)
        {
            var comboBox = (ASPxComboBox)sender;
            comboBox.DataSource = adjType.GetList(lbl_StockIn.Text, LoginInfo.ConnStr);
            comboBox.ValueField = "AdjID";
            comboBox.DataBind();
            if (comboBox.Value != null)
            {
                //comboBox.Text = adjType.GetName(comboBox.Value.ToString(), LoginInfo.ConnStr);
            }
        }

        protected void ddl_Store_Load(object sender, EventArgs e)
        {
            var ddl_StoreCode = sender as ASPxComboBox;

            // Modified on: 12/02/2018, By: Fon, For: P' Oat request.
            //ddl_StoreCode.DataSource = strLct.GetList(LoginInfo.LoginName, hf_ConnStr.Value);
            //ddl_StoreCode.DataBind();
            var ds = strLct.GetList(LoginInfo.LoginName, LoginInfo.ConnStr);
            ds.DefaultView.RowFilter = "EOP <> 2";
            var dt = ds.DefaultView.ToTable();

            ddl_StoreCode.DataSource = dt;
            ddl_StoreCode.DataBind();
            // End Modified.
        }

        //protected void ddl_Store_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ASPxComboBox ddl_Product    = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
        //    ASPxComboBox ddl_Store      = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("ddl_Store") as ASPxComboBox;

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

        protected void ddl_Product_Load(object sender, EventArgs e)
        {
			var ddl_Product = sender as ASPxComboBox;
			var ddl_Store = ddl_Product.Parent.FindControl("ddl_Store") as ASPxComboBox;
			
			if (ddl_Store.Value != null)
			{
			var locationCode = ddl_Store.Value.ToString();
			
			
			var sql = string.Format("SELECT p.ProductCode, p.ProductDesc1, p.ProductDesc2 FROM [IN].ProdLoc pl JOIN [IN].Product p ON p.ProductCode = pl.ProductCode WHERE pl.LocationCode = '{0}'", locationCode );
			
			var dt = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
			
			ddl_Product.DataSource = dt;
			ddl_Product.DataBind();
			}
			
        }

        protected void ddl_Product_ItemRequestedByValue(object source, ListEditItemRequestedByValueEventArgs e)
        {
            var comboBox = source as ASPxComboBox;
            if (comboBox == null) return;
            var sqlDataSource1 = new SqlDataSource();
            var ddlStore = comboBox.Parent.FindControl("ddl_Store") as ASPxComboBox;

            if (e.Value == null || ddlStore.Value == null) return;

            try
            {
                new ProductLookup().ItemRequestedByValue(ref comboBox, sqlDataSource1, hf_ConnStr.Value, e,
                    ddlStore.Value.ToString());
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
        }

        protected void ddl_Product_ItemsRequestedByFilterCondition(object source,
            ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            var ddlProduct = source as ASPxComboBox;
            var sqlDataSource1 = new SqlDataSource();
            var ddlStore = ddlProduct.Parent.FindControl("ddl_Store") as ASPxComboBox;

            if (ddlProduct == null) return;

            if (ddlStore.Value == null || e.Filter == null) return;

            //if (string.IsNullOrEmpty())
            //{
            //    ddlProduct.Value = null;
            //    ddlProduct.Text = string.Empty;
            //    return;
            //}

            var comboBox = (ASPxComboBox)source;
            new ProductLookup().ItemsRequestedByFilterCondition(ref comboBox, sqlDataSource1, hf_ConnStr.Value, e,
                ddlStore.Value.ToString());
        }

        protected void ddl_Product_SelectedIndexChanged(object sender, EventArgs e)
        {
			var ddl_Product = sender as ASPxComboBox;
			
			if (ddl_Product.SelectedItem == null)
				return;
			
			var ddl_Unit = ddl_Product.Parent.FindControl("ddl_Unit") as ASPxComboBox;
            var ddl_Store = ddl_Product.Parent.FindControl("ddl_Store") as ASPxComboBox;
            var txt_UnitCost = ddl_Product.Parent.FindControl("txt_UnitCost") as ASPxSpinEdit;
            var hf_ProductCode = ddl_Product.Parent.FindControl("hf_ProductCode") as HiddenField;
            var hf_Cost = ddl_Product.Parent.FindControl("hf_Cost") as HiddenField;

            //Get StartDate and EndDate for update Avg in inventory
            var startDate = period.GetStartDate(ServerDateTime.Date, hf_ConnStr.Value);
            var endDate = period.GetEndDate(ServerDateTime.Date, hf_ConnStr.Value);

            var p = ddl_Product.Value.ToString().Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
			var productCode = ddl_Product.Value.ToString().Trim();
			
			
            if (ddl_Unit != null)
            {
				//lbl_StockIn.Text = ddl_Product.Value.ToString();
				// var sql = string.Format("SELECT InventoryUnit FROM [IN].Product WHERE ProductCode = '{0}'", productCode );
				// var dtUnit = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
				
                //ddl_Unit.DataSource = prodUnit.GetLookUp_InvenUnit(p, hf_ConnStr.Value);
				var dtUnit = GetProductInventoryUnit(productCode);
				ddl_Unit.DataSource = dtUnit;
                ddl_Unit.DataBind();
				
				ddl_Unit.Value = dtUnit.Rows[0][0].ToString();
            }

            //ddl_Unit.Value = prodUnit.GetInvenUnitType(p, hf_ConnStr.Value);

            var dsPrDtStockSum = new DataSet();

            var getPrDtStockSum = prDt.GetStockSummary(dsPrDtStockSum, p, ddl_Store.Value.ToString(), txt_Date.Text,
                hf_ConnStr.Value);

            if (getPrDtStockSum)
            {
                if (dsPrDtStockSum.Tables[prDt.TableName].Rows.Count > 0)
                {
                    var drStockSummary = dsPrDtStockSum.Tables[prDt.TableName].Rows[0];

                    var lbl_OnHand = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("lbl_OnHand") as Label;
                    lbl_OnHand.Text = drStockSummary["OnHand"].ToString();
                    lbl_OnHand.ToolTip = lbl_OnHand.Text;

                    var lbl_OnOrder = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("lbl_OnOrder") as Label;
                    lbl_OnOrder.Text = drStockSummary["OnOrder"].ToString();
                    lbl_OnOrder.ToolTip = lbl_OnHand.Text;

                    var lbl_ReOrder = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("lbl_ReOrder") as Label;
                    lbl_ReOrder.Text = drStockSummary["Reorder"].ToString();
                    lbl_ReOrder.ToolTip = lbl_OnHand.Text;

                    var lbl_Restock = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("lbl_Restock") as Label;
                    lbl_Restock.Text = drStockSummary["Restock"].ToString();
                    lbl_Restock.ToolTip = lbl_Restock.Text;

                    var lbl_LastPrice =
                        grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("lbl_LastPrice") as Label;
                    lbl_LastPrice.Text = drStockSummary["LastPrice"].ToString();
                    lbl_LastPrice.ToolTip = lbl_LastPrice.Text;

                    var lbl_LastVendor =
                        grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("lbl_LastVendor") as Label;
                    lbl_LastVendor.Text = drStockSummary["LastVendor"].ToString();
                    lbl_LastVendor.ToolTip = lbl_LastVendor.Text;
                }
            }

            hf_ProductCode.Value =
                ddl_Product.Value.ToString().Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            hf_Cost.Value = txt_UnitCost.Text;
        }
		
		private DataTable GetProductInventoryUnit(string productCode)
		{
			var sql = string.Format("SELECT InventoryUnit FROM [IN].Product WHERE ProductCode = '{0}'", productCode );
			var dtUnit = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
			
			return dtUnit;
		}

        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            pop_ConfirmDelete.ShowOnPageLoad = false;
        }

        //private DateTime GetOpenPeriod()
        //{
        //    SqlConnection conn = new SqlConnection(LoginInfo.ConnStr);
        //    conn.Open();

        //    string sql;
        //    sql = "SELECT DATEADD(DAY, DATEDIFF(DAY, 1, CAST(EndDate  AS DATE)), '23:59:00:000') as OpenPeriod ";
        //    sql = sql + " FROM [IN].Period";
        //    sql = sql + " WHERE IsClose='false' or IsClose IS NULL";
        //    sql = sql + " ORDER BY [Year] ASC, PeriodNumber ASC";

        //    var cmd = new SqlCommand(sql, conn);

        //    cmd.ExecuteScalar();

        //    SqlDataReader reader = cmd.ExecuteReader();
        //    reader.Read();

        //    return DateTime.Parse(reader["OpenPeriod"].ToString());

        //}



        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":
                    if (ddl_Type.Value == null)
                    {
                        lbl_Warning.Text = "Stock Type is required.";
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }

                    if (grd_StkInEdit.Rows.Count == 0)
                    {
                        lbl_Warning.Text = "No any details found.";
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }

                    pop_ConfirmSave.ShowOnPageLoad = true;

                    break;

                case "COMMIT":
                    pop_ConfirmCommit.ShowOnPageLoad = true;
                    break;

                case "BACK":
                    if (Request.Params["MODE"].ToUpper() == "EDIT")
                    {
                        Response.Redirect("StkInDt.aspx?BuCode=" + Request.Params["BuCode"] +
                                          "&ID=" + Request.Params["ID"] +
                                          "&VID=" + Request.Params["VID"]);
                    }
                    else
                    {
                        Response.Redirect("StkInLst.aspx");
                    }
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
            }
        }

        private void Create()
        {
            // Disable Save & Commit Button On Header.
            //menu_CmdBar.Items[0].Enabled = false; //Save.
            //menu_CmdBar.Items[1].Enabled = false; //Commit.

            // Disable Create & Delete when click create button on detail grid.
            //menu_CmdGrd.Enabled = false;

            txt_Date.Enabled = false;

            // Add new stkInDt row
            var drNew = dsStkInEdit.Tables[stkInDt.TableName].NewRow();

            drNew["Id"] = string.Empty;
            drNew["RefId"] = (dsStkInEdit.Tables[stkInDt.TableName].Rows.Count == 0
                ? 1
                : int.Parse(
                    dsStkInEdit.Tables[stkInDt.TableName].Rows[dsStkInEdit.Tables[stkInDt.TableName].Rows.Count - 1][
                        "RefId"].ToString()) + 1);
            drNew["UnitCost"] = 0;
            if (dsStkInEdit.Tables[stkInDt.TableName].Rows.Count > 0)
            {
                drNew["StoreId"] =
                    dsStkInEdit.Tables[stkInDt.TableName].Rows[dsStkInEdit.Tables[stkInDt.TableName].Rows.Count - 1][
                        "StoreId"].ToString();
            }

            dsStkInEdit.Tables[stkInDt.TableName].Rows.Add(drNew);

            grd_StkInEdit.DataSource = dsStkInEdit.Tables[stkInDt.TableName];
            grd_StkInEdit.EditIndex = dsStkInEdit.Tables[stkInDt.TableName].Rows.Count - 1;
            grd_StkInEdit.DataBind();

            StkInEditMode = "NEW";
        }

        private void Delete()
        {
            pop_ConfirmDelete.ShowOnPageLoad = true;
        }

        private bool Save()
        {
            string hdrNo = string.Empty;
            string _action = string.Empty;

            ////Check StockIn Detail.It must have data before save.
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                var drStkIn = dsStkInEdit.Tables[stkIn.TableName].NewRow();

                if (MODE.ToUpper() == "NEW")
                {
                    _action = "CREATE";
                    drStkIn["RefId"] = stkIn.GetNewID(DateTime.Parse(txt_Date.Text), hf_ConnStr.Value);

                    foreach (DataRow drStkInDt in dsStkInEdit.Tables[stkInDt.TableName].Rows)
                    {
                        if (drStkInDt.RowState == DataRowState.Deleted)
                        {
                            continue;
                        }

                        drStkInDt["Id"] = drStkIn["RefId"].ToString();
                    }
                }

                drStkIn["Type"] = ddl_Type.Value.ToString();
                drStkIn["Status"] = "Saved";
                drStkIn["Description"] = txt_Desc.Text;
                drStkIn["CreateDate"] = DateTime.Parse(txt_Date.Text).AddHours(ServerDateTime.Hour).AddMinutes(ServerDateTime.Minute).AddSeconds(ServerDateTime.Second);
                drStkIn["CreateBy"] = LoginInfo.LoginName;
                drStkIn["UpdateDate"] = ServerDateTime;
                drStkIn["UpdateBy"] = LoginInfo.LoginName;

                dsStkInEdit.Tables[stkIn.TableName].Rows.Add(drStkIn);
                hdrNo = drStkIn["RefId"].ToString();
            }
            else
            {
                var drStkIn = dsStkInEdit.Tables[stkIn.TableName].Rows[0];

                if (MODE.ToUpper() == "EDIT")
                {
                    _action = "MODIFY";
                    foreach (DataRow drStkInDt in dsStkInEdit.Tables[stkInDt.TableName].Rows)
                    {
                        if (drStkInDt.RowState == DataRowState.Deleted)
                        {
                            continue;
                        }

                        drStkInDt["Id"] = drStkIn["RefId"].ToString();
                    }
                }

                drStkIn["Type"] = ddl_Type.Value.ToString();
                drStkIn["Description"] = txt_Desc.Text;
                drStkIn["Status"] = "Saved";
                drStkIn["UpdateDate"] = ServerDateTime;
                drStkIn["UpdateBy"] = LoginInfo.LoginName;

                hdrNo = drStkIn["RefId"].ToString();
            }

            inv.Delete(hdrNo, LoginInfo.ConnStr);

            bool returnStkIn = stkIn.Save(dsStkInEdit, hf_ConnStr.Value);
            _transLog.Save("IN", "STKIN", hdrNo, _action, string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);
            return returnStkIn;
            // End Modified;
        }

        private bool Commit()
        {

            DateTime OpenPeriod = period.GetLatestOpenEndDate(LoginInfo.ConnStr).AddHours(23).AddMinutes(56);
            DateTime InvCommittedDate;

            if (DateTime.Today > OpenPeriod)  // Over than open period (DateTime)
            {
                if (DateTime.Parse(txt_Date.Text).Date <= OpenPeriod.Date)
                    InvCommittedDate = OpenPeriod;
                else
                    InvCommittedDate = DateTime.Now;
            }
            else // In period
                InvCommittedDate = DateTime.Now;


            dsStkInEdit = (DataSet)Session["dsStkInEdit"];

            var drStkIn = dsStkInEdit.Tables[stkIn.TableName].Rows[0];
            drStkIn["Status"] = "Committed";
            drStkIn["CommitDate"] = DateTime.Parse(txt_Date.Text).AddHours(ServerDateTime.Hour).AddMinutes(ServerDateTime.Minute).AddSeconds(ServerDateTime.Second);
            drStkIn["UpdateBy"] = LoginInfo.LoginName;
            drStkIn["UpdateDate"] = DateTime.Parse(txt_Date.Text).AddHours(ServerDateTime.Hour).AddMinutes(ServerDateTime.Minute).AddSeconds(ServerDateTime.Second);

            var saveStatus = stkIn.Save(dsStkInEdit, LoginInfo.ConnStr);

            if (saveStatus)
            {
                //Update Inventory
                var invent = inv.GetStructure(dsStkInEdit, LoginInfo.ConnStr);

                if (!invent)
                {
                    return false;
                }

                foreach (DataRow drStkInDt in dsStkInEdit.Tables[stkInDt.TableName].Rows)
                {
                    var drInv = dsStkInEdit.Tables[inv.TableName].NewRow();

                    decimal unitCost = Convert.ToDecimal(drStkInDt["UnitCost"].ToString());
                    decimal qty = Convert.ToDecimal(drStkInDt["Qty"].ToString());

                    drInv["HdrNo"] = drStkInDt["Id"].ToString();
                    drInv["DtNo"] = drStkInDt["RefId"].ToString();
                    drInv["InvNo"] = drStkInDt["RefId"].ToString();
                    drInv["Location"] = drStkInDt["StoreId"].ToString();
                    drInv["ProductCode"] = drStkInDt["SKU"].ToString();
                    drInv["IN"] = RoundQty(qty);
                    drInv["OUT"] = 0;
                    drInv["Amount"] = RoundAmt(unitCost);
                    drInv["FIFOAudit"] = RoundAmt(unitCost);
                    drInv["CommittedDate"] = InvCommittedDate;
                    drInv["Type"] = "SI";
                    drInv["PriceOnLots"] = RoundAmt(unitCost * qty);
                    dsStkInEdit.Tables[inv.TableName].Rows.Add(drInv);

                }

                bool result = inv.Save(dsStkInEdit, LoginInfo.ConnStr);

                if (result)
                {
                    string docNo = dsStkInEdit.Tables[stkIn.TableName].Rows[0]["RefId"].ToString();
                    inv.UpdateAverageCostByDocument(docNo, LoginInfo.ConnStr);
                }

                string hdrNo = dsStkInEdit.Tables[inv.TableName].Rows[0]["HdrNo"].ToString();
                _transLog.Save("IN", "STKIN", hdrNo, "COMMIT", string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);

                return result;
            }
            else
                return false;

        }

        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
        }

        protected void btn_CancelSave_Click(object sender, EventArgs e)
        {
            pop_ConfirmSave.ShowOnPageLoad = false;
        }

        #endregion

        #region "Grid View"

        protected void grd_StkInEdit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (grd_StkInEdit.EditIndex != -1)
            {
                menu_CmdBar.Visible = false; //Save, Commit, Back
                menu_CmdGrd.Visible = false; //Create, Delete
            }
            else
            {
                menu_CmdBar.Visible = true; //Save, Commit, Back
                menu_CmdGrd.Visible = true; //Create, Delete
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //// Template
                //if (e.Row.FindControl("tp_Information") != null)
                //{
                var tp_Information = e.Row.FindControl("tp_Information") as ASPxPageControl;
                var hf_Cost = e.Row.FindControl("hf_Cost") as HiddenField;

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
                        ddl_Product.DataSource = product.GetLookUp_LocationCode(ddl_Store.Value.ToString(), hf_ConnStr.Value);
                        ddl_Product.ValueField = "ProductCode";
                        ddl_Product.Value = DataBinder.Eval(e.Row.DataItem, "SKU");
                        ddl_Product.DataBind();
                    }
                }

                if (e.Row.FindControl("ddl_Unit") != null)
                {
                    var ddl_Unit = e.Row.FindControl("ddl_Unit") as ASPxComboBox;
                    var ddl_Product = e.Row.FindControl("ddl_Product") as ASPxComboBox;

                    if (ddl_Product != null && ddl_Product.Value != null)
                    {
                        //var s = ddl_Product.Value.ToString().Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                        //ddl_Unit.DataSource = prodUnit.GetLookUp_ProductCode(s, hf_ConnStr.Value);
						var productCode = ddl_Product.Value.ToString();
						var dtUnit = GetProductInventoryUnit(productCode);
						ddl_Unit.DataSource = dtUnit;
                        ddl_Unit.DataBind();
						ddl_Unit.Value = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();

                    }
                }


                if (e.Row.FindControl("txt_Qty") != null)
                {
                    var txt_Qty = e.Row.FindControl("txt_Qty") as ASPxSpinEdit;
                    txt_Qty.Text = string.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "Qty").ToString());
                }

                if (e.Row.FindControl("txt_UnitCost") != null)
                {
                    var txt_UnitCost = e.Row.FindControl("txt_UnitCost") as ASPxSpinEdit;
                    txt_UnitCost.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "UnitCost").ToString());

                }

                if (e.Row.FindControl("ddl_Debit") != null)
                {
                    var ddl_Debit = e.Row.FindControl("ddl_Debit") as ASPxComboBox;
                    ddl_Debit.Value = DataBinder.Eval(e.Row.DataItem, "DebitAcc");
                }

                if (e.Row.FindControl("ddl_Credit") != null)
                {
                    var ddl_Credit = e.Row.FindControl("ddl_Credit") as ASPxComboBox;
                    ddl_Credit.Value = DataBinder.Eval(e.Row.DataItem, "CreditAcc");
                }

                if (e.Row.FindControl("txt_Comment") != null)
                {
                    var txt_Comment = e.Row.FindControl("txt_Comment") as TextBox;
                    txt_Comment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                }

                // Grid Line.
                if (e.Row.FindControl("lbl_LocationCode") != null)
                {
                    var lbl_LocationCode = e.Row.FindControl("lbl_LocationCode") as Label;
                    lbl_LocationCode.Text = DataBinder.Eval(e.Row.DataItem, "StoreId").ToString();
                    lbl_LocationCode.ToolTip = lbl_LocationCode.Text;
                }

                if (e.Row.FindControl("lbl_LocationName") != null)
                {
                    var lbl_LocationName = e.Row.FindControl("lbl_LocationName") as Label;
                    lbl_LocationName.Text = DataBinder.Eval(e.Row.DataItem, "StoreId") + " : " +
                                            strLct.GetName(DataBinder.Eval(e.Row.DataItem, "StoreId").ToString(),
                                                hf_ConnStr.Value);
                    lbl_LocationName.ToolTip = lbl_LocationName.Text;
                }

                if (e.Row.FindControl("lbl_StoreName") != null)
                {
                    var lbl_StoreName = e.Row.FindControl("lbl_StoreName") as Label;
                    lbl_StoreName.Text = DataBinder.Eval(e.Row.DataItem, "StoreId") + " : " +
                                         strLct.GetName(DataBinder.Eval(e.Row.DataItem, "StoreId").ToString(),
                                             hf_ConnStr.Value);
                    lbl_StoreName.ToolTip = lbl_StoreName.Text;
                }

                if (e.Row.FindControl("lbl_ProductCode") != null)
                {
                    var lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                    lbl_ProductCode.Text = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();
                    lbl_ProductCode.ToolTip = lbl_ProductCode.Text;
                }

                if (e.Row.FindControl("lbl_Item_Desc") != null)
                {
                    var lbl_Item_Desc = e.Row.FindControl("lbl_Item_Desc") as Label;
                    lbl_Item_Desc.Text = DataBinder.Eval(e.Row.DataItem, "SKU") + " : " +
                                         product.GetName(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(),
                                             hf_ConnStr.Value) + " : " +
                                         product.GetName2(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(),
                                             hf_ConnStr.Value);
                    lbl_Item_Desc.ToolTip = lbl_Item_Desc.Text;
                }

                if (e.Row.FindControl("lbl_EnglishName") != null)
                {
                    var lbl_EnglishName = e.Row.FindControl("lbl_EnglishName") as Label;
                    lbl_EnglishName.Text = product.GetName(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(),
                        hf_ConnStr.Value);
                    lbl_EnglishName.ToolTip = lbl_EnglishName.Text;
                }

                if (e.Row.FindControl("lbl_LocalName") != null)
                {
                    var lbl_LocalName = e.Row.FindControl("lbl_LocalName") as Label;
                    lbl_LocalName.Text = product.GetName2(DataBinder.Eval(e.Row.DataItem, "SKU").ToString(),
                        hf_ConnStr.Value);
                    lbl_LocalName.ToolTip = lbl_LocalName.Text;
                }

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                    lbl_Unit.ToolTip = lbl_Status.Text;
                }

                if (e.Row.FindControl("lbl_Qty") != null)
                {
                    var lbl_Qty = e.Row.FindControl("lbl_Qty") as Label;
                    lbl_Qty.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "Qty"));
                    Total += decimal.Parse((lbl_Qty.Text == string.Empty ? "0" : lbl_Qty.Text));
                    lbl_Qty.ToolTip = Total.ToString();
                }

                if (e.Row.FindControl("lbl_UnitCost") != null)
                {
                    var lbl_UnitCost = e.Row.FindControl("lbl_UnitCost") as Label;
                    lbl_UnitCost.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "UnitCost"));
                    lbl_UnitCost.ToolTip = lbl_UnitCost.Text;
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

                //****************** Display Stock Summary **************
                var dsPrDtStockSum = new DataSet();

                var getPrDtStockSum = prDt.GetStockSummary(dsPrDtStockSum,
                    DataBinder.Eval(e.Row.DataItem, "SKU").ToString()
                    , DataBinder.Eval(e.Row.DataItem, "StoreId").ToString(), txt_Date.Text, hf_ConnStr.Value);

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
            }

            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    if (e.Row.FindControl("lbl_TotalQty") != null)
            //    {
            //        Label lbl_TotalQty = e.Row.FindControl("lbl_TotalQty") as Label;
            //        lbl_TotalQty.Text = String.Format("{0:N}", Total);
            //        lbl_TotalQty.ToolTip = lbl_TotalQty.Text;
            //    }
            //}
        }

        protected void grd_StkInEdit_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //// Disable Save & Commit Button On Header.
            //menu_CmdBar.Items[0].Enabled = false; //Save.
            //menu_CmdBar.Items[1].Enabled = false; //Commit.

            //// Disable Create & Delete when click create button on detail grid.
            //menu_CmdGrd.Enabled = false;

            grd_StkInEdit.DataSource = dsStkInEdit.Tables[stkInDt.TableName];
            grd_StkInEdit.EditIndex = e.NewEditIndex;
            grd_StkInEdit.DataBind();

            StkInEditMode = "EDIT";
        }

        protected void grd_StkInEdit_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var ddl_Store = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("ddl_Store") as ASPxComboBox;
            var ddl_Product = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
            var ddl_Debit = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("ddl_Debit") as ASPxComboBox;
            var ddl_Credit = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("ddl_Credit") as ASPxComboBox;
            var txt_Qty = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("txt_Qty") as ASPxSpinEdit;
            var txt_UnitCost = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("txt_UnitCost") as ASPxSpinEdit;
            var txt_Comment = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("txt_Comment") as TextBox;
            //Label lbl_Unit = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("lbl_Unit") as Label;
            var ddl_Unit = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("ddl_Unit") as ASPxComboBox;
            var hf_Cost = grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].FindControl("hf_Cost") as HiddenField;

            //Check field cannot empty
            if (ddl_Store != null)
            {
                if (ddl_Store.Value == null)
                {
                    lbl_Warning.Text = "'Store' is required.";
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }
            }

            if (ddl_Product != null)
            {
                if (ddl_Product.Value == null)
                {
                    lbl_Warning.Text = "'SKU #' is requied.";
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }
            }

            if (txt_Qty != null)
            {
                if (txt_Qty.Text == string.Empty)
                {
                    lbl_Warning.Text = "'Qty' is required.";
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }
            }

            if (txt_UnitCost != null)
            {
                if (txt_UnitCost.Text == string.Empty)
                {
                    lbl_Warning.Text = "'Unit Cost' is required.";
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }
            }

            var drUpdating =
                dsStkInEdit.Tables[stkInDt.TableName].Rows[grd_StkInEdit.Rows[grd_StkInEdit.EditIndex].DataItemIndex];
            drUpdating["StoreId"] = ddl_Store.Value.ToString().Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            drUpdating["SKU"] = ddl_Product.Value.ToString().Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            drUpdating["Qty"] = RoundQty(decimal.Parse(txt_Qty.Text));
            drUpdating["Unit"] = ddl_Unit.Value;
            drUpdating["UnitCost"] = RoundAmt(decimal.Parse(txt_UnitCost.Text));
            drUpdating["Comment"] = txt_Comment.Text == string.Empty ? null : txt_Comment.Text;

            txt_Date.Enabled = true;

            grd_StkInEdit.DataSource = dsStkInEdit.Tables[stkInDt.TableName];
            grd_StkInEdit.EditIndex = -1;
            grd_StkInEdit.DataBind();

            StkInEditMode = string.Empty;
        }

        protected void grd_StkInEdit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                if (StkInEditMode.ToUpper() == "NEW")
                {
                    dsStkInEdit.Tables[stkInDt.TableName].Rows[dsStkInEdit.Tables[stkInDt.TableName].Rows.Count - 1]
                        .Delete();
                }

                if (StkInEditMode.ToUpper() == "EDIT")
                {
                    dsStkInEdit.Tables[stkInDt.TableName].Rows[dsStkInEdit.Tables[stkInDt.TableName].Rows.Count - 1]
                        .CancelEdit();
                }
            }

            if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                if (StkInEditMode.ToUpper() == "NEW")
                {
                    dsStkInEdit.Tables[stkInDt.TableName].Rows[dsStkInEdit.Tables[stkInDt.TableName].Rows.Count - 1]
                        .Delete();
                }

                if (StkInEditMode.ToUpper() == "EDIT")
                {
                    dsStkInEdit.Tables[stkInDt.TableName].Rows[dsStkInEdit.Tables[stkInDt.TableName].Rows.Count - 1]
                        .CancelEdit();
                }
            }

            //// Enable Save & Commit Button On Header.
            //menu_CmdBar.Items[0].Enabled = true; //Save.
            //menu_CmdBar.Items[1].Enabled = true; //Commit.

            //// Enable Create & Delete when click create button on detail grid.
            //menu_CmdGrd.Enabled = true;

            txt_Date.Enabled = true;

            grd_StkInEdit.DataSource = dsStkInEdit.Tables[stkInDt.TableName];
            grd_StkInEdit.EditIndex = -1;
            grd_StkInEdit.DataBind();

            StkInEditMode = string.Empty;
        }

        #endregion
    }
}