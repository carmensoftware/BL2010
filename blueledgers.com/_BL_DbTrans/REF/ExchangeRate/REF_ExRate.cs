using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void REF_ExRate_Get_F_T_EffDate(SqlString FCurrencyCode, SqlString TCurrencyCode, SqlString EffDate)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select * from ref.exrate " +
                                "where fcurrencycode=@FCurrencyCode and tcurrencycode=@TCurrencyCode and effdate<=@EffDate " +
                                "and effdate=(select max(effdate) from ref.exrate where fcurrencycode=@FCurrencyCode and tcurrencycode=@TCurrencyCode and effdate<=@EffDate)";
            SqlCommand command = new SqlCommand(selectComm, connection);

            command.Parameters.Add(new SqlParameter("@FCurrencyCode", FCurrencyCode));
            command.Parameters.Add(new SqlParameter("@TCurrencyCode", TCurrencyCode));
            command.Parameters.Add(new SqlParameter("@EffDate", EffDate));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }    

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void REF_ExRate_GetNewID()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm   = "SELECT ISNULL(MAX(ExRateID),0) + 1 AS [NewID] FROM Ref.ExRate";
            SqlCommand command  = new SqlCommand(selectComm, connection);
            SqlDataReader r     = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void Ref_ExRate_GetList()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm   = "SELECT * FROM Ref.ExRate";
            SqlCommand command  = new SqlCommand(selectComm, connection);
            SqlDataReader r     = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
    
};
