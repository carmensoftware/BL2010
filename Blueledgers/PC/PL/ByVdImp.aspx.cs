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
        private readonly Blue.BL.APP.Config _config = new Blue.BL.APP.Config();
        private readonly Blue.BL.GnxLib gnxLib = new Blue.BL.GnxLib();

        private readonly Blue.BL.PC.PL.PL _pl = new Blue.BL.PC.PL.PL();
        private readonly Blue.BL.PC.PL.PLDt _plDt = new Blue.BL.PC.PL.PLDt();
        private readonly Blue.BL.IN.ProdUnit _prodUnit = new Blue.BL.IN.ProdUnit();
        private readonly Blue.BL.Ref.Currency _currency = new Blue.BL.Ref.Currency();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();

        private DataTable _dtPriceList
        {
            set { ViewState["_dtPriceList"] = value; }
            get { return ViewState["_dtPriceList"] as DataTable; }
        }


        private int _ViewNo
        {
            get
            {
                var dtView = _config.DbExecuteQuery("SELECT TOP(1) ViewNo FROM APP.ViewHandler WHERE PageCode='[IN].[vPLVdList]' ORDER BY ViewNo", null, LoginInfo.ConnStr);

                return dtView != null && dtView.Rows.Count > 0
                    ? dtView.AsEnumerable().FirstOrDefault().Field<Int32>("ViewNo")
                    : 0;
            }
        }

        // -------------------


        private OleDbConnection exConn; //ติดต่อ Excel

        private readonly DataSet dsProdUnit = new DataSet();
        private DataSet dsPLImp = new DataSet();
        private DataTable dt; //Table เก็บข้อมูล Excel  
        private DataTable dtProdUnit = new DataTable();
        private string strConn = "";
        private static string defaultCurrency = string.Empty;


        // Init
        protected void Page_Init(object sender, EventArgs e)
        {
            //LoginInfo.ConnStr = LoginInfo.ConnStr;
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

            defaultCurrency = _config.GetValue("APP", "BU", "DefaultCurrency", LoginInfo.ConnStr);

        }

        private void Page_Retrieve()
        {
            var getPLSchema = _pl.GetSchame(dsPLImp, LoginInfo.ConnStr);

            if (!getPLSchema)
            {
                return;
            }

            var getPLdtSchema = _plDt.GetSchame(dsPLImp, LoginInfo.ConnStr);

            if (!getPLdtSchema)
            {
                return;
            }

            // Add new row to PL DataTable
            var drPL = dsPLImp.Tables[_pl.TableName].NewRow();

            drPL["PriceLstNo"] = _pl.GetNewID(LoginInfo.ConnStr);
            drPL["VendorCode"] = string.Empty;
            drPL["DateFrom"] = ServerDateTime;
            drPL["DateTo"] = ServerDateTime.AddDays(30); // Fixed 30 Days : Get from parameter next relase
            drPL["CreatedDate"] = ServerDateTime;
            drPL["CreatedBy"] = LoginInfo.LoginName;
            drPL["UpdatedDate"] = ServerDateTime;
            drPL["UpdatedBy"] = LoginInfo.LoginName;

            dsPLImp.Tables[_pl.TableName].Rows.Add(drPL);

            Session["dsPLImp"] = dsPLImp;

            Page_Setting();
        }

        private void Page_Setting()
        {
            var drPL = dsPLImp.Tables[_pl.TableName].Rows[0];

            ddl_Vendor.Value = drPL["VendorCode"].ToString();
            txt_DateFrom.Date = DateTime.Parse(drPL["DateFrom"].ToString());
            txt_DateTo.Date = DateTime.Parse(drPL["DateTo"].ToString());
            txt_RefNo.Text = drPL["RefNo"].ToString();

            var dtConfig = _pl.DbExecuteQuery("SELECT [Value] FROM APP.Config WHERE [Key]='DefaultCurrency' AND SubModule='BU' AND Module='APP'", null, LoginInfo.ConnStr);
            defaultCurrency = dtConfig != null && dtConfig.Rows.Count > 0 ? dtConfig.Rows[0][0].ToString() : "THB";

            var dtCurrency = _pl.DbExecuteQuery("SELECT CurrencyCode, CONCAT(CurrencyCode,' : ', [Desc]) as CurrencyName FROM [REF].Currency WHERE IsActived=1 ORDER BY CurrencyCode", null, LoginInfo.ConnStr);
            ddl_CurrCode.Items.Clear();
            ddl_CurrCode.ValueField = "CurrencyCode";
            ddl_CurrCode.TextField = "CurrencyName";

            foreach (DataRow dr in dtCurrency.Rows)
            {
                ddl_CurrCode.Items.Add(new DevExpress.Web.ASPxEditors.ListEditItem
                {
                    Text = dr["CurrencyName"].ToString(),
                    Value = dr["CurrencyCode"].ToString(),
                    Selected = dr["CurrencyCode"].ToString() == defaultCurrency

                });
            }


            var dtVendor = _pl.DbExecuteQuery("SELECT VendorCode, CONCAT(VendorCode, ' : ', [Name]) as VendorName FROM AP.Vendor WHERE IsActive=1 ORDER BY VendorCode", null, LoginInfo.ConnStr);

            ddl_Vendor.Items.Clear();
            ddl_Vendor.ValueField = "VendorCode";
            ddl_Vendor.TextField = "VendorName";

            foreach (DataRow dr in dtVendor.Rows)
            {
                ddl_Vendor.Items.Add(new DevExpress.Web.ASPxEditors.ListEditItem
                {
                    Text = dr["VendorName"].ToString(),
                    Value = dr["VendorCode"].ToString()
                });
            }

        }


        #region -- Event(s)--

        protected void btn_UploadFile_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {

                var guid = Guid.NewGuid().ToString();
                var fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                var tmpFilename = Path.Combine(Path.GetTempPath(), guid + "_" + fileName);

                FileUpload1.PostedFile.SaveAs(tmpFilename);

                ShowUploadFile(tmpFilename);
            }
            else
            {
                lbl_Alert.Text = "Please select a file to import.";
                pop_Alert.ShowOnPageLoad = true;
            }
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":
                    CheckData();
                    break;

                case "BACK":
                    Response.Redirect("Vendor/VdList.aspx");
                    break;
            }
        }

        protected void btn_Save_NewOrderUnit_Click(object sender, EventArgs e)
        {
            var prodUnits = new List<ProdUnit>();

            for (var i = 0; i <= grd_UnitProd.Rows.Count - 1; i++)
            {
                var row = grd_UnitProd.Rows[i];

                var lbl_ProductCode = row.FindControl("lbl_ProductCode") as Label;
                var lbl_Unit = row.FindControl("lbl_Unit") as Label;

                var txt_Rate = row.FindControl("txt_Rate") as TextBox;

                var rate = 0m;

                if (decimal.TryParse(txt_Rate.Text, out rate))
                {

                    if (rate <= 0)
                    {
                        ShowAlert(string.Format("Rate is required for this product '{0}'.(Not allow zero)", lbl_ProductCode.Text));

                        return;
                    }
                    else
                    {
                        prodUnits.Add(new ProdUnit
                        {
                            ProductCode = lbl_ProductCode.Text,
                            OrderUnit = lbl_Unit.Text,
                            Rate = rate
                        });
                    }
                }
            }

            //Insert to [IN].ProdUnit
            foreach (var item in prodUnits)
            {
                _config.DbExecuteQuery(string.Format("INSERT INTO [IN].ProdUnit (ProductCode, OrderUnit, Rate, IsDefault, UnitType) VALUES ('{0}','{1}',{2},0,'O')",
                    item.ProductCode,
                    item.OrderUnit,
                    item.Rate), null, LoginInfo.ConnStr);
            }

            pop_NewOrderUnit.ShowOnPageLoad = false;

            CheckData();
        }

        protected void btn_ConfirmReplace_Click(object sender, EventArgs e)
        {
            SaveReplace();

            pop_NewOrReplace.ShowOnPageLoad = false;
        }

        protected void btn_ConfirmNew_Click(object sender, EventArgs e)
        {
            SaveNew();

            pop_NewOrReplace.ShowOnPageLoad = false;
        }


        protected void btn_Saved_Import_Click(object sender, EventArgs e)
        {
            Response.Redirect("ByVdImp.aspx");
            pop_Saved.ShowOnPageLoad = false;
        }

        protected void btn_Saved_View_Click(object sender, EventArgs e)
        {
            var viewNo = _ViewNo;

            if (viewNo > 0)
            {
                var priceListNo = hf_PriceListNo.Value;

                Response.Redirect(string.Format("Vendor/Vd.aspx?BuCode=PK&ID={0}&VID={1}", priceListNo, viewNo));
            }
            else
            {
                Response.Redirect(string.Format("Vendor/VdList.aspx"));
            }

            pop_Saved.ShowOnPageLoad = false;
        }

        #endregion







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
                lbl_Upload_Message.Text = ex.Message + "";
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





        // -------------------------------------------------------------

        #region -- Method(s)--

        private void ShowAlert(string text)
        {
            lbl_Alert.Text = text;
            pop_Alert.ShowOnPageLoad = true;
        }

        private DataTable CreatePriclistDataTable(string TableColumnList)
        {
            var dt = new DataTable();

            DataColumn myDataColumn;

            var ColumnName = TableColumnList.Split(',');

            for (var i = 0; i <= ColumnName.Length - 1; i++)
            {
                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = ColumnName[i];

                dt.Columns.Add(myDataColumn);
            }

            return dt;
        }

        public void AddItemToPriceList(DataTable dt, string rowColumns)
        {
            var rows = getColumn(rowColumns);
            var dr = _dtPriceList.NewRow();


            for (int i = 0; i < rows.Count; i++)
            {
                dr[i] = rows[i].Replace("'", "");
            }
            dt.Rows.Add(dr);
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

        private decimal CalculateNet(string taxType, decimal taxRate, decimal quotedPrice, decimal discAmt)
        {
            var netAmt = 0m;
            var sub = quotedPrice - discAmt;

            if (taxType == "I")
                netAmt = RoundAmt(100 / (100 + taxRate) * sub);
            else if (taxType == "A")
            {
                netAmt = sub + RoundAmt(sub * taxRate / 100);
            }
            else
                netAmt = sub;

            return netAmt;
        }


        private void ShowUploadFile(string filename)
        {
            var streamReader = new StreamReader(filename, Encoding.GetEncoding("tis-620"));

            // Create Table
            var columnNames = streamReader.ReadLine();

            _dtPriceList = CreatePriclistDataTable(columnNames);

            var dataRow = string.Empty;

            while ((dataRow = streamReader.ReadLine()) != null)
            {
                AddItemToPriceList(_dtPriceList, dataRow);
            }

            #region -- Check invalid product code --

            var productList = _dtPriceList.AsEnumerable()
                .Select(x => x.Field<string>("SKU#").TrimStart('\'').Trim())
                .Distinct().ToArray();
            var productCodes = "('" + String.Join("'), ('", productList) + "')";
            var query = string.Format(@"

DECLARE @prd TABLE(
	ProductCode nvarchar(20) NOT NULL,

	PRIMARY KEY (ProductCode)
);

INSERT INTO @prd VALUES {0};

SELECT
	prd.ProductCode
FROM
	@prd prd
	LEFT JOIN [IN].Product p ON prd.ProductCode=p.ProductCode
WHERE
	p.ProductCode IS NULL
ORDER BY
	prd.ProductCode", productCodes);

            var dtProduct = _pl.DbExecuteQuery(query, null, LoginInfo.ConnStr);

            var errors = new StringBuilder();
            var errorCodes = new List<string>();

            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                foreach (DataRow dr in dtProduct.Rows)
                {
                    var sku = dr[0].ToString();

                    errorCodes.Add(sku);
                    errors.AppendLine(string.Format("'{0}' product not found.<br/>", sku));

                }
            }

            var error = errors.ToString().Trim();

            if (!string.IsNullOrEmpty(error))
            {
                for (int i = _dtPriceList.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = _dtPriceList.Rows[i];

                    var sku = row["SKU#"].ToString();

                    if (errorCodes.IndexOf(sku) >= 0)
                    {
                        _dtPriceList.Rows.Remove(row);
                    }
                }

                _dtPriceList.AcceptChanges();

                ShowAlert(error);
            }

            #endregion

            GridView1.DataSource = _dtPriceList;
            GridView1.DataBind();

            if (streamReader != null)
            {
                streamReader.Close();
            }
        }


        private void CheckData()
        {

            #region -- Check required --
            if (ddl_Vendor.Value == null)
            {
                ShowAlert("Vendor code is required.");
                return;
            }

            if (string.IsNullOrEmpty(ddl_CurrCode.Value.ToString()))
            {
                ShowAlert("Currency is required.");
                return;
            }

            if (string.IsNullOrEmpty(txt_Vrank.Text))
            {
                ShowAlert("Ranking is required.");
                return;
            }

            #endregion

            if (_dtPriceList == null || _dtPriceList.Rows.Count == 0)
            {
                ShowAlert("No any details.");

                return;
            }

            #region -- Check new order unit--

            var productCodeName = _dtPriceList.Columns[0].ColumnName;
            var productDesc1Name = _dtPriceList.Columns[1].ColumnName;
            var productDesc2Name = _dtPriceList.Columns[2].ColumnName;
            var unitCodeName = _dtPriceList.Columns[3].ColumnName;


            var productCodes = _dtPriceList.AsEnumerable()
                .Select(x => x.Field<string>(productCodeName))
                .Distinct()
                .ToArray();

            var query = string.Format(@"
SELECT 
	p.ProductCode, 
	p.InventoryUnit,
	u.OrderUnit,
	u.Rate
FROM
	[IN].Product p
	LEFT JOIN [IN].ProdUnit u ON u.ProductCode=p.ProductCode AND u.UnitType='O'
WHERE 
	p.ProductCode in ('{0}') 
ORDER BY 
	p.ProductCode, 
	u.OrderUnit", string.Join("','", productCodes));

            //ShowAlert(query);
            //return;

            var dt = _config.DbExecuteQuery(query, null, LoginInfo.ConnStr);

            var prodUnits = dt.AsEnumerable()
                .Select(x => new ProdUnit
                {
                    ProductCode = x.Field<string>("ProductCode"),
                    InventoryUnit = x.Field<string>("InventoryUnit"),
                    OrderUnit = x.Field<string>("OrderUnit"),
                })
                .ToArray();

            var priceListUnits = _dtPriceList.AsEnumerable()
                .Select(x => new ProdUnit
                {
                    ProductCode = x.Field<string>(productCodeName),
                    ProductDesc1 = x.Field<string>(productDesc1Name),
                    ProductDesc2 = x.Field<string>(productDesc2Name),
                    OrderUnit = x.Field<string>(unitCodeName)
                })
                .ToArray();


            var dtUnitProd = _config.DbExecuteQuery(@"
SELECT TOP(0)
	ProductCode,
	ProductDesc1,
	ProductDesc2,
	InventoryUnit,
	OrderUnit,
	InventoryConvOrder as Rate
FROM
	[IN].Product", null, LoginInfo.ConnStr);


            foreach (var pl in priceListUnits)
            {
                var item = prodUnits.FirstOrDefault(x => x.ProductCode == pl.ProductCode && x.OrderUnit == pl.OrderUnit);

                if (item == null)
                {
                    var dr = dtUnitProd.NewRow();

                    var unit = prodUnits.FirstOrDefault(x => x.ProductCode == pl.ProductCode);

                    dr["ProductCode"] = pl.ProductCode;
                    dr["ProductDesc1"] = pl.ProductDesc1;
                    dr["ProductDesc2"] = pl.ProductDesc2;
                    dr["InventoryUnit"] = unit.InventoryUnit ?? "";
                    dr["OrderUnit"] = pl.OrderUnit;

                    dtUnitProd.Rows.Add(dr);
                }
            }

            if (dtUnitProd.Rows.Count > 0)
            {
                grd_UnitProd.DataSource = dtUnitProd;
                grd_UnitProd.DataBind();

                pop_NewOrderUnit.ShowOnPageLoad = true;

                return;
            }

            #endregion


            CheckExistPriceList();
        }


        private void CheckExistPriceList()
        {
            var query = new StringBuilder();

            // check if existing pricelist (datefrom, dateTo and vendorCode)
            var dateFrom = txt_DateFrom.Text;
            var dateTo = txt_DateTo.Text;
            var vendorCode = ddl_Vendor.Value.ToString();

            query.AppendLine("SELECT TOP(1) PriceLstNo FROM [IN].PL WHERE 1=1");

            query.AppendFormat(" AND DateFrom {0}", string.IsNullOrEmpty(dateFrom) ? " IS NULL" : string.Format(" = '{0}'", Convert.ToDateTime(txt_DateFrom.Text).ToString("yyyy-MM-dd")));
            query.AppendFormat(" AND DateTo {0}", string.IsNullOrEmpty(dateTo) ? " IS NULL" : string.Format(" = '{0}'", Convert.ToDateTime(txt_DateTo.Text).ToString("yyyy-MM-dd")));
            query.AppendFormat(" AND VendorCode = '{0}'", vendorCode);

            var dt = _config.DbExecuteQuery(query.ToString(), null, LoginInfo.ConnStr);

            if (dt != null && dt.Rows.Count > 0)
            {
                var priceListNo = dt.Rows[0][0].ToString();

                lbl_NewOrReplace.Text = string.Format("This pricelist has already exists by<br/> Valid from '{0}' and Valid To '{1}' and Vendor '{2}'",
                    dateFrom,
                    dateTo,
                    vendorCode);


                link_ExistPriceList.NavigateUrl = string.Format("~/PC/PL/Vendor/Vd.aspx?BuCode={0}&ID={1}&VID={2}", LoginInfo.BuInfo.BuCode, priceListNo, _ViewNo);

                hf_PriceListNo.Value = priceListNo;
                pop_NewOrReplace.ShowOnPageLoad = true;
            }
            else
            {
                SaveNew();
            }
        }

        private void SaveNew()
        {
            var priceLstNo = _pl.GetNewID(LoginInfo.ConnStr);
            var refNo = txt_RefNo.Text.Trim();
            var dateFrom = txt_DateFrom.Date;
            var dateTo = txt_DateTo.Date;
            var vendorCode = ddl_Vendor.Value.ToString();
            var createdBy = LoginInfo.LoginName;

            var query = @"
INSERT INTO [IN].[PL] ([PriceLstNo], [RefNo], [DateFrom], [DateTo], [VendorCode], [CreatedDate], [CreatedBy], [UpdatedDate],[UpdatedBy])
VALUES (@PriceLstNo, @RefNo, @DateFrom, @DateTo, @VendorCode, GETDATE(), @CreatedBy, GETDATE(), @CreatedBy)";

            var sql = new Helpers.SQL(LoginInfo.ConnStr);
            var parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@PriceLstNo", priceLstNo.ToString()));
            parameters.Add(new SqlParameter("@RefNo", refNo));

            parameters.Add(new SqlParameter("@DateFrom", dateFrom.ToString("yyyy-MM-dd")));

            if (string.IsNullOrEmpty(txt_DateTo.Text.Trim()))
                parameters.Add(new SqlParameter("@DateTo", DBNull.Value));
            else
                parameters.Add(new SqlParameter("@DateTo", dateTo.ToString("yyyy-MM-dd")));

            parameters.Add(new SqlParameter("@VendorCode", vendorCode));
            parameters.Add(new SqlParameter("@CreatedBy", createdBy));


            sql.ExecuteQuery(query, parameters.ToArray());

            var seqNo = 1;

            foreach (DataRow dr in _dtPriceList.Rows)
            {
                var value = 0m;

                #region
                var productCode = dr[0].ToString();
                var orderUnit = dr[3].ToString().Trim();
                var qtyFrom = decimal.TryParse(dr[4].ToString(), out value) ? value : 0m; // string.IsNullOrEmpty(dr[4].ToString()) ? 0m : Convert.ToDecimal(dr[4].ToString());
                var qtyTo = decimal.TryParse(dr[5].ToString(), out value) ? value : 0m;// string.IsNullOrEmpty(dr[5].ToString()) ? 0m : Convert.ToDecimal(dr[5].ToString());
                var quotedPrice = decimal.TryParse(dr[6].ToString(), out value) ? value : 0m; // string.IsNullOrEmpty(dr[6].ToString()) ? 0m : Convert.ToDecimal(dr[6].ToString());
                var foc = decimal.TryParse(dr[7].ToString(), out value) ? value : 0m; //string.IsNullOrEmpty(dr[7].ToString()) ? 0m : Convert.ToDecimal(dr[7].ToString());
                var discRate = decimal.TryParse(dr[8].ToString(),out value) ? value : 0m; // string.IsNullOrEmpty(dr[8].ToString()) ? 0m : Convert.ToDecimal(dr[8].ToString());
                var discAmt = decimal.TryParse(dr[9].ToString(), out value) ? value : 0m; //string.IsNullOrEmpty(dr[9].ToString()) ? 0m : Convert.ToDecimal(dr[9].ToString());

                var taxType = dr[10].ToString().ToUpper();
                var taxRate = string.IsNullOrEmpty(dr[11].ToString()) ? 0m : Convert.ToDecimal(dr[11].ToString());
                var comment = dr[12].ToString().Trim();


                if (quotedPrice > 0)
                {

                    var currencyCode = ddl_CurrCode.Value.ToString();
                    var vendorRank = string.IsNullOrEmpty(txt_Vrank.Text) ? 1 : Convert.ToSingle(txt_Vrank.Text);

                    if (taxType.StartsWith("I"))
                        taxType = "I";
                    else if (taxType.StartsWith("A"))
                        taxType = "A";
                    else
                        taxType = "N";

                    discRate = discRate < 0 ? 0m : discRate;
                    discAmt = discAmt < 0 ? 0m : discAmt;

                    var discount = discAmt > 0 ? discAmt : RoundAmt(quotedPrice * discRate / 100);
                    var netAmt = CalculateNet(taxType, taxRate, quotedPrice, discount);

                    query = @"
INSERT INTO [IN].[PLDT] (
    [PriceLstNo], 
    [SeqNo], 
    [ProductCode], 
    [OrderUnit], 
    VendorRank, 
    [QtyFrom], 
    [QtyTo], 
    Foc, 
    CurrencyCode, 
    QuotedPrice, 
    MarketPrice,
    DiscPercent,
    DiscAmt,
    Tax,
    TaxRate,
    NetAmt,
    Comment)
VALUES(
    @PriceLstNo, 
    @SeqNo, 
    @ProductCode, 
    @OrderUnit, 
    @VendorRank, 
    @QtyFrom, 
    @QtyTo, 
    @Foc, 
    @CurrencyCode, 
    @QuotedPrice, 
    0,
    @DiscPercent,
    @DiscAmt,
    @Tax,
    @TaxRate,
    @NetAmt,
    @Comment)

";
                    var param = new List<SqlParameter>();

                    param.Add(new SqlParameter("@PriceLstNo", priceLstNo));
                    param.Add(new SqlParameter("@SeqNo", seqNo++));
                    param.Add(new SqlParameter("@ProductCode", productCode));
                    param.Add(new SqlParameter("@OrderUnit", orderUnit));
                    param.Add(new SqlParameter("@VendorRank", vendorRank));
                    param.Add(new SqlParameter("@QtyFrom", qtyFrom));
                    param.Add(new SqlParameter("@QtyTo", qtyTo));
                    param.Add(new SqlParameter("@Foc", foc));
                    param.Add(new SqlParameter("@CurrencyCode", currencyCode));
                    param.Add(new SqlParameter("@QuotedPrice", quotedPrice));
                    //param.Add(new SqlParameter("@MarketPrice", 0m));
                    param.Add(new SqlParameter("@DiscPercent", discRate));
                    param.Add(new SqlParameter("@DiscAmt", discAmt));
                    param.Add(new SqlParameter("@Tax", taxType));
                    param.Add(new SqlParameter("@TaxRate", taxRate));
                    param.Add(new SqlParameter("@NetAmt", netAmt));
                    param.Add(new SqlParameter("@Comment", comment));

                    sql.ExecuteQuery(query, param.ToArray());
                }
                #endregion
            }

            hf_PriceListNo.Value = priceLstNo.ToString();
            pop_Saved.ShowOnPageLoad = true;
            //ShowAlert("Price list Save");
            //Response.Redirect("ByVdImp.aspx");
        }

        private void SaveReplace()
        {
            var priceLstNo = Convert.ToInt32(hf_PriceListNo.Value);
            var refNo = txt_RefNo.Text.Trim();
            var dateFrom = txt_DateFrom.Date;
            var dateTo = txt_DateTo.Date;
            var vendorCode = ddl_Vendor.Value.ToString();
            var createdBy = LoginInfo.LoginName;

            var query = @"
UPDATE 
    [IN].[PL] 
SET
    RefNo=@RefNo, 
    UpdatedBy=@UpdatedBy,
    UpdatedDate=GETDATE()
WHERE 
    PriceLstNo=@PriceLstNo ;
DELETE FROM [IN].[PLDT] WHERE PriceLstNo=@PriceLstNo;";

            var sql = new Helpers.SQL(LoginInfo.ConnStr);
            var parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@PriceLstNo", priceLstNo.ToString()));
            parameters.Add(new SqlParameter("@RefNo", refNo));
            parameters.Add(new SqlParameter("@UpdatedBy", createdBy));

            sql.ExecuteQuery(query, parameters.ToArray());

            var seqNo = 1;

            foreach (DataRow dr in _dtPriceList.Rows)
            {
                #region
                var value = 0m;

                var productCode = dr[0].ToString();
                var orderUnit = dr[3].ToString().Trim();
                var qtyFrom = decimal.TryParse(dr[4].ToString(), out value) ? value : 0m; // string.IsNullOrEmpty(dr[4].ToString()) ? 0m : Convert.ToDecimal(dr[4].ToString());
                var qtyTo = decimal.TryParse(dr[5].ToString(), out value) ? value : 0m;// string.IsNullOrEmpty(dr[5].ToString()) ? 0m : Convert.ToDecimal(dr[5].ToString());
                var quotedPrice = decimal.TryParse(dr[6].ToString(), out value) ? value : 0m; // string.IsNullOrEmpty(dr[6].ToString()) ? 0m : Convert.ToDecimal(dr[6].ToString());
                var foc = decimal.TryParse(dr[7].ToString(), out value) ? value : 0m; //string.IsNullOrEmpty(dr[7].ToString()) ? 0m : Convert.ToDecimal(dr[7].ToString());
                var discRate = decimal.TryParse(dr[8].ToString(),out value) ? value : 0m; // string.IsNullOrEmpty(dr[8].ToString()) ? 0m : Convert.ToDecimal(dr[8].ToString());
                var discAmt = decimal.TryParse(dr[9].ToString(), out value) ? value : 0m; //string.IsNullOrEmpty(dr[9].ToString()) ? 0m : Convert.ToDecimal(dr[9].ToString());

                var taxType = dr[10].ToString().ToUpper();
                var taxRate = decimal.TryParse(dr[11].ToString(), out value) ? value : 0m; //string.IsNullOrEmpty(dr[11].ToString()) ? 0m : Convert.ToDecimal(dr[11].ToString());
                var comment = dr[12].ToString().Trim();

                if (quotedPrice > 0)
                {
                    var currencyCode = ddl_CurrCode.Value.ToString();
                    var vendorRank = string.IsNullOrEmpty(txt_Vrank.Text) ? 1 : Convert.ToSingle(txt_Vrank.Text);

                    if (taxType.StartsWith("I"))
                        taxType = "I";
                    else if (taxType.StartsWith("A"))
                        taxType = "A";
                    else
                        taxType = "N";

                    discRate = discRate < 0 ? 0m : discRate;
                    discAmt = discAmt < 0 ? 0m : discAmt;

                    var discount = discAmt > 0 ? discAmt : RoundAmt(quotedPrice * discRate / 100);
                    var netAmt = CalculateNet(taxType, taxRate, quotedPrice, discount);

                    //var sub = quotedPrice - discAmt1;

                    //if (taxType == "I")
                    //    netAmt = RoundAmt(100 / (100 + taxRate) * sub);
                    //else if (taxType == "A")
                    //{
                    //    netAmt = sub + RoundAmt(sub * taxRate / 100);
                    //}
                    //else
                    //    netAmt = quotedPrice;

                    #region
                    query = @"
INSERT INTO [IN].[PLDT] (
    [PriceLstNo], 
    [SeqNo], 
    [ProductCode], 
    [OrderUnit], 
    VendorRank, 
    [QtyFrom], 
    [QtyTo], 
    Foc, 
    CurrencyCode, 
    QuotedPrice, 
    MarketPrice,
    DiscPercent,
    DiscAmt,
    Tax,
    TaxRate,
    NetAmt,
    Comment)
VALUES(
    @PriceLstNo, 
    @SeqNo, 
    @ProductCode, 
    @OrderUnit, 
    @VendorRank, 
    @QtyFrom, 
    @QtyTo, 
    @Foc, 
    @CurrencyCode, 
    @QuotedPrice, 
    0,
    @DiscPercent,
    @DiscAmt,
    @Tax,
    @TaxRate,
    @NetAmt,
    @Comment)

";
                    #endregion

                    var param = new List<SqlParameter>();

                    param.Add(new SqlParameter("@PriceLstNo", priceLstNo));
                    param.Add(new SqlParameter("@SeqNo", seqNo++));
                    param.Add(new SqlParameter("@ProductCode", productCode));
                    param.Add(new SqlParameter("@OrderUnit", orderUnit));
                    param.Add(new SqlParameter("@VendorRank", vendorRank));
                    param.Add(new SqlParameter("@QtyFrom", qtyFrom));
                    param.Add(new SqlParameter("@QtyTo", qtyTo));
                    param.Add(new SqlParameter("@Foc", foc));
                    param.Add(new SqlParameter("@CurrencyCode", currencyCode));
                    param.Add(new SqlParameter("@QuotedPrice", quotedPrice));
                    //param.Add(new SqlParameter("@MarketPrice", 0m));
                    param.Add(new SqlParameter("@DiscPercent", discRate));
                    param.Add(new SqlParameter("@DiscAmt", discAmt));
                    param.Add(new SqlParameter("@Tax", taxType));
                    param.Add(new SqlParameter("@TaxRate", taxRate));
                    param.Add(new SqlParameter("@NetAmt", netAmt));
                    param.Add(new SqlParameter("@Comment", comment));

                    sql.ExecuteQuery(query, param.ToArray());
                }
                #endregion
            }

            hf_PriceListNo.Value = priceLstNo.ToString();
            pop_Saved.ShowOnPageLoad = true;
        }



        #endregion


        public class ProdUnit
        {
            public string ProductCode { get; set; }
            public string ProductDesc1 { get; set; }
            public string ProductDesc2 { get; set; }
            public string InventoryUnit { get; set; }
            public string OrderUnit { get; set; }
            public decimal Rate { get; set; }
        }

    }


}