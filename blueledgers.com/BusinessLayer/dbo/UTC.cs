using System.Data;
using Blue.DAL;

namespace Blue.BL.dbo
{
    public class UTC : DbHandler
    {
        public UTC()
        {
            SelectCommand = "SELECT * FROM [dbo].UTC";
            TableName = "UTC";
        }

        /// <summary>
        ///     Get Time Zone List
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList()
        {
            return DbRead("[dbo].[GetUTCList]", null);
        }
    }
}