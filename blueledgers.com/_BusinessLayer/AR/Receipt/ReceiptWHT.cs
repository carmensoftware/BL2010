using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ReceiptWHT : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ReceiptWHT()
        {
            SelectCommand = "SELECT * FROM AR.ReceiptWHT";
            TableName = "ReceiptWHT";
        }

        /// <summary>
        ///     Get ReceiptWHT
        /// </summary>
        /// <param name="dsReceiptWHT"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetReceiptWHTList(DataSet dsReceiptWHT, string connStr)
        {
            DbRetrieve("AR.GetReceiptWHT", dsReceiptWHT, null, TableName, connStr);

            // Return result
            return dsReceiptWHT;
        }

        /// <summary>
        ///     Get receiptWHT using receipt no
        /// </summary>
        /// <param name="receiptNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetReceiptWHTByReceiptNo(string receiptNo, DataSet dsReceiptWHT, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ReceiptNo", receiptNo);

            // Get data
            DbRetrieve("AR.GetReceiptWHTByReceiptNo", dsReceiptWHT, dbParams, TableName, connStr);

            // Return result
            return dsReceiptWHT;
        }

        /// <summary>
        ///     Get receiptwht
        /// </summary>
        /// <param name="dsReceiptDetail"></param>
        /// <returns></returns>
        public bool GetReceiptWHTStructure(DataSet dsReceiptWHT, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("AR.GetReceiptWHT", dsReceiptWHT, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}