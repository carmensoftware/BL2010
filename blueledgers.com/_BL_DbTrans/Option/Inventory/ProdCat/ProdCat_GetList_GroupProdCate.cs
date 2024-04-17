using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void ProdCat_GetList_GroupProdCate()
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm  = "select pro.productCate, cate.CategoryName " +
                                 "from [in].product pro, [in].productCategory cate " +
                                 "where  pro.productCate = cate.CategoryCode " +
                                 "group by pro.productCate, cate.CategoryName";
            SqlCommand command = new SqlCommand(selectComm, connection);            

            SqlDataReader r    = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
