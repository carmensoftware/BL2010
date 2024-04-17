using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class TransactionType : DbHandler
    {
        #region "Attributies"

        //InvoiceDetail invoiceDetail = new InvoiceDetail();
        //InvoiceMisc invoiceMisc     = new InvoiceMisc();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public TransactionType()
        {
            SelectCommand = "SELECT * FROM AR.TransactionType";
            TableName = "TransactionType";
        }

        /// <summary>
        ///     Get transection type using code
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <returns></returns>
        public bool GetTransactionTypeByCode(DataSet dsTransaction, string transactionTypeCode, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@TransactionTypeCode", transactionTypeCode);

            // Get data
            result = DbRetrieve("AR.GetTransactionTypeByCode", dsTransaction, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get data all.
        /// </summary>
        /// <param name="dsTransaction"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetTransactionTypeList(DataSet dsTransaction, int userID, string connStr)
        {
            DbRetrieve("AR.GetTransactionType", dsTransaction, null, TableName, connStr);

            return dsTransaction;
        }

        /// <summary>
        ///     Get name by code.
        /// </summary>
        /// <param name="typeCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string typeCode, string connStr)
        {
            var Name = string.Empty;
            var dsTmp = new DataSet();

            GetTransactionTypeByCode(dsTmp, typeCode, connStr);

            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                Name = dsTmp.Tables[TableName].Rows[0]["Name"].ToString();
            }

            return Name;
        }

        /// <summary>
        ///     Get transaction database schema.
        /// </summary>
        /// <param name="dsTransaction"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceStructure(DataSet dsTransaction, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("AR.GetTransactionType", dsTransaction, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get max transaction type code.
        /// </summary>
        /// <returns></returns>
        public int GetTransactionTypeMaxCode(string connStr)
        {
            var dsTmp = new DataSet();

            // Retrieve data.
            DbRetrieve("AR.GetTransactionTypeMaxCode", dsTmp, null, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows[0]["MaxCode"].ToString() != string.Empty)
            {
                return (int) dsTmp.Tables[TableName].Rows[0]["MaxCode"];
            }
            return 1;
        }

        /// <summary>
        ///     Get data to lookup.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetLookup(string connStr)
        {
            var dsTran = new DataSet();

            // Get Data
            DbRetrieve("AR.GetTransactionTypeLookup", dsTran, null, TableName, connStr);

            // Return result
            return dsTran.Tables[TableName];
        }

        #endregion
    }
}