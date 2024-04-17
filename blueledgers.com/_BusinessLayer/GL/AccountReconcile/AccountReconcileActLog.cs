using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class AccountReconcileActLog : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public AccountReconcileActLog()
        {
            SelectCommand = "SELECT * FROM GL.AccountReconcileActLog";
            TableName = "AccountReconcileActLog";
        }

        /// <summary>
        ///     Get account reconcile detail using accrecno
        /// </summary>
        /// <param name="AccRecNo"></param>
        /// <returns></returns>
        public bool GetListByAccRecNo(string AccRecNo, DataSet dsAccRecDetail, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@RefNo", AccRecNo);

            // Get data            
            result = DbRetrieve("GL.GetAccountReconcileActLogByRefNo", dsAccRecDetail, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get schema
        /// </summary>
        /// <param name="dsAccRecDetailDetail"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsAccRecDetail, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("GL.GetAccountReconcileActLogList", dsAccRecDetail, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Save data
        /// </summary>
        /// <param name="dsStandardVoucherActLog"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsActLog, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsActLog, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}