using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_PRDt_Get_PrNo(SqlString PrNo)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            //string selectComm = "SELECT * from [PC].PrDt where PrNo = @PrNo order by Prdtno";
            //string selectComm = "SELECT	PrDt.*, Product.ProductDesc1 as ProDesc " +
            //                    "FROM [PC].PrDt left outer join [IN].Product on PrDt.ProductCode = Product.ProductCode " +
            //                    "WHERE  PrNo = @PrNo order by Prdtno";
            string selectComm = "SELECT	PrDt.*, Product.ProductDesc1 as ProDesc, Unit.Name as UnitName, " +
                                "Vendor.Name as VendorName, d.Name as DeliName " +
                                "FROM [PC].PrDt left outer join [IN].Product on PrDt.ProductCode = Product.ProductCode " +
                                "left outer join [IN].Unit on PrDt.OrderUnit = Unit.UnitCode " +
                                "left outer join [AP].Vendor on PrDt.VendorCode = Vendor.VendorCode " +
                                "left outer join [IN].DeliveryPoint d on PrDt.DeliPoint = d.DptCode " +
                                "WHERE  PrNo = @PrNo order by Prdtno";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@PrNo", PrNo));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
   
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_PRDt_Get_PrNo_PrDtNo(SqlString PrNo, SqlString PRDtNo)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            //string selectComm = "SELECT * from [PC].PrDt where PrNo = @PrNo order by Prdtno";
            string selectComm = "SELECT	PrDt.*, Product.ProductDesc1 as ProDesc " +
                                "FROM [PC].PrDt left outer join [IN].Product on PrDt.ProductCode = Product.ProductCode " +
                                "WHERE  (PrNo = @PrNo) and (PRDtNo = @PRDtNo) order by Prdtno";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@PrNo", PrNo));
            command.Parameters.Add(new SqlParameter("@PRDtNo", PRDtNo));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
    
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_PRDt_GetList()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select (PRNo + CAST(PRDtNo AS NVARCHAR(3))) AS ID, * from PC.PrDt";

            SqlCommand command = new SqlCommand(selectComm, connection);

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_PrDt_GetList_NotCreatePO()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = " select pr.* from [PC].PrDt pr " +
                                " where pr.PrNo not in (select po.PrNo from [PC].PoDt po where pr.pono = po.prno) and " +
                                " pr.PrDtNo not in (select po.PrDtNo from [PC].PoDt po where pr.pono = po.prno) ";

            SqlCommand command = new SqlCommand(selectComm, connection);

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_PrDt_GetListBy_Vendor(String vendorcode)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = " SELECT ROW_NUMBER() OVER(ORDER BY prno,prdtno ASC) AS ID , PRNo , PRDtNo , P.ProductCode ," +
                                " p.productdesc1,p.productdesc2 , OrderQty , ApprQty FROM [pc].prdt a , [in].product p  where (a.productcode = p.productcode) and (CHARINDEX('_',Apprstatus) = 0 AND " +
                                " CHARINDEX('R',Apprstatus) = 0)  and (PONo is null OR PONO = '')  and " +
                                " (vendorcode = @vendorcode) ";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@vendorcode", vendorcode.ToString()));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_PrDt_GetHd_PrNo(String PrNo)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT DISTINCT(VendorCode) AS VendorCode, CONVERT(CHAR(10),ReqDate,111) as ReqDate FROM PC.PRDt " +
                                "WHERE VendorCode is not null and PRNO in (" + PrNo + ") " +
                                "GROUP BY VendorCode, ReqDate ";
            SqlCommand command = new SqlCommand(selectComm, connection);
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_PrDt_GetHd_PrNoPrDtNo(String PrNoPrDtNo)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm   = "SELECT DISTINCT(VendorCode) AS VendorCode FROM PC.PRDt " +
                                  "WHERE PRNO + cast(PRDtNo as nvarchar(4)) in (" + PrNoPrDtNo + ") " +
                                  "GROUP BY VendorCode" ;          
            SqlCommand command  = new SqlCommand(selectComm, connection);
            SqlDataReader r     = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }  

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_PrDt_Get_PrNo_VendorCode(String PrNo, String VendorCode, String ReqDate)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT	DISTINCT(VendorCode) AS VendorCode, ProductCode, OrderUnit, LocationCode, Buyer, DeliPoint, ReqDate, " +
                                "DiscPercent, DiscAmt, TaxType, TaxRate, TaxAdj, PONo, PODtNo, ISNULL(SUM(OrderQty),0) AS OrderQty, " +
                                "ISNULL(SUM(ApprQty),0) AS ApprQty, " +
                                "ISNULL(SUM(ReqQty),0) AS ReqQty, ISNULL(SUM(FOCQty),0) AS FOCQty, ISNULL(SUM(RcvQty),0) AS RcvQty, " +
                                "ISNULL(SUM(TaxAmt),0) AS TaxAmt, ISNULL(SUM(NetAmt),0) AS NetAmt, ISNULL(SUM(TotalAmt),0) AS TotalAmt, " +
                                "ISNULL(SUM(Price),0) AS Price FROM PC.PRDt WHERE PRNO in (" + PrNo + ") and " +
                                "VendorCode = @VendorCode and CONVERT(CHAR(10),ReqDate,111) = @ReqDate " +
                                "GROUP BY VendorCode, ProductCode, OrderUnit, LocationCode, Buyer, " +
                                "DeliPoint, ReqDate, DiscPercent, DiscAmt, TaxType, TaxRate, TaxAdj, PONo, PODtNo ";

            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@VendorCode", VendorCode.ToString()));
            command.Parameters.Add(new SqlParameter("@ReqDate", ReqDate.ToString()));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_PrDt_Get_PrNoPrDtNo_VendorCode(String PrNoPrDtNo, String VendorCode)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            //string selectComm = "SELECT	DISTINCT(VendorCode) AS VendorCode, ProductCode, OrderUnit, LocationCode, Buyer, DeliPoint, ReqDate, " +
            //                    "DiscPercent, DiscAmt, TaxType, TaxRate, TaxAdj, PONo, PODtNo, ISNULL(SUM(OrderQty),0) AS OrderQty, ISNULL(SUM(ApprQty),0) AS ApprQty, " +
            //                    "ISNULL(SUM(ReqQty),0) AS ReqQty, ISNULL(SUM(FOCQty),0) AS FOCQty, ISNULL(SUM(RcvQty),0) AS RcvQty, " +
            //                    "ISNULL(SUM(TaxAmt),0) AS TaxAmt, ISNULL(SUM(NetAmt),0) AS NetAmt, ISNULL(SUM(TotalAmt),0) AS TotalAmt, " +
            //                    "ISNULL(SUM(Price),0) AS Price FROM	PC.PRDt WHERE PRNO + cast(PRDtNo as nvarchar(4)) in (" + PrNoPrDtNo + ") " +
            //                    "and VendorCode = @VendorCode GROUP BY VendorCode, ProductCode, OrderUnit, LocationCode, Buyer, " +
            //                    "DeliPoint, ReqDate, DiscPercent, DiscAmt, TaxType, TaxRate, TaxAdj, PONo, PODtNo ";
            string selectComm = "SELECT	DISTINCT(VendorCode) AS VendorCode, ProductCode, OrderUnit, LocationCode, Buyer, DeliPoint, ReqDate, " +
                                "DiscPercent, DiscAmt, TaxType, TaxRate, TaxAdj, PONo, PODtNo, Price, ISNULL(SUM(OrderQty),0) AS OrderQty, ISNULL(SUM(ApprQty),0) AS ApprQty, " +
                                "ISNULL(SUM(ReqQty),0) AS ReqQty, ISNULL(SUM(FOCQty),0) AS FOCQty, ISNULL(SUM(RcvQty),0) AS RcvQty, " +
                                "ISNULL(SUM(TaxAmt),0) AS TaxAmt, ISNULL(SUM(NetAmt),0) AS NetAmt, ISNULL(SUM(TotalAmt),0) AS TotalAmt " +
                                "FROM PC.PRDt WHERE PRNO + cast(PRDtNo as nvarchar(4)) in (" + PrNoPrDtNo + ") " +
                                "and VendorCode = @VendorCode GROUP BY VendorCode, ProductCode, OrderUnit, LocationCode, Buyer, " +
                                "DeliPoint, ReqDate, DiscPercent, DiscAmt, TaxType, TaxRate, TaxAdj, PONo, PODtNo, Price ";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@VendorCode", VendorCode.ToString()));
            SqlDataReader r    = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_PrDt_GetDt_PrNo(String PrNo)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM PC.PRDt WHERE PRNO in (" + PrNo + ") ";

            SqlCommand command = new SqlCommand(selectComm, connection);

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_PrDt_GetDt_PrNoPrDtNo(String PrNoPrDtNo)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM PC.PRDt WHERE PRNO + cast(PRDtNo as nvarchar(4)) in (" + PrNoPrDtNo + ") ";
                
                                //"SELECT DISTINCT(VendorCode) AS VendorCode, ProductCode, OrderUnit, LocationCode, Buyer, DeliPoint, ReqDate, " +
                                //"DiscPercent, DiscAmt, TaxType, TaxRate, PONo, PODtNo, ISNULL(SUM(OrderQty),0) AS OrderQty, ISNULL(SUM(ApprQty),0) AS ApprQty, " +
                                //"ISNULL(SUM(ReqQty),0) AS ReqQty, ISNULL(SUM(FOCQty),0) AS FOCQty, ISNULL(SUM(RcvQty),0) AS RcvQty, " +
                                //"ISNULL(SUM(TaxAmt),0) AS TaxAmt, ISNULL(SUM(NetAmt),0) AS NetAmt, ISNULL(SUM(TotalAmt),0) AS TotalAmt, " +
                                //"ISNULL(SUM(Price),0) AS Price FROM PC.PRDt WHERE PRNO + cast(PRDtNo as nvarchar(4)) in (" + PrNoPrDtNo + ") " +
                                //"GROUP BY VendorCode, ProductCode, OrderUnit, LocationCode, Buyer, DeliPoint, ReqDate, DiscPercent, " +
                                //"DiscAmt, TaxType, TaxRate, PONo, PODtNo";                
                                
            SqlCommand command = new SqlCommand(selectComm, connection);            

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }   

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_PrDt_GetList_forPObyPR()
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT	ROW_NUMBER() OVER(ORDER BY dt.PrNo,dt.PrDtNo ASC) AS ID, dt.PrNo, p.Description, dt.PrDtNo, " +
                                "dt.LocationCode, (select BuCode from Admin.BU) as BuCode, VendorCode " +
                                "FROM [PC].PrDt dt, [PC].Pr p WHERE	p.PrNo = dt.PrNo and " +
                                "(dt.PONo is null OR dt.PONO = '') and (charindex('_',dt.apprstatus) < 1 and charindex('R',dt.apprstatus) < 1) " +
                                "ORDER BY dt.PrNo, dt.PrDtNo, dt.LocationCode, VendorCode ";
            SqlCommand command = new SqlCommand(selectComm, connection);            

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_PrDt_GetList_forPObyPR_GroupByPrNo()
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            //string selectComm = "SELECT	ROW_NUMBER() OVER(ORDER BY dt.PrNo ASC) AS ID, dt.PrNo, p.Description " +
            //                    "FROM [PC].PrDt dt, [PC].Pr p WHERE p.PrNo = dt.PrNo and " +
            //                    "(dt.PONo is null OR dt.PONO = '') and (charindex('_',dt.apprstatus) < 1 and charindex('R',dt.apprstatus) < 1) " +
            //                    "GROUP BY dt.PrNo, p.Description ";
             string selectComm = "SELECT ROW_NUMBER() OVER(ORDER BY dt.PrNo ASC) AS ID, dt.PrNo, p.Description, " +
                                 "(select BuCode from Admin.BU) as BuCode " +
                                 "FROM [PC].PrDt dt, [PC].Pr p WHERE p.PrNo = dt.PrNo and " +
                                 "(dt.PONo is null OR dt.PONO = '') and (charindex('_',dt.apprstatus) < 1 and charindex('R',dt.apprstatus) < 1) " +
                                 "GROUP BY dt.PrNo, p.Description " ;
            SqlCommand command = new SqlCommand(selectComm, connection);

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_PrDt_GetListByTmpNo(String TmpNo)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT * FROM [PC].PrDt WHERE PrNo = @TmpNo ";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@TmpNo", TmpNo.ToString()));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_PRDt_GetSchema()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            //string selectComm = "SELECT * from [PC].PrDt";
            string selectComm = "SELECT	PrDt.*, Product.ProductDesc1 as ProDesc, Unit.Name as UnitName, " +
                                "Vendor.Name as VendorName, d.Name as DeliName " +
                                "FROM [PC].PrDt left outer join [IN].Product on PrDt.ProductCode = Product.ProductCode " +
                                "left outer join [IN].Unit on PrDt.OrderUnit = Unit.UnitCode " +
                                "left outer join [AP].Vendor on PrDt.VendorCode = Vendor.VendorCode " +
                                "left outer join [IN].DeliveryPoint d on PrDt.DeliPoint = d.DptCode " +
                                "order by Prdtno";

            SqlCommand command = new SqlCommand(selectComm, connection);

            SqlDataReader r = command.ExecuteReader(CommandBehavior.SchemaOnly);
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_PrDt_CountbyLocationCode(SqlString LocateCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT count(*) FROM [PC].PrDt WHERE LocationCode = @LocateCode";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@LocateCode", LocateCode));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_PrDt_Get_PONo(String PONo)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT	pr.PRNo, pr.PRDate, dt.ProductCode, pro.ProductDesc1, pro.ProductDesc2, " +
		                        "dt.ReqQty, dt.FOCQty, dt.OrderUnit, dt.NetAmt, dt.TaxAmt, dt.TotalAmt " +
                                "FROM [PC].PR pr, [PC].PRDt dt, [IN].Product pro " +
                                "WHERE pr.PRNo = dt.PRNo and dt.ProductCode = pro.ProductCode and PoNo = @PONo ";
      
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@PONo", PONo.ToString()));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    } 


};
