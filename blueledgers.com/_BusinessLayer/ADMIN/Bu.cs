using System;
using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.ADMIN
{
    public class Bu : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        public Bu()
        {
            SelectCommand = "SELECT * FROM [ADMIN].[Bu]";
            TableName = "Bu";
        }

        public bool GetBuList(DataSet dsBu, ref string msgError, string connStr)
        {
            //Get Data
            return DbRetrieve("Admin.GetBuList", dsBu, null, TableName, connStr);
        }

        #endregion

        public void GetListByBuCode()
        {
            throw new NotImplementedException();
        }
    }
}