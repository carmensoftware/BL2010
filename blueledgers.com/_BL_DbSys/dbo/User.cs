using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    /// <summary>
    /// Check User login.
    /// </summary>
    /// <param name="LoginName"></param>
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void User_CheckLogin(SqlString LoginName)
    {
        using(SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[User] WHERE LoginName=@LoginName", connection);
            command.Parameters.Add(new SqlParameter("@LoginName", LoginName));            

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r); 
        }
    }

    /// <summary>
    /// Get all User by BuCode
    /// </summary>
    /// <param name="BuCode"></param>
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void User_GetList_BuCode(SqlString BuCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            SqlCommand command = new SqlCommand("SELECT u.* FROM dbo.[User] u, dbo.BuUser bu " +	
                "WHERE u.LoginName=bu.LoginName AND bu.BuCode=@BuCode", connection);
            command.Parameters.Add(new SqlParameter("@BuCode", BuCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    /// <summary>
    /// Get User Table Structure
    /// </summary>
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void User_GetSchema()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            SqlCommand command = new SqlCommand("SELECT * FROM dbo.[User]", connection);           

            SqlDataReader r = command.ExecuteReader(CommandBehavior.SchemaOnly);
            SqlContext.Pipe.Send(r);
        }
    }

    /// <summary>
    /// Gets User Data by LoginName.
    /// </summary>
    /// <param name="LoginName"></param>
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void User_Get_LoginName(SqlString LoginName)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            SqlCommand command = new SqlCommand("SELECT * FROM dbo.[User] WHERE LoginName=@LoginName", connection);
            command.Parameters.Add(new SqlParameter("@LoginName", LoginName));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void User_GetWFApproval(SqlString BuGrpCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectCmd = "SELECT DISTINCT(bu.LoginName), (SELECT ISNULL(FName,'') + ISNULL(MName,'') + ISNULL(LName,'') " +
                "FROM [User] WHERE LoginName=bu.LoginName) AS [Name] FROM BuUser bu INNER JOIN Bu b ON (bu.BuCode=b.BuCode) " +
                "WHERE b.BuGrpCode=@BuGrpCode";
            SqlCommand command = new SqlCommand(selectCmd, connection);
            command.Parameters.Add(new SqlParameter("@BuGrpCode", BuGrpCode));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }    
};
