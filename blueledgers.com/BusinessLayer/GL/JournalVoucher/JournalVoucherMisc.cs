using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class JournalVoucherMisc : DbHandler
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
        public JournalVoucherMisc()
        {
            SelectCommand = "SELECT * FROM GL.JournalVoucherMisc";
            TableName = "JournalVoucherMisc";
        }

        /// <summary>
        ///     Get all JournalVoucerMisc data the relate to specified JournalVoucherNo.
        /// </summary>
        /// <param name="dsJournalVoucherMisc"></param>
        /// <param name="JournalVoucherNo"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsJournalVoucherMisc, string JournalVoucherNo, string ConnectionString)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@JournalVoucherNo", JournalVoucherNo);

            return DbRetrieve("GL.GetJournalVoucherMiscListByJournalVoucherNo", dsJournalVoucherMisc, dbParam, TableName,
                ConnectionString);
        }

        /// <summary>
        ///     Get structure of JournalVoucherMisc table.
        /// </summary>
        /// <param name="dsJournalVoucherMisc"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsJournalVoucherMisc, string ConnectionString)
        {
            return DbRetrieveSchema("GL.GetJournalVoucherMiscList", dsJournalVoucherMisc, null, TableName,
                ConnectionString);
        }

        #endregion
    }
}