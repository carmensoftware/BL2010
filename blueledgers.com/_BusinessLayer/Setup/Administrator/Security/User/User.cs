using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Security
{
    public class User : DbHandler
    {
        #region "Attibuties"

        public int UserID { get; set; }

        public string Fname { get; set; }

        public string Lname { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int UpdatedBy { get; set; }

        #endregion

        #region "Operations"

        public User()
        {
            SelectCommand = "SELECT * FROM ProjectAdmin.[User]";
            TableName = "User";
        }

        /// <summary>
        ///     Get schema
        /// </summary>
        /// <param name="dsUser"></param>
        public void GetUserSchema(DataSet dsUser, string connStr)
        {
            DbRetrieveSchema("ProjectAdmin.GetUserList", dsUser, null, TableName, connStr);
        }

        /// <summary>
        ///     Get user using user ID
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <param name="categoryCode"></param>
        /// <returns></returns>
        public bool GetUser(DataSet dsUser, int userID, string connStr)
        {
            var result = false;

            // Create parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@UserID", userID.ToString());

            // Get data
            result = DbRetrieve("ProjectAdmin.GetUser", dsUser, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all user data.
        /// </summary>
        /// <param name="dsUser"></param>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsUser, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("ProjectAdmin.GetUserList", dsUser, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     get all user list.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            return DbRead("ProjectAdmin.GetUserList", null, connStr);
        }

        /// <summary>
        ///     Get user display name using user ID
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <returns></returns>
        public string GetUserDisplayName(int userID, string connStr)
        {
            var result = string.Empty;
            var dsUser = new DataSet();

            // Get data
            GetUser(dsUser, userID, connStr);

            // Return result
            if (dsUser.Tables[TableName] != null)
            {
                if (dsUser.Tables[TableName].Rows.Count > 0)
                {
                    result = dsUser.Tables[TableName].Rows[0]["Fname"] + " " +
                             dsUser.Tables[TableName].Rows[0]["Lname"];
                }
            }

            return result.Trim();
        }

        /// <summary>
        ///     Return datatable and get user name
        /// </summary>
        /// <param name="userName">Parameter value</param>
        /// <returns></returns>
        public string GetUserName(int UserID, string ConnectionString)
        {
            var dsUser = new DataSet();
            var userName = string.Empty;

            // Get data
            var retrieved = GetUser(dsUser, UserID, ConnectionString);

            // Return Result
            if (retrieved)
            {
                if (dsUser.Tables[TableName].Rows.Count > 0)
                {
                    userName = dsUser.Tables[TableName].Rows[0]["UserName"].ToString();
                }
            }

            return userName;
        }

        /// <summary>
        ///     Return datatable and get business unit.
        /// </summary>
        /// <param name="userName">parameter value</param>
        /// <param name="password">parameter value</param>
        /// <returns></returns>
        public DataTable GetUserInfoByUserName(string userName, string password, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@userName", userName);
            dbParams[1] = new DbParameter("@password", password);

            return DbRead("ProjectAdmin.GetUserInfoByUserName", dbParams, connStr);
        }

        /// <summary>
        ///     Return datatable and get forget password info.
        /// </summary>
        /// <param name="userName">parameter value.</param>
        /// <returns></returns>
        public DataTable Get_UserForgetInfoByUserName(string userName, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@userName", userName);

            return DbRead("ProjectAdmin.GetUserForgetInfoByUserName", dbParams, connStr);
        }

        /// <summary>
        ///     Get max user id
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewUserID(string connStr)
        {
            var dsTmp = new DataSet();
            DbRetrieve("ProjectAdmin.GetMaxUser", dsTmp, null, TableName, connStr);

            return (int) dsTmp.Tables[TableName].Rows[0]["UserID"];
        }

        /// <summary>
        ///     Save data to database
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, BL.Security.UserRole userRole, string connStr)
        {
            // Build savesource object
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, userRole.SelectCommand, userRole.TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        ///     Delete data from database
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Delete(DataSet savedData, string connStr)
        {
            var userRole = new UserRole();

            // Build savesource object
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, userRole.SelectCommand, userRole.TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, connStr);
        }


        /// <summary>
        ///     Save data to database
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            // Build savesource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}