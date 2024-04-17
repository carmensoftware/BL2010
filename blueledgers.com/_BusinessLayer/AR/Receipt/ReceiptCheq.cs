using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ReceiptCheq : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ReceiptCheq()
        {
            SelectCommand = "SELECT * FROM AR.ReceiptCheq";
            TableName = "ReceiptCheq";
        }

        /// <summary>
        ///     Get Receipt detail list
        /// </summary>
        /// <param name="dsReceiptCheq"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetReceiptCheqList(DataSet dsReceiptCheq, string connStr)
        {
            DbRetrieve("AR.GetReceiptCheqList", dsReceiptCheq, null, TableName, connStr);

            // Return result
            return dsReceiptCheq;
        }

        /// <summary>
        ///     Get receipt cheq detail using receipt no
        /// </summary>
        /// <param name="receiptNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetReceiptCheqByReceiptNo(string receiptNo, DataSet dsReceiptCheq, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ReceiptNo", receiptNo);

            // Get data
            DbRetrieve("AR.GetReceiptCheqByReceiptNo", dsReceiptCheq, dbParams, TableName, connStr);

            // Return result
            return dsReceiptCheq;
        }

        /// <summary>
        ///     Get receipt cheq detail
        /// </summary>
        /// <param name="dsReceiptDetail"></param>
        /// <returns></returns>
        public bool GetReceiptCheqStructure(DataSet dsReceiptCheq, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("AR.GetReceiptCheqList", dsReceiptCheq, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}