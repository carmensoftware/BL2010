using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class DebtorTool : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public DebtorTool()
        {
            SelectCommand = "SELECT * FROM AR.DebtorTool";
            TableName = "DebtorTool";
        }

        /// <summary>
        ///     Get debtor tool list
        /// </summary>
        /// <param name="dsVendorReport"></param>
        /// <returns></returns>
        public bool GetDebtorToolList(DataSet dsDebtorReport, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("AR.GetDebtorToolList", dsDebtorReport, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}