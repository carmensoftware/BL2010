using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    /// <summary>
    /// Get all view column related to ViewNo.
    /// </summary>
    /// <param name="PageCode"></param>
    /// <param name="LoginName"></param>
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_ViewHandlerCols_GetList_ViewNo(SqlInt32 ViewNo)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm   = "SELECT * FROM [APP].[VIEWHANDLERCOLS] " +
                "WHERE ViewNo=@ViewNo";
            SqlCommand command  = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@ViewNo", ViewNo));            
            SqlDataReader r     = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    /// <summary>
    /// Get ViewHandlerCols table schema
    /// </summary>
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_ViewHandlerCols_GetSchema()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM [APP].[VIEWHANDLERCOLS]";
            SqlCommand command = new SqlCommand(selectComm, connection);
            SqlDataReader r = command.ExecuteReader(CommandBehavior.SchemaOnly);
            SqlContext.Pipe.Send(r);
        }
    }
};
