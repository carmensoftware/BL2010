using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void ProdCat_GetList()
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            //string selectComm  = "select pro.productCate, cate.CategoryName " +                                 
            //                     "from [in].product pro, [in].productCategory cate " +
            //                     "where  pro.productCate = cate.CategoryCode " +
            //                     "order by pro.productCate ";

            //string selectComm = "Select c.LevelNo, C.ParentNo, c.CategoryCode, c.CategoryName " +
            //                     "FROM [in].ProductCategory c order by c.CategoryCode ";

            //string selectComm = "select ROW_NUMBER() OVER (ORDER BY c.CategoryCode aSC) AS ID, " +
            //                    "c.LevelNo, C.ParentNo, c.CategoryCode, c.CategoryName, pro.productCode, pro.productDesc1 " +
            //                    "from   [in].ProductCategory c left join [in].product pro  on c.CategoryCode = pro.productCate " +
            //                    "order by c.CategoryCode";

            string selectComm = "select ROW_NUMBER() OVER (ORDER BY c.LevelNo aSC) AS ID, " +
                                "c.LevelNo, C.ParentNo, c.CategoryCode, c.CategoryName " +
                                "from [in].ProductCategory c " +
                                "order by c.LevelNo";

            SqlCommand command = new SqlCommand(selectComm, connection);            

            SqlDataReader r    = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
