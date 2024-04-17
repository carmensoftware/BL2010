using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StandardVoucherAttachment : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Contructor
        /// </summary>
        public StandardVoucherAttachment()
        {
            SelectCommand = "SELECT * FROM GL.StandardVoucherAttachment";
            TableName = "StandardVoucherAttachment";
        }

        /// <summary>
        ///     Get budget attachment list by using budgetid (RefNo)
        /// </summary>
        /// <param name="dsBudgetAttachment"></param>
        /// <param name="RefNo"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsBudgetAttachment, string RefNo, string ConnectionString)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@RefNo", RefNo);

            // Get data and return result
            return DbRetrieve("GL.GetStandardVoucherAttachmentListByRefNo", dsBudgetAttachment, dbParams, TableName,
                ConnectionString);
        }

        /// <summary>
        ///     Get structure database of JournalVoucherAttachment table.
        /// </summary>
        /// <param name="dsBudgetAttachment"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsBudgetAttachment, string ConnectionString)
        {
            // Get structure and return result
            return DbRetrieveSchema("GL.GetStandardVoucherAttachmentList", dsBudgetAttachment, null, TableName,
                ConnectionString);
        }

        /// <summary>
        ///     Save database to budgetattachment table.
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