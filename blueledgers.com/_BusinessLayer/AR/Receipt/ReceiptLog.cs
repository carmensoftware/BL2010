using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ReceiptLog : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ReceiptLog()
        {
            SelectCommand = "SELECT * FROM AR.ReceiptLog";
            TableName = "ReceiptLog";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsReceiptComment"></param>
        /// <param name="refNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetReceiptLogListByRefNo(DataSet dsReceiptLog, string refNo, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefNo", refNo);

            // Get data
            result = DbRetrieve("AR.GetReceiptLogListByRefNo", dsReceiptLog, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get schema for Receipt activelog.
        /// </summary>
        /// <param name="dsReceiptMisc"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsReceiptLog, string ConnectionString)
        {
            return DbRetrieveSchema("AR.GetReceiptLogList", dsReceiptLog, null, TableName, ConnectionString);
        }

        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsReceipt"></param>
        /// <returns></returns>
        public bool Save(DataSet dsReceiptLog, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsReceiptLog, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}