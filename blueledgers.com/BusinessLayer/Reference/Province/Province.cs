using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class Province : DbHandler
    {
        #region " Attributies "

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public Province()
        {
            SelectCommand = "SELECT * FROM Reference.Province";
            TableName = "Province";
        }

        /// <summary>
        ///     Get name province
        /// </summary>
        /// <param name="provinceCode"></param>
        /// <returns></returns>
        public string GetProvinceNameByProvinceCode(string provinceCode, string connStr)
        {
            var result = string.Empty;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@provinceCode", provinceCode);

            DbRetrieve("Reference.GetProvince", dsTmp, dbParams, TableName, connStr);

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
        public void GetProvinceList(DataSet dsProvince, string connStr)
        {
            DbRetrieve("Reference.GetProvinceList", dsProvince, null, TableName, connStr);
        }

        /// <summary>
        ///     Get province by country code.
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="dsCountry"></param>
        /// <returns></returns>
        public DataSet GetProvinceListByCountry(string countryCode, DataSet dsProvince, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@countryCode", countryCode);

            DbRetrieve("Reference.GetProvinceListByCountry", dsProvince, dbParams, TableName, connStr);
            return dsProvince;
        }

        /// <summary>
        ///     Get province by country code.
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetProvinceListByCountry(string countryCode, string connStr)
        {
            var dtProvince = new DataTable();

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@countryCode", countryCode);


            // Get data
            dtProvince = DbRead("Reference.GetProvinceListByCountry", dbParams, connStr);


            // Return result
            if (dtProvince != null)
            {
                var drBlank = dtProvince.NewRow();
                dtProvince.Rows.InsertAt(drBlank, 0);
            }

            return dtProvince;
        }

        /// <summary>
        ///     Get max province Code.
        /// </summary>
        /// <returns></returns>
        public int GetProvinceCodeMax(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("Reference.GetProvinceCodeMax", null, connStr);

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
        public bool Save(DataSet dsProvince, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsProvince, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}