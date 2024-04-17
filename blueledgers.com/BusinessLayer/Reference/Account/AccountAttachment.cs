using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
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
            SelectCommand = "SELECT * FROM Reference.AccountAttachment";
            TableName = "AccountAttachment";
        }

        /// <summary>
        ///     Get account attachment list by using account number (RefNo)
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
            return DbRetrieve("Reference.GetAccountAttachmentListByRefNo", dsAccountAttachment, dbParams, this.TableName,
                ConnectionString);
        }

        /// <summary>
        ///     Get structure database of account attachment table.
        /// </summary>
        /// <param name="dsAccountAttachment"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsAccountAttachment, string ConnectionString)
        {
            // Get structure and return result
            return DbRetrieveSchema("Reference.GetAccountAttachmentList", dsAccountAttachment, null, this.TableName,
                ConnectionString);
        }

        /// <summary>
        ///     Save database to account attachment table.
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            // Build save source object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(savedData, this.SelectCommand, this.TableName);

            // ���¡ dbCommit ���� SaveSource object �� parameter
            DbCommit(dbSaveSource, connStr);

            return true;
        }

        #endregion
    }
}