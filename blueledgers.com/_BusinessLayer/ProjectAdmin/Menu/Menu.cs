using System.Data;
using Blue.DAL;

namespace Blue.BL.ProjectAdmin
{
    public class Menu : DbHandler
    {
        #region "Attributies"

        private readonly SysParameter _sysParameter = new SysParameter();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty contructure
        /// </summary>
        public Menu()
        {
            SelectCommand = "SELECT * FROM ProjectAdmin.Menu";
            TableName = "Menu";
        }

        /// <summary>
        ///     Get all Menu data which MenuLevel is 0.
        /// </summary>
        /// <param name="dsMenu"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetRoot(ref DataSet dsMenu, string type, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Type", type);

            // Get data
            var result = DbRetrieve("ProjectAdmin.GetMenuListRoot", dsMenu, dbParams, "MenuRoot", connStr);

            return result;
        }

        /// <summary>
        ///     Get all Menu data which MenuLevel is 0.
        /// </summary>
        /// <param name="dsMenu"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetChild(ref DataSet dsMenu, string type, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Type", type);

            // Get data
            var result = DbRetrieve("ProjectAdmin.GetMenuListChild", dsMenu, dbParams, "MenuChild", connStr);

            return result;
        }

        /// <summary>
        ///     Get menu data using menu id.
        /// </summary>
        /// <param name="dsMenu"></param>
        /// <param name="menuID"></param>
        /// <returns></returns>
        public bool GetMenu(DataSet dsMenu, int menuID, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@MenuID", menuID.ToString());

            // Get data
            result = DbRetrieve("ProjectAdmin.GetMenu", dsMenu, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get menu navigate URL
        /// </summary>
        /// <param name="menuID"></param>
        /// <returns></returns>
        public string GetMenuNavicateURL(int menuID, string connStr)
        {
            var dsMenu = new DataSet();
            var navicateURL = string.Empty;

            // Get data
            GetMenu(dsMenu, menuID, connStr);

            // Return result
            if (dsMenu.Tables[TableName].Rows.Count > 0)
            {
                navicateURL = dsMenu.Tables[TableName].Rows[0]["NavigateURL"].ToString();
            }

            return navicateURL;
        }

        /// <summary>
        ///     Get menu description
        /// </summary>
        /// <param name="menuID"></param>
        /// <returns></returns>
        public string GetMenuDescription(int menuID, string connStr)
        {
            var dsMenu = new DataSet();
            var description = string.Empty;
            var language = _sysParameter.GetValue("System", "Language", connStr);

            // Get data
            GetMenu(dsMenu, menuID, connStr);

            // Return result
            if (dsMenu.Tables[TableName].Rows.Count > 0)
            {
                if (language.ToUpper() == "EN")
                {
                    description = dsMenu.Tables[TableName].Rows[0]["Description1"].ToString();
                }
                else
                {
                    description = dsMenu.Tables[TableName].Rows[0]["Description2"].ToString();
                }
            }

            return description;
        }

        #endregion
    }
}