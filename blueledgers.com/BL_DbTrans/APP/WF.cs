using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_WF_GetList()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM [APP].[WF]";
            SqlCommand command = new SqlCommand(selectComm, connection);            
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_WF_Get_Module_SubModule(SqlString Module, SqlString SubModule)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM [APP].[WF] " +
                "WHERE Module=@Module AND SubModule=@SubModule";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@Module", Module));
            command.Parameters.Add(new SqlParameter("@SubModule", SubModule));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_WF_GetApprStatus(SqlString Module, SqlString SubModule)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT CASE WHEN Appr > 0 THEN '_' ELSE 'A' END AS ApprStatus " +
                "FROM App.WF wf LEFT OUTER JOIN App.WFDt wfd ON (wf.WFId=wfd.WFId) " +
                "WHERE wf.Module=@Module AND wf.SubModule=@SubModule";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@Module", Module));
            command.Parameters.Add(new SqlParameter("@SubModule", SubModule));
            SqlDataReader reader = command.ExecuteReader();
            
            string apprStatus = string.Empty;

            while (reader.Read())
            {
                apprStatus += reader.GetSqlString(0).ToString();
            }

            reader.Close();

            string selComm = "SELECT '" + apprStatus + "' AS ApprStatus";
            SqlCommand comm = new SqlCommand(selComm, connection);
            SqlDataReader r = comm.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
