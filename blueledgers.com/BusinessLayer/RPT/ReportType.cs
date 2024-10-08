﻿using System.Data;
using Blue.DAL;

namespace Blue.BL.RPT
{
    public class ReportType : DbHandler
    {
        #region "Operations"

        public ReportType()
        {
            SelectCommand = "SELECT * FROM RPT.ReportType";
            TableName = "ReportType";
        }


        /// <summary>
        ///     Get all of datasource list
        /// </summary>
        /// <param name="dsDataSource"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public DataTable GetListActive(string connStr)
        {
            return DbRead("RPT.GetReportTypeActiveList", null, connStr);
        }

        #endregion
    }
}