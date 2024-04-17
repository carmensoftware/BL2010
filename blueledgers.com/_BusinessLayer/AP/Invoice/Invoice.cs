using System.Data;
using System.Globalization;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class Invoice : DbHandler
    {
        #region "Attributies"

        private readonly InvoiceDetail _invoiceDetail = new InvoiceDetail();
        private readonly InvoiceMisc _invoiceMisc = new InvoiceMisc();
        private readonly TaxInvoice _taxInvoice = new TaxInvoice();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Invoice()
        {
            SelectCommand = "SELECT * FROM AP.Invoice";
            TableName = "Invoice";
        }

        /// <summary>
        ///     Get invoice using id
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetInvoice(int voucherNo, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo.ToString(CultureInfo.InvariantCulture));

            // Get data
            var dtInvoice = DbRead("AP.GetInvoice", dbParams, connStr);

            // Return result
            return dtInvoice;
        }

        /// <summary>
        ///     Get invoice using voucherNo
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="dsInvoice"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoice(string voucherNo, DataSet dsInvoice, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            var result = DbRetrieve("AP.GetInvoice", dsInvoice, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get invoice name by profileCode
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetInvoiceName(string profileCode, string connStr)
        {
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            DbRetrieve("AP.GetInvoiceNameByProfileCode", dsTmp, dbParams, TableName, connStr);

            var invoiceName = dsTmp.Tables[TableName].Rows.Count > 0
                ? dsTmp.Tables[TableName].Rows[0]["InvoiceName"].ToString()
                : null;
            return invoiceName;
        }

        /// <summary>
        ///     return datatable and get searching match data.
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetInvoiceListSearch(string keyWord, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@KeyWord", keyWord);

            return DbRead("AP.GetInvoiceListSearch", dbParams, connStr);
        }

        /// <summary>
        ///     Get invoice using account view id
        /// </summary>
        public void GetInvoiceList(DataSet dsInvoice, int invoiceViewID, int userID, string connStr)
        {
            DataTable dtInvoice;
            var invoice = new Invoice();
            var field = new Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate query
            var invoiceViewQuery = new InvoiceView().GetInvoiceViewQuery(invoiceViewID, userID, connStr);

            // Generate parameter
            var dtInvoiceViewCriteria = new InvoiceViewCriteria().GetInvoiceViewCriteriaList(invoiceViewID, connStr);

            if (dtInvoiceViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtInvoiceViewCriteria.Rows.Count];

                for (var i = 0; i < dtInvoiceViewCriteria.Rows.Count; i++)
                {
                    var dr = dtInvoiceViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter(string.Format("@{0}{1}", dr["SeqNo"], field.GetFieldName(dr["FieldID"].ToString(), connStr)),
                            dr["Value"].ToString());
                }

                // Get data                
                dtInvoice = invoice.DbExecuteQuery(invoiceViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtInvoice = invoice.DbExecuteQuery(invoiceViewQuery, null, connStr);
            }

            // Return resutl
            if (dsInvoice.Tables[TableName] != null)
            {
                dsInvoice.Tables.Remove(TableName);
            }

            dtInvoice.TableName = TableName;
            dsInvoice.Tables.Add(dtInvoice);
        }


        /// <summary>
        ///     Get Invoice Preview.
        /// </summary>
        /// <param name="dsInvoice"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetInvoicePreveiw(DataSet dsInvoice, int userID, string connStr)
        {
            DataTable dtInvoice;
            var invoice = new Invoice();

            var field = new Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate  query
            var invoiceViewQuery = new InvoiceView().GetInvoiceViewQueryPreview(dsInvoice, userID, connStr);

            // Generate  parameter
            var dtInvoiceViewCriteria = dsInvoice.Tables["InvoiceViewCriteria"];

            if (dtInvoiceViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtInvoiceViewCriteria.Rows.Count];

                for (var i = 0; i < dtInvoiceViewCriteria.Rows.Count; i++)
                {
                    var dr = dtInvoiceViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter(string.Format("@{0}{1}", dr["SeqNo"], field.GetFieldName(dr["FieldID"].ToString(), connStr)),
                            dr["Value"].ToString());
                }

                // Get data                
                dtInvoice = invoice.DbExecuteQuery(invoiceViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtInvoice = invoice.DbExecuteQuery(invoiceViewQuery, null, connStr);
            }

            // Return result
            if (dsInvoice.Tables[TableName] != null)
            {
                dsInvoice.Tables.Remove(TableName);
            }

            dtInvoice.TableName = TableName;
            dsInvoice.Tables.Add(dtInvoice);
        }

        /// <summary>
        ///     Get all of invoice except recurring type
        /// </summary>
        /// <param name="dsInvoice"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceList(DataSet dsInvoice, string connStr)
        {
            // Get data
            var result = DbRetrieve("AP.GetInvoiceForPopup", dsInvoice, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Savevoid
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool SaveVoid(DataSet savedData, string connStr)
        {
            // สร้าง SaveSource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);

            // Call dbCommit and send savesource object is parameter
            var result = DbCommit(dbSaveSource, connStr);

            return result;
        }

        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsInvoice"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsInvoice, string connStr)
        {
            var invoiceMisc = new InvoiceMisc();
            var taxInvoice = new TaxInvoice();
            var invoiceDetail = new InvoiceDetail();
            var invoiceDetailAmt = new InvoiceDetailAmt();
            var invoiceAttachment = new InvoiceAttachment();


            var dbSaveSorce = new DbSaveSource[6];


            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsInvoice, SelectCommand, TableName);
            dbSaveSorce[1] = new DbSaveSource(dsInvoice, invoiceMisc.SelectCommand, invoiceMisc.TableName);
            dbSaveSorce[2] = new DbSaveSource(dsInvoice, taxInvoice.SelectCommand, taxInvoice.TableName);
            dbSaveSorce[3] = new DbSaveSource(dsInvoice, invoiceDetail.SelectCommand, invoiceDetail.TableName);
            dbSaveSorce[4] = new DbSaveSource(dsInvoice, invoiceDetailAmt.SelectCommand, invoiceDetailAmt.TableName);
            dbSaveSorce[5] = new DbSaveSource(dsInvoice, invoiceAttachment.SelectCommand, invoiceAttachment.TableName);

            // Save to database
            var result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Delete from database
        /// </summary>
        /// <param name="dsInvoice"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsInvoice, string connStr)
        {
            var dbSaveSorce = new DbSaveSource[4];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsInvoice, _invoiceDetail.SelectCommand, _invoiceDetail.TableName);
            dbSaveSorce[1] = new DbSaveSource(dsInvoice, _invoiceMisc.SelectCommand, _invoiceMisc.TableName);
            dbSaveSorce[2] = new DbSaveSource(dsInvoice, _taxInvoice.SelectCommand, _taxInvoice.TableName);
            dbSaveSorce[3] = new DbSaveSource(dsInvoice, SelectCommand, TableName);

            // Save to database
            var result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get invoice database schema
        /// </summary>
        /// <param name="dsInvoice"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceStructure(DataSet dsInvoice, string connStr)
        {
            // Get data
            var result = DbRetrieveSchema("AP.GetInvoiceList", dsInvoice, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get max invoice id
        /// </summary>
        /// <returns></returns>
        public int GetInvoiceMaxID(string connStr)
        {
            // Get data
            var result = DbReadScalar("AP.GetInvoiceMaxID", null, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     return datatable and get searching match data.
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetSearchInvoiceList(string searchParam, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Search_Param", searchParam);

            return DbRead("AP.GetSearchInvoiceList", dbParams, connStr);
        }

        #endregion
    }
}