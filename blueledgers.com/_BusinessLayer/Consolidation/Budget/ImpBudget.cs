using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.Budget
{
    public class ImpBudget : DbHandler
    {
        #region "Attributies"

        private readonly ImpBudgetDetail _impBudgetDetail = new ImpBudgetDetail();

        #endregion

        #region "Operations"

        public ImpBudget()
        {
            SelectCommand = "SELECT * FROM Budget.ImpBudget";
            TableName = "ImpBudget";
        }

        /// <summary>
        ///     Return datatable from ImpBudgetList.
        /// </summary>
        /// <param name="dsBudget"></param>
        public void GetImpBudgetList(DataSet dsImpBudget, string connStr)
        {
            DbRetrieve("Budget.GetImpBudgetList", dsImpBudget, null, TableName, connStr);
        }

        /// <summary>
        ///     Get impbudget using impbudget view id
        /// </summary>
        /// <param name="dsBudget"></param>
        /// <param name="budgetViewID"></param>
        /// <param name="userID"></param>
        public void GetImpBudgetList(DataSet dsImpBudget, int impBudgetViewID, int userID, string connStr)
        {
            var dtImpBudget = new DataTable();
            var dtImpBudgetViewCriteria = new DataTable();
            var impBudget = new ImpBudget();
            var impBudgetViewQuery = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate query
            impBudgetViewQuery = new ImpBudgetView().GetImpBudgetViewQuery(impBudgetViewID, userID, connStr);

            // Generate parameter
            dtImpBudgetViewCriteria = new ImpBudgetViewCriteria().GetImpBudgetViewCriteriaList(impBudgetViewID, connStr);

            if (dtImpBudgetViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtImpBudgetViewCriteria.Rows.Count];

                for (var i = 0; i < dtImpBudgetViewCriteria.Rows.Count; i++)
                {
                    var dr = dtImpBudgetViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtImpBudget = impBudget.DbExecuteQuery(impBudgetViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtImpBudget = impBudget.DbExecuteQuery(impBudgetViewQuery, null, connStr);
            }

            // Return resutl
            if (dsImpBudget.Tables[TableName] != null)
            {
                dsImpBudget.Tables.Remove(TableName);
            }

            dtImpBudget.TableName = TableName;
            dsImpBudget.Tables.Add(dtImpBudget);
        }

        /// <summary>
        ///     Get ImpBudget Preview.
        /// </summary>
        /// <param name="dsImpBudget"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetImpBudgetPreveiw(DataSet dsImpBudget, int userID, string connStr)
        {
            var dtImpBudget = new DataTable();
            var dtImpBudgetViewCriteria = new DataTable();
            var impBudget = new ImpBudget();
            var impBudgetViewQuery = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate  query
            impBudgetViewQuery = new ImpBudgetView().GetImpBudgetViewQueryPreview(dsImpBudget, userID, connStr);

            // Generate  parameter
            dtImpBudgetViewCriteria = dsImpBudget.Tables["ImpBudgetViewCriteria"];

            if (dtImpBudgetViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtImpBudgetViewCriteria.Rows.Count];

                for (var i = 0; i < dtImpBudgetViewCriteria.Rows.Count; i++)
                {
                    var dr = dtImpBudgetViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtImpBudget = impBudget.DbExecuteQuery(impBudgetViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtImpBudget = impBudget.DbExecuteQuery(impBudgetViewQuery, null, connStr);
            }

            // Return result
            if (dsImpBudget.Tables[TableName] != null)
            {
                dsImpBudget.Tables.Remove(TableName);
            }

            dtImpBudget.TableName = TableName;
            dsImpBudget.Tables.Add(dtImpBudget);
        }

        /// <summary>
        ///     Return datatable from ImpBudget.
        /// </summary>
        /// <param name="budgetID"></param>
        /// <param name="dsBudget"></param>
        /// <returns></returns>
        public bool GetImpBudget(string impBudgetID, DataSet dsImpBudget, string connStr)
        {
            try
            {
                // Paramter value assign to dbparameter array.
                var dbParams = new DbParameter[1];
                dbParams[0] = new DbParameter("@impBudgetCode", impBudgetID);

                return DbRetrieve("Budget.GetImpBudget", dsImpBudget, dbParams, TableName, connStr);
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                return false;
            }
        }

        /// <summary>
        ///     Retrieve schema to dataset
        /// </summary>
        /// <param name="dsBudget"></param>
        public void GetImpBudgetSchema(DataSet dsImpBudget, string connStr)
        {
            DbRetrieveSchema("Budget.GetImpBudgetList", dsImpBudget, null, TableName, connStr);
        }

        /// <summary>
        ///     Get new budgetid
        /// </summary>
        /// <returns></returns>
        public int GetNewImpBudgetCode(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("Budget.GetMaxImpBudgetCode", null, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Save data
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool GetSave(DataSet savedData, string connStr)
        {
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, _impBudgetDetail.SelectCommand, _impBudgetDetail.TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        ///     Delete row
        /// </summary>
        /// <param name="dsBudget"></param>
        /// <returns></returns>
        public bool GetDelete(DataSet dsImpBudget, string connStr)
        {
            var result = false;

            var dbSaveSource = new DbSaveSource[2];

            // Create dbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsImpBudget, _impBudgetDetail.SelectCommand, _impBudgetDetail.TableName);
            dbSaveSource[1] = new DbSaveSource(dsImpBudget, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}