using System.Data;
using Blue.DAL;

namespace Blue.BL.Application
{
    public class UserShortcut : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty contructor
        /// </summary>
        public UserShortcut()
        {
            SelectCommand = "SELECT * FROM Application.UserShortcut";
            TableName = "UserShortcut";
        }

        /// <summary>
        ///     Get user shortcut by URL and UserID
        /// </summary>
        /// <param name="dsUserShortcut"></param>
        /// <param name="userId"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsUserShortcut, int userId, string URL, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@UserID", userId.ToString());
            dbParams[1] = new DbParameter("@URL", URL);

            return DbRetrieve("[Application].[GetUserShortcutListByUserIDURL]", dsUserShortcut, dbParams, TableName,
                connStr);
        }

        /// <summary>
        ///     Get all user shortcut
        /// </summary>
        /// <param name="dsUserShortcut"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsUserShortcut, string connStr)
        {
            return DbRetrieve("Application.GetUserShortcutList", dsUserShortcut, null, TableName, connStr);
        }

        /// <summary>
        ///     Get user shortcut list by user id.
        /// </summary>
        /// <param name="dsUserShortcut"></param>
        /// <param name="userId"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsUserShortcut, int userId, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@UserID", userId.ToString());

            return DbRetrieve("Application.GetUserShortcutListByUserID", dsUserShortcut, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get user shortcut list by user id.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(int userId, string connStr)
        {
            var dsUserShortcut = new DataSet();

            var retrieved = GetList(dsUserShortcut, userId, connStr);

            if (retrieved)
            {
                return dsUserShortcut.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsUserShortCut"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsUserShortCut, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("Application.GetUserShortcutList", dsUserShortCut, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetMaxID(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("Application.GetUserShortCutMaxID", null, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Check existing favorite.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="URL"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool IsExist(int userId, string URL, string connStr)
        {
            var dsUserShortcut = new DataSet();

            var retrieved = Get(dsUserShortcut, userId, URL, connStr);

            if (retrieved)
            {
                if (dsUserShortcut.Tables[TableName].Rows.Count > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        ///     This function is used for add page URL to user favorite's page.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="URL"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Add(string name, string URL, int userId, string connStr)
        {
            var dsUserShortcut = new DataSet();

            var getStruct = GetStructure(dsUserShortcut, connStr);

            if (getStruct)
            {
                var drUserShortcut = dsUserShortcut.Tables[TableName].NewRow();

                drUserShortcut["ID"] = GetMaxID(connStr) + 1;
                drUserShortcut["Name"] = name;
                drUserShortcut["URL"] = URL;
                drUserShortcut["UserID"] = userId;

                dsUserShortcut.Tables[TableName].Rows.Add(drUserShortcut);

                return Save(dsUserShortcut, connStr);
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsUserShortCut"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsUserShortCut, string connStr)
        {
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsUserShortCut, SelectCommand, TableName);

            // Save to database
            return DbCommit(dbSaveSorce, connStr);
        }

        #endregion
    }
}