using System.Data;
using Blue.DAL;

namespace Blue.BL.GL.AccountListing
{
    public class AccountDetailViewColumn : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        public AccountDetailViewColumn()
        {
            SelectCommand = "SELECT * FROM GL.AccountDetailViewColumn";
            TableName = "AccountDetailViewColumn";
        }

        /// <summary>
        ///     This function is used for get table structure of table GL.AccountDetailViewColumn
        ///     by using stored procedure "GL.GetAccountDetailViewColumnList".
        /// </summary>
        /// <param name="dsAccDtView"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsAccDtView, string connStr)
        {
            // Declare variable
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetAccountDetailViewColumnList", dsAccDtView, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     This function is used for get all GL.AccountDetailViewColumn data that related
        ///     to specified view id by using stored procedure "GL.GetAccountDetailViewColumn_ViewID".
        /// </summary>
        /// <param name="dsAccDtView"></param>
        /// <param name="viewID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAccDtView, int viewID, string connStr)
        {
            // Declare variable.
            var result = false;

            // Create parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewID", viewID.ToString());

            // Get data
            result = DbRetrieve("GL.GetAccountDetailViewColumnList_ViewID", dsAccDtView, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Generate column list using datatable
        /// </summary>
        /// <param name="dtPreview"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetColumnQuery(DataTable dtPreview, string connStr)
        {
            //DataTable dtviewColumn        = new DataTable();
            var columnList = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate Column List
            if (dtPreview != null)
            {
                foreach (DataRow dr in dtPreview.Rows)
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