using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class PriceList : DbHandler
    {
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();

        public PriceList()
        {
            SelectCommand = "SELECT * FROM [IN].[PriceList]";
            TableName = "PriceList";
        }

        /// <summary>
        ///     Get Data output to dataset
        /// </summary>
        /// <param name="dsRec"></param>
        /// <param name="recNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetListByVendorCodeProductCode(DataSet dsVendor, string strVendorCode, string strProductCode,
            string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@VendorCode", strVendorCode);
            dbParams[1] = new DbParameter("@ProductCode", strProductCode);

            var result = DbRetrieve("dbo.IN_PriceList_GetList_VendorCode_ProductCode", dsVendor, dbParams, TableName,
                connStr);

            if (result)
            {
                return dsVendor;
            }
            return null;
        }

        /// <summary>
        ///     Get data by date from, date to and vendor code
        /// </summary>
        /// <param name="dsVendor"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="strVendorCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetListByDateFromDateToVendor(DataSet dsVendor, string dateFrom, string dateTo,
            string strVendorCode, string connStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@DateFrom", dateFrom);
            dbParams[1] = new DbParameter("@DateTo", dateTo);
            dbParams[2] = new DbParameter("@VendorCode", strVendorCode);

            var result = DbRetrieve("dbo.IN_PriceList_Get_DateFrom_DateTo_VendorCode", dsVendor, dbParams, TableName,
                connStr);

            if (result)
            {
                dsVendor.Tables[TableName].PrimaryKey = GetPK(dsVendor);
                return dsVendor;
            }
            return null;
        }

        /// <summary>
        ///     Get data by date from, date to and product code
        /// </summary>
        /// <param name="dsVendor"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="strProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetListByDateFromDateToProduct(DataSet dsVendor, string dateFrom, string dateTo,
            string strProductCode, string connStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@DateFrom", dateFrom);
            dbParams[1] = new DbParameter("@DateTo", dateTo);
            dbParams[2] = new DbParameter("@ProductCode", strProductCode);

            var result = DbRetrieve("dbo.IN_PriceList_Get_DateFrom_DateTo_ProductCode", dsVendor, dbParams, TableName,
                connStr);

            if (result)
            {
                dsVendor.Tables[TableName].PrimaryKey = GetPK(dsVendor);
                return dsVendor;
            }
            return null;
        }

        /// <summary>
        ///     Get data by vendor code
        /// </summary>
        /// <param name="dsVendor"></param>
        /// <param name="strVendorCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetListByVendorCode(DataSet dsVendor, string strVendorCode, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@VendorCode", strVendorCode);

            var result = DbRetrieve("dbo.IN_PriceList_GetList_VendorCode", dsVendor, dbParams, TableName, connStr);

            if (result)
            {
                dsVendor.Tables[TableName].PrimaryKey = GetPK(dsVendor);
                return dsVendor;
            }
            return null;
        }

        /// <summary>
        ///     Get data by product code
        /// </summary>
        /// <param name="dsVendor"></param>
        /// <param name="strProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetListByProductCode(DataSet dsVendor, string strProductCode, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", strProductCode);

            var result = DbRetrieve("dbo.IN_PriceList_GetList_ProductCode", dsVendor, dbParams, TableName, connStr);

            if (result)
            {
                dsVendor.Tables[TableName].PrimaryKey = GetPK(dsVendor);
                return dsVendor;
            }
            return null;
        }

        /// <summary>
        ///     Get all data
        /// </summary>
        /// <param name="dsVendor"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetList(DataSet dsVendor, string connStr)
        {
            var result = DbRetrieve("IN_PriceList_GetList", dsVendor, null, TableName, connStr);

            if (result)
            {
                dsVendor.Tables[TableName].PrimaryKey = GetPK(dsVendor);
                return dsVendor;
            }
            return null;
        }

        /// <summary>
        ///     Get schema
        /// </summary>
        /// <param name="dsPriceList"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsPriceList, string ConnStr)
        {
            return DbRetrieve("dbo.IN_PriceList_GetSchema", dsPriceList, null, TableName, ConnStr);
        }

        /// <summary>
        ///     Get data group by vendor code
        /// </summary>
        /// <param name="dsInv"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetGroupVendor(DataSet dsInv, string ConnStr)
        {
            return DbRetrieve("dbo.IN_PriceList_GetListGroupVendorCode", dsInv, null, TableName, ConnStr);
        }

        /// <summary>
        ///     Get data group by product code.
        /// </summary>
        /// <param name="dsInv"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetGroupProduct(DataSet dsInv, string ConnStr)
        {
            return DbRetrieve("dbo.IN_PriceList_GetListGroupProductCode", dsInv, null, TableName, ConnStr);
        }

        /// <summary>
        ///     Get vendor product code.
        /// </summary>
        /// <param name="strVendorCode"></param>
        /// <param name="strProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetVendorSKU(string strVendorCode, string strProductCode, string connStr)
        {
            var vSKU = string.Empty;
            var dsTmp = new DataSet();

            GetListByVendorCodeProductCode(dsTmp, strVendorCode, strProductCode, connStr);

            if (dsTmp.Tables[0].Rows.Count > 0)
            {
                vSKU = dsTmp.Tables[0].Rows[0]["VendorProdCode"].ToString();
            }

            return vSKU;
        }

        /// <summary>
        ///     Get ref no.
        /// </summary>
        /// <param name="strVendorCode"></param>
        /// <param name="strProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetQuote(string strVendorCode, string strProductCode, string connStr)
        {
            var strQuote = string.Empty;
            var dsTmp = new DataSet();

            GetListByVendorCodeProductCode(dsTmp, strVendorCode, strProductCode, connStr);

            if (dsTmp != null)
            {
                if (dsTmp.Tables[TableName].Rows.Count > 0)
                {
                    strQuote = dsTmp.Tables[0].Rows[0]["RefNo"].ToString();
                }
            }

            return strQuote;
        }

        /// <summary>
        ///     Save data
        /// </summary>
        /// <param name="dsProduct"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsPriceList, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsPriceList, SelectCommand, TableName);
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsPriceList"></param>
        /// <returns></returns>
        private DataColumn[] GetPK(DataSet dsPriceList)
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsPriceList.Tables[TableName].Columns["PrlNo"];

            return primaryKeys;
        }

        /// <summary>
        ///     Get Prefer Vendor from Price List by Product Code, Date and Request Quantity.
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="PrDate"></param>
        /// <param name="ReqQty"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public DataTable GetList(string ProductCode, DateTime PrDate, decimal ReqQty, string ReqUnit, string ConnStr)
        {
            var dbParams = new DbParameter[4];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);
            dbParams[1] = new DbParameter("@Date", PrDate.ToString("yyyy-MM-dd"));
            dbParams[2] = new DbParameter("@ReqQty", ReqQty.ToString());
            dbParams[3] = new DbParameter("@ReqUnit", ReqUnit);

            return DbRead("[IN].PriceList_GetPreferVendorList", dbParams, ConnStr);
        }

        public bool GetList(DataSet dsPriceList, string ProductCode, DateTime PrDate, decimal ReqQty, string ReqUnit,
            string ConnStr)
        {
            var dbParams = new DbParameter[4];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);
            dbParams[1] = new DbParameter("@Date", PrDate.ToString("yyyy-MM-dd"));
            dbParams[2] = new DbParameter("@ReqQty", ReqQty.ToString());
            dbParams[3] = new DbParameter("@ReqUnit", ReqUnit);

            return DbRetrieve("[IN].PriceList_GetPreferVendorList", dsPriceList, dbParams, TableName, ConnStr);
        }

        public bool GetListHQ(DataSet dsPriceList, string BuGrpCode, string ProductCode, DateTime PrDate, decimal ReqQty,
            string ReqUnit)
        {
            // Get All actived Bu in specified BuGrpCode
            var dsBu = new DataSet();

            var result = bu.GetList(dsBu, BuGrpCode);

            if (result)
            {
                // Get all data in group
                foreach (DataRow drBu in dsBu.Tables[bu.TableName].Rows)
                {
                    // Create Connection String by Business Unit
                    var connStr = new Common.ConnectionStringConstant().Get(drBu);

                    GetList(dsPriceList, ProductCode, PrDate, ReqQty, ReqUnit, connStr);
                }
            }

            return result;
        }

        //public bool GetListByVendorCode(DataSet dsInv, ref string MsgError, string hdrNo, string connStr)
        //{
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0]            = new DbParameter("@HdrNo", hdrNo.ToString());

        //    bool result = DbRetrieve("dbo.IN_Inventory_Get_HdrNo", dsInv, dbParams, this.TableName, connStr);

        //    if (!result)
        //    {
        //        MsgError = "Msg001";
        //        return false;
        //    }

        //    return true;
        //}


        //public int GetMaxInvNo(string ConnStr)
        //{
        //    return DbReadScalar("dbo.IN_Inventory_GetMaxNo", null, ConnStr);
        //}
    }
}