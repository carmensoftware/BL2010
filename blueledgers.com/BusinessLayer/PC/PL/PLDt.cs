using System.Data;
using Blue.DAL;

namespace Blue.BL.PC.PL
{
    public class PLDt : DbHandler
    {
        public PLDt()
        {
            SelectCommand = "SELECT * FROM [IN].[PLDt]";
            TableName = "PLDt";
        }

        public bool GetSchame(DataSet dsPL, string ConnStr)
        {
            return DbRetrieveSchema("[IN].PLDt_GetList", dsPL, null, TableName, ConnStr);
        }

        public bool GetList(DataSet dsPLDt, string ID, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PriceLstNo", ID);

            return DbRetrieve("[IN].PLDt_GetList_PriceLstNo", dsPLDt, dbParams, TableName, ConnStr);
        }

        public bool Save(DataSet dsSaving, string ConnStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSaving, SelectCommand, TableName);

            return DbCommit(dbSaveSource, ConnStr);
        }

        public DataTable GetList(DataSet dsPLDT, string PriceLstNo, string ProductCode, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@PriceLstNo", PriceLstNo);
            dbParams[1] = new DbParameter("@ProductCode", ProductCode);

            return DbRead("[IN].[PLDt_GetListByPriceLstProductCode]", dbParams, ConnStr);
        }

        public bool GetList(DataSet dsPriceList, int PriceLstNo, int SeqNo, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@PriceLstNo", PriceLstNo.ToString());
            dbParams[1] = new DbParameter("@SeqNo", SeqNo.ToString());

            return DbRetrieve("[IN].[PLDt_GetByPriceLstNoSeqNo]", dsPriceList, dbParams, TableName, ConnStr);
        }
    }
}