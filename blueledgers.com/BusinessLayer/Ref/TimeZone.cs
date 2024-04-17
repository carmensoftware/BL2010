using System.Data;
using Blue.DAL;

namespace Blue.BL.Ref
{
    public class TimeZone : DbHandler
    {
        #region " Attributies "

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public TimeZone()
        {
            SelectCommand = "SELECT * FROM Ref.TimeZone";
            TableName = "TimeZone";
        }

        /// <summary>
        ///     Get all data
        /// </summary>
        /// <returns></returns>
        public void GetTimeZoneList(DataSet dsTimeZone, string connStr)
        {
            DbRetrieve("Ref.GetTimeZoneList", dsTimeZone, null, TableName, connStr);
        }

        /// <summary>
        ///     Get data for look up.
        /// </summary>
        /// <param name="dsTimeZone"></param>
        /// <param name="connStr"></param>
        public DataTable GetTimeZoneLookUp(string connStr)
        {
            return DbRead("Ref.GetTimeZoneLookup", null, connStr);
        }

        /// <summary>
        ///     Get data by id
        /// </summary>
        /// <param name="timeZoneID"></param>
        /// <param name="dsTimeZone"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetByTimeZoneID(string timeZoneID, DataSet dsTimeZone, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@TimeZoneID", timeZoneID);

            // Get data
            result = DbRetrieve("Ref.GetTimeZoneByID", dsTimeZone, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get name
        /// </summary>
        /// <param name="timeZoneID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string timeZoneID, string connStr)
        {
            var dsTimeZone = new DataSet();
            var result = string.Empty;

            // Get data
            GetByTimeZoneID(timeZoneID, dsTimeZone, connStr);

            // Return result
            if (dsTimeZone.Tables[TableName].Rows.Count > 0)
            {
                result = dsTimeZone.Tables[TableName].Rows[0]["Desc"].ToString();
            }

            return result;
        }

        #endregion
    }
}