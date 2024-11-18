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


namespace BlueLedger.PL.PC.CN
{
    public partial class CnEdit : BasePage
    {
        #region "Attribute"

        private readonly Blue.BL.Option.Admin.Interface.AccountMapp accMapp = new Blue.BL.Option.Admin.Interface.AccountMapp();
        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();

        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();

        private readonly Blue.BL.PC.CN.Cn cn = new Blue.BL.PC.CN.Cn();
        private readonly Blue.BL.PC.CN.CnDt cnDt = new Blue.BL.PC.CN.CnDt();
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private readonly DataSet dsCnEditCount = new DataSet();
        private readonly DataSet dsInventory = new DataSet();
        private readonly Blue.BL.IN.Inventory inv = new Blue.BL.IN.Inventory();
        private readonly Blue.BL.Option.Inventory.StoreLct locat = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.PC.REC.REC rec = new Blue.BL.PC.REC.REC();
        private readonly Blue.BL.PC.REC.RECDt recDt = new Blue.BL.PC.REC.RECDt();
        private readonly Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();
        private readonly Blue.BL.Ref.Currency currency = new Blue.BL.Ref.Currency();

        private string MsgError = string.Empty;
        private int _CnDtNo;

        private int CN_QTY_INDEX = 0;
        private int CN_AMT_INDEX = 1;


        //BL.IN.Inventory.Fifo Fifo = new BL.IN.Inventory.Fifo();

        //private int _cnDtNo;
        private decimal _totalAmt;

        private Blue.BL.Option.Admin.Interface.AccountMapp accountMapp =
            new Blue.BL.Option.Admin.Interface.AccountMapp();

        //private decimal calculatedOrderqty = 0;
        private Blue.BL.Ref.Currency curr = new Blue.BL.Ref.Currency();
        private Blue.BL.Option.Inventory.DeliveryPoint deliPoint = new Blue.BL.Option.Inventory.DeliveryPoint();
        //private decimal discountUpdateAmt = 0;
        //private decimal discountUpdatePercentage = 0;
        private DataSet dsCnEdit = new DataSet();
        private DataSet dsPoUpdate = new DataSet();
        private Blue.BL.Ref.ExRate exRate = new Blue.BL.Ref.ExRate();
        private Blue.BL.GnxLib gnxLib = new Blue.BL.GnxLib();
        private decimal grandQtyAmt = 0;


        private decimal grandCurrNetAmt = 0;
        private decimal grandCurrTaxAmt = 0;
        private decimal grandCurrTotalAmt = 0;
        private decimal grandNetAmt = 0;
        private decimal grandTaxAmt = 0;
        private decimal grandTotalAmt = 0;

        //private int intCnDtNo = 0;
        private string netAcCode = string.Empty;
        //private decimal netAmtUpdate;
        private Blue.BL.PC.PO.PO po = new Blue.BL.PC.PO.PO();
        private Blue.BL.PC.PO.PoDt poDt = new Blue.BL.PC.PO.PoDt();
        //private decimal priceUpdate;
        private decimal qtyrecUpdate = 0;
        //private decimal recQty;
        private string status = string.Empty;
        //private string statusPartial = "Partial";
        //private string statusPrint = "Printed";
        private string taxAcCode = string.Empty;
        //private decimal taxAmtUpdate;
        //private decimal taxUpdate;
        //private decimal totalAmountUpdate;
        //private decimal totalCancelAmtUpdate = 0;
        private Blue.BL.Option.Inventory.Unit unit = new Blue.BL.Option.Inventory.Unit();

        /// <summary>
        ///     Max CnDtNo
        /// </summary>
        public decimal TotalAmt
        {
            get
            {
                _totalAmt = (ViewState["TotalAmt"] == null ? 0 : (decimal)ViewState["TotalAmt"]);
                return _totalAmt;
            }
            set
            {
                _totalAmt = value;
                ViewState.Add("TotalAmt", _totalAmt);
            }
        }

        public int CnDtNo
        {
            get
            {
                _CnDtNo = (ViewState["CnDtNo"] == null ? 0 : (int)ViewState["CnDtNo"]);
                return _CnDtNo;
            }
            set
            {
                _CnDtNo = value;
                ViewState.Add("CnDtNo", _CnDtNo);
            }
        }

        /// <summary>
        ///     Get audit currency code.
        /// </summary>
        private string AuditCurrencyCode
        {
            get { return config.GetConfigValue("APP", "BU", "AuditCurrencyCode", hf_ConnStr.Value); }
        }

        /// <summary>
        ///     Get management currency code.
        /// </summary>
        private string MngCurrencyCode
        {
            get { return config.GetConfigValue("APP", "BU", "MngCurrencyCode", hf_ConnStr.Value); }
        }

        /// <summary>
        ///     Get exchange rate type used for jv.
        ///     The exchange rate type can be buying, selling or average.
        /// </summary>
        private string ExRateType
        {
            get { return config.GetConfigValue("PC", "CN", "ExRateFMT", hf_ConnStr.Value); }
        }

        /// <summary>
        /// </summary>
        private string CnEditMode
        {
            get { return Session["CnEditMode"].ToString(); }
            set { Session["CnEditMode"] = value; }
        }

        #endregion

        #region "Operation"

        protected override void Page_Load(object sender, EventArgs e)
        {
            // Check login
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsCnEdit = (DataSet)Session["dsCnEdit"];
            }

        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            hf_ConnStr.Value = bu.GetConnectionString(Request.Params["BuCode"]);
            hf_LoginName.Value = LoginInfo.LoginName;
        }

        /// <summary>
        /// </summary>
        private void Page_Retrieve()
        {
            var MODE = Request.QueryString["MODE"];

            if (MODE.ToUpper() == "EDIT")
            {
                var MsgError = string.Empty;

                // Get invoice no from HTTP query string
                var CnNo = Request.QueryString["ID"];

                cn.GetListByCnNo(dsCnEdit, ref MsgError, CnNo, hf_ConnStr.Value);
                cnDt.GetListByCnNo(dsCnEdit, CnNo, hf_ConnStr.Value);
                inv.GetListByHdrNo(dsCnEdit, CnNo, hf_ConnStr.Value);
            }
            else
            {
                var result = cn.GetStructure(dsCnEdit, hf_ConnStr.Value);

                if (result)
                {
                    // Get Schema.
                    cnDt.GetStructure(dsCnEdit, hf_ConnStr.Value);
                    inv.GetStructure(dsCnEdit, hf_ConnStr.Value);
                }
            }

            // Condition for MaxCnDtNo.
            if (Request.QueryString["MODE"].ToUpper() == "NEW")
            {
                CnDtNo = 0;
            }
            else
            {
                if (Request.QueryString["ID"] != null)
                {
                    // Get recdt maxId and keep in the viewstate.
                    cnDt.GetCnDtMaxIDByCnNo(Request.QueryString["ID"], dsCnEditCount, hf_ConnStr.Value);

                    if (dsCnEditCount.Tables[cnDt.TableName].Rows.Count > 0)
                    {
                        CnDtNo = Convert.ToInt32(dsCnEditCount.Tables[cnDt.TableName].Rows[0]["CnDtNo"].ToString());
                    }
                }
            }

            Session["dsCnEdit"] = dsCnEdit;

            Page_Setting();
        }

        /// <summary>
        /// </summary>
        private void Page_Setting()
        {
            var MODE = Request.QueryString["MODE"];

            if (MODE.ToUpper() == "EDIT")
            {
                var drCnEdit = dsCnEdit.Tables[cn.TableName].Rows[0];

                lbl_CnNo.Enabled = false;
                lbl_CnNo.Text = drCnEdit["CnNo"].ToString();

                DateTime date;
                if (DateTime.TryParse(drCnEdit["CnDate"].ToString(), out date))
                    txt_CnDate.Text = DateTime.Parse(drCnEdit["CnDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
                else
                    txt_CnDate.Text = "";

                txt_CnDate.Enabled = false;
                txt_DocNo.Text = drCnEdit["DocNo"].ToString();

                txt_DocDate.Text = (drCnEdit["DocDate"].ToString() != string.Empty
                    ? DateTime.Parse(drCnEdit["DocDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate)
                    : DateTime.Now.ToString(LoginInfo.BuFmtInfo.FmtSDate));
                lbl_Status.Text = drCnEdit["DocStatus"].ToString();
                txt_Desc.Text = drCnEdit["Description"].ToString();
                //lbl_Currency.Text = drCnEdit["CurrencyCode"].ToString();
                //lbl_ExRateAu.Text = (drCnEdit["ExRateAudit"].ToString() == string.Empty
                //    ? "1.0000"
                //    : drCnEdit["ExRateAudit"].ToString());
                ddl_Currency.Value = drCnEdit["CurrencyCode"].ToString();
                txt_ExRateAu.Text = drCnEdit["ExRateAudit"].ToString() == string.Empty ? "1.0000" : drCnEdit["ExRateAudit"].ToString();
                ddl_Vendor.Visible = false;
                lbl_Vendor.Text = drCnEdit["VendorCode"] + " : " + vendor.GetName(drCnEdit["VendorCode"].ToString(), hf_ConnStr.Value);
                txt_DocDate.Enabled = false;
            }
            else if (MODE.ToUpper() == "NEW")  // New only
            {
                lbl_CnNo.Enabled = false;
                lbl_Vendor.Visible = false;

                txt_CnDate.Text = ServerDateTime.ToShortDateString();
                //lbl_Currency.Text = config.GetValue("APP", "BU", "DefaultCurrency", LoginInfo.ConnStr);
                //lbl_ExRateAu.Text = "1.00000";
                ddl_Currency.Value = config.GetValue("APP", "BU", "DefaultCurrency", LoginInfo.ConnStr);
                txt_ExRateAu.Text = "1.00000";
            }

            CnEditMode = string.Empty;
            grd_CnDt.DataSource = dsCnEdit.Tables[cnDt.TableName];
            grd_CnDt.DataBind();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #endregion

        #region "Calculation"

        /// <summary>
        ///     Get exchange rate
        /// </summary>
        /// <returns></returns>
        private decimal GetExchangeRate()
        {
            decimal exchangeRate = 0;

            return exchangeRate;
        }

        /// <summary>
        ///     Calculate quantitydeviation
        /// </summary>
        /// <returns></returns>
        private decimal GetQuantityDeviation(int rowindex)
        {
            decimal quantityDeviation = 0;

            if ((dsCnEdit.Tables[cnDt.TableName] != null) && (dsCnEdit.Tables[cnDt.TableName].Rows.Count > 0))
            {
                product.GetList(dsInventory, dsCnEdit.Tables[cnDt.TableName].Rows[0]["ProductCode"].ToString(),
                    hf_ConnStr.Value);

                if ((dsInventory.Tables[product.TableName] != null) &&
                    (dsInventory.Tables[product.TableName].Rows.Count > 0))
                {
                    quantityDeviation = Convert.ToDecimal(dsCnEdit.Tables[cnDt.TableName].Rows[rowindex]["OrderQty"]) +
                                        (Convert.ToDecimal(dsCnEdit.Tables[cnDt.TableName].Rows[rowindex]["OrderQty"]) *
                                         (Convert.ToDecimal(
                                             dsInventory.Tables[product.TableName].Rows[0]["QuantityDeviation"]) / 100));
                }
            }

            return quantityDeviation;
        }

        /// <summary>
        ///     Calculate priceDeviation
        /// </summary>
        /// <returns></returns>
        private decimal GetPriceDeviation(int rowindex)
        {
            decimal priceDeviation = 0;

            if ((dsCnEdit.Tables[cnDt.TableName] != null) && (dsCnEdit.Tables[cnDt.TableName].Rows.Count > 0))
            {
                product.GetList(dsInventory, dsCnEdit.Tables[cnDt.TableName].Rows[0]["ProductCode"].ToString(),
                    hf_ConnStr.Value);

                if ((dsInventory.Tables[product.TableName] != null) &&
                    (dsInventory.Tables[product.TableName].Rows.Count > 0))
                {
                    priceDeviation = Convert.ToDecimal(dsCnEdit.Tables[cnDt.TableName].Rows[rowindex]["Price"]) +
                                     (Convert.ToDecimal(dsCnEdit.Tables[cnDt.TableName].Rows[rowindex]["Price"]) *
                                      (Convert.ToDecimal(dsInventory.Tables[product.TableName].Rows[0]["PriceDeviation"]) /
                                       100));
                }
            }

            return priceDeviation;
        }

        /// <summary>
        /// </summary>
        private void CalculateDetail()
        {
            var ddl_Product = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
            var ddl_CnType = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_CnType") as ASPxComboBox;
            var ddl_Rec = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Rec") as ASPxComboBox;

            var se_RecQty = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("se_RecQty") as ASPxSpinEdit;

            var se_CurrNetAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("se_CurrNetAmt") as ASPxSpinEdit;
            var se_CurrTaxAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("se_CurrTaxAmt") as ASPxSpinEdit;
            var se_CurrTotalAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("se_CurrTotalAmt") as ASPxSpinEdit;

            var lbl_CurrNetAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_CurrNetAmt") as Label;
            var lbl_CurTaxAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_CurrTaxAmt") as Label;
            var lbl_CurrTotalAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_CurrTotalAmt") as Label;

            var lbl_NetAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_NetAmt") as Label;
            var lbl_TaxAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_TaxAmt") as Label;
            var lbl_TotalAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_TotalAmt") as Label;


            var lbl_RecPrice = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_RecPrice") as Label;
            var lbl_RecTaxType = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_RecTaxType") as Label;
            var lbl_RecTaxRate = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_RecTaxRate") as Label;

            //var lbl_RecNetAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_RecNetAmt") as Label;
            //var lbl_RecTaxAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_RecTaxAmt") as Label;

            decimal currNetAmt = 0m;
            decimal currTaxAmt = 0m;
            decimal currTotalAmt = 0m;

            decimal netAmt = 0m;
            decimal taxAmt = 0m;
            decimal totalAmt = 0m;

            decimal currRate = Convert.ToDecimal(txt_ExRateAu.Text);

            string sql = string.Format("SELECT TOP(1) TaxType, TaxRate, RecQty, Rate, CurrNetAmt, CurrTaxAmt, CurrTotalAmt, NetAmt, TaxAmt, TotalAmt FROM PC.RECDt WHERE RecNo = '{0}' AND ProductCode = '{1}' ", ddl_Rec.Text, ddl_Product.Text.Split(' ')[0].Trim());
            DataTable dt = rec.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            if (dt != null && dt.Rows.Count > 0)
            {

                DataRow dr = dt.Rows[0];
                string recTaxType = dr["TaxType"].ToString();
                decimal rectTaxRate = Convert.ToDecimal(dr["TaxRate"]);

                // Check if CN Type is Qty. or Amount
                if (ddl_CnType.SelectedIndex == CN_QTY_INDEX) // Qty
                {
                    decimal cnQty = Convert.ToDecimal(se_RecQty.Value);
                    decimal unitRate = Convert.ToDecimal(dr["Rate"]);
                    decimal recQty = Convert.ToDecimal(dr["RecQty"]);
                    decimal recBaseQty = recQty * unitRate;

                    decimal recCurrNetAmt = Convert.ToDecimal(dr["CurrNetAmt"]);
                    decimal recCurrTaxAmt = Convert.ToDecimal(dr["CurrTaxAmt"]);
                    //decimal recCurrTotalAmt = Convert.ToDecimal(dr["CurrTotalAmt"]);
                    decimal recNetAmt = Convert.ToDecimal(dr["NetAmt"]);
                    decimal recTaxAmt = Convert.ToDecimal(dr["TaxAmt"]);
                    //decimal recTotalAmt = Convert.ToDecimal(dr["TotalAmt"]);


                    if (cnQty == recBaseQty) // Full return
                    {
                        currNetAmt = recCurrNetAmt;
                        currTaxAmt = recCurrTaxAmt;

                        netAmt = recNetAmt;
                        taxAmt = recTaxAmt;

                    }
                    else
                    {
                        decimal currNetPerUnit = RoundAmt(recCurrNetAmt / recBaseQty);
                        decimal currTaxPerUnit = RoundAmt(recCurrTaxAmt / recBaseQty);

                        currNetAmt = RoundAmt(currNetPerUnit * cnQty);
                        currTaxAmt = RoundAmt(currTaxPerUnit * cnQty);

                        netAmt = RoundAmt(currNetAmt * currRate);
                        taxAmt = RoundAmt(currTaxAmt * currRate);

                    }

                    currTotalAmt = currNetAmt + currTaxAmt;
                    totalAmt = netAmt + taxAmt;


                }
                else // Amount
                {


                    se_RecQty.Value = 0;
                    currNetAmt = Convert.ToDecimal(se_CurrNetAmt.Text);
                    currTaxAmt = Convert.ToDecimal(se_CurrTaxAmt.Text);
                    currTotalAmt = currNetAmt + currTaxAmt;

                    netAmt = RoundAmt(currNetAmt * currRate);
                    taxAmt = RoundAmt(currTaxAmt * currRate);
                    totalAmt = netAmt + taxAmt;

                    //lbl_RecNetAmt.Text = se_CurrNetAmt.Text;
                    //lbl_RecTaxAmt.Text = se_CurrTaxAmt.Text;
                    //lbl_RecNetAmt.Text = (Convert.ToDecimal(se_CurrNetAmt.Text) + Convert.ToDecimal(se_CurrTaxAmt.Text)).ToString();
                }


                se_CurrNetAmt.Value = currNetAmt;
                se_CurrTaxAmt.Value = currTaxAmt;
                se_CurrTotalAmt.Value = currTotalAmt;

                lbl_CurrNetAmt.Text = string.Format(DefaultAmtFmt, currNetAmt);
                lbl_CurTaxAmt.Text = string.Format(DefaultAmtFmt, currTaxAmt);
                lbl_CurrTotalAmt.Text = string.Format(DefaultAmtFmt, currTotalAmt);

                lbl_NetAmt.Text = string.Format(DefaultAmtFmt, netAmt);
                lbl_TaxAmt.Text = string.Format(DefaultAmtFmt, taxAmt);
                lbl_TotalAmt.Text = string.Format(DefaultAmtFmt, totalAmt);
            }
        }

        private double GetProductRemainQty(string recNo, string locationCode, string productCode)
        {
            string cnNo = lbl_CnNo.Text;
            double recQty = 0;
            double cnQty = 0;

            string sql = string.Format("SELECT ISNULL(SUM(RecQty*ISNULL(Rate,1)), 0) FROM PC.RecDt WHERE RecNo = '{0}' AND LocationCode = '{1}' AND ProductCode = '{2}'", recNo, locationCode, productCode);
            DataTable dt = cn.DbExecuteQuery(sql, null, hf_ConnStr.Value);
            if (dt.Rows.Count > 0)
                recQty = Convert.ToDouble(dt.Rows[0][0]);


            sql = string.Format("SELECT ISNULL(SUM(RecQty), 0) FROM PC.Cn cn JOIN PC.CnDt cndt on cndt.CnNo = cn.CnNo WHERE cn.DocStatus = 'Committed' AND cndt.CnType = 'Q' AND cndt.RecNo = '{0}' AND cndt.Location = '{1}'AND cndt.ProductCode = '{2}' AND cn.CnNo <> '{3}'", recNo, locationCode, productCode, cnNo);
            dt = cn.DbExecuteQuery(sql, null, hf_ConnStr.Value);
            if (dt.Rows.Count > 0)
                cnQty = Convert.ToDouble(dt.Rows[0][0]);
            return recQty - cnQty;
        }

        private double GetProductRemainAmount(string recNo, string locationCode, string productCode)
        {
            double recAmt = 0;

            string sql = string.Format("SELECT ISNULL(SUM(CurrNetAmt), 0) FROM PC.RecDt WHERE RecNo = '{0}' AND ProductCode = '{1}'", recNo, productCode);
            DataTable dt = cn.DbExecuteQuery(sql, null, hf_ConnStr.Value);
            if (dt.Rows.Count > 0)
                recAmt = Convert.ToDouble(dt.Rows[0][0]);


            return recAmt;

            //double productAmount = 0.000000;

            //string conStr = LoginInfo.ConnStr;
            //SqlConnection conn = new SqlConnection(conStr);
            //conn.Open();

            //string sql = "EXEC [IN].GetListFIFO '" + locationCode + "', '" + productCode + "', 0";
            //SqlCommand cmd = new SqlCommand(sql, conn);
            //SqlDataReader reader = cmd.ExecuteReader();
            //reader.Read();
            //if (reader["Amount"].ToString() != string.Empty)
            //    productAmount = Convert.ToDouble(reader["Amount"].ToString());
            //else
            //    productAmount = 0;

            //conn.Close();

            //return productAmount;

        }

        private decimal GetOnHand(string productCode, string locationCode, string strDate)
        {
            if (dsCnEdit.Tables[prDt.TableName] != null)
            {
                dsCnEdit.Tables[prDt.TableName].Clear();
            }

            var get = prDt.GetStockSummary(dsCnEdit, productCode, locationCode, strDate, LoginInfo.ConnStr);

            if (get)
            {
                if (dsCnEdit.Tables[prDt.TableName].Rows[0]["OnHand"].ToString() != string.Empty &&
                    dsCnEdit.Tables[prDt.TableName].Rows[0]["OnHand"] != null)
                {
                    return decimal.Parse(dsCnEdit.Tables[prDt.TableName].Rows[0]["OnHand"].ToString());
                }
            }

            return 0;
        }

        #endregion

        #region "Button Process"

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            pop_ConfirmDelete.ShowOnPageLoad = true;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private void Delete()
        {
            for (var i = grd_CnDt.Rows.Count - 1; i >= 0; i--)
            {
                var chk_Item = grd_CnDt.Rows[i].Cells[0].FindControl("chk_Item") as CheckBox;
                if (chk_Item.Checked)
                {
                    var drCnDt = dsCnEdit.Tables[cnDt.TableName].Rows[i];

                    if (drCnDt.RowState != DataRowState.Deleted)
                    {
                        //Delete Inventory
                        for (var j = dsCnEdit.Tables[inv.TableName].Rows.Count - 1; j >= 0; j--)
                        {
                            var drInv = dsCnEdit.Tables[inv.TableName].Rows[j];
                            if (drInv.RowState != DataRowState.Deleted)
                            {
                                if (drInv["HdrNo"].ToString() == drCnDt["CnNo"].ToString() &&
                                    drInv["DtNo"].ToString() == drCnDt["CnDtNo"].ToString())
                                {
                                    drInv.Delete();
                                }
                            }
                        }

                        drCnDt.Delete();
                    }
                }
            }

            grd_CnDt.DataSource = dsCnEdit.Tables[cnDt.TableName];
            grd_CnDt.EditIndex = -1;
            grd_CnDt.DataBind();

            if (grd_CnDt.Rows.Count == 0)
            {
                txt_CnDate.Enabled = true;
                txt_DocDate.Enabled = true;
                ddl_Vendor.Enabled = true;
            }

        }

        private int GetLastCnDtNo()
        {
            int cnDtno = 0;

            var dt = dsCnEdit.Tables[cnDt.TableName];

            foreach (DataRow dr in dt.Rows)
            {

                cnDtno = Convert.ToInt32(dr["CnDtNo"]);
            }

            return cnDtno;
        }


        private string GetLastRecNo()
        {
            string recNo = string.Empty;

            var dt = dsCnEdit.Tables[cnDt.TableName];

            foreach (DataRow dr in dt.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                    recNo = dr["RecNo"].ToString();
            }


            return recNo;
        }

        private void Create()
        {
            int cnDtNo = GetLastCnDtNo() + 1;

            var drNew = dsCnEdit.Tables[cnDt.TableName].NewRow();

            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                drNew["CnNO"] = cn.GetNewID(DateTime.Parse(txt_CnDate.Text), hf_ConnStr.Value);
            }
            else
            {
                drNew["CnNo"] = lbl_CnNo.Text;
            }

            //if (dsCnEdit.Tables[cnDt.TableName].Rows.Count <= 0)
            //{
            //    drNew["CnDtNo"] = 1;
            //}
            //else
            //{
            //    drNew["CnDtNo"] = cnDtNo;
            //}
            drNew["CnDtNo"] = cnDtNo;

            drNew["RecNo"] = GetLastRecNo();
            drNew["ProductCode"] = string.Empty;
            drNew["Location"] = string.Empty;
            drNew["UnitCode"] = string.Empty; //hong 20130904 visible=false
            drNew["NetAmt"] = "0.00";
            drNew["TaxAmt"] = "0.00";
            drNew["TotalAmt"] = "0.00"; //hong 20130904 visible=false
            drNew["CurrNetAmt"] = "0.00";
            drNew["CurrTaxAmt"] = "0.00";
            drNew["CurrTotalAmt"] = "0.00"; //hong 20130904 visible=false

            dsCnEdit.Tables[cnDt.TableName].Rows.Add(drNew);

            grd_CnDt.DataSource = dsCnEdit.Tables[cnDt.TableName];
            grd_CnDt.EditIndex = dsCnEdit.Tables[cnDt.TableName].Rows.Count - 1;
            grd_CnDt.DataBind();

            txt_CnDate.Enabled = false;
            ddl_Vendor.Enabled = false;
            CnEditMode = "New";

            //var lbl_NetAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_NetAmt") as Label;
            //var lbl_TaxAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_TaxAmt") as Label;

            //lbl_NetAmt.Visible = true;
            //lbl_TaxAmt.Visible = true;

            //lbl_TaxAmt.Text = "0.00";
            //lbl_NetAmt.Text = "0.00";

            var ddl_TaxType = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_TaxType") as ASPxComboBox;

        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_NewRcvDt_Click(object sender, EventArgs e)
        {
            var currentUrl = Request.CurrentExecutionFilePath;
            var ID = string.Empty;

            if (Request.QueryString["ID"] != null)
            {
                ID = "&ID=" + Request.QueryString["ID"];
            }

            var statusValue = "&Status=New";

            Response.Redirect(currentUrl + "?MODE=" + Request.QueryString["MODE"] + ID + statusValue);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btn_Save_Click(object sender, EventArgs e)
        private void Save()
        {
            Page.Validate();
            if (Page.IsValid)
            {
                string _action = string.Empty;
                string cnNo = string.Empty;
                var MODE = Request.Params["MODE"];

                if (MODE.ToUpper() == "EDIT")
                {
                    _action = "MODIFY";
                    var drSave = dsCnEdit.Tables[cn.TableName].Rows[0];

                    drSave["Description"] = (txt_Desc.Text != null ? txt_Desc.Text.Trim() : string.Empty);
                    drSave["DocNo"] = txt_DocNo.Text.Trim();
                    drSave["UpdatedDate"] = ServerDateTime;
                    drSave["UpdatedBy"] = LoginInfo.LoginName;

                    cnNo = drSave["CnNo"].ToString();
                }
                else
                {
                    _action = "CREATE";
                    cn.GetStructure(dsCnEdit, hf_ConnStr.Value);

                    var drSaveNew = dsCnEdit.Tables[cn.TableName].NewRow();

                    // For new
                    drSaveNew["CnNo"] = dsCnEdit.Tables[cnDt.TableName].Rows[0]["CnNo"].ToString();
                    drSaveNew["CnDate"] =
                        DateTime.Parse(txt_CnDate.Text)
                            .AddHours(ServerDateTime.Hour)
                            .AddMinutes(ServerDateTime.Minute)
                            .AddSeconds(ServerDateTime.Second);
                    drSaveNew["Description"] = (txt_Desc.Text != null ? txt_Desc.Text : string.Empty);
                    drSaveNew["DocNo"] = txt_DocNo.Text;

                    if (txt_DocDate.Text != string.Empty)
                    {
                        drSaveNew["DocDate"] = DateTime.Parse(txt_DocDate.Text);
                    }
                    else
                    {
                        drSaveNew["DocDate"] = DBNull.Value;
                    }

                    drSaveNew["DocStatus"] = "Saved";
                    drSaveNew["VendorCode"] = ddl_Vendor.Value == null ? DBNull.Value : ddl_Vendor.Value;
                    //drSaveNew["CurrencyCode"] = lbl_Currency.Text == string.Empty ? config.GetValue("APP", "BU", "DefaultCurrency", LoginInfo.ConnStr) : lbl_Currency.Text;
                    //drSaveNew["ExRateAudit"] = lbl_ExRateAu.Text == string.Empty ? 1 : Convert.ToDecimal(lbl_ExRateAu.Text);
                    drSaveNew["CurrencyCode"] = ddl_Currency.Value.ToString();
                    drSaveNew["ExRateAudit"] = txt_ExRateAu.Text == string.Empty ? 1 : Convert.ToDecimal(txt_ExRateAu.Text);
                    drSaveNew["ExportStatus"] = false;
                    drSaveNew["CreatedDate"] = ServerDateTime;
                    drSaveNew["CreatedBy"] = LoginInfo.LoginName;
                    drSaveNew["UpdatedDate"] = ServerDateTime;
                    drSaveNew["UpdatedBy"] = LoginInfo.LoginName;

                    dsCnEdit.Tables[cn.TableName].Rows.Add(drSaveNew);
                }

                // var result = cn.SaveToCnCnDtAndInv(dsCnEdit, hf_ConnStr.Value);
                var result = cn.Save(dsCnEdit, hf_ConnStr.Value);

                if (result)
                {
                    //CreateAccountMap(dsCnEdit, LoginInfo.ConnStr);
                    cnNo = dsCnEdit.Tables[cn.TableName].Rows[0]["CnNo"].ToString();

                    _transLog.Save("PC", "CN", cnNo, _action, string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);
                    Response.Redirect("Cn.aspx?ID=" + cnNo + "&BuCode=" + Request.Params["BuCode"]);
                }
            }
        }

        private void Commit()
        {
            // Check period
            //DateTime openPeriod = period.GetLatestOpenEndDate(hf_ConnStr.Value).AddHours(23).AddMinutes(55);
            //DateTime cnDate = DateTime.Parse(txt_DocDate.Text);
            //DateTime committedDate = DateTime.Today;

            // ---------------------------------------------------------------------------------------------------------

            var MODE = Request.Params["MODE"];


            if (MODE.ToUpper() == "EDIT")
            {
                // Header (PC.CN)
                var drSave = dsCnEdit.Tables[cn.TableName].Rows[0];

                //drSave["CnNo"] = lbl_CnNo.Text;
                drSave["DocStatus"] = "Committed";
                drSave["Description"] = (txt_Desc.Text != null ? txt_Desc.Text : string.Empty);
                drSave["DocNo"] = txt_DocNo.Text;
                drSave["UpdatedDate"] = ServerDateTime;
                drSave["UpdatedBy"] = LoginInfo.LoginName;
            }
            else
            {
                dsCnEdit.Tables[inv.TableName].Clear();
                cn.GetStructure(dsCnEdit, hf_ConnStr.Value);
                //cn.GetStructure(dsCnEdit, LoginInfo.ConnStr);

                var drSaveNew = dsCnEdit.Tables[cn.TableName].NewRow();

                // For new
                drSaveNew["CnNo"] = dsCnEdit.Tables[cnDt.TableName].Rows[0]["CnNo"].ToString();
                drSaveNew["CnDate"] = DateTime.Parse(txt_CnDate.Text).AddHours(ServerDateTime.Hour).AddMinutes(ServerDateTime.Minute).AddSeconds(ServerDateTime.Second);
                drSaveNew["Description"] = (txt_Desc.Text != null ? txt_Desc.Text : string.Empty);
                drSaveNew["DocNo"] = txt_DocNo.Text;

                if (txt_DocDate.Text != string.Empty)
                {
                    drSaveNew["DocDate"] = DateTime.Parse(txt_DocDate.Text);
                }
                else
                {
                    drSaveNew["DocDate"] = DBNull.Value;
                }

                drSaveNew["DocStatus"] = "Committed";
                drSaveNew["VendorCode"] = ddl_Vendor.Value == null ? DBNull.Value : ddl_Vendor.Value;
                //drSaveNew["CurrencyCode"] = lbl_Currency.Text == string.Empty ? config.GetValue("APP", "BU", "DefaultCurrency", LoginInfo.ConnStr) : "1";
                //drSaveNew["ExRateAudit"] = lbl_ExRateAu.Text == string.Empty ? 1 : Convert.ToDecimal(lbl_ExRateAu.Text);
                drSaveNew["CurrencyCode"] = ddl_Currency.Value.ToString();
                drSaveNew["ExRateAudit"] = txt_ExRateAu.Text == string.Empty ? 1 : Convert.ToDecimal(txt_ExRateAu.Text);
                drSaveNew["ExportStatus"] = false;
                drSaveNew["CreatedDate"] = ServerDateTime;
                drSaveNew["CreatedBy"] = LoginInfo.LoginName;
                drSaveNew["UpdatedDate"] = ServerDateTime;
                drSaveNew["UpdatedBy"] = LoginInfo.LoginName;

                dsCnEdit.Tables[cn.TableName].Rows.Add(drSaveNew);
            }


            // Detail
            foreach (DataRow drCnDetail in dsCnEdit.Tables[cnDt.TableName].Rows)
            {
                // Check Onhand/Amount for processing Credit Note.
                string recNo = drCnDetail["RecNo"].ToString();
                string locationCode = drCnDetail["Location"].ToString();
                string productCode = drCnDetail["ProductCode"].ToString();
                string CnTypeValue = drCnDetail["CnType"].ToString();
                double cnQty = Convert.ToDouble(drCnDetail["RecQty"]);
                double cnAmt = Convert.ToDouble(drCnDetail["NetAmt"].ToString()); ;

                //if (CnTypeValue == "Q") // Q = Qantity
                //{
                //    if (cnQty > GetProductRemainQty(recNo, locationCode, productCode))
                //    {
                //        lbl_Warning.Text = "Not enough quantity left to process.";
                //        pop_Warning.ShowOnPageLoad = true;
                //        return;
                //    }
                //}
                //else // CnTypeValue = "A" (Amount)
                //{
                //    if (cnAmt > GetProductRemainAmount(recNo, locationCode, productCode))
                //    {
                //        lbl_Warning.Text = "Not enough amount left to process";
                //        pop_Warning.ShowOnPageLoad = true;
                //        return;
                //    }
                //}
                if (CnTypeValue == "Q")
                {
                    double recQty = GetProductRemainQty(recNo, locationCode, productCode);
                    if (cnQty > recQty)
                    {
                        lbl_Warning.Text = string.Format("The product quantity is exceed than received. ({0:N3}).", recQty);
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }
                }
                else
                {
                    double recAmt = GetProductRemainAmount(recNo, locationCode, productCode);
                    if (cnAmt > recAmt)
                    {
                        lbl_Warning.Text = string.Format("The product amount is exceed than received. ({0:N3}).", recAmt);
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }
                }


                UpdateInventoryForCommit(DateTime.Now, drCnDetail);


            }

            var result = cn.SaveToCnCnDtAndInv(dsCnEdit, hf_ConnStr.Value);
            //var result = cn.SaveToCnCnDtAndInv(dsCnEdit, LoginInfo.ConnStr);

            if (result)
            {
                var cnNo = dsCnEdit.Tables[cn.TableName].Rows[0]["CnNo"].ToString();


                // Update Average Cost
                string costMethod = config.GetValue("IN", "SYS", "COST", LoginInfo.ConnStr);
                if (costMethod.ToUpper() == "AVCO")
                {
                    Blue.BL.IN.Inventory inv = new Blue.BL.IN.Inventory();
                    inv.UpdateAverageCostByDocument(cnNo, LoginInfo.ConnStr);
                }

                var startDate = period.GetStartDate(ServerDateTime.Date, hf_ConnStr.Value);
                var endDate = period.GetEndDate(ServerDateTime.Date, hf_ConnStr.Value);

                dsInventory.Clear();

                if (Request.Params["MODE"].ToUpper() == "EDIT")
                {
                    cnNo = lbl_CnNo.Text;
                    _transLog.Save("PC", "CN", cnNo, "COMMIT", string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);

                    Response.Redirect("Cn.aspx?ID=" + cnNo + "&BuCode=" + LoginInfo.BuInfo.BuCode);

                }
                else
                {
                    _transLog.Save("PC", "CN", cnNo, "CREATE", string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);
                    _transLog.Save("PC", "CN", cnNo, "COMMIT", string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);

                    Response.Redirect("Cn.aspx?ID=" + cnNo + "&BuCode=" + LoginInfo.BuInfo.BuCode);
                }
            }
        }

        private void UpdateInventoryForCommit(DateTime committedDate, DataRow drCnDetail)
        {
            string recNo = drCnDetail["RecNo"].ToString();
            string locationCode = drCnDetail["Location"].ToString();
            string productCode = drCnDetail["ProductCode"].ToString();

            string cnType = drCnDetail["CnType"].ToString();

            SqlConnection conn = new SqlConnection(hf_ConnStr.Value);
            conn.Open();

            string sqlDel = "DELETE FROM [IN].Inventory WHERE HdrNo = @hdrNo AND DtNo = @DtNo";
            SqlCommand cmdDel = new SqlCommand(sqlDel, conn);
            cmdDel.Parameters.AddWithValue("hdrNo", drCnDetail["CnNo"].ToString());
            cmdDel.Parameters.AddWithValue("dtNo", drCnDetail["CnDtNo"].ToString());
            cmdDel.ExecuteNonQuery();



            if (cnType == "Q")
            {

                decimal recQty = Convert.ToDecimal(drCnDetail["RecQty"].ToString());
                string unitCode = drCnDetail["UnitCode"].ToString();
                string invUnit = prodUnit.GetInvenUnit(productCode, hf_ConnStr.Value);
                decimal recRate = prodUnit.GetConvRate(productCode, unitCode, hf_ConnStr.Value);
                decimal cnQty = recQty * recRate;

                string sqlExe = "EXEC [IN].GetListFIFO @locationCode, @productCode, 2"; // Show only available onhand/remain
                SqlCommand cmd = new SqlCommand(sqlExe, conn);
                cmd.Parameters.AddWithValue("locationCode", locationCode);
                cmd.Parameters.AddWithValue("productCode", productCode);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                adapter.Fill(dt);


                bool isAvailable = false;
                int index = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["HdrNo"].ToString() == recNo && cnQty <= Convert.ToDecimal(dt.Rows[i]["Remain"].ToString()))
                    {
                        isAvailable = true;
                        index = i;
                        break;
                    }
                }
                if (!isAvailable)
                    index = 0;

                int invNo = 0;
                //while ((cnQty > 0) && reader.Read())
                for (int i = index; i < dt.Rows.Count; i++)
                {
                    string refNo = dt.Rows[i]["HdrNo"].ToString(); // represent for HdrNo
                    int refDtNo = Convert.ToInt32(dt.Rows[i]["DtNo"].ToString()); // represent for DtNo
                    int lotsNo = Convert.ToInt32(dt.Rows[i]["InvNo"].ToString()); // represent for InvNo

                    decimal fifoCost = Convert.ToDecimal(dt.Rows[i]["Amount"].ToString());
                    decimal fifoQty = Convert.ToDecimal(dt.Rows[i]["Remain"].ToString());

                    decimal invQty = Convert.ToDecimal("0");

                    if (cnQty > fifoQty)
                    {
                        invQty = fifoQty;
                        cnQty = cnQty - fifoQty;
                    }
                    else
                    {
                        invQty = cnQty;
                        cnQty = Convert.ToDecimal("0.00");
                    }


                    // Save to [IN].Inventory
                    var drInv = dsCnEdit.Tables[inv.TableName].NewRow();

                    invNo += 1;

                    drInv["HdrNo"] = drCnDetail["CnNo"].ToString();
                    drInv["DtNo"] = drCnDetail["CnDtNo"].ToString();
                    drInv["InvNo"] = invNo;
                    drInv["ProductCode"] = productCode;
                    drInv["Location"] = locationCode;
                    drInv["IN"] = Convert.ToDecimal("0.00");
                    drInv["OUT"] = invQty;
                    drInv["Amount"] = fifoCost;
                    //drInv["PriceOnLots"] = priceOnLots * (-1);
                    drInv["PriceOnLots"] = Convert.ToDecimal(drCnDetail["NetAmt"]) * (-1);
                    //drInv["FIFOAudit"] = fifoCost;
                    //drInv["FIFOMng"] = System.DBNull.Value;
                    //drInv["FIFOBank"] = System.DBNull.Value;
                    //drInv["MAvgAudit"] = System.DBNull.Value;
                    //drInv["MAvgMng"] = System.DBNull.Value;
                    //drInv["MAvgBank"] = System.DBNull.Value;
                    //drInv["PAvgAudit"] = System.DBNull.Value;
                    //drInv["PAvgMng"] = System.DBNull.Value;
                    //drInv["PAvgBank"] = System.DBNull.Value;
                    //drInv["RptAudit"] = System.DBNull.Value;
                    //drInv["RptMng"] = System.DBNull.Value;
                    //drInv["RptBank"] = System.DBNull.Value;
                    drInv["RefNo"] = refNo;
                    drInv["RefDtNo"] = refDtNo;
                    drInv["LotsNo"] = lotsNo;
                    drInv["CommittedDate"] = committedDate;
                    drInv["Type"] = "CR";

                    dsCnEdit.Tables[inv.TableName].Rows.Add(drInv);

                    if (cnQty <= 0)
                        break;

                } //while

            }
            else if (cnType == "A")
            {
                decimal cnAmt = Convert.ToDecimal(drCnDetail["NetAmt"].ToString());

                //string sql = "SELECT HdrNo, DtNo, InvNo, ProductCode, Location, [IN], [OUT], [IN] as Remain, Round(PriceOnLots/[IN], 2) as Amount ";
                //sql += " FROM [IN].Inventory";
                //sql += " WHERE HdrNo = @RecNo AND ProductCode = @ProductCode"; // Show only available onhand/remain;
                //sql += " ORDER BY HdrNo, DtNo, InvNo";

                string sql = "SELECT HdrNo, DtNo, SUM([IN]) as Qty, rd.NetAmt";
                sql += " FROM [IN].Inventory i";
                sql += " LEFT JOIN PC.RecDt rd ON rd.RecNo = i.HdrNo AND rd.RecDtNo = i.DtNo";
                sql += " WHERE HdrNo = @RecNo AND i.ProductCode = @ProductCode"; // Show only available onhand/remain;
                sql += " GROUP BY HdrNo, DtNo, rd.NetAmt";

                SqlCommand cmd = new SqlCommand(sql.ToString(), conn);
                cmd.Parameters.AddWithValue("RecNo", recNo);
                cmd.Parameters.AddWithValue("ProductCode", productCode);
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                string refNo = reader["HdrNo"].ToString();
                int refDtNo = Convert.ToInt32(reader["DtNo"].ToString());


                decimal recTotal = Convert.ToDecimal(reader["NetAmt"].ToString());
                decimal recQty = Convert.ToDecimal(reader["Qty"].ToString());

                // Remove old transaction
                var drInv = dsCnEdit.Tables[inv.TableName].NewRow();
                drInv["HdrNo"] = drCnDetail["CnNo"].ToString();
                drInv["DtNo"] = drCnDetail["CnDtNo"].ToString();
                drInv["InvNo"] = 1;
                drInv["ProductCode"] = productCode;
                drInv["Location"] = locationCode;
                drInv["IN"] = Convert.ToDecimal("0.00");
                drInv["OUT"] = recQty;
                drInv["Amount"] = RoundAmt(recTotal / recQty);
                drInv["PriceOnLots"] = recTotal * (-1);
                //drInv["FIFOAudit"] = Convert.ToDecimal("0.00");
                //drInv["FIFOMng"] = System.DBNull.Value;
                //drInv["FIFOBank"] = System.DBNull.Value;
                //drInv["MAvgAudit"] = System.DBNull.Value;
                //drInv["MAvgMng"] = System.DBNull.Value;
                //drInv["MAvgBank"] = System.DBNull.Value;
                //drInv["PAvgAudit"] = System.DBNull.Value;
                //drInv["PAvgMng"] = System.DBNull.Value;
                //drInv["PAvgBank"] = System.DBNull.Value;
                //drInv["RptAudit"] = System.DBNull.Value;
                //drInv["RptMng"] = System.DBNull.Value;
                //drInv["RptBank"] = System.DBNull.Value;
                drInv["RefNo"] = refNo;
                drInv["RefDtNo"] = refDtNo;
                drInv["CommittedDate"] = committedDate;
                drInv["Type"] = "CR";

                dsCnEdit.Tables[inv.TableName].Rows.Add(drInv);

                // Adding new transaction
                // Save to [IN].Inventory
                drInv = dsCnEdit.Tables[inv.TableName].NewRow();
                drInv["HdrNo"] = drCnDetail["CnNo"].ToString();
                drInv["DtNo"] = drCnDetail["CnDtNo"].ToString();
                drInv["InvNo"] = 2;
                drInv["ProductCode"] = productCode;
                drInv["Location"] = locationCode;
                drInv["IN"] = recQty;
                drInv["OUT"] = Convert.ToDecimal("0.00");
                drInv["Amount"] = RoundAmt((recTotal - cnAmt) / recQty);
                drInv["PriceOnLots"] = recTotal - cnAmt; //invAmt * fifoQty; // Using for calculation Average
                //drInv["FIFOAudit"] = Convert.ToDecimal("0.00");
                //drInv["FIFOMng"] = System.DBNull.Value;
                //drInv["FIFOBank"] = System.DBNull.Value;
                //drInv["MAvgAudit"] = System.DBNull.Value;
                //drInv["MAvgMng"] = System.DBNull.Value;
                //drInv["MAvgBank"] = System.DBNull.Value;
                //drInv["PAvgAudit"] = System.DBNull.Value;
                //drInv["PAvgMng"] = System.DBNull.Value;
                //drInv["PAvgBank"] = System.DBNull.Value;
                //drInv["RptAudit"] = System.DBNull.Value;
                //drInv["RptMng"] = System.DBNull.Value;
                //drInv["RptBank"] = System.DBNull.Value;
                drInv["RefNo"] = refNo;
                drInv["RefDtNo"] = refDtNo;
                drInv["CommittedDate"] = committedDate;
                drInv["Type"] = "CR";

                dsCnEdit.Tables[inv.TableName].Rows.Add(drInv);

            }

        }


        /// <summary>
        ///     Error message
        /// </summary>
        /// <param name="msg"></param>
        private void MessageBox(string msg)
        {
            var lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
        }
        protected void btn_ConfirmSave_Click(object sender, EventArgs e)
        {
            Save();
            pop_ConfirmSave.ShowOnPageLoad = false;
        }
        protected void btn_ConfirmCommit_Click(object sender, EventArgs e)
        {
            Commit();

            pop_ConfirmCommit.ShowOnPageLoad = false;
        }
        protected void btn_ConfirmDelete_Click(object sender, EventArgs e)
        {
            Delete();
            pop_ConfirmDelete.ShowOnPageLoad = false;
        }
        protected void btn_Save_Click(object sender, EventArgs e)
        {
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                if (ddl_Vendor.Value == null)
                {
                    lbl_Warning.Text = "Vendor is required.";
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }
            }

            if (grd_CnDt.Rows.Count == 0)
            {
                lbl_Warning.Text = "No more detail.";
                pop_Warning.ShowOnPageLoad = true;
                return;
            }

            pop_ConfirmSave.ShowOnPageLoad = true;
        }
        protected void btn_Commit_Click(object sender, EventArgs e)
        {
            if (grd_CnDt.Rows.Count == 0)
            {
                lbl_Warning.Text = "Record not commit, Please input detail!";
                pop_Warning.ShowOnPageLoad = true;
                return;
            }

            if (txt_DocNo.Text == string.Empty || txt_DocDate.Text == string.Empty)
            {
                lbl_Warning.Text = "Doc.# and Doc.Date cannot be empty!";
                pop_Warning.ShowOnPageLoad = true;
                return;
            }

            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                if (ddl_Vendor.Value == null)
                {
                    lbl_Warning.Text = "System cannot be processed, please select vendor.";
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }
            }

            pop_ConfirmCommit.ShowOnPageLoad = true;
        }
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
        protected void btn_Create_Click(object sender, EventArgs e)
        {
            DateTime temp;
            if (!DateTime.TryParse(txt_CnDate.Text, out temp))
            {
                lbl_Warning.Text = "Date is invalid.";
                pop_Warning.ShowOnPageLoad = true;
            }
            else if (!DateTime.TryParse(txt_DocDate.Text, out temp))
            {
                lbl_Warning.Text = "Credit Note date is required.";
                pop_Warning.ShowOnPageLoad = true;
            }
            else
            {
                DateTime openDate = period.GetLatestOpenStartDate(hf_ConnStr.Value);


                if (DateTime.Parse(txt_CnDate.Text).Date < openDate)
                {
                    lbl_Warning.Text = "Not allow date was in closed period.";
                    pop_Warning.ShowOnPageLoad = true;
                }
                else if (DateTime.Parse(txt_DocDate.Text).Date < openDate)
                {
                    lbl_Warning.Text = "Not allow CN Date was in closed period.";
                    pop_Warning.ShowOnPageLoad = true;
                }

                else
                {


                    var btn_Save = UpdatePanelHead.FindControl("btn_Save") as ASPxButton;
                    var btn_Commit = UpdatePanelHead.FindControl("btn_Commit") as ASPxButton;
                    var btn_Back = UpdatePanelHead.FindControl("btn_Back") as ASPxButton;

                    Create();
                }
            }
        }
        protected void btn_CancelSave_Click(object sender, EventArgs e)
        {
            pop_ConfirmSave.ShowOnPageLoad = false;
        }
        protected void btn_CancelCommit_Click(object sender, EventArgs e)
        {
            pop_ConfirmCommit.ShowOnPageLoad = false;
        }
        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            pop_ConfirmDelete.ShowOnPageLoad = false;
        }
        protected void btn_WarningPeriod_Click(object sender, EventArgs e)
        {
            pop_WarningPeriod.ShowOnPageLoad = false;
        }


        #endregion


        private void SetVisibleButtons(bool visible)
        {
            // Header
            txt_CnDate.Visible = visible;

            btn_Save.Visible = visible;
            btn_Commit.Visible = visible;
            btn_Back.Visible = visible;
            // Detail
            btn_Create.Visible = visible;
            btn_Delete.Visible = visible;


        }

        // Header
        protected void txt_CnDate_TextChanged(object sender, EventArgs e)
        {
            //var strProduct = string.Empty;
            //var strRecQty = string.Empty;
            //var strLocation = string.Empty;

            //// เช็คว่า Period นั้นถูกปิดแล้วหรือยัง
            //if (grd_CnDt.Rows.Count == 0)
            //{
            //    DateTime cnDate;
            //    if (!DateTime.TryParse(txt_CnDate.Text, out cnDate))
            //    {
            //        lbl_WarningPeriod.Text = "Date is invalid.";
            //        pop_WarningPeriod.ShowOnPageLoad = true;
            //        return;

            //    }
            //    else
            //        if (!period.GetIsValidDate(DateTime.Parse(txt_CnDate.Text), string.Empty, hf_ConnStr.Value))
            //        {
            //            lbl_WarningPeriod.Text = "Date is not allowed in closed period.";
            //            pop_WarningPeriod.ShowOnPageLoad = true;
            //            txt_CnDate.Text = ServerDateTime.ToShortDateString();
            //            return;
            //        }
            //}

            //// Check Period on Date & Store
            //for (var i = 0; i < grd_CnDt.Rows.Count; i++)
            //{
            //    var lbl_Location = grd_CnDt.Rows[0].FindControl("lbl_Location") as Label;

            //    if (!period.GetIsValidDate(DateTime.Parse(txt_CnDate.Text), lbl_Location.Text.Split(':')[0].Trim(), hf_ConnStr.Value))
            //    {
            //        lbl_WarningPeriod.Text = "Date in close period.";
            //        pop_WarningPeriod.ShowOnPageLoad = true;
            //        txt_CnDate.Text = ServerDateTime.ToShortDateString();
            //        return;
            //    }
            //}

            ////Check 
            //for (var ii = 0; ii < grd_CnDt.Rows.Count; ii++)
            //{
            //    var lbl_chkRecQty = grd_CnDt.Rows[ii].FindControl("lbl_RecQty") as Label;
            //    var lbl_Location = grd_CnDt.Rows[ii].FindControl("lbl_Location") as Label;
            //    var lbl_Product = grd_CnDt.Rows[ii].FindControl("lbl_Product") as Label;

            //    strProduct = lbl_Product.Text.Split(':')[0].Trim();
            //    strRecQty = lbl_chkRecQty.Text;
            //    strLocation = lbl_Location.Text.Split(':')[0].Trim();


            //    var onHand = GetOnHand(strProduct, strLocation, txt_CnDate.Text);
            //    decimal SumQtyForChkOnhand = 0;

            //    foreach (GridViewRow grv_Row in grd_CnDt.Rows)
            //    {
            //        if (dsCnEdit.Tables[cnDt.TableName].Rows[grv_Row.RowIndex]["ProductCode"].ToString() != null &&
            //            strRecQty != null)
            //        {
            //            if (dsCnEdit.Tables[cnDt.TableName].Rows[grv_Row.RowIndex]["ProductCode"].ToString() ==
            //                strProduct)
            //            {
            //                SumQtyForChkOnhand += Convert.ToDecimal(strRecQty);
            //            }
            //        }
            //    }

            //    //if (decimal.Parse(strRecQty) > onHand)
            //    //{
            //    //    lbl_Warning.Text = "cannot approve quantity more than on hand at product: " + strProduct.ToString();
            //    //    pop_Warning.ShowOnPageLoad = true;

            //    //    //se_RecQty.Text              = String.Format("{0:N}", onHand);
            //    //}

            //    if (SumQtyForChkOnhand > onHand)
            //    {
            //        lbl_Warning.Text = "No approved quantity more than onhand of product: " + strProduct;
            //        pop_Warning.ShowOnPageLoad = true;

            //        //se_RecQty.Text              = "0.00";
            //    }
            //}
        }

        protected void ddl_Vendor_ItemsRequestedByFilterCondition(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            var ds = new SqlDataSource();
            var comboBox = (ASPxComboBox)source;
            ds.ConnectionString = LoginInfo.ConnStr;


            ds.SelectCommand =
            @"SELECT VendorCode, Name FROM (SELECT v.VendorCode, v.Name, row_number()over(order by v.[VendorCode]) as [rn] 	
            FROM [AP].[Vendor]  v 
	        WHERE ( v.VendorCode + ' ' + v.Name LIKE @filter )) as st where st.[rn] between @startIndex and @endIndex";

            ds.SelectParameters.Clear();
            ds.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            ds.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString(CultureInfo.InvariantCulture));
            ds.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString(CultureInfo.InvariantCulture));
            comboBox.DataSource = ds;
            comboBox.DataBind();
            comboBox.ToolTip = comboBox.Text;
        }

        protected void ddl_Vendor_ItemRequestedByValue(object source, ListEditItemRequestedByValueEventArgs e)
        {
            var ds = new SqlDataSource();
            var comboBox = (ASPxComboBox)source;

            try
            {
                if (e.Value == null)
                    return;

                ds.SelectCommand = @"SELECT VendorCode, Name FROM [AP].[Vendor] WHERE VendorCode=@VendorCode ORDER BY VendorCode";
                ds.ConnectionString = LoginInfo.ConnStr;
                ds.SelectParameters.Clear();
                ds.SelectParameters.Add("VendorCode", TypeCode.String, e.Value.ToString());
                comboBox.DataSource = ds;
                comboBox.DataBind();
                comboBox.ToolTip = comboBox.Text;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
        }

        // Detail
        protected void ddl_Rec_Load(object sender, EventArgs e)
        {
            var ddl_Vendor = UpdatePanelHead.FindControl("ddl_Vendor") as ASPxComboBox;
            var lbl_Vendor = UpdatePanelHead.FindControl("lbl_Vendor") as Label;
            var ddl_Rec = sender as ASPxComboBox;
            string vendor = string.Empty;
            if (ddl_Vendor.Value != null)
            {
                vendor = ddl_Vendor.Value.ToString();
            }
            else if (lbl_Vendor.Text != null)
            {
                vendor = lbl_Vendor.Text.Split(new[] { " : " }, StringSplitOptions.None)[0];
            }




            // Order By RecNo DESC
            DataView dv = rec.GetListByVendor(vendor, hf_ConnStr.Value).DefaultView;
            dv.Sort = "RecNo DESC";
            DataTable dt = dv.ToTable();

            DataRow[] rowArray = rec.GetListByVendor(vendor, hf_ConnStr.Value).Select(string.Format("CurrencyCode = '{0}'", ddl_Currency.Value));
            dt.Clear();
            foreach (DataRow row in rowArray)
                dt.ImportRow(row);


            //ddl_Rec.DataSource = dv.ToTable();
            ddl_Rec.DataSource = dt;
            ddl_Rec.ValueField = "RecNo";
            ddl_Rec.DataBind();
        }

        protected void ddl_Rec_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl_Rec = sender as ASPxComboBox;

            if (ddl_Rec.SelectedIndex > -1)
            {

                var ddl_Location = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Location") as ASPxComboBox;
                var ddl_Product = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Product") as ASPxComboBox;

                if (ddl_Rec != null)
                {

                    var ds = new DataSet();
                    ds = recDt.GetListByRecNo(ds, ddl_Rec.Value.ToString(), hf_ConnStr.Value);
                    ds.Tables[0].DefaultView.RowFilter = "RecNo ='" + ddl_Rec.Value + "'";
                    var dtt = ds.Tables[0].DefaultView.ToTable();
                    var dsLocations = from b in dtt.AsEnumerable()
                                      group b by b.Field<string>("LocationCode")
                                          into g
                                          let row = g.First()
                                          select new
                                          {
                                              LocationCode = row.Field<string>("LocationCode"),
                                              LocationName = row.Field<string>("LocationName")
                                          };

                    ddl_Location.DataSource = dsLocations;
                    ddl_Location.ValueField = "LocationCode";
                    ddl_Location.DataBind();

                    ddl_Location.Value = null;
                    ddl_Location.Text = string.Empty;

                    ddl_Product.Value = null;
                    ddl_Product.Text = string.Empty;

                    var lbl_RecDate = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_RecDate") as Label;
                    var lbl_RecDesc = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_RecDesc") as Label;

                    ListEditItem selectedItem = ((ASPxComboBox)sender).SelectedItem;
                    lbl_RecDate.Text = String.Format("{0:dd/MM/yyyy}", selectedItem.GetValue("RecDate")); ;
                    lbl_RecDesc.Text = selectedItem.GetValue("Description").ToString();


                }
            }
        }

        protected void ddl_Location_Load(object sender, EventArgs e)
        {
            var ddl_Location = sender as ASPxComboBox;
            ASPxComboBox ddl_Rec = null;
            if (ddl_Location.Value != null)
            {
                ddl_Rec = (ASPxComboBox)grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Rec");
            }

            var ds = new DataSet();
            if (ddl_Rec != null)
            {
                ds = recDt.GetListByRecNo(ds, ddl_Rec.Value.ToString(), hf_ConnStr.Value);
                ds.Tables[0].DefaultView.RowFilter = "RecNo ='" + ddl_Rec.Value + "'";
                var dtt = ds.Tables[0].DefaultView.ToTable();
                var groupedData = from b in dtt.AsEnumerable()
                                  group b by b.Field<string>("LocationCode")
                                      into g
                                      let row = g.First()
                                      select new
                                      {
                                          LocationCode = row.Field<string>("LocationCode"),
                                          LocationName = row.Field<string>("LocationName")
                                      };

                ddl_Location.DataSource = groupedData;
                ddl_Location.ValueField = "LocationCode";
                ddl_Location.DataBind();
            }
        }

        protected void ddl_Location_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl_Rec = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Rec") as ASPxComboBox;
            var ddl_Location = sender as ASPxComboBox;
            var ddl_Product = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
            var se_RecQty = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("se_RecQty") as ASPxSpinEdit;

            // ดูว่า Store ที่อยู่ใน Period นั้น ๆ ถูกนับอยู่หรือป่าว ถ้านับอยู่ไม่สามารถสร้างได้
            //if (!period.GetIsValidDate(DateTime.Parse(txt_CnDate.Text), ddl_Location.ClientValue, hf_ConnStr.Value))
            //{
            //    lbl_WarningPeriod.Text = "Store in stock take process.";
            //    pop_WarningPeriod.ShowOnPageLoad = true;

            //    return;
            //}

            hf_Location.Value = ddl_Location.ClientValue;
            ddl_Product.Value = null;
            ddl_Product.Text = string.Empty;


            if (ddl_Location != null)
            {
                var ds = new DataSet();
                ds = recDt.GetListByRecNo(ds, ddl_Rec.Value.ToString(), hf_ConnStr.Value);
                ds.Tables[0].DefaultView.RowFilter = "RecNo ='" + ddl_Rec.Value + "'";
                ddl_Product.DataSource = ds;
                ddl_Product.ValueField = "ProductCode";
                ddl_Product.DataBind();

                //se_RecQty.Value     = 0.00;
            }
        }

        private DataSet getActiveProductByRecNo(string recNo)
        {
            SqlConnection conn = new SqlConnection(hf_ConnStr.Value);
            conn.Open();

            string sql = string.Empty;
            sql += " SELECT r.RecNo, p.ProductCode, p.ProductDesc1 ProductName";
            sql += " FROM PC.RECDt r";
            sql += " LEFT JOIN [IN].Inventory i ON i.RefNo = r.RecNo AND i.ProductCode = r.ProductCode ";
            sql += " LEFT JOIN [IN].Product p ON p.ProductCode = r.ProductCode";
            sql += " WHERE r.RecNo = @RecNo";
            sql += " AND i.HdrNo IS NULL";
            sql += " ORDER BY r.RecDtNo";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("RecNo", recNo);
            SqlDataAdapter adpter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adpter.Fill(ds);
            return ds;
        }


        protected void ddl_Product_Load(object sender, EventArgs e)
        {
            var ddl_CnType = sender as ASPxComboBox;
            var ddl_Product = sender as ASPxComboBox;
            ASPxComboBox ddl_Rec = null;
            if (ddl_Product.Value != null)
            {
                ddl_CnType = (ASPxComboBox)grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_CnType");
                ddl_Rec = (ASPxComboBox)grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Rec");
                var ddl_Location = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Location") as ASPxComboBox;
            }
            var ds = new DataSet();
            if (ddl_Rec != null)
            {
                Blue.BL.APP.Config config = new Blue.BL.APP.Config();
                bool allowAllReceiving = config.GetConfigValue("PC", "CN", "AllowAllReceiving", LoginInfo.ConnStr).ToUpper() == "TRUE";

                if (allowAllReceiving && ddl_CnType.Value.ToString().ToUpper() == "Q")
                    ds = recDt.GetListByRecNo(ds, ddl_Rec.Value.ToString(), hf_ConnStr.Value);
                else
                    ds = getActiveProductByRecNo(ddl_Rec.Value.ToString());
                ds.Tables[0].DefaultView.RowFilter = "RecNo ='" + ddl_Rec.ClientValue + "'";
                ddl_Product.DataSource = ds;
                ddl_Product.ValueField = "ProductCode";
                ddl_Product.DataBind();
            }
        }

        protected void ddl_Product_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl_Product = sender as ASPxComboBox;

            var ddl_Rec = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Rec") as ASPxComboBox;
            var ddl_Location = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Location") as ASPxComboBox;
            //var ddl_Unit = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Unit") as ASPxComboBox;

            var hf_ProductCode = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("hf_ProductCode") as HiddenField;

            if (ddl_Product.ClientValue != null)
            {
                {
                    //ddl_Unit.DataSource = prodUnit.GetLookUp_ProductCode(ddl_Product.ClientValue, hf_ConnStr.Value);
                    //ddl_Unit.DataBind();
                    //ddl_Unit.Value = product.GetInvenUnit(ddl_Product.ClientValue, hf_ConnStr.Value);

                    DataSet ds = new DataSet();
                    ds = recDt.GetListByRecNo(ds, ddl_Rec.Value.ToString(), hf_ConnStr.Value);


                    ds.Tables[0].DefaultView.RowFilter = "ProductCode ='" + ddl_Product.ClientValue + "'";
                    var dtRec = ds.Tables[0].DefaultView.ToTable();
                    if (dtRec.Rows.Count > 0)
                    {

                        //decimal netAmt = Convert.ToDecimal(dtRec.Rows[0]["NetAmt"].ToString());
                        //decimal recQty = Convert.ToDecimal(dtRec.Rows[0]["RecQty"].ToString());
                        //decimal price = Math.Round(netAmt / recQty, 2); //Convert.ToDecimal(dtRec.Rows[0]["Price"].ToString());
                        //decimal convRate = Convert.ToDecimal(dtRec.Rows[0]["Rate"].ToString());
                        //if (convRate == 0)
                        //    convRate = 1;
                        //price = Math.Round(price / convRate, 2, MidpointRounding.AwayFromZero);


                        //hf_Rate.Value = dtRec.Rows[0]["Rate"].ToString();
                        //hf_RecQty.Value = dtRec.Rows[0]["RecQty"].ToString();
                        //hf_NetAmt.Value = dtRec.Rows[0]["NetAmt"].ToString();
                        //hf_TaxAmt.Value = dtRec.Rows[0]["TaxAmt"].ToString();



                        var lbl_Unit = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_Unit") as Label;
                        var lbl_RecPrice = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_RecPrice") as Label;
                        var lbl_RecTaxType = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_RecTaxType") as Label;
                        var lbl_RecTaxRate = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_RecTaxRate") as Label;


                        lbl_Unit.Text= dtRec.Rows[0]["RcvUnit"].ToString();
                        lbl_RecPrice.Text = string.Format("{0:N3}", dtRec.Rows[0]["Price"]);
                        lbl_RecTaxType.Text = dtRec.Rows[0]["TaxType"].ToString();
                        lbl_RecTaxRate.Text = string.Format(DefaultAmtFmt, dtRec.Rows[0]["TaxRate"]);
                    }



                    //******************************* Stock Summary ******************************************************
                    if (dsCnEdit.Tables[prDt.TableName] != null)
                    {
                        dsCnEdit.Tables[prDt.TableName].Clear();
                    }
                    var cnDate = DateTime.ParseExact(txt_CnDate.Text, "dd/MM/yyyy", null);
                    var getStock = prDt.GetStockSummary(dsCnEdit, ddl_Product.ClientValue, ddl_Location.ClientValue,
                        cnDate.ToString("yyyy-MM-dd"), hf_ConnStr.Value);

                    if (getStock)
                    {
                        var drStockSummary = dsCnEdit.Tables[prDt.TableName].Rows[0];

                        if (grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_OnHand") != null)
                        {
                            var lbl_OnHand = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_OnHand") as Label;

                            if (drStockSummary["OnHand"].ToString() != string.Empty && drStockSummary["OnHand"] != null)
                            {
                                lbl_OnHand.Text = String.Format(DefaultQtyFmt, drStockSummary["OnHand"]);
                            }
                            else
                            {
                                lbl_OnHand.Text = "0";
                            }

                            lbl_OnHand.ToolTip = lbl_OnHand.Text;
                        }

                        if (grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_OnOrder") != null)
                        {
                            var lbl_OnOrder = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_OnOrder") as Label;

                            if (drStockSummary["OnOrder"].ToString() != string.Empty && drStockSummary["OnOrder"] != null)
                            {
                                lbl_OnOrder.Text = String.Format(DefaultQtyFmt, drStockSummary["OnOrder"]);
                            }
                            else
                            {
                                lbl_OnOrder.Text = "0.00";
                            }

                            lbl_OnOrder.ToolTip = lbl_OnOrder.Text;
                        }

                        if (grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_Reorder") != null)
                        {
                            var lbl_Reorder = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_Reorder") as Label;

                            if (drStockSummary["Reorder"].ToString() != string.Empty && drStockSummary["Reorder"] != null)
                            {
                                lbl_Reorder.Text = String.Format(DefaultQtyFmt, drStockSummary["Reorder"]);
                            }
                            else
                            {
                                lbl_Reorder.Text = "0.00";
                            }

                            lbl_Reorder.ToolTip = lbl_Reorder.Text;
                        }

                        if (grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_Restock") != null)
                        {
                            var lbl_Restock = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_Restock") as Label;

                            if (drStockSummary["Restock"].ToString() != string.Empty && drStockSummary["Restock"] != null)
                            {
                                lbl_Restock.Text = String.Format(DefaultQtyFmt, drStockSummary["Restock"]);
                            }
                            else
                            {
                                lbl_Restock.Text = "0.00";
                            }

                            lbl_Restock.ToolTip = lbl_Restock.Text;
                        }

                        if (grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_LastPrice") != null)
                        {
                            var lbl_LastPrice = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_LastPrice") as Label;

                            if (drStockSummary["LastPrice"].ToString() != string.Empty &&
                                drStockSummary["LastPrice"] != null)
                            {
                                lbl_LastPrice.Text = String.Format(DefaultAmtFmt, drStockSummary["LastPrice"]);
                            }
                            else
                            {
                                lbl_LastPrice.Text = "0.00";
                            }

                            lbl_LastPrice.ToolTip = lbl_LastPrice.Text;
                        }

                        if (grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_LastVendor") != null)
                        {
                            var lbl_LastVendor = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_LastVendor") as Label;
                            lbl_LastVendor.Text = drStockSummary["LastVendor"].ToString();
                            lbl_LastVendor.ToolTip = lbl_LastVendor.Text;
                        }
                    }
                }

            }

            ddl_Product.Value = ddl_Product.ClientValue;
            hf_ProductCode.Value = ddl_Product.ClientValue;

        }

        protected void ddl_CnType_SelectedIndexChanged(object sender, EventArgs e)
        {

            // CnType: Hide/Show Qty or Amount
            var ddl_CnType = sender as ASPxComboBox;

            var ddl_Unit = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Unit") as ASPxComboBox;
            var se_RecQty = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("se_RecQty") as ASPxSpinEdit;

            var se_CurrNetAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("se_CurrNetAmt") as ASPxSpinEdit;
            var se_CurrTaxAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("se_CurrTaxAmt") as ASPxSpinEdit;
            var se_CurrTotalAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("se_CurrTotalAmt") as ASPxSpinEdit;

            ddl_Unit.Enabled = ddl_CnType.SelectedIndex == CN_QTY_INDEX;
            se_RecQty.Enabled = ddl_CnType.SelectedIndex == CN_QTY_INDEX;
            ddl_Unit.Visible = ddl_CnType.SelectedIndex == CN_QTY_INDEX;
            se_RecQty.Visible = ddl_CnType.SelectedIndex == CN_QTY_INDEX;


            se_CurrNetAmt.Enabled = ddl_CnType.SelectedIndex == CN_AMT_INDEX;
            se_CurrTaxAmt.Enabled = ddl_CnType.SelectedIndex == CN_AMT_INDEX;
            se_CurrTotalAmt.Enabled = false;

            if (ddl_CnType.SelectedIndex == CN_QTY_INDEX)
            {
                se_RecQty.Focus();
            }
            else
            {
                se_CurrNetAmt.Focus();
                se_CurrTotalAmt.Value = 0;

            }

        }

        protected void se_RecQty_ValueChanged(object sender, EventArgs e)
        {
            var se_RecQty = sender as ASPxSpinEdit;
            var hf_ProductCode = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("hf_ProductCode") as HiddenField;
            var ddl_Location = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Location") as ASPxComboBox;
            var ddl_Product = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
            var ddl_Rec = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Rec") as ASPxComboBox;

            if (se_RecQty.Value.ToString() != null)
            {
                //var ds = new DataSet();
                //ds = recDt.GetListByRecNo(ds, ddl_Rec.Value.ToString(), hf_ConnStr.Value);
                //ds.Tables[0].DefaultView.RowFilter = "ProductCode ='" + ddl_Product.Value + "'";
                //var dt = ds.Tables[0].DefaultView.ToTable();

                //var recQty = Convert.ToDecimal(dt.Rows[0]["RecQty"].ToString());
                //if (decimal.Parse(se_RecQty.Text) > recQty)
                //{
                //    lbl_Warning.Text = "No approved quantity more over than Receive quantity";
                //    pop_Warning.ShowOnPageLoad = true;

                //    se_RecQty.Text = String.Format("{0:N}", recQty);
                //}

                CalculateDetail();
            }
        }

        protected void se_CurrNetAmt_NumberChanged(object sender, EventArgs e)
        {
            var se_CurrNetAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("se_CurrNetAmt") as ASPxSpinEdit;
            var se_CurrTaxAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("se_CurrTaxAmt") as ASPxSpinEdit;

            var lbl_RecTaxType = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_RecTaxType") as Label;
            var lbl_RecTaxRate = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_RecTaxRate") as Label;

            string taxType = lbl_RecTaxType.Text;

            if (taxType != "N")
            {
                decimal taxRate = Convert.ToDecimal(lbl_RecTaxRate.Text);
                decimal currNetAmt = Convert.ToDecimal(se_CurrNetAmt.Value);
                se_CurrTaxAmt.Value = RoundAmt(currNetAmt * taxRate / 100);
            }


            CalculateDetail();






        }

        protected void ddl_Unit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var ddl_Unit = sender as ASPxComboBox;
            //var ddl_Product = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Product") as ASPxComboBox;

            //if (ddl_Unit != null && ddl_Product != null)
            //{
            //    ddl_Unit.DataSource = prodUnit.GetLookUp_ProductCode(ddl_Product.Value.ToString(), hf_ConnStr.Value);
            //    ddl_Unit.ValueField = "OrderUnit";
            //    ddl_Unit.DataBind();
            //}
        }

        protected void grd_CnDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ddl_Vendor.Enabled = grd_CnDt.Rows.Count == 0;


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("hf_CnDtNo") != null)
                {
                    var hf = e.Row.FindControl("hf_CnDtNo") as HiddenField;
                    hf.Value = DataBinder.Eval(e.Row.DataItem, "CnDtNo").ToString();
                }


                if (e.Row.FindControl("lbl_Rec") != null)
                {
                    var lbl_Rec = e.Row.FindControl("lbl_Rec") as Label;
                    lbl_Rec.Text = DataBinder.Eval(e.Row.DataItem, "RecNo").ToString();
                    lbl_Rec.ToolTip = DataBinder.Eval(e.Row.DataItem, "RecNo").ToString();
                }

                if (e.Row.FindControl("ddl_Rec") != null)
                {
                    var ddl_Rec = e.Row.FindControl("ddl_Rec") as ASPxComboBox;
                    ddl_Rec.Value = DataBinder.Eval(e.Row.DataItem, "RecNo") == DBNull.Value
                        ? null
                        : DataBinder.Eval(e.Row.DataItem, "RecNo");
                    ddl_Rec.Enabled = (sender as GridView).Rows.Count == 0;
                }


                if (e.Row.FindControl("lbl_Location") != null)
                {
                    var lbl_Location = e.Row.FindControl("lbl_Location") as Label;
                    lbl_Location.Text = DataBinder.Eval(e.Row.DataItem, "Location") + " : " +
                                        locat.GetName(DataBinder.Eval(e.Row.DataItem, "Location").ToString(),
                                            hf_ConnStr.Value);
                    lbl_Location.ToolTip = DataBinder.Eval(e.Row.DataItem, "Location") + " : " +
                                           locat.GetName(DataBinder.Eval(e.Row.DataItem, "Location").ToString(),
                                               hf_ConnStr.Value);
                }

                if (e.Row.FindControl("ddl_Location") != null)
                {
                    var ddl_Location = e.Row.FindControl("ddl_Location") as ASPxComboBox;
                    var ddl_Rec = e.Row.FindControl("ddl_Rec") as ASPxComboBox;

                    var ds = new DataSet();
                    if (ddl_Rec.Value != null)
                    {
                        ddl_Location.Value = DataBinder.Eval(e.Row.DataItem, "Location") == DBNull.Value
                            ? null
                            : DataBinder.Eval(e.Row.DataItem, "Location") + " : " +
                              locat.GetName(DataBinder.Eval(e.Row.DataItem, "Location").ToString(), hf_ConnStr.Value);
                        ds = recDt.GetListByRecNo(ds, ddl_Rec.Value.ToString(), hf_ConnStr.Value);
                        ds.Tables[0].DefaultView.RowFilter = "RecNo ='" + ddl_Rec.Value + "'";
                        ddl_Location.DataSource = ds;
                        ddl_Location.ValueField = "LocationCode";
                        ddl_Location.DataBind();
                    }

                    if (ddl_Location.Value != null)
                    {
                        hf_Location.Value = ddl_Location.ClientValue;
                    }
                }

                if (e.Row.FindControl("lbl_Product") != null)
                {
                    var lbl_Product = e.Row.FindControl("lbl_Product") as Label;
                    lbl_Product.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode") + " : " +
                                       product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                           hf_ConnStr.Value) + " : " +
                                       product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                           hf_ConnStr.Value);
                    lbl_Product.ToolTip = lbl_Product.Text;
                }

                if (e.Row.FindControl("ddl_Product") != null)
                {
                    var ddl_CnType = e.Row.FindControl("ddl_CnType") as ASPxComboBox;
                    var ddl_Rec = e.Row.FindControl("ddl_Rec") as ASPxComboBox;
                    var ddl_Product = e.Row.FindControl("ddl_Product") as ASPxComboBox;

                    var ds = new DataSet();
                    if (ddl_Rec.Value != null)
                    {
                        ddl_Product.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode") == DBNull.Value
                            ? null
                            : DataBinder.Eval(e.Row.DataItem, "ProductCode") + " : " +
                              product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                  hf_ConnStr.Value);
                        if (ddl_CnType.Value.ToString().ToUpper() == "Q")
                            ds = recDt.GetListByRecNo(ds, ddl_Rec.Value.ToString(), hf_ConnStr.Value);
                        else
                            ds = getActiveProductByRecNo(ddl_Rec.Value.ToString());
                        ds.Tables[0].DefaultView.RowFilter = "RecNo ='" + ddl_Rec.Value + "'";
                        ddl_Product.DataSource = ds;
                        ddl_Product.ValueField = "ProductCode";
                        ddl_Product.DataBind();
                    }
                }

                if (e.Row.FindControl("lbl_ProductCode") != null)
                {
                    var lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                    lbl_ProductCode.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                    lbl_ProductCode.ToolTip = lbl_ProductCode.Text;
                }
                if (e.Row.FindControl("hf_ProductCode") != null)
                {
                    var hf_ProductCode = e.Row.FindControl("hf_ProductCode") as HiddenField;
                    hf_ProductCode.Value = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                }

                if (e.Row.FindControl("lbl_CnType") != null)
                {
                    var lbl_CnType = e.Row.FindControl("lbl_CnType") as Label;

                    string cnTypeValue = DataBinder.Eval(e.Row.DataItem, "CnType").ToString();

                    switch (cnTypeValue)
                    {
                        case "Q":
                            lbl_CnType.Text = "Quantity";
                            break;
                        case "A":
                            lbl_CnType.Text = "Amount";
                            break;
                        default:
                            lbl_CnType.Text = "";
                            break;
                    }

                    lbl_CnType.ToolTip = lbl_CnType.Text;
                }

                if (e.Row.FindControl("ddl_CnType") != null)
                {

                    var ddl_CnType = e.Row.FindControl("ddl_CnType") as ASPxComboBox;

                    if (DataBinder.Eval(e.Row.DataItem, "CnType").ToString().ToUpper() == "A")
                        ddl_CnType.SelectedIndex = CN_AMT_INDEX;
                    else
                        ddl_CnType.SelectedIndex = CN_QTY_INDEX;


                    //var ddl_Unit = e.Row.FindControl("ddl_Unit") as ASPxComboBox;
                    var se_RecQty = e.Row.FindControl("se_RecQty") as ASPxSpinEdit;
                    var se_CurrNetAmt = e.Row.FindControl("se_CurrNetAmt") as ASPxSpinEdit;
                    var se_CurrTaxAmt = e.Row.FindControl("se_CurrTaxAmt") as ASPxSpinEdit;
                    var se_CurrTotalAmt = e.Row.FindControl("se_CurrTotalAmt") as ASPxSpinEdit;

                    //ddl_Unit.Enabled = ddl_CnType.SelectedIndex == CN_QTY_INDEX;
                    se_RecQty.Enabled = ddl_CnType.SelectedIndex == CN_QTY_INDEX;

                    //ddl_Unit.Visible = ddl_CnType.SelectedIndex == CN_QTY_INDEX;
                    se_RecQty.Visible = ddl_CnType.SelectedIndex == CN_QTY_INDEX;

                    se_CurrNetAmt.Enabled = ddl_CnType.SelectedIndex == CN_AMT_INDEX;
                    se_CurrTaxAmt.Enabled = ddl_CnType.SelectedIndex == CN_AMT_INDEX;
                    se_CurrTotalAmt.Enabled = false;

                    if (ddl_CnType.SelectedIndex == 0)
                        se_RecQty.Focus();
                    else
                        se_CurrNetAmt.Focus();
                }


                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "UnitCode").ToString();
                    lbl_Unit.ToolTip = lbl_Unit.Text;

                    string cnTypeValue = DataBinder.Eval(e.Row.DataItem, "CnType").ToString();
                    lbl_Unit.Visible = cnTypeValue.ToUpper() != "A";

                }


                //if (e.Row.FindControl("ddl_Unit") != null)
                //{
                //    var ddl_Unit = e.Row.FindControl("ddl_Unit") as ASPxComboBox;
                //    ddl_Unit.DataSource = prodUnit.GetLookUp_ProductCode(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), hf_ConnStr.Value);
                //    ddl_Unit.ValueField = "OrderUnit";
                //    ddl_Unit.DataBind();
                //    ddl_Unit.Value = DataBinder.Eval(e.Row.DataItem, "UnitCode");
                //}

                if (e.Row.FindControl("lbl_RecQty") != null)
                {
                    var lbl_RecQty = e.Row.FindControl("lbl_RecQty") as Label;
                    lbl_RecQty.Text = String.Format(DefaultQtyFmt,
                        Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "RecQty") == DBNull.Value
                            ? 0.00
                            : DataBinder.Eval(e.Row.DataItem, "RecQty")));
                    lbl_RecQty.ToolTip = lbl_RecQty.Text;
                    grandQtyAmt += Convert.ToDecimal(lbl_RecQty.Text);

                    string cnTypeValue = DataBinder.Eval(e.Row.DataItem, "CnType").ToString();
                    lbl_RecQty.Visible = cnTypeValue.ToUpper() != "A";

                }

                if (e.Row.FindControl("se_RecQty") != null)
                {
                    var se_RecQty = e.Row.FindControl("se_RecQty") as ASPxSpinEdit;

                    if (qtyrecUpdate == 0)
                    {
                        se_RecQty.Text = String.Format(DefaultQtyFmt,
                            Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "RecQty") == DBNull.Value
                                ? 0.00
                                : DataBinder.Eval(e.Row.DataItem, "RecQty")));
                    }
                    else
                    {
                        se_RecQty.Text = String.Format(DefaultQtyFmt, qtyrecUpdate);
                    }
                }


                if (e.Row.FindControl("lbl_CurrNetAmt1") != null)
                {
                    var label = e.Row.FindControl("lbl_CurrNetAmt1") as Label;

                    label.Text = String.Format(DefaultAmtFmt,
                        Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CurrNetAmt") == DBNull.Value
                            ? 0.00
                            : DataBinder.Eval(e.Row.DataItem, "CurrNetAmt")));
                    label.ToolTip = label.Text;
                }




                if (e.Row.FindControl("se_CurrNetAmt") != null)
                {
                    var spinEdit = e.Row.FindControl("se_CurrNetAmt") as ASPxSpinEdit;

                    spinEdit.Text = String.Format(DefaultAmtFmt,
                        Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "NetAmt") == DBNull.Value
                            ? 0.00
                            : DataBinder.Eval(e.Row.DataItem, "NetAmt")));
                }

                if (e.Row.FindControl("lbl_CurrTaxAmt1") != null)
                {
                    var label = e.Row.FindControl("lbl_CurrTaxAmt1") as Label;

                    label.Text = String.Format(DefaultAmtFmt,
                        Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CurrTaxAmt") == DBNull.Value
                            ? 0.00
                            : DataBinder.Eval(e.Row.DataItem, "CurrTaxAmt")));
                    label.ToolTip = label.Text;
                }

                if (e.Row.FindControl("se_CurrTaxAmt") != null)
                {
                    var spinEdit = e.Row.FindControl("se_CurrTaxAmt") as ASPxSpinEdit;

                    spinEdit.Text = String.Format(DefaultAmtFmt,
                        Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TaxAmt") == DBNull.Value
                            ? 0.00
                            : DataBinder.Eval(e.Row.DataItem, "TaxAmt")));
                }


                if (e.Row.FindControl("lbl_CurrTotalAmt1") != null)
                {
                    var label = e.Row.FindControl("lbl_CurrTotalAmt1") as Label;

                    label.Text = String.Format(DefaultAmtFmt,
                        Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt") == DBNull.Value
                            ? 0.00
                            : DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt")));
                    label.ToolTip = label.Text;
                }

                if (e.Row.FindControl("se_CurrTotalAmt") != null)
                {
                    var spinEdit = e.Row.FindControl("se_CurrTotalAmt") as ASPxSpinEdit;

                    spinEdit.Text = String.Format(DefaultAmtFmt,
                        Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalAmt") == DBNull.Value
                            ? 0.00
                            : DataBinder.Eval(e.Row.DataItem, "TotalAmt")));
                }


                // -------------------------------------------------------------------------------------------------------
                // Information of Receiving

                string query = string.Empty;
                query = "SELECT TOP(1) CAST(rh.RecDate as DATE) RecDate, rh.[Description] as RecDesc";
                query += " FROM PC.REC rh";
                query += string.Format(" WHERE rh.RecNo = '{0}' ", DataBinder.Eval(e.Row.DataItem, "RecNo").ToString());
                DataTable dtRec = rec.DbExecuteQuery(query, null, hf_ConnStr.Value);
                if (dtRec.Rows.Count > 0)
                {
                    DateTime recDate = Convert.ToDateTime(dtRec.Rows[0]["RecDate"]);
                    string recDesc = dtRec.Rows[0]["RecDesc"].ToString();

                    if (e.Row.FindControl("lbl_RecDate") != null)
                    {
                        var lbl = e.Row.FindControl("lbl_RecDate") as Label;
                        lbl.Text = string.Format("{0:M/d/yyyy}", recDate);
                        lbl.ToolTip = string.Format("{0:dddd, MMMM d, yyyy}", recDate);
                    }
                    if (e.Row.FindControl("lbl_RecDesc") != null)
                    {
                        var lbl = e.Row.FindControl("lbl_RecDesc") as Label;
                        lbl.Text = recDesc;
                        lbl.ToolTip = lbl.Text;
                    }
                }



                if (e.Row.FindControl("lbl_RecPrice") != null)
                {
                    var label = e.Row.FindControl("lbl_RecPrice") as Label;

                    label.Text = String.Format(DefaultAmtFmt,
                        Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Price") == DBNull.Value
                            ? 0.00
                            : DataBinder.Eval(e.Row.DataItem, "Price")));
                    label.ToolTip = label.Text;
                }

                if (e.Row.FindControl("lbl_RecTaxType") != null)
                {
                    var label = e.Row.FindControl("lbl_RecTaxType") as Label;

                    label.Text = DataBinder.Eval(e.Row.DataItem, "TaxType").ToString();
                    label.ToolTip = label.Text;
                }

                if (e.Row.FindControl("lbl_RecTaxRate") != null)
                {
                    var label = e.Row.FindControl("lbl_RecTaxRate") as Label;

                    label.Text = String.Format("{0:N}",
                        Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TaxRate") == DBNull.Value
                            ? 0.00
                            : DataBinder.Eval(e.Row.DataItem, "TaxRate")));
                    label.ToolTip = label.Text;
                }

                if (e.Row.FindControl("lbl_Currency") != null)
                {
                    var lbl = e.Row.FindControl("lbl_Currency") as Label;
                    lbl.Text = ddl_Currency.Text;
                    lbl.ToolTip = lbl.Text;
                }

                if (e.Row.FindControl("lbl_CurrNetAmt") != null)
                {
                    var lbl = e.Row.FindControl("lbl_CurrNetAmt") as Label;
                    lbl.Text = string.Format(DefaultAmtFmt, string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrNetAmt")));
                    lbl.ToolTip = lbl.Text;
                }


                if (e.Row.FindControl("lbl_CurrTaxAmt") != null)
                {
                    var lbl = e.Row.FindControl("lbl_CurrTaxAmt") as Label;
                    lbl.Text = string.Format(DefaultAmtFmt, string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrTaxAmt")));
                    lbl.ToolTip = lbl.Text;
                }

                if (e.Row.FindControl("lbl_CurrTotalAmt") != null)
                {
                    var lbl = e.Row.FindControl("lbl_CurrTotalAmt") as Label;
                    lbl.Text = string.Format(DefaultAmtFmt, Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt")));
                    lbl.ToolTip = lbl.Text;

                }

                if (e.Row.FindControl("lbl_BaseCurrency") != null)
                {
                    var lbl = e.Row.FindControl("lbl_BaseCurrency") as Label;
                    lbl.Text = config.GetValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value);
                    lbl.ToolTip = lbl.Text;
                }


                if (e.Row.FindControl("lbl_NetAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_NetAmt") as Label;

                    label.Text = String.Format(DefaultAmtFmt,
                        Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "NetAmt") == DBNull.Value
                            ? 0.00
                            : DataBinder.Eval(e.Row.DataItem, "NetAmt")));
                    label.ToolTip = label.Text;
                }

                if (e.Row.FindControl("lbl_TaxAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_TaxAmt") as Label;

                    label.Text = String.Format(DefaultAmtFmt,
                        Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TaxAmt") == DBNull.Value
                            ? 0.00
                            : DataBinder.Eval(e.Row.DataItem, "TaxAmt")));
                    label.ToolTip = label.Text;
                }
                if (e.Row.FindControl("lbl_TotalAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_TotalAmt") as Label;

                    label.Text = String.Format(DefaultAmtFmt,
                        Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalAmt") == DBNull.Value
                            ? 0.00
                            : DataBinder.Eval(e.Row.DataItem, "TotalAmt")));
                    label.ToolTip = label.Text;
                }

                //if (e.Row.FindControl("lbl_RecTaxAmt") != null)
                //{
                //    var label = e.Row.FindControl("lbl_RecTaxAmt") as Label;

                //    label.Text = String.Format("{0:N}",
                //        Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TaxAmt") == DBNull.Value
                //            ? 0.00
                //            : DataBinder.Eval(e.Row.DataItem, "TaxAmt")));
                //    label.ToolTip = label.Text;
                //}

                //if (e.Row.FindControl("lbl_RecNetAmt") != null)
                //{
                //    var label = e.Row.FindControl("lbl_RecNetAmt") as Label;

                //    label.Text = String.Format("{0:N}",
                //        Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "NetAmt") == DBNull.Value
                //            ? 0.00
                //            : DataBinder.Eval(e.Row.DataItem, "NetAmt")));
                //    label.ToolTip = label.Text;
                //}


                ////******************************* Stock Summary ******************************************************
                if (dsCnEdit.Tables[prDt.TableName] != null)
                {
                    dsCnEdit.Tables[prDt.TableName].Clear();
                }

                var getStock = prDt.GetStockSummary(dsCnEdit, DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                    DataBinder.Eval(e.Row.DataItem, "Location").ToString(), "2013-06-27", hf_ConnStr.Value);

                if (getStock)
                {
                    var drStockSummary = dsCnEdit.Tables[prDt.TableName].Rows[0];

                    if (e.Row.FindControl("lbl_OnHand") != null)
                    {
                        var lbl_OnHand = e.Row.FindControl("lbl_OnHand") as Label;

                        if (drStockSummary["OnHand"].ToString() != string.Empty && drStockSummary["OnHand"] != null)
                        {
                            lbl_OnHand.Text = String.Format(DefaultQtyFmt, drStockSummary["OnHand"]);
                        }
                        else
                        {
                            lbl_OnHand.Text = "0";
                        }

                        lbl_OnHand.ToolTip = lbl_OnHand.Text;
                    }
                }
                //    if (e.Row.FindControl("lbl_OnOrder") != null)
                //    {
                //        var lbl_OnOrder = e.Row.FindControl("lbl_OnOrder") as Label;

                //        if (drStockSummary["OnOrder"].ToString() != string.Empty && drStockSummary["OnOrder"] != null)
                //        {
                //            lbl_OnOrder.Text = String.Format("{0:N}", drStockSummary["OnOrder"]);
                //        }
                //        else
                //        {
                //            lbl_OnOrder.Text = "0.00";
                //        }

                //        lbl_OnOrder.ToolTip = lbl_OnOrder.Text;
                //    }

                //    if (e.Row.FindControl("lbl_Reorder") != null)
                //    {
                //        var lbl_Reorder = e.Row.FindControl("lbl_Reorder") as Label;

                //        if (drStockSummary["Reorder"].ToString() != string.Empty && drStockSummary["Reorder"] != null)
                //        {
                //            lbl_Reorder.Text = String.Format("{0:N}", drStockSummary["Reorder"]);
                //        }
                //        else
                //        {
                //            lbl_Reorder.Text = "0.00";
                //        }

                //        lbl_Reorder.ToolTip = lbl_Reorder.Text;
                //    }

                //    if (e.Row.FindControl("lbl_Restock") != null)
                //    {
                //        var lbl_Restock = e.Row.FindControl("lbl_Restock") as Label;

                //        if (drStockSummary["Restock"].ToString() != string.Empty && drStockSummary["Restock"] != null)
                //        {
                //            lbl_Restock.Text = String.Format("{0:N}", drStockSummary["Restock"]);
                //        }
                //        else
                //        {
                //            lbl_Restock.Text = "0.00";
                //        }

                //        lbl_Restock.ToolTip = lbl_Restock.Text;
                //    }

                //    if (e.Row.FindControl("lbl_LastPrice") != null)
                //    {
                //        var lbl_LastPrice = e.Row.FindControl("lbl_LastPrice") as Label;

                //        if (drStockSummary["LastPrice"].ToString() != string.Empty &&
                //            drStockSummary["LastPrice"] != null)
                //        {
                //            lbl_LastPrice.Text = String.Format("{0:N}", drStockSummary["LastPrice"]);
                //        }
                //        else
                //        {
                //            lbl_LastPrice.Text = "0.00";
                //        }

                //        lbl_LastPrice.ToolTip = lbl_LastPrice.Text;
                //    }

                //    if (e.Row.FindControl("lbl_LastVendor") != null)
                //    {
                //        var lbl_LastVendor = e.Row.FindControl("lbl_LastVendor") as Label;
                //        lbl_LastVendor.Text = drStockSummary["LastVendor"].ToString();
                //        lbl_LastVendor.ToolTip = lbl_LastVendor.Text;
                //    }
                //}

                ////******************************* Stock Summary ******************************************************

                grandCurrNetAmt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CurrNetAmt"));
                grandCurrTaxAmt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CurrTaxAmt"));
                grandCurrTotalAmt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt"));

                grandNetAmt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "NetAmt"));
                grandTaxAmt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TaxAmt"));
                grandTotalAmt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalAmt"));

            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (e.Row.FindControl("lbl_Currency_Nm") != null)
                {
                    var lbl = e.Row.FindControl("lbl_Currency_Nm") as Label;
                    lbl.Text = ddl_Currency.Text;
                }


                if (e.Row.FindControl("lbl_Base_Nm") != null)
                {
                    var lbl = e.Row.FindControl("lbl_Base_Nm") as Label;
                    lbl.Text = config.GetValue("APP", "BU", "DefaultCurrency", LoginInfo.ConnStr);
                }

                if (e.Row.FindControl("lbl_CurrNetAmt") != null)
                {
                    var lbl = e.Row.FindControl("lbl_CurrNetAmt") as Label;
                    lbl.Text = String.Format(DefaultAmtFmt, grandCurrNetAmt);
                }

                if (e.Row.FindControl("lbl_CurrTaxAmt") != null)
                {
                    var lbl = e.Row.FindControl("lbl_CurrTaxAmt") as Label;
                    lbl.Text = String.Format(DefaultAmtFmt, grandCurrTaxAmt);
                }

                if (e.Row.FindControl("lbl_CurrTotalAmt") != null)
                {
                    var lbl = e.Row.FindControl("lbl_CurrTotalAmt") as Label;
                    lbl.Text = String.Format(DefaultAmtFmt, grandCurrTotalAmt);
                }


                if (e.Row.FindControl("lbl_NetAmt") != null)
                {
                    var lbl = e.Row.FindControl("lbl_NetAmt") as Label;
                    lbl.Text = String.Format(DefaultAmtFmt, grandNetAmt);
                }

                if (e.Row.FindControl("lbl_TaxAmt") != null)
                {
                    var lbl = e.Row.FindControl("lbl_TaxAmt") as Label;
                    lbl.Text = String.Format(DefaultAmtFmt, grandTaxAmt);
                }

                if (e.Row.FindControl("lbl_TotalAmt") != null)
                {
                    var lbl = e.Row.FindControl("lbl_TotalAmt") as Label;
                    lbl.Text = String.Format(DefaultAmtFmt, grandTotalAmt);
                }

            }

        }

        protected void grd_CnDt_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper() == "SAVENEW" || e.CommandName.ToUpper() == "SAVE")
            {
                CalculateDetail();


                // Header
                var lbl_Status = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_Status") as Label;

                // Detail
                var ddl_Rec = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Rec") as ASPxComboBox;
                var ddl_Location = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Location") as ASPxComboBox;
                var ddl_Product = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
                var ddl_CnType = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_CnType") as ASPxComboBox;
                var se_RecQty = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("se_RecQty") as ASPxSpinEdit;
                //var ddl_Unit = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("ddl_Unit") as ASPxComboBox;
                var lbl_Unit = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_Unit") as Label;

                //var se_CurrNetAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("se_CurrNetAmt") as ASPxSpinEdit;
                //var se_CurrTaxAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("se_CurrTaxAmt") as ASPxSpinEdit;
                //var se_CurrTotalAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("se_CurrTotalAmt") as ASPxSpinEdit;

                var lbl_CurrNetAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_CurrNetAmt") as Label;
                var lbl_CurrTaxAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_CurrTaxAmt") as Label;
                var lbl_CurrTotalAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_CurrTotalAmt") as Label;

                var lbl_NetAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_NetAmt") as Label;
                var lbl_TaxAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_TaxAmt") as Label;
                var lbl_TotalAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_TotalAmt") as Label;

                var lbl_RecPrice = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_RecPrice") as Label;
                var lbl_RecTaxType = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_RecTaxType") as Label;
                var lbl_RecTaxRate = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_RecTaxRate") as Label;
                //var lbl_RecNetAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_RecNetAmt") as Label;
                //var lbl_RecTaxAmt = grd_CnDt.Rows[grd_CnDt.EditIndex].FindControl("lbl_RecTaxAmt") as Label;

                string CnTypeValue = ddl_CnType.Value.ToString().ToUpper();

                if (ddl_Location.Value == null || ddl_Product.Value == null)
                {
                    lbl_Warning.Text = "Store/ Location is required.";
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }


                double cnQty = Convert.ToDouble(se_RecQty.Value.ToString());
                //double cnAmt = Convert.ToDouble(se_CurrTotalAmt.Value.ToString());
                double cnAmt = Convert.ToDouble(lbl_CurrTotalAmt.Text);

                string recNo = ddl_Rec.Value.ToString();
                string locationCode = ddl_Location.Value.ToString().Split(':')[0].Trim();
                string productCode = ddl_Product.Value.ToString().Split(':')[0].Trim();

                if (CnTypeValue == "Q")
                {
                    double recQty = GetProductRemainQty(recNo, locationCode, productCode);
                    if (cnQty > recQty)
                    {
                        lbl_Warning.Text = string.Format("The product quantity is exceed than received. ({0:N3}).", recQty);
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }
                }
                else
                {
                    double recAmt = GetProductRemainAmount(recNo, locationCode, productCode);
                    if (cnAmt > recAmt)
                    {
                        lbl_Warning.Text = string.Format("The product amount is exceed than received. ({0:N3}).", recAmt);
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }
                }

                var drCnDtEdited = dsCnEdit.Tables[cnDt.TableName].Rows[grd_CnDt.Rows[grd_CnDt.EditIndex].DataItemIndex];

                drCnDtEdited["RecNo"] = ddl_Rec.Value.ToString().Split(':')[0].Trim();
                drCnDtEdited["Location"] = ddl_Location.Value.ToString().Split(':')[0].Trim();
                drCnDtEdited["ProductCode"] = ddl_Product.Value.ToString().Split(':')[0].Trim();
                drCnDtEdited["UnitCode"] = lbl_Unit.Text;
                //if (string.IsNullOrEmpty(ddl_Unit.Text))
                //{
                //    string sql = string.Format("SELECT TOP(1) RcvUnit FROM PC.RECDt WHERE RecNo = '{0}' AND ProductCode = '{1}' ", ddl_Rec.Text, ddl_Product.Text.Split(' ')[0].Trim());
                //    DataTable dt = rec.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

                //    drCnDtEdited["UnitCode"] = dt.Rows.Count == 0 ? "" : dt.Rows[0]["RcvUnit"].ToString();
                //}
                //else
                //    drCnDtEdited["UnitCode"] = ddl_Unit.Value.ToString().Split(':')[0].Trim();
                drCnDtEdited["RecQty"] = se_RecQty.Value;
                drCnDtEdited["FOCQty"] = 0;
                drCnDtEdited["Price"] = Convert.ToDecimal(lbl_RecPrice.Text);
                drCnDtEdited["TaxType"] = lbl_RecTaxType.Text;
                drCnDtEdited["TaxRate"] = Convert.ToDecimal(lbl_RecTaxRate.Text);
                drCnDtEdited["TaxAdj"] = 0;
                //drCnDtEdited["NetAmt"] = Convert.ToDecimal(lbl_RecNetAmt.Text);
                //drCnDtEdited["TaxAmt"] = Convert.ToDecimal(lbl_RecTaxAmt.Text);
                //drCnDtEdited["TotalAmt"] = Convert.ToDecimal(lbl_RecNetAmt.Text) + Convert.ToDecimal(lbl_RecTaxAmt.Text);
                drCnDtEdited["CurrNetAmt"] = Convert.ToDecimal(lbl_CurrNetAmt.Text);
                drCnDtEdited["CurrTaxAmt"] = Convert.ToDecimal(lbl_CurrTaxAmt.Text);
                drCnDtEdited["CurrTotalAmt"] = Convert.ToDecimal(lbl_CurrTotalAmt.Text);

                drCnDtEdited["NetAmt"] = Convert.ToDecimal(lbl_NetAmt.Text);
                drCnDtEdited["TaxAmt"] = Convert.ToDecimal(lbl_TaxAmt.Text);
                drCnDtEdited["TotalAmt"] = Convert.ToDecimal(lbl_TotalAmt.Text);

                //drCnDtEdited["NetCrAcc"] = txt_NetCrAcc.Text == string.Empty ? string.Empty : txt_NetCrAcc.Text;
                //drCnDtEdited["TaxCrAcc"] = txt_TaxCrAcc.Text == string.Empty ? string.Empty : txt_TaxCrAcc.Text;
                //drCnDtEdited["Comment"] = txt_Comment.Text;
                drCnDtEdited["CnType"] = CnTypeValue;

                if (CnTypeValue == "A")
                {
                    drCnDtEdited["RecQty"] = 0;
                }

                //dsCnEdit.Tables[cnDt.TableName].AcceptChanges();

                // Comment by op
                grd_CnDt.DataSource = dsCnEdit.Tables[cnDt.TableName];
                grd_CnDt.EditIndex = -1;
                grd_CnDt.DataBind();


                CnEditMode = string.Empty;

                SetVisibleButtons(true);


                //dsCnEdit.Tables[inv.TableName].AcceptChanges();
                if (e.CommandName.ToUpper() == "SAVENEW")
                    Create();

            }

        }

        protected void grd_CnDt_RowEditing(object sender, GridViewEditEventArgs e)
        {
            SetVisibleButtons(false);

            grd_CnDt.DataSource = dsCnEdit.Tables[cnDt.TableName];
            grd_CnDt.EditIndex = e.NewEditIndex;
            grd_CnDt.DataBind();

            CnEditMode = "EDIT";
        }

        protected void grd_CnDt_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                if (CnEditMode.ToUpper() == "NEW")
                {
                    dsCnEdit.Tables[cnDt.TableName].Rows[dsCnEdit.Tables[cnDt.TableName].Rows.Count - 1].Delete();
                }
                if (CnEditMode.ToUpper() == "EDIT")
                {
                    dsCnEdit.Tables[cnDt.TableName].Rows[dsCnEdit.Tables[cnDt.TableName].Rows.Count - 1].CancelEdit();
                }
            }

            if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                if (CnEditMode.ToUpper() == "NEW")
                {
                    dsCnEdit.Tables[cnDt.TableName].Rows[dsCnEdit.Tables[cnDt.TableName].Rows.Count - 1].Delete();
                }
                if (CnEditMode.ToUpper() == "EDIT")
                {
                    dsCnEdit.Tables[cnDt.TableName].Rows[dsCnEdit.Tables[cnDt.TableName].Rows.Count - 1].CancelEdit();
                }
            }

            grd_CnDt.DataSource = dsCnEdit.Tables[cnDt.TableName];
            grd_CnDt.EditIndex = -1;
            grd_CnDt.DataBind();

            //Enable button when cancel edit.

            //var btn_Save = UpdatePanelHead.FindControl("btn_Save") as ASPxButton;
            //var btn_Commit = UpdatePanelHead.FindControl("btn_Commit") as ASPxButton;
            //var btn_Back = UpdatePanelHead.FindControl("btn_Back") as ASPxButton;

            //btn_Save.Visible = true;
            //btn_Commit.Visible = true;
            //btn_Back.Visible = true;
            //btn_Create.Visible = true;
            //btn_Delete.Visible = true;

            //if (grd_CnDt.Rows.Count == 0)
            //{
            //    txt_CnDate.Enabled = true;
            //    txt_DocDate.Enabled = true;
            //    ddl_Vendor.Enabled = true;
            //}
            //CnEditMode = string.Empty;

            SetVisibleButtons(true);
        }

        protected void grd_CnDt_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            dsCnEdit.Tables[cnDt.TableName].Rows[e.RowIndex].Delete();
            dsCnEdit.Tables[cnDt.TableName].AcceptChanges();

            grd_CnDt.DataSource = dsCnEdit.Tables[cnDt.TableName];
            grd_CnDt.EditIndex = -1;
            grd_CnDt.DataBind();

            Session["dsCnEdit"] = dsCnEdit;

            e.Cancel = true;
        }

        protected void ddl_Currency_Init(object sender, EventArgs e)
        {
            ddl_Currency.DataSource = currency.GetLastCurrencyRate(LoginInfo.ConnStr);
            ddl_Currency.DataBind();
            ddl_Currency.Value = config.GetConfigValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value);
        }

        protected void ddl_Currency_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            if (txt_DocDate.Text != string.Empty)
                date = DateTime.Parse(txt_DocDate.Text);
            txt_ExRateAu.Text = currency.GetLastCurrencyRate(ddl_Currency.Value.ToString(), date, LoginInfo.ConnStr).ToString();
        }


        protected void txt_ExRateAu_TextChange(object sender, EventArgs e)
        {
            var dt = dsCnEdit.Tables[cnDt.TableName];

            foreach (DataRow dr in dt.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    decimal currRate = Convert.ToDecimal(txt_ExRateAu.Text);
                    decimal currNetAmt = Convert.ToDecimal(dr["CurrNetAmt"]);
                    decimal currTaxAmt = Convert.ToDecimal(dr["CurrTaxAmt"]);
                    decimal currTotalAmt = Convert.ToDecimal(dr["CurrTotalAmt"]);

                    dr["NetAmt"] = RoundAmt(currNetAmt * currRate);
                    dr["TaxAmt"] = RoundAmt(currNetAmt * currRate);
                    if (Convert.ToDecimal(dr["NetAmt"]) == 0)
                        dr["TotalAmt"] = RoundAmt(currTotalAmt * currRate);
                    else
                        dr["TotalAmt"] = Convert.ToDecimal(dr["NetAmt"]) + Convert.ToDecimal(dr["TaxAmt"]);
                }
            }

            Session["dsCnEdit"] = dsCnEdit;
            grd_CnDt.DataSource = dsCnEdit.Tables[cnDt.TableName];
            grd_CnDt.DataBind();

        }
    }
}