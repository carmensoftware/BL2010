using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class JournalVoucher : DbHandler
    {
        #region "Attibuties"

        private readonly JournalVoucherDetail _journalVoucherDetail = new JournalVoucherDetail();
        private readonly JV.JVView _jvView = new JV.JVView();

        public string JournalVoucherNo { get; set; }

        public string PrefixCode { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string CurrencyCode { get; set; }

        public decimal ExchangeRate { get; set; }

        public string PostFrom { get; set; }

        public bool IsVoid { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        #endregion

        #region "Operations"  

        public JournalVoucher()
        {
            SelectCommand = "SELECT * FROM GL.JournalVoucher";
            TableName = "JournalVoucher";
        }

        public bool GetList(DataSet dsAcc, int intViewID, string strUserName, string strConn)
        {
            // Get query string and parameter
            var dbParams = new DbParameter[0];
            var strQuery = _jvView.GetQuery(intViewID, ref dbParams, strUserName, strConn);

            // Get account list.
            return DbExecuteQuery(strQuery, dsAcc, (dbParams.Length > 0 ? dbParams : null), TableName, strConn);
        }

        /// <summary>
        ///     Get journalvoucher using journalvoucher view id
        /// </summary>
        /// <param name="dsJournalVoucher"></param>
        public void GetList(DataSet dsJournalVoucher, int journalvoucherViewID, int userID, string connStr)
        {
            var dtJournalVoucher = new DataTable();
            var dtJournalVoucherViewCriteria = new DataTable();
            var journalVoucher = new JournalVoucher();
            var journalvoucherViewQuery = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate  query
            journalvoucherViewQuery = new JournalVoucherView().GetJournalVoucherViewQuery(journalvoucherViewID, userID,
                connStr);

            // Generate  parameter

            dtJournalVoucherViewCriteria =
                new JournalVoucherViewCriteria().GetJournalVoucherViewCriteriaList(journalvoucherViewID, connStr);

            if (dtJournalVoucherViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtJournalVoucherViewCriteria.Rows.Count];

                for (var i = 0; i < dtJournalVoucherViewCriteria.Rows.Count; i++)
                {
                    var dr = dtJournalVoucherViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtJournalVoucher = journalVoucher.DbExecuteQuery(journalvoucherViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtJournalVoucher = journalVoucher.DbExecuteQuery(journalvoucherViewQuery, null, connStr);
            }

            // Return result
            if (dsJournalVoucher.Tables[TableName] != null)
            {
                dsJournalVoucher.Tables.Remove(TableName);
            }

            dtJournalVoucher.TableName = TableName;
            dsJournalVoucher.Tables.Add(dtJournalVoucher);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsJournalVoucher"></param>
        /// <param name="journalvoucherViewID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public void GetList(DataSet dsJournalVoucher, int journalvoucherViewID, string connStr)
        {
            //bool result = false;


            //// Paramter value assign to dbparameter array.
            //DbParameter[] dbParams = new DbParameter[1];
            //dbParams[0] = new DbParameter("@JournalVoucherViewID", Convert.ToString(journalvoucherViewID));


            //// Get data
            //result = DbRetrieve("GL.GetJournalVoucherGenerateColumnList", dsJournalVoucher , dbParams, this.TableName, connStr);

            //// Return result
            //return result;

            var dtJournalVoucher = new DataTable();
            var journalVoucher = new JournalVoucher();

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@JournalVoucherViewID", Convert.ToString(journalvoucherViewID));


            // Get data                
            dtJournalVoucher = journalVoucher.DbRead("GL.GetJournalVoucherGenerateColumnList", dbParams, connStr);

            // Return result
            if (dsJournalVoucher.Tables[TableName] != null)
            {
                dsJournalVoucher.Tables.Remove(TableName);
            }

            dtJournalVoucher.TableName = TableName;
            dsJournalVoucher.Tables.Add(dtJournalVoucher);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsJournalVoucher"></param>
        /// <param name="journalvoucherViewID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsJournalVoucher, string connStr)
        {
            var result = false;


            // Get data
            result = DbRetrieve("GL.GetJournalVoucherList", dsJournalVoucher, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get journalvoucher view default.
        /// </summary>
        /// <param name="dsJournalVoucher"></param>
        /// <param name="journalvoucherViewID"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetDefault(DataSet dsJournalVoucher, int journalvoucherViewID, int userID, string connStr)
        {
            var dtJournalVoucher = new DataTable();
            var dtJournalVoucherViewCriteria = new DataTable();
            var journalVoucher = new JournalVoucher();
            var journalvoucherViewQuery = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate  query
            journalvoucherViewQuery = new JournalVoucherView().GetJournalVoucherViewQuery(journalvoucherViewID, userID,
                connStr);

            // Generate  parameter

            dtJournalVoucherViewCriteria =
                new JournalVoucherViewCriteria().GetJournalVoucherViewCriteriaList(journalvoucherViewID, connStr);

            if (dtJournalVoucherViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtJournalVoucherViewCriteria.Rows.Count];

                for (var i = 0; i < dtJournalVoucherViewCriteria.Rows.Count; i++)
                {
                    var dr = dtJournalVoucherViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtJournalVoucher = journalVoucher.DbExecuteQuery(journalvoucherViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtJournalVoucher = journalVoucher.DbExecuteQuery(journalvoucherViewQuery, null, connStr);
            }

            // Return result
            if (dsJournalVoucher.Tables[TableName] != null)
            {
                dsJournalVoucher.Tables.Remove(TableName);
            }

            dtJournalVoucher.TableName = TableName;
            dsJournalVoucher.Tables.Add(dtJournalVoucher);
        }

        /// <summary>
        ///     Get journalvourcher for preview
        /// </summary>
        /// <param name="dsJournalVoucher"></param>
        /// <param name="journalvoucherViewID"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetPreveiw(DataSet dsJournalVoucher, int userID, string connStr)
        {
            var dtJournalVoucher = new DataTable();
            var dtJournalVoucherViewCriteria = new DataTable();
            var journalVoucher = new JournalVoucher();
            var journalvoucherViewQuery = string.Empty;

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate  query
            journalvoucherViewQuery = new JournalVoucherView().GetJournalVoucherViewQueryPreview(dsJournalVoucher,
                userID, connStr);

            // Generate  parameter
            dtJournalVoucherViewCriteria = dsJournalVoucher.Tables["JournalVoucherViewCriteria"];

            if (dtJournalVoucherViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtJournalVoucherViewCriteria.Rows.Count];

                for (var i = 0; i < dtJournalVoucherViewCriteria.Rows.Count; i++)
                {
                    var dr = dtJournalVoucherViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtJournalVoucher = journalVoucher.DbExecuteQuery(journalvoucherViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtJournalVoucher = journalVoucher.DbExecuteQuery(journalvoucherViewQuery, null, connStr);
            }

            // Return result
            if (dsJournalVoucher.Tables[TableName] != null)
            {
                dsJournalVoucher.Tables.Remove(TableName);
            }

            dtJournalVoucher.TableName = TableName;
            dsJournalVoucher.Tables.Add(dtJournalVoucher);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsJournalVoucher"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetPreveiwDefault(DataSet dsJournalVoucher, int userID, string connStr)
        {
            var dtJournalVoucher = new DataTable();
            var dtJournalVoucherViewCriteria = new DataTable();
            var journalVoucher = new JournalVoucher();
            var journalvoucherViewQuery = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate  query
            journalvoucherViewQuery = new JournalVoucherView().GetJournalVoucherViewQueryPreviewDefault(
                dsJournalVoucher, userID, connStr);

            // Generate  parameter
            dtJournalVoucherViewCriteria = dsJournalVoucher.Tables["JournalVoucherViewCriteria"];

            if (dtJournalVoucherViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtJournalVoucherViewCriteria.Rows.Count];

                for (var i = 0; i < dtJournalVoucherViewCriteria.Rows.Count; i++)
                {
                    var dr = dtJournalVoucherViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtJournalVoucher = journalVoucher.DbExecuteQuery(journalvoucherViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtJournalVoucher = journalVoucher.DbExecuteQuery(journalvoucherViewQuery, null, connStr);
            }

            // Return result
            if (dsJournalVoucher.Tables[TableName] != null)
            {
                dsJournalVoucher.Tables.Remove(TableName);
            }

            dtJournalVoucher.TableName = TableName;
            dsJournalVoucher.Tables.Add(dtJournalVoucher);
        }

        /// <summary>
        ///     return dataset and get account list
        /// </summary>
        /// <param name="dsAccount"></param>
        public void GetListAll(DataSet dsJournalVoucher, string connStr)
        {
            DbRetrieve("GL.GetJournalVoucherList", dsJournalVoucher, null, TableName, connStr);
        }

        /// <summary>
        ///     Get data by account code.
        /// </summary>
        /// <param name="dsJV"></param>
        /// <param name="AccountCode"></param>
        /// <param name="connStr"></param>
        public void GetByAccCode(DataSet dsJV, string AccountCode, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AccountCode", AccountCode);

            DbRetrieve("GL.GetJournalVoucherByAcc", dsJV, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Return dataset from journalvoucher.
        /// </summary>
        /// <param name="dsJournalVoucher"></param>
        public void GetByVoucherNo(DataSet dsJournalVoucher, string journalVoucherNo, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@JournalVoucherNo", journalVoucherNo);

            var journalVoucher = new JournalVoucher();
            journalVoucher.DbRetrieve("GL.GetJournalVoucherListByVoucherNo", dsJournalVoucher, dbParams, TableName,
                connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsJV"></param>
        /// <param name="JvNo"></param>
        /// <param name="prefixCode"></param>
        /// <param name="connStr"></param>
        public void GetByJvNoPrefix(DataSet dsJV, string JvNo, string prefixCode, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@JournalVoucherNo", JvNo);
            dbParams[1] = new DbParameter("@PrefixCode", prefixCode);

            var journalVoucher = new JournalVoucher();
            journalVoucher.DbRetrieve("GL.GetJournalVoucherByJvNoPrefixCode", dsJV, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Retrieve all journal voucher by use the default view
        /// </summary>
        /// <param name="journalVoucherViewID"></param>
        /// <returns></returns>
        public virtual DataSet RetrieveByDefaultView7(int journalVoucherViewID)
        {
            throw new System.Exception("Not implemented");
        }

        /// <summary>
        ///     Get journalvoucher table schema.
        /// </summary>
        /// <param name="journalVoucher"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet journalVoucher, string conStr)
        {
            return DbRetrieveSchema("GL.GetJournalVoucherList", journalVoucher, null, TableName, conStr);
        }

        /// <summary>
        ///     Generate new journal voucher number
        /// </summary>
        /// <param name="prefixCode"></param>
        /// <returns></returns>
        public string GetJournalVoucherNo(string prefixCode, string conStr)
        {
            var dtJournalVoucher = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@PrefixCode", prefixCode);

            // Get data
            dtJournalVoucher = DbRead("GL.GetJournalVoucherNo", dbParams, conStr);

            // Return result            
            return dtJournalVoucher.Rows[0]["NewJournalVoucher"].ToString();
        }

        /// <summary>
        ///     Get all recurred journal voucher at end of specified period
        /// </summary>
        /// <param name="dsJournalVoucher"></param>
        /// <returns></returns>
        public bool GetJournalVoucherRecurred(DataSet dsJournalVoucher, int periodID, string connStr)
        {
            var result = false;
            var period = new Period();
            var dbParams = new DbParameter[1];

            // Get end date
            var endDate = period.GetPeriodEndDate(periodID, connStr);

            // Create parameter
            dbParams[0] = new DbParameter("@Date", endDate.ToString("yyyy-MM-dd"));

            // Get data
            result = DbRetrieve("GL.GetJournalVoucherRecurred", dsJournalVoucher, dbParams, connStr);

            // Return result
            return result;
        }

        public string GetCurrencyCode(string JvNo, string conStr)
        {
            var dsTmp = new DataSet();

            GetByVoucherNo(dsTmp, JvNo, conStr);

            return dsTmp.Tables["JournalVoucher"].Rows[0]["CurrencyCode"].ToString();
        }

        public double GetExchangeRate(string JvNo, string conStr)
        {
            var dsTmp = new DataSet();

            GetByVoucherNo(dsTmp, JvNo, conStr);

            return double.Parse(dsTmp.Tables["JournalVoucher"].Rows[0]["ExchangeRate"].ToString());
        }

        /// <summary>
        ///     Save database to journalvoucher table.
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var journalVoucherDetail = new JournalVoucherDetail();
            var journalVoucherMisc = new JournalVoucherMisc();
            var journalVoucherActiveLog = new JournalVoucherActiveLog();
            //JournalVoucherAttachment journalVoucherAttachment = new JournalVoucherAttachment();
            //JournalVoucherComment    journalVoucherComment      = new JournalVoucherComment();


            var result = false;

            // สร้าง SaveSource object
            var dbSaveSource = new DbSaveSource[4];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, journalVoucherDetail.SelectCommand,
                journalVoucherDetail.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, journalVoucherMisc.SelectCommand, journalVoucherMisc.TableName);
            dbSaveSource[3] = new DbSaveSource(savedData, journalVoucherActiveLog.SelectCommand,
                journalVoucherActiveLog.TableName);
            //dbSaveSource[4]             = new DbSaveSource(savedData, journalVoucherAttachment.SelectCommand,journalVoucherAttachment.TableName);


            // Call dbCommit and send savesource object is parameter
            result = DbCommit(dbSaveSource, connStr);

            return result;
        }

        /// <summary>
        ///     save void
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool SaveVoid(DataSet savedData, string connStr)
        {
            var result = false;

            // สร้าง SaveSource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);

            // Call dbCommit and send savesource object is parameter
            result = DbCommit(dbSaveSource, connStr);

            return result;
        }

        /// <summary>
        ///     Save database to journalvoucher table for Copy JournalVoucher.
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool SaveCopy(DataSet savedData, string connStr)
        {
            var journalVoucherDetail = new JournalVoucherDetail();
            var journalVoucherMisc = new JournalVoucherMisc();

            // สร้าง SaveSource object
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, journalVoucherDetail.SelectCommand,
                journalVoucherDetail.TableName);
            //dbSaveSource[2] = new DbSaveSource(savedData, journalVoucherMisc.SelectCommand, journalVoucherMisc.TableName);
            //dbSaveSource[3] = new DbSaveSource(savedData, journalVoucherAttachment.SelectCommand, journalVoucherAttachment.TableName);

            // Call dbCommit and send savesource object is parameter
            DbCommit(dbSaveSource, connStr);

            return true;
        }

        /// <summary>
        ///     Delete from database
        /// </summary>
        /// <param name="dsJournalVoucher"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsJournalVoucher, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[2];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsJournalVoucher, _journalVoucherDetail.SelectCommand,
                _journalVoucherDetail.TableName);
            dbSaveSorce[1] = new DbSaveSource(dsJournalVoucher, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     return datatable and get journalVoucher report data.
        /// </summary>
        /// <param name="Search_Param"></param>
        /// <returns></returns>
        public DataTable GetReport1(DateTime from_Date, DateTime to_Date, string from_JV, string to_JV,
            string prefix_Code, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[5];
            dbParams[0] = new DbParameter("@from_Date", from_Date.ToString("yyyy-MM-dd"));
            dbParams[1] = new DbParameter("@to_Date", to_Date.ToString("yyyy-MM-dd"));
            dbParams[2] = new DbParameter("@from_JV", from_JV);
            dbParams[3] = new DbParameter("@to_JV", to_JV);
            dbParams[4] = new DbParameter("@prefix_Code", prefix_Code);

            return DbRead("GL.GetJournalVoucherReport", dbParams, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="journalVoucherNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetIsVoid(string journalVoucherNo, string connStr)
        {
            bool status;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@JournalVoucherNo", journalVoucherNo);

            DbRetrieve("GL.GetJournalVoucherListByVoucherNo", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                status = ((bool) dsTmp.Tables[TableName].Rows[0]["IsVoid"]);
            }
            else
            {
                status = false;
            }
            return status;
        }

        #endregion
    }
}