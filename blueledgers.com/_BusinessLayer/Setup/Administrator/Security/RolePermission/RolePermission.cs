using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Security
{
    public class RolePermission : DbHandler
    {
        /// <summary>
        ///     Empty constructure
        /// </summary>
        public RolePermission()
        {
            SelectCommand = "SELECT * FROM ProjectAdmin.RolePermission";
            TableName = "RolePermission";
        }

        /// <summary>
        ///     Get role's permission list
        /// </summary>
        /// <returns></returns>
        public bool GetList(DataSet dsRolePermission, string ConnectionString)
        {
            return DbRetrieve("ProjectAdmin.GetRolePermissionList", dsRolePermission, null, TableName, ConnectionString);
        }

        /// <summary>
        ///     Get role's permission data by RoleID
        /// </summary>
        /// <returns></returns>
        public bool GetList(DataSet dsRolePermission, int RoleID, string ConnectionString)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@RoleID", RoleID.ToString());

            return DbRetrieve("ProjectAdmin.GetRolePermissionListByRoleID", dsRolePermission, dbParam, TableName,
                ConnectionString);
        }

        /// <summary>
        ///     Get role's permission data by SchemaName, TableName and FieldName
        /// </summary>
        /// <returns></returns>
        public bool GetList(DataSet dsRolePermission, string SchemaName, string TableName, string FieldName,
            string ConnectionString)
        {
            var dbParam = new DbParameter[3];
            dbParam[0] = new DbParameter("@SchemaName", SchemaName);
            dbParam[1] = new DbParameter("@TableName", TableName);
            dbParam[2] = new DbParameter("@FieldName", FieldName);

            return DbRetrieve("ProjectAdmin.GetRolePermissionListBySchemaNameTableNameFieldName", dsRolePermission,
                dbParam, this.TableName, ConnectionString);
        }

        /// <summary>
        ///     Get RolePermission Table structure.
        /// </summary>
        /// <param name="dsRolePermision"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsRolePermision, string ConnectionString)
        {
            return DbRetrieveSchema("ProjectAdmin.GetRolePermissionList", dsRolePermision, null, TableName,
                ConnectionString);
        }

        /// <summary>
        ///     Get IsVisible data of specified field.
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="SchemaName"></param>
        /// <param name="TableName"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public bool GetIsVisible(int UserID, string SchemaName, string TableName, string FieldName,
            string ConnectionString)
        {
            var result = false;

            var dbParam = new DbParameter[4];
            dbParam[0] = new DbParameter("@UserID", UserID.ToString());
            dbParam[1] = new DbParameter("@SchemaName", SchemaName);
            dbParam[2] = new DbParameter("@TableName", TableName);
            dbParam[3] = new DbParameter("@FieldName", FieldName);

            var dtRolePermission = DbRead("ProjectAdmin.GetRolePermissionIsVisible", dbParam, ConnectionString);

            if (dtRolePermission != null)
            {
                if (dtRolePermission.Rows.Count > 0)
                {
                    foreach (DataRow drRolePermission in dtRolePermission.Rows)
                    {
                        if (drRolePermission["IsVisible"] != DBNull.Value)
                        {
                            result = result || (bool) drRolePermission["IsVisible"];
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="SchemaName"></param>
        /// <param name="TableName"></param>
        /// <param name="FieldName"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetIsModify(int UserID, string SchemaName, string TableName, string FieldName,
            string ConnectionString)
        {
            var result = false;

            var dbParam = new DbParameter[4];
            dbParam[0] = new DbParameter("@UserID", UserID.ToString());
            dbParam[1] = new DbParameter("@SchemaName", SchemaName);
            dbParam[2] = new DbParameter("@TableName", TableName);
            dbParam[3] = new DbParameter("@FieldName", FieldName);

            var dtRolePermission = DbRead("ProjectAdmin.GetRolePermissionIsModify", dbParam, ConnectionString);

            if (dtRolePermission != null)
            {
                if (dtRolePermission.Rows.Count > 0)
                {
                    foreach (DataRow drRolePermission in dtRolePermission.Rows)
                    {
                        if (drRolePermission["IsModify"] != DBNull.Value)
                        {
                            result = result || (bool) drRolePermission["IsModify"];
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsRolePermission"></param>
        /// <param name="roleID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetRolePermissionByRoleID(DataSet dsRolePermission, int roleID, string connStr)
        {
            try
            {
                // Paramter value assign to dbparameter array.
                var dbParams = new DbParameter[1];
                dbParams[0] = new DbParameter("@RoleID", Convert.ToString(roleID));

                return DbRetrieve("ProjectAdmin.GetRolePermissionListByRoleID", dsRolePermission, dbParams, TableName,
                    connStr);
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                return false;
            }
        }

        /// <summary>
        ///     Get RolePermission data with specified RoleID, Schema Name and Table Name.
        /// </summary>
        /// <param name="dsRolePermission"></param>
        /// <param name="RoleID"></param>
        /// <param name="SchemaName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsRolePermission, int RoleID, string SchemaName, string TableName, string Connection)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@RoleID", RoleID.ToString());
            dbParams[1] = new DbParameter("@SchemaName", SchemaName);
            dbParams[2] = new DbParameter("@TableName", TableName);

            return DbRetrieve("ProjectAdmin.GetRolePermissionByRoleID_Schema_Table", dsRolePermission, dbParams,
                this.TableName, Connection);
        }

        /// <summary>
        ///     Save data
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
    }
}