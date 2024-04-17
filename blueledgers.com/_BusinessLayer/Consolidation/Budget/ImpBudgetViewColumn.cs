using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.Budget
{
    public class ImpBudgetViewColumn : DbHandler
    {
        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public ImpBudgetViewColumn()
        {
            SelectCommand = "SELECT * FROM Budget.ImpBudgetViewColumn";
            TableName = "ImpBudgetViewColumn";
        }

        /// <summary>
        ///     Get column list using ImpBudgetViewID
        /// </summary>
        /// <param name="accountViewID"></param>
        /// <returns></returns>
        public bool GetImpBudgetViewColumnList(int impBudgetViewID, DataSet impBudgetViewColumn, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameters
            dbParams[0] = new DbParameter("@impBudgetViewID", impBudgetViewID.ToString());

            // Get Data
            result = DbRetrieve("Budget.GetImpBudgetViewColumnListByImpBudgetViewID", impBudgetViewColumn, dbParams,
                TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Generate column list using budgetViewID
        /// </summary>
        /// <param name="accountViewID"></param>
        /// <returns></returns>
        public static string GetImpBudgetViewColumn(int impBudgetViewID, string connStr)
        {
            var impBudgetViewColumn = new DataTable();
            var dbParams = new DbParameter[1];
            var columnList = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Create parameters
            dbParams[0] = new DbParameter("@impBudgetViewID", impBudgetViewID.ToString());

            // Get Data
            impBudgetViewColumn = new DbHandler().DbRead("Budget.GetImpBudgetViewColumnListByImpBudgetViewID", dbParams,
                connStr);

            // Generate Column List
            if (impBudgetViewColumn != null)
            {
                foreach (DataRow dr in impBudgetViewColumn.Rows)
                {
                    columnList += (columnList != string.Empty ? "," : string.Empty) + "[" +
                                  field.GetFieldName(dr["FieldID"].ToString(), connStr) + "]" +
                                  " AS [" + dr["FieldID"] + "]";
                }
            }

            return columnList;
        }

        /// <summary>
        ///     Get impbudget view column schema
        /// </summary>
        /// <param name="accountView"></param>
        /// <returns></returns>
        public bool GetImpBudgetViewColumnSchema(DataSet impBudgetViewColumn, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("Budget.GetImpBudgetViewColumnList", impBudgetViewColumn, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Generate column list using datatable
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetImpBudgetViewColumnPreview(DataTable dtPreview, string connStr)
        {
            var impBudgetViewColumn = new DataTable();
            var columnList = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Get Data
            impBudgetViewColumn = dtPreview;

            // Generate Column List
            if (impBudgetViewColumn != null)
            {
                foreach (DataRow dr in impBudgetViewColumn.Rows)
                {
                    columnList += (columnList != string.Empty ? "," : string.Empty) + "[" +
                                  field.GetFieldName(dr["FieldID"].ToString(), connStr) + "]" +
                                  " AS [" + dr["FieldID"] + "]";
                }
            }

            return columnList;
        }

        #endregion
    }
}