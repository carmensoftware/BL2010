using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_WFDt_GetList_WFId(SqlInt32 WFId)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM [APP].[WFDt] " +
                "WHERE WFId=@WFId ORDER BY [Step]";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@WFId", WFId));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_WFDt_GetApprStatus(SqlString Module,SqlString SubModule)
    {
        using (SqlConnection connection = new SqlConnection("context connection = true"))
        {
            connection.Open();

            string selectComm = "SELECT REPLICATE('_',wfd.Appr) + REPLICATE('A',10-wfd.Appr) " +
                    "FROM App.WF wf LEFT OUTER JOIN App.WFDt wfd ON (wf.WFId=wfd.WFId) " +
                    "WHERE wf.Module=@Module AND wf.SubModule=@SubModule";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@Module", Module));
            command.Parameters.Add(new SqlParameter("@SubModule", SubModule));
            SqlDataReader reader = command.ExecuteReader();

            string apprStatus = string.Empty;

            while(reader.Read())
               {
                   apprStatus += reader.GetSqlString(0).ToString();
               }
            reader.Close();

            string selComm = "SELECT'"+apprStatus + "'AS ApprStatus";
            SqlCommand comm = new SqlCommand(selComm,connection);
            SqlDataReader r = comm.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
              
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_WFDt_GetList_WFId_Step(SqlInt32 WFId, SqlInt32 Step)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM APP.WFDt WHERE WFId = @WFId AND Step = @Step";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@WFId", WFId));
            command.Parameters.Add(new SqlParameter("@Step", Step));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
