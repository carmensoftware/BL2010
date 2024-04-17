using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StandardVoucher : DbHandler
    {
        #region "Attributies"

        private readonly StandardVoucherDetail _standardVoucherDetail = new StandardVoucherDetail();
        private readonly StandardVoucherMisc _standardVoucherMisc = new StandardVoucherMisc();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public StandardVoucher()
        {
            SelectCommand = "SELECT * FROM GL.StandardVoucher";
            TableName = "StandardVoucher";
        }

        /// <summary>
        ///     Get standard voucher using id
        /// </summary>
        /// <param name="standardVoucherID"></param>
        /// <returns></returns>
        public DataTable GetStandardVoucher(int standardVoucherID, string connStr)
        {
            var dtStandardVoucher = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@StandardVoucherID", standardVoucherID.ToString());

            // Get data
            dtStandardVoucher = DbRead("GL.GetStandardVoucher", dbParams, connStr);

            // Return result
            return dtStandardVoucher;
        }

        /// <summary>
        ///     Get standard voucher using id
        /// </summary>
        /// <param name="standardVoucherID"></param>
        /// <returns></returns>
        public bool GetStandardVoucher(int standardVoucherID, DataSet dsStandardVoucher, string connStr)
        {
            var result = false;
            var dtStandardVoucher = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@StandardVoucherID", standardVoucherID.ToString());

            // Get data
            result = DbRetrieve("GL.GetStandardVoucher", dsStandardVoucher, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get recurring period
        /// </summary>
        /// <param name="periodID"></param>
        /// <returns></returns>
        public DataTable GetStandardVoucherRecurringPeriod(int periodID, string connStr)
        {
            var dtRecurringPeriod = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@PeriodID", periodID.ToString());

            // Get data
            dtRecurringPeriod = DbRead("GL.GetStandardVoucherRecurringPeriod", dbParams, connStr);

            // Return result
            return dtRecurringPeriod;
        }

        /// <summary>
        ///     Get all of standard voucher except recurring type
        /// </summary>
        /// <param name="dsStandardVoucher"></param>
        /// <returns></returns>
        public bool GetStandardVoucherForPopup(DataSet dsStandardVoucher, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetStandardVoucherForPopup", dsStandardVoucher, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     return datatable and get searching match data.
        /// </summary>
        /// <param name="Search_Param"></param>
        /// <returns></returns>
        public DataTable GetStandardVoucherListSearch(string keyWord, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@KeyWord", keyWord);

            return DbRead("GL.GetStandardVoucherListSearch", dbParams, connStr);
        }

        /// <summary>
        ///     Get standard voucher using account view id
        /// </summary>
        /// <param name="dsAccount"></param>
        public void GetStandardVoucherListDefault(DataSet dsStandardVoucher, int standardVoucherViewID, int userID,
            string connStr)
        {
            var dtStandardVoucher = new DataTable();
            var dtStandardVoucherViewCriteria = new DataTable();
            var standardVoucher = new StandardVoucher();
            var standardVoucherViewQuery = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate query
            standardVoucherViewQuery =
                new StandardVoucherView().GetStandardVoucherViewQueryDefault(standardVoucherViewID, userID, connStr);

            // Generate parameter
            dtStandardVoucherViewCriteria =
                new StandardVoucherViewCriteria().GetStandardVoucherViewCriteriaList(standardVoucherViewID, connStr);

            if (dtStandardVoucherViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtStandardVoucherViewCriteria.Rows.Count];

                for (var i = 0; i < dtStandardVoucherViewCriteria.Rows.Count; i++)
                {
                    var dr = dtStandardVoucherViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtStandardVoucher = standardVoucher.DbExecuteQuery(standardVoucherViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtStandardVoucher = standardVoucher.DbExecuteQuery(standardVoucherViewQuery, null, connStr);
            }

            // Return resutl
            if (dsStandardVoucher.Tables[TableName] != null)
            {
                dsStandardVoucher.Tables.Remove(TableName);
            }

            dtStandardVoucher.TableName = TableName;
            dsStandardVoucher.Tables.Add(dtStandardVoucher);
        }

        /// <summary>
        ///     Get standard voucher using account view id
        /// </summary>
        /// <param name="dsAccount"></param>
        public void GetStandardVoucherList(DataSet dsStandardVoucher, int standardVoucherViewID, int userID,
            string connStr)
        {
            var dtStandardVoucher = new DataTable();
            var dtStandardVoucherViewCriteria = new DataTable();
            var standardVoucher = new StandardVoucher();
            var standardVoucherViewQuery = string.Empty;

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate query
            standardVoucherViewQuery = new StandardVoucherView().GetStandardVoucherViewQuery(standardVoucherViewID,
                userID, connStr);

            // Generate parameter
            dtStandardVoucherViewCriteria =
                new StandardVoucherViewCriteria().GetStandardVoucherViewCriteriaList(standardVoucherViewID, connStr);

            if (dtStandardVoucherViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtStandardVoucherViewCriteria.Rows.Count];

                for (var i = 0; i < dtStandardVoucherViewCriteria.Rows.Count; i++)
                {
                    var dr = dtStandardVoucherViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtStandardVoucher = standardVoucher.DbExecuteQuery(standardVoucherViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtStandardVoucher = standardVoucher.DbExecuteQuery(standardVoucherViewQuery, null, connStr);
            }

            // Return resutl
            if (dsStandardVoucher.Tables[TableName] != null)
            {
                dsStandardVoucher.Tables.Remove(TableName);
            }

            dtStandardVoucher.TableName = TableName;
            dsStandardVoucher.Tables.Add(dtStandardVoucher);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsStandardVoucher"></param>
        /// <param name="standardVoucherViewID"></param>
        /// <param name="connStr"></param>
        public bool GetStandardVoucherList(DataSet dsStandardVoucher, int standardVoucherViewID, string connStr)
        {
            var result = false;

            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@StandardVoucherViewID", Convert.ToString(standardVoucherViewID));

            // Get data
            result = DbRetrieve("GL.GetStandardVoucherGenerateColumnList", dsStandardVoucher, dbParams, TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all data
        /// </summary>
        /// <param name="dsStandardVoucher"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetStandardVoucherListAll(DataSet dsStandardVoucher, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetStandardVoucherList", dsStandardVoucher, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get StandardVoucher Preview.
        /// </summary>
        /// <param name="dsStandardVoucher"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetStandardVoucherPreveiw(DataSet dsStandardVoucher, int userID, string connStr)
        {
            var dtStandardVoucher = new DataTable();
            var dtStandardVoucherViewCriteria = new DataTable();
            var standardVoucher = new StandardVoucher();
            var standardvoucherViewQuery = string.Empty;

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate  query
            standardvoucherViewQuery = new StandardVoucherView().GetStandardVoucherViewQueryPreview(dsStandardVoucher,
                userID, connStr);

            // Generate  parameter
            dtStandardVoucherViewCriteria = dsStandardVoucher.Tables["StandardVoucherViewCriteria"];

            if (dtStandardVoucherViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtStandardVoucherViewCriteria.Rows.Count];

                for (var i = 0; i < dtStandardVoucherViewCriteria.Rows.Count; i++)
                {
                    var dr = dtStandardVoucherViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtStandardVoucher = standardVoucher.DbExecuteQuery(standardvoucherViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtStandardVoucher = standardVoucher.DbExecuteQuery(standardvoucherViewQuery, null, connStr);
            }

            // Return result
            if (dsStandardVoucher.Tables[TableName] != null)
            {
                dsStandardVoucher.Tables.Remove(TableName);
            }

            dtStandardVoucher.TableName = TableName;
            dsStandardVoucher.Tables.Add(dtStandardVoucher);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsStandardVoucher"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetStandardVoucherPreveiwDefault(DataSet dsStandardVoucher, int userID, string connStr)
        {
            var dtStandardVoucher = new DataTable();
            var dtStandardVoucherViewCriteria = new DataTable();
            var standardVoucher = new StandardVoucher();
            var standardvoucherViewQuery = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate  query
            standardvoucherViewQuery =
                new StandardVoucherView().GetStandardVoucherViewQueryPreviewDefault(dsStandardVoucher, userID, connStr);

            // Generate  parameter
            dtStandardVoucherViewCriteria = dsStandardVoucher.Tables["StandardVoucherViewCriteria"];

            if (dtStandardVoucherViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtStandardVoucherViewCriteria.Rows.Count];

                for (var i = 0; i < dtStandardVoucherViewCriteria.Rows.Count; i++)
                {
                    var dr = dtStandardVoucherViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtStandardVoucher = standardVoucher.DbExecuteQuery(standardvoucherViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtStandardVoucher = standardVoucher.DbExecuteQuery(standardvoucherViewQuery, null, connStr);
            }

            // Return result
            if (dsStandardVoucher.Tables[TableName] != null)
            {
                dsStandardVoucher.Tables.Remove(TableName);
            }

            dtStandardVoucher.TableName = TableName;
            dsStandardVoucher.Tables.Add(dtStandardVoucher);
        }

        /// <summary>
        ///     Get all of standard voucher except recurring type
        /// </summary>
        /// <param name="dsStandardVoucher"></param>
        /// <returns></returns>
        public bool GetStandardVoucherList(DataSet dsStandardVoucher, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetStandardVoucherForPopup", dsStandardVoucher, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsStandardVoucher"></param>
        /// <returns></returns>
        public bool Save(DataSet dsStandardVoucher, string connStr)
        {
            var standardVoucherMisc = new StandardVoucherMisc();
            //StandardVoucherComment      standardVoucherComment    = new StandardVoucherComment();
            //StandardVoucherAttachment   standardVoucherAttachment = new StandardVoucherAttachment();
            //StandardVoucherActiveLog    standardVoucherActiveLog  = new StandardVoucherActiveLog();

            var result = false;
            var dbSaveSorce = new DbSaveSource[3];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsStandardVoucher, SelectCommand, TableName);
            dbSaveSorce[1] = new DbSaveSource(dsStandardVoucher, standardVoucherMisc.SelectCommand,
                standardVoucherMisc.TableName);
            dbSaveSorce[2] = new DbSaveSource(dsStandardVoucher, _standardVoucherDetail.SelectCommand,
                _standardVoucherDetail.TableName);
            //dbSaveSorce[3]             = new DbSaveSource(dsStandardVoucher, standardVoucherComment.SelectCommand, standardVoucherComment.TableName);
            //dbSaveSorce[4]             = new DbSaveSource(dsStandardVoucher, standardVoucherAttachment.SelectCommand, standardVoucherAttachment.TableName);
            //dbSaveSorce[5]             = new DbSaveSource(dsStandardVoucher, standardVoucherActiveLog.SelectCommand, standardVoucherActiveLog.TableName);


            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Delete from database
        /// </summary>
        /// <param name="dsStandardVoucher"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsStandardVoucher, string connStr)
        {
            var result = false;
            var standardVoucherComment = new StandardVoucherComment();
            //StandardVoucherAttachment standardVoucherAttachment = new StandardVoucherAttachment();
            //StandardVoucherActiveLog standardVoucherActLog      = new StandardVoucherActiveLog();
            var dbSaveSorce = new DbSaveSource[3];

            // Create dbSaveSource            
            dbSaveSorce[0] = new DbSaveSource(dsStandardVoucher, _standardVoucherDetail.SelectCommand,
                _standardVoucherDetail.TableName);
            dbSaveSorce[1] = new DbSaveSource(dsStandardVoucher, _standardVoucherMisc.SelectCommand,
                _standardVoucherMisc.TableName);
            //dbSaveSorce[2] = new DbSaveSource(dsStandardVoucher, standardVoucherAttachment.SelectCommand, standardVoucherAttachment.TableName);
            //dbSaveSorce[3] = new DbSaveSource(dsStandardVoucher, standardVoucherComment.SelectCommand, standardVoucherComment.TableName);
            //dbSaveSorce[4] = new DbSaveSource(dsStandardVoucher, standardVoucherActLog.SelectCommand, standardVoucherActLog.TableName);
            dbSaveSorce[2] = new DbSaveSource(dsStandardVoucher, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get standard voucher database schema
        /// </summary>
        /// <param name="dsStandardVoucher"></param>
        /// <returns></returns>
        public bool GetStandardVoucherStructure(DataSet dsStandardVoucher, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetStandardVoucherList", dsStandardVoucher, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get max standard voucher id
        /// </summary>
        /// <returns></returns>
        public int GetStandardVoucherMaxID(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("GL.GetStandardVocherMaxID", null, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsStandardVoucher"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetStandardVoucherRecurringServerAgentStatus(DataSet dsStandardVoucher, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetStandardVoucherRecurringServerAgentStatus", dsStandardVoucher, null, TableName,
                connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get status running or stopped.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetStandardVoucherRecurringServerAgentStatus(string connStr)
        {
            string status;
            var dsTmp = new DataSet();

            DbRetrieve("GL.GetStandardVoucherRecurringServerAgentStatus", dsTmp, null, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                status = dsTmp.Tables[TableName].Rows[0]["Current Service State"].ToString();
            }
            else
            {
                status = null;
            }
            return status;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsStandardVoucher"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetStandardVoucherRecurringDeleteStatus(DataSet dsStandardVoucher, string jobname, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@JobName", jobname);


            var result = false;

            // Get data
            result = DbRetrieve("GL.GetStandardVoucherRecurringDeleteStatus", dsStandardVoucher, dbParams, TableName,
                connStr);

            // return result
            return result;
        }

        public bool GetStandardVoucherRecurringSQLJobCreate(DataSet dsStandardVoucher, string jobName, string enabled,
            string scheduleName, string freqType, string freqInterval, string occursEveryType, string occursEvery,
            string activeStartTime, string activeEndTime, string activeStartDate, string freqrecirrenceFactor,
            string freqrelativeInterval, string activeendDate, string connStr)
        {
            var dbParams = new DbParameter[13];

            // Create parameter
            dbParams[0] = new DbParameter("@JobName", jobName);
            dbParams[1] = new DbParameter("@Eenabled", enabled);
            dbParams[2] = new DbParameter("@ScheduleName", scheduleName);
            dbParams[3] = new DbParameter("@FreqType", freqType);
            dbParams[4] = new DbParameter("@FreqInterval", freqInterval);
            dbParams[5] = new DbParameter("@FreqsubdayType", occursEveryType);
            dbParams[6] = new DbParameter("@FreqsubdayInterval", occursEvery);
            dbParams[7] = new DbParameter("@ActiveStartTime", activeStartTime);
            dbParams[8] = new DbParameter("@ActiveEndTime", activeEndTime);
            dbParams[9] = new DbParameter("@ActiveStartDate", activeStartDate);
            dbParams[10] = new DbParameter("@FreqrecirrenceFactor", freqrecirrenceFactor);
            dbParams[11] = new DbParameter("@FreqrelativeInterval", freqrelativeInterval);
            dbParams[12] = new DbParameter("@ActiveendDate", activeendDate);

            var result = false;

            // Get data
            result = DbRetrieve("GL.GetStandardVoucherRecurringSQLJobCreate", dsStandardVoucher, dbParams, TableName,
                connStr);

            // return result
            return result;
        }

        #endregion
    }
}