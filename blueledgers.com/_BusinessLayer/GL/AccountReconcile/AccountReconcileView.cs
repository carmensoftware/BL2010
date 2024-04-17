using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class AccountReconcileView : DbHandler
    {
        #region "Operations"

        public AccountReconcileView()
        {
            SelectCommand = "SELECT * FROM GL.AccountReconcileView";
            TableName = "AccountReconcileView";
        }


        /// <summary>
        ///     Get accountreconcile view using accrec view id
        /// </summary>
        /// <param name="accRecViewID"></param>
        /// <returns></returns>
        public DataTable GetAccountReconcileView(int accRecViewID, string connStr)
        {
            var AccountReconcileView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@accRecViewID", accRecViewID.ToString());

            // Get data
            AccountReconcileView = DbRead("GL.GetAccountReconcileView", dbParams, connStr);

            // Return result
            return AccountReconcileView;
        }

        /// <summary>
        ///     Get accountreconcile view using view id
        /// </summary>
        /// <param name="accRecViewID"></param>
        /// <returns></returns>
        public bool GetAccountReconcileView(int accRecViewID, DataSet dsAccountReconcileView, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@accRecViewID", accRecViewID.ToString());

            // Get data
            result = DbRetrieve("GL.GetAccountReconcileView", dsAccountReconcileView, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all account reconcile view depend on user id.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAccountReconcileViewByCreatedBy(int userID, string connStr)
        {
            var dtView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@CreatedBy", userID.ToString());

            // Get data
            dtView = DbRead("GL.GetAccountReconcileViewByCreatedBy", dbParams, connStr);

            // Return result
            return dtView;
        }

        /// <summary>
        ///     Get journalvoucher view schema
        /// </summary>
        /// <param name="AccountReconcileView"></param>
        /// <returns></returns>
        public bool GetAccountReconcileViewSchema(DataSet AccountReconcileView, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetAccountReconcileViewList", AccountReconcileView, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get lastest journalvoucher view id
        /// </summary>
        /// <returns></returns>
        public int GetMaxAccountReconcileViewID(string connStr)
        {
            var maxaccRecViewID = 0;

            // Get data
            maxaccRecViewID = DbReadScalar("GL.GetMaxAccountReconcileViewID", null, connStr);

            // Return result
            return maxaccRecViewID;
        }

        /// <summary>
        ///     Check for return statement is correct or not.
        /// </summary>
        /// <param name="AccountReconcileView"></param>
        /// <param name="AccountReconcileViewCriteria"></param>
        /// <param name="AccountReconcileViewColumn"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool IsValidQuery(DataSet dsAccountReconcileView, string connStr)
        {
            var accountReconcileViewColumn = new AccountReconcileViewColumn();
            var accountReconcileViewCriteria = new AccountReconcileViewCriteria();

            var view = new DataTable();
            var viewCriteria = new DataTable();
            var viewColumn = new DataTable();

            // Set data to datatable
            view = dsAccountReconcileView.Tables[TableName];
            viewCriteria = dsAccountReconcileView.Tables[accountReconcileViewCriteria.TableName];
            viewColumn = dsAccountReconcileView.Tables[accountReconcileViewColumn.TableName];

            // Return result
            return GnxLib.SqlParse(view, viewCriteria, viewColumn, connStr);
        }

        /// <summary>
        ///     Get account reconcile view preview.
        /// </summary>
        /// <param name="dsAccountReconcileView"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetAccountReconcilePreveiw(DataSet dsAccountReconcileView, int userID, string connStr)
        {
            var accountReconcileViewCriteria = new AccountReconcileViewCriteria();
            var dtAccountReconcileView = new DataTable();
            var dtAccountReconcileViewCriteria = new DataTable();
            var accountReconcileView = new AccountReconcileView();
            var accountReconcileViewQuery = string.Empty;

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate  query
            accountReconcileViewQuery =
                new AccountReconcileView().GetAccountReconcileViewQueryPreview(dsAccountReconcileView, userID, connStr);


            // Generate  parameter
            dtAccountReconcileViewCriteria = dsAccountReconcileView.Tables[accountReconcileViewCriteria.TableName];

            if (dtAccountReconcileViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtAccountReconcileViewCriteria.Rows.Count];

                for (var i = 0; i < dtAccountReconcileViewCriteria.Rows.Count; i++)
                {
                    var dr = dtAccountReconcileViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtAccountReconcileView = accountReconcileView.DbExecuteQuery(accountReconcileViewQuery, dbParams,
                    connStr);
            }
            else
            {
                // Get data                
                dtAccountReconcileView = accountReconcileView.DbExecuteQuery(accountReconcileViewQuery, null, connStr);
            }

            // Return result
            if (dsAccountReconcileView.Tables[TableName] != null)
            {
                dsAccountReconcileView.Tables.Remove(TableName);
            }

            dtAccountReconcileView.TableName = TableName;
            dsAccountReconcileView.Tables.Add(dtAccountReconcileView);
        }

        /// <summary>
        ///     Get account reconcile view preview for genearate query.
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetAccountReconcileViewQueryPreview(DataSet dsPreview, int userID, string connStr)
        {
            var accountReconcileViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            if (dsPreview.Tables["AccountReconcileViewColumn"] != null)
            {
                columnList =
                    AccountReconcileViewColumn.GetAccountReconcileViewColumnPreview(
                        dsPreview.Tables["AccountReconcileViewColumn"], connStr);
            }

            // Generate where clause
            if (dsPreview.Tables["AccountReconcileViewCriteria"] != null)
            {
                whereClause = AccountReconcileViewCriteria.GetAccountReconcileViewCriteriaPreview(dsPreview, userID,
                    connStr);
            }


            columnList = columnList + (columnList != string.Empty ? " , " : string.Empty);

            // Generate query
            accountReconcileViewQuery = "SELECT " + columnList.Substring(0, columnList.Length - 2) +
                                        " FROM GL.vAccountReconcile " +
                                        (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);


            return accountReconcileViewQuery;
        }

        /// <summary>
        ///     Commit to data base for save
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var accountReconcileViewCriteria = new AccountReconcileViewCriteria();
            var accountReconcileViewColumn = new AccountReconcileViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, accountReconcileViewCriteria.SelectCommand,
                accountReconcileViewCriteria.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, accountReconcileViewColumn.SelectCommand,
                accountReconcileViewColumn.TableName);

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
            var accountReconcileViewCriteria = new AccountReconcileViewCriteria();
            var accountReconcileViewColumn = new AccountReconcileViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(deletedData, accountReconcileViewCriteria.SelectCommand,
                accountReconcileViewCriteria.TableName);
            dbSaveSource[1] = new DbSaveSource(deletedData, accountReconcileViewColumn.SelectCommand,
                accountReconcileViewColumn.TableName);
            dbSaveSource[2] = new DbSaveSource(deletedData, SelectCommand, TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}