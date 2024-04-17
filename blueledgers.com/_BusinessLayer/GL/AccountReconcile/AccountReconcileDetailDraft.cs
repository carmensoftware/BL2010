using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class AccountReconcileDetailDraft : DbHandler
    {
        #region "Attibuties"

        #endregion

        #region "Operations"

        public AccountReconcileDetailDraft()
        {
            SelectCommand = "SELECT * FROM GL.AccountReconcileDetailDraft";
            TableName = "AccountReconcileDetailDraft";
        }

        /// <summary>
        ///     Get account reconcile detail draft table schema.
        /// </summary>
        /// <param name="journalVoucher"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsAccRecDetailDraft, string conStr)
        {
            return DbRetrieveSchema("GL.GetAccountReconcileDetailDraftList", dsAccRecDetailDraft, null, TableName,
                conStr);
        }

        /// <summary>
        ///     Get account reconcile detail draft list.
        /// </summary>
        /// <param name="dsAccRecDetailDraft"></param>
        /// <param name="connStr"></param>
        public void GetList(DataSet dsAccRecDetailDraft, string connStr)
        {
            DbRetrieve("GL.GetAccountReconcileDetailDraftList", dsAccRecDetailDraft, null, TableName, connStr);
        }

        /// <summary>
        ///     Get data by accrecno.
        /// </summary>
        /// <param name="dsAccRecDetailDraft"></param>
        /// <param name="AccRecNo"></param>
        /// <param name="connStr"></param>
        public void GetByID(DataSet dsAccRecDetailDraft, string AccRecNo, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AccRecNo", AccRecNo);

            DbRetrieve("GL.GetAccountReconcileDetailDraftByAccRecNo", dsAccRecDetailDraft, dbParams, TableName, connStr);
        }

        public DataTable GetDebitList(int Year, int Period, string AccCode, string connStr)
        {
            var dtAccRec = new DataTable();
            var dbParams = new DbParameter[3];

            // Create parameter
            dbParams[0] = new DbParameter("@Year", Year.ToString());
            dbParams[1] = new DbParameter("@PeriodNum", Period.ToString());
            dbParams[2] = new DbParameter("@AccCode", AccCode);

            // Get data            
            dtAccRec = DbRead("GL.GetAccountReconcileDraftDrByYearPeriodAccCode", dbParams, connStr);

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
            dtAccRec = DbRead("GL.GetAccountReconcileDraftCrByYearPeriodAccCode", dbParams, connStr);

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
            dtAccRec = DbRead("GL.GetAccountReconcileDraftBankItemByYearPeriodAccCode", dbParams, connStr);

            // Return result
            return dtAccRec;
        }

        #endregion
    }
}