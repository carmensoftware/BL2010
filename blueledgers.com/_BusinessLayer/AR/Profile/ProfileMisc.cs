using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ProfileMisc : DbHandler
    {
        #region "Attributies"

        public int DebtorCode { get; set; }
        public Guid FieldID { get; set; }
        public string Value { get; set; }

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public ProfileMisc()
        {
            SelectCommand = "SELECT * FROM AR.ProfileMisc";
            TableName = "ProfileMisc";
        }

        /// <summary>
        ///     Get all of debtor Misc.
        /// </summary>
        /// <param name="dsDebtorMisc"></param>
        /// <param name="debtorCode"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsDebtorMisc, string debtorCode, string ConnectionString)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@CustomerCode", debtorCode);

            return DbRetrieve("AR.GetProfileMiscByCustomerCode", dsDebtorMisc, dbParam, TableName, ConnectionString);
        }

        /// <summary>
        ///     Get schema for debtor misc.
        /// </summary>
        /// <param name="dsDebtorMisc"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsDebtorMisc, string ConnectionString)
        {
            return DbRetrieveSchema("AR.GetProfileMiscList", dsDebtorMisc, null, TableName, ConnectionString);
        }

        #endregion
    }
}