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
using System.Drawing;

namespace BlueLedger.PL.IN.REC
{
    public partial class RECEdit : BasePage
    {
        #region --URL Parameters--

        protected string _BuCode { get { return Request.Params["BuCode"].ToString() ?? ""; } }
        protected string _VID { get { return Request.Params["VID"] == null ? "" : Request.Params["VID"].ToString() ?? ""; } }
        protected string _ID { get { return Request.Params["ID"] == null ? "" : Request.Params["ID"].ToString() ?? ""; } }
        protected string _MODE { get { return Request.Params["Mode"] == null ? "" : Request.Params["Mode"].ToString().ToLower(); } }

        #endregion

        private string _connStr;

        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();

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
                    // Set disable controls
                    // header
                    ddl_Vendor.Enabled = false;
                    ddl_Currency.Enabled = false;

                    break;
            }

            SetHeader(dtRec);
            SetDetails(dtRecDt);
            SetGrandTotal();
        }

        private void Page_Setting()
        {

        }

        // Title / Action bar
        protected void btn_Save_Click(object sender, EventArgs e)
        {
            Save();
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
            var ddl = sender as ASPxComboBox;
            var code = ddl.Text;


            if (code == _default.Currency)
            {
                se_CurrencyRate.Text = "1.00";
                se_CurrencyRate.Enabled = false;
            }
            else
            {

                var date = de_RecDate.Date.Date;
                var query = "SELECT TOP(1) CurrencyRate FROM [Ref].[CurrencyExchange] WHERE CurrencyCode=@code AND CAST(UpdatedDate AS DATE) <= CAST(@date as DATE) ORDER BY UpdatedDate DESC";

                var dt = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery(query, new SqlParameter[]
            {
                new SqlParameter("Code", code),
                new SqlParameter("Date", date)
            });

                var rate = 0m;

                if (dt != null || dt.Rows.Count > 0)
                {
                    rate = Convert.ToDecimal(dt.Rows[0][0]);
                }

                se_CurrencyRate.Value = rate;
                se_CurrencyRate.Enabled = true;

            }

            Calculate_CurrencyRate((decimal)se_CurrencyRate.Value);


        }

        protected void se_CurrencyRate_NumberChanged(object sender, EventArgs e)
        {
            Calculate_CurrencyRate((decimal)se_CurrencyRate.Value);
        }

        protected void btn_AllocateExtraCost_Click(object sender, EventArgs e)
        {
        }

        protected void btn_ExtraCostDetail_Click(object sender, EventArgs e)
        {

        }

        // Add Item / PO

        protected void btn_AddItem_Click(object sender, EventArgs e)
        {
            ShowAlert("test");
        }


        // gv_Deatail
        protected void gv_Detail_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem;

                var sql = new Helpers.SQL(hf_ConnStr.Value);
                var query = string.Empty;

                var docDate = de_RecDate.Date;
                var poNo = DataBinder.Eval(e.Row.DataItem, "PoNo") == DBNull.Value ? "" : DataBinder.Eval(e.Row.DataItem, "PoNo").ToString();
                var poDtNo = DataBinder.Eval(e.Row.DataItem, "PoDtNo") == DBNull.Value ? "0" : DataBinder.Eval(e.Row.DataItem, "PoDtNo").ToString();
                var locationCode = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString();
                var productCode = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                var inventoryUnit = DataBinder.Eval(e.Row.DataItem, "InventoryUnit").ToString();
                var unitCode = DataBinder.Eval(dataItem, "UnitCode");
                var orderQty = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "OrderQty"));


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
                if (e.Row.FindControl("lbl_CurrDiscAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_CurrDiscAmt") as Label;

                    label.Text = FormatAmt(DataBinder.Eval(e.Row.DataItem, "CurrDiscAmt"));
                }
                // Net
                if (e.Row.FindControl("lbl_CurrNetAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_CurrNetAmt") as Label;

                    label.Text = FormatAmt(DataBinder.Eval(e.Row.DataItem, "CurrNetAmt"));
                }
                // Tax
                if (e.Row.FindControl("lbl_CurrTaxAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_CurrTaxAmt") as Label;

                    label.Text = FormatAmt(DataBinder.Eval(e.Row.DataItem, "CurrTaxAmt"));
                }

                // Extra Cost
                if (e.Row.FindControl("lbl_ExtCost") != null)
                {
                    var label = e.Row.FindControl("lbl_ExtCost") as Label;

                    label.Text = FormatAmt(DataBinder.Eval(e.Row.DataItem, "ExtraCost"));
                }
                // Total
                if (e.Row.FindControl("lbl_CurrTotalAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_CurrTotalAmt") as Label;

                    label.Text = FormatAmt(DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt"));
                }
                // Total
                if (e.Row.FindControl("lbl_TotalAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_TotalAmt") as Label;

                    label.Text = FormatAmt(DataBinder.Eval(e.Row.DataItem, "TotalAmt"));
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
                #region
                // Base Qty
                if (e.Row.FindControl("lbl_BaseQty") != null)
                {

                    var dt = sql.ExecuteQuery(string.Format("SELECT TOP(1) [Rate] FROM [IN].ProdUnit WHERE UnitType IN ('I','O') AND ProductCode='{0}' AND OrderUnit='{1}' ", productCode, unitCode));
                    var rate = dt != null && dt.Rows.Count > 0 ? Convert.ToDecimal(dt.Rows[0][0]) : 0m;
                    var qty = FormatQty(orderQty * rate);

                    var text = string.Format("{0} {1} (rate : {2})",
                        qty,
                        DataBinder.Eval(e.Row.DataItem, "InventoryUnit").ToString(),
                        rate);

                    var label = e.Row.FindControl("lbl_BaseQty") as Label;

                    label.Text = text;
                }
                #endregion

                #region --Additional Information--

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
                    var dt = sql.ExecuteQuery(string.Format("SELECT ISNULL(SUM([IN]-[OUT]),0) FROM [IN].Inventory WHERE [Location]='{0}' AND ProductCode='{1}' AND CAST(CommittedDate as DATE)<='{2}'",
                        locationCode,
                        productCode,
                        FormatSqlDate(docDate)));

                    var qty = dt != null && dt.Rows.Count > 0 ? Convert.ToDecimal(dt.Rows[0][0]) : 0m;

                    var label = e.Row.FindControl("lbl_Onhand") as Label;

                    label.Text = string.Format("{0} {1} <span style='color:#999999'>*{2}</span>", FormatQty(qty), inventoryUnit, FormatDate(docDate));
                }

                // On order
                if (e.Row.FindControl("lbl_OnOrder") != null)
                {
                    query = @"
SELECT 
	SUM(podt.OrdQty - ISNULL(RcvQty,0)) as Qty
FROM      
	PC.PODt
    JOIN PC.PO 
			ON podt.PoNo=po.PoNo
WHERE   
	po.DocStatus IN ('Printed','Partial')
	AND RcvQty < OrdQty
	AND podt.[Product] = '{0}'";
                    var dt = sql.ExecuteQuery(string.Format(query, productCode));
                    var qty = dt != null && dt.Rows.Count > 0 ? Convert.ToDecimal(dt.Rows[0][0]) : 0m;

                    var label = e.Row.FindControl("lbl_OnOrder") as Label;

                    label.Text = string.Format("{0}", FormatQty(qty));
                }



                #region -- Last vendor /price --
                query = @"
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
                #endregion

                // Last Price
                if (e.Row.FindControl("lbl_LastPrice") != null)
                {
                    var label = e.Row.FindControl("lbl_LastPrice") as Label;

                    label.Text = string.Format("{0} <span style='color:#999999'>*{1}</span>", FormatAmt(lastPrice), lastDocNo);
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

            gv.EditRowStyle.BackColor = Color.FromArgb(254, 249, 231);


            gv.DataSource = dtRecDt;
            gv.EditIndex = e.NewEditIndex;
            gv.DataBind();

            var Img_Btn = gv.Rows[gv.EditIndex].FindControl("Img_Btn") as ImageButton;
            Img_Btn.Visible = false;

            ShowHideColumns(gv, true);
        }

        protected void gv_Detail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            var gv = sender as GridView;

            gv.DataSource = dtRecDt;
            gv.EditIndex = -1;
            gv.DataBind();

            ShowHideColumns(gv, false);
        }




        #endregion

        private void Save()
        {
            var error = ValidateDataBeforeSave();

            if (!string.IsNullOrEmpty(error))
            {
                ShowAlert(error);
                return;
            }


            if (_MODE.ToLower() == "edit")
            {
                // Header


            }
            else // create new one
            {
            }


            

        }

        private void Commit()
        {
        }

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

            return value == null || value == DBNull.Value ? "0.00" : Convert.ToDecimal(value).ToString(f);
        }

        private decimal RoundAmt(decimal value)
        {
            return Math.Round(value, _default.DigitAmt, MidpointRounding.AwayFromZero);
        }

        // ----------------------------------

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
	CASE d.TaxType
		WHEN 'I' THEN 'Include'
		WHEN 'A' THEN 'Add'
		ELSE 'None'
	END as TaxTypeName
FROM 
	PC.RecDt d
	LEFT JOIN [IN].StoreLocation l ON l.LocationCode=d.LocationCode
	LEFT JOIN [IN].Product p ON p.ProductCode=d.ProductCode
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

            // Add some fields to dtRecDt
            // LocationName
            // ProductDesc1
            // ProductDesc2
            // InventoryUnit
            // TaxTypeName
            var locationName = new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "LocationName",
                AllowDBNull = true,
                Unique = false
            };
            var productDesc1 = new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "ProductDesc1",
                AllowDBNull = true,
                Unique = false
            };
            var productDesc2 = new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "ProductDesc2",
                AllowDBNull = true,
                Unique = false
            };

            var InventoryUnit = new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "InventoryUnit",
                AllowDBNull = true,
                Unique = false
            };

            var TaxTypeName = new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "TaxTypeName",
                AllowDBNull = true,
                Unique = false
            };


            dtRecDt.Columns.AddRange(new DataColumn[]
            {
                locationName,
                productDesc1,
                productDesc2,
                InventoryUnit,
                TaxTypeName
            });


            foreach (DataRow dr in dtRecDt.Rows)
            {
                var locationCode = dr["LocationCode"].ToString();
                var productCode = dr["ProductCode"].ToString();

                var sql = new Helpers.SQL(hf_ConnStr.Value);

                var query = "SELECT LocationName FROM [IN].StoreLocation WHERE LocationCode=@code";
                var dt = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("code", locationCode) });

                dr["LocationName"] = dt != null && dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "";

                query = "SELECT ProductDesc1, ProductDesc2, InventoryUnit FROM [IN].Product WHERE ProductCode=@code";
                dt = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("code", productCode) });

                if (dt != null && dt.Rows.Count > 0)
                {
                    dr["ProductDesc1"] = dt.Rows[0]["ProductDesc1"].ToString();
                    dr["ProductDesc2"] = dt.Rows[0]["ProductDesc2"].ToString();
                    dr["InventoryUnit"] = dt.Rows[0]["InventoryUnit"].ToString();
                }
            }
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
            chk_CashConsign.Checked = dr["IsCashConsign"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsCashConsign"]);
            var date = de_RecDate.Date.Date;
            SetCurrencyCode(dr["CurrencyCode"].ToString(), date);
            se_CurrencyRate.Text = dr["CurrencyRate"].ToString();

            txt_Desc.Text = dr["Description"].ToString();

            se_TotalExtraCost.Value = dr["TotalExtraCost"] == DBNull.Value ? 0m : Convert.ToDecimal(dr["TotalExtraCost"]);
            rdb_ExtraCostByAmt.Checked = true;
            rdb_ExtraCostByQty.Checked = true;

            lbl_PoSource.Text = string.IsNullOrEmpty(dr["PoSource"].ToString()) ? "Manually created" : "by Purchase Order";

        }

        private void SetDetails(DataTable dt)
        {
            gv_Detail.DataSource = dtRecDt;
            gv_Detail.DataBind();


        }

        private void ShowHideColumns(GridView gv, bool isEdit)
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
            // Total
            ((DataControlField)gv.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Base")
                .SingleOrDefault()).Visible = !isEdit;
        }

        private void Calculate_CurrencyRate(decimal rate)
        {
            foreach (DataRow dr in dtRecDt.Rows)
            {
                dr["DiccountAmt"] = RoundAmt(Convert.ToDecimal(dr["CurrDiscAmt"]) * rate);
                dr["TaxAmt"] = RoundAmt(Convert.ToDecimal(dr["CurrTaxAmt"]) * rate);
                dr["NetAmt"] = RoundAmt(Convert.ToDecimal(dr["CurrNetAmt"]) * rate);
                dr["TotalAmt"] = RoundAmt(Convert.ToDecimal(dr["CurrTotalAmt"]) * rate);
            }

            SetGrandTotal();
            gv_Detail.DataSource = dtRecDt;
            gv_Detail.DataBind();
        }


        private void SetGrandTotal()
        {
            var currDiscAmt = dtRecDt.AsEnumerable().Select(x => x.Field<decimal>("CurrDiscAmt")).Sum();
            var currNetAmt = dtRecDt.AsEnumerable().Select(x => x.Field<decimal>("CurrNetAmt")).Sum();
            var currTaxAmt = dtRecDt.AsEnumerable().Select(x => x.Field<decimal>("CurrTaxAmt")).Sum();
            var currTotalAmt = dtRecDt.AsEnumerable().Select(x => x.Field<decimal>("CurrTotalAmt")).Sum();

            var discAmt = dtRecDt.AsEnumerable().Select(x => x.Field<decimal>("DiccountAmt")).Sum();
            var netAmt = dtRecDt.AsEnumerable().Select(x => x.Field<decimal>("NetAmt")).Sum();
            var taxAmt = dtRecDt.AsEnumerable().Select(x => x.Field<decimal>("TaxAmt")).Sum();
            var totalAmt = dtRecDt.AsEnumerable().Select(x => x.Field<decimal>("TotalAmt")).Sum();

            lbl_GrandCurrDiscAmt.Text = FormatAmt(currDiscAmt);
            lbl_GrandCurrNetAmt.Text = FormatAmt(currNetAmt);
            lbl_GrandCurrTaxAmt.Text = FormatAmt(currTaxAmt);
            lbl_GrandCurrTotalAmt.Text = FormatAmt(currTotalAmt);

            lbl_GrandDiscAmt.Text = FormatAmt(discAmt);
            lbl_GrandNetAmt.Text = FormatAmt(netAmt);
            lbl_GrandTaxAmt.Text = FormatAmt(taxAmt);
            lbl_GrandTotalAmt.Text = FormatAmt(totalAmt);

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


        private string ValidateDataBeforeSave()
        {
            var message = "";

            if ((decimal)se_CurrencyRate.Value <= 0m)
            {
                return "Invalid currency rate.";
            }



            return message;
        }

        // Popup

        private void ShowAlert(string text)
        {
            lbl_Pop_Alert.Text = text;
            pop_Alert.ShowOnPageLoad = true;
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

        private void SetCurrencyCode(string value, DateTime date)
        {
            var query = @"
;WITH
ex AS(
	SELECT
		DISTINCT CurrencyCode
	FROM
		[REF].CurrencyExchange
)
SELECT
	c.CurrencyCode Code
FROM
	[REF].Currency c
	JOIN ex ON ex.CurrencyCode=c.CurrencyCode 
WHERE
	c.IsActived=1
ORDER BY
	c.CurrencyCode";

            //var dt = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery("SELECT CurrencyCode as [Value], CurrencyCode as [Text] FROM [Ref].Currency WHERE IsActived=1 ORDER BY CurrencyCode");
            var dt = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery(query);

            var items = dt.AsEnumerable()
                .Select(x => new DevExpress.Web.ASPxEditors.ListEditItem
                {
                    Value = x.Field<string>("Code"),
                    Text = x.Field<string>("Code"),
                    Selected = x.Field<string>("Code") == value
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