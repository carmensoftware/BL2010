using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class Movement : DbHandler
    {
        public Movement()
        {
            TableName = "[IN].[Movement]";
            SelectCommand = "SELECT * FROM [IN].[Movement]";
        }

        public bool GetStructure(DataSet dsMovement, string connStr)
        {
            return DbRetrieveSchema("[IN].[GetMovementSchema]", dsMovement, null, TableName, connStr);
        }

        public bool GetList(DataSet dsMovement, string RefId, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefId", RefId);

            return DbRetrieve("[IN].[GetMovementByRefId]", dsMovement, dbParams, TableName, connStr);
        }

        public bool Save(DataSet dsMovement, string connStr)
        {
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsMovement, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsMovement, new MovementDt().SelectCommand, new MovementDt().TableName);
            return DbCommit(dbSaveSource, connStr);
        }

        //Get New Id.
        public string GetNewID(string SubModule, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@SubModule", SubModule);

            var dtMovement = DbRead("[IN].[MoveMent_GetNewID]", dbParams, ConnStr);

            if (dtMovement != null)
            {
                if (dtMovement.Rows.Count > 0)
                {
                    return dtMovement.Rows[0][0].ToString();
                }
            }

            return string.Empty;
        }
    }
}