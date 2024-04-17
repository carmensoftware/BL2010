using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.PC.PR
{
    public class PRDt : DbHandler
    {
        #region "Attributes"

        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();

        #endregion

        #region "Operations"

        public enum LastPriceVendor
        {
            LastPrice = 0,
            LastVendor = 1
        }

        public PRDt()
        {
            SelectCommand = "SELECT * FROM [PC].[PrDt]";
            TableName = "PrDt";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPrDt"></param>
        /// <param name="PrNo"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsPrDt, string PrNo, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PrNo", PrNo);

            return DbRetrieve("PC.PrDt_GetList_PrNo", dsPrDt, dbParams, TableName, ConnStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="PrNo"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public DataTable GetList(string PrNo, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PrNo", PrNo);

            return DbRead("[IN].PrDtGetListByPrNo", dbParams, ConnStr);
        }

        /// <summary>
        ///     Get data by PoNo for expand in grid po page
        /// </summary>
        /// <param name="PoNo"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public DataTable GetListByPONo(string PoNo, int PoDtNo, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@PONo", PoNo);
            dbParams[1] = new DbParameter("@PODtNo", PoDtNo.ToString());

            return DbRead("PC.GetPrDtListByPONo_PODtNo", dbParams, ConnStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="PoNo"></param>
        /// <param name="PoDtNo"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public DataTable GetByPONoPODt(string PoNo, int PoDtNo, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@PONo", PoNo);
            dbParams[1] = new DbParameter("@PODtNo", PoDtNo.ToString());

            return DbRead("PC.GetPrDtByPONo_PODtNo", dbParams, ConnStr);
        }

        /// <summary>
        ///     Get Pr Detail by PoNo.
        /// </summary>
        /// <param name="PoNo"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetByPONo(DataSet dsPr, string PoNo, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PONo", PoNo);

            return DbRetrieve("PC.GetPrDtByPONo", dsPr, dbParams, TableName, ConnStr);

            //return DbRead("PC.GetPrDtByPONo", dbParams, ConnStr);
            //bool result = DbRetrieve("PC.GetPrDtByPONo", dsTmp, dbParams, this.TableName, ConnStr);

            //if(result)
            //{
            //    return dsTmp;
            //}
            //else
            //{
            //    return null;
            //}            
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

            var result = DbRetrieve("PC.GetPRDt_PrNo", dsPR, dbParams, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }
            dsPR.Tables[TableName].PrimaryKey = GetPK(dsPR);

            return true;
        }

        /// <summary>
        ///     Get select data use po create pr
        /// </summary>
        /// <param name="dsPR"></param>
        /// <param name="MsgError"></param>
        /// <param name="prNo"></param>
        /// <param name="PRDtNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListByPrNo_PrDtNo(DataSet dsPR, ref string MsgError, string prNo, string PRDtNo, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@PrNo", prNo);
            dbParams[1] = new DbParameter("@PRDtNo", PRDtNo);

            var result = DbRetrieve("dbo.PC_PRDt_Get_PrNo_PrDtNo", dsPR, dbParams, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }
            dsPR.Tables[TableName].PrimaryKey = GetPK(dsPR);

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPR"></param>
        /// <param name="MsgError"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList_PrAppr(DataSet dsPR, ref string MsgError, string connStr)
        {
            var result = DbRetrieve("dbo.PC_PRDt_GetList_PrAppr", dsPR, null, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPR"></param>
        /// <param name="MsgError"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListAll(DataSet dsPR, ref string MsgError, string connStr)
        {
            var result = DbRetrieve("dbo.PC_PRDt_GetList", dsPR, null, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPR"></param>
        /// <param name="MsgError"></param>
        /// <param name="vendor"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList_Approve_ByVendor(DataSet dsPR, ref string MsgError, string vendor, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@vendorcode", vendor);

            var result = DbRetrieve("dbo.PC_PrDt_GetListBy_Vendor", dsPR, dbParams, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }
            dsPR.Tables[TableName].PrimaryKey = GetPK(dsPR);

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="BuGrpCode"></param>
        /// <param name="dsDataList"></param>
        /// <param name="ViewNo"></param>
        /// <param name="PageCode"></param>
        /// <param name="KeyFieldName"></param>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public bool GetDataListHQ(string BuGrpCode, DataSet dsDataList)
        {
            // Get All actived Bu in specified BuGrpCode
            var dsBu = new DataSet();
            var MsgError = string.Empty;

            var result = bu.GetList(dsBu, BuGrpCode);

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

                    if (bool.Parse(drBu["IsHQ"].ToString()))
                    {
                        GetList_Approve_GroupByPrNo(dsDataList, ref MsgError, connStr);
                    }
                    else
                    {
                        // For Test Save Create PO.
                        //this.GetList_Approve_GroupByPrNo(dsDataList, ref MsgError, connStr);


                        GetList_Approve_GroupByPrNoAndReqHQAppr(dsDataList, ref MsgError, connStr);
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPR"></param>
        /// <param name="MsgError"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList_Approve(DataSet dsPR, ref string MsgError, string connStr)
        {
            var result = DbRetrieve("dbo.PC_PrDt_GetList_forPObyPR", dsPR, null, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }
            dsPR.Tables[TableName].PrimaryKey = GetPK(dsPR);

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPR"></param>
        /// <param name="MsgError"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList_Approve_GroupByPrNo(DataSet dsPR, ref string MsgError, string connStr)
        {
            var result = DbRetrieve("[PC].[GetPRForCreatePO]", dsPR, null, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }
            //else
            //{
            //    dsPR.Tables[this.TableName].PrimaryKey = Get1PK(dsPR);
            //}

            return true;
        }


        public bool GetList_Approve_GroupByPrNoAndReqHQAppr(DataSet dsPR, ref string MsgError, string connStr)
        {
            var result = DbRetrieve("[PC].[GetPRbyReqHQApprForCreatePO]", dsPR, null, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }
            //else
            //{
            //    dsPR.Tables[this.TableName].PrimaryKey = Get1PK(dsPR);
            //}

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPR"></param>
        /// <param name="MsgError"></param>
        /// <param name="PrNoPrDtNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetHd_ByPrNoPrDtNo(DataSet dsPR, ref string MsgError, string PrNoPrDtNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PrNoPrDtNo", PrNoPrDtNo);

            var result = DbRetrieve("dbo.PC_PrDt_GetHd_PrNoPrDtNo", dsPR, dbParams, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPR"></param>
        /// <param name="MsgError"></param>
        /// <param name="prNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetHd_ByPrNo(DataSet dsPR, ref string MsgError, string prNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PrNo", prNo);

            //bool result = DbRetrieve("dbo.PC_PrDt_GetHd_PrNo", dsPR, dbParams, this.TableName, connStr);

            var result = DbRetrieve("[PC].[GetPrDtForHdByPrNo]", dsPR, dbParams, TableName, connStr);

            //bool result = DbRetrieve("[PC].[GetPrDtForHdByPrNo2]", dsPR, dbParams, this.TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="PrNoPrDtNo"></param>
        /// <param name="vendorCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetDt_ByPrNoPrDtNo(string PrNoPrDtNo, string vendorCode, string connStr)
        {
            var dsTmp = new DataSet();
            var dtTmp = new DataTable();
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@PrNoPrDtNo", PrNoPrDtNo);
            dbParams[1] = new DbParameter("@VendorCode", vendorCode);

            DbRetrieve("dbo.PC_PrDt_Get_PrNoPrDtNo_VendorCode", dsTmp, dbParams, TableName, connStr);

            if (dsTmp != null)
            {
                dtTmp = dsTmp.Tables[TableName];
            }

            return dtTmp;
        }

        /// <summary>
        /// </summary>
        /// <param name="PrNo"></param>
        /// <param name="vendorCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetDt_ByPrNo(string PrNo, string vendorCode, string reqDate, string connStr)
        {
            var dsTmp = new DataSet();
            var dtTmp = new DataTable();
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@PrNo", PrNo);
            dbParams[1] = new DbParameter("@VendorCode", vendorCode);
            dbParams[2] = new DbParameter("@ReqDate", reqDate);

            //DbRetrieve("dbo.PC_PrDt_Get_PrNo_VendorCode", dsTmp, dbParams, this.TableName, connStr);
            DbRetrieve("[PC].[GetPRDtbyPrNo_Vendor_ReqDate]", dsTmp, dbParams, TableName, connStr);

            if (dsTmp != null)
            {
                dtTmp = dsTmp.Tables[TableName];
            }

            return dtTmp;
        }

        /// <summary>
        /// </summary>
        /// <param name="PrNo"></param>
        /// <param name="drPR"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetDt_ByPrNo(string PrNo, DataRow drPR, string connStr)
        {
            var dsTmp = new DataSet();
            var dtTmp = new DataTable();
            var dbParams = new DbParameter[12];

            dbParams[0] = new DbParameter("@PRNo", PrNo);
            dbParams[1] = new DbParameter("@VendorCode", drPR["VendorCode"].ToString());
            dbParams[2] = new DbParameter("@ReqDate", drPR["ReqDate"].ToString());
            dbParams[3] = new DbParameter("@OrderUnit", drPR["OrderUnit"].ToString());
            dbParams[4] = new DbParameter("@Buyer", drPR["Buyer"].ToString());
            dbParams[5] = new DbParameter("@DeliPoint", drPR["DeliPoint"].ToString());
            dbParams[6] = new DbParameter("@Taxtype", drPR["Taxtype"].ToString());
            dbParams[7] = new DbParameter("@LocationCode", drPR["LocationCode"].ToString());
            dbParams[8] = new DbParameter("@DiscPercent", drPR["DiscPercent"].ToString());
            dbParams[9] = new DbParameter("@DiscAmt", drPR["DiscAmt"].ToString());
            dbParams[10] = new DbParameter("@TaxRate", drPR["TaxRate"].ToString());
            var test = (drPR["TaxAdj"].ToString() == "True" ? "1" : "0");
            dbParams[11] = new DbParameter("@TaxAdj", test);

            var result = DbRetrieve("[PC].[GetPRDtbyPrNo_Vendor_ReqDate2]", dsTmp, dbParams, TableName, connStr);

            if (dsTmp != null)
            {
                dtTmp = dsTmp.Tables[TableName];
            }

            return dtTmp;
        }

        public DataSet GetDt_ByPrNo(DataSet dsPoDt, string PrNo, string vendorCode, string reqDate, string connStr)
        {
            var dtTmp = new DataTable();
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@PrNo", PrNo);
            dbParams[1] = new DbParameter("@VendorCode", vendorCode);
            dbParams[2] = new DbParameter("@ReqDate", reqDate);

            var result = DbRetrieve("dbo.PC_PrDt_Get_PrNo_VendorCode", dsPoDt, dbParams, TableName, connStr);

            return dsPoDt;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPR"></param>
        /// <param name="MsgError"></param>
        /// <param name="PrNoPrDtNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get_ByPrNoPrDtNo(DataSet dsPR, ref string MsgError, string PrNoPrDtNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PrNoPrDtNo", PrNoPrDtNo);

            var result = DbRetrieve("dbo.PC_PrDt_GetDt_PrNoPrDtNo", dsPR, dbParams, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }
            dsPR.Tables[TableName].PrimaryKey = GetPK(dsPR);

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPR"></param>
        /// <param name="MsgError"></param>
        /// <param name="PrNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get_ByPrNo(DataSet dsPR, ref string MsgError, string PrNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PrNo", PrNo);

            //bool result = DbRetrieve("dbo.PC_PrDt_GetDt_PrNo", dsPR, dbParams, this.TableName, connStr);
            var result = DbRetrieve("[PC].[GetPrDtByPrNo]", dsPR, dbParams, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }
            dsPR.Tables[TableName].PrimaryKey = GetPK(dsPR);

            return true;
        }

        /// <summary>
        ///     get Data by prno
        /// </summary>
        /// <param name="dsTemplateDt"></param>
        /// <param name="TmpNo"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetListBy_PrNo(DataSet dsTemplateDt, string PrNo, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PrNo", PrNo);

            return DbRetrieve("[IN].PrDtGetListByPoNo", dsTemplateDt, dbParams, TableName, ConnStr);
        }

        /// <summary>
        ///     Get data and retieve dataset.
        /// </summary>
        /// <param name="dsPRDt"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        //public DataSet GetList(DataSet dsPRDt, string prNo, string connStr)
        //{
        //    string MsgError = string.Empty;
        //    // Create parameters
        //    bool result = this.GetListByPrNo(dsPRDt,ref MsgError, prNo.ToString(), connStr);
        //    if (!result)
        //    {
        //        MsgError = "Msg001";
        //        return null;
        //    }
        //    else
        //    {
        //        return dsPRDt;
        //    }            
        //}
        public DataSet GetListNotSelectedByPO(DataSet dsPRDt, string connStr)
        {
            var MsgError = string.Empty;

            // Create parameters
            var result = GetListAll(dsPRDt, ref MsgError, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return null;
            }
            return dsPRDt;
        }

        //public bool GetListByPoNo(DataSet dsPr, ref string MsgError, string PrNo, string connStr)
        //{
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0] = new DbParameter("@PrNo", PrNo);

        //    bool result = DbRetrieve("dbo.PC_PRDt_Get_PrNo", dsPr, dbParams, this.TableName, connStr);

        //    if (result)
        //    {
        //        // Create primarykey                
        //        dsPr.Tables[this.TableName].PrimaryKey = GetPK(dsPr);
        //    }

        //    return true;
        //}

        /// <summary>
        ///     Set primary key.
        /// </summary>
        /// <param name="dsPr"></param>
        /// <returns></returns>
        private DataColumn[] GetPK(DataSet dsPr)
        {
            var primaryKeys = new DataColumn[2];
            primaryKeys[0] = dsPr.Tables[TableName].Columns["PrNo"];
            primaryKeys[1] = dsPr.Tables[TableName].Columns["PrDtNo"];

            return primaryKeys;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPr"></param>
        /// <returns></returns>
        private DataColumn[] Get1PK(DataSet dsPr)
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsPr.Tables[TableName].Columns["PrNo"];

            return primaryKeys;
        }

        public bool GetStructure(DataSet dsPR, string ConnStr)
        {
            return DbRetrieveSchema("PC.PrDt_GetList", dsPR, null, TableName, ConnStr);

            //bool result = DbRetrieve("dbo.PC_PRDt_GetSchema", dsPR, null, this.TableName, ConnStr);

            //if (result)
            //{
            //    // Create primarykey                
            //    dsPR.Tables[this.TableName].PrimaryKey = GetPK(dsPR);
            //    return result;
            //}

            //return false;
        }

        public bool Save(DataSet dsSaving, string strConn)
        {
            // Build savesource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSaving, SelectCommand, TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, strConn);
        }

        public decimal GetSumNet(string prNo, string connStr)
        {
            var dsTmp = new DataSet();
            var MsgError = string.Empty;
            decimal decNetAmt = 0;

            var result = GetListByPrNo(dsTmp, ref MsgError, prNo, connStr);

            if (result)
            {
                if (dsTmp.Tables[0].Rows.Count > 0)
                {
                    for (var i = 0; i < dsTmp.Tables[0].Rows.Count; i++)
                    {
                        decNetAmt += decimal.Parse(dsTmp.Tables[0].Rows[i]["NetAmt"].ToString());
                    }
                }
            }

            return decNetAmt;
        }

        public decimal GetSumTax(string prNo, string connStr)
        {
            var dsTmp = new DataSet();
            var MsgError = string.Empty;
            decimal decTaxAmt = 0;

            var result = GetListByPrNo(dsTmp, ref MsgError, prNo, connStr);

            if (result)
            {
                if (dsTmp.Tables[0].Rows.Count > 0)
                {
                    for (var i = 0; i < dsTmp.Tables[0].Rows.Count; i++)
                    {
                        decTaxAmt += decimal.Parse(dsTmp.Tables[0].Rows[i]["TaxAmt"].ToString());
                    }
                }
            }

            return decTaxAmt;
        }

        public int CountByLocationCode(string Location, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LocateCode", Location);

            return DbReadScalar("dbo.PC_PrDt_CountbyLocationCode", dbParams, ConnStr);
        }

        public int GetCountPONo(string PONo, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PONo", PONo);

            return DbReadScalar("[PC].[GetCountPONo]", dbParams, ConnStr);
        }

        /// <summary>
        ///     Gets On Hand, Reorder, Last price, On Order, Restock and Last Vendor data.
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetStockSummary(DataSet dsPrDt, string ProductCode, string LocationCode, string Date, string ConnStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);
            dbParams[1] = new DbParameter("@ProductCode", ProductCode);
            dbParams[2] = new DbParameter("@Date", DateTime.Parse(Date).ToString("yyyy-MM-dd"));

            return DbRetrieve("PC.GetPrDtStockSummary", dsPrDt, dbParams, TableName, ConnStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public string GetTaxAccCode(string ProductCode, string ConnStr)
        {
            var strName = string.Empty;

            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@TaxCode", ProductCode);

            var dtName = DbRead("dbo.PC_PRDt_GetTaxAccCode", dbParams, ConnStr);
            if (dtName.Rows.Count > 0)
            {
                strName = dtName.Rows[0]["TaxAccCode"].ToString();
            }
            return strName;
        }

        public string GetNetAccCode(string StoreCode, string ConnStr)
        {
            var strName = string.Empty;

            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@StoreCode", StoreCode);

            var dtName = DbRead("PC.PRDt_GetNetAccCode", dbParams, ConnStr);
            if (dtName.Rows.Count > 0)
            {
                strName = dtName.Rows[0]["A3"].ToString();
            }
            return strName;
        }

        public string GetName(string PrNo, string ProductCode, string ConnStr)
        {
            var strName = string.Empty;

            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@PrNo", PrNo);
            dbParams[1] = new DbParameter("@ProductCode", ProductCode);

            var dtName = DbRead("PC.PRDt_GetName", dbParams, ConnStr);
            if (dtName.Rows.Count > 0)
            {
                strName = dtName.Rows[0]["Descen"].ToString();
            }

            return strName;
        }

        public string GetName2(string PrNo, string ProductCode, string ConnStr)
        {
            var strName = string.Empty;
            var dbParams = new DbParameter[2];

            dbParams[0] = new DbParameter("@PrNo", PrNo);
            dbParams[1] = new DbParameter("@ProductCode", ProductCode);

            var dtName = DbRead("PC.PRDt_GetName2", dbParams, ConnStr);
            if (dtName.Rows.Count > 0)
            {
                strName = dtName.Rows[0]["Descll"].ToString();
            }

            return strName;
        }

        public int CountByVendorCode(string VendorCode, string ProductCode, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@VendorCode", VendorCode);
            dbParams[1] = new DbParameter("@ProductCode", ProductCode);

            return DbReadScalar("[PC].[PRDt_CountPRDtByVendorCode]", dbParams, ConnStr);
        }

        #endregion
    }
}