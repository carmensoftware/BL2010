using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class VendorComment : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public VendorComment()
        {
            SelectCommand = "SELECT * FROM AP.VendorComment";
            TableName = "VendorComment";
        }


        /// <summary>
        /// </summary>
        /// <param name="dsVendorComment"></param>
        /// <param name="vendorCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetVendorCommentListByVendorCode(DataSet dsVendorComment, string vendorCode, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@VendorCode", vendorCode);

            // Get data
            var result = DbRetrieve("AP.GetVendorCommentListByVendorCode", dsVendorComment, dbParams, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        ///     Get schema for vendor comment.
        /// </summary>
        /// <param name="dsVendorComment"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsVendorComment, string connectionString)
        {
            return DbRetrieveSchema("AP.GetVendorCommentList", dsVendorComment, null, TableName, connectionString);
        }


        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsVendorComment"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsVendorComment, string connStr)
        {
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsVendorComment, SelectCommand, TableName);

            // Save to database
            var result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}