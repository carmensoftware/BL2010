using System.Data;
using Blue.DAL;

namespace Blue.BL.Ref
{
    public class Country : DbHandler
    {
        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Country()
        {
            SelectCommand = "SELECT * FROM Ref.Country";
            TableName = "Country";
        }

        /// <summary>
        ///     Get all data
        /// </summary>
        /// <returns></returns>
        public void GetCountryList(DataSet dsCountry, string connStr)
        {
            DbRetrieve("Ref.GetCountryList", dsCountry, null, TableName, connStr);
        }

        /// <summary>
        ///     Get all data in table
        /// </summary>
        /// <returns></returns>
        public DataTable GetCountryList(string connStr)
        {
            var dtCountry = new DataTable();
            dtCountry = DbRead("Ref.GetCountryList", null, connStr);

            if (dtCountry != null)
            {
                var drBlank = dtCountry.NewRow();
                dtCountry.Rows.InsertAt(drBlank, 0);
            }
            return dtCountry;
        }
    }
}