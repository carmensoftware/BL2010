using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.AP
{
    public class BankAccount : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public BankAccount()
        {
            SelectCommand = "SELECT * FROM AP.BankAccount";
            TableName = "BankAccount";
        }


        /// <summary>
        ///     Get name
        /// </summary>
        /// <param name="accCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetCurrency(string accCode, string connStr)
        {
            var result = string.Empty;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@AccCode", accCode);

            // Get data
            DbRetrieve("AP.GetBankAccount_AccCode", dsTmp, dbParams, TableName, connStr);

            // Return result
            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                result = dsTmp.Tables[TableName].Rows[0]["BankCurrency"].ToString();
            }

            return result;
        }

        #endregion
    }
}