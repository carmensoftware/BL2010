using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class InvoiceLog : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public InvoiceLog()
        {
            SelectCommand = "SELECT * FROM AP.InvoiceLog";
            TableName = "InvoiceLog";
        }


        /// <summary>
        /// </summary>
        /// <param name="dsInvoiceLog"></param>
        /// <param name="refNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceLogListByRefNo(DataSet dsInvoiceLog, string refNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefNo", refNo);

            // Get data
            var result = DbRetrieve("AP.GetInvoiceLogListByRefNo", dsInvoiceLog, dbParams, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        ///     Get schema for invoice activelog.
        /// </summary>
        /// <param name="dsInvoiceLog"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsInvoiceLog, string connectionString)
        {
            return DbRetrieveSchema("AP.GetInvoiceLogList", dsInvoiceLog, null, TableName, connectionString);
        }


        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsInvoiceLog"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsInvoiceLog, string connStr)
        {
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsInvoiceLog, SelectCommand, TableName);

            // Save to database
            var result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}