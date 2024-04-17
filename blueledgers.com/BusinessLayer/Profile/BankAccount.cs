using System.Data;
using Blue.DAL;

namespace Blue.BL.Profile
{
    public class BankAccount : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public BankAccount()
        {
            SelectCommand = "SELECT * FROM Profile.BankAccount";
            TableName = "BankAccount";
        }

        /// <summary>
        ///     Get vendor detail list using vendor id
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="dsProfile"></param>
        /// <returns></returns>
        public bool GetBankAccountList(string profileCode, DataSet dsProfile, string connStr)
        {
            var result = false;
            var dtBankAccount = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            result = DbRetrieve("Profile.GetBankAccountListByProfileCode", dsProfile, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get vendor detail using vendor id
        /// </summary>
        /// <param name="profileCode"></param>
        /// <returns></returns>
        public DataTable GetBankAccountListByProfileCode(string profileCode, string connStr)
        {
            var dtBankAccount = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            dtBankAccount = DbRead("Profile.GetBankAccountListByProfileCode", dbParams, connStr);

            // Return result
            return dtBankAccount;
        }

        /// <summary>
        ///     Get BankAccount
        /// </summary>
        /// <param name="dsBankAccount"></param>
        /// <returns></returns>
        public bool GetBankAccountStructure(DataSet dsBankAccount, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("Profile.GetBankAccountList", dsBankAccount, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get bank account max seqNo
        /// </summary>
        /// <returns></returns>
        public int GetBankAccountMaxSeqNo(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("Profile.GetBankAccountMaxSeqNo", null, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}