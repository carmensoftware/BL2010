using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.IM
{
    public class IMDelete : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        public IMDelete()
        {
            SelectCommand = "select * from IM.[Delete]";
            TableName = "[Delete]";
        }

        public bool GetList(DataSet dsDelete, string strConn)
        {
            return DbRetrieve("IM.GetDeleteList", dsDelete, null, TableName, strConn);
        }

        public bool GetMessage(DataSet dsDelete, string deleteNo, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DeleteNo", deleteNo);

            return DbRetrieve("IM.GetDeleteRead", dsDelete, dbParams, TableName, strConn);
        }

        public bool GetSearch(DataSet dsSearch, string search, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Search", search);

            return DbRetrieve("IM.GetDeleteSearch", dsSearch, dbParams, TableName, strConn);
        }

        public DataTable GetUserList(string loginUser, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LoginUser", loginUser);

            return DbRead("IM.GetDeleteUserList", dbParams, strConn);
        }

        public bool Save(DataSet dsSaved, string strConn)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSaved, SelectCommand, TableName);

            return DbCommit(dbSaveSource, strConn);
        }

        public int GetCount(DataSet dsDelete, string strConn)
        {
            return DbReadScalar("IM.GetDeleteCountUnRead", null, strConn);
        }

        public int GetNewNo(string strConn)
        {
            return DbReadScalar("IM.GetDeleteNewNo", null, strConn);
        }

        #endregion
    }
}