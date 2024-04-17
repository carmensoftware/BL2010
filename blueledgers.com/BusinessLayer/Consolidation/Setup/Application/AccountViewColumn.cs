using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.Setup.Application
{
    public class AccountViewColumn : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        public AccountViewColumn()
        {
            SelectCommand = "SELECT * FROM [Application].AccountViewColumn";
            TableName = "AccountViewColumn";
        }

        public int AccountViewColumnID { get; set; }

        public int AccountViewID { get; set; }

        public int SeqNo { get; set; }

        /// <summary>
        ///     Generate column list using accountViewID
        /// </summary>
        /// <param name="accountViewID"></param>
        /// <returns></returns>
        public static string GetAccountViewColumn(int AccountViewID, string connStr)
        {
            var accountViewColumn = new DataTable();
            var dbParams = new DbParameter[1];
            var columnList = string.Empty;
            var field = new BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Create parameters
            dbParams[0] = new DbParameter("@accountViewID", AccountViewID.ToString());

            // Get Data
            accountViewColumn = new DbHandler().DbRead("[Application].GetAccountViewColumnListByAccountViewID", dbParams,
                connStr);

            // Generate Column List
            if (accountViewColumn != null)
            {
                foreach (DataRow dr in accountViewColumn.Rows)
                {
                    columnList += (columnList != string.Empty ? "," : string.Empty) + "[" +
                                  field.GetFieldName(dr["FieldID"].ToString(), connStr) + "]" +
                                  " AS [" + dr["FieldID"] + "]";
                }
            }

            return columnList;
        }

        /// <summary>
        ///     Get Account view column schema
        /// </summary>
        /// <param name="AccountViewColumn"></param>
        /// <returns></returns>
        public bool GetAccountViewColumnSchema(DataSet AccountViewColumn, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("[Application].GetAccountViewColumnList", AccountViewColumn, null, TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get column list using AccountViewID
        /// </summary>
        /// <param name="accountViewID"></param>
        /// <returns></returns>
        public bool GetAccountViewColumnList(int AccountViewID, DataSet AccountViewColumn, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameters
            dbParams[0] = new DbParameter("@accountViewID", AccountViewID.ToString());

            // Get Data
            result = DbRetrieve("[Application].GetAccountViewColumnListByAccountViewID", AccountViewColumn, dbParams,
                TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Generate column list using datatable
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetAccountViewColumnPreview(DataTable dtPreview, string connStr)
        {
            var accountViewColumn = new DataTable();
            var columnList = string.Empty;
            var field = new BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Get Data
            accountViewColumn = dtPreview;

            // Generate Column List
            if (accountViewColumn != null)
            {
                foreach (DataRow dr in accountViewColumn.Rows)
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