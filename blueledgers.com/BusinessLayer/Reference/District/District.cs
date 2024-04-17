using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class District : DbHandler
    {
        /// <summary>
        ///     Empty constructure
        /// </summary>
        public District()
        {
            SelectCommand = "SELECT * FROM Reference.District";
            TableName = "District";
        }

        /// <summary>
        ///     Get category using category code
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <param name="categoryCode"></param>
        /// <returns></returns>
        public bool GetDistrict(DataSet dsDistrict, string provinceCode, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProvinceCode", provinceCode);

            // Get data
            result = DbRetrieve("Reference.GetDistrict", dsDistrict, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all data
        /// </summary>
        /// <returns></returns>
        public void GetProvinceList(DataSet dsDistrict, string connStr)
        {
            DbRetrieve("Reference.GetDistrictList", dsDistrict, null, TableName, connStr);
        }

        /// <summary>
        ///     Get district by district code.
        /// </summary>
        /// <param name="dsDistrict"></param>
        /// <param name="districtCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetDistrictByDistrictCode(DataSet dsDistrict, string districtCode, string connStr)
        {
            var result = false;

            // Create parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DistrictCode", districtCode);

            // Get data
            result = DbRetrieve("Reference.GetDistrictByDistrictCode", dsDistrict, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get district name by district code.
        /// </summary>
        /// <param name="districtCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetDistrictName(string districtCode, string connStr)
        {
            var result = string.Empty;
            var dsDistrict = new DataSet();

            // Get data
            GetDistrictByDistrictCode(dsDistrict, districtCode, connStr);

            // Return result
            if (dsDistrict.Tables[TableName] != null)
            {
                if (dsDistrict.Tables[TableName].Rows.Count > 0)
                {
                    result = dsDistrict.Tables[TableName].Rows[0]["Name"].ToString();
                }
            }

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="dsCountry"></param>
        /// <returns></returns>
        public DataSet GetDistrictListByProvince(string provinceCode, DataSet dsDistrict, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProvinceCode", provinceCode);

            DbRetrieve("Reference.GetDistrictListByProvince", dsDistrict, dbParams, TableName, connStr);
            return dsDistrict;
        }

        /// <summary>
        ///     Get district list by province code.
        /// </summary>
        /// <param name="provinceCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetDistrictListByProvince(string provinceCode, string connStr)
        {
            var dtDistrict = new DataTable();

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProvinceCode", provinceCode);


            // Get data
            dtDistrict = DbRead("Reference.GetDistrictListByProvince", dbParams, connStr);


            // Return result
            if (dtDistrict != null)
            {
                var drBlank = dtDistrict.NewRow();
                dtDistrict.Rows.InsertAt(drBlank, 0);
            }

            return dtDistrict;
        }

        /// <summary>
        ///     Get max disctrict Code.
        /// </summary>
        /// <returns></returns>
        public int GetDistrictCodeMax(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("Reference.GetDistrictCodeMax", null, connStr);

            // If return null value should be one.
            if (result == Convert.ToInt32("0"))
            {
                result = 1;
            }

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsDistrict"></param>
        /// <returns></returns>
        public bool Save(DataSet dsDistrict, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsDistrict, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }
    }
}