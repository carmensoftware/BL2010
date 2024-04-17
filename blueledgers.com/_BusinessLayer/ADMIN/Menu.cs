using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.ADMIN
{
    public class Menu : DbHandler
    {
        public Menu()
        {
            SelectCommand = "SELECT * FROM [ADMIN].[Menu]";
            TableName = "Menu";
        }

        public DataTable GetList(string loginName, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LoginName", loginName);

            return DbRead("[ADMIN].[GetMenuList_LoginName]", dbParams, connStr);
        }
    }
}