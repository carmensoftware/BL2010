using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StandardVoucherDetailDraft : DbHandler
    {
        #region "Attibuties"

        #endregion

        #region "Operations"

        public StandardVoucherDetailDraft()
        {
            SelectCommand = "SELECT * FROM GL.StandardVoucherDetailDraft";
            TableName = "StandardVoucherDetailDraft";
        }

        /// <summary>
        ///     Get journalvoucherdraft table schema.
        /// </summary>
        /// <param name="journalVoucher"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsStandardVoucherDetailDraft, string conStr)
        {
            return DbRetrieveSchema("GL.GetStandardVoucherDetailDraftList", dsStandardVoucherDetailDraft, null,
                TableName, conStr);
        }

        /// <summary>
        ///     Get journalvoucherdetaildraft list.
        /// </summary>
        /// <param name="dsStandardVoucherDetailDraft"></param>
        /// <param name="connStr"></param>
        public void GetList(DataSet dsStandardVoucherDetailDraft, string connStr)
        {
            DbRetrieve("GL.GetStandardVoucherDetailDraftList", dsStandardVoucherDetailDraft, null, TableName, connStr);
        }

        /// <summary>
        ///     Get data by ID.
        /// </summary>
        /// <param name="dsSDVDetailDraft"></param>
        /// <param name="sdvID"></param>
        /// <param name="connStr"></param>
        public void GetByID(DataSet dsSDVDetailDraft, string sdvID, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@StandardVoucherID", sdvID);

            DbRetrieve("GL.GetStandardVoucherDetailDraftByID", dsSDVDetailDraft, dbParams, TableName, connStr);
        }

        #endregion
    }
}