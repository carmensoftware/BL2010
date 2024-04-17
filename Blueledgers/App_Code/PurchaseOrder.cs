using System;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using BlueLedger.PL.BaseClass;
using FastReport;
using FastReport.Export;
using FastReport.Utils;
using Blue.BL;

namespace BlueLedger.PL.PC.PO
{
    public partial class PoReportMail : BasePage
    {
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private readonly Blue.DAL.DbHandler db = new Blue.DAL.DbHandler();
        private string _poNo = string.Empty;


        private string GetParseText(DataTable dtPo, string parseText)
        {
            // PoNo
            parseText = Regex.Replace(parseText, "{PoNo}", dtPo.Rows[0]["PoNo"].ToString(), RegexOptions.IgnoreCase);
            // VendorName
            parseText = Regex.Replace(parseText, "{VendorName}", dtPo.Rows[0]["VendorName"].ToString(), RegexOptions.IgnoreCase);
            // PoDate
            parseText = Regex.Replace(parseText, "{PoDate}", dtPo.Rows[0]["PoDate"].ToString(), RegexOptions.IgnoreCase);


            return parseText;
        }

        protected void report1_StartReport(object sender, EventArgs e)
        {
            //string id = Request.Params["ID"].ToString();
            string id = _poNo;
            (sender as FastReport.Report).Report.SetParameterValue("ID", id);

            for (var i = 0; i < (sender as FastReport.Report).Report.Dictionary.Connections.Count; i++)
                (sender as FastReport.Report).Report.Dictionary.Connections[0].ConnectionString = LoginInfo.ConnStr;


        }

        public string SendEmailToSupplier(string poNo, string mailVendor, string connectionString)
        {
            _poNo = poNo;
            mailVendor = mailVendor.Trim();
            if (mailVendor != string.Empty)
            {
                string sql = string.Format("SELECT po.*, v.Name as VendorName FROM PC.PO po LEFT JOIN AP.Vendor v ON v.VendorCode = po.Vendor WHERE PoNo = '{0}'", poNo);
                DataTable dtPo = db.DbExecuteQuery(sql, null, connectionString);

                KeyValues mailConfig = new KeyValues();
                string encryptConfig = config.GetValue("SYS", "Mail", "PO", connectionString);
                mailConfig.Text = Blue.BL.GnxLib.EnDecryptString(encryptConfig, Blue.BL.GnxLib.EnDeCryptor.DeCrypt);

                if (mailConfig.Text != string.Empty)
                {
                    sql = "SELECT TOP(1) RptFileName FROM RPT.Report2 WHERE RptID = '9002'";
                    DataTable dtReport = db.DbExecuteQuery(sql, null, connectionString);

                    string reportName = dtReport.Rows[0]["RptFileName"].ToString();
                    string reportFile = System.Web.HttpRuntime.AppDomainAppPath + "App_Files//Reports//" + reportName;

                    Config.ReportSettings.ShowProgress = false; //Disable progress window
                    FastReport.Report report1 = new FastReport.Report();
                    report1.StartReport += report1_StartReport; // Add event to report1

                    report1.Load(reportFile); //Load report
                    report1.Prepare(); //Prepare report
                    FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport(); //Cteate PDF export
                    FastReport.Export.Email.EmailExport email = new FastReport.Export.Email.EmailExport(); //Create Email export

                    email.Account.Host = mailConfig.Value("smtp");
                    email.Account.Port = Convert.ToInt32(mailConfig.Value("port"));
                    email.Account.EnableSSL = Convert.ToBoolean(mailConfig.Value("enablessl"));

                    if (Convert.ToBoolean(mailConfig.Value("authenticate")))
                    {
                        email.Account.UserName = mailConfig.Value("username");
                        email.Account.Password = mailConfig.Value("password");
                    }

                    //email.Account.MessageTemplate = "Test";
                    email.Account.Name = mailConfig.Value("name"); ;
                    email.Account.Address = mailConfig.Value("username");

                    email.Address = mailVendor; //txt_Pop_SendMail_Email.Text;
                    //string[] cc = new string[] { "iamsupat@hotmail.com", "it.genex@hotmail.com" };
                    //email.CC = cc;

                    //string cc = mailConfig.Value("cc").Replace(',', ';').Trim();
                    //if (cc != string.Empty && cc[cc.Length - 1] != ';')
                    //    cc += ";";
                    string cc = mailConfig.Value("cc").Trim();

                    if (cc != string.Empty)
                    {
                        cc = cc.Replace(',', ';').ToString();
                    }

                    if (mailConfig.Value("ccHOD").ToUpper() == "TRUE")
                    {
                        if (cc != string.Empty && cc[cc.Length - 1] != ';')
                            cc = cc + ";";
                        cc = cc + GetHodEmail(poNo, connectionString);
                    }

                    if (cc != string.Empty)
                        email.CC = cc.Split(';');

                    email.Subject = GetParseText(dtPo, mailConfig.Value("subject"));
                    email.MessageBody = GetParseText(dtPo, mailConfig.Value("messagebody"));

                    email.Export = pdf; //Set export type
                    report1.FileName = poNo + ".pdf";
                    try
                    {
                        email.SendEmail(report1);
                        SendEMailToCreator(poNo, connectionString);
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }



                    return string.Empty;
                }
                else
                    return "Please config mail server for Purchase Order";
            }
            else
                return "Vendor email not found, please specific email.";
        }


        public void SendEMailToCreator(string poNo, string connectionString)
        {
            var error = new List<string>();
            Mail email = new Mail();

            string encryptSMTP = config.GetValue("SYS", "Mail", "ServerString", LoginInfo.ConnStr);
            KeyValues smtpConfig = new KeyValues();
            smtpConfig.Text = GnxLib.EnDecryptString(encryptSMTP, GnxLib.EnDeCryptor.DeCrypt);

            string sql = "SELECT pr.PrNo, pr.PrDate, pr.[Description], pr.CreatedBy, u.Email";
            sql += " FROM PC.PRDt prdt";
            sql += " LEFT JOIN PC.Pr pr ON pr.PRNo = prdt.PrNo";
            sql += " LEFT JOIN [ADMIN].vUser u ON u.LoginName COLLATE Latin1_General_CI_AS = pr.CreatedBy";
            sql += string.Format(" WHERE prdt.PoNo = '{0}'", poNo);
            sql += " GROUP BY pr.PrNo, pr.PrDate, pr.[Description], pr.CreatedBy, u.Email";
            DataTable dtReport = db.DbExecuteQuery(sql, null, connectionString);

            foreach (DataRow dr in dtReport.Rows)
            {
                email.SmtpServer = smtpConfig.Value("smtp");
                email.Port = Convert.ToInt16(smtpConfig.Value("port"));
                email.EnableSsl = smtpConfig.Value("enablessl").ToUpper() == "TRUE";
                email.IsAuthentication = smtpConfig.Value("authenticate").ToUpper() == "TRUE";
                if (email.IsAuthentication)
                {
                    email.UserName = smtpConfig.Value("username");
                    email.Password = smtpConfig.Value("password");
                }
                email.Name = smtpConfig.Value("name");
                email.From = smtpConfig.Value("username");

                string creator = dr["CreatedBy"].ToString();
                string prNo = dr["PrNo"].ToString();
                DateTime prDate = (DateTime)dr["PrDate"];
                string prDesc = dr["Description"].ToString();

                email.To = dr["Email"].ToString();
                email.Subject = string.Format("Notification: PR# {0} has been created to PO# {1}", prNo, prNo);

                email.Body = string.Format("Dear {0},\n\tPR# {1} ({2}) has been created to PO# {3}", creator, prNo, prDesc, poNo);

                email.Send();
            }
        }

        public string GetVendorEmail(string vendorCode, string connectionString)
        {
            string vendorEmail = string.Empty;
            string sql = string.Format("SELECT ISNULL(Address6,'') email FROM AP.Vendor v LEFT JOIN [Profile].[Address] a on a.ProfileCode = v.ProfileCode WHERE v.VendorCode = '{0}'", vendorCode);
            DataTable dt = db.DbExecuteQuery(sql, null, connectionString);
            if (dt.Rows.Count > 0)
                vendorEmail = dt.Rows[0]["email"].ToString();
            return vendorEmail;
        }

        //public string GetCcEmail(string connectionString)
        //{
        //    string cc = string.Empty;


        //    return cc;
        //}

        private string GetHodEmail(string poNo, string connectionString)
        {
            string email = string.Empty;
            string sql = string.Format(@"   SELECT 
	                                            DISTINCT hod.Email
                                            FROM 
	                                            PC.PoDt podt
	                                            JOIN PC.PrDt prdt
		                                            ON prdt.PONo = podt.PoNo AND prdt.PODtNo = podt.PoDt
	                                            JOIN PC.Pr pr
		                                            ON pr.PRNo = prdt.PRNo
	                                            JOIN [ADMIN].vHeadOfDepartment hod
		                                            ON hod.DepCode = pr.HOD
                                            WHERE podt.PoNo = '{0}'", poNo);
            DataTable dt = db.DbExecuteQuery(sql, null, connectionString);
            if (dt.Rows.Count > 0)
            {
                List<string> list = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString());
                }

                email = string.Join(";", list);
            }

            return email;
        }



    }


}