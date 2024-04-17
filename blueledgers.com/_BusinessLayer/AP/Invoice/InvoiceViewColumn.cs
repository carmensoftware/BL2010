using System.Data;
using System.Globalization;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.AP
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
            SelectCommand = "SELECT * FROM AP.InvoiceViewColumn";
            TableName = "InvoiceViewColumn";
        }

        /// <summary>
        ///     Get column list using invoiceViewID
        /// </summary>
        /// <param name="invoiceViewID"></param>
        /// <param name="invoiceViewColumn"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceViewColumnList(int invoiceViewID, DataSet invoiceViewColumn, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameters
            dbParams[0] = new DbParameter("@InvoiceViewID", invoiceViewID.ToString(CultureInfo.InvariantCulture));

            // Get Data
            var result = DbRetrieve("AP.GetInvoiceViewColumnListByInvoiceViewID", invoiceViewColumn, dbParams, TableName,
                connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Generate column list using invoiceViewID
        /// </summary>
        /// <param name="invoiceViewID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetInvoiceViewColumn(int invoiceViewID, string connStr)
        {
            var dbParams = new DbParameter[1];
            var columnList = string.Empty;
            var field = new Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Create parameters
            dbParams[0] = new DbParameter("@InvoiceViewID", invoiceViewID.ToString(CultureInfo.InvariantCulture));

            // Get Data
            var invoiceViewColumn = new DbHandler().DbRead("AP.GetInvoiceViewColumnListByInvoiceViewID", dbParams,
                connStr);

            // Generate Column List
            if (invoiceViewColumn != null)
            {
                foreach (DataRow dr in invoiceViewColumn.Rows)
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
        public static string GetInvoiceViewColumnPreview(DataTable dtPreview, string connStr)
        {
            var columnList = string.Empty;
            var field = new Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Get Data
            var standardvoucherViewColumn = dtPreview;

            // Generate Column List
            if (standardvoucherViewColumn != null)
            {
                foreach (DataRow dr in standardvoucherViewColumn.Rows)
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
        /// <param name="invoiceView"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceViewColumnSchema(DataSet invoiceView, string connStr)
        {
            // Get data
            var result = DbRetrieveSchema("AP.GetInvoiceViewColumnList", invoiceView, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}