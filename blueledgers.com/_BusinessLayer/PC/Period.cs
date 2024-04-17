using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.PC
{
    public class Priod : DbHandler
    {
        public Priod()
        {
            SelectCommand = "SELECT * FROM [IN].[Period]";
            TableName = "Period";
        }

        public bool GetList(DataSet dsPeriod, int Year, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Year", Year.ToString());

            return DbRetrieve("[IN].GetPeriodListByYear", dsPeriod, dbParams, TableName, ConnStr);
        }

        public bool GetLatestOpen(DataSet dsPeriod, string ConnStr)
        {
            return DbRetrieve("[IN].GetPeriodLatestOpen", dsPeriod, null, TableName, ConnStr);
        }

        public DataTable GetPeriodListForDropDownList(string connStr)
        {
            var dtPeriod = new DataTable();

            // Get data
            dtPeriod = DbRead("[IN].GetPeriodForDropDownList", null, connStr);

            // Return result
            return dtPeriod;
        }

        public DateTime GetLatestOpenStartDate(string ConnStr)
        {
            var dsPeriod = new DataSet();

            var result = GetLatestOpen(dsPeriod, ConnStr);

            if (result)
            {
                if (dsPeriod.Tables[TableName].Rows.Count > 0)
                {
                    return DateTime.Parse(dsPeriod.Tables[TableName].Rows[0]["StartDate"].ToString()).Date;
                }
            }

            return DateTime.Parse("1900-01-01 00:00:00");
        }

        public DateTime GetLatestOpenEndDate(string ConnStr)
        {
            var dsPeriod = new DataSet();

            var result = GetLatestOpen(dsPeriod, ConnStr);

            if (result)
            {
                if (dsPeriod.Tables[TableName].Rows.Count > 0)
                {
                    return DateTime.Parse(dsPeriod.Tables[TableName].Rows[0]["EndDate"].ToString()).Date;
                }
            }

            return DateTime.Parse("1900-01-01 00:00:00");
        }

        /// <summary>
        /// </summary>
        /// <param name="CommittedDate"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public string GetStartDate(DateTime CommittedDate, string ConnStr)
        {
            var dtPeriod = new DataTable();

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CommittedDate", CommittedDate.ToString("yyyy-MM-dd"));

            dtPeriod = DbRead("[IN].[GetStartDate]", dbParams, ConnStr);

            if (dtPeriod.Rows.Count > 0)
            {
                return String.Format("{0:yyyy-MM-dd}", dtPeriod.Rows[0]["StartDate"]);
            }

            return string.Empty;
            //return DateTime.Parse("1900-01-01 00:00:00");
        }

        /// <summary>
        /// </summary>
        /// <param name="CommittedDate"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public string GetEndDate(DateTime CommittedDate, string ConnStr)
        {
            var dtPeriod = new DataTable();

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CommittedDate", CommittedDate.ToString("yyyy-MM-dd"));

            dtPeriod = DbRead("[IN].[GetEndDate]", dbParams, ConnStr);

            if (dtPeriod.Rows.Count > 0)
            {
                return String.Format("{0:yyyy-MM-dd}", dtPeriod.Rows[0]["EndDate"]);
            }

            return string.Empty;
            //return DateTime.Parse("1900-01-01 00:00:00");
        }

        public bool PeriodEnd(DateTime StartDate, DateTime EndDate, string LoginName, string ConnStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@StartDate", StartDate.ToString("yyyy-MM-dd"));
            dbParams[1] = new DbParameter("@EndDate", EndDate.ToString("yyyy-MM-dd"));
            dbParams[2] = new DbParameter("@LoginName", LoginName);

            DbExecuteQuery("EXEC [PC].PeriodEnd @StartDate, @EndDate, @LoginName", dbParams, ConnStr);

            return true;
        }

        public bool CheckEndDate(DateTime EndDate, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@EndDate", EndDate.ToString("yyy-MM-dd"));

            var countEop = DbReadScalar("[IN].GetCountEopByStatusAndEndDate", dbParams, ConnStr);
            var countLocation = DbReadScalar("[IN].GetCountLocationByEOP", null, ConnStr);

            if (countEop == countLocation)
            {
                return true;
            }
            return false;
        }

        public bool CountPeriod(string ConnStr)
        {
            var Count = 0;
            Count = DbReadScalar("[IN].[GetCountPeriod]", null, ConnStr);

            if (Count > 0)
            {
                return true;
            }
            return false;
        }

        public bool Save(DataSet dsPeriod, string ConnStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsPeriod, SelectCommand, TableName);

            return DbCommit(dbSaveSource, ConnStr);
        }

        public bool GetIsValidDate(DateTime TransDate, string LocationCode, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@TransDate", TransDate.ToString("yyyy-MM-dd"));
            dbParams[1] = new DbParameter("@LocationCode", LocationCode);

            var dtPeriod = new DataTable();

            // Get data
            dtPeriod = DbRead("[IN].GetIsValidDate", dbParams, ConnStr);

            return bool.Parse(dtPeriod.Rows[0][0].ToString());
        }
    }
}