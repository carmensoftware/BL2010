using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StandardVoucherViewCriteria : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public StandardVoucherViewCriteria()
        {
            SelectCommand = "SELECT * FROM GL.StandardVoucherViewCriteria";
            TableName = "StandardVoucherViewCriteria";
        }

        /// <summary>
        ///     Get standard voucher view criteria using standard voucher view id
        /// </summary>
        /// <returns></returns>
        public DataTable GetStandardVoucherViewCriteriaList(int standardVoucherViewID, string connStr)
        {
            var standardVoucherViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@StandardVoucherViewID", standardVoucherViewID.ToString());

            // Get data
            standardVoucherViewCriteria = DbRead("GL.GetStandardVoucherViewCriteriaListByStandardVoucherViewID",
                dbParams, connStr);

            //Return result
            return standardVoucherViewCriteria;
        }

        /// <summary>
        ///     Generate where clause using standardVoucherViewID
        /// </summary>
        /// <param name="standardVoucherViewID"></param>
        /// <returns></returns>
        public static string GetStandardVoucherViewCriteria(int standardVoucherViewID, int userID, string connStr)
        {
            var standardVoucherView = new DataTable();
            var standardVoucherViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];
            var whereClause = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            standardVoucherView = new StandardVoucherView().GetStandardVoucherView(standardVoucherViewID, connStr);

            if (standardVoucherView != null)
            {
                // Get standard voucher view criteria data
                standardVoucherViewCriteria =
                    new StandardVoucherViewCriteria().GetStandardVoucherViewCriteriaList(standardVoucherViewID, connStr);

                // Non-Advance option
                if (standardVoucherView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in standardVoucherViewCriteria.Rows)
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
                    whereClause = standardVoucherView.Rows[0]["AdvanceOption"].ToString();

                    foreach (DataRow dr in standardVoucherViewCriteria.Rows)
                    {
                        var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                              dr["Operator"] + " " + "@" + dr["SeqNo"] +
                                              field.GetFieldName(dr["FieldID"].ToString(), connStr);
                        whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                    }
                }

                // Add display only own view
                if (!(bool) standardVoucherView.Rows[0]["IsAll"])
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
        public static string GetStandardVoucherViewCriteriaPreview(DataSet dsPreview, int userID, string connStr)
        {
            var standardvoucherView = new DataTable();
            var standardvoucherViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];

            var whereClause = string.Empty;

            int standardvoucherViewID;

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            if (dsPreview.Tables["StandardVoucherView"] != null)
            {
                standardvoucherView = dsPreview.Tables["StandardVoucherView"];

                standardvoucherViewID = int.Parse(standardvoucherView.Rows[0]["StandardVoucherViewID"].ToString());

                if (standardvoucherView != null)
                {
                    // Get standardvoucher view criteria data

                    if (dsPreview.Tables["StandardVoucherViewCriteria"] != null)
                    {
                        standardvoucherViewCriteria = dsPreview.Tables["StandardVoucherViewCriteria"];

                        // Non-Advance option

                        if (standardvoucherView.Rows[0]["IsAdvance"].ToString() != "True")
                        {
                            foreach (DataRow dr in standardvoucherViewCriteria.Rows)
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
                            whereClause = standardvoucherView.Rows[0]["AvanceOption"].ToString();

                            foreach (DataRow dr in standardvoucherViewCriteria.Rows)
                            {
                                var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                                      dr["Operator"]
                                                      + " " + "@" + dr["SeqNo"] +
                                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);

                                whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                            }
                        }

                        // Add display only own view

                        if (!(bool) standardvoucherView.Rows[0]["IsAll"])
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
        ///     Get standard view criteria using standard view id
        /// </summary>
        /// <returns></returns>
        public bool GetStandardVoucherViewCriteriaList(int standardViewID, DataSet standardViewCriteria, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@StandardVoucherViewID", standardViewID.ToString());

            // Get data
            result = DbRetrieve("GL.GetStandardVoucherViewCriteriaListByStandardVoucherViewID", standardViewCriteria,
                dbParams, TableName, connStr);

            //Return result
            return result;
        }

        /// <summary>
        ///     Get standard voucher view criteria schema
        /// </summary>
        /// <param name="standardVoucherViewCriteria"></param>
        /// <returns></returns>
        public bool GetStandardVoucherViewCriteriaSchema(DataSet standardVoucherViewCriteria, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetStandardVoucherViewCriteriaList", standardVoucherViewCriteria, null,
                TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetCriteriaColumns(string connStr)
        {
            return DbRead("GL.GetStandardVoucherViewCriteriaAvailableColumns", null, connStr);
        }

        #endregion
    }
}