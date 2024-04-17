using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ReceiptCheqDetail : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ReceiptCheqDetail()
        {
            SelectCommand = "SELECT * FROM AR.ReceiptCheqDetail";
            TableName = "ReceiptCheqDetail";
        }

        /// <summary>
        ///     Get Receipt detail list using Receipt id
        /// </summary>
        /// <param name="ReceiptNo"></param>
        /// <param name="dsReceipt"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetReceiptCheqDetailList(DataSet dsReceipt, string connStr)
        {
            DbRetrieve("AR.GetReceiptCheqDetailList", dsReceipt, null, TableName, connStr);

            // Return result
            return dsReceipt;
        }

        /// <summary>
        ///     Get receipt cheq detail using receipt no
        /// </summary>
        /// <param name="receiptNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetReceiptCheqDetailByReceiptNo(string receiptNo, DataSet dsReceiptCheqDetail, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ReceiptNo", receiptNo);

            // Get data
            DbRetrieve("AR.GetReceiptCheqDetailByReceiptNo", dsReceiptCheqDetail, dbParams, TableName, connStr);

            // Return result
            return dsReceiptCheqDetail;
        }

        /// <summary>
        ///     Get ReceiptDetail
        /// </summary>
        /// <param name="dsReceiptDetail"></param>
        /// <returns></returns>
        public bool GetReceiptCheqDetailStructure(DataSet dsReceiptCheqDetail, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("AR.GetReceiptCheqDetailList", dsReceiptCheqDetail, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get max id.
        /// </summary>
        /// <param name="receiptNo"></param>
        /// <param name="receiptCheqDetailNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetMaxNo(string receiptNo, string connStr)
        {
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ReceiptNo", receiptNo);

            // Retrieve data.
            DbRetrieve("AR.GetReceiptCheqDetailMaxID", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows[0]["MaxReceiptCheqDetailNo"].ToString() != string.Empty)
            {
                return (int) dsTmp.Tables[TableName].Rows[0]["MaxReceiptCheqDetailNo"];
            }
            return 1;
        }

        #endregion
    }
}