using System.Data;
using Blue.DAL;

namespace Blue.BL.AP
{
    public class TransactionType : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public TransactionType()
        {
            SelectCommand = "SELECT * FROM AP.TransactionType";
            TableName = "TransactionType";
        }

        /// <summary>
        ///     Get TransactionType list using transactionTypeCode
        /// </summary>
        /// <param name="transactionTypeCode"></param>
        /// <param name="dsTransactionType"></param>
        /// <returns></returns>
        public bool GetTransactionTypeList(string transactionTypeCode, DataSet dsTransactionType, string connStr)
        {
            var result = false;
            var dtTransactionType = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@TransactionTypeCode", transactionTypeCode);

            // Get data
            result = DbRetrieve("AP.GetTransactionTypeListByTransactionTypeCode", dsTransactionType, dbParams, TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get TransactionType using transactionTypeCode
        /// </summary>
        /// <param name="transactionTypeCode"></param>
        /// <returns></returns>
        public DataTable GetTransactionTypeListByTransactionTypeCode(string transactionTypeCode, string connStr)
        {
            var dtTransactionType = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@TransactionTypeCode", transactionTypeCode);

            // Get data
            dtTransactionType = DbRead("AP.GetTransactionTypeListByTransactionTypeCode", dbParams, connStr);

            // Return result
            return dtTransactionType;
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetTransactionTypeListLookup(string connStr)
        {
            var dtTransactionType = new DataTable();


            // Get data
            dtTransactionType = DbRead("AP.GetTransactionTypeList", null, connStr);

            // Return result
            return dtTransactionType;
        }

        /// <summary>
        ///     Get transaction name by transactionTypeCode
        /// </summary>
        /// <param name="SectionCode"></param>
        /// <returns></returns>
        public string GetTransactionTypeName(string TransactionTypeCode, string connStr)
        {
            string transactionTypeName;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@TransactionTypeCode", TransactionTypeCode);

            DbRetrieve("AP.GetTransactionTypeListByTransactionTypeCode", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                transactionTypeName = dsTmp.Tables[TableName].Rows[0]["Name"].ToString();
            }
            else
            {
                transactionTypeName = null;
            }
            return transactionTypeName;
        }


        /// <summary>
        ///     Get TransactionType
        /// </summary>
        /// <param name="dsTransactionType"></param>
        /// <returns></returns>
        public bool GetTransactionTypeStructure(DataSet dsTransactionType, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("AP.GetTransactionTypeList", dsTransactionType, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}