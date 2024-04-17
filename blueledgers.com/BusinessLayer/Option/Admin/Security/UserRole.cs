using System.Data;
using Blue.DAL;

namespace Blue.BL.Option.Admin.Security
{
    public class UserRole : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empy contructor.
        /// </summary>
        public UserRole()
        {
            SelectCommand = "SELECT * FROM [ADMIN].[UserRole]";
            TableName = "UserRole";
        }

        /// <summary>
        ///     Gets user role data by LoginName.
        /// </summary>
        /// <param name="dsUserRole"></param>
        /// <param name="LoginName"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsUserRole, string LoginName, string ConnStr)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@LoginName", LoginName);

            return DbRetrieve("[dbo].[ADMIN_UserRole_GetList_LoginName]", dsUserRole, dbParam, TableName, ConnStr);
        }

        /// <summary>
        ///     Gets user role data by RoleName.
        /// </summary>
        /// <param name="dsUserRole"></param>
        /// <param name="RoleName"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetListByRoleName(DataSet dsUserRole, string RoleName, string ConnStr)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@RoleName", RoleName);

            return DbRetrieve("[dbo].[ADMIN_UserRole_GetList_RoleName]", dsUserRole, dbParam, TableName, ConnStr);
        }

        /// <summary>
        ///     Get Structure of Role Table.
        /// </summary>
        /// <param name="dsUserRole"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsUserRole, string ConnStr)
        {
            return DbRetrieve("[dbo].[ADMIN_UserRole_GetSchema]", dsUserRole, null, TableName, ConnStr);
        }

        /// <summary>
        ///     Commit changed to database.
        /// </summary>
        /// <param name="dsUserRole"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsUserRole, string ConnStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsUserRole, SelectCommand, TableName);
            return DbCommit(dbSaveSource, ConnStr);
        }

        #endregion
    }
}