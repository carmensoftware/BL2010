using System.Data;
using Blue.DAL;

namespace Blue.BL.dbo
{
    public class Lang : DbHandler
    {
        public Lang()
        {
            SelectCommand = "SELECT * FROM dbo.Lang";
            TableName = "Lang";
        }

        /// <summary>
        ///     Get active language data related to specified business unit code.
        /// </summary>
        /// <param name="BuCode"></param>
        /// <param name="MsgError"></param>
        /// <returns></returns>
        public DataTable GetList(string BuCode)
        {
            // Create parameters.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BuCode", BuCode);

            return DbRead("dbo.Lang_GetActiveList_BuCode", dbParams);
        }

        /// <summary>
        ///     Get active language list
        /// </summary>
        /// <returns></returns>
        public bool GetList(DataSet dsLanguage)
        {
            return DbRetrieve("dbo.GetLanguageListActive", dsLanguage, null, TableName);
        }

        /// <summary>
        ///     Get active language list
        /// </summary>
        /// <returns></returns>
        public DataTable GetList()
        {
            return DbRead("dbo.GetLanguageListActive", null);
        }
    }
}