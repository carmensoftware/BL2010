using System.Data;
using System.Globalization;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class InvoiceViewCriteria : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public InvoiceViewCriteria()
        {
            SelectCommand = "SELECT * FROM AP.InvoiceViewCriteria";
            TableName = "InvoiceViewCriteria";
        }

        /// <summary>
        ///     Get invoice  view criteria using invoice  view id
        /// </summary>
        /// <returns></returns>
        public DataTable GetInvoiceViewCriteriaList(int invoiceViewID, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@InvoiceViewID", invoiceViewID.ToString(CultureInfo.InvariantCulture));

            // Get data
            var invoiceViewCriteria = DbRead("AP.GetInvoiceViewCriteriaListByInvoiceViewID", dbParams, connStr);

            //Return result
            return invoiceViewCriteria;
        }

        /// <summary>
        ///     Generate where clause using invoiceViewID
        /// </summary>
        /// <param name="invoiceViewID"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetInvoiceViewCriteria(int invoiceViewID, int userID, string connStr)
        {
            var whereClause = string.Empty;
            var field = new Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            var invoiceView = new InvoiceView().GetInvoiceView(invoiceViewID, connStr);

            if (invoiceView.Rows.Count > 0)
            {
                // Get invoice  view criteria data
                var invoiceViewCriteria = new InvoiceViewCriteria().GetInvoiceViewCriteriaList(invoiceViewID, connStr);

                // Non-Advance option
                if (invoiceView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in invoiceViewCriteria.Rows)
                    {
                        whereClause += string.Format("{0}{1} {2} @{3}{1} {4}",
                            (whereClause != string.Empty ? " " : string.Empty),
                            field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Operator"], dr["SeqNo"], dr["LogicalOp"]);
                    }
                }
                    // Advance option
                else
                {
                    whereClause = invoiceView.Rows[0]["AvanceOption"].ToString();

                    foreach (DataRow dr in invoiceViewCriteria.Rows)
                    {
                        var eachWhereClause = string.Format("{0} {1} @{2}{0}",
                            field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Operator"], dr["SeqNo"]);
                        whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                    }
                }

                // Add display only own view
                if (!(bool) invoiceView.Rows[0]["IsAll"])
                {
                    whereClause += string.Format("{0} CreatedBy = {1}",
                        (whereClause != string.Empty ? " AND " : string.Empty), userID);
                }
            }

            // Return result
            return whereClause;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetInvoiceViewCriteriaPreview(DataSet dsPreview, int userID, string connStr)
        {
            var whereClause = string.Empty;

            var field = new Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            if (dsPreview.Tables["InvoiceView"] != null)
            {
                var invoiceView = dsPreview.Tables["InvoiceView"];

/*
                invoiceViewID = int.Parse(invoiceView.Rows[0]["InvoiceViewID"].ToString());
*/

                if (invoiceView != null)
                {
                    // Get invoice view criteria data

                    if (dsPreview.Tables["InvoiceViewCriteria"] != null)
                    {
                        var invoiceViewCriteria = dsPreview.Tables["InvoiceViewCriteria"];

                        // Non-Advance option

                        if (invoiceView.Rows[0]["IsAdvance"].ToString() != "True")
                        {
                            foreach (DataRow dr in invoiceViewCriteria.Rows)
                            {
                                whereClause += string.Format("{0}{1} {2} @{3}{1} {4}",
                                    (whereClause != string.Empty ? " " : string.Empty),
                                    field.GetFieldName(dr["FieldID"].ToString(), connStr),
                                    dr["Operator"], dr["SeqNo"], dr["LogicalOp"]);
                            }
                        }

                            // Advance option

                        else
                        {
                            whereClause = invoiceView.Rows[0]["AvanceOption"].ToString();

                            foreach (DataRow dr in invoiceViewCriteria.Rows)
                            {
                                var eachWhereClause = string.Format("{0} {1} @{2}{0}",
                                    field.GetFieldName(dr["FieldID"].ToString(), connStr),
                                    dr["Operator"], dr["SeqNo"]);

                                whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                            }
                        }

                        // Add display only own view

                        if (!(bool) invoiceView.Rows[0]["IsAll"])
                        {
                            whereClause += string.Format("{0} CreatedBy = {1}",
                                (whereClause != string.Empty ? " AND " : string.Empty), userID);
                        }
                    }
                }
            }

            // Return result

            return whereClause;
        }


        /// <summary>
        ///     Get invoice view criteria using invoice view id
        /// </summary>
        /// <returns></returns>
        public bool GetInvoiceViewCriteriaList(int invoiceViewID, DataSet invoiceViewCriteria, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@InvoiceViewID", invoiceViewID.ToString(CultureInfo.InvariantCulture));

            // Get data
            var result = DbRetrieve("AP.GetInvoiceViewCriteriaListByInvoiceViewID", invoiceViewCriteria, dbParams,
                TableName,
                connStr);

            //Return result
            return result;
        }

        /// <summary>
        ///     Get invoice  view criteria schema
        /// </summary>
        /// <param name="invoiceViewCriteria"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceViewCriteriaSchema(DataSet invoiceViewCriteria, string connStr)
        {
            // Get data
            var result = DbRetrieveSchema("AP.GetInvoiceViewCriteriaList", invoiceViewCriteria, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}