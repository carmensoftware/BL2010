using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class AccountReport : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public AccountReport()
        {
            SelectCommand = "SELECT * FROM Reference.AccountReport";
            TableName = "AccountReport";
        }

        /// <summary>
        ///     Get account report link
        /// </summary>
        /// <param name="dsAccountReport"></param>
        /// <returns></returns>
        public bool GetAccountReportList(DataSet dsAccountReport, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Reference.GetAccountReportList", dsAccountReport, null, this.TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}