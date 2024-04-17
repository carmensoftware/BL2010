using System.Data;
using Blue.DAL;

// Added By: Fon
using System.Data.SqlClient;
using System;

namespace Blue.BL.Ref
{
    public class Currency : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public Currency()
        {
            SelectCommand = "SELECT * FROM Ref.Currency";
            TableName = "Currency";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsVendor"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsCurrency, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("dbo.REF_CUR_GetList", dsCurrency, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all actived currency
        /// </summary>
        /// <param name="dsCurrency"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetActiveList(DataSet dsCurrency, string ConnStr)
        {
            return DbRetrieve("dbo.REF_CUR_GetActiveList", dsCurrency, null, TableName, ConnStr);
        }

        /// <summary>
        ///     Get all actived currency
        /// </summary>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public DataTable GetActiveList(string ConnStr)
        {
            var dsCurrency = new DataSet();

            var result = GetActiveList(dsCurrency, ConnStr);

            if (result)
            {
                return dsCurrency.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get name by currency code
        /// </summary>
        /// <param name="CurrencyCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string CurrencyCode, string connStr)
        {
            var strName = string.Empty;

            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CurrencyCode", CurrencyCode);

            var dtName = DbRead("dbo.REF_CUR_GetName_Code", dbParams, connStr);

            if (dtName != null)
            {
                if (dtName.Rows.Count > 0)
                {
                    strName = dtName.Rows[0]["Desc"].ToString();
                }
            }

            return strName;
        }

        public bool Save(DataSet dsCurrency, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsCurrency, SelectCommand, TableName);
            return DbCommit(dbSaveSource, connStr);
        }

        // Added By: Fon, on: 2017-08-02
        public decimal GetLastCurrencyRate(string currCode, DateTime date, string connStr)
        {
            decimal lastRate = 0.0m;
            object[] getResult = GetDataTableByCodeDate(currCode, date, connStr);
            if (Convert.ToBoolean(getResult[0]))
            {
                DataTable dt = (DataTable)getResult[1];
                lastRate = (dt.Rows.Count > 0)
                         ? Convert.ToDecimal(dt.Rows[0]["CurrencyRate"])
                         : lastRate;
                /* May be DataTable don't even have a record.*/
            }

            return lastRate;
        }

        public DataTable GetLastCurrencyRate(string connStr)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connStr);
            string sqlStr = ";WITH group_ce AS";
            sqlStr += " ( SELECT MAX([InputDate]) AS [InputDate], [CurrencyCode]";
            sqlStr += " FROM [Ref].[CurrencyExchange] GROUP BY [CurrencyCode] )";
            sqlStr += " SELECT ce.[InputDate], c.[CurrencyCode]";
            sqlStr += " , ': ' + c.[Desc] AS [Description], ce.[CurrencyRate]";
            sqlStr += " , ce.UpdatedDate, ce.UpdatedBy";
            sqlStr += " FROM [Ref].[CurrencyExchange] AS ce";
            sqlStr += " INNER JOIN group_ce ON ce.InputDate = group_ce.InputDate";
            sqlStr += " AND ce.CurrencyCode = group_ce.CurrencyCode";
            sqlStr += " INNER JOIN [Ref].[Currency] AS c ON group_ce.CurrencyCode = c.CurrencyCode";
            sqlStr += " WHERE c.IsActived = 1";

            conn.Open();
            SqlCommand cmd = new SqlCommand(sqlStr, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();

            return dt;
        }

        protected object[] GetDataTableByCodeDate(string currCode, DateTime date, string connStr)
        {
            object[] result = new object[2];
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connStr);
            string formatedDate = date.ToString("yyyy-MM-dd");

            string sqlStr = string.Format("SELECT TOP(1) *");
            sqlStr += string.Format(" FROM [Ref].[CurrencyExchange]");
            sqlStr += string.Format(" WHERE [InputDate] <= '{0}'", formatedDate);
            sqlStr += string.Format(" AND [CurrencyCode] = '{0}'", currCode);
            sqlStr += string.Format(" ORDER BY [InputDate] DESC");

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conn.Close();
                result[0] = true;
                result[1] = dt;
            }
            catch (Exception ex)
            {
                conn.Close();
                result[0] = false;
                result[1] = ex.Message;
            }
            return result;
        }
        // End of 2017-08-02

        #endregion
    }
}