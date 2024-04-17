using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.Setup.Application
{
    public class Division : DbHandler
    {
        /// <summary>
        ///     Empty contructor
        /// </summary>
        public Division()
        {
            SelectCommand = "SELECT * FROM Application.Division";
            TableName = "Division";
        }

        /// <summary>
        ///     Get division by disivion code.
        /// </summary>
        /// <param name="dsDivision"></param>
        /// <param name="divisionCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsDivision, string divisionCode, string connStr)
        {
            var result = false;

            // Create parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DivisionCode", divisionCode);

            // Get data
            result = DbRetrieve("[Application].GetDivision", dsDivision, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all division.
        /// </summary>
        /// <param name="dsDivision"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsDivision, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Application.GetDivisionList", dsDivision, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all division.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            var dsDividion = new DataSet();

            // Get data
            var result = GetList(dsDividion, connStr);

            // Return result
            if (result)
            {
                return dsDividion.Tables[TableName];
            }
            return null;
        }

        /// <summary>
        ///     Get all division depond on specified DepartmentCode
        /// </summary>
        /// <param name="dsDivision"></param>
        /// <param name="departmentCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsDivision, string departmentCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DepartmentCode", departmentCode);

            // Get data and return result
            return DbRetrieve("Application.GetDivisionListByDepartment", dsDivision, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get all division depond on specified DepartmentCode
        /// </summary>
        /// <param name="departmentCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string departmentCode, string connStr)
        {
            var dsDivision = new DataSet();

            // Get data
            var result = GetList(dsDivision, departmentCode, connStr);

            if (result)
            {
                return dsDivision.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get all actived or inactived division.
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsDivision, bool isActive, string connStr)
        {
            // Create parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@IsActive", isActive.ToString());

            // Get data and return value
            return DbRetrieve("[Aoolication].GetDivisionListActive", dsDivision, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get all actived or inactived division.
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(bool isActive, string connStr)
        {
            var dsDivision = new DataSet();

            // Get data
            var result = GetList(dsDivision, isActive, connStr);

            // Return result
            if (result)
            {
                return dsDivision.Tables[TableName];
            }
            return null;
        }

        /// <summary>
        ///     Get all actived or inactived division depend on specified DepartmentCode
        /// </summary>
        /// <param name="dsDivision"></param>
        /// <param name="departmentCode"></param>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsDivision, string departmentCode, bool isActive, string connStr)
        {
            // Create parameter
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@DepartmentCode", departmentCode);
            dbParams[1] = new DbParameter("@IsActive", isActive.ToString());

            // Get data and return result
            return DbRetrieve("[Application].GetDivisionListActiveByDepartment", dsDivision, dbParams, TableName,
                connStr);
        }

        /// <summary>
        ///     Get all actived or inactived division depend on specified DepartmentCode
        /// </summary>
        /// <param name="departmentCode"></param>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string departmentCode, bool isActive, string connStr)
        {
            var dsDivision = new DataSet();

            // Get data
            var result = GetList(dsDivision, departmentCode, isActive, connStr);

            if (result)
            {
                return dsDivision.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get Division's Description of sepecified DivisionCode
        /// </summary>
        /// <param name="divisionCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetDescription(string divisionCode, string connStr)
        {
            var dsDivision = new DataSet();

            // Get data
            var result = Get(dsDivision, divisionCode, connStr);

            // Return result
            if (result)
            {
                if (dsDivision.Tables[TableName] != null)
                {
                    if (dsDivision.Tables[TableName].Rows.Count > 0)
                    {
                        var drDivision = dsDivision.Tables[TableName].Rows[0];

                        return (drDivision["Description"] == DBNull.Value
                            ? string.Empty
                            : drDivision["Description"].ToString());
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
            result = DbReadScalar("[Application].GetDivisionNewCode", null, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get Division Table Structure.
        /// </summary>
        /// <param name="dsDivision"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsDivision, string connStr)
        {
            // Get data and return result
            return DbRetrieveSchema("Application.GetDivisionList", dsDivision, null, TableName, connStr);
        }

        /// <summary>
        ///     Get DepartmentCode from specified DivisionCode.
        /// </summary>
        /// <param name="divisionCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetParent(string divisionCode, string connStr)
        {
            var dsDivision = new DataSet();

            // Get data
            var result = Get(dsDivision, divisionCode, connStr);

            // Return result
            if (result)
            {
                if (dsDivision.Tables[TableName] != null)
                {
                    if (dsDivision.Tables[TableName].Rows.Count > 0)
                    {
                        var drDivision = dsDivision.Tables[TableName].Rows[0];

                        return (drDivision["DepartmentCode"] == DBNull.Value
                            ? string.Empty
                            : drDivision["DepartmentCode"].ToString());
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Save process.
        /// </summary>
        /// <param name="dsUOM"></param>
        /// <returns></returns>
        public bool Save(DataSet dsDivision, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsDivision, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }
    }
}