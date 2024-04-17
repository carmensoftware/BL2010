using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;


namespace BlueLedger.PL.IN.REC
{
    public partial class RECEdit : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Admin.Interface.AccountMapp accountMapp = new Blue.BL.Option.Admin.Interface.AccountMapp();
        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();

        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();

        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private readonly Blue.BL.Option.Inventory.DeliveryPoint deliPoint = new Blue.BL.Option.Inventory.DeliveryPoint();
        private readonly DataSet dsInventory = new DataSet();
        private readonly DataSet dsPoUpdate = new DataSet();
        private readonly DataSet dsRecEditCount = new DataSet();

        private readonly Blue.BL.Ref.ExRate exRate = new Blue.BL.Ref.ExRate();
        private readonly Blue.BL.IN.Inventory inv = new Blue.BL.IN.Inventory();
        private readonly Blue.BL.Option.Inventory.StoreLct locat = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();

        private readonly Blue.BL.PC.PO.PO po = new Blue.BL.PC.PO.PO();
        private readonly Blue.BL.PC.PO.PoDt poDt = new Blue.BL.PC.PO.PoDt();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.PC.REC.REC rec = new Blue.BL.PC.REC.REC();
        private readonly Blue.BL.PC.REC.RECDt recDt = new Blue.BL.PC.REC.RECDt();
        private readonly Blue.BL.Option.Inventory.Unit unit = new Blue.BL.Option.Inventory.Unit();
        private readonly Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();

        private readonly BlueLedger.PL.IN.REC.RecFunc recFunc = new BlueLedger.PL.IN.REC.RecFunc();
        private readonly Blue.BL.Ref.Currency currency = new Blue.BL.Ref.Currency();

        private string MsgError = string.Empty;
        private string PoNo = string.Empty;
        private int _recDtNo;
        private decimal _totalAmt;
        private List<string> delValues = new List<string>();
        private decimal discountUpdateAmt;
        private DataSet dsPOList = new DataSet();
        private DataSet dsRecEdit = new DataSet();
        private DataSet dsRecOld = new DataSet();
        private DataSet dsSave = new DataSet();
        //private DataSet dsUpdatePO = new DataSet();
        private DataSet dsImport = new DataSet();
        private Blue.BL.GnxLib gnxLib = new Blue.BL.GnxLib();

        //private decimal grandRecAmt;
        private decimal grandNetAmt;
        private decimal grandTaxAmt;
        private decimal grandTotal;
        // Added on: 25/08/2017, By: Fon
        private decimal currGrandNetAmt;
        private decimal currGrandTaxAmt;
        private decimal currGrandTotal;
        //private string poCurrency;
        // End Added.


        //private int intRecDtNo;
        const string statusPartial = "Partial";
        const string statusPrint = "Printed";
        const string recExtCost = "RecExtCost";

        private decimal priceUpdate;
        private decimal qtyrecUpdate;
        private decimal recQty = 0;
        private string status = string.Empty;

        //private decimal taxUpdate;
        //private decimal totalAmountUpdate;

        /// <summary>
        ///     Max RecDtNo
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

        private int lastExtDtNo
        {
            get
            {
                if (Session["lastExtDtNo"] == null)
                    return 0;
                else
                    return (int)Session["lastExtDtNo"];
            }
            set { Session["lastExtDtNo"] = value; }

        }

        /// <summary>
        ///     Get audit currency code.
        /// </summary>
        private string AuditCurrencyCode
        {
            get { return config.GetConfigValue("APP", "BU", "AuditCurrencyCode", hf_ConnStr.Value); }
        }

        // Added on: 25/08/2017, By: Fon
        private string baseCurrency
        {
            get { return config.GetValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value); }
        }
        // End Added.

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
            get { return config.GetConfigValue("PC", "REC", "ExRateFMT", hf_ConnStr.Value); }
        }

        private string RecEditMode
        {
            get { return ViewState["RecEditMode"].ToString(); }
            set { ViewState["RecEditMode"] = value; }
        }

        private string MODE
        {
            get { return Request.QueryString["MODE"]; }
        }

        private DataTable dtRecExtCost
        {
            get { return (DataTable)Session["dtRecExtCost"]; }
            set { Session["dtRecExtCost"] = value; }
        }


        #endregion

        #region "Operation"

        protected void Page_Init(object sender, EventArgs e)
        {
            //if not have PO Number will be redirect to Rec Manual
            if ((Request.Params["Po"] == null || string.IsNullOrEmpty(Request.Params["Po"]))
                && Request.Params["MODE"].ToUpper() != "FPO")
            {
                var url = new StringBuilder().AppendFormat(
                    "RecCreateManual.aspx?MODE={0}&ID={1}&BuCode={2}&VID={3}&Po=",
                    Request.Params["MODE"], Request.Params["ID"], Request.Params["BuCode"], Request.Params["VID"]);
                Response.Redirect(url.ToString());
            }

            hf_ConnStr.Value = bu.GetConnectionString(Request.Params["BuCode"]);
            // Note By: Fon, Becareful about this variable. 
            //          Sometime, they use from [...Source] - that contain wrong value.
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            // Check login
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
                Page_Setting();
            }
            else
            {
                dsRecEdit = (DataSet)Session["dsRecEdit"];
                dsPOList = (DataSet)Session["dsPOList"];
                dsSave = (DataSet)Session["dsSave"];
                dsImport = (DataSet)Session["dsImport"];

                if (ViewState["delValues"] != null)
                {
                    delValues = (List<string>)ViewState["delValues"];
                }
            }
        }

        /// <summary>
        /// </summary>
        private void Page_Retrieve()
        {
            if ((MODE.ToUpper() == "NEW") && (Request.QueryString["Status"] != null))
            {
                dsRecEdit = (DataSet)Session["dsPo"];
                rec.GetStructure(dsRecEdit, hf_ConnStr.Value);

                var drNew = dsRecEdit.Tables[rec.TableName].NewRow();

                drNew["RecNo"] = rec.GetNewID(ServerDateTime, hf_ConnStr.Value);
                drNew["RecDate"] = ServerDateTime;
                drNew["CreatedDate"] = ServerDateTime;
                drNew["CreatedBy"] = LoginInfo.LoginName;
                drNew["UpdatedDate"] = ServerDateTime;
                drNew["UpdatedBy"] = LoginInfo.LoginName;
                drNew["ExRateAudit"] = dsRecEdit.Tables[po.TableName].Rows[0]["ExchageRate"];
                // Modified on: 15/08/2017, By: Fon, For: New Muti-currency.
                drNew["CurrencyCode"] = dsRecEdit.Tables[po.TableName].Rows[0]["CurrencyCode"];
                drNew["CurrencyRate"] = dsRecEdit.Tables[po.TableName].Rows[0]["CurrencyRate"];
                drNew["ExtraCostBy"] = "Q";

                // Add new row
                dsRecEdit.Tables[rec.TableName].Rows.Add(drNew);

                // Get Schema.
                recDt.GetStructure(dsRecEdit, hf_ConnStr.Value);
                inv.GetStructure(dsSave, hf_ConnStr.Value);


                //// Create Schema for ExtraCost
                //DataTable dt = new DataTable();
                //dt = rec.DbExecuteQuery("SELECT TOP(0) * FROM PC.RecExtCost", null, hf_ConnStr.Value);
                //dt.TableName = recExtCost;
                //dsRecEdit.Tables.Add(dt);
                //lastExtDtNo = 0;

                Session["dsRecDt"] = dsRecEdit.Tables[recDt.TableName];

                // Get Data for lookup
                GetLookup();
                GetImportAccCode();
            }
            else if (MODE.ToUpper() == "EDIT")
            {
                var MsgError = string.Empty;

                // Get invoice no from HTTP query string
                var recNo = Request.QueryString["ID"];

                rec.GetListByRecNo(dsSave, ref MsgError, recNo, hf_ConnStr.Value);
                rec.GetListByRecNo(dsRecEdit, ref MsgError, recNo, hf_ConnStr.Value);
                recDt.GetListByRecNo(dsSave, ref MsgError, recNo, hf_ConnStr.Value);
                recDt.GetListByRecNo(dsRecEdit, recNo, hf_ConnStr.Value);
                inv.GetListByHdrNo(dsSave, recNo, hf_ConnStr.Value);


                //// Create Schema for ExtraCost
                //DataTable dt = new DataTable();
                //dt = rec.DbExecuteQuery(string.Format("SELECT * FROM PC.RecExtCost WHERE RecNo = '{0}'", recNo), null, hf_ConnStr.Value);
                //dt.TableName = recExtCost;
                //dsRecEdit.Tables.Add(dt);

                //if (dt.Rows.Count > 0)
                //    lastExtDtNo = (int)dt.Rows[dt.Rows.Count - 1]["DtNo"];
                //else
                //    lastExtDtNo = 0;


                Session["dsRecDt"] = dsRecEdit.Tables[recDt.TableName];

                GetLookup();
                GetImportAccCode();

            }
            else if (MODE.ToUpper() == "FPO")
            {
                dsRecEdit = (DataSet)Session["dsPo"];

                inv.GetStructure(dsSave, hf_ConnStr.Value);

                //// Create Schema for ExtraCost
                //DataTable dt = new DataTable();
                //dt = rec.DbExecuteQuery("SELECT TOP(0) * FROM PC.RecExtCost", null, hf_ConnStr.Value);
                //dt.TableName = recExtCost;
                //dsRecEdit.Tables.Add(dt);
                //lastExtDtNo = 0;

                GetLookup();
                GetImportAccCode();
            }
            else if (MODE.ToUpper() == "CANCELITEM")
            {
                dsRecEdit = (DataSet)Session["dsPo"];
                GetLookup();
                GetImportAccCode();
            }
            else
            {
                var result = rec.GetStructure(dsRecEdit, hf_ConnStr.Value);

                if (result)
                {
                    var drNew = dsRecEdit.Tables[rec.TableName].NewRow();

                    drNew["RecNo"] = rec.GetNewID(ServerDateTime, hf_ConnStr.Value);
                    drNew["RecDate"] = ServerDateTime;
                    drNew["CreatedDate"] = ServerDateTime;
                    drNew["CreatedBy"] = LoginInfo.LoginName;
                    drNew["UpdatedDate"] = ServerDateTime;
                    drNew["UpdatedBy"] = LoginInfo.LoginName;

                    // Add new row
                    dsRecEdit.Tables[rec.TableName].Rows.Add(drNew);

                    // Get Schema.
                    recDt.GetStructure(dsRecEdit, hf_ConnStr.Value);
                    inv.GetStructure(dsSave, hf_ConnStr.Value);

                    //// Create Schema for ExtraCost
                    //DataTable dt = new DataTable();
                    //dt = rec.DbExecuteQuery("SELECT TOP(0) * FROM PC.RecExtCost", null, hf_ConnStr.Value);
                    //dt.TableName = recExtCost;
                    //dsRecEdit.Tables.Add(dt);
                    //lastExtDtNo = 0;

                    // Get Data for lookup
                    GetLookup();
                    GetImportAccCode();
                }
            }

            // Condition for MaxrecdtNo.
            if (MODE.ToUpper() == "NEW")
            {
                RecDtNo = 0;
            }
            else
            {
                if (Request.QueryString["ID"] != null)
                {
                    // Get recdt maxId and keep in the viewstate.
                    recDt.GetRecDtMaxIDByRecNo(Request.QueryString["ID"], dsRecEditCount, hf_ConnStr.Value);

                    if (dsRecEditCount.Tables[recDt.TableName].Rows.Count > 0)
                    {
                        RecDtNo = Convert.ToInt32(dsRecEditCount.Tables[recDt.TableName].Rows[0]["RecDtNo"].ToString());
                    }
                }
            }

            dsPOList.Clear();

            // Get Podtlist by status
            var getPOList = poDt.GetListByStatus(dsPOList, statusPrint, statusPartial, hf_ConnStr.Value);

            if (getPOList)
            {
                // Assign Primarykey                
                dsPOList.Tables[poDt.TableName].PrimaryKey = GetPK();

                Session["dsPOList"] = dsPOList;
            }


            // Create Schema for ExtraCost
            string id = MODE.ToUpper() == "EDIT" ? Request.QueryString["ID"].ToString() : string.Empty;
            DataTable dt = new DataTable();
            dt = rec.DbExecuteQuery(string.Format("SELECT * FROM PC.RecExtCost WHERE RecNo = '{0}'", id), null, hf_ConnStr.Value);
            dt.TableName = recExtCost;
            dsRecEdit.Tables.Add(dt);

            if (dt.Rows.Count > 0)
                lastExtDtNo = (int)dt.Rows[dt.Rows.Count - 1]["DtNo"];
            else
                lastExtDtNo = 0;


            Session["dsRecEdit"] = dsRecEdit;
            Session["dsSave"] = dsSave;
            Session["dsImport"] = dsImport;
            delValues.Clear();
        }

        /// <summary>
        /// </summary>
        private void Page_Setting()
        {
            btn_AddPo.Visible = true;

            rdb_ExtraCostByQty.Checked = true;

            if (MODE.ToUpper() == "EDIT")
            {
                var lbl_Product = grd_RecEdit.FindControl("lbl_ProductCode") as ASPxLabel;

                var drRecEdit = dsRecEdit.Tables[rec.TableName].Rows[0];

                txt_RecNo.Enabled = false;
                txt_RecNo.Text = drRecEdit["RecNo"].ToString();
                de_RecDate.Date = DateTime.Parse(drRecEdit["RecDate"].ToString());
                de_RecDate.Enabled = false;
                lbl_DocStatus.Text = drRecEdit["DocStatus"].ToString();

                // Modified: 15/08/2019, By: Fon, For: New Muti-currency
                /*//lbl_Currency.Text = (drRecEdit["CurrencyCode"].ToString() == string.Empty
                //    ? config.GetValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value)
                //    : drRecEdit["CurrencyCode"].ToString());
                //txt_ExRateAu.Text = (drRecEdit["ExRateAudit"].ToString() == string.Empty
                //    ? "0.0000"
                //    : drRecEdit["ExRateAudit"].ToString());*/
                ddl_Currency.Value = (drRecEdit["CurrencyCode"].ToString() == string.Empty
                    ? config.GetValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value)
                    : drRecEdit["CurrencyCode"].ToString());
                txt_ExRateAu.Text = (drRecEdit["CurrencyRate"].ToString() == string.Empty)
                    ? string.Format("{0:N6}", "0")
                    : drRecEdit["CurrencyRate"].ToString();

                // Modified on: 15/11/2017
                lbl_CommitDate.Text = (drRecEdit["CommitDate"] != DBNull.Value)
                    ? Convert.ToDateTime(drRecEdit["CommitDate"]).ToString("dd/MM/yyyy") : string.Empty;
                // End Modified.
                lbl_Receiver.Text = drRecEdit["CreatedBy"].ToString();
                lbl_VendorCode.Text = drRecEdit["VendorCode"].ToString();
                lbl_VendorNm.Text = " : " + vendor.GetName(drRecEdit["VendorCode"].ToString(), hf_ConnStr.Value);

                txt_Desc.Text = drRecEdit["Description"].ToString();
                txt_InvNo.Text = drRecEdit["InvoiceNo"].ToString();
                de_InvDate.Date = (drRecEdit["InvoiceDate"] == System.DBNull.Value
                    ? DateTime.MinValue
                    : DateTime.Parse(drRecEdit["InvoiceDate"].ToString()));
                chk_CashConsign.Checked = bool.Parse(drRecEdit["IsCashConsign"].ToString());
                cmb_DeliPoint.Value = drRecEdit["DeliPoint"];

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
            else if (MODE.ToUpper() == "FPO")
            {
                var drRecEdit = dsRecEdit.Tables[rec.TableName].Rows[0];
                txt_RecNo.Enabled = false;
                txt_RecNo.Text = string.Empty;
                de_RecDate.Date = ServerDateTime;

                // Modified: 15/08/2019, By: Fon, For: New Muti-currency
                //txt_ExRateAu.Text = (drRecEdit["ExRateAudit"].ToString() == string.Empty
                //    ? "0.0000"
                //    : drRecEdit["ExRateAudit"].ToString());
                ddl_Currency.Value = (drRecEdit["CurrencyCode"].ToString() == string.Empty
                    ? config.GetValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value)
                    : drRecEdit["CurrencyCode"].ToString());
                txt_ExRateAu.Text = (drRecEdit["CurrencyRate"].ToString() == string.Empty)
                    ? string.Format("{0:N6}", 1)
                    : string.Format("{0:N6}", drRecEdit["CurrencyRate"].ToString());

                // Modified on: 15/11/2017
                lbl_CommitDate.Text = (drRecEdit["CommitDate"] != DBNull.Value)
                    ? Convert.ToDateTime(drRecEdit["CommitDate"]).ToString("dd/MM/yyyy") : string.Empty;
                // End Modified.

                lbl_VendorCode.Text = drRecEdit["VendorCode"].ToString();
                lbl_VendorNm.Text = " : " + vendor.GetName(drRecEdit["VendorCode"].ToString(), hf_ConnStr.Value);
                cmb_DeliPoint.Value = drRecEdit["DeliPoint"];
                lbl_Receiver.Text = LoginInfo.LoginName;

                //btn_AddPo.Enabled = false;
                btn_AddPo.Visible = false;
            }




            ddl_Vendor.DataSource = dsRecEdit.Tables[vendor.TableName];
            ddl_Vendor.ValueField = "VendorCode";
            ddl_Vendor.TextField = "Name";
            ddl_Vendor.DataBind();

            cmb_DeliPoint.DataSource = dsRecEdit.Tables[deliPoint.TableName];
            cmb_DeliPoint.ValueField = "DptCode";
            cmb_DeliPoint.TextField = "Name";
            cmb_DeliPoint.DataBind();

            grd_RecEdit.DataSource = dsRecEdit.Tables[recDt.TableName];
            grd_RecEdit.DataBind();
        }

        /// <summary>
        /// </summary>
        private void GetLookup()
        {
            vendor.GetList(dsRecEdit, hf_ConnStr.Value);
            //curr.GetList(dsRecEdit, LoginInfo.ConnStr);
            locat.GetList(dsRecEdit, hf_ConnStr.Value);
            deliPoint.GetList(dsRecEdit, hf_ConnStr.Value);
            product.GetLookUp(dsRecEdit, hf_ConnStr.Value);
            unit.Get(dsRecEdit, hf_ConnStr.Value);
        }

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
            catch (Exception ex)
            {
                con.Close();
                LogManager.Error(ex);

            }
            dsImport.Tables.Add(dt);
        }

        protected void grd_RecEdit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (grd_RecEdit.EditIndex != -1)
            {
                btn_Back.Visible = false;
                btn_Save.Visible = false;
                btn_Commit.Visible = false;
            }
            else
            {
                btn_Back.Visible = true;
                btn_Save.Visible = true;
                btn_Commit.Visible = true;
            }

            #region DataControlRowType.DataRow
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("hf_RecNo") != null)
                {
                    var hf = e.Row.FindControl("hf_RecNo") as HiddenField;
                    hf.Value = DataBinder.Eval(e.Row.DataItem, "RecNo").ToString();
                }

                if (e.Row.FindControl("hf_RecDtNo") != null)
                {
                    var hf = e.Row.FindControl("hf_RecDtNo") as HiddenField;
                    hf.Value = DataBinder.Eval(e.Row.DataItem, "RecDtNo").ToString();
                }


                if (e.Row.FindControl("lbl_Location") != null)
                {
                    var lbl = e.Row.FindControl("lbl_Location") as Label;
                    string loc = locat.GetName(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(), hf_ConnStr.Value);
                    lbl.Text = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString() + " : " + loc;
                    lbl.ToolTip = lbl.Text;
                }

                if (e.Row.FindControl("lbl_ProductCode") != null)
                {
                    var lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                    lbl_ProductCode.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode") + " : " +
                                           product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                               hf_ConnStr.Value) + " : " +
                                           product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                               hf_ConnStr.Value);
                    lbl_ProductCode.ToolTip = lbl_ProductCode.Text;
                }

                if (e.Row.FindControl("lbl_OrderQty") != null)
                {
                    var lbl_OrderQty = e.Row.FindControl("lbl_OrderQty") as Label;
                    var orderqty = (DataBinder.Eval(e.Row.DataItem, "OrderQty") == DBNull.Value
                        ? 0
                        : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "OrderQty").ToString()));
                    lbl_OrderQty.Text = (orderqty - recQty).ToString();
                    lbl_OrderQty.ToolTip = lbl_OrderQty.Text;
                }

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "UnitCode").ToString();
                    lbl_Unit.ToolTip = lbl_Unit.Text;
                }


                if (e.Row.FindControl("lbl_RecQty") != null)
                {
                    var lbl_RecQty = e.Row.FindControl("lbl_RecQty") as Label;
                    lbl_RecQty.Text =
                        (DataBinder.Eval(e.Row.DataItem, "RecQty") == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "RecQty").ToString())).ToString();
                    lbl_RecQty.ToolTip = lbl_RecQty.Text;

                    //grandRecAmt += Convert.ToDecimal(lbl_RecQty.Text);
                }

                //decimal rcv;
                //decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "RecQty").ToString(), out rcv);
                if (e.Row.FindControl("se_RecQtyEdit") != null)
                {
                    var se_RecQtyEdit = e.Row.FindControl("se_RecQtyEdit") as ASPxSpinEdit;

                    if (qtyrecUpdate == 0)
                    {
                        se_RecQtyEdit.Text =
                            (DataBinder.Eval(e.Row.DataItem, "RecQty") == DBNull.Value
                                ? 0
                                : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "RecQty").ToString())).ToString();
                    }
                    else
                    {
                        se_RecQtyEdit.Text = qtyrecUpdate.ToString();
                    }
                }

                if (e.Row.FindControl("lbl_RcvUnit") != null)
                {
                    var lbl_RcvUnit = e.Row.FindControl("lbl_RcvUnit") as Label;
                    lbl_RcvUnit.Text = DataBinder.Eval(e.Row.DataItem, "RcvUnit").ToString();
                    lbl_RcvUnit.ToolTip = lbl_RcvUnit.Text;
                }

                if (e.Row.FindControl("ddl_RcvUnit") != null)
                {
                    var ddl_RcvUnit = e.Row.FindControl("ddl_RcvUnit") as ASPxComboBox;
                    ddl_RcvUnit.DataSource =
                        prodUnit.GetLookUp_ProductCode(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                            hf_ConnStr.Value);
                    ddl_RcvUnit.DataBind();
                    ddl_RcvUnit.Value = DataBinder.Eval(e.Row.DataItem, "RcvUnit");
                }


                if (e.Row.FindControl("lbl_FocQty") != null)
                {
                    var lbl_FocQty = e.Row.FindControl("lbl_FocQty") as Label;
                    lbl_FocQty.Text =
                        (DataBinder.Eval(e.Row.DataItem, "FOCQty") == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "FOCQty").ToString())).ToString();
                    lbl_FocQty.ToolTip = lbl_FocQty.Text;
                }

                if (e.Row.FindControl("se_FocEdit") != null)
                {
                    var se_FocEdit = e.Row.FindControl("se_FocEdit") as ASPxSpinEdit;
                    se_FocEdit.Text =
                        (DataBinder.Eval(e.Row.DataItem, "FOCQty") == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "FOCQty").ToString())).ToString();
                }

                //decimal pricePerU;
                //decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "Price").ToString(), out pricePerU);

                if (e.Row.FindControl("lbl_Price") != null)
                {
                    var lbl_Price = e.Row.FindControl("lbl_Price") as Label;
                    lbl_Price.Text = String.Format("{0:N4}",
                        (DataBinder.Eval(e.Row.DataItem, "Price") == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Price").ToString())));
                    lbl_Price.ToolTip = lbl_Price.Text;
                }

                if (e.Row.FindControl("se_PriceEdit") != null)
                {
                    var se_PriceEdit = e.Row.FindControl("se_PriceEdit") as ASPxSpinEdit;

                    if (priceUpdate == 0)
                    {
                        se_PriceEdit.Text =
                            (DataBinder.Eval(e.Row.DataItem, "Price") == DBNull.Value
                                ? 0
                                : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Price").ToString())).ToString();
                    }
                    else
                    {
                        se_PriceEdit.Text = priceUpdate.ToString();
                    }
                }

                if (e.Row.FindControl("lbl_ExtraCost") != null)
                {
                    var lbl_ExtraCost = e.Row.FindControl("lbl_ExtraCost") as Label;
                    lbl_ExtraCost.Text = String.Format(DefaultAmtFmt,
                        (DataBinder.Eval(e.Row.DataItem, "ExtraCost") == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ExtraCost").ToString())));
                    lbl_ExtraCost.ToolTip = lbl_ExtraCost.Text;
                }

                if (e.Row.FindControl("lbl_TotalAmt") != null)
                {
                    //var lbl_TotalAmt = e.Row.FindControl("lbl_TotalAmt") as Label;

                    //lbl_TotalAmt.Text = String.Format("{0:N}",
                    //    (DataBinder.Eval(e.Row.DataItem, "TotalAmt") == DBNull.Value
                    //        ? 0
                    //        : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalAmt").ToString())));
                    //lbl_TotalAmt.ToolTip = lbl_TotalAmt.Text;

                    //grandTotal += Convert.ToDecimal(lbl_TotalAmt.Text);

                    var lbl_TotalAmt = e.Row.FindControl("lbl_TotalAmt") as Label;

                    // Modified on: 05/02/2018, By: Fon, For: Following from P' Oat request.
                    lbl_TotalAmt.Text = String.Format(DefaultAmtFmt,
                        (DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt") == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt").ToString())));
                    //lbl_TotalAmt.Text = string.Format("{0:N}", rcv * pricePerU);
                    // End Modified.

                    lbl_TotalAmt.ToolTip = lbl_TotalAmt.Text;
                }

                //Tax Type in Gridview Row.
                if (e.Row.FindControl("lbl_TaxType_Row") != null)
                {
                    var lbl_TaxType = e.Row.FindControl("lbl_TaxType_Row") as Label;
                    lbl_TaxType.Text = "(" +
                                       Blue.BL.GnxLib.GetTaxTypeName(
                                           DataBinder.Eval(e.Row.DataItem, "TaxType").ToString()) + ")";
                    lbl_TaxType.ToolTip = lbl_TaxType.Text;
                }

                if (e.Row.FindControl("lbl_Status") != null)
                {
                    var lbl_Status = e.Row.FindControl("lbl_Status") as Label;
                    lbl_Status.Text = DataBinder.Eval(e.Row.DataItem, "Status").ToString();
                    lbl_Status.ToolTip = lbl_Status.Text;
                }

                // Added on: 02/02/2018, By: Fon
                var lblNm_DiscAmt = e.Row.FindControl("lblNm_DiscAmt") as Label;
                if (lblNm_DiscAmt != null)
                {
                    lblNm_DiscAmt.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrDiscAmt"));
                }

                var lblNm_NetAmt = e.Row.FindControl("lblNm_NetAmt") as Label;
                if (lblNm_NetAmt != null)
                {
                    lblNm_NetAmt.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrNetAmt"));
                }
                // End Added.

                grandNetAmt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "NetAmt") == DBNull.Value ? 0 : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "NetAmt").ToString()));
                grandTaxAmt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TaxAmt") == DBNull.Value ? 0 : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TaxAmt").ToString()));
                grandTotal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalAmt") == DBNull.Value ? 0 : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalAmt").ToString()));



                if (e.Row.FindControl("lbl_ExpiryDate") != null)
                {
                    var lbl_ExpiryDate = e.Row.FindControl("lbl_ExpiryDate") as Label;
                    if (string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "ExpiryDate").ToString()))
                        lbl_ExpiryDate.Text = string.Empty;
                    else
                        lbl_ExpiryDate.Text = string.Format("{0:d}", (DateTime)DataBinder.Eval(e.Row.DataItem, "ExpiryDate"));

                    lbl_ExpiryDate.ToolTip = lbl_ExpiryDate.Text;
                }

                if (e.Row.FindControl("de_ExpiryDate") != null)
                {
                    var de_ExpiryDate = e.Row.FindControl("de_ExpiryDate") as ASPxDateEdit;
                    if (!string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "ExpiryDate").ToString()))
                        de_ExpiryDate.Date = (DateTime)DataBinder.Eval(e.Row.DataItem, "ExpiryDate");
                    de_ExpiryDate.ToolTip = de_ExpiryDate.Text;
                }



                //-------------------------------Expand----------------------------------------



                if (e.Row.FindControl("lbl_Disc") != null)
                {
                    var lbl_Disc = e.Row.FindControl("lbl_Disc") as Label;
                    lbl_Disc.Text = DataBinder.Eval(e.Row.DataItem, "Discount").ToString();
                    lbl_Disc.ToolTip = lbl_Disc.Text;
                }
                if (e.Row.FindControl("txt_Disc") != null)
                {
                    var txt_Disc = e.Row.FindControl("txt_Disc") as TextBox;
                    txt_Disc.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "Discount"));
                    txt_Disc.ToolTip = txt_Disc.Text;
                }

                if (e.Row.FindControl("hf_DiscRate") != null)
                {
                    var hf_DiscRate = e.Row.FindControl("hf_DiscRate") as HiddenField;
                    hf_DiscRate.Value = DataBinder.Eval(e.Row.DataItem, "Discount").ToString();
                }

                if (e.Row.FindControl("lbl_DiscAmt") != null)
                {
                    var lbl_DiscAmt = e.Row.FindControl("lbl_DiscAmt") as Label;
                    lbl_DiscAmt.Text = String.Format("({0})", string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "DiccountAmt")));
                    lbl_DiscAmt.ToolTip = lbl_DiscAmt.Text;
                }

                if (e.Row.FindControl("lbl_DiscAmt_Edit") != null)
                {
                    var lbl_DiscAmt_Edit = e.Row.FindControl("lbl_DiscAmt_Edit") as Label;
                    lbl_DiscAmt_Edit.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "DiccountAmt"));
                    lbl_DiscAmt_Edit.ToolTip = lbl_DiscAmt_Edit.Text;
                }

                if (e.Row.FindControl("txt_DiscAmt") != null)
                {
                    var txt_DiscAmt = e.Row.FindControl("txt_DiscAmt") as TextBox;
                    txt_DiscAmt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "DiccountAmt"));
                    txt_DiscAmt.ToolTip = txt_DiscAmt.Text;
                }

                if (e.Row.FindControl("lbl_Receive") != null)
                {
                    var lbl_Receive = e.Row.FindControl("lbl_Receive") as Label;
                    lbl_Receive.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "RecQty"));
                    lbl_Receive.ToolTip = lbl_Receive.Text;
                }

                if (e.Row.FindControl("lbl_RcvUnit_Expand") != null)
                {
                    var lbl_InvUnit = e.Row.FindControl("lbl_RcvUnit_Expand") as Label;
                    lbl_InvUnit.Text = DataBinder.Eval(e.Row.DataItem, "RcvUnit").ToString();
                    lbl_InvUnit.ToolTip = lbl_InvUnit.Text;
                }

                if (e.Row.FindControl("lbl_ConvertRate") != null)
                {
                    var lbl_ConvertRate = e.Row.FindControl("lbl_ConvertRate") as Label;
                    lbl_ConvertRate.Text = String.Format("{0:N3}", DataBinder.Eval(e.Row.DataItem, "Rate"));
                    lbl_ConvertRate.ToolTip = lbl_ConvertRate.Text;
                }

                if (e.Row.FindControl("lbl_BaseUnit") != null)
                {
                    var lbl_BaseUnit = e.Row.FindControl("lbl_BaseUnit") as Label;
                    lbl_BaseUnit.Text = prodUnit.GetInvenUnit(
                        DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), hf_ConnStr.Value);
                    lbl_BaseUnit.ToolTip = lbl_BaseUnit.Text;
                }

                if (e.Row.FindControl("lbl_TaxType") != null)
                {
                    var lbl_TaxType = e.Row.FindControl("lbl_TaxType") as Label;
                    lbl_TaxType.Text =
                        Blue.BL.GnxLib.GetTaxTypeName(DataBinder.Eval(e.Row.DataItem, "TaxType").ToString());
                    lbl_TaxType.ToolTip = lbl_TaxType.Text;
                }

                if (e.Row.FindControl("ddl_TaxType") != null)
                {
                    var ddl_TaxType = e.Row.FindControl("ddl_TaxType") as DropDownList;
                    ddl_TaxType.SelectedValue = DataBinder.Eval(e.Row.DataItem, "TaxType").ToString();
                }

                if (e.Row.FindControl("hf_TaxType") != null)
                {
                    var hf_TaxType = e.Row.FindControl("hf_TaxType") as HiddenField;
                    hf_TaxType.Value = DataBinder.Eval(e.Row.DataItem, "TaxType").ToString();
                }



                if (e.Row.FindControl("lbl_TaxRate") != null)
                {
                    var lbl_TaxRate = e.Row.FindControl("lbl_TaxRate") as Label;
                    lbl_TaxRate.Text = DataBinder.Eval(e.Row.DataItem, "TaxRate").ToString();
                    lbl_TaxRate.ToolTip = lbl_TaxRate.Text;
                }

                if (e.Row.FindControl("txt_TaxRate") != null)
                {
                    //ASPxSpinEdit spe_TaxRate = e.Row.FindControl("spe_TaxRate") as ASPxSpinEdit;
                    var txt_TaxRate = e.Row.FindControl("txt_TaxRate") as TextBox;
                    txt_TaxRate.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "TaxRate"));
                }

                if (e.Row.FindControl("hf_TaxRate") != null)
                {
                    var hf_TaxRate = e.Row.FindControl("hf_TaxRate") as HiddenField;
                    hf_TaxRate.Value = DataBinder.Eval(e.Row.DataItem, "TaxRate").ToString();
                }


                if (e.Row.FindControl("lbl_PrRef") != null)
                {
                    var lbl_PrRef = e.Row.FindControl("lbl_PrRef") as Label;

                    var dtPr = new DataTable();
                    dtPr = recDt.GetPRList(DataBinder.Eval(e.Row.DataItem, "PoNo").ToString(), hf_ConnStr.Value);

                    if (dtPr.Rows.Count > 0)
                    {
                        foreach (DataRow drPr in dtPr.Rows)
                        {
                            if (lbl_PrRef.Text == string.Empty)
                            {
                                lbl_PrRef.Text = drPr[0].ToString();
                            }
                            else
                            {
                                lbl_PrRef.Text = lbl_PrRef.Text + ", " + drPr[0];
                            }
                        }
                    }

                    lbl_PrRef.ToolTip = lbl_PrRef.Text;
                }

                if (e.Row.FindControl("lbl_PoRef") != null)
                {
                    var lbl_PoRef = e.Row.FindControl("lbl_PoRef") as Label;
                    lbl_PoRef.Text = DataBinder.Eval(e.Row.DataItem, "PoNo").ToString();
                    lbl_PoRef.ToolTip = lbl_PoRef.Text;

                }

                if (e.Row.FindControl("hf_PoDtNo") != null)
                {
                    var hf = e.Row.FindControl("hf_PoDtNo") as HiddenField;
                    hf.Value = DataBinder.Eval(e.Row.DataItem, "PoDtNo").ToString();

                }


                if (e.Row.FindControl("lbl_Avg") != null)
                {
                    var startDate = period.GetStartDate(ServerDateTime.Date, hf_ConnStr.Value);
                    var endDate = period.GetEndDate(ServerDateTime.Date, hf_ConnStr.Value);

                    var lbl_Avg = e.Row.FindControl("lbl_Avg") as Label;
                    lbl_Avg.Text = String.Format(DefaultAmtFmt,
                        inv.GetPAvgAudit(startDate, endDate, DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(),
                            DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), hf_ConnStr.Value));
                    lbl_Avg.ToolTip = lbl_Avg.Text;
                }

                if (dsRecEdit.Tables[rec.TableName].Rows[0]["PoSource"].ToString() != LoginInfo.BuInfo.BuCode &&
                    dsRecEdit.Tables[rec.TableName].Rows[0]["PoSource"].ToString() != null)
                {
                    if (e.Row.FindControl("lbl_BuCodeText") != null)
                    {
                        var lbl_BuCodeText = e.Row.FindControl("lbl_BuCodeText") as Label;
                        lbl_BuCodeText.Visible = true;
                    }

                    if (e.Row.FindControl("lbl_BuCode") != null)
                    {
                        var lbl_BuCode = e.Row.FindControl("lbl_BuCode") as Label;
                        lbl_BuCode.Text = LoginInfo.BuInfo.BuCode;
                        lbl_BuCode.Visible = true;
                    }
                }

                if (e.Row.FindControl("lbl_NetAmt") != null)
                {
                    var lbl_NetAmt = e.Row.FindControl("lbl_NetAmt") as Label;

                    lbl_NetAmt.Text = String.Format(DefaultAmtFmt,
                        (DataBinder.Eval(e.Row.DataItem, "NetAmt") == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "NetAmt").ToString())));
                    lbl_NetAmt.ToolTip = lbl_NetAmt.Text;
                }

                if (e.Row.FindControl("lbl_NetAmt_Edit") != null)
                {
                    var lbl_NetAmt_Edit = e.Row.FindControl("lbl_NetAmt_Edit") as Label;

                    lbl_NetAmt_Edit.Text = String.Format(DefaultAmtFmt,
                        (DataBinder.Eval(e.Row.DataItem, "NetAmt") == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "NetAmt").ToString())));
                    lbl_NetAmt_Edit.ToolTip = lbl_NetAmt_Edit.Text;
                }

                if (e.Row.FindControl("txt_NetAmt") != null)
                {
                    var txt_NetAmt = e.Row.FindControl("txt_NetAmt") as TextBox;
                    txt_NetAmt.Text = String.Format(DefaultAmtFmt,
                        (DataBinder.Eval(e.Row.DataItem, "NetAmt") == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "NetAmt").ToString())));
                }

                if (e.Row.FindControl("lbl_TaxAmt") != null)
                {
                    var lbl_TaxAmt = e.Row.FindControl("lbl_TaxAmt") as Label;

                    lbl_TaxAmt.Text = String.Format(DefaultAmtFmt,
                        (DataBinder.Eval(e.Row.DataItem, "TaxAmt") == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TaxAmt").ToString())));
                    lbl_TaxAmt.ToolTip = lbl_TaxAmt.Text;
                }

                if (e.Row.FindControl("lbl_TaxAmt_Edit") != null)
                {
                    var lbl_TaxAmt_Edit = e.Row.FindControl("lbl_TaxAmt_Edit") as Label;

                    lbl_TaxAmt_Edit.Text = String.Format(DefaultAmtFmt,
                        (DataBinder.Eval(e.Row.DataItem, "TaxAmt") == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TaxAmt").ToString())));
                    lbl_TaxAmt_Edit.ToolTip = lbl_TaxAmt_Edit.Text;
                }

                if (e.Row.FindControl("lbl_TotalAmtDt") != null)
                {
                    var lbl_TotalAmtDt = e.Row.FindControl("lbl_TotalAmtDt") as Label;
                    lbl_TotalAmtDt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TotalAmt"));
                    lbl_TotalAmtDt.ToolTip = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TotalAmt"));
                }

                if (e.Row.FindControl("lbl_Receiver") != null)
                {
                    var lbl_Receiver = e.Row.FindControl("lbl_Receiver") as Label;
                    lbl_Receiver.Text = dsRecEdit.Tables[rec.TableName].Rows[0]["CreatedBy"].ToString();
                    lbl_Receiver.ToolTip = lbl_Receiver.Text;
                }

                if (e.Row.FindControl("lbl_BaseQty") != null)
                {
                    var lbl_BaseQty = e.Row.FindControl("lbl_BaseQty") as Label;

                    if (DataBinder.Eval(e.Row.DataItem, "Rate").ToString() != string.Empty)
                    {
                        var strNewUnit = prodUnit.GetInvenUnit(
                            DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), hf_ConnStr.Value);
                        var strOldUnit = DataBinder.Eval(e.Row.DataItem, "RcvUnit").ToString();
                        var decBaseQty =
                            prodUnit.GetQtyAfterChangeUnit(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                strNewUnit, strOldUnit,
                                Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "RecQty").ToString()),
                                hf_ConnStr.Value);

                        lbl_BaseQty.Text = String.Format(DefaultQtyFmt, decBaseQty);
                    }

                    lbl_BaseQty.ToolTip = lbl_BaseQty.Text;
                }

                if (e.Row.FindControl("lbl_DtrComment") != null)
                {
                    var lbl_DtrComment = e.Row.FindControl("lbl_DtrComment") as Label;
                    lbl_DtrComment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                    lbl_DtrComment.ToolTip = lbl_DtrComment.Text;
                }

                if (e.Row.FindControl("txt_DtrComment") != null)
                {
                    var txt_DtrComment = e.Row.FindControl("txt_DtrComment") as TextBox;
                    txt_DtrComment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                }

                if (e.Row.FindControl("txt_TaxAmt") != null)
                {
                    var txt_TaxAmt = e.Row.FindControl("txt_TaxAmt") as TextBox;
                    txt_TaxAmt.Text = String.Format(DefaultAmtFmt,
                        (DataBinder.Eval(e.Row.DataItem, "TaxAmt") == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TaxAmt").ToString())));
                }

                if (e.Row.FindControl("chk_TaxAdj") != null)
                {
                    //var chk_TaxAdj = e.Row.FindControl("chk_TaxAdj") as CheckBox;
                    //chk_TaxAdj.Checked =
                    //    Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "TaxAdj") == DBNull.Value
                    //        ? false
                    //        : Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "TaxAdj")));
                    // Added on: 24/08/2017, By: Fon
                    //UIControlEditGrid_chk_TaxAdj(e);
                    var chk_TaxAdj = e.Row.FindControl("chk_TaxAdj") as CheckBox;
                    if (chk_TaxAdj != null)
                    {
                        //chk_TaxAdj.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "TaxAdj") != DBNull.Value && Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "TaxAdj")));
                        chk_TaxAdj.Checked =
                                DataBinder.Eval(e.Row.DataItem, "TaxAdj") == DBNull.Value
                                ? false
                                : Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "TaxAdj"));

                        DropDownList ddl_TaxType = new DropDownList();
                        TextBox txt_TaxRate = new TextBox();
                        TextBox txt_TaxAmt = new TextBox();
                        TextBox txt_CurrTaxAmt = new TextBox();
                        bool taxAdjCheked = chk_TaxAdj.Checked;

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

                                txt_TaxAmt.Enabled = false;
                            }
                        }
                        // End if find dropdownlist
                    }


                }


                if (e.Row.FindControl("chk_DiscAdj") != null)
                {
                    var chk_DiscAdj = e.Row.FindControl("chk_DiscAdj") as CheckBox;
                    chk_DiscAdj.Checked =
                        Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "DiscAdj") == DBNull.Value
                            ? false
                            : DataBinder.Eval(e.Row.DataItem, "DiscAdj"));

                    #region comment
                    //ASPxSpinEdit se_DiscAmt = e.Row.FindControl("se_DiscAmt") as ASPxSpinEdit;
                    //ASPxSpinEdit spe_Disc   = e.Row.FindControl("spe_Disc") as ASPxSpinEdit;
                    //var txt_DiscAmt = e.Row.FindControl("txt_DiscAmt") as TextBox;
                    //var txt_Disc = e.Row.FindControl("txt_Disc") as TextBox;
                    //if (chk_DiscAdj.Checked)
                    //{
                    //    if (txt_Disc != null)
                    //    {
                    //        txt_Disc.Enabled = true;
                    //    }

                    //    if (txt_DiscAmt != null)
                    //    {
                    //        txt_DiscAmt.Enabled = true;
                    //    }
                    //}
                    //else
                    //{
                    //    if (txt_Disc != null)
                    //    {
                    //        txt_Disc.Enabled = false;
                    //    }

                    //    if (txt_DiscAmt != null)
                    //    {
                    //        txt_DiscAmt.Enabled = false;
                    //    }
                    //}
                    #endregion

                    // Added on: 24/08/2017, By: Fon
                    UIControlEditGrid_chk_DiscAdj(e);
                }

                if (e.Row.FindControl("lbl_NetAcc") != null)
                {
                    var lbl_NetAcc = e.Row.FindControl("lbl_NetAcc") as Label;
                    lbl_NetAcc.Text = DataBinder.Eval(e.Row.DataItem, "NetDrAcc").ToString();
                    lbl_NetAcc.ToolTip = lbl_NetAcc.Text;
                }


                if (e.Row.FindControl("lbl_TaxAcc") != null)
                {
                    var lbl_TaxAcc = e.Row.FindControl("lbl_TaxAcc") as Label;
                    lbl_TaxAcc.Text = DataBinder.Eval(e.Row.DataItem, "TaxDrAcc").ToString();
                    lbl_TaxAcc.ToolTip = lbl_TaxAcc.Text;
                }




                // Added on: 21/08/2017, By: Fon
                #region Ori. Currency part
                decimal currNetAmt = 0;
                if (DataBinder.Eval(e.Row.DataItem, "CurrNetAmt") != DBNull.Value)
                    currNetAmt = decimal.Parse(DataBinder.Eval(e.Row.DataItem, "CurrNetAmt").ToString());

                decimal currDiscAmt = 0;
                if (DataBinder.Eval(e.Row.DataItem, "CurrDiscAmt") != DBNull.Value)
                    currDiscAmt = decimal.Parse(DataBinder.Eval(e.Row.DataItem, "CurrDiscAmt").ToString());

                decimal currTaxAmt = 0;
                if (DataBinder.Eval(e.Row.DataItem, "CurrTaxAmt") != DBNull.Value)
                    currTaxAmt = decimal.Parse(DataBinder.Eval(e.Row.DataItem, "CurrTaxAmt").ToString());

                decimal currTotalAmt = 0;
                if (DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt") != DBNull.Value)
                    currTotalAmt = decimal.Parse(DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt").ToString());

                // Title
                if (e.Row.FindControl("lbl_CurrCurrDt") != null)
                {
                    Label lbl_CurrCurrDt = (Label)e.Row.FindControl("lbl_CurrCurrDt");
                    lbl_CurrCurrDt.Text = string.Format("( {0} )", ddl_Currency.Value);
                }

                if (e.Row.FindControl("lbl_BaseCurrDt") != null)
                {
                    Label lbl_BaseCurrDt = (Label)e.Row.FindControl("lbl_BaseCurrDt");
                    lbl_BaseCurrDt.Text = string.Format("( {0} )", baseCurrency);
                }

                // Net Amount
                currGrandNetAmt += currNetAmt;
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

                // Disc Amount
                if (e.Row.FindControl("lbl_CurrDiscAmt") != null)
                {
                    Label lbl_CurrDiscAmt = (Label)e.Row.FindControl("lbl_CurrDiscAmt");
                    lbl_CurrDiscAmt.Text = string.Format("({0})", string.Format(DefaultAmtFmt, currDiscAmt));
                }
                if (e.Row.FindControl("txt_CurrDiscAmt") != null)
                {
                    TextBox txt_CurrDiscAmt = (TextBox)e.Row.FindControl("txt_CurrDiscAmt");
                    txt_CurrDiscAmt.Text = string.Format(DefaultAmtFmt, currDiscAmt);
                }

                //  Tax Amount
                currGrandTaxAmt += currTaxAmt;
                if (e.Row.FindControl("lbl_CurrTaxAmt") != null)
                {
                    Label lbl_CurrTaxAmt = (Label)e.Row.FindControl("lbl_CurrTaxAmt");
                    lbl_CurrTaxAmt.Text = string.Format(DefaultAmtFmt, currTaxAmt);
                }
                if (e.Row.FindControl("txt_CurrTaxAmt") != null)
                {
                    TextBox txt_CurrTaxAmt = (TextBox)(e.Row.FindControl("txt_CurrTaxAmt"));
                    txt_CurrTaxAmt.Text = string.Format(DefaultAmtFmt, currTaxAmt);
                }

                // Total Amount
                currGrandTotal = currGrandNetAmt + currGrandTaxAmt;
                if (e.Row.FindControl("lbl_CurrTotalAmtDt") != null)
                {
                    Label lbl_CurrTotalAmtDt = (Label)e.Row.FindControl("lbl_CurrTotalAmtDt");
                    lbl_CurrTotalAmtDt.Text = string.Format(DefaultAmtFmt, currTotalAmt);
                }


                #endregion

                //************************** Display Stock Movement ****************************
                if (e.Row.FindControl("uc_StockMovement") != null)
                {
                    var uc_StockMovement = e.Row.FindControl("uc_StockMovement") as BlueLedger.PL.PC.StockMovement;
                    uc_StockMovement.HdrNo = DataBinder.Eval(e.Row.DataItem, "RecNo").ToString();
                    uc_StockMovement.DtNo = DataBinder.Eval(e.Row.DataItem, "RecDtNo").ToString();
                    uc_StockMovement.ConnStr = hf_ConnStr.Value;
                    uc_StockMovement.DataBind();
                }

                // Display Stock Summary --------------------------------------------------------------   

                if (dsRecEdit.Tables[prDt.TableName] != null)
                {
                    dsRecEdit.Tables[prDt.TableName].Clear();
                }

                #region Get stock summary
                var get = prDt.GetStockSummary(dsRecEdit, DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                    DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(),
                    de_RecDate.Text, hf_ConnStr.Value);
                if (get)
                {
                    var drStockSummary = dsRecEdit.Tables[prDt.TableName].Rows[0];

                    if (e.Row.FindControl("lbl_OnHand") != null)
                    {
                        var lbl_OnHand = e.Row.FindControl("lbl_OnHand") as Label;
                        if (drStockSummary["OnHand"].ToString() != string.Empty)
                        {
                            lbl_OnHand.Text = String.Format(DefaultQtyFmt, decimal.Parse(drStockSummary["OnHand"].ToString()));
                        }
                        else
                        {
                            lbl_OnHand.Text = "0.00";
                        }

                        lbl_OnHand.ToolTip = lbl_OnHand.Text;
                    }

                    if (e.Row.FindControl("lbl_OnOrder") != null)
                    {
                        var lbl_OnOrder = e.Row.FindControl("lbl_OnOrder") as Label;
                        if (drStockSummary["OnOrder"].ToString() != string.Empty && drStockSummary["OnOrder"] != null)
                        {
                            lbl_OnOrder.Text = String.Format(DefaultQtyFmt,
                                decimal.Parse(drStockSummary["OnOrder"].ToString()));
                        }
                        else
                        {
                            lbl_OnOrder.Text = "0.00";
                        }

                        lbl_OnOrder.ToolTip = lbl_OnOrder.Text;
                    }

                    if (e.Row.FindControl("lbl_Reorder") != null)
                    {
                        var lbl_Reorder = e.Row.FindControl("lbl_Reorder") as Label;

                        if (drStockSummary["Reorder"].ToString() != string.Empty && drStockSummary["Reorder"] != null)
                        {
                            lbl_Reorder.Text = String.Format(DefaultQtyFmt,
                                decimal.Parse(drStockSummary["Reorder"].ToString()));
                        }
                        else
                        {
                            lbl_Reorder.Text = "0.00";
                        }

                        lbl_Reorder.ToolTip = lbl_Reorder.Text;
                    }

                    if (e.Row.FindControl("lbl_Restock") != null)
                    {
                        var lbl_Restock = e.Row.FindControl("lbl_Restock") as Label;

                        if (drStockSummary["ReStock"].ToString() != string.Empty && drStockSummary["ReStock"] != null)
                        {
                            lbl_Restock.Text = String.Format(DefaultQtyFmt,
                                decimal.Parse(drStockSummary["ReStock"].ToString()));
                        }
                        else
                        {
                            lbl_Restock.Text = "0.00";
                        }

                        lbl_Restock.ToolTip = lbl_Restock.Text;
                    }

                    if (e.Row.FindControl("lbl_LastPrice") != null)
                    {
                        var lbl_LastPrice = e.Row.FindControl("lbl_LastPrice") as Label;

                        if (drStockSummary["LastPrice"].ToString() != string.Empty &&
                            drStockSummary["LastPrice"] != null)
                        {
                            lbl_LastPrice.Text = String.Format(DefaultAmtFmt,
                                decimal.Parse(drStockSummary["LastPrice"].ToString()));
                        }
                        else
                        {
                            lbl_LastPrice.Text = "0.00";
                        }

                        lbl_LastPrice.ToolTip = lbl_LastPrice.Text;
                    }

                    if (e.Row.FindControl("lbl_LastVendor") != null)
                    {
                        var lbl_LastVendor = e.Row.FindControl("lbl_LastVendor") as Label;
                        lbl_LastVendor.Text = drStockSummary["LastVendor"].ToString();
                        lbl_LastVendor.ToolTip = lbl_LastVendor.Text;
                    }
                }
                #endregion
            }

            #endregion

            #region ControlRowType.Footer
            if (e.Row.RowType == DataControlRowType.Footer)
            {

                if (e.Row.FindControl("lbl_GrandTotalNet") != null)
                {
                    var lbl_GrandTotalNet = e.Row.FindControl("lbl_GrandTotalNet") as Label;
                    lbl_GrandTotalNet.Text = String.Format(DefaultAmtFmt, grandNetAmt);
                }

                if (e.Row.FindControl("lbl_GrandTotalTax") != null)
                {
                    var lbl_GrandTotalTax = e.Row.FindControl("lbl_GrandTotalTax") as Label;
                    lbl_GrandTotalTax.Text = String.Format(DefaultAmtFmt, grandTaxAmt);
                }

                if (e.Row.FindControl("lbl_GrandTotalAmt") != null)
                {
                    var lbl_GrandTotalAmt = e.Row.FindControl("lbl_GrandTotalAmt") as Label;
                    lbl_GrandTotalAmt.Text = String.Format(DefaultAmtFmt, grandNetAmt + grandTaxAmt);
                }

                // Added on: 25/08/2017, By: Fon
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
                    lbl_CurrGrandTotalTax.Text = string.Format(DefaultAmtFmt, currGrandTaxAmt);
                }

                if (e.Row.FindControl("lbl_CurrGrandTotalAmt") != null)
                {
                    Label lbl_CurrGrandTotalAmt = (Label)e.Row.FindControl("lbl_CurrGrandTotalAmt");
                    lbl_CurrGrandTotalAmt.Text = string.Format(DefaultAmtFmt, currGrandTotal);
                }
                // End Added.
            }
            #endregion
        }

        private void UIControlEditGrid_chk_DiscAdj(GridViewRowEventArgs e)
        {
            var chkDiscAdj = e.Row.FindControl("chk_DiscAdj") as CheckBox;

            if (chkDiscAdj != null)
            {
                chkDiscAdj.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "DiscAdj") == DBNull.Value
                    ? false
                    : DataBinder.Eval(e.Row.DataItem, "DiscAdj"));

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

                if (e.Row.FindControl("txt_DiscAmt") != null)
                {
                    txt_DiscAmt = (TextBox)e.Row.FindControl("txt_DiscAmt");
                    txt_DiscAmt.Enabled = discAdjChecked;
                }

                if (e.Row.FindControl("txt_CurrDiscAmt") != null)
                {
                    txt_CurrDiscAmt = (TextBox)e.Row.FindControl("txt_CurrDiscAmt");
                    txt_CurrDiscAmt.Enabled = discAdjChecked;
                }
                // End Modified.
            }
        }

        //private void UIControlEditGrid_chk_TaxAdj(GridViewRowEventArgs e)
        //{
        //    var chkTaxAdj = e.Row.FindControl("chk_TaxAdj") as CheckBox;
        //    if (chkTaxAdj != null)
        //    {
        //        chkTaxAdj.Checked =
        //            Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "TaxAdj") != DBNull.Value
        //                            && Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "TaxAdj")));

        //        DropDownList ddl_TaxType = new DropDownList();
        //        TextBox txt_TaxRate = new TextBox();
        //        TextBox txt_TaxAmt = new TextBox();
        //        TextBox txt_CurrTaxAmt = new TextBox();
        //        bool taxAdjCheked = chkTaxAdj.Checked;

        //        if (e.Row.FindControl("ddl_TaxType") != null)
        //        {
        //            ddl_TaxType = (DropDownList)e.Row.FindControl("ddl_TaxType");
        //            txt_TaxRate = (TextBox)e.Row.FindControl("txt_TaxRate");
        //            txt_CurrTaxAmt = (TextBox)e.Row.FindControl("txt_CurrTaxAmt");
        //            txt_TaxAmt = (TextBox)e.Row.FindControl("txt_TaxAmt");

        //            ddl_TaxType.Enabled = taxAdjCheked;
        //            txt_TaxRate.Enabled = false;
        //            txt_CurrTaxAmt.Enabled = false;
        //            txt_TaxAmt.Enabled = false;

        //            if (ddl_TaxType.SelectedItem.Value.ToUpper() != "N")
        //            {
        //                txt_TaxRate.Enabled = taxAdjCheked;
        //                txt_CurrTaxAmt.Enabled = taxAdjCheked;

        //                // Comment on: 01/02/2018, By: Fon, For: Following from P' Oat guide.
        //                txt_TaxAmt.Enabled = taxAdjCheked;
        //                // End Comment.
        //            }
        //        }
        //        // End if find dropdownlist
        //    }
        //}

        protected void grd_RecEdit_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var hf_RecNo = grd_RecEdit.Rows[e.RowIndex].FindControl("hf_RecNo") as HiddenField;
            var hf_RecDtNo = grd_RecEdit.Rows[e.RowIndex].FindControl("hf_RecDtNo") as HiddenField;

            string recNo = hf_RecNo.Value.ToString();
            string recDtNo = hf_RecDtNo.Value.ToString();

            DataTable dt = dsRecEdit.Tables[recDt.TableName];
            DataRow[] dr = dt.Select(string.Format("RecNo='{0}' AND RecDtNo={1}", recNo, recDtNo));

            if (dr.Length > 0)
            {
                int rowIndex = dt.Rows.IndexOf(dr[0]);

                //var drDeleting = dsRecEdit.Tables[recDt.TableName].Rows[e.RowIndex];
                var drDeleting = dsRecEdit.Tables[recDt.TableName].Rows[rowIndex];

                if (drDeleting.RowState != DataRowState.Deleted)
                {
                    if (MODE.ToUpper() == "EDIT" && drDeleting.RowState != DataRowState.Added)
                    {
                        string value = string.Format("{0},{1},{2},{3}",
                            //dsRecEdit.Tables[recDt.TableName].Rows[e.RowIndex]["PoNo"].ToString(),  // PoNo
                            //dsRecEdit.Tables[recDt.TableName].Rows[e.RowIndex]["PoDtNo"].ToString(),  // PoDtNo
                            //dsRecEdit.Tables[recDt.TableName].Rows[e.RowIndex]["RecQty"].ToString(),  // PoDtNo
                            //dsRecEdit.Tables[recDt.TableName].Rows[e.RowIndex]["FocQty"].ToString());  // FocQty

                                dsRecEdit.Tables[recDt.TableName].Rows[rowIndex]["PoNo"].ToString(),  // PoNo
                                dsRecEdit.Tables[recDt.TableName].Rows[rowIndex]["PoDtNo"].ToString(),  // PoDtNo
                                dsRecEdit.Tables[recDt.TableName].Rows[rowIndex]["RecQty"].ToString(),  // PoDtNo
                                dsRecEdit.Tables[recDt.TableName].Rows[rowIndex]["FocQty"].ToString());  // FocQty
                        delValues.Add(value);
                        ViewState["delValues"] = delValues;
                    }

                    //dsRecEdit.Tables[recDt.TableName].Rows[e.RowIndex].Delete();
                    dsRecEdit.Tables[recDt.TableName].Rows[rowIndex].Delete();
                }

                grd_RecEdit.DataSource = dsRecEdit.Tables[recDt.TableName];
                grd_RecEdit.EditIndex = -1;
                grd_RecEdit.DataBind();

                if (MODE.ToUpper() == "EDIT")
                {
                    Session["dsSave"] = dsRecEdit;
                }
                else
                {
                    //Set ViewState to false on 04/04/2012
                    btn_AddPo.Enabled = false;
                }

            }
            else
                e.Cancel = true;



        }

        protected void grd_RecEdit_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //this.Calculate(e.RowIndex);

            var drUpdating = dsRecEdit.Tables[recDt.TableName].Rows[grd_RecEdit.EditIndex];
            var ddl_TaxType = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("ddl_TaxType") as DropDownList;

            //var txt_NetDrAcc = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_NetDrAcc") as TextBox;
            //var txt_TaxDrAcc = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_TaxDrAcc") as TextBox;
            var txt_TaxAmt = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_TaxAmt") as TextBox;
            var txt_TaxRate = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_TaxRate") as TextBox;
            var txt_Disc = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_Disc") as TextBox;
            var txt_DiscAmt = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_DiscAmt") as TextBox;
            var txt_NetAmt = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_NetAmt") as TextBox;
            var txt_DtrComment = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_DtrComment") as TextBox;

            var se_FocEdit = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_FocEdit") as ASPxSpinEdit;

            var chk_TaxAdj = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("chk_TaxAdj") as CheckBox;
            var chk_DiscAdj = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("chk_DiscAdj") as CheckBox;

            var lbl_TotalAmt = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_TotalAmt") as Label;
            var lbl_DiscAmt_Edit = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_DiscAmt_Edit") as Label;

            var lbl_ExtraCost = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_ExtraCost") as Label;
            // Comment on: 28/03/2018, By: Fon, For: Folloeing form P' Oat request.
            //var ddl_NetDrAcc = (ASPxComboBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("ddl_NetDrAcc");
            //var ddl_TaxDrAcc = (ASPxComboBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("ddl_TaxDrAcc");
            // End Comment.

            // Added on: 24/08/2017, By: Fon
            TextBox txt_CurrNetAmt = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_CurrNetAmt");
            TextBox txt_CurrDiscAmt = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_CurrDiscAmt");
            TextBox txt_CurrTaxAmt = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_CurrTaxAmt");
            Label lbl_CurrTotalAmtDt = (Label)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_CurrTotalAmtDt");
            Label lbl_TotalAmtDt = (Label)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_TotalAmtDt");
            // End Added.

            ASPxDateEdit de_ExpiryDate = (ASPxDateEdit)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("de_ExpiryDate");

            //if (txt_TaxDrAcc.Text == string.Empty)
            //{
            //    lbl_WarningDelete.Text = "Tax A/C# is required.";
            //    pop_WarningDelete.ShowOnPageLoad = true;

            //    return;
            //}
            //if (ddl_TaxDrAcc.Value == null)
            //{
            //    lbl_WarningDelete.Text = "Tax A/C# is required.";
            //    pop_WarningDelete.ShowOnPageLoad = true;

            //    return;
            //}
            //taxAcCode = txt_TaxDrAcc.Text;
            //taxAcCode = Convert.ToString(ddl_TaxDrAcc.Text);

            //if (txt_NetDrAcc.Text == string.Empty)
            //{
            //    lbl_WarningDelete.Text = "Net A/C# is required.";
            //    pop_WarningDelete.ShowOnPageLoad = true;

            //    return;
            //}
            //if (ddl_NetDrAcc.Value != null)
            //if (ddl_NetDrAcc.Value == null)
            //{
            //    //lbl_WarningDelete.Text = Convert.ToString(ddl_NetDrAcc.Value);
            //    lbl_WarningDelete.Text = "Net A/C# is required.";
            //    pop_WarningDelete.ShowOnPageLoad = true;

            //    return;
            //}
            //netAcCode = txt_NetDrAcc.Text;
            //netAcCode = Convert.ToString(ddl_NetDrAcc.Text);

            if (decimal.Parse(txt_DiscAmt.Text) < 0)
            {
                //chk_DiscAdj.Checked = false;
                txt_DiscAmt.Text = "0.00";

                lbl_WarningDelete.Text = "Please insert discount amount more than 0";
                pop_WarningDelete.ShowOnPageLoad = true;
                return;
            }

            if (decimal.Parse(txt_DiscAmt.Text) > 0 && decimal.Parse(lbl_TotalAmt.Text) < 0)
            {
                lbl_WarningDelete.Text = "Discount is not allowed exceed than amount of receiving.";
                //"This is action, that cannot be allowed because transaction has discount over than receiving amount.";

                pop_WarningDelete.ShowOnPageLoad = true;
                return;
            }

            if (grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_OrderQty") != null)
            {
                var lbl_OrderQty = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_OrderQty") as Label;
                var ordqty = Convert.ToDecimal(lbl_OrderQty.Text);
                drUpdating["OrderQty"] = Convert.ToDecimal(lbl_OrderQty.Text);
            }

            if (grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_PriceEdit") != null)
            {
                var se_PriceEdit = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_PriceEdit") as ASPxSpinEdit;
                drUpdating["Price"] = Convert.ToDecimal(se_PriceEdit.Text);
            }

            if (grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_RecQtyEdit") != null)
            {
                var se_RecQtyEdit = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_RecQtyEdit") as ASPxSpinEdit;
                drUpdating["RecQty"] = Convert.ToDecimal(se_RecQtyEdit.Text);
            }

            if (grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("ddl_RcvUnit") != null)
            {
                var ddl_RcvUnit = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("ddl_RcvUnit") as ASPxComboBox;
                drUpdating["RcvUnit"] = ddl_RcvUnit.Value;
            }

            if (grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_ConvertRate") != null)
            {
                var lbl_ConvRate = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_ConvertRate") as Label;
                drUpdating["Rate"] = lbl_ConvRate.Text;
            }

            drUpdating["DiscAdj"] = chk_DiscAdj.Checked;
            drUpdating["Discount"] = Convert.ToDecimal(txt_Disc.Text);

            if (chk_DiscAdj.Checked)
            {
                drUpdating["DiccountAmt"] = txt_DiscAmt.Text;
            }
            else
            {
                drUpdating["DiccountAmt"] = discountUpdateAmt * Convert.ToDecimal(drUpdating["RecQty"].ToString());
            }

            drUpdating["TaxAdj"] = chk_TaxAdj.Checked;
            drUpdating["TaxType"] = ddl_TaxType.SelectedItem.Value;
            drUpdating["TaxRate"] = Convert.ToDecimal(txt_TaxRate.Text);
            drUpdating["NetAmt"] = (txt_NetAmt == null
                ? Convert.ToDecimal("0.00")
                : Convert.ToDecimal(txt_NetAmt.Text));
            drUpdating["TaxAmt"] = (txt_TaxAmt.Text == null
                ? Convert.ToDecimal("0.00")
                : Convert.ToDecimal(txt_TaxAmt.Text));
            drUpdating["TotalAmt"] = (lbl_TotalAmtDt.Text == string.Empty
                ? Convert.ToDecimal("0.00")
                : Convert.ToDecimal(lbl_TotalAmtDt.Text));
            drUpdating["FOCQty"] = (se_FocEdit.Text == null
                ? Convert.ToDecimal("0.00")
                : Convert.ToDecimal(se_FocEdit.Text));
            //drUpdating["NetDrAcc"] = txt_NetDrAcc.Text;
            //drUpdating["TaxDrAcc"] = txt_TaxDrAcc.Text;
            //drUpdating["NetDrAcc"] = ddl_NetDrAcc.Text;
            //drUpdating["TaxDrAcc"] = ddl_TaxDrAcc.Text;
            drUpdating["Comment"] = txt_DtrComment.Text;

            // Added on: 24/08/2017, By: Fon
            drUpdating["CurrNetAmt"] = txt_CurrNetAmt.Text;
            drUpdating["CurrDiscAmt"] = txt_CurrDiscAmt.Text;
            drUpdating["CurrTaxAmt"] = txt_CurrTaxAmt.Text;
            drUpdating["CurrTotalAmt"] = lbl_CurrTotalAmtDt.Text;
            // End Added.
            if (drUpdating.Table.Columns.Contains("ExtraCost"))
            {

                if (de_ExpiryDate.Text == string.Empty)
                    drUpdating["ExpiryDate"] = DBNull.Value;
                else
                    drUpdating["ExpiryDate"] = string.Format("{0:yyyy-MM-dd}", de_ExpiryDate.Date);

                drUpdating["ExtraCost"] = (lbl_ExtraCost.Text == string.Empty
                    ? Convert.ToDecimal("0.00")
                    : Convert.ToDecimal(lbl_ExtraCost.Text));
            }

            AllocateExtraCost();


            grd_RecEdit.DataSource = dsRecEdit.Tables[recDt.TableName];
            grd_RecEdit.EditIndex = -1;
            grd_RecEdit.DataBind();

            Session["dsRecEdit"] = dsRecEdit;

            // When edit mode, changes neede to keep to session.
            if (Request.Params["MODE"] != null)
            {
                if (Request.Params["MODE"].ToUpper() == "EDIT")
                {
                    Session["dsSave"] = dsRecEdit;
                }
            }

            e.Cancel = true;
            ddl_Currency.Enabled = true;
            txt_ExRateAu.Enabled = true;

            if (MODE.ToUpper() == "EDIT")
            {
                btn_AddPo.Enabled = true;
            }
            else
            {
                //Set ViewState to false on 04/04/2012
                btn_AddPo.Enabled = false;
            }

        }

        protected void grd_RecEdit_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grd_RecEdit.DataSource = dsRecEdit.Tables[recDt.TableName];
            grd_RecEdit.EditIndex = e.NewEditIndex;
            grd_RecEdit.DataBind();

            var Img_Btn = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("Img_Btn") as ImageButton;
            Img_Btn.Visible = false;
            ddl_Currency.Enabled = false;
            txt_ExRateAu.Enabled = false;

            RecEditMode = "EDIT";
        }

        protected void grd_RecEdit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (MODE.ToUpper() == "NEW")
            {
                if (RecEditMode == "NEW")
                {
                    dsRecEdit.Tables[recDt.TableName].Rows[dsRecEdit.Tables[recDt.TableName].Rows.Count - 1].Delete();
                }

                if (RecEditMode == "EDIT")
                {
                    //dsRecEdit = (DataSet)Session["dsRecEdit"];

                    dsRecEdit.Tables[recDt.TableName].Rows[dsRecEdit.Tables[recDt.TableName].Rows.Count - 1].CancelEdit();
                }
            }

            if (MODE.ToUpper() == "EDIT")
            {
                if (RecEditMode == "NEW")
                {
                    dsRecEdit.Tables[recDt.TableName].Rows[dsRecEdit.Tables[recDt.TableName].Rows.Count - 1].Delete();
                }

                if (RecEditMode == "EDIT")
                {
                    dsRecEdit.Tables[recDt.TableName].Rows[dsRecEdit.Tables[recDt.TableName].Rows.Count - 1].CancelEdit();
                }
            }

            grd_RecEdit.DataSource = dsRecEdit.Tables[recDt.TableName];
            grd_RecEdit.EditIndex = -1;
            grd_RecEdit.DataBind();

            if (MODE.ToUpper() == "EDIT")
            {
                btn_AddPo.Enabled = true;
            }
            else
            {
                //Set ViewState to false on 04/04/2012
                btn_AddPo.Enabled = false;
            }

            RecEditMode = string.Empty;
            ddl_Currency.Enabled = true;
            txt_ExRateAu.Enabled = true;

            //Session["dsRecEdit"] = dsRecEdit;
        }

        #endregion

        #region "Calculation"

        /// <summary>
        ///     Get exchange rate
        /// </summary>
        /// <returns></returns>
        //private decimal GetExchangeRate()
        //{
        //    decimal exchangeRate = 0;
        //    var dtAuditExRate = exRate.Get(lbl_Currency.Text, AuditCurrencyCode, de_RecDate.Date, hf_ConnStr.Value);

        //    if (dtAuditExRate != null)
        //    {
        //        if (dtAuditExRate.Rows.Count > 0)
        //        {
        //            exchangeRate = Convert.ToDecimal(dtAuditExRate.Rows[0][ExRateType].ToString());
        //        }
        //        else
        //        {
        //            exchangeRate = Convert.ToDecimal("0.00000");
        //        }
        //    }
        //    else
        //    {
        //        exchangeRate = Convert.ToDecimal("0.00000");
        //    }

        //    return exchangeRate;
        //}

        /// <summary>
        /// </summary>
        /// <returns></returns>
        //private int GetMaxRecDtNo(string recNo)
        //{
        //    int intChkNo;
        //    int intBaseNo;
        //    intBaseNo = recDt.GetMaxPoDtNo(recNo, hf_ConnStr.Value);

        //    if (dsRecEdit.Tables[recDt.TableName] != null)
        //    {
        //        if (dsRecEdit.Tables[recDt.TableName].Rows.Count > 0)
        //        {
        //            var row = dsRecEdit.Tables[recDt.TableName].Rows.Count;

        //            intChkNo = int.Parse(dsRecEdit.Tables[recDt.TableName].Rows[row - 1]["RecDtNo"].ToString());

        //            if (intBaseNo > intChkNo)
        //            {
        //                intRecDtNo = intBaseNo + 1;
        //            }
        //            else
        //            {
        //                intRecDtNo = intChkNo + 1;
        //            }
        //        }
        //        else if (dsRecEdit.Tables[recDt.TableName].Rows.Count == 0)
        //        {
        //            intRecDtNo = intBaseNo + 1;
        //        }
        //    }

        //    return intRecDtNo;
        //}

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


        protected void txt_NetAmt_TextChanged(object sender, EventArgs e)
        {
            //ASPxSpinEdit se_NetAmt  = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_NetAmt") as ASPxSpinEdit;
            //ASPxSpinEdit se_TaxAmt  = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_TaxAmt") as ASPxSpinEdit;

            //var lbl_TotalAmt = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_TotalAmt") as Label;
            //var txt_NetAmt = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_NetAmt") as TextBox;
            //var txt_TaxAmt = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_TaxAmt") as TextBox;

            //lbl_TotalAmt.Text = (Convert.ToDecimal(txt_NetAmt.Text) + Convert.ToDecimal(txt_TaxAmt.Text)).ToString();
        }


        //private void CalculateTotalAmtForDiscount(int rowIndex)
        //{
        //    var txt_CurrNetAmt = grd_RecEdit.Rows[rowIndex].FindControl("txt_CurrNetAmt") as TextBox;
        //    var txt_NetAmt = grd_RecEdit.Rows[rowIndex].FindControl("txt_NetAmt") as TextBox;

        //    var txt_Disc = grd_RecEdit.Rows[rowIndex].FindControl("txt_Disc") as TextBox;
        //    var txt_CurrDiscAmt = grd_RecEdit.Rows[rowIndex].FindControl("txt_CurrDiscAmt") as TextBox;
        //    var txt_DiscAmt = grd_RecEdit.Rows[rowIndex].FindControl("txt_DiscAmt") as TextBox;
        //    var lblNm_DiscAmt = grd_RecEdit.Rows[rowIndex].FindControl("lblNm_DiscAmt") as Label;


        //    var txt_CurrTaxAmt = grd_RecEdit.Rows[rowIndex].FindControl("txt_CurrTaxAmt") as TextBox;
        //    var txt_TaxAmt = grd_RecEdit.Rows[rowIndex].FindControl("txt_TaxAmt") as TextBox;

        //    var lbl_CurrTotalAmtDt = grd_RecEdit.Rows[rowIndex].FindControl("lbl_CurrTotalAmtDt") as Label;
        //    var lbl_TotalAmtDt = grd_RecEdit.Rows[rowIndex].FindControl("lbl_TotalAmtDt") as Label;
        //    var lbl_TotalAmt = grd_RecEdit.Rows[rowIndex].FindControl("lbl_TotalAmt") as Label;

        //    decimal currRate = Convert.ToDecimal(txt_ExRateAu.Text);

        //    decimal currTotalAmt = Convert.ToDecimal(txt_CurrNetAmt.Text) - Convert.ToDecimal(txt_CurrDiscAmt.Text) + Convert.ToDecimal(txt_CurrTaxAmt.Text);
        //    decimal totalAmt = Math.Round(currTotalAmt * currRate, 2);

        //    lbl_CurrTotalAmtDt.Text = string.Format("{0:N}", currTotalAmt);
        //    lblNm_DiscAmt.Text = string.Format("({0:N})", Convert.ToDecimal(txt_CurrDiscAmt.Text));

        //    lbl_TotalAmtDt.Text = string.Format("{0:N}", totalAmt);
        //    lbl_TotalAmt.Text = lbl_CurrTotalAmtDt.Text;
        //}

        protected void txt_Disc_TextChanged(object sender, EventArgs e)
        {
            CalculationForValueChanged();
        }

        protected void txt_CurrDiscAmt_TextChanged(object sender, EventArgs e)
        {
            CalculationForValueChanged(true, false);
        }


        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txt_DiscAmt_TextChanged(object sender, EventArgs e)
        {
            // Calcualte at txt_CurrDiscAmt_TextChange();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txt_TaxRate_TextChanged(object sender, EventArgs e)
        {
            CalculationForValueChanged();
        }
        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txt_TaxAmt_TextChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Calculate quantitydeviation
        /// </summary>
        /// <returns></returns>
        private decimal GetQuantityDeviation(int rowIndex)
        {
            string productCode = dsRecEdit.Tables[recDt.TableName].Rows[rowIndex]["ProductCode"].ToString();
            decimal orderQty = Convert.ToDecimal(dsRecEdit.Tables[recDt.TableName].Rows[rowIndex]["OrderQty"]);
            decimal quantityDeviation = Convert.ToDecimal(dsRecEdit.Tables[recDt.TableName].Rows[rowIndex]["RecQty"]);


            DataTable dtProduct = product.GetProdList(productCode, hf_ConnStr.Value);
            if (dtProduct.Rows.Count > 0)
            {
                decimal rate = Convert.ToDecimal(dtProduct.Rows[0]["QuantityDeviation"]);
                if (rate != 0)
                {
                    quantityDeviation = orderQty + (orderQty * rate / 100);
                }
            }
            //var ordQty =
            //    prodUnit.GetQtyAfterChangeUnit(
            //        dsRecEdit.Tables[recDt.TableName].Rows[rowindex]["ProductCode"].ToString(),
            //        dsRecEdit.Tables[recDt.TableName].Rows[rowindex]["RcvUnit"].ToString(),
            //        dsRecEdit.Tables[recDt.TableName].Rows[rowindex]["UnitCode"].ToString(),
            //        Convert.ToDecimal(dsRecEdit.Tables[recDt.TableName].Rows[rowindex]["OrderQty"]), hf_ConnStr.Value);

            //if ((dsRecEdit.Tables[recDt.TableName] != null) && (dsRecEdit.Tables[recDt.TableName].Rows.Count > 0))
            //{
            //    product.GetList(dsInventory, dsRecEdit.Tables[recDt.TableName].Rows[rowindex]["ProductCode"].ToString(),
            //        hf_ConnStr.Value);

            //    if ((dsInventory.Tables[product.TableName] != null) &&
            //        (dsInventory.Tables[product.TableName].Rows.Count > 0))
            //    {
            //        quantityDeviation =
            //            Convert.ToDecimal(ordQty +
            //                              ordQty *
            //                              (Convert.ToDecimal(
            //                                  dsInventory.Tables[product.TableName].Rows[0]["QuantityDeviation"]) / 100));
            //    }
            //}
            return quantityDeviation;
        }

        protected void se_RecQtyEdit_OnNumberChanged(object sender, EventArgs e)
        {
            //CalculateV2(grd_RecEdit.EditIndex);

            int editIndex = grd_RecEdit.EditIndex;
            var se_RecQtyEdit = grd_RecEdit.Rows[editIndex].FindControl("se_RecQtyEdit") as ASPxSpinEdit;
            var ddl_RcvUnit = grd_RecEdit.Rows[editIndex].FindControl("ddl_RcvUnit") as ASPxComboBox;
            decimal qty = Convert.ToDecimal(se_RecQtyEdit.Text);
            decimal qtyDeviation = GetQuantityDeviation(editIndex);

            if (qty < 0)
            {
                lbl_WarningOth.Text = "Quantity is less than zero.";
                pop_Warning.ShowOnPageLoad = true;
                se_RecQtyEdit.Value = Convert.ToDecimal(dsRecEdit.Tables[recDt.TableName].Rows[editIndex]["RecQty"]);
            }


            else if (qty > qtyDeviation)
            {
                lbl_WarningOth.Text = string.Format("Quantity is exceed than the value of quantity deviation. Maximum allowing is {0} {1}.", string.Format(DefaultQtyFmt, qtyDeviation), ddl_RcvUnit.Text);
                pop_Warning.ShowOnPageLoad = true;

                se_RecQtyEdit.Value = qtyDeviation;
            }

            CalculationForValueChanged();
        }

        private decimal GetPriceDeviation(int rowIndex)
        {
            string productCode = dsRecEdit.Tables[recDt.TableName].Rows[rowIndex]["ProductCode"].ToString();
            string poNo = dsRecEdit.Tables[recDt.TableName].Rows[rowIndex]["PoNo"].ToString();
            string poDtNo = dsRecEdit.Tables[recDt.TableName].Rows[rowIndex]["PoDtNo"].ToString();
            decimal priceDeviation = Convert.ToDecimal(dsRecEdit.Tables[recDt.TableName].Rows[rowIndex]["Price"]);


            DataTable dtProduct = product.GetProdList(productCode, hf_ConnStr.Value);
            if (dtProduct.Rows.Count > 0)
            {
                decimal rate = Convert.ToDecimal(dtProduct.Rows[0]["PriceDeviation"]);
                if (rate != 0)
                {
                    DataSet dsPoDt = new DataSet();
                    string message = string.Empty;

                    bool isFound = poDt.GetPoDtByPoNo(dsPoDt, ref message, poNo, hf_ConnStr.Value);
                    if (isFound)
                    {
                        DataRow[] dr = dsPoDt.Tables[0].Select(string.Format("PoDt = {0}", poDtNo));

                        if (dr.Length > 0)
                        {
                            decimal poPrice = Convert.ToDecimal(dr[0]["Price"]);

                            priceDeviation = poPrice + (poPrice * rate / 100);


                        }
                    }
                }
            }

            //var se_PriceEdit = grd_RecEdit.Rows[rowindex].FindControl("se_PriceEdit") as ASPxSpinEdit;
            //var ddl_RcvUnit = grd_RecEdit.Rows[rowindex].FindControl("ddl_RcvUnit") as ASPxComboBox;

            //string productCode = dsRecEdit.Tables[recDt.TableName].Rows[rowindex]["ProductCode"].ToString();
            //string unitCode = dsRecEdit.Tables[recDt.TableName].Rows[rowindex]["UnitCode"].ToString();

            //decimal price = 0;

            //if (prodUnit.GetConvRate(productCode, unitCode, hf_ConnStr.Value) != 0)
            //{
            //    price = (Convert.ToDecimal(se_PriceEdit.Text) *
            //             prodUnit.GetConvRate(productCode, ddl_RcvUnit.Value.ToString(), hf_ConnStr.Value)) /
            //            prodUnit.GetConvRate(productCode, unitCode, hf_ConnStr.Value);
            //}

            //if ((dsRecEdit.Tables[recDt.TableName] != null) && (dsRecEdit.Tables[recDt.TableName].Rows.Count > 0))
            //{
            //    product.GetList(dsInventory, productCode, hf_ConnStr.Value);

            //    if ((dsInventory.Tables[product.TableName] != null) &&
            //        (dsInventory.Tables[product.TableName].Rows.Count > 0))
            //    {
            //        decimal deviationRate = Convert.ToDecimal(dsInventory.Tables[product.TableName].Rows[0]["PriceDeviation"]);
            //        priceDeviation = price + price * ( deviationRate/ 100);
            //    }
            //}

            return priceDeviation;
        }

        protected void se_PriceEdit_OnNumberChanged(object sender, EventArgs e)
        {
            //CalculateV2(grd_RecEdit.EditIndex);

            int editIndex = grd_RecEdit.EditIndex;
            var se_PriceEdit = grd_RecEdit.Rows[editIndex].FindControl("se_PriceEdit") as ASPxSpinEdit;
            decimal price = Convert.ToDecimal(se_PriceEdit.Text);
            decimal priceDeviation = GetPriceDeviation(editIndex);

            //txt_InvNo.Text = priceDeviation.ToString();

            if (price < 0)
            {
                lbl_WarningOth.Text = "Price is lower than zero";
                pop_Warning.ShowOnPageLoad = true;

                se_PriceEdit.Value = Convert.ToDecimal(dsRecEdit.Tables[recDt.TableName].Rows[editIndex]["Price"]);
            }

            if (price > priceDeviation)
            {
                lbl_WarningOth.Text = string.Format("Price is exceed than the value of price deviation.  Maximum allowing is {0}.", string.Format(DefaultAmtFmt, priceDeviation));
                pop_Warning.ShowOnPageLoad = true;
                se_PriceEdit.Value = priceDeviation;
            }

            CalculationForValueChanged();
        }


        protected void btn_AllocateExtraCost_Click(object sender, EventArgs e)
        {
            AllocateExtraCost();
            grd_RecEdit.DataSource = dsRecEdit.Tables[recDt.TableName];
            grd_RecEdit.DataBind();

        }

        protected void btn_ExtraCostDetail_Click(object sender, EventArgs e)
        {
            // Bind dll_ExtrCost_Item
            DataTable dt = rec.DbExecuteQuery("SELECT * FROM PC.ExtCostType", null, hf_ConnStr.Value);
            ddl_ExtraCost_Item.DataValueField = "TypeId";
            ddl_ExtraCost_Item.DataTextField = "TypeName";
            ddl_ExtraCost_Item.DataSource = dt;
            ddl_ExtraCost_Item.DataBind();


            grd_ExtraCost.DataSource = dsRecEdit.Tables[recExtCost];
            grd_ExtraCost.DataBind();

            //dtRecExtCost.Clear();
            dtRecExtCost = null;
            if (dsRecEdit.Tables[recExtCost] != null)
                dtRecExtCost = dsRecEdit.Tables[recExtCost].Copy();


            pop_ExtraCostDetail.ShowOnPageLoad = true;

        }


        protected void btn_SaveExtraCost_Pop_ExtraCostDetail(object sender, EventArgs e)
        {
            object sumAmount;
            sumAmount = dsRecEdit.Tables[recExtCost].Compute("Sum(Amount)", string.Empty);

            se_TotalExtraCost.Text = string.IsNullOrEmpty(sumAmount.ToString()) ? "0.00" : sumAmount.ToString();

            btn_AllocateExtraCost_Click(sender, e);

            pop_ExtraCostDetail.ShowOnPageLoad = false;

        }

        protected void btn_CancelExtraCost_Pop_ExtraCostDetail(object sender, EventArgs e)
        {
            if (dtRecExtCost != null)
            {
                dsRecEdit.Tables.Remove(recExtCost);
                DataTable dt = new DataTable();
                dt.TableName = recExtCost;
                dt = dtRecExtCost.Copy();
                dsRecEdit.Tables.Add(dt);
            }
            pop_ExtraCostDetail.ShowOnPageLoad = false;
        }

        private string GetExtraCostTypeName(int id)
        {
            DataTable dt = rec.DbExecuteQuery(string.Format("SELECT TypeName FROM PC.ExtCostType WHERE TypeId = {0}", id), null, hf_ConnStr.Value);
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["TypeName"].ToString();
            else
                return string.Empty;
        }

        protected void btn_Add_Pop_ExtraCostDetail_Click(object sender, EventArgs e)
        {
            if (ddl_ExtraCost_Item.SelectedIndex > -1 && Convert.ToDecimal(se_ExtraCost_Amount.Value) > 0)
            {
                //if (lastExtDtNo == null)
                //    lastExtDtNo = 1;
                //else
                lastExtDtNo += 1;

                //if (dsRecEdit.Tables[recExtCost] != null)
                {
                    var dr = dsRecEdit.Tables[recExtCost].NewRow();
                    dr["RecNo"] = Request.QueryString["ID"] == null ? string.Empty : Request.QueryString["ID"].ToString();
                    dr["DtNo"] = lastExtDtNo;
                    dr["TypeId"] = ddl_ExtraCost_Item.SelectedItem.Value;
                    dr["Amount"] = Convert.ToDecimal(se_ExtraCost_Amount.Value);

                    dsRecEdit.Tables[recExtCost].Rows.Add(dr);

                    Session["dsRecEdit"] = dsRecEdit;
                }
                grd_ExtraCost.DataSource = dsRecEdit.Tables[recExtCost];
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

                    DataTable dt = rec.DbExecuteQuery("SELECT * FROM PC.ExtCostType", null, hf_ConnStr.Value);
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
                }

            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (e.Row.FindControl("lbl_Amount") != null)
                {
                    object sumAmount;
                    sumAmount = dsRecEdit.Tables[recExtCost].Compute("Sum(Amount)", string.Empty);

                    var obj = e.Row.FindControl("lbl_Amount") as Label;
                    obj.Text = string.Format(DefaultAmtFmt, string.IsNullOrEmpty(sumAmount.ToString()) ? 0 : Convert.ToDecimal(sumAmount.ToString()));
                }
            }
        }

        protected void grd_ExtraCost_RowEditing(Object sender, GridViewEditEventArgs e)
        {

            (sender as GridView).EditIndex = e.NewEditIndex;
            (sender as GridView).DataSource = dsRecEdit.Tables[recExtCost];
            (sender as GridView).DataBind();

            btn_SaveExtraCost.Visible = false;

        }

        protected void grd_ExtraCost_RowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
        {
            (sender as GridView).EditIndex = -1;
            (sender as GridView).DataSource = dsRecEdit.Tables[recExtCost];
            (sender as GridView).DataBind();

            btn_SaveExtraCost.Visible = true;
        }

        protected void grd_ExtraCost_RowUpdating(Object sender, GridViewUpdateEventArgs e)
        {
            var hf_DtNo = grd_ExtraCost.Rows[e.RowIndex].FindControl("hf_DtNo") as HiddenField;
            var ddl_Item = grd_ExtraCost.Rows[e.RowIndex].FindControl("ddl_Item") as DropDownList;
            var se_Amount = grd_ExtraCost.Rows[e.RowIndex].FindControl("se_Amount") as ASPxSpinEdit;


            DataTable dt = dsRecEdit.Tables[recExtCost];
            DataRow row = dt.Select("DtNo = " + hf_DtNo.Value.ToString())[0];

            int editIndex = dt.Rows.IndexOf(row);


            DataRow dr = dt.Rows[editIndex];

            dr["TypeId"] = ddl_Item.SelectedItem.Value;
            dr["Amount"] = se_Amount.Value;

            (sender as GridView).EditIndex = -1;
            (sender as GridView).DataSource = dsRecEdit.Tables[recExtCost];
            (sender as GridView).DataBind();

            btn_SaveExtraCost.Visible = true;

        }

        protected void grd_ExtraCost_RowDeleting(Object sender, GridViewDeleteEventArgs e)
        {
            var hf_DtNo = grd_ExtraCost.Rows[e.RowIndex].FindControl("hf_DtNo") as HiddenField;

            //dsRecEdit.Tables[recExtCost].Select("DtNo = " + hf_DtNo.Value.ToString())[0].Delete();
            var rows = dsRecEdit.Tables[recExtCost].Select("DtNo = " + hf_DtNo.Value.ToString());
            foreach (var row in rows)
                row.Delete();

            (sender as GridView).EditIndex = -1;
            (sender as GridView).DataSource = dsRecEdit.Tables[recExtCost];
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
                    sum += (Convert.ToDecimal(dr["FocQty"]) + Convert.ToDecimal(dr["RecQty"])) * Convert.ToDecimal(dr["Rate"]);
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
            DataTable dt = dsRecEdit.Tables[recDt.TableName];

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
                                qty = (Convert.ToDecimal(dr["FOCQty"]) + Convert.ToDecimal(dr["RecQty"])) * Convert.ToDecimal(dr["Rate"]);
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
                Session["dsRecEdit"] = dsRecEdit;
            }

        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chk_TaxAdj_CheckedChanged(object sender, EventArgs e)
        {

            var chkTaxAdj = sender as CheckBox;

            //var txtNetAmt = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_NetAmt") as TextBox;
            //var txtTaxAmt = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_TaxAmt") as TextBox;
            var hfTaxType = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("hf_TaxType") as HiddenField;
            var hfTaxRate = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("hf_TaxRate") as HiddenField;
            var ddlTaxType = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("ddl_TaxType") as DropDownList;
            var txtTaxRate = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_TaxRate") as TextBox;
            var txtCurrTaxAmt = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_CurrTaxAmt") as TextBox;

            ddlTaxType.Enabled = chkTaxAdj.Checked;
            txtTaxRate.Enabled = chkTaxAdj.Checked;
            txtCurrTaxAmt.Enabled = chkTaxAdj.Checked;

            //CheckBox chk_TaxAdj = (CheckBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("chk_TaxAdj");
            if (!chkTaxAdj.Checked)
            {
                ddlTaxType.SelectedValue = hfTaxType.Value.ToString();
                txtTaxRate.Text = hfTaxRate.Value.ToString();

                //CalculateCost(grd_RecEdit.EditIndex);
                CalculationForValueChanged();
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void cmb_TaxTypeEdit_Init(object sender, EventArgs e)
        //{
        //    cmb_TaxTypeEdit = (ASPxComboBox)sender;
        //    GridViewEditItemTemplateContainer container = cmb_TaxTypeEdit.NamingContainer as GridViewEditItemTemplateContainer;
        //    cmb_TaxTypeEdit.Value = DataBinder.Eval(container.DataItem, "TaxType").ToString();
        //}
        protected void chk_DiscAdj_CheckedChanged(object sender, EventArgs e)
        {
            var chk_DiscAdj = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("chk_DiscAdj") as CheckBox;
            var txt_Disc = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_Disc") as TextBox;
            var hf_DiscRate = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("hf_DiscRate") as HiddenField;
            var txt_CurrDiscAmt = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_CurrDiscAmt") as TextBox;
            var txt_DiscAmt = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_DiscAmt") as TextBox;

            txt_Disc.Enabled = chk_DiscAdj.Checked;
            txt_CurrDiscAmt.Enabled = chk_DiscAdj.Checked;

            // return origin value if uncheck
            if (!chk_DiscAdj.Checked)
            {
                txt_Disc.Text = hf_DiscRate.Value.ToString();
                CalculationForValueChanged();

            }



            //// Modified on: 24/08/2017, By: Fon
            //var chkDiscAdj = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("chk_DiscAdj") as CheckBox;
            //var txtDisc = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_Disc") as TextBox;
            //var txtDiscAmt = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_DiscAmt") as TextBox;
            //var txt_CurrDiscAmt = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_CurrDiscAmt") as TextBox;

            //if (chkDiscAdj != null && chkDiscAdj.Checked)
            //{
            //    // Comment on: 01/02/2018, By: Fon, For: Following from P' Oat guide.
            //    //if (txtDiscAmt != null) txtDiscAmt.Enabled = true;
            //    // End Comment.

            //    if (txtDisc != null) txtDisc.Enabled = true;
            //    if (txt_CurrDiscAmt != null) txt_CurrDiscAmt.Enabled = true;
            //}
            //else
            //{
            //    if (txtDiscAmt != null)
            //    {
            //        txtDiscAmt.Enabled = false;
            //        txtDiscAmt.Text = string.Format("{0:N}", 0);

            //        txt_CurrDiscAmt.Enabled = false;
            //        txt_CurrDiscAmt.Text = string.Format("{0:N}", 0);

            //        if (txtDisc != null)
            //        {
            //            txtDisc.Enabled = false;
            //            txtDisc.Text = string.Format("{0:N}", 0);
            //        }
            //    }
            //}

            //CalculateCost(grd_RecEdit.EditIndex);

        }

        /// <summary>
        ///     de_RecDate.Date
        /// </summary>
        private void AssignExchangeRate()
        {
            // Modified on: 25/08/2017, By: Fon
            //var dtAuditExRate = exRate.Get(lbl_Currency.Text, AuditCurrencyCode,
            //    DateTime.Parse(dsRecEdit.Tables[rec.TableName].Rows[0]["RecDate"].ToString()), hf_ConnStr.Value);
            var dtAuditExRate = exRate.Get(ddl_Currency.Value.ToString(), AuditCurrencyCode,
                DateTime.Parse(dsRecEdit.Tables[rec.TableName].Rows[0]["RecDate"].ToString()), hf_ConnStr.Value);
            // End Modified.

            if (dtAuditExRate != null)
            {
                if (dtAuditExRate.Rows.Count > 0)
                {
                    txt_ExRateAu.Text = String.Format("{0:D5}", dtAuditExRate.Rows[0][ExRateType]);
                }
                else
                {
                    txt_ExRateAu.Text = "0.00000";
                }
            }
            else
            {
                txt_ExRateAu.Text = "0.00000";
            }

            // Mangement            de_RecDate.Date            
            var dtMngExRate = exRate.Get(ddl_Currency.Value.ToString(), MngCurrencyCode,
                DateTime.Parse(dsRecEdit.Tables[rec.TableName].Rows[0]["RecDate"].ToString()), hf_ConnStr.Value);

            if (dtMngExRate != null)
            {
                if (dtMngExRate.Rows.Count > 0)
                {
                    //txt_ExRateMng.Text = dtMngExRate.Rows[0][this.ExRateType].ToString();
                }
            }
        }

        private void UpdateInventoryForCommit(DataRow drRecDt)
        {
            recFunc.SetConnectionString(hf_ConnStr.Value);
            recFunc.UpdateInventoryForCommit(dsSave, drRecDt);

            // Remark for using shared funciton. (RecEdit, RecCommitByBatch, RecCreateManual)

            ////FIFO Calculation
            //var invQty = prodUnit.GetQtyAfterChangeUnit(drRecDt["ProductCode"].ToString(),
            //    prodUnit.GetInvenUnit(drRecDt["ProductCode"].ToString(), hf_ConnStr.Value),
            //    drRecDt["RcvUnit"].ToString(),
            //    decimal.Parse(drRecDt["RecQty"].ToString()) + decimal.Parse(drRecDt["FOCQty"].ToString()),
            //    hf_ConnStr.Value);

            //var netPrice = Math.Round(Convert.ToDecimal(drRecDt["NetAmt"].ToString()), 2);
            //var pricePerQty = Math.Round((netPrice / (invQty == 0 ? 1 : invQty)), 2);
            //var diffCount = Math.Round((netPrice - (pricePerQty * (invQty == 0 ? 1 : invQty))), 2) * 100;

            //if (diffCount == 0)
            //{
            //    var drInv = dsSave.Tables[inv.TableName].NewRow();

            //    drInv["HdrNo"] = drRecDt["RecNo"].ToString();
            //    drInv["DtNo"] = drRecDt["RecDtNo"].ToString();
            //    drInv["InvNo"] = 1;
            //    drInv["ProductCode"] = drRecDt["ProductCode"].ToString();
            //    drInv["Location"] = drRecDt["LocationCode"].ToString();
            //    drInv["IN"] = invQty;
            //    drInv["OUT"] = Convert.ToDecimal("0.00");
            //    drInv["Amount"] = pricePerQty;
            //    drInv["FIFOAudit"] = pricePerQty;
            //    drInv["FIFOMng"] = System.DBNull.Value;
            //    drInv["FIFOBank"] = System.DBNull.Value;
            //    drInv["MAvgAudit"] = System.DBNull.Value;
            //    drInv["MAvgMng"] = System.DBNull.Value;
            //    drInv["MAvgBank"] = System.DBNull.Value;
            //    drInv["PAvgAudit"] = 0; // Move the average cost calculation to after completed save Inventory Ledgers 
            //    drInv["CommittedDate"] = dsSave.Tables[rec.TableName].Rows[0]["RecDate"];
            //    drInv["PAvgMng"] = System.DBNull.Value;
            //    drInv["PAvgBank"] = System.DBNull.Value;
            //    drInv["RptAudit"] = System.DBNull.Value;
            //    drInv["RptMng"] = System.DBNull.Value;
            //    drInv["RptBank"] = System.DBNull.Value;
            //    drInv["Type"] = "RC";

            //    dsSave.Tables[inv.TableName].Rows.Add(drInv);

            //    //// ขอเพิ่มเข้า fifo table
            //    //DataRow drFifo = dsFifo.Tables[Fifo.TableName].NewRow();
            //    //drFifo["HdrNo"] = drInv["HdrNo"];
            //    //drFifo["LocationCode"] = drInv["Location"];
            //    //drFifo["ProductCode"] = drInv["ProductCode"];
            //    //drFifo["RcvQty"] = drInv["IN"];
            //    //drFifo["RcvPrice"] = drInv["Amount"];
            //    //drFifo["RcvRemain"] = drInv["IN"];
            //    //drFifo["CreatedDate"] = drInv["CommittedDate"];
            //    //dsFifo.Tables[Fifo.TableName].Rows.Add(drFifo);
            //}
            //else
            //{
            //    for (var i = 0; i < 2; i++)
            //    {
            //        var drInv = dsSave.Tables[inv.TableName].NewRow();

            //        drInv["HdrNo"] = drRecDt["RecNo"].ToString();
            //        drInv["DtNo"] = (drRecDt["RecDtNo"]);
            //        drInv["InvNo"] = i + 1;
            //        drInv["ProductCode"] = drRecDt["ProductCode"].ToString();
            //        drInv["Location"] = drRecDt["LocationCode"].ToString();

            //        if (i == 0)
            //        {
            //            drInv["IN"] = invQty - Math.Abs(diffCount);
            //            drInv["FIFOAudit"] = pricePerQty;
            //            drInv["Amount"] = pricePerQty;
            //        }
            //        else
            //        {
            //            drInv["IN"] = Math.Abs(diffCount);

            //            if (diffCount < 0)
            //            {
            //                //case : QuantityPerUnit * Quantity > Net
            //                drInv["Amount"] = pricePerQty - Convert.ToDecimal("0.01");
            //                drInv["FIFOAudit"] = pricePerQty - Convert.ToDecimal("0.01");
            //            }
            //            else
            //            {
            //                //case : QuantityPerUnit * Quantity < Net
            //                drInv["Amount"] = pricePerQty + Convert.ToDecimal("0.01");
            //                drInv["FIFOAudit"] = pricePerQty + Convert.ToDecimal("0.01");
            //            }
            //        }

            //        drInv["OUT"] = Convert.ToDecimal("0.00");
            //        drInv["FIFOMng"] = System.DBNull.Value;
            //        drInv["FIFOBank"] = System.DBNull.Value;
            //        drInv["MAvgAudit"] = System.DBNull.Value;
            //        drInv["MAvgMng"] = System.DBNull.Value;
            //        drInv["MAvgBank"] = System.DBNull.Value;
            //        drInv["PAvgAudit"] = 0;
            //        drInv["CommittedDate"] = dsSave.Tables[rec.TableName].Rows[0]["RecDate"];

            //        drInv["PAvgMng"] = System.DBNull.Value;
            //        drInv["PAvgBank"] = System.DBNull.Value;
            //        drInv["RptAudit"] = System.DBNull.Value;
            //        drInv["RptMng"] = System.DBNull.Value;
            //        drInv["RptBank"] = System.DBNull.Value;
            //        drInv["Type"] = "RC";

            //        dsSave.Tables[inv.TableName].Rows.Add(drInv);

            //        //// ขอเพิ่มเข้า fifo table
            //        //DataRow drFifo = dsFifo.Tables[Fifo.TableName].NewRow();
            //        //drFifo["HdrNo"] = drInv["HdrNo"];
            //        //drFifo["LocationCode"] = drInv["Location"];
            //        //drFifo["ProductCode"] = drInv["ProductCode"];
            //        //drFifo["RcvQty"] = drInv["IN"];
            //        //drFifo["RcvPrice"] = drInv["Amount"];
            //        //drFifo["RcvRemain"] = drInv["IN"];
            //        //drFifo["CreatedDate"] = drInv["CommittedDate"];
            //        //dsFifo.Tables[Fifo.TableName].Rows.Add(drFifo);
            //    }
            //}
        }

        //private void DeleteItem()
        //{
        //    for (var i = 0; i < delValues.Count; i += 3)
        //    {
        //        po.GetListByPoNo2(dsPoUpdate, ref MsgError, delValues[i], hf_ConnStr.Value);
        //        poDt.GetPoDtByPoNo(dsPoUpdate, ref MsgError, delValues[i], hf_ConnStr.Value);

        //        foreach (DataRow drDelItem in dsPoUpdate.Tables[poDt.TableName].Rows)
        //        {
        //            if (drDelItem["PoNo"].ToString() == delValues[i] && drDelItem["PoDt"].ToString() == delValues[i + 1])
        //            {
        //                drDelItem["RcvQty"] = Convert.ToDecimal(drDelItem["RcvQty"].ToString()) -
        //                                      Convert.ToDecimal(delValues[i + 2]);
        //                break;
        //            }
        //        }

        //        dsPoUpdate.Tables[po.TableName].Rows[0]["DocStatus"] = "Partial";

        //        po.Save(dsPoUpdate, hf_ConnStr.Value);
        //        dsPoUpdate.Clear();
        //    }
        //}


        private void SaveAndCommit(string strAction)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                var MODE = Request.Params["MODE"];
                var _action = string.Empty;

                var OpenPeriod = period.GetLatestOpenEndDate(LoginInfo.ConnStr);
                var InvCommittedDate = de_RecDate.Date.Date <= OpenPeriod.Date ? OpenPeriod : DateTime.Now;
                var deliPoint = cmb_DeliPoint.Value.ToString().Split(':')[0];
                var currRate = Convert.ToDecimal(txt_ExRateAu.Text);

                //For Edit 
                if (MODE.ToUpper() == "EDIT")
                {
                    _action = "MODIFY";

                    #region
                    var drSave = dsSave.Tables[rec.TableName].Rows[0];

                    //drSave["RecNo"] = txt_RecNo.Text;
                    drSave["Description"] = txt_Desc.Text.Trim();
                    drSave["DeliPoint"] = deliPoint;
                    drSave["InvoiceNo"] = txt_InvNo.Text.Trim();
                    drSave["VendorCode"] = lbl_VendorCode.Text;
                    drSave["CurrencyCode"] = ddl_Currency.Value.ToString();
                    drSave["ExRateAudit"] = currRate;
                    drSave["CurrencyRate"] = currRate;
                    drSave["IsCashConsign"] = Convert.ToBoolean(chk_CashConsign.Checked);
                    drSave["ExportStatus"] = false;
                    drSave["UpdatedDate"] = ServerDateTime;
                    drSave["UpdatedBy"] = LoginInfo.LoginName;

                    if (strAction == "Committed")
                    {
                        bool isAVCO = config.GetValue("IN", "SYS", "COST", hf_ConnStr.Value).ToUpper() == "AVCO";

                        //drSave["CommitDate"] = isAVCO ? drSave["RecDate"] : InvCommittedDate;
                        drSave["CommitDate"] = DateTime.Now;
                        drSave["DocStatus"] = "Committed";
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

                    //drSave["TotalExtraCost"] = string.IsNullOrEmpty(se_TotalExtraCost.Text) ? 0m : Convert.ToDecimal(se_TotalExtraCost.Value);
                    drSave["TotalExtraCost"] = se_TotalExtraCost.Number;

                    if (rdb_ExtraCostByAmt.Checked)
                        drSave["ExtraCostBy"] = "A";
                    else
                        drSave["ExtraCostBy"] = "Q";

                    #endregion
                }
                else //For Create
                {
                    _action = "CREATE";

                    #region
                    rec.GetStructure(dsSave, hf_ConnStr.Value);
                    var drSaveNew = dsSave.Tables[rec.TableName].NewRow();


                    // For new
                    string recNo = rec.GetNewID(de_RecDate.Date.Date, hf_ConnStr.Value);
                    drSaveNew["RecNo"] = recNo;
                    drSaveNew["RecDate"] = de_RecDate.Date.Date;
                    drSaveNew["Description"] = txt_Desc.Text;
                    drSaveNew["DeliPoint"] = deliPoint;
                    drSaveNew["InvoiceNo"] = txt_InvNo.Text.Trim();
                    drSaveNew["VendorCode"] = lbl_VendorCode.Text.Trim();
                    drSaveNew["CurrencyCode"] = ddl_Currency.Value.ToString();
                    drSaveNew["ExRateAudit"] = currRate;
                    drSaveNew["CurrencyRate"] = currRate;

                    drSaveNew["IsCashConsign"] = Convert.ToBoolean(chk_CashConsign.Checked);
                    drSaveNew["CreatedDate"] = ServerDateTime;
                    drSaveNew["CreatedBy"] = LoginInfo.LoginName;
                    drSaveNew["UpdatedDate"] = InvCommittedDate;
                    drSaveNew["UpdatedBy"] = LoginInfo.LoginName;
                    drSaveNew["ExportStatus"] = false;
                    drSaveNew["PoSource"] = dsRecEdit.Tables[rec.TableName].Rows[0]["PoSource"];

                    if (drSaveNew.Table.Columns.Contains("TotalExtraCost"))
                        drSaveNew["TotalExtraCost"] = Convert.ToDecimal(se_TotalExtraCost.Number);

                    if (de_InvDate.Value != null)
                    {
                        drSaveNew["InvoiceDate"] = DateTime.Parse(de_InvDate.Date.ToString());
                    }
                    else
                    {
                        drSaveNew["InvoiceDate"] = DBNull.Value;
                    }

                    if (strAction == "Committed")
                    {
                        drSaveNew["DocStatus"] = "Committed";
                        //drSaveNew["CommitDate"] = ServerDateTime;
                        drSaveNew["CommitDate"] = InvCommittedDate;
                    }
                    else
                    {
                        drSaveNew["DocStatus"] = "Received";
                    }

                    if (drSaveNew.Table.Columns.Contains("TotalExtraCost"))
                        drSaveNew["TotalExtraCost"] = se_TotalExtraCost.Text == string.Empty ? 0m : Convert.ToDecimal(se_TotalExtraCost.Value.ToString());

                    dsSave.Tables[rec.TableName].Rows.Add(drSaveNew);

                    // Extra Cost
                    //foreach (DataRow dr in dsRecEdit.Tables[recExtCost].Rows)
                    //{
                    //    if (dr.RowState != DataRowState.Deleted)
                    //        dr["RecNo"] = recNo;
                    //}


                    recDt.GetStructure(dsSave, hf_ConnStr.Value);

                    if (grd_RecEdit.Rows.Count > 0)
                    {
                        foreach (DataRow drSelectedNew in dsRecEdit.Tables[recDt.TableName].Rows)
                        {
                            if (drSelectedNew.RowState != DataRowState.Deleted)
                            {
                                if ((Convert.ToDecimal(drSelectedNew["RecQty"]) > 0))
                                {
                                    // For Detail
                                    var drRecdtNew = dsSave.Tables[recDt.TableName].NewRow();

                                    drRecdtNew["RecNo"] = recNo; //rec.GetNewID(DateTime.Parse(de_RecDate.Text), hf_ConnStr.Value);
                                    drRecdtNew["RecDtNo"] = RecDtNo + 1;
                                    drRecdtNew["LocationCode"] = drSelectedNew["LocationCode"];
                                    drRecdtNew["ProductCode"] = drSelectedNew["ProductCode"];
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

                                    drRecdtNew["NetAmt"] = drSelectedNew["NetAmt"];
                                    drRecdtNew["TaxAmt"] = drSelectedNew["TaxAmt"];
                                    drRecdtNew["DiccountAmt"] = drSelectedNew["DiccountAmt"];
                                    drRecdtNew["TotalAmt"] = drSelectedNew["TotalAmt"];

                                    drRecdtNew["CurrNetAmt"] = drSelectedNew["CurrNetAmt"];
                                    drRecdtNew["CurrTaxAmt"] = drSelectedNew["CurrTaxAmt"];
                                    drRecdtNew["CurrDiscAmt"] = drSelectedNew["CurrDiscAmt"];
                                    drRecdtNew["CurrTotalAmt"] = drSelectedNew["CurrTotalAmt"];


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
                                    drRecdtNew["Status"] = (strAction == "Committed" ? "Committed" : "Received");
                                    if (drRecdtNew.Table.Columns.Contains("TotalExtraCost"))
                                    {
                                        drRecdtNew["ExpiryDate"] = drSelectedNew["ExpiryDate"];
                                        drRecdtNew["ExtraCost"] = drSelectedNew["ExtraCost"];
                                    }
                                    // Add new record
                                    dsSave.Tables[recDt.TableName].Rows.Add(drRecdtNew);

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
                    }
                    #endregion
                }

                // Update Inventory Ledger ****************************************************
                if (!string.IsNullOrEmpty(txt_RecNo.Text))
                {
                    var sql = string.Format("DELETE FROM [IN].Inventory WHERE HdrNo='{0}' AND [Type]='RC'", txt_RecNo.Text);
                    rec.DbExecuteQuery(sql, null, hf_ConnStr.Value);
                }

                if (dsSave.Tables[inv.TableName] == null)
                {
                    inv.GetListByHdrNo(dsSave, dsSave.Tables[rec.TableName].Rows[0]["RecNo"].ToString(), hf_ConnStr.Value);
                }

                if (strAction == "Committed")
                {
                    foreach (DataRow dr in dsSave.Tables[recDt.TableName].Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            dr["Status"] = "Committed";
                            UpdateInventoryForCommit(dr);
                        }
                    }
                }

                // Re-Calculation Extra Cost
                if (se_TotalExtraCost.Number > 0)
                {
                    AllocateExtraCost();

                    var dtRecDt = dsRecEdit.Tables[recDt.TableName];
                    var dtSave = dsSave.Tables[recDt.TableName];

                    for (int i = 0; i < dtRecDt.Rows.Count; i++)
                    {
                        if (dtRecDt.Rows[i].RowState != DataRowState.Deleted)
                        {
                            dtSave.Rows[i]["ExtraCost"] = dtRecDt.Rows[i]["ExtraCost"];
                        }
                    }
                }


                //Save Data to 3 tables Rec, RecDt and Inventory.
                var result = rec.Save(dsSave, hf_ConnStr.Value);

                if (result)
                {
                    #region
                    string recNo = string.Empty;

                    if (MODE == "EDIT")
                        recNo = txt_RecNo.Text;
                    else
                        recNo = dsSave.Tables[rec.TableName].Rows[0]["RecNo"].ToString();

                    // Update PC.RecExtCost
                    string sqlDel = string.Format("DELETE FROM PC.RecExtCost WHERE RecNo = '{0}';", recNo);
                    string sqlIns = string.Empty;

                    DataTable dt = dsRecEdit.Tables[recExtCost];
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
                    rec.DbExecuteQuery(sqlDel + sqlIns, null, hf_ConnStr.Value);

                    // ------------------------------


                    _transLog.Save("PC", "REC", recNo, _action, string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);

                    //Update Po.DocStatus, PoDt.RcvQty, Product.LastCost(Update only commit)
                    UpdateRelateTable(recNo);
                    // Update average cost ********************************************************
                    inv.UpdateAverageCostByDocument(recNo, LoginInfo.ConnStr);
                    // ****************************************************************************

                    if (strAction == "Committed")
                    {
                        _transLog.Save("PC", "REC", recNo, "COMMIT", string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);
                        pop_ConfirmCommit.ShowOnPageLoad = false;
                    }
                    else
                        pop_ConfirmSave.ShowOnPageLoad = false;

                    Response.Redirect("Rec.aspx?ID=" + recNo + "&BuCode=" + Request.Params["BuCode"] + "&Vid=" + Request.Params["Vid"]);
                    #endregion
                }
                else
                {
                    lbl_WarningOth.Text = "Error while saving.";
                    pop_Warning.ShowOnPageLoad = true;

                }

            } // if Page.Valid
        }


        //private void SaveAndCommit_old(string strAction)
        //{
        //    Page.Validate();
        //    if (Page.IsValid)
        //    {
        //        DateTime OpenPeriod = period.GetLatestOpenEndDate(LoginInfo.ConnStr).AddHours(23).AddMinutes(55);

        //        // DateTime OpenPeriod = GetOpenPeriod();
        //        DateTime InvCommittedDate;

        //        if (DateTime.Today > OpenPeriod)  // Over than open period (DateTime)
        //        {
        //            if (de_RecDate.Date.Date <= OpenPeriod.Date)
        //                InvCommittedDate = OpenPeriod;
        //            else
        //                InvCommittedDate = DateTime.Today;
        //        }
        //        else // In period
        //            InvCommittedDate = DateTime.Today;


        //        var MODE = Request.Params["MODE"];
        //        var value = cmb_DeliPoint.Value.ToString().Split(':');

        //        //For Edit 
        //        if (MODE.ToUpper() == "EDIT")
        //        {
        //            #region
        //            var drSave = dsSave.Tables[rec.TableName].Rows[0];

        //            drSave["RecNo"] = txt_RecNo.Text;

        //            drSave["Description"] = (txt_Desc.Text != null ? txt_Desc.Text : string.Empty);
        //            drSave["DeliPoint"] = value[0];
        //            drSave["InvoiceNo"] = (txt_InvNo.Text != null ? txt_InvNo.Text : string.Empty);
        //            drSave["VendorCode"] = lbl_VendorCode.Text;
        //            drSave["CurrencyCode"] = ddl_Currency.Value.ToString();
        //            drSave["ExRateAudit"] = Convert.ToDecimal(txt_ExRateAu.Text);
        //            // Added on: 15/08/2017, By: Fon, For: New Muti-currency.
        //            drSave["CurrencyRate"] = Convert.ToDecimal(txt_ExRateAu.Text);

        //            drSave["IsCashConsign"] = Convert.ToBoolean(chk_CashConsign.Checked);
        //            drSave["UpdatedDate"] = ServerDateTime;
        //            drSave["UpdatedBy"] = LoginInfo.LoginName;
        //            drSave["ExportStatus"] = false;

        //            if (strAction == "Committed")
        //            {
        //                drSave["DocStatus"] = "Committed";
        //                //drSave["CommitDate"] = ServerDateTime;
        //                drSave["CommitDate"] = InvCommittedDate;
        //            }
        //            else
        //            {
        //                drSave["DocStatus"] = "Received";
        //            }

        //            if (de_InvDate.Value != null)
        //            {
        //                drSave["InvoiceDate"] = DateTime.Parse(de_InvDate.Date.ToString());
        //            }
        //            else
        //            {
        //                drSave["InvoiceDate"] = DBNull.Value;
        //            }
        //            #endregion
        //        }
        //        else //For Create
        //        {
        //            #region
        //            rec.GetStructure(dsSave, hf_ConnStr.Value);
        //            var drSaveNew = dsSave.Tables[rec.TableName].NewRow();

        //            // For new
        //            drSaveNew["RecNo"] = rec.GetNewID(DateTime.Parse(de_RecDate.Text), hf_ConnStr.Value);
        //            drSaveNew["RecDate"] =
        //                DateTime.Parse(de_RecDate.Date.ToShortDateString() + " " + ServerDateTime.TimeOfDay);
        //            drSaveNew["Description"] = (txt_Desc.Text != null ? txt_Desc.Text : string.Empty);
        //            drSaveNew["DeliPoint"] = value[0];
        //            drSaveNew["InvoiceNo"] = (txt_InvNo.Text != null ? txt_InvNo.Text : string.Empty);
        //            drSaveNew["VendorCode"] = lbl_VendorCode.Text;
        //            drSaveNew["CurrencyCode"] = ddl_Currency.Value.ToString();
        //            drSaveNew["ExRateAudit"] = Convert.ToDecimal(txt_ExRateAu.Text);

        //            // Added on: 15/08/2017, By: Fon, For: New Muti-currency
        //            drSaveNew["CurrencyRate"] = Convert.ToDecimal(txt_ExRateAu.Text);

        //            drSaveNew["IsCashConsign"] = Convert.ToBoolean(chk_CashConsign.Checked);
        //            drSaveNew["CreatedDate"] = ServerDateTime;
        //            drSaveNew["CreatedBy"] = LoginInfo.LoginName;
        //            //drSaveNew["UpdatedDate"] = ServerDateTime;
        //            drSaveNew["UpdatedDate"] = InvCommittedDate;
        //            drSaveNew["UpdatedBy"] = LoginInfo.LoginName;
        //            drSaveNew["ExportStatus"] = false;
        //            drSaveNew["PoSource"] = dsRecEdit.Tables[rec.TableName].Rows[0]["PoSource"];

        //            if (de_InvDate.Value != null)
        //            {
        //                drSaveNew["InvoiceDate"] = DateTime.Parse(de_InvDate.Date.ToString());
        //            }
        //            else
        //            {
        //                drSaveNew["InvoiceDate"] = DBNull.Value;
        //            }

        //            if (strAction == "Committed")
        //            {
        //                drSaveNew["DocStatus"] = "Committed";
        //                //drSaveNew["CommitDate"] = ServerDateTime;
        //                drSaveNew["CommitDate"] = InvCommittedDate;
        //            }
        //            else
        //            {
        //                drSaveNew["DocStatus"] = "Received";
        //            }

        //            dsSave.Tables[rec.TableName].Rows.Add(drSaveNew);

        //            recDt.GetStructure(dsSave, hf_ConnStr.Value);

        //            if (grd_RecEdit.Rows.Count > 0)
        //            {
        //                foreach (DataRow drSelectedNew in dsRecEdit.Tables[recDt.TableName].Rows)
        //                {
        //                    if (drSelectedNew.RowState != DataRowState.Deleted)
        //                    {
        //                        if ((Convert.ToDecimal(drSelectedNew["RecQty"]) > 0))
        //                        {
        //                            // For Detail
        //                            var drRecdtNew = dsSave.Tables[recDt.TableName].NewRow();

        //                            drRecdtNew["RecNo"] = rec.GetNewID(DateTime.Parse(de_RecDate.Text), hf_ConnStr.Value);
        //                            drRecdtNew["RecDtNo"] = RecDtNo + 1;
        //                            drRecdtNew["LocationCode"] = drSelectedNew["LocationCode"];
        //                            drRecdtNew["ProductCode"] = drSelectedNew["ProductCode"];
        //                            drRecdtNew["UnitCode"] = drSelectedNew["UnitCode"];
        //                            drRecdtNew["OrderQty"] = Convert.ToDecimal(drSelectedNew["OrderQty"]);
        //                            drRecdtNew["FOCQty"] =
        //                                (Convert.ToDecimal(drSelectedNew["FOCQty"] == DBNull.Value
        //                                    ? 0
        //                                    : (decimal)drSelectedNew["FOCQty"]));
        //                            drRecdtNew["RecQty"] =
        //                                (Convert.ToDecimal(drSelectedNew["RecQty"] == DBNull.Value
        //                                    ? 0
        //                                    : (decimal)drSelectedNew["RecQty"]));
        //                            drRecdtNew["Price"] =
        //                                (Convert.ToDecimal(drSelectedNew["Price"] == DBNull.Value
        //                                    ? 0
        //                                    : (decimal)drSelectedNew["Price"]));
        //                            drRecdtNew["Discount"] =
        //                                (Convert.ToDecimal(drSelectedNew["Discount"] == DBNull.Value
        //                                    ? 0
        //                                    : (decimal)drSelectedNew["Discount"]));
        //                            drRecdtNew["DiccountAmt"] =
        //                                (Convert.ToDecimal(drSelectedNew["DiccountAmt"] == DBNull.Value
        //                                    ? 0
        //                                    : (decimal)drSelectedNew["DiccountAmt"]));
        //                            drRecdtNew["TaxType"] = drSelectedNew["TaxType"];
        //                            drRecdtNew["TaxRate"] = drSelectedNew["TaxRate"];
        //                            drRecdtNew["TaxAdj"] = (bool)drSelectedNew["TaxAdj"];
        //                            drRecdtNew["NetAmt"] =
        //                                (Convert.ToDecimal(drSelectedNew["NetAmt"] == DBNull.Value
        //                                    ? 0
        //                                    : (decimal)drSelectedNew["NetAmt"]));
        //                            drRecdtNew["TaxAmt"] =
        //                                (Convert.ToDecimal(drSelectedNew["TaxAmt"] == DBNull.Value
        //                                    ? 0
        //                                    : (decimal)drSelectedNew["TaxAmt"]));
        //                            drRecdtNew["TotalAmt"] =
        //                                (Convert.ToDecimal(drSelectedNew["TotalAmt"] == DBNull.Value
        //                                    ? 0
        //                                    : (decimal)drSelectedNew["TotalAmt"]));
        //                            drRecdtNew["PoNo"] = drSelectedNew["PoNo"];
        //                            drRecdtNew["PoDtNo"] = drSelectedNew["PoDtNo"];
        //                            drRecdtNew["PrNo"] = drSelectedNew["PrNo"];
        //                            drRecdtNew["NetDrAcc"] = drSelectedNew["NetDrAcc"];
        //                            drRecdtNew["TaxDrAcc"] = drSelectedNew["TaxDrAcc"];
        //                            drRecdtNew["ExportStatus"] = false;
        //                            drRecdtNew["Descen"] = drSelectedNew["Descen"];
        //                            drRecdtNew["Descll"] = drSelectedNew["Descll"];
        //                            drRecdtNew["Comment"] = drSelectedNew["Comment"];
        //                            drRecdtNew["Rate"] = drSelectedNew["Rate"];
        //                            drRecdtNew["RcvUnit"] = drSelectedNew["RcvUnit"];
        //                            drRecdtNew["Status"] = (strAction == "Committed" ? "Committed" : "Received");

        //                            // Add new record
        //                            dsSave.Tables[recDt.TableName].Rows.Add(drRecdtNew);

        //                            RecDtNo = Convert.ToInt32(drRecdtNew["RecDtNo"]);

        //                            //this.UpdateInventoryForCommit(drRecdtNew);
        //                        }
        //                        else
        //                        {
        //                            lbl_WarningDelete.Text = "Receiving is not complete";
        //                            pop_WarningDelete.ShowOnPageLoad = true;
        //                            return;
        //                        }
        //                    }
        //                }
        //            }
        //            #endregion
        //        }


        //        // Update Inventory Ledger ****************************************************
        //        if (dsSave.Tables[inv.TableName] == null)
        //        {
        //            inv.GetListByHdrNo(dsSave, dsSave.Tables[rec.TableName].Rows[0]["RecNo"].ToString(), hf_ConnStr.Value);
        //        }

        //        foreach (DataRow drDelete in dsSave.Tables[inv.TableName].Rows)
        //        {
        //            if (drDelete.RowState != DataRowState.Deleted)
        //            {
        //                drDelete.Delete();
        //            }
        //        }




        //        for (var i = dsSave.Tables[recDt.TableName].Rows.Count - 1; i >= 0; i--)
        //        {
        //            var drSelected = dsSave.Tables[recDt.TableName].Rows[i];

        //            if (drSelected.RowState != DataRowState.Deleted)
        //            {
        //                if (Convert.ToDecimal(drSelected["RecQty"]) > 0)
        //                {
        //                    if (strAction == "Committed")
        //                    {
        //                        drSelected["Status"] = "Committed";
        //                        UpdateInventoryForCommit(drSelected);
        //                    }

        //                    drSelected["Status"] = strAction;
        //                }
        //                else // if (Convert.ToDecimal(drSelected["RecQty"]) <= 0)
        //                {
        //                    drSelected.Delete();
        //                }
        //                //else
        //                //{
        //                //    lbl_WarningDelete.Text = "Receiving is not complete";
        //                //    pop_WarningDelete.ShowOnPageLoad = true;

        //                //    return;
        //                //}
        //            }  //if not deleted.
        //        }

        //        // ********************************************************************************

        //        //Save Data to 3 tables Rec, RecDt and Inventory.
        //        string recNo = string.Empty;
        //        var result = rec.Save(dsSave, hf_ConnStr.Value);
        //        if (result)
        //        #region
        //        {
        //            //Update Po.DocStatus, PoDt.RcvQty, Product.LastCost(Update only commit)
        //            UpdateRelateTable();
        //            {

        //                //CreateAccountMap(dsRecEdit, hf_ConnStr.Value);
        //                //string RecNo = string.Empty;

        //                if (MODE == "EDIT")
        //                {
        //                    recNo = txt_RecNo.Text;

        //                }
        //                else
        //                {
        //                    recNo = dsSave.Tables[rec.TableName].Rows[0]["RecNo"].ToString();
        //                }

        //                // Update average cost ********************************************************
        //                //Get StartDate and EndDate for update Avg in inventory                    
        //                //var startDate = period.GetStartDate(Convert.ToDateTime(dsSave.Tables[rec.TableName].Rows[0]["RecDate"]), hf_ConnStr.Value);
        //                //var endDate = period.GetEndDate(Convert.ToDateTime(dsSave.Tables[rec.TableName].Rows[0]["RecDate"]), hf_ConnStr.Value);

        //                //foreach (DataRow drRecDt in dsSave.Tables[recDt.TableName].Rows)
        //                //{
        //                //inv.SetPAvgAudit(startDate, endDate, drRecDt["LocationCode"].ToString(), drRecDt["ProductCode"].ToString(), hf_ConnStr.Value);
        //                //}

        //                inv.UpdateAverageCostByDocument(recNo, LoginInfo.ConnStr);
        //                // ****************************************************************************

        //                pop_ConfirmSave.ShowOnPageLoad = false;

        //                // Added on: 20/09/2017, By: Fon
        //                ClassLogTool pctool = new ClassLogTool();
        //                pctool.SaveActionLog("RC", recNo, strAction);
        //                // End Added.

        //                Response.Redirect("Rec.aspx?ID=" + recNo + "&BuCode=" + Request.Params["BuCode"] + "&Vid=" + Request.Params["Vid"]);
        //            }
        //        }
        //        #endregion

        //    } // if Page.Valid
        //}

        //private void CreateAccountMap(DataSet dsSave, string connStr)
        //{
        //foreach (DataRow item in dsSave.Tables[recDt.TableName].Rows)
        //{
        //    var p = product.GetProductCategory(item["ProductCode"].ToString(), connStr);

        //    var s = new StringBuilder().AppendFormat(
        //        @"BusinessUnitCode = '{0}' and StoreCode = '{1}'  and ItemGroupCode = '{2}' and PostType = 'AP'",
        //        Request.Params["BuCode"], item["LocationCode"], p).ToString();
        //    //s += " and ItemGroupCode = '" + p + "'";
        //    //s += " and A1 = 'RC'";

        //    var ds = new DataSet();
        //    accountMapp.GetStructure(ds, connStr);


        //    var dt = accountMapp.GetList(connStr);
        //    var drs = dt.Select(s).ToList();

        //    if (drs.Count <= 0)
        //    {
        //        var dr = ds.Tables[accountMapp.TableName].NewRow();
        //        dr["ID"] = Guid.NewGuid(); // accMapp.GetNewID(connStr);
        //        dr["BusinessUnitCode"] = Request.Params["BuCode"];
        //        dr["StoreCode"] = item["LocationCode"];
        //        //dr["ItemGroupCode"] = p;
        //        //dr["A1"] = "RC";
        //        dr["A3"] = item["NetDrAcc"];
        //        dr["A4"] = item["TaxDrAcc"];
        //        dr["PostType"] = "AP";

        //        ds.Tables[accountMapp.TableName].Rows.Add(dr);

        //        var save = accountMapp.Save(ds, connStr);

        //        if (save)
        //        {
        //            //Response.Write("SUCCESS");
        //        }
        //    }
        //}
        //}


        private void UpdateRelateTable(string recNo)
        {

            // Update to podetail and Po status.
            var MsgError = string.Empty;
            var strChkPo = string.Empty;
            var strPo = string.Empty;
            var strChkProd = string.Empty;
            var strProd = string.Empty;

            var returnPO = false;
            var returnProd = false;

            if (dsSave.Tables[rec.TableName].Rows[0]["PoSource"].ToString() != string.Empty &&
                dsSave.Tables[rec.TableName].Rows[0]["PoSource"].ToString() != LoginInfo.BuInfo.BuCode)
            {
                hf_ConnStr.Value = bu.GetConnectionString(dsSave.Tables[rec.TableName].Rows[0]["PoSource"].ToString());
            }

            foreach (DataRow drChkPoNo in dsSave.Tables[recDt.TableName].Rows)
            {
                if (drChkPoNo.RowState != DataRowState.Deleted)
                {
                    if (strPo == string.Empty)
                    {
                        strPo = "'" + drChkPoNo["PoNo"] + "'";
                        strChkPo = drChkPoNo["PoNo"].ToString();
                    }
                    else
                    {
                        if (strChkPo != drChkPoNo["PoNo"].ToString())
                        {
                            strPo += ", '" + drChkPoNo["PoNo"] + "'";
                            strChkPo = drChkPoNo["PoNo"].ToString();
                        }
                    }

                    if (strProd == string.Empty)
                    {
                        strProd = "'" + drChkPoNo["ProductCode"] + "'";
                        strChkProd = drChkPoNo["ProductCode"].ToString();
                    }
                    else
                    {
                        if (strChkProd != drChkPoNo["ProductCode"].ToString())
                        {
                            strProd += ", '" + drChkPoNo["ProductCode"] + "'";
                            strChkProd = drChkPoNo["ProductCode"].ToString();
                        }
                    }
                }

            }

            if (strChkPo != string.Empty)
            {
                po.GetList_PoNo(dsPoUpdate, ref MsgError, strPo, hf_ConnStr.Value);
                poDt.GetPoDt_PoNo(dsPoUpdate, ref MsgError, strPo, hf_ConnStr.Value);
            }

            product.Get2(dsSave, ref MsgError, strProd, hf_ConnStr.Value);

            //Retrieve Data PO and PODt for Update field.
            foreach (DataRow drSelected in dsSave.Tables[recDt.TableName].Rows)
            {
                if (drSelected.RowState != DataRowState.Deleted)
                {
                    //Case select edit than delete item detail. 
                    if (delValues.Count > 0)
                    {
                        //for (var i = 0; i < delValues.Count; i += 3)
                        for (var i = 0; i < delValues.Count; i++)
                        {
                            foreach (DataRow drDelItem in dsPoUpdate.Tables[poDt.TableName].Rows)
                            {
                                //if (drDelItem["PoNo"].ToString() == delValues[i] &&
                                //    drDelItem["PoDt"].ToString() == delValues[i + 1])
                                //{
                                //    drDelItem["RcvQty"] = Convert.ToDecimal(drDelItem["RcvQty"].ToString()) - Convert.ToDecimal(delValues[i + 2]);
                                //    break;
                                //}
                                string poNo = delValues[i].Split(',')[0];
                                string poDtNo = delValues[i].Split(',')[1];
                                string rcvQty = delValues[i].Split(',')[2];

                                if (drDelItem["PoNo"].ToString() == poNo && drDelItem["PoDt"].ToString() == poDtNo)
                                {
                                    drDelItem["RcvQty"] = Convert.ToDecimal(drDelItem["RcvQty"].ToString()) - Convert.ToDecimal(rcvQty);
                                    break;
                                }

                            }

                            dsPoUpdate.Tables[po.TableName].Rows[0]["DocStatus"] = "Partial";
                        }
                    }

                    var decSum = recDt.GetSumRecQty(drSelected["PoNo"].ToString(), drSelected["PoDtNo"].ToString(),
                        LoginInfo.ConnStr);

                    foreach (DataRow drPoDt in dsPoUpdate.Tables[poDt.TableName].Rows)
                    {
                        if (drSelected["PoNo"].Equals(drPoDt["PoNo"]) & drSelected["PoDtNo"].Equals(drPoDt["PoDt"]))
                        {
                            drPoDt["RcvQty"] = decSum;
                        }
                    }

                }
            }


            if (MODE.ToUpper() == "EDIT")
            {
                // Update PC.PoDt : RcvQty , FocQty
                for (int i = 0; i < delValues.Count; i++)
                {
                    string poNo = delValues[i].Split(',')[0];
                    string poDtNo = delValues[i].Split(',')[1];
                    string rcvQty = delValues[i].Split(',')[2];
                    string focQty = delValues[i].Split(',')[3];

                    string sql = string.Format("UPDATE PC.PoDt SET RcvQty = RcvQty-{0}, FocQty = FocQty-{1} WHERE PoNo = '{2}' AND PoDt = {3}", rcvQty, focQty, poNo, poDtNo);
                    recDt.DbExecuteQuery(sql, null, hf_ConnStr.Value);

                    // Update PC.Po : DocStatus

                    sql = string.Format("SELECT COUNT(*) AS RecordCount FROM PC.REC rec JOIN PC.RecDt recdt ON recdt.RecNo = rec.RecNo WHERE DocStatus <> 'Voided' AND recdt.PoNo = '{0}'", poNo);
                    DataTable dt = recDt.DbExecuteQuery(sql, null, hf_ConnStr.Value);
                    int recordCount = Convert.ToInt32(dt.Rows[0]["RecordCount"]);

                    if (recordCount == 0) // Not found po in receiving
                        sql = string.Format("UPDATE PC.Po SET DocStatus = 'Printed' WHERE PoNo = '{0}'", poNo);
                    else
                        sql = string.Format("UPDATE PC.Po SET DocStatus = 'Partial' WHERE PoNo = '{0}'", poNo);
                    recDt.DbExecuteQuery(sql, null, hf_ConnStr.Value);



                }
            }

            returnPO = po.Save(dsPoUpdate, hf_ConnStr.Value);
            returnProd = product.Save(dsSave, hf_ConnStr.Value);

            //Check order quantity,receive quantity and cancel quantity.
            //if receive quantity plus cancel quantitty equal to order quantity or more than order quantity,PO's document status change to 'Received'.
            DataTable dtPo = rec.DbExecuteQuery(string.Format("SELECT DISTINCT PoNo FROM PC.RECDt WHERE RecNo = '{0}'", recNo), null, LoginInfo.ConnStr);

            foreach (DataRow dr in dtPo.Rows)
            {
                string poNo = dr["PoNo"].ToString();
                // Check if there is some detail of PO that still remain OrdQty (OrdQty <> RcvQTy + CancelQty)
                string sql = "SELECT COUNT(*) as RecordCount";
                sql += " FROM PC.PoDt";
                sql += " WHERE OrdQty <> RcvQty+CancelQty";
                sql += string.Format(" AND PoNo = '{0}'", poNo);

                DataTable dt = recDt.DbExecuteQuery(sql, null, hf_ConnStr.Value);
                if (dt.Rows[0]["RecordCount"].ToString() == "0")
                    sql = string.Format("UPDATE PC.Po SET DocStatus = 'Completed' WHERE PoNo = '{0}'", poNo);
                else
                    sql = string.Format("UPDATE PC.Po SET DocStatus = 'Partial' WHERE PoNo = '{0}'", poNo);

                recDt.DbExecuteQuery(sql, null, hf_ConnStr.Value);
            }

            //decimal TotalOrdQty = 0;
            //decimal TotalRcvQty = 0;
            //decimal TotalCancelQty = 0;

            //foreach (DataRow drPo in dsPoUpdate.Tables[po.TableName].Rows)
            //{
            //    TotalOrdQty = 0;
            //    TotalRcvQty = 0;
            //    TotalCancelQty = 0;

            //    foreach (DataRow drPoDtCheck in dsPoUpdate.Tables[poDt.TableName].Rows)
            //    {
            //        if (drPoDtCheck.RowState != DataRowState.Deleted) //& drPoDtCheck.RowState == DataRowState.Modified
            //        {
            //            if (drPoDtCheck["PoNo"].ToString() == drPo["PoNo"].ToString())
            //            {
            //                TotalOrdQty += Convert.ToDecimal(drPoDtCheck["OrdQty"].ToString());
            //                TotalRcvQty += Convert.ToDecimal(drPoDtCheck["RcvQty"].ToString());
            //                TotalCancelQty += Convert.ToDecimal(drPoDtCheck["CancelQty"].ToString());
            //            }
            //        }
            //    }

            //    if (TotalOrdQty <= TotalRcvQty + TotalCancelQty)
            //    {
            //        //2013-03-01 
            //        if (drPo["DocStatus"].ToString().ToUpper() == "PARTIAL" ||
            //            drPo["DocStatus"].ToString().ToUpper() == "PRINTED")
            //        {
            //            drPo["DocStatus"] = "Completed";
            //        }
            //        else
            //        {
            //            // Case: 1 Po มี Rec หลายใบ ถ้ากด Commit Rec ใบใดใบหนึ่ง Status ของ PO = Partial
            //            // ซึ่งต้องอธิบายให้ User เข้าใจคือ ยังมีใบ PO ที่ค้างอยู่ใน Rec ใบอื่นอยู่ ถ้ากด Commit ครบ สถานะของ PO ก็จะเป็น Completed
            //            // แต่ปัญหาที่พบคือ กรณีที่ Rec กด Save ทำ PO ครบทุก Detail สถานะของ PO เป็น Partial ซึ่งต้องเป็น Completed
            //            DataTable dtRec = new DataTable();
            //            var intRecStatus = 0;
            //            var intComStatus = 0;

            //            dtRec = rec.GetListByRecStatus(drPo["PoNo"].ToString(), hf_ConnStr.Value);
            //            if (dtRec != null && dtRec.Rows.Count > 0)
            //            {
            //                foreach (DataRow drChkDocStatus in dtRec.Rows)
            //                {
            //                    if (drChkDocStatus["DocStatus"].ToString().ToUpper() == "RECEIVED")
            //                    {
            //                        intRecStatus += 1;
            //                    }
            //                    else if (drChkDocStatus["DocStatus"].ToString().ToUpper() == "COMMITTED")
            //                    {
            //                        intComStatus += 1;
            //                    }
            //                }

            //                if (intRecStatus == dtRec.Rows.Count)
            //                {
            //                    drPo["DocStatus"] = "Completed";
            //                }
            //                else
            //                {
            //                    if (intComStatus == dtRec.Rows.Count)
            //                    {
            //                        drPo["DocStatus"] = "Completed";
            //                    }
            //                    else
            //                    {
            //                        drPo["DocStatus"] = "Partial";
            //                    }
            //                }
            //            }

            //        }
            //    }
            //    else
            //    {
            //        drPo["DocStatus"] = "Partial";
            //    }
            //}


        }

        #endregion

        #region "Button Process"

        #region "GridView"

        protected void grd_PoList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_VendorName") != null)
                {
                    var lbl_VendorName = e.Row.FindControl("lbl_VendorName") as Label;
                    lbl_VendorName.Text = DataBinder.Eval(e.Row.DataItem, "VendorCode") + " : " +
                                          DataBinder.Eval(e.Row.DataItem, "VendorName");
                }

                if (e.Row.FindControl("lbl_DeliveryDate") != null)
                {
                    var lbl_DeliveryDate = e.Row.FindControl("lbl_DeliveryDate") as Label;
                    lbl_DeliveryDate.Text = DataBinder.Eval(e.Row.DataItem, "DeliveryDate").ToString();
                }

                if (e.Row.FindControl("lbl_PoNo") != null)
                {
                    var lbl_PoNo = e.Row.FindControl("lbl_PoNo") as Label;
                    lbl_PoNo.Text = DataBinder.Eval(e.Row.DataItem, "PoNo").ToString();
                }

                if (e.Row.FindControl("lbl_Status") != null)
                {
                    var lbl_Status = e.Row.FindControl("lbl_Status") as Label;
                    lbl_Status.Text = DataBinder.Eval(e.Row.DataItem, "DocStatus").ToString();
                }
            }
        }

        #endregion


        protected void btn_Back_Click(object sender, EventArgs e)
        {
            var MODE = Request.QueryString["MODE"];


            if (MODE.ToUpper() == "EDIT")
            {
                Response.Redirect("Rec.aspx?ID=" + txt_RecNo.Text + "&BuCode=" + Request.Params["BuCode"] + "&Vid=" +
                                  Request.Params["Vid"]);
            }
            else
            {
                Response.Redirect("RecLst.aspx");
            }
        }

        protected void btn_PopUpOK_Click(object sender, EventArgs e)
        {
            var checkItems = new List<string>();

            foreach (GridViewRow grd_Row in grd_PoList.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    checkItems.Add(grd_PoList.DataKeys[grd_Row.RowIndex].Value.ToString());
                }
            }

            AddPO(checkItems, LoginInfo.BuFmtInfo.BuCode, string.Empty);
            //if (Convert.ToBoolean(config.GetConfigValue("PC", "REC", "IsLocation", hf_ConnStr.Value)))
            //{
            //    for (int i = 0; i < checkItems.Count; i++)
            //    {
            //        //Get PO Data
            //        if (i != 0)
            //        {
            //            //Table1 is name of table that added in dsPO and merged others.
            //            dsPOList.Tables["Table1"].Merge(poDt.GetLocationByPoNo(checkItems[i].ToString(), LoginInfo.BuInfo.BuCode.ToString(), hf_ConnStr.Value));
            //        }
            //        else
            //        {
            //            dsPOList.Tables.Add(poDt.GetLocationByPoNo(checkItems[i].ToString(), LoginInfo.BuInfo.BuCode.ToString(), hf_ConnStr.Value));
            //        }
            //    }

            //    if (dsPOList.Tables["Table1"] != null)
            //    {
            //        if (dsPOList.Tables["Table1"].Rows.Count > 0)
            //        {
            //            //Get dupplicate location out.
            //            //Table1 is name of table that added in dsPO and merged others.
            //            var location = (from drLocation in dsPOList.Tables["Table1"].AsEnumerable()
            //                            select new
            //                            {
            //                                LocationCode = drLocation.Field<string>("LocationCode"),
            //                                LocationName = drLocation.Field<string>("LocationName")
            //                            }).Distinct();

            //            ddl_Location.DataSource = location;
            //            ddl_Location.ValueField = "LocationCode";
            //            ddl_Location.TextField = "LocationName";
            //            ddl_Location.SelectedIndex = 0;
            //            ddl_Location.DataBind();

            //            pop_Template.ShowOnPageLoad = false;
            //            pop_LocationList.ShowOnPageLoad = true;
            //        }
            //        else
            //        {
            //            lbl_WarningDelete.Text = "Does not have PO in this location";
            //            pop_Template.ShowOnPageLoad = false;
            //            pop_WarningDelete.ShowOnPageLoad = true;
            //        }
            //    }
            //}
            //else
            //{
            //    this.AddPO(checkItems,LoginInfo.BuFmtInfo.BuCode,string.Empty);
            //}
        }

        protected void btn_AddPo_Click(object sender, EventArgs e)
        {

            //Check PO from HQ
            if (dsRecEdit.Tables[rec.TableName].Rows[0]["PoSource"].ToString() != string.Empty
               && dsRecEdit.Tables[rec.TableName].Rows[0]["PoSource"].ToString() != LoginInfo.BuInfo.BuCode)
            {
                hf_ConnStr.Value = bu.GetConnectionString(dsRecEdit.Tables[rec.TableName].Rows[0]["PoSource"].ToString());
            }

            dsPOList.Clear();

            bool isSingleLocation = Convert.ToBoolean(config.GetConfigValue("PC", "REC", "IsLocation", hf_ConnStr.Value));
            string vendorCode = dsRecEdit.Tables[rec.TableName].Rows[0]["VendorCode"].ToString();
            string currencyCode = ddl_Currency.SelectedItem.Value.ToString();

            if (isSingleLocation)
            {
                //poDt.GetAddPoList(dsPOList, lbl_VendorCode.Text, LoginInfo.BuInfo.BuCode, dsRecEdit.Tables[recDt.TableName].Rows[0]["LocationCode"].ToString(), hf_ConnStr.Value);
                // Add Currency Code
                poDt.GetAddPoList(dsPOList, lbl_VendorCode.Text, LoginInfo.BuInfo.BuCode, dsRecEdit.Tables[recDt.TableName].Rows[0]["LocationCode"].ToString(), currencyCode, hf_ConnStr.Value);
            }
            else
            {
                //poDt.GetListByStatusAndVendor(dsPOList, statusPrint, statusPartial, lbl_VendorCode.Text, LoginInfo.BuFmtInfo.BuCode, hf_ConnStr.Value);
                // Add Currency Code
                poDt.GetListByStatusAndVendor(dsPOList, statusPrint, statusPartial, lbl_VendorCode.Text, LoginInfo.BuFmtInfo.BuCode, hf_ConnStr.Value);
            }

            if (dsPOList.Tables[poDt.TableName].Rows.Count > 0)
            {
                ddl_PopUpVendor.DataSource = po.GetVendorByPOStatus(statusPrint, statusPartial, LoginInfo.BuInfo.BuCode, hf_ConnStr.Value);
                ddl_PopUpVendor.ValueField = "VendorCode";
                ddl_PopUpVendor.DataBind();

                ddl_PopUpVendor.Value = lbl_VendorCode.Text;
                ddl_PopUpVendor.Enabled = false;

                grd_PoList.DataSource = dsPOList.Tables[poDt.TableName];
                grd_PoList.DataBind();

                pop_Template.ShowOnPageLoad = true;
            }
            else
            {
                if (isSingleLocation)
                    lbl_WarningDelete.Text = string.Format("PO is not available for this vendor '{0}' and location '{1}'.", vendorCode, dsRecEdit.Tables[recDt.TableName].Rows[0]["LocationCode"].ToString());
                else
                    lbl_WarningDelete.Text = string.Format("PO is not available for this vendor '{0}'.", vendorCode);

                pop_WarningDelete.ShowOnPageLoad = true;
            }

            //2012-12-25 by Keng
            //hf_ConnStr.Value = bu.GetConnectionString(Request.Params["BuCode"].ToString());
        }

        protected void btn_ConfirmLocation_Click(object sender, EventArgs e)
        {
            var checkItems = new List<string>();

            foreach (GridViewRow grd_Row in grd_PoList.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    checkItems.Add(grd_PoList.DataKeys[grd_Row.RowIndex].Value.ToString());
                }
            }

            AddPO(checkItems, LoginInfo.BuFmtInfo.BuCode, ddl_Location.Value.ToString().Split(':')[0]);
        }

        private void AddPO(List<string> values, string buCode, string location)
        {
            decimal price = 0;
            decimal qtyrec = 0;
            decimal discountPercentage = 0;
            decimal discountAmt = 0;
            decimal calAmount = 0;
            decimal tax = 0;
            decimal total = 0;
            decimal netAmt = 0;
            decimal taxAmt = 0;

            // Added on: 06/02/2018, By: Fon
            decimal currRate = 0;
            decimal currNetAmt = 0;
            decimal currDiscAmt = 0;
            decimal currTaxAmt = 0;
            decimal currTotalAmt = 0;

            //decimal.TryParse(txt_ExRateAu.Text, out currRate);
            currRate = Convert.ToDecimal(txt_ExRateAu.Text);
            // End Added.

            dsPOList.Clear();

            //Check PO from HQ
            if (dsRecEdit.Tables[rec.TableName].Rows[0]["PoSource"].ToString() != string.Empty &&
                dsRecEdit.Tables[rec.TableName].Rows[0]["PoSource"].ToString() != LoginInfo.BuInfo.BuCode)
            {
                hf_ConnStr.Value = bu.GetConnectionString(dsRecEdit.Tables[rec.TableName].Rows[0]["PoSource"].ToString());
            }

            for (var i = 0; i < values.Count; i++)
            {
                poDt.GetPoDtByPoNoForReceiving(dsPOList, ref MsgError, values[i], buCode, location, hf_ConnStr.Value);

                // Insert to RecDt
                foreach (DataRow drSelected in dsPOList.Tables[poDt.TableName].Rows)
                {
                    if (drSelected["PoNo"].ToString() == values[i])
                    {
                        // Check existing po in this receiving
                        string poNo = drSelected["PoNo"].ToString();
                        int poDtNo = Convert.ToInt32(drSelected["PoDt"]);

                        DataRow[] dr = dsRecEdit.Tables[recDt.TableName].Select(string.Format("PoNo = '{0}' AND PoDtNo = '{1}'", poNo, poDtNo.ToString()));

                        if (dr.Length == 0)
                        {
                            int recDtNo = 1;

                            if (dsRecEdit.Tables[recDt.TableName].Rows.Count > 0)
                                recDtNo = (int)dsRecEdit.Tables[recDt.TableName].Rows[dsRecEdit.Tables[recDt.TableName].Rows.Count - 1]["RecDtNo"] + 1;

                            var drRecDt = dsRecEdit.Tables[recDt.TableName].NewRow();

                            drRecDt["RecNo"] = txt_RecNo.Text;
                            drRecDt["RecDtNo"] = recDtNo; //dsRecEdit.Tables[recDt.TableName].Rows.Count + 1;
                            drRecDt["LocationCode"] = drSelected["Location"];
                            drRecDt["ProductCode"] = drSelected["Product"];
                            drRecDt["Descen"] = drSelected["Descen"];
                            drRecDt["Descll"] = drSelected["Descll"];
                            drRecDt["UnitCode"] = drSelected["Unit"];
                            drRecDt["RcvUnit"] = drSelected["Unit"];
                            drRecDt["Rate"] = prodUnit.GetConvRate(drSelected["Product"].ToString(), drSelected["Unit"].ToString(), LoginInfo.ConnStr);
                            drRecDt["OrderQty"] = Convert.ToDecimal(drSelected["OrderQty"] == DBNull.Value ? 0 : (decimal)drSelected["OrderQty"]);
                            drRecDt["FOCQty"] = Convert.ToDecimal(drSelected["FOCQty"] == DBNull.Value ? 0 : (decimal)drSelected["FOCQty"]);
                            drRecDt["RecQty"] = Convert.ToDecimal(drSelected["OrderQty"] == DBNull.Value ? 0 : (decimal)drSelected["OrderQty"]);
                            drRecDt["FOCQty"] = Convert.ToDecimal(drSelected["FOCQty"] == DBNull.Value ? 0 : (decimal)drSelected["FOCQty"]);
                            drRecDt["Price"] = Convert.ToDecimal(drSelected["Price"] == DBNull.Value ? 0 : (decimal)drSelected["Price"]);
                            drRecDt["Discount"] = Convert.ToDecimal(drSelected["Discount"] == DBNull.Value ? 0 : (decimal)drSelected["Discount"]);
                            drRecDt["TaxType"] = drSelected["TaxType"];
                            drRecDt["TaxRate"] = drSelected["TaxRate"];
                            drRecDt["TaxAdj"] = Convert.ToBoolean(drSelected["IsAdj"] == DBNull.Value ? false : (bool)drSelected["IsAdj"]);

                            qtyrec = (Convert.ToDecimal(drSelected["OrderQty"] == DBNull.Value ? 0 : (decimal)drSelected["OrderQty"]));
                            price = Convert.ToDecimal(drSelected["Price"] == DBNull.Value ? 0 : (decimal)drSelected["Price"]);
                            //Clear discount amount
                            discountAmt = 0;

                            // Discount percent
                            discountPercentage = Convert.ToDecimal(drSelected["Discount"] == DBNull.Value ? 0 : (decimal)drSelected["Discount"]);

                            // if discountpercent is zero, skip to calculate for discount amount.
                            if (discountPercentage > 0)
                            {
                                //discountAmt = (price * discountPercentage) / 100;
                                discountAmt = ((price * recQty) * discountPercentage) / 100;
                            }

                            // Taxrate.
                            tax = Convert.ToDecimal(drSelected["TaxRate"] == DBNull.Value ? 0 : (decimal)drSelected["TaxRate"]);

                            // Caluclated amount.
                            calAmount = Blue.BL.GnxLib.CalAmt(price, discountAmt, qtyrec);

                            // Midified on: 06/02/2018, By: Fon
                            //taxAmt = Blue.BL.GnxLib.TaxAmt(drSelected["TaxType"].ToString(), tax, price, discountAmt, qtyrec);
                            //netAmt = Blue.BL.GnxLib.NetAmt(drSelected["TaxType"].ToString(), tax, price, discountAmt, qtyrec);
                            //total = Blue.BL.GnxLib.Amount(drSelected["TaxType"].ToString(), tax, price, discountAmt, qtyrec);

                            currNetAmt = Blue.BL.GnxLib.NetAmt(drSelected["TaxType"].ToString(), tax, price, discountAmt, qtyrec);
                            currDiscAmt = discountAmt;
                            currTaxAmt = Blue.BL.GnxLib.TaxAmt(drSelected["TaxType"].ToString(), tax, price, discountAmt, qtyrec);
                            currTotalAmt = Blue.BL.GnxLib.Amount(drSelected["TaxType"].ToString(), tax, price, discountAmt, qtyrec);

                            netAmt = currNetAmt * currRate;
                            taxAmt = currTaxAmt * currRate;
                            discountAmt = discountAmt * currRate;
                            total = currTotalAmt * currRate;

                            drRecDt["CurrNetAmt"] = currNetAmt; // Convert.ToDecimal(String.Format("{0:0.00}", currNetAmt));
                            drRecDt["CurrDiscAmt"] = currDiscAmt; // Convert.ToDecimal(String.Format("{0:0.00}", currDiscAmt));
                            drRecDt["CurrTaxAmt"] = currTaxAmt; //Convert.ToDecimal(String.Format("{0:0.00}", currTaxAmt));
                            drRecDt["CurrTotalAmt"] = currTotalAmt; // Convert.ToDecimal(String.Format("{0:0.00}", currTotalAmt));
                            // End Modified.

                            drRecDt["DiccountAmt"] = discountAmt;
                            drRecDt["NetAmt"] = netAmt; // Convert.ToDecimal(String.Format("{0:0.00}", netAmt));
                            drRecDt["TaxAmt"] = taxAmt; // Convert.ToDecimal(String.Format("{0:0.00}", taxAmt));
                            drRecDt["TotalAmt"] = total; // Convert.ToDecimal(String.Format("{0:0.00}", total));
                            drRecDt["PoNo"] = drSelected["PoNo"];
                            drRecDt["PoDtNo"] = drSelected["PoDt"];
                            drRecDt["NetDrAcc"] = accountMapp.GetA3Code(LoginInfo.BuInfo.BuCode, drSelected["Location"].ToString(), drSelected["Product"].ToString().Substring(0, 4), LoginInfo.ConnStr);
                            drRecDt["TaxDrAcc"] = drSelected["TaxAccCode"];
                            drRecDt["Status"] = System.DBNull.Value;
                            drRecDt["ExportStatus"] = false;

                            // Add new record
                            dsRecEdit.Tables[recDt.TableName].Rows.Add(drRecDt);
                        }
                    }
                }
            }

            hf_ConnStr.Value = LoginInfo.ConnStr;

            grd_RecEdit.DataSource = dsRecEdit.Tables[recDt.TableName];
            grd_RecEdit.DataBind();

            Session["dsRecEdit"] = dsRecEdit;

            pop_LocationList.ShowOnPageLoad = false;
            pop_Template.ShowOnPageLoad = false;

            // When edit mode, changes neede to keep to session.
            if (Request.Params["MODE"] != null)
            {
                if (Request.Params["MODE"].ToUpper() == "EDIT")
                {
                    Session["dsSave"] = dsRecEdit;
                }
            }
        }

        protected void btn_NewRcvDt_Click(object sender, EventArgs e)
        {
            var currentUrl = Request.CurrentExecutionFilePath;
            var ID = string.Empty;
            var statusValue = "&Status=New";

            if (Request.QueryString["ID"] != null)
            {
                ID = "&ID=" + Request.QueryString["ID"];
            }

            Response.Redirect(currentUrl + "?MODE=" + Request.QueryString["MODE"] + ID + statusValue);
        }

        private DataColumn[] GetPK()
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsPOList.Tables[poDt.TableName].Columns["CompoSiteKey"];
            return primaryKeys;
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {

            if (grd_RecEdit.Rows.Count > 0)
            {
                string errorMessage = string.Empty;

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
                    string vendorCode = lbl_VendorCode.Text.Split(':')[0].ToString().Trim();

                    var sql = "SELECT COUNT(*) as RecordCount FROM PC.REC WHERE VendorCode=@VendorCode AND InvoiceNo=@InvoiceNo AND RecNo<>@RecNo AND DocStatus<>'Voided'";
                    var p = new Blue.DAL.DbParameter[]{
                        new Blue.DAL.DbParameter("@VendorCode",vendorCode),
                        new Blue.DAL.DbParameter("@InvoiceNo", invoiceNo),
                        new Blue.DAL.DbParameter("@RecNo", recNo),
                    };

                    DataTable dt = rec.DbExecuteQuery(sql, p, LoginInfo.ConnStr);

                    if (Convert.ToInt32(dt.Rows[0]["RecordCount"]) > 0) // duplicate
                        errorMessage = string.Format("Invoice No '{0}' already exists.", invoiceNo); ;
                }


                if (errorMessage != string.Empty)
                {
                    lbl_WarningOth.Text = errorMessage;
                    pop_Warning.ShowOnPageLoad = true;
                    return;

                }

                // --------------------------------------------------------

                pop_ConfirmSave.ShowOnPageLoad = true;
            }
            else
            {
                lbl_WarningDelete.Text = "Cannot save because receiving have no details.";
                pop_WarningDelete.ShowOnPageLoad = true;
            }
        }

        protected void btn_Commit_Click(object sender, EventArgs e)
        {
            if (txt_InvNo.Text == string.Empty || de_InvDate.Text == string.Empty)
            {
                lbl_WarningDelete.Text = "Please insert invoice number and invoice date";
                pop_WarningDelete.ShowOnPageLoad = true;
                return;
            }

            if (grd_RecEdit.Rows.Count > 0)
            {
                pop_ConfirmCommit.ShowOnPageLoad = true;
            }
            else
            {
                lbl_WarningDelete.Text = "Cannot commit because receiving have no details.";
                pop_WarningDelete.ShowOnPageLoad = true;
            }
        }

        private void MessageBox(string msg)
        {
            var lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

        protected void btn_WarningPeriod_Click(object sender, EventArgs e)
        {
            Response.Redirect("RecLst.aspx");
        }

        protected void btn_ConfirmDelete_Click(object sender, EventArgs e)
        {
            pop_ConfirmDelete.ShowOnPageLoad = false;
        }

        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            pop_ConfirmDelete.ShowOnPageLoad = false;
        }

        protected void btn_WarningDelete_Click(object sender, EventArgs e)
        {
            pop_WarningDelete.ShowOnPageLoad = false;

            if (MODE.ToUpper() != "EDIT")
            {
                //Set ViewState to false on 04/04/2012
                btn_AddPo.Enabled = false;
            }
        }

        protected void grd_RecEdit_Load(object sender, EventArgs e)
        {
            grandTotal = 0;
            grandNetAmt = 0;
            grandTaxAmt = 0;

            cmb_DeliPoint.DataSource = dsRecEdit.Tables[deliPoint.TableName];
            cmb_DeliPoint.ValueField = "DptCode";
            cmb_DeliPoint.TextField = "Name";
            cmb_DeliPoint.DataBind();
        }

        protected void btn_ConfirmSave_Click(object sender, EventArgs e)
        {
            var strStartDate = period.GetStartDate(Convert.ToDateTime(de_RecDate.Date), hf_ConnStr.Value);
            var strEndDate = period.GetEndDate(Convert.ToDateTime(de_RecDate.Date), hf_ConnStr.Value);

            if (strStartDate != string.Empty & strEndDate != string.Empty)
            {
                SaveAndCommit("Received");
                System.Threading.Thread.Sleep(100);
            }
            else
            {
                pop_WarningPeriod.ShowOnPageLoad = true;
            }


            pop_ConfirmSave.ShowOnPageLoad = false;
        }

        protected void btn_CancelSave_Click(object sender, EventArgs e)
        {
            pop_ConfirmSave.ShowOnPageLoad = false;

            if (MODE.ToUpper() != "EDIT")
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
                // string recNo = dsSave.Tables[rec.TableName].Rows[0]["RecNo"].ToString();
                // string docStatus = dsSave.Tables[rec.TableName].Rows[0]["DocStatus"].ToString();

                // if (docStatus == "Committed")
                // Response.Redirect("Rec.aspx?ID=" + recNo + "&BuCode=" + Request.Params["BuCode"] + "&Vid=" + Request.Params["Vid"]);
                // else
                // {
                // SaveAndCommit("Committed");
                // }
            }
            else
            {
                pop_WarningPeriod.ShowOnPageLoad = true;
            }

        }

        protected void btn_CancelCommit_Click(object sender, EventArgs e)
        {
            pop_ConfirmCommit.ShowOnPageLoad = false;

            if (MODE.ToUpper() != "EDIT")
            {
                //Set ViewState to false on 04/04/2012
                btn_AddPo.Enabled = false;
            }
        }

        //protected void txt_TaxDrAcc_TextChanged(object sender, EventArgs e)
        //{
        //    var txt_TaxDrAcc = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_TaxDrAcc") as TextBox;

        //    if (txt_TaxDrAcc.Text == string.Empty)
        //    {
        //        lbl_WarningDelete.Text = "Tax A/C# is empty";
        //        pop_WarningDelete.ShowOnPageLoad = true;
        //    }
        //    taxAcCode = txt_TaxDrAcc.Text;
        //}

        //protected void txt_NetDrAcc_TextChanged(object sender, EventArgs e)
        //{
        //    var txt_NetDrAcc = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_NetDrAcc") as TextBox;

        //    if (txt_NetDrAcc.Text == string.Empty)
        //    {
        //        lbl_WarningDelete.Text = "Net A/C# is empty";
        //        pop_WarningDelete.ShowOnPageLoad = true;
        //    }
        //    netAcCode = txt_NetDrAcc.Text;
        //}

        protected void ddl_RcvUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lbl_RcvUnit_Expand = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_RcvUnit_Expand") as Label;
            var lbl_ConvertRate = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_ConvertRate") as Label;
            var lbl_BaseQty = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_BaseQty") as Label;
            var lbl_Receive = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_Receive") as Label;
            var se_RecQtyEdit = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_RecQtyEdit") as ASPxSpinEdit;
            var se_PriceEdit = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_PriceEdit") as ASPxSpinEdit;
            var ddl_RcvUnit = sender as ASPxComboBox;

            var product = dsRecEdit.Tables[recDt.TableName].Rows[grd_RecEdit.EditIndex]["ProductCode"].ToString();

            se_RecQtyEdit.Text = String.Format(DefaultQtyFmt,
                prodUnit.GetQtyAfterChangeUnit(product, ddl_RcvUnit.Value.ToString(), lbl_RcvUnit_Expand.Text,
                    Convert.ToDecimal(se_RecQtyEdit.Text), hf_ConnStr.Value));

            se_PriceEdit.Text = String.Format(DefaultAmtFmt,
                (Convert.ToDecimal(se_PriceEdit.Text) *
                 prodUnit.GetConvRate(product, ddl_RcvUnit.Value.ToString(), hf_ConnStr.Value)) /
                prodUnit.GetConvRate(product, lbl_RcvUnit_Expand.Text, hf_ConnStr.Value));

            lbl_Receive.Text = se_RecQtyEdit.Text;
            lbl_Receive.ToolTip = lbl_Receive.Text;

            lbl_ConvertRate.Text =
                prodUnit.GetConvRate(
                    dsRecEdit.Tables[recDt.TableName].Rows[grd_RecEdit.EditIndex]["ProductCode"].ToString(),
                    ddl_RcvUnit.Value.ToString(), hf_ConnStr.Value).ToString();
            lbl_ConvertRate.ToolTip = lbl_ConvertRate.Text;

            lbl_BaseQty.Text = String.Format(DefaultQtyFmt,
                prodUnit.GetQtyAfterChangeUnit(
                    dsRecEdit.Tables[recDt.TableName].Rows[grd_RecEdit.EditIndex]["ProductCode"].ToString(),
                    prodUnit.GetInvenUnit(
                        dsRecEdit.Tables[recDt.TableName].Rows[grd_RecEdit.EditIndex]["ProductCode"].ToString(),
                        hf_ConnStr.Value)
                    , ddl_RcvUnit.Value.ToString(), Convert.ToDecimal(se_RecQtyEdit.Text), hf_ConnStr.Value));
            lbl_BaseQty.ToolTip = lbl_BaseQty.Text;

            lbl_RcvUnit_Expand.Text = ddl_RcvUnit.Value.ToString();
            lbl_RcvUnit_Expand.ToolTip = lbl_RcvUnit_Expand.Text;

            //rcvUnit = string.Empty;
        }

        protected void ddl_RcvUnit_Load(object sender, EventArgs e)
        {
            var ddl_RcvUnit = sender as ASPxComboBox;
            var ProductCode = dsRecEdit.Tables[recDt.TableName].Rows[((GridViewRow)ddl_RcvUnit.NamingContainer).DataItemIndex]["ProductCode"].ToString();

            //if (ProductCode != string.Empty)
            if (!string.IsNullOrEmpty(ProductCode))
            {
                ddl_RcvUnit.DataSource = prodUnit.GetLookUp_ProductCode(ProductCode, hf_ConnStr.Value);
                ddl_RcvUnit.DataBind();
            }
        }

        protected void ddl_TaxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Update DataTable for change dropdrowlist but has problem in case cancel.
            //dsRecEdit.Tables[recDt.TableName].Rows[grd_RecEdit.EditIndex]["TaxType"] = ddl_TaxType.SelectedItem.Value;

            //CalculateTaxType(grd_RecEdit.EditIndex, "TaxType");

            // Modified on: 24/08/2017, By: Fon
            CheckBox chk_TaxAdj = (CheckBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("chk_TaxAdj");
            if (chk_TaxAdj.Checked)
            {

                DropDownList ddl_TaxType = (DropDownList)sender;
                TextBox txt_TaxRate = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_TaxRate");
                TextBox txt_CurrTaxAmt = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_CurrTaxAmt");
                TextBox txt_TaxAmt = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_TaxAmt");

                if (txt_TaxRate != null)
                {
                    txt_TaxRate.Enabled = true;
                    txt_CurrTaxAmt.Enabled = true;
                    txt_TaxAmt.Enabled = false;

                    // Comment on: 01/02/2018, By: Fon, For: Following from P' Oat guide.
                    //txt_TaxAmt.Enabled = true;
                    // End Comment.
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

                //CalculateCost(grd_RecEdit.EditIndex);
                CalculationForValueChanged();
                // End Modified.
            }
        }

        protected void grd_RecEdit_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var lnkb = (LinkButton)e.CommandSource;
            GridViewRow gvr = (GridViewRow)lnkb.NamingContainer;
            switch (e.CommandName)
            {
                case "Edit":
                    // For store netAcCode & taxAcCode
                    var lbl_NetAcc = (Label)gvr.FindControl("lbl_NetAcc");
                    var lbl_TaxAcc = (Label)gvr.FindControl("lbl_TaxAcc");
                    //netAcCode = lbl_NetAcc.Text;
                    //taxAcCode = lbl_TaxAcc.Text;
                    break;
            }
        }

        #endregion

        // Added on: 24/08/2017, By: Fon
        #region "Receiving Header"
        //protected void CostContent_ValueChanged(int editIndex)
        //{
        //    //Fon 's note: I look on price & reqQty like a bid unit
        //    // , so price is big price and recQty = 1. 
        //    #region variable

        //    decimal recQty = 0, price = 0, totalPrice = 0;
        //    decimal netAmt = 0, discAmt = 0, taxAmt = 0, totalAmt = 0;
        //    decimal currNetAmt = 0, currDiscAmt = 0, currTaxAmt = 0, currTotalAmt = 0;
        //    decimal discPercent = 0, taxRate = 0;
        //    decimal currRate = decimal.Parse(txt_ExRateAu.Text);
        //    string taxType = string.Empty;
        //    #endregion

        //    #region Find variable value
        //    if (grd_RecEdit.Rows[editIndex].FindControl("se_RecQtyEdit") != null)
        //    {
        //        ASPxSpinEdit se_RecQtyEdit = (ASPxSpinEdit)grd_RecEdit.Rows[editIndex].FindControl("se_RecQtyEdit");
        //        decimal.TryParse(se_RecQtyEdit.Text, out recQty);
        //    }

        //    if (grd_RecEdit.Rows[editIndex].FindControl("se_PriceEdit") != null)
        //    {
        //        ASPxSpinEdit se_PriceEdit = (ASPxSpinEdit)grd_RecEdit.Rows[editIndex].FindControl("se_PriceEdit");
        //        decimal.TryParse(se_PriceEdit.Text, out price);
        //    }

        //    if (grd_RecEdit.Rows[editIndex].FindControl("chk_DiscAdj") != null)
        //    {
        //        CheckBox chk_DiscAdj = (CheckBox)grd_RecEdit.Rows[editIndex].FindControl("chk_DiscAdj");
        //        if (chk_DiscAdj.Checked && grd_RecEdit.Rows[editIndex].FindControl("txt_Disc") != null)
        //        {
        //            TextBox txt_Disc = (TextBox)grd_RecEdit.Rows[editIndex].FindControl("txt_Disc");
        //            decimal.TryParse(txt_Disc.Text, out discPercent);
        //        }
        //    }


        //    if (grd_RecEdit.Rows[editIndex].FindControl("ddl_TaxType") != null)
        //    {
        //        DropDownList ddl_TaxType = (DropDownList)grd_RecEdit.Rows[editIndex].FindControl("ddl_TaxType");
        //        TextBox txt_TaxRate = (TextBox)grd_RecEdit.Rows[editIndex].FindControl("txt_TaxRate");
        //        decimal.TryParse(txt_TaxRate.Text, out taxRate);
        //        taxType = ddl_TaxType.SelectedItem.Value.ToUpper();
        //    }

        //    if (grd_RecEdit.Rows[editIndex].FindControl("txt_CurrDiscAmt") != null)
        //    {
        //        TextBox txt_CurrDiscAmt = (TextBox)grd_RecEdit.Rows[editIndex].FindControl("txt_CurrDiscAmt");
        //        decimal.TryParse(txt_CurrDiscAmt.Text, out currDiscAmt);
        //    }

        //    if (grd_RecEdit.Rows[editIndex].FindControl("txt_DiscAmt") != null)
        //    {
        //        TextBox txt_DiscAmt = (TextBox)grd_RecEdit.Rows[editIndex].FindControl("txt_DiscAmt");
        //        discAmt = currDiscAmt * currRate;
        //    }
        //    #endregion
        //    totalPrice = price * recQty;

        //    currNetAmt = Blue.BL.GnxLib.NetAmt(taxType, taxRate, totalPrice, currDiscAmt, 1);
        //    currTaxAmt = Blue.BL.GnxLib.TaxAmt(taxType, taxRate, totalPrice, currDiscAmt, 1);
        //    //currTotalAmt = Blue.BL.GnxLib.Amount(taxType, taxRate, totalPrice, currDiscAmt, 1);
        //    currTotalAmt = currNetAmt + currTaxAmt;

        //    //netAmt = Blue.BL.GnxLib.NetAmt(taxType, taxRate, totalPrice * currRate, discAmt, 1);
        //    //taxAmt = Blue.BL.GnxLib.TaxAmt(taxType, taxRate, totalPrice * currRate, discAmt, 1);
        //    //totalAmt = Blue.BL.GnxLib.Amount(taxType, taxRate, totalPrice * currRate, discAmt, 1);
        //    netAmt = currNetAmt * currRate;
        //    currTaxAmt = currTaxAmt * currRate;
        //    totalAmt = currTotalAmt * currRate;

        //    #region Detail Highlight
        //    // Comment By: Fon, Note: Useless
        //    //if (grd_RecEdit.Rows[editIndex].FindControl("se_TotalAmt") != null)
        //    //{
        //    //    ASPxSpinEdit se_TotalAmt = (ASPxSpinEdit)grd_RecEdit.Rows[editIndex].FindControl("se_TotalAmt");
        //    //    //se_TotalAmt.Text = string.Format("{0:N}", Math.Round(currTotalAmt, 2));
        //    //}

        //    if (grd_RecEdit.Rows[editIndex].FindControl("lbl_TotalAmt") != null)
        //    {
        //        Label lbl_TotalAmt = (Label)grd_RecEdit.Rows[editIndex].FindControl("lbl_TotalAmt");
        //        lbl_TotalAmt.Text = string.Format("{0:N}", Math.Round(totalPrice, 2));
        //    }

        //    if (grd_RecEdit.Rows[editIndex].FindControl("lblNm_DiscAmt") != null)
        //    {
        //        Label lblNm_DiscAmt = (Label)grd_RecEdit.Rows[editIndex].FindControl("lblNm_DiscAmt");
        //        lblNm_DiscAmt.Text = string.Format("{0:N}", Math.Round(currDiscAmt, 2));
        //    }

        //    if (grd_RecEdit.Rows[editIndex].FindControl("lblNm_NetAmt") != null)
        //    {
        //        Label lblNm_NetAmt = (Label)grd_RecEdit.Rows[editIndex].FindControl("lblNm_NetAmt");
        //        lblNm_NetAmt.Text = string.Format("{0:N}", Math.Round(totalPrice - currDiscAmt, 2));
        //    }
        //    #endregion

        //    #region Set content value.
        //    if (grd_RecEdit.Rows[editIndex].FindControl("txt_CurrNetAmt") != null)
        //    {
        //        TextBox txt_CurrNetAmt = (TextBox)grd_RecEdit.Rows[editIndex].FindControl("txt_CurrNetAmt");
        //        txt_CurrNetAmt.Text = string.Format("{0:N}", Math.Round(currNetAmt, 2));
        //    }

        //    if (grd_RecEdit.Rows[editIndex].FindControl("txt_CurrDiscAmt") != null)
        //    {
        //        TextBox txt_CurrDiscAmt = (TextBox)grd_RecEdit.Rows[editIndex].FindControl("txt_CurrDiscAmt");
        //        txt_CurrDiscAmt.Text = string.Format("{0:N}", Math.Round(currDiscAmt, 2));
        //    }

        //    if (grd_RecEdit.Rows[editIndex].FindControl("txt_CurrTaxAmt") != null)
        //    {
        //        TextBox txt_CurrTaxAmt = (TextBox)grd_RecEdit.Rows[editIndex].FindControl("txt_CurrTaxAmt");
        //        txt_CurrTaxAmt.Text = string.Format("{0:N}", Math.Round(currTaxAmt, 2));
        //    }

        //    if (grd_RecEdit.Rows[editIndex].FindControl("lbl_CurrTotalAmtDt") != null)
        //    {
        //        Label lbl_CurrTotalAmtDt = (Label)grd_RecEdit.Rows[editIndex].FindControl("lbl_CurrTotalAmtDt");
        //        lbl_CurrTotalAmtDt.Text = string.Format("{0:N}", Math.Round(currTotalAmt, 2));
        //    }

        //    // About Base Currency
        //    if (grd_RecEdit.Rows[editIndex].FindControl("txt_Disc") != null)
        //    {
        //        TextBox txt_Disc = (TextBox)grd_RecEdit.Rows[editIndex].FindControl("txt_Disc");
        //        txt_Disc.Text = string.Format("{0:N}", Math.Round(Get_Disc_Percent(currDiscAmt)));
        //    }

        //    if (grd_RecEdit.Rows[editIndex].FindControl("txt_NetAmt") != null)
        //    {
        //        TextBox txt_NetAmt = (TextBox)grd_RecEdit.Rows[editIndex].FindControl("txt_NetAmt");
        //        txt_NetAmt.Text = string.Format("{0:N}", Math.Round(netAmt, 2));
        //    }

        //    if (grd_RecEdit.Rows[editIndex].FindControl("txt_DiscAmt") != null)
        //    {
        //        TextBox txt_DiscAmt = (TextBox)grd_RecEdit.Rows[editIndex].FindControl("txt_DiscAmt");
        //        txt_DiscAmt.Text = string.Format("{0:N}", Math.Round(currDiscAmt * currRate, 2));
        //    }

        //    if (grd_RecEdit.Rows[editIndex].FindControl("txt_TaxAmt") != null)
        //    {
        //        taxAmt = currTaxAmt * currRate;
        //        TextBox txt_TaxAmt = (TextBox)grd_RecEdit.Rows[editIndex].FindControl("txt_TaxAmt");
        //        txt_TaxAmt.Text = string.Format("{0:N}", Math.Round(taxAmt, 2));
        //    }

        //    if (grd_RecEdit.Rows[editIndex].FindControl("lbl_TotalAmtDt") != null)
        //    {
        //        Label lbl_TotalAmtDt = (Label)grd_RecEdit.Rows[editIndex].FindControl("lbl_TotalAmtDt");
        //        lbl_TotalAmtDt.Text = string.Format("{0:N}", Math.Round(totalAmt, 2));
        //    }
        //    #endregion

        //}

        protected decimal Get_TaxRate(decimal taxAmt, decimal price, decimal discPerUnit, decimal qty)
        {
            // Use before Calculate_Cost()
            DropDownList ddl_TaxType = (DropDownList)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("ddl_TaxType");
            decimal calAmt = Blue.BL.GnxLib.CalAmt(price, discPerUnit, qty);
            decimal taxRate = 0;

            if (ddl_TaxType.SelectedItem.Value.ToUpper() == "I")
                taxRate = taxAmt / (calAmt - taxAmt);

            else
                taxRate = taxAmt / calAmt;

            taxRate = taxRate * 100;
            return taxRate;
        }

        protected void txt_CurrTaxAmt_TextChanged(object sender, EventArgs e)
        {
            CalculationForValueChanged(false, true);

            //CheckBox chk_TaxAdj = (CheckBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("chk_TaxAdj");

            //if (chk_TaxAdj.Checked)
            //{
            //    Label lbl_TotalAmt = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_TotalAmt") as Label;
            //    DropDownList ddl_TaxType = grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("ddl_TaxType") as DropDownList;


            //    TextBox txt_CurrNetAmt = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_CurrNetAmt");
            //    TextBox txt_CurrTaxAmt = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_CurrTaxAmt");
            //    Label lbl_CurrTotalAmtDt = (Label)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_CurrTotalAmtDt");


            //    decimal currNetAmt = decimal.Parse(txt_CurrNetAmt.Text);
            //    decimal currTaxAmt = decimal.Parse(txt_CurrTaxAmt.Text);
            //    decimal currTotalAmt = currNetAmt + currTaxAmt;

            //    if (ddl_TaxType.SelectedValue.ToUpper() == "I")
            //    {
            //        currTotalAmt = decimal.Parse(lbl_TotalAmt.Text);
            //        currNetAmt = currTotalAmt - currTaxAmt;
            //        txt_CurrNetAmt.Text = string.Format("{0:0.00}", currNetAmt);

            //    }

            //    lbl_CurrTotalAmtDt.Text = string.Format("{0:0.00}", currTotalAmt);


            //    TextBox txt_NetAmt = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_NetAmt");
            //    TextBox txt_TaxAmt = (TextBox)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("txt_TaxAmt");
            //    Label lbl_TotalAmtDt = (Label)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("lbl_TotalAmtDt");

            //    decimal currRate = decimal.Parse(txt_ExRateAu.Text);
            //    txt_NetAmt.Text = string.Format("{0:0.00}", currNetAmt * currRate);
            //    txt_TaxAmt.Text = string.Format("{0:0.00}", currTaxAmt * currRate);
            //    lbl_TotalAmtDt.Text = string.Format("{0:0.00}", currTotalAmt * currRate);
            //    lbl_TotalAmt.Text = lbl_TotalAmtDt.Text;

            //}

        }

        protected void ddl_Currency_Init(object sender, EventArgs e)
        {
            ASPxComboBox ddl_Currency = (ASPxComboBox)sender;
            ddl_Currency.DataSource = currency.GetLastCurrencyRate(LoginInfo.ConnStr);
            ddl_Currency.TextField = "CurrencyCode";
            ddl_Currency.ValueField = "CurrencyCode";
            ddl_Currency.DataBind();
            ddl_Currency.Value = baseCurrency;
        }

        protected void ddl_Currency_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime invoiceDate = DateTime.Now;
            if (de_InvDate.Text != string.Empty)
                invoiceDate = de_InvDate.Date;
            txt_ExRateAu.Text = currency.GetLastCurrencyRate(ddl_Currency.Value.ToString(), invoiceDate, LoginInfo.ConnStr).ToString();
            txt_ExRateAu_TextChanged(txt_ExRateAu, e);
        }

        protected void txt_ExRateAu_TextChanged(object sender, EventArgs e)
        {
            RateChanged(dsRecEdit.Tables[recDt.TableName]);
            grd_RecEdit.DataSource = dsRecEdit.Tables[recDt.TableName];
            grd_RecEdit.DataBind();
        }
        protected decimal Get_Disc_Percent(decimal currDiscAmt)
        {
            decimal discRate = 0;
            // Follow from old method at grd_Prdt1_updating
            ASPxSpinEdit se_PriceEdit = (ASPxSpinEdit)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_PriceEdit");
            ASPxSpinEdit se_RecQtyEdit = (ASPxSpinEdit)grd_RecEdit.Rows[grd_RecEdit.EditIndex].FindControl("se_RecQtyEdit");

            decimal price = 0, qty = 0;
            decimal.TryParse(se_PriceEdit.Text, out price);
            decimal.TryParse(se_RecQtyEdit.Text, out qty);
            decimal.TryParse(dsRecEdit.Tables[recDt.TableName].Rows[grd_RecEdit.EditIndex]["RecQty"].ToString(), out qty);
            discRate = (currDiscAmt * 100) / (price * qty);

            return discRate;
        }

        protected DataTable RateChanged(DataTable dtRecEdit)
        {
            // currNetAmt = Blue.BL.GnxLib.NetAmt(taxType, taxRate, price * recQty, currDiscAmt, 1);
            string taxType = string.Empty;
            decimal price = 0, recQty = 0, taxRate = 0, currRate = 0;
            decimal currNetAmt = 0, currDiscAmt = 0, currTaxAmt = 0, currTotalAmt = 0;
            decimal netAmt = 0, discAmt = 0, taxAmt = 0, totalAmt = 0;

            foreach (DataRow dr in dtRecEdit.Rows)
            {
                // Get value
                taxType = Convert.ToString(dr["TaxType"]);

                decimal.TryParse(dr["Price"].ToString(), out price);
                decimal.TryParse(dr["RecQty"].ToString(), out recQty);
                decimal.TryParse(dr["TaxRate"].ToString(), out taxRate);
                decimal.TryParse(txt_ExRateAu.Text, out currRate);
                decimal.TryParse(dr["CurrDiscAmt"].ToString(), out currDiscAmt);
                decimal.TryParse(dr["CurrNetAmt"].ToString(), out currNetAmt);
                decimal.TryParse(dr["CurrTaxAmt"].ToString(), out currTaxAmt);
                decimal.TryParse(dr["CurrTotalAmt"].ToString(), out currTotalAmt);

                // Set value
                //discAmt = currDiscAmt * currRate;
                //netAmt = Blue.BL.GnxLib.NetAmt(taxType, taxRate, (price * currRate) * recQty, discAmt, 1);
                //taxAmt = Blue.BL.GnxLib.TaxAmt(taxType, taxRate, (price * currRate) * recQty, discAmt, 1);
                //totalAmt = Blue.BL.GnxLib.Amount(taxType, taxRate, (price * currRate) * recQty, discAmt, 1);

                discAmt = RoundAmt(currDiscAmt * currRate);
                netAmt = RoundAmt(currNetAmt * currRate);
                taxAmt = RoundAmt(currTaxAmt * currRate);
                totalAmt = RoundAmt(currTotalAmt * currRate);

                dr["NetAmt"] = netAmt;
                dr["DiccountAmt"] = discAmt;
                dr["TaxAmt"] = taxAmt;
                dr["TotalAmt"] = totalAmt;
            }

            return dtRecEdit;
        }
        #endregion

        protected void btn_acceptWarn_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
        }
    }
}