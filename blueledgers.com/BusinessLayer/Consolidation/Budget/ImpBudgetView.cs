using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.Budget
{
    public class ImpBudgetView : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Connect to table impbudgetview.
        /// </summary>
        public ImpBudgetView()
        {
            SelectCommand = "SELECT * FROM Budget.ImpBudgetView";
            TableName = "ImpBudgetView";
        }

        /// <summary>
        ///     Get impbudget view schema
        /// </summary>
        /// <param name="budgetView"></param>
        /// <returns></returns>
        public bool GetImpBudgetViewSchema(DataSet impBudgetView, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("Budget.GetImpBudgetViewList", impBudgetView, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get impbudget view using budget view id
        /// </summary>
        /// <param name="budgetViewID"></param>
        /// <param name="budgetView"></param>
        /// <returns></returns>
        public bool GetImpBudgetView(int impBudgetViewID, DataSet impBudgetView, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@impBudgetViewID", impBudgetViewID.ToString());

            // Get data
            result = DbRetrieve("Budget.GetImpBudgetView", impBudgetView, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get impbudget view using budget view id
        /// </summary>
        /// <param name="budgetViewID"></param>
        /// <returns></returns>
        public DataTable GetImpBudgetView(int impBudgetViewID, string connStr)
        {
            var impBudgetView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@impBudgetViewID", impBudgetViewID.ToString());

            // Get data
            impBudgetView = DbRead("Budget.GetImpBudgetView", dbParams, connStr);

            // Return result
            return impBudgetView;
        }

        /// <summary>
        ///     Get all impbudget view depend on user id.
        /// </summary>
        /// <returns></returns>
        public DataTable GetImpBudgetViewList(int userID, string connStr)
        {
            var dtImpBudgetView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@CreatedBy", userID.ToString());

            // Get data
            dtImpBudgetView = DbRead("Budget.GetImpBudgetViewListByCreatedBy", dbParams, connStr);

            // Return result
            return dtImpBudgetView;
        }

        /// <summary>
        ///     Generate query of assigned BudgetViewID
        /// </summary>
        /// <returns></returns>
        public string GetImpBudgetViewQuery(int impBudgetViewID, int userID, string connStr)
        {
            var impBudgetViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            columnList = ImpBudgetViewColumn.GetImpBudgetViewColumn(impBudgetViewID, connStr);

            // Generate where clause
            whereClause = ImpBudgetViewCriteria.GetImpBudgetViewCriteria(impBudgetViewID, userID, connStr);

            // Generate query
            impBudgetViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                                 " ImpBudgetCode FROM Budget.vImpBudget " +
                                 (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return impBudgetViewQuery;
        }

        /// <summary>
        ///     Get lastest budget view id
        /// </summary>
        /// <returns></returns>
        public int GetMaxImpBudgetViewID(string connStr)
        {
            var maxImpBudgetViewID = 0;

            // Get data
            maxImpBudgetViewID = DbReadScalar("Budget.GetMaxImpBudgetViewID", null, connStr);

            // Return result
            return maxImpBudgetViewID;
        }

        /// <summary>
        ///     Validate query before save
        /// </summary>
        /// <returns></returns>
        public bool GetImpBudgetViewIsValidQuery(DataTable impBudgetView, DataTable impBudgetViewCriteria,
            DataTable impBudgetViewColumn, string connStr)
        {
            var impBudgetViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;
            var result = new DataTable();
            var dbParams = new DbParameter[impBudgetViewCriteria.DefaultView.Count];
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate Column
            if (impBudgetViewColumn != null)
            {
                foreach (DataRow dr in impBudgetViewColumn.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        columnList += (columnList != string.Empty ? "," : string.Empty) +
                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);
                    }
                }
            }

            // Generate Criteria
            if (impBudgetViewCriteria != null)
            {
                // Non-Advance option
                if (impBudgetView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in impBudgetViewCriteria.Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            whereClause += (whereClause != string.Empty ? " " : string.Empty) +
                                           field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " + dr["Operator"] +
                                           " " +
                                           "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr) +
                                           " " + dr["LogicalOp"];
                        }
                    }
                }
                    // Advance option
                else
                {
                    whereClause = impBudgetView.Rows[0]["AdvanceOption"].ToString();

                    foreach (DataRow dr in impBudgetViewCriteria.Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                                  dr["Operator"] + " " + "@" + dr["SeqNo"] +
                                                  field.GetFieldName(dr["FieldID"].ToString(), connStr);
                            whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                        }
                    }
                }

                // Generate parameter
                if (impBudgetViewCriteria.Rows.Count > 0)
                {
                    foreach (DataRow dr in impBudgetViewCriteria.Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            dbParams[(int) dr["SeqNo"] - 1] =
                                new DbParameter(
                                    "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                                    dr["Value"].ToString());
                        }
                    }
                }
                else
                {
                    dbParams = null;
                }
            }

            // Generate query
            impBudgetViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                                 " ImpBudgetCode FROM Budget.vImpBudget " +
                                 (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            // Vlidate query
            result = DbExecuteQuery(impBudgetViewQuery, dbParams, connStr);

            // Return result
            if (result != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetImpBudgetViewQueryPreview(DataSet dsPreview, int userID, string connStr)
        {
            var impBudgetViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            if (dsPreview.Tables["ImpBudgetViewColumn"] != null)
            {
                columnList = ImpBudgetViewColumn.GetImpBudgetViewColumnPreview(dsPreview.Tables["ImpBudgetViewColumn"],
                    connStr);
            }

            // Generate where clause
            if (dsPreview.Tables["ImpBudgetViewCriteria"] != null)
            {
                whereClause = ImpBudgetViewCriteria.GetImpBudgetViewCriteriaPreview(dsPreview, userID, connStr);
            }

            // Generate query
            impBudgetViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                                 " ImpBudgetCode FROM Budget.vImpBudget " +
                                 (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return impBudgetViewQuery;
        }


        /// <summary>
        ///     Commit to data base for save
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var impBudgetViewCriteria = new ImpBudgetViewCriteria();
            var impBudgetViewColumn = new ImpBudgetViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, impBudgetViewCriteria.SelectCommand,
                impBudgetViewCriteria.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, impBudgetViewColumn.SelectCommand,
                impBudgetViewColumn.TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        ///     Commit to data base for delete
        /// </summary>
        /// <param name="deletedData"></param>
        /// <returns></returns>
        public bool Delete(DataSet deletedData, string connStr)
        {
            var impBudgetViewCriteria = new ImpBudgetViewCriteria();
            var impBudgetViewColumn = new ImpBudgetViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(deletedData, impBudgetViewCriteria.SelectCommand,
                impBudgetViewCriteria.TableName);
            dbSaveSource[1] = new DbSaveSource(deletedData, impBudgetViewColumn.SelectCommand,
                impBudgetViewColumn.TableName);
            dbSaveSource[2] = new DbSaveSource(deletedData, SelectCommand, TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        ///     Get lastest impbudget view id
        /// </summary>
        /// <returns></returns>
        public int GetImpBudgetViewMaxID(string connStr)
        {
            var maxImpBudgetViewID = 0;

            // Get data
            maxImpBudgetViewID = DbReadScalar("Budget.GetImpBudgetViewMaxID", null, connStr);

            // Return result
            return maxImpBudgetViewID;
        }

        #endregion
    }
}