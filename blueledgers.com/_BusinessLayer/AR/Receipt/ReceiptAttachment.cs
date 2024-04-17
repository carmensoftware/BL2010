using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ReceiptAttachment : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Contructor
        /// </summary>
        public ReceiptAttachment()
        {
            SelectCommand = "SELECT * FROM AR.ReceiptAttachment";
            TableName = "ReceiptAttachment";
        }

        /// <summary>
        ///     Get data list by refno.
        /// </summary>
        /// <param name="dsAttachment"></param>
        /// <param name="ID"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAttachment, string refNo, string ConnectionString)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@RefNo", refNo);

            // Get data and return result
            return DbRetrieve("AR.GetReceiptAttachmentListByRefNo", dsAttachment, dbParams, TableName, ConnectionString);
        }

        /// <summary>
        ///     Get structure.
        /// </summary>
        /// <param name="dsAttachment"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsAttachment, string ConnectionString)
        {
            // Get structure and return result
            return DbRetrieveSchema("AR.GetReceiptAttachmentList", dsAttachment, null, TableName, ConnectionString);
        }

        /// <summary>
        ///     Save data
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