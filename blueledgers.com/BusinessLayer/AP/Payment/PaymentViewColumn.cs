using System.Data;
using System.Globalization;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentViewColumn : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public PaymentViewColumn()
        {
            SelectCommand = "SELECT * FROM AP.PaymentViewColumn";
            TableName = "PaymentViewColumn";
        }

        /// <summary>
        ///     Get column list using paymentViewID
        /// </summary>
        /// <param name="paymentViewID"></param>
        /// <param name="paymentViewColumn"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentViewColumnList(int paymentViewID, DataSet paymentViewColumn, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameters
            dbParams[0] = new DbParameter("@PaymentViewID", paymentViewID.ToString(CultureInfo.InvariantCulture));

            // Get Data
            var result = DbRetrieve("AP.GetPaymentViewColumnListByPaymentViewID", paymentViewColumn, dbParams, TableName,
                connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Generate column list using paymentViewID
        /// </summary>
        /// <param name="paymentViewID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetPaymentViewColumn(int paymentViewID, string connStr)
        {
            var dbParams = new DbParameter[1];
            var columnList = string.Empty;

            var field = new Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Create parameters
            dbParams[0] = new DbParameter("@PaymentViewID", paymentViewID.ToString(CultureInfo.InvariantCulture));

            // Get Data
            var paymentViewColumn = new DbHandler().DbRead("AP.GetPaymentViewColumnListByPaymentViewID", dbParams,
                connStr);

            // Generate Column List
            if (paymentViewColumn != null)
            {
                foreach (DataRow dr in paymentViewColumn.Rows)
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
        /// <param name="dtPreview"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetPaymentViewColumnPreview(DataTable dtPreview, string connStr)
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
        /// <param name="paymentView"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentViewColumnSchema(DataSet paymentView, string connStr)
        {
            // Get data
            var result = DbRetrieveSchema("AP.GetPaymentViewColumnList", paymentView, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}