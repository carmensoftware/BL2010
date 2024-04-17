using System;
using System.Data;
using System.Text;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StdJvView : DbHandler
    {
        #region "Attibuties"

        private readonly StdJvViewCols _stdJvViewCols = new StdJvViewCols();
        private readonly StdJvViewCrtr _stdJvViewCrtr = new StdJvViewCrtr();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public StdJvView()
        {
            SelectCommand = "SELECT * FROM GL.StdJvView";
            TableName = "StdJvView";
        }

        /// <summary>
        ///     Get standard voucher view table structure.
        /// </summary>
        /// <param name="dsAccView"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsStdJvView, string strConn)
        {
            return DbRetrieveSchema("GL.GetStdJvViewList", dsStdJvView, null, TableName, strConn);
        }

        /// <summary>
        ///     Get standard voucher view using standard voucher view id
        /// </summary>
        /// <param name="StdJvViewID"></param>
        /// <returns></returns>
        public bool GetStdJvView(int StdJvViewID, DataSet StdJvStdJv, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@StdJvViewID", StdJvViewID.ToString());

            // Get data
            result = DbRetrieve("GL.GetStdJvView", StdJvStdJv, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all standard voucher view depend on user id.
        /// </summary>
        /// <returns></returns>
        public DataTable GetStdJvViewList(int userID, string connStr)
        {
            var dtStdJvView = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@CreatedBy", userID.ToString());

            // Get data
            dtStdJvView = DbRead("GL.GetStdJvViewListByCreatedBy", dbParams, connStr);

            // Return result
            return dtStdJvView;
        }

        /// <summary>
        ///     Get query statement accessing from standardvoucher view
        /// </summary>
        /// <param name="intViewID"></param>
        /// <param name="dbParams"></param>
        /// <param name="strUserName"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetQuery(int intViewID, ref DbParameter[] dbParams, string strUserName, string strConn)
        {
            var sbQuery = new StringBuilder();

            // Initial query string.
            sbQuery.Append("SELECT ");

            // Add column list to query.
            sbQuery.Append(_stdJvViewCols.GetColumn(intViewID, strConn));

            // Add table name to query
            sbQuery.Append(" FROM GL.vStdJvList ");

            // Add where clause to query
            if (GetIsAdvance(intViewID, strConn))
            {
                sbQuery.Append(_stdJvViewCrtr.GetAdvanceCriteria(intViewID, ref dbParams, GetAdvOpt(intViewID, strConn),
                    strConn));
            }
            else
            {
                sbQuery.Append(_stdJvViewCrtr.GetCriteria(intViewID, ref dbParams, strConn));
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
        ///     Get advance criteria option.
        /// </summary>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetIsAdvance(int intViewID, string strConn)
        {
            var dsStdJvView = new DataSet();

            var result = Get(dsStdJvView, intViewID, strConn);

            if (result)
            {
                if (dsStdJvView.Tables[TableName].Rows.Count > 0)
                {
                    return bool.Parse(dsStdJvView.Tables[TableName].Rows[0]["IsAdvance"].ToString());
                }
                return false;
            }
            return false;
        }

        /// <summary>
        ///     Get standarvoucher view data related to specified viewid.
        /// </summary>
        /// <param name="dsAccView"></param>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Get(DataSet dsAccView, int intViewID, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewID", intViewID.ToString());

            return DbRetrieve("GL.GetStdJvView_ViewID", dsAccView, dbParams, TableName, strConn);
        }


        /// <summary>
        ///     Get standard voucher view list ralated to logged in user name.
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public DataTable GetList(string strUserName, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CreatedBy", strUserName);
            return DbRead("GL.GetStdJvViewList_IsPublic_CreatedBy", dbParams, strConn);
        }


        /// <summary>
        ///     Get criteria search scope which is all standardvoucher or only the standardvoucher
        ///     was created by the user ralated to specified viewid.
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
        ///     Get standard voucher new id.
        /// </summary>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public int GetNewID(string strConn)
        {
            return DbReadScalar("GL.GetJVView_NewID", null, strConn);
        }

        /// <summary>
        ///     Save Standard voucher view data changed to database.
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var stdJvViewCol = new StdJvViewCols();
            var stdJvViewCrtr = new StdJvViewCrtr();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, stdJvViewCrtr.SelectCommand, stdJvViewCrtr.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, stdJvViewCol.SelectCommand, stdJvViewCol.TableName);

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
            var StdJvViewCrtr = new StdJvViewCrtr();
            var StdJvViewCols = new StdJvViewCols();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(deletedData, StdJvViewCrtr.SelectCommand, StdJvViewCrtr.TableName);
            dbSaveSource[1] = new DbSaveSource(deletedData, StdJvViewCols.SelectCommand, StdJvViewCols.TableName);
            dbSaveSource[2] = new DbSaveSource(deletedData, SelectCommand, TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}