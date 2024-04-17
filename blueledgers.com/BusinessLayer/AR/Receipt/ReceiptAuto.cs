using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ReceiptAuto : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ReceiptAuto()
        {
            SelectCommand = "SELECT * FROM AR.ReceiptAuto";
            TableName = "ReceiptAuto";
        }

        /// <summary>
        ///     Get ReceiptAuto
        /// </summary>
        /// <param name="dsReceiptAuto"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetReceiptAutoList(DataSet dsReceiptAuto, string connStr)
        {
            DbRetrieve("AR.GetReceiptAuto", dsReceiptAuto, null, TableName, connStr);

            // Return result
            return dsReceiptAuto;
        }

        /// <summary>
        ///     Get receiptauto using receipt no
        /// </summary>
        /// <param name="receiptNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetReceiptAutoByReceiptNo(string receiptNo, DataSet dsReceiptAuto, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ReceiptNo", receiptNo);

            // Get data
            DbRetrieve("AR.GetReceiptAutoByReceiptNo", dsReceiptAuto, dbParams, TableName, connStr);

            // Return result
            return dsReceiptAuto;
        }

        /// <summary>
        ///     Get receipt auto
        /// </summary>
        /// <param name="dsReceiptDetail"></param>
        /// <returns></returns>
        public bool GetReceiptAutoStructure(DataSet dsReceiptAuto, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("AR.GetReceiptAuto", dsReceiptAuto, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}