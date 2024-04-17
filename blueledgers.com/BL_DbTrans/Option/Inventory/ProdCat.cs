using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_ProdCat_GetActiveList_ParentNo(SqlString ParentNo)
    {        
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm   = "SELECT * From [IN].[ProductCategory] Where IsActive='true' AND ParentNo=@ParentNo";
            SqlCommand command  = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@ParentNo", ParentNo));
            SqlDataReader r     = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
