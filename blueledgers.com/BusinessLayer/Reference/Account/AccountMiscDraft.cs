using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class AccountMiscDraft : DbHandler
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
        public AccountMiscDraft()
        {
            SelectCommand = "SELECT * FROM Reference.AccountMiscDraft";
            TableName = "AccountMiscDraft";
        }

        /// <summary>
        ///     Get all AccountMisc draft data the relate to specified AccountCode.
        /// </summary>
        /// <param name="dsAccountMisc"></param>
        /// <param name="accountCode"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAccountMisc, string accountCode, string ConnectionString)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@AccountCode", accountCode);

            return DbRetrieve("Reference.GetAccountMiscDraftByAccountCode", dsAccountMisc, dbParam, TableName,
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
            return DbRetrieveSchema("Reference.GetAccountMiscDraftList", dsAccountMisc, null, TableName,
                ConnectionString);
        }

        #endregion
    }
}