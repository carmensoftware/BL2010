using System.Data;
using Blue.DAL;

namespace Blue.BL.Profile
{
    public class ContactType : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ContactType()
        {
            SelectCommand = "SELECT * FROM Profile.ContactType";
            TableName = "ContactType";
        }

        /// <summary>
        /// </summary>
        /// <param name="contactTypeCode"></param>
        /// <param name="dsContactType"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetContactTypeList(string contactTypeCode, DataSet dsContactType, string connStr)
        {
            var result = false;
            var dtContactType = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ContactTypeCode", contactTypeCode);

            // Get data
            result = DbRetrieve("Profile.GetContactTypeListByContactTypeCode", dsContactType, dbParams, TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsContactType"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetContactTypeList(DataSet dsContactType, string connStr)
        {
            var result = false;


            // Get data
            result = DbRetrieve("Profile.GetContactTypeList", dsContactType, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetContactTypeList(string connStr)
        {
            var dtContactType = new DataTable();


            // Get data
            dtContactType = DbRead("Profile.GetContactTypeList", null, connStr);

            // Return result
            return dtContactType;
        }

        /// <summary>
        ///     Get contact detail using contact id
        /// </summary>
        /// <param name="contactTypeCode"></param>
        /// <returns></returns>
        public DataTable GetContactTypeListByContactTypeCode(string contactTypeCode, string connStr)
        {
            var dtContactType = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ContactTypeCode", contactTypeCode);

            // Get data
            dtContactType = DbRead("Profile.GetContactTypeListByContactTypeCode", dbParams, connStr);

            // Return result
            return dtContactType;
        }


        /// <summary>
        ///     Get contacttype name by contactTypeCode
        /// </summary>
        /// <param name="SectionCode"></param>
        /// <returns></returns>
        public string GetContactTypeName(string contactTypeCode, string connStr)
        {
            string contactTypeName;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ContactTypeCode", contactTypeCode);

            DbRetrieve("Profile.GetContactTypeListByContactTypeCode", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                contactTypeName = dsTmp.Tables[TableName].Rows[0]["Name"].ToString();
            }
            else
            {
                contactTypeName = null;
            }
            return contactTypeName;
        }

        /// <summary>
        ///     Get ContactType Structure
        /// </summary>
        /// <param name="dsContactType"></param>
        /// <returns></returns>
        public bool GetContactTypeStructure(DataSet dsContactType, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("Profile.GetContactTypeList", dsContactType, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}