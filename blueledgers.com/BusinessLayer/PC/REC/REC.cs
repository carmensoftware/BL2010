using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.PC.REC
{
    public class REC : DbHandler
    {
        public REC()
        {
            SelectCommand = "SELECT * FROM [PC].[REC]";
            TableName = "REC";
        }

        /// <summary>
        ///     Get data by receive no.
        /// </summary>
        /// <param name="dsRec"></param>
        /// <param name="MsgError"></param>
        /// <param name="recNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListByRecNo(DataSet dsRec, ref string MsgError, string recNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RecNo", recNo);

            var result = DbRetrieve("dbo.PC_REC_Get_RecNo", dsRec, dbParams, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            dsRec.Tables[TableName].PrimaryKey = GetPK(dsRec);

            return true;
        }

        /// <summary>
        ///     Get Receiving list which status not equal to 'Committed'.
        /// </summary>
        /// <param name="PoNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetListByRecStatus(string PoNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PoNo", PoNo);

            return DbRead("[PC].[GetRecListByRecStatus]", dbParams, connStr);
        }

        /// <summary>
        ///     HONG
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetListByVendor(string VendorCode, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@VendorCode", VendorCode);
            return DbRead("[PC].[GetRecListByVendor]", dbParams, connStr);
        }


        public DataTable GetListForCommitByBatch(string connStr)
        {
            return DbRead("[PC].[GetRecListForCommitByBatch]", null, connStr);
        }

        public int GetBatchNo(string connStr)
        {
            return DbReadScalar("[PC].[GetRecBatchNo]", null, connStr);
        }

        public bool GetRecbyDate(DateTime fDate, DateTime tDate, DataSet dsCnDt, int exStatus, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[3];

            // Create parameter
            dbParams[0] = new DbParameter("@FDate", fDate.Date.ToString("yyyy-MM-dd"));
            dbParams[1] = new DbParameter("@TDate", tDate.Date.ToString("yyyy-MM-dd"));
            dbParams[2] = new DbParameter("@ExStatus", exStatus.ToString());

            // Get data
            result = DbRetrieve("[PC].[GetRecByRecDate]", dsCnDt, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get structure
        /// </summary>
        /// <param name="dsREC"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsREC, string ConnStr)
        {
            return DbRetrieve("dbo.PC_REC_GetSchema", dsREC, null, TableName, ConnStr);
        }

        /// <summary>
        ///     Save data to database.
        /// </summary>
        /// <param name="dsSaving"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Save(DataSet dsSaving, string strConn)
        {
            var recDt = new BL.PC.REC.RECDt();
            var invent = new Blue.BL.IN.Inventory();

            // Build savesource object
            var dbSaveSource = new DbSaveSource[3];

            dbSaveSource[0] = new DbSaveSource(dsSaving, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsSaving, recDt.SelectCommand, recDt.TableName);
            dbSaveSource[2] = new DbSaveSource(dsSaving, invent.SelectCommand, invent.TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, strConn);
        }

        /// <summary>
        ///     Update for export to restore.
        /// </summary>
        /// <param name="dsSaving"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool SaveRestore(DataSet dsSaving, string strConn)
        {
            // Build savesource object
            var dbSaveSource = new DbSaveSource[1];

            dbSaveSource[0] = new DbSaveSource(dsSaving, SelectCommand, TableName);

            // call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, strConn);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsSaving"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool UpdateExportStatus(DataSet dsSaving, string strConn)
        {
            // Build savesource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSaving, SelectCommand, TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, strConn);
        }

        /// <summary>
        /// </summary>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public string GetNewID(DateTime DocDate, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AtDate", DocDate.ToString("yyyy-MM-dd"));

            var dtREC = DbRead("[PC].[RECGetNewID]", dbParams, ConnStr);
            if (dtREC != null)
            {
                if (dtREC.Rows.Count > 0)
                {
                    return dtREC.Rows[0][0].ToString();
                }
            }

            return string.Empty;
        }


        public string GetNewID(DateTime DocDate, string prefix, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@AtDate", DocDate.ToString("yyyy-MM-dd"));
            dbParams[1] = new DbParameter("@Prefix", prefix);

            var dtREC = DbRead("[PC].[RECGetNewID]", dbParams, ConnStr);
            if (dtREC != null)
            {
                if (dtREC.Rows.Count > 0)
                {
                    return dtREC.Rows[0][0].ToString();
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsRec"></param>
        /// <returns></returns>
        private DataColumn[] GetPK(DataSet dsRec)
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsRec.Tables[TableName].Columns["RecNo"];

            return primaryKeys;
        }
    }
}