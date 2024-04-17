using System.Data;
using Blue.DAL;

namespace Blue.BL.dbo
{
    public class City : DbHandler
    {
        public City()
        {
            SelectCommand = "SELECT * FROM [dbo].City";
            TableName = "City";
        }

        /// <summary>
        ///     Get List of City filter by country.
        /// </summary>
        /// <param name="CountryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string CountryCode)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CountryCode", CountryCode);

            return DbRead("[dbo].[GetCityList]", dbParams);
        }
    }
}