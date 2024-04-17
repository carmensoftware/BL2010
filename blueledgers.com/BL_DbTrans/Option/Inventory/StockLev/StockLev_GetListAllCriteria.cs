using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void StockLev_GetListAllCriteria(String ProductCode , String strQOH)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            string selectComm = " Select  distinct [IN].StockLevel.ProductCode as PrdCode , P.*, [IN].StockLevel.* ,U.* " +
            "    from  [IN].Product LEFT OUTER JOIN  [IN].StockLevel ON [IN].Product.ProductCode = [IN].StockLevel.ProductCode,  " +
            "          [IN].Product P , [IN].StoreLocation L , [IN].ProductCategory C , [IN].ProdLoc PD , [IN].Unit U" +
            "    where P.ProductCate = C.CategoryCode and P.ProductCode = PD.ProductCode  and U.unitcode = P.InventoryUnit and " +
            "          L.LocationCode = PD.LocationCode  and "+ 
            "          P.ProductCode in (" + ProductCode + ")";

            SqlCommand command = new SqlCommand(selectComm, connection);

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
