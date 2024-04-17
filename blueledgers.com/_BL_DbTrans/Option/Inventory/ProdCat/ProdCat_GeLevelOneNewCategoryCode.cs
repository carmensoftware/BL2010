using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void ProdCat_GeLevelOneNewCategoryCode()
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm = "SELECT Isnull(count(*),0) + 1 as NewCategoryCode From [IN].[ProductCategory] Where LevelNo = '1' ";
            SqlCommand command = new SqlCommand(selectComm, connection);
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void IN_ProductCategory_GetNewItemGroupCode(SqlString SubCategoryCode)
    {
        // Put your code here
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            string selectComm =  "SELECT @SubCategoryCode + " +
                "CASE WHEN LEN(CAST(CAST(SUBSTRING(ISNULL(MAX(CategoryCode),0),3,2) AS INT) + 1 AS NVARCHAR(2))) = 1 THEN " +
	            "    CASE WHEN MAX(CategoryCode) IS NULL THEN " +
		        "        '00' " +
	            "    ELSE " +
		        "        '0' + CAST(CAST(SUBSTRING(ISNULL(MAX(CategoryCode),0),3,2) AS INT) + 1 AS NVARCHAR(2)) " +
	            "    END " +
                "ELSE " +
	            "    CAST(CAST(SUBSTRING(ISNULL(MAX(CategoryCode),0),3,2) AS INT) + 1 AS NVARCHAR(2)) " +
                "END " +
	            "    AS NewItemGroupCode " +
                "FROM " +
	            "    [IN].ProductCategory " +
                "WHERE " +
	            "    ParentNo=@SubCategoryCode";
            SqlCommand command = new SqlCommand(selectComm, connection);
            command.Parameters.Add(new SqlParameter("@SubCategoryCode", SubCategoryCode));
            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
};
