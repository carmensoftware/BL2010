using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web;
using DevExpress.Web.ASPxEditors;

namespace BlueLedger.PL.IN.REC
{
    public partial class RECEdit : BasePage
    {
        private string _connStr;

        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();


        #region --URL Parameters--

        private string _BuCode { get { return Request.Params["BuCode"].ToString() ?? ""; } }
        private string _VID { get { return Request.Params["VID"].ToString() ?? ""; } }
        private string _ID { get { return Request.Params["ID"].ToString() ?? ""; } }

        private string _MODE { get { return Request.Params["Mode"] == null ? "" : Request.Params["Mode"].ToString().ToLower(); } }
        #endregion

        protected DataTable dtRec
        {
            get { return ViewState["dtRec"] as DataTable; }
            set { ViewState["dtRec"] = value; }
        }

        protected DataTable dtRecDt
        {
            get { return ViewState["dtRecDt"] as DataTable; }
            set { ViewState["dtRecDt"] = value; }
        }

        protected DataTable dtPo
        {
            get { return ViewState["dtPo"] as DataTable; }
            set { ViewState["dtPo"] = value; }
        }

        protected DefaultValues _default
        {
            get { return ViewState["DefaultValues"] as DefaultValues; }
            set { ViewState["DefaultValues"] = value; }
        }


        #region --Event(s)--

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_ConnStr.Value = bu.GetConnectionString(_BuCode);

            var currency = config.GetValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value);
            var digitAmt = config.GetValue("APP", "Default", "DigitAmt", hf_ConnStr.Value);
            var digitQty = config.GetValue("APP", "Default", "DigitQty", hf_ConnStr.Value);
            var taxRate = config.GetValue("APP", "Default", "TaxRate", hf_ConnStr.Value);

            _default = new DefaultValues
            {
                Currency = currency,
                DigitAmt = string.IsNullOrEmpty(digitAmt) ? 2 : Convert.ToInt32(digitAmt),
                DigitQty = string.IsNullOrEmpty(digitQty) ? 2 : Convert.ToInt32(digitQty),
                TaxRate = string.IsNullOrEmpty(taxRate) ? 0 : Convert.ToDecimal(taxRate)
            };
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            _connStr = LoginInfo.ConnStr;

            if (!IsPostBack)
            {
                Page_Retrieve();
                Page_Setting();
            }




        }

        private void Page_Retrieve()
        {



            switch (_MODE)
            {
                case "new":
                    SetNew();
                    break;
                case "edit":
                    var recNo = _ID;
                    SetEdit(recNo);
                    break;
                case "fpo":
                    SetFromPO();
                    break;
            }

            SetHeader(dtRec);
            SetDetails(dtRecDt);


        }

        private void Page_Setting()
        {

        }

        // Title / Action bar
        protected void btn_Save_Click(object sender, EventArgs e)
        {
        }

        protected void btn_Commit_Click(object sender, EventArgs e)
        {
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            var MODE = Request.QueryString["MODE"];


            if (MODE.ToUpper() == "EDIT")
            {
                var id = _ID;
                var buCode = _BuCode;
                var vid = _VID;
                Response.Redirect(string.Format("Rec.aspx?ID={0}&BuCode={1}&Vid={2}", id, buCode, vid));
            }
            else
            {
                Response.Redirect("RecLst.aspx");
            }
        }


        // Header
        protected void ddl_Currency_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void btn_AllocateExtraCost_Click(object sender, EventArgs e)
        {
        }

        protected void btn_ExtraCostDetail_Click(object sender, EventArgs e)
        {
        }

        // Add Item / PO
        protected void btn_AddPo_Click(object sender, EventArgs e)
        {
        }

        protected void btn_AddItem_Click(object sender, EventArgs e)
        {
        }


        // gv_Deatail
        protected void gv_Detail_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var sql = new Helpers.SQL(hf_ConnStr.Value);

                var docDate = de_RecDate.Date;
                var poNo = DataBinder.Eval(e.Row.DataItem, "PoNo").ToString();
                var poDtNo = DataBinder.Eval(e.Row.DataItem, "PoDtNo").ToString();
                var locationCode = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString();
                var productCode = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                var inventoryUnit = DataBinder.Eval(e.Row.DataItem, "InventoryUnit").ToString();


                // Location
                if (e.Row.FindControl("lbl_Location") != null)
                {
                    var code = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString();
                    var name = DataBinder.Eval(e.Row.DataItem, "LocationName").ToString();
                    var label = e.Row.FindControl("lbl_Location") as Label;

                    label.Text = string.Format("{0} : {1}", code, name);
                }

                if (e.Row.FindControl("ddl_Location") != null)
                {
                    var code = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString();

                    var ddl = e.Row.FindControl("ddl_Location") as ASPxComboBox;

                    ddl.Items.Clear();
                    ddl.Items.AddRange(GetLocations(code).ToArray());
                }


                // Product
                if (e.Row.FindControl("lbl_Product") != null)
                {
                    var code = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                    var desc1 = DataBinder.Eval(e.Row.DataItem, "ProductDesc1").ToString();
                    var desc2 = DataBinder.Eval(e.Row.DataItem, "ProductDesc2").ToString();
                    var label = e.Row.FindControl("lbl_Product") as Label;

                    label.Text = string.Format("{0} : {1} - {2}", code, desc1, desc2);
                }

                if (e.Row.FindControl("ddl_Product") != null)
                {
                    var code = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();

                    var ddl = e.Row.FindControl("ddl_Product") as ASPxComboBox;

                    ddl.Items.Clear();
                    ddl.Items.AddRange(GetProductsOnLocation(locationCode, code).ToArray());
                }


                // Order Unit
                if (e.Row.FindControl("lbl_OrdUnit") != null)
                {
                    var label = e.Row.FindControl("lbl_OrdUnit") as Label;

                    label.Text = DataBinder.Eval(e.Row.DataItem, "UnitCode").ToString();
                }

                // Order
                if (e.Row.FindControl("lbl_OrdQty") != null)
                {
                    var label = e.Row.FindControl("lbl_OrdQty") as Label;

                    label.Text = FormatQty(DataBinder.Eval(e.Row.DataItem, "OrderQty"));
                }


                // Receive
                if (e.Row.FindControl("lbl_RecQty") != null)
                {
                    var label = e.Row.FindControl("lbl_RecQty") as Label;

                    label.Text = FormatQty(DataBinder.Eval(e.Row.DataItem, "RecQty"));
                }

                if (e.Row.FindControl("se_RecQty") != null)
                {
                    var se = e.Row.FindControl("se_RecQty") as ASPxSpinEdit;

                    se.Value = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "RecQty"));
                    se.DecimalPlaces = _default.DigitQty;
                }

                // Foc
                if (e.Row.FindControl("lbl_FocQty") != null)
                {
                    var label = e.Row.FindControl("lbl_FocQty") as Label;

                    label.Text = FormatQty(DataBinder.Eval(e.Row.DataItem, "FocQty"));
                }

                if (e.Row.FindControl("se_FocQty") != null)
                {
                    var se = e.Row.FindControl("se_FocQty") as ASPxSpinEdit;

                    se.Value = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "FocQty"));
                    se.DecimalPlaces = _default.DigitQty;
                }

                // Price
                if (e.Row.FindControl("lbl_Price") != null)
                {
                    var label = e.Row.FindControl("lbl_Price") as Label;

                    label.Text = FormatAmt(DataBinder.Eval(e.Row.DataItem, "Price"));
                }

                if (e.Row.FindControl("se_Price") != null)
                {
                    var se = e.Row.FindControl("se_Price") as ASPxSpinEdit;

                    se.Value = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Price"));
                    se.DecimalPlaces = _default.DigitAmt;
                }

                // Discount
                if (e.Row.FindControl("lbl_DiscAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_DiscAmt") as Label;

                    label.Text = FormatAmt(DataBinder.Eval(e.Row.DataItem, "CurrDiscAmt"));
                }
                // Net
                if (e.Row.FindControl("lbl_NetAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_NetAmt") as Label;

                    label.Text = FormatAmt(DataBinder.Eval(e.Row.DataItem, "CurrNetAmt"));
                }
                // Tax
                if (e.Row.FindControl("lbl_TaxAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_TaxAmt") as Label;

                    label.Text = FormatAmt(DataBinder.Eval(e.Row.DataItem, "CurrTaxAmt"));
                }

                // Extra Cost
                if (e.Row.FindControl("lbl_ExtCost") != null)
                {
                    var label = e.Row.FindControl("lbl_ExtCost") as Label;

                    label.Text = FormatAmt(DataBinder.Eval(e.Row.DataItem, "ExtraCost"));
                }
                // Total
                if (e.Row.FindControl("lbl_TotalAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_TotalAmt") as Label;

                    label.Text = FormatAmt(DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt"));
                }
                // Expiry date
                if (e.Row.FindControl("lbl_ExpiryDate") != null)
                {
                    var label = e.Row.FindControl("lbl_ExpiryDate") as Label;

                    label.Text = FormatDate(DataBinder.Eval(e.Row.DataItem, "ExpiryDate"));
                }

                if (e.Row.FindControl("de_ExpiryDate") != null)
                {
                    var de = e.Row.FindControl("de_ExpiryDate") as ASPxDateEdit;

                    de.Text = FormatDate(DataBinder.Eval(e.Row.DataItem, "ExpiryDate"));
                }


                #region --Extra Information--
                
                // Base Qty
                if (e.Row.FindControl("lbl_BaseQty") != null)
                {
                    var label = e.Row.FindControl("lbl_BaseQty") as Label;
                    var ordQty = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "OrderQty"));
                    var rate = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "UnitRate"));
                    var qty = FormatQty(ordQty * rate);
                    var text = string.Format("{0} {1} (rate : {2})",
                        qty,
                        DataBinder.Eval(e.Row.DataItem, "InventoryUnit").ToString(),
                        rate);

                    label.Text = text;
                }

                // PoNo

                if (e.Row.FindControl("lbl_PoNo") != null)
                {
                    var label = e.Row.FindControl("lbl_PoNo") as Label;

                    label.Text = poNo;
                }
                // PrNo
                if (e.Row.FindControl("lbl_PrNo") != null)
                {
                    var dt = sql.ExecuteQuery(string.Format("SELECT PrNo FROM PC.PrDt WHERE PoNo='{0}' AND PoDtNo={1}", poNo, poDtNo));
                    var prNo = dt != null && dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "";

                    var label = e.Row.FindControl("lbl_PrNo") as Label;

                    label.Text = prNo;
                }
                // Onhand
                if (e.Row.FindControl("lbl_Onhand") != null)
                {
                    var dt = sql.ExecuteQuery(string.Format("SELECT SUM([IN]-[OUT]) FROM [IN].Inventory WHERE [Location]='{0}' AND ProductCode='{1}' AND CAST(CommittedDate as DATE)<='{2}'",
                        locationCode,
                        productCode,
                        FormatSqlDate(docDate)));
                    var qty = dt != null && dt.Rows.Count > 0 ? Convert.ToDecimal(dt.Rows[0][0]) : 0m;

                    var label = e.Row.FindControl("lbl_Onhand") as Label;

                    label.Text = string.Format("{0} {1} @{2}", FormatQty(qty), inventoryUnit, FormatDate(docDate));
                }

                var query = @"
SELECT
	TOP(1)
	rec.RecNo,
	rec.VendorCode,
	v.[Name] as VendorName,
	recdt.Price
FROM 
	PC.REC
	JOIN PC.RECDt ON rec.RecNo=recdt.RecNo
	LEFT JOIN AP.Vendor v ON v.VendorCode=rec.VendorCode
WHERE
	DocStatus = 'Committed'
	AND rec.RecNo <> @Id
	AND RecDate <= @DocDate
	AND ProductCode =@ProductCode
ORDER BY
	RecDate DESC";
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("Id", _ID),
                    new SqlParameter("DocDate", FormatSqlDate(docDate)),
                    new SqlParameter("ProductCode", productCode)
                };

                var dtLast = sql.ExecuteQuery(query, parameters);

                var lastPrice = 0m;
                var lastVendor = "";
                var lastDocNo = "";

                if (dtLast != null && dtLast.Rows.Count > 0)
                {
                    var dr = dtLast.Rows[0];

                    lastPrice = Convert.ToDecimal(dr["Price"]);
                    lastVendor = string.Format("{0} : {1}", dr["VendorCode"], dr["VendorName"]);
                    lastDocNo = dr["RecNo"].ToString();
                }

                // Last Price
                if (e.Row.FindControl("lbl_LastPrice") != null)
                {
                    var label = e.Row.FindControl("lbl_LastPrice") as Label;

                    label.Text = string.Format("{0} #{1}", FormatAmt(lastPrice), lastDocNo);
                }

                // Last Vendor
                if (e.Row.FindControl("lbl_LastVendor") != null)
                {
                    var label = e.Row.FindControl("lbl_LastVendor") as Label;

                    label.Text = lastVendor;
                }

                #endregion

            }
        }

        protected void gv_Detail_RowEditing(object sender, GridViewEditEventArgs e)
        {
            var gv = sender as GridView;

            gv.DataSource = dtRecDt;
            gv.EditIndex = e.NewEditIndex;
            gv.DataBind();

            var Img_Btn = gv.Rows[gv.EditIndex].FindControl("Img_Btn") as ImageButton;
            Img_Btn.Visible = false;

            SetEditDetail(gv, true);
        }


        protected void gv_Detail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            var gv = sender as GridView;

            gv.DataSource = dtRecDt;
            gv.EditIndex = -1;
            gv.DataBind();

            SetEditDetail(gv, false);
        }




        #endregion

        #region -- Private method(s)--

        protected string FormatSqlDate(object date)
        {
            return date == null || date == DBNull.Value ? "" : Convert.ToDateTime(date).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        protected string FormatDate(object date)
        {
            return date == null || date == DBNull.Value ? "" : Convert.ToDateTime(date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        protected string FormatQty(object value)
        {
            var f = string.Format("N{0}", _default.DigitQty);

            return value == null ? "0.00" : Convert.ToDecimal(value).ToString(f);
        }

        protected string FormatAmt(object value)
        {
            var f = string.Format("N{0}", _default.DigitAmt);

            return value == null ? "0.00" : Convert.ToDecimal(value).ToString(f);
        }

        private void SetNew()
        {
            var sql = new Helpers.SQL(hf_ConnStr.Value);

            dtRec = sql.ExecuteQuery("SELECT TOP(1) * FROM PC.Rec WHERE RecNo='*'");
            dtRecDt = sql.ExecuteQuery("SELECT TOP(1) * FROM PC.RecDt WHERE RecNo='*'");

            dtRec.NewRow();


        }
        private void SetEdit(string id)
        {
            var query = new Helpers.SQL(hf_ConnStr.Value);

            dtRec = query.ExecuteQuery("SELECT * FROM PC.Rec WHERE RecNo=@id", new SqlParameter[] { new SqlParameter("id", id) });

            var sql = @"
SELECT 
	d.*,
	l.LocationName,
	p.ProductDesc1,
	p.ProductDesc2,
	p.InventoryUnit,
	pu.Rate as UnitRate,
	CASE d.TaxType
		WHEN 'I' THEN 'Include'
		WHEN 'A' THEN 'Add'
		ELSE 'None'
	END as TaxTypeName
FROM 
	PC.RecDt d
	LEFT JOIN [IN].StoreLocation l ON l.LocationCode=d.LocationCode
	LEFT JOIN [IN].Product p ON p.ProductCode=d.ProductCode
	LEFT JOIN [IN].ProdUnit pu ON pu.ProductCode=d.ProductCode AND pu.OrderUnit=d.UnitCode AND UnitType='O'
WHERE 
	RecNo=@id 
ORDER BY 
	RecDtNo";
            dtRecDt = query.ExecuteQuery(sql, new SqlParameter[] { new SqlParameter("id", id) });
        }

        private void SetFromPO()
        {
            var dsPo = Session["dsPo"] as DataSet;


            dtPo = dsPo.Tables["PoDt"].Copy();
            dtRec = dsPo.Tables["REC"].Copy();
            dtRecDt = dsPo.Tables["RECDt"].Copy();
        }


        private void SetHeader(DataTable dt)
        {
            var dr = dt.Rows[0];

            lbl_RecNo.Text = dr["RecNo"].ToString();
            lbl_Receiver.Text = dr["CreatedBy"].ToString();
            lbl_CommitDate.Text = dr["CommitDate"] == DBNull.Value ? "" : Convert.ToDateTime(dr["CommitDate"]).ToString("dd/MM/yyyy hh:mm");
            lbl_DocStatus.Text = dr["DocStatus"].ToString();

            de_RecDate.Date = Convert.ToDateTime(dr["RecDate"]);
            SetVendors(dr["VendorCode"].ToString());
            SetDeliveryPoints(dr["DeliPoint"].ToString());


            de_InvDate.Text = dr["InvoiceDate"] == DBNull.Value ? "" : FormatDate(Convert.ToDateTime(dr["InvoiceDate"]));
            txt_InvNo.Text = dr["InvoiceNo"].ToString();
            chk_CashConsign.Checked = Convert.ToBoolean(dr["IsCashConsign"]);
            SetCurrencyCode(dr["CurrencyCode"].ToString());
            se_CurrencyRate.Text = dr["CurrencyRate"].ToString();

            txt_Desc.Text = dr["Description"].ToString();

            se_TotalExtraCost.Value = Convert.ToDecimal(dr["TotalExtraCost"]);
            rdb_ExtraCostByAmt.Checked = true;
            rdb_ExtraCostByQty.Checked = true;

            lbl_PoSource.Text = string.IsNullOrEmpty(dr["PoSource"].ToString()) ? "by manually created" : "by Purchase Order";

        }

        private void SetDetails(DataTable dt)
        {
            gv_Detail.DataSource = dtRecDt;
            gv_Detail.DataBind();


        }

        private void SetEditDetail(GridView gv, bool isEdit)
        {
            // Header

            ddl_Currency.Enabled = !isEdit;
            se_CurrencyRate.Enabled = !isEdit;


            // Detail
            // Discount
            ((DataControlField)gv.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Discount")
                .SingleOrDefault()).Visible = !isEdit;
            // Net
            ((DataControlField)gv.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Net")
                .SingleOrDefault()).Visible = !isEdit;
            // Tax
            ((DataControlField)gv.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Tax")
                .SingleOrDefault()).Visible = !isEdit;
            // Total
            ((DataControlField)gv.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Total")
                .SingleOrDefault()).Visible = !isEdit;
        }


        private void GetCurrencyRate(string currencyCode, DateTime date)
        {
            var dt = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery("SELECT CurrencyCode as [Value], CurrencyCode as [Text] FROM [Ref].Currency WHERE IsActived=1 ORDER BY CurrencyCode");

            var items = dt.AsEnumerable()
                .Select(x => new DevExpress.Web.ASPxEditors.ListEditItem
                {
                    Value = x.Field<string>("Value"),
                    Text = x.Field<string>("Text"),
                    Selected = x.Field<string>("Value") == currencyCode
                })
                .ToArray();


            ddl_Currency.Items.Clear();
            ddl_Currency.Items.AddRange(items);
        }


        // Dropdown(s)

        private void SetVendors(string value)
        {
            var dt = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery("SELECT VendorCode as [Value], CONCAT(VendorCode,' : ',[Name]) as [Text] FROM AP.Vendor WHERE IsActive=1 ORDER BY VendorCode");

            var items = dt.AsEnumerable()
                .Select(x => new DevExpress.Web.ASPxEditors.ListEditItem
                {
                    Value = x.Field<string>("Value"),
                    Text = x.Field<string>("Text"),
                    Selected = x.Field<string>("Value") == value
                })
                .ToArray();


            ddl_Vendor.Items.Clear();
            ddl_Vendor.Items.AddRange(items);
        }

        private void SetDeliveryPoints(string value)
        {
            var dt = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery("SELECT DptCode as [Value], [Name] as [Text] FROM [IN].DeliveryPoint WHERE IsActived=1 ORDER BY DptCode");

            var items = dt.AsEnumerable()
                .Select(x => new DevExpress.Web.ASPxEditors.ListEditItem
                {
                    Value = x.Field<Int32>("Value"),
                    Text = x.Field<string>("Text"),
                    Selected = x.Field<Int32>("Value").ToString() == value
                })
                .ToArray();


            ddl_DeliPoint.Items.Clear();
            ddl_DeliPoint.Items.AddRange(items);
        }

        private void SetCurrencyCode(string value)
        {
            var dt = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery("SELECT CurrencyCode as [Value], CurrencyCode as [Text] FROM [Ref].Currency WHERE IsActived=1 ORDER BY CurrencyCode");

            var items = dt.AsEnumerable()
                .Select(x => new DevExpress.Web.ASPxEditors.ListEditItem
                {
                    Value = x.Field<string>("Value"),
                    Text = x.Field<string>("Text"),
                    Selected = x.Field<string>("Value") == value
                })
                .ToArray();


            ddl_Currency.Items.Clear();
            ddl_Currency.Items.AddRange(items);

            if (value == _default.Currency)
            {
                se_CurrencyRate.Enabled = false;
                se_CurrencyRate.Value = 1;
            }

        }

        private IEnumerable<ListEditItem> GetLocations(string value)
        {
            var items = new List<ListEditItem>();
            var query = @"
SELECT
	us.LocationCode as [Value],
	CONCAT(l.LocationCode,' : ',l.LocationName) as [Text]
FROM 
	[ADMIN].UserStore us
	JOIN [IN].StoreLocation l ON l.LocationCode=us.LocationCode
WHERE 
	LoginName=@loginName
";
            var dt = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery(query, new SqlParameter[] { new SqlParameter("loginName", LoginInfo.LoginName) });

            if (dt.Rows.Count > 0)
            {
                items = dt.AsEnumerable()
                    .Select(x => new ListEditItem
                    {
                        Value = x.Field<string>("Value"),
                        Text = x.Field<string>("Text"),
                        Selected = x.Field<string>("Value") == value
                    })
                    .ToList();
            }

            return items;
        }

        private IEnumerable<ListEditItem> GetProductsOnLocation(string locationCode, string value)
        {
            var items = new List<ListEditItem>();
            var query = @"
SELECT
	pl.ProductCode as [Value],
	CONCAT(pl.ProductCode,' : ',p.ProductDesc1, ' - ', p.ProductDesc2) as [Text]
FROM 
	[IN].ProdLoc pl
	JOIN [IN].Product p ON p.ProductCode=pl.ProductCode
WHERE 
	p.IsActive = 1
	AND pl.LocationCode=@LocationCode
ORDER BY
	pl.ProductCode
";
            var dt = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery(query, new SqlParameter[] { new SqlParameter("LocationCode", locationCode) });

            if (dt.Rows.Count > 0)
            {
                items = dt.AsEnumerable()
                    .Select(x => new ListEditItem
                    {
                        Value = x.Field<string>("Value"),
                        Text = x.Field<string>("Text"),
                        Selected = x.Field<string>("Value") == value
                    })
                    .ToList();
            }

            return items;
        }


        #endregion

        public class DefaultValues
        {
            public string Currency { get; set; }
            public int DigitAmt { get; set; }
            public int DigitQty { get; set; }
            public decimal TaxRate { get; set; }
        }
    }

}