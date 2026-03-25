using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Blue.DAL;
using System.Data;
using System.Data.SqlClient;

public partial class Test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {



    }


    protected void btn_Get_Click(object sender, EventArgs e)
    {
        var docNo = txt_DocNo.Text;
        var wfId = txt_WfId.Text;
        var wfStep = txt_WfStep.Text;
        var connStr = txt_ConnStr.Text;

        var approvals = txt_Approvals.Text;


        var emails = GetApprovalEmails(approvals, connStr);

        var hod = GetHodEmails("701", connStr);

        txt_Emails.Text = emails;

    }

    private  DataTable DbExecuteQuery(string sql, DbParameter[] dbParameters, string connStr)
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

    private string GetApprovalEmails2(string approvals, string connStr)
    {
        var emails = new List<string>();

        var list = approvals.Split(',').Select(x => x.Trim()).ToArray();

        var loginNames = new List<string>();
        var roles = new List<string>();


        // roles
        roles.AddRange(list.Where(x => !x.StartsWith("#")).Select(x => x.Trim()).Distinct().ToArray());

        var roleItems = roles.Select(x => string.Format("'{0}'", x));
        var sql = string.Format(@"
SELECT
	DISTINCT LoginName
FROM
	[ADMIN].UserRole
WHERE
	IsActive=1
	AND RoleName IN ({0})", string.Join(",", roleItems));
        var dtUserRole = DbExecuteQuery(sql, null, connStr);

        // loginName
        loginNames.AddRange(list.Where(x => x.StartsWith("#")).ToArray());

        if (dtUserRole != null && dtUserRole.Rows.Count > 0)
        {
            var items = dtUserRole.AsEnumerable()
                .Select(x => x.Field<string>("LoginName").Trim())
                .Distinct()
                .ToArray();

            loginNames.AddRange(items);
        }

        var parameters = new List<DbParameter>();
        var i = 1;
        sql = "SELECT Email FROM [ADMIN].vUser WHERE LoginName IN (";


        var paramNameList = new List<string>();
        foreach (var loginName in loginNames.Distinct())
        {
            var paramName = string.Format("@LoginName{0}", i++);

            paramNameList.Add(paramName);
            parameters.Add(new DbParameter(paramName, loginName));
        }

        sql += string.Join(",", paramNameList) + ")";

        var dt = DbExecuteQuery(sql, parameters.ToArray(), connStr);

        if (dt != null && dt.Rows.Count > 0)
        {
            emails.AddRange(dt.AsEnumerable()
                .Select(x=>x.Field<string>("Email"))
                .ToArray());
        }



        return string.Join(";", emails.Select(x => x.Trim()).Distinct());
    }

    private string GetApprovalEmails(string docNo, int toStep, string connStr)
    {
        var emails = new List<string>();

        var list = approvals.Split(',').Select(x => x.Trim()).ToArray();

        var loginNames = new List<string>();
        var roles = new List<string>();


        // roles
        roles.AddRange(list.Where(x => !x.StartsWith("#")).Select(x => x.Trim()).Distinct().ToArray());

        var roleItems = roles.Select(x => string.Format("'{0}'", x));
        var sql = string.Format(@"
SELECT
	DISTINCT LoginName
FROM
	[ADMIN].UserRole
WHERE
	IsActive=1
	AND RoleName IN ({0})", string.Join(",", roleItems));
        var dtUserRole = DbExecuteQuery(sql, null, connStr);

        // loginName
        loginNames.AddRange(list.Where(x => x.StartsWith("#")).ToArray());

        if (dtUserRole != null && dtUserRole.Rows.Count > 0)
        {
            var items = dtUserRole.AsEnumerable()
                .Select(x => x.Field<string>("LoginName").Trim())
                .Distinct()
                .ToArray();

            loginNames.AddRange(items);
        }

        var parameters = new List<DbParameter>();
        var i = 1;
        sql = "SELECT Email FROM [ADMIN].vUser WHERE LoginName IN (";


        var paramNameList = new List<string>();
        foreach (var loginName in loginNames.Distinct())
        {
            var paramName = string.Format("@LoginName{0}", i++);

            paramNameList.Add(paramName);
            parameters.Add(new DbParameter(paramName, loginName));
        }

        sql += string.Join(",", paramNameList) + ")";

        var dt = DbExecuteQuery(sql, parameters.ToArray(), connStr);

        if (dt != null && dt.Rows.Count > 0)
        {
            emails.AddRange(dt.AsEnumerable()
                .Select(x => x.Field<string>("Email"))
                .ToArray());
        }



        return string.Join(";", emails.Select(x => x.Trim()).Distinct());
    }


    private string GetHodEmails(string depCode, string connStr)
    {
        var emails = new List<string>();

        var sql = "SELECT DISTINCT Email FROM [ADMIN].vHeadOfDepartment WHERE DepCode=@DepCode";

        var dt = DbExecuteQuery(sql, new DbParameter[] { new DbParameter("@DepCode", depCode) }, connStr);       

        if (dt != null && dt.Rows.Count > 0)
        {
            emails.AddRange(dt.AsEnumerable()
                .Select(x => x.Field<string>("Email"))
                .ToArray());
        }



        return string.Join(";", emails.Select(x => x.Trim()).Distinct());
    }

}