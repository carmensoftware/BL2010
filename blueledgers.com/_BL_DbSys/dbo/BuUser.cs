using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void BuUser_Get_BuCode_LoginName(string BuCode, string LoginName)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM dbo.BuUser WHERE BuCode = @BuCode AND LoginName = @LoginName";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@BuCode", BuCode));
            command.Parameters.Add(new SqlParameter("@LoginName", LoginName));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void BuUser_GetList_LoginName(string LoginName)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM dbo.BuUser WHERE LoginName=@LoginName";
            SqlCommand command = new SqlCommand(selectComm, connection);            
            command.Parameters.Add(new SqlParameter("@LoginName", LoginName));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void BuUser_GetActiveList_LoginName(SqlString LoginName)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT b.*,bu.Theme, bu.DispLang, FmtCode, FmtSDate, FmtLDate, FmtSTime, FmtLTime, FmtAM, FmtPM, " +
                "FmtFirstDOW, FmtNumDec, FmtNumDecNo, FmtNumDecGrp, FmtNumNeg, FmtCurrency, FmtCurrencyDec, " +
                "FmtCurrencyDecNo, FmtCurrencyDecGrp, CountryCode, UTCCode, LangCode, LangCodeOth " +
                "FROM dbo.BuUser bu " +
                "INNER JOIN dbo.Bu b ON (b.BuCode = bu.BuCode) " +
                "LEFT OUTER JOIN dbo.BuFmt bf ON (b.BuCode = bf.BuCode) " +
                "WHERE bu.LoginName = @LoginName AND b.IsActived = 'True'";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@LoginName", LoginName));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
    
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void BuUser_GetUserNo(string BuCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT COUNT(*) AS UserNo FROM dbo.BuUser WHERE BuCode=@BuCode";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@BuCode", BuCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void BuUser_GetShema()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM dbo.BuUser";
            SqlCommand command = new SqlCommand(selectComm, connection);

            SqlDataReader r = command.ExecuteReader(CommandBehavior.SchemaOnly);
            SqlContext.Pipe.Send(r);
        }
    }
};
