using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void ProdCat_GetList_LocatCode(SqlString LocationCode)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "Select	PrdCat.CategoryCode, PrdCat.CategoryName " +
                                "From [IN].Product Prd, [IN].ProductCategory PrdCat, [IN].ProdLoc PrdLoc " +
                                "Where PrdLoc.ProductCode  = Prd.ProductCode and " +
		                        "Prd.ProductCate = PrdCat.CategoryCode and " + 
		                        "PrdLoc.LocationCode = @LocationCode " +
                                "group by PrdCat.CategoryCode, PrdCat.CategoryName	";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@LocationCode", LocationCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
