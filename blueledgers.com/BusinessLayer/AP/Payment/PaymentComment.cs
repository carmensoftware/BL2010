using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentComment : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentComment()
        {
            SelectCommand = "SELECT * FROM AP.PaymentComment";
            TableName = "PaymentComment";
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPaymentComment"></param>
        /// <param name="refNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentCommentListByRefNo(DataSet dsPaymentComment, string refNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefNo", refNo);

            // Get data
            var result = DbRetrieve("AP.GetPaymentCommentListByRefNo", dsPaymentComment, dbParams, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        ///     Get schema for payment comment.
        /// </summary>
        /// <param name="dsPaymentComment"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsPaymentComment, string connectionString)
        {
            return DbRetrieveSchema("AP.GetPaymentCommentList", dsPaymentComment, null, TableName, connectionString);
        }


        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsPaymentComment"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsPaymentComment, string connStr)
        {
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsPaymentComment, SelectCommand, TableName);

            // Save to database
            var result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}