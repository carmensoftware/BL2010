using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void Unit_GetNewID()
    {
        // Put your code here
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT Isnull(Max(UnitCode),0) + 1 as NewUnitCode From [IN].[Unit]";
            SqlCommand command = new SqlCommand(selectComm, connection);
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
