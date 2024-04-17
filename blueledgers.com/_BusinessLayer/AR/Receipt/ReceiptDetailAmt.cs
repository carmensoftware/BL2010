using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ReceiptDetailAmt : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ReceiptDetailAmt()
        {
            SelectCommand = "SELECT * FROM AR.ReceiptDetailAmt";
            TableName = "ReceiptDetailAmt";
        }

        /// <summary>
        ///     Get Receipt detail list using Receipt id
        /// </summary>
        /// <param name="standardVoucherID"></param>
        /// <param name="dsReceipt"></param>
        /// <returns></returns>
        public bool GetReceiptDetailAmtList(DataSet dsReceipt, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("AR.GetReceiptDetailAmtList", dsReceipt, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetReceiptDetailAmtByReceiptDetailNo(DataSet dsReceipt, string receiptNo, int receiptDetailNo,
            string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@ReceiptNo", receiptNo);
            dbParams[1] = new DbParameter("@ReceiptDetailNo", Convert.ToString(receiptDetailNo));

            DbRetrieve("AR.GetReceiptDetailAmtByReceiptDetailNo", dsReceipt, dbParams, TableName, connStr);

            return dsReceipt;
        }

        /// <summary>
        ///     Get Receipt detail amt using Receipt no.
        /// </summary>
        /// <param name="ReceiptNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetReceiptDetailAmtByReceiptNo(string receiptNo, string connStr)
        {
            var dtReceiptDetailAmt = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ReceiptNo", receiptNo);

            // Get data
            dtReceiptDetailAmt = DbRead("AR.GetReceiptDetailAmtByReceiptNo", dbParams, connStr);

            // Return result
            return dtReceiptDetailAmt;
        }

        /// <summary>
        ///     Get ReceiptDetailAmt
        /// </summary>
        /// <param name="dsReceiptDetailAmt"></param>
        /// <returns></returns>
        public bool GetReceiptDetailAmtStructure(DataSet dsReceiptDetailAmt, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("AR.GetReceiptDetailAmtList", dsReceiptDetailAmt, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get max no.
        /// </summary>
        /// <param name="ReceiptNo"></param>
        /// <param name="ReceiptDetailNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetMaxNo(string receiptNo, int receiptDetailNo, string connStr)
        {
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@ReceiptNo", receiptNo);
            dbParams[1] = new DbParameter("@ReceiptDetailNo", Convert.ToString(receiptDetailNo));

            // Retrieve data.
            DbRetrieve("AR.GetReceiptDetailAmtMaxID", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows[0]["MaxReceiptDetailAmtNo"].ToString() != string.Empty)
            {
                return (int) dsTmp.Tables[TableName].Rows[0]["MaxReceiptDetailAmtNo"];
            }
            return 1;
        }

        #endregion
    }
}