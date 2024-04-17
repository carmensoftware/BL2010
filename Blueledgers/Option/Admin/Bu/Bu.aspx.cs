using System;
using System.Data;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxTabControl;

namespace BlueLedger.PL.Option.Admin.Bu
{
    public partial class Bu : BasePage
    {
        #region "Attribute"

        //BL.ADMIN.Bu bu = new BL.ADMIN.Bu();
        //BL.Ref.Country country = new BlueLedger.BL.Ref.Country();
        //BL.Ref.City city = new BlueLedger.BL.Ref.City();
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.dbo.BuFmt buFmt = new Blue.BL.dbo.BuFmt();
        private readonly Blue.BL.dbo.BuInfo buInfo = new Blue.BL.dbo.BuInfo();
        private readonly Blue.BL.dbo.City city = new Blue.BL.dbo.City();
        private readonly Blue.BL.dbo.Country country = new Blue.BL.dbo.Country();
        private readonly Blue.BL.dbo.Lang lang = new Blue.BL.dbo.Lang();
        private readonly Blue.BL.dbo.UTC utc = new Blue.BL.dbo.UTC();

        private DataSet dsBu = new DataSet();
        //private string BuCode   = string.Empty;      

        private string BuCode
        {
            get { return Session["BuCode"].ToString(); }
            set { Session["BuCode"] = value; }
        }

        #endregion

        #region "Page Load"

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                BuCode = LoginInfo.BuInfo.BuCode;
                Page_Retrieve();
            }
            else
            {
                dsBu = (DataSet) Session["dsBu"];
            }
        }

        private void Page_Retrieve()
        {
            // Retrieve all about BU data.

            bu.Get(dsBu, BuCode);
            buInfo.Get(dsBu, BuCode);
            buFmt.Get(dsBu, BuCode);

            Session["dsBu"] = dsBu;
            Page_Setting();
        }

        private void Page_Setting()
        {
            if (dsBu.Tables[buInfo.TableName].Rows.Count <= 0)
            {
                var dr = dsBu.Tables[buInfo.TableName].NewRow();
                dsBu.Tables[buInfo.TableName].Rows.Add(dr);
            }
            var drBu = dsBu.Tables[buInfo.TableName].Rows[0];
            var txt_BuCode = (ASPxTextBox) ASPxNavBar.Groups[0].FindControl("txt_BuCode");
            var txt_BuName = (ASPxTextBox) ASPxNavBar.Groups[0].FindControl("txt_BuName");
            var txt_BuNameBilling = (ASPxTextBox) ASPxNavBar.Groups[0].FindControl("txt_BuNameBilling");
            var ddl_CalcType = (ASPxComboBox) ASPxNavBar.Groups[0].FindControl("ddl_CalcType");
            var txt_Address = (ASPxTextBox) ASPxNavBar.Groups[1].FindControl("txt_Address");
            var txt_PostCode = (ASPxTextBox) ASPxNavBar.Groups[1].FindControl("txt_PostCode");
            var ddl_Country = (ASPxComboBox) ASPxNavBar.Groups[1].FindControl("ddl_Country");
            var ddl_City = (ASPxComboBox) ASPxNavBar.Groups[1].FindControl("ddl_City");
            var txt_Phone = (ASPxTextBox) ASPxNavBar.Groups[1].FindControl("txt_Phone");
            var txt_Fax = (ASPxTextBox) ASPxNavBar.Groups[1].FindControl("txt_Fax");
            var txt_Email = (ASPxTextBox) ASPxNavBar.Groups[1].FindControl("txt_Email");
            var txt_DeliveryTerm = (ASPxMemo) ASPxNavBar.Groups[2].FindControl("txt_DeliveryTerm");
            var pc_Regional = (ASPxPageControl) ASPxNavBar.Groups[3].FindControl("pc_Regional");
            var txt_Number = (ASPxTextBox) pc_Regional.FindControl("txt_Number");
            var txt_Currency = (ASPxTextBox) pc_Regional.FindControl("txt_Currency");
            var txt_Time = (ASPxTextBox) pc_Regional.FindControl("txt_Time");
            var txt_SDate = (ASPxTextBox) pc_Regional.FindControl("txt_SDate");
            var txt_LDate = (ASPxTextBox) pc_Regional.FindControl("txt_LDate");
            var ddl_DefaultLang = (ASPxComboBox) pc_Regional.FindControl("ddl_DefaultLang");
            var ddl_OptionalLang = (ASPxComboBox) pc_Regional.FindControl("ddl_OptionalLang");

            txt_BuCode.Text = drBu["BuCode"].ToString();
            txt_BuName.Text = dsBu.Tables[bu.TableName].Rows[0]["BuName"].ToString();
            txt_BuNameBilling.Text = drBu["BillingName"].ToString();
            txt_Address.Text = drBu["Street"].ToString();
            txt_PostCode.Text = drBu["PostCode"].ToString();
            txt_Phone.Text = drBu["Phone"].ToString();
            txt_Fax.Text = drBu["Fax"].ToString();
            txt_Email.Text = drBu["Email"].ToString();
            txt_DeliveryTerm.Text = drBu["DeliveryTerms"].ToString();

            ddl_Country.DataSource = country.GetList();
            ddl_Country.DataBind();
            ddl_Country.ValueField = "CountryCode";
            ddl_Country.Value = drBu["Country"];

            if (ddl_Country.Value != null)
            {
                ddl_City.DataSource = city.GetList(ddl_Country.Value.ToString());
                ddl_City.DataBind();
                ddl_City.Value = drBu["City"];
                ddl_City.ValueField = "Code";
            }

            // Add CalcType to calculate Average or FIFO cost
            //var _dtCalcType = new DataTable();
            //_dtCalcType.TableName = "dtCalcType";
            //_dtCalcType.Columns.Add("CalcType", typeof(Int32));
            //_dtCalcType.Columns.Add("CalcTypeName", typeof(string));
            //_dtCalcType.Rows.Add(0, "Average");
            //_dtCalcType.Rows.Add(1, "FIFO");

            //ddl_CalcType.DataSource = _dtCalcType; 
            //ddl_CalcType.DataBind();
            ddl_CalcType.Value = drBu["CalcType"] ?? 0;
            ddl_CalcType.ValueField = "CalcType";

            //decimal Num                     = 1000;
            //decimal Currency                = 1000;
            //txt_Number.Text                 = Num.ToString(LoginInfo.BuFmtInfo.FmtNumDec);
            //txt_Currency.Text               = Currency.ToString(LoginInfo.BuFmtInfo.FmtCurrency);
            txt_SDate.Text = ServerDateTime.ToString(LoginInfo.BuFmtInfo.FmtSDate);
            txt_LDate.Text = ServerDateTime.ToString(LoginInfo.BuFmtInfo.FmtLDate);
            txt_Time.Text = ServerDateTime.ToString(LoginInfo.BuFmtInfo.FmtSTime);

            var drBuFmt = dsBu.Tables[buFmt.TableName].Rows[0];
            ddl_FmtNumDecNo.Value = drBuFmt["FmtNumDecNo"];
            txt_FmtNumDec.Text = drBuFmt["FmtNumDec"].ToString();
            txt_FmtNumDecGrp.Text = drBuFmt["FmtNumDecGrp"].ToString();
            txt_FmtNumNeg.Text = drBuFmt["FmtNumNeg"].ToString();
            txt_FmtCurrency.Text = drBuFmt["FmtCurrency"].ToString();
            txt_FmtCurrencyDec.Text = drBuFmt["FmtCurrencyDec"].ToString();
            txt_FmtCurrencyDecNo.Value = drBuFmt["FmtCurrencyDecNo"];
            txt_FmtCurrencyDecgrp.Text = drBuFmt["FmtCurrencyDecGrp"].ToString();

            ShowCurrencySample();
            ShowNumSample();

            ddl_DefaultLang.DataSource = lang.GetList(BuCode);
            ddl_DefaultLang.DataBind();
            ddl_DefaultLang.Value = dsBu.Tables[buFmt.TableName].Rows[0]["LangCode"];
            ddl_DefaultLang.ValueField = "LangCode";

            ddl_OptionalLang.DataSource = lang.GetList(BuCode);
            ddl_OptionalLang.DataBind();
            ddl_OptionalLang.Value = dsBu.Tables[buFmt.TableName].Rows[0]["LangCodeOth"];
            ddl_OptionalLang.ValueField = "LangCode";
        }

        #endregion

        /// <summary>
        ///     Open customize format popup windows.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void bnt_Custz_Click(object sender, EventArgs e)
        {
            var drBuFmt = dsBu.Tables[buFmt.TableName].Rows[0];
            txt_FmtNumDec.Text = drBuFmt["FmtNumDec"].ToString();
            txt_FmtNumDecGrp.Text = drBuFmt["FmtNumDecGrp"].ToString();
            txt_FmtNumNeg.Text = drBuFmt["FmtNumNeg"].ToString();
            ddl_FmtNumDecNo.Value = drBuFmt["FmtNumDecNo"];
            txt_FmtCurrency.Text = drBuFmt["FmtCurrency"].ToString();
            txt_FmtCurrencyDec.Text = drBuFmt["FmtCurrencyDec"].ToString();
            txt_FmtCurrencyDecNo.Value = drBuFmt["FmtCurrencyDecNo"];
            txt_FmtCurrencyDecgrp.Text = drBuFmt["FmtCurrencyDecGrp"].ToString();
            txt_FmtSDate.Text = drBuFmt["FmtSDate"].ToString();
            txt_FmtLDate.Text = drBuFmt["FmtLDate"].ToString();
            txt_FmtSTime.Text = drBuFmt["FmtSTime"].ToString();

            ddl_UTCCode.DataSource = utc.GetList();
            ddl_UTCCode.DataBind();
            ddl_UTCCode.ValueField = "UTCCode";
            ddl_UTCCode.Value = drBuFmt["UTCCode"];

            // Open popup
            pop_FormatCustz.ShowOnPageLoad = true;
        }

        /// <summary>
        ///     Update customized format to dataset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_PopOK_Click(object sender, EventArgs e)
        {
            // Update to dataset
            var drBuFmt = dsBu.Tables[buFmt.TableName].Rows[0];
            drBuFmt["FmtNumDec"] = txt_FmtNumDec.Text == string.Empty ? null : txt_FmtNumDec.Text;
            drBuFmt["FmtNumDecGrp"] = txt_FmtNumDecGrp.Text == string.Empty ? null : txt_FmtNumDecGrp.Text;
            drBuFmt["FmtNumNeg"] = txt_FmtNumNeg.Text == string.Empty ? null : txt_FmtNumNeg.Text;
            drBuFmt["FmtNumDecNo"] = ddl_FmtNumDecNo.Value == null ? DBNull.Value : ddl_FmtNumDecNo.Value;
            drBuFmt["FmtCurrency"] = txt_FmtCurrency.Text == string.Empty ? null : txt_FmtCurrency.Text;
            drBuFmt["FmtCurrencyDec"] = txt_FmtCurrencyDec.Text == string.Empty ? null : txt_FmtCurrencyDec.Text;
            drBuFmt["FmtCurrencyDecNo"] = txt_FmtCurrencyDecNo.Value == null ? DBNull.Value : txt_FmtCurrencyDecNo.Value;
            drBuFmt["FmtCurrencyDecGrp"] = txt_FmtCurrencyDecgrp.Text == string.Empty
                ? null
                : txt_FmtCurrencyDecgrp.Text;
            drBuFmt["FmtSDate"] = txt_FmtSDate.Text == string.Empty ? null : txt_FmtSDate.Text;
            drBuFmt["FmtLDate"] = txt_FmtLDate.Text == string.Empty ? null : txt_FmtLDate.Text;
            drBuFmt["FmtSTime"] = txt_FmtSTime.Text == string.Empty ? null : txt_FmtSTime.Text;
            drBuFmt["UTCCode"] = ddl_UTCCode.Value == null ? DBNull.Value : ddl_UTCCode.Value;

            var pc_Regional = (ASPxPageControl) ASPxNavBar.Groups[3].FindControl("pc_Regional");
            var txt_Time = (ASPxTextBox) pc_Regional.FindControl("txt_Time");
            var txt_SDate = (ASPxTextBox) pc_Regional.FindControl("txt_SDate");
            var txt_LDate = (ASPxTextBox) pc_Regional.FindControl("txt_LDate");

            txt_Time.Text = ServerDateTime.ToString(drBuFmt["FmtSTime"].ToString());
            txt_SDate.Text = ServerDateTime.ToString(drBuFmt["FmtSDate"].ToString());
            txt_LDate.Text = ServerDateTime.ToString(drBuFmt["FmtLDate"].ToString());

            // Close popup
            pop_FormatCustz.ShowOnPageLoad = false;
        }

        /// <summary>
        ///     Cancel customizing format.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_PopCancel_Click(object sender, EventArgs e)
        {
            // Close popup
            pop_FormatCustz.ShowOnPageLoad = false;
        }

        protected void ddl_Country_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl_Country = (ASPxComboBox) ASPxNavBar.Groups[1].FindControl("ddl_Country");
            var ddl_City = (ASPxComboBox) ASPxNavBar.Groups[1].FindControl("ddl_City");

            ddl_City.DataSource = city.GetList(ddl_Country.Value.ToString());
            ddl_City.DataBind();
            ddl_City.ValueField = "Code";
        }

        protected void ddl_DefaultLang_OnLoad(object sender, EventArgs e)
        {
            var pc_Regional = (ASPxPageControl) ASPxNavBar.Groups[3].FindControl("pc_Regional");
            var ddl_DefaultLang = (ASPxComboBox) pc_Regional.FindControl("ddl_DefaultLang");

            ddl_DefaultLang.DataSource = lang.GetList(BuCode);
            ddl_DefaultLang.DataBind();
            ddl_DefaultLang.ValueField = "LangCode";
        }

        protected void ddl_OptionalLang_OnLoad(object sender, EventArgs e)
        {
            var pc_Regional = (ASPxPageControl) ASPxNavBar.Groups[3].FindControl("pc_Regional");
            var ddl_OptionalLang = (ASPxComboBox) pc_Regional.FindControl("ddl_OptionalLang");

            ddl_OptionalLang.DataSource = lang.GetList(BuCode);
            ddl_OptionalLang.DataBind();
            ddl_OptionalLang.ValueField = "LangCode";
        }

        protected void ddl_Country_OnLoad(object sender, EventArgs e)
        {
            var ddl_Country = (ASPxComboBox) ASPxNavBar.Groups[1].FindControl("ddl_Country");
            ddl_Country.DataSource = country.GetList();
            ddl_Country.DataBind();
            ddl_Country.ValueField = "CountryCode";
        }

        protected void ddl_UTCCode_Load(object sender, EventArgs e)
        {
            ddl_UTCCode.DataSource = utc.GetList();
            ddl_UTCCode.DataBind();
            ddl_UTCCode.ValueField = "UTCCode";
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            var txt_BuCode = (ASPxTextBox) ASPxNavBar.Groups[0].FindControl("txt_BuCode");
            var txt_BuName = (ASPxTextBox) ASPxNavBar.Groups[0].FindControl("txt_BuName");
            var txt_BuNameBilling = (ASPxTextBox) ASPxNavBar.Groups[0].FindControl("txt_BuNameBilling");
            var ddl_CalcType = (ASPxComboBox) ASPxNavBar.Groups[0].FindControl("ddl_CalcType");
            var txt_Address = (ASPxTextBox) ASPxNavBar.Groups[1].FindControl("txt_Address");
            var txt_PostCode = (ASPxTextBox) ASPxNavBar.Groups[1].FindControl("txt_PostCode");
            var ddl_Country = (ASPxComboBox) ASPxNavBar.Groups[1].FindControl("ddl_Country");
            var ddl_City = (ASPxComboBox) ASPxNavBar.Groups[1].FindControl("ddl_City");
            var txt_Phone = (ASPxTextBox) ASPxNavBar.Groups[1].FindControl("txt_Phone");
            var txt_Fax = (ASPxTextBox) ASPxNavBar.Groups[1].FindControl("txt_Fax");
            var txt_Email = (ASPxTextBox) ASPxNavBar.Groups[1].FindControl("txt_Email");
            var txt_DeliveryTerm = (ASPxMemo) ASPxNavBar.Groups[2].FindControl("txt_DeliveryTerm");
            var pc_Regional = (ASPxPageControl) ASPxNavBar.Groups[3].FindControl("pc_Regional");
            var txt_Number = (ASPxTextBox) pc_Regional.FindControl("txt_Number");
            var txt_Currency = (ASPxTextBox) pc_Regional.FindControl("txt_Currency");
            var txt_Time = (ASPxTextBox) pc_Regional.FindControl("txt_Time");
            var txt_SDate = (ASPxTextBox) pc_Regional.FindControl("txt_SDate");
            var txt_LDate = (ASPxTextBox) pc_Regional.FindControl("txt_LDate");
            var ddl_DefaultLang = (ASPxComboBox) pc_Regional.FindControl("ddl_DefaultLang");
            var ddl_OptionalLang = (ASPxComboBox) pc_Regional.FindControl("ddl_OptionalLang");

            //Update table Bu
            var drBuSave = dsBu.Tables[bu.TableName].Rows[0];
            drBuSave["BuName"] = txt_BuName.Text;

            //Update table BuInfo
            var drBuInfoSave = dsBu.Tables[buInfo.TableName].Rows[0];
            drBuInfoSave["BillingName"] = txt_BuNameBilling.Text;
            drBuInfoSave["Street"] = txt_Address.Text;
            drBuInfoSave["PostCode"] = txt_PostCode.Text;
            drBuInfoSave["Country"] = ddl_Country.Value;
            drBuInfoSave["CalcType"] = Convert.ToInt32(ddl_CalcType.Value);
            drBuInfoSave["City"] = ddl_City.Value;
            drBuInfoSave["Phone"] = txt_Phone.Text;
            drBuInfoSave["Fax"] = txt_Fax.Text;
            drBuInfoSave["Email"] = txt_Email.Text;
            drBuInfoSave["DeliveryTerms"] = txt_DeliveryTerm.Text;

            //Update table BuFmt
            var drBuFmtSave = dsBu.Tables[buFmt.TableName].Rows[0];
            drBuFmtSave["CountryCode"] = ddl_Country.Value;
            drBuFmtSave["LangCode"] = ddl_DefaultLang.Value;
            drBuFmtSave["LangCodeOth"] = ddl_OptionalLang.Value;

            var save = buInfo.Save(dsBu);

            if (save)
            {
                lbl_Warning.Text = "Save successful.";
                pop_Warning.ShowOnPageLoad = true;
            }
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            dsBu.Clear();
            Page_Retrieve();
        }

        private void ShowNumSample()
        {
            var pc_Regional = (ASPxPageControl) ASPxNavBar.Groups[3].FindControl("pc_Regional");
            var txt_Number = (ASPxTextBox) pc_Regional.FindControl("txt_Number");

            var Num = Convert.ToDecimal("1234.1234567890");

            if (ddl_FmtNumDecNo.Value != null)
            {
                switch (ddl_FmtNumDecNo.Value.ToString())
                {
                    case "1":
                        txt_NumPos.Text =
                            String.Format("{0:#" + txt_FmtNumDecGrp.Text + "###" + txt_FmtNumDec.Text + "#}", Num);
                        txt_NumNeg.Text =
                            String.Format(
                                "{0:" + txt_FmtNumNeg.Text + "#" + txt_FmtNumDecGrp.Text + "###" + txt_FmtNumDec.Text +
                                "#}", Num);
                        txt_Number.Text = txt_NumPos.Text;
                        break;

                    case "2":
                        txt_NumPos.Text =
                            String.Format("{0:#" + txt_FmtNumDecGrp.Text + "###" + txt_FmtNumDec.Text + "##}", Num);
                        txt_NumNeg.Text =
                            String.Format(
                                "{0:" + txt_FmtNumNeg.Text + "#" + txt_FmtNumDecGrp.Text + "###" + txt_FmtNumDec.Text +
                                "##}", Num);
                        txt_Number.Text = txt_NumPos.Text;
                        break;

                    case "3":
                        txt_NumPos.Text =
                            String.Format("{0:#" + txt_FmtNumDecGrp.Text + "###" + txt_FmtNumDec.Text + "###}", Num);
                        txt_NumNeg.Text =
                            String.Format(
                                "{0:" + txt_FmtNumNeg.Text + "#" + txt_FmtNumDecGrp.Text + "###" + txt_FmtNumDec.Text +
                                "###}", Num);
                        txt_Number.Text = txt_NumPos.Text;
                        break;

                    case "4":
                        txt_NumPos.Text =
                            String.Format("{0:#" + txt_FmtNumDecGrp.Text + "###" + txt_FmtNumDec.Text + "####}", Num);
                        txt_NumNeg.Text =
                            String.Format(
                                "{0:" + txt_FmtNumNeg.Text + "#" + txt_FmtNumDecGrp.Text + "###" + txt_FmtNumDec.Text +
                                "####}", Num);
                        txt_Number.Text = txt_NumPos.Text;
                        break;

                    case "5":
                        txt_NumPos.Text =
                            String.Format("{0:#" + txt_FmtNumDecGrp.Text + "###" + txt_FmtNumDec.Text + "#####}", Num);
                        txt_NumNeg.Text =
                            String.Format(
                                "{0:" + txt_FmtNumNeg.Text + "#" + txt_FmtNumDecGrp.Text + "###" + txt_FmtNumDec.Text +
                                "#####}", Num);
                        txt_Number.Text = txt_NumPos.Text;
                        break;

                    case "6":
                        txt_NumPos.Text =
                            String.Format("{0:#" + txt_FmtNumDecGrp.Text + "###" + txt_FmtNumDec.Text + "######}", Num);
                        txt_NumNeg.Text =
                            String.Format(
                                "{0:" + txt_FmtNumNeg.Text + "#" + txt_FmtNumDecGrp.Text + "###" + txt_FmtNumDec.Text +
                                "######}", Num);
                        txt_Number.Text = txt_NumPos.Text;
                        break;

                    case "7":
                        txt_NumPos.Text =
                            String.Format("{0:#" + txt_FmtNumDecGrp.Text + "###" + txt_FmtNumDec.Text + "#######}", Num);
                        txt_NumNeg.Text =
                            String.Format(
                                "{0:" + txt_FmtNumNeg.Text + "#" + txt_FmtNumDecGrp.Text + "###" + txt_FmtNumDec.Text +
                                "#######}", Num);
                        txt_Number.Text = txt_NumPos.Text;
                        break;

                    case "8":
                        txt_NumPos.Text =
                            String.Format("{0:#" + txt_FmtNumDecGrp.Text + "###" + txt_FmtNumDec.Text + "########}", Num);
                        txt_NumNeg.Text =
                            String.Format(
                                "{0:" + txt_FmtNumNeg.Text + "#" + txt_FmtNumDecGrp.Text + "###" + txt_FmtNumDec.Text +
                                "########}", Num);
                        txt_Number.Text = txt_NumPos.Text;
                        break;

                    case "9":
                        txt_NumPos.Text =
                            String.Format("{0:#" + txt_FmtNumDecGrp.Text + "###" + txt_FmtNumDec.Text + "#########}",
                                Num);
                        txt_NumNeg.Text =
                            String.Format(
                                "{0:" + txt_FmtNumNeg.Text + "#" + txt_FmtNumDecGrp.Text + "###" + txt_FmtNumDec.Text +
                                "#########}", Num);
                        txt_Number.Text = txt_NumPos.Text;
                        break;

                    default:
                        txt_NumPos.Text = String.Format("{0:#" + txt_FmtNumDecGrp.Text + "###}", Num);
                        txt_NumNeg.Text =
                            String.Format("{0:" + txt_FmtNumNeg.Text + "#" + txt_FmtNumDecGrp.Text + "###}", Num);
                        txt_Number.Text = txt_NumPos.Text;
                        break;
                }
            }
        }

        private void ShowCurrencySample()
        {
            var pc_Regional = (ASPxPageControl) ASPxNavBar.Groups[3].FindControl("pc_Regional");
            var txt_Currency = (ASPxTextBox) pc_Regional.FindControl("txt_Currency");

            var Num = Convert.ToDecimal("1234.1234567890");

            if (txt_FmtCurrencyDecNo.Value != null)
            {
                switch (txt_FmtCurrencyDecNo.Value.ToString())
                {
                    case "1":
                        txt_CurPos.Text =
                            String.Format(
                                "{0:" + txt_FmtCurrency.Text + "#" + txt_FmtCurrencyDecgrp.Text + "###" +
                                txt_FmtCurrencyDec.Text + "#}", Num);
                        //txt_CurNeg.Text     = String.Format("{0:" + drBuFmt["FmtCurrency"].ToString() + "#" + drBuFmt["FmtCurrencyDecGrp"].ToString() + "###" + drBuFmt["FmtCurrencyDec"].ToString() + "#}", Num);
                        txt_Currency.Text = txt_CurPos.Text;
                        break;

                    case "2":
                        txt_CurPos.Text =
                            String.Format(
                                "{0:" + txt_FmtCurrency.Text + "#" + txt_FmtCurrencyDecgrp.Text + "###" +
                                txt_FmtCurrencyDec.Text + "##}", Num);
                        //txt_CurNeg.Text     = String.Format("{0:" + drBuFmt["FmtCurrency"].ToString() + "#" + drBuFmt["FmtCurrencyDecGrp"].ToString() + "###" + drBuFmt["FmtCurrencyDec"].ToString() + "##}", Num);
                        txt_Currency.Text = txt_CurPos.Text;
                        break;

                    case "3":
                        txt_CurPos.Text =
                            String.Format(
                                "{0:" + txt_FmtCurrency.Text + "#" + txt_FmtCurrencyDecgrp.Text + "###" +
                                txt_FmtCurrencyDec.Text + "###}", Num);
                        //txt_CurNeg.Text     = String.Format("{0:" + drBuFmt["FmtCurrency"].ToString() + "#" + drBuFmt["FmtCurrencyDecGrp"].ToString() + "###" + drBuFmt["FmtCurrencyDec"].ToString() + "###}", Num);
                        txt_Currency.Text = txt_CurPos.Text;
                        break;

                    case "4":
                        txt_CurPos.Text =
                            String.Format(
                                "{0:" + txt_FmtCurrency.Text + "#" + txt_FmtCurrencyDecgrp.Text + "###" +
                                txt_FmtCurrencyDec.Text + "####}", Num);
                        //txt_CurNeg.Text     = String.Format("{0:" + drBuFmt["FmtCurrency"].ToString() + "#" + drBuFmt["FmtCurrencyDecGrp"].ToString() + "###" + drBuFmt["FmtCurrencyDec"].ToString() + "####}", Num);
                        txt_Currency.Text = txt_CurPos.Text;
                        break;

                    case "5":
                        txt_CurPos.Text =
                            String.Format(
                                "{0:" + txt_FmtCurrency.Text + "#" + txt_FmtCurrencyDecgrp.Text + "###" +
                                txt_FmtCurrencyDec.Text + "#####}", Num);
                        //txt_CurNeg.Text     = String.Format("{0:" + drBuFmt["FmtCurrency"].ToString() + "#" + drBuFmt["FmtCurrencyDecGrp"].ToString() + "###" + drBuFmt["FmtCurrencyDec"].ToString() + "#####}", Num);
                        txt_Currency.Text = txt_CurPos.Text;
                        break;

                    case "6":
                        txt_CurPos.Text =
                            String.Format(
                                "{0:" + txt_FmtCurrency.Text + "#" + txt_FmtCurrencyDecgrp.Text + "###" +
                                txt_FmtCurrencyDec.Text + "######}", Num);
                        //txt_CurNeg.Text     = String.Format("{0:" + drBuFmt["FmtCurrency"].ToString() + "#" + drBuFmt["FmtCurrencyDecGrp"].ToString() + "###" + drBuFmt["FmtCurrencyDec"].ToString() + "######}", Num);
                        txt_Currency.Text = txt_CurPos.Text;
                        break;

                    case "7":
                        txt_CurPos.Text =
                            String.Format(
                                "{0:" + txt_FmtCurrency.Text + "#" + txt_FmtCurrencyDecgrp.Text + "###" +
                                txt_FmtCurrencyDec.Text + "#######}", Num);
                        //txt_CurNeg.Text     = String.Format("{0:" + drBuFmt["FmtCurrency"].ToString() + "#" + drBuFmt["FmtCurrencyDecGrp"].ToString() + "###" + drBuFmt["FmtCurrencyDec"].ToString() + "#######}", Num);
                        txt_Currency.Text = txt_CurPos.Text;
                        break;

                    case "8":
                        txt_CurPos.Text =
                            String.Format(
                                "{0:" + txt_FmtCurrency.Text + "#" + txt_FmtCurrencyDecgrp.Text + "###" +
                                txt_FmtCurrencyDec.Text + "########}", Num);
                        //txt_CurNeg.Text     = String.Format("{0:" + drBuFmt["FmtCurrency"].ToString() + "#" + drBuFmt["FmtCurrencyDecGrp"].ToString() + "###" + drBuFmt["FmtCurrencyDec"].ToString() + "########}", Num);
                        txt_Currency.Text = txt_CurPos.Text;
                        break;

                    case "9":
                        txt_CurPos.Text =
                            String.Format(
                                "{0:" + txt_FmtCurrency.Text + "#" + txt_FmtCurrencyDecgrp.Text + "###" +
                                txt_FmtCurrencyDec.Text + "#########}", Num);
                        //txt_CurNeg.Text     = String.Format("{0:" + drBuFmt["FmtCurrency"].ToString() + "#" + drBuFmt["FmtCurrencyDecGrp"].ToString() + "###" + drBuFmt["FmtCurrencyDec"].ToString() + "#########}", Num);
                        txt_Currency.Text = txt_CurPos.Text;
                        break;

                    default:
                        txt_CurPos.Text =
                            String.Format("{0:" + txt_FmtCurrency.Text + "#" + txt_FmtCurrencyDecgrp.Text + "#}", Num);
                        //txt_CurNeg.Text     = String.Format("{0:" + drBuFmt["FmtCurrency"].ToString() + "#" + drBuFmt["FmtCurrencyDecGrp"].ToString() + "0}", Num);
                        txt_Currency.Text = txt_CurPos.Text;
                        break;
                }
            }
        }

        protected void txt_FmtNumDec_TextChanged(object sender, EventArgs e)
        {
            ShowNumSample();
        }

        protected void ddl_FmtNumDecNo_ValueChanged(object sender, EventArgs e)
        {
            ShowNumSample();
        }

        protected void txt_FmtNumDecGrp_TextChanged(object sender, EventArgs e)
        {
            ShowNumSample();
        }

        protected void txt_FmtNumNeg_TextChanged(object sender, EventArgs e)
        {
            ShowNumSample();
        }

        protected void txt_FmtCurrency_TextChanged(object sender, EventArgs e)
        {
            ShowCurrencySample();
        }

        protected void txt_FmtCurrencyDec_TextChanged(object sender, EventArgs e)
        {
            ShowCurrencySample();
        }

        protected void txt_FmtCurrencyDecNo_ValueChanged(object sender, EventArgs e)
        {
            ShowCurrencySample();
        }

        protected void txt_FmtCurrencyDecgrp_TextChanged(object sender, EventArgs e)
        {
            ShowCurrencySample();
        }

        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
        }
    }
}