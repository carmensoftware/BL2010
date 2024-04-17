using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.PC.PL
{
    public class PL : DbHandler
    {
        private readonly PLDt plDt = new PLDt();

        public PL()
        {
            SelectCommand = "SELECT * FROM [IN].[PL]";
            TableName = "PL";
        }

        public bool Get(DataSet dsPL, int PriceLstNo, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PriceLstNo", PriceLstNo.ToString());

            return DbRetrieve("[IN].PL_Get_PriceLstNo", dsPL, dbParams, TableName, ConnStr);
        }

        public bool GetSchame(DataSet dsPL, string ConnStr)
        {
            return DbRetrieveSchema("[IN].PL_GetList", dsPL, null, TableName, ConnStr);
        }

        public int GetNewID(string ConnStr)
        {
            return DbReadScalar("[IN].PL_GetNewID", null, ConnStr);
        }

        public bool Delete(DataSet dsDeleting, string ConnStr)
        {
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsDeleting, plDt.SelectCommand, plDt.TableName);
            dbSaveSource[1] = new DbSaveSource(dsDeleting, SelectCommand, TableName);

            return DbCommit(dbSaveSource, ConnStr);
        }

        public bool Save(DataSet dsSaving, string ConnStr)
        {
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsSaving, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsSaving, plDt.SelectCommand, plDt.TableName);

            return DbCommit(dbSaveSource, ConnStr);
        }

        public int CountByVendorDateFromTo(string VendorCode, DateTime DateFrom, DateTime DateTo, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@VendorCode", VendorCode);
            dbParams[1] = new DbParameter("@DateFrom", DateFrom.ToString("yyyy-MM-dd"));
            dbParams[2] = new DbParameter("@DateTo", DateTo.ToString("yyyy-MM-dd"));

            return DbReadScalar("[IN].[PriceLst_CountPriceLstByVendorDateFromTo]", dbParams, ConnStr);
        }

        public int CountByVendorDateFrom(string VendorCode, DateTime DateFrom, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@VendorCode", VendorCode);
            dbParams[1] = new DbParameter("@DateFrom", DateFrom.ToString("yyyy-MM-dd"));

            return DbReadScalar("[IN].[PriceLst_CountPriceLstByVendorDateFrom]", dbParams, ConnStr);
        }

        public int CountByVendorDateTo(string VendorCode, DateTime DateTo, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@VendorCode", VendorCode);
            dbParams[1] = new DbParameter("@DateTo", DateTo.ToString("yyyy-MM-dd"));

            return DbReadScalar("[IN].[PriceLst_CountPriceLstByVendorDateTo]", dbParams, ConnStr);
        }

        public int CountByVendor(string VendorCode, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@VendorCode", VendorCode);

            return DbReadScalar("[IN].[PriceLst_CountPriceLstByVendor]", dbParams, ConnStr);
        }

        public DataTable GetList(string VendorCode, DateTime DateFrom, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@VendorCode", VendorCode);
            dbParams[1] = new DbParameter("@DateFrom", DateFrom.ToString("yyyy-MM-dd"));

            return DbRead("[IN].[PriceLst_GetPriceLstByVendorDateFrom]", dbParams, ConnStr);
        }

        public DataTable GetListByVendorDateTo(string VendorCode, DateTime DateTo, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@VendorCode", VendorCode);
            dbParams[1] = new DbParameter("@DateTo", DateTo.ToString("yyyy-MM-dd"));

            return DbRead("[IN].[PriceLst_GetPriceLstByVendorDateTo]", dbParams, ConnStr);
        }

        public DataTable GetList(string VendorCode, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@VendorCode", VendorCode);

            return DbRead("[IN].[PriceLst_GetPriceLstByVendor]", dbParams, ConnStr);
        }

        public DataTable GetListByVendorDateFromTo(string VendorCode, DateTime DateFrom, DateTime DateTo, string ConnStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@VendorCode", VendorCode);
            dbParams[1] = new DbParameter("@DateFrom", DateFrom.ToString("yyyy-MM-dd"));
            dbParams[2] = new DbParameter("@DateTo", DateTo.ToString("yyyy-MM-dd"));

            return DbRead("[IN].[PriceLst_GetPriceLstByVendorDateFromTo]", dbParams, ConnStr);
        }

        public bool GetList(DataSet dsPriceList, string VendorCode, DateTime DateFrom, DateTime DateTo, string ConnStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@VendorCode", VendorCode);
            dbParams[1] = new DbParameter("@DateFrom", DateFrom.ToString("yyyy-MM-dd"));
            dbParams[2] = new DbParameter("@DateTo", DateTo.ToString("yyyy-MM-dd"));

            return DbRetrieve("[IN].[PriceLst_GetPriceLstByVendorDateFromTo]", dsPriceList, dbParams, TableName, ConnStr);
        }

        public bool GetList(DataSet dsPriceList, string VendorCode, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@VendorCode", VendorCode);
            ;

            return DbRetrieve("[IN].[PriceLst_GetPriceLstByVendor]", dsPriceList, dbParams, TableName, ConnStr);
        }

        public bool GetList(DataSet dsPriceList, string VendorCode, DateTime DateFrom, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@VendorCode", VendorCode);
            dbParams[1] = new DbParameter("@DateFrom", DateFrom.ToString("yyyy-MM-dd"));

            return DbRetrieve("[IN].[PriceLst_GetPriceLstByVendorDateFrom]", dsPriceList, dbParams, TableName, ConnStr);
        }

        public bool GetListVendorDateTo(DataSet dsPriceList, string VendorCode, DateTime DateTo, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@VendorCode", VendorCode);
            dbParams[1] = new DbParameter("@DateTo", DateTo.ToString("yyyy-MM-dd"));

            return DbRetrieve("[IN].[PriceLst_GetPriceLstByVendorDateTo]", dsPriceList, dbParams, TableName, ConnStr);
        }
    }
}