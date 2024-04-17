using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.IM
{
    public class IMSearchEmail : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        public IMSearchEmail()
        {
            SelectCommand = "select * from ProjectAdmin.[User]";
            TableName = "[User]";
        }

        public bool GetList(DataSet dsEmail, string strConn)
        {
            return DbRetrieve("ProjectAdmin.GetUserEmail", dsEmail, null, TableName, strConn);
        }

        public bool GetEmailList(DataSet dsEmail, string q, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Search", q);

            return DbRetrieve("ProjectAdmin.GetUserEmailChar", dsEmail, dbParams, TableName, strConn);
        }

        #endregion
    }
}