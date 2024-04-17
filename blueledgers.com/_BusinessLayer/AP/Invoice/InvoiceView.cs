using System.Data;
using System.Globalization;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class InvoiceView : DbHandler
    {
        #region "Attibuties"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public InvoiceView()
        {
            SelectCommand = "SELECT * FROM AP.InvoiceView";
            TableName = "InvoiceView";
        }

        /// <summary>
        ///     Get invoice view schema
        /// </summary>
        /// <param name="dsInvoice"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceViewSchema(DataSet dsInvoice, string connStr)
        {
            // Get data
            var result = DbRetrieveSchema("AP.GetInvoiceViewList", dsInvoice, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get invoice  view using invoice  view id
        /// </summary>
        /// <param name="invoiceViewID"></param>
        /// <param name="dsInvoice"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetInvoiceView(int invoiceViewID, DataSet dsInvoice, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@InvoiceViewID", invoiceViewID.ToString(CultureInfo.InvariantCulture));

            // Get data
            var result = DbRetrieve("AP.GetInvoiceView", dsInvoice, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all invoice  view depend on user id.
        /// </summary>
        /// <returns></returns>
        public DataTable GetInvoiceViewList(int userID, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@CreatedBy", userID.ToString(CultureInfo.InvariantCulture));

            // Get data
            var dtInvoiceView = DbRead("AP.GetInvoiceViewListByCreatedBy", dbParams, connStr);

            // Return result
            return dtInvoiceView;
        }

        /// <summary>
        ///     Generate query of assigned InvoiceViewID
        /// </summary>
        /// <returns></returns>
        public string GetInvoiceViewQuery(int invoiceViewID, int userID, string connStr)
        {
            // Generate columns
            var columnList = InvoiceViewColumn.GetInvoiceViewColumn(invoiceViewID, connStr);

            // Generate where clause
            var whereClause = InvoiceViewCriteria.GetInvoiceViewCriteria(invoiceViewID, userID, connStr);

            // Generate query
            var invoiceViewQuery = string.Format("SELECT {0}{1} VoucherNo FROM AP.vInvoice {2}", 
                columnList, (columnList != string.Empty ? " , " : string.Empty)
                , (whereClause != string.Empty 
                ? string.Format(" WHERE {0}", whereClause) 
                : string.Empty));

            return invoiceViewQuery;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetInvoiceViewQueryPreview(DataSet dsPreview, int userID, string connStr)
        {
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            if (dsPreview.Tables["InvoiceViewColumn"] != null)
            {
                columnList = InvoiceViewColumn.GetInvoiceViewColumnPreview(dsPreview.Tables["InvoiceViewColumn"],
                    connStr);
            }

            // Generate where clause
            if (dsPreview.Tables["InvoiceViewCriteria"] != null)
            {
                whereClause = InvoiceViewCriteria.GetInvoiceViewCriteriaPreview(dsPreview, userID, connStr);
            }

            // Generate query
            var invoiceViewQuery = string.Format("SELECT {0}{1} VoucherNo FROM AP.vInvoice {2}", columnList
                , (columnList != string.Empty ? " , " : string.Empty), 
                (whereClause != string.Empty 
                ? string.Format(" WHERE {0}", whereClause) 
                : string.Empty));

            return invoiceViewQuery;
        }


        /// <summary>
        ///     Validate query before save
        /// </summary>
        /// <returns></returns>
        public bool GetInvoiceViewIsValidQuery(DataTable invoiceView, DataTable invoiceViewCriteria,
            DataTable invoiceViewColumn, string connStr)
        {
            var columnList = string.Empty;
            var whereClause = string.Empty;
            var dbParams = new DbParameter[invoiceViewCriteria.DefaultView.Count];
            var field = new Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate Column
            if (invoiceViewColumn != null)
            {
                foreach (DataRow dr in invoiceViewColumn.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        columnList += string.Format("{0}{1}", 
                            (columnList != string.Empty ? "," : string.Empty),
                            field.GetFieldName(dr["FieldID"].ToString(), connStr));
                    }
                }
            }

            // Generate Criteria
            // Non-Advance option
            if (invoiceView.Rows[0]["IsAdvance"].ToString() != "True")
            {
                foreach (DataRow dr in invoiceViewCriteria.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        whereClause += string.Format("{0}{1} {2} @{3}{1} {4}",
                            (whereClause != string.Empty ? " " : string.Empty), 
                            field.GetFieldName(dr["FieldID"].ToString(), connStr), 
                            dr["Operator"], dr["SeqNo"], dr["LogicalOp"]);
                    }
                }
            }
                // Advance option
            else
            {
                whereClause = invoiceView.Rows[0]["AvanceOption"].ToString();

                foreach (DataRow dr in invoiceViewCriteria.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        var eachWhereClause = string.Format("{0} {1} @{2}{0}",
                            field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Operator"], dr["SeqNo"]);
                        whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                    }
                }
            }

            // Generate parameter
            if (invoiceViewCriteria.Rows.Count > 0)
            {
                foreach (DataRow dr in invoiceViewCriteria.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        dbParams[(int) dr["SeqNo"] - 1] =
                            new DbParameter(string.Format("@{0}{1}", dr["SeqNo"], 
                                field.GetFieldName(dr["FieldID"].ToString(), connStr)),
                                dr["Value"].ToString());
                    }
                }
            }
            else
            {
                dbParams = null;
            }

            // Generate query
            var invoiceViewQuery = string.Format("SELECT {0}{1} VoucherNo FROM AP.vInvoice{2}",
                columnList, (columnList != string.Empty ? " , " : string.Empty)
                , (whereClause != string.Empty
                    ? string.Format(" WHERE {0}", whereClause)
                    : string.Empty));

            // Vlidate query
            var result = DbExecuteQuery(invoiceViewQuery, dbParams, connStr);

            // Return result
            if (result != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Get invoice  view using invoice  view id
        /// </summary>
        /// <param name="invoiceViewID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetInvoiceView(int invoiceViewID, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@InvoiceViewID", invoiceViewID.ToString(CultureInfo.InvariantCulture));

            // Get data
            var invoiceView = DbRead("AP.GetInvoiceView", dbParams, connStr);

            // Return result
            return invoiceView;
        }

        /// <summary>
        ///     Get lastest invoice  view id
        /// </summary>
        /// <returns></returns>
        public int GetInvoiceViewMaxID(string connStr)
        {
            // Get data
            var maxAccountViewID = DbReadScalar("AP.GetInvoiceViewMaxID", null, connStr);

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
            var invoiceViewCriteria = new InvoiceViewCriteria();
            var invoiceViewColumn = new InvoiceViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, invoiceViewCriteria.SelectCommand,invoiceViewCriteria.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, invoiceViewColumn.SelectCommand, invoiceViewColumn.TableName);

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
            var invoiceViewCriteria = new InvoiceViewCriteria();
            var invoiceViewColumn = new InvoiceViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(deletedData, invoiceViewCriteria.SelectCommand,invoiceViewCriteria.TableName);
            dbSaveSource[1] = new DbSaveSource(deletedData, invoiceViewColumn.SelectCommand, invoiceViewColumn.TableName);
            dbSaveSource[2] = new DbSaveSource(deletedData, SelectCommand, TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}