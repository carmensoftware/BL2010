using System.Data;
using Blue.DAL;

namespace Blue.BL.dbo
{
    public class OnlineUser : DbHandler
    {
        #region "Attributes" 

        #endregion

        #region "Operations"

        public OnlineUser()
        {
            SelectCommand = "SELECT * FROM [dbo].[OnlineUser]";
            TableName = "OnlineUser";
        }

        public DataTable CountOnlineUserByUser(string User)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@UserId", User);
            var dtCountuser = DbRead("[dbo].[Count_UserOnlineByUser]", dbParams);
            return dtCountuser;
        }

        public DataTable CountOnlineUserByIP(string IP)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@UserIp", IP);
            var dtCountuserIP = DbRead("[dbo].[Count_UserOnlineByIP]", dbParams);
            return dtCountuserIP;
        }

        public bool GetStructure(DataSet dsOnlineUser)
        {
            //return DbRetrieve("dbo.PC_Po_GetSchema", dsPo, null, this.TableName, ConnStr);
            return DbRetrieve("[dbo].[GetOnlineUserSchema]", dsOnlineUser, null, TableName);
        }

        public bool UpdateUserOnline(DataSet dsSaving)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSaving, SelectCommand, TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource);
        }

        #endregion
    }
}