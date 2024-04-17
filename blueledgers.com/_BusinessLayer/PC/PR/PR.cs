using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.PC.PR
{
    public class PR : DbHandler
    {
        public PR()
        {
            SelectCommand = "SELECT * FROM [PC].[Pr]";
            TableName = "Pr";
        }

        /// <summary>
        ///     Get data by PrNo.
        /// </summary>
        /// <param name="dsPR"></param>
        /// <param name="MsgError"></param>
        /// <param name="prNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListByPrNo(DataSet dsPR, ref string MsgError, string prNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PrNo", prNo);

            var result = DbRetrieve("dbo.PC_PR_Get_PrNo", dsPR, dbParams, TableName, connStr);

            if (!result)
            {
                dsPR.Tables[TableName].PrimaryKey = GetPK(dsPR);
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPo"></param>
        /// <returns></returns>
        private DataColumn[] GetPK(DataSet dsPo)
        {
            var primaryKeys = new DataColumn[2];
            primaryKeys[0] = dsPo.Tables[TableName].Columns["PrNo"];
            primaryKeys[1] = dsPo.Tables[TableName].Columns["PrDt"];

            return primaryKeys;
        }

        /// <summary>
        ///     Get structure
        /// </summary>
        /// <param name="dsPR"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsPR, string ConnStr)
        {
            return DbRetrieveSchema("PC.Pr_GetList", dsPR, null, TableName, ConnStr);
        }

        /// <summary>
        ///     Get new pr number.
        /// </summary>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public string GetNewID(DateTime DocDate, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AtDate", DocDate.ToString("yyyy-MM-dd"));

            var dtPR = DbRead("PC.PRGetNewID", dbParams, ConnStr);


            if (dtPR != null)
            {
                if (dtPR.Rows.Count > 0)
                {
                    return dtPR.Rows[0][0].ToString();
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Get new pr number.
        /// </summary>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public string GetNewID(DateTime DocDate, string prefix, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@AtDate", DocDate.ToString("yyyy-MM-dd"));
            dbParams[1] = new DbParameter("@Prefix", prefix);

            var dtPR = DbRead("PC.PRGetNewID", dbParams, ConnStr);


            if (dtPR != null)
            {
                if (dtPR.Rows.Count > 0)
                {
                    return dtPR.Rows[0][0].ToString();
                }
            }

            return string.Empty;
        }


        /// <summary>
        ///     Save data to database.
        /// </summary>
        /// <param name="dsSaving"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Save(DataSet dsSaving, string strConn)
        {
            var prDt = new PRDt();

            // Build savesource object
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsSaving, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsSaving, prDt.SelectCommand, prDt.TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, strConn);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsSaving"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool SaveHdr(DataSet dsSaving, string strConn)
        {
            var prDt = new PRDt();

            // Build savesource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSaving, SelectCommand, TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, strConn);
        }

        /// <summary>
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public int CountByLocationCode(string Location, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LocateCode", Location);

            return DbReadScalar("dbo.PC_PR_CountByLocationCode", dbParams, ConnStr);
        }

        /// <summary>
        ///     Gets voided ability.
        /// </summary>
        /// <param name="PrNo"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool IsInprocess(string @PrNo, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PrNo", PrNo);

            var dbResult = DbRead("[PC].[GetPr_CanVoid]", dbParams, ConnStr);

            if (dbResult != null)
            {
                if (dbResult.Rows.Count > 0)
                {
                    return bool.Parse(dbResult.Rows[0][0].ToString());
                }
            }

            return false;
        }

        public bool CanEdit(string @PrNo, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PrNo", PrNo);

            var dbResult = DbRead("[PC].[GetPr_CanEdit]", dbParams, ConnStr);

            if (dbResult != null)
            {
                if (dbResult.Rows.Count > 0)
                {
                    return bool.Parse(dbResult.Rows[0][0].ToString());
                }
            }

            return false;
        }

        public DataTable GetList(string LoginName, string Connstr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LoginName", LoginName);

            return DbRead("PC.GetPrList_LoginName", dbParams, Connstr);
        }
    }
}