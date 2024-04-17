using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StandardVoucherComment : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public StandardVoucherComment()
        {
            SelectCommand = "SELECT * FROM GL.StandardVoucherComment";
            TableName = "StandardVoucherComment";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsStandardVoucherComment"></param>
        /// <param name="standardvoucherid"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetStandardVoucherCommentListByStandardVoucherID(DataSet dsStandardVoucherComment,
            int standardvoucherid, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@StandardVoucherID", Convert.ToString(standardvoucherid));

            // Get data
            result = DbRetrieve("GL.GetStandardVoucherCommentListByStandardVoucherID", dsStandardVoucherComment,
                dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get schema for standardvoucher comment.
        /// </summary>
        /// <param name="dsStandardVoucherMisc"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsStandardVoucherComment, string ConnectionString)
        {
            return DbRetrieveSchema("GL.GetStandardVoucherCommentList", dsStandardVoucherComment, null, TableName,
                ConnectionString);
        }

        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsStandardVoucher"></param>
        /// <returns></returns>
        public bool Save(DataSet dsStandardVoucherComment, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsStandardVoucherComment, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}