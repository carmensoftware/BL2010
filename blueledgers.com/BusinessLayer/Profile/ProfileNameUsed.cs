using System.Data;
using Blue.DAL;

namespace Blue.BL.Profile
{
    public class ProfileNameUsed : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ProfileNameUsed()
        {
            SelectCommand = "SELECT * FROM Profile.ProfileNameUsed";
            TableName = "ProfileNameUsed";
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="dsProfileNameUsed"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetProfileNameUsedList(string profileCode, DataSet dsProfileNameUsed, string connStr)
        {
            var result = false;
            var dtProfileNameUsed = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            result = DbRetrieve("Profile.GetProfileNameUsedListByProfileCode", dsProfileNameUsed, dbParams, TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get contact detail using contact id
        /// </summary>
        /// <param name="profileCode"></param>
        /// <returns></returns>
        public DataTable GetProfileNameUsedListByProfileCode(string profileCode, string connStr)
        {
            var dtProfileNameUsed = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            dtProfileNameUsed = DbRead("Profile.GetProfileNameUsedListByProfileCode", dbParams, connStr);

            // Return result
            return dtProfileNameUsed;
        }

        /// <summary>
        ///     Get ProfileNameUsed Structure
        /// </summary>
        /// <param name="dsProfileNameUsed"></param>
        /// <returns></returns>
        public bool GetProfileNameUsedStructure(DataSet dsProfileNameUsed, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("Profile.GetProfileNameUsedList", dsProfileNameUsed, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}