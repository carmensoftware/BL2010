using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using DevExpress.Web.ASPxEditors;
using BlueLedger.PL.BaseClass;
using System.Text;
using System.Drawing;
using System.Collections.Generic;


namespace BlueLedger.PL.PC.CN
{
    public partial class CnEdit : BasePage
    {
        private readonly string IMG_CHECK = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAAsQAAALEBxi1JjQAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAALwSURBVEiJ3ZJvSF11GMc/z7l6vf7h7rQ/uWyTxty1si1liZY0IsS9qbmxEbIXMoa0migX92KrER1mBAtKi7A0Ru8TJjpW4JUmbOKcgm4yURizwXJpzjul2bp6z9OL7r3zunsPvfBNPa/O7/t8n8/3Ob9z4L9esqa0L8tzQapQBJf8QF33pLFm8KaK/aiMopwFManrnoS1eAPLMjD7GoEPIrxP8QdOR9tJAzqCTYUI7whShOomgT8Qxghr1/D6hW5LLBvrjRRM9znQ6sjYOfyBmpWcJwIuzny2eSnV3QJ6IPnaMjy7uFhX037pBBD19fLA3IvVHkoa0LnQnK9hDQBbk8Ph0fIyjT1D9s3pucg31AkeUcKpnvknVok+dASbTIFBIM8JvhxWPvl5kJGp2QhAQmrYpdT3DCfyx/4iUU4DeQ9DS9y4dz8h3Fbl88sjMTjA4aIdU1oXGEm2kAFwYao1A+H9xdAyVmCQj7uv0TZwk1A4HGf+fmic/jv3YueSrdkc2rX9uY7gV2WOAUueP98EMtNSDBBFUX4cv0ND1xVuzf5zrb23f+XC2GRscEOGh9rXdiIILuy3HQNcoj4Al2HgLyvEk+IC4O7CQ07+1M93A2O09I0+HhKhYU8hXo8bABV8jgGKZEWFZ9dlcrT4hZghbNtcHP+FkG3HtMqCbRRkr4+dFbyOAaL2zEqxwpdLSW52woFnvBlUFe6I00TkN8cARAdWN2pf3cm6dHc8COG9kpdIc7nidFW96hiwz2wYQZlY2fB63NQUvxhnLvdt4eWcjasZIdt2dTgGiIiKIR+tbr6+LYfSyFU95UnjyO7nEyD024Mb6u86BgBUmv52UdpWG45Hrqr6lXwy3alxPYHr6X+FP0wGj3geV6u2pm4OLn6twrsr9Ynfg/g2mUi8vU9S5WBlln/6XwdE6/xc81uGcAa0KEF7CuTstJn+zTE5tuQETxoQrc75L/LssBQbok8rRlANe/y6d2HIEst2mvt/1d8yOfhIPNVLhwAAAABJRU5ErkJggg==";

        private readonly int CN_QTY_INDEX = 0;
        private readonly int CN_AMT_INDEX = 1;


        #region "Attribute"

        private readonly Blue.BL.dbo.Bu _bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.APP.Config _config = new Blue.BL.APP.Config();
        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();

        #endregion

        #region --URL Parameters--

        protected string _BuCode { get { return Request.Params["BuCode"].ToString() ?? ""; } }
        protected string _VID { get { return Request.Params["VID"] == null ? "" : Request.Params["VID"].ToString() ?? ""; } }
        protected string _ID { get { return Request.Params["ID"] == null ? "" : Request.Params["ID"].ToString() ?? ""; } }
        protected string _MODE { get { return Request.Params["Mode"] == null ? "" : Request.Params["Mode"].ToString().ToUpper(); } }

        #endregion

        protected DefaultValues _default
        {
            get { return ViewState["DefaultValues"] as DefaultValues; }
            set { ViewState["DefaultValues"] = value; }
        }

        protected DataTable _dtCn
        {
            //get { return ViewState["dtRec"] as DataTable; }
            //set { ViewState["dtRec"] = value; }
            get { return ViewState["_dtCn" + _ID] as DataTable; }
            set { ViewState["_dtCn" + _ID] = value; }

        }

        protected DataTable _dtCnDt
        {
            get { return ViewState["_dtCnDt" + _ID] as DataTable; }
            set { ViewState["_dtCnDt" + _ID] = value; }
        }

        protected DataTable _dtRecNo
        {
            get { return ViewState["_dtRecNo" + _ID] as DataTable; }
            set { ViewState["_dtRecNo" + _ID] = value; }
        }

        protected DataTable _dtRecLoc
        {
            get { return ViewState["_dtRecLoc" + _ID] as DataTable; }
            set { ViewState["_dtRecLoc" + _ID] = value; }
        }

        protected DataTable _dtRecPrd
        {
            get { return ViewState["_dtRecPrd" + _ID] as DataTable; }
            set { ViewState["_dtRecPrd" + _ID] = value; }
        }

        // Event(s)

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_ConnStr.Value = LoginInfo.ConnStr;

            var currency = _config.GetValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value);
            var digitAmt = _config.GetValue("APP", "Default", "DigitAmt", hf_ConnStr.Value);
            var digitQty = _config.GetValue("APP", "Default", "DigitQty", hf_ConnStr.Value);
            var taxRate = _config.GetValue("APP", "Default", "TaxRate", hf_ConnStr.Value);
            var costMethod = _config.GetValue("IN", "SYS", "COST", hf_ConnStr.Value);

            _default = new DefaultValues
            {
                Currency = currency,
                DigitAmt = string.IsNullOrEmpty(digitAmt) ? 2 : Convert.ToInt32(digitAmt),
                DigitQty = string.IsNullOrEmpty(digitQty) ? 2 : Convert.ToInt32(digitQty),
                TaxRate = string.IsNullOrEmpty(taxRate) ? 0 : Convert.ToDecimal(taxRate),
                CostMethod = costMethod.ToUpper()
            };

        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
            }

        }

        private void Page_Retrieve()
        {
            ddl_Currency.Items.Clear();
            ddl_Currency.Items.AddRange(GetSelect_CurrencyRate(_default.Currency, DateTime.Today).ToArray());

            ddl_Vendor.Items.Clear();
            ddl_Vendor.Items.AddRange(GetSelect_Vendor().ToArray());

            _dtCn = _bu.DbExecuteQuery("SELECT * FROM PC.Cn WHERE CnNo=@id", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@id", _ID) }, hf_ConnStr.Value);

            if (_MODE == "NEW")
            {
                var drNew = _dtCn.NewRow();

                drNew["DocDate"] = DateTime.Today;
                drNew["CnDate"] = DateTime.Today;
                drNew["VendorCode"] = "";
                drNew["CurrencyCode"] = _default.Currency;
                drNew["ExRateAudit"] = 1;
                drNew["DocStatus"] = "New";
                drNew["ExportStatus"] = false;
                drNew["CreatedBy"] = LoginInfo.LoginName;
                drNew["UpdatedBy"] = LoginInfo.LoginName;

                _dtCn.Rows.Add(drNew);

            }

            var dr = _dtCn.Rows[0];

            lbl_CnNo.Text = dr["CnNo"].ToString();
            de_CnDate.Date = Convert.ToDateTime(dr["CnDate"]);
            txt_DocNo.Text = dr["DocNo"].ToString();
            de_DocDate.Date = Convert.ToDateTime(dr["DocDate"]);


            ddl_Vendor.Value = dr["VendorCode"].ToString();
            ddl_Currency.Value = dr["CurrencyCode"].ToString();
            se_CurrencyRate.Value = Convert.ToDecimal(dr["ExRateAudit"]);
            lbl_Status.Text = dr["DocStatus"].ToString();

            txt_Desc.Text = dr["Description"].ToString();

            var query = "SELECT * FROM PC.CnDt WHERE CnNo=@id";
            _dtCnDt = _bu.DbExecuteQuery(query, new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@id", _ID) }, hf_ConnStr.Value);


            ddl_Vendor.Enabled = _dtCnDt.Rows.Count == 0;

            gv_Detail.DataSource = _dtCnDt;
            gv_Detail.DataBind();

        }

        #region --Event(s)--

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            var MODE = Request.QueryString["MODE"];

            if (MODE.ToUpper() == "EDIT")
            {
                Response.Redirect("Cn.aspx?ID=" + lbl_CnNo.Text + "&BuCode=" + LoginInfo.BuInfo.BuCode);
            }
            else
            {
                Response.Redirect("CnList.aspx");
            }

        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            SaveAndCommit(false);
        }

        protected void btn_Commit_Click(object sender, EventArgs e)
        {
            SaveAndCommit(true);
        }


        // Header
        protected void ddl_Vendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;
            var code = ddl.Value;

            _dtCn.Rows[0]["VendorCode"] = code;
        }

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

                var date = de_CnDate.Date.Date;
                var query = "SELECT TOP(1) CurrencyRate FROM [Ref].[CurrencyExchange] WHERE CurrencyCode=@code AND CAST(UpdatedDate AS DATE) <= CAST(@date as DATE) ORDER BY UpdatedDate DESC";

                var dt = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery(query, new SqlParameter[]
            {
                new SqlParameter("Code", code),
                new SqlParameter("Date", date)
            });

                var rate = 0m;

                if (dt != null && dt.Rows.Count > 0)
                {
                    rate = Convert.ToDecimal(dt.Rows[0][0]);
                }

                se_CurrencyRate.Value = rate;
                se_CurrencyRate.Enabled = true;

            }

            UpdateCurrencyRate((decimal)se_CurrencyRate.Value);
        }

        protected void se_CurrencyRate_NumberChanged(object sender, EventArgs e)
        {
            var item = sender as ASPxSpinEdit;
            var rate = Convert.ToDecimal(item.Value);

            UpdateCurrencyRate(rate);
        }

        // Detail
        protected void btn_AddItem_Click(object sender, EventArgs e)
        {
            var vendorCode = ddl_Vendor.Value == null ? "" : ddl_Vendor.Value.ToString();

            if (string.IsNullOrEmpty(vendorCode))
            {
                ShowWarning("Please select a vendor");

                return;
            }

            ddl_Receiving.SelectedIndex = -1;
            lbl_RecNo.Text = "";
            lbl_RecDate.Text = "";
            lbl_RecInvNo.Text = "";
            lbl_RecInvDate.Text = "";
            lbl_RecDesc.Text = "";
            gv_Receiving.DataSource = null;
            gv_Receiving.DataBind();

            pop_AddItem.ShowOnPageLoad = true;
        }

        protected void btn_DeleteItem_Click(object sender, EventArgs e)
        {
        }


        // gv_Deatail

        protected void gv_Detail_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem;

                // CnType
                var cnType = DataBinder.Eval(dataItem, "CnType").ToString();
                BindGridRow_Label(e, "lbl_CnType", cnType == "Q" ? "Quantity" : "Amount");


                // Receiving
                var recNo = DataBinder.Eval(dataItem, "RecNo").ToString();

                BindGridRow_Label(e, "lbl_Receiving", recNo);

                // Location
                var location = DataBinder.Eval(dataItem, "Location").ToString();
                BindGridRow_Label(e, "lbl_Location", string.Format("{0} : {1}", location, GetLocationName(location)));

                if (e.Row.FindControl("hf_LocationCode") != null)
                {
                    var hf = e.Row.FindControl("hf_LocationCode") as HiddenField;

                    hf.Value = location;
                }


                // Product
                var productCode = DataBinder.Eval(dataItem, "ProductCode").ToString();

                BindGridRow_Label(e, "lbl_Product", string.Format("{0} : {1}", productCode, GetProductName(productCode)));

                if (e.Row.FindControl("hf_ProductCode") != null)
                {
                    var hf = e.Row.FindControl("hf_ProductCode") as HiddenField;

                    hf.Value = productCode;
                }

                // Qty
                var recQty = DataBinder.Eval(dataItem, "RecQty");
                BindGridRow_Label(e, "lbl_RecQty", FormatQty(recQty));
                BindGridRow_SpinEdit(e, "se_RecQty", recQty, _default.DigitQty);

                // Unit
                var unitCode = DataBinder.Eval(dataItem, "UnitCode").ToString();
                BindGridRow_Label(e, "lbl_Unit", unitCode);


                var currencyRate = se_CurrencyRate.Number;

                // CurrNetAmt
                var value = DataBinder.Eval(dataItem, "CurrNetAmt");
                BindGridRow_Label(e, "lbl_CurrNetAmt1", FormatAmt(value));
                BindGridRow_Label(e, "lbl_CurrNetAmt", FormatAmt(value));
                BindGridRow_SpinEdit(e, "se_CnCurrNetAmt", value, _default.DigitAmt);

                value = DataBinder.Eval(dataItem, "NetAmt");
                BindGridRow_Label(e, "lbl_NetAmt", FormatAmt(value));


                // CurrTaxAmt
                value = DataBinder.Eval(dataItem, "CurrTaxAmt");
                BindGridRow_Label(e, "lbl_CurrTaxAmt1", FormatAmt(value));
                BindGridRow_Label(e, "lbl_CurrTaxAmt", FormatAmt(value));
                BindGridRow_SpinEdit(e, "se_CnCurrTaxAmt", value, _default.DigitAmt);

                value = DataBinder.Eval(dataItem, "TaxAmt");
                BindGridRow_Label(e, "lbl_TaxAmt", FormatAmt(value));

                // CurrTotalAmt
                value = DataBinder.Eval(dataItem, "CurrTotalAmt");
                BindGridRow_Label(e, "lbl_CurrTotalAmt1", FormatAmt(value));
                BindGridRow_Label(e, "lbl_CurrTotalAmt", FormatAmt(value));
                BindGridRow_SpinEdit(e, "se_CnCurrTotalAmt", value, _default.DigitAmt);

                value = DataBinder.Eval(dataItem, "TotalAmt");
                BindGridRow_Label(e, "lbl_TotalAmt", FormatAmt(value));



                // Extend Information
                var recDtNo = string.IsNullOrEmpty(DataBinder.Eval(dataItem, "PoDtNo").ToString()) ? 0 : Convert.ToInt32(DataBinder.Eval(dataItem, "PoDtNo"));

                var drRec = GetRecDt(recNo, recDtNo, productCode);

                var taxType = drRec["TaxType"].ToString();
                var taxTypeName = "None";

                switch (taxType.ToUpper())
                {
                    case "I": taxTypeName = "Included";
                        break;
                    case "A": taxTypeName = "Addeds";
                        break;
                }

                BindGridRow_Label(e, "lbl_RecDate", drRec == null ? "" : FormatDate(drRec["RecDate"]));
                BindGridRow_Label(e, "lbl_RecTaxType", taxTypeName);
                BindGridRow_Label(e, "lbl_RecTaxRate", FormatAmt(drRec["TaxRate"]));
                BindGridRow_Label(e, "lbl_RecPrice", FormatAmt(drRec["Price"]));

            }
        }

        protected void gv_Detail_RowEditing(object sender, GridViewEditEventArgs e)
        {
            var gv = sender as GridView;

            gv.EditRowStyle.BackColor = Color.FromArgb(254, 249, 231);
            gv.EditIndex = e.NewEditIndex;
            gv.DataSource = _dtCnDt;
            gv.DataBind();

            var row = gv.Rows[e.NewEditIndex];

            var lbl_CnType = row.FindControl("lbl_CnType") as Label;
            var se_RecQty = row.FindControl("se_RecQty") as ASPxSpinEdit;
            var se_CnCurrNetAmt = row.FindControl("se_CnCurrNetAmt") as ASPxSpinEdit;
            var se_CnCurrTaxAmt = row.FindControl("se_CnCurrTaxAmt") as ASPxSpinEdit;
            var se_CnCurrTotalAmt = row.FindControl("se_CnCurrTotalAmt") as ASPxSpinEdit;



            var cnType = lbl_CnType.Text.Trim();

            se_RecQty.Visible = cnType.StartsWith("Q");

            se_CnCurrNetAmt.Visible = cnType.StartsWith("A");
            se_CnCurrTaxAmt.Visible = cnType.StartsWith("A");
            se_CnCurrTotalAmt.Visible = cnType.StartsWith("A");
        }

        protected void gv_Detail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            var gv = sender as GridView;

            gv.DataSource = _dtCnDt;
            gv.EditIndex = -1;
            gv.DataBind();

            //SetEditItem(false);
        }

        protected void gv_Detail_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var gv = sender as GridView;
            var row = gv.Rows[e.RowIndex];

            DataRow dr = _dtCnDt.Rows[e.RowIndex];



            gv.EditIndex = -1;
            gv.DataSource = _dtCnDt;
            gv.DataBind();

            //SetEditItem(false);
        }

        protected void gv_Detail_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var gv = sender as GridView;

            //var hf_RecDtNo = gv.Rows[e.RowIndex].FindControl("hf_RecDtNo") as HiddenField;
            //var recDtNo = Convert.ToInt32(hf_RecDtNo.Value);
            //var item = _dtCnDt.AsEnumerable().FirstOrDefault(x => x.Field<int>("CnDtNo") == recDtNo);

            //if (item != null)
            //    item.Delete();

            gv.DataSource = _dtCnDt;
            gv.DataBind();
        }


        // gv_Receiving
        private DataTable GetReceivingList(string vendorCode, string currencyCode, int month)
        {
            var toDate = de_DocDate.Date;
            var frDate = new DateTime(toDate.Year, toDate.Month, 1).AddMonths(month * (-1));

            var query = @"
SELECT
	RecNo,
	 CONVERT(VARCHAR(10), RecDate, 103)  as RecDate,
	[Description]
FROM
	PC.REC
WHERE
	DocStatus = 'Committed'
    AND CurrencyCode = @CurrencyCode
	AND VendorCode=@VendorCode
";
            if (month < 0)
                query += string.Format(" AND RecDate <= '{0}'", toDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            else
                query += string.Format(" AND RecDate BETWEEN '{0}' AND '{1}'", frDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), toDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

            query += @" ORDER BY RecNo";

            //lbl_Title.Text = query;

            var parameters = new Blue.DAL.DbParameter[]
            {
                new Blue.DAL.DbParameter("@VendorCode", vendorCode),
                new Blue.DAL.DbParameter("@CurrencyCode", currencyCode),
            };
            var dt = _bu.DbExecuteQuery(query, parameters, hf_ConnStr.Value);


            return dt;
        }



        protected void ddl_Receiving_Load(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;

            var currencyCode = ddl_Currency.Value.ToString();
            var vendorCode = ddl_Vendor.Value.ToString();
            var month = Convert.ToInt32(ddl_RecPeriod.Value);

            var dt = GetReceivingList(vendorCode, currencyCode, month);
            
            ddl.ValueField = "RecNo";
            ddl.DataSource = dt;
            ddl.DataBind();

            
            //            var docDate = de_DocDate.Date;

//            var query = @"
//SELECT
//	RecNo,
//	 CONVERT(VARCHAR(10), RecDate, 103)  as RecDate,
//	[Description]
//FROM
//	PC.REC
//WHERE
//	DocStatus = 'Committed'
//    AND CurrencyCode = @CurrencyCode
//	AND VendorCode=@VendorCode
//	AND RecDate <= @DocDate
//ORDER BY
//	RecNo
//";
//            var parameters = new Blue.DAL.DbParameter[]
//            {
//                new Blue.DAL.DbParameter("@VendorCode", vendorCode),
//                new Blue.DAL.DbParameter("@CurrencyCode", currencyCode),
//                new Blue.DAL.DbParameter("@DocDate", docDate.ToString("yyyy-MM-dd"))
//            };
//            var dt = _bu.DbExecuteQuery(query, parameters, hf_ConnStr.Value);


            //ddl.ValueField = "RecNo";
            //ddl.DataSource = dt;
            //ddl.DataBind();

        }

        protected void ddl_Receiving_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;
            var recNo = ddl.Value.ToString();

            var dtRecDt = GetReceivingDetails(recNo);
            gv_Receiving.DataSource = dtRecDt;
            gv_Receiving.DataBind();

        }

        protected void ddl_RecPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {

            var currencyCode = ddl_Currency.Value.ToString();
            var vendorCode = ddl_Vendor.Value.ToString();
            var month = Convert.ToInt32(ddl_RecPeriod.Value);

            var dt = GetReceivingList(vendorCode, currencyCode, month);

            var ddl = ddl_Receiving;
            ddl.ValueField = "RecNo";
            ddl.DataSource = dt;
            ddl.DataBind();
        }

        protected void gv_Receiving_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem;

                // RecDtNo
                var recDtNo = DataBinder.Eval(dataItem, "RecDtNo").ToString();
                BindGridRow_Label(e, "lbl_RecDtNo", recDtNo);

                // Location
                var location = DataBinder.Eval(dataItem, "LocationCode").ToString();
                BindGridRow_Label(e, "lbl_Location", string.Format("{0} : {1}", location, GetLocationName(location)));

                // Product
                var productCode = DataBinder.Eval(dataItem, "ProductCode").ToString();

                BindGridRow_Label(e, "lbl_Product", string.Format("{0} : {1}", productCode, GetProductName(productCode)));

                // Price
                var price = DataBinder.Eval(dataItem, "Price").ToString();
                BindGridRow_Label(e, "lbl_Price", FormatAmt(price));

                // RcvUnit
                var unitCode = DataBinder.Eval(dataItem, "RcvUnit").ToString();
                BindGridRow_Label(e, "lbl_RcvUnit", unitCode);

                // RecQty
                var recQty = DataBinder.Eval(dataItem, "RecQty");
                BindGridRow_Label(e, "lbl_RecQty", FormatQty(recQty));


                // FocQty
                var focQty = DataBinder.Eval(dataItem, "FocQty");
                BindGridRow_Label(e, "lbl_FocQty", FormatQty(focQty));


                var currencyRate = se_CurrencyRate.Number;

                // CurrNetAmt
                var value = DataBinder.Eval(dataItem, "CurrNetAmt");
                BindGridRow_Label(e, "lbl_CurrNetAmt", FormatAmt(value));

                value = DataBinder.Eval(dataItem, "NetAmt");
                BindGridRow_Label(e, "lbl_NetAmt", FormatAmt(value));

                // CurrTaxAmt
                value = DataBinder.Eval(dataItem, "CurrTaxAmt");
                BindGridRow_Label(e, "lbl_CurrTaxAmt", FormatAmt(value));

                value = DataBinder.Eval(dataItem, "TaxAmt");
                BindGridRow_Label(e, "lbl_TaxAmt", FormatAmt(value));

                // CurrTotalAmt
                value = DataBinder.Eval(dataItem, "CurrTotalAmt");
                BindGridRow_Label(e, "lbl_CurrTotalAmt", FormatAmt(value));

                value = DataBinder.Eval(dataItem, "TotalAmt");
                BindGridRow_Label(e, "lbl_TotalAmt", FormatAmt(value));

                // CnType
                var cnType = DataBinder.Eval(dataItem, "CnType").ToString();

                if (cnType != "N")
                {
                    if (e.Row.FindControl("img_Check") != null)
                    {
                        var img = e.Row.FindControl("img_Check") as System.Web.UI.WebControls.Image;

                        img.ImageUrl = IMG_CHECK;
                        img.Visible = true;

                    }
                }


                if (e.Row.FindControl("ddl_CnType") != null)
                {
                    var dll = e.Row.FindControl("ddl_CnType") as ASPxComboBox;

                    dll.Value = cnType;
                }


                if (e.Row.FindControl("se_CnQty") != null)
                {
                    var se = e.Row.FindControl("se_CnQty") as ASPxSpinEdit;
                    var maxValue = Convert.ToDecimal(recQty);

                    se.DecimalPlaces = _default.DigitQty;
                    se.MaxValue = maxValue;

                    se.Value = DataBinder.Eval(dataItem, "CnQty");
                    se.Visible = cnType == "Q";

                }


                if (e.Row.FindControl("se_cnFoc") != null)
                {
                    var se = e.Row.FindControl("se_cnFoc") as ASPxSpinEdit;
                    var maxValue = Convert.ToDecimal(focQty);

                    se.DecimalPlaces = _default.DigitQty;
                    se.MaxValue = maxValue;

                    se.Value = DataBinder.Eval(dataItem, "CnFoc");
                    se.Visible = cnType == "Q" && maxValue > 0;
                }



                if (e.Row.FindControl("se_CnCurrNetAmt") != null)
                {
                    var se = e.Row.FindControl("se_CnCurrNetAmt") as ASPxSpinEdit;

                    se.Value = DataBinder.Eval(dataItem, "CnCurrNetAmt");
                    se.DecimalPlaces = _default.DigitAmt;
                    se.Visible = cnType == "A";

                }

                if (e.Row.FindControl("se_CnCurrTaxAmt") != null)
                {
                    var se = e.Row.FindControl("se_CnCurrTaxAmt") as ASPxSpinEdit;

                    se.Value = DataBinder.Eval(dataItem, "CnCurrTaxAmt");
                    se.DecimalPlaces = _default.DigitAmt;
                    se.Visible = cnType == "A";

                }

                if (e.Row.FindControl("se_CnCurrTotalAmt") != null)
                {
                    var se = e.Row.FindControl("se_CnCurrTotalAmt") as ASPxSpinEdit;

                    se.Value = DataBinder.Eval(dataItem, "CnCurrTotalAmt");
                    se.DecimalPlaces = _default.DigitAmt;
                    se.Visible = cnType == "A";

                }



                // Hidden Fields


                if (e.Row.FindControl("hf_LocationCode") != null)
                {
                    (e.Row.FindControl("hf_LocationCode") as HiddenField).Value = DataBinder.Eval(dataItem, "LocationCode").ToString();
                }

                if (e.Row.FindControl("hf_ProductCode") != null)
                {
                    (e.Row.FindControl("hf_ProductCode") as HiddenField).Value = DataBinder.Eval(dataItem, "ProductCode").ToString();
                }

                if (e.Row.FindControl("hf_RcvUnit") != null)
                {
                    (e.Row.FindControl("hf_RcvUnit") as HiddenField).Value = DataBinder.Eval(dataItem, "RcvUnit").ToString();
                }

                if (e.Row.FindControl("hf_Price") != null)
                {
                    (e.Row.FindControl("hf_Price") as HiddenField).Value = DataBinder.Eval(dataItem, "Price").ToString();
                }

                if (e.Row.FindControl("hf_TaxType") != null)
                {
                    (e.Row.FindControl("hf_TaxType") as HiddenField).Value = DataBinder.Eval(dataItem, "TaxType").ToString();
                }

                if (e.Row.FindControl("hf_TaxRate") != null)
                {
                    (e.Row.FindControl("hf_TaxRate") as HiddenField).Value = DataBinder.Eval(dataItem, "TaxRate").ToString();
                }

                if (e.Row.FindControl("hf_NetAmt") != null)
                {
                    (e.Row.FindControl("hf_NetAmt") as HiddenField).Value = DataBinder.Eval(dataItem, "NetAmt").ToString();
                }

                if (e.Row.FindControl("hf_TaxAmt") != null)
                {
                    (e.Row.FindControl("hf_TaxAmt") as HiddenField).Value = DataBinder.Eval(dataItem, "TaxAmt").ToString();
                }

                if (e.Row.FindControl("hf_TotalAmt") != null)
                {
                    (e.Row.FindControl("hf_TotalAmt") as HiddenField).Value = DataBinder.Eval(dataItem, "TotalAmt").ToString();
                }

                if (e.Row.FindControl("hf_CnDtNo") != null)
                {
                    (e.Row.FindControl("hf_CnDtNo") as HiddenField).Value = DataBinder.Eval(dataItem, "CnDtNo").ToString();
                }
            }
        }

        protected void ddl_CnType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;
            var se_CnQty = ddl.NamingContainer.FindControl("se_CnQty") as ASPxSpinEdit;
            var se_CnFoc = ddl.NamingContainer.FindControl("se_CnFoc") as ASPxSpinEdit;

            var se_CnCurrNetAmt = ddl.NamingContainer.FindControl("se_CnCurrNetAmt") as ASPxSpinEdit;
            var se_CnCurrTaxAmt = ddl.NamingContainer.FindControl("se_CnCurrTaxAmt") as ASPxSpinEdit;
            var se_CnCurrTotalAmt = ddl.NamingContainer.FindControl("se_CnCurrTotalAmt") as ASPxSpinEdit;

            var cnType = ddl.Value.ToString();

            se_CnQty.Visible = false;
            se_CnFoc.Visible = false;
            se_CnCurrNetAmt.Visible = false;
            se_CnCurrTaxAmt.Visible = false;
            se_CnCurrTotalAmt.Visible = false;

            se_CnQty.Visible = cnType.StartsWith("Q") && se_CnQty.MaxValue > 0;
            se_CnFoc.Visible = cnType.StartsWith("Q") && se_CnFoc.MaxValue > 0;

            se_CnCurrNetAmt.Visible = cnType.StartsWith("A");
            se_CnCurrTaxAmt.Visible = cnType.StartsWith("A");
            se_CnCurrTotalAmt.Visible = cnType.StartsWith("A");

            se_CnCurrTotalAmt.Number = se_CnCurrNetAmt.Number + se_CnCurrTaxAmt.Number;

            if (ddl.NamingContainer.FindControl("img_Check") != null)
            {
                var img = ddl.NamingContainer.FindControl("img_Check") as System.Web.UI.WebControls.Image;

                img.ImageUrl = cnType == "N" ? "" : IMG_CHECK;
                img.Visible = cnType != "N";
            }
        }

        protected void se_CnCurrNetAmt_NumberChanged(object sender, EventArgs e)
        {
            var se_CnCurrNetAmt = (sender as ASPxSpinEdit).NamingContainer.FindControl("se_CnCurrNetAmt") as ASPxSpinEdit;
            var se_CnCurrTaxAmt = (sender as ASPxSpinEdit).NamingContainer.FindControl("se_CnCurrTaxAmt") as ASPxSpinEdit;
            var se_CnCurrTotalAmt = (sender as ASPxSpinEdit).NamingContainer.FindControl("se_CnCurrTotalAmt") as ASPxSpinEdit;

            se_CnCurrTotalAmt.Number = se_CnCurrNetAmt.Number + se_CnCurrTaxAmt.Number;
        }

        protected void se_CnCurrTaxAmt_NumberChanged(object sender, EventArgs e)
        {
            se_CnCurrNetAmt_NumberChanged(sender, e);
        }

        protected void btn_SelectItems_Click(object sender, EventArgs e)
        {
            // Check invalid value (qty/amt)
            foreach (GridViewRow row in gv_Receiving.Rows)
            {
                var lbl_RecDtNo = row.FindControl("lbl_RecDtNo") as Label;
                var ddl_CnType = row.FindControl("ddl_CnType") as ASPxComboBox;


                var recDtNo = lbl_RecDtNo.Text;
                var cnType = ddl_CnType.Value.ToString();



                if (cnType.StartsWith("Q"))
                {

                    var se_CnQty = row.FindControl("se_CnQty") as ASPxSpinEdit;
                    //var se_CnFoc = row.FindControl("se_CnFoc") as ASPxSpinEdit;

                    if (se_CnQty.Number == 0m)
                    {
                        ShowWarning(string.Format("Quantity, is not set at item #{0}.", recDtNo));
                        return;
                    }

                }
                else if (cnType.StartsWith("A"))
                {
                    var se_CnCurrNetAmt = row.FindControl("se_CnCurrNetAmt") as ASPxSpinEdit;

                    if (se_CnCurrNetAmt.Number == 0m)
                    {
                        ShowWarning(string.Format("Net amount is required at item #{0}.", recDtNo));
                        return;
                    }
                }
            }

            foreach (GridViewRow row in gv_Receiving.Rows)
            {
                var hf_CnDtNo = row.FindControl("hf_CnDtNo") as HiddenField;
                var ddl_CnType = row.FindControl("ddl_CnType") as ASPxComboBox;
                var cnType = ddl_CnType.Value.ToString();

                var lbl_RecDtNo = row.FindControl("lbl_RecDtNo") as Label;

                var hf_LocationCode = row.FindControl("hf_LocationCode") as HiddenField;
                var hf_ProductCode = row.FindControl("hf_ProductCode") as HiddenField;
                var hf_RcvUnit = row.FindControl("hf_RcvUnit") as HiddenField;
                var hf_Price = row.FindControl("hf_Price") as HiddenField;
                var hf_TaxType = row.FindControl("hf_TaxType") as HiddenField;
                var hf_TaxRate = row.FindControl("hf_TaxRate") as HiddenField;
                var hf_NetAmt = row.FindControl("hf_NetAmt") as HiddenField;
                var hf_TaxAmt = row.FindControl("hf_TaxAmt") as HiddenField;
                var hf_TotalAmt = row.FindControl("hf_TotalAmt") as HiddenField;

                var se_CnQty = row.FindControl("se_CnQty") as ASPxSpinEdit;
                var se_CnFoc = row.FindControl("se_CnFoc") as ASPxSpinEdit;

                var se_CnCurrNetAmt = row.FindControl("se_CnCurrNetAmt") as ASPxSpinEdit;
                var se_CnCurrTaxAmt = row.FindControl("se_CnCurrTaxAmt") as ASPxSpinEdit;
                var se_CnCurrTotalAmt = row.FindControl("se_CnCurrTotalAmt") as ASPxSpinEdit;

                var recNo = lbl_RecNo.Text.Trim();
                var recDtNo = lbl_RecDtNo.Text;

                var price = Convert.ToDecimal(hf_Price.Value);
                var taxType = hf_TaxType.Value;
                var taxRate = Convert.ToDecimal(hf_TaxRate.Value);

                var cnQty = se_CnQty.Number;
                var cnFoc = se_CnFoc.Number;

                var currNetAmt = se_CnCurrNetAmt.Number;
                var currTaxAmt = se_CnCurrTaxAmt.Number;
                var currTotalAmt = se_CnCurrTotalAmt.Number;
                var comment = "";

                var currencyRate = se_CurrencyRate.Number;


                if (cnType == "Q")
                {
                    var amt = RoundAmt(cnQty * price);

                    if (taxType == "N")
                    {
                        currNetAmt = amt;
                        currTaxAmt = 0;
                        currTotalAmt = amt;
                    }
                    else if (taxType == "A")
                    {

                        currNetAmt = amt;
                        currTaxAmt = RoundAmt(amt * taxRate / 100);
                        currTotalAmt = currNetAmt + currTaxAmt;
                    }
                    else
                    {
                        currTaxAmt = (amt * taxRate) / (100 - taxRate);
                        currNetAmt = amt - currTaxAmt;
                        currTotalAmt = amt;
                    }
                }


                var cnDtNo = Convert.ToInt32(hf_CnDtNo.Value);

                if (cnDtNo > 0)
                {
                    var cnDt = _dtCnDt.AsEnumerable().FirstOrDefault(x => x.Field<int>("CnDtNo") == cnDtNo);

                    if (cnDt != null)
                    {
                        if (cnType == "N")
                            cnDt.Delete();
                        else
                        {
                            cnDt["CnType"] = cnType;

                            cnDt["RecQty"] = cnQty;
                            cnDt["FocQty"] = cnFoc;

                            cnDt["CurrNetAmt"] = currNetAmt;
                            cnDt["CurrTaxAmt"] = currTaxAmt;
                            cnDt["CurrTotalAmt"] = currTotalAmt;
                            cnDt["NetAmt"] = RoundAmt(currNetAmt * currencyRate);
                            cnDt["TaxAmt"] = RoundAmt(currTaxAmt * currencyRate);
                            cnDt["TotalAmt"] = RoundAmt(currTotalAmt * currencyRate);

                        }
                    }

                }
                else if (cnType != "N")
                {

                    var dr = _dtCnDt.NewRow();

                    dr["CnNo"] = lbl_CnNo.Text;
                    dr["CnDtNo"] = 0;
                    dr["CnType"] = cnType;
                    dr["RecNo"] = recNo;
                    dr["Location"] = hf_LocationCode.Value;
                    dr["ProductCode"] = hf_ProductCode.Value;
                    dr["UnitCode"] = hf_RcvUnit.Value;
                    dr["RecQty"] = cnQty;
                    dr["FocQty"] = cnFoc;
                    dr["Price"] = price;
                    dr["TaxAdj"] = 0;
                    dr["TaxType"] = taxType;
                    dr["TaxRate"] = taxRate;
                    dr["CurrNetAmt"] = currNetAmt;
                    dr["CurrTaxAmt"] = currTaxAmt;
                    dr["CurrTotalAmt"] = currTotalAmt;


                    var netAmt = RoundAmt(currNetAmt * currencyRate);
                    var taxAmt = RoundAmt(currTaxAmt * currencyRate);
                    var totalAmt = RoundAmt(currTotalAmt * currencyRate);


                    dr["NetAmt"] = netAmt;
                    dr["TaxAmt"] = taxAmt;
                    dr["TotalAmt"] = totalAmt;
                    dr["Comment"] = comment;

                    // Using for RecNo, RecDtNo
                    dr["PoNo"] = recNo;
                    dr["PoDtNo"] = recDtNo;

                    _dtCnDt.Rows.Add(dr);
                }
            }

            gv_Detail.DataSource = _dtCnDt;
            gv_Detail.DataBind();

            pop_AddItem.ShowOnPageLoad = false;


        }


        #endregion

        // Method(s)

        private void SaveAndCommit(bool isCommit)
        {
            if (ddl_Vendor.SelectedIndex < 0)
            {
                ShowWarning("Vendor is required.");

                return;
            }

            if (se_CurrencyRate.Number <= 0)
            {
                ShowWarning("Invalid currency rate.");

                return;
            }


        }

        private void UpdateCurrencyRate(decimal value)
        {
        }

        private DataTable GetReceivingDetails(string recNo)
        {

            var dtRec = _bu.DbExecuteQuery("SELECT * FROM PC.Rec WHERE RecNo=@RecNo", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@RecNo", recNo) }, hf_ConnStr.Value);

            if (dtRec.Rows.Count > 0)
            {
                var dr = dtRec.Rows[0];

                lbl_RecNo.Text = dr["RecNo"].ToString();
                lbl_RecDate.Text = FormatDate(dr["RecDate"]);
                lbl_RecInvNo.Text = dr["InvoiceNo"].ToString();
                lbl_RecInvDate.Text = FormatDate(dr["InvoiceDate"]);
                lbl_RecDesc.Text = dr["Description"].ToString();
                lbl_RecCurrency.Text = string.Format("{0} @{1}", dr["CurrencyCode"].ToString(), dr["CurrencyRate"].ToString());
            }

            var query = @"
SELECT 
    0 as CnDtNo,
    'N' as CnType,
	CAST(0 as decimal(18,3)) as CnQty, 
	CAST(0 as decimal(18,3)) as CnFoc, 
    CAST(0 as decimal(18,4)) as CnCurrNetAmt, 
	CAST(0 as decimal(18,4)) as CnCurrTaxAmt,
	CAST(0 as decimal(18,4)) as CnCurrTotalAmt,

recdt.*
FROM 
	PC.RecDt 
WHERE 
	RecNo=@RecNo
";

            var dtRecDt = _bu.DbExecuteQuery(query, new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@RecNo", recNo) }, hf_ConnStr.Value);


            //// Set selected item in grid rows

            var cnDtList = _dtCnDt.AsEnumerable()
                        .Where(x => x.Field<string>("RecNo") == recNo)
                        .ToArray();

            foreach (var cndt in cnDtList)
            {
                var recDtNo = cndt.Field<Nullable<int>>("PoDtNo");
                var locationCode = cndt.Field<string>("Location");
                var productCode = cndt.Field<string>("ProductCode");

                var cnDtNo = cndt.Field<int>("CnDtNo");
                var cnType = cndt.Field<string>("CnType");
                var cnQty = cndt.Field<decimal>("RecQty");
                var cnFoc = cndt.Field<decimal>("FocQty");
                var cnCurrNetAmt = cndt.Field<decimal>("CurrNetAmt");
                var cnCurrTaxAmt = cndt.Field<decimal>("CurrTaxAmt");
                var cnCurrTotalAmt = cndt.Field<decimal>("CurrTotalAmt");



                var recItem = recDtNo != null && recDtNo > 0
                    ? dtRecDt.AsEnumerable().FirstOrDefault(x => x.Field<int>("RecDtNo") == recDtNo)
                    : dtRecDt.AsEnumerable().FirstOrDefault(x => x.Field<string>("LocationCode") == locationCode && x.Field<string>("ProductCode") == productCode);

                //var recItem = dt.AsEnumerable().FirstOrDefault(x => x.Field<string>("LocationCode") == locationCode && x.Field<string>("ProductCode") == productCode);

                if (recItem != null)
                {
                    if (cnType == "Q")
                    {
                        recItem["CnDtNo"] = cnDtNo;
                        recItem["CnType"] = "Q";
                        recItem["CnQty"] = cnQty;
                        recItem["CnFoc"] = cnFoc;
                    }
                    else
                    {
                        recItem["CnDtNo"] = cnDtNo;
                        recItem["CnType"] = "A";
                        recItem["CnCurrNetAmt"] = cnCurrNetAmt;
                        recItem["CnCurrTaxAmt"] = cnCurrTaxAmt;
                        recItem["CnCurrTotalAmt"] = cnCurrTotalAmt;
                    }

                }

            }

            return dtRecDt;
        }

        private DataRow GetRecDt(string recNo, int recDtNo, string productCode)
        {
            var query = @"
SELECT TOP(1)
	rec.*,
	recdt.RecDtNo,
	recdt.LocationCode,
	recdt.ProductCode,
	recdt.UnitCode,
	recdt.RecQty,
	recdt.FocQty,
	recdt.RcvUnit,
	recdt.Rate,
	recdt.Price,
	recdt.TaxType,
	recdt.TaxRate,
	recdt.CurrDiscAmt,
	recdt.CurrNetAmt,
	recdt.CurrTaxAmt,
	recdt.CurrTotalAmt,
	recdt.DiccountAmt,
	recdt.NetAmt,
	recdt.TaxAmt,
	recdt.TotalAmt
FROM 
	PC.Rec 
	JOIN PC.RecDt ON rec.RecNo=recdt.RecNo
WHERE 
	rec.RecNo=@RecNo 
	AND recdt.RecDtNo = CASE WHEN @RecDtNo=0 THEN recdt.RecDtNo ELSE @RecDtNo END
	AND recdt.ProductCode = CASE WHEN @RecDtNo=0 THEN @ProductCode ELSE recdt.ProductCode END";

            var dt = _bu.DbExecuteQuery(query,
                new Blue.DAL.DbParameter[] 
                { 
                    new Blue.DAL.DbParameter("@RecNo", recNo), 
                    new Blue.DAL.DbParameter("@RecDtNo", recDtNo.ToString()), 
                    new Blue.DAL.DbParameter("@ProductCode", productCode) 
                },
                hf_ConnStr.Value);

            return dt.Rows.Count == 0 ? null : dt.Rows[0];
        }

        private string GetLocationName(string code)
        {
            var dt = _bu.DbExecuteQuery("SELECT LocationName FROM [IN].StoreLocation WHERE LocationCode=@Code", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@Code", code) }, hf_ConnStr.Value);

            return dt.Rows.Count == 0 ? "" : dt.Rows[0][0].ToString();
        }

        private string GetProductName(string code)
        {
            var dt = _bu.DbExecuteQuery("SELECT ProductDesc1, ProductDesc2 FROM [IN].Product WHERE ProductCode=@Code", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@Code", code) }, hf_ConnStr.Value);

            return dt.Rows.Count == 0 ? "" : string.Format("{0} | {1}", dt.Rows[0][0].ToString(), dt.Rows[0][1].ToString());
        }


        // DbGrid
        private void BindGridRow_DropDownList(ASPxComboBox ddl, string value)
        {
            var item = ddl.Items.FindByValue(value);

            ddl.Value = item == null ? null : value;
        }

        private void BindGridRow_Label(GridViewRowEventArgs e, string itemName, string value)
        {
            if (e.Row.FindControl(itemName) != null)
            {
                var lbl = e.Row.FindControl(itemName) as Label;

                lbl.Text = value;
                lbl.ToolTip = lbl.Text;
            }
        }

        private void BindGridRow_SpinEdit(GridViewRowEventArgs e, string itemName, object value, int digit = 2)
        {
            if (e.Row.FindControl(itemName) != null)
            {
                var item = e.Row.FindControl(itemName) as ASPxSpinEdit;

                item.Value = Convert.ToDecimal(value);
                item.ToolTip = item.Text;
                item.DecimalPlaces = digit;


            }
        }

        // Lookup
        private IEnumerable<ListEditItem> GetSelect_CurrencyRate(string currencyCode, DateTime date)
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

            return items;

            //ddl_Currency.Items.Clear();
            //ddl_Currency.Items.AddRange(items);
        }

        private IEnumerable<ListEditItem> GetSelect_Vendor()
        {
            var query = "SELECT VendorCode as [Value], CONCAT(VendorCode, ' : ', [Name]) as [Text] FROM AP.Vendor WHERE IsActive=1 ORDER BY VendorCode";
            var dt = _bu.DbExecuteQuery(query, null, hf_ConnStr.Value);

            return dt.AsEnumerable()
                .Select(x => new ListEditItem
                {
                    Value = x.Field<string>("Value"),
                    Text = x.Field<string>("Text")
                }).ToArray();

            //ddl_Vendor.Items.Clear();
            //ddl_Vendor.Items.AddRange(dt.AsEnumerable()
            //    .Select(x => new ListEditItem
            //    {
            //        Value = x.Field<string>("Value"),
            //        Text = x.Field<string>("Text")
            //    }).ToArray());
        }


        // Utilities
        private void ShowWarning(string text)
        {
            pop_Alert.HeaderText = "Warning";

            lbl_Pop_Alert.Text = text;
            pop_Alert.ShowOnPageLoad = true;
        }

        private void ShowInfo(string text)
        {
            pop_Alert.HeaderText = "Information";

            lbl_Pop_Alert.Text = text;
            pop_Alert.ShowOnPageLoad = true;
        }

        private string FormatQty(object value)
        {
            var number = string.IsNullOrEmpty(value.ToString()) ? 0m : Convert.ToDecimal(value);

            return number.ToString(string.Format("N{0}", DefaultQtyDigit));
        }

        private string FormatAmt(object value)
        {
            var number = string.IsNullOrEmpty(value.ToString()) ? 0m : Convert.ToDecimal(value);

            return number.ToString(string.Format("N{0}", DefaultAmtDigit));
        }

        private string FormatDate(object value)
        {

            return Convert.ToDateTime(value).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

        }

        private decimal RoundAmt(decimal value)
        {
            return Math.Round(value, Convert.ToInt32(_default.DigitAmt), MidpointRounding.AwayFromZero);
        }

        // Classes
        public class DefaultValues
        {
            public string Currency { get; set; }
            public int DigitAmt { get; set; }
            public int DigitQty { get; set; }
            public decimal TaxRate { get; set; }
            public string CostMethod { get; set; }
        }


    }
}