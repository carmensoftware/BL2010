using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StandardVoucherMonthlyWeek : DbHandler
    {
        #region " Attributies "

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public StandardVoucherMonthlyWeek()
        {
            SelectCommand = "SELECT * FROM GL.StandardVoucherMonthlyWeek";
            TableName = "StandardVoucherMonthlyWeek";
        }

        /// <summary>
        ///     Get all data
        /// </summary>
        /// <returns></returns>
        public void GetList(DataSet dsMonthlyWeek, string connStr)
        {
            DbRetrieve("GL.GetStandardVoucherMonthlyWeekList", dsMonthlyWeek, null, TableName, connStr);
        }

        /// <summary>
        ///     Get data for look up.
        /// </summary>
        /// <param name="dsTimeZone"></param>
        /// <param name="connStr"></param>
        public DataTable GetLookUp(string connStr)
        {
            return DbRead("GL.GetStandardVoucherMonthlyWeekLookUp", null, connStr);
        }

        /// <summary>
        ///     Get data by id.
        /// </summary>
        /// <param name="monthlyWeekID"></param>
        /// <param name="dsMonthlyWeek"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetByID(string monthlyWeekID, DataSet dsMonthlyWeek, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@MonthlyWeekID", monthlyWeekID);

            // Get data
            result = DbRetrieve("GL.GetStandardVoucherMonthlyWeekByID", dsMonthlyWeek, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get name.
        /// </summary>
        /// <param name="monthlyWeekID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string monthlyWeekID, string connStr)
        {
            var dsMonthlyWeek = new DataSet();
            var result = string.Empty;

            // Get data
            GetByID(monthlyWeekID, dsMonthlyWeek, connStr);

            // Return result
            if (dsMonthlyWeek.Tables[TableName].Rows.Count > 0)
            {
                result = dsMonthlyWeek.Tables[TableName].Rows[0]["Name"].ToString();
            }

            return result;
        }

        #endregion
    }
}