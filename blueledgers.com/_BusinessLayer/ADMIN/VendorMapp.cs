using System.Configuration;
using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.ADMIN
{
    public class VendorMapp : DbHandler
    {
        public VendorMapp()
        {
            SelectCommand = "SELECT * FROM [ADMIN].[VendorMapp]";
            TableName = "VendorMapp";
        }

        /// <summary>
        ///     Get all vendor mapping by Business Unit Group Code.
        /// </summary>
        /// <param name="dsVendorMapp"></param>
        /// <param name="buGrpCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsVendorMapp, string buGrpCode, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@BuGrpCode", buGrpCode);
            dbParams[1] = new DbParameter("@SysDbName", ConfigurationManager.AppSettings["SysDb"]);

            return DbRetrieve("dbo.ADMIN_VendorMapp_GetList_BuGrpCode", dsVendorMapp, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get all vendor mapping by Business Unit Group Code.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            return DbRead("[ADMIN].GetVendorMappingList", null, connStr);
        }

        /// <summary>
        ///     Get all vendor mapping data.
        /// </summary>
        /// <param name="dsVendorMapp"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsVendorMapp, string connStr)
        {
            return DbRetrieve("dbo.ADMIN_VendorMapp_GetList", dsVendorMapp, null, TableName, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="hqVendorCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetLocalVendorCode(string hqVendorCode, string connStr)
        {
            var vendorCode = string.Empty;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@VendorCode", hqVendorCode);
            //dbParams[1] = new DbParameter("@BuCode", BuCode);

            var dtGet = DbRead("[ADMIN].[GetLocalVendorCode]", dbParams, connStr);

            if (dtGet.Rows.Count > 0)
            {
                vendorCode = dtGet.Rows[0]["LocalCode"].ToString();
            }
            return vendorCode;
        }

        /// <summary>
        ///     Save Vendor Mapping to database.
        /// </summary>
        /// <param name="dsVendorMapp"></param>
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