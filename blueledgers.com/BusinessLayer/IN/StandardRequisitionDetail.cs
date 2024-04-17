using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class StandardRequisitionDetail : DbHandler
    {
        public StandardRequisitionDetail()
        {
            TableName = "[IN].[StandardRequisitionDetail]";
            SelectCommand = "SELECT * FROM [IN].[StandardRequisitionDetail]";
        }

        public bool GetList(DataSet dsOutv, int headerId, string connStr)
        {
            return false;
        }

        public bool Save(DataSet dsInv, string connStr)
        {
            return false;
        }

        public bool Delete(int stdRqstId, int itemId, string connStr)
        {
            return false;
        }

        //Get Schema
        public bool GetSchema(DataSet dsStdReq, string ConnStr)
        {
            return DbRetrieve("[IN].[StandardRequisitionDetail_GetSchema]", dsStdReq, null, TableName, ConnStr);
        }

        //Get New Id.
        public int GetNewID(string connStr)
        {
            return DbReadScalar("[IN].[StandardRequisitionDetail_GetNewID]", null, connStr);
        }

        //Get List By RefID
        public bool Get(DataSet dsStdReq, int RefId, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefId", RefId.ToString());

            return DbRetrieve("[IN].[StandardRequisitionDetail_GetListByRefId]", dsStdReq, dbParams, TableName, ConnStr);
        }

        public DataTable GetListByRefId(string RefId, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefId", RefId);

            return DbRead("[IN].[StandardRequisitionDetail_GetProductListByRefId]", dbParams, connStr);
        }

        public bool GetListByRefId(DataSet dsStdReq, string RefId, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefId", RefId);

            return DbRetrieve("[IN].[StandardRequisitionDetail_GetProductListByRefId]", dsStdReq, dbParams, TableName,
                connStr);
        }
    }
}