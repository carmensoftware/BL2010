using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Ref
{
    public class ExRate : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public ExRate()
        {
            SelectCommand = "SELECT * FROM Ref.ExRate";
            TableName = "ExRate";
        }

        public bool GetList(DataSet dsExRate, string ConnStr)
        {
            return DbRetrieve("dbo.Ref_ExRate_GetList", dsExRate, null, TableName, ConnStr);
        }

        /// <summary>
        ///     Get New ExRateID.
        /// </summary>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public int GetNewID(string ConnStr)
        {
            return DbReadScalar("dbo.REF_ExRate_GetNewID", null, ConnStr);
        }

        public DataTable Get(string FCurrencyCode, string TCurrencyCode, DateTime effDate, string connStr)
        {
            var result = new DataTable();
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[3];

            // Create parameter
            dbParams[0] = new DbParameter("@FCurrencyCode", FCurrencyCode);
            dbParams[1] = new DbParameter("@TCurrencyCode", TCurrencyCode);
            dbParams[2] = new DbParameter("@EffDate", effDate.ToString());

            // Get data
            //DbRetrieve("Ref.GetExRate_EffDate_F_T_CurrencyCode", dsTmp, dbParams, this.TableName, connStr);
            DbRetrieve("dbo.REF_ExRate_Get_F_T_EffDate", dsTmp, dbParams, TableName, connStr);

            // Return result
            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                result = dsTmp.Tables[TableName];
            }


            return result;
        }

        /// <summary>
        ///     Get exchange
        /// </summary>
        /// <param name="FCurrencyCode"></param>
        /// <param name="TCurrencyCode"></param>
        /// <param name="effDate"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public Double GetBuyingExRate(string FCurrencyCode, string TCurrencyCode, DateTime effDate, string connStr)
        {
            Double result = 0;

            //Get ExRate
            var dtGet = Get(FCurrencyCode, TCurrencyCode, effDate, connStr);

            // Return result
            if (dtGet.Rows.Count > 0)
            {
                result = Double.Parse(dtGet.Rows[0]["BuyingExRate"].ToString());
            }

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="FCurrencyCode"></param>
        /// <param name="TCurrencyCode"></param>
        /// <param name="effDate"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public Decimal GetBuyingExchangeRate(string FCurrencyCode, string TCurrencyCode, DateTime effDate,
            string connStr)
        {
            decimal result = 0;

            //Get ExRate
            var dtGet = Get(FCurrencyCode, TCurrencyCode, effDate, connStr);

            if (dtGet != null)
            {
                // Return result
                if (dtGet.Rows.Count > 0)
                {
                    result = Decimal.Parse(dtGet.Rows[0]["BuyingExRate"].ToString());
                }
            }

            return result;
        }


        public bool Save(DataSet dsExRate, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsExRate, SelectCommand, TableName);
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}