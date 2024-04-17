using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class AccountDraft : DbHandler
    {
        #region "Attibuties"

        #endregion

        #region "Operations"

        public AccountDraft()
        {
            this.SelectCommand = "SELECT * FROM Reference.AccountDraft";
            this.TableName = "AccountDraft";
        }

        /// <summary>
        ///     Get journalvoucher using journalvoucher view id
        /// </summary>
        /// <param name="dsAccountDraftDraft"></param>
        public void GetAccountDraftList(DataSet dsAccount, string connStr)
        {
            var accountDraft = new AccountDraft();
            accountDraft.DbRetrieve("Reference.GetAccountDraftList", dsAccount, null, this.TableName, connStr);
        }

        /// <summary>
        ///     Get account draft by account code.
        /// </summary>
        /// <param name="dsAccountDraft"></param>
        /// <param name="accountCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetAccountDraftByAccountCode(DataSet dsAccountDraft, string accountCode, string connStr)
        {
            var result = false;

            // Created parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AccountCode", accountCode);

            // Get data
            result = DbRetrieve("Reference.GetAccountDraftByAccountCode", dsAccountDraft, dbParams, this.TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get journalvoucherdraft table schema.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsAccount, string conStr)
        {
            return DbRetrieveSchema("Reference.GetAccountDraftList", dsAccount, null, this.TableName, conStr);
        }

        /// <summary>
        ///     Save Data.
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var accountDraft = new AccountDraft();
            var accountMiscDraft = new AccountMiscDraft();

            var result = false;

            // สร้าง SaveSource object
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(savedData, this.SelectCommand, this.TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, accountMiscDraft.SelectCommand, accountMiscDraft.TableName);

            // Call dbCommit and send savesource object is parameter
            result = DbCommit(dbSaveSource, connStr);

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet savedData, string connStr)
        {
            var result = false;

            // สร้าง SaveSource object
            var dbSaveSource = new DbSaveSource[1];

            dbSaveSource[0] = new DbSaveSource(savedData, this.SelectCommand, this.TableName);

            // Call dbCommit and send savesource object is parameter
            result = DbCommit(dbSaveSource, connStr);

            return result;
        }

        #endregion
    }
}