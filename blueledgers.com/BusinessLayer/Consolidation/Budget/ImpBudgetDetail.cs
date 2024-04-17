using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.Budget
{
    public class ImpBudgetDetail : DbHandler
    {
        #region "Attributies"

        public int ImpBudgetID { get; set; }

        public int ImpBudgetDetailID { get; set; }

        public string AccountCode { get; set; }

        public int PeriodID { get; set; }

        public string DivisionCode { get; set; }

        public string DepartmentCode { get; set; }

        public string SectionCode { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int UpdatedBy { get; set; }

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public ImpBudgetDetail()
        {
            SelectCommand = "SELECT * FROM Budget.ImpBudgetDetail";
            TableName = "ImpBudgetDetail";
        }

        /// <summary>
        ///     Retrieve data in table budgetdetail
        /// </summary>
        /// <param name="budgetID"></param>
        /// <param name="dsBudget"></param>
        /// <returns></returns>
        public DataSet GetImpBudgetDetailList(string impBudgetID, DataSet dsImpBudget, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@impBudgetCode", impBudgetID);

            DbRetrieve("Budget.GetImpBudgetDetailList", dsImpBudget, dbParams, TableName, connStr);
            return dsImpBudget;
        }

        /// <summary>
        ///     Get budget detail all
        /// </summary>
        /// <param name="dsBudgetDetailAll"></param>
        /// <returns></returns>
        public DataSet GetImpBudgetDetailAll(DataSet dsImpBudgetDetail, string connStr)
        {
            DbRetrieve("Budget.GetImpBudgetDetailAll", dsImpBudgetDetail, null, TableName, connStr);
            return dsImpBudgetDetail;
        }

        /// <summary>
        ///     Retrieve schema to dataset
        /// </summary>
        /// <param name="dsBudget"></param>
        public void GetImpBudgetSchema(DataSet dsImpBudget, string connStr)
        {
            DbRetrieveSchema("Budget.GetImpBudgetDetail", dsImpBudget, null, TableName, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsSaveBudgetDetail"></param>
        public void GetSaveImpBudgetSchema(DataSet dsSaveImpBudgetDetail, string connStr)
        {
            DbRetrieveSchema("Budget.GetSaveImpBudgetDetail", dsSaveImpBudgetDetail, null, TableName, connStr);
        }

        /// <summary>
        ///     Get new budgetid
        /// </summary>
        /// <returns></returns>
        public int GetNewImpBudgetDetailID(string connStr)
        {
            var dsTmp = new DataSet();
            DbRetrieve("Budget.GetImpBudgetDetailMax", dsTmp, null, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows[0]["ImpBudgetDetailID"].ToString() != string.Empty)
            {
                return (int) dsTmp.Tables[TableName].Rows[0]["ImpBudgetDetailID"];
            }
            return 1;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsSaveBudget"></param>
        /// <returns></returns>
        public bool Save(DataSet dsSaveImpBudgetDetail, string connStr)
        {
            // Build SaveSource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSaveImpBudgetDetail, SelectCommand, TableName);

            // Call dbCommit and send SaveSource object is parameter
            DbCommit(dbSaveSource, connStr);

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="savedData"></param>
        /// <returns></returns>
        public bool Delete(DataSet savedData, string connStr)
        {
            // สร้าง SaveSource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);

            // เรียก dbCommit โดยส่ง SaveSource object เป็น parameter
            DbCommit(dbSaveSource, connStr);

            return true;
        }

        #endregion
    }
}