using System.Data;
using Blue.DAL;

namespace Blue.BL.Option.Inventory
{
    public class Unit : DbHandler
    {
        public Unit()
        {
            SelectCommand = "SELECT * FROM [In].[Unit]";
            TableName = "Unit";
        }

        /// <summary>
        ///     Gets all active unit data related to specified login name.
        /// </summary>
        /// <param name="dsUnit"></param>
        /// <param name="LoginName"></param>
        /// <param name="MsgError"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsUnit, ref string MsgError, string connStr)
        {
            var result = DbRetrieve("dbo.Unit_GetActiveList", dsUnit, null, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        public bool GetList(DataSet dsUnit, string connStr)
        {
            return DbRetrieve("dbo.Unit_GetActiveList", dsUnit, null, TableName, connStr);
        }

        /// <summary>
        ///     Get Lookup UnitCode.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            // Create parameters
            return DbRead("dbo.Unit_GetActiveList", null, connStr);
        }

        public DataSet Get(DataSet dsUnit, string connStr)
        {
            var MsgErr = string.Empty;

            var result = GetList(dsUnit, ref MsgErr, connStr);

            if (!result)
            {
                MsgErr = "Msg001";
                return null;
            }
            return dsUnit;
        }

        /// <summary>
        ///     Get Max UnitCode.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewID(string connStr)
        {
            var NewID = DbReadScalar("dbo.Unit_GetNewID", null, connStr);

            // Return result
            return NewID;
        }

        /// <summary>
        ///     Get Name UnitCode.
        /// </summary>
        /// <param name="unitCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string unitCode, string connStr)
        {
            var strName = string.Empty;

            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@UnitCode", unitCode);

            var drUnit = DbRead("dbo.Unit_GetName", dbParams, connStr);

            if (drUnit.Rows.Count > 0)
            {
                strName = drUnit.Rows[0]["Name"].ToString();
            }

            return strName;
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetUnitLookup(string connStr)
        {
            var dsUnit = new DataSet();

            // Get Data
            DbRetrieve("dbo.Unit_GetActiveList", dsUnit, null, TableName, connStr);

            // Return result
            return dsUnit.Tables[TableName];
        }

        /// <summary>
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetListByRowFilter(string filter, int startIndex, int endIndex, string connStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@filter", filter);
            dbParams[1] = new DbParameter("@startIndex", startIndex.ToString());
            dbParams[2] = new DbParameter("@endIndex", endIndex.ToString());

            // Create parameters
            return DbRead("dbo.Unit_GetActiveListByRowFilter", dbParams, connStr);
        }

        /// <summary>
        ///     Get structure
        /// </summary>
        /// <param name="dsProduct"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsUnit, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("dbo.Unit_GetSchema", dsUnit, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Commit user changed to database.
        /// </summary>
        /// <param name="dsPrefix"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsUnit, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsUnit, SelectCommand, TableName);
            return DbCommit(dbSaveSource, connStr);
        }

        public int Get_CountByUnitCode(string Code, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Code", Code);

            return DbReadScalar("[IN].[Unit_CountByCode]", dbParams, ConnStr);
        }

        public bool Get(DataSet dsUnit, string UnitCode, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@UniCode", UnitCode);

            return DbRetrieve("[IN].[GetUnit_UnitCode]", dsUnit, dbParams, TableName, ConnStr);
        }

        public bool IsExist(string UnitCode, string ConnStr)
        {
            var dsUnit = new DataSet();

            var result = Get(dsUnit, UnitCode, ConnStr);

            if (result)
            {
                if (dsUnit.Tables[TableName].Rows.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}