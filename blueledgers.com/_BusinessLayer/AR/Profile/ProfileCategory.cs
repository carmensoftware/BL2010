using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ProfileCategory : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ProfileCategory()
        {
            SelectCommand = "SELECT * FROM AP.ProfileCategory";
            TableName = "ProfileCategory";
        }

        /// <summary>
        ///     Get data from table profile category by profile category code.
        /// </summary>
        /// <param name="dsprofileCategory"></param>
        /// <param name="profileCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetByCategoryCode(DataSet dsDebtorCategory, string debtorCode, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CategoryCode", debtorCode);

            // Get data
            result = DbRetrieve("AR.GetProfileCategorytByCategoryCode", dsDebtorCategory, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get schema for debtor category.
        /// </summary>
        /// <param name="dsDebtorCategory"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsDebtorCategory, string ConnectionString)
        {
            return DbRetrieveSchema("AP.GetProfileCategoryList", dsDebtorCategory, null, TableName, ConnectionString);
        }

        /// <summary>
        ///     Get debtor category name by debtor category code
        /// </summary>
        /// <param name="debtorCategoryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string debtorCategoryCode, string connStr)
        {
            string debtorCategoryName;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CategoryCode", debtorCategoryCode);

            DbRetrieve("AR.GetProfileCategoryByCategoryCode", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                debtorCategoryName = dsTmp.Tables[TableName].Rows[0]["Name"].ToString();
            }
            else
            {
                debtorCategoryName = null;
            }
            return debtorCategoryName;
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetLookup(string connStr)
        {
            var dsCategory = new DataSet();

            // Get Data
            DbRetrieve("AR.GetProfileCategoryLookup", dsCategory, null, TableName, connStr);


            // Return result
            if (dsCategory.Tables[TableName] != null)
            {
                var drBlank = dsCategory.Tables[TableName].NewRow();
                dsCategory.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            // Return result
            return dsCategory.Tables[TableName];
        }

        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsDebtorCategory"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsDebtorCategory, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsDebtorCategory, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}