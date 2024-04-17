using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class TaxInvoiceStatus : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public TaxInvoiceStatus()
        {
            SelectCommand = "SELECT * FROM AP.TaxInvoiceStatus";
            TableName = "TaxInvoiceStatus";
        }


        public bool GetTaxInvoiceStatusList(string taxInvoiceStatusCode, DataSet dsTaxInvoiceStatus, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@TaxInvoiceStatusCode", taxInvoiceStatusCode);

            // Get data
            var result = DbRetrieve("AP.GetTaxInvoiceStatusListByTaxInvoiceStatusCode", dsTaxInvoiceStatus, dbParams,
                TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get invoice detail using invoice id
        /// </summary>
        /// <param name="taxInvoiceStatusCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetTaxInvoiceStatusListByTaxInvoiceStatusCode(string taxInvoiceStatusCode, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@TaxInvoiceStatusCode", taxInvoiceStatusCode);

            // Get data
            var dtTaxInvoiceStatus = DbRead("AP.GetTaxInvoiceStatusListByTaxInvoiceStatusCode", dbParams, connStr);

            // Return result
            return dtTaxInvoiceStatus;
        }


        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetTaxInvoiceStatusLookup(string connStr)
        {
            // Get data
            var dtInvoiceStatus = DbRead("AP.GetTaxInvoiceStatusList", null, connStr);

            // Return result
            return dtInvoiceStatus;
        }


        /// <summary>
        /// </summary>
        /// <param name="taxInvoiceStatusCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetTaxInvoiceStatusName(string taxInvoiceStatusCode, string connStr)
        {
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@TaxInvoiceStatusCode", taxInvoiceStatusCode);

            DbRetrieve("AP.GetTaxInvoiceStatusListByTaxInvoiceStatusCode", dsTmp, dbParams, TableName, connStr);

            var statusName = dsTmp.Tables[TableName].Rows.Count > 0
                ? dsTmp.Tables[TableName].Rows[0]["Name"].ToString()
                : null;
            return statusName;
        }

        /// <summary>
        ///     Get TaxInvoiceStatus
        /// </summary>
        /// <param name="dsTaxInvoiceStatus"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetTaxInvoiceStatusStructure(DataSet dsTaxInvoiceStatus, string connStr)
        {
            // Get structure
            var result = DbRetrieveSchema("AP.GetTaxInvoiceStatusList", dsTaxInvoiceStatus, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}