using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void VendorMapp_GetList()
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm   = "SELECT VendorMap.ID,VendorMap.VendorCode,VendorMap.SUNVendorCode,Vendor.Name FROM [ADMIN].[VendorMapp] VendorMap LEFT OUTER JOIN " +
                                "[AP].[Vendor] Vendor ON  Vendor.VendorCode = VendorMap.VendorCode";
            SqlCommand command  = new SqlCommand(selectComm, connection);
            SqlDataReader r     = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
}
