using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class Department : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public Department()
        {
            SelectCommand = "SELECT * FROM Reference.Department";
            TableName = "Department";
        }

        /// <summary>
        ///     Get only name department
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public string GetName(string DepCode, string connStr)
        {
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DepartmentCode", DepCode);

            DbRetrieve("Reference.GetDepartment", dsTmp, dbParams, TableName, connStr);
            return dsTmp.Tables[TableName].Rows[0]["Description"].ToString();
        }

        /// <summary>
        ///     Get all data
        /// </summary>
        /// <returns></returns>
        public DataTable GetDepartmentList(string connStr)
        {
            var dsDepartment = new DataSet();

            // Get data
            var department = new Department();


            department.DbRetrieve("Reference.GetDepartmentList", dsDepartment, null, TableName, connStr);

            // Return result
            if (dsDepartment.Tables[TableName] != null)
            {
                var drBlank = dsDepartment.Tables[TableName].NewRow();
                dsDepartment.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsDepartment.Tables[TableName];
        }

        /// <summary>
        /// </summary>
        /// <param name="dsDivision"></param>
        /// <returns></returns>
        public bool GetDepartmentList(DataSet dsDepartment, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Reference.GetDepartmentList", dsDepartment, null, TableName, connStr);

            // Return result
            return result;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="divisionCode"></param>
        ///// <param name="dsDepartment"></param>
        ///// <returns></returns>
        //public DataSet GetDepartmentListByDivision(string divisionCode, DataSet dsDepartment,string connStr)
        //{
        //    DbParameter[] dbParams = new DbParameter[1];

        //    dbParams[0]            = new DbParameter("@DivisionCode", divisionCode);

        //    //return DbRead("GL.GetBudgetSAcc", dbParams);
        //    DbRetrieve("Reference.GetDepartmentListByDivision", dsDepartment, dbParams, this.TableName,connStr);

        //    return dsDepartment;
        //}

        /// <summary>
        ///     Get only data from param
        /// </summary>
        /// <returns></returns>
        public DataTable GetDepartmentLookup(string year, string revisionNo, string connStr)
        {
            var dsDepartment = new DataSet();

            // Get data
            var department = new Department();
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@Year", year);
            dbParams[1] = new DbParameter("@RevisionNo", revisionNo);

            department.DbRetrieve("Reference.GetDepartmentLookup", dsDepartment, dbParams, TableName, connStr);

            // Return result
            if (dsDepartment.Tables[TableName] != null)
            {
                var drBlank = dsDepartment.Tables[TableName].NewRow();
                dsDepartment.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsDepartment.Tables[TableName];
        }

        /// <summary>
        ///     Get active department list.
        /// </summary>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public DataTable GetActiveList(string strConn)
        {
            var dsDep = new DataSet();

            var result = DbRetrieve("Reference.GetDepartmentActiveList", dsDep, null, TableName, strConn);

            if (result)
            {
                return dsDep.Tables[TableName];
            }
            return null;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="DivisionCode"></param>
        ///// <param name="connStr"></param>
        ///// <returns></returns>
        //public DataTable GetDepartmentLookupByDivisionCode(string DivisionCode, string connStr)
        //{
        //    DataSet dsDepartment   = new DataSet();

        //    // Get data
        //    Department department  = new Department();

        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0]            = new DbParameter("@DivisionCode", DivisionCode);

        //    department.DbRetrieve("GL.GetDepartmentLookup", dsDepartment, dbParams, this.TableName, connStr);

        //    return dsDepartment.Tables[this.TableName];
        //}

        /// <summary>
        ///     Get max division Code.
        /// </summary>
        /// <returns></returns>
        public int GetDepartmentCodeMax(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("Reference.GetDepartmentCodeMax", null, connStr);

            // If return null valule
            if (result == Convert.ToInt32("0"))
            {
                result = 1;
            }

            // Return result
            return result;
        }

        /// <summary>
        ///     save process.
        /// </summary>
        /// <param name="dsDivision"></param>
        /// <returns></returns>
        public bool Save(DataSet dsDepartment, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsDepartment, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}