using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ProfileAttachment : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Contructor
        /// </summary>
        public ProfileAttachment()
        {
            SelectCommand = "SELECT * FROM AR.ProfileAttachment";
            TableName = "ProfileAttachment";
        }

        /// <summary>
        ///     Get budget attachment list by using RefNo
        /// </summary>
        /// <param name="dsDebtorAttachment"></param>
        /// <param name="RefNo"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsDebtorAttachment, string RefNo, string ConnectionString)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@RefNo", RefNo);

            // Get data and return result
            return DbRetrieve("AR.GetProfileAttachmentByRefNo", dsDebtorAttachment, dbParams, TableName,
                ConnectionString);
        }

        /// <summary>
        ///     Get structure database of debtor attachment table.
        /// </summary>
        /// <param name="dsDebtorAttachment"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsDebtorAttachment, string ConnectionString)
        {
            // Get structure and return result
            return DbRetrieveSchema("AR.GetProfileAttachmentList", dsDebtorAttachment, null, TableName, ConnectionString);
        }

        /// <summary>
        ///     Save database to debtorattachment table.
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