using System.Data;
using Blue.DAL;

namespace Blue.BL.Option.Inventory
{
    public class UnitConv : DbHandler
    {
        public UnitConv()
        {
            SelectCommand = "SELECT * FROM [IN].[UnitConv]";
            TableName = "UnitConv";
        }

        public bool GetList(DataSet dsConvUnit, ref string MsgError, string connStr)
        {
            var result = DbRetrieve("dbo.UnitConv_GetList", dsConvUnit, null, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }


        public bool Save(DataSet dsUnitConv, ref string MsgError, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsUnitConv, SelectCommand, TableName);

            var result = DbCommit(dbSaveSource, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }
    }
}