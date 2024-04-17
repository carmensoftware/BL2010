using System.Data;
using System.Globalization;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class PaymentView : DbHandler
    {
        #region "Attibuties"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PaymentView()
        {
            SelectCommand = "SELECT * FROM AP.PaymentView";
            TableName = "PaymentView";
        }

        /// <summary>
        ///     Get payment view schema
        /// </summary>
        /// <param name="dsPaymentView"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentViewSchema(DataSet dsPaymentView, string connStr)
        {
            // Get data
            var result = DbRetrieveSchema("AP.GetPaymentViewList", dsPaymentView, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get payment  view using payment  view id
        /// </summary>
        /// <param name="paymentViewID"></param>
        /// <param name="dsPayment"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPaymentView(int paymentViewID, DataSet dsPayment, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@PaymentViewID", paymentViewID.ToString(CultureInfo.InvariantCulture));

            // Get data
            var result = DbRetrieve("AP.GetPaymentView", dsPayment, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all payment  view depend on user id.
        /// </summary>
        /// <returns></returns>
        public DataTable GetPaymentViewList(int userID, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@CreatedBy", userID.ToString(CultureInfo.InvariantCulture));

            // Get data
            var dtPaymentView = DbRead("AP.GetPaymentViewListByCreatedBy", dbParams, connStr);

            // Return result
            return dtPaymentView;
        }

        /// <summary>
        ///     Generate query of assigned PaymentViewID
        /// </summary>
        /// <returns></returns>
        public string GetPaymentViewQuery(int paymentViewID, int userID, string connStr)
        {
            // Generate columns
            var columnList = PaymentViewColumn.GetPaymentViewColumn(paymentViewID, connStr);

            // Generate where clause
            var whereClause = PaymentViewCriteria.GetPaymentViewCriteria(paymentViewID, userID, connStr);

            // Generate query
            var paymentViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                                   " VoucherNo FROM AP.vPayment " +
                                   (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return paymentViewQuery;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetPaymentViewQueryPreview(DataSet dsPreview, int userID, string connStr)
        {
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            if (dsPreview.Tables["PaymentViewColumn"] != null)
            {
                columnList = PaymentViewColumn.GetPaymentViewColumnPreview(dsPreview.Tables["PaymentViewColumn"],
                    connStr);
            }

            // Generate where clause
            if (dsPreview.Tables["PaymentViewCriteria"] != null)
            {
                whereClause = PaymentViewCriteria.GetPaymentViewCriteriaPreview(dsPreview, userID, connStr);
            }

            // Generate query
            var paymentViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                                   " VoucherNo FROM AP.vPayment " +
                                   (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return paymentViewQuery;
        }


        /// <summary>
        ///     Validate query before save
        /// </summary>
        /// <returns></returns>
        public bool GetPaymentViewIsValidQuery(DataTable paymentView, DataTable paymentViewCriteria,
            DataTable paymentViewColumn, string connStr)
        {
            var columnList = string.Empty;
            var whereClause = string.Empty;
            var dbParams = new DbParameter[paymentViewCriteria.DefaultView.Count];
            var field = new Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate Column
            if (paymentViewColumn != null)
            {
                foreach (DataRow dr in paymentViewColumn.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        columnList += (columnList != string.Empty ? "," : string.Empty) +
                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);
                    }
                }
            }

            // Generate Criteria
            // Non-Advance option
            if (paymentView.Rows[0]["IsAdvance"].ToString() != "True")
            {
                foreach (DataRow dr in paymentViewCriteria.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        whereClause += (whereClause != string.Empty ? " " : string.Empty) +
                                       field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " + dr["Operator"] +
                                       " " +
                                       "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr) +
                                       " " + dr["LogicalOp"];
                    }
                }
            }
                // Advance option
            else
            {
                whereClause = paymentView.Rows[0]["AvanceOption"].ToString();

                foreach (DataRow dr in paymentViewCriteria.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                              dr["Operator"] + " " + "@" + dr["SeqNo"] +
                                              field.GetFieldName(dr["FieldID"].ToString(), connStr);
                        whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                    }
                }
            }

            // Generate parameter
            if (paymentViewCriteria.Rows.Count > 0)
            {
                foreach (DataRow dr in paymentViewCriteria.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        dbParams[(int) dr["SeqNo"] - 1] =
                            new DbParameter(
                                "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                                dr["Value"].ToString());
                    }
                }
            }
            else
            {
                dbParams = null;
            }

            // Generate query
            var paymentViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                                   " VoucherNo FROM AP.vPayment" +
                                   (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            // Vlidate query
            var result = DbExecuteQuery(paymentViewQuery, dbParams, connStr);

            // Return result
            if (result != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Get payment  view using payment  view id
        /// </summary>
        /// <param name="paymentViewID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetPaymentView(int paymentViewID, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@PaymentViewID", paymentViewID.ToString(CultureInfo.InvariantCulture));

            // Get data
            var paymentView = DbRead("AP.GetPaymentView", dbParams, connStr);

            // Return result
            return paymentView;
        }

        /// <summary>
        ///     Get lastest payment  view id
        /// </summary>
        /// <returns></returns>
        public int GetPaymentViewMaxID(string connStr)
        {
            // Get data
            var maxAccountViewID = DbReadScalar("AP.GetPaymentViewMaxID", null, connStr);

            // Return result
            return maxAccountViewID;
        }

        /// <summary>
        ///     Commit to data base for save
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var paymentViewCriteria = new PaymentViewCriteria();
            var paymentViewColumn = new PaymentViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, paymentViewCriteria.SelectCommand,
                paymentViewCriteria.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, paymentViewColumn.SelectCommand, paymentViewColumn.TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        ///     Commit to data base for delete
        /// </summary>
        /// <param name="deletedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet deletedData, string connStr)
        {
            var paymentViewCriteria = new PaymentViewCriteria();
            var paymentViewColumn = new PaymentViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(deletedData, paymentViewCriteria.SelectCommand,
                paymentViewCriteria.TableName);
            dbSaveSource[1] = new DbSaveSource(deletedData, paymentViewColumn.SelectCommand, paymentViewColumn.TableName);
            dbSaveSource[2] = new DbSaveSource(deletedData, SelectCommand, TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}