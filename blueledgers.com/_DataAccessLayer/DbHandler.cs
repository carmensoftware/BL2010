using System.Collections;
//using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Blue.DAL.SQL;


// ReSharper disable once CheckNamespace
namespace Blue.DAL
{
    public class DbHandler
    {
        #region "Attributies"

        // ReSharper disable once CSharpWarnings::CS0618
        //private readonly string _dbType = ConfigurationSettings.AppSettings["DBType"];
        private readonly string _dbType = "SQL";
        private readonly SqlHelper _sqlHelper = new SqlHelper();

        public string TableName { get; set; }
        public string SelectCommand { get; set; }

        #endregion

        #region "Operations"

        /// <summary>
        ///     The function is used for read data by searh parameters from data base server (working with BLCntDb).
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public virtual DataTable DbRead(string spName, DbParameter[] dbParameters)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    if (dbParameters != null)
                    {
                        var sqlParam = new SqlParameter[dbParameters.Length];

                        for (var i = 0; i < dbParameters.Length; i++)
                        {
                            sqlParam[i] = new SqlParameter(dbParameters[i].ParameterName, dbParameters[i].ParameterValue);
                        }

                        return _sqlHelper.SqlRead(spName, sqlParam, string.Empty);
                    }
                    return _sqlHelper.SqlRead(spName, null, string.Empty);
            }

            return null;
        }

        /// <summary>
        ///     The function is used for read data by searh parameters from data base server.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="dbParameters"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public virtual DataTable DbRead(string spName, DbParameter[] dbParameters, string connStr)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    if (dbParameters != null)
                    {
                        var sqlParam = new SqlParameter[dbParameters.Length];

                        for (var i = 0; i < dbParameters.Length; i++)
                        {
                            sqlParam[i] = new SqlParameter(dbParameters[i].ParameterName, dbParameters[i].ParameterValue);
                        }

                        return _sqlHelper.SqlRead(spName, sqlParam, connStr);
                    }
                    return _sqlHelper.SqlRead(spName, null, connStr);
            }

            return null;
        }

        /// <summary>
        ///     The function is used for read data a single value from first row and first colunm by search parameters (working
        ///     with BLCntDb).
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public virtual int DbReadScalar(string spName, DbParameter[] dbParameters)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    if (dbParameters != null)
                    {
                        var sqlParam = new SqlParameter[dbParameters.Length];

                        for (var i = 0; i < dbParameters.Length; i++)
                        {
                            sqlParam[i] = new SqlParameter(dbParameters[i].ParameterName, dbParameters[i].ParameterValue);
                        }

                        return _sqlHelper.SqlReadScalar(spName, sqlParam, string.Empty);
                    }
                    return _sqlHelper.SqlReadScalar(spName, null, string.Empty);
            }

            return 0;
        }

        /// <summary>
        ///     The function is used for read data a single value from first row and first colunm by search parameters.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="dbParameters"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public virtual int DbReadScalar(string spName, DbParameter[] dbParameters, string connStr)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    if (dbParameters != null)
                    {
                        var sqlParam = new SqlParameter[dbParameters.Length];

                        for (var i = 0; i < dbParameters.Length; i++)
                        {
                            sqlParam[i] = new SqlParameter(dbParameters[i].ParameterName, dbParameters[i].ParameterValue);
                        }

                        return _sqlHelper.SqlReadScalar(spName, sqlParam, connStr);
                    }
                    return _sqlHelper.SqlReadScalar(spName, null, connStr);
            }

            return 0;
        }

        /// <summary>
        ///     The function is used for save data in data base server.
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="selectCommand"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public virtual DbSaveSource DbSave(DataSet savedData, string selectCommand, string tableName)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    return _sqlHelper.SqlSave(savedData, selectCommand, tableName);
            }

            return null;
        }


        public virtual bool DbRetrieve(string spName, DataSet retrievedData, DbParameter[] dbParameters, string tableName, string connStr)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    if (dbParameters != null)
                    {
                        var sqlParam = new SqlParameter[dbParameters.Length];

                        for (var i = 0; i < dbParameters.Length; i++)
                        {
                            sqlParam[i] = new SqlParameter(dbParameters[i].ParameterName, dbParameters[i].ParameterValue);
                        }

                        return _sqlHelper.SqlRetrieve(spName, retrievedData, sqlParam, tableName, connStr);
                    }
                    return _sqlHelper.SqlRetrieve(spName, retrievedData, null, tableName, connStr);
            }

            return false;
        }

        public virtual bool DbRetrieve(string spName, DataSet retrievedData, DbParameter[] dbParameters, string tableName)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    if (dbParameters != null)
                    {
                        var sqlParam = new SqlParameter[dbParameters.Length];

                        for (var i = 0; i < dbParameters.Length; i++)
                        {
                            sqlParam[i] = new SqlParameter(dbParameters[i].ParameterName, dbParameters[i].ParameterValue);
                        }

                        return _sqlHelper.SqlRetrieve(spName, retrievedData, sqlParam, tableName, string.Empty);
                    }
                    return _sqlHelper.SqlRetrieve(spName, retrievedData, null, tableName, string.Empty);
            }

            return false;
        }

        public virtual bool DbRetrieve(string spName, DataSet retrievedData, DbParameter[] dbParameters, string tableName, ref string errorMessage)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    if (dbParameters != null)
                    {
                        var sqlParam = new SqlParameter[dbParameters.Length];

                        for (var i = 0; i < dbParameters.Length; i++)
                        {
                            sqlParam[i] = new SqlParameter(dbParameters[i].ParameterName, dbParameters[i].ParameterValue);
                        }

                        return _sqlHelper.SqlRetrieve(spName, retrievedData, sqlParam, tableName, string.Empty, ref errorMessage);
                    }
                    return _sqlHelper.SqlRetrieve(spName, retrievedData, null, tableName, string.Empty, ref errorMessage);
            }

            return false;
        }



        /// <summary>
        ///     The function is used for retrieve data and struture from data base server (working with BLCntDb).
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="retrievedData"></param>
        /// <param name="dbParameters"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public virtual bool DbRetrieveSchema(string spName, DataSet retrievedData, DbParameter[] dbParameters,
            string tableName)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    if (dbParameters != null)
                    {
                        var sqlParam = new SqlParameter[dbParameters.Length];

                        for (var i = 0; i < dbParameters.Length; i++)
                        {
                            sqlParam[i] = new SqlParameter(dbParameters[i].ParameterName, dbParameters[i].ParameterValue);
                        }

                        return _sqlHelper.SqlRetrieveSchema(spName, retrievedData, sqlParam, tableName, string.Empty);
                    }
                    return _sqlHelper.SqlRetrieveSchema(spName, retrievedData, null, tableName, string.Empty);
            }

            return false;
        }

        /// <summary>
        ///     The function is used for retrieve data and struture from data base server.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="retrievedData"></param>
        /// <param name="dbParameters"></param>
        /// <param name="tableName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public virtual bool DbRetrieveSchema(string spName, DataSet retrievedData, DbParameter[] dbParameters,
            string tableName, string connStr)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    if (dbParameters != null)
                    {
                        var sqlParam = new SqlParameter[dbParameters.Length];

                        for (var i = 0; i < dbParameters.Length; i++)
                        {
                            sqlParam[i] = new SqlParameter(dbParameters[i].ParameterName, dbParameters[i].ParameterValue);
                        }

                        return _sqlHelper.SqlRetrieveSchema(spName, retrievedData, sqlParam, tableName, connStr);
                    }
                    return _sqlHelper.SqlRetrieveSchema(spName, retrievedData, null, tableName, connStr);
            }

            return false;
        }

        /// <summary>
        ///     The function is used to Insert/Update/Delete data to data base server (working with BLCntDb).
        /// </summary>
        /// <param name="dbSaveScr"></param>
        /// <returns></returns>
        public virtual bool DbCommit(DbSaveSource[] dbSaveScr)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    return _sqlHelper.SqlCommit(dbSaveScr, string.Empty);
            }

            return false;
        }

        /// <summary>
        ///     The function is used to Insert/Update/Delete data to data base server.
        /// </summary>
        /// <param name="dbSaveScr"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public virtual bool DbCommit(DbSaveSource[] dbSaveScr, string connStr)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    return _sqlHelper.SqlCommit(dbSaveScr, connStr);
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cmdParams"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public virtual bool DbCommit(ArrayList cmd, ArrayList cmdParams, string connStr)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    {
                        var sqlParams = new ArrayList();

                        // Convert DbParameter to SqlParameter
                        for (var i = 0; i < cmdParams.Count; i++)
                        {
                            var dbParams = (DbParameter[])cmdParams[i];
                            var sqlParam = new SqlParameter[dbParams.Length];

                            for (var j = 0; j < dbParams.Length; j++)
                            {
                                sqlParam[j] = new SqlParameter(dbParams[j].ParameterName, dbParams[j].ParameterValue);
                            }

                            sqlParams.Add(sqlParam);
                        }

                        return _sqlHelper.SqlCommit(cmd, sqlParams, connStr);
                    }
            }

            return false;
        }

        public virtual bool DbCommit(ArrayList cmd, ArrayList cmdParams, string connStr, ref string error)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    {
                        var sqlParams = new ArrayList();

                        // Convert DbParameter to SqlParameter
                        for (var i = 0; i < cmdParams.Count; i++)
                        {
                            var dbParams = (DbParameter[])cmdParams[i];
                            var sqlParam = new SqlParameter[dbParams.Length];

                            for (var j = 0; j < dbParams.Length; j++)
                            {
                                sqlParam[j] = new SqlParameter(dbParams[j].ParameterName, dbParams[j].ParameterValue);
                            }

                            sqlParams.Add(sqlParam);
                        }

                        return _sqlHelper.SqlCommit(cmd, sqlParams, connStr, ref error);
                    }
            }

            return false;
        }


        /// <summary>
        ///     Execute select command and return datatable (working with BLCntDb).
        /// </summary>
        /// <param name="cmd">select command parameter</param>
        /// <param name="dbParameters">parameter value</param>
        /// <returns></returns>
        public virtual DataTable DbExecuteQuery(string cmd, DbParameter[] dbParameters)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    if (dbParameters != null)
                    {
                        var sqlParam = new SqlParameter[dbParameters.Length];

                        for (var i = 0; i < dbParameters.Length; i++)
                        {
                            sqlParam[i] = new SqlParameter(dbParameters[i].ParameterName, dbParameters[i].ParameterValue);
                        }

                        return _sqlHelper.SqlExecuteQuery(cmd, sqlParam, string.Empty);
                    }
                    return _sqlHelper.SqlExecuteQuery(cmd, null, string.Empty);
            }

            return null;
        }

        /// <summary>
        ///     Execute select command and return datatable.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="dbParameters"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public virtual DataTable DbExecuteQuery(string cmd, DbParameter[] dbParameters, string connStr)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    if (dbParameters != null)
                    {
                        var sqlParam = new SqlParameter[dbParameters.Length];

                        for (var i = 0; i < dbParameters.Length; i++)
                        {
                            sqlParam[i] = new SqlParameter(dbParameters[i].ParameterName, dbParameters[i].ParameterValue);
                        }

                        return _sqlHelper.SqlExecuteQuery(cmd, sqlParam, connStr);
                    }
                    return _sqlHelper.SqlExecuteQuery(cmd, null, connStr);
            }

            return null;
        }

        public virtual DataTable DbExecuteQuery(string cmd, DbParameter[] dbParameters, string connStr, ref string error)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    if (dbParameters != null)
                    {
                        var sqlParam = new SqlParameter[dbParameters.Length];

                        for (var i = 0; i < dbParameters.Length; i++)
                        {
                            sqlParam[i] = new SqlParameter(dbParameters[i].ParameterName, dbParameters[i].ParameterValue);
                        }

                        return _sqlHelper.SqlExecuteQuery(cmd, sqlParam, connStr);
                    }
                    return _sqlHelper.SqlExecuteQuery(cmd, null, connStr, ref error);
            }

            return null;
        }


        /// <summary>
        ///     Execure StoredProcedure without result set return (no connection string required).
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public virtual bool DbExecuteNonQuery(string cmd, DbParameter[] dbParameters)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    if (dbParameters != null)
                    {
                        var sqlParam = new SqlParameter[dbParameters.Length];

                        for (var i = 0; i < dbParameters.Length; i++)
                        {
                            sqlParam[i] = new SqlParameter(dbParameters[i].ParameterName, dbParameters[i].ParameterValue);
                        }

                        return _sqlHelper.SqlExecuteNonQuery(cmd, sqlParam, string.Empty);
                    }
                    return _sqlHelper.SqlExecuteNonQuery(cmd, null, string.Empty);
            }

            return false;
        }

        /// <summary>
        ///     Execure StoredProcedure without result set return (connection string required).
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="dbParameters"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public virtual bool DbExecuteNonQuery(string cmd, DbParameter[] dbParameters, string connStr)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    if (dbParameters != null)
                    {
                        var sqlParam = new SqlParameter[dbParameters.Length];

                        for (var i = 0; i < dbParameters.Length; i++)
                        {
                            sqlParam[i] = new SqlParameter(dbParameters[i].ParameterName, dbParameters[i].ParameterValue);
                        }

                        return _sqlHelper.SqlExecuteNonQuery(cmd, sqlParam, connStr);
                    }
                    return _sqlHelper.SqlExecuteNonQuery(cmd, null, connStr);
            }

            return false;
        }

        /// <summary>
        ///     Execute select command and fill retrieved data to dataset overwrite the original data.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="retrievedData"></param>
        /// <param name="dbParameters"></param>
        /// <param name="tableName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public virtual bool DbExecuteQuery(string cmd, DataSet retrievedData, DbParameter[] dbParameters,
            string tableName, string connStr)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    if (dbParameters != null)
                    {
                        var sqlParam = new SqlParameter[dbParameters.Length];

                        for (var i = 0; i < dbParameters.Length; i++)
                        {
                            sqlParam[i] = new SqlParameter(dbParameters[i].ParameterName, dbParameters[i].ParameterValue);
                        }

                        return _sqlHelper.SqlExecuteQuery(cmd, retrievedData, sqlParam, tableName, connStr);
                    }
                    return _sqlHelper.SqlExecuteQuery(cmd, retrievedData, null, tableName, connStr);
            }

            return false;
        }

        /// <summary>
        ///     Execute select command and return datatable.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="dbParameters"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public virtual bool DbParseQuery(string cmd, DbParameter[] dbParameters, string connStr)
        {
            switch (_dbType.ToUpper().Trim())
            {
                case "SQL":
                    if (dbParameters != null)
                    {
                        var sqlParam = new SqlParameter[dbParameters.Length];

                        for (var i = 0; i < dbParameters.Length; i++)
                        {
                            sqlParam[i] = new SqlParameter(dbParameters[i].ParameterName, dbParameters[i].ParameterValue);
                        }

                        return _sqlHelper.SqlParseQuery(cmd, sqlParam, connStr);
                    }
                    return _sqlHelper.SqlParseQuery(cmd, null, connStr);
            }

            return false;
        }

        #endregion
    }
}