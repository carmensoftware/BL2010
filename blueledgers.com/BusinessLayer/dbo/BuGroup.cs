using System.Data;
using Blue.DAL;

namespace Blue.BL.dbo
{
    public class BuGroup : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        public BuGroup()
        {
            SelectCommand = "SELECT * FROM [dbo].[BuGroup]";
            TableName = "BuGroup";
        }

        public DataTable GetMessageConnStr(string BuGrpCode)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BuGrpCode", BuGrpCode);

            return DbRead("dbo.GetBuGroup_BuGrpCode", dbParams);
        }

        #endregion
    }
}