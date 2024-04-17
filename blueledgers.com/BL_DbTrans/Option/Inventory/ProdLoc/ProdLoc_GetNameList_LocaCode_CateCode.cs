using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void ProdLoc_GetNameList_LocaCode_CateCode(SqlString LocaCode, SqlString CateCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select Prd.Productcode, Prd.ProductDesc1 " +
                                "from [IN].ProdLoc PrdLoc, [IN].Product Prd " +
                                "where Prd.ProductCode = PrdLoc.ProductCode and " +
                                "PrdLoc.LocationCode = @LocaCode and " +
                                "Prd.ProductCate = @CateCode";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@LocaCode", LocaCode));
            command.Parameters.Add(new SqlParameter("@CateCode", CateCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};