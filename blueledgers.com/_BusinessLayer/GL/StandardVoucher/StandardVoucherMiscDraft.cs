using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StandardVoucherMiscDraft : DbHandler
    {
        #region "Attributies"

        public string StandardVoucherNo { get; set; }

        public Guid FieldID { get; set; }

        public string Value{get;set;}

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public StandardVoucherMiscDraft()
        {
            SelectCommand = "SELECT * FROM GL.StandardVoucherMiscDraft";
            TableName = "StandardVoucherMiscDraft";
        }

        /// <summary>
        ///     Get all StandardVoucherMiscDraft data the relate to specified StandardVoucehrID.
        /// </summary>
        /// <param name="dsSDVMiscDraft"></param>
        /// <param name="SDVID"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsSDVMiscDraft, string SDVID, string ConnectionString)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@StandardVoucherID", SDVID);

            return DbRetrieve("GL.GetStandardVoucherMiscDraftListByStandardVoucherID", dsSDVMiscDraft, dbParam,
                TableName, ConnectionString);
        }

        /// <summary>
        ///     Get structure of StandardVoucherMiscDraft table.
        /// </summary>
        /// <param name="dsSDVMiscDraft"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsSDVMiscDraft, string ConnectionString)
        {
            return DbRetrieveSchema("GL.GetStandardVoucherMiscDraftList", dsSDVMiscDraft, null, TableName,
                ConnectionString);
        }

        #endregion
    }
}