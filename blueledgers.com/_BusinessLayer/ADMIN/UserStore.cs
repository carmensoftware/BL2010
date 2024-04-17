using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.ADMIN
{
    public class UserStore : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Instructor.
        /// </summary>
        public UserStore()
        {
            SelectCommand = "SELECT * FROM [ADMIN].[UserStore]";
            TableName = "UserStore";
        }

        /// <summary>
        ///     Get UserStore by LoginName.
        /// </summary>
        /// <param name="dsUserStore"></param>
        /// <param name="loginName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsUserStore, string loginName, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LoginName", loginName);

            return DbRetrieve("dbo.ADMIN_UserStore_GetList_LoginName", dsUserStore, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Save UserStore Changed to Database.
        /// </summary>
        /// <param name="dsUserStore"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsUserStore, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsUserStore, SelectCommand, TableName);

            return DbCommit(dbSaveSource, connStr);
        }

        public int CountByLocationCode(string location, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LocateCode", location);

            return DbReadScalar("dbo.ADMIN_UserStore_CountByLocationCode", dbParams, connStr);
        }

        #endregion
    }
}