using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class AccountReconcileViewColumn : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        public AccountReconcileViewColumn()
        {
            SelectCommand = "SELECT * FROM GL.AccountReconcileViewColumn";
            TableName = "AccountReconcileViewColumn";
        }


        /// <summary>
        ///     Get accountreconcile view column schema
        /// </summary>
        /// <param name="AccountReconcileViewColumn"></param>
        /// <returns></returns>
        public bool GetAccountReconcileViewColumnSchema(DataSet dsAccountReconcileViewColumn, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetAccountReconcileViewColumnList", dsAccountReconcileViewColumn, null,
                TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get column list using accRecViewID
        /// </summary>
        /// <param name="accountViewID"></param>
        /// <returns></returns>
        public bool GetAccountReconcileViewColumnList(int accRecViewID, DataSet dsAccountReconcileViewColumn,
            string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameters
            dbParams[0] = new DbParameter("@accRecViewID", accRecViewID.ToString());

            // Get Data
            result = DbRetrieve("GL.GetAccountReconcileViewColumnListByaccRecViewID", dsAccountReconcileViewColumn,
                dbParams, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get column list
        /// </summary>
        /// <param name="dtPreview"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetAccountReconcileViewColumnPreview(DataTable dtPreview, string connStr)
        {
            var AccountReconcileViewColumn = new DataTable();
            var columnList = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Get Data
            AccountReconcileViewColumn = dtPreview;

            // Generate Column List
            if (AccountReconcileViewColumn != null)
            {
                foreach (DataRow dr in AccountReconcileViewColumn.Rows)
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