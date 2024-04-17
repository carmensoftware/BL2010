using System.Data;
using Blue.DAL;

namespace Blue.BL.Profile
{
    public class AddressType : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public AddressType()
        {
            SelectCommand = "SELECT * FROM Profile.AddressType";
            TableName = "AddressType";
        }

        /// <summary>
        /// </summary>
        /// <param name="addressTypeCode"></param>
        /// <param name="dsAddressType"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetAddressTypeList(DataSet dsAddressType, string connStr)
        {
            return DbRetrieve("Profile.GetAddressTypeList", dsAddressType, null, TableName, connStr);
        }


        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetAddressTypeList(string connStr)
        {
            var dtAddressType = new DataTable();


            // Get data
            dtAddressType = DbRead("Profile.GetAddressTypeList", null, connStr);

            if (dtAddressType != null)
            {
                var drBlank = dtAddressType.NewRow();
                dtAddressType.Rows.InsertAt(drBlank, 0);
            }

            // Return result
            return dtAddressType;
        }


        /// <summary>
        ///     Get contact detail using contact id
        /// </summary>
        /// <param name="addressTypeCode"></param>
        /// <returns></returns>
        public DataTable GetAddressTypeListByAddressTypeCode(string addressTypeCode, string connStr)
        {
            var dtAddressType = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@AddressTypeCode", addressTypeCode);

            // Get data
            dtAddressType = DbRead("Profile.GetAddressTypeListByAddressTypeCode", dbParams, connStr);

            // Return result
            return dtAddressType;
        }

        /// <summary>
        ///     Get AddressTypeName by AddressTypeCode
        /// </summary>
        /// <param name="SectionCode"></param>
        /// <returns></returns>
        public string GetAddressTypeName(string AddressTypeCode, string connStr)
        {
            string addressTypeName;
            var dtTmp = new DataTable();

            dtTmp = GetAddressTypeListByAddressTypeCode(AddressTypeCode, connStr);


            if (dtTmp.Rows.Count > 0)
            {
                addressTypeName = dtTmp.Rows[0]["Name"].ToString();
            }
            else
            {
                addressTypeName = null;
            }

            return addressTypeName;
        }

        /// <summary>
        ///     Get AddressType Structure
        /// </summary>
        /// <param name="dsAddressType"></param>
        /// <returns></returns>
        public bool GetAddressTypeStructure(DataSet dsAddressType, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("Profile.GetAddressTypeList", dsAddressType, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}