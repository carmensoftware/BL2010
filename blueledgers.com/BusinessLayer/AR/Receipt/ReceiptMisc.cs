using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ReceiptMisc : DbHandler
    {
        #region "Attributies"

        public int VoucherNo { get; set; }
        public Guid FieldID { get; set; }
        public string Value { get; set; }

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public ReceiptMisc()
        {
            SelectCommand = "SELECT * FROM AR.ReceiptMisc";
            TableName = "ReceiptMisc";
        }

        /// <summary>
        ///     Get all of Receipt Misc.
        /// </summary>
        /// <param name="dsReceiptMisc"></param>
        /// <param name="ReceiptNo"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsReceiptMisc, string receiptNo, string ConnectionString)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@ReceiptNo", receiptNo);

            return DbRetrieve("AR.GetReceiptMiscListByReceiptNo", dsReceiptMisc, dbParam, TableName, ConnectionString);
        }

        /// <summary>
        ///     Get schema for Receipt misc.
        /// </summary>
        /// <param name="dsReceiptMisc"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsReceiptMisc, string ConnectionString)
        {
            return DbRetrieveSchema("AR.GetReceiptMiscList", dsReceiptMisc, null, TableName, ConnectionString);
        }

        #endregion
    }
}