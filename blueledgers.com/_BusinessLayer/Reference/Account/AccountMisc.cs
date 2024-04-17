using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class AccountMisc : DbHandler
    {
        #region " Attributies "

        public string AccountCode { get; set; }

        public Guid FieldID { get; set; }

        public string Value { get; set; }

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public AccountMisc()
        {
            SelectCommand = "SELECT * FROM Reference.AccountMisc";
            TableName = "AccountMisc";
        }

        /// <summary>
        ///     Get all AccountMisc data the relate to specified AccountCode.
        /// </summary>
        /// <param name="dsAccountMisc"></param>
        /// <param name="accountCode"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAccountMisc, string accountCode, string ConnectionString)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@AccountCode", accountCode);

            return DbRetrieve("Reference.GetAccountMiscListByAccountCode", dsAccountMisc, dbParam, TableName,
                ConnectionString);
        }

        /// <summary>
        ///     Get Budget Misc Schema
        /// </summary>
        /// <param name="dsBudgetMisc"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsAccountMisc, string ConnectionString)
        {
            return DbRetrieveSchema("Reference.GetAccountMiscList", dsAccountMisc, null, TableName, ConnectionString);
        }

        #endregion
    }
}