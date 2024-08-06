using System;
using System.Globalization;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxEditors;

/// <summary>
///     Summary description for VendorLookup
/// </summary>
public class VendorLookup
{
    /// <summary>
    /// </summary>
    /// <param name="comboBox"></param>
    /// <param name="sqlDataSource"></param>
    /// <param name="connStr"></param>
    /// <param name="e"></param>
    public void ItemsRequestedByFilterCondition(ref ASPxComboBox comboBox,
        SqlDataSource sqlDataSource, string connStr,
        ListEditItemsRequestedByFilterConditionEventArgs e)
    {
        sqlDataSource.ConnectionString = connStr;

        sqlDataSource.SelectCommand =
            @"SELECT  [VendorCode]
                FROM    ( SELECT    [VendorCode] ,
                                    ROW_NUMBER() OVER ( ORDER BY [VendorCode] ) AS [rn]
                          FROM      [AP].[Vendor]
                          WHERE     IsActive = 'True'
                                    AND ( VendorCode LIKE @filter )
                        ) AS st
                WHERE   st.[rn] BETWEEN @startIndex AND @endIndex";

        sqlDataSource.SelectParameters.Clear();
        sqlDataSource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
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
    public void ItemRequestedByValue(ref ASPxComboBox comboBox,
        SqlDataSource sqlDataSource, string connStr,
        ListEditItemRequestedByValueEventArgs e)
    {
        try
        {
            var code = string.Empty;

            if (e.Value != null) code = e.Value.ToString();

            sqlDataSource.ConnectionString = connStr;
            sqlDataSource.SelectCommand =
                @"SELECT  [VendorCode]
                FROM    [AP].[Vendor]
                WHERE   IsActive = 'True'
                        AND ( VendorCode = @VendorCode )";

            sqlDataSource.SelectParameters.Clear();
            sqlDataSource.SelectParameters.Add("VendorCode", TypeCode.String, code);
            comboBox.DataSource = sqlDataSource;
            comboBox.DataBind();
            comboBox.ToolTip = comboBox.Text;
        }
        catch (Exception ex)
        {
            LogManager.Error(ex);
        }
    }
}