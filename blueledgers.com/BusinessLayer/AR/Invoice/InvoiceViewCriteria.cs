using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
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
            SelectCommand = "SELECT * FROM AR.InvoiceViewCriteria";
            TableName = "InvoiceViewCriteria";
        }

        /// <summary>
        ///     Get invoice view criteria using invoice view id
        /// </summary>
        /// <returns></returns>
        public DataTable GetInvoiceViewCriteriaList(int invoiceViewID, string connStr)
        {
            var invoiceViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@InvoiceViewID", invoiceViewID.ToString());

            // Get data
            invoiceViewCriteria = DbRead("AR.GetInvoiceViewCriteriaListByInvoiceViewID", dbParams, connStr);

            //Return result
            return invoiceViewCriteria;
        }

        /// <summary>
        ///     Generate where clause using invoiceViewID
        /// </summary>
        /// <param name="invoiceViewID"></param>
        /// <returns></returns>
        public static string GetInvoiceViewCriteria(int invoiceViewID, int userID, string connStr)
        {
            var invoiceView = new DataTable();
            var invoiceViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];
            var whereClause = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            invoiceView = new InvoiceView().GetInvoiceView(invoiceViewID, connStr);

            if (invoiceView != null)
            {
                // Get invoice  view criteria data
                invoiceViewCriteria = new InvoiceViewCriteria().GetInvoiceViewCriteriaList(invoiceViewID, connStr);

                // Non-Advance option
                if (invoiceView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in invoiceViewCriteria.Rows)
                    {
                        whereClause += (whereClause != string.Empty ? " " : string.Empty) +
                                       field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " + dr["Operator"] +
                                       " " +
                                       "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                       dr["LogicalOp"];
                    }
                }
                    // Advance option
                else
                {
                    whereClause = invoiceView.Rows[0]["AvanceOption"].ToString();

                    foreach (DataRow dr in invoiceViewCriteria.Rows)
                    {
                        var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                              dr["Operator"] + " " + "@" + dr["SeqNo"] +
                                              field.GetFieldName(dr["FieldID"].ToString(), connStr);
                        whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                    }
                }

                // Add display only own view
                if (!(bool) invoiceView.Rows[0]["IsAll"])
                {
                    whereClause += (whereClause != string.Empty ? " AND " : string.Empty) + " CreatedBy = " + userID;
                }
            }

            // Return result
            return whereClause;
        }

        /// <summary>
        ///     Get invoice view criteria.
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetInvoiceViewCriteriaPreview(DataSet dsPreview, int userID, string connStr)
        {
            var invoiceView = new DataTable();
            var invoiceViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            var whereClause = string.Empty;

            int invoiceViewID;

            // Checking advance option
            if (dsPreview.Tables["InvoiceView"] != null)
            {
                invoiceView = dsPreview.Tables["InvoiceView"];
                invoiceViewID = int.Parse(invoiceView.Rows[0]["InvoiceViewID"].ToString());

                if (invoiceView != null)
                {
                    // Get invoice view criteria data

                    if (dsPreview.Tables["InvoiceViewCriteria"] != null)
                    {
                        invoiceViewCriteria = dsPreview.Tables["InvoiceViewCriteria"];

                        // Non-Advance option
                        if (invoiceView.Rows[0]["IsAdvance"].ToString() != "True")
                        {
                            foreach (DataRow dr in invoiceViewCriteria.Rows)
                            {
                                whereClause += (whereClause != string.Empty ? " " : string.Empty) +
                                               field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                               dr["Operator"] + " " +
                                               "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr) +
                                               " " + dr["LogicalOp"];
                            }
                        }
                            // Advance option
                        else
                        {
                            whereClause = invoiceView.Rows[0]["AvanceOption"].ToString();

                            foreach (DataRow dr in invoiceViewCriteria.Rows)
                            {
                                var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                                      dr["Operator"]
                                                      + " " + "@" + dr["SeqNo"] +
                                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);
                                whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                            }
                        }
                        // Add display only own view
                        if (!(bool) invoiceView.Rows[0]["IsAll"])
                        {
                            whereClause += (whereClause != string.Empty ? " AND " : string.Empty) + " CreatedBy = " +
                                           userID;
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
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@InvoiceViewID", invoiceViewID.ToString());

            // Get data
            result = DbRetrieve("AR.GetInvoiceViewCriteriaListByInvoiceViewID", invoiceViewCriteria, dbParams, TableName,
                connStr);

            //Return result
            return result;
        }

        /// <summary>
        ///     Get invoice  view criteria schema
        /// </summary>
        /// <param name="invoiceViewCriteria"></param>
        /// <returns></returns>
        public bool GetInvoiceViewCriteriaSchema(DataSet invoiceViewCriteria, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("AR.GetInvoiceViewCriteriaList", invoiceViewCriteria, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}