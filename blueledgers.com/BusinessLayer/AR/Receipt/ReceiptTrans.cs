using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ReceiptTrans : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ReceiptTrans()
        {
            SelectCommand = "SELECT * FROM AR.ReceiptTrans";
            TableName = "ReceiptTrans";
        }

        /// <summary>
        ///     Get Receipt trans list
        /// </summary>
        /// <param name="dsReceiptCheq"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetReceiptTransList(DataSet dsReceiptTrans, string connStr)
        {
            DbRetrieve("AR.GetReceiptTrans", dsReceiptTrans, null, TableName, connStr);

            // Return result
            return dsReceiptTrans;
        }

        /// <summary>
        ///     Get receipt trans using receipt no
        /// </summary>
        /// <param name="receiptNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetReceiptTransByReceiptNo(string receiptNo, DataSet dsReceiptTrans, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ReceiptNo", receiptNo);

            // Get data
            DbRetrieve("AR.GetReceiptTransByReceiptNo", dsReceiptTrans, dbParams, TableName, connStr);

            // Return result
            return dsReceiptTrans;
        }

        /// <summary>
        ///     Get receipt trans
        /// </summary>
        /// <param name="dsReceiptDetail"></param>
        /// <returns></returns>
        public bool GetReceiptTransStructure(DataSet dsReceiptTrans, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("AR.GetReceiptTrans", dsReceiptTrans, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}