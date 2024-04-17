using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ReceiptDetail : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ReceiptDetail()
        {
            SelectCommand = "SELECT * FROM AR.ReceiptDetail";
            TableName = "ReceiptDetail";
        }

        /// <summary>
        ///     Get Receipt detail list using Receipt id
        /// </summary>
        /// <param name="ReceiptNo"></param>
        /// <param name="dsReceipt"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(string receiptNo, DataSet dsReceipt, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ReceiptNo", receiptNo);

            // Get data
            result = DbRetrieve("AR.GetReceiptDetailByReceiptNo", dsReceipt, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get Receipt detail using Receipt no
        /// </summary>
        /// <param name="ReceiptNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetListByReceiptDetailNo(string receiptNo, string receiptDetailNo, string connStr)
        {
            //var result = false;
            var dsReceipt = new DataSet();
            var dbParams = new DbParameter[2];

            // Create parameter
            dbParams[0] = new DbParameter("@ReceiptNo", receiptNo);
            dbParams[1] = new DbParameter("@ReceiptDetailNo", receiptDetailNo);

            // Get data
            DbRetrieve("AR.GetReceiptDetailByReceiptDetailNo", dsReceipt, dbParams, TableName, connStr);

            // Return result
            return dsReceipt;
        }

        /// <summary>
        ///     Get ReceiptDetail
        /// </summary>
        /// <param name="dsReceiptDetail"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsReceiptDetail, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("AR.GetReceiptDetailList", dsReceiptDetail, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}