using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{   
    // ************** SP below need to do code tuning ***************************************
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_Product_GetActiveList()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select ProductCode, ProductDesc1 as Name, ProductCode+' '+ ProductDesc1 as ProductDesc1 " +
                                "from [IN].product where isActive = 'True'";
            SqlCommand command = new SqlCommand(selectComm, connection);
            
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_Product_GetList_LocationCode(SqlString LocateCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "Select * from [IN].Product " +
                                "where ProductCode in (select loc.ProductCode from [IN].Prodloc loc where loc.LocationCode = @LocateCode) and " +
                                "IsActive = 'True'";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@LocateCode", LocateCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_Product_GetOnHand_ByLocaProd(SqlString LocationCode, SqlString ProductCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select [IN],[OUT] from [IN].Inventory " +
                                "where ProductCode = @ProductCode and Location = @LocationCode ";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@LocationCode", LocationCode));
            command.Parameters.Add(new SqlParameter("@ProductCode", ProductCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_Product_GetOnOrder_ByLocaProd(SqlString LocationCode, SqlString ProductCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select	dt.OrdQty, dt.RcvQty from [pc].PoDt dt, [pc].Po p " +
                                "where	p.PONo = dt.PONo and Upper(p.DocStatus) = 'PRINTED' and " +
                                "dt.Product = @ProductCode and dt.Location = @LocationCode ";

            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@LocationCode", LocationCode));
            command.Parameters.Add(new SqlParameter("@ProductCode", ProductCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_Product_GetAllOnHand_ByProd(SqlString ProductCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select [IN],[OUT] from [IN].Inventory " +
                                "where ProductCode = @ProductCode ";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@ProductCode", ProductCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_Product_GetAllOnOrder_ByProd(SqlString ProductCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select	dt.OrdQty, dt.RcvQty from [pc].PoDt dt, [pc].Po p " +
                                "where p.PONo = dt.PONo and Upper(p.DocStatus) = 'PRINTED' and " +
                                "dt.Product = @ProductCode";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@ProductCode", ProductCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_Product_GetReOrderReStk_ByLocaProd(SqlString LocationCode, SqlString ProductCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select OrdReorderPoint, OrdRestockLevel from [IN].StockLevel " +
                                "where ProductCode = @ProductCode and LocationCode = @LocationCode";

            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@LocationCode", LocationCode));
            command.Parameters.Add(new SqlParameter("@ProductCode", ProductCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void Product_GetSubCategoryListLookup()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select * from [IN].[ProductCategory] where LevelNo = '2' ";
            SqlCommand command = new SqlCommand(selectComm, connection);

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void Product_GetSchema()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select * from [in].[product] ";
            SqlCommand command = new SqlCommand(selectComm, connection);

            SqlDataReader r = command.ExecuteReader(CommandBehavior.SchemaOnly);
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void Product_GetList_ProductCode(SqlString ProductCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select * from [in].[product] " +
                                 "where ProductCode = @ProductCode";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@ProductCode", ProductCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
    
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void Product_GetList_CateCode(SqlString CateCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select ProductCode, ProductDesc1 from [in].[product] " +
                                 "where productCate = @CateCode";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@CateCode", CateCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void Product_GetList()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select * from [in].[product] ";
            SqlCommand command = new SqlCommand(selectComm, connection);

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void Product_GetItemGroupListLookupByParentNo(SqlString parentNo)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select * from [IN].[ProductCategory] where LevelNo = '3' and ParentNo=@ParentNo  ";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@ParentNo", parentNo));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void Product_GetItemGroupListLookup()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select * from [IN].[ProductCategory] where LevelNo = '3' ";
            SqlCommand command = new SqlCommand(selectComm, connection);

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void Product_GetCategoryListLookup()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select * from [IN].[ProductCategory] where LevelNo = '1' ";
            SqlCommand command = new SqlCommand(selectComm, connection);

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    // ************** SP above need to do code tuning ***************************************

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_Product_GetProductDescList()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT ProductCode, ProductDesc1 FROM [IN].product WHERE isActive = 'True'";
            SqlCommand command = new SqlCommand(selectComm, connection);


            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
