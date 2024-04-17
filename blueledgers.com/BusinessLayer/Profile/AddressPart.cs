using System.Data;
using Blue.DAL;

namespace Blue.BL.Profile
{
    public class AddressPart : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public AddressPart()
        {
            SelectCommand = "SELECT * FROM Profile.AddressPart";
            TableName = "AddressPart";
        }

        /// <summary>
        /// </summary>
        /// <param name="addressPartCode"></param>
        /// <param name="dsAddressPart"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetAddressPartList(DataSet dsAddressPart, string connStr)
        {
            // Get and return data
            return DbRetrieve("Profile.GetAddressPartList", dsAddressPart, null, TableName, connStr);
        }


        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetAddressPartList(string connStr)
        {
            var dtAddressPart = new DataTable();


            // Get data
            dtAddressPart = DbRead("Profile.GetAddressPartList", null, connStr);

            if (dtAddressPart != null)
            {
                var drBlank = dtAddressPart.NewRow();
                dtAddressPart.Rows.InsertAt(drBlank, 0);
            }

            // Return result
            return dtAddressPart;
        }

        /// <summary>
        ///     Get contact detail using contact id
        /// </summary>
        /// <param name="addressPartCode"></param>
        /// <returns></returns>
        public DataTable GetAddressPartListByAddressPartCode(string addressPartCode, string connStr)
        {
            var dtAddressPart = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@AddressPartCode", addressPartCode);

            // Get data
            dtAddressPart = DbRead("Profile.GetAddressPartListByAddressPartCode", dbParams, connStr);

            // Return result
            return dtAddressPart;
        }

        /// <summary>
        ///     Get AddressPart Structure
        /// </summary>
        /// <param name="dsAddressPart"></param>
        /// <returns></returns>
        public bool GetAddressPartStructure(DataSet dsAddressPart, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("Profile.GetAddressPartList", dsAddressPart, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get name from address part code
        /// </summary>
        /// <param name="addressPartCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetAddressPartName(string addressPartCode, string connStr)
        {
            var result = string.Empty;
            var dtAdd = new DataTable();

            // Get data
            dtAdd = GetAddressPartListByAddressPartCode(addressPartCode, connStr);

            // Return result
            if (dtAdd != null)
            {
                if (dtAdd.Rows.Count > 0)
                {
                    result = dtAdd.Rows[0]["Name"].ToString();
                }
            }

            return result;
        }

        #endregion
    }
}