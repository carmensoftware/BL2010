using System.Data;

/// <summary>
///     Summary description for JobCodeLookup
/// </summary>
public class JobCodeLookup
{
    /// <summary>
    /// </summary>
    /// <param name="jobCode"></param>
    /// <param name="connStr"></param>
    /// <returns></returns>
    public DataSet GetRecord(string jobCode, string connStr)
    {
        return new BlueLedger.PL.BaseClass.BasePage().GetDataSet(connStr,
            string.Format("SELECT Code, Description FROM [IMPORT].[JobCode] WHERE (CODE = '{0}')", jobCode));
    }
}