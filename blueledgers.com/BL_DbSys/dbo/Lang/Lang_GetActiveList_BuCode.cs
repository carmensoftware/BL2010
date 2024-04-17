using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void Lang_GetActiveList_BuCode(string BuCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT l.* " +
                "FROM dbo.Lang l LEFT OUTER JOIN dbo.BuFmt bf ON (l.LangCode = bf.LangCode OR l.LangCode = bf.LangCodeOth) " +
                "WHERE bf.BuCode = @BuCode AND l.IsActived = 'True'";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@BuCode", BuCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
