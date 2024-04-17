using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class JournalVoucherAttachment : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Contructor
        /// </summary>
        public JournalVoucherAttachment()
        {
            SelectCommand = "SELECT * FROM GL.JournalVoucherAttachment";
            TableName = "JournalVoucherAttachment";
        }

        /// <summary>
        ///     Get journal voucher attachment list by using journalvoucher number (RefNo)
        /// </summary>
        /// <param name="dsJournalVoucherAttachment"></param>
        /// <param name="RefNo">JournalVoucher Number</param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsJournalVoucherAttachment, string RefNo, string ConnectionString)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@RefNo", RefNo);

            // Get data and return result
            return DbRetrieve("GL.GetJournalVoucherAttachmentListByRefNo", dsJournalVoucherAttachment, dbParams,
                TableName, ConnectionString);
        }

        /// <summary>
        ///     Get structure database of JournalVoucherAttachment table.
        /// </summary>
        /// <param name="dsJournalVoucherAttachment"></param>
        /// <param name="RefNo"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsJournalVoucherAttachment, string ConnectionString)
        {
            // Get structure and return result
            return DbRetrieveSchema("GL.GetJournalVoucherAttachmentList", dsJournalVoucherAttachment,
                null, TableName, ConnectionString);
        }

        /// <summary>
        ///     Save database to journalvoucherattachment table.
        /// </summary>
        /// <param name="savedData"></param>
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