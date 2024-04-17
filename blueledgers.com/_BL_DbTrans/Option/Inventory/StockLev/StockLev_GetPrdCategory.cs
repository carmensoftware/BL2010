using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void StockLev_GetPrdCategory(SqlString ArrProd, SqlString QOH, SqlBoolean IsHideZero)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = " select * " +
                                " from [IN].StockLevel slv , [IN].ProductCategory pdc " +
                                " Where slv.CategoryCode = pdc.CategoryCode " +
                                " and slv.LocationCode = @LocationCode and pdc.IsActive = 'True' ";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@ArrProd", ArrProd));
            command.Parameters.Add(new SqlParameter("@QOH", QOH));
            command.Parameters.Add(new SqlParameter("@IsHideZero", IsHideZero));


            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
