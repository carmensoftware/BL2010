using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation
{
    public class AddressType : DbHandler
    {
        #region "Attibuties"

        public string AddressTypeID{get;set;}

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int UpdatedBy { get; set; }

        #endregion

        #region "Operations"

        public AddressType()
        {
            SelectCommand = "SELECT * FROM Reference.AddressType";
            TableName = "AddressType";
        }

        /// <summary>
        ///     Get name city
        /// </summary>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        public string GetName(int addressTypeID, string connStr)
        {
            var result = string.Empty;
            var dsAddressType = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AddressTypeID", Convert.ToString(addressTypeID));

            DbRetrieve("Reference.GetAddressType", dsAddressType, dbParams, TableName, connStr);

            if (dsAddressType.Tables[TableName] != null)
            {
                if (dsAddressType.Tables[TableName].Rows.Count > 0)
                {
                    result = dsAddressType.Tables[TableName].Rows[0]["Name"].ToString();
                }
            }
            return result;
        }

        /// <summary>
        ///     Get all address type.
        /// </summary>
        /// <param name="dsAddressType"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetAddressTypeList(DataSet dsAddressType, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Reference.GetAddressTypeList", dsAddressType, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get address type return datatable.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetAddressType(string connStr)
        {
            var dtAddParameter = new DataTable();

            // Get data
            dtAddParameter = DbRead("Reference.GetAddressTypeList", null, connStr);

            // Return result
            return dtAddParameter;
        }

        /// <summary>
        ///     Get address type for dropdownlist.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetAddressTypeForDropDownList(string connStr)
        {
            var dsAddressType = new DataSet();

            // Get data
            GetAddressTypeList(dsAddressType, connStr);

            // Return result
            if (dsAddressType.Tables[TableName] != null)
            {
                var drBlank = dsAddressType.Tables[TableName].NewRow();
                dsAddressType.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsAddressType.Tables[TableName];
        }

        #endregion
    }
}