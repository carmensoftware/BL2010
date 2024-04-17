using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class StandardRequistion : DbHandler
    {
        public StandardRequistion()
        {
            TableName = "[IN].[StandardRequisition]";
            SelectCommand = "SELECT * FROM [IN].[StandardRequisition]";
        }

        public bool GetListByFilter(DataSet dsOpv, string refID, string storeCode, string storeName, string desc,
            string status, string connStr)
        {
            return false;
        }

        public bool GetDataById(DataSet dsOpv, int id, string connStr)
        {
            return false;
        }

        public bool Save(DataSet dsStdReq, string connStr)
        {
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsStdReq, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsStdReq, new StandardRequisitionDetail().SelectCommand,
                new StandardRequisitionDetail().TableName);
            return DbCommit(dbSaveSource, connStr);
        }

        //Get Schema
        public bool GetSchema(DataSet dsStdReq, string ConnStr)
        {
            return DbRetrieve("[IN].[StandardRequisition_GetSchema]", dsStdReq, null, TableName, ConnStr);
        }

        //Get New Id.
        public int GetNewID(string connStr)
        {
            return DbReadScalar("[IN].[StandardRequisition_GetNewID]", null, connStr);
        }

        //Get List By RefID
        public bool Get(DataSet dsStdReq, int RefId, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefId", RefId.ToString());

            return DbRetrieve("[IN].[StandardRequisition_GetListByRefId]", dsStdReq, dbParams, TableName, ConnStr);
        }

        //Delete.
        public bool Delete(DataSet dsStdReq, string connStr)
        {
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsStdReq, new StandardRequisitionDetail().SelectCommand,
                new StandardRequisitionDetail().TableName);
            dbSaveSource[1] = new DbSaveSource(dsStdReq, SelectCommand, TableName);
            return DbCommit(dbSaveSource, connStr);
        }

        public bool Get(DataSet dsStdReq, string LoginName, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LoginName", LoginName);

            return DbRetrieve("[IN].[GetStdReqForStoreReq]", dsStdReq, dbParams, TableName, ConnStr);
        }
    }
}