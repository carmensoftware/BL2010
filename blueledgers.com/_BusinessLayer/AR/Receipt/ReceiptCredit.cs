using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ReceiptCredit : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ReceiptCredit()
        {
            SelectCommand = "SELECT * FROM AR.ReceiptCredit";
            TableName = "ReceiptCredit";
        }

        /// <summary>
        ///     Get receipt credit.
        /// </summary>
        /// <param name="dsReceiptCredit"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetReceiptCreditList(DataSet dsReceiptCredit, string connStr)
        {
            DbRetrieve("AR.GetReceiptCreditList", dsReceiptCredit, null, TableName, connStr);

            // Return result
            return dsReceiptCredit;
        }

        /// <summary>
        ///     Get receipt credit by receipt no.
        /// </summary>
        /// <param name="receiptNo"></param>
        /// <param name="dsReceiptCredit"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetReceiptCreditByReceiptNo(string receiptNo, DataSet dsReceiptCredit, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ReceiptNo", receiptNo);

            // Get data
            DbRetrieve("AR.GetReceiptCreditByReceiptNo", dsReceiptCredit, dbParams, TableName, connStr);

            // Return result
            return dsReceiptCredit;
        }

        /// <summary>
        ///     Get receipt credit.
        /// </summary>
        /// <param name="dsReceiptCredit"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetReceiptCreditStructure(DataSet dsReceiptCredit, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("AR.GetReceiptCreditList", dsReceiptCredit, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}