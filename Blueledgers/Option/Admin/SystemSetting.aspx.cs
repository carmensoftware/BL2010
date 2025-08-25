using System;
using System.Data;
using BlueLedger.PL.BaseClass;
using System.Web;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;

namespace BlueLedger.PL.Option.Admin
{
    public partial class SystemSetting : BasePage
    {
        private readonly string sys_db = System.Configuration.ConfigurationSettings.AppSettings["ConnStr"];
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

            GetConfig_WebServer();

            GetConfig_Mail();

            pop_MailServer.ShowOnPageLoad = true;
        }

        protected void btn_InterfaceSystem_Click(object sender, EventArgs e)
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
            //SetMode_WebServer(false);
            //GetConfig_WebServer();

            //pop_WebServer.ShowOnPageLoad = true;
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

                string connetionString = System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString();
                bu.DbExecuteQuery("UPDATE dbo.Bu SET BuName=@BuName WHERE BuCode=@BuCode", new Blue.DAL.DbParameter[] 
                {
                    new Blue.DAL.DbParameter("@BuCode", LoginInfo.BuInfo.BuCode),
                    new Blue.DAL.DbParameter("@BuName", txt_BuName.Text)
                }, connetionString);

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
            Panel_WebServer.Enabled = isEdit;

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


            // WebServer
            SaveWebServer();

            SetMode_Mail(false);
            GetConfig_Mail();

        }

        private void SaveWebServer()
        {
            string sql = "";

            // WebServer (Domain)
            sql = @"IF NOT EXISTS (SELECT * FROM APP.Config WHERE [Module]='APP' AND [SubModule]='IM' AND [Key]='WebServer')
                    BEGIN
                        INSERT INTO APP.Config ([Module],[SubModule],[Key], [Value], [UpdatedDate], [UpdatedBy])
                        VALUES ('APP','IM', 'WebServer', '{0}', GETDATE(), N'{1}')
                    END
                    ELSE
                    BEGIN
                        UPDATE APP.Config SET Value = '{0}', UpdatedBy= N'{1}' WHERE [Module]='APP' AND [SubModule]='IM' AND [Key]='WebServer'
                    END";
            var webServer = txt_Domain.Text.Trim().TrimEnd('/');

            bu.DbExecuteQuery(string.Format(sql, webServer, LoginInfo.LoginName), null, LoginInfo.ConnStr);

            // WebName (SubDomain)
            sql = @"IF NOT EXISTS (SELECT * FROM APP.Config WHERE [Module]='APP' AND [SubModule]='IM' AND [Key]='WebName')
                    BEGIN
                        INSERT INTO APP.Config ([Module],[SubModule],[Key], [Value], [UpdatedDate], [UpdatedBy])
                        VALUES ('APP','IM', 'WebName', '{0}', GETDATE(), N'{1}')
                    END
                    ELSE
                    BEGIN
                        UPDATE APP.Config SET Value = '{0}', UpdatedBy= N'{1}' WHERE [Module]='APP' AND [SubModule]='IM' AND [Key]='WebName'
                    END";

            var subDomain = txt_SubDomain.Text.Trim().TrimStart('/').TrimEnd('/');

            bu.DbExecuteQuery(string.Format(sql, subDomain, LoginInfo.LoginName), null, LoginInfo.ConnStr);

            //SetMode_WebServer(false);
            GetConfig_WebServer();
        }

        protected void btn_CancelSMTP_Click(object sender, EventArgs e)
        {
            SetMode_Mail(false);
            GetConfig_Mail();
            //Response.Redirect("SystemSetting.aspx");
        }

        protected void btn_TestSendMail_Click(object sender, EventArgs e)
        {
            var smtpNotification = (KeyValues)Session["smtpNotification"];



            var buCode = LoginInfo.BuInfo.BuCode;
            var connectionString = LoginInfo.ConnStr;

            var dtApi = config.DbExecuteQuery("SELECT TOP(1) ClientID FROM [dbo].[BuAPI] WHERE AppName='CARMEN' AND BuCode=@BuCode", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@BuCode", buCode) }, sys_db);

            if (dtApi != null && dtApi.Rows.Count > 0)
            {

                var dtConfig = config.DbExecuteQuery("SELECT RTRIM(LTRIM([Value])) FROM APP.Config WHERE [Module]='APP' AND SubModule='IM' AND [Key]='WebServer'", null, connectionString);
                var webServer = dtConfig != null && dtConfig.Rows.Count > 0 ? dtConfig.Rows[0][0].ToString() : "";
                var url = webServer.Trim().TrimEnd('/') + "/blueledgers.api";
                var token = dtApi.Rows[0][0].ToString();

                var api = new API(url, token);

                var host = txt_ServerName.Text.Trim();
                var port = Convert.ToInt32(txt_Port.Text);
                var useSsl = check_SSL.Checked;
                var useAuthen = check_Authen.Checked;
                var username = txt_Username.Text.Trim();
                var password = txt_Password.Text.Trim();

                if (string.IsNullOrEmpty(password))
                {
                    password = smtpNotification.Value("password");
                }


                var setting = new
                {
                    host,
                    port,
                    useSsl,
                    useAuthen,
                    username,
                    password
                };


                var from = username;
                var to = txt_TestSendMail.Text.Trim();
                var subject = string.Format("Test sending mail from Blueledgers ({0}) at {1}", LoginInfo.BuInfo.BuCode, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                var body = string.Format("This is testing to send email from '{0} : {1} at {2}'.", LoginInfo.BuInfo.BuCode, LoginInfo.BuInfo.BuName, url); ;

                var email = new
                {
                    setting,
                    from,
                    to,
                    subject,
                    body
                };

                var data = JsonConvert.SerializeObject(email);


                try
                {
                    var result = api.Post("api/mail/Send", data);

                    result = string.IsNullOrEmpty(result) ? "Mail sent." : result;

                    lbl_TestSendMail.ForeColor = Color.Blue;
                    lbl_TestSendMail.Text = result + string.Format("<br/><small>{0}</small>", DateTime.Now);
                }
                catch (Exception ex)
                {
                    lbl_TestSendMail.ForeColor = Color.Red;
                    lbl_TestSendMail.Text = ex.Message + string.Format("<br/><small>{0}</small>", DateTime.Now);
                }
            }

            //KeyValues smtpNotification = new KeyValues();
            //smtpNotification = (KeyValues)Session["smtpNotification"];

            //if (txt_TestReceiver.Text != string.Empty)
            //{
            //    Mail email = new Mail();

            //    email.SmtpServer = txt_ServerName.Text;
            //    email.Port = Convert.ToInt16(txt_Port.Text);
            //    email.EnableSsl = check_SSL.Checked;

            //    email.IsAuthentication = check_Authen.Checked;
            //    if (check_Authen.Checked)
            //    {
            //        email.Name = txt_Name.Text;
            //        email.UserName = txt_Username.Text;
            //        if (txt_Password.Text == string.Empty)
            //            email.Password = smtpNotification.Value("password");
            //        else
            //            email.Password = txt_Password.Text;
            //    }

            //    email.From = txt_Username.Text;
            //    email.To = txt_TestReceiver.Text;
            //    email.Subject = string.Format("Test sending mail from Blueledgers ({0}) at {1}", LoginInfo.BuInfo.BuCode, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            //    email.Body = string.Format("This is testing to send email from '{0} : {1}'.", LoginInfo.BuInfo.BuCode, LoginInfo.BuInfo.BuName);

            //    var error = email.Send();

            //    lbl_TestReceiver.Text = string.IsNullOrEmpty(error) ? "Mail sent." : error;
            //}
        }

        protected void btn_TestMailPo_Click(object sender, EventArgs e)
        {
            var receiver = txt_TestMailPo.Text.Trim();


            if (!string.IsNullOrEmpty(receiver))
            {

                var fileBase64 = "JVBERi0xLjcNCiW1tbW1DQoxIDAgb2JqDQo8PC9UeXBlL0NhdGFsb2cvUGFnZXMgMiAwIFIvTGFuZyhlbikgL1N0cnVjdFRyZWVSb290IDE1IDAgUi9NYXJrSW5mbzw8L01hcmtlZCB0cnVlPj4vTWV0YWRhdGEgMjcgMCBSL1ZpZXdlclByZWZlcmVuY2VzIDI4IDAgUj4+DQplbmRvYmoNCjIgMCBvYmoNCjw8L1R5cGUvUGFnZXMvQ291bnQgMS9LaWRzWyAzIDAgUl0gPj4NCmVuZG9iag0KMyAwIG9iag0KPDwvVHlwZS9QYWdlL1BhcmVudCAyIDAgUi9SZXNvdXJjZXM8PC9Gb250PDwvRjEgNSAwIFIvRjIgMTIgMCBSPj4vRXh0R1N0YXRlPDwvR1MxMCAxMCAwIFIvR1MxMSAxMSAwIFI+Pi9Qcm9jU2V0Wy9QREYvVGV4dC9JbWFnZUIvSW1hZ2VDL0ltYWdlSV0gPj4vTWVkaWFCb3hbIDAgMCA2MTIgNzkyXSAvQ29udGVudHMgNCAwIFIvR3JvdXA8PC9UeXBlL0dyb3VwL1MvVHJhbnNwYXJlbmN5L0NTL0RldmljZVJHQj4+L1RhYnMvUy9TdHJ1Y3RQYXJlbnRzIDA+Pg0KZW5kb2JqDQo0IDAgb2JqDQo8PC9GaWx0ZXIvRmxhdGVEZWNvZGUvTGVuZ3RoIDI0OT4+DQpzdHJlYW0NCnicjZC7asRADEX7gfkHlbsB29J4Hh4wgvVjl4RsEeyQIqTYYuMqJo//h2hsF0mqTDGS0NEVulAM75cZ6ro4t7cdYHF/mSfYXefscdgzQ9O18KEV5pheJAMIXv4QDXxetXq6gVmrZtSqOBJIY3zVigRCIAgmR2MhYJV7GN+EOQ2EMH2JIExrSVt50uq5RvSWq1AjUY8UkaOk8cBOOlhyVkp0AalxnCWqapkSQsjSInfgzEpEk4YXkWUsjdtmwQ0lfOOo52pRWTG77Q0uSbBPih2SP/ILjHda9XLlg1b/tcP8tYNizKn8Ycdiwnr6Dva/lkB/buEbsFNTTQ0KZW5kc3RyZWFtDQplbmRvYmoNCjUgMCBvYmoNCjw8L1R5cGUvRm9udC9TdWJ0eXBlL1R5cGUwL0Jhc2VGb250L0JDREVFRStDYWxpYnJpL0VuY29kaW5nL0lkZW50aXR5LUgvRGVzY2VuZGFudEZvbnRzIDYgMCBSL1RvVW5pY29kZSAyMyAwIFI+Pg0KZW5kb2JqDQo2IDAgb2JqDQpbIDcgMCBSXSANCmVuZG9iag0KNyAwIG9iag0KPDwvQmFzZUZvbnQvQkNERUVFK0NhbGlicmkvU3VidHlwZS9DSURGb250VHlwZTIvVHlwZS9Gb250L0NJRFRvR0lETWFwL0lkZW50aXR5L0RXIDEwMDAvQ0lEU3lzdGVtSW5mbyA4IDAgUi9Gb250RGVzY3JpcHRvciA5IDAgUi9XIDI1IDAgUj4+DQplbmRvYmoNCjggMCBvYmoNCjw8L09yZGVyaW5nKElkZW50aXR5KSAvUmVnaXN0cnkoQWRvYmUpIC9TdXBwbGVtZW50IDA+Pg0KZW5kb2JqDQo5IDAgb2JqDQo8PC9UeXBlL0ZvbnREZXNjcmlwdG9yL0ZvbnROYW1lL0JDREVFRStDYWxpYnJpL0ZsYWdzIDMyL0l0YWxpY0FuZ2xlIDAvQXNjZW50IDc1MC9EZXNjZW50IC0yNTAvQ2FwSGVpZ2h0IDc1MC9BdmdXaWR0aCA1MjEvTWF4V2lkdGggMTc0My9Gb250V2VpZ2h0IDQwMC9YSGVpZ2h0IDI1MC9TdGVtViA1Mi9Gb250QkJveFsgLTUwMyAtMjUwIDEyNDAgNzUwXSAvRm9udEZpbGUyIDI0IDAgUj4+DQplbmRvYmoNCjEwIDAgb2JqDQo8PC9UeXBlL0V4dEdTdGF0ZS9CTS9Ob3JtYWwvY2EgMT4+DQplbmRvYmoNCjExIDAgb2JqDQo8PC9UeXBlL0V4dEdTdGF0ZS9CTS9Ob3JtYWwvQ0EgMT4+DQplbmRvYmoNCjEyIDAgb2JqDQo8PC9UeXBlL0ZvbnQvU3VidHlwZS9UcnVlVHlwZS9OYW1lL0YyL0Jhc2VGb250L0JDREZFRStDYWxpYnJpL0VuY29kaW5nL1dpbkFuc2lFbmNvZGluZy9Gb250RGVzY3JpcHRvciAxMyAwIFIvRmlyc3RDaGFyIDMyL0xhc3RDaGFyIDMyL1dpZHRocyAyNiAwIFI+Pg0KZW5kb2JqDQoxMyAwIG9iag0KPDwvVHlwZS9Gb250RGVzY3JpcHRvci9Gb250TmFtZS9CQ0RGRUUrQ2FsaWJyaS9GbGFncyAzMi9JdGFsaWNBbmds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dGlvbiByZGY6YWJvdXQ9IiIgIHhtbG5zOnBkZj0iaHR0cDovL25zLmFkb2JlLmNvbS9wZGYvMS4zLyI+CjxwZGY6UHJvZHVjZXI+TWljcm9zb2Z0wq4gV29yZCAyMDE5PC9wZGY6UHJvZHVjZXI+PC9yZGY6RGVzY3JpcHRpb24+CjxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiICB4bWxuczpkYz0iaHR0cDovL3B1cmwub3JnL2RjL2VsZW1lbnRzLzEuMS8iPgo8ZGM6Y3JlYXRvcj48cmRmOlNlcT48cmRmOmxpPmFrZSBQT05HU1VQQVQ8L3JkZjpsaT48L3JkZjpTZXE+PC9kYzpjcmVhdG9yPjwvcmRmOkRlc2NyaXB0aW9uPgo8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiAgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIj4KPHhtcDpDcmVhdG9yVG9vbD5NaWNyb3NvZnTCriBXb3JkIDIwMTk8L3htcDpDcmVhdG9yVG9vbD48eG1wOkNyZWF0ZURhdGU+MjAyNS0wNy0wM1QxNDozNDoyMSswNzowMDwveG1wOkNyZWF0ZURhdGU+PHhtcDpNb2RpZnlEYXRlPjIwMjUtMDctMDNUMTQ6MzQ6MjErMDc6MDA8L3htcDpNb2RpZnlEYXRlPjwvcmRmOkRlc2NyaXB0aW9uPgo8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiAgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iPgo8eG1wTU06RG9jdW1lbnRJRD51dWlkOjU5MDA3ODJDLUEzM0EtNDA5OS1CRjI4LTZFNEI5OTc0NjJEQzwveG1wTU06RG9jdW1lbnRJRD48eG1wTU06SW5zdGFuY2VJRD51dWlkOjU5MDA3ODJDLUEzM0EtNDA5OS1CRjI4LTZFNEI5OTc0NjJEQzwveG1wTU06SW5zdGFuY2VJRD48L3JkZjpEZXNjcmlwdGlvbj4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCjwvcmRmOlJERj48L3g6eG1wbWV0YT48P3hwYWNrZXQgZW5kPSJ3Ij8+DQplbmRzdHJlYW0NCmVuZG9iag0KMjggMCBvYmoNCjw8L0Rpc3BsYXlEb2NUaXRsZSB0cnVlPj4NCmVuZG9iag0KMjkgMCBvYmoNCjw8L1R5cGUvWFJlZi9TaXplIDI5L1dbIDEgNCAyXSAvUm9vdCAxIDAgUi9JbmZvIDE0IDAgUi9JRFs8MkM3ODAwNTkzQUEzOTk0MEJGMjg2RTRCOTk3NDYyREM+PDJDNzgwMDU5M0FBMzk5NDBCRjI4NkU0Qjk5NzQ2MkRDPl0gL0ZpbHRlci9GbGF0ZURlY29kZS9MZW5ndGggMTEwPj4NCnN0cmVhbQ0KeJwtzL0RRVAUBOC9/ofIeKMLI1GPFsQUIFWEQED85gWvCqlRhFDEtesE+yW7B7B3XcZmCjyMYiPmIG4lvmIlXiN+xJ/EToJM/Em4AI79mcMRrvCEL4x4m4HdRQPncUmSmbSFOEn3IX0N3A1OEHINCmVuZHN0cmVhbQ0KZW5kb2JqDQp4cmVmDQowIDMwDQowMDAwMDAwMDE1IDY1NTM1IGYNCjAwMDAwMDAwMTcgMDAwMDAgbg0KMDAwMDAwMDE2MyAwMDAwMCBuDQowMDAwMDAwMjE5IDAwMDAwIG4NCjAwMDAwMDA0OTcgMDAwMDAgbg0KMDAwMDAwMDgyMCAwMDAwMCBuDQowMDAwMDAwOTUwIDAwMDAwIG4NCjAwMDAwMDA5NzggMDAwMDAgbg0KMDAwMDAwMTEzNSAwMDAwMCBuDQowMDAwMDAxMjA4IDAwMDAwIG4NCjAwMDAwMDE0NDcgMDAwMDAgbg0KMDAwMDAwMTUwMSAwMDAwMCBuDQowMDAwMDAxNTU1IDAwMDAwIG4NCjAwMDAwMDE3MjQgMDAwMDAgbg0KMDAwMDAwMTk2NCAwMDAwMCBuDQowMDAwMDAwMDE2IDY1NTM1IGYNCjAwMDAwMDAwMTcgNjU1MzUgZg0KMDAwMDAwMDAxOCA2NTUzNSBmDQowMDAwMDAwMDE5IDY1NTM1IGYNCjAwMDAwMDAwMjAgNjU1MzUgZg0KMDAwMDAwMDAyMSA2NTUzNSBmDQowMDAwMDAwMDIyIDY1NTM1IGYNCjAwMDAwMDAwMDAgNjU1MzUgZg0KMDAwMDAwMjYwNyAwMDAwMCBuDQowMDAwMDAyOTg3IDAwMDAwIG4NCjAwMDAwMzEwMjEgMDAwMDAgbg0KMDAwMDAzMTIyNSAwMDAwMCBuDQowMDAwMDMxMjUyIDAwMDAwIG4NCjAwMDAwMzQzOTggMDAwMDAgbg0KMDAwMDAzNDQ0MyAwMDAwMCBuDQp0cmFpbGVyDQo8PC9TaXplIDMwL1Jvb3QgMSAwIFIvSW5mbyAxNCAwIFIvSURbPDJDNzgwMDU5M0FBMzk5NDBCRjI4NkU0Qjk5NzQ2MkRDPjwyQzc4MDA1OTNBQTM5OTQwQkYyODZFNEI5OTc0NjJEQz5dID4+DQpzdGFydHhyZWYNCjM0NzU0DQolJUVPRg0KeHJlZg0KMCAwDQp0cmFpbGVyDQo8PC9TaXplIDMwL1Jvb3QgMSAwIFIvSW5mbyAxNCAwIFIvSURbPDJDNzgwMDU5M0FBMzk5NDBCRjI4NkU0Qjk5NzQ2MkRDPjwyQzc4MDA1OTNBQTM5OTQwQkYyODZFNEI5OTc0NjJEQz5dIC9QcmV2IDM0NzU0L1hSZWZTdG0gMzQ0NDM+Pg0Kc3RhcnR4cmVmDQozNTUxMQ0KJSVFT0Y=";

                var sysConnStr = System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString();
                var dtApi = config.DbExecuteQuery("SELECT TOP(1) ClientID FROM [dbo].[BuAPI] WHERE AppName='CARMEN' AND BuCode=@BuCode", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@BuCode", LoginInfo.BuInfo.BuCode) }, sysConnStr);

                if (dtApi != null && dtApi.Rows.Count > 0)
                {
                    var connectionString = LoginInfo.ConnStr;

                    var files = new List<object>
                    {
                        new 
                        {
                            name = "test.pdf",
                            file = fileBase64
                        }
                    };
                    var data = new
                    {
                        to = receiver,
                        file = files

                    };


                    var token = dtApi.Rows[0][0].ToString();
                    var json = JsonConvert.SerializeObject(data);

                    var webServer = config.GetConfigValue("APP", "IM", "WebServer", connectionString);
                    var url = webServer.Trim().TrimEnd('/') + "/blueledgers.api";
                    var api = new API(url, token);
                    try
                    {
                        var result = api.Post("api/mail/po", json);

                        ShowAlert("Mail sent");
                    }
                    catch (Exception ex)
                    {
                        ShowAlert(ex.Message);
                    }
                }
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

            // Options
            var isAllItems = config.GetConfigValue("ADMIN", "AccountMap", "IsAll", LoginInfo.ConnStr).Trim().ToLower();
            var allowTransfer = config.GetConfigValue("ADMIN", "AccountMap", "AllowTransfer", LoginInfo.ConnStr).Trim().ToLower();

            chk_AccountMappAllItems.Checked = isAllItems == "true" || isAllItems == "1";
            chk_PostTranferToGl.Checked = allowTransfer == "true" || allowTransfer == "1";


            var host = config.GetConfigValue("APP", "IM", "WebServer", LoginInfo.ConnStr).Trim().TrimEnd('/');

            var query = string.Format("SELECT ClientID FROM dbo.BuAPI WHERE AppName='CARMEN' AND BuCode='{0}'", LoginInfo.BuInfo.BuCode);
            var dt = config.DbExecuteQuery(query, null, sys_db);

            lbl_AccountEndpoint.Text = string.Format("[POST] {0}/blueledgers.api", host);
            lbl_AccountToken.Text = dt != null && dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "Not Set";

            // POS

            query = string.Format("SELECT ClientID FROM dbo.BuAPI WHERE AppName='POS' AND BuCode='{0}'", LoginInfo.BuInfo.BuCode);
            dt = config.DbExecuteQuery(query, null, sys_db);

            lbl_PosEndpoint.Text = string.Format("[POST] {0}/blueledgers.api/api/interface/pos/sale", host);
            lbl_PosToken.Text = dt != null && dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "Not Set";




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
            string host = txt_InterfaceServer.Text.Trim().TrimEnd('/') + "/";
            string auth = txt_InterfaceToken.Text;
            string accountcode = txt_InterfaceAccCode.Text.Trim().TrimEnd('/');
            string department = txt_InterfaceDepCode.Text.Trim().TrimEnd('/');
            string vendor = txt_InterfaceVendor.Text.Trim().TrimEnd('/');

            string intf = string.Format("type=API; host={0}; auth={1}; accountcode={2}; department={3}; vendor={4};", host, auth, accountcode, department, vendor);

            config.SetConfigValue("APP", "INTF", "ACCOUNT", intf, LoginInfo.ConnStr);



            var isAllItems = chk_AccountMappAllItems.Checked;
            var allowTransfer = chk_PostTranferToGl.Checked;

            config.SetConfigValue("ADMIN", "AccountMap", "IsAll", isAllItems.ToString(), LoginInfo.ConnStr);
            config.SetConfigValue("ADMIN", "AccountMap", "AllowTransfer", allowTransfer.ToString(), LoginInfo.ConnStr);


            SetMode_Interface(false);
            GetConfig_Interface();
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
        #region File Management

        protected void btn_FileManage_Click(object sender, EventArgs e)
        {

            Response.Redirect("Files.aspx");
        }


        #endregion


        // ---------------------------------------------------------------------
        #region Web Server

        //private void SetMode_WebServer(bool isEdit)
        //{
        //    btn_WebServerEdit.Visible = !isEdit;
        //    btn_WebServerSave.Visible = isEdit;
        //    btn_WebServerCancel.Visible = isEdit;

        //    Panel_WebServer.Enabled = isEdit;
        //}

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
            //SetMode_WebServer(true);
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

            //SetMode_WebServer(false);
            GetConfig_WebServer();
        }

        protected void btn_WebServerCancel_Click(object sender, EventArgs e)
        {
            GetConfig_WebServer();
            //SetMode_WebServer(false);
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
            txt_SystemPrice.Enabled = isEdit;
            txt_SystemDigitAmt.Enabled = false;
            txt_SystemDigitQty.Enabled = false;
            txt_SystemCost.Enabled = false;
            chk_EnableEditCommit.Enabled = isEdit;
            chk_UseDeliveryDateForNonMarketList.Enabled = isEdit;
            chk_UseLastPriceForNewPr.Enabled = isEdit;
        }

        private void GetConfig_System()
        {
            string currency = config.GetValue("APP", "BU", "DefaultCurrency", LoginInfo.ConnStr);

            string taxRate = config.GetValue("APP", "Default", "TaxRate", LoginInfo.ConnStr);
            string svcRate = config.GetValue("APP", "Default", "SvcRate", LoginInfo.ConnStr);

            string digitPrice = config.GetValue("APP", "Default", "DigitPrice", LoginInfo.ConnStr);
            string digitAmt = config.GetValue("APP", "Default", "DigitAmt", LoginInfo.ConnStr);
            string digitQty = config.GetValue("APP", "Default", "DigitQty", LoginInfo.ConnStr);

            string cost = config.GetValue("IN", "SYS", "COST", LoginInfo.ConnStr);
            string enableEditCommit = config.GetValue("APP", "SYS", "EnableEditCommit", LoginInfo.ConnStr).Trim();
            string useDelivereyDateForNonMarketList = config.GetValue("PC", "PR", "UseDeliveryDateForNonMarketList", LoginInfo.ConnStr).Trim();
            string useLastPriceForNewPr = config.GetValue("PC", "PR", "ApplyLastPrice", LoginInfo.ConnStr).Trim();


            txt_SystemCurrency.Text = currency;
            txt_SystemTaxRate.Text = taxRate;
            txt_SystemSvcRate.Text = svcRate;
            txt_SystemPrice.Text = string.IsNullOrEmpty(digitPrice) ? "3" : digitPrice;
            txt_SystemDigitAmt.Text = digitAmt;
            txt_SystemDigitQty.Text = digitQty;

            txt_SystemCost.Text = cost.ToUpper() == "FIFO" ? "FIFO" : "Average";
            chk_EnableEditCommit.Checked = enableEditCommit == "1";
            chk_UseDeliveryDateForNonMarketList.Checked = useDelivereyDateForNonMarketList.ToLower() == "true" || useDelivereyDateForNonMarketList == "1";
            chk_UseLastPriceForNewPr.Checked = useLastPriceForNewPr.ToLower() == "true" || useLastPriceForNewPr == "1";
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

            if (!Decimal.TryParse(txt_SystemPrice.Text, out value))
            {
                lbl_Alert.Text = "Price digit is invalid value.";
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

            // Price
            sql = @"IF NOT EXISTS (SELECT * FROM APP.Config WHERE [Module]='APP' AND [SubModule]='Default' AND [Key]='DigitPrice')
                    BEGIN
                        INSERT INTO APP.Config ([Module],[SubModule],[Key], [Value], [UpdatedDate], [UpdatedBy])
                        VALUES ('APP','Default', 'DigitPrice', '{0}', GETDATE(), '{1}')
                    END
                    ELSE
                    BEGIN
                        UPDATE APP.Config SET Value = '{0}', UpdatedBy= N'{1}' WHERE [Module]='APP' AND [SubModule]='Default' AND [Key]='DigitPrice'
                    END";

            bu.DbExecuteQuery(string.Format(sql, txt_SystemPrice.Text, LoginInfo.LoginName), null, LoginInfo.ConnStr);


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

            // Use Delivery date for non-marketlist
            sql = @"IF NOT EXISTS (SELECT * FROM APP.Config WHERE [Module]='PC' AND [SubModule]='PR' AND [Key]='UseDeliveryDateForNonMarketList')
                    BEGIN
                        INSERT INTO APP.Config ([Module],[SubModule],[Key], [Value], [UpdatedDate], [UpdatedBy])
                        VALUES ('PC','PR', 'UseDeliveryDateForNonMarketList', '{0}', GETDATE(), '{1}')
                    END
                    ELSE
                    BEGIN
                        UPDATE APP.Config SET Value = '{0}', UpdatedBy= N'{1}' WHERE [Module]='PC' AND [SubModule]='PR' AND [Key]='UseDeliveryDateForNonMarketList'
                    END";

            bu.DbExecuteQuery(string.Format(sql, chk_UseDeliveryDateForNonMarketList.Checked ? "1" : "0", LoginInfo.LoginName), null, LoginInfo.ConnStr);

            // Use Last price for new PR
            sql = @"IF NOT EXISTS (SELECT * FROM APP.Config WHERE [Module]='PC' AND [SubModule]='PR' AND [Key]='ApplyLastPrice')
                    BEGIN
                        INSERT INTO APP.Config ([Module],[SubModule],[Key], [Value], [UpdatedDate], [UpdatedBy])
                        VALUES ('PC','PR', 'ApplyLastPrice', '{0}', GETDATE(), '{1}')
                    END
                    ELSE
                    BEGIN
                        UPDATE APP.Config SET Value = '{0}', UpdatedBy= N'{1}' WHERE [Module]='PC' AND [SubModule]='PR' AND [Key]='ApplyLastPrice'
                    END";

            bu.DbExecuteQuery(string.Format(sql, chk_UseLastPriceForNewPr.Checked ? "1" : "0", LoginInfo.LoginName), null, LoginInfo.ConnStr);



            SetMode_System(false);
            GetConfig_System();
        }

        protected void btn_CancelSystem_Click(object sender, EventArgs e)
        {
            GetConfig_System();
            SetMode_System(false);
        }

        #endregion


        private void ShowAlert(string text)
        {
            lbl_Alert.Text = text;
            pop_Alert.ShowOnPageLoad = true;
        }
    }


}