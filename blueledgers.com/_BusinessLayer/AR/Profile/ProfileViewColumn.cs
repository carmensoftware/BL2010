using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ProfileViewColumn : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public ProfileViewColumn()
        {
            SelectCommand = "SELECT * FROM AR.ProfileViewColumn";
            TableName = "ProfileViewColumn";
        }

        /// <summary>
        ///     Get column list using debtorViewID
        /// </summary>
        /// <param name="vendorViewID"></param>
        /// <returns></returns>
        public bool GetList(int debtorViewID, DataSet debtorViewColumn, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameters
            dbParams[0] = new DbParameter("@ProfileViewID", debtorViewID.ToString());

            // Get Data
            result = DbRetrieve("AR.GetProfileViewColumnByProfileViewID", debtorViewColumn, dbParams, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Generate column list using debtorViewID
        /// </summary>
        /// <param name="vendorViewID"></param>
        /// <returns></returns>
        public static string GetColumn(int debtorViewID, string connStr)
        {
            var debtorViewColumn = new DataTable();
            var dbParams = new DbParameter[1];
            var columnList = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Create parameters
            dbParams[0] = new DbParameter("@ProfileViewID", debtorViewID.ToString());

            // Get Data
            debtorViewColumn = new DbHandler().DbRead("AR.GetProfileViewColumnByProfileViewID", dbParams, connStr);

            // Generate Column List
            if (debtorViewColumn != null)
            {
                foreach (DataRow dr in debtorViewColumn.Rows)
                {
                    columnList += (columnList != string.Empty ? "," : string.Empty) + "[" +
                                  field.GetFieldName(dr["FieldID"].ToString(), connStr) + "]" +
                                  " AS [" + dr["FieldID"] + "]";
                }
            }

            return columnList;
        }

        /// <summary>
        ///     Generate column list using datatable
        /// </summary>
        /// <param name="dtPreview"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetPreview(DataTable dtPreview, string connStr)
        {
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            var debtorViewColumn = new DataTable();
            var columnList = string.Empty;

            // Get Data
            debtorViewColumn = dtPreview;

            // Generate Column List
            if (debtorViewColumn != null)
            {
                foreach (DataRow dr in debtorViewColumn.Rows)
                {
                    columnList += (columnList != string.Empty ? "," : string.Empty) + "[" +
                                  field.GetFieldName(dr["FieldID"].ToString(), connStr) + "]" +
                                  " AS [" + dr["FieldID"] + "]";
                }
            }

            return columnList;
        }

        /// <summary>
        ///     Get debtor view column schema
        /// </summary>
        /// <param name="debtorView"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet debtorView, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("AR.GetProfileViewColumnList", debtorView, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}