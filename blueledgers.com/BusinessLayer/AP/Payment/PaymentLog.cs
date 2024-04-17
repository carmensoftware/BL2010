using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentLog : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentLog()
        {
            SelectCommand = "SELECT * FROM AP.PaymentLog";
            TableName = "PaymentLog";
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPaymentLog"></param>
        /// <param name="refNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentLogListByRefNo(DataSet dsPaymentLog, string refNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefNo", refNo);

            // Get data
            var result = DbRetrieve("AP.GetPaymentLogListByRefNo", dsPaymentLog, dbParams, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        ///     Get schema for payment activelog.
        /// </summary>
        /// <param name="dsPaymentLog"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsPaymentLog, string connectionString)
        {
            return DbRetrieveSchema("AP.GetPaymentLogList", dsPaymentLog, null, TableName, connectionString);
        }


        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsPaymentLog"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsPaymentLog, string connStr)
        {
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsPaymentLog, SelectCommand, TableName);

            // Save to database
            var result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}