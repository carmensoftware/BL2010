using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void StoreLct_GetStoreListByRowFilter(string filter, int startIndex, int endIndex)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT [LocationCode], [LocationName]  " +
                "FROM (SELECT [LocationCode], [LocationName]  row_number()over(order by StoreLct.[LocationCode])  " +
                "as [rn] FROM [IN].[StoreLocation] AS StoreLct WHERE (([LocationCode] + ' ' + [LocationName] ) LIKE  @filter)) AS StoreLctIndex WHERE StoreLctIndex.[rn] between @startIndex and @endIndex";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@filter",filter));
            command.Parameters.Add(new SqlParameter("@startIndex",startIndex));
            command.Parameters.Add(new SqlParameter("@endIndex",endIndex));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
