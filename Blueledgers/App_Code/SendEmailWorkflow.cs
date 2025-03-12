using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Blue.BL;
using BlueLedger.PL.BaseClass;
using System.Data.SqlClient;
using Blue.DAL;
using System.Globalization;
using Newtonsoft.Json;

/// <summary>
/// Summary description for SendEmailWorkflow
/// </summary>
public static class SendEmailWorkflow
{
    public static DataTable DbExecuteQuery(string sql, DbParameter[] dbParameters, string connStr)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    if (dbParameters != null)
                    {
                        for (var i = 0; i < dbParameters.Length; i++)
                        {

                            cmd.Parameters.AddWithValue(dbParameters[i].ParameterName, dbParameters[i].ParameterValue);
                        }
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
                con.Close();
            }
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public static bool Send(string approveCode, string docNo, int wfId, int wfStep, string loginName, string connectionString)
    {
        var errorMessage = string.Empty;

        var code = approveCode.ToUpper();

        try
        {
            string sql = string.Format("SELECT TOP(1) InboxNo, StepTo FROM [IM].[Inbox] WHERE [RefNo]='{0}' AND [Sender]='{1}' AND StepFrom = {2} ORDER BY InboxNo DESC", docNo, loginName, wfStep);
            var dtInbox = DbExecuteQuery(sql, null, connectionString);

            if (dtInbox != null && dtInbox.Rows.Count > 0) // Found
            {
                var dr = dtInbox.Rows[0];

                var inboxNo = dr["InboxNo"].ToString();
                var stepTo = Convert.ToInt32(dr["StepTo"]);

                var mailTo = "";

                if (code == "A")
                    mailTo = Get_InvolvedLoginFromApprovals(docNo, wfId, stepTo, connectionString);
                else
                    mailTo = Get_InvolvedEmail(docNo, connectionString);

                if (!string.IsNullOrEmpty(mailTo.Trim()))
                {

                    sql = string.Format("UPDATE [IM].Inbox SET Reciever = @mailTo WHERE InboxNo = {0}", inboxNo);
                    var p = new Blue.DAL.DbParameter[] { new DbParameter("@mailTo", mailTo) };

                    DbExecuteQuery(sql, p, connectionString);

                    var sysConnStr = System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString();

                    var dtBu = DbExecuteQuery("SELECT TOP(1) RTRIM(LTRIM([BuCode])) FROM [ADMIN].Bu", null, connectionString);
                    var buCode = dtBu != null & dtBu.Rows.Count > 0 ? dtBu.Rows[0][0].ToString() : "";

                    var dtApi = DbExecuteQuery("SELECT TOP(1) ClientID FROM [dbo].[BuAPI] WHERE AppName='CARMEN' AND BuCode=@BuCode", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@BuCode", buCode) }, sysConnStr);

                    if (dtApi != null && dtApi.Rows.Count > 0)
                    {
                        var token = dtApi.Rows[0][0].ToString();

                        var dtConfig = DbExecuteQuery("SELECT RTRIM(LTRIM([Value])) FROM APP.Config WHERE [Module]='APP' AND SubModule='IM' AND [Key]='WebServer'", null, connectionString);
                        var webServer = dtConfig != null && dtConfig.Rows.Count > 0 ? dtConfig.Rows[0][0].ToString() : "";
                        var url = webServer.Trim().TrimEnd('/') + "/blueledgers.api";

                        var api = new API(url, token);

                        var result = api.Post("api/mail/inbox/" + inboxNo, "");

                        errorMessage = "";
                    }
                    else
                        errorMessage = "Invalid token.";
                }

            }
            else // not found
            {
                errorMessage = "Email does not exists.";
            }
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }


        return string.IsNullOrEmpty(errorMessage);
    }

    public static bool Send_Old(string approveCode, string docNo, int wfId, int wfStep, string loginName, string connectionString)
    {
        var errorMessage = string.Empty;

        approveCode = approveCode.ToUpper();

        // Get recent record from [IM].Inbox (After WF Stored update to IM.Inbox)
        string sql = string.Format("SELECT TOP(1) * FROM [IM].[Inbox] WHERE [RefNo]='{0}' AND [Sender]='{1}' AND StepFrom = {2} ORDER BY InboxNo DESC", docNo, loginName, wfStep);
        var dtPr = DbExecuteQuery(sql, null, connectionString);

        if (dtPr.Rows.Count > 0) // Found
        {
            DataRow dr = dtPr.Rows[0];
            string mailFrom = "", mailTo = "";
            int toStep = int.Parse(dr["StepTo"].ToString());
            string inboxNo = dr["InboxNo"].ToString();
            string refNo = dr["RefNo"].ToString();
            string subject = dr["Subject"].ToString();
            //string mailBody = GnxLib.EnDecryptString(dr["Message"].ToString(), GnxLib.EnDeCryptor.DeCrypt);
            string mailBody = dr["Message"].ToString();

            if (!mailBody.Contains('<')) // Encode
            {
                mailBody = GnxLib.EnDecryptString(dr["Message"].ToString(), GnxLib.EnDeCryptor.DeCrypt);
            }


            try
            {
                // Now: SendBack & Approve 's ["receiver"] is @email  // Test: PR17050002
                if (approveCode.ToUpper() == "A")
                    mailTo = Get_InvolvedLoginFromApprovals(docNo, wfId, toStep, connectionString);
                // Now: Reject 's ["receiver"] is LoginName
                else if (approveCode.ToUpper() == "R" || approveCode.ToUpper() == "S")
                    mailTo = Get_InvolvedEmail(docNo, connectionString);

                #region sending email

                var email = new Mail();
                var dtConfig = DbExecuteQuery("SELECT [Value] FROM [APP].[Config] WHERE Module='SYS' AND SubModule='Mail' AND [Key]='ServerString'", null, connectionString);
                ///var encryptSMTP = dtConfig != null && dtConfig.Rows.Count > 0 ? dtConfig.Rows[0]["Value"].ToString() : string.Empty;

                if (dtConfig != null && dtConfig.Rows.Count > 0)
                {
                    var encryptSMTP = dtConfig.Rows[0]["Value"].ToString();

                    var smtpConfig = new KeyValues();

                    smtpConfig.Text = GnxLib.EnDecryptString(encryptSMTP, GnxLib.EnDeCryptor.DeCrypt);

                    email.SmtpServer = smtpConfig.Value("smtp");
                    email.Port = Convert.ToInt16(smtpConfig.Value("port"));
                    email.EnableSsl = smtpConfig.Value("enablessl").ToLower() == "true";
                    email.IsAuthentication = smtpConfig.Value("authenticate").ToLower() == "true";

                    if (email.IsAuthentication)
                    {
                        email.Name = smtpConfig.Value("name");
                        email.UserName = smtpConfig.Value("username");
                        email.Password = smtpConfig.Value("password");
                    }

                    email.From = mailFrom;
                    email.To = mailTo;
                    email.Subject = subject;
                    email.Body = mailBody;

                    // Update Receiver to [IM].Inbox
                    sql = string.Format("UPDATE [IM].Inbox SET Reciever = @mailTo WHERE InboxNo = {0}", inboxNo);
                    var p = new Blue.DAL.DbParameter[1];
                    p[0] = new DbParameter("@mailTo", mailTo);

                    DbExecuteQuery(sql, p, connectionString);

                    //sql = string.Format("UPDATE [IM].Inbox SET Reciever = '{0}' WHERE InboxNo = {1}", mailTo, inboxNo);
                    //DbExecuteQuery(sql, null, connectionString);

                    errorMessage = email.Send();
                }
                else
                    errorMessage = "No smtp server is configured";
                #endregion
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.ToString();
            }

            //sql = string.Format("INSERT INTO [IM].MailLog(LogDate,InboxNo,RefNo,IsSent,Error) VALUES('{0}',{1},'{2}',{3},'{4}')",
            //    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), // LogDate
            //    inboxNo, // InboxNo
            //    refNo,  // RefNo
            //    string.IsNullOrEmpty(errorMessage) ? 1 : 0, errorMessage);
            var p1 = new List<Blue.DAL.DbParameter>();

            sql = "INSERT INTO [IM].MailLog(LogDate,InboxNo,RefNo,IsSent,Error) VALUES(@LogDate, @InboxNo, @RefNo, @IsSent, @Error)";
            p1.Add(new DbParameter("@LogDate", DateTime.Now.ToString(("yyyy-MM-dd HH:mm:ss"))));
            p1.Add(new DbParameter("@InboxNo", inboxNo));
            p1.Add(new DbParameter("@RefNo", refNo));
            p1.Add(new DbParameter("@IsSent", string.IsNullOrEmpty(errorMessage) ? "1" : "0"));
            p1.Add(new DbParameter("@Error", errorMessage));

            DbExecuteQuery(sql, p1.ToArray(), connectionString);

        }
        else // not found
        {
            errorMessage = "Not found approval emails.";
        }


        return string.IsNullOrEmpty(errorMessage);

    }

    public static void Sen_SR_Approve(string requestCode, int fromStepNo, string loginName, string connectionString)
    {

        // Send email if available.
        var dtFromStep = DbExecuteQuery("SELECT SentEmail, Approvals FROM [APP].WFDt WHERE WFId=2 AND Step=" + fromStepNo.ToString(), null, connectionString);

        if (dtFromStep == null || dtFromStep.Rows.Count == 0 || dtFromStep.Rows[0]["SentEmail"].ToString() == "0")
        {
            return;
        }

        var dtSr = DbExecuteQuery("SELECT RefId FROM [IN].StoreRequisition WHERE RequestCode='" + requestCode + "'", null, connectionString);
        var refId = Convert.ToInt32(dtSr.Rows[0]["RefId"]);


        var sql = string.Format("SELECT TOP(1) * FROM [IM].Inbox WHERE RefNo='{0}' AND StepFrom={1} AND [Sender]='{2}' ORDER BY [Date] Desc", requestCode, fromStepNo, loginName);
        var dtInbox = DbExecuteQuery(sql, null, connectionString);

        if (dtInbox == null || dtInbox.Rows.Count == 0)
            return;

        var toStepNo = Convert.ToInt32(dtInbox.Rows[0]["StepTo"]);
        var inboxNo = Convert.ToInt32(dtInbox.Rows[0]["InboxNo"]);


        var dtToStep = DbExecuteQuery("SELECT Approvals, StepDesc FROM [APP].WFDt WHERE WFId=2 AND Step=" + toStepNo.ToString(), null, connectionString);

        var approvals = dtToStep.Rows[0]["Approvals"].ToString();
        var stepName = dtToStep.Rows[0]["StepDesc"].ToString();

        var fromMail = "";
        var toMail = GetEmails(approvals, connectionString);
        var cc = "";
        var subject = string.Format("{0} is awaiting for approval ({1}).", requestCode, stepName);
        var body = GetBody_Approval(refId, toStepNo, connectionString);


        var p = new List<Blue.DAL.DbParameter>();

        p.Add(new Blue.DAL.DbParameter("@Reciever", toMail));
        p.Add(new Blue.DAL.DbParameter("@Subject", subject));
        p.Add(new Blue.DAL.DbParameter("@Message", @body));

        DbExecuteQuery("UPDATE [IM].Inbox SET [Reciever]=@Reciever, [Subject]=@Subject, [Message]=@Message WHERE InboxNo=" + inboxNo.ToString(), p.ToArray(), connectionString);


        var message = SendMail(fromMail, toMail, cc, subject, body, connectionString);

        sql = "INSERT INTO [IM].MailLog(LogDate, InboxNo, RefNo, IsSent, Error) VALUES(GETDATE(), @InboxNo, @RefNo, @IsSent, @error)";
        var p1 = new List<Blue.DAL.DbParameter>();

        p1.Add(new Blue.DAL.DbParameter("@InboxNo", inboxNo.ToString()));
        p1.Add(new Blue.DAL.DbParameter("@RefNo", requestCode));
        p1.Add(new Blue.DAL.DbParameter("IsSent", string.IsNullOrEmpty(message) ? "1" : "0"));
        p1.Add(new Blue.DAL.DbParameter("@error", message));

        DbExecuteQuery(sql, p1.ToArray(), connectionString);

    }

    // Private method(s)

    private static string ConvertLoginName_ToEmail(string strLogin, string connectionString)
    {
        string emails = string.Empty;
        if (strLogin != null)
        {
            List<string> logins = strLogin.Split(';').ToList();

            foreach (string login in logins)
            {
                string strCmd = "SELECT [Email] FROM [ADMIN].[vUser]";
                strCmd += string.Format(" WHERE [LoginName] = '{0}'", login);

                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connectionString);

                con.Open();
                SqlCommand cmd = new SqlCommand(strCmd, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                    emails += dt.Rows[0][0].ToString() + ";";
                else
                    emails += login + ";";
                con.Close();
            }
        }

        return emails;
    }

    private static string Get_InvolvedEmail(string prNo, string connectionString)
    {
        // Should alert the relevant.
        string emails = string.Empty;
        string cmdStr = "SELECT DISTINCT h.ProcessBy, u.Email";
        cmdStr += " FROM [APP].[WFHis] AS h";
        cmdStr += " LEFT JOIN [ADMIN].[vUser] AS u ON h.ProcessBy COLLATE Latin1_General_CI_AS = u.LoginName COLLATE Latin1_General_CI_AS";
        cmdStr += string.Format(" WHERE RefNo = '{0}'", prNo);

        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(connectionString);
        con.Open();

        SqlCommand cmd = new SqlCommand(cmdStr, con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        con.Close();

        foreach (DataRow dr in dt.Rows)
        {
            emails += dr["Email"].ToString() + ";";
        }
        return emails;
    }

    private static string Get_InvolvedLoginFromApprovals(string prNo, int wfId, int toStep, string connectionString)
    {
        //string prNo = Request.Params["ID"];

        #region
        string sql = string.Format(
@"DECLARE @list TABLE ( [LoginName] NVARCHAR(100), [Email] NVARCHAR(MAX) )
DECLARE @approvals NVARCHAR(MAX) = ( SELECT [Approvals] + ',' FROM [APP].[WFDt]  WHERE [WFId] = @wfId AND [Step] = @wfStep )
               
IF ISNULL((SELECT IsHOD FROM [APP].[WFDt] WHERE [WFId] = @wfId AND [Step] = @wfStep), 0) = 1
BEGIN
	DECLARE @CreateBy NVARCHAR(20) = (SELECT CreatedBy FROM PC.PR WHERE PrNo = '" + prNo + @"');
	DECLARE @DeptCode NVARCHAR(20) = (SELECT DepartmentCode FROM [ADMIN].vUser WHERE LoginName = @CreateBy)

    INSERT INTO @list ( [LoginName], Email )
    SELECT LoginName, Email FROM  [ADMIN].vHeadOfDepartment WHERE DepCode = @DeptCode
END
ELSE
BEGIN
    WHILE LEN(@approvals) > 0
    BEGIN
	    DECLARE @subAppr NVARCHAR(100) = SUBSTRING(@approvals, 1, CHARINDEX(',', @approvals) - 1)
	    IF(@subAppr LIKE '#%')
	    BEGIN
		    INSERT INTO @list ( [LoginName], Email )
		    SELECT vU.[LoginName], vU.[Email]
		    FROM [ADMIN].[vUser] AS vU 
		    LEFT JOIN @list AS l ON l.LoginName COLLATE Latin1_General_CI_AS = vU.[LoginName] COLLATE Latin1_General_CI_AS
		    WHERE vU.[LoginName] = REPLACE(@subAppr, '#', '') AND l.LoginName IS NULL 
	    END
	    ELSE
	    BEGIN
		    INSERT INTO @list ( [LoginName], Email )
		    SELECT ur.[LoginName], vU.[Email]
		    FROM [ADMIN].[UserRole] AS ur
		    LEFT JOIN @list AS l ON l.[LoginName] COLLATE Latin1_General_CI_AS = ur.[LoginName] COLLATE Latin1_General_CI_AS
            JOIN [ADMIN].[vUser] AS vU ON vU.[LoginName] COLLATE Latin1_General_CI_AS= ur.[LoginName] COLLATE Latin1_General_CI_AS
		    WHERE ur.[RoleName] = @subAppr AND ur.[IsActive] = 1 AND l.LoginName IS NULL
	    END
	
	    SET @approvals = STUFF(@approvals, 1, LEN(@subAppr)+1, '')
    END
END
SELECT * FROM @list");

        #endregion
        string login = string.Empty;
        SqlConnection con = new SqlConnection(connectionString);
        DataTable dt = new DataTable();
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@wfId", wfId);
            cmd.Parameters.AddWithValue("@wfStep", toStep);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
        }
        catch
        {
            return null;
        }

        con.Close();

        foreach (DataRow dr in dt.Rows)
        {
            login += dr["Email"].ToString() + ";";
        }
        return login;
    }


    // SR

    private static string SendMail(string fromMail, string toMail, string ccMail, string subject, string body, string connectionString)
    {
        string errorMessage = string.Empty;


        var dt = DbExecuteQuery("SELECT [Value] FROM [APP].Config WHERE Module='SYS' AND SubModule='Mail' AND [Key]='ServerString'", null, connectionString);
        var encryptSMTP = dt != null && dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "";
        var smtpConfig = new KeyValues();

        smtpConfig.Text = GnxLib.EnDecryptString(encryptSMTP, GnxLib.EnDeCryptor.DeCrypt);

        Mail email = new Mail();

        email.SmtpServer = smtpConfig.Value("smtp");
        email.Port = Convert.ToInt16(smtpConfig.Value("port"));
        email.EnableSsl = smtpConfig.Value("enablessl").ToUpper() == "TRUE";
        email.IsAuthentication = smtpConfig.Value("authenticate").ToUpper() == "TRUE";

        if (email.IsAuthentication)
        {
            email.Name = smtpConfig.Value("name");
            email.UserName = smtpConfig.Value("username");
            email.Password = smtpConfig.Value("password");
        }

        email.From = fromMail;
        email.To = toMail;
        email.CC = ccMail;
        email.Subject = subject;
        email.Body = body;

        try
        {

            return email.Send();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

    }

    private static string GetEmails(string value, string connectionString)
    {
        var userList = new List<string>();

        var items = value.Trim().TrimEnd(',').Split(',').Select(x => x.Trim());
        var roles = items.Where(x => !x.StartsWith("#"));
        var users = items.Where(x => x.StartsWith("#")).Select(x => x.Substring(1, x.Length - 1));

        var roleExpression = "'" + string.Join("','", roles) + "'";
        var dtRole = DbExecuteQuery(string.Format("SELECT DISTINCT LoginName FROM [ADMIN].[UserRole] WHERE ISActive=1 AND RoleName IN ({0})", roleExpression), null, connectionString);

        if (dtRole != null && dtRole.Rows.Count > 0)
        {
            userList.AddRange(dtRole.AsEnumerable().Select(x => x.Field<string>("LoginName")));
        }

        userList.AddRange(users);

        var userExpression = "'" + string.Join("','", userList) + "'";
        var sql = string.Format("SELECT DISTINCT Email FROM [ADMIN].vUser WHERE LoginName IN ({0})", userExpression);
        var dtEmail = DbExecuteQuery(sql, null, connectionString);

        var emails = new List<string>();

        if (dtEmail != null && dtEmail.Rows.Count > 0)
        {
            emails.AddRange(dtEmail.AsEnumerable().Select(x => x.Field<string>("Email")));
        }


        return string.Join(";", emails);

    }

    private static string GetBody_Approval(int refId, int stepNo, string connectionString)
    {
        var dtSr = DbExecuteQuery("SELECT sr.*, l.LocationName FROM [IN].StoreRequisition sr JOIN [IN].StoreLocation l ON sr.LocationCode=l.LocationCode WHERE RefId=" + refId.ToString(), null, connectionString);


        if (dtSr == null || dtSr.Rows.Count == 0)
        {
            return "";
        }

        string body = @"
<div style='font-family:Tahoma, Arial, Helvetica, sans-serif;'>
    <h2 style='color:#4285F4;'>{0}</h2>
    <h3>Store Requisition</h3>
    <h4 style='padding:10px; background-color:#4285F4;color:#fff;'>{1}</h4>
    <table>
        <tr>
            <td style='width:120px;'><b>#</b></td>
            <td>{2}</td>
        </tr>
        <tr>
            <td><b>Date</b></td>
            <td>{3}</td>
        </tr>
        <tr>
            <td><b>Description</b></td>
            <td>{4}</td>
        </tr>
        <tr>
            <td><b>Location</b></td>
            <td>{5}</td>
        </tr>
    </table>
    <br />
    <hr />
    <br />
    <a href='{6}' target='_blank' style='cursor:pointer;padding:10 15;background-color:#4285F4;color:white; text-align:center; text-decoration:none;'>View Detail</a>
</div>".Trim();

        var dtHost = DbExecuteQuery("SELECT [Value] FROM [APP].Config WHERE [Module]='APP' AND [SubModule]='IM' AND [Key]='WebServer'", null, connectionString);
        var dtApp = DbExecuteQuery("SELECT [Value] FROM [APP].Config WHERE [Module]='APP' AND [SubModule]='IM' AND [Key]='WebName'", null, connectionString);
        var dtView = DbExecuteQuery("SELECT * FROM [APP].ViewHandler WHERE WfId=2 AND WFStep=" + stepNo.ToString(), null, connectionString);


        var drSr = dtSr.Rows[0];
        var drView = dtView.Rows[0];

        var host = dtHost.Rows[0][0].ToString().TrimEnd('/');
        var app = dtApp.Rows[0][0].ToString().TrimEnd('/');

        var vdesc = drView["Desc"].ToString();
        var vid = drView["ViewNo"].ToString();


        var dtBu = DbExecuteQuery("SELECT TOP 1 * FROM [ADMIN].Bu", null, connectionString);
        var dr = dtBu.Rows[0];

        var buCode = dr["BuCode"].ToString();
        var buName = dr["Name"].ToString();
        var step = vdesc;
        var docNo = drSr["RequestCode"].ToString();
        var docDate = Convert.ToDateTime(drSr["DeliveryDate"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        var docDesc = dtSr.Rows[0]["Description"].ToString();
        var location = string.Format("{0} : {1}", drSr["LocationCode"].ToString(), drSr["LocationName"].ToString());
        var url = string.Format("{0}/{1}/IN/STOREREQ/StoreReqDt.aspx?BuCode={2}&ID={3}&VID={4}",
            host,
            app,
            buCode,
            drSr["RefId"].ToString(),
            vid);

        body = string.Format(body,
            buName,
            step,
            docNo,
            docDate,
            docDesc,
            location,
            url);

        return body;
    }




}