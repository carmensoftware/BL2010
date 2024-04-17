using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class BudgetViewColumn : DbHandler
    {
        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public BudgetViewColumn()
        {
            SelectCommand = "SELECT * FROM GL.BudgetViewColumn";
            TableName = "BudgetViewColumn";
        }

        /// <summary>
        ///     Get column list using budgetViewID
        /// </summary>
        /// <param name="accountViewID"></param>
        /// <returns></returns>
        public bool GetBudgetViewColumnList(int budgetViewID, DataSet budgetViewColumn, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameters
            dbParams[0] = new DbParameter("@BudgetViewID", budgetViewID.ToString());

            // Get Data
            result = DbRetrieve("GL.GetBudgetViewColumnListByBudgetViewID", budgetViewColumn, dbParams, TableName,
                connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Generate column list using datatable
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetBudgetViewColumnPreview(DataTable dtPreview, string connStr)
        {
            var budgetViewColumn = new DataTable();
            var columnList = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Get Data
            budgetViewColumn = dtPreview;

            // Generate Column List
            if (budgetViewColumn != null)
            {
                foreach (DataRow dr in budgetViewColumn.Rows)
                {
                    columnList += (columnList != string.Empty ? "," : string.Empty) + "[" +
                                  field.GetFieldName(dr["FieldID"].ToString(), connStr) + "]" +
                                  " AS [" + dr["FieldID"] + "]";
                }
            }

            return columnList;
        }

        /// <summary>
        ///     Generate column list using budgetViewID
        /// </summary>
        /// <param name="accountViewID"></param>
        /// <returns></returns>
        public static string GetBudgetViewColumn(int budgetViewID, string connStr)
        {
            var budgetViewColumn = new DataTable();
            var dbParams = new DbParameter[1];
            var columnList = string.Empty;

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Create parameters
            dbParams[0] = new DbParameter("@BudgetViewID", budgetViewID.ToString());

            // Get Data
            budgetViewColumn = new DbHandler().DbRead("GL.GetBudgetViewColumnListByBudgetViewID", dbParams, connStr);

            // Generate Column List
            if (budgetViewColumn != null)
            {
                foreach (DataRow dr in budgetViewColumn.Rows)
                {
                    columnList += (columnList != string.Empty ? "," : string.Empty) + "[" +
                                  field.GetFieldName(dr["FieldID"].ToString(), connStr) + "]" +
                                  " AS [" + dr["FieldID"] + "]";
                }
            }

            return columnList;
        }

        /// <summary>
        ///     Get budget view column schema
        /// </summary>
        /// <param name="accountView"></param>
        /// <returns></returns>
        public bool GetBudgetViewColumnSchema(DataSet budgetViewColumn, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetBudgetViewColumnList", budgetViewColumn, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}