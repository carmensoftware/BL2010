using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class TransferOut : DbHandler
    {
        public TransferOut()
        {
            TableName = "[IN].[TransferOut]";
            SelectCommand = "SELECT * FROM [IN].[TransferOut]";
        }

        public bool GetStructure(DataSet dsTrfOut, string connStr)
        {
            return DbRetrieveSchema("[IN].[GetTrfOutSchema]", dsTrfOut, null, TableName, connStr);
        }

        //Get List By RefId
        public bool GetList(DataSet dsTrfOut, string RefId, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefId", RefId);

            return DbRetrieve("[IN].[GetTrfOutListByRefId]", dsTrfOut, dbParams, TableName, connStr);
        }

        public string GetNewId(string connStr)
        {
            var dtGet = new DataTable();

            dtGet = DbRead("[IN].[GetTrfOutNewId]", null, connStr);

            if (dtGet.Rows.Count > 0)
            {
                return dtGet.Rows[0][0].ToString();
            }

            return string.Empty;
        }

        //Save Data
        public bool Save(DataSet dsTrfOut, string connStr)
        {
            var trfOutDt = new BL.IN.TransferOutDt();

            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsTrfOut, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsTrfOut, trfOutDt.SelectCommand, trfOutDt.TableName);
            return DbCommit(dbSaveSource, connStr);
        }

        //public bool Save(DataSet dsAdjType, string connStr)
        //{
        //    DbSaveSource[] dbSaveSource = new DbSaveSource[2];
        //    dbSaveSource[0] = new DbSaveSource(dsAdjType, this.SelectCommand, this.TableName);
        //    dbSaveSource[1] = new DbSaveSource(dsAdjType, new StockInDt().SelectCommand, new StockInDt().TableName);
        //    return DbCommit(dbSaveSource, connStr);            
        //}

        ////Get List By RefID
        //public bool Get(DataSet dsStockIn, string RefId, string ConnStr)
        //{
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0] = new DbParameter("@RefId", RefId.ToString());

        //    bool result = DbRetrieve("[IN].[StockIn_Get_RefId]", dsStockIn, dbParams, this.TableName, ConnStr);

        //    if (!result)
        //    {
        //        dsStockIn.Tables[this.TableName].PrimaryKey = GetPK(dsStockIn);
        //    }

        //    return true;
        //}

        //private DataColumn[] GetPK(DataSet dsStockIn)
        //{
        //    DataColumn[] primaryKeys = new DataColumn[1];
        //    primaryKeys[0] = dsStockIn.Tables[this.TableName].Columns["RefId"];

        //    return primaryKeys;
        //}

        //Get Schema
        //public bool GetSchema(DataSet dsAdjType, string ConnStr)
        //{
        //    return DbRetrieve("[IN].[AdjType_GetSchema]", dsAdjType, null, this.TableName, ConnStr);
        //}
    }
}