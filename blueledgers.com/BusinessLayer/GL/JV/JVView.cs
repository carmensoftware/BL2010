using System;
using System.Data;
using System.Text;
using Blue.DAL;

namespace Blue.BL.GL.JV
{
    public class JVView : DbHandler
    {
        #region "Attributies"

        private readonly JVViewCols _jvViewCols = new JVViewCols();
        private readonly JVViewCrtr _jvViewCrtr = new JVViewCrtr();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public JVView()
        {
            SelectCommand = "SELECT * FROM GL.JVView";
            TableName = "JVView";
        }

        /// <summary>
        ///     Get ournal voucher view data related to specified view id.
        /// </summary>
        /// <param name="dsAccView"></param>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Get(DataSet dsAccView, int intViewID, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewID", intViewID.ToString());

            return DbRetrieve("GL.GetJVView_ViewID", dsAccView, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get advance criteria option.
        /// </summary>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetIsAdvance(int intViewID, string strConn)
        {
            var dsAccView = new DataSet();

            var result = Get(dsAccView, intViewID, strConn);

            if (result)
            {
                if (dsAccView.Tables[TableName].Rows.Count > 0)
                {
                    return bool.Parse(dsAccView.Tables[TableName].Rows[0]["IsAdvance"].ToString());
                }
                return false;
            }
            return false;
        }

        /// <summary>
        ///     Get creteria in advance option.
        /// </summary>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetAdvOpt(int intViewID, string strConn)
        {
            var dsAccView = new DataSet();

            var result = Get(dsAccView, intViewID, strConn);

            if (result)
            {
                return dsAccView.Tables[TableName].Rows[0]["AdvOpt"].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get criteria search scope which is all journal voucher or only the journal voucher was created by the user ralated
        ///     to specified view id.
        /// </summary>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetSearchIn(int intViewID, string strConn)
        {
            var dsAccView = new DataSet();

            var result = Get(dsAccView, intViewID, strConn);

            if (result)
            {
                if (dsAccView.Tables[TableName].Rows.Count > 0)
                {
                    return dsAccView.Tables[TableName].Rows[0]["SearchIn"].ToString();
                }
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get the view modification permission.
        /// </summary>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetIsStandard(int intViewID, string strConn)
        {
            var dsJVView = new DataSet();

            var result = Get(dsJVView, intViewID, strConn);

            if (result)
            {
                return bool.Parse(dsJVView.Tables[TableName].Rows[0]["IsStandard"].ToString());
            }
            return false;
        }

        /// <summary>
        ///     Get ournal voucher view table structure.
        /// </summary>
        /// <param name="dsAccView"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsAccView, string strConn)
        {
            return DbRetrieveSchema("GL.GetJVViewList", dsAccView, null, TableName, strConn);
        }

        /// <summary>
        ///     Get ournal voucher view list ralated to logged in user name.
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public DataTable GetList(string strUserName, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CreatedBy", strUserName);
            return DbRead("GL.GetJVViewList_IsPublic_CreatedBy", dbParams, strConn);
        }

        /// <summary>
        ///     Get SQL query string related sepcified view id.
        ///     The query was generated from view column and view criteria data.
        /// </summary>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetQuery(int intViewID, ref DbParameter[] dbParams, string strUserName, string strConn)
        {
            var sbQuery = new StringBuilder();

            // Initial query string.
            sbQuery.Append("SELECT ");

            // Add column list to query.
            sbQuery.Append(_jvViewCols.GetColumn(intViewID, strConn));

            // Add table name to query
            sbQuery.Append(" FROM GL.vJVList ");

            // Add where clause to query
            if (GetIsAdvance(intViewID, strConn))
            {
                sbQuery.Append(_jvViewCrtr.GetAdvanceCriteria(intViewID, ref dbParams, GetAdvOpt(intViewID, strConn),
                    strConn));
            }
            else
            {
                sbQuery.Append(_jvViewCrtr.GetCriteria(intViewID, ref dbParams, strConn));
            }

            // Add search scope criteria.
            // Search in all ournal voucher or only ournal voucher was created by the user.
            if (GetSearchIn(intViewID, strConn).ToUpper() != "A")
            {
                // If this query already has no where clause.
                // Add "where" before add the search scope criteria.
                if (sbQuery.ToString().IndexOf("WHERE") < 0)
                {
                    sbQuery.Append(" WHERE CreatedBy=@CreatedBy");
                }
                else
                {
                    sbQuery.Append(" AND CreatedBy=@CreatedBy");
                }

                // Add parameter.
                Array.Resize(ref dbParams, dbParams.Length + 1);
                dbParams[dbParams.Length - 1] = new DbParameter("@CreatedBy", strUserName);
            }

            return sbQuery.ToString();
        }

        /// <summary>
        ///     Get new id.
        /// </summary>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public int GetNewID(string strConn)
        {
            return DbReadScalar("GL.GetJVView_NewID", null, strConn);
        }

        /// <summary>
        ///     Save ournal voucher view data changed to database.
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var jvViewCol = new JVViewCols();
            var jvViewCrtr = new JVViewCrtr();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, jvViewCrtr.SelectCommand, jvViewCrtr.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, jvViewCol.SelectCommand, jvViewCol.TableName);

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
            var jvViewCol = new JVViewCols();
            var jvViewCrtr = new JVViewCrtr();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(deletedData, jvViewCrtr.SelectCommand, jvViewCrtr.TableName);
            dbSaveSource[1] = new DbSaveSource(deletedData, jvViewCol.SelectCommand, jvViewCol.TableName);
            dbSaveSource[2] = new DbSaveSource(deletedData, SelectCommand, TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}