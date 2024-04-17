using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class Currency : DbHandler
    {
        #region "Attributies"

        public string CurrencyCode { get; set; }

        public string Name { get; set; }

        public decimal ExchangeRate { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int UpdatedBy { get; set; }

        #endregion

        #region "Operations"

        public Currency()
        {
            SelectCommand = "SELECT * FROM Ref.Currency";
            TableName = "Currency";
        }

        /// <summary>
        ///     Return datatable from Currency.
        /// </summary>
        /// <returns></returns>
        public DataTable GetCurrencyList(string connStr)
        {
            return DbRead("Reference.GetCurrencyList", null, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <returns></returns>
        public bool GetCurrencyList(DataSet dsCurrency, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Reference.GetCurrencylist", dsCurrency, null, TableName, connStr);

            if (result)
            {
                // Create primarykey                
                dsCurrency.Tables[TableName].PrimaryKey = GetPK(dsCurrency);
            }
            // Return result

            return result;
        }


        private DataColumn[] GetPK(DataSet dsCurrency)
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsCurrency.Tables[TableName].Columns["CurrencyCode"];

            return primaryKeys;
        }

        /// <summary>
        ///     Get currency lookup
        /// </summary>
        /// <param name="DepartmentCode"></param>
        /// <returns></returns>
        public DataTable GetCurrencyLookup(string connStr)
        {
            var dsCurrency = new DataSet();

            // Get Data
            DbRetrieve("Reference.GetCurrencyLookup", dsCurrency, null, TableName, connStr);

            // Return result
            return dsCurrency.Tables[TableName];
        }

        /// <summary>
        ///     Return datatable from exchage rate by currency code.
        /// </summary>
        /// <returns></returns>
        public Decimal GetExchangeRate(string CurrencyCode, string connStr)
        {
            var dtCurrency = new DataTable();
            decimal exRate = 0;
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@currency_Code", CurrencyCode);

            dtCurrency = DbRead("Reference.GetExchangeRateByCurrencyCode", dbParams, connStr);

            if (dtCurrency != null)
            {
                if (dtCurrency.Rows.Count > 0)
                {
                    return (dtCurrency.Rows[0]["ExchangeRate"] == DBNull.Value
                        ? exRate
                        : (decimal) dtCurrency.Rows[0]["ExchangeRate"]);
                }
            }

            return exRate;
        }

        /// <summary>
        ///     Get data by id.
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <param name="dsCurrency"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetByID(string currencyCode, DataSet dsCurrency, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@CurrencyCode", currencyCode);

            // Get data
            result = DbRetrieve("Reference.GetCurrencyByID", dsCurrency, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get name.
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string currencyCode, string connStr)
        {
            var dsCurrency = new DataSet();
            var result = string.Empty;

            // Get data
            GetByID(currencyCode, dsCurrency, connStr);

            // Return result
            if (dsCurrency.Tables[TableName].Rows.Count > 0)
            {
                result = dsCurrency.Tables[TableName].Rows[0]["Name"].ToString();
            }

            return result;
        }

        /// <summary>
        ///     Save data to base
        /// </summary>
        /// <param name="dsCurrency"></param>
        /// <returns></returns>
        public bool Save(DataSet dsCurrency, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsCurrency, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}