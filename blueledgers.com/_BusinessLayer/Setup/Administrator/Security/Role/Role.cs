using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Security
{
    public class Role : DbHandler
    {
        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Role()
        {
            SelectCommand = "SELECT * FROM ProjectAdmin.Role";
            TableName = "Role";
        }

        /// <summary>
        ///     Get all role data.
        /// </summary>
        /// <param name="dsRole"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsRole, string connStr)
        {
            return DbRetrieve("ProjectAdmin.GetRoleList", dsRole, null, TableName, connStr);
        }

        /// <summary>
        ///     Get all data
        /// </summary>
        /// <param name="dsRole"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetActiveList(string connStr)
        {
            var dsRole = new DataSet();

            var result = DbRetrieve("ProjectAdmin.GetRoleActiveList", dsRole, null, TableName, connStr);

            if (result)
            {
                return dsRole.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get role data by RoleID.
        /// </summary>
        /// <param name="dsRole"></param>
        /// <param name="roleID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsRole, int roleID, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@roleID", Convert.ToString(roleID));

            return DbRetrieve("ProjectAdmin.GetRoleListByRoleID", dsRole, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get Role Table structure
        /// </summary>
        /// <param name="dsRole"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsRole, string ConnectionString)
        {
            return DbRetrieveSchema("ProjectAdmin.GetRoleList", dsRole, null, TableName, ConnectionString);
        }

        /// <summary>
        ///     Get Max roldID.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewID(string CoonectionString)
        {
            var NewID = DbReadScalar("ProjectAdmin.GetRoleNewID", null, CoonectionString);

            // Return result
            return NewID;
        }


        /// <summary>
        ///     Get rolelist by roleID.
        /// </summary>
        /// <param name="dsRole"></param>
        /// <param name="roleID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetRoleListByRoleID(DataSet dsRole, int roleID, string connStr)
        {
            var result = false;

            // Create parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@roleID", Convert.ToString(roleID));


            // Get data
            result = DbRetrieve("ProjectAdmin.GetRoleListByRoleID", dsRole, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get rolelist by roleid.
        /// </summary>
        /// <param name="dsRole"></param>
        /// <param name="roleID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetRoleListByRoleIDTable(DataSet dsRole, int roleID, string connStr)
        {
            var dtRole = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@RoleID", Convert.ToString(roleID));

            // Get data
            dtRole = DbRead("ProjectAdmin.GetRoleListByRoleID", dbParams, connStr);

            // Return result
            return dtRole;
        }

        /// <summary>
        ///     Save process.
        /// </summary>
        /// <param name="dsRole"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsRole, string connStr)
        {
            // สร้าง SaveSource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsRole, SelectCommand, TableName);

            // เรียก dbCommit โดยส่ง SaveSource object เป็น parameter
            DbCommit(dbSaveSource, connStr);

            return true;
        }

        /// <summary>
        ///     Save process.
        /// </summary>
        /// <param name="dsRole"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsRole, bool IncludeRolePermission, string connStr)
        {
            if (IncludeRolePermission)
            {
                var rolePermission = new RolePermission();

                var dbSaveSource = new DbSaveSource[2];
                dbSaveSource[0] = new DbSaveSource(dsRole, rolePermission.SelectCommand, rolePermission.TableName);
                dbSaveSource[1] = new DbSaveSource(dsRole, SelectCommand, TableName);

                return DbCommit(dbSaveSource, connStr);
            }
            return Save(dsRole, connStr);
        }
    }
}