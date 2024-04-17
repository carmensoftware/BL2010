using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.Setup.Application
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
            SelectCommand = "SELECT * FROM Application.Department";
            TableName = "Department";
        }

        /// <summary>
        ///     Get department by department code.
        /// </summary>
        /// <param name="dsDepartment"></param>
        /// <param name="departmentCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsDepartment, string departmentCode, string connStr)
        {
            var result = false;

            // Create parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DepartmentCode", departmentCode);

            // Get data
            result = DbRetrieve("[Application].GetDepartment", dsDepartment, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get department by department code.
        /// </summary>
        /// <param name="departmentCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable Get(string departmentCode, string connStr)
        {
            var dsDepartment = new DataSet();

            // Get data
            var result = Get(dsDepartment, departmentCode, connStr);

            // Return result
            if (result)
            {
                return dsDepartment.Tables[TableName];
            }
            return null;
        }

        /// <summary>
        ///     Get all department data.
        /// </summary>
        /// <param name="dsDivision"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsDepartment, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Application.GetDepartmentList", dsDepartment, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all department data.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            var dsDepartment = new DataSet();

            // Get data
            var result = GetList(dsDepartment, connStr);

            // Return result
            if (result)
            {
                return dsDepartment.Tables[TableName];
            }
            return null;
        }

        /// <summary>
        ///     Get all actived or inactived department.
        /// </summary>
        /// <param name="dsDepartment"></param>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsDepartment, bool isActive, string connStr)
        {
            // Create parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@IsActive", isActive.ToString());

            // Get data and return result
            return DbRetrieve("[Application].GetDepartmentListActive", dsDepartment, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get all actived or inactived department.
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(bool isActive, string connStr)
        {
            var dsDepartment = new DataSet();

            // Get data 
            var result = GetList(dsDepartment, isActive, connStr);

            // Return result
            if (result)
            {
                return dsDepartment.Tables[TableName];
            }
            return null;
        }

        /// <summary>
        ///     Get Department's Description of sepecified DepartmentCode
        /// </summary>
        /// <param name="departmentCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetDescription(string departmentCode, string connStr)
        {
            var dsDepartment = new DataSet();

            // Get data
            var result = Get(dsDepartment, departmentCode, connStr);

            // Return result
            if (result)
            {
                if (dsDepartment.Tables[TableName] != null)
                {
                    if (dsDepartment.Tables[TableName].Rows.Count > 0)
                    {
                        var drDepartment = dsDepartment.Tables[TableName].Rows[0];

                        return (drDepartment["Description"] == DBNull.Value
                            ? string.Empty
                            : drDepartment["Description"].ToString());
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Get max division Code.
        /// </summary>
        /// <returns></returns>
        public int GetNewCode(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("[Application].GetDepartmentNewCode", null, connStr);

            // If return null valule
            if (result == Convert.ToInt32("0"))
            {
                result = 1;
            }

            // Return result
            return result;
        }

        /// <summary>
        ///     Get Department data structure.
        /// </summary>
        /// <param name="dsDepartment"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsDepartment, string connStr)
        {
            // Get data and return result
            return DbRetrieveSchema("[Application].GetDepartmentList", dsDepartment, null, TableName, connStr);
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