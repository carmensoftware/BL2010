using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class BudgetDetail : DbHandler
    {
        #region "Attributies"

        public int BudgetID { get; set; }

        public int BudgetDetailID { get; set; }

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
        public BudgetDetail()
        {
            SelectCommand = "SELECT * FROM GL.BudgetDetail";
            TableName = "BudgetDetail";
        }

        /// <summary>
        ///     Retrieve data in table budgetdetail
        /// </summary>
        /// <param name="budgetID"></param>
        /// <param name="dsBudget"></param>
        /// <returns></returns>
        public DataSet GetBudgetDetailList(string budgetID, DataSet dsBudget, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@budgetID", budgetID);

            DbRetrieve("GL.GetBudgetDetailList-01", dsBudget, dbParams, TableName, connStr);
            return dsBudget;
        }

        /// <summary>
        ///     Get budget detail all
        /// </summary>
        /// <param name="dsBudgetDetailAll"></param>
        /// <returns></returns>
        public DataSet GetBudgetDetailAll(DataSet dsBudgetDetailAll, string connStr)
        {
            DbRetrieve("GL.GetBudgetDetailAll", dsBudgetDetailAll, null, TableName, connStr);
            return dsBudgetDetailAll;
        }

        /// <summary>
        ///     Retrieve data in store procedure 'GetBudgetDetailGroup'
        /// </summary>
        /// <param name="budgetID"></param>
        /// <param name="dsBDetailGroup"></param>
        /// <returns></returns>
        public DataSet GetBudgetDetailGroup(string budgetID, string year, string departmentCode, string divisionCode,
            string sectionCode, DataSet dsBDetailGroup, string connStr)
        {
            //// Paramter value assign to dbparameter array.
            //DbParameter[] dbParams  = new DbParameter[2];
            //dbParams[0]             = new DbParameter("@BudgetID", budgetID);
            //dbParams[1]             = new DbParameter("@Year", year);

            //DbRetrieve("GL.GetBudgetDetailGroup", dsBDetailGroup, dbParams, this.TableName,connStr);
            //return dsBDetailGroup;                 

            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[5];
            dbParams[0] = new DbParameter("@BudgetID", budgetID);
            dbParams[1] = new DbParameter("@Year", year);
            dbParams[2] = new DbParameter("@DepartmentCode", departmentCode);
            dbParams[3] = new DbParameter("@DivisionCode", divisionCode);
            dbParams[4] = new DbParameter("@SectionCode", sectionCode);

            DbRetrieve("GL.GetBDetailGroupByAccount", dsBDetailGroup, dbParams, TableName, connStr);
            return dsBDetailGroup;
        }

        /// <summary>
        ///     Retrieve data in table budgetdetail and group by account code
        /// </summary>
        /// <param name="budgetID"></param>
        /// <param name="dsBudget"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetBDetailGroupByAccount(string budgetID, DataSet dsBudget, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BudgetID", budgetID);

            DbRetrieve("GL.GetBDetailGroupByAccount", dsBudget, dbParams, TableName, connStr);
            return dsBudget;
        }

        /// <summary>
        ///     Retrieve Data in table budgetdetail send parameters
        /// </summary>
        /// <param name="budgetID"></param>
        /// <param name="accountCode"></param>
        /// <param name="divisionCode"></param>
        /// <param name="departmentCode"></param>
        /// <param name="sectionCode"></param>
        /// <param name="dsBudget"></param>
        /// <returns></returns>
        public DataSet GetBudegetDetail(string budgetID, string accountCode, string divisionCode,
            string departmentCode, string sectionCode, string connStr)
        {
            {
                // Paramter value assign to dbparameter array.
                var dsTmp = new DataSet();
                var dbParams = new DbParameter[5];
                dbParams[0] = new DbParameter("@BudgetID", budgetID);
                dbParams[1] = new DbParameter("@AccountCode", accountCode);
                dbParams[2] = new DbParameter("@DivisionCode", divisionCode);
                dbParams[3] = new DbParameter("@DepartmentCode", departmentCode);
                dbParams[4] = new DbParameter("@SectionCode", sectionCode);

                DbRetrieve("GL.GetBudgetDetail", dsTmp, dbParams, TableName, connStr);
                return dsTmp;
            }
        }

        /// <summary>
        ///     Retrieve Data in table budgetdetail for grid
        /// </summary>
        /// <param name="budgetID"></param>
        /// <param name="accountCode"></param>
        /// <param name="divisionCode"></param>
        /// <param name="departmentCode"></param>
        /// <param name="sectionCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataSet GetBudgetDetailAmount(string budgetID, string accountCode, string divisionCode,
            string departmentCode, string sectionCode, string year, string connStr)
        {
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[6];
            dbParams[0] = new DbParameter("@BudgetID", budgetID);
            dbParams[1] = new DbParameter("@AccountCode", accountCode);
            dbParams[2] = new DbParameter("@DivisionCode", divisionCode);
            dbParams[3] = new DbParameter("@DepartmentCode", departmentCode);
            dbParams[4] = new DbParameter("@SectionCode", sectionCode);
            dbParams[5] = new DbParameter("@Year", year);

            DbRetrieve("GL.GetBudgetDetailAmountGroup", dsTmp, dbParams, TableName, connStr);
            return dsTmp;
        }

        /// <summary>
        /// </summary>
        /// <param name="accountCode"></param>
        /// <param name="divisionCode"></param>
        /// <param name="departmentCode"></param>
        /// <param name="sectionCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataSet GetBudgetDetailShowGrid(string budgetID, string accountCode,
            string divisionCode, string departmentCode,
            string sectionCode, string year, string connStr)
        {
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[6];
            dbParams[0] = new DbParameter("@BudgetID", budgetID);
            dbParams[1] = new DbParameter("@AccountCode", accountCode);
            dbParams[2] = new DbParameter("@DivisionCode", divisionCode);
            dbParams[3] = new DbParameter("@DepartmentCode", departmentCode);
            dbParams[4] = new DbParameter("@SectionCode", sectionCode);
            dbParams[5] = new DbParameter("@Year", year);

            DbRetrieve("GL.GetBudgetDetailShowGrid", dsTmp, dbParams, TableName, connStr);
            return dsTmp;
        }

        /// <summary>
        /// </summary>
        /// <param name="budgetID"></param>
        /// <param name="accountCode"></param>
        /// <param name="divisionCode"></param>
        /// <param name="departmentCode"></param>
        /// <param name="sectionCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public Decimal GetBDetailAmount(string budgetID, string accountCode, string divisionCode,
            string departmentCode, string sectionCode, string year, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[6];
            dbParams[0] = new DbParameter("@BudgetID", budgetID);
            dbParams[1] = new DbParameter("@AccountCode", accountCode);
            dbParams[2] = new DbParameter("@DivisionCode", divisionCode);
            dbParams[3] = new DbParameter("@DepartmentCode", departmentCode);
            dbParams[4] = new DbParameter("@SectionCode", sectionCode);
            dbParams[5] = new DbParameter("@Year", year);

            DbRetrieve("GL.GetBDetailAmount", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName] != null)
            {
                if (dsTmp.Tables[TableName].Rows[0]["Amount"].ToString() != string.Empty)
                {
                    return decimal.Parse(dsTmp.Tables[TableName].Rows[0]["Amount"].ToString());
                }
                return 0;
            }
            return 0;
        }

        /// <summary>
        ///     Get table budget and budget detail by period id and year
        /// </summary>
        /// <param name="periodID"></param>
        /// <param name="year"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetBudgetListByPeriodYear(string periodID, string year, DataSet dsBudget, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@period", periodID);
            dbParams[1] = new DbParameter("@year", year);

            DbRetrieve("GL.GetBudgetListByPeriodYear", dsBudget, dbParams, TableName, connStr);
            return dsBudget;
        }

        /// <summary>
        ///     Retrieve schema to dataset
        /// </summary>
        /// <param name="dsBudget"></param>
        public void GetBudgetDetailSchema(DataSet dsBudget, string connStr)
        {
            DbRetrieveSchema("GL.GetBudgetDetailAll", dsBudget, null, TableName, connStr);
        }

        public void GetBudgetDetailNewSchema(DataSet dsBudget, string connStr)
        {
            DbRetrieveSchema("GL.GetBudgetDetailList", dsBudget, null, TableName, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsSaveBudgetDetail"></param>
        public void GetSaveBudgetSchema(DataSet dsSaveBudgetDetail, string connStr)
        {
            DbRetrieveSchema("GL.GetSaveBudgetDetail", dsSaveBudgetDetail, null, TableName, connStr);
        }

        //public DataSet GetBDetailGroupSchema(DataSet dsBDetailGroup)
        //{
        //    //return dsBDetailGroup;
        //    DataTable BudgetDetail = new DataTable("BudgetDetail");
        //    //BudgetDetail.Columns.Add("BudgetDetailID");
        //    BudgetDetail.Columns.Add("PeriodID");
        //    BudgetDetail.Columns.Add("PeriodNumberID");
        //    BudgetDetail.Columns.Add("Actual");
        //    BudgetDetail.Columns.Add("Amount");            
        //    dsBDetailGroup.Tables.Add(BudgetDetail);
        //    //DbRetrieveSchema("GL.BudgetPeriod", dsBDetailGroup, null, this.TableName);
        //    return dsBDetailGroup;
        //}
        //public DataSet GetBDGroupByAccountSchema(DataSet dsBDetailGroup)
        //{
        //    //return dsBDetailGroup;
        //    //DataSet dsBudget, string connStr
        //    DataTable BudgetDetail = new DataTable("BudgetDetail");
        //    BudgetDetail.Columns.Add("Description");
        //    BudgetDetail.Columns.Add("Year");
        //    BudgetDetail.Columns.Add("BudgetID");
        //    BudgetDetail.Columns.Add("AccountCode");
        //    dsBDetailGroup.Tables.Add(BudgetDetail);
        //    return dsBDetailGroup;
        //    //DbRetrieveSchema("GL.GetBDetailGroupByAccountSchema", dsBudget, null, this.TableName, connStr);
        //}
        /// <summary>
        ///     Retrieve schema to dataset
        /// </summary>
        /// <param name="dsBDetailGroup"></param>
        /// <summary>
        ///     Retrieve schema to dataset
        /// </summary>
        /// <param name="dsBDetailGroup"></param>
        /// <returns></returns>
        /// <summary>
        ///     Get new budgetid
        /// </summary>
        /// <returns></returns>
        public int GetNewBudgetDetailID(string connStr)
        {
            var dsTmp = new DataSet();

            DbRetrieve("GL.GetBudgetDetailMax", dsTmp, null, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows[0]["BudgetDetailID"].ToString() != string.Empty)
            {
                return (int) dsTmp.Tables[TableName].Rows[0]["BudgetDetailID"];
            }
            return 1;
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

        /// <summary>
        ///     Save data only table budget detail
        /// </summary>
        /// <param name="dsSaveBudget"></param>
        /// <returns></returns>
        public bool Save(DataSet dsSaveBudgetDetail, string connStr)
        {
            // Build SaveSource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSaveBudgetDetail, SelectCommand, TableName);

            // Call dbCommit and send SaveSource object is parameter
            DbCommit(dbSaveSource, connStr);

            return true;
        }

        #endregion
    }
}