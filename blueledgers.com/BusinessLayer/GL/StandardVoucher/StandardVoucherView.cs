using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StandardVoucherView : DbHandler
    {
        #region "Attibuties"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public StandardVoucherView()
        {
            SelectCommand = "SELECT * FROM GL.StandardVoucherView";
            TableName = "StandardVoucherView";
        }

        /// <summary>
        ///     Get standardVoucher view schema
        /// </summary>
        /// <param name="standardVoucherViewID"></param>
        /// <returns></returns>
        public bool GetStandardVoucherViewSchema(DataSet standardVoucherViewID, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetStandardVoucherViewList", standardVoucherViewID, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get standard voucher view using standard voucher view id
        /// </summary>
        /// <param name="standardVoucherViewID"></param>
        /// <returns></returns>
        public bool GetStandardVoucherView(int standardVoucherViewID, DataSet standardVoucherStandardVoucher,
            string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@StandardVoucherViewID", standardVoucherViewID.ToString());

            // Get data
            result = DbRetrieve("GL.GetStandardVoucherView", standardVoucherStandardVoucher, dbParams, TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all standard voucher view depend on user id.
        /// </summary>
        /// <returns></returns>
        public DataTable GetStandardVoucherViewList(int userID, string connStr)
        {
            var dtStandardVoucherView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@CreatedBy", userID.ToString());

            // Get data
            dtStandardVoucherView = DbRead("GL.GetStandardVoucherViewListByCreatedBy", dbParams, connStr);

            // Return result
            return dtStandardVoucherView;
        }

        /// <summary>
        ///     Generate query of assigned StandardVoucherViewID
        /// </summary>
        /// <returns></returns>
        public string GetStandardVoucherViewQueryDefault(int standardVoucherViewID, int userID, string connStr)
        {
            var standardVocuherViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            columnList = StandardVoucherViewColumn.GetStandardVoucherViewColumn(standardVoucherViewID, connStr);

            // Generate where clause
            whereClause = StandardVoucherViewCriteria.GetStandardVoucherViewCriteria(standardVoucherViewID, userID,
                connStr);

            // Generate query
            standardVocuherViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                                       " StandardVoucherID FROM GL.vStandardVoucherDefault " +
                                       (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return standardVocuherViewQuery;
        }

        /// <summary>
        ///     Generate query of assigned StandardVoucherViewID
        /// </summary>
        /// <returns></returns>
        public string GetStandardVoucherViewQuery(int standardVoucherViewID, int userID, string connStr)
        {
            var standardVocuherViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            columnList = StandardVoucherViewColumn.GetStandardVoucherViewColumn(standardVoucherViewID, connStr);

            // Generate where clause
            whereClause = StandardVoucherViewCriteria.GetStandardVoucherViewCriteria(standardVoucherViewID, userID,
                connStr);

            // Generate query
            standardVocuherViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                                       " StandardVoucherID FROM GL.vStandardVoucher " +
                                       (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return standardVocuherViewQuery;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetStandardVoucherViewQueryPreview(DataSet dsPreview, int userID, string connStr)
        {
            var standardvoucherViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            if (dsPreview.Tables["StandardVoucherViewColumn"] != null)
            {
                columnList =
                    StandardVoucherViewColumn.GetStandardVoucherViewColumnPreview(
                        dsPreview.Tables["StandardVoucherViewColumn"], connStr);
            }

            // Generate where clause
            if (dsPreview.Tables["StandardVoucherViewCriteria"] != null)
            {
                whereClause = StandardVoucherViewCriteria.GetStandardVoucherViewCriteriaPreview(dsPreview, userID,
                    connStr);
            }

            // Generate query
            //standardvoucherViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
            //                          " StandardVoucherID FROM GL.vStandardVoucher " +
            //                          (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);
            standardvoucherViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                                       " StandardVoucherID FROM GL.vStandardVoucherList " +
                                       (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return standardvoucherViewQuery;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetStandardVoucherViewQueryPreviewDefault(DataSet dsPreview, int userID, string connStr)
        {
            var standardvoucherViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            if (dsPreview.Tables["StandardVoucherViewColumn"] != null)
            {
                columnList =
                    StandardVoucherViewColumn.GetStandardVoucherViewColumnPreview(
                        dsPreview.Tables["StandardVoucherViewColumn"], connStr);
            }

            // Generate where clause
            if (dsPreview.Tables["StandardVoucherViewCriteria"] != null)
            {
                whereClause = StandardVoucherViewCriteria.GetStandardVoucherViewCriteriaPreview(dsPreview, userID,
                    connStr);
            }

            // Generate query
            standardvoucherViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                                       " StandardVoucherID FROM GL.vStandardVoucherDefault " +
                                       (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return standardvoucherViewQuery;
        }

        /// <summary>
        ///     Validate query before save
        /// </summary>
        /// <returns></returns>
        public bool GetStandardVoucherViewIsValidQuery(DataTable standardVoucherView,
            DataTable standardVoucherViewCriteria, DataTable standardVoucherViewColumn, string connStr)
        {
            var standardVoucherViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;
            var result = new DataTable();
            var dbParams = new DbParameter[standardVoucherViewCriteria.DefaultView.Count];
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate Column
            if (standardVoucherViewColumn != null)
            {
                foreach (DataRow dr in standardVoucherViewColumn.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        columnList += (columnList != string.Empty ? "," : string.Empty) +
                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);
                    }
                }
            }

            // Generate Criteria
            if (standardVoucherViewCriteria != null)
            {
                // Non-Advance option
                if (standardVoucherView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in standardVoucherViewCriteria.Rows)
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
                    whereClause = standardVoucherView.Rows[0]["AdvanceOption"].ToString();

                    foreach (DataRow dr in standardVoucherViewCriteria.Rows)
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
                if (standardVoucherViewCriteria.Rows.Count > 0)
                {
                    foreach (DataRow dr in standardVoucherViewCriteria.Rows)
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
            standardVoucherViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                                       " StandardVoucherID FROM GL.vStandardVoucherList" +
                                       (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            // Vlidate query
            result = DbExecuteQuery(standardVoucherViewQuery, dbParams, connStr);

            // Return result
            if (result != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="standardVoucherView"></param>
        /// <param name="standardVoucherViewCriteria"></param>
        /// <param name="standardVoucherViewColumn"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetStandardVoucherViewIsValidQueryDefault(DataTable standardVoucherView,
            DataTable standardVoucherViewCriteria, DataTable standardVoucherViewColumn, string connStr)
        {
            var standardVoucherViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;
            var result = new DataTable();
            var dbParams = new DbParameter[standardVoucherViewCriteria.DefaultView.Count];
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate Column
            if (standardVoucherViewColumn != null)
            {
                foreach (DataRow dr in standardVoucherViewColumn.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        columnList += (columnList != string.Empty ? "," : string.Empty) +
                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);
                    }
                }
            }

            // Generate Criteria
            if (standardVoucherViewCriteria != null)
            {
                // Non-Advance option
                if (standardVoucherView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in standardVoucherViewCriteria.Rows)
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
                    whereClause = standardVoucherView.Rows[0]["AdvanceOption"].ToString();

                    foreach (DataRow dr in standardVoucherViewCriteria.Rows)
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
                if (standardVoucherViewCriteria.Rows.Count > 0)
                {
                    foreach (DataRow dr in standardVoucherViewCriteria.Rows)
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
            standardVoucherViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                                       " StandardVoucherID FROM GL.vStandardVoucherDefault" +
                                       (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            // Vlidate query
            result = DbExecuteQuery(standardVoucherViewQuery, dbParams, connStr);

            // Return result
            if (result != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Get standard voucher view using standard voucher view id
        /// </summary>
        /// <param name="standardVoucherViewID"></param>
        /// <returns></returns>
        public DataTable GetStandardVoucherView(int standardVoucherViewID, string connStr)
        {
            var standardVoucherView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@StandardVoucherViewID", standardVoucherViewID.ToString());

            // Get data
            standardVoucherView = DbRead("GL.GetStandardVoucherView", dbParams, connStr);

            // Return result
            return standardVoucherView;
        }

        /// <summary>
        ///     Get lastest standard voucher view id
        /// </summary>
        /// <returns></returns>
        public int GetStandardVoucherViewMaxID(string connStr)
        {
            var maxAccountViewID = 0;

            // Get data
            maxAccountViewID = DbReadScalar("GL.GetStandardVoucherViewMaxID", null, connStr);

            // Return result
            return maxAccountViewID;
        }

        /// <summary>
        ///     Check for return statement is correct or not.
        /// </summary>
        /// <param name="trialBalanceView"></param>
        /// <param name="trialBalanceViewCriteria"></param>
        /// <param name="trialBalanceViewColumn"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool IsValidQuery(DataSet dsStandardVoucherView, string connStr)
        {
            var standardvoucherViewColumn = new StandardVoucherViewColumn();
            var standardvoucherViewCriteria = new StandardVoucherViewCriteria();

            var view = new DataTable();
            var viewCriteria = new DataTable();
            var viewColumn = new DataTable();

            // Set data to datatable
            view = dsStandardVoucherView.Tables[TableName];
            viewCriteria = dsStandardVoucherView.Tables[standardvoucherViewCriteria.TableName];
            viewColumn = dsStandardVoucherView.Tables[standardvoucherViewColumn.TableName];

            // Return result
            return GnxLib.SqlParse(view, viewCriteria, viewColumn, connStr);
        }


        /// <summary>
        ///     Commit to data base for save
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Save(DataSet dsStandardVoucherView, string connStr)
        {
            var standardVoucherViewCriteria = new StandardVoucherViewCriteria();
            var standardVoucherViewColumn = new StandardVoucherViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(dsStandardVoucherView, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsStandardVoucherView, standardVoucherViewCriteria.SelectCommand,
                standardVoucherViewCriteria.TableName);
            dbSaveSource[2] = new DbSaveSource(dsStandardVoucherView, standardVoucherViewColumn.SelectCommand,
                standardVoucherViewColumn.TableName);

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
            var standardVoucherViewCriteria = new StandardVoucherViewCriteria();
            var standardVoucherViewColumn = new StandardVoucherViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(deletedData, standardVoucherViewCriteria.SelectCommand,
                standardVoucherViewCriteria.TableName);
            dbSaveSource[1] = new DbSaveSource(deletedData, standardVoucherViewColumn.SelectCommand,
                standardVoucherViewColumn.TableName);
            dbSaveSource[2] = new DbSaveSource(deletedData, SelectCommand, TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}