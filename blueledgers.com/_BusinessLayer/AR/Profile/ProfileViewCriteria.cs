using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ProfileViewCriteria : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ProfileViewCriteria()
        {
            SelectCommand = "SELECT * FROM AR.ProfileViewCriteria";
            TableName = "ProfileViewCriteria";
        }

        /// <summary>
        ///     Get debtor view criteria using debtor view id
        /// </summary>
        /// <returns></returns>
        public DataTable GetList(int debtorViewID, string connStr)
        {
            var debtorViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileViewID", debtorViewID.ToString());

            // Get data
            debtorViewCriteria = DbRead("AR.GetProfileViewCriteriaByProfileViewID", dbParams, connStr);

            //Return result
            return debtorViewCriteria;
        }

        /// <summary>
        ///     Get debtor view criteria using debtor view id
        /// </summary>
        /// <returns></returns>
        public bool GetList(int debtorViewID, DataSet debtorViewCriteria, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileViewID", debtorViewID.ToString());

            // Get data
            result = DbRetrieve("AR.GetProfileViewCriteriaByProfileViewID", debtorViewCriteria, dbParams, TableName,
                connStr);

            //Return result
            return result;
        }

        /// <summary>
        ///     Generate where clause using debtorViewID
        /// </summary>
        /// <param name="DebtorViewID"></param>
        /// <returns></returns>
        public static string GetWhereClause(int debtorViewID, int userID, string connStr)
        {
            var debtorView = new DataTable();
            var debtorViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];
            var whereClause = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            debtorView = new ProfileView().GetView(debtorViewID, connStr);

            if (debtorView != null)
            {
                // Get debtor view criteria data
                debtorViewCriteria = new ProfileViewCriteria().GetList(debtorViewID, connStr);

                // Non-Advance option
                if (debtorView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in debtorViewCriteria.Rows)
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
                    whereClause = debtorView.Rows[0]["AvanceOption"].ToString();

                    foreach (DataRow dr in debtorViewCriteria.Rows)
                    {
                        var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                              dr["Operator"] + " " + "@" + dr["SeqNo"] +
                                              field.GetFieldName(dr["FieldID"].ToString(), connStr);
                        whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                    }
                }

                // Add display only own view
                if (!(bool) debtorView.Rows[0]["IsAll"])
                {
                    whereClause += (whereClause != string.Empty ? " AND " : string.Empty) + " CreatedBy = " + userID;
                }
            }

            // Return result
            return whereClause;
        }

        /// <summary>
        ///     Get data from tabel debtro view criteria
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetPreview(DataSet dsPreview, int userID, string connStr)
        {
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            var debtorView = new DataTable();
            var debtorViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];

            var whereClause = string.Empty;
            int debtorViewID;

            // Checking advance option
            if (dsPreview.Tables["DebtorView"] != null)
            {
                debtorView = dsPreview.Tables["DebtorView"];
                debtorViewID = int.Parse(debtorView.Rows[0]["DebtorViewID"].ToString());

                if (debtorView != null)
                {
                    // Get debtor view criteria data

                    if (dsPreview.Tables["ProfileViewCriteria"] != null)
                    {
                        debtorViewCriteria = dsPreview.Tables["ProfileViewCriteria"];

                        // Non-Advance option
                        if (debtorView.Rows[0]["IsAdvance"].ToString() != "True")
                        {
                            foreach (DataRow dr in debtorViewCriteria.Rows)
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
                            whereClause = debtorView.Rows[0]["AvanceOption"].ToString();
                            foreach (DataRow dr in debtorViewCriteria.Rows)
                            {
                                var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                                      dr["Operator"]
                                                      + " " + "@" + dr["SeqNo"] +
                                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);
                                whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                            }
                        }

                        // Add display only own view
                        if (!(bool) debtorView.Rows[0]["IsAll"])
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
        ///     Get debtor view criteria schema
        /// </summary>
        /// <param name="debtorViewCriteria"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet debtorViewCriteria, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("AR.GetProfileViewCriteriaList", debtorViewCriteria, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}