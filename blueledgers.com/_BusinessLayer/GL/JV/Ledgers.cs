using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL.JV
{
    public class Ledgers : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor.
        /// </summary>
        public Ledgers()
        {
            SelectCommand = "SELECT * FROM GL.Ledgers";
            TableName = "Ledgers";
        }

        /// <summary>
        ///     Get ledgers data structure.
        /// </summary>
        /// <param name="dsLedger"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsLedger, string strConn)
        {
            return DbRetrieveSchema("GL.GetLedgersList", dsLedger, null, TableName, strConn);
        }

        /// <summary>
        ///     Get the number of ledgers ralated to specified account code.
        /// </summary>
        /// <param name="strAccCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public int Count(string strAccCode, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AccCode", strAccCode);

            return DbReadScalar("GL.GetLedgersCount_AccCode", dbParams, strConn);
        }

        /// <summary>
        ///     Get ledgers data for journal voucher only.
        ///     By using JVNo as a HdrNo as parameters.
        /// </summary>
        /// <param name="dsLedgers"></param>
        /// <param name="srtHdrNo"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsLedgers, string srtHdrNo, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@HdrNo", srtHdrNo);

            return DbRetrieve("GL.GetLedgersList_HdrNo", dsLedgers, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get the number of ledgers which created between specified start date and end date.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public int Count(DateTime startDate, DateTime endDate, string strConn)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@Sdate", startDate.ToString());
            dbParams[1] = new DbParameter("@Edate", endDate.ToString());

            return DbReadScalar("GL.GetLedgersCount_DateFromTo", dbParams, strConn);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsLedgers"></param>
        /// <param name="jvNo"></param>
        /// <param name="connStr"></param>
        public void GetByHdrNo(DataSet dsLedgers, string jvNo, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@HdrNo", jvNo);

            var ledgers = new Ledgers();
            ledgers.DbRetrieve("GL.GetLedgers_JVNo", dsLedgers, dbParams, TableName, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsLedgers"></param>
        /// <param name="AccCode"></param>
        /// <param name="PeriodNumber"></param>
        /// <param name="Year"></param>
        /// <param name="connStr"></param>
        public void GetBfwdDrBfwdCr(DataSet dsLedgers, string AccCode, int PeriodNumber, string Year, string connStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@AccountCode", AccCode);
            dbParams[1] = new DbParameter("@PeriodNumber", PeriodNumber.ToString());
            dbParams[2] = new DbParameter("@Year", Year);

            var ledger = new Ledgers();
            //ledger.DbRetrieve("GL.GetvTrBal_Year_Period_AccCode", dsLedgers, dbParams, this.TableName, connStr);
            ledger.DbRetrieve("GL.GetAccountListBfwdDrBfwdCr", dsLedgers, dbParams, TableName, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsLedgers"></param>
        /// <param name="AccCode"></param>
        /// <param name="PeriodNumber"></param>
        /// <param name="bfwdDr"></param>
        /// <param name="bfwdCr"></param>
        /// <param name="year"></param>
        /// <param name="connStr"></param>
        public void GetByAccCode(DataSet dsLedgers, string AccCode, int PeriodNumber, int year, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@AccountCode", AccCode);
            dbParams[1] = new DbParameter("@PeriodNumber", PeriodNumber.ToString());
            dbParams[2] = new DbParameter("@Year", year.ToString());

            var ledger = new Ledgers();
            ledger.DbRetrieve("GL.GetLedgers_AccCode", dsLedgers, dbParams, TableName, connStr);
        }

        public void GetByAccCode(DataSet dsLedgers, string AccCode, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AccCode", AccCode);

            var ledger = new Ledgers();
            ledger.DbRetrieve("GL.GetLedger_AccCode", dsLedgers, dbParams, TableName, connStr);
        }

        public void GetByINCFYear(DataSet dsLedgers, int intFYear, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Year", intFYear.ToString());

            var ledger = new Ledgers();
            ledger.DbRetrieve("GL.GetLedgersList_INC_FYear", dsLedgers, dbParams, TableName, connStr);
        }

        public DataTable GetTotal(int intFYear, string connStr)
        {
            var dtTotal = new DataTable();

            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Year", intFYear.ToString());

            dtTotal = DbRead("GL.GetLedgerTotal_FYear", dbParams, connStr);

            return dtTotal;
        }

        #endregion
    }
}