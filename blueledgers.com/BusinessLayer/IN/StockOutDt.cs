using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class StockOutDt : DbHandler
    {
        public StockOutDt()
        {
            var StdR = new StockIn();

            TableName = "[IN].[StockOutDt]";
            SelectCommand = "SELECT * FROM [IN].[StockOutDt]";
        }

        //Get List By ID
        public bool Get(DataSet dsStockOut, string RefId, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefId", RefId);

            var result = DbRetrieve("[IN].[StockOutDt_GetList_Id]", dsStockOut, dbParams, TableName, ConnStr);

            if (!result)
            {
                //MsgError = "Msg001";
                return false;
            }
            dsStockOut.Tables[TableName].PrimaryKey = GetPK(dsStockOut);

            return true;
        }

        private DataColumn[] GetPK(DataSet dsStockOut)
        {
            var primaryKeys = new DataColumn[2];
            primaryKeys[0] = dsStockOut.Tables[TableName].Columns["RefId"];
            primaryKeys[1] = dsStockOut.Tables[TableName].Columns["Id"];

            return primaryKeys;
        }

        public bool GetDetailById(DataSet dsStockOutDt, string RefId, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefId", RefId);

            return DbRetrieve("[IN].[StockOutDt_GetDetail_ById]", dsStockOutDt, dbParams, TableName, ConnStr);
        }

        public DataTable GetDetailById(string RefId, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefId", RefId);

            return DbRead("[IN].[StockOutDt_GetDetail_ById]", dbParams, ConnStr);
        }

        /// <summary>
        ///     Gets On Hand, Reorder, Last price, On Order, Restock, Last Vendor and Average Price data.
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetStockSummary(DataSet dsStockOut, string ProductCode, string LocationCode, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);
            dbParams[1] = new DbParameter("@ProductCode", ProductCode);

            return DbRetrieve("[IN].[StockOut_GetStockOutDtStockSummary]", dsStockOut, dbParams, TableName, ConnStr);
        }

        //Get Schema
        public bool GetSchema(DataSet dsStkOutDt, string ConnStr)
        {
            return DbRetrieve("[IN].[StockOutDt_GetList]", dsStkOutDt, null, TableName, ConnStr);
        }
    }
}