using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ProfileComment : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ProfileComment()
        {
            SelectCommand = "SELECT * FROM AR.ProfileComment";
            TableName = "ProfileComment";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsDebtorComment"></param>
        /// <param name="DebtorCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetByCustomerCode(DataSet dsDebtorComment, string debtorCode, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CustomerCode", debtorCode);

            // Get data
            result = DbRetrieve("AR.GetProfileCommentByCustomerCode", dsDebtorComment, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get schema for debtor comment.
        /// </summary>
        /// <param name="dsDebtorComment"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsDebtorComment, string ConnectionString)
        {
            return DbRetrieveSchema("AR.GetProfileCommentList", dsDebtorComment, null, TableName, ConnectionString);
        }

        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsDebtorComment"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsDebtorComment, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsDebtorComment, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}