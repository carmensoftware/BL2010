using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class City : DbHandler
    {
        /// <summary>
        ///     Empty constructure
        /// </summary>
        public City()
        {
            SelectCommand = "SELECT * FROM Reference.City";
            TableName = "City";
        }

        /// <summary>
        ///     Get name city
        /// </summary>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        public string GetName55(string cityCode, string connStr)
        {
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@cityCode", cityCode);

            DbRetrieve("Reference.GetCity", dsTmp, dbParams, TableName, connStr);

            return dsTmp.Tables[TableName].Rows[0]["Name"].ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="cityCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetCityNameByCityCode(string cityCode, string connStr)
        {
            var result = string.Empty;

            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@cityCode", cityCode);

            DbRetrieve("Reference.GetCity", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName] != null)
            {
                if (dsTmp.Tables[TableName].Rows.Count > 0)
                {
                    result = dsTmp.Tables[TableName].Rows[0]["Name"].ToString();
                }
            }

            return result;
        }

        /// <summary>
        ///     Get all data
        /// </summary>
        /// <returns></returns>
        public void GetCityList(DataSet dsCity, string connStr)
        {
            DbRetrieve("Reference.GetCityList", dsCity, null, TableName, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetCityForDropDownList(string connStr)
        {
            var dsCity = new DataSet();

            // Get data
            GetCityList(dsCity, connStr);

            // Return result
            if (dsCity.Tables[TableName] != null)
            {
                var drBlank = dsCity.Tables[TableName].NewRow();
                dsCity.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsCity.Tables[TableName];
        }

        /// <summary>
        ///     get city by district code.
        /// </summary>
        /// <param name="districtCode"></param>
        /// <param name="dsCity"></param>
        /// <returns></returns>
        public DataSet GetCityListByDistrict(string districtCode, DataSet dsCity, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@districtCode", districtCode);

            DbRetrieve("Reference.GetCityListByDistrict", dsCity, dbParams, TableName, connStr);

            if (dsCity.Tables[TableName] != null)
            {
                var drBlank = dsCity.Tables[TableName].NewRow();
                dsCity.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsCity;
        }


        /// <summary>
        /// </summary>
        /// <param name="districtCode"></param>
        /// <param name="dsCity"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetCityListByDistrictGrid(string districtCode, DataSet dsCity, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@districtCode", districtCode);

            DbRetrieve("Reference.GetCityListByDistrict", dsCity, dbParams, TableName, connStr);

            return dsCity;
        }

        /// <summary>
        ///     Get districtlist by province.
        /// </summary>
        /// <param name="provinceCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetDistrictListByProvince(string districtCode, string connStr)
        {
            var dtCity = new DataTable();

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@districtCode", districtCode);

            // Get data
            dtCity = DbRead("Reference.GetCityListByDistrict", dbParams, connStr);

            // Return result
            if (dtCity != null)
            {
                var drBlank = dtCity.NewRow();
                dtCity.Rows.InsertAt(drBlank, 0);
            }

            return dtCity;
        }

        /// <summary>
        ///     Get max city Code.
        /// </summary>
        /// <returns></returns>
        public int GetCityCodeMax(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("Reference.GetCityCodeMax", null, connStr);

            // If return null value should be one.
            if (result == Convert.ToInt32("0"))
            {
                result = 1;
            }
            // Return result
            return result;
        }

        /// <summary>
        ///     save changes to database.
        /// </summary>
        /// <param name="dsProvince"></param>
        /// <returns></returns>
        public bool Save(DataSet dsCity, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsCity, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }
    }
}