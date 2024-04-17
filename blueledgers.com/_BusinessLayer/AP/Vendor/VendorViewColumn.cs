using System.Data;
using System.Globalization;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class VendorViewColumn : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public VendorViewColumn()
        {
            SelectCommand = "SELECT * FROM AP.VendorViewColumn";
            TableName = "VendorViewColumn";
        }

        /// <summary>
        ///     Get column list using vendorViewID
        /// </summary>
        /// <param name="vendorViewID"></param>
        /// <param name="vendorViewColumn"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetVendorViewColumnList(int vendorViewID, DataSet vendorViewColumn, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameters
            dbParams[0] = new DbParameter("@VendorViewID", vendorViewID.ToString(CultureInfo.InvariantCulture));

            // Get Data
            var result = DbRetrieve("AP.GetVendorViewColumnListByVendorViewID", vendorViewColumn, dbParams, TableName,
                connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Generate column list using vendorViewID
        /// </summary>
        /// <param name="vendorViewID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetVendorViewColumn(int vendorViewID, string connStr)
        {
            var dbParams = new DbParameter[1];
            var columnList = string.Empty;
            var field = new Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Create parameters
            dbParams[0] = new DbParameter("@VendorViewID", vendorViewID.ToString(CultureInfo.InvariantCulture));

            // Get Data
            var vendorViewColumn = new DbHandler().DbRead("AP.GetVendorViewColumnListByVendorViewID", dbParams, connStr);

            // Generate Column List
            if (vendorViewColumn != null)
            {
                foreach (DataRow dr in vendorViewColumn.Rows)
                {
                    columnList += string.Format("{0}[{1}] AS [{2}]", 
                        (columnList != string.Empty ? "," : string.Empty), 
                        field.GetFieldName(dr["FieldID"].ToString(), connStr), dr["FieldID"]);
                }
            }

            return columnList;
        }

        /// <summary>
        ///     Generate column list using datatable
        /// </summary>
        /// <param name="dtPreview"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetVendorViewColumnPreview(DataTable dtPreview, string connStr)
        {
            var columnList = string.Empty;
            var field = new Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Get Data
            var vendorViewColumn = dtPreview;

            // Generate Column List
            if (vendorViewColumn != null)
            {
                foreach (DataRow dr in vendorViewColumn.Rows)
                {
                    columnList += string.Format("{0}[{1}] AS [{2}]", 
                        (columnList != string.Empty ? "," : string.Empty),
                        field.GetFieldName(dr["FieldID"].ToString(), connStr), dr["FieldID"]);
                }
            }

            return columnList;
        }


        /// <summary>
        ///     Get standard voucher view column schema
        /// </summary>
        /// <param name="vendorView"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetVendorViewColumnSchema(DataSet vendorView, string connStr)
        {
            // Get data
            var result = DbRetrieveSchema("AP.GetVendorViewColumnList", vendorView, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}