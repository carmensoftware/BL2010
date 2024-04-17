using System.Data;
using Blue.DAL;

namespace Blue.BL.AP
{
    public class InvoiceDefault : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public InvoiceDefault()
        {
            SelectCommand = "SELECT * FROM AP.InvoiceDefault";
            TableName = "InvoiceDefault";
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="dsInvoiceDefault"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceDefaultList(string profileCode, DataSet dsInvoiceDefault, string connStr)
        {
            var result = false;
            var dtAP = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            result = DbRetrieve("AP.GetInvoiceDefaultListByProfileCode", dsInvoiceDefault, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetInvoiceDefaultListByProfileCode(string profileCode, string connStr)
        {
            var dtAP = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            dtAP = DbRead("AP.GetInvoiceDefaultListByProfileCode", dbParams, connStr);

            // Return result
            return dtAP;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsInvoiceDefault"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceDefaultStructure(DataSet dsInvoiceDefault, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("AP.GetInvoiceDefaultList", dsInvoiceDefault, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get invoice default  max Id
        /// </summary>
        /// <returns></returns>
        public int GetInvoiceDefaultMaxSeq(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("AP.GetInvoiceDefaultMaxSeq", null, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}