using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class JournalVoucherDetail : DbHandler
    {
        #region "Attributies"

        //private string userName = "Developer";
        //private int buID = 1;

        public string JournalVoucherNo { get; set; }

        public int JournalVoucherDetailNo { get; set; }

        public string AccountCode { get; set; }

        public decimal DrAmt { get; set; }

        public decimal CrAmt { get; set; }

        public decimal DrBaseAmt { get; set; }

        public decimal CrBaseAmt { get; set; }

        public string Remark { get; set; }

        #endregion

        #region "Operations"

        public JournalVoucherDetail()
        {
            SelectCommand = "SELECT * FROM GL.JournalVoucherDetail";
            TableName = "JournalVoucherDetail";
        }

        /// <summary>
        ///     Return dataset from journalvoucher detail by journal voucherno.
        /// </summary>
        /// <param name="dsJournalVoucher"></param>
        public void GetByVoucherNo(DataSet dsJournalVoucher, string journalVoucherNo, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@JournalVoucherNo", journalVoucherNo);

            var journalVoucherDetail = new JournalVoucherDetail();
            journalVoucherDetail.DbRetrieve("GL.GetJournalVoucherDetailListByVoucherNo", dsJournalVoucher, dbParams,
                TableName, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsJV"></param>
        /// <param name="JvNo"></param>
        /// <param name="Accountcode"></param>
        /// <param name="connStr"></param>
        public void GetByJvNoAcc(DataSet dsJV, string JvNo, string Accountcode, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@JournalVoucherNo", JvNo);
            dbParams[1] = new DbParameter("@AccountCode", Accountcode);

            var JvDetail = new JournalVoucherDetail();
            JvDetail.DbRetrieve("GL.GetJournalVoucherDetailByJvNoAcc", dsJV, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get data by account code.
        /// </summary>
        /// <param name="dsJV"></param>
        /// <param name="Accountcode"></param>
        /// <param name="connStr"></param>
        public void GetByAcc(DataSet dsJV, string Accountcode, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AccountCode", Accountcode);

            DbRetrieve("GL.GetJournalVoucherDetailByAcc", dsJV, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Return dataset from journalvoucher detail by dates
        /// </summary>
        /// <param name="dsJournalVoucher"></param>
        public void GetByDate(DataSet dsJournalVoucher, DateTime startDate, DateTime endDate, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@startDate", startDate.ToString());
            dbParams[1] = new DbParameter("@endDate", endDate.ToString());

            var journalVoucherDetail = new JournalVoucherDetail();
            journalVoucherDetail.DbRetrieve("GL.GetJournalVoucherDetailListByDate", dsJournalVoucher, dbParams,
                TableName, connStr);
        }

        /// <summary>
        ///     Get journalvoucherdetail table schema.
        /// </summary>
        /// <param name="journalVoucherDetail"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsJournalVoucherDetailEdit, string connStr)
        {
            return DbRetrieveSchema("GL.GetJournalVoucherDetailList", dsJournalVoucherDetailEdit, null, TableName,
                connStr);
        }

        /// <summary>
        ///     Get last journal voucher no.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetMaxNo(string connStr)
        {
            var dtTmp = new DataTable();
            var dtJournalVoucher = new DataTable();
            var dbParams = new DbParameter[1];

            // Get data
            dtTmp = DbRead("GL.GetJournalVoucherDetailMaxNo", null, connStr);

            // Return result            
            return int.Parse(dtTmp.Rows[0]["JvDetailNo"].ToString());
        }

        /// <summary>
        ///     Get Data by account code
        /// </summary>
        /// <param name="dsJV"></param>
        /// <param name="AccCode"></param>
        /// <param name="connStr"></param>
        public void GetByAccCode(DataSet dsJV, string AccCode, int PeriodNumber, decimal bfwdDr, decimal bfwdCr,
            int year, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@AccountCode", AccCode);
            dbParams[1] = new DbParameter("@PeriodNumber", PeriodNumber.ToString());
            dbParams[2] = new DbParameter("@Year", year.ToString());
            //dbParams[2]             = new DbParameter("@BfwdDr", bfwdDr.ToString());
            //dbParams[3]             = new DbParameter("@BfwdCr", bfwdCr.ToString());            

            var journalVoucherDetail = new JournalVoucherDetail();
            journalVoucherDetail.DbRetrieve("GL.GetJournalVoucherDetailListByAccountCode", dsJV, dbParams, TableName,
                connStr);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public void GetBfwdDrBfwdCr(DataSet dsJV, string AccCode, int PeriodNumber, int Year, string connStr)
        {
            //int bfwDr               = 0;            
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@AccountCode", AccCode);
            dbParams[1] = new DbParameter("@PeriodNumber", PeriodNumber.ToString());
            dbParams[2] = new DbParameter("@Year", Year.ToString());

            var journalVoucherDetail = new JournalVoucherDetail();
            journalVoucherDetail.DbRetrieve("GL.GetAccountListBfwdDrBfwdCr", dsJV, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Save database to journalvoucherDetail table.
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            // สร้าง SaveSource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);

            // เรียก dbCommit โดยส่ง SaveSource object เป็น parameter
            DbCommit(dbSaveSource, connStr);

            return true;
        }

        #endregion
    }
}