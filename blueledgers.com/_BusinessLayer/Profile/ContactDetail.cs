using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Profile
{
    public class ContactDetail : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ContactDetail()
        {
            SelectCommand = "SELECT * FROM Profile.ContactDetail";
            TableName = "ContactDetail";
        }

        /// <summary>
        /// </summary>
        /// <param name="contactID"></param>
        /// <param name="dsContactDetail"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetContactDetailList(int contactID, DataSet dsContactDetail, string connStr)
        {
            var result = false;
            var dtContactDetail = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ContactID", Convert.ToString(contactID));

            // Get data
            result = DbRetrieve("Profile.GetContactDetailListByContactID", dsContactDetail, dbParams, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetContactDetailLookup(string connStr)
        {
            var dsContactDetail = new DataSet();

            // Get Data
            DbRetrieve("Profile.GetContactDetailList", dsContactDetail, null, TableName, connStr);


            // Return result
            if (dsContactDetail.Tables[TableName] != null)
            {
                var drBlank = dsContactDetail.Tables[TableName].NewRow();
                dsContactDetail.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            // Return result
            return dsContactDetail.Tables[TableName];
        }


        /// <summary>
        /// </summary>
        /// <param name="dsContactDetail"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetContactDetailList(DataSet dsContactDetail, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Profile.GetContactDetailList", dsContactDetail, null, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        ///     Get contact detail using contact id
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns></returns>
        public DataTable GetContactDetailListByContactID(int contactID, string connStr)
        {
            var dtContactDetail = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ContactID", Convert.ToString(contactID));

            // Get data
            dtContactDetail = DbRead("Profile.GetContactDetailListByContactID", dbParams, connStr);

            // Return result
            return dtContactDetail;
        }

        /// <summary>
        /// </summary>
        /// <param name="contactID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetContactDetailList(int contactID, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ContactID", Convert.ToString(contactID));


            DbRetrieve("Profile.GetContactDetailListByContactID", dsTmp, dbParams, TableName, connStr);

            return dsTmp;
        }

        /// <summary>
        /// </summary>
        /// <param name="Search_Param"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetSearchContactDetailList(string Search_Param, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Search_Param", Search_Param);

            return DbRead("Profile.GetSearchContactDetailList", dbParams, connStr);
        }


        /// <summary>
        ///     Get ContactDetail Structure
        /// </summary>
        /// <param name="dsContactDetail"></param>
        /// <returns></returns>
        public bool GetContactDetailStructure(DataSet dsContactDetail, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("Profile.GetContactDetailList", dsContactDetail, null, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        ///     Get ContactDetail max Id
        /// </summary>
        /// <returns></returns>
        public int GetContactDetailMaxID(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("Profile.GetContactDetailMaxID", null, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}