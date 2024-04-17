using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class VendorActiveLog : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public VendorActiveLog()
        {
            SelectCommand = "SELECT * FROM AP.VendorActLog";
            TableName = "VendorActLog";
        }


        /// <summary>
        /// </summary>
        /// <param name="dsVendorActLog"></param>
        /// <param name="vendorCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetVendorActLogListByVendorCode(DataSet dsVendorActLog, string vendorCode, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@VendorCode", vendorCode);

            // Get data
            var result = DbRetrieve("AP.GetVendorActLogListByVendorCode", dsVendorActLog, dbParams, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        ///     Get schema for vendor activelog.
        /// </summary>
        /// <param name="dsVendorActLog"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsVendorActLog, string connectionString)
        {
            return DbRetrieveSchema("AP.GetVendorActLogList", dsVendorActLog, null, TableName, connectionString);
        }


        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsVendorActLog"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsVendorActLog, string connStr)
        {
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsVendorActLog, SelectCommand, TableName);

            // Save to database
            var result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}