using System.Data;
using Blue.DAL;

namespace Blue.BL.dbo
{
    public class Country : DbHandler
    {
        public Country()
        {
            SelectCommand = "SELECT * FROM [dbo].Country";
            TableName = "Country";
        }

        /// <summary>
        ///     Get Country List.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList()
        {
            return DbRead("[dbo].[GetCountryList]", null);
        }
    }
}