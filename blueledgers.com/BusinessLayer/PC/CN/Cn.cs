using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.PC.CN
{
    public class Cn : DbHandler
    {
        public Cn()
        {
            SelectCommand = "SELECT * FROM [PC].[Cn]";
            TableName = "Cn";
        }

        /// <summary>
        ///     Get data by Cn no.
        /// </summary>
        /// <param name="dsCn"></param>
        /// <param name="MsgError"></param>
        /// <param name="CnNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListByCnNo(DataSet dsCn, ref string MsgError, string CnNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CnNo", CnNo);

            var result = DbRetrieve("[PC].[GetListByCnNo]", dsCn, dbParams, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            dsCn.Tables[TableName].PrimaryKey = GetPK(dsCn);
            return true;
        }

        /// <summary>
        ///     Get structure
        /// </summary>
        /// <param name="dsCn"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsCn, string ConnStr)
        {
            return DbRetrieveSchema("[PC].[GetCnSchema]", dsCn, null, TableName, ConnStr);
        }

        /// <summary>
        ///     Save data to database.
        /// </summary>
        /// <param name="dsSaving"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Save(DataSet dsSaving, string strConn)
        {
            var CnDt = new BL.PC.CN.CnDt();

            // Build savesource object
            var dbSaveSource = new DbSaveSource[2];

            dbSaveSource[0] = new DbSaveSource(dsSaving, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsSaving, CnDt.SelectCommand, CnDt.TableName);

            // Call function dbCommit for commit data to database
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
        ///     Save data to credit note, credit note ditail and inventory.
        /// </summary>
        /// <param name="dsSaving"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool SaveToCnCnDtAndInv(DataSet dsSaving, string strConn)
        {
            var CnDt = new BL.PC.CN.CnDt();
            var Inv = new Blue.BL.IN.Inventory();


            // Build savesource object
            var dbSaveSource = new DbSaveSource[3];

            dbSaveSource[0] = new DbSaveSource(dsSaving, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsSaving, CnDt.SelectCommand, CnDt.TableName);
            dbSaveSource[2] = new DbSaveSource(dsSaving, Inv.SelectCommand, Inv.TableName);

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
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public string GetNewID(DateTime DocDate, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AtDate", DocDate.ToString("yyyy-MM-dd"));

            var dtCn = DbRead("[PC].[CNGetNewID]", dbParams, ConnStr);

            if (dtCn != null)
            {
                if (dtCn.Rows.Count > 0)
                {
                    return dtCn.Rows[0][0].ToString();
                }
            }

            return string.Empty;
        }

        public string GetNewID(DateTime DocDate, string prefix, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@AtDate", DocDate.ToString("yyyy-MM-dd"));
            dbParams[1] = new DbParameter("@Prefix", prefix);

            var dtCn = DbRead("[PC].[CNGetNewID]", dbParams, ConnStr);

            if (dtCn != null)
            {
                if (dtCn.Rows.Count > 0)
                {
                    return dtCn.Rows[0][0].ToString();
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsCn"></param>
        /// <returns></returns>
        private DataColumn[] GetPK(DataSet dsCn)
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsCn.Tables[TableName].Columns["CnNo"];

            return primaryKeys;
        }

        public bool GetCnbyDate(DateTime fDate, DateTime tDate, DataSet dsCn, int exStatus, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[3];

            // Create parameter
            dbParams[0] = new DbParameter("@FDate", fDate.Date.ToString("yyyy-MM-dd"));
            dbParams[1] = new DbParameter("@TDate", tDate.Date.ToString("yyyy-MM-dd"));
            dbParams[2] = new DbParameter("@ExStatus", exStatus.ToString());

            // Get data
            result = DbRetrieve("[PC].[GetCnByCnDate]", dsCn, dbParams, TableName, connStr);

            // Return result
            return result;
        }
    }
}