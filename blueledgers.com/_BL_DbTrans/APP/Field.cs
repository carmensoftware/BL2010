using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_Field_Get_FieldName(SqlString FieldName)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM [APP].[FIELD] " +
                "WHERE FieldName=@FieldName";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@FieldName", FieldName));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_Field_GetList_TableName(SqlString TableName)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm   = "SELECT * FROM [APP].[FIELD] WHERE TableName=@TableName";
            SqlCommand command  = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@TableName", TableName));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    /// <summary>
    /// Get only not selected field data from APP.Field compare with APP.ViewHandlerCols by ViewNo
    /// </summary>
    /// <param name="PageCode"></param>
    /// <param name="ViewNo"></param>
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_Field_GetList_ViewAvaCols(SqlString TableName, SqlInt32 ViewNo)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm   = "SELECT * FROM [APP].[FIELD] WHERE TableName=@TableName AND " +
	            "FieldName NOT IN (SELECT FieldName FROM APP.ViewHandlerCols WHERE ViewNo=@ViewNo)";
            SqlCommand command  = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@TableName", TableName));
            command.Parameters.Add(new SqlParameter("@ViewNo", ViewNo));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    /// <summary>
    /// Get only selected field data from APP.Field compare with APP.ViewHandlerCols by ViewNo
    /// </summary>
    /// <param name="PageCode"></param>
    /// <param name="ViewNo"></param>
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_Field_GetList_ViewSelCols(SqlString TableName, SqlInt32 ViewNo)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm   = "SELECT f.FieldName, f.[Desc], f.OthDesc FROM [APP].[FIELD] f INNER JOIN [APP].[ViewHandlerCols] v " +
                "ON (f.FieldName=v.FieldName) WHERE TableName=@TableName AND v.ViewNo=@ViewNo ORDER BY v.SeqNo";
            SqlCommand command  = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@TableName", TableName));
            command.Parameters.Add(new SqlParameter("@ViewNo", ViewNo));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_Field_GetWFField(SqlString TableList)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT f.FieldName, CAST(t.Description AS NVARCHAR) + ' - ' + f.[Desc] AS [Desc], " +
                "f.[Desc] AS FieldDesc, t.Description AS Section " +
                "FROM APP.[Field] f INNER JOIN APP.[Table] t ON (f.TableName = t.TableName) " +
                "WHERE PATINDEX('%' + f.TableName + '%', @TableList) > 0 AND f.IsActived='True'";
            SqlCommand command = new SqlCommand(selectComm, connection);            
            command.Parameters.Add(new SqlParameter("@TableList", TableList));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
