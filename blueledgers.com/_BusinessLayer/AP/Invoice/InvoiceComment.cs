using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.AP
{
    public class InvoiceComment : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public InvoiceComment()
        {
            SelectCommand = "SELECT * FROM AP.InvoiceComment";
            TableName = "InvoiceComment";
        }


        /// <summary>
        /// </summary>
        /// <param name="dsInvoiceComment"></param>
        /// <param name="refNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceCommentListByRefNo(DataSet dsInvoiceComment, string refNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefNo", refNo);

            // Get data
            var result = DbRetrieve("AP.GetInvoiceCommentListByRefNo", dsInvoiceComment, dbParams, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        ///     Get schema for invoice comment.
        /// </summary>
        /// <param name="dsInvoiceComment"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsInvoiceComment, string connectionString)
        {
            return DbRetrieveSchema("AP.GetInvoiceCommentList", dsInvoiceComment, null, TableName, connectionString);
        }


        /// <summary>
        ///     Save to database
        /// </summary>
        /// <returns></returns>
        public bool Save(DataSet dsInvoiceComment, string connStr)
        {
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsInvoiceComment, SelectCommand, TableName);

            // Save to database
            var result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}