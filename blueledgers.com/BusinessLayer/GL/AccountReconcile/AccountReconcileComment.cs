using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class AccountReconcileComment : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public AccountReconcileComment()
        {
            SelectCommand = "SELECT * FROM GL.AccountReconcileComment";
            TableName = "AccountReconcileComment";
        }

        /// <summary>
        ///     Get data by refno.
        /// </summary>
        /// <param name="dsComment"></param>
        /// <param name="AccRecNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListByAccRecNo(DataSet dsComment, string AccRecNo, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefNo", AccRecNo);

            // Get data
            result = DbRetrieve("GL.GetAccountReconcileCommentByRefNo", dsComment, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get schema for comment.
        /// </summary>
        /// <param name="dsComment"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsComment, string ConnectionString)
        {
            return DbRetrieveSchema("GL.GetAccountReconcileCommentList", dsComment, null, TableName, ConnectionString);
        }

        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsStandardVoucher"></param>
        /// <returns></returns>
        public bool Save(DataSet dsComment, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsComment, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}