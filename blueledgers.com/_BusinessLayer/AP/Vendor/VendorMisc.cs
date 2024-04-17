using System;
using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class VendorMisc : DbHandler
    {
        #region "Attributies"

        public int VendorCode { get; set; }
        public Guid FieldID { get; set; }
        public string Value { get; set; }

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public VendorMisc()
        {
            SelectCommand = "SELECT * FROM AP.VendorMisc";
            TableName = "VendorMisc";
        }

        /// <summary>
        ///     Get all of vendor Misc.
        /// </summary>
        /// <param name="dsVendorMisc"></param>
        /// <param name="vendorCode"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsVendorMisc, string vendorCode, string connectionString)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@VendorCode", vendorCode);

            return DbRetrieve("AP.GetVendorMiscListByVendorCode", dsVendorMisc, dbParam, TableName, connectionString);
        }

        /// <summary>
        ///     Get schema for vendor misc.
        /// </summary>
        /// <param name="dsVendorMisc"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsVendorMisc, string connectionString)
        {
            return DbRetrieveSchema("AP.GetVendorMiscList", dsVendorMisc, null, TableName, connectionString);
        }

        #endregion
    }
}