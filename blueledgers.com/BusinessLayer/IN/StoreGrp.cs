using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class StoreGrp : DbHandler
    {
        public StoreGrp()
        {
            SelectCommand = "SELECT * FROM [IN].[StoreGrp]";
            TableName = "StoreGrp";
        }

        public DataTable GetList(string connStr)
        {
            return DbRead("[IN].[GetStoreGrpList]", null, connStr);
        }
    }
}