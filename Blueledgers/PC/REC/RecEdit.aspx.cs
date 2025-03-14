﻿using System;
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
using System.ComponentModel.DataAnnotations;

namespace BlueLedger.PL.IN.REC
{
    public partial class RECEdit : BasePage
    {
        private readonly Blue.BL.dbo.Bu _bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.APP.Config _config = new Blue.BL.APP.Config();


        #region --URL Parameters--

        protected string _BuCode { get { return Request.Params["BuCode"].ToString() ?? ""; } }
        protected string _VID { get { return Request.Params["VID"] == null ? "" : Request.Params["VID"].ToString() ?? ""; } }
        protected string _ID { get { return Request.Params["ID"] == null ? "" : Request.Params["ID"].ToString() ?? ""; } }
        protected string _MODE { get { return Request.Params["Mode"] == null ? "" : Request.Params["Mode"].ToString().ToLower(); } }

        #endregion


        protected DataTable _dtRec
        {
            //get { return ViewState["dtRec"] as DataTable; }
            //set { ViewState["dtRec"] = value; }
            get { return ViewState["dtRec" + _ID] as DataTable; }
            set { ViewState["dtRec" + _ID] = value; }

        }

        protected DataTable _dtRecDt
        {
            //get { return ViewState["dtRecDt"] as DataTable; }
            //set { ViewState["dtRecDt"] = value; }
            get { return ViewState["dtRecDt" + _ID] as DataTable; }
            set { ViewState["dtRecDt" + _ID] = value; }

        }

        protected DataTable _dtPo
        {
            get { return ViewState["dtPo"] as DataTable; }
            set { ViewState["dtPo"] = value; }
        }

        protected DataTable _dtExtraCost
        {
            get { return ViewState["dtExtraCost"] as DataTable; }
            set { ViewState["dtExtraCost"] = value; }
        }

        protected DefaultValues _default
        {
            get { return ViewState["DefaultValues"] as DefaultValues; }
            set { ViewState["DefaultValues"] = value; }
        }

        #region --Event(s)--

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_ConnStr.Value = _bu.GetConnectionString(_BuCode);

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
            if (!IsPostBack)
            {
                Page_Retrieve();
            }

        }

        private void Page_Retrieve()
        {
            GetCurrencyRate(_default.Currency, DateTime.Today);

            switch (_MODE)
            {
                case "new":
                    SetNew();
                    break;
                case "edit":
                    SetEdit(_ID);

                    //de_RecDate.Enabled = false;
                    ddl_Vendor.Enabled = false;

                    break;
                case "fpo":
                    if (Session["dsPo"] == null)
                    {
                        pop_SessionTimeout.ShowOnPageLoad = true;
                        return;
                    }

                    SetFromPO();
                    // Set disable controls
                    // header
                    ddl_Vendor.Enabled = false;
                    ddl_Currency.Enabled = false;


                    break;
            }

            BindHeader(_dtRec);
            BindDetails();

            var recNo = _dtRec.Rows[0]["RecNo"].ToString();

            _dtExtraCost = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery(string.Format("SELECT e.*, t.TypeName FROM PC.RecExtCost e JOIN PC.ExtCostType t ON t.TypeId=e.TypeId WHERE e.RecNo='{0}'", recNo));
            gv_ExtraCost.DataSource = _dtExtraCost;
            gv_ExtraCost.DataBind();


            Page_Setting();
        }

        private void Page_Setting()
        {

        }

        // Title / Action bar
        protected void btn_Save_Click(object sender, EventArgs e)
        {
            SaveAndCommit(false);
        }


        protected void btn_Commit_Click(object sender, EventArgs e)
        {
            SaveAndCommit(true);
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
            if (IsCreatedManual())
            {
                var locationCode = _dtRecDt.Rows.Count == 0 ? "" : _dtRecDt.Rows[_dtRecDt.Rows.Count - 1]["LocationCode"].ToString();

                var dr = _dtRecDt.NewRow();

                dr["RecNo"] = lbl_RecNo.Text;
                dr["RecDtNo"] = 0;
                dr["LocationCode"] = locationCode;
                dr["ProductCode"] = "";
                dr["UnitCode"] = "";
                dr["OrderQty"] = 0;

                dr["DiscAdj"] = 0;
                dr["Discount"] = 0;


                dr["TaxAdj"] = 0;
                dr["TaxType"] = "N";
                dr["TaxRate"] = 0;

                dr["CurrDiscAmt"] = 0;
                dr["CurrNetAmt"] = 0;
                dr["CurrTaxAmt"] = 0;
                dr["CurrTotalAmt"] = 0;

                dr["DiccountAmt"] = 0;
                dr["NetAmt"] = 0;
                dr["TaxAmt"] = 0;
                dr["TotalAmt"] = 0;

                //dr[""] = "";
                //dr[""] = "";
                //dr[""] = "";
                //dr[""] = "";
                //dr[""] = "";

                dr["ExportStatus"] = false;
                //dr["ExpiryDate"] = DBNull.Value;



                _dtRecDt.Rows.Add(dr);

                gv_Detail.DataSource = _dtRecDt;
                gv_Detail.DataBind();
                gv_Detail.EditIndex = gv_Detail.Rows[gv_Detail.Rows.Count - 1].RowIndex;
                gv_Detail.DataBind();

                SetEditItem(true);

            }
            else // from PO
            {
                var vendorCode = ddl_Vendor.SelectedItem.Value.ToString();
                var locationCode = _dtRecDt.Rows.Count > 0 ? _dtRecDt.Rows[0]["LocationCode"].ToString() : "";
                var currencyCode = ddl_Currency.SelectedItem.Value.ToString();
                var dtPoList = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery(string.Format("EXEC [PC].[GetAddPoList] '{0}', '{1}', '{2}', '{3}'",
                    _BuCode,
                    vendorCode,
                    locationCode,
                    currencyCode));

                if (dtPoList != null && dtPoList.Rows.Count > 0)
                {
                    pop_PoList.ShowOnPageLoad = true;
                    lbl_PoList_Vendor.Text = string.Format("Vendor : {0}", ddl_Vendor.Text);
                    lbl_PoList_Currency.Text = string.Format("Currency : {0}", ddl_Currency.Text);

                    gv_PoList.DataSource = dtPoList;
                    gv_PoList.DataBind();

                }
                else
                {
                    var message = string.Format("No PO available for vendor '{0}' on location '{1}'", vendorCode, locationCode);
                    ShowAlert(message);
                }
            }



        }

        protected void btn_PoList_Ok_Click(object sender, EventArgs e)
        {
            var poList = new List<string>();

            foreach (GridViewRow row in gv_PoList.Rows)
            {
                var chk = row.FindControl("chk_PoItem") as CheckBox;
                var hf = row.FindControl("hf_PoList_PoNo") as HiddenField;

                if (chk.Checked)
                {
                    poList.Add(hf.Value.Trim());
                }
            }

            if (poList.Count == 0)
                return;

            AddPoToRecDt(poList);


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

                var poNo = DataBinder.Eval(dataItem, "PoNo") == DBNull.Value ? "" : DataBinder.Eval(dataItem, "PoNo").ToString();
                var poDtNo = DataBinder.Eval(dataItem, "PoDtNo") == DBNull.Value ? "0" : DataBinder.Eval(dataItem, "PoDtNo").ToString();

                var locationCode = DataBinder.Eval(dataItem, "LocationCode").ToString();
                var productCode = DataBinder.Eval(dataItem, "ProductCode").ToString();

                var unitCode = DataBinder.Eval(dataItem, "UnitCode").ToString();
                var orderQty = string.IsNullOrEmpty(DataBinder.Eval(dataItem, "OrderQty").ToString()) ? 0 : Convert.ToDecimal(DataBinder.Eval(dataItem, "OrderQty"));
                var focQty = string.IsNullOrEmpty(DataBinder.Eval(dataItem, "FocQty").ToString()) ? 0 : Convert.ToDecimal(DataBinder.Eval(dataItem, "FocQty"));
                var recQty = string.IsNullOrEmpty(DataBinder.Eval(dataItem, "RecQty").ToString()) ? 0 : Convert.ToDecimal(DataBinder.Eval(dataItem, "RecQty"));
                var rcvUnit = DataBinder.Eval(dataItem, "RcvUnit").ToString();
                var rate = string.IsNullOrEmpty(DataBinder.Eval(dataItem, "Rate").ToString()) ? 1 : Convert.ToDecimal(DataBinder.Eval(dataItem, "Rate"));
                var price = string.IsNullOrEmpty(DataBinder.Eval(dataItem, "Price").ToString()) ? 0 : Convert.ToDecimal(DataBinder.Eval(dataItem, "Price"));

                var discAdj = Convert.ToBoolean(DataBinder.Eval(dataItem, "DiscAdj"));
                var taxAdj = Convert.ToBoolean(DataBinder.Eval(dataItem, "TaxAdj"));
                var taxRate = Convert.ToDecimal(DataBinder.Eval(dataItem, "TaxRate"));

                var currDiscAmt = Convert.ToDecimal(DataBinder.Eval(dataItem, "CurrDiscAmt"));
                var currNetAmt = Convert.ToDecimal(DataBinder.Eval(dataItem, "CurrNetAmt"));
                var currTaxAmt = Convert.ToDecimal(DataBinder.Eval(dataItem, "CurrTaxAmt"));
                var currTotalAmt = Convert.ToDecimal(DataBinder.Eval(dataItem, "CurrTotalAmt"));

                var diccountAmt = Convert.ToDecimal(DataBinder.Eval(dataItem, "DiccountAmt"));
                var netAmt = Convert.ToDecimal(DataBinder.Eval(dataItem, "NetAmt"));
                var taxAmt = Convert.ToDecimal(DataBinder.Eval(dataItem, "TaxAmt"));
                var totalAmt = string.IsNullOrEmpty(DataBinder.Eval(dataItem, "TotalAmt").ToString()) ? 0 : Convert.ToDecimal(DataBinder.Eval(dataItem, "TotalAmt"));

                var extraCost = string.IsNullOrEmpty(DataBinder.Eval(dataItem, "ExtraCost").ToString()) ? 0 : Convert.ToDecimal(DataBinder.Eval(dataItem, "ExtraCost"));

                var dtLocation = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery("SELECT LocationName FROM [IN].StoreLocation WHERE LocationCode=@code", new SqlParameter[] { new SqlParameter("code", locationCode) });
                var locationName = dtLocation.Rows.Count > 0 ? dtLocation.Rows[0][0].ToString() : "";

                var productItem = new ProductItem(productCode, hf_ConnStr.Value);
                var productDesc1 = productItem.ProductDesc1;
                var productDesc2 = productItem.ProductDesc2;
                var inventoryUnit = productItem.InventoryUnit;

                var displayFormatAmt = "0";
                var displayFormatQty = "0";

                for (int i = 1; i < _default.DigitAmt; i++)
                    displayFormatAmt += "0";

                displayFormatAmt = ",0." + displayFormatAmt;

                for (int i = 1; i < _default.DigitQty; i++)
                    displayFormatQty += "0";

                displayFormatQty = ",0." + displayFormatAmt;

                // Location
                if (e.Row.FindControl("lbl_Location") != null)
                {
                    var label = e.Row.FindControl("lbl_Location") as Label;

                    label.Text = string.Format("{0} : {1}", locationCode, locationName);
                }

                if (e.Row.FindControl("ddl_Location") != null)
                {
                    var ddl = e.Row.FindControl("ddl_Location") as ASPxComboBox;

                    ddl.Items.Clear();
                    ddl.Items.AddRange(GetLocations(locationCode).ToArray());
                }


                // Product
                if (e.Row.FindControl("lbl_Product") != null)
                {
                    var label = e.Row.FindControl("lbl_Product") as Label;

                    label.Text = string.Format("{0} : {1} - {2}", productCode, productDesc1, productDesc2);
                }

                if (e.Row.FindControl("ddl_Product") != null)
                {
                    var ddl = e.Row.FindControl("ddl_Product") as ASPxComboBox;

                    ddl.Items.Clear();
                    ddl.Items.AddRange(GetProductsOnLocation(locationCode, productCode).ToArray());

                }

                if (e.Row.FindControl("hf_ProductCode") != null)
                {
                    var hf = e.Row.FindControl("hf_ProductCode") as HiddenField;

                    hf.Value = productCode;
                }

                // Order Unit
                if (e.Row.FindControl("lbl_OrdUnit") != null)
                {
                    var label = e.Row.FindControl("lbl_OrdUnit") as Label;

                    label.Text = string.IsNullOrEmpty(unitCode) ? productItem.InventoryUnit : unitCode;
                }

                // Order
                if (e.Row.FindControl("lbl_OrdQty") != null)
                {
                    var label = e.Row.FindControl("lbl_OrdQty") as Label;

                    label.Text = FormatQty(orderQty);

                }

                if (e.Row.FindControl("hf_OrdQty") != null)
                {
                    var hf = e.Row.FindControl("hf_OrdQty") as HiddenField;

                    hf.Value = FormatQty(orderQty);
                }

                // Receive Unit
                if (e.Row.FindControl("lbl_RcvUnit") != null)
                {
                    var label = e.Row.FindControl("lbl_OrdUnit") as Label;

                    label.Text = rcvUnit;
                }

                if (e.Row.FindControl("ddl_RcvUnit") != null)
                {
                    var ddl = e.Row.FindControl("ddl_RcvUnit") as DropDownList;

                    ddl.Items.Clear();

                    if (!string.IsNullOrEmpty(productCode))
                    {
                        ddl.Items.AddRange(productItem.Units
                            .Select(x => new ListItem
                            {
                                Text = x.UnitCode,
                                Value = x.UnitRate.ToString()
                            })
                            .ToArray());
                    }

                    ddl.Text = rcvUnit;

                }


                // Receive
                if (e.Row.FindControl("lbl_RecQty") != null)
                {
                    var label = e.Row.FindControl("lbl_RecQty") as Label;

                    label.Text = FormatQty(recQty);
                }

                if (e.Row.FindControl("se_RecQty") != null)
                {
                    var se = e.Row.FindControl("se_RecQty") as ASPxSpinEdit;

                    se.Value = recQty;
                    se.DecimalPlaces = _default.DigitQty;
                }

                if (e.Row.FindControl("hf_RecQty") != null)
                {
                    var hf = e.Row.FindControl("hf_RecQty") as HiddenField;

                    hf.Value = recQty.ToString();
                }

                // Foc
                if (e.Row.FindControl("lbl_FocQty") != null)
                {
                    var label = e.Row.FindControl("lbl_FocQty") as Label;

                    label.Text = FormatQty(focQty);
                }

                if (e.Row.FindControl("se_FocQty") != null)
                {
                    var se = e.Row.FindControl("se_FocQty") as ASPxSpinEdit;

                    se.Value = focQty;
                    se.DecimalPlaces = _default.DigitQty;
                }

                // Price
                if (e.Row.FindControl("lbl_Price") != null)
                {
                    var label = e.Row.FindControl("lbl_Price") as Label;

                    label.Text = FormatAmt(price);
                }

                if (e.Row.FindControl("se_Price") != null)
                {
                    var se = e.Row.FindControl("se_Price") as ASPxSpinEdit;

                    se.Value = price;
                    se.DecimalPlaces = _default.DigitAmt;

                }

                if (e.Row.FindControl("hf_Price") != null)
                {
                    var hf = e.Row.FindControl("hf_Price") as HiddenField;

                    hf.Value = price.ToString();
                }


                // Discount
                if (e.Row.FindControl("lbl_CurrDiscAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_CurrDiscAmt") as Label;

                    label.Text = FormatAmt(currDiscAmt);
                }
                // Net
                if (e.Row.FindControl("lbl_CurrNetAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_CurrNetAmt") as Label;

                    label.Text = FormatAmt(currNetAmt);
                }
                // Tax
                if (e.Row.FindControl("lbl_CurrTaxAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_CurrTaxAmt") as Label;

                    label.Text = FormatAmt(currTaxAmt);
                }

                // Extra Cost
                if (e.Row.FindControl("lbl_ExtCost") != null)
                {
                    var label = e.Row.FindControl("lbl_ExtCost") as Label;

                    label.Text = FormatAmt(extraCost);
                }
                // Total
                if (e.Row.FindControl("lbl_CurrTotalAmt_Row") != null)
                {
                    var label = e.Row.FindControl("lbl_CurrTotalAmt_Row") as Label;

                    label.Text = FormatAmt(currTotalAmt);
                }
                // Total
                if (e.Row.FindControl("lbl_TotalAmt_Row") != null)
                {
                    var label = e.Row.FindControl("lbl_TotalAmt_Row") as Label;

                    //label.Text = FormatAmt(totalAmt);
                }
                // Expiry date
                if (e.Row.FindControl("lbl_ExpiryDate") != null)
                {
                    var label = e.Row.FindControl("lbl_ExpiryDate") as Label;

                    //label.Text = FormatDate(DataBinder.Eval(e.Row.DataItem, "ExpiryDate"));
                }

                if (e.Row.FindControl("de_ExpiryDate") != null)
                {
                    var de = e.Row.FindControl("de_ExpiryDate") as ASPxDateEdit;
                    //DataBinder.Eval(dataItem, "ExpiryDate") 
                    //de.Text = FormatDate(DataBinder.Eval(e.Row.DataItem, "ExpiryDate"));
                }

                if (e.Row.FindControl("chk_AdjTax") != null)
                {
                    var chk = e.Row.FindControl("chk_AdjTax") as CheckBox;

                    chk.Checked = taxAdj;

                }



                if (e.Row.FindControl("lbl_DiscAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_DiscAmt") as Label;

                    label.Text = FormatAmt(diccountAmt);
                }
                if (e.Row.FindControl("lbl_TaxAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_TaxAmt") as Label;

                    label.Text = FormatAmt(taxAmt);
                }
                if (e.Row.FindControl("lbl_NetAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_NetAmt") as Label;

                    label.Text = FormatAmt(netAmt);
                }
                if (e.Row.FindControl("lbl_TotalAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_TotalAmt") as Label;

                    label.Text = FormatAmt(totalAmt);
                }




                #region --Additional--
                // Base Qty
                if (e.Row.FindControl("lbl_BaseQty") != null)
                {

                    var dt = sql.ExecuteQuery(string.Format("SELECT TOP(1) [Rate] FROM [IN].ProdUnit WHERE UnitType IN ('I','O') AND ProductCode='{0}' AND OrderUnit='{1}' ", productCode, unitCode));
                    var unitRate = dt != null && dt.Rows.Count > 0 ? Convert.ToDecimal(dt.Rows[0][0]) : 0m;
                    var qty = FormatQty(orderQty * unitRate);

                    var text = string.Format("{0} {1} (rate : {2})",
                        qty,
                        inventoryUnit, //DataBinder.Eval(e.Row.DataItem, "InventoryUnit").ToString(),
                        unitRate);

                    var label = e.Row.FindControl("lbl_BaseQty") as Label;

                    label.Text = text;
                }

                // Discount

                if (e.Row.FindControl("chk_AdjDisc") != null)
                {
                    var chk = e.Row.FindControl("chk_AdjDisc") as CheckBox;

                    chk.Checked = discAdj;
                }

                if (e.Row.FindControl("hf_DiscAdj") != null)
                {
                    var hf = e.Row.FindControl("hf_DiscAdj") as HiddenField;

                    hf.Value = discAdj.ToString();
                }

                if (e.Row.FindControl("hf_Discount") != null)
                {
                    var hf = e.Row.FindControl("hf_Discount") as HiddenField;

                    hf.Value = DataBinder.Eval(e.Row.DataItem, "Discount").ToString();
                }

                if (e.Row.FindControl("se_DiscRate") != null)
                {
                    var se = e.Row.FindControl("se_DiscRate") as ASPxSpinEdit;

                    se.Value = currDiscAmt;
                    se.DecimalPlaces = _default.DigitAmt;
                    //se.DisplayFormatString = displayFormatAmt;
                    se.Enabled = discAdj;
                }

                if (e.Row.FindControl("se_CurrDiscAmt") != null)
                {
                    var se = e.Row.FindControl("se_CurrDiscAmt") as ASPxSpinEdit;

                    se.Value = currDiscAmt;
                    se.DecimalPlaces = _default.DigitAmt;
                    se.Enabled = taxAdj;
                    se.DisplayFormatString = displayFormatAmt;
                }



                // Tax

                if (e.Row.FindControl("hf_TaxAdj") != null)
                {
                    var hf = e.Row.FindControl("hf_TaxAdj") as HiddenField;

                    hf.Value = DataBinder.Eval(e.Row.DataItem, "TaxAdj").ToString();
                }


                if (e.Row.FindControl("ddl_TaxType") != null)
                {
                    var ddl = e.Row.FindControl("ddl_TaxType") as DropDownList;
                    var value = DataBinder.Eval(dataItem, "TaxType").ToString();

                    ddl.SelectedValue = value;


                }

                if (e.Row.FindControl("hf_TaxType") != null)
                {
                    var hf = e.Row.FindControl("hf_TaxType") as HiddenField;

                    hf.Value = DataBinder.Eval(e.Row.DataItem, "TaxType").ToString();
                }

                if (e.Row.FindControl("hf_TaxRate") != null)
                {
                    var hf = e.Row.FindControl("hf_TaxRate") as HiddenField;

                    hf.Value = DataBinder.Eval(e.Row.DataItem, "TaxRate").ToString();
                }

                if (e.Row.FindControl("se_TaxRate") != null)
                {
                    var se = e.Row.FindControl("se_TaxRate") as ASPxSpinEdit;

                    se.Value = taxRate;
                    se.DecimalPlaces = _default.DigitAmt;
                    se.Enabled = taxAdj;
                }
                if (e.Row.FindControl("se_CurrTaxAmt") != null)
                {
                    var se = e.Row.FindControl("se_CurrTaxAmt") as ASPxSpinEdit;

                    se.Value = currTaxAmt;
                    se.DecimalPlaces = _default.DigitAmt;
                    se.DisplayFormatString = displayFormatAmt;
                }

                // Net

                if (e.Row.FindControl("se_CurrNetAmt") != null)
                {
                    var se = e.Row.FindControl("se_CurrNetAmt") as ASPxSpinEdit;

                    se.Value = currNetAmt;
                    se.DecimalPlaces = _default.DigitAmt;
                    se.ReadOnly = true;
                    se.DisplayFormatString = displayFormatAmt;
                }

                // Total


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
	ISNULL(SUM(ISNULL(podt.OrdQty,0) - ISNULL(RcvQty,0)), 0) as Qty
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

        protected void gv_Detail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            var gv = sender as GridView;

            for (var i = _dtRecDt.Rows.Count - 1; i >= 0; i--)
            {
                var dr = _dtRecDt.Rows[i];
                if (dr.RowState == DataRowState.Modified || dr.RowState == DataRowState.Added)
                    dr.Delete();
            }

            gv.DataSource = _dtRecDt;
            gv.EditIndex = -1;
            gv.DataBind();

            SetEditItem(false);
        }

        protected void gv_Detail_RowEditing(object sender, GridViewEditEventArgs e)
        {
            var gv = sender as GridView;

            gv.EditRowStyle.BackColor = Color.FromArgb(254, 249, 231);
            gv.EditIndex = e.NewEditIndex;
            gv.DataSource = _dtRecDt;
            gv.DataBind();

            var row = gv.Rows[e.NewEditIndex];

            var Img_Btn = row.FindControl("Img_Btn") as ImageButton;
            var ddl_Location = row.FindControl("ddl_Location") as ASPxComboBox;

            Img_Btn.Visible = false;




            SetEditItem(true);

            //ShowHideColumns(gv, true);
        }

        protected void gv_Detail_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var gv = sender as GridView;
            var row = gv.Rows[e.RowIndex];

            DataRow dr = _dtRecDt.Rows[e.RowIndex];

            var se_RecQty = row.FindControl("se_RecQty") as ASPxSpinEdit;
            var se_FocQty = row.FindControl("se_FocQty") as ASPxSpinEdit;
            var se_Price = row.FindControl("se_Price") as ASPxSpinEdit;

            var chk_AdjDisc = row.FindControl("chk_AdjDisc") as CheckBox;
            var se_DiscRate = row.FindControl("se_DiscRate") as ASPxSpinEdit;

            var chk_AdjTax = row.FindControl("chk_AdjTax") as CheckBox;
            var ddl_TaxType = row.FindControl("ddl_TaxType") as DropDownList;
            var se_TaxRate = row.FindControl("se_TaxRate") as ASPxSpinEdit;

            if (IsCreatedManual())
            {
                var ddl_Location = row.FindControl("ddl_Location") as ASPxComboBox;
                var ddl_Product = row.FindControl("ddl_Product") as ASPxComboBox;
                var hf_Rate = row.FindControl("hf_Rate") as HiddenField;
                var ddl_RcvUnit = row.FindControl("ddl_RcvUnit") as DropDownList;

                dr["LocationCode"] = ddl_Location.SelectedItem.Value.ToString(); ;
                dr["ProductCode"] = ddl_Product.SelectedItem.Value.ToString(); ;
                dr["RcvUnit"] = ddl_RcvUnit.SelectedItem.Text;
                dr["Rate"] = Convert.ToDecimal(ddl_RcvUnit.SelectedItem.Value);
            }

            //dr["OrderQty"] = orderQty;
            //dr["UnitCode"] = unitCode;

            dr["FocQty"] = Convert.ToDecimal(se_FocQty.Value); ;
            dr["RecQty"] = Convert.ToDecimal(se_RecQty.Value);
            dr["Price"] = Convert.ToDecimal(se_Price.Value);

            dr["DiscAdj"] = chk_AdjDisc.Checked;
            dr["Discount"] = Convert.ToDecimal(se_DiscRate.Value);

            dr["TaxAdj"] = chk_AdjTax.Checked;
            dr["TaxType"] = ddl_TaxType.SelectedItem.Value;
            dr["TaxRate"] = Convert.ToDecimal(se_TaxRate.Value);


            var se_CurrDiscAmt = row.FindControl("se_CurrDiscAmt") as ASPxSpinEdit;
            var se_CurrTaxAmt = row.FindControl("se_CurrTaxAmt") as ASPxSpinEdit;
            var se_CurrNetAmt = row.FindControl("se_CurrNetAmt") as ASPxSpinEdit;
            var lbl_CurrTotalAmt_Row = row.FindControl("lbl_CurrTotalAmt_Row") as Label;


            var lbl_DiscAmt = row.FindControl("lbl_DiscAmt") as Label;
            var lbl_TaxAmt = row.FindControl("lbl_TaxAmt") as Label;
            var lbl_NetAmt = row.FindControl("lbl_NetAmt") as Label;
            var lbl_TotalAmt = row.FindControl("lbl_TotalAmt") as Label;

            dr["CurrDiscAmt"] = Convert.ToDecimal(se_CurrDiscAmt.Value);
            dr["CurrNetAmt"] = Convert.ToDecimal(se_CurrNetAmt.Value);
            dr["CurrTaxAmt"] = Convert.ToDecimal(se_CurrTaxAmt.Value);
            dr["CurrTotalAmt"] = Convert.ToDecimal(lbl_CurrTotalAmt_Row.Text);


            dr["DiccountAmt"] = Convert.ToDecimal(lbl_DiscAmt.Text.Trim());
            dr["NetAmt"] = Convert.ToDecimal(lbl_NetAmt.Text.Trim());
            dr["TaxAmt"] = Convert.ToDecimal(lbl_TaxAmt.Text.Trim());
            dr["TotalAmt"] = Convert.ToDecimal(lbl_TotalAmt.Text.Trim());

            var de_ExpiryDate = row.FindControl("de_ExpiryDate") as ASPxDateEdit;
            var txt_Comment = row.FindControl("txt_Comment") as TextBox;

            if (string.IsNullOrEmpty(de_ExpiryDate.Text))
                dr["ExpiryDate"] = DBNull.Value;
            else
                dr["ExpiryDate"] = de_ExpiryDate.Date;
            dr["Comment"] = txt_Comment.Text;
            //dr["PoNo"] = poNo;
            //dr["PoDtNo"] = poDtNo;

            gv.EditIndex = -1;
            BindDetails();

            SetEditItem(false);
        }

        protected void gv_Detail_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var gv = sender as GridView;

            var hf_RecDtNo = gv.Rows[e.RowIndex].FindControl("hf_RecDtNo") as HiddenField;
            var recDtNo = Convert.ToInt32(hf_RecDtNo.Value);
            var item = _dtRecDt.AsEnumerable().FirstOrDefault(x => x.Field<int>("RecDtNo") == recDtNo);

            if (item != null)
                item.Delete();

            BindDetails();
        }


        // Controls on gv_Detail
        protected void ddl_Location_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;
            var row = ddl.NamingContainer;

            var ddl_Product = row.FindControl("ddl_Product") as ASPxComboBox;

            var locationCode = ddl.Value.ToString();
            var productCode = ddl_Product.SelectedIndex >= 0 ? ddl_Product.Value.ToString() : "";

            ddl_Product.Items.Clear();
            ddl_Product.Items.AddRange(GetProductsOnLocation(locationCode, productCode).ToArray());

            if (ddl_Product.SelectedIndex < 0)
            {
                ddl_Product.Value = "";
            }
        }

        protected void ddl_Product_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;
            var row = ddl.NamingContainer;

            var hf_ProductCode = row.FindControl("hf_ProductCode") as HiddenField;
            var hf_UnitCode = row.FindControl("hf_UnitCode") as HiddenField;
            var hf_Rate = row.FindControl("hf_Rate") as HiddenField;

            var ddl_RcvUnit = row.FindControl("ddl_RcvUnit") as DropDownList;
            var ddl_TaxType = row.FindControl("ddl_TaxType") as DropDownList;
            var se_TaxRate = row.FindControl("se_TaxRate") as ASPxSpinEdit;

            var productCode = ddl.SelectedItem.Value.ToString();

            var item = new ProductItem(productCode, hf_ConnStr.Value);

            hf_ProductCode.Value = productCode;
            hf_UnitCode.Value = item.InventoryUnit;

            ddl_TaxType.SelectedValue = item.TaxType;
            se_TaxRate.Number = item.TaxRate;


            var sql = new Helpers.SQL(hf_ConnStr.Value);

            var dtOrderUnit = sql.ExecuteQuery("SELECT OrderUnit as [Text], CAST([Rate] as VARCHAR) as Value FROM [IN].ProdUnit WHERE UnitType='O' AND ProductCode=@code ORDER BY IsDefault DESC, OrderUnit", new SqlParameter[] { new SqlParameter("code", productCode) });
            ddl_RcvUnit.Items.Clear();
            if (dtOrderUnit != null && dtOrderUnit.Rows.Count > 0)
            {
                ddl_RcvUnit.Items.AddRange(dtOrderUnit.AsEnumerable()
                    .Select(x => new ListItem
                    {
                        Value = x.Field<string>("Value"),
                        Text = x.Field<string>("Text"),

                    })
                    .ToArray());
            }


            CalculateItem(ddl);





        }

        protected void se_RecQty_NumberChanged(object sender, EventArgs e)
        {
            var item = sender as ASPxSpinEdit;
            var row = item.NamingContainer as GridViewRow;

            if (!IsCreatedManual()) // created from PO, control deviation
            {
                var hf_ProductCode = row.FindControl("hf_ProductCode") as HiddenField;
                var hf_OrdQty = row.FindControl("hf_OrdQty") as HiddenField;

                var productCode = hf_ProductCode.Value;
                var qty = string.IsNullOrEmpty(hf_OrdQty.Value) ? 0m : Convert.ToDecimal(hf_OrdQty.Value);
                var rate = GetDeviation(productCode);

                var devQty = qty * rate.Qty / 100;

                var maxQty = qty + devQty;

                var recQty = (decimal)item.Value;


                if (recQty > maxQty)
                {
                    item.Text = hf_OrdQty.Value;
                    ShowAlert(string.Format("Quantity is over deivation.<br />Maxiumum is {0}", FormatQty(maxQty)));

                    return;
                }
            }

            CalculateItem(sender);

        }

        protected void se_Price_NumberChanged(object sender, EventArgs e)
        {
            var item = sender as ASPxSpinEdit;
            var row = item.NamingContainer as GridViewRow;

            if (!IsCreatedManual()) // created from PO, control deviation
            {
                var hf_ProductCode = row.FindControl("hf_ProductCode") as HiddenField;
                var hf_Price = row.FindControl("hf_Price") as HiddenField;

                var productCode = hf_ProductCode.Value;
                var price = string.IsNullOrEmpty(hf_Price.Value) ? 0m : Convert.ToDecimal(hf_Price.Value);
                var rate = GetDeviation(productCode);

                var devPrice = price * rate.Price / 100;
                var maxPrice = price + devPrice;

                var price1 = (decimal)item.Value;

                if (price1 > maxPrice)
                {
                    item.Text = hf_Price.Value;
                    ShowAlert(string.Format("Price is over deivation.<br />Maxiumum is {0}", FormatAmt(maxPrice)));
                }
            }

            CalculateItem(sender);
        }

        protected void chk_AdjDisc_CheckedChanged(object sender, EventArgs e)
        {
            var item = sender as CheckBox;
            var row = item.NamingContainer as GridViewRow;

            var se_DiscRate = row.FindControl("se_DiscRate") as ASPxSpinEdit;
            var se_CurrDiscAmt = row.FindControl("se_CurrDiscAmt") as ASPxSpinEdit;

            se_DiscRate.Enabled = item.Checked;

            if (!item.Checked)
            {
                var hf_Discount = row.FindControl("hf_Discount") as HiddenField;

                se_DiscRate.Value = hf_Discount.Value;
            }


            se_CurrDiscAmt.ReadOnly = !item.Checked;

        }

        protected void se_DiscRate_NumberChanged(object sender, EventArgs e)
        {
            var se = sender as ASPxSpinEdit;
            var row = se.NamingContainer as GridViewRow;

            //if (!IsCreatedManual()) // created from PO, control deviation
            //{
            //    var hf_ProductCode = row.FindControl("hf_ProductCode") as HiddenField;
            //    var hf_Price = row.FindControl("hf_Price") as HiddenField;

            //    var productCode = hf_ProductCode.Value;
            //    var price = string.IsNullOrEmpty(hf_Price.Value) ? 0m : Convert.ToDecimal(hf_Price.Value);
            //    var rate = GetDeviation(productCode);

            //    var devPrice = price * rate.Price / 100;
            //    var maxPrice = price + devPrice;

            //    var price1 = (decimal)se.Value;

            //    if (price1 > maxPrice)
            //    {
            //        se.Text = hf_Price.Value;
            //        ShowAlert(string.Format("Price is over deivation.<br />Maxiumum is {0}", FormatAmt(maxPrice)));
            //    }
            //}
            CalculateItem(se);


        }

        protected void chk_AdjTax_CheckedChanged(object sender, EventArgs e)
        {
            var item = sender as CheckBox;
            var row = item.NamingContainer as GridViewRow;

            var isManual = string.IsNullOrEmpty(_dtRec.Rows[0]["PoSource"].ToString());


            var ddl_TaxType = row.FindControl("ddl_TaxType") as DropDownList;
            var se_TaxRate = row.FindControl("se_TaxRate") as ASPxSpinEdit;
            var se_CurrTaxAmt = row.FindControl("se_CurrTaxAmt") as ASPxSpinEdit;

            //if (isManual)
            //{
            //ddl_TaxType.Enabled = item.Checked;
            //se_TaxRate.Enabled = item.Checked;
            //}
            ddl_TaxType.Enabled = item.Checked;
            se_TaxRate.Enabled = item.Checked;

            se_CurrTaxAmt.Enabled = item.Checked;

            if (!item.Checked)
            {
                var hf_TaxType = row.FindControl("hf_TaxType") as HiddenField;
                var hf_TaxRate = row.FindControl("hf_TaxRate") as HiddenField;

                ddl_TaxType.SelectedValue = hf_TaxType.Value;
                se_TaxRate.Value = hf_TaxRate.Value;

                CalculateItem(sender);
            }
            //var se_CurrTaxAmt = row.FindControl("se_CurrTaxAmt") as ASPxSpinEdit;

            //se_CurrTaxAmt.ReadOnly = !item.Checked;

        }

        protected void ddl_TaxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as DropDownList;
            var row = ddl.NamingContainer as GridViewRow;

            var se_TaxRate = row.FindControl("se_TaxRate") as ASPxSpinEdit;

            var taxType = ddl.SelectedValue.ToString();
            var taxRate = Convert.ToDecimal(se_TaxRate.Value);

            se_TaxRate.Enabled = taxType != "N";

            if (taxType == "N")
                se_TaxRate.Value = 0m;
            else
                se_TaxRate.Value = taxRate == 0 ? _default.TaxRate : taxRate;

            CalculateItem(ddl);

        }

        protected void se_TaxRate_NumberChanged(object sender, EventArgs e)
        {
            var item = sender as ASPxSpinEdit;
            //var row = item.NamingContainer as GridViewRow;


            CalculateItem(item);
        }

        protected void se_CurrTaxAmt_NumberChanged(object sender, EventArgs e)
        {
            var item = sender as ASPxSpinEdit;
            var row = item.NamingContainer as GridViewRow;

            var chk_AdjTax = row.FindControl("chk_AdjTax") as CheckBox;

            if (chk_AdjTax.Checked)
            {
                var se_CurrNetAmt = row.FindControl("se_CurrNetAmt") as ASPxSpinEdit;
                var se_CurrTaxAmt = row.FindControl("se_CurrTaxAmt") as ASPxSpinEdit;
                var lbl_CurrTotalAmt_Dt = row.FindControl("lbl_CurrTotalAmt_Dt") as Label;

                var currTotalAmt = se_CurrNetAmt.Number + se_CurrTaxAmt.Number;

                lbl_CurrTotalAmt_Dt.Text = FormatAmt(currTotalAmt);

                SetBaseItems(item);
            }
            else
                CalculateItem(item);
        }

        protected void btn_CalTax_Click(object sender, EventArgs e)
        {
            var item = sender as Button;

            CalculateItem(item);
        }

        private void RecQtyChanged(ASPxSpinEdit se_RecQty)
        {
            var row = se_RecQty.NamingContainer as GridViewRow;

            var se_Price = row.FindControl("se_Price") as ASPxSpinEdit;

            var qty = Convert.ToDecimal(se_RecQty.Value);
            var price = Convert.ToDecimal(se_Price.Value);

            //var taxType = 




            var lbl_CurrNetAmt = row.FindControl("") as Label;
            var lbl_CurrTaxAmt = row.FindControl("") as Label;
            var lbl_CurrTotalAmt_Row = row.FindControl("") as Label;
            var lbl_ExtraCost = row.FindControl("") as Label;

        }

        private void CalculateItem(object sender)
        {
            var row = (sender as Control).NamingContainer;


            var se_RecQty = row.FindControl("se_RecQty") as ASPxSpinEdit;
            var se_Price = row.FindControl("se_Price") as ASPxSpinEdit;

            var chk_AdjDisc = row.FindControl("chk_AdjDisc") as CheckBox;
            var se_DiscRate = row.FindControl("se_DiscRate") as ASPxSpinEdit;

            var chk_AdjTax = row.FindControl("chk_AdjTax") as CheckBox;
            var ddl_TaxType = row.FindControl("ddl_TaxType") as DropDownList;
            var se_TaxRate = row.FindControl("se_TaxRate") as ASPxSpinEdit;

            var se_CurrDiscAmt = row.FindControl("se_CurrDiscAmt") as ASPxSpinEdit;
            var se_CurrNetAmt = row.FindControl("se_CurrNetAmt") as ASPxSpinEdit;
            var se_CurrTaxAmt = row.FindControl("se_CurrTaxAmt") as ASPxSpinEdit;

            var qty = Convert.ToDecimal(se_RecQty.Value);
            var price = Convert.ToDecimal(se_Price.Value);

            var adjDisc = chk_AdjDisc.Checked;
            var discRate = Convert.ToDecimal(se_DiscRate.Value);

            var adjTax = chk_AdjTax.Checked;
            var taxType = ddl_TaxType.SelectedValue.ToString();
            var taxRate = Convert.ToDecimal(se_TaxRate.Value);

            var totalPrice = RoundAmt(qty * price);


            var currDiscAmt = Convert.ToDecimal(se_CurrDiscAmt.Value);
            var currTaxAmt = Convert.ToDecimal(se_CurrTaxAmt.Value);
            var currNetAmt = 0m;
            var currTotalAmt = 0m;
            var total = 0m;

            currDiscAmt = RoundAmt(totalPrice * discRate / 100);

            //if (adjTax)
            //{
            //    currNetAmt = se_CurrNetAmt.Number;
            //    currTaxAmt = se_CurrTaxAmt.Number;

            //}
            //else
            if (taxType == "A")
            {
                currNetAmt = totalPrice - currDiscAmt;
                currTaxAmt = RoundAmt(currNetAmt * taxRate / 100);
            }
            else
            {
                total = totalPrice - currDiscAmt;

                currTaxAmt = RoundAmt(total * taxRate / (taxRate + 100));
                currNetAmt = total - currTaxAmt;
            }

            currTotalAmt = currNetAmt + currTaxAmt;

            // Display values

            var lbl_CurrNetAmt = row.FindControl("lbl_CurrNetAmt") as Label;
            var lbl_CurrTotalAmt_Row = row.FindControl("lbl_CurrTotalAmt_Row") as Label;

            //var lbl_CurrNetAmt_Dt = row.FindControl("lbl_CurrNetAmt_Dt") as Label;
            var lbl_CurrTotalAmt_Dt = row.FindControl("lbl_CurrTotalAmt_Dt") as Label;


            // Currency
            se_CurrDiscAmt.Number = currDiscAmt;
            se_CurrNetAmt.Number = currNetAmt;
            lbl_CurrNetAmt.Text = FormatAmt(currNetAmt);
            se_CurrTaxAmt.Number = currTaxAmt;
            lbl_CurrTotalAmt_Row.Text = FormatAmt(currTotalAmt);

            //lbl_CurrNetAmt_Dt.Text = lbl_CurrNetAmt.Text;
            lbl_CurrTotalAmt_Dt.Text = lbl_CurrTotalAmt_Row.Text;


            SetBaseItems(sender);
            //var lbl_DiscAmt = row.FindControl("lbl_DiscAmt") as Label;
            //var lbl_NetAmt = row.FindControl("lbl_NetAmt") as Label;
            //var lbl_TaxAmt = row.FindControl("lbl_TaxAmt") as Label;
            //var lbl_TotalAmt = row.FindControl("lbl_TotalAmt") as Label;


            //var currRate = Convert.ToDecimal(se_CurrencyRate.Value);
            //var discAmt = RoundAmt(currDiscAmt * currRate);
            //var netAmt = RoundAmt(currNetAmt * currRate);
            //var taxAmt = RoundAmt(currTaxAmt * currRate);
            //var totalAmt = RoundAmt(currTotalAmt * currRate);

            //// Base
            //lbl_DiscAmt.Text = FormatAmt(discAmt);
            //lbl_NetAmt.Text = FormatAmt(netAmt);
            //lbl_TaxAmt.Text = FormatAmt(taxAmt);
            //lbl_TotalAmt.Text = FormatAmt(totalAmt);

            se_CurrDiscAmt.ReadOnly = !chk_AdjDisc.Checked;
            se_CurrTaxAmt.ReadOnly = !chk_AdjTax.Checked;
        }

        private void SetBaseItems(object sender)
        {
            var row = (sender as Control).NamingContainer;

            var se_CurrDiscAmt = row.FindControl("se_CurrDiscAmt") as ASPxSpinEdit;
            var se_CurrNetAmt = row.FindControl("se_CurrNetAmt") as ASPxSpinEdit;
            var se_CurrTaxAmt = row.FindControl("se_CurrTaxAmt") as ASPxSpinEdit;
            var lbl_CurrTotalAmt_Dt = row.FindControl("lbl_CurrTotalAmt_Dt") as Label;

            var currTotalAmt = Convert.ToDecimal(lbl_CurrTotalAmt_Dt.Text);

            var currRate = Convert.ToDecimal(se_CurrencyRate.Value);
            var discAmt = RoundAmt(se_CurrDiscAmt.Number * currRate);
            var netAmt = RoundAmt(se_CurrNetAmt.Number * currRate);
            var taxAmt = RoundAmt(se_CurrTaxAmt.Number * currRate);
            var totalAmt = RoundAmt(currTotalAmt * currRate);

            // Base
            var lbl_DiscAmt = row.FindControl("lbl_DiscAmt") as Label;
            var lbl_NetAmt = row.FindControl("lbl_NetAmt") as Label;
            var lbl_TaxAmt = row.FindControl("lbl_TaxAmt") as Label;
            var lbl_TotalAmt = row.FindControl("lbl_TotalAmt") as Label;

            lbl_DiscAmt.Text = FormatAmt(discAmt);
            lbl_NetAmt.Text = FormatAmt(netAmt);
            lbl_TaxAmt.Text = FormatAmt(taxAmt);
            lbl_TotalAmt.Text = FormatAmt(totalAmt);

        }

        // pop_SessionTimeout
        protected void btn_SessionTimeout_Ok_Click(object sender, EventArgs e)
        {
            pop_SessionTimeout.ShowOnPageLoad = false;
            Response.Redirect("RecLst.aspx");
        }


        #endregion


        private string CreateHeader(DataTable dataTable)
        {
            DataRow dr = dataTable.Rows[0];
            var query = @"
DECLARE @dt AS TABLE( RecNo nvarchar(20) NOT NULL)
INSERT INTO @dt(RecNo) EXEC [PC].[RecGetNewID] @AtDate=@RecDate
DECLARE @RecNo nvarchar(20) = (SELECT TOP(1) RecNo FROM @dt)

INSERT INTO PC.REC (RecNo, RecDate, VendorCode, InvoiceNo, InvoiceDate, [Description], PoSource, DeliPoint, CurrencyCode, CurrencyRate, TotalExtraCost, ExtraCostBy, IsCashConsign, DocStatus, ExportStatus, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate)
OUTPUT Inserted.RecNo
VALUES (@RecNo, @RecDate, @VendorCode, @InvoiceNo, @InvoiceDate, @Description, @PoSource, @DeliPoint, @CurrencyCode, @CurrencyRate, @TotalExtraCost, @ExtraCostBy, @IsCashConsign, @DocStatus, 0, @Username, GETDATE(), @Username, GETDATE())
";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("RecDate", dr["RecDate"]),
                new SqlParameter("VendorCode", dr["VendorCode"].ToString()),
                new SqlParameter("InvoiceNo", dr["InvoiceNo"].ToString()),
                new SqlParameter("InvoiceDate", dr["InvoiceDate"]),
                new SqlParameter("Description", dr["Description"]),
                new SqlParameter("PoSource", dr["PoSource"]),
                new SqlParameter("DeliPoint",dr["DeliPoint"]),
                new SqlParameter("CurrencyCode",dr["CurrencyCode"]),
                new SqlParameter("CurrencyRate",dr["CurrencyRate"]),
                new SqlParameter("TotalExtraCost",dr["TotalExtraCost"]),
                new SqlParameter("ExtraCostBy",dr["ExtraCostBy"]),
                new SqlParameter("IsCashConsign",dr["IsCashConsign"]),
                new SqlParameter("DocStatus",dr["DocStatus"]),
                new SqlParameter("Username", LoginInfo.LoginName)
            };

            var dt = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery(query, parameters);


            var recNo = dt != null && dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "";

            return recNo;
        }

        private void CreateDetails(string recNo, DataTable dt)
        {
            new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery("DELETE FROM PC.RecDt WHERE RecNo=@RecNo", new SqlParameter[] { new SqlParameter("RecNo", recNo) });

            var query = @"
INSERT INTO PC.RECDt (
	RecNo, 
	RecDtNo, 
	LocationCode, 
	ProductCode, 
	UnitCode, 
	OrderQty, 
	FOCQty, 
    RecQty,
	RcvUnit, 
	Rate, 
	Price, 
	
	DiscAdj,
	Discount,
	TaxAdj,
	TaxType,
	TaxRate,
	CurrDiscAmt,
	CurrTaxAmt,
	CurrNetAmt,
	CurrTotalAmt,
	DiccountAmt,
	TaxAmt,
	NetAmt,
	TotalAmt,

	ExpiryDate,
	ExtraCost,
	PoNo,
	PoDtNo,
	ExportStatus,
	Comment
)
VALUES ";

            var values = new StringBuilder();
            var parameters = new List<SqlParameter>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i].RowState == DataRowState.Deleted)
                    continue;

                var dr = dt.Rows[i];
                var dtNo = i + 1;

                values.AppendFormat(
@"(
	@RecNo, 
	{0}, 
	@LocationCode{0}, 
	@ProductCode{0}, 
	@UnitCode{0}, 
	@OrderQty{0}, 
	@FOCQty{0}, 
	@RecQty{0}, 
	@RcvUnit{0}, 
	@Rate{0}, 
	@Price{0}, 
	
	@DiscAdj{0},
	@Discount{0},
	@TaxAdj{0},
	@TaxType{0},
	@TaxRate{0},
	@CurrDiscAmt{0},
	@CurrTaxAmt{0},
	@CurrNetAmt{0},
	@CurrTotalAmt{0},
	@DiccountAmt{0},
	@TaxAmt{0},
	@NetAmt{0},
	@TotalAmt{0},

	@ExpiryDate{0},
	@ExtraCost{0},
	@PoNo{0},
	@PoDtNo{0},
	0,
	@Comment{0}),", dtNo.ToString());
                var taxAdj = Convert.ToBoolean(dr["TaxAdj"]); // ? "1" : "0";

                parameters.Add(new SqlParameter(string.Format("LocationCode{0}", dtNo), dr["LocationCode"].ToString()));
                parameters.Add(new SqlParameter(string.Format("ProductCode{0}", dtNo), dr["ProductCode"].ToString()));
                parameters.Add(new SqlParameter(string.Format("UnitCode{0}", dtNo), dr["UnitCode"].ToString()));
                parameters.Add(new SqlParameter(string.Format("OrderQty{0}", dtNo), dr["OrderQty"]));
                parameters.Add(new SqlParameter(string.Format("FOCQty{0}", dtNo), dr["FocQty"]));
                parameters.Add(new SqlParameter(string.Format("RecQty{0}", dtNo), dr["RecQty"]));
                parameters.Add(new SqlParameter(string.Format("RcvUnit{0}", dtNo), dr["RcvUnit"]));
                parameters.Add(new SqlParameter(string.Format("Rate{0}", dtNo), dr["Rate"]));
                parameters.Add(new SqlParameter(string.Format("Price{0}", dtNo), dr["Price"]));
                parameters.Add(new SqlParameter(string.Format("DiscAdj{0}", dtNo), dr["DiscAdj"]));
                parameters.Add(new SqlParameter(string.Format("Discount{0}", dtNo), dr["Discount"]));
                //parameters.Add(new SqlParameter(string.Format("TaxAdj{0}", dtNo), dr["TaxAdj"]));
                parameters.Add(new SqlParameter(string.Format("TaxAdj{0}", dtNo), taxAdj));
                parameters.Add(new SqlParameter(string.Format("TaxType{0}", dtNo), dr["TaxType"].ToString()));
                parameters.Add(new SqlParameter(string.Format("TaxRate{0}", dtNo), dr["TaxRate"]));
                parameters.Add(new SqlParameter(string.Format("CurrDiscAmt{0}", dtNo), dr["CurrDiscAmt"]));
                parameters.Add(new SqlParameter(string.Format("CurrTaxAmt{0}", dtNo), dr["CurrTaxAmt"]));
                parameters.Add(new SqlParameter(string.Format("CurrNetAmt{0}", dtNo), dr["CurrNetAmt"]));
                parameters.Add(new SqlParameter(string.Format("CurrTotalAmt{0}", dtNo), dr["CurrTotalAmt"]));
                parameters.Add(new SqlParameter(string.Format("DiccountAmt{0}", dtNo), dr["DiccountAmt"]));
                parameters.Add(new SqlParameter(string.Format("TaxAmt{0}", dtNo), dr["TaxAmt"]));
                parameters.Add(new SqlParameter(string.Format("NetAmt{0}", dtNo), dr["NetAmt"]));
                parameters.Add(new SqlParameter(string.Format("TotalAmt{0}", dtNo), dr["TotalAmt"]));
                parameters.Add(new SqlParameter(string.Format("ExpiryDate{0}", dtNo), dr["ExpiryDate"]));
                parameters.Add(new SqlParameter(string.Format("ExtraCost{0}", dtNo), dr["ExtraCost"]));
                parameters.Add(new SqlParameter(string.Format("PoNo{0}", dtNo), dr["PoNo"]));
                parameters.Add(new SqlParameter(string.Format("PoDtNo{0}", dtNo), dr["PoDtNo"]));
                parameters.Add(new SqlParameter(string.Format("Comment{0}", dtNo), dr["Comment"]));



            }
            parameters.Add(new SqlParameter("RecNo", recNo));

            query = query + values.ToString().TrimEnd(',');

            try
            {
                var dtResult = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery(query, parameters.ToArray());

                UpdateUnitRate(recNo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SaveHeader(DataTable dataTable)
        {
            DataRow dr = dataTable.Rows[0];
            var query = @"
UPDATE
    PC.Rec
SET
    RecDate=@RecDate,
    VendorCode=@VendorCode,
    InvoiceNo=@InvoiceNo,
    InvoiceDate=@InvoiceDate,
    [Description]=@Description,
    DeliPoint=@DeliPoint,
    CurrencyCode=@CurrencyCode,
    CurrencyRate=@CurrencyRate,
    TotalExtraCost=@TotalExtraCost,
    ExtraCostBy=@ExtraCostBy,
    IsCashConsign=@IsCashConsign,
    UpdatedBy=@Username,
    UpdatedDate=GETDATE()
WHERE
    RecNo=@RecNo
";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("RecNo", dr["RecNo"]),
                new SqlParameter("RecDate", dr["RecDate"]),
                new SqlParameter("VendorCode", dr["VendorCode"].ToString()),
                new SqlParameter("InvoiceNo", dr["InvoiceNo"].ToString()),
                new SqlParameter("InvoiceDate", dr["InvoiceDate"]),
                new SqlParameter("Description", dr["Description"]),
                new SqlParameter("DeliPoint",dr["DeliPoint"]),
                new SqlParameter("CurrencyCode",dr["CurrencyCode"]),
                new SqlParameter("CurrencyRate",dr["CurrencyRate"]),
                new SqlParameter("TotalExtraCost",dr["TotalExtraCost"]),
                new SqlParameter("ExtraCostBy",dr["ExtraCostBy"]),
                new SqlParameter("IsCashConsign",dr["IsCashConsign"]),
                new SqlParameter("Username", LoginInfo.LoginName)
            };

            new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery(query, parameters);
        }

        private void UpdatePO(string recNo)
        {
            var query = @"
DECLARE @po AS TABLE(
	PoNo nvarchar(20) NOT NULL,
	PoDtNo INT NOT NULL,
	RecQty decimal(18,3) default 0,
	FocQty decimal(18,3) default 0,

	PRIMARY KEY (PoNo, PoDtNo)
)

;WITH
po AS(
	SELECT
		PoNo,
		PoDtNo,
		RecQty
	FROM
		PC.RECDt
	WHERE
		RecNo=@RecNo
)
INSERT INTO @po (PoNo, PoDtNo, RecQty, FocQty)
SELECT
	recdt.PoNo,
	recdt.PoDtNo,
	SUM(recdt.RecQty) as RecQty,
	SUM(recdt.FocQty) as FocQty
FROM
	PC.Rec
	JOIN PC.RecDt
		ON recdt.RecNo=rec.RecNo
	JOIN po
		ON po.PoNo=recdt.PoNo AND po.PoDtNo=recdt.PoDtNo
WHERE
	rec.DocStatus <> 'Voided'
GROUP BY
	recdt.PoNo,
	recdt.PoDtNo

UPDATE 
	PC.PoDt
SET
	RcvQty=po.RecQty
FROM
	PC.PoDt
	JOIN @po po
		ON po.PoNo=podt.PoNo AND po.PoDtNo=podt.PoDt
";
            new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery(query, new SqlParameter[] { new SqlParameter("RecNo", recNo) });
        }

        private void RestorePO(string recNo)
        {
            var query = @"
DECLARE @po AS TABLE(
	PoNo nvarchar(20) NOT NULL,
	PoDtNo INT NOT NULL,
	RecQty decimal(18,3) default 0,

	PRIMARY KEY (PoNo, PoDtNo)
)

INSERT INTO @po (PoNo, PoDtNo)
SELECT
	PoNo,
	PoDtNo
FROM
	PC.RECDt
WHERE
	RecNo=@RecNo
GROUP BY
	PoNo,
	PoDtNo

;WITH
rc AS(
	SELECT
		recdt.PoNo,
		recdt.PoDtNo,
		SUM(recdt.RecQty) as RecQty
	FROM
		PC.REC
		JOIN PC.RECDt
			ON recdt.RecNo=rec.RecNo
		JOIN @po po
			ON po.PoNo=recdt.PoNo AND po.PoDtNo=recdt.PoDtNo

	WHERE
		rec.DocStatus <> 'Voided'
		AND rec.RecNo <> @RecNo
	GROUP BY
		recdt.PoNo,
		recdt.PoDtNo
)
UPDATE
	@po
SET
	RecQty=rc.RecQty
FROM
	@po po
	JOIN rc
		ON rc.PoNo=po.PoNo AND rc.PoDtNo=po.PoDtNo

UPDATE 
	PC.PoDt
SET
	RcvQty=po.RecQty
FROM
	PC.PoDt
	JOIN @po po
		ON po.PoNo=podt.PoNo AND po.PoDtNo=podt.PoDt
";
            new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery(query, new SqlParameter[] { new SqlParameter("RecNo", recNo) });
        }

        private void SaveAndCommit(bool isCommitted)
        {
            var error = VerifyDataBeforeSave();

            if (!string.IsNullOrEmpty(error))
            {
                ShowAlert(error);

                return;
            }

            var invoiceNo = txt_InvNo.Text.Trim();

            // Assign Header's value
            var drh = _dtRec.Rows[0];
            var recNo = drh["RecNo"].ToString();
            var poSource = drh["PoSource"].ToString();

            drh["RecNo"] = recNo;
            drh["RecDate"] = de_RecDate.Date.Date;
            drh["VendorCode"] = ddl_Vendor.Value.ToString();
            drh["InvoiceNo"] = invoiceNo;
            if (string.IsNullOrEmpty(de_InvDate.Text))
                drh["InvoiceDate"] = DBNull.Value;
            else
                drh["InvoiceDate"] = de_InvDate.Date.Date;
            drh["Description"] = txt_Desc.Text.Trim();

            //drh["PoSource"] = "";

            drh["DeliPoint"] = ddl_DeliPoint.Value.ToString();
            drh["CurrencyCode"] = ddl_Currency.Value.ToString();
            drh["CurrencyRate"] = Convert.ToDecimal(se_CurrencyRate.Value);

            drh["TotalExtraCost"] = 0;
            drh["ExtraCostBy"] = ddl_ExtraCostBy.SelectedValue.ToString();

            drh["IsCashConsign"] = chk_CashConsign.Checked;
            drh["ExportStatus"] = 0;

            drh["DocStatus"] = DocStatus.Received.ToString();

            drh["UpdatedBy"] = LoginInfo.LoginName;
            drh["UpdatedDate"] = ServerDateTime;

            // Assign Details's value

            if (string.IsNullOrEmpty(recNo)) // create
            {
                recNo = CreateHeader(_dtRec);
            }
            else // edit
            {
                SaveHeader(_dtRec);

                if (!string.IsNullOrEmpty(poSource))
                    RestorePO(recNo);
            }

            CreateDetails(recNo, _dtRecDt);
            if (!string.IsNullOrEmpty(poSource))
                UpdatePO(recNo);

            if (isCommitted)
            {
                error = Commit(recNo);

                if (!string.IsNullOrEmpty(error))
                {
                    ShowAlert(error);

                    return;
                }
            }

            RedirectToView(recNo);
        }

        private string Commit(string recNo)
        {
            var error = "";

            var sql = new Helpers.SQL(hf_ConnStr.Value);
            try
            {
                sql.ExecuteQuery("DELETE FROM [IN].Inventory WHERE HdrNo=@DocNo", new SqlParameter[] { new SqlParameter("DocNo", recNo) });

                if (_default.CostMethod == "FIFO")
                {
                    #region -- query--
                    var query = @"
        DECLARE @CommittedDate DATETIME = GETDATE()
		DECLARE 
			@DigitQty INT = APP.DigitQty(),
			@DigitAmt INT = APP.DigitAmt(),
			@Price decimal(18, 4),
			@DiffAmt decimal(18, 4),
			@DiffQty decimal(18, 3),
			@Qty1 decimal(18,3),
			@Qty2 decimal(18,3),
			@Amt1 decimal(18,4),
			@Amt2 decimal(18,4)

		DECLARE 
			@HdrNo nvarchar(20),
			@DtNo INT,
			@InvNo INT,
			@ProductCode nvarchar(20),
			@LocationCode nvarchar(20),
			@Qty decimal(18,3),
			@Total decimal(18,4) = 100

		DECLARE rc_cursor CURSOR FOR
		SELECT
			RecNo,
			RecDtNo,
			ProductCode,
			LocationCode,
			ROUND(RecQty * Rate, @DigitQty) as Qty,
			NetAmt + ISNULL(ExtraCost,0) as Total
		FROM
			PC.RECDt
		WHERE
			RecNo=@DocNo

		OPEN rc_cursor
		FETCH NEXT FROM rc_cursor INTO @HdrNo, @DtNo, @ProductCode, @LocationCode, @Qty, @Total

		WHILE @@FETCH_STATUS=0
		BEGIN
			SET @Price  = ROUND(@Total/@Qty, APP.DigitAmt())
			SET @DiffAmt = @Total - ROUND(@Price * @Qty, @DigitAmt)
			SET @DiffQty = ABS( (@DiffAmt % 1) * POWER(10, APP.DigitQty()))

			DECLARE @p decimal(18,4) = (CAST(1 as decimal(18,4))/POWER(10,@DigitAmt))

			IF @DiffAmt <= 0
			BEGIN
				SET @Qty1 = @Qty - @DiffQty
				SET @Amt1 = @Price

				SET @Qty2 = @DiffQty
				SET @Amt2 = @Price - @p
			END
			ELSE
			BEGIN
				SET @Qty1 = @DiffQty
				SET @Amt1 = @Price + @p

				SET @Qty2 = @Qty - @DiffQty
				SET @Amt2 = @Price
			END

			INSERT INTO [IN].Inventory (HdrNo, DtNo, InvNo, ProductCode, Location, [IN], [OUT], Amount, PriceOnLots,[Type], CommittedDate) 
			SELECT 
				@HdrNo as HdrNo, 
				@DtNo as DtNo,
				1 as InvNo,
				@ProductCode as ProductCode,
				@LocationCode as [Location],
				@Qty1 as [IN],
				0 as [OUT],
				@Amt1 as Amount,
				ROUND(@Qty1 * @Amt1, @DigitAmt) as PriceOnLots,
				'RC' as [Type],
				@CommittedDate as CommittedDate

			IF (@DiffQty <> 0)
			BEGIN
				INSERT INTO [IN].Inventory (HdrNo, DtNo, InvNo, ProductCode, Location, [IN], [OUT], Amount, PriceOnLots,[Type], CommittedDate) 
				SELECT 
					@HdrNo as HdrNo, 
					@DtNo as DtNo,
					2 as InvNo,
					@ProductCode as ProductCode,
					@LocationCode as [Location],
					@Qty2 as [IN],
					0 as [OUT],
					@Amt2 as Amount,
					ROUND(@Qty2 * @Amt2, @DigitAmt) as PriceOnLots,
					'RC' as [Type],
					@CommittedDate as CommittedDate
			END

			FETCH NEXT FROM rc_cursor INTO @HdrNo, @DtNo, @ProductCode, @LocationCode, @Qty, @Total
		END

		CLOSE rc_cursor
		DEALLOCATE rc_cursor	
";
                    #endregion
                    sql.ExecuteQuery(string.Format(query, _default.DigitQty, _default.DigitAmt), new SqlParameter[] { new SqlParameter("DocNo", recNo) });

                }
                else // Average
                {
                    #region -- query--
                    var query = @"
		INSERT INTO [IN].Inventory (HdrNo, DtNo, InvNo, ProductCode, Location, [IN], [OUT], Amount, PriceOnLots, [Type], CommittedDate) 
		SELECT
			d.RecNo,
			d.RecDtNo,
			1,
			d.ProductCode,
			d.LocationCode,
			ROUND(RecQty * Rate, {0}) as [IN],
			0 as [OUT],
			ROUND(NetAmt/ROUND(RecQty * Rate, {0}), {1}) as [Amount],
			NetAmt + ISNULL(ExtraCost,0) as [PriceOnLots],
			'RC',
			GETDATE()
		FROM
			PC.REC h
			JOIN PC.RECDt d
				ON d.RecNo=h.RecNo
		WHERE
			h.RecNo = @DocNo
		
		EXEC [IN].[UpdateAverageCost] @DocNo";
                    #endregion
                    sql.ExecuteQuery(string.Format(query, _default.DigitQty, _default.DigitAmt), new SqlParameter[] { new SqlParameter("DocNo", recNo) });
                }

                sql.ExecuteQuery("UPDATE PC.Rec SET DocStatus='Committed' WHERE RecNo=@DocNo", new SqlParameter[] { new SqlParameter("DocNo", recNo) });

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return error;
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

        private void RedirectToView(string recNo)
        {
            Response.Redirect(string.Format("Rec.aspx?BuCode={0}&ID={1}&VID={2}", _BuCode, recNo, _VID));
        }

        private void RedirectToList()
        {
            Response.Redirect(string.Format("RecLst.aspx"));
        }

        private void UpdateUnitRate(string recNo)
        {
            var query = @"
;WITH
u AS(
	SELECT
		ProductCode,
		OrderUnit,
		Rate
	FROM
		[IN].ProdUnit
	WHERE
		UnitType='O'
		AND ProductCode IN (SELECT DISTINCT ProductCode FROM PC.RECDt WHERE RecNo=@id)
)
UPDATE
	PC.RECDt
SET
	Rate = ISNULL(u.Rate,1)
FROM
	PC.RECDt d
	LEFT JOIN u ON u.ProductCode=d.ProductCode AND u.OrderUnit=d.RcvUnit";
            new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery(query, new SqlParameter[] { new SqlParameter("id", recNo) });
        }


        // ---------------------------------
        private void SetNew()
        {
            var sql = new Helpers.SQL(hf_ConnStr.Value);

            _dtRec = sql.ExecuteQuery("SELECT TOP(1) * FROM PC.Rec WHERE RecNo='*'");
            _dtRecDt = sql.ExecuteQuery("SELECT TOP(1) * FROM PC.RecDt WHERE RecNo='*'");


            var dtDeliPoint = sql.ExecuteQuery("SELECT TOP(1) DptCode FROM [IN].DeliveryPoint");

            var deliPoint = dtDeliPoint.Rows.Count > 0 ? dtDeliPoint.Rows[0][0].ToString() : "";

            var dr = _dtRec.NewRow();

            dr["RecNo"] = "";
            dr["RecDate"] = DateTime.Today;

            if (!string.IsNullOrEmpty(deliPoint))
                dr["DeliPoint"] = deliPoint;

            dr["CurrencyCode"] = _default.Currency;
            dr["CurrencyRate"] = 1.00;

            dr["CreatedBy"] = LoginInfo.LoginName;
            dr["UpdatedBy"] = LoginInfo.LoginName;

            _dtRec.Rows.Add(dr);

        }

        private void SetEdit(string id)
        {
            var sql = new Helpers.SQL(hf_ConnStr.Value);

            _dtRec = sql.ExecuteQuery("SELECT * FROM PC.Rec WHERE RecNo=@id", new SqlParameter[] { new SqlParameter("id", id) });

            var query = @"
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

            _dtRecDt = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("id", id) });


        }

        private void SetFromPO()
        {
            var dsPo = Session["dsPo"] as DataSet;


            _dtPo = dsPo.Tables["PoDt"].Copy();
            _dtRec = dsPo.Tables["REC"].Copy();
            _dtRecDt = dsPo.Tables["RECDt"].Copy();


            // Set RecNo = "";
            _dtRec.Rows[0]["RecNo"] = "";

            // Add some fields to dtRecDt
            // LocationName
            // ProductDesc1
            // ProductDesc2
            // InventoryUnit
            // TaxTypeName
            //var locationName = new DataColumn
            //{
            //    DataType = typeof(string),
            //    ColumnName = "LocationName",
            //    AllowDBNull = true,
            //    Unique = false
            //};
            //var productDesc1 = new DataColumn
            //{
            //    DataType = typeof(string),
            //    ColumnName = "ProductDesc1",
            //    AllowDBNull = true,
            //    Unique = false
            //};
            //var productDesc2 = new DataColumn
            //{
            //    DataType = typeof(string),
            //    ColumnName = "ProductDesc2",
            //    AllowDBNull = true,
            //    Unique = false
            //};

            //var InventoryUnit = new DataColumn
            //{
            //    DataType = typeof(string),
            //    ColumnName = "InventoryUnit",
            //    AllowDBNull = true,
            //    Unique = false
            //};

            //var TaxTypeName = new DataColumn
            //{
            //    DataType = typeof(string),
            //    ColumnName = "TaxTypeName",
            //    AllowDBNull = true,
            //    Unique = false
            //};


            //_dtRecDt.Columns.AddRange(new DataColumn[]
            //{
            //    locationName,
            //    productDesc1,
            //    productDesc2,
            //    InventoryUnit,
            //    TaxTypeName
            //});


            //foreach (DataRow dr in _dtRecDt.Rows)
            //{
            //    var locationCode = dr["LocationCode"].ToString();
            //    var productCode = dr["ProductCode"].ToString();

            //    var sql = new Helpers.SQL(hf_ConnStr.Value);

            //    var query = "SELECT LocationName FROM [IN].StoreLocation WHERE LocationCode=@code";
            //    var dt = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("code", locationCode) });

            //    dr["LocationName"] = dt != null && dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "";

            //    query = "SELECT ProductDesc1, ProductDesc2, InventoryUnit FROM [IN].Product WHERE ProductCode=@code";
            //    dt = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("code", productCode) });

            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        dr["ProductDesc1"] = dt.Rows[0]["ProductDesc1"].ToString();
            //        dr["ProductDesc2"] = dt.Rows[0]["ProductDesc2"].ToString();
            //        dr["InventoryUnit"] = dt.Rows[0]["InventoryUnit"].ToString();
            //    }
            //}


        }


        private void BindHeader(DataTable dt)
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
            var extraCostBy = dr["ExtraCostBy"] == DBNull.Value ? "Q" : dr["ExtraCostBy"].ToString();
            //rdb_ExtraCostByAmt.Checked = extraCostBy.ToUpper() == "Q";
            //rdb_ExtraCostByQty.Checked = extraCostBy.ToUpper() == "A";
            ddl_ExtraCostBy.SelectedValue = extraCostBy;

            hf_PoSource.Value = dr["PoSource"].ToString();
            lbl_PoSource.Text = string.IsNullOrEmpty(dr["PoSource"].ToString()) ? "Manually created" : "by Purchase Order";


        }

        private void BindDetails()
        {
            _dtRecDt.AcceptChanges();

            gv_Detail.DataSource = _dtRecDt;
            gv_Detail.DataBind();
            SetGrandTotal();
        }

        private void SetItemEdit(GridViewRow row)
        {
            var ddl_Location = row.FindControl("ddl_Location") as ASPxComboBox;

            ddl_Location.Enabled = false;
        }

        private void ShowHideColumns(GridView gv, bool isEdit)
        {
            //var isFPO = !string.IsNullOrEmpty(dtRec.Rows[0]["PoSource"].ToString());

            // Header

            ddl_Currency.Enabled = !isEdit;
            se_CurrencyRate.Enabled = !isEdit;

            // Detail


            // Discount
            //var currDisc = ((DataControlField)gv.Columns
            //    .Cast<DataControlField>()
            //    .Where(fld => fld.HeaderText == "Discount")
            //    .SingleOrDefault());
            //// Net
            //var currNet = ((DataControlField)gv.Columns
            //    .Cast<DataControlField>()
            //    .Where(fld => fld.HeaderText == "Net")
            //    .SingleOrDefault());

            //// Tax
            //var currTax = ((DataControlField)gv.Columns
            //    .Cast<DataControlField>()
            //    .Where(fld => fld.HeaderText == "Tax")
            //    .SingleOrDefault());
            //// Total
            //var currTotal = ((DataControlField)gv.Columns
            //    .Cast<DataControlField>()
            //    .Where(fld => fld.HeaderText == "Total")
            //    .SingleOrDefault());
            //// Total
            //var total = ((DataControlField)gv.Columns
            //    .Cast<DataControlField>()
            //    .Where(fld => fld.HeaderText == "Base")
            //    .SingleOrDefault());

            // -------------------------------

            //currDisc.Visible = !isEdit;
            //currNet.Visible = !isEdit;
            //currTax.Visible = !isEdit;
            //currTotal.Visible = !isEdit;
            //total.Visible = !isEdit;
        }

        private void Calculate_CurrencyRate(decimal rate)
        {
            foreach (DataRow dr in _dtRecDt.Rows)
            {
                dr["DiccountAmt"] = RoundAmt(Convert.ToDecimal(dr["CurrDiscAmt"]) * rate);
                dr["TaxAmt"] = RoundAmt(Convert.ToDecimal(dr["CurrTaxAmt"]) * rate);
                dr["NetAmt"] = RoundAmt(Convert.ToDecimal(dr["CurrNetAmt"]) * rate);
                dr["TotalAmt"] = RoundAmt(Convert.ToDecimal(dr["CurrTotalAmt"]) * rate);
            }

            BindDetails();
            //SetGrandTotal();
            //gv_Detail.DataSource = dtRecDt;
            //gv_Detail.DataBind();
        }

        private void SetGrandTotal()
        {
            var currDiscAmt = _dtRecDt.AsEnumerable().Select(x => x.Field<decimal>("CurrDiscAmt")).Sum();
            var currNetAmt = _dtRecDt.AsEnumerable().Select(x => x.Field<decimal>("CurrNetAmt")).Sum();
            var currTaxAmt = _dtRecDt.AsEnumerable().Select(x => x.Field<decimal>("CurrTaxAmt")).Sum();
            var currTotalAmt = _dtRecDt.AsEnumerable().Select(x => x.Field<decimal>("CurrTotalAmt")).Sum();

            var discAmt = _dtRecDt.AsEnumerable().Select(x => x.Field<decimal>("DiccountAmt")).Sum();
            var netAmt = _dtRecDt.AsEnumerable().Select(x => x.Field<decimal>("NetAmt")).Sum();
            var taxAmt = _dtRecDt.AsEnumerable().Select(x => x.Field<decimal>("TaxAmt")).Sum();
            var totalAmt = _dtRecDt.AsEnumerable().Select(x => x.Field<decimal>("TotalAmt")).Sum();

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

        private string CheckDuplicateInvocieNo(string recNo, string vendorCode, string invoiceNo)
        {
            var query = "SELECT TOP(1) RecNo FROM PC.REC WHERE VendorCode=@VendorCode AND InvoiceNo=@InvoiceNo AND  CASE WHEN ISNULL(RecNo,'')='' THEN '' ELSE RecNo END  <> @RecNo";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("VendorCode", vendorCode),
                new SqlParameter("InvoiceNo", invoiceNo),
                new SqlParameter("RecNo",recNo)
            };

            var dt = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery(query, parameters);

            if (dt != null && dt.Rows.Count > 0)
                return dt.Rows[0][0].ToString();
            else
                return string.Empty;
        }

        private DateTime GetClosedPeriod()
        {
            var query = "SELECT TOP(1) EndDate FROM [IN].[Period] WHERE IsClose=1 ORDER BY EndDate DESC";
            var dt = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery(query);

            if (dt != null && dt.Rows.Count > 0)
                return Convert.ToDateTime(dt.Rows[0][0]);
            else
            {
                return new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1);
            }
        }


        private bool IsCreatedManual()
        {
            return string.IsNullOrEmpty(hf_PoSource.Value);
        }

        // Validation
        private string VerifyDataBeforeSave()
        {
            var message = "";
            var recNo = _dtRec != null && _dtRec.Rows.Count > 0 ? _dtRec.Rows[0]["RecNo"].ToString() : "";
            var closedPeriod = GetClosedPeriod();
            var recDate = de_RecDate.Date.Date;
            var vendorCode = ddl_Vendor.Value.ToString();
            var invoiceNo = txt_InvNo.Text.Trim();


            // Header

            // Document date
            if (recDate <= closedPeriod)
            {
                return string.Format("Document date must not be during the closed period.", FormatDate(closedPeriod));
            }

            // Required fields

            // Vendor
            if (ddl_Vendor.Value == null)
            {
                return "Vendor is required.";
            }

            // DeliveryPoint
            if (ddl_DeliPoint.Value == null)
            {
                return "Delivery point is required.";
            }

            // Currency
            if (ddl_Currency.Value == null)
            {
                return "Currency is required.";
            }
            // Currency rate
            if ((decimal)se_CurrencyRate.Value <= 0m)
            {
                return "Invalid currency rate.";
            }

            if (string.IsNullOrEmpty(de_InvDate.Text))
            {
                return "Invoice date is required.";
            }

            // Invocie No (no duplicate)
            if (string.IsNullOrEmpty(invoiceNo))
                return "Invoice no is required.";
            else
            {
                var query = "SELECT TOP(1) RecNo FROM PC.REC WHERE DocStatus<>'Voided'AND VendorCode=@VendorCode AND InvoiceNo=@InvoiceNo AND RecNo<>@RecNo";

                var parameters = new SqlParameter[]
                    {
                        new SqlParameter("VendorCode", vendorCode),
                        new SqlParameter("InvoiceNo", invoiceNo),
                        new SqlParameter("RecNo",recNo)
                    };

                var dt = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery(query, parameters);


                if (dt != null && dt.Rows.Count > 0)
                {
                    var dupRecNo = dt.Rows[0][0].ToString();
                    return string.Format("Invoice no. '{0}' already exists in receiving no. {1}", invoiceNo, dupRecNo); ;
                }

                //if (Convert.ToInt32(dt.Rows[0][0]) > 0) // duplicate
                //    return string.Format("Invoice No '{0}' already exists.", invoiceNo); ;
            }


            // -------------------------------------------------------------


            //// Invoice no
            //var docNo = CheckDuplicateInvocieNo(recNo, vendorCode, invoiceNo);
            //if (!string.IsNullOrEmpty(docNo))
            //{
            //    return string.Format("Duplicate invocie no, it was used on '{0}'.", docNo);
            //}


            // Details
            if (_dtRecDt.Rows.Count == 0)
            {
                return "Please add any item.";
            }

            return message;
        }

        private Deviation GetDeviation(string productCode)
        {
            var result = new Deviation
            {
                Price = 0,
                Qty = 0
            };

            var query = "SELECT TOP(1) ISNULL(PriceDeviation,0) Price, ISNULL(QuantityDeviation,0) Qty FROM [IN].[Product] WHERE ProductCode=@ProductCode";
            var dt = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery(query, new SqlParameter[] { new SqlParameter("ProductCode", productCode) });

            if (dt != null && dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];

                result.Qty = Convert.ToDecimal(dr["Qty"]);
                result.Price = Convert.ToDecimal(dr["Price"]);
            }


            return result;
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
            var dt = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery("SELECT CAST(DptCode as VARCHAR) as [Value], [Name] as [Text] FROM [IN].DeliveryPoint WHERE IsActived=1 ORDER BY DptCode");

            var items = dt.AsEnumerable()
                .Select(x => new DevExpress.Web.ASPxEditors.ListEditItem
                {
                    Value = x.Field<string>("Value"),
                    Text = x.Field<string>("Text"),
                    Selected = x.Field<string>("Value") == value
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

        private IEnumerable<ListEditItem> GetProductsOnLocation(string locationCode, string value = "")
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

        private void AddPoToRecDt(IEnumerable<string> poList)
        {
            var query = @"
;WITH
unit AS(
	SELECT
		ProductCode,
		OrderUnit,
		MAX(Rate) AS Rate
	FROM
		[IN].ProdUnit
	WHERE
		UnitType='O'
	GROUP BY
		ProductCode,
		OrderUnit
)
SELECT 
	PoNo,
	PoDt,
	BuCode,
	[Location], 
	l.LocationName,
	[Product],
	p.ProductDesc1,
	p.ProductDesc2,
	Unit,
	InventoryUnit,
	ISNULL(u.Rate, 0) as Rate,
	OrdQty,
	OrdQty-RcvQty+CancelQty as RemainQty,
	FocQty,
	Price,
	podt.TaxType,
	podt.TaxRate,
	Discount,
	CurrDiscAmt,
	CurrTaxAmt,
	CurrNetAmt,
	CurrTotalAmt,
	DiscountAmt,
	NetAmt,
	TaxAmt,
	TotalAmt,
	Comment
FROM 
	PC.PoDt 
	LEFT JOIN [IN].Product p
		ON p.ProductCode=podt.Product
	LEFT JOIN [IN].StoreLocation l
		ON l.LocationCode=podt.Location
	LEFT JOIN unit u
		On u.ProductCode=podt.Product AND u.OrderUnit=podt.Unit

WHERE 
	PoNo IN ({0}) 
	AND RcvQty < OrdQty-CancelQty 
	AND RcvQty >= 0
";
            var poListText = "'" + string.Join("','", poList) + "'";
            query = string.Format(query, poListText);
            var dtPo = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery(query);

            var existItems = _dtRecDt.AsEnumerable()
                .Select(x => new
                {
                    PoNo = x.Field<string>("PoNo"),
                    PoDtNo = x.Field<int>("PoDtNo")
                }
                ).ToArray();

            var newItems = dtPo.AsEnumerable()
                .Select(x => new
                {
                    PoNo = x.Field<string>("PoNo"),
                    PoDtNo = x.Field<int>("PoDt")
                }
                ).ToArray();

            // get po do not exists in current
            var recNo = lbl_RecNo.Text;
            var items = newItems.Except(existItems);

            foreach (var item in items)
            {
                var poNo = item.PoNo;
                var poDtNo = item.PoDtNo;

                var poItems = dtPo.AsEnumerable()
                    .Where(x => x.Field<string>("PoNo") == poNo && x.Field<int>("PoDt") == poDtNo)
                    .Select(x => new RecDt
                    {
                        RecNo = recNo,
                        LocationCode = x.Field<string>("Location"),
                        LocationName = x.Field<string>("LocationName"),
                        ProductCode = x.Field<string>("Product"),
                        ProductDesc1 = x.Field<string>("ProductDesc1"),
                        ProductDesc2 = x.Field<string>("ProductDesc2"),

                        UnitCode = x.Field<string>("InventoryUnit"),
                        RcvUnit = x.Field<string>("Unit"),
                        OrderQty = x.Field<decimal>("OrdQty"),
                        FocQty = x.Field<decimal>("FocQty"),
                        RecQty = x.Field<decimal>("RemainQty"),
                        Price = x.Field<decimal>("Price"),
                        Discount = x.Field<decimal>("Discount"),
                        TaxType = x.Field<string>("TaxType"),
                        TaxRate = x.Field<decimal>("TaxRate"),

                        CurrDiscAmt = x.Field<decimal>("CurrDiscAmt"),
                        CurrTaxAmt = x.Field<decimal>("CurrTaxAmt"),
                        CurrNetAmt = x.Field<decimal>("CurrNetAmt"),
                        CurrTotalAmt = x.Field<decimal>("CurrTotalAmt"),

                        DiccountAmt = x.Field<decimal>("DiscountAmt"),
                        TaxAmt = x.Field<decimal>("TaxAmt"),
                        NetAmt = x.Field<decimal>("NetAmt"),
                        TotalAmt = x.Field<decimal>("TotalAmt"),

                        PoNo = x.Field<string>("PoNo"),
                        PoDtNo = x.Field<int>("PoDt"),
                    })
                    .ToArray();

                // Recalculation if partial qty
                var currencyRate = se_CurrencyRate.Number;
                foreach (var po in poItems)
                {
                    if (po.OrderQty != po.RecQty)
                    {
                        var qty = po.RecQty;
                        var price = po.Price;
                        var taxType = po.TaxType;
                        var taxRate = po.TaxRate;
                        var discRate = po.Discount;
                        var currDisc = 0m;
                        var currNet = 0m;
                        var currTax = 0m;
                        var currTotal = 0m;
                        var amt = RoundAmt(price * qty);

                        switch (taxType.ToUpper())
                        {
                            case "I":
                                currDisc = RoundAmt(amt * discRate / 100);
                                var net = RoundAmt(amt - currDisc);
                                currTax = RoundAmt(net * (taxRate / (taxRate + 100)));
                                currNet = net - currTax;

                                break;
                            case "A":
                                currDisc = RoundAmt(amt * discRate / 100);
                                currNet = amt - currDisc;
                                currTax = RoundAmt(currNet * taxRate / 100);

                                break;
                            default:
                                currDisc = RoundAmt(amt * discRate / 100);
                                currNet = amt - currDisc;
                                currTax = 0m;
                                break;
                        }

                        currTotal = currNet + currTax;

                        po.CurrDiscAmt = currDisc;
                        po.CurrNetAmt = currNet;
                        po.CurrTaxAmt = currTax;
                        po.TotalAmt = currTotal;

                        po.DiccountAmt = RoundAmt(po.CurrDiscAmt * currencyRate);
                        po.NetAmt = RoundAmt(po.CurrNetAmt * currencyRate);
                        po.TaxAmt = RoundAmt(po.CurrTaxAmt * currencyRate);
                        po.TotalAmt = RoundAmt(po.CurrTotalAmt * currencyRate);
                    }
                }


                InsertToRecDt(poItems);


            }

            pop_PoList.ShowOnPageLoad = false;
            BindDetails();


        }

        private void InsertToRecDt(IEnumerable<RecDt> items)
        {
            foreach (var item in items)
            {
                var dr = _dtRecDt.NewRow();

                dr["RecNo"] = item.RecNo;
                dr["RecDtNo"] = 0;
                dr["LocationCode"] = item.LocationCode;
                dr["LocationName"] = item.LocationName;
                dr["ProductCode"] = item.ProductCode;
                dr["ProductDesc1"] = item.ProductDesc1;
                dr["ProductDesc2"] = item.ProductDesc2;
                dr["UnitCode"] = item.UnitCode;
                dr["OrderQty"] = item.OrderQty;
                dr["FocQty"] = item.FocQty;
                dr["RecQty"] = item.RecQty;
                dr["RcvUnit"] = item.RcvUnit;
                dr["Rate"] = item.Rate;
                dr["Price"] = item.Price;
                dr["DiscAdj"] = item.DiscAdj;
                dr["Discount"] = item.Discount;
                dr["TaxAdj"] = item.TaxAdj;
                dr["TaxType"] = item.TaxType;
                dr["TaxRate"] = item.TaxRate;
                dr["CurrDiscAmt"] = item.CurrDiscAmt;
                dr["CurrTaxAmt"] = item.CurrTaxAmt;
                dr["CurrNetAmt"] = item.CurrNetAmt;
                dr["CurrTotalAmt"] = item.CurrTotalAmt;
                dr["DiccountAmt"] = item.DiccountAmt;
                dr["TaxAmt"] = item.TaxAmt;
                dr["NetAmt"] = item.NetAmt;
                dr["TotalAmt"] = item.TotalAmt;
                if (item.ExpiryDate == null)
                    dr["ExpiryDate"] = DBNull.Value;
                else
                    dr["ExpiryDate"] = item.ExpiryDate;
                dr["ExtraCost"] = item.ExtraCost;
                dr["PoNo"] = item.PoNo;
                if (item.PoDtNo == null)
                    dr["PoDtNo"] = DBNull.Value;
                else
                    dr["PoDtNo"] = item.PoDtNo;
                dr["Comment"] = item.Comment;

                dr["Status"] = item.Status;
                dr["ExportStatus"] = item.ExportStatus;

                _dtRecDt.Rows.Add(dr);
            }
        }

        private void SetEditItem(bool isEdit)
        {
            btn_AddItem.Visible = !isEdit;
            btn_Save.Visible = !isEdit;
            btn_Commit.Visible = !isEdit;
        }

        private IEnumerable<ProductUnit> GetProductUnit(string productCode, string connectionString)
        {
            var result = new List<ProductUnit>();

            var dtOrderUnit = new Helpers.SQL(connectionString).ExecuteQuery("SELECT OrderUnit, [Rate] FROM [IN].ProdUnit WHERE UnitType='O' AND ProductCode=@code ORDER BY IsDefault DESC, OrderUnit", new SqlParameter[] { new SqlParameter("code", productCode) });

            if (dtOrderUnit != null && dtOrderUnit.Rows.Count > 0)
            {
                result = dtOrderUnit.AsEnumerable()
                    .Select(x => new ProductUnit
                    {
                        UnitCode = x.Field<string>("OrderUnit"),
                        UnitRate = x.Field<decimal>("Rate"),
                    })
                    .ToList();
            }


            return result;
        }

        // Inventory


        #endregion

        #region -- Models --

        private enum DocStatus
        {
            Received,
            Committed,
            Voided
        }

        enum ItemAction
        {
            CREATE,
            UPDATE,
            DELETE
        }
        public class DefaultValues
        {
            public string Currency { get; set; }
            public int DigitAmt { get; set; }
            public int DigitQty { get; set; }
            public decimal TaxRate { get; set; }
            public string CostMethod { get; set; }
        }

        public class Deviation
        {
            public Deviation()
            {
                Price = 0m;
                Qty = 0m;
            }

            public decimal Price { get; set; }
            public decimal Qty { get; set; }
        }


        public class RecDt
        {
            public RecDt()
            {
                OrderQty = 0;
                FocQty = 0;
                RecQty = 0;
                Rate = 1;
                Price = 0;
                TaxAdj = false;
                DiscAdj = false;

                PoNo = null;
                PoDtNo = null;
                ExpiryDate = null;
                Status = "Received";
                ExtraCost = 0;
                ExportStatus = false;
            }

            public string RecNo { get; set; }
            public int RecDtno { get; set; }
            public string LocationCode { get; set; }
            public string LocationName { get; set; }
            public string ProductCode { get; set; }
            public string ProductDesc1 { get; set; }
            public string ProductDesc2 { get; set; }
            public string UnitCode { get; set; }
            public decimal OrderQty { get; set; }
            public decimal FocQty { get; set; }
            public decimal RecQty { get; set; }
            public string RcvUnit { get; set; }
            public decimal Rate { get; set; }
            public decimal Price { get; set; }
            public bool DiscAdj { get; set; }
            public decimal Discount { get; set; }
            public bool TaxAdj { get; set; }
            public string TaxType { get; set; }
            public decimal TaxRate { get; set; }
            public decimal CurrDiscAmt { get; set; }
            public decimal CurrTaxAmt { get; set; }
            public decimal CurrNetAmt { get; set; }
            public decimal CurrTotalAmt { get; set; }
            public decimal DiccountAmt { get; set; }
            public decimal TaxAmt { get; set; }
            public decimal NetAmt { get; set; }
            public decimal TotalAmt { get; set; }
            public Nullable<DateTime> ExpiryDate { get; set; }
            public decimal ExtraCost { get; set; }
            public string PoNo { get; set; }
            public Nullable<int> PoDtNo { get; set; }
            public string Status { get; set; }
            public bool ExportStatus { get; set; }
            public string Comment { get; set; }








        }

        public class ProductUnit
        {
            public string UnitCode { get; set; }
            public decimal UnitRate { get; set; }
        }

        public class ProductItem
        {
            private string _productCode = "";
            private string _productDesc1 = "";
            private string _productDesc2 = "";
            private string _itemGroupCode = "";
            private string _inventoryUnit = "";
            private string _taxType = "N";
            private decimal _taxRate = 0;
            private decimal _priceDeviation = 0;
            private decimal _qtyDeviation = 0;
            private IEnumerable<ProductUnit> _units = new List<ProductUnit>();


            public ProductItem(string code, string connectionString)
            {
                _productCode = code;
                var sql = new Helpers.SQL(connectionString);

                var dt = sql.ExecuteQuery("SELECT * FROM [IN].Product WHERE ProductCode = @code", new SqlParameter[] { new SqlParameter("code", code) });

                if (dt != null && dt.Rows.Count > 0)
                {
                    var dr = dt.Rows[0];

                    _productDesc1 = dr["ProductDesc1"].ToString();
                    _productDesc2 = dr["ProductDesc2"].ToString();
                    _itemGroupCode = dr["ProductCate"].ToString();
                    _inventoryUnit = dr["InventoryUnit"].ToString();
                    _taxType = dr["TaxType"].ToString();
                    _taxRate = string.IsNullOrEmpty(dr["TaxRate"].ToString()) ? 0m : Convert.ToDecimal(dr["TaxRate"]);
                    _priceDeviation = string.IsNullOrEmpty(dr["PriceDeviation"].ToString()) ? 0m : Convert.ToDecimal(dr["PriceDeviation"]);
                    _qtyDeviation = string.IsNullOrEmpty(dr["QuantityDeviation"].ToString()) ? 0m : Convert.ToDecimal(dr["QuantityDeviation"]);
                    _units = GetProductUnit(code, connectionString);



                }

            }

            public string ProductCode { get { return _productCode; } }
            public string ProductDesc1 { get { return _productDesc1; } }
            public string ProductDesc2 { get { return _productDesc2; } }
            public string ItemGroupCode { get { return _itemGroupCode; } }
            public string InventoryUnit { get { return _inventoryUnit; } }
            public string TaxType { get { return _taxType; } }
            public decimal TaxRate { get { return _taxRate; } }
            public decimal PriceDeviation { get { return _priceDeviation; } }
            public decimal QtyDeviation { get { return _qtyDeviation; } }
            public IEnumerable<ProductUnit> Units { get { return _units; } }

            public IEnumerable<ProductUnit> GetProductUnit(string productCode, string connectionString)
            {
                var result = new List<ProductUnit>();

                var dtOrderUnit = new Helpers.SQL(connectionString).ExecuteQuery("SELECT OrderUnit, [Rate] FROM [IN].ProdUnit WHERE UnitType='O' AND ProductCode=@code ORDER BY IsDefault DESC, OrderUnit", new SqlParameter[] { new SqlParameter("code", productCode) });

                if (dtOrderUnit != null && dtOrderUnit.Rows.Count > 0)
                {
                    result = dtOrderUnit.AsEnumerable()
                        .Select(x => new ProductUnit
                        {
                            UnitCode = x.Field<string>("OrderUnit"),
                            UnitRate = x.Field<decimal>("Rate"),
                        })
                        .ToList();
                }


                return result;
            }


        }

        #endregion
    }

}