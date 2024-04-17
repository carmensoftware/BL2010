using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class ExchangeRate : DbHandler
    {
        #region "Attributies"

        public int ExRateID { get; set; }

        public DateTime EffDate { get; set; }

        public string FromCurrencyCode { get; set; }

        public string ToCurrencyCode { get; set; }

        public int BuyingExRate { get; set; }

        public int SellingExRate { get; set; }

        public int AverageExRate { get; set; }

        #endregion

        #region "Operations"

        public ExchangeRate()
        {
            SelectCommand = "SELECT * FROM Ref.ExRate";
            TableName = "ExRate";
        }

        /// <summary>
        ///     Return datatable from Currency.
        /// </summary>
        /// <returns></returns>
        public DataTable GetExchangeRateList(string connStr)
        {
            return DbRead("Reference.GetExRateList", null, connStr);
        }


        /// <summary>
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <returns></returns>
        public bool GetExchangeRateList(DataSet dsExRate, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Reference.GetExRatelist", dsExRate, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get currency lookup
        /// </summary>
        /// <param name="DepartmentCode"></param>
        /// <returns></returns>
        public DataTable GetCurrencyLookup(string connStr)
        {
            var dsExRate = new DataSet();

            // Get Data
            DbRetrieve("Reference.GetCurrencyLookup", dsExRate, null, TableName, connStr);

            // Return result
            return dsExRate.Tables[TableName];
        }

        /// <summary>
        ///     Get currency lookup
        /// </summary>
        /// <param name="DepartmentCode"></param>
        /// <returns></returns>
        public DataTable GetCurrencyLookupIsActive(string connStr)
        {
            var dsExRate = new DataSet();

            // Get Data
            DbRetrieve("Reference.GetCurrencyLookupIsActive", dsExRate, null, TableName, connStr);

            // Return result
            return dsExRate.Tables[TableName];
        }

        /// <summary>
        ///     Get currency lookup
        /// </summary>
        /// <param name="DepartmentCode"></param>
        /// <returns></returns>
        public DataTable GetExRateListLookup(string connStr)
        {
            var dsExRate = new DataSet();

            // Get Data
            DbRetrieve("Reference.GetExRateListLookup", dsExRate, null, TableName, connStr);

            // Return result
            return dsExRate.Tables[TableName];
        }

        /// <summary>
        ///     Get exchange relate to specified FCurrencyCode, TCurrencyCode and EffDate.
        /// </summary>
        /// <param name="FCurrencyCode"></param>
        /// <param name="TCurrencyCode"></param>
        /// <param name="EffDate"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public DataTable GetExRate(string FCurrencyCode, string TCurrencyCode, DateTime EffDate, string strConn)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@FCurrencyCode", FCurrencyCode);
            dbParams[1] = new DbParameter("@TCurrencyCode", TCurrencyCode);
            dbParams[2] = new DbParameter("@EffDate", EffDate.ToString());

            return DbRead("Ref.GetExRate_EffDate_F_T_CurrencyCode", dbParams, strConn);
        }

        /// <summary>
        ///     Save data to base
        /// </summary>
        /// <param name="dsExRate"></param>
        /// <returns></returns>
        public bool Save(DataSet dsExRate, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsExRate, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }


        /// <summary>
        ///     Get max disctrict Code.
        /// </summary>
        /// <returns></returns>
        public int GetExchangeRateCodeMax(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("Reference.GetExchangeRateCodeMax", null, connStr);

            // If return null value should be one.
            if (result == Convert.ToInt32("0"))
            {
                result = 1;
            }

            // Return result
            return result;
        }

        #endregion
    }
}