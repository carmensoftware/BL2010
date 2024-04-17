using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
//using DevExpress.Xpo.Metadata;

namespace BlueLedger.PL.PC.EOP
{
    public partial class EOPEdit : BasePage
    {
        #region variable
        private readonly Blue.BL.PC.EOP eop = new Blue.BL.PC.EOP();
        private readonly Blue.BL.PC.EOPDt eopDt = new Blue.BL.PC.EOPDt();
        private readonly Blue.BL.IN.Inventory inventory = new Blue.BL.IN.Inventory();
        private readonly Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.Option.Inventory.StoreLct store = new Blue.BL.Option.Inventory.StoreLct();
        private Blue.BL.Option.Admin.Interface.AccountMapp accMapp = new Blue.BL.Option.Admin.Interface.AccountMapp();
        private Blue.BL.IN.AdjType adjType = new Blue.BL.IN.AdjType();
        private DataSet dsEOP = new DataSet();

        // Addded on: 22/09/2017, By: Fon
        private int eopID
        { get { return int.Parse(Request.Params["ID"]); } }
        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsEOP = (DataSet)Session["dsEOP"];
            }
        }

        private void Page_Retrieve()
        {
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                // New
                var getEopStruct = eop.GetStructure(dsEOP, LoginInfo.ConnStr);

                if (getEopStruct)
                {
                    // Check any peiod is open.
                    if (period.GetLatestOpenStartDate(LoginInfo.ConnStr) == DateTime.Parse("1900-01-01 00:00:00") ||
                        period.GetLatestOpenEndDate(LoginInfo.ConnStr) == DateTime.Parse("1900-01-01 00:00:00"))
                    {
                        // Display warning message to create period and redirect to inventory's period setup page.                        
                    }

                    // Add default new row
                    var drNewEOP = dsEOP.Tables[eop.TableName].NewRow();
                    drNewEOP["EopId"] = eop.GetNewID(LoginInfo.ConnStr);
                    drNewEOP["Date"] = ServerDateTime.Date;
                    drNewEOP["Status"] = "Open";
                    drNewEOP["StartDate"] = period.GetLatestOpenStartDate(LoginInfo.ConnStr);
                    drNewEOP["EndDate"] = period.GetLatestOpenEndDate(LoginInfo.ConnStr);
                    //                    drNewEOP["CreateDate"] = ServerDateTime;
                    drNewEOP["CreateDate"] = period.GetLatestOpenEndDate(LoginInfo.ConnStr);
                    drNewEOP["CreateBy"] = LoginInfo.LoginName;
                    drNewEOP["UpdateDate"] = ServerDateTime;
                    drNewEOP["UpdateBy"] = LoginInfo.LoginName;
                    dsEOP.Tables[eop.TableName].Rows.Add(drNewEOP);
                }
                else
                {
                    // Display Error Message
                    return;
                }

                var getEopDtStruct = eopDt.GetStructure(dsEOP, LoginInfo.ConnStr);

                if (!getEopDtStruct)
                {
                    // Display Error Message
                    return;
                }
            }
            else
            {
                // Edit
                var EOPId = int.Parse(Request.Params["ID"]);

                var getEOP = eop.Get(dsEOP, EOPId, LoginInfo.ConnStr);

                if (!getEOP)
                {
                    // Display Error Message
                    return;
                }

                var drEop = dsEOP.Tables[eop.TableName].Rows[0];

                var getEOPDt = eopDt.GetList(dsEOP, EOPId, LoginInfo.ConnStr);

                if (!getEOPDt)
                {
                    // Display Error Message
                    return;
                }
            }

            Session["dsEOP"] = dsEOP;

            Page_Setting();
        }

        private void Page_Setting()
        {
            var drEop = dsEOP.Tables[eop.TableName].Rows[0];

            ddl_Store.Value = drEop["StoreId"].ToString();
            txt_Date.Date = DateTime.Parse(drEop["Date"].ToString()).Date;
            lbl_EndDate.Text = DateTime.Parse(drEop["EndDate"].ToString()).ToString("dd/MM/yyyy");
            lbl_Status.Text = drEop["Status"].ToString();
            txt_Desc.Text = drEop["Description"].ToString();
            txt_Remark.Text = drEop["Remark"].ToString();

            grd_Product.DataSource = dsEOP.Tables[eopDt.TableName];
            grd_Product.DataBind();


            if (drEop["Status"].ToString().ToUpper() == "OPEN")
            {
                btn_Commit.Visible = false;
                btn_Save.Visible = false;
                btn_Delete.Visible = false;

                //column index = 4.Means column Quantity in gridview.
                grd_Product.Columns[4].Visible = false;
            }

            // User cannot save if document status is not "open"
            btn_Save.Enabled = (drEop["Status"].ToString().ToUpper() == "PRINTED" ||
                                drEop["Status"].ToString().ToUpper().Trim() == "IN PROCESS"
                ? true
                : false);

            // User cannot commit if doucument status is not "Saved"
            btn_Commit.Enabled = (drEop["Status"].ToString().ToUpper() == "PRINTED" ||
                                  drEop["Status"].ToString().ToUpper().Trim() == "IN PROCESS"
                ? true
                : false);

            btn_Delete.Enabled = (drEop["Status"].ToString().ToUpper() == "PRINTED" ||
                                  drEop["Status"].ToString().ToUpper().Trim() == "IN PROCESS"
                ? true
                : false);
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                // Redirect to list page.
                Response.Redirect("EOPList.aspx");
            }
            else
            {
                // Redirect to detail page.
                Response.Redirect("EOP.aspx?BuCode=" + Request.Params["BuCode"] +
                                  "&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"]);
            }
        }

        protected void ddl_Store_Load(object sender, EventArgs e)
        {
            var drEop = dsEOP.Tables[eop.TableName].Rows[0];

            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                ddl_Store.DataSource = store.GetList(1, LoginInfo.LoginName, DateTime.Parse(drEop["EndDate"].ToString()),
                    LoginInfo.ConnStr);
                ddl_Store.DataBind();
            }
            else
            {
                ddl_Store.DataSource = store.GetList(1, LoginInfo.LoginName, LoginInfo.ConnStr);
                ddl_Store.Enabled = false;
                ddl_Store.DataBind();
            }
        }

        private void GetProductList(string locationCode)
        {
            //var conn = new SqlConnection(LoginInfo.ConnStr);
            //conn.Open();

            //string sql;
            //sql = "SELECT ROW_NUMBER() OVER(ORDER BY p.ProductCode) AS RowNo, ";
            //sql = sql + " p.ProductCode, p.ProductDesc1, p.ProductDesc2, p.InventoryUnit, CAST(0 as NUMERIC(18,6)) as Qty ";
            //sql = sql + " FROM [IN].Product p";
            //sql = sql + " JOIN [IN].ProdLoc pl ON (p.ProductCode=pl.ProductCode)";
            //sql = sql + " WHERE pl.LocationCode='" + locationCode + "'";
            //sql = sql + " AND p.IsActive='true'";
            //sql = sql + " ORDER BY p.ProductCode";

            //var cmd = new SqlCommand(sql, conn);

            //var adapter = new SqlDataAdapter(cmd);

            //var data = new DataSet();
            //adapter.Fill(data, "ProductList");

            //// Set row select property to grid
            //ASPxGridView1.KeyFieldName = "ProductCode";
            ////ASPxGridView1.Settings.ShowGroupPanel = true;
            //ASPxGridView1.Settings.GridLines = GridLines.Horizontal;
            ////ASPxGridView1.Settings.ShowGroupPanel = false;
            ////ASPxGridView1.Settings.ShowGroupButtons = false;
            ////ASPxGridView1.SettingsBehavior.AllowFocusedRow = true;
            ////ASPxGridView1.SettingsBehavior.AllowGroup = false;
            ////ASPxGridView1.SettingsBehavior.AllowDragDrop = false;
            ////ASPxGridView1.SettingsBehavior.ColumnResizeMode = DevExpress.Web.ASPxGridView.ColumnResizeMode.NextColumn;
            ////ASPxGridView1.SettingsPager.Mode = DevExpress.Web.ASPxGridView.GridViewPagerMode.ShowAllRecords;
            //////ASPxGridView1.SettingsPager.PageSize = 1000;
            ////ASPxGridView1.SettingsPager.RenderMode = DevExpress.Web.ASPxClasses.ControlRenderMode.Lightweight;
            ////ASPxGridView1.SettingsEditing.Mode = DevExpress.Web.ASPxGridView.GridViewEditingMode.Inline;

            ////ASPxGridView1.AutoGenerateColumns = false;
            ////ASPxGridView1.Columns.Clear();
            ////GridViewDataColumn col0 = new GridViewDataColumn("RowNo", "No.");
            ////col0.Width = 50;
            ////GridViewDataColumn col1 = new GridViewDataColumn("ProductCode", "Code#");
            ////GridViewDataColumn col2 = new GridViewDataColumn("ProductDesc1", "Description");
            ////GridViewDataColumn col3 = new GridViewDataColumn("ProductDesc2", "Other Description");
            ////GridViewDataColumn col4 = new GridViewDataColumn("Qty", "Quantity");

            ////ASPxGridView1.Columns.Add(col0);
            ////ASPxGridView1.Columns.Add(col1);
            ////ASPxGridView1.Columns.Add(col2);
            ////ASPxGridView1.Columns.Add(col3);
            ////ASPxGridView1.Columns.Add(col4);

            //// Set data
            //ASPxGridView1.DataSource = data;
            //ASPxGridView1.DataBind();

            //conn.Close();
        }



        protected void ddl_Store_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear any data in EOP Detail
            dsEOP.Tables[eopDt.TableName].Clear();

            // Assing Product to EOP Detail -------------------------------------------------------
            // Get active product related to selected store.
            var dsProduct = new DataSet();


            var getProduct = product.GetActiveList(dsProduct, ddl_Store.Value.ToString(), LoginInfo.ConnStr);
            //Blue.DAL.DbHandler db = new Blue.DAL.DbHandler();
            //db.DbRead(

            if (getProduct)
            {
                // Insert the product data to EOP detail for prepare to counting
                foreach (DataRow drProduct in dsProduct.Tables[product.TableName].Rows)
                {
                    var drNewEOPDt = dsEOP.Tables[eopDt.TableName].NewRow();

                    drNewEOPDt["EopDtId"] = dsEOP.Tables[eopDt.TableName].Rows.Count + 1;
                    drNewEOPDt["EopId"] = dsEOP.Tables[eop.TableName].Rows[0]["EopId"].ToString();
                    drNewEOPDt["ProductCode"] = drProduct["ProductCode"];
                    drNewEOPDt["ProductDesc1"] = drProduct["ProductDesc1"];
                    drNewEOPDt["ProductDesc2"] = drProduct["ProductDesc2"];
                    drNewEOPDt["InventoryUnit"] = drProduct["InventoryUnit"];

                    dsEOP.Tables[eopDt.TableName].Rows.Add(drNewEOPDt);
                }
            }
            // ------------------------------------------------------------------------------------

            // Rebbinding grd_Products    
            grd_Product.DataSource = dsEOP.Tables[eopDt.TableName];

            grd_Product.AllowPaging = true;
            grd_Product.PageSize = 100;
            grd_Product.DataBind();

            //GetProductList(ddl_Store.Value.ToString());
        }

        protected void grd_Product_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd_Product.PageIndex = e.NewPageIndex;
            grd_Product.DataBind();
        }

        protected void grd_Product_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("txt_Qty") != null)
                {
                    //var txt_Qty = (TextBox)e.Row.FindControl("txt_Qty");
                    var txt_Qty = e.Row.FindControl("txt_Qty") as ASPxSpinEdit;
                    txt_Qty.Text = string.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "Qty").ToString());
                    txt_Qty.DecimalPlaces = DefaultQtyDigit;
                }

                //if (inventory.GetPAvgAudit(
                //    Convert.ToDateTime(dsEOP.Tables[eop.TableName].Rows[0]["StartDate"].ToString())
                //        .ToString("yyyy-MM-dd 00:00:00"),
                //    Convert.ToDateTime(dsEOP.Tables[eop.TableName].Rows[0]["EndDate"].ToString())
                //        .ToString("yyyy-MM-dd 00:00:00"),
                //    string.Empty,
                //    DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr) == 0)
                //{
                //    // comment เพื่อให้มี rows {op 07/09/2013}
                //    //e.Row.Visible = false;
                //}
            }
        }

        protected void btn_Print_Click(object sender, EventArgs e)
        {
            Save("Printed");

            // Print Report
            Report rpt = new Report();
            rpt.PrintForm(this, "../../RPT/PrintForm.aspx", dsEOP.Tables[eop.TableName].Rows[0]["EopId"].ToString(), "EopForm");
        }

        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            pop_DeleteConfirm.ShowOnPageLoad = true;
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            pop_SaveConfirm.ShowOnPageLoad = true;
        }

        protected void btn_SaveConfirm_OK_Click(object sender, EventArgs e)
        {
            Save("In Process");

            // Show Success Message
            pop_SaveConfirm.ShowOnPageLoad = false;
            //pop_UpdateSuccess.ShowOnPageLoad = true;
            Response.Redirect("EOP.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"]);
        }

        protected void btn_SaveConfirm_Cancel_Click(object sender, EventArgs e)
        {
            pop_SaveConfirm.ShowOnPageLoad = false;
        }
        

        protected void btn_Commit_Click(object sender, EventArgs e)
        {
            int eopID = Convert.ToInt32(Request.Params["ID"].Trim().ToString());

            // Check Qty is empty (null)
            DataTable dt = eop.DbExecuteQuery(string.Format("SELECT COUNT(EopDtId) as RecordCount FROM [IN].EopDt WHERE EopId={0} AND Qty IS NULL", eopID.ToString()), null, LoginInfo.ConnStr);
            int recordCount = Convert.ToInt32(dt.Rows[0][0]);

            if (recordCount > 0)
            {
                lbl_Alert.Text = string.Format("There are {0:D} product(s) are not assigned quantity.", recordCount);
                pop_Alert.ShowOnPageLoad = true;
            }
            else
                pop_CommitConfirm.ShowOnPageLoad = true;
        }

        protected void btn_CommitConfirm_Yes_Click(object sender, EventArgs e)
        {
            //// Check for null Quantity before commit
            //foreach (GridViewRow gvrProduct in grd_Product.Rows)
            //{
            //    var txt_Qty = (TextBox)gvrProduct.FindControl("txt_Qty");
            //    if (txt_Qty.Text.Trim() == string.Empty)
            //    {
            //        pop_CommitConfirm.ShowOnPageLoad = false;
            //        pop_Alert.ShowOnPageLoad = true;
            //        return;
            //    }
            //}

            Commit();

            #region
            //this.Save("Committed");

            ////Get inventory for update
            ////inventory.GetListByHdrNo(dsEOP, dsEOP.Tables[eop.TableName].Rows[0]["EopId"].ToString(), LoginInfo.ConnStr);

            //// Update Average Cost and Insert Consumption
            //DataRow drEOP = dsEOP.Tables[eop.TableName].Rows[0];

            //for (int i = 0; i <= grd_Product.Rows.Count - 1; i++)
            //{ 
            //    DataRow drEOPDt = dsEOP.Tables[eopDt.TableName].Rows[i];

            //    // Insert Consimption by Product by Store
            //    inventory.InsertConsumption(int.Parse(drEOPDt["EOPId"].ToString()), int.Parse(drEOPDt["EOPDtId"].ToString()),
            //        drEOP["StoreId"].ToString(), drEOPDt["ProductCode"].ToString(), DateTime.Parse(drEOP["StartDate"].ToString()),
            //        DateTime.Parse(drEOP["EndDate"].ToString()), decimal.Parse(drEOPDt["Qty"].ToString()),
            //        ServerDateTime.Date, LoginInfo.ConnStr);

            //    inventory.SetPAvgAudit(DateTime.Parse(drEOP["StartDate"].ToString()), DateTime.Parse(drEOP["EndDate"].ToString()), 
            //        drEOP["StoreId"].ToString(), drEOPDt["ProductCode"].ToString(), LoginInfo.ConnStr);
            //}

            //// Show Success Message
            //pop_CommitConfirm.ShowOnPageLoad = false;
            //pop_UpdateSuccess.ShowOnPageLoad = true;
            #endregion
        }

        protected void btn_Alert_OK_Click(object sender, EventArgs e)
        {
            //// Check for null Quantity before commit
            //foreach (GridViewRow gvrProduct in grd_Product.Rows)
            //{
            //    var txt_Qty = (TextBox)gvrProduct.FindControl("txt_Qty");

            //    if (txt_Qty.Text.Trim() == string.Empty)
            //    {
            //        txt_Qty.Text = "0";
            //    }
            //}

            //Commit();
            pop_Alert.ShowOnPageLoad = false;
        }

        protected void btn_UpdateSuccess_OK_Click(object sender, EventArgs e)
        {
            // Redirect to detail page.
            Response.Redirect("EOP.aspx?BuCode=" + Request.Params["BuCode"] +
                              "&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"]);
        }

        private void Save(string DocStatus)
        {
            // Prepare data before save
            // EOP Header
            var drEOP = dsEOP.Tables[eop.TableName].Rows[0];

            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                drEOP["EopId"] = eop.GetNewID(LoginInfo.ConnStr);
                drEOP["StoreId"] = ddl_Store.Value.ToString().Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                drEOP["Date"] = txt_Date.Date.Date;
                drEOP["Status"] = DocStatus;
                drEOP["Description"] = txt_Desc.Text.Trim();
                drEOP["Remark"] = txt_Remark.Text.Trim();
                //drEOP["StartDate"]    = Default when transaction was creating in Page_Retrieve
                //drEOP["EndDate"]      = Default when transaction was creating in Page_Retrieve
                drEOP["UpdateDate"] = ServerDateTime;
                drEOP["UpdateBy"] = LoginInfo.LoginName;
            }
            else
            {

                drEOP["StoreId"] = ddl_Store.Value.ToString().Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                drEOP["Date"] = txt_Date.Date.Date;
                drEOP["Status"] = DocStatus;
                drEOP["Description"] = txt_Desc.Text.Trim();
                drEOP["Remark"] = txt_Remark.Text.Trim();
                //drEOP["StartDate"]    = Default when transaction was creating in Page_Retrieve
                //drEOP["EndDate"]      = Default when transaction was creating in Page_Retrieve
                drEOP["UpdateDate"] = ServerDateTime;
                drEOP["UpdateBy"] = LoginInfo.LoginName;

                // EOP Detail
                for (var i = 0; i <= grd_Product.Rows.Count - 1; i++)
                {
                    var drEOPDt = dsEOP.Tables[eopDt.TableName].Rows[i];

                    if (Request.Params["MODE"].ToUpper() == "NEW")
                    {
                        drEOPDt["EopId"] = drEOP["EopId"];
                    }

                    // Capture Qty from User Entry
                    var txt_Qty = (ASPxSpinEdit)grd_Product.Rows[i].FindControl("txt_Qty");

                    if (txt_Qty.Text.Trim() != string.Empty)
                    {
                        drEOPDt["Qty"] = txt_Qty.Value;
                    }
                }
            }

            // Save to database.
            var saved = eop.Save(dsEOP, LoginInfo.ConnStr);
            if (saved)
            {
                //CreateAccountMap(dsEOP, LoginInfo.ConnStr);
                // Display Success Message

                // Added on: 22/09/2017, By: Fon
                ClassLogTool pctool = new ClassLogTool();
                pctool.SaveActionLog("EOP", drEOP["EopId"].ToString(), "Save");
                // End Added.
            }
        }

        private void CreateAccountMap(DataSet dsEOP, string connStr)
        {
            //foreach (DataRow item in dsEOP.Tables[eopDt.TableName].Rows)
            //{
            //    var p = product.GetProductCategory(item["ProductCode"].ToString(), connStr);

            //    var s = "BusinessUnitCode = '" + Request.Params["BuCode"].ToString() + "'";
            //    s += " and StoreCode = '" + ddl_Store.Value.ToString() + "'";
            //    //s += " and ItemGroupCode = '" + p + "'";
            //    s += " and A1 = 'EOP'";
            //    s += " and PostType = 'GL' ";

            //    DataSet ds = new DataSet();
            //    accMapp.GetStructure(ds, connStr);

            //    DataTable dt = accMapp.GetList(connStr);
            //    var drs = dt.Select(s).ToList();

            //    if (drs.Count <= 0)
            //    {
            //        DataRow dr = ds.Tables[accMapp.TableName].NewRow();
            //        dr["ID"] = Guid.NewGuid(); // accMapp.GetNewID(connStr);
            //        dr["BusinessUnitCode"] = Request.Params["BuCode"].ToString();
            //        dr["StoreCode"] = ddl_Store.Value.ToString();
            //        //dr["ItemGroupCode"] = p;
            //        dr["A1"] = "EOP";
            //        dr["PostType"] = "GL";

            //        ds.Tables[accMapp.TableName].Rows.Add(dr);

            //        bool save = accMapp.Save(ds, connStr);

            //        if (save)
            //        {
            //            Response.Write("SUCCESS");
            //        }

            //    }
            //}
        }

        protected void btn_UncountItem_Click(object sender, EventArgs e)
        {
            // Check for null Quantity before commit
            foreach (GridViewRow gvrProduct in grd_Product.Rows)
            {
                var txt_Qty = (TextBox)gvrProduct.FindControl("txt_Qty");

                if (txt_Qty.Text.Trim() != string.Empty)
                {
                    gvrProduct.Visible = false;
                }
            }
        }

        private void Commit()
        {
            //Save("Committed");
            Save("In Process"); //Note By Fon: They just Save on top then update status on bottom.

            int eopID = Convert.ToInt32(Request.Params["ID"].Trim().ToString());

            //// Remove existing EOP
            //eop.DbExecuteQuery(string.Format("DELETE FROM [IN].Inventory WHERE HdrNo = '{0}' AND [Type]='EOP'", eopID.ToString()), null, LoginInfo.ConnStr);
            //// Insert by batch
            //eop.DbExecuteQuery(string.Format("[IN].[InsertConsumption] @EOPId={0}", eopID.ToString()), null, LoginInfo.ConnStr);
            //// Update EOP In if available
            //eop.DbExecuteQuery(string.Format("EXEC [IN].UpdateCostOfEopIn @EOPId={0}", eopID.ToString()), null, LoginInfo.ConnStr);
            eop.DbExecuteQuery(string.Format("EXEC [IN].InsertEOP @EOPId={0}", eopID.ToString()), null, LoginInfo.ConnStr);


            // Update EOP Status
            UpdateEopStatus(eopID, "Committed");


            // Added on: 22/09/2017, By: Fon
            ClassLogTool pctool = new ClassLogTool();
            pctool.SaveActionLog("EOP", eopID.ToString(), "Commit");
            // End Added.
            //_transLog.Save("PC", "PR", refNo, _action, string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);

            // Show Success Message
            pop_CommitConfirm.ShowOnPageLoad = false;
            //pop_Alert.ShowOnPageLoad = false;
            //pop_UpdateSuccess.ShowOnPageLoad = true;

            Response.Redirect("EOP.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"]);

        }

        private void UpdateEopStatus(int eopId, string textStatus)
        {
            string sqlUpdate = string.Format("UPDATE [IN].Eop SET [Status] = '{0}' WHERE EopId = {1}", textStatus, eopId.ToString());


            using (SqlConnection sqlConn = new SqlConnection(LoginInfo.ConnStr))
            {
                sqlConn.Open();

                // Update command
                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, sqlConn);

                cmdUpdate.ExecuteNonQuery();
                sqlConn.Close();
            }
        }


        protected void btn_CommitConfirm_No_Click(object sender, EventArgs e)
        {
            pop_CommitConfirm.ShowOnPageLoad = false;
        }

        protected void btn_Delete_Ok_Click(object sender, EventArgs e)
        {
            foreach (DataRow drDelete in dsEOP.Tables[eopDt.TableName].Rows)
            {
                if (drDelete.RowState != DataRowState.Deleted)
                {
                    drDelete.Delete();
                }
            }

            eopDt.Save(dsEOP, LoginInfo.ConnStr);

            dsEOP.Tables[eop.TableName].Rows[0].Delete();

            var save = eop.Save(dsEOP, LoginInfo.ConnStr);

            if (save)
            {
                Response.Redirect("EOPList.aspx");
            }
        }

        protected void btn_Delete_Cancel_Click(object sender, EventArgs e)
        {
            pop_DeleteConfirm.ShowOnPageLoad = false;
        }

        protected void btn_SetZero_Click(object sender, EventArgs e)
        {
            //var rows = grd_Product.Rows;
            foreach (GridViewRow row in grd_Product.Rows)
            {
                var txt_Qty = row.FindControl("txt_Qty") as ASPxSpinEdit;
                if (txt_Qty.Text == string.Empty)
                    txt_Qty.Text = "0";
            }
        }
    }
}