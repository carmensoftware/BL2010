using System.Data;
using Blue.DAL;

namespace Blue.BL.Profile
{
    public class ContactCategory : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ContactCategory()
        {
            SelectCommand = "SELECT * FROM Profile.ContactCategory";
            TableName = "ContactCategory";
        }

        /// <summary>
        /// </summary>
        /// <param name="contactCategoryCode"></param>
        /// <param name="dsContactCategory"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetContactCategoryList(string contactCategoryCode, DataSet dsContactCategory, string connStr)
        {
            var result = false;
            var dtContactCategory = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ContactCategoryCode", contactCategoryCode);

            // Get data
            result = DbRetrieve("Profile.GetContactCategoryListByContactCategoryCode", dsContactCategory, dbParams,
                TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetContactCategoryLookup(string connStr)
        {
            var dsContactCategory = new DataSet();

            // Get Data
            DbRetrieve("Profile.GetContactCategoryLookup", dsContactCategory, null, TableName, connStr);


            // Return result
            if (dsContactCategory.Tables[TableName] != null)
            {
                var drBlank = dsContactCategory.Tables[TableName].NewRow();
                dsContactCategory.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            // Return result
            return dsContactCategory.Tables[TableName];
        }


        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetContactCategory(string connStr)
        {
            var dsContactCategory = new DataSet();

            // Get Data
            DbRetrieve("Profile.GetContactCategoryLookup", dsContactCategory, null, TableName, connStr);


            // Return result
            return dsContactCategory.Tables[TableName];
        }


        /// <summary>
        /// </summary>
        /// <param name="dsContactCategory"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetContactCategoryList(DataSet dsContactCategory, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Profile.GetContactCategoryList", dsContactCategory, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get contactCategory detail using contactCategory id
        /// </summary>
        /// <param name="contactCategoryCode"></param>
        /// <returns></returns>
        public DataTable GetContactCategoryListByContactCategoryCode(string contactCategoryCode, string connStr)
        {
            var dtContactCategory = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ContactCategoryCode", contactCategoryCode);

            // Get data
            dtContactCategory = DbRead("Profile.GetContactCategoryListByContactCategoryCode", dbParams, connStr);

            // Return result
            return dtContactCategory;
        }

        /// <summary>
        ///     Get ContactCategoryName by ContactCategoryCode
        /// </summary>
        /// <param name="SectionCode"></param>
        /// <returns></returns>
        public string GetContactCategoryName(string contactCategoryCode, string connStr)
        {
            string contactCategoryName;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ContactCategoryCode", contactCategoryCode);

            DbRetrieve("Profile.GetContactCategoryListByContactCategoryCode", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                contactCategoryName = dsTmp.Tables[TableName].Rows[0]["Name"].ToString();
            }
            else
            {
                contactCategoryName = null;
            }
            return contactCategoryName;
        }

        /// <summary>
        /// </summary>
        /// <param name="Search_Param"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetSearchContactCategoryList(string Search_Param, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Search_Param", Search_Param);

            return DbRead("Profile.GetSearchContactCategoryList", dbParams, connStr);
        }

        /// <summary>
        ///     Get ContactCategory Structure
        /// </summary>
        /// <param name="dsContactCategory"></param>
        /// <returns></returns>
        public bool GetContactCategoryStructure(DataSet dsContactCategory, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("Profile.GetContactCategoryList", dsContactCategory, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}