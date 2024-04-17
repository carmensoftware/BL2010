using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_RECDt_Get_RecNo(SqlString RecNo)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm  = "SELECT rDt.*, u.[Name] UnitName, p.ProductDesc1 ProductName, s.LocationName,acc1.[desc] as NetDrAccNm,acc2.[desc] as TaxDrAccNm " +
                "from PC.RECDt rDt left outer join [IN].Unit u on rDt.UnitCode = u.UnitCode " +
                "left outer join [in].StoreLocation s on (rDt.locationcode=s.locationcode) " +
                "left outer join gl.account acc1 on (rdt.netdracc=acc1.acccode) " +
                "left outer join gl.account acc2 on (rdt.taxdracc=acc2.acccode), [IN].Product p " +
                "where	rDt.ProductCode = p.ProductCode and " +
                "RecNo = @RecNo";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@RecNo", RecNo));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_RECDt_GetSchema()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm  = "SELECT * from [PC].RECDt";
            SqlCommand command = new SqlCommand(selectComm, connection);

            SqlDataReader r = command.ExecuteReader(CommandBehavior.SchemaOnly);
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_RECDt_GetMaxNo(SqlString RecNo)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            //string selectComm = "select isNull(max(RecDtNo),0)+1 as RecDtNo from PC.RecDt where RecNo = @RecNo";
            string selectComm = "select isNull(max(RecDtNo),0) as RecDtNo from PC.RecDt where RecNo = @RecNo";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@RecNo", RecNo));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_RECDt_GetPrice_ProductCode(SqlString ProductCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            
            string selectComm = "SELECT recdt.Price FROM PC.RecDt recdt INNER JOIN PC.Rec rec ON (recdt.RecNo=rec.RecNo AND " + 
					            "Rec.RecDate=(SELECT MAX(RecDate) FROM PC.Rec WHERE ProductCode = @ProductCode))";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@ProductCode", ProductCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }


};
