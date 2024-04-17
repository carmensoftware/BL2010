using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class BudgetViewCriteria : DbHandler
    {
        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public BudgetViewCriteria()
        {
            SelectCommand = "SELECT * FROM GL.BudgetViewCriteria";
            TableName = "BudgetViewCriteria";
        }

        /// <summary>
        ///     Get budget view criteria using budget view id
        /// </summary>
        /// <returns></returns>
        public bool GetBudgetViewCriteriaList(int budgetViewID, DataSet budgetViewCriteria, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("BudgetViewID", budgetViewID.ToString());

            // Get data
            result = DbRetrieve("GL.GetBudgetViewCriteriaListByBudgetViewID", budgetViewCriteria, dbParams, TableName,
                connStr);

            //Return result
            return result;
        }

        /// <summary>
        ///     Get budget view criteria using budget view id
        /// </summary>
        /// <returns></returns>
        public DataTable GetBudgetViewCriteriaList(int budgetViewID, string connStr)
        {
            var budgetViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("BudgetViewID", budgetViewID.ToString());

            // Get data
            budgetViewCriteria = DbRead("GL.GetBudgetViewCriteriaListByBudgetViewID", dbParams, connStr);

            //Return result
            return budgetViewCriteria;
        }

        /// <summary>
        ///     Generate where clause using budgetViewID
        /// </summary>
        /// <param name="budgetViewID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string GetBudgetViewCriteria(int budgetViewID, int userID, string connStr)
        {
            var budgetView = new DataTable();
            var budgetViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];
            var whereClause = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            budgetView = new BudgetView().GetBudgetView(budgetViewID, connStr);

            if (budgetView != null)
            {
                // Get budget view criteria data
                budgetViewCriteria = new BudgetViewCriteria().GetBudgetViewCriteriaList(budgetViewID, connStr);

                // Non-Advance option
                if (budgetView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in budgetViewCriteria.Rows)
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
                    whereClause = budgetView.Rows[0]["AdvanceOption"].ToString();

                    foreach (DataRow dr in budgetViewCriteria.Rows)
                    {
                        var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                              dr["Operator"] + " " + "@" + dr["SeqNo"] +
                                              field.GetFieldName(dr["FieldID"].ToString(), connStr);
                        whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                    }
                }

                // Add display only own view
                if (!(bool) budgetView.Rows[0]["IsAll"])
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
        public static string GetBudgetViewCriteriaPreview(DataSet dsPreview, int userID, string connStr)
        {
            var budgetView = new DataTable();
            var budgetViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];

            var whereClause = string.Empty;

            int budgetViewID;

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            if (dsPreview.Tables["BudgetView"] != null)
            {
                budgetView = dsPreview.Tables["BudgetView"];
                budgetViewID = int.Parse(budgetView.Rows[0]["BudgetViewID"].ToString());

                if (budgetView != null)
                {
                    // Get budget view criteria data

                    if (dsPreview.Tables["BudgetViewCriteria"] != null)
                    {
                        budgetViewCriteria = dsPreview.Tables["BudgetViewCriteria"];

                        // Non-Advance option
                        if (budgetView.Rows[0]["IsAdvance"].ToString() != "True")
                        {
                            foreach (DataRow dr in budgetViewCriteria.Rows)
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
                            whereClause = budgetView.Rows[0]["AvanceOption"].ToString();

                            foreach (DataRow dr in budgetViewCriteria.Rows)
                            {
                                var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                                      dr["Operator"]
                                                      + " " + "@" + dr["SeqNo"] +
                                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);

                                whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                            }
                        }

                        // Add display only own view
                        if (!(bool) budgetView.Rows[0]["IsAll"])
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
        ///     Get budget view criteria schema
        /// </summary>
        /// <param name="budgetViewCriteria"></param>
        /// <returns></returns>
        public bool GetBudgetViewCriteriaSchema(DataSet budgetViewCriteria, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetBudgetViewCriteriaList", budgetViewCriteria, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}