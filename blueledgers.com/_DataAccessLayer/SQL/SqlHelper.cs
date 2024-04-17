using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace Blue.DAL.SQL
{
    public class SqlHelper
    {
        #region "Attributies"

        private readonly SqlConnection _sqlConn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnStr"]);


        #endregion

        #region "Operations"

        /// <summary>
        ///     To get date time from database server.
        /// </summary>
        /// <returns></returns>
        public virtual DateTime SqlDate(string connStr)
        {

            var dtbSQLDate = new DataTable();
            var sqlCmd = new SqlCommand
            {
                Connection = (connStr == string.Empty ? _sqlConn : new SqlConnection(connStr)),
                CommandText = "Select Getdate() as SQLDate"
            };

            var sqlDataAdp = new SqlDataAdapter { SelectCommand = sqlCmd };

            try
            {
                sqlDataAdp.Fill(dtbSQLDate);
                return (DateTime)dtbSQLDate.Rows[0]["SQLDate"];
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
            finally
            {
                _sqlConn.Close();
            }
        }

        /// <summary>
        ///     Retrieve data and return in DataTable format.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="sqlParameters"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public virtual DataTable SqlRead(string spName, SqlParameter[] sqlParameters, string connStr)
        {
            var returnTable = new DataTable();
            var sqlCmd = new SqlCommand
            {
                Connection = (connStr == string.Empty ? _sqlConn : new SqlConnection(connStr)),
                CommandText = spName,
                CommandType = CommandType.StoredProcedure
            };

            if (sqlParameters != null)
            {
                for (var i = 0; i < sqlParameters.Length; i++)
                {
                    sqlCmd.Parameters.Add(new SqlParameter(sqlParameters[i].ParameterName, sqlParameters[i].Value));
                }
            }

            var sqlDataAdp = new SqlDataAdapter { SelectCommand = sqlCmd };

            try
            {
                sqlDataAdp.Fill(returnTable);
                return returnTable;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        ///     Read data a single value from first row and first colunm
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="sqlParameters"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public virtual int SqlReadScalar(string spName, SqlParameter[] sqlParameters, string connStr)
        {
            var sqlCmd = new SqlCommand
            {
                Connection = (connStr == string.Empty ? _sqlConn : new SqlConnection(connStr)),
                CommandText = spName,
                CommandType = CommandType.StoredProcedure
            };

            if (sqlParameters != null)
            {
                for (var i = 0; i < sqlParameters.Length; i++)
                {
                    sqlCmd.Parameters.Add(new SqlParameter(sqlParameters[i].ParameterName, sqlParameters[i].Value));
                }
            }

            sqlCmd.Connection.Open();

            try
            {
                return (int)sqlCmd.ExecuteScalar();
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                sqlCmd.Connection.Close();
            }
        }

        /// <summary>
        ///     Prepare resource for commit to datqabase
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="selectCommand"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public virtual DbSaveSource SqlSave(DataSet savedData, string selectCommand, string tableName)
        {
            var rtnDBSaveSource = new DbSaveSource
            {
                SavedData = savedData,
                SelectCommand = selectCommand,
                TableName = tableName
            };

            return rtnDBSaveSource;
        }

        /// <summary>
        ///     Retrieve data from database
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="retrievedData"></param>
        /// <param name="sqlParameters"></param>
        /// <param name="tableName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public virtual bool SqlRetrieve(string spName, DataSet retrievedData, SqlParameter[] sqlParameters,
            string tableName, string connStr)
        {
            var sqlCmd = new SqlCommand
            {
                Connection = (connStr == string.Empty ? _sqlConn : new SqlConnection(connStr)),
                CommandText = spName,
                CommandType = CommandType.StoredProcedure
            };

            if (sqlParameters != null)
            {
                for (var i = 0; i < sqlParameters.Length; i++)
                {
                    sqlCmd.Parameters.Add(new SqlParameter(sqlParameters[i].ParameterName, sqlParameters[i].Value));
                }
            }

            var sqlDataAdp = new SqlDataAdapter { SelectCommand = sqlCmd };

            try
            {
                sqlDataAdp.Fill(retrievedData, tableName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual bool SqlRetrieve(string spName, DataSet retrievedData, SqlParameter[] sqlParameters,
            string tableName, string connStr, ref string errorMessage)
        {
            var sqlCmd = new SqlCommand
            {
                Connection = (connStr == string.Empty ? _sqlConn : new SqlConnection(connStr)),
                CommandText = spName,
                CommandType = CommandType.StoredProcedure
            };

            if (sqlParameters != null)
            {
                for (var i = 0; i < sqlParameters.Length; i++)
                {
                    sqlCmd.Parameters.Add(new SqlParameter(sqlParameters[i].ParameterName, sqlParameters[i].Value));
                }
            }

            var sqlDataAdp = new SqlDataAdapter { SelectCommand = sqlCmd };

            try
            {
                sqlDataAdp.Fill(retrievedData, tableName);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
        /// <summary>
        ///     Retrieve data and structure from database
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="retrievedData"></param>
        /// <param name="sqlParameters"></param>
        /// <param name="tableName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public virtual bool SqlRetrieveSchema(string spName, DataSet retrievedData, SqlParameter[] sqlParameters,
            string tableName, string connStr)
        {
            var sqlCmd = new SqlCommand
            {
                Connection = (connStr == string.Empty ? _sqlConn : new SqlConnection(connStr)),
                CommandText = spName,
                CommandType = CommandType.StoredProcedure
            };

            if (sqlParameters != null)
            {
                for (var i = 0; i < sqlParameters.Length; i++)
                {
                    sqlCmd.Parameters.Add(new SqlParameter(sqlParameters[i].ParameterName, sqlParameters[i].Value));
                }
            }

            var sqlDataAdp = new SqlDataAdapter { SelectCommand = sqlCmd };

            try
            {
                sqlDataAdp.FillSchema(retrievedData, SchemaType.Source, tableName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Insert/Update/Delete data to database
        /// </summary>
        /// <param name="dbSaveSrc"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public virtual bool SqlCommit(DbSaveSource[] dbSaveSrc, string connStr)
        {
            var dbConn = (connStr == string.Empty ? _sqlConn : new SqlConnection(connStr));

            dbConn.Open();

            var dbSaveTrans = dbConn.BeginTransaction();

            try
            {
                foreach (var eachDBSaveSrc in dbSaveSrc)
                {
                    var sqlCommand = new SqlCommand(eachDBSaveSrc.SelectCommand, dbConn, dbSaveTrans);
                    //var sqlDataAdp = new SqlDataAdapter(sqlCommand);
                    var sqlDataAdp = new SqlDataAdapter();
                    sqlDataAdp.SelectCommand = sqlCommand;
                    SqlCommandBuilder scb = new SqlCommandBuilder(sqlDataAdp);

                    sqlDataAdp.Update(eachDBSaveSrc.SavedData, eachDBSaveSrc.TableName);
                }

                dbSaveTrans.Commit();
                return true;
            }
            catch
            {

                dbSaveTrans.Rollback();
                return false;
            }
            finally
            {
                dbConn.Close();
            }
        }

        public virtual bool SqlCommit(ArrayList cmd, ArrayList sqlParams, string connStr)
        {
            var dbConn = (connStr == string.Empty ? _sqlConn : new SqlConnection(connStr));

            dbConn.Open();

            var dbSaveTrans = dbConn.BeginTransaction();

            try
            {
                for (var i = 0; i < cmd.Count; i++)
                {
                    var sqlCmd = new SqlCommand
                    {
                        Connection = dbConn,
                        CommandText = cmd[i].ToString(),
                        Transaction = dbSaveTrans,
                        CommandType = CommandType.Text
                    };

                    if (sqlParams[i] != null)
                    {
                        foreach (var sqlParam in (SqlParameter[])sqlParams[i])
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(sqlParam.ParameterName, sqlParam.Value));
                        }
                    }

                    sqlCmd.ExecuteNonQuery();
                }

                dbSaveTrans.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                dbConn.Close();
            }
        }


        public virtual bool SqlCommit(ArrayList cmd, ArrayList sqlParams, string connStr, ref string error)
        {
            error = string.Empty;
            var dbConn = (connStr == string.Empty ? _sqlConn : new SqlConnection(connStr));

            dbConn.Open();

            var dbSaveTrans = dbConn.BeginTransaction();

            try
            {
                for (var i = 0; i < cmd.Count; i++)
                {
                    var sqlCmd = new SqlCommand
                    {
                        Connection = dbConn,
                        CommandText = cmd[i].ToString(),
                        Transaction = dbSaveTrans,
                        CommandType = CommandType.Text
                    };

                    if (sqlParams[i] != null)
                    {
                        foreach (var sqlParam in (SqlParameter[])sqlParams[i])
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(sqlParam.ParameterName, sqlParam.Value));
                        }
                    }

                    sqlCmd.ExecuteNonQuery();
                }

                dbSaveTrans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
            finally
            {
                dbConn.Close();
            }
        }


        /// <summary>
        ///     Execute select command and fill retrieved data to dataset overwrite the original data.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="retrievedData"></param>
        /// <param name="sqlParameters"></param>
        /// <param name="tableName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public virtual bool SqlExecuteQuery(string cmd, DataSet retrievedData, SqlParameter[] sqlParameters,
            string tableName, string connStr)
        {
            var sqlCmd = new SqlCommand
            {
                Connection = (connStr == string.Empty ? _sqlConn : new SqlConnection(connStr)),
                CommandText = cmd,
                CommandType = CommandType.Text
            };

            if (sqlParameters != null)
            {
                for (var i = 0; i < sqlParameters.Length; i++)
                {
                    sqlCmd.Parameters.Add(new SqlParameter(sqlParameters[i].ParameterName, sqlParameters[i].Value));
                }
            }

            var sqlDataAdp = new SqlDataAdapter { SelectCommand = sqlCmd };

            try
            {
                // Clear exist data in fill table
                //if (retrievedData.Tables.Count > 0)
                //{
                //    if (retrievedData.Tables[tableName] != null)
                //    {
                //        retrievedData.Tables.Remove(tableName);
                //    }
                //}

                sqlDataAdp.Fill(retrievedData, tableName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Execute select command and return datatable.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sqlParameters"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public virtual DataTable SqlExecuteQuery(string cmd, SqlParameter[] sqlParameters, string connStr)
        {
            var returnTable = new DataTable();
            var sqlCmd = new SqlCommand
            {
                Connection = (connStr == string.Empty ? _sqlConn : new SqlConnection(connStr)),
                CommandText = cmd,
                CommandType = CommandType.Text
            };

            if (sqlParameters != null)
            {
                for (var i = 0; i < sqlParameters.Length; i++)
                {
                    sqlCmd.Parameters.Add(new SqlParameter(sqlParameters[i].ParameterName, sqlParameters[i].Value));
                }
            }

            var sqlDataAdp = new SqlDataAdapter { SelectCommand = sqlCmd };

            try
            {
                sqlDataAdp.Fill(returnTable);
                return returnTable;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual DataTable SqlExecuteQuery(string cmd, SqlParameter[] sqlParameters, string connStr, ref string error)
        {
            error = string.Empty;

            var returnTable = new DataTable();
            var sqlCmd = new SqlCommand
            {
                Connection = (connStr == string.Empty ? _sqlConn : new SqlConnection(connStr)),
                CommandText = cmd,
                CommandType = CommandType.Text
            };

            if (sqlParameters != null)
            {
                for (var i = 0; i < sqlParameters.Length; i++)
                {
                    sqlCmd.Parameters.Add(new SqlParameter(sqlParameters[i].ParameterName, sqlParameters[i].Value));
                }
            }

            var sqlDataAdp = new SqlDataAdapter { SelectCommand = sqlCmd };

            try
            {
                sqlDataAdp.Fill(returnTable);
                return returnTable;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        
        /// <summary>
        ///     Execure StoredProcedure without result set return.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sqlParameters"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public virtual bool SqlExecuteNonQuery(string cmd, SqlParameter[] sqlParameters, string connStr)
        {
            var sqlCmd = new SqlCommand
            {
                Connection = (connStr == string.Empty ? _sqlConn : new SqlConnection(connStr)),
                CommandText = cmd,
                CommandType = CommandType.StoredProcedure
            };

            if (sqlParameters != null)
            {
                for (var i = 0; i < sqlParameters.Length; i++)
                {
                    sqlCmd.Parameters.Add(new SqlParameter(sqlParameters[i].ParameterName, sqlParameters[i].Value));
                }
            }

            try
            {
                sqlCmd.Connection.Open();
                sqlCmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                sqlCmd.Connection.Close();
            }
        }

        /// <summary>
        ///     Execute select command and return datatable.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sqlParameters"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public virtual bool SqlParseQuery(string cmd, SqlParameter[] sqlParameters, string connStr)
        {
            var returnTable = new DataTable();
            var sqlCmd = new SqlCommand
            {
                Connection = (connStr == string.Empty ? _sqlConn : new SqlConnection(connStr)),
                CommandText = cmd,
                CommandType = CommandType.Text
            };

            if (sqlParameters != null)
            {
                for (var i = 0; i < sqlParameters.Length; i++)
                {
                    sqlCmd.Parameters.Add(new SqlParameter(sqlParameters[i].ParameterName, sqlParameters[i].Value));
                }
            }

            var sqlDataAdp = new SqlDataAdapter { SelectCommand = sqlCmd };

            try
            {
                sqlDataAdp.Fill(returnTable);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}