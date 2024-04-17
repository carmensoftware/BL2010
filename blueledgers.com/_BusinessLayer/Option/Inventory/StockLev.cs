using System.Data;
using Blue.DAL;

namespace Blue.BL.Option.Inventory
{
    public class StockLev : DbHandler
    {
        public StockLev()
        {
            SelectCommand = "SELECT * FROM [IN].[StockLevel]";
            TableName = "StockLevel";
        }

        //public bool GetList(DataSet dsStockLev, ref string MsgError, string connStr)
        //{

        //    bool result = DbRetrieve("dbo.StockLev_GetList", dsStockLev, null, this.TableName, connStr);

        //    if (!result)
        //    {
        //        MsgError = "Msg001";
        //        return false;
        //    }

        //    return true;
        //}

        public bool GetList(DataSet dsStockLev, string LocationCode, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);

            return DbRetrieve("[IN].StockLevGetList_LocationCode", dsStockLev, dbParams, TableName, connStr);
        }

        public DataTable GetProductCategory(string LocationCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);

            return DbRead("dbo.StockLev_GetPrdCategory", dbParams, connStr);
        }

        public DataTable GetStocLevList(string LocationCode, string ProductCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);
            dbParams[1] = new DbParameter("@ProductCode", ProductCode);

            // Create parameters
            return DbRead("dbo.StockLev_GetList_LocaProd", dbParams, connStr);
        }

        //public DataTable StockLev_GetListAllCriteria(string ProductCode ,  string strQOH , string connStr)
        //{
        //    // Create parameters
        //    DbParameter[] dbParams = new DbParameter[2];
        //    dbParams[0] = new DbParameter("@ProductCode", ProductCode);
        //    dbParams[1] = new DbParameter("@strQOH", strQOH);

        //    // Create parameters
        //    return DbRead("dbo.StockLev_GetListAllCriteria", dbParams, connStr);
        //}


        public bool StockLev_GetListAllCriteria(DataSet dsStockLev, string ProductCode, string strQOH,
            ref string MsgError, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);
            dbParams[1] = new DbParameter("@strQOH", strQOH);


            var result = DbRetrieve("dbo.StockLev_GetListAllCriteria", dsStockLev, dbParams, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        public bool Save(DataSet dsStoreLev, ref string MsgError, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsStoreLev, SelectCommand, TableName);

            var result = DbCommit(dbSaveSource, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }


        /// <summary>
        ///     Get schema
        /// </summary>
        /// <param name="dsUser"></param>
        public void GetStockLevSchema(DataSet StockLev, string connStr)
        {
            DbRetrieveSchema("dbo.StockLev_GetList", StockLev, null, TableName, connStr);
        }

        /// <summary>
        ///     Get Lookup Store location.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            // Create parameters
            return DbRead("dbo.StockLev_GetList", null, connStr);
        }
    }
}