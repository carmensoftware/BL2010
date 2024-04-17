using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StdJvSchdType : DbHandler
    {
        #region " Attributies "

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public StdJvSchdType()
        {
            SelectCommand = "SELECT * FROM GL.StdJvSchdType";
            TableName = "StdJvSchdType";
        }


        /// <summary>
        ///     Get data for look up.
        /// </summary>
        /// <param name="dsTimeZone"></param>
        /// <param name="connStr"></param>
        public DataTable GetLookUp(string connStr)
        {
            return DbRead("GL.GetStdJvSchdTypeLookUp", null, connStr);
        }


        /// <summary>
        ///     Get schedule name
        /// </summary>
        /// <param name="scheduleTypeID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetScheduleName(string scheduleTypeID, string connStr)
        {
            var dsStdJvSchdType = new DataSet();
            var result = string.Empty;

            // Get data
            GetByScheduleTypeID(scheduleTypeID, dsStdJvSchdType, connStr);

            // Return result
            if (dsStdJvSchdType.Tables[TableName].Rows.Count > 0)
            {
                result = dsStdJvSchdType.Tables[TableName].Rows[0]["Desc"].ToString();
            }

            return result;
        }


        /// <summary>
        ///     Get data by scheduleTypeID.
        /// </summary>
        /// <param name="scheduleTypeID"></param>
        /// <param name="dsStdJvSchdType"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetByScheduleTypeID(string scheduleTypeID, DataSet dsStdJvSchdType, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ScheduleTypeID", scheduleTypeID);

            // Get data
            result = DbRetrieve("GL.GetStdJvSchdType_ScheduleTypeID", dsStdJvSchdType, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}