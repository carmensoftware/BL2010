using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Blue.DAL;

namespace Blue.BL.Option.Inventory
{
    public class ProdLocHQ : DbHandler
    {
        // Created on: 03/04/2018, By: Fon
        public ProdLocHQ()
        {
            SelectCommand = "SELECT * FROM [IN].[ProdLocHQ]";
            TableName = "ProdLocHQ";
        }

        public bool GetList(DataSet dsProdLocHQ, string connStrHQ)
        {
            return DbRetrieve("[IN].[GetProdLocHQByProdLocBU]", dsProdLocHQ, null, TableName, connStrHQ);
        }

        public bool GetList(DataSet dsProdLocHQ, string buCode, string connStrHQ)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@buCode", buCode);

            return DbRetrieve("[IN].[GetProdLocHQByProdLocBU]", dsProdLocHQ, dbParams, TableName, connStrHQ);
        }

        public bool GetListByProductCodeBU(DataSet dsProdLocHQ, string buCode, string productCodeBU, string connStrHQ)
        {
            // Create parameters
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@buCode", buCode);
            dbParams[1] = new DbParameter("@productCodeBU", productCodeBU);
            dbParams[2] = new DbParameter("@locationCodeBU", null);

            var result = DbRetrieve("[IN].[GetProdLocHQByProdLocBU]", dsProdLocHQ, dbParams, TableName, connStrHQ);
            return result;
        }

        public DataTable GetListByProdLocBU(string buCode, string productCodeBU, string locationBU, string connStrHQ)
        {
            // Create Parameter
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@buCode", buCode);
            dbParams[1] = new DbParameter("@productCodeBU", productCodeBU);
            dbParams[2] = new DbParameter("@locationCodeBU", locationBU);

            return DbRead("[IN].[GetProdLocHQByProdLocBU]", dbParams, connStrHQ);
        }

        public string GetProductCodeHQ(string buCode, string productCodeBu, string connStrHQ)
        {
            string productCodeHQ = string.Empty;
            DataSet dsProdLocHQ = new DataSet();

            if (GetListByProductCodeBU(dsProdLocHQ, buCode, productCodeBu, connStrHQ))
            {
                if (dsProdLocHQ.Tables.Contains(TableName))
                {
                    if (dsProdLocHQ.Tables[TableName].Rows.Count > 0)
                        productCodeBu = dsProdLocHQ.Tables[TableName].Rows[0]["ProductCodeHQ"].ToString();
                }
            }

            return productCodeHQ;
        }

        public bool Save(DataSet dsProdLocHQ, string connStrHQ)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsProdLocHQ, SelectCommand, TableName);
            return DbCommit(dbSaveSource, connStrHQ);
        }
    }
}
