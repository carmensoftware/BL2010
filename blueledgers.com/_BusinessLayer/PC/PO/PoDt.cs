using System.Data;
using Blue.DAL;

namespace Blue.BL.PC.PO
{
    public class PoDt : DbHandler
    {
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();

        public PoDt()
        {
            SelectCommand = "SELECT * FROM [PC].[PoDt]";
            TableName = "PoDt";
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

            var result = DbRetrieve("[PC].[GetPoDtbyPoNo]", dsPo, dbParams, TableName, connStr);

            if (result)
            {
                // Create primarykey                
                dsPo.Tables[TableName].PrimaryKey = GetPK(dsPo);
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPo"></param>
        /// <param name="MsgError"></param>
        /// <param name="PoNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPoDtByPoNo(DataSet dsPo, ref string MsgError, string PoNo, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PoNo", PoNo);

            result = DbRetrieve("[PC].[GetPoDtByPoNo]", dsPo, dbParams, TableName, connStr);

            return result;
        }

        public bool GetPoDt_PoNo(DataSet dsPo, ref string MsgError, string PoNo, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PoNo", PoNo);

            result = DbRetrieve("[PC].[GetPoDt_PoNo]", dsPo, dbParams, TableName, connStr);

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPo"></param>
        /// <param name="MsgError"></param>
        /// <param name="PoNo"></param>
        /// <param name="BuCode"></param>
        /// <param name="LocationCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPoDtByPoNoForReceiving(DataSet dsPo, ref string MsgError, string PoNo, string BuCode,
            string LocationCode, string connStr)
        {
            var result = false;


            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@PoNo", PoNo);
            dbParams[1] = new DbParameter("@BuCode", BuCode);
            dbParams[2] = new DbParameter("@Location", LocationCode);

            result = DbRetrieve("[PC].[GetPoDtByPoNoForReceiving]", dsPo, dbParams, TableName, connStr);

            return result;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="dsPo"></param>
        ///// <param name="MsgError"></param>
        ///// <param name="PoNo"></param>
        ///// <param name="BuCode"></param>
        ///// <param name="connStr"></param>
        ///// <returns></returns>
        //public bool GetPoDtByPoNoForAddPo(DataSet dsPo, ref string MsgError, string PoNo, string BuCode, string connStr)
        //{
        //    DbParameter[] dbParams = new DbParameter[2];
        //    dbParams[0] = new DbParameter("@PoNo", PoNo);
        //    dbParams[1] = new DbParameter("@BuCode", BuCode);

        //    return DbRetrieve("[PC].[GetPoDtByPoNoForAddPo]", dsPo, dbParams, this.TableName, connStr);;
        //}
        public bool GetListForAddPo(DataSet dsPoDt, string statusPrint, string statusPartial, string vendor,
            string BuCode, string LocationCode, string connStr)
        {
            var dbParams = new DbParameter[5];
            dbParams[0] = new DbParameter("@StatusPrint", statusPrint);
            dbParams[1] = new DbParameter("@StatusPartial", statusPartial);
            dbParams[2] = new DbParameter("@BuCode", BuCode);
            dbParams[3] = new DbParameter("@Vendor", vendor);
            dbParams[4] = new DbParameter("@Location", LocationCode);

            return DbRetrieve("[PC].[GetPoListForAddPO]", dsPoDt, dbParams, TableName, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPo"></param>
        /// <param name="MsgError"></param>
        /// <param name="PoNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetPoDtCancelByPoNo(DataSet dsPo, ref string MsgError, string PoNo, string BuCode, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@PoNo", PoNo);
            dbParams[1] = new DbParameter("@BuCode", BuCode);
            result = DbRetrieve("[PC].[GetPoDtCancelByPoNo]", dsPo, dbParams, TableName, connStr);

            return result;
        }

        /// <summary>
        ///     Create primary keys for season DataTable from specified DataSet.
        /// </summary>
        /// <param name="dsSeason"></param>
        /// <returns></returns>
        private DataColumn[] GetPK(DataSet dsPo)
        {
            var primaryKeys = new DataColumn[2];
            primaryKeys[0] = dsPo.Tables[TableName].Columns["PoNo"];
            primaryKeys[1] = dsPo.Tables[TableName].Columns["PoDt"];

            return primaryKeys;
        }

        /// <summary>
        ///     Get data and retieve dataset.
        /// </summary>
        /// <param name="dsPoDt"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetList(DataSet dsPoDt, string PoNo, string connStr)
        {
            var MsgError = string.Empty;

            // Create parameters
            var result = GetListByPoNo(dsPoDt, ref MsgError, PoNo, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return null;
            }
            return dsPoDt;
        }

        //public DataSet GetListByVendor(DataSet dsPoDt, string Vendor, string connStr)
        //{
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0] = new DbParameter("@Vendor", Vendor);
        //    bool result = DbRetrieve("dbo.PC_PO_Get_Vendor", dsPoDt, dbParams, this.TableName, connStr);
        //    if (result)
        //    {
        //        dsPoDt.Tables[this.TableName].PrimaryKey = GetPK(dsPoDt);
        //        return dsPoDt;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        /// <summary>
        /// </summary>
        /// <returns></returns>
        /// <summary>
        ///     Get PoDtList for Receiving side.
        /// </summary>
        /// <param name="dsPoDt"></param>
        /// <param name="statusPrint"></param>
        /// <param name="statusPartial"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListByStatus(DataSet dsPoDt, string statusPrint, string statusPartial, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@StatusPrint", statusPrint);
            dbParams[1] = new DbParameter("@StatusPartial", statusPartial);

            result = DbRetrieve("[PC].[GetPOListByStatus]", dsPoDt, dbParams, TableName, connStr);

            return result;
        }

        public bool GetListByBuCode(DataSet dsPoDt, string BuCode, string statusPrint, string statusPartial,
            string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@BuCode", BuCode);
            dbParams[1] = new DbParameter("@StatusPrint", statusPrint);
            dbParams[2] = new DbParameter("@StatusPartial", statusPartial);

            result = DbRetrieve("[PC].[GetPOListByBuCode]", dsPoDt, dbParams, TableName, connStr);

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="PoNo"></param>
        /// <param name="BuCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetLocationByPoNo(string PoNo, string BuCode, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@PoNo", PoNo);
            dbParams[1] = new DbParameter("@BuCode", BuCode);

            return DbRead("[PC].[GetLocationByPoNo]", dbParams, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPoDt"></param>
        /// <param name="statusPrint"></param>
        /// <param name="statusPartial"></param>
        /// <param name="vendor"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListByStatusAndVendor(DataSet dsPoDt, string statusPrint, string statusPartial, string vendor, string BuCode, string connStr)
        {
            var dbParams = new DbParameter[4];
            dbParams[0] = new DbParameter("@StatusPrint", statusPrint);
            dbParams[1] = new DbParameter("@StatusPartial", statusPartial);
            dbParams[2] = new DbParameter("@BuCode", BuCode);
            dbParams[3] = new DbParameter("@Vendor", vendor);

            return DbRetrieve("[PC].[GetPOListByStatusAndVendor]", dsPoDt, dbParams, TableName, connStr);
        }

        public bool GetListByStatusAndVendor(DataSet dsPoDt, string statusPrint, string statusPartial, string vendor, string BuCode, string currencyCode, string connStr)
        {
            var dbParams = new DbParameter[5];
            dbParams[0] = new DbParameter("@StatusPrint", statusPrint);
            dbParams[1] = new DbParameter("@StatusPartial", statusPartial);
            dbParams[2] = new DbParameter("@BuCode", BuCode);
            dbParams[3] = new DbParameter("@Vendor", vendor);
            dbParams[4] = new DbParameter("@CurrencyCode", currencyCode);

            return DbRetrieve("[PC].[GetPOListByStatusAndVendor]", dsPoDt, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get PO list for function add po by location code
        /// </summary>
        /// <param name="dsPoDt"></param>
        /// <param name="vendor"></param>
        /// <param name="BuCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetAddPoList(DataSet dsPoDt, string vendor, string BuCode, string Location, string connStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@BuCode", BuCode);
            dbParams[1] = new DbParameter("@Vendor", vendor);
            dbParams[2] = new DbParameter("@Location", Location);

            return DbRetrieve("[PC].[GetAddPoList]", dsPoDt, dbParams, TableName, connStr);
        }

        public bool GetAddPoList(DataSet dsPoDt, string vendor, string BuCode, string Location, string currencyCode, string connStr)
        {
            var dbParams = new DbParameter[4];
            dbParams[0] = new DbParameter("@BuCode", BuCode);
            dbParams[1] = new DbParameter("@Vendor", vendor);
            dbParams[2] = new DbParameter("@Location", Location);
            dbParams[3] = new DbParameter("@CurrencyCode", currencyCode);

            return DbRetrieve("[PC].[GetAddPoList]", dsPoDt, dbParams, TableName, connStr);
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPODt"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsPODt, string ConnStr)
        {
            return DbRetrieve("dbo.PC_PODt_GetSchema", dsPODt, null, TableName, ConnStr);
        }

        public bool GetSchema2(DataSet dsPODt, string ConnStr)
        {
            return DbRetrieveSchema("[PC].[GetPODtSchema]", dsPODt, null, TableName, ConnStr);
        }

        public bool GetSchema_Bu(DataSet dsDataList, string BuCode)
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

                    GetSchema(dsDataList, connStr);
                }

                return true;
            }

            return false;
        }

        public decimal GetSumTotalAmt(string poNo, string connStr)
        {
            var dsTmp = new DataSet();
            var MsgError = string.Empty;
            decimal decTotalAmt = 0;

            var result = GetPoDtByPoNo(dsTmp, ref MsgError, poNo, connStr);

            if (result)
            {
                if (dsTmp.Tables[0].Rows.Count > 0)
                {
                    for (var i = 0; i < dsTmp.Tables[0].Rows.Count; i++)
                    {
                        decTotalAmt += decimal.Parse(dsTmp.Tables[0].Rows[i]["TotalAmt"].ToString());
                    }
                }
            }

            return decTotalAmt;
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
    }
}