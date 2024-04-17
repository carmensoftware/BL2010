using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class BudgetMisc : DbHandler
    {
        #region " Attributies "

        public int BudgetID { get; set; }
        public Guid FieldID { get; set; }
        public string Value { get; set; }

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public BudgetMisc()
        {
            SelectCommand = "SELECT * FROM GL.BudgetMisc";
            TableName = "BudgetMisc";
        }

        /// <summary>
        ///     Get all JournalVoucerMisc data the relate to specified JournalVoucherNo.
        /// </summary>
        /// <param name="dsJournalVoucherMisc"></param>
        /// <param name="JournalVoucherNo"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsBudgetMisc, int budGetID, string ConnectionString)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@BudgetID", Convert.ToString(budGetID));

            return DbRetrieve("GL.GetBudgetMiscListByBudgetID", dsBudgetMisc, dbParam, TableName, ConnectionString);
        }

        /// <summary>
        ///     Get Budget Misc Schema
        /// </summary>
        /// <param name="dsBudgetMisc"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsBudgetMisc, string ConnectionString)
        {
            return DbRetrieveSchema("GL.GetBudgetMiscList", dsBudgetMisc, null, TableName, ConnectionString);
        }

        /// <summary>
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Delete(DataSet savedData, string connStr)
        {
            //Build SaveSource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);

            //Call dbCommit and sent SaveSource object by parameter
            DbCommit(dbSaveSource, connStr);

            return true;
        }

        #endregion
    }
}