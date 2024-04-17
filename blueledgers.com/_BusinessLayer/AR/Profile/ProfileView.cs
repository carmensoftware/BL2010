using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ProfileView : DbHandler
    {
        #region "Attibuties"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ProfileView()
        {
            SelectCommand = "SELECT * FROM AR.ProfileView";
            TableName = "ProfileView";
        }

        /// <summary>
        ///     Get debtor view schema
        /// </summary>
        /// <param name="debtorViewID"></param>
        /// <returns></returns>
        public bool GetViewSchema(DataSet dsDebtorView, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("AR.GetProfileViewList", dsDebtorView, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get debtor view using debtor view id
        /// </summary>
        /// <param name="debtorViewID"></param>
        /// <returns></returns>
        public bool GetView(int debtorViewID, DataSet debtordebtor, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@ProfileViewID", debtorViewID.ToString());

            // Get data
            result = DbRetrieve("AR.GetProfileViewByProfileViewID", debtordebtor, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get debtor view using debtor  view id
        /// </summary>
        /// <param name="debtorViewID"></param>
        /// <returns></returns>
        public DataTable GetView(int debtorViewID, string connStr)
        {
            var debtorView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@ProfileViewID", debtorViewID.ToString());

            // Get data
            debtorView = DbRead("AR.GetProfileViewList", dbParams, connStr);

            // Return result
            return debtorView;
        }

        /// <summary>
        ///     Get all debtor view depend on user id.
        /// </summary>
        /// <returns></returns>
        public DataTable GetViewList(int userID, string connStr)
        {
            var dtDebtorView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@CreatedBy", userID.ToString());

            // Get data
            dtDebtorView = DbRead("AR.GetProfileViewByCreatedBy", dbParams, connStr);

            // Return result
            return dtDebtorView;
        }

        /// <summary>
        ///     Generate query of assigned debtorViewID
        /// </summary>
        /// <returns></returns>
        public string GetViewQuery(int debtorViewID, int userID, string connStr)
        {
            var debtorViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            columnList = ProfileViewColumn.GetColumn(debtorViewID, connStr);

            // Generate where clause
            whereClause = ProfileViewCriteria.GetWhereClause(debtorViewID, userID, connStr);

            // Generate query
            debtorViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                              " CustomerCode FROM AR.vProfile " +
                              (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return debtorViewQuery;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetViewQueryPreview(DataSet dsPreview, int userID, string connStr)
        {
            var debtorViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            if (dsPreview.Tables["debtorViewColumn"] != null)
            {
                columnList = ProfileViewColumn.GetPreview(dsPreview.Tables["DebtorViewColumn"], connStr);
            }

            // Generate where clause
            if (dsPreview.Tables["debtorViewCriteria"] != null)
            {
                whereClause = ProfileViewCriteria.GetPreview(dsPreview, userID, connStr);
            }

            // Generate query
            debtorViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                              " CustomerCode FROM AR.vProfile " +
                              (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return debtorViewQuery;
        }

        /// <summary>
        ///     Validate query before save
        /// </summary>
        /// <returns></returns>
        public bool GetViewIsValidQuery(DataTable dtDebtorView, DataTable dtDebtorViewCriteria,
            DataTable dtDebtorViewColumn, string connStr)
        {
            var debtorViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;
            var result = new DataTable();
            var dbParams = new DbParameter[dtDebtorViewCriteria.DefaultView.Count];

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BlueLedger.BL.APP.Field();

            // Generate Column
            if (dtDebtorViewColumn != null)
            {
                foreach (DataRow dr in dtDebtorViewColumn.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        columnList += (columnList != string.Empty ? "," : string.Empty) +
                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);
                    }
                }
            }

            // Generate Criteria
            if (dtDebtorViewCriteria != null)
            {
                // Non-Advance option
                if (dtDebtorView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in dtDebtorViewCriteria.Rows)
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
                    whereClause = dtDebtorView.Rows[0]["AdvanceOption"].ToString();

                    foreach (DataRow dr in dtDebtorViewCriteria.Rows)
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
                if (dtDebtorViewCriteria.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDebtorViewCriteria.Rows)
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
            debtorViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                              " CustomerCode FROM AR.vProfile" +
                              (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            // Vlidate query
            result = DbExecuteQuery(debtorViewQuery, dbParams, connStr);

            // Return result
            if (result != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Get lastest debtor view id
        /// </summary>
        /// <returns></returns>
        public int GetViewMaxID(string connStr)
        {
            var maxDebtorViewID = 0;

            // Get data
            maxDebtorViewID = DbReadScalar("AR.GetProfileViewMaxID", null, connStr);

            // Return result
            return maxDebtorViewID;
        }

        /// <summary>
        ///     Save data
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var debtorViewCriteria = new ProfileViewCriteria();
            var debtorViewColumn = new ProfileViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, debtorViewCriteria.SelectCommand, debtorViewCriteria.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, debtorViewColumn.SelectCommand, debtorViewColumn.TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        ///     Delete data
        /// </summary>
        /// <param name="deletedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet deletedData, string connStr)
        {
            var debtorViewCriteria = new ProfileViewCriteria();
            var debtorViewColumn = new ProfileViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(deletedData, debtorViewCriteria.SelectCommand,
                debtorViewCriteria.TableName);
            dbSaveSource[1] = new DbSaveSource(deletedData, debtorViewColumn.SelectCommand, debtorViewColumn.TableName);
            dbSaveSource[2] = new DbSaveSource(deletedData, SelectCommand, TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}