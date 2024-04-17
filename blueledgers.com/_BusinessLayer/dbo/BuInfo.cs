using System.Data;
using Blue.DAL;

namespace Blue.BL.dbo
{
    public class BuInfo : DbHandler
    {
        public BuInfo()
        {
            SelectCommand = "SELECT * FROM [dbo].[BuInfo]";
            TableName = "BuInfo";
        }

        /// <summary>
        ///     Get BuInfo By BuCode.
        /// </summary>
        /// <param name="dsBuInfo"></param>
        /// <param name="BuCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsBuInfo, string BuCode)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BuCode", BuCode);

            return DbRetrieve("[dbo].[GetBuInfoByBuCode]", dsBuInfo, dbParams, TableName);
        }

        /// <summary>
        ///     Save Data to [dbo].BuInfo, [dbo].Bu and [dbo].BuFmt
        /// </summary>
        /// <param name="dsBuInfo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsSave)
        {
            var bu = new BL.dbo.Bu();
            var buFmt = new BL.dbo.BuFmt();

            var dbSaveSource = new DbSaveSource[3];

            dbSaveSource[0] = new DbSaveSource(dsSave, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsSave, bu.SelectCommand, bu.TableName);
            dbSaveSource[2] = new DbSaveSource(dsSave, buFmt.SelectCommand, buFmt.TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, string.Empty);
        }
    }
}