using System.Data;
using Blue.DAL;

namespace Blue.BL.ProjectAdmin
{
    public class User : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        public User()
        {
            SelectCommand = "select * from ProjectAdmin.[User]";
            TableName = "[User]";
        }

        public bool GetList(DataSet dsList, string strConn)
        {
            return DbRetrieve("ProjectAdmin.GetList", dsList, null, TableName, strConn);
        }

        #endregion
    }
}