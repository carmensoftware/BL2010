using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class VendorProd : DbHandler
    {
        public VendorProd()
        {
            SelectCommand = "SELECT * FROM [IN].[VendorProd]";
            TableName = "VendorProd";
        }

        public bool GetSchema(DataSet dsVendorProd, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", string.Empty);
            return DbRetrieveSchema("[IN].[GetProdVendorList]", dsVendorProd, dbParams, TableName, ConnStr);
        }

        public bool GetVendorProdList(DataSet dsVendorProd, string ProductCode, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);
            return DbRetrieve("[IN].[GetVendorProdList]", dsVendorProd, dbParams, TableName, ConnStr);
        }

        public bool GetList(DataSet dsVendorProd, string ProductCode, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);
            return DbRetrieve("[IN].[GetProdVendorList]", dsVendorProd, dbParams, TableName, ConnStr);
        }

        public DataTable GetList(string ProductCode, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);
            return DbRead("[IN].[GetProdVendorList]", dbParams, ConnStr);
        }

        public bool Save(DataSet dsVendorProd, string ConnStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsVendorProd, SelectCommand, TableName);
            return DbCommit(dbSaveSource, ConnStr);
        }
    }
}