using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.ComponentModel;
/// <summary>
/// Summary description for Mail
/// </summary>
public class Mail
{
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
        string errorMsg = string.Empty;
        string sender = string.Empty;
        if (IsAuthentication)
            sender = Name + "<" + UserName + ">";
        else
            sender = From;

        try
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress(sender);

            string[] to = To.Split(';');
            foreach (string eachTo in to)
            {
                if (eachTo != string.Empty)
                    mailMsg.To.Add(new MailAddress(eachTo));
            }

            if (CC != string.Empty)
            {
                MailAddress sentCC = new MailAddress(CC);
                mailMsg.CC.Add(sentCC);
            }

            mailMsg.Subject = Subject;
            mailMsg.SubjectEncoding = System.Text.Encoding.GetEncoding(Encoding);
            mailMsg.Body = Body;
            mailMsg.BodyEncoding = System.Text.Encoding.GetEncoding(Encoding);
            mailMsg.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = SmtpServer;
            if (IsAuthentication)
                smtpClient.Credentials = new System.Net.NetworkCredential(UserName, Password);

            smtpClient.EnableSsl = EnableSsl;
            smtpClient.Port = Port;
            smtpClient.Send(mailMsg);
            //smtpClient.SendCompleted += new SendCompletedEventHandler(OnSendCompletedCallback);
            //smtpClient.SendAsync(mailMsg, null);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

        return errorMsg;
    }

    private void OnSendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
        // Handle the callback if you need to do anything after the email is sent.
    }
}