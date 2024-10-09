using System;
using System.Globalization;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using DevExpress.Web.ASPxGridView;

namespace BlueLedger.PL.BaseClass
{
    public class BasePage : Page
    {
        #region Static Method

        /// <summary>
        ///     Use for set default value to zero if not found.
        /// </summary>
        /// <param name="value">string or null</param>
        /// <returns></returns>
        public static string StringOrZero(string value)
        {
            return string.IsNullOrEmpty(value) ? "0.00" : value;
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StringOrZero(object value)
        {
            return value == null || value == DBNull.Value ? "0.00" : StringOrZero(value.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal DecimalOrZero(object value)
        {
            return Convert.ToDecimal(StringOrZero(value));
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal DecimalOrZero(string value)
        {
            return Convert.ToDecimal(StringOrZero(value));
        }

        public static decimal RoundNumber(decimal value, int digits)
        {
            return Math.Round(value, digits, MidpointRounding.AwayFromZero);
        }

        public static double RoundNumber(double value, int digits)
        {
            return Math.Round(value, digits, MidpointRounding.AwayFromZero);
        }


        #endregion

        #region "Attributes"

        private readonly Blue.BL.ProjectAdmin.SysParameter _sysParameter = new Blue.BL.ProjectAdmin.SysParameter();
        private readonly Blue.BL.APP.Config _config = new Blue.BL.APP.Config();
        private int _DigitAmt = 2;
        private int _DigitQty = 3;

        /// <summary>
        ///     Gets current login user information.
        /// </summary>
        protected LoginInformation LoginInfo
        {
            get { return (LoginInformation)Session["LoginInfo"]; }
        }

        public int DefaultAmtDigit
        {
            get { return _DigitAmt; }
        }

        public int DefaultQtyDigit
        {
            get { return _DigitQty; }
        }

        public string DefaultAmtFmt
        {
            get { return "{0:N" + _DigitAmt.ToString() + "}"; }
        }

        public string DefaultQtyFmt
        {
            get { return "{0:N" + _DigitQty.ToString() + "}"; }
        }


        /// <summary>
        ///     Gets current date time base on server datetime and current business regional setting.
        /// </summary>
        protected DateTime ServerDateTime
        {
            get { return Blue.BL.GnxLib.GetSysDate(string.Empty); }
        }

        /// <summary>
        ///     Project name.
        /// </summary>
        protected string AppName
        {
            get { return _sysParameter.GetValue("System", "AppName", LoginInfo.ConnStr); }
        }

        /// <summary>
        ///     DateTime format depend on culture or manual configuration.
        /// </summary>
        protected string DateTimeFormat
        {
            get
            {
                var culture = new CultureInfo(_sysParameter.GetValue("System", "Culture", LoginInfo.ConnStr));
                var dateTimePattern = _sysParameter.GetValue("System", "DateTimePattern", LoginInfo.ConnStr);

                return (dateTimePattern != string.Empty ? dateTimePattern : culture.DateTimeFormat.ShortDatePattern);
            }
        }

        /// <summary>Get numeric format string of BU</summary>
        protected string BuNumericFormat(object value)
        {
            if (value != null)
                //return String.Format("{0:"+LoginInfo.BuFmtInfo.LocalNumericFormat + "}", Convert.ToDecimal(value));
                return Convert.ToDecimal(value).ToString(LoginInfo.BuFmtInfo.LocalNumericFormat);
            else
                return "0.00";

        }

        /// <summary>
        ///     Get the global application culture object.
        /// </summary>
        protected IFormatProvider AppCulture
        {
            get
            {
                IFormatProvider appCulture =
                    new CultureInfo(_sysParameter.GetValue("System", "Culture", LoginInfo.ConnStr), true);

                return appCulture;
            }
        }

        /// <summary>
        ///     Internal datetime format depend on culture or manual configuration.
        /// </summary>
        protected string InternalDateTimeFormat
        {
            get
            {
                var culture = new CultureInfo(_sysParameter.GetValue("System", "Culture", LoginInfo.ConnStr));
                var internalDateTimePattern = _sysParameter.GetValue("System", "InternalDateTimePattern",
                    LoginInfo.ConnStr);

                return (internalDateTimePattern != string.Empty
                    ? internalDateTimePattern
                    : culture.DateTimeFormat.ShortDatePattern);
            }
        }

        #endregion

        #region "Operations"

        ///// <summary>
        /////     Status of datarow to determine command type. (update, insert or delete)
        ///// </summary>
        //public enum RowState
        //{
        //    Unchanged,
        //    Modified,
        //    Added,
        //    Deleted
        //}


        /// <summary>
        ///     Assign page defaul setting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Page_PreInit(object sender, EventArgs e)
        {
            // assign page theme.
            if (LoginInfo != null)
            {
                Page.Theme = LoginInfo.Theme;

                // assign page culture.            
                Page.Culture = LoginInfo.DispLang;
                Page.UICulture = LoginInfo.DispLang;

                var appCulture = Thread.CurrentThread.CurrentCulture.Clone() as CultureInfo;

                if (appCulture != null)
                {
                    appCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                    appCulture.DateTimeFormat.DateSeparator = "/";
                    appCulture.DateTimeFormat.ShortTimePattern = "hh:mm";
                    appCulture.DateTimeFormat.LongTimePattern = "hh:mm:ss";
                    Thread.CurrentThread.CurrentCulture = appCulture;
                }
                Session["PreviousPage"] = null;
            }
            else
            {
                Session["PreviousPage"] = HttpContext.Current.Request.Url.AbsoluteUri;
                Response.Redirect("~/ErrorPages/SessionTimeOut.aspx");
            }

            // This is necessary because Safari and Chrome browsers don't display the Menu control correctly.
            // All webpages displaying an ASP.NET menu control must inherit this class.
            if (Request.ServerVariables["http_user_agent"].IndexOf("Safari", StringComparison.CurrentCultureIgnoreCase) != -1)
            {
                Page.ClientTarget = "uplevel";
            }


            //Blue.BL.APP.Config _config = new Blue.BL.APP.Config();

            //string digits = _config.GetValue("APP", "Default", "DigitQty", LoginInfo.ConnStr);
            //if (string.IsNullOrEmpty(digits))
            //{
            //    _DigitQty = 3;
            //    _config.SetConfigValue("APP", "Default", "DigitQty", "3", LoginInfo.ConnStr);
            //}
            //else
            //    _DigitQty = Convert.ToInt32(digits);


            //digits = _config.GetValue("APP", "Default", "DigitAmt", LoginInfo.ConnStr);
            //if (string.IsNullOrEmpty(digits))
            //{
            //    _DigitAmt = 2;
            //    _config.SetConfigValue("APP", "Default", "DigitAmt", "2", LoginInfo.ConnStr);
            //}
            //else
            //    _DigitAmt = Convert.ToInt32(digits);

            SetDefault();
        }


        public void SetDefault()
        {
            string digits = _config.GetValue("APP", "Default", "DigitQty", LoginInfo.ConnStr);
            if (string.IsNullOrEmpty(digits))
            {
                _DigitQty = 3;
                // _config.SetConfigValue("APP", "Default", "DigitQty", "3", LoginInfo.ConnStr);
            }
            else
                _DigitQty = Convert.ToInt32(digits);


            digits = _config.GetValue("APP", "Default", "DigitAmt", LoginInfo.ConnStr);
            if (string.IsNullOrEmpty(digits))
            {
                _DigitAmt = 2;
                // _config.SetConfigValue("APP", "Default", "DigitAmt", "2", LoginInfo.ConnStr);
            }
            else
                _DigitAmt = Convert.ToInt32(digits);

        }

        /// <summary>
        ///     Register javascript reference at start page.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            //var script = string.Format(" <link type=\"text/css\" rel=\"stylesheet\" href=\"{0}App_Themes/Default/theme.css\" />" +
            //                           " <script type=\"text/javascript\" src=\"{0}Scripts/jquery-1.9.1.js\"></script>" +
            //                           " <script type=\"text/javascript\" src=\"{0}Scripts/jquery-ui-1.8.21.custom.min.js\"></script>" +
            //                           " <link type=\"text/css\" rel=\"stylesheet\" href=\"{0}Scripts//jquery-ui-1.8.21.custom.css\" />" +
            //                           " <script type=\"text/javascript\" src=\"{0}Scripts/GnxLib.js\"></script>",
            //    ResolveUrl("~"));

            //if (!ClientScript.IsClientScriptBlockRegistered("Startup"))
            //{
            //    ClientScript.RegisterClientScriptBlock(GetType(), "Startup", script);
            //}

            base.OnPreRender(e);
        }

        /// <summary>
        ///     Page load event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            bool isEnabledTrace;

            if (!Trace.IsEnabled && bool.TryParse(ConfigurationManager.AppSettings["OpenTraceOnPage"], out isEnabledTrace))
                Trace.IsEnabled = isEnabledTrace;

            AddKeepAlive();

        }


        private void AddKeepAlive()
        {
            //const int intMilliSecondsTimeOut = 2 * 60 * 1000;
            ////Math.Max((this.Session.Timeout * 60000) - 30000, 5000);
            //var path = VirtualPathUtility.ToAbsolute("~/KeepAlive.aspx");
            //var strScript = string.Format(
            //               @"<script>(function(){{var r=0,w=window;if (w.setInterval)w.setInterval(function() {{r++;var img=new Image(1,1);img.src='{0}?count='+r;}},{1});}})();</script>",
            //               path, intMilliSecondsTimeOut);

            //Page.ClientScript.RegisterStartupScript(typeof(Page), UniqueID + "Reconnect", strScript);
            //Session.Timeout = 60 * 2;

        }

        /// <summary>
        ///     Set time out value by second
        /// </summary>
        /// <param name="control"></param>
        public void SetTimeOut(Control control)
        {
            // Form the script to be registered at client side.
            var sb = new StringBuilder();

            // 1 seconds
            const int timeInterval = 1000;
            sb.Append("<script language='javascript'>\n");
            sb.Append("{\n");
#pragma warning disable 618
            sb.AppendFormat("window.setTimeout(\"{0}\",{1});\n", Page.GetPostBackClientEvent(control, ""), timeInterval);
#pragma warning restore 618
            sb.Append("}\n");
            sb.Append("</script>\n");

            // Register the startup script
#pragma warning disable 618
            Page.RegisterStartupScript(control.ClientID, sb.ToString());
#pragma warning restore 618
        }

        /// <summary>
        ///     Determine specified object is equal to String Empty or not.
        /// </summary>
        /// <param name="Object"></param>
        /// <returns></returns>
        protected bool IsStringEmpty(string Object)
        {
            return Object.Trim() == string.Empty;
        }

        /// <summary>
        ///     Approve WorkFlow
        /// </summary>
        /// <param name="apprStatus"></param>
        /// <param name="step"></param>
        /// <param name="appr"></param>
        /// <param name="neededAppr"></param>
        /// <returns></returns>
        protected bool IsApproveWF(string apprStatus, int step, int appr, int neededAppr)
        {
            var found = 0;

            if (apprStatus.Length > 0)
            {
                var stepApprStatus = apprStatus.Substring((step - 1) * 10, 10);

                for (var i = 0; i < stepApprStatus.Count(); i++)
                {
                    var c = stepApprStatus[i];
                    if (c.ToString(CultureInfo.InvariantCulture).ToUpper() == "A")
                    {
                        found++;
                    }
                }
            }

            return found == ((10 - appr) + neededAppr);
        }

        /// <summary>
        ///     Determine specified object is equal to DBNull value or not.
        /// </summary>
        /// <param name="Object"></param>
        /// <returns></returns>
        protected bool IsDBNull(object Object)
        {
            return Object == DBNull.Value;
        }

        public void Page_Error(object sender, EventArgs e)
        {
            var objErr = Server.GetLastError().GetBaseException();
            Session["Err"] = objErr;

            var err = string.Format(
                @"<br />Message : {0}
                <br />StackTrace : {1}
                <br />Source :{2}
                <br /> Target Site : {3}
                <br /> Data : {4}", objErr.Message, objErr.StackTrace, objErr.Source, objErr.TargetSite, objErr.Data);
            Response.Write(err);
            Response.Redirect("~/ErrorPages/Default.aspx");
            Server.ClearError();
        }

        #endregion

        #region "ColumnTemplate"

        /// <summary>
        /// </summary>
        public class WorkFlowColumnTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                var gridContainer = (GridViewDataItemTemplateContainer)container;

                for (var i = 0; i <= gridContainer.Text.Length - 10; i += 10)
                {
                    var imgWF = new Image();

                    if (gridContainer.Text.Substring(i, 10).Contains('R'))
                    {
                        imgWF.ImageUrl = "~/App_Themes/Default/Images/WF/REJ.gif";
                    }
                    else if (gridContainer.Text.Substring(i, 10).Contains('_') &&
                             gridContainer.Text.Substring(i, 10).Contains('P'))
                    {
                        imgWF.ImageUrl = "~/App_Themes/Default/Images/WF/PAR.gif";
                    }
                    else if (gridContainer.Text.Substring(i, 10).Contains('_'))
                    {
                        imgWF.ImageUrl = "~/App_Themes/Default/Images/WF/NA.gif";
                    }
                    else
                    {
                        imgWF.ImageUrl = "~/App_Themes/Default/Images/WF/APP.gif";
                    }

                    container.Controls.Add(imgWF);
                }
            }
        }

        #endregion

        #region Functions

        public decimal RoundQty(decimal value, string connectionString = null)
        {
            string defaultDigit = DefaultQtyDigit.ToString(); // Current Business unit

            if (!string.IsNullOrEmpty(connectionString)) // Get from specific Business unit
            {
                Blue.BL.APP.Config c = new Blue.BL.APP.Config();
                defaultDigit = c.GetValue("APP", "Default", "DigitQty", connectionString);
                if (string.IsNullOrEmpty(defaultDigit))
                    defaultDigit = "3";
            }

            return Math.Round(value, Convert.ToInt32(defaultDigit), MidpointRounding.AwayFromZero);
        }
        
        public decimal RoundAmt(decimal value, string connectionString = null)
        {
            string defaultDigit = DefaultAmtDigit.ToString(); // Current Business unit

            if (!string.IsNullOrEmpty(connectionString)) // Get from specific Business unit
            {
                Blue.BL.APP.Config c = new Blue.BL.APP.Config();
                defaultDigit = c.GetValue("APP", "Default", "DigitAmt", connectionString);
                if (string.IsNullOrEmpty(defaultDigit))
                    defaultDigit = "3";
            }

            return Math.Round(value, Convert.ToInt32(defaultDigit), MidpointRounding.AwayFromZero);
        }

        // คำนวนหา ผลรวมราคาก่อนหักภาษี
        public decimal CalAmt(decimal price, decimal discount, decimal quantity)
        {
            return RoundAmt((price - discount) * quantity); ;
        }

        // คำนวนหา ผลรวมของภาษี
        public decimal TaxAmt(string taxType, decimal taxRate, decimal price, decimal discount, decimal quantity)
        {
            taxType = taxType[0].ToString().ToUpper();
            decimal value;
            switch (taxType)
            {
                case "I":
                    value = (CalAmt(price, discount, quantity) * taxRate) / (100 + taxRate);
                    break;
                case "A":
                    value = (CalAmt(price, discount, quantity) * taxRate) / 100;
                    break;
                default:
                    value = 0;
                    break;
            }
            return RoundAmt(value);
        }

        // คำนวนหา ผลรวมราคาหลังหักภาษี
        public decimal NetAmt(string taxType, decimal taxRate, decimal price, decimal discount, decimal quantity)
        {
            decimal value;

            if (taxType.ToUpper().StartsWith("I"))
            {
                value = CalAmt(price, discount, quantity) - TaxAmt(taxType, taxRate, price, discount, quantity);
            }
            else
            {
                value = CalAmt(price, discount, quantity);
            }
            return RoundAmt(value);
        }

        // คำนวนหา ผลรวมราคา + ภาษี
        public decimal Amount(string taxType, decimal taxRate, decimal price, decimal discount, decimal quantity)
        {
            taxType = taxType[0].ToString().ToUpper();
            decimal value;
            if (taxType == "I")
            {
                value = NetAmt(taxType, taxRate, price, discount, quantity) + TaxAmt(taxType, taxRate, price, discount, quantity);
            }
            else
            {
                value = CalAmt(price, discount, quantity) + TaxAmt(taxType, taxRate, price, discount, quantity);
            }

            return RoundAmt(value);
        }

        public string GetTaxTypeName(string taxType)
        {
            var taxName = string.Empty;
            if (string.IsNullOrEmpty(taxType))
                taxType = "N";

            switch (taxType[0].ToString().ToUpper())
            {
                case "A":
                    taxName = "Add";
                    break;
                case "I":
                    taxName = "Included";
                    break;
                default:
                    taxName = "None";
                    break;
            }

            return taxName;
        }

        #endregion

        protected void AlertMessageBox(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Alert", "alert('" + msg + "');", true);
        }

        public DataSet GetDataSet(string connectionString, string sql)
        {
            var conn = new SqlConnection(connectionString);
            var da = new SqlDataAdapter();
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            da.SelectCommand = cmd;
            var ds = new DataSet();

            conn.Open();
            da.Fill(ds);
            conn.Close();

            return ds;
        }

    }


    // Other Classs
    public class KeyValues
    {
        private Dictionary<string, string> _dictionary;
        private string _textString;
        private char _delimiter;

        public KeyValues()
        {
            _dictionary = new Dictionary<string, string>();
            _textString = string.Empty;
            _delimiter = ';';

        }

        public KeyValues(string value, char delimiter)
        {
            _delimiter = delimiter;
            _textString = value;
            ToDictionary(value);
        }

        // ----------------------------------------------------
        // Properties

        public string Text
        {
            get { return _textString; }
            set
            {
                _textString = value;
                ToDictionary(value);
            }
        }

        public char Delimiter
        {
            get { return _delimiter; }
            set { _delimiter = value; }
        }

        // ----------------------------------------------------
        // Method

        public string Value(string key)
        {
            if (_dictionary.ContainsKey(key))
                return _dictionary[key].ToString();
            else
                return string.Empty;

        }

        public void Add(string key, string value)
        {
            if (_dictionary.ContainsKey(key))
                _dictionary[key] = value;
            else
                _dictionary.Add(key, value);
        }

        public override string ToString()
        {
            return string.Join(_delimiter.ToString(), _dictionary.Select(x => x.Key + "=" + x.Value).ToArray());
        }

        public void Clear()
        {
            _textString = string.Empty;
        }

        // ------------------------------------------------------

        private void ToDictionary(string text)
        {
            if (text == string.Empty)
            {
                text = string.Empty;
                _dictionary.Clear();
            }
            else
            {

                // remove ; at the end
                text = text.EndsWith(";") ? text.Remove(text.Length - 1) : text;
                try
                {
                    _dictionary = text.Split(_delimiter).Select(s => s.Split('=')).ToDictionary(key => key[0].Trim(), value => value[1].Trim());
                }
                catch
                {
                }
            }
        }





    }

    public class Calculation
    {
        #region Properties

        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        public decimal Quantity
        {
            get { return qty; }
            set { qty = value; }
        }

        public string TaxType
        {
            get { return taxType; }
            set { taxType = value; }
        }

        public decimal TaxRate
        {
            get { return taxRate; }
            set { TaxRate = value; }
        }

        public decimal DiscountRate
        {
            get { return discRate; }
            set { discRate = value; }
        }

        public decimal DiscountAmount
        {
            get { return discAmt; }
            set { discAmt = value; }
        }

        public decimal NetAmount
        {
            get { return GetNetAmt(); }
        }

        public decimal TaxAmount
        {
            get { return GetTaxAmt(); }
        }

        public decimal TotalAmount
        {
            get { return GetTotalAmt(); }
        }

        #endregion

        #region Private variables

        private string taxType = "N";
        private decimal taxRate = 0m;
        private decimal discRate = 0m;
        private decimal discAmt = 0m;

        private decimal price = 0m;
        private decimal qty = 0m;

        #endregion

        public Calculation()
        {
        }


        private decimal GetNetAmt()
        {
            throw new NotImplementedException();
        }
        private decimal GetTaxAmt()
        {
            throw new NotImplementedException();
        }

        private decimal GetTotalAmt()
        {
            throw new NotImplementedException();
        }


    }
}