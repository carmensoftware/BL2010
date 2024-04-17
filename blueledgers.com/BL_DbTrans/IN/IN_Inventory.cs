using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_Inventory_Get_HdrNo(SqlString HdrNo)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm  = "SELECT * from [IN].Inventory where HdrNo = @HdrNo";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@HdrNo", HdrNo));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }


    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_Inventory_GetSchema()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm  = "SELECT * from [IN].Inventory";
            SqlCommand command = new SqlCommand(selectComm, connection);

            SqlDataReader r = command.ExecuteReader(CommandBehavior.SchemaOnly);
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_Inventory_GetMaxNo()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select isNull(max(InvNo),0)+1 as InvNo from [IN].Inventory";
            SqlCommand command = new SqlCommand(selectComm, connection);

            SqlDataReader r = command.ExecuteReader(CommandBehavior.SchemaOnly);
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_Inventory_GetMAvgAudit_ProductCode(SqlString ProductCode)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT v.MAvgAudit FROM PC.RecDt recdt " +
	                            "INNER JOIN PC.Rec rec ON (recdt.RecNo=rec.RecNo AND " +
	 						    "Rec.RecDate=(SELECT MAX(RecDate) FROM PC.Rec WHERE ProductCode = @ProductCode)) " +
	                            "INNER Join [IN].Inventory v ON (recdt.RecNo = v.HdrNo and recdt.RecDtNo = v.DtNo and " +
                                "v.ProductCode = recdt.ProductCode) WHERE recdt.ProductCode = @ProductCode";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@ProductCode", ProductCode));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
