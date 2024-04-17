using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.Setup.Application
{
    public class CurrencyExRate : DbHandler
    {
        /// <summary>
        ///     Empty constructor.
        /// </summary>
        public CurrencyExRate()
        {
            SelectCommand = "SELECT * FROM [Application].CurrencyExRate";
            TableName = "CurrencyExRate";
        }

        /// <summary>
        ///     Get CurrencyExRate data by using CurrencyCode.
        /// </summary>
        /// <param name="dsCurrencyExRate"></param>
        /// <param name="currencyCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(ref DataSet dsCurrencyExRate, string currencyCode, string connStr)
        {
            var result = false;

            // Created parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@currencyCode", currencyCode);

            // Get data
            result = DbRetrieve("[Application].GetCurrencyExRateByCurrencyCode", dsCurrencyExRate, dbParams, TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all CurrencyExRate data by using CurrencyCode.
        /// </summary>
        /// <param name="dsCurrencyExRate"></param>
        /// <param name="currencyCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(ref DataSet dsCurrencyExRate, string currencyCode, string connStr)
        {
            var result = false;

            // Created parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@currencyCode", currencyCode);

            // Get data
            result = DbRetrieve("[Application].GetCurrencyExRateListByCurrencyCode", dsCurrencyExRate, dbParams,
                TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get valid ExRate of specified CurrencyCode
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public decimal GetExRate(string currencyCode, string connStr)
        {
            var dsCurrencyExRate = new DataSet();
            var currencyExRate = new CurrencyExRate();

            var result = currencyExRate.Get(ref dsCurrencyExRate, currencyCode, connStr);

            if (result)
            {
                if (dsCurrencyExRate.Tables[TableName].Rows[0]["Extra"].ToString() != string.Empty)
                {
                    return decimal.Parse(dsCurrencyExRate.Tables[TableName].Rows[0]["Extra"].ToString());
                }
                return decimal.Parse("1.00000");
            }
            return decimal.Parse("1.00000");
        }

        /// <summary>
        ///     Get valid ExRate of specified CurrencyCode
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public decimal GetExRatebyValidForm(string currencyCode, string connStr)
        {
            var dsCurrencyExRate = new DataSet();
            var currencyExRate = new CurrencyExRate();
            var result = false;

            // Created parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@currencyCode", currencyCode);

            // Get data
            result = DbRetrieve("Application.GetExchangeRatebyValidForm", dsCurrencyExRate, dbParams, TableName, connStr);

            if (result)
            {
                if (dsCurrencyExRate.Tables[TableName].Rows.Count > 0)
                {
                    if (dsCurrencyExRate.Tables[TableName].Rows[0]["ExRate"].ToString() != string.Empty)
                    {
                        return decimal.Parse(dsCurrencyExRate.Tables[TableName].Rows[0]["ExRate"].ToString());
                    }
                    return decimal.Parse("1.00000");
                }
                return decimal.Parse("1.00000");
            }
            return decimal.Parse("1.00000");
        }

        /// <summary>
        ///     Get valid ExRate List of specified CurrencyCode
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetExRateList(ref DataSet dsExRate, string currencyCode, string connStr)
        {
            var currencyExRate = new CurrencyExRate();

            var result = currencyExRate.GetList(ref dsExRate, currencyCode, connStr);

            return result;
        }

        /// <summary>
        ///     Get new ID
        /// </summary>
        /// <returns></returns>
        public int GetNewID(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("[Application].GetCurrencyExRateIDMax", null, connStr);

            // Return result
            return result;
        }
    }
}