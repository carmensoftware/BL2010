using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class AccountViewColumn : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public AccountViewColumn()
        {
            this.SelectCommand = "SELECT * FROM Reference.AccountViewColumn";
            this.TableName = "AccountViewColumn";
        }

        /// <summary>
        ///     Get column list using accountViewID
        /// </summary>
        /// <param name="accountViewID"></param>
        /// <returns></returns>
        public bool GetAccountViewColumnList(int accountViewID, DataSet accountViewColumn, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameters
            dbParams[0] = new DbParameter("@AccountViewID", accountViewID.ToString());

            // Get Data
            result = DbRetrieve("Reference.GetAccountViewColumnListByAccountViewID", accountViewColumn, dbParams,
                this.TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Generate column list using accountViewID
        /// </summary>
        /// <param name="accountViewID"></param>
        /// <returns></returns>
        public static string GetAccountViewColumn(int accountViewID, string connStr)
        {
            var accountViewColumn = new DataTable();
            var dbParams = new DbParameter[1];
            var columnList = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Create parameters
            dbParams[0] = new DbParameter("@AccountViewID", accountViewID.ToString());

            // Get Data
            accountViewColumn = new DbHandler().DbRead("Reference.GetAccountViewColumnListByAccountViewID", dbParams,
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
        ///     Get account view column schema
        /// </summary>
        /// <param name="accountView"></param>
        /// <returns></returns>
        public bool GetAccountViewColumnSchema(DataSet accountViewColumn, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("Reference.GetAccountViewColumnList", accountViewColumn, null, this.TableName,
                connStr);

            // Return result
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

            var field = new Blue.BL.Application.Field();
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