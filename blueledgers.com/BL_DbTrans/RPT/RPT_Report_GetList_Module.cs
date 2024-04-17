using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void RPT_Report_GetList_Module(SqlString Module)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT rpt.*, 'CategoryName'=cat.Name " +
                "FROM RPT.Report rpt LEFT OUTER JOIN RPT.Category cat ON (rpt.Category=cat.CategoryCode) " +
                "WHERE Module=@Module AND rpt.IsActive = 'true' ORDER BY Category ASC, ReportID ASC";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@Module", Module));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
