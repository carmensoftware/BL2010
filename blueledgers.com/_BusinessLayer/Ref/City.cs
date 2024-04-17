using System.Data;
using Blue.DAL;

namespace Blue.BL.Ref
{
    public class City : DbHandler
    {
        /// <summary>
        ///     Empty constructure
        /// </summary>
        public City()
        {
            SelectCommand = "SELECT * FROM Ref.City";
            TableName = "City";
        }

        public void GetCityList(DataSet dsCity, string connStr)
        {
            DbRetrieve("Ref.GetCityList", dsCity, null, TableName, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetCityByCountryForDDL(string country, string connStr)
        {
            var dtCity = new DataTable();

            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@country", country);
            // Get data
            dtCity = DbRead("Ref.GetCityListByCountry", dbParams, connStr);


            // Return result
            if (dtCity != null)
            {
                var drBlank = dtCity.NewRow();
                dtCity.Rows.InsertAt(drBlank, 0);
            }

            return dtCity;
        }
    }
}