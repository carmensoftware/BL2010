using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_PriceList_GetList_VendorCode_ProductCode(SqlString VendorCode, SqlString ProductCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm  = "SELECT * from [IN].PriceList where VendorCode = @VendorCode and ProductCode = @ProductCode";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@VendorCode", VendorCode));
            command.Parameters.Add(new SqlParameter("@ProductCode", ProductCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_PriceList_GetList()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm  = "SELECT * from [IN].PriceList";
            SqlCommand command = new SqlCommand(selectComm, connection);

            SqlDataReader r    = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
    
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_PriceList_GetListGroupProductCode()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm  = "select plct.DateFrom, plct.DateTo, plct.ProductCode, pro.ProductDesc1 " + 
                                 "from [IN].PriceList plct, [IN].Product pro " +
                                 "where	plct.ProductCode = pro.ProductCode " +
                                 "group by plct.DateFrom, plct.DateTo, plct.ProductCode, pro.ProductDesc1";
            SqlCommand command = new SqlCommand(selectComm, connection);            

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_PriceList_GetListGroupVendorCode()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select	p.DateFrom, p.DateTo, p.VendorCode, v.Name " +
                                "from [IN].PriceList p, [AP].Vendor v " + 
                                "where	p.VendorCode = v.VendorCode " +
                                "group by p.DateFrom, p.DateTo, p.VendorCode, v.Name";
            SqlCommand command = new SqlCommand(selectComm, connection);

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_PriceList_GetList_VendorCode(SqlString VendorCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * from [IN].PriceList where VendorCode = @VendorCode";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@VendorCode", VendorCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_PriceList_GetList_ProductCode(SqlString ProductCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT pl.*, p.ProductDesc1, v.Name, " +
                                "CASE WHEN pl.Tax = 'A' THEN 'Add' " +
	                            "WHEN pl.Tax = 'I' THEN 'Included' " +
                                "WHEN pl.Tax = 'N' THEN 'None' END AS TaxType " +
                                "from [IN].PriceList pl left outer join [IN].Product p on pl.ProductCode = p.ProductCode " +
					            "left outer join [AP].Vendor v on pl.VendorCode = v.VendorCode " +
                                "where pl.ProductCode = @ProductCode";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@ProductCode", ProductCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_PriceList_Get_DateFrom_DateTo_VendorCode(SqlDateTime DateFrom, SqlDateTime DateTo, SqlString VendorCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select	pl.*, " +
                                "CASE WHEN pl.Tax = 'A' THEN 'Add' " +
                                "WHEN pl.Tax = 'I' THEN 'Included' " +
                                "WHEN pl.Tax = 'N' THEN 'None' END AS TaxType, " +
                                "pro.productDesc1, v.Name " +
                                "from [IN].pricelist pl left outer join  AP.Vendor v on (pl.VendorCode = v.VendorCode) " +
					            "left outer join [IN].product pro on pl.productCode = pro.productCode " +
                                "where pl.datefrom = @DateFrom and pl.dateto = @DateTo and pl.vendorcode = @VendorCode";
                                
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@DateFrom", DateFrom));
            command.Parameters.Add(new SqlParameter("@DateTo", DateTo));
            command.Parameters.Add(new SqlParameter("@VendorCode", VendorCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_PriceList_Get_DateFrom_DateTo_ProductCode(SqlDateTime DateFrom, SqlDateTime DateTo, SqlString ProductCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select	pl.*, " +
                                "CASE WHEN pl.Tax = 'A' THEN 'Add' " +
                                "WHEN pl.Tax = 'I' THEN 'Included' " +
                                "WHEN pl.Tax = 'N' THEN 'None' END AS TaxType, " +
                                "pro.productDesc1, v.Name " +
                                "from [IN].pricelist pl left outer join  AP.Vendor v on (pl.VendorCode = v.VendorCode) " +
                                "left outer join [IN].product pro on pl.productCode = pro.productCode " +
                                "where pl.datefrom = @DateFrom and pl.dateto = @DateTo and pl.productCode = @ProductCode";

            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@DateFrom", DateFrom));
            command.Parameters.Add(new SqlParameter("@DateTo", DateTo));
            command.Parameters.Add(new SqlParameter("@ProductCode", ProductCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_PriceList_GetSchema()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * from [IN].PriceList";
            SqlCommand command = new SqlCommand(selectComm, connection);

            SqlDataReader r = command.ExecuteReader(CommandBehavior.SchemaOnly);
            SqlContext.Pipe.Send(r);
        }
    }

    //[Microsoft.SqlServer.Server.SqlProcedure]
    //public static void IN_Inventory_GetMaxNo()
    //{
    //    using (SqlConnection connection = new SqlConnection("context connection=true"))
    //    {
    //        connection.Open();

    //        string selectComm = "select isNull(max(InvNo),0)+1 as InvNo from [IN].Inventory";
    //        SqlCommand command = new SqlCommand(selectComm, connection);

    //        SqlDataReader r = command.ExecuteReader(CommandBehavior.SchemaOnly);
    //        SqlContext.Pipe.Send(r);
    //    }
    //}
};
