using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.Setup.Admin
{
    public class User : DbHandler
    {
        /// <summary>
        ///     Empry Constructor
        /// </summary>
        public User()
        {
            SelectCommand = "SELECT * FROM ProjectAdmin.User";
            TableName = "User";
        }

        /// <summary>
        ///     Get user data by User's ID.
        /// </summary>
        /// <param name="dsUser"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(ref DataSet dsUser, int userID, string connStr)
        {
            return true;
        }

        /// <summary>
        ///     Get user data by UserName.
        /// </summary>
        /// <param name="dsUser"></param>
        /// <param name="userName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(ref DataSet dsUser, string userName, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@userName", userName);

            return DbRetrieve("ProjectAdmin.GetUserByUserName", dsUser, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get user data by UserName and Password.
        /// </summary>
        /// <param name="dsUser"></param>
        /// <param name="userName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(ref DataSet dsUser, string userName, string Password, string connStr)
        {
            return true;
        }
    }
}