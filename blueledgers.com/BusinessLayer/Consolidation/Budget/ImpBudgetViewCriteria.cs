using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.Budget
{
    public class ImpBudgetViewCriteria : DbHandler
    {
        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public ImpBudgetViewCriteria()
        {
            SelectCommand = "SELECT * FROM Budget.ImpBudgetViewCriteria";
            TableName = "ImpBudgetViewCriteria";
        }

        /// <summary>
        ///     Get table impbudgetviewcriteria by impbudgetview id.
        /// </summary>
        /// <returns></returns>
        public bool GetImpBudgetViewCriteriaList(int impBudgetViewID, DataSet impBudgetViewCriteria, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@impBudgetViewID", impBudgetViewID.ToString());

            // Get data
            result = DbRetrieve("Budget.GetImpBudgetViewCriteriaListByImpBudgetViewID", impBudgetViewCriteria, dbParams,
                TableName, connStr);

            //Return result
            return result;
        }

        /// <summary>
        ///     Get budget view criteria using budget view id
        /// </summary>
        /// <returns></returns>
        public DataTable GetImpBudgetViewCriteriaList(int impBudgetViewID, string connStr)
        {
            var impBudgetViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@impBudgetViewID", impBudgetViewID.ToString());

            // Get data
            impBudgetViewCriteria = DbRead("Budget.GetImpBudgetViewCriteriaListByImpBudgetViewID", dbParams, connStr);

            //Return result
            return impBudgetViewCriteria;
        }

        /// <summary>
        ///     Generate where clause using impbudgetViewID
        /// </summary>
        /// <param name="budgetViewID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string GetImpBudgetViewCriteria(int impBudgetViewID, int userID, string connStr)
        {
            var impBudgetView = new DataTable();
            var impBudgetViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];
            var whereClause = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            impBudgetView = new ImpBudgetView().GetImpBudgetView(impBudgetViewID, connStr);

            if (impBudgetView != null)
            {
                // Get budget view criteria data
                impBudgetViewCriteria = new ImpBudgetViewCriteria().GetImpBudgetViewCriteriaList(impBudgetViewID,
                    connStr);

                // Non-Advance option
                if (impBudgetView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in impBudgetViewCriteria.Rows)
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
                    whereClause = impBudgetView.Rows[0]["AdvanceOption"].ToString();

                    foreach (DataRow dr in impBudgetViewCriteria.Rows)
                    {
                        var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                              dr["Operator"] + " " + "@" + dr["SeqNo"] +
                                              field.GetFieldName(dr["FieldID"].ToString(), connStr);
                        whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                    }
                }

                // Add display only own view
                if (!(bool) impBudgetView.Rows[0]["IsAll"])
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
        public static string GetImpBudgetViewCriteriaPreview(DataSet dsPreview, int userID, string connStr)
        {
            var impBudgetView = new DataTable();
            var impBudgetViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];

            var whereClause = string.Empty;

            int impBudgetViewID;

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            if (dsPreview.Tables["ImpBudgetView"] != null)
            {
                impBudgetView = dsPreview.Tables["ImpBudgetView"];
                impBudgetViewID = int.Parse(impBudgetView.Rows[0]["ImpBudgetViewID"].ToString());

                if (impBudgetView != null)
                {
                    // Get impBudget view criteria data
                    if (dsPreview.Tables["ImpBudgetViewCriteria"] != null)
                    {
                        impBudgetViewCriteria = dsPreview.Tables["ImpBudgetViewCriteria"];

                        // Non-Advance option
                        if (impBudgetView.Rows[0]["IsAdvance"].ToString() != "True")
                        {
                            foreach (DataRow dr in impBudgetViewCriteria.Rows)
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
                            whereClause = impBudgetView.Rows[0]["AvanceOption"].ToString();

                            foreach (DataRow dr in impBudgetViewCriteria.Rows)
                            {
                                var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                                      dr["Operator"]
                                                      + " " + "@" + dr["SeqNo"] +
                                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);
                                whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                            }
                        }

                        // Add display only own view
                        if (!(bool) impBudgetView.Rows[0]["IsAll"])
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
        ///     Get impbudget view criteria schema
        /// </summary>
        /// <param name="budgetViewCriteria"></param>
        /// <returns></returns>
        public bool GetImpBudgetViewCriteriaSchema(DataSet impBudgetViewCriteria, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("Budget.GetImpBudgetViewCriteriaList", impBudgetViewCriteria, null, TableName,
                connStr);

            // Return result
            return result;
        }

        #endregion
    }
}