using System;
using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class Expense : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Expense()
        {
            SelectCommand = "SELECT * FROM AP.Expense";
            TableName = "Expense";
        }

        /// <summary>
        ///     Get invoice detail list using invoice id
        /// </summary>
        /// <param name="refNoDetail"></param>
        /// <param name="dsInvoice"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetExpenseList(int refNoDetail, DataSet dsInvoice, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@refNoDetail", Convert.ToString(refNoDetail));

            // Get data
            var result = DbRetrieve("AP.GetExpenseListByrefNoDetail", dsInvoice, dbParams, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        /// </summary>
        /// <param name="refNoDetail"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetExpenseList(int refNoDetail, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@refNoDetail", Convert.ToString(refNoDetail));


            DbRetrieve("AP.GetExpenseListByrefNoDetail", dsTmp, dbParams, TableName, connStr);

            return dsTmp;
        }

        /// <summary>
        ///     Get expense detail using refno
        /// </summary>
        /// <param name="refNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetExpenseListByRefNo(string refNo, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@RefNo", refNo);

            // Get data
            var dtExpense = DbRead("AP.GetExpenseListByRefNo", dbParams, connStr);

            // Return result
            return dtExpense;
        }

        /// <summary>
        ///     Get Expense
        /// </summary>
        /// <param name="dsExpense"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetExpenseStructure(DataSet dsExpense, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetExpenseList", dsExpense, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <returns></returns>
        public int GetExpenseMax(string refNo, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", refNo);

            // Get data
            var expenseMax = DbReadScalar("AP.GetExpenseMax", dbParams, connStr);

            // Return result
            return expenseMax;
        }

        /// <summary>
        ///     Get max seqNo
        /// </summary>
        /// <returns></returns>
        public int GetExpenseMaxSeqNo(string connStr)
        {
            // Get data
            var result = DbReadScalar("AP.GetExpenseMaxSeqNo", null, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}