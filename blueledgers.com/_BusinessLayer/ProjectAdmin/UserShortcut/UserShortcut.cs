using System.Data;
using Blue.DAL;

namespace Blue.BL.ProjectAdmin
{
    public class UserShortcut : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty contructor
        /// </summary>
        public UserShortcut()
        {
            SelectCommand = "SELECT * FROM ProjectAdmin.UserShortcut";
            TableName = "UserShortcut";
        }

        /// <summary>
        ///     Get user shortcut list using user id
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DataTable GetUserShortcutListByUserID(int userID, string connStr)
        {
            var dtUserShortcut = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@UserID", userID.ToString());

            // Get data
            dtUserShortcut = DbRead("ProjectAdmin.GetUserShortcutListByUserID", dbParams, connStr);

            // Return result
            return dtUserShortcut;
        }

        #endregion
    }
}