using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ProfileActiveLog : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ProfileActiveLog()
        {
            SelectCommand = "SELECT * FROM AR.ProfileActLog";
            TableName = "ProfileActLog";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsDebtorActLog"></param>
        /// <param name="debtorCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetByCustomerCode(DataSet dsProfileActLog, string customerCode, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CustomerCode", customerCode);

            // Get data
            result = DbRetrieve("AR.GetProfileActLogByCustomerCode", dsProfileActLog, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get Schema.
        /// </summary>
        /// <param name="dsDebtorActLog"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsProfileActLog, string ConnectionString)
        {
            return DbRetrieveSchema("AR.GetProfileActLogList", dsProfileActLog, null, TableName, ConnectionString);
        }

        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsVendor"></param>
        /// <returns></returns>
        public bool Save(DataSet dsProfileActLog, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsProfileActLog, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}