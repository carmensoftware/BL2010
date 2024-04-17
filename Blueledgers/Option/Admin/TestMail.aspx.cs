using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Net.Mail;
using System.Net;


namespace BlueLedger.PL.Option.Admin
{
    public partial class TestMail : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_TestSendMail_Click(object sender, EventArgs e)
        {
            var from = txt_Username.Text;
            var to = txt_TestReceiver.Text;
            var server = txt_ServerName.Text;
            var port = Convert.ToInt16(txt_Port.Text);
            var username = txt_Username.Text;
            var password = txt_Password.Text;
            var useSsl = check_SSL.Checked;

            var mail = new MailMessage();
            
            mail.From = new System.Net.Mail.MailAddress(from);
            mail.To.Add(to);
            mail.Subject = "Test email configuration";
            mail.Body = "<h3>Testing email configuration</h3>";
            mail.IsBodyHtml = true;
            //mail.Attachments.Add(new Attachment("C:\\file.zip"));

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls; 
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            using (SmtpClient smtp = new SmtpClient(server, port))
            {
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.TargetName = "STARTTLS/" + server;

                smtp.Credentials = new NetworkCredential(username, password);
                smtp.EnableSsl = useSsl;

                try
                {
                    smtp.Send(mail);
                    lbl_TestReceiver.Text = "Mail sent.";
                    lbl_TestReceiver.ForeColor = System.Drawing.Color.DarkGreen;

                }
                catch (Exception ex)
                {
                    //lbl_TestReceiver.Text = ex.InnerException.Message;
                    //lbl_TestReceiver.ForeColor = System.Drawing.Color.Red;
                    throw new Exception(ex.Message, ex.InnerException);

                }


                lbl_TestReceiver.ToolTip = lbl_TestReceiver.Text;

            }
        }

    }
}
