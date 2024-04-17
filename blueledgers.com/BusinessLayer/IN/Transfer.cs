using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class Transfer : DbHandler
    {
        public Transfer()
        {
            SelectCommand = "SELECT * FROM [IN].Transfer";
            TableName = "Transfer";
        }

        public bool GetStructure(DataSet dsStoreReq, string connStr)
        {
            return DbRetrieveSchema("[IN].[GetTrfSchema]", dsStoreReq, null, TableName, connStr);
        }

        //public bool GetListByFilter(DataSet dsStoreReq, string refID, string storeCode, string storeName, string desc, string status, string processStatus, string connStr)
        //{
        //    //Create Parameters
        //    DbParameter[] dbParams  = new DbParameter[6];
        //    dbParams[0]             = new DbParameter("@RefId", refID);
        //    dbParams[1]             = new DbParameter("@LocationCode", storeCode);
        //    dbParams[2]             = new DbParameter("@LocationName", storeName);
        //    dbParams[3]             = new DbParameter("@Desc", desc);
        //    dbParams[4]             = new DbParameter("@Status", status);
        //    dbParams[5]             = new DbParameter("@ProcessStatus", processStatus);

        //    //Get Data
        //    return DbRetrieve("[IN].[GetStoreReqListByFilter]", dsStoreReq, dbParams, this.TableName, connStr);

        //}

        public bool GetListById(DataSet dsStoreReq, int id, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefId", id.ToString());

            return DbRetrieve("[IN].[GetTrfListByRefId]", dsStoreReq, dbParams, TableName, connStr);
        }

        public int GetLastID(string connStr)
        {
            return DbReadScalar("[IN].[GetTrfMaxId]", null, connStr);
        }

        public string GetNewRequestCode(string connStr)
        {
            var dtGet = new DataTable();
            dtGet = DbRead("[IN].[GetTrfRequestCode]", null, connStr);

            if (dtGet.Rows.Count > 0)
            {
                return dtGet.Rows[0][0].ToString();
            }

            return string.Empty;
        }

        //public bool GetCompleteStoreReqList(DataSet dsStoreReq, string connStr)
        //{
        //    return DbRetrieve("[IN].[GetCompleteStoreReq]", dsStoreReq, null, this.TableName, connStr);
        //}

        public bool Save(DataSet dsSave, string connStr)
        {
            //BL.IN.StoreRequisitionDetail storeReqDt = new BL.IN.StoreRequisitionDetail();

            // Build savesource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSave, SelectCommand, TableName);
            //dbSaveSource[1]             = new DbSaveSource(dsSave, storeReqDt.SelectCommand, storeReqDt.TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, connStr);
        }
    }
}