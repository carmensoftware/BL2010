using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class JournalVoucherViewCriteria : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public JournalVoucherViewCriteria()
        {
            SelectCommand = "SELECT * FROM GL.JournalVoucherViewCriteria";
            TableName = "JournalVoucherViewCriteria";
        }

        public int JournalVoucherViewID { get; set; }

        public int JournalVoucherViewCriteriaID { get; set; }

        public int SeqNo { get; set; }

        public string CompareOp { get; set; }

        public string LogicalOp { get; set; }

        /// <summary>
        ///     Get journalvoucher view criteria using jounalvoucher view id
        /// </summary>
        /// <returns></returns>
        public DataTable GetJournalVoucherViewCriteriaList(int journalvoucherViewID, string connStr)
        {
            var journalvoucherViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@journalvoucherViewID", journalvoucherViewID.ToString());

            // Get data
            journalvoucherViewCriteria = DbRead("GL.GetJournalVoucherViewCriteriaListByJournalVoucherViewID", dbParams,
                connStr);

            //Return result
            return journalvoucherViewCriteria;
        }

        /// <summary>
        ///     Get journalvoucher view criteria using journalvoucher view id
        /// </summary>
        /// <returns></returns>
        public bool GetJournalVoucherViewCriteriaList(int journalvoucherViewID, DataSet journalvoucherViewCriteria,
            string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@journalvoucherViewID", journalvoucherViewID.ToString());

            // Get data
            result = DbRetrieve("GL.GetJournalVoucherViewCriteriaListByJournalVoucherViewID", journalvoucherViewCriteria,
                dbParams, TableName, connStr);

            //Return result
            return result;
        }

        /// <summary>
        ///     Generate where clause using journalvoucherViewID
        /// </summary>
        /// <param name="journalvoucherViewID"></param>
        /// <returns></returns>
        public static string GetJournalVoucherViewCriteria(int journalvoucherViewID, int userID, string connStr)
        {
            var journalvoucherView = new DataTable();
            var journalvoucherViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];
            var whereClause = string.Empty;

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            journalvoucherView = new JournalVoucherView().GetJournalVoucherView(journalvoucherViewID, connStr);

            if (journalvoucherView != null)
            {
                // Get journalvoucher view criteria data
                journalvoucherViewCriteria =
                    new JournalVoucherViewCriteria().GetJournalVoucherViewCriteriaList(journalvoucherViewID, connStr);

                // Non-Advance option
                if (journalvoucherView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in journalvoucherViewCriteria.Rows)
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
                    whereClause = journalvoucherView.Rows[0]["AvanceOption"].ToString();

                    foreach (DataRow dr in journalvoucherViewCriteria.Rows)
                    {
                        var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                              dr["Operator"]
                                              + " " + "@" + dr["SeqNo"] +
                                              field.GetFieldName(dr["FieldID"].ToString(), connStr);
                        whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                    }
                }

                // Add display only own view
                if (!(bool) journalvoucherView.Rows[0]["IsAll"])
                {
                    whereClause += (whereClause != string.Empty ? " AND " : string.Empty) + " CreatedBy = " + userID;
                }
            }

            // Return result
            return whereClause;
        }

        /// <summary>
        ///     Generate where clause using datatable.
        /// </summary>
        /// <param name="dtPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetJournalVoucherViewCriteriaPreview(DataSet dsPreview, int userID, string connStr)
        {
            var journalvoucherView = new DataTable();
            var journalvoucherViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];

            var whereClause = string.Empty;

            int journalvoucherViewID;

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            if (dsPreview.Tables["JournalVoucherView"] != null)
            {
                journalvoucherView = dsPreview.Tables["JournalVoucherView"];
                journalvoucherViewID = int.Parse(journalvoucherView.Rows[0]["JournalVoucherViewID"].ToString());

                if (journalvoucherView != null)
                {
                    // Get journalvoucher view criteria data
                    if (dsPreview.Tables["JournalVoucherViewCriteria"] != null)
                    {
                        journalvoucherViewCriteria = dsPreview.Tables["JournalVoucherViewCriteria"];

                        // Non-Advance option
                        if (journalvoucherView.Rows[0]["IsAdvance"].ToString() != "True")
                        {
                            foreach (DataRow dr in journalvoucherViewCriteria.Rows)
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
                            whereClause = journalvoucherView.Rows[0]["AvanceOption"].ToString();

                            foreach (DataRow dr in journalvoucherViewCriteria.Rows)
                            {
                                var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                                      dr["Operator"]
                                                      + " " + "@" + dr["SeqNo"] +
                                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);

                                whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                            }
                        }

                        // Add display only own view
                        if (!(bool) journalvoucherView.Rows[0]["IsAll"])
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
        ///     Get journalvoucher view criteria schema
        /// </summary>
        /// <param name="accountViewCriteria"></param>
        /// <returns></returns>
        public bool GetJournalVoucherViewCriteriaSchema(DataSet journalvoucherViewCriteria, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetJournalVoucherViewCriteriaList", journalvoucherViewCriteria, null,
                TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all available criterai column.
        /// </summary>
        /// <returns></returns>
        public DataTable GetCriteriaColumns(string connStr)
        {
            return DbRead("GL.GetJournalVoucherViewCriteriaAvailableColumns", null, connStr);
        }

        #endregion
    }
}