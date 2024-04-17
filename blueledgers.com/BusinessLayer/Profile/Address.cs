using System.Data;
using Blue.DAL;

namespace Blue.BL.Profile
{
    public class Address : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Address()
        {
            SelectCommand = "SELECT * FROM Profile.Address";
            TableName = "Address";
        }

        /// <summary>
        ///     Get all data by profile code.
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="dsAddress"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetAddressList(string profileCode, DataSet dsAddress, string connStr)
        {
            var result = false;
            var dtAddress = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            result = DbRetrieve("Profile.GetAddressListByProfileCode", dsAddress, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="dsAddress"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetAddressListDescByProfileCode(string profileCode, DataSet dsAddress, string connStr)
        {
            var result = false;
            var dtAddress = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            result = DbRetrieve("Profile.GetAddressListDescByProfileCode", dsAddress, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsAddress"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetAddressList(DataSet dsAddress, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Profile.GetAddressList", dsAddress, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get Address using profile code.
        /// </summary>
        /// <param name="profileCode"></param>
        /// <returns></returns>
        public bool GetAddressListByProfileCode(string profileCode, DataSet dsAddress, string connStr)
        {
            //bool result = false;

            // Return result
            return GetAddressList(profileCode, dsAddress, connStr);
        }

        /// <summary>
        ///     Get Address using profile code and address id.
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="addressID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetAddressListByProfileCodeAndID(string profileCode, string addressID, string connStr)
        {
            var dsAddress = new DataSet();
            var dbParams = new DbParameter[2];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);
            dbParams[1] = new DbParameter("@AddressID", addressID);

            // Get data
            DbRetrieve("Profile.GetAddressListByProfileCodeAndID", dsAddress, dbParams, TableName, connStr);

            // Return result
            return dsAddress;
        }

        /// <summary>
        ///     Get Address name by profilecode and addressPartName
        /// </summary>
        /// <param name="SectionCode"></param>
        /// <returns></returns>
        public string GetAddressName(string profileCode, string addressPartName, string connStr)
        {
            string AddressName;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);
            dbParams[1] = new DbParameter("@AddressPartName", addressPartName);

            DbRetrieve("Profile.GetAddressListByProfileCodeAndAddressPartName", dsTmp, dbParams, TableName, connStr);


            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                AddressName = dsTmp.Tables[TableName].Rows[0]["Name"].ToString();
            }
            else
            {
                AddressName = null;
            }
            return AddressName;
        }

        /// <summary>
        ///     Get Address Structure
        /// </summary>
        /// <param name="dsAddress"></param>
        /// <returns></returns>
        public bool GetAddressStructure(DataSet dsAddress, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("Profile.GetAddressList", dsAddress, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get new address id
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewAddressID(string profileCode, string connStr)
        {
            var result = 0;

            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            DbRetrieve("[Profile].GetAddressID", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                result = int.Parse(dsTmp.Tables[TableName].Rows[0]["AddressID"].ToString());
            }
            else
            {
                result = 1;
            }

            // Return result
            return result;
        }

        /// <summary>
        ///     Save address.
        /// </summary>
        /// <param name="dsAddress"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsAddress, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsAddress, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}