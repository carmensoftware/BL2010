using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    /// <summary>
    /// Get all view related to PageCode and LoginName.
    /// </summary>
    /// <param name="PageCode"></param>
    /// <param name="LoginName"></param>
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_ViewHandler_GetList(SqlString PageCode, SqlString LoginName)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT v.ViewNo, v.PageCode, v.[Desc], v.IsPublic, v. SearchIn, v.IsAdvance, " +
                "v.AdvOpt, v.IsStandard, v.WFId, v.WFStep, v.CreatedDate, v.CreatedBy, " +
                "v.UpdatedDate, v.UpdatedBy, 'Work-Flow' AS ViewType " +
                "FROM [APP].[VIEWHANDLER] v " +
                "LEFT OUTER JOIN APP.WFDt wfdt ON (wfdt.WFId=v.WFId AND wfdt.Step=v.WFStep) " +
                "WHERE PageCode=@PageCode AND " +
                "(IsPublic='True' OR (IsPublic='False' AND CreatedBy=@LoginName)) AND " +
                "(v.WFID IS NOT NULL AND v.WFStep IS NOT NULL) AND " +
                "(wfdt.Appr1=@LoginName OR wfdt.Appr2=@LoginName OR wfdt.Appr3=@LoginName OR " +
                "wfdt.Appr4=@LoginName OR wfdt.Appr5=@LoginName OR wfdt.Appr6=@LoginName OR " +
                "wfdt.Appr7=@LoginName OR wfdt.Appr8=@LoginName OR wfdt.Appr9=@LoginName OR wfdt.Appr10=@LoginName OR " +
                "(SELECT COUNT(*) FROM Admin.[UserRole] WHERE LoginName=@LoginName AND UPPER(RoleName)='ADMINISTRATORS' AND IsActive='true') > 0) " +
                "UNION " +
                "SELECT v.ViewNo, v.PageCode, v.[Desc], v.IsPublic, v. SearchIn, v.IsAdvance, " +
                "v.AdvOpt, v.IsStandard, v.WFId, v.WFStep, v.CreatedDate, v.CreatedBy, " +
                "v.UpdatedDate, v.UpdatedBy, 'Work-Flow' AS ViewType " +
                "FROM [APP].[VIEWHANDLER] v " +
                "LEFT OUTER JOIN APP.WFDt wfdt ON (wfdt.WFId=v.WFId AND wfdt.Step=v.WFStep) " +
                "WHERE PageCode=@PageCode AND " +
                "(IsPublic='True' OR (IsPublic='False' AND CreatedBy=@LoginName)) AND " +
                "(v.WFID IS NOT NULL AND v.WFStep IS NOT NULL) AND " +
                "((wfdt.Appr1 IS NULL AND wfdt.Appr2 IS NULL AND wfdt.Appr3 IS NULL AND " +
                "wfdt.Appr4 IS NULL AND wfdt.Appr5 IS NULL AND wfdt.Appr6 IS NULL AND " +
                "wfdt.Appr7 IS NULL AND wfdt.Appr8 IS NULL AND wfdt.Appr9 IS NULL AND wfdt.Appr10 IS NULL) OR " +
                "(SELECT COUNT(*) FROM Admin.[UserRole] WHERE LoginName=@LoginName AND UPPER(RoleName)='ADMINISTRATORS' AND IsActive='true') > 0) " +
                "UNION " +
                "SELECT v.ViewNo, v.PageCode, v.[Desc], v.IsPublic, v. SearchIn, v.IsAdvance, " +
                "v.AdvOpt, v.IsStandard, v.WFId, v.WFStep, v.CreatedDate, v.CreatedBy, " +
                "v.UpdatedDate, v.UpdatedBy, CASE WHEN v.IsStandard='true' THEN 'Standard View' ELSE 'Customize View' END AS ViewType " +
                "FROM [APP].[VIEWHANDLER] v " +
                "WHERE PageCode=@PageCode AND " +
                "(IsPublic='True' OR (IsPublic='False' AND CreatedBy=@LoginName)) AND " +
                "(v.WFID IS NULL AND v.WFStep IS NULL)";
            SqlCommand command  = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@PageCode", PageCode));
            command.Parameters.Add(new SqlParameter("@LoginName", LoginName));
            SqlDataReader r     = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    /// <summary>
    /// Get ViewHandler by ViewNo.
    /// </summary>
    /// <param name="ViewNo"></param>
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_ViewHandler_Get_ViewNo(SqlInt32 ViewNo)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm   = "SELECT * FROM [APP].[VIEWHANDLER] WHERE ViewNo=@ViewNo";
            SqlCommand command  = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@ViewNo", ViewNo));
            SqlDataReader r     = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    /// <summary>
    /// Get ViewHandler table schema
    /// </summary>
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_ViewHandler_GetSchema()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM [APP].[VIEWHANDLER]";
            SqlCommand command = new SqlCommand(selectComm, connection);
            SqlDataReader r = command.ExecuteReader(CommandBehavior.SchemaOnly);
            SqlContext.Pipe.Send(r);
        }
    }

    /// <summary>
    /// Get new ViewNo
    /// </summary>
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_ViewHandler_GetNewID()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm   = "SELECT MAX(ViewNo) + 1 FROM APP.ViewHandler";
            SqlCommand command  = new SqlCommand(selectComm, connection);            
            SqlDataReader r     = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }     
};
