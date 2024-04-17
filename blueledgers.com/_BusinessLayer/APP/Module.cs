using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.APP
{
    public class Module : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empy contructor.
        /// </summary>
        public Module()
        {
            SelectCommand = "SELECT * FROM [APP].[Module]";
            TableName = "Module";
        }

        /// <summary>
        ///     Get root menu that call CLR procedure for version ibis.
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetRoot(string loginName, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LoginName", loginName);

            return DbRead("[dbo].[APP_Module_GetRoot_LoginName]", dbParams, connStr);
            //return DbRead("[APP].[GetModuleRootByLoginName]", dbParams, ConnStr);
        }

        /// <summary>
        ///     Get root menu.
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetRoot2(string loginName, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LoginName", loginName);

            return DbRead("[APP].[GetModuleRootByLoginName]", dbParams, connStr);
        }

        /// <summary>
        ///     Get all active module that call CLR procedure for version ibis.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetActiveList(string connStr)
        {
            return DbRead("dbo.APP_Module_GetActiveList", null, connStr);
            //return DbRead("[APP].[GetModuleActiveList]", null, ConnStr);
        }

        /// <summary>
        ///     Get all active module.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetActiveList2(string connStr)
        {
            return DbRead("[APP].[GetModuleActiveList]", null, connStr);
        }

        /// <summary>
        ///     Get all active module.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetListModule(string connStr)
        {
            return DbRead("[APP].[GetListModule]", null, connStr);
        }

        /// <summary>
        ///     Get all active module.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetListSubModule(string connStr)
        {
            return DbRead("[APP].[GetListSubModule]", null, connStr);
        }
        
        /// <summary>
        ///     Get all child menu of specified ParentID that call CLR procedure for version ibis.
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string parentID, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ID", parentID);

            return DbRead("APP_Module_GetList_ID", dbParams, connStr);
            //return DbRead("[APP].[GetModuleListByID]", dbParams, ConnStr);
        }

        /// <summary>
        ///     Get all child menu of specified ParentID that call CLR procedure for version ibis.
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList2(string parentID, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ID", parentID);

            return DbRead("[APP].[GetModuleListByID]", dbParams, connStr);
        }

        /// <summary>
        ///     Get all child menu of specified ParentID and LoginName that call CLR procedure for version ibis.
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="loginName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string parentID, string loginName, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@ID", parentID);
            dbParams[1] = new DbParameter("@LoginName", loginName);

            return DbRead("APP_Module_GetList_ID_LoginName", dbParams, connStr);
            //return DbRead("[APP].[GetModuleListByID_LoginName]", dbParams, ConnStr);
        }

        /// <summary>
        ///     Get all child menu of specified ParentID and LoginName.
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="loginName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList2(string parentID, string loginName, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@ID", parentID);
            dbParams[1] = new DbParameter("@LoginName", loginName);

            return DbRead("[APP].[GetModuleListByID_LoginName]", dbParams, connStr);
        }

        /// <summary>
        ///     Determine the specified id has child or not. [for version ibis]
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool HasChild(string parentID, string connStr)
        {
            var dtModule = GetList(parentID, connStr);

            if (dtModule.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Determine the specified id has child or not.
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool HasChild2(string parentID, string connStr)
        {
            var dtModule = GetList2(parentID, connStr);

            if (dtModule.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}