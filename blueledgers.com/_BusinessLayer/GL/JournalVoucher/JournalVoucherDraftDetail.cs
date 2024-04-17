using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class JournalVoucherDetailDraft : DbHandler
    {
        #region "Attibuties"

        #endregion

        #region "Operations"

        public JournalVoucherDetailDraft()
        {
            SelectCommand = "SELECT * FROM GL.JournalVoucherDetailDraft";
            TableName = "JournalVoucherDetailDraft";
        }


        /// <summary>
        ///     Get journalvoucherdraft table schema.
        /// </summary>
        /// <param name="journalVoucher"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsJournalVoucherDetailDraft, string conStr)
        {
            return DbRetrieveSchema("GL.GetJournalVoucherDetailDraftList", dsJournalVoucherDetailDraft, null, TableName,
                conStr);
        }

        /// <summary>
        ///     Get journalvoucherdetaildraft list.
        /// </summary>
        /// <param name="dsJournalVoucherDetailDraft"></param>
        /// <param name="connStr"></param>
        public void GetList(DataSet dsJournalVoucherDetailDraft, string connStr)
        {
            DbRetrieve("GL.GetJournalVoucherDetailDraftList", dsJournalVoucherDetailDraft, null, TableName, connStr);
        }

        /// <summary>
        ///     Return dataset from journalvoucher detail by journal voucherno.
        /// </summary>
        /// <param name="dsJournalVoucher"></param>
        /// <param name="journalVoucherNo"></param>
        /// <param name="connStr"></param>
        public void GetByVoucherNo(DataSet dsJournalVoucherDetailDraft, string JVNo, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@JournalVoucherNo", JVNo);

            //JournalVoucherDetailDraft JVDetailDraft = new JournalVoucherDetailDraft();
            //JVDetailDraft.
            DbRetrieve("GL.GetJournalVoucherDetailDraftListByVoucherNo", dsJournalVoucherDetailDraft, dbParams,
                TableName, connStr);
        }

        #endregion
    }
}