using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class BudgetReport : DbHandler
    {
        /// <summary>
        ///     Connect to table budgetreport.
        /// </summary>
        public BudgetReport()
        {
            SelectCommand = "SELECT * FROM GL.BudgetReport";
            TableName = "BudgetReport";
        }

        /// <summary>
        ///     Return dataset from BudgetReport.
        /// </summary>
        /// <param name="dsBudget"></param>
        public void GetBudgetReportList(DataSet dsBudget, string connStr)
        {
            DbRetrieve("GL.GetBudgetReportList", dsBudget, null, TableName, connStr);
        }
    }
}