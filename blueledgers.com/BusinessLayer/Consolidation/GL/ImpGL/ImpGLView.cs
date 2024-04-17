using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.GL.ImpGL
{
    public class ImpGLView : DbHandler
    {
        #region "Operations"

        public ImpGLView()
        {
            SelectCommand = "SELECT * FROM GL.ImpGLView";
            TableName = "ImpGLView";
        }

        /// <summary>
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetImpGLViewList(int userID, string connStr)
        {
            var dtImpGLView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@CreatedBy", userID.ToString());

            // Get data
            dtImpGLView = DbRead("GL.GetImpGLViewListByCreatedBy", dbParams, connStr);

            // Return result
            return dtImpGLView;
        }

        /// <summary>
        /// </summary>
        /// <param name="impGLViewID"></param>
        /// <param name="dsImpGLView"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetImpGLView(int impGLViewID, DataSet dsImpGLView, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@impGLViewID", impGLViewID.ToString());

            // Get data
            result = DbRetrieve("GL.GetImpGLView", dsImpGLView, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsImpGLView"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetImpGLViewSchema(DataSet dsImpGLView, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetImpGLViewList", dsImpGLView, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="impGLViewID"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetImpGLViewQuery(int impGLViewID, int userID, string connStr)
        {
            var impGLViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            columnList = ImpGLViewColumn.GetImpGLViewColumn(impGLViewID, connStr);

            // Generate where clause
            whereClause = ImpGLViewCriteria.GetImpGLViewCriteria(impGLViewID, userID, connStr);

            // Generate query
            impGLViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                             " ImpGLCode FROM GL.vImpGL " +
                             (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return impGLViewQuery;
        }


        /// <summary>
        ///     For Preview.
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetImpGLViewQueryPreview(DataSet dsPreview, int userID, string connStr)
        {
            var ImpGLViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            if (dsPreview.Tables["ImpGLViewColumn"] != null)
            {
                columnList = ImpGLViewColumn.GetImpGLViewColumnPreview(dsPreview.Tables["ImpGLViewColumn"], connStr);
            }

            // Generate where clause
            if (dsPreview.Tables["ImpGLViewCriteria"] != null)
            {
                whereClause = ImpGLViewCriteria.GetImpGLViewCriteriaPreview(dsPreview, userID, connStr);
            }

            // Generate query
            ImpGLViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                             " ImpGLCode FROM GL.vImpGL " +
                             (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            return ImpGLViewQuery;
        }

        /// <summary>
        /// </summary>
        /// <param name="impGLViewID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetImpGLView(int impGLViewID, string connStr)
        {
            var impGLView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@impGLViewID", impGLViewID.ToString());

            // Get data
            impGLView = DbRead("GL.GetImpGLView", dbParams, connStr);

            // Return result
            return impGLView;
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetMaxImpGLViewID(string connStr)
        {
            var maxImpGLViewID = 0;

            // Get data
            maxImpGLViewID = DbReadScalar("GL.GetMaxImpGLViewID", null, connStr);

            // Return result
            return maxImpGLViewID;
        }

        /// <summary>
        ///     Validate query before save
        /// </summary>
        /// <returns></returns>
        public bool GetImpGLViewIsValidQuery(DataTable impGLView, DataTable impGLViewCriteria,
            DataTable impGLViewColumn, string connStr)
        {
            var impGLViewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;
            var result = new DataTable();
            var dbParams = new DbParameter[impGLViewCriteria.DefaultView.Count];

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate Column
            if (impGLViewColumn != null)
            {
                foreach (DataRow dr in impGLViewColumn.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        columnList += (columnList != string.Empty ? "," : string.Empty) +
                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);
                    }
                }
            }

            // Generate Criteria
            if (impGLViewCriteria != null)
            {
                // Non-Advance option
                if (impGLView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in impGLViewCriteria.Rows)
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
                    whereClause = impGLView.Rows[0]["AvanceOption"].ToString();

                    foreach (DataRow dr in impGLViewCriteria.Rows)
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
                if (impGLViewCriteria.Rows.Count > 0)
                {
                    foreach (DataRow dr in impGLViewCriteria.Rows)
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
            impGLViewQuery = "SELECT " + columnList + (columnList != string.Empty ? " , " : string.Empty) +
                             " ImpGLCode FROM GL.vImpGL " +
                             (whereClause != string.Empty ? " WHERE " + whereClause : string.Empty);

            // Validate query
            result = DbExecuteQuery(impGLViewQuery, dbParams, connStr);

            // Return result
            if (result != null)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        ///     Get lastest impJV View ID.
        /// </summary>
        /// <returns></returns>
        public int GetimpGLViewMaxID(string connStr)
        {
            var maxImpJVViewID = 0;

            // Get data
            maxImpJVViewID = DbReadScalar("GL.GetImpGLViewMaxID", null, connStr);

            // Return result
            return maxImpJVViewID;
        }

        /// <summary>
        ///     Commit to data base for save
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var impGLViewCriteria = new ImpGLViewCriteria();
            var impGLViewColumn = new ImpGLViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, impGLViewCriteria.SelectCommand, impGLViewCriteria.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, impGLViewColumn.SelectCommand, impGLViewColumn.TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        ///     For delete
        /// </summary>
        /// <param name="dsImpGLView"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsImpGLView, string connStr)
        {
            var impGLViewCriteria = new ImpGLViewCriteria();
            var impGLViewColumn = new ImpGLViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(dsImpGLView, impGLViewCriteria.SelectCommand, impGLViewCriteria.TableName);
            dbSaveSource[1] = new DbSaveSource(dsImpGLView, impGLViewColumn.SelectCommand, impGLViewColumn.TableName);
            dbSaveSource[2] = new DbSaveSource(dsImpGLView, SelectCommand, TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}