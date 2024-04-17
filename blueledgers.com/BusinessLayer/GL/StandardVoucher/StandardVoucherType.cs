using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StandardVoucherType : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public StandardVoucherType()
        {
            SelectCommand = "SELECT * FROM GL.StandardVoucherType";
            TableName = "StandardVoucherType";
        }

        /// <summary>
        ///     Get standard voucher type
        /// </summary>
        /// <param name="typeID"></param>
        public bool GetStandardVoucherType(int typeID, DataSet dsStandardVoucher, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@TypeID", typeID.ToString());

            // Get data
            result = DbRetrieve("GL.GetStandardVoucherType", dsStandardVoucher, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get name of standard voucher
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        public string GetStandardVoucherTypeName(int typeID, string connStr)
        {
            var dsStandardVoucher = new DataSet();
            var result = string.Empty;

            // Get data
            GetStandardVoucherType(typeID, dsStandardVoucher, connStr);

            // Return result
            if (dsStandardVoucher.Tables[TableName].Rows.Count > 0)
            {
                result = dsStandardVoucher.Tables[TableName].Rows[0]["Name"].ToString();
            }

            return result;
        }

        /// <summary>
        ///     Get all standard voucher type
        /// </summary>
        /// <returns></returns>
        public DataTable GetStandardVoucherTypeList(string connStr)
        {
            var dtStandardVoucherType = new DataTable();

            // Get data
            dtStandardVoucherType = DbRead("GL.GetStandardVoucherTypeList", null, connStr);

            // Return result
            return dtStandardVoucherType;
        }

        #endregion
    }
}