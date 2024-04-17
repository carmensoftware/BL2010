using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_Module_GetList_ID(SqlString ID)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM [APP].[Module] " +
                "WHERE Parent=@ID AND IsActive='true' ORDER BY ID";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@ID", ID));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_Module_GetList_ID_LoginName(SqlString ID, SqlString LoginName)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM APP.Module " +
                "WHERE Parent=@ID AND IsActive='true' AND " +
                "ID IN (SELECT ModuleID FROM [ADMIN].RolePermission WHERE RoleName IN " +
                "(SELECT RoleName FROM [ADMIN].UserRole WHERE LoginName=@LoginName AND IsActive='true')) " +
                "ORDER BY ID";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@ID", ID));
            command.Parameters.Add(new SqlParameter("@LoginName", LoginName));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_Module_GetRoot()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM [APP].[Module] " +
                "WHERE Parent IS NULL AND IsActive='true' ORDER BY ID";
            SqlCommand command = new SqlCommand(selectComm, connection);
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_Module_GetRoot_LoginName(SqlString LoginName)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM APP.Module " +
                "WHERE Parent IS NULL AND IsActive='true' AND " +
                "ID IN (SELECT ModuleID FROM [ADMIN].RolePermission WHERE RoleName IN " +
                "(SELECT RoleName FROM [ADMIN].UserRole WHERE LoginName=@LoginName AND IsActive='true')) " +
                "ORDER BY ID";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@LoginName", LoginName));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_Module_GetActiveList()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM APP.Module WHERE IsActive='true'";
            SqlCommand command = new SqlCommand(selectComm, connection);
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
