using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class JournalVoucherComment : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public JournalVoucherComment()
        {
            SelectCommand = "SELECT * FROM GL.JournalVoucherComment";
            TableName = "JournalVoucherComment";
        }


        /// <summary>
        /// </summary>
        /// <param name="dsJournalVoucherComment"></param>
        /// <param name="journalVoucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetJournalVoucherCommentListByJournalVoucherNo(DataSet dsJournalVoucherComment,
            string journalVoucherNo, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@JournalVoucherNo", journalVoucherNo);

            // Get data
            result = DbRetrieve("GL.GetJournalVoucherCommentListByJournalVoucherNo", dsJournalVoucherComment, dbParams,
                TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get schema for journalvoucher comment.
        /// </summary>
        /// <param name="dsJournalVoucherMisc"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsJournalVoucherComment, string ConnectionString)
        {
            return DbRetrieveSchema("GL.GetJournalVoucherCommentList", dsJournalVoucherComment, null, TableName,
                ConnectionString);
        }


        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsJournalVoucher"></param>
        /// <returns></returns>
        public bool Save(DataSet dsJournalVoucherComment, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsJournalVoucherComment, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}