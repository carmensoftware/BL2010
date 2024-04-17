using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class Receipt : DbHandler
    {
        #region "Attributies"

        private readonly ReceiptAttachment _receiptAttachment = new ReceiptAttachment();
        private readonly ReceiptAuto _receiptAuto = new ReceiptAuto();
        private readonly ReceiptCash _receiptCash = new ReceiptCash();
        private readonly ReceiptCheq _receiptCheq = new ReceiptCheq();
        private readonly ReceiptCheqDetail _receiptCheqDetail = new ReceiptCheqDetail();
        private readonly ReceiptComment _receiptComment = new ReceiptComment();
        private readonly ReceiptCredit _receiptCredit = new ReceiptCredit();
        private readonly ReceiptDetail _receiptDetail = new ReceiptDetail();
        private readonly ReceiptDetailAmt _receiptDetailAmt = new ReceiptDetailAmt();
        private readonly ReceiptLog _receiptLog = new ReceiptLog();
        private readonly ReceiptMisc _receiptMisc = new ReceiptMisc();
        private readonly ReceiptTrans _receiptTrans = new ReceiptTrans();
        private readonly ReceiptWHT _receiptWHT = new ReceiptWHT();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Receipt()
        {
            SelectCommand = "SELECT * FROM AR.Receipt";
            TableName = "Receipt";
        }

        /// <summary>
        ///     Get receipt using no.
        /// </summary>
        /// <param name="ReceiptNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetReceipt(DataSet dsReceipt, string connStr)
        {
            DbRetrieve("AR.GetReceiptList", dsReceipt, null, TableName, connStr);

            // Return result
            return dsReceipt;
        }

        /// <summary>
        ///     Get data by receipt no.
        /// </summary>
        /// <param name="dsReceipt"></param>
        /// <param name="ReceiptNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetReceiptByReceiptNo(DataSet dsReceipt, string ReceiptNo, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ReceiptNo", ReceiptNo);

            // Get data
            result = DbRetrieve("AR.GetReceiptByReceiptNo", dsReceipt, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get receipt using receipt view id
        /// </summary>
        /// <param name="dsAccount"></param>
        public void GetReceiptList(DataSet dsReceipt, int receiptViewID, int userID, string connStr)
        {
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            var dtReceipt = new DataTable();
            var dtReceiptViewCriteria = new DataTable();
            var receipt = new Receipt();
            var receiptViewQuery = string.Empty;

            // Generate query
            receiptViewQuery = new ReceiptView().GetReceiptViewQuery(receiptViewID, userID, connStr);

            // Generate parameter
            dtReceiptViewCriteria = new ReceiptViewCriteria().GetReceiptViewCriteriaList(receiptViewID, connStr);

            if (dtReceiptViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtReceiptViewCriteria.Rows.Count];

                for (var i = 0; i < dtReceiptViewCriteria.Rows.Count; i++)
                {
                    var dr = dtReceiptViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtReceipt = receipt.DbExecuteQuery(receiptViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtReceipt = receipt.DbExecuteQuery(receiptViewQuery, null, connStr);
            }

            // Return resutl
            if (dsReceipt.Tables[TableName] != null)
            {
                dsReceipt.Tables.Remove(TableName);
            }

            dtReceipt.TableName = TableName;
            dsReceipt.Tables.Add(dtReceipt);
        }

        /// <summary>
        ///     Save data.
        /// </summary>
        /// <param name="dsReceipt"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsReceipt, string paymentMethodCode, string connStr)
        {
            var result = false;
            var receiptMisc = new ReceiptMisc();
            var receiptComment = new ReceiptComment();
            var receiptAttachment = new ReceiptAttachment();
            var receiptWHT = new ReceiptWHT();
            var receiptLog = new ReceiptLog();
            var receiptDetail = new ReceiptDetail();
            var receiptDetailAmt = new ReceiptDetailAmt();
            var receiptAuto = new ReceiptAuto();
            var receiptCash = new ReceiptCash();
            var receiptCredit = new ReceiptCredit();
            var receiptCheq = new ReceiptCheq();
            var receiptCheqDetail = new ReceiptCheqDetail();
            var receiptTrans = new ReceiptTrans();

            switch (paymentMethodCode.ToUpper())
            {
                case "AUTO":
                {
                    // Create dbSaveSource
                    var dbSaveSorce = new DbSaveSource[3];
                    dbSaveSorce[0] = new DbSaveSource(dsReceipt, SelectCommand, TableName);
                    dbSaveSorce[1] = new DbSaveSource(dsReceipt, receiptAuto.SelectCommand, receiptAuto.TableName);
                    dbSaveSorce[2] = new DbSaveSource(dsReceipt, receiptAttachment.SelectCommand,
                        receiptAttachment.TableName);
                    //dbSaveSorce[3] = new DbSaveSource(dsReceipt, receiptMisc.SelectCommand, receiptMisc.TableName);
                    //dbSaveSorce[4] = new DbSaveSource(dsReceipt, receiptComment.SelectCommand, receiptComment.TableName);
                    //dbSaveSorce[5] = new DbSaveSource(dsReceipt, receiptWHT.SelectCommand, receiptWHT.TableName);
                    //dbSaveSorce[6] = new DbSaveSource(dsReceipt, receiptLog.SelectCommand, receiptLog.TableName);
                    //dbSaveSorce[7] = new DbSaveSource(dsReceipt, receiptDetail.SelectCommand, receiptDetail.TableName);
                    //dbSaveSorce[8] = new DbSaveSource(dsReceipt, receiptDetailAmt.SelectCommand, receiptDetailAmt.TableName);

                    // Save to database
                    result = DbCommit(dbSaveSorce, connStr);

                    break;
                }
                case "CARD":
                {
                    // Create dbSaveSource
                    var dbSaveSorce = new DbSaveSource[3];
                    dbSaveSorce[0] = new DbSaveSource(dsReceipt, SelectCommand, TableName);
                    dbSaveSorce[1] = new DbSaveSource(dsReceipt, receiptCredit.SelectCommand, receiptCredit.TableName);
                    dbSaveSorce[2] = new DbSaveSource(dsReceipt, receiptAttachment.SelectCommand,
                        receiptAttachment.TableName);
                    //dbSaveSorce[3] = new DbSaveSource(dsReceipt, receiptMisc.SelectCommand, receiptMisc.TableName);
                    //dbSaveSorce[4] = new DbSaveSource(dsReceipt, receiptComment.SelectCommand, receiptComment.TableName);
                    //dbSaveSorce[5] = new DbSaveSource(dsReceipt, receiptWHT.SelectCommand, receiptWHT.TableName);
                    //dbSaveSorce[6] = new DbSaveSource(dsReceipt, receiptLog.SelectCommand, receiptLog.TableName);
                    //dbSaveSorce[7] = new DbSaveSource(dsReceipt, receiptDetail.SelectCommand, receiptDetail.TableName);
                    //dbSaveSorce[8] = new DbSaveSource(dsReceipt, receiptDetailAmt.SelectCommand, receiptDetailAmt.TableName);

                    // Save to database
                    result = DbCommit(dbSaveSorce, connStr);

                    break;
                }
                case "CASH":
                {
                    // Create dbSaveSource
                    var dbSaveSorce = new DbSaveSource[3];
                    dbSaveSorce[0] = new DbSaveSource(dsReceipt, SelectCommand, TableName);
                    dbSaveSorce[1] = new DbSaveSource(dsReceipt, receiptCash.SelectCommand, receiptCash.TableName);
                    dbSaveSorce[2] = new DbSaveSource(dsReceipt, receiptAttachment.SelectCommand,
                        receiptAttachment.TableName);
                    //dbSaveSorce[3] = new DbSaveSource(dsReceipt, receiptMisc.SelectCommand, receiptMisc.TableName);
                    //dbSaveSorce[4] = new DbSaveSource(dsReceipt, receiptComment.SelectCommand, receiptComment.TableName);
                    //dbSaveSorce[5] = new DbSaveSource(dsReceipt, receiptWHT.SelectCommand, receiptWHT.TableName);
                    //dbSaveSorce[6] = new DbSaveSource(dsReceipt, receiptLog.SelectCommand, receiptLog.TableName);
                    //dbSaveSorce[7] = new DbSaveSource(dsReceipt, receiptDetail.SelectCommand, receiptDetail.TableName);
                    //dbSaveSorce[8] = new DbSaveSource(dsReceipt, receiptDetailAmt.SelectCommand, receiptDetailAmt.TableName);

                    // Save to database
                    result = DbCommit(dbSaveSorce, connStr);

                    break;
                }
                case "CHEQ":
                {
                    // Create dbSaveSource
                    var dbSaveSorce = new DbSaveSource[4];
                    dbSaveSorce[0] = new DbSaveSource(dsReceipt, SelectCommand, TableName);
                    dbSaveSorce[1] = new DbSaveSource(dsReceipt, receiptCheq.SelectCommand, receiptCheq.TableName);
                    dbSaveSorce[2] = new DbSaveSource(dsReceipt, receiptCheqDetail.SelectCommand,
                        receiptCheqDetail.TableName);
                    dbSaveSorce[3] = new DbSaveSource(dsReceipt, receiptAttachment.SelectCommand,
                        receiptAttachment.TableName);
                    //dbSaveSorce[3] = new DbSaveSource(dsReceipt, receiptMisc.SelectCommand, receiptMisc.TableName);
                    //dbSaveSorce[4] = new DbSaveSource(dsReceipt, receiptComment.SelectCommand, receiptComment.TableName);
                    //dbSaveSorce[5] = new DbSaveSource(dsReceipt, receiptWHT.SelectCommand, receiptWHT.TableName);
                    //dbSaveSorce[6] = new DbSaveSource(dsReceipt, receiptLog.SelectCommand, receiptLog.TableName);
                    //dbSaveSorce[7] = new DbSaveSource(dsReceipt, receiptDetail.SelectCommand, receiptDetail.TableName);
                    //dbSaveSorce[8] = new DbSaveSource(dsReceipt, receiptDetailAmt.SelectCommand, receiptDetailAmt.TableName);

                    // Save to database
                    result = DbCommit(dbSaveSorce, connStr);

                    break;
                }
                case "TRAN":
                {
                    // Create dbSaveSource
                    var dbSaveSorce = new DbSaveSource[3];
                    dbSaveSorce[0] = new DbSaveSource(dsReceipt, SelectCommand, TableName);
                    dbSaveSorce[1] = new DbSaveSource(dsReceipt, receiptTrans.SelectCommand, receiptTrans.TableName);
                    dbSaveSorce[2] = new DbSaveSource(dsReceipt, receiptAttachment.SelectCommand,
                        receiptAttachment.TableName);
                    //dbSaveSorce[3] = new DbSaveSource(dsReceipt, receiptMisc.SelectCommand, receiptMisc.TableName);
                    //dbSaveSorce[4] = new DbSaveSource(dsReceipt, receiptComment.SelectCommand, receiptComment.TableName);
                    //dbSaveSorce[5] = new DbSaveSource(dsReceipt, receiptWHT.SelectCommand, receiptWHT.TableName);
                    //dbSaveSorce[6] = new DbSaveSource(dsReceipt, receiptLog.SelectCommand, receiptLog.TableName);
                    //dbSaveSorce[7] = new DbSaveSource(dsReceipt, receiptDetail.SelectCommand, receiptDetail.TableName);
                    //dbSaveSorce[8] = new DbSaveSource(dsReceipt, receiptDetailAmt.SelectCommand, receiptDetailAmt.TableName);

                    // Save to database
                    result = DbCommit(dbSaveSorce, connStr);

                    break;
                }
            }

            // Return result
            return result;
        }

        ///// <summary>
        ///// Get Receipt Preview.
        ///// </summary>
        ///// <param name="dsReceipt"></param>
        ///// <param name="userID"></param>
        ///// <param name="connStr"></param>
        //public void GetReceiptPreveiw(DataSet dsReceipt, int userID, string connStr)
        //{
        //    DataTable dtReceipt             = new DataTable();
        //    DataTable dtReceiptViewCriteria = new DataTable();
        //    Receipt Receipt                 = new Receipt();
        //    string ReceiptViewQuery         = string.Empty;
        //    BL.Application.Field field      = new BL.Application.Field();

        //    // Generate  query
        //    ReceiptViewQuery = new ReceiptView().GetReceiptViewQueryPreview(dsReceipt, userID, connStr);

        //    // Generate  parameter
        //    dtReceiptViewCriteria = dsReceipt.Tables["ReceiptViewCriteria"];

        //    if (dtReceiptViewCriteria.Rows.Count > 0)
        //    {
        //        DbParameter[] dbParams = new DbParameter[dtReceiptViewCriteria.Rows.Count];

        //        for (int i = 0; i < dtReceiptViewCriteria.Rows.Count; i++)
        //        {
        //            DataRow dr = dtReceiptViewCriteria.Rows[i];
        //            dbParams[i] = new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr), dr["Value"].ToString());
        //        }

        //        // Get data                
        //        dtReceipt = Receipt.DbExecuteQuery(ReceiptViewQuery, dbParams, connStr);
        //    }
        //    else
        //    {
        //        // Get data                
        //        dtReceipt = Receipt.DbExecuteQuery(ReceiptViewQuery, null, connStr);
        //    }

        //    // Return result
        //    if (dsReceipt.Tables[this.TableName] != null)
        //    {
        //        dsReceipt.Tables.Remove(this.TableName);
        //    }

        //    dtReceipt.TableName = this.TableName;
        //    dsReceipt.Tables.Add(dtReceipt);
        //}

        ///// <summary>
        ///// Get all of Receipt except recurring type
        ///// </summary>
        ///// <param name="dsReceipt"></param>
        ///// <returns></returns>
        //public bool GetReceiptList(DataSet dsReceipt, string connStr)
        //{
        //    bool result = false;

        //    // Get data
        //    result = DbRetrieve("AP.GetReceiptForPopup", dsReceipt, null, this.TableName, connStr);

        //    // Return result
        //    return result;
        //}

        ///// <summary>
        ///// Save to database
        ///// </summary>
        ///// <param name="dsReceipt"></param>
        ///// <returns></returns>
        ////public bool Save(DataSet dsReceipt, string connStr)
        ////{
        ////    ReceiptMisc ReceiptMisc = new ReceiptMisc();
        ////    ReceiptComment ReceiptComment = new ReceiptComment();
        ////    ReceiptAttachment ReceiptAttachment = new ReceiptAttachment();
        ////    Profile.BankAccount bankAccount = new Profile.BankAccount();
        ////    Profile.Contact contact = new Profile.Contact();
        ////    Profile.ContactDetail contactDetail = new Profile.ContactDetail();
        ////    ReceiptDefaultDetail ReceiptDefaultDetail = new ReceiptDefaultDetail();
        ////    PaymentDefaultCash paymentDefaultCash = new PaymentDefaultCash();
        ////    PaymentDefaultCheq paymentDefaultCheq = new PaymentDefaultCheq();
        ////    PaymentDefaultCredit paymentDefaultCredit = new PaymentDefaultCredit();
        ////    PaymentDefaultAuto paymentDefaultAuto = new PaymentDefaultAuto();
        ////    PaymentDefaultTrans paymentDefaultTrans = new PaymentDefaultTrans();
        ////    ReceiptDefaultWHT ReceiptDefaultWHT = new ReceiptDefaultWHT();

        ////    bool result = false;
        ////    DbSaveSource[] dbSaveSorce = new DbSaveSource[13];

        ////    // Create dbSaveSource
        ////    dbSaveSorce[0] = new DbSaveSource(dsReceipt, this.SelectCommand, this.TableName);
        ////    dbSaveSorce[1] = new DbSaveSource(dsReceipt, ReceiptMisc.SelectCommand, ReceiptMisc.TableName);
        ////    dbSaveSorce[2] = new DbSaveSource(dsReceipt, bankAccount.SelectCommand, bankAccount.TableName);
        ////    dbSaveSorce[3] = new DbSaveSource(dsReceipt, contact.SelectCommand, contact.TableName);
        ////    dbSaveSorce[4] = new DbSaveSource(dsReceipt, contactDetail.SelectCommand, contactDetail.TableName);
        ////    dbSaveSorce[5] = new DbSaveSource(dsReceipt, ReceiptDefaultDetail.SelectCommand, ReceiptDefaultDetail.TableName);
        ////    dbSaveSorce[6] = new DbSaveSource(dsReceipt, paymentDefaultCash.SelectCommand, paymentDefaultCash.TableName);
        ////    dbSaveSorce[7] = new DbSaveSource(dsReceipt, paymentDefaultCheq.SelectCommand, paymentDefaultCheq.TableName);
        ////    dbSaveSorce[8] = new DbSaveSource(dsReceipt, paymentDefaultCredit.SelectCommand, paymentDefaultCredit.TableName);
        ////    dbSaveSorce[9] = new DbSaveSource(dsReceipt, paymentDefaultAuto.SelectCommand, paymentDefaultAuto.TableName);
        ////    dbSaveSorce[10] = new DbSaveSource(dsReceipt, paymentDefaultTrans.SelectCommand, paymentDefaultTrans.TableName);

        ////    dbSaveSorce[11] = new DbSaveSource(dsReceipt, ReceiptDefaultWHT.SelectCommand, ReceiptDefaultWHT.TableName);
        ////    dbSaveSorce[12] = new DbSaveSource(dsReceipt, ReceiptAttachment.SelectCommand, ReceiptAttachment.TableName);

        ////    // Save to database
        ////    result = DbCommit(dbSaveSorce, connStr);

        ////    // Return result
        ////    return result;
        ////}

        /// <summary>
        ///     Delete from database
        /// </summary>
        /// <param name="dsReceipt"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsReceipt, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[14];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsReceipt, _receiptComment.SelectCommand, _receiptComment.TableName);
            dbSaveSorce[1] = new DbSaveSource(dsReceipt, _receiptMisc.SelectCommand, _receiptMisc.TableName);
            dbSaveSorce[2] = new DbSaveSource(dsReceipt, _receiptAttachment.SelectCommand, _receiptAttachment.TableName);
            dbSaveSorce[3] = new DbSaveSource(dsReceipt, _receiptLog.SelectCommand, _receiptLog.TableName);
            dbSaveSorce[4] = new DbSaveSource(dsReceipt, _receiptWHT.SelectCommand, _receiptWHT.TableName);
            dbSaveSorce[5] = new DbSaveSource(dsReceipt, _receiptTrans.SelectCommand, _receiptTrans.TableName);
            dbSaveSorce[6] = new DbSaveSource(dsReceipt, _receiptAuto.SelectCommand, _receiptAuto.TableName);
            dbSaveSorce[7] = new DbSaveSource(dsReceipt, _receiptCash.SelectCommand, _receiptCash.TableName);
            dbSaveSorce[8] = new DbSaveSource(dsReceipt, _receiptCheq.SelectCommand, _receiptCheq.TableName);
            dbSaveSorce[9] = new DbSaveSource(dsReceipt, _receiptCheqDetail.SelectCommand, _receiptCheqDetail.TableName);
            dbSaveSorce[10] = new DbSaveSource(dsReceipt, _receiptCredit.SelectCommand, _receiptCredit.TableName);
            dbSaveSorce[11] = new DbSaveSource(dsReceipt, _receiptDetailAmt.SelectCommand, _receiptDetailAmt.TableName);
            dbSaveSorce[12] = new DbSaveSource(dsReceipt, _receiptDetail.SelectCommand, _receiptDetail.TableName);
            dbSaveSorce[13] = new DbSaveSource(dsReceipt, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get Receipt database schema
        /// </summary>
        /// <param name="dsReceipt"></param>
        /// <returns></returns>
        public bool GetReceiptStructure(DataSet dsReceipt, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("AR.GetReceiptList", dsReceipt, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get max Receipt id
        /// </summary>
        /// <returns></returns>
        public int GetReceiptMaxID(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("AR.GetReceiptMaxID", null, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get data to preview page.
        /// </summary>
        /// <param name="dsReceipt"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetReceiptPreveiw(DataSet dsReceipt, int userID, string connStr)
        {
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            var Receipt = new Receipt();
            var dtReceipt = new DataTable();
            var dtReceiptViewCriteria = new DataTable();

            var receiptViewQuery = string.Empty;

            // Generate  query
            receiptViewQuery = new ReceiptView().GetReceiptViewQueryPreview(dsReceipt, userID, connStr);

            // Generate  parameter
            dtReceiptViewCriteria = dsReceipt.Tables["ReceiptViewCriteria"];

            if (dtReceiptViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtReceiptViewCriteria.Rows.Count];

                for (var i = 0; i < dtReceiptViewCriteria.Rows.Count; i++)
                {
                    var dr = dtReceiptViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtReceipt = Receipt.DbExecuteQuery(receiptViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtReceipt = Receipt.DbExecuteQuery(receiptViewQuery, null, connStr);
            }

            // Return result
            if (dsReceipt.Tables[TableName] != null)
            {
                dsReceipt.Tables.Remove(TableName);
            }

            dtReceipt.TableName = TableName;
            dsReceipt.Tables.Add(dtReceipt);
        }

        #endregion
    }
}