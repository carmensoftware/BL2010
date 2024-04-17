using System;
using System.Data;
using System.Text;
using Blue.DAL;

namespace Blue.BL.GL.TrBal
{
    public class TrBalView : DbHandler
    {
        #region "Attributies"

        private readonly TrBalViewColumn _trBalViewCols = new TrBalViewColumn();
        private readonly TrBalViewCriteria _trBalViewCrtr = new TrBalViewCriteria();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public TrBalView()
        {
            SelectCommand = "SELECT * FROM GL.TrBalView";
            TableName = "TrBalView";
        }

        /// <summary>
        ///     Get trial balance view data related to specified view id.
        /// </summary>
        /// <param name="dsTrBalView"></param>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Get(DataSet dsTrBalView, int intViewID, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewID", intViewID.ToString());

            return DbRetrieve("GL.GetTrBalView_ViewID", dsTrBalView, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get advance criteria option.
        /// </summary>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetIsAdvance(int intViewID, string strConn)
        {
            var dsTrBalView = new DataSet();

            var result = Get(dsTrBalView, intViewID, strConn);

            if (result)
            {
                if (dsTrBalView.Tables[TableName].Rows.Count > 0)
                {
                    return bool.Parse(dsTrBalView.Tables[TableName].Rows[0]["IsAdvance"].ToString());
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
            var dsTrBalView = new DataSet();

            var result = Get(dsTrBalView, intViewID, strConn);

            if (result)
            {
                return dsTrBalView.Tables[TableName].Rows[0]["AdvOpt"].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get criteria search scope which is all account or only the account was created by the user ralated to specified
        ///     view id.
        /// </summary>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetSearchIn(int intViewID, string strConn)
        {
            var dsTrBalView = new DataSet();

            var result = Get(dsTrBalView, intViewID, strConn);

            if (result)
            {
                if (dsTrBalView.Tables[TableName].Rows.Count > 0)
                {
                    return dsTrBalView.Tables[TableName].Rows[0]["SearchIn"].ToString();
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
            var dsTrBalView = new DataSet();

            var result = Get(dsTrBalView, intViewID, strConn);

            if (result)
            {
                return bool.Parse(dsTrBalView.Tables[TableName].Rows[0]["IsStandard"].ToString());
            }
            return false;
        }

        /// <summary>
        ///     Get trial balance view table structure.
        /// </summary>
        /// <param name="dsAccView"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsTrBalView, string strConn)
        {
            return DbRetrieveSchema("[GL].[GetTrBalViewList]", dsTrBalView, null, TableName, strConn);
        }

        /// <summary>
        ///     Get account view list ralated to logged in user name.
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public DataTable GetList(string strUserName, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CreatedBy", strUserName);
            return DbRead("[GL].[GetTrBalViewList_IsPublic_CreatedBy]", dbParams, strConn);
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
            sbQuery.Append(_trBalViewCols.GetColumn(intViewID, strConn));

            // Add table name to query
            sbQuery.Append(" FROM GL.vTrialBalance ");

            // Add where clause to query
            if (GetIsAdvance(intViewID, strConn))
            {
                sbQuery.Append(_trBalViewCrtr.GetAdvanceCriteria(intViewID, ref dbParams, GetAdvOpt(intViewID, strConn),
                    strConn));
            }
            else
            {
                sbQuery.Append(_trBalViewCrtr.GetCriteria(intViewID, ref dbParams, strConn));
            }

            // Add search scope criteria.
            // Search in all account or only account was created by the user.
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
            return DbReadScalar("[GL].[GetTrBalView_NewID]", null, strConn);
        }

        /// <summary>
        ///     Save trial balance view data changed to database.
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var trBalViewCrtr = new TrBalViewCriteria();
            var trBalViewCol = new TrBalViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, trBalViewCrtr.SelectCommand, trBalViewCrtr.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, trBalViewCol.SelectCommand, trBalViewCol.TableName);

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
            var trBalViewCrtr = new TrBalViewCriteria();
            var trBalViewCol = new TrBalViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(deletedData, trBalViewCrtr.SelectCommand, trBalViewCrtr.TableName);
            dbSaveSource[1] = new DbSaveSource(deletedData, trBalViewCol.SelectCommand, trBalViewCol.TableName);
            dbSaveSource[2] = new DbSaveSource(deletedData, SelectCommand, TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}