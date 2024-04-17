using System.Data;
using System.Globalization;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentViewCriteria : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentViewCriteria()
        {
            SelectCommand = "SELECT * FROM AP.PaymentViewCriteria";
            TableName = "PaymentViewCriteria";
        }

        /// <summary>
        ///     Get payment  view criteria using payment  view id
        /// </summary>
        /// <returns></returns>
        public DataTable GetPaymentViewCriteriaList(int paymentViewID, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@PaymentViewID", paymentViewID.ToString(CultureInfo.InvariantCulture));

            // Get data
            var paymentViewCriteria = DbRead("AP.GetPaymentViewCriteriaListByPaymentViewID", dbParams, connStr);

            //Return result
            return paymentViewCriteria;
        }

        /// <summary>
        ///     Generate where clause using paymentViewID
        /// </summary>
        /// <param name="paymentViewID"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetPaymentViewCriteria(int paymentViewID, int userID, string connStr)
        {
            var whereClause = string.Empty;
            var field = new Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            var paymentView = new PaymentView().GetPaymentView(paymentViewID, connStr);

            if (paymentView.Rows.Count > 0)
            {
                // Get payment  view criteria data
                var paymentViewCriteria = new PaymentViewCriteria().GetPaymentViewCriteriaList(paymentViewID, connStr);

                // Non-Advance option
                if (paymentView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in paymentViewCriteria.Rows)
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
                    whereClause = paymentView.Rows[0]["AvanceOption"].ToString();

                    foreach (DataRow dr in paymentViewCriteria.Rows)
                    {
                        var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                              dr["Operator"] + " " + "@" + dr["SeqNo"] +
                                              field.GetFieldName(dr["FieldID"].ToString(), connStr);
                        whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                    }
                }

                // Add display only own view
                if (!(bool) paymentView.Rows[0]["IsAll"])
                {
                    whereClause += (whereClause != string.Empty ? " AND " : string.Empty) + " CreatedBy = " + userID;
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
        public static string GetPaymentViewCriteriaPreview(DataSet dsPreview, int userID, string connStr)
        {
            var whereClause = string.Empty;

            var field = new Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            if (dsPreview.Tables["PaymentView"] != null)
            {
                var paymentView = dsPreview.Tables["PaymentView"];

                if (paymentView != null)
                {
                    // Get payment view criteria data

                    if (dsPreview.Tables["PaymentViewCriteria"] != null)
                    {
                        var paymentViewCriteria = dsPreview.Tables["PaymentViewCriteria"];

                        // Non-Advance option

                        if (paymentView.Rows[0]["IsAdvance"].ToString() != "True")
                        {
                            foreach (DataRow dr in paymentViewCriteria.Rows)
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
                            whereClause = paymentView.Rows[0]["AvanceOption"].ToString();

                            foreach (DataRow dr in paymentViewCriteria.Rows)
                            {
                                var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                                      dr["Operator"]
                                                      + " " + "@" + dr["SeqNo"] +
                                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);

                                whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                            }
                        }

                        // Add display only own view

                        if (!(bool) paymentView.Rows[0]["IsAll"])
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
        ///     Get payment view criteria using payment view id
        /// </summary>
        /// <returns></returns>
        public bool GetPaymentViewCriteriaList(int paymentViewID, DataSet paymentViewCriteria, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@PaymentViewID", paymentViewID.ToString(CultureInfo.InvariantCulture));

            // Get data
            var result = DbRetrieve("AP.GetPaymentViewCriteriaListByPaymentViewID", paymentViewCriteria, dbParams,
                TableName,
                connStr);

            //Return result
            return result;
        }

        /// <summary>
        ///     Get payment  view criteria schema
        /// </summary>
        /// <param name="paymentViewCriteria"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentViewCriteriaSchema(DataSet paymentViewCriteria, string connStr)
        {
            // Get data
            var result = DbRetrieveSchema("AP.GetPaymentViewCriteriaList", paymentViewCriteria, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}