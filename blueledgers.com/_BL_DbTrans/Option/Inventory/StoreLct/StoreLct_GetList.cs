using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void StoreLct_GetList()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * from [IN].[StoreLocation]";
            SqlCommand command = new SqlCommand(selectComm, connection);

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void StoreLct_Get_LocateCode(SqlString LocateCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select * from [IN].StoreLocation where LocationCode = @LocateCode";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@LocateCode", LocateCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_StoreLct_GetActiveProdCate()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm   = "SELECT CategoryCode, CategoryName, ParentNo FROM [IN].ProductCategory " +
                "UNION SELECT ProductCode AS CategoryCode, ProductDesc1 AS CategoryName, " + 
                "ProductCate AS ParentNo FROM [IN].Product";
            SqlCommand command  = new SqlCommand(selectComm, connection);            
            SqlDataReader r     = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void StoreLct_Get_CountByDeliveryPoint(SqlString DeliveryPoint)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT COUNT(*) FROM [IN].StoreLocation WHERE DeliveryPoint = @DeliveryPoint";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@DeliveryPoint", DeliveryPoint));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void StoreLctCountByLocationCode(SqlString LocateCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT COUNT(*) FROM [IN].StoreLocation WHERE LocationCode = @LocateCode";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@LocateCode", LocateCode));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
