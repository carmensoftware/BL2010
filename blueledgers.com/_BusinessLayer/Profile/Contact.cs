using System.Data;
using Blue.DAL;

namespace Blue.BL.Profile
{
    public class Contact : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Contact()
        {
            SelectCommand = "SELECT * FROM Profile.Contact";
            TableName = "Contact";
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="dsContact"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetContactList(string profileCode, DataSet dsContact, string connStr)
        {
            var result = false;
            var dtContact = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            result = DbRetrieve("Profile.GetContactListByProfileCode", dsContact, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get contact detail using contact id
        /// </summary>
        /// <param name="profileCode"></param>
        /// <returns></returns>
        public DataTable GetContactListByProfileCode(string profileCode, string connStr)
        {
            var dtContact = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            dtContact = DbRead("Profile.GetContactListByProfileCode", dbParams, connStr);

            // Return result
            return dtContact;
        }

        /// <summary>
        ///     Get Contact Structure
        /// </summary>
        /// <param name="dsContact"></param>
        /// <returns></returns>
        public bool GetContactStructure(DataSet dsContact, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("Profile.GetContactList", dsContact, null, TableName, connStr);

            // Return result
            return result;
        }

        public bool GetContactList(DataSet dsContact, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieve("Profile.GetContactList", dsContact, null, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        ///     Get contact max Id
        /// </summary>
        /// <returns></returns>
        public int GetContactMaxID(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("Profile.GetContactMaxID", null, connStr);

            // Return result
            return result;
        }


        public bool Save(DataSet dsContactList, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsContactList, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}