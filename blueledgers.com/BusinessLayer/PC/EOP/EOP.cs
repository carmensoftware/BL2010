using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.PC
{
    public class EOP : DbHandler
    {
        private readonly EOPDt eopDt = new EOPDt();

        public EOP()
        {
            SelectCommand = "SELECT * FROM [IN].[EOP]";
            TableName = "EOP";
        }

        public bool Get(DataSet dsEOP, int EOPId, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@EOPId", EOPId.ToString());

            return DbRetrieve("[IN].GetEOPByEopId", dsEOP, dbParams, TableName, ConnStr);
        }

        public bool GetStructure(DataSet dsEOP, string ConnStr)
        {
            return DbRetrieveSchema("[IN].[GetEOPList]", dsEOP, null, TableName, ConnStr);
        }

        public int GetNewID(string ConnStr)
        {
            return DbReadScalar("[IN].GetEOPNewID", null, ConnStr);
        }

        public bool EopExits(DateTime endDate, string storeId, string connStr)
        {
            string cmd = string.Empty;
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@EndDate", endDate.ToString("yyyy-MM-dd HH:mm:ss"));
            dbParams[1] = new DbParameter("@StoreId", storeId);

            cmd += "SELECT CAST(CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END as BIT) as Value ";
            cmd += "FROM [IN].Eop ";
            cmd += "WHERE CAST(EndDate as DATE) = CAST(@EndDate as DATE) ";
            cmd += "  AND StoreId = @StoreId";

            DataTable dt = DbExecuteQuery(cmd, dbParams, connStr);


            if (dt.Rows[0][0].ToString() == "1")
                return true;
            else
                return false;
        }
        public bool Save(DataSet dsEop, string ConnStr)
        {
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsEop, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsEop, eopDt.SelectCommand, eopDt.TableName);

            return DbCommit(dbSaveSource, ConnStr);
        }
    }
}