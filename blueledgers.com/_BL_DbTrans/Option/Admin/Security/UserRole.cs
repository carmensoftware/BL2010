using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    /// <summary>
    /// Get User's Role data by LoginName.
    /// </summary>
    /// <param name="LoginName"></param>
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void ADMIN_UserRole_GetList_LoginName(SqlString LoginName)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM [ADMIN].[UserRole] WHERE LoginName=@LoginName";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@LoginName", LoginName));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void ADMIN_UserRole_GetList_RoleName(SqlString RoleName)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM [ADMIN].[UserRole] WHERE RoleName=@RoleName";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@RoleName", RoleName));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }    

    /// <summary>
    /// Get User's Role Table Schema for create new User.
    /// </summary>
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void ADMIN_UserRole_GetSchema()
    { 
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm   = "SELECT * FROM [ADMIN].[UserRole]";
            SqlCommand command  = new SqlCommand(selectComm, connection);
            SqlDataReader r = command.ExecuteReader(CommandBehavior.SchemaOnly);
            SqlContext.Pipe.Send(r);
        }
    }
};
