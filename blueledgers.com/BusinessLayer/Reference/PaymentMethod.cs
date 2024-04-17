using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class PaymentMethod : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentMethod()
        {
            SelectCommand = "SELECT * FROM [Reference].PaymentMethod";
            TableName = "PaymentMethod";
        }

        /// <summary>
        /// </summary>
        /// <param name="paymentMethodCode"></param>
        /// <param name="dsPaymentMethod"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentMethodList(string paymentMethodCode, DataSet dsPaymentMethod, string connStr)
        {
            var result = false;
            var dtAP = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@PaymentMethodCode", paymentMethodCode);

            // Get data
            result = DbRetrieve("[Reference].GetPaymentMethodListByPaymentMethodCode", dsPaymentMethod, dbParams,
                TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="paymentMethodCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPaymentMethodListByPaymentMethodCode(string paymentMethodCode, string connStr)
        {
            var dtAP = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@PaymentMethodCode", paymentMethodCode);

            // Get data
            dtAP = DbRead("[Reference].GetPaymentMethodListByPaymentMethodCode", dbParams, connStr);

            // Return result
            return dtAP;
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPaymentMethodListForLookUp(string connStr)
        {
            var dtAP = new DataTable();

            // Get data
            dtAP = DbRead("[Reference].GetPaymentMethodList", null, connStr);

            // Return result
            return dtAP;
        }

        /// <summary>
        /// </summary>
        /// <param name="PaymentMethodCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetPaymentMethodName(string PaymentMethodCode, string connStr)
        {
            string paymentMethodName;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PaymentMethodCode", PaymentMethodCode);

            DbRetrieve("[Reference].GetPaymentMethodListByPaymentMethodCode", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                paymentMethodName = dsTmp.Tables[TableName].Rows[0]["Name"].ToString();
            }
            else
            {
                paymentMethodName = null;
            }
            return paymentMethodName;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPaymentMethod"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentMethodStructure(DataSet dsPaymentMethod, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("[Reference].GetPaymentMethodList", dsPaymentMethod, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}