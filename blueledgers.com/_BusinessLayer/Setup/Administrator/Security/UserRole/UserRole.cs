using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Security
{
    public class UserRole : DbHandler
    {
        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public UserRole()
        {
            SelectCommand = "SELECT * FROM ProjectAdmin.UserRole";
            TableName = "UserRole";
        }

        /// <summary>
        ///     Get all user role data.
        /// </summary>
        /// <param name="dsUserRole"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsUserRole, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("ProjectAdmin.GetUserRoleList", dsUserRole, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get new id
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewUserRoleID(string connStr)
        {
            var dsTmp = new DataSet();
            DbRetrieve("ProjectAdmin.GetMaxRole", dsTmp, null, TableName, connStr);

            return (int) dsTmp.Tables[TableName].Rows[0]["UserRoleID"];
        }

        /// <summary>
        ///     Get schema
        /// </summary>
        /// <param name="dsReport"></param>
        /// <param name="connStr"></param>
        public void GetSchemaUserRole(DataSet dsUserRole, string connStr)
        {
            DbRetrieveSchema("ProjectAdmin.GetUserRoleList", dsUserRole, null, TableName, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsUserRole"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetUserRoleByUserID(DataSet dsUserRole, string userID, string connStr)
        {
            try
            {
                // Paramter value assign to dbparameter array.
                var dbParams = new DbParameter[1];
                dbParams[0] = new DbParameter("@UserID", userID);

                return DbRetrieve("ProjectAdmin.GetUserRoleByUserID", dsUserRole, dbParams, TableName, connStr);
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                return false;
            }
        }

        /// <summary>
        ///     Get UserRole data with specified RoleID.
        /// </summary>
        /// <param name="UserRole"></param>
        /// <param name="RoleID"></param>
        /// <param name="Connection"></param>
        /// <returns></returns>
        public bool GetList(DataSet UserRole, int RoleID, string Connection)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RoleID", RoleID.ToString());

            return DbRetrieve("ProjectAdmin.GetUserRoleByRoleID", UserRole, dbParams, TableName, Connection);
        }

        /// <summary>
        ///     Get userRole by RoleID.
        /// </summary>
        /// <param name="dsUserRole"></param>
        /// <param name="roleID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable _GetUserRoleByRoleID(DataSet dsUserRole, int roleID, string connStr)
        {
            var dtUserRole = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@RoleID", Convert.ToString(roleID));

            // Get data
            dtUserRole = DbRead("ProjectAdmin.GetUserRoleByRoleID", dbParams, connStr);

            // Return result
            return dtUserRole;
        }
    }
}