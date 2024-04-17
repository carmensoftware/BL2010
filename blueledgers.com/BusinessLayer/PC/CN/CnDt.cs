using System.Data;
using Blue.DAL;

namespace Blue.BL.PC.CN
{
    public class CnDt : DbHandler
    {
        public CnDt()
        {
            SelectCommand = "SELECT * FROM [PC].[CnDt]";
            TableName = "CnDt";
        }

        public int CountByLocationCode(string Location, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LocateCode", Location);

            //return DbReadScalar("dbo.PC_CnDt_CountByLocationCode", dbParams, ConnStr);
            return DbReadScalar("PC.GetCnDtCountByLocationCode", dbParams, ConnStr);
        }

        public DataTable GetListByCnNo(string CnNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CnNo", CnNo);

            return DbRead("[PC].[GetCnDtByCnNo]", dbParams, connStr);
        }

        /// <summary>
        ///     Get data by receive no.
        /// </summary>
        /// <param name="dsCnDt"></param>
        /// <param name="MsgError"></param>
        /// <param name="CnNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListByCnNo(DataSet dsCnDt, ref string MsgError, string CnNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CnNo", CnNo);

            var result = DbRetrieve("[PC].[GetCnDtByCnNo]", dsCnDt, dbParams, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            dsCnDt.Tables[TableName].PrimaryKey = GetPK(dsCnDt);
            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsCnDt"></param>
        /// <param name="MsgError"></param>
        /// <param name="CnNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetCnDtByPoNoAndPoDtNo(DataSet dsCnDt, string poNo, int poDtNo, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@PONo", poNo);
            dbParams[1] = new DbParameter("@PoDtNo", poDtNo.ToString());

            result = DbRetrieve("[PC].[GetCnDtByPoNoAndPoDtNo]", dsCnDt, dbParams, TableName, connStr);

            return result;
        }

        /// <summary>
        ///     Get Data to dataset.
        /// </summary>
        /// <param name="dsCnDt"></param>
        /// <param name="CnNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetListByCnNo(DataSet dsCnDt, string CnNo, string connStr)
        {
            var MsgError = string.Empty;

            var result = GetListByCnNo(dsCnDt, ref MsgError, CnNo, connStr);

            if (result)
            {
                return dsCnDt;
            }
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="CnNo"></param>
        /// <param name="CnDtNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetCnDetailByCnNoAndCnDtNo(string CnNo, int CnDtNo, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@CnNo", CnNo);
            dbParams[1] = new DbParameter("@CnDtNo", CnDtNo.ToString());

            return DbRead("[PC].[GetCnDetailByCnNoAndCnDtNo]", dbParams, connStr);
        }

        /// <summary>
        ///     Get structure
        /// </summary>
        /// <param name="dsCnDt"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsCnDt, string ConnStr)
        {
            return DbRetrieveSchema("[PC].[GetCnDtSchema]", dsCnDt, null, TableName, ConnStr);
        }

        //public decimal GetPrice(string strProductCode, string connStr)
        //{
        //    decimal decPrice = 0;
        //    DataTable dtGet = new DataTable();
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0] = new DbParameter("@ProductCode", strProductCode.ToString());
        //    dtGet = DbRead("dbo.PC_RECDt_GetPrice_ProductCode", dbParams, connStr);
        //    if (dtGet.Rows.Count > 0)
        //    {
        //        decPrice = decimal.Parse(dtGet.Rows[0]["Price"].ToString());
        //    }
        //    return decPrice;
        //}
        /// <summary>
        /// </summary>
        /// <param name="strProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        /// <summary>
        /// </summary>
        /// <param name="CnNo"></param>
        /// <param name="dsCnDtDt"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetCnDtMaxIDByCnNo(string CnNo, DataSet dsCnDtDt, string connStr)
        {
            var result = false;
            var dtRecDetail = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@CnNo", CnNo);

            // Get data
            result = DbRetrieve("[PC].[GetCnDtMaxNo]", dsCnDtDt, dbParams, TableName, connStr);

            // Return result
            return result;
        }


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

        /// <summary>
        /// </summary>
        /// <param name="dsRec"></param>
        /// <returns></returns>
        private DataColumn[] GetPK(DataSet dsCnDtDt)
        {
            var primaryKeys = new DataColumn[2];
            primaryKeys[0] = dsCnDtDt.Tables[TableName].Columns["CnNo"];
            primaryKeys[1] = dsCnDtDt.Tables[TableName].Columns["CnDtNo"];

            return primaryKeys;
        }
    }
}