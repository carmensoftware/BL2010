using System;
using System.Globalization;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxEditors;

/// <summary>
///     Summary description for UnitLookup
/// </summary>
public class UnitLookup
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
            @"SELECT  UnitCode
                FROM    ( SELECT    UnitCode ,
                                    [Name],
                                    ROW_NUMBER() OVER ( ORDER BY UnitCode ) AS [rn]
                          FROM      [IN].[Unit]
                          WHERE     ( UnitCode LIKE @filter )
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
            if (e.Value == null)
                return;

            sqlDataSource.SelectCommand =
                @"SELECT  UnitCode, [Name]
                FROM    [IN].[Unit]
                WHERE   Isactived = 'True'
                        AND UnitCode = @UnitCode
                ORDER BY UnitCode";
            sqlDataSource.ConnectionString = connStr;
            sqlDataSource.SelectParameters.Clear();
            sqlDataSource.SelectParameters.Add("UnitCode", TypeCode.String, e.Value.ToString());
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