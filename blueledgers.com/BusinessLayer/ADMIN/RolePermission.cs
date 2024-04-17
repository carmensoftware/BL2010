using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.ADMIN
{
    public class RolePermission : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Instructor.
        /// </summary>
        public RolePermission()
        {
            SelectCommand = "SELECT * FROM [ADMIN].[RolePermission]";
            TableName = "RolePermission";
        }

        /// <summary>
        ///     Get RolePermission by RoleName.
        /// </summary>
        /// <param name="dsRolePermission"></param>
        /// <param name="roleName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsRolePermission, string roleName, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RoleName", roleName);

            return DbRetrieve("dbo.ADMIN_RolePermission_GetList_RoleName", dsRolePermission, dbParams, TableName,
                connStr);
        }

        /// <summary>
        /// Get permission list
        /// </summary>
        /// <param name="dsRolePermission"></param>
        /// <param name="roleName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string roleName, string connStr)
        {
            // Added on: 28/09/2017, By:Fon
            DataSet dsRolePermission = new DataSet();
            bool getlst = GetList(dsRolePermission, roleName, connStr);
            if (getlst)
            {
                return dsRolePermission.Tables[TableName];
            }
            return null;
        }

        /// <summary>
        /// Get permission 's value
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public int GetPermission(string roleName, string moduleID, string connStr)
        {
            // Added on: 28/09/2017, By: Fon
            int returnV = 0;
            DataTable dt = GetList(roleName, connStr);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow[] drs = dt.Select(string.Format("ModuleID = '{0}'", moduleID));
                    if (drs.Length > 0)
                        returnV = int.Parse(drs[0]["Permission"].ToString());
                }
            }

            return returnV;
        }

        /// <summary>
        /// Get page permission
        /// </summary>
        /// <param name="moduleID"></param>
        /// <param name="loginName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetPagePermission(string moduleID, string loginName, string connStr)
        {
            // Column Name: View | Create/Edit | Delete
            // View Only (1) = 1 0 0
            // View and Create/Edit (3) = 1 1 0
            // Full (7) = 1 1 1

            DataSet dsRolePermission = new DataSet();
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@loginName", loginName);
            dbParams[1] = new DbParameter("@moduleID", moduleID);

            if (DbRetrieve("[ADMIN].[GetPagePermissionByLogin]", dsRolePermission, dbParams, TableName, connStr))
            {
                if (dsRolePermission.Tables[TableName].Rows.Count > 0)
                    return int.Parse(dsRolePermission.Tables[TableName].Rows[0][0].ToString());
            }
            return 0;
        }

        /// <summary>
        ///     Save RolePermission Changed to Database.
        /// </summary>
        /// <param name="dsRolePermission"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsRolePermission, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsRolePermission, SelectCommand, TableName);

            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        /// Commit to DataBase
        /// </summary>
        /// <param name="dsRolePermission"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsRolePermission, string connStr)
        {
            // Added on: 30/09/2017, By: Fon
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsRolePermission, SelectCommand, TableName);

            return DbCommit(dbSaveSource, connStr);
        }


        #endregion
    }
}