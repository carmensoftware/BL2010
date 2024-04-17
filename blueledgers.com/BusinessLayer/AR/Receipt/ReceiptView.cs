using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ReceiptView : DbHandler
    {
        #region "Attibuties"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ReceiptView()
        {
            SelectCommand = "SELECT * FROM AR.ReceiptView";
            TableName = "ReceiptView";
        }

        /// <summary>
        ///     Get Receipt view schema
        /// </summary>
        /// <param name="ReceiptViewID"></param>
        /// <returns></returns>
        public bool GetReceiptViewSchema(DataSet dsReceiptViewID, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("AR.GetReceiptViewList", dsReceiptViewID, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get Receipt view using Receipt view id
        /// </summary>
        /// <param name="ReceiptViewID"></param>
        /// <returns></returns>
        public bool GetReceiptView(int receiptViewID, DataSet dsReceiptReceipt, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@ReceiptViewID", receiptViewID.ToString());

            // Get data
            result = DbRetrieve("AR.GetReceiptView", dsReceiptReceipt, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all Receipt view depend on user id.
        /// </summary>
        /// <returns></returns>
        public DataTable GetReceiptViewList(int userID, string connStr)
        {
            var dtReceiptView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@CreatedBy", userID.ToString());

            // Get data
            dtReceiptView = DbRead("AR.GetReceiptViewListByCreatedBy", dbParams, connStr);

            // Return result
            return dtReceiptView;
        }

        /// <summary>
        ///     Generate query of assigned ReceiptViewID
        /// </summary>
        /// <returns></returns>
        public string GetReceiptViewQuery(int ReceiptViewID, int userID, string connStr)
        {
            var ReceiptViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            columnList = ReceiptViewColumn.GetReceiptViewColumn(ReceiptViewID, connStr);

            // Generate where clause
            whereClause = ReceiptViewCriteria.GetReceiptViewCriteria(ReceiptViewID, userID, connStr);

            // Generate query
            ReceiptViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                               " ReceiptNo FROM AR.vReceipt " +
                               (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);
            return ReceiptViewQuery;
        }

        /// <summary>
        ///     Get Receipt view query.
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetReceiptViewQueryPreview(DataSet dsPreview, int userID, string connStr)
        {
            var ReceiptViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            if (dsPreview.Tables["ReceiptViewColumn"] != null)
            {
                columnList = ReceiptViewColumn.GetReceiptViewColumnPreview(dsPreview.Tables["ReceiptViewColumn"],
                    connStr);
            }

            // Generate where clause
            if (dsPreview.Tables["ReceiptViewCriteria"] != null)
            {
                whereClause = ReceiptViewCriteria.GetReceiptViewCriteriaPreview(dsPreview, userID, connStr);
            }

            // Generate query
            ReceiptViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                               " ReceiptNo FROM AR.vReceipt " +
                               (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return ReceiptViewQuery;
        }

        /// <summary>
        ///     Validate query before save
        /// </summary>
        /// <returns></returns>
        public bool GetReceiptViewIsValidQuery(DataTable dtReceiptView, DataTable dtReceiptViewCriteria,
            DataTable dtReceiptViewColumn, string connStr)
        {
            var ReceiptViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;
            var result = new DataTable();
            var dbParams = new DbParameter[dtReceiptViewCriteria.DefaultView.Count];

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate Column
            if (dtReceiptViewColumn != null)
            {
                foreach (DataRow dr in dtReceiptViewColumn.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        columnList += (columnList != string.Empty ? "," : string.Empty) +
                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);
                    }
                }
            }

            // Generate Criteria
            if (dtReceiptViewCriteria != null)
            {
                // Non-Advance option
                if (dtReceiptView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in dtReceiptViewCriteria.Rows)
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
                    whereClause = dtReceiptView.Rows[0]["AvanceOption"].ToString();

                    foreach (DataRow dr in dtReceiptViewCriteria.Rows)
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
                if (dtReceiptViewCriteria.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtReceiptViewCriteria.Rows)
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
            }

            // Generate query
            ReceiptViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                               " ReceiptNo FROM AR.vReceipt" +
                               (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            // Vlidate query
            result = DbExecuteQuery(ReceiptViewQuery, dbParams, connStr);

            // Return result
            if (result != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Get Receipt view using Receipt view id
        /// </summary>
        /// <param name="ReceiptViewID"></param>
        /// <returns></returns>
        public DataTable GetReceiptView(int receiptViewID, string connStr)
        {
            var dtReceiptView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@ReceiptViewID", receiptViewID.ToString());

            // Get data
            dtReceiptView = DbRead("AR.GetReceiptView", dbParams, connStr);

            // Return result
            return dtReceiptView;
        }

        /// <summary>
        ///     Get lastest receipt view id
        /// </summary>
        /// <returns></returns>
        public int GetReceiptViewMaxID(string connStr)
        {
            var maxAccountViewID = 0;

            // Get data
            maxAccountViewID = DbReadScalar("AR.GetReceiptViewMaxID", null, connStr);

            // Return result
            return maxAccountViewID;
        }

        /// <summary>
        ///     Commit to data base for save
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var receiptViewCriteria = new ReceiptViewCriteria();
            var receiptViewColumn = new ReceiptViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, receiptViewCriteria.SelectCommand,
                receiptViewCriteria.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, receiptViewColumn.SelectCommand, receiptViewColumn.TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        ///     Commit to data base for delete
        /// </summary>
        /// <param name="deletedData"></param>
        /// <returns></returns>
        public bool Delete(DataSet deletedData, string connStr)
        {
            var receiptViewCriteria = new ReceiptViewCriteria();
            var receiptViewColumn = new ReceiptViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(deletedData, receiptViewCriteria.SelectCommand,
                receiptViewCriteria.TableName);
            dbSaveSource[1] = new DbSaveSource(deletedData, receiptViewColumn.SelectCommand, receiptViewColumn.TableName);
            dbSaveSource[2] = new DbSaveSource(deletedData, SelectCommand, TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}