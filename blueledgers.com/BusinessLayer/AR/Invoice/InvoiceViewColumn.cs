using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class InvoiceViewColumn : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public InvoiceViewColumn()
        {
            SelectCommand = "SELECT * FROM AR.InvoiceViewColumn";
            TableName = "InvoiceViewColumn";
        }

        /// <summary>
        ///     Get column list using invoiceViewID
        /// </summary>
        /// <param name="invoiceViewID"></param>
        /// <returns></returns>
        public bool GetInvoiceViewColumnList(int invoiceViewID, DataSet invoiceViewColumn, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameters
            dbParams[0] = new DbParameter("@InvoiceViewID", invoiceViewID.ToString());

            // Get Data
            result = DbRetrieve("AR.GetInvoiceViewColumnListByInvoiceViewID", invoiceViewColumn, dbParams, TableName,
                connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Generate column list using invoiceViewID
        /// </summary>
        /// <param name="invoiceViewID"></param>
        /// <returns></returns>
        public static string GetInvoiceViewColumn(int invoiceViewID, string connStr)
        {
            var invoiceViewColumn = new DataTable();
            var dbParams = new DbParameter[1];
            var columnList = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Create parameters
            dbParams[0] = new DbParameter("@InvoiceViewID", invoiceViewID.ToString());

            // Get Data
            invoiceViewColumn = new DbHandler().DbRead("AR.GetInvoiceViewColumnListByInvoiceViewID", dbParams, connStr);

            // Generate Column List
            if (invoiceViewColumn != null)
            {
                foreach (DataRow dr in invoiceViewColumn.Rows)
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
        public static string GetInvoiceViewColumnPreview(DataTable dtPreview, string connStr)
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
        ///     Get invoice view column schema
        /// </summary>
        /// <param name="invoiceView"></param>
        /// <returns></returns>
        public bool GetInvoiceViewColumnSchema(DataSet invoiceView, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("AR.GetInvoiceViewColumnList", invoiceView, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}