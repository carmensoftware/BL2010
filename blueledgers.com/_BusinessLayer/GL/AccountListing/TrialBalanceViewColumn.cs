using System.Data;
using Blue.DAL;

namespace Blue.BL.GL.AccountListing
{
    public class TrialBalanceViewColumn : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        public TrialBalanceViewColumn()
        {
            SelectCommand = "SELECT * FROM GL.TrialBalanceViewColumn";
            TableName = "TrialBalanceViewColumn";
        }

        /// <summary>
        ///     This function is used for get all GL.TrialBalanceViewColumn data that
        ///     related to specified view id by using strored procedure
        ///     "GL.GetTrialBalanceViewColumnList_ViewID"
        /// </summary>
        /// <param name="dsTriBalList"></param>
        /// <param name="viewID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsTriBalList, int viewID, string connStr)
        {
            // declare varialbe.
            var result = false;

            var dbParams = new DbParameter[1];


            // Create parameter
            dbParams[0] = new DbParameter("@ViewID", viewID.ToString());

            // Get data
            result = DbRetrieve("GL.GetTrialBalanceViewColumnList_ViewID", dsTriBalList, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     This function user for get table schema of trial balance view column data
        ///     by using stored procedure "GL.GetTrialBalanceViewColumnList".
        /// </summary>
        /// <param name="dsTrialBalView"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsTrialBalView, string connStr)
        {
            // declare varialbe.
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetTrialBalanceViewColumnList", dsTrialBalView, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Generate column list using datatable
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetTrialBalanceViewColumnPreview(DataTable dtPreview, string connStr)
        {
            var trialBalanceViewColumn = new DataTable();
            var columnList = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Get Data
            trialBalanceViewColumn = dtPreview;

            // Generate Column List
            if (trialBalanceViewColumn != null)
            {
                foreach (DataRow dr in trialBalanceViewColumn.Rows)
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