using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class TransferOutDt : DbHandler
    {
        public TransferOutDt()
        {
            TableName = "[IN].[TransferOutDt]";
            SelectCommand = "SELECT * FROM [IN].[TransferOutDt]";
        }

        public bool GetStructure(DataSet dsTrfOutDt, string connStr)
        {
            return DbRetrieveSchema("[IN].[GetTrfOutDtSchema]", dsTrfOutDt, null, TableName, connStr);
        }

        //Get List By RefId
        public bool GetList(DataSet dsTrfOutDt, string RefId, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefId", RefId);

            return DbRetrieve("[IN].[GetTrfOutDtListByRefId]", dsTrfOutDt, dbParams, TableName, connStr);
        }

        public decimal GetTotalQty(string RefId, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefId", RefId);

            var dtGet = new DataTable();
            dtGet = DbRead("[IN].[GetTransferOutTotalQty]", dbParams, connStr);

            if (dtGet.Rows.Count > 0)
            {
                return decimal.Parse(dtGet.Rows[0][0].ToString());
            }

            return 0;
        }

        //Save Data
        public bool Save(DataSet dsTrfOutDt, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsTrfOutDt, SelectCommand, TableName);

            return DbCommit(dbSaveSource, connStr);
        }

        ////Get List By ID
        //public bool Get(DataSet dsStockIn, string Id, string ConnStr)
        //{
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0] = new DbParameter("@Id", Id.ToString());

        //    bool result = DbRetrieve("[IN].[StockInDt_GetList_Id]", dsStockIn, dbParams, this.TableName, ConnStr);

        //    if (!result)
        //    {
        //        //MsgError = "Msg001";
        //        return false;
        //    }
        //    else
        //    {
        //        dsStockIn.Tables[this.TableName].PrimaryKey = GetPK(dsStockIn);
        //    }

        //    return true;
        //}

        //private DataColumn[] GetPK(DataSet dsStockIn)
        //{
        //    DataColumn[] primaryKeys = new DataColumn[2];
        //    primaryKeys[0] = dsStockIn.Tables[this.TableName].Columns["Id"];
        //    primaryKeys[1] = dsStockIn.Tables[this.TableName].Columns["RefId"];

        //    return primaryKeys;
        //}

        //public bool GetDetailById(DataSet dsStockInDt,string Id, string ConnStr)
        //{
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0] = new DbParameter("@Id", Id);

        //    return DbRetrieve("[IN].[StockInDt_GetDetail_ById]", dsStockInDt, dbParams, this.TableName, ConnStr);
        //}

        //public DataTable GetDetailById(string Id, string ConnStr)
        //{
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0] = new DbParameter("@Id", Id);

        //    return DbRead("[IN].[StockInDt_GetDetail_ById]", dbParams, ConnStr);
        //}

        ///// <summary>
        ///// Gets On Hand, Reorder, Last price, On Order, Restock, Last Vendor and Average Price data.
        ///// </summary>
        ///// <param name="ProductCode"></param>
        ///// <param name="ConnStr"></param>
        ///// <returns></returns>
        //public bool GetStockSummary(DataSet dsStockIn, string ProductCode, string LocationCode, string ConnStr)
        //{
        //    DbParameter[] dbParams = new DbParameter[2];
        //    dbParams[0] = new DbParameter("@LocationCode", LocationCode);
        //    dbParams[1] = new DbParameter("@ProductCode", ProductCode);

        //    return DbRetrieve("[IN].[StockIn_GetStockInDtStockSummary]", dsStockIn, dbParams, this.TableName, ConnStr);
        //}

        //public bool GetList(DataSet dsOutv,int headerId,string connStr) {

        //        return false;

        //}

        //public bool Save(DataSet dsInv,string connStr) 
        //{
        //    return false;
        //}

        //public bool Delete(int stdRqstId,int itemId,string connStr) 
        //{

        //        return false;

        //}

        ////Get Schema
        //public bool GetSchema(DataSet dsStdReq, string ConnStr)
        //{
        //    return DbRetrieve("[IN].[StandardRequisitionDetail_GetSchema]", dsStdReq, null, this.TableName, ConnStr);
        //}

        ////Get New Id.
        //public int GetNewID(string connStr)
        //{
        //    return DbReadScalar("[IN].[StandardRequisitionDetail_GetNewID]", null, connStr);
        //}


        //public DataTable GetListByRefId(string RefId, string connStr)
        //{
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0] = new DbParameter("@RefId", RefId.ToString());

        //    return DbRead("[IN].[StandardRequisitionDetail_GetProductListByRefId]", dbParams, connStr);
        //}

        //public bool GetListByRefId(DataSet dsStdReq, string RefId, string connStr)
        //{
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0] = new DbParameter("@RefId", RefId.ToString());

        //    return DbRetrieve("[IN].[StandardRequisitionDetail_GetProductListByRefId]", dsStdReq, dbParams, this.TableName, connStr);
        //}
    }
}