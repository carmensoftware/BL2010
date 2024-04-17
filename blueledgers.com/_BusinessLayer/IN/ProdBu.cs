using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class ProdBu : DbHandler
    {
        public ProdBu()
        {
            SelectCommand = "SELECT * FROM [IN].[ProdBu]";
            TableName = "ProdBu";
        }

        /// <summary>
        ///     Get Product assigned to Business Unit by Product Code.
        /// </summary>
        /// <param name="dsProdBu"></param>
        /// <param name="ProductCode"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsProdBu, string ProductCode, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);
            return DbRetrieve("[IN].[GetProdBuList]", dsProdBu, dbParams, TableName, ConnStr);
        }

        /// <summary>
        ///     Get Product assigned to Business Unit by Product Code.
        /// </summary>
        /// <param name="dsProdBu"></param>
        /// <param name="ProductCode"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public DataTable GetList(string ProductCode, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);
            return DbRead("[IN].[GetProdBuList]", dbParams, ConnStr);
        }

        public bool Save(DataSet dsProdBu, string ConnStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsProdBu, SelectCommand, TableName);
            return DbCommit(dbSaveSource, ConnStr);
        }
    }
}