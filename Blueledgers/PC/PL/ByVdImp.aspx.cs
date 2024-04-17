using BlueLedger.PL.BaseClass;
using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace BlueLedger.PL.PC.PL
{
    public partial class ByVdImp : BasePage
    {
        private readonly DataSet dsProdUnit = new DataSet();
        private readonly Blue.BL.PC.PL.PL pl = new Blue.BL.PC.PL.PL();
        private readonly Blue.BL.PC.PL.PLDt plDt = new Blue.BL.PC.PL.PLDt();
        private readonly Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private readonly Blue.BL.APP.Config con = new Blue.BL.APP.Config();
        private readonly Blue.BL.Ref.Currency curr = new Blue.BL.Ref.Currency();

        private DataTable CSVTable = new DataTable();
        private DataSet dsPLImp = new DataSet();
        private DataTable dt; //Table เก็บข้อมูล Excel  
        private DataTable dtProdUnit = new DataTable();
        private OleDbConnection exConn; //ติดต่อ Excel
        private Blue.BL.GnxLib gnxLib = new Blue.BL.GnxLib();
        private Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        //private SqlConnection sqConn; //ติดต่อ SQL Server
        private string strConn = "";
        private Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();

        private static string defaultCurrency = string.Empty;



        protected void Page_Init(object sender, EventArgs e)
        {
            hf_ConnStr.Value = LoginInfo.ConnStr;
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsPLImp = (DataSet)Session["dsPLImp"];
            }

            defaultCurrency = con.GetValue("APP", "BU", "DefaultCurrency", LoginInfo.ConnStr);

        }

        private void Page_Retrieve()
        {
            var getPLSchema = pl.GetSchame(dsPLImp, LoginInfo.ConnStr);

            if (!getPLSchema)
            {
                return;
            }

            var getPLdtSchema = plDt.GetSchame(dsPLImp, LoginInfo.ConnStr);

            if (!getPLdtSchema)
            {
                return;
            }

            // Add new row to PL DataTable
            var drPL = dsPLImp.Tables[pl.TableName].NewRow();

            drPL["PriceLstNo"] = pl.GetNewID(LoginInfo.ConnStr);
            drPL["VendorCode"] = string.Empty;
            drPL["DateFrom"] = ServerDateTime;
            drPL["DateTo"] = ServerDateTime.AddDays(30); // Fixed 30 Days : Get from parameter next relase
            drPL["CreatedDate"] = ServerDateTime;
            drPL["CreatedBy"] = LoginInfo.LoginName;
            drPL["UpdatedDate"] = ServerDateTime;
            drPL["UpdatedBy"] = LoginInfo.LoginName;

            dsPLImp.Tables[pl.TableName].Rows.Add(drPL);

            Session["dsPLImp"] = dsPLImp;

            Page_Setting();
        }

        private void Page_Setting()
        {
            var drPL = dsPLImp.Tables[pl.TableName].Rows[0];

            ddl_Vendor.Value = drPL["VendorCode"].ToString();
            //txt_Vendor.Text     = vendor.GetName(drPL["VendorCode"].ToString(), LoginInfo.ConnStr);
            txt_DateFrom.Date = DateTime.Parse(drPL["DateFrom"].ToString());
            txt_DateTo.Date = DateTime.Parse(drPL["DateTo"].ToString());
            txt_RefNo.Text = drPL["RefNo"].ToString();

            // Added on: 28/01/2018, By: Fon
            ddl_CurrCode.Value = defaultCurrency;
            // End Added.

            //grd_PLDt.DataSource = dsPLEdit.Tables[plDt.TableName];
            //grd_PLDt.DataBind();
        }

        private string GetInventoryUnit(DataTable dt, string productcode)
        {
            var row = dt.AsEnumerable().Where(x => x.Field<string>("ProductCode") == productcode).FirstOrDefault();
            if (row != null)
                return row["InventoryUnit"].ToString();
            else
                return string.Empty;
        }

        private void Save()
        {
            if (ddl_Vendor.Value == null)
            {
                pop_VendorCode.ShowOnPageLoad = true;
                return;
            }

            if (txt_Vrank.Text == string.Empty)
            {
                pop_Vendor.ShowOnPageLoad = true;
                return;
            }

            var dtProduct = product.DbExecuteQuery("SELECT ProductCode, InventoryUnit FROM [IN].Product", null, LoginInfo.ConnStr);


            //Check product no unit
            for (var i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                var ProductCode = GridView1.Rows[i].Cells[0].Text.Trim();
                var OrderUnit = GridView1.Rows[i].Cells[3].Text.Trim();

                if (prodUnit.CountByProdUnitCode(ProductCode, OrderUnit, LoginInfo.ConnStr) > 0)
                {
                    continue;
                }
                else
                {
                    //Insert Unit on ProdUnit
                    dtProdUnit.Columns.Add("ProductCode");
                    dtProdUnit.Columns.Add("DescriptionEn");
                    dtProdUnit.Columns.Add("DescriptionTH");
                    dtProdUnit.Columns.Add("InventoryUnit");
                    dtProdUnit.Columns.Add("OrderUnit");
                    dtProdUnit.Columns.Add("Rate");

                    for (var j = 0; j <= GridView1.Rows.Count - 1; j++)
                    {
                        var ProductCodeInsertUnit = GridView1.Rows[j].Cells[0].Text.Trim();
                        var DescEnInsertUnit = GridView1.Rows[j].Cells[1].Text.Trim();
                        var DescTHInsertUnit = GridView1.Rows[j].Cells[2].Text.Trim();
                        //var InventoryUnitInsertUnit = prodUnit.GetInvenUnit(ProductCodeInsertUnit, LoginInfo.ConnStr);
                        var InventoryUnitInsertUnit = GetInventoryUnit(dtProduct, ProductCodeInsertUnit);
                        var OrderUnitInsertUnit = GridView1.Rows[j].Cells[3].Text.Trim();

                        if (prodUnit.CountByProdUnitCode(ProductCodeInsertUnit, OrderUnitInsertUnit, LoginInfo.ConnStr) == 0)
                        {
                            DataRow drProdUnit;
                            drProdUnit = dtProdUnit.NewRow();

                            drProdUnit["ProductCode"] = ProductCodeInsertUnit;
                            drProdUnit["DescriptionEn"] = DescEnInsertUnit;
                            drProdUnit["DescriptionTH"] = DescTHInsertUnit;
                            drProdUnit["InventoryUnit"] = InventoryUnitInsertUnit;
                            drProdUnit["OrderUnit"] = OrderUnitInsertUnit;

                            dtProdUnit.Rows.Add(drProdUnit);
                        }
                    }

                    grd_UnitProd.DataSource = dtProdUnit;
                    grd_UnitProd.DataBind();

                    Session["dtProdUnit"] = dtProdUnit;

                    pop_UnitInsert.ShowOnPageLoad = true;
                    return;
                }
            }

            //pl.GetSchame(dsPLImp, LoginInfo.ConnStr);
            var drPL = dsPLImp.Tables[pl.TableName].Rows[0];

            drPL["PriceLstNo"] = pl.GetNewID(LoginInfo.ConnStr);

            // Update Detail Information
            foreach (DataRow drPLDt in dsPLImp.Tables[plDt.TableName].Rows)
            {
                if (drPLDt.RowState != DataRowState.Deleted)
                {
                    drPLDt["PriceLstNo"] = drPL["PriceLstNo"].ToString();
                }
            }

            drPL["RefNo"] = txt_RefNo.Text.Trim();
            drPL["DateFrom"] = DateTime.Parse(txt_DateFrom.Date.ToString("dd/MM/yyyy"));

            if (txt_DateTo.Text != string.Empty)
            {
                drPL["DateTo"] = DateTime.Parse(txt_DateTo.Date.ToString("dd/MM/yyyy"));
            }
            else
            {
                drPL["DateTo"] = DBNull.Value;
            }

            drPL["VendorCode"] = ddl_Vendor.Value.ToString();
            drPL["UpdatedDate"] = ServerDateTime;
            drPL["UpdatedBy"] = LoginInfo.LoginName;

            for (var i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                var drNew = dsPLImp.Tables[plDt.TableName].NewRow();

                var ProductCode = GridView1.Rows[i].Cells[0].Text.Trim();
                var OrderUnit = GridView1.Rows[i].Cells[3].Text.Trim();

                if (GridView1.Rows[i].Cells[6].Text.Trim() != "&nbsp;") // Check Price is empty
                {
                    if (GridView1.Rows[i].Cells[6].Text.Trim() != string.Empty)
                    {
                        if (decimal.Parse(GridView1.Rows[i].Cells[6].Text.Trim()) > 0) // check price > 0
                        {
                            if (GridView1.Rows[i].Cells[10].Text.Trim() == "&nbsp;")
                                GridView1.Rows[i].Cells[10].Text = "N";
                            if (GridView1.Rows[i].Cells[11].Text.Trim() == "&nbsp;")
                                GridView1.Rows[i].Cells[11].Text = "0";

                            if ((GridView1.Rows[i].Cells[10].Text.Trim() == "A" &&
                                 decimal.Parse(GridView1.Rows[i].Cells[11].Text.Trim()) > 0)
                                ||
                                (GridView1.Rows[i].Cells[10].Text.Trim() == "I" &&
                                 decimal.Parse(GridView1.Rows[i].Cells[11].Text.Trim()) > 0)
                                || (GridView1.Rows[i].Cells[10].Text.Trim() == "N"))
                            {
                                drNew["PriceLstNo"] = dsPLImp.Tables[pl.TableName].Rows[0]["PriceLstNo"].ToString();

                                if (dsPLImp.Tables[plDt.TableName].Rows.Count == 0)
                                {
                                    drNew["SeqNo"] = 1;
                                }
                                else
                                {
                                    var RowCount = dsPLImp.Tables[plDt.TableName].Rows.Count;
                                    var LatestSeqNo =
                                        int.Parse(dsPLImp.Tables[plDt.TableName].Rows[RowCount - 1]["SeqNo"].ToString());
                                    drNew["SeqNo"] = LatestSeqNo + 1;
                                }

                                drNew["ProductCode"] = GridView1.Rows[i].Cells[0].Text.Trim();
                                drNew["OrderUnit"] = GridView1.Rows[i].Cells[3].Text.Trim();

                                if (txt_Vrank.Text != string.Empty)
                                {
                                    drNew["VendorRank"] = txt_Vrank.Text;
                                }
                                else
                                {
                                    pop_Vendor.ShowOnPageLoad = true;
                                    return;
                                }

                                if (GridView1.Rows[i].Cells[4].Text.Trim() == "&nbsp;" ||
                                    GridView1.Rows[i].Cells[4].Text.Trim() == string.Empty)
                                {
                                    drNew["QtyFrom"] = 0;
                                }
                                else
                                {
                                    drNew["QtyFrom"] = GridView1.Rows[i].Cells[4].Text.Trim();
                                }

                                if (GridView1.Rows[i].Cells[5].Text.Trim() == "&nbsp;" ||
                                    GridView1.Rows[i].Cells[5].Text.Trim() == string.Empty)
                                {
                                    drNew["QtyTo"] = 0;
                                }
                                else
                                {
                                    drNew["QtyTo"] = GridView1.Rows[i].Cells[5].Text.Trim();
                                }

                                if (GridView1.Rows[i].Cells[6].Text.Trim() == "&nbsp;" ||
                                    GridView1.Rows[i].Cells[6].Text.Trim() == string.Empty)
                                {
                                    drNew["QuotedPrice"] = 0;
                                }
                                else
                                {
                                    drNew["QuotedPrice"] = GridView1.Rows[i].Cells[6].Text.Trim();
                                }

                                drNew["MarketPrice"] = 0.00;

                                if (GridView1.Rows[i].Cells[7].Text.Trim() == "&nbsp;" ||
                                    GridView1.Rows[i].Cells[7].Text.Trim() == string.Empty)
                                {
                                    drNew["FOC"] = 0;
                                }
                                else
                                {
                                    drNew["FOC"] = GridView1.Rows[i].Cells[7].Text.Trim();
                                }

                                drNew["Comment"] = (GridView1.Rows[i].Cells[13].Text.Trim() == "&nbsp;"
                                    ? string.Empty
                                    : GridView1.Rows[i].Cells[13].Text.Trim());

                                if (GridView1.Rows[i].Cells[8].Text.Trim() == "&nbsp;" ||
                                    GridView1.Rows[i].Cells[8].Text.Trim() == string.Empty)
                                {
                                    drNew["DiscPercent"] = 0;
                                }
                                else
                                {
                                    drNew["DiscPercent"] = GridView1.Rows[i].Cells[8].Text.Trim();
                                }

                                if (GridView1.Rows[i].Cells[9].Text.Trim() == "&nbsp;" ||
                                    GridView1.Rows[i].Cells[9].Text.Trim() == string.Empty)
                                {
                                    drNew["DiscAmt"] = 0;
                                }
                                else
                                {
                                    drNew["DiscAmt"] = GridView1.Rows[i].Cells[9].Text.Trim();
                                }

                                if (GridView1.Rows[i].Cells[11].Text.Trim() == "&nbsp;" ||
                                    GridView1.Rows[i].Cells[11].Text.Trim() == string.Empty)
                                {
                                    drNew["TaxRate"] = 0;
                                }
                                else
                                {
                                    drNew["TaxRate"] = GridView1.Rows[i].Cells[11].Text.Trim();
                                }

                                if (GridView1.Rows[i].Cells[10].Text.Trim() != null)
                                {
                                    drNew["Tax"] = GridView1.Rows[i].Cells[10].Text.Trim();
                                    decimal DiscAmt = 0;
                                    decimal TaxRate = 0;
                                    decimal Price = 0;

                                    TaxRate = Convert.ToDecimal(drNew["TaxRate"].ToString());
                                    DiscAmt = Convert.ToDecimal(drNew["DiscAmt"].ToString());
                                    Price = Convert.ToDecimal(drNew["QuotedPrice"].ToString());

                                    switch (drNew["Tax"].ToString().ToUpper())
                                    {
                                        case "N":
                                            drNew["NetAmt"] = (Price - DiscAmt).ToString();
                                            break;
                                        case "A":
                                            //drNew["NetAmt"] = ((Price - DiscAmt) + ((Price * TaxRate) / 100)).ToString();
                                            drNew["NetAmt"] = (Price - DiscAmt).ToString();
                                            break;

                                        case "I":
                                            //drNew["NetAmt"] = ((Price - DiscAmt) - ((Price * TaxRate) / (100 + TaxRate))).ToString();
                                            drNew["NetAmt"] = NetAmt(drNew["Tax"].ToString(), TaxRate,
                                                Price, DiscAmt, 1); // price list จะคิดต่อ 1 ชิ้น
                                            break;

                                    }
                                }

                                drNew["VendorProdCode"] = (GridView1.Rows[i].Cells[12].Text.Trim() == "&nbsp;"
                                    ? string.Empty
                                    : GridView1.Rows[i].Cells[12].Text.Trim());
                                drNew["AvgPrice"] = 0.00;
                                drNew["LastPrice"] = 0.00;

                                // Added on: 26/01/2018, By: Fon
                                drNew["CurrencyCode"] = ddl_CurrCode.Value;
                                // End Added.

                                dsPLImp.Tables[plDt.TableName].Rows.Add(drNew);
                            }
                        }
                    }
                }

                Session["dsPLImp"] = dsPLImp;
            }

            if (txt_DateFrom.Text != string.Empty && txt_DateTo.Text != string.Empty)
            {
                if (pl.CountByVendorDateFromTo(drPL["VendorCode"].ToString(),
                    DateTime.Parse(drPL["DateFrom"].ToString())
                    , DateTime.Parse(drPL["DateTo"].ToString()), LoginInfo.ConnStr) > 0)
                {
                    pop_ConfirmSave.ShowOnPageLoad = true;
                }
                else
                {
                    var result = pl.Save(dsPLImp, LoginInfo.ConnStr);

                    if (result)
                    {
                        pop_ImportSuccess.ShowOnPageLoad = true;
                    }
                }
            }
            else if (txt_DateFrom.Text != string.Empty && txt_DateTo.Text == string.Empty)
            {
                if (
                    pl.CountByVendorDateFrom(drPL["VendorCode"].ToString(), DateTime.Parse(drPL["DateFrom"].ToString()),
                        LoginInfo.ConnStr) > 0)
                {
                    pop_ConfirmSave.ShowOnPageLoad = true;
                }
                else
                {
                    var result = pl.Save(dsPLImp, LoginInfo.ConnStr);

                    if (result)
                    {
                        pop_ImportSuccess.ShowOnPageLoad = true;
                    }
                }
            }
            else if (txt_DateFrom.Text == string.Empty && txt_DateTo.Text != string.Empty)
            {
                if (
                    pl.CountByVendorDateTo(drPL["VendorCode"].ToString(), DateTime.Parse(drPL["DateTo"].ToString()),
                        LoginInfo.ConnStr) > 0)
                {
                    pop_ConfirmSave.ShowOnPageLoad = true;
                }
                else
                {
                    var result = pl.Save(dsPLImp, LoginInfo.ConnStr);

                    if (result)
                    {
                        pop_ImportSuccess.ShowOnPageLoad = true;
                    }
                }
            }
            else if (txt_DateFrom.Text == string.Empty && txt_DateTo.Text == string.Empty)
            {
                if (pl.CountByVendor(drPL["VendorCode"].ToString(), LoginInfo.ConnStr) > 0)
                {
                    pop_ConfirmSave.ShowOnPageLoad = true;
                }
                else
                {
                    var result = pl.Save(dsPLImp, LoginInfo.ConnStr);

                    if (result)
                    {
                        pop_ImportSuccess.ShowOnPageLoad = true;
                    }
                }
            }
        }

        protected void btn_ConfirmSave_Click(object sender, EventArgs e)
        {
            dsPLImp.Clear();

            var dtSave = new DataTable();

            if (txt_DateFrom.Text != string.Empty && txt_DateTo.Text != string.Empty)
            {
                var getVenderFromTo = pl.GetList(dsPLImp, ddl_Vendor.Value.ToString(),
                    DateTime.Parse(txt_DateFrom.Date.ToString()), DateTime.Parse(txt_DateTo.Date.ToString()),
                    LoginInfo.ConnStr);

                dtSave = pl.GetListByVendorDateFromTo(ddl_Vendor.Value.ToString(),
                    DateTime.Parse(txt_DateFrom.Date.ToString()), DateTime.Parse(txt_DateTo.Date.ToString()),
                    LoginInfo.ConnStr);

                Session["dsPLImp"] = dsPLImp;
            }
            else if (txt_DateFrom.Text != string.Empty && txt_DateTo.Text == string.Empty)
            {
                dtSave = pl.GetList(ddl_Vendor.Value.ToString(), DateTime.Parse(txt_DateFrom.Date.ToString()),
                    LoginInfo.ConnStr);

                var getVendorFrom = pl.GetList(dsPLImp, ddl_Vendor.Value.ToString(),
                    DateTime.Parse(txt_DateFrom.Date.ToString()), LoginInfo.ConnStr);

                Session["dsPLImp"] = dsPLImp;
            }
            else if (txt_DateFrom.Text == string.Empty && txt_DateTo.Text != string.Empty)
            {
                dtSave = pl.GetListByVendorDateTo(ddl_Vendor.Value.ToString(),
                    DateTime.Parse(txt_DateTo.Date.ToString()), LoginInfo.ConnStr);

                var getVendorTo = pl.GetListVendorDateTo(dsPLImp, ddl_Vendor.Value.ToString(),
                    DateTime.Parse(txt_DateTo.Date.ToString()), LoginInfo.ConnStr);

                Session["dsPLImp"] = dsPLImp;
            }
            else if (txt_DateFrom.Text == string.Empty && txt_DateTo.Text == string.Empty)
            {
                dtSave = pl.GetList(ddl_Vendor.Value.ToString(), LoginInfo.ConnStr);

                var getVendor = pl.GetList(dsPLImp, ddl_Vendor.Value.ToString(), LoginInfo.ConnStr);

                Session["dsPLImp"] = dsPLImp;
            }

            var drSave = dsPLImp.Tables[pl.TableName].Rows[0];
            foreach (DataRow dr in dtSave.Rows)
            {
                drSave["PriceLstNo"] = dr["PriceLstNo"];
                drSave["UpdatedDate"] = ServerDateTime;
                drSave["UpdatedBy"] = LoginInfo.LoginName;
            }

            var getPLDt = plDt.GetList(dsPLImp, drSave["PriceLstNo"].ToString(), LoginInfo.ConnStr);

            if (!getPLDt)
            {
                return;
            }

            // Update Detail Information
            foreach (DataRow drPLDt in dsPLImp.Tables[plDt.TableName].Rows)
            {
                if (drPLDt.RowState != DataRowState.Deleted)
                {
                    drPLDt["PriceLstNo"] = drSave["PriceLstNo"];
                }
            }

            for (var i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                var dt = plDt.GetList(dsPLImp, drSave["PriceLstNo"].ToString(), GridView1.Rows[i].Cells[0].Text,
                    LoginInfo.ConnStr);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow drUpdating in dsPLImp.Tables[plDt.TableName].Rows)
                    {
                        if (drUpdating["ProductCode"].ToString() == GridView1.Rows[i].Cells[0].Text)
                        {
                            if (GridView1.Rows[i].Cells[6].Text.Trim() != "&nbsp;") // Check Price is empty
                            {
                                if (GridView1.Rows[i].Cells[6].Text.Trim() != string.Empty)
                                {
                                    if (decimal.Parse(GridView1.Rows[i].Cells[6].Text.Trim()) > 0) // check price > 0
                                    {
                                        if ((GridView1.Rows[i].Cells[10].Text.Trim() == "A" &&
                                             decimal.Parse(GridView1.Rows[i].Cells[11].Text.Trim()) > 0)
                                            ||
                                            (GridView1.Rows[i].Cells[10].Text.Trim() == "I" &&
                                             decimal.Parse(GridView1.Rows[i].Cells[11].Text.Trim()) > 0)
                                            || (GridView1.Rows[i].Cells[10].Text.Trim() == "N"))
                                        {
                                            drUpdating["OrderUnit"] = GridView1.Rows[i].Cells[3].Text.Trim();
                                            drUpdating["VendorRank"] = txt_Vrank.Text;

                                            if (GridView1.Rows[i].Cells[4].Text.Trim() == "&nbsp;" ||
                                                GridView1.Rows[i].Cells[4].Text.Trim() == string.Empty)
                                            {
                                                drUpdating["QtyFrom"] = 0;
                                            }
                                            else
                                            {
                                                drUpdating["QtyFrom"] = GridView1.Rows[i].Cells[4].Text.Trim();
                                            }

                                            if (GridView1.Rows[i].Cells[5].Text.Trim() == "&nbsp;" ||
                                                GridView1.Rows[i].Cells[5].Text.Trim() == string.Empty)
                                            {
                                                drUpdating["QtyTo"] = 0;
                                            }
                                            else
                                            {
                                                drUpdating["QtyTo"] = GridView1.Rows[i].Cells[5].Text.Trim();
                                            }

                                            if (GridView1.Rows[i].Cells[6].Text.Trim() == "&nbsp;" ||
                                                GridView1.Rows[i].Cells[6].Text.Trim() == string.Empty)
                                            {
                                                drUpdating["QuotedPrice"] = 0;
                                            }
                                            else
                                            {
                                                drUpdating["QuotedPrice"] = GridView1.Rows[i].Cells[6].Text.Trim();
                                            }

                                            drUpdating["MarketPrice"] = 0.00;

                                            if (GridView1.Rows[i].Cells[7].Text.Trim() == "&nbsp;" ||
                                                GridView1.Rows[i].Cells[7].Text.Trim() == string.Empty)
                                            {
                                                drUpdating["FOC"] = 0;
                                            }
                                            else
                                            {
                                                drUpdating["FOC"] = GridView1.Rows[i].Cells[7].Text.Trim();
                                            }

                                            drUpdating["Comment"] = (GridView1.Rows[i].Cells[13].Text.Trim() == "&nbsp;"
                                                ? string.Empty
                                                : GridView1.Rows[i].Cells[13].Text.Trim());

                                            if (GridView1.Rows[i].Cells[8].Text.Trim() == "&nbsp;" ||
                                                GridView1.Rows[i].Cells[8].Text.Trim() == string.Empty)
                                            {
                                                drUpdating["DiscPercent"] = 0;
                                            }
                                            else
                                            {
                                                drUpdating["DiscPercent"] = GridView1.Rows[i].Cells[8].Text.Trim();
                                            }

                                            if (GridView1.Rows[i].Cells[9].Text.Trim() == "&nbsp;" ||
                                                GridView1.Rows[i].Cells[9].Text.Trim() == string.Empty)
                                            {
                                                drUpdating["DiscAmt"] = 0;
                                            }
                                            else
                                            {
                                                drUpdating["DiscAmt"] = GridView1.Rows[i].Cells[9].Text.Trim();
                                            }

                                            if (GridView1.Rows[i].Cells[11].Text.Trim() == "&nbsp;" ||
                                                GridView1.Rows[i].Cells[11].Text.Trim() == string.Empty)
                                            {
                                                drUpdating["TaxRate"] = 0;
                                            }
                                            else
                                            {
                                                drUpdating["TaxRate"] = GridView1.Rows[i].Cells[11].Text.Trim();
                                            }

                                            if (GridView1.Rows[i].Cells[10].Text.Trim() != null)
                                            {
                                                drUpdating["Tax"] = GridView1.Rows[i].Cells[10].Text.Trim();
                                                decimal DiscAmt = 0;
                                                decimal TaxRate = 0;
                                                decimal Price = 0;

                                                TaxRate = Convert.ToDecimal(drUpdating["TaxRate"].ToString());
                                                DiscAmt = Convert.ToDecimal(drUpdating["DiscAmt"].ToString());
                                                Price = Convert.ToDecimal(drUpdating["QuotedPrice"].ToString());

                                                switch (drUpdating["Tax"].ToString().ToUpper())
                                                {
                                                    case "N":
                                                        drUpdating["NetAmt"] = (Price - DiscAmt).ToString();
                                                        break;

                                                    case "A":
                                                        //drUpdating["NetAmt"] = ((Price - DiscAmt) + ((Price * TaxRate) / 100)).ToString();
                                                        drUpdating["NetAmt"] = (Price - DiscAmt).ToString();
                                                        break;

                                                    case "I":
                                                        //drUpdating["NetAmt"] = ((Price - DiscAmt) - ((Price * TaxRate) / (100 + TaxRate))).ToString();
                                                        drUpdating["NetAmt"] =
                                                            NetAmt(drUpdating["Tax"].ToString(), TaxRate,
                                                                Price, DiscAmt, 1); // price list จะคิดต่อ 1 ชิ้น
                                                        break;
                                                }
                                            }

                                            drUpdating["VendorProdCode"] = (GridView1.Rows[i].Cells[12].Text.Trim() ==
                                                                            "&nbsp;"
                                                ? string.Empty
                                                : GridView1.Rows[i].Cells[12].Text.Trim());
                                            drUpdating["AvgPrice"] = 0.00;
                                            drUpdating["LastPrice"] = 0.00;

                                            drUpdating["CurrencyCode"] = ddl_CurrCode.Value;

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (GridView1.Rows[i].Cells[6].Text.Trim() != "&nbsp;") // Check Price is empty
                    {
                        if (GridView1.Rows[i].Cells[6].Text.Trim() != string.Empty)
                        {
                            if (decimal.Parse(GridView1.Rows[i].Cells[6].Text.Trim()) > 0) // check price > 0
                            {
                                if ((GridView1.Rows[i].Cells[10].Text.Trim() == "A" &&
                                     decimal.Parse(GridView1.Rows[i].Cells[11].Text.Trim()) > 0)
                                    ||
                                    (GridView1.Rows[i].Cells[10].Text.Trim() == "I" &&
                                     decimal.Parse(GridView1.Rows[i].Cells[11].Text.Trim()) > 0)
                                    || (GridView1.Rows[i].Cells[10].Text.Trim() == "N"))
                                {
                                    var drNew = dsPLImp.Tables[plDt.TableName].NewRow();
                                    drNew["PriceLstNo"] = dsPLImp.Tables[pl.TableName].Rows[0]["PriceLstNo"].ToString();

                                    if (dsPLImp.Tables[plDt.TableName].Rows.Count == 0)
                                    {
                                        drNew["SeqNo"] = 1;
                                    }
                                    else
                                    {
                                        var RowCount = dsPLImp.Tables[plDt.TableName].Rows.Count;
                                        var LatestSeqNo =
                                            int.Parse(
                                                dsPLImp.Tables[plDt.TableName].Rows[RowCount - 1]["SeqNo"].ToString());
                                        drNew["SeqNo"] = LatestSeqNo + 1;
                                    }

                                    drNew["ProductCode"] = GridView1.Rows[i].Cells[0].Text.Trim();
                                    drNew["OrderUnit"] = GridView1.Rows[i].Cells[3].Text.Trim();
                                    drNew["VendorRank"] = txt_Vrank.Text;

                                    if (GridView1.Rows[i].Cells[4].Text.Trim() == "&nbsp;" ||
                                        GridView1.Rows[i].Cells[4].Text.Trim() == string.Empty)
                                    {
                                        drNew["QtyFrom"] = 0;
                                    }
                                    else
                                    {
                                        drNew["QtyFrom"] = GridView1.Rows[i].Cells[4].Text.Trim();
                                    }

                                    if (GridView1.Rows[i].Cells[5].Text.Trim() == "&nbsp;" ||
                                        GridView1.Rows[i].Cells[5].Text.Trim() == string.Empty)
                                    {
                                        drNew["QtyTo"] = 0;
                                    }
                                    else
                                    {
                                        drNew["QtyTo"] = GridView1.Rows[i].Cells[5].Text.Trim();
                                    }

                                    if (GridView1.Rows[i].Cells[6].Text.Trim() == "&nbsp;" ||
                                        GridView1.Rows[i].Cells[6].Text.Trim() == string.Empty)
                                    {
                                        drNew["QuotedPrice"] = 0;
                                    }
                                    else
                                    {
                                        drNew["QuotedPrice"] = GridView1.Rows[i].Cells[6].Text.Trim();
                                    }

                                    drNew["MarketPrice"] = 0.00;

                                    if (GridView1.Rows[i].Cells[7].Text.Trim() == "&nbsp;" ||
                                        GridView1.Rows[i].Cells[7].Text.Trim() == string.Empty)
                                    {
                                        drNew["FOC"] = 0;
                                    }
                                    else
                                    {
                                        drNew["FOC"] = GridView1.Rows[i].Cells[7].Text.Trim();
                                    }

                                    drNew["Comment"] = (GridView1.Rows[i].Cells[13].Text.Trim() == "&nbsp;"
                                        ? string.Empty
                                        : GridView1.Rows[i].Cells[13].Text.Trim());

                                    if (GridView1.Rows[i].Cells[8].Text.Trim() == "&nbsp;" ||
                                        GridView1.Rows[i].Cells[8].Text.Trim() == string.Empty)
                                    {
                                        drNew["DiscPercent"] = 0;
                                    }
                                    else
                                    {
                                        drNew["DiscPercent"] = GridView1.Rows[i].Cells[8].Text.Trim();
                                    }

                                    if (GridView1.Rows[i].Cells[9].Text.Trim() == "&nbsp;" ||
                                        GridView1.Rows[i].Cells[9].Text.Trim() == string.Empty)
                                    {
                                        drNew["DiscAmt"] = 0;
                                    }
                                    else
                                    {
                                        drNew["DiscAmt"] = GridView1.Rows[i].Cells[9].Text.Trim();
                                    }

                                    if (GridView1.Rows[i].Cells[11].Text.Trim() == "&nbsp;" ||
                                        GridView1.Rows[i].Cells[11].Text.Trim() == string.Empty)
                                    {
                                        drNew["TaxRate"] = 0;
                                    }
                                    else
                                    {
                                        drNew["TaxRate"] = GridView1.Rows[i].Cells[11].Text.Trim();
                                    }

                                    if (GridView1.Rows[i].Cells[10].Text.Trim() != null)
                                    {
                                        drNew["Tax"] = GridView1.Rows[i].Cells[10].Text.Trim();
                                        decimal DiscAmt = 0;
                                        decimal TaxRate = 0;
                                        decimal Price = 0;

                                        TaxRate = Convert.ToDecimal(drNew["TaxRate"].ToString());
                                        DiscAmt = Convert.ToDecimal(drNew["DiscAmt"].ToString());
                                        Price = Convert.ToDecimal(drNew["QuotedPrice"].ToString());

                                        switch (drNew["Tax"].ToString().ToUpper())
                                        {
                                            case "N":
                                                drNew["NetAmt"] = (Price - DiscAmt).ToString();
                                                break;

                                            case "A":
                                                drNew["NetAmt"] = ((Price - DiscAmt) + ((Price * TaxRate) / 100)).ToString();
                                                break;

                                            case "I":
                                                drNew["NetAmt"] =
                                                    ((Price - DiscAmt) - ((Price * TaxRate) / (100 + TaxRate))).ToString();
                                                break;
                                        }
                                    }

                                    drNew["VendorProdCode"] = (GridView1.Rows[i].Cells[12].Text.Trim() == "&nbsp;"
                                        ? string.Empty
                                        : GridView1.Rows[i].Cells[12].Text.Trim());
                                    drNew["AvgPrice"] = 0.00;
                                    drNew["LastPrice"] = 0.00;
                                    drNew["CurrencyCode"] = ddl_CurrCode.Value;

                                    dsPLImp.Tables[plDt.TableName].Rows.Add(drNew);

                                    Session["dsPLImp"] = dsPLImp;
                                }
                            }
                        }
                    }
                }
            }

            var result = pl.Save(dsPLImp, LoginInfo.ConnStr);

            if (result)
            {
                pop_ImportSuccess.ShowOnPageLoad = true;
            }

            pop_ConfirmSave.ShowOnPageLoad = false;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                var strTest = Guid.NewGuid().ToString();
                var FileNameUpload = strTest +
                                     FileUpload1.FileName.Substring(FileUpload1.FileName.LastIndexOf('.')).ToLower();
                FileUpload1.PostedFile.SaveAs(Server.MapPath("~/Tmp/") + FileNameUpload);
                ShowDatatoGrid(FileNameUpload);
            }
        }

        private void ShowDatatoGrid(string f)
        {
            var sr_Imported = new StreamReader(Server.MapPath("~/Tmp/") + f, Encoding.GetEncoding("tis-620"));
            var headerLine = string.Empty;
            var dataLine = string.Empty;

            // Create Table
            headerLine = sr_Imported.ReadLine();
            CreateCSVTable(headerLine);

            while ((dataLine = sr_Imported.ReadLine()) != null)
            {
                AddRowCSVTable(dataLine);
            }


            GridView1.DataSource = CSVTable;
            GridView1.DataBind();

            if (sr_Imported != null)
            {
                sr_Imported.Close();
            }
        }

        public void CreateCSVTable(string TableColumnList)
        {
            CSVTable = new DataTable("CSVTable");
            DataColumn myDataColumn;

            var ColumnName = TableColumnList.Split(',');

            for (var i = 0; i <= ColumnName.Length - 1; i++)
            {
                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = ColumnName[i];
                CSVTable.Columns.Add(myDataColumn);
            }
        }

        private List<string> getColumn(string line)
        {
            List<string> column = new List<string>();

            string value = string.Empty;

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == ',')
                {
                    column.Add(value);
                    value = string.Empty;
                }
                else if (line[i] == '"')
                {
                    for (i++; i < line.Length; i++)
                    {
                        if (line[i] == '"' && i + 1 < line.Length && line[i + 1] == '"') // Check if next is ", then it will be represent double quote '"' example "" => "
                        {
                            value += line[i];
                            i = i + 1;
                        }
                        else if (line[i] == '"') // End sentence
                        {
                            //column.Add(value);
                            //value = string.Empty;
                            break;
                        }
                        else
                        {
                            value += line[i];
                        }

                    }
                }
                else
                {
                    value += line[i];
                }
            }


            return column;
        }



        public void AddRowCSVTable(string RowValueList)
        {
            List<string> rows = getColumn(RowValueList);
            DataRow myDataRow = CSVTable.NewRow();

            for (int i = 0; i < rows.Count; i++)
            {
                myDataRow[i] = rows[i].Replace("'","");
            }
            CSVTable.Rows.Add(myDataRow);

        }

        private DataTable getWorksheet(string worksheet)
        {
            var m_ds = new DataSet();
            try
            {
                exConn = new OleDbConnection(strConn);
                exConn.Open();
                var m_da = new OleDbDataAdapter("SELECT * FROM [" + worksheet + "] ", exConn);
                m_da.Fill(m_ds, "Table");
            }
            catch (Exception ex)
            {
                Label14.Text = ex.Message + "";
            }
            finally
            {
                exConn.Close();
            }
            if (m_ds.Tables.Count > 0)
            {
                return m_ds.Tables[0];
            }

            dt = new DataTable();
            return dt;
        }

        protected void btn_ok_Click(object sender, EventArgs e)
        {
            Response.Redirect("ByVdImp.aspx");
        }

        protected void btn_CancelSave_Click(object sender, EventArgs e)
        {
            pop_ConfirmSave.ShowOnPageLoad = false;
        }

        protected void btn_OK_Vendor_Click(object sender, EventArgs e)
        {
            pop_Vendor.ShowOnPageLoad = false;
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":
                    Save();
                    break;

                case "BACK":
                    Response.Redirect("Vendor/VdList.aspx");
                    break;
            }
        }

        protected void btn_OK_Vendor_Pop_Click(object sender, EventArgs e)
        {
            pop_VendorCode.ShowOnPageLoad = false;
        }

        protected void btn_OK_ProdUnit_Click(object sender, EventArgs e)
        {
            pop_ProdUnit.ShowOnPageLoad = false;
        }

        protected void btn_OK_UnitInsert_Click(object sender, EventArgs e)
        {

            dtProdUnit = (DataTable)Session["dtProdUnit"];
            var get = prodUnit.GetSchema(dsProdUnit, hf_ConnStr.Value);
            if (!get)
            {
                return;
            }

            for (var i = 0; i <= grd_UnitProd.Rows.Count - 1; i++)
            {
                var txt_Rate = grd_UnitProd.Rows[i].FindControl("txt_Rate") as TextBox;

                var drProdUnitOrd = dsProdUnit.Tables[prodUnit.TableName].NewRow();

                drProdUnitOrd["ProductCode"] = dtProdUnit.Rows[i]["ProductCode"].ToString();
                drProdUnitOrd["OrderUnit"] = dtProdUnit.Rows[i]["OrderUnit"].ToString();
                drProdUnitOrd["Rate"] = decimal.Parse(txt_Rate.Text);
                drProdUnitOrd["IsDefault"] = false;
                drProdUnitOrd["UnitType"] = 'O';

                dsProdUnit.Tables[prodUnit.TableName].Rows.Add(drProdUnitOrd);
            }

            var result = prodUnit.Save(dsProdUnit, hf_ConnStr.Value);
            if (result)
            {
                pop_UnitInsert.ShowOnPageLoad = false;
                Save();
            }



        }
    }
}