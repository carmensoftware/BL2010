using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation
{
    public class BusinessUnitAddress : DbHandler
    {
        #region "Attibuties"

        public string BusinessUnitAddressID { get; set; }

        public string BusinessUnitCode { get; set; }

        public string AddressTypeID { get; set; }

        public string Street { get; set; }

        public string RegionCode { get; set; }

        public string CountryCode { get; set; }

        public string ProvinceCode { get; set; }

        public string DistrictCode { get; set; }

        public string CityCode { get; set; }

        public string PostalCode { get; set; }

        public bool IsMailing { get; set; }

        #endregion

        #region "Operations"

        public BusinessUnitAddress()
        {
            SelectCommand = "SELECT * FROM Reference.BusinessUnitAddress";
            TableName = "BusinessUnitAddress";
        }

        /// <summary>
        ///     Get all data from table businessunit
        /// </summary>
        /// <param name="dsBusinessUnit"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataSet GetBusinessUnitAddress(DataSet dsBusinessUnitAddress, string businessCode, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BusinessUnitCode", businessCode);

            DbRetrieve("Reference.GetBusinessUnitAddress", dsBusinessUnitAddress, dbParams, TableName, connStr);
            return dsBusinessUnitAddress;
        }

        /// <summary>
        ///     Save data
        /// </summary>
        /// <param name="dsBusinessUnit"></param>
        /// <returns></returns>
        public bool Save(DataSet dsBusinessUnitAddress, string connStr)
        {
            var businessUnit = new BusinessUnit();
            var result = false;
            var dbSaveSource = new DbSaveSource[2];


            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsBusinessUnitAddress, businessUnit.SelectCommand, businessUnit.TableName);
            dbSaveSource[1] = new DbSaveSource(dsBusinessUnitAddress, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}