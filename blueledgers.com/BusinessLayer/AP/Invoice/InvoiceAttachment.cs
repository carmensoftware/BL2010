using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.AP
{
    public class InvoiceAttachment : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Contructor
        /// </summary>
        public InvoiceAttachment()
        {
            SelectCommand = "SELECT * FROM AP.InvoiceAttachment";
            TableName = "InvoiceAttachment";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsAttachment"></param>
        /// <param name="refNo"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAttachment, string refNo, string connectionString)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@RefNo", refNo);

            // Get data and return result
            return DbRetrieve("AP.GetInvoiceAttachmentListByRefNo", dsAttachment, dbParams, TableName, connectionString);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsAttachment"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsAttachment, string connectionString)
        {
            // Get structure and return result
            return DbRetrieveSchema("AP.GetInvoiceAttachmentList", dsAttachment, null, TableName, connectionString);
        }

        /// <summary>
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            // Build save source object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);

            // ���¡ dbCommit ���� SaveSource object �� parameter
            DbCommit(dbSaveSource, connStr);

            return true;
        }

        #endregion
    }
}