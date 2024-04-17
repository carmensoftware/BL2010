using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.Setup.Admin
{
    public class BusinessUnit : DbHandler
    {
        /// <summary>
        ///     Empty Constructure
        /// </summary>
        public BusinessUnit()
        {
            SelectCommand = "SELECT * FROM Admin.BusinessUnit";
            TableName = "BusinessUnit";
        }

        /// <summary>
        ///     Get all BusinessUnit data.
        /// </summary>
        /// <param name="dsBusinessUnit"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(ref DataSet dsBusinessUnit, string connStr)
        {
            // Get data
            var result = DbRetrieve("Admin.GetBusinessUnitList", dsBusinessUnit, null, TableName);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get max nearest period data from assigned year
        /// </summary>
        /// <returns></returns>
        public DataTable GetListTrans(string connStr, string buID)
        {
            var dtPeriod = new DataTable();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@id", buID);

            // Retrieve data
            dtPeriod = DbRead("Admin.GetBusinessUnitListByID", dbParams, connStr);

            // Return result
            return dtPeriod;
        }

        /// <summary>
        ///     Get max nearest period data from assigned year
        /// </summary>
        /// <returns></returns>
        public DataTable GetListByBuID(string connStr, string buID)
        {
            var dtPeriod = new DataTable();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@id", buID);

            // Retrieve data
            dtPeriod = DbRead("Admin.GetBusinessUnitListByBuID", dbParams, connStr);

            // Return result
            return dtPeriod;
        }

        /// <summary>
        ///     Get all active or inactive BusinessUnit data.
        /// </summary>
        /// <param name="dsBusinessUnit"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(ref DataSet dsBusinessUnit, bool isActive, string connStr)
        {
            var dbParams = new DbParameter[1];

            dbParams[0] = new DbParameter("@IsActive", isActive.ToString());

            // Get data
            var result = DbRetrieve("[Admin].GetBusinessUnitListIsActive", dsBusinessUnit, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Commit changed to database.
        /// </summary>
        /// <param name="dsRegion"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsBusinessUnit, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsBusinessUnit, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }
    }
}