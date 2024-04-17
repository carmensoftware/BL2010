using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

public partial class BuList : BasePage
{
    #region "Attributes"

    private readonly Blue.BL.dbo.Bu _bu = new Blue.BL.dbo.Bu();
    private readonly Blue.BL.dbo.BUUser _buUser = new Blue.BL.dbo.BUUser();
    private readonly DataSet _dsUser = new DataSet();
    private readonly Blue.BL.dbo.Lang _lang = new Blue.BL.dbo.Lang();
    private readonly Blue.BL.dbo.User _user = new Blue.BL.dbo.User();
    private DataSet _dsBuUser = new DataSet();
    //private Blue.BL.dbo.BuGroup _buGroup = new Blue.BL.dbo.BuGroup();


    /// <summary>
    ///     Gets or set business unit related to user.
    /// </summary>
    private DataSet DsBuUser
    {
        get
        {
            if (ViewState["dsBuUser"] != null)
            {
                _dsBuUser = (DataSet)ViewState["dsBuUser"];
            }
            return _dsBuUser;
        }
        set
        {
            _dsBuUser = value;
            ViewState["dsBuUser"] = _dsBuUser;
        }
    }

    #endregion

    #region "Operations"

    /// <summary>
    ///     Override page pre-initial to cancel all page default setting command.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected override void Page_PreInit(object sender, EventArgs e)
    {
        // Do nothing to override page setting command.            
    }

    /// <summary>
    ///     Page load event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected override void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            imgbtnBack.Attributes.Add("onmouseover", "this.src= '" + this.Page.ResolveClientUrl("~/App_Themes/Default/Images/master/icon/gray-back.png") + "'");
            imgbtnBack.Attributes.Add("onmouseout", "this.src= '" + this.Page.ResolveClientUrl("~/App_Themes/Default/Images/master/icon/back.png") + "'");
            Page_Retrieve();

            // If Session["SelectedBu"] is not null, automatic select BU
            if (Request.Params["BuCode"] != null)
            {
                // Login Pass-Through
                LoginPassThrough();
            }
        }
    }

    /// <summary>
    ///     Get business unit data related to login user.
    /// </summary>
    private void Page_Retrieve()
    {
        var dsTmp = new DataSet();
        var msgError = string.Empty;

        var getBU = _buUser.GetList(dsTmp, LoginInfo.LoginName, ref msgError);

        if (getBU)
        {
            DsBuUser = dsTmp;
        }
        else
        {
            // Display error message
            lbl_MsgError.Text = Resources.MsgError.ResourceManager.GetString(msgError);
            return;
        }

        var getUser = _user.Get(_dsUser, LoginInfo.LoginName);

        if (!getUser)
        {
            // Display Error Message
            return;
        }
        Session["dsUser"] = _dsUser;

        Page_Setting();
    }

    /// <summary>
    ///     Display business unit data which retrieved from Page_Retrieve procedure.
    /// </summary>
    private void Page_Setting()
    {

        DataView dv = DsBuUser.Tables[0].DefaultView;
        dv.Sort = "IsHQ DESC";
        DataTable sortedDT = dv.ToTable();

        //grd_BU.DataSource = DsBuUser;
        grd_BU.DataSource = sortedDT;
        grd_BU.DataBind();
    }

    /// <summary>
    ///     Display business unit data in grid view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grd_BU_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var lb = (LinkButton)e.Row.Cells[1].Controls[0];
            lb.Text = DataBinder.Eval(e.Row.DataItem, "BUName").ToString();

            // Display language
            if (e.Row.FindControl("ddl_Lang") != null)
            {
                var ddlLang = (DropDownList)e.Row.FindControl("ddl_Lang");
                var dtLang = _lang.GetList(DataBinder.Eval(e.Row.DataItem, "BUCode").ToString());
                if (dtLang != null)
                {
                    ddlLang.DataSource = dtLang;
                    ddlLang.DataTextField = "Name";
                    ddlLang.DataValueField = "LangCode";
                    ddlLang.DataBind();
                    ddlLang.SelectedValue = DataBinder.Eval(e.Row.DataItem, "DispLang").ToString();
                }
            }
        }
    }

    /// <summary>
    ///     Update login information's connection string and redirec to home page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grd_BU_SelectedIndexChanged(object sender, EventArgs e)
    {
        var drBuUser = DsBuUser.Tables[_buUser.TableName].Rows[grd_BU.SelectedIndex];

        // Update LoginInfo
        drBuUser["DispLang"] = ((DropDownList)((GridView)sender).SelectedRow.FindControl("ddl_Lang")).SelectedItem.Value;
        // Update selected language
        UpdateLoginInfo(drBuUser);

        // update latest selected display language to database.
        var dsTmp = new DataSet();
        var msgError = string.Empty;

        var result = _buUser.Get(dsTmp, LoginInfo.BuInfo.BuCode, LoginInfo.LoginName, ref msgError);

        if (result)
        {
            dsTmp.Tables[_buUser.TableName].Rows[0]["DispLang"] = LoginInfo.DispLang;
            _buUser.Save(dsTmp);
        }

        // Redirect to home page.

        var getUser = _user.Get(_dsUser, LoginInfo.LoginName);

        if (!getUser)
        {
            // Display Error Message
            return;
        }
        Session["dsUser"] = _dsUser;

        // Redirect to personal home page (if any) otherwise system default page. *************
        var drChkHomepage = _dsUser.Tables[_user.TableName].Rows[0];
        var homepage = drChkHomepage["HomePage"].ToString();

        Response.Redirect(string.IsNullOrEmpty(homepage) ? "~/Option/User/Default.aspx" : homepage);
        // ************************************************************************************
    }

    private void UpdateLoginInfo(DataRow buInfo)
    {
        // Update selecedted BU coockies
        if (Request.Cookies["SelectedBU"] == null)
        {
            Response.Cookies.Add(new HttpCookie("SelectedBU"));
        }

        var cookie = Response.Cookies["SelectedBU"];
        if (cookie != null)
        {
            cookie.Value = buInfo["BuCode"].ToString();
            cookie.Expires = DateTime.MaxValue;
        }

        // update personal setting depend on selected business unit
        LoginInfo.Theme = (buInfo["Theme"] != DBNull.Value ? buInfo["Theme"].ToString() : "Default");
        LoginInfo.DispLang = (buInfo["DispLang"] != DBNull.Value ? buInfo["DispLang"].ToString() : "en-US");

        // update connection string.
        LoginInfo.ConnStr = "Data Source=" + buInfo["ServerName"] + "; " +
                            "Initial Catalog = " + buInfo["DatabaseName"] + "; " +
                            "User ID = " + buInfo["UserName"] + "; " +
                            "Password = " +
                            Blue.BL.GnxLib.EnDecryptString(buInfo["Password"].ToString(),
                                Blue.BL.GnxLib.EnDeCryptor.DeCrypt);

        var drHQ = _bu.GetHQConnectionString(buInfo["BuGrpCode"].ToString()).Rows[0];

        LoginInfo.HQConnStr = "Data Source=" + drHQ["ServerName"] + "; " +
                              "Initial Catalog = " + drHQ["DatabaseName"] + "; " +
                              "User ID = " + drHQ["UserName"] + "; " +
                              "Password = " +
                              Blue.BL.GnxLib.EnDecryptString(drHQ["Password"].ToString(),
                                  Blue.BL.GnxLib.EnDeCryptor.DeCrypt);

        // update selected business unit information.
        LoginInfo.BuInfo.BuCode = buInfo["BuCode"].ToString();
        LoginInfo.BuInfo.BuName = buInfo["BuName"].ToString();
        //LoginInfo.BuInfo.BUCalcType = (CalcCostType.CalcType)Enum.Parse(typeof(CalcCostType.CalcType), BuInfo["CalcType"].ToString()  );
        LoginInfo.BuInfo.ServerName = buInfo["ServerName"].ToString();
        LoginInfo.BuInfo.DBName = buInfo["DatabaseName"].ToString();
        LoginInfo.BuInfo.UserName = buInfo["UserName"].ToString();
        LoginInfo.BuInfo.Password = buInfo["Password"].ToString();
        LoginInfo.BuInfo.BuGrpCode = buInfo["BuGrpCode"].ToString();
        LoginInfo.BuInfo.IsHQ = bool.Parse(buInfo["IsHQ"].ToString());
        LoginInfo.BuInfo.HQBuCode = _bu.GetHQBuCode(LoginInfo.BuInfo.BuGrpCode);

        // update business unit format setting.
        LoginInfo.BuFmtInfo.BuCode = LoginInfo.BuInfo.BuCode;
        LoginInfo.BuFmtInfo.Theme = buInfo["Theme"].ToString();
        LoginInfo.BuFmtInfo.FmtCode = buInfo["FmtCode"].ToString();
        LoginInfo.BuFmtInfo.FmtSDate = buInfo["FmtSDate"].ToString();
        LoginInfo.BuFmtInfo.FmtLDate = buInfo["FmtLDate"].ToString();
        LoginInfo.BuFmtInfo.FmtSTime = buInfo["FmtSTime"].ToString();
        LoginInfo.BuFmtInfo.FmtLTime = buInfo["FmtLTime"].ToString();
        LoginInfo.BuFmtInfo.FmtAM = buInfo["FmtAM"].ToString();
        LoginInfo.BuFmtInfo.FmtPM = buInfo["FmtPM"].ToString();
        LoginInfo.BuFmtInfo.FmtFirstDOW = buInfo["FmtFirstDOW"].ToString();
        LoginInfo.BuFmtInfo.FmtNumDec = buInfo["FmtNumDec"].ToString();
        LoginInfo.BuFmtInfo.FmtNumDecNo = (buInfo["FmtNumDecNo"] != DBNull.Value
            ? int.Parse(buInfo["FmtNumDecNo"].ToString())
            : 0);
        LoginInfo.BuFmtInfo.FmtNumDecGrp = buInfo["FmtNumDecGrp"].ToString();
        LoginInfo.BuFmtInfo.FmtNumNeg = buInfo["FmtNumNeg"].ToString();
        LoginInfo.BuFmtInfo.FmtCurrency = buInfo["FmtCurrency"].ToString();
        LoginInfo.BuFmtInfo.FmtCurrencyDec = buInfo["FmtCurrencyDec"].ToString();
        LoginInfo.BuFmtInfo.FmtCurrencyDecNo = (buInfo["FmtCurrencyDecNo"] != DBNull.Value
            ? int.Parse(buInfo["FmtCurrencyDecNo"].ToString())
            : 0);
        LoginInfo.BuFmtInfo.FmtCurrencyDecGrp = buInfo["FmtCurrencyDecGrp"].ToString();
        LoginInfo.BuFmtInfo.CountryCode = buInfo["CountryCode"].ToString();
        LoginInfo.BuFmtInfo.UTCCode = buInfo["UTCCode"].ToString();
        LoginInfo.BuFmtInfo.LangCode = buInfo["LangCode"].ToString();
        LoginInfo.BuFmtInfo.LangCodeOth = buInfo["LangCodeOth"].ToString();
        LoginInfo.BuFmtInfo.IsDefaultLangCode = (LoginInfo.DispLang.ToUpper() ==
                                                 LoginInfo.BuFmtInfo.LangCode.ToUpper());
    }

    #endregion

    #region "Redirect to Inbox Page."

    /// <summary>
    ///     Check bu from session and redirect to inbox
    /// </summary>
    private void LoginPassThrough()
    {
        foreach (DataRow drBuUser in DsBuUser.Tables[_buUser.TableName].Rows)
        {
            if (drBuUser["BuCode"].ToString().ToUpper() == Request.Params["BuCode"].ToUpper())
            {
                UpdateLoginInfo(drBuUser);
                break;
            }
        }

        Response.Redirect("~/Option/User/Default.aspx");
    }


    protected void imgbtnBack_Click(object sender, EventArgs e)
    {
        var backUrl = Session["BackUrl"].ToString();
        Response.Redirect(backUrl);
    }

    #endregion
}