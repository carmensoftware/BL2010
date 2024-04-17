using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void VendorCategory_GetActiveListByRowFilter(string filter, int startIndex, int endIndex)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT [VendorCategoryCode], [Name]  " +
                "FROM (SELECT [VendorCategoryCode], [Name]  row_number()over(order by VendorCat.[VendorCategoryCode])  " +
                "as [rn] FROM [AP].[VendorCategory] AS VendorCat WHERE (([VendorCategoryCode] + ' ' + [Name] ) LIKE  @filter)) AS VendorCatIndex WHERE VendorCatIndex.[rn] between @startIndex and @endIndex";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@filter",filter));
            command.Parameters.Add(new SqlParameter("@startIndex",startIndex));
            command.Parameters.Add(new SqlParameter("@endIndex",endIndex));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
