using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class storeRequisition : DbHandler
    {
        public storeRequisition()
        {
            SelectCommand = "SELECT * FROM [IN].StoreRequisition";
            TableName = "StoreRequisition";
        }

        public bool GetStructure(DataSet dsStoreReq, string connStr)
        {
            return DbRetrieveSchema("[IN].[GetStoreReqSchema]", dsStoreReq, null, TableName, connStr);
        }

        public bool GetListByFilter(DataSet dsStoreReq, string refID, string storeCode, string storeName, string desc,
            string status, string processStatus, string connStr)
        {
            //Create Parameters
            var dbParams = new DbParameter[6];
            dbParams[0] = new DbParameter("@RefId", refID);
            dbParams[1] = new DbParameter("@LocationCode", storeCode);
            dbParams[2] = new DbParameter("@LocationName", storeName);
            dbParams[3] = new DbParameter("@Desc", desc);
            dbParams[4] = new DbParameter("@Status", status);
            dbParams[5] = new DbParameter("@ProcessStatus", processStatus);

            //Get Data
            return DbRetrieve("[IN].[GetStoreReqListByFilter]", dsStoreReq, dbParams, TableName, connStr);
        }

        public bool GetListById(DataSet dsStoreReq, int id, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefId", id.ToString());

            return DbRetrieve("[IN].[GetStoreReqListByRefId]", dsStoreReq, dbParams, TableName, connStr);
        }

        public int GetLastID(string connStr)
        {
            return DbReadScalar("[IN].[GetStoreReqMaxId]", null, connStr);
        }

        public string GetNewRequestCode(DateTime DocDate, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AtDate", DocDate.ToString("yyyy-MM-dd"));

            var dtGet = new DataTable();
            dtGet = DbRead("[IN].[GetStoreReqRequestCode]", dbParams, connStr);

            if (dtGet.Rows.Count > 0)
            {
                return dtGet.Rows[0][0].ToString();
            }

            return string.Empty;
        }

        public string GetNewRequestCode(DateTime DocDate, string prefix, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@AtDate", DocDate.ToString("yyyy-MM-dd"));
            dbParams[1] = new DbParameter("@Prefix", prefix);

            var dtGet = new DataTable();
            dtGet = DbRead("[IN].[GetStoreReqRequestCode]", dbParams, connStr);

            if (dtGet.Rows.Count > 0)
            {
                return dtGet.Rows[0][0].ToString();
            }

            return string.Empty;
        }


        public bool GetCompleteStoreReqList(DataSet dsStoreReq, string connStr)
        {
            return DbRetrieve("[IN].[GetCompleteStoreReq]", dsStoreReq, null, TableName, connStr);
        }

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