using System.Data;
using Blue.DAL;

namespace Blue.BL.GL.AccountListing
{
    public class AccountDetailView : DbHandler
    {
        #region "Attributies"

        private readonly AccountDetailViewColumn _accDetailViewColumn = new AccountDetailViewColumn();
        private readonly AccountDetailViewCriteria _accDetailViewCriteria = new AccountDetailViewCriteria();

        #endregion

        #region "Operations"

        public AccountDetailView()
        {
            SelectCommand = "SELECT * FROM GL.AccountDetailView";
            TableName = "AccountDetailView";
        }

        /// <summary>
        ///     This function user for get list of account detail view data that relate to sepecified user id by
        ///     using stored procedure "GL.GetAccountDetailViewList_UserID".
        /// </summary>
        /// <param name="dsAccDtLst"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAccDtLst, int userID, string connStr)
        {
            // Declare varialbe and initialize.
            var result = false;

            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@UserID", userID.ToString());

            // Get data
            result = DbRetrieve("GL.GetAccountDetailViewList_UserID", dsAccDtLst, dbParams, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     This function is used for get table structure of GL.AccountDetailView
        ///     by using stored procedure "GL.GetAccountDetailViewList".
        /// </summary>
        /// <param name="dsAccDtView"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsAccDtView, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetAccountDetailViewList", dsAccDtView, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     This function is used for get GL.AccountDetailView data that related to specified view id
        ///     by using stored procedure "GL.GetAccountDetailView_ViewID"
        /// </summary>
        /// <param name="dsAccDtView"></param>
        /// <param name="viewId"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsAccDtView, int viewId, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ViewID", viewId.ToString());

            // Get data
            result = DbRetrieve("GL.GetAccountDetailViewList_ViewID", dsAccDtView, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     This function is used for get GL.AccountDetailView new id by
        ///     using stored procedure "GL.GetAccountDetailViewNewID".
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewID(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("GL.GetAccountDetailViewNewID", null, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     This function is used for validate account detail view setting is valid or not.
        /// </summary>
        /// <param name="dsAccDtView"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool IsValidQuery(DataSet dsAccDtView, string connStr)
        {
            var dtaccDetailView = new DataTable();
            var dtaccDetailViewCriteria = new DataTable();
            var dtaccDetailViewColumn = new DataTable();

            // Set data to datatable
            dtaccDetailView = dsAccDtView.Tables[TableName];
            dtaccDetailViewColumn = dsAccDtView.Tables[_accDetailViewColumn.TableName];
            dtaccDetailViewCriteria = dsAccDtView.Tables[_accDetailViewCriteria.TableName];

            // Return result
            return GnxLib.SqlParse(dtaccDetailView, dtaccDetailViewCriteria, dtaccDetailViewColumn, connStr);
        }

        /// <summary>
        ///     This function is used for commit account detail view changed to database.
        /// </summary>
        /// <param name="dsAccDtView"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsAccDtView, string connStr)
        {
            var result = false;
            var accDtViewColumn = new AccountDetailViewColumn();
            var accDtViewCriteria = new AccountDetailViewCriteria();
            var dbSaveSorce = new DbSaveSource[3];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsAccDtView, SelectCommand, TableName);
            dbSaveSorce[1] = new DbSaveSource(dsAccDtView, accDtViewColumn.SelectCommand, accDtViewColumn.TableName);
            dbSaveSorce[2] = new DbSaveSource(dsAccDtView, accDtViewCriteria.SelectCommand, accDtViewCriteria.TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     This function is used for commit deleted account detail view data to database.
        /// </summary>
        /// <param name="dsAccDtView"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsAccDtView, string connStr)
        {
            var result = false;
            var accDtViewColumn = new AccountDetailViewColumn();
            var accDtViewCriteria = new AccountDetailViewCriteria();
            var dbSaveSorce = new DbSaveSource[3];

            // Create dbSaveSource            
            dbSaveSorce[0] = new DbSaveSource(dsAccDtView, accDtViewColumn.SelectCommand, accDtViewColumn.TableName);
            dbSaveSorce[1] = new DbSaveSource(dsAccDtView, accDtViewCriteria.SelectCommand, accDtViewCriteria.TableName);
            dbSaveSorce[2] = new DbSaveSource(dsAccDtView, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get account detail for preview
        /// </summary>
        /// <param name="dsAccuntDetail"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetPreveiw(DataSet dsAccountDetail, int userID, string connStr)
        {
            var dtAccDtView = new DataTable();
            var dtAccDtViewCriteria = new DataTable();
            var accDtView = new AccountDetailView();
            var accDtViewCriteria = new AccountDetailViewCriteria();
            var accDtViewQuery = string.Empty;

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate  query
            accDtViewQuery = new AccountDetailView().GetQueryPreview(dsAccountDetail, userID, connStr);

            // Generate  parameter
            dtAccDtViewCriteria = dsAccountDetail.Tables[accDtViewCriteria.TableName];

            if (dtAccDtViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtAccDtViewCriteria.Rows.Count];

                for (var i = 0; i < dtAccDtViewCriteria.Rows.Count; i++)
                {
                    var dr = dtAccDtViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data  
                dtAccDtView = accDtView.DbExecuteQuery(accDtViewQuery, dbParams, connStr);
                //dtAccDtView = DbExecuteQuery(accDtViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtAccDtView = accDtView.DbExecuteQuery(accDtViewQuery, null, connStr);
                //dtAccDtView = DbExecuteQuery(accDtViewQuery, null, connStr);
            }

            // Return result
            if (dsAccountDetail.Tables[TableName] != null)
            {
                dsAccountDetail.Tables.Remove(TableName);
            }

            dtAccDtView.TableName = TableName;
            dsAccountDetail.Tables.Add(dtAccDtView);
        }

        /// <summary>
        ///     Generate query of assigned dataset
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetQueryPreview(DataSet dsPreview, int userID, string connStr)
        {
            var viewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            if (dsPreview.Tables["AccountDetailViewColumn"] != null)
            {
                columnList = AccountDetailViewColumn.GetColumnQuery(dsPreview.Tables["AccountDetailViewColumn"], connStr);
            }

            // Generate where clause
            if (dsPreview.Tables["AccountDetailViewCriteria"] != null)
            {
                whereClause = AccountDetailViewCriteria.GetCriteriaQuery(dsPreview, userID, connStr);
            }

            columnList = columnList + (columnList != string.Empty ? " , " : string.Empty);

            // Generate query
            viewQuery = "SELECT " + columnList.Substring(0, columnList.Length - 2) +
                        " FROM GL.vGLHisAccountDetail " +
                        (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return viewQuery;
        }

        /// <summary>
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetViewList(int userID, string connStr)
        {
            var dtView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@CreatedBy", userID.ToString());

            // Get data
            dtView = DbRead("GL.GetAccountDetailViewList_CreatedBy", dbParams, connStr);

            // Return result
            return dtView;
        }

        #endregion
    }
}