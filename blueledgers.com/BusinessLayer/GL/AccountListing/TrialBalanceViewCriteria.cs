using System.Data;
using Blue.DAL;

namespace Blue.BL.GL.AccountListing
{
    public class TrialBalanceViewCriteria : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        public TrialBalanceViewCriteria()
        {
            SelectCommand = "SELECT * FROM GL.TrialBalanceViewCriteria";
            TableName = "TrialBalanceViewCriteria";
        }


        /// <summary>
        ///     This function is used for get all GL.TrialBalanceViewCriteria data that related to
        ///     specified view id by using strored procedure "GL.GetTrialBalanceViewCriteriaList_ViewID"
        /// </summary>
        /// <param name="dsTriBalList"></param>
        /// <param name="viewID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsTriBalList, int viewID, string connStr)
        {
            // Declare variable
            var result = false;

            var dbParams = new DbParameter[1];


            // Create parameter
            dbParams[0] = new DbParameter("@ViewID", viewID.ToString());

            // Get data
            result = DbRetrieve("GL.GetTrialBalanceViewCriteriaList_ViewID", dsTriBalList, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     This function user for get table schema of trial balance view criteria
        ///     data by using stored procedure "GL.GetTrialBalanceViewCriteriaList".
        /// </summary>
        /// <param name="dsTrialBalView"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsTrialBalView, string connStr)
        {
            // Declare variable
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetTrialBalanceViewCriteriaList", dsTrialBalView, null, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        ///     This function user for get whereclause criteria.
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetTrialBalanceViewCriteriaPreview(DataSet dsPreview, int userID, string connStr)
        {
            var trialBalanceView = new DataTable();
            var trialBalanceViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];

            var whereClause = string.Empty;

            int trialBalanceViewID;

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            if (dsPreview.Tables["TrialBalanceView"] != null)
            {
                trialBalanceView = dsPreview.Tables["TrialBalanceView"];

                trialBalanceViewID = int.Parse(trialBalanceView.Rows[0]["TrialBalanceViewID"].ToString());

                if (trialBalanceView != null)
                {
                    // Get trialBalance view criteria data

                    if (dsPreview.Tables["TrialBalanceViewCriteria"] != null)
                    {
                        trialBalanceViewCriteria = dsPreview.Tables["TrialBalanceViewCriteria"];

                        // Non-Advance option
                        if (trialBalanceView.Rows[0]["IsAdvance"].ToString() != "True")
                        {
                            foreach (DataRow dr in trialBalanceViewCriteria.Rows)
                            {
                                whereClause += (whereClause != string.Empty ? " " : string.Empty) +
                                               field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                               dr["Operator"] + " " +
                                               "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr) +
                                               " " + dr["LogicalOp"];
                            }
                        }

                            //Advance option

                        else
                        {
                            whereClause = trialBalanceView.Rows[0]["AvanceOption"].ToString();

                            foreach (DataRow dr in trialBalanceViewCriteria.Rows)
                            {
                                var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                                      dr["Operator"]
                                                      + " " + "@" + dr["SeqNo"] +
                                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);

                                whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                            }
                        }

                        // Add display only own view

                        if (!(bool) trialBalanceView.Rows[0]["IsAll"])
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

        #endregion
    }
}