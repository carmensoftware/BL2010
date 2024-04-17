using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void ADMIN_VendorMapp_GetList_BuGrpCode(SqlString BuGrpCode, SqlString SysDbName)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT b.BuCode+v.VendorCode AS ID, b.BuCode, " + 
                "b.BuName, vm.LocalCode, v.Name, v.VendorCode AS HQCode " +
                "FROM AP.Vendor v INNER JOIN " + SysDbName.ToString() + ".dbo.Bu b ON (b.BuGrpCode=@BuGrpCode AND b.IsHQ='false') " +
	            "LEFT OUTER JOIN ADMIN.VendorMapp vm ON (vm.BuCode=b.BuCode AND vm.HQCode=v.VendorCode)";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@BuGrpCode", BuGrpCode));            
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void ADMIN_VendorMapp_GetList()
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm   = "SELECT * FROM [ADMIN].VendorMapp";
            SqlCommand command  = new SqlCommand(selectComm, connection);
            SqlDataReader r     = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
