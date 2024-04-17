using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class AdjType : DbHandler
    {
        public AdjType()
        {
            TableName = "[IN].[AdjType]";
            SelectCommand = "SELECT * FROM [IN].[AdjType]";
        }

        public bool Save(DataSet dsAdjType, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsAdjType, SelectCommand, TableName);
            return DbCommit(dbSaveSource, connStr);
        }

        //Get Schema
        public bool GetSchema(DataSet dsAdjType, string ConnStr)
        {
            return DbRetrieve("[IN].[AdjType_GetSchema]", dsAdjType, null, TableName, ConnStr);
        }

        public int GetNewID(string connStr)
        {
            var newID = DbReadScalar("[IN].[AdjType_GetNewID]", null, connStr);

            // Return result
            return newID;
        }

        public int Get_CountByType(string Type, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Type", Type);

            return DbReadScalar("[IN].[AdjType_CountByType]", dbParams, ConnStr);
        }

        public int Get_CountByCodeType(string AdjType, string AdjCode, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@AdjType", AdjType);
            dbParams[1] = new DbParameter("@AdjCode", AdjCode);

            return DbReadScalar("[IN].[StockIn_CountByAdjTypeCode]", dbParams, ConnStr);
        }

        public bool GetList(DataSet dsAdjType, string AdjType, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AdjType", AdjType);

            return DbRetrieve("[IN].[AdjType_GetlistByAdjType]", dsAdjType, dbParams, TableName, ConnStr);
        }

        public DataTable GetList(string AdjType, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AdjType", AdjType);

            return DbRead("[IN].[AdjType_GetlistByAdjType]", dbParams, ConnStr);
        }

        public DataTable GetList(string ConnStr)
        {
            return DbExecuteQuery(SelectCommand, null, ConnStr);
        }

        public string GetName(string AdjId, string connStr)
        {
            var strName = string.Empty;

            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AdjId", AdjId);

            var drUnit = DbRead("[IN].[AdjType_GetName]", dbParams, connStr);

            if (drUnit.Rows.Count > 0)
            {
                strName = drUnit.Rows[0]["Name"].ToString();
            }

            return strName;
        }

        // Created by Ake (2013-11-27)
        public string GetAdjType(string adjID, string connStr)
        {
            var strName = string.Empty;

            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@adjID", adjID);

            var drUnit = DbRead("[IN].[AdjType_GetSchema]", dbParams, connStr);

            if (drUnit.Rows.Count > 0)
            {
                strName = drUnit.Rows[0]["AdjType"].ToString();
            }

            return strName;
        }
    }
}