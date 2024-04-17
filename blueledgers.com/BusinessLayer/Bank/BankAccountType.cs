using System.Data;
using Blue.DAL;

namespace Blue.BL.Bank
{
    public class BankAccountType : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public BankAccountType()
        {
            SelectCommand = "SELECT * FROM Bank.BankAccountType";
            TableName = "BankAccountType";
        }

        /// <summary>
        ///     Get bankAccountType list using bankAccountTypeCode
        /// </summary>
        /// <param name="bankAccountTypeCode"></param>
        /// <param name="dsBank"></param>
        /// <returns></returns>
        public bool GetBankAccountTypeList(string bankAccountTypeCode, DataSet dsBank, string connStr)
        {
            var result = false;
            var dtBankAccount = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@BankAccountTypeCode", bankAccountTypeCode);

            // Get data
            result = DbRetrieve("Bank.GetBankAccountTypeListByBankAccountTypeCode", dsBank, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     GetBankAccountTypeList
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetBankAccountTypeList(string connStr)
        {
            var dtBankAccount = new DataTable();

            // Get data
            dtBankAccount = DbRead("Bank.GetBankAccountTypeList", null, connStr);

            // Return result
            return dtBankAccount;
        }

        /// <summary>
        ///     Get bankAccountType using bankAccountTypeCode
        /// </summary>
        /// <param name="bankAccountTypeCode"></param>
        /// <returns></returns>
        public DataTable GetBankAccountTypeListByBankAccountTypeCode(string bankAccountTypeCode, string connStr)
        {
            var dtBankAccount = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@BankAccountTypeCode", bankAccountTypeCode);

            // Get data
            dtBankAccount = DbRead("Bank.GetBankAccountTypeListByBankAccountTypeCode", dbParams, connStr);

            // Return result
            return dtBankAccount;
        }

        /// <summary>
        ///     Get BankAccountType name by bankAccountTypeCode
        /// </summary>
        /// <param name="SectionCode"></param>
        /// <returns></returns>
        public string GetBankAccountTypeName(string bankAccountTypeCode, string connStr)
        {
            string bankAccountTypeName;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BankAccountTypeCode", bankAccountTypeCode);

            DbRetrieve("Bank.GetBankAccountTypeListByBankAccountTypeCode", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                bankAccountTypeName = dsTmp.Tables[TableName].Rows[0]["Name"].ToString();
            }
            else
            {
                bankAccountTypeName = null;
            }
            return bankAccountTypeName;
        }

        /// <summary>
        ///     Get BankAccount
        /// </summary>
        /// <param name="dsBankAccountType"></param>
        /// <returns></returns>
        public bool GetBankAccountStructure(DataSet dsBankAccountType, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("Bank.GetBankAccountTypeList", dsBankAccountType, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get data to lookup.
        /// </summary>
        /// <param name="IsActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetBankAccountTypeLookup(string IsActive, string connStr)
        {
            var dsBankAccType = new DataSet();

            // Create parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@IsActive", IsActive);

            DbRetrieve("Bank.GetBankAccountTypeByIsActive", dsBankAccType, dbParams, TableName, connStr);

            // Return result
            return dsBankAccType.Tables[TableName];
        }

        #endregion
    }
}