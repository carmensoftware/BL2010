using System.Data;
using Blue.DAL;

namespace Blue.BL.SB
{
    public class Subscriber : DbHandler
    {
        public Subscriber()
        {
            TableName = "[sb].[Subscriber]";
            SelectCommand = "SELECT * FROM [sb].[Subscriber]";
        }

        public bool GetSchema(DataSet dsSubscribe, string ConnStr)
        {
            // return DbRetrieve("[IN].[StandardRequisitionDetail_GetSchema]", dsStdReq, null, this.TableName, ConnStr);
            return DbRetrieve("[SB].[Subscriber_GetSchema]", dsSubscribe, null, TableName, ConnStr);
        }

        public bool Save(DataSet dsSubscribe, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSubscribe, SelectCommand, TableName);
            return DbCommit(dbSaveSource, connStr);
        }
    }
}