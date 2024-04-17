using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class JournalVoucherActiveLog : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public JournalVoucherActiveLog()
        {
            SelectCommand = "SELECT * FROM GL.JournalVoucherActLog";
            TableName = "JournalVoucherActLog";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsJournalVoucherComment"></param>
        /// <param name="journalVoucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetJournalVoucherActLogListByJournalVoucherNo(DataSet dsJournalVoucherActLog,
            string journalVoucherNo, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@JournalVoucherNo", journalVoucherNo);

            // Get data
            result = DbRetrieve("GL.GetJournalVoucherActLogListByJournalVoucherNo", dsJournalVoucherActLog, dbParams,
                TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get schema for journalvoucher activelog.
        /// </summary>
        /// <param name="dsJournalVoucherMisc"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsJournalVoucherActLog, string ConnectionString)
        {
            return DbRetrieveSchema("GL.GetJournalVoucherActLogList", dsJournalVoucherActLog, null, TableName,
                ConnectionString);
        }

        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsJournalVoucher"></param>
        /// <returns></returns>
        public bool Save(DataSet dsJournalVoucherActLog, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsJournalVoucherActLog, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get journalvoucher logInformation by refNo.
        /// </summary>
        /// <param name="refNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetJournalVoucherActLogByRefNo(string refNo, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefNo", refNo);


            return DbRead("GL.GetJournalVoucherActLogByRefNo", dbParams, connStr);
        }

        #endregion
    }
}