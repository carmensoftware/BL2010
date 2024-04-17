using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class JournalVoucherDraft : DbHandler
    {
        #region "Attibuties"

        #endregion

        #region "Operations"

        public JournalVoucherDraft()
        {
            SelectCommand = "SELECT * FROM GL.JournalVoucherDraft";
            TableName = "JournalVoucherDraft";
        }

        /// <summary>
        ///     Get journalvoucher using journalvoucher view id
        /// </summary>
        /// <param name="dsJournalVoucherDraftDraft"></param>
        public bool GetList(DataSet dsJournalVoucherDraft, string connStr)
        {
            return DbRetrieve("GL.GetJournalVoucherDraftList", dsJournalVoucherDraft, null, TableName, connStr);
        }

        /// <summary>
        ///     Get journal voucher draft data by UserID
        /// </summary>
        /// <param name="dsJournalVoucherDraft"></param>
        /// <param name="userId"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsJournalVoucherDraft, int userId, string connStr)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@CreatedBy", userId.ToString());

            return DbRetrieve("GL.GetJournalVoucherDraftListByCreatedBy", dsJournalVoucherDraft, dbParam, TableName,
                connStr);
        }

        /// <summary>
        ///     Return dataset from journalvoucherdraft.
        /// </summary>
        /// <param name="dsJournalVoucherDraftDraft"></param>
        public void GetByJouranlVoucherNo(DataSet dsJournalVoucherDraft, string journalVoucherNo, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@JournalVoucherNo", journalVoucherNo);

            var journalVoucher = new JournalVoucherDraft();
            journalVoucher.DbRetrieve("GL.GetJournalVoucherDraftListByVoucherNo", dsJournalVoucherDraft, dbParams,
                TableName, connStr);
        }

        /// <summary>
        ///     Get journalvoucherdraft table schema.
        /// </summary>
        /// <param name="journalVoucher"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsJournalVoucher, string conStr)
        {
            return DbRetrieveSchema("GL.GetJournalVoucherDraftList", dsJournalVoucher, null, TableName, conStr);
        }

        /// <summary>
        ///     Save database to journalvoucher table.
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var journalVoucherDetailDraft = new JournalVoucherDetailDraft();
            var journalVoucherMiscDraft = new JournalVoucherMiscDraft();

            var result = false;

            // สร้าง SaveSource object
            var dbSaveSource = new DbSaveSource[3];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, journalVoucherDetailDraft.SelectCommand,
                journalVoucherDetailDraft.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, journalVoucherMiscDraft.SelectCommand,
                journalVoucherMiscDraft.TableName);

            // Call dbCommit and send savesource object is parameter
            result = DbCommit(dbSaveSource, connStr);

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet savedData, string connStr)
        {
            var journalVoucherDetailDraft = new JournalVoucherDetailDraft();

            var result = false;

            // สร้าง SaveSource object
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(savedData, journalVoucherDetailDraft.SelectCommand,
                journalVoucherDetailDraft.TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, SelectCommand, TableName);

            // Call dbCommit and send savesource object is parameter
            result = DbCommit(dbSaveSource, connStr);

            return result;
        }

        #endregion
    }
}