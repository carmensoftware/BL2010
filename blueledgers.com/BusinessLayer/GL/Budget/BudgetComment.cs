using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class BudGetComment : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public BudGetComment()
        {
            SelectCommand = "SELECT * FROM GL.BudGetComment";
            TableName = "BudGetComment";
        }


        /// <summary>
        ///     Get all of budget except recurring type
        /// </summary>
        /// <param name="dsBudGet"></param>
        /// <returns></returns>
        public bool GetBudGetCommentListByBudGetID(DataSet dsBudGetComment, int budgetID, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BudgetID", Convert.ToString(budgetID));

            // Get data
            result = DbRetrieve("GL.GetBudGetCommentListByBudgetID", dsBudGetComment, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get schema for standardvoucher comment.
        /// </summary>
        /// <param name="dsBudGetMisc"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsBudGetComment, string ConnectionString)
        {
            return DbRetrieveSchema("GL.GetBudGetCommentList", dsBudGetComment, null, TableName, ConnectionString);
        }


        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsBudGet"></param>
        /// <returns></returns>
        public bool Save(DataSet dsBudGetComment, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsBudGetComment, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}