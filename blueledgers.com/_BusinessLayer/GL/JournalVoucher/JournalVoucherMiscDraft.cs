using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class JournalVoucherMiscDraft : DbHandler
    {
        #region "Attributies"

        public string JournalVoucherNo { get; set; }

        public Guid FieldID { get; set; }

        public string Value{get;set;}

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public JournalVoucherMiscDraft()
        {
            SelectCommand = "SELECT * FROM GL.JournalVoucherMiscDraft";
            TableName = "JournalVoucherMiscDraft";
        }

        /// <summary>
        ///     Get all JournalVoucerMiscDraft data the relate to specified JournalVoucherNo.
        /// </summary>
        /// <param name="dsJVMiscDraft"></param>
        /// <param name="JVNo"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsJVMiscDraft, string JVNo, string ConnectionString)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@JournalVoucherNo", JVNo);

            return DbRetrieve("GL.GetJournalVoucherMiscDraftListByJournalVoucherNo", dsJVMiscDraft, dbParam, TableName,
                ConnectionString);
        }

        /// <summary>
        ///     Get structure of JournalVoucherMiscDraft table.
        /// </summary>
        /// <param name="dsJVMiscDraft"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsJVMiscDraft, string ConnectionString)
        {
            return DbRetrieveSchema("GL.GetJournalVoucherMiscDraftList", dsJVMiscDraft, null, TableName,
                ConnectionString);
        }

        #endregion
    }
}