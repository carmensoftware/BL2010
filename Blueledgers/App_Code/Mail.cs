using System;
using System.ComponentModel;
using BlueLedger.PL.BaseClass;
using Newtonsoft.Json;


/// <summary>
/// Summary description for Mail
/// </summary>
public class Mail: BasePage
{
    private readonly Blue.BL.APP.Config _config = new Blue.BL.APP.Config();


    // smtpClient
    public string SmtpServer;
    public bool IsAuthentication;
    public string UserName;
    public string Password;
    public bool EnableSsl;
    public int Port;

    // Mail
    public string Encoding;
    public string From;
    public string Name;
    public string To;
    public string CC;
    public string Subject;
    public string Body;

    // Document
    public string DocumentNo;

    public Mail()
    {
        IsAuthentication = false;
        EnableSsl = false;
        Port = 587;
        Encoding = "UTF-8";
        CC = string.Empty;
    }

    public string Send()
    {
        try
        {
            var sysConnStr = System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString();

            var dtApi = _config.DbExecuteQuery("SELECT TOP(1) ClientID FROM [dbo].[BuAPI] WHERE AppName='CARMEN' AND BuCode=@BuCode", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@BuCode", LoginInfo.BuInfo.BuCode) }, sysConnStr);

            if (dtApi != null && dtApi.Rows.Count > 0)
            {
                var setting = new
                {
                    Host = SmtpServer,
                    Port = Port,
                    UseSsl = EnableSsl,
                    UseAuthen = IsAuthentication,
                    Username = UserName,
                    Password = Password
                };


                var data = new
                {
                    Setting = setting,
                    DocNo = string.IsNullOrEmpty(DocumentNo) ? "" : DocumentNo,
                    From =Name,
                    To = To,
                    Cc = CC,
                    Subject = Subject,
                    Body = Body
                };



                var token = dtApi.Rows[0][0].ToString();
                var json = JsonConvert.SerializeObject(data);

                var webServer = _config.GetConfigValue("APP", "IM", "WebServer", LoginInfo.ConnStr);
                var url = webServer.Trim().TrimEnd('/') + "/blueledgers.api";

                var api = new API(url, token);

                var result = api.Post("api/mail/send", json);

                return result;
            }
            else
                return "Invalid token.";


        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }



    //public string Send()
    //{
    //    string errorMsg = string.Empty;
    //    string sender = string.Empty;
    //    if (IsAuthentication)
    //        sender = Name + "<" + UserName + ">";
    //    else
    //        sender = From;

    //    try
    //    {
    //        MailMessage mailMsg = new MailMessage();
    //        mailMsg.From = new MailAddress(sender);

    //        string[] to = To.Split(';');
    //        foreach (string eachTo in to)
    //        {
    //            if (eachTo != string.Empty)
    //                mailMsg.To.Add(new MailAddress(eachTo));
    //        }

    //        if (CC != string.Empty)
    //        {
    //            MailAddress sentCC = new MailAddress(CC);
    //            mailMsg.CC.Add(sentCC);
    //        }

    //        mailMsg.Subject = Subject;
    //        mailMsg.SubjectEncoding = System.Text.Encoding.GetEncoding(Encoding);
    //        mailMsg.Body = Body;
    //        mailMsg.BodyEncoding = System.Text.Encoding.GetEncoding(Encoding);
    //        mailMsg.IsBodyHtml = true;

    //        SmtpClient smtpClient = new SmtpClient();
    //        smtpClient.Host = SmtpServer;
    //        if (IsAuthentication)
    //            smtpClient.Credentials = new System.Net.NetworkCredential(UserName, Password);

    //        smtpClient.EnableSsl = EnableSsl;
    //        smtpClient.Port = Port;
    //        smtpClient.Send(mailMsg);
    //        //smtpClient.SendCompleted += new SendCompletedEventHandler(OnSendCompletedCallback);
    //        //smtpClient.SendAsync(mailMsg, null);
    //    }
    //    catch (Exception ex)
    //    {
    //        return ex.Message;
    //    }

    //    return errorMsg;
    //}

    private void OnSendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
        // Handle the callback if you need to do anything after the email is sent.
    }
}