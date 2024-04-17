using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void Acc_GetName(string AccCode)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT [Desc] FROM GL.Account WHERE Isactived = 'True' and AccCode = @AccCode";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@AccCode", AccCode));
            
            SqlDataReader r    = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
}