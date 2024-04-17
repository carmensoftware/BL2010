using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ProfileDefaultWHT : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ProfileDefaultWHT()
        {
            SelectCommand = "SELECT * FROM AR.ProfileDefaultWHT";
            TableName = "ProfileDefaultWHT";
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="dsVendorDefaultWHT"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(string profileCode, DataSet dsDebtor, string connStr)
        {
            var result = false;
            var dtAR = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            result = DbRetrieve("AR.GetProfileDefaultWHTByProfileCode", dsDebtor, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetList(string profileCode, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);


            DbRetrieve("AR.GetProfileDefaultWHTByProfileCode", dsTmp, dbParams, TableName, connStr);

            return dsTmp;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsDebtor"></param>
        /// <param name="debtorCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetByCustomerCode(DataSet dsDebtor, string debtorCode, string connStr)
        {
            var result = false;
            // Create parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CustomerCode", debtorCode);

            // Get data
            result = DbRetrieve("AR.GetProfileDefaultWHTByCustomerCode", dsDebtor, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetByProfileCode(string profileCode, string connStr)
        {
            var dtAR = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            dtAR = DbRead("AR.GetProfileDefaultWHTByProfileCode", dbParams, connStr);

            // Return result
            return dtAR;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsVendorDefaultWHT"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsDebtor, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("AR.GetProfileDefaultWHTList", dsDebtor, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}