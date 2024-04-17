using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void ProdLoc_GetSelectProduct(SqlString LocatCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm  = "select prod.ProductCate, prod.productCode, prod.ProductDesc1, " +
	                             "'check' = case when prod.productCode in (select pl.ProductCode from [in].prodloc pl " +
                                 "where pl.LocationCode = @LocatCode) " +
                                 "then 'true' else 'false' end from [in].product prod";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@LocatCode", LocatCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
