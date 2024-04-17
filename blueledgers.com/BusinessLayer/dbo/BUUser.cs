using System.Data;
using Blue.DAL;

namespace Blue.BL.dbo
{
    public class BUUser : DbHandler
    {
        public BUUser()
        {
            SelectCommand = "SELECT * FROM [BUUser]";
            TableName = "BUUser";
        }

        /// <summary>
        ///     Get business unit related to user.
        /// </summary>
        /// <param name="dsBuUser"></param>
        /// <param name="BuCode"></param>
        /// <param name="LoginName"></param>
        /// <param name="MsgError"></param>
        /// <returns></returns>
        public bool Get(DataSet dsBuUser, string BuCode, string LoginName, ref string MsgError)
        {
            // Create parameters
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@BuCode", BuCode);
            dbParams[1] = new DbParameter("@LoginName", LoginName);

            var result = DbRetrieve("dbo.BuUser_Get_BuCode_LoginName", dsBuUser, dbParams, TableName);

            if (!result)
            {
                MsgError = "Msg000";
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Gets all active business unit data related to specified login name.
        /// </summary>
        /// <param name="dsBUUser"></param>
        /// <param name="LoginName"></param>
        /// <param name="MsgError"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsBUUser, string LoginName, ref string MsgError)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LoginName", LoginName);

            var result = DbRetrieve("dbo.BUUser_GetActiveList_LoginName", dsBUUser, dbParams, TableName);

            if (result)
            {
                var pk = new DataColumn[1];
                pk[0] = dsBUUser.Tables[TableName].Columns["BuCode"];

                dsBUUser.Tables[TableName].PrimaryKey = pk;
            }
            else
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        public bool GetList(DataSet dsBuUser, string LoginName)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LoginName", LoginName);

            return DbRetrieve("dbo.BuUser_GetList_LoginName", dsBuUser, dbParams, TableName);
        }


        public DataTable GetList(string LoginName, ref string MsgError)
        {
            var dsBUUser = new DataSet();

            var result = GetList(dsBUUser, LoginName, ref MsgError);

            if (result)
            {
                return dsBUUser.Tables[TableName];
            }
            return null;
        }


        /// <summary>
        ///     Get number of user in businees unit by BuCode.
        /// </summary>
        /// <param name="BuCode"></param>
        /// <returns></returns>
        public int GetUserNo(string BuCode)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BuCode", BuCode);

            return DbReadScalar("[dbo].[BuUser_GetUserNo]", dbParams);
        }

        /// <summary>
        ///     Get BuUser Table Structure
        /// </summary>
        /// <param name="dsBuUser"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsBuUser)
        {
            return DbRetrieve("dbo.BuUser_GetShema", dsBuUser, null, TableName);
        }

        /// <summary>
        ///     Commit user changed to database.
        /// </summary>
        /// <param name="dsPrefix"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsBuUser)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsBuUser, SelectCommand, TableName);
            return DbCommit(dbSaveSource, string.Empty);
        }

    }
}