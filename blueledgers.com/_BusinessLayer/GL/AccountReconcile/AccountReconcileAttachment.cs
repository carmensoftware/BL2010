using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class AccountReconcileAttachment : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Contructor
        /// </summary>
        public AccountReconcileAttachment()
        {
            SelectCommand = "SELECT * FROM GL.AccountReconcileAttachment";
            TableName = "AccountReconcileAttachment";
        }

        /// <summary>
        ///     Get attachment list by using refno
        /// </summary>
        /// <param name="dsAttachment"></param>
        /// <param name="RefNo"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetListByAccRecNo(DataSet dsAttachment, string AccRecNo, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@RefNo", AccRecNo);

            // Get data and return result
            return DbRetrieve("GL.GetAccountReconcileAttachmentByRefNo", dsAttachment, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get structure database
        /// </summary>
        /// <param name="dsAttachment"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsAttachment, string connStr)
        {
            // Get structure and return result
            return DbRetrieveSchema("GL.GetAccountReconcileAttachmentList", dsAttachment, null, TableName, connStr);
        }

        /// <summary>
        ///     Save database to attachment table.
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            // Build save source object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);

            // Call dbCommit send by SaveSource object �� parameter
            DbCommit(dbSaveSource, connStr);

            return true;
        }

        #endregion
    }
}