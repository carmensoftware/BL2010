using System;
using System.Data;
using System.Text;
using Blue.DAL;

namespace Blue.BL.GL.Rec
{
    public class RecView : DbHandler
    {
        #region "Attributies"

        private readonly RecViewCols recViewCols = new RecViewCols();
        private readonly RecViewCrtr recViewCrtr = new RecViewCrtr();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public RecView()
        {
            SelectCommand = "SELECT * FROM GL.RecView";
            TableName = "RecView";
        }

        /// <summary>
        ///     Get view data related to specified view id.
        /// </summary>
        /// <param name="dsView"></param>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Get(DataSet dsView, int intViewID, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewID", intViewID.ToString());

            return DbRetrieve("GL.GetRecView_ViewID", dsView, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get advance criteria option.
        /// </summary>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetIsAdvance(int intViewID, string strConn)
        {
            var dsView = new DataSet();

            var result = Get(dsView, intViewID, strConn);

            if (result)
            {
                if (dsView.Tables[TableName].Rows.Count > 0)
                {
                    return bool.Parse(dsView.Tables[TableName].Rows[0]["IsAdvance"].ToString());
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
            var dsView = new DataSet();

            var result = Get(dsView, intViewID, strConn);

            if (result)
            {
                return dsView.Tables[TableName].Rows[0]["AdvOpt"].ToString();
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
            var dsView = new DataSet();

            var result = Get(dsView, intViewID, strConn);

            if (result)
            {
                if (dsView.Tables[TableName].Rows.Count > 0)
                {
                    return dsView.Tables[TableName].Rows[0]["SearchIn"].ToString();
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
            var dsView = new DataSet();

            var result = Get(dsView, intViewID, strConn);

            if (result)
            {
                return bool.Parse(dsView.Tables[TableName].Rows[0]["IsStandard"].ToString());
            }
            return false;
        }

        /// <summary>
        ///     Get view table structure.
        /// </summary>
        /// <param name="dsAccView"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsView, string strConn)
        {
            return DbRetrieveSchema("GL.GetRecViewList", dsView, null, TableName, strConn);
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
            return DbRead("GL.GetRecViewList_IsPublic_CreatedBy", dbParams, strConn);
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
            sbQuery.Append(recViewCols.GetColumn(intViewID, strConn));

            // Add table name to query
            sbQuery.Append(" FROM GL.vReconcile ");

            // Add where clause to query
            if (GetIsAdvance(intViewID, strConn))
            {
                sbQuery.Append(recViewCrtr.GetAdvanceCriteria(intViewID, ref dbParams, GetAdvOpt(intViewID, strConn),
                    strConn));
            }
            else
            {
                sbQuery.Append(recViewCrtr.GetCriteria(intViewID, ref dbParams, strConn));
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
            return DbReadScalar("GL.GetRecView_NewID", null, strConn);
        }

        /// <summary>
        ///     Save view data changed to database.
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var recViewCols = new RecViewCols();
            var recViewCrtr = new RecViewCrtr();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, recViewCrtr.SelectCommand, recViewCrtr.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, recViewCols.SelectCommand, recViewCols.TableName);

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
            var recViewCols = new RecViewCols();
            var recViewCrtr = new RecViewCrtr();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(deletedData, recViewCrtr.SelectCommand, recViewCrtr.TableName);
            dbSaveSource[1] = new DbSaveSource(deletedData, recViewCols.SelectCommand, recViewCols.TableName);
            dbSaveSource[2] = new DbSaveSource(deletedData, SelectCommand, TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}