using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{   
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_TemplateDt_GetSchema()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM [IN].TemplateDt";
            SqlCommand command = new SqlCommand(selectComm, connection);
            SqlDataReader r = command.ExecuteReader(CommandBehavior.SchemaOnly);
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void TP_GetTmpDtNewNo()
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT Isnull(Max(TmpDtNo),0) + 1 From [IN].[TemplateDt]";
            SqlCommand command = new SqlCommand(selectComm, connection);
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void TemplateDt_CountByTemplateDtNo(SqlString TemplateDt)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT COUNT(*) FROM [IN].TemplateDt WHERE TmpNo = @TemplateDt";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@TemplateDt", TemplateDt));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
