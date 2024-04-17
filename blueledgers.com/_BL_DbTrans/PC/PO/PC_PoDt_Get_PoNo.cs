using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PC_PoDt_Get_PoNo(SqlString PoNo)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            //string selectComm = "select d.*, d.product as ProductCode, p.ProductDesc1, " +
            //                    "l.LocationName, p.StandardCost, p.LastCost, u.[name] as UnitName, " +
            //                    "deli.name as DeliName, TaxTypeN = CASE d.TaxType WHEN 'I' THEN 'Included' WHEN 'A' THEN 'Add' WHEN 'N' THEN 'None' END " +
            //                    "from PC.PoDt d " +
            //                    "left outer join [IN].Product p on d.product = p.productcode " +
            //                    "left outer join [IN].StoreLocation l on  d.location = l.LocationCode " +
            //                    "left outer join [IN].Unit u on d.unit=u.unitcode " +
            //                    "left outer join [IN].DeliveryPoint deli on d.DeliveryPoint = deli.DptCode " +                                
            //                    "where d.pono = @PoNo ";
            string selectComm = "select	d.*, d.product as ProductCode, p.ProductDesc1, " +
		                        "l.LocationName, p.StandardCost, p.LastCost, u.[name] as UnitName, " +
		                        "deli.name as DeliName, prDt.PRNo, " +
                                "TaxTypeN = CASE d.TaxType WHEN 'I' THEN 'Included' WHEN 'A' THEN 'Add' WHEN 'N' THEN 'None' END " +
                                "from PC.PoDt d left outer join [IN].Product p on d.product = p.productcode " +
				                "left outer join [IN].StoreLocation l on  d.location = l.LocationCode " +
				                "left outer join [IN].Unit u on d.unit=u.unitcode " +
				                "left outer join [IN].DeliveryPoint deli on d.DeliveryPoint = deli.DptCode " +
				                "left outer join [PC].PRDt prDt on prDt.PONo = d.PoNo and prDt.PODtNo = d.PoDt " +
                                "where d.pono = @PoNo";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@PoNo", PoNo));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
