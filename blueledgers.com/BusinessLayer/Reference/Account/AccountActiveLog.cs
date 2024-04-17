using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class AccountActiveLog : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public AccountActiveLog()
        {
            SelectCommand = "SELECT * FROM Reference.AccountActLog";
            TableName = "AccountActLog";
        }


        /// <summary>
        ///     Get all of account voucher except recurring type
        /// </summary>
        /// <param name="dsAccount"></param>
        /// <returns></returns>
        public bool GetAccountActLogListByAccountCode(DataSet dsAccountActLog, string accountCode, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AccountCode", accountCode);

            // Get data
            result = DbRetrieve("Reference.GetAccountActLogListByAccountCode", dsAccountActLog, dbParams, TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get schema for accountvoucher activelog.
        /// </summary>
        /// <param name="dsAccountMisc"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsAccountActLog, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("Reference.GetAccountActLogList", dsAccountActLog, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsAccount"></param>
        /// <returns></returns>
        public bool Save(DataSet dsAccountActLog, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsAccountActLog, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}