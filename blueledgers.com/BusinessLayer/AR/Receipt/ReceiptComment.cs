using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ReceiptComment : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ReceiptComment()
        {
            SelectCommand = "SELECT * FROM AR.ReceiptComment";
            TableName = "ReceiptComment";
        }

        /// <summary>
        ///     Get data by ref no.
        /// </summary>
        /// <param name="dsReceiptComment"></param>
        /// <param name="RefNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetReceiptCommentListByRefNo(DataSet dsReceiptComment, string RefNo, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefNo", RefNo);

            // Get data
            result = DbRetrieve("AR.GetReceiptCommentListByRefNo", dsReceiptComment, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get schema for Receipt comment.
        /// </summary>
        /// <param name="dsReceiptComment"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsReceiptComment, string ConnectionString)
        {
            return DbRetrieveSchema("AR.GetReceiptCommentList", dsReceiptComment, null, TableName, ConnectionString);
        }

        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsReceiptComment"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsReceiptComment, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsReceiptComment, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}