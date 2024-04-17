using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class InvoiceDetail : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public InvoiceDetail()
        {
            SelectCommand = "SELECT * FROM AR.InvoiceDetail";
            TableName = "InvoiceDetail";
        }

        /// <summary>
        ///     Get invoice detail list using invoice id
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <param name="dsInvoice"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(string invoiceNo, DataSet dsInvoice, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@InvoiceNo", invoiceNo);

            // Get data
            result = DbRetrieve("AR.GetInvoiceDetailByInvoiceNo", dsInvoice, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get invoice detail using invoice no
        /// </summary>
        /// <param name="InvoiceNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetListByInvoiceDetailNo(string InvoiceNo, string InvoiceDetailNo, string connStr)
        {
            //var result = false;
            var dsInvoice = new DataSet();
            var dbParams = new DbParameter[2];

            // Create parameter
            dbParams[0] = new DbParameter("@InvoiceNo", InvoiceNo);
            dbParams[1] = new DbParameter("@InvoiceDetailNo", InvoiceDetailNo);

            // Get data
            DbRetrieve("AR.GetInvoiceDetailByInvoiceDetailNo", dsInvoice, dbParams, TableName, connStr);

            // Return result
            return dsInvoice;
        }

        /// <summary>
        ///     Get InvoiceDetail
        /// </summary>
        /// <param name="dsInvoiceDetail"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsInvoiceDetail, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("AR.GetInvoiceDetailList", dsInvoiceDetail, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get new id.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewInvoiceDetailID(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("AR.GetInvoiceDetailMaxID", null, connStr);

            // Return result
            return (result == 0 ? 1 : result + 1);
        }

        #endregion
    }
}