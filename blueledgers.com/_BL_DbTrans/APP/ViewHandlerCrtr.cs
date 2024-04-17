using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_ViewHandlerCrtr_GetList_ViewNo(SqlInt32 ViewNo)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM [APP].[VIEWHANDLERCRTR] " +
                "WHERE ViewNo=@ViewNo";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@ViewNo", ViewNo));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    /// <summary>
    /// Get ViewHandler table schema
    /// </summary>
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_ViewHandlerCrtr_GetSchema()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM [APP].[VIEWHANDLERCRTR]";
            SqlCommand command = new SqlCommand(selectComm, connection);
            SqlDataReader r = command.ExecuteReader(CommandBehavior.SchemaOnly);
            SqlContext.Pipe.Send(r);
        }
    }

    /// <summary>
    /// Get ViewHandler by field IsAsking
    /// </summary>
    /// <param name="isAsking"></param>
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_ViewHandlerCrtr_GetList_IsAsking(SqlInt32 ViewNo)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT	v.*, f.FieldTypeCode " +
                                "FROM [APP].[VIEWHANDLERCRTR] v inner join [APP].Field f on f.fieldName = v.fieldName " +
                                "WHERE v.isAsking = 'true' and ViewNo = @ViewNo";

            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@ViewNo", ViewNo));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
    
};
