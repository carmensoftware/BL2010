using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class Country : DbHandler
    {
        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Country()
        {
            SelectCommand = "SELECT * FROM Reference.Country";
            TableName = "Country";
        }

        /// <summary>
        ///     Get all data
        /// </summary>
        /// <returns></returns>
        public void GetCountryList(DataSet dsCountry, string connStr)
        {
            DbRetrieve("Reference.GetCountryList", dsCountry, null, TableName, connStr);
        }

        /// <summary>
        ///     Get all data in table
        /// </summary>
        /// <returns></returns>
        public DataTable GetCountryList(string connStr)
        {
            var dtCountry = new DataTable();
            dtCountry = DbRead("Reference.GetCountryList", null, connStr);

            if (dtCountry != null)
            {
                var drBlank = dtCountry.NewRow();
                dtCountry.Rows.InsertAt(drBlank, 0);
            }
            return dtCountry;
        }

        /// <summary>
        ///     Retrieve country by region code
        /// </summary>
        /// <param name="regionCode"></param>
        /// <param name="dsCountry"></param>
        /// <returns></returns>
        public DataSet GetCountryListByRegion(string regionCode, DataSet dsCountry, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RegionCode", regionCode);

            DbRetrieve("Reference.GetCountry", dsCountry, dbParams, TableName, connStr);
            return dsCountry;
        }

        /// <summary>
        ///     Get country by region code
        /// </summary>
        /// <param name="regionCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetCountryListByRegion(string regionCode, string connStr)
        {
            var dtCountry = new DataTable();

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RegionCode", regionCode);

            // Get data
            dtCountry = DbRead("Reference.GetCountry", dbParams, connStr);

            // Return result
            if (dtCountry != null)
            {
                var drBlank = dtCountry.NewRow();
                dtCountry.Rows.InsertAt(drBlank, 0);
            }

            return dtCountry;
        }

        /// <summary>
        ///     Get country by country code.
        /// </summary>
        /// <param name="dsCountry"></param>
        /// <param name="countryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetCountry(DataSet dsCountry, string countryCode, string connStr)
        {
            var result = false;

            // Create parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CountryCode", countryCode);

            // Get data
            result = DbRetrieve("Reference.GetCountryByCountryCode", dsCountry, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetCountryName(string countryCode, string connStr)
        {
            var result = string.Empty;
            var dsCountry = new DataSet();

            // Get data
            GetCountry(dsCountry, countryCode, connStr);

            // Return result
            if (dsCountry.Tables[TableName] != null)
            {
                if (dsCountry.Tables[TableName].Rows.Count > 0)
                {
                    result = dsCountry.Tables[TableName].Rows[0]["Name"].ToString();
                }
            }

            return result;
        }

        /// <summary>
        ///     Get max region Code.
        /// </summary>
        /// <returns></returns>
        public int GetCountryCodeMax(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("Reference.GetCountryCodeMax", null, connStr);

            if (result == Convert.ToInt32("0"))
            {
                result = 1;
            }
            // Return result
            return result;
        }

        public string GetRegionCodeByCountryCode(string countryCode, string connStr)
        {
            var result = string.Empty;

            var dsCountry = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CountryCode", countryCode);

            //Get Data
            DbRetrieve("Reference.GetCountryByCountryCode", dsCountry, dbParams, TableName, connStr);

            if (dsCountry.Tables[TableName].Rows.Count > 0)
            {
                result = dsCountry.Tables[TableName].Rows[0]["RegionCode"].ToString();
            }

            return result;
        }

        /// <summary>
        ///     Save data
        /// </summary>
        /// <param name="dsCountry"></param>
        /// <returns></returns>
        public bool Save(DataSet dsCountry, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsCountry, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }
    }
}