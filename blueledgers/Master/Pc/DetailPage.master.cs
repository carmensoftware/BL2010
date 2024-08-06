using System;
using System.Data;
using System.Web.UI.WebControls;

namespace BlueLedger.PL.Master.Pc
{
    public partial class MasterDetailPage : System.Web.UI.MasterPage
    {
        /// <summary>
        ///     Gets current login user information.
        /// </summary>
        protected BaseClass.LoginInformation LoginInfo
        {
            get { return (BaseClass.LoginInformation)Session["LoginInfo"]; }
        }

        #region "Attributes"

        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.dbo.BUUser buUser = new Blue.BL.dbo.BUUser();

        private readonly DataSet dsBuUser = new DataSet();
        private readonly DataSet dsLanguage = new DataSet();
        private readonly DataSet dsUser = new DataSet();
        private readonly Blue.BL.dbo.Lang language = new Blue.BL.dbo.Lang();
        private readonly Blue.BL.dbo.User user = new Blue.BL.dbo.User();
        private string MsgError = string.Empty;
        private Blue.BL.dbo.BuFmt buFmt = new Blue.BL.dbo.BuFmt();
        private Blue.BL.APP.Module modlue = new Blue.BL.APP.Module();
        private Blue.BL.APP.TransType transType = new Blue.BL.APP.TransType();

        #endregion

        #region "Page Event"

        protected void Page_Load(object sender, EventArgs e)
        {
            // Display Login Information
            lnk_UserName.Text = LoginInfo.LoginName;
            lnk_BuName.Text = LoginInfo.BuInfo.BuName;

            // Display Language
            lbl_Language.Text = (LoginInfo.DispLang.Split('-'))[0].ToUpper();
            lbl_Language.ToolTip = LoginInfo.DispLang;
        }

        #endregion

        # region "Change Business Unit"

        protected void grd_BU_Load(object sender, EventArgs e)
        {
            var getBU = buUser.GetList(dsBuUser, LoginInfo.LoginName, ref MsgError);

            if (getBU)
            {
                grd_BU.DataSource = dsBuUser.Tables[buUser.TableName];
                grd_BU.DataBind();
            }
        }

        protected void grd_BU_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var lnkb_BU = e.Row.FindControl("lnkb_BU") as LinkButton;
                lnkb_BU.Text = System.Web.UI.DataBinder.Eval(e.Row.DataItem, "BuName").ToString();
            }
        }

        protected void lnkb_BU_Click(object sender, EventArgs e)
        {
            // Get Selectd BUCode
            var lnkb_BU = sender as LinkButton;
            var grdRow = lnkb_BU.Parent.Parent as GridViewRow;
            var BuInfo = dsBuUser.Tables[buUser.TableName].Rows[grdRow.DataItemIndex];

            // Update selecedted BU coockies
            if (Request.Cookies["SelectedBU"] == null)
            {
                Response.Cookies.Add(new System.Web.HttpCookie("SelectedBU"));
            }

            Response.Cookies["SelectedBU"].Value = BuInfo["BuCode"].ToString();
            Response.Cookies["SelectedBU"].Expires = DateTime.MaxValue;

            // update connection string.
            LoginInfo.ConnStr = bu.GetConnectionString(BuInfo["BuCode"].ToString());

            // update selected business unit information.
            LoginInfo.BuInfo.BuCode = BuInfo["BuCode"].ToString();
            LoginInfo.BuInfo.BuName = BuInfo["BuName"].ToString();
            LoginInfo.BuInfo.ServerName = BuInfo["ServerName"].ToString();
            LoginInfo.BuInfo.DBName = BuInfo["DatabaseName"].ToString();
            LoginInfo.BuInfo.UserName = BuInfo["UserName"].ToString();
            LoginInfo.BuInfo.Password = BuInfo["Password"].ToString();
            LoginInfo.BuInfo.BuGrpCode = BuInfo["BuGrpCode"].ToString();
            LoginInfo.BuInfo.IsHQ = bool.Parse(BuInfo["IsHQ"].ToString());

            // update business unit format setting.
            LoginInfo.BuFmtInfo.BuCode = LoginInfo.BuInfo.BuCode;
            LoginInfo.BuFmtInfo.Theme = BuInfo["Theme"].ToString();
            LoginInfo.BuFmtInfo.FmtCode = BuInfo["FmtCode"].ToString();
            LoginInfo.BuFmtInfo.FmtSDate = BuInfo["FmtSDate"].ToString();
            LoginInfo.BuFmtInfo.FmtLDate = BuInfo["FmtLDate"].ToString();
            LoginInfo.BuFmtInfo.FmtSTime = BuInfo["FmtSTime"].ToString();
            LoginInfo.BuFmtInfo.FmtLTime = BuInfo["FmtLTime"].ToString();
            LoginInfo.BuFmtInfo.FmtAM = BuInfo["FmtAM"].ToString();
            LoginInfo.BuFmtInfo.FmtPM = BuInfo["FmtPM"].ToString();
            LoginInfo.BuFmtInfo.FmtFirstDOW = BuInfo["FmtFirstDOW"].ToString();
            LoginInfo.BuFmtInfo.FmtNumDec = BuInfo["FmtNumDec"].ToString();
            LoginInfo.BuFmtInfo.FmtNumDecNo = (BuInfo["FmtNumDecNo"] != DBNull.Value
                ? int.Parse(BuInfo["FmtNumDecNo"].ToString())
                : 0);
            LoginInfo.BuFmtInfo.FmtNumDecGrp = BuInfo["FmtNumDecGrp"].ToString();
            LoginInfo.BuFmtInfo.FmtNumNeg = BuInfo["FmtNumNeg"].ToString();
            LoginInfo.BuFmtInfo.FmtCurrency = BuInfo["FmtCurrency"].ToString();
            LoginInfo.BuFmtInfo.FmtCurrencyDec = BuInfo["FmtCurrencyDec"].ToString();
            LoginInfo.BuFmtInfo.FmtCurrencyDecNo = (BuInfo["FmtCurrencyDecNo"] != DBNull.Value
                ? int.Parse(BuInfo["FmtCurrencyDecNo"].ToString())
                : 0);
            LoginInfo.BuFmtInfo.FmtCurrencyDecGrp = BuInfo["FmtCurrencyDecGrp"].ToString();
            LoginInfo.BuFmtInfo.CountryCode = BuInfo["CountryCode"].ToString();
            LoginInfo.BuFmtInfo.UTCCode = BuInfo["UTCCode"].ToString();
            LoginInfo.BuFmtInfo.LangCode = BuInfo["LangCode"].ToString();
            LoginInfo.BuFmtInfo.LangCodeOth = BuInfo["LangCodeOth"].ToString();
            LoginInfo.BuFmtInfo.IsDefaultLangCode = (LoginInfo.DispLang.ToUpper() ==
                                                     LoginInfo.BuFmtInfo.LangCode.ToUpper()
                ? true
                : false);

            var drHQ = bu.GetHQConnectionString(BuInfo["BuGrpCode"].ToString()).Rows[0];
            LoginInfo.HQConnStr = "Data Source=" + drHQ["ServerName"] + "; " +
                                  "Initial Catalog = " + drHQ["DatabaseName"] + "; " +
                                  "User ID = " + drHQ["UserName"] + "; " +
                                  "Password = " +
                                  Blue.BL.GnxLib.EnDecryptString(drHQ["Password"].ToString(),
                                      Blue.BL.GnxLib.EnDeCryptor.DeCrypt);

            // Redirect to personal home page (if any) otherwise system default page. *************
            var getUser = user.Get(dsUser, LoginInfo.LoginName);

            var drChkHomepage = dsUser.Tables[user.TableName].Rows[0];
            var homepage = drChkHomepage["HomePage"].ToString();

            if (string.IsNullOrEmpty(homepage))
            {
                Response.Redirect("~/Option/User/Default.aspx");
            }
            else
            {
                Response.Redirect(homepage);
            }
        }

        #endregion

        #region "Change Language"

        protected void grd_Language_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var lnkb_Language = e.Row.FindControl("lnkb_Language") as LinkButton;
                lnkb_Language.Text = System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Name").ToString();
            }
        }

        protected void grd_Language_Load(object sender, EventArgs e)
        {
            var getLanguage = language.GetList(dsLanguage);

            if (getLanguage)
            {
                grd_Language.DataSource = dsLanguage.Tables[language.TableName];
                grd_Language.DataBind();
            }
        }

        protected void lnkb_Language_Click(object sender, EventArgs e)
        {
            var dsTmp = new DataSet();

            // Get Selectd Language
            var lnkb_Language = sender as LinkButton;
            var grdRow = lnkb_Language.Parent.Parent as GridViewRow;
            var drLanguage = dsLanguage.Tables[language.TableName].Rows[grdRow.DataItemIndex];

            // Update Selectd Language
            LoginInfo.DispLang = drLanguage["LangCode"].ToString();

            var result = buUser.Get(dsTmp, LoginInfo.BuInfo.BuCode, LoginInfo.LoginName, ref MsgError);

            if (result)
            {
                dsTmp.Tables[buUser.TableName].Rows[0]["DispLang"] = drLanguage["LangCode"].ToString();
                buUser.Save(dsTmp);
            }

            // Refresh Page
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        #endregion

        #region "Options"

        protected void imb_Home_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            //Response.Redirect("~/Home/Default.aspx");
        }

        protected void lnkb_Help_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Help/Default.aspx");
        }

        protected void lnkb_Logout_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }

        #endregion
    }
}