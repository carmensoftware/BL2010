using System;
using System.Data;
using System.Globalization;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.Application
{
    public class Menu : DbHandler
    {
        /// <summary>
        ///     Empty constructor
        /// </summary>
        public Menu()
        {
            SelectCommand = "SELECT * FROM [Application].[Menu]";
            TableName = "Menu";
        }

        /// <summary>
        ///     Get Menu data by menu ID.
        /// </summary>
        /// <param name="dsMenu"></param>
        /// <param name="menuID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsMenu, int menuID, string connStr)
        {
            // Create parameter
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@MenuID", menuID.ToString(CultureInfo.InvariantCulture));

            // Get data and return vale
            return DbRetrieve("[Application].GetMenuByMenuID", dsMenu, dbParam, TableName, connStr);
        }

        /// <summary>
        ///     Get all active or inactive menu data.
        /// </summary>
        /// <param name="dsMenu"></param>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsMenu, bool isActive, string connStr)
        {
            // Create parameter
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@IsActive", isActive.ToString());

            // Get data and return value
            return DbRetrieve("[Application].GetMenuListByIsActive", dsMenu, dbParam, TableName, connStr);
        }

        /// <summary>
        ///     Get all active or inactive menu data.
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(bool isActive, string connStr)
        {
            var dsMenu = new DataSet();

            // Get data
            var result = GetList(dsMenu, isActive, connStr);

            // Return value
            if (result)
            {
                return dsMenu.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get all root menu
        /// </summary>
        /// <returns></returns>
        public DataTable GetRoot(string connStr)
        {
            var dsMenu = new DataSet();

            // Get data
            var result = DbRetrieve("[Application].GetMenuListRoot", dsMenu, null, TableName, connStr);

            // Return value
            if (result)
            {
                return dsMenu.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get all child menu depend on ParentMenuID
        /// </summary>
        /// <param name="parentMenuID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetChild(int parentMenuID, string connStr)
        {
            var dsMenu = new DataSet();

            // Create Paramenter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ParentMenuID", parentMenuID.ToString(CultureInfo.InvariantCulture));

            // Get data
            var result = DbRetrieve("[Application].GetMenuListChild", dsMenu, dbParams, TableName, connStr);

            // Return value
            if (result)
            {
                return dsMenu.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Determine the root menu has or hasn't child menu.
        /// </summary>
        /// <param name="parentMenuID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool HasChild(int parentMenuID, string connStr)
        {
            var dtChild = GetChild(parentMenuID, connStr);

            if (dtChild != null)
            {
                if (dtChild.Rows.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Get Navigate URL from specicified Menu ID
        /// </summary>
        /// <param name="menuID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetNavigateURL(int menuID, string connStr)
        {
            var dsMenu = new DataSet();
            var navigateURL = string.Empty;

            var result = Get(dsMenu, menuID, connStr);

            if (result)
            {
                if (dsMenu.Tables[TableName] != null)
                {
                    if (dsMenu.Tables[TableName].Rows.Count > 0)
                    {
                        if (dsMenu.Tables[TableName].Rows[0]["NavigateURL"] != DBNull.Value)
                        {
                            navigateURL = dsMenu.Tables[TableName].Rows[0]["NavigateURL"].ToString();
                        }
                    }
                }
            }

            return navigateURL;
        }

        /// <summary>
        ///     Get menu description depend on Language
        /// </summary>
        /// <param name="menuID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetDescription(int menuID, string connStr)
        {
            var sysParameter = new ProjectAdmin.SysParameter();
            var dsMenu = new DataSet();
            var description = string.Empty;

            var result = Get(dsMenu, menuID, connStr);

            if (result)
            {
                if (dsMenu.Tables[TableName] != null)
                {
                    if (dsMenu.Tables[TableName].Rows.Count > 0)
                    {
                        if (sysParameter.GetValue("System", "Language", connStr).ToUpper() == "EN")
                        {
                            if (dsMenu.Tables[TableName].Rows[0]["Description1"] != DBNull.Value)
                            {
                                description = dsMenu.Tables[TableName].Rows[0]["Description1"].ToString();
                            }
                        }
                        else
                        {
                            if (dsMenu.Tables[TableName].Rows[0]["Description2"] != DBNull.Value)
                            {
                                description = dsMenu.Tables[TableName].Rows[0]["Description2"].ToString();
                            }
                        }
                    }
                }
            }

            return description;
        }
    }
}