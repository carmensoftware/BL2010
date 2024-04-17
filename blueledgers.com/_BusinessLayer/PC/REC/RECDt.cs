using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.PC.REC
{
    public class RECDt : DbHandler
    {
        public RECDt()
        {
            SelectCommand = "SELECT * FROM [PC].[RECDt]";
            TableName = "RECDt";
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

            var result = DbRetrieve("[PC].[GetRecDtByRecNo]", dsRec, dbParams, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            dsRec.Tables[TableName].PrimaryKey = GetPK(dsRec);
            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsRec"></param>
        /// <param name="MsgError"></param>
        /// <param name="recNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetRecDtByPoNoAndPoDtNo(DataSet dsRec, string poNo, int poDtNo, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@PONo", poNo);
            dbParams[1] = new DbParameter("@PoDtNo", poDtNo.ToString());

            result = DbRetrieve("[PC].[GetRecDtByPoNoAndPoDtNo]", dsRec, dbParams, TableName, connStr);

            return result;
        }


        /// <summary>
        ///     Get Data to dataset.
        /// </summary>
        /// <param name="dsRec"></param>
        /// <param name="recNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetListByRecNo(DataSet dsRec, string recNo, string connStr)
        {
            var MsgError = string.Empty;

            var result = GetListByRecNo(dsRec, ref MsgError, recNo, connStr);

            if (result)
            {
                return dsRec;
            }
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="recNo"></param>
        /// <param name="recDtNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetRecDetailByRecNoAndRecDtNo(string recNo, int recDtNo, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@RecNo", recNo);
            dbParams[1] = new DbParameter("@RecDtNo", recDtNo.ToString());

            return DbRead("[PC].[GetRecDetailByRecNoAndRecDtNo]", dbParams, connStr);
        }


        /// <summary>
        ///     Get structure
        /// </summary>
        /// <param name="dsREC"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsREC, string ConnStr)
        {
            var result = DbRetrieve("dbo.PC_RECDt_GetSchema", dsREC, null, TableName, ConnStr);
            if (result)
            {
                dsREC.Tables[TableName].PrimaryKey = GetPK(dsREC);
                return true;
            }
            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="recNo"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public int GetMaxPoDtNo(string recNo, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RecNo", recNo);

            return DbReadScalar("dbo.PC_RECDt_GetMaxNo", dbParams, ConnStr);
        }

        /// <summary>
        ///     Get price by product code. อยู่ในส่วนที่ folder blueledgers_ibis ใช้งานอยู่
        /// </summary>
        /// <param name="strProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public decimal GetPrice(string strProductCode, string connStr)
        {
            decimal decPrice = 0;
            var dtGet = new DataTable();

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", strProductCode);

            dtGet = DbRead("dbo.PC_RECDt_GetPrice_ProductCode", dbParams, connStr);

            if (dtGet.Rows.Count > 0)
            {
                decPrice = decimal.Parse(dtGet.Rows[0]["Price"].ToString());
            }

            return decPrice;
        }

        /// <summary>
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="RecNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public decimal GetLastPrice(string ProductCode, string RecNo, string connStr)
        {
            decimal LastPrice = 0;
            var dtGetLastPrice = new DataTable();

            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);
            dbParams[1] = new DbParameter("@RecNo", RecNo);

            dtGetLastPrice = DbRead("[PC].[REC_GetLastPrice]", dbParams, connStr);
            if (dtGetLastPrice.Rows.Count > 0)
            {
                LastPrice = decimal.Parse(dtGetLastPrice.Rows[0]["Price"].ToString());
            }
            return LastPrice;
        }

        /// <summary>
        /// </summary>
        /// <param name="recNo"></param>
        /// <param name="dsRecDt"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetRecDtMaxIDByRecNo(string recNo, DataSet dsRecDt, string connStr)
        {
            var result = false;
            var dtRecDetail = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@RecNo", recNo);

            // Get data
            result = DbRetrieve("[PC].[GetRecDtMaxIDByRecNo]", dsRecDt, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsRec"></param>
        /// <returns></returns>
        private DataColumn[] GetPK(DataSet dsRec)
        {
            var primaryKeys = new DataColumn[2];
            primaryKeys[0] = dsRec.Tables[TableName].Columns["RecNo"];
            primaryKeys[1] = dsRec.Tables[TableName].Columns["RecDtNo"];

            return primaryKeys;
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

        public DataTable GetPOList(string RecNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RecNo", RecNo);

            return DbRead("[PC].[GetPoNoFromRecDt]", dbParams, connStr);
        }

        public DataTable GetPRList(string PoNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PoNo", PoNo);

            return DbRead("[PC].[GetPrNoByPoNo]", dbParams, connStr);
        }

        public bool GetInterfaceRestore(DataSet dsRec, string keyWord, string keyWord2, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@FromDate", keyWord);
            dbParams[1] = new DbParameter("@ToDate", keyWord2);

            result = DbRetrieve("ADMIN.GetInterfaceExpFalse", dsRec, dbParams, TableName, connStr);

            return result;
        }

        /// <summary>
        ///     Gets On Hand from inventory to check these receiving can void or not
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public int GetInventoryOnHandAfterVoid(string HdrNo, string DtNo, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@HdrNo", HdrNo);
            dbParams[1] = new DbParameter("@DtNo", DtNo);

            return DbReadScalar("[PC].[GetInventoryOnHandAfterVoid]", dbParams);
        }

        /// <summary>
        ///     Get Qty summary of receiving by po number and podt number.
        /// </summary>
        /// <param name="PoNo"></param>
        /// <param name="PoDt"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public decimal GetSumRecQty(string PoNo, string PoDt, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@PoNo", PoNo);
            dbParams[1] = new DbParameter("@PoDt", PoDt);

            var dtGet = new DataTable();
            dtGet = DbRead("[PC].[GetSumRcvQty]", dbParams, connStr);

            if (dtGet.Rows.Count > 0 || dtGet.Rows[0][0] != null)
            {
                decimal decSumRecQty = 0;

                if (dtGet.Rows[0][0].ToString() != string.Empty)
                {
                    decSumRecQty = Convert.ToDecimal(dtGet.Rows[0][0].ToString());
                }
                else
                {
                    decSumRecQty = 0;
                }

                return decSumRecQty;
            }

            return 0;
        }
    }
}