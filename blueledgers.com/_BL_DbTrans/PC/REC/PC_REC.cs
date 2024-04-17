using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_REC_Get_RecNo(SqlString RecNo)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm  = "SELECT * from PC.REC where RecNo = @RecNo";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@RecNo", RecNo));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_REC_GetNewID()
    {
        using (SqlConnection connection = new SqlConnection("context connection = true"))
        {
            connection.Open();

            string preFix = string.Empty;
            string runningNo = string.Empty;

            string prefixSelComm = "SELECT 'REC' + (substring(cast(year(getdate())as varchar(4)),3,2)"
                                + "+ case when len(cast(month(getdate())as varchar(2))) = 1 then '0'"
                                + "+ cast(month(getdate())as varchar(2)) else cast(month(getdate())as varchar(2)) end"
                                + "+ case when len(cast(day(getdate())as varchar(2))) = 1 then '0' + cast(day(getdate())as varchar(2))"
                                + " else cast(day(getdate())as varchar(2)) end)";
            SqlCommand prefixCmd = new SqlCommand(prefixSelComm, connection);
            SqlDataReader prefixReader = prefixCmd.ExecuteReader();

            while (prefixReader.Read())
            {
                preFix = prefixReader.GetString(0);
            }

            prefixReader.Close();

            string runningNoSelCmd = "SELECT ISNULL(CAST(CAST(SUBSTRING(MAX(RecNo),10,4) AS INT) + 1 AS NVARCHAR),'') " +
                "FROM PC.REC WHERE RecNo LIKE '" + preFix + "%'";
            SqlCommand runningNoCmd = new SqlCommand(runningNoSelCmd, connection);
            SqlDataReader runningNoReader = runningNoCmd.ExecuteReader();

            while (runningNoReader.Read())
            {
                runningNo = runningNoReader.GetString(0);
            }


            runningNoReader.Close();

            string returnSelComm = string.Empty;

            if (runningNo == string.Empty)
            {
                returnSelComm = "SELECT '" + preFix + "0001'";
            }
            else
            {
                runningNo = runningNo.PadLeft(4, '0');
                returnSelComm = "SELECT '" + preFix + runningNo + "'";
            }

            SqlCommand returnCmd = new SqlCommand(returnSelComm, connection);
            SqlDataReader returnReader = returnCmd.ExecuteReader();
            SqlContext.Pipe.Send(returnReader);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_REC_GetSchema()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * from [PC].REC";

            SqlCommand command = new SqlCommand(selectComm, connection);

            SqlDataReader r = command.ExecuteReader(CommandBehavior.SchemaOnly);
            SqlContext.Pipe.Send(r);
        }
    }


};
