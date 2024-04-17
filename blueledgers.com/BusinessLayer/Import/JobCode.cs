using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.Import
{
    public class JobCode : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        public JobCode()
        {
            SelectCommand = "SELECT * FROM [IMPORT].[JobCode]";
            TableName = "JobCode";
        }

        public bool GetJobCodeList(DataSet dsJobCode, string filter, string startIndex, string endIndex, string connStr)
        {
            //Get Data

            //    @filter = N'4',
            //@startIndex = 0,
            //@endIndex = 9

            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@filter", filter);
            dbParams[1] = new DbParameter("@startIndex", startIndex);
            dbParams[2] = new DbParameter("@endIndex", endIndex);
            return DbRetrieve("[IMPORT].[GetJobCodeList]", dsJobCode, dbParams, TableName, connStr);
        }

        public string GetSingleJobCode(string connStr)
        {
            string sql = "SELECT COUNT(*) as RecordCount FROM [IMPORT].JobCode";
            DataTable dt = DbExecuteQuery(sql, null, connStr);
            string value = dt.Rows[0]["RecordCount"].ToString();
            if (value == "1")
            {
                sql = "SELECT TOP(1) Code as JobCode FROM [IMPORT].JobCode";
                dt.Clear();
                dt = DbExecuteQuery(sql, null, connStr);
                return dt.Rows[0]["JobCode"].ToString();
            }
            else
                return string.Empty;

        }

        #endregion
    }
}