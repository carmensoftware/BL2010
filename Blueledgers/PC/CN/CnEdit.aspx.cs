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

        private readonly Blue.BL.PC.CN.Cn _cn = new Blue.BL.PC.CN.Cn();

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

            se_CurrencyRate.Enabled = ddl_Currency.Text != _default.Currency;

            lbl_Status.Text = dr["DocStatus"].ToString();

            txt_Desc.Text = dr["Description"].ToString();

            //var query = "SELECT cndt.*, p.ProductDesc1, p.ProductDesc2, p.InventoryUnit FROM PC.CnDt LEFT JOIN [IN].Product p ON p.ProductCode=cndt.ProductCode WHERE CnNo=@id";
            //_dtCnDt = _bu.DbExecuteQuery(query, new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@id", _ID) }, hf_ConnStr.Value);
            _dtCnDt = GetCnDt(_ID);


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

            lbl_AlreadyCreatedReceiving.Text = "";
            pop_AddItem.ShowOnPageLoad = true;
        }

        protected void btn_DeleteItems_Click(object sender, EventArgs e)
        {
            var items = new List<string>();

            foreach (GridViewRow row in gv_Detail.Rows)
            {
                var chk_Item = row.FindControl("chk_Item") as CheckBox;
                var hf_CnDtNo = row.FindControl("hf_CnDtNo") as HiddenField;

                if (chk_Item.Checked)
                {
                    items.Add(hf_CnDtNo.Value);
                }
            }

            if (items.Count == 0)
            {
                ShowWarning("Please select any item.");

                return;
            }

            hf_DeletedItems.Value = string.Join(",", items);

            lbl_DeletedItems.Text = "Do you want to delete the selected items?";

            pop_ConfirmDelete.ShowOnPageLoad = true;

        }

        protected void btn_ConfirmDeleteItems_Click(object sender, EventArgs e)
        {
            var deleledItems = hf_DeletedItems.Value;
            var items = deleledItems.Split(',').AsEnumerable().Select(x => x.Trim()).ToArray();

            foreach (DataRow dr in _dtCnDt.Rows)
            {
                var cnDtNo = dr["CnDtNo"].ToString();

                if (items.Contains(cnDtNo))
                {
                    dr.Delete();
                }
            }

            _dtCnDt.AcceptChanges();


            gv_Detail.DataSource = _dtCnDt;
            gv_Detail.DataBind();

            pop_ConfirmDelete.ShowOnPageLoad = false;
        }

        // gv_Deatail

        protected void gv_Detail_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem;

                // CnDtNo
                if (e.Row.FindControl("hf_CnDtNo") != null)
                {
                    var hf = e.Row.FindControl("hf_CnDtNo") as HiddenField;

                    hf.Value = DataBinder.Eval(dataItem, "CnDtNo").ToString();
                }

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
                var drProduct = GetProductItem(productCode);
                var productDesc1 = drProduct == null ? "" : drProduct["ProductDesc1"].ToString();
                var productDesc2 = drProduct == null ? "" : drProduct["ProductDesc2"].ToString();
                var inventoryUnit = drProduct == null ? "" : drProduct["InventoryUnit"].ToString();

                var productDesc = string.Format("{0} | {1}", productDesc1, productDesc2);

                BindGridRow_Label(e, "lbl_Product", string.Format("{0} : {1}", productCode, productDesc));

                if (e.Row.FindControl("hf_ProductCode") != null)
                {
                    var hf = e.Row.FindControl("hf_ProductCode") as HiddenField;

                    hf.Value = productCode;
                }

                // Unit
                var unitCode = DataBinder.Eval(dataItem, "UnitCode").ToString();

                BindGridRow_Label(e, "lbl_Unit", unitCode);

                var price = Convert.ToDecimal(DataBinder.Eval(dataItem, "Price"));
                var taxType = DataBinder.Eval(dataItem, "TaxType").ToString();
                var taxRate = Convert.ToDecimal(DataBinder.Eval(dataItem, "TaxRate"));


                var recDtNo = string.IsNullOrEmpty(DataBinder.Eval(dataItem, "PoDtNo").ToString()) ? 0 : Convert.ToInt32(DataBinder.Eval(dataItem, "PoDtNo"));
                var drRec = GetRecDtItem(recNo, recDtNo, productCode, location);

                var originRecQty = drRec == null ? 0m : Convert.ToDecimal(drRec["RecQty"]);
                var originFocQty = drRec == null ? 0m : Convert.ToDecimal(drRec["FocQty"]);
                var originCurrNetAmt = drRec == null ? 0m : Convert.ToDecimal(drRec["CurrNetAmt"]);
                var originCurrTaxAmt = drRec == null ? 0m : Convert.ToDecimal(drRec["CurrTaxAmt"]);

                var rcvUnit = drRec == null ? "" : drRec["RcvUnit"].ToString();
                var rate = drRec == null ? 0m : Convert.ToDecimal(drRec["Rate"]);

                BindGridRow_HiddenField(e, "hf_RcvUnit", rcvUnit);
                BindGridRow_HiddenField(e, "hf_RecQty", originRecQty.ToString());
                BindGridRow_HiddenField(e, "hf_Rate", rate);
                BindGridRow_HiddenField(e, "hf_Price", price);
                BindGridRow_HiddenField(e, "hf_TaxRate", taxRate);
                BindGridRow_HiddenField(e, "hf_TaxType", taxType);
                BindGridRow_HiddenField(e, "hf_NetAmt", originCurrNetAmt);
                BindGridRow_HiddenField(e, "hf_TaxAmt", originCurrTaxAmt);

                BindGridRow_HiddenField(e, "hf_OriginNetAmt", originCurrNetAmt);
                BindGridRow_HiddenField(e, "hf_OriginTaxAmt", originCurrTaxAmt);



                if (rcvUnit != unitCode)
                {
                    originRecQty = RoundQty(originRecQty * rate);
                }
                // Qty
                var recQty = DataBinder.Eval(dataItem, "RecQty");
                BindGridRow_Label(e, "lbl_RecQty", FormatQty(recQty));
                BindGridRow_SpinEdit(e, "se_CnQty", recQty, originRecQty, _default.DigitQty);


                // Foc
                var focQty = DataBinder.Eval(dataItem, "FocQty");
                BindGridRow_Label(e, "lbl_FocQty", FormatQty(focQty));
                BindGridRow_SpinEdit(e, "se_CnFoc", focQty, originFocQty, _default.DigitQty);


                if (e.Row.FindControl("ddl_CnUnit") != null)
                {
                    var ddl = e.Row.FindControl("ddl_CnUnit") as ASPxComboBox;

                    if (inventoryUnit == rcvUnit)
                        ddl.Items.Add(new ListEditItem { Value = rcvUnit, Text = rcvUnit });
                    else
                        ddl.Items.AddRange(new ListEditItem[]
                        {
                            new ListEditItem { Value = rcvUnit, Text = rcvUnit},
                            new ListEditItem { Value = inventoryUnit, Text = inventoryUnit}
                        });

                    ddl.Value = unitCode;
                    ddl.Visible = cnType == "Q";
                }


                var taxAdj = Convert.ToBoolean(DataBinder.Eval(dataItem, "TaxAdj"));

                if (e.Row.FindControl("chk_TaxAdj") != null)
                {
                    var chk = e.Row.FindControl("chk_TaxAdj") as CheckBox;

                    chk.Checked = taxAdj;
                }



                var currencyRate = se_CurrencyRate.Number;

                // CurrNetAmt
                var value = DataBinder.Eval(dataItem, "CurrNetAmt");
                BindGridRow_Label(e, "lbl_CurrNetAmt1", FormatAmt(value));
                BindGridRow_Label(e, "lbl_CurrNetAmt", FormatAmt(value));
                BindGridRow_SpinEdit(e, "se_CnCurrNetAmt", value, originCurrNetAmt, _default.DigitAmt);

                value = DataBinder.Eval(dataItem, "NetAmt");
                BindGridRow_Label(e, "lbl_NetAmt", FormatAmt(value));

                // CurrTaxAmt
                value = DataBinder.Eval(dataItem, "CurrTaxAmt");
                BindGridRow_Label(e, "lbl_CurrTaxAmt1", FormatAmt(value));
                BindGridRow_Label(e, "lbl_CurrTaxAmt", FormatAmt(value));
                BindGridRow_SpinEdit(e, "se_CnCurrTaxAmt", value, originCurrTaxAmt, _default.DigitAmt);

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

                //var taxType = drRec["TaxType"].ToString();
                var taxTypeName = "None";

                switch (taxType.ToUpper())
                {
                    case "I": taxTypeName = "Include";
                        break;
                    case "A": taxTypeName = "Add";
                        break;
                }

                BindGridRow_Label(e, "lbl_RecDate", drRec == null ? "" : FormatDate(drRec["RecDate"]));
                //BindGridRow_Label(e, "lbl_RecTaxType", taxTypeName);
                //BindGridRow_Label(e, "lbl_RecTaxRate", FormatAmt(drRec["TaxRate"]));
                //BindGridRow_Label(e, "lbl_RecPrice", FormatAmt(drRec["Price"]));
                BindGridRow_Label(e, "lbl_RecTaxType", taxTypeName);
                BindGridRow_Label(e, "lbl_RecTaxRate", FormatAmt(drRec["TaxRate"]));
                BindGridRow_Label(e, "lbl_RecPrice", FormatAmt(drRec["Price"]));


                btn_Save.Visible = true;
                btn_Commit.Visible = true;

                btn_AddItem.Enabled = true;
                btn_DeleteItems.Enabled = true;

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
            var lbl_Unit = row.FindControl("lbl_Unit") as Label;


            var se_CnQty = row.FindControl("se_CnQty") as ASPxSpinEdit;
            var se_CnFoc = row.FindControl("se_CnFoc") as ASPxSpinEdit;


            var chk_TaxAdj = row.FindControl("chk_TaxAdj") as CheckBox;
            var se_CnCurrNetAmt = row.FindControl("se_CnCurrNetAmt") as ASPxSpinEdit;
            var se_CnCurrTaxAmt = row.FindControl("se_CnCurrTaxAmt") as ASPxSpinEdit;
            var se_CnCurrTotalAmt = row.FindControl("se_CnCurrTotalAmt") as ASPxSpinEdit;



            var cnType = lbl_CnType.Text.Trim().ToUpper();
            var taxAdj = chk_TaxAdj.Checked;

            se_CnQty.Visible = cnType.StartsWith("Q");
            se_CnFoc.Visible = cnType.StartsWith("Q");
            chk_TaxAdj.Visible = cnType.StartsWith("Q");

            se_CnCurrNetAmt.Visible = cnType.StartsWith("A") || cnType.StartsWith("Q");
            se_CnCurrTaxAmt.Visible = cnType.StartsWith("A") || cnType.StartsWith("Q");
            se_CnCurrTotalAmt.Visible = cnType.StartsWith("A") || cnType.StartsWith("Q");

            se_CnCurrNetAmt.ReadOnly = !taxAdj && cnType.StartsWith("Q");
            se_CnCurrTaxAmt.ReadOnly = !taxAdj && cnType.StartsWith("Q");

            btn_AddItem.Enabled = false;
            btn_DeleteItems.Enabled = false;

            btn_Save.Visible = false;
            btn_Commit.Visible = false;


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

            var recNo = dr["RecNo"].ToString();
            var recDtNo = dr["PoDtNo"].ToString(); // For recDtNo

            var cnQty = 0m;
            var cnFoc = 0m;
            var currNetAmt = 0m;
            var currTaxAmt = 0m;
            var currTotalAmt = 0m;


            var lbl_CnType = row.FindControl("lbl_CnType") as Label;
            var cnType = lbl_CnType.Text.Trim().StartsWith("Q") ? "Q" : "A";

            if (cnType == "Q")
            {
                var se_CnQty = row.FindControl("se_CnQty") as ASPxSpinEdit;
                var se_CnFoc = row.FindControl("se_CnFoc") as ASPxSpinEdit;

                cnQty = se_CnQty.Number;
                cnFoc = se_CnFoc.Number;
            }


            var chk_TaxAdj = row.FindControl("chk_TaxAdj") as CheckBox;

            var se_CnCurrNetAmt = row.FindControl("se_CnCurrNetAmt") as ASPxSpinEdit;
            var se_CnCurrTaxAmt = row.FindControl("se_CnCurrTaxAmt") as ASPxSpinEdit;
            var se_CnCurrTotalAmt = row.FindControl("se_CnCurrTotalAmt") as ASPxSpinEdit;

            currNetAmt = se_CnCurrNetAmt.Number;
            currTaxAmt = se_CnCurrTaxAmt.Number;
            currTotalAmt = se_CnCurrTotalAmt.Number;

            var taxAdj = chk_TaxAdj.Checked;



            dr["RecQty"] = cnQty;
            dr["FocQty"] = cnFoc;

            dr["TaxAdj"] = taxAdj;
            dr["CurrNetAmt"] = currNetAmt;
            dr["CurrTaxAmt"] = currTaxAmt;
            dr["CurrTotalAmt"] = currTotalAmt;

            //dr["Comment"] = "";

            gv.EditIndex = -1;
            gv.DataSource = _dtCnDt;
            gv.DataBind();

        }

        protected void gv_Detail_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var gv = sender as GridView;

            var hf_CnDtNo = gv.Rows[e.RowIndex].FindControl("hf_CnDtNo") as HiddenField;
            var cnDtNo = Convert.ToInt32(hf_CnDtNo.Value);

            //ShowInfo(hf_CnDtNo.Value);

            //var hf_RecDtNo = gv.Rows[e.RowIndex].FindControl("hf_RecDtNo") as HiddenField;
            //var recDtNo = Convert.ToInt32(hf_RecDtNo.Value);
            var item = _dtCnDt.AsEnumerable().FirstOrDefault(x => x.Field<int>("CnDtNo") == cnDtNo);

            if (item != null)
            {
                item.Delete();
                _dtCnDt.AcceptChanges();

            }

            gv.DataSource = _dtCnDt;
            gv.DataBind();
        }


        // gv_Receiving
        private DataTable GetReceivingList(string vendorCode, string currencyCode, int month)
        {
            var toDate = de_CnDate.Date;
            var frDate = new DateTime(toDate.Year, toDate.Month, 1).AddMonths(month * (-1));

            var query = @"
SELECT
	rec.RecNo,
	 CONVERT(VARCHAR(10), RecDate, 103)  as RecDate,
	[Description]
FROM
	PC.REC
WHERE
	DocStatus = 'Committed'
    AND CurrencyCode = @CurrencyCode
	AND VendorCode=@VendorCode";

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
            var vendorCode = ddl_Vendor.Value == null ? "" : ddl_Vendor.Value.ToString();
            var month = Convert.ToInt32(ddl_RecPeriod.Value);

            var dt = GetReceivingList(vendorCode, currencyCode, month);

            ddl.ValueField = "RecNo";
            ddl.DataSource = dt;
            ddl.DataBind();

            //var items = dt.AsEnumerable()
            //    .Select(x => new ListEditItem
            //    {
            //        Text = string.Format("{0} |",x.Field<string>("RecNo")),
            //        Value = x.Field<string>("RecNo"),
            //    })
            //    .ToArray();

            //ddl.Items.Clear();
            //ddl.Items.AddRange(items);


        }

        protected void ddl_Receiving_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;
            var recNo = ddl.Value.ToString();
            var cnNo = _ID;


            var message = "";
            var found = _dtCnDt.AsEnumerable().Select(x => x.Field<string>("RecNo") == recNo).Count() > 0;

            if (!found) // not used in this cn
            {
                var query = @"
SELECT
    TOP(1)
	cn.CnNo, 
	cn.CnDate
FROM 
	PC.Cn 
	JOIN PC.CnDt ON cn.CnNo=cndt.CnNo 
WHERE 
	cn.DocStatus <> 'Voided' 
    AND cn.CnNo <> @CnNo
	AND RecNo = @RecNo 
";

                var dt = _bu.DbExecuteQuery(
                        query,
                        new Blue.DAL.DbParameter[] 
                        { 
                            new Blue.DAL.DbParameter("@CnNo", cnNo),
                            new Blue.DAL.DbParameter("@RecNo", recNo)
                        },
                        hf_ConnStr.Value);

                if (dt.Rows.Count > 0)
                {
                    var dr = dt.Rows[0];
                    var link = string.Format("Cn.aspx?BuCode={0}&ID={1}", LoginInfo.BuInfo.BuCode, dr["CnNo"].ToString());
                    message = string.Format("This receiving has already used in <a href='{0}' target='_blank'>{1}</a> @{2}", link, dr["CnNo"], FormatDate(dr["CnDate"]));
                }
            }

            lbl_AlreadyCreatedReceiving.Text = message;

            // Get receiving details
            var dtRecDt = GetRecDtList(recNo);

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
                var locationCode = DataBinder.Eval(dataItem, "LocationCode").ToString();
                var locationName = DataBinder.Eval(dataItem, "LocationName").ToString();

                BindGridRow_Label(e, "lbl_Location", string.Format("{0} : {1}", locationCode, locationName));

                // Product
                var productCode = DataBinder.Eval(dataItem, "ProductCode").ToString();
                var productDesc1 = DataBinder.Eval(dataItem, "ProductDesc1").ToString();
                var productDesc2 = DataBinder.Eval(dataItem, "ProductDesc2").ToString();

                BindGridRow_Label(e, "lbl_Product", string.Format("{0} : {1} | {2}", productCode, productDesc1, productDesc2));

                // Price
                var price = DataBinder.Eval(dataItem, "Price").ToString();

                BindGridRow_Label(e, "lbl_Price", FormatAmt(price));


                // RcvUnit
                var rcvUnit = DataBinder.Eval(dataItem, "RcvUnit").ToString();

                BindGridRow_Label(e, "lbl_RcvUnit", rcvUnit);
                BindGridRow_Label(e, "lbl_RcvUnitFoc", rcvUnit);

                // RecQty
                var recQty = Convert.ToDecimal(DataBinder.Eval(dataItem, "RecQty"));

                BindGridRow_Label(e, "lbl_RecQty", FormatQty(recQty));
                BindGridRow_HiddenField(e, "hf_RecQty", recQty);

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

                // Credit Note
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
                    var ddl = e.Row.FindControl("ddl_CnType") as ASPxComboBox;

                    ddl.Value = cnType;

                    ddl.Enabled = string.IsNullOrEmpty(lbl_AlreadyCreatedReceiving.Text.Trim());
                }

                var inventoryUnit = DataBinder.Eval(dataItem, "InventoryUnit").ToString();
                var inventoryQty = Convert.ToDecimal(DataBinder.Eval(dataItem, "Rate")) * recQty;
                var rate = Convert.ToDecimal(DataBinder.Eval(dataItem, "Rate"));
                var cnUnit = DataBinder.Eval(dataItem, "CnUnit").ToString();

                BindGridRow_Label(e, "lbl_InventoryQty", string.Format("<b>Unit rate</b>: {0} | <b>{1}</b> {2}", FormatQty(rate), FormatQty(inventoryQty), inventoryUnit));

                if (e.Row.FindControl("ddl_CnUnit") != null)
                {
                    var ddl = e.Row.FindControl("ddl_CnUnit") as ASPxComboBox;

                    ddl.Items.AddRange(new ListEditItem[]
                    {
                        new ListEditItem { Value = rcvUnit, Text = rcvUnit},
                        new ListEditItem { Value = inventoryUnit, Text = inventoryUnit}
                    });

                    ddl.Value = cnUnit;
                    ddl.Visible = cnType == "Q";
                }

                if (e.Row.FindControl("se_CnQty") != null)
                {
                    var se = e.Row.FindControl("se_CnQty") as ASPxSpinEdit;
                    var maxValue = rcvUnit == cnUnit ? Convert.ToDecimal(recQty) : inventoryQty;

                    se.DecimalPlaces = _default.DigitQty;
                    se.MaxValue = maxValue;

                    se.Value = DataBinder.Eval(dataItem, "CnQty");
                    se.Visible = cnType == "Q";

                }


                if (e.Row.FindControl("se_cnFoc") != null)
                {
                    var se = e.Row.FindControl("se_cnFoc") as ASPxSpinEdit;
                    var maxValue = Convert.ToDecimal(focQty);

                    se.DecimalPlaces = 3; // _default.DigitQty;
                    se.MaxValue = maxValue;

                    se.Value = DataBinder.Eval(dataItem, "CnFoc");
                    se.Visible = cnType == "Q" && maxValue > 0;
                }

                var taxAdj = string.IsNullOrEmpty(DataBinder.Eval(dataItem, "TaxAdj").ToString()) ? false : Convert.ToBoolean(DataBinder.Eval(dataItem, "TaxAdj"));



                if (e.Row.FindControl("chk_TaxAdj") != null)
                {
                    var chk = e.Row.FindControl("chk_TaxAdj") as CheckBox;
                    chk.Checked = taxAdj;
                    chk.Visible = cnType == "Q";

                }

                if (e.Row.FindControl("se_CnCurrNetAmt") != null)
                {
                    var se = e.Row.FindControl("se_CnCurrNetAmt") as ASPxSpinEdit;


                    se.Value = DataBinder.Eval(dataItem, "CnCurrNetAmt");
                    se.DecimalPlaces = _default.DigitAmt;
                    se.Visible = cnType != "N";
                    se.ReadOnly = taxAdj == false;

                    se.MaxValue = Convert.ToDecimal(DataBinder.Eval(dataItem, "CurrNetAmt"));
                }

                if (e.Row.FindControl("se_CnCurrTaxAmt") != null)
                {
                    var se = e.Row.FindControl("se_CnCurrTaxAmt") as ASPxSpinEdit;

                    se.Value = DataBinder.Eval(dataItem, "CnCurrTaxAmt");
                    se.DecimalPlaces = _default.DigitAmt;
                    se.Visible = cnType != "N";
                    se.ReadOnly = taxAdj == false;

                    se.MaxValue = Convert.ToDecimal(DataBinder.Eval(dataItem, "CurrTaxAmt"));

                }

                if (e.Row.FindControl("se_CnCurrTotalAmt") != null)
                {
                    var se = e.Row.FindControl("se_CnCurrTotalAmt") as ASPxSpinEdit;

                    se.Value = DataBinder.Eval(dataItem, "CnCurrTotalAmt");
                    se.DecimalPlaces = _default.DigitAmt;
                    se.Visible = cnType != "N";

                }

                // Hidden Fields
                #region --Hidden Fields--

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

                if (e.Row.FindControl("hf_RecQty") != null)
                {
                    (e.Row.FindControl("hf_RecQty") as HiddenField).Value = DataBinder.Eval(dataItem, "RecQty").ToString();
                }

                if (e.Row.FindControl("hf_FocQty") != null)
                {
                    (e.Row.FindControl("hf_FocQty") as HiddenField).Value = DataBinder.Eval(dataItem, "FocQty").ToString();
                }


                if (e.Row.FindControl("hf_Rate") != null)
                {
                    (e.Row.FindControl("hf_Rate") as HiddenField).Value = DataBinder.Eval(dataItem, "Rate").ToString();
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
                #endregion
            }
        }

        protected void ddl_CnType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;

            // Controls
            var ddl_CnUnit = ddl.NamingContainer.FindControl("ddl_CnUnit") as ASPxComboBox;
            var se_CnQty = ddl.NamingContainer.FindControl("se_CnQty") as ASPxSpinEdit;
            var se_CnFoc = ddl.NamingContainer.FindControl("se_CnFoc") as ASPxSpinEdit;
            var chk_TaxAdj = ddl.NamingContainer.FindControl("chk_TaxAdj") as CheckBox;

            var se_CnCurrNetAmt = ddl.NamingContainer.FindControl("se_CnCurrNetAmt") as ASPxSpinEdit;
            var se_CnCurrTaxAmt = ddl.NamingContainer.FindControl("se_CnCurrTaxAmt") as ASPxSpinEdit;
            var se_CnCurrTotalAmt = ddl.NamingContainer.FindControl("se_CnCurrTotalAmt") as ASPxSpinEdit;


            var cnType = ddl.Value.ToString();

            ddl_CnUnit.Visible = false;
            se_CnQty.Visible = false;
            se_CnFoc.Visible = false;
            se_CnCurrNetAmt.Visible = false;
            se_CnCurrTaxAmt.Visible = false;
            se_CnCurrTotalAmt.Visible = false;

            se_CnCurrNetAmt.ReadOnly = cnType.StartsWith("Q");
            se_CnCurrTaxAmt.ReadOnly = cnType.StartsWith("Q");
            se_CnCurrTotalAmt.ReadOnly = true;

            ddl_CnUnit.Visible = cnType.StartsWith("Q") && se_CnQty.MaxValue > 0;
            se_CnQty.Visible = cnType.StartsWith("Q") && se_CnQty.MaxValue > 0;
            se_CnFoc.Visible = cnType.StartsWith("Q") && se_CnFoc.MaxValue > 0;

            chk_TaxAdj.Visible = cnType.StartsWith("Q") && se_CnQty.MaxValue > 0;

            se_CnCurrNetAmt.Visible = cnType.StartsWith("A") || cnType.StartsWith("Q");
            se_CnCurrTaxAmt.Visible = cnType.StartsWith("A") || cnType.StartsWith("Q");
            se_CnCurrTotalAmt.Visible = cnType.StartsWith("A") || cnType.StartsWith("Q");

            se_CnCurrTotalAmt.Number = se_CnCurrNetAmt.Number + se_CnCurrTaxAmt.Number;

            if (ddl.NamingContainer.FindControl("img_Check") != null)
            {
                var img = ddl.NamingContainer.FindControl("img_Check") as System.Web.UI.WebControls.Image;

                img.ImageUrl = cnType == "N" ? "" : IMG_CHECK;
                img.Visible = cnType != "N";
            }
        }

        protected void ddl_CnUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;

            var hf_Rate = ddl.NamingContainer.FindControl("hf_Rate") as HiddenField;
            var hf_RcvUnit = ddl.NamingContainer.FindControl("hf_RcvUnit") as HiddenField;
            var hf_RecQty = ddl.NamingContainer.FindControl("hf_RecQty") as HiddenField;

            var se_CnQty = ddl.NamingContainer.FindControl("se_CnQty") as ASPxSpinEdit;


            var cnUnit = ddl.SelectedItem.Value.ToString();
            var rcvUnit = hf_RcvUnit.Value.ToString();
            var recQty = Convert.ToDecimal(hf_RecQty.Value);
            var rate = Convert.ToDecimal(hf_Rate.Value);

            var cnQty = se_CnQty.Number;

            if (cnUnit == rcvUnit)
            {
                se_CnQty.MaxValue = recQty;

                if (cnQty > recQty)
                    se_CnQty.Number = recQty;
            }
            else
            {
                var baseQty = RoundQty(recQty * rate);
                se_CnQty.MaxValue = baseQty;

                if (cnQty > baseQty)
                    se_CnQty.Number = baseQty;
            }

            CnQtyChanged(sender);
        }

        protected void se_CnQty_NumberChanged(object sender, EventArgs e)
        {
            //var se = sender as ASPxSpinEdit;

            CnQtyChanged(sender);

        }


        protected void chk_TaxAdj_CheckedChanged(object sender, EventArgs e)
        {
            var chk = sender as CheckBox;

            var se_CnCurrNetAmt = chk.NamingContainer.FindControl("se_CnCurrNetAmt") as ASPxSpinEdit;
            var se_CnCurrTaxAmt = chk.NamingContainer.FindControl("se_CnCurrTaxAmt") as ASPxSpinEdit;
            var se_CnCurrTotalAmt = chk.NamingContainer.FindControl("se_CnCurrTotalAmt") as ASPxSpinEdit;

            var hf_OriginNetAmt = chk.NamingContainer.FindControl("hf_OriginNetAmt") as HiddenField;
            var hf_OriginTaxAmt = chk.NamingContainer.FindControl("hf_OriginTaxAmt") as HiddenField;

            if (chk.Checked)
            {
                se_CnCurrNetAmt.ReadOnly = false;
                se_CnCurrTaxAmt.ReadOnly = false;

                se_CnCurrTaxAmt.Enabled = se_CnCurrTaxAmt.MaxValue > 0;



                hf_OriginNetAmt.Value = se_CnCurrNetAmt.Number.ToString();
                hf_OriginTaxAmt.Value = se_CnCurrTaxAmt.Number.ToString();
            }
            else
            {

                se_CnCurrNetAmt.ReadOnly = true;
                se_CnCurrTaxAmt.ReadOnly = true;

                se_CnCurrNetAmt.Number = Convert.ToDecimal(hf_OriginNetAmt.Value);
                se_CnCurrTaxAmt.Number = Convert.ToDecimal(hf_OriginTaxAmt.Value);
            }

            se_CnCurrTotalAmt.Number = se_CnCurrTaxAmt.Number + se_CnCurrNetAmt.Number;
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
            #region
            foreach (GridViewRow row in gv_Receiving.Rows)
            {
                var lbl_RecDtNo = row.FindControl("lbl_RecDtNo") as Label;
                var ddl_CnType = row.FindControl("ddl_CnType") as ASPxComboBox;


                var recDtNo = lbl_RecDtNo.Text;
                var cnType = ddl_CnType.Value.ToString();


                if (cnType.StartsWith("Q"))
                {
                    var se_CnQty = row.FindControl("se_CnQty") as ASPxSpinEdit;

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
            #endregion

            var maxCnDtNo = _dtCnDt.Rows.Count == 0 ? 1 : _dtCnDt.AsEnumerable().Max(x => x.Field<int>("CnDtNo"));

            foreach (GridViewRow row in gv_Receiving.Rows)
            {
                var hf_CnDtNo = row.FindControl("hf_CnDtNo") as HiddenField;
                var ddl_CnType = row.FindControl("ddl_CnType") as ASPxComboBox;

                var lbl_RecDtNo = row.FindControl("lbl_RecDtNo") as Label;

                var hf_LocationCode = row.FindControl("hf_LocationCode") as HiddenField;
                var hf_ProductCode = row.FindControl("hf_ProductCode") as HiddenField;
                var hf_RcvUnit = row.FindControl("hf_RcvUnit") as HiddenField;
                var hf_RecQty = row.FindControl("hf_RecQty") as HiddenField;
                var hf_FocQty = row.FindControl("hf_FocQty") as HiddenField;
                var hf_Rate = row.FindControl("hf_Rate") as HiddenField;
                var hf_Price = row.FindControl("hf_Price") as HiddenField;
                var hf_TaxType = row.FindControl("hf_TaxType") as HiddenField;
                var hf_TaxRate = row.FindControl("hf_TaxRate") as HiddenField;
                var hf_NetAmt = row.FindControl("hf_NetAmt") as HiddenField;
                var hf_TaxAmt = row.FindControl("hf_TaxAmt") as HiddenField;
                var hf_TotalAmt = row.FindControl("hf_TotalAmt") as HiddenField;

                var ddl_CnUnit = row.FindControl("ddl_CnUnit") as ASPxComboBox;
                var se_CnQty = row.FindControl("se_CnQty") as ASPxSpinEdit;
                var se_CnFoc = row.FindControl("se_CnFoc") as ASPxSpinEdit;

                var chk_TaxAdj = row.FindControl("chk_TaxAdj") as CheckBox;
                var se_CnCurrNetAmt = row.FindControl("se_CnCurrNetAmt") as ASPxSpinEdit;
                var se_CnCurrTaxAmt = row.FindControl("se_CnCurrTaxAmt") as ASPxSpinEdit;
                var se_CnCurrTotalAmt = row.FindControl("se_CnCurrTotalAmt") as ASPxSpinEdit;

                var cnType = ddl_CnType.Value.ToString();
                var recNo = lbl_RecNo.Text.Trim();
                var recDtNo = lbl_RecDtNo.Text;

                var price = Convert.ToDecimal(hf_Price.Value);
                var taxType = hf_TaxType.Value;
                var taxRate = Convert.ToDecimal(hf_TaxRate.Value);
                var rcvUnit = hf_RcvUnit.Value.ToString();
                var recQty = Convert.ToDecimal(hf_RecQty.Value);
                var focQty = Convert.ToDecimal(hf_FocQty.Value);
                var cnUnit = ddl_CnUnit.SelectedItem.Value.ToString();
                var rate = Convert.ToDecimal(hf_Rate.Value);
                var cnQty = se_CnQty.Number;
                var cnFoc = se_CnFoc.Number;

                var taxAdj = chk_TaxAdj.Checked;
                var currNetAmt = se_CnCurrNetAmt.Number;
                var currTaxAmt = se_CnCurrTaxAmt.Number;
                var currTotalAmt = se_CnCurrTotalAmt.Number;
                var comment = "";

                var currencyRate = se_CurrencyRate.Number;
                var cnDtNo = Convert.ToInt32(hf_CnDtNo.Value);

                // Edit
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

                            cnDt["RecQty"] = cnType == "A" ? recQty : cnQty;
                            cnDt["FocQty"] = cnType == "A" ? focQty : cnFoc;
                            cnDt["UnitCode"] = cnType == "Q" ? ddl_CnUnit.SelectedItem.Value.ToString() : hf_RcvUnit.Value;


                            cnDt["TaxAdj"] = taxAdj;
                            cnDt["CurrNetAmt"] = currNetAmt;
                            cnDt["CurrTaxAmt"] = currTaxAmt;
                            cnDt["CurrTotalAmt"] = currTotalAmt;

                            cnDt["NetAmt"] = RoundAmt(currNetAmt * currencyRate);
                            cnDt["TaxAmt"] = RoundAmt(currTaxAmt * currencyRate);
                            cnDt["TotalAmt"] = RoundAmt(currTotalAmt * currencyRate);

                        }
                    }

                }

                // Add new
                else if (cnType != "N")
                {
                    maxCnDtNo++;

                    var dr = _dtCnDt.NewRow();

                    dr["CnNo"] = lbl_CnNo.Text;
                    dr["CnDtNo"] = maxCnDtNo;
                    dr["CnType"] = cnType;
                    dr["RecNo"] = recNo;
                    dr["Location"] = hf_LocationCode.Value;
                    dr["ProductCode"] = hf_ProductCode.Value;
                    dr["UnitCode"] = cnType == "Q" ? ddl_CnUnit.SelectedItem.Value.ToString() : hf_RcvUnit.Value;
                    dr["RecQty"] = cnType == "A" ? recQty : cnQty;
                    dr["FocQty"] = cnType == "A" ? focQty : cnFoc;
                    dr["Price"] = price;
                    dr["TaxAdj"] = 0;
                    dr["TaxType"] = taxType;
                    dr["TaxRate"] = taxRate;

                    dr["TaxAdj"] = taxAdj;
                    dr["CurrNetAmt"] = currNetAmt;
                    dr["CurrTaxAmt"] = currTaxAmt;
                    dr["CurrTotalAmt"] = currTotalAmt;

                    dr["NetAmt"] = RoundAmt(currNetAmt * currencyRate);
                    dr["TaxAmt"] = RoundAmt(currTaxAmt * currencyRate);
                    dr["TotalAmt"] = RoundAmt(currTotalAmt * currencyRate);

                    dr["Comment"] = comment;

                    // Using for RecNo, RecDtNo
                    dr["PoNo"] = recNo;
                    dr["PoDtNo"] = recDtNo;

                    _dtCnDt.Rows.Add(dr);
                }
            }

            _dtCnDt.AcceptChanges();

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

            var isNew = string.IsNullOrEmpty(_ID);

            var queries = new StringBuilder();
            var parameters = new List<Blue.DAL.DbParameter>();

            queries.AppendLine("BEGIN TRANSACTION");
            queries.AppendLine("BEGIN TRY");

            // Header
            #region -- header --

            var cnDate = de_CnDate.Date;
            var cnNo = string.IsNullOrEmpty(_ID) ? _cn.GetNewID(cnDate, hf_ConnStr.Value) : _ID;
            var docNo = txt_DocNo.Text.Trim();
            var docDate = de_DocDate.Date;
            var vendorCode = ddl_Vendor.Value.ToString();
            var currencyCode = ddl_Currency.Value.ToString();
            var currencyRate = se_CurrencyRate.Number;
            var description = txt_Desc.Text.Trim();

            parameters.Add(new Blue.DAL.DbParameter("@CnNo", cnNo));
            parameters.Add(new Blue.DAL.DbParameter("@CnDate", cnDate.ToString("yyyy-MM-dd")));
            parameters.Add(new Blue.DAL.DbParameter("@DocNo", docNo));
            parameters.Add(new Blue.DAL.DbParameter("@DocDate", docDate.ToString("yyyy-MM-dd")));
            parameters.Add(new Blue.DAL.DbParameter("@VendorCode", vendorCode));
            parameters.Add(new Blue.DAL.DbParameter("@Description", description));
            parameters.Add(new Blue.DAL.DbParameter("@CurrencyCode", currencyCode));
            parameters.Add(new Blue.DAL.DbParameter("@ExRateAudit", currencyRate.ToString()));
            parameters.Add(new Blue.DAL.DbParameter("@UpdatedBy", LoginInfo.LoginName));

            if (isNew)
            {
                queries.AppendLine("INSERT INTO PC.Cn (CnNo, CnDate, DocNo, DocDate, VendorCode, Description, CurrencyCode, ExRateAudit, DocStatus, ExportStatus, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)");
                queries.AppendLine("VALUES(@CnNo, @CnDate, @DocNo, @DocDate, @VendorCode, @Description, @CurrencyCode, @ExRateAudit, 'Saved', 0, GETDATE(), @UpdatedBy, GETDATE(), @UpdatedBy)");
            }
            else
            {
                queries.AppendLine("UPDATE PC.Cn");
                queries.AppendLine("SET");
                queries.AppendLine("  CnDate=@CnDate,");
                queries.AppendLine("  DocNo=@DocNo,");
                queries.AppendLine("  DocDate=@DocDate,");
                queries.AppendLine("  VendorCode=@VendorCode,");
                queries.AppendLine("  Description=@Description,");
                queries.AppendLine("  CurrencyCode=@CurrencyCode,");
                queries.AppendLine("  ExRateAudit=@ExRateAudit,");
                queries.AppendLine("  UpdatedDate=GETDATE(),");
                queries.AppendLine("  UpdatedBy=@UpdatedBy");
                queries.AppendLine("WHERE CnNo=@CnNo");
            }

            _bu.DbExecuteQuery(queries.ToString(), parameters.ToArray(), hf_ConnStr.Value);

            #endregion


            // Detail


            var cnDtNo = 1;
            #region --details--

            queries.AppendLine("  DELETE FROM PC.CnDt WHERE CnNo=@CnNo");

            foreach (DataRow dr in _dtCnDt.Rows)
            {
                var cnType = dr["CnType"].ToString();
                var recNo = dr["RecNo"].ToString();
                var location = dr["Location"].ToString();
                var productCode = dr["ProductCode"].ToString();
                var unitCode = dr["UnitCode"].ToString();
                var recQty = dr["RecQty"].ToString();
                var focQty = dr["FocQty"].ToString();
                var price = dr["Price"].ToString();
                var taxAdj = dr["TaxAdj"].ToString(); //Convert.ToBoolean(dr["TaxAdj"]);
                var taxType = dr["TaxType"].ToString();
                var taxRate = dr["TaxRate"].ToString();
                var currNetAmt = dr["CurrNetAmt"].ToString();
                var currTaxAmt = dr["CurrTaxAmt"].ToString();
                var currTotalAmt = dr["CurrTotalAmt"].ToString();

                //var netAmt = dr["NetAmt"].ToString();
                //var taxAmt = dr["TaxAmt"].ToString();
                //var totalAmt = dr["TotalAmt"].ToString();
                var netAmt = RoundAmt(Convert.ToDecimal(currNetAmt) * Convert.ToDecimal(currencyRate));
                var taxAmt = RoundAmt(Convert.ToDecimal(currTaxAmt) * Convert.ToDecimal(currencyRate));
                var totalAmt = RoundAmt(Convert.ToDecimal(currTotalAmt) * Convert.ToDecimal(currencyRate));


                var comment = dr["Comment"].ToString();
                var poNo = dr["RecNo"].ToString();
                var poDtNo = string.IsNullOrEmpty(dr["PoDtNo"].ToString()) ? "NULL" : dr["PoDtNo"].ToString();

                queries.Append("INSERT INTO PC.CnDt (CnNo, CnDtNo, CnType, RecNo, Location, ProductCode, UnitCode, RecQty, FocQty, Price, TaxType, TaxRate, TaxAdj, CurrNetAmt, CurrTaxAmt, CurrTotalAmt, NetAmt, TaxAmt, TotalAmt, Comment, PoNo, PoDtNo)");
                queries.AppendFormat(" VALUES(@CnNo, {0}, '{1}', '{2}', '{3}', '{4}', @UnitCode{0}, {5}, {6}, {7}, '{8}', {9}, '{10}', {11}, {12}, {13}, {14}, {15}, {16}, @Comment{0}, '{17}', {18})",
                    cnDtNo,
                    cnType,
                    recNo,
                    location,
                    productCode,
                    recQty,
                    focQty,
                    price,
                    taxType,
                    taxRate,
                    taxAdj,
                    currNetAmt,
                    currTaxAmt,
                    currTotalAmt,
                    netAmt,
                    taxAmt,
                    totalAmt,
                    poNo,
                    poDtNo
                    );
                queries.AppendLine(";");

                parameters.Add(new Blue.DAL.DbParameter("@UnitCode" + cnDtNo.ToString(), unitCode));
                parameters.Add(new Blue.DAL.DbParameter("@Comment" + cnDtNo.ToString(), comment));

                cnDtNo++;
            }

            queries.AppendLine("  COMMIT");
            queries.AppendLine("  SELECT ''");
            queries.AppendLine("END TRY");
            queries.AppendLine("BEGIN CATCH");
            queries.AppendLine("  ROLLBACK");
            queries.AppendLine("  SELECT ERROR_MESSAGE()");
            queries.AppendLine("END CATCH");

            #endregion

            var dt = _bu.DbExecuteQuery(queries.ToString(), parameters.ToArray(), hf_ConnStr.Value);


            var error = dt != null ? dt.Rows[0][0].ToString() : queries.ToString();

            if (!string.IsNullOrEmpty(error))
            {
                ShowWarning(error);
            }
            else
            {
                _bu.DbExecuteQuery("DELETE FROM [IN].Inventory WHERE HdrNo=@CnNo AND [Type] ='CR'", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@CnNo", cnNo) }, hf_ConnStr.Value);

                if (isCommit)
                {
                    // Check Onhand
                    var query = @"
DECLARE @DocDate DATE = (SELECT CnDate FROM PC.Cn WHERE CnNo=@CnNo)
DECLARE @CommittedDate DATE = [IN].GetCommittedDate(@DocDate, NULL)

;WITH
cnrec AS(
	SELECT 
		cndt.Location as LocationCode,
		cndt.ProductCode,
		cndt.RecQty as CnQty,
		CASE 
			WHEN cndt.UnitCode=recdt.RcvUnit THEN recdt.Rate
			ELSE 1
		END as UnitRate
	FROM 
		PC.CnDt
		JOIN PC.RECDt
			ON recdt.RecNo=cndt.RecNo 
			AND recdt.ProductCode=cndt.ProductCode 
			AND recdt.RecDtNo=CASE WHEN ISNULL(cndt.PoDtNo,0)=0 THEN recdt.RecDtNo ELSE cndt.PoDtNo END
	WHERE
		CnType = 'Q'
		AND CnNo = @CnNo
),
cn AS(

	SELECT
		LocationCode,
		ProductCode,
		SUM(ROUND(CnQty * UnitRate, App.DigitQty())) as Qty
	FROM 
		cnrec
	GROUP BY
		LocationCode,
		ProductCode
),
onhand AS(
	SELECT
		cn.LocationCode,
		cn.ProductCode,
		cn.Qty,
		SUM(ISNULL([IN],0)-ISNULL([OUT],0)) as Onhand
	FROM
		cn
		LEFT JOIN [IN].Inventory i ON cn.LocationCode=i.[Location] AND cn.ProductCode=i.[ProductCode]
	WHERE
		CAST(i.CommittedDate as DATE) <= @CommittedDate
	GROUP BY
		cn.LocationCode,
		cn.ProductCode,
		cn.Qty
)
SELECT
	*
FROM
	onhand
WHERE
	Qty > onhand";
                    var dtOnhand = _bu.DbExecuteQuery(
                        query,
                        new Blue.DAL.DbParameter[]
                        {
                            new Blue.DAL.DbParameter("@CnNo", cnNo),
                        },
                        hf_ConnStr.Value);

                    if (dtOnhand.Rows.Count > 0)
                    {
                        var productCode = dtOnhand.Rows[0]["ProductCode"].ToString();
                        var locationCode = dtOnhand.Rows[0]["LocationCode"].ToString();

                        error = string.Format("Not enough quantity in Location='{0}' : Product='{1}'.", locationCode, productCode);
                        ShowWarning(error);

                        return;
                    }


                    _bu.DbExecuteQuery("EXEC [PC].CnCommit @DocNo", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@DocNo", cnNo) }, hf_ConnStr.Value);



                    _transLog.Save("PC", "CN", cnNo, "CREATE", string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);
                    _transLog.Save("PC", "CN", cnNo, "COMMIT", string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);

                    ShowInfo("Committed.");

                    Response.Redirect("Cn.aspx?ID=" + cnNo + "&BuCode=" + _BuCode);
                }
                else
                {
                    ShowInfo("Saved.");

                    var action = isNew ? "CREATE" : "MODIFY";
                    _transLog.Save("PC", "CN", cnNo, action, string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);
                    Response.Redirect("Cn.aspx?ID=" + cnNo + "&BuCode=" + _BuCode);
                }
            }
        }

        private void UpdateCurrencyRate(decimal rate)
        {
            if (_dtCnDt != null)
            {
                foreach (DataRow dr in _dtCnDt.Rows)
                {
                    var currTotalAmt = Convert.ToDecimal(dr["CurrTotalAmt"]);
                    var currTaxAmt = Convert.ToDecimal(dr["CurrTaxAmt"]);

                    var totalAmt = RoundAmt(currTotalAmt * rate);
                    var taxAmt = RoundAmt(currTaxAmt * rate);
                    var netAmt = totalAmt - taxAmt;

                    dr["NetAmt"] = netAmt;
                    dr["TaxAmt"] = taxAmt;
                    dr["TotalAmt"] = totalAmt;
                }

                gv_Detail.DataSource = _dtCnDt;
                gv_Detail.DataBind();

            }

        }

        private void CnQtyChanged(object sender)
        {
            var control = sender as Control;

            // Controls
            var hf_Rate = control.NamingContainer.FindControl("hf_Rate") as HiddenField;
            var hf_RcvUnit = control.NamingContainer.FindControl("hf_RcvUnit") as HiddenField;
            var hf_Price = control.NamingContainer.FindControl("hf_Price") as HiddenField;
            var hf_TaxType = control.NamingContainer.FindControl("hf_TaxType") as HiddenField;
            var hf_TaxRate = control.NamingContainer.FindControl("hf_TaxRate") as HiddenField;

            var hf_NetAmt = control.NamingContainer.FindControl("hf_NetAmt") as HiddenField;
            var hf_TaxAmt = control.NamingContainer.FindControl("hf_TaxAmt") as HiddenField;
            var hf_TotalAmt = control.NamingContainer.FindControl("hf_TotalAmt") as HiddenField;


            var se_CnQty = control.NamingContainer.FindControl("se_CnQty") as ASPxSpinEdit;
            var ddl_CnUnit = control.NamingContainer.FindControl("ddl_CnUnit") as ASPxComboBox;

            var se_CnCurrNetAmt = control.NamingContainer.FindControl("se_CnCurrNetAmt") as ASPxSpinEdit;
            var se_CnCurrTaxAmt = control.NamingContainer.FindControl("se_CnCurrTaxAmt") as ASPxSpinEdit;
            var se_CnCurrTotalAmt = control.NamingContainer.FindControl("se_CnCurrTotalAmt") as ASPxSpinEdit;
            
            // ----------------------------------------------------------------------------------------------------


            var rcvUnit = hf_RcvUnit.Value.ToString();
            var cnUnit = ddl_CnUnit.SelectedItem.Value.ToString();
            var cnQty = se_CnQty.Number;

            var rate = Convert.ToDecimal(hf_Rate.Value);
            var price = Convert.ToDecimal(hf_Price.Value);
            var taxRate = Convert.ToDecimal(hf_TaxRate.Value);
            var taxType = hf_TaxType.Value;

            var rcNetAmt = Convert.ToDecimal(hf_NetAmt.Value);
            var rcTaxAmt = Convert.ToDecimal(hf_TaxAmt.Value);
            var rcTotalAmt = rcNetAmt + rcTaxAmt; //Convert.ToDecimal(hf_TotalAmt.Value);

            price = cnUnit == rcvUnit ? price : price = Math.Round(price / rate, 4, MidpointRounding.AwayFromZero);
            //price = GetPrice(se);


            var item = GetTaxNetAmt(cnQty, price, taxRate, taxType);

            var cnNetAmt = item.NetAmt > rcNetAmt ? rcNetAmt : item.NetAmt;
            var cnTaxAmt = item.TaxAmt > rcTaxAmt ? rcTaxAmt : item.TaxAmt;

            se_CnCurrNetAmt.Number = cnNetAmt;
            se_CnCurrTaxAmt.Number = cnTaxAmt;
            se_CnCurrTotalAmt.Number = cnNetAmt + cnTaxAmt;
        }


        //private void CnQtyChanged(object sender)
        //{
        //    var control = sender as Control;

        //    // Controls
        //    var hf_Rate = control.NamingContainer.FindControl("hf_Rate") as HiddenField;
        //    var hf_RcvUnit = control.NamingContainer.FindControl("hf_RcvUnit") as HiddenField;
        //    var hf_Price = control.NamingContainer.FindControl("hf_Price") as HiddenField;
        //    var hf_TaxType = control.NamingContainer.FindControl("hf_TaxType") as HiddenField;
        //    var hf_TaxRate = control.NamingContainer.FindControl("hf_TaxRate") as HiddenField;

        //    var hf_NetAmt = control.NamingContainer.FindControl("hf_NetAmt") as HiddenField;
        //    var hf_TaxAmt = control.NamingContainer.FindControl("hf_TaxAmt") as HiddenField;
        //    var hf_TotalAmt = control.NamingContainer.FindControl("hf_TotalAmt") as HiddenField;


        //    var se_CnQty = control.NamingContainer.FindControl("se_CnQty") as ASPxSpinEdit;
        //    var ddl_CnUnit = control.NamingContainer.FindControl("ddl_CnUnit") as ASPxComboBox;

        //    var se_CnCurrNetAmt = control.NamingContainer.FindControl("se_CnCurrNetAmt") as ASPxSpinEdit;
        //    var se_CnCurrTaxAmt = control.NamingContainer.FindControl("se_CnCurrTaxAmt") as ASPxSpinEdit;
        //    var se_CnCurrTotalAmt = control.NamingContainer.FindControl("se_CnCurrTotalAmt") as ASPxSpinEdit;
        //    // -------------------------


        //    var rcvUnit = hf_RcvUnit.Value.ToString();
        //    var cnUnit = ddl_CnUnit.SelectedItem.Value.ToString();
        //    var cnQty = se_CnQty.Number;

        //    var rate = Convert.ToDecimal(hf_Rate.Value);
        //    var price = Convert.ToDecimal(hf_Price.Value);
        //    var taxRate = Convert.ToDecimal(hf_TaxRate.Value);
        //    var taxType = hf_TaxType.Value;

        //    var rcNetAmt = Convert.ToDecimal(hf_NetAmt.Value);
        //    var rcTaxAmt = Convert.ToDecimal(hf_TaxAmt.Value);
        //    var rcTotalAmt = rcNetAmt + rcTaxAmt; //Convert.ToDecimal(hf_TotalAmt.Value);

        //    price = cnUnit == rcvUnit ? price : price = Math.Round(price / rate, 4, MidpointRounding.AwayFromZero);
        //    //price = GetPrice(se);


        //    var item = GetTaxNetAmt(cnQty, price, taxRate, taxType);

        //    var cnNetAmt = item.NetAmt > rcNetAmt ? rcNetAmt : item.NetAmt;
        //    var cnTaxAmt = item.TaxAmt > rcTaxAmt ? rcTaxAmt : item.TaxAmt;

        //    se_CnCurrNetAmt.Number = cnNetAmt;
        //    se_CnCurrTaxAmt.Number = cnTaxAmt;
        //    se_CnCurrTotalAmt.Number = cnNetAmt + cnTaxAmt;
        //}


        // Get data

        private DataTable GetRecDtList(string recNo)
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
    --recdt.*,
    recdt.RecNo,
    recdt.RecDtNo,
    recdt.LocationCode,
    recdt.ProductCode,
    recdt.UnitCode,
    --recdt.OrderQty,
    recdt.FocQty,
    recdt.RecQty,
    recdt.RcvUnit,
    recdt.Rate,
    recdt.Price,
    CAST(0 AS BIT) as TaxAdj,
    recdt.TaxType,
    recdt.TaxRate,
    
    recdt.CurrDiscAmt,
    recdt.CurrNetAmt,
    recdt.CurrTaxAmt,
    recdt.CurrTotalAmt,

    recdt.DiccountAmt,
    recdt.NetAmt,
    recdt.TaxAmt,
    recdt.TotalAmt,

    l.LocationName,
	p.ProductDesc1,
	p.ProductDesc2,
	p.InventoryUnit,


    0 as CnDtNo,
    'N' as CnType,
    RcvUnit as CnUnit,
	CAST(0 as decimal(18,3)) as CnQty, 
	CAST(0 as decimal(18,3)) as CnFoc, 
    CAST(0 as decimal(18,4)) as CnCurrNetAmt, 
	CAST(0 as decimal(18,4)) as CnCurrTaxAmt,
	CAST(0 as decimal(18,4)) as CnCurrTotalAmt
FROM 
	PC.RecDt
	LEFT JOIN [IN].[Product] p ON p.ProductCode=recdt.ProductCode
    LEFT JOIN [IN].StoreLocation l ON l.LocationCode=recdt.LocationCode
    
WHERE 
	RecNo=@RecNo
";

            var dtRecDt = _bu.DbExecuteQuery(
                query,
                new Blue.DAL.DbParameter[] 
                { 
                    new Blue.DAL.DbParameter("@RecNo", recNo)
                },
                hf_ConnStr.Value);


            //// Set selected item in grid rows

            var cnDtList = _dtCnDt.AsEnumerable()
                        .Where(x => x.Field<string>("RecNo") == recNo)
                        .ToArray();

            //foreach (var cndt in cnDtList)
            foreach (DataRow dr in _dtCnDt.Rows)
            {

                var cnRecNo = dr["RecNo"].ToString().Trim();
                var recDtNo = dr["PoDtNo"].ToString() == "" ? -1 : Convert.ToInt32(dr["PoDtNo"]);
                var locationCode = dr["Location"].ToString().Trim();
                var productCode = dr["ProductCode"].ToString().Trim();

                var cnDtNo = Convert.ToInt32(dr["CnDtNo"]);
                var cnType = dr["CnType"].ToString();
                var cnUnit = dr["UnitCode"].ToString();
                var cnQty = Convert.ToDecimal(dr["RecQty"]);
                var cnFoc = Convert.ToDecimal(dr["FocQty"]);
                var taxAdj = Convert.ToBoolean(dr["TaxAdj"]);
                var cnCurrNetAmt = Convert.ToDecimal(dr["CurrNetAmt"]);
                var cnCurrTaxAmt = Convert.ToDecimal(dr["CurrTaxAmt"]);
                var cnCurrTotalAmt = Convert.ToDecimal(dr["CurrTotalAmt"]);


                //var recDtNo = cndt.Field<Nullable<int>>("PoDtNo");
                //var locationCode = cndt.Field<string>("Location");
                //var productCode = cndt.Field<string>("ProductCode");

                //var cnDtNo = cndt.Field<int>("CnDtNo");
                //var cnType = cndt.Field<string>("CnType");
                //var cnUnit = cndt.Field<string>("UnitCode");
                //var cnQty = cndt.Field<decimal>("RecQty");
                //var cnFoc = cndt.Field<decimal>("FocQty");
                //var taxAdj = cndt.Field<bool>("TaxAdj");
                //var cnCurrNetAmt = cndt.Field<decimal>("CurrNetAmt");
                //var cnCurrTaxAmt = cndt.Field<decimal>("CurrTaxAmt");
                //var cnCurrTotalAmt = cndt.Field<decimal>("CurrTotalAmt");


                var recItem = recDtNo > -1
                    ? dtRecDt.AsEnumerable().FirstOrDefault(x => x.Field<string>("RecNo").Trim() == cnRecNo && x.Field<int>("RecDtNo") == recDtNo)
                    : dtRecDt.AsEnumerable()
                        .Where(x => x.Field<string>("RecNo").Trim() == cnRecNo)
                        .Where(x => x.Field<string>("LocationCode").Trim() == locationCode)
                        .Where(x => x.Field<string>("ProductCode").Trim() == productCode)
                        .FirstOrDefault();




                if (recItem != null)
                {
                    if (cnType == "Q")
                    {
                        recItem["CnDtNo"] = cnDtNo;
                        recItem["CnType"] = "Q";
                        recItem["CnUnit"] = cnUnit;
                        recItem["CnQty"] = cnQty;
                        recItem["CnFoc"] = cnFoc;

                        recItem["TaxAdj"] = taxAdj;
                        recItem["CnCurrNetAmt"] = cnCurrNetAmt;
                        recItem["CnCurrTaxAmt"] = cnCurrTaxAmt;
                        recItem["CnCurrTotalAmt"] = cnCurrTotalAmt;
                    }
                    else
                    {
                        recItem["TaxAdj"] = taxAdj;
                        recItem["CnDtNo"] = cnDtNo;
                        recItem["CnType"] = "A";
                        recItem["TaxAdj"] = taxAdj;
                        recItem["CnCurrNetAmt"] = cnCurrNetAmt;
                        recItem["CnCurrTaxAmt"] = cnCurrTaxAmt;
                        recItem["CnCurrTotalAmt"] = cnCurrTotalAmt;
                    }
                }
            }

            return dtRecDt;
        }

        private DataTable GetCnDt(string cnNo)
        {
            var query = @"
SELECT 
    * 
FROM 
    PC.CnDt 
WHERE 
    CnNo=@id";

            return _bu.DbExecuteQuery(query, new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@id", cnNo) }, hf_ConnStr.Value);

        }

        private DataRow GetRecDtItem(string recNo, int recDtNo, string productCode, string locationCode)
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
	AND recdt.ProductCode = CASE WHEN @RecDtNo=0 THEN @ProductCode ELSE recdt.ProductCode END
	AND recdt.LocationCode = CASE WHEN @RecDtNo=0 THEN @LocationCode ELSE recdt.LocationCode END";

            var dt = _bu.DbExecuteQuery(query,
                new Blue.DAL.DbParameter[] 
                { 
                    new Blue.DAL.DbParameter("@RecNo", recNo), 
                    new Blue.DAL.DbParameter("@RecDtNo", recDtNo.ToString()), 
                    new Blue.DAL.DbParameter("@ProductCode", productCode), 
                    new Blue.DAL.DbParameter("@LocationCode", locationCode) 
                },
                hf_ConnStr.Value);

            return dt.Rows.Count == 0 ? null : dt.Rows[0];
        }

        private DataRow GetProductItem(string code)
        {
            var dt = _bu.DbExecuteQuery("SELECT * FROM [IN].Product WHERE ProductCode=@Code", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@Code", code) }, hf_ConnStr.Value);

            //return dt.Rows.Count == 0 ? "" : string.Format("{0} | {1}", dt.Rows[0][0].ToString(), dt.Rows[0][1].ToString());
            return dt.Rows.Count == 0 ? null : dt.Rows[0];
        }

        private string GetLocationName(string code)
        {
            var dt = _bu.DbExecuteQuery("SELECT LocationName FROM [IN].StoreLocation WHERE LocationCode=@Code", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@Code", code) }, hf_ConnStr.Value);

            return dt.Rows.Count == 0 ? "" : dt.Rows[0][0].ToString();
        }

        // DbGrid
        #region

        private void BindGridRow_HiddenField(GridViewRowEventArgs e, string itemName, object value)
        {
            if (e.Row.FindControl(itemName) != null)
            {
                var hf = e.Row.FindControl(itemName) as HiddenField;

                hf.Value = value.ToString();
            }
        }

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

        private void BindGridRow_SpinEdit(GridViewRowEventArgs e, string itemName, object value, decimal maxValue, int digit = 2)
        {
            if (e.Row.FindControl(itemName) != null)
            {
                var item = e.Row.FindControl(itemName) as ASPxSpinEdit;

                item.Value = Convert.ToDecimal(value);
                item.ToolTip = item.Text;
                item.DecimalPlaces = digit;
                item.MaxValue = maxValue;

                item.Enabled = maxValue > 0;
            }
        }
        #endregion

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

        private TaxNetAmt GetTaxNetAmt(decimal qty, decimal price, decimal taxRate, string taxType)
        {
            var netAmt = 0m;
            var taxAmt = 0m;
            var amt = RoundAmt(qty * price);

            if (taxType == "N")
            {
                netAmt = amt;
                taxAmt = 0;
            }
            else if (taxType == "A")
            {

                netAmt = amt;
                taxAmt = RoundAmt(amt * taxRate / 100);
            }
            else
            {
                netAmt = RoundAmt( amt * 100 / (taxRate + 100));
                taxAmt = amt - netAmt;
                //taxAmt = (amt * taxRate) / (100 - taxRate);
                //netAmt = amt - taxAmt;
            }

            return new TaxNetAmt { NetAmt = netAmt, TaxAmt = taxAmt };
        }

        private decimal GetPrice(WebControl control)
        {
            var hf_Rate = control.NamingContainer.FindControl("hf_Rate") as HiddenField;
            var hf_Price = control.NamingContainer.FindControl("hf_Price") as HiddenField;

            var hf_RcvUnit = control.NamingContainer.FindControl("hf_RcvUnit") as HiddenField;
            var ddl_CnUnit = control.FindControl("ddl_CnUnit") as ASPxComboBox;

            var rcvUnit = hf_RcvUnit.Value;
            var cnUnit = ddl_CnUnit.SelectedItem.Value.ToString();

            var price = Convert.ToDecimal(hf_Price.Value);
            var rate = Convert.ToDecimal(hf_Rate.Value);

            return cnUnit == rcvUnit ? price : price = Math.Round(price / rate, 4, MidpointRounding.AwayFromZero);
        }

        private void SetCnAmountValues(WebControl control, decimal qty)
        {



            var hf_Rate = control.NamingContainer.FindControl("hf_Rate") as HiddenField;
            var hf_Price = control.NamingContainer.FindControl("hf_Price") as HiddenField;
            var hf_TaxTye = control.NamingContainer.FindControl("hf_TaxType") as HiddenField;
            var hf_TaxRate = control.NamingContainer.FindControl("hf_TaxRate") as HiddenField;

            var hf_RcvUnit = control.NamingContainer.FindControl("hf_RcvUnit") as HiddenField;
            var ddl_CnUnit = control.NamingContainer.FindControl("ddl_CnUnit") as ASPxComboBox;

            var se_CnCurrNetAmt = control.NamingContainer.FindControl("se_CnCurrNetAmt") as ASPxSpinEdit;
            var se_CnCurrTaxAmt = control.NamingContainer.FindControl("se_CnCurrTaxAmt") as ASPxSpinEdit;
            var se_CnCurrTotalAmt = control.NamingContainer.FindControl("se_CnCurrTotalAmt") as ASPxSpinEdit;

            // -------------------------------

            var rcvUnit = hf_RcvUnit.Value;
            var cnUnit = ddl_CnUnit.SelectedItem.Value.ToString();

            var price = Convert.ToDecimal(hf_Price.Value);
            var rate = Convert.ToDecimal(hf_Rate.Value);

            var taxType = hf_TaxTye.Value;
            var taxRate = Convert.ToDecimal(hf_TaxRate.Value);

            price = cnUnit == rcvUnit ? price : price = Math.Round(price / rate, 4, MidpointRounding.AwayFromZero);

            var netAmt = 0m;
            var taxAmt = 0m;
            var totalAmt = 0m;
            var amt = RoundAmt(qty * price);

            if (taxType == "N")
            {
                netAmt = amt;
                taxAmt = 0;
            }
            else if (taxType == "A")
            {

                netAmt = amt;
                taxAmt = RoundAmt(amt * taxRate / 100);
            }
            else
            {
                taxAmt = (amt * taxRate) / (100 - taxRate);
                netAmt = amt - taxAmt;
            }

            totalAmt = netAmt + taxAmt;

            se_CnCurrNetAmt.Number = netAmt;
            se_CnCurrTaxAmt.Number = taxAmt;
            se_CnCurrTotalAmt.Number = totalAmt;
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

        public class TaxNetAmt
        {
            public decimal NetAmt { get; set; }
            public decimal TaxAmt { get; set; }
        }

        public class TaxCalculation
        {
            private decimal taxAmt { get; set; }
            private decimal netAmt { get; set; }
            private decimal totalAmt { get; set; }

            public TaxCalculation(decimal qty, decimal price, string taxType, decimal taxRate, int digitAmt = 2)
            {
                Qty = qty;
                Price = price;
                TaxType = taxType;
                TaxRate = taxRate;

                var amount = Math.Round(qty * price, digitAmt, MidpointRounding.AwayFromZero);

                taxType = taxType.ToLower();

                if (taxType.StartsWith("a"))
                {
                    netAmt = amount;
                    taxAmt = Math.Round(amount * (taxRate / 100), digitAmt, MidpointRounding.AwayFromZero);
                    totalAmt = netAmt + taxAmt;
                }
                else if (taxType.StartsWith("i"))
                {
                    totalAmt = amount;
                    netAmt = Math.Round(amount * (100 / 107), digitAmt, MidpointRounding.AwayFromZero);
                    taxAmt = totalAmt - netAmt;
                }
                else
                {
                    netAmt = amount;
                    taxAmt = 0;
                    totalAmt = amount;
                }



            }

            public decimal Qty { get; set; }
            public decimal Price { get; set; }
            public string TaxType { get; set; }
            public decimal TaxRate { get; set; }

            public decimal TaxAmt { get { return taxAmt; } }
            public decimal NetAmt { get { return netAmt; } }
            public decimal TotalAmt { get { return totalAmt; } }

        }


    }
}