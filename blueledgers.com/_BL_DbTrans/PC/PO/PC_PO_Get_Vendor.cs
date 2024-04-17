using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_PO_Get_Vendor(SqlString Vendor)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "select	podt.*, po.*, Lo.LocationName " + 
                                "from PC.PoDt podt inner join PC.Po po on(podt.pono=po.pono), [IN].StoreLocation Lo " +
                                "where	podt.Location = Lo.LocationCode and po.vendor = @Vendor and " +
		                        "PoDt not in (select PoDtNo from pc.recdt where pono=po.pono)";

            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@Vendor", Vendor));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
