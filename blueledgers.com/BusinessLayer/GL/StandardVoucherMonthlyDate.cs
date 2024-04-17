using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StandardVoucherMonthlyDate : DbHandler
    {
        #region " Attributies "

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public StandardVoucherMonthlyDate()
        {
            SelectCommand = "SELECT * FROM GL.StandardVoucherdsMonthlyDate";
            TableName = "StandardVoucherdsMonthlyDate";
        }

        /// <summary>
        ///     Get all data
        /// </summary>
        /// <returns></returns>
        public void GetList(DataSet dsMonthlyDate, string connStr)
        {
            DbRetrieve("GL.GetStandardVoucherMonthlyDateLookUp", dsMonthlyDate, null, TableName, connStr);
        }

        /// <summary>
        ///     Get data for look up.
        /// </summary>
        /// <param name="dsTimeZone"></param>
        /// <param name="connStr"></param>
        public DataTable GetLookUp(string connStr)
        {
            return DbRead("GL.GetStandardVoucherMonthlyDateLookUp", null, connStr);
        }

        /// <summary>
        ///     Get data by id.
        /// </summary>
        /// <param name="monthlyDateID"></param>
        /// <param name="dsScheduleOccure"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetByID(string monthlyDateID, DataSet dsMonthlyDate, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@MonthlyDateID", monthlyDateID);

            // Get data
            result = DbRetrieve("GL.GetStandardVoucherMonthlyDateByID", dsMonthlyDate, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get name.
        /// </summary>
        /// <param name="monthlyDateID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string monthlyDateID, string connStr)
        {
            var dsMonthlyDate = new DataSet();
            var result = string.Empty;

            // Get data
            GetByID(monthlyDateID, dsMonthlyDate, connStr);

            // Return result
            if (dsMonthlyDate.Tables[TableName].Rows.Count > 0)
            {
                result = dsMonthlyDate.Tables[TableName].Rows[0]["Name"].ToString();
            }

            return result;
        }

        #endregion
    }
}