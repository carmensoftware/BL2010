using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class StockInDt : DbHandler
    {
        public StockInDt()
        {
            TableName = "[IN].[StockInDt]";
            SelectCommand = "SELECT * FROM [IN].[StockInDt]";
        }

        //Get List By ID
        public bool Get(DataSet dsStockIn, string Id, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Id", Id);

            var result = DbRetrieve("[IN].[StockInDt_GetList_Id]", dsStockIn, dbParams, TableName, ConnStr);

            if (!result)
            {
                //MsgError = "Msg001";
                return false;
            }
            dsStockIn.Tables[TableName].PrimaryKey = GetPK(dsStockIn);

            return true;
        }

        private DataColumn[] GetPK(DataSet dsStockIn)
        {
            var primaryKeys = new DataColumn[2];
            primaryKeys[0] = dsStockIn.Tables[TableName].Columns["Id"];
            primaryKeys[1] = dsStockIn.Tables[TableName].Columns["RefId"];

            return primaryKeys;
        }

        public bool GetDetailById(DataSet dsStockInDt, string Id, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Id", Id);

            return DbRetrieve("[IN].[StockInDt_GetDetail_ById]", dsStockInDt, dbParams, TableName, ConnStr);
        }

        public DataTable GetDetailById(string Id, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Id", Id);

            return DbRead("[IN].[StockInDt_GetDetail_ById]", dbParams, ConnStr);
        }

        /// <summary>
        ///     Gets On Hand, Reorder, Last price, On Order, Restock, Last Vendor and Average Price data.
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetStockSummary(DataSet dsStockIn, string ProductCode, string LocationCode, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);
            dbParams[1] = new DbParameter("@ProductCode", ProductCode);

            return DbRetrieve("[IN].[StockIn_GetStockInDtStockSummary]", dsStockIn, dbParams, TableName, ConnStr);
        }

        //Get Schema
        public bool GetSchema(DataSet dsStkDt, string ConnStr)
        {
            return DbRetrieve("[IN].[StockInDt_GetList]", dsStkDt, null, TableName, ConnStr);
        }
    }
}