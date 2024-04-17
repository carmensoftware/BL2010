using System.Data;
using Blue.DAL;
using System;


namespace Blue.BL.PT.RCP
{
    public class RcpDt : DbHandler
    {
        public RcpDt()
        {
            SelectCommand = "SELECT * FROM [PT].[RcpDt]";
            TableName = "RcpDt";

        }

        /// <summary>
        ///     Get structure
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet ds, string ConnStr)
        {
            return DbRetrieveSchema("[PT].[GetSchemaRcpDt]", ds, null, TableName, ConnStr);
        }


        /// <summary>
        ///     Get data by Recipe no.
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="MsgError"></param>
        /// <param name="rcpNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet ds, string rcpNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RcpNo", rcpNo);

            var result = DbRetrieve("[PT].[GetListRcpDt]", ds, dbParams, TableName, connStr);
            return (bool)result;
        }

        /// <summary>
        /// </summary>
        /// <param name="rcpNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        //public DataTable GetList(string rcpNo, string connStr)
        //{
        //    var dbParams = new DbParameter[1];
        //    dbParams[0] = new DbParameter("@RcpNo", rcpNo);

        //    return DbRead("[PT].[GetListRcpDt]", dbParams, connStr);
        //}


        /// <summary>
        /// </summary>
        /// <param name="dsSaving"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Save(DataSet dsSaving, string strConn)
        {
            // Build savesource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSaving, SelectCommand, TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, strConn);
        }

        public DataTable GetListItem(string rcpCode, string connStr)
        {
            string sqlSelect = string.Empty;
            sqlSelect = "SELECT DISTINCT u.ProductCode as IngredientCode, p.ProductDesc1 as IngredientName, 'P' as IngredientType";
            sqlSelect += " FROM [IN].ProdUnit u";
            sqlSelect += " LEFT JOIN [IN].Product p ON u.ProductCode = p.ProductCode";
            sqlSelect += " WHERE u.UnitType = 'R'";
            sqlSelect += " UNION";
            sqlSelect += " SELECT RcpCode, RcpDesc1, 'R'";
            sqlSelect += " FROM PT.Rcp";
            sqlSelect += " WHERE RcpCode <> '" + rcpCode + "'";
            sqlSelect += " ORDER BY IngredientCode";

            return DbExecuteQuery(sqlSelect, null, connStr);

            //var dbParams = new DbParameter[1];
            //dbParams[0] = new DbParameter("@RcpCode", rcpCode);

            //return DbRead("[PT].[GetListRcpDtIngredient]", dbParams, connStr);

        }



        public DataTable GetListUnit(string productCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", productCode);

            string sqlSelect = string.Empty;
            sqlSelect = "SELECT OrderUnit as UnitCode, Rate as UnitRate FROM [IN].ProdUnit WHERE UnitType = 'R' AND ProductCode = @ProductCode";
            sqlSelect += " UNION";
            sqlSelect += " SELECT RcpUnitCode as UnitCode, 1 as UnitRate FROM PT.Rcp WHERE RcpCode = @ProductCode";

            return DbExecuteQuery(sqlSelect, dbParams, connStr);

        }

        public string GetItemDesc1(string productCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", productCode);

            string sqlSelect = string.Empty;
            sqlSelect = "SELECT TOP(1) ProductDesc1 as ItemDesc FROM [IN].Product WHERE ProductCode = @ProductCode";
            sqlSelect += " UNION";
            sqlSelect += " SELECT TOP(1) RcpDesc1 as ItemDesc FROM PT.Rcp WHERE RcpCode = @ProductCode";

            DataTable dt = DbExecuteQuery(sqlSelect, dbParams, connStr);
            if (dt.Rows.Count == 0)
                return string.Empty;
            else
                return dt.Rows[0][0].ToString();

        }

        public string GetItemDesc2(string productCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", productCode);

            string sqlSelect = string.Empty;
            sqlSelect = "SELECT TOP(1) ProductDesc2 as ItemDesc FROM [IN].Product WHERE ProductCode = @ProductCode";
            sqlSelect += " UNION";
            sqlSelect += " SELECT TOP(1) RcpDesc2 as ItemDesc FROM PT.Rcp WHERE RcpCode = @ProductCode";

            DataTable dt = DbExecuteQuery(sqlSelect, dbParams, connStr);
            if (dt.Rows.Count == 0)
                return string.Empty;
            else
                return dt.Rows[0][0].ToString();

        }
        public string GetUnitDefault(string productCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", productCode);

            string sqlSelect = string.Empty;
            sqlSelect = "SELECT TOP(1) OrderUnit as UnitCode FROM [IN].ProdUnit WHERE UnitType = 'R' AND IsDefault = 1 AND ProductCode = @ProductCode";
            sqlSelect += " UNION";
            sqlSelect += " SELECT TOP(1) RcpUnitCode as UnitCode FROM PT.Rcp WHERE RcpCode = @ProductCode";

            DataTable dt = DbExecuteQuery(sqlSelect, dbParams, connStr);
            if (dt.Rows.Count == 0)
                return string.Empty;
            else
                return dt.Rows[0][0].ToString();

        }

        public string GetBaseUnit(string productCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", productCode);

            string sqlSelect = string.Empty;
            sqlSelect = "SELECT TOP(1) InventoryUnit as BaseUnit FROM [IN].Product WHERE ProductCode = @ProductCode";
            sqlSelect += " UNION";
            sqlSelect += " SELECT TOP(1) RcpUnitCode as BaseUnit FROM PT.Rcp WHERE RcpCode = @ProductCode";

            DataTable dt = DbExecuteQuery(sqlSelect, dbParams, connStr);
            if (dt.Rows.Count == 0)
                return string.Empty;
            else
                return dt.Rows[0][0].ToString();

        }

        public decimal GetUnitConvRate(string productCode, string unitCode, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@ProductCode", productCode);
            dbParams[1] = new DbParameter("@UnitCode", unitCode);

            string sqlSelect = string.Empty;
            sqlSelect = "SELECT TOP(1) Rate FROM [IN].ProdUnit WHERE UnitType = 'R' AND OrderUnit = @UnitCode AND ProductCode = @ProductCode";
            sqlSelect += " UNION";
            sqlSelect += " SELECT TOP(1) 1 FROM PT.Rcp WHERE RcpCode = @ProductCode";

            DataTable dt = DbExecuteQuery(sqlSelect, dbParams, connStr);

            if (dt.Rows.Count > 0 && dt.Rows[0][0] != null && dt.Rows[0][0].ToString() != string.Empty)
                return System.Convert.ToDecimal(dt.Rows[0][0].ToString());
            else
                return 0;
        }

        private DataTable GetRecipeOfIngredient(string rcpCode, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RcpCode", rcpCode);

            string sqlSelect = string.Empty;
            sqlSelect += " SELECT IngredientCode FROM PT.RcpDt WHERE IngredientType = 'R' AND RcpCode = @RcpCode";

            return DbExecuteQuery(sqlSelect, dbParams, connStr);
        }

        public bool IsRecursiveRecipe(string rcpCode, ref string ingredientCode, string connStr)
        {
            DataTable dt = GetRecipeOfIngredient(ingredientCode, connStr);

            if (dt.Rows.Count == 0)
                return false;

            bool isFound = false;
            // Check recursive recipe
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["IngredientCode"].ToString() == rcpCode)
                {
                    isFound = true;
                    break;
                }
            }

            if (isFound)
                return true;
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ingredientCode = dr["IngredientCode"].ToString();
                    isFound = IsRecursiveRecipe(rcpCode, ref ingredientCode, connStr);
                }
                return isFound;
            }
        }

        public string GetRecursiveRecipe(string IngredientCode)
        {

            return string.Empty;
        }

        #region
        //public decimal GetCost(string ingredientType, string ingredientCode, string connStr)
        //{
        //    var dbParams = new DbParameter[2];
        //    dbParams[0] = new DbParameter("@IngredientType", ingredientType);
        //    dbParams[1] = new DbParameter("@IngredientCode", ingredientCode);

        //    DataTable dt = DbRead("[PT].[GetIngredientCost]", dbParams, connStr);

        //    if (dt.Rows[0][0] == null)
        //        return 0;
        //    else
        //        return System.Decimal.Round(System.Convert.ToDecimal(dt.Rows[0][0].ToString()), 2);
        //}
        #endregion

        // Modified on: 07/09/2017, By: Fon, For: Add dates 's parameter 
        public decimal GetLastCost(string ingredientType, string ingredientCode, DateTime date, string connStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@IngredientCode", ingredientCode);
            dbParams[1] = new DbParameter("@IngredientType", ingredientType);
            dbParams[2] = new DbParameter("@Date", date.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            decimal cost = 0;
            DataTable dt = DbRead("[PT].[GetLastCost]", dbParams, connStr);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0][0] != DBNull.Value)
                    cost = System.Decimal.Round(System.Convert.ToDecimal(dt.Rows[0][0].ToString()), 2);
            }
            return cost;
        }

        // Added on: 12/09/2017, By: Fon, For: Update cost of Recipe
        public bool UpdateCostOfRecipe(DbParameter[] dbParams, string connStr)
        {
            string cmd = string.Format("EXEC {0}", "[PT].[UpdateRecipeCost]");
            for (int i = 0; i <= dbParams.Length - 1; i++)
            {
                cmd += dbParams[i].ParameterName + (i == dbParams.Length - 1 ? string.Empty : " , ");
            }

            var result = DbExecuteQuery(cmd, dbParams, connStr);
            return (result != null) ? true : false;
        }

        // Added on: 15/09/2017, By: Fon, For: Update cost of recipe list
        public bool UpdateCostOfRcpLst(DbParameter[] dbParams, string connStr)
        {
            string cmd = string.Format("EXEC {0}", "[PT].[UpdateCostOfRcpLst]");
            for (int i = 0; i <= dbParams.Length - 1; i++)
            {
                cmd += dbParams[i].ParameterName + (i == dbParams.Length - 1 ? string.Empty : " , ");
            }

            var result = DbExecuteQuery(cmd, dbParams, connStr);
            return (result != null) ? true : false;
        }

        // Added on: 18/09/2017, By: Fon, Get new RcpCode
        public string GetNewRcpCode(string connStr)
        {
            string newCode = string.Empty;
            string cmd = string.Format("EXEC {0}", "[PT].[GenerateRcpCode]");
            var dbParams = new DbParameter[0];
            DataTable dt = DbExecuteQuery(cmd, dbParams, connStr);

            if(dt.Rows.Count > 0)
                newCode = dt.Rows[0][0].ToString();

            return newCode;
        }
    }
}
