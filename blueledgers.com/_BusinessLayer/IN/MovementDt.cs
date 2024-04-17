using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class MovementDt : DbHandler
    {
        public MovementDt()
        {
            TableName = "[IN].[MovementDt]";
            SelectCommand = "SELECT * FROM [IN].[MovementDt]";
        }

        public bool GetList(DataSet dsMovementDt, string RefId, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefId", RefId);

            return DbRetrieve("[IN].[GetMovementDtListByRefId]", dsMovementDt, dbParams, TableName, connStr);
        }

        public bool GetSchema(DataSet dsMovementDt, string connStr)
        {
            return DbRetrieve("[IN].[MovementDt_GetList]", dsMovementDt, null, TableName, connStr);
        }

        public decimal GetQty(string RefId, string DtId, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@RefId", RefId);
            dbParams[1] = new DbParameter("@DtId", DtId);

            var dtGet = new DataTable();
            dtGet = DbRead("[IN].[GetMovementQty]", dbParams, connStr);

            if (dtGet.Rows.Count > 0)
            {
                return Convert.ToDecimal(dtGet.Rows[0][0]);
            }

            return 0;
        }

        public decimal GetTotalQty(string RefId, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefId", RefId);

            var dtGet = new DataTable();
            dtGet = DbRead("[IN].[GetMovementTotalQty]", dbParams, connStr);

            if (dtGet.Rows.Count > 0)
            {
                return decimal.Parse(dtGet.Rows[0][0].ToString());
            }

            return 0;
        }
    }
}