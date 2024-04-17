using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class StoreRequisitionDetail : DbHandler
    {
        public StoreRequisitionDetail()
        {
            var SR = new storeRequisition();
            SelectCommand = "SELECT * FROM [IN].StoreRequisitionDetail";
            TableName = "StoreRequisitionDetail";
        }

        public bool GetStructure(DataSet dsStoreReqDt, string connStr)
        {
            return DbRetrieveSchema("[IN].[GetStoreReqDtSchema]", dsStoreReqDt, null, TableName, connStr);
        }

        public bool GetListByHeaderId(DataSet dsStoreReqDt, int DocumentId, string connStr)
        {
            //Create Parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DocumentId", DocumentId.ToString());

            //Get Data
            return DbRetrieve("[IN].GetStoreReqDtByStoreReqId", dsStoreReqDt, dbParams, TableName, connStr);
        }

        public bool GetDetailByItemId(DataSet dsStoreReqDt, int DocumentId, int itemId, string connStr)
        {
            //Create Parameters
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@DocumentID", DocumentId.ToString());
            dbParams[1] = new DbParameter("@RefId", itemId.ToString());

            //Get Data
            return DbRetrieve("[IN].[GetStoreReqDtByStoreReqIdAndStoreReqDtId]", dsStoreReqDt, dbParams, TableName,
                connStr);
        }

        public bool Save(DataSet dsSave, string connStr)
        {
            // Build savesource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSave, SelectCommand, TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, connStr);
        }

        public bool Delete(int stdRqstId, int itemId, string connStr)
        {
            return false;
        }
    }
}