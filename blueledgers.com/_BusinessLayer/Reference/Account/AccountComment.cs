using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class AccountComment : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public AccountComment()
        {
            SelectCommand = "SELECT * FROM Reference.AccountComment";
            TableName = "AccountComment";
        }

        /// <summary>
        ///     Get all of account voucher except recurring type
        /// </summary>
        /// <param name="dsAccount"></param>
        /// <returns></returns>
        public bool GetAccountCommentListByAccountCode(DataSet dsAccountComment, string accountCode, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AccountCode", accountCode);

            // Get data
            result = DbRetrieve("Reference.GetAccountCommentListByAccountCode", dsAccountComment, dbParams,
                this.TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get schema for accountvoucher comment.
        /// </summary>
        /// <param name="dsAccountMisc"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsAccountComment, string ConnectionString)
        {
            return DbRetrieveSchema("Reference.GetAccountCommentList", dsAccountComment, null, this.TableName,
                ConnectionString);
        }

        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsAccount"></param>
        /// <returns></returns>
        public bool Save(DataSet dsAccountComment, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsAccountComment, this.SelectCommand, this.TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}