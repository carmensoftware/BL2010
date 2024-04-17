using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class StockIn : DbHandler
    {
        public StockIn()
        {
            TableName = "[IN].[StockIn]";
            SelectCommand = "SELECT * FROM [IN].[StockIn]";
        }

        //Get List By RefID
        public bool Get(DataSet dsStockIn, string RefId, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefId", RefId);

            var result = DbRetrieve("[IN].[StockIn_Get_RefId]", dsStockIn, dbParams, TableName, ConnStr);

            if (!result)
            {
                dsStockIn.Tables[TableName].PrimaryKey = GetPK(dsStockIn);
            }

            return true;
        }

        private DataColumn[] GetPK(DataSet dsStockIn)
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsStockIn.Tables[TableName].Columns["RefId"];

            return primaryKeys;
        }

        //Get Schema
        public bool GetSchema(DataSet dsStkIn, string ConnStr)
        {
            return DbRetrieve("[IN].[StockIn_GetList]", dsStkIn, null, TableName, ConnStr);
        }

        //Get New Id.
        public string GetNewID(DateTime DocDate, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AtDate", DocDate.ToString("yyyy-MM-dd"));

            var dtStkIn = DbRead("[IN].[StockIn_GetNewID]", dbParams, ConnStr);

            if (dtStkIn != null)
            {
                if (dtStkIn.Rows.Count > 0)
                {
                    return dtStkIn.Rows[0][0].ToString();
                }
            }

            return string.Empty;
        }


        public string GetNewID(DateTime DocDate, string prefix, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@AtDate", DocDate.ToString("yyyy-MM-dd"));
            dbParams[1] = new DbParameter("@Prefix", prefix);

            var dtStkIn = DbRead("[IN].[StockIn_GetNewID]", dbParams, ConnStr);

            if (dtStkIn != null)
            {
                if (dtStkIn.Rows.Count > 0)
                {
                    return dtStkIn.Rows[0][0].ToString();
                }
            }

            return string.Empty;
        }


        // Save.
        public bool Save(DataSet dsStkIn, string connStr)
        {
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsStkIn, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsStkIn, new StockInDt().SelectCommand, new StockInDt().TableName);
            return DbCommit(dbSaveSource, connStr);
        }
    }
}