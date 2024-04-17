using System;
using System.Data;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;

namespace BlueLedger.PL.Option.Admin.Bu
{
    public partial class Bu : BasePage
    {
        #region "Attribute"

        private readonly Blue.BL.ADMIN.Bu bu = new Blue.BL.ADMIN.Bu();
        private readonly Blue.BL.Ref.City city = new Blue.BL.Ref.City();
        private readonly Blue.BL.Ref.Country country = new Blue.BL.Ref.Country();


        private DataSet dsBu = new DataSet();

        #endregion

        #region "Page Load"

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
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
            dsBu.Clear();
            var MsgError = string.Empty;

            var getBu = bu.GetBuList(dsBu, ref MsgError, LoginInfo.ConnStr);

            if (!getBu)
            {
                return;
            }
            Session["dsBu"] = dsBu;
            Page_Setting();
        }

        private void Page_Setting()
        {
            var drBu = dsBu.Tables[bu.TableName].Rows[0];
            var txtBuCode = (ASPxTextBox) ASPxNavBar.Groups[0].FindControl("txt_BuCode");
            var txtBuName = (ASPxTextBox) ASPxNavBar.Groups[0].FindControl("txt_BuName");
            var txtBuNameBilling = (ASPxTextBox) ASPxNavBar.Groups[0].FindControl("txt_BuNameBilling");
            var txtAddress = (ASPxTextBox) ASPxNavBar.Groups[1].FindControl("txt_Address");
            var txtPostCode = (ASPxTextBox) ASPxNavBar.Groups[1].FindControl("txt_PostCode");
            var ddlCountry = (ASPxComboBox) ASPxNavBar.Groups[1].FindControl("ddl_Country");
            var ddlCity = (ASPxComboBox) ASPxNavBar.Groups[1].FindControl("ddl_City");
            var txtPhone = (ASPxTextBox) ASPxNavBar.Groups[1].FindControl("txt_Phone");
            var txtFax = (ASPxTextBox) ASPxNavBar.Groups[1].FindControl("txt_Fax");
            var txtEmail = (ASPxTextBox) ASPxNavBar.Groups[1].FindControl("txt_Email");
            txtBuCode.Text = drBu["BuCode"].ToString();
            txtBuName.Text = drBu["Name"].ToString();
            txtBuNameBilling.Text = drBu["NameBilling"].ToString();
            txtAddress.Text = drBu["Address"].ToString();
            txtPostCode.Text = drBu["PostCode"].ToString();
            var dtTest = country.GetCountryList(LoginInfo.ConnStr);
            ddlCountry.DataSource = country.GetCountryList(LoginInfo.ConnStr);
            ddlCountry.DataBind();
            ddlCountry.Value = "TH; THAILAND";
            var Code = ddlCountry.Text.Split(';');
            ddlCity.DataSource = city.GetCityByCountryForDDL(Code[0], LoginInfo.ConnStr);
            ddlCity.DataBind();
            ddlCity.Value = "TH-10; Bangkok";
            txtPhone.Text = drBu["Phone"].ToString();
            txtFax.Text = drBu["Fax"].ToString();
            txtEmail.Text = drBu["Email"].ToString();
        }

        #endregion

        /// <summary>
        ///     Open customize format popup windows.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void bnt_Custz_Click(object sender, EventArgs e)
        {
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
            var ddlCountry = (ASPxComboBox) ASPxNavBar.Groups[1].FindControl("ddl_Country");
            var ddlCity = (ASPxComboBox) ASPxNavBar.Groups[1].FindControl("ddl_City");
            ddlCity.Value = "";
            var Code = ddlCountry.Text.Split(';');
            ddlCity.DataSource = city.GetCityByCountryForDDL(Code[0], LoginInfo.ConnStr);
            ddlCity.DataBind();
        }

        protected void ddl_City_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddlCountry = (ASPxComboBox) ASPxNavBar.Groups[1].FindControl("ddl_Country");
            ddlCountry.DataSource = country.GetCountryList(LoginInfo.ConnStr);
            ddlCountry.DataBind();
        }
    }
}