using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.PC
{
    public class EOPDt : DbHandler
    {
        public EOPDt()
        {
            SelectCommand = "SELECT * FROM [IN].[EOPDt]";
            TableName = "EOPDt";
        }

        public bool GetList(DataSet dsEOP, int EOPId, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@EOPId", EOPId.ToString());

            return DbRetrieve("[IN].GetEOPDtByEopId", dsEOP, dbParams, TableName, ConnStr);
        }

        public bool GetList(DataSet dsEOP, int EOPId, DateTime StartDate, DateTime EndDate, string ConnStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@EOPId", EOPId.ToString());
            dbParams[1] = new DbParameter("@StartDate", StartDate.ToString("yyyy-MM-dd"));
            dbParams[2] = new DbParameter("@EndDate", EndDate.ToString("yyyy-MM-dd"));

            return DbRetrieve("[IN].GetEOPDtByEopIdStartDateEndDate", dsEOP, dbParams, TableName, ConnStr);
        }

        public bool GetStructure(DataSet dsEOPDt, string ConnStr)
        {
            return DbRetrieveSchema("[IN].[GetEOPDtSchema]", dsEOPDt, null, TableName, ConnStr);
        }

        public bool Save(DataSet dsEOPDt, string ConnStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsEOPDt, SelectCommand, TableName);

            return DbCommit(dbSaveSource, ConnStr);
        }
    }
}