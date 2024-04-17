using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class StockOut : DbHandler
    {
        public StockOut()
        {
            TableName = "[IN].[StockOut]";
            SelectCommand = "SELECT * FROM [IN].[StockOut]";
        }

        //Get List By RefID
        public bool Get(DataSet dsStockOut, string RefId, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefId", RefId);

            var result = DbRetrieve("[IN].[StockOut_Get_RefId]", dsStockOut, dbParams, TableName, ConnStr);

            if (!result)
            {
                dsStockOut.Tables[TableName].PrimaryKey = GetPK(dsStockOut);
            }

            return true;
        }

        private DataColumn[] GetPK(DataSet dsStockOut)
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsStockOut.Tables[TableName].Columns["RefId"];

            return primaryKeys;
        }

        //Get Schema
        public bool GetSchema(DataSet dsStkIn, string ConnStr)
        {
            return DbRetrieve("[IN].[StockOut_GetList]", dsStkIn, null, TableName, ConnStr);
        }

        //Get New Id.
        public string GetNewID(DateTime DocDate, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AtDate", DocDate.ToString("yyyy-MM-dd"));

            var dtStkOut = DbRead("[IN].[StockOut_GetNewID]", dbParams, ConnStr);

            if (dtStkOut != null)
            {
                if (dtStkOut.Rows.Count > 0)
                {
                    return dtStkOut.Rows[0][0].ToString();
                }
            }

            return string.Empty;
        }

        public string GetNewID(DateTime DocDate, string prefix, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@AtDate", DocDate.ToString("yyyy-MM-dd"));
            dbParams[1] = new DbParameter("@Prefix", prefix);

            var dtStkOut = DbRead("[IN].[StockOut_GetNewID]", dbParams, ConnStr);

            if (dtStkOut != null)
            {
                if (dtStkOut.Rows.Count > 0)
                {
                    return dtStkOut.Rows[0][0].ToString();
                }
            }

            return string.Empty;
        }

        // Save.
        public bool Save(DataSet dsStkOut, string connStr)
        {
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsStkOut, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsStkOut, new StockOutDt().SelectCommand, new StockOutDt().TableName);
            return DbCommit(dbSaveSource, connStr);
        }
    }
}