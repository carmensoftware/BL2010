using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.GL.ImpGL
{
    public class ImpGL : DbHandler
    {
        /// <summary>
        ///     Empty constructor
        /// </summary>
        public ImpGL()
        {
            SelectCommand = "SELECT * FROM GL.ImpGL";
            TableName = "ImpGL";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsImpGL"></param>
        /// <param name="impGLCode"></param>
        /// <param name="connStr"></param>
        public void GetImpGLListByImpGLCode(DataSet dsImpGL, string impGLCode, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@impGLCode", impGLCode);

            var impGL = new ImpGL();
            impGL.DbRetrieve("GL.GetImpGLListByImpGLCode", dsImpGL, dbParams, TableName, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsImpGL"></param>
        /// <param name="connStr"></param>
        public void GetImpGLList(DataSet dsImpGL, string connStr)
        {
            var impGL = new ImpGL();
            impGL.DbRetrieve("GL.GetImpGLList", dsImpGL, null, TableName, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsImpGL"></param>
        /// <param name="impGLViewID"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetImpGLList(DataSet dsImpGL, int impGLViewID, int userID, string connStr)
        {
            var dtImpGL = new DataTable();
            var dtImpGLViewCriteria = new DataTable();
            var impGL = new ImpGL();
            var impGLViewQuery = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate  query
            impGLViewQuery = new ImpGLView().GetImpGLViewQuery(impGLViewID, userID, connStr);

            // Generate  parameter
            dtImpGLViewCriteria = new ImpGLViewCriteria().GetImpGLViewCriteriaList(impGLViewID, connStr);

            if (dtImpGLViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtImpGLViewCriteria.Rows.Count];

                for (var i = 0; i < dtImpGLViewCriteria.Rows.Count; i++)
                {
                    var dr = dtImpGLViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtImpGL = impGL.DbExecuteQuery(impGLViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtImpGL = impGL.DbExecuteQuery(impGLViewQuery, null, connStr);
            }

            // Return result
            if (dsImpGL.Tables[TableName] != null)
            {
                dsImpGL.Tables.Remove(TableName);
            }

            dtImpGL.TableName = TableName;
            dsImpGL.Tables.Add(dtImpGL);
        }

        /// <summary>
        ///     Get ImpGL Preview.
        /// </summary>
        /// <param name="dsImpGL"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetImpGLPreveiw(DataSet dsImpGL, int userID, string connStr)
        {
            var dtImpGL = new DataTable();
            var dtImpGLViewCriteria = new DataTable();
            var impGL = new ImpGL();
            var ImpGLViewQuery = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate  query
            ImpGLViewQuery = new ImpGLView().GetImpGLViewQueryPreview(dsImpGL, userID, connStr);

            // Generate  parameter
            dtImpGLViewCriteria = dsImpGL.Tables["ImpGLViewCriteria"];

            if (dtImpGLViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtImpGLViewCriteria.Rows.Count];

                for (var i = 0; i < dtImpGLViewCriteria.Rows.Count; i++)
                {
                    var dr = dtImpGLViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtImpGL = impGL.DbExecuteQuery(ImpGLViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtImpGL = impGL.DbExecuteQuery(ImpGLViewQuery, null, connStr);
            }

            // Return result
            if (dsImpGL.Tables[TableName] != null)
            {
                dsImpGL.Tables.Remove(TableName);
            }

            dtImpGL.TableName = TableName;
            dsImpGL.Tables.Add(dtImpGL);
        }


        /// <summary>
        ///     Get impGL table schema.
        /// </summary>
        /// <param name="impGL"></param>
        /// <returns></returns>
        public bool GetImpGLStructure(DataSet dsImpGL, string conStr)
        {
            return DbRetrieveSchema("GL.GetImpGLList", dsImpGL, null, TableName, conStr);
        }


        /// <summary>
        ///     Get max ImpGLCode
        /// </summary>
        /// <returns></returns>
        public int GetImpGLCodeMax(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("GL.GetImpGLMax", null, connStr);

            // If return null value should be one.
            if (result == Convert.ToInt32("0"))
            {
                result = 1;
            }

            // Return result
            return result;
        }


        /// <summary>
        ///     Save to database.
        /// </summary>
        /// <param name="dsImpGL"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsImpGL, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[2];

            var impGLDetail = new ImpGLDetail();

            // Create dbSaveSource

            dbSaveSorce[0] = new DbSaveSource(dsImpGL, SelectCommand, TableName);
            dbSaveSorce[1] = new DbSaveSource(dsImpGL, impGLDetail.SelectCommand, impGLDetail.TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsImpGL"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsImpGL, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[2];

            var impGLDetail = new ImpGLDetail();

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsImpGL, impGLDetail.SelectCommand, impGLDetail.TableName);
            dbSaveSorce[1] = new DbSaveSource(dsImpGL, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }
    }
}