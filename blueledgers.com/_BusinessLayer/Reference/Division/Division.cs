using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class Division : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public Division()
        {
            SelectCommand = "SELECT * FROM Reference.Division";
            TableName = "Division";
        }

        /// <summary>
        ///     Get name division
        /// </summary>
        /// <param name="DepCode"></param>
        /// <param name="DivisionCode"></param>
        /// <returns></returns>
        public string GetName(string DivisionCode, string connStr)
        {
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DivisionCode", DivisionCode);

            DbRetrieve("GL.GetDivision", dsTmp, dbParams, TableName, connStr);
            return dsTmp.Tables[TableName].Rows[0]["Description"].ToString();
        }

        /// <summary>
        ///     Get all data in table
        /// </summary>
        /// <returns></returns>
        public DataTable GetDivisionList(string connStr)
        {
            var dsDivision = new DataSet();

            // Get data
            var division = new Division();


            division.DbRetrieve("Reference.GetDivisionList", dsDivision, null, TableName, connStr);

            // Return result
            if (dsDivision.Tables[TableName] != null)
            {
                var drBlank = dsDivision.Tables[TableName].NewRow();
                dsDivision.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsDivision.Tables[TableName];
        }

        /// <summary>
        ///     Get only data from param
        /// </summary>
        /// <returns></returns>
        public DataTable GetDivisionLookUp(string connStr)
        {
            var dsDivision = new DataSet();

            // Get data
            var division = new Division();

            division.DbRetrieve("Reference.GetDivisionList", dsDivision, null, TableName, connStr);

            // Return result
            if (dsDivision.Tables[TableName] != null)
            {
                var drBlank = dsDivision.Tables[TableName].NewRow();
                dsDivision.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsDivision.Tables[TableName];
        }

        /// <summary>
        ///     Lookup division by department code
        /// </summary>
        /// <returns></returns>
        public DataTable GetDivisionLookUp(string DepartmentCode, string connStr)
        {
            var dsDivision = new DataSet();

            // Get data
            var division = new Division();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DepartmentCode", DepartmentCode);

            division.DbRetrieve("Reference.GetDivisionListByDepartmentCode", dsDivision, dbParams, TableName, connStr);

            // Return result
            if (dsDivision.Tables[TableName] != null)
            {
                var drBlank = dsDivision.Tables[TableName].NewRow();
                dsDivision.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsDivision.Tables[TableName];
        }

        /// <summary>
        ///     Get active division related to sepecified department code
        /// </summary>
        /// <param name="strDepCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public DataTable GetActiveList(string strDepCode, string strConn)
        {
            var dsDiv = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DepartmentCode", strDepCode);

            var result = DbRetrieve("Reference.GetDivisionActiveList_DepartmentCode", dsDiv, dbParams, TableName,
                strConn);

            if (result)
            {
                return dsDiv.Tables[TableName];
            }
            return null;
        }

        /// <summary>
        ///     Get table division by department code
        /// </summary>
        /// <param name="divisionCode"></param>
        /// <param name="dsDepartment"></param>
        /// <returns></returns>
        public DataSet GetDivisionListByDepartmentCode(string deptCode, DataSet dsDivision, string connStr)
        {
            var dbParams = new DbParameter[1];

            dbParams[0] = new DbParameter("@DepartmentCode", deptCode);

            DbRetrieve("Reference.GetDivisionListByDepartmentCode", dsDivision, dbParams, TableName, connStr);

            return dsDivision;
        }

        /// <summary>
        ///     Get table division by department code
        /// </summary>
        /// <param name="divisionCode"></param>
        /// <param name="dsDepartment"></param>
        /// <returns></returns>
        public DataSet GetDivisionListByDepartmentCode(string deptCode, string connStr)
        {
            var dsDivision = new DataSet();

            var dbParams = new DbParameter[1];

            dbParams[0] = new DbParameter("@DepartmentCode", deptCode);


            DbRetrieve("Reference.GetDivisionListByDepartmentCode", dsDivision, dbParams, TableName, connStr);

            return dsDivision;
        }

        public DataTable GetDivisionListByDepCode(string deptCode, string strConn)
        {
            var dsDivision = new DataSet();

            var dbParams = new DbParameter[1];

            dbParams[0] = new DbParameter("@DepartmentCode", deptCode);

            var result = DbRetrieve("Reference.GetDivisionListByDepartmentCode", dsDivision, dbParams, TableName,
                strConn);

            if (result)
            {
                return dsDivision.Tables[TableName];
            }
            return null;
        }

        /// <summary>
        ///     Get all table division
        /// </summary>
        /// <param name="dsUOM"></param>
        /// <returns></returns>
        public bool GetDivisionList(DataSet dsDivision, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Reference.GetDivisionList", dsDivision, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get max division Code.
        /// </summary>
        /// <returns></returns>
        public int GetDivisionCodeMax(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("Reference.GetDivisionCodeMax", null, connStr);

            // Return result
            return result;
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

        #endregion
    }
}