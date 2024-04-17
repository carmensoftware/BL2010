using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class BudgetTool : DbHandler
    {
        #region "Operations"

        /// <summary>
        ///     Connect to table BudgetTool.
        /// </summary>
        public BudgetTool()
        {
            SelectCommand = "SELECT * FROM GL.BudgetTool";
            TableName = "BudgetTool";
        }

        /// <summary>
        ///     Return datatable from BudgetTool.
        /// </summary>
        /// <param name="dsBudget"></param>
        public void GetBudgetToolList(DataSet dsBudget, string connStr)
        {
            DbRetrieve("GL.GetBudgetToolList", dsBudget, null, TableName, connStr);
        }

        #endregion
    }
}