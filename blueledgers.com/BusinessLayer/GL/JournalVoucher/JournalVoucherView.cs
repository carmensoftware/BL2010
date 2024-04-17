using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class JournalVoucherView : DbHandler
    {
        #region "Attributies"

        public int JournalVoucherViewID { get; set; }

        public string Description { get; set; }

        public bool IsPublic { get; set; }

        public bool IsAll { get; set; }

        public string IsAdvance { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        #endregion

        #region "Operations"

        public JournalVoucherView()
        {
            SelectCommand = "SELECT * FROM GL.JournalVoucherView";
            TableName = "JournalVoucherView";
        }

        /// <summary>
        ///     Get all journalvoucher view depend on user id.
        /// </summary>
        /// <returns></returns>
        public DataTable GetJournalVoucherViewList(int userID, string connStr)
        {
            var dtJournalVoucherView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@CreatedBy", userID.ToString());

            // Get data
            dtJournalVoucherView = DbRead("GL.GetJournalVoucherViewListByCreatedBy", dbParams, connStr);

            // Return result
            return dtJournalVoucherView;
        }

        /// <summary>
        ///     Get journalvoucher view using journalvoucher view id
        /// </summary>
        /// <param name="journalvoucherViewID"></param>
        /// <returns></returns>
        public DataTable GetJournalVoucherView(int journalvoucherViewID, string connStr)
        {
            var journalvoucherView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@journalvoucherViewID", journalvoucherViewID.ToString());

            // Get data
            journalvoucherView = DbRead("GL.GetJournalVoucherView", dbParams, connStr);

            // Return result
            return journalvoucherView;
        }

        /// <summary>
        ///     Get journalvoucher view using journalvoucher view id
        /// </summary>
        /// <param name="journalvoucherViewID"></param>
        /// <returns></returns>
        public bool GetJournalVoucherView(int journalvoucherViewID, DataSet journalvoucherView, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@journalvoucherViewID", journalvoucherViewID.ToString());

            // Get data
            result = DbRetrieve("GL.GetJournalVoucherView", journalvoucherView, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Generate query of assigned JournalVoucherViewID
        /// </summary>
        /// <returns></returns>
        public string GetJournalVoucherViewQuery(int journalvoucherViewID, int userID, string connStr)
        {
            var journalvoucherViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            columnList = JournalVoucherViewColumn.GetJournalVoucherViewColumn(journalvoucherViewID, connStr);

            // Generate where clause
            whereClause = JournalVoucherViewCriteria.GetJournalVoucherViewCriteria(journalvoucherViewID, userID, connStr);

            // Generate query
            journalvoucherViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                                      " JournalVoucherNo FROM GL.vJournalVoucher " +
                                      (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return journalvoucherViewQuery;
        }

        /// <summary>
        ///     Generate query of assigned JournalVoucherViewID
        /// </summary>
        /// <returns></returns>
        public string GetJournalVoucherViewQueryDefault(int journalvoucherViewID, int userID, string connStr)
        {
            var journalvoucherViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            columnList = JournalVoucherViewColumn.GetJournalVoucherViewColumn(journalvoucherViewID, connStr);

            // Generate where clause
            whereClause = JournalVoucherViewCriteria.GetJournalVoucherViewCriteria(journalvoucherViewID, userID, connStr);

            // Generate query
            journalvoucherViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                                      " JournalVoucherNo FROM GL.vJournalVoucherDefault " +
                                      (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return journalvoucherViewQuery;
        }

        /// <summary>
        ///     Generate query of assigned dataset
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetJournalVoucherViewQueryPreview(DataSet dsPreview, int userID, string connStr)
        {
            var journalvoucherViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            if (dsPreview.Tables["JournalVoucherViewColumn"] != null)
            {
                columnList =
                    JournalVoucherViewColumn.GetJournalVoucherViewColumnPreview(
                        dsPreview.Tables["JournalVoucherViewColumn"], connStr);
            }

            // Generate where clause
            if (dsPreview.Tables["JournalVoucherViewCriteria"] != null)
            {
                whereClause = JournalVoucherViewCriteria.GetJournalVoucherViewCriteriaPreview(dsPreview, userID, connStr);
            }

            // Generate query
            //journalvoucherViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
            //                          " JournalVoucherNo FROM GL.vJournalVoucher " +
            //                          (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);
            journalvoucherViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                                      " JournalVoucherNo FROM GL.vJournalVoucherList " +
                                      (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return journalvoucherViewQuery;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetJournalVoucherViewQueryPreviewDefault(DataSet dsPreview, int userID, string connStr)
        {
            var journalvoucherViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            if (dsPreview.Tables["JournalVoucherViewColumn"] != null)
            {
                columnList =
                    JournalVoucherViewColumn.GetJournalVoucherViewColumnPreview(
                        dsPreview.Tables["JournalVoucherViewColumn"], connStr);
            }

            // Generate where clause
            if (dsPreview.Tables["JournalVoucherViewCriteria"] != null)
            {
                whereClause = JournalVoucherViewCriteria.GetJournalVoucherViewCriteriaPreview(dsPreview, userID, connStr);
            }

            // Generate query
            journalvoucherViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                                      " JournalVoucherNo FROM GL.vJournalVoucherDefault " +
                                      (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return journalvoucherViewQuery;
        }

        /// <summary>
        ///     Get journalvoucher view schema
        /// </summary>
        /// <param name="journalvoucherView"></param>
        /// <returns></returns>
        public bool GetJournalVoucherViewSchema(DataSet journalvoucherView, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetJournalVoucherViewList", journalvoucherView, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get lastest journalvoucher view id
        /// </summary>
        /// <returns></returns>
        public int GetMaxJournalVoucherViewID(string connStr)
        {
            var maxJournalVoucherViewID = 0;

            // Get data
            maxJournalVoucherViewID = DbReadScalar("GL.GetMaxJournalVoucherViewID", null, connStr);

            // Return result
            return maxJournalVoucherViewID;
        }

        /// <summary>
        ///     Validate query before save
        /// </summary>
        /// <returns></returns>
        public bool GetJournalVoucherViewIsValidQuery(DataTable journalVoucherView, DataTable journalVoucherViewCriteria,
            DataTable journalVoucherViewColumn, string connStr)
        {
            var journalVoucherViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;
            bool result;
            var dbParams = new DbParameter[2];
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate Column
            if (journalVoucherViewColumn != null)
            {
                foreach (DataRow dr in journalVoucherViewColumn.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        columnList += (columnList != string.Empty ? "," : string.Empty) +
                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);
                    }
                }
            }

            // Generate Criteria
            if (journalVoucherViewCriteria != null)
            {
                // Non-Advance option
                if (journalVoucherView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in journalVoucherViewCriteria.Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            whereClause += (whereClause != string.Empty ? " " : string.Empty) +
                                           field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " + dr["Operator"] +
                                           " '" +
                                           dr["Value"] + "' " + dr["LogicalOp"];
                        }
                    }
                }
                    // Advance option
                else
                {
                    whereClause = journalVoucherView.Rows[0]["AvanceOption"].ToString();

                    foreach (DataRow dr in journalVoucherViewCriteria.Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                                  dr["Operator"] + " '" +
                                                  dr["Value"] + "' ";
                            whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                        }
                    }
                }

                // Generate parameter
                //if (journalVoucherViewCriteria.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in journalVoucherViewCriteria.Rows)
                //    {
                //        if (dr.RowState != DataRowState.Deleted)
                //        {
                //            string paramName    = "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr);
                //            string paramValue   = dr["Value"].ToString();                            

                //            dbParams[(int)dr["SeqNo"] - 1] = new DbParameter(paramName, paramValue);                                 
                //        }
                //    }
                //}
                //else
                //{
                //    dbParams = null;
                //}
            }

            // Generate query
            //journalVoucherViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
            //                          " JournalVoucherNo FROM GL.vJournalVoucher " +
            //                            (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            // Validate query
            dbParams[0] = new DbParameter("@ColumnList", columnList);
            dbParams[1] = new DbParameter("@WhereClause", whereClause);

            result = DbRetrieve("GL.GetJournalVoucherViewValid", new DataSet(), dbParams, journalVoucherView.TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="journalVoucherView"></param>
        /// <param name="journalVoucherViewCriteria"></param>
        /// <param name="journalVoucherViewColumn"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetJournalVoucherViewIsValidQueryDefault(DataTable journalVoucherView,
            DataTable journalVoucherViewCriteria,
            DataTable journalVoucherViewColumn, string connStr)
        {
            var journalVoucherViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;
            var result = new DataTable();
            var dbParams = new DbParameter[journalVoucherViewCriteria.DefaultView.Count];

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate Column
            if (journalVoucherViewColumn != null)
            {
                foreach (DataRow dr in journalVoucherViewColumn.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        columnList += (columnList != string.Empty ? "," : string.Empty) +
                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);
                    }
                }
            }

            // Generate Criteria
            if (journalVoucherViewCriteria != null)
            {
                // Non-Advance option
                if (journalVoucherView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in journalVoucherViewCriteria.Rows)
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
                    whereClause = journalVoucherView.Rows[0]["AvanceOption"].ToString();

                    foreach (DataRow dr in journalVoucherViewCriteria.Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                                  dr["Operator"] + " " +
                                                  "@" + dr["SeqNo"] +
                                                  field.GetFieldName(dr["FieldID"].ToString(), connStr);
                            whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                        }
                    }
                }

                // Generate parameter
                if (journalVoucherViewCriteria.Rows.Count > 0)
                {
                    foreach (DataRow dr in journalVoucherViewCriteria.Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            var paramName = "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr);
                            var paramValue = dr["Value"].ToString();

                            dbParams[(int) dr["SeqNo"] - 1] = new DbParameter(paramName, paramValue);
                        }
                    }
                }
                else
                {
                    dbParams = null;
                }
            }

            // Generate query
            journalVoucherViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                                      " JournalVoucherNo FROM GL.vJournalVoucherDefault " +
                                      (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            // Validate query
            result = DbExecuteQuery(journalVoucherViewQuery, dbParams, connStr);

            // Return result
            if (result != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Commit to data base for save
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var journalVoucherViewCriteria = new JournalVoucherViewCriteria();
            var journalVoucherViewColumn = new JournalVoucherViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, journalVoucherViewCriteria.SelectCommand,
                journalVoucherViewCriteria.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, journalVoucherViewColumn.SelectCommand,
                journalVoucherViewColumn.TableName);

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
            var journalvoucherViewCriteria = new JournalVoucherViewCriteria();
            var journalvoucherViewColumn = new JournalVoucherViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(deletedData, journalvoucherViewCriteria.SelectCommand,
                journalvoucherViewCriteria.TableName);
            dbSaveSource[1] = new DbSaveSource(deletedData, journalvoucherViewColumn.SelectCommand,
                journalvoucherViewColumn.TableName);
            dbSaveSource[2] = new DbSaveSource(deletedData, SelectCommand, TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}