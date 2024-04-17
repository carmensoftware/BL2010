using System.Data;
using Blue.DAL;

namespace Blue.BL.Option.Inventory
{
    public class ProdLoc : DbHandler
    {
        public ProdLoc()
        {
            SelectCommand = "SELECT * FROM [In].[ProdLoc]";
            TableName = "ProdLoc";
        }

        /// <summary>
        /// </summary>
        /// <param name="CateogryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetProdName_LocaCode(string LocaCode, string CateCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@LocaCode", LocaCode);
            dbParams[1] = new DbParameter("@CateCode", CateCode);

            return DbRead("dbo.ProdLoc_GetNameList_LocaCode_CateCode", dbParams, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="LocaCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string LocatCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LocatCode", LocatCode);

            //return DbRead("dbo.ProdLoc_GetSelectProduct", dbParams, connStr);
            return DbRead("dbo.ProdLoc_GetList_LocatCode", dbParams, connStr);
        }

        public bool GetListByLocation(DataSet dsProdLoc, string LocatCode, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LocatCode", LocatCode);

            return DbRetrieve("dbo.ProdLoc_GetList_LocatCode", dsProdLoc, dbParams, TableName, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsProdLoc"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetList(DataSet dsProdLoc, string connStr)
        {
            // Create parameters
            DbRetrieve("dbo.ProdLoc_GetList", dsProdLoc, null, TableName, connStr);

            return dsProdLoc;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsProduct"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsProdLoc, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsProdLoc, SelectCommand, TableName);
            return DbCommit(dbSaveSource, connStr);
        }


        /// <summary>
        /// </summary>
        /// <param name="dsProdLoc"></param>
        /// <param name="ProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsProdLoc, string ProductCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);

            var result = DbRetrieve("dbo.ProdLoc_GetList_ProductCode", dsProdLoc, dbParams, TableName, connStr);

            if (!result)
            {
                return false;
            }

            return true;
        }

        public bool GetList_ProductCode(DataSet dsProdLoc, string ProductCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);

            var result = DbRetrieve("[IN].[ProdLoc_GetlistByProductCode]", dsProdLoc, dbParams, TableName, connStr);

            if (!result)
            {
                return false;
            }

            return true;
        }

        public DataTable GetProductNameByProdLoc(string LocationCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);

            //return DbRead("dbo.ProdLoc_GetSelectProduct", dbParams, connStr);
            return DbRead("dbo.ProdLoc_GetProductListByLocationCode", dbParams, connStr);
        }

        public int CountByProductCode(string ProductCode, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);

            return DbReadScalar("[IN].[ProdLoc_CountProductByProductCode]", dbParams, ConnStr);
        }
    }
}