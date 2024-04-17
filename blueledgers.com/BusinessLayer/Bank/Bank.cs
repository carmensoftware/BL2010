using System.Data;
using Blue.DAL;

namespace Blue.BL.Bank
{
    public class Bank : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Bank()
        {
            SelectCommand = "SELECT * FROM Bank.Bank";
            TableName = "Bank";
        }

        /// <summary>
        ///     Get Bank list using bankCode
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="dsBank"></param>
        /// <returns></returns>
        public bool GetBankList(string bankCode, DataSet dsBank, string connStr)
        {
            var result = false;
            var dtBank = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@BankCode", bankCode);

            // Get data
            result = DbRetrieve("Bank.GetBankListByBankCode", dsBank, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsBank"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetBankList(DataSet dsBank, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Bank.GetBankList", dsBank, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get Bank using bankCode
        /// </summary>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        public DataTable GetBankList(string connStr)
        {
            var dtBank = new DataTable();

            // Get data
            dtBank = DbRead("Bank.GetBankList", null, connStr);

            // Return result
            return dtBank;
        }

        /// <summary>
        ///     Get Bank using bankCode
        /// </summary>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        public DataTable GetBankListByBankCode(string bankCode, string connStr)
        {
            var dtBank = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@BankCode", bankCode);

            // Get data
            dtBank = DbRead("Bank.GetBankListByBankCode", dbParams, connStr);

            // Return result
            return dtBank;
        }

        /// <summary>
        ///     Get branch name by branchcode
        /// </summary>
        /// <param name="SectionCode"></param>
        /// <returns></returns>
        public string GetBranchName(string BranchCode, string connStr)
        {
            string branchName;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BranchCode", BranchCode);

            DbRetrieve("Bank.GetBankListByBranchCode", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                branchName = dsTmp.Tables[TableName].Rows[0]["BranchName"].ToString();
            }
            else
            {
                branchName = null;
            }
            return branchName;
        }

        /// <summary>
        /// </summary>
        /// <param name="BankCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetBranchCodebyBankCode(string BankCode, string connStr)
        {
            string branchCode;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BankCode", BankCode);

            DbRetrieve("Bank.GetBankListByBankCode", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                branchCode = dsTmp.Tables[TableName].Rows[0]["BranchCode"].ToString();
            }
            else
            {
                branchCode = null;
            }
            return branchCode;
        }

        /// <summary>
        /// </summary>
        /// <param name="BranchCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetBranchNamebyBankCode(string BankCode, string connStr)
        {
            string branchName;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BankCode", BankCode);

            DbRetrieve("Bank.GetBankListByBankCode", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                branchName = dsTmp.Tables[TableName].Rows[0]["BranchName"].ToString();
            }
            else
            {
                branchName = null;
            }
            return branchName;
        }

        /// <summary>
        ///     Get bank name by bankcode
        /// </summary>
        /// <param name="SectionCode"></param>
        /// <returns></returns>
        public string GetBankName(string BankCode, string connStr)
        {
            string bankName;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BankCode", BankCode);

            DbRetrieve("Bank.GetBankListByBankCode", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                bankName = dsTmp.Tables[TableName].Rows[0]["Name"].ToString();
            }
            else
            {
                bankName = null;
            }
            return bankName;
        }

        /// <summary>
        /// </summary>
        /// <param name="Search_Param"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetSearchBranchList(string Search_Param, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Search_Param", Search_Param);

            return DbRead("Bank.GetSearchBranchList", dbParams, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="Search_Param"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetSearchBankList(string Search_Param, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Search_Param", Search_Param);

            return DbRead("Bank.GetSearchBankList", dbParams, connStr);
        }

        /// <summary>
        ///     Get Bank
        /// </summary>
        /// <param name="dsBank"></param>
        /// <returns></returns>
        public bool GetBankStructure(DataSet dsBank, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("Bank.GetBankList", dsBank, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}