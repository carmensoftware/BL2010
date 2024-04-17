using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class PaymentType : DbHandler
    {
        #region "Attributies"

/*
        private ReceiptDetail _receiptDetail = new ReceiptDetail();
        private ReceiptMisc _receiptMisc = new ReceiptMisc();
*/

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentType()
        {
            SelectCommand = "SELECT * FROM AR.PaymentType";
            TableName = "PaymentTypet";
        }

        /// <summary>
        ///     Get payment type.
        /// </summary>
        /// <param name="ReceiptNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetPaymentTypeList(DataSet dsPaymentType, string IsActive, string connStr)
        {
            // Create parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@IsActive", IsActive);

            DbRetrieve("AR.GetPaymentTypeByIsActive", dsPaymentType, dbParams, TableName, connStr);

            // Return result
            return dsPaymentType;
        }

        /// <summary>
        ///     Get data to lookup.
        /// </summary>
        /// <param name="IsActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPaymentTypeLookup(string IsActive, string connStr)
        {
            var dsPaymentType = new DataSet();

            GetPaymentTypeList(dsPaymentType, IsActive, connStr);

            // Return result
            return dsPaymentType.Tables[TableName];
        }

        /// <summary>
        ///     Get data by payment type code.
        /// </summary>
        /// <param name="dsReceipt"></param>
        /// <param name="PaymentTypeCode"></param>
        /// <param name="IsActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentTypeByPaymentTypeCode(DataSet dsReceipt, string PaymentTypeCode, string IsActive,
            string connStr)
        {
            var result = false;

            // Create parameter
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@ReceiptNo", PaymentTypeCode);
            dbParams[1] = new DbParameter("@IsActive", IsActive);

            // Get data
            result = DbRetrieve("AR.GetPaymentTypeByPaymentCode", dsReceipt, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get payment type database schema
        /// </summary>
        /// <param name="dsPaymentType"></param>
        /// <returns></returns>
        public bool GetPaymentTypeStructure(DataSet dsPaymentType, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("AR.GetPaymentTypeList", dsPaymentType, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get max payment type code
        /// </summary>
        /// <returns></returns>
        public int GetPaymentTypeMaxID(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("AR.GetPaymentTypeMaxID", null, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}