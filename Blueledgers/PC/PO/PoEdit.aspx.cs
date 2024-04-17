using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxMenu;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using Resources;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.PC.PO
{
    public partial class PoEdit : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.GL.Account.Account _acc = new Blue.BL.GL.Account.Account();
        private readonly Blue.BL.Option.Admin.Interface.AccountMapp _accMapp = new Blue.BL.Option.Admin.Interface.AccountMapp();
        private readonly Blue.BL.dbo.Bu _bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.Ref.Currency _curr = new Blue.BL.Ref.Currency();
        private readonly Blue.BL.Option.Inventory.DeliveryPoint _deliPoint = new Blue.BL.Option.Inventory.DeliveryPoint();
        private readonly DataSet _dsRecDt = new DataSet();
        private readonly DataSet _dsStockSum = new DataSet();

        private readonly Blue.BL.PC.Priod _period = new Blue.BL.PC.Priod();
        private readonly Blue.BL.PC.PO.PO _po = new Blue.BL.PC.PO.PO();
        private readonly Blue.BL.PC.PO.PoDt _poDt = new Blue.BL.PC.PO.PoDt();
        private readonly Blue.BL.PC.PR.PRDt _prDt = new Blue.BL.PC.PR.PRDt();

        private readonly Blue.BL.IN.ProdUnit _prodUnit = new Blue.BL.IN.ProdUnit();
        private readonly Blue.BL.Option.Inventory.Product _product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.PC.REC.RECDt _recDt = new Blue.BL.PC.REC.RECDt();
        private readonly Blue.BL.Option.Inventory.StoreLct _storeLct = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();
        private readonly Blue.BL.Option.Inventory.Unit _unit = new Blue.BL.Option.Inventory.Unit();
        private readonly Blue.BL.dbo.User _user = new Blue.BL.dbo.User();
        private readonly Blue.BL.AP.Vendor _vendor = new Blue.BL.AP.Vendor();
        private string _action = string.Empty;
        private DataSet _dsPo = new DataSet();
        private DataTable _dtPrDt = new DataTable();
        private Blue.BL.GnxLib _gnxLib = new Blue.BL.GnxLib();
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();

        private Boolean _isEdit;
        private decimal _netamt;
        private decimal _discAmt;
        private decimal _netamtpr;
        private decimal _taxamt;
        private decimal _taxamtpr;
        private decimal _totalamt;
        private decimal _totalamtpr;

        /*
                private PriceList priceList = new PriceList();
                private ProdCat prodCat = new ProdCat();
                private Template template = new Template();
                private TemplateDt templateDt = new TemplateDt();
                private WF workFlow = new WF();
        */

        /*
                private string[] AddEditRowPK
                {
                    get { return (string[]) Session["AddEditRowPK"]; }
                    set { Session["AddEditRowPK"] = value; }
                }
        */

        // Added on: 29/08/2017, By: Fon
        private decimal _currNetAmt;
        private decimal _currDiscAmt;
        private decimal _currTaxAmt;
        private decimal _currTotalAmt;
        private string baseCurrency
        {
            get { return config.GetValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value); }
        }

        // End Added.
        #endregion

        #region "Operations"

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            // Check login
            base.Page_Load(sender, e);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            //hf_ConnStr.Value = LoginInfo.ConnStr;
            hf_ConnStr.Value = _bu.GetConnectionString(Request.Params["BuCode"]);

            if (!IsPostBack)
            {
                Page_Retrieve();
                Page_Setting();
            }
            else
            {
                _dsPo = (DataSet)Session["dsPo"];

                //if (dsPo.Tables[po.TableName].Rows[0]["DocStatus"].ToString() == "Approved")
                //{
                //    for (int i = 0; i < grd_PoDt.Rows.Count; i++)
                //    {
                //        LinkButton lnb_Edit = grd_PoDt.Rows[i].FindControl("lnb_Edit") as LinkButton;
                //        lnb_Edit.Enabled = true;

                //        LinkButton lnb_Cancel = grd_PoDt.Rows[i].FindControl("lnb_Cancel") as LinkButton;
                //        lnb_Cancel.Enabled = true;
                //    }
                //} 
            }
        }

        /// <summary>
        ///     Gets season data.
        /// </summary>
        private void Page_Retrieve()
        {
            var msgError = string.Empty;
            var mode = Request.QueryString["MODE"];

            switch (mode.ToUpper())
            {
                case "EDIT":
                    ViewState["IsEdit"] = true;
                    if (!string.IsNullOrEmpty(Request.Params["ID"]))
                    {
                        // Get invoice no from HTTP query string
                        var poNo = Request.QueryString["ID"];

                        var getStrLct = _po.GetListByPoNo2(_dsPo, ref msgError, poNo, hf_ConnStr.Value);

                        if (getStrLct)
                        {
                            _poDt.GetListByPoNo(_dsPo, ref msgError, poNo, hf_ConnStr.Value);
                        }
                        else
                        {
                            var error = MsgError.ResourceManager.GetString(msgError);
                            return;
                        }
                    }
                    break;

                case "TEMPLATE":
                    {
                        _dsPo = (DataSet)Session["dsTemplate"];

                        var poNoTemp = _dsPo.Tables[_po.TableName].Rows[0]["PONo"].ToString();

                        var resultDt = _poDt.GetListByPoNo(_dsPo, ref msgError, poNoTemp, hf_ConnStr.Value);
                        if (!resultDt)
                        {
                            // Display Error Message
                            return;
                        }
                    }
                    break;

                case "NEW":
                    {
                        var result = _po.GetStructure2(_dsPo, hf_ConnStr.Value);
                        //string MsgError = string.Empty;

                        if (result)
                        {
                            var drNew = _dsPo.Tables[_po.TableName].NewRow();

                            drNew["PoNo"] = _po.GetNewID(ServerDateTime, hf_ConnStr.Value);
                            drNew["PoDate"] = ServerDateTime;
                            drNew["Description"] = string.Empty;
                            drNew["CreatedDate"] = ServerDateTime;
                            drNew["CreatedBy"] = LoginInfo.LoginName;
                            drNew["UpdatedDate"] = ServerDateTime;
                            drNew["UpdatedBy"] = LoginInfo.LoginName;

                            // Add new row
                            _dsPo.Tables[_po.TableName].Rows.Add(drNew);
                            _poDt.GetSchema(_dsPo, hf_ConnStr.Value);
                        }
                    }
                    break;
            }

            // Get PODt Table Schema
            _curr.GetList(_dsPo, hf_ConnStr.Value);
            _storeLct.GetList(_dsPo, hf_ConnStr.Value);
            _product.GetList(_dsPo, hf_ConnStr.Value);
            _vendor.GetList(_dsPo, hf_ConnStr.Value);
            _deliPoint.GetList(_dsPo, hf_ConnStr.Value);
            _user.GetList(_dsPo, Request.Params["BuCode"]);
            _unit.GetList(_dsPo, ref msgError, hf_ConnStr.Value);
            _acc.GetList(_dsPo, hf_ConnStr.Value);

            Session["dsPo"] = _dsPo;
        }

        /// <summary>
        ///     Display season data.
        /// </summary>
        private void Page_Setting()
        {
            var mode = Request.QueryString["MODE"];
            var drPo = _dsPo.Tables[_po.TableName].Rows[0];

            //Binding lookup.
            cmb_Vendor.DataSource = _dsPo.Tables[_vendor.TableName];
            cmb_Vendor.DataBind();

            //cmb_Location.DataSource = dsPo.Tables[storeLct.TableName];
            //cmb_Location.DataBind();

            //cmb_Delivery.DataSource = dsPo.Tables[deliPoint.TableName];
            //cmb_Delivery.DataBind();

            //cmb_Currency.DataSource = dsPo.Tables[curr.TableName];
            //cmb_Currency.DataBind();
            //lbl_Buyer.Text = LoginInfo.LoginName.ToString();


            // Modified: 15/08/2017: By: Fon, About: New Multi-currency
            switch (mode.ToUpper())
            {
                case "NEW":
                    lbl_Status.Text = @"Pending";
                    break;

                case "EDIT":
                    lbl_Status.Text = drPo["DocStatus"].ToString();
                    lbl_PONumber.Text = drPo["PoNo"].ToString();

                    // Modified on: 30/08/2017, By: Fon
                    //lbl_Exchange.Text = drPo["ExchageRate"].ToString();
                    //lbl_Currency.Text = drPo["Currency"].ToString();
                    lbl_Currency.Text = drPo["CurrencyCode"].ToString();
                    txt_CurrRate.Text = drPo["CurrencyRate"].ToString();
                    // End Modified.

                    txt_PODate.Text = drPo["PoDate"].ToString() != string.Empty
                        ? DateTime.Parse(drPo["PoDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate)
                        : string.Empty;

                    if (drPo["DeliDate"].ToString() != string.Empty)
                    {
                        dte_DeliDate.Date = DateTime.Parse(drPo["DeliDate"].ToString());
                        //cld_std.SelectedDate  = DateTime.Parse(drPo["DeliDate"].ToString());
                    }

                    txt_Desc.Text = drPo["Description"].ToString();
                    txt_Remark1.Text = drPo["AddField1"].ToString();
                    txt_Remark2.Text = drPo["AddField2"].ToString();
                    txt_Remark3.Text = drPo["AddField3"].ToString();
                    lbl_Buyer.Text = drPo["Buyer"].ToString();
                    cmb_Vendor.Value = drPo["Vendor"].ToString();
                    txt_CreditTerm.Text = drPo["CreditTerm"].ToString();
                    if (drPo["DocStatus"].ToString() == "Approved")
                    {
                        dte_DeliDate.Enabled = true;
                        //cmb_Vendor.Enabled      = true;
                        txt_Desc.Enabled = true;
                        txt_Remark1.Enabled = true;
                        txt_Remark2.Enabled = true;
                        txt_Remark3.Enabled = true;
                        //dte_PODate.Enabled      = true;
                        txt_PODate.Enabled = true;

                        //LinkButton lnb_Edit = grd_PoDt.Rows[0].FindControl("lnb_Edit") as LinkButton;
                        //lnb_Edit.Enabled    = true;
                    }
                    break;

                case "TEMPLATE":
                    {
                        _dsPo = (DataSet)Session["dsTemplate"];
                        var drPoTmp = _dsPo.Tables[_po.TableName].Rows[0];

                        lbl_Status.Text = @"Approved";
                        //sp_CreditTerm.Value     = drPoTmp["CreditTerm"].ToString();
                        txt_CreditTerm.Text = drPoTmp["CreditTerm"].ToString();
                        cmb_Vendor.Value = drPoTmp["Vendor"].ToString();

                        if (drPoTmp["DeliDate"].ToString() != string.Empty)
                        {
                            dte_DeliDate.Date = DateTime.Parse(drPoTmp["DeliDate"].ToString());
                        }

                        var isVendor = _vendor.GetVendor(drPoTmp["Vendor"].ToString(), _dsPo, hf_ConnStr.Value);
                        if (isVendor)
                        {
                            txt_CreditTerm.Text = _dsPo.Tables[_vendor.TableName].Rows[0]["CreditTerm"].ToString();
                            //sp_CreditTerm.Value    = dsPo.Tables[vendor.TableName].Rows[0]["CreditTerm"].ToString();                    
                            //spe_ExchangeRate.Value  = 1;
                            //cmb_Currency.Value      = "THB";                    
                        }
                    }
                    break;
            }

            Total();
            lbl_TNet.Text = String.Format(DefaultAmtFmt, _netamt);
            lbl_TTax.Text = String.Format(DefaultAmtFmt, _taxamt);
            lbl_TAmount.Text = String.Format(DefaultAmtFmt, _totalamt);

            // Added on: 29/08/2017, By: Fon
            lbl_CurrGrandTitle.Text = string.Format("( {0} )", lbl_Currency.Text);
            lbl_BaseGrandTitle.Text = string.Format("( {0} )", baseCurrency);
            lbl_CurrTNet.Text = string.Format(DefaultAmtFmt, _currNetAmt);
            lbl_CurrTTax.Text = string.Format(DefaultAmtFmt, _currTaxAmt);
            lbl_CurrTAmount.Text = string.Format(DefaultAmtFmt, _currTotalAmt);

            // Added on: 12/02/2018, For: Following from P'Oat.
            lbl_CurrTDisc.Text = string.Format(DefaultAmtFmt, _currDiscAmt);
            lbl_TDisc.Text = string.Format(DefaultAmtFmt, _discAmt);

            // End Added.

            grd_PoDt.DataSource = _dsPo.Tables[_poDt.TableName];
            grd_PoDt.DataBind();

            switch (drPo["DocStatus"].ToString())
            {
                case "Approved":
                    for (var i = 0; i < grd_PoDt.Rows.Count; i++)
                    {
                        var lnbEdit = grd_PoDt.Rows[i].FindControl("lnb_Edit") as LinkButton;
                        if (lnbEdit != null) lnbEdit.Visible = true;

                        var lnbCancel = grd_PoDt.Rows[i].FindControl("lnb_Cancel") as LinkButton;
                        if (lnbCancel != null) lnbCancel.Visible = true;
                    }
                    break;
            }

            // Display Log
            //((BlueLedger.PL.Master.Pc.Blue)this.Master).AllowShowLog = true;
            //((BlueLedger.PL.Master.Pc.Blue)this.Master).TableName    = "Admin.TransLog";//"GL.JournalVoucherActLog";
            //((BlueLedger.PL.Master.Pc.Blue)this.Master).Module       = "PC";
            //((BlueLedger.PL.Master.Pc.Blue)this.Master).SubModule    = "PO";
            //((BlueLedger.PL.Master.Pc.Blue)this.Master).RefNo        = drPo["PoNo"].ToString();      

            //PL.UserControls.Log2 log = (PL.UserControls.Log2)((BlueLedger.PL.Master_In_Default)this.Master).FindControl("Log");
            //log.Module = "PC";
            //log.SubModule = "PO";
            //log.RefNo = dsPo.Tables[po.TableName].Rows[0]["PoNo"].ToString();
            //log.Visible = true;
            //log.DataBind();


            //// Display Comment       
            //PL.UserControls.Comment2 comment = (PL.UserControls.Comment2)((BlueLedger.PL.Master_In_Default)this.Master).FindControl("Comment");
            //comment.Module = "PC";
            //comment.SubModule = "PO";
            //comment.RefNo = dsPo.Tables[po.TableName].Rows[0]["PoNo"].ToString();
            //comment.Visible = true;
            //comment.DataBind();
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_CmdBar_ItemClick(object source, MenuItemEventArgs e)
        {
            var mode = Request.QueryString["MODE"];

            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":

                    Page.Validate();

                    if (Page.IsValid)
                    {
                        // Prepare save data to Po 
                        var drPo = _dsPo.Tables[_po.TableName].Rows[0];

                        _isEdit = ViewState["IsEdit"] != null
                            ? Boolean.Parse(ViewState["IsEdit"].ToString())
                            : mode.ToUpper() == "EDIT";

                        if (_isEdit != true)
                        {
                            drPo["PoNo"] = _po.GetNewID(ServerDateTime, hf_ConnStr.Value);
                            drPo["DocStatus"] = lbl_Status.Text;
                        }

                        if (cmb_Vendor.Value != null)
                        {
                            drPo["Vendor"] = cmb_Vendor.Value.ToString() != string.Empty
                                ? cmb_Vendor.Value
                                : DBNull.Value;
                        }

                        //if (cmb_Currency.Value != null)
                        //{
                        //    if (cmb_Currency.Value.ToString() != string.Empty)
                        //    {
                        //        drPo["Currency"] = cmb_Currency.Value;
                        //    }
                        //    else
                        //    {
                        //        drPo["Currency"] = DBNull.Value;
                        //    }
                        //}String.Format("{0:d/m/yyyy HH:mm:ss}", drPo["DeliDate"].ToString())

                        drPo["PoDate"] =
                            DateTime.Parse(txt_PODate.Text)
                                .AddHours(ServerDateTime.Hour)
                                .AddMinutes(ServerDateTime.Minute)
                                .AddSeconds(ServerDateTime.Second);
                        drPo["DeliDate"] = dte_DeliDate.Date;

                        if (txt_CreditTerm.Text != "")
                        {
                            drPo["CreditTerm"] = txt_CreditTerm.Text; //sp_CreditTerm.Text.ToString());
                        }

                        // Modified on: 29/08/2017
                        //Convert.ToInt32(spe_ExchangeRate.Text.ToString());
                        //drPo["ExchageRate"] = 1; 
                        drPo["CurrencyRate"] = txt_CurrRate.Text;

                        drPo["Description"] = txt_Desc.Text.Trim();
                        drPo["AddField1"] = txt_Remark1.Text.Trim();
                        drPo["AddField2"] = txt_Remark2.Text.Trim();
                        drPo["AddField3"] = txt_Remark3.Text.Trim();
                        drPo["IsVoid"] = false;
                        drPo["UpdatedDate"] = ServerDateTime;
                        drPo["UpdatedBy"] = LoginInfo.LoginName;
                        drPo["Buyer"] = LoginInfo.LoginName;

                        //---------------------------- TransLog --------------------------------------------
                        var drLog = _dsPo.Tables[_po.TableName].Rows[0];

                        // Arrylist for comibne TableName/FieldName/FieldValue
                        var arrayCombine = new ArrayList();

                        if (Request.Params["MODE"].ToUpper() == "EDIT")
                        {
                            // if the action is edit.action value need to assign for Modify.
                            _action = "Modify";

                            //Check field description.
                            var descOld = drLog[10, DataRowVersion.Original].ToString();

                            if (!descOld.Equals(txt_Desc.Text))
                            {
                                var activityPrefixCode = " From " +
                                                         (descOld == string.Empty ? "''" : descOld) +
                                                         " To " + txt_Desc.Text;

                                // Log information pass by arraylist
                                arrayCombine.Add(new[] { "PO", "AddField1", activityPrefixCode });
                            }

                            //Check field Remark1.
                            var remk1Old = drLog[18, DataRowVersion.Original].ToString();

                            if (!remk1Old.Equals(txt_Remark1.Text))
                            {
                                var activityPrefixCode = " From " +
                                                         (remk1Old == string.Empty ? "''" : remk1Old) +
                                                         " To " + txt_Remark1.Text;

                                // Log information pass by arraylist
                                arrayCombine.Add(new[] { "PO", "AddField1", activityPrefixCode });
                            }

                            //Check field Remark2.
                            var remk2Old = drLog[19, DataRowVersion.Original].ToString();

                            if (!remk2Old.Equals(txt_Remark2.Text))
                            {
                                var activityPrefixCode = " From " +
                                                         (remk2Old == string.Empty ? "''" : remk2Old) +
                                                         " To " + txt_Remark2.Text;

                                // Log information pass by arraylist
                                arrayCombine.Add(new[] { "PO", "AddField2", activityPrefixCode });
                            }

                            //Check field Remark3.
                            var remk3Old = drLog[20, DataRowVersion.Original].ToString();

                            if (!remk3Old.Equals(txt_Remark3.Text))
                            {
                                var activityPrefixCode = " From " +
                                                         (remk3Old == string.Empty ? "''" : remk3Old) +
                                                         " To " + txt_Remark3.Text;

                                // Log information pass by arraylist
                                arrayCombine.Add(new[] { "PO", "AddField3", activityPrefixCode });
                            }

                            //Check field vendor.
                            var vendorOld = drLog[2, DataRowVersion.Original].ToString();

                            if (cmb_Vendor.Value != null && !vendorOld.Equals(cmb_Vendor.Value.ToString()))
                            {
                                var activityVendorOld = " From " + (vendorOld == string.Empty
                                    ? "''"
                                    : vendorOld) + " To " + cmb_Vendor.Value;

                                // Log information pass by arraylist
                                arrayCombine.Add(new[] { "PO", "Vendor", activityVendorOld });
                            }

                            //Check field delivery date.
                            var deliDate = drLog[7, DataRowVersion.Original].ToString();

                            if (!deliDate.Equals(dte_DeliDate.Date.ToString()))
                            {
                                var activityDeliDate = " From " +
                                                       (deliDate == string.Empty
                                                           ? "''"
                                                           : deliDate) + " To " + dte_DeliDate.Date;

                                // Log information pass by arraylist
                                arrayCombine.Add(new[] { "PO", "DeliveryDate", activityDeliDate });
                            }

                            //Check field credit term.
                            var creditTerm = drLog[9, DataRowVersion.Original].ToString();

                            //if (!creditTerm.Equals(sp_CreditTerm.Value.ToString()))
                            if (!creditTerm.Equals(txt_CreditTerm.Text))
                            {
                                var activityCreditTerm = " From " +
                                                         (creditTerm == string.Empty
                                                             ? "''"
                                                             : creditTerm) + " To " + txt_CreditTerm.Text;
                                //sp_CreditTerm.Value.ToString();

                                // Log information pass by arraylist
                                arrayCombine.Add(new[] { "PO", "CreditTerm", activityCreditTerm });
                            }
                        }
                        //else
                        //{ 

                        //}

                        var dsT = new DataSet();

                        // Get trans log schema.
                        _transLog.GetSchema(dsT, LoginInfo.ConnStr);

                        // Create new row.
                        var drNew = dsT.Tables[_transLog.TableName].NewRow();

                        // Prepare for Insert to accountactlog table.
                        drNew["ID"] = _transLog.GetNewID(hf_ConnStr.Value);
                        drNew["Module"] = "PC";
                        drNew["Submodule"] = "PO";
                        drNew["RefNo"] = _dsPo.Tables[_po.TableName].Rows[0]["PONo"].ToString();
                        drNew["Log"] = Blue.BL.GnxLib.GetXMLFormat(_action, arrayCombine);
                        drNew["CreatedDate"] = ServerDateTime;
                        drNew["CreatedBy"] = LoginInfo.LoginName;

                        // Add new row
                        dsT.Tables[_transLog.TableName].Rows.Add(drNew);

                        var resultLog = _transLog.Save(dsT, hf_ConnStr.Value);

                        //Case create po by pr
                        bool saved;
                        if (mode.ToUpper() == "TEMPLATE")
                        {
                            DataTable dtPrDt = _dsPo.Tables[_prDt.TableName];
                            DataTable dtPoDt = _dsPo.Tables[_poDt.TableName];

                            foreach (DataRow drPrDt in dtPrDt.Rows)
                            {
                                foreach (DataRow drPoDt in dtPoDt.Rows)
                                {
                                    if (drPrDt["PRNO"].Equals(drPoDt["PrNo"]) && drPrDt["PRDtNO"].Equals(drPoDt["PrDtNo"]))
                                    {
                                        drPrDt["ApprQty"] = drPoDt["OrdQty"];
                                        drPrDt["PONo"] = drPoDt["PoNo"];
                                    }
                                }

                            }


                            //if (_dsPo.Tables[_prDt.TableName].Rows.Count > 0)
                            //{
                            //    for (var y = 0; y <= _dsPo.Tables[_prDt.TableName].Rows.Count - 1; y++)
                            //    {
                            //        var drPrDt = _dsPo.Tables[_prDt.TableName].Rows[y];

                            //        if (_dsPo.Tables[_poDt.TableName].Rows.Count > 0)
                            //        {
                            //            for (var i = 0; i <= _dsPo.Tables[_poDt.TableName].Rows.Count - 1; i++)
                            //            {
                            //                var drPoDt = _dsPo.Tables[_poDt.TableName].Rows[i];
                            //                if (drPrDt["PRNO"].Equals(drPoDt["PrNo"]) &
                            //                    drPrDt["PRDtNO"].Equals(drPoDt["PrDtNo"]))
                            //                {
                            //                    drPrDt["ApprQty"] = drPoDt["OrdQty"];
                            //                    drPrDt["PONo"] = drPoDt["PoNo"];
                            //                }
                            //            }
                            //        }
                            //    }
                            //}

                            // Save changed to database.
                            saved = _po.SavePRPO(_dsPo, hf_ConnStr.Value);
                        }
                        else
                        {
                            // Save changed to database.
                            saved = _po.Save(_dsPo, hf_ConnStr.Value);
                        }

                        if (saved)
                        {
                            switch (mode.ToUpper())
                            {
                                case "NEW":
                                    {
                                        var body =
                                            " Select link below to view details and process document \n http://www.blueledgers.com/blueledger/PC/PO/PoList.aspx?id=" + drPo["PoNo"];

                                        //const string receivers = "demo@blueledgers.com";
                                        //// changed to user from work-flow config
                                        //var subjects = "PO No. " + drPo["PONo"] + " awaiting your approval";

                                        //try
                                        //{
                                        //    SendMail(receivers, subjects, body);
                                        //}
                                        //catch (Exception ex)
                                        //{
                                        //    LogManager.Error(ex);
                                        //    // Keep error to log
                                        //}
                                    }
                                    break;
                            }

                            // Added on: 21/09/2017, By: Fon
                            ClassLogTool pctool = new ClassLogTool();
                            pctool.SaveActionLog("PO", lbl_PONumber.Text, "Save");
                            // End Added.

                            Response.Redirect("PoList.aspx?id=" + drPo["PoNo"]);
                        }
                    }

                    break;

                case "BACK":

                    switch (mode.ToUpper())
                    {
                        case "EDIT":
                            Response.Redirect("Po.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + lbl_PONumber.Text);
                            break;

                        default:
                            Response.Redirect("PoList.aspx");
                            break;
                    }

                    break;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmb_Delivery_Load(object sender, EventArgs e)
        {
            //cmb_Delivery.DataSource = dsPo.Tables[deliPoint.TableName];
            //cmb_Delivery.DataBind();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmb_Location_Load(object sender, EventArgs e)
        {
            //cmb_Location.DataSource = storeLct.GetList(LoginInfo.ConnStr);
            //cmb_Location.DataBind();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmb_Location_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cmb_Location.Value.ToString() != string.Empty)
            //{
            //    cmb_Delivery.Value = storeLct.GetDeliveryPoint(cmb_Location.Value.ToString(), LoginInfo.ConnStr);
            //}
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmb_Currency_Load(object sender, EventArgs e)
        {
            //cmb_Currency.DataSource = dsPo.Tables[curr.TableName];
            //cmb_Currency.DataBind();
        }

        /// <summary>
        /// </summary>
        //private void SaveData()
        //{
        //    // Validation
        //    //if (cmb_ProductCode.Value == null)
        //    //{
        //    //    // Display error message.
        //    //    PopupWindow pcWindow = new PopupWindow("Please enter Product");
        //    //    pcWindow.ShowOnPageLoad = true;
        //    //    pcWindow.Modal = true;
        //    //    ASPxPopupControl.Windows.Add(pcWindow);
        //    //    return;
        //    //}

        //    switch (_dsPo.Tables[_poDt.TableName].Rows.Count)
        //    {
        //        case 0:
        //            {
        //                var drNew = _dsPo.Tables[_poDt.TableName].NewRow();

        //                drNew["PoNo"] = _dsPo.Tables[_po.TableName].Rows[0]["PoNo"].ToString();
        //                drNew["PoDt"] = _dsPo.Tables[_poDt.TableName].Rows.Count + 1;
        //                drNew["RcvQty"] = 0;
        //            }
        //            break;
        //    }
        //}

        /// <summary>
        /// </summary>
        protected void Total()
        {
            _netamt = 0;
            _discAmt = 0;
            _taxamt = 0;
            _totalamt = 0;

            // Added on: 27/08/2017, By: Fon, For: Added about currency field.
            _currNetAmt = 0;
            _currDiscAmt = 0;
            _currTaxAmt = 0;
            _currTotalAmt = 0;

            if (_dsPo.Tables[_poDt.TableName].Rows.Count > 0)
            {
                for (var i = 0; i < _dsPo.Tables[_poDt.TableName].Rows.Count; i++)
                {
                    // Modified by: Fon
                    #region Comment
                    //if (_dsPo.Tables[_poDt.TableName].Rows[i]["NetAmt"].ToString() != string.Empty)
                    //{
                    //    _netamt += decimal.Parse(_dsPo.Tables[_poDt.TableName].Rows[i]["NetAmt"].ToString());
                    //}
                    //else
                    //{
                    //    _netamt += 0;
                    //}

                    //if (_dsPo.Tables[_poDt.TableName].Rows[i]["TaxAmt"].ToString() != string.Empty)
                    //{
                    //    _taxamt += decimal.Parse(_dsPo.Tables[_poDt.TableName].Rows[i]["TaxAmt"].ToString());
                    //}
                    //else
                    //{
                    //    _taxamt += 0;
                    //}
                    #endregion

                    _netamt += (_dsPo.Tables[_poDt.TableName].Rows[i]["NetAmt"].ToString() != string.Empty)
                        ? decimal.Parse(_dsPo.Tables[_poDt.TableName].Rows[i]["NetAmt"].ToString()) : 0;
                    _taxamt += (_dsPo.Tables[_poDt.TableName].Rows[i]["TaxAmt"].ToString() != string.Empty)
                        ? decimal.Parse(_dsPo.Tables[_poDt.TableName].Rows[i]["TaxAmt"].ToString()) : 0;

                    _currNetAmt += (_dsPo.Tables[_poDt.TableName].Rows[i]["CurrNetAmt"].ToString() != string.Empty)
                        ? decimal.Parse(_dsPo.Tables[_poDt.TableName].Rows[i]["CurrNetAmt"].ToString()) : 0;
                    _currTaxAmt += (_dsPo.Tables[_poDt.TableName].Rows[i]["CurrTaxAmt"].ToString() != string.Empty)
                        ? decimal.Parse(_dsPo.Tables[_poDt.TableName].Rows[i]["CurrTaxAmt"].ToString()) : 0;

                    // Added on: 12/02/2018, For: Following from P'Oat request.
                    _currDiscAmt += (_dsPo.Tables[_poDt.TableName].Rows[i]["CurrDiscAmt"].ToString() != string.Empty)
                        ? decimal.Parse(_dsPo.Tables[_poDt.TableName].Rows[i]["CurrDiscAmt"].ToString()) : 0;
                    _discAmt += (_dsPo.Tables[_poDt.TableName].Rows[i]["DisCountAmt"].ToString() != string.Empty)
                        ? decimal.Parse(_dsPo.Tables[_poDt.TableName].Rows[i]["DisCountAmt"].ToString()) : 0;

                    // End Modified.
                }
            }

            _totalamt = _netamt + _taxamt;
            _currTotalAmt = _currNetAmt + _currTaxAmt;
        }

        /// <summary>
        ///     Calculate total in pr
        /// </summary>
        protected void TotalPR()
        {
            _netamtpr = 0;
            _taxamtpr = 0;
            _totalamtpr = 0;

            if (_dtPrDt != null)
            {
                if (_dtPrDt.Rows.Count > 0)
                {
                    for (var i = 0; i < _dtPrDt.Rows.Count; i++)
                    {
                        if (_dtPrDt.Rows[i]["NetAmt"].ToString() != string.Empty)
                        {
                            _netamtpr += decimal.Parse(_dtPrDt.Rows[i]["NetAmt"].ToString());
                        }
                        else
                        {
                            _netamtpr += 0;
                        }

                        if (_dtPrDt.Rows[i]["TaxAmt"].ToString() != string.Empty)
                        {
                            _taxamtpr += decimal.Parse(_dtPrDt.Rows[i]["TaxAmt"].ToString());
                        }
                        else
                        {
                            _taxamtpr += 0;
                        }
                    }
                }

                _totalamtpr = _netamtpr + _taxamtpr;
                //lbl_TPrNet.Text = netamt.ToString();
                //lbl_TPrTax.Text = taxamt.ToString();
                //lbl_TPrAmount.Text = (netamt + taxamt).ToString();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PriceList_BeforePerformDataSelect(object sender, EventArgs e)
        {
            // Display Price Comparison.
            if (_dtPrDt != null)
            {
                var prNo = ((ASPxGridView)sender).GetMasterRowKeyValue().ToString();

                foreach (DataRow drPrDt in _dtPrDt.Rows)
                {
                    if (drPrDt["PRNo"].ToString() == prNo)
                    {
                        Session["ProductCode"] = drPrDt["ProductCode"].ToString();
                        Session["ReqDate"] = drPrDt["ReqDate"].ToString();
                        Session["ApprQty"] = drPrDt["ReqQty"].ToString();
                        break;
                    }
                }
            }
        }

        /// <summary>
        ///     Click expand in grid po
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Expand_Click(object sender, ImageClickEventArgs e)
        {
            //// Change Expand/Collapse Image
            //ImageButton btn_Expand = sender as ImageButton;
            //GridViewRow selectedRow = btn_Expand.Parent.Parent as GridViewRow;
            //GridView grd_PrDt = selectedRow.FindControl("grd_PrDt2") as GridView;

            //// If grid view of transaction detail was display, hide it and change the image to expand
            //// otherwise display it and change image to collapse.
            //if (grd_PrDt.Visible)
            //{
            //    grd_PrDt.Visible = false;
            //    btn_Expand.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Plus.jpg";
            //}
            //else
            //{
            //    grd_PrDt.Visible = true;
            //    btn_Expand.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Minus.jpg";

            //    // Find PoNo and PoDtNo.            
            //    string poNo = lbl_PONumber.Text;
            //    int poDtNo = Convert.ToInt32(grd_PoDt2.DataKeys[selectedRow.RowIndex].Value);

            //    dtPrDt.Clear();
            //    dtPrDt = prDt.GetByPONoPODt(poNo, poDtNo, hf_ConnStr.Value);

            //    this.TotalPR();

            //    grd_PrDt.DataSource = dtPrDt;
            //    grd_PrDt.DataBind();

            //    Session["dtPrDt"] = dtPrDt;
            //    //Session["index"] = e.VisibleIndex;
            //}

            //---- 2011-10-06 ---------------------------------------------------
            //// Change Expand/Collapse Image
            //ImageButton btn_Expand  = sender as ImageButton;
            //GridViewRow selectedRow = btn_Expand.Parent.Parent as GridViewRow;
            //Panel p_PrDail          = selectedRow.FindControl("p_PrDail") as Panel;
            //Label lbl_Buyer         = selectedRow.FindControl("lbl_Buyer") as Label;
            //Label lbl_BU            = selectedRow.FindControl("lbl_BU") as Label;
            //Label lbl_Store         = selectedRow.FindControl("lbl_Store") as Label;
            //Label lbl_DeliDate      = selectedRow.FindControl("lbl_DeliDate") as Label;
            //Label lbl_QtyReq        = selectedRow.FindControl("lbl_QtyReq") as Label;
            //Label lbl_QtyOrd        = selectedRow.FindControl("lbl_QtyOrd") as Label;
            //Label lbl_DeliPoint     = selectedRow.FindControl("lbl_DeliPoint") as Label;
            //Label lbl_PricePR       = selectedRow.FindControl("lbl_PricePR") as Label;
            //Label lbl_PRDate        = selectedRow.FindControl("lbl_PRDate") as Label;
            //Label lbl_TaxType       = selectedRow.FindControl("lbl_TaxType") as Label;
            //Label lbl_NetAmt        = selectedRow.FindControl("lbl_NetAmt") as Label;
            //Label lbl_PRRef         = selectedRow.FindControl("lbl_PRRef") as Label;
            //Label lbl_Disc          = selectedRow.FindControl("lbl_Disc") as Label;
            //Label lbl_DiscAmt       = selectedRow.FindControl("lbl_DiscAmt") as Label;
            //Label lbl_Ref           = selectedRow.FindControl("lbl_Ref") as Label;
            //Label lbl_TaxRate       = selectedRow.FindControl("lbl_TaxRate") as Label;
            //Label lbl_TaxAmt        = selectedRow.FindControl("lbl_TaxAmt") as Label;
            //Label lbl_TotalAmt      = selectedRow.FindControl("lbl_TotalAmt") as Label;
            //Label lbl_Approve       = selectedRow.FindControl("lbl_Approve") as Label;
            //Label lbl_Receive       = selectedRow.FindControl("lbl_Receive") as Label;
            //Label lbl_ConvRate      = selectedRow.FindControl("lbl_ConvRate") as Label;
            //Label lbl_BaseQty       = selectedRow.FindControl("lbl_BaseQty") as Label;

            ////DataTable dtPriceList   = new DataTable();
            ////GridView grd_PriceList  = selectedRow.FindControl("grd_PriceList") as GridView;
            ////GridView grd_PrDt2      = selectedRow.FindControl("grd_PrDt") as GridView;

            //// If grid view of transaction detail was display, hide it and change the image to expand
            //// otherwise display it and change image to collapse.
            //if (p_PrDail.Visible)
            //{
            //    p_PrDail.Visible = false;
            //    btn_Expand.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Plus.jpg";
            //}
            //else
            //{
            //    p_PrDail.Visible = true;
            //    btn_Expand.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Minus.jpg";

            //    // Find data and display about pr detail, stock summary and receive
            //    string poNo = lbl_PONumber.Text;
            //    int poDtNo  = Convert.ToInt32(grd_PoDt.DataKeys[selectedRow.RowIndex].Value);

            //    dtPrDt.Clear();
            //    dtPrDt = prDt.GetByPONoPODt(poNo, poDtNo, hf_ConnStr.Value);

            //    if (dtPrDt.Rows.Count > 0)
            //    {
            //        lbl_Buyer.Text      = dtPrDt.Rows[0]["Buyer"].ToString();
            //        lbl_BU.Text         = dtPrDt.Rows[0]["BUCode"].ToString();
            //        lbl_Store.Text      = storeLct.GetName(dtPrDt.Rows[0]["LocationCode"].ToString(), hf_ConnStr.Value);
            //        lbl_DeliDate.Text   = DateTime.Parse(dtPrDt.Rows[0]["ReqDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            //        lbl_QtyReq.Text     = dtPrDt.Rows[0]["ReqQty"].ToString();
            //        lbl_QtyOrd.Text     = dtPrDt.Rows[0]["OrderQty"].ToString();
            //        lbl_DeliPoint.Text  = deliPoint.GetName(dtPrDt.Rows[0]["DeliPoint"].ToString(), hf_ConnStr.Value);
            //        lbl_PricePR.Text    = String.Format("{0:n}", dtPrDt.Rows[0]["Price"]);
            //        lbl_PRDate.Text     = DateTime.Parse(dtPrDt.Rows[0]["PRDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            //        lbl_TaxType.Text    = GetTaxTypeName(dtPrDt.Rows[0]["TaxType"].ToString());
            //        lbl_NetAmt.Text     = String.Format("{0:n}", dtPrDt.Rows[0]["NetAmt"]);
            //        lbl_PRRef.Text      = dtPrDt.Rows[0]["PRNo1"].ToString();
            //        lbl_Disc.Text       = String.Format("{0:n}", dtPrDt.Rows[0]["DiscPercent"]);
            //        lbl_DiscAmt.Text    = String.Format("{0:n}", dtPrDt.Rows[0]["DiscAmt"]);
            //        lbl_Ref.Text        = dtPrDt.Rows[0]["RefNo"].ToString();
            //        lbl_TaxRate.Text    = String.Format("{0:n}", dtPrDt.Rows[0]["TaxRate"]);
            //        lbl_TaxAmt.Text     = String.Format("{0:n}", dtPrDt.Rows[0]["TaxAmt"]);
            //        lbl_TotalAmt.Text   = String.Format("{0:n}", dtPrDt.Rows[0]["TotalAmt"]);


            //        //dtPriceList         = priceList.GetList(dtPrDt.Rows[0]["ProductCode"].ToString(),
            //        //                                        DateTime.Parse(dtPrDt.Rows[0]["ReqDate"].ToString()),
            //        //                                        decimal.Parse(dtPrDt.Rows[0]["ReqQty"].ToString()), hf_ConnStr.Value);
            //        //grd_PriceList.DataSource = dtPriceList;
            //        //grd_PriceList.DataBind();

            //        //this.TotalPR();

            //        //grd_PrDt2.DataSource = dtPrDt;
            //        //grd_PrDt2.DataBind();
            //    }


            //    Session["dtPrDt"] = dtPrDt;
            //}  
        }

        /// <summary>
        ///     Click expand in grid pr
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ExpandPr_Click(object sender, ImageClickEventArgs e)
        {
            //// Change Expand/Collapse Image
            //ImageButton btn_ExpandPr = sender as ImageButton;
            //GridViewRow selectedRow = btn_ExpandPr.Parent.Parent as GridViewRow;
            //GridView grd_PriceList = selectedRow.FindControl("grd_PriceList") as GridView;
            //Panel p_PrDail = selectedRow.FindControl("p_PrDail") as Panel;
            //DataTable dtPriceList = new DataTable();

            //// If grid view of transaction detail was display, hide it and change the image to expand
            //// otherwise display it and change image to collapse.
            //if (p_PrDail.Visible)
            //{
            //    p_PrDail.Visible = false;
            //    btn_ExpandPr.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Plus.jpg";
            //}
            //else
            //{
            //    p_PrDail.Visible = true;
            //    btn_ExpandPr.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Minus.jpg";

            //    DataTable dtPrDt = (DataTable)Session["dtPrDt"];

            //    dtPriceList = priceList.GetList(dtPrDt.Rows[selectedRow.RowIndex]["ProductCode"].ToString(),
            //                                    DateTime.Parse(dtPrDt.Rows[selectedRow.RowIndex]["ReqDate"].ToString()),
            //                                    decimal.Parse(dtPrDt.Rows[selectedRow.RowIndex]["ReqQty"].ToString()), 
            //                                    dtPrDt.Rows[selectedRow.RowIndex]["OrderUnit"].ToString(),
            //                                    hf_ConnStr.Value);

            //    grd_PriceList.DataSource = dtPriceList;
            //    grd_PriceList.DataBind();

            //    //Session["ProductCode"]  = dtPrDt.Rows[selectedRow.RowIndex]["ProductCode"].ToString();
            //    //Session["ReqDate"]      = dtPrDt.Rows[selectedRow.RowIndex]["ReqDate"].ToString(); 
            //    //Session["ApprQty"]      = dtPrDt.Rows[selectedRow.RowIndex]["ReqQty"].ToString();                
            //}
        }

        /// <summary>
        ///     Binding field in grid po
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PoDt2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            #region DataRow
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_SKU") != null)
                {
                    var lblSku = (Label)e.Row.FindControl("lbl_SKU");
                    lblSku.Text = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();
                }

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lblUnit = (Label)e.Row.FindControl("lbl_Unit");
                    lblUnit.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                }


                if (e.Row.FindControl("lbl_QTYOrder") != null)
                {
                    var lblQtyOrder = (Label)e.Row.FindControl("lbl_QTYOrder");
                    lblQtyOrder.Text = DataBinder.Eval(e.Row.DataItem, "OrdQty").ToString();
                }


                if (e.Row.FindControl("lbl_FOC") != null)
                {
                    var lblFoc = (Label)e.Row.FindControl("lbl_FOC");
                    lblFoc.Text = DataBinder.Eval(e.Row.DataItem, "FOCQty").ToString();
                }


                if (e.Row.FindControl("lbl_RCV") != null)
                {
                    var lblRcv = (Label)e.Row.FindControl("lbl_RCV");
                    lblRcv.Text = DataBinder.Eval(e.Row.DataItem, "RcvQty").ToString();
                }


                if (e.Row.FindControl("lbl_Cancel") != null)
                {
                    var lblCancel = (Label)e.Row.FindControl("lbl_Cancel");
                    lblCancel.Text = DataBinder.Eval(e.Row.DataItem, "CancelQty").ToString();
                }

                if (e.Row.FindControl("lbl_Price") != null)
                {
                    var lblPrice = (Label)e.Row.FindControl("lbl_Price");
                    lblPrice.Text = string.Format("{0:N4}", DataBinder.Eval(e.Row.DataItem, "Price"));
                }

                if (e.Row.FindControl("chk_Adj") != null)
                {
                    var chkAdj = (CheckBox)e.Row.FindControl("chk_Adj");
                    chkAdj.Checked = (bool)DataBinder.Eval(e.Row.DataItem, "IsAdj");
                }

                #region About Amount
                if (e.Row.FindControl("lbl_Net") != null)
                {
                    var lblNet = (Label)e.Row.FindControl("lbl_Net");
                    lblNet.Text = DataBinder.Eval(e.Row.DataItem, "NetAmt").ToString();
                }

                if (e.Row.FindControl("lbl_Tax") != null)
                {
                    var lblTax = (Label)e.Row.FindControl("lbl_Tax");
                    lblTax.Text = DataBinder.Eval(e.Row.DataItem, "TaxAmt").ToString();
                }

                if (e.Row.FindControl("lbl_Amount") != null)
                {
                    var lblAmount = (Label)e.Row.FindControl("lbl_Amount");
                    lblAmount.Text = DataBinder.Eval(e.Row.DataItem, "TotalAmt").ToString();
                }
                #endregion
            }
            #endregion

            #region Footer
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (e.Row.FindControl("lbl_TNet") != null)
                {
                    var lblTNet = (Label)e.Row.FindControl("lbl_TNet");
                    lblTNet.Text = _netamt.ToString();
                }

                if (e.Row.FindControl("lbl_TTax") != null)
                {
                    var lblTTax = (Label)e.Row.FindControl("lbl_TTax");
                    lblTTax.Text = _taxamt.ToString();
                }

                if (e.Row.FindControl("lbl_TAmount") != null)
                {
                    var lblTAmount = (Label)e.Row.FindControl("lbl_TAmount");
                    lblTAmount.Text = _totalamt.ToString();
                }

                // Added on: 12/02/2018, By: Fon, For: Following from P'Oat request.
                if (e.Row.FindControl("lbl_TDisc") != null)
                {
                    Label lbl_TDisc = (Label)e.Row.FindControl("lbl_TDisc");
                    lbl_TDisc.Text = _discAmt.ToString();
                }
                // End Added.
            }
            #endregion
        }

        /// <summary>
        ///     Binding field in grid pr
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PrDt2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_SKU") != null)
                {
                    var lblSku = (Label)e.Row.FindControl("lbl_SKU");
                    lblSku.Text = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();
                }

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lblUnit = (Label)e.Row.FindControl("lbl_Unit");
                    lblUnit.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                }

                if (e.Row.FindControl("lbl_QTYOrder") != null)
                {
                    var lblQtyOrder = (Label)e.Row.FindControl("lbl_QTYOrder");
                    lblQtyOrder.Text = DataBinder.Eval(e.Row.DataItem, "OrderQty").ToString();
                }

                if (e.Row.FindControl("lbl_FOC") != null)
                {
                    var lblFoc = (Label)e.Row.FindControl("lbl_FOC");
                    lblFoc.Text = DataBinder.Eval(e.Row.DataItem, "FOCQty").ToString();
                }

                if (e.Row.FindControl("lbl_RCV") != null)
                {
                    var lblRcv = (Label)e.Row.FindControl("lbl_RCV");
                    lblRcv.Text = DataBinder.Eval(e.Row.DataItem, "RcvQty").ToString();
                }

                if (e.Row.FindControl("lbl_Cancel") != null)
                {
                    var lblCancel = (Label)e.Row.FindControl("lbl_Cancel");
                    lblCancel.Text = DataBinder.Eval(e.Row.DataItem, "CancelQty").ToString();
                }

                if (e.Row.FindControl("lbl_grdPrice") != null)
                {
                    var lblGrdPrice = (Label)e.Row.FindControl("lbl_grdPrice");
                    lblGrdPrice.Text = DataBinder.Eval(e.Row.DataItem, "Price").ToString();
                }

                if (e.Row.FindControl("chk_Adj") != null)
                {
                    var chkAdj = (CheckBox)e.Row.FindControl("chk_Adj");
                    chkAdj.Checked = (bool)DataBinder.Eval(e.Row.DataItem, "TaxAdj");
                }

                if (e.Row.FindControl("lbl_Net") != null)
                {
                    var lblNet = (Label)e.Row.FindControl("lbl_Net");
                    lblNet.Text = DataBinder.Eval(e.Row.DataItem, "NetAmt").ToString();
                }

                if (e.Row.FindControl("lbl_Tax") != null)
                {
                    var lblTax = (Label)e.Row.FindControl("lbl_Tax");
                    lblTax.Text = DataBinder.Eval(e.Row.DataItem, "TaxAmt").ToString();
                }

                if (e.Row.FindControl("lbl_Amount") != null)
                {
                    var lblAmount = (Label)e.Row.FindControl("lbl_Amount");
                    lblAmount.Text = DataBinder.Eval(e.Row.DataItem, "TotalAmt").ToString();
                }

                // Display Transaction Detail ---------------------------------------------------------
                if (e.Row.FindControl("lbl_Store") != null)
                {
                    var lblStore = (Label)e.Row.FindControl("lbl_Store");
                    lblStore.Text = //DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString() + " : " +
                        _storeLct.GetName(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(), hf_ConnStr.Value);
                    //----02/03/2012----storeLct.GetName2(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(), hf_ConnStr.Value);
                }

                if (e.Row.FindControl("lbl_DeliveryPoint") != null)
                {
                    var lblDeliPoint = (Label)e.Row.FindControl("lbl_DeliveryPoint");
                    lblDeliPoint.Text = //DataBinder.Eval(e.Row.DataItem, "DeliPoint").ToString() + " : " +
                        _deliPoint.GetName(DataBinder.Eval(e.Row.DataItem, "DeliPoint").ToString(), hf_ConnStr.Value);
                }

                if (e.Row.FindControl("lbl_Price") != null)
                {
                    var lblPrice = (Label)e.Row.FindControl("lbl_Price");
                    lblPrice.Text = string.Format("{0:N4}", DataBinder.Eval(e.Row.DataItem, "Price"));
                }

                if (e.Row.FindControl("lbl_DiscPercent") != null)
                {
                    var lblDiscPercent = (Label)e.Row.FindControl("lbl_DiscPercent");
                    lblDiscPercent.Text = DataBinder.Eval(e.Row.DataItem, "DiscPercent").ToString();
                }

                if (e.Row.FindControl("lbl_DiscAmt") != null)
                {
                    var lblDiscAmt = (Label)e.Row.FindControl("lbl_DiscAmt");
                    lblDiscAmt.Text = DataBinder.Eval(e.Row.DataItem, "DiscAmt").ToString();
                }

                if (e.Row.FindControl("lbl_TaxType") != null)
                {
                    var lblTaxType = (Label)e.Row.FindControl("lbl_TaxType");
                    lblTaxType.Text = DataBinder.Eval(e.Row.DataItem, "TaxType").ToString();
                }

                if (e.Row.FindControl("lbl_TaxRate") != null)
                {
                    var lblTaxRate = (Label)e.Row.FindControl("lbl_TaxRate");
                    lblTaxRate.Text = DataBinder.Eval(e.Row.DataItem, "TaxRate").ToString();
                }

                if (e.Row.FindControl("lbl_RefNo") != null)
                {
                    var lblRefNo = (Label)e.Row.FindControl("lbl_RefNo");
                    lblRefNo.Text = DataBinder.Eval(e.Row.DataItem, "RefNo").ToString();
                }

                if (e.Row.FindControl("lbl_ReqQty") != null)
                {
                    var lblReqQty = (Label)e.Row.FindControl("lbl_ReqQty");
                    lblReqQty.Text = DataBinder.Eval(e.Row.DataItem, "ReqQty").ToString();
                }

                if (e.Row.FindControl("lbl_PRNo") != null)
                {
                    var lblPRNo = (Label)e.Row.FindControl("lbl_PRNo");
                    lblPRNo.Text = DataBinder.Eval(e.Row.DataItem, "PRNo1").ToString();
                }

                if (e.Row.FindControl("lbl_PRDate") != null)
                {
                    var lblPRDate = (Label)e.Row.FindControl("lbl_PRDate");
                    lblPRDate.Text = String.Format("{0:d/M/yyyy HH:mm:ss}", DataBinder.Eval(e.Row.DataItem, "PRDate"));
                    //DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "PRDate").ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
                }

                // Comment ----------------------------------------------------------------------------
                if (e.Row.FindControl("lbl_Comment") != null)
                {
                    var lblComment = (Label)e.Row.FindControl("lbl_Comment");
                    lblComment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                }

                // Display Stock Summary --------------------------------------------------------------      
                if (e.Row.FindControl("uc_StockSummary") != null)
                {
                    var ucStockSummary = (StockSummary)e.Row.FindControl("uc_StockSummary");

                    ucStockSummary.ProductCode = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();
                    ucStockSummary.LocationCode = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString();
                    ucStockSummary.ConnStr = hf_ConnStr.Value;
                    ucStockSummary.DataBind();
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (e.Row.FindControl("lbl_TPrNet") != null)
                {
                    var lblTPrNet = (Label)e.Row.FindControl("lbl_TPrNet");
                    lblTPrNet.Text = _netamtpr.ToString();
                }

                if (e.Row.FindControl("lbl_TPrTax") != null)
                {
                    var lblTPrTax = (Label)e.Row.FindControl("lbl_TPrTax");
                    lblTPrTax.Text = _taxamtpr.ToString();
                }

                if (e.Row.FindControl("lbl_TPrAmount") != null)
                {
                    var lblTPrAmount = (Label)e.Row.FindControl("lbl_TPrAmount");
                    lblTPrAmount.Text = _totalamtpr.ToString();
                }
            }
        }

        /// <summary>
        ///     Binding field in grid price list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PriceList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (e.Row.FindControl("lbl_Ref") != null)
                    {
                        var lblRef = (Label)e.Row.FindControl("lbl_Ref");
                        lblRef.Text = DataBinder.Eval(e.Row.DataItem, "RefNo").ToString();
                    }
                    if (e.Row.FindControl("lbl_Name") != null)
                    {
                        var lblName = (Label)e.Row.FindControl("lbl_Name");
                        lblName.Text = DataBinder.Eval(e.Row.DataItem, "VendorName").ToString();
                    }
                    if (e.Row.FindControl("lbl_Rank") != null)
                    {
                        var lblRank = (Label)e.Row.FindControl("lbl_Rank");
                        lblRank.Text = DataBinder.Eval(e.Row.DataItem, "VendorRank").ToString();
                    }
                    if (e.Row.FindControl("lbl_Price") != null)
                    {
                        var lblPrice = (Label)e.Row.FindControl("lbl_Price");
                        lblPrice.Text = string.Format("{0:N4}", DataBinder.Eval(e.Row.DataItem, "Price").ToString());
                    }
                    if (e.Row.FindControl("lbl_Disc") != null)
                    {
                        var lblDisc = (Label)e.Row.FindControl("lbl_Disc");
                        lblDisc.Text = DataBinder.Eval(e.Row.DataItem, "DiscountPercent").ToString();
                    }
                    if (e.Row.FindControl("lbl_Amount") != null)
                    {
                        var lblAmount = (Label)e.Row.FindControl("lbl_Amount");
                        lblAmount.Text = DataBinder.Eval(e.Row.DataItem, "DiscountAmt").ToString();
                    }
                    if (e.Row.FindControl("lbl_FOC") != null)
                    {
                        var lblFoc = (Label)e.Row.FindControl("lbl_FOC");
                        lblFoc.Text = DataBinder.Eval(e.Row.DataItem, "FOC").ToString();
                    }
                    if (e.Row.FindControl("lbl_QtyF") != null)
                    {
                        var lblQtyF = (Label)e.Row.FindControl("lbl_QtyF");
                        lblQtyF.Text = DataBinder.Eval(e.Row.DataItem, "QtyFrom").ToString();
                    }
                    if (e.Row.FindControl("lbl_QtyT") != null)
                    {
                        var lblQtyT = (Label)e.Row.FindControl("lbl_QtyT");
                        lblQtyT.Text = DataBinder.Eval(e.Row.DataItem, "QtyTo").ToString();
                    }
                    break;
            }
        }

        protected void grd_PoDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                #region DataRow
                case DataControlRowType.DataRow:
                    {
                        if (e.Row.FindControl("lbl_SKU") != null)
                        {
                            var lblSku = (Label)e.Row.FindControl("lbl_SKU");
                            lblSku.Text = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();
                            lblSku.ToolTip = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();
                        }

                        //if (e.Row.FindControl("lbl_Unit") != null)
                        //{
                        //    Label lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                        //    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                        //}

                        if (e.Row.FindControl("lbl_QTYOrder") != null)
                        {
                            var lblQtyOrder = (Label)e.Row.FindControl("lbl_QTYOrder");
                            lblQtyOrder.Text = DataBinder.Eval(e.Row.DataItem, "OrdQty") + " " +
                                               DataBinder.Eval(e.Row.DataItem, "Unit");
                        }

                        if (e.Row.FindControl("txt_QtyOrder") != null)
                        {
                            var txtQtyOrder = (TextBox)e.Row.FindControl("txt_QtyOrder");
                            txtQtyOrder.Text = DataBinder.Eval(e.Row.DataItem, "OrdQty").ToString();
                        }

                        if (e.Row.FindControl("cmb_Unit") != null)
                        {
                            var lnbEdit = (LinkButton)e.Row.FindControl("lnb_Edit");
                            var lnbCancel = (LinkButton)e.Row.FindControl("lnb_Cancel");
                            var cmbUnit = (ASPxComboBox)e.Row.FindControl("cmb_Unit");
                            cmbUnit.DataSource =
                                _prodUnit.GetLookUp_ProductCode(DataBinder.Eval(e.Row.DataItem, "Product").ToString(),
                                    hf_ConnStr.Value);
                            cmbUnit.DataBind();
                            cmbUnit.Value = DataBinder.Eval(e.Row.DataItem, "Unit");

                            //if (dsPo.Tables[po.TableName].Rows[0]["DocStatus"].ToString() == "Approved")
                            //{
                            ////    cmb_Unit.Enabled   = true;
                            ////    lnb_Edit.Enabled   = true;
                            ////    lnb_Cancel.Enabled = true;
                            //}
                        }

                        if (e.Row.FindControl("lbl_FOC") != null)
                        {
                            var lblFoc = (Label)e.Row.FindControl("lbl_FOC");
                            lblFoc.Text = DataBinder.Eval(e.Row.DataItem, "FOCQty").ToString();
                        }

                        if (e.Row.FindControl("lbl_RCV") != null)
                        {
                            var lblRcv = (Label)e.Row.FindControl("lbl_RCV");
                            lblRcv.Text = DataBinder.Eval(e.Row.DataItem, "RcvQty").ToString();
                        }

                        if (e.Row.FindControl("lbl_Cancel") != null)
                        {
                            var lblCancel = (Label)e.Row.FindControl("lbl_Cancel");
                            lblCancel.Text = DataBinder.Eval(e.Row.DataItem, "CancelQty").ToString();
                        }

                        if (e.Row.FindControl("lbl_Price") != null)
                        {
                            var lblPrice = (Label)e.Row.FindControl("lbl_Price");
                            lblPrice.Text = String.Format("{0:N4}", DataBinder.Eval(e.Row.DataItem, "Price"));
                        }

                        if (e.Row.FindControl("chk_Adj") != null)
                        {
                            var chkAdj = (CheckBox)e.Row.FindControl("chk_Adj");
                            chkAdj.Checked = (bool)DataBinder.Eval(e.Row.DataItem, "IsAdj");
                        }

                        //if (e.Row.FindControl("lbl_Net") != null)
                        //{
                        //    Label lbl_Net = e.Row.FindControl("lbl_Net") as Label;
                        //    lbl_Net.Text  = String.Format("{0:n}", DataBinder.Eval(e.Row.DataItem, "NetAmt"));
                        //}

                        //if (e.Row.FindControl("lbl_Tax") != null)
                        //{
                        //    Label lbl_Tax = e.Row.FindControl("lbl_Tax") as Label;
                        //    lbl_Tax.Text  = String.Format("{0:n}", DataBinder.Eval(e.Row.DataItem, "TaxAmt"));
                        //}

                        if (e.Row.FindControl("lbl_Amount") != null)
                        {
                            var lblAmount = (Label)e.Row.FindControl("lbl_Amount");
                            //lbl_Amount.Text  = String.Format("{0:n}", DataBinder.Eval(e.Row.DataItem, "TotalAmt"));

                            //2012-01-17 เปลี่ยนการแสดงข้อมูลจาก TotalAmt เป็น NetAmt เนื่องจาก TotalAmt จะเป็นค่า Price+Tax 
                            //lblAmount.Text = String.Format("{0:n}", DataBinder.Eval(e.Row.DataItem, "NetAmt"));
                            // Modified on: 29/08/2017, By: Fon, For: Follow currency
                            lblAmount.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt"));
                        }

                        if (e.Row.FindControl("lbl_NetAC") != null)
                        {
                            var lblNetAc = (Label)e.Row.FindControl("lbl_NetAC");
                            var strProd = DataBinder.Eval(e.Row.DataItem, "Product").ToString();
                            lblNetAc.Text = _accMapp.GetA3Code(DataBinder.Eval(e.Row.DataItem, "BUCode").ToString(),
                                DataBinder.Eval(e.Row.DataItem, "Location").ToString(),
                                strProd.Substring(0, 4), hf_ConnStr.Value);
                        }

                        if (e.Row.FindControl("lbl_TaxAC") != null)
                        {
                            var lblTaxAc = (Label)e.Row.FindControl("lbl_TaxAC");
                            lblTaxAc.Text = _product.GetTaxAccCode(DataBinder.Eval(e.Row.DataItem, "Product").ToString(),
                                hf_ConnStr.Value);
                        }

                        //----------------------- Expand Po Detail, Stock Summary and Pr Detail ----------------------------
                        //Binding display Po Detail.
                        #region
                        if (e.Row.FindControl("lbl_Receive") != null)
                        {
                            var lblReceive = (Label)e.Row.FindControl("lbl_Receive");
                            lblReceive.Text = DataBinder.Eval(e.Row.DataItem, "RcvQty").ToString();
                            ;
                        }

                        if (e.Row.FindControl("lbl_Comment") != null)
                        {
                            var lblComment = (Label)e.Row.FindControl("lbl_Comment");
                            lblComment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                        }

                        if (e.Row.FindControl("lbl_TaxType") != null)
                        {
                            var lblTaxType = (Label)e.Row.FindControl("lbl_TaxType");
                            lblTaxType.Text =
                                GetTaxTypeName(DataBinder.Eval(e.Row.DataItem, "TaxType").ToString());
                        }

                        if (e.Row.FindControl("lbl_Disc") != null)
                        {
                            var lblDisc = (Label)e.Row.FindControl("lbl_Disc");
                            lblDisc.Text = DataBinder.Eval(e.Row.DataItem, "Discount") + " %";
                        }

                        if (e.Row.FindControl("lbl_TaxRate") != null)
                        {
                            var lblTaxRate = (Label)e.Row.FindControl("lbl_TaxRate");
                            lblTaxRate.Text = DataBinder.Eval(e.Row.DataItem, "TaxRate") + " %";

                        }

                        if (e.Row.FindControl("lbl_NetAmt") != null)
                        {
                            var lblNetAmt = (Label)e.Row.FindControl("lbl_NetAmt");
                            lblNetAmt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "NetAmt"));
                        }

                        if (e.Row.FindControl("lbl_DiscAmt") != null)
                        {
                            var lblDiscAmt = (Label)e.Row.FindControl("lbl_DiscAmt");
                            lblDiscAmt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "DisCountAmt"));
                        }

                        if (e.Row.FindControl("lbl_TaxAmt") != null)
                        {
                            var lblTaxAmt = (Label)e.Row.FindControl("lbl_TaxAmt");
                            lblTaxAmt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TaxAmt"));
                        }

                        if (e.Row.FindControl("lbl_TotalAmt") != null)
                        {
                            var lblTotalAmt = (Label)e.Row.FindControl("lbl_TotalAmt");
                            lblTotalAmt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TotalAmt"));
                        }

                        // Added on: 29/08/2017, By: Fon
                        if (e.Row.FindControl("lbl_CurrCurrDt") != null)
                        {
                            Label lbl_CurrCurrDt = (Label)e.Row.FindControl("lbl_CurrCurrDt");
                            lbl_CurrCurrDt.Text = string.Format("( {0} )", lbl_Currency.Text);
                        }
                        if (e.Row.FindControl("lbl_BaseCurrDt") != null)
                        {
                            Label lbl_BaseCurrDt = (Label)e.Row.FindControl("lbl_BaseCurrDt");
                            lbl_BaseCurrDt.Text = string.Format("( {0} )", baseCurrency);
                        }

                        if (e.Row.FindControl("lbl_CurrNetAmt") != null)
                        {
                            Label lbl_CurrNetAmt = (Label)e.Row.FindControl("lbl_CurrNetAmt");
                            lbl_CurrNetAmt.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrNetAmt"));
                        }

                        if (e.Row.FindControl("lbl_CurrDiscAmt") != null)
                        {
                            Label lbl_CurrDiscAmt = (Label)e.Row.FindControl("lbl_CurrDiscAmt");
                            lbl_CurrDiscAmt.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrDiscAmt"));
                        }

                        if (e.Row.FindControl("lbl_CurrTaxAmt") != null)
                        {
                            Label lbl_CurrTaxAmt = (Label)e.Row.FindControl("lbl_CurrTaxAmt");
                            lbl_CurrTaxAmt.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrTaxAmt"));
                        }

                        if (e.Row.FindControl("lbl_CurrTotalAmt") != null)
                        {
                            Label lbl_CurrTotalAmt = (Label)e.Row.FindControl("lbl_CurrTotalAmt");
                            lbl_CurrTotalAmt.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt"));
                        }
                        #endregion
                        //Binding display Stock Summary.
                        _dsStockSum.Clear();
                        var strConnStr = _bu.GetConnectionString(DataBinder.Eval(e.Row.DataItem, "BUCode").ToString());
                        var getPrDtStockSum = _prDt.GetStockSummary(_dsStockSum,
                            DataBinder.Eval(e.Row.DataItem, "Product").ToString(),
                            DataBinder.Eval(e.Row.DataItem, "Location").ToString(),
                            txt_PODate.Text, strConnStr);
                        if (getPrDtStockSum)
                        {
                            if (e.Row.FindControl("lbl_OnHand") != null)
                            {
                                var lblOnHand = (Label)e.Row.FindControl("lbl_OnHand");
                                lblOnHand.Text = _dsStockSum.Tables[_prDt.TableName].Rows[0]["OnHand"].ToString();
                            }

                            if (e.Row.FindControl("lbl_OnOrder") != null)
                            {
                                var lblOnOrder = (Label)e.Row.FindControl("lbl_OnOrder");
                                lblOnOrder.Text = String.Format(DefaultQtyFmt, _dsStockSum.Tables[_prDt.TableName].Rows[0]["OnOrder"]);
                            }

                            if (e.Row.FindControl("lbl_Restock") != null)
                            {
                                var lblRestock = (Label)e.Row.FindControl("lbl_Restock");
                                lblRestock.Text = string.Format(DefaultQtyFmt, _dsStockSum.Tables[_prDt.TableName].Rows[0]["Restock"]);
                            }

                            if (e.Row.FindControl("lbl_Reorder") != null)
                            {
                                var lblReorder = (Label)e.Row.FindControl("lbl_Reorder");
                                lblReorder.Text = String.Format(DefaultQtyFmt, _dsStockSum.Tables[_prDt.TableName].Rows[0]["Reorder"]);
                            }

                            if (e.Row.FindControl("lbl_LastVendor") != null)
                            {
                                var lblLastVendor = (Label)e.Row.FindControl("lbl_LastVendor");
                                lblLastVendor.Text = _dsStockSum.Tables[_prDt.TableName].Rows[0]["LastVendor"].ToString();
                            }

                            if (e.Row.FindControl("lbl_LastPrice") != null)
                            {
                                var lblLastPrice = (Label)e.Row.FindControl("lbl_LastPrice");
                                lblLastPrice.Text = String.Format(DefaultAmtFmt, _dsStockSum.Tables[_prDt.TableName].Rows[0]["LastPrice"]);
                            }
                        }

                        //Binding display Pr Detail.
                        var poNo = lbl_PONumber.Text;
                        var poDtNo = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "PoDt").ToString());

                        _dtPrDt.Clear();
                        _dtPrDt = _prDt.GetByPONoPODt(poNo, poDtNo, hf_ConnStr.Value);

                        if (_dtPrDt.Rows.Count > 0)
                        {
                            if (e.Row.FindControl("lbl_Buyer") != null)
                            {
                                var lblBuyer = (Label)e.Row.FindControl("lbl_Buyer");
                                lblBuyer.Text = _dtPrDt.Rows[0]["Buyer"].ToString();
                            }

                            if (e.Row.FindControl("lbl_QtyReq") != null)
                            {
                                var lblQtyReq = (Label)e.Row.FindControl("lbl_QtyReq");
                                lblQtyReq.Text = string.Format("{0} {1}", string.Format(DefaultQtyFmt, _dtPrDt.Rows[0]["ReqQty"]), _dtPrDt.Rows[0]["Unit"]);
                            }

                            if (e.Row.FindControl("lbl_PricePR") != null)
                            {
                                var lblPricePR = (Label)e.Row.FindControl("lbl_PricePR");
                                lblPricePR.Text = String.Format(DefaultAmtFmt, _dtPrDt.Rows[0]["Price"]);
                            }

                            if (e.Row.FindControl("lbl_BU") != null)
                            {
                                var lblBu = (Label)e.Row.FindControl("lbl_BU");
                                lblBu.Text = _dtPrDt.Rows[0]["BUCode"].ToString();
                            }

                            if (e.Row.FindControl("lbl_Store") != null)
                            {
                                var lblStore = (Label)e.Row.FindControl("lbl_Store");
                                lblStore.Text = _storeLct.GetName(_dtPrDt.Rows[0]["LocationCode"].ToString(),
                                    hf_ConnStr.Value);
                                //----02/03/2012----storeLct.GetName2(dtPrDt.Rows[0]["LocationCode"].ToString(), hf_ConnStr.Value);
                            }

                            if (e.Row.FindControl("lbl_PRRef") != null)
                            {
                                var lblPRRef = (Label)e.Row.FindControl("lbl_PRRef");
                                lblPRRef.Text = _dtPrDt.Rows[0]["PRNo1"].ToString();
                            }

                            if (e.Row.FindControl("lbl_PRDate") != null)
                            {
                                var lblPRDate = (Label)e.Row.FindControl("lbl_PRDate");
                                lblPRDate.Text = String.Format("{0:d/M/yyyy HH:mm:ss}", _dtPrDt.Rows[0]["PRDate"]);
                                //DateTime.Parse(dtPrDt.Rows[0]["PRDate"].ToString()).ToString();
                            }

                            if (e.Row.FindControl("lbl_Approve") != null)
                            {
                                var lblPricePR = (Label)e.Row.FindControl("lbl_Approve");
                                lblPricePR.Text = String.Format(DefaultQtyFmt, _dtPrDt.Rows[0]["ApprQty"]);
                            }

                            if (e.Row.FindControl("grd_PR") != null)
                            {
                                var grdPR = (GridView)e.Row.FindControl("grd_PR");
                                grdPR.DataSource = _dtPrDt;
                                grdPR.DataBind();
                            }
                        }

                        //Binding display Rec Detail.
                        _dsRecDt.Clear();

                        _recDt.GetRecDtByPoNoAndPoDtNo(_dsRecDt, poNo, poDtNo, strConnStr);
                        //hf_ConnStr.Value

                        if (_dsRecDt.Tables[_recDt.TableName].Rows.Count > 0)
                        {
                            var drRecDt = _dsRecDt.Tables[_recDt.TableName].Rows[0];

                            if (e.Row.FindControl("lbl_Receive") != null)
                            {
                                var lblReceive = (Label)e.Row.FindControl("lbl_Receive");
                                lblReceive.Text = String.Format("{0} {1}", string.Format(DefaultQtyFmt, drRecDt["RecQty"]), drRecDt["RcvUnit"]);
                            }

                            if (e.Row.FindControl("lbl_ConvRate") != null)
                            {
                                var lblConvRate = (Label)e.Row.FindControl("lbl_ConvRate");
                                lblConvRate.Text = String.Format("{0:n}", drRecDt["Rate"]);
                            }

                            if (e.Row.FindControl("lbl_BaseQty") != null)
                            {
                                var lblBaseQty = (Label)e.Row.FindControl("lbl_BaseQty");
                                var strProd = DataBinder.Eval(e.Row.DataItem, "Product").ToString();
                                var decBaseQty = _prodUnit.GetQtyAfterChangeUnit(strProd,
                                    drRecDt["RcvUnit"].ToString(), drRecDt["UnitCode"].ToString(),
                                    decimal.Parse(drRecDt["OrderQty"].ToString()), hf_ConnStr.Value);
                                var strBaseUnit = _prodUnit.GetInvenUnit(strProd, hf_ConnStr.Value);
                                lblBaseQty.Text = string.Format("{0} {1}", string.Format(DefaultQtyFmt, decBaseQty), strBaseUnit);
                            }
                        }
                    }
                    break;
                #endregion
            }
        }

        protected void grd_PoDt_RowEditing(object sender, GridViewEditEventArgs e)
        {
            txt_CurrRate.Enabled = false;
            grd_PoDt.DataSource = _dsPo.Tables[_poDt.TableName];
            grd_PoDt.EditIndex = e.NewEditIndex;
            grd_PoDt.DataBind();
        }

        protected void grd_PoDt_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grd_PoDt.DataSource = _dsPo.Tables[_poDt.TableName];
            grd_PoDt.EditIndex = -1;
            grd_PoDt.DataBind();

            switch (_dsPo.Tables[_po.TableName].Rows[0]["DocStatus"].ToString())
            {
                case "Approved":
                    for (var i = 0; i < grd_PoDt.Rows.Count; i++)
                    {
                        var lnbEdit = (LinkButton)grd_PoDt.Rows[i].FindControl("lnb_Edit");
                        lnbEdit.Visible = true;

                        var lnbCancel = (LinkButton)grd_PoDt.Rows[i].FindControl("lnb_Cancel");
                        lnbCancel.Visible = true;
                    }
                    break;
            }

            txt_CurrRate.Enabled = true;
        }

        protected void grd_PoDt_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Prepare save data to Po Detail
            var cmbUnit = (ASPxComboBox)grd_PoDt.Rows[grd_PoDt.EditIndex].FindControl("cmb_Unit");
            var txtQtyOrder = (TextBox)grd_PoDt.Rows[grd_PoDt.EditIndex].FindControl("txt_QtyOrder");

            var drUpdating = _dsPo.Tables[_poDt.TableName].Rows[grd_PoDt.EditIndex];
            drUpdating["Unit"] = cmbUnit.Value.ToString();
            drUpdating["OrdQty"] = txtQtyOrder.Text;

            grd_PoDt.DataSource = _dsPo.Tables[_poDt.TableName];
            grd_PoDt.EditIndex = -1;
            grd_PoDt.DataBind();

            if (_dsPo.Tables[_po.TableName].Rows[0]["DocStatus"].ToString() == "Approved")
            {
                for (var i = 0; i < grd_PoDt.Rows.Count; i++)
                {
                    var lnbEdit = (LinkButton)grd_PoDt.Rows[i].FindControl("lnb_Edit");
                    lnbEdit.Visible = true;

                    var lnbCancel = (LinkButton)grd_PoDt.Rows[i].FindControl("lnb_Cancel");
                    lnbCancel.Visible = true;
                }
            }

            txt_CurrRate.Enabled = true;
            Session["dsPo"] = _dsPo;
        }

        protected void grd_PrDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_SKU") != null)
                {
                    var lblSku = (Label)e.Row.FindControl("lbl_SKU");
                    lblSku.Text = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();
                    lblSku.ToolTip = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();
                }

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lblUnit = (Label)e.Row.FindControl("lbl_Unit");
                    lblUnit.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                }

                if (e.Row.FindControl("lbl_QTYOrder") != null)
                {
                    var lblQtyOrder = (Label)e.Row.FindControl("lbl_QTYOrder");
                    lblQtyOrder.Text = DataBinder.Eval(e.Row.DataItem, "OrderQty").ToString();
                }

                if (e.Row.FindControl("lbl_FOC") != null)
                {
                    var lblFoc = (Label)e.Row.FindControl("lbl_FOC");
                    lblFoc.Text = DataBinder.Eval(e.Row.DataItem, "FOCQty").ToString();
                }

                if (e.Row.FindControl("lbl_RCV") != null)
                {
                    var lblRcv = (Label)e.Row.FindControl("lbl_RCV");
                    lblRcv.Text = DataBinder.Eval(e.Row.DataItem, "RcvQty").ToString();
                }

                if (e.Row.FindControl("lbl_Cancel") != null)
                {
                    var lblCancel = (Label)e.Row.FindControl("lbl_Cancel");
                    lblCancel.Text = DataBinder.Eval(e.Row.DataItem, "CancelQty").ToString();
                }

                if (e.Row.FindControl("lbl_grdPrice") != null)
                {
                    var lblGrdPrice = (Label)e.Row.FindControl("lbl_grdPrice");
                    lblGrdPrice.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "Price"));
                }

                if (e.Row.FindControl("chk_Adj") != null)
                {
                    var chkAdj = (CheckBox)e.Row.FindControl("chk_Adj");
                    chkAdj.Checked = (bool)DataBinder.Eval(e.Row.DataItem, "TaxAdj");
                }

                if (e.Row.FindControl("lbl_Net") != null)
                {
                    var lblNet = (Label)e.Row.FindControl("lbl_Net");
                    lblNet.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "NetAmt"));
                }

                if (e.Row.FindControl("lbl_Tax") != null)
                {
                    var lblTax = (Label)e.Row.FindControl("lbl_Tax");
                    lblTax.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TaxAmt"));
                }

                if (e.Row.FindControl("lbl_Amount") != null)
                {
                    var lblAmount = (Label)e.Row.FindControl("lbl_Amount");
                    lblAmount.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TotalAmt"));
                }

                // Display Transaction Detail ---------------------------------------------------------
                if (e.Row.FindControl("lbl_Store") != null)
                {
                    var lblStore = (Label)e.Row.FindControl("lbl_Store");
                    lblStore.Text = //DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString() + " : " +
                        _storeLct.GetName(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(), hf_ConnStr.Value);
                    //----02/03/2012----storeLct.GetName2(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(), hf_ConnStr.Value);
                }

                if (e.Row.FindControl("lbl_DeliveryPoint") != null)
                {
                    var lblDeliPoint = (Label)e.Row.FindControl("lbl_DeliveryPoint");
                    lblDeliPoint.Text = //DataBinder.Eval(e.Row.DataItem, "DeliPoint").ToString() + " : " + 
                        _deliPoint.GetName(DataBinder.Eval(e.Row.DataItem, "DeliPoint").ToString(), hf_ConnStr.Value);
                }

                if (e.Row.FindControl("lbl_Price") != null)
                {
                    var lblPrice = (Label)e.Row.FindControl("lbl_Price");
                    lblPrice.Text = String.Format("{0:n4}", DataBinder.Eval(e.Row.DataItem, "Price"));
                }

                if (e.Row.FindControl("lbl_DiscPercent") != null)
                {
                    var lblDiscPercent = (Label)e.Row.FindControl("lbl_DiscPercent");
                    lblDiscPercent.Text = DataBinder.Eval(e.Row.DataItem, "DiscPercent").ToString();
                }

                if (e.Row.FindControl("lbl_DiscAmt") != null)
                {
                    var lblDiscAmt = (Label)e.Row.FindControl("lbl_DiscAmt");
                    lblDiscAmt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "DiscAmt"));
                }

                if (e.Row.FindControl("lbl_TaxType") != null)
                {
                    var lblTaxType = (Label)e.Row.FindControl("lbl_TaxType");
                    lblTaxType.Text = DataBinder.Eval(e.Row.DataItem, "TaxType").ToString();
                }

                if (e.Row.FindControl("lbl_TaxRate") != null)
                {
                    var lblTaxRate = (Label)e.Row.FindControl("lbl_TaxRate");
                    lblTaxRate.Text = String.Format("{0:n}", DataBinder.Eval(e.Row.DataItem, "TaxRate"));
                }

                if (e.Row.FindControl("lbl_RefNo") != null)
                {
                    var lblRefNo = (Label)e.Row.FindControl("lbl_RefNo");
                    lblRefNo.Text = DataBinder.Eval(e.Row.DataItem, "RefNo").ToString();
                }

                if (e.Row.FindControl("lbl_ReqQty") != null)
                {
                    var lblReqQty = (Label)e.Row.FindControl("lbl_ReqQty");
                    lblReqQty.Text = DataBinder.Eval(e.Row.DataItem, "ReqQty").ToString();
                }

                if (e.Row.FindControl("lbl_PRNo") != null)
                {
                    var lblPRNo = (Label)e.Row.FindControl("lbl_PRNo");
                    lblPRNo.Text = DataBinder.Eval(e.Row.DataItem, "PRNo1").ToString();
                }

                if (e.Row.FindControl("lbl_PRDate") != null)
                {
                    var lblPRDate = (Label)e.Row.FindControl("lbl_PRDate");
                    lblPRDate.Text = String.Format("{0:d/M/yyyy HH:mm:ss}", DataBinder.Eval(e.Row.DataItem, "PRDate"));
                    //DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "PRDate").ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
                }

                // Comment ----------------------------------------------------------------------------
                if (e.Row.FindControl("lbl_Comment") != null)
                {
                    var lblComment = (Label)e.Row.FindControl("lbl_Comment");
                    lblComment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                }

                // Display Stock Summary --------------------------------------------------------------      
                if (e.Row.FindControl("uc_StockSummary") != null)
                {
                    var ucStockSummary = (StockSummary)e.Row.FindControl("uc_StockSummary");

                    ucStockSummary.ProductCode = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();
                    ucStockSummary.LocationCode = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString();
                    ucStockSummary.ConnStr = hf_ConnStr.Value;
                    ucStockSummary.DataBind();
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (e.Row.FindControl("lbl_TPrNet") != null)
                {
                    var lblTPrNet = (Label)e.Row.FindControl("lbl_TPrNet");
                    lblTPrNet.Text = String.Format(DefaultAmtFmt, _netamtpr);
                }

                if (e.Row.FindControl("lbl_TPrTax") != null)
                {
                    var lblTPrTax = (Label)e.Row.FindControl("lbl_TPrTax");
                    lblTPrTax.Text = String.Format(DefaultAmtFmt, _taxamtpr);
                }

                if (e.Row.FindControl("lbl_TPrAmount") != null)
                {
                    var lblTPrAmount = (Label)e.Row.FindControl("lbl_TPrAmount");
                    lblTPrAmount.Text = String.Format(DefaultAmtFmt, _totalamtpr);
                }
            }
        }

        protected void grd_PR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var strConnStr = _bu.GetConnectionString(DataBinder.Eval(e.Row.DataItem, "BUCode").ToString());

                if (e.Row.FindControl("lbl_BU") != null)
                {
                    var lblBu = (Label)e.Row.FindControl("lbl_BU");
                    lblBu.Text = DataBinder.Eval(e.Row.DataItem, "BUCode").ToString();
                }

                if (e.Row.FindControl("lbl_Store") != null)
                {
                    var lblStore = (Label)e.Row.FindControl("lbl_Store");
                    lblStore.Text = _storeLct.GetName(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(),
                        strConnStr);
                    //----02/03/2012----storeLct.GetName2(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(), strConnStr);
                }

                //*********************************************************
                if (e.Row.FindControl("hf_Store") != null)
                {
                    var hfStore = (HiddenField)e.Row.FindControl("hf_Store");
                    hfStore.Value = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString();
                }

                if (e.Row.FindControl("lbl_PRRef") != null)
                {
                    var lblPRRef = (Label)e.Row.FindControl("lbl_PRRef");
                    lblPRRef.Text = DataBinder.Eval(e.Row.DataItem, "PRNo1").ToString();
                }

                if (e.Row.FindControl("lbl_PRDate") != null)
                {
                    var lblPRDate = (Label)e.Row.FindControl("lbl_PRDate");
                    lblPRDate.Text = String.Format("{0:d/M/yyyy HH:mm:ss}", DataBinder.Eval(e.Row.DataItem, "PRDate"));
                    //DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "PRDate").ToString()).ToString();
                }

                if (e.Row.FindControl("lbl_QtyReq") != null)
                {
                    var lblQtyReq = (Label)e.Row.FindControl("lbl_QtyReq");
                    lblQtyReq.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "ReqQty")) + " " +
                                     DataBinder.Eval(e.Row.DataItem, "Unit");
                    //dtPrDt.Rows[0]["ReqQty"].ToString() + " " + dtPrDt.Rows[0]["Unit"].ToString();
                }

                if (e.Row.FindControl("lbl_PricePR") != null)
                {
                    var lblPricePR = (Label)e.Row.FindControl("lbl_PricePR");
                    lblPricePR.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "Price"));
                    //String.Format("{0:n}", decPrice); //String.Format("{0:n}", dtPrDt.Rows[0]["Price"]);
                }

                if (e.Row.FindControl("lbl_Approve") != null)
                {
                    var lblApprove = (Label)e.Row.FindControl("lbl_Approve");
                    lblApprove.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "ApprQty"));
                    //String.Format("{0:n}", dtPrDt.Rows[0]["ApprQty"]);
                }

                if (e.Row.FindControl("lbl_Buyer") != null)
                {
                    var lblBuyer = (Label)e.Row.FindControl("lbl_Buyer");
                    lblBuyer.Text = DataBinder.Eval(e.Row.DataItem, "Buyer").ToString();
                }
            }
        }

        protected void cmb_Unit_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cmbUnit = (ASPxComboBox)sender;
            var txtQtyOrder = (TextBox)grd_PoDt.Rows[grd_PoDt.EditIndex].FindControl("txt_QtyOrder");
            var lblQtyReq = (Label)grd_PoDt.Rows[grd_PoDt.EditIndex].FindControl("lbl_QtyReq");

            var productCode = _dsPo.Tables[_poDt.TableName].Rows[grd_PoDt.EditIndex]["Product"].ToString();
            var value = lblQtyReq.Text.Split(' ');

            var qty = _prodUnit.GetQtyAfterChangeUnit(productCode, cmbUnit.Value.ToString(), value[1],
                Convert.ToDecimal(value[0]), hf_ConnStr.Value);

            if (qty > 0)
            {
                txtQtyOrder.Text = qty.ToString();
            }
        }

        protected void cmb_Unit_Load(object sender, EventArgs e)
        {
            var cmbUnit = (ASPxComboBox)sender;
            var productCode =
                _dsPo.Tables[_poDt.TableName].Rows[((GridViewRow)cmbUnit.NamingContainer).DataItemIndex]["Product"]
                    .ToString();

            if (productCode != string.Empty)
            {
                cmbUnit.DataSource = _prodUnit.GetLookUp_ProductCode(productCode, hf_ConnStr.Value);
                cmbUnit.DataBind();
            }
        }

        // Check Period ********** By Praem
        protected void txt_PODate_TextChanged(object sender, EventArgs e)
        {
            for (var i = 0; i < grd_PoDt.Rows.Count; i++)
            {
                var grdPr = (GridView)grd_PoDt.Rows[i].FindControl("grd_PR");

                var hfStore = (HiddenField)grdPr.Rows[0].FindControl("hf_Store");
                var store = (Label)grdPr.Rows[0].FindControl("lbl_Store");


                //if (!_period.GetIsValidDate(DateTime.Parse(txt_PODate.Text), hfStore.Value, hf_ConnStr.Value))
                //{
                //    lbl_WarningPeriod.Text = string.Format("Store {0} is not period.", store.Text);
                //    pop_Warning.ShowOnPageLoad = true;
                //    return;
                //}
            }
        }

        protected void btn_WarningPeriod_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
            txt_PODate.Text = ServerDateTime.ToShortDateString();
        }

        #region cmb_Vendor

        protected void cmb_Vendor_OnItemsRequestedByFilterCondition_SQL(object source,
            ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            var comboBox = (ASPxComboBox)source;

            SqlDataSource1.ConnectionString = LoginInfo.ConnStr;

            SqlDataSource1.SelectCommand =
                @"SELECT
                  VendorCode
                 ,Name
                FROM
                  (SELECT
	                VendorCode
                   ,Name
                   ,ROW_NUMBER() OVER (ORDER BY [VendorCode]) AS [rn]
                   FROM
	                [AP].[Vendor]
                   WHERE
	                (VendorCode + ' ' + Name LIKE @filter)) AS st
                WHERE
                  st.[rn] BETWEEN @startIndex AND @endIndex";

            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            SqlDataSource1.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            SqlDataSource1.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = SqlDataSource1;
            comboBox.DataBind();
            comboBox.ToolTip = comboBox.Text;
        }

        protected void cmb_Vendor_OnItemRequestedByValue_SQL(object source,
            ListEditItemRequestedByValueEventArgs e)
        {
            var comboBox = (ASPxComboBox)source;

            try
            {
                if (e.Value == null)
                    return;

                SqlDataSource1.SelectCommand =
                    @"SELECT
                      VendorCode
                     ,Name
                    FROM
                      [AP].[Vendor]
                    WHERE
                      (VendorCode = @VendorCode)
                    ORDER BY
                      VendorCode";
                SqlDataSource1.ConnectionString = LoginInfo.ConnStr;
                SqlDataSource1.SelectParameters.Clear();
                SqlDataSource1.SelectParameters.Add("VendorCode", TypeCode.String, e.Value.ToString());
                comboBox.DataSource = SqlDataSource1;
                comboBox.DataBind();
            }
            catch (Exception ex)
            {
                comboBox.ToolTip = @"Found Exception.";
                LogManager.Error(ex);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmb_Vendor_Load(object sender, EventArgs e)
        {
            cmb_Vendor.DataSource = _dsPo.Tables[_vendor.TableName];
            cmb_Vendor.DataBind();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmb_Vendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_Vendor.Value.ToString() != string.Empty)
            {
                var creditTerm = _vendor.GetCreditTerm(cmb_Vendor.Value.ToString(), LoginInfo.ConnStr);

                txt_CreditTerm.Text = creditTerm.ToString();
                //sp_CreditTerm.Value    = creditTerm.ToString();
                //spe_ExchangeRate.Value  = "1";
            }
        }

        #endregion

        #region About Currency
        // Added on: 30/08/2017, By:Fon
        protected void txt_CurrRate_TextChanged(object sender, EventArgs e)
        {
            RateChanged(_dsPo.Tables[_poDt.TableName]);
            Total();
            lbl_TNet.Text = string.Format(DefaultAmtFmt, _netamt);
            lbl_TTax.Text = string.Format(DefaultAmtFmt, _taxamt);
            lbl_TAmount.Text = string.Format(DefaultAmtFmt, _totalamt);
            lbl_TDisc.Text = string.Format(DefaultAmtFmt, _discAmt);

            grd_PoDt.DataSource = _dsPo.Tables[_poDt.TableName];
            grd_PoDt.DataBind();
        }

        protected DataTable RateChanged(DataTable dtPoDt)
        {
            string taxType = string.Empty;
            decimal
                price = 0,
                ordQty = 0,
                taxRate = 0,
                currDiscAmt = 0,
                currRate = 0;
            decimal 
                netAmt = 0, 
                discAmt = 0, 
                taxAmt = 0, 
                totalAmt = 0;

            foreach (DataRow dr in dtPoDt.Rows)
            {
                // Get value
                decimal.TryParse(dr["Price"].ToString(), out price);
                decimal.TryParse(dr["OrdQty"].ToString(), out ordQty);
                decimal.TryParse(dr["CurrDiscAmt"].ToString(), out currDiscAmt);
                decimal.TryParse(dr["TaxRate"].ToString(), out taxRate);
                decimal.TryParse(txt_CurrRate.Text, out currRate);
                taxType = Convert.ToString(dr["TaxType"]);


                var currNetAmt = Convert.ToDecimal(dr["CurrNetAmt"]);
                var currTaxAmt = Convert.ToDecimal(dr["CurrTaxAmt"]);
                var currTotalAmt = Convert.ToDecimal(dr["CurrTotalAmt"]);


                discAmt = RoundAmt(currDiscAmt * currRate);
                //netAmt = NetAmt(taxType, taxRate, (price * currRate) * ordQty, discAmt, 1);
                //taxAmt = TaxAmt(taxType, taxRate, (price * currRate) * ordQty, discAmt, 1);
                //totalAmt = Amount(taxType, taxRate, (price * currRate) * ordQty, discAmt, 1);
                netAmt = RoundAmt(currNetAmt * currRate);
                taxAmt = RoundAmt(currTaxAmt * currRate);
                totalAmt = RoundAmt(currTotalAmt * currRate);

                dr["NetAmt"] = netAmt;
                dr["DisCountAmt"] = discAmt;
                dr["TaxAmt"] = taxAmt;
                dr["TotalAmt"] = totalAmt;
            }

            return dtPoDt;
        }

        #endregion

        #endregion


    }
}
