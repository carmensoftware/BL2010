using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class Invoice : DbHandler
    {
        #region "Attributies"

        private readonly InvoiceDetail _invoiceDetail = new InvoiceDetail();
        private readonly InvoiceMisc _invoiceMisc = new InvoiceMisc();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Invoice()
        {
            SelectCommand = "SELECT * FROM AR.Invoice";
            TableName = "Invoice";
        }

        /// <summary>
        ///     Get invoice using id
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <returns></returns>
        public DataTable Get(int InvoiceNo, string connStr)
        {
            var dtInvoice = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@InvoiceNo", InvoiceNo.ToString());

            // Get data
            dtInvoice = DbRead("AR.GetInvoice", dbParams, connStr);

            // Return result
            return dtInvoice;
        }

        /// <summary>
        ///     Get invoice using id
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <returns></returns>
        public bool Get(DataSet dsInvoice, string InvoiceNo, string connStr)
        {
            var result = false;
            var dtInvoice = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@InvoiceNo", InvoiceNo);

            // Get data
            result = DbRetrieve("AR.GetInvoiceByInvoiceNo", dsInvoice, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get invoice using invoice view id
        /// </summary>
        /// <param name="dsAccount"></param>
        public void GetList(DataSet dsInvoice, int invoiceViewID, int userID, string connStr)
        {
            var dtInvoice = new DataTable();
            var dtInvoiceViewCriteria = new DataTable();
            var invoice = new Invoice();
            var invoiceViewQuery = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate query
            invoiceViewQuery = new InvoiceView().GetInvoiceViewQuery(invoiceViewID, userID, connStr);

            // Generate parameter
            dtInvoiceViewCriteria = new InvoiceViewCriteria().GetInvoiceViewCriteriaList(invoiceViewID, connStr);

            if (dtInvoiceViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtInvoiceViewCriteria.Rows.Count];

                for (var i = 0; i < dtInvoiceViewCriteria.Rows.Count; i++)
                {
                    var dr = dtInvoiceViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
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
        ///     Get invoice database schema
        /// </summary>
        /// <param name="dsInvoice"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsInvoice, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("AR.GetInvoiceList", dsInvoice, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get max invoice id
        /// </summary>
        /// <returns></returns>
        public int GetNewInvoiceNo(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("AR.GetInvoiceMaxID", null, connStr);

            // Return result
            return (result == 0 ? 1 : result + 1);
        }

        /// <summary>
        ///     Get data to preview page.
        /// </summary>
        /// <param name="dsInvoice"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetPreveiw(DataSet dsInvoice, int userID, string connStr)
        {
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            var invoice = new Invoice();
            var dtInvoice = new DataTable();
            var dtInvoiceViewCriteria = new DataTable();

            var invoiceViewQuery = string.Empty;

            // Generate  query
            invoiceViewQuery = new InvoiceView().GetInvoiceViewQueryPreview(dsInvoice, userID, connStr);

            // Generate  parameter
            dtInvoiceViewCriteria = dsInvoice.Tables["InvoiceViewCriteria"];

            if (dtInvoiceViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtInvoiceViewCriteria.Rows.Count];

                for (var i = 0; i < dtInvoiceViewCriteria.Rows.Count; i++)
                {
                    var dr = dtInvoiceViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
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
        ///     Save to database
        /// </summary>
        /// <param name="dsInvoice"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsInvoice, string connStr)
        {
            ////    InvoiceMisc invoiceMisc = new InvoiceMisc();
            ////    InvoiceComment invoiceComment = new InvoiceComment();
            ////    InvoiceAttachment invoiceAttachment = new InvoiceAttachment();
            ////    Profile.BankAccount bankAccount = new Profile.BankAccount();
            ////    Profile.Contact contact = new Profile.Contact();
            ////    Profile.ContactDetail contactDetail = new Profile.ContactDetail();
            ////    InvoiceDefaultDetail invoiceDefaultDetail = new InvoiceDefaultDetail();
            ////    PaymentDefaultCash paymentDefaultCash = new PaymentDefaultCash();
            ////    PaymentDefaultCheq paymentDefaultCheq = new PaymentDefaultCheq();
            ////    PaymentDefaultCredit paymentDefaultCredit = new PaymentDefaultCredit();
            ////    PaymentDefaultAuto paymentDefaultAuto = new PaymentDefaultAuto();
            ////    PaymentDefaultTrans paymentDefaultTrans = new PaymentDefaultTrans();
            ////    InvoiceDefaultWHT invoiceDefaultWHT = new InvoiceDefaultWHT();

            var result = false;
            var dbSaveSorce = new DbSaveSource[13];

            ////    // Create dbSaveSource
            ////    dbSaveSorce[0] = new DbSaveSource(dsInvoice, this.SelectCommand, this.TableName);
            ////    dbSaveSorce[1] = new DbSaveSource(dsInvoice, invoiceMisc.SelectCommand, invoiceMisc.TableName);
            ////    dbSaveSorce[2] = new DbSaveSource(dsInvoice, bankAccount.SelectCommand, bankAccount.TableName);
            ////    dbSaveSorce[3] = new DbSaveSource(dsInvoice, contact.SelectCommand, contact.TableName);
            ////    dbSaveSorce[4] = new DbSaveSource(dsInvoice, contactDetail.SelectCommand, contactDetail.TableName);
            ////    dbSaveSorce[5] = new DbSaveSource(dsInvoice, invoiceDefaultDetail.SelectCommand, invoiceDefaultDetail.TableName);
            ////    dbSaveSorce[6] = new DbSaveSource(dsInvoice, paymentDefaultCash.SelectCommand, paymentDefaultCash.TableName);
            ////    dbSaveSorce[7] = new DbSaveSource(dsInvoice, paymentDefaultCheq.SelectCommand, paymentDefaultCheq.TableName);
            ////    dbSaveSorce[8] = new DbSaveSource(dsInvoice, paymentDefaultCredit.SelectCommand, paymentDefaultCredit.TableName);
            ////    dbSaveSorce[9] = new DbSaveSource(dsInvoice, paymentDefaultAuto.SelectCommand, paymentDefaultAuto.TableName);
            ////    dbSaveSorce[10] = new DbSaveSource(dsInvoice, paymentDefaultTrans.SelectCommand, paymentDefaultTrans.TableName);

            ////    dbSaveSorce[11] = new DbSaveSource(dsInvoice, invoiceDefaultWHT.SelectCommand, invoiceDefaultWHT.TableName);
            ////    dbSaveSorce[12] = new DbSaveSource(dsInvoice, invoiceAttachment.SelectCommand, invoiceAttachment.TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Delete from database
        /// </summary>
        /// <param name="dsInvoice"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsInvoice, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[3];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsInvoice, _invoiceDetail.SelectCommand, _invoiceDetail.TableName);
            dbSaveSorce[1] = new DbSaveSource(dsInvoice, _invoiceMisc.SelectCommand, _invoiceMisc.TableName);
            dbSaveSorce[2] = new DbSaveSource(dsInvoice, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}