using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ReceiptViewColumn : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public ReceiptViewColumn()
        {
            SelectCommand = "SELECT * FROM AR.ReceiptViewColumn";
            TableName = "ReceiptViewColumn";
        }

        /// <summary>
        ///     Get column list using ReceiptViewID
        /// </summary>
        /// <param name="ReceiptViewID"></param>
        /// <returns></returns>
        public bool GetReceiptViewColumnList(int receiptViewID, DataSet dsReceiptViewColumn, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameters
            dbParams[0] = new DbParameter("@ReceiptViewID", receiptViewID.ToString());

            // Get Data
            result = DbRetrieve("AR.GetReceiptViewColumnListByReceiptViewID", dsReceiptViewColumn, dbParams, TableName,
                connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Generate column list using ReceiptViewID
        /// </summary>
        /// <param name="ReceiptViewID"></param>
        /// <returns></returns>
        public static string GetReceiptViewColumn(int receiptViewID, string connStr)
        {
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            var dtReceiptViewColumn = new DataTable();
            var dbParams = new DbParameter[1];
            var columnList = string.Empty;

            // Create parameters
            dbParams[0] = new DbParameter("@ReceiptViewID", receiptViewID.ToString());

            // Get Data
            dtReceiptViewColumn = new DbHandler().DbRead("AR.GetReceiptViewColumnListByReceiptViewID", dbParams, connStr);

            // Generate Column List
            if (dtReceiptViewColumn != null)
            {
                foreach (DataRow dr in dtReceiptViewColumn.Rows)
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
        public static string GetReceiptViewColumnPreview(DataTable dtPreview, string connStr)
        {
            var dtReceiptViewColumn = new DataTable();
            var columnList = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Get Data
            dtReceiptViewColumn = dtPreview;

            // Generate Column List
            if (dtReceiptViewColumn != null)
            {
                foreach (DataRow dr in dtReceiptViewColumn.Rows)
                {
                    columnList += (columnList != string.Empty ? "," : string.Empty) + "[" +
                                  field.GetFieldName(dr["FieldID"].ToString(), connStr) + "]" +
                                  " AS [" + dr["FieldID"] + "]";
                }
            }

            return columnList;
        }

        /// <summary>
        ///     Get Receipt view column schema
        /// </summary>
        /// <param name="ReceiptView"></param>
        /// <returns></returns>
        public bool GetReceiptViewColumnSchema(DataSet dsReceiptView, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("AR.GetReceiptViewColumnList", dsReceiptView, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}