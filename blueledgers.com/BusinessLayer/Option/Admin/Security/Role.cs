using System.Data;
using Blue.DAL;

namespace Blue.BL.Option.Admin.Security
{
    public class Role : DbHandler
    {
        #region "Attributes"

        private readonly ADMIN.RolePermission _rolePermission = new ADMIN.RolePermission();
        private readonly UserRole _userRole = new UserRole();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empy contructor.
        /// </summary>
        public Role()
        {
            SelectCommand = "SELECT * FROM [ADMIN].[Role]";
            TableName = "Role";
        }

        /// <summary>
        ///     Gets all role data.
        /// </summary>
        /// <param name="dsRole"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsRole, string ConnStr)
        {
            return DbRetrieve("[dbo].[ADMIN_Role_GetList]", dsRole, null, TableName, ConnStr);
        }

        /// <summary>
        ///     Gets all role data.
        /// </summary>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public DataTable GetList(string ConnStr)
        {
            var dsRole = new DataSet();

            var result = GetList(dsRole, ConnStr);

            if (result)
            {
                return dsRole.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get role data by role name.
        /// </summary>
        /// <param name="dsRole"></param>
        /// <param name="RoleName"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsRole, string RoleName, string ConnStr)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@RoleName", RoleName);

            return DbRetrieve("[dbo].[ADMIN_Role_Get_RoleName]", dsRole, dbParam, TableName, ConnStr);
        }

        /// <summary>
        ///     Get Structure of Role Table.
        /// </summary>
        /// <param name="dsRole"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsRole, string ConnStr)
        {
            return DbRetrieve("[dbo].[ADMIN_Role_GetSchema]", dsRole, null, TableName, ConnStr);
        }

        /// <summary>
        ///     Commit changed to database.
        /// </summary>
        /// <param name="dsRole"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsRole, string ConnStr)
        {
            var dbSaveSource = new DbSaveSource[3];
            dbSaveSource[0] = new DbSaveSource(dsRole, _userRole.SelectCommand, _userRole.TableName);
            dbSaveSource[1] = new DbSaveSource(dsRole, _rolePermission.SelectCommand, _rolePermission.TableName);
            dbSaveSource[2] = new DbSaveSource(dsRole, SelectCommand, TableName);
            return DbCommit(dbSaveSource, ConnStr);
        }

        /// <summary>
        ///     Commit changed to database.
        /// </summary>
        /// <param name="dsRole"></param>
        /// <param name="UserRoleCmd"></param>
        /// <param name="UserRoleTableName"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsRole, string ConnStr)
        {
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsRole, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsRole, _userRole.SelectCommand, _userRole.TableName);
            return DbCommit(dbSaveSource, ConnStr);
        }

        #endregion
    }
}