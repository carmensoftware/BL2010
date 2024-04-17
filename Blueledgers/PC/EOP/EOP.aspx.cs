using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

using BlueLedger.PL.BaseClass;
using System.IO;
using System.Text;

namespace BlueLedger.PL.PC.EOP
{
    public partial class EOP : BasePage
    {
        #region Variable
        private readonly Blue.BL.PC.EOP eop = new Blue.BL.PC.EOP();
        private readonly Blue.BL.PC.EOPDt eopDt = new Blue.BL.PC.EOPDt();
        private readonly Blue.BL.IN.Inventory inventory = new Blue.BL.IN.Inventory();
        private DataSet dsEOP = new DataSet();
        private decimal grandTotal = decimal.Parse("0.00");
        private Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();
        private Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private Blue.BL.Option.Inventory.StoreLct store = new Blue.BL.Option.Inventory.StoreLct();

        // Added on: 21/09/2017, By: Fon
        private int eopId
        { get { return int.Parse(Request.Params["ID"]); } }
        // End Added.
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

                if (pop_CommitConfirm.IsCallback)
                {
                    dsEOP = (DataSet)Session["dsEOP"];

                }

            }

        }

        private void Page_Retrieve()
        {
            var EOPId = int.Parse(Request.Params["ID"]);

            var getEOP = eop.Get(dsEOP, EOPId, LoginInfo.ConnStr);

            if (!getEOP)
            {
                // Display Error Message
                return;
            }

            var drEop = dsEOP.Tables[eop.TableName].Rows[0];

            var getEOPDt = eopDt.GetList(dsEOP, EOPId, DateTime.Parse(drEop["StartDate"].ToString()),
                DateTime.Parse(drEop["EndDate"].ToString()), LoginInfo.ConnStr);

            if (!getEOPDt)
            {
                // Display Error Message
                return;
            }

            Session["dsEOP"] = dsEOP;

            Page_Setting();
        }

        private void Page_Setting()
        {
            var drEop = dsEOP.Tables[eop.TableName].Rows[0];

            lbl_Store.Text = drEop["StoreID"] + " : " + drEop["StoreName"];
            lbl_Date.Text = DateTime.Parse(drEop["Date"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            lbl_EndDate.Text = DateTime.Parse(drEop["EndDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            lbl_Desc.Text = drEop["Description"].ToString();
            lbl_Remark.Text = drEop["Remark"].ToString();
            lbl_Status.Text = drEop["Status"].ToString();


            if (drEop["Status"].ToString().ToUpper() == "PRINTED")
            {
                btn_Commit.Visible = false;
            }

            if (drEop["Status"].ToString().ToUpper() == "COMMITTED")
            {
                btn_Import.Visible = false;
                btn_Export.Visible = false;

                btn_Edit.Visible = false;
                btn_Commit.Visible = false;

                btn_UpdateProduct.Visible = false;
            }


            grd_Product.DataSource = dsEOP.Tables[eopDt.TableName];
            grd_Product.DataBind();

            // Display Comment.
            var comment = (PL.UserControls.Comment2)Master.FindControl("Comment");
            comment.Module = "PC";
            comment.SubModule = "EOP";
            comment.RefNo = drEop["EOPId"].ToString();
            comment.Visible = true;
            comment.DataBind();

            // Display Attach
            var attach = (PL.UserControls.Attach2)Master.FindControl("Attach");
            attach.BuCode = Request.Params["BuCode"];
            attach.ModuleName = "PC";
            attach.RefNo = drEop["EOPId"].ToString();
            attach.Visible = true;
            attach.DataBind();

            // Display Log
            var log = (PL.UserControls.Log2)Master.FindControl("Log");
            log.Module = "PC";
            log.SubModule = "EOP";
            log.RefNo = drEop["EOPId"].ToString();
            log.Visible = true;
            log.DataBind();

        }

        protected void grd_Product_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd_Product.PageIndex = e.NewPageIndex;
            grd_Product.DataBind();
        }

        protected void btn_Edit_Click(object sender, EventArgs e)
        {
            Response.Redirect("EOPEdit.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"] + "&MODE=EDIT");
        }

        protected void btn_Print_Click(object sender, EventArgs e)
        {
            Report rpt = new Report();
            rpt.PrintForm(this, "../../RPT/PrintForm.aspx", Request.Params["ID"].ToString(), "EopForm");

        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            // Redirect to list page.
            Response.Redirect("EOPList.aspx");
        }

        protected void grd_Product_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //GridView
            (grd_Product.Columns[4] as BoundField).DataFormatString = DefaultQtyFmt;

            (grd_Product.Columns[5] as BoundField).DataFormatString = DefaultAmtFmt;
            (grd_Product.Columns[6] as BoundField).DataFormatString = DefaultAmtFmt;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_AvgPrice") != null)
                {
                    var lbl_AvgPrice = (Label)e.Row.FindControl("lbl_AvgPrice");
                    lbl_AvgPrice.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "AvgPrice").ToString());
                }

                if (e.Row.FindControl("lbl_TotalAmount") != null)
                {
                    decimal Qty = 0;
                    decimal AvgPrice = 0;

                    if (DataBinder.Eval(e.Row.DataItem, "Qty") != DBNull.Value)
                    {
                        Qty = decimal.Parse(DataBinder.Eval(e.Row.DataItem, "Qty").ToString());
                    }

                    if (DataBinder.Eval(e.Row.DataItem, "AvgPrice") != DBNull.Value)
                    {
                        AvgPrice = decimal.Parse(DataBinder.Eval(e.Row.DataItem, "AvgPrice").ToString());
                    }

                    var lbl_TotalAmount = (Label)e.Row.FindControl("lbl_TotalAmount");
                    lbl_TotalAmount.Text = string.Format(DefaultAmtFmt, RoundAmt(Qty * AvgPrice));

                    grandTotal += RoundAmt(Qty * AvgPrice);
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (e.Row.FindControl("lbl_GrandTotal") != null)
                {
                    var lbl_GrandTotal = (Label)e.Row.FindControl("lbl_GrandTotal");
                    lbl_GrandTotal.Text = string.Format(DefaultAmtFmt, RoundAmt(grandTotal));
                }
            }
        }

        protected void ExportToCSV(string fileName, DataSet ds)
        {
            string csvText;
            string storeId, productCode, productDesc1, productDesc2, inventoryUnit, qty;

            // Add the first line
            csvText = "StoreId,ProductCode,ProductDesc1,ProductDesc2,InventoryUnit,Qty" + System.Environment.NewLine;

            // Add Contents
            for (int i = 0; i < ds.Tables["eop"].Rows.Count; i++)
            {
                storeId = ds.Tables["eop"].Rows[i]["StoreId"].ToString();
                productCode = ds.Tables["eop"].Rows[i]["ProductCode"].ToString();
                productDesc1 = ds.Tables["eop"].Rows[i]["ProductDesc1"].ToString();
                productDesc1 = productDesc1.Replace(",", " ");
                productDesc2 = ds.Tables["eop"].Rows[i]["ProductDesc2"].ToString();
                productDesc2 = productDesc2.Replace(",", " ");
                inventoryUnit = ds.Tables["eop"].Rows[i]["InventoryUnit"].ToString();
                qty = ds.Tables["eop"].Rows[i]["Qty"].ToString();

                csvText = csvText + storeId + "," + productCode + "," + productDesc1.ToString() + "," + productDesc2 + "," + inventoryUnit + "," + qty + System.Environment.NewLine;
            }

            // Write to file

            Response.Clear();
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.csv", fileName));
            Response.ContentType = "text/csv";

            // Output with CharSet
            //Response.Charset = "ISO-8859-11";
            //Response.Charset = "TIS620";
            //Response.Write(csvText);
            //byte[] buffer = System.Text.Encoding.Default.GetBytes(csvText);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(csvText);
            Response.BinaryWrite(buffer);
            Response.Flush();
            Response.End();

        }

        protected void btn_UpdateProduct_Click(object sender, EventArgs e)
        {
            string connString = LoginInfo.ConnStr;


            SqlConnection conn = new SqlConnection(connString);

            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "[IN].UpdateProductToEop";
            cmd.Parameters.Add("@EopId", SqlDbType.Int).Value = Convert.ToInt32(Request.Params["ID"].ToString());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;

            conn.Open();

            reader = cmd.ExecuteReader();
            // Data is accessible through the DataReader object here.

            reader.Read();
            Label_Information.Text = reader["RecordCount"].ToString() + " product(s) have been added.";
            pop_Information.ShowOnPageLoad = true;

            conn.Close();
        }

        protected void btn_Export_Click(object sender, EventArgs e)
        {
            //string connString = LoginInfo.ConnStr;

            //SqlConnection conn = new SqlConnection(connString);
            //conn.Open();

            //string sqlstring = "SELECT h.StoreId ,h.EndDate, d.ProductCode, d.ProductDesc1, d.ProductDesc2, d.InventoryUnit, d.Qty";
            //sqlstring = sqlstring + " FROM [IN].Eop h";
            //sqlstring = sqlstring + " LEFT JOIN [IN].EopDt d ON h.EopId = d.EopId";
            //sqlstring = sqlstring + " WHERE h.EopId = " + Request.Params["ID"];
            //sqlstring = sqlstring + " ORDER BY d.EopId";


            //SqlCommand cmd = new SqlCommand(sqlstring, conn);

            //try
            //{
            //    SqlDataAdapter da = new SqlDataAdapter(cmd);

            //    DataSet ds = new DataSet();
            //    da.Fill(ds, "eop");

            //    string fileName = "EOP_" + ds.Tables["eop"].Rows[0]["StoreId"].ToString() + "_" + ds.Tables["eop"].Rows[0]["EndDate"].ToString();
            //    ExportToCSV(fileName, ds);

            //}
            //catch (Exception ex)
            //{
            //}

            string sql = string.Format("EXEC [IN].EopExport {0}", Request.Params["ID"].ToString());

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = eop.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            if (dt.Rows.Count > 0)
            {

                ds.Tables.Add(dt);
                ds.Tables[0].TableName = "eop";
                string fileName = "EOP_" + dt.Rows[0]["StoreId"].ToString() + "_" + dt.Rows[0]["EndDate"].ToString();
                ExportToCSV(fileName, ds);
            }
            else
            {
                AlertMessageBox("No data");
            }

        }
        protected void btn_Import_Click(object sender, EventArgs e)
        {
            string connString = LoginInfo.ConnStr;

            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string sqlstring = "SELECT TOP 1 h.StoreId, s.LocationName, CONVERT(char(10), h.EndDate,126) as EndDate";
            sqlstring = sqlstring + " FROM [IN].Eop h";
            sqlstring = sqlstring + " LEFT JOIN [IN].StoreLocation s ON h.StoreId = s.LocationCode";
            sqlstring = sqlstring + " WHERE h.EopId = " + Request.Params["ID"];

            SqlCommand cmd = new SqlCommand(sqlstring, conn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            da.Fill(ds, "eop");

            string storeId = ds.Tables["eop"].Rows[0]["StoreId"].ToString();
            string storeName = ds.Tables["eop"].Rows[0]["LocationName"].ToString();
            string atDate = ds.Tables["eop"].Rows[0]["EndDate"].ToString();


            string queryString = "ImportFile.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"];
            queryString = queryString + "&StoreId=" + storeId + "&StoreName=" + storeName + "&AtDate=" + atDate;

            //Session["PreviousURL"] = Request.Url;
            Response.Redirect(queryString);



        }

        protected void btn_Commit_Click(object sender, EventArgs e)
        {
            int eopID = Convert.ToInt32(Request.Params["ID"].Trim().ToString());

            // Check Qty is empty (null)
            DataTable dt = eop.DbExecuteQuery(string.Format("SELECT COUNT(EopDtId) as RecordCount FROM [IN].EopDt WHERE EopId={0} AND Qty IS NULL", eopID.ToString()), null, LoginInfo.ConnStr);
            int recordCount = Convert.ToInt32(dt.Rows[0][0]);

            if (recordCount > 0)
            {
                Label_Information.Text = string.Format("There are {0:D} product(s) are not assigned quantity.", recordCount);
                pop_Information.ShowOnPageLoad = true;
            }
            else
                pop_CommitConfirm.ShowOnPageLoad = true;
        }

        protected void btn_CommitConfirm_OK_Click(object sender, EventArgs e)
        {
            pop_CommitConfirm.ShowOnPageLoad = false;
            pop_Processing.ShowOnPageLoad = false;

            string returnCommit = Commit();
            if (returnCommit != string.Empty)
            {
                Label_Information.Text = returnCommit;
                pop_Information.ShowOnPageLoad = true;
            }

            // Check Qty [IN] > 0 ; Physical count qty is more than on-hand
            DataTable dt = eop.DbExecuteQuery(string.Format("SELECT COUNT(DtNo) FROM [IN].Inventory WHERE [IN] > 0 AND HdrNo='{0}' AND [Type]='EOP'", eopId.ToString()), null, LoginInfo.ConnStr);
            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                pop_ConfirmAdjustIn.ShowOnPageLoad = true;

            }
            else
            {
                ClassLogTool pctool = new ClassLogTool();
                pctool.SaveActionLog("EOP", eopId.ToString(), "Commit");
                Response.Redirect("EOP.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"]);
            }
        }


        private string Commit()
        {
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

            //int eopID = Convert.ToInt32(Request.Params["ID"].Trim().ToString());
            //int eopDtID = 0;
            //string locationCode;
            //string productCode;
            //DateTime startDate;
            //DateTime endDate;
            //Decimal closingQty;
            //DateTime committedDate;

            //SqlConnection con = new SqlConnection();
            //con.ConnectionString = LoginInfo.ConnStr;


            //SqlCommand cmdDelete = new SqlCommand();
            //cmdDelete.Connection = con;
            //cmdDelete.CommandText = string.Format("DELETE FROM [IN].Inventory WHERE HdrNo = '{0}'", eopID.ToString());

            //string sqlText = "SELECT h.EopID, d.EopDtID, h.StoreId, h.StartDate, h.EndDate, d.ProductCode, d.Qty";
            //sqlText = sqlText + " FROM [IN].Eop h";
            //sqlText = sqlText + " LEFT JOIN [IN].EopDt d ON d.EopId = h.EopId";
            //sqlText = sqlText + " WHERE h.EopId = " + eopID.ToString();

            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = con;
            //cmd.CommandText = sqlText;

            //con.Open();

            //// Delete old EOP
            //cmdDelete.ExecuteNonQuery();

            //// Insert Consumption of EOP
            //SqlDataReader reader = cmd.ExecuteReader();

            //while (reader.Read())
            //{
            //    eopDtID = Convert.ToInt32(reader["EOPDtId"].ToString());
            //    locationCode = reader["StoreId"].ToString();
            //    productCode = reader["ProductCode"].ToString();
            //    startDate = DateTime.Parse(reader["StartDate"].ToString());
            //    endDate = DateTime.Parse(reader["EndDate"].ToString());

            //    // Modified on: 21/09/2017, By: Fon
            //    //closingQty = decimal.Parse(reader["Qty"].ToString());

            //    if (!decimal.TryParse(reader["Qty"].ToString(), out closingQty))
            //    {
            //        Label_Information.Text = string.Format("Quantity");
            //        return "Balance Qty. is Required.";
            //    }
            //    // End Modified.
            //    committedDate = DateTime.Parse(reader["EndDate"].ToString());
            //    inventory.InsertConsumption(eopID, eopDtID, locationCode, productCode, startDate, endDate, closingQty, committedDate, LoginInfo.ConnStr);


            //} //while
            //reader.Close();
            //con.Close();

            //var drEOP = dsEOP.Tables[eop.TableName].Rows[0];

            //try
            //{
            //    con.Open();

            //    string sqlUpdate = "EXEC [IN].UpdateCostOfEopIn @EopId";
            //    SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, con);
            //    cmdUpdate.Parameters.AddWithValue("@EopId", eopID.ToString());
            //    cmdUpdate.CommandTimeout = 600;
            //    cmdUpdate.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    return ex.Message;
            //}
            //finally
            //{ con.Close(); }
            //UpdateEopStatus(eopID, "Committed");
            return string.Empty;
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


        protected void btn_CommitConfirm_Cancel_Click(object sender, EventArgs e)
        {
            pop_CommitConfirm.ShowOnPageLoad = false;
            pop_UpdateSuccess.ShowOnPageLoad = false;
            pop_Information.ShowOnPageLoad = false;

            // Redirect to detail page.
            // Response.Redirect("EOP.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"]);
        }
        protected void btn_UpdateSuccess_OK_Click(object sender, EventArgs e)
        {
            pop_CommitConfirm.ShowOnPageLoad = false;
            pop_UpdateSuccess.ShowOnPageLoad = false;
            pop_Information.ShowOnPageLoad = false;

            // Redirect to detail page.
            Response.Redirect("EOP.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"]);
        }

        protected void btn_Information_OK_Click(object sender, EventArgs e)
        {
            pop_CommitConfirm.ShowOnPageLoad = false;
            pop_UpdateSuccess.ShowOnPageLoad = false;
            pop_Information.ShowOnPageLoad = false;
            Label_Information.Text = "";

            // Redirect to detail page.
            Response.Redirect("EOP.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"]);
        }

        protected void btn_ConfirmAdjustIn_Yes_Click(object sender, EventArgs e)
        {
            pop_ConfirmAdjustIn.ShowOnPageLoad = false;

            ClassLogTool pctool = new ClassLogTool();
            pctool.SaveActionLog("EOP", eopId.ToString(), "Commit");
            Response.Redirect("EOP.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"]);

        }

        protected void btn_ConfirmAdjustIn_No_Click(object sender, EventArgs e)
        {
            string eopID = Request.Params["ID"].Trim().ToString();

            // Remove existing EOP
            eop.DbExecuteQuery(string.Format("DELETE FROM [IN].Inventory WHERE HdrNo = '{0}' AND [Type]='EOP'", eopID), null, LoginInfo.ConnStr);
            UpdateEopStatus(Convert.ToInt32(eopID), "In Process");

            pop_ConfirmAdjustIn.ShowOnPageLoad = false;
        }
    }
}