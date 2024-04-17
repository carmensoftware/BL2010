using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.GL
{
    public class Period : DbHandler
    {
        /// <summary>
        ///     Empty constructor
        /// </summary>
        public Period()
        {
            SelectCommand = "SELECT * FROM GL.Period";
            TableName = "Period";
        }

        /// <summary>
        ///     Get Period data by using ID.
        /// </summary>
        /// <param name="dsPeriod"></param>
        /// <param name="id"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsPeriod, int id, string connStr)
        {
            var result = false;

            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ID", Convert.ToString(id));

            // Get data
            result = DbRetrieve("GL.GetPeriodByID", dsPeriod, dbParams, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get Period data by using Year and PeriodNo.
        /// </summary>
        /// <param name="dsPeriod"></param>
        /// <param name="year"></param>
        /// <param name="periodNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsPeriod, int year, int periodNo, string connStr)
        {
            var result = false;

            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@Year", Convert.ToString(year));
            dbParams[1] = new DbParameter("@PeriodNo", Convert.ToString(periodNo));

            // Get data
            result = DbRetrieve("GL.GetPeriodByYearPeriodNo", dsPeriod, dbParams, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get all Period data.
        /// </summary>
        /// <param name="dsPeriod"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsPeriod, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetPeriodList", dsPeriod, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get all Period data related to specified Year.
        /// </summary>
        /// <param name="dsPeriod"></param>
        /// <param name="year"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsPeriod, int year, string connStr)
        {
            var result = false;

            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];

            dbParams[0] = new DbParameter("@Year", Convert.ToString(year));

            // Get data
            result = DbRetrieve("GL.GetPeriodListByYear", dsPeriod, dbParams, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get the lastest closed period data.
        /// </summary>
        /// <param name="dsPeriod"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetLastestClose(DataSet dsPeriod, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetPeriodLastestClosed", dsPeriod, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get the lastest open period data.
        /// </summary>
        /// <param name="dsPeriod"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetLastestOpen(DataSet dsPeriod, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetPeriodLastestOpen", dsPeriod, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get StartDate of Period that related to specified Year and PeriodNo.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="periodNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DateTime GetStartDate(int year, int periodNo, string connStr)
        {
            var dtPeriod = new DataTable();
            //Nullable<DateTime> result = null;
            DateTime? result = Convert.ToDateTime(null);
            var dbParams = new DbParameter[2];


            // Generate parameter.
            dbParams[0] = new DbParameter("@Year", year.ToString());
            dbParams[1] = new DbParameter("@PeriodNo", periodNo.ToString());

            // Get data
            dtPeriod = DbRead("GL.GetPeriodByYearPeriodNo", dbParams, connStr);

            // Get start date.
            if (dtPeriod == null)
            {
                // return null.
                return (DateTime) result;
            }
            if (dtPeriod.Rows.Count > 0)
            {
                if (dtPeriod.Rows[0]["StartDate"] == DBNull.Value)
                {
                    // return null.
                    return (DateTime) result;
                }
                try
                {
                    // return correct value.
                    result = ((DateTime) dtPeriod.Rows[0]["StartDate"]);
                }
                catch (Exception ex)
                {
                    LogManager.Error(ex);
                    return (DateTime) result;
                }
            }

            // Return result
            return (DateTime) result;
        }

        /// <summary>
        ///     Get EndDate of Period that related to specified Year and PeriodNo.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="periodNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DateTime GetEndDate(int year, int periodNo, string connStr)
        {
            periodNo++;

            if (periodNo > Convert.ToInt32(12))
            {
                year++;
                periodNo = 1;
            }
            var dtPeriod = new DataTable();
            DateTime? result = Convert.ToDateTime(null);

            var dbParams = new DbParameter[2];


            // Generate parameter.
            dbParams[0] = new DbParameter("@Year", year.ToString());
            dbParams[1] = new DbParameter("@PeriodNo", periodNo.ToString());

            // Get data
            dtPeriod = DbRead("GL.GetPeriodByYearPeriodNo", dbParams, connStr);


            // Get end date.
            if (dtPeriod == null)
            {
                // return null.
                return (DateTime) result;
            }
            if (dtPeriod.Rows.Count > 0)
            {
                if (dtPeriod.Rows[0]["StartDate"] == DBNull.Value)
                {
                    // return null.
                    return (DateTime) result;
                }
                try
                {
                    // return correct value.
                    result = (((DateTime) dtPeriod.Rows[0]["StartDate"]).AddMonths(-1));
                }
                catch (Exception ex)
                {
                    LogManager.Error(ex);
                    return (DateTime) result;}
            }

            // Return result
            return (DateTime) result;
        }

        /// <summary>
        ///     Get period first open.
        /// </summary>
        /// <param name="dsPeriod"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetFirstOpen(DataSet dsPeriod, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetPeriodFirstOpen", dsPeriod, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Determine chkDate is in ClosedPerid or not.
        /// </summary>
        /// <param name="chkDate"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool IsClosedDate(DateTime chkDate, string connStr)
        {
            var result = false;
            var dsPeriod = new DataSet();
            var startDate = DateTime.Parse(string.Empty);
            var endDate = DateTime.Parse(string.Empty);


            result = GetLastestClose(dsPeriod, connStr);

            if (result) // yes
            {
                if (dsPeriod.Tables[TableName].Rows.Count > 0) // yes
                {
                    var year = (int) dsPeriod.Tables[TableName].Rows[0]["Year"];
                    var periodNo = (int) dsPeriod.Tables[TableName].Rows[0]["PeriodNo"];
                    periodNo++;

                    if (periodNo > 12)
                    {
                        year++;
                        periodNo = 1;
                    }

                    result = Get(dsPeriod, year, periodNo, connStr);

                    if (result) //yes
                    {
                        if (dsPeriod.Tables[TableName].Rows.Count > 0)
                        {
                            startDate = (DateTime) dsPeriod.Tables[TableName].Rows[0]["StartDate"];
                            dsPeriod.Clear();

                            result = GetLastestOpen(dsPeriod, connStr);

                            if (result) //yes
                            {
                                if (dsPeriod.Tables[TableName].Rows.Count > 0)
                                {
                                    endDate = ((DateTime) dsPeriod.Tables[TableName].Rows[0]["StartDate"]).AddDays(-1);

                                    if (chkDate >= startDate && chkDate <= endDate)
                                    {
                                        return false;
                                    }
                                    return true;
                                }
                                return true;
                            }
                            // B
                            return true;
                        }
                        return true; // c
                    }
                    // C
                    return true;
                }
                dsPeriod.Clear();

                result = GetFirstOpen(dsPeriod, connStr);

                if (result)
                {
                    if (dsPeriod.Tables[TableName].Rows.Count > 0)
                    {
                        startDate = (DateTime) dsPeriod.Tables[TableName].Rows[0]["StartDate"];
                        dsPeriod.Clear();

                        result = GetLastestOpen(dsPeriod, connStr);

                        if (result) //yes
                        {
                            if (dsPeriod.Tables[TableName].Rows.Count > 0)
                            {
                                endDate = ((DateTime) dsPeriod.Tables[TableName].Rows[0]["StartDate"]).AddDays(-1);

                                if (chkDate >= startDate && chkDate <= endDate)
                                {
                                    return false;
                                }
                                return true;
                            }
                            // c
                            return true;
                        }
                        // c
                        return true;
                    }
                    return true;
                }
                return true;
            }
            dsPeriod.Clear();

            result = GetFirstOpen(dsPeriod, connStr);

            if (result)
            {
                if (dsPeriod.Tables[TableName].Rows.Count > 0)
                {
                    startDate = (DateTime) dsPeriod.Tables[TableName].Rows[0]["StartDate"];
                    dsPeriod.Clear();

                    result = GetLastestOpen(dsPeriod, connStr);

                    if (result) //yes
                    {
                        if (dsPeriod.Tables[TableName].Rows.Count > 0)
                        {
                            endDate = ((DateTime) dsPeriod.Tables[TableName].Rows[0]["StartDate"]).AddDays(-1);

                            if (chkDate >= startDate && chkDate <= endDate)
                            {
                                return false;
                            }
                            return true;
                        }
                        // c
                        return true;
                    }
                    // c
                    return true;
                }
                return true;
            }
            return true;
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

        /// <summary>
        ///     Get max nearest period data from assigned year
        /// </summary>
        /// <returns></returns>
        public DataTable GetPeriodMaxNearest(int year, string connStr)
        {
            var dtPeriod = new DataTable();
            var dbParams = new DbParameter[1];

            // Create prameter
            dbParams[0] = new DbParameter("@Year", year.ToString());

            // Retrieve data
            dtPeriod = DbRead("GL.GetPeriodMaxNearest", dbParams, connStr);

            // Return result
            return dtPeriod;
        }

        /// <summary>
        ///     Get min nearest period data from assigned year
        /// </summary>
        /// <returns></returns>
        public DataTable GetPeriodMinNearest(int year, string connStr)
        {
            var dtPeriod = new DataTable();
            var dbParams = new DbParameter[1];

            // Create prameter
            dbParams[0] = new DbParameter("@Year", year.ToString());

            // Retrieve data
            dtPeriod = DbRead("GL.GetPeriodMinNearest", dbParams, connStr);

            // Return result
            return dtPeriod;
        }

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
        ///     Commit changed to Database.
        /// </summary>
        /// <param name="dsPeriod"></param>
        /// <param name="dbSaveSource"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsPeriod, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsPeriod, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }
    }
}