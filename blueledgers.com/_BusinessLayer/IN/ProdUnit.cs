using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class ProdUnit : DbHandler
    {
        public ProdUnit()
        {
            SelectCommand = "SELECT * FROM [IN].[ProdUnit]";
            TableName = "ProdUnit";
        }

        public bool GetList(DataSet dsProduct, string ProductCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);

            var result = DbRetrieve("[IN].[ProdUnit_GetList]", dsProduct, dbParams, TableName, connStr);

            if (!result)
            {
                return false;
            }

            return true;
        }

        public bool GetListAll(DataSet dsProduct, string ProductCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);

            var result = DbRetrieve("[IN].[ProdUnit_GetforEdit]", dsProduct, dbParams, TableName, connStr);

            if (!result)
            {
                return false;
            }

            return true;
        }

        public bool GetListByUnitType(DataSet dsProduct, string ProductCode, string tableName, string UnitType,
            string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);
            dbParams[1] = new DbParameter("@UnitType", UnitType);

            var result = DbRetrieve("[IN].[GetProdUnitByProd_UnitType]", dsProduct, dbParams, tableName, connStr);

            if (!result)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsProduct"></param>
        /// <param name="ProductCode"></param>
        /// <param name="UnitType"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsProduct, string ProductCode, string UnitType, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);
            dbParams[1] = new DbParameter("@UnitType", UnitType);

            var result = DbRetrieve("[IN].[GetProdUnitByProd_UnitType]", dsProduct, dbParams, TableName, connStr);

            if (!result)
            {
                return false;
            }

            return true;
        }

        public bool GetSchema(DataSet dsProduct, string connStr)
        {
            return DbRetrieveSchema("[IN].[ProdUnit_Get]", dsProduct, null, TableName, connStr);
        }

        public bool GetSchema(DataSet dsProduct, string tableName, string connStr)
        {
            return DbRetrieveSchema("[IN].[ProdUnit_Get]", dsProduct, null, tableName, connStr);
        }

        public string GetInvenUnit(string ProductCode, string ConnStr)
        {
            var dsProduct = new DataSet();

            var result = GetList(dsProduct, ProductCode, ConnStr);

            if (result)
            {
                foreach (DataRow drProdUnit in dsProduct.Tables[TableName].Rows)
                {
                    if (decimal.Parse(drProdUnit["Rate"].ToString()) == 1)
                    {
                        return drProdUnit["OrderUnit"].ToString();
                    }
                }
            }

            return string.Empty;
        }

        public string GetInvenUnitType(string ProductCode, string ConnStr)
        {
            var dsProduct = new DataSet();

            var result = GetList(dsProduct, ProductCode, ConnStr);

            if (result)
            {
                foreach (DataRow drProdUnit in dsProduct.Tables[TableName].Rows)
                {
                    if (decimal.Parse(drProdUnit["Rate"].ToString()) == 1 &&
                        drProdUnit["UnitType"].ToString().ToUpper() == "I")
                    {
                        return drProdUnit["OrderUnit"].ToString();
                    }
                }
            }

            return string.Empty;
        }

        public DataTable GetLookUp_InvenUnit(string ProductCode, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);

            // Create parameters
            var dtLookUp = DbRead("[IN].[ProdUnit_GetListInven]", dbParams, ConnStr);

            return dtLookUp;
        }

        public string GetDefaultOrderUnit(string ProductCode, string ConnStr)
        {
            var dsProduct = new DataSet();

            var result = GetList(dsProduct, ProductCode, ConnStr);

            if (result)
            {
                foreach (DataRow drProdUnit in dsProduct.Tables[TableName].Rows)
                {
                    if (bool.Parse(drProdUnit["IsDefault"].ToString()) &&
                        drProdUnit["UnitType"].ToString().ToUpper() == "O")
                    {
                        return drProdUnit["OrderUnit"].ToString();
                    }
                }
            }

            return string.Empty;
        }

        public DataTable GetLookUp_ProductCode(string ProductCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);

            // Create parameters
            var dtLookUp = DbRead("[IN].[ProdUnit_GetList]", dbParams, connStr);

            return dtLookUp;
        }

        public DataTable GetLookUp_OrderUnitByProductCode(string ProductCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);

            // Create parameters
            var dtLookUp = DbRead("[IN].[ProdUnit_GetUnitTypeOrderUnit]", dbParams, connStr);

            return dtLookUp;
        }

        /// <summary>
        ///     Get Product unit data for edit.
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetUnitList(DataSet dsProdUnit, string ProductCode, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);

            return DbRetrieve("[IN].GetProdUnit_ProductCode", dsProdUnit, dbParams, "UnitList", ConnStr);
        }

        public int CountByProdUnitCode(string ProductCode, string OrderUnit, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);
            dbParams[1] = new DbParameter("@OrderUnit", OrderUnit);

            return DbReadScalar("[IN].[ProdUnit_CountProdUnitByProductCodeUnitCode]", dbParams, ConnStr);
        }

        public decimal GetConvRate(string ProductCode, string Unit, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);
            dbParams[1] = new DbParameter("@Unit", Unit);

            var dtGet = DbRead("[IN].[ProdUnit_GetConvRate]", dbParams, ConnStr);

            if (dtGet.Rows.Count > 0 && dtGet.Rows[0][0] != null && dtGet.Rows[0][0].ToString() != string.Empty)
            {
                return Convert.ToDecimal(dtGet.Rows[0][0].ToString());
            }

            return 0;
        }

        public decimal GetQtyAfterChangeUnit(string ProductCode, string NewUnit, string OldUnit, decimal Qty,
            string ConnStr)
        {
            var NewRate = GetConvRate(ProductCode, NewUnit, ConnStr);
            var OldRate = GetConvRate(ProductCode, OldUnit, ConnStr);

            if (NewRate != 0)
            {
                return (Qty*OldRate)/NewRate;
            }

            return 0;
        }

        /// <summary>
        ///     Save Change to data base.
        /// </summary>
        /// <param name="dsProdUnit"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsProdUnit, string ConnStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsProdUnit, SelectCommand, TableName);
            return DbCommit(dbSaveSource, ConnStr);
        }
    }
}