using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class Period : DbHandler
    {
        #region "Attibuties"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Period()
        {
            SelectCommand = "SELECT * FROM GL.Period";
            TableName = "Period";
        }

        /// <summary>
        ///     Get period
        /// </summary>
        /// <param name="PeriodID"></param>
        /// <returns></returns>
        public DataTable GetPeriod(int periodID, string connStr)
        {
            var dtPeriod = new DataTable();
            var dbParams = new DbParameter[1];


            // Generate parameter
            dbParams[0] = new DbParameter("@PeriodID", periodID.ToString());

            // Get data
            dtPeriod = DbRead("GL.GetPeriod", dbParams, connStr);

            // Return result
            return dtPeriod;
        }

        /// <summary>
        ///     Get period using year
        /// </summary>
        /// <param name="budgetID"></param>
        /// <param name="dsBudget"></param>
        /// <returns></returns>
        public DataSet GetPeriod(string year, DataSet dsPeriod, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Year", year);

            DbRetrieve("GL.GetPeriodByYear", dsPeriod, dbParams, TableName, connStr);
            return dsPeriod;
        }

        public DataTable GetPeriodByYearAndPeriodID(int year, int periodID, string connStr)
        {
            var dtPeriod = new DataTable();
            var dbParams = new DbParameter[2];

            // Generate parameter
            dbParams[0] = new DbParameter("@Year", year.ToString());
            dbParams[1] = new DbParameter("@PeriodID", periodID.ToString());

            // Get data
            dtPeriod = DbRead("GL.GetPeriodByYearAndPeriodID", dbParams, connStr);

            // Return result
            return dtPeriod;
        }

        /// <summary>
        ///     Get period
        /// </summary>
        /// <param name="PeriodID"></param>
        /// <returns></returns>
        public DataTable GetPeriodByYearAndPeriodNumber(int year, int periodNumber, string connStr)
        {
            var dtPeriod = new DataTable();
            var dbParams = new DbParameter[2];

            // Generate parameter
            dbParams[0] = new DbParameter("@Year", year.ToString());
            dbParams[1] = new DbParameter("@PeriodNumber", periodNumber.ToString());

            // Get data
            dtPeriod = DbRead("GL.GetPeriodByYearAndPeriodNumber", dbParams, connStr);

            // Return result
            return dtPeriod;
        }

        /// <summary>
        ///     Get all period which not closed
        /// </summary>
        /// <returns></returns>
        public DataTable GetPeriodListForDropDownList(string connStr)
        {
            var dtPeriod = new DataTable();

            // Get data
            dtPeriod = DbRead("GL.GetPeriodListForDropDownList", null, connStr);

            // Return result
            return dtPeriod;
        }

        /// <summary>
        ///     Get period number using period id
        /// </summary>
        /// <param name="periodID"></param>
        /// <returns></returns>
        public int GetPeriodNumber(int periodID, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Generate parameter
            dbParams[0] = new DbParameter("@PeriodID", periodID.ToString());

            // Return result
            return DbReadScalar("GL.GetPeriodNumber", dbParams, connStr);
        }

        /// <summary>
        ///     Get end of period by year and periodnumber.
        /// </summary>
        /// <returns></returns>
        public DateTime GetPeriodEndDate(int year, int periodNumber, string connStr)
        {
            periodNumber++;

            if (periodNumber > Convert.ToInt32(12))
            {
                year++;
                periodNumber = 1;
            }

            var dtPeriod = new DataTable();
            var result = DateTime.MinValue;

            // Get data
            dtPeriod = GetPeriodByYearAndPeriodNumber(year, periodNumber, connStr);

            // Get end date.
            if (dtPeriod == null)
            {
                // return null.
                return result;
            }
            if (dtPeriod.Rows.Count > 0)
            {
                if (dtPeriod.Rows[0]["StartDate"] == DBNull.Value)
                {
                    // return null.
                    return result;
                }
                try
                {
                    // return correct value.
                    result = (((DateTime) dtPeriod.Rows[0]["StartDate"]).AddMonths(-1));
                }
                catch (Exception ex)
                {
                    LogManager.Error(ex);
                    return result;
                }
            }

            // Return result
            return result;
        }

        /// <summary>
        ///     Get end of period by year and periodnumber.
        /// </summary>
        /// <param name="periodID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DateTime GetPeriodEndDate(int periodID, string connStr)
        {
            var dtPeriod = new DataTable();
            var result = DateTime.MinValue;

            // Get data
            dtPeriod = GetPeriod(periodID, connStr);


            // Get end date
            if (dtPeriod != null)
            {
                result = ((DateTime) dtPeriod.Rows[0]["StartDate"]);
            }


            // Return result
            return result;
        }

        /// <summary>
        ///     Get lastest open period
        /// </summary>
        /// <returns></returns>
        public bool GetPeriodLastestOpen(DataSet dsPeriod, string connStr)
        {
            var dtPeriod = new DataTable();
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetPeriodLastestOpen", dsPeriod, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get period Latest open.
        /// </summary>
        /// <param name="PeriodID"></param>
        /// <returns></returns>
        public DataTable GetPeriodLastestOpen(string connStr)
        {
            var dtPeriod = new DataTable();

            // Get data
            dtPeriod = DbRead("GL.GetPeriodLastestOpen", null, connStr);

            // Return result
            return dtPeriod;
        }

        /// <summary>
        ///     Get lastest open period
        /// </summary>
        /// <returns></returns>
        public bool GetPeriodFirstOpen(DataSet dsPeriod, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetPeriodFirstOpen", dsPeriod, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPeriod"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetYearActive(DataSet dsPeriod, string connStr)
        {
            var result = DbRetrieve("GL.GetPeriodYearActive", dsPeriod, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Commit dataset to database
        /// </summary>
        /// <param name="dsPeriod"></param>
        /// <returns></returns>
        public bool Save(DataSet dsPeriod, int intAmtTb, string connStr)
        {
            var result = false;
            var periodEnd = new PeriodEnd();
            var jv = new BL.GL.JV.JV();
            var jvDt = new BL.GL.JV.JVDt();
            var ledgers = new BL.GL.JV.Ledgers();

            if (intAmtTb == 2)
            {
                var dbSaveSource = new DbSaveSource[2];

                // Create DbSaveSource
                dbSaveSource[0] = new DbSaveSource(dsPeriod, SelectCommand, TableName);
                dbSaveSource[1] = new DbSaveSource(dsPeriod, periodEnd.SelectCommand, periodEnd.TableName);

                // Commit to database
                result = DbCommit(dbSaveSource, connStr);
            }
            else
            {
                var dbSaveSource = new DbSaveSource[5];

                // Create DbSaveSource                
                dbSaveSource[0] = new DbSaveSource(dsPeriod, jv.SelectCommand, jv.TableName);
                dbSaveSource[1] = new DbSaveSource(dsPeriod, jvDt.SelectCommand, jvDt.TableName);
                dbSaveSource[2] = new DbSaveSource(dsPeriod, ledgers.SelectCommand, ledgers.TableName);
                dbSaveSource[3] = new DbSaveSource(dsPeriod, SelectCommand, TableName);
                dbSaveSource[4] = new DbSaveSource(dsPeriod, periodEnd.SelectCommand, periodEnd.TableName);

                // Commit to database
                result = DbCommit(dbSaveSource, connStr);
            }

            // Return result
            return result;
        }

        /// <summary>
        ///     Get number of row in period table
        /// </summary>
        /// <returns></returns>
        public int GetPeriodRowCount(string connStr)
        {
            var periodRowCount = 0;

            // Retrieve data
            periodRowCount = DbReadScalar("GL.GetPeriodRowCount", null, connStr);

            // Return result
            return periodRowCount;
        }

        ///// <summary>
        ///// Get max nearest period data from assigned year 
        ///// </summary>
        ///// <returns></returns>
        //public DataTable GetPeriodMaxNearest(int year, string connStr)
        //{
        //    DataTable dtPeriod = new DataTable();
        //    DbParameter[] dbParams = new DbParameter[1];

        //    // Create prameter
        //    dbParams[0] = new DbParameter("@Year", year.ToString());

        //    // Retrieve data
        //    dtPeriod = DbRead("GL.GetPeriodMaxNearest", dbParams, connStr);

        //    // Return result
        //    return dtPeriod;
        //}

        ///// <summary>
        ///// Get min nearest period data from assigned year 
        ///// </summary>
        ///// <returns></returns>
        //public DataTable GetPeriodMinNearest(int year, string connStr)
        //{
        //    DataTable dtPeriod = new DataTable();
        //    DbParameter[] dbParams = new DbParameter[1];

        //    // Create prameter
        //    dbParams[0] = new DbParameter("@Year", year.ToString());

        //    // Retrieve data
        //    dtPeriod = DbRead("GL.GetPeriodMinNearest", dbParams, connStr);

        //    // Return result
        //    return dtPeriod;
        //}

        /// <summary>
        ///     Get max period id
        /// </summary>
        /// <returns></returns>
        public int GetPeriodMaxID(string connStr)
        {
            var maxPeriodID = DbReadScalar("GL.GetPeriodMaxID", null, connStr);

            // Return result
            return maxPeriodID;
        }

        /// <summary>
        ///     Get period number of today.
        /// </summary>
        /// <returns></returns>
        public int GetCurrentPeriodNumber(string connStr)
        {
            var currentPeriodNumber = 1;

            // Get data
            currentPeriodNumber = DbReadScalar("GL.GetCurrentPeriodNumber", null, connStr);

            // Return resutl
            return currentPeriodNumber;
        }

        /// <summary>
        ///     Determine CheckingDate is in closed period or not.
        /// </summary>
        /// <param name="CheckingDate"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool IsInClosedPeriod(DateTime CheckingDate, string ConnectionString)
        {
            var dsPeriod = new DataSet();

            // Get period data.
            Get(dsPeriod, CheckingDate, ConnectionString);

            if (dsPeriod.Tables[TableName] != null)
            {
                if (dsPeriod.Tables[TableName].Rows.Count > 0)
                {
                    return (dsPeriod.Tables[TableName].Rows[0]["IsClose"] == DBNull.Value
                        ? false
                        : (bool) dsPeriod.Tables[TableName].Rows[0]["IsClose"]);
                }
            }

            return false;
        }

        /// <summary>
        ///     Get period data from StartDate
        /// </summary>
        /// <param name="dsPeriod"></param>
        /// <param name="StartDate"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool Get(DataSet dsPeriod, DateTime Date, string ConnectionString)
        {
            // Create parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Date", Date.ToString("yyyy-MM-dd"));

            // Get data and return result
            return DbRetrieve("GL.GetPeriod_Date", dsPeriod, dbParams, TableName, ConnectionString);
        }

        /// <summary>
        ///     Determine date is in active period.
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool IsInActive(DateTime Date, string strConn)
        {
            var dsPeriod = new DataSet();

            var result = Get(dsPeriod, Date, strConn);

            if (result)
            {
                if (dsPeriod.Tables[TableName].Rows.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Get period's year of specified date.
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public int GetYear(DateTime Date, string strConn)
        {
            var dsPeriod = new DataSet();

            var result = Get(dsPeriod, Date, strConn);

            if (result)
            {
                return int.Parse(dsPeriod.Tables[TableName].Rows[0]["Year"].ToString());
            }
            return 0;
        }

        /// <summary>
        ///     Get period's period number of specified date.
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public int GetPeriodNumber(DateTime Date, string strConn)
        {
            var dsPeriod = new DataSet();

            var result = Get(dsPeriod, Date, strConn);

            if (result)
            {
                return int.Parse(dsPeriod.Tables[TableName].Rows[0]["PeriodNumber"].ToString());
            }
            return 0;
        }

        /// <summary>
        ///     Get period number name
        /// </summary>
        /// <param name="periodNumber"></param>
        /// <returns></returns>
        public string GetPeriodNumberName(int periodNumber)
        {
            var month = string.Empty;
            switch (periodNumber)
            {
                case 1:
                {
                    month = "January";
                    break;
                }
                case 2:
                {
                    month = "February";
                    break;
                }
                case 3:
                {
                    month = "March";
                    break;
                }
                case 4:
                {
                    month = "April";
                    break;
                }
                case 5:
                {
                    month = "May";
                    break;
                }
                case 6:
                {
                    month = "June";
                    break;
                }
                case 7:
                {
                    month = "July";
                    break;
                }
                case 8:
                {
                    month = "August";
                    break;
                }
                case 9:
                {
                    month = "September";
                    break;
                }
                case 10:
                {
                    month = "October";
                    break;
                }
                case 11:
                {
                    month = "November";
                    break;
                }
                case 12:
                {
                    month = "December";
                    break;
                }
            }

            return month;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPeriod"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsPeriod, string strConn)
        {
            return DbRetrieveSchema("GL.GetPeriodAll", dsPeriod, null, TableName, strConn);
        }

        /// <summary>
        ///     Get year for lookup.
        /// </summary>
        /// <param name="dsPeriod"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public DataTable GetYearForLookup(string strConn)
        {
            var dtPeriod = new DataTable();

            dtPeriod = DbRead("GL.GetPeriodYearOnly", null, strConn);

            return dtPeriod;
        }

        #endregion
    }
}