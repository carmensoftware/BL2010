using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation
{
    public class BusinessUnitCntr : DbHandler
    {
        #region "Operations"

        public BusinessUnitCntr()
        {
            SelectCommand = "SELECT * FROM Security.BusinessUnit";
            TableName = "BusinessUnit";
        }


        /// <summary>
        ///     Get businessunit from CntrDb for update process.
        /// </summary>
        /// <param name="dsBusinessUnitCntr"></param>
        /// <returns></returns>
        public bool GetBusinessUnitCntrStructure(DataSet dsBusinessUnitCntr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Security.GetBusinessUnitList", dsBusinessUnitCntr, null, TableName);

            // return result
            return result;
        }

        /// <summary>
        ///     Save data for CntrDB.
        /// </summary>
        /// <param name="dsCity"></param>
        /// <returns></returns>
        public bool SaveCntrDB(DataSet dsBusinessUnit)
        {
            var result = false;

            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsBusinessUnit, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource);

            // Return result
            return result;
        }

        #endregion
    }
}