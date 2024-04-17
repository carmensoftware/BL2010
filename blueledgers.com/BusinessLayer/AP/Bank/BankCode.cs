using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.AP.Bank
{
    public class BankCode : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        public BankCode()
        {
            SelectCommand = "select * from AP.BankCode";
            TableName = "BankCode";
        }

        public bool Get(ref DataSet dsBankCode, string strBankCode, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BankCode", strBankCode);

            return DbRetrieve("AP.GetBankCode_BankCode", dsBankCode, dbParams, TableName, strConn);
        }

        public string GetName(string bankCode, string strConn)
        {
            var nameView = new DataSet();

            var result = Get(ref nameView, bankCode, strConn);

            if (result)
            {
                return nameView.Tables[TableName].Rows[0]["Name"].ToString();
            }
            return string.Empty;
        }

        public bool GetList(ref DataSet dsBankCode, string strConn)
        {
            return DbRetrieve("AP.GetBankCodeList", dsBankCode, null, TableName, strConn);
        }

        public DataTable GetActive(string strConn)
        {
            return DbRead("AP.GetBankCodeActive", null, strConn);
        }

        public bool Save(DataSet dsSaved, string strConn)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSaved, SelectCommand, TableName);

            return DbCommit(dbSaveSource, strConn);
        }

        #endregion
    }
}