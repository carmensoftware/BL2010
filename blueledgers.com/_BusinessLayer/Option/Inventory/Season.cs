using System.Data;
using Blue.DAL;

namespace Blue.BL.Option.Inventory
{
    public class Season : DbHandler
    {
        public Season()
        {
            SelectCommand = "SELECT * FROM [In].[Season]";
            TableName = "Season";
        }

        /// <summary>
        ///     Gets all season data.
        /// </summary>
        /// <param name="dsSeason"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsSeason, string connStr)
        {
            var result = DbRetrieve("dbo.Season_GetList", dsSeason, null, TableName, connStr);
            if (result)
            {
                // Create primarykey                
                dsSeason.Tables[TableName].PrimaryKey = GetPK(dsSeason);
            }

            return result;
        }

        /// <summary>
        ///     Get Lookup UnitCode.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            // Create parameters
            return DbRead("dbo.Season_GetList", null, connStr);
        }

        /// <summary>
        ///     Commit changed to database.
        /// </summary>
        /// <param name="dsPrefix"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsSeason, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSeason, SelectCommand, TableName);
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        ///     Create primary keys for season DataTable from specified DataSet.
        /// </summary>
        /// <param name="dsSeason"></param>
        /// <returns></returns>
        private DataColumn[] GetPK(DataSet dsSeason)
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsSeason.Tables[TableName].Columns["SeasonCode"];

            return primaryKeys;
        }
    }
}