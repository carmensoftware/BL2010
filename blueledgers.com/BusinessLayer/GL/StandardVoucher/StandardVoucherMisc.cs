using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StandardVoucherMisc : DbHandler
    {
        #region "Attributies"

        public int StandardVoucherID { get; set; }

        public Guid FieldID { get; set; }

        public string Value { get; set; }

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public StandardVoucherMisc()
        {
            SelectCommand = "SELECT * FROM GL.StandardVoucherMisc";
            TableName = "StandardVoucherMisc";
        }

        /// <summary>
        ///     Get all of standardVoucher Misc.
        /// </summary>
        /// <param name="dsStandardVoucherMisc"></param>
        /// <param name="StandardVoucherID"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsStandardVoucherMisc, int StandardVoucherID, string ConnectionString)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@StandardVoucherID", Convert.ToString(StandardVoucherID));

            return DbRetrieve("GL.GetStandardVoucherMiscListByStandardVoucherID", dsStandardVoucherMisc, dbParam,
                TableName, ConnectionString);
        }

        /// <summary>
        ///     Get schema for standardvoucher misc.
        /// </summary>
        /// <param name="dsStandardVoucherMisc"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsStandardVoucherMisc, string ConnectionString)
        {
            return DbRetrieveSchema("GL.GetStandardVoucherMiscList", dsStandardVoucherMisc, null, TableName,
                ConnectionString);
        }

        #endregion
    }
}