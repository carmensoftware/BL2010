using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.Setup.Application
{
    public class Currency : DbHandler
    {
        /// <summary>
        ///     Empty constructor
        /// </summary>
        public Currency()
        {
            SelectCommand = "SELECT * FROM Application.Currency";
            TableName = "Currency";
        }

        /// <summary>
        ///     Get Currency data by using CurrencyCode.
        /// </summary>
        /// <param name="dsCurrency"></param>
        /// <param name="currencyCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(ref DataSet dsCurrency, string currencyCode, string connStr)
        {
            var result = false;

            // Created parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@currencyCode", currencyCode);

            // Get data
            result = DbRetrieve("Application.GetCurrencyByCurrencyCode", dsCurrency, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all Currency data.
        /// </summary>
        /// <param name="dsCurrency"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(ref DataSet dsCurrency, string connStr)
        {
            var result = false;
            result = DbRetrieve("[Application].GetCurrencyList", dsCurrency, null, TableName, connStr);
            return result;
        }

        /// <summary>
        ///     Get active or inactive Currency data.
        /// </summary>
        /// <param name="dsCurrency"></param>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(ref DataSet dsCurrency, bool isActive, string connStr)
        {
            // Paramter value.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@isActive", isActive.ToString());

            return DbRetrieve("[Application].GetCurrencyListActive", dsCurrency, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get Currency Name by using CurrencyCode.
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string currencyCode, string connStr)
        {
            bool result;
            string currencyName;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@currencyCode", currencyCode);

            result = DbRetrieve("Application.GetCurrencyByCurrencyCode", dsTmp, dbParams, TableName, connStr);

            if (result)
            {
                if (dsTmp.Tables[TableName].Rows.Count > 0)
                {
                    if (dsTmp.Tables[TableName].Rows[0]["Name"].ToString() != string.Empty)
                    {
                        currencyName = dsTmp.Tables[TableName].Rows[0]["Name"].ToString();
                    }
                    else
                    {
                        currencyName = string.Empty;
                    }
                }
                else
                {
                    currencyName = string.Empty;
                }
            }
            else
            {
                currencyName = string.Empty;
            }
            return currencyName;
        }

        /// <summary>
        ///     Save data to base
        /// </summary>
        /// <param name="dsCurrency"></param>
        /// <returns></returns>
        public bool Save(DataSet dsCurrency, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[2];

            // Create DbSaveSource
            var currencyExRate = new CurrencyExRate();
            dbSaveSource[0] = new DbSaveSource(dsCurrency, currencyExRate.SelectCommand, currencyExRate.TableName);
            dbSaveSource[1] = new DbSaveSource(dsCurrency, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }
    }
}