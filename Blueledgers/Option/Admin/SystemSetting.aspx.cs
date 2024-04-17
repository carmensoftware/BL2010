using System;
using System.Data;
using BlueLedger.PL.BaseClass;
using System.Web;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;

namespace BlueLedger.PL.Option.Admin
{
    public partial class SystemSetting : BasePage
    {
        private string sys_db = System.Configuration.ConfigurationSettings.AppSettings["ConnStr"];
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private readonly Blue.BL.ADMIN.Bu bu = new Blue.BL.ADMIN.Bu();

        //private DataSet dsConfig = null;
        //private KeyValues smtpNotification = new KeyValues();
        //private KeyValues smtpPO = new KeyValues();

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Session.Remove("smtpNotification");
                Session.Remove("smtpPO");
                Session.Remove("dsConfig");

                Page_Setting();
            }
            else
            {
                //smtpNotification = (KeyValues)Session["smtpNotification"];
                //smtpPO = (KeyValues)Session["smtpPO"];

                if (FileUpload1.PostedFile != null)
                {
                    if (FileUpload1.PostedFile.FileName.Length > 0)
                    {
                        Load_BuLogo();
                    }

                }


            }


        }

        private void Page_Setting()
        {
            //SetControlEditMode_Bu(false);
            //SetControlEditMode_Mail(false);
            //SetControlEditMode_Interface(false);
            //SetControlEditMode_Password(false);
            //SetControlEditMode_WebServer(false);

            //GetCompanyProfile();
            //GetMailConfig();
            //GetInterfaceConfig();
            //GetPasswordConfig();
            //GetWebSererConfig();

        }

        protected void btn_CompanyProfile_Click(object sender, EventArgs e)
        {
            SetMode_Profile(false);
            GetConfig_Profile();

            pop_CompanyProfile.ShowOnPageLoad = true;
        }

        protected void btn_MailServer_Click(object sender, EventArgs e)
        {
            SetMode_Mail(false);
            GetConfig_Mail();

            pop_MailServer.ShowOnPageLoad = true;
        }

        protected void btn_InterfaceAccount_Click(object sender, EventArgs e)
        {
            SetMode_Interface(false);
            GetConfig_Interface();

            pop_AccountInterface.ShowOnPageLoad = true;
        }

        protected void btn_Password_Click(object sender, EventArgs e)
        {
            SetMode_Password(false);
            GetConfig_Password();

            pop_Password.ShowOnPageLoad = true;
        }

        protected void btn_WebServer_Click(object sender, EventArgs e)
        {
            SetMode_WebServer(false);
            GetConfig_WebServer();

            pop_WebServer.ShowOnPageLoad = true;
        }

        protected void btn_System_Click(object sendere, EventArgs e)
        {
            SetMode_System(false);
            GetConfig_System();

            pop_System.ShowOnPageLoad = true;
        }

        protected void btn_Alert_Ok_Click(object sender, EventArgs e)
        {
            pop_Alert.ShowOnPageLoad = false;
        }

        // ---------------------------------------------------------------
        #region Company Profile

        private void SetMode_Profile(bool isEdit)
        {
            btn_EditBu.Visible = !isEdit;
            btn_SaveBu.Visible = isEdit;
            btn_CancelBu.Visible = isEdit;

            txt_BuName.ReadOnly = !isEdit;
            txt_NameBilling.ReadOnly = !isEdit;
            txt_Address.ReadOnly = !isEdit;
            txt_PostCode.ReadOnly = !isEdit;
            txt_Phone.ReadOnly = !isEdit;
            txt_Fax.ReadOnly = !isEdit;
            txt_Email.ReadOnly = !isEdit;
            txt_TaxId.ReadOnly = !isEdit;

            FileUpload1.Visible = isEdit;

        }

        private void GetConfig_Profile()
        {
            string message = string.Empty;
            DataSet ds = new DataSet();
            if (bu.GetBuList(ds, ref message, LoginInfo.ConnStr))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                lbl_BuCode.Text = "(" + dr["BuCode"].ToString() + ")";

                txt_BuName.Text = dr["Name"].ToString();
                txt_NameBilling.Text = dr["NameBilling"].ToString();
                txt_Address.Text = dr["Address"].ToString();
                txt_PostCode.Text = dr["PostCode"].ToString();
                txt_Phone.Text = dr["Phone"].ToString();
                txt_Fax.Text = dr["Fax"].ToString();
                txt_Email.Text = dr["Email"].ToString();
                txt_TaxId.Text = dr["TaxId"].ToString();


                if (dr["BuLogo"] != System.DBNull.Value)
                {
                    byte[] bytes = (byte[])dr["BuLogo"];
                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    img_BuLogo.ImageUrl = "data:image/png;base64," + base64String;
                }

            }
        }

        private void Load_BuLogo()
        {
            if (FileUpload1.HasFile)
            {
                HttpPostedFile file = FileUpload1.PostedFile;
                Byte[] imgbyte = new Byte[file.ContentLength];
                file.InputStream.Read(imgbyte, 0, file.ContentLength);

                SqlConnection con = new SqlConnection(LoginInfo.ConnStr);
                SqlCommand cmd = new SqlCommand("UPDATE [ADMIN].Bu SET BuLogo = @img", con);
                cmd.Parameters.AddWithValue("@img", imgbyte);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    //Label1.Text = "Image Uploaded";
                    GetConfig_Profile();
                }
                catch
                {
                    //Label1.Text = "Try Again";
                }
            }
        }

        protected void btn_EditBu_Click(object sender, EventArgs e)
        {
            GetConfig_Profile();
            SetMode_Profile(true);

        }

        protected void btn_SaveBu_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(LoginInfo.ConnStr);
            SqlCommand cmd = new SqlCommand("UPDATE [ADMIN].Bu SET Name=@Name, NameBilling=@NameBilling, Address=@Address, PostCode=@PostCode, Phone=@Phone, Fax=@Fax, Email=@Email, TaxId=@TaxId", con);
            cmd.Parameters.AddWithValue("@Name", txt_BuName.Text);
            cmd.Parameters.AddWithValue("@NameBilling", txt_NameBilling.Text);
            cmd.Parameters.AddWithValue("@Address", txt_Address.Text);
            cmd.Parameters.AddWithValue("@PostCode", txt_PostCode.Text);
            cmd.Parameters.AddWithValue("@Phone", txt_Phone.Text);
            cmd.Parameters.AddWithValue("@Fax", txt_Fax.Text);
            cmd.Parameters.AddWithValue("@Email", txt_Email.Text);
            cmd.Parameters.AddWithValue("@TaxId", txt_TaxId.Text);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                GetConfig_Profile();
                //pop_Alert.Text = "Profile is saved.";
                //pop_Alert.ShowOnPageLoad = true;

            }
            catch (Exception ex)
            {
                pop_Alert.Text = ex.Message;
                pop_Alert.ShowOnPageLoad = true;
            }

            SetMode_Profile(false);
            Response.Redirect("SystemSetting.aspx");
        }

        protected void btn_CancelBu_Click(object sender, EventArgs e)
        {
            SetMode_Profile(false);
            Response.Redirect("SystemSetting.aspx");
        }


        #endregion

        // ---------------------------------------------------------------
        #region Mail Configuration

        private void SetMode_Mail(bool isEdit)
        {
            btn_EditSMTP.Visible = !isEdit;
            btn_SaveSMTP.Visible = isEdit;
            btn_CancelSMTP.Visible = isEdit;

            txt_ServerName.ReadOnly = !isEdit;
            txt_Port.ReadOnly = !isEdit;
            check_SSL.ReadOnly = !isEdit;
            check_Authen.ReadOnly = !isEdit;

            txt_Username.ReadOnly = !isEdit;
            txt_Password.ReadOnly = !isEdit;
            txt_Name.ReadOnly = !isEdit;

            txt_PoUsername.ReadOnly = !isEdit;
            txt_PoPassword.ReadOnly = !isEdit;
            txt_PoSenderName.ReadOnly = !isEdit;
            txt_PoMessageBody.ReadOnly = !isEdit;
            txt_PoCc.ReadOnly = !isEdit;
            chk_PoCcHod.Enabled = isEdit;
        }

        private void GetConfig_Mail()
        {
            KeyValues smtpNotification = new KeyValues();
            KeyValues smtpPO = new KeyValues();
            string encryptString = string.Empty;

            // Notification
            encryptString = config.GetValue("SYS", "Mail", "ServerString", LoginInfo.ConnStr);
            if (encryptString != string.Empty)
            {
                smtpNotification.Text = Blue.BL.GnxLib.EnDecryptString(encryptString, Blue.BL.GnxLib.EnDeCryptor.DeCrypt);

                txt_ServerName.Text = smtpNotification.Value("smtp");
                txt_Port.Text = smtpNotification.Value("port");
                check_SSL.Checked = smtpNotification.Value("enablessl").ToUpper() == "TRUE";
                check_Authen.Checked = smtpNotification.Value("authenticate").ToUpper() == "TRUE";
                txt_Username.Text = smtpNotification.Value("username");
                txt_Password.Text = smtpNotification.Value("password");
                txt_Name.Text = smtpNotification.Value("name");
            }

            // PO
            encryptString = config.GetValue("SYS", "Mail", "PO", LoginInfo.ConnStr);
            if (encryptString != string.Empty)
            {
                smtpPO.Text = Blue.BL.GnxLib.EnDecryptString(encryptString, Blue.BL.GnxLib.EnDeCryptor.DeCrypt);

                //txt_ServerName.Text = smtpPO.Value("smtp");
                //txt_Port.Text = smtpPO.Value("port");
                //check_SSL.Checked = smtpPO.Value("enablessl").ToUpper() == "TRUE";
                //check_Authen.Checked = smtpPO.Value("authenticate").ToUpper() == "TRUE";
                txt_PoSenderName.Text = smtpPO.Value("name");
                txt_PoUsername.Text = smtpPO.Value("username");
                txt_PoPassword.Text = smtpPO.Value("password");

                txt_PoSubject.Text = smtpPO.Value("subject");
                txt_PoMessageBody.Text = smtpPO.Value("messagebody");
                txt_PoMessageBody.ToolTip = txt_PoMessageBody.Text;
                txt_PoCc.Text = smtpPO.Value("cc");
                txt_PoCc.ToolTip = txt_PoCc.Text;
                chk_PoCcHod.Checked = smtpPO.Value("ccHOD").ToUpper() == "TRUE";

            }



            //Session["dsConfig"] = dsConfig;
            Session["smtpNotification"] = smtpNotification;
            Session["smtpPO"] = smtpPO;
        }

        protected void check_SSL_CheckedChanged(object sender, EventArgs e)
        {
            //if (check_SSL.Checked)
            //{
            //    lbl_PortDefault.Text = "Default: 465";
            //    txt_Port.Number = 465;
            //}
            //else
            //{
            //    lbl_PortDefault.Text = "Default: 587";
            //    txt_Port.Number = 587;
            //}
        }

        protected void check_Authen_CheckedChanged(object sender, EventArgs e)
        {
            txt_Username.Enabled = check_Authen.Checked;
            txt_Password.Enabled = check_Authen.Checked;
        }

        protected void btn_EditSMTP_Click(object sender, EventArgs e)
        {
            GetConfig_Mail();
            SetMode_Mail(true);
        }

        protected void btn_SaveSMTP_Click(object sender, EventArgs e)
        {
            KeyValues smtpNotification = new KeyValues();
            smtpNotification = (KeyValues)Session["smtpNotification"];

            DataSet dsNotification = new DataSet();
            // Notification

            string oldPassword = smtpNotification.Value("password");
            smtpNotification.Text = string.Empty;

            smtpNotification.Add("smtp", txt_ServerName.Text);
            smtpNotification.Add("port", txt_Port.Text == string.Empty ? "587" : txt_Port.Text);
            smtpNotification.Add("enablessl", check_SSL.Checked ? "true" : "false");
            smtpNotification.Add("authenticate", check_Authen.Checked ? "true" : "false");
            smtpNotification.Add("username", txt_Username.Text);
            if (txt_Password.Text == string.Empty)
                smtpNotification.Add("password", oldPassword);
            else
                smtpNotification.Add("password", txt_Password.Text);
            smtpNotification.Add("name", txt_Name.Text);

            string encryptString = Blue.BL.GnxLib.EnDecryptString(smtpNotification.ToString(), Blue.BL.GnxLib.EnDeCryptor.EnCrypt);
            config.Get(dsNotification, "SYS", "Mail", "ServerString", LoginInfo.ConnStr);
            if (dsNotification.Tables[0].Rows.Count == 0)
            {
                DataRow dr = dsNotification.Tables[0].NewRow();
                dr["Module"] = "SYS";
                dr["SubModule"] = "Mail";
                dr["Key"] = "ServerString";
                dr["Value"] = encryptString;
                dsNotification.Tables[0].Rows.Add(dr);
            }
            else
                dsNotification.Tables[0].Rows[0]["Value"] = encryptString;

            config.Save(dsNotification, LoginInfo.ConnStr);


            // PO Server
            KeyValues smtpPO = new KeyValues();
            smtpPO = (KeyValues)Session["smtpPO"];

            oldPassword = smtpPO.Value("password");

            smtpPO.Text = string.Empty;
            smtpPO.Add("smtp", txt_ServerName.Text);
            smtpPO.Add("port", txt_Port.Text == string.Empty ? "587" : txt_Port.Text);
            smtpPO.Add("enablessl", check_SSL.Checked ? "true" : "false");
            smtpPO.Add("authenticate", check_Authen.Checked ? "true" : "false");

            smtpPO.Add("username", txt_PoUsername.Text);
            if (txt_PoPassword.Text == string.Empty)
                smtpPO.Add("password", oldPassword);
            else
                smtpPO.Add("password", txt_PoPassword.Text);

            smtpPO.Add("name", txt_PoSenderName.Text);
            smtpPO.Add("subject", txt_PoSubject.Text);
            smtpPO.Add("messagebody", txt_PoMessageBody.Text.Replace(';', ' '));
            smtpPO.Add("cc", txt_PoCc.Text.Replace(';', ','));
            smtpPO.Add("ccHOD", chk_PoCcHod.Checked ? "true" : "false");


            encryptString = Blue.BL.GnxLib.EnDecryptString(smtpPO.ToString(), Blue.BL.GnxLib.EnDeCryptor.EnCrypt);
            DataSet dsPO = new DataSet();
            config.Get(dsPO, "SYS", "Mail", "PO", LoginInfo.ConnStr);
            if (dsPO.Tables[0].Rows.Count == 0)
            {
                DataRow dr = dsPO.Tables[0].NewRow();
                dr["Module"] = "SYS";
                dr["SubModule"] = "Mail";
                dr["Key"] = "PO";
                dr["Value"] = encryptString;
                dsPO.Tables[0].Rows.Add(dr);
            }
            else
                dsPO.Tables[0].Rows[0]["Value"] = encryptString;

            config.Save(dsPO, LoginInfo.ConnStr);

            SetMode_Mail(false);
            GetConfig_Mail();
        }

        protected void btn_CancelSMTP_Click(object sender, EventArgs e)
        {
            SetMode_Mail(false);
            GetConfig_Mail();
            //Response.Redirect("SystemSetting.aspx");
        }

        protected void btn_TestSendMail_Click(object sender, EventArgs e)
        {
            KeyValues smtpNotification = new KeyValues();
            smtpNotification = (KeyValues)Session["smtpNotification"];

            if (txt_TestReceiver.Text != string.Empty)
            {
                Mail email = new Mail();

                

                email.SmtpServer = txt_ServerName.Text;
                email.Port = Convert.ToInt16(txt_Port.Text);
                email.EnableSsl = check_SSL.Checked;

                email.IsAuthentication = check_Authen.Checked;
                if (check_Authen.Checked)
                {
                    email.Name = txt_Name.Text;
                    email.UserName = txt_Username.Text;
                    if (txt_Password.Text == string.Empty)
                        email.Password = smtpNotification.Value("password");
                    else
                        email.Password = txt_Password.Text;
                }
                
                email.From = txt_Username.Text;
                email.To = txt_TestReceiver.Text;
                email.Subject = string.Format("Test sending mail from Blueledgers ({0}) at {1}", LoginInfo.BuInfo.BuCode, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                email.Body = string.Format("This is testing to send email from '{0} : {1}'.", LoginInfo.BuInfo.BuCode, LoginInfo.BuInfo.BuName);

                try
                {
                    var error = email.Send();

                    if (string.IsNullOrEmpty(error))
                    {
                        lbl_TestReceiver.Text = "Mail sent.";
                        lbl_TestReceiver.ForeColor = System.Drawing.Color.DarkGreen;
                    }
                    else
                    {

                        lbl_TestReceiver.Text = error;
                        lbl_TestReceiver.ForeColor = System.Drawing.Color.Red;
                    }
                }
                catch (Exception ex)
                {
                    lbl_TestReceiver.Text = ex.Message;
                }

                lbl_TestReceiver.ToolTip = lbl_TestReceiver.Text;

            }
        }


        #endregion

        // ---------------------------------------------------------------------
        #region Interface

        private void SetMode_Interface(bool isEdit)
        {
            btn_InterfaceEdit.Visible = !isEdit;
            btn_InterfaceSave.Visible = isEdit;
            btn_InterfaceCancel.Visible = isEdit;

            pn_Interface.Enabled = isEdit;
        }

        private void GetConfig_Interface()
        {
            KeyValues intf = new KeyValues();
            intf.Text = config.GetConfigValue("APP", "INTF", "ACCOUNT", LoginInfo.ConnStr);
            // type=API; auth=direct 602380b9f449404d7d6f6aaffcee4bd5|06ac476f-1ed4-4acf-9102-ad3d0bcabf17; host=http://app.blueledgers.com:88/Carmen.dev.api/; vendor=api/interface/vendor; accountcode=api/interface/accountCode; department=api/interface/department;

            if (intf.Text != string.Empty)
            {
                txt_InterfaceServer.Text = intf.Value("host");
                txt_InterfaceToken.Text = intf.Value("auth");
                txt_InterfaceAccCode.Text = intf.Value("accountcode");
                txt_InterfaceDepCode.Text = intf.Value("department");
                txt_InterfaceVendor.Text = intf.Value("vendor");
            }
            else
            {
                txt_InterfaceServer.Text = "";
                txt_InterfaceToken.Text = "";
                txt_InterfaceAccCode.Text = "";
                txt_InterfaceDepCode.Text = "";
                txt_InterfaceVendor.Text = "";
            }




            //KeyValues connStr = new KeyValues();
            //connStr.Text = config.GetConfigValue("SYS", "INTF", "AP", LoginInfo.ConnStr);

            //if (connStr.Text != string.Empty)
            //{
            //    connStr.Text = Blue.BL.GnxLib.EnDecryptString(connStr.Text, Blue.BL.GnxLib.EnDeCryptor.DeCrypt);

            //    txt_InterfaceServer.Text = connStr.Value("Server");
            //    txt_InterfacePort.Text = connStr.Value("Port");
            //    txt_InterfaceDatabase.Text = connStr.Value("Database");
            //    txt_InterfaceUserName.Text = connStr.Value("Uid");
            //}
            //else
            //{
            //    txt_InterfaceServer.Text = string.Empty;
            //    txt_InterfacePort.Text = string.Empty;
            //    txt_InterfaceDatabase.Text = string.Empty;
            //    txt_InterfaceUserName.Text = string.Empty;
            //    txt_InterfacePassword.Text = string.Empty;

            //}
        }

        // Interface
        protected void btn_InterfaceEdit_Click(object sender, EventArgs e)
        {
            SetMode_Interface(true);
        }

        protected void btn_InterfaceSave_Click(object sender, EventArgs e)
        {
            string host = txt_InterfaceServer.Text;
            string auth = txt_InterfaceToken.Text;
            string accountcode = txt_InterfaceAccCode.Text;
            string department = txt_InterfaceDepCode.Text;
            string vendor = txt_InterfaceVendor.Text;

            string intf = string.Format("type=API; host={0}; auth={1}; accountcode={2}; department={3}; vendor={4};", host, auth, accountcode, department, vendor);

            config.SetConfigValue("APP", "INTF", "ACCOUNT", intf, LoginInfo.ConnStr);


            SetMode_Interface(false);
        }

        protected void btn_InterfaceCancel_Click(object sender, EventArgs e)
        {
            GetConfig_Interface();
            SetMode_Interface(false);
        }


        #endregion

        // ---------------------------------------------------------------------
        #region Password

        private void SetMode_Password(bool isEdit)
        {
            btn_PasswordPolicyEdit.Visible = !isEdit;
            btn_PasswordPolicySave.Visible = isEdit;
            btn_PasswordPolicyCancel.Visible = isEdit;

            Panel_PasswordPolicy.Enabled = isEdit;
        }

        private void GetConfig_Password()
        {
            DataTable dtPassword = bu.DbExecuteQuery("SELECT * FROM dbo.Config WHERE [key] LIKE 'Password.%'", null, sys_db);

            if (dtPassword != null && dtPassword.Rows.Count > 0)
            {
                foreach (DataRow dr in dtPassword.Rows)
                {
                    string key = dr["Key"].ToString().ToLower();
                    string val = dr["Value"].ToString();

                    if (key == "password.length")
                    {
                        txt_PasswordLength.Text = val;
                    }

                    if (key == "password.complexity")
                    {
                        chk_ComplexityPassword.Checked = val.ToLower() == "true" || val == "1";
                    }

                    if (key == "password.expiredays")
                    {
                        txt_ExpireDays.Text = val;
                    }
                }
            }

        }


        protected void btn_PasswordPolicyEdit_Click(object sender, EventArgs e)
        {
            SetMode_Password(true);
        }

        protected void btn_PasswordPolicySave_Click(object sender, EventArgs e)
        {
            string sql = "";

            sql = @"IF NOT EXISTS (SELECT * FROM dbo.Config WHERE [key]='{0}')
                    BEGIN
                        INSERT INTO dbo.Config ([Key], Value)
                        VALUES ('{0}','{1}')
                    END
                    ELSE
                    BEGIN
                        UPDATE dbo.Config SET Value = '{1}' WHERE [Key] = '{0}'
                    END";

            // Password Length
            bu.DbExecuteQuery(string.Format(sql, "Password.Length", txt_PasswordLength.Text), null, sys_db);
            // Password Complexity
            bu.DbExecuteQuery(string.Format(sql, "Password.Complexity", chk_ComplexityPassword.Checked ? "1" : "0"), null, sys_db);
            // Password Expired day(s)
            bu.DbExecuteQuery(string.Format(sql, "Password.ExpireDays", txt_ExpireDays.Text), null, sys_db);

            SetMode_Password(false);

        }

        protected void btn_PasswordPolicyCancel_Click(object sender, EventArgs e)
        {
            GetConfig_Password();
            SetMode_Password(false);
        }

        #endregion

        // ---------------------------------------------------------------------
        #region Web Server

        private void SetMode_WebServer(bool isEdit)
        {
            btn_WebServerEdit.Visible = !isEdit;
            btn_WebServerSave.Visible = isEdit;
            btn_WebServerCancel.Visible = isEdit;

            Panel_WebServer.Enabled = isEdit;
        }

        private void GetConfig_WebServer()
        {
            string domain = config.GetValue("APP", "IM", "WebServer", LoginInfo.ConnStr);
            string subDomain = config.GetValue("APP", "IM", "WebName", LoginInfo.ConnStr);

            txt_Domain.Text = domain;
            txt_SubDomain.Text = subDomain;
            lbl_URL.Text = domain + "/" + subDomain;

        }

        protected void btn_WebServerEdit_Click(object sender, EventArgs e)
        {
            SetMode_WebServer(true);
        }

        protected void btn_WebServerSave_Click(object sender, EventArgs e)
        {
            string sql = "";

            // WebServer (Domain)
            sql = @"IF NOT EXISTS (SELECT * FROM APP.Config WHERE [Module]='APP' AND [SubModule]='IM' AND [Key]='WebServer')
                    BEGIN
                        INSERT INTO APP.Config ([Module],[SubModule],[Key], [Value], [UpdatedDate], [UpdatedBy])
                        VALUES ('APP','IM', 'WebServer', '{0}', GETDATE(), '{1}')
                    END
                    ELSE
                    BEGIN
                        UPDATE APP.Config SET Value = '{0}', UpdatedBy= N'{1}' WHERE [Module]='APP' AND [SubModule]='IM' AND [Key]='WebServer'
                    END";

            bu.DbExecuteQuery(string.Format(sql, txt_Domain.Text, LoginInfo.LoginName), null, LoginInfo.ConnStr);

            // WebName (SubDomain)
            sql = @"IF NOT EXISTS (SELECT * FROM APP.Config WHERE [Module]='APP' AND [SubModule]='IM' AND [Key]='WebName')
                    BEGIN
                        INSERT INTO APP.Config ([Module],[SubModule],[Key], [Value], [UpdatedDate], [UpdatedBy])
                        VALUES ('APP','IM', 'WebName', '{0}', GETDATE(), '{1}')
                    END
                    ELSE
                    BEGIN
                        UPDATE APP.Config SET Value = '{0}', UpdatedBy= N'{1}' WHERE [Module]='APP' AND [SubModule]='IM' AND [Key]='WebName'
                    END";

            bu.DbExecuteQuery(string.Format(sql, txt_SubDomain.Text, LoginInfo.LoginName), null, LoginInfo.ConnStr);

            SetMode_WebServer(false);
            GetConfig_WebServer();
        }

        protected void btn_WebServerCancel_Click(object sender, EventArgs e)
        {
            GetConfig_WebServer();
            SetMode_WebServer(false);
        }

        #endregion

        // ---------------------------------------------------------------------
        #region System Paramters

        private void SetMode_System(bool isEdit)
        {
            btn_EditSystem.Visible = !isEdit;
            btn_SaveSystem.Visible = isEdit;
            btn_CancelSystem.Visible = isEdit;

            txt_SystemCurrency.Enabled = isEdit;
            txt_SystemTaxRate.Enabled = isEdit;
            txt_SystemSvcRate.Enabled = isEdit;
            txt_SystemDigitAmt.Enabled = false;
            txt_SystemDigitQty.Enabled = false;
            txt_SystemCost.Enabled = false;
            chk_EnableEditCommit.Enabled = isEdit;
        }

        private void GetConfig_System()
        {
            string currency = config.GetValue("APP", "BU", "DefaultCurrency", LoginInfo.ConnStr);

            string taxRate = config.GetValue("APP", "Default", "TaxRate", LoginInfo.ConnStr);
            string svcRate = config.GetValue("APP", "Default", "SvcRate", LoginInfo.ConnStr);

            string digitAmt = config.GetValue("APP", "Default", "DigitAmt", LoginInfo.ConnStr);
            string digitQty = config.GetValue("APP", "Default", "DigitQty", LoginInfo.ConnStr);

            string cost = config.GetValue("IN", "SYS", "COST", LoginInfo.ConnStr);
            string enableEditCommit = config.GetValue("APP", "SYS", "EnableEditCommit", LoginInfo.ConnStr);


            txt_SystemCurrency.Text = currency;
            txt_SystemTaxRate.Text = taxRate;
            txt_SystemSvcRate.Text = svcRate;
            txt_SystemDigitAmt.Text = digitAmt;
            txt_SystemDigitQty.Text = digitQty;

            txt_SystemCost.Text = cost.ToUpper() == "FIFO" ? "FIFO" : "Average";
            chk_EnableEditCommit.Checked = enableEditCommit == "1";
        }

        protected void btn_EditSystem_Click(object sender, EventArgs e)
        {
            SetMode_System(true);

        }

        protected void btn_SaveSystem_Click(object sender, EventArgs e)
        {
            decimal value;

            if (!Decimal.TryParse(txt_SystemTaxRate.Text, out value))
            {
                lbl_Alert.Text = "Tax rate is invalid value.";
                pop_Alert.ShowOnPageLoad = true;
                return;
            }

            if (!Decimal.TryParse(txt_SystemSvcRate.Text, out value))
            {
                lbl_Alert.Text = "Service rate is invalid value.";
                pop_Alert.ShowOnPageLoad = true;
                return;
            }


            string sql = "";

            // Tax Rate
            sql = @"IF NOT EXISTS (SELECT * FROM APP.Config WHERE [Module]='APP' AND [SubModule]='Default' AND [Key]='TaxRate')
                    BEGIN
                        INSERT INTO APP.Config ([Module],[SubModule],[Key], [Value], [UpdatedDate], [UpdatedBy])
                        VALUES ('APP','Default', 'TaxRate', '{0}', GETDATE(), '{1}')
                    END
                    ELSE
                    BEGIN
                        UPDATE APP.Config SET Value = '{0}', UpdatedBy= N'{1}' WHERE [Module]='APP' AND [SubModule]='Default' AND [Key]='TaxRate'
                    END";

            bu.DbExecuteQuery(string.Format(sql, txt_SystemTaxRate.Text, LoginInfo.LoginName), null, LoginInfo.ConnStr);

            // Tax Rate
            sql = @"IF NOT EXISTS (SELECT * FROM APP.Config WHERE [Module]='APP' AND [SubModule]='Default' AND [Key]='SvcRate')
                    BEGIN
                        INSERT INTO APP.Config ([Module],[SubModule],[Key], [Value], [UpdatedDate], [UpdatedBy])
                        VALUES ('APP','Default', 'SvcRate', '{0}', GETDATE(), '{1}')
                    END
                    ELSE
                    BEGIN
                        UPDATE APP.Config SET Value = '{0}', UpdatedBy= N'{1}' WHERE [Module]='APP' AND [SubModule]='Default' AND [Key]='SvcRate'
                    END";

            bu.DbExecuteQuery(string.Format(sql, txt_SystemSvcRate.Text, LoginInfo.LoginName), null, LoginInfo.ConnStr);


            // Enable edit commit
            sql = @"IF NOT EXISTS (SELECT * FROM APP.Config WHERE [Module]='APP' AND [SubModule]='SYS' AND [Key]='EnableEditCommit')
                    BEGIN
                        INSERT INTO APP.Config ([Module],[SubModule],[Key], [Value], [UpdatedDate], [UpdatedBy])
                        VALUES ('APP','SYS', 'EnableEditCommit', '{0}', GETDATE(), '{1}')
                    END
                    ELSE
                    BEGIN
                        UPDATE APP.Config SET Value = '{0}', UpdatedBy= N'{1}' WHERE [Module]='APP' AND [SubModule]='SYS' AND [Key]='EnableEditCommit'
                    END";

            bu.DbExecuteQuery(string.Format(sql, chk_EnableEditCommit.Checked ? "1" : "0", LoginInfo.LoginName), null, LoginInfo.ConnStr);

            SetMode_System(false);
            GetConfig_System();
        }

        protected void btn_CancelSystem_Click(object sender, EventArgs e)
        {
            GetConfig_System();
            SetMode_System(false);
        }

        #endregion



    }


}