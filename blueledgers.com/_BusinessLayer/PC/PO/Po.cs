using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.PC.PO
{
    public class PO : DbHandler
    {
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly BL.PC.PO.PoDt podt = new BL.PC.PO.PoDt();
        private readonly BL.PC.PR.PRDt prDt = new BL.PC.PR.PRDt();

        public PO()
        {
            SelectCommand = "SELECT * FROM [PC].[Po]";
            TableName = "Po";
        }

        /// <summary>
        ///     Get data by PoNo.
        /// </summary>
        /// <param name="dsPo"></param>
        /// <param name="MsgError"></param>
        /// <param name="PoNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListByPoNo(DataSet dsPo, ref string MsgError, string PoNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PoNo", PoNo);

            var result = DbRetrieve("dbo.PC_Po_Get_PoNo", dsPo, dbParams, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        public bool GetListByPoNo2(DataSet dsPo, ref string MsgError, string PoNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PoNo", PoNo);

            var result = DbRetrieve("PC.GetPoListbyPoNo", dsPo, dbParams, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        public bool GetList_PoNo(DataSet dsPo, ref string MsgError, string PoNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PoNo", PoNo);

            var result = DbRetrieve("PC.GetPoList_PoNo", dsPo, dbParams, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Get structure
        /// </summary>
        /// <param name="dsPo"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsPo, string ConnStr)
        {
            return DbRetrieve("dbo.PC_Po_GetSchema", dsPo, null, TableName, ConnStr);
        }

        public bool GetStructure2(DataSet dsPo, string ConnStr)
        {
            return DbRetrieveSchema("[PC].[GetPOSchema]", dsPo, null, TableName, ConnStr);
        }

        //public bool GetStructure_Bu(DataSet dsDataList, string BuCode)
        //{
        //    DataSet dsBu    = new DataSet();
        //    string MsgError = string.Empty;

        //    bool result = bu.Get(dsBu, BuCode);

        //    if (result)
        //    {
        //        // Get all data in group
        //        foreach (DataRow drBu in dsBu.Tables[bu.TableName].Rows)
        //        {
        //            // Create Connection String by Business Unit
        //            string connStr = "Data Source=" + drBu["ServerName"].ToString() + "; " +
        //                             "Initial Catalog = " + drBu["DatabaseName"].ToString() + "; " +
        //                             "User ID = " + drBu["UserName"].ToString() + "; " +
        //                             "Password = " + GnxLib.EnDecryptString(drBu["Password"].ToString(), GnxLib.EnDeCryptor.DeCrypt);

        //            this.GetStructure(dsDataList, connStr);
        //        }

        //        return true;
        //    }

        //    return false;
        //}

        public bool GetStructure_Bu2(DataSet dsDataList, string BuCode)
        {
            var dsBu = new DataSet();
            var MsgError = string.Empty;

            var result = bu.Get(dsBu, BuCode);

            if (result)
            {
                // Get all data in group
                foreach (DataRow drBu in dsBu.Tables[bu.TableName].Rows)
                {
                    // Create Connection String by Business Unit
                    var connStr = new Common.ConnectionStringConstant().Get(drBu);

                    //string connStr = "Data Source=" + drBu["ServerName"].ToString() + "; " +
                    //                 "Initial Catalog = " + drBu["DatabaseName"].ToString() + "; " +
                    //                 "User ID = " + drBu["UserName"].ToString() + "; " +
                    //                 "Password = " + GnxLib.EnDecryptString(drBu["Password"].ToString(), GnxLib.EnDeCryptor.DeCrypt);

                    GetStructure2(dsDataList, connStr);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        ///     Get Max PODtNo
        /// </summary>
        /// <param name="unitCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetMaxPoDt(string poNo, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PoNO", poNo);

            var drPoDt = DbRead("dbo.PC_PoDtNewID", dbParams, connStr);

            var maxPoDt = Convert.ToInt32(drPoDt.Rows[0]["MaxPoDt"]);

            return maxPoDt;
        }

        public int GetMaxPoDt2(string poNo, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PoNO", poNo);

            //DataTable drPoDt        = DbRead("dbo.PC_PoDtNewID", dbParams, connStr);
            var drPoDt = DbRead("PC.GetPoDtNewID", dbParams, connStr);

            var maxPoDt = Convert.ToInt32(drPoDt.Rows[0]["MaxPoDt"]);

            return maxPoDt;
        }

        /// <summary>
        ///     Get new po number.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetNewID(DateTime DocDate, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AtDate", DocDate.ToString("yyyy-MM-dd"));

            var dtPO = DbRead("[PC].[POGetNewID]", dbParams, ConnStr);

            if (dtPO != null)
            {
                if (dtPO.Rows.Count > 0)
                {
                    return dtPO.Rows[0][0].ToString();
                }
            }

            return string.Empty;
        }

        public string GetNewID(DateTime DocDate, string prefix, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@AtDate", DocDate.ToString("yyyy-MM-dd"));
            dbParams[1] = new DbParameter("@Prefix", prefix);

            var dtPO = DbRead("[PC].[POGetNewID]", dbParams, ConnStr);

            if (dtPO != null)
            {
                if (dtPO.Rows.Count > 0)
                {
                    return dtPO.Rows[0][0].ToString();
                }
            }

            return string.Empty;
        }


        public bool GetListAndVendor(DataSet dsPo, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("dbo.PC_PO_GetList_and_Vendor", dsPo, null, TableName, connStr);

            // Return result
            return result;
        }

        public bool GetListAndVendor2(DataSet dsPo, string connStr)
        {
            var result = false;

            // Get data            
            result = DbRetrieve("PC.GetPOList_Vendor", dsPo, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="statusPrint"></param>
        /// <param name="statusPartial"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetVendorByPOStatus(string statusPrint, string statusPartial, string BuCode, string connStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@StatusPrint", statusPrint);
            dbParams[1] = new DbParameter("@StatusPartial", statusPartial);
            dbParams[2] = new DbParameter("@BuCode", BuCode);

            return DbRead("PC.GetVendorListByPOStatus", dbParams, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsPo, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("dbo.PC_PO_GetList", dsPo, null, TableName, connStr);

            // Return result
            return result;
        }

        public bool GetList2(DataSet dsPo, string connStr)
        {
            var result = false;

            // Get data            
            result = DbRetrieve("PC.GetPoList", dsPo, null, TableName, connStr);

            // Return result
            return result;
        }

        public bool GetListByClosePO(DataSet dsPo, string vendorCode, DateTime deliDate, string connStr)
        {
            var result = false;
            var dtVendor = new DataTable();
            var dbParams = new DbParameter[2];

            // Create parameter
            dbParams[0] = new DbParameter("@Vendor", vendorCode);
            dbParams[1] = new DbParameter("@DeliDate", deliDate.ToString("yyyy-MM-dd"));

            // Get data
            result = DbRetrieve("PC.GetPoListByClosePO", dsPo, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Update Receive quantity in PO
        /// </summary>
        /// <param name="PoNo"></param>
        /// <param name="PoDt"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool UpdateRcvQty(string PoNo, string PoDt, decimal RcvQty, string connStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@PoNo", PoNo);
            dbParams[1] = new DbParameter("@PoDt", PoDt);
            dbParams[2] = new DbParameter("@RcvQty", RcvQty.ToString());

            var dtGet = DbExecuteQuery("EXEC [PC].[UpdateRcvQty] @PoNo,@PoDt,@RcvQty", dbParams, connStr);

            if (dtGet.Rows.Count > 0)
            {
                return Boolean.Parse(dtGet.Rows[0]["Result"].ToString());
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsSaving"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool SaveOnlyPO(DataSet dsSaving, string strConn)
        {
            // Build savesource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSaving, SelectCommand, TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, strConn);
        }

        /// <summary>
        ///     Save data to database.
        /// </summary>
        /// <param name="dsSaving"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Save(DataSet dsSaving, string strConn)
        {
            // Build savesource object
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsSaving, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsSaving, podt.SelectCommand, podt.TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, strConn);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsSaving"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool SavePRPO(DataSet dsSaving, string strConn)
        {
            // Build savesource object
            var dbSaveSource = new DbSaveSource[3];
            dbSaveSource[0] = new DbSaveSource(dsSaving, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsSaving, podt.SelectCommand, podt.TableName);
            dbSaveSource[2] = new DbSaveSource(dsSaving, prDt.SelectCommand, prDt.TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, strConn);
        }

        public DataTable GetOpenList(string ConnStr)
        {
            return DbRead("PC.GetPoListPrintedPartial", null, ConnStr);
        }

        public void InsertSignature(string PoNo, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PONo", PoNo);

            DbExecuteNonQuery("[PC].[POInsertSignature]", dbParams, ConnStr);
        }
    }
}