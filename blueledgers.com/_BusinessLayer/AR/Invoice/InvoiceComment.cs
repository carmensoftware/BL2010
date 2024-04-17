using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
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
            SelectCommand = "SELECT * FROM AR.InvoiceComment";
            TableName = "InvoiceComment";
        }

        /// <summary>
        ///     Get invoice comment by ref no.
        /// </summary>
        /// <param name="dsInvoiceComment"></param>
        /// <param name="refNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListByRefNo(DataSet dsInvoiceComment, string refNo, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefNo", refNo);

            // Get data
            result = DbRetrieve("AR.GetInvoiceCommentListByRefNo", dsInvoiceComment, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get schema for invoice comment.
        /// </summary>
        /// <param name="dsInvoiceMisc"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsInvoiceComment, string ConnectionString)
        {
            return DbRetrieveSchema("AR.GetInvoiceCommentList", dsInvoiceComment, null, TableName, ConnectionString);
        }

        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsInvoice"></param>
        /// <returns></returns>
        public bool Save(DataSet dsInvoiceComment, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsInvoiceComment, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}