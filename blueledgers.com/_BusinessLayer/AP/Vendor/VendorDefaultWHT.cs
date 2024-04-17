using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class VendorDefaultWHT : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public VendorDefaultWHT()
        {
            SelectCommand = "SELECT * FROM AP.VendorDefaultWHT";
            TableName = "VendorDefaultWHT";
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="dsVendorDefaultWHT"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetVendorDefaultWHTList(string profileCode, DataSet dsVendorDefaultWHT, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            var result = DbRetrieve("AP.GetVendorDefaultWHTListByProfileCode", dsVendorDefaultWHT, dbParams, TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetVendorDefaultWHTListByProfileCode(string profileCode, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            var dtAP = DbRead("AP.GetVendorDefaultWHTListByProfileCode", dbParams, connStr);

            // Return result
            return dtAP;
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetVendorDefaultWHTList(string profileCode, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);


            DbRetrieve("AP.GetVendorDefaultWHTListByProfileCode", dsTmp, dbParams, TableName, connStr);

            return dsTmp;
        }


        /// <summary>
        ///     Get structure
        /// </summary>
        /// <param name="dsVendorDefaultWHT"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetVendorDefaultWHTStructure(DataSet dsVendorDefaultWHT, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetVendorDefaultWHTList", dsVendorDefaultWHT, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     GetVendorDefaultWHTMaxSeqNo
        /// </summary>
        /// <returns></returns>
        public int GetVendorDefaultWHTMaxSeqNo(string connStr)
        {
            // Get data
            var result = DbReadScalar("AP.GetVendorDefaultWHTMaxSeqNo", null, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}