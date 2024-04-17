using System.Data;
using Blue.DAL;

namespace Blue.BL.Option.Admin.Security
{
    public class Department : DbHandler
    {
        public Department()
        {
            SelectCommand = "SELECT * FROM [ADMIN].[Department]";
            TableName = "Department";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsDept"></param>
        /// <param name="MsgError"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsDept, ref string MsgError, string connStr)
        {
            var result = DbRetrieve("dbo.Dept_GetActiveList", dsDept, null, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsDeliPoint"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsDept, string connStr)
        {
            return DbRetrieve("dbo.IN_StoreDept_GetList", dsDept, null, TableName, connStr);
        }

        /// <summary>
        ///     Get List for lookup
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            // Create parameters
            return DbRead("dbo.Dept_GetActiveList", null, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewID(string connStr)
        {
            var NewID = DbReadScalar("dbo.Dept_GetNewID", null, connStr);

            // Return result
            return NewID;
        }

        /// <summary>
        ///     Get Name of Delivery Point
        /// </summary>
        /// <param name="dptCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string dptCode, string connStr)
        {
            var strName = string.Empty;

            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DptCode", dptCode);

            var drDeptNm = DbRead("dbo.Dept_GetName", dbParams, connStr);

            if (drDeptNm.Rows.Count > 0)
            {
                strName = drDeptNm.Rows[0]["Name"].ToString();
            }

            return strName;
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetLookup(string connStr)
        {
            var dsDept = new DataSet();

            // Get Data
            GetList(dsDept, connStr);

            // Return result
            return dsDept.Tables[TableName];
        }

        /// <summary>
        ///     Commit user changed to database.
        /// </summary>
        /// <param name="dsPrefix"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsDept, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsDept, SelectCommand, TableName);
            return DbCommit(dbSaveSource, connStr);
        }
    }
}