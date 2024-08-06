using System;
using System.Globalization;
using System.Linq;
using DevExpress.Web.ASPxGridView;

namespace BlueLedger.PL.BaseClass
{
    /// <summary>
    ///     Initial neccessary variable which widely used in presentation layer.
    /// </summary>
    public class BasePage2 : System.Web.UI.Page
    {
        #region "Attributes"

        private readonly Blue.BL.ProjectAdmin.SysParameter _sysParameter = new Blue.BL.ProjectAdmin.SysParameter();

        /// <summary>
        ///     Gets current login user information.
        /// </summary>
        protected LoginInformation LoginInfo
        {
            get { return (LoginInformation)Session["LoginInfo"]; }
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

        /// <summary>
        ///     Assign page defaul setting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Page_PreInit(object sender, EventArgs e)
        {
            // assign page theme.          
            Page.Theme = LoginInfo.Theme;

            // assign page culture.            
            Page.Culture = LoginInfo.DispLang;
            Page.UICulture = LoginInfo.DispLang;

            var appCulture = System.Threading.Thread.CurrentThread.CurrentCulture.Clone() as CultureInfo;

            if (appCulture != null)
            {
                appCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                appCulture.DateTimeFormat.DateSeparator = "/";
                appCulture.DateTimeFormat.ShortTimePattern = "hh:mm";
                appCulture.DateTimeFormat.LongTimePattern = "hh:mm:ss";

                System.Threading.Thread.CurrentThread.CurrentCulture = appCulture;
            }

            // This is necessary because Safari and Chrome browsers don't display the Menu control correctly.
            // All webpages displaying an ASP.NET menu control must inherit this class.
            if (Request.ServerVariables["http_user_agent"].IndexOf("Safari",
                StringComparison.CurrentCultureIgnoreCase) != -1)
            {
                Page.ClientTarget = "uplevel";
            }
        }

        /// <summary>
        ///     Register javascript reference at start page.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            const string script = "<script type=\"text/javascript\" src=\"/blueledgers/Scripts/GnxLib.js\"></script>";

            if (!ClientScript.IsClientScriptBlockRegistered("Startup"))
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "Startup", script);
            }

            base.OnPreRender(e);
        }

        /// <summary>
        ///     Page load event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            AddKeepAlive();

            //if (Session["LoginInfo"] != null)
            //{
            //    //update time 
            //    useronline.GetStructure(dsUserOnline);
            //    for (int i = 0; i < dsUserOnline.Tables[0].Rows.Count; i++)
            //    {
            //        if (dsUserOnline.Tables[0].Rows[i]["UserId"].ToString() == LoginInfo.LoginName.ToString())
            //        {
            //            DateTime expireTime = (DateTime)dsUserOnline.Tables[0].Rows[i]["ExpireTime"];
            //            if (expireTime > DateTime.Now)
            //            {
            //                dsUserOnline.Tables[0].Rows[i]["ExpireTime"] = DateTime.Now.AddMinutes(10);
            //            }
            //        }
            //    }
            //    useronline.UpdateUserOnline(dsUserOnline);
            //}
            //else
            //{ 
            //    //clear user expire
            //    useronline.GetStructure(dsUserOnline);
            //    for (int i = 0; i < dsUserOnline.Tables[0].Rows.Count; i++)
            //    {
            //    if(DateTime.Parse(dsUserOnline.Tables[0].Rows[i][4].ToString())<DateTime.Now)
            //        {
            //        dsUserOnline.Tables[0].Rows[i].Delete();
            //        }
            //    }
            //    useronline.UpdateUserOnline(dsUserOnline);
            //}
        }

        /// <summary>
        /// </summary>
        private void AddKeepAlive()
        {
            const int intMilliSecondsTimeOut = 2 * 60 * 1000;
            //Math.Max((this.Session.Timeout * 60000) - 30000, 5000);
            //var path = System.Web.VirtualPathUtility.ToAbsolute("~/KeepAlive.aspx");
            //var strScript = string.Format(
            //    @"<script>(function(){{var r=0,w=window;if (w.setInterval)w.setInterval(function() {{r++;var img=new Image(1,1);img.src='{0}?count='+r;}},{1});}})();</script>",
            //    path, intMilliSecondsTimeOut);

            //Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), UniqueID + "Reconnect", strScript);
        }

        /// <summary>
        ///     Set time out value by second
        /// </summary>
        /// <param name="control"></param>
        public void SetTimeOut(System.Web.UI.Control control)
        {
            // Form the script to be registered at client side.
            var sb = new System.Text.StringBuilder();

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

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Page_Error(object sender, EventArgs e)
        {
            //Exception objErr = Server.GetLastError().GetBaseException();
            //Session["Err"] = objErr;

            //string err = "<br />Message : " + objErr.Message.ToString()+
            //    "<br />StackTrace : " + objErr.StackTrace.ToString()+
            //    "<br />Source :" + objErr.Source.ToString()+
            //    "<br /> Target Site : " + objErr.TargetSite.ToString()+
            //    "<br /> Data : " + objErr.Data.ToString();
            //Response.Write(err);
            //Response.Redirect("~/ErrorPages/Default.aspx");
            //Server.ClearError();
        }

        #endregion

        #region "ColumnTemplate"

        /// <summary>
        /// </summary>
        public class WorkFlowColumnTemplate : System.Web.UI.ITemplate
        {
            /*
                        private LoginInformation _loginInfo = new BasePage2().LoginInfo;
            */

            //protected string GetApprStatusHeader(string ApprStatus, int Appr)
            //{
            //  int countPending = 0; 
            //  string tmpValue = string.Empty;
            //  string tmpStr =  ApprStatus.Substring(0, Appr);

            //      // Check Pending
            //      for (int x = 0; x < ApprStatus.Length; x++)
            //      {
            //          if (tmpStr.IndexOf('_') > -1)
            //          {
            //              countPending++;
            //          }
            //      }


            //      if ((tmpStr.IndexOf('A') > -1) || (tmpStr.IndexOf('P') > -1))
            //      {
            //          tmpValue = "A";
            //      }
            //      else if (tmpStr.IndexOf('R') > -1)
            //      {
            //          tmpValue = "R";
            //      }
            //      else if (countPending >= Appr)
            //      {
            //          tmpValue = "_";
            //      }

            //  return tmpValue;
            //}
            //protected string GetApprStatusDetail(string ApprStatus, int Appr)
            //{
            //    int countPending = 0;
            //    string tmpValue = string.Empty;
            //    string tmpStr = ApprStatus.Substring(0, Appr);


            //    for (int x = 0; x < ApprStatus.Length; x++)
            //    {
            //        if (ApprStatus.Substring(x, 1) == "_")
            //        {
            //            countPending++;
            //        }

            //        if (ApprStatus.Substring(x, 1) == "R")
            //        {
            //             tmpValue = "R";
            //             break;
            //        }

            //        if ((ApprStatus.Substring(x, 1) == "A") || (ApprStatus.Substring(x, 1) == "P"))
            //        {
            //            tmpValue = "A";
            //        }

            //    }

            //    if ((countPending >= Appr) && (countPending > 0))
            //    {
            //        tmpValue = "_";
            //    }

            //    return tmpValue;
            //}

            /// <summary>
            /// </summary>
            /// <param name="container"></param>
            public void InstantiateIn(System.Web.UI.Control container)
            {
                var gridContainer = (GridViewDataItemTemplateContainer)container;

                //int nloop = 0 ;
                //string[] Level = new string[nloop];

                //// case apprstatus is null , assign charecter '_' to default
                //if (gridContainer.Text ==  "&nbsp;" )
                //{
                //    nloop = (8);
                //    Level = new string[nloop];
                //    for (int i = 0; i < 8; i++)
                //    {
                //        Level[i] = "_";
                //    }
                //}
                //else
                //{
                //    // SubString  ApprStatus Detail to Array
                //    nloop = (gridContainer.Text.Length / 10);
                //    Level = new string[nloop];
                //    int iRunning = 0;
                //    for (int x = 0; x < nloop; x++)
                //    {
                //        BL.APP.WF workFlow = new BL.APP.WF();
                //        BL.APP.WFDt workFlowDt = new BL.APP.WFDt();

                //        DataTable dtworkFlow = workFlowDt.GetList(workFlow.GetWFId("PC", "PR", LoginInfo.ConnStr), LoginInfo.ConnStr);

                //        Level[x] = GetApprStatusDetail(gridContainer.Text.Substring(iRunning, 10), int.Parse(dtworkFlow.Rows[x]["Appr"].ToString()));
                //        iRunning += 10;
                //    }
                //}

                for (var i = 0; i <= gridContainer.Text.Length - 10; i += 10)
                {
                    var imgWF = new System.Web.UI.WebControls.Image();

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
    }
}