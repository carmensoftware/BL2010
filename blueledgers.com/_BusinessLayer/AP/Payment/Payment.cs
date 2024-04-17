using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.AP
{
    public class Payment : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Payment()
        {
            SelectCommand = "SELECT * FROM AP.Payment";
            TableName = "Payment";
        }

        /// <summary>
        ///     Get payment using id
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPayment(string voucherNo, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            DataTable dtPayment = DbRead("AP.GetPayment", dbParams, connStr);

            // Return result
            return dtPayment;
        }

        /// <summary>
        ///     Get payment using id
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="dsPayment"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPayment(string voucherNo, DataSet dsPayment, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VoucherNo", voucherNo);

            // Get data
            bool result = DbRetrieve("AP.GetPayment", dsPayment, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     return datatable and get searching match data.
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPaymentListSearch(string keyWord, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@KeyWord", keyWord);

            return DbRead("AP.GetPaymentListSearch", dbParams, connStr);
        }

        /// <summary>
        ///     Get payment using account view id
        /// </summary>
        /// <param name="dsPayment"></param>
        /// <param name="paymentViewID"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetPaymentList(DataSet dsPayment, int paymentViewID, int userID, string connStr)
        {
            DataTable dtPayment;
            var payment = new Payment();
            var field = new Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate query
            string paymentViewQuery = new PaymentView().GetPaymentViewQuery(paymentViewID, userID, connStr);

            // Generate parameter
            DataTable dtPaymentViewCriteria = new PaymentViewCriteria().GetPaymentViewCriteriaList(paymentViewID, connStr);

            if (dtPaymentViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtPaymentViewCriteria.Rows.Count];

                for (var i = 0; i < dtPaymentViewCriteria.Rows.Count; i++)
                {
                    var dr = dtPaymentViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter(string.Format("@{0}{1}", dr["SeqNo"],
                            field.GetFieldName(dr["FieldID"].ToString(), connStr)),
                            dr["Value"].ToString());
                }

                // Get data                
                dtPayment = payment.DbExecuteQuery(paymentViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtPayment = payment.DbExecuteQuery(paymentViewQuery, null, connStr);
            }

            // Return resutl
            if (dsPayment.Tables[TableName] != null)
            {
                dsPayment.Tables.Remove(TableName);
            }

            dtPayment.TableName = TableName;
            dsPayment.Tables.Add(dtPayment);
        }

        /// <summary>
        ///     Get Payment Preview.
        /// </summary>
        /// <param name="dsPayment"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetPaymentPreveiw(DataSet dsPayment, int userID, string connStr)
        {
            DataTable dtPayment;
            var payment = new Payment();
            var field = new Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate  query
            string paymentViewQuery = new PaymentView().GetPaymentViewQueryPreview(dsPayment, userID, connStr);

            // Generate  parameter
            DataTable dtPaymentViewCriteria = dsPayment.Tables["PaymentViewCriteria"];

            if (dtPaymentViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtPaymentViewCriteria.Rows.Count];

                for (var i = 0; i < dtPaymentViewCriteria.Rows.Count; i++)
                {
                    var dr = dtPaymentViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter(string.Format("@{0}{1}", dr["SeqNo"],
                            field.GetFieldName(dr["FieldID"].ToString(), connStr)),
                            dr["Value"].ToString());
                }

                // Get data                
                dtPayment = payment.DbExecuteQuery(paymentViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtPayment = payment.DbExecuteQuery(paymentViewQuery, null, connStr);
            }

            // Return result
            if (dsPayment.Tables[TableName] != null)
            {
                dsPayment.Tables.Remove(TableName);
            }

            dtPayment.TableName = TableName;
            dsPayment.Tables.Add(dtPayment);
        }

        /// <summary>
        ///     Get all of payment except recurring type
        /// </summary>
        /// <param name="dsPayment"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentList(DataSet dsPayment, string connStr)
        {
            // Get data
            bool result = DbRetrieve("AP.GetPaymentForPopup", dsPayment, null, TableName, connStr);

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
            bool result = DbCommit(dbSaveSource, connStr);

            return result;
        }

        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsPayment"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsPayment, string connStr)
        {
            var paymentCash = new PaymentCash();
            var paymentCheq = new PaymentCheq();
            var paymentCredit = new PaymentCredit();
            var paymentAuto = new PaymentAuto();
            var paymentTrans = new PaymentTrans();
            var paymentCheqDetail = new PaymentCheqDetail();
            var paymentMisc = new PaymentMisc();
            var paymentWHT = new PaymentWHT();
            var paymentDetail = new PaymentDetail();
            var paymentDetailAmt = new PaymentDetailAmt();
            var paymentAttachment = new PaymentAttachment();


            var dbSaveSorce = new DbSaveSource[12];


            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsPayment, SelectCommand, TableName);
            dbSaveSorce[1] = new DbSaveSource(dsPayment, paymentCash.SelectCommand, paymentCash.TableName);
            dbSaveSorce[2] = new DbSaveSource(dsPayment, paymentCheq.SelectCommand, paymentCheq.TableName);
            dbSaveSorce[3] = new DbSaveSource(dsPayment, paymentCredit.SelectCommand, paymentCredit.TableName);
            dbSaveSorce[4] = new DbSaveSource(dsPayment, paymentAuto.SelectCommand, paymentAuto.TableName);
            dbSaveSorce[5] = new DbSaveSource(dsPayment, paymentTrans.SelectCommand, paymentTrans.TableName);
            dbSaveSorce[6] = new DbSaveSource(dsPayment, paymentCheqDetail.SelectCommand, paymentCheqDetail.TableName);
            dbSaveSorce[7] = new DbSaveSource(dsPayment, paymentMisc.SelectCommand, paymentMisc.TableName);
            dbSaveSorce[8] = new DbSaveSource(dsPayment, paymentWHT.SelectCommand, paymentWHT.TableName);
            dbSaveSorce[9] = new DbSaveSource(dsPayment, paymentDetailAmt.SelectCommand, paymentDetailAmt.TableName);
            dbSaveSorce[10] = new DbSaveSource(dsPayment, paymentDetail.SelectCommand, paymentDetail.TableName);

            dbSaveSorce[11] = new DbSaveSource(dsPayment, paymentAttachment.SelectCommand, paymentAttachment.TableName);

            // Save to database
            bool result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Delete from database
        /// </summary>
        /// <param name="dsPayment"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsPayment, string connStr)
        {
            var paymentMisc = new PaymentMisc();
            var paymentDetail = new PaymentDetail();
            var dbSaveSorce = new DbSaveSource[3];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsPayment, paymentDetail.SelectCommand, paymentDetail.TableName);
            dbSaveSorce[1] = new DbSaveSource(dsPayment, paymentMisc.SelectCommand, paymentMisc.TableName);
            dbSaveSorce[2] = new DbSaveSource(dsPayment, SelectCommand, TableName);

            // Save to database
            bool result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get payment database schema
        /// </summary>
        /// <param name="dsPayment"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentStructure(DataSet dsPayment, string connStr)
        {
            // Get data
            bool result = DbRetrieveSchema("AP.GetPaymentList", dsPayment, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get max payment id
        /// </summary>
        /// <returns></returns>
        public int GetPaymentMaxID(string connStr)
        {
            // Get data
            int result = DbReadScalar("AP.GetPaymentMaxID", null, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     return datatable and get searching match data.
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetSearchPaymentList(string searchParam, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Search_Param", searchParam);

            return DbRead("AP.GetSearchPaymentList", dbParams, connStr);
        }

        #endregion
    }
}