using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using BlueLedger.PL.BaseClass;

/// <summary>
/// Summary description for ClassPcTool
/// </summary>
public class ClassLogTool : BasePage
{
    public ClassLogTool()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string SaveActionLog(string module, string docId, string actionStr)
    {
        string cmdStr = string.Format(@"INSERT INTO [PC].[Log]
                ([Module], [DocumentId], [Action], [ActionBy], [ActionTime])
                VALUES ( @module, @docId, @actionStr, @by, @dateTime );");
        SqlConnection conn = new SqlConnection(LoginInfo.ConnStr);
        try
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.AddWithValue("@module", module);
            cmd.Parameters.AddWithValue("@docId", docId);
            cmd.Parameters.AddWithValue("@actionStr", actionStr);
            cmd.Parameters.AddWithValue("@by", LoginInfo.LoginName);
            cmd.Parameters.AddWithValue("@dateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            cmd.ExecuteNonQuery();
            return string.Empty;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            conn.Close();
        }
    }

    public string DefragmentActionLog(int years)
    {
        if (years < 1) return string.Format("{0}", "Cann't less than 1 year.");

        string cmdStr = string.Format(@"DELETE [PC].[Log]
            WHERE [ActionTime] < DATEADD(YEAR, @years, GETDATE()) ");
        SqlConnection conn = new SqlConnection(LoginInfo.ConnStr);
        try
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.AddWithValue("@years", System.Math.Abs(years) * -1);
            cmd.ExecuteNonQuery();
            return string.Empty;
        }
        catch (Exception ex)
        {
            return string.Format("{0}", ex.Message);
        }
        finally
        {
            conn.Close();
        }
    }
}