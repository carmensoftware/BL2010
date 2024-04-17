using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.APP
{
    public class TAX : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empy contructor.
        /// </summary>
        public TAX()
        {
            SelectCommand = "SELECT * FROM [APP].[TAX]";
            TableName = "TAX";
        }

        /// <summary>
        ///     Get active tax data.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetActiveList(string connStr)
        {
            return DbRead("[APP].GetTaxActiveList", null, connStr);
        }

        /// <summary>
        ///     Get TAX data by Code.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable Get(string code, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Code", code);

            return DbRead("[APP].GetTaxByCode", dbParams, connStr);
        }

        /// <summary>
        ///     Get TAX Rate by Code.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public decimal GetRate(string code, string connStr)
        {
            var dtTax = Get(code, connStr);

            if (dtTax == null)
            {
                return decimal.Zero;
            }

            if (dtTax.Rows.Count == 0)
            {
                return decimal.Zero;
            }

            return decimal.Parse(dtTax.Rows[0]["Rate"].ToString());
        }

        /// <summary>
        ///     Get Tax Name by Tax Code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string code, string connStr)
        {
            var dtTax = Get(code, connStr);

            if (dtTax == null)
            {
                return string.Empty;
            }

            if (dtTax.Rows.Count == 0)
            {
                return string.Empty;
            }

            return dtTax.Rows[0]["Name"].ToString();
        }

        #endregion
    }
}