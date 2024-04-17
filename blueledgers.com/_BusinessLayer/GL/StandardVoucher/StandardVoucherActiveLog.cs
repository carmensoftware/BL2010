using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StandardVoucherActiveLog : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public StandardVoucherActiveLog()
        {
            SelectCommand = "SELECT * FROM GL.StandardVoucherActLog";
            TableName = "StandardVoucherActLog";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsStandardVoucherComment"></param>
        /// <param name="standardvoucherid"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetStandardVoucherActLogListByStandardVoucherID(DataSet dsStandardVoucherActLog,
            int standardvoucherid, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@StandardVoucherID", Convert.ToString(standardvoucherid));

            // Get data
            result = DbRetrieve("GL.GetStandardVoucherActLogListByStandardVoucherID", dsStandardVoucherActLog, dbParams,
                TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get schema for standardvoucher activelog.
        /// </summary>
        /// <param name="dsStandardVoucherMisc"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsStandardVoucherActLog, string ConnectionString)
        {
            return DbRetrieveSchema("GL.GetStandardVoucherActLogList", dsStandardVoucherActLog, null, TableName,
                ConnectionString);
        }

        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsStandardVoucher"></param>
        /// <returns></returns>
        public bool Save(DataSet dsStandardVoucherActLog, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsStandardVoucherActLog, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}