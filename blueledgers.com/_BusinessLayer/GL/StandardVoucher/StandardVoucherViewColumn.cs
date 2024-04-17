using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StandardVoucherViewColumn : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public StandardVoucherViewColumn()
        {
            SelectCommand = "SELECT * FROM GL.StandardVoucherViewColumn";
            TableName = "StandardVoucherViewColumn";
        }

        /// <summary>
        ///     Get column list using standardVoucherViewID
        /// </summary>
        /// <param name="standardVoucherViewID"></param>
        /// <returns></returns>
        public bool GetStandardVoucherViewColumnList(int standardVoucherViewID, DataSet standardVoucherViewColumn,
            string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameters
            dbParams[0] = new DbParameter("@StandardVoucherViewID", standardVoucherViewID.ToString());

            // Get Data
            result = DbRetrieve("GL.GetStandardVoucherViewColumnListByStandardVoucherViewID", standardVoucherViewColumn,
                dbParams, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Generate column list using standardVoucherViewID
        /// </summary>
        /// <param name="standardVoucherViewID"></param>
        /// <returns></returns>
        public static string GetStandardVoucherViewColumn(int standardVoucherViewID, string connStr)
        {
            var standardVoucherViewColumn = new DataTable();
            var dbParams = new DbParameter[1];
            var columnList = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Create parameters
            dbParams[0] = new DbParameter("@StandardVoucherViewID", standardVoucherViewID.ToString());

            // Get Data
            standardVoucherViewColumn =
                new DbHandler().DbRead("GL.GetStandardVoucherViewColumnListByStandardVoucherViewID", dbParams, connStr);

            // Generate Column List
            if (standardVoucherViewColumn != null)
            {
                foreach (DataRow dr in standardVoucherViewColumn.Rows)
                {
                    columnList += (columnList != string.Empty ? "," : string.Empty) + "[" +
                                  field.GetFieldName(dr["FieldID"].ToString(), connStr) + "]" +
                                  " AS [" + dr["FieldID"] + "]";
                }
            }

            return columnList;
        }

        /// <summary>
        ///     Generate column list using datatable
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetStandardVoucherViewColumnPreview(DataTable dtPreview, string connStr)
        {
            var standardvoucherViewColumn = new DataTable();
            var columnList = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Get Data
            standardvoucherViewColumn = dtPreview;

            // Generate Column List
            if (standardvoucherViewColumn != null)
            {
                foreach (DataRow dr in standardvoucherViewColumn.Rows)
                {
                    columnList += (columnList != string.Empty ? "," : string.Empty) + "[" +
                                  field.GetFieldName(dr["FieldID"].ToString(), connStr) + "]" +
                                  " AS [" + dr["FieldID"] + "]";
                }
            }

            return columnList;
        }

        /// <summary>
        ///     Get standard voucher view column schema
        /// </summary>
        /// <param name="standardVoucherView"></param>
        /// <returns></returns>
        public bool GetStandardVoucherViewColumnSchema(DataSet standardVoucherView, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetStandardVoucherViewColumnList", standardVoucherView, null, TableName,
                connStr);

            // Return result
            return result;
        }

        #endregion
    }
}