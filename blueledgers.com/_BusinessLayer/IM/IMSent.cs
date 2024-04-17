using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.IM
{
    public class IMSent : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        public IMSent()
        {
            SelectCommand = "select * from IM.Sent";
            TableName = "Sent";
        }

        public bool GetList(DataSet dsSent, string strConn)
        {
            return DbRetrieve("IM.GetSEntList", dsSent, null, TableName, strConn);
        }

        /// <summary>
        ///     Get IM.Sent Table Structure.
        /// </summary>
        /// <param name="dsSent"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsSent, string ConnStr)
        {
            return DbRetrieveSchema("IM.GetSentList", dsSent, null, TableName, ConnStr);
        }

        public bool GetMessage(DataSet dsSent, string sentNo, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@SentNo", sentNo);

            return DbRetrieve("IM.GetSentRead", dsSent, dbParams, TableName, strConn);
        }

        public bool GetSearch(DataSet dsSearch, string search, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Search", search);

            return DbRetrieve("IM.GetSentSearch", dsSearch, dbParams, TableName, strConn);
        }

        public DataTable GetUserList(string loginUser, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LoginUser", loginUser);

            return DbRead("IM.GetSentUserList", dbParams, strConn);
        }

        public bool Save(DataSet dsSaved, string strConn)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSaved, SelectCommand, TableName);

            return DbCommit(dbSaveSource, strConn);
        }

        public int GetNewNo(string strConn)
        {
            return DbReadScalar("IM.GetSentNewNo", null, strConn);
        }

        #endregion
    }
}