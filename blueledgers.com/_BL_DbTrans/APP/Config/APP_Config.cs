using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
	/// TEST
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void APP_Config_Get_Module_SubModule_Key(SqlString Module, SqlString SubModule, SqlString Key)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm  = "SELECT * FROM APP.Config WHERE Module=@Module AND SubModule=@SubModule AND [Key]=@Key";
            SqlCommand command = new SqlCommand(selectComm, connection);

            command.Parameters.Add(new SqlParameter("@Module", Module));
            command.Parameters.Add(new SqlParameter("@SubModule", SubModule));
            command.Parameters.Add(new SqlParameter("@Key", Key));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    //[Microsoft.SqlServer.Server.SqlProcedure]
    //public static void REF_CUR_GetList()
    //{
    //    using (SqlConnection connection = new SqlConnection("context connection=true"))
    //    {
    //        connection.Open();

    //        string selectComm  = "SELECT * from Ref.Currency";
    //        SqlCommand command = new SqlCommand(selectComm, connection);
    //        SqlDataReader r    = command.ExecuteReader();
    //        SqlContext.Pipe.Send(r);
    //    }
    //}
};
