using System.Data;
using Blue.DAL;

namespace Blue.BL.Option.Inventory
{
    public class ProdCateType : DbHandler
    {
        public ProdCateType()
        {
            SelectCommand = "SELECT * FROM [IN].[ProdCateType]";
            TableName = "ProdCateType";
        }

        public DataTable GetList(string connStr)
        {
            // Create parameters
            return DbRead("[IN].[ProdCateType_GetActivedList]", null, connStr);
        }

        public bool GetList(DataSet dsProdCateType, string Code, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Code", Code);

            var result = DbRetrieve("[IN].[ProdCateType_GetList]", dsProdCateType, dbParams, TableName, connStr);

            if (!result)
            {
                return false;
            }

            return true;
        }

        public string GetName(string Code, string connStr)
        {
            var strName = string.Empty;
            var dsProdCateType = new DataSet();

            GetList(dsProdCateType, Code, connStr);

            if (dsProdCateType.Tables[TableName] != null)
            {
                if (dsProdCateType.Tables[TableName].Rows.Count > 0)
                {
                    strName = dsProdCateType.Tables[TableName].Rows[0]["Description"].ToString();
                }
            }

            return strName;
        }
    }
}