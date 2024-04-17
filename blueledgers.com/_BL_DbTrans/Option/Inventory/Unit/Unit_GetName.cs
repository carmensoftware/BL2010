using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void Unit_GetName(SqlString UnitCode)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm  = "SELECT [Name] FROM [IN].[Unit] WHERE Isactived = 'True' and UnitCode = @UnitCode";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@UnitCode", UnitCode));
            
            SqlDataReader r    = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
}