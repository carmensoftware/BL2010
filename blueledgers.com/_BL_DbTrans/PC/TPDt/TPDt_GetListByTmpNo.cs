using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void TPDt_GetListByTmpNo(int TmpNo)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT tmpdt.TmpNo,tmpdt.TmpDtNo,tmpdt.ProductCode,tmpdt.UnitCode,tmpdt.DiscPercent,Pt.ProductDesc1,Pt.ProductDesc2" +
                                " FROM [IN].TemplateDt tmpdt " +
                                " LEFT OUTER JOIN  [IN].Product Pt ON tmpdt.Productcode = Pt.Productcode WHERE tmpdt.TmpNo = @TmpNo ";
            SqlCommand command  = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@TmpNo", TmpNo.ToString()));

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
