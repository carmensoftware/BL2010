using System.Data;
using Blue.DAL;

namespace Blue.BL.dbo
{
    public class BuFmt : DbHandler
    {
        public BuFmt()
        {
            SelectCommand = "SELECT * FROM [dbo].BuFmt";
            TableName = "BuFmt";
        }

        /// <summary>
        ///     Get Bu format by BuCode
        /// </summary>
        /// <param name="dsBuFmt"></param>
        /// <param name="BuCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsBuFmt, string BuCode)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BuCode", BuCode);

            return DbRetrieve("[dbo].[GetBuFmtByBuCode]", dsBuFmt, dbParams, TableName);
        }

        /// <summary>
        ///     Save Data.
        /// </summary>
        /// <param name="dsSave"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsSave)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSave, SelectCommand, TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, string.Empty);
        }
    }
}