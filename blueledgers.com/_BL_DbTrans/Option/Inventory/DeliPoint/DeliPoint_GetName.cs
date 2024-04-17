using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void DeliPoint_GetName(int DptCode)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm  = "SELECT [Name] FROM [IN].[DeliveryPoint] WHERE Isactived = 'True' and DptCode = @DptCode";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@DptCode", DptCode));
            
            SqlDataReader r    = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
}