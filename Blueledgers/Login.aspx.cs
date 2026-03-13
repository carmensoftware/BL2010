using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;


public partial class Login : BasePage
{
    #region "Attributes"

    private readonly Blue.BL.dbo.Bu _bu = new Blue.BL.dbo.Bu();
    private readonly Blue.BL.dbo.BUUser _buUser = new Blue.BL.dbo.BUUser();
    private readonly Blue.BL.dbo.User _user = new Blue.BL.dbo.User();

    private readonly DataSet _dsUser = new DataSet();
    private readonly LoginInformation _loginInfo = new LoginInformation();

    
    #endregion

    #region "Operations"

    
    protected override void Page_PreInit(object sender, EventArgs e)
    {
    }

    
    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (!IsPostBack)
        {
            Session["LoginInfo"] = null;
        }
    }

    
    protected void LoginControl_Authenticate(object sender, AuthenticateEventArgs e)
    {
        // check login

        var username = LoginControl.UserName;
        var password = LoginControl.Password;
        var message = string.Empty;

        var validUser = _user.CheckLogin(_dsUser, username, password, ref message);

        if (validUser)
        {
            bool isPasswordExpired = dlgPassword.IsPasswordExpired(LoginControl.UserName);

            LoginControl.FailureText = string.Empty;

            if (isPasswordExpired)
            {
                dlgPassword.ChangePassword(LoginControl.UserName, "Password is expired. Please change password.");
                e.Authenticated = false;
            }
            else
            {
                if (message.ToString() != string.Empty)
                    Session["License"] = message;

                e.Authenticated = true;
            }
        }
        else
        {
            e.Authenticated = false;

            // Display error message
            if (message.Substring(0, 3).ToUpper() == "MSG")
            {
                if (message.ToUpper() == "MSG008") // Exceed active user license
                    Response.Redirect("~/UserActive.aspx");
                else
                    LoginControl.FailureText = Resources.MsgLogin.ResourceManager.GetString(message);
            }
            else
                LoginControl.FailureText = message;

            if (message.StartsWith("You have more users"))
            {

            }
        }
    }

    
    protected void LoginControl_LoggedIn(object sender, EventArgs e)
    {
        var drUser = _dsUser.Tables[_user.TableName].Rows[0];

        // Create user information            
        _loginInfo.LoginName = drUser["LoginName"].ToString();
        _loginInfo.FName = drUser["FName"].ToString();
        _loginInfo.MName = drUser["MName"].ToString();
        _loginInfo.LName = drUser["LName"].ToString();
        _loginInfo.Email = drUser["Email"].ToString();
        _loginInfo.EmailSign = drUser["EmailSign"].ToString();
        _loginInfo.DepartmentCode = drUser["DepartmentCode"].ToString();
        _loginInfo.DivisionCode = drUser["DivisionCode"].ToString();
        _loginInfo.SectionCode = drUser["SectionCode"].ToString();

        if (drUser["LastLogin"] != DBNull.Value)
        {
            _loginInfo.LastLogin = (DateTime)drUser["LastLogin"];
        }

        // Update LoginInfo
        UpdateLoginInfo();

        // Update last login
        _dsUser.Tables[_user.TableName].Rows[0]["LastLogin"] = ServerDateTime;
        _user.Save(_dsUser);

        // update latest selected display language to database.
        var dsTmp = new DataSet();
        var msgError = string.Empty;

        var result = _buUser.Get(dsTmp, _loginInfo.BuInfo.BuCode, _loginInfo.LoginName, ref msgError);

        if (result)
        {
            dsTmp.Tables[_buUser.TableName].Rows[0]["DispLang"] = _loginInfo.DispLang;
            _buUser.Save(dsTmp);
        }

        // Redirect to home page.
        var getUser = _user.Get(_dsUser, _loginInfo.LoginName);

        if (!getUser)
        {
            // Display Error Message
            return;
        }

        // Redirect to personal home page (if any) otherwise system default page. *************
        var drChkHomepage = _dsUser.Tables[_user.TableName].Rows[0];
        var homepage = drChkHomepage["HomePage"].ToString();

        if (Session["PreviousPage"] != null)
            Response.Redirect((string)Session["PreviousPage"]);
        else
            Response.Redirect(string.IsNullOrEmpty(homepage) ? "~/Option/User/Default.aspx" : homepage);
		
    }

    #endregion

    // Method(s)

    private void UpdateLoginInfo()
    {
        var msgError = string.Empty;

        // Assign default BU, Language, Theme and Formatting **********************************            
        var dsBuUser = new DataSet();
        DataRow drBuUser;

        // Get all accessible BU related to login name. 
        var getBU = _buUser.GetList(dsBuUser, _loginInfo.LoginName, ref msgError);

        if (!getBU)
        {
            // Display error message
            LoginControl.FailureText = Resources.MsgError.ResourceManager.GetString(msgError);
            return;
        }

        // If there is only one bu, then use as default, otherwise if there is the latest selected BU, then use as default else select from the first of the bu list.
        if (dsBuUser.Tables[_buUser.TableName].Rows.Count == 0)
        {
            return;
        }

        if (dsBuUser.Tables[_buUser.TableName].Rows.Count == 1)
        {
            // Assign default BU at row 0
            drBuUser = dsBuUser.Tables[_buUser.TableName].Rows[0];
        }
        else
        {
            if (Request.Cookies["SelectedBU"] != null)
            {
                // There is any selected BU. (not a first time login)
                drBuUser = dsBuUser.Tables[_buUser.TableName].Rows.Find(Request.Cookies["SelectedBU"].Value) ??
                           dsBuUser.Tables[_buUser.TableName].Rows[0];
            }
            else
            {
                // There is no selected BU.
                drBuUser = dsBuUser.Tables[_buUser.TableName].Rows[0];
            }
        }

        // Update selecedted BU coockies
        if (Request.Cookies["SelectedBU"] == null)
        {
            Response.Cookies.Add(new HttpCookie("SelectedBU"));
        }

        var cookie = Response.Cookies["SelectedBU"];
        if (cookie != null)
        {
            cookie.Value = drBuUser["BuCode"].ToString();
            cookie.Expires = DateTime.MaxValue;
        }
        // ************************************************************************************

        // update personal setting depend on selected business unit
        _loginInfo.Theme = (drBuUser["Theme"] != DBNull.Value ? drBuUser["Theme"].ToString() : "Default");
        _loginInfo.DispLang = (drBuUser["DispLang"] != DBNull.Value ? drBuUser["DispLang"].ToString() : "en-US");

        // update connection string. 
        _loginInfo.ConnStr = new StringBuilder().AppendFormat(
            //"Data Source={0};Initial Catalog={1};User ID={2};Password={3};packet size=4096;Min Pool Size=5; Max Pool Size=200;Connection Timeout=30;",
            "Data Source={0};Initial Catalog={1};User ID={2};Password={3};Connection Timeout=30;",
            drBuUser["ServerName"],
            drBuUser["DatabaseName"],
            drBuUser["UserName"],
            Blue.BL.GnxLib.EnDecryptString(drBuUser["Password"].ToString(), Blue.BL.GnxLib.EnDeCryptor.DeCrypt))
            .ToString();


        // update selected business unit information.
        _loginInfo.BuInfo.BuCode = drBuUser["BuCode"].ToString();
        _loginInfo.BuInfo.BuName = drBuUser["BuName"].ToString();
        _loginInfo.BuInfo.ServerName = drBuUser["ServerName"].ToString();
        _loginInfo.BuInfo.DBName = drBuUser["DatabaseName"].ToString();
        _loginInfo.BuInfo.UserName = drBuUser["UserName"].ToString();
        _loginInfo.BuInfo.Password = drBuUser["Password"].ToString();
        _loginInfo.BuInfo.BuGrpCode = drBuUser["BuGrpCode"].ToString();
        _loginInfo.BuInfo.IsHQ = bool.Parse(drBuUser["IsHQ"].ToString());


        // update business unit format setting.
        _loginInfo.BuFmtInfo.BuCode = _loginInfo.BuInfo.BuCode;
        _loginInfo.BuFmtInfo.Theme = drBuUser["Theme"].ToString();
        _loginInfo.BuFmtInfo.FmtCode = drBuUser["FmtCode"].ToString();
        _loginInfo.BuFmtInfo.FmtSDate = drBuUser["FmtSDate"].ToString();
        _loginInfo.BuFmtInfo.FmtLDate = drBuUser["FmtLDate"].ToString();
        _loginInfo.BuFmtInfo.FmtSTime = drBuUser["FmtSTime"].ToString();
        _loginInfo.BuFmtInfo.FmtLTime = drBuUser["FmtLTime"].ToString();
        _loginInfo.BuFmtInfo.FmtAM = drBuUser["FmtAM"].ToString();
        _loginInfo.BuFmtInfo.FmtPM = drBuUser["FmtPM"].ToString();
        _loginInfo.BuFmtInfo.FmtFirstDOW = drBuUser["FmtFirstDOW"].ToString();
        _loginInfo.BuFmtInfo.FmtNumDec = drBuUser["FmtNumDec"].ToString();
        _loginInfo.BuFmtInfo.FmtNumDecNo = (drBuUser["FmtNumDecNo"] != DBNull.Value
            ? int.Parse(drBuUser["FmtNumDecNo"].ToString())
            : 0);
        _loginInfo.BuFmtInfo.FmtNumDecGrp = drBuUser["FmtNumDecGrp"].ToString();
        _loginInfo.BuFmtInfo.FmtNumNeg = drBuUser["FmtNumNeg"].ToString();
        _loginInfo.BuFmtInfo.FmtCurrency = drBuUser["FmtCurrency"].ToString();
        _loginInfo.BuFmtInfo.FmtCurrencyDec = drBuUser["FmtCurrencyDec"].ToString();
        _loginInfo.BuFmtInfo.FmtCurrencyDecNo = (drBuUser["FmtCurrencyDecNo"] != DBNull.Value
            ? int.Parse(drBuUser["FmtCurrencyDecNo"].ToString())
            : 0);
        _loginInfo.BuFmtInfo.FmtCurrencyDecGrp = drBuUser["FmtCurrencyDecGrp"].ToString();
        _loginInfo.BuFmtInfo.CountryCode = drBuUser["CountryCode"].ToString();
        _loginInfo.BuFmtInfo.UTCCode = drBuUser["UTCCode"].ToString();
        _loginInfo.BuFmtInfo.LangCode = drBuUser["LangCode"].ToString();
        _loginInfo.BuFmtInfo.LangCodeOth = drBuUser["LangCodeOth"].ToString();
        _loginInfo.BuFmtInfo.IsDefaultLangCode = (_loginInfo.DispLang.ToUpper() == _loginInfo.BuFmtInfo.LangCode.ToUpper());
        _loginInfo.BuFmtInfo.LocalNumericFormat = "#,0.00";

        // Update HQ ConnectionString
        var drHQ = _bu.GetHQConnectionString(drBuUser["BuGrpCode"].ToString()).Rows[0];
        _loginInfo.HQConnStr = "Data Source=" + drHQ["ServerName"] + "; " +
                               "Initial Catalog = " + drHQ["DatabaseName"] + "; " +
                               "User ID = " + drHQ["UserName"] + "; " +
                               "Password = " +
                               Blue.BL.GnxLib.EnDecryptString(drHQ["Password"].ToString(),
                                   Blue.BL.GnxLib.EnDeCryptor.DeCrypt);

        Session["LoginInfo"] = _loginInfo;

    }
    
    //private void LoginPassThrough()
    //{
    //    var paramsLogin = Blue.BL.GnxLib.EnDecryptString(Request.Params["ID"], Blue.BL.GnxLib.EnDeCryptor.DeCrypt);

    //    var urlParams = paramsLogin.Split('&');
    //    var bu = urlParams[0].Split('=');
    //    var username = urlParams[1].Split('=');
    //    var password = urlParams[2].Split('=');

    //    var xParam = Blue.BL.GnxLib.EnDecryptString(Request.Params["ID"], Blue.BL.GnxLib.EnDeCryptor.DeCrypt, Blue.BL.GnxLib.KEY_LOGIN_PASSWORD);

    //    var xID = xParam.Split('&');
    //    password = xID[2].Split('=');


    //    // Assign UserName and Password
    //    _UserName = username[1];
    //    _Password = password[1];

    //     Fire Authenticate event

    //    LoginControl_Authenticate(this, new AuthenticateEventArgs());
    //    LoginControl_LoggedIn(this, new EventArgs());

    //    Response.Redirect("BuList.aspx?BuCode=" + bu[1]);
    //}

    



}