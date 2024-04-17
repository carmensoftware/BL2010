using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class BudgetView : DbHandler
    {
        #region "Attributies"

        //private BudgetViewCriteria[] _BudgetViewCriterias;
        //private BudgetViewColumn[] _BudgetViewColumns;
        //private int budgetViewID;
        //private string description;
        //private bool isPublic;
        //private bool isAll;
        //private bool isAdvance;
        //private bool isDefault;
        //private DateTime createdDate;
        //private int createdBy;
        //private DateTime updatedDate;
        //private int updatedBy;

        //public int BudgetViewID
        //{
        //    get
        //    {
        //        return budgetViewID;
        //    }

        //    set
        //    {
        //        budgetViewID = value;
        //    }
        //}

        //public string Description
        //{
        //    get
        //    {
        //        return description;
        //    }

        //    set
        //    {
        //        description = value;
        //    }
        //}

        //public bool IsPublic
        //{
        //    get
        //    {
        //        return isPublic;
        //    }

        //    set
        //    {
        //        isPublic = value;
        //    }
        //}

        //public bool IsAll
        //{
        //    get
        //    {
        //        return isAll;
        //    }

        //    set
        //    {
        //        isAll = value;
        //    }
        //}

        //public bool IsAdvance
        //{
        //    get
        //    {
        //        return isAdvance;
        //    }

        //    set
        //    {
        //        isAdvance = value;
        //    }
        //}

        //public bool IsDefault
        //{
        //    get
        //    {
        //        return isDefault;
        //    }

        //    set
        //    {
        //        isDefault = value;
        //    }
        //}

        //public DateTime CreatedDate
        //{
        //    get
        //    {
        //        return createdDate;
        //    }

        //    set
        //    {
        //        createdDate = value;
        //    }
        //}

        //public int CreatedBy
        //{
        //    get
        //    {
        //        return createdBy;
        //    }

        //    set
        //    {
        //        createdBy = value;
        //    }
        //}

        //public DateTime UpdatedDate
        //{
        //    get
        //    {
        //        return updatedDate;
        //    }

        //    set
        //    {
        //        updatedDate = value;
        //    }
        //}

        //public int UpdatedBy
        //{
        //    get
        //    {
        //        return updatedBy;
        //    }

        //    set
        //    {
        //        updatedBy = value;
        //    }
        //}

        #endregion

        #region "Operations"

        /// <summary>
        ///     Connect to table budgetview.
        /// </summary>
        public BudgetView()
        {
            SelectCommand = "SELECT * FROM GL.BudgetView";
            TableName = "BudgetView";
        }

        /// <summary>
        ///     Get budget view schema
        /// </summary>
        /// <param name="budgetView"></param>
        /// <returns></returns>
        public bool GetBudgetViewSchema(DataSet budgetView, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetBudgetViewList", budgetView, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get budget view using budget view id
        /// </summary>
        /// <param name="budgetViewID"></param>
        /// <param name="budgetView"></param>
        /// <returns></returns>
        public bool GetBudgetView(int budgetViewID, DataSet budgetView, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@BudgetViewID", budgetViewID.ToString());

            // Get data
            result = DbRetrieve("GL.GetBudgetView", budgetView, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get budget view using budget view id
        /// </summary>
        /// <param name="budgetViewID"></param>
        /// <returns></returns>
        public DataTable GetBudgetView(int budgetViewID, string connStr)
        {
            var budgetView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@BudgetViewID", budgetViewID.ToString());

            // Get data
            budgetView = DbRead("GL.GetBudgetView", dbParams, connStr);

            // Return result
            return budgetView;
        }

        /// <summary>
        ///     Get all budget view depend on user id.
        /// </summary>
        /// <returns></returns>
        public DataTable GetBudgetViewList(int userID, string connStr)
        {
            var dtBudgetView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@CreatedBy", userID.ToString());

            // Get data
            dtBudgetView = DbRead("GL.GetBudgetViewListByCreatedBy", dbParams, connStr);

            // Return result
            return dtBudgetView;
        }

        /// <summary>
        ///     Generate query of assigned BudgetViewID
        /// </summary>
        /// <returns></returns>
        public string GetBudgetViewQuery(int budgetViewID, int userID, string connStr)
        {
            var budgetViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            columnList = BudgetViewColumn.GetBudgetViewColumn(budgetViewID, connStr);

            // Generate where clause
            whereClause = BudgetViewCriteria.GetBudgetViewCriteria(budgetViewID, userID, connStr);

            // Generate query
            budgetViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                              " BudgetID, DepartmentCode, DivisionCode, SectionCode FROM GL.vBudget " +
                              (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            //budgetViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
            //                  " BudgetID FROM GL.vBudget " +
            //                  (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return budgetViewQuery;
        }

        /// <summary>
        ///     Generate query of assigned dataset
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetBudgetViewQueryPreview(DataSet dsPreview, int userID, string connStr)
        {
            var budgetViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            if (dsPreview.Tables["BudgetViewColumn"] != null)
            {
                columnList = BudgetViewColumn.GetBudgetViewColumnPreview(dsPreview.Tables["BudgetViewColumn"], connStr);
            }

            // Generate where clause
            if (dsPreview.Tables["BudgetViewCriteria"] != null)
            {
                whereClause = BudgetViewCriteria.GetBudgetViewCriteriaPreview(dsPreview, userID, connStr);
            }

            // Generate query
            budgetViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                              " BudgetID FROM GL.vBudget " +
                              (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return budgetViewQuery;
        }

        /// <summary>
        ///     Get lastest budget view id
        /// </summary>
        /// <returns></returns>
        public int GetMaxBudgetViewID(string connStr)
        {
            var maxBudgetViewID = 0;

            // Get data
            maxBudgetViewID = DbReadScalar("GL.GetMaxBudgetViewID", null, connStr);

            // Return result
            return maxBudgetViewID;
        }

        /// <summary>
        ///     Validate query before save
        /// </summary>
        /// <returns></returns>
        public bool GetBudgetViewIsValidQuery(DataTable budgetView, DataTable budgetViewCriteria,
            DataTable budgetViewColumn, string connStr)
        {
            var budgetViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;
            var result = new DataTable();
            var dbParams = new DbParameter[budgetViewCriteria.DefaultView.Count];
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate Column
            if (budgetViewColumn != null)
            {
                foreach (DataRow dr in budgetViewColumn.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        columnList += (columnList != string.Empty ? "," : string.Empty) +
                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);
                    }
                }
            }

            // Generate Criteria
            if (budgetViewCriteria != null)
            {
                // Non-Advance option
                if (budgetView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in budgetViewCriteria.Rows)
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
                    whereClause = budgetView.Rows[0]["AdvanceOption"].ToString();

                    foreach (DataRow dr in budgetViewCriteria.Rows)
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
                if (budgetViewCriteria.Rows.Count > 0)
                {
                    foreach (DataRow dr in budgetViewCriteria.Rows)
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
            budgetViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                              " BudgetID FROM GL.vBudget " +
                              (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            // Vlidate query
            result = DbExecuteQuery(budgetViewQuery, dbParams, connStr);

            // Return result
            if (result != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Commit to data base for save
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var budgetViewCriteria = new BudgetViewCriteria();
            var budgetViewColumn = new BudgetViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, budgetViewCriteria.SelectCommand, budgetViewCriteria.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, budgetViewColumn.SelectCommand, budgetViewColumn.TableName);

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
            var budgetViewCriteria = new BudgetViewCriteria();
            var budgetViewColumn = new BudgetViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(deletedData, budgetViewCriteria.SelectCommand,
                budgetViewCriteria.TableName);
            dbSaveSource[1] = new DbSaveSource(deletedData, budgetViewColumn.SelectCommand, budgetViewColumn.TableName);
            dbSaveSource[2] = new DbSaveSource(deletedData, SelectCommand, TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}