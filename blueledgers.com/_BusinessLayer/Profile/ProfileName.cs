using System.Data;
using Blue.DAL;

namespace Blue.BL.Profile
{
    public class ProfileName : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ProfileName()
        {
            SelectCommand = "SELECT * FROM Profile.ProfileName";
            TableName = "ProfileName";
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="dsProfileName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetProfileNameList(string profileCode, DataSet dsProfileName, string connStr)
        {
            var result = false;
            var dtProfileName = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            result = DbRetrieve("Profile.GetProfileNameListByProfileCode", dsProfileName, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get contact detail using contact id
        /// </summary>
        /// <param name="profileCode"></param>
        /// <returns></returns>
        public DataTable GetProfileNameListByProfileCode(string profileCode, string connStr)
        {
            var dtProfileName = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            dtProfileName = DbRead("Profile.GetProfileNameListByProfileCode", dbParams, connStr);

            // Return result
            return dtProfileName;
        }

        /// <summary>
        ///     Get ProfileName Structure
        /// </summary>
        /// <param name="dsProfileName"></param>
        /// <returns></returns>
        public bool GetProfileNameStructure(DataSet dsProfileName, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("Profile.GetProfileNameList", dsProfileName, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}