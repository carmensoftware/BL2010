using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ReceiptCash : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ReceiptCash()
        {
            SelectCommand = "SELECT * FROM AR.ReceiptCash";
            TableName = "ReceiptCash";
        }

        /// <summary>
        ///     Get Receipt detail list
        /// </summary>
        /// <param name="dsReceiptCheq"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetReceiptCashList(DataSet dsReceiptCash, string connStr)
        {
            DbRetrieve("AR.GetReceiptCash", dsReceiptCash, null, TableName, connStr);

            // Return result
            return dsReceiptCash;
        }

        /// <summary>
        ///     Get receipt cash using receipt no
        /// </summary>
        /// <param name="receiptNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetReceiptCashByReceiptNo(string receiptNo, DataSet dsReceiptCash, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ReceiptNo", receiptNo);

            // Get data
            DbRetrieve("AR.GetReceiptCashByReceiptNo", dsReceiptCash, dbParams, TableName, connStr);

            // Return result
            return dsReceiptCash;
        }

        /// <summary>
        ///     Get receipt cash
        /// </summary>
        /// <param name="dsReceiptDetail"></param>
        /// <returns></returns>
        public bool GetReceiptCashStructure(DataSet dsReceiptCash, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("AR.GetReceiptCash", dsReceiptCash, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}