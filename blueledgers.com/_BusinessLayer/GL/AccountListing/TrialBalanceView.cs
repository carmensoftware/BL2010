using System.Data;
using Blue.DAL;

namespace Blue.BL.GL.AccountListing
{
    public class TrialBalanceView : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        public TrialBalanceView()
        {
            SelectCommand = "SELECT * FROM GL.TrialBalanceView";
            TableName = "TrialBalanceView";
        }


        /// <summary>
        ///     This function user for get trial balance view data by using stored procedure
        ///     "GL.GetTrialBalanceView_ViewID".
        /// </summary>
        /// <param name="dsTriBalList"></param>
        /// <param name="viewID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsTriBalList, int viewID, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];


            // Create parameter
            dbParams[0] = new DbParameter("@ViewID", viewID.ToString());

            // Get data
            result = DbRetrieve("GL.GetTrialBalanceView_ViewID", dsTriBalList, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     This function is used for get new trial balance view id
        ///     by using strored procedure "GL.GetTrialBalanceViewNewID".
        /// </summary>
        /// <returns></returns>
        public int GetNewID(string connStr)
        {
            // Initialize value.
            var result = 0;

            // Get data
            result = DbReadScalar("GL.GetTrialBalanceViewNewID", null, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     This function user for get list of trial balance view data
        ///     that relate to sepecified user id by using stored procedure "GL.GetTrialBalanceViewList_UserID".
        /// </summary>
        /// <param name="dsTriBalList"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsTriBalList, int userID, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];


            // Create parameter
            dbParams[0] = new DbParameter("@UserID", userID.ToString());

            // Get data
            result = DbRetrieve("GL.GetTrialBalanceViewList_UserID", dsTriBalList, dbParams, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     This function user for get table schema of trial
        ///     balance view data by using stored procedure "GL.GetTrialBalanceViewList".
        /// </summary>
        /// <param name="dsTrialBalView"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsTrialBalView, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetTrialBalanceViewList", dsTrialBalView, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Check for return statement is correct or not.
        /// </summary>
        /// <param name="trialBalanceView"></param>
        /// <param name="trialBalanceViewCriteria"></param>
        /// <param name="trialBalanceViewColumn"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool IsValidQuery(DataSet dsTrialBalView, string connStr)
        {
            var trialBalanceViewColumn = new TrialBalanceViewColumn();
            var trialBalanceViewCriteria = new TrialBalanceViewCriteria();

            var view = new DataTable();
            var viewCriteria = new DataTable();
            var viewColumn = new DataTable();

            // Set data to datatable
            view = dsTrialBalView.Tables[TableName];
            viewCriteria = dsTrialBalView.Tables[trialBalanceViewCriteria.TableName];
            viewColumn = dsTrialBalView.Tables[trialBalanceViewColumn.TableName];

            // Return result
            return GnxLib.SqlParse(view, viewCriteria, viewColumn, connStr);
        }

        /// <summary>
        ///     This function is used for commit changed trial balance view data to database.
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsTrialBalView, string connStr)
        {
            var trialBalanceViewCriteria = new TrialBalanceViewCriteria();
            var trialBalanceViewColumn = new TrialBalanceViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(dsTrialBalView, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsTrialBalView, trialBalanceViewCriteria.SelectCommand,
                trialBalanceViewCriteria.TableName);
            dbSaveSource[2] = new DbSaveSource(dsTrialBalView, trialBalanceViewColumn.SelectCommand,
                trialBalanceViewColumn.TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        ///     This function is used for commit deleted trial balance view data to database.
        /// </summary>
        /// <param name="deletedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsTrialBalView, string connStr)
        {
            var trialBalanceViewCriteria = new TrialBalanceViewCriteria();
            var trialBalanceViewColumn = new TrialBalanceViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(dsTrialBalView, trialBalanceViewCriteria.SelectCommand,
                trialBalanceViewCriteria.TableName);
            dbSaveSource[1] = new DbSaveSource(dsTrialBalView, trialBalanceViewColumn.SelectCommand,
                trialBalanceViewColumn.TableName);
            dbSaveSource[2] = new DbSaveSource(dsTrialBalView, SelectCommand, TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsTrialBalance"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetTrialBalancePreveiw(DataSet dsTrialBalance, int userID, string connStr)
        {
            var trialBalanceViewCriteria = new TrialBalanceViewCriteria();
            var dtTrialBalanceView = new DataTable();
            var dtTrialBalanceViewCriteria = new DataTable();
            var trialBalanceView = new TrialBalanceView();
            var trialBalanceViewQuery = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate  query
            trialBalanceViewQuery = new TrialBalanceView().GetTrialBalanceViewQueryPreview(dsTrialBalance, userID,
                connStr);


            // Generate  parameter
            dtTrialBalanceViewCriteria = dsTrialBalance.Tables[trialBalanceViewCriteria.TableName];

            if (dtTrialBalanceViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtTrialBalanceViewCriteria.Rows.Count];

                for (var i = 0; i < dtTrialBalanceViewCriteria.Rows.Count; i++)
                {
                    var dr = dtTrialBalanceViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtTrialBalanceView = trialBalanceView.DbExecuteQuery(trialBalanceViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtTrialBalanceView = trialBalanceView.DbExecuteQuery(trialBalanceViewQuery, null, connStr);
            }

            // Return result
            if (dsTrialBalance.Tables[TableName] != null)
            {
                dsTrialBalance.Tables.Remove(TableName);
            }

            dtTrialBalanceView.TableName = TableName;
            dsTrialBalance.Tables.Add(dtTrialBalanceView);
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetTrialBalanceViewQueryPreview(DataSet dsPreview, int userID, string connStr)
        {
            var trialBalanceViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            if (dsPreview.Tables["TrialBalanceViewColumn"] != null)
            {
                columnList =
                    TrialBalanceViewColumn.GetTrialBalanceViewColumnPreview(dsPreview.Tables["TrialBalanceViewColumn"],
                        connStr);
            }

            // Generate where clause
            if (dsPreview.Tables["TrialBalanceViewCriteria"] != null)
            {
                whereClause = TrialBalanceViewCriteria.GetTrialBalanceViewCriteriaPreview(dsPreview, userID, connStr);
            }


            columnList = columnList + (columnList != string.Empty ? " , " : string.Empty);

            // Generate query
            //trialBalanceViewQuery = "SELECT " + columnList.ToString().Substring(0,columnList.Length - 1) +
            //                          " TrialBalanceCode FROM GL.vGLHisTrialBalance " +
            //                          (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            trialBalanceViewQuery = "SELECT " + columnList.Substring(0, columnList.Length - 2) +
                                    " FROM GL.vGLHisTrialBalance " +
                                    (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);


            return trialBalanceViewQuery;
        }

        /// <summary>
        ///     Get viewlist by userID.
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
            dtView = DbRead("GL.GetTrialBalanceViewListByCreatedBy", dbParams, connStr);

            // Return result
            return dtView;
        }

        #endregion
    }
}