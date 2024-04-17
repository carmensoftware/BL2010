using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class CADt : DbHandler
    {
        public CADt()
        {
            SelectCommand = "SELECT * FROM CADt";
            TableName = "CADt";
        }

        public bool GetSchema(DataSet dsCADt, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefNo", string.Empty);

            return DbRetrieveSchema("[IN].GetCADt_RefNo", dsCADt, dbParams, TableName, ConnStr);
        }
    }
}