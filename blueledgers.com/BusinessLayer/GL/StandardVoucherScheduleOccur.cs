using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StandardVoucherScheduleOccur : DbHandler
    {
        #region " Attributies "

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public StandardVoucherScheduleOccur()
        {
            SelectCommand = "SELECT * FROM GL.StandardVoucherScheduleOccur";
            TableName = "StandardVoucherScheduleOccur";
        }

        /// <summary>
        ///     Get all data
        /// </summary>
        /// <returns></returns>
        public void GetList(DataSet dsScheduleOccur, string connStr)
        {
            DbRetrieve("GL.GetStandardVoucherScheduleOccurList", dsScheduleOccur, null, TableName, connStr);
        }

        /// <summary>
        ///     Get data for look up.
        /// </summary>
        /// <param name="dsTimeZone"></param>
        /// <param name="connStr"></param>
        public DataTable GetLookUp(string connStr)
        {
            var dsScheduleOccur = new DataSet();

            // Get data
            var standardVoucherScheduleOccur = new StandardVoucherScheduleOccur();


            standardVoucherScheduleOccur.DbRetrieve("GL.GetStandardVoucherScheduleOccurLookUp", dsScheduleOccur, null,
                TableName, connStr);

            return dsScheduleOccur.Tables[TableName];
        }

        /// <summary>
        ///     Get data by id.
        /// </summary>
        /// <param name="scheduleOccureID"></param>
        /// <param name="dsScheduleOccure"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetByID(string scheduleOccurID, DataSet dsScheduleOccure, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ScheduleOccurID", scheduleOccurID);

            // Get data
            result = DbRetrieve("GL.GetStandardVoucherScheduleOccurByID", dsScheduleOccure, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get name.
        /// </summary>
        /// <param name="scheduleOccureID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string scheduleOccureID, string connStr)
        {
            var dsScheduleOccure = new DataSet();
            var result = string.Empty;

            // Get data
            GetByID(scheduleOccureID, dsScheduleOccure, connStr);

            // Return result
            if (dsScheduleOccure.Tables[TableName].Rows.Count > 0)
            {
                result = dsScheduleOccure.Tables[TableName].Rows[0]["Name"].ToString();
            }

            return result;
        }

        #endregion
    }
}