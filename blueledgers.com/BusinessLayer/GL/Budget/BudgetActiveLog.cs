using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class BudGetActiveLog : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public BudGetActiveLog()
        {
            SelectCommand = "SELECT * FROM GL.BudGetActLog";
            TableName = "BudGetActLog";
        }


        /// <summary>
        ///     Get all of budget except recurring type
        /// </summary>
        /// <param name="dsBudGet"></param>
        /// <returns></returns>
        public bool GetBudGetActLogListByBudgetID(DataSet dsBudGetActLog, int budgetID, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BudgetID", Convert.ToString(budgetID));

            // Get data
            result = DbRetrieve("GL.GetBudGetActLogListByBudgetID", dsBudGetActLog, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get schema for budget activelog.
        /// </summary>
        /// <param name="dsBudGetMisc"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsBudGetActLog, string ConnectionString)
        {
            return DbRetrieveSchema("GL.GetBudGetActLogList", dsBudGetActLog, null, TableName, ConnectionString);
        }


        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsBudGet"></param>
        /// <returns></returns>
        public bool Save(DataSet dsBudGetActLog, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsBudGetActLog, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}