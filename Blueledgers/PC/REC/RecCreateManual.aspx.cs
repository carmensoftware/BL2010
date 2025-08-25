using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
using System.Data.SqlClient;

namespace BlueLedger.PL.IN.REC
{
    public partial class PcRecRecCreateManual : BasePage
    {
        #region Properties

        public Blue.BL.Option.Admin.Interface.AccountMapp AccountMapp = new Blue.BL.Option.Admin.Interface.AccountMapp();
        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();

        public Blue.BL.APP.Config Config = new Blue.BL.APP.Config();
        //public Blue.BL.GnxLib GnxLib = new Blue.BL.GnxLib();
        public Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();
        public Blue.BL.PC.PO.PO Po = new Blue.BL.PC.PO.PO();
        public Blue.BL.PC.PR.PRDt PrDt = new Blue.BL.PC.PR.PRDt();
        public Blue.BL.IN.ProdUnit ProdUnit = new Blue.BL.IN.ProdUnit();
        public Blue.BL.Option.Inventory.Product Product = new Blue.BL.Option.Inventory.Product();
        public Blue.BL.PC.REC.REC Rec = new Blue.BL.PC.REC.REC();
        public Blue.BL.PC.REC.RECDt RecDt = new Blue.BL.PC.REC.RECDt();
        private int _recDtNo;
        public Blue.BL.IN.Inventory inv = new Blue.BL.IN.Inventory();

        private readonly Blue.BL.Ref.Currency currency = new Blue.BL.Ref.Currency();
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        //private readonly BlueLedger.PL.IN.REC.RecFunc recFunc = new BlueLedger.PL.IN.REC.RecFunc();

        protected DefaultValues _default
        {
            get { return ViewState["DefaultValues"] as DefaultValues; }
            set { ViewState["DefaultValues"] = value; }
        }

        public DataSet dsImport = new DataSet();
        public DataSet DsRecEdit
        {
            get
            {
                if (Session["dsRecEdit"] == null)
                    Session["dsRecEdit"] = new DataSet();

                return (DataSet)Session["dsRecEdit"];
            }
            set
            {
                Session["dsRecEdit"] = value;
            }
        }

        public DataSet dsSave
        {
            get
            {
                if (Session["dsSave"] == null)
                    Session["dsSave"] = new DataSet();

                return (DataSet)Session["dsSave"];
            }
            set
            {
                Session["dsSave"] = value;
            }
        }

        public string Mode
        {
            get
            {
                if (Request.QueryString.Count > 0 && Request.QueryString["MODE"] != null)
                    return Request.QueryString["MODE"];

                return string.Empty;
            }
        }

        private string RecEditMode
        {
            get
            {
                if (ViewState["RecEditMode"] != null)
                    return ViewState["RecEditMode"].ToString();
                ViewState["RecEditMode"] = "NEW";
                return ViewState["RecEditMode"].ToString();
            }
            set { ViewState["RecEditMode"] = value; }
        }

        public int RecDtNo
        {
            get
            {
                _recDtNo = (ViewState["RecDtNo"] == null ? 0 : (int)ViewState["RecDtNo"]);
                return _recDtNo;
            }
            set
            {
                _recDtNo = value;
                ViewState.Add("RecDtNo", _recDtNo);
            }
        }

        //public BlueLedger.BL.IN.Inventory.Fifo Fifo = new BlueLedger.BL.IN.Inventory.Fifo();
        public string RecNo
        {
            get { return txt_RecNo.Text; }
            set { txt_RecNo.Text = value; }
        }

        public string LocationStamp
        {
            get
            {
                if (ddl_Location_Stamp.Value != null)
                    return ddl_Location_Stamp.Value.ToString();

                return string.Empty;
            }
        }

        private DataTable dtRecExtCost
        {
            get { return (DataTable)Session["dtRecExtCost"]; }
            set { Session["dtRecExtCost"] = value; }
        }

        private int lastExtDtNo
        {
            get { return (int)Session["lastExtDtNo"]; }
            set { Session["lastExtDtNo"] = value; }

        }



        public decimal GrandNetAmt { get; set; }
        public decimal GrandTaxAmt { get; set; }
        public decimal GrandTotal { get; set; }

        public string NetAcCode { get; set; }
        public string TaxAcCode { get; set; }
        public decimal DiscountUpdateAmt { get; set; }
        public decimal RecQty { get; set; }

        public decimal GrandRecAmt { get; set; }
        public decimal QtyRecUpdate { get; set; }
        public decimal PriceUpdate { get; set; }
        public decimal TaxUpdate { get; set; }
        public decimal TotalAmountUpdate { get; set; }

        // Added on: 25/08/2017
        public decimal currGrandNetAmt { get; set; }
        public decimal currGranTaxAmt { get; set; }
        public decimal currGrandTotalAmt { get; set; }


        const string recExtCost = "RecExtCost";

        private string baseCurrency
        {
            get { return config.GetValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value); }
        }
        // End Added.

        //DataSet dsRecEditCount = new DataSet();

        public decimal currRate
        {
            get { return decimal.Parse(txt_ExRateAu.Text); }
            set { txt_ExRateAu.Text = value.ToString(); }
        }

        #endregion


        #region Page Life Cycle

        protected void Page_Init(object sender, EventArgs e)
        {
            var s = Request.Params["BuCode"].ToString();

            if (Request.Params["BuCode"] != null)
            {
                var bu = new Blue.BL.dbo.Bu();
                hf_ConnStr.Value = bu.GetConnectionString(s);

                inv.GetStructure(dsSave, hf_ConnStr.Value);
                GetImportAccCode();
                Session["dsSave"] = dsSave;
                Session["dsImport"] = dsImport;
            }
            else
            {
                Response.Redirect("~/ErrorPages/SessionTimeOut.aspx");
            }




            var currency = config.GetValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value);
            var digitAmt = config.GetValue("APP", "Default", "DigitAmt", hf_ConnStr.Value);
            var digitQty = config.GetValue("APP", "Default", "DigitQty", hf_ConnStr.Value);
            var digitPrice = config.GetValue("APP", "Default", "DigitPrice", hf_ConnStr.Value);
            var taxRate = config.GetValue("APP", "Default", "TaxRate", hf_ConnStr.Value);
            var costMethod = config.GetValue("IN", "SYS", "COST", hf_ConnStr.Value);

            _default = new DefaultValues
            {
                Currency = currency,
                DigitAmt = string.IsNullOrEmpty(digitAmt) ? 2 : Convert.ToInt32(digitAmt),
                DigitQty = string.IsNullOrEmpty(digitQty) ? 2 : Convert.ToInt32(digitQty),
                DigitPrice = string.IsNullOrEmpty(digitPrice) ? 3 : Convert.ToInt32(digitPrice),
                TaxRate = string.IsNullOrEmpty(taxRate) ? 0 : Convert.ToDecimal(taxRate),
                CostMethod = costMethod.ToUpper()
            };
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                // Initial page
                InitialPage();

            }
            else
            {
                dsImport = (DataSet)Session["dsImport"];
            }
        }

        #endregion

        #region events

        #region Save Commit Back

        private string CheckRequiredBeforeSave()
        {
            string errorMessage = string.Empty;

            if (grd_RecEdit.Rows.Count > 0)
            {
                var startPeriodDate = period.GetLatestOpenStartDate(LoginInfo.ConnStr);

                if (de_RecDate.Date < startPeriodDate)
                {
                    errorMessage = string.Format("Date should be on the openning period '{0}'.", startPeriodDate.ToString("dd/MM/yyyy"));
                }


                if (de_RecDate.Date.Date > DateTime.Today.Date)
                    errorMessage = "Receiving date does not allow in advance.";

                // Check Invoice Date
                if (de_InvDate.Text == string.Empty)
                    errorMessage = "Invoice date is required.";

                if (de_InvDate.Date.Date > DateTime.Today.Date)
                    errorMessage = "Invoice date does not allow in advance.";


                // Check duplicate Invoice No (by Vendor)
                if (txt_InvNo.Text == string.Empty)
                    errorMessage = "Invoice no is required.";
                else
                {
                    string recNo = txt_RecNo.Text.Trim();
                    string invoiceNo = txt_InvNo.Text.Trim();
                    string vendorCode = ddl_Vendor.Visible ? ddl_Vendor.Text.Split(':')[0].ToString() : lbl_VendorCode.Text.Split(':')[0].ToString().Trim();

                    var sql = "SELECT COUNT(*) as RecordCount FROM PC.REC WHERE VendorCode=@VendorCode AND InvoiceNo=@InvoiceNo AND RecNo<>@RecNo AND DocStatus<>'Voided'";
                    var p = new Blue.DAL.DbParameter[]{
                        new Blue.DAL.DbParameter("@VendorCode",vendorCode),
                        new Blue.DAL.DbParameter("@InvoiceNo", invoiceNo),
                        new Blue.DAL.DbParameter("@RecNo", recNo),
                    };

                    DataTable dt = Po.DbExecuteQuery(sql, p, LoginInfo.ConnStr);

                    if (Convert.ToInt32(dt.Rows[0]["RecordCount"]) > 0) // duplicate
                        errorMessage = string.Format("Invoice No '{0}' already exists.", invoiceNo); ;
                }

            }
            else
            {
                errorMessage = "Cannot save because receiving have no any detail.";
            }


            return errorMessage;
        }


        protected void btn_Save_Click(object sender, EventArgs e)
        {
            var errorMessage = CheckRequiredBeforeSave();

            if (errorMessage != string.Empty)
            {
                lbl_WarningOth.Text = errorMessage;
                pop_Warning.ShowOnPageLoad = true;
            }
            else
                pop_ConfirmSave.ShowOnPageLoad = true;

        }

        protected void btn_Commit_Click(object sender, EventArgs e)
        {
            var errorMessage = CheckRequiredBeforeSave();

            if (errorMessage != string.Empty)
            {
                lbl_WarningOth.Text = errorMessage;
                pop_Warning.ShowOnPageLoad = true;
            }
            else
                pop_ConfirmCommit.ShowOnPageLoad = true;


            //if (txt_InvNo.Text == string.Empty || de_InvDate.Text == string.Empty)
            //{
            //    lbl_WarningDelete.Text = "Please insert invoice number and invoice date";
            //    pop_WarningDelete.ShowOnPageLoad = true;
            //    return;
            //}

            //if (grd_RecEdit.Rows.Count > 0)
            //{
            //    pop_ConfirmCommit.ShowOnPageLoad = true;
            //}
            //else
            //{
            //    lbl_WarningDelete.Text = "Cannot commit because receiving have no details.";
            //    pop_WarningDelete.ShowOnPageLoad = true;
            //}
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            if (Mode.ToUpper() == "EDIT")
            {
                Response.Redirect("Rec.aspx?ID=" + txt_RecNo.Text + "&BuCode=" + Request.Params["BuCode"] + "&Vid=" +
                                  Request.Params["Vid"]);
            }
            else
            {
                Response.Redirect("RecLst.aspx");
            }
        }

        #endregion

        #region CommbBox events

        protected void ddl_Vendor_OnItemsRequestedByFilterCondition_SQL(object source,
            ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            var comboBox = (ASPxComboBox)source;
            SqlDataSource1.ConnectionString = hf_ConnStr.Value;

            SqlDataSource1.SelectCommand =
                @"SELECT VendorCode, Name FROM (SELECT v.VendorCode, v.Name, row_number()over(order by v.[VendorCode]) as [rn] 
	FROM [AP].[Vendor]  v 
	WHERE ( v.VendorCode + ' ' + v.Name LIKE @filter ) AND v.IsActive = 'True' ) as st where st.[rn] between @startIndex and @endIndex";

            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            SqlDataSource1.SelectParameters.Add("startIndex", TypeCode.Int64,
                (e.BeginIndex + 1).ToString());
            SqlDataSource1.SelectParameters.Add("endIndex", TypeCode.Int64,
                (e.EndIndex + 1).ToString());
            comboBox.DataSource = SqlDataSource1;
            comboBox.DataBind();
            comboBox.ToolTip = comboBox.Text;
        }

        protected void ddl_Vendor_OnItemRequestedByValue_SQL(object source,
            ListEditItemRequestedByValueEventArgs e)
        {
            var comboBox = (ASPxComboBox)source;

            try
            {
                if (e.Value == null)
                    return;

                SqlDataSource1.SelectCommand =
                    @"SELECT VendorCode, Name FROM [AP].[Vendor] WHERE VendorCode=@VendorCode AND IsActive = 'True' ORDER BY VendorCode";
                SqlDataSource1.ConnectionString = hf_ConnStr.Value;
                SqlDataSource1.SelectParameters.Clear();
                SqlDataSource1.SelectParameters.Add("VendorCode", TypeCode.String, e.Value.ToString());
                comboBox.DataSource = SqlDataSource1;
                comboBox.DataBind();
                comboBox.ToolTip = comboBox.Text;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
        }

        protected void ddl_Vendor_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void ddl_ProductCode_OnItemsRequestedByFilterCondition_SQL(object source,
            ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            var locationCode = LocationStamp ?? string.Empty;

            if (string.IsNullOrEmpty(locationCode)) return;

            var comboBox = (ASPxComboBox)source;

            //new ProductLookup().ItemsRequestedByFilterCondition(
            //    ref comboBox, SqlDataSource1, hf_ConnStr.Value, e, locationCode);
            if (string.IsNullOrEmpty(locationCode)) return;

            SqlDataSource1.ConnectionString = hf_ConnStr.Value;

            SqlDataSource1.SelectCommand =
                @"SELECT  ProductCode ,
                    ProductDesc1 ,
                    ProductDesc2
            FROM    ( SELECT    p.ProductCode ,
                                p.ProductDesc1 ,
                                p.ProductDesc2 ,
                                ROW_NUMBER() OVER ( ORDER BY p.[ProductCode] ) AS [rn]
                        FROM      [IN].Product p
                        WHERE     p.productcode IN ( SELECT   ProductCode
                                                    FROM     [IN].[ProdLoc]
                                                    WHERE    LocationCode = @LocationCode )
                                AND p.IsActive = 'True'
                                AND ( p.ProductCode + ' ' + p.ProductDesc1 + ' '
                                        + p.ProductDesc2 LIKE @filter )
                    ) AS st
            WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            SqlDataSource1.SelectParameters.Add("LocationCode", TypeCode.String, locationCode);
            SqlDataSource1.SelectParameters.Add("startIndex", TypeCode.Int64,
                (e.BeginIndex + 1).ToString());
            SqlDataSource1.SelectParameters.Add("endIndex", TypeCode.Int64,
                (e.EndIndex + 1).ToString());
            comboBox.DataSource = SqlDataSource1;
            comboBox.DataBind();
            comboBox.ToolTip = comboBox.Text;

        }

        protected void ddl_ProductCode_OnItemRequestedByValue_SQL(object source,
            ListEditItemRequestedByValueEventArgs e)
        {
            var comboBox = (ASPxComboBox)source;
            var locationCode = ddl_Location_Stamp.ClientValue;
            //(null != ViewState["ddl_LocationCode.Value"]) ? ViewState["ddl_LocationCode.Value"].ToString() : string.Empty;

            if (string.IsNullOrEmpty(locationCode)) return;

            //new ProductLookup().ItemRequestedByValue(ref comboBox, SqlDataSource1, hf_ConnStr.Value, e, locationCode);

            try
            {
                if ((e.Value == null || string.IsNullOrEmpty(locationCode))) return;

                SqlDataSource1.ConnectionString = hf_ConnStr.Value;
                SqlDataSource1.SelectCommand =
                    @"SELECT  p.ProductCode ,
                        p.ProductDesc1 ,
                        p.ProductDesc2
                FROM    [IN].Product p
                WHERE   p.productcode IN ( SELECT   ProductCode
                                           FROM     [IN].[ProdLoc]
                                           WHERE    LocationCode = @LocationCode )
                        AND p.IsActive = 'True'
                        AND ( p.ProductCode = @ProductCode )";

                SqlDataSource1.SelectParameters.Clear();
                SqlDataSource1.SelectParameters.Add("ProductCode", TypeCode.String, e.Value.ToString());
                SqlDataSource1.SelectParameters.Add("LocationCode", TypeCode.String, locationCode);
                comboBox.DataSource = SqlDataSource1;
                comboBox.DataBind();
                comboBox.ToolTip = comboBox.Text;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }

        }

        protected void ddl_Product_SelectedIndexChanged(object sender, EventArgs e)
        {
            var o = grd_RecEdit.Rows[grd_RecEdit.EditIndex];
            var ddlProduct = o.FindControl("ddl_Product") as ASPxComboBox;
            var lblUnit = o.FindControl("lbl_Unit") as Label;
            var ddlRcvUnit = o.FindControl("ddl_RcvUnit") as ASPxComboBox;
            var lblConvertRate = o.FindControl("lbl_ConvertRate") as Label;
            var ddlLocationStamp = UpdatePanel2.FindControl("ddl_Location_Stamp") as ASPxComboBox;
            //var txt_NetDrAcc = o.FindControl("txt_NetDrAcc") as TextBox;
            //var txt_TaxDrAcc = o.FindControl("txt_TaxDrAcc") as TextBox;
            var ddl_NetDrAcc = (ASPxComboBox)o.FindControl("ddl_NetDrAcc");
            var ddl_TaxDrAcc = (ASPxComboBox)o.FindControl("ddl_TaxDrAcc");

            var chk_TaxAdj = o.FindControl("chk_TaxAdj") as CheckBox;
            var ddl_TaxType = (DropDownList)o.FindControl("ddl_TaxType");
            var txt_TaxRate = (TextBox)o.FindControl("txt_TaxRate");

            if (ddlProduct != null &&
                (ddlLocationStamp != null &&
                 (ddlLocationStamp.Value != null && ddlProduct.Value != null &&
                  !string.IsNullOrEmpty(ddlProduct.Value.ToString()))))
            {
                var productCode = ddlProduct.Value.ToString().Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                //not work >> string produceCode = ddl_Product.SelectedItem.Value.ToString();
                if (lblUnit != null)
                {
                    lblUnit.Text = Product.GetInvenUnit(productCode, hf_ConnStr.Value);
                    //ddl_RcvUnit.DataSource = LookupUnit(produceCode); //ProdUnit.GetLookUp_OrderUnitByProductCode(produceCode, hf_ConnStr.Value);
                    if (ddlRcvUnit != null)
                    {
                        ddlRcvUnit.DataSource = ProdUnit.GetLookUp_OrderUnitByProductCode(productCode, hf_ConnStr.Value);
                        ddlRcvUnit.ValueField = "OrderUnit";
                        ddlRcvUnit.Text = lblUnit.Text;
                        ddlRcvUnit.DataBind();
                    }
                    var drRecDt = DsRecEdit.Tables[RecDt.TableName].Rows[grd_RecEdit.EditIndex];
                    drRecDt["ProductCode"] = ddlProduct.Value.ToString();

                    if (lblConvertRate != null)
                    {
                        lblConvertRate.Text = ProdUnit.GetConvRate(productCode, lblUnit.Text, hf_ConnStr.Value).ToString();
                    }
                }

                string taxType = Product.GetTaxType(productCode, hf_ConnStr.Value);

                if (string.IsNullOrEmpty(taxType))
                    taxType = "N";

                if (!chk_TaxAdj.Checked)
                {
                    ddl_TaxType.SelectedValue = taxType;
                    txt_TaxRate.Text = Product.GetTaxRate(productCode, hf_ConnStr.Value).ToString();
                }


                string buCode = Request.Params["BuCode"].ToString();
            }
        }

        private void CalculateBaseQty()
        {
            var o = grd_RecEdit.Rows[grd_RecEdit.EditIndex];
            var lblBaseQty = o.FindControl("lbl_BaseQty") as Label;
            var ddlRcvUnit = o.FindControl("ddl_RcvUnit") as ASPxComboBox;
            var seRecQtyEdit = o.FindControl("se_RecQtyEdit") as ASPxSpinEdit;
            var productCode = DsRecEdit.Tables[RecDt.TableName].Rows[grd_RecEdit.EditIndex]["ProductCode"].ToString();

            if (lblBaseQty != null && ddlRcvUnit != null && seRecQtyEdit != null && !string.IsNullOrEmpty(productCode))
            {
                decimal recQtyEdit;
                decimal.TryParse(seRecQtyEdit.Text, out recQtyEdit);
                var oldUnit = ddlRcvUnit.Value == null ? "" : ddlRcvUnit.Value.ToString();

                lblBaseQty.Text = String.Format(DefaultQtyFmt,
                    ProdUnit.GetQtyAfterChangeUnit(
                        productCode,
                        ProdUnit.GetInvenUnit(productCode, hf_ConnStr.Value),
                        oldUnit,
                        recQtyEdit,
                        hf_ConnStr.Value));
                lblBaseQty.ToolTip = lblBaseQty.Text;
                var lblReceive = o.FindControl("lbl_Receive") as Label;
                if (lblReceive != null)
                {
                    lblReceive.Text = seRecQtyEdit.Text;
                    lblReceive.ToolTip = seRecQtyEdit.Text;
                }
            }
        }

        protected void ddl_RcvUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl_RcvUnit = sender as ASPxComboBox;

            var se_RecQtyEdit = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_RecQtyEdit") as ASPxSpinEdit;
            var lbl_Receive = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_Receive") as Label;
            var lbl_RcvUnit_Expand = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_RcvUnit_Expand") as Label;
            var lbl_ConvertRate = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_ConvertRate") as Label;
            var lbl_BaseQty = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_BaseQty") as Label;

            string productCode = DsRecEdit.Tables[RecDt.TableName].Rows[grd_RecEdit.EditIndex]["ProductCode"].ToString();

            decimal recQty = Convert.ToDecimal(se_RecQtyEdit.Value);

            if (lbl_Receive != null)
            {
                lbl_Receive.Text = string.Format(DefaultQtyFmt, recQty);
                lbl_Receive.ToolTip = lbl_Receive.Text;
            }

            if (lbl_RcvUnit_Expand != null)
            {
                lbl_RcvUnit_Expand.Text = ddl_RcvUnit.Text;
                lbl_RcvUnit_Expand.ToolTip = lbl_RcvUnit_Expand.Text;
            }

            decimal convertRate = 0;
            if (lbl_ConvertRate != null)
            {
                convertRate = ProdUnit.GetConvRate(productCode, ddl_RcvUnit.Value.ToString(), hf_ConnStr.Value);
                lbl_ConvertRate.Text = string.Format(DefaultQtyFmt, convertRate);
                lbl_ConvertRate.ToolTip = lbl_ConvertRate.Text;
            }

            if (lbl_BaseQty != null)
            {
                lbl_BaseQty.Text = string.Format(DefaultQtyFmt, RoundQty(recQty * convertRate));
                lbl_BaseQty.ToolTip = lbl_BaseQty.Text;
            }



        }

        protected void ddl_RcvUnit_OnItemsRequestedByFilterCondition(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            var comboBox = (ASPxComboBox)source;
            if (comboBox != null)
            {
                SqlDataSource1.ConnectionString = hf_ConnStr.Value;

                SqlDataSource1.SelectCommand = @"SELECT OrderUnit, Rate 
                                                FROM (  SELECT OrderUnit, Rate, ROW_NUMBER() OVER (ORDER BY [OrderUnit]) AS [rn] 
	                                                    FROM [IN].ProdUnit
	                                                    WHERE OrderUnit LIKE @filter) AS st 
                                                WHERE st.[rn] between @startIndex and @endIndex";

                SqlDataSource1.SelectParameters.Clear();
                SqlDataSource1.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
                SqlDataSource1.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
                SqlDataSource1.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
                comboBox.DataSource = SqlDataSource1;
                comboBox.DataBind();
                comboBox.ToolTip = comboBox.Text;
            }
        }

        protected void ddl_TaxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl_TaxType = (DropDownList)sender;

            TextBox txt_TaxRate = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_TaxRate");

            TextBox txt_CurrTaxAmt = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_CurrTaxAmt");
            TextBox txt_TaxAmt = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_TaxAmt");


            if (txt_TaxRate != null)
            {
                txt_TaxRate.Enabled = true;
                txt_CurrTaxAmt.Enabled = true;
            }

            if (ddl_TaxType.SelectedItem.Value.ToUpper() == "N")
            {
                if (txt_TaxRate != null)
                {
                    txt_TaxRate.Text = string.Format("{0:N}", 0);
                    txt_TaxRate.Enabled = false;
                    txt_CurrTaxAmt.Enabled = false;
                    txt_TaxAmt.Enabled = false;
                }
            }
            else
            {
                var taxRate = string.IsNullOrEmpty(txt_TaxRate.Text.Trim()) ? 0m : Convert.ToDecimal(txt_TaxRate.Text.Trim());

                if (taxRate == 0m)
                {
                    txt_TaxRate.Text = _default.TaxRate.ToString();
                }

            }

            CalculateCost(grd_RecEdit.EditIndex);
            // End Modified.
        }



        #region cmb_DeliPoint

        protected void cmb_DeliPoint_OnItemsRequestedByFilterCondition_SQL(object source,
            ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            var comboBox = (ASPxComboBox)source;

            SqlDataSource1.ConnectionString = hf_ConnStr.Value;

            SqlDataSource1.SelectCommand =
                @"SELECT DptCode, Name FROM (SELECT DptCode, Name, row_number()over(order by [DptCode]) as [rn] 
	FROM [IN].DeliveryPoint
	WHERE ( CAST(DptCode AS char) + ' ' + Name LIKE @filter )) as st where st.[rn] between @startIndex and @endIndex";

            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            SqlDataSource1.SelectParameters.Add("startIndex", TypeCode.Int64,
                (e.BeginIndex + 1).ToString());
            SqlDataSource1.SelectParameters.Add("endIndex", TypeCode.Int64,
                (e.EndIndex + 1).ToString());
            comboBox.DataSource = SqlDataSource1;
            comboBox.DataBind();
            comboBox.ToolTip = comboBox.Text;
        }

        protected void cmb_DeliPoint_OnItemRequestedByValue_SQL(object source,
            ListEditItemRequestedByValueEventArgs e)
        {
            var comboBox = (ASPxComboBox)source;

            try
            {
                if (e.Value == null)
                    return;

                SqlDataSource1.SelectCommand =
                    @"SELECT DptCode,Name FROM [IN].DeliveryPoint WHERE DptCode=@DptCode ORDER BY VendorCode";
                SqlDataSource1.ConnectionString = hf_ConnStr.Value;
                SqlDataSource1.SelectParameters.Clear();
                SqlDataSource1.SelectParameters.Add("DptCode", TypeCode.String, e.Value.ToString());
                comboBox.DataSource = SqlDataSource1;
                comboBox.DataBind();
                comboBox.ToolTip = comboBox.Text;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
        }


        #endregion

        #region ddl_Location_Stamp

        protected void ddl_Location_Stamp_OnItemsRequestedByFilterCondition_SQL(object source,
            ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            var comboBox = (ASPxComboBox)source;
            SqlDataSource1.ConnectionString = hf_ConnStr.Value;

            SqlDataSource1.SelectCommand =
                @"SELECT LocationCode, LocationName FROM (
SELECT LocationCode, LocationName, row_number()over(order by [LocationCode]) as [rn] 
 FROM [IN].[StoreLocation] WHERE IsActive = 'True' AND LocationCode+ ' ' + LocationName LIKE @filter )
as st where st.[rn] between @startIndex and @endIndex";

            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            SqlDataSource1.SelectParameters.Add("startIndex", TypeCode.Int64,
                (e.BeginIndex + 1).ToString());
            SqlDataSource1.SelectParameters.Add("endIndex", TypeCode.Int64,
                (e.EndIndex + 1).ToString());
            comboBox.DataSource = SqlDataSource1;
            comboBox.DataBind();
            comboBox.ToolTip = comboBox.Text;
        }

        protected void ddl_Location_Stamp_OnItemRequestedByValue_SQL(object source,
            ListEditItemRequestedByValueEventArgs e)
        {
            var comboBox = (ASPxComboBox)source;

            try
            {
                if (e.Value == null)
                    return;

                SqlDataSource1.SelectCommand =
                    @"SELECT LocationCode, LocationName FROM [IN].[StoreLocation] WHERE LocationCode=@LocationCode ORDER BY LocationCode";
                SqlDataSource1.ConnectionString = hf_ConnStr.Value;
                SqlDataSource1.SelectParameters.Clear();
                SqlDataSource1.SelectParameters.Add("LocationCode", TypeCode.String, e.Value.ToString());
                comboBox.DataSource = SqlDataSource1;
                comboBox.DataBind();
                comboBox.ToolTip = comboBox.Text;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
        }

        protected void ddl_Location_Stamp_Load(object sender, EventArgs e)
        {
        }

        protected void ddl_Location_Stamp_OnSelectedIndexChanged(object sender, EventArgs e)
        {
        }

        #endregion

        #endregion

        #region TextBox events

        protected void txt_NetAmt_TextChanged(object sender, EventArgs e)
        {
            var row = ((GridViewRow)((TextBox)sender).NamingContainer);

            var txtNetAmt = sender as TextBox;
            var seTotalAmt = row.FindControl("se_TotalAmt") as ASPxSpinEdit;
            //Label lbl_TotalAmt = row.FindControl("lbl_TotalAmt") as Label;
            var txtTaxAmt = row.FindControl("txt_TaxAmt") as TextBox;
            decimal netAmt = 0;
            if (txtNetAmt != null) decimal.TryParse(txtNetAmt.Text, out netAmt);
            decimal taxAmt = 0;
            if (txtTaxAmt != null) decimal.TryParse(txtTaxAmt.Text, out taxAmt);
            if (seTotalAmt != null) seTotalAmt.Text = string.Format("{0}", netAmt + taxAmt);
        }

        protected void txt_NetDrAcc_TextChanged(object sender, EventArgs e)
        {
            var txtNetDrAcc = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_NetDrAcc") as TextBox;
            if (txtNetDrAcc != null) NetAcCode = txtNetDrAcc.Text;
        }


        protected void txt_Disc_TextChanged(object sender, EventArgs e)
        {
            // Modified on: 23/08/2017, By: Fon
            TextBox txt_Disc = (TextBox)sender;
            ASPxSpinEdit se_RecQtyEdit = (ASPxSpinEdit)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_RecQtyEdit");
            ASPxSpinEdit se_PriceEdit = (ASPxSpinEdit)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_PriceEdit");
            TextBox txt_CurrDiscAmt = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_CurrDiscAmt");

            if (txt_Disc.Text == string.Empty) txt_Disc.Text = string.Format(DefaultAmtFmt, 0);
            decimal price_Qty = RoundAmt(decimal.Parse(se_PriceEdit.Text) * decimal.Parse(se_RecQtyEdit.Text));
            decimal currDiscAmt = RoundAmt((decimal.Parse(txt_Disc.Text) / 100) * price_Qty);
            txt_CurrDiscAmt.Text = string.Format(DefaultAmtFmt, currDiscAmt);

            CalculateCost(grd_RecEdit.EditIndex);
            // End Modified.
        }

        protected void txt_DiscAmt_TextChanged(object sender, EventArgs e)
        {
            //CalculateCost(grd_RecEdit.EditIndex, "TaxType");
            //this.Calculate(grd_RecEdit.EditIndex);

            // Modified on: 23/08/2017, By: Fon
            TextBox txt_DiscAmt = (TextBox)sender;
            GridViewRow gvr = (GridViewRow)txt_DiscAmt.NamingContainer;
            TextBox txt_CurrNetAmt = (TextBox)gvr.FindControl("txt_CurrNetAmt");
            TextBox txt_CurrDiscAmt = (TextBox)gvr.FindControl("txt_CurrDiscAmt");
            TextBox txt_Disc = (TextBox)gvr.FindControl("txt_Disc");

            if (txt_DiscAmt.Text == string.Empty)
                txt_DiscAmt.Text = string.Format(DefaultAmtFmt, 0);
            decimal currDiscAmt = RoundAmt(decimal.Parse(txt_DiscAmt.Text) / decimal.Parse(txt_ExRateAu.Text));
            decimal discPercent = Math.Round((currDiscAmt / decimal.Parse(txt_CurrNetAmt.Text)) * 100, 2, MidpointRounding.AwayFromZero);
            txt_CurrDiscAmt.Text = string.Format(DefaultAmtFmt, currDiscAmt);
            txt_Disc.Text = string.Format(DefaultAmtFmt, discPercent);

            CalculateCost(grd_RecEdit.EditIndex);
            // End Modified.
        }

        protected void se_PriceEdit_OnNumberChanged(object sender, EventArgs e)
        {
            CalculateCost(grd_RecEdit.EditIndex);
            //CalculateCost(grd_RecEdit.EditIndex, "TaxType");
            //this.Calculate(grd_RecEdit.EditIndex);
        }


        protected void btn_AllocateExtraCost_Click(object sender, EventArgs e)
        {
            AllocateExtraCost();
            grd_RecEdit.DataSource = DsRecEdit.Tables[RecDt.TableName];
            grd_RecEdit.DataBind();

        }

        protected void btn_ExtraCostDetail_Click(object sender, EventArgs e)
        {
            // Bind dll_ExtrCost_Item
            DataTable dt = Rec.DbExecuteQuery("SELECT * FROM PC.ExtCostType", null, hf_ConnStr.Value);
            ddl_ExtraCost_Item.DataValueField = "TypeId";
            ddl_ExtraCost_Item.DataTextField = "TypeName";
            ddl_ExtraCost_Item.DataSource = dt;
            ddl_ExtraCost_Item.DataBind();


            grd_ExtraCost.DataSource = DsRecEdit.Tables[recExtCost];
            grd_ExtraCost.DataBind();

            //dtRecExtCost.Clear();
            dtRecExtCost = null;
            if (DsRecEdit.Tables[recExtCost] != null)
                dtRecExtCost = DsRecEdit.Tables[recExtCost].Copy();

            pop_ExtraCostDetail.ShowOnPageLoad = true;
        }


        protected void btn_SaveExtraCost_Pop_ExtraCostDetail(object sender, EventArgs e)
        {
            object sumAmount;
            sumAmount = DsRecEdit.Tables[recExtCost].Compute("Sum(Amount)", string.Empty);

            se_TotalExtraCost.Text = string.IsNullOrEmpty(sumAmount.ToString()) ? "0.00" : sumAmount.ToString();

            btn_AllocateExtraCost_Click(sender, e);

            pop_ExtraCostDetail.ShowOnPageLoad = false;

        }

        protected void btn_CancelExtraCost_Pop_ExtraCostDetail(object sender, EventArgs e)
        {
            if (dtRecExtCost != null)
            {
                DsRecEdit.Tables.Remove(recExtCost);
                DataTable dt = new DataTable();
                dt.TableName = recExtCost;
                dt = dtRecExtCost.Copy();
                DsRecEdit.Tables.Add(dt);
            }
            pop_ExtraCostDetail.ShowOnPageLoad = false;
        }

        private string GetExtraCostTypeName(int id)
        {
            DataTable dt = Rec.DbExecuteQuery(string.Format("SELECT TypeName FROM PC.ExtCostType WHERE TypeId = {0}", id), null, hf_ConnStr.Value);
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["TypeName"].ToString();
            else
                return string.Empty;
        }

        protected void btn_Add_Pop_ExtraCostDetail_Click(object sender, EventArgs e)
        {
            if (ddl_ExtraCost_Item.SelectedIndex > -1 && Convert.ToDecimal(se_ExtraCost_Amount.Value) > 0)
            {
                lastExtDtNo += 1;

                DataTable dt = DsRecEdit.Tables[recExtCost];
                DataRow dr = dt.NewRow();
                dr["RecNo"] = Request.QueryString["ID"] == null ? string.Empty : Request.QueryString["ID"].ToString(); //Request.QueryString["ID"].ToString();
                dr["DtNo"] = lastExtDtNo;
                dr["TypeId"] = ddl_ExtraCost_Item.SelectedItem.Value;
                dr["Amount"] = Convert.ToDecimal(se_ExtraCost_Amount.Value);

                dt.Rows.Add(dr);

                grd_ExtraCost.DataSource = dt;
                grd_ExtraCost.DataBind();
            }


        }

        protected void grd_ExtraCost_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("hf_DtNo") != null)
                {
                    var obj = e.Row.FindControl("hf_DtNo") as HiddenField;
                    obj.Value = DataBinder.Eval(e.Row.DataItem, "DtNo").ToString();
                }


                if (e.Row.FindControl("lbl_Item") != null)
                {
                    var obj = e.Row.FindControl("lbl_Item") as Label;
                    var value = DataBinder.Eval(e.Row.DataItem, "TypeId");
                    obj.Text = string.IsNullOrEmpty(value.ToString()) ? string.Empty : GetExtraCostTypeName(Convert.ToInt32(value));
                }

                if (e.Row.FindControl("lbl_Amount") != null)
                {
                    var obj = e.Row.FindControl("lbl_Amount") as Label;
                    obj.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "Amount"));
                }


                if (e.Row.FindControl("ddl_Item") != null)
                {
                    var obj = e.Row.FindControl("ddl_Item") as DropDownList;

                    DataTable dt = Rec.DbExecuteQuery("SELECT * FROM PC.ExtCostType", null, hf_ConnStr.Value);
                    obj.DataValueField = "TypeId";
                    obj.DataTextField = "TypeName";
                    obj.DataSource = dt;
                    obj.DataBind();
                    obj.SelectedItem.Value = DataBinder.Eval(e.Row.DataItem, "TypeId").ToString();

                }
                if (e.Row.FindControl("se_Amount") != null)
                {
                    var obj = e.Row.FindControl("se_Amount") as ASPxSpinEdit;
                    obj.Value = DataBinder.Eval(e.Row.DataItem, "Amount");

                    obj.DecimalPlaces = _default.DigitAmt;
                }



            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (e.Row.FindControl("lbl_Amount") != null)
                {
                    object sumAmount;
                    sumAmount = DsRecEdit.Tables[recExtCost].Compute("Sum(Amount)", string.Empty);

                    var obj = e.Row.FindControl("lbl_Amount") as Label;
                    obj.Text = string.IsNullOrEmpty(sumAmount.ToString()) ? string.Format(DefaultAmtFmt, 0) : string.Format(DefaultAmtFmt, Convert.ToDecimal(sumAmount.ToString()));
                }
            }
        }

        protected void grd_ExtraCost_RowEditing(Object sender, GridViewEditEventArgs e)
        {

            (sender as GridView).EditIndex = e.NewEditIndex;
            (sender as GridView).DataSource = DsRecEdit.Tables[recExtCost];
            (sender as GridView).DataBind();

            btn_SaveExtraCost.Visible = false;

        }

        protected void grd_ExtraCost_RowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
        {
            (sender as GridView).EditIndex = -1;
            (sender as GridView).DataSource = DsRecEdit.Tables[recExtCost];
            (sender as GridView).DataBind();

            btn_SaveExtraCost.Visible = true;
        }

        protected void grd_ExtraCost_RowUpdating(Object sender, GridViewUpdateEventArgs e)
        {
            var hf_DtNo = grd_ExtraCost.Rows[e.RowIndex].FindControl("hf_DtNo") as HiddenField;
            var ddl_Item = grd_ExtraCost.Rows[e.RowIndex].FindControl("ddl_Item") as DropDownList;
            var se_Amount = grd_ExtraCost.Rows[e.RowIndex].FindControl("se_Amount") as ASPxSpinEdit;


            DataTable dt = DsRecEdit.Tables[recExtCost];
            DataRow row = dt.Select("DtNo = " + hf_DtNo.Value.ToString())[0];

            int editIndex = dt.Rows.IndexOf(row);


            DataRow dr = dt.Rows[editIndex];

            dr["TypeId"] = ddl_Item.SelectedItem.Value;
            dr["Amount"] = se_Amount.Value;

            (sender as GridView).EditIndex = -1;
            (sender as GridView).DataSource = DsRecEdit.Tables[recExtCost];
            (sender as GridView).DataBind();

            btn_SaveExtraCost.Visible = true;

        }

        protected void grd_ExtraCost_RowDeleting(Object sender, GridViewDeleteEventArgs e)
        {
            var hf_DtNo = grd_ExtraCost.Rows[e.RowIndex].FindControl("hf_DtNo") as HiddenField;

            DsRecEdit.Tables[recExtCost].Select("DtNo = " + hf_DtNo.Value.ToString())[0].Delete();

            (sender as GridView).EditIndex = -1;
            (sender as GridView).DataSource = DsRecEdit.Tables[recExtCost];
            (sender as GridView).DataBind();
        }

        // ------------------------------------


        private decimal SumOfBaseQty(DataTable dt)
        {
            decimal sum = 0;

            foreach (DataRow dr in dt.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    sum += RoundQty((Convert.ToDecimal(dr["FocQty"]) + Convert.ToDecimal(dr["RecQty"])) * Convert.ToDecimal(dr["Rate"]));
                }
            }
            return sum;
        }

        private decimal SumOfNetAmt(DataTable dt)
        {
            decimal sum = 0;

            foreach (DataRow dr in dt.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    sum += (Convert.ToDecimal(dr["NetAmt"]));
                }
            }
            return sum;
        }


        private void AllocateExtraCost()
        {
            DataTable dt = DsRecEdit.Tables[RecDt.TableName];

            if (dt.Columns.Contains("ExtraCost"))
            {
                bool isCalByQty = rdb_ExtraCostByQty.Checked;

                if (se_TotalExtraCost.Text != string.Empty && dt.Rows.Count > 0)
                {
                    decimal totalExtraCost = se_TotalExtraCost.Number;
                    decimal extraCost = 0;
                    decimal sumQty = 0;
                    decimal sumAmt = 0;
                    decimal qty = 0;
                    decimal amt = 0;

                    if (isCalByQty)
                    {
                        sumQty = SumOfBaseQty(dt);
                        if (sumQty != 0)
                            extraCost = totalExtraCost / sumQty;
                    }
                    else
                    {
                        sumAmt = SumOfNetAmt(dt);
                        if (sumAmt != 0)
                            extraCost = totalExtraCost / sumAmt;
                    }



                    int lastIndex = -1;

                    // foreach (DataRow dr in dt.Rows)
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            if (isCalByQty)
                            {
                                qty = RoundQty((Convert.ToDecimal(dr["FOCQty"]) + Convert.ToDecimal(dr["RecQty"])) * Convert.ToDecimal(dr["Rate"]));
                                dr["ExtraCost"] = RoundAmt(qty * extraCost);
                            }
                            else
                            {
                                amt = Convert.ToDecimal(dr["NetAmt"]);
                                dr["ExtraCost"] = RoundAmt(amt * extraCost);
                            }

                            lastIndex = i;
                        }
                    }

                    // Assign the left to the last one
                    if (lastIndex >= 0)
                    {
                        decimal sumExtraCost = RoundAmt(Convert.ToDecimal(dt.Compute("SUM(ExtraCost)", string.Empty).ToString()));
                        decimal diffAmt = totalExtraCost - sumExtraCost;

                        dt.Rows[lastIndex]["ExtraCost"] = Convert.ToDecimal(dt.Rows[lastIndex]["ExtraCost"]) + diffAmt;
                    }

                }


            }
        }



        protected void se_TotalAmt_OnNumberChanged(object sender, EventArgs e)
        {
            CalculateCost(grd_RecEdit.EditIndex);
        }

        protected void se_RecQtyEdit_OnNumberChanged(object sender, EventArgs e)
        {
            CalculateCost(grd_RecEdit.EditIndex);
        }

        protected void txt_TaxRate_TextChanged(object sender, EventArgs e)
        {
            CalculateCost(grd_RecEdit.EditIndex);
        }

        protected void txt_TaxAmt_TextChanged(object sender, EventArgs e)
        {
            //ถ้า Tax Rate Amt เปลี่ยน ไม่ต้องกังวลกับ Tax Rate % แต่ต้องคำนวน Total Amt กับ Net Amt ใหม่
            //CalculateCost(grd_RecEdit.EditIndex, "TaxAmt");

            // Modified on: 23/08/2017, By: Fon
            TextBox txt_TaxAmt = (TextBox)sender;
            TextBox txt_CurrTaxAmt = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_CurrTaxAmt");
            TextBox txt_CurrDiscAmt = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_CurrDiscAmt");
            TextBox txt_TaxRate = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_TaxRate");
            ASPxSpinEdit se_PriceEdit = (ASPxSpinEdit)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_PriceEdit");
            ASPxSpinEdit se_RecQtyEdit = (ASPxSpinEdit)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_RecQtyEdit");

            if (txt_TaxAmt.Text == string.Empty)
                txt_TaxAmt.Text = string.Format(DefaultAmtFmt, 0);
            decimal price = decimal.Parse(se_PriceEdit.Text);
            decimal recQty = decimal.Parse(se_RecQtyEdit.Text);
            decimal currDiscAmt = decimal.Parse(txt_CurrDiscAmt.Text);
            decimal currTaxAmt = RoundAmt(decimal.Parse(txt_TaxAmt.Text) / decimal.Parse(txt_ExRateAu.Text));
            decimal taxRate = Get_TaxRate(currTaxAmt, price * recQty, currDiscAmt, 1);

            txt_CurrTaxAmt.Text = string.Format(DefaultAmtFmt, currTaxAmt);
            txt_TaxRate.Text = string.Format("{0:N}", taxRate);
            CalculateCost(grd_RecEdit.EditIndex);
            // End Modified.
        }

        protected void txt_TaxDrAcc_TextChanged(object sender, EventArgs e)
        {
            var txtTaxDrAcc = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_TaxDrAcc") as TextBox;
            if (txtTaxDrAcc != null) TaxAcCode = txtTaxDrAcc.Text;
        }

        #endregion

        #region CheckBox events

        protected void chk_DiscAdj_CheckedChanged(object sender, EventArgs e)
        {
            var chkDiscAdj = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("chk_DiscAdj") as CheckBox;
            var txtDisc = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_Disc") as TextBox;
            var txtDiscAmt = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_DiscAmt") as TextBox;
            var txt_CurrDiscAmt = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_CurrDiscAmt") as TextBox;

            if (chkDiscAdj != null && chkDiscAdj.Checked)
            {
                // Comment on: 01/02/2018, By: Fon, For: Following from P' Oat guide.
                //if (txtDiscAmt != null) txtDiscAmt.Enabled = true;
                // End Comment.

                if (txtDisc != null) txtDisc.Enabled = true;
                if (txt_CurrDiscAmt != null) txt_CurrDiscAmt.Enabled = true;
            }
            else
            {
                if (txtDiscAmt != null)
                {
                    txtDiscAmt.Enabled = false;
                    txtDiscAmt.Text = string.Format(DefaultAmtFmt, 0);

                    txt_CurrDiscAmt.Enabled = false;
                    txt_CurrDiscAmt.Text = string.Format(DefaultAmtFmt, 0);

                    if (txtDisc != null)
                    {
                        txtDisc.Enabled = false;
                        txtDisc.Text = string.Format(DefaultAmtFmt, 0);
                    }
                }
            }

            //CalculateCost(grd_RecEdit.EditIndex, "TaxType"); //this.Calculate(grd_RecEdit.EditIndex);
            CalculateCost(grd_RecEdit.EditIndex);
        }

        protected void chk_TaxAdj_CheckedChanged(object sender, EventArgs e)
        {
            var chkTaxAdj = sender as CheckBox;
            var txtNetAmt = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_NetAmt") as TextBox;
            var txtTaxAmt = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_TaxAmt") as TextBox;
            var txtTaxRate = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_TaxRate") as TextBox;
            var txt_CurrTaxAmt = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_CurrTaxAmt") as TextBox;
            var ddlTaxType = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("ddl_TaxType") as DropDownList;

            if (ddlTaxType != null && chkTaxAdj != null)
                ddlTaxType.Enabled = chkTaxAdj.Checked;

            if (chkTaxAdj != null && chkTaxAdj.Checked)
            {
                if (ddlTaxType.SelectedItem.Value != "N")
                {
                    txt_CurrTaxAmt.Enabled = true;
                    txtTaxAmt.Enabled = true;
                    txtTaxRate.Enabled = true;
                }
            }
            else
            {
                txtTaxRate.Enabled = false;
                txtTaxRate.Text = string.Format("{0:N}", 0);

                txtTaxAmt.Enabled = false;
                txtTaxAmt.Text = string.Format(DefaultAmtFmt, 0);

                txt_CurrTaxAmt.Enabled = false;
                txt_CurrTaxAmt.Text = string.Format(DefaultAmtFmt, 0);

                CalculateCost(grd_RecEdit.EditIndex);
                //CalculateCost(grd_RecEdit.EditIndex, "TaxType"); // this.Calculate(grd_RecEdit.EditIndex);
            }

        }

        #endregion

        #region grd_RecEdit events

        protected void grd_RecEdit_Load(object sender, EventArgs e)
        {
        }

        protected void grd_RecEdit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // When cancel click
            if (RecEditMode == "NEW")
            {
                DsRecEdit.Tables[RecDt.TableName].Rows[DsRecEdit.Tables[RecDt.TableName].Rows.Count - 1].Delete();
            }

            if (RecEditMode == "EDIT")
            {
                DsRecEdit.Tables[RecDt.TableName].Rows[DsRecEdit.Tables[RecDt.TableName].Rows.Count - 1].CancelEdit();
            }

            RecEditMode = string.Empty; // set before databind
            ddl_Currency.Enabled = true;
            txt_ExRateAu.Enabled = true;

            grd_RecEdit.DataSource = DsRecEdit.Tables[RecDt.TableName];
            grd_RecEdit.EditIndex = -1;
            grd_RecEdit.DataBind();
            UIControl_BetweenAddItem();
        }

        protected void grd_RecEdit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (grd_RecEdit.EditIndex != -1)
            {
                btn_Save.Visible = false;
                btn_Commit.Visible = false;
                btn_Back.Visible = false;
                btn_AddItem.Visible = false;
            }
            else
            {
                btn_Save.Visible = true;
                btn_Commit.Visible = true;
                btn_Back.Visible = true;
                btn_AddItem.Visible = true;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
                GridDataRow(e);

            if (e.Row.RowType == DataControlRowType.Footer)
                GridFooter(e);
        }

        protected void grd_RecEdit_RowEditing(object sender, GridViewEditEventArgs e)
        {
            RecEditMode = "EDIT";
            btn_AddPo.Enabled = false;
            ddl_Currency.Enabled = false;
            txt_ExRateAu.Enabled = false;

            grd_RecEdit.DataSource = DsRecEdit.Tables[RecDt.TableName];
            grd_RecEdit.EditIndex = e.NewEditIndex;
            grd_RecEdit.DataBind();
        }

        private bool ValidateItem()
        {
            var ddlProduct = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
            if (ddlProduct != null)
            {
                ddlProduct.Validate();
                var seRecQtyEdit = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_RecQtyEdit") as ASPxSpinEdit;
                if (seRecQtyEdit != null)
                {
                    seRecQtyEdit.Validate();
                    var sePriceEdit =
                        grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_PriceEdit") as ASPxSpinEdit;
                    if (sePriceEdit != null)
                    {
                        sePriceEdit.Validate();

                        if (ddlProduct.Value == null) return false;
                        decimal dec;
                        decimal.TryParse(seRecQtyEdit.Value.ToString(), out dec);
                        if (dec <= 0) return false;
                        decimal.TryParse(sePriceEdit.Value.ToString(), out dec);
                        if (dec <= 0) return false;
                    }
                }
            }

            return true;
        }

        protected void grd_RecEdit_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            RecEditMode = string.Empty;
            //validate Item
            var ddlProduct = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
            var seRecQtyEdit = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_RecQtyEdit") as ASPxSpinEdit;
            var sePriceEdit = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_PriceEdit") as ASPxSpinEdit;

            if (!ValidateItem()) return;

            // When click save on grid
            btn_AddItem.Enabled = true;
            ddl_Currency.Enabled = true;
            txt_ExRateAu.Enabled = true;


            var o = grd_RecEdit.Rows[grd_RecEdit.EditIndex];
            var drUpdating = DsRecEdit.Tables[RecDt.TableName].Rows[grd_RecEdit.EditIndex];

            var ddlTaxType = o.FindControl("ddl_TaxType") as DropDownList;

            var txtTaxAmt = o.FindControl("txt_TaxAmt") as TextBox;
            var txtTaxRate = o.FindControl("txt_TaxRate") as TextBox;
            var txtDisc = o.FindControl("txt_Disc") as TextBox;
            var txtDiscAmt = o.FindControl("txt_DiscAmt") as TextBox;
            var txtNetAmt = o.FindControl("txt_NetAmt") as TextBox;
            var txtDtrComment = o.FindControl("txt_DtrComment") as TextBox;
            var seFocEdit = o.FindControl("se_FocEdit") as ASPxSpinEdit;
            var chkTaxAdj = o.FindControl("chk_TaxAdj") as CheckBox;
            var chkDiscAdj = o.FindControl("chk_DiscAdj") as CheckBox;
            var seTotalAmt = o.FindControl("se_TotalAmt") as ASPxSpinEdit;
            var lblPoNo = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_PoNo") as Label;
            var lblBaseQty = o.FindControl("lbl_BaseQty") as Label;

            // Added on: 24/08/2017, By: Fon
            TextBox txt_CurrNetAmt = (TextBox)o.FindControl("txt_CurrNetAmt");
            TextBox txt_CurrDiscAmt = (TextBox)o.FindControl("txt_CurrDiscAmt");
            TextBox txt_CurrTaxAmt = (TextBox)o.FindControl("txt_CurrTaxAmt");
            Label lbl_CurrTotalAmtDt = (Label)o.FindControl("lbl_CurrTotalAmtDt");
            Label lbl_TotalAmtDt = (Label)o.FindControl("lbl_TotalAmtDt");
            // End Added.

            ASPxDateEdit de_ExpiryDate = (ASPxDateEdit)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("de_ExpiryDate");

            decimal discAmt;
            decimal.TryParse(txtDiscAmt.Text, out discAmt);
            //decimal totalAmt;
            //decimal.TryParse(seTotalAmt.Text, out totalAmt);
            var ddlLocationStamp = UpdatePanel2.FindControl("ddl_Location_Stamp") as ASPxComboBox;

            if (ddlLocationStamp != null)
                drUpdating["LocationCode"] = ddlLocationStamp.Value.ToString();

            var lblOrderQty = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_OrderQty") as Label;

            if (lblOrderQty != null)
            {
                decimal ordqty;
                decimal.TryParse(lblOrderQty.Text, out ordqty);
                drUpdating["OrderQty"] = ordqty;
            }

            if (sePriceEdit != null)
            {
                decimal priceEdit;
                decimal.TryParse(sePriceEdit.Text, out priceEdit);
                drUpdating["Price"] = priceEdit;
            }

            decimal qtyEdit = 0;
            if (seRecQtyEdit != null)
            {
                decimal.TryParse(seRecQtyEdit.Text, out qtyEdit);
                drUpdating["RecQty"] = qtyEdit;
            }

            var ddlRcvUnit = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("ddl_RcvUnit") as ASPxComboBox;

            if (ddlRcvUnit != null)
            {
                drUpdating["UnitCode"] = ddlRcvUnit.Value;
                drUpdating["RcvUnit"] = ddlRcvUnit.Value;
            }

            var lblConvRate = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_ConvertRate") as Label;

            decimal convRate = 0;
            if (lblConvRate != null)
            {
                decimal.TryParse(lblConvRate.Text, out convRate);
                drUpdating["Rate"] = convRate;
            }

            lblBaseQty.Text = string.Format(DefaultQtyFmt, (qtyEdit * convRate));
            drUpdating["DiscAdj"] = chkDiscAdj.Checked;
            decimal disc;
            decimal.TryParse(txtDisc.Text, out disc);
            drUpdating["Discount"] = disc;

            if (chkDiscAdj.Checked)
                drUpdating["DiccountAmt"] = txtDiscAmt.Text;
            else
            {
                decimal recQty;
                decimal.TryParse(drUpdating["RecQty"].ToString(), out recQty);
                drUpdating["DiccountAmt"] = RoundAmt(DiscountUpdateAmt * recQty);
            }

            var taxType = ddlTaxType.SelectedItem.Value;

            drUpdating["TaxAdj"] = chkTaxAdj.Checked;
            drUpdating["TaxType"] = taxType;

            decimal taxRate;
            decimal.TryParse(txtTaxRate.Text, out taxRate);
            drUpdating["TaxRate"] = taxType == "N" ? 0m : taxRate;


            if (taxType != "N" && taxRate == 0m)
            {
                lbl_WarningMessage.Text = "Please set tax rate.";
                pop_Warning.ShowOnPageLoad = true;

                return;
            }

            decimal netAmt;
            decimal.TryParse(txtNetAmt.Text, out netAmt);
            drUpdating["NetAmt"] = netAmt;
            decimal taxAmt;
            decimal.TryParse(txtTaxAmt.Text, out taxAmt);
            drUpdating["TaxAmt"] = taxAmt;
            //decimal amt;
            //decimal.TryParse(seTotalAmt.Text.ToString(CultureInfo.InvariantCulture), out amt);
            //drUpdating["TotalAmt"] = amt;
            drUpdating["TotalAmt"] = lbl_TotalAmtDt.Text;
            decimal focEdit;
            decimal.TryParse(seFocEdit.Text, out focEdit);
            drUpdating["FOCQty"] = focEdit;
            //drUpdating["NetDrAcc"] = txtNetDrAcc.Text;
            //drUpdating["TaxDrAcc"] = txtTaxDrAcc.Text;
            //drUpdating["NetDrAcc"] = ddl_NetDrAcc.Text;
            //drUpdating["TaxDrAcc"] = ddl_TaxDrAcc.Text;
            drUpdating["Comment"] = txtDtrComment.Text;


            if (de_ExpiryDate.Text == string.Empty)
                drUpdating["ExpiryDate"] = DBNull.Value;
            else
                drUpdating["ExpiryDate"] = string.Format("{0:yyyy-MM-dd}", de_ExpiryDate.Date);

            // Added on: 24/08/2017, By: Fon
            drUpdating["CurrNetAmt"] = txt_CurrNetAmt.Text;
            drUpdating["CurrDiscAmt"] = txt_CurrDiscAmt.Text;
            drUpdating["CurrTaxAmt"] = txt_CurrTaxAmt.Text;
            drUpdating["CurrTotalAmt"] = lbl_CurrTotalAmtDt.Text;

            if (ddlProduct != null && (lblPoNo == null) || string.IsNullOrEmpty(lblPoNo.Text))
            {
                var productCode = (ddlProduct.Value.ToString().Contains(":"))
                    ? ddlProduct.Value.ToString().Split(new[] { ":" }, StringSplitOptions.None)[0]
                    : ddlProduct.Value.ToString();

                drUpdating["ProductCode"] = productCode;
            }

            AllocateExtraCost();


            //drUpdating.AcceptChanges();
            grd_RecEdit.DataSource = DsRecEdit.Tables[RecDt.TableName];
            grd_RecEdit.EditIndex = -1;
            grd_RecEdit.DataBind();
        }

        protected void grd_RecEdit_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var drDeleting = DsRecEdit.Tables[RecDt.TableName].Rows[e.RowIndex];

            if (drDeleting.RowState != DataRowState.Deleted)
            {
                DsRecEdit.Tables[RecDt.TableName].Rows[e.RowIndex].Delete();
            }

            RecEditMode = string.Empty;
            grd_RecEdit.DataSource = DsRecEdit.Tables[RecDt.TableName];
            grd_RecEdit.EditIndex = -1;
            grd_RecEdit.DataBind();

            if (grd_RecEdit.Rows.Count < 1)
                UIControl_BetweenAddItem();
        }

        #region GridDataRow

        private void UIControlProductItem(GridViewRowEventArgs e)
        {
            var lblProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
            var ddlProduct = e.Row.FindControl("ddl_Product") as ASPxComboBox;

            if (RecEditMode.Equals("NEW") || RecEditMode.Equals("EDIT"))
            {
                if (lblProductCode != null) lblProductCode.Visible = false;
                if (ddlProduct != null) ddlProduct.Visible = true;
            }
            else
            {
                if (lblProductCode != null) lblProductCode.Visible = true;
                if (ddlProduct != null) ddlProduct.Visible = false;
            }
        }

        private void GridDataRow(GridViewRowEventArgs e)
        {
            cmb_DeliPoint.Enabled = false; // When have item cannot change
            // Common parameter
            var productCodeDataItem = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();

            var productCode = productCodeDataItem.Contains(":")
                ? productCodeDataItem.Split(new string[] { }, StringSplitOptions.None)[0].Trim()
                : productCodeDataItem;

            #region ProductCode

            var lblProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
            var ddlProduct = e.Row.FindControl("ddl_Product") as ASPxComboBox;

            if (!string.IsNullOrEmpty(productCodeDataItem))
            {
                if (productCodeDataItem.Contains(":"))
                {
                    if (lblProductCode != null)
                    {
                        lblProductCode.Text = productCodeDataItem;
                        lblProductCode.ToolTip = lblProductCode.Text;
                    }
                    if (ddlProduct != null) ddlProduct.Value = productCodeDataItem;
                }
                else
                {
                    if (lblProductCode != null)
                    {
                        lblProductCode.Text = new StringBuilder().AppendFormat("{0} : {1} : {2}",
                            productCodeDataItem,
                            Product.GetName(productCodeDataItem, hf_ConnStr.Value),
                            Product.GetName2(productCodeDataItem, hf_ConnStr.Value)).ToString();
                        lblProductCode.ToolTip = lblProductCode.Text;
                        if (ddlProduct != null) ddlProduct.Value = lblProductCode.Text;
                    }
                }
            }

            UIControlProductItem(e);

            #endregion

            UIControlEditGrid_chk_DiscAdj(e);
            UIControlEditGrid_chk_TaxAdj(e);

            #region UnitCode

            var lblUnit = e.Row.FindControl("lbl_Unit") as Label;
            if (lblUnit != null)
            {
                lblUnit.Text = DataBinder.Eval(e.Row.DataItem, "UnitCode").ToString();
                lblUnit.ToolTip = lblUnit.Text;
            }

            #endregion

            #region RcvUnit

            var rcvUnit = DataBinder.Eval(e.Row.DataItem, "UnitCode").ToString();
            var lblRcvUnit = e.Row.FindControl("lbl_RcvUnit") as Label;
            if (lblRcvUnit != null)
            {
                lblRcvUnit.Text = rcvUnit;
                lblRcvUnit.ToolTip = lblRcvUnit.Text;
            }

            var ddlRcvUnit = e.Row.FindControl("ddl_RcvUnit") as ASPxComboBox;
            if (ddlRcvUnit != null)
            {
                ddlRcvUnit.DataSource = LookupUnit(productCode);
                ddlRcvUnit.DataBind();
                ddlRcvUnit.Value = rcvUnit;
            }

            #endregion

            #region RecQty

            var lblRecQty = e.Row.FindControl("lbl_RecQty") as Label;
            decimal recQty;
            decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "RecQty").ToString(), out recQty);

            if (lblRecQty != null)
            {
                lblRecQty.Text = string.Format(DefaultQtyFmt, recQty);
                lblRecQty.ToolTip = lblRecQty.Text;
                GrandRecAmt += recQty;
            }

            var seRecQtyEdit = e.Row.FindControl("se_RecQtyEdit") as ASPxSpinEdit;


            if (seRecQtyEdit != null)
            {
                seRecQtyEdit.Text = QtyRecUpdate == 0
                    ? string.Format(DefaultQtyFmt, recQty)
                    : string.Format(DefaultQtyFmt, QtyRecUpdate);

                seRecQtyEdit.DecimalPlaces = _default.DigitQty;
            }

            #endregion

            #region FocQty

            var lblFocQty = e.Row.FindControl("lbl_FocQty") as Label;
            decimal focQty;
            decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "FOCQty").ToString(), out focQty);

            if (lblFocQty != null)
            {
                lblFocQty.Text = string.Format(DefaultQtyFmt, focQty);
                lblFocQty.ToolTip = lblFocQty.Text;
            }

            var seFocEdit = e.Row.FindControl("se_FocEdit") as ASPxSpinEdit;


            if (seFocEdit != null)
            {
                seFocEdit.Text = string.Format(DefaultQtyFmt, focQty);

                seFocEdit.DecimalPlaces = _default.DigitQty;
            }

            #endregion

            #region Price

            var lblPrice = e.Row.FindControl("lbl_Price") as Label;
            decimal price;
            decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "Price").ToString(), out price);

            if (lblPrice != null)
            {
                lblPrice.Text = String.Format(DefaultPriceFmt, price);
                lblPrice.ToolTip = lblPrice.Text;
            }

            var sePriceEdit = e.Row.FindControl("se_PriceEdit") as ASPxSpinEdit;


            if (sePriceEdit != null)
            {
                sePriceEdit.Text = PriceUpdate == 0
                    ? string.Format(DefaultPriceFmt, price)
                    : string.Format(DefaultPriceFmt, PriceUpdate);

                sePriceEdit.DecimalPlaces = _default.DigitPrice;
            }

            #endregion




            decimal netAmt;
            decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "NetAmt").ToString(), out netAmt);
            GrandNetAmt += netAmt;
            decimal taxAmt;
            decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "TaxAmt").ToString(), out taxAmt);
            GrandTaxAmt += taxAmt;
            GrandTotal = GrandNetAmt + GrandTaxAmt;

            // Added on: 25/08/2017. By: Fon
            decimal currNetAmt;
            decimal currTaxAmt;
            decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "CurrNetAmt").ToString(), out currNetAmt);
            decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "CurrTaxAmt").ToString(), out currTaxAmt);
            currGrandNetAmt += currNetAmt;
            currGranTaxAmt += currTaxAmt;
            currGrandTotalAmt = currGrandNetAmt + currGranTaxAmt;
            // End Added;


            #region TotalAmt

            var lblTotalAmt = e.Row.FindControl("lbl_TotalAmt") as Label;
            decimal totalAmt;

            // Modified on: 05/02/2018, By: Fon, For: Following from P'Oat request.
            //decimal fieldTotalAmt;
            //decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt").ToString(), out fieldTotalAmt);

            decimal fieldTotal = RoundAmt(recQty * price);
            fieldTotal = currNetAmt + currTaxAmt;
            // End Modified.

            decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "TotalAmt").ToString(), out totalAmt);
            if (lblTotalAmt != null)
            {
                //lblTotalAmt.Text = totalAmt.ToString(CultureInfo.InvariantCulture);
                //lblTotalAmt.Text = fieldTotalAmt.ToString(CultureInfo.InvariantCulture);

                lblTotalAmt.Text = string.Format(DefaultAmtFmt, fieldTotal);
                lblTotalAmt.ToolTip = lblTotalAmt.Text;
            }

            var seTotalAmt = e.Row.FindControl("se_TotalAmt") as ASPxSpinEdit;
            if (seTotalAmt != null)
            {

                seTotalAmt.Text = string.Format(DefaultAmtFmt, fieldTotal);
                seTotalAmt.ToolTip = seTotalAmt.Text;
            }

            #endregion

            //Tax Type in Gridview Row.
            #region Tax Type

            var lbl_TaxType = e.Row.FindControl("lbl_TaxType") as Label;
            var taxType = DataBinder.Eval(e.Row.DataItem, "TaxType").ToString();

            if (lbl_TaxType != null)
            {
                lbl_TaxType.Text =
                    //new StringBuilder().AppendFormat("({0} Tax)", GetTaxTypeName(taxType)).ToString();
                    new StringBuilder().AppendFormat("({0} Tax)", GetTaxTypeName(taxType)).ToString();
                lbl_TaxType.ToolTip = lbl_TaxType.Text;
            }

            var lblStatus = e.Row.FindControl("lbl_Status") as Label;
            if (lblStatus != null)
            {
                lblStatus.Text = DataBinder.Eval(e.Row.DataItem, "Status").ToString();
                lblStatus.ToolTip = lblStatus.Text;
            }

            #endregion




            if (e.Row.FindControl("lbl_ExpiryDate") != null)
            {
                var lbl_ExpiryDate = e.Row.FindControl("lbl_ExpiryDate") as Label;
                if (string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "ExpiryDate").ToString()))
                    lbl_ExpiryDate.Text = string.Empty;
                else
                    lbl_ExpiryDate.Text = string.Format("{0:D}", (DateTime)DataBinder.Eval(e.Row.DataItem, "ExpiryDate"));

                lbl_ExpiryDate.ToolTip = lbl_ExpiryDate.Text;
            }

            if (e.Row.FindControl("de_ExpiryDate") != null)
            {
                var de_ExpiryDate = e.Row.FindControl("de_ExpiryDate") as ASPxDateEdit;
                if (!string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "ExpiryDate").ToString()))
                    de_ExpiryDate.Date = (DateTime)DataBinder.Eval(e.Row.DataItem, "ExpiryDate");
                de_ExpiryDate.ToolTip = de_ExpiryDate.Text;
            }

            if (e.Row.FindControl("lbl_ExtraCost") != null)
            {
                var lbl_ExtraCost = e.Row.FindControl("lbl_ExtraCost") as Label;
                lbl_ExtraCost.Text = DataBinder.Eval(e.Row.DataItem, "ExtraCost") == DBNull.Value
                        ? "0"
                        : string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "ExtraCost"));
                lbl_ExtraCost.ToolTip = lbl_ExtraCost.Text;
            }


            //-------------------------------Expand----------------------------------------

            #region Expand

            var lblDisc = e.Row.FindControl("lbl_Disc") as Label;
            decimal discount;
            decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "Discount").ToString(), out discount);

            if (lblDisc != null)
            {
                lblDisc.Text = string.Format(DefaultAmtFmt, discount);
                lblDisc.ToolTip = lblDisc.Text;
            }

            var txtDisc = e.Row.FindControl("txt_Disc") as TextBox;
            if (txtDisc != null)
            {
                txtDisc.Text = string.Format(DefaultAmtFmt, discount);
                txtDisc.ToolTip = txtDisc.Text;
            }

            var lblDiscAmt = e.Row.FindControl("lbl_DiscAmt") as Label;
            decimal diccountAmt;
            decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "DiccountAmt").ToString(), out diccountAmt);

            if (lblDiscAmt != null)
            {
                lblDiscAmt.Text = string.Format(DefaultAmtFmt, diccountAmt);
                lblDiscAmt.ToolTip = lblDiscAmt.Text;
            }

            var txtDiscAmt = e.Row.FindControl("txt_DiscAmt") as TextBox;
            if (txtDiscAmt != null)
            {
                txtDiscAmt.Text = string.Format(DefaultAmtFmt, diccountAmt);
                txtDiscAmt.ToolTip = txtDiscAmt.Text;
            }

            var lblReceive = e.Row.FindControl("lbl_Receive") as Label;
            if (lblReceive != null)
            {
                lblReceive.Text = string.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "RecQty"));
                lblReceive.ToolTip = lblReceive.Text;
            }

            var lblInvUnit = e.Row.FindControl("lbl_RcvUnit_Expand") as Label;
            if (lblInvUnit != null)
            {
                lblInvUnit.Text = DataBinder.Eval(e.Row.DataItem, "UnitCode").ToString();
                lblInvUnit.ToolTip = lblInvUnit.Text;
            }

            var lblConvertRate = e.Row.FindControl("lbl_ConvertRate") as Label;
            if (lblConvertRate != null)
            {
                lblConvertRate.Text = String.Format("{0:N6}", DataBinder.Eval(e.Row.DataItem, "Rate"));
                lblConvertRate.ToolTip = lblConvertRate.Text;
            }

            var lblBaseUnit = e.Row.FindControl("lbl_BaseUnit") as Label;
            if (lblBaseUnit != null)
            {
                lblBaseUnit.Text = ProdUnit.GetInvenUnit(productCode, hf_ConnStr.Value);
                lblBaseUnit.ToolTip = lblBaseUnit.Text;
            }

            if (lbl_TaxType != null)
            {
                lbl_TaxType.Text = GetTaxTypeName(taxType);
                lbl_TaxType.ToolTip = lbl_TaxType.Text;
            }

            var ddlTaxType = e.Row.FindControl("ddl_TaxType") as DropDownList;
            if (ddlTaxType != null)
                ddlTaxType.SelectedValue = taxType;

            var lblTaxRate = e.Row.FindControl("lbl_TaxRate") as Label;
            decimal taxRate;
            decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "TaxRate").ToString(), out taxRate);

            if (lblTaxRate != null)
            {
                lblTaxRate.Text = string.Format("{0:N}", taxRate);
                lblTaxRate.ToolTip = lblTaxRate.Text;
            }

            var txtTaxRate = e.Row.FindControl("txt_TaxRate") as TextBox;
            if (txtTaxRate != null)
                txtTaxRate.Text = String.Format("{0:N}", taxRate);

            #endregion

            #region Ori Currency
            // Added on: 22/08/2017, By: Fon
            decimal currDiscAmt, currTotalAmt;
            decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "CurrNetAmt").ToString(), out currNetAmt);
            decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "CurrDiscAmt").ToString(), out currDiscAmt);
            decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "CurrTaxAmt").ToString(), out currTaxAmt);
            decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt").ToString(), out currTotalAmt);

            // Currency Title
            if (e.Row.FindControl("lbl_CurrCurrDt") != null)
            {
                Label lbl_CurrCurrDt = (Label)e.Row.FindControl("lbl_CurrCurrDt");
                lbl_CurrCurrDt.Text = string.Format("( {0} )", ddl_Currency.Text.ToString());
            }
            if (e.Row.FindControl("lbl_BaseCurrDt") != null)
            {
                Label lbl_BaseCurrDt = (Label)e.Row.FindControl("lbl_BaseCurrDt");
                lbl_BaseCurrDt.Text = string.Format("( {0} )", baseCurrency);
            }

            // Net Amt
            if (e.Row.FindControl("lbl_CurrNetAmt") != null)
            {
                Label lbl_CurrNetAmt = (Label)e.Row.FindControl("lbl_CurrNetAmt");
                lbl_CurrNetAmt.Text = string.Format(DefaultAmtFmt, currNetAmt);
            }
            if (e.Row.FindControl("txt_CurrNetAmt") != null)
            {
                TextBox txt_CurrNetAmt = (TextBox)e.Row.FindControl("txt_CurrNetAmt");
                txt_CurrNetAmt.Text = string.Format(DefaultAmtFmt, currNetAmt);
            }

            // Discount Amount
            if (e.Row.FindControl("lbl_CurrDiscAmt") != null)
            {
                Label lbl_CurrDiscAmt = (Label)e.Row.FindControl("lbl_CurrDiscAmt");
                lbl_CurrDiscAmt.Text = string.Format(DefaultAmtFmt, currDiscAmt);
            }
            if (e.Row.FindControl("txt_CurrDiscAmt") != null)
            {
                TextBox txt_CurrDiscAmt = (TextBox)e.Row.FindControl("txt_CurrDiscAmt");
                txt_CurrDiscAmt.Text = string.Format(DefaultAmtFmt, currDiscAmt);
            }

            // Tax Amount
            if (e.Row.FindControl("lbl_CurrTaxAmt") != null)
            {
                Label lbl_CurrTaxAmt = (Label)e.Row.FindControl("lbl_CurrTaxAmt");
                lbl_CurrTaxAmt.Text = string.Format(DefaultAmtFmt, currTaxAmt);
            }
            if (e.Row.FindControl("txt_CurrTaxAmt") != null)
            {
                TextBox txt_CurrTaxAmt = (TextBox)e.Row.FindControl("txt_CurrTaxAmt");
                txt_CurrTaxAmt.Text = string.Format(DefaultAmtFmt, currTaxAmt);
            }

            // Total Amount
            if (e.Row.FindControl("lbl_CurrTotalAmtDt") != null)
            {
                Label lbl_CurrTotalAmtDt = (Label)e.Row.FindControl("lbl_CurrTotalAmtDt");
                lbl_CurrTotalAmtDt.Text = string.Format(DefaultAmtFmt, currTotalAmt);
            }
            #endregion

            #region Base Currency
            var lblNetAmt = e.Row.FindControl("lbl_NetAmt") as Label;

            if (lblNetAmt != null)
            {
                lblNetAmt.Text = String.Format(DefaultAmtFmt, netAmt);
                lblNetAmt.ToolTip = lblNetAmt.Text;
            }

            var txtNetAmt = e.Row.FindControl("txt_NetAmt") as TextBox;
            if (txtNetAmt != null)
                txtNetAmt.Text = String.Format(DefaultAmtFmt, netAmt);

            var lblTaxAmt = e.Row.FindControl("lbl_TaxAmt") as Label;

            if (lblTaxAmt != null)
            {
                lblTaxAmt.Text = String.Format(DefaultAmtFmt, taxAmt);
                lblTaxAmt.ToolTip = lblTaxAmt.Text;
            }

            var lblTotalAmtDt = e.Row.FindControl("lbl_TotalAmtDt") as Label;

            if (lblTotalAmtDt != null)
            {
                lblTotalAmtDt.Text = String.Format(DefaultAmtFmt, totalAmt);
                lblTotalAmtDt.ToolTip = lblTotalAmtDt.Text;
            }
            #endregion

            // Added on: 02/02/2018, By: Fon
            var lblNm_DiscAmt = e.Row.FindControl("lblNm_DiscAmt") as Label;
            if (lblNm_DiscAmt != null)
            {
                lblNm_DiscAmt.Text = string.Format("(" + DefaultAmtFmt + ")", currDiscAmt);
            }

            var lblNm_NetAmt = e.Row.FindControl("lblNm_NetAmt") as Label;
            if (lblNm_NetAmt != null)
            {
                lblNm_NetAmt.Text = string.Format(DefaultAmtFmt, currNetAmt);
            }
            // End Added.

            var lblReceiver = e.Row.FindControl("lbl_Receiver") as Label;
            if (lblReceiver != null)
            {
                lblReceiver.Text = DsRecEdit.Tables[Rec.TableName].Rows[0]["CreatedBy"].ToString();
                lblReceiver.ToolTip = lblReceiver.Text;
            }

            var lblBaseQty = e.Row.FindControl("lbl_BaseQty") as Label;
            if (lblBaseQty != null)
            {
                if (DataBinder.Eval(e.Row.DataItem, "Rate").ToString() != string.Empty)
                {
                    var strNewUnit = ProdUnit.GetInvenUnit(productCode, hf_ConnStr.Value);
                    var strOldUnit = rcvUnit.ToString();
                    var decBaseQty = ProdUnit.GetQtyAfterChangeUnit(productCode, strNewUnit, strOldUnit, recQty, hf_ConnStr.Value);

                    lblBaseQty.Text = String.Format(DefaultQtyFmt, decBaseQty);
                }

                lblBaseQty.ToolTip = lblBaseQty.Text;
            }

            var lblDtrComment = e.Row.FindControl("lbl_DtrComment") as Label;
            var comment = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();

            if (lblDtrComment != null)
            {
                lblDtrComment.Text = comment;
                lblDtrComment.ToolTip = lblDtrComment.Text;
            }

            var txtDtrComment = e.Row.FindControl("txt_DtrComment") as TextBox;
            if (txtDtrComment != null)
            {
                txtDtrComment.Text = comment;
            }

            var txtTaxAmt = e.Row.FindControl("txt_TaxAmt") as TextBox;

            if (txtTaxAmt != null)
            {
                txtTaxAmt.Text = String.Format(DefaultAmtFmt, taxAmt);
            }

            #region chk_TaxAdj

            var chkTaxAdj = e.Row.FindControl("chk_TaxAdj") as CheckBox;
            bool taxAdj;
            bool.TryParse(DataBinder.Eval(e.Row.DataItem, "TaxAdj").ToString(), out taxAdj);
            if (chkTaxAdj != null)
            {
                chkTaxAdj.Checked = taxAdj;

                if (chkTaxAdj.Checked)
                {
                    //if (txtNetAmt != null)
                    //    txtNetAmt.Enabled = true;

                    //if (txtTaxAmt != null)
                    //    txtTaxAmt.Enabled = true;

                    //if (txtTaxRate != null)
                    //    txtTaxRate.Enabled = true;

                    //if (ddlTaxType != null)
                    //    ddlTaxType.Enabled = true;

                    //if (ddlTaxType != null)
                    //{
                    //    if (ddlTaxType.SelectedItem.Value == "N")
                    //    {
                    //        txtTaxAmt.Enabled = false;
                    //        txtTaxRate.Enabled = false;
                    //    }
                    //}


                }
                else
                {
                    //if (txtNetAmt != null)
                    //    txtNetAmt.Enabled = false;

                    //if (txtTaxAmt != null)
                    //    txtTaxAmt.Enabled = false;

                    //if (txtTaxRate != null)
                    //    txtTaxRate.Enabled = false;

                    //if (ddlTaxType != null)
                    //    ddlTaxType.Enabled = false;


                }
            }

            #endregion

            #region chk_DiscAdj

            var chkDiscAdj = e.Row.FindControl("chk_DiscAdj") as CheckBox;
            bool discAdj;
            bool.TryParse(DataBinder.Eval(e.Row.DataItem, "DiscAdj").ToString(), out discAdj);

            if (chkDiscAdj != null)
            {
                chkDiscAdj.Checked = discAdj;

                if (chkDiscAdj.Checked)
                {
                    if (txtDisc != null)
                        txtDisc.Enabled = true;

                    if (txtDiscAmt != null)
                        txtDiscAmt.Enabled = true;
                }
                else
                {
                    if (txtDisc != null)
                        txtDisc.Enabled = false;

                    if (txtDiscAmt != null)
                        txtDiscAmt.Enabled = false;
                }
            }

            #endregion

            var lblNetAcc = e.Row.FindControl("lbl_NetAcc") as Label;
            var netDrAcc = DataBinder.Eval(e.Row.DataItem, "NetDrAcc").ToString();
            if (lblNetAcc != null)
            {
                lblNetAcc.Text = netDrAcc;
                lblNetAcc.ToolTip = lblNetAcc.Text;
            }

            //var txtNetDrAcc = e.Row.FindControl("txt_NetDrAcc") as TextBox;
            //if (txtNetDrAcc != null)
            //{
            //    txtNetDrAcc.Text = string.IsNullOrEmpty(NetAcCode) ? netDrAcc : NetAcCode;
            //}
            if (e.Row.FindControl("ddl_NetDrAcc") != null)
            {
                ASPxComboBox ddl_NetDrAcc = (ASPxComboBox)e.Row.FindControl("ddl_NetDrAcc");
                ddl_NetDrAcc.DataSource = dsImport.Tables["AccoundCode"];
                ddl_NetDrAcc.TextField = "Code";
                ddl_NetDrAcc.ValueField = "Code";
                ddl_NetDrAcc.ValueType = typeof(string);
                ddl_NetDrAcc.DataBind();
            }

            var lblTaxAcc = e.Row.FindControl("lbl_TaxAcc") as Label;
            var taxDrAcc = DataBinder.Eval(e.Row.DataItem, "TaxDrAcc").ToString();
            if (lblTaxAcc != null)
            {
                lblTaxAcc.Text = taxDrAcc;
                lblTaxAcc.ToolTip = lblTaxAcc.Text;
            }

            //var txtTaxDrAcc = e.Row.FindControl("txt_TaxDrAcc") as TextBox;
            //if (txtTaxDrAcc != null)
            //{
            //    txtTaxDrAcc.Text = string.IsNullOrEmpty(TaxAcCode) ? taxDrAcc : TaxAcCode;
            //}
            if (e.Row.FindControl("ddl_TaxDrAcc") != null)
            {
                ASPxComboBox ddl_TaxDrAcc = (ASPxComboBox)e.Row.FindControl("ddl_TaxDrAcc");
                ddl_TaxDrAcc.DataSource = dsImport.Tables["AccoundCode"];
                ddl_TaxDrAcc.TextField = "Code";
                ddl_TaxDrAcc.ValueField = "Code";
                ddl_TaxDrAcc.ValueType = typeof(string);
                ddl_TaxDrAcc.DataBind();
            }


            //************************** Display Stock Movement ****************************

            #region  Display Stock Movement

            var ucStockMovement = e.Row.FindControl("uc_StockMovement") as BlueLedger.PL.PC.StockMovement;
            var recNo = DataBinder.Eval(e.Row.DataItem, "RecNo").ToString();
            var recDtNo = DataBinder.Eval(e.Row.DataItem, "RecDtNo").ToString();

            if (ucStockMovement != null)
            {
                ucStockMovement.HdrNo = recNo;
                ucStockMovement.DtNo = recDtNo;
                ucStockMovement.ConnStr = hf_ConnStr.Value;
                ucStockMovement.DataBind();
            }

            #endregion

            // Display Stock Summary --------------------------------------------------------------   

            #region  Display Stock Summary

            if (DsRecEdit.Tables[PrDt.TableName] != null)
            {
                DsRecEdit.Tables[PrDt.TableName].Clear();
            }

            var get = PrDt.GetStockSummary(DsRecEdit, productCode,
                DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(),
                de_RecDate.Text, hf_ConnStr.Value);

            if (get)
            {
                var drStockSummary = DsRecEdit.Tables[PrDt.TableName].Rows[0];

                if (e.Row.FindControl("lbl_OnHand") != null)
                {
                    var lblOnHand = e.Row.FindControl("lbl_OnHand") as Label;
                    lblOnHand.Text = !string.IsNullOrEmpty(drStockSummary["OnHand"].ToString())
                        ? String.Format(DefaultQtyFmt, drStockSummary["OnHand"])
                        : string.Format(DefaultQtyFmt, 0);

                    lblOnHand.ToolTip = lblOnHand.Text;
                }

                if (e.Row.FindControl("lbl_OnOrder") != null)
                {
                    var lblOnOrder = e.Row.FindControl("lbl_OnOrder") as Label;

                    lblOnOrder.Text = !string.IsNullOrEmpty(drStockSummary["OnOrder"].ToString())
                        ? string.Format(DefaultQtyFmt, drStockSummary["OnOrder"])
                        : string.Format(DefaultQtyFmt, 0);

                    lblOnOrder.ToolTip = lblOnOrder.Text;
                }

                if (e.Row.FindControl("lbl_Reorder") != null)
                {
                    var lblReorder = e.Row.FindControl("lbl_Reorder") as Label;

                    lblReorder.Text = drStockSummary["Reorder"].ToString() != string.Empty
                        ? String.Format(DefaultQtyFmt, drStockSummary["Reorder"])
                        : string.Format(DefaultQtyFmt, 0);

                    lblReorder.ToolTip = lblReorder.Text;
                }

                if (e.Row.FindControl("lbl_Restock") != null)
                {
                    var lblRestock = e.Row.FindControl("lbl_Restock") as Label;

                    lblRestock.Text = drStockSummary["ReStock"].ToString() != string.Empty
                        ? String.Format(DefaultQtyFmt, drStockSummary["ReStock"])
                        : string.Format(DefaultQtyFmt, 0);

                    lblRestock.ToolTip = lblRestock.Text;
                }

                if (e.Row.FindControl("lbl_LastPrice") != null)
                {
                    var lblLastPrice = e.Row.FindControl("lbl_LastPrice") as Label;

                    lblLastPrice.Text = drStockSummary["LastPrice"].ToString() != string.Empty
                        ? String.Format(DefaultAmtFmt, drStockSummary["LastPrice"])
                        : string.Format(DefaultAmtFmt, 0);

                    lblLastPrice.ToolTip = lblLastPrice.Text;
                }

                if (e.Row.FindControl("lbl_LastVendor") != null)
                {
                    var lblLastVendor = e.Row.FindControl("lbl_LastVendor") as Label;
                    lblLastVendor.Text = drStockSummary["LastVendor"].ToString();
                    lblLastVendor.ToolTip = lblLastVendor.Text;
                }
            }

            if (string.IsNullOrEmpty(de_RecDate.Text))
                de_RecDate.Date = DateTime.Now;

            #endregion
        }

        #region CheckBox

        private void UIControlEditGrid_chk_DiscAdj(GridViewRowEventArgs e)
        {
            var chkDiscAdj = e.Row.FindControl("chk_DiscAdj") as CheckBox;

            if (chkDiscAdj != null)
            {
                chkDiscAdj.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "DiscAdj") == DBNull.Value
                    ? false
                    : DataBinder.Eval(e.Row.DataItem, "DiscAdj"));
                #region comment
                //var txtDiscAmt = e.Row.FindControl("txt_DiscAmt") as TextBox;
                //var txtDisc = e.Row.FindControl("txt_Disc") as TextBox;
                //if (chkDiscAdj.Checked)
                //{
                //    if (txtDisc != null)
                //        txtDisc.Enabled = true;

                //    if (txtDiscAmt != null)
                //        txtDiscAmt.Enabled = true;
                //}
                //else
                //{
                //    if (txtDisc != null)
                //        txtDisc.Enabled = false;

                //    if (txtDiscAmt != null)
                //        txtDiscAmt.Enabled = false;
                //}
                #endregion

                // Modified on: 22/08/2017, By: Fon
                TextBox txt_Disc = new TextBox();
                TextBox txt_DiscAmt = new TextBox();
                TextBox txt_CurrDiscAmt = new TextBox();
                bool discAdjChecked = chkDiscAdj.Checked;

                if (e.Row.FindControl("txt_Disc") != null)
                {
                    txt_Disc = (TextBox)e.Row.FindControl("txt_Disc");
                    txt_Disc.Enabled = discAdjChecked;
                }

                // Comment on: 01/02/2018, By: Fon, For: Following from P' Oat guide.
                //if (e.Row.FindControl("txt_DiscAmt") != null)
                //{
                //    txt_DiscAmt = (TextBox)e.Row.FindControl("txt_DiscAmt");
                //    txt_DiscAmt.Enabled = discAdjChecked;
                //}
                // End Comment.

                if (e.Row.FindControl("txt_CurrDiscAmt") != null)
                {
                    txt_CurrDiscAmt = (TextBox)e.Row.FindControl("txt_CurrDiscAmt");
                    txt_CurrDiscAmt.Enabled = discAdjChecked;
                }
                // End Modified.
            }
        }

        private void UIControlEditGrid_chk_TaxAdj(GridViewRowEventArgs e)
        {
            var chkTaxAdj = e.Row.FindControl("chk_TaxAdj") as CheckBox;
            if (chkTaxAdj != null)
            {
                chkTaxAdj.Checked =
                    Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "TaxAdj") != DBNull.Value
                                    && Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "TaxAdj")));


                DropDownList ddl_TaxType = new DropDownList();
                TextBox txt_TaxRate = new TextBox();
                TextBox txt_TaxAmt = new TextBox();
                TextBox txt_CurrTaxAmt = new TextBox();
                bool taxAdjCheked = chkTaxAdj.Checked;

                if (e.Row.FindControl("ddl_TaxType") != null)
                {
                    ddl_TaxType = (DropDownList)e.Row.FindControl("ddl_TaxType");
                    txt_TaxRate = (TextBox)e.Row.FindControl("txt_TaxRate");
                    txt_CurrTaxAmt = (TextBox)e.Row.FindControl("txt_CurrTaxAmt");
                    txt_TaxAmt = (TextBox)e.Row.FindControl("txt_TaxAmt");

                    ddl_TaxType.Enabled = taxAdjCheked;
                    txt_TaxRate.Enabled = false;
                    txt_CurrTaxAmt.Enabled = false;
                    txt_TaxAmt.Enabled = false;

                    if (ddl_TaxType.SelectedItem.Value.ToUpper() != "N")
                    {
                        txt_TaxRate.Enabled = taxAdjCheked;
                        txt_CurrTaxAmt.Enabled = taxAdjCheked;

                        // Comment on: 01/02/2018, By: Fon, For: Following from P' Oat guide.
                        //txt_TaxAmt.Enabled = taxAdjCheked;
                        // End Comment.
                    }

                }
                // End if find dropdownlist
            }
        }

        #endregion

        #endregion

        #region GridFooter

        private void GridFooter(GridViewRowEventArgs e)
        {
            var lblGrandTotalAmt = (Label)e.Row.FindControl("lbl_GrandTotalAmt");
            var grandTotalNet = (Label)e.Row.FindControl("lbl_GrandTotalNet");
            var lblGrandTotalTax = (Label)e.Row.FindControl("lbl_GrandTotalTax");

            if (lblGrandTotalAmt != null)
                lblGrandTotalAmt.Text = String.Format(DefaultAmtFmt, GrandTotal);

            if (grandTotalNet != null)
                grandTotalNet.Text = String.Format(DefaultAmtFmt, GrandNetAmt);

            if (lblGrandTotalTax != null)
                lblGrandTotalTax.Text = String.Format(DefaultAmtFmt, GrandTaxAmt);

            // Added on: 24/08/2017
            if (e.Row.FindControl("lbl_CurrGrandTitle") != null)
            {
                Label lbl_CurrGrandTitle = (Label)e.Row.FindControl("lbl_CurrGrandTitle");
                lbl_CurrGrandTitle.Text = string.Format("( {0} )", ddl_Currency.Value.ToString());
            }
            if (e.Row.FindControl("lbl_BaseGrandTitle") != null)
            {
                Label lbl_BaseGrandTitle = (Label)e.Row.FindControl("lbl_BaseGrandTitle");
                lbl_BaseGrandTitle.Text = string.Format("( {0} )", baseCurrency);
            }

            if (e.Row.FindControl("lbl_CurrGrandTotalNet") != null)
            {
                Label lbl_CurrGrandTotalNet = (Label)e.Row.FindControl("lbl_CurrGrandTotalNet");
                lbl_CurrGrandTotalNet.Text = string.Format(DefaultAmtFmt, currGrandNetAmt);
            }

            if (e.Row.FindControl("lbl_CurrGrandTotalTax") != null)
            {
                Label lbl_CurrGrandTotalTax = (Label)e.Row.FindControl("lbl_CurrGrandTotalTax");
                lbl_CurrGrandTotalTax.Text = string.Format(DefaultAmtFmt, currGranTaxAmt);
            }

            if (e.Row.FindControl("lbl_CurrGrandTotalAmt") != null)
            {
                Label lbl_CurrGrandTotalAmt = (Label)e.Row.FindControl("lbl_CurrGrandTotalAmt");
                lbl_CurrGrandTotalAmt.Text = string.Format(DefaultAmtFmt, currGrandTotalAmt);
            }
            // End Added.
        }

        #endregion

        #endregion

        #region grd_PoList events

        protected void grd_PoList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        #endregion

        #region Button events

        protected void btn_AddItem_Click(object sender, EventArgs e)
        {
            // Validate Date:Delivery Point:Invoice Date:Invoice#:Location
            if (ValidateAddItem())
            {
                // Add item to Grid
                NewItem();
                RecEditMode = "NEW";
                grd_RecEdit.DataSource = DsRecEdit.Tables[RecDt.TableName];
                grd_RecEdit.DataBind();
                grd_RecEdit.EditIndex = grd_RecEdit.Rows[grd_RecEdit.Rows.Count - 1].RowIndex;
                grd_RecEdit.DataBind();

                UIControl_BetweenAddItem();

                // Added on: 05/09/2017, By: Fon
                ddl_Currency.Enabled = false;
                txt_ExRateAu.Enabled = false;
            }
        }

        private void UIControl_BetweenAddItem()
        {
            //hong visible button 20130909
            //btn_AddItem.Enabled = !isProcess;
            //btn_Save.Enabled = !isProcess;
            //btn_Commit.Enabled = !isProcess;
            //de_RecDate.Enabled = !isProcess;
            //de_InvDate.Enabled = !isProcess;
            //cmb_DeliPoint.Enabled = !isProcess;
            //ddl_Location_Stamp.Enabled = !isProcess;
            //ddl_Vendor.Enabled = !isProcess;
        }

        private void NewItem()
        {
            if (DsRecEdit != null)
            {
                var rowId = DsRecEdit.Tables[RecDt.TableName].Rows.Count;
                var recDtNo = (rowId == 0) ? 1 : Convert.ToInt32(DsRecEdit.Tables[RecDt.TableName].Rows[rowId - 1]["RecDtNo"].ToString()) + 1;

                DsRecEdit.Tables[Rec.TableName].NewRow();
                var drRecDt = DsRecEdit.Tables[RecDt.TableName].NewRow();
                drRecDt["RecNo"] = RecNo;
                //drRecDt["RecDtNo"] = DsRecEdit.Tables[RecDt.TableName].Rows.Count + 1;
                drRecDt["RecDtNo"] = recDtNo;
                drRecDt["LocationCode"] = LocationStamp;
                drRecDt["ProductCode"] = string.Empty;
                drRecDt["UnitCode"] = string.Empty;
                drRecDt["UnitCode"] = string.Empty;
                drRecDt["Rate"] = 0;
                drRecDt["OrderQty"] = 0;
                drRecDt["FOCQty"] = 0;
                drRecDt["RecQty"] = 0;
                drRecDt["FOCQty"] = 0;
                drRecDt["Price"] = 0;
                drRecDt["Discount"] = 0;
                drRecDt["TaxType"] = "N";
                drRecDt["TaxRate"] = 0;
                drRecDt["TaxAdj"] = false;
                drRecDt["Descen"] = string.Empty;
                drRecDt["Descll"] = string.Empty;
                drRecDt["DiccountAmt"] = 0;
                drRecDt["NetAmt"] = 0;
                drRecDt["TaxAmt"] = 0;
                drRecDt["TotalAmt"] = 0;
                drRecDt["PoNo"] = DBNull.Value;
                drRecDt["PoDtNo"] = DBNull.Value;
                drRecDt["NetDrAcc"] = string.Empty;
                drRecDt["TaxDrAcc"] = string.Empty;
                drRecDt["Status"] = DBNull.Value;
                drRecDt["ExportStatus"] = false;
                drRecDt["RcvUnit"] = string.Empty;

                // Added on: 22/08/2017, By: Fon
                drRecDt["CurrNetAmt"] = 0;
                drRecDt["CurrDiscAmt"] = 0;
                drRecDt["CurrTaxAmt"] = 0;
                drRecDt["CurrTotalAmt"] = 0;

                // Added on: 05/02/2018, By: Fon
                drRecDt["DiscAdj"] = false;

                // End Added.

                DsRecEdit.Tables[RecDt.TableName].Rows.Add(drRecDt);
            }
        }

        protected void btn_AddPo_Click(object sender, EventArgs e)
        {
        }

        protected void btn_WarningPeriod_Click(object sender, EventArgs e)
        {
            Response.Redirect("RecLst.aspx");
        }

        protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        protected void btn_WarningDelete_Click(object sender, EventArgs e)
        {
            pop_WarningDelete.ShowOnPageLoad = false;
        }

        protected void btn_ConfirmSave_Click(object sender, EventArgs e)
        {
            var strStartDate = period.GetStartDate(Convert.ToDateTime(de_RecDate.Date), hf_ConnStr.Value);
            var strEndDate = period.GetEndDate(Convert.ToDateTime(de_RecDate.Date), hf_ConnStr.Value);

            if (strStartDate.ToString() != string.Empty &
                strEndDate.ToString() != string.Empty)
            {
                SaveAndCommit("Received");
            }
            else
            {
                pop_ConfirmSave.ShowOnPageLoad = false;
                pop_WarningPeriod.ShowOnPageLoad = true;
            }
        }

        protected void btn_CancelSave_Click(object sender, EventArgs e)
        {
            pop_ConfirmSave.ShowOnPageLoad = false;

            if (Mode.ToUpper() != "EDIT")
            {
                //Set ViewState to false on 04/04/2012
                btn_AddPo.Enabled = false;
            }
        }

        protected void btn_ConfirmCommit_Click(object sender, EventArgs e)
        {
            var strStartDate = period.GetStartDate(Convert.ToDateTime(de_RecDate.Date), hf_ConnStr.Value);
            var strEndDate = period.GetEndDate(Convert.ToDateTime(de_RecDate.Date), hf_ConnStr.Value);

            if (strStartDate != string.Empty & strEndDate != string.Empty)
            {
                SaveAndCommit("Committed");
            }
            else
            {
                pop_ConfirmSave.ShowOnPageLoad = false;
                pop_WarningPeriod.ShowOnPageLoad = true;
            }
        }

        protected void btn_CancelCommit_Click(object sender, EventArgs e)
        {
            pop_ConfirmCommit.ShowOnPageLoad = false;

            if (Mode.ToUpper() != "EDIT")
            {
                //Set ViewState to false on 04/04/2012
                btn_AddPo.Enabled = false;
            }
        }

        protected void btn_PopUpOK_Click(object sender, EventArgs e)
        {
        }

        protected void btn_ConfirmLocation_Click(object sender, EventArgs e)
        {
        }

        #endregion

        #endregion



        #region Method

        private bool ValidateAddItem()
        {
            de_RecDate.Validate();
            cmb_DeliPoint.Validate();
            de_InvDate.Validate();
            ddl_Location_Stamp.Validate();
            ddl_Vendor.Validate();

            if (ddl_Location_Stamp.Value == null)
                return false;

            if (ddl_Vendor.Value == null)
                return false;

            if (de_RecDate.Date.Equals(DateTime.MinValue))
                return false;

            if (cmb_DeliPoint.Text.Equals(string.Empty))
                return false;

            if (de_InvDate.Date.Equals(DateTime.MinValue))
                return false;

            return true;
        }

        private void InitialPage()
        {
            de_RecDate.Date = DateTime.Now;
            de_InvDate.Date = DateTime.Now;

            //Reset dataset session
            DsRecEdit = null;
            RecDtNo = 0;
            txt_RecNo.Visible = false;
            rdb_ExtraCostByQty.Checked = true;


            //if (Request.QueryString["ID"] != null)
            if (Mode.ToUpper() == "EDIT")
            {
                var msgError = string.Empty;

                // Get invoice no from HTTP query string
                var recNo = Request.QueryString["ID"];

                txt_RecNo.Visible = false;
                txt_RecNo.Text = string.Empty;

                Rec.GetListByRecNo(DsRecEdit, ref msgError, recNo, hf_ConnStr.Value);
                RecDt.GetListByRecNo(DsRecEdit, recNo, hf_ConnStr.Value);

                #region Mode: Edit
                if (Mode.ToUpper() == "EDIT")
                {
                    RecEditMode = string.Empty; // set for initial

                    if (DsRecEdit != null)
                    {
                        var drRecEdit = DsRecEdit.Tables[Rec.TableName].Rows[0];
                        var drRecDtEdit = DsRecEdit.Tables[RecDt.TableName].Rows[0];
                        txt_RecNo.Visible = true;
                        txt_RecNo.Text = drRecEdit["RecNo"].ToString();
                        de_RecDate.Date = DateTime.Parse(drRecEdit["RecDate"].ToString());
                        de_RecDate.Enabled = false;
                        lbl_DocStatus.Text = drRecEdit["DocStatus"].ToString();

                        // Modified on: 17/08/2017. By: Fon
                        //lbl_ExRateAu.Text = (drRecEdit["ExRateAudit"].ToString() == string.Empty
                        //    ? "0.0000"
                        //    : drRecEdit["ExRateAudit"].ToString());
                        //lbl_Currency.Text = (drRecEdit["CurrencyCode"].ToString() == string.Empty
                        //    ? "THB"
                        //    : drRecEdit["CurrencyCode"].ToString());
                        txt_ExRateAu.Text = (drRecEdit["ExRateAudit"].ToString() == string.Empty
                            ? "0.0000"
                            : drRecEdit["ExRateAudit"].ToString());
                        ddl_Currency.Value = (drRecEdit["CurrencyCode"].ToString() == string.Empty
                            ? "THB"
                            : drRecEdit["CurrencyCode"].ToString());
                        // End Modified.

                        lbl_Receiver.Text = drRecEdit["CreatedBy"].ToString();
                        lbl_VendorCode.Text = drRecEdit["VendorCode"].ToString();
                        var vendor = new Blue.BL.AP.Vendor();
                        lbl_VendorNm.Text =
                            new StringBuilder(" : ").Append(vendor.GetName(drRecEdit["VendorCode"].ToString(),
                                hf_ConnStr.Value)).ToString();
                        ddl_Vendor.Value = lbl_VendorCode.Text;
                        txt_Desc.Text = drRecEdit["Description"].ToString();
                        txt_InvNo.Text = drRecEdit["InvoiceNo"].ToString();
                        de_InvDate.Date = (drRecEdit["InvoiceDate"] == DBNull.Value
                            ? DateTime.MinValue
                            : DateTime.Parse(drRecEdit["InvoiceDate"].ToString()));
                        chk_CashConsign.Checked = bool.Parse(drRecEdit["IsCashConsign"].ToString());

                        cmb_DeliPoint.Value = drRecEdit["DeliPoint"];
                        cmb_DeliPoint.Enabled = false;
                        ddl_Vendor.Visible = false;
                        var deliPoint = new Blue.BL.Option.Inventory.DeliveryPoint();
                        deliPoint.GetList(DsRecEdit, hf_ConnStr.Value);
                        cmb_DeliPoint.DataSource = DsRecEdit.Tables[deliPoint.TableName];
                        cmb_DeliPoint.ValueField = "DptCode";
                        cmb_DeliPoint.TextField = "Name";
                        cmb_DeliPoint.DataBind();
                        ddl_Location_Stamp.Value = drRecDtEdit["LocationCode"];

                        se_TotalExtraCost.Enabled = drRecEdit.Table.Columns.Contains("TotalExtraCost");
                        if (se_TotalExtraCost.Enabled)
                        {
                            se_TotalExtraCost.Value = string.IsNullOrEmpty(drRecEdit["TotalExtraCost"].ToString()) ? 0 : Convert.ToDecimal(drRecEdit["TotalExtraCost"].ToString());
                            if (drRecEdit["ExtraCostBy"].ToString().ToUpper() == "A")
                                rdb_ExtraCostByAmt.Checked = true;
                            else
                                rdb_ExtraCostByQty.Checked = true;
                        }

                    }
                    ddl_Location_Stamp.Enabled = false;
                    if (DsRecEdit != null) grd_RecEdit.DataSource = DsRecEdit.Tables[RecDt.TableName];
                    grd_RecEdit.DataBind();
                }
                #endregion
            }
            else
            {
                // New 
                CreateRecHeader();
                CreateRecDetail();
            }

            // Create Schema for ExtraCost
            string id = Request.QueryString["ID"] != null ? Request.QueryString["ID"].ToString() : string.Empty;
            DataTable dt = new DataTable();
            dt = Rec.DbExecuteQuery(string.Format("SELECT * FROM PC.RecExtCost WHERE RecNo = '{0}'", id), null, hf_ConnStr.Value);
            dt.TableName = recExtCost;
            DsRecEdit.Tables.Add(dt);

            if (dt.Rows.Count > 0)
                lastExtDtNo = (int)dt.Rows[dt.Rows.Count - 1]["DtNo"];
            else
                lastExtDtNo = 0;



        }

        private void CreateRecHeader()
        {
            string defaultCurrency = Config.GetConfigValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value);
            Rec.GetStructure(DsRecEdit, hf_ConnStr.Value);

            var drNew = DsRecEdit.Tables[Rec.TableName].NewRow();

            drNew["RecNo"] = Rec.GetNewID(ServerDateTime, hf_ConnStr.Value);
            RecNo = drNew["RecNo"].ToString();
            drNew["RecDate"] = ServerDateTime;
            drNew["CreatedDate"] = ServerDateTime;
            drNew["CreatedBy"] = LoginInfo.LoginName;
            drNew["UpdatedDate"] = ServerDateTime;
            drNew["UpdatedBy"] = LoginInfo.LoginName;

            // initial value
            lbl_Receiver.Text = LoginInfo.LoginName;

            // Modifed on: 17/08/2017, By: Fon
            //lbl_Currency.Text = Config.GetConfigValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value);
            //lbl_ExRateAu.Text = "1.000000";
            ddl_Currency.Value = defaultCurrency;
            txt_ExRateAu.Text = Convert.ToString(currency.GetLastCurrencyRate(defaultCurrency, DateTime.Now, LoginInfo.ConnStr));

            // 
            var deliPoint = new Blue.BL.Option.Inventory.DeliveryPoint();
            deliPoint.GetList(DsRecEdit, hf_ConnStr.Value);
            if (DsRecEdit.Tables[deliPoint.TableName].Rows.Count > 0)
            {
                cmb_DeliPoint.Text = string.Format("{0} : {1}",
                    DsRecEdit.Tables[deliPoint.TableName].Rows[0]["DptCode"],
                    DsRecEdit.Tables[deliPoint.TableName].Rows[0]["Name"]);
            }
            // End Modified.

            // Add new row
            DsRecEdit.Tables[Rec.TableName].Rows.Add(drNew);

            // Get Schema.
            RecDt.GetStructure(DsRecEdit, hf_ConnStr.Value);



        }

        private void CreateRecDetail()
        {
            var recDt = new Blue.BL.PC.REC.RECDt();
            // Get Schema.
            recDt.GetStructure(DsRecEdit, hf_ConnStr.Value);
        }

        public DataTable LookupUnit(string productCode)
        {
            if (ViewState["LookupUnit"] == null)
                ViewState["LookupUnit"] = ProdUnit.GetLookUp_ProductCode(productCode, hf_ConnStr.Value);

            return ViewState["LookupUnit"] as DataTable;
        }


        /// <summary>
        ///     Calculage update value from TaxType, TaxAmt, TaxRate(%)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="strChk"></param>
        //private void CalculateTaxType(int index, string strChk)
        private void CalculateCost(int index)
        {
            CostContent_ValueChanged(grd_RecEdit.EditIndex);
            CalculateBaseQty();
        }



        #region When value change
        // Added on: 22/08/2017, By: Fon
        protected void CostContent_ValueChanged(int editIndex)
        {
            #region variable

            decimal recQty = 0, price = 0, price_Qty = 0;
            decimal netAmt = 0, discAmt = 0, taxAmt = 0, totalAmt = 0;
            decimal currNetAmt = 0, currDiscAmt = 0, currTaxAmt = 0, currTotalAmt = 0;
            decimal discPercent = 0, taxRate = 0;
            decimal currRate = decimal.Parse(txt_ExRateAu.Text);
            string taxType = string.Empty;
            #endregion

            #region Find variable value
            if (grd_RecEdit.Rows[editIndex].FindControl("se_RecQtyEdit") != null)
            {
                ASPxSpinEdit se_RecQtyEdit = (ASPxSpinEdit)grd_RecEdit.Rows[editIndex].FindControl("se_RecQtyEdit");
                decimal.TryParse(se_RecQtyEdit.Text, out recQty);
            }

            if (grd_RecEdit.Rows[editIndex].FindControl("se_PriceEdit") != null)
            {
                ASPxSpinEdit se_PriceEdit = (ASPxSpinEdit)grd_RecEdit.Rows[editIndex].FindControl("se_PriceEdit");
                decimal.TryParse(se_PriceEdit.Text, out price);
            }

            if (grd_RecEdit.Rows[editIndex].FindControl("chk_DiscAdj") != null)
            {
                CheckBox chk_DiscAdj = (CheckBox)grd_RecEdit.Rows[editIndex].FindControl("chk_DiscAdj");
                if (grd_RecEdit.Rows[editIndex].FindControl("txt_Disc") != null)
                {
                    TextBox txt_Disc = (TextBox)grd_RecEdit.Rows[editIndex].FindControl("txt_Disc");
                    decimal.TryParse(txt_Disc.Text, out discPercent);
                }
            }

            if (grd_RecEdit.Rows[editIndex].FindControl("chk_TaxAdj") != null)
            {
                CheckBox chk_TaxAdj = (CheckBox)grd_RecEdit.Rows[editIndex].FindControl("chk_TaxAdj");
                if (grd_RecEdit.Rows[editIndex].FindControl("ddl_TaxType") != null)
                {
                    DropDownList ddl_TaxType = (DropDownList)grd_RecEdit.Rows[editIndex].FindControl("ddl_TaxType");
                    TextBox txt_TaxRate = (TextBox)grd_RecEdit.Rows[editIndex].FindControl("txt_TaxRate");

                    decimal.TryParse(txt_TaxRate.Text, out taxRate);
                    taxType = ddl_TaxType.SelectedItem.Value.ToUpper();
                }
            }

            if (grd_RecEdit.Rows[editIndex].FindControl("txt_CurrDiscAmt") != null)
            {
                TextBox txt_CurrDiscAmt = (TextBox)grd_RecEdit.Rows[editIndex].FindControl("txt_CurrDiscAmt");
                decimal.TryParse(txt_CurrDiscAmt.Text, out currDiscAmt);
            }


            #endregion

            price_Qty = RoundAmt(price * recQty);

            currNetAmt = NetAmt(taxType, taxRate, price * recQty, currDiscAmt, 1);
            currTaxAmt = TaxAmt(taxType, taxRate, price * recQty, currDiscAmt, 1);
            currTotalAmt = Amount(taxType, taxRate, price * recQty, currDiscAmt, 1);

            netAmt = RoundAmt(currNetAmt * currRate);
            taxAmt = RoundAmt(currTaxAmt * currRate);
            totalAmt = netAmt + taxAmt;

            discAmt = RoundAmt(currDiscAmt * currRate);

            #region Detail Highlight
            if (grd_RecEdit.Rows[editIndex].FindControl("se_TotalAmt") != null)
            {
                ASPxSpinEdit se_TotalAmt = (ASPxSpinEdit)grd_RecEdit.Rows[editIndex].FindControl("se_TotalAmt");

                se_TotalAmt.Text = string.Format(DefaultAmtFmt, currTotalAmt);
            }

            if (grd_RecEdit.Rows[editIndex].FindControl("lblNm_DiscAmt") != null)
            {
                Label lblNm_DiscAmt = (Label)grd_RecEdit.Rows[editIndex].FindControl("lblNm_DiscAmt");
                lblNm_DiscAmt.Text = string.Format(DefaultAmtFmt, currDiscAmt);
            }

            if (grd_RecEdit.Rows[editIndex].FindControl("lblNm_NetAmt") != null)
            {
                Label lblNm_NetAmt = (Label)grd_RecEdit.Rows[editIndex].FindControl("lblNm_NetAmt");
                lblNm_NetAmt.Text = string.Format(DefaultAmtFmt, currNetAmt);
            }
            #endregion

            #region Set content value.
            if (grd_RecEdit.Rows[editIndex].FindControl("txt_CurrNetAmt") != null)
            {
                TextBox txt_CurrNetAmt = (TextBox)grd_RecEdit.Rows[editIndex].FindControl("txt_CurrNetAmt");
                txt_CurrNetAmt.Text = string.Format(DefaultAmtFmt, currNetAmt);
            }

            if (grd_RecEdit.Rows[editIndex].FindControl("txt_CurrDiscAmt") != null)
            {
                TextBox txt_CurrDiscAmt = (TextBox)grd_RecEdit.Rows[editIndex].FindControl("txt_CurrDiscAmt");
                txt_CurrDiscAmt.Text = string.Format(DefaultAmtFmt, currDiscAmt);
            }

            if (grd_RecEdit.Rows[editIndex].FindControl("txt_CurrTaxAmt") != null)
            {
                TextBox txt_CurrTaxAmt = (TextBox)grd_RecEdit.Rows[editIndex].FindControl("txt_CurrTaxAmt");
                txt_CurrTaxAmt.Text = string.Format(DefaultAmtFmt, currTaxAmt, 2);
            }

            if (grd_RecEdit.Rows[editIndex].FindControl("lbl_CurrTotalAmtDt") != null)
            {
                Label lbl_CurrTotalAmtDt = (Label)grd_RecEdit.Rows[editIndex].FindControl("lbl_CurrTotalAmtDt");
                lbl_CurrTotalAmtDt.Text = string.Format(DefaultAmtFmt, currTotalAmt);
            }

            // About Base Currency
            if (grd_RecEdit.Rows[editIndex].FindControl("txt_NetAmt") != null)
            {
                TextBox txt_NetAmt = (TextBox)grd_RecEdit.Rows[editIndex].FindControl("txt_NetAmt");
                txt_NetAmt.Text = string.Format(DefaultAmtFmt, netAmt);
            }

            if (grd_RecEdit.Rows[editIndex].FindControl("txt_DiscAmt") != null)
            {
                TextBox txt_DiscAmt = (TextBox)grd_RecEdit.Rows[editIndex].FindControl("txt_DiscAmt");
                txt_DiscAmt.Text = string.Format(DefaultAmtFmt, discAmt);
            }

            if (grd_RecEdit.Rows[editIndex].FindControl("txt_TaxAmt") != null)
            {
                TextBox txt_TaxAmt = (TextBox)grd_RecEdit.Rows[editIndex].FindControl("txt_TaxAmt");
                txt_TaxAmt.Text = string.Format(DefaultAmtFmt, taxAmt);
            }

            if (grd_RecEdit.Rows[editIndex].FindControl("lbl_TotalAmtDt") != null)
            {
                Label lbl_TotalAmtDt = (Label)grd_RecEdit.Rows[editIndex].FindControl("lbl_TotalAmtDt");
                lbl_TotalAmtDt.Text = string.Format(DefaultAmtFmt, totalAmt);
            }
            #endregion

        }
        #endregion

        private void SaveAndCommit(string strAction)
        {

            Page.Validate();

            if (Page.IsValid)
            {
                var _mode = Request.Params["MODE"];
                var _action = string.Empty;

                //var OpenPeriod = period.GetLatestOpenEndDate(LoginInfo.ConnStr);
                //var InvCommittedDate = de_RecDate.Date.Date <= OpenPeriod.Date ? OpenPeriod : DateTime.Now;

                var deliPoint = cmb_DeliPoint.Value.ToString().Split(':')[0];
                var currencyRate = Convert.ToDecimal(txt_ExRateAu.Text);
                bool isAVCO = config.GetValue("IN", "SYS", "COST", hf_ConnStr.Value).ToUpper() == "AVCO";

                // Re-Calculation Extra Cost
                AllocateExtraCost();

                //For Edit 
                #region EDIT

                if (_mode.ToUpper() == "EDIT")
                {
                    _action = "MODIFY";

                    dsSave = DsRecEdit;
                    var drSave = dsSave.Tables[Rec.TableName].Rows[0];

                    drSave["Description"] = txt_Desc.Text.Trim();
                    drSave["DeliPoint"] = deliPoint;
                    drSave["InvoiceNo"] = txt_InvNo.Text.Trim();
                    drSave["VendorCode"] = (ddl_Vendor.SelectedItem != null)
                        ? ddl_Vendor.SelectedItem.Value.ToString()
                        : ddl_Vendor.Value.ToString();
                    drSave["CurrencyCode"] = ddl_Currency.Value.ToString();
                    drSave["ExRateAudit"] = currencyRate;
                    drSave["CurrencyRate"] = currencyRate;
                    drSave["IsCashConsign"] = Convert.ToBoolean(chk_CashConsign.Checked);
                    drSave["UpdatedDate"] = ServerDateTime;
                    drSave["UpdatedBy"] = LoginInfo.LoginName;
                    drSave["ExportStatus"] = false;

                    if (strAction == "Committed")
                    {
                        drSave["DocStatus"] = "Committed";
                        drSave["CommitDate"] = ServerDateTime;
                    }
                    else
                    {
                        drSave["DocStatus"] = "Received";
                    }

                    if (de_InvDate.Value != null)
                    {
                        drSave["InvoiceDate"] = de_InvDate.Date.Date;
                    }
                    else
                    {
                        drSave["InvoiceDate"] = DBNull.Value;
                    }

                    if (drSave.Table.Columns.Contains("TotalExtraCost"))
                    {
                        drSave["TotalExtraCost"] = se_TotalExtraCost.Text == string.Empty ? 0m : Convert.ToDecimal(se_TotalExtraCost.Value.ToString());
                        if (rdb_ExtraCostByAmt.Checked)
                            drSave["ExtraCostBy"] = "A";
                        else
                            drSave["ExtraCostBy"] = "Q";
                    }
                }
                #endregion

                #region Create
                else //For Create
                {
                    _action = "CREATE";

                    Rec.GetStructure(dsSave, hf_ConnStr.Value);

                    // Added on: 01/02/2018, By: Fon
                    if (dsSave.Tables[Rec.TableName].Rows.Count > 0)
                    {
                        dsSave.Tables[Rec.TableName].Rows.Clear();
                        dsSave.Tables[Rec.TableName].AcceptChanges();
                    }
                    // End Added.

                    var drSaveNew = dsSave.Tables[Rec.TableName].NewRow();

                    // For new
                    string newRecNo = Rec.GetNewID(DateTime.Parse(de_RecDate.Text), hf_ConnStr.Value);
                    drSaveNew["RecNo"] = newRecNo;
                    drSaveNew["RecDate"] = de_RecDate.Date.Date; //DateTime.Parse(de_RecDate.Date.ToShortDateString() + " " + ServerDateTime.TimeOfDay);
                    drSaveNew["Description"] = txt_Desc.Text.Trim();
                    drSaveNew["DeliPoint"] = deliPoint;
                    drSaveNew["InvoiceNo"] = txt_InvNo.Text.Trim();
                    drSaveNew["VendorCode"] = ddl_Vendor.Value.ToString().Split(new[] { ":" }, StringSplitOptions.None)[0].Trim(); //ddl_Vendor.Value;
                    drSaveNew["CurrencyCode"] = ddl_Currency.Value.ToString();
                    drSaveNew["ExRateAudit"] = currencyRate;
                    drSaveNew["CurrencyRate"] = currencyRate;
                    drSaveNew["IsCashConsign"] = Convert.ToBoolean(chk_CashConsign.Checked);
                    drSaveNew["CreatedDate"] = ServerDateTime;
                    drSaveNew["CreatedBy"] = LoginInfo.LoginName;
                    drSaveNew["UpdatedDate"] = ServerDateTime;
                    drSaveNew["UpdatedBy"] = LoginInfo.LoginName;
                    drSaveNew["ExportStatus"] = false;
                    drSaveNew["PoSource"] = ""; // DsRecEdit.Tables[Rec.TableName].Rows[0]["PoSource"];

                    if (de_InvDate.Value != null)
                    {
                        drSaveNew["InvoiceDate"] = de_InvDate.Date;
                    }
                    else
                    {
                        drSaveNew["InvoiceDate"] = DBNull.Value;
                    }

                    if (strAction == "Committed")
                    {
                        drSaveNew["DocStatus"] = "Committed";
                        drSaveNew["CommitDate"] = ServerDateTime;
                    }
                    else
                    {
                        drSaveNew["DocStatus"] = "Received";
                    }

                    if (drSaveNew.Table.Columns.Contains("TotalExtraCost"))
                        drSaveNew["TotalExtraCost"] = se_TotalExtraCost.Text == string.Empty ? 0m : Convert.ToDecimal(se_TotalExtraCost.Value.ToString());


                    dsSave.Tables[Rec.TableName].Rows.Add(drSaveNew);

                    // Added on: 30/01/2018, By: Fon
                    RecDt.GetStructure(dsSave, hf_ConnStr.Value);

                    if (dsSave.Tables[RecDt.TableName].Rows.Count > 0)
                    {
                        dsSave.Tables[RecDt.TableName].Rows.Clear();
                        dsSave.Tables[RecDt.TableName].AcceptChanges();
                    }
                    // End Added.

                    if (grd_RecEdit.Rows.Count > 0)
                    {
                        #region Loop grd_RecEdit
                        foreach (DataRow drSelectedNew in DsRecEdit.Tables[RecDt.TableName].Rows)
                        {
                            if (drSelectedNew.RowState != DataRowState.Deleted)
                            {
                                if ((Convert.ToDecimal(drSelectedNew["RecQty"]) > 0))
                                {
                                    // For Detail
                                    var drRecdtNew = dsSave.Tables[RecDt.TableName].NewRow();

                                    drRecdtNew["RecNo"] = newRecNo; // Rec.GetNewID(DateTime.Parse(de_RecDate.Text), hf_ConnStr.Value);
                                    drRecdtNew["RecDtNo"] = RecDtNo + 1;
                                    drRecdtNew["LocationCode"] = drSelectedNew["LocationCode"];
                                    drRecdtNew["ProductCode"] = (drSelectedNew["ProductCode"].ToString().Contains(":"))
                                        ? drSelectedNew["ProductCode"].ToString()
                                            .Split(new[] { ":" }, StringSplitOptions.None)[0].Trim()
                                        : drSelectedNew["ProductCode"];
                                    drRecdtNew["UnitCode"] = drSelectedNew["UnitCode"];
                                    drRecdtNew["OrderQty"] = Convert.ToDecimal(drSelectedNew["OrderQty"]);
                                    drRecdtNew["FOCQty"] =
                                        (Convert.ToDecimal(drSelectedNew["FOCQty"] == DBNull.Value
                                            ? 0
                                            : (decimal)drSelectedNew["FOCQty"]));
                                    drRecdtNew["RecQty"] =
                                        (Convert.ToDecimal(drSelectedNew["RecQty"] == DBNull.Value
                                            ? 0
                                            : (decimal)drSelectedNew["RecQty"]));
                                    drRecdtNew["Price"] =
                                        (Convert.ToDecimal(drSelectedNew["Price"] == DBNull.Value
                                            ? 0
                                            : (decimal)drSelectedNew["Price"]));
                                    drRecdtNew["Discount"] =
                                        (Convert.ToDecimal(drSelectedNew["Discount"] == DBNull.Value
                                            ? 0
                                            : (decimal)drSelectedNew["Discount"]));
                                    drRecdtNew["TaxType"] = drSelectedNew["TaxType"];
                                    drRecdtNew["TaxRate"] = drSelectedNew["TaxRate"];
                                    drRecdtNew["TaxAdj"] = (bool)drSelectedNew["TaxAdj"];

                                    drRecdtNew["NetAmt"] =
                                        (Convert.ToDecimal(drSelectedNew["NetAmt"] == DBNull.Value
                                            ? 0
                                            : (decimal)drSelectedNew["NetAmt"]));
                                    drRecdtNew["DiccountAmt"] =
                                        (Convert.ToDecimal(drSelectedNew["DiccountAmt"] == DBNull.Value
                                            ? 0
                                            : (decimal)drSelectedNew["DiccountAmt"]));
                                    drRecdtNew["TaxAmt"] =
                                        (Convert.ToDecimal(drSelectedNew["TaxAmt"] == DBNull.Value
                                            ? 0
                                            : (decimal)drSelectedNew["TaxAmt"]));
                                    drRecdtNew["TotalAmt"] =
                                        (Convert.ToDecimal(drSelectedNew["TotalAmt"] == DBNull.Value
                                            ? 0
                                            : (decimal)drSelectedNew["TotalAmt"]));

                                    // Added on: 05/09/2017, By: Fon
                                    drRecdtNew["CurrNetAmt"] = Convert.ToDecimal(drSelectedNew["CurrNetAmt"] == DBNull.Value
                                        ? 0 : decimal.Parse(drSelectedNew["CurrNetAmt"].ToString()));
                                    drRecdtNew["CurrDiscAmt"] = Convert.ToDecimal(drSelectedNew["CurrDiscAmt"] == DBNull.Value
                                        ? 0 : decimal.Parse(drSelectedNew["CurrDiscAmt"].ToString()));
                                    drRecdtNew["CurrTaxAmt"] = Convert.ToDecimal(drSelectedNew["CurrTaxAmt"] == DBNull.Value
                                        ? 0 : decimal.Parse(drSelectedNew["CurrTaxAmt"].ToString()));
                                    drRecdtNew["CurrTotalAmt"] = Convert.ToDecimal(drSelectedNew["CurrTotalAmt"] == DBNull.Value
                                        ? 0 : decimal.Parse(drSelectedNew["CurrTotalAmt"].ToString()));
                                    // End Added.

                                    drRecdtNew["PoNo"] = drSelectedNew["PoNo"];
                                    drRecdtNew["PoDtNo"] = drSelectedNew["PoDtNo"];
                                    drRecdtNew["PrNo"] = drSelectedNew["PrNo"];
                                    drRecdtNew["NetDrAcc"] = drSelectedNew["NetDrAcc"];
                                    drRecdtNew["TaxDrAcc"] = drSelectedNew["TaxDrAcc"];
                                    drRecdtNew["ExportStatus"] = false;
                                    drRecdtNew["Descen"] = drSelectedNew["Descen"];
                                    drRecdtNew["Descll"] = drSelectedNew["Descll"];
                                    drRecdtNew["Comment"] = drSelectedNew["Comment"];
                                    drRecdtNew["Rate"] = drSelectedNew["Rate"];
                                    drRecdtNew["RcvUnit"] = drSelectedNew["RcvUnit"];
                                    drRecdtNew["DiscAdj"] = drSelectedNew["DiscAdj"];
                                    drRecdtNew["Status"] = (strAction == "Committed") ? "Committed" : "Received";
                                    if (drRecdtNew.Table.Columns.Contains("ExtraCost"))
                                    {
                                        drRecdtNew["ExpiryDate"] = drSelectedNew["ExpiryDate"];
                                        drRecdtNew["ExtraCost"] = drSelectedNew["ExtraCost"];
                                    }

                                    // Add new record
                                    dsSave.Tables[RecDt.TableName].Rows.Add(drRecdtNew);

                                    RecDtNo = Convert.ToInt32(drRecdtNew["RecDtNo"]);
                                    //this.UpdateInventoryForCommit(drRecdtNew);
                                }
                                else
                                {
                                    lbl_WarningDelete.Text = "Receiving is not complete";
                                    pop_WarningDelete.ShowOnPageLoad = true;
                                    return;
                                }
                            }
                        }
                        #endregion // Loop in grd_RecEdit
                    }
                }
                #endregion


                var recNo = _mode == "EDIT" ? txt_RecNo.Text : dsSave.Tables[Rec.TableName].Rows[0]["RecNo"].ToString();

                if (!string.IsNullOrEmpty(txt_RecNo.Text))
                {
                    var sql = string.Format("DELETE FROM [IN].Inventory WHERE HdrNo='{0}' AND [Type]='RC'", txt_RecNo.Text);
                    Rec.DbExecuteQuery(sql, null, hf_ConnStr.Value);

                    inv.GetListByHdrNo(dsSave, recNo, hf_ConnStr.Value);
                }



                // ********************************************************************************

                //Save Data to 3 tables Rec, RecDt and Inventory.

                foreach (DataTable dt in dsSave.Tables)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr.RowError))
                        {
                            pop_ConfirmSave.ShowOnPageLoad = false;
                            AlertMessageBox(dr.RowError);
                            return;
                        }
                    }
                }


                var result = Rec.Save(dsSave, hf_ConnStr.Value);

                if (result)
                {
                    _transLog.Save("PC", "REC", recNo, _action, string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);


                    // Update PC.RecExtCost
                    string sqlDel = string.Format("DELETE FROM PC.RecExtCost WHERE RecNo = '{0}';", recNo);
                    string sqlIns = string.Empty;

                    DataTable dt = DsRecEdit.Tables[recExtCost];
                    if (dt.Rows.Count > 0)
                    {
                        int i = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr.RowState != DataRowState.Deleted)
                                sqlIns += string.Format("('{0}', {1}, {2}, {3}),", recNo, ++i, dr["TypeId"].ToString(), dr["Amount"].ToString());
                        }

                        if (sqlIns != string.Empty)
                        {
                            sqlIns = "INSERT INTO PC.RecExtCost (RecNo, DtNo, TypeId, Amount) VALUES " + sqlIns;
                            sqlIns = sqlIns.Remove(sqlIns.Length - 1);  // remove last comma (,)
                        }

                    }
                    Rec.DbExecuteQuery(sqlDel + sqlIns, null, hf_ConnStr.Value);

                    // ------------------------------

                    Rec.DbExecuteQuery("UPDATE PC.RecDt SET [Status]='Received' WHERE RecNo=@DocNo", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@DocNo", recNo) }, LoginInfo.ConnStr);


                    if (strAction == "Committed")
                    {
                        Rec.DbExecuteQuery("EXEC PC.RecCommit @DocNo", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@DocNo", recNo) }, LoginInfo.ConnStr);

                        _transLog.Save("PC", "REC", recNo, "COMMIT", string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);
                    }


                    // ****************************************************************************

                    pop_ConfirmSave.ShowOnPageLoad = false;

                    Response.Redirect("Rec.aspx?ID=" + recNo + "&BuCode=" + Request.Params["BuCode"] + "&Vid=" + Request.Params["Vid"]);
                }
            }
        }

        private void CreateAccountMap(DataSet dsSave, string connStr)
        {
            return;

        }


        #endregion

        private void GetImportAccCode()
        {
            DataTable dt = new DataTable("AccoundCode");
            string conStr = LoginInfo.ConnStr;
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [IMPORT].AccountCode", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            catch
            {
                con.Close();
            }

            dsImport.Tables.Add(dt);
        }

        #region Ori. Currency
        // Added on: 08/2017, By: Fon
        // For: about multi-currency content and calculate process.
        protected void ddl_Currency_Init(object sender, EventArgs e)
        {
            ddl_Currency.DataSource = currency.GetLastCurrencyRate(LoginInfo.ConnStr);
            ddl_Currency.DataBind();
            ddl_Currency.Value = Config.GetConfigValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value);
        }
        protected void ddl_Currency_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime invoiceDate = DateTime.Now;
            if (de_InvDate.Text != string.Empty)
                invoiceDate = de_InvDate.Date;
            txt_ExRateAu.Text = currency.GetLastCurrencyRate(ddl_Currency.Value.ToString(), invoiceDate, LoginInfo.ConnStr).ToString();
            txt_ExRateAu_TextChanged(txt_ExRateAu, e);

            //ASPxComboBox ddl_Currency = (ASPxComboBox)sender;
            //txt_ExRateAu.Text = Convert.ToString(currency.GetLastCurrencyRate(ddl_Currency.Value.ToString(), DateTime.Now, LoginInfo.ConnStr));
            //txt_ExRateAu_TextChanged(txt_ExRateAu, e);
        }


        private void CalculationForValueChanged(bool discAmtChanged = false, bool taxAmtChanged = false)
        {
            //var gvRow = grd_RecEdit.Rows[editIndex];
            var gvRow = grd_RecEdit.Rows[grd_RecEdit.EditIndex];

            // Controls on screen
            var se_RecQtyEdit = gvRow.FindControl("se_RecQtyEdit") as ASPxSpinEdit;
            var se_FocEdit = gvRow.FindControl("se_FocEdit") as ASPxSpinEdit;
            var se_PriceEdit = gvRow.FindControl("se_PriceEdit") as ASPxSpinEdit;

            // Discount Rate
            var chk_DiscAdj = gvRow.FindControl("chk_DiscAdj") as CheckBox;
            var txt_Disc = gvRow.FindControl("txt_Disc") as TextBox;

            // Tax Rate
            var chk_TaxAdj = gvRow.FindControl("chk_TaxAdj") as CheckBox;
            var ddl_TaxType = gvRow.FindControl("ddl_TaxType") as DropDownList;
            var txt_TaxRate = gvRow.FindControl("txt_TaxRate") as TextBox;

            // Currency
            var txt_CurrNetAmt = gvRow.FindControl("txt_CurrNetAmt") as TextBox;
            var txt_CurrDiscAmt = gvRow.FindControl("txt_CurrDiscAmt") as TextBox;
            var txt_CurrTaxAmt = gvRow.FindControl("txt_CurrTaxAmt") as TextBox;
            var lbl_CurrTotalAmtDt = gvRow.FindControl("lbl_CurrTotalAmtDt") as Label;


            // Calculate totalPrice
            decimal qty = Convert.ToDecimal(se_RecQtyEdit.Value);
            decimal price = Convert.ToDecimal(se_PriceEdit.Value);
            decimal totalPrice = RoundAmt(qty * price);

            // Calculate Discount
            decimal discRate = Convert.ToDecimal(txt_Disc.Text);
            decimal discAmt = 0;

            // Calculate Tax
            string taxType = ddl_TaxType.SelectedItem.Value.ToUpper();
            decimal taxRate = taxType == "N" ? 0 : Convert.ToDecimal(txt_TaxRate.Text);  // if taxType is None, reset taxRate=0
            decimal taxAmt = taxType == "N" ? 0 : Convert.ToDecimal(txt_CurrTaxAmt.Text);  // if taxType is None, reset taxRate=0;

            decimal netAmt = 0;
            decimal totalAmt = 0;

            if (taxType == "I")
            {
                if (discAmtChanged)
                {
                    discAmt = Convert.ToDecimal(txt_CurrDiscAmt.Text);

                    discRate = RoundNumber(discAmt * 100 / totalPrice, 2);
                    txt_Disc.Text = string.Format("{0:N}", discRate);
                }
                else
                    discAmt = RoundAmt(totalPrice * discRate / 100);

                totalAmt = totalPrice - discAmt;

                if (taxAmtChanged)
                {
                    taxAmt = Convert.ToDecimal(txt_CurrTaxAmt.Text);

                }
                else
                    taxAmt = RoundAmt((totalAmt * taxRate) / (100 + taxRate));

                netAmt = totalAmt - taxAmt;
            }
            else
            {
                //                discAmt = Math.Round(totalPrice * discRate / 100, 2);
                if (discAmtChanged)
                {
                    discAmt = Convert.ToDecimal(txt_CurrDiscAmt.Text);

                    discRate = RoundNumber(discAmt * 100 / totalPrice, 2);
                    txt_Disc.Text = string.Format("{0:N}", discRate);
                }
                else
                    discAmt = RoundAmt(totalPrice * discRate / 100);


                netAmt = totalPrice - discAmt;
                if (taxAmtChanged)
                {
                    taxAmt = Convert.ToDecimal(txt_CurrTaxAmt.Text);
                }
                else
                    taxAmt = RoundAmt(netAmt * taxRate / 100);

                totalAmt = netAmt + taxAmt;
            }

            // Assign value to control(on screen)


            txt_CurrNetAmt.Text = String.Format(DefaultAmtFmt, netAmt);
            txt_CurrDiscAmt.Text = String.Format(DefaultAmtFmt, discAmt);
            txt_CurrTaxAmt.Text = String.Format(DefaultAmtFmt, taxAmt);
            lbl_CurrTotalAmtDt.Text = String.Format(DefaultAmtFmt, totalAmt);

            // local (Base)
            var txt_NetAmt = gvRow.FindControl("txt_NetAmt") as TextBox;
            var txt_DiscAmt = gvRow.FindControl("txt_DiscAmt") as TextBox;
            var txt_TaxAmt = gvRow.FindControl("txt_TaxAmt") as TextBox;
            var lbl_TotalAmtDt = gvRow.FindControl("lbl_TotalAmtDt") as Label;

            decimal currRate = decimal.Parse(txt_ExRateAu.Text);
            netAmt = RoundAmt(netAmt * currRate);
            taxAmt = RoundAmt(taxAmt * currRate);

            txt_NetAmt.Text = String.Format(DefaultAmtFmt, netAmt);
            txt_DiscAmt.Text = String.Format(DefaultAmtFmt, RoundAmt(discAmt * currRate));
            txt_TaxAmt.Text = String.Format(DefaultAmtFmt, taxAmt);
            //lbl_TotalAmtDt.Text = String.Format("{0:N}", Math.Round(totalAmt * currRate, 2));
            lbl_TotalAmtDt.Text = String.Format(DefaultAmtFmt, netAmt + taxAmt);

        }

        protected void txt_CurrDiscAmt_TextChanged(object sender, EventArgs e)
        {
            CalculationForValueChanged(true, false);
            //TextBox txt_CurrDiscAmt = (TextBox)sender;
            //TextBox txt_Disc = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_Disc");
            //ASPxSpinEdit se_PriceEdit = (ASPxSpinEdit)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_PriceEdit");
            //ASPxSpinEdit se_RecQtyEdit = (ASPxSpinEdit)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_RecQtyEdit");

            //if (txt_CurrDiscAmt.Text == string.Empty)
            //    txt_CurrDiscAmt.Text = string.Format(DefaultAmtFmt, 0);
            //decimal price_Qty = RoundAmt(decimal.Parse(se_PriceEdit.Text) * decimal.Parse(se_RecQtyEdit.Text));
            //decimal currDiscAmt = decimal.Parse(txt_CurrDiscAmt.Text);
            //decimal discAmt = RoundAmt(currDiscAmt * decimal.Parse(txt_ExRateAu.Text));
            //decimal discPercent = Math.Round((currDiscAmt / price_Qty) * 100, 2, MidpointRounding.AwayFromZero);
            //txt_Disc.Text = string.Format(DefaultAmtFmt, discPercent);

            //CalculateCost(grd_RecEdit.EditIndex);
        }

        protected void txt_CurrTaxAmt_TextChanged(object sender, EventArgs e)
        {
            CalculationForValueChanged(false, true);

            //    TextBox txt_CurrTaxAmt = (TextBox)sender;
            //    TextBox txt_CurrNetAmt = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_CurrNetAmt");
            //    TextBox txt_CurrDiscAmt = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_CurrDiscAmt");
            //    TextBox txt_TaxRate = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_TaxRate");
            //    ASPxSpinEdit se_PriceEdit = (ASPxSpinEdit)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_PriceEdit");
            //    ASPxSpinEdit se_RecQtyEdit = (ASPxSpinEdit)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_RecQtyEdit");

            //    if (txt_CurrTaxAmt.Text == string.Empty)
            //        txt_CurrTaxAmt.Text = string.Format(DefaultAmtFmt, 0);
            //    decimal price = decimal.Parse(se_PriceEdit.Text);
            //    decimal recQty = decimal.Parse(se_RecQtyEdit.Text);
            //    decimal discAmt = decimal.Parse(txt_CurrDiscAmt.Text);
            //    decimal currTaxAmt = decimal.Parse(txt_CurrTaxAmt.Text);
            //    decimal taxRate = Get_TaxRate(currTaxAmt, price * recQty, discAmt, 1);

            //    txt_TaxRate.Text = string.Format("{0:N}", taxRate);
            //    CalculateCost(grd_RecEdit.EditIndex);
        }

        protected decimal Get_TaxRate(decimal taxAmt, decimal price, decimal discPerUnit, decimal qty)
        {
            // Use before Calculate_Cost()
            DropDownList ddl_TaxType = (DropDownList)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("ddl_TaxType");
            decimal calAmt = CalAmt(price, discPerUnit, qty);
            decimal taxRate = 0;

            if (ddl_TaxType.SelectedItem.Value.ToUpper() == "I")
                taxRate = Math.Round(taxAmt / (calAmt - taxAmt), 2, MidpointRounding.AwayFromZero);

            else
                taxRate = Math.Round(taxAmt / calAmt, 2, MidpointRounding.AwayFromZero);

            taxRate = Math.Round(taxRate * 100, 2, MidpointRounding.AwayFromZero);
            return taxRate;
        }

        protected void txt_ExRateAu_TextChanged(object sender, EventArgs e)
        {
            RateChanged(DsRecEdit.Tables[RecDt.TableName]);
            grd_RecEdit.DataSource = DsRecEdit.Tables[RecDt.TableName];
            grd_RecEdit.DataBind();
        }

        protected DataTable RateChanged(DataTable dtRecEdit)
        {
            string taxType = string.Empty;
            decimal price = 0, recQty = 0, taxRate = 0, currDiscAmt = 0, currRate = 0;
            decimal netAmt = 0, discAmt = 0, taxAmt = 0, totalAmt = 0;

            foreach (DataRow dr in dtRecEdit.Rows)
            {
                // Get value
                decimal.TryParse(dr["Price"].ToString(), out price);
                decimal.TryParse(dr["RecQty"].ToString(), out recQty);
                decimal.TryParse(dr["CurrDiscAmt"].ToString(), out currDiscAmt);
                decimal.TryParse(dr["TaxRate"].ToString(), out taxRate);
                decimal.TryParse(txt_ExRateAu.Text, out currRate);
                taxType = Convert.ToString(dr["TaxType"]);

                // Set value
                discAmt = RoundAmt(currDiscAmt * currRate);
                netAmt = NetAmt(taxType, taxRate, (price * currRate) * recQty, discAmt, 1);
                taxAmt = TaxAmt(taxType, taxRate, (price * currRate) * recQty, discAmt, 1);
                totalAmt = Amount(taxType, taxRate, (price * currRate) * recQty, discAmt, 1);

                dr["NetAmt"] = netAmt;
                dr["DiccountAmt"] = discAmt;
                dr["TaxAmt"] = taxAmt;
                dr["TotalAmt"] = totalAmt;
            }

            return dtRecEdit;
        }
        #endregion

        // Added on: 01/02/2018, By: Fon
        public bool Check_DuplicateInvoiceNoByVendor(string currentRecNo, string invoiceNo, string vendorCode)
        {
            DataTable dtRecByVendor = Rec.GetListByVendor(vendorCode, LoginInfo.ConnStr);
            foreach (DataRow dr in dtRecByVendor.Rows)
            {
                if (string.Format("{0}", dr["InvoiceNo"]) == invoiceNo)
                {
                    return (string.Format("{0}", dr["RecNo"]) == currentRecNo)
                        ? true : false;
                }
            }

            return true;
        }

        protected void btn_acceptWarn_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
        }
        // End Added.

        private void DeleteInventory(string recNo)
        {
            var sql = string.Format("DELETE FROM [IN].Inventory WHERE [Type]='RC' AND HdrNo='{0}'", recNo);

            inv.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

        }

        public string DefaultPriceFmt
        {
            get { return "{0:N" + _default.DigitPrice.ToString() + "}"; }
        }

        public class DefaultValues
        {
            public string Currency { get; set; }
            public int DigitPrice { get; set; }
            public int DigitAmt { get; set; }
            public int DigitQty { get; set; }
            public decimal TaxRate { get; set; }
            public string CostMethod { get; set; }
        }
    }
}