using System.Data;
using Blue.DAL;

namespace Blue.BL.Option.Admin.Interface
{
    public class VendorMapp : DbHandler
    {
        public VendorMapp()
        {
            SelectCommand = "SELECT * FROM [ADMIN].[VendorMapp]";
            TableName = "VendorMapp";
        }

        /// <summary>
        ///     Gets all active VendorMapp data related to specified login name.
        /// </summary>
        /// <param name="dsVendorMapp"></param>
        /// <param name="LoginName"></param>
        /// <param name="MsgError"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsVendorMapp, ref string MsgError, string connStr)
        {
            var result = DbRetrieve("dbo.VendorMapp_GetList", dsVendorMapp, null, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Get Max AccCode.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewID(string connStr)
        {
            var NewID = DbReadScalar("dbo.VendorMapp_GetNewID", null, connStr);

            // Return result
            return NewID;
        }

        /// <summary>
        ///     Get Lookup AccCode.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            // Create parameters
            return DbRead("dbo.VendorMapp_GetList", null, connStr);
        }


        /// <summary>
        ///     Get structure
        /// </summary>
        /// <param name="dsProduct"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsVendorMapp, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("dbo.VendorMapp_GetSchema", dsVendorMapp, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Commit user changed to database.
        /// </summary>
        /// <param name="dsPrefix"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsVendorMapp, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsVendorMapp, SelectCommand, TableName);
            return DbCommit(dbSaveSource, connStr);
        }
    }
}