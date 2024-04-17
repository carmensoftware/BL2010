using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class TransferIn : DbHandler
    {
        public TransferIn()
        {
            TableName = "[IN].[TransferIn]";
            SelectCommand = "SELECT * FROM [IN].[TransferIn]";
        }

        public bool GetStructure(DataSet dsTrfIn, string connStr)
        {
            return DbRetrieveSchema("[IN].[GetTrfInSchema]", dsTrfIn, null, TableName, connStr);
        }

        //Get List By RefId
        public bool GetList(DataSet dsTrfIn, string RefId, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefId", RefId);

            return DbRetrieve("[IN].[GetTrfInListByRefId]", dsTrfIn, dbParams, TableName, connStr);
        }

        public string GetNewId(string connStr)
        {
            var dtGet = new DataTable();

            dtGet = DbRead("[IN].[GetTrfInNewId]", null, connStr);

            if (dtGet.Rows.Count > 0)
            {
                return dtGet.Rows[0][0].ToString();
            }

            return string.Empty;
        }

        //Save Data
        public bool Save(DataSet dsTrfIn, string connStr)
        {
            var trfInDt = new BL.IN.TransferInDt();

            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsTrfIn, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsTrfIn, trfInDt.SelectCommand, trfInDt.TableName);

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