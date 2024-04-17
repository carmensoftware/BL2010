using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void Unit_GetActiveListByRowFilter(string filter, int startIndex, int endIndex)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT [UnitCode], [Name]  " +
                "FROM (SELECT [UnitCode], [Name]  row_number()over(order by Unit.[UnitCode])  " +
                "as [rn] FROM [IN].[Unit] AS Unit WHERE (([UnitCode] + ' ' + [Name] ) LIKE  @filter)) AS UnitIndex WHERE UnitIndex.[rn] between @startIndex and @endIndex";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@filter",filter));
            command.Parameters.Add(new SqlParameter("@startIndex",startIndex));
            command.Parameters.Add(new SqlParameter("@endIndex",endIndex));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
