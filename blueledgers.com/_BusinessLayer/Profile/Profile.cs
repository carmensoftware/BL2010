using System.Data;
using Blue.DAL;

namespace Blue.BL.Profile
{
    public class Profile : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Profile()
        {
            SelectCommand = "SELECT * FROM Profile.Profile";
            TableName = "Profile";
        }

        /// <summary>
        ///     Get ProfileName Structure
        /// </summary>
        /// <param name="dsProfileName"></param>
        /// <returns></returns>
        public bool GetProfileStructure(DataSet dsProfile, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("Profile.GetProfileList", dsProfile, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsProfile"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetProfileListByProfileCode(string profileCode, DataSet dsProfile, string connStr)
        {
            var result = false;
            var dtProfile = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            result = DbRetrieve("Profile.GetProfileListByProfileCode", dsProfile, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}