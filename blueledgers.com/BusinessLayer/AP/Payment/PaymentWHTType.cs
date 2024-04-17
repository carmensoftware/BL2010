using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentWHTType : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentWHTType()
        {
            SelectCommand = "SELECT * FROM AP.PaymentWHTType";
            TableName = "PaymentWHTType";
        }

        /// <summary>
        /// </summary>
        /// <param name="paymentWHTTypeCode"></param>
        /// <param name="dsPayment"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentWHTTypeList(string paymentWHTTypeCode, DataSet dsPayment, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@PaymentWHTTypeCode", paymentWHTTypeCode);

            // Get data
            var result = DbRetrieve("AP.GetPaymentWHTTypeListByPaymentWHTTypeCode", dsPayment, dbParams, TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPaymentWHTTypeList(string connStr)
        {
            // Get data
            var dtPaymentWHTType = DbRead("AP.GetPaymentWHTTypeList", null, connStr);

            // Return result
            return dtPaymentWHTType;
        }

        /// <summary>
        /// </summary>
        /// <param name="paymentWHTTypeCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetPaymentWHTTypeName(string paymentWHTTypeCode, string connStr)
        {
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PaymentWHTTypeCode", paymentWHTTypeCode);

            DbRetrieve("AP.GetPaymentWHTTypeListByPaymentWHTTypeCode", dsTmp, dbParams, TableName, connStr);

            var name = dsTmp.Tables[TableName].Rows.Count > 0
                ? dsTmp.Tables[TableName].Rows[0]["Name"].ToString()
                : null;
            return name;
        }


        /// <summary>
        /// </summary>
        /// <param name="paymentWHTTypeCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPaymentWHTTypeListByPaymentWHTTypeCode(string paymentWHTTypeCode, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@PaymentWHTTypeCode", paymentWHTTypeCode);

            // Get data
            var dtPaymentWHTType = DbRead("AP.GetPaymentWHTTypeListByPaymentWHTTypeCode", dbParams, connStr);

            // Return result
            return dtPaymentWHTType;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPaymentWHTType"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentWHTTypeStructure(DataSet dsPaymentWHTType, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetPaymentWHTTypeList", dsPaymentWHTType, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}