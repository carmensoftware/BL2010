using System.Data;
using Blue.DAL;

namespace Blue.BL.Option.Inventory
{
    public class ApprLv : DbHandler
    {
        public ApprLv()
        {
            SelectCommand = "SELECT * FROM [IN].[ApprLv]";
            TableName = "ApprLv";
        }

        public bool GetListApprLv(DataSet dsApprLv, string connStr)
        {
            return DbRetrieve("[IN].[GetApprLvSchema]", dsApprLv, null, TableName, connStr);
        }

        public bool Save(DataSet dsApprLv, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsApprLv, SelectCommand, TableName);
            return DbCommit(dbSaveSource, connStr);
        }

        public DataTable GetLookUp(string connStr)
        {
            // Create parameters
            return DbRead("[IN].[ApprLv_GetLookUp]", null, connStr);
        }

        public string GetName(string ApprLvCode, string connStr)
        {
            var strName = string.Empty;

            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ApprLv", ApprLvCode);

            var drUnit = DbRead("[IN].[ApprLv_GetName]", dbParams, connStr);

            if (drUnit.Rows.Count > 0)
            {
                strName = drUnit.Rows[0]["Name"].ToString();
            }

            return strName;
        }
    }
}