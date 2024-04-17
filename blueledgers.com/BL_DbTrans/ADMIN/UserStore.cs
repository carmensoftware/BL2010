using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void ADMIN_UserStore_GetList_LoginName(SqlString LoginName)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm   = "SELECT * FROM [ADMIN].[UserStore] WHERE LoginName=@LoginName";
            SqlCommand command  = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@LoginName", LoginName));
            SqlDataReader r     = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void ADMIN_UserStore_CountByLocationCode(SqlString LocateCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT count(*) FROM [ADMIN].UserStore WHERE LocationCode = @LocateCode";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@LocateCode", LocateCode));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
