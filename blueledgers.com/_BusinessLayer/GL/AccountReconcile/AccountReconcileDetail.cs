using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class AccountReconcileDetail : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public AccountReconcileDetail()
        {
            SelectCommand = "SELECT * FROM GL.AccountReconcileDetail";
            TableName = "AccountReconcileDetail";
        }

        /// <summary>
        ///     Get account reconcile detail using accrecno
        /// </summary>
        /// <param name="AccRecNo"></param>
        /// <returns></returns>
        public bool GetByAccRecNo(string AccRecNo, DataSet dsAccRecDetail, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@AccRecNo", AccRecNo);

            // Get data            
            result = DbRetrieve("GL.GetAccountReconcileDetailByAccRecNo", dsAccRecDetail, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get data debit list.
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Period"></param>
        /// <param name="AccCode"></param>
        /// <param name="dsAccRecDetail"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetDebitList(int Year, int Period, string AccCode, string connStr)
        {
            var dtAccRec = new DataTable();
            var dbParams = new DbParameter[3];

            // Create parameter
            dbParams[0] = new DbParameter("@Year", Year.ToString());
            dbParams[1] = new DbParameter("@PeriodNum", Period.ToString());
            dbParams[2] = new DbParameter("@AccCode", AccCode);

            // Get data            
            //dtAccRec = DbRead("GL.GetAccountReconcileDrByYearPeriodAccCode", dbParams, connStr);
            dtAccRec = DbRead("GL.GetAccountReconcileDrByYearPeriodAccCodeUnion", dbParams, connStr);

            // Return result
            return dtAccRec;
        }

        /// <summary>
        ///     Get data credit list.
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Period"></param>
        /// <param name="AccCode"></param>
        /// <param name="dsAccRecDetail"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetCreditList(int Year, int Period, string AccCode, string connStr)
        {
            var dtAccRec = new DataTable();
            var dbParams = new DbParameter[3];

            // Create parameter
            dbParams[0] = new DbParameter("@Year", Year.ToString());
            dbParams[1] = new DbParameter("@PeriodNum", Period.ToString());
            dbParams[2] = new DbParameter("@AccCode", AccCode);

            // Get data            
            //dtAccRec = DbRead("GL.GetAccountReconcileCrByYearPeriodAccCode", dbParams, connStr);
            dtAccRec = DbRead("GL.GetAccountReconcileCrByYearPeriodAccCodeUnion", dbParams, connStr);

            // Return result
            return dtAccRec;
        }

        /// <summary>
        ///     Get data bank item list.
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Period"></param>
        /// <param name="AccCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetBankItemList(int Year, int Period, string AccCode, string connStr)
        {
            var dtAccRec = new DataTable();
            var dbParams = new DbParameter[3];

            // Create parameter
            dbParams[0] = new DbParameter("@Year", Year.ToString());
            dbParams[1] = new DbParameter("@PeriodNum", Period.ToString());
            dbParams[2] = new DbParameter("@AccCode", AccCode);

            // Get data            
            //dtAccRec = DbRead("GL.GetAccountReconcileBankItemByYearPeriodAccCode", dbParams, connStr);
            dtAccRec = DbRead("GL.GetAccountReconcileBankItemByYearPeriodAccCodeUnion", dbParams, connStr);

            // Return result
            return dtAccRec;
        }

        /// <summary>
        ///     Get max accrecno
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetMaxNo(string connStr)
        {
            int maxNo;

            // Get data
            maxNo = DbReadScalar("GL.GetAccountReconcileDetailMaxGroupNo", null, connStr);

            return maxNo;
        }

        /// <summary>
        ///     Get account reconcile detail
        /// </summary>
        /// <param name="dsAccRecDetailDetail"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsAccRecDetail, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("GL.GetAccountReconcileDetailList", dsAccRecDetail, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsAccRecDetail"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsAccRecDetail, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource            
            dbSaveSorce[0] = new DbSaveSource(dsAccRecDetail, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}