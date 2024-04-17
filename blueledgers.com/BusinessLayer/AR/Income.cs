using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class Income : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Income()
        {
            SelectCommand = "SELECT * FROM AR.Income";
            TableName = "Income";
        }

        /// <summary>
        ///     Get invoice detail list using invoice id
        /// </summary>
        /// <param name="standardVoucherID"></param>
        /// <param name="dsInvoice"></param>
        /// <returns></returns>
        public bool GetList(int refNoDetail, DataSet dsIncome, string connStr)
        {
            var result = false;
            var dtExpense = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@refNoDetail", Convert.ToString(refNoDetail));

            // Get data
            result = DbRetrieve("AR.GetIncomeByRefNoDetail", dsIncome, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="RefNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetList(int refNoDetail, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@refNoDetail", Convert.ToString(refNoDetail));


            DbRetrieve("AR.GetIncomeByRefNoDetail", dsTmp, dbParams, TableName, connStr);

            return dsTmp;
        }

        /// <summary>
        ///     Get expense detail using refno
        /// </summary>
        /// <param name="standardVoucherID"></param>
        /// <returns></returns>
        public DataTable GetByRefNo(string RefNo, string connStr)
        {
            var dtExpense = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@RefNo", RefNo);

            // Get data
            dtExpense = DbRead("AR.GetIncomeByRefNo", dbParams, connStr);

            // Return result
            return dtExpense;
        }

        /// <summary>
        ///     Get Schema
        /// </summary>
        /// <param name="dsExpense"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsExpense, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("AR.GetIncomeList", dsExpense, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <returns></returns>
        public int GetMax(string refNo, string connStr)
        {
            var expenseMax = 0;

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@RefNo", refNo);

            // Get data
            expenseMax = DbReadScalar("AP.GetIncomeMax", dbParams, connStr);

            // Return result
            return expenseMax;
        }

        #endregion
    }
}