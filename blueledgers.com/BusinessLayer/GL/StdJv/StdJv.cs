using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StdJv : DbHandler
    {
        #region "Attributies"

        private readonly StdJvView _stdJvView = new StdJvView();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public StdJv()
        {
            SelectCommand = "SELECT * FROM GL.StdJv";
            TableName = "StdJv";
        }

        /// <summary>
        ///     Get standard voucher data structure.
        /// </summary>
        /// <param name="dsStdJv"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsStdJv, string strConn)
        {
            return DbRetrieveSchema("GL.GetStdJvList", dsStdJv, null, TableName, strConn);
        }

        /// <summary>
        ///     Get standardvoucher list based on view id.
        /// </summary>
        /// <param name="dsStdJv"></param>
        /// <param name="intViewID"></param>
        /// <param name="strUserName"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsStdJv, int intViewID, string strUserName, string strConn)
        {
            // Get query string and parameter
            var dbParams = new DbParameter[0];
            var strQuery = _stdJvView.GetQuery(intViewID, ref dbParams, strUserName, strConn);

            // Get account list.
            return DbExecuteQuery(strQuery, dsStdJv, (dbParams.Length > 0 ? dbParams : null), TableName, strConn);
        }


        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsStdJv"></param>
        /// <returns></returns>
        public bool Save(DataSet dsStdJv, string connStr)
        {
            var stdJvDt = new StdJvDt();
            var ledgers = new StdJvLg();
            var result = false;
            var dbSaveSorce = new DbSaveSource[3];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsStdJv, SelectCommand, TableName);
            dbSaveSorce[1] = new DbSaveSource(dsStdJv, stdJvDt.SelectCommand, stdJvDt.TableName);
            dbSaveSorce[2] = new DbSaveSource(dsStdJv, ledgers.SelectCommand, ledgers.TableName);


            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Delete from database
        /// </summary>
        /// <param name="dsStdJv"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsStdJv, string connStr)
        {
            var stdJvDt = new StdJvDt();
            var ledgers = new StdJvLg();
            var result = false;
            var dbSaveSorce = new DbSaveSource[3];

            // Create dbSaveSource            

            dbSaveSorce[0] = new DbSaveSource(dsStdJv, ledgers.SelectCommand, ledgers.TableName);
            dbSaveSorce[1] = new DbSaveSource(dsStdJv, stdJvDt.SelectCommand, stdJvDt.TableName);
            dbSaveSorce[2] = new DbSaveSource(dsStdJv, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get max standard voucher id
        /// </summary>
        /// <returns></returns>
        public int GetNewStdJvNo(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("GL.GetNewStdJvNo", null, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Return dataset from standardvoucher.
        /// </summary>
        /// <param name="dsJournalVoucher"></param>
        public void GetByStdJVNo(DataSet dsStdJv, string stdJvNo, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@StdJvNo", stdJvNo);

            var stdJv = new StdJv();
            stdJv.DbRetrieve("GL.GetStdJv_StdJvNo", dsStdJv, dbParams, TableName, connStr);
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
        /// <param name="dsStdJv"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetStandardVoucherRecurringDeleteStatus(DataSet dsStdJv, string jobname, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@JobName", jobname);


            var result = false;

            // Get data
            result = DbRetrieve("GL.GetStandardVoucherRecurringDeleteStatus", dsStdJv, dbParams, TableName, connStr);

            // return result
            return result;
        }


        public bool GetStandardVoucherRecurringSQLJobCreate(DataSet dsStdJv, string jobName, string enabled,
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
            result = DbRetrieve("GL.GetStandardVoucherRecurringSQLJobCreate", dsStdJv, dbParams, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get all of standard voucher except recurring type
        /// </summary>
        /// <param name="dsStandardVoucher"></param>
        /// <returns></returns>
        public bool GetStdJvForPopup(DataSet dsStdJv, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetStdJvForPopup", dsStdJv, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     return datatable and get searching match data.
        /// </summary>
        /// <param name="Search_Param"></param>
        /// <returns></returns>
        public DataTable GetStdJvListSearch(string keyWord, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@KeyWord", keyWord);

            return DbRead("GL.GetStdJvListSearch", dbParams, connStr);
        }

        public bool Void(DataSet savedData, string connStr)
        {
            // Build savesource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}