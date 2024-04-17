using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ReceiptViewCriteria : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ReceiptViewCriteria()
        {
            SelectCommand = "SELECT * FROM AR.ReceiptViewCriteria";
            TableName = "ReceiptViewCriteria";
        }

        /// <summary>
        ///     Get Receipt view criteria using Receipt view id
        /// </summary>
        /// <returns></returns>
        public DataTable GetReceiptViewCriteriaList(int receiptViewID, string connStr)
        {
            var ReceiptViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ReceiptViewID", receiptViewID.ToString());

            // Get data
            ReceiptViewCriteria = DbRead("AR.GetReceiptViewCriteriaListByReceiptViewID", dbParams, connStr);

            //Return result
            return ReceiptViewCriteria;
        }

        /// <summary>
        ///     Generate where clause using ReceiptViewID
        /// </summary>
        /// <param name="ReceiptViewID"></param>
        /// <returns></returns>
        public static string GetReceiptViewCriteria(int receiptViewID, int userID, string connStr)
        {
            var dtReceiptView = new DataTable();
            var dtReceiptViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];
            var whereClause = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            dtReceiptView = new ReceiptView().GetReceiptView(receiptViewID, connStr);

            if (dtReceiptView != null)
            {
                // Get Receipt  view criteria data
                dtReceiptViewCriteria = new ReceiptViewCriteria().GetReceiptViewCriteriaList(receiptViewID, connStr);

                // Non-Advance option
                if (dtReceiptView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in dtReceiptViewCriteria.Rows)
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
                    whereClause = dtReceiptView.Rows[0]["AvanceOption"].ToString();

                    foreach (DataRow dr in dtReceiptViewCriteria.Rows)
                    {
                        var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                              dr["Operator"] + " " + "@" + dr["SeqNo"] +
                                              field.GetFieldName(dr["FieldID"].ToString(), connStr);
                        whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                    }
                }

                // Add display only own view
                if (!(bool) dtReceiptView.Rows[0]["IsAll"])
                {
                    whereClause += (whereClause != string.Empty ? " AND " : string.Empty) + " CreatedBy = " + userID;
                }
            }

            // Return result
            return whereClause;
        }

        /// <summary>
        ///     Get receipt view criteria.
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetReceiptViewCriteriaPreview(DataSet dsPreview, int userID, string connStr)
        {
            var dtReceiptView = new DataTable();
            var dtReceiptViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();
            var whereClause = string.Empty;

            int ReceiptViewID;

            // Checking advance option
            if (dsPreview.Tables["ReceiptView"] != null)
            {
                dtReceiptView = dsPreview.Tables["ReceiptView"];
                ReceiptViewID = int.Parse(dtReceiptView.Rows[0]["ReceiptViewID"].ToString());

                if (dtReceiptView != null)
                {
                    // Get Receipt view criteria data
                    if (dsPreview.Tables["ReceiptViewCriteria"] != null)
                    {
                        dtReceiptViewCriteria = dsPreview.Tables["ReceiptViewCriteria"];

                        // Non-Advance option
                        if (dtReceiptView.Rows[0]["IsAdvance"].ToString() != "True")
                        {
                            foreach (DataRow dr in dtReceiptViewCriteria.Rows)
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
                            whereClause = dtReceiptView.Rows[0]["AvanceOption"].ToString();

                            foreach (DataRow dr in dtReceiptViewCriteria.Rows)
                            {
                                var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                                      dr["Operator"]
                                                      + " " + "@" + dr["SeqNo"] +
                                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);
                                whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                            }
                        }
                        // Add display only own view
                        if (!(bool) dtReceiptView.Rows[0]["IsAll"])
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
        ///     Get Receipt view criteria using Receipt view id
        /// </summary>
        /// <returns></returns>
        public bool GetReceiptViewCriteriaList(int receiptViewID, DataSet dtReceiptViewCriteria, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ReceiptViewID", receiptViewID.ToString());

            // Get data
            result = DbRetrieve("AR.GetReceiptViewCriteriaListByReceiptViewID", dtReceiptViewCriteria, dbParams,
                TableName, connStr);

            //Return result
            return result;
        }

        /// <summary>
        ///     Get Receipt  view criteria schema
        /// </summary>
        /// <param name="ReceiptViewCriteria"></param>
        /// <returns></returns>
        public bool GetReceiptViewCriteriaSchema(DataSet dtReceiptViewCriteria, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("AR.GetReceiptViewCriteriaList", dtReceiptViewCriteria, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}