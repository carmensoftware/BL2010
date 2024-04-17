using System.Data;
using Blue.DAL;

namespace Blue.BL.GL.Rec
{
    public class RecDt : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public RecDt()
        {
            SelectCommand = "SELECT * FROM GL.RecDt";
            TableName = "RecDt";
        }

        /// <summary>
        /// </summary>
        /// <param name="recNo"></param>
        /// <param name="dsRecDt"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetByRecNo(string recNo, DataSet dsRecDt, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@RecNo", recNo);

            // Get data            
            result = DbRetrieve("GL.GetRecDt_RecNo", dsRecDt, dbParams, TableName, connStr);

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
            dbParams[1] = new DbParameter("@PeriodID", Period.ToString());
            dbParams[2] = new DbParameter("@AccCode", AccCode);

            // Get data            
            //dtAccRec = DbRead("GL.GetAccountReconcileDrByYearPeriodAccCode", dbParams, connStr);
            dtAccRec = DbRead("GL.GetRecDr_Year_Period_AccCodeUnion", dbParams, connStr);

            // Return result
            return dtAccRec;
        }

        /// <summary>
        ///     Get data credit list.
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Period"></param>
        /// <param name="AccCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetCreditList(int Year, int Period, string AccCode, string connStr)
        {
            var dtAccRec = new DataTable();
            var dbParams = new DbParameter[3];

            // Create parameter
            dbParams[0] = new DbParameter("@Year", Year.ToString());
            dbParams[1] = new DbParameter("@PeriodID", Period.ToString());
            dbParams[2] = new DbParameter("@AccCode", AccCode);

            // Get data            
            //dtAccRec = DbRead("GL.GetAccountReconcileCrByYearPeriodAccCode", dbParams, connStr);
            dtAccRec = DbRead("GL.GetRecCr_Year_Period_AccCodeUnion", dbParams, connStr);

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
            dbParams[1] = new DbParameter("@PeriodID", Period.ToString());
            dbParams[2] = new DbParameter("@AccCode", AccCode);

            // Get data            
            //dtAccRec = DbRead("GL.GetAccountReconcileBankItemByYearPeriodAccCode", dbParams, connStr);
            dtAccRec = DbRead("GL.GetRecBank_Year_Period_AccCodeUnion", dbParams, connStr);

            // Return result
            return dtAccRec;
        }

        /// <summary>
        ///     Get max group number.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetMaxNo(string connStr)
        {
            int maxNo;

            // Get data
            maxNo = DbReadScalar("GL.GetRecDtMaxGroupNo", null, connStr);

            return maxNo;
        }

        /// <summary>
        ///     Get schema recondile detail.
        /// </summary>
        /// <param name="dsRecDt"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsRecDt, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("GL.GetRecDtList", dsRecDt, null, TableName, connStr);

            // Return result
            return result;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="dsAccRecDetail"></param>
        ///// <param name="connStr"></param>
        ///// <returns></returns>
        //public bool Delete(DataSet dsAccRecDetail, string connStr)
        //{
        //    bool result                 = false;
        //    DbSaveSource[] dbSaveSorce  = new DbSaveSource[1];

        //    // Create dbSaveSource            
        //    dbSaveSorce[0] = new DbSaveSource(dsAccRecDetail, this.SelectCommand, this.TableName);

        //    // Save to database
        //    result = DbCommit(dbSaveSorce, connStr);

        //    // Return result
        //    return result;
        //}

        #endregion
    }
}