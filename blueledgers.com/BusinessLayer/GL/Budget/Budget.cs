using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class Budget : DbHandler
    {
        #region "Attributies"

        private readonly BudGetComment _budGetComment = new BudGetComment();
        private readonly BudgetAttachment _budgetAttachment = new BudgetAttachment();
        private readonly BudgetDetail _budgetDetail = new BudgetDetail();
        private readonly BudgetMisc _budgetMisc = new BudgetMisc();

        #endregion

        #region "Operations"  

        public Budget()
        {
            SelectCommand = "SELECT * FROM GL.Budget";
            TableName = "Budget";
        }

        /// <summary>
        ///     Return datatable from BudgetList.
        /// </summary>
        /// <param name="dsBudget"></param>
        public void GetBudgetList(DataSet dsBudget, string connStr)
        {
            DbRetrieve("GL.GetBudgetList", dsBudget, null, TableName, connStr);
        }

        /// <summary>
        ///     Get budget using budget view id
        /// </summary>
        /// <param name="dsBudget"></param>
        /// <param name="budgetViewID"></param>
        /// <param name="userID"></param>
        public void GetBudgetList(DataSet dsBudget, int budgetViewID, int userID, string connStr)
        {
            var dtBudget = new DataTable();
            var dtBudgetViewCriteria = new DataTable();
            var budget = new Budget();
            var budgetViewQuery = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate query
            budgetViewQuery = new BudgetView().GetBudgetViewQuery(budgetViewID, userID, connStr);

            // Generate parameter
            dtBudgetViewCriteria = new BudgetViewCriteria().GetBudgetViewCriteriaList(budgetViewID, connStr);

            if (dtBudgetViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtBudgetViewCriteria.Rows.Count];

                for (var i = 0; i < dtBudgetViewCriteria.Rows.Count; i++)
                {
                    var dr = dtBudgetViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtBudget = budget.DbExecuteQuery(budgetViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtBudget = budget.DbExecuteQuery(budgetViewQuery, null, connStr);
            }

            // Return resutl
            if (dsBudget.Tables[TableName] != null)
            {
                dsBudget.Tables.Remove(TableName);
            }

            dtBudget.TableName = TableName;
            dsBudget.Tables.Add(dtBudget);
        }

        /// <summary>
        ///     Return datatable from Budget.
        /// </summary>
        /// <param name="budgetID"></param>
        /// <param name="dsBudget"></param>
        /// <returns></returns>
        public bool GetBudget(string budgetID, string DepCode, string DivCode, string SecCode, DataSet dsBudget,
            string connStr)
            //public bool GetBudget(string budgetID, DataSet dsBudget, string connStr)
        {
            //try
            //{
            //    // Paramter value assign to dbparameter array.
            //    DbParameter[] dbParams  = new DbParameter[1];
            //    dbParams[0]             = new DbParameter("@budgetID",budgetID);

            //    return DbRetrieve("GL.GetBudget", dsBudget, dbParams,this.TableName,connStr);       
            //}
            //catch
            //{
            //    return false;
            //}

            try
            {
                // Paramter value assign to dbparameter array.
                var dbParams = new DbParameter[4];
                dbParams[0] = new DbParameter("@BudgetID", budgetID);
                dbParams[1] = new DbParameter("@DepartmentCode", DepCode);
                dbParams[2] = new DbParameter("@DivisionCode", DivCode);
                dbParams[3] = new DbParameter("@SectionCode", SecCode);

                return DbRetrieve("GL.GetBudget", dsBudget, dbParams, TableName, connStr);
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="dsAccRec"></param>
        /// <param name="budgetID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAccRec, int budgetID, string connStr)
        {
            var result = false;

            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BudgetID", budgetID.ToString());

            // Get data
            result = DbRetrieve("GL.GetBudgetByBudgetID", dsAccRec, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get budget for preview
        /// </summary>
        /// <param name="dsBudget"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetBudgetPreveiw(DataSet dsBudget, int userID, string connStr)
        {
            var dtBudget = new DataTable();
            var dtBudgetViewCriteria = new DataTable();
            var budget = new Budget();
            var budgetViewQuery = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate  query
            budgetViewQuery = new BudgetView().GetBudgetViewQueryPreview(dsBudget, userID, connStr);

            // Generate  parameter
            dtBudgetViewCriteria = dsBudget.Tables["BudgetViewCriteria"];

            if (dtBudgetViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtBudgetViewCriteria.Rows.Count];

                for (var i = 0; i < dtBudgetViewCriteria.Rows.Count; i++)
                {
                    var dr = dtBudgetViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtBudget = budget.DbExecuteQuery(budgetViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtBudget = budget.DbExecuteQuery(budgetViewQuery, null, connStr);
            }

            // Return result
            if (dsBudget.Tables[TableName] != null)
            {
                dsBudget.Tables.Remove(TableName);
            }

            dtBudget.TableName = TableName;
            dsBudget.Tables.Add(dtBudget);
        }

        /// <summary>
        ///     Retrieve schema to dataset
        /// </summary>
        /// <param name="dsBudget"></param>
        public void GetBudgetSchema(DataSet dsBudget, string connStr)
        {
            DbRetrieveSchema("GL.GetBudgetList", dsBudget, null, TableName, connStr);
        }

        /// <summary>
        ///     Get new budgetid
        /// </summary>
        /// <returns></returns>
        public int GetNewBudgetID(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("GL.GetBudgetMax", null, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get new revisionnumber
        /// </summary>
        /// <returns></returns>
        public int GetNewRevision(string year, string connStr)
        {
            var result = 0;

            // Get data
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Year", year);

            result = DbReadScalar("GL.GetBudgetRevisionMax", dbParams, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Save data
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool GetSave(DataSet savedData, string Type, string connStr)
        {
            // Build savesource object
            if (Type == "2")
            {
                var dbSaveSource = new DbSaveSource[4];
                dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
                dbSaveSource[1] = new DbSaveSource(savedData, _budgetMisc.SelectCommand, _budgetMisc.TableName);
                dbSaveSource[2] = new DbSaveSource(savedData, _budgetAttachment.SelectCommand, _budgetAttachment.TableName);
                dbSaveSource[3] = new DbSaveSource(savedData, _budGetComment.SelectCommand, _budGetComment.TableName);

                // Call function dbCommit for commit data to database
                return DbCommit(dbSaveSource, connStr);
            }
            else
            {
                var dbSaveSource = new DbSaveSource[1];
                dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);

                // Call function dbCommit for commit data to database
                return DbCommit(dbSaveSource, connStr);
            }
        }

        /// <summary>
        ///     Delete row
        /// </summary>
        /// <param name="dsBudget"></param>
        /// <returns></returns>
        public bool GetDelete(DataSet dsBudget, string connStr)
        {
            var result = false;

            var dbSaveSource = new DbSaveSource[4];

            // Create dbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsBudget, _budgetMisc.SelectCommand, _budgetMisc.TableName);
            dbSaveSource[1] = new DbSaveSource(dsBudget, _budgetDetail.SelectCommand, _budgetDetail.TableName);
            dbSaveSource[2] = new DbSaveSource(dsBudget, _budgetAttachment.SelectCommand, _budgetMisc.TableName);
            dbSaveSource[3] = new DbSaveSource(dsBudget, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Check Revision Number
        /// </summary>
        /// <returns></returns>
        public bool CheckRevisionNo(string RevisionNo, string Year, string connStr)
        {
            var dsTmp = new DataSet();
            int row;

            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@revision", RevisionNo);
            dbParams[1] = new DbParameter("@year", Year);

            DbRetrieve("GL.GetBudgetRevisionNo", dsTmp, dbParams, TableName, connStr);

            row = dsTmp.Tables[TableName].Rows.Count;

            if (row == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Retrieve data about year that set lookup
        /// </summary>
        /// <returns></returns>
        public DataTable GetYear(string connStr)
        {
            var dsYear = new DataSet();

            // Get data
            var budget = new Budget();
            budget.DbRetrieve("GL.GetYear", dsYear, null, TableName, connStr);

            // Return result
            if (dsYear.Tables[TableName] != null)
            {
                var drBlank = dsYear.Tables[TableName].NewRow();
                dsYear.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsYear.Tables[TableName];
        }

        /// <summary>
        ///     Retrieve data about division, revision, section, department that set lookup
        /// </summary>
        /// <returns></returns>
        public DataTable GetRevisionNumberLookup(string year, string connStr)
        {
            //DbParameter[] dbParams = new DbParameter[1];
            //dbParams[0] = new DbParameter("@Year", year);

            //return DbRead("GL.GetRevisionNoLookup",dbParams);

            var dsRevisionNumber = new DataSet();

            // Get data
            var budget = new Budget();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Year", year);

            budget.DbRetrieve("GL.GetRevisionNoLookup", dsRevisionNumber, dbParams, TableName, connStr);

            // Return result
            if (dsRevisionNumber.Tables[TableName] != null)
            {
                var drBlank = dsRevisionNumber.Tables[TableName].NewRow();
                dsRevisionNumber.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsRevisionNumber.Tables[TableName];
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public DataSet GetDisplayGrid(string chk, string year, string revision, string division, string department,
            string section, string connStr)
        {
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[6];
            dbParams[0] = new DbParameter("@Chk", chk);
            dbParams[1] = new DbParameter("@Year", year);
            dbParams[2] = new DbParameter("@RevisionNo", revision);
            dbParams[3] = new DbParameter("@DivisionCode", division);
            dbParams[4] = new DbParameter("@DepartmentCode", department);
            dbParams[5] = new DbParameter("@SectionCode", section);

            //DbRetrieve("GL.GetBudgetSAcc", dsTmp, dbParams, this.TableName,connStr);
            DbRetrieve("GL.GetBudgetCopyGrid", dsTmp, dbParams, TableName, connStr);
            return dsTmp;
        }

        /// <summary>
        ///     Get data from field exchangerate in table budget
        /// </summary>
        /// <returns></returns>
        public decimal GetExchangeRate(string year, string budgetID, string connStr)
        {
            var dsTmp = new DataSet();
            var dtTmp = new DataTable();

            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@Year", year);
            dbParams[1] = new DbParameter("@BudgetID", budgetID);

            dtTmp = DbRead("GL.GetBudgetExchangeRate", dbParams, connStr);

            return (decimal) dtTmp.Rows[0]["ExchangeRate"];
        }

        #endregion
    }
}