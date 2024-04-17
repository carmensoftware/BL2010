using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.Setup.Application
{
    public class AccountAttachment : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Contructor
        /// </summary>
        public AccountAttachment()
        {
            SelectCommand = "SELECT * FROM [Application].AccountAttachment";
            TableName = "AccountAttachment";
        }

        /// <summary>
        ///     Get budget attachment list by using budgetid (RefNo)
        /// </summary>
        /// <param name="dsAccountAttachment"></param>
        /// <param name="RefNo"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAccountAttachment, string RefNo, string ConnectionString)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@RefNo", RefNo);

            // Get data and return result
            return DbRetrieve("[Application].GetAccountAttachmentListByRefNo", dsAccountAttachment, dbParams, TableName,
                ConnectionString);
        }

        /// <summary>
        ///     Get structure database of AccountAttachment table.
        /// </summary>
        /// <param name="dsAccountAttachment"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsAccountAttachment, string ConnectionString)
        {
            // Get structure and return result
            return DbRetrieveSchema("[Application].GetAccountAttachmentList", dsAccountAttachment, null, TableName,
                ConnectionString);
        }

        /// <summary>
        ///     Save database to AccountAttachment table.
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