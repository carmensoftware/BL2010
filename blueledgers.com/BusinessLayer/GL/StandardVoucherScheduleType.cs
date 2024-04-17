using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StandardVoucherScheduleType : DbHandler
    {
        #region " Attributies "

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public StandardVoucherScheduleType()
        {
            SelectCommand = "SELECT * FROM GL.StandardVoucherScheduleType";
            TableName = "StandardVoucherScheduleType";
        }

        /// <summary>
        ///     Get all data
        /// </summary>
        /// <returns></returns>
        public void GetList(DataSet dsScheduleType, string connStr)
        {
            DbRetrieve("GL.GetStandardVoucherScheduleTypeList", dsScheduleType, null, TableName, connStr);
        }

        /// <summary>
        ///     Get data for look up.
        /// </summary>
        /// <param name="dsTimeZone"></param>
        /// <param name="connStr"></param>
        public DataTable GetLookUp(string connStr)
        {
            return DbRead("GL.GetStandardVoucherScheduleTypeLookUp", null, connStr);
        }

        /// <summary>
        ///     Get data by id.
        /// </summary>
        /// <param name="scheduleOccureID"></param>
        /// <param name="dsScheduleOccure"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetByID(string scheduleTypeID, DataSet dsScheduleType, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ScheduleTypeID", scheduleTypeID);

            // Get data
            result = DbRetrieve("GL.GetStandardVoucherScheduleTypeByID", dsScheduleType, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get name.
        /// </summary>
        /// <param name="scheduleOccureID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string scheduleTypeID, string connStr)
        {
            var dsScheduleType = new DataSet();
            var result = string.Empty;

            // Get data
            GetByID(scheduleTypeID, dsScheduleType, connStr);

            // Return result
            if (dsScheduleType.Tables[TableName].Rows.Count > 0)
            {
                result = dsScheduleType.Tables[TableName].Rows[0]["Name"].ToString();
            }

            return result;
        }

        #endregion
    }
}