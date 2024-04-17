using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void ProdCat_GetTaxAccCode_ProductCode(SqlString ProductCode)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select	pc.TaxAccCode from	[IN].Product pro, [IN].ProductCategory pc " +
                                "where pro.ProductCate = pc.CategoryCode and pro.IsActive = 'True' and " +
		                        "pro.ProductCode = @ProductCode" ;

            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@ProductCode", ProductCode));


            SqlDataReader r    = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
