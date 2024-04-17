using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class AccountTool : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public AccountTool()
        {
            SelectCommand = "SELECT * FROM Reference.AccountTool";
            TableName = "AccountTool";
        }

        /// <summary>
        ///     Get account tool link
        /// </summary>
        /// <param name="dsAccountTool"></param>
        /// <returns></returns>
        public bool GetAccountToolList(DataSet dsAccountTool, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Reference.GetAccountToolList", dsAccountTool, null, this.TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}