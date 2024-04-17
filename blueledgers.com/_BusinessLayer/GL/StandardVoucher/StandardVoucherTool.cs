using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StandardVoucherTool : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public StandardVoucherTool()
        {
            SelectCommand = "SELECT * FROM GL.StandardVoucherTool";
            TableName = "StandardVoucherTool";
        }

        /// <summary>
        ///     Get standard voucher tool list
        /// </summary>
        /// <param name="dsStandardVoucherReport"></param>
        /// <returns></returns>
        public bool GetStandardVoucherToolList(DataSet dsStandardVoucherReport, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetStandardVoucherToolList", dsStandardVoucherReport, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}