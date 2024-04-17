using System;
using System.Globalization;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxEditors;

/// <summary>
///     Summary description for ProductLookup
/// </summary>
public class ProductLookup
{
    /// <summary>
    /// </summary>
    /// <param name="comboBox"></param>
    /// <param name="sqlDataSource"></param>
    /// <param name="connStr"></param>
    /// <param name="e"></param>
    /// <param name="locationCode"></param>
    public void ItemsRequestedByFilterCondition(ref ASPxComboBox comboBox, SqlDataSource sqlDataSource, string connStr,
        ListEditItemsRequestedByFilterConditionEventArgs e, string locationCode)
    {
        if (string.IsNullOrEmpty(locationCode)) return;

        sqlDataSource.ConnectionString = connStr;

        sqlDataSource.SelectCommand =
            @"SELECT  ProductCode ,
                    ProductDesc1 ,
                    ProductDesc2
            FROM    ( SELECT    p.ProductCode ,
                                p.ProductDesc1 ,
                                p.ProductDesc2 ,
                                ROW_NUMBER() OVER ( ORDER BY p.[ProductCode] ) AS [rn]
                        FROM      [IN].Product p
                        WHERE     p.productcode IN ( SELECT   ProductCode
                                                    FROM     [IN].[ProdLoc]
                                                    WHERE    LocationCode = @LocationCode )
                                AND p.IsActive = 'True'
                                AND ( p.ProductCode + ' ' + p.ProductDesc1 + ' '
                                        + p.ProductDesc2 LIKE @filter )
                    ) AS st
            WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

        sqlDataSource.SelectParameters.Clear();
        sqlDataSource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
        sqlDataSource.SelectParameters.Add("LocationCode", TypeCode.String, locationCode);
        sqlDataSource.SelectParameters.Add("startIndex", TypeCode.Int64,
            (e.BeginIndex + 1).ToString(CultureInfo.InvariantCulture));
        sqlDataSource.SelectParameters.Add("endIndex", TypeCode.Int64,
            (e.EndIndex + 1).ToString(CultureInfo.InvariantCulture));
        comboBox.DataSource = sqlDataSource;
        comboBox.DataBind();
        comboBox.ToolTip = comboBox.Text;
    }

    /// <summary>
    /// </summary>
    /// <param name="comboBox"></param>
    /// <param name="sqlDataSource"></param>
    /// <param name="connStr"></param>
    /// <param name="e"></param>
    /// <param name="locationCode"></param>
    public void ItemRequestedByValue(ref ASPxComboBox comboBox, SqlDataSource sqlDataSource, string connStr,
        ListEditItemRequestedByValueEventArgs e, string locationCode)
    {
        try
        {
            if ((e.Value == null || string.IsNullOrEmpty(locationCode))) return;

            sqlDataSource.ConnectionString = connStr;
            sqlDataSource.SelectCommand =
//                @"SELECT  p.ProductCode ,
//                        p.ProductDesc1 ,
//                        p.ProductDesc2
//                FROM    [IN].Product p
//                WHERE   p.productcode IN ( SELECT   ProductCode
//                                           FROM     [IN].[ProdLoc]
//                                           WHERE    LocationCode = @LocationCode )
//                        AND p.IsActive = 'True'
//                        AND ( p.ProductCode = @ProductCode )";

//            sqlDataSource.SelectParameters.Clear();
//            sqlDataSource.SelectParameters.Add("ProductCode", TypeCode.String, e.Value.ToString());
//            sqlDataSource.SelectParameters.Add("LocationCode", TypeCode.String, locationCode);

                @"SELECT  p.ProductCode ,
                        p.ProductDesc1 ,
                        p.ProductDesc2
                FROM    [IN].Product p
                WHERE   p.productcode = @ProductCode
                        AND p.IsActive = 'True' ";

            sqlDataSource.SelectParameters.Clear();
            sqlDataSource.SelectParameters.Add("ProductCode", TypeCode.String, e.Value.ToString());

            comboBox.DataSource = sqlDataSource;
            comboBox.DataBind();
            comboBox.ToolTip = comboBox.Text;
        }
        catch (Exception ex)
        {
            LogManager.Error(ex);
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="comboBox"></param>
    /// <param name="sqlDataSource"></param>
    /// <param name="connStr"></param>
    /// <param name="e"></param>
    /// <param name="locationCode1"></param>
    /// <param name="locationCode2"></param>
    public void ItemsRequestedByFilterCondition2Location(ref ASPxComboBox comboBox, SqlDataSource sqlDataSource,
        string connStr, ListEditItemsRequestedByFilterConditionEventArgs e, string locationCode1, string locationCode2)
    {
        if (string.IsNullOrEmpty(locationCode1)) return;
        if (string.IsNullOrEmpty(locationCode2)) return;

        sqlDataSource.ConnectionString = connStr;

        sqlDataSource.SelectCommand =
            @"SELECT  ProductCode ,
                    ProductDesc1 ,
                    ProductDesc2
            FROM    ( SELECT    p.ProductCode ,
                                p.ProductDesc1 ,
                                p.ProductDesc2 ,
                                ROW_NUMBER() OVER ( ORDER BY p.[ProductCode] ) AS [rn]
                      FROM      [IN].Product p
                      WHERE     p.productcode IN ( SELECT   ProductCode
                                                   FROM     [IN].[ProdLoc]
                                                   WHERE    LocationCode = @LocationCode1 )
                                AND p.ProductCode IN (
                                SELECT  ProductCode
                                FROM    [IN].Prodloc
                                WHERE   LocationCode = @LocationCode2 )
                                AND p.IsActive = 'True'
                                AND ( p.ProductCode + ' ' + p.ProductDesc1 + ' '
                                      + p.ProductDesc2 LIKE @filter )
                    ) AS st
            WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

        sqlDataSource.SelectParameters.Clear();
        sqlDataSource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
        sqlDataSource.SelectParameters.Add("LocationCode1", TypeCode.String, locationCode1);
        sqlDataSource.SelectParameters.Add("LocationCode2", TypeCode.String, locationCode2);
        sqlDataSource.SelectParameters.Add("startIndex", TypeCode.Int64,
            (e.BeginIndex + 1).ToString(CultureInfo.InvariantCulture));
        sqlDataSource.SelectParameters.Add("endIndex", TypeCode.Int64,
            (e.EndIndex + 1).ToString(CultureInfo.InvariantCulture));
        comboBox.DataSource = sqlDataSource;
        comboBox.DataBind();
        comboBox.ToolTip = comboBox.Text;
    }

    /// <summary>
    /// </summary>
    /// <param name="comboBox"></param>
    /// <param name="sqlDataSource"></param>
    /// <param name="connStr"></param>
    /// <param name="e"></param>
    /// <param name="locationCode1"></param>
    /// <param name="locationCode2"></param>
    public void ItemRequestedByValue2Location(ref ASPxComboBox comboBox, SqlDataSource sqlDataSource, string connStr,
        ListEditItemRequestedByValueEventArgs e, string locationCode1, string locationCode2)
    {
        try
        {
            if ((e.Value == null || string.IsNullOrEmpty(locationCode1))) return;
            if ((e.Value == null || string.IsNullOrEmpty(locationCode2))) return;

            sqlDataSource.ConnectionString = connStr;
            sqlDataSource.SelectCommand =
                @"SELECT  p.ProductCode ,
                        p.ProductDesc1 ,
                        p.ProductDesc2
                FROM    [IN].Product p
                WHERE   p.productcode IN ( SELECT   ProductCode
                                           FROM     [IN].[ProdLoc]
                                           WHERE    LocationCode = @LocationCode1 )
                        AND p.productcode IN ( SELECT   ProductCode
                                               FROM     [IN].[ProdLoc]
                                               WHERE    LocationCode = @LocationCode2 )
                        AND p.IsActive = 'True'
                        AND ( p.ProductCode = @ProductCode )";

            sqlDataSource.SelectParameters.Clear();
            sqlDataSource.SelectParameters.Add("ProductCode", TypeCode.String, e.Value.ToString());
            sqlDataSource.SelectParameters.Add("LocationCode1", TypeCode.String, locationCode1);
            sqlDataSource.SelectParameters.Add("LocationCode2", TypeCode.String, locationCode2);
            comboBox.DataSource = sqlDataSource;
            comboBox.DataBind();
            comboBox.ToolTip = comboBox.Text;
        }
        catch (Exception ex)
        {
            LogManager.Error(ex);
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="locationCode1"></param>
    /// <param name="locationCode2"></param>
    /// <param name="productCode"></param>
    /// <param name="connStr"></param>
    /// <returns></returns>
    public System.Data.DataSet GetRecordBy2Location(string locationCode1, string locationCode2, string productCode,
        string connStr)
    {
        var conn = new System.Data.SqlClient.SqlConnection(connStr);
        var da = new System.Data.SqlClient.SqlDataAdapter();
        var cmd = conn.CreateCommand();
        cmd.CommandText =
            @"SELECT  p.ProductCode ,
                    p.ProductDesc1 ,
                    p.ProductDesc2
            FROM    [IN].Product p
            WHERE   p.productcode IN ( SELECT   ProductCode
                                        FROM     [IN].[ProdLoc]
                                        WHERE    LocationCode = @LocationCode1 )
                    AND p.productcode IN ( SELECT   ProductCode
                                            FROM     [IN].[ProdLoc]
                                            WHERE    LocationCode = @LocationCode2 )
                    AND p.IsActive = 'True'
                    AND ( p.ProductCode = @ProductCode )";

        cmd.Parameters.AddWithValue("ProductCode", productCode);
        cmd.Parameters.AddWithValue("LocationCode1", locationCode1);
        cmd.Parameters.AddWithValue("LocationCode2", locationCode2);

        da.SelectCommand = cmd;
        var ds = new System.Data.DataSet();

        conn.Open();
        da.Fill(ds);
        conn.Close();

        return ds;
    }
}