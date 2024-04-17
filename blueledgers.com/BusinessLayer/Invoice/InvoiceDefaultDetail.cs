using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.AP
{
    public class InvoiceDefaultDetail : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public InvoiceDefaultDetail()
        {
            SelectCommand = "SELECT * FROM AP.InvoiceDefaultDetail";
            TableName = "InvoiceDefaultDetail";
        }

        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="dsInvoiceDefaultDetail"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceDefaultDetailList(string profileCode, DataSet dsInvoiceDefaultDetail, string connStr)
        {
            var result = false;
            var dtAP = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            result = DbRetrieve("AP.GetInvoiceDefaultDetailListByProfileCode", dsInvoiceDefaultDetail, dbParams,
                TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetInvoiceDefaultDetailListByProfileCode(string profileCode, string connStr)
        {
            var dtAP = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            // Get data
            dtAP = DbRead("AP.GetInvoiceDefaultDetailListByProfileCode", dbParams, connStr);

            // Return result
            return dtAP;
        }

        /// <summary>
        /// </summary>
        /// <param name="seqNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetInvoiceDetailListBySeqNo(int seqNo, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@SeqNo", Convert.ToString(seqNo));


            DbRetrieve("AP.GetInvoiceDetailListBySeqNo", dsTmp, dbParams, TableName, connStr);

            return dsTmp;
        }

        /// <summary>
        /// </summary>
        /// <param name="seqNo"></param>
        /// <param name="dsInvoiceDefaultDetail"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceDetailListBySeqNo(int seqNo, DataSet dsInvoiceDefaultDetail, string connStr)
        {
            var result = false;
            var dtAP = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@SeqNo", Convert.ToString(seqNo));


            // Get data
            result = DbRetrieve("AP.GetInvoiceDetailListBySeqNo", dsInvoiceDefaultDetail, dbParams, TableName, connStr);

            // Return result
            return result;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsInvoiceDefaultDetail"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceDefaultDetailStructure(DataSet dsInvoiceDefaultDetail, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("AP.GetInvoiceDefaultDetailList", dsInvoiceDefaultDetail, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get invoice default max Id
        /// </summary>
        /// <returns></returns>
        public int GetInvoiceDefaultDetailMaxSeq(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("AP.GetInvoiceDefaultDetailMaxSeqNo", null, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}